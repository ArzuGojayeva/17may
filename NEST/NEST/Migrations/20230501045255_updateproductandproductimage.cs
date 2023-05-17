using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEST.Migrations
{
    public partial class updateproductandproductimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsMain",
                table: "productsImage",
                newName: "IsFront");

            migrationBuilder.AddColumn<bool>(
                name: "IsBack",
                table: "productsImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StockCount",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBack",
                table: "productsImage");

            migrationBuilder.DropColumn(
                name: "StockCount",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "IsFront",
                table: "productsImage",
                newName: "IsMain");
        }
    }
}
