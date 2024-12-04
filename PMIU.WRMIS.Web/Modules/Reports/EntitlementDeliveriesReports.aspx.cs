using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.Reports;
using PMIU.WRMIS.BLL.SeasonalPlanning;
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
    public partial class EntitlementDeliveriesReports : BasePage
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
                    Dropdownlist.BindDropdownlist<List<object>>(ddlReports, CommonLists.GetReports(), (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlSeason, CommonLists.GetSeasonDropDown(), (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlCommandType, CommonLists.GetCommandType(), (int)Constants.DropDownFirstOption.Select);
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

        protected void ddlSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSeason.SelectedValue != "")
            {
                ddlYearDiv.Visible = true;
                List<object> lst = new ReportsBLL().GetEntitlementAndDeliveriesYearBySessionID(Convert.ToInt32(ddlSeason.SelectedValue), ddlReports.SelectedIndex);
                Dropdownlist.BindDropdownlist<List<object>>(ddlYear, lst);
            }
            else
            {
                ddlYearDiv.Visible = false;
            }
        }

        protected void ddlReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSeason.SelectedValue != "")
            {
                ddlYearDiv.Visible = true;
                List<object> lst = new ReportsBLL().GetEntitlementAndDeliveriesYearBySessionID(Convert.ToInt32(ddlSeason.SelectedValue), ddlReports.SelectedIndex);
                Dropdownlist.BindDropdownlist<List<object>>(ddlYear, lst);
            }
            else
            {
                ddlYearDiv.Visible = false;
            }


            if (ddlReports.SelectedItem.Value != "4")
            {
                ddlMonthDiv.Visible = false;
                ddlCommandTypeDiv.Visible = true;

            }
            else
            {
                ddlCommandTypeDiv.Visible = false;
                ddlMonthDiv.Visible = true;
                Dropdownlist.BindDropdownlist<List<object>>(ddlMonth, CommonLists.GetMonthListAbr(), (int)Constants.DropDownFirstOption.Select);
            }

            if (ddlReports.SelectedItem.Value != "5")
            {
                // ddlMonthDiv.Visible = false;
                ddlTenDailyDiv.Visible = false;
                ddlCommandTypeDiv.Visible = true;
            }
            else
            {
                ddlCommandTypeDiv.Visible = false;
                ddlMonthDiv.Visible = true;
                ddlTenDailyDiv.Visible = true;
                Dropdownlist.BindDropdownlist<List<object>>(ddlMonth, CommonLists.GetMonthListAbr(), (int)Constants.DropDownFirstOption.Select);
                Dropdownlist.BindDropdownlist<List<object>>(ddlTenDaily, CommonLists.GetTenDaily(), (int)Constants.DropDownFirstOption.Select);
            }
            if (ddlReports.SelectedItem.Value == "4")
            {
                ddlCommandTypeDiv.Visible = false;

            }
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReports.SelectedItem.Value == "5")
            {
                ddlTenDailyDiv.Visible = true;
                Dropdownlist.BindDropdownlist<List<object>>(ddlTenDaily, CommonLists.GetTenDaily(), (int)Constants.DropDownFirstOption.Select);
            }
            else
            {
                ddlTenDailyDiv.Visible = false;
            }

        }

        #endregion

        #region Button
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string reports = ddlReports.SelectedItem.Value;
                string season = ddlSeason.SelectedItem.Value;
                string commandType = "";
                string month = "";
                string tenDaily = "";
                ReportData rptData = new ReportData();
                ReportParameter reportParameter = null;

                reportParameter = new ReportParameter("Year", ddlYear.SelectedItem.Value);
                rptData.Parameters.Add(reportParameter);

                reportParameter = new ReportParameter("SeasonID", season);
                rptData.Parameters.Add(reportParameter);


                if (reports == "4" || reports == "5")
                {
                    month = ddlMonth.SelectedItem.Value;
                    reportParameter = new ReportParameter("Month", month);
                    rptData.Parameters.Add(reportParameter);

                }
                else
                {
                    commandType = ddlCommandType.SelectedItem.Value;
                    reportParameter = new ReportParameter("CommandTypeID", commandType);
                    rptData.Parameters.Add(reportParameter);

                }

                if (reports == "5")
                {
                    tenDaily = ddlTenDaily.SelectedItem.Value;
                    reportParameter = new ReportParameter("TenDaily", tenDaily);
                    rptData.Parameters.Add(reportParameter);

                }


                if (reports == "1")
                {
                    rptData.Name = ReportConstants.rptEDPlannedVsActualDeliveries;
                }
                else if (reports == "2")
                {
                    if (season == "1" && commandType == "1")
                    {
                        rptData.Name = ReportConstants.rptEDTentativeDisProIndusZoneRabi;
                    }
                    if (season == "1" && commandType == "2")
                    {
                        rptData.Name = ReportConstants.rptEDTentativeDisProJhelumChenabRabi;
                    }
                    if (season == "2" && commandType == "1")
                    {
                        rptData.Name = ReportConstants.rptEDTentativeDisProIndusZoneKharif;
                    }
                    if (season == "2" && commandType == "2")
                    {
                        rptData.Name = ReportConstants.rptEDTentativeDisProJhelumChenabKharif;
                    }
                }
                else if (reports == "3")
                {
                    if (season == "1" && commandType == "1")
                    {
                        rptData.Name = ReportConstants.rptEDTentativeEntitlementProIndusZoneRabi;
                    }
                    if (season == "1" && commandType == "2")
                    {
                        rptData.Name = ReportConstants.rptEDTentativeEntitlementProJhelumChenabRabi;
                    }
                    if (season == "2" && commandType == "1")
                    {
                        rptData.Name = ReportConstants.rptEDTentativeEntitlementProIndusZoneKharif;
                    }
                    if (season == "2" && commandType == "2")
                    {
                        rptData.Name = ReportConstants.rptEDTentativeEntitlementProJhelumChenabKharif;
                    }
                }
                else if (reports == "4")
                {
                    ddlCommandTypeDiv.Visible = false;
                    rptData.Name = ReportConstants.rptEDPunjabCanalWithdrawalsIndusJC;
                }
                else if (reports == "5")
                {
                    rptData.Name = ReportConstants.rptEDTentativePunjabCanalsWithdrawalsJC;
                }

                Session[SessionValues.ReportData] = rptData;
                iframestyle.Visible = true;
                iframestyle.Src = Constants.ReportsUrl;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #endregion




        #region Functions


        #endregion












    }
}
