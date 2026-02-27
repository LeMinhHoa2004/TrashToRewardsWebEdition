namespace TrashToRewardsV2.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; } // MaND hoặc MaNV hoặc MaNVGQ

        // Thông tin cá nhân
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public float? DiemTichLuy { get; set; } // chỉ Người dùng mới có

        
    }
}
