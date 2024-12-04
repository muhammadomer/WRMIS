using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PMIU.WRMIS.Web.Startup))]
namespace PMIU.WRMIS.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
