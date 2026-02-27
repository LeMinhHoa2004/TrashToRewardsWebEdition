using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrashToRewardsV2.Models;
using TrashToRewardsV2.Services;

using TrashToRewardsV2.ViewModels;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "NHANVIEN")]
    public class PhieuThuGomController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IThongBaoService _thongBaoService;
        private readonly IEmailService _emailService;
        public PhieuThuGomController(ApplicationDbContext context, IThongBaoService thongBaoService, IEmailService emailService)
        {
            _context = context;
            _thongBaoService = thongBaoService; // Initialize the service
            _emailService = emailService;
        }

        // GET: Danh sách phiếu thu gom của nhân viên
        public async Task<IActionResult> Index()
        {
            var maNVStr = User.FindFirst("MaNV").Value;
            if (int.TryParse(maNVStr, out int maNV))
            {
                var phieuThuGoms = await _context.PHIEUTHUGOM
                    .Include(p => p.XACNHAN)
                        .ThenInclude(x => x.PHIEUDATLICH)
                            .ThenInclude(pdl => pdl.NGUOIDUNG)
                    .Include(p => p.XACNHAN)
                        .ThenInclude(x => x.NHANVIEN)
                    .Include(p => p.NHANVIEN)
                    .Where(p => p.MaNV == maNV)
                    .OrderByDescending(p => p.XACNHAN.PHIEUDATLICH.NgayDL)
                    .ToListAsync();


                return View(phieuThuGoms);
            }
            return NotFound();
        }

        
        // GET: Tạo phiếu thu gom mới
        public async Task<IActionResult> Create(int? maPhieuDL)
        {
            var maNV = int.Parse(User.FindFirstValue("MaNV"));
            //TempData["Debug"] = $"[DEBUG] Nhận maPhieuDL = {maPhieuDL}";

            if (!maPhieuDL.HasValue)
            {
                TempData["Error"] = "Không tìm thấy mã phiếu đặt lịch.";
                return RedirectToAction("List", "PhieuDatLich");
            }

            var xacNhan = await _context.XACNHAN
                .Include(x => x.PHIEUDATLICH)
                    .ThenInclude(p => p.NGUOIDUNG)
                .FirstOrDefaultAsync(x => x.MaPhieuDL == maPhieuDL.Value && x.MaNV == maNV && x.PHIEUDATLICH.TrangThaiDL == "DaXacNhan");

            if (xacNhan == null)
            {
                TempData["Error"] = "Bạn không có quyền lập phiếu thu gom cho lịch này. Lịch này đã được xác nhận bởi nhân viên khác.";
                return RedirectToAction("List", "PhieuDatLich");
            }

            ViewBag.LoaiPheLieuList = await _context.LOAIPHELIEU.ToListAsync();

            var viewModel = new PhieuThuGomViewModel
            {
                PhieuThuGom = new PHIEUTHUGOM
                {
                    MaXN = xacNhan.MaXN,
                    XACNHAN = xacNhan
                },
                ChiTietPhieuThuGom = new List<CTPHIEUTHUGOM>()
            };

            return View(viewModel);
        }






        // POST: Lưu phiếu thu gom mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhieuThuGomViewModel viewModel, List<int> maLoaiPL, List<float> khoiLuong)
        {
            var maNVStr = User.FindFirst("MaNV")?.Value;
            viewModel.PhieuThuGom ??= new PHIEUTHUGOM();
            Console.WriteLine($"[DEBUG] POST Create - MaXN nhận từ View: {viewModel.PhieuThuGom.MaXN}");

            if (!int.TryParse(maNVStr, out int maNV) || viewModel.PhieuThuGom == null)
            {
                ModelState.AddModelError("", "Không xác định được nhân viên.");
                return View(viewModel);
            }

            var xacNhan = await _context.XACNHAN
                .Include(x => x.PHIEUDATLICH)
                .FirstOrDefaultAsync(x => x.MaNV == maNV && x.MaXN == viewModel.PhieuThuGom.MaXN);

            if (xacNhan == null)
            {
                ModelState.AddModelError("", "Bạn không có quyền tạo phiếu thu gom này (chưa xác nhận).");
                return View(viewModel);
            }

            viewModel.PhieuThuGom.MaNV = maNV;
            viewModel.PhieuThuGom.TongDiem = 0;

            if (maLoaiPL == null || khoiLuong == null || maLoaiPL.Count != khoiLuong.Count)
            {
                ModelState.AddModelError("", "Dữ liệu chi tiết phế liệu không hợp lệ.");
                return View(viewModel);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.PHIEUTHUGOM.Add(viewModel.PhieuThuGom);
                    await _context.SaveChangesAsync();

                    float tongDiem = 0;

                    for (int i = 0; i < maLoaiPL.Count; i++)
                    {
                        if (khoiLuong[i] > 0)
                        {
                            var loai = await _context.LOAIPHELIEU.FindAsync(maLoaiPL[i]);
                            if (loai != null)
                            {
                                var chiTiet = new CTPHIEUTHUGOM
                                {
                                    MaPhieu = viewModel.PhieuThuGom.MaPhieu,
                                    MaLoaiPL = maLoaiPL[i],
                                    KhoiLuong = khoiLuong[i],
                                    SoDiemNhanDuoc = khoiLuong[i] * loai.TiLeDiem
                                };
                                _context.CTPHIEUTHUGOM.Add(chiTiet);
                                tongDiem += chiTiet.SoDiemNhanDuoc;
                            }
                        }
                    }

                    viewModel.PhieuThuGom.TongDiem = tongDiem;
                    _context.Update(viewModel.PhieuThuGom);

                    var phieuDatLich = xacNhan.PHIEUDATLICH;
                    phieuDatLich.TrangThaiDL = "DaXuLy";
                    _context.Update(phieuDatLich);

                    var nguoiDung = await _context.NGUOIDUNG.FindAsync(phieuDatLich.MaND);
                    if (nguoiDung != null)
                    {
                        nguoiDung.DiemTichLuy += tongDiem;
                        _context.Update(nguoiDung);

                        _context.LICHSUTICHDIEM.Add(new LICHSUTICHDIEM
                        {
                            MaND = nguoiDung.MaND,
                            NgayGioCapNhat = DateTime.Now,
                            SoDiemThayDoi = tongDiem,
                            LyDo = $"Thu gom rác từ phiếu #{viewModel.PhieuThuGom.MaPhieu}"
                        });

                        await _context.SaveChangesAsync();

                        // Gửi Email
                        var builder = new StringBuilder();
                        builder.Append($"<h2 style='text-align:center;'>PHIẾU THU GOM #{viewModel.PhieuThuGom.MaPhieu}</h2>");
                        builder.Append($"<p><strong>Người dùng:</strong> {nguoiDung.HoTen}<br>");
                        builder.Append($"<strong>Địa chỉ:</strong> {nguoiDung.DiaChi}<br>");
                        builder.Append($"<strong>Ngày đặt:</strong> {phieuDatLich.NgayDL:dd/MM/yyyy HH:mm}</p>");
                        builder.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;width:100%'>");
                        builder.Append("<thead><tr><th>Loại phế liệu</th><th>Khối lượng</th><th>Điểm</th></tr></thead><tbody>");

                        var chiTietList = await _context.CTPHIEUTHUGOM
                            .Include(ct => ct.LOAIPHELIEU)
                            .Where(ct => ct.MaPhieu == viewModel.PhieuThuGom.MaPhieu)
                            .ToListAsync();

                        foreach (var ct in chiTietList)
                        {
                            builder.Append($"<tr><td>{ct.LOAIPHELIEU?.TenLoai}</td><td>{ct.KhoiLuong} {ct.LOAIPHELIEU?.DonVi}</td><td>{ct.SoDiemNhanDuoc}</td></tr>");
                        }

                        builder.Append("</tbody></table>");
                        builder.Append($"<p><strong>Tổng điểm:</strong> {tongDiem}</p>");

                        await _emailService.SendEmailAsync(nguoiDung.Email, $"[TrashToRewards] Phiếu thu gom #{viewModel.PhieuThuGom.MaPhieu}", builder.ToString());
                    }

                    await _thongBaoService.ThongBaoHoanThanhThuGomAsync(xacNhan.MaPhieuDL, tongDiem);

                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Lỗi khi lưu phiếu thu gom: " + ex.Message);
                    return View(viewModel);
                }
            }
        }

        // GET: Chi tiết phiếu thu gom
        public async Task<IActionResult> Details(int id)
        {
            var phieuThuGom = await _context.PHIEUTHUGOM
                .Include(p => p.XACNHAN)
                    .ThenInclude(x => x.PHIEUDATLICH)
                        .ThenInclude(pdl => pdl.NGUOIDUNG)
                .Include(p => p.NHANVIEN)
                .Include(p => p.CTPHIEUTHUGOMs)
                    .ThenInclude(ct => ct.LOAIPHELIEU)
                .FirstOrDefaultAsync(p => p.MaPhieu == id);

            if (phieuThuGom == null)
                return NotFound();

            var viewModel = new PhieuThuGomViewModel
            {
                PhieuThuGom = phieuThuGom,
                ChiTietPhieuThuGom = phieuThuGom.CTPHIEUTHUGOMs?.ToList() ?? new List<CTPHIEUTHUGOM>()
            };

            return View(viewModel);
        }


        // API: Lấy thông tin chi tiết phiếu đặt lịch
        [HttpGet]
        public async Task<IActionResult> GetPhieuDatLichInfo(int maPhieuDL)
        {
            var phieuDatLich = await _context.PHIEUDATLICH
                .Include(p => p.NGUOIDUNG)
                .FirstOrDefaultAsync(p => p.MaPhieuDL == maPhieuDL);

            if (phieuDatLich == null)
                return NotFound();

            return Json(new
            {
                maPhieuDL = phieuDatLich.MaPhieuDL,
                ngayDL = phieuDatLich.NgayDL.ToString("dd/MM/yyyy HH:mm"),
                hoTenND = phieuDatLich.NGUOIDUNG.HoTen,
                diaChiND = phieuDatLich.NGUOIDUNG.DiaChi,
                sdtND = phieuDatLich.NGUOIDUNG.SDT
            });
        }

    }
}