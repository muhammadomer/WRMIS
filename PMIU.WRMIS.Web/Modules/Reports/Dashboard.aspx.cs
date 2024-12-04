
using PMIU.WRMIS.BLL.Reports;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;

namespace PMIU.WRMIS.Web.Modules.Reports
{
    public partial class Dashboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    long userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    long? IrrigationLevelID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

                    string CurrentDate = Utility.GetFormattedDate(DateTime.Now);
                    string FromDate = Utility.GetFormattedDate(DateTime.Now.AddDays(-30));


                    txtDateTSFS.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));
                    txtDateTSPS.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));

                    txtDateCSFrom.Text = FromDate;
                    txtDateCSTo.Text = CurrentDate;


                    txtDateWTCFrom.Text = FromDate;
                    txtDateWTCTo.Text = CurrentDate;

                    Dropdownlist.BindDropdownlist<List<object>>(ddlPE, new ReportsBLL().GetPerformanceEvaluationPeriod(), (int)Constants.DropDownFirstOption.NoOption);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlSession, CommonLists.GetPerformanceEvaluationSession(), (int)Constants.DropDownFirstOption.NoOption);


                    if (userID > 0) // Irrigation Staff
                    {
                        LoadAllRegionDDByUser(userID, IrrigationLevelID);
                    }
                    else
                    {
                        BindDropdownlists();
                    }

                    LoadTailStatusLatestReadingDate();
                }

                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void LoadTailStatusLatestReadingDate()
        {
            string CurrentDate = Utility.GetFormattedDate(DateTime.Now);
            long zoneID = ddlZone.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlZone.SelectedItem.Value);
            long circleID = ddlCircle.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlCircle.SelectedItem.Value);
            long divisionID = ddlDivision.SelectedItem.Value == string.Empty ? 0 : Convert.ToInt64(ddlDivision.SelectedItem.Value);
            
            //DateTime? TSFSDate = new ReportsBLL().GetTailStatuFieldLatestReadingDate(zoneID, circleID, divisionID);
            //DateTime? TSPSDate = new ReportsBLL().GetTailStatusPMIULatestReadingDate(zoneID, circleID, divisionID);
            //txtDateTSFS.Text = TSFSDate.HasValue ? Utility.GetFormattedDate(TSFSDate.Value) : CurrentDate;
            //txtDateTSPS.Text = TSPSDate.HasValue ? Utility.GetFormattedDate(TSPSDate.Value) : CurrentDate;
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
        #endregion

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static Dictionary<string, List<object>> GetReportsDashboard(string _ZoneID, string _CircleID, string _DivisionID, string _ToDate)
        //{
        //    DateTime? ToDate = null;
        //    Dictionary<string, List<object>> dictDashboard = null;

        //    if (!string.IsNullOrEmpty(_ToDate))
        //        ToDate = Utility.GetParsedDate(_ToDate);
        //    try
        //    {
        //        dictDashboard = new ReportsBLL().GetReportsDashboard(Convert.ToInt64(_ZoneID), Convert.ToInt64(_CircleID), Convert.ToInt64(_DivisionID), ToDate);
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException(SessionManagerFacade.UserInformation.ID, ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //    return dictDashboard;
        //}


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetTailStatusPMIUStaff(string _ZoneID, string _CircleID, string _DivisionID, string _ToDate)
        {
            DateTime? ToDate = null;
            List<object> lstTailStatusPMIUStaff = null;

            if (!string.IsNullOrEmpty(_ToDate))
                ToDate = Utility.GetParsedDate(_ToDate);
            try
            {
                lstTailStatusPMIUStaff = new ReportsBLL().GetTailStatusPMIUStaff(Convert.ToInt64(_ZoneID), Convert.ToInt64(_CircleID), Convert.ToInt64(_DivisionID), ToDate);
            }
            catch (Exception ex)
            {
                new WRException(SessionManagerFacade.UserInformation.ID, ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTailStatusPMIUStaff;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetTailStatusFieldStaff(string _ZoneID, string _CircleID, string _DivisionID, string _ToDate)
        {
            DateTime? ToDate = null;
            List<object> lstTailStatusFieldStaff = null;

            if (!string.IsNullOrEmpty(_ToDate))
                ToDate = Utility.GetParsedDate(_ToDate);
            try
            {
                lstTailStatusFieldStaff = new ReportsBLL().GetTailStatusFieldStaff(Convert.ToInt64(_ZoneID), Convert.ToInt64(_CircleID), Convert.ToInt64(_DivisionID), ToDate);
            }
            catch (Exception ex)
            {
                new WRException(SessionManagerFacade.UserInformation.ID, ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTailStatusFieldStaff;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetComplaintStatus(string _ZoneID, string _CircleID, string _DivisionID, string _FromDate, string _ToDate)
        {
            DateTime? ToDate = null;
            DateTime? FromDate = null;
            List<object> lstTailStatusFieldStaff = null;

            if (!string.IsNullOrEmpty(_ToDate))
                ToDate = Utility.GetParsedDate(_ToDate);
            if (!string.IsNullOrEmpty(_FromDate))
                FromDate = Utility.GetParsedDate(_FromDate);
            try
            {
                lstTailStatusFieldStaff = new ReportsBLL().GetComplaintStatus(Convert.ToInt64(_ZoneID), Convert.ToInt64(_CircleID), Convert.ToInt64(_DivisionID), FromDate, ToDate);
            }
            catch (Exception ex)
            {
                new WRException(SessionManagerFacade.UserInformation.ID, ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTailStatusFieldStaff;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetWaterTheftStatuse(string _ZoneID, string _CircleID, string _DivisionID, string _FromDate, string _ToDate)
        {
            DateTime? ToDate = null;
            DateTime? FromDate = null;
            List<object> lstWaterTheftStatuse = null;

            if (!string.IsNullOrEmpty(_ToDate))
                ToDate = Utility.GetParsedDate(_ToDate);
            if (!string.IsNullOrEmpty(_FromDate))
                FromDate = Utility.GetParsedDate(_FromDate);
            try
            {
                lstWaterTheftStatuse = new ReportsBLL().GetWaterTheftStatuse(Convert.ToInt64(_ZoneID), Convert.ToInt64(_CircleID), Convert.ToInt64(_DivisionID), FromDate, ToDate);
            }
            catch (Exception ex)
            {
                new WRException(SessionManagerFacade.UserInformation.ID, ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstWaterTheftStatuse;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetPerformanceEvaluation(string _ZoneID, string _CircleID, string _DivisionID, string _FromDate, string _ToDate, string _Session)
        {
            DateTime? ToDate = null;
            DateTime? FromDate = null;
            List<object> lstPerformanceEvaluation = null;

            if (!string.IsNullOrEmpty(_ToDate))
                ToDate = Utility.GetParsedDate(_ToDate);
            if (!string.IsNullOrEmpty(_FromDate))
                FromDate = Utility.GetParsedDate(_FromDate);
            try
            {
                lstPerformanceEvaluation = new ReportsBLL().GetPerformanceEvaluation(Convert.ToInt64(_ZoneID), Convert.ToInt64(_CircleID), Convert.ToInt64(_DivisionID), FromDate, ToDate, _Session);
            }
            catch (Exception ex)
            {
                new WRException(SessionManagerFacade.UserInformation.ID, ex).LogException(Constants.MessageCategory.WebApp);
            }
            return lstPerformanceEvaluation;
        }
    }
}