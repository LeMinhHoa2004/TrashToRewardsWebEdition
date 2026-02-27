using TrashToRewardsV2.Models;

namespace TrashToRewardsV2.ViewModels
{
    public class PhieuThuGomViewModel
    {
        public PHIEUTHUGOM PhieuThuGom { get; set; }
        public List<CTPHIEUTHUGOM> ChiTietPhieuThuGom { get; set; }

        // Lấy PHIEUDATLICH từ XACNHAN
        public PHIEUDATLICH PhieuDatLich => PhieuThuGom?.XACNHAN?.PHIEUDATLICH;

        // Thông tin xác nhận
        public string NhanVienXacNhan => PhieuThuGom?.XACNHAN?.NHANVIEN?.HoTenNV;
        public DateTime? NgayXacNhan => PhieuThuGom?.XACNHAN?.NgayXN;
        public int MaPhieuDL { get; set; } // bind từ View để lookup XACNHAN

    }
}
