using Microsoft.AspNetCore.Mvc;
using TrashToRewardsV2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrashToRewardsV2.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "NHANVIEN")]
    public class NhanVienController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NhanVienController(ApplicationDbContext context)
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

        public async Task<IActionResult> Profile()
        {
            var maNV = int.Parse(User.FindFirstValue("MaNV"));
            var nv = await _context.NHANVIEN.Include(x => x.DONVITHUGOM).FirstOrDefaultAsync(x => x.MaNV == maNV);

            if (nv == null) return NotFound();

            var model = new ProfileViewModel
            {
                Id = nv.MaNV,
                HoTen = nv.HoTenNV,
                Email = nv.EmailNV,
                SDT = nv.SDTNV,
                DiaChi = nv.DONVITHUGOM?.TenDV ?? "(Chưa có đơn vị)"
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (await EmailExistsAsync(model.Email, "NHANVIEN", model.Id))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
            }

            if (!ModelState.IsValid)
            {
                var nv = await _context.NHANVIEN.Include(x => x.DONVITHUGOM).FirstOrDefaultAsync(x => x.MaNV == model.Id);
                if (nv != null)
                    ViewBag.TenDonVi = nv.DONVITHUGOM?.TenDV ?? "(Chưa có đơn vị)";
                return View("Profile", model);
            }

            var nvUpdate = await _context.NHANVIEN.FindAsync(model.Id);
            if (nvUpdate == null) return NotFound();

            nvUpdate.HoTenNV = model.HoTen;
            nvUpdate.EmailNV = model.Email;
            nvUpdate.SDTNV = model.SDT;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_ChangePasswordPartial", model);

            var maNV = int.Parse(User.FindFirstValue("MaNV"));
            var nv = await _context.NHANVIEN.FindAsync(maNV);

            if (nv == null) return NotFound();

            if (model.CurrentPassword != nv.MatKhauNV)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                return PartialView("_ChangePasswordPartial", model);
            }

            nv.MatKhauNV = model.NewPassword;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction(nameof(Profile));
        }


    }
}