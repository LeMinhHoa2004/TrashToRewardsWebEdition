using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "NGUOIDUNG,NHANVIEN")]
    public class LoaiPheLieuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoaiPheLieuController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _context.LOAIPHELIEU.ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var loai = await _context.LOAIPHELIEU.FindAsync(id);
            if (loai == null) return NotFound();
            return View(loai);
        }
    }
}
