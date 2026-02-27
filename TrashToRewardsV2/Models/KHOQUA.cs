using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class KHOQUA
    {
        [Key]
        public int MaKQ { get; set; }

        public string TenKQ { get; set; }

        public string DiaChiKQ { get; set; }

        public string SDTKQ { get; set; }
        [ValidateNever]
        public ICollection<NHANVIENGQ> NHANVIENGQs { get; set; } = new List<NHANVIENGQ>();
        [ValidateNever]
        public ICollection<CTKHOQUA> CTKHOQUAs { get; set; } 
    }
}
