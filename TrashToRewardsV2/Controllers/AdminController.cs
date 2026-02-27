using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;
using TrashToRewardsV2.Services;

namespace TrashToRewardsV2.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IThongBaoService _thongBaoService;

        public AdminController(ApplicationDbContext context, IThongBaoService thongBaoService)
        {
            _context = context;
            _thongBaoService = thongBaoService;
        }

        public async Task<IActionResult> Dashboard()
        {
            IEnumerable<NGUOIDUNG> users = await _context.NGUOIDUNG.ToListAsync();
            IEnumerable<NHANVIEN> staff = await _context.NHANVIEN.Include(n => n.DONVITHUGOM).ToListAsync();
            return View((users, staff));
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



        // NGƯỜI DÙNG

        public IActionResult CreateUser() => View();
        
        [HttpPost]
        public async Task<IActionResult> CreateUser(NGUOIDUNG user)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            if (await EmailExistsAsync(user.Email,"NGUOIDUNG",user.MaND))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
                return View(user);
            }
            if (ModelState.IsValid)
            {
                
                user.DiemTichLuy = 0; // đảm bảo không bị lỗi
                _context.NGUOIDUNG.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(user);
        }

        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.NGUOIDUNG.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, NGUOIDUNG user)
        {
            if (await EmailExistsAsync(user.Email, "NGUOIDUNG",id))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
                return View(user);
            }
            var existingUser = await _context.NGUOIDUNG.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Gán từng thuộc tính để tránh lỗi overposting
                existingUser.HoTen = user.HoTen;
                existingUser.Email = user.Email;
                existingUser.SDT = user.SDT;
                existingUser.DiaChi = user.DiaChi;
                existingUser.MatKhau = user.MatKhau;

                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(user);
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.NGUOIDUNG.FindAsync(id);
            if (user == null) return NotFound();
            return View(user); // => hiển thị trang xác nhận
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int MaND)
        {
            var user = await _context.NGUOIDUNG.FindAsync(MaND);
            if (user != null)
            {
                _context.NGUOIDUNG.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }


        // NHÂN VIÊN

        public IActionResult CreateStaff()
        {
            var donvis = _context.DONVITHUGOM.ToList();
            
            if (!donvis.Any())
            {
                TempData["Error"] = "Bạn cần tạo đơn vị thu gom trước khi thêm nhân viên.";
                return RedirectToAction("CreateUnit");
            }

            ViewBag.DonViList = new SelectList(donvis, "MaDV", "TenDV");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateStaff(NHANVIEN staff)
        {
            // In thông tin để debug (bạn đã có rồi)
            Console.WriteLine("MaDV được chọn: " + staff.MaDV);

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Model error: " + error.ErrorMessage);
                }
            }

            if (await EmailExistsAsync(staff.EmailNV,"NHANVIEN",staff.MaNV))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
                ViewBag.DonViList = new SelectList(_context.DONVITHUGOM, "MaDV", "TenDV");
                return View(staff);
            }

            if (ModelState.IsValid)
            {
                
                _context.NHANVIEN.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }

            ViewBag.DonViList = new SelectList(_context.DONVITHUGOM, "MaDV", "TenDV");
            return View(staff);
        }



        public async Task<IActionResult> EditStaff(int id)
        {
            var staff = await _context.NHANVIEN.FindAsync(id);
            if (staff == null) return NotFound();

            ViewBag.DonViList = new SelectList(_context.DONVITHUGOM, "MaDV", "TenDV", staff.MaDV);
            return View(staff);
        }

        [HttpPost]
        public async Task<IActionResult> EditStaff(NHANVIEN staff)
        {
            if (await EmailExistsAsync(staff.EmailNV, "NHANVIEN", staff.MaNV))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
                ViewBag.DonViList = new SelectList(_context.DONVITHUGOM, "MaDV", "TenDV", staff.MaDV);
                return View(staff);
            }

            var existingStaff = await _context.NHANVIEN.FindAsync(staff.MaNV);
            if (existingStaff == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Cập nhật từng trường cụ thể để tránh lỗi overposting
                existingStaff.HoTenNV = staff.HoTenNV;
                existingStaff.SDTNV = staff.SDTNV;
                existingStaff.EmailNV = staff.EmailNV;
                existingStaff.MatKhauNV = staff.MatKhauNV;
                existingStaff.MaDV = staff.MaDV;

                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }

            ViewBag.DonViList = new SelectList(_context.DONVITHUGOM, "MaDV", "TenDV", staff.MaDV);
            return View(staff);
        }


        // Hiển thị trang xác nhận
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _context.NHANVIEN.FindAsync(id);
            if (staff == null)
                return NotFound();

            return View(staff); // trả về view xác nhận
        }

        // Thực hiện xóa sau khi xác nhận
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStaffConfirmed(int MaNV)
        {
            var staff = await _context.NHANVIEN.FindAsync(MaNV);
            if (staff != null)
            {
                _context.NHANVIEN.Remove(staff);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> DetailsStaff(int id)
        {
            var staff = await _context.NHANVIEN
                .Include(n => n.DONVITHUGOM)
                .FirstOrDefaultAsync(n => n.MaNV == id);

            if (staff == null)
                return NotFound();

            return View(staff);
        }

        public IActionResult Profile()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var maADStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (role != "ADMIN" || string.IsNullOrEmpty(maADStr) || !int.TryParse(maADStr.Replace("ADMIN_", ""), out int maAD))
            {
                return RedirectToAction("Login", "Account");
            }

            var admin = _context.ADMIN.FirstOrDefault(a => a.MaAD == maAD);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }
        public async Task<IActionResult> DetailsUser(int id)
        {
            var user = await _context.NGUOIDUNG.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        // ĐƠN VỊ THU GOM

        public IActionResult CreateUnit() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUnit(DONVITHUGOM unit)
        {
            if (ModelState.IsValid)
            {
                _context.DONVITHUGOM.Add(unit);
                await _context.SaveChangesAsync();

                // Thêm thông báo khi tạo đơn vị thu gom mới
                await _thongBaoService.ThongBaoThayDoiDonViThuGomAsync("thêm mới", unit.TenDV);

                return RedirectToAction("Dashboard");
            }
            return View(unit);
        }

        public async Task<IActionResult> EditUnit(int id)
        {
            var unit = await _context.DONVITHUGOM.FindAsync(id);
            if (unit == null) return NotFound();
            return View(unit);
        }

        [HttpPost]
        public async Task<IActionResult> EditUnit(DONVITHUGOM unit)
        {
            if (ModelState.IsValid)
            {
                _context.Update(unit);
                await _context.SaveChangesAsync();

                // Thêm thông báo khi cập nhật đơn vị thu gom
                await _thongBaoService.ThongBaoThayDoiDonViThuGomAsync("cập nhật", unit.TenDV);

                return RedirectToAction("Dashboard");
            }
            return View(unit);
        }

        public async Task<IActionResult> DeleteUnit(int id)
        {
            var unit = await _context.DONVITHUGOM.FindAsync(id);
            if (unit == null) return NotFound();
            return View(unit);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUnit(DONVITHUGOM unit)
        {
            var dbUnit = await _context.DONVITHUGOM.FindAsync(unit.MaDV);
            if (dbUnit != null)
            {
                // Lưu tên đơn vị thu gom trước khi xóa để sử dụng trong thông báo
                string tenDV = dbUnit.TenDV;

                _context.DONVITHUGOM.Remove(dbUnit);
                await _context.SaveChangesAsync();

                // Thêm thông báo sau khi xóa đơn vị thu gom
                await _thongBaoService.ThongBaoThayDoiDonViThuGomAsync("xóa", tenDV);
            }
            return RedirectToAction("Dashboard");
        }
        public async Task<IActionResult> DetailsUnit(int id)
        {
            var unit = await _context.DONVITHUGOM
                .Include(dv => dv.NHANVIENs)
                .FirstOrDefaultAsync(dv => dv.MaDV == id);

            if (unit == null)
                return NotFound();

            return View(unit);
        }
        // LOẠI PHẾ LIỆU

        

        public IActionResult CreateScrapType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateScrapType(LOAIPHELIEU type)
        {
            if (ModelState.IsValid)
            {
                _context.LOAIPHELIEU.Add(type);
                await _context.SaveChangesAsync();

                // Thêm thông báo khi tạo loại phế liệu mới
                await _thongBaoService.ThongBaoThayDoiLoaiPheLieuAsync("thêm mới", type.TenLoai);

                return RedirectToAction("Dashboard");
            }
            return View(type);
        }

        public async Task<IActionResult> EditScrapType(int id)
        {
            var type = await _context.LOAIPHELIEU.FindAsync(id);
            if (type == null)
                return NotFound();
            return View(type);
        }

        [HttpPost]
        public async Task<IActionResult> EditScrapType(LOAIPHELIEU type)
        {
            if (ModelState.IsValid)
            {
                _context.LOAIPHELIEU.Update(type);
                await _context.SaveChangesAsync();

                // Thêm thông báo khi cập nhật loại phế liệu
                await _thongBaoService.ThongBaoThayDoiLoaiPheLieuAsync("cập nhật", type.TenLoai);

                return RedirectToAction("Dashboard");
            }
            return View(type);
        }

        public async Task<IActionResult> DeleteScrapType(int id)
        {
            var type = await _context.LOAIPHELIEU.FindAsync(id);
            if (type == null)
                return NotFound();
            return View(type); // Hiển thị xác nhận
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteScrapTypeConfirmed(int MaLoaiPL)
        {
            var type = await _context.LOAIPHELIEU.FindAsync(MaLoaiPL);
            if (type != null)
            {
                // Lưu tên loại phế liệu trước khi xóa để sử dụng trong thông báo
                string tenLoai = type.TenLoai;

                _context.LOAIPHELIEU.Remove(type);
                await _context.SaveChangesAsync();

                // Thêm thông báo sau khi xóa loại phế liệu
                await _thongBaoService.ThongBaoThayDoiLoaiPheLieuAsync("xóa", tenLoai);
            }
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> DetailsScrapType(int id)
        {
            var type = await _context.LOAIPHELIEU.FindAsync(id);
            if (type == null)
                return NotFound();
            return View(type);
        }
        // PHẦN THƯỞNG

        public IActionResult CreateReward()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReward(PHANTHUONG reward, IFormFile hinhAnh)
        {
            try
            {
                // Xử lý validation cho file upload trước
                if (hinhAnh == null || hinhAnh.Length == 0)
                {
                    ModelState.AddModelError("hinhAnh", "Vui lòng chọn hình ảnh cho phần thưởng");
                    return View(reward);
                }

                // Tạm thời bỏ qua validation cho trường HinhAnh của model vì 
                // chúng ta sẽ gán giá trị cho nó sau khi upload file
                if (ModelState.ContainsKey("HinhAnh"))
                {
                    ModelState.Remove("HinhAnh");
                }

                // Kiểm tra các validation khác
                if (!ModelState.IsValid)
                {
                    return View(reward);
                }

                // Xử lý upload file
                string fileName = ProcessUploadedFile(hinhAnh);
                if (string.IsNullOrEmpty(fileName))
                {
                    ModelState.AddModelError("hinhAnh", "Không thể xử lý file ảnh. Vui lòng thử lại");
                    return View(reward);
                }

                // Gán tên file vào model
                reward.HinhAnh = fileName;
                reward.SoLuongCon = 0; // Luôn set về 0 khi mới thêm
                // Lưu vào database
                _context.PHANTHUONG.Add(reward);
                await _context.SaveChangesAsync();

                // Thông báo
                await _thongBaoService.ThongBaoThayDoiPhanThuongAsync("thêm mới", reward.TenQua);

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm phần thưởng: " + ex.Message);
                return View(reward);
            }
        }

        public async Task<IActionResult> EditReward(int id)
        {
            var reward = await _context.PHANTHUONG.FindAsync(id);
            if (reward == null)
                return NotFound();
            return View(reward);
        }

        [HttpPost]
        public async Task<IActionResult> EditReward(PHANTHUONG reward, IFormFile hinhAnh)
        {
            try
            {
                var existingReward = await _context.PHANTHUONG.FindAsync(reward.MaQua);
                if (existingReward == null)
                    return NotFound();

                // Nếu validation cho HinhAnh gây vấn đề, loại bỏ nó
                if (ModelState.ContainsKey("HinhAnh"))
                {
                    ModelState.Remove("HinhAnh");
                }

                // Kiểm tra validation cho các trường khác
                if (!ModelState.IsValid)
                {
                    return View(reward);
                }

                // Cập nhật các thuộc tính
                existingReward.TenQua = reward.TenQua;
                existingReward.MoTa = reward.MoTa;
                existingReward.DiemDoi = reward.DiemDoi;
                

                // Xử lý upload ảnh mới nếu có
                if (hinhAnh != null && hinhAnh.Length > 0)
                {
                    string fileName = ProcessUploadedFile(hinhAnh);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        existingReward.HinhAnh = fileName;
                    }
                }
                // Khi cập nhật, nếu không có ảnh mới, giữ nguyên ảnh cũ

                await _context.SaveChangesAsync();

                await _thongBaoService.ThongBaoThayDoiPhanThuongAsync("cập nhật", existingReward.TenQua);

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật phần thưởng: " + ex.Message);
                return View(reward);
            }
        }
        private string ProcessUploadedFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return null;

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Đảm bảo thư mục tồn tại
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Tạo tên file duy nhất 
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Log thông tin để debug
                Console.WriteLine($"Đang lưu file {file.FileName} tới {filePath}");

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                // Kiểm tra file đã được tạo
                if (System.IO.File.Exists(filePath))
                {
                    Console.WriteLine($"File đã được lưu thành công: {filePath}");
                    return uniqueFileName;
                }
                else
                {
                    Console.WriteLine($"Lưu file thất bại: {filePath}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xử lý file: {ex.Message}");
                return null;
            }
        }
        public async Task<IActionResult> DeleteReward(int id)
        {
            var reward = await _context.PHANTHUONG.FindAsync(id);
            if (reward == null)
                return NotFound();
            return View(reward); // Hiển thị xác nhận
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRewardConfirmed(int MaQua)
        {
            var reward = await _context.PHANTHUONG.FindAsync(MaQua);
            if (reward != null)
            {
                // Lưu tên phần thưởng trước khi xóa để sử dụng trong thông báo
                string tenQua = reward.TenQua;

                _context.PHANTHUONG.Remove(reward);
                await _context.SaveChangesAsync();

                // Thêm thông báo sau khi xóa phần thưởng
                await _thongBaoService.ThongBaoThayDoiPhanThuongAsync("xóa", tenQua);
            }
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> DetailsReward(int id)
        {
            var reward = await _context.PHANTHUONG
                .Include(p => p.CTKHOQUAs)
                .ThenInclude(ct => ct.KHOQUA)
                .FirstOrDefaultAsync(p => p.MaQua == id);

            if (reward == null)
                return NotFound();

            return View(reward);
        }

        // --- ADMIN ---

        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(ADMIN admin)
        {
            if (await EmailExistsAsync(admin.EmailAD,"ADMIN",admin.MaAD))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
                return View(admin);
            }
            if (ModelState.IsValid)
            {
                _context.ADMIN.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(admin);
        }

        public async Task<IActionResult> EditAdmin(int id)
        {
            var admin = await _context.ADMIN.FindAsync(id);
            if (admin == null)
                return NotFound();

            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdmin(ADMIN admin)
        {
            if (await EmailExistsAsync(admin.EmailAD, "ADMIN", admin.MaAD))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng trong hệ thống.");
                return View(admin);
            }
            if (ModelState.IsValid)
            {
                _context.ADMIN.Update(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(admin);
        }

        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.ADMIN.FindAsync(id);
            if (admin == null)
                return NotFound();

            var currentAdminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentAdminIdStr?.Replace("ADMIN_", ""), out int currentAdminId);

            if (admin.MaAD == currentAdminId)
            {
                TempData["Error"] = "Bạn không thể xóa chính tài khoản của mình.";
                return View(admin);
            }
            if (admin.MaAD == 1)
            {
                TempData["Error"] = "Bạn không thể xóa ADMIN gốc (ID = 1).";
                return RedirectToAction("Dashboard");
            }

            return View(admin); // hiển thị xác nhận
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdminConfirmed(int MaAD)
        {
            var currentAdminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(currentAdminIdStr?.Replace("ADMIN_", ""), out int currentAdminId);
            if (MaAD == 1)
            {
                TempData["Error"] = "Bạn không thể xóa ADMIN gốc (ID = 1).";
                return RedirectToAction("Dashboard");
            }
            var admin = await _context.ADMIN.FindAsync(MaAD);
            if (admin != null && admin.MaAD != currentAdminId)
            {
                _context.ADMIN.Remove(admin);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["Error"] = "Bạn không thể xóa chính tài khoản của mình.";
            }
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> DetailsAdmin(int id)
        {
            var admin = await _context.ADMIN.FindAsync(id);
            if (admin == null)
                return NotFound();
            return View(admin);
        }

        


    }
}
