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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class ReceivedStockReturnPurchased : BasePage
    {
        #region View State keys

        public const string UserIDKey = "UserID";
        public const string UserCircleKey = "UserCircle";
        public const string UserDivisionKey = "UserDivision";

        #endregion View State keys

        private DivisionStoreBLL bllDivisionStoreBLL = new DivisionStoreBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();
                    BindDropdownlists();
                    if (SessionManagerFacade.UserInformation.UA_Designations.ID ==
                        Convert.ToInt64(Constants.Designation.XEN))
                    {
                        btnSave.Visible = true;
                        gvItems.Enabled = true;
                        gvReceivedStockInfrastructure.Enabled = true;
                    }
                    else
                    {
                        btnSave.Visible = false;
                        gvItems.Enabled = false;
                        gvReceivedStockInfrastructure.Enabled = false;
                    }
                    hlBack.NavigateUrl =
                        string.Format("~/Modules/FloodOperations/DivisionStore/SearchReceivedStock.aspx");
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(
                        Constants.MessageCategory.WebApp);
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
                //Dropdownlist.DDLItemCategory(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLInfrastructureType(ddlInfrastructureType, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLDivisionsByUserID(ddlDivision, (long)SessionManagerFacade.UserInformation.ID,
                    (long)SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID,
                    (int)Constants.DropDownFirstOption.All);
                //BindUserLocation();
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
                    gvItems.Visible = false;
                    gvReceivedStockInfrastructure.Visible = false;
                    gvCampSite.Visible = false;
                    gvFFP.Visible = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    if (ddlStockType.SelectedValue == "2")
                    {
                        gvItems.Visible = true;
                        BindItemsGridView();
                        gvItems.Enabled = true;
                    }
                    else if (ddlStockType.SelectedValue == "3")
                    {
                        gvReceivedStockInfrastructure.Visible = true;
                        BindReceivedStockInfrastructureGridView();
                        gvReceivedStockInfrastructure.Enabled = true;
                    }
                    else if (ddlStockType.SelectedValue == "4")
                    {
                        gvCampSite.Visible = true;
                        BindGridViewCampSite();
                        gvCampSite.Enabled = true;
                    }
                    else if (ddlStockType.SelectedValue == "1")
                    {
                        gvFFP.Visible = true;
                        BindGridViewFFP();
                        gvFFP.Enabled = true;
                    }
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
                DataSet DS = new DivisionStoreBLL().DSReceivedStockInfraStructure(Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), Convert.ToInt64(ddlStockType.SelectedValue), Convert.ToInt64(ddlItemCategory.SelectedValue));

                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvReceivedStockInfrastructure.DataSource = DS.Tables[0];
                }
                gvReceivedStockInfrastructure.DataBind();
                //var LstItem = IeDivisionStoreItem.Select(dataRow => new
                //{
                //    ItemName = dataRow.Field<string>("ItemName"),
                //    ItemId = dataRow.Field<long>("ItemID"),
                //    DSID = dataRow.Field<long?>("DSID"),
                //    ReceivedQty = dataRow.Field<Int32?>("ReceivedQty"),
                //    IssuedQty = dataRow.Field<Int32?>("IssuedQty"),
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

        private void BindItemsGridView()
        {
            try
            {
                IEnumerable<DataRow> IeDivisionStoreItem =
                    new DivisionStoreBLL().DSReceivedStockPurchased(Convert.ToInt64(ddlDivision.SelectedValue),
                        Convert.ToInt64(InfrastructureTypeID.Value),
                        Convert.ToInt64(ddlInfrastructureName.SelectedValue),
                        Convert.ToInt64(ddlStockType.SelectedValue), Convert.ToInt64(ddlItemCategory.SelectedValue));
                var LstItem = IeDivisionStoreItem.Select(dataRow => new
                {
                    ItemName = dataRow.Field<string>("ItemName"),
                    ItemId = dataRow.Field<long>("ItemID"),
                    DSID = dataRow.Field<long?>("DSID"),
                    ReceivedQty = dataRow.Field<Int32?>("ReceivedQty"),
                    PurchasedQty = dataRow.Field<Int32?>("PurchasedQty"),
                }).ToList();
                gvItems.DataSource = LstItem;
                gvItems.DataBind();
                gvItems.Enabled = true;
                if (gvItems.Rows.Count > 0)
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

        protected void gvReceivedStockInfrastructure_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataKey key = gvReceivedStockInfrastructure.DataKeys[e.Row.RowIndex];
                Int64 IssuedQty = Convert.ToInt64(key["IssuedQty"]);
                Int64 AvailableIssuedCount = Convert.ToInt64(key["AvailableIssuedCount"]);

                CheckBox chk_Qty = (CheckBox)e.Row.FindControl("chk_Qty");
                TextBox txt_Received_Qty = (TextBox)e.Row.FindControl("txt_Received_Qty");
                Label lbl_ReceivedQtyInfrastructure = (Label)e.Row.FindControl("lbl_ReceivedQtyInfrastructure");
                Label lbl_flood = (Label)e.Row.FindControl("lbl_flood");
                Label lbl_floodAsset = (Label)e.Row.FindControl("lbl_floodAsset");

                chk_Qty.Visible = false;
                txt_Received_Qty.Visible = false;

                if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                {

                    lbl_floodAsset.Visible = true;
                    lbl_flood.Visible = false;

                    if (AvailableIssuedCount > 0)
                    {
                        lbl_floodAsset.Text = "Yes";
                    }
                    else if (AvailableIssuedCount == 0)
                    {
                        lbl_floodAsset.Text = "No";
                    }

                    chk_Qty.Visible = true;

                    //e.Row.Cells[1].Visible = false;
                    e.Row.Cells[3].Visible = false;

                    //if (IssuedQty == 0)
                    //{
                    //    chk_Qty.Checked = false;
                    //    chk_Qty.Enabled = false;
                    //}
                    //else
                    //{
                    if (AvailableIssuedCount == 0)
                    {
                        chk_Qty.Checked = false;
                        chk_Qty.Enabled = false;
                    }
                    else
                    {
                        chk_Qty.Checked = false;
                        chk_Qty.Enabled = true;
                    }
                    //}
                }
                else
                {
                    txt_Received_Qty.Visible = true;
                }


            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                {
                    //e.Row.Cells[1].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[1].Text = "Issued";
                    e.Row.Cells[5].Text = "Received Status";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            bool IsSave = false;
            if (ddlStockType.SelectedValue == "2")
            {
                for (int m = 0; m < gvItems.Rows.Count; m++)
                {
                    try
                    {
                        string ItemID = gvItems.DataKeys[m].Values[0].ToString();
                        string ExistReceivedQty = gvItems.DataKeys[m].Values[2].ToString();
                        string DSID = gvItems.DataKeys[m].Values[3].ToString();
                        Label lbl_flood = (Label)gvItems.Rows[m].FindControl("lbl_flood");
                        Label lbl_ReceivedQty = (Label)gvItems.Rows[m].FindControl("lbl_ReceivedQty");
                        TextBox txt_Received_Qty = (TextBox)gvItems.Rows[m].FindControl("txt_Received_Qty");
                        //if (Convert.ToInt32(DSID) != 0)
                        //{
                        //    if (Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                        //    {
                        //        int qtyrec = -1 * Convert.ToInt32(ExistReceivedQty);
                        //        bllDivisionStoreBLL.DivisionStoreInsertion(Convert.ToInt64(DSID), DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, 11, Convert.ToInt32(qtyrec), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, Convert.ToInt64(DSID), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID),0);
                        //    }
                        //}

                        //if (txt_Received_Qty.Text != "" && Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                        //{
                        if (txt_Received_Qty.Text != "")
                        {
                            //string StrreceivedQty = "";
                            //if (lbl_ReceivedQty.Text == "")
                            //{
                            //    StrreceivedQty = "0";
                            //}
                            //else
                            //{
                            //    StrreceivedQty=lbl_ReceivedQty.Text;
                            //}
                            //  int receivedQty = Convert.ToInt32(StrreceivedQty) + Convert.ToInt32(txt_Received_Qty.Text);
                            bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now,
                                Convert.ToInt64(ddlDivision.SelectedValue),
                                Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null,
                                Convert.ToInt32(ddlStockType.SelectedValue), Convert.ToInt32(txt_Received_Qty.Text),
                                Convert.ToInt64(InfrastructureTypeID.Value),
                                Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null,
                                Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            //   bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, Convert.ToInt32(ddlStockType.SelectedValue), Convert.ToInt32(receivedQty), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            IsSave = true;
                        }
                    }
                    catch (Exception exp)
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                        new WRException((long)Session[SessionValues.UserID], exp).LogException(
                            Constants.MessageCategory.WebApp);
                    }
                }
                try
                {
                    if (IsSave == true)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        BindItemsGridView();
                        gvItems.Enabled = true;
                        btnSave.Enabled = true;
                    }
                }
                catch (Exception exp)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                    new WRException((long)Session[SessionValues.UserID], exp).LogException(
                        Constants.MessageCategory.WebApp);
                }
            }
            else if (ddlStockType.SelectedValue == "3")
            {
                for (int m = 0; m < gvReceivedStockInfrastructure.Rows.Count; m++)
                {
                    try
                    {
                        string ItemID = gvReceivedStockInfrastructure.DataKeys[m].Values[0].ToString();
                        string ExistReceivedQty = gvReceivedStockInfrastructure.DataKeys[m].Values[2].ToString();
                        string DSID = gvReceivedStockInfrastructure.DataKeys[m].Values[3].ToString();
                        Int64 AvailableIssuedCount = Convert.ToInt64(gvReceivedStockInfrastructure.DataKeys[m].Values[6].ToString());
                        Label lbl_flood = (Label)gvReceivedStockInfrastructure.Rows[m].FindControl("lbl_flood");
                        Label lbl_ReceivedQtyInfrastructure = (Label)gvReceivedStockInfrastructure.Rows[m].FindControl("lbl_ReceivedQtyInfrastructure");
                        TextBox txt_Received_Qty = (TextBox)gvReceivedStockInfrastructure.Rows[m].FindControl("txt_Received_Qty");

                        if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                        {
                            CheckBox chk_Qty = (CheckBox)gvReceivedStockInfrastructure.Rows[m].FindControl("chk_Qty");

                            if (chk_Qty.Enabled == true)
                            {
                                if (AvailableIssuedCount != 0)
                                {
                                    if (chk_Qty.Checked == true)
                                    {
                                        bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now,
                                                                                   Convert.ToInt64(ddlDivision.SelectedValue),
                                                                                   Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), Convert.ToInt32(gvReceivedStockInfrastructure.DataKeys[m].Values[5].ToString()),
                                                                                   Convert.ToInt32(ddlStockType.SelectedValue), 1,
                                                                                   Convert.ToInt64(InfrastructureTypeID.Value),
                                                                                   Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null,
                                                                                   Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                        IsSave = true;
                                    }

                                }
                                else
                                {
                                    Master.ShowMessage("Item can not be Received", SiteMaster.MessageType.Error);
                                    return;
                                }
                            }

                        }
                        else
                        {
                            //if (Convert.ToInt32(DSID) != 0)
                            //{
                            //    if (Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                            //    {
                            //        int qtyrec = -1 * Convert.ToInt32(ExistReceivedQty);
                            //        bllDivisionStoreBLL.DivisionStoreInsertion(Convert.ToInt64(DSID), DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, 11, Convert.ToInt32(qtyrec), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, Convert.ToInt64(DSID), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID),0);
                            //    }
                            //}
                            //if (txt_Received_Qty.Text != "" && Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                            //{
                            if (txt_Received_Qty.Text != "")
                            {
                                //string StrreceivedQty = "";
                                //if (lbl_ReceivedQtyInfrastructure.Text == "")
                                //{
                                //    StrreceivedQty = "0";
                                //}
                                //else
                                //{
                                //    StrreceivedQty = lbl_ReceivedQtyInfrastructure.Text;
                                //}
                                //  int receivedQty = Convert.ToInt32(StrreceivedQty) + Convert.ToInt32(txt_Received_Qty.Text);

                                bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now,
                                    Convert.ToInt64(ddlDivision.SelectedValue),
                                    Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null,
                                    Convert.ToInt32(ddlStockType.SelectedValue), Convert.ToInt32(txt_Received_Qty.Text),
                                    Convert.ToInt64(InfrastructureTypeID.Value),
                                    Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null,
                                    Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                //  bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, Convert.ToInt32(ddlStockType.SelectedValue), Convert.ToInt32(receivedQty), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                IsSave = true;
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                        new WRException((long)Session[SessionValues.UserID], exp).LogException(
                            Constants.MessageCategory.WebApp);
                    }
                }
                try
                {
                    if (IsSave == true)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        BindReceivedStockInfrastructureGridView();
                        gvReceivedStockInfrastructure.Enabled = true;
                        btnSave.Enabled = true;
                    }
                }
                catch (Exception exp)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                    new WRException((long)Session[SessionValues.UserID], exp).LogException(
                        Constants.MessageCategory.WebApp);
                }
            }
            else if (ddlStockType.SelectedValue == "1")
            {
                for (int m = 0; m < gvFFP.Rows.Count; m++)
                {
                    try
                    {
                        string ItemID = gvFFP.DataKeys[m].Values[0].ToString();
                        string ExistReceivedQty = gvFFP.DataKeys[m].Values[2].ToString();
                        string DSID = gvFFP.DataKeys[m].Values[3].ToString();
                        string FFPCampSiteID = "";
                        if (gvFFP.DataKeys[m].Values[4] != null)
                        {
                            FFPCampSiteID = gvFFP.DataKeys[m].Values[4].ToString();
                        }

                        TextBox txt_Received_Qty = (TextBox)gvFFP.Rows[m].FindControl("txt_Received_QtyFFP");
                        Label lbl_ReceivedQtyFFP = (Label)gvFFP.Rows[m].FindControl("lbl_ReceivedQtyFFP");
                        //if (Convert.ToInt32(DSID) != 0)
                        //{
                        //    if (Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                        //    {
                        //        int qtyrec = -1 * Convert.ToInt32(ExistReceivedQty);
                        //        bllDivisionStoreBLL.DivisionStoreInsertion(Convert.ToInt64(DSID), DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, 11, Convert.ToInt32(qtyrec), null, null, null, null, Convert.ToInt64(DSID), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                        //    }
                        //}
                        //if (txt_Received_Qty.Text != "" && Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                        //{
                        if (txt_Received_Qty.Text != "")
                        {
                            //string StrreceivedQty = "";
                            //if (lbl_ReceivedQtyFFP.Text == "")
                            //{
                            //    StrreceivedQty = "0";
                            //}
                            //else
                            //{
                            //    StrreceivedQty = lbl_ReceivedQtyFFP.Text;
                            //}
                            // int receivedQty = Convert.ToInt32(StrreceivedQty) + Convert.ToInt32(txt_Received_Qty.Text);

                            bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now,
                                Convert.ToInt64(ddlDivision.SelectedValue),
                                Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null,
                                Convert.ToInt32(ddlStockType.SelectedValue), Convert.ToInt32(txt_Received_Qty.Text),
                                null, null, null, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID),
                                0);
                            //   bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, Convert.ToInt32(ddlStockType.SelectedValue), Convert.ToInt32(receivedQty), null, null, null, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            IsSave = true;
                        }
                    }
                    catch (Exception exp)
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                        new WRException((long)Session[SessionValues.UserID], exp).LogException(
                            Constants.MessageCategory.WebApp);
                    }
                }
                try
                {
                    if (IsSave == true)
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        BindGridViewFFP();
                        gvFFP.Enabled = true;
                        btnSave.Enabled = true;
                    }
                }
                catch (Exception exp)
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                    new WRException((long)Session[SessionValues.UserID], exp).LogException(
                        Constants.MessageCategory.WebApp);
                }
            }
        }

        protected void ddlInfrastructureName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlInfrastructureType.SelectedItem.Value != "")
                {
                    long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructureType.SelectedItem.Value);
                    long InfrastructureNameSelectedValue = Convert.ToInt64(ddlInfrastructureName.SelectedItem.Value);
                    object bllGetStructureTypeIDByvalue = null;
                    if (InfrastructureTypeSelectedValue == 1)
                    {
                        bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(1, InfrastructureNameSelectedValue);
                        InfrastructureTypeID.Value = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
                    }
                    else if (InfrastructureTypeSelectedValue == 2)
                    {
                        bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(2, InfrastructureNameSelectedValue);
                        InfrastructureTypeID.Value = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
                    }
                    else if (InfrastructureTypeSelectedValue == 3)
                    {
                        bllGetStructureTypeIDByvalue = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(3, InfrastructureNameSelectedValue);
                        InfrastructureTypeID.Value = Convert.ToString(bllGetStructureTypeIDByvalue.GetType().GetProperty("InfrastructureTypeID").GetValue(bllGetStructureTypeIDByvalue));
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStockType.SelectedValue == "2")
            {
                div_Purchased.Visible = true;
                Div_category.Visible = true;
                ddlInfrastructureType.ClearSelection();
                ddlInfrastructureName.ClearSelection();
                Div_GvPurchased.Visible = true;
                Div_GvInfraStruct.Visible = false;
                Div_GvCampSite.Visible = false;
                Div_GvFFP.Visible = false;
                btnSave.Visible = true;
                Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                ddlItemCategory.ClearSelection();
            }
            else if (ddlStockType.SelectedValue == "3")
            {
                div_Purchased.Visible = true;
                Div_category.Visible = true;
                ddlInfrastructureType.ClearSelection();
                ddlInfrastructureName.ClearSelection();
                Div_GvCampSite.Visible = false;
                Div_GvPurchased.Visible = false;
                Div_GvInfraStruct.Visible = true;
                Div_GvFFP.Visible = false;
                btnSave.Visible = true;
                Dropdownlist.DDLItemCategory(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                ddlItemCategory.ClearSelection();
                gvReceivedStockInfrastructure.Visible = false;
            }
            else if (ddlStockType.SelectedValue == "4")
            {
                div_Purchased.Visible = false;
                Div_category.Visible = false;
                btnSave.Visible = false;
                Div_GvCampSite.Visible = true;
                Div_GvPurchased.Visible = false;
                Div_GvInfraStruct.Visible = false;
                Div_GvFFP.Visible = false;
                gvCampSite.Visible = true;
                BindGridViewCampSite();
                Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                ddlItemCategory.ClearSelection();
            }
            else if (ddlStockType.SelectedValue == "1")
            {
                div_Purchased.Visible = false;
                Div_category.Visible = true;
                btnSave.Visible = true;
                Div_GvCampSite.Visible = false;
                Div_GvPurchased.Visible = false;
                Div_GvInfraStruct.Visible = false;
                Div_GvFFP.Visible = true;
                gvFFP.Visible = true;
                Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                ddlItemCategory.ClearSelection();
            }
            else
            {
                Div_GvCampSite.Visible = false;
                Div_GvPurchased.Visible = false;
                Div_GvInfraStruct.Visible = false;
            }
        }

        protected void gvCampSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataKey key = gvCampSite.DataKeys[e.Row.RowIndex];
                string RD = Convert.ToString(key.Values["RD"]);
                string StructureType = Convert.ToString(key["StructureType"]);
                Label lblRD = (Label)e.Row.FindControl("lblRD");
                Label lblInfrastructureType = (Label)e.Row.FindControl("lblInfrastructureType");
                if (lblRD.Text != "")
                {
                    lblRD.Text = Calculations.GetRDText(Convert.ToInt64(RD));
                }
                if (StructureType.Contains("Barrage") || StructureType.Contains("Head work"))
                {
                    lblRD.Text = "";
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
                    Session["FFPcampSiteID"] = "";
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvCampSite.DataKeys[row.RowIndex];
                    Session["StructureTypeID"] = Convert.ToString(key["StructureTypeID"]);
                    Session["StructureID"] = Convert.ToString(key["StructureID"]);

                    Session["DivisionID"] = ddlDivision.SelectedValue;
                    Session["stockType"] = ddlStockType.SelectedValue;
                    Session["infrastructureType"] = Convert.ToString(key["StructureType"]);
                    Session["InfraName"] = Convert.ToString(key["InfraStructureName"]);
                    Session["FFPcampSiteID"] = Convert.ToString(key["FFPcampSiteID"]);
                    Session["RD"] = Convert.ToString(key["RD"]);
                    Response.Redirect("ReceivedStockReturnCamSitesItems.aspx");
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
                ddlStockType.ClearSelection();
                Div_GvCampSite.Visible = false;
                Div_GvPurchased.Visible = false;
                Div_GvInfraStruct.Visible = false;
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
                    InfraStructureName = dataRow.Field<string>("InfraStructureName"),
                    StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                    StructureID = dataRow.Field<long>("StructureID"),
                    RD = dataRow.Field<Int32>("RD"),
                    FFPcampSiteID = dataRow.Field<long>("FFPcampSiteID"),
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

        private void BindGridViewFFP()
        {
            try
            {
                IEnumerable<DataRow> IeDivisionStoreItem = new DivisionStoreBLL().DSIssuedStockFFP(Convert.ToInt64(ddlDivision.SelectedValue), null, null, Convert.ToInt64(ddlStockType.SelectedValue), Convert.ToInt64(ddlItemCategory.SelectedValue));
                var LstItem = IeDivisionStoreItem.Select(dataRow => new
                {
                    ItemName = dataRow.Field<string>("ItemName"),
                    ItemId = dataRow.Field<long>("ItemID"),
                    FFPCampSiteID = dataRow.Field<long?>("FFPCampSiteID"),
                    DSID = dataRow.Field<long?>("DSID"),
                    QuantityAvailableInStore = dataRow.Field<Int32?>("QuantityAvailableInStore"),
                    QuantityApprovedFFP = dataRow.Field<Int32?>("QuantityApprovedFFP"),
                    ReceivedQty = dataRow.Field<Int32?>("ReceivedQty"),
                }).ToList();
                gvFFP.DataSource = LstItem;
                gvFFP.DataBind();
                if (gvFFP.Rows.Count > 0)
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

        protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvItems.PageIndex = e.NewPageIndex;
                BindItemsGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReceivedStockInfrastructure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvReceivedStockInfrastructure.PageIndex = e.NewPageIndex;
                BindReceivedStockInfrastructureGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCampSite_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCampSite.PageIndex = e.NewPageIndex;
                BindGridViewCampSite();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFFP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFFP.PageIndex = e.NewPageIndex;
                //  BindGridViewCampSite();
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