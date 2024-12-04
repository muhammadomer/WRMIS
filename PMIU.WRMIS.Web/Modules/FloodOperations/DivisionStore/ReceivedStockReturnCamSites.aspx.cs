using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;


namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class ReceivedStockReturnCamSites : BasePage
    {
        #region View State keys
        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();
                    BindDropdownlists();
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                }

            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindDropdownlists()
        {
            try
            {

                Dropdownlist.DDLDSEntryType(ddlStockType, "Received", false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
                //BindUserLocation();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindGridViewCampSite()
        {
            try
            {
                IEnumerable<DataRow> IeDivisionStoreItem = new DivisionStoreBLL().DSReceivedStockCampSite(Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlStockType.SelectedValue), null, null, null, null);
                var LstItem = IeDivisionStoreItem.Select(dataRow => new
                {
                    StructureType = dataRow.Field<string>("StructureType"),
                    InfraStructureName = dataRow.Field<long>("InfraStructureName"),
                    StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                    StructureID = dataRow.Field<long>("StructureID"),
                    RD = dataRow.Field<Int32>("RD"),

                }).ToList();
                gvCampSite.DataSource = LstItem;
                gvCampSite.DataBind();
                gvCampSite.Enabled = true;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                BindGridViewCampSite();
                gvCampSite.Visible = true;

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

                ddlStockType.ClearSelection();
                gvCampSite.Visible = false;

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvCampSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataKey key = gvCampSite.DataKeys[e.Row.RowIndex];
                string RD = Convert.ToString(key.Values["RD"]);
                Label lblRD = (Label)e.Row.FindControl("lblRD");
                if (lblRD.Text != "")
                {
                    lblRD.Text = Calculations.GetRDText(Convert.ToInt64(RD));

                }
            }
        }

        protected void gvCampSite_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ItemDetail")
                {
                    Session["DivisionID"] = "";
                    Session["infrastructureType"] = "";
                    Session["InfraName"] = "";
                    Session["StructureTypeID"] = "";
                    Session["StructureID"] = "";
                    Session["RD"] = "";
                    Session["stockType"] = "";
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvCampSite.DataKeys[row.RowIndex];
                    string StructureTypeID = Convert.ToString(key["StructureTypeID"]);
                    string StructureID = Convert.ToString(key["StructureID"]);

                    Session["DivisionID"] = ddlDivision.SelectedValue;
                    Session["stockType"] = ddlStockType.SelectedValue;
                    Session["infrastructureType"] = row.Cells[0].Text;
                    Session["InfraName"] = row.Cells[1].Text;
                    Session["RD"] = row.Cells[2].Text;
                    Session["FFPcampSiteID"] = Convert.ToString(key["FFPcampSiteID"]);
                    Response.Redirect("ReceivedStockReturnCamSitesItems.aspx");
                }
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
                        if (mdlUser.UA_Designations.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
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

                            List<CO_Circle> lstCircle = new CircleBLL().GetFilteredCircles(SelectedZoneID, lstUserCircle);

                            long SelectedCircleID = lstCircle.FirstOrDefault().ID;

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
    }
}