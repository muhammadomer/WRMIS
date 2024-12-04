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
    public partial class ItemStatusDivisionStore : BasePage
    {
        #region View State keys
        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion
        DivisionStoreBLL bllDivisionStoreBLL = new DivisionStoreBLL();
        int DivisionID, StructureID, CatID, ItemID, StockType, FFPcampSiteID;
        int Year;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);
                    CatID = Utility.GetNumericValueFromQueryString("CatID", 0);
                    ItemID = Utility.GetNumericValueFromQueryString("ItemID", 0);
                    Year = Utility.GetNumericValueFromQueryString("Year", 0);

                    SetPageTitle();
                    ddlCategory.Enabled = false;
                    ddlDivision.Enabled = false;
                    ddlItem.Enabled = false;
                    BindDropdownlists();
                    
                    txtAvailableQty.Text = "";
                    AvailableDivisionQty();
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionStore/SearchDivisionStore.aspx?DivisionStoreID={0}", 0);
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
                
                Dropdownlist.DDLItemCategoryByID(ddlCategory,CatID, false);

                Dropdownlist.SetSelectedValue(ddlCategory, Convert.ToString(CatID));
                Dropdownlist.DDLItemByID(ddlItem, Convert.ToInt32(ItemID), false);
                Dropdownlist.SetSelectedValue(ddlItem, Convert.ToString(ItemID));
                Dropdownlist.DDLDSEntryType(ddlcondition, "Updated", false, (int)Constants.DropDownFirstOption.Select);
                //BindUserLocation();
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.Select);
                Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(DivisionID));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);
                //CatID = Utility.GetNumericValueFromQueryString("CatID", 0);
                //ItemID = Utility.GetNumericValueFromQueryString("ItemID", 0);
                //Year = Utility.GetNumericValueFromQueryString("Year", 0);
                //Dropdownlist.DDLItemByID(ddlItem, Convert.ToInt32(ItemID), false);
                //Dropdownlist.SetSelectedValue(ddlItem, Convert.ToString(ItemID));
               // txtAvailableQty.Text = "";
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);
            //CatID = Utility.GetNumericValueFromQueryString("CatID", 0);
            //ItemID = Utility.GetNumericValueFromQueryString("ItemID", 0);
            //Year = Utility.GetNumericValueFromQueryString("Year", 0);
            //AvailableDivisionQty();
        }
        private void AvailableDivisionQty()
        {
            try
            {

                DataSet DS = new DivisionStoreBLL().DSAvailableDivisionQtyItemWise(Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(Year), Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlItem.SelectedValue));
                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = DS.Tables[0].Rows[0];
                    txtAvailableQty.Text = DR["AvailableDivisionStore"].ToString();
                }
                else
                {
                    txtAvailableQty.Text = "";
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                string reason = string.Empty;
                if (txteffectedQty.Text != "" && txteffectedQty.Text != "0")
                {
                    if (Convert.ToDateTime(txtDate.Text) <= DateTime.Today)
                    {
                        if (Convert.ToDateTime(txtDate.Text).Year == DateTime.Now.Year)
                        {
                            if (txtAvailableQty.Text != "" && txtAvailableQty.Text != "0")
                            {
                                if (Convert.ToInt32(txteffectedQty.Text) <= Convert.ToInt32(txtAvailableQty.Text))
                                {
                                    bllDivisionStoreBLL.DivisionStoreInsertion(0, Convert.ToDateTime(txtDate.Text), Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlItem.SelectedValue), null, Convert.ToInt32(ddlcondition.SelectedValue), Convert.ToInt32(txteffectedQty.Text), null, null, null, txtreason.Text, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                    SearchDivisionStore.IsSaved = true;
                                    Response.Redirect("SearchDivisionStore.aspx?DivisionStoreID=" + 1, false);
                                }
                                else
                                {
                                    Master.ShowMessage("Quantity Affected Must Be Less Or Equal To Available Quantity", SiteMaster.MessageType.Error);
                                    return;
                                }
                            }
                            else
                            {
                                Master.ShowMessage("Available Quantity Required", SiteMaster.MessageType.Error);
                                return;
                            }

                        }
                        else
                        {
                            Master.ShowMessage("Previous Year Not Allowed", SiteMaster.MessageType.Error);
                            return;
                        }

                    }
                    else
                    {
                        Master.ShowMessage("Future Date Not Allowed", SiteMaster.MessageType.Error);
                        return;
                    }

                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
          //  txtAvailableQty.Text = "";
          //  ddlItem.ClearSelection();
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

                            Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(DivisionID));

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