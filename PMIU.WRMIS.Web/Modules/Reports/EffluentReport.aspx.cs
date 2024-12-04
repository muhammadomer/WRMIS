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
    public partial class EffluentReport : BasePage
    {
        #region Initialize
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

                    //if (userID > 0) // Irrigation Staff
                    //{
                    //    LoadAllRegionDDByUser(userID, IrrigationLevelID);
                    //}
                    //else
                    //{
                        BindDropdownlists();
                    //}
                    LoadIndustryReports();


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
        #region "Dropdownlists Events
        private void BindDropdownlists()
        {
            try
            {
                Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.All);
                DDLEmptyCircleDivision();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void DDLEmptyCircleDivision()
        {
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircles(ddlCircle, true, -1, (int)Constants.DropDownFirstOption.All);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);

        }
        private void DDLEmptyDivision()
        {
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);

        }

        #region DropDownIndexChanged
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                iframestyle.Visible = false;
                // Reset Circle,Division, Sub Division dropdownlists
                DDLEmptyCircleDivision();
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

                int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                // Bind Division dropdownlist based on Circle 
                Dropdownlist.DDLDivisions(ddlDivision, false, circleID, -1, (int)Constants.DropDownFirstOption.All);
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

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #endregion

        #region RadioBtn
        protected void rbIndustryReports_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIndustryReports.Checked)
            {
                LoadIndustryReports();
            }
        }

        protected void rbRecoveryOfAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRecoveryOfAccount.Checked)
            {
                LoadRecoveryOfAccount();
            }
        }

        protected void rbPaymentReport_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPaymentReport.Checked)
            {
                LoadPaymentReport();
            }
        }
        #endregion

        #endregion

        #region Functions
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

        private void LoadIndustryReports()
        {
            Dropdownlist.DDLEffluentSreviceType(ddlServiceTypeIndustry, null, (int)Constants.DropDownFirstOption.All);
            RecoveryOfAccountData.Visible = false;
            PaymentReport.Visible = false;
            IndustryData.Visible = true;

        }

        private void LoadRecoveryOfAccount()
        {
            //ServiceType
            Dropdownlist.DDLEffluentSreviceType(ddlServiceTypeRecovery, null, (int)Constants.DropDownFirstOption.Select);
            //Financial Year
            Dropdownlist.DDLFinancialYear(ddlFinancialYear, null, false);
            ddlFinancialYear.SelectedIndex = 1;
            IndustryData.Visible = false;
            PaymentReport.Visible = false;
            RecoveryOfAccountData.Visible = true;

        }

        private void LoadPaymentReport()
        {
            Dropdownlist.DDLEffluentSreviceType(ddlPaymentReportServiceType, null, (int)Constants.DropDownFirstOption.All);
            txtFromDate.Text = String.Format("{0:dd-MMM-yyyy}", DateTime.Now);
            txtToDate.Text = String.Format("{0:dd-MMM-yyyy}", DateTime.Now);
            RecoveryOfAccountData.Visible = false;
            IndustryData.Visible = false;
            PaymentReport.Visible = true;
        }
       
        #endregion

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string ZoneID = ddlZone.SelectedValue == String.Empty ? "0" : ddlZone.SelectedItem.Value;
                string CircleID = ddlCircle.SelectedValue == String.Empty ? "0" : ddlCircle.SelectedItem.Value;
                string DivisionID = ddlDivision.SelectedValue == String.Empty ? "0" : ddlDivision.SelectedItem.Value;
                string ServiceType= "";
                if (rbIndustryReports.Checked)
                {
                    ServiceType = "";
                    ServiceType= ddlServiceTypeIndustry.SelectedIndex == 0 ? "0" : ddlServiceTypeIndustry.SelectedItem.Value;
                }

                string FromDate = "";
                string ToDate = "";
                if (rbPaymentReport.Checked)
                {
                    ServiceType = "";
                    FromDate = "";
                    ToDate = "";
                    ServiceType = ddlPaymentReportServiceType.SelectedIndex == 0 ? "0" : ddlPaymentReportServiceType.SelectedItem.Value;
                    FromDate = txtFromDate.Text;
                    ToDate = txtToDate.Text;
                }
                
                string ServiceTypeRecovery = "";
                string FinancialYear = "";
                if (rbRecoveryOfAccount.Checked)
                {
                    ServiceTypeRecovery = ddlServiceTypeRecovery.SelectedIndex == 0 ? "0" : ddlServiceTypeRecovery.SelectedItem.Value;
                    FinancialYear = ddlFinancialYear.SelectedItem.Value;
                }


                ReportData rptData = new ReportData();
                ReportParameter reportParameter = new ReportParameter("ZoneID", ZoneID);
                rptData.Parameters.Add(reportParameter);

                reportParameter = new ReportParameter("CircleID", CircleID);
                rptData.Parameters.Add(reportParameter);

                reportParameter = new ReportParameter("DivisionID", DivisionID);
                rptData.Parameters.Add(reportParameter);


                if (rbIndustryReports.Checked)
                {
                    reportParameter = new ReportParameter("ServiceTypeID", ServiceType);
                    rptData.Parameters.Add(reportParameter);

                    rptData.Name = ReportConstants.rptECIndustries;
                }

                if (rbPaymentReport.Checked)
                {
                    reportParameter = new ReportParameter("ServiceTypeID", ServiceType);
                    rptData.Parameters.Add(reportParameter);
                    
                    reportParameter = new ReportParameter("FromDate", FromDate);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("ToDate", ToDate);
                    rptData.Parameters.Add(reportParameter);

                    rptData.Name = ReportConstants.rptECPaymentsReport;
                }

                if (rbRecoveryOfAccount.Checked)
                {
                    reportParameter = new ReportParameter("ServiceTypeID", ServiceTypeRecovery);
                    rptData.Parameters.Add(reportParameter);

                    reportParameter = new ReportParameter("Year", FinancialYear);
                    rptData.Parameters.Add(reportParameter);
                    
                    rptData.Name = ReportConstants.rptECRecoveryOfAccount;
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
    }
}