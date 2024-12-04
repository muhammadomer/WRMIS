using PMIU.WRMIS.DAL.DataAccess.SeasonalPlanning;
using PMIU.WRMIS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.BLL.SeasonalPlanning
{
    public class SeasonalPlanningBLL : BaseBLL
    {
        SeasonalPlanningDAL DALSeasonalPlanning = new SeasonalPlanningDAL();

        #region Filling Fraction

        public List<object> GetFillingFractionDetail(long _SeasonID, long _RimStationID)
        {
            return DALSeasonalPlanning.GetFillingFractionDetail(_SeasonID, _RimStationID);
        }

        public bool UpdateFillingFraction(long? _ID, Decimal? _MaxPercentage, Decimal? _MinPercentage, Decimal? _LikelyPercentage, long? _UserID)
        {
            return DALSeasonalPlanning.UpdateFillingFraction(_ID, _MaxPercentage, _MinPercentage, _LikelyPercentage, _UserID);
        }

        public List<object> GetFillingFractionHistory(long _TDailyID)
        {
            return DALSeasonalPlanning.GetFillingFractionHistory(_TDailyID);
        }

        #endregion

        #region Share Distribution

        public List<object> GetShareDistribution(long _SeasonID)
        {
            return DALSeasonalPlanning.GetShareDistribution(_SeasonID);
        }

        //public bool UpdateShareDistribution(SP_RefShareDistribution _ObjSave)
        //{
        //    return DALSeasonalPlanning.UpdateShareDistribution(_ObjSave);
        //}

        public List<object> GetShareDistributionHistory(long _TDailyCalID)
        {
            return DALSeasonalPlanning.GetShareDistributionHistory(_TDailyCalID);
        }

        public bool UpdateShareDistribution(long? _ID, decimal? _Balochistan, decimal? _KPK, decimal? _HistPunjab, decimal? _HistSindh, decimal? _ParaPunjab, decimal? _ParaSindh, long? _UserID)
        {
            return DALSeasonalPlanning.UpdateShareDistribution(_ID, _Balochistan, _KPK, _HistPunjab, _HistSindh, _ParaPunjab, _ParaSindh, _UserID);
        }

        #endregion

        #region Flow7782

        public List<object> GetFlows(long _SeasonID)
        {
            return DALSeasonalPlanning.GetFlows(_SeasonID);
        }
        public List<object> GetFlow7782History(long _TDailyCalID)
        {
            return DALSeasonalPlanning.GetFlow7782History(_TDailyCalID);
        }
        public bool UpdateFlow7782(long? _RecordID, decimal? _JC, long? _UserID)
        {
            return DALSeasonalPlanning.UpdateFlow7782(_RecordID, _JC, _UserID);
        }

        #endregion

        #region Para 2

        public List<object> GetPara2()
        {
            return DALSeasonalPlanning.GetPara2();
        }

        #endregion

        #region Water Distribution

        public List<object> GetWaterDistribution()
        {
            return DALSeasonalPlanning.GetWaterDistribution();
        }

        #endregion

        #region Eastern Component

        public List<object> GetCalendarYears(long _SeasonID)
        {
            return DALSeasonalPlanning.GetCalendarYears(_SeasonID);
        }

        public List<object> GetTDailyDataForEasternComponent(long _SeasonID, int _Year)
        {
            return DALSeasonalPlanning.GetTDailyDataForEasternComponent(_SeasonID, _Year);
        }

        #endregion

        #region Elevation Capacity

        public List<object> GetSavedDates()
        {
            return DALSeasonalPlanning.GetSavedDates();
        }

        public List<SP_RefElevationCapacity> GetRecordsOfSelectedDate(DateTime _Date, long _RimstationID)
        {
            return DALSeasonalPlanning.GetRecordsOfSelectedDate(_Date, _RimstationID);
        }

        public bool UpDateElevationCapacity(long _RecordID, decimal _Capacity, long _UserID)
        {
            return DALSeasonalPlanning.UpDateElevationCapacity(_RecordID, _Capacity, _UserID);
        }

        public List<object> GetElevationCapacityHistory(long _ID)
        {
            return DALSeasonalPlanning.GetElevationCapacityHistory(_ID);
        }

        public bool AddBulkElevationCapacity(List<SP_RefElevationCapacity> _lstEC)
        {
            return DALSeasonalPlanning.AddBulkElevationCapacity(_lstEC);
        }

        #endregion

        #region Probability

        public List<object> GetProbabilityYears(int _SeasonID)
        {
            return DALSeasonalPlanning.GetProbabilityYears(_SeasonID);
        }

        public List<object> ViewProbability(int _RimStationID, int _SeasonID, string _Year)
        {
            return DALSeasonalPlanning.ViewProbability(_RimStationID, _SeasonID, _Year);
        }

        #endregion

        #region Statistical Inflow Forecasting

        public object GetPrevoidSeasonMAF(int _SeasonID)
        {
            return DALSeasonalPlanning.GetPrevoidSeasonMAF(_SeasonID);
        }

        public List<object> GetMatchingYears(long? _StationID, decimal _StartVariation, decimal _EndVariation)
        {
            return DALSeasonalPlanning.GetMatchingYears(_StationID, _StartVariation, _EndVariation);
        }

        public int? ForecastProbability(decimal? _MAF, int? _StationID, int? _SeasonID, int? TDAilyID)
        {
            return DALSeasonalPlanning.ForecastProbability(_MAF, _StationID, _SeasonID, TDAilyID);
        }

        public List<object> GetForecastedValues(int _SeasonID, int? _JM, int? _JMLK, int? _CM, int? _CMLK, int? _IT, int? _ITLK, int? _KN, int? _KNLK, bool _CalculateMAF)
        {
            return DALSeasonalPlanning.GetForecastedValues(_SeasonID, _JM, _JMLK, _CM, _CMLK, _IT, _ITLK, _KN, _KNLK, _CalculateMAF);
        }

        public bool SaveForecastedValues(int _SeasonID, int? _JM, int? _JMLK, int? _CM, int? _CMLK, int? _IT, int? _ITLK, int? _KN, int? _KNLK,
                                        long _JMScenarioID, long _CMScenarioID, long _ITScenarioID, long _KNScenarioID, int _UserID)
        {
            return DALSeasonalPlanning.SaveForecastedValues(_SeasonID, _JM, _JMLK, _CM, _CMLK, _IT, _ITLK, _KN, _KNLK, _JMScenarioID, _CMScenarioID, _ITScenarioID, _KNScenarioID, _UserID);
        }

        public long SaveStatisticalDraftBasicInfo(SP_ForecastDraft _ObjSave)
        {
            return DALSeasonalPlanning.SaveStatisticalDraftBasicInfo(_ObjSave);
        }

        public long SaveStatisticalDraftScenarios(SP_ForecastScenario _ObjSave)
        {
            return DALSeasonalPlanning.SaveStatisticalDraftScenarios(_ObjSave);
        }

        public List<SP_ForecastDraft> GetDraftsInformation()
        {
            return DALSeasonalPlanning.GetDraftsInformation();
        }

        public bool DeleteDraft(long _DraftID)
        {
            return DALSeasonalPlanning.DeleteDraft(_DraftID);
        }

        public string GetDraftCountName(int _Season)
        {
            return DALSeasonalPlanning.GetDraftCountName(_Season);
        }

        public List<object> GetStatDraftDetail(long _RecordID, string _Scenario)
        {
            return DALSeasonalPlanning.GetStatDraftDetail(_RecordID, _Scenario);
        }

        #endregion

        #region SRM InflowForecasting

        public List<SP_ForecastDraft> GetSRMDrafts()
        {
            return DALSeasonalPlanning.GetSRMDrafts();
        }

        public List<object> GetSRMDetail(long _RecordID, string _Scenarion)
        {
            return DALSeasonalPlanning.GetSRMDetail(_RecordID, _Scenarion);
        }

        public bool AllowedToAddMoreDrafts()
        {
            return DALSeasonalPlanning.AllowedToAddMoreDrafts();
        }

        public List<SP_ForecastScenario> GetSavedProbabilities(long _DraftID)
        {
            return DALSeasonalPlanning.GetSavedProbabilities(_DraftID);
        }

        public void DeletePreviousData(long _DraftID)
        {
            DALSeasonalPlanning.DeletePreviousData(_DraftID);
        }

        //public bool SaveSRMForecastedValues(List<SP_GetForecastedValues_Result> _lstforecastedValues, long _JMScenarioID, long _CMScenarioID, long _ITScenarioID, long _KNScenarioID, int _UserID)
        //{
        //    return DALSeasonalPlanning.SaveSRMForecastedValues(_lstforecastedValues, _JMScenarioID, _CMScenarioID, _ITScenarioID, _KNScenarioID, _UserID);
        //}

        #endregion

        #region Selected forecast

        public string GetSRMDraftName()
        {
            return DALSeasonalPlanning.GetSRMDraftName();
        }

        public List<SP_ForecastScenario> GetStatisticalDraftProbabilities(long _StatisticalID, string _Scenario)
        {
            return DALSeasonalPlanning.GetStatisticalDraftProbabilities(_StatisticalID, _Scenario);
        }

        public List<SP_ForecastScenario> GetSRMDraftProbabilities(string _Scenario)
        {
            return DALSeasonalPlanning.GetSRMDraftProbabilities(_Scenario);
        }

        public List<object> GetStatisticalAndSRMDraftDetail(long _StatisticalID, string _Scenario)
        {
            return DALSeasonalPlanning.GetStatisticalAndSRMDraftDetail(_StatisticalID, _Scenario);
        }

        public List<object> GetFinalizedDraft(long? _JMDraftID, long? _CMDraftID, long? _ITDraftID, long? _KNDraftID, string _Scenario)
        {
            return DALSeasonalPlanning.GetFinalizedDraft(_JMDraftID, _CMDraftID, _ITDraftID, _KNDraftID, _Scenario);
        }

        public List<SP_ForecastScenario> GetProbabilitiesForFinalizedDraft(long? _JMDraftID, long? _CMDraftID, long? _ITDraftID, long? _KNDraftID, string _Scenario)
        {
            return DALSeasonalPlanning.GetProbabilitiesForFinalizedDraft(_JMDraftID, _CMDraftID, _ITDraftID, _KNDraftID, _Scenario);
        }

        public List<long> SaveSelectedScenarios(SP_ForecastScenario _JMObjSave, SP_ForecastScenario _CMObjSave, SP_ForecastScenario _ITObjSave, SP_ForecastScenario _KNObjSave,
                                          SP_ForecastScenario _JMObjSaveMin, SP_ForecastScenario _CMObjSaveMin, SP_ForecastScenario _ITObjSaveMin, SP_ForecastScenario _KNObjSaveMin,
                                          SP_ForecastScenario _JMObjSaveLikely, SP_ForecastScenario _CMObjSaveLikely, SP_ForecastScenario _ITObjSaveLikely, SP_ForecastScenario _KNObjSaveLikely)
        {
            return DALSeasonalPlanning.SaveSelectedScenarios(_JMObjSave, _CMObjSave, _ITObjSave, _KNObjSave, _JMObjSaveMin, _CMObjSaveMin, _ITObjSaveMin, _KNObjSaveMin, _JMObjSaveLikely, _CMObjSaveLikely, _ITObjSaveLikely, _KNObjSaveLikely);
        }

        public bool SaveSelectedForecastValues(long? _JM, long? _CM, long? _IT, long? _KN, long _JMScenarioID, long _CMScenarioID, long _ITScenarioID, long _KNScenarioID, int _UserID, string _Scenario)
        {
            return DALSeasonalPlanning.SaveSelectedForecastValues(_JM, _CM, _IT, _KN, _JMScenarioID, _CMScenarioID, _ITScenarioID, _KNScenarioID, _UserID, _Scenario);
        }

        public List<SP_ForecastDraft> GetSelectedDraftsInformation()
        {
            return DALSeasonalPlanning.GetSelectedDraftsInformation();
        }

        public string GetDefaultDraftName(int _Season)
        {
            return DALSeasonalPlanning.GetDefaultDraftName(_Season);
        }

        #endregion

        #region Balance Reservoir

        public object GetElevationCapacitiesForBalanceReservoir(long _ManglaLevel, long _TarbelaLevel, long _TarbelaFillingLimit, long _Chashma, long _ChashmaMinLevel)
        {
            return DALSeasonalPlanning.GetElevationCapacitiesForBalanceReservoir(_ManglaLevel, _TarbelaLevel, _TarbelaFillingLimit, _Chashma, _ChashmaMinLevel);
        }

        public Object GetInflowsforKharifSeason(long _ForecastDraftID, string _Scenario)
        {
            return DALSeasonalPlanning.GetInflowsforKharifSeason(_ForecastDraftID, _Scenario);
        }

        public object GetInitialLevels(double? _JMStorage, double? _ITStorage)
        {
            return DALSeasonalPlanning.GetInitialLevels(_JMStorage, _ITStorage);
        }

        public object GetParaKPKBALKharifMAF()
        {
            return DALSeasonalPlanning.GetParaKPKBALKharifMAF();
        }

        public object GetParaForIndus()
        {
            return DALSeasonalPlanning.GetParaForIndus();
        }

        public Object GetInflowsforRabiSeason(long _ForecastDraftID, string _Scenario)
        {
            return DALSeasonalPlanning.GetInflowsforRabiSeason(_ForecastDraftID, _Scenario);
        }

        public object GetParaKPKBALRabiMAF()
        {
            return DALSeasonalPlanning.GetParaKPKBALRabiMAF();
        }

        public long SaveDraft(SP_PlanDraft _ObjSave)
        {
            return DALSeasonalPlanning.SaveSeasonalPlanningDraft(_ObjSave);
        }

        public string GetSeasonalDraftCountName(int _Season)
        {
            return DALSeasonalPlanning.GetSeasonalDraftCountName(_Season);
        }

        public List<object> GetSeasonalDrafts()
        {
            return DALSeasonalPlanning.GetSeasonalDrafts();
        }

        public long SaveSeasonalPlanningScenario(SP_PlanScenario _ObjSave, bool _CheckData)
        {
            return DALSeasonalPlanning.SaveSeasonalPlanningScenario(_ObjSave, _CheckData);
        }

        public bool SaveSeasonalPlanningBalance(SP_PlanBalance _ObjSave)
        {
            return DALSeasonalPlanning.SaveSeasonalPlanningBalance(_ObjSave);
        }

        public bool DeleteSeasonalIncompleteDraft(long _DraftID, long? _StationID, string _Scenario)
        {
            return DALSeasonalPlanning.DeleteSeasonalIncompleteDraft(_DraftID, _StationID, _Scenario);
        }

        public object GetEastern(long _SeasonID, long _Years)
        {
            return DALSeasonalPlanning.GetEastern(_SeasonID, _Years);
        }

        public string GetEasternRabi(long _SeasonID, long _Years)
        {
            return DALSeasonalPlanning.GetEasternRabi(_SeasonID, _Years);
        }

        public SP_PlanBalance GetExistingBalanceData(long _SeasonalDraftID, long _StationID, string _Scenario)
        {
            return DALSeasonalPlanning.GetExistingBalanceData(_SeasonalDraftID, _StationID, _Scenario);
        }

        public List<SP_ForecastScenario> GetForecastPercentages(long _ForecastDraftID, String _Scenario)
        {
            return DALSeasonalPlanning.GetForecastPercentages(_ForecastDraftID, _Scenario);
        }

        public SP_PlanBalance GetLikelyBalanceJhelum(long _SPDratfID, long _IFDraftID, long _SeasonID)
        {
            return DALSeasonalPlanning.GetLikelyBalanceJhelum(_SPDratfID, _IFDraftID, _SeasonID);
        }

        public SP_PlanBalance GetLikelyBalanceIndus(long _SPDratfID, long _IFDraftID, long _SeasonID)
        {
            return DALSeasonalPlanning.GetLikelyBalanceIndus(_SPDratfID, _IFDraftID, _SeasonID);
        }

        public long ApproveDraft(long _SeasonalDraftID)
        {
            return DALSeasonalPlanning.ApproveDraft(_SeasonalDraftID);
        }

        public double? GetReservoirLevel(long _StationID, double? _Capacity)
        {
            return DALSeasonalPlanning.GetReservoirLevel(_StationID, _Capacity);
        }

        #endregion

        #region Plan Jhelum Chenab

        public List<object> CalculateJhelumchenabPlan(long _ForecastDraftID, long _SeasonalDraftID, string _Scenario, bool _CalculateMAF)
        {
            return DALSeasonalPlanning.CalculateJhelumchenabPlan(_ForecastDraftID, _SeasonalDraftID, _Scenario, _CalculateMAF);
        }

        public bool SavePlanBulk(List<object> _lstData, long _UserID, long _SPDraftID, long _StationID, string _Scenario)
        {
            return DALSeasonalPlanning.SavePlanBulk(_lstData, _UserID, _SPDraftID, _StationID, _Scenario);
        }

        public List<object> GetLikelyPlanData(long _SPDraftID, bool _CalculateMAF, bool _FromSave)
        {
            return DALSeasonalPlanning.GetLikelyPlanData(_SPDraftID, _CalculateMAF, _FromSave);
        }

        public double GetStorageAgainstReservoirLevel(long _StationID, double? _ResLevel)
        {
            return DALSeasonalPlanning.GetStorageAgainstReservoirLevel(_StationID, _ResLevel);
        }

        #endregion

        #region Plan Indus

        public List<object> CalculateIndusPlan(long _ForecastDraftID, long _SeasonalDraftID, string _Scenario, bool _CalculateMAF)
        {
            return DALSeasonalPlanning.CalculateIndusPlan(_ForecastDraftID, _SeasonalDraftID, _Scenario, _CalculateMAF);
        }

        public List<object> GetSavedPlan(long _SPDraftID, long _StationID, string _Scenario)
        {
            return DALSeasonalPlanning.GetSavedPlan(_SPDraftID, _StationID, _Scenario);
        }

        public List<object> GetLikelyPlanDataIndus(long _SPDraftID, bool _CalculateMAF, bool _FromSave)
        {
            return DALSeasonalPlanning.GetLikelyPlanDataIndus(_SPDraftID, _CalculateMAF, _FromSave);
        }

        #endregion

        #region AnticipatedJC

        public object AnticipatedData(long _ForecastDraftID, long _PlanDraftID, long _StationID, string _Scenario)
        {

            return DALSeasonalPlanning.AnticipatedData(_ForecastDraftID, _PlanDraftID, _StationID, _Scenario);
        }
        public object AnticipatedJCData(long _DraftID, long _StationIDJC, string _Scenario)
        {
            return DALSeasonalPlanning.AnticipatedJCData(_DraftID, _StationIDJC, _Scenario);
        }
        public object AnticipatedIKData(long _DraftID, long _StationIDIK, string _Scenario)
        {
            return DALSeasonalPlanning.AnticipatedIKData(_DraftID, _StationIDIK, _Scenario);
        }
        public object JCFlowData(long _SeasonID)
        {
            return DALSeasonalPlanning.JCFlowData(_SeasonID);
        }
        public object AnticipatedRabiData(long _ForecastDraftID, string _Scenario, long _StationID, long _PlanDraftID)
        {
            return DALSeasonalPlanning.AnticipatedRabiData(_ForecastDraftID, _Scenario, _StationID, _PlanDraftID);
        }
        public object ERComponents(long _DraftID, long _StationID, string _Scenario)
        {
            return DALSeasonalPlanning.ERComponents(_DraftID, _StationID, _Scenario);
        }

        public bool SaveParahistoricBit(long _PlanDraftID, bool _Para)
        {
            return DALSeasonalPlanning.SaveParahistoricBit(_PlanDraftID, _Para);
        }

        public bool? GetParaHistoricBit(long _PlanDraftID)
        {
            return DALSeasonalPlanning.GetParaHistoricBit(_PlanDraftID);
        }

        #endregion

        public List<object> GetSeasonalPlanningYearBySessionID(int SeasonID)
        {
            return DALSeasonalPlanning.GetSeasonalPlanningYearBySessionID(SeasonID);
        }

        public List<object> GetSeasonalPlanningYearForRiverFlowBySessionID(int SeasonID)
        {
            return DALSeasonalPlanning.GetSeasonalPlanningYearForRiverFlowBySessionID(SeasonID);
        }
    }
}
