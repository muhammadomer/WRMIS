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
    public partial class IrrigatorFeedback : BasePage
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
                    LoadSecondaryParameter();

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

            if (lstDivision.Count == 1)
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
            Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);
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
                if (ddlCircle.SelectedValue==string.Empty)
                {
                    Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlDivision, null, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    // Reset division dropdownlist
                    Dropdownlist.DDLDivisions(ddlDivision, true, -1, -1, (int)Constants.DropDownFirstOption.All);
                    // Reset sub division dropdownlist
                    

                    int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                    // Bind Division dropdownlist based on Circle 
                    Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID, (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);
                }
                

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
                if (ddlDivision.SelectedItem.Value == string.Empty)
                {
                    Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, null, (int)Constants.DropDownFirstOption.All);
                }
                else
                {
                    long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                    Dropdownlist.BindDropdownlist<List<CO_Channel>>(ddlChannel, GetChannelsByIrrigationBoundary(), (int)Constants.DropDownFirstOption.All);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        protected void btnIrrigatorFeedback_Click(object sender, EventArgs e)
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
        private ReportData GetReportData()
        {
            ReportData rptData = GetReportPrimaryParameters();
            rptData.Name = ReportConstants.rptIrrigatorFeedback;
            return rptData;
        }

        #region "Get Reports Paramert Methods"
        private ReportData GetReportPrimaryParameters()
        {
            string ZoneID = ddlZone.SelectedItem.Value == string.Empty ? "0" : ddlZone.SelectedItem.Value;
            string CircleID = ddlCircle.SelectedItem.Value == string.Empty ? "0" : ddlCircle.SelectedItem.Value;
            string DivisionID = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;
            string ChannelID = ddlChannel.SelectedItem.Value == string.Empty ? "0" : ddlChannel.SelectedItem.Value;
            string FromDate = string.IsNullOrEmpty(txtFromDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtFromDate.Text));
            string ToDate = string.IsNullOrEmpty(txtToDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtToDate.Text));
            string TailStatus = ddlTailStatus.SelectedItem.Value == string.Empty ? "0" : ddlTailStatus.SelectedItem.Value;
            string RotationalVoilation = ddlRotationalVoilation.SelectedItem.Value == string.Empty ? "-1" : ddlRotationalVoilation.SelectedItem.Value;
            string WaterTheft = ddlWaterTheft.SelectedItem.Value == string.Empty ? "-1" : ddlWaterTheft.SelectedItem.Value;

            ReportData rptData = new ReportData();
            ReportParameter reportParameter = new ReportParameter("ZoneID", ZoneID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("CircleID", CircleID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("DivisionID", DivisionID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ChaneelID", ChannelID);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("FromDate", FromDate);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("ToDate", ToDate);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("TailStatus", TailStatus);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("RotationalVoilation", RotationalVoilation);
            rptData.Parameters.Add(reportParameter);

            reportParameter = new ReportParameter("WaterTheft", WaterTheft);
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
            return new ChannelBLL().GetChannelsByIrrigationBoundary(ZoneID, CircleID, DivisionID, -1);
        }

        private void LoadSecondaryParameter()
        {
            Dropdownlist.BindDropdownlist<List<object>>(ddlTailStatus, CommonLists.GetTailStatus(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlRotationalVoilation, CommonLists.GetYesNo(), (int)Constants.DropDownFirstOption.All);
            Dropdownlist.BindDropdownlist<List<object>>(ddlWaterTheft, CommonLists.GetYesNo(), (int)Constants.DropDownFirstOption.All);
        }
        #endregion
    }
}