using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class PHIEUDATLICH
    {
        [Key]
        public int MaPhieuDL { get; set; }


        [Required]
        public DateTime NgayDL { get; set; }
        [Required]
        public string TrangThaiDL { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa điểm thu gom")]

        public string DiaDiemDL {  get; set; }
        [Required]
        public int MaND { get; set; }
        [ForeignKey("MaND")]
        [ValidateNever]
        public NGUOIDUNG NGUOIDUNG { get; set; }

        [ValidateNever]
        public XACNHAN XACNHAN { get; set; }

        
        
    }
    
}
