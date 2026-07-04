# TaskSystem-Backend — Documentación de API para el Frontend

> Generado a partir del código fuente en `TaskSystem-Back/` (ASP.NET Core Web API + EF Core + PostgreSQL/Neon).
> Fecha de análisis: 2026-07-03.

---

## 0. Información general

| Item | Valor |
|---|---|
| Framework | ASP.NET Core (Controllers, no Minimal API) |
| Base de datos | PostgreSQL (Neon), vía EF Core (`Npgsql`) |
| Auth | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| URL local (http) | `http://localhost:5186` |
| URL local (https) | `https://localhost:7071` |
| Swagger | Habilitado solo en `Development`, en `/swagger` |
| Prefijo de rutas | `api/...` (todas las rutas empiezan con `api/`) |
| Formato | JSON (`camelCase` no está forzado explícitamente — ver nota en sección 1.4) |

### ✅ CORS configurado (política abierta)

`Program.cs` ya tiene una política de CORS llamada `"AllowAll"` que permite **cualquier origen, método y header** (`AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()`), aplicada globalmente vía `app.UseCors("AllowAll")`. Esto significa que el frontend puede consumir la API desde cualquier dominio/puerto sin configuración adicional en el backend.

**Nota de seguridad:** al usar `AllowAnyOrigin()`, no se puede combinar con `AllowCredentials()` (restricción del propio protocolo CORS) — no es un problema aquí porque la autenticación va por header `Authorization: Bearer <token>`, no por cookies. Si en el futuro pasan a producción con datos sensibles, se recomienda restringir esta política a los orígenes reales del frontend en vez de dejarla abierta a cualquiera.

---

## 1. Autenticación

### 1.1 Esquema

- Tipo: **JWT Bearer**.
- Header: `Authorization: Bearer <token>`.
- Emisor (`Issuer`) / Audiencia (`Audience`): `TaskSystem` (ambos, configurados en `appsettings.json`).
- Expiración del token: **8 horas** desde el login (`DateTime.UtcNow.AddHours(8)`).
- Todos los controllers tienen `[Authorize]` a nivel de clase **excepto** el endpoint de login, que tiene `[AllowAnonymous]`.
- No hay endpoint de refresh token. Cuando expira, el usuario debe volver a hacer login.

### 1.2 Claims incluidos en el token

```
NameIdentifier -> user.Id (Guid como string)
Email          -> user.Email
Role           -> user.Role.Name (ej: "Gerente", "Director", "Cliente", "Operativo 1", "Operativo 2", "Pasante")
departments    -> string único con nombres de departamento separados por coma (ej: "Diseño,Creatividad"). Puede venir vacío "".
```

El frontend puede decodificar el JWT (sin verificar firma) para leer estos claims si necesita mostrar rol/departamentos sin volver a pedir `/api/users/{id}`.

### 1.3 Login

**POST** `/api/users/login` — **sin autenticación** (`AllowAnonymous`)

Request body (`LoginDto`):
```json
{
  "email": "vernica.paz@gmail.com",
  "password": "texto-plano-que-se-compara-con-bcrypt"
}
```
- `email`: string, requerido.
- `password`: string, requerido.

Response `200 OK` (`LoginResponseDto`):
```json
{
  "token": "eyJhbGciOi...",
  "id": "44444444-4444-4444-4444-444444444401",
  "firstName": "Vernica",
  "lastName": "Paz",
  "email": "vernica.paz@gmail.com",
  "roleName": "Gerente",
  "departments": ["Gerencia"]
}
```

Response `401 Unauthorized` (credenciales incorrectas, usuario no existe, o usuario sin password — p.ej. usuarios "Cliente" sembrados sin password):
```json
{ "message": "Credenciales incorrectas." }
```

**Notas importantes para el front:**
- Los usuarios con rol **"Cliente"** en el seed data **no tienen password** (`Password = null`) → **no pueden hacer login**. Son registros de referencia (para asociarlos a proyectos como `ClientUserId`), no cuentas de acceso al sistema.
- Las contraseñas se hashean con **BCrypt** (`BCrypt.Net.BCrypt`).

### 1.4 Convención de nombres JSON

