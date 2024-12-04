using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class ViewSchedule : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleID"]))
                    {
                        long scheduleID = Convert.ToInt64(Request.QueryString["ScheduleID"]);
                        LoadScheduleDetails(scheduleID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ViewScheduleInspection);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;


        }
        private void LoadScheduleDetails(long _ScheduleID)
        {
            SI_Schedule schedule = new ScheduleInspectionBLL().GetSchedule(_ScheduleID);
            SI_ScheduleStatus scheduleStatus = new ScheduleInspectionBLL().GetScheduleStatusByScheduleID(_ScheduleID);

            lblScheduleName.Text = schedule.Name;
            lblScheduleDescription.Text = schedule.Description;
            lblFromDate.Text = Convert.ToString(schedule.FromDate);
            lblToDate.Text = Convert.ToString(schedule.ToDate);
            lblStatus.Text = scheduleStatus.Name;

            //1 Load Gauge Inspection
            LoadGaugeInspection(_ScheduleID);
            //2 Load Outlet Alteration
            LoadOutletAlteration(_ScheduleID);
            //3 Load Works
            LoadWorks(_ScheduleID);
            //4 Load Outlet Performance Register
            LoadOutletPerformanceRegister(_ScheduleID);
            //5 Load Tender
            LoadTender(_ScheduleID);
            //6 Load Discharge Measurement
            LoadDischargeMeasurement(_ScheduleID);
        }
        private void LoadGaugeInspection(long _ScheduleID)
        {
            List<object> lstGaugeInspection = new ScheduleInspectionBLL().GetGaugeInspectionsByScheduleID(_ScheduleID);
            gvGaugeInspection.DataSource = lstGaugeInspection;
            gvGaugeInspection.DataBind();
        }
        private void LoadOutletAlteration(long _ScheduleID)
        {
            List<object> lstOutletAlteration = new ScheduleInspectionBLL().GetOutletAlterationByScheduleID(_ScheduleID);
            gvOutletAlteration.DataSource = lstOutletAlteration;
            gvOutletAlteration.DataBind();
        }
        private void LoadWorks(long _ScheduleID)
        {
            List<object> lstWork = new ScheduleInspectionBLL().GetWorksByScheduleID(_ScheduleID);
            gvWorks.DataSource = lstWork;
            gvWorks.DataBind();
        }
        private void LoadOutletPerformanceRegister(long _ScheduleID)
        {
            List<object> lstOutletPerformanceRegister = new ScheduleInspectionBLL().GetOutletPerformanceRegisterByScheduleID(_ScheduleID);
            gvOutletPerformanceRegister.DataSource = lstOutletPerformanceRegister;
            gvOutletPerformanceRegister.DataBind();
        }
        private void LoadTender(long _ScheduleID)
        {
            List<object> lstTender = new ScheduleInspectionBLL().GetTenderByScheduleID(_ScheduleID);
            gvTender.DataSource = lstTender;
            gvTender.DataBind();
        }
        private void LoadDischargeMeasurement(long _ScheduleID)
        {
            List<object> lstDischargeMeasurement = new ScheduleInspectionBLL().GetDischargeMeasurementByScheduleID(_ScheduleID);
            gvDischargeMeasurement.DataSource = lstDischargeMeasurement;
            gvDischargeMeasurement.DataBind();
        }
    }
}