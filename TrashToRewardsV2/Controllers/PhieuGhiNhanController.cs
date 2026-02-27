// Controllers/PhieuGhiNhanController.cs
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;
using TrashToRewardsV2.Services;

using TrashToRewardsV2.ViewModels;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "NHANVIEN")]
    public class PhieuGhiNhanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IThongBaoService _thongBaoService;
        private readonly IEmailService _emailService;
        public PhieuGhiNhanController(ApplicationDbContext context, IThongBaoService thongBaoService, IEmailService emailService)
        {
            _context = context;
            _thongBaoService = thongBaoService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new PhieuGhiNhanViewModel
            {
                PhieuGhiNhan = new PHIEUGHINHAN(),
                DanhSachNguoiDung = _context.NGUOIDUNG.ToList(),
                DanhSachLoaiPheLieu = _context.LOAIPHELIEU.ToList()
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhieuGhiNhanViewModel viewModel)
        {
            try
            {

                if (viewModel.ChiTietPhieuGhiNhan == null || !viewModel.ChiTietPhieuGhiNhan.Any())
                {
                    ModelState.AddModelError("", "Chưa thêm chi tiết phế liệu.");
                }

                var maLoaiPLList = viewModel.ChiTietPhieuGhiNhan.Select(ct => ct.MaLoaiPL).ToList();
                if (maLoaiPLList.Distinct().Count() != maLoaiPLList.Count)
                {
                    ModelState.AddModelError("", "Không được chọn trùng loại phế liệu.");
                }

                if (!ModelState.IsValid)
                {
                    foreach (var entry in ModelState)
                    {
                        var key = entry.Key;
                        var errors = entry.Value.Errors;
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"ModelState Error - Field: {key}, Error: {error.ErrorMessage}");
                        }
                    }
                    viewModel.DanhSachNguoiDung = _context.NGUOIDUNG.ToList();
                    viewModel.DanhSachLoaiPheLieu = _context.LOAIPHELIEU.ToList();
                    return View(viewModel);
                }

                var tongDiem = 0f;
                var phieuGhiNhan = viewModel.PhieuGhiNhan;
                phieuGhiNhan.NgayGhiNhan = DateTime.Now;
                phieuGhiNhan.MaNV = int.Parse(User.FindFirst("MaNV").Value);
                phieuGhiNhan.CTPHIEUGHINHANs = new List<CTPHIEUGHINHAN>();

                foreach (var chiTiet in viewModel.ChiTietPhieuGhiNhan)
                {
                    var loai = await _context.LOAIPHELIEU.FindAsync(chiTiet.MaLoaiPL);
                    if (loai == null) continue;

                    var diem = chiTiet.KhoiLuongGN * loai.TiLeDiem;
                    tongDiem += diem;

                    phieuGhiNhan.CTPHIEUGHINHANs.Add(new CTPHIEUGHINHAN
                    {
                        MaLoaiPL = chiTiet.MaLoaiPL,
                        KhoiLuongGN = chiTiet.KhoiLuongGN,
                        SoDiemNhanDuocGN = diem
                    });
                }

                phieuGhiNhan.TongDiemGN = tongDiem;
                _context.PHIEUGHINHAN.Add(phieuGhiNhan);
                await _context.SaveChangesAsync();

                var nguoiDung = await _context.NGUOIDUNG.FindAsync(phieuGhiNhan.MaND);
                if (nguoiDung != null)
                {
                    nguoiDung.DiemTichLuy += tongDiem;
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                }
                // Ghi nhận thông báo cho người dùng
                await _thongBaoService.ThongBaoNDAsync(phieuGhiNhan.MaND,
                    $"Bạn vừa được ghi nhận {tongDiem} điểm từ phiếu ghi nhận lúc {DateTime.Now:dd/MM/yyyy HH:mm}.");

                // Tạo lịch sử tích điểm
                _context.LICHSUTICHDIEM.Add(new LICHSUTICHDIEM
                {
                    MaND = phieuGhiNhan.MaND,
                    NgayGioCapNhat = DateTime.Now,
                    SoDiemThayDoi = tongDiem,
                    LyDo = $"Ghi nhận thu gom từ nhân viên vào {DateTime.Now:dd/MM/yyyy HH:mm}"
                });
                await _context.SaveChangesAsync();
                var builder = new StringBuilder();
                builder.Append("<h2 style='text-align:center;'>PHIẾU GHI NHẬN #" + phieuGhiNhan.MaPhieuGN + "</h2>");
                builder.Append("<p><strong>Người dùng:</strong> " + nguoiDung.HoTen + "<br>");
                builder.Append("<strong>Địa chỉ:</strong> " + nguoiDung.DiaChi + "<br>");
                builder.Append("<strong>Ngày ghi nhận:</strong> " + phieuGhiNhan.NgayGhiNhan.ToString("dd/MM/yyyy HH:mm") + "</p>");

                builder.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;width:100%'>");
                builder.Append("<thead><tr><th>Loại phế liệu</th><th>Khối lượng</th><th>Điểm</th></tr></thead><tbody>");
                foreach (var ct in phieuGhiNhan.CTPHIEUGHINHANs)
                {
                    builder.Append("<tr><td>" + ct.LOAIPHELIEU?.TenLoai + "</td>");
                    builder.Append("<td>" + ct.KhoiLuongGN + " " + ct.LOAIPHELIEU?.DonVi + "</td>");
                    builder.Append("<td>" + ct.SoDiemNhanDuocGN + "</td></tr>");
                }
                builder.Append("</tbody></table>");
                builder.Append("<p><strong>Tổng điểm:</strong> " + phieuGhiNhan.TongDiemGN + "</p>");

                await _emailService.SendEmailAsync(
                    nguoiDung.Email,
                    $"[TrashToRewards] Phiếu ghi nhận #{phieuGhiNhan.MaPhieuGN}",
                    builder.ToString()
                );


                return RedirectToAction("Index");

               
            }
            catch (Exception ex)
            {
                // Báo lỗi trên UI
                ModelState.AddModelError("", "Đã xảy ra lỗi khi tạo phiếu ghi nhận: " + ex.Message);

                // Load lại danh sách để render lại form
                viewModel.DanhSachNguoiDung = _context.NGUOIDUNG.ToList();
                viewModel.DanhSachLoaiPheLieu = _context.LOAIPHELIEU.ToList();

                return View(viewModel);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetNguoiDungInfo(int maND)
        {
            var nd = await _context.NGUOIDUNG.FindAsync(maND);
            if (nd == null) return NotFound();

            return Json(new
            {
                hoTen = nd.HoTen,
                email = nd.Email,
                sdt = nd.SDT,
                diaChi = nd.DiaChi,
                diem = nd.DiemTichLuy
            });
        }
        public async Task<IActionResult> Index()
        {
            var danhSach = await _context.PHIEUGHINHAN
                .Include(p => p.NGUOIDUNG)
                .Include(p => p.NHANVIEN)
                .ToListAsync();

            return View(danhSach); // => tạo View Index.cshtml nếu chưa có
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var phieu = await _context.PHIEUGHINHAN
                .Include(p => p.NGUOIDUNG)
                .Include(p => p.NHANVIEN)
                .Include(p => p.CTPHIEUGHINHANs)
                    .ThenInclude(ct => ct.LOAIPHELIEU)
                .FirstOrDefaultAsync(p => p.MaPhieuGN == id);

            if (phieu == null)
            {
                return NotFound();
            }

            return View(phieu);
        }
    }
}