Los DTOs de C# están en `PascalCase` (ej. `FirstName`, `RoleId`). ASP.NET Core con `System.Text.Json` por defecto en .NET serializa **tal cual sin transformar a camelCase salvo que el proyecto lo configure explícitamente**. No se encontró configuración de `JsonOptions` (`PropertyNamingPolicy`) en `Program.cs`. **Recomendación:** antes de asumir camelCase en el front, verificar la respuesta real de Swagger o de un request de prueba (`GET /api/roles`), ya que puede llegar en `PascalCase` (ej. `Id`, `Name`) en vez de `id`, `name`. Ajustar el cliente HTTP del front (o el mapeo de tipos) según lo que se observe.

---

## 2. Convenciones generales de la API

### 2.1 Paginación

Los endpoints de listado (`GET` de colección) devuelven un objeto envolvente `PagedResultDto<T>`:

```json
{
  "totalItems": 42,
  "page": 1,
  "pageSize": 10,
  "items": [ /* array de T */ ]
}
```

Query params comunes a **todos** los listados:
- `page` (int, default `1`)
- `pageSize` (int, default `10`)

No hay tope máximo de `pageSize` validado en el backend — el front debe autolimitarse (evitar mandar `pageSize` gigantes).

### 2.2 Soft delete

Todas las entidades tienen borrado lógico (`DeletedAt`). Un `DELETE` marca `DeletedAt = UtcNow` y devuelve `204 No Content`. Los listados y `GetById` **siempre filtran `DeletedAt == null`**, por lo que un recurso "eliminado" deja de ser visible en todos los endpoints (como si no existiera). No hay endpoint de "restaurar".

### 2.3 Códigos de estado usados

| Código | Cuándo |
|---|---|
| `200 OK` | GET exitoso, PUT exitoso, POST de login exitoso |
| `201 Created` | POST exitoso (incluye header `Location` apuntando a `GetById`) |
| `204 No Content` | DELETE exitoso |
| `400 Bad Request` | Falla de validación de modelo (`[Required]` no cumplido, tipos inválidos) — formato estándar `ValidationProblemDetails` de ASP.NET Core |
| `401 Unauthorized` | Token ausente/inválido/expirado, o login con credenciales incorrectas |
| `404 Not Found` | GetById/Update/Delete sobre un id que no existe (o fue soft-deleted) — body vacío |
| `409 Conflict` | Solo en `POST /api/department-users` cuando ya existe la combinación usuario+departamento |

**Importante:** Los controladores **no** capturan errores de integridad referencial (FK inválida, ej. mandar un `roleId` que no existe). Eso puede resultar en una excepción no controlada de EF Core / PostgreSQL, que probablemente se traduzca en un `500 Internal Server Error` sin cuerpo estructurado. El front debería validar que los IDs de FK (roleId, departmentId, projectTypeId, etc.) existan antes de enviarlos, idealmente poblando selects desde los propios endpoints de catálogo (`/api/roles`, `/api/departments`, etc.).

### 2.4 Tipos de datos especiales

- **Guid**: string en formato UUID (ej. `"44444444-4444-4444-4444-444444444401"`).
- **DateOnly**: se serializa como string `"YYYY-MM-DD"` (sin hora). Usado en `StartDate` / `EndDate` de Project, SubProject, TaskItem, SubTask.
- **Status** (SubProject, TaskItem, SubTask): es un `string` **libre**, no un enum validado por el backend. Los valores usados en el seed / valor por defecto son:
  - `"Pendiente"` (default en Create)
  - `"En curso"`
  - `"Completado"`
  
  El front debería tratar esto como un enum cerrado por convención de UI (ej. un `<select>`), pero el backend no lo valida — cualquier string pasa.

---

## 3. Catálogos "seed" (valores existentes en la base de datos actual)

Estos datos ya existen en la BD (vía `HasData` en `AppDbContext`). Útil para que el front sepa qué esperar al hacer los primeros GETs, y para pruebas manuales.

**Roles** (`GET /api/roles`):
| Id | Name |
|---|---|
| `11111111-...101` | Cliente |
| `11111111-...102` | Gerente |
| `11111111-...103` | Director |
| `11111111-...104` | Operativo 1 |
| `11111111-...105` | Operativo 2 |
| `11111111-...106` | Pasante |

**Departments** (`GET /api/departments`): ATL, Digital, Diseño, Creatividad, Content, Ejecutivo de cuenta, Gerencia.

**ClientCompanies** (`GET /api/client-companies`): Honor, Adidas, Tigo.

**ProjectTypes** (`GET /api/project-types`): Branding, Marketing, Producción Audiovisual, Medios Publicitarios.

