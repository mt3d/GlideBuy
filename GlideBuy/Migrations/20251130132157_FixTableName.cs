using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlideBuy.Migrations
{
    /// <inheritdoc />
    public partial class FixTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UrlRecoreds",
                table: "UrlRecoreds");

            migrationBuilder.RenameTable(
                name: "UrlRecoreds",
                newName: "UrlRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UrlRecords",
                table: "UrlRecords",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UrlRecords",
                table: "UrlRecords");

            migrationBuilder.RenameTable(
                name: "UrlRecords",
                newName: "UrlRecoreds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UrlRecoreds",
                table: "UrlRecoreds",
                column: "Id");
        }
    }
}
