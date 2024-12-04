using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.SeasonalPlanning;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.SeasonalPlanning
{
    public class SeasonalPlanningDAL
    {
        ContextDB db = new ContextDB();

        #region Filling Fraction

        public List<object> GetFillingFractionDetail(long _SeasonID, long _RimStationID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetFillingFractionDetail(_SeasonID, _RimStationID);
        }

        public bool UpdateFillingFraction(long? _ID, Decimal? _MaxPercentage, Decimal? _MinPercentage, Decimal? _LikelyPercentage, long? _UserID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().UpdateFillingFraction(_ID, _MaxPercentage, _MinPercentage, _LikelyPercentage, _UserID);
        }

        public List<object> GetFillingFractionHistory(long _TDailyID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetFillingFractionHistory(_TDailyID);
        }

        #endregion

        #region Share Distribution

        public List<object> GetShareDistribution(long _SeasonID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetShareDistribution(_SeasonID);
        }

        public bool UpdateShareDistribution(SP_RefShareDistribution _ObjSave)
        {
            bool Result = false;
            try
            {
                SP_RefShareDistribution objShareDist = db.Repository<SP_RefShareDistribution>().FindById(_ObjSave.ID);
                if (objShareDist != null)
                {
                    objShareDist.BalochistanShare = _ObjSave.BalochistanShare;
                    objShareDist.KPShare = _ObjSave.KPShare;
                    objShareDist.PunjabHistoric = _ObjSave.PunjabHistoric;
                    objShareDist.PunjabPara2 = _ObjSave.PunjabPara2;
                    objShareDist.SindhPara2 = _ObjSave.SindhPara2;
                    objShareDist.SindhHistoric = _ObjSave.SindhHistoric;
                    objShareDist.ModifiedBy = _ObjSave.ModifiedBy;
                    objShareDist.ModifiedDate = DateTime.Now;

                    db.Repository<SP_RefShareDistribution>().Update(objShareDist);
                    db.Save();
                    Result = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public List<object> GetShareDistributionHistory(long _TDailyCalID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetShareDistributionHistory(_TDailyCalID);
        }

        public bool UpdateShareDistribution(long? _ID, decimal? _Balochistan, decimal? _KPK, decimal? _HistPunjab, decimal? _HistSindh, decimal? _ParaPunjab, decimal? _ParaSindh, long? _UserID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().UpdateShareDistribution(_ID, _Balochistan, _KPK, _HistPunjab, _HistSindh, _ParaPunjab, _ParaSindh, _UserID);
        }


        #endregion

        #region Flow7782

        public List<object> GetFlows(long _SeasonID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetFlows(_SeasonID);
        }

        public List<object> GetFlow7782History(long _TDailyCalID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetFlow7782History(_TDailyCalID);
        }

        public bool UpdateFlow7782(long? _RecordID, decimal? _JC, long? _UserID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().UpdateFlow7782(_RecordID, _JC, _UserID);
        }


        #endregion

        #region Para 2

        public List<object> GetPara2()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetPara2();
        }

        #endregion

        #region Water Distribution

        public List<object> GetWaterDistribution()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetWaterDistribution();
        }

        #endregion

        #region Eastern River Component

        public List<object> GetCalendarYears(long _SeasonID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetCalendarYears(_SeasonID);
        }

        public List<object> GetTDailyDataForEasternComponent(long _SeasonID, int _Year)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetTDailyDataForEasternComponent(_SeasonID, _Year);
        }

        #endregion

        #region Elevation Capacity

        public List<object> GetSavedDates()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSavedDates();
        }

        public List<SP_RefElevationCapacity> GetRecordsOfSelectedDate(DateTime _Date, long _RimstationID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetRecordsOfSelectedDate(_Date, _RimstationID);
        }

        public bool UpDateElevationCapacity(long _RecordID, decimal _Capacity, long _UserID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().UpDateElevationCapacity(_RecordID, _Capacity, _UserID);
        }

        public List<object> GetElevationCapacityHistory(long _ID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetElevationCapacityHistory(_ID);
        }

        public bool AddBulkElevationCapacity(List<SP_RefElevationCapacity> _lstEC)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().AddBulkElevationCapacity(_lstEC);
        }

        #endregion

        #region Probability

        public List<object> GetProbabilityYears(int _SeasonID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetProbabilityYears(_SeasonID);
        }

        public List<object> ViewProbability(int _RimStationID, int _SeasonID, string _Year)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().ViewProbability(_RimStationID, _SeasonID, _Year);
        }

        #endregion

        #region Statistical Inflow forecasting

        public object GetPrevoidSeasonMAF(int _SeasonID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetPrevoidSeasonMAF(_SeasonID);
        }

        public List<object> GetMatchingYears(long? _StationID, decimal _StartVariation, decimal _EndVariation)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetMatchingYears(_StationID, _StartVariation, _EndVariation);
        }

        public int? ForecastProbability(decimal? _MAF, int? _StationID, int? _SeasonID, int? TDAilyID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().ForecastProbability(_MAF, _StationID, _SeasonID, TDAilyID);
        }

        public List<object> GetForecastedValues(int _SeasonID, int? _JM, int? _JMLK, int? _CM, int? _CMLK, int? _IT, int? _ITLK, int? _KN, int? _KNLK, bool _CalculateMAF)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetFinalForecastedValues(_SeasonID, _JM, _JMLK, _CM, _CMLK, _IT, _ITLK, _KN, _KNLK, _CalculateMAF);
        }

        public bool SaveForecastedValues(int _SeasonID, int? _JM, int? _JMLK, int? _CM, int? _CMLK, int? _IT, int? _ITLK, int? _KN, int? _KNLK,
                                        long _JMScenarioID, long _CMScenarioID, long _ITScenarioID, long _KNScenarioID, int _UserID)
        {
            try
            {
                List<SP_GetForecastedValues_Result> lstResult = new List<SP_GetForecastedValues_Result>();
                SP_ForecastData ObjSave;
                lstResult = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetForecastedValues(_SeasonID, _JM, _JMLK, _CM, _CMLK, _IT, _ITLK, _KN, _KNLK, false);
                foreach (var res in lstResult)
                {
                    ObjSave = new SP_ForecastData();
                    ObjSave.ForecastScenarioID = _JMScenarioID;
                    ObjSave.TDailyID = res.TDailyID;
                    ObjSave.Volume = res.JhelumMangla;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = _UserID;
                    db.Repository<SP_ForecastData>().Insert(ObjSave);

                    ObjSave = new SP_ForecastData();
                    ObjSave.ForecastScenarioID = _CMScenarioID;
                    ObjSave.TDailyID = res.TDailyID;
                    ObjSave.Volume = res.ChenabMarala;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = _UserID;
                    db.Repository<SP_ForecastData>().Insert(ObjSave);

                    ObjSave = new SP_ForecastData();
                    ObjSave.ForecastScenarioID = _ITScenarioID;
                    ObjSave.TDailyID = res.TDailyID;
                    ObjSave.Volume = res.IndusTarbela;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = _UserID;
                    db.Repository<SP_ForecastData>().Insert(ObjSave);

                    ObjSave = new SP_ForecastData();
                    ObjSave.ForecastScenarioID = _KNScenarioID;
                    ObjSave.TDailyID = res.TDailyID;
                    ObjSave.Volume = res.KabulNowshera;
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = _UserID;
                    db.Repository<SP_ForecastData>().Insert(ObjSave);
                }
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public long SaveStatisticalDraftBasicInfo(SP_ForecastDraft _ObjSave)
        {
            long RecordID = -1;
            try
            {
                db.Repository<SP_ForecastDraft>().Insert(_ObjSave);
                db.Save();
                RecordID = _ObjSave.ID;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return RecordID;
            }
            return RecordID;

        }

        public long SaveStatisticalDraftScenarios(SP_ForecastScenario _ObjSave)
        {
            long RecordID = -1;
            try
            {
                db.Repository<SP_ForecastScenario>().Insert(_ObjSave);
                db.Save();
                RecordID = _ObjSave.ID;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return RecordID;
            }
            return RecordID;

        }

        public List<SP_ForecastDraft> GetDraftsInformation()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetDraftsInformation();
        }

        public bool DeleteDraft(long _DraftID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().DeleteDraft(_DraftID);
        }

        public string GetDraftCountName(int _Season)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetDraftCountName(_Season);
        }

        public List<object> GetStatDraftDetail(long _RecordID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetStatDraftDetail(_RecordID, _Scenario);
        }


        #endregion

        #region SRM Inflow Forecasting

        public List<SP_ForecastDraft> GetSRMDrafts()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDrafts();
        }

        public List<object> GetSRMDetail(long _RecordID, string _Scenarion)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDetail(_RecordID, _Scenarion);
        }

        public bool AllowedToAddMoreDrafts()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().AllowedToAddMoreDrafts();
        }

        public List<SP_ForecastScenario> GetSavedProbabilities(long _DraftID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSavedProbabilities(_DraftID);
        }

        public void DeletePreviousData(long _DraftID)
        {
            db.ExtRepositoryFor<SeasonalPlanningRepository>().DeletePreviousData(_DraftID);
        }

        //public bool SaveSRMForecastedValues(int _SeasonID, int? _JM, int? _JMLK, int? _CM, int? _CMLK, int? _IT, int? _ITLK, int? _KN, int? _KNLK,
        //                                    long _JMScenarioID, long _CMScenarioID, long _ITScenarioID, long _KNScenarioID, int _UserID)
        //{
        //    try
        //    {   
        //        SP_ForecastData ObjSave;                
        //        foreach (var res in _lstforecastedValues)
        //        {
        //            ObjSave = new SP_ForecastData();
        //            ObjSave.ForecastScenarioID = _JMScenarioID;
        //            ObjSave.TDailyID = res.TDailyID;
        //            ObjSave.Volume = res.JhelumMangla;
        //            ObjSave.CreatedDate = DateTime.Now;
        //            ObjSave.CreatedBy = _UserID;
        //            db.Repository<SP_ForecastData>().Insert(ObjSave);
        //            db.Save();

        //            ObjSave = new SP_ForecastData();
        //            ObjSave.ForecastScenarioID = _CMScenarioID;
        //            ObjSave.TDailyID = res.TDailyID;
        //            ObjSave.Volume = res.ChenabMarala;
        //            ObjSave.CreatedDate = DateTime.Now;
        //            ObjSave.CreatedBy = _UserID;
        //            db.Repository<SP_ForecastData>().Insert(ObjSave);
        //            db.Save();

        //            ObjSave = new SP_ForecastData();
        //            ObjSave.ForecastScenarioID = _ITScenarioID;
        //            ObjSave.TDailyID = res.TDailyID;
        //            ObjSave.Volume = res.IndusTarbela;
        //            ObjSave.CreatedDate = DateTime.Now;
        //            ObjSave.CreatedBy = _UserID;
        //            db.Repository<SP_ForecastData>().Insert(ObjSave);
        //            db.Save();

        //            ObjSave = new SP_ForecastData();
        //            ObjSave.ForecastScenarioID = _KNScenarioID;
        //            ObjSave.TDailyID = res.TDailyID;
        //            ObjSave.Volume = res.KabulNowshera;
        //            ObjSave.CreatedDate = DateTime.Now;
        //            ObjSave.CreatedBy = _UserID;
        //            db.Repository<SP_ForecastData>().Insert(ObjSave);
        //            db.Save();
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //        return false;
        //    }
        //    return true;
        //}


        #endregion

        #region Selected Forecast

        public string GetSRMDraftName()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftName();
        }

        public List<SP_ForecastScenario> GetStatisticalDraftProbabilities(long _StatisticalID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetStatisticalDraftProbabilities(_StatisticalID, _Scenario);
        }

        public List<SP_ForecastScenario> GetSRMDraftProbabilities(string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftProbabilities(_Scenario);
        }

        public List<object> GetStatisticalAndSRMDraftDetail(long _StatisticalID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetStatisticalAndSRMDraftDetail(_StatisticalID, _Scenario);
        }

        public List<object> GetFinalizedDraft(long? _JMDraftID, long? _CMDraftID, long? _ITDraftID, long? _KNDraftID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetFinalizedDraft(_JMDraftID, _CMDraftID, _ITDraftID, _KNDraftID, _Scenario, true);
        }

        public List<SP_ForecastScenario> GetProbabilitiesForFinalizedDraft(long? _JMDraftID, long? _CMDraftID, long? _ITDraftID, long? _KNDraftID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetProbabilitiesForFinalizedDraft(_JMDraftID, _CMDraftID, _ITDraftID, _KNDraftID, _Scenario);
        }

        public List<long> SaveSelectedScenarios(SP_ForecastScenario _JMObjSave, SP_ForecastScenario _CMObjSave, SP_ForecastScenario _ITObjSave, SP_ForecastScenario _KNObjSave,
                                          SP_ForecastScenario _JMObjSaveMin, SP_ForecastScenario _CMObjSaveMin, SP_ForecastScenario _ITObjSaveMin, SP_ForecastScenario _KNObjSaveMin,
                                          SP_ForecastScenario _JMObjSaveLikely, SP_ForecastScenario _CMObjSaveLikely, SP_ForecastScenario _ITObjSaveLikely, SP_ForecastScenario _KNObjSaveLikely)
        {
            List<long> lstScenarioIDs = new List<long>();
            try
            {
                if (_JMObjSave != null)
                {
                    if (_JMObjSave.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_JMObjSave);
                    else
                    {
                        _JMObjSave.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_JMObjSave);
                    }
                }

                if (_CMObjSave != null)
                {
                    if (_CMObjSave.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_CMObjSave);
                    else
                    {
                        _CMObjSave.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_CMObjSave);
                    }
                }

                if (_ITObjSave != null)
                {
                    if (_ITObjSave.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_ITObjSave);
                    else
                    {
                        _ITObjSave.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_ITObjSave);
                    }
                }

                if (_KNObjSave != null)
                {
                    if (_KNObjSave.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_KNObjSave);
                    else
                    {
                        _KNObjSave.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_KNObjSave);
                    }
                }

                if (_JMObjSaveMin != null)
                {
                    if (_JMObjSaveMin.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_JMObjSaveMin);
                    else
                    {
                        _JMObjSaveMin.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_JMObjSaveMin);
                    }
                }

                if (_CMObjSaveMin != null)
                {
                    if (_CMObjSaveMin.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_CMObjSaveMin);
                    else
                    {
                        _CMObjSaveMin.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_CMObjSaveMin);
                    }
                }

                if (_CMObjSaveMin != null)
                {
                    if (_CMObjSaveMin.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_CMObjSaveMin);
                    else
                    {
                        _CMObjSaveMin.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_CMObjSaveMin);
                    }
                }

                if (_ITObjSaveMin != null)
                {
                    if (_ITObjSaveMin.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_ITObjSaveMin);
                    else
                    {
                        _ITObjSaveMin.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_ITObjSaveMin);
                    }
                }

                if (_KNObjSaveMin != null)
                {
                    if (_KNObjSaveMin.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_KNObjSaveMin);
                    else
                    {
                        _KNObjSaveMin.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_KNObjSaveMin);
                    }
                }

                if (_JMObjSaveLikely != null)
                {
                    if (_JMObjSaveLikely.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_JMObjSaveLikely);
                    else
                    {
                        _JMObjSaveLikely.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_JMObjSaveLikely);
                    }
                }

                if (_CMObjSaveLikely != null)
                {
                    if (_CMObjSaveLikely.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_CMObjSaveLikely);
                    else
                    {
                        _CMObjSaveLikely.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_CMObjSaveLikely);
                    }
                }

                if (_ITObjSaveLikely != null)
                {
                    if (_ITObjSaveLikely.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_ITObjSaveLikely);
                    else
                    {
                        _ITObjSaveLikely.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_ITObjSaveLikely);
                    }
                }

                if (_KNObjSaveLikely != null)
                {
                    if (_KNObjSaveLikely.ModelDraftID != null)
                        db.Repository<SP_ForecastScenario>().Insert(_KNObjSaveLikely);
                    else
                    {
                        _KNObjSaveLikely.ModelDraftID = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSRMDraftID();
                        db.Repository<SP_ForecastScenario>().Insert(_KNObjSaveLikely);
                    }
                }
                db.Save();

                lstScenarioIDs.Add(_JMObjSave.ID);
                lstScenarioIDs.Add(_CMObjSave.ID);
                lstScenarioIDs.Add(_ITObjSave.ID);
                lstScenarioIDs.Add(_KNObjSave.ID);
                lstScenarioIDs.Add(_JMObjSaveMin.ID);
                lstScenarioIDs.Add(_CMObjSaveMin.ID);
                lstScenarioIDs.Add(_ITObjSaveMin.ID);
                lstScenarioIDs.Add(_KNObjSaveMin.ID);
                lstScenarioIDs.Add(_JMObjSaveLikely.ID);
                lstScenarioIDs.Add(_CMObjSaveLikely.ID);
                lstScenarioIDs.Add(_ITObjSaveLikely.ID);
                lstScenarioIDs.Add(_KNObjSaveLikely.ID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstScenarioIDs;
        }

        public bool SaveSelectedForecastValues(long? _JM, long? _CM, long? _IT, long? _KN, long _JMScenarioID, long _CMScenarioID, long _ITScenarioID, long _KNScenarioID, int _UserID, string _Scenario)
        {
            try
            {
                List<object> lstResult = new List<object>();
                SP_ForecastData ObjSave;
                lstResult = db.ExtRepositoryFor<SeasonalPlanningRepository>().GetFinalizedDraft(_JM, _CM, _IT, _KN, _Scenario, false);
                foreach (var res in lstResult)
                {
                    ObjSave = new SP_ForecastData();
                    ObjSave.ForecastScenarioID = _JMScenarioID;
                    ObjSave.TDailyID = Convert.ToInt16(res.GetType().GetProperty("TDailyID").GetValue(res));
                    ObjSave.Volume = Convert.ToDouble(res.GetType().GetProperty("JhelumMangla").GetValue(res));
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = _UserID;
                    db.Repository<SP_ForecastData>().Insert(ObjSave);

                    ObjSave = new SP_ForecastData();
                    ObjSave.ForecastScenarioID = _CMScenarioID;
                    ObjSave.TDailyID = Convert.ToInt16(res.GetType().GetProperty("TDailyID").GetValue(res));
                    ObjSave.Volume = Convert.ToDouble(res.GetType().GetProperty("ChenabMarala").GetValue(res));
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = _UserID;
                    db.Repository<SP_ForecastData>().Insert(ObjSave);

                    ObjSave = new SP_ForecastData();
                    ObjSave.ForecastScenarioID = _ITScenarioID;
                    ObjSave.TDailyID = Convert.ToInt16(res.GetType().GetProperty("TDailyID").GetValue(res));
                    ObjSave.Volume = Convert.ToDouble(res.GetType().GetProperty("IndusTarbela").GetValue(res));
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = _UserID;
                    db.Repository<SP_ForecastData>().Insert(ObjSave);

                    ObjSave = new SP_ForecastData();
                    ObjSave.ForecastScenarioID = _KNScenarioID;
                    ObjSave.TDailyID = Convert.ToInt16(res.GetType().GetProperty("TDailyID").GetValue(res));
                    ObjSave.Volume = Convert.ToDouble(res.GetType().GetProperty("KabulNowshera").GetValue(res));
                    ObjSave.CreatedDate = DateTime.Now;
                    ObjSave.CreatedBy = _UserID;
                    db.Repository<SP_ForecastData>().Insert(ObjSave);
                }
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public List<SP_ForecastDraft> GetSelectedDraftsInformation()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSelectedDraftsInformation();
        }

        public string GetDefaultDraftName(int _Season)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetDefaultDraftName(_Season);
        }

        #endregion

        #region Balance Reservoir

        public object GetElevationCapacitiesForBalanceReservoir(long _ManglaLevel, long _TarbelaLevel, long _TarbelaFillingLimit, long _Chashma, long _ChashmaMinLevel)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetElevationCapacitiesForBalanceReservoir(_ManglaLevel, _TarbelaLevel, _TarbelaFillingLimit, _Chashma, _ChashmaMinLevel);
        }

        public Object GetInflowsforKharifSeason(long _ForecastDraftID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetInflowsforKharifSeason(_ForecastDraftID, _Scenario);
        }

        public object GetInitialLevels(double? _JMStorage, double? _ITStorage)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetInitialLevels(_JMStorage, _ITStorage);
        }

        public object GetParaKPKBALKharifMAF()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetParaKPKBALKharifMAF();
        }

        public object GetParaForIndus()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetParaForIndus();
        }

        public Object GetInflowsforRabiSeason(long _ForecastDraftID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetInflowsforRabiSeason(_ForecastDraftID, _Scenario);
        }

        public object GetParaKPKBALRabiMAF()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetParaKPKBALRabiMAF();
        }

        public long SaveSeasonalPlanningDraft(SP_PlanDraft _ObjSave)
        {
            long ID = -1;
            try
            {
                db.Repository<SP_PlanDraft>().Insert(_ObjSave);
                db.Save();
                ID = _ObjSave.ID;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return ID;
            }
            return ID;
        }

        public string GetSeasonalDraftCountName(int _Season)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSeasonalDraftCountName(_Season);
        }

        public List<object> GetSeasonalDrafts()
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSeasonalDrafts();
        }

        public long SaveSeasonalPlanningScenario(SP_PlanScenario _ObjSave, bool _CheckData)
        {
            long ID = -1;
            long DeleteID = _ObjSave.PlanDraftID;
            long? StationID = _ObjSave.StationID;
            string Scenario = _ObjSave.Scenario;
            try
            {
                if (_CheckData)
                {
                    db.ExtRepositoryFor<SeasonalPlanningRepository>().DeleteSeasonalIncompleteDraft(DeleteID, StationID, Scenario);
                    db.ExtRepositoryFor<SeasonalPlanningRepository>().DeleteSeasonalIncompleteDraft(DeleteID, StationID, "Likely"); // delete likely scenario every time we save Max and Min Scenario so that average of max and min would always bhe updated when any scenarion is updated
                }
                db.Repository<SP_PlanScenario>().Insert(_ObjSave);
                db.Save();
                ID = _ObjSave.ID;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return ID;
            }
            return ID;
        }

        public bool SaveSeasonalPlanningBalance(SP_PlanBalance _ObjSave)
        {
            try
            {
                db.Repository<SP_PlanBalance>().Insert(_ObjSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool DeleteSeasonalIncompleteDraft(long _DraftID, long? _StationID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().DeleteSeasonalIncompleteDraft(_DraftID, _StationID, _Scenario);
        }

        public object GetEastern(long _SeasonID, long _Years)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetEastern(_SeasonID, _Years);
        }

        public string GetEasternRabi(long _SeasonID, long _Years)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetEasternRabi(_SeasonID, _Years);
        }

        public SP_PlanBalance GetExistingBalanceData(long _SeasonalDraftID, long _StationID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetExistingBalanceData(_SeasonalDraftID, _StationID, _Scenario);
        }

        public List<SP_ForecastScenario> GetForecastPercentages(long _ForecastDraftID, String _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetForecastPercentages(_ForecastDraftID, _Scenario);
        }

        public SP_PlanBalance GetLikelyBalanceJhelum(long _SPDratfID, long _IFDraftID, long _SeasonID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetLikelyBalanceJhelum(_SPDratfID, _IFDraftID, _SeasonID);
        }

        public SP_PlanBalance GetLikelyBalanceIndus(long _SPDratfID, long _IFDraftID, long _SeasonID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetLikelyBalanceIndus(_SPDratfID, _IFDraftID, _SeasonID);
        }

        public long ApproveDraft(long _SeasonalDraftID)
        {
            long Result = -1;
            try
            {
                SP_PlanDraft Draft = db.Repository<SP_PlanDraft>().FindById(_SeasonalDraftID);
                if (Draft != null)
                {
                    if ((bool)Draft.IsApproved)
                    {
                        Draft.IsApproved = false;
                        Result = 0;
                    }
                    else
                    {
                        Draft.IsApproved = true;
                        Result = 1;
                    }
                }
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return Result;
            }
            return Result;
        }

        public double? GetReservoirLevel(long _StationID, double? _Capacity)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetReservoirLevel(_StationID, _Capacity);
        }

        #endregion

        #region Plan Jhelum Chenab

        public List<object> CalculateJhelumchenabPlan(long _ForecastDraftID, long _SeasonalDraftID, string _Scenario, bool _CalculateMAF)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().CalculateJhelumchenabPlan(_ForecastDraftID, _SeasonalDraftID, _Scenario, _CalculateMAF);
        }

        public bool SavePlanBulk(List<object> _lstData, long _UserID, long _SPDraftID, long _StationID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().SavePlanBulk(_lstData, _UserID, _SPDraftID, _StationID, _Scenario);
        }

        public List<object> GetLikelyPlanData(long _SPDraftID, bool _CalculateMAF, bool _FromSave)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetLikelyPlanData(_SPDraftID, _CalculateMAF, _FromSave);
        }

        public double GetStorageAgainstReservoirLevel(long _StationID, double? _ResLevel)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetStorageAgainstReservoirLevel(_StationID, _ResLevel);
        }

        #endregion

        #region Plan Indus

        public List<object> CalculateIndusPlan(long _ForecastDraftID, long _SeasonalDraftID, string _Scenario, bool _CalculateMAF)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().CalculateIndusPlan(_ForecastDraftID, _SeasonalDraftID, _Scenario, _CalculateMAF);
        }

        public List<object> GetSavedPlan(long _SPDraftID, long _StationID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetSavedPlan(_SPDraftID, _StationID, _Scenario);
        }

        public List<object> GetLikelyPlanDataIndus(long _SPDraftID, bool _CalculateMAF, bool _FromSave)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetLikelyPlanDataIndus(_SPDraftID, _CalculateMAF, _FromSave);
        }


        #endregion

        #region AnticipatedJC
        public object AnticipatedData(long _ForecastDraftID, long _PlanDraftID, long _StationID, string _Scenario)
        {

            return db.ExtRepositoryFor<SeasonalPlanningRepository>().AnticipatedData(_ForecastDraftID, _PlanDraftID, _StationID, _Scenario);

        }
        public object AnticipatedJCData(long _DraftID, long _StationIDJC, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().AnticipatedJCData(_DraftID, _StationIDJC, _Scenario);
        }

        public object AnticipatedIKData(long _DraftID, long _StationIDIK, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().AnticipatedIKData(_DraftID, _StationIDIK, _Scenario);
        }
        public object JCFlowData(long _SeasonID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().JCFlowData(_SeasonID);

        }
        public object AnticipatedRabiData(long _ForecastDraftID, string _Scenario, long _StationID, long _PlanDraftID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().AnticipatedRabiData(_ForecastDraftID, _Scenario, _StationID, _PlanDraftID);
        }

        public object ERComponents(long _DraftID, long _StationID, string _Scenario)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().ERComponents(_DraftID, _StationID, _Scenario);
        }

        public bool SaveParahistoricBit(long _PlanDraftID, bool _Para)
        {
            try
            {
                SP_PlanDraft objPlan = db.Repository<SP_PlanDraft>().GetAll().Where(q => q.ID == _PlanDraftID).FirstOrDefault();
                objPlan.IsPara2 = _Para;
                db.Repository<SP_PlanDraft>().Update(objPlan);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool? GetParaHistoricBit(long _PlanDraftID)
        {
            return db.ExtRepositoryFor<SeasonalPlanningRepository>().GetParaHistoricBit(_PlanDraftID);
        }

        #endregion


        public List<object> GetSeasonalPlanningYearBySessionID(int SeasonID)
        {
            
            List<SP_PlanDraft> lstData = db.Repository<SP_PlanDraft>().Query().Get().Where(X => X.IsApproved == true).ToList();
            if (lstData == null || lstData.Count <= 0)
                return null;
            List<object> listData = new List<object>();
            if (SeasonID == 1)
            {
                listData = lstData.OrderByDescending(x => x.Year).Select(x => new { ID = x.Year, Name = x.Year + " - " + (x.Year + 1) }).Distinct().ToList<object>();
            }
            else
            {
                listData = lstData.OrderByDescending(x => x.Year).Select(x => new { ID = x.Year, Name = x.Year }).Distinct().ToList<object>();
            }

            return listData;
        }
        public List<object> GetSeasonalPlanningYearForRiverFlowBySessionID(int SeasonID)
        {
            List<SP_RefTDailyCalendar> lstData = db.Repository<SP_RefTDailyCalendar>().Query().Get().Where(X => X.Year >= 1976).ToList();
            if (lstData == null || lstData.Count <= 0)
                return null;
            List<object> listData = new List<object>();
            if (SeasonID == 1)
            {
                listData = lstData.OrderByDescending(x => x.Year).Select(x => new { ID = x.Year, Name = x.Year + " - " + (x.Year + 1) }).Distinct().ToList<object>();
            }
            else
            {
                listData = lstData.OrderByDescending(x => x.Year).Select(x => new { ID = x.Year, Name = x.Year }).Distinct().ToList<object>();
            }

            return listData;
        }
    }
}
