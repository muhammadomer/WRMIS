using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class ChangePassword : BasePage
    {
        UserBLL blluser = new UserBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = null;
            pageTitle = base.SetPageTitle(PageName.AddUser);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        //SessionManagerFacade.UserInformation.ID;
        // UA_Users mdlUser = SessionManagerFacade.UserInformation;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users ObjUser = new UserBLL().GetUserbyID(Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                string password = Convert.ToString(WRMISEncryption.DecryptString(ObjUser.Password));
                if (password != txtoldPassword.Text)
                {
                    Master.ShowMessage("Current Password is not correct.", SiteMaster.MessageType.Error);
                    return;
                }
                blluser.UserPasswordUpdation(Convert.ToInt64(SessionManagerFacade.UserInformation.ID), Convert.ToString(WRMISEncryption.EncryptString(txtPassword.Text)));
                Master.ShowMessage(Message.PasswordChange.Description);
                cleartextbox();

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Success);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void cleartextbox()
        {
            txtoldPassword.Text="";
            txtPassword.Text="";
            txtConfirmPassword.Text="";
        }

    }
}