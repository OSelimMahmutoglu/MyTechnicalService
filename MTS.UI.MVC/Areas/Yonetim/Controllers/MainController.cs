using MTS.BLL.Repository;
using MTS.Models.Entities;
using MTS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace MTS.UI.MVC.Areas.Yonetim.Controllers
{
    public class MainController : Controller
    {
        // GET: Yonetim/Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Arizalar()
        {

            return View();
        }

    }
}