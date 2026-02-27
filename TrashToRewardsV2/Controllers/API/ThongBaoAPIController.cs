using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;

namespace TrashToRewardsV2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ThongBaoAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ThongBaoAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("KiemTraThongBaoMoi")]
        public async Task<ActionResult<int>> KiemTraThongBaoMoi(string loaiNguoiDung, int ma)
        {
            if (loaiNguoiDung == "NGUOIDUNG")
            {
                int soThongBao = await _context.THONGBAOND
                    .CountAsync(tb => tb.MaND == ma && tb.DaDoc == false);
                return Ok(soThongBao);
            }
            else if (loaiNguoiDung == "NHANVIEN")
            {
                int soThongBao = await _context.THONGBAONV
                    .CountAsync(tb => tb.MaNV == ma && tb.DaDoc == false);
                return Ok(soThongBao);
            }
            else if (loaiNguoiDung == "NHANVIENGQ")
            {
                int soThongBao = await _context.THONGBAONV
                    .CountAsync(tb => tb.MaNV == ma && tb.DaDoc == false);
                return Ok(soThongBao);
            }

            return BadRequest("Loại người dùng không hợp lệ");
        }
        [HttpPost("DanhDauDocND/{id}")]
        public async Task<IActionResult> DanhDauDocND(int id)
        {
            var tb = await _context.THONGBAOND.FindAsync(id);
            if (tb != null)
            {
                tb.DaDoc = true;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("DanhDauDocNV/{id}")]
        public async Task<IActionResult> DanhDauDocNV(int id)
        {
            var tb = await _context.THONGBAONV.FindAsync(id);
            if (tb != null)
            {
                tb.DaDoc = true;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
        [HttpPost("DanhDauDocNVGQ/{id}")]
        public async Task<IActionResult> DanhDauDocNVGQ(int id)
        {
            var tb = await _context.THONGBAONVGQ.FindAsync(id);
            if (tb != null)
            {
                tb.DaDoc = true;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}
