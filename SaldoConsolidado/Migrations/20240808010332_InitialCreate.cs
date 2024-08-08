using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaldoConsolidado.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaldosConsolidados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Saldo = table.Column<decimal>(type: "numeric(15,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaldosConsolidados", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaldosConsolidados");
        }
    }
}
