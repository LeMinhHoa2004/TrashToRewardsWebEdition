using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrashToRewardsV2.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CTKHOQUAs_KHOQUAs_MaKQ",
                table: "CTKHOQUAs");

            migrationBuilder.DropForeignKey(
                name: "FK_CTKHOQUAs_PHANTHUONG_MaQua",
                table: "CTKHOQUAs");

            migrationBuilder.DropForeignKey(
                name: "FK_NHANVIENGQs_KHOQUAs_MaKQ",
                table: "NHANVIENGQs");

            migrationBuilder.DropForeignKey(
                name: "FK_PHIEUDOIQUA_KHOQUAs_MaKQ",
                table: "PHIEUDOIQUA");

            migrationBuilder.DropForeignKey(
                name: "FK_THONGBAONDs_NGUOIDUNG_MaND",
                table: "THONGBAONDs");

            migrationBuilder.DropForeignKey(
                name: "FK_THONGBAONVGQs_NHANVIENGQs_MaNVGQ",
                table: "THONGBAONVGQs");

            migrationBuilder.DropForeignKey(
                name: "FK_THONGBAONVs_NHANVIEN_MaNV",
                table: "THONGBAONVs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_THONGBAONVs",
                table: "THONGBAONVs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_THONGBAONVGQs",
                table: "THONGBAONVGQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_THONGBAONDs",
                table: "THONGBAONDs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NHANVIENGQs",
                table: "NHANVIENGQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KHOQUAs",
                table: "KHOQUAs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CTKHOQUAs",
                table: "CTKHOQUAs");

            migrationBuilder.RenameTable(
                name: "THONGBAONVs",
                newName: "THONGBAONV");

            migrationBuilder.RenameTable(
                name: "THONGBAONVGQs",
                newName: "THONGBAONVGQ");

            migrationBuilder.RenameTable(
                name: "THONGBAONDs",
                newName: "THONGBAOND");

            migrationBuilder.RenameTable(
                name: "NHANVIENGQs",
                newName: "NHANVIENGQ");

            migrationBuilder.RenameTable(
                name: "KHOQUAs",
                newName: "KHOQUA");

            migrationBuilder.RenameTable(
                name: "CTKHOQUAs",
                newName: "CTKHOQUA");

            migrationBuilder.RenameIndex(
                name: "IX_THONGBAONVs_MaNV",
                table: "THONGBAONV",
                newName: "IX_THONGBAONV_MaNV");

            migrationBuilder.RenameIndex(
                name: "IX_THONGBAONVGQs_MaNVGQ",
                table: "THONGBAONVGQ",
                newName: "IX_THONGBAONVGQ_MaNVGQ");

            migrationBuilder.RenameIndex(
                name: "IX_THONGBAONDs_MaND",
                table: "THONGBAOND",
                newName: "IX_THONGBAOND_MaND");

            migrationBuilder.RenameIndex(
                name: "IX_NHANVIENGQs_MaKQ",
                table: "NHANVIENGQ",
                newName: "IX_NHANVIENGQ_MaKQ");

            migrationBuilder.RenameIndex(
                name: "IX_CTKHOQUAs_MaQua",
                table: "CTKHOQUA",
                newName: "IX_CTKHOQUA_MaQua");

            migrationBuilder.AddPrimaryKey(
                name: "PK_THONGBAONV",
                table: "THONGBAONV",
                column: "MaTBNV");

            migrationBuilder.AddPrimaryKey(
                name: "PK_THONGBAONVGQ",
                table: "THONGBAONVGQ",
                column: "MaTBNVGQ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_THONGBAOND",
                table: "THONGBAOND",
                column: "MaTBND");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NHANVIENGQ",
                table: "NHANVIENGQ",
                column: "MaNVGQ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KHOQUA",
                table: "KHOQUA",
                column: "MaKQ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CTKHOQUA",
                table: "CTKHOQUA",
                columns: new[] { "MaKQ", "MaQua" });

            migrationBuilder.AddForeignKey(
                name: "FK_CTKHOQUA_KHOQUA_MaKQ",
                table: "CTKHOQUA",
                column: "MaKQ",
                principalTable: "KHOQUA",
                principalColumn: "MaKQ",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CTKHOQUA_PHANTHUONG_MaQua",
                table: "CTKHOQUA",
                column: "MaQua",
                principalTable: "PHANTHUONG",
                principalColumn: "MaQua",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NHANVIENGQ_KHOQUA_MaKQ",
                table: "NHANVIENGQ",
                column: "MaKQ",
                principalTable: "KHOQUA",
                principalColumn: "MaKQ",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PHIEUDOIQUA_KHOQUA_MaKQ",
                table: "PHIEUDOIQUA",
                column: "MaKQ",
                principalTable: "KHOQUA",
                principalColumn: "MaKQ",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_THONGBAOND_NGUOIDUNG_MaND",
                table: "THONGBAOND",
                column: "MaND",
                principalTable: "NGUOIDUNG",
                principalColumn: "MaND",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_THONGBAONV_NHANVIEN_MaNV",
                table: "THONGBAONV",
                column: "MaNV",
                principalTable: "NHANVIEN",
                principalColumn: "MaNV",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_THONGBAONVGQ_NHANVIENGQ_MaNVGQ",
                table: "THONGBAONVGQ",
                column: "MaNVGQ",
                principalTable: "NHANVIENGQ",
                principalColumn: "MaNVGQ",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CTKHOQUA_KHOQUA_MaKQ",
                table: "CTKHOQUA");

            migrationBuilder.DropForeignKey(
                name: "FK_CTKHOQUA_PHANTHUONG_MaQua",
                table: "CTKHOQUA");

            migrationBuilder.DropForeignKey(
                name: "FK_NHANVIENGQ_KHOQUA_MaKQ",
                table: "NHANVIENGQ");

            migrationBuilder.DropForeignKey(
                name: "FK_PHIEUDOIQUA_KHOQUA_MaKQ",
                table: "PHIEUDOIQUA");

            migrationBuilder.DropForeignKey(
                name: "FK_THONGBAOND_NGUOIDUNG_MaND",
                table: "THONGBAOND");

            migrationBuilder.DropForeignKey(
                name: "FK_THONGBAONV_NHANVIEN_MaNV",
                table: "THONGBAONV");

            migrationBuilder.DropForeignKey(
                name: "FK_THONGBAONVGQ_NHANVIENGQ_MaNVGQ",
                table: "THONGBAONVGQ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_THONGBAONVGQ",
                table: "THONGBAONVGQ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_THONGBAONV",
                table: "THONGBAONV");

            migrationBuilder.DropPrimaryKey(
                name: "PK_THONGBAOND",
                table: "THONGBAOND");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NHANVIENGQ",
                table: "NHANVIENGQ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KHOQUA",
                table: "KHOQUA");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CTKHOQUA",
                table: "CTKHOQUA");

            migrationBuilder.RenameTable(
                name: "THONGBAONVGQ",
                newName: "THONGBAONVGQs");

            migrationBuilder.RenameTable(
                name: "THONGBAONV",
                newName: "THONGBAONVs");

            migrationBuilder.RenameTable(
                name: "THONGBAOND",
                newName: "THONGBAONDs");

            migrationBuilder.RenameTable(
                name: "NHANVIENGQ",
                newName: "NHANVIENGQs");

            migrationBuilder.RenameTable(
                name: "KHOQUA",
                newName: "KHOQUAs");

            migrationBuilder.RenameTable(
                name: "CTKHOQUA",
                newName: "CTKHOQUAs");

            migrationBuilder.RenameIndex(
                name: "IX_THONGBAONVGQ_MaNVGQ",
                table: "THONGBAONVGQs",
                newName: "IX_THONGBAONVGQs_MaNVGQ");

            migrationBuilder.RenameIndex(
                name: "IX_THONGBAONV_MaNV",
                table: "THONGBAONVs",
                newName: "IX_THONGBAONVs_MaNV");

            migrationBuilder.RenameIndex(
                name: "IX_THONGBAOND_MaND",
                table: "THONGBAONDs",
                newName: "IX_THONGBAONDs_MaND");

            migrationBuilder.RenameIndex(
                name: "IX_NHANVIENGQ_MaKQ",
                table: "NHANVIENGQs",
                newName: "IX_NHANVIENGQs_MaKQ");

            migrationBuilder.RenameIndex(
                name: "IX_CTKHOQUA_MaQua",
                table: "CTKHOQUAs",
                newName: "IX_CTKHOQUAs_MaQua");

            migrationBuilder.AddPrimaryKey(
                name: "PK_THONGBAONVGQs",
                table: "THONGBAONVGQs",
                column: "MaTBNVGQ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_THONGBAONVs",
                table: "THONGBAONVs",
                column: "MaTBNV");

            migrationBuilder.AddPrimaryKey(
                name: "PK_THONGBAONDs",
                table: "THONGBAONDs",
                column: "MaTBND");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NHANVIENGQs",
                table: "NHANVIENGQs",
                column: "MaNVGQ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KHOQUAs",
                table: "KHOQUAs",
                column: "MaKQ");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CTKHOQUAs",
                table: "CTKHOQUAs",
                columns: new[] { "MaKQ", "MaQua" });

            migrationBuilder.AddForeignKey(
                name: "FK_CTKHOQUAs_KHOQUAs_MaKQ",
                table: "CTKHOQUAs",
                column: "MaKQ",
                principalTable: "KHOQUAs",
                principalColumn: "MaKQ",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CTKHOQUAs_PHANTHUONG_MaQua",
                table: "CTKHOQUAs",
                column: "MaQua",
                principalTable: "PHANTHUONG",
                principalColumn: "MaQua",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NHANVIENGQs_KHOQUAs_MaKQ",
                table: "NHANVIENGQs",
                column: "MaKQ",
                principalTable: "KHOQUAs",
                principalColumn: "MaKQ",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PHIEUDOIQUA_KHOQUAs_MaKQ",
                table: "PHIEUDOIQUA",
                column: "MaKQ",
                principalTable: "KHOQUAs",
                principalColumn: "MaKQ",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_THONGBAONDs_NGUOIDUNG_MaND",
                table: "THONGBAONDs",
                column: "MaND",
                principalTable: "NGUOIDUNG",
                principalColumn: "MaND",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_THONGBAONVGQs_NHANVIENGQs_MaNVGQ",
                table: "THONGBAONVGQs",
                column: "MaNVGQ",
                principalTable: "NHANVIENGQs",
                principalColumn: "MaNVGQ",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_THONGBAONVs_NHANVIEN_MaNV",
                table: "THONGBAONVs",
                column: "MaNV",
                principalTable: "NHANVIEN",
                principalColumn: "MaNV",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
