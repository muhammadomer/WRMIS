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
    public partial class IssuedStockViewCampSiteItems : BasePage
    {
        int DivisionID, StructureID, StructureTypeID, RD, StockType, FFPcampSiteID, SelectedYear;
        string infrastructureType, InfraName;

        protected void Page_Load(object sender, EventArgs e)
        {

            DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);
            StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
            StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);
            StockType = Utility.GetNumericValueFromQueryString("StockType", 0);
            FFPcampSiteID = Utility.GetNumericValueFromQueryString("FFPCampSiteID", 0);
            SelectedYear = Utility.GetNumericValueFromQueryString("SelectedYear", 0);

            hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionStore/SearchIssuedStock.aspx");

            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();
                    LoadItemCategory();

                    infrastructureType = Utility.GetStringValueFromQueryString("infrastructureType", "");
                    InfraName = Utility.GetStringValueFromQueryString("InfraName", "");
                    RD = Utility.GetNumericValueFromQueryString("RD", 0);

                    lbl_infra_type.Text = infrastructureType;
                    lbl_infrastructure.Text = InfraName;
                    lbl_RD.Text = Convert.ToString(Calculations.GetRDText(RD));
                    if (infrastructureType.Contains("Barrage") || infrastructureType.Contains("Head work"))
                    {
                        lbl_RD.Visible = false;
                        lblRdName.Visible = false;
                    }
                    else
                    {
                        lbl_RD.Visible = true;
                        lblRdName.Visible = true;
                    }
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
                    if (SessionManagerFacade.UserInformation.UA_Designations.ID ==
                        Convert.ToInt64(Constants.Designation.XEN) ||
                        SessionManagerFacade.UserInformation.UA_Designations.ID ==
                        Convert.ToInt64(Constants.Designation.SE) ||
                        SessionManagerFacade.UserInformation.UA_Designations.ID ==
                        Convert.ToInt64(Constants.Designation.Secretary) ||
                        SessionManagerFacade.UserInformation.UA_Designations.ID ==
                        Convert.ToInt64(Constants.Designation.ChiefIrrigation))
                    {
                        gvItems.Visible = true;
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
                DataSet DS = new DivisionStoreBLL().DSIssuedStockViewCampSiteItems(DivisionID, StockType, SelectedYear,
                    Convert.ToInt32(ddlItemCategory.SelectedValue), FFPcampSiteID, StructureTypeID, StructureID);
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvItems.DataSource = DS.Tables[0];
                }
                //var LstItem = IeDivisionStoreItem.Select(dataRow => new
                //{
                //    ItemName = dataRow.Field<string>("ItemName"),
                //    ItemId = dataRow.Field<long>("ItemId"),
                //    QuantityAvailableInStore = dataRow.Field<Int32>("QuantityAvailableInStore"),
                //    QuantityApproved = dataRow.Field<Int32>("QuantityApproved"),
                //    IssuedQty = dataRow.Field<Int32>("IssuedQty"),

                //}).ToList();
                //gvItems.DataSource = LstItem;
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
                Int64 AvailableIssuedCount = Convert.ToInt64(key["AvailableIssuedCount"]);
                Label lbl_QuantityIssued = (Label)e.Row.FindControl("lbl_QuantityIssued");
                Int64 QuantityAvailableInStore = Convert.ToInt64(key["QuantityAvailableInStore"]);
                Label lbl_StoreQty = (Label)e.Row.FindControl("lbl_StoreQty");

                if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                {
                    gvItems.Columns[2].Visible = false;
                    gvItems.Columns[3].Visible = false;
                    gvItems.Columns[4].Visible = true;
                    lbl_StoreQty.Text = QuantityAvailableInStore == 0 ? "No" : "Yes";
                    lbl_QuantityIssued.Text = AvailableIssuedCount == 0 ? "No" : "Yes";
                }
                else
                {
                    gvItems.Columns[4].Visible = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Convert.ToInt64(ddlItemCategory.SelectedItem.Value) == 4)
                {
                    e.Row.Cells[1].Text = "Available in Store";
                }
            }
        }
    }
}