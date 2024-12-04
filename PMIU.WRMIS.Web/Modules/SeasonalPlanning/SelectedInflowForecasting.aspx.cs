using PMIU.WRMIS.BLL.SeasonalPlanning;
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

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning
{
    public partial class SelectedInflowForecasting : BasePage
    {
        #region ViewState

        public string AddView = "AddView";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    if (Convert.ToString(Request.QueryString["From"]).ToUpper() == "VIEW")
                    {
                        ViewState[AddView] = "VIEW";
                        divStep1DraftSelection.Visible = false;
                        divView.Visible = true;
                        BindViewGrid();
                    }
                    else
                    {
                        ViewState[AddView] = "ADD";
                        lblSRMName.Text = new SeasonalPlanningBLL().GetSRMDraftName();
                        BindStatisticalDrafts();
                        SetDefaultDraftName();
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.InflowForecasting);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        public void BindStatisticalDrafts()
        {
            try
            {
                gvStatisticalDrafts.DataSource = new SeasonalPlanningBLL().GetDraftsInformation();
                gvStatisticalDrafts.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void rbDrafts_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvStatisticalDrafts.Rows)
                    ((RadioButton)row.FindControl("rbDrafts")).Checked = false;

                RadioButton rb = (RadioButton)sender;
                GridViewRow Checkedrow = (GridViewRow)rb.NamingContainer;
                ((RadioButton)Checkedrow.FindControl("rbDrafts")).Checked = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            bool Selection = false;
            bool JMSelection = false;
            bool CMSelection = false;
            bool ITSelection = false;
            bool KNSelection = false;
            try
            {
                foreach (GridViewRow row in gvStatisticalDrafts.Rows)
                    if (((RadioButton)row.FindControl("rbDrafts")).Checked == true)
                    {
                        long StatisticalDraftID = Convert.ToInt64(((Label)row.FindControl("lblID")).Text);
                        hfStatDraftID.Value = StatisticalDraftID.ToString();
                        divStep1DraftSelection.Visible = false;
                        divStep2InflowsSelection.Visible = true;
                        List<object> lstData = new SeasonalPlanningBLL().GetStatisticalAndSRMDraftDetail(StatisticalDraftID, "Maximum");
                        gvMaxCombined.DataSource = lstData;
                        gvMaxCombined.DataBind();

                        string JM = Convert.ToString(lstData[0].GetType().GetProperty("JhelumManglaSRM").GetValue(lstData[0]));
                        string CM = Convert.ToString(lstData[0].GetType().GetProperty("ChenabMaralaSRM").GetValue(lstData[0]));
                        string IT = Convert.ToString(lstData[0].GetType().GetProperty("IndusTarbelaSRM").GetValue(lstData[0]));
                        string KN = Convert.ToString(lstData[0].GetType().GetProperty("KabulNowsheraSRM").GetValue(lstData[0]));

                        if (JM == "")
                            JMSelection = true;
                        if (CM == "")
                            CMSelection = true;
                        if (IT == "" || KN == "")
                            ITSelection = true;
                        if (KN == "")
                            KNSelection = true;

                        SetMaxScenarioHeaderProbabilities(StatisticalDraftID, JMSelection, CMSelection, ITSelection, KNSelection);
                        // SetMaxScenarioFooter();

                        lstData = new SeasonalPlanningBLL().GetStatisticalAndSRMDraftDetail(StatisticalDraftID, "Minimum");
                        gvMinCombined.DataSource = lstData;
                        gvMinCombined.DataBind();

                        JMSelection = false;
                        CMSelection = false;
                        ITSelection = false;
                        KNSelection = false;

                        JM = Convert.ToString(lstData[0].GetType().GetProperty("JhelumManglaSRM").GetValue(lstData[0]));
                        CM = Convert.ToString(lstData[0].GetType().GetProperty("ChenabMaralaSRM").GetValue(lstData[0]));
                        IT = Convert.ToString(lstData[0].GetType().GetProperty("IndusTarbelaSRM").GetValue(lstData[0]));
                        KN = Convert.ToString(lstData[0].GetType().GetProperty("KabulNowsheraSRM").GetValue(lstData[0]));

                        if (JM == "")
                            JMSelection = true;
                        if (CM == "")
                            CMSelection = true;
                        if (IT == "" || KN == "")
                            ITSelection = true;
                        if (KN == "")
                            KNSelection = true;
                        SetMinScenarioHeaderProbabilities(StatisticalDraftID, JMSelection, CMSelection, ITSelection, KNSelection);

                        //  SetMinScenarioFooter();

                        JMSelection = false;
                        CMSelection = false;
                        ITSelection = false;
                        KNSelection = false;

                        lstData = new SeasonalPlanningBLL().GetStatisticalAndSRMDraftDetail(StatisticalDraftID, "Likely");
                        gvLikelyCombined.DataSource = lstData;
                        gvLikelyCombined.DataBind();

                        JM = Convert.ToString(lstData[0].GetType().GetProperty("JhelumManglaSRM").GetValue(lstData[0]));
                        CM = Convert.ToString(lstData[0].GetType().GetProperty("ChenabMaralaSRM").GetValue(lstData[0]));
                        IT = Convert.ToString(lstData[0].GetType().GetProperty("IndusTarbelaSRM").GetValue(lstData[0]));
                        KN = Convert.ToString(lstData[0].GetType().GetProperty("KabulNowsheraSRM").GetValue(lstData[0]));

                        if (JM == "")
                            JMSelection = true;
                        if (CM == "")
                            CMSelection = true;
                        if (IT == "" || KN == "")
                            ITSelection = true;
                        if (KN == "")
                            KNSelection = true;
                        SetLikelyScenarioHeaderProbabilities(StatisticalDraftID, JMSelection, CMSelection, ITSelection, KNSelection);
                        //  SetLikelyScenarioFooter();
                        Selection = true;
                        break;
                    }
                if (!Selection)
                    Master.ShowMessage(Message.ForecastDraftNotSelect.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.RedirectPermanent("InflowForecasting.aspx");

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetMaxScenarioHeaderProbabilities(long _StatisticalDraftID, bool _JMNoData, bool _CMNoData, bool _ITNoData, bool _KNNoData)
        {
            try
            {
                List<SP_ForecastScenario> lstProbabilities = new SeasonalPlanningBLL().GetStatisticalDraftProbabilities(_StatisticalDraftID, "Maximum");
                Label JMEKPer = gvMaxCombined.HeaderRow.FindControl("lblJMKahrifPer") as Label;
                Label CMEKPer = gvMaxCombined.HeaderRow.FindControl("lblCMKahrifPer") as Label;
                Label ITEKPer = gvMaxCombined.HeaderRow.FindControl("lblITKahrifPer") as Label;
                Label KNEKPer = gvMaxCombined.HeaderRow.FindControl("lblKNKahrifPer") as Label;
                Label JMLKPer = gvMaxCombined.HeaderRow.FindControl("lblJMLKPer") as Label;
                Label CMLKPer = gvMaxCombined.HeaderRow.FindControl("lblCMLKPer") as Label;
                Label ITLKPer = gvMaxCombined.HeaderRow.FindControl("lblITLKPer") as Label;
                Label KNLKPer = gvMaxCombined.HeaderRow.FindControl("lblKNLKPer") as Label;

                JMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                lstProbabilities = new SeasonalPlanningBLL().GetSRMDraftProbabilities("Maximum");
                JMEKPer = gvMaxCombined.HeaderRow.FindControl("lblJMKahrifPerSRM") as Label;
                CMEKPer = gvMaxCombined.HeaderRow.FindControl("lblCMKahrifPerSRM") as Label;
                ITEKPer = gvMaxCombined.HeaderRow.FindControl("lblITKahrifPerSRM") as Label;
                KNEKPer = gvMaxCombined.HeaderRow.FindControl("lblKNKahrifPerSRM") as Label;
                JMLKPer = gvMaxCombined.HeaderRow.FindControl("lblJMLKPerSRM") as Label;
                CMLKPer = gvMaxCombined.HeaderRow.FindControl("lblCMLKPerSRM") as Label;
                ITLKPer = gvMaxCombined.HeaderRow.FindControl("lblITLKPerSRM") as Label;
                KNLKPer = gvMaxCombined.HeaderRow.FindControl("lblKNLKPerSRM") as Label;

                JMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                if (_JMNoData)
                {
                    CheckBox JMCB = gvMaxCombined.HeaderRow.FindControl("cbJMSRMMax") as CheckBox;
                    JMCB.Enabled = false;
                }

                if (_CMNoData)
                {
                    CheckBox CMCB = gvMaxCombined.HeaderRow.FindControl("cbCMSRMMax") as CheckBox;
                    CMCB.Enabled = false;
                }

                if (_ITNoData)
                {
                    CheckBox ITCB = gvMaxCombined.HeaderRow.FindControl("cbITSRMMax") as CheckBox;
                    ITCB.Enabled = false;
                }

                if (_KNNoData)
                {
                    CheckBox KNCB = gvMaxCombined.HeaderRow.FindControl("cbKNSRMMax") as CheckBox;
                    KNCB.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }


        }

        public void SetMinScenarioHeaderProbabilities(long _StatisticalDraftID, bool _JMNoData, bool _CMNoData, bool _ITNoData, bool _KNNoData)
        {
            try
            {
                List<SP_ForecastScenario> lstProbabilities = new SeasonalPlanningBLL().GetStatisticalDraftProbabilities(_StatisticalDraftID, "Minimum");
                Label JMEKPer = gvMinCombined.HeaderRow.FindControl("lblJMKahrifPer") as Label;
                Label CMEKPer = gvMinCombined.HeaderRow.FindControl("lblCMKahrifPer") as Label;
                Label ITEKPer = gvMinCombined.HeaderRow.FindControl("lblITKahrifPer") as Label;
                Label KNEKPer = gvMinCombined.HeaderRow.FindControl("lblKNKahrifPer") as Label;
                Label JMLKPer = gvMinCombined.HeaderRow.FindControl("lblJMLKPer") as Label;
                Label CMLKPer = gvMinCombined.HeaderRow.FindControl("lblCMLKPer") as Label;
                Label ITLKPer = gvMinCombined.HeaderRow.FindControl("lblITLKPer") as Label;
                Label KNLKPer = gvMinCombined.HeaderRow.FindControl("lblKNLKPer") as Label;

                JMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                lstProbabilities = new SeasonalPlanningBLL().GetSRMDraftProbabilities("Maximum");
                JMEKPer = gvMinCombined.HeaderRow.FindControl("lblJMKahrifPerSRM") as Label;
                CMEKPer = gvMinCombined.HeaderRow.FindControl("lblCMKahrifPerSRM") as Label;
                ITEKPer = gvMinCombined.HeaderRow.FindControl("lblITKahrifPerSRM") as Label;
                KNEKPer = gvMinCombined.HeaderRow.FindControl("lblKNKahrifPerSRM") as Label;
                JMLKPer = gvMinCombined.HeaderRow.FindControl("lblJMLKPerSRM") as Label;
                CMLKPer = gvMinCombined.HeaderRow.FindControl("lblCMLKPerSRM") as Label;
                ITLKPer = gvMinCombined.HeaderRow.FindControl("lblITLKPerSRM") as Label;
                KNLKPer = gvMinCombined.HeaderRow.FindControl("lblKNLKPerSRM") as Label;

                JMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                if (_JMNoData)
                {
                    CheckBox JMCB = gvMaxCombined.HeaderRow.FindControl("cbJMSRMMin") as CheckBox;
                    JMCB.Enabled = false;
                }

                if (_CMNoData)
                {
                    CheckBox CMCB = gvMaxCombined.HeaderRow.FindControl("cbCMSRMMin") as CheckBox;
                    CMCB.Enabled = false;
                }

                if (_ITNoData)
                {
                    CheckBox ITCB = gvMaxCombined.HeaderRow.FindControl("cbITSRMMin") as CheckBox;
                    ITCB.Enabled = false;
                }

                if (_KNNoData)
                {
                    CheckBox KNCB = gvMaxCombined.HeaderRow.FindControl("cbKNSRMMin") as CheckBox;
                    KNCB.Enabled = false;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }


        }

        public void SetLikelyScenarioHeaderProbabilities(long _StatisticalDraftID, bool _JMNoData, bool _CMNoData, bool _ITNoData, bool _KNNoData)
        {
            try
            {
                List<SP_ForecastScenario> lstProbabilities = new SeasonalPlanningBLL().GetStatisticalDraftProbabilities(_StatisticalDraftID, "Likely");
                Label JMEKPer = gvLikelyCombined.HeaderRow.FindControl("lblJMKahrifPer") as Label;
                Label CMEKPer = gvLikelyCombined.HeaderRow.FindControl("lblCMKahrifPer") as Label;
                Label ITEKPer = gvLikelyCombined.HeaderRow.FindControl("lblITKahrifPer") as Label;
                Label KNEKPer = gvLikelyCombined.HeaderRow.FindControl("lblKNKahrifPer") as Label;
                Label JMLKPer = gvLikelyCombined.HeaderRow.FindControl("lblJMLKPer") as Label;
                Label CMLKPer = gvLikelyCombined.HeaderRow.FindControl("lblCMLKPer") as Label;
                Label ITLKPer = gvLikelyCombined.HeaderRow.FindControl("lblITLKPer") as Label;
                Label KNLKPer = gvLikelyCombined.HeaderRow.FindControl("lblKNLKPer") as Label;

                JMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                lstProbabilities = new SeasonalPlanningBLL().GetSRMDraftProbabilities("Maximum");
                JMEKPer = gvLikelyCombined.HeaderRow.FindControl("lblJMKahrifPerSRM") as Label;
                CMEKPer = gvLikelyCombined.HeaderRow.FindControl("lblCMKahrifPerSRM") as Label;
                ITEKPer = gvLikelyCombined.HeaderRow.FindControl("lblITKahrifPerSRM") as Label;
                KNEKPer = gvLikelyCombined.HeaderRow.FindControl("lblKNKahrifPerSRM") as Label;
                JMLKPer = gvLikelyCombined.HeaderRow.FindControl("lblJMLKPerSRM") as Label;
                CMLKPer = gvLikelyCombined.HeaderRow.FindControl("lblCMLKPerSRM") as Label;
                ITLKPer = gvLikelyCombined.HeaderRow.FindControl("lblITLKPerSRM") as Label;
                KNLKPer = gvLikelyCombined.HeaderRow.FindControl("lblKNLKPerSRM") as Label;

                JMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK %:" + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                if (_JMNoData)
                {
                    CheckBox JMCB = gvMaxCombined.HeaderRow.FindControl("cbJMSRMML") as CheckBox;
                    JMCB.Enabled = false;
                }

                if (_CMNoData)
                {
                    CheckBox CMCB = gvMaxCombined.HeaderRow.FindControl("cbCMSRMML") as CheckBox;
                    CMCB.Enabled = false;
                }

                if (_ITNoData)
                {
                    CheckBox ITCB = gvMaxCombined.HeaderRow.FindControl("cbITSRMML") as CheckBox;
                    ITCB.Enabled = false;
                }

                if (_KNNoData)
                {
                    CheckBox KNCB = gvMaxCombined.HeaderRow.FindControl("cbKNSRMML") as CheckBox;
                    KNCB.Enabled = false;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }


        }

        public void SetMaxScenarioFooter()
        {
            //Max Scenario Footer Early Kharif
            GridViewRow rowKharif = gvMaxCombined.Rows[6] as GridViewRow;
            Label Volume = rowKharif.FindControl("lblJM") as Label;
            Label Footer = gvMaxCombined.FooterRow.FindControl("lblJMKharif") as Label;
            if (Volume != null && Footer != null)
                Footer.Text = Volume.Text;

            Volume = rowKharif.FindControl("lblCM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblCMKharif") as Label;
            if (Volume != null && Footer != null)
                Footer.Text = Volume.Text;

            Volume = rowKharif.FindControl("lblIT") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblITKharif") as Label;
            if (Volume != null && Footer != null)
                Footer.Text = Volume.Text;

            Volume = rowKharif.FindControl("lblKN") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblKNKharif") as Label;
            if (Volume != null && Footer != null)
                Footer.Text = Volume.Text;

            Volume = rowKharif.FindControl("lblJMSRM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblJMKharifSRM") as Label;
            if (Volume != null && Footer != null)
                Footer.Text = Volume.Text;

            Volume = rowKharif.FindControl("lblCMSRM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblCMKharifSRM") as Label;
            if (Volume != null && Footer != null)
                Footer.Text = Volume.Text;

            Volume = rowKharif.FindControl("lblITSRM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblITKharifSRM") as Label;
            if (Volume != null && Footer != null)
                Footer.Text = Volume.Text;

            Volume = rowKharif.FindControl("lblKNSRM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblKNKharifSRM") as Label;
            if (Volume != null && Footer != null)
                Footer.Text = Volume.Text;

            //Max Scenario Footer Late Kharif
            rowKharif = gvMaxCombined.Rows[17] as GridViewRow;
            Volume = rowKharif.FindControl("lblJM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblJMLK") as Label;
            Label FooterTotal = gvMaxCombined.FooterRow.FindControl("lblJMTotal") as Label;
            if (Volume != null && Footer != null && FooterTotal != null)
            {
                Footer.Text = Volume.Text;
                FooterTotal.Text = Volume.Text;
            }

            Volume = rowKharif.FindControl("lblCM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblCMLK") as Label;
            FooterTotal = gvMaxCombined.FooterRow.FindControl("lblCMTotal") as Label;
            if (Volume != null && Footer != null && FooterTotal != null)
            {
                Footer.Text = Volume.Text;
                FooterTotal.Text = Volume.Text;
            }

            Volume = rowKharif.FindControl("lblIT") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblITLK") as Label;
            FooterTotal = gvMaxCombined.FooterRow.FindControl("lblITTotal") as Label;
            if (Volume != null && Footer != null && FooterTotal != null)
            {
                Footer.Text = Volume.Text;
                FooterTotal.Text = Volume.Text;
            }

            Volume = rowKharif.FindControl("lblKN") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblKNLK") as Label;
            FooterTotal = gvMaxCombined.FooterRow.FindControl("lblKNTotal") as Label;
            if (Volume != null && Footer != null && FooterTotal != null)
            {
                Footer.Text = Volume.Text;
                FooterTotal.Text = Volume.Text;
            }

            Volume = rowKharif.FindControl("lblJMSRM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblJMLKSRM") as Label;
            FooterTotal = gvMaxCombined.FooterRow.FindControl("lblJMTotalSRM") as Label;
            if (Volume != null && Footer != null && FooterTotal != null)
            {
                Footer.Text = Volume.Text;
                FooterTotal.Text = Volume.Text;
            }

            Volume = rowKharif.FindControl("lblCMSRM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblCMLKSRM") as Label;
            FooterTotal = gvMaxCombined.FooterRow.FindControl("lblCMTotalSRM") as Label;
            if (Volume != null && Footer != null && FooterTotal != null)
            {
                Footer.Text = Volume.Text;
                FooterTotal.Text = Volume.Text;
            }

            Volume = rowKharif.FindControl("lblITSRM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblITLKSRM") as Label;
            FooterTotal = gvMaxCombined.FooterRow.FindControl("lblITTotalSRM") as Label;
            if (Volume != null && Footer != null && FooterTotal != null)
            {
                Footer.Text = Volume.Text;
                FooterTotal.Text = Volume.Text;
            }

            Volume = rowKharif.FindControl("lblKNSRM") as Label;
            Footer = gvMaxCombined.FooterRow.FindControl("lblKNLKSRM") as Label;
            FooterTotal = gvMaxCombined.FooterRow.FindControl("lblKNTotalSRM") as Label;
            if (Volume != null && Footer != null && FooterTotal != null)
            {
                Footer.Text = Volume.Text;
                FooterTotal.Text = Volume.Text;
            }
        }

        public void SetMinScenarioFooter()
        {
            try
            {
                //Min Scenario Footer Early Kharif
                GridViewRow rowKharif = gvMinCombined.Rows[6] as GridViewRow;
                Label Volume = rowKharif.FindControl("lblJM") as Label;
                Label Footer = gvMinCombined.FooterRow.FindControl("lblJMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblCM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblCMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblIT") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblITKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblKN") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblKNKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblJMSRM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblJMKharifSRM") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblCMSRM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblCMKharifSRM") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblITSRM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblITKharifSRM") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblKNSRM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblKNKharifSRM") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                //Min Scenario Footer Late Kharif
                rowKharif = gvMinCombined.Rows[17] as GridViewRow;
                Volume = rowKharif.FindControl("lblJM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblJMLK") as Label;
                Label FooterTotal = gvMinCombined.FooterRow.FindControl("lblJMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblCM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblCMLK") as Label;
                FooterTotal = gvMinCombined.FooterRow.FindControl("lblCMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblIT") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblITLK") as Label;
                FooterTotal = gvMinCombined.FooterRow.FindControl("lblITTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblKN") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblKNLK") as Label;
                FooterTotal = gvMinCombined.FooterRow.FindControl("lblKNTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblJMSRM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblJMLKSRM") as Label;
                FooterTotal = gvMinCombined.FooterRow.FindControl("lblJMTotalSRM") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblCMSRM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblCMLKSRM") as Label;
                FooterTotal = gvMinCombined.FooterRow.FindControl("lblCMTotalSRM") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblITSRM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblITLKSRM") as Label;
                FooterTotal = gvMinCombined.FooterRow.FindControl("lblITTotalSRM") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblKNSRM") as Label;
                Footer = gvMinCombined.FooterRow.FindControl("lblKNLKSRM") as Label;
                FooterTotal = gvMinCombined.FooterRow.FindControl("lblKNTotalSRM") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetLikelyScenarioFooter()
        {
            try
            {
                //Likely Scenario Footer Early Kharif
                GridViewRow rowKharif = gvLikelyCombined.Rows[6] as GridViewRow;
                Label Volume = rowKharif.FindControl("lblJM") as Label;
                Label Footer = gvLikelyCombined.FooterRow.FindControl("lblJMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblCM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblCMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblIT") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblITKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblKN") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblKNKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblJMSRM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblJMKharifSRM") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblCMSRM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblCMKharifSRM") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblITSRM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblITKharifSRM") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = rowKharif.FindControl("lblKNSRM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblKNKharifSRM") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                //Min Scenario Footer Late Kharif
                rowKharif = gvLikelyCombined.Rows[17] as GridViewRow;
                Volume = rowKharif.FindControl("lblJM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblJMLK") as Label;
                Label FooterTotal = gvLikelyCombined.FooterRow.FindControl("lblJMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblCM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblCMLK") as Label;
                FooterTotal = gvLikelyCombined.FooterRow.FindControl("lblCMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblIT") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblITLK") as Label;
                FooterTotal = gvLikelyCombined.FooterRow.FindControl("lblITTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblKN") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblKNLK") as Label;
                FooterTotal = gvLikelyCombined.FooterRow.FindControl("lblKNTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblJMSRM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblJMLKSRM") as Label;
                FooterTotal = gvLikelyCombined.FooterRow.FindControl("lblJMTotalSRM") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblCMSRM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblCMLKSRM") as Label;
                FooterTotal = gvLikelyCombined.FooterRow.FindControl("lblCMTotalSRM") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblITSRM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblITLKSRM") as Label;
                FooterTotal = gvLikelyCombined.FooterRow.FindControl("lblITTotalSRM") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = rowKharif.FindControl("lblKNSRM") as Label;
                Footer = gvLikelyCombined.FooterRow.FindControl("lblKNLKSRM") as Label;
                FooterTotal = gvLikelyCombined.FooterRow.FindControl("lblKNTotalSRM") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox cbJMStatistical = gvMaxCombined.HeaderRow.FindControl("cbJMStaticticalMax") as CheckBox;
                CheckBox cbCMStatistical = gvMaxCombined.HeaderRow.FindControl("cbCMStaticticalMax") as CheckBox;
                CheckBox cbITStatistical = gvMaxCombined.HeaderRow.FindControl("cbITStaticticalMax") as CheckBox;
                CheckBox cbKNStatistical = gvMaxCombined.HeaderRow.FindControl("cbKNStaticticalMax") as CheckBox;
                long? JMDraftID = null;
                long? CMDraftID = null;
                long? ITDraftID = null;
                long? KNDraftID = null;


                CheckBox cbJMSRM = gvMaxCombined.HeaderRow.FindControl("cbJMSRMMax") as CheckBox;
                CheckBox cbCMSRM = gvMaxCombined.HeaderRow.FindControl("cbCMSRMMax") as CheckBox;
                CheckBox cbITSRM = gvMaxCombined.HeaderRow.FindControl("cbITSRMMax") as CheckBox;
                CheckBox cbKNSRM = gvMaxCombined.HeaderRow.FindControl("cbKNSRMMax") as CheckBox;



                if (cbJMStatistical.Checked == true)
                    JMDraftID = Convert.ToInt64(hfStatDraftID.Value);
                else if (cbJMSRM.Checked == false)
                {
                    Master.ShowMessage(Message.AllRimStationSelection.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (cbCMStatistical.Checked == true)
                    CMDraftID = Convert.ToInt64(hfStatDraftID.Value);
                else if (cbCMSRM.Checked == false)
                {
                    Master.ShowMessage(Message.AllRimStationSelection.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (cbITStatistical.Checked == true)
                    ITDraftID = Convert.ToInt64(hfStatDraftID.Value);
                else if (cbITSRM.Checked == false)
                {
                    Master.ShowMessage(Message.AllRimStationSelection.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (cbKNStatistical.Checked == true)
                    KNDraftID = Convert.ToInt64(hfStatDraftID.Value);
                else if (cbKNSRM.Checked == false)
                {
                    Master.ShowMessage(Message.AllRimStationSelection.Description, SiteMaster.MessageType.Error);
                    return;
                }

                gvMax.DataSource = new SeasonalPlanningBLL().GetFinalizedDraft(JMDraftID, CMDraftID, ITDraftID, KNDraftID, "Maximum");
                gvMax.DataBind();
                SetHeaderPercentageForMaximumScenario(JMDraftID, CMDraftID, ITDraftID, KNDraftID, Convert.ToInt64(hfStatDraftID.Value));
                //  SetFooterForMaximumScenario();

                gvMin.DataSource = new SeasonalPlanningBLL().GetFinalizedDraft(JMDraftID, CMDraftID, ITDraftID, KNDraftID, "Minimum");
                gvMin.DataBind();
                SetHeaderPercentageForMinimumScenario(JMDraftID, CMDraftID, ITDraftID, KNDraftID, Convert.ToInt64(hfStatDraftID.Value));
                //  SetFooterForMinimumScenario();

                gvLikely.DataSource = new SeasonalPlanningBLL().GetFinalizedDraft(JMDraftID, CMDraftID, ITDraftID, KNDraftID, "Likely");
                gvLikely.DataBind();
                SetHeaderPercentageForLikelyScenario(JMDraftID, CMDraftID, ITDraftID, KNDraftID, Convert.ToInt64(hfStatDraftID.Value));
                //  SetFooterForLikelyScenario();
                divStep2InflowsSelection.Visible = false;
                divStep3forecast.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetHeaderPercentageForMaximumScenario(long? _JMDraftID, long? _CMDraftID, long? _ITDraftID, long? _KNDraftID, long _DraftID)
        {
            try
            {
                List<SP_ForecastScenario> lstProbabilities = new SeasonalPlanningBLL().GetProbabilitiesForFinalizedDraft(_JMDraftID, _CMDraftID, _ITDraftID, _KNDraftID, "Maximum");
                Label JMEKPer = gvMax.HeaderRow.FindControl("lblJMKahrifPer") as Label;
                Label CMEKPer = gvMax.HeaderRow.FindControl("lblCMKahrifPer") as Label;
                Label ITEKPer = gvMax.HeaderRow.FindControl("lblITKahrifPer") as Label;
                Label KNEKPer = gvMax.HeaderRow.FindControl("lblKNKahrifPer") as Label;
                Label JMLKPer = gvMax.HeaderRow.FindControl("lblJMLKPer") as Label;
                Label CMLKPer = gvMax.HeaderRow.FindControl("lblCMLKPer") as Label;
                Label ITLKPer = gvMax.HeaderRow.FindControl("lblITLKPer") as Label;
                Label KNLKPer = gvMax.HeaderRow.FindControl("lblKNLKPer") as Label;
                Label DraftName = null;

                JMEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK %: " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                DraftName = gvMax.HeaderRow.FindControl("lblJMDraft") as Label;
                if (_DraftID == _JMDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvMax.HeaderRow.FindControl("lblCMDraft") as Label;
                if (_DraftID == _CMDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvMax.HeaderRow.FindControl("lblITDraft") as Label;
                if (_DraftID == _ITDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvMax.HeaderRow.FindControl("lblKNDraft") as Label;
                if (_DraftID == _KNDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetHeaderPercentageForMinimumScenario(long? _JMDraftID, long? _CMDraftID, long? _ITDraftID, long? _KNDraftID, long _DraftID)
        {
            try
            {
                List<SP_ForecastScenario> lstProbabilities = new SeasonalPlanningBLL().GetProbabilitiesForFinalizedDraft(_JMDraftID, _CMDraftID, _ITDraftID, _KNDraftID, "Minimum");
                Label JMEKPer = gvMin.HeaderRow.FindControl("lblJMKahrifPer") as Label;
                Label CMEKPer = gvMin.HeaderRow.FindControl("lblCMKahrifPer") as Label;
                Label ITEKPer = gvMin.HeaderRow.FindControl("lblITKahrifPer") as Label;
                Label KNEKPer = gvMin.HeaderRow.FindControl("lblKNKahrifPer") as Label;
                Label JMLKPer = gvMin.HeaderRow.FindControl("lblJMLKPer") as Label;
                Label CMLKPer = gvMin.HeaderRow.FindControl("lblCMLKPer") as Label;
                Label ITLKPer = gvMin.HeaderRow.FindControl("lblITLKPer") as Label;
                Label KNLKPer = gvMin.HeaderRow.FindControl("lblKNLKPer") as Label;
                Label DraftName = null;

                JMEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                DraftName = gvMin.HeaderRow.FindControl("lblJMDraft") as Label;
                if (_DraftID == _JMDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvMin.HeaderRow.FindControl("lblCMDraft") as Label;
                if (_DraftID == _CMDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvMin.HeaderRow.FindControl("lblITDraft") as Label;
                if (_DraftID == _ITDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvMin.HeaderRow.FindControl("lblKNDraft") as Label;
                if (_DraftID == _KNDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetHeaderPercentageForLikelyScenario(long? _JMDraftID, long? _CMDraftID, long? _ITDraftID, long? _KNDraftID, long _DraftID)
        {
            try
            {
                List<SP_ForecastScenario> lstProbabilities = new SeasonalPlanningBLL().GetProbabilitiesForFinalizedDraft(_JMDraftID, _CMDraftID, _ITDraftID, _KNDraftID, "Likely");
                Label JMEKPer = gvLikely.HeaderRow.FindControl("lblJMKahrifPer") as Label;
                Label CMEKPer = gvLikely.HeaderRow.FindControl("lblCMKahrifPer") as Label;
                Label ITEKPer = gvLikely.HeaderRow.FindControl("lblITKahrifPer") as Label;
                Label KNEKPer = gvLikely.HeaderRow.FindControl("lblKNKahrifPer") as Label;
                Label JMLKPer = gvLikely.HeaderRow.FindControl("lblJMLKPer") as Label;
                Label CMLKPer = gvLikely.HeaderRow.FindControl("lblCMLKPer") as Label;
                Label ITLKPer = gvLikely.HeaderRow.FindControl("lblITLKPer") as Label;
                Label KNLKPer = gvLikely.HeaderRow.FindControl("lblKNLKPer") as Label;
                Label DraftName = null;

                JMEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent);
                JMLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent);
                CMEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent);
                CMLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent);
                ITEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent);
                ITLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent);
                KNEKPer.Text = "EK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent);
                KNLKPer.Text = "LK % : " + Convert.ToString(lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent);

                DraftName = gvLikely.HeaderRow.FindControl("lblJMDraft") as Label;
                if (_DraftID == _JMDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvLikely.HeaderRow.FindControl("lblCMDraft") as Label;
                if (_DraftID == _CMDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvLikely.HeaderRow.FindControl("lblITDraft") as Label;
                if (_DraftID == _ITDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();

                DraftName = gvLikely.HeaderRow.FindControl("lblKNDraft") as Label;
                if (_DraftID == _KNDraftID)
                    DraftName.Text = Constants.StatisticalDraft.ToString();
                else
                    DraftName.Text = Constants.SRMDraft.ToString();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForMaximumScenario()
        {
            try
            {
                GridViewRow row = gvMax.Rows[6] as GridViewRow;
                Label Volume = row.FindControl("lblJM") as Label;
                Label Footer = gvMax.FooterRow.FindControl("lblJMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblcM") as Label;
                Footer = gvMax.FooterRow.FindControl("lblcMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvMax.FooterRow.FindControl("lblITKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvMax.FooterRow.FindControl("lblKNKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                row = gvMax.Rows[17] as GridViewRow;
                Volume = row.FindControl("lblJM") as Label;
                Footer = gvMax.FooterRow.FindControl("lblJMLK") as Label;
                Label FooterTotal = gvMax.FooterRow.FindControl("lblJMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvMax.FooterRow.FindControl("lblCMLK") as Label;
                FooterTotal = gvMax.FooterRow.FindControl("lblCMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }


                Volume = row.FindControl("lblIT") as Label;
                Footer = gvMax.FooterRow.FindControl("lblITLK") as Label;
                FooterTotal = gvMax.FooterRow.FindControl("lblITTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvMax.FooterRow.FindControl("lblKNLK") as Label;
                FooterTotal = gvMax.FooterRow.FindControl("lblKNTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForMinimumScenario()
        {
            try
            {
                GridViewRow row = gvMin.Rows[6] as GridViewRow;
                Label Volume = row.FindControl("lblJM") as Label;
                Label Footer = gvMin.FooterRow.FindControl("lblJMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvMin.FooterRow.FindControl("lblCMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvMin.FooterRow.FindControl("lblITKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvMin.FooterRow.FindControl("lblKNKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                row = gvMin.Rows[17] as GridViewRow;
                Volume = row.FindControl("lblJM") as Label;
                Footer = gvMin.FooterRow.FindControl("lblJMLK") as Label;
                Label FooterTotal = gvMin.FooterRow.FindControl("lblJMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvMin.FooterRow.FindControl("lblCMLK") as Label;
                FooterTotal = gvMin.FooterRow.FindControl("lblCMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvMin.FooterRow.FindControl("lblITLK") as Label;
                FooterTotal = gvMin.FooterRow.FindControl("lblITTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvMin.FooterRow.FindControl("lblKNLK") as Label;
                FooterTotal = gvMin.FooterRow.FindControl("lblKNTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetFooterForLikelyScenario()
        {
            try
            {
                GridViewRow row = gvLikely.Rows[6] as GridViewRow;
                Label Volume = row.FindControl("lblJM") as Label;
                Label Footer = gvLikely.FooterRow.FindControl("lblJMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblCMKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblITKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblKNKharif") as Label;
                if (Volume != null && Footer != null)
                    Footer.Text = Volume.Text;


                row = gvLikely.Rows[17] as GridViewRow;
                Volume = row.FindControl("lblJM") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblJMLK") as Label;
                Label FooterTotal = gvLikely.FooterRow.FindControl("lblJMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblCM") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblCMLK") as Label;
                FooterTotal = gvLikely.FooterRow.FindControl("lblCMTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblIT") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblITLK") as Label;
                FooterTotal = gvLikely.FooterRow.FindControl("lblITTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

                Volume = row.FindControl("lblKN") as Label;
                Footer = gvLikely.FooterRow.FindControl("lblKNLK") as Label;
                FooterTotal = gvLikely.FooterRow.FindControl("lblKNTotal") as Label;
                if (Volume != null && Footer != null && FooterTotal != null)
                {
                    Footer.Text = Volume.Text;
                    FooterTotal.Text = Volume.Text;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Variables
                string JMDraft = "";
                string CMDraft = "";
                string ITDraft = "";
                string KNDraft = "";
                string StrPercentage = "";
                string Percentage = "";
                long StatDraftID = Convert.ToInt64(hfStatDraftID.Value);
                long NewDraftID = -1;

                SP_ForecastScenario ObjSaveJMScenario = null;
                SP_ForecastScenario ObjSaveCMScenario = null;
                SP_ForecastScenario ObjSaveITScenario = null;
                SP_ForecastScenario ObjSaveKNScenario = null;

                SP_ForecastScenario ObjSaveJMScenarioMin = null;
                SP_ForecastScenario ObjSaveCMScenarioMin = null;
                SP_ForecastScenario ObjSaveITScenarioMin = null;
                SP_ForecastScenario ObjSaveKNScenarioMin = null;

                SP_ForecastScenario ObjSaveJMScenarioLikely = null;
                SP_ForecastScenario ObjSaveCMScenarioLikely = null;
                SP_ForecastScenario ObjSaveITScenarioLikely = null;
                SP_ForecastScenario ObjSaveKNScenarioLikely = null;

                short JMEK = -1;
                short JMLK = -1;
                short CMEK = -1;
                short CMLK = -1;
                short ITEK = -1;
                short ITLK = -1;
                short KNEK = -1;
                short KNLK = -1;

                long? JMDraftID = null;
                long? CMDraftID = null;
                long? ITDraftID = null;
                long? KNDraftID = null;

                #endregion

                SP_ForecastDraft Objsave = new SP_ForecastDraft();
                Objsave.Description = txtName.Text;
                Objsave.DraftType = (int)Constants.InflowForecstDrafts.SelectedDraft;
                Objsave.SeasonID = (short)Utility.GetCurrentSeasonForView();
                Objsave.Year = (short)DateTime.Now.Year;
                Objsave.CreatedDate = DateTime.Now;
                Objsave.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                Objsave.ModifiedDate = DateTime.Now;
                Objsave.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                NewDraftID = new SeasonalPlanningBLL().SaveStatisticalDraftBasicInfo(Objsave);

                CheckBox cbJMStatistical = gvMaxCombined.HeaderRow.FindControl("cbJMStaticticalMax") as CheckBox;
                CheckBox cbCMStatistical = gvMaxCombined.HeaderRow.FindControl("cbCMStaticticalMax") as CheckBox;
                CheckBox cbITStatistical = gvMaxCombined.HeaderRow.FindControl("cbITStaticticalMax") as CheckBox;
                CheckBox cbKNStatistical = gvMaxCombined.HeaderRow.FindControl("cbKNStaticticalMax") as CheckBox;

                if (cbJMStatistical.Checked == true)
                {
                    JMDraft = Constants.StatisticalDraft.ToString();
                    JMDraftID = Convert.ToInt64(hfStatDraftID.Value);
                }
                else
                    JMDraft = Constants.SRMDraft.ToString();

                if (cbCMStatistical.Checked == true)
                {
                    CMDraft = Constants.StatisticalDraft.ToString();
                    CMDraftID = Convert.ToInt64(hfStatDraftID.Value);
                }
                else
                    CMDraft = Constants.SRMDraft.ToString();

                if (cbITStatistical.Checked == true)
                {
                    ITDraft = Constants.StatisticalDraft.ToString();
                    ITDraftID = Convert.ToInt64(hfStatDraftID.Value);
                }
                else
                    ITDraft = Constants.SRMDraft.ToString();

                if (cbKNStatistical.Checked == true)
                {
                    KNDraft = Constants.StatisticalDraft.ToString();
                    KNDraftID = Convert.ToInt64(hfStatDraftID.Value);
                }
                else
                    KNDraft = Constants.SRMDraft.ToString();

                GridViewRow Header = gvMax.HeaderRow;
                if (Header != null)
                {
                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblJMKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    JMEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblJMLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    JMLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblCMKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    CMEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblCMLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    CMLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblITKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    ITEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblITLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    ITLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblKNKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    KNEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblKNLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    KNLK = Convert.ToInt16(Percentage);

                    ObjSaveJMScenario = new SP_ForecastScenario();
                    ObjSaveJMScenario.ForecastDraftID = NewDraftID;
                    ObjSaveJMScenario.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSaveJMScenario.Scenario = "Maximum";
                    if (JMDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveJMScenario.ModelDraftID = StatDraftID;
                        ObjSaveJMScenario.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveJMScenario.ModelDraftID = null;
                        ObjSaveJMScenario.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveJMScenario.EkPercent = JMEK;
                    ObjSaveJMScenario.LkPercent = JMLK;
                    ObjSaveJMScenario.CreatedDate = DateTime.Now;
                    ObjSaveJMScenario.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveJMScenario.ModifiedDate = DateTime.Now;
                    ObjSaveJMScenario.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveCMScenario = new SP_ForecastScenario();
                    ObjSaveCMScenario.ForecastDraftID = NewDraftID;
                    ObjSaveCMScenario.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSaveCMScenario.Scenario = "Maximum";
                    if (CMDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveCMScenario.ModelDraftID = StatDraftID;
                        ObjSaveCMScenario.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveCMScenario.ModelDraftID = null;
                        ObjSaveCMScenario.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveCMScenario.EkPercent = CMEK;
                    ObjSaveCMScenario.LkPercent = CMLK;
                    ObjSaveCMScenario.CreatedDate = DateTime.Now;
                    ObjSaveCMScenario.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveCMScenario.ModifiedDate = DateTime.Now;
                    ObjSaveCMScenario.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveITScenario = new SP_ForecastScenario();
                    ObjSaveITScenario.ForecastDraftID = NewDraftID;
                    ObjSaveITScenario.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSaveITScenario.Scenario = "Maximum";
                    if (ITDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveITScenario.ModelDraftID = StatDraftID;
                        ObjSaveITScenario.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveITScenario.ModelDraftID = null;
                        ObjSaveITScenario.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveITScenario.EkPercent = ITEK;
                    ObjSaveITScenario.LkPercent = ITLK;
                    ObjSaveITScenario.CreatedDate = DateTime.Now;
                    ObjSaveITScenario.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveITScenario.ModifiedDate = DateTime.Now;
                    ObjSaveITScenario.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveKNScenario = new SP_ForecastScenario();
                    ObjSaveKNScenario.ForecastDraftID = NewDraftID;
                    ObjSaveKNScenario.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSaveKNScenario.Scenario = "Maximum";
                    if (KNDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveKNScenario.ModelDraftID = StatDraftID;
                        ObjSaveKNScenario.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveKNScenario.ModelDraftID = null;
                        ObjSaveKNScenario.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveKNScenario.EkPercent = KNEK;
                    ObjSaveKNScenario.LkPercent = KNLK;
                    ObjSaveKNScenario.CreatedDate = DateTime.Now;
                    ObjSaveKNScenario.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveKNScenario.ModifiedDate = DateTime.Now;
                    ObjSaveKNScenario.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }

                Header = gvMin.HeaderRow;
                if (Header != null)
                {
                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblJMKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    JMEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblJMLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    JMLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblCMKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    CMEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblCMLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    CMLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblITKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    ITEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblITLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    ITLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblKNKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    KNEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblKNLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    KNLK = Convert.ToInt16(Percentage);

                    ObjSaveJMScenarioMin = new SP_ForecastScenario();
                    ObjSaveJMScenarioMin.ForecastDraftID = NewDraftID;
                    ObjSaveJMScenarioMin.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSaveJMScenarioMin.Scenario = "Minimum";
                    if (JMDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveJMScenarioMin.ModelDraftID = StatDraftID;
                        ObjSaveJMScenarioMin.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveJMScenarioMin.ModelDraftID = null;
                        ObjSaveJMScenarioMin.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveJMScenarioMin.EkPercent = JMEK;
                    ObjSaveJMScenarioMin.LkPercent = JMLK;
                    ObjSaveJMScenarioMin.CreatedDate = DateTime.Now;
                    ObjSaveJMScenarioMin.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveJMScenarioMin.ModifiedDate = DateTime.Now;
                    ObjSaveJMScenarioMin.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveCMScenarioMin = new SP_ForecastScenario();
                    ObjSaveCMScenarioMin.ForecastDraftID = NewDraftID;
                    ObjSaveCMScenarioMin.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSaveCMScenarioMin.Scenario = "Minimum";
                    if (CMDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveCMScenarioMin.ModelDraftID = StatDraftID;
                        ObjSaveCMScenarioMin.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveCMScenarioMin.ModelDraftID = null;
                        ObjSaveCMScenarioMin.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveCMScenarioMin.EkPercent = CMEK;
                    ObjSaveCMScenarioMin.LkPercent = CMLK;
                    ObjSaveCMScenarioMin.CreatedDate = DateTime.Now;
                    ObjSaveCMScenarioMin.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveCMScenarioMin.ModifiedDate = DateTime.Now;
                    ObjSaveCMScenarioMin.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveITScenarioMin = new SP_ForecastScenario();
                    ObjSaveITScenarioMin.ForecastDraftID = NewDraftID;
                    ObjSaveITScenarioMin.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSaveITScenarioMin.Scenario = "Minimum";
                    if (ITDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveITScenarioMin.ModelDraftID = StatDraftID;
                        ObjSaveITScenarioMin.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveITScenarioMin.ModelDraftID = null;
                        ObjSaveITScenarioMin.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveITScenarioMin.EkPercent = ITEK;
                    ObjSaveITScenarioMin.LkPercent = ITLK;
                    ObjSaveITScenarioMin.CreatedDate = DateTime.Now;
                    ObjSaveITScenarioMin.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveITScenarioMin.ModifiedDate = DateTime.Now;
                    ObjSaveITScenarioMin.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveKNScenarioMin = new SP_ForecastScenario();
                    ObjSaveKNScenarioMin.ForecastDraftID = NewDraftID;
                    ObjSaveKNScenarioMin.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSaveKNScenarioMin.Scenario = "Minimum";
                    if (KNDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveKNScenarioMin.ModelDraftID = StatDraftID;
                        ObjSaveKNScenarioMin.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveKNScenarioMin.ModelDraftID = null;
                        ObjSaveKNScenarioMin.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveKNScenarioMin.EkPercent = KNEK;
                    ObjSaveKNScenarioMin.LkPercent = KNLK;
                    ObjSaveKNScenarioMin.CreatedDate = DateTime.Now;
                    ObjSaveKNScenarioMin.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveKNScenarioMin.ModifiedDate = DateTime.Now;
                    ObjSaveKNScenarioMin.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }

                Header = gvLikely.HeaderRow;
                if (Header != null)
                {
                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblJMKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    JMEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblJMLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    JMLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblCMKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    CMEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblCMLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    CMLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblITKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    ITEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblITLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    ITLK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblKNKahrifPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    KNEK = Convert.ToInt16(Percentage);

                    StrPercentage = Convert.ToString(((Label)Header.FindControl("lblKNLKPer")).Text);
                    Percentage = StrPercentage.Substring(StrPercentage.Length - 2);
                    KNLK = Convert.ToInt16(Percentage);

                    ObjSaveJMScenarioLikely = new SP_ForecastScenario();
                    ObjSaveJMScenarioLikely.ForecastDraftID = NewDraftID;
                    ObjSaveJMScenarioLikely.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                    ObjSaveJMScenarioLikely.Scenario = "Likely";
                    if (JMDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveJMScenarioLikely.ModelDraftID = StatDraftID;
                        ObjSaveJMScenarioLikely.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveJMScenarioLikely.ModelDraftID = null;
                        ObjSaveJMScenarioLikely.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveJMScenarioLikely.EkPercent = JMEK;
                    ObjSaveJMScenarioLikely.LkPercent = JMLK;
                    ObjSaveJMScenarioLikely.CreatedDate = DateTime.Now;
                    ObjSaveJMScenarioLikely.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveJMScenarioLikely.ModifiedDate = DateTime.Now;
                    ObjSaveJMScenarioLikely.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveCMScenarioLikely = new SP_ForecastScenario();
                    ObjSaveCMScenarioLikely.ForecastDraftID = NewDraftID;
                    ObjSaveCMScenarioLikely.StationID = (int)Constants.RimStationsIDs.ChenabAtMarala;
                    ObjSaveCMScenarioLikely.Scenario = "Likely";
                    if (CMDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveCMScenarioLikely.ModelDraftID = StatDraftID;
                        ObjSaveCMScenarioLikely.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveCMScenarioLikely.ModelDraftID = null;
                        ObjSaveCMScenarioLikely.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveCMScenarioLikely.EkPercent = CMEK;
                    ObjSaveCMScenarioLikely.LkPercent = CMLK;
                    ObjSaveCMScenarioLikely.CreatedDate = DateTime.Now;
                    ObjSaveCMScenarioLikely.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveCMScenarioLikely.ModifiedDate = DateTime.Now;
                    ObjSaveCMScenarioLikely.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveITScenarioLikely = new SP_ForecastScenario();
                    ObjSaveITScenarioLikely.ForecastDraftID = NewDraftID;
                    ObjSaveITScenarioLikely.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                    ObjSaveITScenarioLikely.Scenario = "Likely";
                    if (ITDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveITScenarioLikely.ModelDraftID = StatDraftID;
                        ObjSaveITScenarioLikely.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveITScenarioLikely.ModelDraftID = null;
                        ObjSaveITScenarioLikely.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveITScenarioLikely.EkPercent = ITEK;
                    ObjSaveITScenarioLikely.LkPercent = ITLK;
                    ObjSaveITScenarioLikely.CreatedDate = DateTime.Now;
                    ObjSaveITScenarioLikely.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveITScenarioLikely.ModifiedDate = DateTime.Now;
                    ObjSaveITScenarioLikely.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);

                    ObjSaveKNScenarioLikely = new SP_ForecastScenario();
                    ObjSaveKNScenarioLikely.ForecastDraftID = NewDraftID;
                    ObjSaveKNScenarioLikely.StationID = (int)Constants.RimStationsIDs.KabulAtNowshera;
                    ObjSaveKNScenarioLikely.Scenario = "Likely";
                    if (KNDraft == Constants.StatisticalDraft.ToString())
                    {
                        ObjSaveKNScenarioLikely.ModelDraftID = StatDraftID;
                        ObjSaveKNScenarioLikely.ModelDraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
                    }
                    else
                    {
                        ObjSaveKNScenarioLikely.ModelDraftID = null;
                        ObjSaveKNScenarioLikely.ModelDraftType = (int)Constants.InflowForecstDrafts.SRMDraft;
                    }
                    ObjSaveKNScenarioLikely.EkPercent = KNEK;
                    ObjSaveKNScenarioLikely.LkPercent = KNLK;
                    ObjSaveKNScenarioLikely.CreatedDate = DateTime.Now;
                    ObjSaveKNScenarioLikely.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    ObjSaveKNScenarioLikely.ModifiedDate = DateTime.Now;
                    ObjSaveKNScenarioLikely.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                }

                List<long> lstScenarioIDs = new SeasonalPlanningBLL().SaveSelectedScenarios(ObjSaveJMScenario, ObjSaveCMScenario, ObjSaveITScenario, ObjSaveKNScenario,
                    ObjSaveJMScenarioMin, ObjSaveCMScenarioMin, ObjSaveITScenarioMin, ObjSaveKNScenarioMin,
                    ObjSaveJMScenarioLikely, ObjSaveCMScenarioLikely, ObjSaveITScenarioLikely, ObjSaveKNScenarioLikely);

                new SeasonalPlanningBLL().SaveSelectedForecastValues(JMDraftID, CMDraftID, ITDraftID, KNDraftID, lstScenarioIDs.ElementAtOrDefault(0), lstScenarioIDs.ElementAtOrDefault(1), lstScenarioIDs.ElementAtOrDefault(2), lstScenarioIDs.ElementAtOrDefault(3), Convert.ToInt32(Session[SessionValues.UserID]), "Maximum");
                new SeasonalPlanningBLL().SaveSelectedForecastValues(JMDraftID, CMDraftID, ITDraftID, KNDraftID, lstScenarioIDs.ElementAtOrDefault(4), lstScenarioIDs.ElementAtOrDefault(5), lstScenarioIDs.ElementAtOrDefault(6), lstScenarioIDs.ElementAtOrDefault(7), Convert.ToInt32(Session[SessionValues.UserID]), "Minimum");
                bool Result = new SeasonalPlanningBLL().SaveSelectedForecastValues(JMDraftID, CMDraftID, ITDraftID, KNDraftID, lstScenarioIDs.ElementAtOrDefault(8), lstScenarioIDs.ElementAtOrDefault(9), lstScenarioIDs.ElementAtOrDefault(10), lstScenarioIDs.ElementAtOrDefault(11), Convert.ToInt32(Session[SessionValues.UserID]), "Likely");

                if (Result)
                {
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    divStep3forecast.Visible = false;
                    BindViewGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        protected void gvView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int RowID = e.NewEditIndex;
                long RecordID = Convert.ToInt64(((Label)gvView.Rows[RowID].Cells[0].FindControl("lblID")).Text);
                List<SP_ForecastScenario> lstProbabilities = new SeasonalPlanningBLL().GetSavedProbabilities(RecordID);
                gvMax.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(RecordID, "Maximum");
                gvMax.DataBind();
                SetHeaderForMaximumScenario(lstProbabilities, gvMax, "Maximum");
                SetFooterForMaximumScenario();

                gvMin.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(RecordID, "Minimum");
                gvMin.DataBind();
                SetHeaderForMaximumScenario(lstProbabilities, gvMin, "Minimum");
                SetFooterForMinimumScenario();

                gvLikely.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(RecordID, "Likely");
                gvLikely.DataBind();
                SetHeaderForMaximumScenario(lstProbabilities, gvLikely, "Likely");
                SetFooterForLikelyScenario();

                btnSave.Visible = false;
                divView.Visible = false;
                divStep3forecast.Visible = true;
                btnViewBack.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetHeaderForMaximumScenario(List<SP_ForecastScenario> _lstProbabilities, GridView _gv, string _Scenario)
        {
            try
            {
                Label JMEK = (Label)_gv.HeaderRow.FindControl("lblJMKahrifPer");
                Label JMLK = (Label)_gv.HeaderRow.FindControl("lblJMLKPer");
                Label CMEK = (Label)_gv.HeaderRow.FindControl("lblCMKahrifPer");
                Label CMLK = (Label)_gv.HeaderRow.FindControl("lblCMLKPer");
                Label ITEK = (Label)_gv.HeaderRow.FindControl("lblITKahrifPer");
                Label ITLK = (Label)_gv.HeaderRow.FindControl("lblITLKPer");
                Label KNEK = (Label)_gv.HeaderRow.FindControl("lblKNKahrifPer");
                Label KNLK = (Label)_gv.HeaderRow.FindControl("lblKNLKPer");
                Label JMDraft = (Label)_gv.HeaderRow.FindControl("lblJMDraft");
                Label CMDraft = (Label)_gv.HeaderRow.FindControl("lblCMDraft");
                Label ITDraft = (Label)_gv.HeaderRow.FindControl("lblITDraft");
                Label KNDraft = (Label)_gv.HeaderRow.FindControl("lblKNDraft");

                JMEK.Text = "EK %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                            && q.Scenario == _Scenario).FirstOrDefault().EkPercent);
                JMLK.Text = "LK %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                            && q.Scenario == _Scenario).FirstOrDefault().LkPercent);
                if (_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                            && q.Scenario == _Scenario).FirstOrDefault().ModelDraftType == (int)Constants.InflowForecstDrafts.StatisticalDraft)
                    JMDraft.Text = Constants.StatisticalDraft.ToString();
                else
                    JMDraft.Text = Constants.SRMDraft.ToString();

                CMEK.Text = "EK %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                            && q.Scenario == _Scenario).FirstOrDefault().EkPercent);
                CMLK.Text = "LK %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                            && q.Scenario == _Scenario).FirstOrDefault().LkPercent);

                if (_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                            && q.Scenario == _Scenario).FirstOrDefault().ModelDraftType == (int)Constants.InflowForecstDrafts.StatisticalDraft)
                    CMDraft.Text = Constants.StatisticalDraft.ToString();
                else
                    CMDraft.Text = Constants.SRMDraft.ToString();

                ITEK.Text = "EK %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                            && q.Scenario == _Scenario).FirstOrDefault().EkPercent);
                ITLK.Text = "LK %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                            && q.Scenario == _Scenario).FirstOrDefault().LkPercent);

                if (_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                            && q.Scenario == _Scenario).FirstOrDefault().ModelDraftType == (int)Constants.InflowForecstDrafts.StatisticalDraft)
                    ITDraft.Text = Constants.StatisticalDraft.ToString();
                else
                    ITDraft.Text = Constants.SRMDraft.ToString();

                KNEK.Text = "EK %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                            && q.Scenario == _Scenario).FirstOrDefault().EkPercent);
                KNLK.Text = "LK %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                            && q.Scenario == _Scenario).FirstOrDefault().LkPercent);

                if (_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                            && q.Scenario == _Scenario).FirstOrDefault().ModelDraftType == (int)Constants.InflowForecstDrafts.StatisticalDraft)
                    KNDraft.Text = Constants.StatisticalDraft.ToString();
                else
                    KNDraft.Text = Constants.SRMDraft.ToString();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int RecordID = Convert.ToInt32(((Label)gvView.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool Result = new SeasonalPlanningBLL().DeleteDraft(RecordID);
                if (Result)
                {
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    BindViewGrid();
                }
                else
                    Master.ShowMessage(Message.NotallowedToDelete.Description, SiteMaster.MessageType.Error);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindViewGrid()
        {
            try
            {
                gvView.DataSource = new SeasonalPlanningBLL().GetSelectedDraftsInformation();
                gvView.DataBind();
                divView.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnBackViewDiv_Click(object sender, EventArgs e)
        {
            try
            {
                Response.RedirectPermanent("InflowForecasting.aspx");
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public bool SetDefaultDraftName()
        {
            string DraftName = "";
            string Description = "Selected Inflow Forecast Draft for ";
            bool Result = true;
            int Season = Utility.GetCurrentSeasonForView();
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    DraftName = new SeasonalPlanningBLL().GetDefaultDraftName(Season);
                    if (DraftName.ToUpper() == "NOT ALLOWED")
                    {
                        Result = false;
                        Master.ShowMessage(Message.MoreDraftsNotAllowed.Description, SiteMaster.MessageType.Error);
                        divView.Visible = true;
                        divStep1DraftSelection.Visible = false;
                        BindViewGrid();
                    }
                    else
                        txtName.Text = DraftName + Description + "Kharif " + DateTime.Now.Year.ToString();
                }
                else
                {
                    DraftName = new SeasonalPlanningBLL().GetDraftCountName(Season);
                    if (DraftName.ToUpper() == "NOT ALLOWED")
                    {
                        Result = false;
                        Master.ShowMessage(Message.MoreDraftsNotAllowed.Description, SiteMaster.MessageType.Error);
                        divView.Visible = true;
                        divStep1DraftSelection.Visible = false;
                        BindViewGrid();
                    }
                    else
                    {
                        if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch && DateTime.Now.Day < (int)Constants.PlanningMonthsAndDays.PlanningDay)
                            || DateTime.Now.Month < (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch)
                            txtName.Text = DraftName + Description + "Rabi " + (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString();
                        else
                            txtName.Text = DraftName + Description + "Rabi " + DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        protected void btnViewBack_Click(object sender, EventArgs e)
        {
            try
            {
                BindViewGrid();
                divView.Visible = true;
                divStep3forecast.Visible = false;
                btnViewBack.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMaxCombined_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTDaily");
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)"))
                        e.Row.Font.Bold = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMinCombined_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTDaily");
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)"))
                        e.Row.Font.Bold = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLikelyCombined_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTDaily");
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)"))
                        e.Row.Font.Bold = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMax_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTDaily");
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)"))
                        e.Row.Font.Bold = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTDaily");
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)"))
                        e.Row.Font.Bold = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvLikely_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTDaily");
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)"))
                        e.Row.Font.Bold = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSelectionBack_Click(object sender, EventArgs e)
        {
            try
            {
                divStep2InflowsSelection.Visible = false;
                divStep1DraftSelection.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}