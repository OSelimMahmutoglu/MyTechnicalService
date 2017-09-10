using MTS.DAL;
using MTS.Models.Entities;
using MTS.Models.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTS.BLL.Repository
{
    public class ArizaKaydiDetayRepo : RepositoryBase<ArizaKaydiDetay, int> { }
    public class ArizaKayitRepo : RepositoryBase<ArizaKayit, int>
    {
        public List<ArizaKayit> GetById(string Id) => GetAll().Where(x => x.KullaniciId == Id).ToList();
        public int Insert(ArizaKayitViewModel model)
        {
            MyContext db = new MyContext();
            using (db.Database.BeginTransaction())
            {
                try
                {
                    ArizaKayit yeniArizaKayit = new ArizaKayit()
                    {
                        
                        Aciklama = model.Aciklama,
                        FotografYolu = model.FotografYolu,
                        KullaniciId = model.KullaniciId,
                        KategoriId = model.KategoriId,
                        Konum = model.Konum,
                        ArizaKayitZamani = model.ArizaKayitZamani,
                        ArizaGiderildiMi = model.ArizaGiderildiMi,
                    };
                    db.ArizaKayitlari.Add(yeniArizaKayit);
                    db.SaveChanges();
                        db.ArizaKayitDetaylari.Add(new ArizaKaydiDetay()
                        {
                             ArizaKayitId= yeniArizaKayit.Id
                        });
                    db.SaveChanges();
                    db.Database.CurrentTransaction.Commit();
                }
                catch (Exception ex)
                {
                    db.Database.CurrentTransaction.Rollback();
                }
                return 0;
            }
        }
    }
    public class KategoriRepo : RepositoryBase<Kategori, int> { }
}
