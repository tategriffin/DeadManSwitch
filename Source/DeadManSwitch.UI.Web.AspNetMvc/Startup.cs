using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeadManSwitch.UI.Web.AspNetMvc.Startup))]
namespace DeadManSwitch.UI.Web.AspNetMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
