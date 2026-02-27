using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;
using System.Security.Claims;
using TrashToRewardsV2.ViewModels;

namespace TrashToRewardsV2.Controllers
{
    public class NhanVienGQController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NhanVienGQController(ApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<bool> EmailExistsAsync(string email, string currentEntityType, int? excludeId = null)
        {
            email = email?.Trim().ToLower();

            var emailInNguoiDung = await _context.NGUOIDUNG
                .AnyAsync(u => u.Email.ToLower() == email && !(currentEntityType == "NGUOIDUNG" && u.MaND == excludeId));
            var emailInNhanVien = await _context.NHANVIEN
                .AnyAsync(nv => nv.EmailNV.ToLower() == email && !(currentEntityType == "NHANVIEN" && nv.MaNV == excludeId));
            var emailInNhanVienGQ = await _context.NHANVIENGQ
                .AnyAsync(nvgq => nvgq.EmailNVGQ.ToLower() == email && !(currentEntityType == "NHANVIENGQ" && nvgq.MaNVGQ == excludeId));
            var emailInAdmin = await _context.ADMIN
                .AnyAsync(ad => ad.EmailAD.ToLower() == email && !(currentEntityType == "ADMIN" && ad.MaAD == excludeId));

            return emailInNguoiDung || emailInNhanVien || emailInNhanVienGQ || emailInAdmin;
        }


        // === QUẢN LÝ PROFILE CÁ NHÂN NHANVIENGQ ===
        [Authorize(Roles = "NHANVIENGQ")]
        public async Task<IActionResult> Profile()
        {
            var maNVGQ = int.Parse(User.FindFirstValue("MaNVGQ"));
            var nvgq = await _context.NHANVIENGQ.Include(x => x.KHOQUA).FirstOrDefaultAsync(x => x.MaNVGQ == maNVGQ);

            if (nvgq == null) return NotFound();

            var model = new ProfileViewModel
            {
                Id = nvgq.MaNVGQ,
                HoTen = nvgq.HoTenNVGQ,
                Email = nvgq.EmailNVGQ,
                SDT = nvgq.SDTNVGQ,
                DiaChi = nvgq.KHOQUA?.TenKQ ?? "(Chưa có kho)"
            };
            return View(model);
        }
        // ====== NHANVIENGQ tự sửa chính mình ======
        [Authorize(Roles = "NHANVIENGQ")]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (await EmailExistsAsync(model.Email, "NHANVIENGQ", model.Id))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
            }

            if (!ModelState.IsValid)
            {
                var nvgq = await _context.NHANVIENGQ.Include(x => x.KHOQUA).FirstOrDefaultAsync(x => x.MaNVGQ == model.Id);
                if (nvgq != null)
                    ViewBag.TenKhoQua = nvgq.KHOQUA?.TenKQ ?? "(Chưa có kho)";
                return View("Profile", model);
            }

            var nvgqUpdate = await _context.NHANVIENGQ.FindAsync(model.Id);
            if (nvgqUpdate == null) return NotFound();

