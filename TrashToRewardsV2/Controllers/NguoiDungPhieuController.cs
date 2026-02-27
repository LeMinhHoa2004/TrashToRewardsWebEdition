using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;
using System.Security.Claims;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "NGUOIDUNG")]
    public class NguoiDungPhieuController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NguoiDungPhieuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== Xem danh sách Phiếu Thu Gom =====
        public async Task<IActionResult> PhieuThuGom()
        {
            var maNDStr = User.FindFirstValue("MaND");
            if (string.IsNullOrEmpty(maNDStr) || !int.TryParse(maNDStr, out int maND))
                return Unauthorized();

            var phieuThuGomList = await _context.PHIEUTHUGOM
                .Include(p => p.XACNHAN)
                    .ThenInclude(x => x.PHIEUDATLICH)
                        .ThenInclude(pdl => pdl.NGUOIDUNG)
                .Include(p => p.NHANVIEN)
                .Include(p => p.CTPHIEUTHUGOMs)
                    .ThenInclude(ct => ct.LOAIPHELIEU)
                .Where(p => p.XACNHAN.PHIEUDATLICH.MaND == maND)
                .OrderByDescending(p => p.MaPhieu)
                .ToListAsync();

            return View(phieuThuGomList);
        }


        // ===== Xem chi tiết Phiếu Thu Gom =====
        public async Task<IActionResult> PhieuThuGomDetails(int id)
        {
            var phieu = await _context.PHIEUTHUGOM
                .Include(p => p.XACNHAN)
                    .ThenInclude(x => x.PHIEUDATLICH)
                        .ThenInclude(pdl => pdl.NGUOIDUNG)
                .Include(p => p.NHANVIEN)
                .Include(p => p.CTPHIEUTHUGOMs)
                    .ThenInclude(ct => ct.LOAIPHELIEU)
                .FirstOrDefaultAsync(p => p.MaPhieu == id);

            if (phieu == null)
                return NotFound();

            return View(phieu);
        }

        // ===== Xem danh sách Phiếu Ghi Nhận =====
        public async Task<IActionResult> PhieuGhiNhan()
        {
            var maNDStr = User.FindFirstValue("MaND");
            if (string.IsNullOrEmpty(maNDStr) || !int.TryParse(maNDStr, out int maND))
                return Unauthorized();

            var phieuGhiNhanList = await _context.PHIEUGHINHAN
                .Include(p => p.NHANVIEN)
                .Where(p => p.MaND == maND)
                .OrderByDescending(p => p.MaPhieuGN)
                .ToListAsync();

            return View(phieuGhiNhanList);
        }

        // ===== Xem chi tiết Phiếu Ghi Nhận =====
        public async Task<IActionResult> PhieuGhiNhanDetails(int id)
        {
            var phieu = await _context.PHIEUGHINHAN
                .Include(p => p.NGUOIDUNG)
                .Include(p => p.NHANVIEN)
                    .ThenInclude(nv => nv.DONVITHUGOM)
                .Include(p => p.CTPHIEUGHINHANs)
                    .ThenInclude(ct => ct.LOAIPHELIEU)
                .FirstOrDefaultAsync(p => p.MaPhieuGN == id);

            if (phieu == null)
                return NotFound();

            return View(phieu);
        }
        // === Xem danh sách Phiếu Đổi Quà ===
        public async Task<IActionResult> PhieuDoiQua()
        {
            var maNDStr = User.FindFirstValue("MaND");
            if (string.IsNullOrEmpty(maNDStr) || !int.TryParse(maNDStr, out int maND))
                return Unauthorized();

            var danhSach = await _context.PHIEUDOIQUA
                .Include(p => p.KHOQUA)
                .Where(p => p.MaND == maND)
                .OrderByDescending(p => p.NgayGioDoi)
                .ToListAsync();

            return View(danhSach);
        }

        // === Xem chi tiết Phiếu Đổi Quà ===
        public async Task<IActionResult> PhieuDoiQuaDetails(int id)
        {
            var phieu = await _context.PHIEUDOIQUA
                .Include(p => p.KHOQUA)
                .Include(p => p.CTPHIEUDOIQUAs)
                    .ThenInclude(ct => ct.PHANTHUONG)
                .FirstOrDefaultAsync(p => p.MaPDQ == id);

            if (phieu == null)
                return NotFound();

            return View(phieu);
        }

        // === Xem danh sách Phần thưởng ===
        public async Task<IActionResult> PhanThuong()
        {
            var danhSach = await _context.PHANTHUONG.ToListAsync();
            return View(danhSach);
        }

        // === Xem chi tiết Phần thưởng (kèm tồn kho ở các kho quà) ===
        public async Task<IActionResult> PhanThuongDetails(int id)
        {
            var phanThuong = await _context.PHANTHUONG
                .Include(pt => pt.CTKHOQUAs)
                    .ThenInclude(ct => ct.KHOQUA)
                .FirstOrDefaultAsync(pt => pt.MaQua == id);

            if (phanThuong == null)
                return NotFound();

            return View(phanThuong);
        }

        // === Xem danh sách Kho quà ===
        public async Task<IActionResult> KhoQua()
        {
            var danhSach = await _context.KHOQUA.ToListAsync();
            return View(danhSach);
        }

        // === Xem chi tiết Kho quà (kèm tồn phần thưởng) ===
        public async Task<IActionResult> KhoQuaDetails(int id)
        {
            var kho = await _context.KHOQUA
                .Include(k => k.CTKHOQUAs)
                    .ThenInclude(ct => ct.PHANTHUONG)
                .FirstOrDefaultAsync(k => k.MaKQ == id);

            if (kho == null)
                return NotFound();

            return View(kho);
        }
    }
}
