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
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class AssignRoleToUser : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    LoadRoles();
                    LoadUnAssignedRoleUsers();
                    // LoadAssignedRoleUsers(0);
                    EnableDisableControls(false);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssignRoleToUser);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadUnAssignedRoleUsers()
        {
            try
            {
                List<object> lstUsers = new UserAdministrationBLL().GetUnAssignedRoleUsers();
                lstBoxUsers.DataValueField = "ID";
                lstBoxUsers.DataTextField = "Name";
                lstBoxUsers.DataSource = lstUsers;
                lstBoxUsers.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadAssignedRoleUsers(long _RoleID)
        {
            try
            {
                List<object> bllAssignedRoleUsers = new UserAdministrationBLL().GetAssignedRoleUsers(_RoleID);
                lstBoxAssignedUsers.DataValueField = "ID";
                lstBoxAssignedUsers.DataTextField = "Name";
                lstBoxAssignedUsers.DataSource = bllAssignedRoleUsers;
                lstBoxAssignedUsers.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadRoles()
        {
            try
            {
                List<UA_Roles> lstRoles = new RolesBLL().GetAllRoles();
                Dropdownlist.BindDropdownlist<List<UA_Roles>>(ddlRoles, lstRoles);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ID = ddlRoles.SelectedItem.Value;
                if (string.IsNullOrEmpty(ID))
                {
                    EnableDisableControls(false);
                    lstBoxAssignedUsers.Items.Clear();
                }
                else
                {
                    EnableDisableControls(true);

                    long roleID = Convert.ToInt64(ID);
                    LoadUnAssignedRoleUsers();
                    LoadAssignedRoleUsers(roleID);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void EnableDisableControls(bool value)
        {
            lstBoxUsers.Enabled = value;
            lstBoxAssignedUsers.Enabled = value;
            txtFilterUsers.Enabled = value;
            txtFilterAssignedUsers.Enabled = value;
            btnAdd.Disabled = value;
            btnRemove.Disabled = value;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool AssignRoleToUsers(string _RoleID, List<string> _UserIDs)
        {
            bool IsSaved = false;
            if (_UserIDs != null && _UserIDs.Count > 0 && !string.IsNullOrEmpty(_RoleID) && _RoleID != "0")
                IsSaved = new UserAdministrationBLL().AssignRoleToUsers(Convert.ToInt64(_RoleID), _UserIDs.Select(long.Parse).ToList());

            return IsSaved;
        }
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string box = Request.Form[lstBoxAssignedUsers.UniqueID];
        //    if (lstBoxAssignedUsers.Items.Count > 0)
        //    {
        //        List<long> lstUserIDs = new List<long>();
        //        foreach (ListItem item in lstBoxAssignedUsers.Items)
        //        {
        //            lstUserIDs.Add(Convert.ToInt64(item.Value));
        //        }

        //        bool IsSaved = new UserAdministrationBLL().AssignRoleToUsers(Convert.ToInt64(ddlRoles.SelectedItem.Value), lstUserIDs);
        //        if (IsSaved)
        //            Master.ShowMessage("Successfully assigned role to users.");
        //        else
        //            Master.ShowMessage("An error occurred while assigning role to users.");
        //    }
        //}
    }
}