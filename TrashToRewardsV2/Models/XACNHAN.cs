using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrashToRewardsV2.Models
{
    public class XACNHAN
    {
        [Key]
        public int MaXN { get; set; }


        [Required]
        public DateTime NgayXN { get; set; }
        
        [Required]
        public int MaNV { get; set; }
        [ForeignKey("MaNV")]
        [ValidateNever]
        public NHANVIEN NHANVIEN { get; set; }

        [Required]
        public int MaPhieuDL { get; set; }
        [ForeignKey("MaPhieuDL")]
        [ValidateNever]
        public PHIEUDATLICH PHIEUDATLICH { get; set; }
        [ValidateNever]
        [InverseProperty("XACNHAN")]
        public PHIEUTHUGOM PhieuThuGomLienKet { get; set; }

    }
}
