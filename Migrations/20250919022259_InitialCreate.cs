using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionViajes.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destinos",
                columns: table => new
                {
                    DestinoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinos", x => x.DestinoId);
                });

            migrationBuilder.CreateTable(
                name: "Turistas",
                columns: table => new
                {
                    TuristaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turistas", x => x.TuristaId);
                });

            migrationBuilder.InsertData(
                table: "Destinos",
                columns: new[] { "DestinoId", "Costo", "Descripcion", "FechaActualizacion", "FechaCreacion", "Nombre", "Pais" },
                values: new object[,]
                {
                    { 1, 250.00m, "Antigua ciudad inca en la cordillera de los Andes", null, new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3771), "Machu Picchu", "Perú" },
                    { 2, 180.00m, "Icónica estatua de Cristo en Río de Janeiro", null, new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3952), "Cristo Redentor", "Brasil" },
                    { 3, 150.00m, "Majestuosas cascadas en la frontera argentino-brasileña", null, new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3954), "Cataratas del Iguazú", "Argentina" },
                    { 4, 200.00m, "Antigua ciudad maya y una de las nuevas siete maravillas del mundo", null, new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3955), "Chichén Itzá", "México" },
                    { 5, 300.00m, "Parque nacional con impresionantes formaciones rocosas en la Patagonia", null, new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3957), "Torres del Paine", "Chile" }
                });

            migrationBuilder.InsertData(
                table: "Turistas",
                columns: new[] { "TuristaId", "Apellido", "Email", "FechaActualizacion", "FechaRegistro", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Mendoza", "carlos.mendoza@example.com", null, new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(213), "Carlos", "+51 987654321" },
                    { 2, "González", "maria.gonzalez@example.com", null, new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(399), "María", "+54 9 11 1234-5678" },
                    { 3, "Silva", "jose.silva@example.com", null, new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(400), "José", "+55 11 98765-4321" },
                    { 4, "López", "ana.lopez@example.com", null, new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(402), "Ana", "+52 55 1234-5678" },
                    { 5, "Rodríguez", "pedro.rodriguez@example.com", null, new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(403), "Pedro", "+56 9 8765-4321" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinos_Nombre",
                table: "Destinos",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Destinos_Pais",
                table: "Destinos",
                column: "Pais");

            migrationBuilder.CreateIndex(
                name: "IX_Turistas_Apellido",
                table: "Turistas",
                column: "Apellido");

            migrationBuilder.CreateIndex(
                name: "IX_Turistas_Email",
                table: "Turistas",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destinos");

            migrationBuilder.DropTable(
                name: "Turistas");
        }
    }
}
