using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpendWise.Migrations
{
    /// <inheritdoc />
    public partial class ErrorLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificacionesActivas",
                table: "Perfiles");

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mensaje_error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enlace_error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_error = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.AddColumn<bool>(
                name: "NotificacionesActivas",
                table: "Perfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
