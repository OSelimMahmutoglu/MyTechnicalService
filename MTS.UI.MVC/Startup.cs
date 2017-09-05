using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MTS.UI.MVC.Startup))]
namespace MTS.UI.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
