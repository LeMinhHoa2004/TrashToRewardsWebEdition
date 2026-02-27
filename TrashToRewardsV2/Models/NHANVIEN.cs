using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class NHANVIEN
    {
        [Key]
        public int MaNV { get; set; }

        [Required]
        public string HoTenNV { get; set; }

        [Required]
        public string SDTNV { get; set; }

        [Required]
        public string EmailNV { get; set; }

        [Required]
        public string MatKhauNV { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn đơn vị thu gom")]
        public int MaDV { get; set; }
        [ForeignKey("MaDV")]
        [ValidateNever]
        public DONVITHUGOM DONVITHUGOM { get; set; }

        [ValidateNever]
        public ICollection<XACNHAN> XACNHANs { get; set; } = new List<XACNHAN>();

        [ValidateNever]
        public ICollection<PHIEUGHINHAN> PHIEUGHINHANs { get; set; } = new List<PHIEUGHINHAN>();
        [ValidateNever]
        public ICollection<THONGBAONV> THONGBAONVs { get; set; } = new List<THONGBAONV>();
    }
}
