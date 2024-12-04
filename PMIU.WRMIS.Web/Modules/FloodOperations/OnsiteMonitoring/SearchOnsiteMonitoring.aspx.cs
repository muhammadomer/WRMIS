using PMIU.WRMIS.BLL.FloodOperations;
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
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.OnsiteMonitoring
{
    public partial class SearchOnsiteMonitoring : BasePage
    {
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

        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string YearDateKey = "Year";
        public const string InfrastructureTypeIDKey = "InfrastructureTypeID";
        public const string InfrastructureNameIDKey = "InfrastructureNameID";
        public const string PageIndexKey = "PageIndex";

        #endregion Hash Table Keys

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
                    long FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);
                    if (FFPID > 0)
                    {
                    hdnFFPID.Value = FFPID.ToString();
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        BindSearchResultsGrid(Convert.ToInt64(hdnFFPID.Value));
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Dropdown Lists Binding

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

        private void BindDropdownlists()
        {
            try
            {
                //BindZoneDropdown();
                //DDLEmptyCircleDivision();
                BindUserLocation();
                BindInfrastructureTypeDropdown();
                Dropdownlist.DDLYear(ddlYear, false, 2, -1, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindInfrastructureTypeDropdown()
        {
            try
            {
                Dropdownlist.DDLInfrastructureType(ddlInfrastructureType, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

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
                    // ddlCircle.Enabled = true;
                }

                ddlDivision.SelectedIndex = 0;
                //  ddlDivision.Enabled = false;

                // gvSearchIndependent.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    ddlDivision.SelectedIndex = 0;
                    // ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);
                    // ddlDivision.Enabled = true;
                }
                //   gvSearchIndependent.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlInfrastructureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UA_Users _Users = SessionManagerFacade.UserInformation;

            if (ddlInfrastructureType.SelectedItem.Value != "")
            {
                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 1)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1, (int)Constants.DropDownFirstOption.All);
                else if (InfrastructureTypeSelectedValue == 2)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2, (int)Constants.DropDownFirstOption.All);
                else if (InfrastructureTypeSelectedValue == 3)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3, (int)Constants.DropDownFirstOption.All);
            }
        }

        #endregion Dropdown Lists Binding

        #region Set Page Title

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set Page Title

        #region Gridview Method

        private void BindSearchResultsGrid(long _FFPID)
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                string SelectedInfrastyructureTypeID = null;
                string SelectedInfrastyructureName = null;

                if (ddlZone.SelectedItem.Value != String.Empty)
                {
                    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                }
                SearchCriteria.Add(ZoneIDKey, ddlZone.SelectedItem.Value);

                if (ddlCircle.SelectedItem.Value != String.Empty)
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                SearchCriteria.Add(CircleIDKey, ddlCircle.SelectedItem.Value);

                if (ddlDivision.SelectedItem.Value != String.Empty)
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }
                SearchCriteria.Add(DivisionIDKey, ddlDivision.SelectedItem.Value);

                if (ddlYear.SelectedItem.Value != String.Empty)
                {
                    SelectedYear = Convert.ToInt32(ddlYear.SelectedItem.Value);
                }
                SearchCriteria.Add(YearDateKey, ddlYear.SelectedItem.Value);
                if (ddlInfrastructureType.SelectedItem.Text != "All")
                {
                    SelectedInfrastyructureTypeID = Convert.ToString(ddlInfrastructureType.SelectedItem.Text) == "Barrage/Headwork" ? "Control Structure1" : Convert.ToString(ddlInfrastructureType.SelectedItem.Text);
                }

                SearchCriteria.Add(InfrastructureTypeIDKey, SelectedInfrastyructureTypeID);

                if (ddlInfrastructureName.SelectedItem.Text != "All")
                {
                    SelectedInfrastyructureName = Convert.ToString(ddlInfrastructureName.SelectedItem.Text);
                }

                SearchCriteria.Add(InfrastructureNameIDKey, ddlInfrastructureName.SelectedItem.Text);

                var FloodFightingPlanID = _FFPID == 0 ? (long?)null : _FFPID;

                //CO_StructureType structureType = new OnsiteMonitoringBLL().GetStructureInformationByStonePositionID(Convert.ToInt64(hdnStonePositionID));

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                IEnumerable<DataRow> IeOnsiteMonitoring =
                    new OnsiteMonitoringBLL().GetOnsiteMonitoringSearch(FloodFightingPlanID, SelectedInfrastyructureTypeID, SelectedInfrastyructureName, SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedYear);
                var LstOnsiteMonitoring = IeOnsiteMonitoring.Select(dataRow => new
                {
                    FFPID = dataRow.Field<long>("FFPID"),
                    Year = dataRow.Field<int>("Year"),
                    Zone = dataRow.Field<string>("Zone"),
                    Circle = dataRow.Field<string>("Circle"),
                    Division = dataRow.Field<string>("Division"),
                    InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                    InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                    DivisionID = dataRow.Field<long>("DivisionID"),
                    StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                    StructureID = dataRow.Field<long>("StructureID"),
                }).ToList();
                gvOnsiteMonitoring.DataSource = LstOnsiteMonitoring;
                gvOnsiteMonitoring.DataBind();
                SearchCriteria.Add(PageIndexKey, gvOnsiteMonitoring.PageIndex);
                Session[SessionValues.SearchOnsiteMonitoring] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOnsiteMonitoring_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvOnsiteMonitoring.EditIndex = -1;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOnsiteMonitoring_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOnsiteMonitoring.PageIndex = e.NewPageIndex;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Gridview Method

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchResultsGrid(0);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void DDLEmptyCircleDivision()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
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

        protected void gvOnsiteMonitoring_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "StonePosition")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvOnsiteMonitoring.DataKeys[row.RowIndex];
                    Response.Redirect("StonePositionOnsiteMonitoring.aspx?FFPID=" + Convert.ToInt64(key["FFPID"]) + "&Year=" + Convert.ToInt32(key["Year"]) + "&StructureTypeID=" + Convert.ToInt64(key["StructureTypeID"]) + "&StructureID=" + Convert.ToInt64(key["StructureID"]) + "&Zone=" + Convert.ToString(key["Zone"]) + "&Circle=" + Convert.ToString(key["Circle"]) + "&Division=" + Convert.ToString(key["Division"]) + "&InfrastructureType=" + Convert.ToString(key["InfrastructureType"]) + "&InfrastructureName=" + Convert.ToString(key["InfrastructureName"]));
                }
                else if (e.CommandName == "CampSite")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvOnsiteMonitoring.DataKeys[row.RowIndex];
                    Response.Redirect("ViewCampSite.aspx?FFPID=" + Convert.ToInt64(key["FFPID"]) + "&Year=" + Convert.ToInt32(key["Year"]) + "&StructureTypeID=" + Convert.ToInt64(key["StructureTypeID"]) + "&StructureID=" + Convert.ToInt64(key["StructureID"]) + "&Zone=" + Convert.ToString(key["Zone"]) + "&Circle=" + Convert.ToString(key["Circle"]) + "&Division=" + Convert.ToString(key["Division"]) + "&InfrastructureType=" + Convert.ToString(key["InfrastructureType"]) + "&InfrastructureName=" + Convert.ToString(key["InfrastructureName"]));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}