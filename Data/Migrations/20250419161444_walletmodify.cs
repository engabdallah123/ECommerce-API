using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class walletmodify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Registers_UserId",
                table: "Wallets");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Registers_UserId",
                table: "Wallets",
                column: "UserId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Registers_UserId",
                table: "Wallets");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Registers_UserId",
                table: "Wallets",
                column: "UserId",
                principalTable: "Registers",
                principalColumn: "Id");
        }
    }
}
