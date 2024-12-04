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
    public partial class ReceivedStockReturnCamSitesItems : BasePage
    {
        private DivisionStoreBLL bllDivisionStoreBLL = new DivisionStoreBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();
                    LoadItemCategory();
                    lbl_infra_type.Text = Session["infrastructureType"].ToString();
                    lbl_infrastructure.Text = Session["InfraName"].ToString();
                    lbl_RD.Text = Convert.ToString(Calculations.GetRDText(Convert.ToInt64(Session["RD"].ToString())));
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
                    // hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/joint/ReceivedStockReturnCamSites.aspx?ID={0}", 0);
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionStore/ReceivedStockReturnPurchased.aspx");
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
                DataSet DS = new DivisionStoreBLL().DSReceivedStockCampSiteItems(Convert.ToInt64(Session["DivisionID"].ToString()), Convert.ToInt32(Session["stockType"].ToString()), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(Session["FFPcampSiteID"].ToString()));

                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvItems.DataSource = DS.Tables[0];
                }
                gvItems.DataBind();

                //var LstItem = IeDivisionStoreItem.Select(dataRow => new
                //{
                //    ItemName = dataRow.Field<string>("ItemName"),
                //    ItemId = dataRow.Field<long>("ItemID"),
                //    DSID = dataRow.Field<long?>("DSID"),
                //    ReceivedQty = dataRow.Field<Int32?>("ReceivedQty"),
                //    IssuedQty = dataRow.Field<Int32?>("IssuedQty"),
                //}).ToList();
                //gvItems.DataSource = LstItem;
                //gvItems.DataBind();
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

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                    Int64 IssuedQty = Convert.ToInt64(key["IssuedQty"]);
                    Int64 AvailableIssuedCount = Convert.ToInt64(key["AvailableIssuedCount"]);

                    CheckBox chk_Qty = (CheckBox)e.Row.FindControl("chk_Qty");
                    TextBox txt_Received_Qty = (TextBox)e.Row.FindControl("txt_Received_Qty");
                    Label lbl_ReceivedQtyInfrastructure = (Label)e.Row.FindControl("lbl_ReceivedQtyInfrastructure");
                    Label lbl_IssueQty = (Label)e.Row.FindControl("lbl_IssueQty");
                    Label lbl_IssueQtyAsset = (Label)e.Row.FindControl("lbl_IssueQtyAsset");
                    
                    chk_Qty.Visible = false;
                    txt_Received_Qty.Visible = false;

                    if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                    {

                        lbl_IssueQtyAsset.Visible = true;
                        lbl_IssueQty.Visible = false;

                        if (AvailableIssuedCount > 0)
                        {
                            lbl_IssueQtyAsset.Text = "Yes";
                        }
                        else if (AvailableIssuedCount == 0)
                        {
                            lbl_IssueQtyAsset.Text = "No";
                        } 

                        chk_Qty.Visible = true;

                        //e.Row.Cells[1].Visible = false;
                        //e.Row.Cells[3].Visible = false;
                        e.Row.Cells[2].Visible = false;
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


                    //#region "Data Keys"

                    //DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                    //string ID = Convert.ToString(key.Values["ID"]);

                    //#endregion "Data Keys"

                    //#region "Controls"

                    //Label lbl_IssueQty = (Label)e.Row.FindControl("lbl_IssueQty");
                    //TextBox txt_Qty = (TextBox)e.Row.FindControl("txt_Received_Qty");

                    //#endregion "Controls"

                    //if (CanView == true)
                    //{
                    //    // txt_Qty.Enabled = true;
                    //    e.Row.Cells[3].Enabled = true;
                    //}
                    //else
                    //{
                    //    e.Row.Cells[3].Enabled = false;
                    //}
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                    {
                        //e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[2].Text = "Issued Status";
                        e.Row.Cells[1].Text = "Issued";
                        e.Row.Cells[3].Text = "Received Status";
                    }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            bool IsSave = false;
            for (int m = 0; m < gvItems.Rows.Count; m++)
            {
                try
                {
                    string ItemID = gvItems.DataKeys[m].Values[0].ToString();
                    string ExistReceivedQty = gvItems.DataKeys[m].Values[2].ToString();
                    string DSID = gvItems.DataKeys[m].Values[3].ToString();
                    Int64 AvailableIssuedCount = Convert.ToInt64(gvItems.DataKeys[m].Values[6].ToString());
                    Label lbl_ReceivedQty = (Label)gvItems.Rows[m].FindControl("lbl_ReceivedQty");
                    TextBox txt_Received_Qty = (TextBox)gvItems.Rows[m].FindControl("txt_Received_Qty");
                    

                    if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                    {
                        CheckBox chk_Qty = (CheckBox)gvItems.Rows[m].FindControl("chk_Qty");

                        if (chk_Qty.Enabled == true)
                        {
                            if (AvailableIssuedCount != 0)
                            {
                                if (chk_Qty.Checked == true)
                                {
                                    bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now,
                                Convert.ToInt64(Session["DivisionID"].ToString()),
                                Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), Convert.ToInt32(gvItems.DataKeys[m].Values[5].ToString()),
                                Convert.ToInt32(Session["stockType"].ToString()), 1,
                                Convert.ToInt64(Session["StructureTypeID"].ToString()),
                                Convert.ToInt64(Session["StructureID"].ToString()), Convert.ToInt64(Session["FFPcampSiteID"].ToString()), null, null,
                                Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);

                                    //bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now,
                                    //                                           Convert.ToInt64(ddlDivision.SelectedValue),
                                    //                                           Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), Convert.ToInt32(gvReceivedStockInfrastructure.DataKeys[m].Values[5].ToString()),
                                    //                                           Convert.ToInt32(ddlStockType.SelectedValue), 1,
                                    //                                           Convert.ToInt64(InfrastructureTypeID.Value),
                                    //                                           Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null,
                                    //                                           Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
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
                                Convert.ToInt64(Session["DivisionID"].ToString()), 
                                Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, 
                                Convert.ToInt32(Session["stockType"].ToString()), Convert.ToInt32(txt_Received_Qty.Text), 
                                Convert.ToInt64(Session["StructureTypeID"].ToString()), 
                                Convert.ToInt64(Session["StructureID"].ToString()), Convert.ToInt64(Session["FFPcampSiteID"].ToString()), null, null, 
                                Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            
                            //bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now,
                            //    Convert.ToInt64(ddlDivision.SelectedValue),
                            //    Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null,
                            //    Convert.ToInt32(ddlStockType.SelectedValue), Convert.ToInt32(txt_Received_Qty.Text),
                            //    Convert.ToInt64(InfrastructureTypeID.Value),
                            //    Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null,
                            //    Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            //  bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, Convert.ToInt64(ddlDivision.SelectedValue), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, Convert.ToInt32(ddlStockType.SelectedValue), Convert.ToInt32(receivedQty), Convert.ToInt64(InfrastructureTypeID.Value), Convert.ToInt64(ddlInfrastructureName.SelectedValue), null, null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                            IsSave = true;
                        }
                    }

                    //if (Convert.ToInt32(DSID) != 0)
                    //{
                    //    if (Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                    //    {
                    //        int qtyrec = -1 * Convert.ToInt32(ExistReceivedQty);
                    //        bllDivisionStoreBLL.DivisionStoreInsertion(Convert.ToInt64(DSID), DateTime.Now, Convert.ToInt64(Session["DivisionID"].ToString()), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, 11, Convert.ToInt32(qtyrec), Convert.ToInt64(Session["StructureTypeID"].ToString()), Convert.ToInt64(Session["StructureID"].ToString()), Convert.ToInt64(Session["FFPcampSiteID"].ToString()), null, Convert.ToInt64(DSID), Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                    //    }
                    //}
                    //if (txt_Received_Qty.Text != "" && Convert.ToInt32(ExistReceivedQty) != Convert.ToInt32(txt_Received_Qty.Text))
                    //{
                    ////if (txt_Received_Qty.Text != "")
                    ////{
                    ////    //string StrreceivedQty = "";
                    ////    //if (lbl_ReceivedQty.Text == "")
                    ////    //{
                    ////    //    StrreceivedQty = "0";
                    ////    //}
                    ////    //else
                    ////    //{
                    ////    //    StrreceivedQty = lbl_ReceivedQty.Text;
                    ////    //}
                    ////    //  int receivedQty = Convert.ToInt32(StrreceivedQty) + Convert.ToInt32(txt_Received_Qty.Text);

                    ////    bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, Convert.ToInt64(Session["DivisionID"].ToString()), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, Convert.ToInt32(Session["stockType"].ToString()), Convert.ToInt32(txt_Received_Qty.Text), Convert.ToInt64(Session["StructureTypeID"].ToString()), Convert.ToInt64(Session["StructureID"].ToString()), Convert.ToInt64(Session["FFPcampSiteID"].ToString()), null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                    ////    //  bllDivisionStoreBLL.DivisionStoreInsertion(0, DateTime.Now, Convert.ToInt64(Session["DivisionID"].ToString()), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(ItemID), null, Convert.ToInt32(Session["stockType"].ToString()), Convert.ToInt32(receivedQty), Convert.ToInt64(Session["StructureTypeID"].ToString()), Convert.ToInt64(Session["StructureID"].ToString()), Convert.ToInt64(Session["FFPcampSiteID"].ToString()), null, null, Convert.ToInt32(mdlUser.ID), Convert.ToInt32(mdlUser.ID), 0);
                    ////    IsSave = true;
                    ////}
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