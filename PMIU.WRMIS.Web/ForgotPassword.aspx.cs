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
using PMIU.WRMIS.Logging;

namespace PMIU.WRMIS.Web
{
    public partial class ForgotPassword : System.Web.UI.Page
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

                }
                catch (Exception Exp)
                {
                    new WRException(Constants.UserID, Exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
        }

        protected void btnrecover_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users ObjUser = new UserAdministrationBLL().GetUserPasswordID(Convert.ToString(txtMobileNumber.Text));
                if (ObjUser != null)
                {
                    string password = Convert.ToString(WRMISEncryption.DecryptString(ObjUser.Password));
                    string _SMSMessage = "WRMIS Login Password: " + password + "";
                    PMIU.WRMIS.Common.Utility.SendSMS(txtMobileNumber.Text, _SMSMessage);
                    btnrecover.Visible = false;
                    validationMessageId.Text = "Password Sent at your mobile number. Please go back to login";
                    validationMessageId.Visible = true;
                }
                else
                {
                    validationMessageId.Text = "Invalid Mobile Number. Please try again.";
                    validationMessageId.Visible = true;
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