using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrashToRewardsV2.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaDiemNhanQua",
                table: "PHIEUDOIQUA",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhThucNhanQua",
                table: "PHIEUDOIQUA",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiaDiemDL",
                table: "PHIEUDATLICH",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaDiemNhanQua",
                table: "PHIEUDOIQUA");

            migrationBuilder.DropColumn(
                name: "HinhThucNhanQua",
                table: "PHIEUDOIQUA");

            migrationBuilder.DropColumn(
                name: "DiaDiemDL",
                table: "PHIEUDATLICH");
        }
    }
}
