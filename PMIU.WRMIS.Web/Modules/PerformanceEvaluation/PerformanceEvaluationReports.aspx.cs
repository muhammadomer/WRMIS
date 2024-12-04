using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.PerformanceEvaluation;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.PerformanceEvaluation
{
    public partial class PerformanceEvaluationReports : BasePage
    {
        #region GridIndex

        public const int ReportIDIndex = 0;

        #endregion

        #region Hash Table Keys

        public const string FromDateKey = "FromDate";
        public const string ToDateKey = "ToDate";
        public const string ReportNameKey = "ReportName";
        public const string PageIndexKey = "PageIndex";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    DateTime Now = DateTime.Now;
                    txtToDate.Text = Utility.GetFormattedDate(Now);
                    txtFromDate.Text = Utility.GetFormattedDate(Now.AddDays(-1));

                    if (!string.IsNullOrEmpty(Request.QueryString["ShowHistory"]))
                    {
                        bool ShowHistory = Convert.ToBoolean(Request.QueryString["ShowHistory"]);

                        if (ShowHistory)
                        {
                            if (Session[SessionValues.SearchPEReportCriteria] != null)
                            {
                                BindHistoryData();
                            }
                        }
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
        /// Created on 03-11-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PerformanceEvaluation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the history data from session.
        /// Created On 22-11-2016
        /// </summary>
        public void BindHistoryData()
        {
            Hashtable SearchCriteria = (Hashtable)Session[SessionValues.SearchPEReportCriteria];

            txtFromDate.Text = (string)SearchCriteria[FromDateKey];
            txtToDate.Text = (string)SearchCriteria[ToDateKey];

            txtReportName.Text = (string)SearchCriteria[ReportNameKey];

            gvReport.PageIndex = (int)SearchCriteria[PageIndexKey];

            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
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

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvReport.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    long ReportID = Convert.ToInt64(gvReport.DataKeys[e.Row.RowIndex].Values[ReportIDIndex].ToString());

                    Label lblGenerationDate = (Label)e.Row.FindControl("lblGenerationDate");
                    Label lblReportSpan = (Label)e.Row.FindControl("lblReportSpan");
                    Label lblEvaluationType = (Label)e.Row.FindControl("lblEvaluationType");

                    if (lblGenerationDate != null)
                    {
                        DateTime GenerationDate = DateTime.Parse(lblGenerationDate.Text);
                        lblGenerationDate.Text = Utility.GetFormattedDate(GenerationDate);
                    }

                    if (lblReportSpan != null)
                    {
                        string ReportSpan = lblReportSpan.Text;

                        if (ReportSpan == Constants.ReportSpan.W.ToString())
                        {
                            lblReportSpan.Text = "Weekly";
                        }
                        else if (ReportSpan == Constants.ReportSpan.F.ToString())
                        {
                            lblReportSpan.Text = "Fortnightly";
                        }
                        else if (ReportSpan == Constants.ReportSpan.M.ToString())
                        {
                            lblReportSpan.Text = "Monthly";
                        }
                        else if (ReportSpan == Constants.ReportSpan.S.ToString())
                        {
                            lblReportSpan.Text = "Seasonal";
                        }
                    }

                    if (lblEvaluationType != null)
                    {
                        string EvaluationType = lblEvaluationType.Text;

                        if (EvaluationType == "G")
                        {
                            lblEvaluationType.Text = "General";
                        }
                        else if (EvaluationType == "S")
                        {
                            lblEvaluationType.Text = "Specific";
                        }
                    }

                    HyperLink hlShow = (HyperLink)e.Row.FindControl("hlShow");

                    if (hlShow != null)
                    {
                        hlShow.NavigateUrl = string.Format("~/Modules/PerformanceEvaluation/PerformanceEvaluationScore.aspx?ReportID={0}&ReportMode={1}", ReportID, true);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ReportID = Convert.ToInt64(gvReport.DataKeys[e.RowIndex].Values[ReportIDIndex].ToString());

                bool IsDeleted = new PerformanceEvaluationBLL().DeleteEvaluationReport(ReportID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);

                    bool IsDelete = true;

                    BindGrid(IsDelete);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds the reports grid
        /// Created On 28-11-2016
        /// </summary>        
        private void BindGrid(bool _IsDelete = false)
        {
            Hashtable SearchCriteria = new Hashtable();

            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (txtFromDate.Text.Trim() != String.Empty)
            {
                FromDate = Utility.GetParsedDate(txtFromDate.Text.Trim());
            }

            SearchCriteria.Add(FromDateKey, txtFromDate.Text.Trim());

            if (txtToDate.Text.Trim() != String.Empty)
            {
                ToDate = Utility.GetParsedDate(txtToDate.Text.Trim());
            }

            SearchCriteria.Add(ToDateKey, txtToDate.Text.Trim());

            if (FromDate != null && ToDate != null && FromDate > ToDate)
            {
                Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                return;
            }

            string ReportName = txtReportName.Text.Trim();

            SearchCriteria.Add(ReportNameKey, ReportName);

            List<PE_EvaluationReports> lstEvaluationReports = new PerformanceEvaluationBLL().GetEvaluationReports(FromDate, ToDate, ReportName);

            if (lstEvaluationReports.Count > 0)
            {
                gvReport.DataSource = lstEvaluationReports;
                gvReport.DataBind();

                gvReport.Visible = true;
                dvGrid.Attributes.Add("style", "display:block;");

                SearchCriteria.Add(PageIndexKey, gvReport.PageIndex);

                Session[SessionValues.SearchPEReportCriteria] = SearchCriteria;
            }
            else
            {
                if (!_IsDelete)
                {
                    Master.ShowMessage(Message.NoReportExists.Description, SiteMaster.MessageType.Error);
                }

                gvReport.Visible = false;
            }
        }

        protected void lbtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrCurrent = ((GridViewRow)((Control)sender).NamingContainer);

                long ReportID = Convert.ToInt64(gvReport.DataKeys[gvrCurrent.RowIndex].Values[ReportIDIndex]);

                ReportData mdlReportData = new ReportData();

                ReportParameter ReportParameter = new ReportParameter("ReportID", ReportID.ToString());
                mdlReportData.Parameters.Add(ReportParameter);

                mdlReportData.Name = Constants.PerformanceEvaluationReport;

                Session[SessionValues.ReportData] = mdlReportData;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "<script>window.open('" + Constants.ReportsUrl + "','_blank');</script>", false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}