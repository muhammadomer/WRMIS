using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.FloodOperations;
using System.Collections;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class SearchIssuedStock : BasePage
    {
        private static bool _IsSaved = false;

        public static bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        #region Hash Table Keys

        public const string DivisionIDKey = "DivisionID";
        public const string YearDateKey = "Year";
        public const string InfrastructureTypeIDKey = "InfrastructureTypeID";
        public const string InfrastructureNameIDKey = "InfrastructureNameID";
        public const string DivisionStoreIssueTypeIDKey = "DivisionStoreIssueTypeIDKey";
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
                    long DivisionStoreID = Utility.GetNumericValueFromQueryString("DivisionStoreID", 0);
                    if (DivisionStoreID > 0)
                    {
                        //  hdnDivisionStoreID.Value = DivisionStoreID.ToString();
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        // BindSearchResultsGrid(Convert.ToInt64(hdnDivisionStoreID.Value));
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

        private void BindDropdownlists()
        {
            try
            {
                //BindUserLocation();
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLDSEntryType(ddlDivisionStoreissueType, "Issued", false,
                    (int)Constants.DropDownFirstOption.All);
                BindInfrastructureTypeDropdown();
                // Bind year dropdownlist
                Dropdownlist.DDLYear(ddlYear, false, 2, -1, (int)Constants.DropDownFirstOption.All);
                //Dropdownlist.DDLYear(ddlYear, false, 0, 2015, (int)Constants.DropDownFirstOption.All);     change year dropdown (only show current and privious year)
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
                    List<UA_AssociatedLocation> lstAssociatedLocation =
                        new UserAdministrationBLL().GetUserLocationsByUserID(mdlUser.ID);

                    if (lstAssociatedLocation.Count() > 0)
                    {
                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
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

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

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
                        Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
                    }
                }
                else
                {
                    Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
                }
            }
            else
            {
                Dropdownlist.DDLDivisions(ddlDivision, false, (int)Constants.DropDownFirstOption.All);
            }
            ViewState.Add(UserDivisionKey, lstUserDivision);
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

        protected void ddlInfrastructureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UA_Users _Users = SessionManagerFacade.UserInformation;

            if (ddlInfrastructureType.SelectedItem.Value != "")
            {
                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 1)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 1,
                        (int)Constants.DropDownFirstOption.All);
                else if (InfrastructureTypeSelectedValue == 2)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 2,
                        (int)Constants.DropDownFirstOption.All);
                else if (InfrastructureTypeSelectedValue == 3)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, 3,
                        (int)Constants.DropDownFirstOption.All);
            }
        }

        #region  Issued Stock Infrastructure Gridview Method

        private void BindInfrastructureSearchResultsGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                string SelectedInfrastyructureTypeID = null;
                string SelectedInfrastyructureName = null;
                int? SelecteDivisionStoreIssueType = null;

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
                    SelectedInfrastyructureTypeID = Convert.ToString(ddlInfrastructureType.SelectedItem.Text) ==
                                                    "Barrage/Headwork"
                        ? "Control Structure1"
                        : Convert.ToString(ddlInfrastructureType.SelectedItem.Text);
                }
                SearchCriteria.Add(InfrastructureTypeIDKey, SelectedInfrastyructureTypeID);

                if (ddlInfrastructureName.SelectedItem.Text != "All")
                {
                    SelectedInfrastyructureName = Convert.ToString(ddlInfrastructureName.SelectedItem.Text);
                }

                SearchCriteria.Add(InfrastructureNameIDKey, ddlInfrastructureName.SelectedItem.Text);

                if (ddlDivisionStoreissueType.SelectedItem.Value != String.Empty)
                {
                    SelecteDivisionStoreIssueType = Convert.ToInt32(ddlDivisionStoreissueType.SelectedItem.Value);
                }
                SearchCriteria.Add(DivisionStoreIssueTypeIDKey, ddlDivisionStoreissueType.SelectedItem.Value);

                // var DivID = _DivisionID == 0 ? (long?)null : _DivisionID;

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                IEnumerable<DataRow> IeSearchIssueStockInfrastructure =
                    new DivisionStoreBLL().GetDSSearchIssuedStock(SelectedDivisionID, SelecteDivisionStoreIssueType,
                        SelectedYear, SelectedInfrastyructureTypeID, SelectedInfrastyructureName, Convert.ToInt64( ddlInfrastructureType.SelectedValue),Convert.ToInt64( ddlInfrastructureName.SelectedValue));
                var LstSearchIssueStockInfrastructure = IeSearchIssueStockInfrastructure.Select(dataRow => new
                {
                    InfraStructureType = dataRow.Field<string>("InfraStructureType"),
                    InfrastructureName = dataRow.Field<string>("InfraStructureName"),
                    DivisionName = dataRow.Field<string>("DivisionName"),
                    DivisionStoreID = dataRow.Field<long>("DivisionStoreID"),
                    StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                    StructureID = dataRow.Field<long>("StructureID")
                }).ToList();

                //var LstSearchIssueStockInfrastructure = IeSearchIssueStockInfrastructure.Select(dataRow => new
                //{
                //    InfraStructureType = dataRow.Field<string>("InfraStructureType"),
                //    InfrastructureName = dataRow.Field<string>("InfraStructureName"),
                //    DivisionName = dataRow.Field<string>("DivisionName"),
                //    DivisionStoreID = dataRow.Field<long>("DivisionStoreID")
                //}).ToList();
                gvIssuedStockInfrastructure.DataSource = LstSearchIssueStockInfrastructure;
                gvIssuedStockInfrastructure.DataBind();
                SearchCriteria.Add(PageIndexKey, gvIssuedStockInfrastructure.PageIndex);
                Session[SessionValues.SearchIssueStockInfrastructure] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIssuedStockInfrastructure_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvIssuedStockInfrastructure.EditIndex = -1;
                BindInfrastructureSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIssuedStockInfrastructure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIssuedStockInfrastructure.PageIndex = e.NewPageIndex;
                BindInfrastructureSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIssuedStockInfrastructure_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ItemDetail")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvIssuedStockCampSite.DataKeys[row.RowIndex];
                    //Response.Redirect("IssuedStockViewCampSiteItems.aspx");
                    Response.Redirect("IssuedStockViewInfrastructuresItems.aspx?DivisionID=" + ddlDivision.SelectedValue + "&SelectedYear=" + ddlYear.SelectedValue + "&StockType=" + ddlDivisionStoreissueType.SelectedValue + "&infrastructureType=" + Convert.ToString(key["InfraStructureType"]) + "&InfraName=" + Convert.ToString(key["InfraStructureName"]));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Issued Stock Infrastructure Gridview Method

        #region  Issued Stock CampSite Gridview Method

        private void BindCampSiteSearchResultsGrid()
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                string SelectedInfrastyructureTypeID = null;
                string SelectedInfrastyructureName = null;
                int? SelecteDivisionStoreIssueType = null;

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
                    SelectedInfrastyructureTypeID = Convert.ToString(ddlInfrastructureType.SelectedItem.Text) ==
                                                    "Barrage/Headwork"
                        ? "Control Structure1"
                        : Convert.ToString(ddlInfrastructureType.SelectedItem.Text);
                }
                SearchCriteria.Add(InfrastructureTypeIDKey, SelectedInfrastyructureTypeID);

                if (ddlInfrastructureName.SelectedItem.Text != "All")
                {
                    SelectedInfrastyructureName = Convert.ToString(ddlInfrastructureName.SelectedItem.Text);
                }

                SearchCriteria.Add(InfrastructureNameIDKey, ddlInfrastructureName.SelectedItem.Text);

                if (ddlDivisionStoreissueType.SelectedItem.Value != String.Empty)
                {
                    SelecteDivisionStoreIssueType = Convert.ToInt32(ddlDivisionStoreissueType.SelectedItem.Value);
                }
                SearchCriteria.Add(DivisionStoreIssueTypeIDKey, ddlDivisionStoreissueType.SelectedItem.Value);

                // var DivID = _DivisionID == 0 ? (long?)null : _DivisionID;

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                IEnumerable<DataRow> IeSearchIssueStockCampSite =
                    new DivisionStoreBLL().GetDSSearchIssuedStock(SelectedDivisionID, SelecteDivisionStoreIssueType,
                        SelectedYear, SelectedInfrastyructureTypeID, SelectedInfrastyructureName, Convert.ToInt64(ddlInfrastructureType.SelectedValue), Convert.ToInt64(ddlInfrastructureName.SelectedValue));
                var LstSearchIssueStockCampSite = IeSearchIssueStockCampSite.Select(dataRow => new
                {
                    InfraStructureType = dataRow.Field<string>("InfraStructureType"),
                    InfrastructureName = dataRow.Field<string>("InfraStructureName"),
                    CampSiteRD = Calculations.GetRDText(Convert.ToInt32(dataRow.Field<int>("CampSiteRD"))),
                    DivisionName = dataRow.Field<string>("DivisionName"),
                    DivisionStoreID = dataRow.Field<long>("DivisionStoreID"),
                    FFPCampSiteID = dataRow.Field<long>("FFPCampSiteID"),
                    StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                    StructureID = dataRow.Field<long>("StructureID"),
                    RD = dataRow.Field<Int32>("CampSiteRD")
                }).ToList();
                gvIssuedStockCampSite.DataSource = LstSearchIssueStockCampSite;
                gvIssuedStockCampSite.DataBind();
                SearchCriteria.Add(PageIndexKey, gvIssuedStockCampSite.PageIndex);
                Session[SessionValues.SearchIssueStockInfrastructure] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIssuedStockCampSite_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvIssuedStockCampSite.EditIndex = -1;
                BindCampSiteSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIssuedStockCampSite_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIssuedStockCampSite.PageIndex = e.NewPageIndex;
                BindCampSiteSearchResultsGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIssuedStockCampSite_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ItemDetail")
                {
                    Session["DivisionID"] = "";
                    Session["infrastructureType"] = "";
                    Session["InfraName"] = "";
                    Session["RD"] = "";
                    Session["stockType"] = "";
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvIssuedStockCampSite.DataKeys[row.RowIndex];
                    Session["DivisionID"] = ddlDivision.SelectedValue;
                    Session["stockType"] = ddlDivisionStoreissueType.SelectedValue;
                    Session["infrastructureType"] = Convert.ToString(key["InfraStructureType"]);
                    Session["InfraName"] = Convert.ToString(key["InfraStructureName"]);
                    Session["RD"] = Convert.ToString(key["CampSiteRD"]);
                    Session["FFPcampSiteID"] = Convert.ToString(key["FFPcampSiteID"]);
                    //Response.Redirect("IssuedStockViewCampSiteItems.aspx");
                    Response.Redirect("IssuedStockViewCampSiteItems.aspx?DivisionID=" + ddlDivision.SelectedValue + "&FFPCampSiteID=" + Convert.ToString(key["FFPCampSiteID"]) + "&SelectedYear=" + ddlYear.SelectedValue + "&StructureTypeID=" + Convert.ToString(key["StructureTypeID"]) + "&StructureID=" + ddlInfrastructureName.SelectedValue + "&StockType=" + ddlDivisionStoreissueType.SelectedValue + "&infrastructureType=" + Convert.ToString(key["InfraStructureType"]) + "&InfraName=" + Convert.ToString(key["InfraStructureName"]) + "&RD=" + Convert.ToString(key["RD"]));

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Issued Stock CampSite Gridview Method

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlDivisionStoreissueType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.IssuedInfrastructure)
                {
                    IssuedStockInfrastructure.Visible = true;
                    BindInfrastructureSearchResultsGrid();
                }
                else if (Convert.ToInt32(ddlDivisionStoreissueType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.IssuedCampSites)
                {
                    IssuedStockCampSite.Visible = true;
                    BindCampSiteSearchResultsGrid();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivisionStoreissueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlDivisionStoreissueType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.IssuedInfrastructure)
                {
                    IssuedStockInfrastructure.Visible = true;
                    IssuedStockCampSite.Visible = false;
                }
                else if (Convert.ToInt32(ddlDivisionStoreissueType.SelectedItem.Value) == (int)Constants.DivisionStoreEntryType.IssuedCampSites)
                {
                    IssuedStockCampSite.Visible = true;
                    IssuedStockInfrastructure.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["DivisionID"] = null;
                Session["DivisionID"] = ddlDivision.SelectedValue;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnIssuenewStock_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value != string.Empty)
                {
                    Response.Redirect("IssuedNewStockInfrastructureCampSite.aspx?DivisionID=" + ddlDivision.SelectedItem.Value);
                }
                else
                {
                    //Session["DivisionID"] = null;
                    //Session["DivisionID"] = ddlDivision.SelectedValue;
                    Master.ShowMessage("Please Select Division to issue new Stock", SiteMaster.MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIssuedStockInfrastructure_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            DataKey key = gvIssuedStockInfrastructure.DataKeys[row.RowIndex];
            Response.Redirect("IssueStockViewInfraStructureItems.aspx?DivisionID=" + ddlDivision.SelectedValue + "&SelectedYear=" + ddlYear.SelectedValue + "&StructureTypeID=" + Convert.ToString(key["StructureTypeID"]) + "&StructureID=" + ddlInfrastructureName.SelectedValue + "&StockType=" + ddlDivisionStoreissueType.SelectedValue + "&infrastructureType=" + Convert.ToString(key["InfraStructureType"]) + "&InfraName=" + Convert.ToString(key["InfraStructureName"]));
        }

        protected void gvIssuedStockCampSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataKey key = gvIssuedStockCampSite.DataKeys[e.Row.RowIndex];
                string StructureType = Convert.ToString(key["InfraStructureType"]);
                Label lblRD = (Label)e.Row.FindControl("lblRD");
                Label lblInfrastructureType = (Label)e.Row.FindControl("lblInfrastructureType");

                if (StructureType.Contains("Barrage") || StructureType.Contains("Head work"))
                {
                    lblRD.Text = "";
                }
            }
        }
    }
}