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
    public partial class SeasonalPlanning : BasePage
    {
        #region Variables

        public static long SPDraftID;
        public static long ForecastDraftID;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();

                    if (Request.QueryString["Call"] != null && Request.QueryString["Call"].ToString() == "1")
                    {
                        btnAddNew.Visible = false;
                        divPanel.Visible = true;
                        if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                            SetKharifBalanceReservoirFields(ManglaIndusOperations.ForecastDraftID, ManglaIndusOperations.Scenario);
                        else
                            SetRabiBalanceReservoirFields(ManglaIndusOperations.ForecastDraftID, ManglaIndusOperations.Scenario);
                    }
                    else
                    {
                        BindSeasonalPlanningDrafts();
                        if (!Utility.PlanningDaysLimit())
                            btnAddNew.Visible = false;
                        else
                            btnAddNew.Visible = true;
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

        #region Balance Reservoir For Kharif

        public void EditViewKharifTrue()
        {
            try
            {
                txtJCManglaLevel.Enabled = true;
                txtTarbelaLevel.Enabled = true;
                txtFillingLimit.Enabled = true;
                txtChashmaLevel.Enabled = true;
                txtChashmaMinLevel.Enabled = true;
                txtEasternEK.Enabled = true;
                txtEasternLK.Enabled = true;
                txtJCInitialStorage.Enabled = true;
                txtJCStrToFillEK.Enabled = true;
                txtJCStrToFillLK.Enabled = true;
                txtICInitialStorage.Enabled = true;
                txtICStrToFillEK.Enabled = true;
                txtICStrToFillLK.Enabled = true;
                txtJCStrDep.Enabled = true;
                txtICStrDep.Enabled = true;
                txtJCSysLossGainPerEK.Enabled = true;
                txtJCSysLossGainPerLK.Enabled = true;
                txtICSysLossGainPerEK.Enabled = true;
                txtICSysLossGainPerLK.Enabled = true;
                txtJCCanalAvailEK.Enabled = true;
                txtJCCanalAvailLK.Enabled = true;
                txtICCanalAvailEK.Enabled = true;
                txtICCanalAvailLK.Enabled = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public String ConvertToThreeDecimalPlaces(String _Value)
        {
            return String.Format("{0:0.000}", Convert.ToDouble(_Value));
        }

        public String ConvertToTwoDecimalPlaces(String _Value)
        {
            return String.Format("{0:0.00}", Convert.ToDouble(_Value));
        }

        public String ConvertToThreeDecimalPlaces(Double? _Value)
        {
            return String.Format("{0:0.000}", _Value);
        }

        public String ConvertToTwoDecimalPlaces(Double? _Value)
        {
            return String.Format("{0:0.00}", _Value);
        }

        public void SetKharifSavedFields(SP_PlanBalance BalanceObj, long _IFDraftID, string _Scenario)
        {
            try
            {
                if (BalanceObj != null)
                {
                    List<SP_ForecastScenario> lstPercentages = new SeasonalPlanningBLL().GetForecastPercentages(_IFDraftID, _Scenario);

                    //Jhelum Command
                    txtJCManglaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.MCL);
                    lblManglaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.Storage);
                    if (_Scenario == "Likely")
                    {
                        lblJMEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumEK);
                        lblJMLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumLK);
                        lblCMEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChenabEK);
                        lblCMLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChenabLK);
                    }
                    else
                    {
                        lblJMEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumEK) + " (" + lstPercentages.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().EkPercent + "%)";
                        lblJMLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumLK) + " (" + lstPercentages.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault().LkPercent + "%)";
                        lblCMEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChenabEK) + " (" + lstPercentages.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().EkPercent + "%)";
                        lblCMLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChenabLK) + " (" + lstPercentages.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala).FirstOrDefault().LkPercent + "%)";
                    }

                    lblJMTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Jhelum);
                    lblCMTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Chenab);
                    ddlKharifEastern.ClearSelection();
                    if (_Scenario != "Likely")
                        ddlKharifEastern.Items.FindByText(Convert.ToString(BalanceObj.EasternYears)).Selected = true;
                    txtEasternEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.EasternEK);
                    txtEasternLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.EasternLK);
                    lblEasternTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Eastern);
                    lblJCTotalEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumEK + BalanceObj.ChenabEK + BalanceObj.EasternEK);
                    lblJCTotalLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumLK + BalanceObj.ChenabLK + BalanceObj.EasternLK);
                    lblJCTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Jhelum + BalanceObj.Chenab + BalanceObj.Eastern);
                    lblJCInitialLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.InitResLevel);
                    txtJCInitialStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.InitStorage);
                    txtJCStrToFillEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.StrtofillEK);
                    txtJCStrToFillLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.StrtofillLK);
                    lblJCStrFillEKMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsEK);
                    lblJCStrFillLKMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsLK);
                    lblJCStrFillTotalMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsEK + BalanceObj.FlowsLK);
                    txtJCStrDep.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageDep);
                    lblJCStrRel.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageRelease);
                    lblJCSysInflowEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK);
                    lblJCSysInflowLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsLK);
                    lblJCSysInflowTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK + BalanceObj.SysInflowsLK);
                    txtJCSysLossGainPerEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.EarlyKharif);
                    txtJCSysLossGainPerLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.LateKharif);
                    lblJCSysLossGainMAFEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK * (BalanceObj.EarlyKharif / 100));
                    lblJCSysLossGainMAFLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsLK * (BalanceObj.LateKharif / 100));
                    lblJCWaterAvailEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailabilityEK);
                    lblJCWaterAvailLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailabilityLK);
                    lblJCWaterAvailTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailability);
                    lblJCParaEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2EK);
                    lblJCParaLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2LK);
                    lblJCParaTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2);
                    txtJCCanalAvailEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailabilityEK);
                    txtJCCanalAvailLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailabilityLK);
                    lblJCCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailability);
                    lblJCOutflowEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflowEK);
                    lblJCOutflowLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCoutflowLK);
                    lblJCOutflowTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflow);
                    lblJCShortageEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ShortageEK);
                    lblJCShortageLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ShortageLK);
                    lblJCShortageTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Shortage);

                    //Indus command  

                    BalanceObj = new SeasonalPlanningBLL().GetExistingBalanceData(SPDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, _Scenario);

                    txtTarbelaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.MCL);
                    lblTarbelaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.Storage);
                    txtFillingLimit.Text = ConvertToTwoDecimalPlaces(BalanceObj.FillingLimit);
                    lblFillingStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.FillingLimitStorage);
                    txtChashmaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.ChashmaResLevel);
                    lblChashmaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChashmaStorage);
                    txtChashmaMinLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.ChashmaMinReslevel);
                    lblChashmaMinStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChashmaMinStorage);
                    if (_Scenario == "Likely")
                    {
                        lblITEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusEK);
                        lblITLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusLK);
                        lblKNEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KabulEK);
                        lblKNLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KabulLK);
                    }
                    else
                    {
                        lblITEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusEK) + " (" + lstPercentages.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().EkPercent + "%)"; //lstPercentages.ElementAt(3).EkPercent + "%)";
                        lblITLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusLK) + " (" + lstPercentages.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault().LkPercent + "%)";
                        lblKNEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KabulEK) + " (" + lstPercentages.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().EkPercent + "%)";
                        lblKNLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KabulLK) + " (" + lstPercentages.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera).FirstOrDefault().LkPercent + "%)";
                    }

                    lblITTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Indus);
                    lblKNotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Kabul);
                    lblICTotalEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusEK + BalanceObj.KabulEK);
                    lblICTotalLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusLK + BalanceObj.KabulLK);
                    lblICTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Indus + BalanceObj.Kabul);
                    lblICInitialLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.InitResLevel);
                    txtICInitialStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.InitStorage);
                    txtICStrToFillEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.StrtofillEK);
                    txtICStrToFillLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.StrtofillLK);
                    lblICStrFillEKMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsEK);
                    lblICStrFillLKMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsLK);
                    lblICStrFillTotalMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsEK + BalanceObj.FlowsLK);
                    txtICStrDep.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageDep);
                    lblICStrRel.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageRelease);
                    lblICSysInflowEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK);
                    lblICSysInflowLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsLK);
                    lblICSysInflowTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK + BalanceObj.SysInflowsLK);
                    txtICSysLossGainPerEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.EarlyKharif);
                    txtICSysLossGainPerLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.LateKharif);
                    lblICSysLossGainMAFEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK * (BalanceObj.EarlyKharif / 100));
                    lblICSysLossGainMAFLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsLK * (BalanceObj.LateKharif / 100));
                    lblICWaterAvailEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailabilityEK);
                    lblICWaterAvailLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailabilityLK);
                    lblICWaterAvailTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailability);
                    lblKPKBalEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KPKBalochistanEK);
                    lblKPKBalLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KPKBalochistanLK);
                    lblKPKBalTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.KPKBalochistan);
                    lblICParaEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2EK);
                    lblICParaLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2LK);
                    lblICParaTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2);
                    lblPunjSindhEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.BalPunSindhEK);
                    lblPunjSindhLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.BalPunsindhLK);
                    lblPunjSindhTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.BalPunSindh);
                    txtICCanalAvailEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailabilityEK);
                    txtICCanalAvailLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailabilityLK);
                    lblICCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailability);
                    lblICOutflowEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflowEK);
                    lblICOutflowLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCoutflowLK);
                    lblICOutflowTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflow);
                    lblKotriEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KotriEK);
                    lblKotriLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KotriLK);
                    lblKotriTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Kotri);
                    lblICShortageEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ShortageEK);
                    lblICShortageLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ShortageLK);
                    lblICShortageTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Shortage);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetKharifBalanceReservoirFields(long _IFDraftID, string _Scenario)
        {
            object DataObj = new object();
            divKharif.Visible = true;
            lblScenarioName.Text = _Scenario + " Scenario";
            htitle.InnerText = "Seasonal Planning-" + _Scenario + " Scenario";
            try
            {
                if (_Scenario == "Likely")
                {
                    SetLikelyFieldsKharif(_IFDraftID, (int)Constants.Seasons.Kharif);
                }
                else
                {
                    ddlKharifEastern.DataSource = CommonLists.GetEasternYears();
                    ddlKharifEastern.DataTextField = "Name";
                    ddlKharifEastern.DataValueField = "ID";
                    ddlKharifEastern.DataBind();

                    ddlRabiEastern.DataSource = CommonLists.GetEasternYears();
                    ddlRabiEastern.DataTextField = "Name";
                    ddlRabiEastern.DataValueField = "ID";
                    ddlRabiEastern.DataBind();

                    SP_PlanBalance BalanceObj = new SeasonalPlanningBLL().GetExistingBalanceData(SPDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                    if (BalanceObj != null)
                        SetKharifSavedFields(BalanceObj, _IFDraftID, _Scenario);
                    else
                    {
                        DataObj = new SeasonalPlanningBLL().GetElevationCapacitiesForBalanceReservoir((long)Constants.ReservoirLevels.Mangla, (long)Constants.ReservoirLevels.Tarbela, (long)Constants.ReservoirLevels.TarbelaFillingLimit, (long)Constants.ReservoirLevels.Chashma, (long)Constants.ReservoirLevels.ChashmaMinOptLevel);
                        if (DataObj != null)
                        {
                            txtJCManglaLevel.Text = ConvertToTwoDecimalPlaces(((int)Constants.ReservoirLevels.Mangla).ToString());
                            lblManglaStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "ManglaStorage");

                            txtTarbelaLevel.Text = ConvertToTwoDecimalPlaces(((int)Constants.ReservoirLevels.Tarbela).ToString());
                            lblTarbelaStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "TarbelaStorage");

                            txtFillingLimit.Text = ConvertToTwoDecimalPlaces(((int)Constants.ReservoirLevels.TarbelaFillingLimit).ToString());
                            lblFillingStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "TarbelaFillLimitStorage");

                            txtChashmaLevel.Text = ConvertToTwoDecimalPlaces(((int)Constants.ReservoirLevels.Chashma).ToString());
                            lblChashmaStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "ChashmaStorage");

                            txtChashmaMinLevel.Text = ConvertToTwoDecimalPlaces(((int)Constants.ReservoirLevels.ChashmaMinOptLevel).ToString());
                            lblChashmaMinStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "ChashmaMinOptLevelStorage");
                        }

                        DataObj = new SeasonalPlanningBLL().GetEastern((int)Constants.Seasons.Kharif, Convert.ToInt64(ddlKharifEastern.SelectedItem.Value));
                        if (DataObj != null)
                        {
                            if (_Scenario == "Minimum")
                            {
                                txtEasternEK.Text = ConvertToThreeDecimalPlaces((ConvertToDouble(Utility.GetDynamicPropertyValue(DataObj, "EK"))) / 2);
                                txtEasternLK.Text = ConvertToThreeDecimalPlaces((ConvertToDouble(Utility.GetDynamicPropertyValue(DataObj, "LK"))) / 2);
                            }
                            else
                            {
                                txtEasternEK.Text = ConvertToThreeDecimalPlaces(Utility.GetDynamicPropertyValue(DataObj, "EK"));
                                txtEasternLK.Text = ConvertToThreeDecimalPlaces(Utility.GetDynamicPropertyValue(DataObj, "LK"));
                            }

                            lblEasternTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(txtEasternEK.Text) + ConvertToDouble(txtEasternLK.Text));
                        }

                        DataObj = new SeasonalPlanningBLL().GetInflowsforKharifSeason(_IFDraftID, _Scenario);
                        if (DataObj != null)
                        {
                            lblJMEK.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaEK");
                            lblJMLK.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaLK");
                            lblJMTotal.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtManglaTotal");
                            lblCMEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaEK");
                            lblCMLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaLK");
                            lblCMTotal.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMaralaTotal");
                            lblITEK.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusAtTarbelaEK");
                            lblITLK.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusAtTarbelaLK");
                            lblITTotal.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusAtTarbelaTotal");
                            lblKNEK.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulAtNowsheraEK");
                            lblKNLK.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulAtNowsheraLK");
                            lblKNotal.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulAtNowsheraTotal");

                            lblJCTotalEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(GetDynamicPropertyValue(DataObj, "JhelumAtManglaEK")) + ConvertToDouble(GetDynamicPropertyValue(DataObj, "ChenabAtMaralaEK")) + ConvertToDouble(txtEasternEK.Text));
                            lblJCTotalLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(GetDynamicPropertyValue(DataObj, "JhelumAtManglaLK")) + ConvertToDouble(GetDynamicPropertyValue(DataObj, "ChenabAtMaralaLK")) + ConvertToDouble(txtEasternLK.Text));
                            lblJCTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCTotalEK.Text) + ConvertToDouble(lblJCTotalLK.Text));

                            lblICTotalEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(GetDynamicPropertyValue(DataObj, "IndusAtTarbelaEK")) + ConvertToDouble(GetDynamicPropertyValue(DataObj, "KabulAtNowsheraEK")));
                            lblICTotalLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(GetDynamicPropertyValue(DataObj, "IndusAtTarbelaLK")) + ConvertToDouble(GetDynamicPropertyValue(DataObj, "KabulAtNowsheraLK")));
                            lblICTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalEK.Text) + ConvertToDouble(lblICTotalLK.Text));
                        }

                        DataObj = new SeasonalPlanningBLL().GetInitialLevels(Constants.ElevationCapInitialStoragesMangla, Constants.ElevationCapInitialStoragesTarbela);
                        if (DataObj != null)
                        {
                            lblJCInitialLevel.Text = ConvertToTwoDecimalPlaces(Utility.GetDynamicPropertyValue(DataObj, "JMLevel"));
                            txtJCInitialStorage.Text = ConvertToThreeDecimalPlaces((double)Constants.ElevationCapInitialStoragesMangla);
                            lblICInitialLevel.Text = ConvertToTwoDecimalPlaces(Utility.GetDynamicPropertyValue(DataObj, "ITLevel"));
                            txtICInitialStorage.Text = ConvertToThreeDecimalPlaces((double)Constants.ElevationCapInitialStoragesTarbela);
                        }

                        txtJCStrToFillEK.Text = ConvertToString((int)Constants.StorageToFill.JhelumEK);
                        txtJCStrToFillLK.Text = ConvertToString((int)Constants.StorageToFill.JhelumLK);
                        txtICStrToFillEK.Text = ConvertToString((int)Constants.StorageToFill.IndusEK);
                        txtICStrToFillLK.Text = ConvertToString((int)Constants.StorageToFill.IndusLK);

                        lblJCStrFillEKMAF.Text = AppBlocks.Calculations.CalculateStorageToFill(ConvertToDouble(lblManglaStorage.Text), ConvertToDouble(txtJCInitialStorage.Text), Convert.ToDouble((int)Constants.StorageToFill.JhelumEK));
                        lblJCStrFillLKMAF.Text = AppBlocks.Calculations.CalculateStorageToFill(ConvertToDouble(lblManglaStorage.Text), ConvertToDouble(txtJCInitialStorage.Text), (int)Constants.StorageToFill.JhelumLK);
                        lblJCStrFillTotalMAF.Text = ConvertToString(ConvertToDouble(lblJCStrFillEKMAF.Text) + ConvertToDouble(lblJCStrFillLKMAF.Text));

                        lblICStrFillEKMAF.Text = AppBlocks.Calculations.CalculateStorageToFill(ConvertToDouble(lblTarbelaStorage.Text), ConvertToDouble(txtICInitialStorage.Text), (int)Constants.StorageToFill.IndusEK);
                        lblICStrFillLKMAF.Text = AppBlocks.Calculations.CalculateStorageToFill(ConvertToDouble(lblTarbelaStorage.Text), ConvertToDouble(txtICInitialStorage.Text), (int)Constants.StorageToFill.IndusLK);
                        lblICStrFillTotalMAF.Text = ConvertToString(ConvertToDouble(lblICStrFillEKMAF.Text) + ConvertToDouble(lblICStrFillLKMAF.Text));

                        txtJCStrDep.Text = ConvertToString(Constants.JCStorageDepretion);
                        txtICStrDep.Text = ConvertToString(Constants.ICStorageDepretion);

                        lblJCStrRel.Text = AppBlocks.Calculations.CalculateStorageRelease(Constants.JCStorageDepretion, ConvertToDouble(lblManglaStorage.Text), ConvertToDouble(txtJCInitialStorage.Text));
                        lblICStrRel.Text = AppBlocks.Calculations.CalculateStorageRelease(Constants.ICStorageDepretion, ConvertToDouble(lblTarbelaStorage.Text), ConvertToDouble(txtICInitialStorage.Text));

                        lblJCSysInflowEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCTotalEK.Text) - ConvertToDouble(lblJCStrFillEKMAF.Text));
                        lblJCSysInflowLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCTotalLK.Text) - ConvertToDouble(lblJCStrFillLKMAF.Text) + ((ConvertToDouble(txtJCStrDep.Text) / 100) * ConvertToDouble(lblManglaStorage.Text)));
                        lblJCSysInflowTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowEK.Text) + ConvertToDouble(lblJCSysInflowLK.Text));

                        txtJCSysLossGainPerEK.Text = ConvertToString((int)Constants.LossGain.JhelumEK);
                        txtJCSysLossGainPerLK.Text = ConvertToString((int)Constants.LossGain.JhelumLK);

                        txtICSysLossGainPerEK.Text = ConvertToString((int)Constants.LossGain.IndusEK);
                        txtICSysLossGainPerLK.Text = ConvertToString((int)Constants.LossGain.IndusLK);

                        lblJCSysLossGainMAFEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowEK.Text) * (double)(ConvertToDouble(txtJCSysLossGainPerEK.Text) / 100));
                        lblJCSysLossGainMAFLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowLK.Text) * (double)(ConvertToDouble(txtJCSysLossGainPerLK.Text) / 100));

                        lblJCWaterAvailEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowEK.Text) + ConvertToDouble(lblJCSysLossGainMAFEK.Text));
                        lblJCWaterAvailLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowLK.Text) + ConvertToDouble(lblJCSysLossGainMAFLK.Text));
                        lblJCWaterAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCWaterAvailEK.Text) + ConvertToDouble(lblJCWaterAvailLK.Text));

                        DataObj = new SeasonalPlanningBLL().GetParaKPKBALKharifMAF();
                        if (DataObj != null)
                        {
                            lblJCParaEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaEK");
                            lblJCParaLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaLK");
                            lblJCParaTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCParaEK.Text) + ConvertToDouble(lblJCParaLK.Text));

                            lblKPKBalEK.Text = Utility.GetDynamicPropertyValue(DataObj, "KPKBALEK");
                            lblKPKBalLK.Text = Utility.GetDynamicPropertyValue(DataObj, "KPKBALLK");
                            lblKPKBalTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblKPKBalEK.Text) + ConvertToDouble(lblKPKBalLK.Text));
                        }

                        if (lblJCWaterAvailEK.Text != "" && lblJCParaEK.Text != "")
                        {
                            if (ConvertToDouble(lblJCWaterAvailEK.Text) > ConvertToDouble(lblJCParaEK.Text))
                                txtJCCanalAvailEK.Text = lblJCParaEK.Text;
                            else
                                txtJCCanalAvailEK.Text = lblJCWaterAvailEK.Text;
                            lblJCCanalAvailTotal.Text = txtJCCanalAvailEK.Text;
                        }

                        if (lblJCWaterAvailLK.Text != "" && lblJCParaLK.Text != "")
                        {
                            if (ConvertToDouble(lblJCWaterAvailLK.Text) > ConvertToDouble(lblJCParaLK.Text))
                                txtJCCanalAvailLK.Text = lblJCParaLK.Text;
                            else
                                txtJCCanalAvailLK.Text = lblJCWaterAvailLK.Text;
                            lblJCCanalAvailTotal.Text = ConvertToString(ConvertToDouble(lblJCCanalAvailTotal.Text) + ConvertToDouble(txtJCCanalAvailLK.Text));
                        }

                        if (lblJCWaterAvailEK.Text != "" && txtJCCanalAvailEK.Text != "")
                            lblJCOutflowEK.Text = ConvertToString(ConvertToDouble(lblJCWaterAvailEK.Text) - ConvertToDouble(txtJCCanalAvailEK.Text));
                        lblICOutflowEK.Text = lblJCOutflowEK.Text;

                        if (lblJCWaterAvailLK.Text != "" && txtJCCanalAvailLK.Text != "")
                            lblJCOutflowLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCWaterAvailLK.Text) - ConvertToDouble(txtJCCanalAvailLK.Text));
                        lblICOutflowLK.Text = lblJCOutflowLK.Text;
                        lblJCOutflowTotal.Text = ConvertToString(ConvertToDouble(lblJCOutflowEK.Text) + ConvertToDouble(lblJCOutflowLK.Text));

                        if (txtJCCanalAvailEK.Text != "" && lblJCParaEK.Text != "")
                            lblJCShortageEK.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(txtJCCanalAvailEK.Text) / ConvertToDouble(lblJCParaEK.Text))) * 100);

                        if (txtJCCanalAvailLK.Text != "" && lblJCParaLK.Text != "")
                            lblJCShortageLK.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(txtJCCanalAvailLK.Text) / ConvertToDouble(lblJCParaLK.Text))) * 100);

                        if (lblJCCanalAvailTotal.Text != "" && lblJCParaTotal.Text != "")
                            lblJCShortageTotal.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(lblJCCanalAvailTotal.Text) / ConvertToDouble(lblJCParaTotal.Text))) * 100);

                        lblICOutflowTotal.Text = lblJCOutflowTotal.Text;

                        lblICSysInflowEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalEK.Text) - ConvertToDouble(lblICStrFillEKMAF.Text) + ConvertToDouble(lblICOutflowEK.Text));
                        lblICSysInflowLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalLK.Text) - ConvertToDouble(lblICStrFillLKMAF.Text) + ConvertToDouble(lblICOutflowLK.Text) + ((ConvertToDouble(txtICStrDep.Text) / 100) * ConvertToDouble(lblTarbelaStorage.Text)));
                        lblICSysInflowTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowEK.Text) + ConvertToDouble(lblICSysInflowLK.Text));

                        lblICSysLossGainMAFEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowEK.Text) * (double)(ConvertToDouble(txtICSysLossGainPerEK.Text) / 100));
                        lblICSysLossGainMAFLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowLK.Text) * (double)(ConvertToDouble(txtICSysLossGainPerLK.Text) / 100));

                        lblICWaterAvailEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowEK.Text) + ConvertToDouble(lblICSysLossGainMAFEK.Text));
                        lblICWaterAvailLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowLK.Text) + ConvertToDouble(lblICSysLossGainMAFLK.Text));
                        lblICWaterAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICWaterAvailEK.Text) + ConvertToDouble(lblICWaterAvailLK.Text));

                        DataObj = new SeasonalPlanningBLL().GetParaForIndus();
                        if (DataObj != null)
                        {
                            lblICParaEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaEK");
                            lblICParaLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaLK");
                            lblICParaTotal.Text = ConvertToString(ConvertToDouble(lblICParaEK.Text) + ConvertToDouble(lblICParaLK.Text));
                        }

                        if (lblICWaterAvailEK.Text != "" && lblKPKBalEK.Text != "")
                            lblPunjSindhEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICWaterAvailEK.Text) - ConvertToDouble(lblKPKBalEK.Text));

                        if (lblICWaterAvailLK.Text != "" && lblKPKBalLK.Text != "")
                            lblPunjSindhLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICWaterAvailLK.Text) - ConvertToDouble(lblKPKBalLK.Text));

                        if (lblPunjSindhEK.Text != "" && lblPunjSindhLK.Text != "")
                            lblPunjSindhTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblPunjSindhEK.Text) + ConvertToDouble(lblPunjSindhLK.Text));

                        if (lblPunjSindhEK.Text != "" && lblICParaEK.Text != "")
                        {
                            if (ConvertToDouble(lblPunjSindhEK.Text) > ConvertToDouble(lblICParaEK.Text))
                                txtICCanalAvailEK.Text = lblICParaEK.Text;
                            else
                                txtICCanalAvailEK.Text = lblPunjSindhEK.Text;
                            lblICCanalAvailTotal.Text = txtICCanalAvailEK.Text;
                        }

                        if (lblPunjSindhLK.Text != "" && lblICParaLK.Text != "")
                        {
                            if (ConvertToDouble(lblPunjSindhLK.Text) > ConvertToDouble(lblICParaLK.Text))
                                txtICCanalAvailLK.Text = lblICParaLK.Text;
                            else
                                txtICCanalAvailLK.Text = lblPunjSindhLK.Text;
                            lblICCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(txtICCanalAvailEK.Text) + ConvertToDouble(txtICCanalAvailLK.Text));
                        }

                        if (lblPunjSindhEK.Text != "" && txtICCanalAvailEK.Text != "")
                            lblKotriEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblPunjSindhEK.Text) - ConvertToDouble(txtICCanalAvailEK.Text));

                        if (lblPunjSindhLK.Text != "" && txtICCanalAvailLK.Text != "")
                            lblKotriLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblPunjSindhLK.Text) - ConvertToDouble(txtICCanalAvailLK.Text));

                        lblKotriTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblKotriEK.Text) + ConvertToDouble(lblKotriLK.Text));

                        if (txtICCanalAvailEK.Text != "" && lblICParaEK.Text != "")
                            lblICShortageEK.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(txtICCanalAvailEK.Text) / ConvertToDouble(lblICParaEK.Text))) * 100);

                        if (txtICCanalAvailLK.Text != "" && lblICParaLK.Text != "")
                            lblICShortageLK.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(txtICCanalAvailLK.Text) / ConvertToDouble(lblICParaLK.Text))) * 100);

                        if (lblJCCanalAvailTotal.Text != "" && lblJCParaTotal.Text != "")
                            lblICShortageTotal.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(lblICCanalAvailTotal.Text) / ConvertToDouble(lblICParaTotal.Text))) * 100);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public Double? ConvertToDouble(string _Text)
        {
            Double? ReturnValue = 0;
            try
            {
                if (_Text != "")
                    ReturnValue = Convert.ToDouble(_Text);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ReturnValue;
        }

        public String ConvertToString(Double? _Value)
        {
            String ReturnValue = "";
            try
            {
                if (_Value != null)
                    ReturnValue = Convert.ToString(_Value);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ReturnValue;
        }

        public long ConvertToLong(string _Text)
        {
            long ReturnValue = 0;
            try
            {
                if (_Text != "")
                    ReturnValue = Convert.ToInt64(_Text);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ReturnValue;
        }

        public static string GetDynamicPropertyValue(dynamic _Object, string _PropertyName)
        {
            string Result = Convert.ToString(_Object.GetType().GetProperty(_PropertyName).GetValue(_Object, null));
            //Result = Result.Remove(Result.Length - 6);
            string[] arr = Result.Split(' ');
            Result = arr[0];
            return Result;
        }

        public static string GetInflowsWithoutPercentage(string _Text)
        {
            string[] arr = _Text.Split(' ');
            _Text = arr[0];
            //if (_Text.Length > 6)
            //    _Text = _Text.Remove(_Text.Length - 6);
            return _Text;
        }

        /// <summary>
        /// This function handles Whole Inflows part
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        public void EditCanalAvailabiltyForKharifJhelum()
        {
            try
            {
                lblJCCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(txtJCCanalAvailEK.Text) + ConvertToDouble(txtJCCanalAvailLK.Text));

                if (lblJCWaterAvailEK.Text != "" && txtJCCanalAvailEK.Text != "")
                    lblJCOutflowEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCWaterAvailEK.Text) - ConvertToDouble(txtJCCanalAvailEK.Text));
                lblICOutflowEK.Text = lblJCOutflowEK.Text;

                if (lblJCWaterAvailLK.Text != "" && txtJCCanalAvailLK.Text != "")
                    lblJCOutflowLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCWaterAvailLK.Text) - ConvertToDouble(txtJCCanalAvailLK.Text));
                lblICOutflowLK.Text = lblJCOutflowLK.Text;

                lblJCOutflowTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCOutflowEK.Text) + ConvertToDouble(lblJCOutflowLK.Text));
                lblICOutflowTotal.Text = lblJCOutflowTotal.Text;

                // To have impact on calculation in Indus Command
                lblICSysInflowEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalEK.Text) - ConvertToDouble(lblICStrFillEKMAF.Text) + ConvertToDouble(lblICOutflowEK.Text));
                lblICSysInflowLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalLK.Text) - ConvertToDouble(lblICStrFillLKMAF.Text) + ConvertToDouble(lblICOutflowLK.Text) + ((ConvertToDouble(txtICStrDep.Text) / 100) * ConvertToDouble(lblTarbelaStorage.Text)));
                lblICSysInflowTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowEK.Text) + ConvertToDouble(lblICSysInflowLK.Text));
                EditSystemInflowsRegionForKharifIndus();
                //

                if (txtJCCanalAvailEK.Text != "" && lblJCParaEK.Text != "")
                    lblJCShortageEK.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(txtJCCanalAvailEK.Text) / ConvertToDouble(lblJCParaEK.Text))) * 100);

                if (txtJCCanalAvailLK.Text != "" && lblJCParaLK.Text != "")
                    lblJCShortageLK.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(txtJCCanalAvailLK.Text) / ConvertToDouble(lblJCParaLK.Text))) * 100);

                if (lblJCCanalAvailTotal.Text != "" && lblJCParaTotal.Text != "")
                    lblJCShortageTotal.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(lblJCCanalAvailTotal.Text) / ConvertToDouble(lblJCParaTotal.Text))) * 100);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditSystemInflowsRegionForKharifJhelum()
        {
            try
            {
                object DataObj = new object();
                lblJCSysLossGainMAFEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowEK.Text) * (double)(ConvertToDouble(txtJCSysLossGainPerEK.Text) / 100));
                lblJCSysLossGainMAFLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowLK.Text) * (double)(ConvertToDouble(txtJCSysLossGainPerLK.Text) / 100));

                lblJCWaterAvailEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowEK.Text) + ConvertToDouble(lblJCSysLossGainMAFEK.Text));
                lblJCWaterAvailLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowLK.Text) + ConvertToDouble(lblJCSysLossGainMAFLK.Text));
                lblJCWaterAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCWaterAvailEK.Text) + ConvertToDouble(lblJCWaterAvailLK.Text));
                //if (lblJCWaterAvailEK.Text != "" && lblJCParaEK.Text != "")
                //{
                //    if (ConvertToDouble(lblJCWaterAvailEK.Text) > ConvertToDouble(lblJCParaEK.Text))
                //        txtJCCanalAvailEK.Text = lblJCParaEK.Text;
                //    else
                //        txtJCCanalAvailEK.Text = lblJCWaterAvailEK.Text;
                //    lblJCCanalAvailTotal.Text = txtJCCanalAvailEK.Text;
                //}

                //if (lblJCWaterAvailLK.Text != "" && lblJCParaLK.Text != "")
                //{
                //    if (ConvertToDouble(lblJCWaterAvailLK.Text) > ConvertToDouble(lblJCParaLK.Text))
                //        txtJCCanalAvailLK.Text = lblJCParaLK.Text;
                //    else
                //        txtJCCanalAvailLK.Text = lblJCWaterAvailLK.Text;
                //    lblJCCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCCanalAvailTotal.Text) + ConvertToDouble(txtJCCanalAvailLK.Text));
                //}


                //if (lblJCWaterAvailEK.Text != "" && txtJCCanalAvailEK.Text != "")
                //    lblJCOutflowEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCWaterAvailEK.Text) - ConvertToDouble(txtJCCanalAvailEK.Text));
                //lblICOutflowEK.Text = lblJCOutflowEK.Text;

                //if (lblJCWaterAvailLK.Text != "" && txtJCCanalAvailLK.Text != "")
                //    lblJCOutflowLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCWaterAvailLK.Text) - ConvertToDouble(txtJCCanalAvailLK.Text));
                //lblICOutflowLK.Text = lblJCOutflowLK.Text;

                //lblJCOutflowTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCOutflowEK.Text) + ConvertToDouble(lblJCOutflowLK.Text));
                //lblICOutflowTotal.Text = lblJCOutflowTotal.Text;

                //if (txtJCCanalAvailEK.Text != "" && lblJCParaEK.Text != "")
                //    lblJCShortageEK.Text = ConvertToThreeDecimalPlaces(1 - (ConvertToDouble(txtJCCanalAvailEK.Text) / ConvertToDouble(lblJCParaEK.Text)));

                //if (txtJCCanalAvailLK.Text != "" && lblJCParaLK.Text != "")
                //    lblJCShortageLK.Text = ConvertToThreeDecimalPlaces(1 - (ConvertToDouble(txtJCCanalAvailLK.Text) / ConvertToDouble(lblJCParaLK.Text)));

                //if (lblJCCanalAvailTotal.Text != "" && lblJCParaTotal.Text != "")
                //    lblJCShortageTotal.Text = ConvertToThreeDecimalPlaces(1 - (ConvertToDouble(lblJCCanalAvailTotal.Text) / ConvertToDouble(lblJCParaTotal.Text)));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditSystemInflowsRegionForKharifIndus()
        {
            try
            {
                object DataObj = new object();

                lblICSysLossGainMAFEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowEK.Text) * (double)(ConvertToDouble(txtICSysLossGainPerEK.Text) / 100));
                lblICSysLossGainMAFLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowLK.Text) * (double)(ConvertToDouble(txtICSysLossGainPerLK.Text) / 100));

                lblICWaterAvailEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowEK.Text) + ConvertToDouble(lblICSysLossGainMAFEK.Text));
                lblICWaterAvailLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowLK.Text) + ConvertToDouble(lblICSysLossGainMAFLK.Text));
                lblICWaterAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICWaterAvailEK.Text) + ConvertToDouble(lblICWaterAvailLK.Text));

                DataObj = new SeasonalPlanningBLL().GetParaKPKBALKharifMAF();
                if (DataObj != null)
                {
                    lblJCParaEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaEK");
                    lblJCParaLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaLK");
                    lblJCParaTotal.Text = ConvertToString(ConvertToDouble(lblJCParaEK.Text) + ConvertToDouble(lblJCParaLK.Text));

                    lblKPKBalEK.Text = Utility.GetDynamicPropertyValue(DataObj, "KPKBALEK");
                    lblKPKBalLK.Text = Utility.GetDynamicPropertyValue(DataObj, "KPKBALLK");
                    lblKPKBalTotal.Text = ConvertToString(ConvertToDouble(lblKPKBalEK.Text) + ConvertToDouble(lblKPKBalLK.Text));
                }

                DataObj = new SeasonalPlanningBLL().GetParaForIndus();
                if (DataObj != null)
                {
                    lblICParaEK.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaEK");
                    lblICParaLK.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaLK");
                    lblICParaTotal.Text = ConvertToString(ConvertToDouble(lblICParaEK.Text) + ConvertToDouble(lblICParaLK.Text));
                }

                if (lblICWaterAvailEK.Text != "" && lblKPKBalEK.Text != "")
                    lblPunjSindhEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICWaterAvailEK.Text) - ConvertToDouble(lblKPKBalEK.Text));

                if (lblICWaterAvailLK.Text != "" && lblKPKBalLK.Text != "")
                    lblPunjSindhLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICWaterAvailLK.Text) - ConvertToDouble(lblKPKBalLK.Text));

                lblPunjSindhTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblPunjSindhEK.Text) + ConvertToDouble(lblPunjSindhLK.Text));

                if (lblPunjSindhEK.Text != "" && lblICParaEK.Text != "")
                {
                    if (ConvertToDouble(lblPunjSindhEK.Text) > ConvertToDouble(lblICParaEK.Text))
                        txtICCanalAvailEK.Text = lblICParaEK.Text;
                    else
                        txtICCanalAvailEK.Text = lblPunjSindhEK.Text;
                }

                lblICCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(txtICCanalAvailEK.Text) + ConvertToDouble(txtICCanalAvailLK.Text));

                if (lblPunjSindhLK.Text != "" && lblICParaLK.Text != "")
                {
                    if (ConvertToDouble(lblICWaterAvailLK.Text) > ConvertToDouble(lblICParaLK.Text))
                        txtICCanalAvailLK.Text = lblICParaLK.Text;
                    else
                        txtICCanalAvailLK.Text = lblPunjSindhLK.Text;
                }

                lblICCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(txtICCanalAvailEK.Text) + ConvertToDouble(txtICCanalAvailLK.Text));

                if (lblPunjSindhEK.Text != "" && txtICCanalAvailEK.Text != "")
                {
                    lblKotriEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblPunjSindhEK.Text) - ConvertToDouble(txtICCanalAvailEK.Text));
                    lblKotriTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblKotriEK.Text));
                }

                if (lblPunjSindhLK.Text != "" && txtICCanalAvailLK.Text != "")
                {
                    lblKotriLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblPunjSindhLK.Text) - ConvertToDouble(txtICCanalAvailLK.Text));
                    lblKotriTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblKotriLK.Text));
                }

                if (lblKotriLK.Text != "" && lblKotriEK.Text != "")
                    lblKotriTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblKotriEK.Text) + ConvertToDouble(lblKotriLK.Text));


                if (txtICCanalAvailEK.Text != "" && lblICParaEK.Text != "")
                    lblICShortageEK.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(txtICCanalAvailEK.Text) / ConvertToDouble(lblICParaEK.Text))) * 100);

                if (txtICCanalAvailLK.Text != "" && lblICParaLK.Text != "")
                    lblICShortageLK.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(txtICCanalAvailLK.Text) / ConvertToDouble(lblICParaLK.Text))) * 100);

                if (lblJCCanalAvailTotal.Text != "" && lblJCParaTotal.Text != "")
                    lblICShortageTotal.Text = ConvertToThreeDecimalPlaces((1 - (ConvertToDouble(lblICCanalAvailTotal.Text) / ConvertToDouble(lblICParaTotal.Text))) * 100);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditReservirLevelRegionForKharifJhelum()
        {
            try
            {
                if (ConvertToDouble(txtJCInitialStorage.Text) <= ConvertToDouble(lblManglaStorage.Text))
                    lblJCInitialLevel.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, ConvertToDouble(txtJCInitialStorage.Text)));
                else
                {
                    txtJCInitialStorage.Text = lblManglaStorage.Text;
                    lblJCInitialLevel.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, ConvertToDouble(txtJCInitialStorage.Text)));
                    Master.ShowMessage(Message.MCLMustBeLess.Description, SiteMaster.MessageType.Error);
                }

                if (ConvertToDouble(txtJCStrToFillEK.Text) + ConvertToDouble(txtJCStrToFillLK.Text) <= 100)
                {
                    lblJCStrFillEKMAF.Text = AppBlocks.Calculations.CalculateStorageToFill(ConvertToDouble(lblManglaStorage.Text), ConvertToDouble(txtJCInitialStorage.Text), ConvertToDouble(txtJCStrToFillEK.Text));
                    lblJCStrFillLKMAF.Text = AppBlocks.Calculations.CalculateStorageToFill(ConvertToDouble(lblManglaStorage.Text), ConvertToDouble(txtJCInitialStorage.Text), ConvertToDouble(txtJCStrToFillLK.Text));
                    lblJCStrFillTotalMAF.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCStrFillEKMAF.Text) + ConvertToDouble(lblJCStrFillLKMAF.Text));
                }
                else
                    Master.ShowMessage(Message.StrFillPercentage.Description, SiteMaster.MessageType.Error);

                lblJCStrRel.Text = AppBlocks.Calculations.CalculateStorageRelease(ConvertToDouble(txtJCStrDep.Text), ConvertToDouble(lblManglaStorage.Text), ConvertToDouble(txtJCInitialStorage.Text));

                lblJCSysInflowEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCTotalEK.Text) - ConvertToDouble(lblJCStrFillEKMAF.Text));
                lblJCSysInflowLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCTotalLK.Text) - ConvertToDouble(lblJCStrFillLKMAF.Text) + ((ConvertToDouble(txtJCStrDep.Text) / 100) * ConvertToDouble(lblManglaStorage.Text)));
                lblJCSysInflowTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowEK.Text) + ConvertToDouble(lblJCSysInflowLK.Text));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditReservirLevelRegionForKharifIndus()
        {
            try
            {
                if (ConvertToDouble(txtICInitialStorage.Text) <= ConvertToDouble(lblTarbelaStorage.Text))
                    lblICInitialLevel.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, ConvertToDouble(txtICInitialStorage.Text)));
                else
                {
                    txtICInitialStorage.Text = lblTarbelaStorage.Text;
                    lblICInitialLevel.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, ConvertToDouble(txtICInitialStorage.Text)));
                    Master.ShowMessage(Message.MCLMustBeLess.Description, SiteMaster.MessageType.Error);
                }

                if (ConvertToDouble(txtICStrToFillEK.Text) + ConvertToDouble(txtICStrToFillLK.Text) <= 100)
                {
                    lblICStrFillEKMAF.Text = AppBlocks.Calculations.CalculateStorageToFill(ConvertToDouble(lblTarbelaStorage.Text), ConvertToDouble(txtICInitialStorage.Text), ConvertToDouble(txtICStrToFillEK.Text));
                    lblICStrFillLKMAF.Text = AppBlocks.Calculations.CalculateStorageToFill(ConvertToDouble(lblTarbelaStorage.Text), ConvertToDouble(txtICInitialStorage.Text), ConvertToDouble(txtICStrToFillLK.Text));
                    lblICStrFillTotalMAF.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICStrFillEKMAF.Text) + ConvertToDouble(lblICStrFillLKMAF.Text));
                }
                else
                {
                    Master.ShowMessage(Message.StrFillPercentage.Description, SiteMaster.MessageType.Error);
                    txtICStrToFillEK.Text = "";
                    txtICStrToFillLK.Text = "";
                }

                lblICStrRel.Text = AppBlocks.Calculations.CalculateStorageRelease(ConvertToDouble(txtICStrDep.Text), ConvertToDouble(lblTarbelaStorage.Text), ConvertToDouble(txtICInitialStorage.Text));

                lblICSysInflowEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalEK.Text) - ConvertToDouble(lblICStrFillEKMAF.Text) + ConvertToDouble(lblICOutflowEK.Text));
                lblICSysInflowLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalLK.Text) - ConvertToDouble(lblICStrFillLKMAF.Text) + ConvertToDouble(lblICOutflowLK.Text) + ((ConvertToDouble(txtICStrDep.Text) / 100) * ConvertToDouble(lblTarbelaStorage.Text)));
                lblICSysInflowTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowEK.Text) + ConvertToDouble(lblICSysInflowLK.Text));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditEasternRegionForKharif()
        {
            try
            {
                lblEasternTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(txtEasternEK.Text) + ConvertToDouble(txtEasternLK.Text));
                lblJCTotalEK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(GetInflowsWithoutPercentage(lblJMEK.Text)) + ConvertToDouble(GetInflowsWithoutPercentage(lblCMEK.Text)) + ConvertToDouble(txtEasternEK.Text));
                lblJCTotalLK.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(GetInflowsWithoutPercentage(lblJMLK.Text)) + ConvertToDouble(GetInflowsWithoutPercentage(lblCMLK.Text)) + ConvertToDouble(txtEasternLK.Text));
                lblJCTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCTotalEK.Text) + ConvertToDouble(lblJCTotalLK.Text));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditMCLLevelsForKharifJhelum()
        {
            try
            {
                lblManglaStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, ConvertToDouble(txtJCManglaLevel.Text)));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditMCLLevelsForKharifIndus()
        {
            try
            {
                lblTarbelaStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, ConvertToDouble(txtTarbelaLevel.Text)));
                if (ConvertToDouble(txtFillingLimit.Text) < ConvertToDouble(txtTarbelaLevel.Text))
                    lblFillingStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, ConvertToDouble(txtFillingLimit.Text)));
                else
                    Master.ShowMessage(Message.FillingLimitCannotBeGreater.Description, SiteMaster.MessageType.Error);

                lblChashmaStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.Chashma, ConvertToDouble(txtChashmaLevel.Text)));
                lblChashmaMinStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.Chashma, ConvertToDouble(txtChashmaMinLevel.Text)));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJCSysLossGainPerEK_TextChanged(object sender, EventArgs e)
        {
            EditSystemInflowsRegionForKharifJhelum();
            EditCanalAvailabiltyForKharifJhelum();
            EditViewKharifTrue();

            if (((TextBox)sender).ID == "txtJCSysLossGainPerEK")
                txtJCSysLossGainPerLK.Focus();
            else if (((TextBox)sender).ID == "txtJCSysLossGainPerLK")
                txtJCCanalAvailEK.Focus();
        }

        protected void txtICSysLossGainPerEK_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditSystemInflowsRegionForKharifIndus();
                EditViewKharifTrue();
                txtICSysLossGainPerLK.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJCInitialStorage_TextChanged(object sender, EventArgs e)
        {
            EditReservirLevelRegionForKharifJhelum();
            EditSystemInflowsRegionForKharifJhelum();
            EditCanalAvailabiltyForKharifJhelum();
            EditViewKharifTrue();

            if (((TextBox)sender).ID == "txtJCInitialStorage")
                txtJCStrToFillEK.Focus();
            else if (((TextBox)sender).ID == "txtJCStrToFillEK")
                txtJCStrToFillLK.Focus();
            else if (((TextBox)sender).ID == "txtJCStrToFillLK")
                txtJCStrDep.Focus();
            else if (((TextBox)sender).ID == "txtJCStrDep")
                txtJCSysLossGainPerEK.Focus();
        }

        protected void txtICInitialStorage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditReservirLevelRegionForKharifIndus();
                EditSystemInflowsRegionForKharifIndus();
                EditViewKharifTrue();

                if (((TextBox)sender).ID == "txtICInitialStorage")
                    txtICStrToFillEK.Focus();
                else if (((TextBox)sender).ID == "txtICStrToFillEK")
                    txtICStrToFillLK.Focus();
                else if (((TextBox)sender).ID == "txtICStrToFillLK")
                    txtICStrDep.Focus();
                else if (((TextBox)sender).ID == "txtICStrDep")
                    txtICSysLossGainPerEK.Focus();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJCManglaLevel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditMCLLevelsForKharifJhelum();
                EditEasternRegionForKharif();
                EditReservirLevelRegionForKharifJhelum();
                EditSystemInflowsRegionForKharifJhelum();
                EditCanalAvailabiltyForKharifJhelum();
                EditViewKharifTrue();
                txtEasternEK.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtTarbelaLevel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditMCLLevelsForKharifIndus();
                EditReservirLevelRegionForKharifIndus();
                EditSystemInflowsRegionForKharifIndus();
                EditViewKharifTrue();

                if (((TextBox)sender).ID == "txtTarbelaLevel")
                    txtFillingLimit.Focus();
                else if (((TextBox)sender).ID == "txtFillingLimit")
                    txtChashmaLevel.Focus();
                else if (((TextBox)sender).ID == "txtChashmaLevel")
                    txtChashmaMinLevel.Focus();
                else if (((TextBox)sender).ID == "txtChashmaMinLevel")
                    txtICInitialStorage.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtEasternEK_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditEasternRegionForKharif();
                EditReservirLevelRegionForKharifJhelum();
                EditSystemInflowsRegionForKharifJhelum();
                EditCanalAvailabiltyForKharifJhelum();
                EditViewKharifTrue();

                if (((TextBox)sender).ID == "txtEasternEK")
                    txtEasternLK.Focus();
                else
                    txtJCInitialStorage.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJCCanalAvailEK_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditCanalAvailabiltyForKharifJhelum();
                EditViewKharifTrue();
                txtJCCanalAvailLK.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetLikelyFieldsKharif(long _IFDraftID, long _SeasonID)
        {
            try
            {
                btnREdit.Visible = false;
                btnEdit.Visible = false;
                btnRabiSave.Visible = false;
                btnKharifSave.Visible = false;
                ddlKharifEastern.Visible = false;
                ddlRabiEastern.Visible = false;

                if (_SeasonID == (int)Constants.Seasons.Kharif)
                {
                    SP_PlanBalance BalanceObj = new SeasonalPlanningBLL().GetExistingBalanceData(SPDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, "Likely");
                    if (BalanceObj != null)
                    {
                        SetKharifSavedFields(BalanceObj, _IFDraftID, "Likely");
                        divSPDrafts.Visible = false;
                        divPanel.Visible = true;
                        divKharif.Visible = true;
                        lblScenarioName.Text = "Likely Scenario";
                    }
                    else
                    {
                        BalanceObj = new SeasonalPlanningBLL().GetLikelyBalanceJhelum(SPDraftID, _IFDraftID, _SeasonID);
                        if (BalanceObj != null && BalanceObj.ID == 0)
                        {
                            divSPDrafts.Visible = false;
                            divPanel.Visible = true;
                            divKharif.Visible = true;
                            lblScenarioName.Text = "Likely Scenario";
                            txtJCManglaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.MCL);
                            lblManglaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.Storage);
                            lblJMEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumEK);
                            lblJMLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumLK);
                            lblJMTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Jhelum);
                            lblCMEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChenabEK);
                            lblCMLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChenabLK);
                            lblCMTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Chenab);
                            txtEasternEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.EasternEK);
                            txtEasternLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.EasternLK);
                            lblEasternTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Eastern);
                            lblJCTotalEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumEK + BalanceObj.ChenabEK + BalanceObj.EasternEK);
                            lblJCTotalLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumLK + BalanceObj.ChenabLK + BalanceObj.EasternLK);
                            lblJCTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Jhelum + BalanceObj.Chenab + BalanceObj.Eastern);
                            lblJCInitialLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.InitResLevel);
                            txtJCInitialStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.InitStorage);
                            txtJCStrToFillEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.StrtofillEK);
                            txtJCStrToFillLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.StrtofillLK);
                            lblJCStrFillEKMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsEK);
                            lblJCStrFillLKMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsLK);
                            lblJCStrFillTotalMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsEK + BalanceObj.FlowsLK);
                            txtJCStrDep.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageDep);
                            lblJCStrRel.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageRelease);
                            lblJCSysInflowEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK);
                            lblJCSysInflowLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsLK);
                            lblJCSysInflowTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK + BalanceObj.SysInflowsLK);
                            txtJCSysLossGainPerEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.EarlyKharif);
                            txtJCSysLossGainPerLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.LateKharif);
                            lblJCSysLossGainMAFEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK * (BalanceObj.EarlyKharif / 100));
                            lblJCSysLossGainMAFLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsLK * (BalanceObj.LateKharif / 100));
                            lblJCWaterAvailEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailabilityEK);
                            lblJCWaterAvailLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailabilityLK);
                            lblJCWaterAvailTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailability);
                            lblJCParaEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2EK);
                            lblJCParaLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2LK);
                            lblJCParaTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2);
                            txtJCCanalAvailEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailabilityEK);
                            txtJCCanalAvailLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailabilityLK);
                            lblJCCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailability);
                            lblJCOutflowEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflowEK);
                            lblJCOutflowLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCoutflowLK);
                            lblJCOutflowTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflow);
                            lblJCShortageEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ShortageEK);
                            lblJCShortageLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ShortageLK);
                            lblJCShortageTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Shortage);

                            BalanceObj = new SeasonalPlanningBLL().GetLikelyBalanceIndus(SPDraftID, _IFDraftID, _SeasonID);

                            if (BalanceObj != null && BalanceObj.ID == 0)
                            {
                                txtTarbelaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.MCL);
                                lblTarbelaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.Storage);
                                txtFillingLimit.Text = ConvertToTwoDecimalPlaces(BalanceObj.FillingLimit);
                                lblFillingStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.FillingLimitStorage);
                                txtChashmaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.ChashmaResLevel);
                                lblChashmaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChashmaStorage);
                                txtChashmaMinLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.ChashmaMinReslevel);
                                lblChashmaMinStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChashmaMinStorage);
                                lblITEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusEK);
                                lblITLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusLK);
                                lblITTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Indus);
                                lblKNEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KabulEK);
                                lblKNLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KabulLK);
                                lblKNotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Kabul);
                                lblICTotalEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusEK + BalanceObj.KabulEK);
                                lblICTotalLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusLK + BalanceObj.KabulLK);
                                lblICTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Indus + BalanceObj.Kabul);
                                lblICInitialLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.InitResLevel);
                                txtICInitialStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.InitStorage);
                                txtICStrToFillEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.StrtofillEK);
                                txtICStrToFillLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.StrtofillLK);
                                lblICStrFillEKMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsEK);
                                lblICStrFillLKMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsLK);
                                lblICStrFillTotalMAF.Text = ConvertToThreeDecimalPlaces(BalanceObj.FlowsEK + BalanceObj.FlowsLK);
                                txtICStrDep.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageDep);
                                lblICStrRel.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageRelease);
                                lblICSysInflowEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK);
                                lblICSysInflowLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsLK);
                                lblICSysInflowTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK + BalanceObj.SysInflowsLK);
                                txtICSysLossGainPerEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.EarlyKharif);
                                txtICSysLossGainPerLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.LateKharif);
                                lblICSysLossGainMAFEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsEK * (BalanceObj.EarlyKharif / 100));
                                lblICSysLossGainMAFLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysInflowsLK * (BalanceObj.LateKharif / 100));
                                lblICWaterAvailEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailabilityEK);
                                lblICWaterAvailLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailabilityLK);
                                lblICWaterAvailTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailability);
                                lblKPKBalEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KPKBalochistanEK);
                                lblKPKBalLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KPKBalochistanLK);
                                lblKPKBalTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.KPKBalochistan);
                                lblICParaEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2EK);
                                lblICParaLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2LK);
                                lblICParaTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2);
                                lblPunjSindhEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.BalPunSindhEK);
                                lblPunjSindhLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.BalPunsindhLK);
                                lblPunjSindhTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.BalPunSindh);
                                txtICCanalAvailEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailabilityEK);
                                txtICCanalAvailLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailabilityLK);
                                lblICCanalAvailTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailability);
                                lblICOutflowEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflowEK);
                                lblICOutflowLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCoutflowLK);
                                lblICOutflowTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflow);
                                lblKotriEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KotriEK);
                                lblKotriLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.KotriLK);
                                lblKotriTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Kotri);
                                lblICShortageEK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ShortageEK);
                                lblICShortageLK.Text = ConvertToThreeDecimalPlaces(BalanceObj.ShortageLK);
                                lblICShortageTotal.Text = ConvertToThreeDecimalPlaces(BalanceObj.Shortage);
                            }

                            btnKharifSave_Click(null, null);
                        }
                        else
                            Master.ShowMessage(Message.MaxMinScenariosNotExist.Description, SiteMaster.MessageType.Error);
                    }
                }
                else // Rabi 
                {
                    SP_PlanBalance BalanceObj = new SeasonalPlanningBLL().GetExistingBalanceData(SPDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, "Likely");
                    if (BalanceObj != null)
                    {
                        SetRabiSavedfields(BalanceObj, _IFDraftID, "Likely");
                        divSPDrafts.Visible = false;
                        divPanel.Visible = true;
                        divRabi.Visible = true;
                        lblRabiScenarioName.Text = "Likely Scenario";
                    }
                    else
                    {
                        BalanceObj = new SeasonalPlanningBLL().GetLikelyBalanceJhelum(SPDraftID, _IFDraftID, _SeasonID);
                        if (BalanceObj != null && BalanceObj.ID == 0)
                        {
                            divSPDrafts.Visible = false;
                            divPanel.Visible = true;
                            divRabi.Visible = true;
                            lblRabiScenarioName.Text = "Likely Scenario";
                            txtRJCManglaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.MCL);
                            lblRManglaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.Storage);
                            lblJMRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.JhelumRabi);
                            lblCMRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChenabRabi);
                            txtEasternRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Eastern);
                            lblJCTotalRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Inflow);
                            lblJCInitialLevelRabi.Text = ConvertToTwoDecimalPlaces(BalanceObj.InitResLevel);
                            txtJCInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.InitStorage);
                            lblJCMaxStorageRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.MaxStorage);
                            lblJCInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageAvailable);
                            txtJCStrDepRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageDep);
                            lblJCStrRelRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageRelease);
                            lblJCSysInflowRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalSysInfow);
                            txtJCSysLossGainPerRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysLossGain);
                            lblJCSysLossGainMAFRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalSysInfow * (BalanceObj.SysLossGain / 100));
                            lblJCWaterAvailRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailability);
                            lblJCParaRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2);
                            txtJCCanalAvailRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailability);
                            lblJCOutflowRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflow);
                            lblJCShortageRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Shortage);

                            BalanceObj = new SeasonalPlanningBLL().GetLikelyBalanceIndus(SPDraftID, _IFDraftID, _SeasonID);

                            if (BalanceObj != null && BalanceObj.ID == 0)
                            {
                                txtRTarbelaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.MCL);
                                lblRTarbelaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.Storage);
                                txtRFillingLimit.Text = ConvertToTwoDecimalPlaces(BalanceObj.FillingLimit);
                                lblRFillingStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.FillingLimitStorage);
                                txtRChashmaLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.ChashmaResLevel);
                                lblRChashmaStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChashmaStorage);
                                txtRChashmaMinLevel.Text = ConvertToTwoDecimalPlaces(BalanceObj.ChashmaMinReslevel);
                                lblRChashmaMinStorage.Text = ConvertToThreeDecimalPlaces(BalanceObj.ChashmaMinStorage);
                                lblITRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.IndusRabi);
                                lblKNRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.KabulRabi);
                                lblICTotalRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Inflow);
                                lblICInitialLevelRabi.Text = ConvertToTwoDecimalPlaces(BalanceObj.InitResLevel);
                                txtICInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.InitStorage);
                                lblICMaxStorageRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.MaxStorage);
                                lblICInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageAvailable);
                                txtICStrDepRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageDep);
                                lblICStrRelRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.StorageRelease);
                                lblICSysInflowRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalSysInfow);
                                txtICSysLossGainPerRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.SysLossGain);
                                lblICSysLossGainMAFRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalSysInfow * (BalanceObj.SysLossGain / 100));
                                lblICWaterAvailRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.TotalAvailability);
                                lblKPKBalRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.KPKBalochistan);
                                lblPunjSindhRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.BalPunSindh);
                                // lblICParaRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Para2);
                                txtICCanalAvailRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.CanalAvailability);
                                lblKotriRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Kotri);
                                lblICOutflowRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.JCOutflow);
                                lblICHistoricRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Historic);
                                lblICShortageRabi.Text = ConvertToThreeDecimalPlaces(BalanceObj.Shortage);
                            }

                            btnRabiSave_Click(null, null);
                        }
                        else
                            Master.ShowMessage(Message.MaxMinScenariosNotExist.Description, SiteMaster.MessageType.Error);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Balance Reservoir Rabi

        public void SetRabiSavedfields(SP_PlanBalance _BalanceObj, long _IFDraftID, String _Scenario)
        {
            try
            {
                List<SP_ForecastScenario> lstPercentages = new SeasonalPlanningBLL().GetForecastPercentages(_IFDraftID, _Scenario);

                //jhelum 
                txtRJCManglaLevel.Text = ConvertToTwoDecimalPlaces(_BalanceObj.MCL);
                lblRManglaStorage.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Storage);
                if (_Scenario == "Likely")
                {
                    lblJMRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.JhelumRabi);
                    lblCMRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.ChenabRabi);
                }
                else
                {
                    lblJMRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.JhelumRabi) + " (" + lstPercentages.ElementAt(0).RabiPercent + "%)";
                    lblCMRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.ChenabRabi) + " (" + lstPercentages.ElementAt(1).RabiPercent + "%)";
                    ddlRabiEastern.Items.FindByText(Convert.ToString(_BalanceObj.EasternYears)).Selected = true;
                }

                txtEasternRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Eastern);
                lblJCTotalRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Inflow);
                lblJCInitialLevelRabi.Text = ConvertToTwoDecimalPlaces(_BalanceObj.InitResLevel);
                txtJCInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.InitStorage);
                lblJCMaxStorageRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.MaxStorage);
                lblJCInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.StorageAvailable);
                txtJCStrDepRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.StorageDep);
                lblJCStrRelRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.StorageRelease);
                lblJCSysInflowRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.TotalSysInfow);
                txtJCSysLossGainPerRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.SysLossGain);
                lblJCSysLossGainMAFRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.TotalSysInfow * (_BalanceObj.SysLossGain / 100));
                lblJCWaterAvailRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.TotalAvailability);
                lblJCParaRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Para2);
                txtJCCanalAvailRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.CanalAvailability);
                lblJCOutflowRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.JCOutflow);
                lblJCShortageRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Shortage);

                //indus 

                _BalanceObj = new SeasonalPlanningBLL().GetExistingBalanceData(SPDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, _Scenario);

                txtRTarbelaLevel.Text = ConvertToTwoDecimalPlaces(_BalanceObj.MCL);
                lblRTarbelaStorage.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Storage);
                txtRFillingLimit.Text = ConvertToTwoDecimalPlaces(_BalanceObj.FillingLimit);
                lblRFillingStorage.Text = ConvertToThreeDecimalPlaces(_BalanceObj.FillingLimitStorage);
                txtRChashmaLevel.Text = ConvertToTwoDecimalPlaces(_BalanceObj.ChashmaResLevel);
                lblRChashmaStorage.Text = ConvertToThreeDecimalPlaces(_BalanceObj.ChashmaStorage);
                txtRChashmaMinLevel.Text = ConvertToTwoDecimalPlaces(_BalanceObj.ChashmaMinReslevel);
                lblRChashmaMinStorage.Text = ConvertToThreeDecimalPlaces(_BalanceObj.ChashmaMinStorage);
                if (_Scenario == "Likely")
                {
                    lblITRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.IndusRabi);
                    lblKNRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.KabulRabi);
                }
                else
                {
                    lblITRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.IndusRabi) + " (" + lstPercentages.ElementAt(3).RabiPercent + "%)";
                    lblKNRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.KabulRabi) + " (" + lstPercentages.ElementAt(2).RabiPercent + "%)";
                }
                lblICTotalRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Inflow);
                lblICInitialLevelRabi.Text = ConvertToTwoDecimalPlaces(_BalanceObj.InitResLevel);
                txtICInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.InitStorage);
                lblICMaxStorageRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.MaxStorage);
                lblICInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.StorageAvailable);
                txtICStrDepRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.StorageDep);
                lblICStrRelRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.StorageRelease);
                lblICSysInflowRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.TotalSysInfow);
                txtICSysLossGainPerRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.SysLossGain);
                lblICSysLossGainMAFRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.TotalSysInfow * (_BalanceObj.SysLossGain / 100));
                lblICWaterAvailRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.TotalAvailability);
                lblKPKBalRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.KPKBalochistan);
                lblPunjSindhRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.BalPunSindh);
                // lblICParaRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Para2);
                txtICCanalAvailRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.CanalAvailability);
                lblKotriRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Kotri);
                lblICOutflowRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.JCOutflow);
                lblICHistoricRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Historic);
                lblICShortageRabi.Text = ConvertToThreeDecimalPlaces(_BalanceObj.Shortage);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetRabiBalanceReservoirFields(long _IFDraftID, string _Scenario)
        {
            object DataObj = new object();
            divKharif.Visible = false;
            divRabi.Visible = true;
            lblScenarioName.Text = _Scenario + " Scenario";
            lblRabiScenarioName.Text = _Scenario + " Scenario";
            htitle.InnerText = "Seasonal Planning-" + _Scenario + " Scenario";
            try
            {
                if (_Scenario == "Likely")
                {
                    SetLikelyFieldsKharif(_IFDraftID, (int)Constants.Seasons.Rabi);
                }
                else
                {
                    ddlKharifEastern.DataSource = CommonLists.GetEasternYears();
                    ddlKharifEastern.DataTextField = "Name";
                    ddlKharifEastern.DataValueField = "ID";
                    ddlKharifEastern.DataBind();

                    ddlRabiEastern.DataSource = CommonLists.GetEasternYears();
                    ddlRabiEastern.DataTextField = "Name";
                    ddlRabiEastern.DataValueField = "ID";
                    ddlRabiEastern.DataBind();

                    SP_PlanBalance BalanceObj = new SeasonalPlanningBLL().GetExistingBalanceData(SPDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario);
                    if (BalanceObj != null)
                        SetRabiSavedfields(BalanceObj, _IFDraftID, _Scenario);
                    else
                    {
                        DataObj = new SeasonalPlanningBLL().GetElevationCapacitiesForBalanceReservoir((long)Constants.ReservoirLevels.Mangla, (long)Constants.ReservoirLevels.Tarbela, (long)Constants.ReservoirLevels.TarbelaFillingLimit, (long)Constants.ReservoirLevels.Chashma, (long)Constants.ReservoirLevels.ChashmaMinOptLevel);

                        if (DataObj != null)
                        {
                            txtRJCManglaLevel.Text = ((int)Constants.ReservoirLevels.Mangla).ToString();
                            lblRManglaStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "ManglaStorage");

                            txtRTarbelaLevel.Text = ((int)Constants.ReservoirLevels.Tarbela).ToString();
                            lblRTarbelaStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "TarbelaStorage");

                            txtRFillingLimit.Text = ((int)Constants.ReservoirLevels.TarbelaFillingLimit).ToString();
                            lblRFillingStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "TarbelaFillLimitStorage");

                            txtRChashmaLevel.Text = ((int)Constants.ReservoirLevels.Chashma).ToString();
                            lblRChashmaStorage.Text = Utility.GetDynamicPropertyValue(DataObj, "ChashmaStorage");

                            txtRChashmaMinLevel.Text = ((int)Constants.ReservoirLevels.ChashmaMinOptLevel).ToString();
                            lblRChashmaMinStorage.Text = ConvertToThreeDecimalPlaces(Utility.GetDynamicPropertyValue(DataObj, "ChashmaMinOptLevelStorage"));
                        }

                        DataObj = new SeasonalPlanningBLL().GetInflowsforRabiSeason(_IFDraftID, _Scenario);
                        if (DataObj != null)
                        {
                            if (_Scenario == "Minimum")
                                txtEasternRabi.Text = ConvertToThreeDecimalPlaces(((ConvertToDouble(new SeasonalPlanningBLL().GetEasternRabi((int)Constants.Seasons.Rabi, Convert.ToInt64(ddlRabiEastern.SelectedItem.Value)))) / 2));
                            else
                                txtEasternRabi.Text = new SeasonalPlanningBLL().GetEasternRabi((int)Constants.Seasons.Rabi, Convert.ToInt64(ddlRabiEastern.SelectedItem.Value));
                            lblJMRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "JhelumAtMangla");
                            lblCMRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "ChenabAtMarala");
                            lblITRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "IndusAtTarbela");
                            lblKNRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "KabulAtNowshera");
                            lblJCTotalRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(GetDynamicPropertyValue(DataObj, "JhelumAtMangla")) + ConvertToDouble(GetDynamicPropertyValue(DataObj, "ChenabAtMarala")) + ConvertToDouble(txtEasternRabi.Text));
                            lblICTotalRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(GetDynamicPropertyValue(DataObj, "IndusAtTarbela")) + ConvertToDouble(GetDynamicPropertyValue(DataObj, "KabulAtNowshera")));
                        }

                        lblJCInitialLevelRabi.Text = txtRJCManglaLevel.Text;
                        txtJCInitialStorageRabi.Text = lblRManglaStorage.Text;
                        lblICInitialLevelRabi.Text = txtRTarbelaLevel.Text;
                        txtICInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(lblRTarbelaStorage.Text);

                        lblJCMaxStorageRabi.Text = lblRManglaStorage.Text;
                        lblICMaxStorageRabi.Text = lblRTarbelaStorage.Text;

                        lblJCInitialStorageRabi.Text = txtJCInitialStorageRabi.Text;
                        lblICInitialStorageRabi.Text = txtICInitialStorageRabi.Text;

                        txtJCStrDepRabi.Text = ConvertToThreeDecimalPlaces(Constants.JCStorageDepretionRabi);
                        txtICStrDepRabi.Text = ConvertToThreeDecimalPlaces(Constants.ICStorageDepretionRabi);

                        lblJCStrRelRabi.Text = AppBlocks.Calculations.CalculateStorageRelease(Constants.JCStorageDepretionRabi, ConvertToDouble(lblRManglaStorage.Text), ConvertToDouble(txtJCInitialStorageRabi.Text));
                        lblICStrRelRabi.Text = AppBlocks.Calculations.CalculateStorageRelease(Constants.ICStorageDepretionRabi, ConvertToDouble(lblRTarbelaStorage.Text), ConvertToDouble(txtICInitialStorageRabi.Text));

                        lblJCSysInflowRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCTotalRabi.Text) + ConvertToDouble(lblJCStrRelRabi.Text));
                        txtJCSysLossGainPerRabi.Text = ConvertToString((int)Constants.LossGain.JhelumRabi);
                        lblJCSysLossGainMAFRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowRabi.Text) * (double)(ConvertToDouble(txtJCSysLossGainPerRabi.Text) / 100));
                        lblJCWaterAvailRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowRabi.Text) + ConvertToDouble(lblJCSysLossGainMAFRabi.Text));

                        DataObj = new SeasonalPlanningBLL().GetParaKPKBALRabiMAF();
                        if (DataObj != null)
                        {
                            lblJCParaRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "Para");
                            lblKPKBalRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "KPKBAL");
                            //  lblICParaRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaIndus");
                            lblICHistoricRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "Historic");
                        }

                        if (lblJCWaterAvailRabi.Text != "" && lblJCParaRabi.Text != "")
                        {
                            if (ConvertToDouble(lblJCWaterAvailRabi.Text) > ConvertToDouble(lblJCParaRabi.Text))
                                txtJCCanalAvailRabi.Text = lblJCParaRabi.Text;
                            else
                                txtJCCanalAvailRabi.Text = lblJCWaterAvailRabi.Text;
                        }

                        if (lblJCWaterAvailRabi.Text != "" && txtJCCanalAvailRabi.Text != "")
                            lblJCOutflowRabi.Text = ConvertToString(ConvertToDouble(lblJCWaterAvailRabi.Text) - ConvertToDouble(txtJCCanalAvailRabi.Text));
                        lblICOutflowRabi.Text = lblJCOutflowRabi.Text;

                        lblICSysInflowRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalRabi.Text) + ConvertToDouble(lblICStrRelRabi.Text) + ConvertToDouble(lblICOutflowRabi.Text));
                        txtICSysLossGainPerRabi.Text = ConvertToString((int)Constants.LossGain.IndusRabi);
                        lblICSysLossGainMAFRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowRabi.Text) * (double)(ConvertToDouble(txtICSysLossGainPerRabi.Text) / 100));
                        lblICWaterAvailRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowRabi.Text) + ConvertToDouble(lblICSysLossGainMAFRabi.Text));

                        //if (lblICWaterAvailRabi.Text != "" && lblICHistoricRabi.Text != "")
                        //{
                        //    if (ConvertToDouble(lblICWaterAvailRabi.Text) > ConvertToDouble(lblICHistoricRabi.Text))
                        //        txtICCanalAvailRabi.Text = lblICHistoricRabi.Text;
                        //    else
                        //        txtICCanalAvailRabi.Text = lblICWaterAvailRabi.Text;
                        //}
                        lblKotriRabi.Text = "0.000";

                        txtICCanalAvailRabi.Text = ConvertToString(ConvertToDouble(lblICWaterAvailRabi.Text) - ConvertToDouble(lblKotriRabi.Text));

                        if (txtICCanalAvailRabi.Text != "" && lblKPKBalRabi.Text != "")
                            lblPunjSindhRabi.Text = ConvertToString(ConvertToDouble(txtICCanalAvailRabi.Text) - ConvertToDouble(lblKPKBalRabi.Text));

                        //if (lblICWaterAvailRabi.Text != "" && txtICCanalAvailRabi.Text != "")
                        //    lblKotriRabi.Text = ConvertToString(ConvertToDouble(lblICWaterAvailRabi.Text) - ConvertToDouble(txtICCanalAvailRabi.Text));

                        if (txtJCCanalAvailRabi.Text != "" && lblJCParaRabi.Text != "")
                            lblJCShortageRabi.Text = ConvertToThreeDecimalPlaces(100 - ((ConvertToDouble(txtJCCanalAvailRabi.Text) / ConvertToDouble(lblJCParaRabi.Text)) * 100));

                        if (lblPunjSindhRabi.Text != "" && lblICHistoricRabi.Text != "")
                            lblICShortageRabi.Text = ConvertToThreeDecimalPlaces(100 - ((ConvertToDouble(lblPunjSindhRabi.Text) / ConvertToDouble(lblICHistoricRabi.Text)) * 100));
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditMCLLevelsForRabiMangla()
        {
            try
            {
                if (ConvertToDouble(txtRJCManglaLevel.Text) >= (int)Constants.ReservoirLevelsLimits.JhelumMin && ConvertToDouble(txtRJCManglaLevel.Text) <= (int)Constants.ReservoirLevelsLimits.IndusMax)
                {
                    lblRManglaStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, Convert.ToDouble(txtRJCManglaLevel.Text)));
                    txtJCInitialStorageRabi.Text = lblRManglaStorage.Text;
                }
                else
                    Master.ShowMessage(Message.ReservoirLimitExceeded.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditMCLLevelsForRabiIndus()
        {
            try
            {
                if (ConvertToDouble(txtRTarbelaLevel.Text) >= (int)Constants.ReservoirLevelsLimits.IndusMin && ConvertToDouble(txtRTarbelaLevel.Text) <= (int)Constants.ReservoirLevelsLimits.IndusMax)
                    lblRTarbelaStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, ConvertToDouble(txtRTarbelaLevel.Text)));
                else
                    Master.ShowMessage(Message.ReservoirLimitExceeded.Description, SiteMaster.MessageType.Error);

                if (ConvertToDouble(txtRFillingLimit.Text) >= (int)Constants.ReservoirLevelsLimits.IndusMin && ConvertToDouble(txtRFillingLimit.Text) <= (int)Constants.ReservoirLevelsLimits.IndusMax)
                {
                    if (ConvertToDouble(txtRFillingLimit.Text) <= ConvertToDouble(txtRTarbelaLevel.Text))
                        lblRFillingStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, ConvertToDouble(txtRFillingLimit.Text)));
                    else
                        Master.ShowMessage(Message.FillingLimitCannotBeGreater.Description, SiteMaster.MessageType.Error);
                }
                else
                    Master.ShowMessage(Message.ReservoirLimitExceeded.Description, SiteMaster.MessageType.Error);

                if (ConvertToDouble(txtRChashmaLevel.Text) >= (int)Constants.ReservoirLevelsLimits.ChashmaMin && ConvertToDouble(txtRChashmaLevel.Text) <= (int)Constants.ReservoirLevelsLimits.ChashmaMax)
                    lblRChashmaStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.Chashma, ConvertToDouble(txtRChashmaLevel.Text)));
                else
                    Master.ShowMessage(Message.ReservoirLimitExceeded.Description, SiteMaster.MessageType.Error);

                if (ConvertToDouble(txtRChashmaMinLevel.Text) >= (int)Constants.ReservoirLevelsLimits.ChashmaMin && ConvertToDouble(txtRChashmaMinLevel.Text) <= (int)Constants.ReservoirLevelsLimits.ChashmaMax)
                    lblRChashmaMinStorage.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.Chashma, ConvertToDouble(txtRChashmaMinLevel.Text)));
                else
                    Master.ShowMessage(Message.ReservoirLimitExceeded.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditEasternRegionForRabiMangla()
        {
            try
            {
                lblJCTotalRabi.Text = ConvertToString(ConvertToDouble(GetInflowsWithoutPercentage(lblJMRabi.Text)) + ConvertToDouble(GetInflowsWithoutPercentage(lblCMRabi.Text)) + ConvertToDouble(txtEasternRabi.Text));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditReservirLevelRegionForRabiMangla()
        {
            try
            {
                if (ConvertToDouble(txtJCInitialStorageRabi.Text) <= ConvertToDouble(lblRManglaStorage.Text))
                    lblJCInitialLevelRabi.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, ConvertToDouble(txtJCInitialStorageRabi.Text)));
                else
                {
                    txtJCInitialStorageRabi.Text = lblRManglaStorage.Text;
                    lblJCInitialLevelRabi.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, ConvertToDouble(txtJCInitialStorageRabi.Text)));
                    Master.ShowMessage(Message.MCLMustBeLess.Description, SiteMaster.MessageType.Error);
                }

                lblJCMaxStorageRabi.Text = ConvertToThreeDecimalPlaces(lblRManglaStorage.Text);
                lblJCInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(txtJCInitialStorageRabi.Text);

                if (ConvertToDouble(txtJCStrDepRabi.Text) <= 100)
                    lblJCStrRelRabi.Text = AppBlocks.Calculations.CalculateStorageRelease(ConvertToDouble(txtJCStrDepRabi.Text), ConvertToDouble(lblRManglaStorage.Text), ConvertToDouble(txtJCInitialStorageRabi.Text));
                else
                    Master.ShowMessage(Message.StorageDepressionLimit.Description, SiteMaster.MessageType.Error);

                lblJCSysInflowRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCTotalRabi.Text) + ConvertToDouble(lblJCStrRelRabi.Text));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditReservirLevelRegionForRabiIndus()
        {
            try
            {
                if (ConvertToDouble(txtICInitialStorageRabi.Text) <= ConvertToDouble(lblRTarbelaStorage.Text))
                    lblICInitialLevelRabi.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, ConvertToDouble(txtICInitialStorageRabi.Text)));
                else
                {
                    txtICInitialStorageRabi.Text = lblRTarbelaStorage.Text;
                    lblICInitialLevelRabi.Text = ConvertToThreeDecimalPlaces(new SeasonalPlanningBLL().GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, ConvertToDouble(txtICInitialStorageRabi.Text)));
                    Master.ShowMessage(Message.MCLMustBeLess.Description, SiteMaster.MessageType.Error);
                }

                lblICMaxStorageRabi.Text = ConvertToThreeDecimalPlaces(lblRTarbelaStorage.Text);
                lblICInitialStorageRabi.Text = ConvertToThreeDecimalPlaces(txtICInitialStorageRabi.Text);

                if (ConvertToDouble(txtICStrDepRabi.Text) <= 100)
                    lblICStrRelRabi.Text = AppBlocks.Calculations.CalculateStorageRelease(ConvertToDouble(txtICStrDepRabi.Text), ConvertToDouble(lblRTarbelaStorage.Text), ConvertToDouble(txtICInitialStorageRabi.Text));
                else
                    Master.ShowMessage(Message.StorageDepressionLimit.Description, SiteMaster.MessageType.Error);
                lblICSysInflowRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalRabi.Text) + ConvertToDouble(lblICStrRelRabi.Text) + ConvertToDouble(lblICOutflowRabi.Text));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditSystemInflowsRegionForRabiMangla()
        {
            try
            {
                Object DataObj = new object();
                if (ConvertToDouble(txtJCSysLossGainPerRabi.Text) <= 100 && ConvertToDouble(txtJCSysLossGainPerRabi.Text) >= -100)
                    lblJCSysLossGainMAFRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowRabi.Text) * (double)(ConvertToDouble(txtJCSysLossGainPerRabi.Text) / 100));
                else
                    Master.ShowMessage(Message.LossGainLimit.Description, SiteMaster.MessageType.Error);

                lblJCWaterAvailRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCSysInflowRabi.Text) + ConvertToDouble(lblJCSysLossGainMAFRabi.Text));


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditSystemInflowsRegionForRabiIndus()
        {
            try
            {
                Object DataObj = new object();
                if (ConvertToDouble(lblICSysInflowRabi.Text) <= 100 && ConvertToDouble(lblICSysInflowRabi.Text) >= -100)
                    lblICSysLossGainMAFRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowRabi.Text) * (double)(ConvertToDouble(txtICSysLossGainPerRabi.Text) / 100));
                else
                    Master.ShowMessage(Message.LossGainLimit.Description, SiteMaster.MessageType.Error);

                lblICWaterAvailRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICSysInflowRabi.Text) + ConvertToDouble(lblICSysLossGainMAFRabi.Text));
                DataObj = new SeasonalPlanningBLL().GetParaKPKBALRabiMAF();
                if (DataObj != null)
                {
                    lblJCParaRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "Para");
                    lblKPKBalRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "KPKBAL");
                    //  lblICParaRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "ParaIndus");
                    lblICHistoricRabi.Text = Utility.GetDynamicPropertyValue(DataObj, "Historic");
                }

                //if (lblICWaterAvailRabi.Text != "" && lblICHistoricRabi.Text != "")
                //{
                //    if (ConvertToDouble(lblICWaterAvailRabi.Text) > ConvertToDouble(lblICHistoricRabi.Text))
                //        txtICCanalAvailRabi.Text = lblICHistoricRabi.Text;
                //    else
                //        txtICCanalAvailRabi.Text = lblICWaterAvailRabi.Text;
                //}

                lblKotriRabi.Text = "0.000";
                txtICCanalAvailRabi.Text = ConvertToString(ConvertToDouble(lblICWaterAvailRabi.Text) - ConvertToDouble(lblKotriRabi.Text));

                if (txtICCanalAvailRabi.Text != "" && lblKPKBalRabi.Text != "")
                    lblPunjSindhRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(txtICCanalAvailRabi.Text) - ConvertToDouble(lblKPKBalRabi.Text));

                //if (lblICWaterAvailRabi.Text != "" && txtICCanalAvailRabi.Text != "")
                //    lblKotriRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICWaterAvailRabi.Text) - ConvertToDouble(txtICCanalAvailRabi.Text));


                if (lblPunjSindhRabi.Text != "" && lblICHistoricRabi.Text != "")
                    lblICShortageRabi.Text = ConvertToThreeDecimalPlaces(100 - ((ConvertToDouble(lblPunjSindhRabi.Text) / ConvertToDouble(lblICHistoricRabi.Text)) * 100));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditCanalAvailabilityforJhelum()
        {
            try
            {
                if (lblJCWaterAvailRabi.Text != "" && txtJCCanalAvailRabi.Text != "")
                    lblJCOutflowRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblJCWaterAvailRabi.Text) - ConvertToDouble(txtJCCanalAvailRabi.Text));
                lblICOutflowRabi.Text = lblJCOutflowRabi.Text;

                // to have impact of Canal Availability change Indus side 
                lblICSysInflowRabi.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(lblICTotalRabi.Text) + ConvertToDouble(lblICStrRelRabi.Text) + ConvertToDouble(lblICOutflowRabi.Text));
                EditSystemInflowsRegionForRabiIndus();
                //
                if (txtJCCanalAvailRabi.Text != "" && lblJCParaRabi.Text != "")
                    lblJCShortageRabi.Text = ConvertToThreeDecimalPlaces(100 - ((ConvertToDouble(txtJCCanalAvailRabi.Text) / ConvertToDouble(lblJCParaRabi.Text)) * 100));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtRJCManglaLevel_TextChanged(object sender, EventArgs e)
        {
            EditMCLLevelsForRabiMangla();
            EditEasternRegionForRabiMangla();
            EditReservirLevelRegionForRabiMangla();
            EditSystemInflowsRegionForRabiMangla();
            EditCanalAvailabilityforJhelum();
            EditViewTrue();
            txtEasternRabi.Focus();
        }

        protected void txtRTarbelaLevel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditMCLLevelsForRabiIndus();
                EditReservirLevelRegionForRabiIndus();
                EditSystemInflowsRegionForRabiIndus();
                EditViewTrue();

                if (((TextBox)sender).ID == "txtRTarbelaLevel")
                    txtRFillingLimit.Focus();
                else if (((TextBox)sender).ID == "txtRFillingLimit")
                    txtRChashmaLevel.Focus();
                else if (((TextBox)sender).ID == "txtRChashmaLevel")
                    txtRChashmaMinLevel.Focus();
                else if (((TextBox)sender).ID == "txtRChashmaMinLevel")
                    txtICInitialStorageRabi.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtEasternRabi_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditEasternRegionForRabiMangla();
                EditReservirLevelRegionForRabiMangla();
                EditSystemInflowsRegionForRabiMangla();
                EditCanalAvailabilityforJhelum();
                EditViewTrue();
                txtJCInitialStorageRabi.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJCInitialStorageRabi_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditReservirLevelRegionForRabiMangla();
                EditSystemInflowsRegionForRabiMangla();
                EditCanalAvailabilityforJhelum();
                EditViewTrue();

                if (((TextBox)sender).ID == "txtJCInitialStorageRabi")
                    txtJCStrDepRabi.Focus();
                else if (((TextBox)sender).ID == "txtJCStrDepRabi")
                    txtJCSysLossGainPerRabi.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtICInitialStorageRabi_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditReservirLevelRegionForRabiIndus();
                EditSystemInflowsRegionForRabiIndus();
                EditViewTrue();

                if (((TextBox)sender).ID == "txtICInitialStorageRabi")
                    txtICStrDepRabi.Focus();
                else if (((TextBox)sender).ID == "txtICStrDepRabi")
                    txtICSysLossGainPerRabi.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJCSysLossGainPerRabi_TextChanged(object sender, EventArgs e)
        {
            EditSystemInflowsRegionForRabiMangla();
            EditCanalAvailabilityforJhelum();
            EditViewTrue();
            txtJCCanalAvailRabi.Focus();
        }

        protected void txtICSysLossGainPerRabi_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditSystemInflowsRegionForRabiIndus();
                EditViewTrue();
                txtICCanalAvailRabi.Focus();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtJCCanalAvailRabi_TextChanged(object sender, EventArgs e)
        {
            try
            {
                EditCanalAvailabilityforJhelum();
                EditViewTrue();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void EditViewTrue()
        {
            try
            {
                txtRJCManglaLevel.Enabled = true;
                txtRTarbelaLevel.Enabled = true;
                txtRFillingLimit.Enabled = true;
                txtRChashmaLevel.Enabled = true;
                txtRChashmaMinLevel.Enabled = true;
                txtEasternRabi.Enabled = true;
                txtJCInitialStorageRabi.Enabled = true;
                txtICInitialStorageRabi.Enabled = true;
                txtJCStrDepRabi.Enabled = true;
                txtICStrDepRabi.Enabled = true;
                txtJCSysLossGainPerRabi.Enabled = true;
                txtICSysLossGainPerRabi.Enabled = true;
                txtJCCanalAvailRabi.Enabled = true;
                txtICCanalAvailRabi.Enabled = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region Initial Screens and Save functionality

        public void BindInflowForecastingDraft()
        {
            try
            {
                if ((int)Constants.Seasons.Rabi == Utility.GetCurrentSeasonForView())
                {
                    gvView.DataSource = new SeasonalPlanningBLL().GetDraftsInformation();
                    gvView.DataBind();
                }
                else
                {
                    gvView.DataSource = new SeasonalPlanningBLL().GetSelectedDraftsInformation();
                    gvView.DataBind();
                }

                divForecastDrafts.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                long ForecastDraftID = e.NewEditIndex;


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSaveViewDiv_Click(object sender, EventArgs e)
        {
            try
            {
                bool Selection = false;
                SP_PlanDraft objDraft = new SP_PlanDraft();
                objDraft.Description = txtName.Text;
                foreach (GridViewRow row in gvView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox cb = (row.Cells[1].FindControl("cbSelection") as CheckBox);
                        if (cb.Checked)
                        {
                            objDraft.ForecastDraftID = Convert.ToInt64(((Label)(row.Cells[0].FindControl("lblID"))).Text);
                            Selection = true;
                            break;
                        }
                    }
                }

                if (Selection)
                {
                    objDraft.SeasonID = (short)Utility.GetCurrentSeasonForView();
                    objDraft.Year = (short)(DateTime.Now.Year);
                    objDraft.IsApproved = false;
                    objDraft.IsValid = false;
                    objDraft.IsPara2 = false;
                    objDraft.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    objDraft.CreatedDate = DateTime.Now;
                    objDraft.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                    objDraft.ModifiedDate = DateTime.Now;
                    SPDraftID = new SeasonalPlanningBLL().SaveDraft(objDraft);
                    if (SPDraftID != -1)
                    {
                        divSPDrafts.Visible = true;
                        divForecastDrafts.Visible = false;
                        BindSeasonalPlanningDrafts();
                    }
                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
                else
                    Master.ShowMessage(Message.ForecastDraftNotSelect.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {

                bool Result = SetDefaultDraftName();
                if (Result)
                {
                    BindInflowForecastingDraft();
                    divSPDrafts.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public bool SetDefaultDraftName()
        {
            string DraftName = "";
            string Description = "Seasonal Planning Draft for ";
            bool Result = true;
            int Season = Utility.GetCurrentSeasonForView();
            try
            {
                if (Season == (int)Constants.Seasons.Kharif)
                {
                    DraftName = new SeasonalPlanningBLL().GetSeasonalDraftCountName(Season);
                    if (DraftName.ToUpper() == "NOT ALLOWED")
                    {
                        Result = false;
                        Master.ShowMessage(Message.MoreDraftsNotAllowed.Description, SiteMaster.MessageType.Error);
                    }
                    else
                        txtName.Text = DraftName + Description + "Kharif " + DateTime.Now.Year.ToString();
                }
                else
                {
                    DraftName = new SeasonalPlanningBLL().GetSeasonalDraftCountName(Season);
                    if (DraftName.ToUpper() == "NOT ALLOWED")
                    {
                        Result = false;
                        Master.ShowMessage(Message.MoreDraftsNotAllowed.Description, SiteMaster.MessageType.Error);
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

        protected void lbtnCancelViewDiv_Click(object sender, EventArgs e)
        {
            divSPDrafts.Visible = true;
            divForecastDrafts.Visible = false;
            divforecast.Visible = false;
        }

        public void BindSeasonalPlanningDrafts()
        {
            try
            {
                gvSPDraftsView.DataSource = new SeasonalPlanningBLL().GetSeasonalDrafts();
                gvSPDraftsView.DataBind();

                //if ((int)Constants.Seasons.Rabi == Utility.GetCurrentSeasonForView())
                //{
                //    gvSPDraftsView.DataSource = new SeasonalPlanningBLL().GetSeasonalDrafts((int)Constants.InflowForecstDrafts.StatisticalDraft);
                //    gvSPDraftsView.DataBind();
                //}
                //else
                //{
                //    gvSPDraftsView.DataSource = new SeasonalPlanningBLL().GetSeasonalDrafts((int)Constants.InflowForecstDrafts.SelectedDraft);
                //    gvSPDraftsView.DataBind();
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSPDraftsView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSPDraftsView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int ID = gvr.RowIndex;
                SPDraftID = Convert.ToInt64(((Label)gvSPDraftsView.Rows[ID].FindControl("lblSPID")).Text);

                if (e.CommandName == "Approve")
                {
                    long Result = new SeasonalPlanningBLL().ApproveDraft(SPDraftID);
                    if (Result != -1)
                    {
                        if (Result == 1)
                            Master.ShowMessage(Message.DraftApproved.Description, SiteMaster.MessageType.Success);
                        else if (Result == 0)
                            Master.ShowMessage(Message.DraftUnApproved.Description, SiteMaster.MessageType.Success);
                        BindSeasonalPlanningDrafts();
                    }
                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
                else // Max Min and Likely Case
                {
                    lblRabiScenarioName.Text = e.CommandName.ToString();
                    ManglaIndusOperations.SeasonalDraftID = SPDraftID;
                    ManglaIndusOperations.ForecastDraftID = Convert.ToInt64(e.CommandArgument);
                    ManglaIndusOperations.Scenario = e.CommandName;

                    IndusOperations.SeasonalDraftID = SPDraftID;
                    IndusOperations.ForecastDraftID = Convert.ToInt64(e.CommandArgument);
                    IndusOperations.Scenario = e.CommandName;

                    AnticipatedJC._ForecastDraftID = Convert.ToInt64(e.CommandArgument);
                    AnticipatedJC._PlanDraftID = SPDraftID;
                    AnticipatedJC._Scenario = e.CommandName;

                    AnticipatedIndusRiver._ForecastDraftID = Convert.ToInt64(e.CommandArgument);
                    AnticipatedIndusRiver._PlanDraftID = SPDraftID;
                    AnticipatedIndusRiver._Scenario = e.CommandName;

                    AnticipatedIndusBasin._ForecastDraftID = Convert.ToInt64(e.CommandArgument);
                    AnticipatedIndusBasin._PlanDraftID = SPDraftID;
                    AnticipatedIndusBasin._Scenario = e.CommandName;
                    htitle.InnerText = "Seasonal Planning-" + e.CommandName.ToString() + " Scenario";

                    if (e.CommandName == "Likely")
                    {
                        //btnREdit.Visible = false;
                        //btnEdit.Visible = false;
                        //btnRabiSave.Visible = false;
                        //btnKharifSave.Visible = false;
                        if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                            SetLikelyFieldsKharif(Convert.ToInt64(e.CommandArgument), (int)Constants.Seasons.Kharif);
                        else
                            SetLikelyFieldsKharif(Convert.ToInt64(e.CommandArgument), (int)Constants.Seasons.Rabi);
                    }
                    else
                    {
                        btnEdit.Visible = true;
                        btnREdit.Visible = true;
                        divSPDrafts.Visible = false;
                        divPanel.Visible = true;
                        //btnRabiSave.Visible = true;
                        //btnKharifSave.Visible = true;
                        if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                            SetKharifBalanceReservoirFields(Convert.ToInt64(e.CommandArgument), e.CommandName);
                        else
                            SetRabiBalanceReservoirFields(Convert.ToInt64(e.CommandArgument), e.CommandName);

                        if (!Utility.PlanningDaysLimit())
                        {
                            btnRabiSave.Visible = false;
                            btnKharifSave.Visible = false;
                        }
                        else
                        {
                            btnRabiSave.Visible = true;
                            btnKharifSave.Visible = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnRabiSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool Result = false;
                SP_PlanScenario ObjScenario = new SP_PlanScenario();
                SP_PlanBalance BalanceObj = new SP_PlanBalance();

                ObjScenario.Scenario = lblRabiScenarioName.Text.Substring(0, 7).ToString().TrimEnd();
                ObjScenario.PlanDraftID = SPDraftID;
                ObjScenario.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                ObjScenario.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjScenario.CreatedDate = DateTime.Now;
                ObjScenario.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjScenario.ModifiedDate = DateTime.Now;
                BalanceObj.PlanScenarioID = new SeasonalPlanningBLL().SaveSeasonalPlanningScenario(ObjScenario, true);

                BalanceObj.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                BalanceObj.MCL = Convert.ToDouble(txtRJCManglaLevel.Text);
                BalanceObj.Storage = ConvertToDouble(lblRManglaStorage.Text);
                BalanceObj.JhelumRabi = ConvertToDouble(GetInflowsWithoutPercentage(lblJMRabi.Text));
                BalanceObj.ChenabRabi = ConvertToDouble(GetInflowsWithoutPercentage(lblCMRabi.Text));
                BalanceObj.Eastern = ConvertToDouble(txtEasternRabi.Text);
                if (ObjScenario.Scenario != "Likely")
                    BalanceObj.EasternYears = Convert.ToDouble(ddlKharifEastern.SelectedItem.Value);
                BalanceObj.Inflow = ConvertToDouble(lblJCTotalRabi.Text);
                BalanceObj.InitResLevel = ConvertToDouble(lblJCInitialLevelRabi.Text);
                BalanceObj.InitStorage = ConvertToDouble(txtJCInitialStorageRabi.Text);
                BalanceObj.MaxStorage = ConvertToDouble(lblJCMaxStorageRabi.Text);
                BalanceObj.StorageAvailable = ConvertToDouble(lblJCInitialStorageRabi.Text);
                BalanceObj.StorageDep = ConvertToDouble(txtJCStrDepRabi.Text);
                BalanceObj.StorageRelease = ConvertToDouble(lblJCStrRelRabi.Text);
                BalanceObj.TotalSysInfow = ConvertToDouble(lblJCSysInflowRabi.Text);
                BalanceObj.SysLossGain = ConvertToDouble(txtJCSysLossGainPerRabi.Text);
                BalanceObj.SysLossGainVol = ConvertToDouble(lblJCSysLossGainMAFRabi.Text);
                BalanceObj.TotalAvailability = ConvertToDouble(lblJCWaterAvailRabi.Text);
                BalanceObj.Para2 = ConvertToDouble(lblJCParaRabi.Text);
                BalanceObj.CanalAvailability = ConvertToDouble(txtJCCanalAvailRabi.Text);
                BalanceObj.JCOutflow = ConvertToDouble(lblJCOutflowRabi.Text);
                BalanceObj.Shortage = ConvertToDouble(lblJCShortageRabi.Text);
                BalanceObj.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                BalanceObj.CreatedDate = DateTime.Now;
                BalanceObj.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                BalanceObj.ModifiedDate = DateTime.Now;
                Result = new SeasonalPlanningBLL().SaveSeasonalPlanningBalance(BalanceObj);

                ObjScenario = new SP_PlanScenario();
                BalanceObj = new SP_PlanBalance();
                ObjScenario.Scenario = lblRabiScenarioName.Text.Substring(0, 7).ToString().TrimEnd();
                ObjScenario.PlanDraftID = SPDraftID;
                ObjScenario.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                ObjScenario.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjScenario.CreatedDate = DateTime.Now;
                ObjScenario.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjScenario.ModifiedDate = DateTime.Now;
                BalanceObj.PlanScenarioID = new SeasonalPlanningBLL().SaveSeasonalPlanningScenario(ObjScenario, true);


                BalanceObj.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                BalanceObj.MCL = Convert.ToDouble(txtRTarbelaLevel.Text);
                BalanceObj.Storage = ConvertToDouble(lblRTarbelaStorage.Text);
                BalanceObj.FillingLimit = ConvertToDouble(txtRFillingLimit.Text);
                BalanceObj.FillingLimitStorage = ConvertToDouble(lblRFillingStorage.Text);
                BalanceObj.ChashmaResLevel = ConvertToDouble(txtRChashmaLevel.Text);
                BalanceObj.ChashmaStorage = ConvertToDouble(lblRChashmaStorage.Text);
                BalanceObj.ChashmaMinReslevel = ConvertToDouble(txtRChashmaMinLevel.Text);
                BalanceObj.ChashmaMinStorage = ConvertToDouble(lblRChashmaMinStorage.Text);
                BalanceObj.IndusRabi = ConvertToDouble(GetInflowsWithoutPercentage(lblITRabi.Text));
                BalanceObj.KabulRabi = ConvertToDouble(GetInflowsWithoutPercentage(lblKNRabi.Text));
                BalanceObj.Inflow = ConvertToDouble(lblICTotalRabi.Text);
                BalanceObj.InitResLevel = ConvertToDouble(lblICInitialLevelRabi.Text);
                BalanceObj.InitStorage = ConvertToDouble(txtICInitialStorageRabi.Text);
                BalanceObj.MaxStorage = ConvertToDouble(lblICMaxStorageRabi.Text);
                BalanceObj.StorageAvailable = ConvertToDouble(lblICInitialStorageRabi.Text);
                BalanceObj.StorageDep = ConvertToDouble(txtICStrDepRabi.Text);
                BalanceObj.StorageRelease = ConvertToDouble(lblICStrRelRabi.Text);
                BalanceObj.TotalSysInfow = ConvertToDouble(lblICSysInflowRabi.Text);
                BalanceObj.SysLossGain = ConvertToDouble(txtICSysLossGainPerRabi.Text);
                BalanceObj.SysLossGainVol = ConvertToDouble(lblICSysLossGainMAFRabi.Text);
                BalanceObj.TotalAvailability = ConvertToDouble(lblICWaterAvailRabi.Text);
                BalanceObj.KPKBalochistan = ConvertToDouble(lblKPKBalRabi.Text);
                BalanceObj.BalPunSindh = ConvertToDouble(lblPunjSindhRabi.Text);
                //  BalanceObj.Para2 = ConvertToDouble(lblICParaRabi.Text);
                BalanceObj.CanalAvailability = ConvertToDouble(txtICCanalAvailRabi.Text);
                BalanceObj.Kotri = ConvertToDouble(lblKotriRabi.Text);
                BalanceObj.JCOutflow = ConvertToDouble(lblICOutflowRabi.Text);
                BalanceObj.Historic = ConvertToDouble(lblICHistoricRabi.Text);
                BalanceObj.Shortage = ConvertToDouble(lblICShortageRabi.Text);
                BalanceObj.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                BalanceObj.CreatedDate = DateTime.Now;
                BalanceObj.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                BalanceObj.ModifiedDate = DateTime.Now;
                Result = new SeasonalPlanningBLL().SaveSeasonalPlanningBalance(BalanceObj);

                if (sender != null)
                {
                    if (!Result)
                    {
                        new SeasonalPlanningBLL().DeleteSeasonalIncompleteDraft(SPDraftID, ObjScenario.StationID, ObjScenario.Scenario);
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                    else
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnKharifSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool Result = false;
                SP_PlanScenario ObjScenario = new SP_PlanScenario();
                SP_PlanBalance BalanceObj = new SP_PlanBalance();
                ObjScenario = new SP_PlanScenario();
                ObjScenario.Scenario = lblScenarioName.Text.Substring(0, 7).ToString().TrimEnd();
                ObjScenario.PlanDraftID = SPDraftID;
                ObjScenario.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                ObjScenario.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjScenario.CreatedDate = DateTime.Now;
                ObjScenario.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjScenario.ModifiedDate = DateTime.Now;
                BalanceObj.PlanScenarioID = new SeasonalPlanningBLL().SaveSeasonalPlanningScenario(ObjScenario, true);

                BalanceObj.StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
                BalanceObj.MCL = Convert.ToDouble(txtJCManglaLevel.Text);
                BalanceObj.Storage = ConvertToDouble(lblManglaStorage.Text);
                BalanceObj.JhelumEK = ConvertToDouble(GetInflowsWithoutPercentage(lblJMEK.Text));
                BalanceObj.JhelumLK = ConvertToDouble(GetInflowsWithoutPercentage(lblJMLK.Text));
                BalanceObj.Jhelum = ConvertToDouble(lblJMTotal.Text);
                BalanceObj.ChenabEK = ConvertToDouble(GetInflowsWithoutPercentage(lblCMEK.Text));
                BalanceObj.ChenabLK = ConvertToDouble(GetInflowsWithoutPercentage(lblCMLK.Text));
                BalanceObj.Chenab = ConvertToDouble(lblCMTotal.Text);
                BalanceObj.EasternEK = ConvertToDouble(txtEasternEK.Text);
                BalanceObj.EasternLK = ConvertToDouble(txtEasternLK.Text);
                BalanceObj.Eastern = ConvertToDouble(lblEasternTotal.Text);
                if (ObjScenario.Scenario != "Likely")
                    BalanceObj.EasternYears = Convert.ToDouble(ddlKharifEastern.SelectedItem.Value);
                BalanceObj.InitResLevel = ConvertToDouble(lblJCInitialLevel.Text);
                BalanceObj.InitStorage = ConvertToDouble(txtJCInitialStorage.Text);
                BalanceObj.StrtofillEK = ConvertToDouble(txtJCStrToFillEK.Text);
                BalanceObj.StrtofillLK = ConvertToDouble(txtJCStrToFillLK.Text);
                BalanceObj.FlowsEK = ConvertToDouble(lblJCStrFillEKMAF.Text);
                BalanceObj.FlowsLK = ConvertToDouble(lblJCStrFillLKMAF.Text);
                BalanceObj.StorageDep = ConvertToDouble(txtJCStrDep.Text);
                BalanceObj.StorageRelease = ConvertToDouble(lblJCStrRel.Text);
                BalanceObj.SysInflowsEK = ConvertToDouble(lblJCSysInflowEK.Text);
                BalanceObj.SysInflowsLK = ConvertToDouble(lblJCSysInflowLK.Text);
                BalanceObj.EarlyKharif = ConvertToDouble(txtJCSysLossGainPerEK.Text);
                BalanceObj.LateKharif = ConvertToDouble(txtJCSysLossGainPerLK.Text);

                //Loss gain MAF 
                //BalanceObj.SysLossGainVol = ConvertToDouble(lblJCSysLossGainMAFRabi.Text);                

                BalanceObj.TotalAvailabilityEK = ConvertToDouble(lblJCWaterAvailEK.Text);
                BalanceObj.TotalAvailabilityLK = ConvertToDouble(lblJCWaterAvailLK.Text);
                BalanceObj.TotalAvailability = ConvertToDouble(lblJCWaterAvailTotal.Text);
                BalanceObj.Para2EK = ConvertToDouble(lblJCParaEK.Text);
                BalanceObj.Para2LK = ConvertToDouble(lblJCParaLK.Text);
                BalanceObj.Para2 = ConvertToDouble(lblJCParaTotal.Text);
                BalanceObj.CanalAvailabilityEK = ConvertToDouble(txtJCCanalAvailEK.Text);
                BalanceObj.CanalAvailabilityLK = ConvertToDouble(txtJCCanalAvailLK.Text);
                BalanceObj.CanalAvailability = ConvertToDouble(lblJCCanalAvailTotal.Text);
                BalanceObj.JCOutflowEK = ConvertToDouble(lblJCOutflowEK.Text);
                BalanceObj.JCoutflowLK = ConvertToDouble(lblJCOutflowLK.Text);
                BalanceObj.JCOutflow = ConvertToDouble(lblJCOutflowTotal.Text);
                BalanceObj.ShortageEK = ConvertToDouble(lblJCShortageEK.Text);
                BalanceObj.ShortageLK = ConvertToDouble(lblJCShortageLK.Text);
                BalanceObj.Shortage = ConvertToDouble(lblJCShortageTotal.Text);
                BalanceObj.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                BalanceObj.CreatedDate = DateTime.Now;
                BalanceObj.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                BalanceObj.ModifiedDate = DateTime.Now;
                Result = new SeasonalPlanningBLL().SaveSeasonalPlanningBalance(BalanceObj);

                ObjScenario = new SP_PlanScenario();
                BalanceObj = new SP_PlanBalance();
                ObjScenario.Scenario = lblScenarioName.Text.Substring(0, 7).ToString().TrimEnd();
                ObjScenario.PlanDraftID = SPDraftID;
                ObjScenario.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                ObjScenario.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjScenario.CreatedDate = DateTime.Now;
                ObjScenario.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                ObjScenario.ModifiedDate = DateTime.Now;
                BalanceObj.PlanScenarioID = new SeasonalPlanningBLL().SaveSeasonalPlanningScenario(ObjScenario, true);

                BalanceObj.StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
                BalanceObj.MCL = Convert.ToDouble(txtTarbelaLevel.Text);
                BalanceObj.Storage = ConvertToDouble(lblTarbelaStorage.Text);
                BalanceObj.FillingLimit = ConvertToDouble(txtFillingLimit.Text);
                BalanceObj.FillingLimitStorage = ConvertToDouble(lblFillingStorage.Text);
                BalanceObj.ChashmaResLevel = ConvertToDouble(txtChashmaLevel.Text);
                BalanceObj.ChashmaStorage = ConvertToDouble(lblChashmaStorage.Text);
                BalanceObj.ChashmaMinReslevel = ConvertToDouble(txtChashmaMinLevel.Text);
                BalanceObj.ChashmaMinStorage = ConvertToDouble(lblChashmaMinStorage.Text);
                BalanceObj.IndusEK = ConvertToDouble(GetInflowsWithoutPercentage(lblITEK.Text));
                BalanceObj.IndusLK = ConvertToDouble(GetInflowsWithoutPercentage(lblITLK.Text));
                BalanceObj.Indus = ConvertToDouble(lblITTotal.Text);
                BalanceObj.KabulEK = ConvertToDouble(GetInflowsWithoutPercentage(lblKNEK.Text));
                BalanceObj.KabulLK = ConvertToDouble(GetInflowsWithoutPercentage(lblKNLK.Text));
                BalanceObj.Kabul = ConvertToDouble(lblKNotal.Text);
                BalanceObj.InitResLevel = ConvertToDouble(lblICInitialLevel.Text);
                BalanceObj.InitStorage = ConvertToDouble(txtICInitialStorage.Text);
                BalanceObj.StrtofillEK = ConvertToDouble(txtICStrToFillEK.Text);
                BalanceObj.StrtofillLK = ConvertToDouble(txtICStrToFillLK.Text);
                BalanceObj.FlowsEK = ConvertToDouble(lblICStrFillEKMAF.Text);
                BalanceObj.FlowsLK = ConvertToDouble(lblICStrFillLKMAF.Text);
                BalanceObj.StorageDep = ConvertToDouble(txtICStrDep.Text);
                BalanceObj.StorageRelease = ConvertToDouble(lblICStrRel.Text);
                BalanceObj.SysInflowsEK = ConvertToDouble(lblICSysInflowEK.Text);
                BalanceObj.SysInflowsLK = ConvertToDouble(lblICSysInflowLK.Text);
                BalanceObj.EarlyKharif = ConvertToDouble(txtICSysLossGainPerEK.Text);
                BalanceObj.LateKharif = ConvertToDouble(txtICSysLossGainPerLK.Text);

                //Loss gain MAF 
                //BalanceObj.SysLossGainVol = ConvertToDouble(lblJCSysLossGainMAFRabi.Text);                

                BalanceObj.TotalAvailabilityEK = ConvertToDouble(lblICWaterAvailEK.Text);
                BalanceObj.TotalAvailabilityLK = ConvertToDouble(lblICWaterAvailLK.Text);
                BalanceObj.TotalAvailability = ConvertToDouble(lblICWaterAvailTotal.Text);
                BalanceObj.KPKBalochistanEK = ConvertToDouble(lblKPKBalEK.Text);
                BalanceObj.KPKBalochistanLK = ConvertToDouble(lblKPKBalLK.Text);
                BalanceObj.KPKBalochistan = ConvertToDouble(lblKPKBalTotal.Text);
                BalanceObj.BalPunSindhEK = ConvertToDouble(lblPunjSindhEK.Text);
                BalanceObj.BalPunsindhLK = ConvertToDouble(lblPunjSindhLK.Text);
                BalanceObj.BalPunSindh = ConvertToDouble(lblPunjSindhTotal.Text);
                BalanceObj.Para2EK = ConvertToDouble(lblICParaEK.Text);
                BalanceObj.Para2LK = ConvertToDouble(lblICParaLK.Text);
                BalanceObj.Para2 = ConvertToDouble(lblICParaTotal.Text);
                BalanceObj.CanalAvailabilityEK = ConvertToDouble(txtICCanalAvailEK.Text);
                BalanceObj.CanalAvailabilityLK = ConvertToDouble(txtICCanalAvailLK.Text);
                BalanceObj.CanalAvailability = ConvertToDouble(lblICCanalAvailTotal.Text);
                BalanceObj.JCOutflowEK = ConvertToDouble(lblICOutflowEK.Text);
                BalanceObj.JCoutflowLK = ConvertToDouble(lblICOutflowLK.Text);
                BalanceObj.JCOutflow = ConvertToDouble(lblICOutflowTotal.Text);
                BalanceObj.KotriEK = ConvertToDouble(lblKotriEK.Text);
                BalanceObj.KotriLK = ConvertToDouble(lblKotriLK.Text);
                BalanceObj.Kotri = ConvertToDouble(lblKotriTotal.Text);
                BalanceObj.ShortageEK = ConvertToDouble(lblICShortageEK.Text);
                BalanceObj.ShortageLK = ConvertToDouble(lblICShortageLK.Text);
                BalanceObj.Shortage = ConvertToDouble(lblICShortageTotal.Text);
                BalanceObj.CreatedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                BalanceObj.CreatedDate = DateTime.Now;
                BalanceObj.ModifiedBy = Convert.ToInt32(Session[SessionValues.UserID]);
                BalanceObj.ModifiedDate = DateTime.Now;
                Result = new SeasonalPlanningBLL().SaveSeasonalPlanningBalance(BalanceObj);

                if (sender != null)
                {
                    if (!Result)
                    {
                        new SeasonalPlanningBLL().DeleteSeasonalIncompleteDraft(SPDraftID, (int)Constants.RimStationsIDs.JhelumATMangla, ObjScenario.Scenario);
                        new SeasonalPlanningBLL().DeleteSeasonalIncompleteDraft(SPDraftID, (int)Constants.RimStationsIDs.IndusAtTarbela, ObjScenario.Scenario);
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                    else
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        protected void gvView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long ForecstDraftID = Convert.ToInt64(e.CommandArgument);
                ViewForecastDraftScreen(ForecstDraftID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void ViewForecastDraftScreen(long _ForecastDraftID)
        {
            try
            {
                List<SP_ForecastScenario> lstProbabilities = new SeasonalPlanningBLL().GetSavedProbabilities(_ForecastDraftID);

                gvMax.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(_ForecastDraftID, "Maximum");
                gvMax.DataBind();
                SetHeaderForProbabilities(lstProbabilities, gvMax, "Maximum");

                gvMin.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(_ForecastDraftID, "Minimum");
                gvMin.DataBind();
                SetHeaderForProbabilities(lstProbabilities, gvMin, "Minimum");

                gvLikely.DataSource = new SeasonalPlanningBLL().GetStatDraftDetail(_ForecastDraftID, "Likely");
                gvLikely.DataBind();
                SetHeaderForProbabilities(lstProbabilities, gvLikely, "Likely");

                divforecast.Visible = true;
                divForecastDrafts.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetHeaderForProbabilities(List<SP_ForecastScenario> _lstProbabilities, GridView _gv, string _Scenario)
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

                if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                {
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
                else
                {
                    JMEK.Text = "Rabi %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                                && q.Scenario == _Scenario).FirstOrDefault().RabiPercent);

                    CMEK.Text = "Rabi %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala
                                                && q.Scenario == _Scenario).FirstOrDefault().RabiPercent);

                    ITEK.Text = "Rabi %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela
                                                && q.Scenario == _Scenario).FirstOrDefault().RabiPercent);

                    KNEK.Text = "Rabi %:" + Convert.ToString(_lstProbabilities.Where(q => q.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera
                                                && q.Scenario == _Scenario).FirstOrDefault().RabiPercent);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnForecastOK_Click(object sender, EventArgs e)
        {
            try
            {
                divforecast.Visible = false;
                divForecastDrafts.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnRabiBack_Click(object sender, EventArgs e)
        {
            try
            {
                htitle.InnerText = "Seasonal Planning";
                BindSeasonalPlanningDrafts();
                divKharif.Visible = false;
                divRabi.Visible = false;
                divPanel.Visible = false;
                divSPDrafts.Visible = true;
                gvSPDraftsView.Visible = true;
                //  btnAddNew.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SaveRabifunction()
        {
            try
            {

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
                    if (lbl != null && (lbl.Text == "EK(MAF)" || lbl.Text == "LK(MAF)" || lbl.Text == "Total(MAF)" || lbl.Text == "Rabi(MAF)"))
                        e.Row.Font.Bold = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlEastern_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                {
                    object Eastern = new SeasonalPlanningBLL().GetEastern((int)Constants.Seasons.Kharif, Convert.ToInt64(ddlKharifEastern.SelectedItem.Value));
                    if (Eastern != null)
                    {
                        string[] str = lblScenarioName.Text.Split(' ');

                        if (str[0] == "Minimum")
                        {
                            txtEasternEK.Text = ConvertToThreeDecimalPlaces((ConvertToDouble(Utility.GetDynamicPropertyValue(Eastern, "EK"))) / 2);
                            txtEasternLK.Text = ConvertToThreeDecimalPlaces((ConvertToDouble(Utility.GetDynamicPropertyValue(Eastern, "LK"))) / 2);
                        }
                        else
                        {
                            txtEasternEK.Text = ConvertToThreeDecimalPlaces(Utility.GetDynamicPropertyValue(Eastern, "EK"));
                            txtEasternLK.Text = ConvertToThreeDecimalPlaces(Utility.GetDynamicPropertyValue(Eastern, "LK"));
                        }

                        lblEasternTotal.Text = ConvertToThreeDecimalPlaces(ConvertToDouble(txtEasternEK.Text) + ConvertToDouble(txtEasternLK.Text));

                        EditEasternRegionForKharif();
                        EditReservirLevelRegionForKharifJhelum();
                        EditSystemInflowsRegionForKharifJhelum();
                        EditCanalAvailabiltyForKharifJhelum();
                        EditViewKharifTrue();
                    }
                }
                else
                {
                    string[] str = lblRabiScenarioName.Text.Split(' ');

                    if (str[0] == "Minimum")
                        txtEasternRabi.Text = ConvertToThreeDecimalPlaces(((ConvertToDouble(new SeasonalPlanningBLL().GetEasternRabi((int)Constants.Seasons.Rabi, Convert.ToInt64(ddlRabiEastern.SelectedItem.Value)))) / 2));
                    else
                        txtEasternRabi.Text = new SeasonalPlanningBLL().GetEasternRabi((int)Constants.Seasons.Rabi, Convert.ToInt64(ddlRabiEastern.SelectedItem.Value));

                    EditEasternRegionForRabiMangla();
                    EditReservirLevelRegionForRabiMangla();
                    EditSystemInflowsRegionForRabiMangla();
                    EditCanalAvailabilityforJhelum();
                    EditViewTrue();
                    txtJCInitialStorageRabi.Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}