using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedHotelAndUserEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Hotels");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Hotels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Hotels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ImageId",
                table: "Hotels",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_UserId",
                table: "Hotels",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_AspNetUsers_UserId",
                table: "Hotels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Images_ImageId",
                table: "Hotels",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserId",
                table: "UserProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_AspNetUsers_UserId",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Images_ImageId",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ImageId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_UserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserId",
                table: "UserProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
