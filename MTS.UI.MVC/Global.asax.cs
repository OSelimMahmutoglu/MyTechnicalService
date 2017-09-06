using Microsoft.AspNet.Identity;
using MTS.BLL.Account;
using MTS.Models.IdentityModels;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MTS.UI.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var roleManager = MemberShipTools.NewRoleManager();
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Admin",
                    Description = "Site Yöneticisi"
                });
            }
            if (!roleManager.RoleExists("Musteri"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Musteri",
                    Description = "Uygulama Müşterisi"
                });
            }
            if (!roleManager.RoleExists("Operator"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Operator",
                    Description = "Uygulama yöneticisi"
                });
            }
            if (!roleManager.RoleExists("Teknisyen"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Teknisyen",
                    Description = "Teknisyenler, Tamirciler, Yetkili servisler"
                });
            }
        }
    }
}
