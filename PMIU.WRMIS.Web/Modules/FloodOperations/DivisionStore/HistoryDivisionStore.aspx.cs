using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class HistoryDivisionStore : BasePage
    {
        int DivisionID, CatID, ItemID, Year;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DivisionID = Utility.GetNumericValueFromQueryString("DivisionID", 0);
                    CatID = Utility.GetNumericValueFromQueryString("CatID", 0);
                    ItemID = Utility.GetNumericValueFromQueryString("ItemID", 0);
                    Year = Utility.GetNumericValueFromQueryString("Year", 0);
                    SetPageTitle();
                    ShowHeader();
                    BindHistoryGridView();
                    hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/DivisionStore/SearchDivisionStore.aspx?DivisionStoreID={0}", 0);
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

        private void ShowHeader()
        {
            try
            {
                lblZonevalue.Text = Convert.ToString(Utility.GetStringValueFromQueryString("ZoneName", ""));
                lblCirclevalue.Text = Convert.ToString(Utility.GetStringValueFromQueryString("CircleName", ""));
                lblDivisionvalue.Text = Convert.ToString(Utility.GetStringValueFromQueryString("DivisionName", ""));
                lblYearvalue.Text = Convert.ToString(Year);
                lblQuantityAvailablevalue.Text = Convert.ToString(Utility.GetNumericValueFromQueryString("QuantityAvailable", 0));

                if(lblQuantityAvailablevalue.Text == "0")
                {
                    lblQuantityAvailablevalue.Text = "No";
                }
                else
                {
                    lblQuantityAvailablevalue.Text = "Yes";
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindHistoryGridView()
        {
            try
            {
                DataSet DS = new DivisionStoreBLL().GetDivisionStoreItemHistory(DivisionID, Year, ItemID, CatID);
                
                if (DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    gvItemsHistory.DataSource = DS.Tables[0];
                }

                gvItemsHistory.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvItemsHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvItemsHistory.PageIndex = e.NewPageIndex;
                BindHistoryGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvItemsHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gvItemsHistory.DataKeys[e.Row.RowIndex];
                    string ETStatus = Convert.ToString(key.Values["ETStatus"]);
                    string StructureName = Convert.ToString(key.Values["StructureName"]);
                    #endregion

                    #region Control

                    Label lblIssuedStatus = (Label)e.Row.FindControl("lblIssuedStatus");
                    Label lblReceivedStatus = (Label)e.Row.FindControl("lblReceivedStatus");

                    #endregion

                    if (ETStatus == "Issued")
                    {
                        lblIssuedStatus.Text = "Issued at " + StructureName;
                        lblReceivedStatus.Text = "";

                    }
                    else if (ETStatus == "Received")
                    {
                        lblIssuedStatus.Text = "";
                        lblReceivedStatus.Text = ETStatus;

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