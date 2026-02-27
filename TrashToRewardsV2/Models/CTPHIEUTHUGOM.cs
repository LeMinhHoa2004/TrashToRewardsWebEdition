using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrashToRewardsV2.Models
{
    public class CTPHIEUTHUGOM
    {
        [Key, Column(Order = 0)]
        public int MaLoaiPL { get; set; }
        public LOAIPHELIEU LOAIPHELIEU { get; set; }

        [Key, Column(Order = 1)]
        public int MaPhieu { get; set; }
        public PHIEUTHUGOM PHIEUTHUGOM { get; set; }

        public float KhoiLuong { get; set; }

        public float SoDiemNhanDuoc { get; set; }
    }
}
