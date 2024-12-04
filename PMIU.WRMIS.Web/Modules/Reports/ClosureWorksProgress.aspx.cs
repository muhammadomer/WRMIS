using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.ClosureOperations;
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
    public partial class ClosureWorksProgress : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    if (userID > 0) // Irrigation Staff
                    {
                        LoadAllRegionDDByUser(userID, IrrigationLevelID);
                    }
                    else
                    {
                        BindDropdownlists();
                    }
                    List<object> lstYear = new List<object>();
                    lstYear = new ClosureOperationsBLL().GetOldestClosureWorkPlan();
                    Dropdownlist.DDLLoading(ddlClosurePeriod, false, (int)Constants.DropDownFirstOption.NoOption, lstYear);
                   // Dropdownlist.BindDropdownlist<List<dynamic>>(ddlClosurePeriod, CommonLists.GetClosurePeriod(), (int)Constants.DropDownFirstOption.NoOption);
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlClosureWorkType, CommonLists.GetClosureWorkType(), (int)Constants.DropDownFirstOption.NoOption);
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlWorkStatus, CommonLists.GetClosureWorkStatus(), (int)Constants.DropDownFirstOption.All);

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
        }
        private void DDLEmptyDivisionSubDivision()
        {
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, (int)Constants.DropDownFirstOption.All);
        }

        protected void rbReports_CheckedChanged(object sender, EventArgs e)
        {
            try
            {   
                iframestyle.Visible = false;
                HideReportParameters();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void HideReportParameters()
        {
            if (rbClosureWorkProgress.Checked)
            {
                this.RowParam1.Visible = true;                
                this.RowParam3.Visible = true;
                this.RowParam4.Visible = true;
            }
            else
            {
                this.RowParam1.Visible = false;                
                this.RowParam3.Visible = false;
                this.RowParam4.Visible = false;
            }
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
                // Bind Sub Division dropdownlist based on Division 
                long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region "Reports Paramert Methods"
        private ReportData GetReportParameters()
        {
            ReportData rptData = new ReportData();
            if (rbClosureWorkProgress.Checked)
            {
                string ZoneID = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
                string CircleID = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
                string DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;

                string ClosurePeriod = ddlClosurePeriod.SelectedItem.Text == string.Empty ? "0" : ddlClosurePeriod.SelectedItem.Text;
                string ClosureWorkType = ddlClosureWorkType.SelectedItem.Value == string.Empty ? "0" : ddlClosureWorkType.SelectedItem.Value;
                string WorkStatus = ddlWorkStatus.SelectedItem.Value == string.Empty ? "0" : ddlWorkStatus.SelectedItem.Value;

                
                ReportParameter reportParameter = new ReportParameter("ZoneID", ZoneID);
                rptData.Parameters.Add(reportParameter);

                reportParameter = new ReportParameter("CircleID", CircleID);
                rptData.Parameters.Add(reportParameter);

                reportParameter = new ReportParameter("DivisionID", DivisionID);
                rptData.Parameters.Add(reportParameter);


                reportParameter = new ReportParameter("ClosurePeriodID", ClosurePeriod);
                rptData.Parameters.Add(reportParameter);

                reportParameter = new ReportParameter("ClosureWorkType", ClosureWorkType);
                rptData.Parameters.Add(reportParameter);

                reportParameter = new ReportParameter("WorkStatus", WorkStatus);
                rptData.Parameters.Add(reportParameter);
            }
            else if (rbDesiltingProgress.Checked)
            {
                string ClosurePeriod = ddlClosurePeriod.SelectedItem.Text == string.Empty ? "0" : ddlClosurePeriod.SelectedItem.Text;
                ReportParameter reportParameter = new ReportParameter("ClosurePeriodID", ClosurePeriod);
                rptData.Parameters.Add(reportParameter);
            }
            
            return rptData;
        }
        private ReportData GetReportData()
        {
            ReportData rptData = null;
            rptData = GetReportParameters();
            rptData.Name = rbClosureWorkProgress.Checked ? ReportConstants.rptClosureWorksProgress : ReportConstants.rptCWDesiltingProgress;
            return rptData;
        }
        #endregion
        protected void btnViewReport_Click(object sender, EventArgs e)
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

    }
}