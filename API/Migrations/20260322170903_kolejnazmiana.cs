using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class kolejnazmiana : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashCards_FlashCardsLists_FlashCardsListsCardsListID",
                table: "FlashCards");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "FlashCards",
                newName: "Translate");

            migrationBuilder.RenameColumn(
                name: "IdCards",
                table: "FlashCards",
                newName: "CardsId");

            migrationBuilder.AlterColumn<int>(
                name: "FlashCardsListsCardsListID",
                table: "FlashCards",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCards_FlashCardsLists_FlashCardsListsCardsListID",
                table: "FlashCards",
                column: "FlashCardsListsCardsListID",
                principalTable: "FlashCardsLists",
                principalColumn: "CardsListID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashCards_FlashCardsLists_FlashCardsListsCardsListID",
                table: "FlashCards");

            migrationBuilder.RenameColumn(
                name: "Translate",
                table: "FlashCards",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CardsId",
                table: "FlashCards",
                newName: "IdCards");

            migrationBuilder.AlterColumn<int>(
                name: "FlashCardsListsCardsListID",
                table: "FlashCards",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCards_FlashCardsLists_FlashCardsListsCardsListID",
                table: "FlashCards",
                column: "FlashCardsListsCardsListID",
                principalTable: "FlashCardsLists",
                principalColumn: "CardsListID");
        }
    }
}
