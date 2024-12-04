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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.EmergencyPurchases
{
    public partial class EmergencyPurchasesItem : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int EmergencyPurchaseID = Utility.GetNumericValueFromQueryString("EmergencyPurchaseID", 0);
                    if (EmergencyPurchaseID > 0)
                    {
                        hdnEmergencyPurchaseID.Value = Convert.ToString(EmergencyPurchaseID);

                        hlBack.NavigateUrl =
                            string.Format("~/Modules/FloodOperations/EmergencyPurchases/SearchEmergencyPurchases.aspx?EmergencyPurchaseID={0}", EmergencyPurchaseID);
                        Header_Show(EmergencyPurchaseID);
                        LoadItemCategory();
                        //  BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedValue));
                        //    if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                        //    {
                        //        btnSave.Visible = true;
                        //    }
                        //    else
                        //    {
                        //        btnSave.Visible = false;
                        //    }
                        //}
                        //btnSave.Enabled = false;
                    }
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

        private void Header_Show(int EmergencyPurchaseID)
        {
            try
            {
                object lstItemP = new FloodOperationsBLL().GetItemPurchasing_EmergcnyP_ID(EmergencyPurchaseID);
                int Struct_type_id = 0;
                int StructureID = 0;
                string StructypeName = string.Empty;

                if (lstItemP != null)
                {
                    Struct_type_id = Convert.ToInt32(lstItemP.GetType().GetProperty("Struct_type_id").GetValue(lstItemP));
                    StructureID = Convert.ToInt32(lstItemP.GetType().GetProperty("StructureID").GetValue(lstItemP));
                    StructypeName = Convert.ToString(lstItemP.GetType().GetProperty("StructypeName").GetValue(lstItemP));
                    lbl_RD.Text = Convert.ToString(lstItemP.GetType().GetProperty("RD").GetValue(lstItemP));

                    if (StructypeName.Equals("Control Structure1"))
                    {
                        lbl_RD.Visible = false;
                        lblRDName.Visible = false;
                    }
                    if (lbl_RD.Text == "0+000")
                    {
                        lbl_RD.Visible = false;
                        lblRDName.Visible = false;
                    }
                }

                if (StructypeName.Equals("Protection Infrastructure"))
                {
                    object lstF_InfrastructureName = new FloodOperationsBLL().GetItemPurchas_Type_InsfrastructureName(1, StructureID);
                    lbl_infrastructure.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("infraName").GetValue(lstF_InfrastructureName));
                    lbl_infra_type.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("structypeName").GetValue(lstF_InfrastructureName));
                }
                else if (StructypeName.Equals("Control Structure1"))
                {
                    object lstF_InfrastructureName = new FloodOperationsBLL().GetItemPurchas_Type_InsfrastructureName(2, StructureID);
                    lbl_infrastructure.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("infraName").GetValue(lstF_InfrastructureName));
                    lbl_infra_type.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("structypeName").GetValue(lstF_InfrastructureName));
                }
                else if (StructypeName.Equals("Drain"))
                {
                    object lstF_InfrastructureName = new FloodOperationsBLL().GetItemPurchas_Type_InsfrastructureName(3, StructureID);
                    lbl_infrastructure.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("infraName").GetValue(lstF_InfrastructureName));
                    lbl_infra_type.Text = Convert.ToString(lstF_InfrastructureName.GetType().GetProperty("structypeName").GetValue(lstF_InfrastructureName));
                }
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
                Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false);
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
                IEnumerable<DataRow> IeDivisionStoreItem = new FloodOperationsBLL().GetFO_EMItemsPurchasingQty(Convert.ToInt32(hdnEmergencyPurchaseID.Value), Convert.ToInt32(ddlItemCategory.SelectedValue));
                var LstItem = IeDivisionStoreItem.Select(dataRow => new
                {
                    ItemName = dataRow.Field<string>("ItemName"),
                    ItemID = dataRow.Field<long>("ItemID"),
                    CreatedBy = dataRow.Field<Int32>("CreatedBy"),
                    CreatedDate = dataRow.Field<DateTime>("CreatedDate"),
                    PurchasedQty = dataRow.Field<Int32>("PurchasedQty"),
                    Purchased_Flood_seasonQty = dataRow.Field<Int32>("Purchased_Flood_seasonQty"),
                }).ToList();
                gvItems.DataSource = LstItem;
                gvItems.DataBind();
                gvItems.Enabled = true;
                //if (gvItems.Rows.Count > 0)
                //{
                //    btnSave.Enabled = true;
                //}
                //else
                //{
                //    btnSave.Enabled = false;
                //}
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
                UA_SystemParameters systemParameters = null;
                bool CanAddEditEP = false;
                int EmergencyPurchaseYear = Utility.GetNumericValueFromQueryString("EmergencyPurchaseYear", 0);
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "StartDate");
                //string StartDate = systemParameters.ParameterValue + "-" + EmergencyPurchaseYear; // 01-Jan
                //systemParameters = new FloodFightingPlanBLL().SystemParameterValue("FloodSeason", "EndDate"); // 31-Mar
                //string EndDate = systemParameters.ParameterValue + "-" + EmergencyPurchaseYear;

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txt_Purchased_Qty = (TextBox)e.Row.FindControl("txt_Purchased_Qty");

                    txt_Purchased_Qty.Enabled = false;
                    btnSave.Enabled = false;

                    CanAddEditEP = new FloodOperationsBLL().CanAddEditEmergencyPurchase(EmergencyPurchaseYear, SessionManagerFacade.UserInformation.UA_Designations.ID);
                    if (CanAddEditEP)
                    {
                        txt_Purchased_Qty.Enabled = CanAddEditEP;
                        btnSave.Enabled = CanAddEditEP;
                    }
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN))
                    //{
                    //    if (DateTime.Now >= Convert.ToDateTime(StartDate) && DateTime.Now <= Convert.ToDateTime(EndDate))
                    //    {
                    //        txt_Purchased_Qty.Enabled = true;
                    //        btnSave.Enabled = true;
                    //    }
                    //}

                    gvItems.Enabled = true;
                    //TextBox txt_Purchased_Qty = (TextBox)e.Row.FindControl("txt_Purchased_Qty");
                    //if (txt_Purchased_Qty.Text == "0")
                    //{
                    //    txt_Purchased_Qty.Text = "";

                    //}

                    //if (CanView == true)
                    //{
                    //    gvItems.Enabled = true;

                    //}
                    //else
                    //{
                    //    gvItems.Enabled = false;

                    //}
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
            for (int m = 0; m < gvItems.Rows.Count; m++)
            {
                try
                {
                    Label lbl_flood = (Label)gvItems.Rows[m].FindControl("lbl_flood");
                    lbl_flood.Text = "0";
                    TextBox txt_Purchased_Qty = (TextBox)gvItems.Rows[m].FindControl("txt_Purchased_Qty");
                    if (txt_Purchased_Qty.Text != "")
                    {
                        string ItemID = gvItems.DataKeys[m].Values[0].ToString();
                        string CreatedBy = gvItems.DataKeys[m].Values[2].ToString();
                        string CreatedDate = gvItems.DataKeys[m].Values[3].ToString();
                        string previousPurchasedQuantity = gvItems.DataKeys[m].Values[4].ToString();

                        object lstIsExist_record = new FloodOperationsBLL().GetIsexistFo_EPItem_ID(Convert.ToInt32(hdnEmergencyPurchaseID.Value), Convert.ToInt32(ItemID));
                        int Fo_EPItem_ID = 0;

                        if (lstIsExist_record != null)
                        {
                            Fo_EPItem_ID = Convert.ToInt32(lstIsExist_record.GetType().GetProperty("FO_EPItem_ID").GetValue(lstIsExist_record));
                        }

                        FO_EPItem mdlFO_EPItem = new FO_EPItem();

                        mdlFO_EPItem.ID = Fo_EPItem_ID;
                        mdlFO_EPItem.EmergencyPurchaseID = Convert.ToInt64(hdnEmergencyPurchaseID.Value);
                        mdlFO_EPItem.ItemID = Convert.ToInt32(ItemID);
                        mdlFO_EPItem.CurrentQty = Convert.ToInt32(lbl_flood.Text);

                        int previousPurchasedQty = new FloodOperationsBLL().Get_EP_Item_PreviousQuantity(mdlFO_EPItem);

                        mdlFO_EPItem.PurchasedQty = Convert.ToInt32(txt_Purchased_Qty.Text) + previousPurchasedQty;// Convert.ToInt32(previousPurchasedQuantity);

                        if (mdlFO_EPItem.ID == 0)
                        {
                            mdlFO_EPItem.CreatedBy = Convert.ToInt32(mdlUser.ID);
                            mdlFO_EPItem.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            mdlFO_EPItem.CreatedBy = Convert.ToInt32(CreatedBy);
                            mdlFO_EPItem.CreatedDate = Convert.ToDateTime(CreatedDate);
                            mdlFO_EPItem.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                            mdlFO_EPItem.ModifiedDate = DateTime.Now;
                        }

                        IsSave = new FloodOperationsBLL().SaveFo_EP_Item(mdlFO_EPItem);
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
                else
                {
                    BindItemsGridView();
                    Master.ShowMessage("Quantity Purchased Required", SiteMaster.MessageType.Error);
                    return;
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