**Usuario de prueba para login** (tiene password real):
```
email: vernica.paz@gmail.com
rol: Gerente
```
(Y también `vanet.garcia@gmail.com`, `anto.pomacusi@gmail.com`, `yoss.quiroga@gmail.com`, `belen.rodriguez@gmail.com`, `gabriel.aguilar@gmail.com`, `ariane.alvarez@gmail.com` — todos comparten el mismo hash bcrypt en el seed, pero **no se conoce la contraseña en texto plano** desde el código; hay que pedirla a quien gestiona la BD o resetearla.)

---

## 4. Modelo de relaciones (jerarquía del dominio)

```
Role ──┐
       ├─< User >──── ClientCompany
Department ──< DepartmentUser >── User   (tabla puente, único por (UserId, DepartmentId))

ProjectType ──┐
              ├─< Project >── User (ClientUserId, opcional)
              │
              └─< SubProject >── Department
                       │      └── User (AssignedUserId, opcional)
                       │
                       └─< TaskItem >── User (AssignedUserId, opcional)
                                │
                                └─< SubTask >── User (AssignedUserId, opcional)
```

Jerarquía de trabajo: **Project → SubProject → TaskItem → SubTask** (4 niveles). Cada nivel tiene: `Title`, `Detail?`, `StartDate`, `EndDate?`, y desde `SubProject` hacia abajo también `Status` y `AssignedUserId?`.

---

## 5. Endpoints por módulo

