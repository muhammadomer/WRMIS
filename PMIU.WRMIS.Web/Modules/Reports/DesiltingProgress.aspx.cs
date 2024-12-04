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
    public partial class DesiltingProgress : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Dropdownlist.BindDropdownlist<List<dynamic>>(ddlClosurePeriod, CommonLists.GetClosurePeriod(), (int)Constants.DropDownFirstOption.NoOption);

                }
                iframestyle.Visible = false;

                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #region "Reports Paramert Methods"
        private ReportData GetReportParameters()
        {
            string ClosurePeriod = ddlClosurePeriod.SelectedItem.Value == string.Empty ? "0" : ddlClosurePeriod.SelectedItem.Value;
            ReportParameter reportParameter=null;
            ReportData rptData = new ReportData();
            reportParameter = new ReportParameter("ClosurePeriod", ClosurePeriod);
            rptData.Parameters.Add(reportParameter);
            return rptData;
        }
        private ReportData GetReportData()
        {
            ReportData rptData = null;
            rptData = GetReportParameters();
            rptData.Name = ReportConstants.rptCWDesiltingProgress;
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