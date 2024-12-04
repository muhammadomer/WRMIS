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
    public partial class AnticipatedIndusRiver : BasePage
    {
        public static long _ForecastDraftID;
        // public static long _SeasonID = Utility.GetCurrentSeasonForView();
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
                    LoadAnticipatedIndusRiverData();
                }
                else
                {
                    divRabi.Style.Add("display", "block");
                    LoadAnticipatedIndusRiverRabiData();
                }
            }

        }

        public void LoadAnticipatedIndusRiverData()
        {

            try
            {
                object DataObj = new SeasonalPlanningBLL().AnticipatedData(_ForecastDraftID, _PlanDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, _Scenario);
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

                    lblSubTotalRimStationEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblIndusAtTarbelaInflowsEK.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsEK.Text)));
                    lblSubTotalRimStationLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblIndusAtTarbelaInflowsLK.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsLK.Text)));
                    lblSubTotalRimStationTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblIndusAtTarbelaInflowsTotal.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsTotal.Text)));
                }

                DataObj = new SeasonalPlanningBLL().AnticipatedIKData(_PlanDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, _Scenario);
                if (DataObj != null)
                {
                    lblTarbelaEK.Text = Utility.GetDynamicPropertyValue(DataObj, "StorageReleaseEK");
                    lblTarbelaLK.Text = Utility.GetDynamicPropertyValue(DataObj, "StorageReleaseLK");
                    lblTarbelaTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTarbelaEK.Text) + Convert.ToDouble(lblTarbelaLK.Text)));

                    lblChashmaEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ChashmaStorageReleaseEK");
                    lblChashmaLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ChashmaStorageReleaseLK");
                    lblChashmaTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblChashmaEK.Text) + Convert.ToDouble(lblChashmaLK.Text)));

                    lblSubTotalStorageReleaseEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTarbelaEK.Text) + Convert.ToDouble(lblChashmaEK.Text)));
                    lblSubTotalStorageReleaseLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTarbelaLK.Text) + Convert.ToDouble(lblChashmaLK.Text)));
                    lblSubTotalStorageReleaseTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTarbelaTotal.Text) + Convert.ToDouble(lblChashmaTotal.Text)));

                    lblJCOutflowITEK.Text = Utility.GetDynamicPropertyValue(DataObj, "JCOutflowEK");
                    lblJCOutflowITLK.Text = Utility.GetDynamicPropertyValue(DataObj, "JCOutflowLK");
                    lblJCOutflowITTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblJCOutflowITEK.Text) + Convert.ToDouble(lblJCOutflowITLK.Text)));

                    lblTotalSystemInflowsEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSubTotalRimStationEK.Text) + Convert.ToDouble(lblSubTotalStorageReleaseEK.Text) + Convert.ToDouble(lblJCOutflowITEK.Text)));
                    lblTotalSystemInflowsLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSubTotalRimStationLK.Text) + Convert.ToDouble(lblSubTotalStorageReleaseLK.Text) + Convert.ToDouble(lblJCOutflowITLK.Text)));
                    lblTotalSystemInflowsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSubTotalRimStationTotal.Text) + Convert.ToDouble(lblSubTotalStorageReleaseTotal.Text) + Convert.ToDouble(lblJCOutflowITTotal.Text)));

                    lblSystemLossesGainsEK.Text = Utility.GetDynamicPropertyValue(DataObj, "LossGainEK");
                    lblSystemLossesGainsLK.Text = Utility.GetDynamicPropertyValue(DataObj, "LossGainLK");
                    lblSystemLossesGainsTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSystemLossesGainsEK.Text) + Convert.ToDouble(lblSystemLossesGainsLK.Text)));

                    lblTotaAvailabilityEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsEK.Text) + Convert.ToDouble(lblSystemLossesGainsEK.Text)));
                    lblTotaAvailabilityLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsLK.Text) + Convert.ToDouble(lblSystemLossesGainsLK.Text)));
                    lblTotaAvailabilityTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsTotal.Text) + Convert.ToDouble(lblSystemLossesGainsTotal.Text)));

                    lblKotriBelowEK.Text = Utility.GetDynamicPropertyValue(DataObj, "KotriBelowEK");
                    lblKotriBelowLK.Text = Utility.GetDynamicPropertyValue(DataObj, "KotriBelowLK");
                    lblKotriBelowEKTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblKotriBelowEK.Text) + Convert.ToDouble(lblKotriBelowLK.Text)));

                    lblCanalAvailabilityEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotaAvailabilityEK.Text) - Convert.ToDouble(lblKotriBelowEK.Text)));
                    lblCanalAvailabilityLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotaAvailabilityLK.Text) - Convert.ToDouble(lblKotriBelowLK.Text)));
                    lblCanalAvailabilityTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotaAvailabilityTotal.Text) - Convert.ToDouble(lblKotriBelowEKTotal.Text)));
                }

                bool? IsPara = new SeasonalPlanningBLL().GetParaHistoricBit(_PlanDraftID);

                GetFlows(IsPara);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void GetFlows(bool? _IsPara)
        {
            try
            {
                object DataObj = new SeasonalPlanningBLL().JCFlowData(Utility.GetCurrentSeasonForView());
                if (DataObj != null)
                {
                    lblLessKPKplusBalochistanEK.Text = Utility.GetDynamicPropertyValue(DataObj, "LesskPPlusBlochEK");
                    lblLessKPKplusBalochistanLK.Text = Utility.GetDynamicPropertyValue(DataObj, "LesskPPlusBlochLK");
                    lblLessKPKplusBalochistanTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblLessKPKplusBalochistanEK.Text) + Convert.ToDouble(lblLessKPKplusBalochistanLK.Text)));

                    lblBalanceforPunjabAndSindhEK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblCanalAvailabilityEK.Text) - Convert.ToDouble(lblLessKPKplusBalochistanEK.Text)));
                    lblBalanceforPunjabAndSindhLK.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblCanalAvailabilityLK.Text) - Convert.ToDouble(lblLessKPKplusBalochistanLK.Text)));
                    lblBalanceforPunjabAndSindhTotal.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblCanalAvailabilityTotal.Text) - Convert.ToDouble(lblLessKPKplusBalochistanTotal.Text)));

                    if ((bool)_IsPara)
                        lblShareofPunjabAndSindhEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ShareOfPAndSEKPara");
                    else
                        lblShareofPunjabAndSindhEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ShareOfPAndSEK");
                    lblShareofPunjabAndSindhLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ShareOfPAndSLK");
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

                cbPara.Checked = (bool)_IsPara;
                new SeasonalPlanningBLL().SaveParahistoricBit(_PlanDraftID, (bool)_IsPara);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void LoadAnticipatedIndusRiverRabiData()
        {
            try
            {
                object DataObj = new SeasonalPlanningBLL().AnticipatedRabiData(_ForecastDraftID, _Scenario, (int)Constants.RimStationsIDs.IndusAtTarbela, _PlanDraftID);
                if (DataObj != null)
                {
                    lblIndusAtTarbelaInflowsRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "IndusTarbelaRabiValue"))));
                    lblIndusAtTarbelaProbabilityRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusTarbelaRabiPercentvalue") + " %";
                    lblKabulAtNowsheraInflowsRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(Utility.GetDynamicPropertyValue(DataObj, "KabulNowsheraRabiValue"))));
                    lblKabulAtNowsheraProbabilityRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulNowsheraRabiPercentValue") + " %";

                    lblSubTotalIndusAndKabulRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblIndusAtTarbelaInflowsRabi.Text) + Convert.ToDouble(lblKabulAtNowsheraInflowsRabi.Text)));

                }
                DataObj = new SeasonalPlanningBLL().AnticipatedIKData(_PlanDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, _Scenario);
                if (DataObj != null)
                {
                    lblTarbelaRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "StorageReleaseRabi");
                    lblChashmatRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "ChashmaStorageReleaseRabi");
                    lblSubTotalTerbelaAndChashmaRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTarbelaRabi.Text) + Convert.ToDouble(lblChashmatRabi.Text)));
                    lblJCOutFlowsIKRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "JCOutflowRabi");
                    lblTotalSystemInflowsIKRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblSubTotalIndusAndKabulRabi.Text) + Convert.ToDouble(lblSubTotalTerbelaAndChashmaRabi.Text) + Convert.ToDouble(lblJCOutFlowsIKRabi.Text)));
                    lblSystemLGIKRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "LossGainRabi");
                    TotalAvailabilityIKRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblTotalSystemInflowsIKRabi.Text) + Convert.ToDouble(lblSystemLGIKRabi.Text)));
                    lblKotriBelowIKRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "KotriBelowRabi");
                    lblCanalAvailabilityIKRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(TotalAvailabilityIKRabi.Text) - Convert.ToDouble(lblKotriBelowIKRabi.Text)));
                }

                DataObj = new SeasonalPlanningBLL().JCFlowData(Utility.GetCurrentSeasonForView());
                if (DataObj != null)
                {
                    lblLessKPKPlusBalochistanIKRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "LesskPPlusBlochRabiValue");
                    lblBalanceforPunjabandSindhIKRabi.Text = Convert.ToString(string.Format("{0:0.000}", Convert.ToDouble(lblCanalAvailabilityIKRabi.Text) - Convert.ToDouble(lblLessKPKPlusBalochistanIKRabi.Text)));
                    lblShareofPunjabandSindhIKRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "ShareOfPAndSRabiValue");
                }
                //lblShortageIKRabi.Text = Convert.ToString(Math.Round(((Convert.ToDouble(lblBalanceforPunjabandSindhIKRabi.Text) / Convert.ToDouble(lblShareofPunjabandSindhIKRabi.Text)) * 100) - 100));
                //lblShortageIKRabi.Text = Convert.ToString(string.Format("{0:0.000}", ((Convert.ToDouble(lblBalanceforPunjabandSindhIKRabi.Text) / Convert.ToDouble(lblShareofPunjabandSindhIKRabi.Text)) * 100) - 100));
                lblShortageIKRabi.Text = Convert.ToString(string.Format("{0:0}", ((Convert.ToDouble(lblBalanceforPunjabandSindhIKRabi.Text) / Convert.ToDouble(lblShareofPunjabandSindhIKRabi.Text)) * 100) - 100));
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

        protected void cbPara_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GetFlows(cbPara.Checked);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}