using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterTheft;
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
    public partial class DashBoardReports : BasePage
    {
        public enum DashboardReportName
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

                    PrepopulateCriteria();
                }
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle(int _ReportType)
        {
            string pageTitle = "Tail Status - Field Staff Analysis Report";
            switch (_ReportType)
            {
                case (int)DashboardReportName.TailStatusFieldStaff:
                    pageTitle = "Tail Status - Field Staff Analysis Report";
                    break;
                case (int)DashboardReportName.TailStatusPMIUStaff:
                    pageTitle = "Tail Status - PMIU Staff Analysis Report";
                    break;
                case (int)DashboardReportName.ComplaintStatus:
                    pageTitle = "Complaints Analysis Report";
                    break;
                case (int)DashboardReportName.WaterTheft:
                    pageTitle = "Water Theft Analysis Report";
                    break;
                case (int)DashboardReportName.PerformanceEvaluation:
                    pageTitle = "Performance Evaluation Analysis Report";
                    break;
                default:
                    break;
            }
            pageTitleID.InnerText = pageTitle;
        }

        private void PrepopulateCriteria()
        {
            ////////////////////////////////////////////////////////////
            string qsZoneID = string.Empty;
            string qsCircleID = string.Empty;
            string qsDivisionID = string.Empty;
            string qsFromDate = string.Empty;
            string qsToDate = string.Empty;
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
            /////////////////////////////////////////////////////////////

            ////////////////////////////////
            txtFromDate.Text = qsFromDate;
            txtToDate.Text = qsToDate;
            ////////////////////////////////

            //////////////////////////////////////
            if (string.IsNullOrEmpty(qsZoneID) && string.IsNullOrEmpty(qsCircleID) && string.IsNullOrEmpty(qsDivisionID))
            {
                rbZoneWise.Checked = true;
            }
            else if (!string.IsNullOrEmpty(qsZoneID) && string.IsNullOrEmpty(qsCircleID) && string.IsNullOrEmpty(qsDivisionID))
            {
                rbCircleWise.Checked = true;
                SPzone.Visible = true;
                Dropdownlist.SetSelectedValue(ddlZone, qsZoneID);
            }
            else if (!string.IsNullOrEmpty(qsZoneID) && !string.IsNullOrEmpty(qsCircleID) && string.IsNullOrEmpty(qsDivisionID))
            {
                rbDivisionWise.Checked = true;
                SPzone.Visible = true;
                SPcircle.Visible = true;
                Dropdownlist.SetSelectedValue(ddlZone, qsZoneID);
                Dropdownlist.SetSelectedValue(ddlCircle, qsCircleID);
            }
            else if (!string.IsNullOrEmpty(qsZoneID) && !string.IsNullOrEmpty(qsCircleID) && !string.IsNullOrEmpty(qsDivisionID))
            {
                rbSubDivisionWise.Checked = true;
                SPzone.Visible = true;
                SPcircle.Visible = true;
                SPdivision.Visible = true;
                Dropdownlist.SetSelectedValue(ddlZone, qsZoneID);
                Dropdownlist.SetSelectedValue(ddlCircle, qsCircleID);
                Dropdownlist.SetSelectedValue(ddlDivision, qsDivisionID);
            }
            ///////////////////////////////////////
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
                Session[SessionValues.ReportData] = GetReportData(GetReportID());
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
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Bind Sub Division dropdownlist based on Division 
                long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                iframestyle.Visible = false;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        private ReportData GetReportData(Int32 _ReportID)
        {
            ReportData rptData = null;
            switch (_ReportID)
            {
                case (int)ReportConstants.Dashboard.ZoneWise:
                    rptData = GetZoneWiseParameter();
                    break;
                case (int)ReportConstants.Dashboard.CircleWise:
                    rptData = GetCircleWiseParameter();
                    break;
                case (int)ReportConstants.Dashboard.DivisionWise:
                    rptData = GetDivisionWiseParameter();
                    break;
                case (int)ReportConstants.Dashboard.SubDivisionWise:
                    rptData = GetSubDivisionWiseParameter();
                    break;
                default:

                    break;
            }
            return rptData;
        }

        #region "Reports Paramert Methods"
        private ReportData GetReportPrimaryParameters()
        {
            string zoneID = "0";
            string circleID = "0";
            string divisionID = "0";
            string subDivisionID = "0";
            if (rbCircleWise.Checked || rbDivisionWise.Checked || rbSubDivisionWise.Checked)
                zoneID = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
            if (rbDivisionWise.Checked || rbSubDivisionWise.Checked)
                circleID = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
            if (rbSubDivisionWise.Checked)
                divisionID = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;

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

            reportParameter = new ReportParameter("SubDivisionID", subDivisionID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        #endregion
        public ReportData GetZoneWiseParameter()
        {
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = GetReportName();
            return rptData;
        }
        public ReportData GetCircleWiseParameter()
        {
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = GetReportName();

            return rptData;
        }
        public ReportData GetDivisionWiseParameter()
        {
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = GetReportName();

            return rptData;
        }
        public ReportData GetSubDivisionWiseParameter()
        {
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = GetReportName();

            return rptData;
        }
        private void HideSecondaryParameters()
        {
            SPzone.Visible = false;
            SPcircle.Visible = false;
            SPdivision.Visible = false;
            iframestyle.Visible = false;
        }
        private int GetReportID()
        {
            int reportID = 0;
            if (rbZoneWise.Checked)
                reportID = (int)ReportConstants.Dashboard.ZoneWise;
            else if (rbCircleWise.Checked)
                reportID = (int)ReportConstants.Dashboard.CircleWise;
            else if (rbDivisionWise.Checked)
                reportID = (int)ReportConstants.Dashboard.DivisionWise;
            else if (rbSubDivisionWise.Checked)
                reportID = (int)ReportConstants.Dashboard.SubDivisionWise;
            return reportID;
        }
        private string GetReportName()
        {
            string reportName = string.Empty;
            switch (Convert.ToInt32(hdnReportType.Value))
            {
                case (int)DashboardReportName.TailStatusFieldStaff:
                    reportName = ReportConstants.rptDBTailStatusField;
                    break;
                case (int)DashboardReportName.TailStatusPMIUStaff:
                    reportName = ReportConstants.rptDBTailStatusPMIU;
                    break;
                case (int)DashboardReportName.ComplaintStatus:
                    reportName = ReportConstants.rptDBComplaintAnalysis;
                    break;
                case (int)DashboardReportName.WaterTheft:
                    reportName = ReportConstants.rptDBWaterTheftCases;
                    break;
                case (int)DashboardReportName.PerformanceEvaluation:
                    reportName = "";
                    break;
                default:
                    break;
            }
            return reportName;
        }
        #region "Load Reports Secondary Parameters"
        private void LoadCircleDropdownlist()
        {
            SPzone.Visible = true;
        }
        private void LoadDivisionDropdownlist()
        {
            SPzone.Visible = true;
            SPcircle.Visible = true;
        }
        private void LoadSubDivisionDropdownlist()
        {
            SPzone.Visible = true;
            SPcircle.Visible = true;
            SPdivision.Visible = true;
        }
        #endregion
        protected void rbReports_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HideSecondaryParameters();
                iframestyle.Visible = false;
                int reportID = GetReportID();
                switch (reportID)
                {
                    case (int)ReportConstants.Dashboard.CircleWise:
                        LoadCircleDropdownlist();
                        break;
                    case (int)ReportConstants.Dashboard.DivisionWise:
                        LoadDivisionDropdownlist();
                        break;
                    case (int)ReportConstants.Dashboard.SubDivisionWise:
                        LoadSubDivisionDropdownlist();
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
    }
}