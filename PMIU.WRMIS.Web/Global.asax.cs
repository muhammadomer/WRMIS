using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace PMIU.WRMIS.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            WRMIS.Exceptions.WRException excep = new WRMIS.Exceptions.WRException(2, exc);

            //Logging.LogMessage.LogMessageNow(2, "Application_Error - IIS Session: " + Environment.MachineName.ToString() + " - Error" + exc.Message.ToString());
            
            Response.Redirect("~/AccessDenied.aspx?Code=E");
        }
        
    }
}