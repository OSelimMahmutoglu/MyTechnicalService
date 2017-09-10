using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTS.Models.ViewModels
{
   public class KategoriViewModel
    {
        public int Id { get; set; }
        [StringLength(200)]
        [Display(Name = "Description")]
        public string Aciklama { get; set; }
        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        [Display(Name = "Category Name")]
        public string KategoriAdi { get; set; }
    }
}
