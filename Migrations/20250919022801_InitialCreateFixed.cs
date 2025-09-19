using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionViajes.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 3,
                column: "FechaRegistro",
                value: new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 4,
                column: "FechaRegistro",
                value: new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 5,
                column: "FechaRegistro",
                value: new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3771));

            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3952));

            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3954));

            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 4,
                column: "FechaCreacion",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3955));

            migrationBuilder.UpdateData(
                table: "Destinos",
                keyColumn: "DestinoId",
                keyValue: 5,
                column: "FechaCreacion",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 357, DateTimeKind.Utc).AddTicks(3957));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(399));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 3,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(400));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 4,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(402));

            migrationBuilder.UpdateData(
                table: "Turistas",
                keyColumn: "TuristaId",
                keyValue: 5,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 19, 2, 22, 58, 358, DateTimeKind.Utc).AddTicks(403));
        }
    }
}
