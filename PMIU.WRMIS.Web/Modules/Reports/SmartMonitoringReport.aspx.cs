using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.DailyData;
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
    public partial class SmartMonitoringReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindDropdowns();
                    txtFromDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));
                    txtToDate.Text = Utility.GetFormattedDate(DateTime.Now);
                    if (SessionManagerFacade.UserInformation.DesignationID == null)
                    {
                        BindDropdowns();
                    }
                    else if ((SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.MA)))
                    {
                        ddlActivityBy.Items.Clear();
                        ddlActivityBy.Items.Insert(0, new ListItem("MA", ""));
                        ddlADM.Enabled = false;
                        ddlMA.DataSource = new DailyDataBLL().GetUser(Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.UserID));
                        ddlMA.DataTextField = "Name";
                        ddlMA.DataValueField = "ID";
                        ddlMA.DataBind();

                    }
                    else if ((SessionManagerFacade.UserInformation.UA_Designations.ID == Convert.ToInt64(Constants.Designation.ADM)))
                    {
                        ddlActivityBy.ClearSelection();
                        ddlActivityBy.Items.FindByText("ADM").Selected = true;
                        ddlADM.SelectedItem.Text = Convert.ToString(SessionManagerFacade.UserAssociatedLocations.UA_Users.FirstName + " " + SessionManagerFacade.UserAssociatedLocations.UA_Users.LastName);
                        ddlADM.SelectedItem.Value = Convert.ToString(SessionManagerFacade.UserInformation.ID);
                        ddlADM.Enabled = true;
                    }
                }
                iframestyle.Visible = false;
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }



        protected void btnLoadScheduleInspection_Click(object sender, EventArgs e)
        {
            try
            {
                string reportName = "";
                int reportID = GetReportID(out reportName);
                Session[SessionValues.ReportData] = GetReportParameters(reportID.ToString(), reportName);
                iframestyle.Visible = true;
                iframestyle.Src = Constants.ReportsUrl;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        #region "Reports Paramert Methods"
        private ReportData GetReportParameters(string ReportID, string reportName)
        {
            string ActivityBy = ddlActivityBy.SelectedItem.Value == string.Empty ? null : ddlActivityBy.SelectedItem.Value;
            //string Ma = ddlMA.SelectedItem.Value == string.Empty ? "0" : ddlMA.SelectedItem.Value;
            //string Adm = ddlADM.SelectedItem.Value == string.Empty ? "0" : ddlADM.SelectedItem.Value;
            //string UserID = Convert.ToString(SessionManagerFacade.UserInformation.ID);
            string FromDate = string.IsNullOrEmpty(txtFromDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtFromDate.Text));
            string ToDate = string.IsNullOrEmpty(txtToDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtToDate.Text));
            long? SelectedUserID = null;
            if (ddlADM.SelectedItem.Value != String.Empty && ddlADM.SelectedItem.Value != "0")
            {
                SelectedUserID = Convert.ToInt64(ddlADM.SelectedItem.Value);
            }
            if (ddlMA.SelectedItem.Value != String.Empty && ddlMA.SelectedItem.Value != "0")
            {
                SelectedUserID = Convert.ToInt64(ddlMA.SelectedItem.Value);
            }

            ReportData rptData = new ReportData();
            ReportParameter reportParameter;

            if (ReportID == Convert.ToString((int)ReportConstants.SmartMonitoring.MeterReading))
            {
                reportParameter = new ReportParameter("ReadingType", "Meter");
                rptData.Parameters.Add(reportParameter);
            }
            else if (ReportID == Convert.ToString((int)ReportConstants.SmartMonitoring.Fuel))
            {
                reportParameter = new ReportParameter("ReadingType", "Fuel");
                rptData.Parameters.Add(reportParameter);
            }
            reportParameter = new ReportParameter("FromDate", FromDate);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ToDate",ToDate);
            rptData.Parameters.Add(reportParameter);

            if (SelectedUserID.HasValue)
            {
                reportParameter = new ReportParameter("UserID", Convert.ToString(SelectedUserID));
                rptData.Parameters.Add(reportParameter);
            }
            else
            {
                reportParameter = new ReportParameter("UserID", Convert.ToString(null));
                rptData.Parameters.Add(reportParameter);
            }

            reportParameter = new ReportParameter("ActivityBy", ActivityBy);
            rptData.Parameters.Add(reportParameter);


            rptData.Name = reportName;
            return rptData;
        }
        #endregion

        private int GetReportID(out string reportName)
        {
            int reportID = 0;
            reportName = "";
            if (rbMeterReading.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.MeterReading;
                reportName = ReportConstants.rptMeterReading;
            }
            else if (rbFuel.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.Fuel;
                reportName = ReportConstants.rptFuel;
            }
            else if (rbGaugeObservation.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.GaugeObservation;
                reportName = ReportConstants.rptGaugeObservation;
            }
            else if (rbOutletChecking.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.OutletChecking;
                reportName = ReportConstants.rptOutletChecking;
            }
            else if (rbCutBreach.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.CutBreach;
                reportName = ReportConstants.rptCutBreach;
            }
            else if (rbRotationalViolation.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.RotationalViolation;
                reportName = ReportConstants.rptSMRotationalViolation;
            }
            else if (rbLeaves.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.Leaves;
                reportName = ReportConstants.rptLeaves;
            }
            else if (rbWaterTheftOutlet.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.WaterTheftOutlet;
                reportName = ReportConstants.rptSMWaterTheftOutlet;
            }
            if (rbWaterTheftChannel.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.WaterTheftChannel;
                reportName = ReportConstants.rptSMWaterTheftChannel;
            }
            if (rbAll.Checked)
            {
                reportID = (int)ReportConstants.SmartMonitoring.All;
                reportName = ReportConstants.rptAll;
            }
            return reportID;
        }


        protected void rbReports_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                iframestyle.Visible = false;
                string reportName = "";
                int reportID = GetReportID(out reportName);
                switch (reportID)
                {
                    case (int)ReportConstants.SmartMonitoring.MeterReading:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.MeterReading), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.Fuel:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.Fuel), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.GaugeObservation:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.GaugeObservation), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.OutletChecking:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.OutletChecking), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.CutBreach:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.CutBreach), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.RotationalViolation:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.RotationalViolation), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.Leaves:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.Leaves), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.WaterTheftOutlet:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.WaterTheftOutlet), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.WaterTheftChannel:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.WaterTheftChannel), reportName);
                        break;
                    case (int)ReportConstants.SmartMonitoring.All:
                        GetReportParameters(Convert.ToString(ReportConstants.SmartMonitoring.All), reportName);
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

        protected void ddlActivityBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (ddlActivityBy.SelectedItem.Value != "")
                {
                    if (ddlActivityBy.SelectedItem.Text == "ADM")
                    {
                        if ((SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.ADM)))
                        {

                            ddlADM.Enabled = true;
                            ddlMA.Enabled = false;
                            ddlADM.SelectedItem.Text = Convert.ToString(SessionManagerFacade.UserAssociatedLocations.UA_Users.FirstName + " " + SessionManagerFacade.UserAssociatedLocations.UA_Users.LastName);
                        }
                        else
                        {
                            ddlADM.Enabled = true;
                            ddlMA.Enabled = false;
                            Dropdownlist.DDLMeterReading(ddlADM, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 12);
                        }

                    }
                    else if (ddlActivityBy.SelectedItem.Text == "MA")
                    {
                        if ((SessionManagerFacade.UserInformation.DesignationID == Convert.ToInt64(Constants.Designation.ADM)))
                        {
                            ddlADM.SelectedItem.Text = Convert.ToString(SessionManagerFacade.UserAssociatedLocations.UA_Users.FirstName + " " + SessionManagerFacade.UserAssociatedLocations.UA_Users.LastName);
                            ddlMA.Enabled = true;
                            Dropdownlist.DDLGetMA(ddlMA, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.UserID));
                        }
                        else
                        {
                            ddlMA.Enabled = true;
                            Dropdownlist.DDLMeterReading(ddlADM, Convert.ToInt64(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID), 12);
                        }

                    }
                }
                else
                {
                    ddlADM.Enabled = true;
                    ddlMA.Items.Clear();
                    ddlADM.Items.Clear();
                    ddlMA.Items.Insert(0, new ListItem("All", ""));
                    ddlADM.Items.Insert(0, new ListItem("All", ""));
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlADM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (ddlADM.SelectedItem.Value != "")
                {
                    if (Convert.ToString(ddlActivityBy.SelectedItem.Text) == "ADM")
                    {
                        ddlMA.Items.Clear();
                        ddlMA.Items.Insert(0, new ListItem("All", ""));
                        ddlMA.Enabled = false;
                        //Dropdownlist.DDLGetMA(ddlMA, Convert.ToInt64(ddlADM.SelectedItem.Value));

                    }
                    else
                    {
                        ddlMA.Enabled = true;
                        Dropdownlist.DDLGetMA(ddlMA, Convert.ToInt64(ddlADM.SelectedItem.Value));

                    }
                }
                else
                {
                    ddlMA.Items.Clear();
                    ddlMA.Items.Insert(0, new ListItem("All", ""));
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindDropdowns()
        {
            try
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlActivityBy, CommonLists.GetActivityBy(), (int)Constants.DropDownFirstOption.All);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}