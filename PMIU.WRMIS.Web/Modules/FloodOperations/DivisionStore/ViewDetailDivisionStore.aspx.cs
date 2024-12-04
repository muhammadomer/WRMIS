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
    public partial class ViewDetailDivisionStore : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    ShowHeader();
                    if (CanView == true)
                    {
                        // BindItemsGridView();
                        BindMinorItemsGridView();
                    }
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

        #region Major Item GridView

        private void BindItemsGridView()
        {
            try
            {
                IEnumerable<DataRow> IeDivisionStore = new DivisionStoreBLL().GetDivisionStoreViewDetail(Convert.ToInt64(Session["DivisionID"].ToString()), Convert.ToInt32(Session["Year"].ToString()), Convert.ToInt16(Session["ItemID"].ToString()));
                var LstItem = IeDivisionStore.Select(dataRow => new
                {
                    ItemName = dataRow.Field<string>("ItemName"),
                    QuantityApproved = dataRow.Field<int>("QuantityApproved"),
                    IssuedQty = dataRow.Field<int>("IssuedQty"),
                    QuantityAvailable = dataRow.Field<int>("QuantityAvailable"),
                    Infraname = dataRow.Field<string>("Infraname"),
                    IssueDate = dataRow.Field<string>("IssueDate"),

                }).ToList();
                gvItems.DataSource = LstItem;
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

        #endregion

        #region Minor Item GridView

        private void BindMinorItemsGridView()
        {
            try
            {
                IEnumerable<DataRow> IeDivisionStoreDtail = new DivisionStoreBLL().GetDSMinorItemViewDetail(Convert.ToInt64(Session["DivisionID"].ToString()), Convert.ToInt32(Session["Year"].ToString()), Convert.ToInt16(Session["ItemID"].ToString()));
                var LstMinorItem = IeDivisionStoreDtail.Select(dataRow => new
                {
                    EntryDate = dataRow.Field<DateTime>("EntryDate"),
                    ItemName = dataRow.Field<string>("ItemName"),
                    QuantityEffected = dataRow.Field<int>("QuantityEffected"),
                    Condition = dataRow.Field<string>("Condition"),
                }).ToList();
                gvViewDetailMinorItem.DataSource = LstMinorItem;
                gvViewDetailMinorItem.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvViewDetailMinorItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvViewDetailMinorItem.PageIndex = e.NewPageIndex;
                BindMinorItemsGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void ShowHeader()
        {
            try
            {
                lblZonevalue.Text = Convert.ToString(Session["ZoneName"]);
                lblCirclevalue.Text = Convert.ToString(Session["CircleName"]);
                lblDivisionvalue.Text = Convert.ToString(Session["DivisionName"]); ;
                lblYearvalue.Text = Convert.ToString(Session["Year"]); ;
                lblQuantityAvailablevalue.Text = Convert.ToString(Session["QuantityAvailable"]);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

    }
}