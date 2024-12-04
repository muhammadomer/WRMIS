using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.AppBlocks;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class SearchReceivedStock : BasePage
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
        public const string ReceivedStockTypeIDKey = "ReceivedStockTypeID";
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
                    if (_IsSaved)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description);
                        _IsSaved = false; // Reset flag after displaying message.
                    }
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

        #region Gridview Method

        #region Purchase FloodFightingPlan GridView Method

        private void BindSearchPurchaseFloodFightingPlanGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                int? SelecteReceivedStockType = null;
                int? SelecteItemCategoryID = null;

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

                if (ddlReceivedStockType.SelectedItem.Value != String.Empty)
                {
                    SelecteReceivedStockType = Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value);
                }
                SearchCriteria.Add(SelecteReceivedStockType, ddlReceivedStockType.SelectedItem.Value);

                if (ddlItemCategory.SelectedItem.Value != String.Empty)
                {
                    SelecteItemCategoryID = Convert.ToInt32(ddlItemCategory.SelectedItem.Value);
                }
                //SearchCriteria.Add(SelecteItemCategoryID, ddlItemCategory.SelectedItem.Value);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                DataSet DS = new DivisionStoreBLL().GetDSSearchReceivedStock(SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedYear, SelecteReceivedStockType, SelecteItemCategoryID);
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvPurchaseFloodFightingPlan.DataSource = DS.Tables[0];
                }
                gvPurchaseFloodFightingPlan.DataBind();
                //var LstPurchaseFloodFightingPlan = IePurchaseFloodFightingPlan.Select(dataRow => new
                //{
                //    itemsName = dataRow.Field<string>("itemsName"),
                //    QuantityApproved = dataRow.Field<int>("QuantityApproved"),
                //    QuantityReceived = dataRow.Field<int>("QuantityReceived"),
                //    //QuantityPurchased = dataRow.Field<int>("QuantityPurchased"),
                //    //InfrastructureType = dataRow.Field<string>("InfrastructureType"),
                //    //InfrastructureName = dataRow.Field<string>("InfrastructureName"),
                //    //CampSiteRD = dataRow.Field<int>("CampSiteRD"),
                //    //DivisionName = dataRow.Field<string>("DivisionName"),
                //}).ToList();
                //gvPurchaseFloodFightingPlan.DataSource = LstPurchaseFloodFightingPlan;
                //gvPurchaseFloodFightingPlan.DataBind();
                SearchCriteria.Add(PageIndexKey, gvPurchaseFloodFightingPlan.PageIndex);
                Session[SessionValues.SearchPurchaseFloodFightingPlan] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvPurchaseFloodFightingPlan_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvPurchaseFloodFightingPlan.EditIndex = -1;
                BindSearchPurchaseFloodFightingPlanGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvPurchaseFloodFightingPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPurchaseFloodFightingPlan.PageIndex = e.NewPageIndex;
                BindSearchPurchaseFloodFightingPlanGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #endregion Purchase FloodFightingPlan GridView Method

        #region Return PurchaseDuringFlood GridView Method
        private void BindSearchReturnPurchaseDuringFloodGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                int? SelecteReceivedStockType = null;
                int? SelecteItemCategoryID = null;

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

                if (ddlReceivedStockType.SelectedItem.Value != String.Empty)
                {
                    SelecteReceivedStockType = Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value);
                }
                SearchCriteria.Add(SelecteReceivedStockType, ddlReceivedStockType.SelectedItem.Value);

                if (ddlItemCategory.SelectedItem.Value != String.Empty)
                {
                    SelecteItemCategoryID = Convert.ToInt32(ddlItemCategory.SelectedItem.Value);
                }
                //SearchCriteria.Add(SelecteItemCategoryID, ddlItemCategory.SelectedItem.Value);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                DataSet DS = new DivisionStoreBLL().GetDSSearchReceivedStock(SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedYear, SelecteReceivedStockType, SelecteItemCategoryID);
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvReturnPurchaseDuringFlood.DataSource = DS.Tables[0];
                }
                gvReturnPurchaseDuringFlood.DataBind();
                //var LstReturnPurchaseDuringFlood = IeReturnPurchaseDuringFlood.Select(dataRow => new
                //{
                //    itemsName = dataRow.Field<string>("ItemName"),
                //    QuantityPurchased = dataRow.Field<int>("QuantityPurchased"),
                //    RQuantityReceived = dataRow.Field<int>("RQuantityReceived"),

                //}).ToList();
                //gvReturnPurchaseDuringFlood.DataSource = LstReturnPurchaseDuringFlood;
                //gvReturnPurchaseDuringFlood.DataBind();
                SearchCriteria.Add(PageIndexKey, gvReturnPurchaseDuringFlood.PageIndex);
                Session[SessionValues.SearchReturnPurchaseDuringFlood] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReturnPurchaseDuringFlood_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvReturnPurchaseDuringFlood.EditIndex = -1;
                BindSearchReturnPurchaseDuringFloodGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReturnPurchaseDuringFlood_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvReturnPurchaseDuringFlood.PageIndex = e.NewPageIndex;
                BindSearchReturnPurchaseDuringFloodGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Return PurchaseDuringFlood GridView Method

        #region  Return Infrastructure Gridview Method
        private void BindSearchReturnInfrastructureGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                int? SelecteReceivedStockType = null;

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

                if (ddlReceivedStockType.SelectedItem.Value != String.Empty)
                {
                    SelecteReceivedStockType = Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value);
                }
                SearchCriteria.Add(SelecteReceivedStockType, ddlReceivedStockType.SelectedItem.Value);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                DataSet DS = new DivisionStoreBLL().GetDSSearchReceivedStock(SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedYear, SelecteReceivedStockType, null);
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvReturnInfrastructure.DataSource = DS.Tables[0];
                }
                gvReturnInfrastructure.DataBind();
                //var LstReturnInfrastructure = IeReturnInfrastructure.Select(dataRow => new
                //{
                //    InfraStructureType = dataRow.Field<string>("InfraStructureType"),
                //    InfrastructureName = dataRow.Field<string>("InfraStructureName"),
                //    DivisionName = dataRow.Field<string>("DivisionName"),
                //    StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                //    StructureID = dataRow.Field<long>("StructureID"),

                //}).ToList();
                //gvReturnInfrastructure.DataSource = LstReturnInfrastructure;
                //gvReturnInfrastructure.DataBind();
                SearchCriteria.Add(PageIndexKey, gvReturnInfrastructure.PageIndex);
                Session[SessionValues.SearchReturnInfrastructure] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvReturnInfrastructure_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvReturnInfrastructure.EditIndex = -1;
                BindSearchReturnInfrastructureGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvReturnInfrastructure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvReturnInfrastructure.PageIndex = e.NewPageIndex;
                BindSearchReturnInfrastructureGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion  Return Infrastructure Gridview Method

        #region  Return CampSite Gridview Method

        private void BindSearchReturnCampSiteGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedZoneID = null;
                long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                int? SelecteReceivedStockType = null;

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

                if (ddlReceivedStockType.SelectedItem.Value != String.Empty)
                {
                    SelecteReceivedStockType = Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value);
                }
                SearchCriteria.Add(SelecteReceivedStockType, ddlReceivedStockType.SelectedItem.Value);

                //var DivID = _DivisionID == 0 ? (long?)null : _DivisionID
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                DataSet DS = new DivisionStoreBLL().GetDSSearchReceivedStock(SelectedZoneID, SelectedCircleID, SelectedDivisionID, SelectedYear, SelecteReceivedStockType, null);
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvReturnCampSite.DataSource = DS.Tables[0];
                }
                gvReturnCampSite.DataBind();
                //var LstReturnCampSite = IeReturnCampSite.Select(dataRow => new
                //{
                //    InfraStructureType = dataRow.Field<string>("InfraStructureType"),
                //    InfrastructureName = dataRow.Field<string>("InfraStructureName"),
                //    CampSiteRD = Calculations.GetRDText(Convert.ToInt32(dataRow.Field<Int32>("CampSiteRD"))),// dataRow.Field<int>("CampSiteRD"),
                //    DivisionName = dataRow.Field<string>("DivisionName"),
                //    StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                //    StructureID = dataRow.Field<long>("StructureID"),
                //}).ToList();
                //gvReturnCampSite.DataSource = LstReturnCampSite;
                //gvReturnCampSite.DataBind();
                SearchCriteria.Add(PageIndexKey, gvReturnCampSite.PageIndex);
                Session[SessionValues.SearchReturnCampSite] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvReturnCampSite_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvReturnCampSite.EditIndex = -1;
                BindSearchReturnCampSiteGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvReturnCampSite_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvReturnCampSite.PageIndex = e.NewPageIndex;
                BindSearchReturnCampSiteGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion  Return CampSite Gridview Method

        #endregion Gridview Method

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
                Dropdownlist.DDLYear(ddlYear, false, 2, -1, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLDSEntryType(ddlReceivedStockType, "Received", false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLItemCategory(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
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

        private void DDLEmptyCircleDivision()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircleByUserIDAndZoneID(ddlCircle, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisionsByUserIDAndCircleID(ddlDivision, mdlUser.ID, 0, -1, true, (int)Constants.DropDownFirstOption.All);
        }

        //protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ddlItemCategory.SelectedItem.Value == "")
        //        {
        //            gvItems.Visible = false;
        //            gvReceivedStockInfrastructure.Visible = false;
        //            gvCampSite.Visible = false;
        //            gvFFP.Visible = false;
        //            btnSave.Enabled = false;
        //        }
        //        else
        //        {
        //            if (ddlStockType.SelectedValue == "2")
        //            {
        //                gvItems.Visible = true;
        //                BindItemsGridView();
        //                gvItems.Enabled = true;
        //            }
        //            else if (ddlStockType.SelectedValue == "3")
        //            {
        //                gvReceivedStockInfrastructure.Visible = true;
        //                BindReceivedStockInfrastructureGridView();
        //                gvReceivedStockInfrastructure.Enabled = true;

        //            }
        //            else if (ddlStockType.SelectedValue == "4")
        //            {
        //                gvCampSite.Visible = true;
        //                BindGridViewCampSite();
        //                gvCampSite.Enabled = true;
        //            }
        //            else if (ddlStockType.SelectedValue == "1")
        //            {
        //                gvFFP.Visible = true;
        //                BindGridViewFFP();
        //                gvFFP.Enabled = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        #endregion Dropdown Lists Binding

        protected void ddlReceivedStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.PurchaseFloodFightingPlan)
                {
                    Div_category.Visible = true;
                    PurchaseFloodFightingPlan.Visible = true;
                    ReturnPurchaseDuringFlood.Visible = false;
                    ReturnInfrastructure.Visible = false;
                    ReturnCampSite.Visible = false;
                    Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                    ddlItemCategory.ClearSelection();
                }
                else if (Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.ReturnsPurchasedduringFlood)
                {
                    Div_category.Visible = true;
                    PurchaseFloodFightingPlan.Visible = false;
                    ReturnPurchaseDuringFlood.Visible = true;
                    ReturnInfrastructure.Visible = false;
                    ReturnCampSite.Visible = false;
                    Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                    ddlItemCategory.ClearSelection();
                }
                else if (Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.ReturnsInfrastructures)
                {
                    Div_category.Visible = false;
                    PurchaseFloodFightingPlan.Visible = false;
                    ReturnPurchaseDuringFlood.Visible = false;
                    ReturnInfrastructure.Visible = true;
                    ReturnCampSite.Visible = false;
                    Dropdownlist.DDLItemCategory(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                    ddlItemCategory.ClearSelection();
                }
                else if (Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.ReturnsCampSites)
                {
                    Div_category.Visible = false;
                    PurchaseFloodFightingPlan.Visible = false;
                    ReturnPurchaseDuringFlood.Visible = false;
                    ReturnInfrastructure.Visible = false;
                    ReturnCampSite.Visible = true;
                    Dropdownlist.DDLItemCategory(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                    ddlItemCategory.ClearSelection();
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
                if (Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.PurchaseFloodFightingPlan)
                {
                    PurchaseFloodFightingPlan.Visible = true;
                    BindSearchPurchaseFloodFightingPlanGrid();
                }
                else if (Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.ReturnsPurchasedduringFlood)
                {
                    ReturnPurchaseDuringFlood.Visible = true;
                    BindSearchReturnPurchaseDuringFloodGrid();
                }
                else if (Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.ReturnsInfrastructures)
                {
                    ReturnInfrastructure.Visible = true;
                    BindSearchReturnInfrastructureGrid();
                }
                else if (Convert.ToInt32(ddlReceivedStockType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.ReturnsCampSites)
                {
                    ReturnCampSite.Visible = true;
                    BindSearchReturnCampSiteGrid();
                }



            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReturnInfrastructure_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ItemDetail")
                {
                    Session["infrastructureType"] = "";
                    Session["InfraName"] = "";
                    Session["stockType"] = "";
                    Session["StructureTypeID"] = "";
                    Session["StructureID"] = "";
                    Session["DivisionIDD"] = "";
                    Session["DivisionIDD"] = ddlDivision.SelectedValue;
                    Session["stockType"] = ddlReceivedStockType.SelectedValue;
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvReturnInfrastructure.DataKeys[row.RowIndex];
                    Session["StructureTypeID"] = Convert.ToString(key["StructureTypeID"]);
                    Session["StructureID"] = Convert.ToString(key["StructureID"]);
                    Session["infrastructureType"] = Convert.ToString(key["InfraStructureType"]);
                    Session["InfraName"] = Convert.ToString(key["InfrastructureName"]);
                    Response.Redirect("ViewReceivedStockInfraStructure.aspx");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReturnCampSite_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ItemDetail")
                {
                    Session["infrastructureType"] = "";
                    Session["InfraName"] = "";
                    Session["stockType"] = "";
                    Session["StructureTypeID"] = "";
                    Session["StructureID"] = "";
                    Session["DivisionIDD"] = "";
                    Session["RD"] = "";
                    Session["DivisionIDD"] = ddlDivision.SelectedValue;
                    Session["stockType"] = ddlReceivedStockType.SelectedValue;
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvReturnCampSite.DataKeys[row.RowIndex];
                    Session["StructureTypeID"] = Convert.ToString(key["StructureTypeID"]);
                    Session["StructureID"] = Convert.ToString(key["StructureID"]);
                    Session["infrastructureType"] = Convert.ToString(key["InfraStructureType"]);
                    Session["InfraName"] = Convert.ToString(key["InfrastructureName"]);
                    Session["RD"] = Convert.ToString(key["CampSiteRD"]);
                    Session["FFPcampSiteID"] = Convert.ToString(key["FFPcampSiteID"]);
                    Response.Redirect("ViewReceivedStockCampSite.aspx");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PurchaseFloodFightingPlan.Visible = false;
            ReturnPurchaseDuringFlood.Visible = false;
            ReturnInfrastructure.Visible = false;
            ReturnCampSite.Visible = false;
        }

        protected void gvReturnCampSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataKey key = gvReturnCampSite.DataKeys[e.Row.RowIndex];
                string StructureType = Convert.ToString(key["InfraStructureType"]);
                int CampSiteRD = Convert.ToInt32(key["CampSiteRD"]);
                Label lblRD = (Label)e.Row.FindControl("lblRD");
                Label lblInfrastructureType = (Label)e.Row.FindControl("lblInfrastructureType");

                lblRD.Text = Calculations.GetRDText(Convert.ToInt64(CampSiteRD));

                if (StructureType.Contains("Barrage") || StructureType.Contains("Head work"))
                {
                    lblRD.Text = "";
                }
            }
        }


    }
}