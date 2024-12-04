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
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionSummary
{
    public partial class InfrastructureBasedDivisionSummary : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long DivisionSummaryID = Utility.GetNumericValueFromQueryString("DivisionSummaryID", 0);
                    if (DivisionSummaryID > 0)
                    {
                        hdnDivisionSummaryID.Value = Convert.ToString(DivisionSummaryID);
                        ItemsDivisionSummary._DivisionSummaryID = DivisionSummaryID;
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionSummary/SearchDivisionSummary.aspx?DivisionSummaryID={0}", DivisionSummaryID);
                        DivisionSummaryDetail._DivisionSummaryID = DivisionSummaryID;
                        BindResultsGrid(DivisionSummaryID);
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

        private void BindResultsGrid(long _DivisionSummaryID)
        {
            try
            {
                object _DivisionSummaryDetail = new FloodOperationsBLL().GetDivisionSummaryDetailByID(_DivisionSummaryID);
                if (_DivisionSummaryDetail != null)
                {
                    long divisionID = Convert.ToInt64(Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "DivisionID"));

                    IEnumerable<DataRow> IeDivisionSummary = new FloodOperationsBLL().GetDivisionSummayInfrastructure(Convert.ToInt64(divisionID));
                    var lstDivisionSummary = IeDivisionSummary.Select(dataRow => new
                    {
                        StructureName = dataRow.Field<string>("StructureName"),
                        StructureType = dataRow.Field<string>("StructureType"),
                        StructureTypeID = dataRow.Field<long>("StructureTypeID"),
                        StructureID = dataRow.Field<long>("StructureID")

                    }).ToList();
                    gvInfrastructureBasedItems.DataSource = lstDivisionSummary;
                    gvInfrastructureBasedItems.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }


        }

        protected void gvInfrastructureBasedItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "InfrastructureItem")
                {
                    object _DivisionSummaryDetail = new FloodOperationsBLL().GetDivisionSummaryDetailByID(Convert.ToInt64(hdnDivisionSummaryID.Value));
                    if (_DivisionSummaryDetail != null)
                    {
                        long _DivisionID = Convert.ToInt64(Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "DivisionID"));
                        int _Year = Convert.ToInt32(Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "Year"));

                        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        DataKey key = gvInfrastructureBasedItems.DataKeys[row.RowIndex];
                        Response.Redirect("ItemsDivisionSummary.aspx?DivisionID=" + _DivisionID + "&year=" + _Year + "&StructureTypeID=" + Convert.ToString(key["StructureTypeID"]) + "&StructureID=" + Convert.ToString(key["StructureID"]) + "&DivisionSummaryID=" + Convert.ToString(hdnDivisionSummaryID.Value) + "&StructureType=" + Convert.ToString(key["StructureType"]) + "&StructureName=" + Convert.ToString(key["StructureName"]));
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