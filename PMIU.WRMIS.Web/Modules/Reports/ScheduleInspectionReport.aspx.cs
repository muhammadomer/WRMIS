using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.ScheduleInspection;
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
    public partial class ScheduleInspectionReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
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
                    Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlGaugeRD, null, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlInspectedBy, CommonLists.GetScheduleInspectionInspectedBy(), (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlInspectionType, CommonLists.GetInspectionCategories(), (int)Constants.DropDownFirstOption.All);

                    LoadGaugeInspectionSecondaryParam();

                }
                iframestyle.Visible = false;
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
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
            }

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
                // Reset Circle,Division, Sub Division dropdownlists
                DDLEmptyCircleDivisionSubDivision();
                // Bind Circel dropdownlist based on Zone
                string zoneID = ddlZone.SelectedItem.Value;
                Dropdownlist.DDLCircles(ddlCircle, false, string.IsNullOrEmpty(zoneID) == true ? -1 : Convert.ToInt64(zoneID), (int)Constants.DropDownFirstOption.All);

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
                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist<List<object>>(ddlGaugeRD, null, (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        protected void btnLoadScheduleInspection_Click(object sender, EventArgs e)
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
                case (int)ReportConstants.ScheduleInspectionReports.GaugeInspection:
                    rptData = GetGaugeInspectionParams();
                    break;
                case (int)ReportConstants.ScheduleInspectionReports.OutletPerformance:
                    rptData = GetOutletPerformanceParams();
                    break;
                case (int)ReportConstants.ScheduleInspectionReports.DischargeTableCalculations:
                    rptData = GetDischargeTableCalculationsParams();
                    break;
                case (int)ReportConstants.ScheduleInspectionReports.InspectionOfOutletAlteration:
                    rptData = GetInspectionOfOutletAlterationParams();
                    break;
                case (int)ReportConstants.ScheduleInspectionReports.Tenders:
                    rptData = GetTendersParams();
                    break;
                case (int)ReportConstants.ScheduleInspectionReports.Works:
                    rptData = GetWorksParams();
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
            string ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? "0" : ddlChannel.SelectedItem.Value;
            string InspectionType = ddlInspectionType.SelectedItem.Value == string.Empty ? "0" : ddlInspectionType.SelectedItem.Value;
            string InspectedBy = ddlInspectedBy.SelectedItem.Value == string.Empty ? "0" : ddlInspectedBy.SelectedItem.Value;
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

            reportParameter = new ReportParameter("ChannelID", ChannelID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("InspectionType", InspectionType);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("InspectedBy", InspectedBy);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("FromDate", FromDate);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ToDate", ToDate);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
        private ReportData GetGaugeInspectionParams()
        {
            string GaugeCategory = ddlGaugeCategory.SelectedItem.Value == string.Empty ? "0" : ddlGaugeCategory.SelectedItem.Value;
            string GaugeStatus = ddlGaugeStatus.SelectedItem.Value == string.Empty ? "0" : ddlGaugeStatus.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptGaugeInspection;

            ReportParameter reportParameter = new ReportParameter("GaugeCategory", GaugeCategory);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("GaugeStatus", GaugeStatus);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetOutletPerformanceParams()
        {
            string Efficiency = ddlEfficiency.SelectedItem.Value == string.Empty ? "0" : ddlEfficiency.SelectedItem.Value;
            string OutletID = ddlOutlet.SelectedItem.Value == string.Empty ? "0" : ddlOutlet.SelectedItem.Value;
            

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptOutletPerformance;

            ReportParameter reportParameter = new ReportParameter("Efficiency", Efficiency);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("OutletID", OutletID);
            rptData.Parameters.Add(reportParameter);
            
            return rptData;
        }
        private ReportData GetDischargeTableCalculationsParams()
        {
            string GaugeCategory = ddlGaugeCategory.SelectedItem.Value == string.Empty ? "0" : ddlGaugeCategory.SelectedItem.Value;
            string GaugeRD = ddlGaugeRD.SelectedItem.Value == string.Empty ? "0" : ddlGaugeRD.SelectedItem.Value;
            string DTCalculationType = ddlDTCalculation.SelectedItem.Value == string.Empty ? "0" : ddlDTCalculation.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptDischargeTableCalculation;

            ReportParameter reportParameter = new ReportParameter("GaugeCategory", GaugeCategory);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("GaugeRD", GaugeRD);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("DTCalculationType", DTCalculationType);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetInspectionOfOutletAlterationParams()
        {
            string OutletID = ddlOutlet.SelectedItem.Value == string.Empty ? "0" : ddlOutlet.SelectedItem.Value;
            string AlterType = ddlAlteredFor.SelectedItem.Value == string.Empty ? "0" : ddlAlteredFor.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptOutletAlteration;

            ReportParameter reportParameter = new ReportParameter("OutletID", OutletID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("AlterType", AlterType);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetTendersParams()
        {
            //string GaugeDifferenceID = ddlGaugeComparison.SelectedItem.Value == string.Empty ? "-1" : ddlGaugeComparison.SelectedItem.Value;
            //string SessionID = ddlSession.SelectedItem.Value == string.Empty ? "-1" : ddlSession.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptGaugeReadingComparison;

            //ReportParameter reportParameter = new ReportParameter("Session", SessionID);
            //rptData.Parameters.Add(reportParameter);

            //reportParameter = new ReportParameter("GaugeDifference", GaugeDifferenceID);
            //rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetWorksParams()
        {
            //string GaugeChangedByID = ddlGaugeReadingsChangedBy.SelectedItem.Value == string.Empty ? "-1" : ddlGaugeReadingsChangedBy.SelectedItem.Value;
            //string SessionID = ddlSessionChangesInGaugeReadings.SelectedItem.Value == string.Empty ? "-1" : ddlSessionChangesInGaugeReadings.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptGaugeReadingComparison; // To DO: put report correct path 

            //ReportParameter reportParameter = new ReportParameter("Session", SessionID);
            //rptData.Parameters.Add(reportParameter);

            //reportParameter = new ReportParameter("GaugeChangedBy", GaugeChangedByID);
            //rptData.Parameters.Add(reportParameter);

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
            if (ZoneID == -1 && DivisionID == -1 && CircleID == -1 && SubDivisionID == -1)
            {
                List<CO_Channel> lst = new List<CO_Channel>();
                //lst.Add(new CO_Channel());
                return lst;
            }
            return new ChannelBLL().GetChannelsByIrrigationBoundary(ZoneID, CircleID, DivisionID, SubDivisionID);
        }
        private void LoadGaugeInspectionSecondaryParam()
        {
            Dropdownlist.DDLGetAllGaugeCategories(ddlGaugeCategory, false, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlGaugeStatus, CommonLists.GetGaugeStatus(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlInspectedBy, CommonLists.GetScheduleInspectionInspectedBy(), (int)Constants.DropDownFirstOption.All);
            SPGaugeCategory.Visible = true;
            SPGaugeStatus.Visible = true;
        }
        private void LoadOutletPerformanceSecondaryParam()
        {
            string ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? "-1" : ddlChannel.SelectedItem.Value;
            Dropdownlist.BindDropdownlist<List<object>>(ddlOutlet, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ChannelID)), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlEfficiency, CommonLists.GetEfficiency(), (int)Constants.DropDownFirstOption.All);
            
            SPOutlet.Visible = true;
            SPOutletPerformance.Visible = true;
            SPAlteredFor.Visible = false;
        }
        private void LoadDischargeTableCalculationsSecondaryParam()
        {
            Dropdownlist.DDLGetAllGaugeCategories(ddlGaugeCategory, false, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLGaugeLevels(ddlDTCalculation);
            Dropdownlist.BindDropdownlist<List<object>>(ddlInspectedBy, CommonLists.GetScheduleInspectionInspectedBy(), (int)Constants.DropDownFirstOption.All);

            SPGaugeCategory.Visible = true;
            SPDischargeTable.Visible = true;
        }
        private void LoadInspectionOfOutletAlterationSecondaryParam()
        {
            string ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? "-1" : ddlChannel.SelectedItem.Value;
            Dropdownlist.BindDropdownlist<List<object>>(ddlOutlet, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ChannelID)), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlInspectedBy, CommonLists.GetScheduleInspectionInspectedBy(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlAlteredFor, CommonLists.GetAlteredChangedValueOf(), (int)Constants.DropDownFirstOption.All);
            SPOutlet.Visible = true;
            SPAlteredFor.Visible = true;
        }
        #endregion
        private void HideSecondaryParameters()
        {
            SPOutlet.Visible = false;
            SPGaugeCategory.Visible = false;
            SPDischargeTable.Visible = false;
            SPGaugeStatus.Visible = false;
            SPOutletPerformance.Visible = false;
            SPAlteredFor.Visible = false;
        }
        private int GetReportID()
        {
            int reportID = 0;
            if (rbGaugeInspection.Checked)
                reportID = (int)ReportConstants.ScheduleInspectionReports.GaugeInspection;
            else if (rbOutletPerformance.Checked)
                reportID = (int)ReportConstants.ScheduleInspectionReports.OutletPerformance;
            else if (rbInspectionOfOutletAlteration.Checked)
                reportID = (int)ReportConstants.ScheduleInspectionReports.InspectionOfOutletAlteration;
            else if (rbDischargeTableCalculations.Checked)
                reportID = (int)ReportConstants.ScheduleInspectionReports.DischargeTableCalculations;
            //else if (rbTenders.Checked)
            //    reportID = (int)ReportConstants.ScheduleInspectionReports.Tenders;
            //else if (rbWorks.Checked)
            //    reportID = (int)ReportConstants.ScheduleInspectionReports.Works;
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
                    case (int)ReportConstants.ScheduleInspectionReports.GaugeInspection:
                        LoadGaugeInspectionSecondaryParam();
                        break;
                    case (int)ReportConstants.ScheduleInspectionReports.OutletPerformance:
                        LoadOutletPerformanceSecondaryParam();
                        break;
                    case (int)ReportConstants.ScheduleInspectionReports.InspectionOfOutletAlteration:
                        LoadInspectionOfOutletAlterationSecondaryParam();
                        break;
                    case (int)ReportConstants.ScheduleInspectionReports.DischargeTableCalculations:
                        LoadDischargeTableCalculationsSecondaryParam();
                        break;
                    //case (int)ReportConstants.ScheduleInspectionReports.Tenders:

                    //    break;
                    //case (int)ReportConstants.ScheduleInspectionReports.Works:

                    //    break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlChannel.SelectedItem.Value);
                if (rbOutletPerformance.Checked || rbInspectionOfOutletAlteration.Checked)
                    Dropdownlist.BindDropdownlist<List<object>>(ddlOutlet, new WaterTheftBLL().GetOutletByChannelId(ChannelID, 0, 0), (int)Constants.DropDownFirstOption.All);
                if (ChannelID == 0)
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    Dropdownlist.DDLGaugeRDbyChannelID(ddlGaugeRD, Convert.ToInt64(ddlChannel.SelectedValue), (int)Constants.DropDownFirstOption.All, "Name", "ID");
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}