using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class zmiana : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashCards",
                columns: table => new
                {
                    Cards = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FlashCardsListsCardsListID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCards", x => x.Cards);
                    table.ForeignKey(
                        name: "FK_FlashCards_FlashCardsLists_FlashCardsListsCardsListID",
                        column: x => x.FlashCardsListsCardsListID,
                        principalTable: "FlashCardsLists",
                        principalColumn: "CardsListID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlashCards_FlashCardsListsCardsListID",
                table: "FlashCards",
                column: "FlashCardsListsCardsListID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashCards");
        }
    }
}
