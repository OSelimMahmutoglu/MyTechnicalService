using Microsoft.AspNet.Identity.EntityFramework;
using MTS.Models.Entities;
using MTS.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTS.DAL
{
   public class MyContext: IdentityDbContext<ApplicationUser>
    {
        public MyContext()
            : base("name=MyCon")
        {        }
        public virtual DbSet<ArizaKaydiDetay> ArizaKayitDetaylari { get; set; }
        public virtual DbSet<ArizaKayit> ArizaKayitlari { get; set; }
        public virtual DbSet<Kategori> Kategoriler { get; set; }
    }
}
