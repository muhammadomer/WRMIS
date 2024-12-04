using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class IndentsHistory : BasePage
    {
        long OfftakeID;
        DateTime Date;
        bool Page;
        public const string QSOffTakeID = "OffTakeID";
        public const string QSDate = "Date";
        public const string QSPage = "Page";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    OfftakeID = Convert.ToInt64(Request.QueryString[QSOffTakeID]);
                    Date = Convert.ToDateTime(Request.QueryString[QSDate]);
                    //hlBack.NavigateUrl = "~/Modules/DailyData/DailyIndents.aspx";
                    BindGrid(OfftakeID, Date);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid(long _OfftakeID, DateTime _Date)
        {
            IndentsBLL bllIndents = new IndentsBLL();
            List<CO_ChannelIndentOfftakes> lstChannelIndentOfftakes = bllIndents.GetOfftakesByOfftakeID(_OfftakeID, _Date);
            gvIndentHistory.DataSource = lstChannelIndentOfftakes;
            gvIndentHistory.DataBind();
        }

        protected void gvIndentHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Label lblOfftakeIndentDate = (Label)e.Row.FindControl("lblOfftakeIndentDate");
                Label lblChannelIndent = (Label)e.Row.FindControl("lblChannelIndent");
                if (lblOfftakeIndentDate.Text != "")
                {
                    lblOfftakeIndentDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblOfftakeIndentDate.Text));
                }
                if (lblChannelIndent.Text != string.Empty)
                {
                    double IndentVal = Convert.ToDouble(lblChannelIndent.Text);
                    lblChannelIndent.Text = String.Format("{0:0.00}", (IndentVal));
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 16/08/2016 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.IndentsHistory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Page = Convert.ToBoolean(Request.QueryString[QSPage]);
                if (Page)
                {
                    Response.Redirect("~/Modules/DailyData/PlacingIndents.aspx?ShowHistory=true", false);
                }
                else
                {
                    Response.Redirect("~/Modules/DailyData/ViewIndents.aspx?ShowHistory=true", false);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}