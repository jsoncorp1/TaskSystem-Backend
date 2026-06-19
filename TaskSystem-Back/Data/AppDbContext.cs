using Microsoft.EntityFrameworkCore;
using TaskSystem_Back.Models;

namespace TaskSystem_Back.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Aquí irán tus tablas más adelante, por ejemplo:
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<ProjectType> ProjectTypes => Set<ProjectType>();
    public DbSet<ClientCompany> ClientCompanies => Set<ClientCompany>();
    public DbSet<User> Users => Set<User>();
    public DbSet<DepartmentUser> DepartmentUsers => Set<DepartmentUser>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<SubProject> SubProjects => Set<SubProject>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<SubTask> SubTasks => Set<SubTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DepartmentUser>()
            .HasIndex(du => new { du.UserId, du.DepartmentId })
            .IsUnique();

        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111101"), Name = "Cliente",
                Description = "Usuario externo que representa a una empresa cliente.", CreatedAt = seedDate
            },
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111102"), Name = "Gerente",
                Description = "Responsable de la gestión general y toma de decisiones.", CreatedAt = seedDate
            },
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111103"), Name = "Director",
                Description = "Lidera un área o departamento de la empresa.", CreatedAt = seedDate
            },
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111104"), Name = "Operativo 1",
                Description = "Personal operativo con mayor nivel de experiencia.", CreatedAt = seedDate
            },
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111105"), Name = "Operativo 2",
                Description = "Personal operativo de nivel inicial.", CreatedAt = seedDate
            },
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111106"), Name = "Pasante",
                Description = "Estudiante en formación práctica dentro de la empresa.", CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222201"), Name = "ATL",
                Description = "Medios tradicionales: TV, radio, prensa y vía pública.", CreatedAt = seedDate
            },
            new Department
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222202"), Name = "Digital",
                Description = "Estrategia y ejecución de campañas en medios digitales.", CreatedAt = seedDate
            },
            new Department
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222203"), Name = "Diseño",
                Description = "Diseño gráfico y producción visual de piezas.", CreatedAt = seedDate
            },
            new Department
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222204"), Name = "Creatividad",
                Description = "Conceptualización creativa de campañas y contenidos.", CreatedAt = seedDate
            },
            new Department
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222205"), Name = "Content",
                Description = "Generación de contenido para redes y plataformas digitales.", CreatedAt = seedDate
            },
            new Department
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222206"), Name = "Ejecutivo de cuenta",
                Description = "Gestión y seguimiento directo de las cuentas de clientes.", CreatedAt = seedDate
            },
            new Department 
            { 
                Id = Guid.Parse("22222222-2222-2222-2222-222222222207"), Name = "Gerencia",
                Description = "Dirección y gestión general de la empresa.", CreatedAt = seedDate 
            }
        );

        modelBuilder.Entity<ClientCompany>().HasData(
            new ClientCompany
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333301"), Name = "Honor",
                Description = "Empresa cliente del sector tecnología y dispositivos móviles.", CreatedAt = seedDate
            },
            new ClientCompany
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333302"), Name = "Adidas",
                Description = "Empresa cliente del sector deportivo y moda.", CreatedAt = seedDate
            },
            new ClientCompany
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333303"), Name = "Tigo",
                Description = "Empresa cliente del sector telecomunicaciones.", CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<User>().HasData(
            // Gerente
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444401"), FirstName = "Vernica", LastName = "Paz",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111102"), CreatedAt = seedDate
            },

            // Directores
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444402"), FirstName = "Vanet", LastName = "Garcia",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111103"), CreatedAt = seedDate
            },
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444403"), FirstName = "Anto", LastName = "Pomacusi",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111103"), CreatedAt = seedDate
            },

            // Clientes (Role = Cliente + ClientCompanyId)
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444404"), FirstName = "Carlos", LastName = "Apala",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111101"),
                ClientCompanyId = Guid.Parse("33333333-3333-3333-3333-333333333301"), CreatedAt = seedDate
            }, // Honor
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444405"), FirstName = "Harold", LastName = "Apaza",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111101"),
                ClientCompanyId = Guid.Parse("33333333-3333-3333-3333-333333333302"), CreatedAt = seedDate
            }, // Adidas
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444406"), FirstName = "Miguel", LastName = "Calvimontes",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111101"),
                ClientCompanyId = Guid.Parse("33333333-3333-3333-3333-333333333303"), CreatedAt = seedDate
            }, // Tigo

            // Operativos
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444407"), FirstName = "Yoss", LastName = "Quiroga",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111104"), CreatedAt = seedDate
            }, // Operativo 1
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444408"), FirstName = "Belen", LastName = "Rodriguez",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111105"), CreatedAt = seedDate
            }, // Operativo 2
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444409"), FirstName = "Gabriel", LastName = "Aguilar",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111104"), CreatedAt = seedDate
            }, // Operativo 1
            new User
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444410"), FirstName = "Ariane", LastName = "Alvarez",
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111104"), CreatedAt = seedDate
            } // Operativo 1
        );

        modelBuilder.Entity<ProjectType>().HasData(
            new ProjectType
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555501"), Name = "Branding",
                Description = "Proyectos de construcción e identidad de marca.", CreatedAt = seedDate
            },
            new ProjectType
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555502"), Name = "Marketing",
                Description = "Estrategias y campañas de marketing en general.", CreatedAt = seedDate
            },
            new ProjectType
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555503"), Name = "Producción Audiovisual",
                Description = "Producción de video, fotografía y contenido audiovisual.", CreatedAt = seedDate
            },
            new ProjectType
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555504"), Name = "Medios Publicitarios",
                Description = "Planificación y compra de espacios publicitarios.", CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<Project>().HasData(
            new Project
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666601"),
                Title = "Campaña de lanzamiento Honor X10",
                Detail = "Campaña integral de branding y marketing para el lanzamiento del nuevo modelo.",
                StartDate = new DateOnly(2026, 6, 1),
                EndDate = new DateOnly(2026, 8, 31),
                ProjectTypeId = Guid.Parse("55555555-5555-5555-5555-555555555501"), // Branding
                ClientUserId = Guid.Parse("44444444-4444-4444-4444-444444444404"), // Carlos Apala (Honor)
                CreatedAt = seedDate
            },
            new Project
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666602"),
                Title = "Spot publicitario Adidas Running",
                Detail = "Producción audiovisual de spot para campaña de la línea de running.",
                StartDate = new DateOnly(2026, 7, 1),
                EndDate = null,
                ProjectTypeId = Guid.Parse("55555555-5555-5555-5555-555555555503"), // Producción Audiovisual
                ClientUserId = Guid.Parse("44444444-4444-4444-4444-444444444405"), // Harold Apaza (Adidas)
                CreatedAt = seedDate
            },
            new Project
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666603"),
                Title = "Plan de medios Tigo Q3",
                Detail = "Planificación y compra de espacios publicitarios para el tercer trimestre.",
                StartDate = new DateOnly(2026, 7, 15),
                EndDate = new DateOnly(2026, 9, 30),
                ProjectTypeId = Guid.Parse("55555555-5555-5555-5555-555555555504"), // Medios Publicitarios
                ClientUserId = Guid.Parse("44444444-4444-4444-4444-444444444406"), // Miguel Calvimontes (Tigo)
                CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<SubProject>().HasData(
            new SubProject
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777701"),
                Title = "Diseño de piezas gráficas - Lanzamiento Honor X10",
                Detail = "Creación de banners, posts y material gráfico para la campaña.",
                StartDate = new DateOnly(2026, 6, 1),
                EndDate = new DateOnly(2026, 6, 20),
                Status = "En curso",
                ProjectId = Guid.Parse("66666666-6666-6666-6666-666666666601"), // Campaña Honor X10
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222203"), // Diseño
                AssignedUserId = Guid.Parse("44444444-4444-4444-4444-444444444409"), // Gabriel Aguilar
                CreatedAt = seedDate
            },
            new SubProject
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777702"),
                Title = "Guion y storyboard - Spot Adidas Running",
                Detail = "Desarrollo creativo del concepto y guion para el spot audiovisual.",
                StartDate = new DateOnly(2026, 7, 1),
                EndDate = new DateOnly(2026, 7, 10),
                Status = "Pendiente",
                ProjectId = Guid.Parse("66666666-6666-6666-6666-666666666602"), // Spot Adidas
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222204"), // Creatividad
                AssignedUserId = null,
                CreatedAt = seedDate
            },
            new SubProject
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777703"),
                Title = "Compra de espacios en medios - Tigo Q3",
                Detail = "Negociación y reserva de espacios publicitarios para el trimestre.",
                StartDate = new DateOnly(2026, 7, 15),
                EndDate = new DateOnly(2026, 8, 1),
                Status = "Pendiente",
                ProjectId = Guid.Parse("66666666-6666-6666-6666-666666666603"), // Plan de medios Tigo
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222201"), // ATL
                AssignedUserId = Guid.Parse("44444444-4444-4444-4444-444444444407"), // Yoss Quiroga
                CreatedAt = seedDate
            }
        );
        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888801"),
                Title = "Diseñar banner principal para redes",
                Detail = "Banner para Instagram y Facebook con las dimensiones requeridas por campaña.",
                StartDate = new DateOnly(2026, 6, 1),
                EndDate = new DateOnly(2026, 6, 5),
                Status = "En curso",
                SubProjectId = Guid.Parse("77777777-7777-7777-7777-777777777701"), // Diseño de piezas - Honor X10
                AssignedUserId = Guid.Parse("44444444-4444-4444-4444-444444444409"), // Gabriel Aguilar
                CreatedAt = seedDate
            },
            new TaskItem
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888802"),
                Title = "Redactar guion del spot",
                Detail = "Primera versión del guion narrativo para revisión del cliente.",
                StartDate = new DateOnly(2026, 7, 1),
                EndDate = new DateOnly(2026, 7, 4),
                Status = "Pendiente",
                SubProjectId = Guid.Parse("77777777-7777-7777-7777-777777777702"), // Guion - Spot Adidas
                AssignedUserId = null,
                CreatedAt = seedDate
            },
            new TaskItem
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888803"),
                Title = "Cotizar espacios en vía pública",
                Detail = "Solicitar cotizaciones a proveedores de espacios publicitarios.",
                StartDate = new DateOnly(2026, 7, 15),
                EndDate = new DateOnly(2026, 7, 20),
                Status = "Pendiente",
                SubProjectId = Guid.Parse("77777777-7777-7777-7777-777777777703"), // Compra de espacios - Tigo
                AssignedUserId = Guid.Parse("44444444-4444-4444-4444-444444444407"), // Yoss Quiroga
                CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<SubTask>().HasData(
            new SubTask
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999901"),
                Title = "Seleccionar paleta de colores",
                Detail = "Definir colores acorde a la identidad de marca de Honor.",
                StartDate = new DateOnly(2026, 6, 1),
                EndDate = new DateOnly(2026, 6, 2),
                Status = "Completado",
                TaskItemId = Guid.Parse("88888888-8888-8888-8888-888888888801"), // Diseñar banner principal
                AssignedUserId = Guid.Parse("44444444-4444-4444-4444-444444444409"), // Gabriel Aguilar
                CreatedAt = seedDate
            },
            new SubTask
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999902"),
                Title = "Investigar referencias narrativas",
                Detail = "Buscar referencias de spots similares para inspiración del guion.",
                StartDate = new DateOnly(2026, 7, 1),
                EndDate = new DateOnly(2026, 7, 2),
                Status = "Pendiente",
                TaskItemId = Guid.Parse("88888888-8888-8888-8888-888888888802"), // Redactar guion del spot
                AssignedUserId = null,
                CreatedAt = seedDate
            },
            new SubTask
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999903"),
                Title = "Contactar proveedores de vallas",
                Detail = "Armar lista de proveedores y solicitar datos de contacto.",
                StartDate = new DateOnly(2026, 7, 15),
                EndDate = new DateOnly(2026, 7, 16),
                Status = "Pendiente",
                TaskItemId = Guid.Parse("88888888-8888-8888-8888-888888888803"), // Cotizar espacios en vía pública
                AssignedUserId = Guid.Parse("44444444-4444-4444-4444-444444444407"), // Yoss Quiroga
                CreatedAt = seedDate
            }
        );

        modelBuilder.Entity<DepartmentUser>().HasData(
            new DepartmentUser
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa08"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444401"), // Vernica Paz
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222207"), // Gerencia
                Email = "vernica.paz@gmail.com",
                Password = "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy",
                CreatedAt = seedDate
            },
            new DepartmentUser
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444402"), // Vanet García
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222202"), // Digital
                Email = "vanet.garcia@gmail.com",
                Password = "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy",
                CreatedAt = seedDate
            },
            new DepartmentUser
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444403"), // Anto Pomacusi
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222203"), // Diseño
                Email = "anto.pomacusi@gmail.com",
                Password = "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy",
                CreatedAt = seedDate
            },
            new DepartmentUser
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa03"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444403"), // Anto Pomacusi
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222204"), // Creatividad
                Email = "anto.pomacusi@gmail.com",
                Password = "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy",
                CreatedAt = seedDate
            },
            new DepartmentUser
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa04"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444407"), // Yoss Quiroga
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222203"), // Diseño
                Email = "yoss.quiroga@gmail.com",
                Password = "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy",
                CreatedAt = seedDate
            },
            new DepartmentUser
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa05"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444408"), // Belen Rodriguez
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222203"), // Diseño
                Email = "belen.rodriguez@gmail.com",
                Password = "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy",
                CreatedAt = seedDate
            },
            new DepartmentUser
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa06"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444409"), // Gabriel Aguilar
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222206"), // Ejecutivo de cuenta
                Email = "gabriel.aguilar@gmail.com",
                Password = "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy",
                CreatedAt = seedDate
            },
            new DepartmentUser
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa07"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444410"), // Ariane Alvarez
                DepartmentId = Guid.Parse("22222222-2222-2222-2222-222222222201"), // ATL
                Email = "ariane.alvarez@gmail.com",
                Password = "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy",
                CreatedAt = seedDate
            }
        );
    }
}