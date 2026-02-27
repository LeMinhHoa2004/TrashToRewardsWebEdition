using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrashToRewardsV2.Migrations
{
    /// <inheritdoc />
    public partial class v12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PHIEUTHUGOM_PHIEUDATLICH_MaPhieuDL",
                table: "PHIEUTHUGOM");

            migrationBuilder.RenameColumn(
                name: "MaPhieuDL",
                table: "PHIEUTHUGOM",
                newName: "MaXN");

            migrationBuilder.RenameIndex(
                name: "IX_PHIEUTHUGOM_MaPhieuDL",
                table: "PHIEUTHUGOM",
                newName: "IX_PHIEUTHUGOM_MaXN");

            migrationBuilder.CreateTable(
                name: "XACNHAN",
                columns: table => new
                {
                    MaXN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayXN = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNV = table.Column<int>(type: "int", nullable: false),
                    MaPhieuDL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XACNHAN", x => x.MaXN);
                    table.ForeignKey(
                        name: "FK_XACNHAN_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XACNHAN_PHIEUDATLICH_MaPhieuDL",
                        column: x => x.MaPhieuDL,
                        principalTable: "PHIEUDATLICH",
                        principalColumn: "MaPhieuDL",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_XACNHAN_MaNV",
                table: "XACNHAN",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_XACNHAN_MaPhieuDL",
                table: "XACNHAN",
                column: "MaPhieuDL",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PHIEUTHUGOM_XACNHAN_MaXN",
                table: "PHIEUTHUGOM",
                column: "MaXN",
                principalTable: "XACNHAN",
                principalColumn: "MaXN",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PHIEUTHUGOM_XACNHAN_MaXN",
                table: "PHIEUTHUGOM");

            migrationBuilder.DropTable(
                name: "XACNHAN");

            migrationBuilder.RenameColumn(
                name: "MaXN",
                table: "PHIEUTHUGOM",
                newName: "MaPhieuDL");

            migrationBuilder.RenameIndex(
                name: "IX_PHIEUTHUGOM_MaXN",
                table: "PHIEUTHUGOM",
                newName: "IX_PHIEUTHUGOM_MaPhieuDL");

            migrationBuilder.AddForeignKey(
                name: "FK_PHIEUTHUGOM_PHIEUDATLICH_MaPhieuDL",
                table: "PHIEUTHUGOM",
                column: "MaPhieuDL",
                principalTable: "PHIEUDATLICH",
                principalColumn: "MaPhieuDL",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
