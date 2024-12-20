using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoomEntityModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Rooms",
                newName: "RoomNumber");

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomNumber",
                table: "Rooms",
                newName: "Quantity");
        }
    }
}
