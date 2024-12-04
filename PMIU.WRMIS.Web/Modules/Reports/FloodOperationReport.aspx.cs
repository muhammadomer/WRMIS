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
    public partial class FloodOperationReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                    Dropdownlist.BindDropdownlist<List<object>>(ddlInfrastructureType, CommonLists.InfrastructureTypes(), (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.DDLYear(ddlYear, false, 0, 2015, (int)Constants.DropDownFirstOption.Select);
                    InfrastructureTypeInfrastructureName(true);
                    if (userID > 0) // Irrigation Staff
                    {
                        LoadAllRegionDDByUser(userID, IrrigationLevelID);
                    }
                    else
                    {
                        BindDropdownlists();
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


        private ReportData GetSecondaryParameter(ReportData rptData)
        {
            string InfrastructureTypeID= ddlInfrastructureType.SelectedItem.Value == string.Empty ? "0" : ddlInfrastructureType.SelectedItem.Value;
            string InfrastructureNameID= ddlInfrastructureName.SelectedItem.Value == string.Empty ? "0" : ddlInfrastructureName.SelectedItem.Value;
            

            
            ReportParameter reportParameter = new ReportParameter("StructureTypeID", InfrastructureTypeID);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("StructureID", InfrastructureNameID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetPrimaryParameter()
        {
            string division = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;
            string zone = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
            string circle = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
            string year = ddlYear.SelectedItem.Value == string.Empty ? "0" : ddlYear.SelectedItem.Value;


            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("DivisionID", division);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ZoneID", zone);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("CircleID", circle);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("Year", year);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
        private ReportData GetReportData(Int32 _ReportID)
        {
            ReportData rptData = GetPrimaryParameter();
            switch (_ReportID)
            {
                case (int)ReportConstants.FloodOperationReports.StoneInformation:
                    rptData = GetSecondaryParameter(rptData);
                    rptData.Name = ReportConstants.rptStoneInformation;
                    break;
                case (int)ReportConstants.FloodOperationReports.MachineryEquipment:  
                    rptData.Name = ReportConstants.rptFloodFightingPlanPrintable;   // TODO  set exact name of report 
                    break;
                case (int)ReportConstants.FloodOperationReports.EmergencyPurchasesofItems:
                    rptData = GetSecondaryParameter(rptData);
                    rptData.Name = ReportConstants.rptEmergencyPurchases;
                    break;
                case (int)ReportConstants.FloodOperationReports.EmergencyWorks:
                      rptData = GetSecondaryParameter(rptData);  // TODO  set exact name of report 
                      rptData.Name = ReportConstants.rptEmergencyWorks;
                    break;
                case (int)ReportConstants.FloodOperationReports.OnsiteStoneMonitoring:
                    rptData = GetSecondaryParameter(rptData);
                    rptData.Name = ReportConstants.rptOnsiteStoneMonitoring;
                    break;
                case (int)ReportConstants.FloodOperationReports.OnsiteCampSiteMonitoring:
                    rptData.Name = ReportConstants.rptOnsiteCampSiteMonitoring;
                    break;
                case (int)ReportConstants.FloodOperationReports.FloodInspections:
                      rptData = GetSecondaryParameter(rptData);
                      rptData.Name = ReportConstants.rptFloodInspection;
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
            string Year = ddlYear.SelectedItem.Value == string.Empty ? "0" : ddlYear.SelectedItem.Value;
          
            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("ZoneID", ZoneID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("CircleID", CircleID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("DivisionID", DivisionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("Year",Year);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
       
        #endregion

        #region "Load Reports Secondary Parameters"
        private void InfrastructureTypeInfrastructureName(bool show)
        {
            SPInfrastructureType.Visible = show;
            SPInfrastructureName.Visible = show;
        }
        #endregion
        private void HideSecondaryParameters()
        {
            InfrastructureTypeInfrastructureName(false);
            iframestyle.Visible = false;
        }
        private int GetReportID()
        {
            int reportID = 0;
            if (rbStoneInformation.Checked)
                reportID = (int)ReportConstants.FloodOperationReports.StoneInformation;
            //else if (rbMachineryEquipment.Checked)
            //    reportID = (int)ReportConstants.FloodOperationReports.MachineryEquipment;
            else if (rbEmergencyPurchasesofItems.Checked)
                reportID = (int)ReportConstants.FloodOperationReports.EmergencyPurchasesofItems;
            else if (rbEmergencyWorks.Checked)
                reportID = (int)ReportConstants.FloodOperationReports.EmergencyWorks;
            else if (rbOnsiteStoneMonitoring.Checked)
                reportID = (int)ReportConstants.FloodOperationReports.OnsiteStoneMonitoring;
            else if (rbOnsiteCampSiteMonitoring.Checked)
                reportID = (int)ReportConstants.FloodOperationReports.OnsiteCampSiteMonitoring;
            else if (rbFloodInspections.Checked)
                reportID = (int)ReportConstants.FloodOperationReports.FloodInspections;
            return reportID;

        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HideSecondaryParameters();
                iframestyle.Visible = false;
                int reportID = GetReportID();
                switch (reportID)
                {
                    case (int)ReportConstants.FloodOperationReports.StoneInformation:
                        InfrastructureTypeInfrastructureName(true);
                        break;
                    case (int)ReportConstants.FloodOperationReports.MachineryEquipment:
                        InfrastructureTypeInfrastructureName(false);
                        break;
                    case (int)ReportConstants.FloodOperationReports.EmergencyPurchasesofItems:
                        InfrastructureTypeInfrastructureName(true);
                        break;
                    case (int)ReportConstants.FloodOperationReports.OnsiteStoneMonitoring:
                        InfrastructureTypeInfrastructureName(true);
                        break;
                    case (int)ReportConstants.FloodOperationReports.OnsiteCampSiteMonitoring:
                        InfrastructureTypeInfrastructureName(false);
                        break;
                    case (int)ReportConstants.FloodOperationReports.EmergencyWorks:
                        InfrastructureTypeInfrastructureName(true);
                        break;
                    case (int)ReportConstants.FloodOperationReports.FloodInspections:
                        InfrastructureTypeInfrastructureName(true);
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
                Session[SessionValues.ReportData] = GetReportData(GetReportID());
                iframestyle.Visible = true;
                iframestyle.Src = Constants.ReportsUrl;
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlInfrastructureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                ddlInfrastructureName.Enabled = true;
                UA_Users _Users = SessionManagerFacade.UserInformation;

                if (ddlInfrastructureType.SelectedItem.Value != "")
                {
                    string InfrastructureTypeSelectedValue = ddlInfrastructureType.SelectedItem.Value;
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructureName, _Users.ID, Int64.Parse(InfrastructureTypeSelectedValue), (int)Constants.DropDownFirstOption.All);                    
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        

    }
}