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
    public partial class AnticipatedIndusBasin : BasePage
    {
        public static long _ForecastDraftID;
        public static long _SeasonID = Utility.GetCurrentSeasonForView();
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
                    LoadAnticipatedIndusBasinData();
                }
                else
                {
                    divRabi.Style.Add("display", "block");
                    LoadAnticipatedIndusBasinRabiData();
                }
            }

        }
        public void LoadAnticipatedIndusBasinData()
        {

            try
            {
                object DataObj = new SeasonalPlanningBLL().AnticipatedData(_ForecastDraftID, _PlanDraftID, -1, _Scenario); // here -1 is a signal that we have to get both the rim station values 
                if (DataObj != null)
                {
                    lblIndusAtTarbelaInflowsEK.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusTarbelaEK");
                    lblIndusAtTarbelaInflowsLK.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusTarbelaLK");
                    lblIndusAtTarbelaInflowsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblIndusAtTarbelaInflowsEK.Text) + Convert.ToDouble(lblIndusAtTarbelaInflowsLK.Text)));
                    lblIndusAtTarbelaProbabilityEk.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusTerbelaEkPercent") + " %";
                    lblIndusAtTarbelaProbabilityLK.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusTerbelaLkPercent") + " %";

                    lblKabulAtNowsheraInflowsEK.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulNowsheraEK");
                    lblKabulAtNowsheraInflowsLK.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulNowsheraLK");
                    lblKabulAtNowsheraInflowsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblKabulAtNowsheraInflowsEK.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsLK.Text)));
                    lblKabulAtNowsheraProbabilityEK.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulNowsheraEkPercent") + " %";
                    lblKabulAtNowsheraProbabilityLK.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulNowsheraLkPercent") + " %";

                    lblJhelumAtManglaInflowssEK.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaEK");
                    lblJhelumAtManglaInflowssLK.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaLK");
                    lblJhelumAtManglaInflowssTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblJhelumAtManglaInflowssEK.Text) + Convert.ToDouble(lblJhelumAtManglaInflowssLK.Text)));
                    lblJhelumAtManglaProbabilityEK.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumEkPercent") + " %";
                    lblJhelumAtManglaProbabilityLK.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumLkPercent") + " %";

                    lblChenabAtMarlaInflowsEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaEK");
                    lblChenabAtMarlaInflowsLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaLK");
                    lblChenabAtMarlaInflowsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblChenabAtMarlaInflowsEK.Text) + Convert.ToDouble(lblChenabAtMarlaInflowsLK.Text)));
                    lblChenabAtMarlaProbabilityEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabEkPercent") + " %";
                    lblChenabAtMarlaProbabilityLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabLkPercent") + " %";


                    lblSubTotalRimStationEK.Text = Convert.ToString(Convert.ToDouble(lblIndusAtTarbelaInflowsEK.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsEK.Text));
                    lblSubTotalRimStationLK.Text = Convert.ToString(Convert.ToDouble(lblIndusAtTarbelaInflowsLK.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsLK.Text));
                    lblSubTotalRimStationTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblIndusAtTarbelaInflowsTotal.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsTotal.Text)));
                }
                #region ER Components

                object ERComponent = new SeasonalPlanningBLL().ERComponents(_PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                if (ERComponent != null)
                {

                    lblERComponentsEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(ERComponent, "EasternEK"))));
                    lblERComponentsLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(ERComponent, "EasternLK"))));
                    lblERComponentsTotal.Text = Convert.ToString(string.Format("{0:0.000}", (Convert.ToDouble(lblERComponentsEK.Text) + Convert.ToDouble(lblERComponentsLK.Text))));
                }
                //object EasternDataObj = new SeasonalPlanningBLL().GetEastern((int)Constants.Seasons.Kharif, 5);
                //if (EasternDataObj != null)
                //{

                //    double EK = Convert.ToDouble(Utility.GetDynamicPropertyValue(EasternDataObj, "EK"));
                //    double LK = Convert.ToDouble(Utility.GetDynamicPropertyValue(EasternDataObj, "LK"));

                //    lblERComponentsEK.Text = string.Format("{0:0.000}", EK);
                //    lblERComponentsLK.Text = string.Format("{0:0.000}", LK);
                //    lblERComponentsTotal.Text = Convert.ToString(Convert.ToDouble(lblERComponentsEK.Text) + Convert.ToDouble(lblERComponentsLK.Text));
                //}

                #endregion ER Components

                #region ER Components

                lblSubTotalRimStationEK.Text = Convert.ToString(Convert.ToDouble(lblIndusAtTarbelaInflowsEK.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsEK.Text) + Convert.ToDouble(lblJhelumAtManglaInflowssEK.Text) + Convert.ToDouble(lblChenabAtMarlaInflowsEK.Text) + Convert.ToDouble(lblERComponentsEK.Text));
                lblSubTotalRimStationLK.Text = Convert.ToString(Convert.ToDouble(lblIndusAtTarbelaInflowsLK.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsLK.Text) + Convert.ToDouble(lblJhelumAtManglaInflowssLK.Text) + Convert.ToDouble(lblChenabAtMarlaInflowsLK.Text) + Convert.ToDouble(lblERComponentsLK.Text));
                lblSubTotalRimStationTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblIndusAtTarbelaInflowsTotal.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsTotal.Text) + Convert.ToDouble(lblJhelumAtManglaInflowssTotal.Text) + Convert.ToDouble(lblChenabAtMarlaInflowsTotal.Text) + Convert.ToDouble(lblERComponentsTotal.Text)));

                #endregion ER Components
                object JCDataObj = new SeasonalPlanningBLL().AnticipatedJCData(_PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                if (JCDataObj != null)
                {
                    lblManglaEK.Text = Utility.GetDynamicPropertyValue(JCDataObj, "StorageReleaseEK");
                    lblManglaLK.Text = Utility.GetDynamicPropertyValue(JCDataObj, "StorageReleaseLK");
                    lblManglaTotal.Text = Convert.ToString(Convert.ToDouble(lblManglaEK.Text) + Convert.ToDouble(lblManglaLK.Text));

                    var lossgainEK = Utility.GetDynamicPropertyValue(JCDataObj, "LossGainEK");
                    var lossgainLK = Utility.GetDynamicPropertyValue(JCDataObj, "LossGainLK");

                }

                object IKDataObj = new SeasonalPlanningBLL().AnticipatedIKData(_PlanDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, _Scenario);
                if (IKDataObj != null)
                {
                    lblTarbelaEK.Text = Utility.GetDynamicPropertyValue(IKDataObj, "StorageReleaseEK");
                    lblTarbelaLK.Text = Utility.GetDynamicPropertyValue(IKDataObj, "StorageReleaseLK");
                    lblTarbelaTotal.Text = Convert.ToString(Convert.ToDouble(lblTarbelaEK.Text) + Convert.ToDouble(lblTarbelaLK.Text));

                    lblChashmaEK.Text = Utility.GetDynamicPropertyValue(IKDataObj, "ChashmaStorageReleaseEK");
                    lblChashmaLK.Text = Utility.GetDynamicPropertyValue(IKDataObj, "ChashmaStorageReleaseLK");
                    lblChashmaTotal.Text = Convert.ToString(Convert.ToDouble(lblChashmaEK.Text) + Convert.ToDouble(lblChashmaLK.Text));

                    lblSubTotalStorageReleaseEK.Text = Convert.ToString(Convert.ToDouble(lblTarbelaEK.Text) + Convert.ToDouble(lblManglaEK.Text) + Convert.ToDouble(lblChashmaEK.Text));
                    lblSubTotalStorageReleaseLK.Text = Convert.ToString(Convert.ToDouble(lblTarbelaLK.Text) + Convert.ToDouble(lblManglaLK.Text) + Convert.ToDouble(lblChashmaLK.Text));
                    lblSubTotalStorageReleaseTotal.Text = Convert.ToString(Convert.ToDouble(lblTarbelaTotal.Text) + Convert.ToDouble(lblManglaTotal.Text) + Convert.ToDouble(lblChashmaTotal.Text));

                    lblTotalSystemInflowsEK.Text = Convert.ToString(Convert.ToDouble(lblSubTotalRimStationEK.Text) + Convert.ToDouble(lblSubTotalStorageReleaseEK.Text));
                    lblTotalSystemInflowsLK.Text = Convert.ToString(Convert.ToDouble(lblSubTotalRimStationLK.Text) + Convert.ToDouble(lblSubTotalStorageReleaseLK.Text));
                    lblTotalSystemInflowsTotal.Text = Convert.ToString(Convert.ToDouble(lblSubTotalRimStationTotal.Text) + Convert.ToDouble(lblSubTotalStorageReleaseTotal.Text));

                    lblSystemLossesGainsEK.Text = Convert.ToString(Convert.ToDouble(Utility.GetDynamicPropertyValue(JCDataObj, "LossGainEK")) + Convert.ToDouble(Utility.GetDynamicPropertyValue(IKDataObj, "LossGainEK")));
                    lblSystemLossesGainsLK.Text = Convert.ToString(Convert.ToDouble(Utility.GetDynamicPropertyValue(JCDataObj, "LossGainLK")) + Convert.ToDouble(Utility.GetDynamicPropertyValue(IKDataObj, "LossGainLK")));
                    lblSystemLossesGainsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSystemLossesGainsEK.Text) + Convert.ToDouble(lblSystemLossesGainsLK.Text)));

                    lblTotaAvailabilityEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsEK.Text) + Convert.ToDouble(lblSystemLossesGainsEK.Text)));
                    lblTotaAvailabilityLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsLK.Text) + Convert.ToDouble(lblSystemLossesGainsLK.Text)));
                    lblTotaAvailabilityTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsTotal.Text) + Convert.ToDouble(lblSystemLossesGainsTotal.Text)));

                    lblKotriBelowEK.Text = Utility.GetDynamicPropertyValue(IKDataObj, "KotriBelowEK");
                    lblKotriBelowLK.Text = Utility.GetDynamicPropertyValue(IKDataObj, "KotriBelowLK");
                    lblKotriBelowEKTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblKotriBelowEK.Text) + Convert.ToDouble(lblKotriBelowLK.Text)));

                    lblCanalAvailabilityEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotaAvailabilityEK.Text) - Convert.ToDouble(lblKotriBelowEK.Text)));
                    lblCanalAvailabilityLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotaAvailabilityLK.Text) - Convert.ToDouble(lblKotriBelowLK.Text)));
                    lblCanalAvailabilityTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotaAvailabilityTotal.Text) - Convert.ToDouble(lblKotriBelowEKTotal.Text)));
                }

                object JCFlowDataObj = new SeasonalPlanningBLL().JCFlowData(Utility.GetCurrentSeasonForView());
                if (JCFlowDataObj != null)
                {

                    lblLessKPKplusBalochistanEK.Text = Utility.GetDynamicPropertyValue(JCFlowDataObj, "LesskPPlusBlochEK");
                    lblLessKPKplusBalochistanLK.Text = Utility.GetDynamicPropertyValue(JCFlowDataObj, "LesskPPlusBlochLK");
                    lblLessKPKplusBalochistanTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblLessKPKplusBalochistanEK.Text) + Convert.ToDouble(lblLessKPKplusBalochistanLK.Text)));

                    lblBalanceforPunjabAndSindhEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblCanalAvailabilityEK.Text) - Convert.ToDouble(lblLessKPKplusBalochistanEK.Text)));
                    lblBalanceforPunjabAndSindhLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblCanalAvailabilityLK.Text) - Convert.ToDouble(lblLessKPKplusBalochistanLK.Text)));
                    lblBalanceforPunjabAndSindhTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblCanalAvailabilityTotal.Text) - Convert.ToDouble(lblLessKPKplusBalochistanTotal.Text)));

                    bool? IsPara = new SeasonalPlanningBLL().GetParaHistoricBit(_PlanDraftID);

                    if ((bool)IsPara)
                        lblShareofPunjabAndSindhEK.Text = Utility.GetDynamicPropertyValue(JCFlowDataObj, "ShareOfJCAndIKEKPara");
                    else
                        lblShareofPunjabAndSindhEK.Text = Utility.GetDynamicPropertyValue(JCFlowDataObj, "ShareOfJCAndIKEK");

                    lblShareofPunjabAndSindhLK.Text = Utility.GetDynamicPropertyValue(JCFlowDataObj, "ShareOfJCAndIKLK");
                    lblShareofPunjabAndSindhTotal.Text = "-"; //Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblShareofPunjabAndSindhEK.Text) + Convert.ToDouble(lblShareofPunjabAndSindhLK.Text)));

                }

                //lblShortageEK.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblBalanceforPunjabAndSindhEK.Text) / Convert.ToDouble(lblShareofPunjabAndSindhEK.Text)) * 100) - 100));
                //lblShortageLK.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblBalanceforPunjabAndSindhLK.Text) / Convert.ToDouble(lblShareofPunjabAndSindhLK.Text)) * 100) - 100));
                //lblShortageTotal.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblBalanceforPunjabAndSindhTotal.Text) / Convert.ToDouble(lblShareofPunjabAndSindhTotal.Text)) * 100) - 100));

                //lblShortageEK.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblBalanceforPunjabAndSindhEK.Text) / Convert.ToDouble(lblShareofPunjabAndSindhEK.Text)) * 100) - 100));
                //lblShortageLK.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblBalanceforPunjabAndSindhLK.Text) / Convert.ToDouble(lblShareofPunjabAndSindhLK.Text)) * 100) - 100));
                //lblShortageTotal.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblBalanceforPunjabAndSindhTotal.Text) / Convert.ToDouble(lblShareofPunjabAndSindhTotal.Text)) * 100) - 100));

                lblShortageEK.Text = Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblBalanceforPunjabAndSindhEK.Text) / Convert.ToDouble(lblShareofPunjabAndSindhEK.Text)) * 100) - 100));
                lblShortageLK.Text = Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblBalanceforPunjabAndSindhLK.Text) / Convert.ToDouble(lblShareofPunjabAndSindhLK.Text)) * 100) - 100));
                lblShortageTotal.Text = "-"; //Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblBalanceforPunjabAndSindhTotal.Text) / Convert.ToDouble(lblShareofPunjabAndSindhTotal.Text)) * 100) - 100));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void LoadAnticipatedIndusBasinRabiData()
        {

            try
            {
                object DataObj = new SeasonalPlanningBLL().AnticipatedRabiData(_ForecastDraftID, _Scenario, -1, _PlanDraftID);
                if (DataObj != null)
                {


                    lblJhelumAtManglaInflowsRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaRabiValue"))));
                    lblJhelumAtManglaProbabilityRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaRabiPercentValue") + " %";
                    lblChenabAtMarlaInflowsIBRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaRabiValue"))));
                    lblChenabAtMarlaProbabilityIBRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaRabiPercentValue") + " %";
                    lblIndusAtTarbelaInflowsIBRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "IndusTarbelaRabiValue"))));
                    lblIndusAtTarbelaProbabilityIBRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusTarbelaRabiPercentvalue") + " %";
                    lblKabulAtNowsheraInflowsIBRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "KabulNowsheraRabiValue"))));
                    lblKabulAtNowsheraProbabilityIBRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulNowsheraRabiPercentValue") + " %";


                    //double ERComp = Convert.ToDouble(new SeasonalPlanningBLL().GetEasternRabi((int)Constants.Seasons.Rabi, 5));
                    //lblERComponentsIBRabi.Text = string.Format("{0:0.000}", ERComp);

                    object ERComponent = new SeasonalPlanningBLL().ERComponents(_PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                    if (ERComponent != null)
                    {
                        lblERComponentsIBRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(ERComponent, "EasternRabi"))));
                    }


                    lblSubTotalRimStationIBRabi.Text = Convert.ToString(Convert.ToDouble(lblJhelumAtManglaInflowsRabi.Text) + Convert.ToDouble(lblChenabAtMarlaInflowsIBRabi.Text) + Convert.ToDouble(lblIndusAtTarbelaInflowsIBRabi.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsIBRabi.Text) + Convert.ToDouble(lblERComponentsIBRabi.Text));
                }
                object JCDataObj = new SeasonalPlanningBLL().AnticipatedJCData(_PlanDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                if (JCDataObj != null)
                {
                    lblManglaIBRabi.Text = Utility.GetDynamicPropertyValue(JCDataObj, "StorageReleaseRabiValue");
                }
                object IKDataObj = new SeasonalPlanningBLL().AnticipatedIKData(_PlanDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, _Scenario);
                if (IKDataObj != null)
                {
                    lblTarbelaRabi.Text = Utility.GetDynamicPropertyValue(IKDataObj, "StorageReleaseRabi");
                    lblChashmatRabi.Text = Utility.GetDynamicPropertyValue(IKDataObj, "ChashmaStorageReleaseRabi");
                    lblSubTotalTerbelaChashmaAndManglaRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTarbelaRabi.Text) + Convert.ToDouble(lblManglaIBRabi.Text) + Convert.ToDouble(lblChashmatRabi.Text)));
                    lblTotalSystemInflowsIBRabi.Text = Convert.ToString(Convert.ToDouble(lblSubTotalRimStationIBRabi.Text) + Convert.ToDouble(lblSubTotalTerbelaChashmaAndManglaRabi.Text));
                    lblSystemLGIKRabi.Text = Convert.ToString(Convert.ToDouble(Utility.GetDynamicPropertyValue(JCDataObj, "LossGainRabiValue")) + Convert.ToDouble(Utility.GetDynamicPropertyValue(IKDataObj, "LossGainRabi")));
                    TotalAvailabilityIBRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsIBRabi.Text) + Convert.ToDouble(lblSystemLGIKRabi.Text)));
                    lblKotriBelowIKRabi.Text = Utility.GetDynamicPropertyValue(IKDataObj, "KotriBelowRabi");
                    lblCanalAvailabilityIBRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(TotalAvailabilityIBRabi.Text) - Convert.ToDouble(lblKotriBelowIKRabi.Text)));

                }
                object JCFlowDataObj = new SeasonalPlanningBLL().JCFlowData(Utility.GetCurrentSeasonForView());
                if (JCFlowDataObj != null)
                {
                    lblLessKPKPlusBalochistanIBRabi.Text = Utility.GetDynamicPropertyValue(JCFlowDataObj, "LesskPPlusBlochRabiValue");
                    lblBalanceforPunjabandSindhIBRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblCanalAvailabilityIBRabi.Text) - Convert.ToDouble(lblLessKPKPlusBalochistanIBRabi.Text)));
                    lblShareofPunjabandSindhIBRabi.Text = Utility.GetDynamicPropertyValue(JCFlowDataObj, "ShareOfJCAndIKRabiValue");
                }
                //lblShortageIBRabi.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblBalanceforPunjabandSindhIBRabi.Text) / Convert.ToDouble(lblShareofPunjabandSindhIBRabi.Text)) * 100) - 100));
                //lblShortageIBRabi.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblBalanceforPunjabandSindhIBRabi.Text) / Convert.ToDouble(lblShareofPunjabandSindhIBRabi.Text)) * 100) - 100));
                lblShortageIBRabi.Text = Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblBalanceforPunjabandSindhIBRabi.Text) / Convert.ToDouble(lblShareofPunjabandSindhIBRabi.Text)) * 100) - 100));
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