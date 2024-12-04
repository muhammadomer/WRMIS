using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using System.Collections;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class SearchUser : BasePage
    {

        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        public const string UserSubDivisionKey = "UserSubDivision";
        public const string UserSectionKey = "UserSection";

        #endregion

        #region Hash Table keys

        public const string UserNameKey = "UserName";
        public const string NameKey = "Name";
        public const string StatusIDKey = "StatusID";
        public const string RoleIDKey = "RoleID";
        public const string OrganizationIDKey = "OrganizationID";
        public const string DesignationIDKey = "DesignationID";
        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string SubDivisionIDKey = "SubDivisionID";
        public const string SectionIDKey = "SectionID";
        public const string PageIndexKey = "PageIndex";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    BindRoleAndOrganizationDropdowns();
                    BindUserLocation();

                    txtUserName.Focus();
                    this.Form.DefaultButton = btnSearch.UniqueID;

                    btnAddUser.Visible = base.CanAdd;

                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchUserCriteria] != null)
                            {
                                BindHistoryData();
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["UserName"]))
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                        string UserName = Request.QueryString["UserName"];
                        txtUserName.Text = UserName;

                        BindGrid();
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 26-01-2016
        /// </summary>
        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchUser);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the Role and Organization dropdowns.
        /// Created On 22-01-2016
        /// </summary>
        public void BindRoleAndOrganizationDropdowns()
        {
            Dropdownlist.DDLRole(ddlRole, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLOrganizations(ddlOrganization, (int)Constants.DropDownFirstOption.All);
        }

        /// <summary>
        /// This function binds data to Zone, Circle, Division, SubDivision and Section according to the location of the user.
        /// Created On 26-01-2016
        /// </summary>
        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();
            List<long> lstUserSubDivision = new List<long>();
            List<long> lstUserSection = new List<long>();

            long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

            UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

            ViewState.Add(UserIDKey, mdlUser.ID);

            if (mdlUser.RoleID != Constants.AdministratorRoleID)
            {
                if (mdlUser.UA_Designations.IrrigationLevelID != null)
                {
                    List<UA_AssociatedLocation> lstAssociatedLocation = new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

                    if (lstAssociatedLocation.Count() > 0)
                    {
                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                        {
                            #region Zone Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserZone.Add((long)mdlAssociatedLocation.IrrigationBoundryID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        {
                            #region Circle Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserCircle.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Circle mdlCircle = new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserZone.Add(mdlCircle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            #region Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Division mdlDivision = new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserCircle.Add((long)mdlDivision.CircleID);
                                lstUserZone.Add(mdlDivision.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                        {
                            #region Sub Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserSubDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_SubDivision mdlSubDivision = new SubDivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserDivision.Add((long)mdlSubDivision.DivisionID);
                                lstUserCircle.Add((long)mdlSubDivision.CO_Division.CircleID);
                                lstUserZone.Add(mdlSubDivision.CO_Division.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                            ddlSubDivision.DataSource = lstSubDivision;
                            ddlSubDivision.DataTextField = "Name";
                            ddlSubDivision.DataValueField = "ID";
                            ddlSubDivision.DataBind();
                            ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                            Dropdownlist.DDLSections(ddlSection, false, SelectedSubDivisionID, (int)Constants.DropDownFirstOption.All);

                            #endregion
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
                        {
                            #region Section Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserSection.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Section mdlSection = new SectionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
                                lstUserSubDivision.Add((long)mdlSection.SubDivID);
                                lstUserDivision.Add((long)mdlSection.CO_SubDivision.DivisionID);
                                lstUserCircle.Add((long)mdlSection.CO_SubDivision.CO_Division.CircleID);
                                lstUserZone.Add(mdlSection.CO_SubDivision.CO_Division.CO_Circle.ZoneID);
                            }

                            List<CO_Zone> lstZone = new ZoneBLL().GetFilteredZones(lstUserZone);

                            long SelectedZoneID = lstZone.FirstOrDefault().ID;

                            ddlZone.DataSource = lstZone;
                            ddlZone.DataTextField = "Name";
                            ddlZone.DataValueField = "ID";
                            ddlZone.DataBind();
                            ddlZone.SelectedValue = SelectedZoneID.ToString();

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                            ddlCircle.DataSource = lstCircle;
                            ddlCircle.DataTextField = "Name";
                            ddlCircle.DataValueField = "ID";
                            ddlCircle.DataBind();
                            ddlCircle.SelectedValue = SelectedCircleID.ToString();

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                            ddlSubDivision.DataSource = lstSubDivision;
                            ddlSubDivision.DataTextField = "Name";
                            ddlSubDivision.DataValueField = "ID";
                            ddlSubDivision.DataBind();
                            ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                            List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstUserSection);

                            long SelectedSectionID = lstSection.FirstOrDefault().ID;

                            ddlSection.DataSource = lstSection;
                            ddlSection.DataTextField = "Name";
                            ddlSection.DataValueField = "ID";
                            ddlSection.DataBind();
                            ddlSection.SelectedValue = SelectedSectionID.ToString();

                            #endregion
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                }
            }
            else
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
            }

            ViewState.Add(UserCircleKey, lstUserCircle);
            ViewState.Add(UserDivisionKey, lstUserDivision);
            ViewState.Add(UserSubDivisionKey, lstUserSubDivision);
            ViewState.Add(UserSectionKey, lstUserSection);
        }

        /// <summary>
        /// This function binds the history data from session
        /// Created On 03-02-2016
        /// </summary>
        public void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.SearchUserCriteria];

            txtUserName.Text = (string)SearchCriteria[UserNameKey];
            txtFullName.Text = (string)SearchCriteria[NameKey];

            string StatusID = (string)SearchCriteria[StatusIDKey];

            if (StatusID != String.Empty)
            {
                ddlStatus.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlStatus, StatusID);
            }

            string RoleID = (string)SearchCriteria[RoleIDKey];

            if (RoleID != String.Empty)
            {
                ddlRole.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlRole, RoleID);
            }

            string OrganizationID = (string)SearchCriteria[OrganizationIDKey];

            if (OrganizationID != String.Empty)
            {
                ddlOrganization.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlOrganization, OrganizationID);
                ddlOrganization_SelectedIndexChanged(null, null);

                string DesignationID = (string)SearchCriteria[DesignationIDKey];

                if (DesignationID != String.Empty)
                {
                    ddlDesignation.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlDesignation, DesignationID);
                    ddlDesignation_SelectedIndexChanged(null, null);
                }
            }

            string ZoneID = (string)SearchCriteria[ZoneIDKey];

            if (ZoneID != String.Empty)
            {
                ddlZone.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlZone, ZoneID);
                ddlZone_SelectedIndexChanged(null, null);

                string CircleID = (string)SearchCriteria[CircleIDKey];

                if (CircleID != String.Empty)
                {
                    ddlCircle.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlCircle, CircleID);
                    ddlCircle_SelectedIndexChanged(null, null);

                    string DivisionID = (string)SearchCriteria[DivisionIDKey];

                    if (DivisionID != String.Empty)
                    {
                        ddlDivision.ClearSelection();
                        Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
                        ddlDivision_SelectedIndexChanged(null, null);

                        string SubDivisionID = (string)SearchCriteria[SubDivisionIDKey];

                        if (SubDivisionID != String.Empty)
                        {
                            ddlSubDivision.ClearSelection();
                            Dropdownlist.SetSelectedValue(ddlSubDivision, SubDivisionID);
                            ddlSubDivision_SelectedIndexChanged(null, null);

                            string SectionID = (string)SearchCriteria[SectionIDKey];

                            if (SectionID != String.Empty)
                            {
                                ddlSection.ClearSelection();
                                Dropdownlist.SetSelectedValue(ddlSection, SectionID);
                            }
                        }
                    }
                }
            }

            gvSearchUser.PageIndex = (int)SearchCriteria[PageIndexKey];

            BindGrid();
        }

        /// <summary>
        /// This function binds users to the grid according to the search criteria.
        /// Created on 27-01-2016
        /// </summary>
        public void BindGrid()
        {
            Hashtable SearchCriteria = new Hashtable();

            long SelectedDesignationID = -1;
            long SelectedOrganizationID = -1;
            long SelectedRoleID = -1;
            long SelectedZoneID = -1;
            long SelectedCircleID = -1;
            long SelectedDivisionID = -1;
            long SelectedSubDivisionID = -1;
            long SelectedSectionID = -1;

            bool? SelectedStatus = null;

            long? UserLevelID = null;
            string UserName = txtUserName.Text.Trim();
            string FullName = txtFullName.Text.Trim();

            SearchCriteria.Add(UserNameKey, UserName);
            SearchCriteria.Add(NameKey, FullName);

            long UserID = Convert.ToInt64(ViewState[UserIDKey]);

            UA_Users mdlUser = new UserBLL().GetUserByID(UserID);

            UserAdministrationBLL bllUserAdministration = new UserAdministrationBLL();

            if (ddlDesignation.SelectedItem.Value != String.Empty)
            {
                SelectedDesignationID = Convert.ToInt64(ddlDesignation.SelectedItem.Value);

                if (mdlUser.RoleID != Constants.AdministratorRoleID)
                {
                    long UserDesignationID = (long)mdlUser.DesignationID;

                    bool isReporting = bllUserAdministration.IsReportingDesignation(UserDesignationID, SelectedDesignationID);

                    if (!isReporting)
                    {
                        Master.ShowMessage(Message.NoAuthorityRights.Description, SiteMaster.MessageType.Error);
                        gvSearchUser.DataSource = new List<UA_Users>();
                        gvSearchUser.DataBind();
                        return;
                    }
                }
            }

            SearchCriteria.Add(DesignationIDKey, ddlDesignation.SelectedItem.Value);

            if (ddlOrganization.SelectedItem.Value != String.Empty)
            {
                SelectedOrganizationID = Convert.ToInt64(ddlOrganization.SelectedItem.Value);
            }

            SearchCriteria.Add(OrganizationIDKey, ddlOrganization.SelectedItem.Value);

            if (ddlRole.SelectedItem.Value != String.Empty)
            {
                SelectedRoleID = Convert.ToInt64(ddlRole.SelectedItem.Value);
            }

            SearchCriteria.Add(RoleIDKey, ddlRole.SelectedItem.Value);

            if (ddlStatus.SelectedItem.Value != String.Empty)
            {
                SelectedStatus = Convert.ToBoolean(ddlStatus.SelectedItem.Value);
            }

            SearchCriteria.Add(StatusIDKey, ddlStatus.SelectedItem.Value);

            if (divZone.Visible != false && ddlZone.SelectedItem.Value != String.Empty)
            {
                SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
            }

            SearchCriteria.Add(ZoneIDKey, ddlZone.SelectedItem.Value);

            if (divCircle.Visible != false && ddlCircle.SelectedItem.Value != String.Empty)
            {
                SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
            }

            SearchCriteria.Add(CircleIDKey, ddlCircle.SelectedItem.Value);

            if (divDivision.Visible != false && ddlDivision.SelectedItem.Value != String.Empty)
            {
                SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
            }

            SearchCriteria.Add(DivisionIDKey, ddlDivision.SelectedItem.Value);

            if (divSubDivision.Visible != false && ddlSubDivision.SelectedItem.Value != String.Empty)
            {
                SelectedSubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
            }

            SearchCriteria.Add(SubDivisionIDKey, ddlSubDivision.SelectedItem.Value);

            if (divSection.Visible != false && ddlSection.SelectedItem.Value != String.Empty)
            {
                SelectedSectionID = Convert.ToInt64(ddlSection.SelectedItem.Value);
            }

            SearchCriteria.Add(SectionIDKey, ddlSection.SelectedItem.Value);

            if (mdlUser.RoleID != Constants.AdministratorRoleID)
            {
                UserLevelID = mdlUser.UA_Designations.IrrigationLevelID;
            }

            List<UA_Users> lstUser = bllUserAdministration.GetUsers(UserName, FullName, SelectedOrganizationID, SelectedDesignationID, SelectedRoleID,
                SelectedStatus, SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedSubDivisionID, SelectedSectionID, UserID, UserLevelID,
                (long)mdlUser.RoleID);

            gvSearchUser.DataSource = lstUser;
            gvSearchUser.DataBind();

            SearchCriteria.Add(PageIndexKey, gvSearchUser.PageIndex);

            Session[SessionValues.SearchUserCriteria] = SearchCriteria;
        }

        protected void ddlOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlOrganization.SelectedItem.Value != String.Empty)
                {
                    long OrganizationID = Convert.ToInt64(ddlOrganization.SelectedItem.Value);

                    Dropdownlist.DDLDesignations(ddlDesignation, OrganizationID, (int)Constants.DropDownFirstOption.All);
                    ddlDesignation.Enabled = true;
                }
                else
                {
                    ddlDesignation.ClearSelection();
                    Dropdownlist.SetSelectedValue(ddlDesignation, "All");
                    ddlDesignation.Enabled = false;
                }

                divZone.Visible = true;
                divCircle.Visible = true;
                divDivision.Visible = true;
                divSubDivision.Visible = true;
                divSection.Visible = true;
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDesignation.SelectedItem.Value != String.Empty)
                {
                    long SelectedDesignationID = Convert.ToInt64(ddlDesignation.SelectedItem.Value);

                    UA_Designations mdlDesignation = new DesignationBLL().GetByID(SelectedDesignationID);

                    if (mdlDesignation.IrrigationLevelID != null)
                    {
                        if (mdlDesignation.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                        {
                            divZone.Visible = true;
                            divCircle.Visible = false;
                            divDivision.Visible = false;
                            divSubDivision.Visible = false;
                            divSection.Visible = false;
                        }
                        else if (mdlDesignation.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        {
                            divZone.Visible = true;
                            divCircle.Visible = true;
                            divDivision.Visible = false;
                            divSubDivision.Visible = false;
                            divSection.Visible = false;
                        }
                        else if (mdlDesignation.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            divZone.Visible = true;
                            divCircle.Visible = true;
                            divDivision.Visible = true;
                            divSubDivision.Visible = false;
                            divSection.Visible = false;
                        }
                        else if (mdlDesignation.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                        {
                            divZone.Visible = true;
                            divCircle.Visible = true;
                            divDivision.Visible = true;
                            divSubDivision.Visible = true;
                            divSection.Visible = false;
                        }
                        else if (mdlDesignation.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
                        {
                            divZone.Visible = true;
                            divCircle.Visible = true;
                            divDivision.Visible = true;
                            divSubDivision.Visible = true;
                            divSection.Visible = true;
                        }
                    }
                    else
                    {
                        divZone.Visible = false;
                        divCircle.Visible = false;
                        divDivision.Visible = false;
                        divSubDivision.Visible = false;
                        divSection.Visible = false;
                    }
                }
                else
                {
                    divZone.Visible = true;
                    divCircle.Visible = true;
                    divDivision.Visible = true;
                    divSubDivision.Visible = true;
                    divSection.Visible = true;
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem BlankEntry = new ListItem("All", "");

                if (ddlZone.SelectedValue != String.Empty)
                {
                    List<long> lstUserCircle = (List<long>)ViewState[UserCircleKey];

                    long SelectedZoneID = Convert.ToInt64(ddlZone.SelectedValue);

                    if (lstUserCircle.Count() > 0)
                    {
                        List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                        long SelectedCircleID = lstCircle.FirstOrDefault().ID;

                        ddlCircle.DataSource = lstCircle;
                        ddlCircle.DataTextField = "Name";
                        ddlCircle.DataValueField = "ID";
                        ddlCircle.DataBind();
                        ddlCircle.SelectedValue = SelectedCircleID.ToString();

                        List<long> lstUserDivision = (List<long>)ViewState[UserDivisionKey];

                        if (lstUserDivision.Count() > 0)
                        {
                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            List<long> lstUserSubDivision = (List<long>)ViewState[UserSubDivisionKey];

                            if (lstUserDivision.Count() > 0 && lstUserSubDivision.Count>0)
                            {
                                List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                                long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                                ddlSubDivision.DataSource = lstSubDivision;
                                ddlSubDivision.DataTextField = "Name";
                                ddlSubDivision.DataValueField = "ID";
                                ddlSubDivision.DataBind();
                                ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                                List<long> lstUserSection = (List<long>)ViewState[UserSectionKey];

                                if (lstUserSection.Count() > 0)
                                {
                                    List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstUserSection);

                                    long SelectedSectionID = lstSection.FirstOrDefault().ID;

                                    ddlSection.DataSource = lstSection;
                                    ddlSection.DataTextField = "Name";
                                    ddlSection.DataValueField = "ID";
                                    ddlSection.DataBind();
                                    ddlSection.SelectedValue = SelectedSectionID.ToString();
                                }
                                else
                                {
                                    Dropdownlist.DDLSections(ddlSection, false, SelectedSubDivisionID, (int)Constants.DropDownFirstOption.All);
                                }
                            }
                            else
                            {
                                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);

                                ddlSection.Items.Clear();
                                ddlSection.Items.Insert(0, BlankEntry);
                            }
                        }
                        else
                        {
                            Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                            ddlSubDivision.Items.Clear();
                            ddlSubDivision.Items.Insert(0, BlankEntry);

                            ddlSection.Items.Clear();
                            ddlSection.Items.Insert(0, BlankEntry);
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID, (int)Constants.DropDownFirstOption.All);

                        ddlDivision.Items.Clear();
                        ddlDivision.Items.Insert(0, BlankEntry);

                        ddlSubDivision.Items.Clear();
                        ddlSubDivision.Items.Insert(0, BlankEntry);

                        ddlSection.Items.Clear();
                        ddlSection.Items.Insert(0, BlankEntry);
                    }
                }
                else
                {
                    ddlCircle.Items.Clear();
                    ddlCircle.Items.Insert(0, BlankEntry);

                    ddlDivision.Items.Clear();
                    ddlDivision.Items.Insert(0, BlankEntry);

                    ddlSubDivision.Items.Clear();
                    ddlSubDivision.Items.Insert(0, BlankEntry);

                    ddlSection.Items.Clear();
                    ddlSection.Items.Insert(0, BlankEntry);
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem BlankEntry = new ListItem("All", "");

                if (ddlCircle.SelectedValue != String.Empty)
                {
                    List<long> lstUserDivision = (List<long>)ViewState[UserDivisionKey];

                    long SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedValue);

                    if (lstUserDivision.Count() > 0)
                    {
                        List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID, lstUserDivision);

                        long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                        ddlDivision.DataSource = lstDivision;
                        ddlDivision.DataTextField = "Name";
                        ddlDivision.DataValueField = "ID";
                        ddlDivision.DataBind();
                        ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                        List<long> lstUserSubDivision = (List<long>)ViewState[UserSubDivisionKey];

                        if (lstUserDivision.Count() > 0 && lstUserSubDivision.Count>0)
                        {
                            List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                            long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                            ddlSubDivision.DataSource = lstSubDivision;
                            ddlSubDivision.DataTextField = "Name";
                            ddlSubDivision.DataValueField = "ID";
                            ddlSubDivision.DataBind();
                            ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                            List<long> lstUserSection = (List<long>)ViewState[UserSectionKey];

                            if (lstUserSection.Count() > 0)
                            {
                                List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstUserSection);

                                long SelectedSectionID = lstSection.FirstOrDefault().ID;

                                ddlSection.DataSource = lstSection;
                                ddlSection.DataTextField = "Name";
                                ddlSection.DataValueField = "ID";
                                ddlSection.DataBind();
                                ddlSection.SelectedValue = SelectedSectionID.ToString();
                            }
                            else
                            {
                                Dropdownlist.DDLSections(ddlSection, false, SelectedSubDivisionID, (int)Constants.DropDownFirstOption.All);
                            }
                        }
                        else
                        {
                            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);

                            ddlSection.Items.Clear();
                            ddlSection.Items.Insert(0, BlankEntry);
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1, (int)Constants.DropDownFirstOption.All);

                        ddlSubDivision.Items.Clear();
                        ddlSubDivision.Items.Insert(0, BlankEntry);

                        ddlSection.Items.Clear();
                        ddlSection.Items.Insert(0, BlankEntry);
                    }
                }
                else
                {
                    ddlDivision.Items.Clear();
                    ddlDivision.Items.Insert(0, BlankEntry);

                    ddlSubDivision.Items.Clear();
                    ddlSubDivision.Items.Insert(0, BlankEntry);

                    ddlSection.Items.Clear();
                    ddlSection.Items.Insert(0, BlankEntry);
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem BlankEntry = new ListItem("All", "");

                if (ddlDivision.SelectedValue != String.Empty)
                {
                    List<long> lstUserSubDivision = (List<long>)ViewState[UserSubDivisionKey];

                    long SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedValue);

                    if (lstUserSubDivision.Count() > 0)
                    {
                        List<CO_SubDivision> lstSubDivision = new SubDivisionBLL().GetFilteredSubDivisions(SelectedDivisionID, lstUserSubDivision);

                        long SelectedSubDivisionID = lstSubDivision.FirstOrDefault().ID;

                        ddlSubDivision.DataSource = lstSubDivision;
                        ddlSubDivision.DataTextField = "Name";
                        ddlSubDivision.DataValueField = "ID";
                        ddlSubDivision.DataBind();
                        ddlSubDivision.SelectedValue = SelectedSubDivisionID.ToString();

                        List<long> lstUserSection = (List<long>)ViewState[UserSectionKey];

                        if (lstUserSection.Count() > 0)
                        {
                            List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstUserSection);

                            long SelectedSectionID = lstSection.FirstOrDefault().ID;

                            ddlSection.DataSource = lstSection;
                            ddlSection.DataTextField = "Name";
                            ddlSection.DataValueField = "ID";
                            ddlSection.DataBind();
                            ddlSection.SelectedValue = SelectedSectionID.ToString();
                        }
                        else
                        {
                            Dropdownlist.DDLSections(ddlSection, false, SelectedSubDivisionID, (int)Constants.DropDownFirstOption.All);
                        }
                    }
                    else
                    {
                        Dropdownlist.DDLSubDivisions(ddlSubDivision, false, SelectedDivisionID, (int)Constants.DropDownFirstOption.All);

                        ddlSection.Items.Clear();
                        ddlSection.Items.Insert(0, BlankEntry);
                    }
                }
                else
                {
                    ddlSubDivision.Items.Clear();
                    ddlSubDivision.Items.Insert(0, BlankEntry);

                    ddlSection.Items.Clear();
                    ddlSection.Items.Insert(0, BlankEntry);
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem BlankEntry = new ListItem("All", "");

                if (ddlSubDivision.SelectedValue != String.Empty)
                {
                    List<long> lstUserSection = (List<long>)ViewState[UserSectionKey];

                    long SelectedSubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedValue);

                    if (lstUserSection.Count() > 0)
                    {
                        List<CO_Section> lstSection = new SectionBLL().GetFilteredSections(SelectedSubDivisionID, lstUserSection);

                        long SelectedSectionID = lstSection.FirstOrDefault().ID;

                        ddlSection.DataSource = lstSection;
                        ddlSection.DataTextField = "Name";
                        ddlSection.DataValueField = "ID";
                        ddlSection.DataBind();
                        ddlSection.SelectedValue = SelectedSectionID.ToString();
                    }
                    else
                    {
                        Dropdownlist.DDLSections(ddlSection, false, SelectedSubDivisionID, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    ddlSection.Items.Clear();
                    ddlSection.Items.Insert(0, BlankEntry);
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                // Response.Redirect("User.aspx?PageID=" + base.PageID, false); //Add user flow Changed BY IQBAL SB 
                Response.RedirectPermanent("AddUser.aspx", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchUser_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int rowIndex = e.NewEditIndex;

                int UserID = Convert.ToInt32(((Label)gvSearchUser.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                Response.Redirect("User.aspx?UserID=" + UserID + "&PageID=" + base.PageID);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchUser.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblFullName = (Label)e.Row.FindControl("lblFullName");
                    Label lblLastName = (Label)e.Row.FindControl("lblLastName");

                    lblFullName.Text = String.Format("{0} {1}", lblFullName.Text, lblLastName.Text);

                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    bool Status = Convert.ToBoolean(lblStatus.Text);

                    ListItem StatusItem = ddlStatus.Items.FindByValue(lblStatus.Text.ToLower());
                    lblStatus.Text = StatusItem.Text;

                    HyperLink hlAssociation = (HyperLink)e.Row.FindControl("hlAssociation");
                    HyperLink hlBarrage = (HyperLink)e.Row.FindControl("hlBarrage");
                    Label lblLevelID = (Label)e.Row.FindControl("lblLevelID");

                    if (string.IsNullOrEmpty(lblLevelID.Text) || Convert.ToInt64(lblLevelID.Text) == (int)Constants.IrrigationLevelID.Office)
                    {
                        hlAssociation.Visible = false;
                        hlBarrage.Visible = false;
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