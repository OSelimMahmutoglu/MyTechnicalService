using Microsoft.AspNet.Identity;
using MTS.BLL.Repository;
using MTS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MTS.UI.MVC.Controllers
{
    public class OperatorController : Controller
    {
        // GET: Operator
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Operator")]
        public ActionResult OperatorArizaKayitlariGoruntuleme()
        {
            List<ArizaKayitViewModel> model = new List<ArizaKayitViewModel>();
            var arizakayitlarim = new ArizaKayitRepo().GetAll().Where(x => x.ArizaGiderildiMi == false);
            foreach (var item in arizakayitlarim)
            {
                ArizaKayitViewModel yenikayit = new ArizaKayitViewModel()
                {
                    Aciklama = item.Aciklama,
                    ArizaGiderildiMi = item.ArizaGiderildiMi,
                    ArizaKayitZamani = item.ArizaKayitZamani,
                    FotografYolu = item.FotografYolu,
                    Konum = item.Konum,
                    KullaniciId = item.KullaniciId,
                    ArizaKayitNumarasi = item.Id
                };
                model.Add(yenikayit);
            }

            return View(model);
        }
    }
}