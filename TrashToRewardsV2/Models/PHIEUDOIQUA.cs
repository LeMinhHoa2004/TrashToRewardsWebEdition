using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class PHIEUDOIQUA
    {
        [Key]
        public int MaPDQ { get; set; }
        [Required]
        public DateTime NgayGioDoi { get; set; }
        [Required]
        public float SoDiemSuDung { get; set; }
        [Required]
        public string TrangThai { get; set; }
        [Required]
        public string HinhThucNhanQua {  get; set; }
        [Required]
        public string DiaDiemNhanQua {  get; set; }
       
        [Required]
        public int MaND { get; set; }
        [ForeignKey("MaND")]
        [ValidateNever]
        public NGUOIDUNG NGUOIDUNG { get; set; }
        [Required]
        public int MaKQ { get; set; }  
        [ForeignKey("MaKQ")]
        [ValidateNever]
        public KHOQUA KHOQUA { get; set; }
        [ValidateNever]
        public ICollection<CTPHIEUDOIQUA> CTPHIEUDOIQUAs { get; set; }
    }
}
