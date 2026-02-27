using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrashToRewardsV2.Migrations
{
    /// <inheritdoc />
    public partial class v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CTUUDAIs");

            migrationBuilder.DropTable(
                name: "UUDAIs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UUDAIs",
                columns: table => new
                {
                    MaUuDai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenUuDai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiUD = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UUDAIs", x => x.MaUuDai);
                });

            migrationBuilder.CreateTable(
                name: "CTUUDAIs",
                columns: table => new
                {
                    MaLoaiPL = table.Column<int>(type: "int", nullable: false),
                    MaUuDai = table.Column<int>(type: "int", nullable: false),
                    HeSoTang = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTUUDAIs", x => new { x.MaLoaiPL, x.MaUuDai });
                    table.ForeignKey(
                        name: "FK_CTUUDAIs_LOAIPHELIEU_MaLoaiPL",
                        column: x => x.MaLoaiPL,
                        principalTable: "LOAIPHELIEU",
                        principalColumn: "MaLoaiPL",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CTUUDAIs_UUDAIs_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UUDAIs",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CTUUDAIs_MaUuDai",
                table: "CTUUDAIs",
                column: "MaUuDai");
        }
    }
}
