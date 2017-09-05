using MTS.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTS.Models.Entities
{
    [Table("ArizaKayitDetaylari")]

    public class ArizaKaydiDetay
    {
        [Key]
        public int Id { get; set; }
        public int ArizaKayitId { get; set; }
        public string OperatorId { get; set; }
        public DateTime OperatorIslemZamani { get; set; }
        public bool OperatorOnayladiMi { get; set; } = false;
        [Display(Name = "Operator Açıklaması")]
        public string OperatorAciklamasi { get; set; }
        public string TeknisyenId { get; set; }
        [Display(Name = "Teknisyen Raporu")]
        public string TeknisyenRaporu { get; set; }
        public decimal Fiyat { get; set; }
        public DateTime BitisZamani { get; set; }

        [ForeignKey("ArizaKayitId")]
        public virtual ArizaKayit ArizaKayit { get; set; }
        [ForeignKey("OperatorId")]
        public virtual ApplicationUser OperatorUser { get; set; }
        [ForeignKey("TeknisyenId")]
        public virtual ApplicationUser TeknisyenUser { get; set; }



    }
}
