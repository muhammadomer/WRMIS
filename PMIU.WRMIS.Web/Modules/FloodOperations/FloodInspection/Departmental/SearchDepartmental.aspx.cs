using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Departmental
{
    public partial class SearchDepartmental : BasePage
    {
        #region Initialize
        private static bool _IsSaved = false;

        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }

        #region Hash Table Keys

        public const string fromDateKey = "FromDate";
        public const string toDateKey = "ToDate";
        public const string zoneIDKey = "ZoneID";
        public const string circleIDKey = "CircleID";
        public const string divisionIDKey = "DivisionID";
        public const string statusIDKey = "StatusID";
        public const string pageIndexKey = "PageIndex";
        #endregion

        #region View State keys
        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                    this.Form.DefaultButton = btnShow.UniqueID;
                    long FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);
                    if (FloodInspectionID > 0)
                    {
                        hdnFloodInspectionID.Value = FloodInspectionID.ToString();
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(FloodInspectionID).ToString();
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                    }
                    //if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    //{
                    //    bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                    //    if (ShowHistory)
                    //    {
                    //        if (Session[SessionValues.SearchFloodInspection] != null)
                    //        {
                    //            BindHistoryData();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddDays(-1)));
                    //        txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    //        BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                    //    }
                    //}
                    //else
                    //{
                    //    txtFromDate.Text = "";
                    //    txtToDate.Text = "";
                    //    BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                    //}

                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    lblStatus.Visible = true;
                    //    ddlStatus.Visible = true;
                    //}
                    //else
                    //{
                    //    lblStatus.Visible = false;
                    //    ddlStatus.Visible = true;
                    //}

                    //if (hdnInspectionStatus.Value == "2")
                    //{
                    //    DisablePublishDeleteColumn(gvSearchDepartmental);
                    //}
                    //DisableEditOnPublish(gvSearchDepartmental);

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #region DropDown


        private void BindDropdownlists()
        {
            try
            {
                //BindZoneDropdown();
                //DDLEmptyCircleDivision();
                BindUserLocation();

                if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                {
                    divStatus.Attributes.Add("style", "display:visible");
                    BindInspectionStatusDropDown();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindInspectionStatusDropDown()
        {
            try
            {
                Dropdownlist.DDLActiveInspectionStatus(ddlStatus, false, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindZoneDropdown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, 0, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
            }
        }

        private void BindCircleDropdown(long _ZoneID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.All);

            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, _ZoneID, false, (int)Constants.DropDownFirstOption.All);

            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false, (int)Constants.DropDownFirstOption.All);
            }
        }

        private void BindDivisionDropdown(long _CircleID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation || mdlUser.DesignationID == (long)Constants.Designation.SE || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false, (int)Constants.DropDownFirstOption.All);
            }
        }
        #endregion

        public void BindHistoryData()
        {
            Hashtable searchCriteria = (Hashtable)Session[SessionValues.SearchFloodInspection];
            string ZoneID = (string)searchCriteria[zoneIDKey];
            string CircleID = (string)searchCriteria[circleIDKey];
            string DivisionID = (string)searchCriteria[divisionIDKey];
            string StatusID = (string)searchCriteria[statusIDKey];

            if (ZoneID != String.Empty)
            {
                ddlZone.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlZone, ZoneID);
            }
            if (CircleID != String.Empty)
            {
                ddlCircle.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlCircle, CircleID);
            }
            if (DivisionID != string.Empty && ddlDivision.Enabled == true)
            {
                ddlDivision.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlDivision, DivisionID);
            }
            if (StatusID != String.Empty)
            {
                ddlStatus.ClearSelection();
                Dropdownlist.SetSelectedValue(ddlStatus, StatusID);
            }

            gvSearchDepartmental.PageIndex = (int)searchCriteria[pageIndexKey];
            BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
        }

        private void BindSearchResultsGrid(long _FloodInspectionID)
        {
            try
            {
                Hashtable searchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                long? SelectedStatusID = null;
                var ID = (_FloodInspectionID == 0 ? (long?)null : _FloodInspectionID);

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }
                searchCriteria.Add(fromDateKey, txtFromDate.Text.Trim());

                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }
                searchCriteria.Add(toDateKey, txtToDate.Text.Trim());

                if (FromDate != null && ToDate != null && FromDate > ToDate)
                {
                    Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                    txtToDate.Text = String.Empty;
                    return;
                }

                if (ddlZone.SelectedItem.Value != String.Empty && ddlZone.SelectedItem.Value != "0")
                {
                    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }
                searchCriteria.Add(zoneIDKey, ddlZone.SelectedItem.Value);

                if (ddlCircle.SelectedItem.Value != String.Empty && ddlCircle.SelectedItem.Value != "0")
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                searchCriteria.Add(circleIDKey, ddlCircle.SelectedItem.Value);

                if (ddlDivision.SelectedItem.Value != String.Empty && ddlDivision.SelectedItem.Value != "0")
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                searchCriteria.Add(divisionIDKey, ddlDivision.SelectedItem.Value);

                if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                {
                    if (ddlStatus.SelectedItem.Value != String.Empty && ddlStatus.SelectedItem.Value != "0")
                    {
                        SelectedStatusID = Convert.ToInt64(ddlStatus.SelectedItem.Value);
                    }
                    searchCriteria.Add(statusIDKey, ddlStatus.SelectedItem.Value);
                }
                else
                {
                    SelectedStatusID = 2;
                }

                List<object> lstDepartmentalFI = new FloodInspectionsBLL().GetDepartmentalInspectionSearch(ID, SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedStatusID, FromDate, ToDate);
                gvSearchDepartmental.DataSource = lstDepartmentalFI;
                gvSearchDepartmental.DataBind();
                searchCriteria.Add(pageIndexKey, gvSearchDepartmental.PageIndex);
                Session[SessionValues.SearchDepInspection] = searchCriteria;



            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void DisablePublishDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button hlPublish = (Button)r.FindControl("hlPublish");
                HyperLink linkButtonDelete = (HyperLink)r.FindControl("linkButtonDelete");
                hlPublish.Visible = false;
                linkButtonDelete.Visible = false;
            }
        }

        private void PublishConfirmMessage(GridViewRowEventArgs _e)
        {
            Button publish = (Button)_e.Row.FindControl("hlPublish");
            if (publish != null)
            {
                publish.OnClientClick = "return confirm('Are you sure you want to Publish this record?');";
            }
        }

        private void DisableEditOnPublish(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                DataKey key = gvSearchDepartmental.DataKeys[r.RowIndex];
                string thisInfrastructureStatusID = Convert.ToString(key.Values["InfrastructureStatus"]);

                if (thisInfrastructureStatusID.Equals("Published"))
                {
                    HyperLink hlEdit = (HyperLink)r.FindControl("hlEdit");
                    hlEdit.Visible = false;
                }
            }
        }
        #endregion

        #region Events

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    //ddlCircle.Enabled = false;
                }
                else
                {
                    DDLEmptyCircleDivision();
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);
                    //ddlCircle.Enabled = true;
                }

                ddlDivision.SelectedIndex = 0;
                //ddlDivision.Enabled = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //DropDown Events
        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    //  ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);
                    // ddlDivision.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //Button Events
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchResultsGrid(0);
                // DisableEditOnPublish(gvSearchDepartmental);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindUserLocation()
        {
            List<long> lstUserZone = new List<long>();
            List<long> lstUserCircle = new List<long>();
            List<long> lstUserDivision = new List<long>();

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
        }
        private void DDLEmptyCircleDivision()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
        }

        //Grid Events
        protected void gvSearchDepartmental_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            bool CanEditDep = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PublishConfirmMessage(e);

                #region "Data Keys"

                DataKey key = gvSearchDepartmental.DataKeys[e.Row.RowIndex];
                int status = Convert.ToInt32(key.Values["StatusID"]);
                int DepInspectionYear = Convert.ToInt32(key.Values["InspectionYear"]);
                #endregion

                #region Control

                Button hlEdit = (Button)e.Row.FindControl("hlEdit");
                Button hlPublish = (Button)e.Row.FindControl("hlPublish");
                Button linkButtonDelete = (Button)e.Row.FindControl("linkButtonDelete");
                #endregion

                linkButtonDelete.Enabled = false;
                hlEdit.Enabled = false;
                //  hlEdit.ForeColor = System.Drawing.Color.DimGray;
                if (status == 1)
                {
                    CanEditDep = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(DepInspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, 1);
                    if (CanEditDep)
                    {
                        linkButtonDelete.Enabled = CanEditDep;
                        hlPublish.Enabled = CanEditDep;
                        hlEdit.Enabled = CanEditDep;
                    }
                }
                else if (status == 2)
                {
                    CanEditDep = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(DepInspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, 2);
                    if (CanEditDep)
                    {
                        linkButtonDelete.Enabled = CanEditDep;
                        hlEdit.Enabled = CanEditDep;
                    }
                }
                if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                {
                    gvSearchDepartmental.Columns[2].Visible = true;
                }
            }
        }

        protected void gvSearchDepartmental_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long FloodInspectionID = Convert.ToInt64(gvSearchDepartmental.DataKeys[e.RowIndex].Values["FloodInspectionID"]);

                if (new FloodOperationsBLL().IsDepartmentalInspectionDependencyExists(Convert.ToInt64(FloodInspectionID)))
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = new FloodOperationsBLL().DeleteFloodInspection(FloodInspectionID);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                }

                //  bool IsDeleted = new FloodOperationsBLL().DeleteInspectionByFloodInspectionID(FloodInspectionID);  // Make new delete Functin after the discussin with Wajahat bhai 
                //if (IsDeleted)
                //{
                //    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                //    BindSearchResultsGrid(Convert.ToInt64(hdnFloodInspectionID.Value));
                //}
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchDepartmental_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvSearchDepartmental.EditIndex = -1;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchDepartmental_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSearchDepartmental.PageIndex = e.NewPageIndex;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchDepartmental_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                FO_FloodInspection floodInspection = new FO_FloodInspection();
                if (e.CommandName == "Published")
                {
                    long FloodInspectionID = Convert.ToInt64(e.CommandArgument);
                    bool IsSave = new FloodInspectionsBLL().CheckInspectionStatusByID(FloodInspectionID);
                    if (IsSave)
                        BindSearchResultsGrid(0);
                    Master.ShowMessage("Published Successfully", SiteMaster.MessageType.Success);
                    return;
                }
                else if (e.CommandName == "EditAddDep")
                {
                    long FloodInspectionID = Convert.ToInt64(e.CommandArgument);
                    Response.Redirect("AddDepartmental.aspx?FloodInspectionID=" + FloodInspectionID);
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
    }
}