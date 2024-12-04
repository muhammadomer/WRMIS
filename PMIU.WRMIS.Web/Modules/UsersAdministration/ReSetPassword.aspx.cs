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
    public partial class ReSetPassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                BindData(Convert.ToInt64(Request.QueryString["UserID"]));
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

        public void BindData(long _UserID)
        {
            try
            {
                UA_Users UserDetail = new UserBLL().GetUserByID(_UserID);
                if (UserDetail != null)
                {
                    lblUserName.Text = UserDetail.LoginName.ToString();
                    lblFirstName.Text = UserDetail.FirstName.ToString();
                    lblLastName.Text = UserDetail.LastName.ToString();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool Result = new UserBLL().ResetPassword(Convert.ToInt64(Request.QueryString["UserID"]), WRMISEncryption.EncryptString(txtPassword.Text.Trim()));
                if (Result)
                {
                    string Url = String.Format("SearchUser.aspx?UserName={0}", lblUserName.Text);
                    Response.Redirect(Url, false);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SearchUser.aspx?ShowHistory=true", false);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}