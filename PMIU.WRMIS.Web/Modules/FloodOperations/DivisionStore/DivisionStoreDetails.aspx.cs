using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.DivisionStore
{
    public partial class DivisionStoreDetails : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionStore_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivisionStore_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid(long _ItemCategoryID)
        {
            //IEnumerable<DataRow> IeDivisionStoreItem = new FloodFightingPlanBLL().GetFFPItemsQty(1, 1);
            //var LstItem = IeDivisionStoreItem.Select(dataRow => new
            //{
            //    ItemName = dataRow.Field<string>("ItemName"),
            //    ItemId = dataRow.Field<long>("ItemId"),
            //    MajorMinor = dataRow.Field<string>("MajorMinor"),
            //    Description = dataRow.Field<string>("Description"),

            //    CreatedBy = dataRow.Field<Int32>("CreatedBy"),
            //    CreatedDate = dataRow.Field<DateTime>("CreatedDate"),
            //    RequiredQty = dataRow.Field<Int32>("RequiredQty"),
            //    DivisionQty = dataRow.Field<Int32>("DivisionQty"),

            //}).ToList();
            //gvDivisionStore.DataSource = LstItem;
            //gvDivisionStore.DataBind();
            //gvDivisionStore.Enabled = true;
        }
    }
}