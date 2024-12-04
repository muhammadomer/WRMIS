using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.BLL.DailyData;
using System.Web.Services;
using System.Web.Script.Services;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
namespace PMIU.WRMIS.Web.Modules.DailyData
{
    public partial class OperationalData : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SetPageTitle();
                    BindDropdownlists();
                    EnableControls();
                }
                catch (Exception ex)
                {
                    new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
                }

            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.OperationalData);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void EnableControls()
        {
            switch (SessionManagerFacade.UserInformation.DesignationID)
            {
                case (long)Constants.Designation.DataAnalyst:
                    ddlZone.Enabled = true;
                    ddlCircle.Enabled = true;
                    ddlDivision.Enabled = true;
                    ddlSubDivision.Enabled = true;
                    break;
                case (long)Constants.Designation.XEN:
                    ddlSubDivision.Enabled = true;
                    break;
                default:
                    ddlZone.Enabled = false;
                    ddlCircle.Enabled = false;
                    ddlDivision.Enabled = false;
                    ddlSubDivision.Enabled = false;
                    break;
            }
        }

        #region "Dropdownlists Events
        private void BindDropdownlists()
        {
            try
            {
                UA_AssociatedLocation userLocation = SessionManagerFacade.UserAssociatedLocations;
                switch (SessionManagerFacade.UserInformation.DesignationID)
                {
                    case (long)Constants.Designation.SDO:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.SubDivision == userLocation.IrrigationLevelID)
                        {
                            LoadAllDropdownlistsData(new SubDivisionBLL().GetByID(userLocation.IrrigationBoundryID.Value).DivisionID.Value);
                            Dropdownlist.SetSelectedValue(ddlSubDivision, Convert.ToString(userLocation.IrrigationBoundryID.Value));
                        }
                        break;
                    case (long)Constants.Designation.XEN:
                        if (userLocation != null && (long)Constants.IrrigationLevelID.Division == userLocation.IrrigationLevelID)
                            LoadAllDropdownlistsData(userLocation.IrrigationBoundryID.Value);
                        break;
                    case (long)Constants.Designation.DataAnalyst:
                        DDLEmptyCircleDivisionSubDivision();
                        break;
                }
                Dropdownlist.BindDropdownlist<List<object>>(ddlSession, CommonLists.GetSession());
                Dropdownlist.BindDropdownlist<List<CO_ReasonForChange>>(ddlReasonForChange, new DailyDataBLL().GetReasonForChange());

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void LoadAllDropdownlistsData(long _DivisionID)
        {
            CO_Division division = new DivisionBLL().GetByID(_DivisionID);
            CO_Circle circle = new CircleBLL().GetByID(division.CircleID.Value);
            // Bind Zone dropdownlist 
            Dropdownlist.DDLZones(ddlZone);
            Dropdownlist.SetSelectedValue(ddlZone, Convert.ToString(circle.ZoneID));
            Dropdownlist.DDLCircles(ddlCircle, false, Convert.ToInt64(ddlZone.SelectedItem.Value));
            Dropdownlist.SetSelectedValue(ddlCircle, Convert.ToString(circle.ID));
            Dropdownlist.DDLDivisions(ddlDivision, false, Convert.ToInt64(ddlCircle.SelectedItem.Value));
            Dropdownlist.SetSelectedValue(ddlDivision, Convert.ToString(division.ID));
            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, Convert.ToInt64(ddlDivision.SelectedItem.Value));
        }
        private void DDLEmptyCircleDivisionSubDivision()
        {
            // Bind empty circle dropdownlist
            Dropdownlist.DDLCircles(ddlCircle, true);
            // Bind empty division dropdownlist
            Dropdownlist.DDLDivisions(ddlDivision, true);
            // Bind empty sub division dropdownlist
            Dropdownlist.DDLSubDivisions(ddlSubDivision, true);
        }
        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Reset Circle,Division, Sub Division dropdownlists
                DDLEmptyCircleDivisionSubDivision();
                // Bind Circel dropdownlist based on Zone
                string zoneID = ddlZone.SelectedItem.Value;
                Dropdownlist.DDLCircles(ddlCircle, false, string.IsNullOrEmpty(zoneID) == true ? -1 : Convert.ToInt64(zoneID));

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
                Dropdownlist.DDLDivisions(ddlDivision, true);
                // Reset sub division dropdownlist
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true);

                int circleID = Convert.ToInt32(ddlCircle.SelectedItem.Value);
                // Bind Division dropdownlist based on Circle 
                Dropdownlist.DDLDivisions(ddlDivision, false, circleID, Constants.IrrigationDomainID);

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
                Dropdownlist.DDLSubDivisions(ddlSubDivision, true);

                // Bind Sub Division dropdownlist based on Division 
                long divisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
                Dropdownlist.DDLSubDivisions(ddlSubDivision, false, divisionID);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Dictionary<string, object> GetDailyGaugeReadingData(string _ZoneID = "0", string _CircleID = "0", string _DivisionID = "0", string _SubDivisionID = "0", string _Date = "", string _Session = "1", int _PageIndex = 0, int _PageSize = 10)
        {
            Dictionary<string, object> dictDailyData = null;
            try
            {
                //dictDailyData = new DailyDataBLL().GetDailyGaugeReadingData(Convert.ToInt64(_ZoneID)
                //    , Convert.ToInt64(_CircleID)
                //    , Convert.ToInt64(_DivisionID)
                //    , Convert.ToInt64(string.IsNullOrEmpty(_SubDivisionID) ? "-1" : _SubDivisionID)
                //    , Utility.GetParsedDate(_Date)
                //    , Convert.ToInt16(_Session)
                //    , _PageIndex
                //    , _PageSize);
                return dictDailyData;
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
                dictDailyData = new Dictionary<string, object>();
                return dictDailyData;
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Dictionary<string, object> GetAuditTrail(string _Date = "", string _Session = "1", string _DailyGaugeReadingID = "0", int _PageIndex = 0, int _PageSize = 10)
        {
            Dictionary<string, object> dictAuditTrail = null;
            try
            {
                //dictAuditTrail = new DailyDataBLL().GetAuditTrail(Utility.GetParsedDate(_Date)
                //            , Convert.ToInt16(_Session)
                //            , Convert.ToInt64(_DailyGaugeReadingID)
                //            , _PageIndex
                //            , _PageSize);
                return dictAuditTrail;
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.WebApp);
                dictAuditTrail = new Dictionary<string, object>();
                return dictAuditTrail;
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool SaveGaugeValue(string _NewGuageValue = "", string _GaugeID = "", string _ResonForChange = "", string _DailyGaugeReadingID = "")
        {
            if (_ResonForChange != "" && _NewGuageValue != "")
            {
                //double discharge = new DailyDataBLL().CalculateDischarge(Convert.ToInt64(_GaugeID), Convert.ToDouble(_NewGuageValue));
                //bool result = new DailyDataBLL().UpdateDischarge(Convert.ToInt64(_DailyGaugeReadingID), discharge, Convert.ToInt64(_ResonForChange), Convert.ToDouble(_NewGuageValue));
               // return result;
                return true;
            }
            else
            {
                return false;

            }
        }
        #endregion
    }
}