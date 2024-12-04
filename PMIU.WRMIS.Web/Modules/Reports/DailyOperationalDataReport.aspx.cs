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
    public partial class DailyOperationalDataReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var selectList = Enumerable.Range(2008, (DateTime.Now.Year - 2008) + 1);
                    SetPageTitle();
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    txtFromDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));
                    txtToDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));

                    if (userID > 0) // Irrigation Staff
                    {
                        LoadAllRegionDDByUser(userID, IrrigationLevelID);
                    }
                    else
                    {
                        BindDropdownlists();
                    }
                    LoadChannelDischargeHistorySecondaryParam();

                    
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
            //Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.DailyDataOperationalReport);
            //Master.ModuleTitle = pageTitle.Item1;
            //Master.PageTitle = pageTitle.Item2;
            //Master.NavigationBar = pageTitle.Item3;
        }
        private void LoadAllRegionDDByUser(long _UserID, long? _IrrigationLevelID)
        {
            if (_IrrigationLevelID == null)
                return;

            List<object> lstData = new UserAdministrationBLL().GetRegionsListByUser(_UserID, Convert.ToInt32(_IrrigationLevelID));

            int all = (int)Constants.DropDownFirstOption.All;
            int noOption = (int)Constants.DropDownFirstOption.NoOption;

            List<CO_SubDivision> lstSubDivision = (List<CO_SubDivision>)lstData.ElementAt(0);
            if (lstSubDivision.Count > 0) // Subdivision
            {
                Dropdownlist.BindDropdownlist<List<CO_SubDivision>>(ddlSubDivision, lstSubDivision, lstSubDivision.Count == 1 ? noOption : all);
                //ddlSubDivision.Enabled = true;
                //if (lstSubDivision.Count == 1)
                //{
                //    ddlSubDivision.SelectedIndex = 1;
                //    // ddlSubDivision.Enabled = false;
                //}
            }

            List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
            if (lstDivision.Count > 0) // Division
            {
                Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlDivision, lstDivision, lstDivision.Count == 1 ? noOption : all);
                //ddlDivision.Enabled = true;
                //if (lstDivision.Count == 1)
                //{
                //    ddlDivision.SelectedIndex = 1;
                //    // ddlDivision.Enabled = false;
                //}
            }

            List<CO_Circle> lstCircle = (List<CO_Circle>)lstData.ElementAt(2);
            if (lstCircle.Count > 0) // Circle
            {
                Dropdownlist.BindDropdownlist<List<CO_Circle>>(ddlCircle, lstCircle, lstCircle.Count == 1 ? noOption : all);
                //ddlCircle.Enabled = true;
                //if (lstCircle.Count == 1)
                //{
                //    ddlCircle.SelectedIndex = 1;
                //    // ddlCircle.Enabled = false;
                //}
            }

            List<CO_Zone> lstZone = (List<CO_Zone>)lstData.ElementAt(3);
            if (lstZone.Count > 0) // Zone
            {
                Dropdownlist.BindDropdownlist<List<CO_Zone>>(ddlZone, lstZone, lstZone.Count == 1 ? noOption : all);
                //ddlZone.Enabled = true;
                //if (lstZone.Count == 1)
                //{
                //    ddlZone.SelectedIndex = 1;
                //    //   ddlZone.Enabled = false;
                //}
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
            // Bind empty sub division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        }
        private void DDLEmptyDivisionSubDivision()
        {
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, (int)Constants.DropDownFirstOption.All);
            // Bind empty sub division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iframestyle.Visible = false;
                // Reset Circle,Division, Sub Division dropdownlists
                DDLEmptyCircleDivisionSubDivision();
                // Bind Circel dropdownlist based on Zone
                string zoneID = ddlZone.SelectedItem.Value;
                Dropdownlist.DDLCircles(ddlCircle, false, string.IsNullOrEmpty(zoneID) == true ? -1 : Convert.ToInt64(zoneID), (int)Constants.DropDownFirstOption.All);

                //Dropdownlist.DDLChannels(ddlChannel, true, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
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
                // Reset sub division dropdownlist
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

                int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                // Bind Division dropdownlist based on Circle 
                Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);

                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
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
                // Reset sub division dropdownlist
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

                // Bind Sub Division dropdownlist based on Division 
                long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, divisionID, (int)Constants.DropDownFirstOption.All);

                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Dropdownlist.DDLChannels(ddlChannel, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
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
                case (int)ReportConstants.DailyOperationalDataReports.ChannelDischargeHistory:
                    rptData = GetChannelDischargeHistoryParams();
                    break;
                case (int)ReportConstants.DailyOperationalDataReports.ClosedChannels:
                    rptData = GetClosedChannelsParams();
                    break;
                case (int)ReportConstants.DailyOperationalDataReports.TailStatusFieldStaffData:
                    rptData = GetTailStatusFieldStaffDataParams();
                    break;
                case (int)ReportConstants.DailyOperationalDataReports.TailStatusPMIUData:
                    rptData = GetTailStatusPMIUDataParams();
                    break;
                case (int)ReportConstants.DailyOperationalDataReports.GaugeReadingComparison:
                    rptData = GetGaugeReadingComparisonParams();
                    break;
                case (int)ReportConstants.DailyOperationalDataReports.ChangesInGaugeReadings:
                    rptData = GetChangesInGaugeReadingsParams();
                    break;
                case (int)ReportConstants.DailyOperationalDataReports.GaugeStatusFieldStaffData:
                    rptData = GetGaugeStatusFieldStaffDataParams();
                    break;
                case (int)ReportConstants.DailyOperationalDataReports.GaugeStatusComparison:
                    rptData = GetGaugeStatusComparisonParams();
                    break;
                case (int)ReportConstants.DailyOperationalDataReports.Indents:
                    rptData = GetIndentsParams();
                    break;
                default:

                    break;
            }
            return rptData;
        }

        #region "Reports Paramert Methods"
        private ReportData GetReportPrimaryParameters()
        {
            string ZoneID = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
            string CircleID = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
            string DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;
            string SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? "0" : ddlSubDivision.SelectedItem.Value;
            string FromDate = string.IsNullOrEmpty(txtFromDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtFromDate.Text));
            string ToDate = string.IsNullOrEmpty(txtToDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtToDate.Text));

            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("ZoneID", ZoneID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("CircleID", CircleID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("DivisionID", DivisionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("SubDivisionID", SubDivisionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("FromDate", FromDate);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ToDate", ToDate);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
        private ReportData GetChannelDischargeHistoryParams()
        {
            string ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? "0" : ddlChannel.SelectedItem.Value;
            string GaugeID = ddlGauge.SelectedItem.Value == string.Empty ? "0" : ddlGauge.SelectedItem.Value;
            string SessionID = ddlSession.SelectedItem.Value == string.Empty ? "0" : ddlSession.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptChannelDischargeHistory;

            ReportParameter reportParameter = new ReportParameter("ChannelID", ChannelID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("GaugeCategoryID", GaugeID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("Session", SessionID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetClosedChannelsParams()
        {
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptClosedChannels;

            return rptData;
        }
        private ReportData GetTailStatusFieldStaffDataParams()
        {
            string HeadStatusID = ddlHeadDischargeTailStatusFieldStaffData.SelectedItem.Value == string.Empty ? "0" : ddlHeadDischargeTailStatusFieldStaffData.SelectedItem.Value;
            string TailStatusID = ddlTailStatusTailStatusFieldStaffData.SelectedItem.Value == string.Empty ? "0" : ddlTailStatusTailStatusFieldStaffData.SelectedItem.Value;
            string SessionID = ddlSession.SelectedItem.Value == string.Empty ? "0" : ddlSession.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptTailStatusFieldStaff;

            ReportParameter reportParameter = new ReportParameter("Session", SessionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("HeadStatus", HeadStatusID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("TailStatus", TailStatusID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetTailStatusPMIUDataParams()
        {
            string TailStatusID = ddlTailStatusTailStatusPMIUData.SelectedItem.Value == string.Empty ? "0" : ddlTailStatusTailStatusPMIUData.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptTailStatusPMIUStaff;

            ReportParameter reportParameter = new ReportParameter("TailStatus", TailStatusID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetGaugeReadingComparisonParams()
        {
            string GaugeDifferenceID = ddlGaugeComparison.SelectedItem.Value == string.Empty ? "0" : ddlGaugeComparison.SelectedItem.Value;
            string SessionID = ddlSession.SelectedItem.Value == string.Empty ? "0" : ddlSession.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptGaugeReadingComparison;

            ReportParameter reportParameter = new ReportParameter("Session", SessionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("GaugeDifference", GaugeDifferenceID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetChangesInGaugeReadingsParams()
        {
            string GaugeChangedByID = ddlGaugeReadingsChangedBy.SelectedItem.Value == string.Empty ? "0" : ddlGaugeReadingsChangedBy.SelectedItem.Value;
            string SessionID = ddlSessionChangesInGaugeReadings.SelectedItem.Value == string.Empty ? "0" : ddlSessionChangesInGaugeReadings.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptChangesInGaugeReadings;

            ReportParameter reportParameter = new ReportParameter("Session", SessionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("GaugeChangedBy", GaugeChangedByID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetGaugeStatusFieldStaffDataParams()
        {
            string GaugeStatusID = ddlGaugeStatusGaugeStatusFieldStaffData.SelectedItem.Value == string.Empty ? "0" : ddlGaugeStatusGaugeStatusFieldStaffData.SelectedItem.Value;
            string SessionID = ddlSessionGaugeStatusFieldStaffData.SelectedItem.Value == string.Empty ? "0" : ddlSessionGaugeStatusFieldStaffData.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptGaugeStatusFieldStaff;

            ReportParameter reportParameter = new ReportParameter("Session", SessionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("GaugeStatusID", GaugeStatusID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetGaugeStatusComparisonParams()
        {
            string GaugeStatusID = ddlGaugeStatusComparison.SelectedItem.Value == string.Empty ? "0" : ddlGaugeStatusComparison.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptGaugeStatusComparison;

            ReportParameter reportParameter = new ReportParameter("GaugeStatus", GaugeStatusID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetIndentsParams()
        {
            string ChannelID = ddlChannelIndents.SelectedItem.Value == string.Empty ? "0" : ddlChannelIndents.SelectedItem.Value;
            string IndentPlacementID = ddlIndentPlacements.SelectedItem.Value == string.Empty ? "0" : ddlIndentPlacements.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptChannelIndent;

            ReportParameter reportParameter = new ReportParameter("ChannelID", ChannelID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("IndentPlacement", IndentPlacementID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        #endregion

        #region "Load Reports Secondary Parameters"
        private List<CO_Channel> GetChannelsByIrrigationBoundary()
        {
            long ZoneID = ddlZone.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlZone.SelectedItem.Value);
            long CircleID = ddlCircle.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
            long DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
            long SubDivisionID = ddlSubDivision.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlSubDivision.SelectedItem.Value);
            if (ZoneID==-1 && DivisionID==-1 && CircleID==-1  && SubDivisionID==-1)
            {
                List<CO_Channel> lst = null;
                return lst;
            }
            return new ChannelBLL().GetChannelsByIrrigationBoundary(ZoneID, CircleID, DivisionID, SubDivisionID);
        }
        private void LoadChannelDischargeHistorySecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLGetAllGaugeCategories(ddlGauge, false, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlSession, CommonLists.GetSession(), (int)Constants.DropDownFirstOption.All);
            SPChannelDischargeHistory.Visible = true;
        }
        private void LoadTailStatusFieldStaffDataSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlSessionTailStatusFieldStaffData, CommonLists.GetSession(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlHeadDischargeTailStatusFieldStaffData, CommonLists.GetHeadDischarge(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlTailStatusTailStatusFieldStaffData, CommonLists.GetFieldStaffTailStatus(), (int)Constants.DropDownFirstOption.All);
            SPTailStatusFieldStaffData.Visible = true;
        }
        private void LoadTailStatusPMIUDataSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlTailStatusTailStatusPMIUData, CommonLists.GetFieldStaffTailStatus(), (int)Constants.DropDownFirstOption.All);
            SPTailStatusPMIUData.Visible = true;
        }
        private void LoadGaugeReadingComparisonSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlSessionGaugeReadingComparison, CommonLists.GetSession(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlGaugeComparison, CommonLists.GetGaugeComparison(), (int)Constants.DropDownFirstOption.All);
            SPGaugeReadingComparison.Visible = true;
        }
        private void LoadChangesInGaugeReadingsSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlSessionChangesInGaugeReadings, CommonLists.GetSession(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlGaugeReadingsChangedBy, CommonLists.GetGaugeReadingsChangedBy(), (int)Constants.DropDownFirstOption.All);
            SPChangesInGaugeReadings.Visible = true;
        }
        private void LoadGaugeStatusFieldStaffDataSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlSessionGaugeStatusFieldStaffData, CommonLists.GetSession(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlGaugeStatusGaugeStatusFieldStaffData, CommonLists.GetGaugeStatus(), (int)Constants.DropDownFirstOption.All);
            SPGaugeStatusFieldStaffData.Visible = true;
        }
        private void LoadGaugeStatusComparisonSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlGaugeStatusComparison, CommonLists.GetGaugeStatusComparison(), (int)Constants.DropDownFirstOption.All);
            SPGaugeStatusComparison.Visible = true;
        }
        private void LoadIndentsSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannelIndents, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlIndentPlacements, CommonLists.GetIndentPlacements(), (int)Constants.DropDownFirstOption.All);
            SPIndents.Visible = true;
        }
        #endregion
        private void HideSecondaryParameters()
        {
            SPChannelDischargeHistory.Visible = false;
            SPTailStatusFieldStaffData.Visible = false;
            SPTailStatusPMIUData.Visible = false;
            SPGaugeReadingComparison.Visible = false;
            SPChangesInGaugeReadings.Visible = false;
            SPGaugeStatusFieldStaffData.Visible = false;
            SPGaugeStatusComparison.Visible = false;
            SPIndents.Visible = false;
        }
        private int GetReportID()
        {
            int reportID = 0;
            if (rbChannelDischargeHistory.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.ChannelDischargeHistory;
            else if (rbClosedChannels.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.ClosedChannels;
            else if (rbTailStatusFieldStaff.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.TailStatusFieldStaffData;
            else if (rbTailStatusPMIU.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.TailStatusPMIUData;
            else if (rbGaugeReadingComparsion.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.GaugeReadingComparison;
            else if (rbGaugeStatusComparsion.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.GaugeStatusComparison;
            else if (rbGaugeStatusFieldStaff.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.GaugeStatusFieldStaffData;
            else if (rbChangeInGaugeReading.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.ChangesInGaugeReadings;
            else if (rbIndents.Checked)
                reportID = (int)ReportConstants.DailyOperationalDataReports.Indents;

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
                    case (int)ReportConstants.DailyOperationalDataReports.ChannelDischargeHistory:
                        LoadChannelDischargeHistorySecondaryParam();
                        break;
                    case (int)ReportConstants.DailyOperationalDataReports.TailStatusFieldStaffData:
                        LoadTailStatusFieldStaffDataSecondaryParam();
                        break;
                    case (int)ReportConstants.DailyOperationalDataReports.TailStatusPMIUData:
                        LoadTailStatusPMIUDataSecondaryParam();
                        break;
                    case (int)ReportConstants.DailyOperationalDataReports.GaugeReadingComparison:
                        LoadGaugeReadingComparisonSecondaryParam();
                        break;
                    case (int)ReportConstants.DailyOperationalDataReports.ChangesInGaugeReadings:
                        LoadChangesInGaugeReadingsSecondaryParam();
                        break;
                    case (int)ReportConstants.DailyOperationalDataReports.GaugeStatusFieldStaffData:
                        LoadGaugeStatusFieldStaffDataSecondaryParam();
                        break;
                    case (int)ReportConstants.DailyOperationalDataReports.GaugeStatusComparison:
                        LoadGaugeStatusComparisonSecondaryParam();
                        break;
                    case (int)ReportConstants.DailyOperationalDataReports.Indents:
                        LoadIndentsSecondaryParam();
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
    }
}