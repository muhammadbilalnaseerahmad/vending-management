using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppVMManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Sale");

            migrationBuilder.RenameColumn(
                name: "QuantitySold",
                table: "Sale",
                newName: "UnitSold");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Sale",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSale",
                table: "Sale",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sale_ProductId",
                table: "Sale",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Products_ProductId",
                table: "Sale",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Products_ProductId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Sale_ProductId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "TotalSale",
                table: "Sale");

            migrationBuilder.RenameColumn(
                name: "UnitSold",
                table: "Sale",
                newName: "QuantitySold");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Sale",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