### 5.1 Users — `/api/users`

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/api/users` | Sí | Lista paginada de todos los usuarios |
| GET | `/api/users/clients` | Sí | Lista paginada, filtrada solo a usuarios con rol "Cliente" |
| GET | `/api/users/{id}` | Sí | Detalle de un usuario |
| POST | `/api/users` | Sí | Crear usuario |
| PUT | `/api/users/{id}` | Sí | Actualizar usuario |
| DELETE | `/api/users/{id}` | Sí | Soft delete |
| POST | `/api/users/login` | **No** (`AllowAnonymous`) | Login, devuelve JWT |

**GET `/api/users`** — query params:
- `page`, `pageSize`
- `nombre` (string, contains, case-insensitive sobre `FirstName`)
- `apellido` (string, contains sobre `LastName`)
- `email` (string, contains sobre `Email`)
- `roleId` (Guid, filtro exacto)
- `clientCompanyId` (Guid, filtro exacto)

**GET `/api/users/clients`** — query params: `page`, `pageSize`, `nombre`, `apellido`, `clientCompanyId` (sin `email` ni `roleId`, porque el rol ya está fijado a "Cliente" internamente).

**UserDto** (respuesta):
```json
{
  "id": "guid",
  "firstName": "string",
  "lastName": "string",
  "phone": "string|null",
  "email": "string|null",
  "roleId": "guid",
  "roleName": "string",
  "clientCompanyId": "guid|null",
  "clientCompanyName": "string|null"
}
```

**CreateUserDto / UpdateUserDto** (body):
```json
{
  "firstName": "string (requerido)",
  "lastName": "string (requerido)",
  "phone": "string|null",
  "email": "string|null",
  "password": "string|null",
  "roleId": "guid (requerido)",
  "clientCompanyId": "guid|null"
}
```
- Si `password` viene con valor, se hashea con BCrypt. En `Update`, si `password` viene vacío/null, **no se modifica** la contraseña actual.
- `email`/`password` no son requeridos a nivel de validación — un usuario puede crearse sin credenciales de login (como los "Cliente" del seed).

**LoginDto** (body de `/login`): ver sección 1.3.

---

### 5.2 Roles — `/api/roles`

CRUD estándar. Query en GET: `page`, `pageSize`, `nombre` (contains sobre `Name`).

**RoleDto**:
```json
{ "id": "guid", "name": "string", "description": "string|null" }
```

**CreateRoleDto / UpdateRoleDto**:
```json
{ "name": "string (requerido)", "description": "string|null" }
```

---

### 5.3 Departments — `/api/departments`

CRUD estándar. Query en GET: `page`, `pageSize`, `nombre` (contains sobre `Name`).

**DepartmentDto**:
```json
{ "id": "guid", "name": "string", "description": "string|null" }
```

**CreateDepartmentDto / UpdateDepartmentDto**:
```json
{ "name": "string (requerido)", "description": "string|null" }
```

---

### 5.4 DepartmentUsers (asignación de usuarios a departamentos) — `/api/department-users`

Tabla puente muchos-a-muchos entre `User` y `Department`, con **restricción de unicidad** `(UserId, DepartmentId)`.

| Método | Ruta | Notas |
|---|---|---|
| GET | `/api/department-users` | Filtros: `page`, `pageSize`, `userId`, `departmentId` |
| GET | `/api/department-users/{id}` | — |
| POST | `/api/department-users` | Devuelve `409 Conflict` si el usuario ya está en ese departamento |
| PUT | `/api/department-users/{id}` | Reemplaza `userId`/`departmentId` de la asignación |
| DELETE | `/api/department-users/{id}` | Soft delete |

**DepartmentUserDto**:
```json
{
  "id": "guid",
  "userId": "guid",
  "userFirstName": "string",
  "userLastName": "string",
  "departmentId": "guid",
  "departmentName": "string"
}
```

**CreateDepartmentUserDto / UpdateDepartmentUserDto**:
```json
{ "userId": "guid (requerido)", "departmentId": "guid (requerido)" }
```

Respuesta `409` en creación duplicada:
```json
{ "message": "El usuario ya pertenece a ese departamento." }
```

---

### 5.5 ClientCompanies — `/api/client-companies`

CRUD estándar. Query en GET: `page`, `pageSize`, `nombre` (contains sobre `Name`), `email` (contains sobre `Email`).

**ClientCompanyDto**:
```json
{ "id": "guid", "name": "string", "description": "string|null", "email": "string|null", "phone": "string|null" }
```

**CreateClientCompanyDto / UpdateClientCompanyDto**:
```json
{ "name": "string (requerido)", "description": "string|null", "email": "string|null", "phone": "string|null" }
```

---

### 5.6 ProjectTypes — `/api/project-types`

CRUD estándar. Query en GET: `page`, `pageSize`, `nombre` (contains sobre `Name`).

**ProjectTypeDto** / **Create/UpdateProjectTypeDto**: idéntica forma que Department (`name`, `description`).

---

### 5.7 Projects — `/api/projects`

| Método | Ruta | Notas |
|---|---|---|
| GET | `/api/projects` | Ver filtros abajo |
| GET | `/api/projects/{id}` | Detalle simple |
| GET | `/api/projects/{id}/full` | Detalle + lista de sub-proyectos (resumen) |
| POST | `/api/projects` | Crear |
| PUT | `/api/projects/{id}` | Actualizar |
| DELETE | `/api/projects/{id}` | Soft delete |

**GET `/api/projects`** — query params:
- `page`, `pageSize`
- `titulo` (string, contains sobre `Title`)
- `projectTypeId` (Guid)
- `clientUserId` (Guid)
- `startDateFrom` (DateOnly, `>=`)
- `startDateTo` (DateOnly, `<=`)

**ProjectDto**:
```json
{
  "id": "guid",
  "title": "string",
  "detail": "string|null",
  "startDate": "2026-07-01",
  "endDate": "2026-08-31|null",
  "projectTypeId": "guid",
  "projectTypeName": "string",
  "clientUserId": "guid|null",
  "clientUserFirstName": "string|null",
  "clientUserLastName": "string|null"
}
```

**GET `/api/projects/{id}/full`** → `ProjectFullDto` (todo lo de `ProjectDto` más):
```json
{
  ...campos de ProjectDto,
  "subProjects": [
    {
      "id": "guid",
      "title": "string",
      "detail": "string|null",
      "startDate": "date",
      "endDate": "date|null",
      "status": "string",
      "departmentName": "string",
      "assignedUserFirstName": "string|null",
      "assignedUserLastName": "string|null"
    }
  ]
}
```
Nota: `SubProjectSummaryDto` **no incluye `projectId`** (implícito, ya es hijo del proyecto consultado) ni `assignedUserId` (solo el nombre). Si el front necesita el id del usuario asignado desde esta vista, tendría que ir a `GET /api/subprojects/{id}` individualmente.

**CreateProjectDto / UpdateProjectDto**:
```json
{
  "title": "string (requerido)",
  "detail": "string|null",
  "startDate": "date (requerido)",
  "endDate": "date|null",
  "projectTypeId": "guid (requerido)",
  "clientUserId": "guid|null"
}
```

---

### 5.8 SubProjects — `/api/subprojects`

| Método | Ruta |
|---|---|
| GET | `/api/subprojects` |
| GET | `/api/subprojects/{id}` |
| POST | `/api/subprojects` |
| PUT | `/api/subprojects/{id}` |
| DELETE | `/api/subprojects/{id}` |

**GET** — query params: `page`, `pageSize`, `titulo` (contains), `projectId` (Guid), `departmentId` (Guid), `status` (string — revisar si es exact match o contains; el patrón del código sugiere filtro simple, tratarlo como **igualdad exacta** salvo que se confirme lo contrario en Swagger), `assignedUserId` (Guid).

**SubProjectDto**:
```json
{
  "id": "guid",
  "title": "string",
  "detail": "string|null",
  "startDate": "date",
  "endDate": "date|null",
  "status": "string",
  "projectId": "guid",
  "projectTitle": "string",
  "departmentId": "guid",
  "departmentName": "string",
  "assignedUserId": "guid|null",
  "assignedUserFirstName": "string|null",
  "assignedUserLastName": "string|null"
}
```

**CreateSubProjectDto**:
```json
{
  "title": "string (requerido)",
  "detail": "string|null",
  "startDate": "date (requerido)",
  "endDate": "date|null",
  "status": "string (default: \"Pendiente\" si se omite)",
  "projectId": "guid (requerido)",
  "departmentId": "guid (requerido)",
  "assignedUserId": "guid|null"
}
```

**UpdateSubProjectDto**: igual que Create, pero `status` es **requerido explícitamente** (no tiene default) — hay que mandarlo siempre en un PUT.

---

### 5.9 TaskItems (tareas) — `/api/tasks`

⚠️ Nota de ruta: el controlador se llama `TaskItemsController` pero la ruta base es **`/api/tasks`** (no `/api/task-items` ni `/api/taskitems`).

| Método | Ruta |
|---|---|
| GET | `/api/tasks` |
| GET | `/api/tasks/{id}` |
| POST | `/api/tasks` |
| PUT | `/api/tasks/{id}` |
| DELETE | `/api/tasks/{id}` |

**GET** — query params: `page`, `pageSize`, `titulo` (contains sobre `Title`), `subProjectId` (Guid), `status` (string), `assignedUserId` (Guid).

**TaskItemDto**:
```json
{
  "id": "guid",
  "title": "string",
  "detail": "string|null",
  "startDate": "date",
  "endDate": "date|null",
  "status": "string",
  "subProjectId": "guid",
  "subProjectTitle": "string",
  "assignedUserId": "guid|null",
  "assignedUserFirstName": "string|null",
  "assignedUserLastName": "string|null"
}
```

**CreateTaskItemDto**:
```json
{
  "title": "string (requerido)",
  "detail": "string|null",
  "startDate": "date (requerido)",
  "endDate": "date|null",
  "status": "string (default \"Pendiente\")",
  "subProjectId": "guid (requerido)",
  "assignedUserId": "guid|null"
}
```

**UpdateTaskItemDto**: igual, `status` requerido en el body.

---

### 5.10 SubTasks — `/api/subtasks`

| Método | Ruta |
|---|---|
| GET | `/api/subtasks` |
| GET | `/api/subtasks/{id}` |
| POST | `/api/subtasks` |
| PUT | `/api/subtasks/{id}` |
| DELETE | `/api/subtasks/{id}` |

**GET** — query params: `page`, `pageSize`, `titulo` (contains), `taskItemId` (Guid), `status` (string), `assignedUserId` (Guid).

**SubTaskDto**:
```json
{
  "id": "guid",
  "title": "string",
  "detail": "string|null",
  "startDate": "date",
  "endDate": "date|null",
  "status": "string",
  "taskItemId": "guid",
  "taskItemTitle": "string",
  "assignedUserId": "guid|null",
  "assignedUserFirstName": "string|null",
  "assignedUserLastName": "string|null"
}
```

**CreateSubTaskDto**:
```json
{
  "title": "string (requerido)",
  "detail": "string|null",
  "startDate": "date (requerido)",
  "endDate": "date|null",
  "status": "string (default \"Pendiente\")",
  "taskItemId": "guid (requerido)",
  "assignedUserId": "guid|null"
}
```

**UpdateSubTaskDto**: igual, `status` requerido en el body.

---

## 6. Resumen rápido de rutas (cheatsheet)

```
POST   /api/users/login                    [anon]  → login
GET    /api/users                          [auth]  ?page&pageSize&nombre&apellido&email&roleId&clientCompanyId
GET    /api/users/clients                  [auth]  ?page&pageSize&nombre&apellido&clientCompanyId
GET    /api/users/{id}                     [auth]
POST   /api/users                          [auth]
PUT    /api/users/{id}                     [auth]
DELETE /api/users/{id}                     [auth]

