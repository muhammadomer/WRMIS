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
    public partial class WaterTheftReport : BasePage
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
                        Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);
                    }

                    LoadWaterTheftChannelsSecondaryParam();

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



            if (lstDivision.Count==1)
            {
                ddlDivision_SelectedIndexChanged(null, null);
            }
            else
            {
                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);
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
            //  Dropdownlist.DDLChannels(ddlChannel, false, -1, (int)Constants.DropDownFirstOption.All);
        }
        private void DDLEmptyDivisionSubDivision()
        {
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true, -1, (int)Constants.DropDownFirstOption.All);
            // Bind empty sub division dropdownlist
            /// Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);
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
                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);

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
                // Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

                int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                // Bind Division dropdownlist based on Circle 
                Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);
                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);
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
                //   Dropdownlist.DDLSubDivisions(ddlSubDivision, true, -1, (int)Constants.DropDownFirstOption.All);

                // Bind Sub Division dropdownlist based on Division 
                long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                List<CO_Channel> lstChannel = GetChannelsByIrrigationBoundary();
                Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, lstChannel, (int)Constants.DropDownFirstOption.All);
                if (lstChannel.Count==1)
                {
                    Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);    
                }
                
            

                //  Dropdownlist.DDLSubDivisions(ddlSubDivision, false, divisionID, (int)Constants.DropDownFirstOption.All);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        protected void btnWaterTheftReport_Click(object sender, EventArgs e)
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
                case (int)ReportConstants.WaterTheftReports.WaterTheftChannels:
                    rptData = GetWaterTheftChannelsParams();
                    break;
                case (int)ReportConstants.WaterTheftReports.WaterTheftOutlets:
                    rptData = GetWaterTheftOutletsParams();
                    break;
                case (int)ReportConstants.WaterTheftReports.Breach:
                    rptData = GetBreachParams();
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
            string ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? "0" : ddlChannel.SelectedItem.Value;
            string FromDate = string.IsNullOrEmpty(txtFromDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtFromDate.Text));
            string ToDate = string.IsNullOrEmpty(txtToDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtToDate.Text));

            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("ZoneID", ZoneID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("CircleID", CircleID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("DivisionID", DivisionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ChannelID", ChannelID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("FromDate", FromDate);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ToDate", ToDate);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
        private ReportData GetWaterTheftParams()
        {
            string OffenceTypeID = ddlOffenceType.SelectedItem.Value == string.Empty ? "0" : ddlOffenceType.SelectedItem.Value;
            string Status = ddlStatus.SelectedItem.Value == string.Empty ? "0" : ddlStatus.SelectedItem.Value;
            string AssignedTo = ddlAssignedTo.SelectedItem.Value == string.Empty ? "0" : ddlAssignedTo.SelectedItem.Value;
            string ReportedBy = ddlReportedBy.SelectedItem.Value == string.Empty ? "0" : ddlReportedBy.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();

            ReportParameter reportParameter = new ReportParameter("OffenceTypeID", OffenceTypeID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("StatusID", Status);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("AssignedTo", AssignedTo);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ReportedBy", ReportedBy);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetWaterTheftChannelsParams()
        {
            ReportData rptData = GetWaterTheftParams();
            rptData.Name = ReportConstants.rptWaterTheftChannel;

            return rptData;
        }
        private ReportData GetWaterTheftOutletsParams()
        {
            string OutletID = ddlOutlet.SelectedItem.Value == string.Empty ? "0" : ddlOutlet.SelectedItem.Value;
            string OutletConditionID = ddlOutletCondition.SelectedItem.Value == string.Empty ? "0" : ddlOutletCondition.SelectedItem.Value;

            ReportData rptData = GetWaterTheftParams();
            rptData.Name = ReportConstants.rptWaterTheftOutlet;

            ReportParameter reportParameter = new ReportParameter("OutLetID", OutletID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("OutLetConditionID", OutletConditionID);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetBreachParams()
        {
            string BreachLength = ddlBreachLength.SelectedItem.Value == string.Empty ? "0" : ddlBreachLength.SelectedItem.Value;
            string ReportedBy = ddlReportedBy.SelectedItem.Value == string.Empty ? "0" : ddlReportedBy.SelectedItem.Value;

            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptBreach;

            ReportParameter reportParameter = new ReportParameter("BreachLength", BreachLength);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ReportedBy", ReportedBy);
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
            if (ZoneID == -1 && DivisionID == -1 && CircleID == -1)
            {
                List<CO_Channel> lst = null;
                return lst;
            }
            List <CO_Channel> lstData = new ChannelBLL().GetChannelsByIrrigationBoundary(ZoneID, CircleID, DivisionID, -1);
            return lstData;
        }
        private void LoadWaterTheftChannelsSecondaryParam()
        {
            LoadWaterTheftParameter();
            SPWaterTheft.Visible = true;
        }
        private void LoadWaterTheftOutletsSecondaryParam()
        {
            long ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlChannel.SelectedItem.Value);
            LoadWaterTheftParameter();
            Dropdownlist.BindDropdownlist<List<object>>(ddlOutlet, new WaterTheftBLL().GetOutletByChannelId(ChannelID, 0, 0), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlOutletCondition, new WaterTheftBLL().GetOutletCondition("o"), (int)Constants.DropDownFirstOption.All); // O => Offence Type
            Dropdownlist.BindDropdownlist<List<object>>(ddlOffenceType, new WaterTheftBLL().GetAllTheftType("o"), (int)Constants.DropDownFirstOption.All); // O => Offence Type
            SPWaterTheftOutlet.Visible = true;
            SPWaterTheft.Visible = true;
        }
        private void LoadWaterTheftParameter()
        {
            Dropdownlist.DDLWaterTheftStatus(ddlStatus, (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlReportedBy, CommonLists.GetWaterTheftReportedBy(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlAssignedTo, CommonLists.GetWaterTheftAssignedTo(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlOffenceType, new WaterTheftBLL().GetAllTheftType("c"), (int)Constants.DropDownFirstOption.All); // O => Offence Type
        }
        private void LoadBreachSecondaryParam()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlBreachLength, CommonLists.GetBreachLength(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlReportedBy, CommonLists.GetWaterTheftReportedBy(), (int)Constants.DropDownFirstOption.All);
            SPBreach.Visible = true;
        }
        #endregion
        private void HideSecondaryParameters()
        {
            SPWaterTheft.Visible = false;
            SPWaterTheftOutlet.Visible = false;
            SPBreach.Visible = false;
        }
        private int GetReportID()
        {
            int reportID = 0;
            if (rbWaterTheftChannels.Checked)
                reportID = (int)ReportConstants.WaterTheftReports.WaterTheftChannels;
            else if (rbWaterTheftOutlets.Checked)
                reportID = (int)ReportConstants.WaterTheftReports.WaterTheftOutlets;
            else if (rbBreach.Checked)
                reportID = (int)ReportConstants.WaterTheftReports.Breach;
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
                    case (int)ReportConstants.WaterTheftReports.WaterTheftChannels:
                        LoadWaterTheftChannelsSecondaryParam();
                        break;
                    case (int)ReportConstants.WaterTheftReports.WaterTheftOutlets:
                        LoadWaterTheftOutletsSecondaryParam();
                        break;
                    case (int)ReportConstants.WaterTheftReports.Breach:
                        LoadBreachSecondaryParam();
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

        protected void ddlChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlChannel.SelectedItem.Value);
                if (rbWaterTheftOutlets.Checked)
                    Dropdownlist.BindDropdownlist<List<object>>(ddlOutlet, new WaterTheftBLL().GetOutletByChannelId(ChannelID, 0, 0), (int)Constants.DropDownFirstOption.All);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}