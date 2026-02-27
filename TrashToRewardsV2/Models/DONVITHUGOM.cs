using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrashToRewardsV2.Models
{
    public class DONVITHUGOM
    {
        [Key]
        public int MaDV { get; set; }

        public string TenDV { get; set; }

        public string DiaChiDV { get; set; }

        public string SDTDV { get; set; }
        [ValidateNever]
        public ICollection<NHANVIEN> NHANVIENs { get; set; }=new List<NHANVIEN>();
    }
}
