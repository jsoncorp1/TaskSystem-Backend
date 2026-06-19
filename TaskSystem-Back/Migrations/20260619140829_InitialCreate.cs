using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskSystem_Back.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientCompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ClientCompanies_ClientCompanyId",
                        column: x => x.ClientCompanyId,
                        principalTable: "ClientCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentUsers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ProjectTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectTypes_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalTable: "ProjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Users_ClientUserId",
                        column: x => x.ClientUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubProjects_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubProjects_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    SubProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItems_SubProjects_SubProjectId",
                        column: x => x.SubProjectId,
                        principalTable: "SubProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItems_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TaskItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTasks_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubTasks_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ClientCompanies",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "Email", "Name", "Phone", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333301"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Empresa cliente del sector tecnología y dispositivos móviles.", null, "Honor", null, null, null },
                    { new Guid("33333333-3333-3333-3333-333333333302"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Empresa cliente del sector deportivo y moda.", null, "Adidas", null, null, null },
                    { new Guid("33333333-3333-3333-3333-333333333303"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Empresa cliente del sector telecomunicaciones.", null, "Tigo", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222201"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Medios tradicionales: TV, radio, prensa y vía pública.", "ATL", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222202"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Estrategia y ejecución de campañas en medios digitales.", "Digital", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222203"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Diseño gráfico y producción visual de piezas.", "Diseño", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222204"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Conceptualización creativa de campañas y contenidos.", "Creatividad", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222205"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Generación de contenido para redes y plataformas digitales.", "Content", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222206"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Gestión y seguimiento directo de las cuentas de clientes.", "Ejecutivo de cuenta", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222207"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Dirección y gestión general de la empresa.", "Gerencia", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProjectTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555501"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Proyectos de construcción e identidad de marca.", "Branding", null, null },
                    { new Guid("55555555-5555-5555-5555-555555555502"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Estrategias y campañas de marketing en general.", "Marketing", null, null },
                    { new Guid("55555555-5555-5555-5555-555555555503"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Producción de video, fotografía y contenido audiovisual.", "Producción Audiovisual", null, null },
                    { new Guid("55555555-5555-5555-5555-555555555504"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Planificación y compra de espacios publicitarios.", "Medios Publicitarios", null, null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111101"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Usuario externo que representa a una empresa cliente.", "Cliente", null, null },
                    { new Guid("11111111-1111-1111-1111-111111111102"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Responsable de la gestión general y toma de decisiones.", "Gerente", null, null },
                    { new Guid("11111111-1111-1111-1111-111111111103"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Lidera un área o departamento de la empresa.", "Director", null, null },
                    { new Guid("11111111-1111-1111-1111-111111111104"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Personal operativo con mayor nivel de experiencia.", "Operativo 1", null, null },
                    { new Guid("11111111-1111-1111-1111-111111111105"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Personal operativo de nivel inicial.", "Operativo 2", null, null },
                    { new Guid("11111111-1111-1111-1111-111111111106"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Estudiante en formación práctica dentro de la empresa.", "Pasante", null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ClientCompanyId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "FirstName", "LastName", "Phone", "RoleId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444401"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Vernica", "Paz", null, new Guid("11111111-1111-1111-1111-111111111102"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444402"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Vanet", "Garcia", null, new Guid("11111111-1111-1111-1111-111111111103"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444403"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Anto", "Pomacusi", null, new Guid("11111111-1111-1111-1111-111111111103"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444404"), new Guid("33333333-3333-3333-3333-333333333301"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Carlos", "Apala", null, new Guid("11111111-1111-1111-1111-111111111101"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444405"), new Guid("33333333-3333-3333-3333-333333333302"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Harold", "Apaza", null, new Guid("11111111-1111-1111-1111-111111111101"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444406"), new Guid("33333333-3333-3333-3333-333333333303"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Miguel", "Calvimontes", null, new Guid("11111111-1111-1111-1111-111111111101"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444407"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Yoss", "Quiroga", null, new Guid("11111111-1111-1111-1111-111111111104"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444408"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Belen", "Rodriguez", null, new Guid("11111111-1111-1111-1111-111111111105"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444409"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Gabriel", "Aguilar", null, new Guid("11111111-1111-1111-1111-111111111104"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444410"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Ariane", "Alvarez", null, new Guid("11111111-1111-1111-1111-111111111104"), null, null }
                });

            migrationBuilder.InsertData(
                table: "DepartmentUsers",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "DepartmentId", "Email", "Password", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222202"), "vanet.garcia@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy", null, null, new Guid("44444444-4444-4444-4444-444444444402") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222203"), "anto.pomacusi@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy", null, null, new Guid("44444444-4444-4444-4444-444444444403") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa03"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222204"), "anto.pomacusi@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy", null, null, new Guid("44444444-4444-4444-4444-444444444403") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa04"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222203"), "yoss.quiroga@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy", null, null, new Guid("44444444-4444-4444-4444-444444444407") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa05"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222203"), "belen.rodriguez@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy", null, null, new Guid("44444444-4444-4444-4444-444444444408") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa06"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222206"), "gabriel.aguilar@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy", null, null, new Guid("44444444-4444-4444-4444-444444444409") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa07"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222201"), "ariane.alvarez@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy", null, null, new Guid("44444444-4444-4444-4444-444444444410") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa08"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222207"), "vernica.paz@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy", null, null, new Guid("44444444-4444-4444-4444-444444444401") }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ClientUserId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Detail", "EndDate", "ProjectTypeId", "StartDate", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666601"), new Guid("44444444-4444-4444-4444-444444444404"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Campaña integral de branding y marketing para el lanzamiento del nuevo modelo.", new DateOnly(2026, 8, 31), new Guid("55555555-5555-5555-5555-555555555501"), new DateOnly(2026, 6, 1), "Campaña de lanzamiento Honor X10", null, null },
                    { new Guid("66666666-6666-6666-6666-666666666602"), new Guid("44444444-4444-4444-4444-444444444405"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Producción audiovisual de spot para campaña de la línea de running.", null, new Guid("55555555-5555-5555-5555-555555555503"), new DateOnly(2026, 7, 1), "Spot publicitario Adidas Running", null, null },
                    { new Guid("66666666-6666-6666-6666-666666666603"), new Guid("44444444-4444-4444-4444-444444444406"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Planificación y compra de espacios publicitarios para el tercer trimestre.", new DateOnly(2026, 9, 30), new Guid("55555555-5555-5555-5555-555555555504"), new DateOnly(2026, 7, 15), "Plan de medios Tigo Q3", null, null }
                });

            migrationBuilder.InsertData(
                table: "SubProjects",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "DepartmentId", "Detail", "EndDate", "ProjectId", "StartDate", "Status", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777701"), new Guid("44444444-4444-4444-4444-444444444409"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222203"), "Creación de banners, posts y material gráfico para la campaña.", new DateOnly(2026, 6, 20), new Guid("66666666-6666-6666-6666-666666666601"), new DateOnly(2026, 6, 1), "En curso", "Diseño de piezas gráficas - Lanzamiento Honor X10", null, null },
                    { new Guid("77777777-7777-7777-7777-777777777702"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222204"), "Desarrollo creativo del concepto y guion para el spot audiovisual.", new DateOnly(2026, 7, 10), new Guid("66666666-6666-6666-6666-666666666602"), new DateOnly(2026, 7, 1), "Pendiente", "Guion y storyboard - Spot Adidas Running", null, null },
                    { new Guid("77777777-7777-7777-7777-777777777703"), new Guid("44444444-4444-4444-4444-444444444407"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, new Guid("22222222-2222-2222-2222-222222222201"), "Negociación y reserva de espacios publicitarios para el trimestre.", new DateOnly(2026, 8, 1), new Guid("66666666-6666-6666-6666-666666666603"), new DateOnly(2026, 7, 15), "Pendiente", "Compra de espacios en medios - Tigo Q3", null, null }
                });

            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Detail", "EndDate", "StartDate", "Status", "SubProjectId", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888801"), new Guid("44444444-4444-4444-4444-444444444409"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Banner para Instagram y Facebook con las dimensiones requeridas por campaña.", new DateOnly(2026, 6, 5), new DateOnly(2026, 6, 1), "En curso", new Guid("77777777-7777-7777-7777-777777777701"), "Diseñar banner principal para redes", null, null },
                    { new Guid("88888888-8888-8888-8888-888888888802"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Primera versión del guion narrativo para revisión del cliente.", new DateOnly(2026, 7, 4), new DateOnly(2026, 7, 1), "Pendiente", new Guid("77777777-7777-7777-7777-777777777702"), "Redactar guion del spot", null, null },
                    { new Guid("88888888-8888-8888-8888-888888888803"), new Guid("44444444-4444-4444-4444-444444444407"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Solicitar cotizaciones a proveedores de espacios publicitarios.", new DateOnly(2026, 7, 20), new DateOnly(2026, 7, 15), "Pendiente", new Guid("77777777-7777-7777-7777-777777777703"), "Cotizar espacios en vía pública", null, null }
                });

            migrationBuilder.InsertData(
                table: "SubTasks",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Detail", "EndDate", "StartDate", "Status", "TaskItemId", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999901"), new Guid("44444444-4444-4444-4444-444444444409"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Definir colores acorde a la identidad de marca de Honor.", new DateOnly(2026, 6, 2), new DateOnly(2026, 6, 1), "Completado", new Guid("88888888-8888-8888-8888-888888888801"), "Seleccionar paleta de colores", null, null },
                    { new Guid("99999999-9999-9999-9999-999999999902"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Buscar referencias de spots similares para inspiración del guion.", new DateOnly(2026, 7, 2), new DateOnly(2026, 7, 1), "Pendiente", new Guid("88888888-8888-8888-8888-888888888802"), "Investigar referencias narrativas", null, null },
                    { new Guid("99999999-9999-9999-9999-999999999903"), new Guid("44444444-4444-4444-4444-444444444407"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Armar lista de proveedores y solicitar datos de contacto.", new DateOnly(2026, 7, 16), new DateOnly(2026, 7, 15), "Pendiente", new Guid("88888888-8888-8888-8888-888888888803"), "Contactar proveedores de vallas", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentUsers_DepartmentId",
                table: "DepartmentUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentUsers_UserId_DepartmentId",
                table: "DepartmentUsers",
                columns: new[] { "UserId", "DepartmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientUserId",
                table: "Projects",
                column: "ClientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectTypeId",
                table: "Projects",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProjects_AssignedUserId",
                table: "SubProjects",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProjects_DepartmentId",
                table: "SubProjects",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProjects_ProjectId",
                table: "SubProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_AssignedUserId",
                table: "SubTasks",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TaskItemId",
                table: "SubTasks",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_AssignedUserId",
                table: "TaskItems",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_SubProjectId",
                table: "TaskItems",
                column: "SubProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientCompanyId",
                table: "Users",
                column: "ClientCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentUsers");

            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "SubProjects");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClientCompanies");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
