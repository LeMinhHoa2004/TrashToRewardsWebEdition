using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrashToRewardsV2.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADMIN",
                columns: table => new
                {
                    MaAD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTenAD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDTAD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhauAD = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN", x => x.MaAD);
                });

            migrationBuilder.CreateTable(
                name: "DONVITHUGOM",
                columns: table => new
                {
                    MaDV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiDV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDTDV = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DONVITHUGOM", x => x.MaDV);
                });

            migrationBuilder.CreateTable(
                name: "LOAIPHELIEU",
                columns: table => new
                {
                    MaLoaiPL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonVi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TiLeDiem = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAIPHELIEU", x => x.MaLoaiPL);
                });

            migrationBuilder.CreateTable(
                name: "NGUOIDUNG",
                columns: table => new
                {
                    MaND = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemTichLuy = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGUOIDUNG", x => x.MaND);
                });

            migrationBuilder.CreateTable(
                name: "PHANTHUONG",
                columns: table => new
                {
                    MaQua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQua = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemDoi = table.Column<float>(type: "real", nullable: false),
                    SoLuongCon = table.Column<int>(type: "int", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHANTHUONG", x => x.MaQua);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTenNV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDTNV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailNV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhauNV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaDV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHANVIEN", x => x.MaNV);
                    table.ForeignKey(
                        name: "FK_NHANVIEN_DONVITHUGOM_MaDV",
                        column: x => x.MaDV,
                        principalTable: "DONVITHUGOM",
                        principalColumn: "MaDV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LICHSUTICHDIEM",
                columns: table => new
                {
                    MaLS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayGioCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDiemThayDoi = table.Column<float>(type: "real", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaND = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LICHSUTICHDIEM", x => x.MaLS);
                    table.ForeignKey(
                        name: "FK_LICHSUTICHDIEM_NGUOIDUNG_MaND",
                        column: x => x.MaND,
                        principalTable: "NGUOIDUNG",
                        principalColumn: "MaND",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PHIEUDATLICH",
                columns: table => new
                {
                    MaPhieuDL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDL = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiDL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaND = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEUDATLICH", x => x.MaPhieuDL);
                    table.ForeignKey(
                        name: "FK_PHIEUDATLICH_NGUOIDUNG_MaND",
                        column: x => x.MaND,
                        principalTable: "NGUOIDUNG",
                        principalColumn: "MaND",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PHIEUDOIQUA",
                columns: table => new
                {
                    MaPDQ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayGioDoi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDiemSuDung = table.Column<float>(type: "real", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaND = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEUDOIQUA", x => x.MaPDQ);
                    table.ForeignKey(
                        name: "FK_PHIEUDOIQUA_NGUOIDUNG_MaND",
                        column: x => x.MaND,
                        principalTable: "NGUOIDUNG",
                        principalColumn: "MaND",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "THONGBAONDs",
                columns: table => new
                {
                    MaTBND = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDungND = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTaoND = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaDoc = table.Column<bool>(type: "bit", nullable: false),
                    MaND = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THONGBAONDs", x => x.MaTBND);
                    table.ForeignKey(
                        name: "FK_THONGBAONDs_NGUOIDUNG_MaND",
                        column: x => x.MaND,
                        principalTable: "NGUOIDUNG",
                        principalColumn: "MaND",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PHIEUGHINHAN",
                columns: table => new
                {
                    MaPhieuGN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayGhiNhan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongDiemGN = table.Column<float>(type: "real", nullable: false),
                    MaND = table.Column<int>(type: "int", nullable: false),
                    MaNV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEUGHINHAN", x => x.MaPhieuGN);
                    table.ForeignKey(
                        name: "FK_PHIEUGHINHAN_NGUOIDUNG_MaND",
                        column: x => x.MaND,
                        principalTable: "NGUOIDUNG",
                        principalColumn: "MaND",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PHIEUGHINHAN_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "THONGBAONVs",
                columns: table => new
                {
                    MaTBNV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDungNV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTaoNV = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaDoc = table.Column<bool>(type: "bit", nullable: false),
                    MaNV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THONGBAONVs", x => x.MaTBNV);
                    table.ForeignKey(
                        name: "FK_THONGBAONVs_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PHIEUTHUGOM",
                columns: table => new
                {
                    MaPhieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TongDiem = table.Column<float>(type: "real", nullable: false),
                    MaPhieuDL = table.Column<int>(type: "int", nullable: false),
                    MaNV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEUTHUGOM", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_PHIEUTHUGOM_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PHIEUTHUGOM_PHIEUDATLICH_MaPhieuDL",
                        column: x => x.MaPhieuDL,
                        principalTable: "PHIEUDATLICH",
                        principalColumn: "MaPhieuDL",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CTPHIEUDOIQUA",
                columns: table => new
                {
                    MaPDQ = table.Column<int>(type: "int", nullable: false),
                    MaQua = table.Column<int>(type: "int", nullable: false),
                    SoLuongDoi = table.Column<int>(type: "int", nullable: false),
                    SoDiemCan = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPHIEUDOIQUA", x => new { x.MaPDQ, x.MaQua });
                    table.ForeignKey(
                        name: "FK_CTPHIEUDOIQUA_PHANTHUONG_MaQua",
                        column: x => x.MaQua,
                        principalTable: "PHANTHUONG",
                        principalColumn: "MaQua",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CTPHIEUDOIQUA_PHIEUDOIQUA_MaPDQ",
                        column: x => x.MaPDQ,
                        principalTable: "PHIEUDOIQUA",
                        principalColumn: "MaPDQ",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CTPHIEUGHINHAN",
                columns: table => new
                {
                    MaLoaiPL = table.Column<int>(type: "int", nullable: false),
                    MaPhieuGN = table.Column<int>(type: "int", nullable: false),
                    KhoiLuongGN = table.Column<float>(type: "real", nullable: false),
                    SoDiemNhanDuocGN = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPHIEUGHINHAN", x => new { x.MaLoaiPL, x.MaPhieuGN });
                    table.ForeignKey(
                        name: "FK_CTPHIEUGHINHAN_LOAIPHELIEU_MaLoaiPL",
                        column: x => x.MaLoaiPL,
                        principalTable: "LOAIPHELIEU",
                        principalColumn: "MaLoaiPL",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CTPHIEUGHINHAN_PHIEUGHINHAN_MaPhieuGN",
                        column: x => x.MaPhieuGN,
                        principalTable: "PHIEUGHINHAN",
                        principalColumn: "MaPhieuGN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CTPHIEUTHUGOM",
                columns: table => new
                {
                    MaLoaiPL = table.Column<int>(type: "int", nullable: false),
                    MaPhieu = table.Column<int>(type: "int", nullable: false),
                    KhoiLuong = table.Column<float>(type: "real", nullable: false),
                    SoDiemNhanDuoc = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPHIEUTHUGOM", x => new { x.MaLoaiPL, x.MaPhieu });
                    table.ForeignKey(
                        name: "FK_CTPHIEUTHUGOM_LOAIPHELIEU_MaLoaiPL",
                        column: x => x.MaLoaiPL,
                        principalTable: "LOAIPHELIEU",
                        principalColumn: "MaLoaiPL",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CTPHIEUTHUGOM_PHIEUTHUGOM_MaPhieu",
                        column: x => x.MaPhieu,
                        principalTable: "PHIEUTHUGOM",
                        principalColumn: "MaPhieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CTPHIEUDOIQUA_MaQua",
                table: "CTPHIEUDOIQUA",
                column: "MaQua");

            migrationBuilder.CreateIndex(
                name: "IX_CTPHIEUGHINHAN_MaPhieuGN",
                table: "CTPHIEUGHINHAN",
                column: "MaPhieuGN");

            migrationBuilder.CreateIndex(
                name: "IX_CTPHIEUTHUGOM_MaPhieu",
                table: "CTPHIEUTHUGOM",
                column: "MaPhieu");

            migrationBuilder.CreateIndex(
                name: "IX_LICHSUTICHDIEM_MaND",
                table: "LICHSUTICHDIEM",
                column: "MaND");

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIEN_MaDV",
                table: "NHANVIEN",
                column: "MaDV");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUDATLICH_MaND",
                table: "PHIEUDATLICH",
                column: "MaND");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUDOIQUA_MaND",
                table: "PHIEUDOIQUA",
                column: "MaND");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUGHINHAN_MaND",
                table: "PHIEUGHINHAN",
                column: "MaND");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUGHINHAN_MaNV",
                table: "PHIEUGHINHAN",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUTHUGOM_MaNV",
                table: "PHIEUTHUGOM",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUTHUGOM_MaPhieuDL",
                table: "PHIEUTHUGOM",
                column: "MaPhieuDL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_THONGBAONDs_MaND",
                table: "THONGBAONDs",
                column: "MaND");

            migrationBuilder.CreateIndex(
                name: "IX_THONGBAONVs_MaNV",
                table: "THONGBAONVs",
                column: "MaNV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADMIN");

            migrationBuilder.DropTable(
                name: "CTPHIEUDOIQUA");

            migrationBuilder.DropTable(
                name: "CTPHIEUGHINHAN");

            migrationBuilder.DropTable(
                name: "CTPHIEUTHUGOM");

            migrationBuilder.DropTable(
                name: "LICHSUTICHDIEM");

            migrationBuilder.DropTable(
                name: "THONGBAONDs");

            migrationBuilder.DropTable(
                name: "THONGBAONVs");

            migrationBuilder.DropTable(
                name: "PHANTHUONG");

            migrationBuilder.DropTable(
                name: "PHIEUDOIQUA");

            migrationBuilder.DropTable(
                name: "PHIEUGHINHAN");

            migrationBuilder.DropTable(
                name: "LOAIPHELIEU");

            migrationBuilder.DropTable(
                name: "PHIEUTHUGOM");

            migrationBuilder.DropTable(
                name: "NHANVIEN");

            migrationBuilder.DropTable(
                name: "PHIEUDATLICH");

            migrationBuilder.DropTable(
                name: "DONVITHUGOM");

            migrationBuilder.DropTable(
                name: "NGUOIDUNG");
        }
    }
}
