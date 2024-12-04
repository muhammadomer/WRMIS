using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.SeasonalPlanning;
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
    public partial class SeasonalPlanning : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlSeason, CommonLists.GetSeasonDropDown(), (int)Constants.DropDownFirstOption.Select);

                }
                iframestyle.Visible = false;
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Events

        #region Dropdown
        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSeason.SelectedValue != "")
            {
                SPYear.Visible = true;
                SPReports.Visible = true;

                List<object> lst = new SeasonalPlanningBLL().GetSeasonalPlanningYearBySessionID(Convert.ToInt32(ddlSeason.SelectedValue));
                if (ddlSeason.SelectedValue == "1")
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlReports, CommonLists.GetRabiSession(), (int)Constants.DropDownFirstOption.Select);
                }
                else
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlReports, CommonLists.GetKharifSession(), (int)Constants.DropDownFirstOption.Select);
                }
                Dropdownlist.BindDropdownlist<List<object>>(ddlYear, lst);
            }
            else
            {
                SPYear.Visible = false;
                SPReports.Visible = false;
            }
        }
        protected void ddlRiverSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRiverSeason.SelectedValue != "")
            {
                RFFromYear.Visible = true;
                RFToYear.Visible = true;
                divTDaily.Visible = true;

                List<object> lst = new SeasonalPlanningBLL().GetSeasonalPlanningYearForRiverFlowBySessionID(Convert.ToInt32(ddlRiverSeason.SelectedValue));
                Dropdownlist.BindDropdownlist<List<object>>(ddlFromYear, lst);
                Dropdownlist.BindDropdownlist<List<object>>(ddlToYear, lst);
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlTDaily, CommonLists.GetTDailyDropDown(), (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                RFFromYear.Visible = false;
                RFToYear.Visible = false;
                divTDaily.Visible = false;
            }
        }


        #endregion

        #region Button
        protected void btnSeasonalPlanningReport_Click(object sender, EventArgs e)
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

        protected void rbSeasonalPlanning_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSeasonalPlanning.Checked)
            {
                divSeasonalPlanning.Visible = true;
                divRiverFlow.Visible = false;
            }
        }
        protected void rbRiverFlows_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRiverFlows.Checked)
            {
                divSeasonalPlanning.Visible = false;
                divRiverFlow.Visible = true;
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlRiverSeason, CommonLists.GetSeasonDropDown(), (int)Constants.DropDownFirstOption.Select);
            }
        }
        #endregion

        #endregion

        #region Functions
        private ReportData GetReportData()
        {
            ReportData rptData = new ReportData();

            if (rbSeasonalPlanning.Checked)
            {
                //Seasonal Planning
                rptData = GetSeasonalPlanningParameters();    
            }
            if (rbRiverFlows.Checked)
            {
                //River Flow
                rptData = GetRiverFlowParameters();
            }
            
            return rptData;
        }

        
        #region "Get Reports Paramert Methods"
        private ReportData GetSeasonalPlanningParameters()
        {
            int reportID = Convert.ToInt32(ddlReports.SelectedValue);
            string Year = ddlYear.SelectedItem.Value == string.Empty ? "0" : ddlYear.SelectedItem.Value;
            string Season = ddlSeason.SelectedItem.Value == string.Empty ? "0" : ddlSeason.SelectedItem.Value;
            string Station = ddlReports.SelectedItem.Text.ToUpper().Contains("TARBELA") ? "20" : ddlReports.SelectedItem.Text.ToUpper().Contains("MANGLA") ? "18" : ddlReports.SelectedItem.Text.ToUpper().Contains("CHENAB") ? "5" : ddlReports.SelectedItem.Text.ToUpper().Contains("KABUL") ? "24" : "";
            string Scenario = ddlReports.SelectedItem.Text.ToUpper().Contains("MAX") ? "1" : ddlReports.SelectedItem.Text.ToUpper().Contains("MIN") ? "2" : ddlReports.SelectedItem.Text.ToUpper().Contains("LIKELY") ? "3" : "";
            ReportParameter reportParameter = null;
            ReportData rptData = new ReportData();
            reportParameter = new ReportParameter("Year", Year);
            rptData.Parameters.Add(reportParameter);
            if (!string.IsNullOrEmpty(Station))
            {
                reportParameter = new ReportParameter("StationID", Station);
                rptData.Parameters.Add(reportParameter);
            }
            if (!string.IsNullOrEmpty(Scenario))
            {
                reportParameter = new ReportParameter("ScenarioID", Scenario);
                rptData.Parameters.Add(reportParameter);
            }

            rptData.Name = GetReportName(reportID);

            if ((rptData.Name.ToUpper().Contains("RESERVOIROPERATIONS")))
            {
                reportParameter = new ReportParameter("IsPreviousTDaily", "0");
                rptData.Parameters.Add(reportParameter);
            }

            bool flage = false;
            if ((rptData.Name.ToUpper().Contains("RABI")))
            {
                flage = true;
            }
            if ((rptData.Name.ToUpper().Contains("KHARIF")))
            {
                flage = true;
            }
            if (!flage)
            {
                reportParameter = new ReportParameter("SeasonID", Season);
                rptData.Parameters.Add(reportParameter);
                flage = false;
            }




            return rptData;
        }

        private ReportData GetRiverFlowParameters()
        {
            string Season = ddlRiverSeason.SelectedItem.Value == string.Empty ? "0" : ddlRiverSeason.SelectedItem.Value;
            string YearFrom = ddlFromYear.SelectedItem.Value == string.Empty ? "0" : ddlFromYear.SelectedItem.Value;
            string YearTo = ddlToYear.SelectedItem.Value == string.Empty ? "0" : ddlToYear.SelectedItem.Value;
            string TDaily = ddlTDaily.SelectedItem.Value == string.Empty ? "0" : ddlTDaily.SelectedItem.Value;

            ReportParameter reportParameter = null;
            ReportData rptData = new ReportData();
            reportParameter = new ReportParameter("YearFrom", YearFrom);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("YearTo", YearTo);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("TDaily", TDaily);
            rptData.Parameters.Add(reportParameter);

            if (Season == "1")
            {
                rptData.Name = ReportConstants.rptSP_RiverFlowsAtRIMStationRabi;    
            }
            if (Season == "2")
            {
                rptData.Name = ReportConstants.rptSP_RiverFlowsAtRIMStationKharif;
            }

            return rptData;
        }

        public string GetReportName(int reportID)
        {


            #region Common Reports
            if (reportID == 7 || reportID == 15) //4 reports
            {
                return ReportConstants.rptDamRuleCurveRabiKharifTarbelaMangla;
            }
            if (reportID == 6 || reportID == 14) //2 reports
            {
                return ReportConstants.rptReservoirOperationsRabiKharifTarbelaMangla;
            }
            #endregion
            #region Rabi
            if (ddlSeason.SelectedItem.Value == "1") // Rabi
            {
                if (reportID == 1)
                {
                    return ReportConstants.rptAnticipatedWaterAvailabilityBasinRabi;
                }
                else if (reportID == 2)
                {
                    return ReportConstants.rptAnticipatedWaterAvailabilityRiverRabi;
                }
                else if (reportID == 3 || reportID == 4)
                {
                    return ReportConstants.rptDistributionRABI_MinMax;
                }
                else if (reportID == 5)
                {
                    return ReportConstants.rptInflowForecastRabi;
                }
                else if (reportID == 8 || reportID == 9 || reportID == 10) //3 reports
                {
                    return ReportConstants.rptProvincialSharesRABI_MinMaxMostLikely;
                }
                else if (reportID == 11 || reportID == 12 || reportID == 13) //1 reports
                {

                    return ReportConstants.rptSystemOperationOfJCRabi;

                }
                else if (reportID == 16)
                {
                    return ReportConstants.rptWaterAvailbilityJCRabi;
                }
                else if (reportID == 17 || reportID == 18 || reportID == 19) //1 reports
                {

                    return ReportConstants.rptSystemOperationOfIndusRabi;
                }
                else if (reportID == 20 || reportID == 21 || reportID == 22 || reportID == 23) //1 reports
                {

                    return ReportConstants.rptProbabilityFlowsRabi;
                }
                else
                {
                    return "";
                }
            }
            #endregion
            #region Kharif
            else
            {
                if (reportID == 1)
                {
                    return ReportConstants.rptAnticipatedWaterAvailabilityBasinKharif;
                }
                else if (reportID == 2)
                {
                    return ReportConstants.rptAnticipatedWaterAvailabilityRiverKharif;
                }
                if (reportID == 3 || reportID == 4)
                {
                    return ReportConstants.rptDistributionKharif_MinMax;
                }
                else if (reportID == 5)
                {
                    return ReportConstants.rptInflowForecastKharif;
                }
                else if (reportID == 8 || reportID == 9 || reportID == 10) //3 reports
                {
                    return ReportConstants.rptProvincialSharesKharif_MinMaxMostLikely;
                }
                else if (reportID == 11 || reportID == 12 || reportID == 13) //1 reports
                {

                    return ReportConstants.rptSystemOperationOfJCKharif;
                }
                else if (reportID == 16)
                {
                    return ReportConstants.rptWaterAvailbilityJCKharif;
                }
                else if (reportID == 17 || reportID == 18 || reportID == 19) //1 reports
                {

                    return ReportConstants.rptSystemOperationOfIndusKharif;
                }
                else if (reportID == 20 || reportID == 21 || reportID == 22 || reportID == 23) //1 reports
                {

                    return ReportConstants.rptProbabilityFlowsKharif;
                }
                else
                {
                    return "";
                }
            }
            #endregion
        }
        #endregion

        
        #endregion

        




    }
}
