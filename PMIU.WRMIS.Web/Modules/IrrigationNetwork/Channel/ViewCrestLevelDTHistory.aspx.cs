using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
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

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.Channel
{
    public partial class ViewCrestLevelDTHistory : BasePage
    {
        #region DataKeys Indexes

        public int GaugeIDIndex = 0;
        public int ReadingDateIndex = 1;
        public int HistoryIDIndex = 2;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["ChannelGaugeID"]))
                    {
                        string Now = Utility.GetFormattedDate(DateTime.Now);
                        string Yesterday = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));

                        txtFromDate.Text = Yesterday;
                        txtToDate.Text = Now;

                        long ChannelGaugeID = Convert.ToInt64(Request.QueryString["ChannelGaugeId"]);
                        BindChannelData(ChannelGaugeID);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 18-11-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ViewCrestLevelDTHistory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds data to the upper section of section
        /// Created on 19-11-2015
        /// </summary>
        /// <param name="_ChannelGaugeID"></param>
        private void BindChannelData(long _ChannelGaugeID)
        {
            ChannelBLL bllChannel = new ChannelBLL();
            CO_ChannelGauge mdlChannelGauge = bllChannel.GetChannelGaugeByID(_ChannelGaugeID);

            lblChannelName.Text = mdlChannelGauge.CO_Channel.NAME;
            lblChannelType.Text = mdlChannelGauge.CO_Channel.CO_ChannelType.Name;
            lblTotalRDs.Text = Calculations.GetRDText(mdlChannelGauge.CO_Channel.TotalRDs);
            lblFlowType.Text = mdlChannelGauge.CO_Channel.CO_ChannelFlowType.Name;
            lblCommandName.Text = mdlChannelGauge.CO_Channel.CO_ChannelComndType.Name;
            lblCategoryOfGauge.Text = mdlChannelGauge.CO_GaugeCategory.Name;
            lblGaugeID.Text = Convert.ToString(_ChannelGaugeID);
            lblChannelID.Text = Convert.ToString(mdlChannelGauge.ChannelID);

            if (mdlChannelGauge.CO_GaugeType != null)
            {
                lblGaugeType.Text = mdlChannelGauge.CO_GaugeType.Name;
            }
        }

        /// <summary>
        /// This function binds data to the history grid
        /// Created On 19-11-2015
        /// </summary>
        private void BindGrid()
        {
            int GaugeID = Convert.ToInt32(lblGaugeID.Text);
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (txtFromDate.Text.Trim() != String.Empty)
            {
                FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
            }

            if (txtToDate.Text.Trim() != String.Empty)
            {
                ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
            }

            if (FromDate != null && ToDate != null && FromDate > ToDate)
            {
                Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                gvHistory.Visible = false;
                return;
            }

            List<CO_ChannelGaugeDTPFall> lstParameterHistory = new ChannelBLL().GetCrestLevelParameterHistory(FromDate, ToDate, GaugeID);

            gvHistory.DataSource = lstParameterHistory;
            gvHistory.DataBind();

            gvHistory.Visible = true;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                string Url = "~/Modules/IrrigationNetwork/Channel/GaugeInformation.aspx?ChannelID=" + lblChannelID.Text;
                Response.Redirect(Url);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHistory.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblDateOfObservation = (Label)e.Row.FindControl("lblDateOfObservation");
                    Label lblDateOfApproval = (Label)e.Row.FindControl("lblDateOfApproval");
                    DateTime ObservationDate = DateTime.Parse(lblDateOfObservation.Text);
                    lblDateOfObservation.Text = Utility.GetFormattedDate(ObservationDate);
                    if (lblDateOfApproval.Text == "01-Jan-1950")
                    {
                        lblDateOfApproval.Text = "Pending for approval";
                        lblDateOfApproval.ForeColor = System.Drawing.Color.Red;
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnDischargeTable_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrCurrent = ((GridViewRow)((Control)sender).NamingContainer);

                long GaugeID = Convert.ToInt64(gvHistory.DataKeys[gvrCurrent.RowIndex].Values[GaugeIDIndex]);
                DateTime ReadingDate = Convert.ToDateTime(gvHistory.DataKeys[gvrCurrent.RowIndex].Values[ReadingDateIndex]);
                long DischargeTableID = Convert.ToInt64(gvHistory.DataKeys[gvrCurrent.RowIndex].Values[HistoryIDIndex]);

                ReportData mdlReportData = new ReportData();

                ReportParameter ReportParameter = new ReportParameter("GaugeID", GaugeID.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                ReportParameter = new ReportParameter("ReadingDate", ReadingDate.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                ReportParameter = new ReportParameter("DischargeTableID", DischargeTableID.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                mdlReportData.Name = Constants.DischargeTableCrestReport;

                Session[SessionValues.ReportData] = mdlReportData;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsNestedUrl + "','_blank');</script>", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}