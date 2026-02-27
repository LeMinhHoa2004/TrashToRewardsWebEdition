using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrashToRewardsV2.Migrations
{
    /// <inheritdoc />
    public partial class v8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KHOQUAs",
                columns: table => new
                {
                    MaKQ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKQ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiKQ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDTKQ = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHOQUAs", x => x.MaKQ);
                });

            migrationBuilder.CreateTable(
                name: "CTKHOQUAs",
                columns: table => new
                {
                    MaKQ = table.Column<int>(type: "int", nullable: false),
                    MaQua = table.Column<int>(type: "int", nullable: false),
                    SoLuongTrongKho = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTKHOQUAs", x => new { x.MaKQ, x.MaQua });
                    table.ForeignKey(
                        name: "FK_CTKHOQUAs_KHOQUAs_MaKQ",
                        column: x => x.MaKQ,
                        principalTable: "KHOQUAs",
                        principalColumn: "MaKQ",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CTKHOQUAs_PHANTHUONG_MaQua",
                        column: x => x.MaQua,
                        principalTable: "PHANTHUONG",
                        principalColumn: "MaQua",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIENGQs",
                columns: table => new
                {
                    MaNVGQ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTenNVGQ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDTNVGQ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailNVGQ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhauNVGQ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaKQ = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHANVIENGQs", x => x.MaNVGQ);
                    table.ForeignKey(
                        name: "FK_NHANVIENGQs_KHOQUAs_MaKQ",
                        column: x => x.MaKQ,
                        principalTable: "KHOQUAs",
                        principalColumn: "MaKQ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "THONGBAONVGQs",
                columns: table => new
                {
                    MaTBNVGQ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDungNVGQ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTaoNVGQ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaDoc = table.Column<bool>(type: "bit", nullable: false),
                    MaNVGQ = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THONGBAONVGQs", x => x.MaTBNVGQ);
                    table.ForeignKey(
                        name: "FK_THONGBAONVGQs_NHANVIENGQs_MaNVGQ",
                        column: x => x.MaNVGQ,
                        principalTable: "NHANVIENGQs",
                        principalColumn: "MaNVGQ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CTKHOQUAs_MaQua",
                table: "CTKHOQUAs",
                column: "MaQua");

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIENGQs_MaKQ",
                table: "NHANVIENGQs",
                column: "MaKQ");

            migrationBuilder.CreateIndex(
                name: "IX_THONGBAONVGQs_MaNVGQ",
                table: "THONGBAONVGQs",
                column: "MaNVGQ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CTKHOQUAs");

            migrationBuilder.DropTable(
                name: "THONGBAONVGQs");

            migrationBuilder.DropTable(
                name: "NHANVIENGQs");

            migrationBuilder.DropTable(
                name: "KHOQUAs");
        }
    }
}
