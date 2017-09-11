using MTS.BLL.Repository;
using MTS.Models.Entities;
using MTS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MTS.UI.MVC.Areas.Yonetim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KategoriController : Controller
    {
        // GET: Yonetim/Kategori
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Ekle(KategoriViewModel model)
        {
            try
            {
                new KategoriRepo().Insert(new Kategori()
                {
                    KategoriAdi = model.KategoriAdi,
                    Aciklama = model.Aciklama
                });
                var data = new
                {
                    success = true,
                    message = "Kategori ekleme işlemi başarılı"
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new
                {
                    success = false,
                    message = "Kategori ekleme işlemi başarısız: " + ex.Message
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Getir()
        {
            var model = new KategoriRepo().GetAll().OrderBy(x => x.KategoriAdi).Select(x => new KategoriViewModel()
            {
                Id = x.Id,
                Aciklama = x.Aciklama,
                KategoriAdi = x.KategoriAdi
            }).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Guncelle(KategoriViewModel model)
        {
            try
            {
                var kategori = new KategoriRepo().GetById(model.Id);
                kategori.KategoriAdi = model.KategoriAdi;
                kategori.Aciklama = model.Aciklama;
                new KategoriRepo().Update();
                var data = new
                {
                    success = true,
                    message = "Kategori Güncelleme işlemi başarılı"
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new
                {
                    success = false,
                    message = "Kategori Güncelleme işlemi başarısız: " + ex.Message
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Sil(int? id)
        {
            try
            {
                var kategori = new KategoriRepo().GetById(id.Value);
                new KategoriRepo().Delete(kategori);
                var data = new
                {
                    success = true,
                    message = "Kategori Silme işlemi başarılı"
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new
                {
                    success = false,
                    message = "Kategori Silme işlemi başarısız :" + ex.Message
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}