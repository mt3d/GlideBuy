using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlideBuy.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePictureBinaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PictureBinaries_PictureId",
                table: "PictureBinaries");

            migrationBuilder.CreateIndex(
                name: "IX_PictureBinaries_PictureId",
                table: "PictureBinaries",
                column: "PictureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PictureBinaries_PictureId",
                table: "PictureBinaries");

            migrationBuilder.CreateIndex(
                name: "IX_PictureBinaries_PictureId",
                table: "PictureBinaries",
                column: "PictureId",
                unique: true);
        }
    }
}
