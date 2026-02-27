using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class PHANTHUONG
    {
        [Key]
        public int MaQua { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên phần thưởng")]
        [Display(Name = "Tên phần thưởng")]
        public string TenQua { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm đổi")]
        [Display(Name = "Điểm đổi")]
        [Range(0, float.MaxValue, ErrorMessage = "Điểm đổi phải lớn hơn 0")]
        public float DiemDoi { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Display(Name = "Số lượng còn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm")]
        public int SoLuongCon { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình ảnh cho phần thưởng")]
        [Display(Name = "Hình ảnh")]
        public string HinhAnh { get; set; }
   

        [ValidateNever]
        // Navigation property
        public ICollection<CTPHIEUDOIQUA> CTPHIEUDOIQUAs { get; set; }
        [ValidateNever]
        // Navigation property
        public ICollection<CTKHOQUA> CTKHOQUAs { get; set; }

    }
}