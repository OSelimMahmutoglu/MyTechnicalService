using Microsoft.AspNet.Identity;
using MTS.BLL.Repository;
using MTS.Models.Entities;
using MTS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MTS.UI.MVC.Controllers
{
    [Authorize(Roles = "Musteri")]
    public class MusteriController : Controller
    {
        // GET: Musteri
        [HttpGet]
        public ActionResult ArizaKayitSayfasi()
        {
            ViewBag.Kategori = KategoriSelectList();
            return View();
        }
        [HttpPost]
        public ActionResult ArizaKayit(ArizaKayitViewModel model)
        {
            model.KullaniciId = HttpContext.User.Identity.GetUserId();
            new ArizaKayitRepo().Insert(model);
            return RedirectToAction("Index", "Main");
        }

        private List<SelectListItem> KategoriSelectList()
        {
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            new KategoriRepo().GetAll().OrderBy(x => x.KategoriAdi).ToList().ForEach(x =>
            kategoriler.Add(new SelectListItem()
            {
                Text = x.KategoriAdi,
                Value = x.Id.ToString()
            }));
            return kategoriler;
        }
        public ActionResult ArizaKayitlariGoruntuleme()
        {
            List<ArizaKayitViewModel> model = new List<ArizaKayitViewModel>();
            var arizakayitlarim = new ArizaKayitRepo().GetById(HttpContext.User.Identity.GetUserId());
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
                     ArizaKayitNumarasi=item.Id
                };
                model.Add(yenikayit);
            }
            
            return View(model);
        }

        

    }
}