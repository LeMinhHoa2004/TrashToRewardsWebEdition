
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrashToRewardsV2.Models;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "NGUOIDUNG")]
    public class LichSuTichDiemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LichSuTichDiemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var maNDStr = User.FindFirst("MaND")?.Value;
            if (int.TryParse(maNDStr, out int maNd))
            {
                var nguoiDung = await _context.NGUOIDUNG.FindAsync(maNd);
                var lichSuTichDiem = await _context.LICHSUTICHDIEM
                    .Where(ls => ls.MaND == maNd)
                    .OrderByDescending(ls => ls.NgayGioCapNhat)
                    .ToListAsync();

                ViewBag.NguoiDung = nguoiDung;
                return View(lichSuTichDiem);
            }
            return NotFound();
        }
    }
}
