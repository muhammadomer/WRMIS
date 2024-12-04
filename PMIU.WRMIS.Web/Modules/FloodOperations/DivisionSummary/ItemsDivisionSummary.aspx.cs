using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary
{
    public partial class ItemsDivisionSummary : BasePage
    {
        int DivSummaryID;
        public static long _DivisionSummaryID;
        protected void Page_Load(object sender, EventArgs e)
        {
            long DivisionSummaryID = 0, StructureTypeID, StructureID;
            string StructureType, StructureName;
            try
            {
                DivSummaryID = Utility.GetNumericValueFromQueryString("DivisionSummaryID", 0);
                StructureTypeID = Utility.GetNumericValueFromQueryString("StructureTypeID", 0);
                StructureID = Utility.GetNumericValueFromQueryString("StructureID", 0);
                StructureType=Utility.GetStringValueFromQueryString("StructureType", "");
                StructureName = Utility.GetStringValueFromQueryString("StructureName", "");
                if (!IsPostBack)
                {

                    long.TryParse(Convert.ToString(_DivisionSummaryID), out DivisionSummaryID);
                    long ItemsID = Utility.GetNumericValueFromQueryString("ItemsID", 0);

                    SetPageTitle();
                    LoadItemCategory();
                    if (DivSummaryID > 0)
                    {
                        hdnDivisionSummaryID.Value = Convert.ToString(DivSummaryID);
                        hdnItemsID.Value = Convert.ToString(ItemsID);
                        hdnStructureTypeID.Value = Convert.ToString(StructureTypeID);
                        hdnStructureID.Value = Convert.ToString(StructureID);
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionSummary/InfrastructureBasedDivisionSummary.aspx?DivisionSummaryID={0}", DivSummaryID);
                      //  hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionSummary/SearchDivisionSummary.aspx?DivisionSummaryID={0}", DivisionSummaryID);
                        DivisionSummaryDetail._DivisionSummaryID = DivSummaryID;

                      //  DivisionSummaryInfrastructure();
                        lblInfrastructureType.Text = StructureType;
                        lblInfrastructureName.Text = StructureName;

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
                    lblInfrastructureType.Text = DR["StructureType"].ToString();
                    lblInfrastructureName.Text = DR["StructureName"].ToString();
                    hdnStructureTypeID.Value = DR["StructureTypeID"].ToString();
                    hdnStructureID.Value = DR["StructureID"].ToString();
                }

                //object _DivisionSummaryDetail = new FloodOperationsBLL().GetDivisionSummaryDetailByID(Convert.ToInt64(hdnDivisionSummaryID.Value));
                //if (_DivisionSummaryDetail != null)
                //{
                //    long divisionID = Convert.ToInt64(Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "DivisionID"));
                //dynamic ObjDivisionSummary = new FloodOperationsBLL().GetDivisionSummaryInfrastructure(Convert.ToInt64(divisionID));
                //if (ObjDivisionSummary != null)
                //{
                //    lblInfrastructureName.Text = Convert.ToString(Utility.GetDynamicPropertyValue(ObjDivisionSummary, "StructureName"));
                //    lblInfrastructureType.Text = Convert.ToString(Utility.GetDynamicPropertyValue(ObjDivisionSummary, "StructureType"));
                //    hdnStructureTypeID.Value = Convert.ToString(Utility.GetDynamicPropertyValue(ObjDivisionSummary, "StructureTypeID"));
                //    hdnStructureID.Value = Convert.ToString(Utility.GetDynamicPropertyValue(ObjDivisionSummary, "StructureID"));
                //}
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
                DataSet DS = new FloodOperationsBLL().GetDivisionSummaryItemsQty(Convert.ToInt32(hdnDivisionSummaryID.Value), Convert.ToInt32(ddlItemCategory.SelectedValue), Convert.ToInt32(hdnStructureTypeID.Value), Convert.ToInt32(hdnStructureID.Value));

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
                    #region Keys
                    DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                    Int64 DivisionStoreQty = Convert.ToInt64(key["DivisionStoreQty"]);

                    #endregion

                    if (Convert.ToInt32(ddlItemCategory.SelectedValue) == 4)
                    {

                        if (DivisionStoreQty == 0)
                        {
                            e.Row.Cells[1].Text = "No";
                        }
                        else
                        {
                            e.Row.Cells[1].Text = "Yes";
                        }
                    }
                    
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Convert.ToInt32(ddlItemCategory.SelectedValue) == 4)
                    {
                        e.Row.Cells[1].Text = "Available at Infrastructures";
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