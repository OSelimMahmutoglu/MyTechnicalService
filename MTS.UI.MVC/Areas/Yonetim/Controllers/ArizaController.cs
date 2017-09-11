using MTS.BLL.Repository;
using MTS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MTS.UI.MVC.Areas.Yonetim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArizaController : Controller
    {
        // GET: Yonetim/Ariza
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Arizalar()
        {
            var model = new ArizaKayitRepo().GetAll().OrderBy(x => x.ArizaKayitZamani).Select(x => new ArizaKayitViewModel()
            {
                KullaniciId = x.KullaniciId,
                Aciklama = x.Aciklama,
                Konum = x.Konum,
                ArizaKayitZamani = x.ArizaKayitZamani,
                KategoriId = x.KategoriId,
                KategoriAdi=x.Kategori.KategoriAdi,
                KullaniciAdi=x.Kullanici.UserName
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}