GET    /api/roles                          [auth]  ?page&pageSize&nombre
GET    /api/roles/{id}                     [auth]
POST   /api/roles                          [auth]
PUT    /api/roles/{id}                     [auth]
DELETE /api/roles/{id}                     [auth]

GET    /api/departments                    [auth]  ?page&pageSize&nombre
GET    /api/departments/{id}                [auth]
POST   /api/departments                    [auth]
PUT    /api/departments/{id}               [auth]
DELETE /api/departments/{id}               [auth]

GET    /api/department-users               [auth]  ?page&pageSize&userId&departmentId
GET    /api/department-users/{id}          [auth]
POST   /api/department-users               [auth]  → 409 si duplicado
PUT    /api/department-users/{id}          [auth]
DELETE /api/department-users/{id}          [auth]

GET    /api/client-companies               [auth]  ?page&pageSize&nombre&email
GET    /api/client-companies/{id}          [auth]
POST   /api/client-companies               [auth]
PUT    /api/client-companies/{id}          [auth]
DELETE /api/client-companies/{id}          [auth]

GET    /api/project-types                  [auth]  ?page&pageSize&nombre
GET    /api/project-types/{id}             [auth]
POST   /api/project-types                  [auth]
PUT    /api/project-types/{id}             [auth]
DELETE /api/project-types/{id}             [auth]

