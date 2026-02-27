using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class NGUOIDUNG
    {
        [Key]
        public int MaND { get; set; }

        [Required, MaxLength(100)]
        public string HoTen { get; set; }

        [Required, MaxLength(15)]
        public string SDT { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string MatKhau { get; set; }

        public string DiaChi { get; set; }

        public float DiemTichLuy { get; set; } = 0;

        [ValidateNever]
        public ICollection<PHIEUDATLICH> PHIEUDATLICHs { get; set; } = new List<PHIEUDATLICH>();


        [ValidateNever]
        public ICollection<PHIEUGHINHAN> PHIEUGHINHANs { get; set; } = new List<PHIEUGHINHAN>();

        [ValidateNever]
        public ICollection<PHIEUDOIQUA> PHIEUDOIQUAs { get; set; } = new List<PHIEUDOIQUA>();

        [ValidateNever]
        public ICollection<LICHSUTICHDIEM> LICHSUTICHDIEMs { get; set; } = new List<LICHSUTICHDIEM>();
        [ValidateNever]
        public ICollection<THONGBAOND> THONGBAONDs { get; set; } = new List<THONGBAOND>();
    }
}
