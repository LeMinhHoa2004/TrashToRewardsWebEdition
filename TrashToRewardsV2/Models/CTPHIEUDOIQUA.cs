using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrashToRewardsV2.Models
{
    public class CTPHIEUDOIQUA
    {
        [Key, Column(Order = 0)]
        public int MaPDQ { get; set; }
        public PHIEUDOIQUA PHIEUDOIQUA { get; set; }

        [Key, Column(Order = 1)]
        public int MaQua { get; set; }
        public PHANTHUONG PHANTHUONG { get; set; }
        public int SoLuongDoi { get; set; }
        public float SoDiemCan { get; set; }
    }
}
