using Microsoft.AspNetCore.Mvc;
using TrashToRewardsV2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrashToRewardsV2.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "NGUOIDUNG")]
    public class NguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NguoiDungController(ApplicationDbContext context)
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
            var maND = int.Parse(User.FindFirstValue("MaND"));
            var nd = await _context.NGUOIDUNG.FindAsync(maND);

            if (nd == null) return NotFound();

            var model = new ProfileViewModel
            {
                Id = nd.MaND,
                HoTen = nd.HoTen,
                Email = nd.Email,
                SDT = nd.SDT,
                DiaChi = nd.DiaChi,
                DiemTichLuy = nd.DiemTichLuy
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (await EmailExistsAsync(model.Email, "NGUOIDUNG", model.Id))
            {
                ModelState.AddModelError("Email", "Email này đã tồn tại trong hệ thống.");
            }

            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var nd = await _context.NGUOIDUNG.FindAsync(model.Id);
            if (nd == null) return NotFound();

            nd.HoTen = model.HoTen;
            nd.Email = model.Email;
            nd.SDT = model.SDT;
            nd.DiaChi = model.DiaChi;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_ChangePasswordPartial", model);

            var maND = int.Parse(User.FindFirstValue("MaND"));
            var nd = await _context.NGUOIDUNG.FindAsync(maND);

            if (nd == null) return NotFound();

            if (model.CurrentPassword != nd.MatKhau)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                return PartialView("_ChangePasswordPartial", model);
            }

            nd.MatKhau = model.NewPassword;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction(nameof(Profile));
        }
        [AllowAnonymous]
        public async Task<IActionResult> DonViThuGom()
        {
            var donVis = await _context.DONVITHUGOM.ToListAsync();
            return View(donVis);
        }


    }
}