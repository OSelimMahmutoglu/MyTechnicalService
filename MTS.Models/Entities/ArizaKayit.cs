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
    [Table("ArizaKayitlari")]
  public class ArizaKayit
    {
        [Key]
        public int Id { get; set; }
        public string Adres { get; set; }
        public string KullaniciId { get; set; }
        public int KategoriId { get; set; }
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }
        public string FotografYolu { get; set; }
        public string Konum { get; set; }
        public byte Puan  { get; set; }
        [Display(Name = "Arıza Kayıt Zamanı")]
        public DateTime ArizaKayitZamani { get; set; } = DateTime.Now;
        public bool ArizaGiderildiMi { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual ApplicationUser Kullanici { get; set; }
        [ForeignKey("KategoriId")]
        public virtual Kategori Kategori { get; set; }
        public List<ArizaKaydiDetay> ArizaKayitDetay { get; set; } = new List<ArizaKaydiDetay>();
    }
}
