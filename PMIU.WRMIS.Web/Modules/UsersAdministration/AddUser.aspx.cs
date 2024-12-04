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
    public partial class AddUser : BasePage
    {
        static long UserID;
        static long? DesignationID;
        static long? ManagerDesignationID;
        static long UserLevelID;
        static string UserLevelName;
        static string UserName;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                Dropdownlist.DDLRole(ddlRole, (int)Constants.DropDownFirstOption.Select, true);
                Dropdownlist.DDLOrganizations(ddlOrganization, (int)Constants.DropDownFirstOption.Select, true);
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

        #region Basic Info

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
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

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

        protected void btnStep1Next_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlCurrentUser = SessionManagerFacade.UserInformation;
                DateTime Now = DateTime.Now;

                UA_Users mdlUser = new UA_Users();

                //long UserID = Convert.ToInt64(lblID.Text);
                // mdlUser.ID = UserID;
                mdlUser.ID = -1;
                mdlUser.FirstName = txtFirstName.Text.Trim();
                mdlUser.LastName = txtLastName.Text.Trim();
                mdlUser.LoginName = txtUserName.Text.Trim();
                mdlUser.Email = txtEmailAddress.Text.Trim();
                mdlUser.Password = WRMISEncryption.EncryptString(txtPassword.Text.Trim());
                mdlUser.LandLineNo = txtLandlineNumber.Text.Trim();
                mdlUser.MobilePhone = txtMobileNumber.Text.Trim();
                mdlUser.DesignationID = Convert.ToInt64(ddlDesignation.SelectedItem.Value);
                mdlUser.RoleID = Convert.ToInt64(ddlRole.SelectedItem.Value);
                mdlUser.IsActive = false;

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

                //bool IsRecordSaved = false;

                mdlUser.CreatedDate = Now;
                mdlUser.CreatedBy = mdlCurrentUser.ID;
                mdlUser.IsActive = false;

                UserID = bllUser.AddUsr(mdlUser);

                if (UserID != -1)
                {
                    //txtFirstName.Enabled = false;
                    //txtLastName.Enabled = false;
                    //txtUserName.Enabled = false;
                    //txtEmailAddress.Enabled = false;
                    //txtPassword.Enabled = false;
                    //txtConfirmPassword.Enabled = false;
                    //txtLandlineNumber.Enabled = false;
                    //txtMobileNumber.Enabled = false;
                    //ddlOrganization.Enabled = false;
                    //ddlDesignation.Enabled = false;
                    //ddlRole.Enabled = false;

                    UA_IrrigationLevel UserLevel = new UserAdministrationBLL().GetUserLevel(UserID);
                    if (UserLevel == null || UserLevel.ID == (long)Constants.IrrigationLevelID.Office)
                    {
                        bool Result = new UserBLL().ActivateUser(UserID);
                        if (Result)
                            Response.RedirectPermanent("SearchUser.aspx?UserName=" + mdlUser.LoginName, false);
                    }
                    else
                    {
                        UserLevelName = UserLevel.Name.Trim();
                        UserLevelID = UserLevel.ID;
                        DesignationID = mdlUser.DesignationID;
                        dvLocation.Visible = true;
                        btnStep1Next.Enabled = false;
                        BindData();
                    }
                }
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Associate Location

        public void BindData()
        {
            try
            {
                List<object> lstLevels = new UserAdministrationBLL().GetAssignedLevelsList(UserID, UserLevelID, UserLevelName);
                gvLocation.DataSource = lstLevels;
                gvLocation.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<object> lstLevels = new UserAdministrationBLL().GetAssignedLevelsList(UserID, UserLevelID, UserLevelName);

                    object newRow = new
                    {
                        ID = -1,
                        zoneName = "",
                        zoneID = "",
                        circleName = "",
                        circleID = "",
                        divisionName = "",
                        divisionID = "",
                        subDivisionName = "",
                        subDivisionID = "",
                        sectionName = "",
                        sectioID = ""
                    };

                    // ViewState[RecordID] = -1;
                    lstLevels.Add(newRow);
                    gvLocation.DataSource = lstLevels;
                    gvLocation.PageIndex = gvLocation.PageCount;
                    gvLocation.DataBind();

                    gvLocation.EditIndex = gvLocation.Rows.Count - 1;
                    gvLocation.DataBind();

                    btnStep2Next.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvLocation.EditIndex = -1;
                btnStep2Next.Visible = true;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvLocation.EditIndex = e.NewEditIndex;
            btnStep2Next.Visible = false;
            BindData();
            Master.HideMessageInstantly();
        }

        protected void gvLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (!CanAdd)
                    {
                        LinkButton lbtn = e.Row.FindControl("lbtnAdd") as LinkButton;
                        lbtn.Visible = false;
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvLocation.EditIndex == e.Row.RowIndex)
                    {
                        Label LabelID = e.Row.FindControl("lblID") as Label;
                        long RecordID = Convert.ToInt64(LabelID.Text);

                        if (RecordID == -1) // add new scenario
                        {
                            DropDownList ddlZone = e.Row.FindControl("ddlZone") as DropDownList;
                            DropDownList ddCircle = e.Row.FindControl("ddlCircle") as DropDownList;
                            DropDownList ddDivision = e.Row.FindControl("ddlDivision") as DropDownList;
                            DropDownList ddSubDivision = e.Row.FindControl("ddlSubDivision") as DropDownList;
                            DropDownList ddSection = e.Row.FindControl("ddlSection") as DropDownList;
                            ddCircle.Enabled = false;
                            ddDivision.Enabled = false;
                            ddSubDivision.Enabled = false;
                            ddSection.Enabled = false;

                            ddCircle.CssClass = "form-control";
                            ddDivision.CssClass = "form-control";
                            ddSubDivision.CssClass = "form-control";
                            ddSection.CssClass = "form-control";

                            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddlZone);
                            ddlZone.Enabled = true;
                        }
                        else  // edit scenario
                        {
                            //if (ViewState[UserLevel_VS] != null)
                            //{
                            LabelID = e.Row.FindControl("lblID") as Label;
                            RecordID = Convert.ToInt64(LabelID.Text);

                            DropDownList ddZone = e.Row.FindControl("ddlZone") as DropDownList;
                            DropDownList ddCircle = e.Row.FindControl("ddlCircle") as DropDownList;
                            DropDownList ddDivison = e.Row.FindControl("ddlDivision") as DropDownList;
                            DropDownList ddSubDivision = e.Row.FindControl("ddlSubDivision") as DropDownList;
                            DropDownList ddSection = e.Row.FindControl("ddlSection") as DropDownList;

                            if (UserLevelName == "Section")
                            {
                                object RecordDetail = new UserAdministrationBLL().GetUserDetail(RecordID, UserLevelName);
                                string ZoneName = RecordDetail.GetType().GetProperty("zoneName").GetValue(RecordDetail).ToString();
                                long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID").GetValue(RecordDetail));

                                string CircleName = RecordDetail.GetType().GetProperty("circleName").GetValue(RecordDetail).ToString();
                                long CircleID = Convert.ToInt64(RecordDetail.GetType().GetProperty("circleID").GetValue(RecordDetail));

                                string DivisionName = RecordDetail.GetType().GetProperty("divisionName").GetValue(RecordDetail).ToString();
                                long DivisionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("divisionID").GetValue(RecordDetail));

                                string SubDivisionName = RecordDetail.GetType().GetProperty("subDivisionName").GetValue(RecordDetail).ToString();
                                long SubDivisionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("subDivisionID").GetValue(RecordDetail));

                                string SectionName = RecordDetail.GetType().GetProperty("sectionName").GetValue(RecordDetail).ToString();
                                long SectionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("sectioID").GetValue(RecordDetail));

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                ddZone.Enabled = true;
                                ddZone.ClearSelection();
                                ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                                ddCircle.Enabled = true;
                                ddCircle.ClearSelection();
                                ddCircle.Items.FindByValue(CircleID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                ddDivison.Enabled = true;
                                ddDivison.ClearSelection();
                                ddDivison.Items.FindByValue(DivisionID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                                ddSubDivision.Enabled = true;
                                ddSubDivision.ClearSelection();
                                ddSubDivision.Items.FindByValue(SubDivisionID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddSection, false, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                                ddSection.Enabled = true;
                                ddSection.ClearSelection();
                                ddSection.Items.FindByValue(SectionID.ToString()).Selected = true;
                            }
                            else if (UserLevelName == "Sub Division")
                            {
                                object RecordDetail = new UserAdministrationBLL().GetUserDetail(RecordID, UserLevelName);
                                string ZoneName = RecordDetail.GetType().GetProperty("zoneName").GetValue(RecordDetail).ToString();
                                long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID").GetValue(RecordDetail));

                                string CircleName = RecordDetail.GetType().GetProperty("circleName").GetValue(RecordDetail).ToString();
                                long CircleID = Convert.ToInt64(RecordDetail.GetType().GetProperty("circleID").GetValue(RecordDetail));

                                string DivisionName = RecordDetail.GetType().GetProperty("divisionName").GetValue(RecordDetail).ToString();
                                long DivisionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("divisionID").GetValue(RecordDetail));

                                string SubDivisionName = RecordDetail.GetType().GetProperty("subDivisionName").GetValue(RecordDetail).ToString();
                                long SubDivisionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("subDivisionID").GetValue(RecordDetail));

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                ddZone.Enabled = true;
                                ddZone.ClearSelection();
                                ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                                ddCircle.Enabled = true;
                                ddCircle.ClearSelection();
                                ddCircle.Items.FindByValue(CircleID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                ddDivison.Enabled = true;
                                ddDivison.ClearSelection();
                                ddDivison.Items.FindByValue(DivisionID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                                ddSubDivision.Enabled = true;
                                ddSubDivision.ClearSelection();
                                ddSubDivision.Items.FindByValue(SubDivisionID.ToString()).Selected = true;
                                ddSection.Enabled = false;

                                ddSection.CssClass = "form-control";
                            }
                            else if (UserLevelName == "Division")
                            {
                                object RecordDetail = new UserAdministrationBLL().GetUserDetail(RecordID, UserLevelName);
                                string ZoneName = RecordDetail.GetType().GetProperty("zoneName").GetValue(RecordDetail).ToString();
                                long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID").GetValue(RecordDetail));

                                string CircleName = RecordDetail.GetType().GetProperty("circleName").GetValue(RecordDetail).ToString();
                                long CircleID = Convert.ToInt64(RecordDetail.GetType().GetProperty("circleID").GetValue(RecordDetail));

                                string DivisionName = RecordDetail.GetType().GetProperty("divisionName").GetValue(RecordDetail).ToString();
                                long DivisionID = Convert.ToInt64(RecordDetail.GetType().GetProperty("divisionID").GetValue(RecordDetail));

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                ddZone.Enabled = true;
                                ddZone.ClearSelection();
                                ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                                ddCircle.Enabled = true;
                                ddCircle.ClearSelection();
                                ddCircle.Items.FindByValue(CircleID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddDivison, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                                ddDivison.Enabled = true;
                                ddDivison.ClearSelection();
                                ddDivison.Items.FindByValue(DivisionID.ToString()).Selected = true;

                                ddSubDivision.Enabled = false;
                                ddSection.Enabled = false;

                                ddSubDivision.CssClass = "form-control";
                                ddSection.CssClass = "form-control";
                            }
                            else if (UserLevelName == "Circle")
                            {
                                object RecordDetail = new UserAdministrationBLL().GetUserDetail(RecordID, UserLevelName);
                                string ZoneName = RecordDetail.GetType().GetProperty("zoneName").GetValue(RecordDetail).ToString();
                                long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID").GetValue(RecordDetail));

                                string CircleName = RecordDetail.GetType().GetProperty("circleName").GetValue(RecordDetail).ToString();
                                long CircleID = Convert.ToInt64(RecordDetail.GetType().GetProperty("circleID").GetValue(RecordDetail));

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                ddZone.Enabled = true;
                                ddZone.ClearSelection();
                                ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                                ddCircle.Enabled = true;
                                ddCircle.ClearSelection();
                                ddCircle.Items.FindByValue(CircleID.ToString()).Selected = true;

                                ddDivison.Enabled = false;
                                ddSubDivision.Enabled = false;
                                ddSection.Enabled = false;

                                ddDivison.CssClass = "form-control";
                                ddSubDivision.CssClass = "form-control";
                                ddSection.CssClass = "form-control";
                            }
                            else if (UserLevelName == "Zone")
                            {
                                object RecordDetail = new UserAdministrationBLL().GetUserDetail(RecordID, UserLevelName);
                                string ZoneName = RecordDetail.GetType().GetProperty("zoneName").GetValue(RecordDetail).ToString();
                                long ZoneID = Convert.ToInt64(RecordDetail.GetType().GetProperty("zoneID").GetValue(RecordDetail));

                                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLZones(ddZone);
                                ddZone.Enabled = true;
                                ddZone.ClearSelection();
                                ddZone.Items.FindByValue(ZoneID.ToString()).Selected = true;

                                ddCircle.Enabled = false;
                                ddDivison.Enabled = false;
                                ddSubDivision.Enabled = false;
                                ddSection.Enabled = false;

                                ddCircle.CssClass = "form-control";
                                ddDivison.CssClass = "form-control";
                                ddSubDivision.CssClass = "form-control";
                                ddSection.CssClass = "form-control";

                            }
                            //  }
                        }
                    }

                    if (!CanEdit)
                    {
                        LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                        lbtnEdit.Visible = false;
                    }

                    if (!CanDelete)
                    {
                        LinkButton lbtnDel = e.Row.FindControl("lbtnDelete") as LinkButton;
                        lbtnDel.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long recordID = Convert.ToInt64(((Label)gvLocation.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool LeafSelect = false;
                bool saved = false;
                bool LocationAssigned = false;

                if (recordID == -1)
                {
                    if (UserLevelName == "Zone")
                    {
                        string ZoneName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[1].FindControl("ddlZone")).Text;
                        if (ZoneName != "Select" && ZoneName != "")
                        {
                            long ZoneID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[1].FindControl("ddlZone")).Text);
                            LeafSelect = true;
                            LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(UserLevelID, ZoneID, UserID);
                            if (!LocationAssigned)
                            {
                                saved = new UserAdministrationBLL().AssignLocation(UserID, UserLevelID, ZoneID, DesignationID);
                                if (saved)
                                    gvLocation.PageIndex = 0;
                            }
                        }
                    }
                    else if (UserLevelName == "Circle")
                    {
                        string CircleName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[2].FindControl("ddlCircle")).Text;
                        if (CircleName != "Select" && CircleName != "")
                        {
                            long CircleID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[2].FindControl("ddlCircle")).Text);
                            LeafSelect = true;
                            LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(UserLevelID, CircleID, UserID);
                            if (!LocationAssigned)
                            {
                                saved = new UserAdministrationBLL().AssignLocation(UserID, UserLevelID, CircleID, DesignationID);
                                if (saved)
                                    gvLocation.PageIndex = 0;
                            }
                        }
                    }
                    else if (UserLevelName == "Division")
                    {
                        string DivName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[3].FindControl("ddlDivision")).Text;
                        if (DivName != "Select" && DivName != "")
                        {
                            long DivisionID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[3].FindControl("ddlDivision")).Text);
                            LeafSelect = true;
                            LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(UserLevelID, DivisionID, UserID);
                            if (!LocationAssigned)
                            {
                                saved = new UserAdministrationBLL().AssignLocation(UserID, UserLevelID, DivisionID, DesignationID);
                                if (saved)
                                    gvLocation.PageIndex = 0;
                            }
                        }
                    }
                    else if (UserLevelName == "Sub Division")
                    {
                        string SubDiv = ((DropDownList)gvLocation.Rows[rowIndex].Cells[4].FindControl("ddlSubDivision")).Text;
                        if (SubDiv != "Select" && SubDiv != "")
                        {
                            long SubDivisionID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[4].FindControl("ddlSubDivision")).Text);
                            LeafSelect = true;
                            LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(UserLevelID, SubDivisionID, UserID);
                            if (!LocationAssigned)
                            {
                                saved = new UserAdministrationBLL().AssignLocation(UserID, UserLevelID, SubDivisionID, DesignationID);
                                if (saved)
                                    gvLocation.PageIndex = 0;
                            }
                        }
                    }
                    else if (UserLevelName == "Section")
                    {
                        string SecName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[5].FindControl("ddlSection")).Text;
                        if (SecName != "Select" && SecName != "")
                        {
                            long SectionID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[5].FindControl("ddlSection")).Text);
                            LeafSelect = true;
                            LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssigned(UserLevelID, SectionID, UserID);
                            if (!LocationAssigned)
                            {
                                saved = new UserAdministrationBLL().AssignLocation(UserID, UserLevelID, SectionID, DesignationID);
                                if (saved)
                                    gvLocation.PageIndex = 0;
                            }
                        }
                    }
                }
                else
                {
                    bool Result = new UserAdministrationBLL().AssociationExistAgainstLocation(UserID);
                    if (!Result)
                    {
                        if (UserLevelName == "Zone")
                        {
                            string ZoneName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[1].FindControl("ddlZone")).Text;
                            if (ZoneName != "Select" && ZoneName != "")
                            {
                                long ZoneID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[1].FindControl("ddlZone")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(UserLevelID, ZoneID, UserID, recordID);
                                if (!LocationAssigned)
                                    saved = new UserAdministrationBLL().UpdateLocation(recordID, ZoneID, Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                            }
                        }
                        else if (UserLevelName == "Circle")
                        {
                            string CircleName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[2].FindControl("ddlCircle")).Text;
                            if (CircleName != "Select" && CircleName != "")
                            {
                                long CircleID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[2].FindControl("ddlCircle")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(UserLevelID, CircleID, UserID, recordID);
                                if (!LocationAssigned)
                                    saved = new UserAdministrationBLL().UpdateLocation(recordID, CircleID, Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                            }
                        }
                        else if (UserLevelName == "Division")
                        {
                            string DivName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[3].FindControl("ddlDivision")).Text;
                            if (DivName != "Select" && DivName != "")
                            {
                                long DivisionID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[3].FindControl("ddlDivision")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(UserLevelID, DivisionID, UserID, recordID);
                                if (!LocationAssigned)
                                    saved = new UserAdministrationBLL().UpdateLocation(recordID, DivisionID, Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                            }
                        }
                        else if (UserLevelName == "Sub Division")
                        {
                            string SubDiv = ((DropDownList)gvLocation.Rows[rowIndex].Cells[4].FindControl("ddlSubDivision")).Text;
                            if (SubDiv != "Select" && SubDiv != "")
                            {
                                long SubDivisionID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[4].FindControl("ddlSubDivision")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(UserLevelID, SubDivisionID, UserID, recordID);
                                if (!LocationAssigned)
                                    saved = new UserAdministrationBLL().UpdateLocation(recordID, SubDivisionID, Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                            }
                        }
                        else if (UserLevelName == "Section")
                        {
                            string SecName = ((DropDownList)gvLocation.Rows[rowIndex].Cells[5].FindControl("ddlSection")).Text;
                            if (SecName != "Select" && SecName != "")
                            {
                                long SectionID = Convert.ToInt64(((DropDownList)gvLocation.Rows[rowIndex].Cells[5].FindControl("ddlSection")).Text);
                                LeafSelect = true;
                                LocationAssigned = new UserAdministrationBLL().LocationAlreadyAssignedUpdate(UserLevelID, SectionID, UserID, recordID);
                                if (!LocationAssigned)
                                    saved = new UserAdministrationBLL().UpdateLocation(recordID, SectionID, Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                            }
                        }
                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordAssociationsNotEdited.Description, SiteMaster.MessageType.Error);
                        LeafSelect = true;
                    }
                }

                if (saved)
                {
                    gvLocation.EditIndex = -1;
                    btnStep2Next.Visible = true;
                    BindData();
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                }

                if (LocationAssigned)
                {
                    Master.ShowMessage(Message.OneLocationMustBeAssigned.Description, SiteMaster.MessageType.Error);
                }

                if (!LeafSelect)
                {
                    Master.ShowMessage(Message.SelectionTillLeafLevel.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlZone_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                // if (ViewState[UserLevel_VS] != null)
                //  {
                if (UserLevelName == "Zone")
                {

                }
                else
                {
                    DropDownList ddlZon = (DropDownList)sender;
                    GridViewRow gvRow = (GridViewRow)ddlZon.NamingContainer;
                    DropDownList ddlZone = (DropDownList)gvRow.FindControl("ddlZone");

                    if (ddlZone.SelectedItem.Text != "Select" && ddlZone.SelectedItem.Text != "")
                    {
                        long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                        string ZoneName = ddlZone.SelectedItem.Text;

                        DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlCircle, false, ZoneID, (int)Constants.DropDownFirstOption.Select);
                        ddlCircle.Enabled = true;
                        ddlCircle.CssClass = "form-control required";

                        DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlDivision.Enabled = false;

                        DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSubDivision.Enabled = false;

                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSection.Enabled = false;
                    }
                    else
                    {
                        DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLCircles(ddlCircle, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlCircle.Enabled = false;

                        DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlDivision.Enabled = false;

                        DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSubDivision.Enabled = false;

                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSection.Enabled = false;
                    }
                }
                // }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                //if (ViewState[UserLevel_VS] != null)
                // {
                if (UserLevelName != "Circle")
                {
                    DropDownList ddlCir = (DropDownList)sender;
                    GridViewRow gvRow = (GridViewRow)ddlCir.NamingContainer;
                    DropDownList ddlCircle = (DropDownList)gvRow.FindControl("ddlCircle");

                    if (ddlCircle.SelectedItem.Value != "Select" && ddlCircle.SelectedItem.Value != "")
                    {
                        long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                        string CircleName = ddlCircle.SelectedItem.Text;

                        DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, false, CircleID, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlDivision.Enabled = true;
                        ddlDivision.CssClass = "form-control required";

                        DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSubDivision.Enabled = false;

                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSection.Enabled = false;
                    }
                    else
                    {
                        DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlDivision.Enabled = false;

                        DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSubDivision.Enabled = false;

                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSection.Enabled = false;
                    }
                }
                //  }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                //if (ViewState[UserLevel_VS] != null)
                //{
                if (UserLevelName != "Division")
                {
                    DropDownList ddlDiv = (DropDownList)sender;
                    GridViewRow gvRow = (GridViewRow)ddlDiv.NamingContainer;
                    DropDownList ddlDivision = (DropDownList)gvRow.FindControl("ddlDivision");

                    if (ddlDivision.SelectedItem.Value != "Select" && ddlDivision.SelectedItem.Value != "")
                    {
                        long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                        string DivisionName = ddlDivision.SelectedItem.Text;

                        DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, false, DivisionID, (int)Constants.DropDownFirstOption.Select);
                        ddlSubDivision.Enabled = true;
                        ddlSubDivision.CssClass = "form-control required";

                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSection.Enabled = false;
                    }
                    else
                    {
                        DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSubDivision.Enabled = false;

                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSection.Enabled = false;
                    }
                }
                // }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                //if (ViewState[UserLevel_VS] != null)
                //{
                if (UserLevelName != "Sub Division")
                {
                    DropDownList ddlSubDiv = (DropDownList)sender;
                    GridViewRow gvRow = (GridViewRow)ddlSubDiv.NamingContainer;
                    DropDownList ddlSubDivision = (DropDownList)gvRow.FindControl("ddlSubDivision");

                    if (ddlSubDivision.SelectedItem.Value != "Select" && ddlSubDivision.SelectedItem.Value != "")
                    {
                        long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
                        string SubDivisionName = ddlSubDivision.SelectedItem.Text;

                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, false, SubDivisionID, (int)Constants.DropDownFirstOption.Select);
                        ddlSection.Enabled = true;
                        ddlSection.CssClass = "form-control required";
                    }
                    else
                    {
                        DropDownList ddlSection = (DropDownList)gvRow.FindControl("ddlSection");
                        PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLSections(ddlSection, true, -1, (int)Constants.DropDownFirstOption.Select);
                        ddlSection.Enabled = false;
                    }
                }
                // }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvLocation.EditIndex = -1;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLocation.PageIndex = e.NewPageIndex;
                BindData();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long recordID = Convert.ToInt64(((Label)gvLocation.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool Result = new UserAdministrationBLL().AssociationExistAgainstLocation(UserID);
                if (!Result)
                {
                    new UserAdministrationBLL().DeleteLocation(recordID, Convert.ToInt64(SessionManagerFacade.UserInformation.ID));
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                    BindData();
                }
                else
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLocation_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Visible = (bool)mdlRoleRights.BEdit;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = (bool)mdlRoleRights.BDelete;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnStep2Next_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvLocation.Rows.Count > 0)
                {
                    LinkButton lbtnAd = (LinkButton)gvLocation.HeaderRow.FindControl("lbtnAdd");
                    if (lbtnAd != null)
                        lbtnAd.Enabled = false;

                    foreach (GridViewRow row in gvLocation.Rows)
                    {
                        LinkButton lbtn = ((LinkButton)row.FindControl("lbtnEdit"));
                        if (lbtn != null)
                            lbtn.Enabled = false;

                        Button btn = ((Button)row.FindControl("lbtnDelete"));
                        if (btn != null)
                            btn.Enabled = false;
                    }

                    btnStep2Next.Enabled = false;
                    dvUserManagers.Visible = true;

                    List<object> lstManagers = new UserBLL().GetManagers((long)DesignationID, UserID);
                    if (lstManagers.Count() > 0)
                    {
                        ddlManager.DataSource = lstManagers;
                        ddlManager.DataTextField = "Name";
                        ddlManager.DataValueField = "ID";
                        ddlManager.DataBind();
                    }
                    else
                    {
                        UA_Designations mdlDesignation = new DesignationBLL().GetByID((long)DesignationID);

                        if (mdlDesignation.ReportingToDesignationID != null)
                        {
                            List<dynamic> lstUsers = new UserBLL().GetUsersByDesignationID(mdlDesignation.ReportingToDesignationID, UserID);

                            if (lstUsers.Count() > 0)
                            {
                                ddlManager.DataSource = lstUsers;
                                ddlManager.DataTextField = "Name";
                                ddlManager.DataValueField = "ID";
                                ddlManager.DataBind();
                            }
                        }
                    }
                    //if (lstManagers.Count() > 0)
                    //    ManagerDesignationID = Convert.ToInt64(lstManagers.FirstOrDefault().GetType().GetProperty("DesignationID").GetValue(lstManagers.FirstOrDefault()));
                }
                else
                    Master.ShowMessage(Message.LocationNotSelected.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region User Manager

        protected void btnFinalSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlManager.SelectedItem != null && ddlManager.SelectedItem.Value != null)
                {
                    UA_UserManager UM = new UA_UserManager();
                    UM.UserID = UserID;
                    UM.ManagerID = Convert.ToInt64(ddlManager.SelectedItem.Value);
                    UM.IsActive = true;
                    UM.CreatedDate = DateTime.Now;
                    UM.CreatedBy = Convert.ToInt64(SessionValues.LoggedInUserID);
                    UM.ModifiedDate = DateTime.Now;
                    UM.ModifiedBy = Convert.ToInt64(SessionValues.LoggedInUserID);
                    UM.UserDesignationID = DesignationID;
                    UM.ManagerDesignationID = Convert.ToInt64(ddlManager.SelectedItem.Value);
                    bool Result = new UserBLL().AddManagerAndActivateUser(UM, UserID);
                    if (Result)
                        Response.Redirect("SearchUser.aspx?ShowHistory=true", false);
                }
                else if (ddlManager.Visible == true)
                {
                    Master.ShowMessage(Message.SelectUserManager.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion



    }
}