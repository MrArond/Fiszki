using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class _12356 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FlashCardsLists",
                newName: "CardsListID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FlashCardsLists",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FlashCardsLists",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "FlashCardsLists");

            migrationBuilder.RenameColumn(
                name: "CardsListID",
                table: "FlashCardsLists",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "FlashCardsLists",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
