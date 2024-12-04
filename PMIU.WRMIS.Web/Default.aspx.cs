using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.Web
{
    public partial class _Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Logging.LogMessage.LogMessageNow(2, "Default Page - Load", WRMIS.Common.Constants.MessageType.Info, WRMIS.Common.Constants.MessageCategory.WebApp);
            //Logging.LogMessage.LogMessageNow(2, "Default Page - Load - IIS Session: " + Environment.MachineName.ToString());

            Master.ModuleTitle = "";
            Master.PageTitle = "Dashboard";
            Master.NavigationBar = "Dashboard";

            //string PWSAdminLink = PMIU.WRMIS.Common.Utility.ReadConfiguration("PWSAdmin");
            //lnkPWSAdmin.HRef = PWSAdminLink;

            UserBLL bllUser = new UserBLL();

            UA_Users user = SessionManagerFacade.UserInformation;

            List<object> ds = bllUser.GenerateMenu_Dashboard(user.RoleID.Value);

            rpDashboard.DataSource = ds;
            rpDashboard.DataBind();
        }
    }
}