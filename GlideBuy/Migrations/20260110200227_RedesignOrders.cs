using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlideBuy.Migrations
{
    /// <inheritdoc />
    public partial class RedesignOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Line2",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Line3",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Orders",
                newName: "ShippingMethod");

            migrationBuilder.RenameColumn(
                name: "Shipped",
                table: "Orders",
                newName: "PickupInStore");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Orders",
                newName: "PaymentMethodSystemName");

            migrationBuilder.RenameColumn(
                name: "Line1",
                table: "Orders",
                newName: "CustomerIp");

            migrationBuilder.RenameColumn(
                name: "GiftWrap",
                table: "Orders",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Orders",
                newName: "CustomerCurrencyCode");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Orders",
                newName: "CustomOrderNumber");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Orders",
                newName: "CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "AllowStoringCreditCardInfo",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BillingAddressId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CardName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderGuid",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "OrderShippingExclTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderShippingInclTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderSubtotalDiscountExclTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderSubtotalDiscountInclTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderSubtotalExclTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderSubtotalInclTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderTotal",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDateUtc",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentMethodAdditionalFeeInclTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentMethodAttditionalFeeExclTax",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PickupAddressId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRates",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AllowStoringCreditCardInfo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderGuid",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderShippingExclTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderShippingInclTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderSubtotalDiscountExclTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderSubtotalDiscountInclTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderSubtotalExclTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderSubtotalInclTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaidDateUtc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodAdditionalFeeInclTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodAttditionalFeeExclTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PickupAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TaxRates",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingMethod",
                table: "Orders",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "PickupInStore",
                table: "Orders",
                newName: "Shipped");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodSystemName",
                table: "Orders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "Orders",
                newName: "GiftWrap");

            migrationBuilder.RenameColumn(
                name: "CustomerIp",
                table: "Orders",
                newName: "Line1");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Orders",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "CustomerCurrencyCode",
                table: "Orders",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "CustomOrderNumber",
                table: "Orders",
                newName: "City");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Line2",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Line3",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderId");

            migrationBuilder.CreateTable(
                name: "ShoppingCartItem",
                columns: table => new
                {
                    ShoppingCartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItem", x => x.ShoppingCartItemId);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_ShoppingCartItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_OrderId",
                table: "ShoppingCartItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_ProductId",
                table: "ShoppingCartItem",
                column: "ProductId");
        }
    }
}
