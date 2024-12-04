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
    public partial class ComplaintReport : BasePage
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
                iframestyle.Visible = false;
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
        private ReportData GetReportData(Int32 _ReportID)
        {
            ReportData rptData = null;
            switch (_ReportID)
            {
                case (int)ReportConstants.ComplaintReports.ComplaintSource:
                    rptData = GetComplaintSourceParams();
                    break;
                case (int)ReportConstants.ComplaintReports.ComplaintStatus:
                    rptData = GetComplaintStatusParams();
                    break;
                case (int)ReportConstants.ComplaintReports.ComplaintType:
                    rptData = GetComplaintTypeParams();
                    break;
                case (int)ReportConstants.ComplaintReports.Directives:
                    rptData = GetDirectiveParams();
                    break;
                case (int)ReportConstants.ComplaintReports.Structure:
                    rptData = GetStructureParams();
                    break;
                case (int)ReportConstants.ComplaintReports.ComplaintDetails:
                    rptData = GetComplaintDetailParams();
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
            string FromDate = string.IsNullOrEmpty(txtFromDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtFromDate.Text));
            string ToDate = string.IsNullOrEmpty(txtToDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtToDate.Text));

            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("ZoneID", ZoneID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("CircleID", CircleID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("DivisionID", DivisionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("FromDate", FromDate);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ToDate", ToDate);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
        private ReportData GetComplaintSourceParams()
        {
            string Overall = "0"; // = chkOverall.Checked == true ? "1" : "0";
            string Farmers = chkFrames.Checked == true ? "1" : "0";
            string PMIU = chkPMIU.Checked == true ? "1" : "0";
            string Directives = chkDirectives.Checked == true ? "1" : "0";

            if (Farmers == "1" && PMIU == "1" && Directives == "1")
                Overall = "1";


            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptComplaintSource;

            ReportParameter reportParameterOverall = new ReportParameter("SourceTypeOverall", Overall);
            rptData.Parameters.Add(reportParameterOverall);
            ReportParameter reportParameterFarmers = new ReportParameter("SourceTypeFarmer", Farmers);
            rptData.Parameters.Add(reportParameterFarmers);
            ReportParameter reportParameterPMIU = new ReportParameter("SourceTypePMIU", PMIU);
            rptData.Parameters.Add(reportParameterPMIU);
            ReportParameter reportParameterDirectives = new ReportParameter("SourceTypeDirectives", Directives);
            rptData.Parameters.Add(reportParameterDirectives);
            return rptData;
        }
        private ReportData GetComplaintTypeParams()
        {
            string ComplaintTypeID = ddlComplaintType.SelectedItem.Value == string.Empty ? "0" : ddlComplaintType.SelectedItem.Value;
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptComplaintType;
            ReportParameter reportParameter = new ReportParameter("ComplaintTypeID", ComplaintTypeID);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
        private ReportData GetComplaintStatusParams()
        {
            string ComplaintStatus = ddlComplaintStatus.SelectedItem.Value == string.Empty ? "0" : ddlComplaintStatus.SelectedItem.Value;
            string AssignedTo = ddlAssignedTo.SelectedItem.Value == string.Empty ? "0" : ddlAssignedTo.SelectedItem.Value;
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptComplaintStatus;
            ReportParameter reportParameter = new ReportParameter("ComplaintStatus", ComplaintStatus);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("AssignedTo", AssignedTo);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
        private ReportData GetDirectiveParams()
        {
            string DirectiveTypes = ddlDirectiveTypes.SelectedItem.Value == string.Empty ? "0" : ddlDirectiveTypes.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptComplaintDirective;
            ReportParameter reportParameter = new ReportParameter("ComplaintSource", DirectiveTypes);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetStructureParams()
        {

            ////string BreachLength = ddlBreachLength.SelectedItem.Value == string.Empty ? "0" : ddlBreachLength.SelectedItem.Value;
            ////string ReportedBy = ddlReportedBy.SelectedItem.Value == string.Empty ? "0" : ddlReportedBy.SelectedItem.Value;

            //ReportData rptData = GetReportPrimaryParameters();
            //rptData.Name = ReportConstants.rptComplaintStructure;

            ////ReportParameter reportParameter = new ReportParameter("BreachLength", BreachLength);
            ////rptData.Parameters.Add(reportParameter);

            ////reportParameter = new ReportParameter("ReportedBy", ReportedBy);
            ////rptData.Parameters.Add(reportParameter);

            //return rptData;

            string StructerType = ddlStructure.SelectedItem.Value == string.Empty ? "0" : ddlStructure.SelectedItem.Value;
            ReportData rptData = GetReportPrimaryParameters();
            ReportParameter reportParameter;
            string StructureID = ddlSecond.SelectedItem == null ? "0" : ddlSecond.SelectedItem.Value == string.Empty ? "0" : ddlSecond.SelectedItem.Value;
            reportParameter = new ReportParameter("StructureID", StructureID);
            rptData.Parameters.Add(reportParameter);
            string channelID = ddlFirst.SelectedItem == null ? "0" : ddlFirst.SelectedItem.Value == string.Empty ? "0" : ddlFirst.SelectedItem.Value;
            rptData.Name = ReportConstants.rptComplaintStructure;
            reportParameter = new ReportParameter("StructureTypeID", StructerType);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ChannelID", channelID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetComplaintDetailParams()
        {
            string ComplaintType = ddlComplaintType.SelectedItem.Value == string.Empty ? "0" : ddlComplaintType.SelectedItem.Value;
            string ComplaintSource = ddlComplaintSource.SelectedItem.Value == string.Empty ? "0" : ddlComplaintSource.SelectedItem.Value;
            string ComplaintStatus = ddlComplaintStatus.SelectedItem.Value == string.Empty ? "0" : ddlComplaintStatus.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptComplaintDetail;

            ReportParameter reportParameter = new ReportParameter("ComplaintType", ComplaintType);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ComplaintSource", ComplaintSource);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ComplaintStatus", ComplaintStatus);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        #endregion

        #region "Load Reports Secondary Parameters"
        private void LoadComplaintSourceParameter()
        {
            SPComplaintSource.Visible = true;
        }
        private void LoadComplaintTypeParameter()
        {
            Dropdownlist.DDLComplaintType(ddlComplaintType, (int)Constants.DropDownFirstOption.All);
            SPComplaintType.Visible = true;
        }
        private void LoadComplaintStatusParameter()
        {
            Dropdownlist.DDLComplaintStatus(ddlComplaintStatus, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlAssignedTo, CommonLists.GetComplaintAssignedTo(), (int)Constants.DropDownFirstOption.All);
            SPComplaintStatus.Visible = true;
            SPAssignedTo.Visible = true;
        }
        private void LoadDirectivesParameter()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlDirectiveTypes, CommonLists.GetDirectives(), (int)Constants.DropDownFirstOption.All);
            SPDirective.Visible = true;
        }
        private void LoadStructureParameter()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlStructure, CommonLists.GetStructs(), (int)Constants.DropDownFirstOption.Select);
            SPStructure.Visible = true;
            divFirst.Visible = false;
            divSecond.Visible = false;
        }
        private void LoadComplaintDetailsParameter()
        {
            Dropdownlist.DDLComplaintType(ddlComplaintType, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.DDLComplaintStatus(ddlComplaintStatus, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlComplaintSource, CommonLists.GetComplaintSource(), (int)Constants.DropDownFirstOption.All);
            SPComplaintDetails.Visible = true;
            SPComplaintType.Visible = true;
            SPComplaintStatus.Visible = true;
        }
        #endregion
        private void HideSecondaryParameters()
        {
            SPComplaintSource.Visible = false;
            SPComplaintType.Visible = false;
            SPComplaintStatus.Visible = false;
            divFirst.Visible = false;
            divSecond.Visible = false;
            SPDirective.Visible = false;
            SPStructure.Visible = false;
            SPComplaintDetails.Visible = false;
            SPAssignedTo.Visible = false;
            iframestyle.Visible = false;

        }
        private int GetReportID()
        {
            int reportID = 0;
            if (rbComplaintSource.Checked)
                reportID = (int)ReportConstants.ComplaintReports.ComplaintSource;
            else if (rbComplaintType.Checked)
                reportID = (int)ReportConstants.ComplaintReports.ComplaintType;
            else if (rbComplaintStatus.Checked)
                reportID = (int)ReportConstants.ComplaintReports.ComplaintStatus;
            else if (rbDirectives.Checked)
                reportID = (int)ReportConstants.ComplaintReports.Directives;
            else if (rbStructure.Checked)
                reportID = (int)ReportConstants.ComplaintReports.Structure;
            else if (rbComplaintDetails.Checked)
                reportID = (int)ReportConstants.ComplaintReports.ComplaintDetails;
            return reportID;

        }

        protected void ddlFirst_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFirst.SelectedItem.Value!=string.Empty)
            {
                if (ddlStructure.SelectedValue == "2")
                {
                    Dropdownlist.BindDropdownlist<List<object>>(ddlSecond, new ScheduleInspectionBLL().GetOutletsAgainstChannel(Convert.ToInt64(ddlFirst.SelectedValue)), (int)Constants.DropDownFirstOption.All);
                }    
            }
            else
            {
                Dropdownlist.BindDropdownlist<List<object>>(ddlSecond, null, (int)Constants.DropDownFirstOption.All);
            }
            
        }


        

        protected void ddlStructure_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Structure = ddlStructure.SelectedItem.Text == string.Empty ? "" : ddlStructure.SelectedItem.Text;
            switch (Structure)
            {
                case "Select":
                    divFirst.Visible = false;
                    divSecond.Visible = false;
                    break;
                case "Channels":
                    //Dropdownlist.BindDropdownlist<List<object>>(ddlFirst, CommonLists.GetStructsChannels(), (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.DDLChannels(ddlFirst, false, (int)Constants.DropDownFirstOption.All);
                //Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlFirst, SessionManagerFacade.UserAssociatedLocations.UserID,SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, Convert.ToInt64(ddlDivision.SelectedValue));
                    lblFirst.InnerText = "Channels";
                    divFirst.Visible = true;
                    divSecond.Visible = false;
                    break;
                case "Outlet":
                    Dropdownlist.DDLChannels(ddlFirst, false, (int)Constants.DropDownFirstOption.All);
                    //Dropdownlist.DDLChannelsByUserIDAndDivisionID(ddlFirst, SessionManagerFacade.UserAssociatedLocations.UserID, SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID, Convert.ToInt64(ddlDivision.SelectedValue));
                    Dropdownlist.BindDropdownlist<List<object>>(ddlSecond,null, (int)Constants.DropDownFirstOption.All);
                    divFirst.Visible = true;
                    divSecond.Visible = true;
                    break;
                case "Barrage/Headwork":
                    Dropdownlist.BindDropdownlist<List<object>>(ddlFirst, CommonLists.GetStructsHeadWorks(), (int)Constants.DropDownFirstOption.All);
                    lblFirst.InnerText = "Barrage/Headwork";
                    divFirst.Visible = true;
                    divSecond.Visible = false;
                    break;
                case "Protection Infrastructure":
                    Dropdownlist.BindDropdownlist<List<object>>(ddlFirst, CommonLists.GetStructsProtectionInfrastructure(), (int)Constants.DropDownFirstOption.All);
                    lblFirst.InnerText = "Protection Infrastructure";
                    divFirst.Visible = true;
                    divSecond.Visible = false;
                    break;
                case "Drain":
                    Dropdownlist.BindDropdownlist<List<object>>(ddlFirst, CommonLists.GetStructsDrain(), (int)Constants.DropDownFirstOption.All);
                    lblFirst.InnerText = "Drain";
                    divFirst.Visible = true;
                    divSecond.Visible = false;
                    break;
                default:
                    break;
            }
        }
        protected void rbReports_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HideSecondaryParameters();
                iframestyle.Visible = false;
                int reportID = GetReportID();
               // btnComplaintViewReport.Visible = false;
                switch (reportID)
                {
                    case (int)ReportConstants.ComplaintReports.ComplaintSource:
                        LoadComplaintSourceParameter();
                        break;
                    case (int)ReportConstants.ComplaintReports.ComplaintType:
                        LoadComplaintTypeParameter();
                        break;
                    case (int)ReportConstants.ComplaintReports.ComplaintStatus:
                        LoadComplaintStatusParameter();
                        break;
                    case (int)ReportConstants.ComplaintReports.Directives:
                        LoadDirectivesParameter();
                        break;
                    case (int)ReportConstants.ComplaintReports.Structure:
                        LoadStructureParameter();
                        break;
                    case (int)ReportConstants.ComplaintReports.ComplaintDetails:
                        LoadComplaintDetailsParameter();
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

       

        

    }
}