GET    /api/projects                       [auth]  ?page&pageSize&titulo&projectTypeId&clientUserId&startDateFrom&startDateTo
GET    /api/projects/{id}                  [auth]
GET    /api/projects/{id}/full             [auth]  → incluye subProjects[]
POST   /api/projects                       [auth]
PUT    /api/projects/{id}                  [auth]
DELETE /api/projects/{id}                  [auth]

GET    /api/subprojects                    [auth]  ?page&pageSize&titulo&projectId&departmentId&status&assignedUserId
GET    /api/subprojects/{id}                [auth]
POST   /api/subprojects                    [auth]
PUT    /api/subprojects/{id}               [auth]
DELETE /api/subprojects/{id}               [auth]

GET    /api/tasks                          [auth]  ?page&pageSize&titulo&subProjectId&status&assignedUserId
GET    /api/tasks/{id}                     [auth]
POST   /api/tasks                          [auth]
PUT    /api/tasks/{id}                     [auth]
DELETE /api/tasks/{id}                     [auth]

GET    /api/subtasks                       [auth]  ?page&pageSize&titulo&taskItemId&status&assignedUserId
GET    /api/subtasks/{id}                  [auth]
POST   /api/subtasks                       [auth]
PUT    /api/subtasks/{id}                  [auth]
DELETE /api/subtasks/{id}                  [auth]
```

## 7. Cosas a decidir/confirmar antes de implementar el front (para pasarle a Claude Code)

1. **CORS no está configurado en el backend.** Bloqueante real — hay que resolverlo (agregar política CORS) antes de poder consumir la API desde un front en otro origen.
2. **Confirmar casing real del JSON** (PascalCase vs camelCase) haciendo un request real o mirando Swagger, ya que no hay configuración explícita de `JsonNamingPolicy`.
3. **No hay endpoint de "usuario actual" (`/api/users/me`)** — el front debe decodificar el JWT o buscar por el `id` devuelto en el login.
4. **No hay refresh token** — al expirar (8h), forzar logout/redirección a login.
5. **`status` es texto libre** sin validación de backend — el front debe fijar los valores permitidos en su propia UI (`Pendiente`, `En curso`, `Completado` son los observados en los datos semilla).
6. **Errores 500 posibles por FK inválidas** en creates/updates (no hay manejo explícito) — conviene que el front solo permita seleccionar IDs desde catálogos ya cargados (dropdowns poblados vía GET), no ingreso libre de GUID.
7. **Filtro `status`** en subprojects/tasks/subtasks: no confirmado si es case-sensitive/contains o exact-match — validar con un request real antes de construir filtros de UI complejos.