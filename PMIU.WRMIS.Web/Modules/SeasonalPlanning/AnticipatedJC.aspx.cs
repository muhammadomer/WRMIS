using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.SeasonalPlanning;

namespace PMIU.WRMIS.Web.Modules.SeasonalPlanning
{
    public partial class AnticipatedJC : BasePage
    {

        public static long _ForecastDraftID;
        public static long _PlanDraftID;
        public static string _Scenario;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTitle();
                htitle.InnerText = "Seasonal Planning-" + _Scenario + " Scenario";
                if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                {
                    divKharif.Style.Add("display", "block");
                    LoadAnticipatedJCData();
                }
                else
                {
                    divRabi.Style.Add("display", "block");
                    LoadAnticipatedJCRabiData();

                }
            }
        }
        public void LoadAnticipatedJCData()
        {

            try
            {
                object DataObj = new SeasonalPlanningBLL().AnticipatedData(_ForecastDraftID, _PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                // short TDailyID = Convert.ToInt16(Utility.GetDynamicPropertyValue(DataObj, "TDailyID"));

                if (DataObj != null)
                {
                    var JhelumAtManglaEK = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaEK");
                    var JhelumAtManglaLK = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaLK");
                    var ChenabAtMaralaEK = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaEK");
                    var ChenabAtMaralaLK = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaLK");
                    var JhelumEkPercent = Utility.GetDynamicPropertyValue(DataObj, "JhelumEkPercent");
                    var JhelumLkPercent = Utility.GetDynamicPropertyValue(DataObj, "JhelumLkPercent");
                    var ChenabEkPercent = Utility.GetDynamicPropertyValue(DataObj, "ChenabEkPercent");
                    var ChenabLkPercent = Utility.GetDynamicPropertyValue(DataObj, "ChenabLkPercent");
                    //var JhelumAtManglaLKValue = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaLKValue");
                    //var ChenabAtMaralaLKValue = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaLKValue");


                    #region Jhelum At Mangla

                    lblJhelumManglaInflowsEK.Text = JhelumAtManglaEK;
                    lblJhelumManglaInflowsLK.Text = JhelumAtManglaLK;//JhelumAtManglaLKValue;
                    lblJhelumManglaInflowsTotal.Text = Convert.ToString(Convert.ToDouble(JhelumAtManglaEK) + Convert.ToDouble(JhelumAtManglaLK));
                    lbljhelumManglaProbabilityEk.Text = JhelumEkPercent + " %";
                    lbljhelumManglaProbabilityLK.Text = JhelumLkPercent + " %";
                    //  lbljhelumManglaProbabilityTotal.Text = "";

                    #endregion

                    #region Chenab At Marala
                    lblChenabMarlaInflowsEK.Text = ChenabAtMaralaEK;
                    lblChenabMarlaInflowsLK.Text = ChenabAtMaralaLK;//ChenabAtMaralaLKValue;
                    lblChenabMarlaInflowsTotal.Text = Convert.ToString(Convert.ToDouble(ChenabAtMaralaEK) + Convert.ToDouble(ChenabAtMaralaLK));
                    lblChenabMarlaProbabilityEK.Text = ChenabEkPercent + " %";
                    lblChenabMarlaProbabilityLK.Text = ChenabLkPercent + " %";

                    #endregion

                }

                #region ER Components

                object ERComponent = new SeasonalPlanningBLL().ERComponents(_PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                if (ERComponent != null)
                {


                    lblERComponentsEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(ERComponent, "EasternEK"))));
                    lblERComponentsLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(ERComponent, "EasternLK"))));
                    lblERComponentsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblERComponentsEK.Text) + Convert.ToDouble(lblERComponentsLK.Text)));

                    //    double EK = Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "EK"));
                    //    double LK = Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "LK"));
                    //    lblERComponentsEK.Text = string.Format("{0:0.000}", EK);
                    //    lblERComponentsLK.Text = string.Format("{0:0.000}", LK);
                    //    lblERComponentsTotal.Text = Convert.ToString(Convert.ToDouble(lblERComponentsEK.Text) + Convert.ToDouble(lblERComponentsLK.Text));
                }

                #endregion ER Components

                #region Sub Total
                lblSubTotalEK.Text = Convert.ToString(Convert.ToDouble(lblJhelumManglaInflowsEK.Text) + Convert.ToDouble(lblChenabMarlaInflowsEK.Text) + Convert.ToDouble(lblERComponentsEK.Text));
                lblSubTotalLK.Text = Convert.ToString(Convert.ToDouble(lblJhelumManglaInflowsLK.Text) + Convert.ToDouble(lblChenabMarlaInflowsLK.Text) + Convert.ToDouble(lblERComponentsLK.Text));
                lblSubTotal_Total.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblJhelumManglaInflowsTotal.Text) + Convert.ToDouble(lblChenabMarlaInflowsTotal.Text) + Convert.ToDouble(lblERComponentsTotal.Text)));

                #endregion

                DataObj = new SeasonalPlanningBLL().AnticipatedJCData(_PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                if (DataObj != null)
                {
                    lblManglaEK.Text = Utility.GetDynamicPropertyValue(DataObj, "StorageReleaseEK");
                    lblManglaLK.Text = Utility.GetDynamicPropertyValue(DataObj, "StorageReleaseLK");
                    lblManglaTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblManglaEK.Text) + Convert.ToDouble(lblManglaLK.Text)));


                    lblTotalSystemInflowsEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSubTotalEK.Text) + Convert.ToDouble(lblManglaEK.Text)));
                    lblTotalSystemInflowsLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSubTotalLK.Text) + Convert.ToDouble(lblManglaLK.Text)));
                    lblTotalSystemInflowsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSubTotal_Total.Text) + Convert.ToDouble(lblManglaTotal.Text)));

                    lblSystemLGEK.Text = Utility.GetDynamicPropertyValue(DataObj, "LossGainEK");
                    lblSystemLGLK.Text = Utility.GetDynamicPropertyValue(DataObj, "LossGainLK");
                    lblSystemLGTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSystemLGEK.Text) + Convert.ToDouble(lblSystemLGLK.Text)));

                    lblTotalAvailabilityEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsEK.Text) + Convert.ToDouble(lblSystemLGEK.Text)));
                    lblTotalAvailabilityLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsLK.Text) + Convert.ToDouble(lblSystemLGLK.Text)));
                    lblTotalAvailabilityTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsTotal.Text) + Convert.ToDouble(lblSystemLGTotal.Text)));

                    lblJCOutFlowsEK.Text = Utility.GetDynamicPropertyValue(DataObj, "JCOutflowEK");
                    lblJCOutFlowsLK.Text = Utility.GetDynamicPropertyValue(DataObj, "JCOutflowLK");
                    lblJCOutFlowsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblJCOutFlowsEK.Text) + Convert.ToDouble(lblJCOutFlowsLK.Text)));

                    lblCanalAvailabilityEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalAvailabilityEK.Text) - Convert.ToDouble(lblJCOutFlowsEK.Text)));
                    lblCanalAvailabilityLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalAvailabilityLK.Text) - Convert.ToDouble(lblJCOutFlowsLK.Text)));
                    lblCanalAvailabilityTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalAvailabilityTotal.Text) - Convert.ToDouble(lblJCOutFlowsTotal.Text)));



                }

                DataObj = new SeasonalPlanningBLL().JCFlowData(Utility.GetCurrentSeasonForView());
                if (DataObj != null)
                {
                    lblShareEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ShareEKValue");
                    lblShareLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ShareLKValue");
                    lblShareTotal.Text = "-"; //Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblShareEK.Text) + Convert.ToDouble(lblShareLK.Text)));

                }

                //lblShortageEK.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblCanalAvailabilityEK.Text) / Convert.ToDouble(lblShareEK.Text)) * 100) - 100));
                //lblShortageLK.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblCanalAvailabilityLK.Text) / Convert.ToDouble(lblShareLK.Text)) * 100) - 100));
                //lblShortageTotal.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblCanalAvailabilityTotal.Text) / Convert.ToDouble(lblShareTotal.Text)) * 100) - 100));

                //lblShortageEK.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblCanalAvailabilityEK.Text) / Convert.ToDouble(lblShareEK.Text)) * 100) - 100));
                //lblShortageLK.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblCanalAvailabilityLK.Text) / Convert.ToDouble(lblShareLK.Text)) * 100) - 100));
                //lblShortageTotal.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblCanalAvailabilityTotal.Text) / Convert.ToDouble(lblShareTotal.Text)) * 100) - 100));

                lblShortageEK.Text = Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblCanalAvailabilityEK.Text) / Convert.ToDouble(lblShareEK.Text)) * 100) - 100));
                lblShortageLK.Text = Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblCanalAvailabilityLK.Text) / Convert.ToDouble(lblShareLK.Text)) * 100) - 100));
                lblShortageTotal.Text = "-"; //Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblCanalAvailabilityTotal.Text) / Convert.ToDouble(lblShareTotal.Text)) * 100) - 100));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        public void LoadAnticipatedJCRabiData()
        {
            try
            {
                object DataObj = new SeasonalPlanningBLL().AnticipatedRabiData(_ForecastDraftID, _Scenario, (int)Constants.RimStationsIDs.JhelumATMangla, _PlanDraftID);
                if (DataObj != null)
                {
                    lblJhelumAtManglaInflowsRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaRabiValue"))));
                    lblJhelumAtManglaProbabilityRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaRabiPercentValue") + " %";
                    lblChenabAtMarlaInflowsrabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaRabiValue"))));
                    lblChenabAtMarlaProbabilityRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaRabiPercentValue") + " %";

                }

                //double ERComp = Convert.ToDouble(new SeasonalPlanningBLL().GetEasternRabi((int)Constants.Seasons.Rabi, 5));
                //lblERComponentRabi.Text = string.Format("{0:0.000}", ERComp);
                object ERComponent = new SeasonalPlanningBLL().ERComponents(_PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                if (ERComponent != null)
                {
                    lblERComponentRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(ERComponent, "EasternRabi"))));
                }

                lblSubTotalRabi.Text = Convert.ToString(Convert.ToDouble(lblJhelumAtManglaInflowsRabi.Text) + Convert.ToDouble(lblChenabAtMarlaInflowsrabi.Text) + Convert.ToDouble(lblERComponentRabi.Text));

                DataObj = new SeasonalPlanningBLL().AnticipatedJCData(_PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                if (DataObj != null)
                {
                    lblManglaRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "StorageReleaseRabiValue");
                    lblTotalSystemInflowsRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSubTotalRabi.Text) + Convert.ToDouble(lblManglaRabi.Text)));
                    lblSystemLGRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "LossGainRabiValue");
                    TotalAvailabilityRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsRabi.Text) + Convert.ToDouble(lblSystemLGRabi.Text)));
                    lblJCOutFlowsRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "JCOutflowRabiValue");
                    lblCanalAvailabilityRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(TotalAvailabilityRabi.Text) - Convert.ToDouble(lblJCOutFlowsRabi.Text)));

                }
                DataObj = new SeasonalPlanningBLL().JCFlowData(Utility.GetCurrentSeasonForView());
                if (DataObj != null)
                {
                    lblShareRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "RabiShareValue");
                }

                //lblShortageRabi.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblCanalAvailabilityRabi.Text) / Convert.ToDouble(lblShareRabi.Text)) * 100) - 100));
                //lblShortageRabi.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblCanalAvailabilityRabi.Text) / Convert.ToDouble(lblShareRabi.Text)) * 100) - 100));
                lblShortageRabi.Text = Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblCanalAvailabilityRabi.Text) / Convert.ToDouble(lblShareRabi.Text)) * 100) - 100));
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

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.InflowForecasting);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }



    }

}