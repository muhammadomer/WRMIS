using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
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
    public partial class IndentHistory : BasePage
    {
        long ChannelID;
        long SubDivID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelID"]) && !string.IsNullOrEmpty(Request.QueryString["SubDivID"]))
                    {
                        ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
                        SubDivID = Convert.ToInt64(Request.QueryString["SubDivID"]);
                        BindChannelData(ChannelID, SubDivID);
                        txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddDays(-1)));
                        txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    }
                }
                catch (Exception Exp)
                {
                    new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
                }
            }
        }

        /// <summary>
        /// this function sets the title of the page
        /// Created On: 8/12/2015 
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.IndentHistory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function binds all the data to labels
        /// Created On: 8/12/2015
        /// </summary>
        /// <param name="_ChannelId"></param>
        /// <param name="_SubDivId"></param>
        private void BindChannelData(long _ChannelID, long _SubDivID)
        {
            ChannelBLL bllChannel = new ChannelBLL();
            CO_Channel mdlChannel = bllChannel.GetChannelByID(_ChannelID);

            SubDivisionBLL bllSubDivision = new SubDivisionBLL();
            CO_SubDivision mdlSubDivison = bllSubDivision.GetSubDivisionsBySubDivisionID(_SubDivID);

            lblChannelName.Text = mdlChannel.NAME;
            lblChannelType.Text = mdlChannel.CO_ChannelType.Name;
            lblTotalRD.Text = Calculations.GetRDText(mdlChannel.TotalRDs);
            lblFlowType.Text = mdlChannel.CO_ChannelFlowType.Name;
            lblCommandName.Text = mdlChannel.CO_ChannelComndType.Name;
            lblIMISCode.Text = mdlChannel.IMISCode;
            lblDivision.Text = mdlSubDivison.CO_Division.Name;
            lblSubDivision.Text = mdlSubDivison.Name;
        }

        /// <summary>
        /// this function bind indent history with grid based on Channel ID, SubDiv ID, From Date, To Date
        /// Created On: 15/12/2015
        /// </summary>
        private void IndentsHistory()
        {
            ChannelID = Convert.ToInt64(Request.QueryString["ChannelID"]);
            SubDivID = Convert.ToInt64(Request.QueryString["SubDivID"]);
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (txtFromDate.Text != String.Empty)
            {
                FromDate = DateTime.Parse(txtFromDate.Text);
            }

            if (txtToDate.Text != String.Empty)
            {
                ToDate = DateTime.Parse(txtToDate.Text);
            }
            if (FromDate != null && ToDate != null && FromDate > ToDate)
            {
                Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                gvIndentHistory.Visible = false;
                return;
            }



            IndentsBLL bllIndents = new IndentsBLL();
            List<dynamic> lstChannelIndents = bllIndents.GetIndentHistoryByChannelIDAndSubDivID(ChannelID, SubDivID, FromDate, ToDate);
            gvIndentHistory.DataSource = lstChannelIndents;
            gvIndentHistory.DataBind();
            gvIndentHistory.Visible = true;
        }

        //protected void lnkBtnBack_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Response.Redirect("~/Modules/DailyData/SearchForPlacingIndents.aspx");
        //    }
        //    catch (Exception Exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                IndentsHistory();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndentHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIndentHistory.PageIndex = e.NewPageIndex;
                IndentsHistory();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndentHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Label lblFromDate = (Label)e.Row.FindControl("lblFromDate");
                if (lblFromDate != null)
                {
                    if (lblFromDate.Text != "")
                        lblFromDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblFromDate.Text));
                }


                Label lblToDate = (Label)e.Row.FindControl("lblToDate");
                if (lblToDate != null)
                {
                    if (lblToDate.Text != "")
                        lblToDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblToDate.Text));
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lnkBtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Modules/DailyData/SearchForPlacingIndents.aspx?ShowHistory=true");
            }
            catch (Exception Exp)
            {
                new WRException((long)Session[SessionValues.UserID], Exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}