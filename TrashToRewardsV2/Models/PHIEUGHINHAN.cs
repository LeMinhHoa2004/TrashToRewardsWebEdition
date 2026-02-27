using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class PHIEUGHINHAN
    {
        [Key]
        public int MaPhieuGN { get; set; }

        public DateTime NgayGhiNhan { get; set; }

        public float TongDiemGN { get; set; }
        [Required]
        public int MaND { get; set; }
        [ForeignKey("MaND")]
        [ValidateNever]
        public NGUOIDUNG NGUOIDUNG { get; set; }
        [Required]
        [ValidateNever]
        public int MaNV { get; set; }
        [ForeignKey("MaNV")]
        [ValidateNever]
        public NHANVIEN NHANVIEN { get; set; }
        [ValidateNever]
        public ICollection<CTPHIEUGHINHAN> CTPHIEUGHINHANs { get; set; }
    }
}
