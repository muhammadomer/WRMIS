using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PMIU.WRMIS.PWS.Startup))]
namespace PMIU.WRMIS.PWS
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
