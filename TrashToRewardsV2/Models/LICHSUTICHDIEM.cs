using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrashToRewardsV2.Models
{
    public class LICHSUTICHDIEM
    {
        [Key]
        public int MaLS { get; set; }

        public DateTime NgayGioCapNhat { get; set; }

        public float SoDiemThayDoi { get; set; }
        public string LyDo { get; set; }
        public int MaND { get; set; }
        [ForeignKey("MaND")]
        public NGUOIDUNG NGUOIDUNG { get; set; }


    }
}
