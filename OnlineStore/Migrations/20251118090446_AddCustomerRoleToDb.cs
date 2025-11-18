using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlideBuy.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerRoleToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerRole_CustomerRoleId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerRole",
                table: "CustomerRole");

            migrationBuilder.RenameTable(
                name: "CustomerRole",
                newName: "CustomerRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerRoles",
                table: "CustomerRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerRoles_CustomerRoleId",
                table: "Customers",
                column: "CustomerRoleId",
                principalTable: "CustomerRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerRoles_CustomerRoleId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerRoles",
                table: "CustomerRoles");

            migrationBuilder.RenameTable(
                name: "CustomerRoles",
                newName: "CustomerRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerRole",
                table: "CustomerRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerRole_CustomerRoleId",
                table: "Customers",
                column: "CustomerRoleId",
                principalTable: "CustomerRole",
                principalColumn: "Id");
        }
    }
}
