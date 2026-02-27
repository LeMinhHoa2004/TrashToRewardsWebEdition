using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminThongBaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminThongBaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==== THÔNG BÁO NGƯỜI DÙNG ====
        public async Task<IActionResult> ThongBaoND()
        {
            var list = await _context.THONGBAOND
                .Include(tb => tb.NGUOIDUNG)
                .OrderByDescending(tb => tb.NgayTaoND)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> DetailsND(int id)
        {
            var tb = await _context.THONGBAOND
                .Include(tb => tb.NGUOIDUNG)
                .FirstOrDefaultAsync(tb => tb.MaTBND == id);
            return tb == null ? NotFound() : View(tb);
        }

        [HttpGet]
        public IActionResult CreateND()
        {
            ViewBag.NguoiDungList = new SelectList(_context.NGUOIDUNG, "MaND", "HoTen");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateND(THONGBAOND tb)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Xem lỗi chi tiết
                }
            }

            if (ModelState.IsValid)
            {
                tb.NgayTaoND = DateTime.Now;
                tb.DaDoc = false;
                _context.THONGBAOND.Add(tb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThongBaoND));
            }
            ViewBag.NguoiDungList = new SelectList(_context.NGUOIDUNG, "MaND", "HoTen", tb.MaND);
            return View(tb);
        }

        [HttpGet]
        public async Task<IActionResult> EditND(int id)
        {
            var tb = await _context.THONGBAOND.FindAsync(id);
            if (tb == null) return NotFound();
            ViewBag.NguoiDungList = new SelectList(_context.NGUOIDUNG, "MaND", "HoTen", tb.MaND);
            return View(tb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditND(THONGBAOND tb)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                var existingTb = await _context.THONGBAOND.FindAsync(tb.MaTBND);
                if (existingTb == null) return NotFound();

                // ❗ CHỈ cập nhật các trường được phép sửa
                existingTb.NoiDungND = tb.NoiDungND;
                existingTb.MaND = tb.MaND;

                // ❗ KHÔNG đụng tới existingTb.NgayTaoND, giữ nguyên

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThongBaoND));
            }

            ViewBag.NguoiDungList = new SelectList(_context.NGUOIDUNG, "MaND", "HoTen", tb.MaND);
            return View(tb);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteND(int id)
        {
            var tb = await _context.THONGBAOND.FindAsync(id);
            if (tb != null)
            {
                _context.THONGBAOND.Remove(tb);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ThongBaoND));
        }

        // ==== THÔNG BÁO NHÂN VIÊN ====
        public async Task<IActionResult> ThongBaoNV()
        {
            var list = await _context.THONGBAONV
                .Include(tb => tb.NHANVIEN)
                .OrderByDescending(tb => tb.NgayTaoNV)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> DetailsNV(int id)
        {
            var tb = await _context.THONGBAONV
                .Include(tb => tb.NHANVIEN)
                .FirstOrDefaultAsync(tb => tb.MaTBNV == id);
            return tb == null ? NotFound() : View(tb);
        }

        [HttpGet]
        public IActionResult CreateNV()
        {
            ViewBag.NhanVienList = new SelectList(_context.NHANVIEN, "MaNV", "HoTenNV");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNV(THONGBAONV tb)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Xem lỗi chi tiết
                }
            }

            if (ModelState.IsValid)
            {
                tb.NgayTaoNV = DateTime.Now;
                tb.DaDoc = false;
                _context.THONGBAONV.Add(tb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThongBaoNV));
            }
            ViewBag.NhanVienList = new SelectList(_context.NHANVIEN, "MaNV", "HoTenNV", tb.MaNV);
            return View(tb);
        }

        [HttpGet]
        public async Task<IActionResult> EditNV(int id)
        {
            var tb = await _context.THONGBAONV.FindAsync(id);
            if (tb == null) return NotFound();
            ViewBag.NhanVienList = new SelectList(_context.NHANVIEN, "MaNV", "HoTenNV", tb.MaNV);
            return View(tb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNV(THONGBAONV tb)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                var existingTb = await _context.THONGBAONV.FindAsync(tb.MaTBNV);
                if (existingTb == null) return NotFound();

                // ❗ CHỈ cập nhật các trường được phép sửa
                existingTb.NoiDungNV = tb.NoiDungNV;
                existingTb.MaNV = tb.MaNV;

                // ❗ KHÔNG đụng tới existingTb.NgayTaoND, giữ nguyên

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThongBaoNV));
            }

            ViewBag.NhanVienList = new SelectList(_context.NGUOIDUNG, "MaNV", "HoTenNV", tb.MaNV);
            return View(tb);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteNV(int id)
        {
            var tb = await _context.THONGBAONV.FindAsync(id);
            if (tb != null)
            {
                _context.THONGBAONV.Remove(tb);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ThongBaoNV));
        }

        // ==== THÔNG BÁO NHÂN VIÊN GIAO QUÀ ====
        public async Task<IActionResult> ThongBaoNVGQ()
        {
            var list = await _context.THONGBAONVGQ
                .Include(tb => tb.NHANVIENGQ)
                .OrderByDescending(tb => tb.NgayTaoNVGQ)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> DetailsNVGQ(int id)
        {
            var tb = await _context.THONGBAONVGQ
                .Include(tb => tb.NHANVIENGQ)
                .FirstOrDefaultAsync(tb => tb.MaTBNVGQ == id);
            return tb == null ? NotFound() : View(tb);
        }

        [HttpGet]
        public IActionResult CreateNVGQ()
        {
            ViewBag.NhanVienGQList = new SelectList(_context.NHANVIENGQ, "MaNVGQ", "HoTenNVGQ");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNVGQ(THONGBAONVGQ tb)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Xem lỗi chi tiết
                }
            }

            if (ModelState.IsValid)
            {
                tb.NgayTaoNVGQ = DateTime.Now;
                tb.DaDoc = false;
                _context.THONGBAONVGQ.Add(tb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThongBaoNVGQ));
            }
            ViewBag.NhanVienGQList = new SelectList(_context.NHANVIENGQ, "MaNVGQ", "HoTenNVGQ", tb.MaNVGQ);
            return View(tb);
        }

        [HttpGet]
        public async Task<IActionResult> EditNVGQ(int id)
        {
            var tb = await _context.THONGBAONVGQ.FindAsync(id);
            if (tb == null) return NotFound();
            ViewBag.NhanVienGQList = new SelectList(_context.NHANVIENGQ, "MaNVGQ", "HoTenNVGQ", tb.MaNVGQ);
            return View(tb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNVGQ(THONGBAONVGQ tb)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                var existingTb = await _context.THONGBAONVGQ.FindAsync(tb.MaTBNVGQ);
                if (existingTb == null) return NotFound();

                
                existingTb.NoiDungNVGQ = tb.NoiDungNVGQ;
                existingTb.MaNVGQ = tb.MaNVGQ;

               

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThongBaoNVGQ));
            }

            ViewBag.NhanVienGQList = new SelectList(_context.NGUOIDUNG, "MaNVGQ", "HoTenNVGQ", tb.MaNVGQ);
            return View(tb);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNVGQ(int id)
        {
            var tb = await _context.THONGBAONVGQ.FindAsync(id);
            if (tb != null)
            {
                _context.THONGBAONVGQ.Remove(tb);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ThongBaoNVGQ));
        }
        // GỬI THÔNG BÁO HÀNG LOẠT CHO NGƯỜI DÙNG
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendBulkToNguoiDung(string noiDung)
        {
            var users = await _context.NGUOIDUNG.ToListAsync();
            foreach (var user in users)
            {
                var tb = new THONGBAOND
                {
                    MaND = user.MaND,
                    NoiDungND = noiDung,
                    NgayTaoND = DateTime.Now,
                    DaDoc = false
                };
                _context.THONGBAOND.Add(tb);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Đã gửi thông báo đến {users.Count} người dùng.";
            return RedirectToAction(nameof(ThongBaoND));
        }

        // GỬI THÔNG BÁO HÀNG LOẠT CHO NHÂN VIÊN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendBulkToNhanVien(string noiDung)
        {
            var staff = await _context.NHANVIEN.ToListAsync();
            foreach (var nv in staff)
            {
                var tb = new THONGBAONV
                {
                    MaNV = nv.MaNV,
                    NoiDungNV = noiDung,
                    NgayTaoNV = DateTime.Now,
                    DaDoc = false
                };
                _context.THONGBAONV.Add(tb);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Đã gửi thông báo đến {staff.Count} nhân viên.";
            return RedirectToAction(nameof(ThongBaoNV));
        }

        // GỬI THÔNG BÁO HÀNG LOẠT CHO NHÂN VIÊN GIAO QUÀ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendBulkToNhanVienGQ(string noiDung)
        {
            var staffGQ = await _context.NHANVIENGQ.ToListAsync();
            foreach (var nvgq in staffGQ)
            {
                var tb = new THONGBAONVGQ
                {
                    MaNVGQ = nvgq.MaNVGQ,
                    NoiDungNVGQ = noiDung,
                    NgayTaoNVGQ = DateTime.Now,
                    DaDoc = false
                };
                _context.THONGBAONVGQ.Add(tb);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Đã gửi thông báo đến {staffGQ.Count} nhân viên giao quà.";
            return RedirectToAction(nameof(ThongBaoNVGQ));
        }

    }
}
