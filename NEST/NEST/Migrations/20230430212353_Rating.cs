using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEST.Migrations
{
    public partial class Rating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "productsImage");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "products");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "productsImage",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
