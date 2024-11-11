using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroWaveAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToCharIndicator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharIndicator",
                table: "heating_mode",
                type: "varchar(1)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_heating_mode_CharIndicator",
                table: "heating_mode",
                column: "CharIndicator",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_heating_mode_CharIndicator",
                table: "heating_mode");

            migrationBuilder.DropColumn(
                name: "CharIndicator",
                table: "heating_mode");
        }
    }
}
