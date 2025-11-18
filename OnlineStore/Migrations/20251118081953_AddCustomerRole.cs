using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlideBuy.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateOnUtc",
                table: "Customers",
                newName: "CreatedOnUtc");

            migrationBuilder.AddColumn<int>(
                name: "CustomerRoleId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FreeShipping = table.Column<bool>(type: "bit", nullable: false),
                    TaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false),
                    SystemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnablePasswordLifetime = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRole", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BillingAddressId",
                table: "Customers",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerRoleId",
                table: "Customers",
                column: "CustomerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ShippingAddressId",
                table: "Customers",
                column: "ShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Addresses_BillingAddressId",
                table: "Customers",
                column: "BillingAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Addresses_ShippingAddressId",
                table: "Customers",
                column: "ShippingAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerRole_CustomerRoleId",
                table: "Customers",
                column: "CustomerRoleId",
                principalTable: "CustomerRole",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Addresses_BillingAddressId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Addresses_ShippingAddressId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerRole_CustomerRoleId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "CustomerRole");

            migrationBuilder.DropIndex(
                name: "IX_Customers_BillingAddressId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerRoleId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ShippingAddressId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerRoleId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "Customers",
                newName: "CreateOnUtc");
        }
    }
}
