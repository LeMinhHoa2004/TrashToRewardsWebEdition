using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class NHANVIENGQ
    {
        [Key]
        public int MaNVGQ { get; set; }

        [Required]
        public string HoTenNVGQ { get; set; }

        [Required]
        public string SDTNVGQ { get; set; }

        [Required]
        public string EmailNVGQ { get; set; }

        [Required]
        public string MatKhauNVGQ { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn đơn vị thu gom")]
        public int MaKQ { get; set; }
        [ForeignKey("MaKQ")]
        [ValidateNever]
        public KHOQUA KHOQUA { get; set; }

        
        [ValidateNever]
        public ICollection<THONGBAONVGQ> THONGBAONVGQs { get; set; } = new List<THONGBAONVGQ>();

    }
}
