using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoguMap.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteForRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "rooms_building_id_fkey",
                table: "rooms");

            migrationBuilder.AddForeignKey(
                name: "rooms_building_id_fkey",
                table: "rooms",
                column: "building_id",
                principalTable: "buildings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "rooms_building_id_fkey",
                table: "rooms");

            migrationBuilder.AddForeignKey(
                name: "rooms_building_id_fkey",
                table: "rooms",
                column: "building_id",
                principalTable: "buildings",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
