using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class ReceivedStockDivisionStore : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys

        private DivisionStoreBLL bllDivisionStoreBLL = new DivisionStoreBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownlists();
                    //  hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionStore/SearchReceived.aspx?ID={0}");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
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
                Dropdownlist.DDLDSEntryType(ddlReceivedStockType, "Received", false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLItemCategory(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLInfrastructureType(ddlInfrastructureType, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID, (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, (int)Constants.DropDownFirstOption.All);
                //  BindUserLocation();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlInfrastructureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlItemCategory.SelectedItem.Value == "")
                {
                    gvReceivedStockInfrastructure.Visible = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    gvReceivedStockInfrastructure.Visible = true;
                    BindReceivedStockInfrastructureGridView();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindReceivedStockInfrastructureGridView()
        {
            try
            {
                DataSet DS = new DivisionStoreBLL().DSReceivedStockInfraStructure(Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), Convert.ToInt64(ddlReceivedStockType.SelectedValue), Convert.ToInt64(ddlItemCategory.SelectedValue));
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvReceivedStockInfrastructure.DataSource = DS.Tables[0];
                }
                gvReceivedStockInfrastructure.DataBind();
                //var LstItem = IeDivisionStoreItem.Select(dataRow => new
                //{
                //    ItemName = dataRow.Field<string>("ItemName"),
                //    ItemId = dataRow.Field<long>("ItemID"),
                //    DSID = dataRow.Field<long>("DSID"),
                //    ReceivedQty = dataRow.Field<Int32>("ReceivedQty"),
                //    IssuedQty = dataRow.Field<Int32>("IssuedQty"),
                //}).ToList();
                //gvReceivedStockInfrastructure.DataSource = LstItem;
                //gvReceivedStockInfrastructure.DataBind();
                gvReceivedStockInfrastructure.Enabled = true;
                if (gvReceivedStockInfrastructure.Rows.Count > 0)
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            bool IsSave = false;
            for (int m = 0; m < gvReceivedStockInfrastructure.Rows.Count; m++)
            {
                try
                {
                    string ItemID = gvReceivedStockInfrastructure.DataKeys[m].Values[0].ToString();
                    string ExistReceivedQty = gvReceivedStockInfrastructure.DataKeys[m].Values[2].ToString();
                    string DSID = gvReceivedStockInfrastructure.DataKeys[m].Values[3].ToString();
                    Label lbl_flood = (Label)gvReceivedStockInfrastructure.Rows[m].FindControl("lbl_flood");
                    TextBox txt_Received_Qty = (TextBox)gvReceivedStockInfrastructure.Rows[m].FindControl("txt_Received_Qty");
                    if (Convert.ToInt32(DSID) != 0)
                    {
                        if (Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                        {
                            int qtyrec = -1 * Convert.ToInt32(ExistReceivedQty);
                            bllDivisionStoreBLL.DivisionStoreInsertion(Convert.ToInt64(DSID), DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, 11, Convert.ToInt32(qtyrec), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, Convert.ToInt64(DSID), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                        }
                    }
                    if (txt_Received_Qty.Text != "")
                    {
                        bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, Convert.ToInt32(ddlReceivedStockType.SelectedValue), Convert.ToInt32(txt_Received_Qty.Text), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                        IsSave = true;
                    }
                }
                catch (Exception exp)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                    new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
            try
            {
                if (IsSave == true)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    BindReceivedStockInfrastructureGridView();
                    //   gvReceivedStockInfrastructure.Enabled = false;
                    //  btnSave.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlInfrastructureName_SelectedIndexChanged(object sender, EventArgs e)
        {
            object bllGetStructureTypeIDByvalue = null;
            if (Convert.ToInt64(ddlInfrastructureType.SelectedValue) == 1)
            {
                bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(1, Convert.ToInt64(ddlInfrastructureName.SelectedValue));
                InfrastructureTypeID.Value = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
            }
            else if (Convert.ToInt64(ddlInfrastructureType.SelectedValue) == 2)
            {
                bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(2, Convert.ToInt64(ddlInfrastructureName.SelectedValue));
                InfrastructureTypeID.Value = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
            }
            else if (Convert.ToInt64(ddlInfrastructureType.SelectedValue) == 3)
            {
                bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(3, Convert.ToInt64(ddlInfrastructureName.SelectedValue));
                InfrastructureTypeID.Value = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
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
    }
}