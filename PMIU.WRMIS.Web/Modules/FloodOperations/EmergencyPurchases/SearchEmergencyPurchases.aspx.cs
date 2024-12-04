using PMIU.WRMIS.AppBlocks;
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
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases
{
    public partial class SearchEmergencyPurchases : BasePage
    {
        private static bool _IsSaved = false;

        public static bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        #region Hash Table Keys

        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string YearDateKey = "Year";
        public const string CampSiteIDKey = "CampSiteID";
        public const string PageIndexKey = "PageIndex";

        #endregion Hash Table Keys

        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys

        #region PageLoad

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                    this.Form.DefaultButton = btnShow.UniqueID;
                    long EmergencyPurchasesID = Utility.GetNumericValueFromQueryString("EmergencyPurchaseID", 0);
                    if (EmergencyPurchasesID > 0)
                    {
                        hdnEmergencyPurchasesID.Value = EmergencyPurchasesID.ToString();
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        BindSearchResultsGrid(Convert.ToInt64(hdnEmergencyPurchasesID.Value));
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion PageLoad

        #region Dropdown Lists Binding

        private void BindZoneDropdown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation ||
                mdlUser.DesignationID == (long)Constants.Designation.SE ||
                mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID,
                    (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, 0, (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLZoneByUserID(ddlZone, mdlUser.ID, (long)mdlUser.UA_Designations.IrrigationLevelID,
                    (int)Constants.DropDownFirstOption.All);
            }
        }

        private void BindCircleDropdown(long _ZoneID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation ||
                mdlUser.DesignationID == (long)Constants.Designation.SE ||
                mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID,
                    (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false,
                    (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, _ZoneID, false,
                    (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID,
                    (long)mdlUser.UA_Designations.IrrigationLevelID, _ZoneID, false,
                    (int)Constants.DropDownFirstOption.All);
            }
        }

        private void BindDivisionDropdown(long _CircleID)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ChiefIrrigation ||
                mdlUser.DesignationID == (long)Constants.Designation.SE ||
                mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID,
                    (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false,
                    (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, _CircleID, false,
                    (int)Constants.DropDownFirstOption.All);
            }
            else if (mdlUser.DesignationID != null)
            {
                Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID,
                    (long)mdlUser.UA_Designations.IrrigationLevelID, _CircleID, false,
                    (int)Constants.DropDownFirstOption.All);
            }
        }

        private void BindDropdownlists()
        {
            try
            {
                //BindZoneDropdown();
                //DDLEmptyCircleDivision();
                BindUserLocation();
                Dropdownlist.DDLYesNo(ddlCampSite, (int)Constants.DropDownFirstOption.All);
                //Dropdownlist.DDLEmergencyPurchasesYear(ddlYear);
                Dropdownlist.DDLYear(ddlYear, false, 0, 2015, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
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

        private void BindSearchResultsGrid(long _EmergencyPurchasesID)
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                bool? SelectedCompSiteID = null;

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

                if (ddlCampSite.SelectedItem.Value != String.Empty)
                {
                    SelectedCompSiteID = Convert.ToBoolean(Convert.ToUInt16((ddlCampSite.SelectedItem.Value)));
                }

                SearchCriteria.Add(CampSiteIDKey, ddlCampSite.SelectedItem.Value);

                var EmergencyPurchaseID = _EmergencyPurchasesID == 0 ? (long?)null : _EmergencyPurchasesID;

                CO_StructureType structureType =
                    new FloodOperationsBLL().GetStructureInformationByEmergencyPurchaseID(EmergencyPurchaseID);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                IEnumerable<DataRow> IeEmergencyPurchase =
                    new FloodOperationsBLL().GetEmergencyPurchasesSearch(
                        structureType == null ? null : structureType.Source, EmergencyPurchaseID, SelectedZoneID,
                        SelectedCircleID, SelectedDivisionID, SelectedYear, SelectedCompSiteID);
                var LstEmergencyPurchase = IeEmergencyPurchase.Select(dataRow => new
                {
                    EmergencyPurchaseID = dataRow.Field<long>("EmergencyPurchaseID"),
                    InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                    InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                    CompSite = dataRow.Field<bool>("CompSite"),
                    RD = Calculations.GetRDText(Convert.ToInt64(dataRow.Field<int?>("RD"))),
                    EmergencyCreatedDate = dataRow.Field<DateTime>("EmergencyCreatedDate"),
                    Year = dataRow.Field<long>("Year")
                }).ToList();
                gvEmergencyPurchases.DataSource = LstEmergencyPurchase;
                gvEmergencyPurchases.DataBind();
                SearchCriteria.Add(PageIndexKey, gvEmergencyPurchases.PageIndex);
                Session[SessionValues.SearchEmergencyPurchase] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEmergencyPurchases_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvEmergencyPurchases.EditIndex = -1;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEmergencyPurchases_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvEmergencyPurchases.PageIndex = e.NewPageIndex;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEmergencyPurchases_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long EmergencyPurchasesID =
                    Convert.ToInt64(
                        ((Label)gvEmergencyPurchases.Rows[e.RowIndex].FindControl("lblEmergencyPurchasesID")).Text);

                if (new FloodOperationsBLL().IsEmergencyPurchaseDependencyExists(Convert.ToInt64(EmergencyPurchasesID)))
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = new FloodOperationsBLL().DeleteJointEmergencyPurchase(EmergencyPurchasesID);

                if (IsDeleted)
                {
                    BindSearchResultsGrid(0);
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
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
            Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, -1, true,
                (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, -1, true,
                (int)Constants.DropDownFirstOption.All);
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
                    List<UA_AssociatedLocation> lstAssociatedLocation =
                        new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

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

                            Dropdownlist.DDLCircles(ddlCircle, false, SelectedZoneID,
                                (int)Constants.DropDownFirstOption.All);

                            #endregion Zone Level Bindings
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        {
                            #region Circle Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserCircle.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Circle mdlCircle =
                                    new CircleBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
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

                            Dropdownlist.DDLDivisions(ddlDivision, false, SelectedCircleID, -1,
                                (int)Constants.DropDownFirstOption.All);

                            #endregion Circle Level Bindings
                        }
                        else if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            #region Division Level Bindings

                            foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                            {
                                lstUserDivision.Add((long)mdlAssociatedLocation.IrrigationBoundryID);

                                CO_Division mdlDivision =
                                    new DivisionBLL().GetByID((long)mdlAssociatedLocation.IrrigationBoundryID);
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

                            List<CO_Division> lstDivision = new DivisionBLL().GetFilteredDivisions(SelectedCircleID,
                                lstUserDivision);

                            long SelectedDivisionID = lstDivision.FirstOrDefault().ID;

                            ddlDivision.DataSource = lstDivision;
                            ddlDivision.DataTextField = "Name";
                            ddlDivision.DataValueField = "ID";
                            ddlDivision.DataBind();
                            ddlDivision.SelectedValue = SelectedDivisionID.ToString();

                            #endregion Division Level Bindings
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

        protected void gvEmergencyPurchases_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    UA_SystemParameters systemParameters = null;
                    bool CanAddEditEP = false;
                    DataKey key = gvEmergencyPurchases.DataKeys[e.Row.RowIndex];
                    string RD = Convert.ToString(key.Values["RD"]);
                    string StructureType = Convert.ToString(key["InfrastructureType"]);
                    int EmergencyPurchasesYear = Convert.ToInt32(key["Year"]);
                    Label lblRD = (Label)e.Row.FindControl("lblRD");
                    Label lblCampSite = (Label)e.Row.FindControl("lblCampSite");
                    Button btnEditEmergencyPurchase = (Button)e.Row.FindControl("btnEditEmergencyPurchase");
                    Button btnDeleteEmergencyPurchase = (Button)e.Row.FindControl("btnDeleteEmergencyPurchase");

                    if (StructureType.Contains("Control Structure1"))
                    {
                        lblRD.Text = "";
                    }
                    if (lblCampSite.Text == "No")
                    {
                        lblRD.Text = "";
                    }

                    #region User Role

                    btnEditEmergencyPurchase.Enabled = false;
                    btnDeleteEmergencyPurchase.Enabled = false;

                    CanAddEditEP = new FloodOperationsBLL().CanAddEditEmergencyPurchase(EmergencyPurchasesYear, SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        btnEditEmergencyPurchase.Enabled = CanAddEditEP;
                        btnDeleteEmergencyPurchase.Enabled = CanAddEditEP;
                    }
                    //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "StartDate");
                    //string StartDate = systemParameters.ParameterValue + "-" + EmergencyPurchasesYear; // 01-Jan
                    //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "EndDate"); // 31-Mar
                    //string EndDate = systemParameters.ParameterValue + "-" + EmergencyPurchasesYear;

                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        btnEditEmergencyPurchase.Enabled = true;
                    //        btnDeleteEmergencyPurchase.Enabled = true;
                    //    }
                    //}

                    #endregion User Role
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvEmergencyPurchases_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditEmergencyPurchase")
                {
                    long _EmergerncyPurchaseID = Convert.ToInt64(e.CommandArgument);
                    Response.Redirect("AddEmergencyPurchases.aspx?EmergencyPurchaseID=" + _EmergerncyPurchaseID);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}