// File: Controllers/ThongBaoController.cs
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;
using TrashToRewardsV2.Services;

namespace TrashToRewardsV2.Controllers
{
    [Authorize]
    public class ThongBaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IThongBaoService _thongBaoService;
        public ThongBaoController(ApplicationDbContext context, IThongBaoService thongBaoService)
        {
            _context = context;
            _thongBaoService = thongBaoService;
        }

        [Authorize(Roles = "NGUOIDUNG")]
        public async Task<IActionResult> NguoiDung()
        {
            var maND = int.Parse(User.FindFirstValue("MaND"));

            var thongBao = await _context.THONGBAOND
                .Where(tb => tb.MaND == maND)
                .OrderByDescending(tb => tb.NgayTaoND)
                .ToListAsync();

            return View(thongBao);
        }

        [Authorize(Roles = "NHANVIEN")]
        public async Task<IActionResult> NhanVien()
        {
            var maNV = int.Parse(User.FindFirstValue("MaNV"));

            var thongBao = await _context.THONGBAONV
                .Where(tb => tb.MaNV == maNV)
                .OrderByDescending(tb => tb.NgayTaoNV)
                .ToListAsync();

            return View(thongBao);
        }
        [Authorize(Roles = "NHANVIENGQ")]
        public async Task<IActionResult> NhanVienGQ()
        {
            var maNVGQ = int.Parse(User.FindFirstValue("MaNVGQ"));

            var thongBao = await _context.THONGBAONVGQ
                .Where(tb => tb.MaNVGQ == maNVGQ)
                .OrderByDescending(tb => tb.NgayTaoNVGQ)
                .ToListAsync();

            return View(thongBao);
        }
        [HttpPost]
        [Authorize(Roles = "NGUOIDUNG")]
        public async Task<IActionResult> DanhDauTatCaDaDocND()
        {
            int maND = int.Parse(User.FindFirstValue("MaND"));
            await _thongBaoService.DanhDauTatCaLaDaDocNDAsync(maND);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "NHANVIEN")]
        public async Task<IActionResult> DanhDauTatCaDaDocNV()
        {
            int maNV = int.Parse(User.FindFirstValue("MaNV"));
            await _thongBaoService.DanhDauTatCaLaDaDocNVAsync(maNV);
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "NHANVIENGQ")]
        public async Task<IActionResult> DanhDauTatCaDaDocNVGQ()
        {
            int maNVGQ = int.Parse(User.FindFirstValue("MaNVGQ"));
            await _thongBaoService.DanhDauTatCaLaDaDocNVAsync(maNVGQ);
            return Ok();
        }
    }
}