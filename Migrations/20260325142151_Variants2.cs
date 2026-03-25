using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oliva.Migrations
{
    /// <inheritdoc />
    public partial class Variants2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Color",
                table: "ProductVariants",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProductVariants",
                newName: "Color");
        }
    }
}
