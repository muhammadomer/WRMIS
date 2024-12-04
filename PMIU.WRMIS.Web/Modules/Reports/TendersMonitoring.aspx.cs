
using Microsoft.Reporting.WebForms;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.BLL.ClosureOperations;
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
using System.Web.Services;
using System.Web.Script.Services;

namespace PMIU.WRMIS.Web.Modules.Reports
{
    public partial class TendersMonitoring : BasePage
    {
        long userID;
        long? IsLocationAssigned;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                userID = SessionManagerFacade.UserAssociatedLocations.UserID;
                IsLocationAssigned = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;
                              
                if (!IsPostBack)
                {
                   
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlStatus, CommonLists.GetTenderMonitoringStatus(), (int)Constants.DropDownFirstOption.All);
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDivision, null, (int)Constants.DropDownFirstOption.Select);
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlTenderNoticeName, null, (int)Constants.DropDownFirstOption.Select);
                    txtFromDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));
                    txtToDate.Text = Utility.GetFormattedDate(DateTime.Now.AddDays(-1));

                    if (IsLocationAssigned != null && userID > 0)
                    {
                        LoadAllRegionDDByUser(userID, IsLocationAssigned);
                    }
                    else
                    {
                        BindDDLForLocationLessUser();
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
            List<object> lstData = new UserAdministrationBLL().GetRegionsListByUser(_UserID, Convert.ToInt32(_IrrigationLevelID));
            List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
            if (lstDivision.Count > 0) // Division
            {
                Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlDivision, lstDivision, (int)Constants.DropDownFirstOption.Select);
            }
            Dropdownlist.DDLDomainByUserID(ddlDomain, _UserID, (long)_IrrigationLevelID);
        }


        #region "Dropdownlists Events
        private void BindDDLForLocationLessUser()
        {
            try
            {
                Dropdownlist.DDLDomains(ddlDomain);
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
            string division = ddlDivision.SelectedItem.Value == string.Empty ? "0" : ddlDivision.SelectedItem.Value;
            string status = ddlStatus.SelectedItem.Value == string.Empty ? "0" : ddlStatus.SelectedItem.Value;
            string domain = ddlDomain.SelectedItem.Value == string.Empty ? "0" : ddlDomain.SelectedItem.Value;
            string tenderNotice = ddlTenderNoticeName.SelectedItem.Value == string.Empty ? "0" : ddlTenderNoticeName.SelectedItem.Value;
            
            string FromDate = string.IsNullOrEmpty(txtFromDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtFromDate.Text));
            string ToDate = string.IsNullOrEmpty(txtToDate.Text) ? null : Utility.GetFormattedDate(Convert.ToDateTime(txtToDate.Text));

            ReportData rptData = new ReportData();
            ReportParameter reportParameter;
            reportParameter = new ReportParameter("DomainID", domain);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("DivisionID", division);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("Status", status);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("TenderNoticeName", tenderNotice);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("FromDate", FromDate);
            rptData.Parameters.Add(reportParameter);
            reportParameter = new ReportParameter("ToDate", ToDate);
            rptData.Parameters.Add(reportParameter);

            return rptData;
        }
        private ReportData GetReportData()
        {
            ReportData rptData = null;
            rptData = GetReportParameters();
            rptData.Name = ReportConstants.rptSoldTenderList;
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

        protected void ddlDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDomain.SelectedValue))
            {
                if (IsLocationAssigned != null)
                {
                    List<object> lstData = new UserAdministrationBLL().GetRegionsListByUser(userID, Convert.ToInt32(IsLocationAssigned));
                    List<CO_Division> lstDivision = (List<CO_Division>)lstData.ElementAt(1);
                    if (lstDivision.Count > 0) // Division
                    {
                        Dropdownlist.BindDropdownlist<List<CO_Division>>(ddlDivision, lstDivision, (int)Constants.DropDownFirstOption.Select);
                    }
                }
                else
                {
                    Dropdownlist.DDLDivisionsByDomainID(ddlDivision, Convert.ToInt64(ddlDomain.SelectedValue));
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlTenderNoticeName, null);
                }
            }
            else
            {
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlDivision, null);
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlTenderNoticeName, null);
            }
        }
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDivision.SelectedValue))
            {
                Dropdownlist.DDLTenderMonitoringTenderNotice(ddlTenderNoticeName, Convert.ToInt32(ddlDivision.SelectedValue));
            }
            else
            {
                Dropdownlist.BindDropdownlist<List<dynamic>>(ddlTenderNoticeName, null, (int)Constants.DropDownFirstOption.Select);
            }
        }
    }
}