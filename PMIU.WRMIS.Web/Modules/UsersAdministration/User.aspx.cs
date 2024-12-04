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
    public partial class AddEditUser : BasePage
    {
        #region View State Keys

        public const string ReportingToDesignationID = "ReportingToDesignationID";
        static long? OrganizatnID;
        static long? DesignatnID;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindOrganizationDropdown();
                    BindRoleDropdown();

                    long UserID = 0;
                    bool ViewMode = false;

                    if (!string.IsNullOrEmpty(Request.QueryString["UserID"]))
                    {
                        UserID = Convert.ToInt64(Request.QueryString["UserID"]);

                        if (!string.IsNullOrEmpty(Request.QueryString["ViewMode"]))
                        {
                            Boolean.TryParse(Request.QueryString["ViewMode"], out ViewMode);
                        }

                        BindUserData(UserID, ViewMode);
                    }

                    txtFirstName.Focus();
                    SetPageTitle(UserID, ViewMode);
                    lblID.Text = UserID.ToString();

                    if (!string.IsNullOrEmpty(Request.QueryString["PageID"]))
                    {
                        long PageID = Convert.ToInt64(Request.QueryString["PageID"]);

                        List<UA_RoleRights> lstRoleRight = (List<UA_RoleRights>)Session[SessionValues.RoleRightList];

                        UA_RoleRights mdlRoleRights = lstRoleRight.FirstOrDefault(rr => rr.PageID == PageID);

                        if (!(bool)mdlRoleRights.BView)
                        {
                            Response.Redirect("SearchUser.aspx", false);
                        }
                        else
                        {
                            if (UserID != 0 && !(bool)mdlRoleRights.BEdit)
                            {
                                Response.Redirect("SearchUser.aspx", false);
                            }
                            else if (UserID == 0 && !(bool)mdlRoleRights.BAdd)
                            {
                                Response.Redirect("SearchUser.aspx", false);
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("SearchUser.aspx", false);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function sets the page title and description text in the master file.
        /// Created on 15-12-2015
        /// </summary>
        private void SetPageTitle(long _UserID, bool _ViewMode)
        {
            Tuple<string, string, string> pageTitle = null;

            if (_UserID == 0)
            {
                pageTitle = base.SetPageTitle(PageName.AddUser);
                ltlPageTitle.Text = "Add User";
            }
            else
            {
                if (_ViewMode)
                {
                    pageTitle = base.SetPageTitle(PageName.ViewUser);
                    ltlPageTitle.Text = "View User";
                }
                else
                {
                    pageTitle = base.SetPageTitle(PageName.EditUser);
                    ltlPageTitle.Text = "Edit User";
                }
            }

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// The function binds data to the form for edit and view cases.
        /// Created On 28-12-2015
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_ViewMode"></param>
        public void BindUserData(long _UserID, bool _ViewMode)
        {
            UserBLL bllUser = new UserBLL();

            UA_Users mdlUser = bllUser.GetUserByID(_UserID);

            if (mdlUser != null)
            {
                txtFirstName.Text = mdlUser.FirstName;
                txtLastName.Text = mdlUser.LastName;

                txtUserName.Text = mdlUser.LoginName;
                txtUserName.ReadOnly = true;
                txtUserName.CssClass = "form-control";

                txtEmailAddress.Text = mdlUser.Email;
                txtPassword.Text = WRMISEncryption.DecryptString(mdlUser.Password);
                txtConfirmPassword.Text = WRMISEncryption.DecryptString(mdlUser.Password);
                divPassword.Visible = false;

                if (mdlUser.LandLineNo != null)
                {
                    txtLandlineNumber.Text = mdlUser.LandLineNo;
                }

                txtMobileNumber.Text = mdlUser.MobilePhone;

                OrganizatnID = mdlUser.UA_Designations.OrganizationID;
                DesignatnID = mdlUser.DesignationID;

                Dropdownlist.SetSelectedValue(ddlOrganization, mdlUser.UA_Designations.OrganizationID.ToString());

                ddlOrganization_SelectedIndexChanged(null, null);

                Dropdownlist.SetSelectedValue(ddlDesignation, mdlUser.DesignationID.ToString());

                ddlDesignation_SelectedIndexChanged(null, null);

                UA_UserManager mdlManager = bllUser.GetUserManager(_UserID);

                if (mdlManager != null)
                {
                    ViewState.Add(ReportingToDesignationID, mdlManager.ManagerDesignationID.ToString());

                    if (ddlManager.Items.FindByValue(mdlManager.ManagerID.ToString()) != null)
                    {
                        Dropdownlist.SetSelectedValue(ddlManager, mdlManager.ManagerID.ToString());
                    }
                }

                Dropdownlist.SetSelectedValue(ddlRole, mdlUser.RoleID.ToString());

                if ((bool)mdlUser.IsActive)
                {
                    rbStatusActive.Checked = true;
                }
                else
                {
                    rbStatusInactive.Checked = true;
                }

                if (_ViewMode)
                {
                    txtFirstName.ReadOnly = true;
                    txtFirstName.CssClass = "form-control";

                    txtLastName.ReadOnly = true;
                    txtLastName.CssClass = "form-control";

                    txtEmailAddress.ReadOnly = true;
                    txtEmailAddress.CssClass = "form-control";

                    divPassword.Style.Add(HtmlTextWriterStyle.Display, "None");

                    txtLandlineNumber.ReadOnly = true;
                    txtLandlineNumber.CssClass = "form-control";

                    txtMobileNumber.ReadOnly = true;
                    txtMobileNumber.CssClass = "form-control";

                    txtFirstName.ReadOnly = true;
                    txtFirstName.CssClass = "form-control";

                    ddlOrganization.Enabled = false;
                    ddlOrganization.CssClass = "form-control";

                    ddlDesignation.Enabled = false;
                    ddlDesignation.CssClass = "form-control";

                    ddlManager.Enabled = false;
                    ddlManager.CssClass = "form-control";

                    ddlRole.Enabled = false;
                    ddlRole.CssClass = "form-control";

                    rbStatusActive.Disabled = true;
                    rbStatusInactive.Disabled = true;

                    btnSave.Visible = false;
                }
            }
        }

        /// <summary>
        /// This function binds data to the Organization dropdown
        /// Created On 18-12-2015
        /// </summary>
        private void BindOrganizationDropdown()
        {
            Dropdownlist.DDLOrganizations(ddlOrganization, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds data to the Role dropdown
        /// Created On 18-12-2015
        /// </summary>
        private void BindRoleDropdown()
        {
            Dropdownlist.DDLRole(ddlRole, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function check for UserName duplication.
        /// Created On 23-12-2015
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_UserName"></param>
        /// <returns>bool</returns>
        private bool IsValidUserName(long _UserID, string _UserName)
        {
            UserBLL bllUser = new UserBLL();

            bool IsExists = bllUser.IsUserNameExists(_UserID, _UserName);

            if (IsExists)
            {
                Master.ShowMessage(Message.UserNameExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function check for UserName duplication.
        /// Created On 14-04-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_UserName"></param>
        /// <returns>bool</returns>
        private bool IsValidUserEmail(long _UserID, string _UserEmail)
        {
            UserBLL bllUser = new UserBLL();

            bool IsExists = bllUser.IsUserEmailExists(_UserID, _UserEmail);

            if (IsExists)
            {
                Master.ShowMessage(Message.UserEmailExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function check for UserMobile duplication.
        /// Created On 14-04-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_UserMobile"></param>
        /// <returns>bool</returns>
        private bool IsValidUserMobile(long _UserID, string _UserMobile)
        {
            UserBLL bllUser = new UserBLL();

            bool IsExists = bllUser.IsUserMobileExists(_UserID, _UserMobile);

            if (IsExists)
            {
                Master.ShowMessage(Message.UserMobileExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function resets the fields on the form after saving user
        /// Created On 25-01-2015
        /// </summary>
        private void ResetForm()
        {
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtUserName.Text = String.Empty;
            txtEmailAddress.Text = String.Empty;
            txtPassword.Text = String.Empty;
            txtConfirmPassword.Text = String.Empty;
            txtLandlineNumber.Text = String.Empty;
            txtMobileNumber.Text = String.Empty;
            ddlOrganization.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            ddlDesignation.Enabled = false;
            ddlRole.SelectedIndex = 0;
            rbStatusActive.Checked = false;
            rbStatusInactive.Checked = false;
        }

        protected void ddlOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlOrganization.SelectedItem.Value == String.Empty)
                {
                    ddlDesignation.SelectedItem.Selected = false;
                    Dropdownlist.SetSelectedText(ddlDesignation, "Select");
                    ddlDesignation.Enabled = false;
                }
                else
                {
                    long OrganizationID = Convert.ToInt64(ddlOrganization.SelectedItem.Value);

                    Dropdownlist.DDLDesignations(ddlDesignation, OrganizationID, (int)Constants.DropDownFirstOption.Select, true);
                    ddlDesignation.Enabled = true;
                }

                ddlManager.SelectedItem.Selected = false;
                Dropdownlist.SetSelectedText(ddlManager, "Select");
                ddlManager.Enabled = false;

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlCurrentUser = SessionManagerFacade.UserInformation;
                DateTime Now = DateTime.Now;

                UA_Users mdlUser = new UA_Users();

                long UserID = Convert.ToInt64(lblID.Text);

                mdlUser.ID = UserID;
                mdlUser.FirstName = txtFirstName.Text.Trim();
                mdlUser.LastName = txtLastName.Text.Trim();
                mdlUser.LoginName = txtUserName.Text.Trim();
                mdlUser.Email = txtEmailAddress.Text.Trim();
                mdlUser.Password = WRMISEncryption.EncryptString(txtPassword.Text.Trim());
                mdlUser.LandLineNo = txtLandlineNumber.Text.Trim();
                mdlUser.MobilePhone = txtMobileNumber.Text.Trim();
                mdlUser.DesignationID = Convert.ToInt64(ddlDesignation.SelectedItem.Value);
                mdlUser.RoleID = Convert.ToInt64(ddlRole.SelectedItem.Value);
                mdlUser.IsActive = rbStatusActive.Checked;

                mdlUser.ModifiedDate = Now;
                mdlUser.ModifiedBy = mdlCurrentUser.ID;

                if (!IsValidUserName(mdlUser.ID, mdlUser.LoginName))
                {
                    return;
                }

                if (mdlUser.Email != "" && !IsValidUserEmail(mdlUser.ID, mdlUser.Email))
                {
                    return;
                }

                if (!IsValidUserMobile(mdlUser.ID, mdlUser.MobilePhone))
                {
                    return;
                }

                UserBLL bllUser = new UserBLL();

                bool IsRecordSaved = false;

                if (UserID == 0)
                {
                    mdlUser.CreatedDate = Now;
                    mdlUser.CreatedBy = mdlCurrentUser.ID;
                    mdlUser.IsActive = true;

                    IsRecordSaved = bllUser.AddUser(mdlUser);
                }
                else
                {
                    IsRecordSaved = bllUser.UpdateUser(mdlUser, DesignatnID);
                }

                if (IsRecordSaved)
                {
                    UA_UserManager mdlUserManager = null;

                    if (ddlManager.SelectedItem.Value != string.Empty)
                    {
                        if (UserID == 0)
                        {
                            mdlUser = bllUser.GetUserByUserName(mdlUser.LoginName);

                            mdlUserManager = new UA_UserManager
                            {
                                UserID = mdlUser.ID,
                                UserDesignationID = mdlUser.DesignationID,
                                ManagerID = Convert.ToInt64(ddlManager.SelectedItem.Value),
                                ManagerDesignationID = Convert.ToInt64(ViewState[ReportingToDesignationID]),
                                CreatedDate = Now,
                                ModifiedDate = Now,
                                CreatedBy = mdlCurrentUser.ID,
                                ModifiedBy = mdlCurrentUser.ID,
                                IsActive = true
                            };

                            bllUser.AddUserManager(mdlUserManager);
                        }
                        else
                        {
                            mdlUserManager = bllUser.GetUserManager(UserID);

                            if (mdlUserManager == null)
                            {
                                mdlUserManager = new UA_UserManager
                                {
                                    UserID = mdlUser.ID,
                                    UserDesignationID = mdlUser.DesignationID,
                                    ManagerID = Convert.ToInt64(ddlManager.SelectedItem.Value),
                                    ManagerDesignationID = Convert.ToInt64(ViewState[ReportingToDesignationID]),
                                    CreatedDate = Now,
                                    ModifiedDate = Now,
                                    CreatedBy = mdlCurrentUser.ID,
                                    ModifiedBy = mdlCurrentUser.ID,
                                    IsActive = true
                                };

                                bllUser.AddUserManager(mdlUserManager);
                            }
                            else
                            {
                                mdlUserManager.UserID = mdlUser.ID;
                                mdlUserManager.UserDesignationID = mdlUser.DesignationID;
                                mdlUserManager.ManagerID = Convert.ToInt64(ddlManager.SelectedItem.Value);
                                mdlUserManager.ManagerDesignationID = Convert.ToInt64(ViewState[ReportingToDesignationID]);
                                mdlUserManager.ModifiedDate = Now;
                                mdlUserManager.ModifiedBy = mdlCurrentUser.ID;

                                bllUser.UpdateUserManager(mdlUserManager);
                            }
                        }
                    }
                    else
                    {
                        mdlUserManager = bllUser.GetUserManager(UserID);

                        if (mdlUserManager != null)
                        {
                            bllUser.DeleteUserManager(mdlUserManager.ID);
                        }
                    }

                    string Url = String.Format("SearchUser.aspx?UserName={0}", mdlUser.LoginName);
                    Response.Redirect(Url, false);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Success);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SearchUser.aspx?ShowHistory=true");
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlManager.CssClass = "form-control required";

                if (ddlDesignation.SelectedItem.Value == string.Empty)
                {
                    ddlManager.SelectedItem.Selected = false;
                    Dropdownlist.SetSelectedText(ddlManager, "Select");
                    ddlManager.Enabled = false;
                }
                else
                {
                    long DesignationID = Convert.ToInt64(ddlDesignation.SelectedItem.Value);

                    if (OrganizatnID != Convert.ToInt64(ddlOrganization.SelectedItem.Value)
                        || (OrganizatnID == Convert.ToInt64(ddlOrganization.SelectedItem.Value) && DesignatnID != Convert.ToInt64(ddlDesignation.SelectedItem.Value)))
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scriptname2", "alert('Change in designation also remove location allocations for current user(if exist).');", true);

                    //{
                    //    btnSave.Attributes.Add("onclick", "javascript:return Test()");
                    //}

                    UA_Designations mdlDesignation = new DesignationBLL().GetByID(DesignationID);

                    ddlManager.SelectedItem.Selected = false;
                    Dropdownlist.SetSelectedText(ddlManager, "Select");
                    ddlManager.Enabled = false;

                    if (mdlDesignation.ReportingToDesignationID != null)
                    {
                        ViewState.Add(ReportingToDesignationID, mdlDesignation.ReportingToDesignationID);

                        long UserID = Convert.ToInt64(lblID.Text);

                        List<dynamic> lstUsers = new UserBLL().GetUsersByDesignationID(mdlDesignation.ReportingToDesignationID, UserID);

                        if (lstUsers.Count() > 0)
                        {
                            ddlManager.DataSource = lstUsers;
                            ddlManager.DataTextField = "Name";
                            ddlManager.DataValueField = "ID";
                            ddlManager.DataBind();

                            ddlManager.Items.Insert(0, new ListItem("Select", ""));

                            ddlManager.Enabled = true;
                        }
                        else
                        {
                            ddlManager.CssClass = "form-control";
                        }
                    }
                    else
                    {
                        ddlManager.CssClass = "form-control";
                    }
                }


            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}