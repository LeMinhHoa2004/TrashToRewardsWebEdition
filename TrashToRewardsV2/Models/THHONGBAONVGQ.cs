using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class THONGBAONVGQ
    {
        [Key]
        public int MaTBNVGQ { get; set; }

        [Required]
        public string NoiDungNVGQ { get; set; }

        [Required]
        public DateTime NgayTaoNVGQ { get; set; }

        public bool DaDoc { get; set; } = false;



        public int MaNVGQ { get; set; } // ID của người dùng liên quan
        [ForeignKey("MaNVGQ")]
        [ValidateNever]
        public NHANVIENGQ NHANVIENGQ { get; set; }


    }
}