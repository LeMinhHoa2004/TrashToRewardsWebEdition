using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrashToRewardsV2.Migrations
{
    /// <inheritdoc />
    public partial class v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaKQ",
                table: "PHIEUDOIQUA",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUDOIQUA_MaKQ",
                table: "PHIEUDOIQUA",
                column: "MaKQ");

            migrationBuilder.AddForeignKey(
                name: "FK_PHIEUDOIQUA_KHOQUAs_MaKQ",
                table: "PHIEUDOIQUA",
                column: "MaKQ",
                principalTable: "KHOQUAs",
                principalColumn: "MaKQ",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PHIEUDOIQUA_KHOQUAs_MaKQ",
                table: "PHIEUDOIQUA");

            migrationBuilder.DropIndex(
                name: "IX_PHIEUDOIQUA_MaKQ",
                table: "PHIEUDOIQUA");

            migrationBuilder.DropColumn(
                name: "MaKQ",
                table: "PHIEUDOIQUA");
        }
    }
}
