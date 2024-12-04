using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
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
    public partial class TempAssignment : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long AssignRoleID = 0;

                    if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                    {
                        AssignRoleID = Convert.ToInt64(Request.QueryString["ID"]);

                        BindUserData(AssignRoleID);
                    }
                    else
                    {
                        string Now = Utility.GetFormattedDate(DateTime.Now);
                        txtFrom.Text = Now;
                        txtTo.Text = Now;
                    }

                    SetPageTitle();
                    txtDelegatingFrom.Focus();

                    if (!string.IsNullOrEmpty(Request.QueryString["PageID"]))
                    {
                        long PageID = Convert.ToInt64(Request.QueryString["PageID"]);

                        List<UA_RoleRights> lstRoleRight = (List<UA_RoleRights>)Session[SessionValues.RoleRightList];

                        UA_RoleRights mdlRoleRights = lstRoleRight.FirstOrDefault(rr => rr.PageID == PageID);

                        if (!(bool)mdlRoleRights.BView)
                        {
                            Response.Redirect("SearchTempAssignment.aspx", false);
                        }
                        else
                        {
                            if (AssignRoleID != 0 && !(bool)mdlRoleRights.BEdit)
                            {
                                Response.Redirect("SearchTempAssignment.aspx", false);
                            }
                            else if (AssignRoleID == 0 && !(bool)mdlRoleRights.BAdd)
                            {
                                Response.Redirect("SearchTempAssignment.aspx", false);
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("SearchTempAssignment.aspx", false);
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
        /// Created on 11-01-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.TempAssignment);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the acting role data to the form.
        /// Created On: 11-01-2016
        /// </summary>
        /// <param name="_AssignRoleID"></param>
        public void BindUserData(long _ActingRoleID)
        {
            UserBLL bllUser = new UserBLL();

            UA_ActingRoles mdlActingRole = bllUser.GetActingRoleByID(_ActingRoleID);

            UA_Users mdlAssignedUser = bllUser.GetUserByID((long)mdlActingRole.AssignedUserID);

            if (mdlActingRole != null)
            {
                lblID.Text = _ActingRoleID.ToString();

                txtDelegatingFrom.Text = String.Format("{0} {1}", mdlActingRole.UA_Users.FirstName, mdlActingRole.UA_Users.LastName);
                txtFromID.Text = mdlActingRole.UserID.ToString();
                txtFromDesignationID.Text = mdlActingRole.DesignationID.ToString();

                txtDelegatingTo.Text = String.Format("{0} {1}", mdlAssignedUser.FirstName, mdlAssignedUser.LastName);
                txtToID.Text = mdlActingRole.AssignedUserID.ToString();
                txtToDesignationID.Text = mdlActingRole.AssignedDesignationID.ToString();

                txtFrom.Text = Utility.GetFormattedDate(mdlActingRole.FromDate.Value);
                txtTo.Text = Utility.GetFormattedDate(mdlActingRole.ToDate.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ActingRole"></param>
        /// <returns>bool</returns>
        private bool ValidateInput(UA_ActingRoles _ActingRole)
        {
            if (_ActingRole.UserID == _ActingRole.AssignedUserID)
            {
                Master.ShowMessage(Message.SamePersonAssignment.Description, SiteMaster.MessageType.Error);
                return false;
            }

            DateTime Now = DateTime.Now;

            if (_ActingRole.FromDate < Now.Date || _ActingRole.ToDate < Now.Date)
            {
                Master.ShowMessage(Message.DateBeforeToday.Description, SiteMaster.MessageType.Error);
                return false;
            }

            if (_ActingRole.FromDate > _ActingRole.ToDate)
            {
                Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function returns user data for the user dropdowns
        /// Created On 08-01-2016
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns>List<dynamic></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<dynamic> GetUserInfo(string _Name)
        {
            try
            {
                _Name = _Name.Trim();

                long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

                List<dynamic> lstUserInfo = new UserBLL().GetUserInfo(_Name, UserID);

                return lstUserInfo;
            }
            catch (Exception exp)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);

                return null;
            }
        }

        /// <summary>
        /// This function checks for valid assignments
        /// Created On 03-02-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns>int</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int CheckValidAssignment(string _UserID, string _FromDate, string _ToDate)
        {
            try
            {
                long UserID = Convert.ToInt64(_UserID);
                DateTime FromDate = Utility.GetParsedDate(_FromDate);
                DateTime ToDate = Utility.GetParsedDate(_ToDate);

                bool IsAssigned = new UserBLL().IsUserAssigned(UserID, FromDate, ToDate);

                if (IsAssigned)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);

                return 0;
            }
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                UA_ActingRoles mdlActingRole = new UA_ActingRoles();

                mdlActingRole.ID = Convert.ToInt64(lblID.Text);
                mdlActingRole.UserID = Convert.ToInt64(txtFromID.Text);
                mdlActingRole.DesignationID = Convert.ToInt64(txtFromDesignationID.Text);
                mdlActingRole.AssignedUserID = Convert.ToInt64(txtToID.Text);
                mdlActingRole.AssignedDesignationID = Convert.ToInt64(txtToDesignationID.Text);
                mdlActingRole.FromDate = Utility.GetParsedDate(txtFrom.Text.Trim());
                mdlActingRole.ToDate = Utility.GetParsedDate(txtTo.Text.Trim());

                if (!ValidateInput(mdlActingRole))
                {
                    return;
                }

                UserBLL bllUser = new UserBLL();

                bool IsRecordSaved = false;

                if (mdlActingRole.ID == 0)
                {
                    IsRecordSaved = bllUser.AddActingRole(mdlActingRole);
                }
                else
                {
                    IsRecordSaved = bllUser.UpdateActingRole(mdlActingRole);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    Response.Redirect("SearchTempAssignment.aspx", false);
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
                Response.Redirect("SearchTempAssignment.aspx", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}