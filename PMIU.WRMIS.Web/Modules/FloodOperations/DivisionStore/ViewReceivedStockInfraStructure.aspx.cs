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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class ViewReceivedStockInfraStructure : BasePage
    {
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
                    hlBack.NavigateUrl =
                        string.Format("~/Modules/FloodOperations/DivisionStore/SearchReceivedStock.aspx?ID={0}", 0);

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
                    //if (SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.XEN) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.SE) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.Secretary) || SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ChiefIrrigation))
                    //{
                    //    gvItems.Visible = true;
                    //    BindItemsGridView();
                    //}
                    //else
                    //{
                    //    gvItems.Visible = false;
                    //}
                    if (CanView == true)
                    {
                        gvItems.Visible = true;
                        BindItemsGridView();
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
                DataSet DS = new DivisionStoreBLL().DSViewReceivedStockInfrastructure(Convert.ToInt64(Session["DivisionIDD"].ToString()), Convert.ToInt64(Session["StructureTypeID"].ToString()), Convert.ToInt64(Session["StructureID"].ToString()), Convert.ToInt64(ddlItemCategory.SelectedValue));
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvItems.DataSource = DS.Tables[0];
                }
                gvItems.DataBind();
                //var LstItem = IeDivisionStoreItem.Select(dataRow => new
                //{
                //    ItemName = dataRow.Field<string>("ItemName"),
                //    ItemId = dataRow.Field<long>("ItemId"),
                //    IssuedQty = dataRow.Field<Int32>("IssuedQty"),
                //    ReceivedQty = dataRow.Field<Int32>("ReceivedQty"),
                //}).ToList();
                //gvItems.DataSource = LstItem;
                //gvItems.DataBind();
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
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                    Int64 AvailableIssuedCount = Convert.ToInt64(key["AvailableIssuedCount"]);
                    Label lbl_StoreQty = (Label)e.Row.FindControl("lbl_StoreQty");
                    Label lbl_IssuedQty = (Label)e.Row.FindControl("lbl_IssuedQty");

                    if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                    {
                        if(AvailableIssuedCount> 0)
                        {
                            lbl_StoreQty.Text = "Yes";
                            lbl_IssuedQty.Text ="No";
                        }
                        else
                        {
                            lbl_StoreQty.Text = "No";
                            lbl_IssuedQty.Text = "No";
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                    {
                        e.Row.Cells[1].Text = "Issued at Infrastructure";
                        e.Row.Cells[2].Text = "Received from Infrastructure";
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
    }
}