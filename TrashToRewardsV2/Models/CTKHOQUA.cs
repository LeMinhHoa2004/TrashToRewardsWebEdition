using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class CTKHOQUA
    {
        [Key, Column(Order = 0)]
        public int MaKQ { get; set; }
        [ValidateNever]
        public KHOQUA KHOQUA { get; set; }

        [Key, Column(Order = 1)]
        public int MaQua { get; set; }
        public PHANTHUONG PHANTHUONG { get; set; }
        public int SoLuongTrongKho { get; set; }
    }
}
