using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations.ReferenceData;
using System;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using System.Collections;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Exceptions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary
{
    public partial class DivisionItems : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    int DivisionSummaryID = Utility.GetNumericValueFromQueryString("DivisionSummaryID", 0);
                    long ItemsID = Utility.GetNumericValueFromQueryString("ItemsID", 0);
                    if (DivisionSummaryID > 0)
                    {
                        hdnDivisionSummaryID.Value = Convert.ToString(DivisionSummaryID);
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionSummary/SearchDivisionSummary.aspx?DivisionSummaryID={0}", DivisionSummaryID);
                        hdnItemsID.Value = Convert.ToString(ItemsID);
                        DivisionSummaryDetail._DivisionSummaryID = DivisionSummaryID;
                        LoadItemCategory();
                        DivisionSummaryInfrastructure();
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

        private void DivisionSummaryInfrastructure()
        {
            try
            {
                long _DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);
                DataSet DS = new FloodOperationsBLL().DivisionSummayInfrastructure(_DivisionID);
                if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    DataRow DR = DS.Tables[0].Rows[0];
                    hdnStructureTypeID.Value = DR["StructureTypeID"].ToString();
                    hdnStructureID.Value = DR["StructureID"].ToString();
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
                DataSet DS = new FloodOperationsBLL().GetDivisionStoreItemsQty(Convert.ToInt32(hdnDivisionSummaryID.Value), Convert.ToInt32(ddlItemCategory.SelectedValue));

                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvItems.DataSource = DS.Tables[0];
                }

                gvItems.DataBind();
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

                    #region "Controls"

                    Label lbl_Infra_Qty = (Label)e.Row.FindControl("lbl_Infra_Qty");
                    Label lbl_Div_Store_Qty = (Label)e.Row.FindControl("lbl_Div_Store_Qty");
                    Label lbl_total_Qty = (Label)e.Row.FindControl("lbl_total_Qty");

                    lbl_total_Qty.Text = Convert.ToString(Convert.ToDouble(lbl_Infra_Qty.Text) + Convert.ToDouble(lbl_Div_Store_Qty.Text));

                    #endregion

                    #region Keys
                    DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                    Int64 DivisionStoreQty = Convert.ToInt64(key["DivisionStoreQty"]);

                    #endregion

                    if (Convert.ToInt32(ddlItemCategory.SelectedValue) == 4)
                    {
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[3].Visible = false;

                        if (DivisionStoreQty == 0)
                        {
                            e.Row.Cells[2].Text = "No";
                        }
                        else
                        {
                            e.Row.Cells[2].Text = "Yes";
                        }
                    }

                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Convert.ToInt32(ddlItemCategory.SelectedValue) == 4)
                    {
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[2].Text = "Available in Division Store";
                    }
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}