            nvgqUpdate.HoTenNVGQ = model.HoTen;
            nvgqUpdate.EmailNVGQ = model.Email;
            nvgqUpdate.SDTNVGQ = model.SDT;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Profile));
        }


        [Authorize(Roles = "NHANVIENGQ")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_ChangePasswordPartial", model);

            var maNVGQ = int.Parse(User.FindFirstValue("MaNVGQ"));
            var nvgq = await _context.NHANVIENGQ.FindAsync(maNVGQ);

            if (nvgq == null) return NotFound();

            if (model.CurrentPassword != nvgq.MatKhauNVGQ)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                return PartialView("_ChangePasswordPartial", model);
            }

            nvgq.MatKhauNVGQ = model.NewPassword;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction(nameof(Profile));
        }
        // --- Xem danh sách phần thưởng trong Kho mà NVGQ đang làm ---
        [Authorize(Roles = "NHANVIENGQ")]
        public async Task<IActionResult> DanhSachPhanThuongTrongKho()
        {
            var maNVGQStr = User.FindFirstValue("MaNVGQ");
            if (string.IsNullOrEmpty(maNVGQStr) || !int.TryParse(maNVGQStr, out int maNVGQ))
            {
                return Unauthorized();
            }

            var nhanVien = await _context.NHANVIENGQ
                .Include(nv => nv.KHOQUA)
                .FirstOrDefaultAsync(nv => nv.MaNVGQ == maNVGQ);

            if (nhanVien == null || nhanVien.KHOQUA == null)
                return NotFound("Không tìm thấy kho quà.");

            var danhSach = await _context.CTKHOQUA
                .Include(ct => ct.PHANTHUONG)
                .Where(ct => ct.MaKQ == nhanVien.MaKQ)
                .ToListAsync();

            ViewBag.KhoTen = nhanVien.KHOQUA.TenKQ;
            return View(danhSach);
        }

        // --- Xem chi tiết phần thưởng trong Kho ---
        [Authorize(Roles = "NHANVIENGQ")]
        public async Task<IActionResult> ChiTietPhanThuongTrongKho(int maQua)
        {
            var maNVGQStr = User.FindFirstValue("MaNVGQ");
            if (string.IsNullOrEmpty(maNVGQStr) || !int.TryParse(maNVGQStr, out int maNVGQ))
            {
                return Unauthorized();
            }

            var nhanVien = await _context.NHANVIENGQ
                .Include(nv => nv.KHOQUA)
                .FirstOrDefaultAsync(nv => nv.MaNVGQ == maNVGQ);

            if (nhanVien == null)
                return NotFound();

            var ct = await _context.CTKHOQUA
                .Include(ct => ct.PHANTHUONG)
                .Include(ct => ct.KHOQUA)
                .FirstOrDefaultAsync(ct => ct.MaKQ == nhanVien.MaKQ && ct.MaQua == maQua);

            if (ct == null)
                return NotFound();

            return View(ct);
        }


        // === CRUD QUẢN LÝ NHANVIENGQ DÀNH CHO ADMIN ===
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            var nhanVienGQs = await _context.NHANVIENGQ
                .Include(nv => nv.KHOQUA)
                .ToListAsync();
            return View(nhanVienGQs);
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create()
        {
            ViewBag.KhoList = new SelectList(await _context.KHOQUA.ToListAsync(), "MaKQ", "TenKQ");
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NHANVIENGQ nv)
        {
            if (await EmailExistsAsync(nv.EmailNVGQ,"NHANVIENGQ",nv.MaNVGQ))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
                ViewBag.KhoList = new SelectList(await _context.KHOQUA.ToListAsync(), "MaKQ", "TenKQ", nv.MaKQ);
                return View(nv);
            }
            if (ModelState.IsValid)
            {
                _context.NHANVIENGQ.Add(nv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KhoList = new SelectList(await _context.KHOQUA.ToListAsync(), "MaKQ", "TenKQ", nv.MaKQ);
            return View(nv);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> EditAdmin(int id)
        {
            var nv = await _context.NHANVIENGQ.FindAsync(id);
            if (nv == null) return NotFound();

            ViewBag.KhoList = new SelectList(await _context.KHOQUA.ToListAsync(), "MaKQ", "TenKQ", nv.MaKQ);
            return View(nv);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(NHANVIENGQ nv)
        {
            if (await EmailExistsAsync(nv.EmailNVGQ, "NHANVIENGQ", nv.MaNVGQ))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
                ViewBag.KhoList = new SelectList(await _context.KHOQUA.ToListAsync(), "MaKQ", "TenKQ", nv.MaKQ);
                return View(nv);
            }
            if (ModelState.IsValid)
            {
                _context.NHANVIENGQ.Update(nv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KhoList = new SelectList(await _context.KHOQUA.ToListAsync(), "MaKQ", "TenKQ", nv.MaKQ);
            return View(nv);
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Details(int id)
        {
            var nv = await _context.NHANVIENGQ
                .Include(n => n.KHOQUA)
                .FirstOrDefaultAsync(n => n.MaNVGQ == id);
            if (nv == null) return NotFound();
            return View(nv);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var nv = await _context.NHANVIENGQ
                .Include(n => n.KHOQUA)
                .FirstOrDefaultAsync(n => n.MaNVGQ == id);

            if (nv == null) return NotFound();

            return View(nv); // Hiển thị Delete.cshtml
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nv = await _context.NHANVIENGQ
                .Include(n => n.THONGBAONVGQs)
                .FirstOrDefaultAsync(n => n.MaNVGQ == id);

            if (nv == null) return NotFound();

            try
            {
                _context.THONGBAONVGQ.RemoveRange(nv.THONGBAONVGQs);
                _context.NHANVIENGQ.Remove(nv);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Không thể xóa: " + (ex.InnerException?.Message ?? ex.Message));
                var nvReload = await _context.NHANVIENGQ.Include(n => n.KHOQUA).FirstOrDefaultAsync(n => n.MaNVGQ == id);
                return View("Delete", nvReload);
            }

            return RedirectToAction(nameof(Index));
        }









    }
}
