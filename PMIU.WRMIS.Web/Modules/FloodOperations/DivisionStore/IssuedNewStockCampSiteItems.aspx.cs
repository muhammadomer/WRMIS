using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class IssuedNewStockCampSiteItems : BasePage
    {
        private DivisionStoreBLL bllDivisionStoreBLL = new DivisionStoreBLL();
        private int DivisionID, StructureID, StructureTypeID, RD, StockType, FFPcampSiteID;
        private string infrastructureType, InfraName;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);
                StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
                StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);
                StockType = Utility.GetNumericValueFromQueryString("StockType", 0);
                FFPcampSiteID = Utility.GetNumericValueFromQueryString("FFPcampSiteID", 0);
                hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionStore/IssuedNewStockInfrastructureCampSite.aspx?DivisionID=" + DivisionID.ToString());

                if (!IsPostBack)
                {
                    infrastructureType = Utility.GetStringValueFromQueryString("infrastructureType", "");
                    InfraName = Utility.GetStringValueFromQueryString("InfraName", "");
                    RD = Utility.GetNumericValueFromQueryString("RD", 0);

                    SetPageTitle();

                    lbl_infra_type.Text = infrastructureType.ToString();
                    lbl_infrastructure.Text = InfraName.ToString();
                    lbl_RD.Text = Convert.ToString(Calculations.GetRDText(RD));
                    if (lbl_infra_type.Text.Contains("Barrage") || lbl_infra_type.Text.Contains("Head work"))
                    {
                        lbl_RD.Visible = false;
                        lblRdName.Visible = false;
                    }
                    else
                    {
                        lbl_RD.Visible = true;
                        lblRdName.Visible = true;
                    }
                    LoadItemCategory();
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

        private void LoadItemCategory()
        {
            try
            {
                Dropdownlist.DDLItemCategory(ddlItemCategory, false);
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
                }
                else
                {
                    if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SE) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.Secretary) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ChiefIrrigation))
                    {
                        gvItems.Visible = true;
                        gvItems.Enabled = true;
                        BindItemsGridView();
                    }
                    else
                    {
                        gvItems.Visible = false;
                    }
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
                //IEnumerable<DataRow> IeDivisionStoreItem = new DivisionStoreBLL().DSIssuedStockCamSiteItems(DivisionID, StructureTypeID, StructureID, StockType, Convert.ToInt64(ddlItemCategory.SelectedValue), FFPcampSiteID);
                //var LstItem = IeDivisionStoreItem.Select(dataRow => new
                //{
                //    ItemName = dataRow.Field<string>("ItemName"),
                //    ItemId = dataRow.Field<long>("ItemID"),
                //    DSID = dataRow.Field<long?>("DSID"),
                //    QuantityAvailableInStore = dataRow.Field<Int32?>("QuantityAvailableInStore"),
                //    QuantityApproved = dataRow.Field<Int32?>("QuantityApproved"),
                //    IssuedQty = dataRow.Field<Int32?>("IssuedQty"),
                //}).ToList();
                //gvItems.DataSource = LstItem;

                DataSet DS = new DivisionStoreBLL().DSIssuedStockCamSiteItems(DivisionID, StructureTypeID, StructureID, StockType, Convert.ToInt64(ddlItemCategory.SelectedValue), FFPcampSiteID);

                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvItems.DataSource = DS.Tables[0];
                }

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
                gvItems.DataBind();

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

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                Int64 IssuedQty = Convert.ToInt64(key["IssuedQty"]);
                Int64 AvailableIssuedCount = Convert.ToInt64(key["AvailableIssuedCount"]);

                CheckBox chk_Qty = (CheckBox)e.Row.FindControl("chk_Qty");
                TextBox txt_Received_Qty = (TextBox)e.Row.FindControl("txt_Received_Qty");
                Label lbl_QuantityAvailableInStore = (Label)e.Row.FindControl("lbl_StoreQty");
                Label lbl_StoreQtyAsset = (Label)e.Row.FindControl("lbl_StoreQtyAsset");

                chk_Qty.Visible = false;
                txt_Received_Qty.Visible = false;

                if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                {

                    lbl_StoreQtyAsset.Visible = true;
                    lbl_QuantityAvailableInStore.Visible = false;

                    if (lbl_QuantityAvailableInStore.Text != "0")
                    {
                        lbl_StoreQtyAsset.Text = "Yes";
                    }
                    else if (lbl_QuantityAvailableInStore.Text == "0")
                    {
                        lbl_StoreQtyAsset.Text = "No";
                    } 
                    
                    chk_Qty.Visible = true;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                    if (AvailableIssuedCount != 0 && IssuedQty > 0)
                    {
                        chk_Qty.Checked = true;
                        chk_Qty.Enabled = false;
                    }
                    else
                    {
                        if (Convert.ToInt32(lbl_QuantityAvailableInStore.Text) == 0)
                        {
                            chk_Qty.Checked = false;
                            chk_Qty.Enabled = false;
                        }
                        else
                        {
                            chk_Qty.Checked = false;
                            chk_Qty.Enabled = true;
                        }
                    }

                    //e.Row.Cells[2].Visible = false;
                    //e.Row.Cells[3].Visible = false;

                    //if (IssuedQty != 0)
                    //{
                    //    chk_Qty.Checked = true;
                    //    chk_Qty.Enabled = false;
                    //}
                    //else
                    //{
                    //    if (lbl_QuantityAvailableInStore.Text != "" && lbl_QuantityAvailableInStore.Text != "0")
                    //    {
                    //        chk_Qty.Enabled = true;
                    //        chk_Qty.Checked = false;
                    //    }
                    //    else
                    //    {
                    //        chk_Qty.Enabled = false;
                    //    }
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
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[1].Text = "Available in Store";
                    e.Row.Cells[4].Text = "Issued Status";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            bool IsSave = false;
            for (int m = 0; m < gvItems.Rows.Count; m++)
            {
                try
                {
                    string str_storeQty = "";
                    string ItemID = gvItems.DataKeys[m].Values[0].ToString();
                    string ExistReceivedQty = gvItems.DataKeys[m].Values[2].ToString();
                    string DSID = gvItems.DataKeys[m].Values[3].ToString();
                    Int64 AvailableIssuedCount = Convert.ToInt64(gvItems.DataKeys[m].Values[5].ToString());
                    TextBox txt_Received_Qty = (TextBox)gvItems.Rows[m].FindControl("txt_Received_Qty");
                    Label lbl_IssuedQty = (Label)gvItems.Rows[m].FindControl("lbl_IssuedQty");
                    Label lbl_StoreQty = (Label)gvItems.Rows[m].FindControl("lbl_StoreQty");
                    str_storeQty = lbl_StoreQty.Text;
                    if (str_storeQty == "")
                    {
                        str_storeQty = "0";
                    }
                    else
                    {
                        str_storeQty = lbl_StoreQty.Text;
                    }

                    // LOOK this if and else condition and verify it
                    if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                    {
                        CheckBox chk_Qty = (CheckBox)gvItems.Rows[m].FindControl("chk_Qty");

                        if (chk_Qty.Enabled == true)
                        {
                            if ((Convert.ToInt64(lbl_IssuedQty.Text) == 0 && AvailableIssuedCount == 0) || (Convert.ToInt64(lbl_IssuedQty.Text) == 0 && AvailableIssuedCount != 0) || (Convert.ToInt64(lbl_IssuedQty.Text) != 0 && AvailableIssuedCount == 0))
                            {
                                if (chk_Qty.Checked == true)
                                {
                                    bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), Convert.ToInt32(gvItems.DataKeys[m].Values[4].ToString()), StockType, 1, StructureTypeID, StructureID, FFPcampSiteID, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                    //bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, StockType, Convert.ToInt32(txt_Received_Qty.Text), StructureTypeID, StructureID, FFPcampSiteID, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                    IsSave = true;
                                }

                            }
                            else
                            {
                                if (chk_Qty.Checked == true)
                                {
                                    Master.ShowMessage("Item can not be issue more than one time", SiteMaster.MessageType.Error);
                                    return;
                                }
                            }
                        }

                    }
                    else
                    {

                        if (Convert.ToInt64(txt_Received_Qty.Text) <= Convert.ToInt64(str_storeQty))
                        {
                            //if (Convert.ToInt32(DSID) != 0)
                            //{
                            //    if (Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                            //    {
                            //        int qtyrec = -1 * Convert.ToInt32(ExistReceivedQty);
                            //        bllDivisionStoreBLL.DivisionStoreInsertion(Convert.ToInt64(DSID), DateTime.Now, DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, 11, Convert.ToInt32(qtyrec), StructureTypeID, StructureID, FFPcampSiteID, null, Convert.ToInt64(DSID), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            //    }
                            //}
                            //if (txt_Received_Qty.Text != "" && Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                            //{
                            if (txt_Received_Qty.Text != "")
                            {
                                //string StrIssueQty = "";
                                //if (lbl_IssuedQty.Text == "")
                                //{
                                //    StrIssueQty = "0";
                                //}
                                //else
                                //{
                                //    StrIssueQty = lbl_IssuedQty.Text;
                                //}
                                //int receivedQty = Convert.ToInt32(StrIssueQty) + Convert.ToInt32(txt_Received_Qty.Text);

                                bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, StockType, Convert.ToInt32(txt_Received_Qty.Text), StructureTypeID, StructureID, FFPcampSiteID, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                //bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, StockType, Convert.ToInt32(receivedQty), StructureTypeID, StructureID, FFPcampSiteID, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                IsSave = true;
                            }
                        }
                        else
                        {
                            Master.ShowMessage("Issue Quantity Cannot Be Greater Than Quantity Available in Store", SiteMaster.MessageType.Error);
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

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}