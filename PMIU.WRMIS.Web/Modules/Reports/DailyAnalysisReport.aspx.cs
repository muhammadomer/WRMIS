using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Reports
{
    public partial class DailyAnalysisReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    txtDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));
                    BindDropdownlists();

                }
                iframestyle.Visible = false;
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            //Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DailyAnalysisReport);
            //Master.ModuleTitle = pageTitle.Item1;
            //Master.PageTitle = pageTitle.Item2;
            //Master.NavigationBar = pageTitle.Item3;
        }

        #region "Dropdownlists Events
        private void BindDropdownlists()
        {
            try
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlSession, CommonLists.GetSession(), (int)Constants.DropDownFirstOption.NoOption);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion
        protected void btnLoadDailyData_Click(object sender, EventArgs e)
        {
            try
            {
                Session[SessionValues.ReportData] = GetReportData(GetReportID());

                iframestyle.Visible = true;
                iframestyle.Src = Constants.ReportsUrl;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private ReportData GetReportData(Int32 _ReportID)
        {
            ReportData rptData = null;
            switch (_ReportID)
            {
                case (int)ReportConstants.DailyDataAnalysisReports.DailyAnalysis:
                    rptData = GetDailyAnalysisParams();
                    break;
                case (int)ReportConstants.DailyDataAnalysisReports.DailyChannelsStatus:
                    rptData = GetDailyChannelsStatusParams();
                    break;
                case (int)ReportConstants.DailyDataAnalysisReports.ComparisonOfShortDryTails:
                    rptData = GetComparisonOfShortDryTailsParams();
                    break;
                default:

                    break;
            }
            return rptData;
        }

        #region "Reports Paramert Methods"
        private ReportData GetReportPrimaryParameters()
        {
            string Date = string.IsNullOrEmpty(txtDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtDate.Text));
            string SessionID = ddlSession.SelectedItem.Value == string.Empty ? "0" : ddlSession.SelectedItem.Value;

            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("ReadingDate", Date);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("Session", SessionID);
            rptData.Parameters.Add(reportParameter);


            return rptData;
        }
        private ReportData GetDailyAnalysisParams()
        {
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptDailyAnalysis;

            return rptData;
        }
        private ReportData GetDailyChannelsStatusParams()
        {

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptDailyChannelStatus;

            int SearchTypeID = 0;

            Int32.TryParse(ddlIrrigationLevel.SelectedItem.Value, out SearchTypeID);

            ReportParameter reportParameter = new ReportParameter("SearchType", Convert.ToString(SearchTypeID));
            rptData.Parameters.Add(reportParameter);

            return rptData;

        }
        private ReportData GetComparisonOfShortDryTailsParams()
        {
            string ComparisonYearID = ddlYearsForComparison.SelectedItem.Value == string.Empty ? "0" : ddlYearsForComparison.SelectedItem.Value;
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptComparisonShortDryTails;

            ReportParameter reportParameter = new ReportParameter("ComparisonYearID", ComparisonYearID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        #endregion

        #region "Load Reports Secondary Parameters"
        private void LoadComparisonOfShortDryTailsSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlYearsForComparison, CommonLists.GetComparsionYears());
            SPComparisonOfShortDryTails.Visible = true;
        }
        private void LoadDailyChannelsStatusSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlIrrigationLevel, GetIrrigationLevel(), (int)Constants.DropDownFirstOption.NoOption);
            SPDailyChannelsStatus.Visible = true;
        }
        #endregion
        private void HideSecondaryParameters()
        {
            SPDailyChannelsStatus.Visible = false;
            SPComparisonOfShortDryTails.Visible = false;
        }
        private int GetReportID()
        {
            int reportID = 0;
            if (rbDailyAnalysis.Checked)
                reportID = (int)ReportConstants.DailyDataAnalysisReports.DailyAnalysis;
            else if (rbDailyStatusOfChannels.Checked)
                reportID = (int)ReportConstants.DailyDataAnalysisReports.DailyChannelsStatus;
            else if (rbComparisonOfShortDryTails.Checked)
                reportID = (int)ReportConstants.DailyDataAnalysisReports.ComparisonOfShortDryTails;
            return reportID;

        }
        protected void rbReports_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HideSecondaryParameters();
                iframestyle.Visible = false;
                int reportID = GetReportID();
                switch (reportID)
                {
                    case (int)ReportConstants.DailyDataAnalysisReports.DailyChannelsStatus:
                        LoadDailyChannelsStatusSecondaryParam();
                        break;
                    case (int)ReportConstants.DailyDataAnalysisReports.ComparisonOfShortDryTails:
                        LoadComparisonOfShortDryTailsSecondaryParam();
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private List<object> GetIrrigationLevel()
        {
            List<object> lstIrrigationLevel = new List<object>();
            lstIrrigationLevel.Add(new { ID = 1, Name = "Zone" });
            lstIrrigationLevel.Add(new { ID = 2, Name = "Circle" });
            lstIrrigationLevel.Add(new { ID = 3, Name = "Division" });
            return lstIrrigationLevel;
        }
    }
}