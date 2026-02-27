using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TrashToRewardsV2.Models;

namespace TrashToRewardsV2.ViewModels
{
    public class PhieuGhiNhanViewModel
    {
        public PHIEUGHINHAN PhieuGhiNhan { get; set; }
        public List<CTPHIEUGHINHAN> ChiTietPhieuGhiNhan { get; set; } = new List<CTPHIEUGHINHAN>();

        // Để binding dropdown trong View
        [ValidateNever]
        public List<NGUOIDUNG> DanhSachNguoiDung { get; set; }
        [ValidateNever]
        public List<LOAIPHELIEU> DanhSachLoaiPheLieu { get; set; }
    }
}
