using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskSystem_Back.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailPasswordToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "DepartmentUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "DepartmentUsers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444401"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "vernica.paz@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444402"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "vanet.garcia@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444403"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "anto.pomacusi@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444404"),
                columns: new[] { "Email", "Password" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444405"),
                columns: new[] { "Email", "Password" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444406"),
                columns: new[] { "Email", "Password" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444407"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "yoss.quiroga@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444408"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "belen.rodriguez@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444409"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "gabriel.aguilar@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444410"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "ariane.alvarez@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "DepartmentUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "DepartmentUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DepartmentUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "vanet.garcia@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "DepartmentUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "anto.pomacusi@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "DepartmentUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa03"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "anto.pomacusi@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "DepartmentUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa04"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "yoss.quiroga@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "DepartmentUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa05"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "belen.rodriguez@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "DepartmentUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa06"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "gabriel.aguilar@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "DepartmentUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa07"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "ariane.alvarez@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });

            migrationBuilder.UpdateData(
                table: "DepartmentUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa08"),
                columns: new[] { "Email", "Password" },
                values: new object[] { "vernica.paz@gmail.com", "$2b$11$nimYdExq5VFLkLlkcI2/aeCSrdXHIIZ8ytITFH13EMfcIM8LWB4Sy" });
        }
    }
}
