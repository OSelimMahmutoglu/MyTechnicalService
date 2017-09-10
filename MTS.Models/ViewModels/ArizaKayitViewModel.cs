using MTS.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MTS.Models.ViewModels
{

    public class ArizaKayitViewModel
    {
        public string KullaniciId { get; set; }
        public int KategoriId { get; set; }
        [Display(Name = "Telefon Numarası")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }
        public string FotografYolu { get; set; }
        public string Konum { get; set; }
        [Display(Name = "Arıza Kayıt Zamanı")]
        public DateTime ArizaKayitZamani { get; set; } = DateTime.Now;
        [Display(Name = "Arıza Giderildi Mi")]
        public bool ArizaGiderildiMi { get; set; } = false;
    }

}
