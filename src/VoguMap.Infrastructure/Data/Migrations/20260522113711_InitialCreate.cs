using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VoguMap.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "buildings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    address = table.Column<string>(type: "text", nullable: false, comment: "Полный адрес"),
                    latitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true, comment: "Широта"),
                    longitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true, comment: "Долгота")
                },
                constraints: table =>
                {
                    table.PrimaryKey("buildings_pkey", x => x.id);
                },
                comment: "Учебные корпуса");

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    building_id = table.Column<int>(type: "integer", nullable: false, comment: "Корпус"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Название"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "Описание"),
                    floor = table.Column<int>(type: "integer", nullable: false, comment: "Этаж")
                },
                constraints: table =>
                {
                    table.PrimaryKey("rooms_pkey", x => x.id);
                    table.ForeignKey(
                        name: "rooms_building_id_fkey",
                        column: x => x.building_id,
                        principalTable: "buildings",
                        principalColumn: "id");
                },
                comment: "Помещения");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_building_id",
                table: "rooms",
                column: "building_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "buildings");
        }
    }
}
