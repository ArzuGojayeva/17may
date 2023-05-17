using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEST.Migrations
{
    public partial class AddNewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "products",
                newName: "SellPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "DisCountPrice",
                table: "products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "products");

            migrationBuilder.DropColumn(
                name: "DisCountPrice",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "categories");

            migrationBuilder.RenameColumn(
                name: "SellPrice",
                table: "products",
                newName: "Price");
        }
    }
}
