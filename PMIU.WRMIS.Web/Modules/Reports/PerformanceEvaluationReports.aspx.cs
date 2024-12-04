using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.PerformanceEvaluation;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterTheft;
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
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Reports
{
    public partial class PerformanceEvaluationReports : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    txtFromDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now.AddMonths(-6)));
                    txtToDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                    LoadPeriods();
                    iframestyle.Visible = false;
                }
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private ReportData GetReportData()
        {
            ReportData mdlReportData = new ReportData();
            string ReportID = "";
            if (ddlTenderNoticeName.SelectedValue != "-1")
            {
                ReportID = ddlTenderNoticeName.SelectedValue;
            }
            ReportParameter ReportParameter = new ReportParameter("ReportID", ReportID);
            mdlReportData.Parameters.Add(ReportParameter);
            mdlReportData.Name = mdlReportData.Name = Constants.PerformanceEvaluationReport;
            return mdlReportData;
        }
        private void LoadReport()
        {
            try
            {
                Session[SessionValues.ReportData] = GetReportData();
                iframestyle.Visible = true;
                iframestyle.Src = Constants.ReportsUrl;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                LoadReport();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                iframestyle.Visible = false;
                LoadPeriods();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadPeriods()
        {
           
                DateTime? FromDate = Convert.ToDateTime(txtFromDate.Text);
                DateTime? ToDate = Convert.ToDateTime(txtToDate.Text);
                if (FromDate!=null && ToDate!=null && ToDate>FromDate)
                {
                    List<object> lstEvaluationReports = new PerformanceEvaluationBLL().GetEvaluationReportsByDateRange(FromDate, ToDate);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlTenderNoticeName, lstEvaluationReports, (int)Constants.DropDownFirstOption.Select);    
                }
                else
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlTenderNoticeName, null, (int)Constants.DropDownFirstOption.Select); 
                }
                
            
        }


      
    }
}