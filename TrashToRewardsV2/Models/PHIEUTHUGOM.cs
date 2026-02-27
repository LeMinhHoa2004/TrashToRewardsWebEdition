using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrashToRewardsV2.Models
{
    public class PHIEUTHUGOM
    {
        [Key]
        public int MaPhieu { get; set; }

        

        public float TongDiem { get; set; }
        [Required]
        public int MaXN { get; set; }
        [ForeignKey("MaXN")]
        [InverseProperty("PhieuThuGomLienKet")]
        public XACNHAN XACNHAN { get; set; }
        [Required]
        public int MaNV { get; set; }
        [ForeignKey("MaNV")]
        public NHANVIEN NHANVIEN { get; set; }

        public ICollection<CTPHIEUTHUGOM> CTPHIEUTHUGOMs { get; set; }
    }
}
