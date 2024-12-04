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
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class IssuedNewStockInfrastructureCampSite : BasePage
    {
        private int DivisionID;

        protected void Page_Load(object sender, EventArgs e)
        {
            DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);

            if (!IsPostBack)
            {
                try
                {
                    //DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);

                    SetPageTitle();
                    if (rdolStatus.SelectedValue == "5")
                    {
                        divInfra.Visible = true;
                        divCampSite.Visible = false;
                        btnSave.Visible = true;
                        BindDropdownlists();
                    }
                    else
                    {
                        divCampSite.Visible = true;
                        divInfra.Visible = false;
                        btnSave.Visible = false;
                        BindCampSiteGridView();
                    }
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionStore/SearchIssuedStock.aspx?DivisionStoreID={0}", 0);
                }
                catch (Exception ex)
                {
                    new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RadioButtonListFormat", "<script>$('.My-Radio label').each(function () { $(this).css('margin-right', '25px'); $(this).css('margin-left', '3px'); });</script>", false);
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
                Dropdownlist.DDLItemCategory(ddlItemCategory, false, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.DDLInfrastructureType(ddlInfrastructureType, (int)Constants.DropDownFirstOption.All);
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
                    gvItems.Visible = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    gvItems.Visible = true;
                    BindItemsGridView();
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

                DataSet DS = new DivisionStoreBLL().DSIssuedStockInfraStructure(DivisionID, Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), Convert.ToInt64(rdolStatus.SelectedValue), Convert.ToInt64(ddlItemCategory.SelectedValue));

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
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindCampSiteGridView()
        {
            try
            {
                IEnumerable<DataRow> IeDivisionStoreItem = new DivisionStoreBLL().DSIssuedStockCampSite(DivisionID, null, null);
                var LstItem = IeDivisionStoreItem.Select(dataRow => new
                {
                    StructureType = dataRow.Field<string>("StructureType"),
                    InfraStructureName = dataRow.Field<string>("InfraStructureName"),
                    StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                    StructureID = dataRow.Field<long>("StructureID"),
                    RD = dataRow.Field<Int32>("RD"),
                    FFPcampSiteID = dataRow.Field<long>("FFPcampSiteID"),
                }).ToList();
                gvIssuedStockCampSite.DataSource = LstItem;
                gvIssuedStockCampSite.DataBind();
                gvIssuedStockCampSite.Enabled = true;
                gvIssuedStockCampSite.Visible = true;

                // gvIssuedStockCampSite.Visible = true;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rdolStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdolStatus.SelectedValue == "5")
            {
                divInfra.Visible = true;
                divCampSite.Visible = false;
                btnSave.Visible = true;
                ddlInfrastructureType.ClearSelection();
                ddlInfrastructureName.ClearSelection();
                ddlItemCategory.ClearSelection();
                gvItems.Visible = false;
                gvIssuedStockCampSite.Visible = false;
            }
            else
            {
                divCampSite.Visible = true;
                divInfra.Visible = false;
                btnSave.Visible = false;
                ddlInfrastructureType.ClearSelection();
                ddlInfrastructureName.ClearSelection();
                ddlItemCategory.ClearSelection();
                gvItems.Visible = false;
                gvIssuedStockCampSite.Visible = false;
                BindCampSiteGridView();
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

        protected void gvIssuedStockCampSite_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIssuedStockCampSite.PageIndex = e.NewPageIndex;
                BindCampSiteGridView();
            }
            catch (Exception exp)
            {
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

        protected void btnSave_Click(object sender, EventArgs e)
        {

            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            DivisionStoreBLL bllDivisionStoreBLL = new DivisionStoreBLL();
            bool IsSave = false;
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
            for (int m = 0; m < gvItems.Rows.Count; m++)
            {

                TextBox txt_Received_Qty = (TextBox)gvItems.Rows[m].FindControl("txt_Qty");
                Label lbl_StoreQty = (Label)gvItems.Rows[m].FindControl("lbl_StoreQty");
                if (txt_Received_Qty.Text != "")
                {
                    if (Convert.ToInt64(txt_Received_Qty.Text) > Convert.ToInt64(lbl_StoreQty.Text))
                    {
                        Master.ShowMessage("Issue Quantity Cannot Be Greater Than Quantity Available in Store",
                            SiteMaster.MessageType.Error);
                        return;
                    }
                }
            }

            for (int m = 0; m < gvItems.Rows.Count; m++)
            {
                try
                {

                    string str_storeQty = "";
                    string DSID = "";
                    string ItemID = gvItems.DataKeys[m].Values["ItemId"].ToString();
                    string ExistReceivedQty = gvItems.DataKeys[m].Values[2].ToString();
                    Int64 AvailableIssuedCount = Convert.ToInt64(gvItems.DataKeys[m].Values["AvailableIssuedCount"].ToString());
                    DSID = Convert.ToString(gvItems.DataKeys[m].Values[3].ToString());
                    TextBox txt_Received_Qty = (TextBox)gvItems.Rows[m].FindControl("txt_Qty");
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

                    if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                    {

                        CheckBox chk_Qty = (CheckBox)gvItems.Rows[m].FindControl("chk_Qty");

                        if (chk_Qty.Enabled == true)
                        {
                            if ((Convert.ToInt64(lbl_IssuedQty.Text) == 0 && AvailableIssuedCount == 0) ||
                                (Convert.ToInt64(lbl_IssuedQty.Text) == 0 && AvailableIssuedCount != 0) ||
                                (Convert.ToInt64(lbl_IssuedQty.Text) != 0 && AvailableIssuedCount == 0))
                            {
                                if (chk_Qty.Checked == true)
                                {
                                    bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), Convert.ToInt32(gvItems.DataKeys[m].Values[4].ToString()),
                                        Convert.ToInt32(rdolStatus.SelectedValue), 1,
                                        Convert.ToInt64(InfrastructureTypeID.Value),
                                        Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null,
                                        Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
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


                            if (Convert.ToInt64(txt_Received_Qty.Text) <= Convert.ToInt64(lbl_StoreQty.Text))
                            {

                                //if (Convert.ToInt32(DSID) != 0)
                                //{
                                //    if (Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                                //    {
                                //        int qtyrec = -1 * Convert.ToInt32(ExistReceivedQty);
                                //        bllDivisionStoreBLL.DivisionStoreInsertion(Convert.ToInt64(DSID), DateTime.Now, DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, 11, Convert.ToInt32(qtyrec), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, Convert.ToInt64(DSID), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                //        IsSave = true;
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
                                    //   int receivedQty = Convert.ToInt32(StrIssueQty) + Convert.ToInt32(txt_Received_Qty.Text);

                                    bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, DivisionID,
                                        Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null,
                                        Convert.ToInt32(rdolStatus.SelectedValue), Convert.ToInt32(txt_Received_Qty.Text),
                                        Convert.ToInt64(InfrastructureTypeID.Value),
                                        Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null,
                                        Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                                    //  bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, DivisionID, Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, Convert.ToInt32(rdolStatus.SelectedValue), Convert.ToInt32(receivedQty), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
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

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                Int64 IssuedQty = Convert.ToInt64(key["IssuedQty"]);
                Int64 AvailableIssuedCount = Convert.ToInt64(key["AvailableIssuedCount"]);

                CheckBox chk_Qty = (CheckBox)e.Row.FindControl("chk_Qty");
                TextBox txt_Qty = (TextBox)e.Row.FindControl("txt_Qty");
                Label lbl_QuantityAvailableInStore = (Label)e.Row.FindControl("lbl_StoreQty");
                Label lbl_StoreQtyAsset = (Label)e.Row.FindControl("lbl_StoreQtyAsset");


                chk_Qty.Visible = false;
                txt_Qty.Visible = false;

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
                    txt_Qty.Visible = true;
                }

                //if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                //{
                //    chk_Qty.Visible = true;

                //    e.Row.Cells[2].Visible = false;
                //    e.Row.Cells[3].Visible = false;

                //    if (IssuedQty != 0)
                //    {
                //        chk_Qty.Checked = true;
                //        chk_Qty.Enabled = false;
                //    }
                //    else
                //    {
                //        if (lbl_QuantityAvailableInStore.Text != "" && lbl_QuantityAvailableInStore.Text != "0")
                //        {
                //            chk_Qty.Checked = false;
                //        }
                //        else
                //        {
                //            chk_Qty.Enabled = false;
                //        }
                //    }
                //}
                //else
                //{
                //    txt_Qty.Visible = true;
                //}


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

        protected void gvIssuedStockCampSite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataKey key = gvIssuedStockCampSite.DataKeys[e.Row.RowIndex];
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

        protected void gvIssuedStockCampSite_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ItemDetail")
                {
                    //Session["infrastructureType"] = "";
                    //Session["InfraName"] = "";
                    //Session["StructureTypeID"] = "";
                    //Session["StructureID"] = "";
                    //Session["RD"] = "";
                    //Session["stockType"] = "";
                    //Session["FFPcampSiteID"] = "";
                    //Session["stockType"] = rdolStatus.SelectedValue;
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    DataKey key = gvIssuedStockCampSite.DataKeys[row.RowIndex];
                    //Session["StructureTypeID"] = Convert.ToString(key["StructureTypeID"]);
                    //Session["StructureID"] = Convert.ToString(key["StructureID"]);
                    //Session["infrastructureType"] = Convert.ToString(key["StructureType"]);
                    //Session["InfraName"] = Convert.ToString(key["InfraStructureName"]);
                    //Session["FFPcampSiteID"] = Convert.ToString(key["FFPcampSiteID"]);
                    //Session["RD"] = Convert.ToString(key["RD"]);
                    Response.Redirect("IssuedNewStockCampSiteItems.aspx?DivisionID=" + DivisionID.ToString() + "&FFPcampSiteID=" + Convert.ToString(key["FFPcampSiteID"]) + "&StructureTypeID=" + Convert.ToString(key["StructureTypeID"]) + "&StructureID=" + Convert.ToString(key["StructureID"]) + "&StockType=" + rdolStatus.SelectedValue + "&infrastructureType=" + Convert.ToString(key["StructureType"]) + "&InfraName=" + Convert.ToString(key["InfraStructureName"]) + "&RD=" + Convert.ToString(key["RD"]));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}