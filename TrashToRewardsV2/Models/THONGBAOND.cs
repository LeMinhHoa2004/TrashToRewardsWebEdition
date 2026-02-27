using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class THONGBAOND
    {
        [Key]
        public int MaTBND { get; set; }

        [Required]
        public string NoiDungND { get; set; }

        [Required]
        public DateTime NgayTaoND { get; set; }

        public bool DaDoc { get; set; } = false;



        public int MaND { get; set; } // ID của người dùng liên quan
        [ForeignKey("MaND")]
        [ValidateNever]
        public NGUOIDUNG NGUOIDUNG { get; set; }

        
    }
}