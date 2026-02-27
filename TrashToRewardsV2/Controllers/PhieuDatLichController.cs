using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using TrashToRewardsV2.Models;
using TrashToRewardsV2.Services;

namespace TrashToRewardsV2.Controllers
{
    public class PhieuDatLichController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IThongBaoService _thongBaoService;

        public PhieuDatLichController(ApplicationDbContext context, IThongBaoService thongBaoService)
        {
            _context = context;
            _thongBaoService = thongBaoService;
        }
        private async Task CapNhatTrangThaiPhieuDatLichQuaHan()
        {
            var now = DateTime.Now;
            var danhSachPhieuQuaHan = await _context.PHIEUDATLICH
                .Where(p => (p.TrangThaiDL == "ChuaXacNhan" || p.TrangThaiDL == "DaXacNhan")
                            && p.NgayDL.AddHours(3) < now)
                .ToListAsync();

            foreach (var phieu in danhSachPhieuQuaHan)
            {
                phieu.TrangThaiDL = "DaHuy";
                await _thongBaoService.ThongBaoHuyPhieuDatLichQuaHanAsync(phieu.MaPhieuDL);
            }

            if (danhSachPhieuQuaHan.Count > 0)
            {
                await _context.SaveChangesAsync();
            }
        }


        // GET: Trang đặt lịch cho người dùng
        [Authorize(Roles = "NGUOIDUNG")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lưu phiếu đặt lịch mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "NGUOIDUNG")]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            var maNDStr = User.FindFirst("MaND")?.Value;
            if (!int.TryParse(maNDStr, out int maND))
                return Unauthorized();

            var diaDiem = form["DiaDiemDL"];
            var ngayDLStr = form["NgayDL"];
            DateTime ngayDL;
            if (!DateTime.TryParseExact(ngayDLStr, "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayDL))
            {
                ModelState.AddModelError("NgayDL", "Ngày không hợp lệ. Vui lòng nhập đúng định dạng dd/MM/yyyy HH:mm.");
                return View();
            }

            var phieuDatLich = new PHIEUDATLICH
            {
                MaND = maND,
                NgayDL = ngayDL,
                DiaDiemDL = diaDiem,
                TrangThaiDL = "ChuaXacNhan"
            };

            if (ModelState.IsValid)
            {
                _context.PHIEUDATLICH.Add(phieuDatLich);
                await _context.SaveChangesAsync();
                await _thongBaoService.ThongBaoPhieuDatLichMoiAsync(phieuDatLich.MaPhieuDL);
                return RedirectToAction("Index");
            }

            return View(phieuDatLich);
        }





        // GET: Danh sách phiếu đặt lịch của người dùng hiện tại
        [Authorize(Roles = "NGUOIDUNG")]
        public async Task<IActionResult> Index()
        {
            await CapNhatTrangThaiPhieuDatLichQuaHan();

            var maNDStr = User.FindFirst("MaND")?.Value;
            if (!int.TryParse(maNDStr, out int maND)) return Unauthorized();

            var danhSach = await _context.PHIEUDATLICH
                .Include(p => p.NGUOIDUNG)
                .Where(p => p.MaND == maND)
                .OrderByDescending(p => p.NgayDL)
                .ToListAsync();

            return View(danhSach);
        }

        // GET: Chi tiết phiếu đặt lịch
        public async Task<IActionResult> Details(int id)
        {
            var phieuDatLich = await _context.PHIEUDATLICH
                .Include(p => p.NGUOIDUNG)
                .FirstOrDefaultAsync(p => p.MaPhieuDL == id);

            if (phieuDatLich == null) return NotFound();

            // Tìm phiếu thu gom thông qua bảng XACNHAN
            var phieuThuGom = await _context.PHIEUTHUGOM
                .Include(p => p.XACNHAN)
                    .ThenInclude(x => x.NHANVIEN)
                .Include(p => p.CTPHIEUTHUGOMs)
                    .ThenInclude(ct => ct.LOAIPHELIEU)
                .FirstOrDefaultAsync(p => p.XACNHAN.MaPhieuDL == id);

            ViewBag.PhieuThuGom = phieuThuGom;

            return View(phieuDatLich);
        }

        // POST: Người dùng hủy đặt lịch
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "NGUOIDUNG")]
        public async Task<IActionResult> Cancel(int id)
        {
            var phieuDatLich = await _context.PHIEUDATLICH.FindAsync(id);

            if (phieuDatLich == null)
                return NotFound();

            // Chỉ cho phép hủy khi chưa xác nhận hoặc đã xác nhận nhưng chưa xử lý
            if (phieuDatLich.TrangThaiDL == "ChuaXacNhan" || phieuDatLich.TrangThaiDL == "DaXacNhan")
            {
                phieuDatLich.TrangThaiDL = "DaHuy";
                await _context.SaveChangesAsync();

                // Thông báo cho nhân viên về việc hủy đặt lịch
                await _thongBaoService.ThongBaoHuyPhieuDatLichAsync(phieuDatLich.MaPhieuDL);

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        // GET: Danh sách phiếu đặt lịch cho nhân viên
        [Authorize(Roles = "NHANVIEN")]
        public async Task<IActionResult> List()
        {
            await CapNhatTrangThaiPhieuDatLichQuaHan();

            var danhSach = await _context.PHIEUDATLICH
                .Include(p => p.NGUOIDUNG)
                .OrderByDescending(p => p.NgayDL)
                .ToListAsync();

            return View(danhSach);
        }


        // POST: Nhân viên xác nhận đặt lịch
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "NHANVIEN")]
        public async Task<IActionResult> Confirm(int id)
        {
            var maNV = int.Parse(User.FindFirstValue("MaNV"));

            var phieuDatLich = await _context.PHIEUDATLICH.FindAsync(id);
            if (phieuDatLich == null)
                return NotFound();

            // Đã được xác nhận từ trước bởi nhân viên này?
            var daXacNhan = await _context.XACNHAN
                .AnyAsync(x => x.MaPhieuDL == id && x.MaNV == maNV);
            if (daXacNhan)
            {
                TempData["Info"] = "Bạn đã xác nhận phiếu này rồi.";
                return RedirectToAction("List");
            }

            if (phieuDatLich.TrangThaiDL == "ChuaXacNhan")
            {
                phieuDatLich.TrangThaiDL = "DaXacNhan";
                _context.XACNHAN.Add(new XACNHAN
                {
                    MaPhieuDL = id,
                    MaNV = maNV,
                    NgayXN = DateTime.Now
                });

                await _context.SaveChangesAsync();

                // Gửi thông báo cho người dùng
                await _thongBaoService.ThongBaoXacNhanDatLichAsync(phieuDatLich.MaPhieuDL);

                TempData["Success"] = "Xác nhận phiếu đặt lịch thành công!";
                return RedirectToAction("List");
            }

            return BadRequest();
        }



    }

}