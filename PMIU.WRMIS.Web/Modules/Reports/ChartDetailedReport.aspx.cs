using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMIU.WRMIS.Web.Modules.Reports
{
    public partial class ChartDetailedReport : BasePage
    {
        public enum ChartDetailedReportName
        {
            TailStatusFieldStaff = 1,
            TailStatusPMIUStaff = 2,
            ComplaintStatus = 3,
            WaterTheft = 4,
            PerformanceEvaluation = 5
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

                    if (!string.IsNullOrEmpty(Request.QueryString["ReportType"]))
                    {
                        hdnReportType.Value = Request.QueryString["ReportType"];
                        SetPageTitle(Convert.ToInt32(Request.QueryString["ReportType"]));
                    }

                    if (userID > 0) // Irrigation Staff
                    {
                        LoadAllRegionDDByUser(userID, IrrigationLevelID);
                    }
                    else
                    {
                        BindDropdownlists();
                    }

                    LoadCriteriaByReportType();
                    PrepopulateCriteria();
                }
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadCriteriaByReportType()
        {
            string statusID = string.Empty;
            string reportedBy = string.Empty;
            switch (Convert.ToInt32(hdnReportType.Value))
            {
                case (int)ChartDetailedReportName.ComplaintStatus:
                    if (!string.IsNullOrEmpty(Request.QueryString["StatusID"]))
                        statusID = Convert.ToString(Request.QueryString["StatusID"]);
                    Dropdownlist.DDLComplaintStatus(ddlComplaintStatus, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.SetSelectedValue(ddlComplaintStatus, statusID);
                    SPComplaint.Visible = true;
                    break;
                case (int)ChartDetailedReportName.WaterTheft:
                    if (!string.IsNullOrEmpty(Request.QueryString["ReportedBy"]))
                        reportedBy = Convert.ToString(Request.QueryString["ReportedBy"]);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlReportedBy, CommonLists.GetWaterTheftReportedBy(), (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.SetSelectedValue(ddlReportedBy, reportedBy);
                    SPReportedBy.Visible = true;
                    break;
                case (int)ChartDetailedReportName.TailStatusFieldStaff:
                case (int)ChartDetailedReportName.TailStatusPMIUStaff:
                    if (!string.IsNullOrEmpty(Request.QueryString["StatusID"]))
                        statusID = Convert.ToString(Request.QueryString["StatusID"]);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlTailStatus, CommonLists.GetFieldStaffTailStatus(), (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.SetSelectedValue(ddlTailStatus, statusID);
                    SPTailStatus.Visible = true;
                    break;
                default:
                    break;
            }
        }
        private void SetPageTitle(int _ReportType)
        {
            string pageTitle = "Tail Status - Field Staff Analysis Report";
            switch (_ReportType)
            {
                case (int)ChartDetailedReportName.TailStatusFieldStaff:
                    pageTitle = "Tail Status - Field Staff Analysis Report";
                    break;
                case (int)ChartDetailedReportName.TailStatusPMIUStaff:
                    pageTitle = "Tail Status - PMIU Staff Analysis Report";
                    break;
                case (int)ChartDetailedReportName.ComplaintStatus:
                    pageTitle = "Complaints Analysis Report";
                    break;
                case (int)ChartDetailedReportName.WaterTheft:
                    pageTitle = "Water Theft Analysis Report";
                    break;
                case (int)ChartDetailedReportName.PerformanceEvaluation:
                    pageTitle = "Performance Evaluation Analysis Report";
                    break;
                default:
                    break;
            }
            pageTitleID.InnerText = pageTitle;
        }

        private void PrepopulateCriteria()
        {
            string qsZoneID = string.Empty;
            string qsCircleID = string.Empty;
            string qsDivisionID = string.Empty;
            string qsFromDate = string.Empty;
            string qsToDate = string.Empty;
            string complaintStatusID = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["ZoneID"]))
                qsZoneID = Convert.ToString(Request.QueryString["ZoneID"]);
            if (!string.IsNullOrEmpty(Request.QueryString["CircleID"]))
                qsCircleID = Convert.ToString(Request.QueryString["CircleID"]);

            if (!string.IsNullOrEmpty(Request.QueryString["DivisionID"]))
                qsDivisionID = Convert.ToString(Request.QueryString["DivisionID"]);
            if (!string.IsNullOrEmpty(Request.QueryString["FromDate"]))
                qsFromDate = Convert.ToString(Request.QueryString["FromDate"]);
            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                qsToDate = Convert.ToString(Request.QueryString["ToDate"]);


            txtFromDate.Text = qsFromDate;
            txtToDate.Text = qsToDate;

            Dropdownlist.SetSelectedValue(ddlZone, qsZoneID);
            Dropdownlist.SetSelectedValue(ddlCircle, qsCircleID);
            Dropdownlist.SetSelectedValue(ddlDivision, qsDivisionID);

            LoadReport();
        }
        private void LoadAllRegionDDByUser(long _UserID, long? _IrrigationLevelID)
        {
            if (_IrrigationLevelID == null)
                return;

            List<object> lstData = new UserAdministrationBLL().GetRegionsListByUser(_UserID, Convert.ToInt32(_IrrigationLevelID));

            int all = (int)Constants.DropDownFirstOption.All;
            int noOption = (int)Constants.DropDownFirstOption.NoOption;

            List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
            if (lstDivision.Count > 0) // Division
            {
                Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlDivision, lstDivision, lstDivision.Count == 1 ? noOption : all);
            }

            List<CO_Circle> lstCircle = (List<CO_Circle>)lstData.ElementAt(2);
            if (lstCircle.Count > 0) // Circle
            {
                Dropdownlist.BindDropdownlist<List<CO_Circle>>(ddlCircle, lstCircle, lstCircle.Count == 1 ? noOption : all);
            }

            List<CO_Zone> lstZone = (List<CO_Zone>)lstData.ElementAt(3);
            if (lstZone.Count > 0) // Zone
            {
                Dropdownlist.BindDropdownlist<List<CO_Zone>>(ddlZone, lstZone, lstZone.Count == 1 ? noOption : all);
            }
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
        #region "Dropdownlists Events
        private void BindDropdownlists()
        {
            try
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                DDLEmptyCircleDivisionSubDivision();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void DDLEmptyCircleDivisionSubDivision()
        {
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircles(ddlCircle, true, -1, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
        }
        private void DDLEmptyDivisionSubDivision()
        {
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Reset Circle,Division, Sub Division dropdownlists
                DDLEmptyCircleDivisionSubDivision();
                // Bind Circel dropdownlist based on Zone
                string zoneID = ddlZone.SelectedItem.Value;
                Dropdownlist.DDLCircles(ddlCircle, false, string.IsNullOrEmpty(zoneID) == true ? -1 : Convert.ToInt64(zoneID), (int)Constants.DropDownFirstOption.All);
                iframestyle.Visible = false;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Reset division dropdownlist
                Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);

                int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                // Bind Division dropdownlist based on Circle 
                Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);
                iframestyle.Visible = false;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        private ReportData GetComplaintDetailParams()
        {
            string ComplaintStatus = ddlComplaintStatus.SelectedItem.Value == string.Empty ? "0" : ddlComplaintStatus.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptComplaintDetail;

            ReportParameter reportParameter = new ReportParameter("ComplaintType", "0");
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ComplaintSource", "0");
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ComplaintStatus", ComplaintStatus);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetTailStatusParams()
        {
            string TailStatusID = ddlTailStatus.SelectedItem.Value == string.Empty ? "0" : ddlTailStatus.SelectedItem.Value;
            string reportName = string.Empty;

            ReportData rptData = GetReportPrimaryParameters();

            rptData.Name = GetReportName();

            ReportParameter reportParameter = new ReportParameter("Session", "0");
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("SubDivisionID", "0");
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("HeadStatus", "0");
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("TailStatus", TailStatusID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetTailStatusPMIUParams()
        {
            string TailStatusID = ddlTailStatus.SelectedItem.Value == string.Empty ? "0" : ddlTailStatus.SelectedItem.Value;
            string reportName = string.Empty;

            ReportData rptData = GetReportPrimaryParameters();

            rptData.Name = GetReportName();

            ReportParameter reportParameter = new ReportParameter("SubDivisionID", "0");
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("TailStatus", TailStatusID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetWaterTheftParams()
        {
            string ReportedBy = ddlReportedBy.SelectedItem.Value == string.Empty ? "0" : ddlReportedBy.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();

            ReportParameter reportParameter = new ReportParameter("ChannelID", "0");
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ReportedBy", ReportedBy);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("OffenceTypeID", "0");
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("StatusID", "0");
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("AssignedTo", "0");
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetWaterTheftChannelStatus()
        {
            ReportData rptData = GetWaterTheftParams();
            rptData.Name = ReportConstants.rptWaterTheftChannel;

            return rptData;
        }
        private ReportData GetWaterTheftOutletStatus()
        {
            ReportData rptData = GetWaterTheftParams();
            rptData.Name = ReportConstants.rptWaterTheftOutlet;

            ReportParameter reportParameter = new ReportParameter("OutLetID", "0");
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("OutLetConditionID", "0");
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetReportData()
        {
            ReportData rptData = null;
            int statusID = 1;
            int _ReportID = Convert.ToInt32(hdnReportType.Value);
            if (!string.IsNullOrEmpty(Request.QueryString["StatusID"]))
                statusID = Convert.ToInt32(Request.QueryString["StatusID"]);

            switch (_ReportID)
            {
                case (int)ChartDetailedReportName.ComplaintStatus:
                    rptData = GetComplaintDetailParams();
                    break;
                case (int)ChartDetailedReportName.WaterTheft:
                    if (statusID == 1 || statusID == 2)
                        rptData = GetWaterTheftOutletStatus();
                    else
                        rptData = GetWaterTheftChannelStatus();
                    break;
                case (int)ChartDetailedReportName.TailStatusFieldStaff:
                    rptData = GetTailStatusParams();
                    break;
                case (int)ChartDetailedReportName.TailStatusPMIUStaff:
                    rptData = GetTailStatusPMIUParams();
                    break;
                default:

                    break;
            }
            return rptData;
        }

        #region "Reports Paramert Methods"
        private ReportData GetReportPrimaryParameters()
        {
            string zoneID = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
            string circleID = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
            string divisionID = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;

            string FromDate = string.IsNullOrEmpty(txtFromDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtFromDate.Text));
            string ToDate = string.IsNullOrEmpty(txtToDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtToDate.Text));

            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("FromDate", FromDate);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ToDate", ToDate);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ZoneID", zoneID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("CircleID", circleID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("DivisionID", divisionID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        #endregion
        private void HideSecondaryParameters()
        {
            iframestyle.Visible = false;
        }
        private int GetReportID()
        {
            return 0;
        }
        private string GetReportName()
        {
            string reportName = string.Empty;
            switch (Convert.ToInt32(hdnReportType.Value))
            {
                case (int)ChartDetailedReportName.TailStatusFieldStaff:
                    reportName = ReportConstants.rptTailStatusFieldStaff;
                    break;
                case (int)ChartDetailedReportName.TailStatusPMIUStaff:
                    reportName = ReportConstants.rptTailStatusPMIUStaff;
                    break;
                case (int)ChartDetailedReportName.ComplaintStatus:
                    reportName = ReportConstants.rptComplaintDetail;
                    break;
                case (int)ChartDetailedReportName.WaterTheft:
                    reportName = ReportConstants.rptDBWaterTheftCases;
                    break;
                case (int)ChartDetailedReportName.PerformanceEvaluation:
                    reportName = "";
                    break;
                default:
                    break;
            }
            return reportName;
        }
        protected void btnViewChartDetailedReport_Click(object sender, EventArgs e)
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
    }
}