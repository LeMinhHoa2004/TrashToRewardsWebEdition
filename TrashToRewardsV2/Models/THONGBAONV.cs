using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class THONGBAONV
    {
        [Key]
        public int MaTBNV { get; set; }

        [Required]
        public string NoiDungNV { get; set; }

        [Required]
        public DateTime NgayTaoNV { get; set; }

        public bool DaDoc { get; set; } = false;



        public int MaNV { get; set; } // ID của người dùng liên quan
        [ForeignKey("MaNV")]
        [ValidateNever]
        public NHANVIEN NHANVIEN { get; set; }


    }
}