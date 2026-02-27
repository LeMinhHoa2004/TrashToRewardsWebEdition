using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class LOAIPHELIEU
    {
        [Key]
        public int MaLoaiPL { get; set; }

        public string TenLoai { get; set; }

        public string DonVi { get; set; }

        public float TiLeDiem { get; set; }

        [ValidateNever]
        public ICollection<CTPHIEUTHUGOM> CTPHIEUTHUGOMs { get; set; } = new List<CTPHIEUTHUGOM>();

        [ValidateNever]
        public ICollection<CTPHIEUGHINHAN> CTPHIEUGHINHANs { get; set; } = new List<CTPHIEUGHINHAN>();
        

    }
}
