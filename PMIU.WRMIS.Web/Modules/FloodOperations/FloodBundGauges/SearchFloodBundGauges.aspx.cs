using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodBundGauges
{
    public partial class SearchFloodBundGauges : BasePage
    {
        private static bool _IsSaved = false;

        public static bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        #region Hash Table Keys

        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string ZoneIDKey = "ZoneID";
        public const string CircleIDKey = "CircleID";
        public const string DivisionIDKey = "DivisionID";
        public const string StructureTypeIDKey = "StructureTypeIDKey";
        public const string StructureNameIDKey = "StructureNameIDKey";
        public const string RDKey = "RDKey";
        public const string TimeKey = "TimeKey";
        public const string PageIndexKey = "PageIndex";

        #endregion Hash Table Keys

        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                    this.Form.DefaultButton = btnShow.UniqueID;
                    if (_IsSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description);
                        _IsSaved = false; // Reset flag after displaying message.
                    }
                    BindSearchResultsGrid();
                    hlAddNew.Visible = base.CanAdd;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Set Page Title

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        #endregion Set Page Title

        private void BindDropdownlists()
        {
            try
            {
                BindUserLocation();
                BindStructureTypeFloodBund();

                BindTimeFloodBund();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindTimeFloodBund()
        {
            try
            {
                //Dropdownlist.DDLTime(ddlTime, (int)Constants.DropDownFirstOption.All);
                List<string> TimeList = new FloodOperationsBLL().GetTimeListFloodBund();
                ddlTime.DataSource = TimeList;
                ddlTime.DataBind();
                ddlTime.Items.Insert(0, "All");
                //items.insert(0,"--Select--")

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

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

        private void BindStructureTypeFloodBund()
        {
            Dropdownlist.DDLStructureTypesForFloodBund(ddlStructureType, (int)Constants.DropDownFirstOption.All);
        }

        private void BindFloodGaugesRDs(long _StructureNameID)
        {
            Dropdownlist.DDLFloodGaugesRDs(ddlRD, _StructureNameID, (int)Constants.DropDownFirstOption.All);
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    //  ddlCircle.Enabled = false;
                }
                else
                {
                    //DDLEmptyCircleDivision();
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);
                    //  ddlCircle.Enabled = true;
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
                    //   ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);
                    //  ddlDivision.Enabled = true;
                }
                //   gvSearchIndependent.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
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

        private void BindSearchResultsGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();
                string GaugeLeftRD = string.Empty;
                string GaugeRightRD = string.Empty;
                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                string SelectedStructureTypeID = null;
                string SelectedStructureName = null;
                long? SelectedRD = null;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                string SelectedTime = null;

                if (txtFromDate.Text.Trim() != String.Empty)
                {
                    FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
                }

                SearchCriteria.Add(FromDateKey, txtFromDate.Text.Trim());

                if (txtToDate.Text != String.Empty)
                {
                    ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
                }

                SearchCriteria.Add(ToDateKey, txtToDate.Text.Trim());

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
                SearchCriteria.Add(ZoneIDKey, ddlZone.SelectedItem.Value);

                if (ddlCircle.SelectedItem.Value != String.Empty && ddlCircle.SelectedItem.Value != "0")
                {
                    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                }
                SearchCriteria.Add(CircleIDKey, ddlCircle.SelectedItem.Value);

                if (ddlDivision.SelectedItem.Value != String.Empty && ddlDivision.SelectedItem.Value != "0")
                {
                    SelectedDivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                }

                SearchCriteria.Add(DivisionIDKey, ddlDivision.SelectedItem.Value);

                if (ddlStructureType.SelectedItem.Text != "All")
                {
                    SelectedStructureTypeID = Convert.ToString(ddlStructureType.SelectedItem.Text);
                }

                SearchCriteria.Add(StructureTypeIDKey, SelectedStructureTypeID);

                if (ddlStructureName.SelectedItem.Text != "All")
                {
                    SelectedStructureName = Convert.ToString(ddlStructureName.SelectedItem.Text);
                }

                SearchCriteria.Add(StructureNameIDKey, SelectedStructureName);

                if (ddlRD.SelectedItem.Text != "All")
                {
                    if (!string.IsNullOrEmpty(ddlRD.SelectedItem.Text))
                    {
                        SelectedRD = Convert.ToInt64(ddlRD.SelectedItem.Text.Replace("+", ""));
                    }
                }
                SearchCriteria.Add(RDKey, SelectedRD);

                if (ddlTime.SelectedItem.Text != "All")
                {
                    SelectedTime = Convert.ToString(ddlTime.SelectedItem.Text);
                    //DateTime.ParseExact(ddlTime.SelectedItem.Text, "hh:mm tt", null);
                    //string and = DateTime.Now.ToString("hh:mm");
                }
                SearchCriteria.Add(TimeKey, SelectedTime);

                if (SelectedStructureTypeID == "Nallah" || SelectedStructureTypeID == "Hill Torrent")
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    IEnumerable<DataRow> ieFloodBund = new FloodOperationsBLL().GetBundGaugesDataSearch(SelectedDivisionID, SelectedStructureTypeID, SelectedStructureName, SelectedRD, SelectedTime, FromDate, ToDate);
                    var lstFloodBundGauge = ieFloodBund.Select(dataRow => new
                    {
                        GaugeDate = dataRow.Field<string>("GaugeDate"),
                        GaugeTime = dataRow.Field<string>("GaugeTime"),
                        GaugeValue = dataRow.Field<double>("GaugeValue")
                        //GaugeRD = Calculations.GetRDText(Convert.ToInt64(dataRow.Field<int?>("GaugeRD"))),

                    }).ToList();

                    gvNallahHillTorrent.DataSource = lstFloodBundGauge;
                    gvNallahHillTorrent.DataBind();

                    SearchCriteria.Add(PageIndexKey, gvNallahHillTorrent.PageIndex);
                    Session[SessionValues.SearchFloodBundGauges] = SearchCriteria;
                    gvSearchBundGaugeData.Visible = false;
                    gvNallahHillTorrent.Visible = true;
                }
                else
                {
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    IEnumerable<DataRow> ieFloodBund = new FloodOperationsBLL().GetBundGaugesDataSearch(SelectedDivisionID, SelectedStructureTypeID, SelectedStructureName, SelectedRD, SelectedTime, FromDate, ToDate);
                    var lstFloodBundGauge = ieFloodBund.Select(dataRow => new
                    {
                        GaugeDate = dataRow.Field<string>("GaugeDate"),
                        GaugeTime = dataRow.Field<string>("GaugeTime"),
                        GaugeValue = dataRow.Field<double>("GaugeValue"),
                        GaugeRD = Calculations.GetRDText(Convert.ToInt64(dataRow.Field<int?>("GaugeRD"))),

                    }).ToList();

                    gvSearchBundGaugeData.DataSource = lstFloodBundGauge;
                    gvSearchBundGaugeData.DataBind();

                    SearchCriteria.Add(PageIndexKey, gvSearchBundGaugeData.PageIndex);
                    Session[SessionValues.SearchFloodBundGauges] = SearchCriteria;
                    gvNallahHillTorrent.Visible = false;
                    gvSearchBundGaugeData.Visible = true;
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchResultsGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlStructureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                if (ddlStructureType.SelectedItem.Value != "")
                {
                    long StructureTypeSelectedValue = Convert.ToInt64(ddlStructureType.SelectedItem.Value);
                    if (StructureTypeSelectedValue == 1)
                    {
                        Dropdownlist.DDLStructureNameFloodBund(ddlStructureName, _Users.ID, "Bund",
                            (int)Constants.DropDownFirstOption.All);
                        ddlRD.Visible = true;
                        lblRD.Visible = true;
                    }
                    else if (StructureTypeSelectedValue == 2)
                    {
                        Dropdownlist.DDLStructureNameFloodBund(ddlStructureName, _Users.ID, "Nallah",
                            (int)Constants.DropDownFirstOption.All);
                        ddlRD.Visible = false;
                        lblRD.Visible = false;
                    }

                    else if (StructureTypeSelectedValue == 3)
                    {
                        Dropdownlist.DDLStructureNameFloodBund(ddlStructureName, _Users.ID, "Hill Torrent",
                            (int)Constants.DropDownFirstOption.All);
                        ddlRD.Visible = false;
                        lblRD.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlStructureName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStructureName.SelectedItem.Value != "")
            {
                BindFloodGaugesRDs(Convert.ToInt64(ddlStructureName.SelectedItem.Value));
            }
        }
    }
}
