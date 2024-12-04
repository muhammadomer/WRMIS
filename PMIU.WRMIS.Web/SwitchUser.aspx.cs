using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web
{
    public partial class SwitchUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session[SessionValues.UserID] = null;
            //Session.RemoveAll();
            //Session.Abandon();

            long SwitchUserID = Convert.ToInt32(Request.QueryString["ID"].ToString());

            if (SwitchUserID != Convert.ToInt32(Session[SessionValues.OriginalUserID])) // some user to switch
            {
                Session[SessionValues.OriginalUserID] = Session[SessionValues.UserID];
                Session[SessionValues.IsSwitchUser] = "Y";
            }
            else // coming back orignal user
            {
                Session[SessionValues.OriginalUserID] = null;
                Session[SessionValues.IsSwitchUser] = null;
            }

            UA_Users user = new UserBLL().GetUserByID(SwitchUserID);
            UA_AssociatedLocation userLocation = new UserAdministrationBLL().GetUserAssociateLocation(user.ID);
            Session[SessionValues.UserID] = user.ID;
            SessionValues.LoggedInUserID = user.ID;
            SessionManagerFacade.UserInformation = user;
            SessionManagerFacade.UserAssociatedLocations = userLocation;
            List<UA_RoleRights> lstRoleRights = new UserBLL().GetRoleRightsByUserID(user.RoleID.Value);
            Session[SessionValues.RoleRightList] = lstRoleRights;
            
            
            
            Response.Redirect("Default.aspx", false);

            //ClientScript.RegisterStartupScript(Page.GetType(), "", "<script> $(\"#dvLoading\").show();</script>");
        }
    }
}