using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrashToRewardsV2.Models
{
    public class ADMIN
    {
        [Key]
        public int MaAD { get; set; }

        [Required]
        public string HoTenAD { get; set; }

        [Required]
        public string SDTAD { get; set; }

        [Required]
        public string EmailAD { get; set; }

        [Required]
        public string MatKhauAD { get; set; } 
    }
}
