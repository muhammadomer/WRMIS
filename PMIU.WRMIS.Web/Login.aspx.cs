using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.UserAdministration;

namespace PMIU.WRMIS.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Session[SessionValues.UserID] = null;
                    Session.RemoveAll();
                    Session.Abandon();
                    if (Session[SessionValues.UserID] != null)
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                }
                catch (Exception Exp)
                {
                    new WRException(Constants.UserID, Exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {


                if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    string EncPassword = WRMISEncryption.EncryptString(txtPassword.Text.Trim());

                    LoginBLL bll = new LoginBLL();

                    //System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
                    //s.Start();

                    var user = bll.ValidateUser(txtUserName.Text, EncPassword);

                    //s.Stop();

                    //Logging.LogMessage.LogMessageNow(user.ID, "Login : Authentication Done - " + s.Elapsed.Milliseconds.ToString());
                    //Logging.LogMessage.LogMessageNow(user.ID, "Login : Authentication Done - IIS Session: " + Environment.MachineName.ToString());

                    if (user != null)
                    {
                        //s.Reset();

                        //s.Start();

                        //User log history
                        UA_LoginHistory UserLog = new UA_LoginHistory();
                        UserLog.UserID = user.ID;
                        UserLog.LoginDatetime = System.DateTime.Now;
                        UserLog.UserAgent = Utility.GetUserAgent();
                        UserLog.IPAddress = Utility.GetUserIPAddress();
                        bll.UserLog(UserLog);

                        UserAdministrationBLL bllUA = new UserAdministrationBLL();

                        UA_AssociatedLocation userLocation = bllUA.GetUserAssociateLocation(user.ID);

                        Session[SessionValues.UserID] = user.ID;
                        SessionValues.LoggedInUserID = user.ID;
                        SessionManagerFacade.UserInformation = user;
                        SessionManagerFacade.UserAssociatedLocations = userLocation;

                        UserBLL bllUser = new UserBLL();


                        List<UA_RoleRights> lstRoleRights = bllUser.GetRoleRightsByUserID(user.RoleID.Value);

                        Session[SessionValues.RoleRightList] = lstRoleRights;

                        Session[SessionValues.IsSwitchUser] = null;
                        Session[SessionValues.OriginalUserID] = null;

                        //s.Stop();

                        //Logging.LogMessage.LogMessageNow(user.ID, "Login : Default Redirection - " + s.Elapsed.Milliseconds.ToString());
                        //Logging.LogMessage.LogMessageNow(user.ID, "Login : Default Redirection - IIS Session: " + Environment.MachineName.ToString());

                        Response.Redirect("Default.aspx", false);
                    }
                    else
                    {
                        validationMessageId.Text = "Invalid credentials. Please try again.";
                        validationMessageId.Visible = true;
                    }
                }
            }
            catch (Exception Exp)
            {
                new WRException(Constants.UserID, Exp).LogException(Constants.MessageCategory.WebApp);
                validationMessageId.Text = Message.GeneralError.Description;
                validationMessageId.Visible = true;
            }
        }
    }
}