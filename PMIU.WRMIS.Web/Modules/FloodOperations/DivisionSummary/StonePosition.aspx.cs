using PMIU.WRMIS.AppBlocks;
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
    public partial class StonePositionDivisionSummary : BasePage
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
                    
                    var divisionID = Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "DivisionID");
                    string yearid = Utility.GetDynamicPropertyValue(_DivisionSummaryDetail, "Year");
                    IEnumerable<DataRow> IeDivisionSummary =
                        new FloodOperationsBLL().GetDivisionSummayStonePosition(Convert.ToInt64(divisionID), Convert.ToInt32(yearid));
                    var lstDivisionSummary = IeDivisionSummary.Select(dataRow => new
                    {
                        ID = dataRow.Field<long>("ID"),
                        StructureName = dataRow.Field<string>("StructureName"),
                        StructureType = dataRow.Field<string>("StructureType"),
                      //  RD = dataRow.Field<Int32>("RD"),
                        RD = Calculations.GetRDText(Convert.ToInt64(dataRow.Field<Int32>("RD"))),
                        AvailableQty = dataRow.Field<Int32>("AvailableQty")
                    }).ToList();
                    gvstonePosition.DataSource = lstDivisionSummary;
                    gvstonePosition.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }


        }

        protected void gvstonePosition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataKey key = gvstonePosition.DataKeys[e.Row.RowIndex];
                string struct_type = Convert.ToString(key.Values["StructureType"]);
                Label lblRD = (Label)e.Row.FindControl("lblRD");

                if (struct_type.Contains("Barrage") || struct_type.Contains("Head work"))
                {
                    lblRD.Visible = false;
                }
            }
        }

 
    }
}