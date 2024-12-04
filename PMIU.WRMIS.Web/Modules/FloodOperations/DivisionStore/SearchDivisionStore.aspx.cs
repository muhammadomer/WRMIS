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
using NPOI.SS.Formula.Functions;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class SearchDivisionStore : BasePage
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
        public const string ItemCategoryIDKey = "ItemCategoryID";
        public const string MajorMinorItemIDKey = "MajorMinorItemID";
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
                        hdnDivisionStoreID.Value = DivisionStoreID.ToString();
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
                LoadItemCategory();
                Dropdownlist.DDLYear(ddlYear, false, 0, 2015, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadItemCategory()
        {
            try
            {
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
                }
                else
                {
                    DDLEmptyCircleDivision();
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                    BindCircleDropdown(ZoneID);
                }

                ddlDivision.SelectedIndex = 0;
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
                    //       ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                    BindDivisionDropdown(CircleID);
                    //     ddlDivision.Enabled = true;
                }
                //   gvSearchIndependent.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion Dropdown Lists Binding

        #region Gridview Method

        private void BindSearchResultsGrid(long _DivisionStoreID)
        {
            try
            {
                Hashtable SearchCriteria = new Hashtable();

                //long? SelectedZoneID = null;
                //long? SelectedCircleID = null;
                long? SelectedDivisionID = null;
                int? SelectedYear = null;
                Int16? SelectedItemategoryID = null;
                int? SelectedddlMajorMinorID = null;

                //if (ddlZone.SelectedItem.Value != String.Empty)
                //{
                //    SelectedZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
                //}
                //SearchCriteria.Add(ZoneIDKey, ddlZone.SelectedItem.Value);

                //if (ddlCircle.SelectedItem.Value != String.Empty)
                //{
                //    SelectedCircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
                //}
                //SearchCriteria.Add(CircleIDKey, ddlCircle.SelectedItem.Value);

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

                if (ddlItemCategory.SelectedItem.Value != String.Empty)
                {
                    SelectedItemategoryID = Convert.ToInt16(ddlItemCategory.SelectedItem.Value);
                }
                SearchCriteria.Add(ItemCategoryIDKey, ddlItemCategory.SelectedItem.Value);

                var DivStoreID = _DivisionStoreID == 0 ? (long?)null : _DivisionStoreID;

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                DataSet DS = new DivisionStoreBLL().GetDivisionStoreSearch(DivStoreID,
                   SelectedDivisionID, SelectedYear, SelectedItemategoryID);
                //var LstDivisionStore = IeDivisionStore.Select(dataRow => new
                //{
                //    ItemID = dataRow.Field<long>("ItemID"),
                //    ItemName = dataRow.Field<string>("ItemName"),
                //    QuantityAvailable = dataRow.Field<int>("QuantityAvailable")
                //}).ToList();
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvDivisionStore.DataSource = DS.Tables[0];
                }

                gvDivisionStore.DataBind();
                SearchCriteria.Add(PageIndexKey, gvDivisionStore.PageIndex);
                Session[SessionValues.SearchDivisionStore] = SearchCriteria;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionStore_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDivisionStore.EditIndex = -1;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionStore_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDivisionStore.PageIndex = e.NewPageIndex;
                BindSearchResultsGrid(0);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionStore_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long DivisionStoreID =
                    Convert.ToInt64(((Label)gvDivisionStore.Rows[e.RowIndex].FindControl("lblDivisionStoreID")).Text);

                if (new FloodOperationsBLL().IsEmergencyPurchaseDependencyExists(Convert.ToInt64(DivisionStoreID)))
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = new FloodOperationsBLL().DeleteJointEmergencyPurchase(DivisionStoreID);

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
                //gvDivisionStore.Visible = true;
            }
            catch (Exception ex)
            {
                //gvDivisionStore.Visible = false;
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

        protected void gvDivisionStore_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ItemDetail")
                {
                    Session["DivisionID"] = "";
                    Session["ZoneName"] = "";
                    Session["CircleName"] = "";
                    Session["DivisionName"] = "";
                    Session["ItemID"] = "";
                    Session["QuantityAvailable"] = "";
                    Session["Year"] = "";
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvDivisionStore.DataKeys[row.RowIndex];
                    Session["DivisionID"] = ddlDivision.SelectedValue;
                    Session["ZoneName"] = Convert.ToString(ddlZone.SelectedItem.Text);
                    Session["CircleName"] = Convert.ToString(ddlCircle.SelectedItem.Text);
                    Session["DivisionName"] = Convert.ToString(ddlDivision.SelectedItem.Text);
                    Session["Year"] = Convert.ToInt32(ddlYear.SelectedItem.Value);
                    Session["ItemID"] = Convert.ToString(key["ItemID"]);
                    Session["QuantityAvailable"] = Convert.ToString(key["QuantityAvailable"]);
                    Response.Redirect("ViewDetailDivisionStore.aspx");
                }
                else if (e.CommandName == "UpdateItem")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvDivisionStore.DataKeys[row.RowIndex];
                    Response.Redirect("ItemStatusDivisionStore.aspx?DivisionID=" + ddlDivision.SelectedValue + "&CatID=" +
                                      ddlItemCategory.SelectedValue + "&ItemID=" + Convert.ToString(key["ItemID"]) +
                                      "&Year=" + ddlYear.SelectedValue);
                }
                else if (e.CommandName == "ItemHistory")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvDivisionStore.DataKeys[row.RowIndex];
                    Response.Redirect("HistoryDivisionStore.aspx?ZoneName=" + Convert.ToString(ddlZone.SelectedItem.Text) + "&CircleName=" + Convert.ToString(ddlCircle.SelectedItem.Text) + "&DivisionName=" + Convert.ToString(ddlDivision.SelectedItem.Text) +
                        "&QuantityAvailable=" + Convert.ToString(key["QuantityAvailable"]) + "&DivisionID=" + Convert.ToInt64(ddlDivision.SelectedValue) + "&CatID=" + Convert.ToInt32(ddlItemCategory.SelectedValue) + "&ItemID=" + Convert.ToString(key["ItemID"]) + "&Year=" + ddlYear.SelectedValue, false);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionStore_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    #region Control
                    LinkButton hlDetails = (LinkButton)e.Row.FindControl("hlDetails");
                    LinkButton hlUpdateItemStatus = (LinkButton)e.Row.FindControl("hlUpdateItemStatus");
                    LinkButton lnkDSHistory = (LinkButton)e.Row.FindControl("lnkDSHistory");
                    Label lblCampSite = (Label)e.Row.FindControl("lblCampSite");

                    #endregion

                    if (Convert.ToInt32(ddlItemCategory.SelectedValue) == 4)
                    {
                        hlDetails.Visible = false;
                        hlUpdateItemStatus.Visible = false;
                        lnkDSHistory.Visible = true;

                        if (lblCampSite.Text == "0")
                        {
                            lblCampSite.Text = "No";
                        }
                        else
                        {
                            lblCampSite.Text = "Yes";
                        }
                    }
                    else
                    {
                        hlDetails.Visible = true;
                        hlUpdateItemStatus.Visible = true;
                        lnkDSHistory.Visible = false;

                    }
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[2].Text = "Available in Division Store";
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}