using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;
using TrashToRewardsV2.Services;
using System.Security.Claims;
using System.Text;


namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "NHANVIENGQ")]
    public class NhanVienGQPhieuDoiQuaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IThongBaoService _thongBaoService;
        private readonly IEmailService _emailService;
        public NhanVienGQPhieuDoiQuaController(ApplicationDbContext context, IThongBaoService thongBaoService, IEmailService emailService)
        {
            _context = context;
            _thongBaoService = thongBaoService;
            _emailService = emailService;
        }

        // Xem các phiếu thuộc kho mình
        public async Task<IActionResult> Index()
        {
            var maNVGQStr = User.FindFirstValue("MaNVGQ");
            if (string.IsNullOrEmpty(maNVGQStr) || !int.TryParse(maNVGQStr, out int maNVGQ))
                return Unauthorized();

            var nhanVien = await _context.NHANVIENGQ.Include(n => n.KHOQUA).FirstOrDefaultAsync(n => n.MaNVGQ == maNVGQ);
            if (nhanVien == null) return Unauthorized();

            var phieus = await _context.PHIEUDOIQUA
                .Where(p => p.MaKQ == nhanVien.MaKQ)
                .Include(p => p.NGUOIDUNG)
                .OrderByDescending(p => p.NgayGioDoi)
                .ToListAsync();

            return View(phieus);
        }

        // Xem chi tiết phiếu
        public async Task<IActionResult> Details(int id)
        {
            var phieu = await _context.PHIEUDOIQUA
                .Include(p => p.NGUOIDUNG)
                .Include(p => p.CTPHIEUDOIQUAs)
                .ThenInclude(ct => ct.PHANTHUONG)
                .FirstOrDefaultAsync(p => p.MaPDQ == id);

            if (phieu == null) return NotFound();

            return View(phieu);
        }

        // Xác nhận phiếu
        [HttpPost]
        public async Task<IActionResult> XacNhan(int id)
        {
            var phieu = await _context.PHIEUDOIQUA
                .Include(p => p.NGUOIDUNG)
                .Include(p => p.CTPHIEUDOIQUAs)
                    .ThenInclude(ct => ct.PHANTHUONG)
                .FirstOrDefaultAsync(p => p.MaPDQ == id);

            if (phieu == null) return NotFound();

            string newTrangThai = phieu.TrangThai;
            bool sendEmail = false;

            if (phieu.HinhThucNhanQua == "TaiNha")
            {
                if (phieu.TrangThai == "ChuaXacNhan")
                {
                    newTrangThai = "DaXacNhan";
                }
                else if (phieu.TrangThai == "DaXacNhan")
                {
                    newTrangThai = "DangGiao";
                }
                else if (phieu.TrangThai == "DangGiao")
                {
                    newTrangThai = "DaGiao";
                    sendEmail = true;
                    await _thongBaoService.ThongBaoNDAsync(phieu.MaND, $"Phiếu đổi quà #{phieu.MaPDQ} đã giao thành công!");
                }
            }
            else if (phieu.HinhThucNhanQua == "TrucTiep" && phieu.TrangThai == "ChuaNhan")
            {
                newTrangThai = "DaNhan";
                sendEmail = true;
                await _thongBaoService.ThongBaoNDAsync(phieu.MaND, $"Bạn đã nhận thành công phiếu đổi quà #{phieu.MaPDQ}!");
            }

            phieu.TrangThai = newTrangThai;
            await _context.SaveChangesAsync();

            if (sendEmail)
            {
                var builder = new StringBuilder();
                builder.Append("<h2 style='text-align:center;'>PHIẾU ĐỔI QUÀ #" + phieu.MaPDQ + "</h2>");
                builder.Append("<p><strong>Người dùng:</strong> " + phieu.NGUOIDUNG.HoTen + "<br>");
                builder.Append("<strong>Ngày đổi:</strong> " + phieu.NgayGioDoi.ToString("dd/MM/yyyy HH:mm") + "<br>");
                builder.Append("<strong>Hình thức nhận:</strong> " + phieu.HinhThucNhanQua + "<br>");
                if (!string.IsNullOrEmpty(phieu.DiaDiemNhanQua))
                    builder.Append("<strong>Địa điểm nhận:</strong> " + phieu.DiaDiemNhanQua + "<br>");
                builder.Append("</p>");

                builder.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;width:100%'>");
                builder.Append("<thead><tr><th>Phần thưởng</th><th>Số lượng</th><th>Điểm cần</th></tr></thead><tbody>");
                foreach (var ct in phieu.CTPHIEUDOIQUAs)
                {
                    builder.Append("<tr><td>" + ct.PHANTHUONG?.TenQua + "</td>");
                    builder.Append("<td>" + ct.SoLuongDoi + "</td>");
                    builder.Append("<td>" + ct.SoDiemCan + "</td></tr>");
                }
                builder.Append("</tbody></table>");
                builder.Append("<p><strong>Tổng điểm sử dụng:</strong> " + phieu.SoDiemSuDung + "</p>");

                await _emailService.SendEmailAsync(
                    phieu.NGUOIDUNG.Email,
                    $"[TrashToRewards] Xác nhận phiếu đổi quà #{phieu.MaPDQ}",
                    builder.ToString()
                );
            }

            return RedirectToAction(nameof(Details), new { id });
        }


        // Hủy phiếu (chỉ được hủy nếu chưa xác nhận hoặc chưa nhận)
        [HttpPost]
        public async Task<IActionResult> Huy(int id)
        {
            var phieu = await _context.PHIEUDOIQUA
                .Include(p => p.CTPHIEUDOIQUAs)
                .FirstOrDefaultAsync(p => p.MaPDQ == id);

            if (phieu == null) return NotFound();
            if (!(phieu.TrangThai == "ChuaXacNhan" || phieu.TrangThai == "ChuaNhan"))
            {
                return BadRequest("Không thể hủy phiếu đã xác nhận hoặc đã giao.");
            }

            var nguoiDung = await _context.NGUOIDUNG.FindAsync(phieu.MaND);

            // Hoàn điểm
            if (nguoiDung != null)
                nguoiDung.DiemTichLuy += phieu.SoDiemSuDung;

            // Hoàn lại số lượng quà trong kho
            foreach (var ct in phieu.CTPHIEUDOIQUAs)
            {
                var ctkho = await _context.CTKHOQUA.FirstOrDefaultAsync(c => c.MaKQ == phieu.MaKQ && c.MaQua == ct.MaQua);
                if (ctkho != null)
                {
                    ctkho.SoLuongTrongKho += ct.SoLuongDoi;
                }
            }

            // Đổi trạng thái
            phieu.TrangThai = "DaHuy";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
