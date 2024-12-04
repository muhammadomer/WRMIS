using Microsoft.Reporting.WebForms;
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
    public partial class DailyWaterPosition : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindScenarioDDL();
                    txtDate.Text = Utility.GetFormattedDate(DateTime.Now);
                }

                iframestyle.Visible = false;
                Master.FindControl("dvPageTitle").Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindScenarioDDL()
        {
            ddlScenario.DataSource = CommonLists.GetScenarios();
            ddlScenario.DataTextField = "Name";
            ddlScenario.DataValueField = "ID";
            ddlScenario.DataBind();
        }

        protected void btnDailyWaterPositionReports_Click(object sender, EventArgs e)
        {
            try
            {
                Session[SessionValues.ReportData] = GetReportData();
                iframestyle.Visible = true;
                iframestyle.Src = Constants.ReportsUrl;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }



        private ReportData GetReportData()
        {
            ReportData rptData = new ReportData();

            if (rbDailyWaterAvailabilityPosition.Checked)
            {
                rptData = DailyWaterAvailabilityPositionParameters();
            }
            else if (rbSuppyPosition.Checked)
            {
                rptData = DailyWaterAvailabilityPositionParameters();
            }
            else if (rbCanalWithdrawals.Checked)
            {

                rptData = CanalWithdrawalsIndusJC();
            }
            else if (rbDailyGauge.Checked)
            {

                rptData = DailyGauge_CanalWithdrwal();
            }
            else if (rbChannelWithdrawals.Checked)
            {
                rptData = DailyGauge_CanalWithdrwal();
            }
            else if (rbCanalWireIndus.Checked)
            {
                rptData = CanalWire();
            }
            else if (rbCanalWireJC.Checked)
            {
                rptData = CanalWire(false);
            }

            return rptData;
        }


        public ReportData DailyWaterAvailabilityPositionParameters()
        {
            ReportParameter rptParameter = null;
            ReportData rptData = new ReportData();


            string Date = txtDate.Text;
            string Scenario = ddlScenario.SelectedItem.Text;


            rptParameter = new ReportParameter("date", Date);
            rptData.Parameters.Add(rptParameter);

            rptParameter = new ReportParameter("Scenario", Scenario);
            rptData.Parameters.Add(rptParameter);

            //rptParameter = new ReportParameter("UserName", SessionManagerFacade.UserInformation.UserName.ToString());
            //rptData.Parameters.Add(rptParameter);

            //if (SessionManagerFacade.UserInformation.UA_Designations != null)
            //{
            //    rptParameter = new ReportParameter("UserDesignation", SessionManagerFacade.UserInformation.UA_Designations.Name.ToString());
            //    rptData.Parameters.Add(rptParameter);
            //}

            if (rbDailyWaterAvailabilityPosition.Checked)
                rptData.Name = ReportConstants.rptDailyWaterAvailibityPosition;
            else
                rptData.Name = ReportConstants.rptSupplyPosition;

            return rptData;
        }
        public ReportData CanalWithdrawalsIndusJC()
        {
            ReportParameter rptParameter = null;
            ReportData rptData = new ReportData();
            string Date = txtDate.Text;
            string Scenario = ddlScenario.SelectedItem.Text;
            rptParameter = new ReportParameter("Readingdate", Date);
            rptData.Parameters.Add(rptParameter);
            if (SessionManagerFacade.UserInformation.UA_Designations != null)
            {
                rptParameter = new ReportParameter("UserDesignation", SessionManagerFacade.UserInformation.UA_Designations.Name.ToString());
                rptData.Parameters.Add(rptParameter);
            }

            rptData.Name = ReportConstants.rptCanalsWithDrwals;

            return rptData;
        }

        public ReportData DailyGauge_CanalWithdrwal()
        {
            ReportParameter rptParameter = null;
            ReportData rptData = new ReportData();
            string Date = txtDate.Text;
            rptParameter = new ReportParameter("PReadingDate", Date);
            rptData.Parameters.Add(rptParameter);
            if (rbDailyGauge.Checked)
            {
                rptData.Name = ReportConstants.rptDailyGauge;
            }
            else if (rbChannelWithdrawals.Checked)
            {
                rptData.Name = ReportConstants.rptCanalWithdrwals_ManglaAndTarbela;
            }

            return rptData;
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCanalWithdrawals.Checked || rbDailyGauge.Checked || rbChannelWithdrawals.Checked || rbCanalWireIndus.Checked || rbCanalWireJC.Checked)
            {
                Scenario.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "DisableDesignParameter(" + Scenario.ClientID + ");", true);
            }
            else if (rbDailyWaterAvailabilityPosition.Checked || rbSuppyPosition.Checked)
            {
                Scenario.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "EnableDesignParameter(" + Scenario.ClientID + ");", true);
            }

        }

        public ReportData CanalWire(bool _IsIndus = true)
        {
            ReportData reportData = new ReportData();

            string Date = txtDate.Text;

            ReportParameter reportParameter = new ReportParameter("Date", Date);
            reportData.Parameters.Add(reportParameter);

            if (_IsIndus)
            {
                reportData.Name = ReportConstants.rptCanalWireIndus;
            }
            else
            {
                reportData.Name = ReportConstants.rptCanalWireJC;
            }

            return reportData;
        }








    }
}