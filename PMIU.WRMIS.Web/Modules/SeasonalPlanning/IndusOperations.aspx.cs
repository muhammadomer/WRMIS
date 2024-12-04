using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.SeasonalPlanning;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning
{
    public partial class IndusOperations : BasePage
    {
        public static long ForecastDraftID;// = 141;
        public static long SeasonalDraftID;// = 22;
        public static string Scenario;// = "Maximum";
        public static List<object> lstData = new List<object>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                htitle.InnerText = "Seasonal Planning-" + Scenario + " Scenario";
                SetTitle();
                BindIndusData();
                if (!Utility.PlanningDaysLimit() || Scenario == "Likely")
                    btnSave.Visible = false;
                //else
                //    btnSave.Visible = true;
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.InflowForecasting);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void BindIndusData()
        {
            try
            {
                lstData = new SeasonalPlanningBLL().GetSavedPlan(SeasonalDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, Scenario);

                if (lstData.Count() < 18)
                {
                    if (Scenario == "Likely")
                        lstData = new SeasonalPlanningBLL().GetLikelyPlanDataIndus(SeasonalDraftID, true, false);
                    else
                        lstData = new SeasonalPlanningBLL().CalculateIndusPlan(ForecastDraftID, SeasonalDraftID, Scenario, true);
                    btnSave.Visible = true;
                }
                else
                    btnSave.Visible = false;

                gvIndus.DataSource = lstData;
                gvIndus.DataBind();
                if (Scenario == "Likely")
                    btnsave_Click(null, null);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Scenario == "Likely")
                    lstData = new SeasonalPlanningBLL().GetLikelyPlanDataIndus(SeasonalDraftID, false, true);
                else
                    lstData = new SeasonalPlanningBLL().CalculateIndusPlan(ForecastDraftID, SeasonalDraftID, Scenario, false);

                bool Result = new SeasonalPlanningBLL().SavePlanBulk(lstData, Convert.ToInt64(Session[SessionValues.UserID]), SeasonalDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, Scenario);
                if (sender != null)
                {
                    if (Result)
                    {
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                        btnSave.Visible = false;
                    }
                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void anchBalance_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Response.RedirectPermanent("SeasonalPlanning.aspx?Call=1");
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvIndus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblPeriod");
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)" || lbl.Text == "Rabi(MAF)"))
                        e.Row.Font.Bold = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent("SeasonalPlanning.aspx?Call=0");
        }
    }
}