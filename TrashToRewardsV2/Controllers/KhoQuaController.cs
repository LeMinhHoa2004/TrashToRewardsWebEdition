using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;
using TrashToRewardsV2.Services;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class KhoQuaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IThongBaoService _thongBaoService;
        public KhoQuaController(ApplicationDbContext context, IThongBaoService thongBaoService)
        {
            _context = context;
            _thongBaoService = thongBaoService;
        }

        public async Task<IActionResult> Index()
        {
            var kho = await _context.KHOQUA.Include(k => k.CTKHOQUAs).ThenInclude(ct => ct.PHANTHUONG).ToListAsync();
            return View(kho);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KHOQUA kho)
        {
            if (ModelState.IsValid)
            {
                _context.KHOQUA.Add(kho);
                await _context.SaveChangesAsync();
                await _thongBaoService.ThongBaoThayDoiDonViThuGomAsync("thêm mới", kho.TenKQ);
                return RedirectToAction(nameof(Index));
            }
            return View(kho);
        }

        public async Task<IActionResult> Details(int id)
        {
            var kho = await _context.KHOQUA
                .Include(k => k.CTKHOQUAs)
                .ThenInclude(ct => ct.PHANTHUONG)
                .FirstOrDefaultAsync(k => k.MaKQ == id);

            if (kho == null) return NotFound();
            return View(kho);
        }

        public async Task<IActionResult> AddCTKhoQua(int khoId)
        {
            ViewBag.ListPhanThuong = new SelectList(await _context.PHANTHUONG.ToListAsync(), "MaQua", "TenQua");
            ViewBag.KhoId = khoId;
            return View(new CTKHOQUA { MaKQ = khoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCTKhoQua(CTKHOQUA ct)
        {
            ModelState.Remove("KHOQUA");
            ModelState.Remove("PHANTHUONG");
            // Kiểm tra phần thưởng đã tồn tại trong kho chưa
            bool isDuplicate = await _context.CTKHOQUA
                .AnyAsync(c => c.MaKQ == ct.MaKQ && c.MaQua == ct.MaQua);

            if (isDuplicate)
            {
                ModelState.AddModelError("", "Phần thưởng này đã tồn tại trong kho.");
            }
            if (ModelState.IsValid)
            {
                _context.CTKHOQUA.Add(ct);
                await _context.SaveChangesAsync();
                await UpdateSoLuongConPhanThuong(ct.MaQua);

                return RedirectToAction(nameof(Details), new { id = ct.MaKQ });
            }

            ViewBag.ListPhanThuong = new SelectList(await _context.PHANTHUONG.ToListAsync(), "MaQua", "TenQua");
            ViewBag.KhoId = ct.MaKQ;
            return View(ct);
        }

        public async Task<IActionResult> EditCTKhoQua(int maKQ, int maQua)
        {
            var ct = await _context.CTKHOQUA
                .Include(c => c.PHANTHUONG) // <-- THÊM DÒNG NÀY
                .FirstOrDefaultAsync(c => c.MaKQ == maKQ && c.MaQua == maQua);
            if (ct == null) return NotFound();
            return View(ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCTKhoQua(CTKHOQUA ct)
        {
            ModelState.Remove("PHANTHUONG");
            ModelState.Remove("KHOQUA");
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[EditCTKhoQua ERROR] {key}: {error.ErrorMessage}");
                    }
                }
            }

            var existing = await _context.CTKHOQUA.FindAsync(ct.MaKQ, ct.MaQua);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                existing.SoLuongTrongKho = ct.SoLuongTrongKho;
                await _context.SaveChangesAsync();
                await UpdateSoLuongConPhanThuong(ct.MaQua);

                return RedirectToAction(nameof(Details), new { id = ct.MaKQ });
            }
            return View(ct);
        }

        public async Task<IActionResult> DeleteCTKhoQua(int maKQ, int maQua)
        {
            var ct = await _context.CTKHOQUA
                .Include(c => c.PHANTHUONG)
                .FirstOrDefaultAsync(c => c.MaKQ == maKQ && c.MaQua == maQua);

            if (ct == null) return NotFound();
            return View(ct);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCTKhoQuaConfirmed(int maKQ, int maQua)
        {
            var ct = await _context.CTKHOQUA.FindAsync(maKQ, maQua);
            if (ct != null)
            {
                _context.CTKHOQUA.Remove(ct);
                await _context.SaveChangesAsync();
                await UpdateSoLuongConPhanThuong(maQua);
            }
            return RedirectToAction(nameof(Details), new { id = maKQ });
        }

        private async Task UpdateSoLuongConPhanThuong(int maQua)
        {
            var tong = await _context.CTKHOQUA
                .Where(c => c.MaQua == maQua)
                .SumAsync(c => (int?)c.SoLuongTrongKho) ?? 0;

            var phanThuong = await _context.PHANTHUONG.FindAsync(maQua);
            if (phanThuong != null)
            {
                phanThuong.SoLuongCon = tong;
                await _context.SaveChangesAsync();
            }
        }
        // Hiển thị trang xác nhận xóa kho
        public async Task<IActionResult> Delete(int id)
        {
            var kho = await _context.KHOQUA
                .Include(k => k.CTKHOQUAs)
                .FirstOrDefaultAsync(k => k.MaKQ == id);

            if (kho == null) return NotFound();
            return View(kho);
        }

        // Thực hiện xóa kho sau xác nhận
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int MaKQ)
        {
            var kho = await _context.KHOQUA
                .Include(k => k.CTKHOQUAs)
                .FirstOrDefaultAsync(k => k.MaKQ == MaKQ);

            if (kho != null)
            {
                string tenKQ = kho.TenKQ;

                // Lưu danh sách MaQua trước khi xóa
                var maQuaList = kho.CTKHOQUAs.Select(ct => ct.MaQua).ToList();

                _context.CTKHOQUA.RemoveRange(kho.CTKHOQUAs); // Xóa chi tiết kho
                _context.KHOQUA.Remove(kho);                  // Xóa kho
                await _context.SaveChangesAsync();

                // Cập nhật SoLuongCon cho mỗi phần thưởng vừa bị xóa khỏi kho
                foreach (var maQua in maQuaList)
                {
                    await UpdateSoLuongConPhanThuong(maQua);
                }

                await _thongBaoService.ThongBaoThayDoiDonViThuGomAsync("xóa", tenKQ);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var kho = await _context.KHOQUA.FindAsync(id);
            if (kho == null) return NotFound();
            return View(kho);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KHOQUA kho)
        {
            if (id != kho.MaKQ) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingKho = await _context.KHOQUA.FindAsync(id);
                    if (existingKho == null) return NotFound();

                    existingKho.TenKQ = kho.TenKQ;
                    existingKho.DiaChiKQ = kho.DiaChiKQ;
                    existingKho.SDTKQ = kho.SDTKQ;

                    await _context.SaveChangesAsync();
                    await _thongBaoService.ThongBaoThayDoiDonViThuGomAsync("cập nhật", kho.TenKQ);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.KHOQUA.AnyAsync(k => k.MaKQ == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(kho);
        }

    }
}
