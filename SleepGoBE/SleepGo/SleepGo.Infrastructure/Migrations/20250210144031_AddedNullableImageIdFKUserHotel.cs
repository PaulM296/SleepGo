using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedNullableImageIdFKUserHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Images_ImageId",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Images_ImageId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ImageId",
                table: "Hotels");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ImageId",
                table: "Hotels",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Images_ImageId",
                table: "Hotels",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Images_ImageId",
                table: "UserProfiles",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Images_ImageId",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Images_ImageId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ImageId",
                table: "Hotels");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ImageId",
                table: "Hotels",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Images_ImageId",
                table: "Hotels",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Images_ImageId",
                table: "UserProfiles",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
