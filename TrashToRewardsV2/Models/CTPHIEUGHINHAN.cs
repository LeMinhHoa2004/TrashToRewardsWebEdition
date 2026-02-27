using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class CTPHIEUGHINHAN
    {
        [Key, Column(Order = 0)]
        public int MaLoaiPL { get; set; }
        [ValidateNever]
        public LOAIPHELIEU LOAIPHELIEU { get; set; }

        [Key, Column(Order = 1)]
        public int MaPhieuGN { get; set; }
        [ValidateNever]
        public PHIEUGHINHAN PHIEUGHINHAN { get; set; }
        public float KhoiLuongGN { get; set; }
        public float SoDiemNhanDuocGN { get; set; }
    }
}
