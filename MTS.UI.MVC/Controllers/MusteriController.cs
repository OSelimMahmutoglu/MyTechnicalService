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
            return View();
        }
        [HttpPost]
        [Authorize]
       
        public ActionResult ArizaKayitSayfasi(ArizaKayitViewModel model)
        {
            model.KullaniciId = HttpContext.User.Identity.GetUserId();
            new ArizaKayitRepo().Insert(model);
            return RedirectToAction("ArizaKayitSayfasi", "Musteri");

        }


    }
}