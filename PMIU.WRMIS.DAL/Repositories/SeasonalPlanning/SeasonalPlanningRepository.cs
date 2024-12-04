using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.SeasonalPlanning
{
    class SeasonalPlanningRepository : Repository<SP_RefFillingFraction>
    {

        public SeasonalPlanningRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<SP_RefFillingFraction>();

        }


        #region Filling Fraction

        public List<object> GetFillingFractionDetail(long _SeasonID, long _RimStationID)
        {
            List<object> lstFillingFraction = new List<object>();
            try
            {
                lstFillingFraction = (from FF in context.SP_RefFillingFraction
                                      join Cal in context.SP_RefTDailyCalendar on FF.TDailyID equals Cal.TDailyID
                                      where
                                      FF.SeasonID == _SeasonID && FF.StationID == _RimStationID && Cal.Year == DateTime.Now.Year - 1
                                      orderby Cal.TDailyID ascending
                                      select new
                                      {
                                          ID = FF.ID,
                                          TDaily = Cal.ShortName,
                                          TDailyID = FF.TDailyID,
                                          MaxPercentage = FF.MaxFill,
                                          MinPercentage = FF.MinFill,
                                          LikelyPercentage = FF.LikelyFill
                                      }
                    ).ToList()
                    .Select(q =>
                        new
                        {
                            ID = q.ID,
                            TDaily = q.TDaily,
                            TDailyID = q.TDailyID,
                            MaxPercentage = OneDecimalPlace(q.MaxPercentage),
                            MinPercentage = OneDecimalPlace(q.MinPercentage),
                            LikelyPercentage = OneDecimalPlace(q.LikelyPercentage)
                        }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstFillingFraction;
        }

        public bool UpdateFillingFraction(long? _ID, Decimal? _MaxPercentage, Decimal? _MinPercentage,
            Decimal? _LikelyPercentage, long? _UserID)
        {
            bool Result = false;
            try
            {
                int RowsUpdated = context.SP_UpdateFillingFraction(_ID, _MaxPercentage, _MinPercentage,
                    _LikelyPercentage, _UserID);
                if (RowsUpdated == 2)
                    Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public String OneDecimalPlace(double? _Value)
        {
            return String.Format("{0:0.0}", _Value);
        }





        public List<object> GetFillingFractionHistory(long _TDailyID)
        {
            List<object> lstHistory = new List<object>();
            try
            {
                lstHistory = (from FF in context.SP_RefFillingFraction
                              join FFH in context.SP_RefFillingFractionHistory on FF.TDailyID equals FFH.TDailyID
                              join CAL in context.SP_RefTDailyCalendar on FFH.TDailyID equals CAL.TDailyID
                              join USR in context.UA_Users on (Int64)FFH.ModifiedBy equals USR.ID
                              where
                              FF.SeasonID == FFH.SeasonID && FFH.TDailyID == _TDailyID && FF.StationID == FFH.StationID &&
                              CAL.Year == DateTime.Now.Year - 1
                              orderby FFH.ModifiedDate descending
                              select new
                              {
                                  ID = FFH.ID,
                                  TDaily = CAL.ShortName,
                                  MaxPercentage = FFH.MaxFill,
                                  MinPercentage = FFH.MinFill,
                                  LikelyPercentage = FFH.LikelyFill,
                                  Date = FFH.ModifiedDate,
                                  FirstName = USR.FirstName,
                                  LastName = USR.LastName
                              }
                    )
                    .ToList()
                    .Select(q => new
                    {
                        q.ID,
                        q.TDaily,
                        q.MaxPercentage,
                        q.MinPercentage,
                        q.LikelyPercentage,
                        ModifiedDate = Utility.GetFormattedDate(q.Date),
                        ModifiedBy = q.FirstName + " " + q.LastName
                    }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstHistory;
        }

        #endregion

        #region Share Distribution

        public List<object> GetShareDistribution(long _SeasonID)
        {
            List<object> lstShareDist = new List<object>();
            try
            {
                lstShareDist = (from SD in context.SP_RefShareDistribution
                                join Cal in context.SP_RefTDailyCalendar on SD.TDailyID equals Cal.TDailyID
                                where SD.SeasonID == _SeasonID && Cal.Year == DateTime.Now.Year - 1
                                select new
                                {
                                    ID = SD.ID,
                                    TDaily = Cal.ShortName,
                                    TDailyID = Cal.TDailyID,
                                    Balochistan = SD.BalochistanShare,
                                    KPK = SD.KPShare,
                                    HistPunjab = SD.PunjabHistoric,
                                    HistSindh = SD.SindhHistoric,
                                    ParaPunjab = SD.PunjabPara2,
                                    ParaSindh = SD.SindhPara2
                                }
                ).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstShareDist;
        }

        public List<object> GetShareDistributionHistory(long _TDailyCalID)
        {
            List<object> lstHistory = new List<object>();
            try
            {
                lstHistory = (from SD in context.SP_RefShareDistribution
                              join SDH in context.SP_RefShareDistributionHistory on SD.TDailyID equals SDH.TDailyID
                              join CAL in context.SP_RefTDailyCalendar on SDH.TDailyID equals CAL.TDailyID
                              join USR in context.UA_Users on (Int64)SDH.ModifiedBy equals USR.ID
                              where SDH.TDailyID == _TDailyCalID && SDH.Jc7782 == null && CAL.Year == DateTime.Now.Year - 1
                              orderby SDH.ModifiedDate descending
                              select new
                              {
                                  ID = SDH.ID,
                                  TDaily = CAL.ShortName,
                                  Balochistan = SDH.BalochistanShare,
                                  KPK = SDH.KPShare,
                                  HistPunjab = SDH.PunjabHistoric,
                                  HistSindh = SDH.SindhHistoric,
                                  ParaPunjab = SDH.PunjabPara2,
                                  ParaSindh = SDH.SindhPara2,
                                  Date = SDH.ModifiedDate,
                                  FirstName = USR.FirstName,
                                  LastName = USR.LastName
                              }
                    )
                    .ToList()
                    .Select(q => new
                    {
                        q.ID,
                        q.TDaily,
                        q.Balochistan,
                        q.KPK,
                        q.HistPunjab,
                        q.HistSindh,
                        q.ParaPunjab,
                        q.ParaSindh,
                        ModifiedDate = Utility.GetFormattedDate(q.Date),
                        ModifiedBy = q.FirstName + " " + q.LastName
                    }).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstHistory;
        }

        public bool UpdateShareDistribution(long? _ID, decimal? _Balochistan, decimal? _KPK, decimal? _HistPunjab,
            decimal? _HistSindh, decimal? _ParaPunjab, decimal? _ParaSindh, long? _UserID)
        {
            bool Result = false;
            try
            {
                int UpdateCount = context.SP_UpdateShareDistribution(_ID, _Balochistan, _KPK, _HistPunjab, _HistSindh,
                    _ParaPunjab, _ParaSindh, _UserID);
                if (UpdateCount == 2)
                    Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }


        #endregion

        #region Flow7782

        public List<object> GetFlows(long _SeasonID)
        {
            List<object> lstFlows = new List<object>();
            try
            {
                lstFlows = (from SD in context.SP_RefShareDistribution
                            join Cal in context.SP_RefTDailyCalendar on SD.TDailyID equals Cal.TDailyID
                            where SD.SeasonID == _SeasonID && Cal.Year == DateTime.Now.Year - 1
                            select new
                            {
                                ID = SD.ID,
                                TDailyID = Cal.TDailyID,
                                TDaily = Cal.ShortName,
                                Bal = SD.BalochistanShare,
                                KPK = SD.KPShare,
                                HistPunjab = SD.PunjabHistoric,
                                HistSindh = SD.SindhHistoric,
                                JC = SD.Jc7782
                            }
                    ).ToList()
                    .Select(q => new
                    {
                        q.ID,
                        q.TDailyID,
                        q.TDaily,
                        Indus = q.Bal + q.KPK + q.HistPunjab + q.HistSindh,
                        q.JC
                    }).ToList<object>();


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstFlows;
        }

        public List<object> GetFlow7782History(long _TDailyCalID)
        {
            List<object> lstHistory = new List<object>();
            try
            {
                lstHistory = (from SDH in context.SP_RefShareDistributionHistory
                              join CAL in context.SP_RefTDailyCalendar on SDH.TDailyID equals CAL.TDailyID
                              join USR in context.UA_Users on (Int64)SDH.ModifiedBy equals USR.ID
                              where SDH.TDailyID == _TDailyCalID && SDH.Jc7782 != null && CAL.Year == DateTime.Now.Year - 1
                              orderby SDH.ModifiedDate descending
                              select new
                              {
                                  ID = SDH.ID,
                                  TDaily = CAL.ShortName,
                                  JC = SDH.Jc7782,
                                  Date = SDH.ModifiedDate,
                                  FirstName = USR.FirstName,
                                  LastName = USR.LastName
                              }
                    )
                    .ToList()
                    .Select(q => new
                    {
                        q.ID,
                        q.TDaily,
                        q.JC,
                        ModifiedDate = Utility.GetFormattedDate(q.Date),
                        ModifiedBy = q.FirstName + " " + q.LastName
                    }).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstHistory;
        }

        public bool UpdateFlow7782(long? _RecordID, decimal? _JC, long? _UserID)
        {
            bool Updated = false;
            try
            {
                int Result = context.SP_UpdateFlow7782(_RecordID, _JC, _UserID);
                if (Result == 2)
                    Updated = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Updated;
        }

        #endregion

        #region Para2

        public List<object> GetPara2()
        {
            List<object> lstPara2 = new List<object>();
            try
            {
                lstPara2 = (from SD in context.SP_RefShareDistribution
                            join Cal in context.SP_RefTDailyCalendar on SD.TDailyID equals Cal.TDailyID
                            where SD.SeasonID == (int)Constants.Seasons.Kharif && Cal.Year == DateTime.Now.Year - 1
                            select new
                            {
                                ID = Cal.TDailyID,
                                TDaily = Cal.ShortName,
                                ParaPunjab = SD.PunjabPara2,
                                ParaSindh = SD.SindhPara2,
                                Balochistan = SD.BalochistanShare,
                                KPKShare = SD.KPShare
                            })
                    .ToList()
                    .Select(q =>
                        new
                        {
                            q.ID,
                            q.TDaily,
                            Indus = q.ParaPunjab + q.ParaSindh + q.Balochistan + q.KPKShare
                        }).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstPara2;
        }

        #endregion

        #region Water Distribution

        public List<object> GetWaterDistribution()
        {
            List<object> lstWaterDist = new List<object>();
            try
            {
                lstWaterDist = (from WD in context.SP_RefWaterDistribution
                                join Cal in context.SP_RefTDailyCalendar on WD.TDailyID equals Cal.TDailyID
                                where Cal.Year == DateTime.Now.Year - 1
                                // because we always has all the TDailies of last year
                                select new
                                {
                                    ID = WD.ID,
                                    TDaily = Cal.ShortName,
                                    WD.Percentile0,
                                    WD.Percentile5,
                                    WD.Percentile10,
                                    WD.Percentile15,
                                    WD.Percentile20,
                                    WD.Percentile25,
                                    WD.Percentile30,
                                }).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstWaterDist;
        }


        #endregion

        #region Eastern Component

        public string GetFormattedYear(long _SeasonID, short? _Year)
        {
            string FormattedYear = _Year.ToString();
            try
            {
                if (_SeasonID == 1)
                    FormattedYear = Convert.ToString(_Year) + "-" + Convert.ToString(_Year + 1);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return FormattedYear;
        }

        public List<object> GetCalendarYears(long _SeasonID)
        {
            List<object> lstYears = new List<object>();
            try
            {
                lstYears = (from Cal in context.SP_RefTDailyCalendar
                            where Cal.SeasonID == _SeasonID
                            orderby Cal.Year ascending
                            select new
                            {
                                Cal.Year
                            }).ToList()
                    .Select(q => new
                        {
                            Year = GetFormattedYear(_SeasonID, q.Year)
                        }
                    ).Distinct().ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstYears;
        }

        public List<object> GetTDailyDataForEasternComponent(long _SeasonID, int _Year)
        {
            double? SulemankiComp = 0;
            double? BallokiComp = 0;
            double? EasternComp = 0;
            List<object> lstEastern = new List<object>();

            try
            {
                if (_SeasonID == (long)Constants.Seasons.Kharif)
                {
                    List<double?> lstSulemanki = (from CAL in context.SP_RefTDailyCalendar
                                                  join Barage in context.SP_TDailyBarrageData on CAL.ID equals Barage.TDailyCalendarID
                                                  where
                                                  Barage.StationID == (int)Constants.ShareDistribution.SulemankiID && CAL.SeasonID == _SeasonID &&
                                                  CAL.Year == _Year
                                                  select Barage.Upstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstBSOne = (from CAL in context.SP_RefTDailyCalendar
                                              join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                              where
                                              Canal.ChannelID == (int)Constants.ShareDistribution.BSOne && CAL.SeasonID == _SeasonID &&
                                              CAL.Year == _Year
                                              select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstBSTwo = (from CAL in context.SP_RefTDailyCalendar
                                              join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                              where
                                              Canal.ChannelID == (int)Constants.ShareDistribution.BSTwo && CAL.SeasonID == _SeasonID &&
                                              CAL.Year == _Year
                                              select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstBalloki = (from CAL in context.SP_RefTDailyCalendar
                                                join Barage in context.SP_TDailyBarrageData on CAL.ID equals Barage.TDailyCalendarID
                                                where
                                                Barage.StationID == (int)Constants.ShareDistribution.BallokiID && CAL.SeasonID == _SeasonID &&
                                                CAL.Year == _Year
                                                select Barage.Upstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstUCC = (from CAL in context.SP_RefTDailyCalendar
                                            join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                            where
                                            Canal.ChannelID == (int)Constants.ShareDistribution.UCC && CAL.SeasonID == _SeasonID &&
                                            CAL.Year == _Year
                                            select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstMR = (from CAL in context.SP_RefTDailyCalendar
                                           join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                           where
                                           Canal.ChannelID == (int)Constants.ShareDistribution.MR && CAL.SeasonID == _SeasonID &&
                                           CAL.Year == _Year
                                           select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstQB = (from CAL in context.SP_RefTDailyCalendar
                                           join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                           where
                                           Canal.ChannelID == (int)Constants.ShareDistribution.QB && CAL.SeasonID == _SeasonID &&
                                           CAL.Year == _Year
                                           select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<SP_RefTDailyCalendar> lstCalendar = (from CAl in context.SP_RefTDailyCalendar
                                                              where CAl.Year == _Year && CAl.SeasonID == _SeasonID
                                                              orderby CAl.TDailyID
                                                              select CAl).ToList<SP_RefTDailyCalendar>();

                    //each list must have exactly 18 records 
                    for (int i = 0;
                        i < (int)Constants.SeasonDistribution.KharifEnd;
                        i++, BallokiComp = 0, SulemankiComp = 0, EasternComp = 0)
                    {
                        if (lstSulemanki.ElementAtOrDefault(i).HasValue)
                            SulemankiComp = lstSulemanki.ElementAtOrDefault(i).Value;

                        if (lstBSOne.ElementAtOrDefault(i).HasValue)
                            SulemankiComp = SulemankiComp - lstBSOne.ElementAtOrDefault(i).Value;

                        if (lstBSTwo.ElementAtOrDefault(i).HasValue)
                            SulemankiComp = SulemankiComp - lstBSTwo.ElementAtOrDefault(i).Value;

                        if (lstBalloki.ElementAtOrDefault(i).HasValue)
                            BallokiComp = lstBalloki.ElementAtOrDefault(i).Value;

                        if (lstUCC.ElementAtOrDefault(i).HasValue)
                            BallokiComp = BallokiComp - lstUCC.ElementAtOrDefault(i).Value;

                        if (lstMR.ElementAtOrDefault(i).HasValue)
                            BallokiComp = BallokiComp - lstMR.ElementAtOrDefault(i).Value;

                        if (lstQB.ElementAtOrDefault(i).HasValue)
                            BallokiComp = BallokiComp - lstQB.ElementAtOrDefault(i).Value;

                        if (SulemankiComp >= 0 && BallokiComp >= 0)
                            EasternComp = SulemankiComp + BallokiComp;
                        else if (SulemankiComp >= 0 && BallokiComp < 0)
                            EasternComp = SulemankiComp;
                        else
                            EasternComp = BallokiComp;

                        object NewRecord = new
                        {
                            Sulemanki = lstSulemanki.ElementAtOrDefault(i).HasValue ? lstSulemanki.ElementAtOrDefault(i).Value : 0,
                            BS1 = lstBSOne.ElementAtOrDefault(i).HasValue ? lstBSOne.ElementAtOrDefault(i).Value : 0,
                            BS2 = lstBSTwo.ElementAtOrDefault(i).HasValue ? lstBSTwo.ElementAtOrDefault(i).Value : 0,
                            Balloki = lstBalloki.ElementAtOrDefault(i).HasValue ? lstBalloki.ElementAtOrDefault(i).Value : 0,
                            UCC = lstUCC.ElementAtOrDefault(i).HasValue ? lstUCC.ElementAtOrDefault(i).Value : 0,
                            MR = lstMR.ElementAtOrDefault(i).HasValue ? lstMR.ElementAtOrDefault(i).Value : 0,
                            QB = lstQB.ElementAtOrDefault(i).HasValue ? lstQB.ElementAtOrDefault(i).Value : 0,
                            Eastern = EasternComp,
                            TDaily = lstCalendar.ElementAtOrDefault(i).ShortName,
                            ID = lstCalendar.ElementAtOrDefault(i).TDailyID
                        };
                        lstEastern.Add(NewRecord);
                    }
                }
                else
                {
                    List<double?> lstSulemanki = (from CAL in context.SP_RefTDailyCalendar
                                                  join Barage in context.SP_TDailyBarrageData on CAL.ID equals Barage.TDailyCalendarID
                                                  where
                                                  Barage.StationID == (int)Constants.ShareDistribution.SulemankiID &&
                                                  ((CAL.Year == _Year && (CAL.TDailyID > 18 && CAL.TDailyID <= 27)) || (CAL.Year == _Year + 1 && CAL.TDailyID > 27))
                                                  select Barage.Upstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstBSOne = (from CAL in context.SP_RefTDailyCalendar
                                              join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                              where
                                              Canal.ChannelID == (int)Constants.ShareDistribution.BSOne && CAL.SeasonID == _SeasonID &&
                                              ((CAL.Year == _Year && (CAL.TDailyID > 18 && CAL.TDailyID <= 27)) || (CAL.Year == _Year + 1 && CAL.TDailyID > 27))
                                              select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstBSTwo = (from CAL in context.SP_RefTDailyCalendar
                                              join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                              where
                                              Canal.ChannelID == (int)Constants.ShareDistribution.BSTwo && CAL.SeasonID == _SeasonID &&
                                              ((CAL.Year == _Year && (CAL.TDailyID > 18 && CAL.TDailyID <= 27)) || (CAL.Year == _Year + 1 && CAL.TDailyID > 27))
                                              select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstBalloki = (from CAL in context.SP_RefTDailyCalendar
                                                join Barage in context.SP_TDailyBarrageData on CAL.ID equals Barage.TDailyCalendarID
                                                where
                                                Barage.StationID == (int)Constants.ShareDistribution.BallokiID && CAL.SeasonID == _SeasonID &&
                                                ((CAL.Year == _Year && (CAL.TDailyID > 18 && CAL.TDailyID <= 27)) || (CAL.Year == _Year + 1 && CAL.TDailyID > 27))
                                                select Barage.Upstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstUCC = (from CAL in context.SP_RefTDailyCalendar
                                            join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                            where
                                            Canal.ChannelID == (int)Constants.ShareDistribution.UCC && CAL.SeasonID == _SeasonID &&
                                            ((CAL.Year == _Year && (CAL.TDailyID > 18 && CAL.TDailyID <= 27)) || (CAL.Year == _Year + 1 && CAL.TDailyID > 27))
                                            select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstMR = (from CAL in context.SP_RefTDailyCalendar
                                           join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                           where
                                           Canal.ChannelID == (int)Constants.ShareDistribution.MR && CAL.SeasonID == _SeasonID &&
                                           ((CAL.Year == _Year && (CAL.TDailyID > 18 && CAL.TDailyID <= 27)) || (CAL.Year == _Year + 1 && CAL.TDailyID > 27))
                                           select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<double?> lstQB = (from CAL in context.SP_RefTDailyCalendar
                                           join Canal in context.SP_TDailyCanalData on CAL.ID equals Canal.TDailyCalendarID
                                           where
                                           Canal.ChannelID == (int)Constants.ShareDistribution.QB && CAL.SeasonID == _SeasonID &&
                                          ((CAL.Year == _Year && (CAL.TDailyID > 18 && CAL.TDailyID <= 27)) || (CAL.Year == _Year + 1 && CAL.TDailyID > 27))
                                           select Canal.Downstream / Constants.CusecsConvertion).ToList<double?>();

                    List<SP_RefTDailyCalendar> lstCalendar = (from CAl in context.SP_RefTDailyCalendar
                                                              where CAl.Year == _Year && CAl.SeasonID == _SeasonID
                                                              orderby CAl.TDailyID
                                                              select CAl).ToList<SP_RefTDailyCalendar>();

                    //each list must have exactly 18 records 
                    for (int i = 0;
                        i < (int)Constants.SeasonDistribution.KharifEnd;
                        i++, BallokiComp = 0, SulemankiComp = 0, EasternComp = 0)
                    {
                        if (lstSulemanki.ElementAtOrDefault(i).HasValue)
                            SulemankiComp = lstSulemanki.ElementAtOrDefault(i).Value;

                        if (lstBSOne.ElementAtOrDefault(i).HasValue)
                            SulemankiComp = SulemankiComp - lstBSOne.ElementAtOrDefault(i).Value;

                        if (lstBSTwo.ElementAtOrDefault(i).HasValue)
                            SulemankiComp = SulemankiComp - lstBSTwo.ElementAtOrDefault(i).Value;

                        if (lstBalloki.ElementAtOrDefault(i).HasValue)
                            BallokiComp = lstBalloki.ElementAtOrDefault(i).Value;

                        if (lstUCC.ElementAtOrDefault(i).HasValue)
                            BallokiComp = BallokiComp - lstUCC.ElementAtOrDefault(i).Value;

                        if (lstMR.ElementAtOrDefault(i).HasValue)
                            BallokiComp = BallokiComp - lstMR.ElementAtOrDefault(i).Value;

                        if (lstQB.ElementAtOrDefault(i).HasValue)
                            BallokiComp = BallokiComp - lstQB.ElementAtOrDefault(i).Value;

                        if (SulemankiComp >= 0 && BallokiComp >= 0)
                            EasternComp = SulemankiComp + BallokiComp;
                        else if (SulemankiComp >= 0 && BallokiComp < 0)
                            EasternComp = SulemankiComp;
                        else
                            EasternComp = BallokiComp;

                        object NewRecord = new
                        {
                            Sulemanki = lstSulemanki.ElementAtOrDefault(i).HasValue ? lstSulemanki.ElementAtOrDefault(i).Value : 0,
                            BS1 = lstBSOne.ElementAtOrDefault(i).HasValue ? lstBSOne.ElementAtOrDefault(i).Value : 0,
                            BS2 = lstBSTwo.ElementAtOrDefault(i).HasValue ? lstBSTwo.ElementAtOrDefault(i).Value : 0,
                            Balloki = lstBalloki.ElementAtOrDefault(i).HasValue ? lstBalloki.ElementAtOrDefault(i).Value : 0,
                            UCC = lstUCC.ElementAtOrDefault(i).HasValue ? lstUCC.ElementAtOrDefault(i).Value : 0,
                            MR = lstMR.ElementAtOrDefault(i).HasValue ? lstMR.ElementAtOrDefault(i).Value : 0,
                            QB = lstQB.ElementAtOrDefault(i).HasValue ? lstQB.ElementAtOrDefault(i).Value : 0,
                            Eastern = EasternComp,
                            TDaily = lstCalendar.ElementAtOrDefault(i).ShortName,
                            ID = lstCalendar.ElementAtOrDefault(i).TDailyID
                        };
                        lstEastern.Add(NewRecord);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstEastern;
        }

        #endregion

        #region Elevation Capacity

        public List<object> GetSavedDates()
        {
            List<object> lstDates = new List<object>();
            try
            {
                lstDates = (from EC in context.SP_RefElevationCapacity
                            select new
                            {
                                EC.ElevationCapacityDate
                            }).Distinct().OrderByDescending(w => w.ElevationCapacityDate).ToList()
                    .Select(q => new
                    {
                        Year = Utility.GetFormattedDate(q.ElevationCapacityDate)
                    }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstDates;
        }

        public List<SP_RefElevationCapacity> GetRecordsOfSelectedDate(DateTime _Date, long _RimstationID)
        {
            List<SP_RefElevationCapacity> lstRecords = new List<SP_RefElevationCapacity>();
            try
            {
                lstRecords = (from EC in context.SP_RefElevationCapacity
                              where EC.ElevationCapacityDate == _Date && EC.StationID == _RimstationID
                              select EC).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstRecords;
        }

        public bool UpDateElevationCapacity(long _RecordID, decimal _Capacity, long _UserID)
        {
            bool Result = false;
            try
            {
                int RowsUpdated = (context.SP_UpdateElevationCapacity(_RecordID, _Capacity, _UserID));
                if (RowsUpdated == 2)
                    Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public List<object> GetElevationCapacityHistory(long _ID)
        {
            List<object> lstHistory = new List<object>();
            try
            {
                lstHistory = (from ECH in context.SP_RefElevationCapacityHistory
                              join USR in context.UA_Users on (Int64)ECH.CreatedBy equals USR.ID
                              where ECH.ElevationCapacityID == _ID
                              orderby ECH.ModifiedDate descending
                              select new
                              {
                                  ID = ECH.ID,
                                  Level = ECH.Level,
                                  Capacity = ECH.Capacity,
                                  ElevationCapacityDate = ECH.ElevationCapacityDate,
                                  Date = ECH.CreatedDate,
                                  FirstName = USR.FirstName,
                                  LastName = USR.LastName
                              }
                    )
                    .ToList()
                    .Select(q => new
                    {
                        q.ID,
                        q.Level,
                        q.Capacity,
                        ElevationCapacityDate = Utility.GetFormattedDate(q.ElevationCapacityDate),
                        ModifiedDate = Utility.GetFormattedDate(q.Date),
                        ModifiedBy = q.FirstName + " " + q.LastName
                    }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstHistory;
        }

        public bool AddBulkElevationCapacity(List<SP_RefElevationCapacity> _lstEC)
        {
            try
            {
                context.SP_RefElevationCapacity.AddRange(_lstEC);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        #endregion

        #region Probability

        public List<object> GetProbabilityYears(int _SeasonID)
        {
            List<object> lstYears = new List<object>();
            try
            {
                lstYears = (from Prob in context.SP_ProbabilityTable
                            where Prob.SeasonID == _SeasonID
                            orderby Prob.ToYear ascending
                            select new
                            {
                                Prob.ToYear
                            }).Distinct().ToList()
                    .Select(q => new
                        {
                            Year = GetFormattedYear(_SeasonID, q.ToYear)
                        }
                    ).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstYears;
        }

        public List<SP_RefTDailyCalendar> CalendarList(int _SeasonID)
        {
            int Year = DateTime.Now.Year - 1;
            List<SP_RefTDailyCalendar> lstCalID = new List<SP_RefTDailyCalendar>();
            try
            {
                lstCalID = (from CAL in context.SP_RefTDailyCalendar
                            where CAL.Year == Year && CAL.SeasonID == _SeasonID
                            orderby CAL.TDailyID ascending
                            select CAL).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstCalID;
        }

        public List<object> ViewProbability(int _RimStationID, int _SeasonID, string _Year)
        {
            List<object> lstFinalProb = new List<object>();
            List<SP_ProbabilityTable> lstProb = new List<SP_ProbabilityTable>();
            object MAF = new object();
            object LKMAF = new object();
            object KharifMAF = new object();

            try
            {
                int Year = Convert.ToInt32(_Year.Substring(0, 4));

                lstProb = (from prob in context.SP_ProbabilityTable
                           where prob.SeasonID == _SeasonID && prob.StationID == _RimStationID && prob.ToYear == Year
                           select prob).ToList<SP_ProbabilityTable>();

                List<SP_RefTDailyCalendar> lstCalNames = CalendarList(_SeasonID);

                List<short?> lstTDailyID = lstProb.Select(q => q.TDailyID).Distinct().ToList();

                foreach (var v in lstTDailyID)
                {
                    List<SP_ProbabilityTable> TDaily =
                        lstProb.Where(q => q.TDailyID == v.Value).OrderBy(w => w.TDailyID).ToList();
                    int? TDID = TDaily.FirstOrDefault().TDailyID;
                    string TDName = "";

                    if (lstCalNames != null && lstCalNames.Count() == 18)
                        TDName = lstCalNames.Where(q => q.TDailyID == TDID).FirstOrDefault().ShortName.ToString();

                    object obj = new
                    {
                        TDailyName = TDName,
                        TDailyID = TDID,

                        MaxDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 0).FirstOrDefault().Discharge),
                        MaxVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 0).FirstOrDefault().CumulativeVolume),

                        FiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 5).FirstOrDefault().Discharge),
                        FiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 5).FirstOrDefault().CumulativeVolume),

                        TenDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 10).FirstOrDefault().Discharge),
                        TenVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 10).FirstOrDefault().CumulativeVolume),

                        FifteenDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 15).FirstOrDefault().Discharge),
                        FifteenVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 15).FirstOrDefault().CumulativeVolume),

                        TwentyDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 20).FirstOrDefault().Discharge),
                        TwentyVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 20).FirstOrDefault().CumulativeVolume),

                        TwentyFiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 25).FirstOrDefault().Discharge),
                        TwentyFiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 25).FirstOrDefault().CumulativeVolume),

                        ThirtyDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 30).FirstOrDefault().Discharge),
                        ThirtyVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 30).FirstOrDefault().CumulativeVolume),

                        ThirtyFiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 35).FirstOrDefault().Discharge),
                        ThirtyFiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 35).FirstOrDefault().CumulativeVolume),

                        FourtyDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 40).FirstOrDefault().Discharge),
                        FourtyVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 40).FirstOrDefault().CumulativeVolume),

                        FourtyFiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 45).FirstOrDefault().Discharge),
                        FourtyFiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 45).FirstOrDefault().CumulativeVolume),

                        FiftyDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 50).FirstOrDefault().Discharge),
                        FiftyVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 50).FirstOrDefault().CumulativeVolume),

                        FiftyFiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 55).FirstOrDefault().Discharge),
                        FiftyFiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 55).FirstOrDefault().CumulativeVolume),

                        SixtyDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 60).FirstOrDefault().Discharge),
                        SixtyVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 60).FirstOrDefault().CumulativeVolume),

                        SixtyFiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 65).FirstOrDefault().Discharge),
                        SixtyFiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 65).FirstOrDefault().CumulativeVolume),

                        SeventyDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 70).FirstOrDefault().Discharge),
                        SeventyVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 70).FirstOrDefault().CumulativeVolume),

                        SeventyFiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 75).FirstOrDefault().Discharge),
                        SeventyFiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 75).FirstOrDefault().CumulativeVolume),

                        EightyDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 80).FirstOrDefault().Discharge),
                        EightyVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 80).FirstOrDefault().CumulativeVolume),

                        EightyFiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 85).FirstOrDefault().Discharge),
                        EightyFiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 85).FirstOrDefault().CumulativeVolume),

                        NinetyDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 90).FirstOrDefault().Discharge),
                        NinetyVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 90).FirstOrDefault().CumulativeVolume),

                        NinetyFiveDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 95).FirstOrDefault().Discharge),
                        NinetyFiveVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 95).FirstOrDefault().CumulativeVolume),

                        MinimumDis =
                        String.Format("{0:0.0}", TDaily.Where(q => q.PercentileID == 100).FirstOrDefault().Discharge),
                        MinimumVol =
                        String.Format("{0:0.000}",
                            TDaily.Where(q => q.PercentileID == 100).FirstOrDefault().CumulativeVolume),
                    };

                    if (_SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        if (TDID == 7)
                        {
                            MAF = new
                            {
                                TDailyName = "EK(MAF)",
                                TDailyID = "",
                                MaxDis = "",
                                FiveDis = "",
                                TenDis = "",
                                FifteenDis = "",
                                TwentyDis = "",
                                TwentyFiveDis = "",
                                ThirtyDis = "",
                                ThirtyFiveDis = "",
                                FourtyDis = "",
                                FourtyFiveDis = "",
                                FiftyDis = "",
                                FiftyFiveDis = "",
                                SixtyDis = "",
                                SixtyFiveDis = "",
                                SeventyDis = "",
                                SeventyFiveDis = "",
                                EightyDis = "",
                                EightyFiveDis = "",
                                NinetyDis = "",
                                NinetyFiveDis = "",
                                MinimumDis = "",
                                MaxVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 0).FirstOrDefault().CumulativeVolume),
                                FiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 5).FirstOrDefault().CumulativeVolume),
                                TenVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 10).FirstOrDefault().CumulativeVolume),
                                FifteenVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 15).FirstOrDefault().CumulativeVolume),
                                TwentyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 20).FirstOrDefault().CumulativeVolume),
                                TwentyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 25).FirstOrDefault().CumulativeVolume),
                                ThirtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 30).FirstOrDefault().CumulativeVolume),
                                ThirtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 35).FirstOrDefault().CumulativeVolume),
                                FourtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 40).FirstOrDefault().CumulativeVolume),
                                FourtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 45).FirstOrDefault().CumulativeVolume),
                                FiftyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 50).FirstOrDefault().CumulativeVolume),
                                FiftyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 55).FirstOrDefault().CumulativeVolume),
                                SixtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 60).FirstOrDefault().CumulativeVolume),
                                SixtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 65).FirstOrDefault().CumulativeVolume),
                                SeventyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 70).FirstOrDefault().CumulativeVolume),
                                SeventyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 75).FirstOrDefault().CumulativeVolume),
                                EightyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 80).FirstOrDefault().CumulativeVolume),
                                EightyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 85).FirstOrDefault().CumulativeVolume),
                                NinetyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 90).FirstOrDefault().CumulativeVolume),
                                NinetyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 95).FirstOrDefault().CumulativeVolume),
                                MinimumVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 100).FirstOrDefault().CumulativeVolume),
                            };
                        }
                        else if (TDID == 18)
                        {
                            LKMAF = new
                            {
                                TDailyName = "LK(MAF)",
                                TDailyID = "",
                                MaxDis = "",
                                FiveDis = "",
                                TenDis = "",
                                FifteenDis = "",
                                TwentyDis = "",
                                TwentyFiveDis = "",
                                ThirtyDis = "",
                                ThirtyFiveDis = "",
                                FourtyDis = "",
                                FourtyFiveDis = "",
                                FiftyDis = "",
                                FiftyFiveDis = "",
                                SixtyDis = "",
                                SixtyFiveDis = "",
                                SeventyDis = "",
                                SeventyFiveDis = "",
                                EightyDis = "",
                                EightyFiveDis = "",
                                NinetyDis = "",
                                NinetyFiveDis = "",
                                MinimumDis = "",
                                MaxVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 0).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("MaxVol").GetValue(MAF)))),
                                FiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 5).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("FiveVol").GetValue(MAF)))),
                                TenVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 10).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("TenVol").GetValue(MAF)))),
                                FifteenVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 15).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("FifteenVol").GetValue(MAF)))),
                                TwentyVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 20).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("TwentyVol").GetValue(MAF)))),
                                TwentyFiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 25).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("TwentyFiveVol").GetValue(MAF)))),
                                ThirtyVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 30).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("ThirtyVol").GetValue(MAF)))),
                                ThirtyFiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 35).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("ThirtyFiveVol").GetValue(MAF)))),
                                FourtyVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 40).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("FourtyVol").GetValue(MAF)))),
                                FourtyFiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 45).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("FourtyFiveVol").GetValue(MAF)))),
                                FiftyVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 50).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("FiftyVol").GetValue(MAF)))),
                                FiftyFiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 55).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("FiftyFiveVol").GetValue(MAF)))),
                                SixtyVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 60).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("SixtyVol").GetValue(MAF)))),
                                SixtyFiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 65).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("SixtyFiveVol").GetValue(MAF)))),
                                SeventyVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 70).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("SeventyVol").GetValue(MAF)))),
                                SeventyFiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 75).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("SeventyFiveVol").GetValue(MAF)))),
                                EightyVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 80).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("EightyVol").GetValue(MAF)))),
                                EightyFiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 85).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("EightyFiveVol").GetValue(MAF)))),
                                NinetyVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 90).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("NinetyVol").GetValue(MAF)))),
                                NinetyFiveVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 95).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("NinetyFiveVol").GetValue(MAF)))),
                                MinimumVol =
                                String.Format("{0:0.000}",
                                (TDaily.Where(q => q.PercentileID == 100).FirstOrDefault().CumulativeVolume -
                                 Convert.ToDouble(MAF.GetType().GetProperty("MinimumVol").GetValue(MAF)))),
                            };

                            KharifMAF = new
                            {
                                TDailyName = "Kharif(MAF)",
                                TDailyID = "",
                                MaxDis = "",
                                FiveDis = "",
                                TenDis = "",
                                FifteenDis = "",
                                TwentyDis = "",
                                TwentyFiveDis = "",
                                ThirtyDis = "",
                                ThirtyFiveDis = "",
                                FourtyDis = "",
                                FourtyFiveDis = "",
                                FiftyDis = "",
                                FiftyFiveDis = "",
                                SixtyDis = "",
                                SixtyFiveDis = "",
                                SeventyDis = "",
                                SeventyFiveDis = "",
                                EightyDis = "",
                                EightyFiveDis = "",
                                NinetyDis = "",
                                NinetyFiveDis = "",
                                MinimumDis = "",
                                MaxVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 0).FirstOrDefault().CumulativeVolume),
                                FiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 5).FirstOrDefault().CumulativeVolume),
                                TenVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 10).FirstOrDefault().CumulativeVolume),
                                FifteenVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 15).FirstOrDefault().CumulativeVolume),
                                TwentyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 20).FirstOrDefault().CumulativeVolume),
                                TwentyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 25).FirstOrDefault().CumulativeVolume),
                                ThirtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 30).FirstOrDefault().CumulativeVolume),
                                ThirtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 35).FirstOrDefault().CumulativeVolume),
                                FourtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 40).FirstOrDefault().CumulativeVolume),
                                FourtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 45).FirstOrDefault().CumulativeVolume),
                                FiftyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 50).FirstOrDefault().CumulativeVolume),
                                FiftyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 55).FirstOrDefault().CumulativeVolume),
                                SixtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 60).FirstOrDefault().CumulativeVolume),
                                SixtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 65).FirstOrDefault().CumulativeVolume),
                                SeventyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 70).FirstOrDefault().CumulativeVolume),
                                SeventyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 75).FirstOrDefault().CumulativeVolume),
                                EightyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 80).FirstOrDefault().CumulativeVolume),
                                EightyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 85).FirstOrDefault().CumulativeVolume),
                                NinetyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 90).FirstOrDefault().CumulativeVolume),
                                NinetyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 95).FirstOrDefault().CumulativeVolume),
                                MinimumVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 100).FirstOrDefault().CumulativeVolume),
                            };
                        }
                    }
                    else
                    {
                        if (TDID == 36)
                        {
                            MAF = new
                            {
                                TDailyName = "Rabi(MAF)",
                                TDailyID = "",
                                MaxDis = "",
                                FiveDis = "",
                                TenDis = "",
                                FifteenDis = "",
                                TwentyDis = "",
                                TwentyFiveDis = "",
                                ThirtyDis = "",
                                ThirtyFiveDis = "",
                                FourtyDis = "",
                                FourtyFiveDis = "",
                                FiftyDis = "",
                                FiftyFiveDis = "",
                                SixtyDis = "",
                                SixtyFiveDis = "",
                                SeventyDis = "",
                                SeventyFiveDis = "",
                                EightyDis = "",
                                EightyFiveDis = "",
                                NinetyDis = "",
                                NinetyFiveDis = "",
                                MinimumDis = "",
                                MaxVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 0).FirstOrDefault().CumulativeVolume),
                                FiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 5).FirstOrDefault().CumulativeVolume),
                                TenVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 10).FirstOrDefault().CumulativeVolume),
                                FifteenVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 15).FirstOrDefault().CumulativeVolume),
                                TwentyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 20).FirstOrDefault().CumulativeVolume),
                                TwentyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 25).FirstOrDefault().CumulativeVolume),
                                ThirtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 30).FirstOrDefault().CumulativeVolume),
                                ThirtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 35).FirstOrDefault().CumulativeVolume),
                                FourtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 40).FirstOrDefault().CumulativeVolume),
                                FourtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 45).FirstOrDefault().CumulativeVolume),
                                FiftyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 50).FirstOrDefault().CumulativeVolume),
                                FiftyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 55).FirstOrDefault().CumulativeVolume),
                                SixtyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 60).FirstOrDefault().CumulativeVolume),
                                SixtyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 65).FirstOrDefault().CumulativeVolume),
                                SeventyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 70).FirstOrDefault().CumulativeVolume),
                                SeventyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 75).FirstOrDefault().CumulativeVolume),
                                EightyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 80).FirstOrDefault().CumulativeVolume),
                                EightyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 85).FirstOrDefault().CumulativeVolume),
                                NinetyVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 90).FirstOrDefault().CumulativeVolume),
                                NinetyFiveVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 95).FirstOrDefault().CumulativeVolume),
                                MinimumVol =
                                String.Format("{0:0.000}",
                                    TDaily.Where(q => q.PercentileID == 100).FirstOrDefault().CumulativeVolume),
                            };
                        }
                    }
                    lstFinalProb.Add(obj);
                }

                if (_SeasonID == (int)Constants.Seasons.Kharif)
                {
                    lstFinalProb.Add(MAF);
                    lstFinalProb.Add(LKMAF);
                    lstFinalProb.Add(KharifMAF);
                }
                else
                    lstFinalProb.Add(MAF);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstFinalProb;
        }

        #endregion

        #region Statistical Inflowforecasting

        public object GetPrevoidSeasonMAF(int _SeasonID)
        {
            List<SP_GetPreviousSeasonMaf_Result> lstMaf = new List<SP_GetPreviousSeasonMaf_Result>();
            double? JhelumAtMangla = 0;
            double? ChenabAtMarala = 0;
            double? IndusAtTarbela = 0;
            double? KabulAtNowshera = 0;
            object MafValues = new object();
            try
            {
                lstMaf = context.SP_GetPreviousSeasonMaf(_SeasonID).ToList<SP_GetPreviousSeasonMaf_Result>();

                List<SP_GetPreviousSeasonMaf_Result> lstJM = lstMaf.Where(q => q.StationID == 18).ToList();
                List<SP_GetPreviousSeasonMaf_Result> lstIT = lstMaf.Where(q => q.StationID == 20).ToList();
                List<SP_GetPreviousSeasonMaf_Result> lstCM = lstMaf.Where(q => q.StationID == 5).ToList();
                List<SP_GetPreviousSeasonMaf_Result> lstKN = lstMaf.Where(q => q.StationID == 24).ToList();

                foreach (var v in lstJM)
                    JhelumAtMangla += v.MAF;
                String JM = String.Format("{0:0.000}", JhelumAtMangla);

                foreach (var v in lstIT)
                    IndusAtTarbela += v.MAF;
                String IT = String.Format("{0:0.000}", IndusAtTarbela);

                foreach (var v in lstCM)
                    ChenabAtMarala += v.MAF;
                String CM = String.Format("{0:0.000}", ChenabAtMarala);

                foreach (var v in lstKN)
                    KabulAtNowshera += v.MAF;
                String KN = String.Format("{0:0.000}", KabulAtNowshera);

                MafValues = new
                {
                    JM = JM,
                    CM = CM,
                    IT = IT,
                    KN = KN
                };
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return MafValues;
        }

        public List<object> GetMatchingYears(long? _StationID, decimal _StartVariation, decimal _EndVariation)
        {
            int Season = 1;
            List<object> lstMatchingYears = new List<object>();
            object AddNew = new object();
            decimal? Rabi = null;
            decimal? EK = null;
            decimal? LK = null;
            string SYear = "";
            try
            {
                if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch &&
                     DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
                    || DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningApril)
                    Season = 2;

                List<SP_GetMatchingYears_Result> lstAllYears =
                    context.SP_GetMatchingYears(_StationID).ToList<SP_GetMatchingYears_Result>();
                if (Season == (int)Constants.Seasons.Kharif) // planning for kharif compare rabi
                {
                    for (int i = 0; i < lstAllYears.Count(); i++)
                    {
                        if (lstAllYears.ElementAtOrDefault(i).RVolume >= _StartVariation &&
                            lstAllYears.ElementAtOrDefault(i).RVolume <= _EndVariation &&
                            lstAllYears.ElementAtOrDefault(i).YEAR < DateTime.Now.Year - 1)
                        {
                            SYear = lstAllYears.ElementAtOrDefault(i).YEAR + "-" +
                                    (lstAllYears.ElementAtOrDefault(i).YEAR + 1).ToString();
                            Rabi = lstAllYears.ElementAtOrDefault(i).RVolume;
                            if (i + 1 < lstAllYears.Count())
                            {
                                EK = lstAllYears.ElementAtOrDefault(i + 1).EVolume;
                                LK = lstAllYears.ElementAtOrDefault(i + 1).LVolume;
                            }

                            AddNew = new
                            {
                                years = SYear,
                                Rabi = Rabi,
                                Kharif = EK,
                                LK = LK
                            };
                            lstMatchingYears.Add(AddNew);
                        }
                    }


                    //foreach (var lst in lstAllYears)
                    //{
                    //    if (lst.RVolume >= _StartVariation && lst.RVolume <= _EndVariation && lst.YEAR != DateTime.Now.Year - 1)
                    //    {
                    //        SYear = lst.YEAR + "-" + (lst.YEAR + 1).ToString();
                    //        AddNew = new
                    //        {
                    //            years = SYear,
                    //            Rabi = lst.RVolume,
                    //            Kharif = lst.EVolume,
                    //            LK = lst.LVolume
                    //        };
                    //        lstMatchingYears.Add(AddNew);
                    //    }
                    //}
                }
                else // planning for rabi
                {
                    foreach (var lst in lstAllYears)
                    {
                        if (lst.KVolume >= _StartVariation && lst.KVolume <= _EndVariation &&
                            lst.YEAR != DateTime.Now.Year)
                        {
                            AddNew = new
                            {
                                years = lst.YEAR,
                                Rabi = lst.RVolume,
                                Kharif = lst.KVolume,
                                LK = lst.LVolume
                            };
                            lstMatchingYears.Add(AddNew);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstMatchingYears;
        }

        public int? ForecastProbability(decimal? _MAF, int? _StationID, int? _SeasonID, int? TDAilyID)
        {
            short? ReturnProbability = null;
            try
            {
                ReturnProbability =
                    context.SP_GetProbabiltyForMatchingInflows(_MAF, _StationID, _SeasonID, TDAilyID).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ReturnProbability;
        }

        public List<SP_GetForecastedValues_Result> GetForecastedValues(int _SeasonID, int? _JM, int? _JMLK, int? _CM,
            int? _CMLK, int? _IT, int? _ITLK, int? _KN, int? _KNLK, bool _CalculateMAF)
        {
            List<SP_GetForecastedValues_Result> lstResult = new List<SP_GetForecastedValues_Result>();
            List<SP_GetForecastedValues_Result> lstResultFinal = new List<SP_GetForecastedValues_Result>();
            SP_GetForecastedValues_Result ObjResult = new SP_GetForecastedValues_Result();
            double? JMEK = 0;
            double? JMLK = 0;
            double? CMEK = 0;
            double? CMLK = 0;
            double? ITEK = 0;
            double? ITLK = 0;
            double? KNEK = 0;
            double? KNLK = 0;
            try
            {
                lstResult =
                    context.SP_GetForecastedValues(_SeasonID, _JM, _JMLK, _CM, _CMLK, _IT, _ITLK, _KN, _KNLK).ToList();

                if (_CalculateMAF)
                {
                    if (lstResult.Count() > 0)
                    {
                        if (_SeasonID == (int)Constants.Seasons.Kharif) // MAF footer
                        {
                            foreach (var v in lstResult)
                            {
                                if (v.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                                {
                                    if (v.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                                    {
                                        JMEK += v.JhelumMangla * Constants.TDailyConversion;
                                        CMEK += v.ChenabMarala * Constants.TDailyConversion;
                                        ITEK += v.IndusTarbela * Constants.TDailyConversion;
                                        KNEK += v.KabulNowshera * Constants.TDailyConversion;
                                    }
                                    else
                                    {
                                        JMEK += v.JhelumMangla;
                                        CMEK += v.ChenabMarala;
                                        ITEK += v.IndusTarbela;
                                        KNEK += v.KabulNowshera;
                                    }
                                }
                                else
                                {
                                    if (v.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                                        v.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                                    {
                                        JMLK += v.JhelumMangla * Constants.TDailyConversion;
                                        CMLK += v.ChenabMarala * Constants.TDailyConversion;
                                        ITLK += v.IndusTarbela * Constants.TDailyConversion;
                                        KNLK += v.KabulNowshera * Constants.TDailyConversion;
                                    }
                                    else
                                    {
                                        JMLK += v.JhelumMangla;
                                        CMLK += v.ChenabMarala;
                                        ITLK += v.IndusTarbela;
                                        KNLK += v.KabulNowshera;
                                    }
                                }
                            }

                            SP_GetForecastedValues_Result MAF = new SP_GetForecastedValues_Result();
                            MAF.Shortname = "EK(MAF)";
                            MAF.JhelumMangla =
                                Convert.ToDouble(String.Format("{0:0.000}", (JMEK * Constants.MAFConversion)));
                            MAF.ChenabMarala =
                                Convert.ToDouble(String.Format("{0:0.000}", (CMEK * Constants.MAFConversion)));
                            MAF.IndusTarbela =
                                Convert.ToDouble(String.Format("{0:0.000}", (ITEK * Constants.MAFConversion)));
                            MAF.KabulNowshera =
                                Convert.ToDouble(String.Format("{0:0.000}", (KNEK * Constants.MAFConversion)));
                            lstResult.Add(MAF);

                            MAF = new SP_GetForecastedValues_Result();
                            MAF.Shortname = "LK(MAF)";
                            MAF.JhelumMangla =
                                Convert.ToDouble(String.Format("{0:0.000}", (JMLK * Constants.MAFConversion)));
                            MAF.ChenabMarala =
                                Convert.ToDouble(String.Format("{0:0.000}", (CMLK * Constants.MAFConversion)));
                            MAF.IndusTarbela =
                                Convert.ToDouble(String.Format("{0:0.000}", (ITLK * Constants.MAFConversion)));
                            MAF.KabulNowshera =
                                Convert.ToDouble(String.Format("{0:0.000}", (KNLK * Constants.MAFConversion)));
                            lstResult.Add(MAF);

                            MAF = new SP_GetForecastedValues_Result();
                            MAF.Shortname = "Total(MAF)";
                            MAF.JhelumMangla =
                                Convert.ToDouble(String.Format("{0:0.000}", ((JMEK + JMLK) * Constants.MAFConversion)));
                            MAF.ChenabMarala =
                                Convert.ToDouble(String.Format("{0:0.000}", ((CMEK + CMLK) * Constants.MAFConversion)));
                            MAF.IndusTarbela =
                                Convert.ToDouble(String.Format("{0:0.000}", ((ITEK + ITLK) * Constants.MAFConversion)));
                            MAF.KabulNowshera =
                                Convert.ToDouble(String.Format("{0:0.000}", ((KNEK + KNLK) * Constants.MAFConversion)));
                            lstResult.Add(MAF);
                        }
                        else
                        {
                            foreach (var v in lstResult)
                            {
                                ObjResult = new SP_GetForecastedValues_Result();
                                ObjResult.ChenabMarala = Convert.ToDouble(String.Format("{0:0.0}", v.ChenabMarala));
                                ObjResult.IndusTarbela = Convert.ToDouble(String.Format("{0:0.0}", v.IndusTarbela));
                                ObjResult.JhelumMangla = Convert.ToDouble(String.Format("{0:0.0}", v.JhelumMangla));
                                ObjResult.KabulNowshera = Convert.ToDouble(String.Format("{0:0.0}", v.KabulNowshera));
                                ObjResult.Shortname = v.Shortname;
                                ObjResult.TDailyID = v.TDailyID;
                                lstResultFinal.Add(ObjResult);

                                if (v.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily ||
                                    v.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                                    || v.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                                    v.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                                {
                                    JMEK += v.JhelumMangla * Constants.TDailyConversion;
                                    CMEK += v.ChenabMarala * Constants.TDailyConversion;
                                    ITEK += v.IndusTarbela * Constants.TDailyConversion;
                                    KNEK += v.KabulNowshera * Constants.TDailyConversion;
                                }
                                else if (v.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                                {
                                    if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                    {
                                        JMEK += v.JhelumMangla * Constants.LeapYearTrue;
                                        CMEK += v.ChenabMarala * Constants.LeapYearTrue;
                                        ITEK += v.IndusTarbela * Constants.LeapYearTrue;
                                        KNEK += v.KabulNowshera * Constants.LeapYearTrue;
                                    }
                                    else
                                    {
                                        JMEK += v.JhelumMangla * Constants.LeapYearFalse;
                                        CMEK += v.ChenabMarala * Constants.LeapYearFalse;
                                        ITEK += v.IndusTarbela * Constants.LeapYearFalse;
                                        KNEK += v.KabulNowshera * Constants.LeapYearFalse;
                                    }
                                }
                                else
                                {
                                    JMEK += v.JhelumMangla;
                                    CMEK += v.ChenabMarala;
                                    ITEK += v.IndusTarbela;
                                    KNEK += v.KabulNowshera;
                                }
                            }

                            SP_GetForecastedValues_Result MAF = new SP_GetForecastedValues_Result();
                            MAF.Shortname = "Rabi(MAF)";
                            MAF.JhelumMangla =
                                Convert.ToDouble(String.Format("{0:0.000}",
                                    Convert.ToDouble(JMEK * Constants.MAFConversion)));
                            MAF.ChenabMarala =
                                Convert.ToDouble(String.Format("{0:0.000}",
                                    Convert.ToDouble(CMEK * Constants.MAFConversion)));
                            MAF.IndusTarbela =
                                Convert.ToDouble(String.Format("{0:0.000}",
                                    Convert.ToDouble(ITEK * Constants.MAFConversion)));
                            MAF.KabulNowshera =
                                Convert.ToDouble(String.Format("{0:0.000}",
                                    Convert.ToDouble(KNEK * Constants.MAFConversion)));
                            lstResult.Add(MAF);
                        }
                    }
                }
                else
                    lstResultFinal.AddRange(lstResult);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstResultFinal;
        }

        public List<object> GetFinalForecastedValues(int _SeasonID, int? _JM, int? _JMLK, int? _CM, int? _CMLK, int? _IT,
            int? _ITLK, int? _KN, int? _KNLK, bool _CalculateMAF)
        {
            List<SP_GetForecastedValues_Result> lstResult = new List<SP_GetForecastedValues_Result>();
            List<object> lstResultFinal = new List<object>();
            object ObjResult;
            double? JMEK = 0;
            double? JMLK = 0;
            double? CMEK = 0;
            double? CMLK = 0;
            double? ITEK = 0;
            double? ITLK = 0;
            double? KNEK = 0;
            double? KNLK = 0;
            try
            {
                lstResult =
                    context.SP_GetForecastedValues(_SeasonID, _JM, _JMLK, _CM, _CMLK, _IT, _ITLK, _KN, _KNLK).ToList();

                if (_CalculateMAF)
                {
                    if (lstResult.Count() > 0)
                    {
                        if (_SeasonID == (int)Constants.Seasons.Kharif) // MAF footer
                        {
                            foreach (var v in lstResult)
                            {
                                ObjResult = new
                                {
                                    ChenabMarala = String.Format("{0:0.0}", v.ChenabMarala),
                                    IndusTarbela = String.Format("{0:0.0}", v.IndusTarbela),
                                    JhelumMangla = String.Format("{0:0.0}", v.JhelumMangla),
                                    KabulNowshera = String.Format("{0:0.0}", v.KabulNowshera),
                                    Shortname = v.Shortname,
                                    TDailyID = v.TDailyID
                                };
                                lstResultFinal.Add(ObjResult);

                                if (v.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                                {
                                    if (v.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                                    {
                                        JMEK += v.JhelumMangla * Constants.TDailyConversion;
                                        CMEK += v.ChenabMarala * Constants.TDailyConversion;
                                        ITEK += v.IndusTarbela * Constants.TDailyConversion;
                                        KNEK += v.KabulNowshera * Constants.TDailyConversion;
                                    }
                                    else
                                    {
                                        JMEK += v.JhelumMangla;
                                        CMEK += v.ChenabMarala;
                                        ITEK += v.IndusTarbela;
                                        KNEK += v.KabulNowshera;
                                    }
                                }
                                else
                                {
                                    if (v.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                                        v.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                                    {
                                        JMLK += v.JhelumMangla * Constants.TDailyConversion;
                                        CMLK += v.ChenabMarala * Constants.TDailyConversion;
                                        ITLK += v.IndusTarbela * Constants.TDailyConversion;
                                        KNLK += v.KabulNowshera * Constants.TDailyConversion;
                                    }
                                    else
                                    {
                                        JMLK += v.JhelumMangla;
                                        CMLK += v.ChenabMarala;
                                        ITLK += v.IndusTarbela;
                                        KNLK += v.KabulNowshera;
                                    }
                                }
                            }

                            object MAF = new
                            {
                                Shortname = "EK(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JMEK * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CMEK * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (ITEK * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KNEK * Constants.MAFConversion))
                            };

                            lstResultFinal.Add(MAF);

                            MAF = new
                            {
                                Shortname = "LK(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JMLK * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CMLK * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (ITLK * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KNLK * Constants.MAFConversion)),
                            };
                            lstResultFinal.Add(MAF);

                            MAF = new
                            {
                                Shortname = "Total(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", ((JMEK + JMLK) * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", ((CMEK + CMLK) * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", ((ITEK + ITLK) * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", ((KNEK + KNLK) * Constants.MAFConversion))
                            };
                            lstResultFinal.Add(MAF);
                        }
                        else
                        {
                            foreach (var v in lstResult)
                            {
                                ObjResult = new
                                {
                                    ChenabMarala = String.Format("{0:0.0}", v.ChenabMarala),
                                    IndusTarbela = String.Format("{0:0.0}", v.IndusTarbela),
                                    JhelumMangla = String.Format("{0:0.0}", v.JhelumMangla),
                                    KabulNowshera = String.Format("{0:0.0}", v.KabulNowshera),
                                    Shortname = v.Shortname,
                                    TDailyID = v.TDailyID
                                };

                                lstResultFinal.Add(ObjResult);

                                if (v.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily ||
                                    v.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                                    || v.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                                    v.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                                {
                                    JMEK += v.JhelumMangla * Constants.TDailyConversion;
                                    CMEK += v.ChenabMarala * Constants.TDailyConversion;
                                    ITEK += v.IndusTarbela * Constants.TDailyConversion;
                                    KNEK += v.KabulNowshera * Constants.TDailyConversion;
                                }
                                else if (v.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                                {
                                    if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                    {
                                        JMEK += v.JhelumMangla * Constants.LeapYearTrue;
                                        CMEK += v.ChenabMarala * Constants.LeapYearTrue;
                                        ITEK += v.IndusTarbela * Constants.LeapYearTrue;
                                        KNEK += v.KabulNowshera * Constants.LeapYearTrue;
                                    }
                                    else
                                    {
                                        JMEK += v.JhelumMangla * Constants.LeapYearFalse;
                                        CMEK += v.ChenabMarala * Constants.LeapYearFalse;
                                        ITEK += v.IndusTarbela * Constants.LeapYearFalse;
                                        KNEK += v.KabulNowshera * Constants.LeapYearFalse;
                                    }
                                }
                                else
                                {
                                    JMEK += v.JhelumMangla;
                                    CMEK += v.ChenabMarala;
                                    ITEK += v.IndusTarbela;
                                    KNEK += v.KabulNowshera;
                                }
                            }

                            //SP_GetForecastedValues_Result MAF = new SP_GetForecastedValues_Result();
                            //MAF.Shortname = "Rabi(MAF)";
                            //MAF.JhelumMangla = Convert.ToDouble(String.Format("{0:0.000}", Convert.ToDouble(JMEK * Constants.MAFConversion)));
                            //MAF.ChenabMarala = Convert.ToDouble(String.Format("{0:0.000}", Convert.ToDouble(CMEK * Constants.MAFConversion)));
                            //MAF.IndusTarbela = Convert.ToDouble(String.Format("{0:0.000}", Convert.ToDouble(ITEK * Constants.MAFConversion)));
                            //MAF.KabulNowshera = Convert.ToDouble(String.Format("{0:0.000}", Convert.ToDouble(KNEK * Constants.MAFConversion)));
                            //lstResult.Add(MAF);


                            ObjResult = new
                            {
                                Shortname = "Rabi(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JMEK * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CMEK * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (ITEK * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KNEK * Constants.MAFConversion))
                            };
                            lstResultFinal.Add(ObjResult);
                        }
                    }
                }
                else
                    lstResultFinal.AddRange(lstResult);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstResultFinal;
        }

        public List<SP_ForecastDraft> GetDraftsInformation()
        {
            int SeasonID = 2;
            short Year = Convert.ToInt16(DateTime.Now.Year);
            List<SP_ForecastDraft> lstDrafts = new List<SP_ForecastDraft>();
            try
            {
                if ((DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember &&
                     DateTime.Now.Day >= (int)Constants.PlanningMonthsAndDays.PlanningDay)
                    || DateTime.Now.Month > (int)Constants.PlanningMonthsAndDays.RabiPlanningSeptember)
                    SeasonID = 1;
                else if (DateTime.Now.Month < (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch ||
                         (DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch &&
                          DateTime.Now.Day < (int)Constants.PlanningMonthsAndDays.PlanningDay))
                {
                    SeasonID = 1;
                    Year = Convert.ToInt16(DateTime.Now.Year - 1);
                }

                lstDrafts = (from draft in context.SP_ForecastDraft
                             where
                             draft.DraftType == (int)Constants.InflowForecstDrafts.StatisticalDraft &&
                             draft.SeasonID == SeasonID && draft.Year == Year
                             select draft).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstDrafts;
        }

        public bool DeleteDraft(long _DraftID)
        {
            List<SP_ForecastScenario> lstScenarioIDs = new List<SP_ForecastScenario>();

            try
            {
                long Count = (from PD in context.SP_PlanDraft
                              where PD.ForecastDraftID == _DraftID
                              select PD).ToList().Count();

                if (Count > 0)
                    return false;

                lstScenarioIDs = (from Scenario in context.SP_ForecastScenario
                                  where Scenario.ForecastDraftID == _DraftID
                                  select Scenario).ToList();

                foreach (var sce in lstScenarioIDs)
                    context.SP_ForecastData.RemoveRange(context.SP_ForecastData.Where(q => q.ForecastScenarioID == sce.ID));

                context.SP_ForecastScenario.RemoveRange(context.SP_ForecastScenario.Where(q => q.ForecastDraftID == _DraftID));
                context.SP_ForecastDraft.RemoveRange(context.SP_ForecastDraft.Where(q => q.ID == _DraftID));
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public List<object> GetStatDraftDetail(long _RecordID, string _Scenario)
        {
            List<SP_ForecastData> lstSavedValues = new List<SP_ForecastData>();
            List<SP_ForecastData> lstTDaily = new List<SP_ForecastData>();
            List<object> lstTDailyValues = new List<object>();
            double? JMEK = 0;
            double? JM = 0;
            double? CMEK = 0;
            double? CM = 0;
            double? ITEK = 0;
            double? IT = 0;
            double? KNEK = 0;
            double? KN = 0;
            int LoopStart = 1;
            int LoopEnd = 18;

            if ((int)Constants.Seasons.Rabi == Utility.GetCurrentSeasonForView())
            {
                LoopStart = 19;
                LoopEnd = 36;
            }

            try
            {
                lstSavedValues = (from Data in context.SP_ForecastData
                                  where ((from Scenario in context.SP_ForecastScenario
                                          where Scenario.ForecastDraftID == _RecordID && Scenario.Scenario == _Scenario
                                          select Scenario.ID).ToList())
                                      .Contains(Data.ForecastScenarioID)
                                  select Data).ToList();

                for (int i = LoopStart; i <= LoopEnd; i++)
                {
                    lstTDaily = new List<SP_ForecastData>();
                    lstTDaily = lstSavedValues.Where(q => q.TDailyID == i).ToList();

                    object TDaily = new
                    {
                        Shortname =
                        Utility.GetTDailyShortName(lstTDaily.ElementAtOrDefault(0).TDailyID,
                            Utility.GetCurrentSeasonForView()),
                        JhelumMangla = String.Format("{0:0.0}", lstTDaily.ElementAtOrDefault(0).Volume),
                        ChenabMarala = String.Format("{0:0.0}", lstTDaily.ElementAtOrDefault(1).Volume),
                        IndusTarbela = String.Format("{0:0.0}", lstTDaily.ElementAtOrDefault(2).Volume),
                        KabulNowshera = String.Format("{0:0.0}", lstTDaily.ElementAtOrDefault(3).Volume)
                    };

                    lstTDailyValues.Add(TDaily);

                    if (lstTDaily.ElementAtOrDefault(0).TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (lstTDaily.ElementAtOrDefault(0).TDailyID ==
                            (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                        {
                            JMEK += lstTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CMEK += lstTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            ITEK += lstTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KNEK += lstTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;
                        }
                        else
                        {
                            JMEK += lstTDaily.ElementAtOrDefault(0).Volume;
                            CMEK += lstTDaily.ElementAtOrDefault(1).Volume;
                            ITEK += lstTDaily.ElementAtOrDefault(2).Volume;
                            KNEK += lstTDaily.ElementAtOrDefault(3).Volume;
                        }
                    }
                    else if (lstTDaily.ElementAtOrDefault(0).TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                    {
                        if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily
                            || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                        {
                            JM += lstTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CM += lstTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            IT += lstTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KN += lstTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;
                        }
                        else
                        {
                            JM += lstTDaily.ElementAtOrDefault(0).Volume;
                            CM += lstTDaily.ElementAtOrDefault(1).Volume;
                            IT += lstTDaily.ElementAtOrDefault(2).Volume;
                            KN += lstTDaily.ElementAtOrDefault(3).Volume;
                        }

                        if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.SeasonDistribution.LKEnd)
                        {
                            TDaily = new
                            {
                                Shortname = "EK(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JMEK * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CMEK * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (ITEK * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KNEK * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);

                            TDaily = new
                            {
                                Shortname = "LK(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JM * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CM * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (IT * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KN * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);

                            TDaily = new
                            {
                                Shortname = "Total(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", ((JMEK + JM) * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", ((CMEK + CM) * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", ((ITEK + IT) * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", ((KNEK + KN) * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);
                        }
                    }
                    else
                    {
                        if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily
                            || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                            || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily
                            || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                        {
                            JM += lstTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CM += lstTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            IT += lstTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KN += lstTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;
                        }
                        else if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                        {
                            if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                            {
                                JM += lstTDaily.ElementAtOrDefault(0).Volume * Constants.LeapYearTrue;
                                CM += lstTDaily.ElementAtOrDefault(1).Volume * Constants.LeapYearTrue;
                                IT += lstTDaily.ElementAtOrDefault(2).Volume * Constants.LeapYearTrue;
                                KN += lstTDaily.ElementAtOrDefault(3).Volume * Constants.LeapYearTrue;
                            }
                            else
                            {
                                JM += lstTDaily.ElementAtOrDefault(0).Volume * Constants.LeapYearFalse;
                                CM += lstTDaily.ElementAtOrDefault(1).Volume * Constants.LeapYearFalse;
                                IT += lstTDaily.ElementAtOrDefault(2).Volume * Constants.LeapYearFalse;
                                KN += lstTDaily.ElementAtOrDefault(3).Volume * Constants.LeapYearFalse;
                            }
                        }
                        else
                        {
                            JM += lstTDaily.ElementAtOrDefault(0).Volume;
                            CM += lstTDaily.ElementAtOrDefault(1).Volume;
                            IT += lstTDaily.ElementAtOrDefault(2).Volume;
                            KN += lstTDaily.ElementAtOrDefault(3).Volume;
                        }

                        if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.SeasonDistribution.RabiEnd)
                        {
                            TDaily = new
                            {
                                Shortname = "Rabi(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JM * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CM * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (IT * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KN * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTDailyValues;
        }

        public string GetDraftCountName(int _Season)
        {
            string DraftCount = "";
            try
            {
                int Count = (from Draft in context.SP_ForecastDraft
                             where
                             Draft.Year == DateTime.Now.Year && Draft.SeasonID == _Season &&
                             Draft.DraftType == (int)Constants.InflowForecstDrafts.StatisticalDraft
                             select Draft).Count();
                if (Count == 0)
                {
                    DraftCount = "1st ";
                }
                else if (Count == 1)
                {
                    DraftCount = "2nd ";
                }
                else if (Count == 2)
                {
                    DraftCount = "3rd ";
                }
                else if (Count == 3)
                {
                    DraftCount = "4th ";
                }
                else if (Count == 4)
                {
                    DraftCount = "5th ";
                }
                else if (Count == 5)
                {
                    DraftCount = "Not Allowed";
                }


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return DraftCount;
        }


        #endregion

        #region SRM InflowForecasting

        public List<SP_ForecastDraft> GetSRMDrafts()
        {
            List<SP_ForecastDraft> objDrafts = new List<SP_ForecastDraft>();
            try
            {
                objDrafts = (from draft in context.SP_ForecastDraft
                             where draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft
                                   && draft.SeasonID == (Int16)Constants.Seasons.Kharif
                                   && draft.Year == DateTime.Now.Year
                             select draft).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objDrafts;
        }

        public List<object> GetSRMDetail(long _RecordID, string _Scenario)
        {
            List<SP_ForecastData> lstSavedValues = new List<SP_ForecastData>();
            List<SP_ForecastData> lstTDaily = new List<SP_ForecastData>();
            List<object> lstTDailyValues = new List<object>();
            List<long> lstScenario = new List<long>();
            double? JMEK = 0;
            double? JM = 0;
            double? CMEK = 0;
            double? CM = 0;
            double? ITEK = 0;
            double? IT = 0;
            double? KNEK = 0;
            double? KN = 0;
            try
            {
                lstScenario = (from Scenario in context.SP_ForecastScenario
                               where Scenario.ForecastDraftID == _RecordID && Scenario.Scenario == _Scenario
                               select Scenario.ID).ToList();

                lstSavedValues = (from Data in context.SP_ForecastData
                                  where lstScenario.Contains(Data.ForecastScenarioID)
                                  select Data).ToList();

                for (int i = 1; i <= 18; i++)
                {
                    lstTDaily = new List<SP_ForecastData>();
                    lstTDaily = lstSavedValues.Where(q => q.TDailyID == i).ToList();

                    object TDaily = new
                    {
                        Shortname =
                        Utility.GetTDailyShortName(lstTDaily.ElementAtOrDefault(0).TDailyID,
                            Utility.GetCurrentSeasonForView()),
                        JhelumMangla = lstTDaily.ElementAtOrDefault(0).Volume,
                        ChenabMarala = lstTDaily.ElementAtOrDefault(1).Volume,
                        IndusTarbela = lstTDaily.ElementAtOrDefault(2).Volume,
                        KabulNowshera = lstTDaily.ElementAtOrDefault(3).Volume
                    };

                    lstTDailyValues.Add(TDaily);

                    if (lstTDaily.ElementAtOrDefault(0).TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (lstTDaily.ElementAtOrDefault(0).TDailyID ==
                            (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                        {
                            JMEK += lstTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CMEK += lstTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            ITEK += lstTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KNEK += lstTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;
                        }
                        else
                        {
                            JMEK += lstTDaily.ElementAtOrDefault(0).Volume;
                            CMEK += lstTDaily.ElementAtOrDefault(1).Volume;
                            ITEK += lstTDaily.ElementAtOrDefault(2).Volume;
                            KNEK += lstTDaily.ElementAtOrDefault(3).Volume;
                        }
                    }
                    else if (lstTDaily.ElementAtOrDefault(0).TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                    {
                        if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily
                            || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                        {
                            JM += lstTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CM += lstTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            IT += lstTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KN += lstTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;
                        }
                        else
                        {
                            JM += lstTDaily.ElementAtOrDefault(0).Volume;
                            CM += lstTDaily.ElementAtOrDefault(1).Volume;
                            IT += lstTDaily.ElementAtOrDefault(2).Volume;
                            KN += lstTDaily.ElementAtOrDefault(3).Volume;
                        }

                        if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.SeasonDistribution.LKEnd)
                        {
                            TDaily = new
                            {
                                Shortname = "EK(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JMEK * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CMEK * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (ITEK * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KNEK * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);

                            TDaily = new
                            {
                                Shortname = "LK(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JM * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CM * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (IT * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KN * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);

                            TDaily = new
                            {
                                Shortname = "Total(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", ((JMEK + JM) * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", ((CMEK + CM) * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", ((ITEK + IT) * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", ((KNEK + KN) * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);
                        }
                    }
                    else
                    {
                        if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily
                            || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                            || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily
                            || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                        {
                            JM += lstTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CM += lstTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            IT += lstTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KN += lstTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;
                        }
                        else if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                        {
                            if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                            {
                                JM += lstTDaily.ElementAtOrDefault(0).Volume * Constants.LeapYearTrue;
                                CM += lstTDaily.ElementAtOrDefault(1).Volume * Constants.LeapYearTrue;
                                IT += lstTDaily.ElementAtOrDefault(2).Volume * Constants.LeapYearTrue;
                                KN += lstTDaily.ElementAtOrDefault(3).Volume * Constants.LeapYearTrue;
                            }
                            else
                            {
                                JM += lstTDaily.ElementAtOrDefault(0).Volume * Constants.LeapYearFalse;
                                CM += lstTDaily.ElementAtOrDefault(1).Volume * Constants.LeapYearFalse;
                                IT += lstTDaily.ElementAtOrDefault(2).Volume * Constants.LeapYearFalse;
                                KN += lstTDaily.ElementAtOrDefault(3).Volume * Constants.LeapYearFalse;
                            }
                        }
                        else
                        {
                            JMEK += lstTDaily.ElementAtOrDefault(0).Volume;
                            CMEK += lstTDaily.ElementAtOrDefault(1).Volume;
                            ITEK += lstTDaily.ElementAtOrDefault(2).Volume;
                            KNEK += lstTDaily.ElementAtOrDefault(3).Volume;
                        }

                        if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.SeasonDistribution.RabiEnd)
                        {
                            TDaily = new
                            {
                                Shortname = "Rabi(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JM * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CM * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (IT * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KN * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTDailyValues;
        }

        public bool AllowedToAddMoreDrafts()
        {
            bool Result = true;
            try
            {
                SP_ForecastDraft objDraft = (from Draft in context.SP_ForecastDraft
                                             where
                                             Draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft && Draft.Year == DateTime.Now.Year
                                             select Draft).FirstOrDefault();
                if (objDraft != null)
                    Result = false;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public List<SP_ForecastScenario> GetSavedProbabilities(long _DraftID)
        {
            List<SP_ForecastScenario> lstScenarios = new List<SP_ForecastScenario>();
            try
            {
                lstScenarios = (from Scenario in context.SP_ForecastScenario
                                where Scenario.ForecastDraftID == _DraftID
                                select Scenario).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstScenarios;
        }

        public void DeletePreviousData(long _DraftID)
        {
            try
            {
                List<SP_ForecastScenario> lstScenario = (from Scenario in context.SP_ForecastScenario
                                                         where Scenario.ForecastDraftID == _DraftID
                                                         select Scenario).ToList();

                foreach (var sce in lstScenario)
                    context.SP_ForecastData.RemoveRange(
                        context.SP_ForecastData.Where(q => q.ForecastScenarioID == sce.ID));

                context.SP_ForecastScenario.RemoveRange(
                    context.SP_ForecastScenario.Where(q => q.ForecastDraftID == _DraftID));
                context.SP_ForecastDraft.RemoveRange(context.SP_ForecastDraft.Where(q => q.ID == _DraftID));
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion

        #region Selected InflowForecasting

        public string GetSRMDraftName()
        {
            string Description = "";
            try
            {
                Description = (from Draft in context.SP_ForecastDraft
                               where
                               Draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft && Draft.Year == DateTime.Now.Year
                               select Draft.Description).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Description;
        }

        public List<SP_ForecastScenario> GetStatisticalDraftProbabilities(long _StatisticalID, string _Scenario)
        {
            List<SP_ForecastScenario> lstSavedProbs = new List<SP_ForecastScenario>();
            try
            {
                //long SRMDraftID = ((from Draft in context.SP_ForecastDraft
                //                    where Draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft && Draft.Year == DateTime.Now.Year
                //                    select Draft.ID).FirstOrDefault());

                lstSavedProbs = (from Scenario in context.SP_ForecastScenario
                                 where Scenario.Scenario == _Scenario && Scenario.ForecastDraftID == _StatisticalID
                                 select Scenario).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstSavedProbs;
        }

        public List<SP_ForecastScenario> GetSRMDraftProbabilities(string _Scenario)
        {
            List<SP_ForecastScenario> lstSavedProbs = new List<SP_ForecastScenario>();
            try
            {
                lstSavedProbs = (from Scenario in context.SP_ForecastScenario
                                 where
                                 Scenario.Scenario == _Scenario &&
                                 Scenario.ForecastDraftID == ((from Draft in context.SP_ForecastDraft
                                                               where
                                                               Draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft &&
                                                               Draft.Year == DateTime.Now.Year
                                                               select Draft.ID).FirstOrDefault())
                                 select Scenario).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstSavedProbs;
        }

        public List<object> GetStatisticalAndSRMDraftDetail(long _StatisticalID, string _Scenario)
        {
            #region Variables

            List<SP_ForecastData> lstStatSavedValues = new List<SP_ForecastData>();
            List<SP_ForecastData> lstSRMSavedValues = new List<SP_ForecastData>();
            List<SP_ForecastData> lstStatTDaily = new List<SP_ForecastData>();
            List<SP_ForecastData> lstSRMTDaily = new List<SP_ForecastData>();
            List<object> lstTDailyValues = new List<object>();
            double? JMEK = 0;
            double? CMEK = 0;
            double? ITEK = 0;
            double? KNEK = 0;
            double? JM = 0;
            double? CM = 0;
            double? KN = 0;
            double? IT = 0;

            double? SRMJMEK = 0;
            double? SRMCMEK = 0;
            double? SRMITEK = 0;
            double? SRMKNEK = 0;
            double? SRMJM = 0;
            double? SRMCM = 0;
            double? SRMKN = 0;
            double? SRMIT = 0;

            #endregion

            try
            {
                long SRMID = (from Draft in context.SP_ForecastDraft
                              where
                              Draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft && Draft.Year == DateTime.Now.Year
                              select Draft.ID).FirstOrDefault();

                lstSRMSavedValues = (from Data in context.SP_ForecastData
                                     where ((from Scenario in context.SP_ForecastScenario
                                             where Scenario.ForecastDraftID == SRMID && Scenario.Scenario == _Scenario
                                             select Scenario.ID).ToList())
                                         .Contains(Data.ForecastScenarioID)
                                     select Data).ToList();

                lstStatSavedValues = (from Data in context.SP_ForecastData
                                      where ((from Scenario in context.SP_ForecastScenario
                                              where Scenario.ForecastDraftID == _StatisticalID && Scenario.Scenario == _Scenario
                                              select Scenario.ID).ToList())
                                          .Contains(Data.ForecastScenarioID)
                                      select Data).ToList();

                for (int i = 1; i <= 18; i++)
                {
                    lstStatTDaily = new List<SP_ForecastData>();
                    lstSRMTDaily = new List<SP_ForecastData>();
                    lstStatTDaily = lstStatSavedValues.Where(q => q.TDailyID == i).ToList();
                    lstSRMTDaily = lstSRMSavedValues.Where(q => q.TDailyID == i).ToList();
                    object TDaily = new
                    {
                        Shortname =
                        Utility.GetTDailyShortName(lstStatTDaily.ElementAtOrDefault(0).TDailyID,
                            Utility.GetCurrentSeasonForView()),
                        JhelumMangla = String.Format("{0:0.0}", lstStatTDaily.ElementAtOrDefault(0).Volume),
                        ChenabMarala = String.Format("{0:0.0}", lstStatTDaily.ElementAtOrDefault(1).Volume),
                        IndusTarbela = String.Format("{0:0.0}", lstStatTDaily.ElementAtOrDefault(2).Volume),
                        KabulNowshera = String.Format("{0:0.0}", lstStatTDaily.ElementAtOrDefault(3).Volume),
                        JhelumManglaSRM = lstSRMTDaily.ElementAtOrDefault(0) == null ? "" : String.Format("{0:0.0}", lstSRMTDaily.ElementAtOrDefault(0).Volume),
                        ChenabMaralaSRM = lstSRMTDaily.ElementAtOrDefault(1) == null ? "" : String.Format("{0:0.0}", lstSRMTDaily.ElementAtOrDefault(1).Volume),
                        IndusTarbelaSRM = lstSRMTDaily.ElementAtOrDefault(2) == null ? "" : String.Format("{0:0.0}", lstSRMTDaily.ElementAtOrDefault(2).Volume),
                        KabulNowsheraSRM = lstSRMTDaily.ElementAtOrDefault(3) == null ? "" : String.Format("{0:0.0}", lstSRMTDaily.ElementAtOrDefault(3).Volume)
                    };
                    lstTDailyValues.Add(TDaily);

                    if (lstStatTDaily.ElementAtOrDefault(0).TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (lstStatTDaily.ElementAtOrDefault(0).TDailyID ==
                            (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                        {
                            JMEK += lstStatTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CMEK += lstStatTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            ITEK += lstStatTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KNEK += lstStatTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;

                            SRMJMEK += lstSRMTDaily.ElementAtOrDefault(0) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion);
                            SRMCMEK += lstSRMTDaily.ElementAtOrDefault(1) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion);
                            SRMITEK += lstSRMTDaily.ElementAtOrDefault(2) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion);
                            SRMKNEK += lstSRMTDaily.ElementAtOrDefault(3) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion);
                        }
                        else
                        {
                            JMEK += lstStatTDaily.ElementAtOrDefault(0).Volume;
                            CMEK += lstStatTDaily.ElementAtOrDefault(1).Volume;
                            ITEK += lstStatTDaily.ElementAtOrDefault(2).Volume;
                            KNEK += lstStatTDaily.ElementAtOrDefault(3).Volume;

                            SRMJMEK += lstSRMTDaily.ElementAtOrDefault(0) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(0).Volume;
                            SRMCMEK += lstSRMTDaily.ElementAtOrDefault(1) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(1).Volume;
                            SRMITEK += lstSRMTDaily.ElementAtOrDefault(2) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(2).Volume;
                            SRMKNEK += lstSRMTDaily.ElementAtOrDefault(3) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(3).Volume;
                        }
                    }
                    else if (lstStatTDaily.ElementAtOrDefault(0).TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                    {
                        if (lstStatTDaily.ElementAtOrDefault(0).TDailyID ==
                            (int)Constants.TDAilySpecialCases.JulyTDaily
                            ||
                            lstStatTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                        {
                            JM += lstStatTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CM += lstStatTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            IT += lstStatTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KN += lstStatTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;

                            SRMJM += lstSRMTDaily.ElementAtOrDefault(0) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion);
                            SRMCM += lstSRMTDaily.ElementAtOrDefault(1) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion);
                            SRMIT += lstSRMTDaily.ElementAtOrDefault(2) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion);
                            SRMKN += lstSRMTDaily.ElementAtOrDefault(3) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion);
                        }
                        else
                        {
                            JM += lstStatTDaily.ElementAtOrDefault(0).Volume;
                            CM += lstStatTDaily.ElementAtOrDefault(1).Volume;
                            IT += lstStatTDaily.ElementAtOrDefault(2).Volume;
                            KN += lstStatTDaily.ElementAtOrDefault(3).Volume;

                            SRMJM += lstSRMTDaily.ElementAtOrDefault(0) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(0).Volume;
                            SRMCM += lstSRMTDaily.ElementAtOrDefault(1) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(1).Volume;
                            SRMIT += lstSRMTDaily.ElementAtOrDefault(2) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(2).Volume;
                            SRMKN += lstSRMTDaily.ElementAtOrDefault(3) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(3).Volume;
                        }

                        if (lstStatTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.SeasonDistribution.LKEnd)
                        {
                            TDaily = new
                            {
                                Shortname = "EK(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JMEK * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CMEK * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (ITEK * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KNEK * Constants.MAFConversion)),

                                JhelumManglaSRM = SRMJMEK == 0 ? "" : (String.Format("{0:0.000}", (SRMJMEK * Constants.MAFConversion))),
                                ChenabMaralaSRM = SRMCMEK == 0 ? "" : (String.Format("{0:0.000}", (SRMCMEK * Constants.MAFConversion))),
                                IndusTarbelaSRM = SRMITEK == 0 ? "" : (String.Format("{0:0.000}", (SRMITEK * Constants.MAFConversion))),
                                KabulNowsheraSRM = SRMKNEK == 0 ? "" : (String.Format("{0:0.000}", (SRMKNEK * Constants.MAFConversion)))
                            };
                            lstTDailyValues.Add(TDaily);

                            TDaily = new
                            {
                                Shortname = "LK(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JM * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CM * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (IT * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KN * Constants.MAFConversion)),

                                JhelumManglaSRM = SRMJM == 0 ? "" : String.Format("{0:0.000}", (SRMJM * Constants.MAFConversion)),
                                ChenabMaralaSRM = SRMCM == 0 ? "" : String.Format("{0:0.000}", (SRMCM * Constants.MAFConversion)),
                                IndusTarbelaSRM = SRMIT == 0 ? "" : String.Format("{0:0.000}", (SRMIT * Constants.MAFConversion)),
                                KabulNowsheraSRM = SRMKN == 0 ? "" : String.Format("{0:0.000}", (SRMKN * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);

                            TDaily = new
                            {
                                Shortname = "Total(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", ((JMEK + JM) * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", ((CMEK + CM) * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", ((ITEK + IT) * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", ((KNEK + KN) * Constants.MAFConversion)),

                                JhelumManglaSRM = (SRMJMEK + SRMJM) == 0 ? "" :
                                String.Format("{0:0.000}", ((SRMJMEK + SRMJM) * Constants.MAFConversion)),
                                ChenabMaralaSRM = (SRMCMEK + SRMCM) == 0 ? "" :
                                String.Format("{0:0.000}", ((SRMCMEK + SRMCM) * Constants.MAFConversion)),
                                IndusTarbelaSRM = (SRMITEK + SRMIT) == 0 ? "" :
                                String.Format("{0:0.000}", ((SRMITEK + SRMIT) * Constants.MAFConversion)),
                                KabulNowsheraSRM = (SRMKNEK + SRMKN) == 0 ? "" :
                                String.Format("{0:0.000}", ((SRMKNEK + SRMKN) * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);
                        }
                    }
                    else
                    {
                        if (lstStatTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily
                            ||
                            lstStatTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                            ||
                            lstStatTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily
                            ||
                            lstStatTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                        {
                            JM += lstStatTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                            CM += lstStatTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                            IT += lstStatTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                            KN += lstStatTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;

                            SRMJM += lstSRMTDaily.ElementAtOrDefault(0) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion);
                            SRMCM += lstSRMTDaily.ElementAtOrDefault(1) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion);
                            SRMIT += lstSRMTDaily.ElementAtOrDefault(2) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion);
                            SRMKN += lstSRMTDaily.ElementAtOrDefault(3) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion);
                        }
                        else if (lstStatTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                        {
                            if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                            {
                                JM += lstStatTDaily.ElementAtOrDefault(0).Volume * Constants.LeapYearTrue;
                                CM += lstStatTDaily.ElementAtOrDefault(1).Volume * Constants.LeapYearTrue;
                                IT += lstStatTDaily.ElementAtOrDefault(2).Volume * Constants.LeapYearTrue;
                                KN += lstStatTDaily.ElementAtOrDefault(3).Volume * Constants.LeapYearTrue;

                                SRMJM += lstSRMTDaily.ElementAtOrDefault(0) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(0).Volume * Constants.LeapYearTrue);
                                SRMCM += lstSRMTDaily.ElementAtOrDefault(1) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(1).Volume * Constants.LeapYearTrue);
                                SRMIT += lstSRMTDaily.ElementAtOrDefault(2) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(2).Volume * Constants.LeapYearTrue);
                                SRMKN += lstSRMTDaily.ElementAtOrDefault(3) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(3).Volume * Constants.LeapYearTrue);
                            }
                            else
                            {
                                JM += lstStatTDaily.ElementAtOrDefault(0).Volume * Constants.LeapYearFalse;
                                CM += lstStatTDaily.ElementAtOrDefault(1).Volume * Constants.LeapYearFalse;
                                IT += lstStatTDaily.ElementAtOrDefault(2).Volume * Constants.LeapYearFalse;
                                KN += lstStatTDaily.ElementAtOrDefault(3).Volume * Constants.LeapYearFalse;

                                SRMJM += lstSRMTDaily.ElementAtOrDefault(0) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(0).Volume * Constants.LeapYearFalse);
                                SRMCM += lstSRMTDaily.ElementAtOrDefault(1) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(1).Volume * Constants.LeapYearFalse);
                                SRMIT += lstSRMTDaily.ElementAtOrDefault(2) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(2).Volume * Constants.LeapYearFalse);
                                SRMKN += lstSRMTDaily.ElementAtOrDefault(3) == null ? 0 : (lstSRMTDaily.ElementAtOrDefault(3).Volume * Constants.LeapYearFalse);
                            }
                        }
                        else
                        {
                            JM += lstStatTDaily.ElementAtOrDefault(0).Volume;
                            CM += lstStatTDaily.ElementAtOrDefault(1).Volume;
                            IT += lstStatTDaily.ElementAtOrDefault(2).Volume;
                            KN += lstStatTDaily.ElementAtOrDefault(3).Volume;

                            SRMJM += lstSRMTDaily.ElementAtOrDefault(0) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(0).Volume;
                            SRMCM += lstSRMTDaily.ElementAtOrDefault(1) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(1).Volume;
                            SRMIT += lstSRMTDaily.ElementAtOrDefault(2) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(2).Volume;
                            SRMKN += lstSRMTDaily.ElementAtOrDefault(3) == null ? 0 : lstSRMTDaily.ElementAtOrDefault(3).Volume;
                        }

                        if (lstStatTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.SeasonDistribution.RabiEnd)
                        {
                            TDaily = new
                            {
                                Shortname = "Rabi(MAF)",
                                JhelumMangla = String.Format("{0:0.000}", (JM * Constants.MAFConversion)),
                                ChenabMarala = String.Format("{0:0.000}", (CM * Constants.MAFConversion)),
                                IndusTarbela = String.Format("{0:0.000}", (IT * Constants.MAFConversion)),
                                KabulNowshera = String.Format("{0:0.000}", (KN * Constants.MAFConversion)),

                                JhelumManglaSRM = String.Format("{0:0.000}", (SRMJM * Constants.MAFConversion)),
                                ChenabMaralaSRM = String.Format("{0:0.000}", (SRMCM * Constants.MAFConversion)),
                                IndusTarbelaSRM = String.Format("{0:0.000}", (SRMIT * Constants.MAFConversion)),
                                KabulNowsheraSRM = String.Format("{0:0.000}", (SRMKN * Constants.MAFConversion))
                            };
                            lstTDailyValues.Add(TDaily);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTDailyValues;
        }

        public List<object> GetFinalizedDraft(long? _JMDraftID, long? _CMDraftID, long? _ITDraftID, long? _KNDraftID,
            string _Scenario, bool _CalculateMAF)
        {
            // List<SP_ForecastData> lstSavedValues = new List<SP_ForecastData>();
            List<object> lstTDailyValues = new List<object>();
            // List<SP_ForecastData> lstTDaily = new List<SP_ForecastData>();
            double? JMEK = 0;
            double? CMEK = 0;
            double? ITEK = 0;
            double? KNEK = 0;
            double? JM = 0;
            double? CM = 0;
            double? IT = 0;
            double? KN = 0;

            try
            {
                long SRMDraftID = (from Draft in context.SP_ForecastDraft
                                   where
                                   Draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft && Draft.Year == DateTime.Now.Year
                                   select Draft.ID).FirstOrDefault();

                if (_JMDraftID == null)
                    _JMDraftID = SRMDraftID;

                if (_CMDraftID == null)
                    _CMDraftID = SRMDraftID;

                if (_ITDraftID == null)
                    _ITDraftID = SRMDraftID;

                if (_KNDraftID == null)
                    _KNDraftID = SRMDraftID;

                //lstSavedValues = (from Data in context.SP_ForecastData
                //                  where ((from Scenario in context.SP_ForecastScenario
                //                          where (Scenario.ForecastDraftID == _JMDraftID && Scenario.Scenario == _Scenario && Scenario.StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                //                          || (Scenario.ForecastDraftID == _CMDraftID && Scenario.Scenario == _Scenario && Scenario.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala)
                //                          || (Scenario.ForecastDraftID == _ITDraftID && Scenario.Scenario == _Scenario && Scenario.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela)
                //                          || (Scenario.ForecastDraftID == _KNDraftID && Scenario.Scenario == _Scenario && Scenario.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera)
                //                          select Scenario.ID).ToList())
                //                      .Contains(Data.ForecastScenarioID)
                //                  select Data).ToList();

                List<SP_GetSelectedFinalForecast_Result> lstSavedValues = context.SP_GetSelectedFinalForecast(Convert.ToInt32(_JMDraftID), Convert.ToInt32(_CMDraftID),
                        Convert.ToInt32(_ITDraftID), Convert.ToInt32(_KNDraftID), _Scenario).ToList();


                for (int i = 0; i < 18; i++)
                {
                    //   lstTDaily = new List<SP_ForecastData>();
                    //lstTDaily = lstSavedValues.Where(q => q.TDailyID == i).ToList();

                    object TDaily = new
                    {
                        Shortname =
                        Utility.GetTDailyShortName(lstSavedValues.ElementAtOrDefault(i).TDailyID,
                            Utility.GetCurrentSeasonForView()),
                        TDailyID = lstSavedValues.ElementAtOrDefault(i).TDailyID,
                        JhelumMangla = String.Format("{0:0.0}", lstSavedValues.ElementAtOrDefault(i).jhelum),
                        ChenabMarala = String.Format("{0:0.0}", lstSavedValues.ElementAtOrDefault(i).Chenab),
                        IndusTarbela = String.Format("{0:0.0}", lstSavedValues.ElementAtOrDefault(i).Indus),
                        KabulNowshera = String.Format("{0:0.0}", lstSavedValues.ElementAtOrDefault(i).Kabul)

                    };
                    lstTDailyValues.Add(TDaily);



                    if (_CalculateMAF)
                    {
                        if (lstSavedValues.ElementAtOrDefault(i).TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                        {
                            if (lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            {
                                JMEK += lstSavedValues.ElementAtOrDefault(i).jhelum * Constants.TDailyConversion;
                                CMEK += lstSavedValues.ElementAtOrDefault(i).Chenab * Constants.TDailyConversion;
                                ITEK += lstSavedValues.ElementAtOrDefault(i).Indus * Constants.TDailyConversion;
                                KNEK += lstSavedValues.ElementAtOrDefault(i).Kabul * Constants.TDailyConversion;
                            }
                            else
                            {
                                JMEK += lstSavedValues.ElementAtOrDefault(i).jhelum;
                                CMEK += lstSavedValues.ElementAtOrDefault(i).Chenab;
                                ITEK += lstSavedValues.ElementAtOrDefault(i).Indus;
                                KNEK += lstSavedValues.ElementAtOrDefault(i).Kabul;
                            }
                        }
                        else if (lstSavedValues.ElementAtOrDefault(i).TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                        {
                            if (lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.TDAilySpecialCases.JulyTDaily
                                ||
                                lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.TDAilySpecialCases.AugTDaily)
                            {
                                JM += lstSavedValues.ElementAtOrDefault(i).jhelum * Constants.TDailyConversion;
                                CM += lstSavedValues.ElementAtOrDefault(i).Chenab * Constants.TDailyConversion;
                                IT += lstSavedValues.ElementAtOrDefault(i).Indus * Constants.TDailyConversion;
                                KN += lstSavedValues.ElementAtOrDefault(i).Kabul * Constants.TDailyConversion;
                            }
                            else
                            {
                                JM += lstSavedValues.ElementAtOrDefault(i).jhelum;
                                CM += lstSavedValues.ElementAtOrDefault(i).Chenab;
                                IT += lstSavedValues.ElementAtOrDefault(i).Indus;
                                KN += lstSavedValues.ElementAtOrDefault(i).Kabul;
                            }

                            if (lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.SeasonDistribution.LKEnd)
                            {
                                TDaily = new
                                {
                                    Shortname = "EK(MAF)",
                                    JhelumMangla = String.Format("{0:0.000}", (JMEK * Constants.MAFConversion)),
                                    ChenabMarala = String.Format("{0:0.000}", (CMEK * Constants.MAFConversion)),
                                    IndusTarbela = String.Format("{0:0.000}", (ITEK * Constants.MAFConversion)),
                                    KabulNowshera = String.Format("{0:0.000}", (KNEK * Constants.MAFConversion))
                                };
                                lstTDailyValues.Add(TDaily);

                                TDaily = new
                                {
                                    Shortname = "LK(MAF)",
                                    JhelumMangla = String.Format("{0:0.000}", (JM * Constants.MAFConversion)),
                                    ChenabMarala = String.Format("{0:0.000}", (CM * Constants.MAFConversion)),
                                    IndusTarbela = String.Format("{0:0.000}", (IT * Constants.MAFConversion)),
                                    KabulNowshera = String.Format("{0:0.000}", (KN * Constants.MAFConversion))
                                };
                                lstTDailyValues.Add(TDaily);

                                TDaily = new
                                {
                                    Shortname = "Total(MAF)",
                                    JhelumMangla = String.Format("{0:0.000}", ((JMEK + JM) * Constants.MAFConversion)),
                                    ChenabMarala = String.Format("{0:0.000}", ((CMEK + CM) * Constants.MAFConversion)),
                                    IndusTarbela = String.Format("{0:0.000}", ((ITEK + IT) * Constants.MAFConversion)),
                                    KabulNowshera = String.Format("{0:0.000}", ((KNEK + KN) * Constants.MAFConversion))
                                };
                                lstTDailyValues.Add(TDaily);
                            }
                        }
                        else
                        {
                            if (lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.TDAilySpecialCases.OctTDaily
                                ||
                                lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.TDAilySpecialCases.DecTDaily
                                ||
                                lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.TDAilySpecialCases.JanTDaily
                                ||
                                lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.TDAilySpecialCases.MarTDaily)
                            {
                                JM += lstSavedValues.ElementAtOrDefault(i).jhelum * Constants.TDailyConversion;
                                CM += lstSavedValues.ElementAtOrDefault(i).Chenab * Constants.TDailyConversion;
                                IT += lstSavedValues.ElementAtOrDefault(i).Indus * Constants.TDailyConversion;
                                KN += lstSavedValues.ElementAtOrDefault(i).Kabul * Constants.TDailyConversion;
                            }
                            else if (lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                     (int)Constants.TDAilySpecialCases.FebTDaily)
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                {
                                    JM += lstSavedValues.ElementAtOrDefault(i).jhelum * Constants.LeapYearTrue;
                                    CM += lstSavedValues.ElementAtOrDefault(i).Chenab * Constants.LeapYearTrue;
                                    IT += lstSavedValues.ElementAtOrDefault(i).Indus * Constants.LeapYearTrue;
                                    KN += lstSavedValues.ElementAtOrDefault(i).Kabul * Constants.LeapYearTrue;
                                }
                                else
                                {
                                    JM += lstSavedValues.ElementAtOrDefault(i).jhelum * Constants.LeapYearFalse;
                                    CM += lstSavedValues.ElementAtOrDefault(i).Chenab * Constants.LeapYearFalse;
                                    IT += lstSavedValues.ElementAtOrDefault(i).Indus * Constants.LeapYearFalse;
                                    KN += lstSavedValues.ElementAtOrDefault(i).Kabul * Constants.LeapYearFalse;
                                }
                            }
                            else
                            {
                                JMEK += lstSavedValues.ElementAtOrDefault(i).jhelum;
                                CMEK += lstSavedValues.ElementAtOrDefault(i).Chenab;
                                ITEK += lstSavedValues.ElementAtOrDefault(i).Indus;
                                KNEK += lstSavedValues.ElementAtOrDefault(i).Kabul;
                            }

                            if (lstSavedValues.ElementAtOrDefault(i).TDailyID ==
                                (int)Constants.SeasonDistribution.RabiEnd)
                            {
                                TDaily = new
                                {
                                    Shortname = "Rabi(MAF)",
                                    JhelumMangla = String.Format("{0:0.000}", (JM * Constants.MAFConversion)),
                                    ChenabMarala = String.Format("{0:0.000}", (CM * Constants.MAFConversion)),
                                    IndusTarbela = String.Format("{0:0.000}", (IT * Constants.MAFConversion)),
                                    KabulNowshera = String.Format("{0:0.000}", (KN * Constants.MAFConversion))
                                };
                                lstTDailyValues.Add(TDaily);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTDailyValues;
        }

        public List<SP_ForecastScenario> GetProbabilitiesForFinalizedDraft(long? _JMDraftID, long? _CMDraftID,
            long? _ITDraftID, long? _KNDraftID, string _Scenario)
        {
            List<SP_ForecastScenario> lstSavedProbs = new List<SP_ForecastScenario>();
            try
            {
                long SRMDraftID = (from Draft in context.SP_ForecastDraft
                                   where
                                   Draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft && Draft.Year == DateTime.Now.Year
                                   select Draft.ID).FirstOrDefault();

                if (_JMDraftID == null)
                    _JMDraftID = SRMDraftID;

                if (_CMDraftID == null)
                    _CMDraftID = SRMDraftID;

                if (_ITDraftID == null)
                    _ITDraftID = SRMDraftID;

                if (_KNDraftID == null)
                    _KNDraftID = SRMDraftID;

                lstSavedProbs = (from Scenario in context.SP_ForecastScenario
                                 where
                                 (Scenario.ForecastDraftID == _JMDraftID && Scenario.Scenario == _Scenario &&
                                  Scenario.StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                                 ||
                                 (Scenario.ForecastDraftID == _CMDraftID && Scenario.Scenario == _Scenario &&
                                  Scenario.StationID == (int)Constants.RimStationsIDs.ChenabAtMarala)
                                 ||
                                 (Scenario.ForecastDraftID == _ITDraftID && Scenario.Scenario == _Scenario &&
                                  Scenario.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela)
                                 ||
                                 (Scenario.ForecastDraftID == _KNDraftID && Scenario.Scenario == _Scenario &&
                                  Scenario.StationID == (int)Constants.RimStationsIDs.KabulAtNowshera)
                                 select Scenario).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstSavedProbs;
        }

        public long GetSRMDraftID()
        {
            long SRMDraftID = -1;
            try
            {
                SRMDraftID = (from Draft in context.SP_ForecastDraft
                              where
                              Draft.DraftType == (int)Constants.InflowForecstDrafts.SRMDraft && Draft.Year == DateTime.Now.Year
                              select Draft.ID).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return SRMDraftID;
        }

        public List<SP_ForecastDraft> GetSelectedDraftsInformation()
        {
            List<SP_ForecastDraft> lstDrafts = new List<SP_ForecastDraft>();
            try
            {
                lstDrafts = (from draft in context.SP_ForecastDraft
                             where
                             draft.DraftType == (int)Constants.InflowForecstDrafts.SelectedDraft &&
                             draft.SeasonID == (int)Constants.Seasons.Kharif && draft.Year == DateTime.Now.Year
                             select draft).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstDrafts;
        }

        public string GetDefaultDraftName(int _Season)
        {
            string DraftCount = "";
            try
            {
                int Count = (from Draft in context.SP_ForecastDraft
                             where
                             Draft.Year == DateTime.Now.Year && Draft.SeasonID == _Season &&
                             Draft.DraftType == (int)Constants.InflowForecstDrafts.SelectedDraft
                             select Draft).Count();
                if (Count == 0)
                {
                    DraftCount = "1st ";
                }
                else if (Count == 1)
                {
                    DraftCount = "2nd ";
                }
                else if (Count == 2)
                {
                    DraftCount = "3rd ";
                }
                else if (Count == 3)
                {
                    DraftCount = "4th ";
                }
                else if (Count == 4)
                {
                    DraftCount = "5th ";
                }
                else if (Count >= 5)
                {
                    DraftCount = "NOT ALLOWED";
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return DraftCount;
        }

        #endregion

        #region Balance Reservoir

        public DateTime GetECAPLatestDate()
        {
            DateTime LatestDate = new DateTime();
            try
            {
                LatestDate = ((from ECDate in context.SP_RefElevationCapacity
                               select new
                               {
                                   ECDate.ElevationCapacityDate
                               }).Distinct().OrderByDescending(w => w.ElevationCapacityDate).ToList()
                    .Select(q => new
                    {
                        Year = q.ElevationCapacityDate
                    }).Where(p => p.Year < DateTime.Now).FirstOrDefault().Year);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return LatestDate;

        }

        public Object GetElevationCapacitiesForBalanceReservoir(long _ManglaLevel, long _TarbelaLevel,
            long _TarbelaFillingLimit, long _Chashma, long _ChashmaMinLevel)
        {
            Object ReservoirCapacities = new object();
            try
            {
                object res = ((from ECDate in context.SP_RefElevationCapacity
                               select new
                               {
                                   ECDate.ElevationCapacityDate
                               })
                            .Distinct().
                            OrderByDescending(w => w.ElevationCapacityDate).ToList()
                            .Select(q => new
                            {
                                Year = q.ElevationCapacityDate
                            }).Where(p => p.Year < DateTime.Now).FirstOrDefault().Year);



                List<SP_RefElevationCapacity> lstValues = (from EC in context.SP_RefElevationCapacity
                                                           where EC.ElevationCapacityDate == ((from ECDate in context.SP_RefElevationCapacity
                                                                                               select new
                                                                                               {
                                                                                                   ECDate.ElevationCapacityDate
                                                                                               }).Distinct().OrderByDescending(w => w.ElevationCapacityDate).ToList()
                                                                     .Select(q => new
                                                                     {
                                                                         Year = q.ElevationCapacityDate
                                                                     }).Where(p => p.Year < DateTime.Now).FirstOrDefault().Year)
                                                                 && (EC.Level == _ManglaLevel || EC.Level == _TarbelaLevel
                                                                     || EC.Level == _TarbelaFillingLimit || EC.Level == _Chashma ||
                                                                     EC.Level == _ChashmaMinLevel)
                                                           select EC).ToList();
                ReservoirCapacities = new
                {
                    ManglaStorage = lstValues.Where(q => q.Level == _ManglaLevel).FirstOrDefault().Capacity,
                    TarbelaStorage = lstValues.Where(q => q.Level == _TarbelaLevel).FirstOrDefault().Capacity,
                    TarbelaFillLimitStorage =
                    lstValues.Where(q => q.Level == _TarbelaFillingLimit).FirstOrDefault().Capacity,
                    ChashmaStorage = lstValues.Where(q => q.Level == _Chashma).FirstOrDefault().Capacity,
                    ChashmaMinOptLevelStorage =
                    lstValues.Where(q => q.Level == _ChashmaMinLevel).FirstOrDefault().Capacity
                };
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ReservoirCapacities;
        }

        public String AddInflows(double? _EK, double? _LK)
        {
            double? Total = 0;
            try
            {
                if (_EK != null && _LK != null)
                {
                    Total = _LK + _EK;
                }
                else if (_EK == null)
                    Total = _LK;
                else
                    Total = _EK;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return String.Format("{0:0.000}", (Total * Constants.MAFConversion));
        }

        public string GetInflowsWithPercentage(double? _Inflow, short? _Percentage)
        {
            string RetInflow = "";
            try
            {
                RetInflow = String.Format("{0:0.000}", _Inflow) + " (" + Convert.ToString(_Percentage) + "%)";
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return RetInflow;
        }

        public Object GetInflowsforKharifSeason(long _ForecastDraftID, string _Scenario)
        {
            Object Inflows = new object();
            double? JMEK = 0;
            double? JMLK = 0;
            double? CMEK = 0;
            double? CMLK = 0;
            double? ITEK = 0;
            double? ITLK = 0;
            double? KNEK = 0;
            double? KNLK = 0;
            try
            {
                List<SP_ForecastScenario> lstScenarios = (from Sce in context.SP_ForecastScenario
                                                          where Sce.ForecastDraftID == _ForecastDraftID && Sce.Scenario == _Scenario
                                                          select Sce).ToList();

                List<long> lstScenarioIDs =
                    lstScenarios.Where(q => q.ForecastDraftID == _ForecastDraftID).Select(w => w.ID).ToList<long>();

                List<SP_ForecastData> lstData = (from Data in context.SP_ForecastData
                                                 where lstScenarioIDs.Contains(Data.ForecastScenarioID)
                                                 select Data).ToList();

                List<SP_ForecastData> lstJhelum =
                    lstData.Where(q => q.ForecastScenarioID == lstScenarioIDs.ElementAtOrDefault(0)).ToList();

                foreach (var v in lstJhelum)
                {
                    if (v.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (v.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            JMEK += v.Volume * Constants.TDailyConversion;
                        else
                            JMEK += v.Volume;
                    }
                    else
                    {
                        if (v.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                            v.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            JMLK += v.Volume * Constants.TDailyConversion;
                        else
                            JMLK += v.Volume;
                    }
                }

                List<SP_ForecastData> lstChenab =
                    lstData.Where(q => q.ForecastScenarioID == lstScenarioIDs.ElementAtOrDefault(1)).ToList();

                foreach (var v in lstChenab)
                {
                    if (v.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (v.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            CMEK += v.Volume * Constants.TDailyConversion;
                        else
                            CMEK += v.Volume;
                    }
                    else
                    {
                        if (v.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                            v.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            CMLK += v.Volume * Constants.TDailyConversion;
                        else
                            CMLK += v.Volume;
                    }
                }

                List<SP_ForecastData> lstIndus =
                    lstData.Where(q => q.ForecastScenarioID == lstScenarioIDs.ElementAtOrDefault(2)).ToList();

                foreach (var v in lstIndus)
                {
                    if (v.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (v.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            ITEK += v.Volume * Constants.TDailyConversion;
                        else
                            ITEK += v.Volume;
                    }
                    else
                    {
                        if (v.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                            v.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            ITLK += v.Volume * Constants.TDailyConversion;
                        else
                            ITLK += v.Volume;
                    }
                }

                List<SP_ForecastData> lstKabul =
                    lstData.Where(q => q.ForecastScenarioID == lstScenarioIDs.ElementAtOrDefault(3)).ToList();

                foreach (var v in lstKabul)
                {
                    if (v.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (v.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            KNEK += v.Volume * Constants.TDailyConversion;
                        else
                            KNEK += v.Volume;
                    }
                    else
                    {
                        if (v.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                            v.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            KNLK += v.Volume * Constants.TDailyConversion;
                        else
                            KNLK += v.Volume;
                    }
                }

                Inflows = new
                {
                    JhelumAtManglaEK =
                    GetInflowsWithPercentage(JMEK * Constants.MAFConversion, lstScenarios.ElementAtOrDefault(0).EkPercent),
                    JhelumAtManglaLK =
                    GetInflowsWithPercentage(JMLK * Constants.MAFConversion, lstScenarios.ElementAtOrDefault(0).LkPercent),
                    JhelumAtManglaTotal = AddInflows(JMEK, JMLK),

                    ChenabAtMaralaEK =
                    GetInflowsWithPercentage(CMEK * Constants.MAFConversion, lstScenarios.ElementAtOrDefault(1).EkPercent),
                    ChenabAtMaralaLK =
                    GetInflowsWithPercentage(CMLK * Constants.MAFConversion, lstScenarios.ElementAtOrDefault(1).LkPercent),
                    ChenabAtMaralaTotal = AddInflows(CMEK, CMLK),

                    IndusAtTarbelaEK =
                    GetInflowsWithPercentage(ITEK * Constants.MAFConversion, lstScenarios.ElementAtOrDefault(2).EkPercent),
                    IndusAtTarbelaLK =
                    GetInflowsWithPercentage(ITLK * Constants.MAFConversion, lstScenarios.ElementAtOrDefault(2).LkPercent),
                    IndusAtTarbelaTotal = AddInflows(ITEK, ITLK),

                    KabulAtNowsheraEK =
                    GetInflowsWithPercentage(KNEK * Constants.MAFConversion, lstScenarios.ElementAtOrDefault(3).EkPercent),
                    KabulAtNowsheraLK =
                    GetInflowsWithPercentage(KNLK * Constants.MAFConversion, lstScenarios.ElementAtOrDefault(3).LkPercent),
                    KabulAtNowsheraTotal = AddInflows(KNEK, KNLK)
                };
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Inflows;
        }

        public object GetInitialLevels(double? _JMStorage, double? _ITStorage)
        {
            //List<SP_RefElevationCapacity> lstECAP = new List<SP_RefElevationCapacity>();
            object ECAPLevels = new object();
            try
            {
                ECAPLevels = new
                {
                    JMLevel = GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, _JMStorage),
                    ITLevel = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, _ITStorage)
                };

                //DateTime LatestDate = ((from ECDate in context.SP_RefElevationCapacity
                //                        select new
                //                        {
                //                            ECDate.ElevationCapacityDate
                //                        }).Distinct().OrderByDescending(w => w.ElevationCapacityDate).ToList()
                //    .Select(q => new
                //    {
                //        Year = q.ElevationCapacityDate
                //    }).Where(p => p.Year < DateTime.Now).FirstOrDefault().Year);

                //lstECAP = (from ECAP in context.SP_RefElevationCapacity
                //           where
                //           ECAP.ElevationCapacityDate == (LatestDate) &&
                //           ((ECAP.Capacity == _JMStorage && ECAP.StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                //            || (ECAP.Capacity == _ITStorage && ECAP.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela))
                //           select ECAP).ToList();

                //if (lstECAP.Count() > 0)
                //{
                //    ECAPLevels = new
                //    {
                //        JMLevel =
                //        lstECAP.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                //            .FirstOrDefault()
                //            .Level,
                //        ITLevel =
                //        lstECAP.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela)
                //            .FirstOrDefault()
                //            .Level
                //    };
                //}

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ECAPLevels;
        }

        public object GetParaKPKBALKharifMAF()
        {
            object MAFValues = new object();
            try
            {
                List<SP_GetParaKPKBalochistanMAF_Result> lstMAFValues =
                    context.SP_GetParaKPKBalochistanMAF((int)Constants.Seasons.Kharif)
                        .ToList<SP_GetParaKPKBalochistanMAF_Result>();
                if (lstMAFValues != null)
                {
                    MAFValues = new
                    {
                        ParaEK = String.Format("{0:0.000}", lstMAFValues.ElementAtOrDefault(0).ParaEK),
                        ParaLK = String.Format("{0:0.000}", lstMAFValues.ElementAtOrDefault(1).ParaEK),
                        KPKBALEK = String.Format("{0:0.000}", lstMAFValues.ElementAtOrDefault(0).KPKBALEK),
                        KPKBALLK = String.Format("{0:0.000}", lstMAFValues.ElementAtOrDefault(1).KPKBALEK)
                    };
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return MAFValues;
        }

        public object GetParaForIndus()
        {
            object MAFValues = new object();
            try
            {
                List<double?> lstMAFValues = context.SP_GetParaMAFForIndus((int)Constants.Seasons.Kharif).ToList();
                if (lstMAFValues != null)
                {
                    MAFValues = new
                    {
                        ParaEK = String.Format("{0:0.000}", lstMAFValues.ElementAtOrDefault(0).Value),
                        ParaLK = String.Format("{0:0.000}", lstMAFValues.ElementAtOrDefault(1).Value)
                    };
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return MAFValues;
        }

        public Object GetInflowsforRabiSeason(long _ForecastDraftID, string _Scenario)
        {
            Object Inflows = new object();
            double? JhelumAtMangla = 0;
            double? ChenabAtMarala = 0;
            double? IndusatAtTarbela = 0;
            double? KabulAtNowshera = 0;

            try
            {
                List<SP_ForecastScenario> lstScenarios = (from Sce in context.SP_ForecastScenario
                                                          where Sce.ForecastDraftID == _ForecastDraftID && Sce.Scenario == _Scenario
                                                          select Sce).ToList();

                List<long> lstScenarioIDs =
                    lstScenarios.Where(q => q.ForecastDraftID == _ForecastDraftID).Select(w => w.ID).ToList<long>();

                List<SP_ForecastData> lstData = (from Data in context.SP_ForecastData
                                                 where lstScenarioIDs.Contains(Data.ForecastScenarioID)
                                                 select Data).ToList();

                List<SP_ForecastData> lstJhelum =
                    lstData.Where(q => q.ForecastScenarioID == lstScenarioIDs.ElementAtOrDefault(0)).ToList();

                foreach (var v in lstJhelum)
                {
                    if (v.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                        v.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily
                        || v.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily ||
                        v.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily)
                        JhelumAtMangla += v.Volume * Constants.TDailyConversion;
                    else if (v.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                    {
                        if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                            JhelumAtMangla += v.Volume * Constants.LeapYearTrue;
                        else
                            JhelumAtMangla += v.Volume * Constants.LeapYearFalse;
                    }
                    else
                        JhelumAtMangla += v.Volume;
                }

                List<SP_ForecastData> lstChenab =
                    lstData.Where(q => q.ForecastScenarioID == lstScenarioIDs.ElementAtOrDefault(1)).ToList();

                foreach (var v in lstChenab)
                {
                    if (v.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                        v.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily
                        || v.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily ||
                        v.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily)
                        ChenabAtMarala += v.Volume * Constants.TDailyConversion;
                    else if (v.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                    {
                        if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                            ChenabAtMarala += v.Volume * Constants.LeapYearTrue;
                        else
                            ChenabAtMarala += v.Volume * Constants.LeapYearFalse;
                    }
                    else
                        ChenabAtMarala += v.Volume;
                }

                List<SP_ForecastData> lstIndus =
                    lstData.Where(q => q.ForecastScenarioID == lstScenarioIDs.ElementAtOrDefault(3)).ToList();

                foreach (var v in lstIndus)
                {
                    if (v.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                        v.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily
                        || v.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily ||
                        v.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily)
                        IndusatAtTarbela += v.Volume * Constants.TDailyConversion;
                    else if (v.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                    {
                        if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                            IndusatAtTarbela += v.Volume * Constants.LeapYearTrue;
                        else
                            IndusatAtTarbela += v.Volume * Constants.LeapYearFalse;
                    }
                    else
                        IndusatAtTarbela += v.Volume;
                }

                List<SP_ForecastData> lstKabul =
                    lstData.Where(q => q.ForecastScenarioID == lstScenarioIDs.ElementAtOrDefault(2)).ToList();

                foreach (var v in lstKabul)
                {
                    if (v.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                        v.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily
                        || v.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily ||
                        v.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily)
                        KabulAtNowshera += v.Volume * Constants.TDailyConversion;
                    else if (v.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                    {
                        if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                            KabulAtNowshera += v.Volume * Constants.LeapYearTrue;
                        else
                            KabulAtNowshera += v.Volume * Constants.LeapYearFalse;
                    }
                    else
                        KabulAtNowshera += v.Volume;
                }

                Inflows = new
                {
                    JhelumAtMangla =
                    GetInflowsWithPercentage(JhelumAtMangla * Constants.MAFConversion,
                        lstScenarios.ElementAtOrDefault(0).RabiPercent),

                    ChenabAtMarala =
                    GetInflowsWithPercentage(ChenabAtMarala * Constants.MAFConversion,
                        lstScenarios.ElementAtOrDefault(1).RabiPercent),

                    IndusAtTarbela =
                    GetInflowsWithPercentage(IndusatAtTarbela * Constants.MAFConversion,
                        lstScenarios.ElementAtOrDefault(3).RabiPercent),

                    KabulAtNowshera =
                    GetInflowsWithPercentage(KabulAtNowshera * Constants.MAFConversion,
                        lstScenarios.ElementAtOrDefault(2).RabiPercent)
                };
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Inflows;
        }

        public object GetParaKPKBALRabiMAF()
        {
            object MAFValues = new object();
            try
            {
                SP_GetParaKPKBalochistanMAFRabi_Result objParaKPKBAL =
                    context.SP_GetParaKPKBalochistanMAFRabi((int)Constants.Seasons.Rabi).FirstOrDefault();

                if (objParaKPKBAL != null)
                {
                    MAFValues = new
                    {
                        Para = objParaKPKBAL.Para,
                        ParaIndus = objParaKPKBAL.ParaIndus,
                        KPKBAL = objParaKPKBAL.KPKBAL,
                        Historic = objParaKPKBAL.Historic
                    };
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return MAFValues;
        }

        public string GetSeasonalDraftCountName(int _Season)
        {
            string DraftCount = "";
            try
            {
                int Count = (from Draft in context.SP_PlanDraft
                             where Draft.Year == DateTime.Now.Year && Draft.SeasonID == _Season
                             //&& Draft.IsValid == true
                             select Draft).Count();
                if (Count == 0)
                {
                    DraftCount = "1st ";
                }
                else if (Count == 1)
                {
                    DraftCount = "2nd ";
                }
                else if (Count == 2)
                {
                    DraftCount = "3rd ";
                }
                else if (Count == 3)
                {
                    DraftCount = "4th ";
                }
                else if (Count == 4)
                {
                    DraftCount = "5th ";
                }
                else if (Count >= 5)
                {
                    DraftCount = "Not Allowed";
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return DraftCount;
        }

        public List<object> GetSeasonalDrafts()
        {
            List<dynamic> lstDrafts = null;
            List<dynamic> lstDraftsRet = new List<dynamic>();
            dynamic ObjDraft = null;
            long DraftType = (int)Constants.InflowForecstDrafts.StatisticalDraft;
            long SeasonID = 1;
            short Year = (short)DateTime.Now.Year;
            try
            {
                if ((int)Constants.Seasons.Kharif == Utility.GetCurrentSeasonForView())
                {
                    DraftType = (int)Constants.InflowForecstDrafts.SelectedDraft;
                    SeasonID = 2;
                }
                else if (DateTime.Now.Month < (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch ||
                    (DateTime.Now.Month == (int)Constants.PlanningMonthsAndDays.KharifPlanningMarch && DateTime.Now.Day < (int)Constants.PlanningMonthsAndDays.PlanningDay))
                    Year = (short)(DateTime.Now.Year - 1);


                lstDrafts = (from SPDraft in context.SP_PlanDraft
                             join IFDraft in context.SP_ForecastDraft on SPDraft.ForecastDraftID equals IFDraft.ID
                             where
                             IFDraft.DraftType == DraftType && SPDraft.SeasonID == SeasonID && SPDraft.Year == Year
                             //&& SPDraft.IsValid == true
                             select new
                             {
                                 SPDraftID = SPDraft.ID,
                                 SPDraftDescription = SPDraft.Description,
                                 Approve = SPDraft.IsApproved,
                                 IFDraftID = IFDraft.ID,
                                 IFDraftDescription = IFDraft.Description
                             }).ToList<dynamic>();

                //var vvlstDrafts = (from SPDraft in context.SP_PlanDraft
                //             join IFDraft in context.SP_ForecastDraft on SPDraft.ForecastDraftID equals IFDraft.ID
                //             where IFDraft.DraftType == DraftType && SPDraft.SeasonID == SeasonID && SPDraft.Year == DateTime.Now.Year //&& SPDraft.IsValid == true
                //             select new
                //             {
                //                 SPDraftID = SPDraft.ID,
                //                 SPDraftDescription = SPDraft.Description,
                //                 Approve = SPDraft.IsApproved,
                //                 IFDraftID = IFDraft.ID,
                //                 IFDraftDescription = IFDraft.Description
                //             });



                dynamic ApprovedDraft = lstDrafts.Where(q => q.Approve == true).FirstOrDefault();

                if (ApprovedDraft != null)
                {
                    foreach (var obj in lstDrafts)
                    {
                        if (obj.SPDraftID == ApprovedDraft.SPDraftID)
                        {
                            ObjDraft = new
                            {
                                SPDraftID = obj.SPDraftID,
                                SPDraftDescription = obj.SPDraftDescription,
                                Approve = true,
                                IFDraftID = obj.IFDraftID,
                                IFDraftDescription = obj.IFDraftDescription
                            };
                        }
                        else
                        {
                            ObjDraft = new
                            {
                                SPDraftID = obj.SPDraftID,
                                SPDraftDescription = obj.SPDraftDescription,
                                Approve = false,
                                IFDraftID = obj.IFDraftID,
                                IFDraftDescription = obj.IFDraftDescription
                            };
                        }

                        lstDraftsRet.Add(ObjDraft);
                    }
                }
                else
                {
                    foreach (var obj in lstDrafts)
                    {
                        ObjDraft = new
                        {
                            SPDraftID = obj.SPDraftID,
                            SPDraftDescription = obj.SPDraftDescription,
                            Approve = true,
                            IFDraftID = obj.IFDraftID,
                            IFDraftDescription = obj.IFDraftDescription
                        };
                        lstDraftsRet.Add(ObjDraft);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstDraftsRet;
        }

        public bool DeleteSeasonalIncompleteDraft(long _DraftID, long? _StationID, String _Scenario)
        {
            List<SP_PlanScenario> lstScenario = new List<SP_PlanScenario>();

            try
            {
                lstScenario = (from Scenario in context.SP_PlanScenario
                               where Scenario.PlanDraftID == _DraftID && Scenario.Scenario == _Scenario && Scenario.StationID == _StationID
                               select Scenario).ToList();

                foreach (var sce in lstScenario)
                {
                    context.SP_PlanBalance.RemoveRange(context.SP_PlanBalance.Where(q => q.PlanScenarioID == sce.ID));
                    context.SP_PlanData.RemoveRange(context.SP_PlanData.Where(q => q.PlanScenarioID == sce.ID));
                }

                context.SP_PlanScenario.RemoveRange(context.SP_PlanScenario.Where(q => q.PlanDraftID == _DraftID && q.StationID == _StationID && q.Scenario == _Scenario));
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public object GetEastern(long _SeasonID, long _Years)
        {
            object Eastern = new object();
            try
            {
                Eastern = new
                {
                    EK = context.SPGetEasternComponentDataEKSum((short)_SeasonID, (short)_Years).FirstOrDefault(),
                    LK = context.SPGetEasternComponentDataLKSum((short)_SeasonID, (short)_Years).FirstOrDefault()
                };
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Eastern;
        }

        public string GetEasternRabi(long _SeasonID, long _Years)
        {
            string Eastern = null;
            try
            {
                Eastern = String.Format("{0:0.000}",
                    context.SPGetEasternComponentTDailiesSUM((short?)_SeasonID, (short?)_Years).FirstOrDefault());
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Eastern;
        }

        public SP_PlanBalance GetExistingBalanceData(long _SeasonalDraftID, long _StationID, string _Scenario)
        {
            SP_PlanBalance ObjBalance = new SP_PlanBalance();
            try
            {
                ObjBalance = (from SPSce in context.SP_PlanScenario
                              join SPBal in context.SP_PlanBalance on SPSce.ID equals SPBal.PlanScenarioID
                              where SPSce.StationID == _StationID && SPSce.PlanDraftID == _SeasonalDraftID && SPSce.Scenario == _Scenario
                              select SPBal).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ObjBalance;
        }

        public List<SP_ForecastScenario> GetForecastPercentages(long _ForecastDraftID, String _Scenario)
        {
            List<SP_ForecastScenario> lstScenario = new List<SP_ForecastScenario>();
            try
            {
                lstScenario = (from Scenario in context.SP_ForecastScenario
                               where Scenario.ForecastDraftID == _ForecastDraftID && Scenario.Scenario == _Scenario
                               select Scenario).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstScenario;
        }

        public SP_PlanBalance GetLikelyBalanceJhelum(long _SPDratfID, long _IFDraftID, long _SeasonID)
        {
            SP_PlanBalance LikelyScenario = new SP_PlanBalance();
            try
            {
                SP_PlanBalance MaxScenario = GetExistingBalanceData(_SPDratfID, (int)Constants.RimStationsIDs.JhelumATMangla, "Maximum");
                SP_PlanBalance MinScenario = GetExistingBalanceData(_SPDratfID, (int)Constants.RimStationsIDs.JhelumATMangla, "Minimum");
                LikelyScenario.ID = -1;

                if (MaxScenario != null && MinScenario != null)
                {
                    if (_SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        LikelyScenario.ID = 0;
                        LikelyScenario.Storage = (MaxScenario.Storage + MinScenario.Storage) / 2;
                        LikelyScenario.MCL = GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, LikelyScenario.Storage);
                        LikelyScenario.JhelumEK = (MaxScenario.JhelumEK + MinScenario.JhelumEK) / 2;
                        LikelyScenario.JhelumLK = (MaxScenario.JhelumLK + MinScenario.JhelumLK) / 2;
                        LikelyScenario.Jhelum = LikelyScenario.JhelumEK + LikelyScenario.JhelumLK;
                        LikelyScenario.ChenabEK = (MaxScenario.ChenabEK + MinScenario.ChenabEK) / 2;
                        LikelyScenario.ChenabLK = (MaxScenario.ChenabLK + MinScenario.ChenabLK) / 2;
                        LikelyScenario.Chenab = LikelyScenario.ChenabEK + LikelyScenario.ChenabLK;
                        LikelyScenario.EasternEK = (MaxScenario.EasternEK + MinScenario.EasternEK) / 2;
                        LikelyScenario.EasternLK = (MaxScenario.EasternLK + MinScenario.EasternLK) / 2;
                        LikelyScenario.Eastern = LikelyScenario.EasternEK + LikelyScenario.EasternLK;
                        LikelyScenario.InitStorage = (MaxScenario.InitStorage + MinScenario.InitStorage) / 2;
                        LikelyScenario.InitResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, LikelyScenario.InitStorage);
                        LikelyScenario.StrtofillEK = (MaxScenario.StrtofillEK + MinScenario.StrtofillEK) / 2;
                        LikelyScenario.StrtofillLK = (MaxScenario.StrtofillLK + MinScenario.StrtofillLK) / 2;
                        LikelyScenario.FlowsEK = (MaxScenario.FlowsEK + MinScenario.FlowsEK) / 2;
                        LikelyScenario.FlowsLK = (MaxScenario.FlowsLK + MinScenario.FlowsLK) / 2;
                        LikelyScenario.StorageDep = (MaxScenario.StorageDep + MinScenario.StorageDep) / 2;
                        LikelyScenario.StorageRelease = (MaxScenario.StorageRelease + MaxScenario.StorageRelease) / 2;
                        LikelyScenario.SysInflowsEK = (MaxScenario.SysInflowsEK + MinScenario.SysInflowsEK) / 2;
                        LikelyScenario.SysInflowsLK = (MaxScenario.SysInflowsLK + MinScenario.SysInflowsLK) / 2;
                        LikelyScenario.TotalSysInfow = LikelyScenario.SysInflowsEK + LikelyScenario.SysInflowsLK;
                        LikelyScenario.EarlyKharif = (MaxScenario.EarlyKharif + MinScenario.EarlyKharif) / 2;
                        LikelyScenario.LateKharif = (MaxScenario.LateKharif + MinScenario.LateKharif) / 2;
                        LikelyScenario.TotalAvailabilityEK = (MaxScenario.TotalAvailabilityEK + MinScenario.TotalAvailabilityEK) / 2;
                        LikelyScenario.TotalAvailabilityLK = (MaxScenario.TotalAvailabilityLK + MinScenario.TotalAvailabilityLK) / 2;
                        LikelyScenario.TotalAvailability = (LikelyScenario.TotalAvailabilityEK + LikelyScenario.TotalAvailabilityLK) / 2;
                        LikelyScenario.Para2EK = (MaxScenario.Para2EK + MinScenario.Para2EK) / 2;
                        LikelyScenario.Para2LK = (MaxScenario.Para2LK + MaxScenario.Para2LK) / 2;
                        LikelyScenario.Para2 = LikelyScenario.Para2EK + LikelyScenario.Para2LK;
                        LikelyScenario.CanalAvailabilityEK = (MaxScenario.CanalAvailabilityEK + MinScenario.CanalAvailabilityEK) / 2;
                        LikelyScenario.CanalAvailabilityLK = (MaxScenario.CanalAvailabilityLK + MinScenario.CanalAvailabilityLK) / 2;
                        LikelyScenario.CanalAvailability = LikelyScenario.CanalAvailabilityEK + LikelyScenario.CanalAvailabilityLK;
                        LikelyScenario.JCOutflowEK = (MaxScenario.JCOutflowEK + MinScenario.JCOutflowEK) / 2;
                        LikelyScenario.JCoutflowLK = (MaxScenario.JCoutflowLK + MinScenario.JCoutflowLK) / 2;
                        LikelyScenario.JCOutflow = LikelyScenario.JCOutflowEK + LikelyScenario.JCoutflowLK;
                        LikelyScenario.ShortageEK = (MaxScenario.ShortageEK + MinScenario.ShortageEK) / 2;
                        LikelyScenario.ShortageLK = (MaxScenario.ShortageLK + MinScenario.ShortageLK) / 2;
                        LikelyScenario.Shortage = LikelyScenario.ShortageEK + LikelyScenario.ShortageLK;
                    }
                    else
                    {
                        LikelyScenario.ID = 0;
                        LikelyScenario.Storage = (MaxScenario.Storage + MinScenario.Storage) / 2;
                        LikelyScenario.MCL = GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, LikelyScenario.Storage);
                        LikelyScenario.JhelumRabi = (MaxScenario.JhelumRabi + MinScenario.JhelumRabi) / 2;
                        LikelyScenario.ChenabRabi = (MaxScenario.ChenabRabi + MinScenario.ChenabRabi) / 2;
                        LikelyScenario.Eastern = (MaxScenario.Eastern + MinScenario.Eastern) / 2;
                        LikelyScenario.EasternYears = 5;
                        LikelyScenario.Inflow = (MaxScenario.Inflow + MinScenario.Inflow) / 2;
                        LikelyScenario.InitStorage = (MaxScenario.InitStorage + MinScenario.InitStorage) / 2;
                        LikelyScenario.InitResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, LikelyScenario.InitStorage);
                        LikelyScenario.MaxStorage = (MaxScenario.MaxStorage + MinScenario.MaxStorage) / 2;
                        LikelyScenario.StorageAvailable = (MaxScenario.StorageAvailable + MinScenario.StorageAvailable) / 2;
                        LikelyScenario.StorageDep = (MaxScenario.StorageDep + MinScenario.StorageDep) / 2;
                        LikelyScenario.StorageRelease = (MaxScenario.StorageRelease + MinScenario.StorageRelease) / 2;
                        LikelyScenario.TotalSysInfow = (MaxScenario.TotalSysInfow + MinScenario.TotalSysInfow) / 2;
                        LikelyScenario.SysLossGain = (MaxScenario.SysLossGain + MinScenario.SysLossGain) / 2;
                        LikelyScenario.SysLossGainVol = (MaxScenario.SysLossGainVol + MinScenario.SysLossGainVol) / 2;
                        LikelyScenario.TotalAvailability = (MaxScenario.TotalAvailability + MinScenario.TotalAvailability) / 2;
                        LikelyScenario.Para2 = (MaxScenario.Para2 + MinScenario.Para2) / 2;
                        LikelyScenario.CanalAvailability = (MaxScenario.CanalAvailability + MinScenario.CanalAvailability) / 2;
                        LikelyScenario.JCOutflow = (MaxScenario.JCOutflow + MinScenario.JCOutflow) / 2;
                        LikelyScenario.Shortage = (MaxScenario.Shortage + MinScenario.Shortage) / 2;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return LikelyScenario;
        }

        public SP_PlanBalance GetLikelyBalanceIndus(long _SPDratfID, long _IFDraftID, long _SeasonID)
        {
            SP_PlanBalance LikelyScenario = new SP_PlanBalance();
            try
            {
                SP_PlanBalance MaxScenario = GetExistingBalanceData(_SPDratfID, (int)Constants.RimStationsIDs.IndusAtTarbela, "Maximum");
                SP_PlanBalance MinScenario = GetExistingBalanceData(_SPDratfID, (int)Constants.RimStationsIDs.IndusAtTarbela, "Minimum");
                LikelyScenario.ID = -1;

                if (MaxScenario != null && MinScenario != null)
                {
                    if (_SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        LikelyScenario.ID = 0;
                        LikelyScenario.Storage = (MaxScenario.Storage + MinScenario.Storage) / 2;
                        LikelyScenario.MCL = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, LikelyScenario.Storage);
                        LikelyScenario.FillingLimitStorage = (MaxScenario.FillingLimitStorage + MinScenario.FillingLimitStorage) / 2;
                        LikelyScenario.FillingLimit = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, LikelyScenario.FillingLimitStorage);
                        LikelyScenario.ChashmaStorage = (MaxScenario.ChashmaStorage + MinScenario.ChashmaStorage) / 2;
                        LikelyScenario.ChashmaResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.Chashma, LikelyScenario.ChashmaStorage);
                        LikelyScenario.ChashmaMinStorage = (MaxScenario.ChashmaMinStorage + MinScenario.ChashmaMinStorage) / 2;
                        LikelyScenario.ChashmaMinReslevel = GetReservoirLevel((int)Constants.RimStationsIDs.Chashma, LikelyScenario.ChashmaMinStorage);
                        LikelyScenario.IndusEK = (MaxScenario.IndusEK + MinScenario.IndusEK) / 2;
                        LikelyScenario.IndusLK = (MaxScenario.IndusLK + MinScenario.IndusLK) / 2;
                        LikelyScenario.Indus = LikelyScenario.IndusEK + LikelyScenario.IndusLK;
                        LikelyScenario.KabulEK = (MaxScenario.KabulEK + MinScenario.KabulEK) / 2;
                        LikelyScenario.KabulLK = (MaxScenario.KabulLK + MinScenario.KabulLK) / 2;
                        LikelyScenario.Kabul = LikelyScenario.KabulEK + LikelyScenario.KabulLK;
                        LikelyScenario.InitStorage = (MaxScenario.InitStorage + MinScenario.InitStorage) / 2;
                        LikelyScenario.InitResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, LikelyScenario.InitStorage);
                        LikelyScenario.StrtofillEK = (MaxScenario.StrtofillEK + MinScenario.StrtofillEK) / 2;
                        LikelyScenario.StrtofillLK = (MaxScenario.StrtofillLK + MinScenario.StrtofillLK) / 2;
                        LikelyScenario.FlowsEK = (MaxScenario.FlowsEK + MinScenario.FlowsEK) / 2;
                        LikelyScenario.FlowsLK = (MaxScenario.FlowsLK + MinScenario.FlowsLK) / 2;
                        LikelyScenario.StorageDep = (MaxScenario.StorageDep + MinScenario.StorageDep) / 2;
                        LikelyScenario.StorageRelease = (MaxScenario.StorageRelease + MaxScenario.StorageRelease) / 2;
                        LikelyScenario.SysInflowsEK = (MaxScenario.SysInflowsEK + MinScenario.SysInflowsEK) / 2;
                        LikelyScenario.SysInflowsLK = (MaxScenario.SysInflowsLK + MinScenario.SysInflowsLK) / 2;
                        LikelyScenario.TotalSysInfow = LikelyScenario.SysInflowsEK + LikelyScenario.SysInflowsLK;
                        LikelyScenario.EarlyKharif = (MaxScenario.EarlyKharif + MinScenario.EarlyKharif) / 2;
                        LikelyScenario.LateKharif = (MaxScenario.LateKharif + MinScenario.LateKharif) / 2;
                        LikelyScenario.TotalAvailabilityEK = (MaxScenario.TotalAvailabilityEK + MinScenario.TotalAvailabilityEK) / 2;
                        LikelyScenario.TotalAvailabilityLK = (MaxScenario.TotalAvailabilityLK + MinScenario.TotalAvailabilityLK) / 2;
                        LikelyScenario.TotalAvailability = (LikelyScenario.TotalAvailabilityEK + LikelyScenario.TotalAvailabilityLK) / 2;
                        LikelyScenario.KPKBalochistanEK = (MaxScenario.KPKBalochistanEK + MinScenario.KPKBalochistanEK) / 2;
                        LikelyScenario.KPKBalochistanLK = (MaxScenario.KPKBalochistanLK + MinScenario.KPKBalochistanLK) / 2;
                        LikelyScenario.KPKBalochistan = LikelyScenario.KPKBalochistanEK + LikelyScenario.KPKBalochistanLK;
                        LikelyScenario.BalPunSindhEK = (MaxScenario.BalPunSindhEK + MinScenario.BalPunSindhEK) / 2;
                        LikelyScenario.BalPunsindhLK = (MaxScenario.BalPunsindhLK + MinScenario.BalPunsindhLK) / 2;
                        LikelyScenario.BalPunSindh = LikelyScenario.BalPunSindhEK + LikelyScenario.BalPunsindhLK;
                        LikelyScenario.Para2EK = (MaxScenario.Para2EK + MinScenario.Para2EK) / 2;
                        LikelyScenario.Para2LK = (MaxScenario.Para2LK + MaxScenario.Para2LK) / 2;
                        LikelyScenario.Para2 = LikelyScenario.Para2EK + LikelyScenario.Para2LK;
                        LikelyScenario.CanalAvailabilityEK = (MaxScenario.CanalAvailabilityEK + MinScenario.CanalAvailabilityEK) / 2;
                        LikelyScenario.CanalAvailabilityLK = (MaxScenario.CanalAvailabilityLK + MinScenario.CanalAvailabilityLK) / 2;
                        LikelyScenario.CanalAvailability = LikelyScenario.CanalAvailabilityEK + LikelyScenario.CanalAvailabilityLK;
                        LikelyScenario.JCOutflowEK = (MaxScenario.JCOutflowEK + MinScenario.JCOutflowEK) / 2;
                        LikelyScenario.JCoutflowLK = (MaxScenario.JCoutflowLK + MinScenario.JCoutflowLK) / 2;
                        LikelyScenario.JCOutflow = LikelyScenario.JCOutflowEK + LikelyScenario.JCoutflowLK;
                        LikelyScenario.KotriEK = (MaxScenario.KotriEK + MinScenario.KotriEK) / 2;
                        LikelyScenario.KotriLK = (MaxScenario.KotriLK + MinScenario.KotriLK) / 2;
                        LikelyScenario.Kotri = LikelyScenario.KotriEK + LikelyScenario.KotriLK;
                        LikelyScenario.ShortageEK = (MaxScenario.ShortageEK + MinScenario.ShortageEK) / 2;
                        LikelyScenario.ShortageLK = (MaxScenario.ShortageLK + MinScenario.ShortageLK) / 2;
                        LikelyScenario.Shortage = LikelyScenario.ShortageEK + LikelyScenario.ShortageLK;
                    }
                    else
                    {
                        LikelyScenario.ID = 0;
                        LikelyScenario.Storage = (MaxScenario.Storage + MinScenario.Storage) / 2;
                        LikelyScenario.MCL = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, LikelyScenario.Storage);
                        LikelyScenario.FillingLimitStorage = (MaxScenario.FillingLimitStorage + MinScenario.FillingLimitStorage) / 2;
                        LikelyScenario.FillingLimit = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, LikelyScenario.FillingLimitStorage);
                        LikelyScenario.ChashmaStorage = (MaxScenario.ChashmaStorage + MinScenario.ChashmaStorage) / 2;
                        LikelyScenario.ChashmaResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.Chashma, LikelyScenario.ChashmaStorage);
                        LikelyScenario.ChashmaMinStorage = (MaxScenario.ChashmaMinStorage + MinScenario.ChashmaMinStorage) / 2;
                        LikelyScenario.ChashmaMinReslevel = GetReservoirLevel((int)Constants.RimStationsIDs.Chashma, LikelyScenario.ChashmaMinStorage);
                        LikelyScenario.IndusRabi = (MaxScenario.IndusRabi + MinScenario.IndusRabi) / 2;
                        LikelyScenario.KabulRabi = (MaxScenario.KabulRabi + MinScenario.KabulRabi) / 2;
                        LikelyScenario.Inflow = (MaxScenario.Inflow + MinScenario.Inflow) / 2;
                        LikelyScenario.InitStorage = (MaxScenario.InitStorage + MinScenario.InitStorage) / 2;
                        LikelyScenario.InitResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, LikelyScenario.InitStorage);
                        LikelyScenario.MaxStorage = (MaxScenario.MaxStorage + MinScenario.MaxStorage) / 2;
                        LikelyScenario.StorageAvailable = (MaxScenario.StorageAvailable + MinScenario.StorageAvailable) / 2;
                        LikelyScenario.StorageDep = (MaxScenario.StorageDep + MinScenario.StorageDep) / 2;
                        LikelyScenario.StorageRelease = (MaxScenario.StorageRelease + MinScenario.StorageRelease) / 2;
                        LikelyScenario.TotalSysInfow = (MaxScenario.TotalSysInfow + MinScenario.TotalSysInfow) / 2;
                        LikelyScenario.SysLossGain = (MaxScenario.SysLossGain + MinScenario.SysLossGain) / 2;
                        LikelyScenario.SysLossGainVol = (MaxScenario.SysLossGainVol + MinScenario.SysLossGainVol) / 2;
                        LikelyScenario.TotalAvailability = (MaxScenario.TotalAvailability + MinScenario.TotalAvailability) / 2;
                        LikelyScenario.KPKBalochistan = (MaxScenario.KPKBalochistan + MinScenario.KPKBalochistan) / 2;
                        LikelyScenario.BalPunSindh = (MaxScenario.BalPunSindh + MinScenario.BalPunSindh) / 2;
                        LikelyScenario.Para2 = (MaxScenario.Para2 + MinScenario.Para2) / 2;
                        LikelyScenario.CanalAvailability = (MaxScenario.CanalAvailability + MinScenario.CanalAvailability) / 2;
                        LikelyScenario.Kotri = (MaxScenario.Kotri + MinScenario.Kotri) / 2;
                        LikelyScenario.JCOutflow = (MaxScenario.JCOutflow + MinScenario.JCOutflow) / 2;
                        LikelyScenario.Historic = (MaxScenario.Historic + MinScenario.Historic) / 2;
                        LikelyScenario.Shortage = (MaxScenario.Shortage + MinScenario.Shortage) / 2;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return LikelyScenario;
        }

        #endregion

        #region Plan Jhelum Chenab Command

        public List<SP_RefTDailyCalendar> GetTDailyIDsWithNames(long _SeasonID)
        {
            List<SP_RefTDailyCalendar> lstTDailyIDs = new List<SP_RefTDailyCalendar>();
            try
            {
                if (_SeasonID == (int)Constants.Seasons.Rabi)
                {
                    lstTDailyIDs = (from Cal in context.SP_RefTDailyCalendar
                                    where Cal.SeasonID == _SeasonID
                                          && (Cal.Year == DateTime.Now.Year && Cal.TDailyID < 28)
                                          || (Cal.Year == DateTime.Now.Year + 1 && Cal.TDailyID > 27)
                                    select Cal).Distinct().OrderBy(q => q.TDailyID).ToList();
                }
                else
                {
                    lstTDailyIDs = (from Cal in context.SP_RefTDailyCalendar
                                    where Cal.SeasonID == _SeasonID && Cal.Year == DateTime.Now.Year
                                    select Cal).Distinct().OrderBy(q => q.TDailyID).ToList();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTDailyIDs;
        }

        public List<SP_ForecastData> GetForecastVolume(long _ForecastDraftID, string _Scenario, long _StationID)
        {
            List<SP_ForecastData> lstData = new List<SP_ForecastData>();
            try
            {
                lstData = (from FC in context.SP_ForecastScenario
                           join FD in context.SP_ForecastData on FC.ID equals FD.ForecastScenarioID
                           where
                           FC.ForecastDraftID == _ForecastDraftID && FC.Scenario == _Scenario && FC.StationID == _StationID
                           select FD).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstData;
        }

        public SP_PlanBalance GetBalanceReservoirData(long _SPDraftID, long _RimstationID, string _Scenario)
        {
            SP_PlanBalance ObjBalance = new SP_PlanBalance();
            try
            {
                ObjBalance = (from Scenarion in context.SP_PlanScenario
                              join Balance in context.SP_PlanBalance on Scenarion.ID equals Balance.PlanScenarioID
                              where Scenarion.PlanDraftID == _SPDraftID && Scenarion.StationID == _RimstationID && Scenarion.Scenario == _Scenario
                              select Balance).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ObjBalance;
        }

        public List<SP_RefFillingFraction> GetFillingFraction(long _SeasonID, long _StationID)
        {
            List<SP_RefFillingFraction> lstFillingFraction = new List<SP_RefFillingFraction>();
            try
            {
                lstFillingFraction = (from FF in context.SP_RefFillingFraction
                                      where FF.SeasonID == _SeasonID && FF.StationID == _StationID
                                      select FF).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstFillingFraction;
        }

        public double? GetLiveContent(SP_PlanBalance _ObjBalance, List<SP_RefFillingFraction> _LstFillingFraction,
            short? _TDAilyID, double? _PreLiveContent, string _Scenario, long _SeasonID)
        {
            double? LiveContent = -1;
            double? FF = -1;
            try
            {
                if (_Scenario == "Maximum")
                    FF = _LstFillingFraction.Where(q => q.TDailyID == _TDAilyID).FirstOrDefault().MaxFill;
                else if (_Scenario == "Minimum")
                    FF = _LstFillingFraction.Where(q => q.TDailyID == _TDAilyID).FirstOrDefault().MinFill;
                else
                    FF = _LstFillingFraction.Where(q => q.TDailyID == _TDAilyID).FirstOrDefault().LikelyFill;
                FF = FF / 100;

                if (_SeasonID == (int)Constants.Seasons.Kharif)
                {
                    if (_TDAilyID > 15)
                        LiveContent = _PreLiveContent - ((_ObjBalance.StorageDep / 100) * _ObjBalance.Storage * FF);
                    else
                    {
                        if (_TDAilyID < 8)
                            LiveContent = (FF * _ObjBalance.FlowsEK) + _PreLiveContent;
                        else
                            LiveContent = (FF * _ObjBalance.FlowsLK) + _PreLiveContent;
                    }
                }
                else
                    LiveContent = _PreLiveContent - (_ObjBalance.StorageRelease * FF);

                if (LiveContent < 0)
                    LiveContent = 0.000;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return LiveContent;
        }

        public double GetReservoirLevel(long _StationID, double? _Capacity)
        {
            List<SP_RefElevationCapacity> lstValues = new List<SP_RefElevationCapacity>();
            double ReservoirLevel = 0;
            try
            {
                //DateTime date = ((from ECDate in context.SP_RefElevationCapacity
                //                  select new
                //                  {
                //                      ECDate.ElevationCapacityDate
                //                  }).Distinct().OrderByDescending(w => w.ElevationCapacityDate).ToList()
                //    .Select(q => new
                //    {
                //        Year = q.ElevationCapacityDate
                //    }).Where(p => p.Year < DateTime.Now).FirstOrDefault().Year);


                lstValues = (from EC in context.SP_RefElevationCapacity
                             where EC.ElevationCapacityDate == ((from ECDate in context.SP_RefElevationCapacity
                                                                 select new
                                                                 {
                                                                     ECDate.ElevationCapacityDate
                                                                 }).Distinct().OrderByDescending(w => w.ElevationCapacityDate).ToList()
                                       .Select(q => new
                                       {
                                           Year = q.ElevationCapacityDate
                                       }).Where(p => p.Year < DateTime.Now).FirstOrDefault().Year)

                                   && EC.StationID == _StationID
                             select EC).ToList();

                if (lstValues != null && lstValues.Count() > 0)
                {
                    SP_RefElevationCapacity ExactValue = lstValues.Where(p => p.Capacity == _Capacity).LastOrDefault();
                    if (ExactValue == null)
                    {
                        SP_RefElevationCapacity LessValue =
                            lstValues.TakeWhile(p => p.Capacity < _Capacity).LastOrDefault();
                        SP_RefElevationCapacity GreaterValue =
                            lstValues.Where(p => p.Capacity > LessValue.Capacity).FirstOrDefault();

                        double y1 = 0, y2 = 0, x1 = 0, x2 = 0;

                        if (GreaterValue != null && LessValue != null)
                        {
                            y1 = LessValue.Level.GetValueOrDefault(0);
                            x1 = LessValue.Capacity;

                            y2 = GreaterValue.Level.GetValueOrDefault(0);
                            x2 = GreaterValue.Capacity;

                            ReservoirLevel = y1 + ((((double)_Capacity - x1) / (x2 - x1)) * (y2 - y1));
                        }
                    }
                    else
                        ReservoirLevel = (double)ExactValue.Level;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ReservoirLevel;
        }

        public double? CalculteOutflow(double? _Inflow, long _NoOfDays, double? _CurrLiveContent,
            double? _PreLiveContent)
        {
            double? Outflow = -1;
            try
            {
                Outflow = _Inflow * 0.001983471 * (double)_NoOfDays; //MAF conversion
                Outflow = Outflow - (_CurrLiveContent - _PreLiveContent);
                Outflow = Outflow / (0.001983471 * (double)_NoOfDays); //back to 000cfs
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Outflow;
        }

        public List<SP_RefWaterDistribution> GetListWaterDistribution()
        {
            List<SP_RefWaterDistribution> lstWaterDist = new List<SP_RefWaterDistribution>();
            try
            {
                lstWaterDist = (from WD in context.SP_RefWaterDistribution
                                select WD).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstWaterDist;
        }

        public double? GetProposedCanalWDLSKharif(short? _TDailyID, double? _BalanceInflows, SP_PlanBalance _ObjBalance,
            List<SP_RefWaterDistribution> _lstWaterDist)
        {
            double? Shortage = null;
            double? WaterDist = null;
            SP_RefWaterDistribution ObjShareDist = new SP_RefWaterDistribution();
            try
            {
                if (_TDailyID < 8)
                    Shortage = Math.Round((double)((1 - (_ObjBalance.CanalAvailabilityEK / _ObjBalance.Para2EK)) * 100), 3); //Shortage = _ObjBalance.ShortageEK;
                else
                    Shortage = Math.Round((double)((1 - (_ObjBalance.CanalAvailabilityLK / _ObjBalance.Para2LK)) * 100), 3);//Shortage = _ObjBalance.ShortageLK;

                if (Shortage == 0)
                    WaterDist =
                        _lstWaterDist.Where(q => q.TDailyID == _TDailyID)
                            .FirstOrDefault()
                            .Percentile0.GetValueOrDefault(0);
                else if (Shortage == 5)
                    WaterDist =
                        _lstWaterDist.Where(q => q.TDailyID == _TDailyID)
                            .FirstOrDefault()
                            .Percentile5.GetValueOrDefault(0);
                else if (Shortage == 10)
                    WaterDist =
                        _lstWaterDist.Where(q => q.TDailyID == _TDailyID)
                            .FirstOrDefault()
                            .Percentile10.GetValueOrDefault(0);
                else if (Shortage == 15)
                    WaterDist =
                        _lstWaterDist.Where(q => q.TDailyID == _TDailyID)
                            .FirstOrDefault()
                            .Percentile15.GetValueOrDefault(0);
                else if (Shortage == 20)
                    WaterDist =
                        _lstWaterDist.Where(q => q.TDailyID == _TDailyID)
                            .FirstOrDefault()
                            .Percentile20.GetValueOrDefault(0);
                else if (Shortage == 25)
                    WaterDist =
                        _lstWaterDist.Where(q => q.TDailyID == _TDailyID)
                            .FirstOrDefault()
                            .Percentile25.GetValueOrDefault(0);
                else if (Shortage == 30)
                    WaterDist =
                        _lstWaterDist.Where(q => q.TDailyID == _TDailyID)
                            .FirstOrDefault()
                            .Percentile30.GetValueOrDefault(0);

                if (WaterDist == null) // apply IP Formula
                {
                    double y1 = 0, y2 = 0, x1 = 0, x2 = 0;

                    if (Shortage > 0 && Shortage < 5)
                    {
                        ObjShareDist = _lstWaterDist.Where(q => q.TDailyID == _TDailyID).FirstOrDefault();

                        y1 = ObjShareDist.Percentile0.GetValueOrDefault(0);
                        x1 = (int)Constants.WaterDistributionPercentiles.ZeroPercent;
                        y2 = ObjShareDist.Percentile5.GetValueOrDefault(0);
                        x2 = (int)Constants.WaterDistributionPercentiles.FivePercent;
                        WaterDist = (y1 + (((Shortage - x1) / (x2 - x1)) * (y2 - y1)));
                    }
                    else if (Shortage > 5 && Shortage < 10)
                    {
                        ObjShareDist = _lstWaterDist.Where(q => q.TDailyID == _TDailyID).FirstOrDefault();

                        y1 = ObjShareDist.Percentile5.GetValueOrDefault(0);
                        x1 = (int)Constants.WaterDistributionPercentiles.FivePercent;
                        y2 = ObjShareDist.Percentile10.GetValueOrDefault(0);
                        x2 = (int)Constants.WaterDistributionPercentiles.TenPercent;
                        WaterDist = (y1 + (((Shortage - x1) / (x2 - x1)) * (y2 - y1)));
                    }
                    else if (Shortage > 10 && Shortage < 15)
                    {
                        ObjShareDist = _lstWaterDist.Where(q => q.TDailyID == _TDailyID).FirstOrDefault();

                        y1 = ObjShareDist.Percentile10.GetValueOrDefault(0);
                        x1 = (int)Constants.WaterDistributionPercentiles.TenPercent;
                        y2 = ObjShareDist.Percentile15.GetValueOrDefault(0);
                        x2 = (int)Constants.WaterDistributionPercentiles.FifteenPercent;
                        WaterDist = (y1 + (((Shortage - x1) / (x2 - x1)) * (y2 - y1)));
                    }
                    else if (Shortage > 15 && Shortage < 20)
                    {
                        ObjShareDist = _lstWaterDist.Where(q => q.TDailyID == _TDailyID).FirstOrDefault();

                        y1 = ObjShareDist.Percentile15.GetValueOrDefault(0);
                        x1 = (int)Constants.WaterDistributionPercentiles.FifteenPercent;
                        y2 = ObjShareDist.Percentile20.GetValueOrDefault(0);
                        x2 = (int)Constants.WaterDistributionPercentiles.TwentyPercent;
                        WaterDist = (y1 + (((Shortage - x1) / (x2 - x1)) * (y2 - y1)));
                    }
                    else if (Shortage > 20 && Shortage < 25)
                    {
                        ObjShareDist = _lstWaterDist.Where(q => q.TDailyID == _TDailyID).FirstOrDefault();

                        y1 = ObjShareDist.Percentile20.GetValueOrDefault(0);
                        x1 = (int)Constants.WaterDistributionPercentiles.TwentyPercent;
                        y2 = ObjShareDist.Percentile25.GetValueOrDefault(0);
                        x2 = (int)Constants.WaterDistributionPercentiles.TwentyFivePercent;
                        WaterDist = (y1 + (((Shortage - x1) / (x2 - x1)) * (y2 - y1)));
                    }
                    else if (Shortage > 25 && Shortage < 30)
                    {
                        ObjShareDist = _lstWaterDist.Where(q => q.TDailyID == _TDailyID).FirstOrDefault();

                        y1 = ObjShareDist.Percentile25.GetValueOrDefault(0);
                        x1 = (int)Constants.WaterDistributionPercentiles.TwentyFivePercent;
                        y2 = ObjShareDist.Percentile30.GetValueOrDefault(0);
                        x2 = (int)Constants.WaterDistributionPercentiles.ThirtyPercent;
                        WaterDist = (y1 + (((Shortage - x1) / (x2 - x1)) * (y2 - y1)));
                    }
                }

                if (_BalanceInflows < WaterDist)
                    WaterDist = _BalanceInflows;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return WaterDist;
        }

        public double? GetProposedCanalWDLSRabi(short? _TDailyID, List<SP_RefShareDistribution> _lstFlow7782,
            SP_PlanBalance _ObjBalance, double? _BalanceInflows)
        {
            SP_RefShareDistribution ObjFlow = new SP_RefShareDistribution();
            double? CanalWDL = null;
            try
            {
                ObjFlow = _lstFlow7782.Where(q => q.TDailyID == _TDailyID).FirstOrDefault();
                CanalWDL = 1 - (_ObjBalance.Shortage / 100);
                CanalWDL = CanalWDL * ObjFlow.Jc7782;

                if (_BalanceInflows < CanalWDL)
                    CanalWDL = _BalanceInflows;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return CanalWDL;
        }

        public List<SP_RefShareDistribution> GetFlows7782(long _SeasonID)
        {
            List<SP_RefShareDistribution> lstFlows = new List<SP_RefShareDistribution>();
            try
            {
                lstFlows = (from SD in context.SP_RefShareDistribution
                            where SD.SeasonID == _SeasonID
                            select SD).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstFlows;
        }

        public List<SPGetEasternComponentTDailies_Result> GetEasternhistoricTDailies(short? _SeasonID, short? _Years)
        {
            List<SPGetEasternComponentTDailies_Result> lstEasternTDailies =
                new List<SPGetEasternComponentTDailies_Result>();
            try
            {
                if (_Years == null)
                    _Years = 5;
                lstEasternTDailies = context.SPGetEasternComponentTDailies(_SeasonID, _Years).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstEasternTDailies;
        }

        public double? GetEasternHistoricTDailiesSUM(short? _SeasonID, short? _Years)
        {
            double? EasternSum = null;
            try
            {
                if (_Years == null)
                    _Years = 5;
                EasternSum = context.SPGetEasternComponentTDailiesSUM(_SeasonID, _Years).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return EasternSum;
        }

        public double? GetEasternTDailies(short? _TDailyID,
            List<SPGetEasternComponentTDailies_Result> _lstEasternTDailies, String _EasternEKSum, String _EasternLKSum,
            SP_PlanBalance _ObjBalance)
        {
            double? Factor = null;
            double? TDailyValue = null;
            try
            {
                if (_TDailyID < 8)
                    Factor = _ObjBalance.EasternEK / Convert.ToDouble(_EasternEKSum);
                else if (_TDailyID > 7 && _TDailyID < 19)
                    Factor = _ObjBalance.EasternLK / Convert.ToDouble(_EasternLKSum);
                else
                    Factor = _ObjBalance.Eastern / Convert.ToDouble(_EasternLKSum);

                TDailyValue = _lstEasternTDailies.Where(q => q.Period == _TDailyID).FirstOrDefault().EASTERN * Factor;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return TDailyValue;
        }

        public List<object> CalculateJhelumchenabPlan(long _ForecastDraftID, long _SeasonalDraftID, string _Scenario, bool _CalculateMAF)
        {
            #region Declerations

            SP_PlanData JCData = new SP_PlanData();
            List<SP_PlanData> lstJCData = new List<SP_PlanData>();
            object Data = new object();
            List<object> lstData = new List<object>();
            List<SP_RefTDailyCalendar> lstTDailyIDs = new List<SP_RefTDailyCalendar>();
            List<SP_ForecastData> lstJhelumForecastData = new List<SP_ForecastData>();
            List<SP_ForecastData> lstChenabForecastData = new List<SP_ForecastData>();
            SP_PlanBalance ObjBalance = new SP_PlanBalance();
            List<SP_RefFillingFraction> lstFillingFraction = new List<SP_RefFillingFraction>();
            List<SP_RefWaterDistribution> lstWaterDist = new List<SP_RefWaterDistribution>();
            List<SP_RefShareDistribution> lstFlow7782 = new List<SP_RefShareDistribution>();
            List<SPGetEasternComponentTDailies_Result> lstEasternTDailies = new List<SPGetEasternComponentTDailies_Result>();
            long SeasonID = Utility.GetCurrentSeasonForView();
            long StationID = (int)Constants.RimStationsIDs.JhelumATMangla;
            double? PreLiveContent = -1;
            double? InitialLiveContent = -1;
            String EasternEKSum, EasternLKSum;

            #endregion

            try
            {
                ObjBalance = GetBalanceReservoirData(_SeasonalDraftID, StationID, _Scenario);
                if (ObjBalance != null)
                {
                    PreLiveContent = ObjBalance.InitStorage;
                    InitialLiveContent = PreLiveContent;
                    lstTDailyIDs = GetTDailyIDsWithNames(SeasonID);
                    lstJhelumForecastData = GetForecastVolume(_ForecastDraftID, _Scenario,
                        (int)Constants.RimStationsIDs.JhelumATMangla);
                    lstChenabForecastData = GetForecastVolume(_ForecastDraftID, _Scenario,
                        (int)Constants.RimStationsIDs.ChenabAtMarala);
                    lstEasternTDailies = GetEasternhistoricTDailies((short?)SeasonID, (short?)ObjBalance.EasternYears);
                    lstFillingFraction = GetFillingFraction(SeasonID, StationID);
                    lstFlow7782 = GetFlows7782(SeasonID);

                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        lstWaterDist = GetListWaterDistribution();
                        EasternEKSum = String.Format("{0:0.000}",
                            context.SPGetEasternComponentDataEKSum((short)SeasonID, (short?)ObjBalance.EasternYears)
                                .FirstOrDefault());
                        EasternLKSum = String.Format("{0:0.000}", context.SPGetEasternComponentDataLKSum((short)SeasonID, (short?)ObjBalance.EasternYears).FirstOrDefault());
                    }
                    else
                    {
                        EasternLKSum = String.Format("{0:0.000}",
                            GetEasternHistoricTDailiesSUM((short?)SeasonID, (short?)ObjBalance.EasternYears));
                        EasternEKSum = "";
                    }

                    if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                    {
                        Data = new
                        {
                            ShortName = "Mar3",
                            TDailyID = "",
                            Inflows = "",
                            StorageRelease = "",
                            LiveContent = String.Format("{0:0.000}", PreLiveContent),
                            ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                            Outflow = "",
                            Chenab = "",
                            Eastern = "",
                            SystemInflow = "",
                            LossGain = "",
                            BalanceInflow = "",
                            ProposedCanalWDL = "",
                            SystemOutflow = "",
                            Shortage = ""
                        };
                    }
                    else
                    {
                        Data = new
                        {
                            ShortName = "Sep3",
                            TDailyID = "",
                            Inflows = "",
                            StorageRelease = "",
                            LiveContent = String.Format("{0:0.000}", PreLiveContent),
                            ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                            Outflow = "",
                            Chenab = "",
                            Eastern = "",
                            SystemInflow = "",
                            LossGain = "",
                            BalanceInflow = "",
                            ProposedCanalWDL = "",
                            SystemOutflow = "",
                            Shortage = ""
                        };
                    }

                    lstData.Add(Data);


                    foreach (var TDaily in lstTDailyIDs)
                    {
                        double? JMInflows =
                            lstJhelumForecastData.Where(q => q.TDailyID == TDaily.TDailyID).FirstOrDefault().Volume;
                        double? CurrLiveContent = GetLiveContent(ObjBalance, lstFillingFraction, TDaily.TDailyID,
                            PreLiveContent, _Scenario, SeasonID);
                        double? ResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla,
                            CurrLiveContent);
                        double? Outflow = CalculteOutflow(JMInflows, (long)TDaily.NoOfDays, CurrLiveContent,
                            PreLiveContent);
                        double? StrRel = Outflow - JMInflows;
                        double? CMInflows =
                            lstChenabForecastData.Where(q => q.TDailyID == TDaily.TDailyID).FirstOrDefault().Volume;
                        double? Eastern = GetEasternTDailies(TDaily.TDailyID, lstEasternTDailies, EasternEKSum,
                            EasternLKSum, ObjBalance);
                        double? SystemInflows = Outflow + CMInflows + Eastern;
                        double? LossGain = null;

                        if (TDaily.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                            LossGain = (ObjBalance.EarlyKharif / 100) * SystemInflows;
                        else if (TDaily.TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                            LossGain = (ObjBalance.LateKharif / 100) * SystemInflows;
                        else
                            LossGain = (ObjBalance.SysLossGain / 100) * SystemInflows;

                        double? BalanceInflow = SystemInflows + LossGain;
                        double? ProposedWDLS = null;

                        if (SeasonID == (int)Constants.Seasons.Kharif)
                            ProposedWDLS = GetProposedCanalWDLSKharif(TDaily.TDailyID, BalanceInflow, ObjBalance,
                                lstWaterDist);
                        else
                            ProposedWDLS = GetProposedCanalWDLSRabi(TDaily.TDailyID, lstFlow7782, ObjBalance,
                                BalanceInflow);

                        double? SystemOutflows = BalanceInflow - ProposedWDLS;
                        PreLiveContent = CurrLiveContent;

                        SystemOutflows = Math.Round((double)SystemOutflows, 1);

                        double? Shortage = null;
                        SP_RefShareDistribution Flow7782 = lstFlow7782.Where(q => q.TDailyID == TDaily.TDailyID).FirstOrDefault();
                        Shortage = ((ProposedWDLS / Flow7782.Jc7782) * 100) - 100;

                        //Data = new
                        //{
                        //    ShortName = TDaily.ShortName,
                        //    TDailyID = TDaily.TDailyID,
                        //    TDailyCalID = TDaily.ID,
                        //    Inflows = String.Format("{0:0.0}", JMInflows),
                        //    StorageRelease = String.Format("{0:0.0}", StrRel),
                        //    LiveContent = String.Format("{0:0.000}", CurrLiveContent),
                        //    ReservoirLevel = String.Format("{0:0.00}", ResLevel),
                        //    Outflow = String.Format("{0:0.0}", Outflow),
                        //    Chenab = String.Format("{0:0.0}", CMInflows),
                        //    Eastern = String.Format("{0:0.0}", Eastern),
                        //    SystemInflow = String.Format("{0:0.0}", SystemInflows),
                        //    LossGain = String.Format("{0:0.0}", LossGain),
                        //    BalanceInflow = String.Format("{0:0.0}", BalanceInflow),
                        //    ProposedCanalWDL = String.Format("{0:0.0}", ProposedWDLS),
                        //    SystemOutflow = String.Format("{0:0.0}", SystemOutflows)
                        //};
                        //lstData.Add(Data);


                        Data = new
                        {
                            ShortName = TDaily.ShortName,
                            TDailyID = TDaily.TDailyID,
                            TDailyCalID = TDaily.ID,
                            Inflows = JMInflows,
                            StorageRelease = StrRel,
                            LiveContent = CurrLiveContent,
                            ReservoirLevel = ResLevel,
                            Outflow = Outflow,
                            Chenab = CMInflows,
                            Eastern = Eastern,
                            SystemInflow = SystemInflows,
                            LossGain = LossGain,
                            BalanceInflow = BalanceInflow,
                            ProposedCanalWDL = ProposedWDLS,
                            SystemOutflow = SystemOutflows,
                            Shortage = Shortage
                        };
                        lstData.Add(Data);
                    }

                    if (_CalculateMAF)
                        lstData = CalculateMAF(lstData, (int)Constants.RimStationsIDs.JhelumATMangla, _Scenario, _SeasonalDraftID);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstData;
        }

        public bool SavePlanBulk(List<object> _lstData, long _UserID, long _SPDraftID, long _StationID, string _Scenario)
        {
            List<SP_PlanData> lstPlanData = new List<SP_PlanData>();
            SP_PlanData PlanData;

            try
            {
                long ScenarioID = (from SPSce in context.SP_PlanScenario
                                   where
                                   SPSce.PlanDraftID == _SPDraftID && SPSce.StationID == _StationID && SPSce.Scenario == _Scenario
                                   select SPSce.ID).FirstOrDefault();

                context.SP_PlanData.RemoveRange(context.SP_PlanData.Where(q => q.PlanScenarioID == ScenarioID));

                if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                {
                    //remove MAF and previous season rows
                    //  if (_lstData.Count() == 22)
                    //    _lstData.RemoveAt(21);
                    //   if (_lstData.Count() == 21)
                    //      _lstData.RemoveAt(20);
                    //  if (_lstData.Count() == 20)
                    //      _lstData.RemoveAt(19);
                    _lstData.RemoveAt(0);

                    foreach (var obj in _lstData)
                    {
                        PlanData = new SP_PlanData();
                        PlanData.PlanScenarioID = ScenarioID;
                        PlanData.TDailyCalendarID =
                            Convert.ToInt64(obj.GetType().GetProperty("TDailyCalID").GetValue(obj));
                        PlanData.TDailyID = Convert.ToInt16(obj.GetType().GetProperty("TDailyID").GetValue(obj));
                        PlanData.Inflow = Convert.ToDouble(obj.GetType().GetProperty("Inflows").GetValue(obj));
                        PlanData.StorageRelease =
                            Convert.ToDouble(obj.GetType().GetProperty("StorageRelease").GetValue(obj));
                        PlanData.LiveContent = Convert.ToDouble(obj.GetType().GetProperty("LiveContent").GetValue(obj));
                        PlanData.ReservoirLevel =
                            Convert.ToDouble(obj.GetType().GetProperty("ReservoirLevel").GetValue(obj));
                        PlanData.Outflow = Convert.ToDouble(obj.GetType().GetProperty("Outflow").GetValue(obj));
                        PlanData.SystemInflow = Convert.ToDouble(obj.GetType().GetProperty("SystemInflow").GetValue(obj));
                        PlanData.LossGain = Convert.ToDouble(obj.GetType().GetProperty("LossGain").GetValue(obj));
                        PlanData.BalanceInflow = Convert.ToDouble(obj.GetType().GetProperty("BalanceInflow").GetValue(obj));
                        PlanData.ProposedCanalWDL = Convert.ToDouble(obj.GetType().GetProperty("ProposedCanalWDL").GetValue(obj));
                        PlanData.SystemOutflow = Convert.ToDouble(obj.GetType().GetProperty("SystemOutflow").GetValue(obj));
                        PlanData.WDLShortage = Convert.ToDouble(obj.GetType().GetProperty("Shortage").GetValue(obj));

                        if (_StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                        {
                            PlanData.Eastern = Convert.ToDouble(obj.GetType().GetProperty("Eastern").GetValue(obj));
                            PlanData.Chenab = Convert.ToDouble(obj.GetType().GetProperty("Chenab").GetValue(obj));
                        }
                        else
                        {
                            PlanData.Kabul = Convert.ToDouble(obj.GetType().GetProperty("Kabul").GetValue(obj));
                            PlanData.ChashmaStorageRelease =
                                Convert.ToDouble(obj.GetType().GetProperty("ChashmaStorageRelease").GetValue(obj));
                            PlanData.ChashmaLiveContent =
                                Convert.ToDouble(obj.GetType().GetProperty("ChashmaLiveContent").GetValue(obj));
                            PlanData.ChashmaLevel =
                                Convert.ToDouble(obj.GetType().GetProperty("ChashmaLevel").GetValue(obj));
                            PlanData.JCOutflow = Convert.ToDouble(obj.GetType().GetProperty("JCOutflow").GetValue(obj));
                        }

                        PlanData.CreatedDate = DateTime.Now;
                        PlanData.CreatedBy = (int)_UserID;
                        PlanData.ModifiedDate = DateTime.Now;
                        PlanData.ModifiedBy = (int)_UserID;
                        lstPlanData.Add(PlanData);
                    }
                }
                else
                {
                    //remove MAF and previous season rows  
                    //if (_lstData.Count() == 20)
                    //    _lstData.RemoveAt(19);
                    _lstData.RemoveAt(0);

                    foreach (var obj in _lstData)
                    {
                        PlanData = new SP_PlanData();
                        PlanData.PlanScenarioID = ScenarioID;
                        PlanData.TDailyCalendarID =
                            Convert.ToInt64(obj.GetType().GetProperty("TDailyCalID").GetValue(obj));
                        PlanData.TDailyID = Convert.ToInt16(obj.GetType().GetProperty("TDailyID").GetValue(obj));
                        PlanData.Inflow = Convert.ToDouble(obj.GetType().GetProperty("Inflows").GetValue(obj));
                        PlanData.StorageRelease =
                            Convert.ToDouble(obj.GetType().GetProperty("StorageRelease").GetValue(obj));
                        PlanData.LiveContent = Convert.ToDouble(obj.GetType().GetProperty("LiveContent").GetValue(obj));
                        PlanData.ReservoirLevel =
                            Convert.ToDouble(obj.GetType().GetProperty("ReservoirLevel").GetValue(obj));
                        PlanData.Outflow = Convert.ToDouble(obj.GetType().GetProperty("Outflow").GetValue(obj));
                        PlanData.SystemInflow = Convert.ToDouble(obj.GetType().GetProperty("SystemInflow").GetValue(obj));
                        PlanData.LossGain = Convert.ToDouble(obj.GetType().GetProperty("LossGain").GetValue(obj));
                        PlanData.BalanceInflow =
                            Convert.ToDouble(obj.GetType().GetProperty("BalanceInflow").GetValue(obj));
                        PlanData.ProposedCanalWDL =
                            Convert.ToDouble(obj.GetType().GetProperty("ProposedCanalWDL").GetValue(obj));
                        PlanData.SystemOutflow = Convert.ToDouble(obj.GetType().GetProperty("SystemOutflow").GetValue(obj));
                        PlanData.WDLShortage = Convert.ToDouble(obj.GetType().GetProperty("Shortage").GetValue(obj));

                        if (_StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                        {
                            PlanData.Eastern = Convert.ToDouble(obj.GetType().GetProperty("Eastern").GetValue(obj));
                            PlanData.Chenab = Convert.ToDouble(obj.GetType().GetProperty("Chenab").GetValue(obj));
                        }
                        else
                        {
                            PlanData.Kabul = Convert.ToDouble(obj.GetType().GetProperty("Kabul").GetValue(obj));
                            PlanData.ChashmaStorageRelease =
                                Convert.ToDouble(obj.GetType().GetProperty("ChashmaStorageRelease").GetValue(obj));
                            PlanData.ChashmaLiveContent =
                                Convert.ToDouble(obj.GetType().GetProperty("ChashmaLiveContent").GetValue(obj));
                            PlanData.ChashmaLevel =
                                Convert.ToDouble(obj.GetType().GetProperty("ChashmaLevel").GetValue(obj));
                            PlanData.JCOutflow = Convert.ToDouble(obj.GetType().GetProperty("JCOutflow").GetValue(obj));
                        }

                        PlanData.CreatedDate = DateTime.Now;
                        PlanData.CreatedBy = (int)_UserID;
                        PlanData.ModifiedDate = DateTime.Now;
                        PlanData.ModifiedBy = (int)_UserID;
                        lstPlanData.Add(PlanData);
                    }
                }

                context.SP_PlanData.AddRange(lstPlanData);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public List<object> GetLikelyPlanData(long _SPDraftID, bool _CalculateMAF, bool _FromSave)
        {
            #region Variables
            List<object> lstData = new List<object>();
            List<SP_RefTDailyCalendar> lstTDailyIDs = new List<SP_RefTDailyCalendar>();
            object objData;
            int CurrTDaily = 1;
            int EndTDaily = 18;
            int SeasonID = Utility.GetCurrentSeasonForView();
            double?
               JMInflowEK = 0,
               StrRelEK = 0,
               LiveContentEK = 0,
               OutflowEK = 0,
               CMInflowsEK = 0,
               EasternEK = 0,
               SystemInflowsEK = 0,
               LossGainEK = 0,
               BalanceInflowEK = 0,
               ProposedWDLSEK = 0,
               SystemOutflowEK = 0,
               JMInflowLK = 0,
               StrRelLK = 0,
               LiveContentLK = 0,
               OutflowLK = 0,
               CMInflowsLK = 0,
               EasternLK = 0,
               SystemInflowsLK = 0,
               LossGainLK = 0,
               BalanceInflowLK = 0,
               ProposedWDLSLK = 0,
               SystemOutflowLK = 0,
               EKLivecontent = 0,
               InitialLiveContent = 0;
            #endregion

            try
            {
                List<SP_RefShareDistribution> lstFlow7782 = GetFlows7782(SeasonID);

                if (SeasonID == (int)Constants.Seasons.Rabi)
                {
                    CurrTDaily = 19;
                    EndTDaily = 36;
                }

                lstTDailyIDs = GetTDailyIDsWithNames(SeasonID);

                List<SP_PlanData> lstMax = (from Sce in context.SP_PlanScenario
                                            join Data in context.SP_PlanData on Sce.ID equals Data.PlanScenarioID
                                            where Sce.StationID == (int)Constants.RimStationsIDs.JhelumATMangla && Sce.PlanDraftID == _SPDraftID && Sce.Scenario == "Maximum"
                                            select Data).ToList();

                List<SP_PlanData> lstMin = (from Sce in context.SP_PlanScenario
                                            join Data in context.SP_PlanData on Sce.ID equals Data.PlanScenarioID
                                            where Sce.StationID == (int)Constants.RimStationsIDs.JhelumATMangla && Sce.PlanDraftID == _SPDraftID && Sce.Scenario == "Minimum"
                                            select Data).ToList();

                SP_PlanBalance ObjBalance = (from Sce in context.SP_PlanScenario
                                             join Bal in context.SP_PlanBalance on Sce.ID equals Bal.PlanScenarioID
                                             where Sce.StationID == (int)Constants.RimStationsIDs.JhelumATMangla && Sce.PlanDraftID == _SPDraftID && Sce.Scenario == "Likely"
                                             select Bal).FirstOrDefault();

                if (lstMax.Count() == 18 && lstMin.Count() == 18 && ObjBalance != null)
                {
                    InitialLiveContent = ObjBalance.InitStorage;

                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        objData = new
                        {
                            ShortName = "Mar3",
                            Inflows = "",
                            StorageRelease = "",
                            LiveContent = String.Format("{0:0.000}", InitialLiveContent),
                            ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                            Outflow = "",
                            Chenab = "",
                            Eastern = "",
                            SystemInflow = "",
                            LossGain = "",
                            BalanceInflow = "",
                            ProposedCanalWDL = "",
                            SystemOutflow = "",
                            Shortage = ""
                        };
                        lstData.Add(objData);
                    }
                    else
                    {
                        objData = new
                        {
                            ShortName = "Sep3",
                            Inflows = "",
                            StorageRelease = "",
                            LiveContent = String.Format("{0:0.000}", InitialLiveContent),
                            ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                            Outflow = "",
                            Chenab = "",
                            Eastern = "",
                            SystemInflow = "",
                            LossGain = "",
                            BalanceInflow = "",
                            ProposedCanalWDL = "",
                            SystemOutflow = "",
                            Shortage = ""
                        };
                        lstData.Add(objData);
                    }

                    for (; CurrTDaily <= EndTDaily; CurrTDaily++)
                    {
                        SP_PlanData ObjMax = lstMax.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault();
                        SP_PlanData ObjMin = lstMin.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault();
                        objData = new object();

                        double? Inflow = (ObjMax.Inflow + ObjMin.Inflow) / 2;
                        double? StorageRelease = (ObjMax.StorageRelease + ObjMin.StorageRelease) / 2;
                        double? LiveContent = (ObjMax.LiveContent + ObjMin.LiveContent) / 2;
                        double? ReservoirLevel = GetReservoirLevel((int)Constants.RimStationsIDs.JhelumATMangla, LiveContent);
                        double? Outflow = (ObjMax.Outflow + ObjMin.Outflow) / 2;
                        double? Chenab = (ObjMax.Chenab + ObjMin.Chenab) / 2;
                        double? Eastern = (ObjMax.Eastern + ObjMin.Eastern) / 2;
                        double? SystemInflow = (ObjMax.SystemInflow + ObjMin.SystemInflow) / 2;
                        double? LossGain = (ObjMax.LossGain + ObjMin.LossGain) / 2;
                        double? BalanceInflow = (ObjMax.BalanceInflow + ObjMin.BalanceInflow) / 2;
                        double? ProposedCanalWDL = (ObjMax.ProposedCanalWDL + ObjMin.ProposedCanalWDL) / 2;
                        double? SystemOutflow = (ObjMax.SystemOutflow + ObjMin.SystemOutflow) / 2;

                        double? Shortage = null;
                        SP_RefShareDistribution Flow7782 = lstFlow7782.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault();
                        Shortage = ((ProposedCanalWDL / Flow7782.Jc7782) * 100) - 100;

                        if (_FromSave)
                        {
                            objData = new
                            {
                                ShortName = Utility.GetTDailyShortName((short)ObjMax.TDailyID, Utility.GetCurrentSeasonForView()),
                                TDailyID = lstTDailyIDs.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault().TDailyID,
                                TDailyCalID = lstTDailyIDs.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault().ID,
                                Inflows = Inflow,
                                StorageRelease = StorageRelease,
                                LiveContent = LiveContent,
                                ReservoirLevel = ReservoirLevel,
                                Outflow = Outflow,
                                Chenab = Chenab,
                                Eastern = Eastern,
                                SystemInflow = SystemInflow,
                                LossGain = LossGain,
                                BalanceInflow = BalanceInflow,
                                ProposedCanalWDL = ProposedCanalWDL,
                                SystemOutflow = SystemOutflow,
                                Shortage = Shortage
                            };
                            lstData.Add(objData);
                        }
                        else
                        {
                            objData = new
                            {
                                ShortName = Utility.GetTDailyShortName((short)ObjMax.TDailyID, Utility.GetCurrentSeasonForView()),
                                TDailyID = lstTDailyIDs.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault().TDailyID,
                                TDailyCalID = lstTDailyIDs.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault().ID,
                                Inflows = String.Format("{0:0.0}", Inflow),
                                StorageRelease = String.Format("{0:0.0}", StorageRelease),
                                LiveContent = String.Format("{0:0.000}", LiveContent),
                                ReservoirLevel = String.Format("{0:0.00}", ReservoirLevel),
                                Outflow = String.Format("{0:0.0}", Outflow),
                                Chenab = String.Format("{0:0.0}", Chenab),
                                Eastern = String.Format("{0:0.0}", Eastern),
                                SystemInflow = String.Format("{0:0.0}", SystemInflow),
                                LossGain = String.Format("{0:0.0}", LossGain),
                                BalanceInflow = String.Format("{0:0.0}", BalanceInflow),
                                ProposedCanalWDL = String.Format("{0:0.0}", ProposedCanalWDL),
                                SystemOutflow = String.Format("{0:0.0}", SystemOutflow),
                                Shortage = String.Format("{0:0.0}", Shortage)
                            };
                            lstData.Add(objData);

                            if (SeasonID == (int)Constants.Seasons.Kharif)
                            {
                                if (CurrTDaily <= (int)Constants.SeasonDistribution.EKEnd)
                                {
                                    if (CurrTDaily == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                                    {
                                        JMInflowEK = JMInflowEK + (Inflow * Constants.TDailyConversion);
                                        CMInflowsEK = CMInflowsEK + (Chenab * Constants.TDailyConversion);
                                        StrRelEK = StrRelEK + (StorageRelease * Constants.TDailyConversion);
                                        OutflowEK = OutflowEK + (Outflow * Constants.TDailyConversion);
                                        EasternEK = EasternEK + (Eastern * Constants.TDailyConversion);
                                        SystemInflowsEK = SystemInflowsEK + (SystemInflow * Constants.TDailyConversion);
                                        LossGainEK = LossGainEK + (LossGain * Constants.TDailyConversion);
                                        BalanceInflowEK = BalanceInflowEK + (BalanceInflow * Constants.TDailyConversion);
                                        ProposedWDLSEK = ProposedWDLSEK + (ProposedCanalWDL * Constants.TDailyConversion);
                                        SystemOutflowEK = SystemOutflowEK + (SystemOutflow * Constants.TDailyConversion);
                                    }
                                    else
                                    {
                                        JMInflowEK = JMInflowEK + Inflow;
                                        CMInflowsEK = CMInflowsEK + Chenab;
                                        StrRelEK = StrRelEK + StorageRelease;
                                        OutflowEK = OutflowEK + Outflow;
                                        EasternEK = EasternEK + Eastern;
                                        SystemInflowsEK = SystemInflowsEK + SystemInflow;
                                        LossGainEK = LossGainEK + LossGain;
                                        BalanceInflowEK = BalanceInflowEK + BalanceInflow;
                                        ProposedWDLSEK = ProposedWDLSEK + ProposedCanalWDL;
                                        SystemOutflowEK = SystemOutflowEK + SystemOutflow;
                                    }

                                    if (CurrTDaily == (int)Constants.SeasonDistribution.EKEnd)
                                    {
                                        EKLivecontent = LiveContent;
                                        JMInflowEK = JMInflowEK * Constants.MAFConversion;
                                        CMInflowsEK = CMInflowsEK * Constants.MAFConversion;
                                        StrRelEK = StrRelEK * Constants.MAFConversion;
                                        LiveContentEK = InitialLiveContent - LiveContent;
                                        OutflowEK = OutflowEK * Constants.MAFConversion;
                                        EasternEK = EasternEK * Constants.MAFConversion;
                                        SystemInflowsEK = SystemInflowsEK * Constants.MAFConversion;
                                        LossGainEK = LossGainEK * Constants.MAFConversion;
                                        BalanceInflowEK = BalanceInflowEK * Constants.MAFConversion;
                                        ProposedWDLSEK = ProposedWDLSEK * Constants.MAFConversion;
                                        SystemOutflowEK = SystemOutflowEK * Constants.MAFConversion;

                                        InitialLiveContent = LiveContent;
                                    }
                                }
                                else if (CurrTDaily > (int)Constants.SeasonDistribution.EKEnd && CurrTDaily <= (int)Constants.SeasonDistribution.LKEnd)
                                {
                                    if (CurrTDaily == (int)Constants.TDAilySpecialCases.JulyTDaily || CurrTDaily == (int)Constants.TDAilySpecialCases.AugTDaily)
                                    {
                                        JMInflowLK = JMInflowLK + (Inflow * Constants.TDailyConversion);
                                        CMInflowsLK = CMInflowsLK + (Chenab * Constants.TDailyConversion);
                                        StrRelLK = StrRelLK + (StorageRelease * Constants.TDailyConversion);
                                        OutflowLK = OutflowLK + (Outflow * Constants.TDailyConversion);
                                        EasternLK = EasternLK + (Eastern * Constants.TDailyConversion);
                                        SystemInflowsLK = SystemInflowsLK + (SystemInflow * Constants.TDailyConversion);
                                        LossGainLK = LossGainLK + (LossGain * Constants.TDailyConversion);
                                        BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.TDailyConversion);
                                        ProposedWDLSLK = ProposedWDLSLK + (ProposedCanalWDL * Constants.TDailyConversion);
                                        SystemOutflowLK = SystemOutflowLK + (SystemOutflow * Constants.TDailyConversion);
                                    }
                                    else
                                    {
                                        JMInflowLK = JMInflowLK + Inflow;
                                        CMInflowsLK = CMInflowsLK + Chenab;
                                        StrRelLK = StrRelLK + StorageRelease;
                                        OutflowLK = OutflowLK + Outflow;
                                        EasternLK = EasternLK + Eastern;
                                        SystemInflowsLK = SystemInflowsLK + SystemInflow;
                                        LossGainLK = LossGainLK + LossGain;
                                        BalanceInflowLK = BalanceInflowLK + BalanceInflow;
                                        ProposedWDLSLK = ProposedWDLSLK + ProposedCanalWDL;
                                        SystemOutflowLK = SystemOutflowLK + SystemOutflow;
                                    }

                                    if (CurrTDaily == (int)Constants.SeasonDistribution.LKEnd)
                                    {
                                        JMInflowLK = JMInflowLK * Constants.MAFConversion;
                                        CMInflowsLK = CMInflowsLK * Constants.MAFConversion;
                                        StrRelLK = StrRelLK * Constants.MAFConversion;
                                        LiveContentLK = InitialLiveContent - LiveContent;
                                        OutflowLK = OutflowLK * Constants.MAFConversion;
                                        EasternLK = EasternLK * Constants.MAFConversion;
                                        SystemInflowsLK = SystemInflowsLK * Constants.MAFConversion;
                                        LossGainLK = LossGainLK * Constants.MAFConversion;
                                        BalanceInflowLK = BalanceInflowLK * Constants.MAFConversion;
                                        ProposedWDLSLK = ProposedWDLSLK * Constants.MAFConversion;
                                        SystemOutflowLK = SystemOutflowLK * Constants.MAFConversion;

                                        if (_CalculateMAF)
                                        {
                                            object Data = new
                                             {
                                                 ShortName = "EK(MAF)",
                                                 TDailyID = "",
                                                 TDailyCalID = "",
                                                 Inflows = String.Format("{0:0.000}", JMInflowEK),
                                                 StorageRelease = String.Format("{0:0.000}", StrRelEK),
                                                 LiveContent = String.Format("{0:0.000}", LiveContentEK),
                                                 ReservoirLevel = "",
                                                 Outflow = String.Format("{0:0.000}", OutflowEK),
                                                 Chenab = String.Format("{0:0.000}", CMInflowsEK),
                                                 Eastern = String.Format("{0:0.000}", EasternEK),
                                                 SystemInflow = String.Format("{0:0.000}", SystemInflowsEK),
                                                 LossGain = String.Format("{0:0.000}", LossGainEK),
                                                 BalanceInflow = String.Format("{0:0.000}", BalanceInflowEK),
                                                 ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSEK),
                                                 SystemOutflow = String.Format("{0:0.000}", SystemOutflowEK),
                                                 Shortage = ""
                                             };
                                            lstData.Add(Data);

                                            Data = new
                                            {
                                                ShortName = "LK(MAF)",
                                                TDailyID = "",
                                                TDailyCalID = "",
                                                Inflows = String.Format("{0:0.000}", JMInflowLK),
                                                StorageRelease = String.Format("{0:0.000}", StrRelLK),
                                                LiveContent = String.Format("{0:0.000}", LiveContentLK),
                                                ReservoirLevel = "",
                                                Outflow = String.Format("{0:0.000}", OutflowLK),
                                                Chenab = String.Format("{0:0.000}", CMInflowsLK),
                                                Eastern = String.Format("{0:0.000}", EasternLK),
                                                SystemInflow = String.Format("{0:0.000}", SystemInflowsLK),
                                                LossGain = String.Format("{0:0.000}", LossGainLK),
                                                BalanceInflow = String.Format("{0:0.000}", BalanceInflowLK),
                                                ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSLK),
                                                SystemOutflow = String.Format("{0:0.000}", SystemOutflowLK),
                                                Shortage = ""
                                            };
                                            lstData.Add(Data);

                                            Data = new
                                            {
                                                ShortName = "Total(MAF)",
                                                TDailyID = "",
                                                TDailyCalID = "",
                                                Inflows = String.Format("{0:0.000}", (JMInflowEK + JMInflowLK)),
                                                StorageRelease = String.Format("{0:0.000}", (StrRelEK + StrRelLK)),
                                                LiveContent = String.Format("{0:0.000}", (LiveContentEK + LiveContentLK)),
                                                ReservoirLevel = "",
                                                Outflow = String.Format("{0:0.000}", (OutflowEK + OutflowLK)),
                                                Chenab = String.Format("{0:0.000}", (CMInflowsEK + CMInflowsLK)),
                                                Eastern = String.Format("{0:0.000}", (EasternEK + EasternLK)),
                                                SystemInflow = String.Format("{0:0.000}", (SystemInflowsEK + SystemInflowsLK)),
                                                LossGain = String.Format("{0:0.000}", (LossGainEK + LossGainLK)),
                                                BalanceInflow = String.Format("{0:0.000}", (BalanceInflowEK + BalanceInflowLK)),
                                                ProposedCanalWDL = String.Format("{0:0.000}", (ProposedWDLSEK + ProposedWDLSLK)),
                                                SystemOutflow = String.Format("{0:0.000}", (SystemOutflowEK + SystemOutflowLK)),
                                                Shortage = ""
                                            };
                                            lstData.Add(Data);

                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (CurrTDaily == (int)Constants.TDAilySpecialCases.OctTDaily || CurrTDaily == (int)Constants.TDAilySpecialCases.DecTDaily
                                || CurrTDaily == (int)Constants.TDAilySpecialCases.JanTDaily || CurrTDaily == (int)Constants.TDAilySpecialCases.MarTDaily)
                                {
                                    JMInflowLK = JMInflowLK + (Inflow * Constants.TDailyConversion);
                                    CMInflowsLK = CMInflowsLK + (Chenab * Constants.TDailyConversion);
                                    StrRelLK = StrRelLK + (StorageRelease * Constants.TDailyConversion);
                                    OutflowLK = OutflowLK + (Outflow * Constants.TDailyConversion);
                                    EasternLK = EasternLK + (Eastern * Constants.TDailyConversion);
                                    SystemInflowsLK = SystemInflowsLK + (SystemInflow * Constants.TDailyConversion);
                                    LossGainLK = LossGainLK + (LossGain * Constants.TDailyConversion);
                                    BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.TDailyConversion);
                                    ProposedWDLSLK = ProposedWDLSLK + (ProposedCanalWDL * Constants.TDailyConversion);
                                    SystemOutflowLK = SystemOutflowLK + (SystemOutflow * Constants.TDailyConversion);
                                }
                                else if (CurrTDaily == (int)Constants.TDAilySpecialCases.FebTDaily)
                                {
                                    if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                    {
                                        JMInflowLK = JMInflowLK + (Inflow * Constants.LeapYearTrue);
                                        CMInflowsLK = CMInflowsLK + (Chenab * Constants.LeapYearTrue);
                                        StrRelLK = StrRelLK + (StorageRelease * Constants.LeapYearTrue);
                                        OutflowLK = OutflowLK + (Outflow * Constants.LeapYearTrue);
                                        EasternLK = EasternLK + (Eastern * Constants.LeapYearTrue);
                                        SystemInflowsLK = SystemInflowsLK + (SystemInflow * Constants.LeapYearTrue);
                                        LossGainLK = LossGainLK + (LossGain * Constants.LeapYearTrue);
                                        BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.LeapYearTrue);
                                        ProposedWDLSLK = ProposedWDLSLK + (ProposedCanalWDL * Constants.LeapYearTrue);
                                        SystemOutflowLK = SystemOutflowLK + (SystemOutflow * Constants.LeapYearTrue);
                                    }
                                    else
                                    {
                                        JMInflowLK = JMInflowLK + (Inflow * Constants.LeapYearFalse);
                                        CMInflowsLK = CMInflowsLK + (Chenab * Constants.LeapYearFalse);
                                        StrRelLK = StrRelLK + (StorageRelease * Constants.LeapYearFalse);
                                        OutflowLK = OutflowLK + (Outflow * Constants.LeapYearFalse);
                                        EasternLK = EasternLK + (Eastern * Constants.LeapYearFalse);
                                        SystemInflowsLK = SystemInflowsLK + (SystemInflow * Constants.LeapYearFalse);
                                        LossGainLK = LossGainLK + (LossGain * Constants.LeapYearFalse);
                                        BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.LeapYearFalse);
                                        ProposedWDLSLK = ProposedWDLSLK + (ProposedCanalWDL * Constants.LeapYearFalse);
                                        SystemOutflowLK = SystemOutflowLK + (SystemOutflow * Constants.LeapYearFalse);
                                    }
                                }
                                else
                                {
                                    JMInflowLK = JMInflowLK + Inflow;
                                    CMInflowsLK = CMInflowsLK + Chenab;
                                    StrRelLK = StrRelLK + StorageRelease;
                                    OutflowLK = OutflowLK + Outflow;
                                    EasternLK = EasternLK + Eastern;
                                    SystemInflowsLK = SystemInflowsLK + SystemInflow;
                                    LossGainLK = LossGainLK + LossGain;
                                    BalanceInflowLK = BalanceInflowLK + BalanceInflow;
                                    ProposedWDLSLK = ProposedWDLSLK + ProposedCanalWDL;
                                    SystemOutflowLK = SystemOutflowLK + SystemOutflow;
                                }

                                if (CurrTDaily == (int)Constants.SeasonDistribution.RabiEnd)
                                {
                                    JMInflowLK = JMInflowLK * Constants.MAFConversion;
                                    CMInflowsLK = CMInflowsLK * Constants.MAFConversion;
                                    StrRelLK = StrRelLK * Constants.MAFConversion;
                                    LiveContentLK = InitialLiveContent - LiveContent;
                                    OutflowLK = OutflowLK * Constants.MAFConversion;
                                    EasternLK = EasternLK * Constants.MAFConversion;
                                    SystemInflowsLK = SystemInflowsLK * Constants.MAFConversion;
                                    LossGainLK = LossGainLK * Constants.MAFConversion;
                                    BalanceInflowLK = BalanceInflowLK * Constants.MAFConversion;
                                    ProposedWDLSLK = ProposedWDLSLK * Constants.MAFConversion;
                                    SystemOutflowLK = SystemOutflowLK * Constants.MAFConversion;
                                    if (_CalculateMAF)
                                    {
                                        object Data = new
                                        {
                                            ShortName = "Rabi(MAF)",
                                            TDailyID = "",
                                            TDailyCalID = "",
                                            Inflows = String.Format("{0:0.000}", JMInflowLK),
                                            StorageRelease = String.Format("{0:0.000}", StrRelLK),
                                            LiveContent = String.Format("{0:0.000}", LiveContentLK),
                                            ReservoirLevel = "",
                                            Outflow = String.Format("{0:0.000}", OutflowLK),
                                            Chenab = String.Format("{0:0.000}", CMInflowsLK),
                                            Eastern = String.Format("{0:0.000}", EasternLK),
                                            SystemInflow = String.Format("{0:0.000}", SystemInflowsLK),
                                            LossGain = String.Format("{0:0.000}", LossGainLK),
                                            BalanceInflow = String.Format("{0:0.000}", BalanceInflowLK),
                                            ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSLK),
                                            SystemOutflow = String.Format("{0:0.000}", SystemOutflowLK),
                                            Shortage = ""
                                        };
                                        lstData.Add(Data);
                                    }
                                }
                            }


                        }

                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstData;
        }

        #endregion

        #region Indus Command

        public double GetStorageAgainstReservoirLevel(long _StationID, double? _ResLevel)
        {
            List<SP_RefElevationCapacity> lstValues = new List<SP_RefElevationCapacity>();
            double Capacity = 0;
            try
            {
                lstValues = (from EC in context.SP_RefElevationCapacity
                             where EC.ElevationCapacityDate == ((from ECDate in context.SP_RefElevationCapacity
                                                                 select new
                                                                 {
                                                                     ECDate.ElevationCapacityDate
                                                                 }).Distinct().OrderByDescending(w => w.ElevationCapacityDate).ToList()
                                       .Select(q => new
                                       {
                                           Year = q.ElevationCapacityDate
                                       }).Where(p => p.Year < DateTime.Now).FirstOrDefault().Year)

                                   && EC.StationID == _StationID
                             select EC).ToList();

                if (lstValues != null && lstValues.Count() > 0)
                {
                    SP_RefElevationCapacity ExactValue = lstValues.Where(p => p.Level == _ResLevel).LastOrDefault();
                    if (ExactValue == null)
                    {
                        SP_RefElevationCapacity LessValue =
                            lstValues.TakeWhile(p => p.Level < _ResLevel).LastOrDefault();
                        SP_RefElevationCapacity GreaterValue =
                            lstValues.Where(p => p.Level > LessValue.Level).FirstOrDefault();

                        double y1 = 0, y2 = 0, x1 = 0, x2 = 0;

                        if (GreaterValue != null && LessValue != null)
                        {
                            x1 = LessValue.Level.GetValueOrDefault(0);
                            y1 = LessValue.Capacity;

                            x2 = GreaterValue.Level.GetValueOrDefault(0);
                            y2 = GreaterValue.Capacity;

                            Capacity = y1 + ((((double)_ResLevel - x1) / (x2 - x1)) * (y2 - y1));
                        }
                    }
                    else
                        Capacity = (double)ExactValue.Capacity;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Capacity;
        }

        public double? GetLiveContentIndus(SP_PlanBalance _ObjBalance, List<SP_RefFillingFraction> _LstFillingFraction,
            short? _TDAilyID, double? _PreLiveContent, double? _PreResLevel, string _Scenario, long _SeasonID,
            double? _EKLiveContent)
        {
            double? LiveContent = 0;
            double? FF = -1;
            try
            {
                if (_Scenario == "Maximum")
                    FF = _LstFillingFraction.Where(q => q.TDailyID == _TDAilyID).FirstOrDefault().MaxFill;
                else if (_Scenario == "Minimum")
                    FF = _LstFillingFraction.Where(q => q.TDailyID == _TDAilyID).FirstOrDefault().MinFill;
                else
                    FF = _LstFillingFraction.Where(q => q.TDailyID == _TDAilyID).FirstOrDefault().LikelyFill;
                FF = FF / 100;

                if (_SeasonID == (int)Constants.Seasons.Kharif)
                {
                    if (_TDAilyID <= (int)Constants.SeasonDistribution.EKEnd)
                        LiveContent = (FF * _ObjBalance.FlowsEK) + _PreLiveContent;
                    else if (_TDAilyID == 8)
                        LiveContent = ((_ObjBalance.FillingLimitStorage - _PreLiveContent) * FF) + _PreLiveContent;
                    else if (_TDAilyID == 9)
                        LiveContent = ((_ObjBalance.FillingLimitStorage - _EKLiveContent) * FF) + _PreLiveContent;

                    if (_Scenario == "Minimum")
                    {
                        if (_TDAilyID == 10)
                            LiveContent = ((_ObjBalance.FillingLimitStorage - _EKLiveContent) * FF) + _PreLiveContent;
                        else if (_TDAilyID > 10 && _TDAilyID < 15)
                        {
                            _PreResLevel += 10;
                            LiveContent = GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela,
                                _PreResLevel);
                        }
                        else if (_TDAilyID == 15)
                            LiveContent = _PreLiveContent;
                    }
                    else
                    {
                        if (_TDAilyID > 9 && _TDAilyID < 14)
                        {
                            _PreResLevel += 10;
                            LiveContent = GetStorageAgainstReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela,
                                _PreResLevel);
                        }

                        if (_TDAilyID == 14 || _TDAilyID == 15)
                            LiveContent = _PreLiveContent;
                    }

                    if (_TDAilyID > 15)
                        LiveContent = _PreLiveContent - ((_ObjBalance.StorageDep / 100) * _ObjBalance.Storage * FF);
                }
                else
                    LiveContent = _PreLiveContent - (_ObjBalance.StorageRelease * FF);

                if (LiveContent < 0)
                    LiveContent = 0.000;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return LiveContent;
        }

        public double? GetLivecontentChashma(SP_PlanBalance _ObjBalance, short? _TDAilyID, long _SeasonID)
        {
            double? Livecontent = null;
            try
            {
                if (_SeasonID == (int)Constants.Seasons.Rabi)
                    Livecontent = _ObjBalance.ChashmaMinStorage;
                else
                {
                    if (_TDAilyID == 1 || _TDAilyID == 15 || _TDAilyID == 17)
                        Livecontent = (_ObjBalance.ChashmaStorage - _ObjBalance.ChashmaMinStorage) / 1.5;
                    else if (_TDAilyID == 16)
                        Livecontent = _ObjBalance.ChashmaStorage;
                    else
                        Livecontent = _ObjBalance.ChashmaMinStorage;
                }

                if (Livecontent < 0)
                    Livecontent = 0.00;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Livecontent;
        }

        public List<double?> GetJCOutflowIndus(long _SeasonalDraftID, string _Scenario)
        {
            List<double?> lstJCOutflows = new List<double?>();
            try
            {
                lstJCOutflows = (from SPSce in context.SP_PlanScenario
                                 join SPData in context.SP_PlanData on SPSce.ID equals SPData.PlanScenarioID
                                 where
                                 SPSce.PlanDraftID == _SeasonalDraftID && SPSce.Scenario == _Scenario &&
                                 SPSce.StationID == (int)Constants.RimStationsIDs.JhelumATMangla
                                 select SPData.SystemOutflow).ToList<double?>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstJCOutflows;
        }

        public double? GetLossGain(double? _SystemInflow, SP_PlanBalance _ObjBalance, short? _TDailyID)
        {
            double? LossGain = null;
            try
            {
                if (_TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    LossGain = _SystemInflow * (_ObjBalance.EarlyKharif / 100);
                else if (_TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                    LossGain = _SystemInflow * (_ObjBalance.LateKharif / 100);
                else if (_TDailyID <= (int)Constants.SeasonDistribution.RabiEnd)
                    LossGain = _SystemInflow * (_ObjBalance.SysLossGain / 100);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return LossGain;
        }

        public List<object> GetPara2TDailies()
        {
            List<object> lstPara2 = new List<object>();
            try
            {
                lstPara2 = (from SD in context.SP_RefShareDistribution
                            join Cal in context.SP_RefTDailyCalendar on SD.TDailyID equals Cal.TDailyID
                            where SD.SeasonID == (int)Constants.Seasons.Kharif && Cal.Year == DateTime.Now.Year - 1
                            select new
                            {
                                ID = Cal.TDailyID,
                                ParaPunjab = SD.PunjabPara2,
                                ParaSindh = SD.SindhPara2,
                                Balochistan = SD.BalochistanShare,
                                KPKShare = SD.KPShare,
                            })
                    .ToList()
                    .Select(q =>
                        new
                        {
                            q.ID,
                            Para = q.ParaPunjab + q.ParaSindh + q.Balochistan + q.KPKShare
                        }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstPara2;
        }

        public double? GetIndusProposedCanalWDLSKharif(double? _BalanceInflows, List<object> _lstPara, int _Iterators,
            long _SeasonID, List<SP_RefShareDistribution> _lstFlow7782, short? _TDailyID)
        {
            double? CanalWDLS = null;
            try
            {
                if (_SeasonID == (int)Constants.Seasons.Kharif)
                {
                    CanalWDLS =
                        Convert.ToDouble(
                            _lstPara.ElementAtOrDefault(_Iterators)
                                .GetType()
                                .GetProperty("Para")
                                .GetValue(_lstPara.ElementAtOrDefault(_Iterators)));
                    if (_BalanceInflows < CanalWDLS)
                        CanalWDLS = _BalanceInflows;
                }
                else
                {
                    //SP_RefShareDistribution ObjFlow = _lstFlow7782.Where(q => q.TDailyID == _TDailyID).FirstOrDefault();
                    //CanalWDLS = ObjFlow.BalochistanShare + ObjFlow.KPShare + ObjFlow.PunjabHistoric + ObjFlow.SindhHistoric;
                    //if (_BalanceInflows < CanalWDLS)
                    //    CanalWDLS = _BalanceInflows;
                    CanalWDLS = _BalanceInflows;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return CanalWDLS;
        }

        public List<object> CalculateIndusPlan(long _ForecastDraftID, long _SeasonalDraftID, string _Scenario, bool _CalculateMAF)
        {
            #region Variables

            object Data = new object();
            List<object> lstData = new List<object>();
            List<SP_RefTDailyCalendar> lstTDailyIDs = new List<SP_RefTDailyCalendar>();
            List<SP_ForecastData> lstIndusForecastData = new List<SP_ForecastData>();
            List<SP_ForecastData> lstKabulForecastData = new List<SP_ForecastData>();
            SP_PlanBalance ObjBalance = new SP_PlanBalance();
            List<double?> lstJCOutflows = new List<double?>();
            List<SP_RefFillingFraction> lstFillingFraction = new List<SP_RefFillingFraction>();
            List<object> lstPara = new List<object>();
            List<SP_RefShareDistribution> lstFlow7782 = new List<SP_RefShareDistribution>();
            long SeasonID = Utility.GetCurrentSeasonForView();
            long StationID = (int)Constants.RimStationsIDs.IndusAtTarbela;
            int Iterator = 0;
            double? PreLiveContent = -1,
                InitialLiveContent = -1,
                PreLiveContentChashma = -1,
                InitialLiveContentChashma = -1,
                PreReservoirLevel = -1,
                EKLiveContent = 0;

            #endregion

            try
            {
                ObjBalance = GetBalanceReservoirData(_SeasonalDraftID, StationID, _Scenario);
                lstTDailyIDs = GetTDailyIDsWithNames(SeasonID);
                lstIndusForecastData = GetForecastVolume(_ForecastDraftID, _Scenario,
                    (int)Constants.RimStationsIDs.IndusAtTarbela);
                lstKabulForecastData = GetForecastVolume(_ForecastDraftID, _Scenario,
                    (int)Constants.RimStationsIDs.KabulAtNowshera);
                lstJCOutflows = GetJCOutflowIndus(_SeasonalDraftID, _Scenario);
                lstFillingFraction = GetFillingFraction(SeasonID, StationID);
                if (SeasonID == (int)Constants.Seasons.Kharif)
                    lstPara = GetPara2TDailies(); // for kharif only
                // else
                lstFlow7782 = GetFlows7782(SeasonID); // for Rabi
                PreLiveContent = ObjBalance.InitStorage;
                InitialLiveContent = PreLiveContent;
                PreLiveContentChashma = ObjBalance.ChashmaStorage;
                InitialLiveContentChashma = PreLiveContentChashma;

                if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                {
                    Data = new
                    {
                        ShortName = "Mar3",
                        TDailyID = "",
                        TDailyCalID = "",
                        Inflows = "",
                        StorageRelease = "",
                        LiveContent = String.Format("{0:0.000}", PreLiveContent),
                        ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                        Outflow = "",
                        Kabul = "",
                        ChashmaStorageRelease = "",
                        ChashmaLiveContent = String.Format("{0:0.000}", ObjBalance.ChashmaStorage),
                        ChashmaLevel = String.Format("{0:0.00}", ObjBalance.ChashmaResLevel),
                        JCOutflow = "",
                        SystemInflow = "",
                        LossGain = "",
                        BalanceInflow = "",
                        ProposedCanalWDL = "",
                        SystemOutflow = "",
                        Shortage = ""
                    };
                }
                else
                {
                    Data = new
                    {
                        ShortName = "Sep3",
                        TDailyID = "",
                        TDailyCalID = "",
                        Inflows = "",
                        StorageRelease = "",
                        LiveContent = String.Format("{0:0.000}", PreLiveContent),
                        ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                        Outflow = "",
                        Kabul = "",
                        ChashmaStorageRelease = "",
                        ChashmaLiveContent = String.Format("{0:0.000}", ObjBalance.ChashmaStorage),
                        ChashmaLevel = String.Format("{0:0.00}", ObjBalance.ChashmaResLevel),
                        JCOutflow = "",
                        SystemInflow = "",
                        LossGain = "",
                        BalanceInflow = "",
                        ProposedCanalWDL = "",
                        SystemOutflow = "",
                        Shortage = ""
                    };

                }
                lstData.Add(Data);

                foreach (var TDaily in lstTDailyIDs)
                {
                    double? InflowsIT =
                        lstIndusForecastData.Where(q => q.TDailyID == TDaily.TDailyID).FirstOrDefault().Volume;
                    double? CurrLiveContent = GetLiveContentIndus(ObjBalance, lstFillingFraction, TDaily.TDailyID,
                        PreLiveContent, PreReservoirLevel, _Scenario, SeasonID, EKLiveContent);

                    if (TDaily.TDailyID == (int)Constants.SeasonDistribution.EKEnd)
                        EKLiveContent = CurrLiveContent;

                    double? ResLevel = null;

                    if (_Scenario == "Maximum")
                    {
                        if (TDaily.TDailyID > 9 && TDaily.TDailyID < 16)
                        {
                            if (TDaily.TDailyID == 14 || TDaily.TDailyID == 15)
                                ResLevel = PreReservoirLevel;
                            else
                                ResLevel = PreReservoirLevel + 10;
                        }
                        else
                            ResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, CurrLiveContent);
                    }
                    else
                    {
                        if (TDaily.TDailyID > 10 && TDaily.TDailyID < 16)
                        {
                            if (TDaily.TDailyID == 15)
                                ResLevel = PreReservoirLevel;
                            else
                                ResLevel = PreReservoirLevel + 10;
                        }
                        else
                            ResLevel = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, CurrLiveContent);
                    }

                    PreReservoirLevel = ResLevel;
                    double? Outflow = CalculteOutflow(InflowsIT, (long)TDaily.NoOfDays, CurrLiveContent, PreLiveContent);
                    double? StrRel = Outflow - InflowsIT;
                    double? InflowsKN =
                        lstKabulForecastData.Where(q => q.TDailyID == TDaily.TDailyID).FirstOrDefault().Volume;
                    double? LiveContentChashma = GetLivecontentChashma(ObjBalance, TDaily.TDailyID, SeasonID);
                    double? ResLevelChashma = GetReservoirLevel((int)Constants.RimStationsIDs.Chashma,
                        LiveContentChashma);
                    double? StrRelChashma = 0;

                    if (TDaily.TDailyID == 19)
                        StrRelChashma = (PreLiveContentChashma - LiveContentChashma) / Constants.MAFConversion;
                    else
                        StrRelChashma = 0;

                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        // Just to have pression at 3rd decimal places 
                        string SLiveContent = String.Format("{0:0.000}", LiveContentChashma);
                        double? LiveContent = Convert.ToDouble(SLiveContent);
                        string SPreLiveContentChashma = String.Format("{0:0.000}", PreLiveContentChashma);
                        double? PLiveContentChashma = Convert.ToDouble(SPreLiveContentChashma);
                        //

                        if (TDaily.TDailyID == 6 || TDaily.TDailyID == 12 || TDaily.TDailyID == 15)
                            StrRelChashma = (PLiveContentChashma - LiveContent) / (Constants.MAFConversion * 1.1);
                        else
                            StrRelChashma = (PLiveContentChashma - LiveContent) / Constants.MAFConversion;
                    }

                    double? JCOutflow = null;
                    if (lstJCOutflows.ElementAtOrDefault(Iterator).HasValue)
                        JCOutflow = Convert.ToDouble(lstJCOutflows.ElementAtOrDefault(Iterator));
                    double? SystemInflow = Outflow + InflowsKN + StrRelChashma + JCOutflow;
                    double? LossGain = GetLossGain(SystemInflow, ObjBalance, TDaily.TDailyID);
                    double? BalanceInflow = SystemInflow + LossGain;
                    double? ProposedWDLS = GetIndusProposedCanalWDLSKharif(BalanceInflow, lstPara, Iterator, SeasonID,
                        lstFlow7782, TDaily.TDailyID);
                    double? SystemOutflows = BalanceInflow - ProposedWDLS;
                    double? Shortage = null;
                    SP_RefShareDistribution Flow7782 = lstFlow7782.Where(q => q.TDailyID == TDaily.TDailyID).FirstOrDefault();

                    if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.LKStart && TDaily.TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                        Shortage = ((ProposedWDLS / (Flow7782.PunjabPara2 + Flow7782.SindhPara2 + Flow7782.BalochistanShare + Flow7782.KPShare)) * 100) - 100;
                    else
                        Shortage = ((ProposedWDLS / (Flow7782.BalochistanShare + Flow7782.KPShare + Flow7782.PunjabHistoric + Flow7782.SindhHistoric)) * 100) - 100;

                    PreLiveContent = CurrLiveContent;
                    PreLiveContentChashma = LiveContentChashma;
                    Iterator++;

                    //Data = new
                    //{
                    //    ShortName = TDaily.ShortName,
                    //    TDailyID = TDaily.TDailyID,
                    //    TDailyCalID = TDaily.ID,
                    //    Inflows = String.Format("{0:0.0}", InflowsIT),
                    //    StorageRelease = String.Format("{0:0.0}", StrRel),
                    //    LiveContent = String.Format("{0:0.000}", CurrLiveContent),
                    //    ReservoirLevel = String.Format("{0:0.00}", ResLevel),
                    //    Outflow = String.Format("{0:0.0}", Outflow),
                    //    Kabul = String.Format("{0:0.0}", InflowsKN),
                    //    ChashmaStorageRelease = String.Format("{0:0.0}", StrRelChashma),
                    //    ChashmaLiveContent = String.Format("{0:0.000}", LiveContentChashma),
                    //    ChashmaLevel = String.Format("{0:0.00}", ResLevelChashma),
                    //    JCOutflow = String.Format("{0:0.0}", JCOutflow),
                    //    SystemInflow = String.Format("{0:0.0}", SystemInflow),
                    //    LossGain = String.Format("{0:0.0}", LossGain),
                    //    BalanceInflow = String.Format("{0:0.0}", BalanceInflow),
                    //    ProposedCanalWDL = String.Format("{0:0.0}", ProposedWDLS),
                    //    SystemOutflow = String.Format("{0:0.0}", SystemOutflows)
                    //};
                    //lstData.Add(Data);


                    Data = new
                    {
                        ShortName = TDaily.ShortName,
                        TDailyID = TDaily.TDailyID,
                        TDailyCalID = TDaily.ID,
                        Inflows = InflowsIT,
                        StorageRelease = StrRel,
                        LiveContent = CurrLiveContent,
                        ReservoirLevel = ResLevel,
                        Outflow = Outflow,
                        Kabul = InflowsKN,
                        ChashmaStorageRelease = StrRelChashma,
                        ChashmaLiveContent = LiveContentChashma,
                        ChashmaLevel = ResLevelChashma,
                        JCOutflow = JCOutflow,
                        SystemInflow = SystemInflow,
                        LossGain = LossGain,
                        BalanceInflow = BalanceInflow,
                        ProposedCanalWDL = ProposedWDLS,
                        SystemOutflow = SystemOutflows,
                        Shortage = Shortage
                    };
                    lstData.Add(Data);
                }

                if (_CalculateMAF)
                    lstData = CalculateMAF(lstData, (int)Constants.RimStationsIDs.IndusAtTarbela, _Scenario, _SeasonalDraftID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstData;
        }

        public List<object> GetSavedPlan(long _SPDraftID, long _StationID, string _Scenario)
        {
            #region Variables

            List<object> lstData = new List<object>();
            List<object> lstFinalData = new List<object>();
            int SeasonID = Utility.GetCurrentSeasonForView();

            #endregion

            try
            {
                lstData = (from PData in context.SP_PlanData
                           join Cal in context.SP_RefTDailyCalendar on PData.TDailyID equals Cal.TDailyID
                           orderby Cal.TDailyID ascending
                           where Cal.Year == DateTime.Now.Year - 1 && Cal.SeasonID == SeasonID &&
                                 PData.PlanScenarioID == ((from SPSce in context.SP_PlanScenario
                                                           where
                                                           SPSce.PlanDraftID == _SPDraftID && SPSce.StationID == _StationID &&
                                                           SPSce.Scenario == _Scenario
                                                           select SPSce.ID).FirstOrDefault())
                           select new
                           {
                               ShortName = Cal.ShortName,
                               TDailyID = PData.TDailyID,
                               TDailyCalID = PData.TDailyCalendarID,
                               Inflows = PData.Inflow,
                               StorageRelease = PData.StorageRelease,
                               LiveContent = PData.LiveContent,
                               ReservoirLevel = PData.ReservoirLevel,
                               Outflow = PData.Outflow,
                               Chenab = PData.Chenab,
                               Kabul = PData.Kabul,
                               ChashmaStorageRelease = PData.ChashmaStorageRelease,
                               ChashmaLiveContent = PData.ChashmaLiveContent,
                               ChashmaLevel = PData.ChashmaLevel,
                               JCOutflow = PData.JCOutflow,
                               Eastern = PData.Eastern,
                               SystemInflow = PData.SystemInflow,
                               LossGain = PData.LossGain,
                               BalanceInflow = PData.BalanceInflow,
                               ProposedCanalWDL = PData.ProposedCanalWDL,
                               SystemOutflow = PData.SystemOutflow,
                               Shortage = PData.WDLShortage
                           }).ToList<object>();

                SP_PlanBalance ObjBalance = GetBalanceReservoirData(_SPDraftID, _StationID, _Scenario);
                object Previous = new object();
                object Data = new object();
                if (Utility.GetCurrentSeasonForView() == (int)Constants.Seasons.Kharif)
                {
                    Previous = new
                    {
                        ShortName = "Mar3",
                        TDailyID = "",
                        TDailyCalID = "",
                        Inflows = "",
                        StorageRelease = "",
                        LiveContent = String.Format("{0:0.000}", ObjBalance.InitStorage),
                        ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                        Outflow = "",
                        Chenab = "",
                        Kabul = "",
                        Eastern = "",
                        ChashmaStorageRelease = "",
                        ChashmaLiveContent = String.Format("{0:0.000}", ObjBalance.ChashmaStorage),
                        ChashmaLevel = String.Format("{0:0.00}", ObjBalance.ChashmaResLevel),
                        JCOutflow = "",
                        SystemInflow = "",
                        LossGain = "",
                        BalanceInflow = "",
                        ProposedCanalWDL = "",
                        SystemOutflow = "",
                        Shortage = ""
                    };
                }
                else
                {
                    Previous = new
                    {
                        ShortName = "Sep3",
                        TDailyID = "",
                        TDailyCalID = "",
                        Inflows = "",
                        StorageRelease = "",
                        LiveContent = String.Format("{0:0.000}", ObjBalance.InitStorage),
                        ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                        Outflow = "",
                        Chenab = "",
                        Kabul = "",
                        Eastern = "",
                        ChashmaStorageRelease = "",
                        ChashmaLiveContent = String.Format("{0:0.000}", ObjBalance.ChashmaStorage),
                        ChashmaLevel = String.Format("{0:0.00}", ObjBalance.ChashmaResLevel),
                        JCOutflow = "",
                        SystemInflow = "",
                        LossGain = "",
                        BalanceInflow = "",
                        ProposedCanalWDL = "",
                        SystemOutflow = "",
                        Shortage = ""
                    };
                }
                lstFinalData.Add(Previous);
                //  lstFinalData.AddRange(lstData);

                foreach (var obj in lstData)
                {
                    long TDailyID = Convert.ToInt64(obj.GetType().GetProperty("TDailyID").GetValue(obj));
                    long TDailyCalID = Convert.ToInt64(obj.GetType().GetProperty("TDailyCalID").GetValue(obj));
                    double? Inflows = Convert.ToDouble(obj.GetType().GetProperty("Inflows").GetValue(obj));
                    double? StorageRelease = Convert.ToDouble(obj.GetType().GetProperty("StorageRelease").GetValue(obj));
                    double? LiveContent = Convert.ToDouble(obj.GetType().GetProperty("LiveContent").GetValue(obj));
                    double? ResLevel = Convert.ToDouble(obj.GetType().GetProperty("ReservoirLevel").GetValue(obj));
                    double? Outflow = Convert.ToDouble(obj.GetType().GetProperty("Outflow").GetValue(obj));
                    double? SystemInflow = Convert.ToDouble(obj.GetType().GetProperty("SystemInflow").GetValue(obj));
                    double? LossGain = Convert.ToDouble(obj.GetType().GetProperty("LossGain").GetValue(obj));
                    double? BalanceInflow = Convert.ToDouble(obj.GetType().GetProperty("BalanceInflow").GetValue(obj));
                    double? ProposedCanalWDL = Convert.ToDouble(obj.GetType().GetProperty("ProposedCanalWDL").GetValue(obj));
                    double? SystemOutflow = Convert.ToDouble(obj.GetType().GetProperty("SystemOutflow").GetValue(obj));
                    double? Shortage = Convert.ToDouble(obj.GetType().GetProperty("Shortage").GetValue(obj));

                    if (_StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                    {
                        double? Chenab = Convert.ToDouble(obj.GetType().GetProperty("Chenab").GetValue(obj));
                        double? Eastern = Convert.ToDouble(obj.GetType().GetProperty("Eastern").GetValue(obj));

                        //Data = new
                        //{
                        //    ShortName = (obj.GetType().GetProperty("ShortName").GetValue(obj)).ToString(),
                        //    TDailyID = TDailyID,
                        //    Inflows = String.Format("{0:0.0}", Inflows),
                        //    StorageRelease = String.Format("{0:0.0}", StorageRelease),
                        //    LiveContent = String.Format("{0:0.000}", LiveContent),
                        //    ReservoirLevel = String.Format("{0:0.00}", ResLevel),
                        //    Outflow = String.Format("{0:0.0}", Outflow),
                        //    Chenab = String.Format("{0:0.0}", Chenab),
                        //    Eastern = String.Format("{0:0.0}", Eastern),
                        //    SystemInflow = String.Format("{0:0.0}", SystemInflow),
                        //    LossGain = String.Format("{0:0.0}", LossGain),
                        //    BalanceInflow = String.Format("{0:0.0}", BalanceInflow),
                        //    ProposedCanalWDL = String.Format("{0:0.0}", ProposedCanalWDL),
                        //    SystemOutflow = String.Format("{0:0.0}", SystemOutflow)
                        //};


                        Data = new
                        {
                            ShortName = (obj.GetType().GetProperty("ShortName").GetValue(obj)).ToString(),
                            TDailyID = TDailyID,
                            TDailyCalID = TDailyCalID,
                            Inflows = Inflows,
                            StorageRelease = StorageRelease,
                            LiveContent = LiveContent,
                            ReservoirLevel = ResLevel,
                            Outflow = Outflow,
                            Chenab = Chenab,
                            Eastern = Eastern,
                            SystemInflow = SystemInflow,
                            LossGain = LossGain,
                            BalanceInflow = BalanceInflow,
                            ProposedCanalWDL = ProposedCanalWDL,
                            SystemOutflow = SystemOutflow,
                            Shortage = Shortage
                        };

                        lstFinalData.Add(Data);
                    }
                    else // indus plan 
                    {
                        double? Kabul = Convert.ToDouble(obj.GetType().GetProperty("Kabul").GetValue(obj));
                        double? ChashmaStorageRelease = Convert.ToDouble(obj.GetType().GetProperty("ChashmaStorageRelease").GetValue(obj));
                        double? ChashmaLiveContent = Convert.ToDouble(obj.GetType().GetProperty("ChashmaLiveContent").GetValue(obj));
                        double? ChashmaResLevel = Convert.ToDouble(obj.GetType().GetProperty("ChashmaLevel").GetValue(obj));
                        double? JCOutflow = Convert.ToDouble(obj.GetType().GetProperty("JCOutflow").GetValue(obj));

                        //Data = new
                        //{
                        //    ShortName = (obj.GetType().GetProperty("ShortName").GetValue(obj)).ToString(),
                        //    TDailyID = TDailyID,
                        //    Inflows = String.Format("{0:0.0}", Inflows),
                        //    StorageRelease = String.Format("{0:0.0}", StorageRelease),
                        //    LiveContent = String.Format("{0:0.000}", LiveContent),
                        //    ReservoirLevel = String.Format("{0:0.00}", ResLevel),
                        //    Outflow = String.Format("{0:0.0}", Outflow),
                        //    Kabul = String.Format("{0:0.0}", Kabul),
                        //    ChashmaStorageRelease = String.Format("{0:0.0}", ChashmaStorageRelease),
                        //    ChashmaLiveContent = String.Format("{0:0.000}", ChashmaLiveContent),
                        //    ChashmaLevel = String.Format("{0:0.00}", ChashmaResLevel),
                        //    JCOutflow = String.Format("{0:0.0}", JCOutflow),
                        //    SystemInflow = String.Format("{0:0.0}", SystemInflow),
                        //    LossGain = String.Format("{0:0.0}", LossGain),
                        //    BalanceInflow = String.Format("{0:0.0}", BalanceInflow),
                        //    ProposedCanalWDL = String.Format("{0:0.0}", ProposedCanalWDL),
                        //    SystemOutflow = String.Format("{0:0.0}", SystemOutflow)
                        //};

                        Data = new
                        {
                            ShortName = (obj.GetType().GetProperty("ShortName").GetValue(obj)).ToString(),
                            TDailyID = TDailyID,
                            TDailyCalID = TDailyCalID,
                            Inflows = Inflows,
                            StorageRelease = StorageRelease,
                            LiveContent = LiveContent,
                            ReservoirLevel = ResLevel,
                            Outflow = Outflow,
                            Kabul = Kabul,
                            ChashmaStorageRelease = ChashmaStorageRelease,
                            ChashmaLiveContent = ChashmaLiveContent,
                            ChashmaLevel = ChashmaResLevel,
                            JCOutflow = JCOutflow,
                            SystemInflow = SystemInflow,
                            LossGain = LossGain,
                            BalanceInflow = BalanceInflow,
                            ProposedCanalWDL = ProposedCanalWDL,
                            SystemOutflow = SystemOutflow,
                            Shortage = Shortage
                        };

                        lstFinalData.Add(Data);
                    }
                }
                if (lstFinalData.Count > 18)
                    lstFinalData = CalculateMAF(lstFinalData, _StationID, _Scenario, _SPDraftID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstFinalData;
        }

        public List<object> GetLikelyPlanDataIndus(long _SPDraftID, bool _CalculateMAF, bool _FromSave)
        {
            #region Variables
            List<object> lstData = new List<object>();
            List<SP_RefTDailyCalendar> lstTDailyIDs = new List<SP_RefTDailyCalendar>();
            object objData;
            int CurrTDaily = 1;
            int EndTDaily = 18;
            int SeasonID = Utility.GetCurrentSeasonForView();
            double?
               JMInflowEK = 0,
               StrRelEK = 0,
               LiveContentEK = 0,
               OutflowEK = 0,
               KNInflowsEK = 0,
               SystemInflowsEK = 0,
               LossGainEK = 0,
               BalanceInflowEK = 0,
               ProposedWDLSEK = 0,
               SystemOutflowEK = 0,
               JMInflowLK = 0,
               StrRelLK = 0,
               LiveContentLK = 0,
               OutflowLK = 0,
               KNInflowsLK = 0,
               SystemInflowsLK = 0,
               LossGainLK = 0,
               BalanceInflowLK = 0,
               ProposedWDLSLK = 0,
               SystemOutflowLK = 0,
               ChashmaStorageReleaseEK = 0,
               ChashmaStorageReleaseLK = 0,
               ChashmaLiveContentEK = 0,
               ChashmaLiveContentLK = 0,
               JCOutflowEK = 0,
               JCOutflowLK = 0,
               InitialSorage = 0,
               InitialSorageChashma = 0;
            #endregion

            try
            {
                if (SeasonID == (int)Constants.Seasons.Rabi)
                {
                    CurrTDaily = 19;
                    EndTDaily = 36;
                }
                List<SP_RefShareDistribution> lstFlow7782 = GetFlows7782(SeasonID);
                lstTDailyIDs = GetTDailyIDsWithNames(SeasonID);

                List<SP_PlanData> lstMax = (from Sce in context.SP_PlanScenario
                                            join Data in context.SP_PlanData on Sce.ID equals Data.PlanScenarioID
                                            where Sce.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela && Sce.PlanDraftID == _SPDraftID && Sce.Scenario == "Maximum"
                                            select Data).ToList();

                List<SP_PlanData> lstMin = (from Sce in context.SP_PlanScenario
                                            join Data in context.SP_PlanData on Sce.ID equals Data.PlanScenarioID
                                            where Sce.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela && Sce.PlanDraftID == _SPDraftID && Sce.Scenario == "Minimum"
                                            select Data).ToList();

                SP_PlanBalance ObjBalance = (from Sce in context.SP_PlanScenario
                                             join Bal in context.SP_PlanBalance on Sce.ID equals Bal.PlanScenarioID
                                             where Sce.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela && Sce.PlanDraftID == _SPDraftID && Sce.Scenario == "Likely"
                                             select Bal).FirstOrDefault();

                if (lstMax.Count() == 18 && lstMin.Count() == 18 && ObjBalance != null)
                {
                    InitialSorage = ObjBalance.InitStorage;
                    InitialSorageChashma = ObjBalance.ChashmaStorage;

                    if (SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        objData = new
                        {
                            ShortName = "Mar3",
                            Inflows = "",
                            StorageRelease = "",
                            LiveContent = String.Format("{0:0.000}", InitialSorage),
                            ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                            Outflow = "",
                            Kabul = "",
                            ChashmaStorageRelease = "",
                            ChashmaLiveContent = String.Format("{0:0.000}", InitialSorageChashma),
                            ChashmaLevel = String.Format("{0:0.00}", ObjBalance.ChashmaResLevel),
                            JCOutflow = "",
                            SystemInflow = "",
                            LossGain = "",
                            BalanceInflow = "",
                            ProposedCanalWDL = "",
                            SystemOutflow = "",
                            Shortage = ""
                        };
                        lstData.Add(objData);
                    }
                    else
                    {
                        objData = new
                        {
                            ShortName = "Sep3",
                            Inflows = "",
                            StorageRelease = "",
                            LiveContent = String.Format("{0:0.000}", InitialSorage),
                            ReservoirLevel = String.Format("{0:0.00}", ObjBalance.InitResLevel),
                            Outflow = "",
                            Kabul = "",
                            ChashmaStorageRelease = "",
                            ChashmaLiveContent = String.Format("{0:0.000}", InitialSorageChashma),
                            ChashmaLevel = String.Format("{0:0.00}", ObjBalance.ChashmaResLevel),
                            JCOutflow = "",
                            SystemInflow = "",
                            LossGain = "",
                            BalanceInflow = "",
                            ProposedCanalWDL = "",
                            SystemOutflow = "",
                            Shortage = ""
                        };
                        lstData.Add(objData);
                    }

                    for (; CurrTDaily <= EndTDaily; CurrTDaily++)
                    {
                        SP_PlanData ObjMax = lstMax.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault();
                        SP_PlanData ObjMin = lstMin.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault();
                        objData = new object();

                        double? Inflow = (ObjMax.Inflow + ObjMin.Inflow) / 2;
                        double? StorageRelease = (ObjMax.StorageRelease + ObjMin.StorageRelease) / 2;
                        double? LiveContent = (ObjMax.LiveContent + ObjMin.LiveContent) / 2;
                        double? ReservoirLevel = GetReservoirLevel((int)Constants.RimStationsIDs.IndusAtTarbela, LiveContent);
                        double? Outflow = (ObjMax.Outflow + ObjMin.Outflow) / 2;
                        double? Kabul = (ObjMax.Kabul + ObjMin.Kabul) / 2;
                        double? ChashmaStorageRelease = (ObjMax.ChashmaStorageRelease + ObjMin.ChashmaStorageRelease) / 2;
                        double? ChashmaLiveContent = (ObjMax.ChashmaLiveContent + ObjMin.ChashmaLiveContent) / 2;
                        double? ChashmaLevel = (ObjMax.ChashmaLevel + ObjMin.ChashmaLevel) / 2;
                        double? JCOutflow = (ObjMax.JCOutflow + ObjMin.JCOutflow) / 2;
                        double? SystemInflow = (ObjMax.SystemInflow + ObjMin.SystemInflow) / 2;
                        double? LossGain = (ObjMax.LossGain + ObjMin.LossGain) / 2;
                        double? BalanceInflow = (ObjMax.BalanceInflow + ObjMin.BalanceInflow) / 2;
                        double? ProposedCanalWDL = (ObjMax.ProposedCanalWDL + ObjMin.ProposedCanalWDL) / 2;
                        double? SystemOutflow = (ObjMax.SystemOutflow + ObjMin.SystemOutflow) / 2;

                        double? Shortage = null;
                        SP_RefShareDistribution Flow7782 = lstFlow7782.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault();
                        //Shortage = ((ProposedCanalWDL / Flow7782.Jc7782) * 100) - 100;

                        if (CurrTDaily >= (int)Constants.SeasonDistribution.LKStart && CurrTDaily <= (int)Constants.SeasonDistribution.LKEnd)
                            Shortage = ((ProposedCanalWDL / (Flow7782.PunjabPara2 + Flow7782.SindhPara2 + Flow7782.BalochistanShare + Flow7782.KPShare)) * 100) - 100;
                        else
                            Shortage = ((ProposedCanalWDL / (Flow7782.BalochistanShare + Flow7782.KPShare + Flow7782.PunjabHistoric + Flow7782.SindhHistoric)) * 100) - 100;

                        if (_FromSave)
                        {
                            objData = new
                            {
                                ShortName = Utility.GetTDailyShortName((short)ObjMax.TDailyID, Utility.GetCurrentSeasonForView()),
                                TDailyID = lstTDailyIDs.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault().TDailyID,
                                TDailyCalID = lstTDailyIDs.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault().ID,
                                Inflows = Inflow,
                                StorageRelease = StorageRelease,
                                LiveContent = LiveContent,
                                ReservoirLevel = ReservoirLevel,
                                Outflow = Outflow,
                                Kabul = Kabul,
                                ChashmaStorageRelease = ChashmaStorageRelease,
                                ChashmaLiveContent = ChashmaLiveContent,
                                ChashmaLevel = ChashmaLevel,
                                JCOutflow = JCOutflow,
                                SystemInflow = SystemInflow,
                                LossGain = LossGain,
                                BalanceInflow = BalanceInflow,
                                ProposedCanalWDL = ProposedCanalWDL,
                                SystemOutflow = SystemOutflow,
                                Shortage = Shortage
                            };
                            lstData.Add(objData);
                        }
                        else
                        {
                            objData = new
                            {
                                ShortName = Utility.GetTDailyShortName((short)ObjMax.TDailyID, Utility.GetCurrentSeasonForView()),
                                TDailyID = lstTDailyIDs.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault().TDailyID,
                                TDailyCalID = lstTDailyIDs.Where(q => q.TDailyID == CurrTDaily).FirstOrDefault().ID,
                                Inflows = String.Format("{0:0.0}", Inflow),
                                StorageRelease = String.Format("{0:0.0}", StorageRelease),
                                LiveContent = String.Format("{0:0.000}", LiveContent),
                                ReservoirLevel = String.Format("{0:0.00}", ReservoirLevel),
                                Outflow = String.Format("{0:0.0}", Outflow),
                                Kabul = String.Format("{0:0.0}", Kabul),
                                ChashmaStorageRelease = String.Format("{0:0.0}", ChashmaStorageRelease),
                                ChashmaLiveContent = String.Format("{0:0.000}", ChashmaLiveContent),
                                ChashmaLevel = String.Format("{0:0.00}", ChashmaLevel),
                                JCOutflow = String.Format("{0:0.0}", JCOutflow),
                                SystemInflow = String.Format("{0:0.0}", SystemInflow),
                                LossGain = String.Format("{0:0.0}", LossGain),
                                BalanceInflow = String.Format("{0:0.0}", BalanceInflow),
                                ProposedCanalWDL = String.Format("{0:0.0}", ProposedCanalWDL),
                                SystemOutflow = String.Format("{0:0.0}", SystemOutflow),
                                Shortage = String.Format("{0:0.0}", Shortage)
                            };
                            lstData.Add(objData);

                            if (SeasonID == (int)Constants.Seasons.Kharif)
                            {
                                if (CurrTDaily <= (int)Constants.SeasonDistribution.EKEnd)
                                {
                                    if (CurrTDaily == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                                    {
                                        JMInflowEK = JMInflowEK + (Inflow * Constants.TDailyConversion);
                                        KNInflowsEK = KNInflowsEK + (Kabul * Constants.TDailyConversion);
                                        StrRelEK = StrRelEK + (StorageRelease * Constants.TDailyConversion);
                                        OutflowEK = OutflowEK + (Outflow * Constants.TDailyConversion);
                                        ChashmaStorageReleaseEK = ChashmaStorageReleaseEK + (ChashmaStorageRelease * Constants.TDailyConversion);
                                        ChashmaLiveContentEK = ChashmaLiveContentEK + (ChashmaLiveContent * Constants.TDailyConversion);
                                        JCOutflowEK = JCOutflowEK + (JCOutflow * Constants.TDailyConversion);
                                        SystemInflowsEK = SystemInflowsEK + (SystemInflow * Constants.TDailyConversion);
                                        LossGainEK = LossGainEK + (LossGain * Constants.TDailyConversion);
                                        BalanceInflowEK = BalanceInflowEK + (BalanceInflow * Constants.TDailyConversion);
                                        ProposedWDLSEK = ProposedWDLSEK + (ProposedCanalWDL * Constants.TDailyConversion);
                                        SystemOutflowEK = SystemOutflowEK + (SystemOutflow * Constants.TDailyConversion);
                                    }
                                    else
                                    {
                                        JMInflowEK = JMInflowEK + Inflow;
                                        KNInflowsEK = KNInflowsEK + Kabul;
                                        StrRelEK = StrRelEK + StorageRelease;
                                        OutflowEK = OutflowEK + Outflow;
                                        ChashmaStorageReleaseEK = ChashmaStorageReleaseEK + ChashmaStorageRelease;
                                        ChashmaLiveContentEK = ChashmaLiveContentEK + ChashmaLiveContent;
                                        JCOutflowEK = JCOutflowEK + JCOutflow;
                                        SystemInflowsEK = SystemInflowsEK + SystemInflow;
                                        LossGainEK = LossGainEK + LossGain;
                                        BalanceInflowEK = BalanceInflowEK + BalanceInflow;
                                        ProposedWDLSEK = ProposedWDLSEK + ProposedCanalWDL;
                                        SystemOutflowEK = SystemOutflowEK + SystemOutflow;
                                    }

                                    if (CurrTDaily == (int)Constants.SeasonDistribution.EKEnd)
                                    {
                                        JMInflowEK = JMInflowEK * Constants.MAFConversion;
                                        KNInflowsEK = KNInflowsEK * Constants.MAFConversion;
                                        StrRelEK = StrRelEK * Constants.MAFConversion;
                                        LiveContentEK = InitialSorage - LiveContent;
                                        OutflowEK = OutflowEK * Constants.MAFConversion;
                                        ChashmaStorageReleaseEK = ChashmaStorageReleaseEK * Constants.MAFConversion;
                                        ChashmaLiveContentEK = InitialSorageChashma - ChashmaLiveContent;
                                        JCOutflowEK = JCOutflowEK * Constants.MAFConversion;
                                        SystemInflowsEK = SystemInflowsEK * Constants.MAFConversion;
                                        LossGainEK = LossGainEK * Constants.MAFConversion;
                                        BalanceInflowEK = BalanceInflowEK * Constants.MAFConversion;
                                        ProposedWDLSEK = ProposedWDLSEK * Constants.MAFConversion;
                                        SystemOutflowEK = SystemOutflowEK * Constants.MAFConversion;
                                        InitialSorage = LiveContent;
                                        InitialSorageChashma = ChashmaLiveContent;
                                    }
                                }
                                else if (CurrTDaily > (int)Constants.SeasonDistribution.EKEnd && CurrTDaily <= (int)Constants.SeasonDistribution.LKEnd)
                                {
                                    if (CurrTDaily == (int)Constants.TDAilySpecialCases.JulyTDaily || CurrTDaily == (int)Constants.TDAilySpecialCases.AugTDaily)
                                    {
                                        JMInflowLK = JMInflowLK + (Inflow * Constants.TDailyConversion);
                                        KNInflowsLK = KNInflowsLK + (Kabul * Constants.TDailyConversion);
                                        StrRelLK = StrRelLK + (StorageRelease * Constants.TDailyConversion);
                                        OutflowLK = OutflowLK + (Outflow * Constants.TDailyConversion);
                                        ChashmaStorageReleaseLK = ChashmaStorageReleaseLK + (ChashmaStorageRelease * Constants.TDailyConversion);
                                        JCOutflowLK = JCOutflowLK + (JCOutflow * Constants.TDailyConversion);
                                        SystemInflowsLK = SystemInflowsLK + (SystemInflow * Constants.TDailyConversion);
                                        LossGainLK = LossGainLK + (LossGain * Constants.TDailyConversion);
                                        BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.TDailyConversion);
                                        ProposedWDLSLK = ProposedWDLSLK + (ProposedCanalWDL * Constants.TDailyConversion);
                                        SystemOutflowLK = SystemOutflowLK + (SystemOutflow * Constants.TDailyConversion);
                                    }
                                    else
                                    {
                                        JMInflowLK = JMInflowLK + Inflow;
                                        KNInflowsLK = KNInflowsLK + Kabul;
                                        StrRelLK = StrRelLK + StorageRelease;
                                        OutflowLK = OutflowLK + Outflow;
                                        ChashmaStorageReleaseLK = ChashmaStorageReleaseLK + ChashmaStorageRelease;
                                        JCOutflowLK = JCOutflowLK + JCOutflow;
                                        SystemInflowsLK = SystemInflowsLK + SystemInflow;
                                        LossGainLK = LossGainLK + LossGain;
                                        BalanceInflowLK = BalanceInflowLK + BalanceInflow;
                                        ProposedWDLSLK = ProposedWDLSLK + ProposedCanalWDL;
                                        SystemOutflowLK = SystemOutflowLK + SystemOutflow;
                                    }

                                    if (CurrTDaily == (int)Constants.SeasonDistribution.LKEnd)
                                    {
                                        JMInflowLK = JMInflowLK * Constants.MAFConversion;
                                        KNInflowsLK = KNInflowsLK * Constants.MAFConversion;
                                        StrRelLK = StrRelLK * Constants.MAFConversion;
                                        LiveContentLK = InitialSorage - LiveContent;
                                        ChashmaStorageReleaseLK = (double)(Math.Truncate((decimal)(ChashmaStorageReleaseLK * Constants.MAFConversion) * 1000m) / 1000m); //ChashmaStorageReleaseLK * Constants.MAFConversion;
                                        ChashmaLiveContentLK = InitialSorageChashma - ChashmaLiveContent;
                                        JCOutflowLK = JCOutflowLK * Constants.MAFConversion;
                                        OutflowLK = OutflowLK * Constants.MAFConversion;
                                        SystemInflowsLK = SystemInflowsLK * Constants.MAFConversion;
                                        LossGainLK = LossGainLK * Constants.MAFConversion;
                                        BalanceInflowLK = BalanceInflowLK * Constants.MAFConversion;
                                        ProposedWDLSLK = ProposedWDLSLK * Constants.MAFConversion;
                                        SystemOutflowLK = SystemOutflowLK * Constants.MAFConversion;

                                        if (_CalculateMAF)
                                        {
                                            object Data = new
                                            {
                                                ShortName = "EK(MAF)",
                                                TDailyID = "",
                                                TDailyCalID = "",
                                                Inflows = String.Format("{0:0.000}", JMInflowEK),
                                                StorageRelease = String.Format("{0:0.000}", StrRelEK),
                                                LiveContent = String.Format("{0:0.000}", LiveContentEK),
                                                ReservoirLevel = "",
                                                Outflow = String.Format("{0:0.000}", OutflowEK),
                                                Kabul = String.Format("{0:0.000}", KNInflowsEK),
                                                ChashmaStorageRelease = String.Format("{0:0.000}", ChashmaStorageReleaseEK),
                                                ChashmaLiveContent = String.Format("{0:0.000}", ChashmaLiveContentEK),
                                                ChashmaLevel = "",
                                                JCOutflow = String.Format("{0:0.000}", JCOutflowEK),
                                                SystemInflow = String.Format("{0:0.000}", SystemInflowsEK),
                                                LossGain = String.Format("{0:0.000}", LossGainEK),
                                                BalanceInflow = String.Format("{0:0.000}", BalanceInflowEK),
                                                ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSEK),
                                                SystemOutflow = String.Format("{0:0.000}", SystemOutflowEK),
                                                Shortage = ""
                                            };
                                            lstData.Add(Data);

                                            Data = new
                                            {
                                                ShortName = "LK(MAF)",
                                                TDailyID = "",
                                                TDailyCalID = "",
                                                Inflows = String.Format("{0:0.000}", JMInflowLK),
                                                StorageRelease = String.Format("{0:0.000}", StrRelLK),
                                                LiveContent = String.Format("{0:0.000}", LiveContentLK),
                                                ReservoirLevel = "",
                                                Outflow = String.Format("{0:0.000}", OutflowLK),
                                                Kabul = String.Format("{0:0.000}", KNInflowsLK),
                                                ChashmaStorageRelease = String.Format("{0:0.000}", ChashmaStorageReleaseLK),
                                                ChashmaLiveContent = String.Format("{0:0.000}", ChashmaLiveContentLK),
                                                ChashmaLevel = "",
                                                JCOutflow = String.Format("{0:0.000}", JCOutflowLK),
                                                SystemInflow = String.Format("{0:0.000}", SystemInflowsLK),
                                                LossGain = String.Format("{0:0.000}", LossGainLK),
                                                BalanceInflow = String.Format("{0:0.000}", BalanceInflowLK),
                                                ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSLK),
                                                SystemOutflow = String.Format("{0:0.000}", SystemOutflowLK),
                                                Shortage = ""
                                            };
                                            lstData.Add(Data);

                                            Data = new
                                            {
                                                ShortName = "Total(MAF)",
                                                TDailyID = "",
                                                TDailyCalID = "",
                                                Inflows = String.Format("{0:0.000}", (JMInflowEK + JMInflowLK)),
                                                StorageRelease = String.Format("{0:0.000}", (StrRelEK + StrRelLK)),
                                                LiveContent = String.Format("{0:0.000}", (LiveContentEK + LiveContentLK)),
                                                ReservoirLevel = "",
                                                Outflow = String.Format("{0:0.000}", (OutflowEK + OutflowLK)),
                                                Kabul = String.Format("{0:0.000}", (KNInflowsEK + KNInflowsLK)),
                                                ChashmaStorageRelease = String.Format("{0:0.000}", (ChashmaStorageReleaseEK + ChashmaStorageReleaseLK)),
                                                ChashmaLiveContent = String.Format("{0:0.000}", (ChashmaLiveContentEK + ChashmaLiveContentLK)),
                                                ChashmaLevel = "",
                                                JCOutflow = String.Format("{0:0.000}", (JCOutflowEK + JCOutflowLK)),
                                                SystemInflow = String.Format("{0:0.000}", (SystemInflowsEK + SystemInflowsLK)),
                                                LossGain = String.Format("{0:0.000}", (LossGainEK + LossGainLK)),
                                                BalanceInflow = String.Format("{0:0.000}", (BalanceInflowEK + BalanceInflowLK)),
                                                ProposedCanalWDL = String.Format("{0:0.000}", (ProposedWDLSEK + ProposedWDLSLK)),
                                                SystemOutflow = String.Format("{0:0.000}", (SystemOutflowEK + SystemOutflowLK)),
                                                Shortage = ""
                                            };
                                            lstData.Add(Data);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (CurrTDaily == (int)Constants.TDAilySpecialCases.OctTDaily || CurrTDaily == (int)Constants.TDAilySpecialCases.DecTDaily
                                    || CurrTDaily == (int)Constants.TDAilySpecialCases.JanTDaily || CurrTDaily == (int)Constants.TDAilySpecialCases.MarTDaily)
                                {
                                    JMInflowLK = JMInflowLK + (Inflow * Constants.TDailyConversion);
                                    KNInflowsLK = KNInflowsLK + (Kabul * Constants.TDailyConversion);
                                    StrRelLK = StrRelLK + (StorageRelease * Constants.TDailyConversion);
                                    OutflowLK = OutflowLK + (Outflow * Constants.TDailyConversion);
                                    ChashmaStorageReleaseLK = ChashmaStorageReleaseLK + (ChashmaStorageRelease * Constants.TDailyConversion);
                                    JCOutflowLK = JCOutflowLK + (JCOutflow * Constants.TDailyConversion);
                                    SystemInflowsLK = SystemInflowsLK + (SystemInflow * Constants.TDailyConversion);
                                    LossGainLK = LossGainLK + (LossGain * Constants.TDailyConversion);
                                    BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.TDailyConversion);
                                    ProposedWDLSLK = ProposedWDLSLK + (ProposedCanalWDL * Constants.TDailyConversion);
                                    SystemOutflowLK = SystemOutflowLK + (SystemOutflow * Constants.TDailyConversion);
                                }
                                else if (CurrTDaily == (int)Constants.TDAilySpecialCases.FebTDaily)
                                {
                                    if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                    {
                                        JMInflowLK = JMInflowLK + (Inflow * Constants.LeapYearTrue);
                                        KNInflowsLK = KNInflowsLK + (Kabul * Constants.LeapYearTrue);
                                        StrRelLK = StrRelLK + (StorageRelease * Constants.LeapYearTrue);
                                        OutflowLK = OutflowLK + (Outflow * Constants.LeapYearTrue);
                                        ChashmaStorageReleaseLK = ChashmaStorageReleaseLK + (ChashmaStorageRelease * Constants.LeapYearTrue);
                                        JCOutflowLK = JCOutflowLK + (JCOutflow * Constants.LeapYearTrue);
                                        SystemInflowsLK = SystemInflowsLK + (SystemInflow * Constants.LeapYearTrue);
                                        LossGainLK = LossGainLK + (LossGain * Constants.LeapYearTrue);
                                        BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.LeapYearTrue);
                                        ProposedWDLSLK = ProposedWDLSLK + (ProposedCanalWDL * Constants.LeapYearTrue);
                                        SystemOutflowLK = SystemOutflowLK + (SystemOutflow * Constants.LeapYearTrue);
                                    }
                                    else
                                    {
                                        JMInflowLK = JMInflowLK + (Inflow * Constants.LeapYearFalse);
                                        KNInflowsLK = KNInflowsLK + (Kabul * Constants.LeapYearFalse);
                                        StrRelLK = StrRelLK + (StorageRelease * Constants.LeapYearFalse);
                                        OutflowLK = OutflowLK + (Outflow * Constants.LeapYearFalse);
                                        ChashmaStorageReleaseLK = ChashmaStorageReleaseLK + (ChashmaStorageRelease * Constants.LeapYearFalse);
                                        JCOutflowLK = JCOutflowLK + (JCOutflow * Constants.LeapYearFalse);
                                        SystemInflowsLK = SystemInflowsLK + (SystemInflow * Constants.LeapYearFalse);
                                        LossGainLK = LossGainLK + (LossGain * Constants.LeapYearFalse);
                                        BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.LeapYearFalse);
                                        ProposedWDLSLK = ProposedWDLSLK + (ProposedCanalWDL * Constants.LeapYearFalse);
                                        SystemOutflowLK = SystemOutflowLK + (SystemOutflow * Constants.LeapYearFalse);
                                    }
                                }
                                else
                                {
                                    JMInflowLK = JMInflowLK + Inflow;
                                    KNInflowsLK = KNInflowsLK + Kabul;
                                    StrRelLK = StrRelLK + StorageRelease;
                                    OutflowLK = OutflowLK + Outflow;
                                    ChashmaStorageReleaseLK = ChashmaStorageReleaseLK + ChashmaStorageRelease;
                                    JCOutflowLK = JCOutflowLK + JCOutflow;
                                    SystemInflowsLK = SystemInflowsLK + SystemInflow;
                                    LossGainLK = LossGainLK + LossGain;
                                    BalanceInflowLK = BalanceInflowLK + BalanceInflow;
                                    ProposedWDLSLK = ProposedWDLSLK + ProposedCanalWDL;
                                    SystemOutflowLK = SystemOutflowLK + SystemOutflow;
                                }

                                if (CurrTDaily == (int)Constants.SeasonDistribution.RabiEnd)
                                {
                                    JMInflowLK = JMInflowLK * Constants.MAFConversion;
                                    KNInflowsLK = KNInflowsLK * Constants.MAFConversion;
                                    StrRelLK = StrRelLK * Constants.MAFConversion;
                                    LiveContentLK = InitialSorage - LiveContent;
                                    ChashmaStorageReleaseLK = ChashmaStorageReleaseLK * Constants.MAFConversion;
                                    ChashmaLiveContentLK = InitialSorageChashma - ChashmaLiveContent;
                                    JCOutflowLK = JCOutflowLK * Constants.MAFConversion;
                                    OutflowLK = OutflowLK * Constants.MAFConversion;
                                    SystemInflowsLK = SystemInflowsLK * Constants.MAFConversion;
                                    LossGainLK = LossGainLK * Constants.MAFConversion;
                                    BalanceInflowLK = BalanceInflowLK * Constants.MAFConversion;
                                    ProposedWDLSLK = ProposedWDLSLK * Constants.MAFConversion;
                                    SystemOutflowLK = SystemOutflowLK * Constants.MAFConversion;
                                    if (_CalculateMAF)
                                    {
                                        object Data = new
                                        {
                                            ShortName = "Rabi(MAF)",
                                            TDailyID = "",
                                            TDailyCalID = "",
                                            Inflows = String.Format("{0:0.000}", JMInflowLK),
                                            StorageRelease = String.Format("{0:0.000}", StrRelLK),
                                            LiveContent = String.Format("{0:0.000}", LiveContentLK),
                                            ReservoirLevel = "",
                                            Outflow = String.Format("{0:0.000}", OutflowLK),
                                            Kabul = String.Format("{0:0.000}", KNInflowsLK),
                                            ChashmaStorageRelease = String.Format("{0:0.000}", ChashmaStorageReleaseLK),
                                            ChashmaLiveContent = String.Format("{0:0.000}", ChashmaLiveContentLK),
                                            ChashmaLevel = "",
                                            JCOutflow = String.Format("{0:0.000}", JCOutflowLK),
                                            SystemInflow = String.Format("{0:0.000}", SystemInflowsLK),
                                            LossGain = String.Format("{0:0.000}", LossGainLK),
                                            BalanceInflow = String.Format("{0:0.000}", BalanceInflowLK),
                                            ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSLK),
                                            SystemOutflow = String.Format("{0:0.000}", SystemOutflowLK),
                                            Shortage = ""
                                        };
                                        lstData.Add(Data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstData;
        }

        public List<object> CalculateMAF(List<object> _lstData, long _StationID, String _Scenario, long _SeasonalDraftID)
        {
            #region Variables
            List<object> lstfinaldata = new List<object>();
            double?
               JMInflowEK = 0,
               StrRelEK = 0,
               LiveContentEK = 0,
               OutflowEK = 0,
               CMInflowsEK = 0,
               EasternEK = 0,
               SystemInflowsEK = 0,
               LossGainEK = 0,
               BalanceInflowEK = 0,
               ProposedWDLSEK = 0,
               SystemOutflowEK = 0,
               JMInflowLK = 0,
               StrRelLK = 0,
               LiveContentLK = 0,
               OutflowLK = 0,
               CMInflowsLK = 0,
               EasternLK = 0,
               SystemInflowsLK = 0,
               LossGainLK = 0,
               BalanceInflowLK = 0,
               ProposedWDLSLK = 0,
               SystemOutflowLK = 0,
               InitialLiveContent = 0,
               InitialLiveContentChashma = 0,
               InflowsITEK = 0,
               InflowsKNEK = 0,
               StrRelChashmaEK = 0,
               SystemOutflowsEK = 0,
               InflowsITLK = 0,
               InflowsKNLK = 0,
               StrRelChashmaLK = 0,
               SystemOutflowsLK = 0,
               JCOutflowEK = 0,
               JCOutflowLK = 0,
               SystemInflowEK = 0,
               SystemInflowLK = 0,
               LiveContentChashmaEK = 0,
               LiveContentChashmaLK = 0;
            //ShortageEK = 0,
            //ShortageLK = 0;

            SP_PlanBalance Objbalance = new SP_PlanBalance();
            object Data;
            #endregion
            try
            {
                if (_Scenario == "Likely")
                {
                    Objbalance = (from SPSce in context.SP_PlanScenario
                                  join SPBal in context.SP_PlanBalance on SPSce.ID equals SPBal.PlanScenarioID
                                  where SPSce.PlanDraftID == _SeasonalDraftID && SPSce.StationID == _StationID && SPSce.Scenario == _Scenario
                                  select SPBal).FirstOrDefault();
                }

                foreach (var obj in _lstData)
                {
                    // lstfinaldata.Add(obj);
                    string Name = (obj.GetType().GetProperty("ShortName").GetValue(obj)).ToString();

                    if (_StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                    {
                        if ((Name == "Mar3" || Name == "Sep3") && Convert.ToString(obj.GetType().GetProperty("TDailyID").GetValue(obj)) == "")
                        {
                            InitialLiveContent = Convert.ToDouble(obj.GetType().GetProperty("LiveContent").GetValue(obj));
                            double? ReserLevel = Convert.ToDouble(obj.GetType().GetProperty("ReservoirLevel").GetValue(obj));

                            Data = new
                            {
                                ShortName = Name,
                                TDailyID = "",
                                Inflows = "",
                                StorageRelease = "",
                                LiveContent = String.Format("{0:0.000}", InitialLiveContent),
                                ReservoirLevel = String.Format("{0:0.00}", ReserLevel),
                                Outflow = "",
                                Chenab = "",
                                Eastern = "",
                                SystemInflow = "",
                                LossGain = "",
                                BalanceInflow = "",
                                ProposedCanalWDL = "",
                                SystemOutflow = "",
                                Shortage = ""
                            };
                            lstfinaldata.Add(Data);
                            continue;
                        }

                        long TdailyID = Convert.ToInt64(obj.GetType().GetProperty("TDailyID").GetValue(obj));
                        long TDailyCalID = Convert.ToInt64(obj.GetType().GetProperty("TDailyCalID").GetValue(obj));
                        double? JMInflows = Convert.ToDouble(obj.GetType().GetProperty("Inflows").GetValue(obj));
                        double? StrRel = Convert.ToDouble(obj.GetType().GetProperty("StorageRelease").GetValue(obj));
                        double? CurrLiveContent = Convert.ToDouble(obj.GetType().GetProperty("LiveContent").GetValue(obj));
                        double? ResLevel = Convert.ToDouble(obj.GetType().GetProperty("ReservoirLevel").GetValue(obj));
                        double? Outflow = Convert.ToDouble(obj.GetType().GetProperty("Outflow").GetValue(obj));
                        double? CMInflows = Convert.ToDouble(obj.GetType().GetProperty("Chenab").GetValue(obj));
                        double? Eastern = Convert.ToDouble(obj.GetType().GetProperty("Eastern").GetValue(obj));
                        double? SystemInflows = Convert.ToDouble(obj.GetType().GetProperty("SystemInflow").GetValue(obj));
                        double? LossGain = Convert.ToDouble(obj.GetType().GetProperty("LossGain").GetValue(obj));
                        double? BalanceInflow = Convert.ToDouble(obj.GetType().GetProperty("BalanceInflow").GetValue(obj));
                        double? ProposedWDLS = Convert.ToDouble(obj.GetType().GetProperty("ProposedCanalWDL").GetValue(obj));
                        double? SystemOutflows = Convert.ToDouble(obj.GetType().GetProperty("SystemOutflow").GetValue(obj));
                        double? Shortage = Convert.ToDouble(obj.GetType().GetProperty("Shortage").GetValue(obj));

                        Data = new
                       {
                           ShortName = Name,
                           TDailyID = TdailyID,
                           TDailyCalID = TDailyCalID,
                           Inflows = String.Format("{0:0.0}", JMInflows),
                           StorageRelease = String.Format("{0:0.0}", StrRel),
                           LiveContent = String.Format("{0:0.000}", CurrLiveContent),
                           ReservoirLevel = String.Format("{0:0.00}", ResLevel),
                           Outflow = String.Format("{0:0.0}", Outflow),
                           Chenab = String.Format("{0:0.0}", CMInflows),
                           Eastern = String.Format("{0:0.0}", Eastern),
                           SystemInflow = String.Format("{0:0.0}", SystemInflows),
                           LossGain = String.Format("{0:0.0}", LossGain),
                           BalanceInflow = String.Format("{0:0.0}", BalanceInflow),
                           ProposedCanalWDL = String.Format("{0:0.0}", ProposedWDLS),
                           SystemOutflow = String.Format("{0:0.0}", SystemOutflows),
                           Shortage = String.Format("{0:0.0}", Shortage)
                       };
                        lstfinaldata.Add(Data);

                        if (TdailyID <= (int)Constants.SeasonDistribution.EKEnd)
                        {
                            if (TdailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            {
                                JMInflowEK = JMInflowEK + (JMInflows * Constants.TDailyConversion);
                                CMInflowsEK = CMInflowsEK + (CMInflows * Constants.TDailyConversion);
                                StrRelEK = StrRelEK + (StrRel * Constants.TDailyConversion);
                                OutflowEK = OutflowEK + (Outflow * Constants.TDailyConversion);
                                EasternEK = EasternEK + (Eastern * Constants.TDailyConversion);
                                SystemInflowsEK = SystemInflowsEK + (SystemInflows * Constants.TDailyConversion);
                                LossGainEK = LossGainEK + (LossGain * Constants.TDailyConversion);
                                BalanceInflowEK = BalanceInflowEK + (BalanceInflow * Constants.TDailyConversion);
                                ProposedWDLSEK = ProposedWDLSEK + (ProposedWDLS * Constants.TDailyConversion);
                                SystemOutflowEK = SystemOutflowEK + (SystemOutflows * Constants.TDailyConversion);
                                // ShortageEK = ShortageEK + (Shortage * Constants.TDailyConversion);
                            }
                            else
                            {
                                JMInflowEK = JMInflowEK + JMInflows;
                                CMInflowsEK = CMInflowsEK + CMInflows;
                                StrRelEK = StrRelEK + StrRel;
                                OutflowEK = OutflowEK + Outflow;
                                EasternEK = EasternEK + Eastern;
                                SystemInflowsEK = SystemInflowsEK + SystemInflows;
                                LossGainEK = LossGainEK + LossGain;
                                BalanceInflowEK = BalanceInflowEK + BalanceInflow;
                                ProposedWDLSEK = ProposedWDLSEK + ProposedWDLS;
                                SystemOutflowEK = SystemOutflowEK + SystemOutflows;
                                // ShortageEK = ShortageEK + Shortage;
                            }

                            if (TdailyID == (int)Constants.SeasonDistribution.EKEnd)
                            {
                                if (_Scenario == "Likely")
                                {
                                    JMInflowEK = Objbalance.JhelumEK;
                                    CMInflowsEK = Objbalance.ChenabEK;
                                }
                                else
                                {
                                    JMInflowEK = JMInflowEK * Constants.MAFConversion;
                                    CMInflowsEK = CMInflowsEK * Constants.MAFConversion;
                                }
                                StrRelEK = StrRelEK * Constants.MAFConversion;
                                LiveContentEK = InitialLiveContent - CurrLiveContent;
                                OutflowEK = OutflowEK * Constants.MAFConversion;
                                EasternEK = EasternEK * Constants.MAFConversion;
                                SystemInflowsEK = SystemInflowsEK * Constants.MAFConversion;
                                LossGainEK = LossGainEK * Constants.MAFConversion;
                                BalanceInflowEK = BalanceInflowEK * Constants.MAFConversion;
                                ProposedWDLSEK = ProposedWDLSEK * Constants.MAFConversion;
                                SystemOutflowEK = SystemOutflowEK * Constants.MAFConversion;
                                // ShortageEK = ShortageEK * Constants.MAFConversion;
                                InitialLiveContent = CurrLiveContent;
                            }
                        }
                        else if (TdailyID > (int)Constants.SeasonDistribution.EKEnd &&
                                 TdailyID <= (int)Constants.SeasonDistribution.LKEnd)
                        {
                            if (TdailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                                TdailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            {
                                JMInflowLK = JMInflowLK + (JMInflows * Constants.TDailyConversion);
                                CMInflowsLK = CMInflowsLK + (CMInflows * Constants.TDailyConversion);
                                StrRelLK = StrRelLK + (StrRel * Constants.TDailyConversion);
                                OutflowLK = OutflowLK + (Outflow * Constants.TDailyConversion);
                                EasternLK = EasternLK + (Eastern * Constants.TDailyConversion);
                                SystemInflowsLK = SystemInflowsLK + (SystemInflows * Constants.TDailyConversion);
                                LossGainLK = LossGainLK + (LossGain * Constants.TDailyConversion);
                                BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.TDailyConversion);
                                ProposedWDLSLK = ProposedWDLSLK + (ProposedWDLS * Constants.TDailyConversion);
                                SystemOutflowLK = SystemOutflowLK + (SystemOutflows * Constants.TDailyConversion);
                                // ShortageLK = ShortageLK + (Shortage * Constants.TDailyConversion);
                            }
                            else
                            {
                                JMInflowLK = JMInflowLK + JMInflows;
                                CMInflowsLK = CMInflowsLK + CMInflows;
                                StrRelLK = StrRelLK + StrRel;
                                OutflowLK = OutflowLK + Outflow;
                                EasternLK = EasternLK + Eastern;
                                SystemInflowsLK = SystemInflowsLK + SystemInflows;
                                LossGainLK = LossGainLK + LossGain;
                                BalanceInflowLK = BalanceInflowLK + BalanceInflow;
                                ProposedWDLSLK = ProposedWDLSLK + ProposedWDLS;
                                SystemOutflowLK = SystemOutflowLK + SystemOutflows;
                                //  ShortageLK = ShortageLK + Shortage;
                            }

                            if (TdailyID == (int)Constants.SeasonDistribution.LKEnd)
                            {
                                if (_Scenario == "Likely")
                                {
                                    JMInflowLK = Objbalance.JhelumLK;
                                    CMInflowsLK = Objbalance.ChenabLK;
                                }
                                else
                                {
                                    JMInflowLK = (JMInflowLK * Constants.MAFConversion);
                                    CMInflowsLK = (CMInflowsLK * Constants.MAFConversion);
                                }
                                StrRelLK = StrRelLK * Constants.MAFConversion;
                                LiveContentLK = InitialLiveContent - CurrLiveContent;
                                OutflowLK = OutflowLK * Constants.MAFConversion;
                                EasternLK = EasternLK * Constants.MAFConversion;
                                SystemInflowsLK = SystemInflowsLK * Constants.MAFConversion;
                                LossGainLK = LossGainLK * Constants.MAFConversion;
                                BalanceInflowLK = BalanceInflowLK * Constants.MAFConversion;
                                ProposedWDLSLK = ProposedWDLSLK * Constants.MAFConversion;
                                SystemOutflowLK = SystemOutflowLK * Constants.MAFConversion;
                                // ShortageLK = ShortageLK * Constants.MAFConversion;
                            }
                        }
                        else // rabi case
                        {
                            if (TdailyID == (int)Constants.TDAilySpecialCases.OctTDaily
                                || TdailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                                || TdailyID == (int)Constants.TDAilySpecialCases.JanTDaily
                                || TdailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                            {
                                JMInflowLK = JMInflowLK + (JMInflows * Constants.TDailyConversion);
                                CMInflowsLK = CMInflowsLK + (CMInflows * Constants.TDailyConversion);
                                StrRelLK = StrRelLK + (StrRel * Constants.TDailyConversion);
                                OutflowLK = OutflowLK + (Outflow * Constants.TDailyConversion);
                                EasternLK = EasternLK + (Eastern * Constants.TDailyConversion);
                                SystemInflowsLK = SystemInflowsLK + (SystemInflows * Constants.TDailyConversion);
                                LossGainLK = LossGainLK + (LossGain * Constants.TDailyConversion);
                                BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.TDailyConversion);
                                ProposedWDLSLK = ProposedWDLSLK + (ProposedWDLS * Constants.TDailyConversion);
                                SystemOutflowLK = SystemOutflowLK + (SystemOutflows * Constants.TDailyConversion);
                                // ShortageLK = ShortageLK + (Shortage * Constants.TDailyConversion);
                            }
                            else if (TdailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                {
                                    JMInflowLK = JMInflowLK + (JMInflows * Constants.LeapYearTrue);
                                    CMInflowsLK = CMInflowsLK + (CMInflows * Constants.LeapYearTrue);
                                    StrRelLK = StrRelLK + (StrRel * Constants.LeapYearTrue);
                                    OutflowLK = OutflowLK + (Outflow * Constants.LeapYearTrue);
                                    EasternLK = EasternLK + (Eastern * Constants.LeapYearTrue);
                                    SystemInflowsLK = SystemInflowsLK + (SystemInflows * Constants.LeapYearTrue);
                                    LossGainLK = LossGainLK + (LossGain * Constants.LeapYearTrue);
                                    BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.LeapYearTrue);
                                    ProposedWDLSLK = ProposedWDLSLK + (ProposedWDLS * Constants.LeapYearTrue);
                                    SystemOutflowLK = SystemOutflowLK + (SystemOutflows * Constants.LeapYearTrue);
                                    //  ShortageLK = ShortageLK + (Shortage * Constants.LeapYearTrue);
                                }
                                else
                                {
                                    JMInflowLK = JMInflowLK + (JMInflows * Constants.LeapYearFalse);
                                    CMInflowsLK = CMInflowsLK + (CMInflows * Constants.LeapYearFalse);
                                    StrRelLK = StrRelLK + (StrRel * Constants.LeapYearFalse);
                                    OutflowLK = OutflowLK + (Outflow * Constants.LeapYearFalse);
                                    EasternLK = EasternLK + (Eastern * Constants.LeapYearFalse);
                                    SystemInflowsLK = SystemInflowsLK + (SystemInflows * Constants.LeapYearFalse);
                                    LossGainLK = LossGainLK + (LossGain * Constants.LeapYearFalse);
                                    BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.LeapYearFalse);
                                    ProposedWDLSLK = ProposedWDLSLK + (ProposedWDLS * Constants.LeapYearFalse);
                                    SystemOutflowLK = SystemOutflowLK + (SystemOutflows * Constants.LeapYearFalse);
                                    //   ShortageLK = ShortageLK + (Shortage * Constants.LeapYearFalse);
                                }
                            }
                            else
                            {
                                JMInflowLK = JMInflowLK + JMInflows;
                                CMInflowsLK = CMInflowsLK + CMInflows;
                                StrRelLK = StrRelLK + StrRel;
                                OutflowLK = OutflowLK + Outflow;
                                EasternLK = EasternLK + Eastern;
                                SystemInflowsLK = SystemInflowsLK + SystemInflows;
                                LossGainLK = LossGainLK + LossGain;
                                BalanceInflowLK = BalanceInflowLK + BalanceInflow;
                                ProposedWDLSLK = ProposedWDLSLK + ProposedWDLS;
                                SystemOutflowLK = SystemOutflowLK + SystemOutflows;
                                // ShortageLK = ShortageLK + Shortage;
                            }

                            if (TdailyID == (int)Constants.SeasonDistribution.RabiEnd)
                            {
                                if (_Scenario == "Likely")
                                {
                                    JMInflowLK = Objbalance.JhelumRabi;
                                    CMInflowsLK = Objbalance.ChenabRabi;
                                }
                                else
                                {
                                    JMInflowLK = JMInflowLK * Constants.MAFConversion;
                                    CMInflowsLK = CMInflowsLK * Constants.MAFConversion;
                                }
                                StrRelLK = StrRelLK * Constants.MAFConversion;
                                LiveContentLK = InitialLiveContent - CurrLiveContent;
                                OutflowLK = OutflowLK * Constants.MAFConversion;
                                EasternLK = EasternLK * Constants.MAFConversion;
                                SystemInflowsLK = SystemInflowsLK * Constants.MAFConversion;
                                LossGainLK = LossGainLK * Constants.MAFConversion;
                                BalanceInflowLK = BalanceInflowLK * Constants.MAFConversion;
                                ProposedWDLSLK = ProposedWDLSLK * Constants.MAFConversion;
                                SystemOutflowLK = SystemOutflowLK * Constants.MAFConversion;
                                // ShortageLK = ShortageLK + (Shortage * Constants.MAFConversion);
                            }
                        }

                        if (TdailyID == (int)Constants.SeasonDistribution.LKEnd)
                        {
                            Data = new
                            {
                                ShortName = "EK(MAF)",
                                TDailyID = "",
                                TDailyCalID = "",
                                Inflows = String.Format("{0:0.000}", JMInflowEK),
                                StorageRelease = String.Format("{0:0.000}", StrRelEK),
                                LiveContent = String.Format("{0:0.000}", LiveContentEK),
                                ReservoirLevel = "",
                                Outflow = String.Format("{0:0.000}", OutflowEK),
                                Chenab = String.Format("{0:0.000}", CMInflowsEK),
                                Eastern = String.Format("{0:0.000}", EasternEK),
                                SystemInflow = String.Format("{0:0.000}", SystemInflowsEK),
                                LossGain = String.Format("{0:0.000}", LossGainEK),
                                BalanceInflow = String.Format("{0:0.000}", BalanceInflowEK),
                                ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSEK),
                                SystemOutflow = String.Format("{0:0.000}", SystemOutflowEK),
                                Shortage = "" //String.Format("{0:0.000}", ShortageEK)
                            };
                            lstfinaldata.Add(Data);

                            Data = new
                            {
                                ShortName = "LK(MAF)",
                                TDailyID = "",
                                TDailyCalID = "",
                                Inflows = String.Format("{0:0.000}", JMInflowLK),
                                StorageRelease = String.Format("{0:0.000}", StrRelLK),
                                LiveContent = String.Format("{0:0.000}", LiveContentLK),
                                ReservoirLevel = "",
                                Outflow = String.Format("{0:0.000}", OutflowLK),
                                Chenab = String.Format("{0:0.000}", CMInflowsLK),
                                Eastern = String.Format("{0:0.000}", EasternLK),
                                SystemInflow = String.Format("{0:0.000}", SystemInflowsLK),
                                LossGain = String.Format("{0:0.000}", LossGainLK),
                                BalanceInflow = String.Format("{0:0.000}", BalanceInflowLK),
                                ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSLK),
                                SystemOutflow = String.Format("{0:0.000}", SystemOutflowLK),
                                Shortage = "" //String.Format("{0:0.000}", ShortageLK)
                            };
                            lstfinaldata.Add(Data);

                            Data = new
                            {
                                ShortName = "Total(MAF)",
                                TDailyID = "",
                                TDailyCalID = "",
                                Inflows = String.Format("{0:0.000}", (JMInflowEK + JMInflowLK)),
                                StorageRelease = String.Format("{0:0.000}", (StrRelEK + StrRelLK)),
                                LiveContent = String.Format("{0:0.000}", (LiveContentEK + LiveContentLK)),
                                ReservoirLevel = "",
                                Outflow = String.Format("{0:0.000}", (OutflowEK + OutflowLK)),
                                Chenab = String.Format("{0:0.000}", (CMInflowsEK + CMInflowsLK)),
                                Eastern = String.Format("{0:0.000}", (EasternEK + EasternLK)),
                                SystemInflow = String.Format("{0:0.000}", (SystemInflowsEK + SystemInflowsLK)),
                                LossGain = String.Format("{0:0.000}", (LossGainEK + LossGainLK)),
                                BalanceInflow = String.Format("{0:0.000}", (BalanceInflowEK + BalanceInflowLK)),
                                ProposedCanalWDL = String.Format("{0:0.000}", (ProposedWDLSEK + ProposedWDLSLK)),
                                SystemOutflow = String.Format("{0:0.000}", (SystemOutflowEK + SystemOutflowLK)),
                                Shortage = ""// String.Format("{0:0.000}", (ShortageEK + ShortageLK))
                            };
                            lstfinaldata.Add(Data);
                        }
                        else if (TdailyID == (int)Constants.SeasonDistribution.RabiEnd)
                        {
                            Data = new
                           {
                               ShortName = "Rabi(MAF)",
                               TDailyID = "",
                               TDailyCalID = "",
                               Inflows = String.Format("{0:0.000}", JMInflowLK),
                               StorageRelease = String.Format("{0:0.000}", StrRelLK),
                               LiveContent = String.Format("{0:0.000}", (InitialLiveContent - CurrLiveContent)),
                               ReservoirLevel = "",
                               Outflow = String.Format("{0:0.000}", OutflowLK),
                               Chenab = String.Format("{0:0.000}", CMInflowsLK),
                               Eastern = String.Format("{0:0.000}", EasternLK),
                               SystemInflow = String.Format("{0:0.000}", SystemInflowsLK),
                               LossGain = String.Format("{0:0.000}", LossGainLK),
                               BalanceInflow = String.Format("{0:0.000}", BalanceInflowLK),
                               ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSLK),
                               SystemOutflow = String.Format("{0:0.000}", SystemOutflowLK),
                               Shortage = ""// String.Format("{0:0.000}", ShortageLK)
                           };
                            lstfinaldata.Add(Data);
                        }
                    }
                    else // for indus 
                    {
                        if ((Name == "Mar3" || Name == "Sep3") && Convert.ToString(obj.GetType().GetProperty("TDailyID").GetValue(obj)) == "")
                        {
                            InitialLiveContent = Convert.ToDouble(obj.GetType().GetProperty("LiveContent").GetValue(obj));
                            double? ReserLevel = Convert.ToDouble(obj.GetType().GetProperty("ReservoirLevel").GetValue(obj));
                            InitialLiveContentChashma = Convert.ToDouble(obj.GetType().GetProperty("ChashmaLiveContent").GetValue(obj));
                            double? ReserLevelChashma = Convert.ToDouble(obj.GetType().GetProperty("ChashmaLevel").GetValue(obj));

                            Data = new
                            {
                                ShortName = Name,
                                TDailyID = "",
                                TDailyCalID = "",
                                Inflows = "",
                                StorageRelease = "",
                                LiveContent = String.Format("{0:0.000}", InitialLiveContent),
                                ReservoirLevel = String.Format("{0:0.00}", ReserLevel),
                                Outflow = "",
                                Kabul = "",
                                ChashmaStorageRelease = "",
                                ChashmaLiveContent = String.Format("{0:0.000}", InitialLiveContentChashma),
                                ChashmaLevel = String.Format("{0:0.00}", ReserLevelChashma),
                                JCOutflow = "",
                                SystemInflow = "",
                                LossGain = "",
                                BalanceInflow = "",
                                ProposedCanalWDL = "",
                                SystemOutflow = "",
                                Shortage = ""
                            };
                            lstfinaldata.Add(Data);
                            continue;
                        }

                        long TdailyID = Convert.ToInt64(obj.GetType().GetProperty("TDailyID").GetValue(obj));
                        long TDailyCalID = Convert.ToInt64(obj.GetType().GetProperty("TDailyCalID").GetValue(obj));
                        double? InflowsIT = Convert.ToDouble(obj.GetType().GetProperty("Inflows").GetValue(obj));
                        double? StrRel = Convert.ToDouble(obj.GetType().GetProperty("StorageRelease").GetValue(obj));
                        double? CurrLiveContent = Convert.ToDouble(obj.GetType().GetProperty("LiveContent").GetValue(obj));
                        double? ResLevel = Convert.ToDouble(obj.GetType().GetProperty("ReservoirLevel").GetValue(obj));
                        double? Outflow = Convert.ToDouble(obj.GetType().GetProperty("Outflow").GetValue(obj));
                        double? InflowsKN = Convert.ToDouble(obj.GetType().GetProperty("Kabul").GetValue(obj));
                        double? StrRelChashma = Convert.ToDouble(obj.GetType().GetProperty("ChashmaStorageRelease").GetValue(obj));
                        double? LiveContentChashma = Convert.ToDouble(obj.GetType().GetProperty("ChashmaLiveContent").GetValue(obj));
                        double? ResLevelChashma = Convert.ToDouble(obj.GetType().GetProperty("ChashmaLevel").GetValue(obj));
                        double? JCOutflow = Convert.ToDouble(obj.GetType().GetProperty("JCOutflow").GetValue(obj));
                        double? SystemInflow = Convert.ToDouble(obj.GetType().GetProperty("SystemInflow").GetValue(obj));
                        double? LossGain = Convert.ToDouble(obj.GetType().GetProperty("LossGain").GetValue(obj));
                        double? BalanceInflow = Convert.ToDouble(obj.GetType().GetProperty("BalanceInflow").GetValue(obj));
                        double? ProposedWDLS = Convert.ToDouble(obj.GetType().GetProperty("ProposedCanalWDL").GetValue(obj));
                        double? SystemOutflows = Convert.ToDouble(obj.GetType().GetProperty("SystemOutflow").GetValue(obj));
                        double? Shortage = Convert.ToDouble(obj.GetType().GetProperty("Shortage").GetValue(obj));

                        Data = new
                        {
                            ShortName = Name,
                            TDailyID = TdailyID,
                            TDailyCalID = TDailyCalID,
                            Inflows = String.Format("{0:0.0}", InflowsIT),
                            StorageRelease = String.Format("{0:0.0}", StrRel),
                            LiveContent = String.Format("{0:0.000}", CurrLiveContent),
                            ReservoirLevel = String.Format("{0:0.00}", ResLevel),
                            Outflow = String.Format("{0:0.0}", Outflow),
                            Kabul = String.Format("{0:0.0}", InflowsKN),
                            ChashmaStorageRelease = String.Format("{0:0.0}", StrRelChashma),
                            ChashmaLiveContent = String.Format("{0:0.000}", LiveContentChashma),
                            ChashmaLevel = String.Format("{0:0.00}", ResLevelChashma),
                            JCOutflow = String.Format("{0:0.0}", JCOutflow),
                            SystemInflow = String.Format("{0:0.0}", SystemInflow),
                            LossGain = String.Format("{0:0.0}", LossGain),
                            BalanceInflow = String.Format("{0:0.0}", BalanceInflow),
                            ProposedCanalWDL = String.Format("{0:0.0}", ProposedWDLS),
                            SystemOutflow = String.Format("{0:0.0}", SystemOutflows),
                            Shortage = String.Format("{0:0.0}", Shortage)
                        };
                        lstfinaldata.Add(Data);

                        if (TdailyID <= (int)Constants.SeasonDistribution.EKEnd)
                        {
                            if (TdailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            {
                                InflowsITEK += (InflowsIT * Constants.TDailyConversion);
                                InflowsKNEK += (InflowsKN * Constants.TDailyConversion);
                                StrRelEK = StrRelEK + (StrRel * Constants.TDailyConversion);
                                OutflowEK = OutflowEK + (Outflow * Constants.TDailyConversion);
                                StrRelChashmaEK = StrRelChashmaEK + (StrRelChashma * Constants.TDailyConversion);
                                JCOutflowEK = JCOutflowEK + (JCOutflow * Constants.TDailyConversion);
                                SystemInflowEK = SystemInflowEK + (SystemInflow * Constants.TDailyConversion);
                                LossGainEK = LossGainEK + (LossGain * Constants.TDailyConversion);
                                BalanceInflowEK = BalanceInflowEK + (BalanceInflow * Constants.TDailyConversion);
                                ProposedWDLSEK = ProposedWDLSEK + (ProposedWDLS * Constants.TDailyConversion);
                                SystemOutflowsEK = SystemOutflowsEK + (SystemOutflows * Constants.TDailyConversion);
                                //    ShortageEK = ShortageEK + (Shortage * Constants.TDailyConversion);
                            }
                            else
                            {
                                InflowsITEK += InflowsIT;
                                InflowsKNEK += InflowsKN;
                                StrRelEK = StrRelEK + StrRel;
                                OutflowEK = OutflowEK + Outflow;
                                StrRelChashmaEK = StrRelChashmaEK + StrRelChashma;
                                JCOutflowEK = JCOutflowEK + JCOutflow;
                                SystemInflowEK = SystemInflowEK + SystemInflow;
                                LossGainEK = LossGainEK + LossGain;
                                BalanceInflowEK = BalanceInflowEK + BalanceInflow;
                                ProposedWDLSEK = ProposedWDLSEK + ProposedWDLS;
                                SystemOutflowsEK = SystemOutflowsEK + SystemOutflows;
                                //    ShortageEK = ShortageEK + Shortage;
                            }

                            if (TdailyID == (int)Constants.SeasonDistribution.EKEnd)
                            {
                                if (_Scenario == "Likely")
                                {
                                    InflowsITEK = Objbalance.IndusEK;
                                    InflowsKNEK = Objbalance.KabulEK;
                                }
                                else
                                {
                                    InflowsITEK = InflowsITEK * Constants.MAFConversion;
                                    InflowsKNEK = InflowsKNEK * Constants.MAFConversion;
                                }
                                StrRelEK = StrRelEK * Constants.MAFConversion;
                                LiveContentEK = InitialLiveContent - CurrLiveContent;
                                OutflowEK = OutflowEK * Constants.MAFConversion;

                                StrRelChashmaEK = StrRelChashmaEK * Constants.MAFConversion;
                                LiveContentChashmaEK = InitialLiveContentChashma - LiveContentChashma;
                                JCOutflowEK = JCOutflowEK * Constants.MAFConversion;
                                SystemInflowEK = SystemInflowEK * Constants.MAFConversion;
                                LossGainEK = LossGainEK * Constants.MAFConversion;
                                BalanceInflowEK = BalanceInflowEK * Constants.MAFConversion;
                                ProposedWDLSEK = ProposedWDLSEK * Constants.MAFConversion;
                                SystemOutflowsEK = SystemOutflowsEK * Constants.MAFConversion;
                                // ShortageEK = ShortageEK * Constants.MAFConversion;
                                InitialLiveContent = CurrLiveContent;
                                InitialLiveContentChashma = LiveContentChashma;
                            }
                        }
                        else if (TdailyID > (int)Constants.SeasonDistribution.EKEnd &&
                                TdailyID <= (int)Constants.SeasonDistribution.LKEnd)
                        {
                            if (TdailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                                TdailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            {
                                InflowsITLK += (InflowsIT * Constants.TDailyConversion);
                                InflowsKNLK += (InflowsKN * Constants.TDailyConversion);
                                StrRelLK = StrRelLK + (StrRel * Constants.TDailyConversion);
                                OutflowLK = OutflowLK + (Outflow * Constants.TDailyConversion);
                                StrRelChashmaLK = StrRelChashmaLK + (StrRelChashma * Constants.TDailyConversion);
                                JCOutflowLK = JCOutflowLK + (JCOutflow * Constants.TDailyConversion);
                                SystemInflowLK = SystemInflowLK + (SystemInflow * Constants.TDailyConversion);
                                LossGainLK = LossGainLK + (LossGain * Constants.TDailyConversion);
                                BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.TDailyConversion);
                                ProposedWDLSLK = ProposedWDLSLK + (ProposedWDLS * Constants.TDailyConversion);
                                SystemOutflowsLK = SystemOutflowsLK + (SystemOutflows * Constants.TDailyConversion);
                                //  ShortageLK = ShortageLK + (Shortage * Constants.TDailyConversion);
                            }
                            else
                            {
                                InflowsITLK += InflowsIT;
                                InflowsKNLK += InflowsKN;
                                StrRelLK = StrRelLK + StrRel;
                                LiveContentLK = LiveContentLK + CurrLiveContent;
                                OutflowLK = OutflowLK + Outflow;
                                StrRelChashmaLK = StrRelChashmaLK + StrRelChashma;
                                // LiveContentChashmaLK = LiveContentChashmaLK + LiveContentChashma;
                                JCOutflowLK = JCOutflowLK + JCOutflow;
                                SystemInflowLK = SystemInflowLK + SystemInflow;
                                LossGainLK = LossGainLK + LossGain;
                                BalanceInflowLK = BalanceInflowLK + BalanceInflow;
                                ProposedWDLSLK = ProposedWDLSLK + ProposedWDLS;
                                SystemOutflowsLK = SystemOutflowsLK + SystemOutflows;
                                //   ShortageLK = ShortageLK + Shortage;
                            }

                            if (TdailyID == (int)Constants.SeasonDistribution.LKEnd)
                            {
                                if (_Scenario == "Likely")
                                {
                                    InflowsITLK = Objbalance.IndusLK;
                                    InflowsKNLK = Objbalance.KabulLK;
                                }
                                else
                                {
                                    InflowsITLK = (InflowsITLK * Constants.MAFConversion);
                                    InflowsKNLK = (InflowsKNLK * Constants.MAFConversion);
                                }
                                StrRelLK = StrRelLK * Constants.MAFConversion;
                                LiveContentLK = InitialLiveContent - CurrLiveContent;
                                OutflowLK = OutflowLK * Constants.MAFConversion;
                                StrRelChashmaLK = StrRelChashmaLK * Constants.MAFConversion;
                                LiveContentChashmaLK = InitialLiveContentChashma - LiveContentChashma;
                                JCOutflowLK = JCOutflowLK * Constants.MAFConversion;
                                SystemInflowLK = SystemInflowLK * Constants.MAFConversion;
                                LossGainLK = LossGainLK * Constants.MAFConversion;
                                BalanceInflowLK = BalanceInflowLK * Constants.MAFConversion;
                                ProposedWDLSLK = ProposedWDLSLK * Constants.MAFConversion;
                                SystemOutflowsLK = SystemOutflowsLK * Constants.MAFConversion;
                                //  ShortageLK = ShortageLK * Constants.MAFConversion;

                                Data = new
                                {
                                    ShortName = "EK(MAF)",
                                    TDailyID = "",
                                    TDailyCalID = "",
                                    Inflows = String.Format("{0:0.000}", InflowsITEK),
                                    StorageRelease = String.Format("{0:0.000}", StrRelEK),
                                    LiveContent = String.Format("{0:0.000}", LiveContentEK),
                                    ReservoirLevel = "",
                                    Outflow = String.Format("{0:0.000}", OutflowEK),
                                    Kabul = String.Format("{0:0.000}", InflowsKNEK),
                                    ChashmaStorageRelease = String.Format("{0:0.000}", StrRelChashmaEK),
                                    ChashmaLiveContent = String.Format("{0:0.000}", LiveContentChashmaEK),
                                    ChashmaLevel = "",
                                    JCOutflow = String.Format("{0:0.000}", JCOutflowEK),
                                    SystemInflow = String.Format("{0:0.000}", SystemInflowEK),
                                    LossGain = String.Format("{0:0.000}", LossGainEK),
                                    BalanceInflow = String.Format("{0:0.000}", BalanceInflowEK),
                                    ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSEK),
                                    SystemOutflow = String.Format("{0:0.000}", SystemOutflowsEK),
                                    Shortage = ""// String.Format("{0:0.000}", ShortageEK)
                                };
                                lstfinaldata.Add(Data);

                                Data = new
                                {
                                    ShortName = "LK(MAF)",
                                    TDailyID = "",
                                    TDailyCalID = "",
                                    Inflows = String.Format("{0:0.000}", InflowsITLK),
                                    StorageRelease = String.Format("{0:0.000}", StrRelLK),
                                    LiveContent = String.Format("{0:0.000}", LiveContentLK),
                                    ReservoirLevel = "",
                                    Outflow = String.Format("{0:0.000}", OutflowLK),
                                    Kabul = String.Format("{0:0.000}", InflowsKNLK),
                                    ChashmaStorageRelease = String.Format("{0:0.000}", StrRelChashmaLK),
                                    ChashmaLiveContent = String.Format("{0:0.000}", LiveContentChashmaLK),
                                    ChashmaLevel = "",
                                    JCOutflow = String.Format("{0:0.000}", JCOutflowLK),
                                    SystemInflow = String.Format("{0:0.000}", SystemInflowLK),
                                    LossGain = String.Format("{0:0.000}", LossGainLK),
                                    BalanceInflow = String.Format("{0:0.000}", BalanceInflowLK),
                                    ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSLK),
                                    SystemOutflow = String.Format("{0:0.000}", SystemOutflowsLK),
                                    Shortage = ""// String.Format("{0:0.000}", ShortageLK)
                                };
                                lstfinaldata.Add(Data);

                                Data = new
                                {
                                    ShortName = "Total(MAF)",
                                    TDailyID = "",
                                    TDailyCalID = "",
                                    Inflows = String.Format("{0:0.000}", (InflowsITEK + InflowsITLK)),
                                    StorageRelease = String.Format("{0:0.000}", (StrRelEK + StrRelLK)),
                                    LiveContent = String.Format("{0:0.000}", (LiveContentEK + LiveContentLK)),
                                    ReservoirLevel = "",
                                    Outflow = String.Format("{0:0.000}", (OutflowEK + OutflowLK)),
                                    Kabul = String.Format("{0:0.000}", (InflowsKNEK + InflowsKNLK)),
                                    ChashmaStorageRelease = String.Format("{0:0.000}", (StrRelChashmaEK + StrRelChashmaLK)),
                                    ChashmaLiveContent = String.Format("{0:0.000}", (LiveContentChashmaEK + LiveContentChashmaLK)),
                                    ChashmaLevel = "",
                                    JCOutflow = String.Format("{0:0.000}", (JCOutflowEK + JCOutflowLK)),
                                    SystemInflow = String.Format("{0:0.000}", (SystemInflowEK + SystemInflowLK)),
                                    LossGain = String.Format("{0:0.000}", (LossGainEK + LossGainLK)),
                                    BalanceInflow = String.Format("{0:0.000}", (BalanceInflowEK + BalanceInflowLK)),
                                    ProposedCanalWDL = String.Format("{0:0.000}", (ProposedWDLSEK + ProposedWDLSLK)),
                                    SystemOutflow = String.Format("{0:0.000}", (SystemOutflowsEK + SystemOutflowsLK)),
                                    Shortage = ""// String.Format("{0:0.000}", (ShortageEK + ShortageLK))
                                };
                                lstfinaldata.Add(Data);
                            }
                        }
                        else if (TdailyID > (int)Constants.SeasonDistribution.LKEnd)
                        {
                            if (TdailyID == (int)Constants.TDAilySpecialCases.OctTDaily
                                || TdailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                                || TdailyID == (int)Constants.TDAilySpecialCases.JanTDaily
                                || TdailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                            {
                                InflowsITLK = InflowsITLK + (InflowsIT * Constants.TDailyConversion);
                                StrRelLK = StrRelLK + (StrRel * Constants.TDailyConversion);
                                OutflowLK = OutflowLK + (Outflow * Constants.TDailyConversion);
                                InflowsKNLK = InflowsKNLK + (InflowsKN * Constants.TDailyConversion);
                                StrRelChashmaLK = StrRelChashmaLK + (StrRelChashma * Constants.TDailyConversion);
                                JCOutflowLK = JCOutflowLK + (JCOutflow * Constants.TDailyConversion);
                                SystemInflowLK = SystemInflowLK + (SystemInflow * Constants.TDailyConversion);
                                LossGainLK = LossGainLK + (LossGain * Constants.TDailyConversion);
                                BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.TDailyConversion);
                                ProposedWDLSLK = ProposedWDLSLK + (ProposedWDLS * Constants.TDailyConversion);
                                SystemOutflowsLK = SystemOutflowsLK + (SystemOutflows * Constants.TDailyConversion);
                                //   ShortageLK = ShortageLK + (Shortage * Constants.TDailyConversion);

                            }
                            else if (TdailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                {
                                    InflowsITLK = InflowsITLK + (InflowsIT * Constants.LeapYearTrue);
                                    StrRelLK = StrRelLK + (StrRel * Constants.LeapYearTrue);
                                    OutflowLK = OutflowLK + (Outflow * Constants.LeapYearTrue);
                                    InflowsKNLK = InflowsKNLK + (InflowsKN * Constants.LeapYearTrue);
                                    StrRelChashmaLK = StrRelChashmaLK + (StrRelChashma * Constants.LeapYearTrue);
                                    JCOutflowLK = JCOutflowLK + (JCOutflow * Constants.LeapYearTrue);
                                    SystemInflowLK = SystemInflowLK + (SystemInflow * Constants.LeapYearTrue);
                                    LossGainLK = LossGainLK + (LossGain * Constants.LeapYearTrue);
                                    BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.LeapYearTrue);
                                    ProposedWDLSLK = ProposedWDLSLK + (ProposedWDLS * Constants.LeapYearTrue);
                                    SystemOutflowsLK = SystemOutflowsLK + (SystemOutflows * Constants.LeapYearTrue);
                                    // ShortageLK = ShortageLK + (Shortage * Constants.LeapYearTrue);
                                }
                                else
                                {
                                    InflowsITLK = InflowsITLK + (InflowsIT * Constants.LeapYearFalse);
                                    StrRelLK = StrRelLK + (StrRel * Constants.LeapYearFalse);
                                    OutflowLK = OutflowLK + (Outflow * Constants.LeapYearFalse);
                                    InflowsKNLK = InflowsKNLK + (InflowsKN * Constants.LeapYearFalse);
                                    StrRelChashmaLK = StrRelChashmaLK + (StrRelChashma * Constants.LeapYearFalse);
                                    JCOutflowLK = JCOutflowLK + (JCOutflow * Constants.LeapYearFalse);
                                    SystemInflowLK = SystemInflowLK + (SystemInflow * Constants.LeapYearFalse);
                                    LossGainLK = LossGainLK + (LossGain * Constants.LeapYearFalse);
                                    BalanceInflowLK = BalanceInflowLK + (BalanceInflow * Constants.LeapYearFalse);
                                    ProposedWDLSLK = ProposedWDLSLK + (ProposedWDLS * Constants.LeapYearFalse);
                                    SystemOutflowsLK = SystemOutflowsLK + (SystemOutflows * Constants.LeapYearFalse);
                                    //    ShortageLK = ShortageLK + (Shortage * Constants.LeapYearFalse);
                                }
                            }
                            else
                            {
                                InflowsITLK = InflowsITLK + InflowsIT;
                                StrRelLK = StrRelLK + StrRel;
                                OutflowLK = OutflowLK + Outflow;
                                InflowsKNLK = InflowsKNLK + InflowsKN;
                                StrRelChashmaLK = StrRelChashmaLK + StrRelChashma;
                                JCOutflowLK = JCOutflowLK + JCOutflow;
                                SystemInflowLK = SystemInflowLK + SystemInflow;
                                LossGainLK = LossGainLK + LossGain;
                                BalanceInflowLK = BalanceInflowLK + BalanceInflow;
                                ProposedWDLSLK = ProposedWDLSLK + ProposedWDLS;
                                SystemOutflowsLK = SystemOutflowsLK + SystemOutflows;
                                //   ShortageLK = ShortageLK + Shortage;
                            }

                            if (TdailyID == (int)Constants.SeasonDistribution.RabiEnd)
                            {
                                if (_Scenario == "Likely")
                                {
                                    InflowsITLK = Objbalance.IndusRabi;
                                    InflowsKNLK = Objbalance.KabulRabi;
                                }
                                else
                                {
                                    InflowsITLK = InflowsITLK * Constants.MAFConversion;
                                    InflowsKNLK = InflowsKNLK * Constants.MAFConversion;
                                }
                                StrRelLK = StrRelLK * Constants.MAFConversion;
                                LiveContentLK = CurrLiveContent;
                                OutflowLK = OutflowLK * Constants.MAFConversion;
                                StrRelChashmaLK = StrRelChashmaLK * Constants.MAFConversion;
                                LiveContentChashmaLK = LiveContentChashma;
                                JCOutflowLK = JCOutflowLK * Constants.MAFConversion;
                                SystemInflowLK = SystemInflowLK * Constants.MAFConversion;
                                LossGainLK = LossGainLK * Constants.MAFConversion;
                                BalanceInflowLK = BalanceInflowLK * Constants.MAFConversion;
                                ProposedWDLSLK = ProposedWDLSLK * Constants.MAFConversion;
                                SystemOutflowsLK = SystemOutflowsLK * Constants.MAFConversion;
                                //  ShortageLK = ShortageLK + (Shortage * Constants.MAFConversion);

                                Data = new
                                {
                                    ShortName = "Rabi(MAF)",
                                    TDailyID = "",
                                    TDailyCalID = "",
                                    Inflows = String.Format("{0:0.000}", InflowsITLK),
                                    StorageRelease = String.Format("{0:0.000}", StrRelLK),
                                    LiveContent = String.Format("{0:0.000}", (InitialLiveContent - LiveContentLK)),
                                    ReservoirLevel = "",
                                    Outflow = String.Format("{0:0.000}", OutflowLK),
                                    Kabul = String.Format("{0:0.000}", InflowsKNLK),
                                    ChashmaStorageRelease = String.Format("{0:0.000}", StrRelChashmaLK),
                                    ChashmaLiveContent =
                                    String.Format("{0:0.000}", (InitialLiveContentChashma - LiveContentChashmaLK)),
                                    ChashmaLevel = "",
                                    JCOutflow = String.Format("{0:0.000}", JCOutflowLK),
                                    SystemInflow = String.Format("{0:0.000}", SystemInflowLK),
                                    LossGain = String.Format("{0:0.000}", LossGainLK),
                                    BalanceInflow = String.Format("{0:0.000}", BalanceInflowLK),
                                    ProposedCanalWDL = String.Format("{0:0.000}", ProposedWDLSLK),
                                    SystemOutflow = String.Format("{0:0.000}", SystemOutflowsLK),
                                    Shortage = ""// String.Format("{0:0.000}", ShortageLK)
                                };
                                lstfinaldata.Add(Data);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstfinaldata;
        }

        #endregion

        #region AnticipatedJC

        public object AnticipatedData(long _ForecastDraftID, long _PlanDraftID, long _StationID, string _Scenario)
        {
            List<SP_ForecastData> lstSavedValues = new List<SP_ForecastData>();
            List<SP_ForecastData> lstTDaily = new List<SP_ForecastData>();
            SP_PlanBalance Objbalance = new SP_PlanBalance();
            double? JhelumAtManglaEK = 0;
            double? ChenabAtMaralaEK = 0;
            double? IndusTarbelaEK = 0;
            double? KabulNowsheraEK = 0;
            double? JhelumAtManglaLK = 0;
            double? ChenabAtMaralaLK = 0;
            double? IndusTarbelaLK = 0;
            double? KabulNowsheraLK = 0;

            object lstScenario = null;
            try
            {
                if (_Scenario == "Likely")
                {
                    if (_StationID != -1)
                    {
                        Objbalance = (from SPSce in context.SP_PlanScenario
                                      join SPBal in context.SP_PlanBalance on SPSce.ID equals SPBal.PlanScenarioID
                                      where SPSce.PlanDraftID == _PlanDraftID && SPSce.StationID == _StationID && SPSce.Scenario == _Scenario
                                      select SPBal).FirstOrDefault();

                        if (Objbalance != null)
                        {
                            JhelumAtManglaEK = Objbalance.JhelumEK;
                            JhelumAtManglaLK = Objbalance.JhelumLK;
                            ChenabAtMaralaEK = Objbalance.ChenabEK;
                            ChenabAtMaralaLK = Objbalance.ChenabLK;
                            IndusTarbelaEK = Objbalance.IndusEK;
                            IndusTarbelaLK = Objbalance.IndusLK;
                            KabulNowsheraEK = Objbalance.KabulEK;
                            KabulNowsheraLK = Objbalance.KabulLK;

                        }
                    }
                    else
                    {
                        List<SP_PlanBalance> lstBalance = (from SPSce in context.SP_PlanScenario
                                                           join SPBal in context.SP_PlanBalance on SPSce.ID equals SPBal.PlanScenarioID
                                                           where SPSce.PlanDraftID == _PlanDraftID && SPSce.Scenario == _Scenario
                                                           && (SPSce.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela ||
                                                           SPSce.StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                                                           select SPBal).ToList();

                        Objbalance = lstBalance.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault();
                        if (Objbalance != null)
                        {
                            JhelumAtManglaEK = Objbalance.JhelumEK;
                            JhelumAtManglaLK = Objbalance.JhelumLK;
                            ChenabAtMaralaEK = Objbalance.ChenabEK;
                            ChenabAtMaralaLK = Objbalance.ChenabLK;
                        }

                        Objbalance = lstBalance.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault();
                        if (Objbalance != null)
                        {
                            IndusTarbelaEK = Objbalance.IndusEK;
                            IndusTarbelaLK = Objbalance.IndusLK;
                            KabulNowsheraEK = Objbalance.KabulEK;
                            KabulNowsheraLK = Objbalance.KabulLK;
                        }
                    }

                    //List<dynamic> lstObjbalance = (from SPSce in context.SP_PlanScenario
                    //                               join SPBal in context.SP_PlanBalance on SPSce.ID equals SPBal.PlanScenarioID
                    //                               where SPSce.PlanDraftID == _PlanDraftID
                    //                               && SPSce.StationID == _StationID
                    //                               && (SPSce.Scenario == "Maximum" || SPSce.Scenario == "Minimum")
                    //                               select new
                    //                               {
                    //                                   Scenario = SPSce.Scenario,
                    //                                   JhelumAtManglaEK = SPBal.JhelumEK,
                    //                                   JhelumAtManglaLK = SPBal.JhelumLK,
                    //                                   ChenabAtMaralaEK = SPBal.ChenabEK,
                    //                                   ChenabAtMaralaLK = SPBal.ChenabLK,
                    //                                   IndusTarbelaEK = SPBal.IndusEK,
                    //                                   IndusTarbelaLK = SPBal.IndusLK,
                    //                                   KabulNowsheraEK = SPBal.KabulEK,
                    //                                   KabulNowsheraLK = SPBal.KabulLK
                    //                               }).ToList<dynamic>();

                    //dynamic ObjMax = lstObjbalance.Where(q => q.Scenario == "Maximum").FirstOrDefault();

                    //dynamic ObjMin = lstObjbalance.Where(q => q.Scenario == "Minimum").FirstOrDefault();

                    //if (ObjMax != null && ObjMin != null)
                    //{
                    //    JhelumAtManglaEK = ((ObjMax.JhelumAtManglaEK == null ? 0 : ObjMax.JhelumAtManglaEK) + (ObjMin.JhelumAtManglaEK == null ? 0 : ObjMin.JhelumAtManglaEK)) / 2; // Objbalance.JhelumEK;
                    //    JhelumAtManglaLK = ((ObjMax.JhelumAtManglaLK == null ? 0 : ObjMax.JhelumAtManglaLK) + (ObjMin.JhelumAtManglaLK == null ? 0 : ObjMin.JhelumAtManglaLK)) / 2;//Objbalance.JhelumLK;
                    //    ChenabAtMaralaEK = ((ObjMax.ChenabAtMaralaEK == null ? 0 : ObjMax.ChenabAtMaralaEK) + (ObjMin.ChenabAtMaralaEK == null ? 0 : ObjMin.ChenabAtMaralaEK)) / 2;//Objbalance.ChenabEK;
                    //    ChenabAtMaralaLK = ((ObjMax.ChenabAtMaralaLK == null ? 0 : ObjMax.ChenabAtMaralaLK) + (ObjMin.ChenabAtMaralaLK == null ? 0 : ObjMin.ChenabAtMaralaLK)) / 2;//Objbalance.ChenabLK;
                    //    IndusTarbelaEK = ((ObjMax.IndusTarbelaEK == null ? 0 : ObjMax.IndusTarbelaEK) + (ObjMin.IndusTarbelaEK == null ? 0 : ObjMin.IndusTarbelaEK)) / 2;//Objbalance.IndusEK;
                    //    IndusTarbelaLK = ((ObjMax.IndusTarbelaLK == null ? 0 : ObjMax.IndusTarbelaLK) + (ObjMin.IndusTarbelaLK == null ? 0 : ObjMin.IndusTarbelaLK)) / 2;//Objbalance.IndusLK;
                    //    KabulNowsheraEK = ((ObjMax.KabulNowsheraEK == null ? 0 : ObjMax.KabulNowsheraEK) + (ObjMin.KabulNowsheraEK == null ? 0 : ObjMin.KabulNowsheraEK)) / 2;//Objbalance.KabulEK;
                    //    KabulNowsheraLK = ((ObjMax.KabulNowsheraLK == null ? 0 : ObjMax.KabulNowsheraLK) + (ObjMin.KabulNowsheraLK == null ? 0 : ObjMin.KabulNowsheraLK)) / 2;//Objbalance.KabulLK;
                    //}
                }
                else
                {
                    lstSavedValues = (from Data in context.SP_ForecastData
                                      where ((from Scenario in context.SP_ForecastScenario
                                              where Scenario.ForecastDraftID == _ForecastDraftID && Scenario.Scenario == _Scenario
                                              select Scenario.ID).ToList())
                                          .Contains(Data.ForecastScenarioID)
                                      select Data).ToList();

                    for (int i = 1; i <= 18; i++)
                    {

                        lstTDaily = lstSavedValues.Where(q => q.TDailyID == i).ToList();
                        if (lstTDaily.ElementAtOrDefault(0).TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                        {
                            if (lstTDaily.ElementAtOrDefault(0).TDailyID ==
                                (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            {
                                JhelumAtManglaEK += lstTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                                ChenabAtMaralaEK += lstTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                                IndusTarbelaEK += lstTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                                KabulNowsheraEK += lstTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;
                            }
                            else
                            {
                                JhelumAtManglaEK += lstTDaily.ElementAtOrDefault(0).Volume;
                                ChenabAtMaralaEK += lstTDaily.ElementAtOrDefault(1).Volume;
                                IndusTarbelaEK += lstTDaily.ElementAtOrDefault(2).Volume;
                                KabulNowsheraEK += lstTDaily.ElementAtOrDefault(3).Volume;
                            }
                        }
                        else if (lstTDaily.ElementAtOrDefault(0).TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                        {
                            if (lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily
                                || lstTDaily.ElementAtOrDefault(0).TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            {
                                JhelumAtManglaLK += lstTDaily.ElementAtOrDefault(0).Volume * Constants.TDailyConversion;
                                ChenabAtMaralaLK += lstTDaily.ElementAtOrDefault(1).Volume * Constants.TDailyConversion;
                                IndusTarbelaLK += lstTDaily.ElementAtOrDefault(2).Volume * Constants.TDailyConversion;
                                KabulNowsheraLK += lstTDaily.ElementAtOrDefault(3).Volume * Constants.TDailyConversion;
                            }
                            else
                            {
                                JhelumAtManglaLK += lstTDaily.ElementAtOrDefault(0).Volume;
                                ChenabAtMaralaLK += lstTDaily.ElementAtOrDefault(1).Volume;
                                IndusTarbelaLK += lstTDaily.ElementAtOrDefault(2).Volume;
                                KabulNowsheraLK += lstTDaily.ElementAtOrDefault(3).Volume;
                            }
                        }
                    }

                    JhelumAtManglaEK = JhelumAtManglaEK * Constants.MAFConversion;
                    ChenabAtMaralaEK = ChenabAtMaralaEK * Constants.MAFConversion;
                    IndusTarbelaEK = IndusTarbelaEK * Constants.MAFConversion;
                    KabulNowsheraEK = KabulNowsheraEK * Constants.MAFConversion;

                    JhelumAtManglaLK = JhelumAtManglaLK * Constants.MAFConversion;
                    ChenabAtMaralaLK = ChenabAtMaralaLK * Constants.MAFConversion;
                    IndusTarbelaLK = IndusTarbelaLK * Constants.MAFConversion;
                    KabulNowsheraLK = KabulNowsheraLK * Constants.MAFConversion;
                }

                //double? JhelumAtManglaLKValue = (JhelumAtManglaLK * Constants.MAFConversion);
                //// Calculate Late Kharif Values
                //double? ChenabAtMaralaLKValue = (ChenabAtMaralaLK * Constants.MAFConversion);
                //double? IndusTarbelaLKValue = (IndusTarbelaLK * Constants.MAFConversion);
                //double? KabulNowsheraLKValue = (KabulNowsheraLK * Constants.MAFConversion);

                List<SP_ForecastScenario> lstGetEKLKPercent = (from Scenario in context.SP_ForecastScenario
                                                               where Scenario.ForecastDraftID == _ForecastDraftID && Scenario.Scenario == _Scenario
                                                               select Scenario).ToList();

                #region Early and Lare Kharif Percentage

                var JhelumEkPercent = lstGetEKLKPercent.Where(q => q.StationID == 18).FirstOrDefault().EkPercent;
                var JhelumLkPercent = lstGetEKLKPercent.Where(q => q.StationID == 18).FirstOrDefault().LkPercent;
                var ChenabEkPercent = lstGetEKLKPercent.Where(q => q.StationID == 5).FirstOrDefault().EkPercent;
                var ChenabLkPercent = lstGetEKLKPercent.Where(q => q.StationID == 5).FirstOrDefault().LkPercent;

                var IndusTerbelaEkPercent = lstGetEKLKPercent.Where(q => q.StationID == 20).FirstOrDefault().EkPercent;
                var IndusTerbelaLkPercent = lstGetEKLKPercent.Where(q => q.StationID == 20).FirstOrDefault().LkPercent;
                var KabulNowsheraEkPercent = lstGetEKLKPercent.Where(q => q.StationID == 24).FirstOrDefault().EkPercent;
                var KabulNowsheraLkPercent = lstGetEKLKPercent.Where(q => q.StationID == 24).FirstOrDefault().LkPercent;

                #endregion


                //lstScenario = new
                //{
                //    JhelumAtManglaEK = String.Format("{0:0.000}", (JhelumAtManglaEK * Constants.MAFConversion)),
                //    JhelumAtManglaLK = String.Format("{0:0.000}", (JhelumAtManglaLK * Constants.MAFConversion)),
                //    ChenabAtMaralaEK = String.Format("{0:0.000}", (ChenabAtMaralaEK * Constants.MAFConversion)),
                //    ChenabAtMaralaLK = String.Format("{0:0.000}", (ChenabAtMaralaLK * Constants.MAFConversion)),
                //    JhelumAtManglaLKValue = String.Format("{0:0.000}", JhelumAtManglaLKValue),
                //    ChenabAtMaralaLKValue = String.Format("{0:0.000}", ChenabAtMaralaLKValue),
                //    JhelumEkPercent = JhelumEkPercent,
                //    JhelumLkPercent = JhelumLkPercent,
                //    ChenabEkPercent = ChenabEkPercent,
                //    ChenabLkPercent = ChenabLkPercent,

                //    IndusTarbelaEK = String.Format("{0:0.000}", (IndusTarbelaEK * Constants.MAFConversion)),
                //    IndusTarbelaLK = String.Format("{0:0.000}", (IndusTarbelaLK * Constants.MAFConversion)),
                //    KabulNowsheraEK = String.Format("{0:0.000}", (KabulNowsheraEK * Constants.MAFConversion)),
                //    KabulNowsheraLK = String.Format("{0:0.000}", (KabulNowsheraLK * Constants.MAFConversion)),
                //    IndusTarbelaLKValue = String.Format("{0:0.000}", IndusTarbelaLKValue),
                //    KabulNowsheraLKValue = String.Format("{0:0.000}", KabulNowsheraLKValue),
                //    IndusTerbelaEkPercent = IndusTerbelaEkPercent,
                //    IndusTerbelaLkPercent = IndusTerbelaLkPercent,
                //    KabulNowsheraEkPercent = KabulNowsheraEkPercent,
                //    KabulNowsheraLkPercent = KabulNowsheraLkPercent
                //};

                lstScenario = new
                {
                    JhelumAtManglaEK = String.Format("{0:0.000}", JhelumAtManglaEK),
                    JhelumAtManglaLK = String.Format("{0:0.000}", JhelumAtManglaLK),
                    ChenabAtMaralaEK = String.Format("{0:0.000}", ChenabAtMaralaEK),
                    ChenabAtMaralaLK = String.Format("{0:0.000}", ChenabAtMaralaLK),
                    JhelumEkPercent = JhelumEkPercent,
                    JhelumLkPercent = JhelumLkPercent,
                    ChenabEkPercent = ChenabEkPercent,
                    ChenabLkPercent = ChenabLkPercent,

                    IndusTarbelaEK = String.Format("{0:0.000}", IndusTarbelaEK),
                    IndusTarbelaLK = String.Format("{0:0.000}", IndusTarbelaLK),
                    KabulNowsheraEK = String.Format("{0:0.000}", KabulNowsheraEK),
                    KabulNowsheraLK = String.Format("{0:0.000}", KabulNowsheraLK),
                    IndusTerbelaEkPercent = IndusTerbelaEkPercent,
                    IndusTerbelaLkPercent = IndusTerbelaLkPercent,
                    KabulNowsheraEkPercent = KabulNowsheraEkPercent,
                    KabulNowsheraLkPercent = KabulNowsheraLkPercent
                };
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstScenario;
        }

        public object AnticipatedJCData(long _DraftID, long _StationIDJC, string _Scenario)
        {
            double? LossGainEK = 0;
            double? LossGainLK = 0;
            double? JCOutflowEK = 0;
            double? JCOutflowLK = 0;
            double? StorageReleaseEK = 0;
            double? StorageReleaseLK = 0;

            double? LossGainRabi = 0;
            double? JCOutflowRabi = 0;
            double? StorageReleaseRabi = 0;

            object ObjPlanData = null;
            try
            {

                List<SP_PlanData> lstPlanData = (from PD in context.SP_PlanData
                                                 join PS in context.SP_PlanScenario on PD.PlanScenarioID equals PS.ID
                                                 where PS.PlanDraftID == _DraftID && PS.Scenario == _Scenario && PS.StationID == _StationIDJC
                                                 select PD).ToList();


                foreach (var TDaily in lstPlanData)
                {

                    if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.EKStart &&
                        TDaily.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                        {
                            StorageReleaseEK += TDaily.StorageRelease * (double)Constants.TDailyFactor;
                            LossGainEK += TDaily.LossGain * (double)Constants.TDailyFactor;
                            JCOutflowEK += TDaily.SystemOutflow * (double)Constants.TDailyFactor;
                        }
                        else
                        {
                            StorageReleaseEK += TDaily.StorageRelease;
                            LossGainEK += TDaily.LossGain;
                            JCOutflowEK += TDaily.SystemOutflow;
                        }
                    }
                    else if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.LKStart &&
                             TDaily.TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                    {
                        if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                            TDaily.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                        {
                            StorageReleaseLK += TDaily.StorageRelease * (double)Constants.TDailyFactor;
                            LossGainLK += TDaily.LossGain * (double)Constants.TDailyFactor;
                            JCOutflowLK += TDaily.SystemOutflow * (double)Constants.TDailyFactor;
                        }
                        else
                        {
                            StorageReleaseLK += TDaily.StorageRelease;
                            LossGainLK += TDaily.LossGain;
                            JCOutflowLK += TDaily.SystemOutflow;
                        }
                    }
                    else if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.RabiStart &&
                             TDaily.TDailyID <= (int)Constants.SeasonDistribution.RabiEnd)
                    {
                        if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily ||
                            TDaily.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                            || TDaily.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                            TDaily.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                        {
                            StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyFactor;
                            LossGainRabi += TDaily.LossGain * (double)Constants.TDailyFactor;
                            JCOutflowRabi += TDaily.SystemOutflow * (double)Constants.TDailyFactor;

                        }
                        else if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                        {
                            string MonthName = DateTime.Now.ToString("MMM");
                            if (MonthName == "Oct" || MonthName == "Nov" || MonthName == "Dec")
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                {
                                    StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearTrue;
                                    LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearTrue;
                                    JCOutflowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearTrue;
                                }
                                else
                                {
                                    StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearFalse;
                                    LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearFalse;
                                    JCOutflowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearFalse;
                                }
                            }
                            else
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year))
                                {
                                    StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearTrue;
                                    LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearTrue;
                                    JCOutflowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearTrue;
                                }
                                else
                                {
                                    StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearFalse;
                                    LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearFalse;
                                    JCOutflowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearFalse;
                                }
                            }
                        }
                        else
                        {
                            StorageReleaseRabi += TDaily.StorageRelease;
                            LossGainRabi += TDaily.LossGain;
                            JCOutflowRabi += TDaily.SystemOutflow;
                        }
                    }


                }
                ObjPlanData = new
                {
                    StorageReleaseEK = String.Format("{0:0.000}", StorageReleaseEK * Constants.MAFConversion),
                    LossGainEK = String.Format("{0:0.000}", LossGainEK * Constants.MAFConversion),
                    JCOutflowEK = String.Format("{0:0.000}", JCOutflowEK * Constants.MAFConversion),
                    StorageReleaseLK = String.Format("{0:0.000}", StorageReleaseLK * Constants.MAFConversion),
                    LossGainLK = String.Format("{0:0.000}", LossGainLK * Constants.MAFConversion),
                    JCOutflowLK = String.Format("{0:0.000}", JCOutflowLK * Constants.MAFConversion),

                    StorageReleaseRabiValue = String.Format("{0:0.000}", StorageReleaseRabi * Constants.MAFConversion),
                    LossGainRabiValue = String.Format("{0:0.000}", LossGainRabi * Constants.MAFConversion),
                    JCOutflowRabiValue = String.Format("{0:0.000}", JCOutflowRabi * Constants.MAFConversion)
                };

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ObjPlanData;
        }

        public object AnticipatedIKData(long _DraftID, long _StationIDIK, string _Scenario)
        {
            double? KotriBelowEK = 0;
            double? KotriBelowLK = 0;
            double? LossGainEK = 0;
            double? JCOutflowEK = 0;
            double? LossGainLK = 0;
            double? JCOutflowLK = 0;
            double? StorageReleaseEK = 0;
            double? StorageReleaseLK = 0;
            double? ChashmaStorageReleaseEK = 0;
            double? ChashmaStorageReleaseLK = 0;

            #region Rabi Variable

            double? KotriBelowRabi = 0;
            double? LossGainRabi = 0;
            double? JCOutflowRabi = 0;
            double? StorageReleaseRabi = 0;
            double? ChashmaStorageReleaseRabi = 0;



            #endregion


            object ObjPlanData = null;

            try
            {

                List<SP_PlanData> lstPlanData = (from PD in context.SP_PlanData
                                                 join PS in context.SP_PlanScenario on PD.PlanScenarioID equals PS.ID
                                                 where PS.PlanDraftID == _DraftID && PS.Scenario == _Scenario && PS.StationID == _StationIDIK
                                                 select PD).ToList();


                foreach (var TDaily in lstPlanData)
                {

                    if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.EKStart &&
                        TDaily.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                    {
                        if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                        {
                            StorageReleaseEK += TDaily.StorageRelease * (double)Constants.TDailyFactor;
                            LossGainEK += TDaily.LossGain * (double)Constants.TDailyFactor;
                            JCOutflowEK += TDaily.JCOutflow * (double)Constants.TDailyFactor;
                            ChashmaStorageReleaseEK += TDaily.ChashmaStorageRelease * (double)Constants.TDailyFactor;
                            KotriBelowEK += TDaily.SystemOutflow * (double)Constants.TDailyFactor;
                        }
                        else
                        {
                            StorageReleaseEK += TDaily.StorageRelease;
                            LossGainEK += TDaily.LossGain;
                            JCOutflowEK += TDaily.JCOutflow;
                            ChashmaStorageReleaseEK += TDaily.ChashmaStorageRelease;
                            KotriBelowEK += TDaily.SystemOutflow;
                        }

                    }
                    else if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.LKStart &&
                             TDaily.TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                    {
                        if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                            TDaily.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                        {
                            StorageReleaseLK += TDaily.StorageRelease * (double)Constants.TDailyFactor;
                            LossGainLK += TDaily.LossGain * (double)Constants.TDailyFactor;
                            JCOutflowLK += TDaily.JCOutflow * (double)Constants.TDailyFactor;
                            ChashmaStorageReleaseLK += TDaily.ChashmaStorageRelease * (double)Constants.TDailyFactor;
                            KotriBelowLK += TDaily.SystemOutflow * (double)Constants.TDailyFactor;
                        }
                        else
                        {
                            StorageReleaseLK += TDaily.StorageRelease;
                            LossGainLK += TDaily.LossGain;
                            JCOutflowLK += TDaily.JCOutflow;
                            ChashmaStorageReleaseLK += TDaily.ChashmaStorageRelease;
                            KotriBelowLK += TDaily.SystemOutflow;

                        }

                    }
                    else if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.RabiStart &&
                             TDaily.TDailyID <= (int)Constants.SeasonDistribution.RabiEnd)
                    {
                        if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily ||
                            TDaily.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                            || TDaily.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                            TDaily.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                        {
                            StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyFactor;
                            LossGainRabi += TDaily.LossGain * (double)Constants.TDailyFactor;
                            JCOutflowRabi += TDaily.JCOutflow * (double)Constants.TDailyFactor;
                            ChashmaStorageReleaseRabi += TDaily.ChashmaStorageRelease * (double)Constants.TDailyFactor;
                            KotriBelowRabi += TDaily.SystemOutflow * (double)Constants.TDailyFactor;
                        }
                        else if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                        {
                            string MonthName = DateTime.Now.ToString("MMM");

                            if (MonthName == "Oct" || MonthName == "Nov" || MonthName == "Dec")
                            {
                                if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                {
                                    StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearTrue;
                                    LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearTrue;
                                    JCOutflowRabi += TDaily.JCOutflow * (double)Constants.TDailyLeapYearTrue;
                                    ChashmaStorageReleaseRabi += TDaily.ChashmaStorageRelease *
                                                                 (double)Constants.TDailyLeapYearTrue;
                                    KotriBelowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearTrue;
                                }
                                else
                                {
                                    StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearFalse;
                                    LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearFalse;
                                    JCOutflowRabi += TDaily.JCOutflow * (double)Constants.TDailyLeapYearFalse;
                                    ChashmaStorageReleaseRabi += TDaily.ChashmaStorageRelease *
                                                                 (double)Constants.TDailyLeapYearFalse;
                                    KotriBelowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearFalse;
                                }
                            }
                            else
                            {
                                //StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearFalse;
                                //LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearFalse;
                                //JCOutflowRabi += TDaily.JCOutflow * (double)Constants.TDailyLeapYearFalse;
                                //ChashmaStorageReleaseRabi += TDaily.ChashmaStorageRelease *
                                //                             (double)Constants.TDailyLeapYearFalse;
                                //KotriBelowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearFalse;
                                if (DateTime.IsLeapYear(DateTime.Now.Year))
                                {
                                    StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearTrue;
                                    LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearTrue;
                                    JCOutflowRabi += TDaily.JCOutflow * (double)Constants.TDailyLeapYearTrue;
                                    ChashmaStorageReleaseRabi += TDaily.ChashmaStorageRelease *
                                                                 (double)Constants.TDailyLeapYearTrue;
                                    KotriBelowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearTrue;
                                }
                                else
                                {
                                    StorageReleaseRabi += TDaily.StorageRelease * (double)Constants.TDailyLeapYearFalse;
                                    LossGainRabi += TDaily.LossGain * (double)Constants.TDailyLeapYearFalse;
                                    JCOutflowRabi += TDaily.JCOutflow * (double)Constants.TDailyLeapYearFalse;
                                    ChashmaStorageReleaseRabi += TDaily.ChashmaStorageRelease *
                                                                 (double)Constants.TDailyLeapYearFalse;
                                    KotriBelowRabi += TDaily.SystemOutflow * (double)Constants.TDailyLeapYearFalse;
                                }
                            }
                        }
                        else
                        {
                            StorageReleaseRabi += TDaily.StorageRelease;
                            LossGainRabi += TDaily.LossGain;
                            JCOutflowRabi += TDaily.JCOutflow;
                            ChashmaStorageReleaseRabi += TDaily.ChashmaStorageRelease;
                            KotriBelowRabi += TDaily.SystemOutflow;

                        }
                    }
                }
                ObjPlanData = new
                {
                    StorageReleaseEK = String.Format("{0:0.000}", StorageReleaseEK * Constants.MAFConversion),
                    StorageReleaseLK = String.Format("{0:0.000}", StorageReleaseLK * Constants.MAFConversion),
                    LossGainEK = String.Format("{0:0.000}", LossGainEK * Constants.MAFConversion),
                    LossGainLK = String.Format("{0:0.000}", LossGainLK * Constants.MAFConversion),
                    JCOutflowEK = String.Format("{0:0.000}", JCOutflowEK * Constants.MAFConversion),
                    JCOutflowLK = String.Format("{0:0.000}", JCOutflowLK * Constants.MAFConversion),
                    ChashmaStorageReleaseEK = String.Format("{0:0.000}", ChashmaStorageReleaseEK * Constants.MAFConversion),
                    ChashmaStorageReleaseLK = String.Format("{0:0.000}", Math.Truncate((decimal)(ChashmaStorageReleaseLK * Constants.MAFConversion) * 1000m) / 1000m),
                    //String.Format("{0:0.000}", ChashmaStorageReleaseLK * Constants.MAFConversion),
                    KotriBelowEK = String.Format("{0:0.000}", KotriBelowEK * Constants.MAFConversion),
                    KotriBelowLK = String.Format("{0:0.000}", KotriBelowLK * Constants.MAFConversion),
                    StorageReleaseRabi = String.Format("{0:0.000}", StorageReleaseRabi * Constants.MAFConversion),
                    LossGainRabi = String.Format("{0:0.000}", LossGainRabi * Constants.MAFConversion),
                    JCOutflowRabi = String.Format("{0:0.000}", JCOutflowRabi * Constants.MAFConversion),
                    ChashmaStorageReleaseRabi =
                    String.Format("{0:0.000}", ChashmaStorageReleaseRabi * Constants.MAFConversion),
                    KotriBelowRabi = String.Format("{0:0.000}", KotriBelowRabi * Constants.MAFConversion)

                };

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ObjPlanData;
        }

        public object JCFlowData(long _SeasonID)
        {

            List<SP_RefShareDistribution> lstFlows = null;
            object JCFlow = null;
            double? ShareJCEK = 0;
            double? ShareJCLK = 0;
            double? KPShareEK = 0;
            double? KPShareLK = 0;
            double? BalochistanShareEK = 0;
            double? BalochistanShareLK = 0;
            double? PunjabHIstoricEK = 0;
            double? PunjabHIstoricEKPara = 0;
            double? PunjabHIstoricLK = 0;
            double? SindhHIstoricEK = 0;
            double? SindhHIstoricEKPara = 0;
            double? SindhHIstoricLK = 0;
            double? LesskPPlusBlochEK,
                LesskPPlusBlochLK,
                ShareOfPAndSEK,
                ShareOfPAndSEKPara,
                ShareOfPAndSLK,
                ShareOfJCAndIKEK,
                ShareOfJCAndIKEKPara,
                ShareOfJCAndIKLK;

            #region Rabi Variable

            double? RabiShare = 0;
            double? KPShareRabi = 0;
            double? BalochistanShareRabi = 0;
            double? PunjabHIstoricRabi = 0;
            double? SindhHIstoricRabi = 0;

            double? LesskPPlusBlochRabi, ShareOfPAndSRabi, ShareOfJCAndIKRabi;

            #endregion

            try
            {
                lstFlows = (from SD in context.SP_RefShareDistribution
                            where SD.SeasonID == _SeasonID
                            select SD).ToList();

                foreach (var TDaily in lstFlows)
                {
                    if (_SeasonID == (int)Constants.Seasons.Kharif)
                    {
                        if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.EKStart &&
                            TDaily.TDailyID <= (int)Constants.SeasonDistribution.EKEnd)
                        {
                            if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.MaySPecialTDaily)
                            {

                                ShareJCEK += TDaily.Jc7782 * (double)Constants.TDailyFactor;
                                KPShareEK += TDaily.KPShare * (double)Constants.TDailyFactor;
                                BalochistanShareEK += TDaily.BalochistanShare * (double)Constants.TDailyFactor;
                                PunjabHIstoricEK += TDaily.PunjabHistoric * (double)Constants.TDailyFactor;
                                SindhHIstoricEK += TDaily.SindhHistoric * (double)Constants.TDailyFactor;
                                //
                                PunjabHIstoricEKPara += TDaily.PunjabPara2 * (double)Constants.TDailyFactor;
                                SindhHIstoricEKPara += TDaily.SindhPara2 * (double)Constants.TDailyFactor;
                            }
                            else
                            {

                                ShareJCEK += TDaily.Jc7782;
                                KPShareEK += TDaily.KPShare;
                                BalochistanShareEK += TDaily.BalochistanShare;
                                PunjabHIstoricEK += TDaily.PunjabHistoric;
                                SindhHIstoricEK += TDaily.SindhHistoric;
                                //
                                PunjabHIstoricEKPara += TDaily.PunjabPara2;
                                SindhHIstoricEKPara += TDaily.SindhPara2;
                            }

                        }
                        else if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.LKStart &&
                                 TDaily.TDailyID <= (int)Constants.SeasonDistribution.LKEnd)
                        {
                            if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.JulyTDaily ||
                                TDaily.TDailyID == (int)Constants.TDAilySpecialCases.AugTDaily)
                            {
                                ShareJCLK += TDaily.Jc7782 * (double)Constants.TDailyFactor;
                                KPShareLK += TDaily.KPShare * (double)Constants.TDailyFactor;
                                BalochistanShareLK += TDaily.BalochistanShare * (double)Constants.TDailyFactor;
                                PunjabHIstoricLK += TDaily.PunjabPara2 * (double)Constants.TDailyFactor;
                                SindhHIstoricLK += TDaily.SindhPara2 * (double)Constants.TDailyFactor;
                            }
                            else
                            {
                                ShareJCLK += TDaily.Jc7782;
                                KPShareLK += TDaily.KPShare;
                                BalochistanShareLK += TDaily.BalochistanShare;
                                PunjabHIstoricLK += TDaily.PunjabPara2;
                                SindhHIstoricLK += TDaily.SindhPara2;
                            }

                        }

                    }
                    else
                    {
                        if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.RabiStart &&
                            TDaily.TDailyID <= (int)Constants.SeasonDistribution.RabiEnd)
                        {
                            if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily ||
                                TDaily.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                                || TDaily.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                                TDaily.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                            {

                                RabiShare += TDaily.Jc7782 * (double)Constants.TDailyFactor;
                                KPShareRabi += TDaily.KPShare * (double)Constants.TDailyFactor;
                                BalochistanShareRabi += TDaily.BalochistanShare * (double)Constants.TDailyFactor;
                                PunjabHIstoricRabi += TDaily.PunjabHistoric * (double)Constants.TDailyFactor;
                                SindhHIstoricRabi += TDaily.SindhHistoric * (double)Constants.TDailyFactor;
                            }
                            else if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                            {
                                string MonthName = DateTime.Now.ToString("MMM");

                                if (MonthName == "Oct" || MonthName == "Nov" || MonthName == "Dec")
                                {
                                    if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                    {
                                        RabiShare += TDaily.Jc7782 * (double)Constants.TDailyLeapYearTrue;
                                        KPShareRabi += TDaily.KPShare * (double)Constants.TDailyLeapYearTrue;
                                        BalochistanShareRabi += TDaily.BalochistanShare *
                                                                (double)Constants.TDailyLeapYearTrue;
                                        PunjabHIstoricRabi += TDaily.PunjabHistoric *
                                                              (double)Constants.TDailyLeapYearTrue;
                                        SindhHIstoricRabi += TDaily.SindhHistoric * (double)Constants.TDailyLeapYearTrue;
                                    }
                                    else // added
                                    {
                                        RabiShare += TDaily.Jc7782 * (double)Constants.TDailyLeapYearFalse;
                                        KPShareRabi += TDaily.KPShare * (double)Constants.TDailyLeapYearFalse;
                                        BalochistanShareRabi += TDaily.BalochistanShare *
                                                                (double)Constants.TDailyLeapYearFalse;
                                        PunjabHIstoricRabi += TDaily.PunjabHistoric * (double)Constants.TDailyLeapYearFalse;
                                        SindhHIstoricRabi += TDaily.SindhHistoric * (double)Constants.TDailyLeapYearFalse;
                                    }
                                }
                                else
                                {
                                    //RabiShare += TDaily.Jc7782 * (double)Constants.TDailyLeapYearFalse;
                                    //KPShareRabi += TDaily.KPShare * (double)Constants.TDailyLeapYearFalse;
                                    //BalochistanShareRabi += TDaily.BalochistanShare *
                                    //                        (double)Constants.TDailyLeapYearFalse;
                                    //PunjabHIstoricRabi += TDaily.PunjabHistoric * (double)Constants.TDailyLeapYearFalse;
                                    //SindhHIstoricRabi += TDaily.SindhHistoric * (double)Constants.TDailyLeapYearFalse;

                                    //added
                                    if (DateTime.IsLeapYear(DateTime.Now.Year))
                                    {
                                        RabiShare += TDaily.Jc7782 * (double)Constants.TDailyLeapYearTrue;
                                        KPShareRabi += TDaily.KPShare * (double)Constants.TDailyLeapYearTrue;
                                        BalochistanShareRabi += TDaily.BalochistanShare *
                                                                (double)Constants.TDailyLeapYearTrue;
                                        PunjabHIstoricRabi += TDaily.PunjabHistoric *
                                                              (double)Constants.TDailyLeapYearTrue;
                                        SindhHIstoricRabi += TDaily.SindhHistoric * (double)Constants.TDailyLeapYearTrue;
                                    }
                                    else
                                    {
                                        RabiShare += TDaily.Jc7782 * (double)Constants.TDailyLeapYearFalse;
                                        KPShareRabi += TDaily.KPShare * (double)Constants.TDailyLeapYearFalse;
                                        BalochistanShareRabi += TDaily.BalochistanShare *
                                                                (double)Constants.TDailyLeapYearFalse;
                                        PunjabHIstoricRabi += TDaily.PunjabHistoric * (double)Constants.TDailyLeapYearFalse;
                                        SindhHIstoricRabi += TDaily.SindhHistoric * (double)Constants.TDailyLeapYearFalse;
                                    }
                                }
                            }
                            else
                            {
                                RabiShare += TDaily.Jc7782;
                                KPShareRabi += TDaily.KPShare;
                                BalochistanShareRabi += TDaily.BalochistanShare;
                                PunjabHIstoricRabi += TDaily.PunjabHistoric;
                                SindhHIstoricRabi += TDaily.SindhHistoric;
                            }
                        }
                    }
                }
                LesskPPlusBlochEK = KPShareEK + BalochistanShareEK;
                LesskPPlusBlochLK = KPShareLK + BalochistanShareLK;
                ShareOfPAndSEK = PunjabHIstoricEK + SindhHIstoricEK;
                ShareOfPAndSEKPara = PunjabHIstoricEKPara + SindhHIstoricEKPara;
                ShareOfPAndSLK = PunjabHIstoricLK + SindhHIstoricLK;
                ShareOfJCAndIKEK = ShareJCEK + ShareOfPAndSEK;
                ShareOfJCAndIKEKPara = PunjabHIstoricEKPara + SindhHIstoricEKPara + ShareJCEK;
                ShareOfJCAndIKLK = ShareJCLK + ShareOfPAndSLK;

                LesskPPlusBlochRabi = KPShareRabi + BalochistanShareRabi;
                ShareOfPAndSRabi = PunjabHIstoricRabi + SindhHIstoricRabi;
                ShareOfJCAndIKRabi = RabiShare + ShareOfPAndSRabi;

                JCFlow = new
                {
                    ShareEKValue = String.Format("{0:0.000}", ShareJCEK * Constants.MAFConversion),
                    ShareLKValue = String.Format("{0:0.000}", ShareJCLK * Constants.MAFConversion),
                    LesskPPlusBlochEK = String.Format("{0:0.000}", LesskPPlusBlochEK * Constants.MAFConversion),
                    LesskPPlusBlochLK = String.Format("{0:0.000}", LesskPPlusBlochLK * Constants.MAFConversion),
                    ShareOfPAndSEK = String.Format("{0:0.000}", ShareOfPAndSEK * Constants.MAFConversion),
                    //
                    ShareOfPAndSEKPara = String.Format("{0:0.000}", ShareOfPAndSEKPara * Constants.MAFConversion),
                    //
                    ShareOfPAndSLK = String.Format("{0:0.000}", ShareOfPAndSLK * Constants.MAFConversion),
                    ShareOfJCAndIKEK = String.Format("{0:0.000}", ShareOfJCAndIKEK * Constants.MAFConversion),
                    ShareOfJCAndIKEKPara = String.Format("{0:0.000}", ShareOfJCAndIKEKPara * Constants.MAFConversion),
                    ShareOfJCAndIKLK = String.Format("{0:0.000}", ShareOfJCAndIKLK * Constants.MAFConversion),

                    RabiShareValue = String.Format("{0:0.000}", RabiShare * Constants.MAFConversion),
                    LesskPPlusBlochRabiValue = String.Format("{0:0.000}", LesskPPlusBlochRabi * Constants.MAFConversion),
                    ShareOfPAndSRabiValue = String.Format("{0:0.000}", ShareOfPAndSRabi * Constants.MAFConversion),
                    ShareOfJCAndIKRabiValue = String.Format("{0:0.000}", ShareOfJCAndIKRabi * Constants.MAFConversion)

                };


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return JCFlow;
        }

        public object AnticipatedRabiData(long _ForecastDraftID, string _Scenario, long _StationID, long _PlanDraftID)
        {
            List<SP_ForecastData> lstSavedValues = new List<SP_ForecastData>();
            SP_PlanBalance Objbalance = new SP_PlanBalance();
            object lstScenario = null;
            double? JhelumAtManglaRabi = 0;
            double? ChenabAtMaralaRabi = 0;
            double? IndusTarbelaRabi = 0;
            double? KabulNowsheraRabi = 0;


            try
            {
                List<SP_ForecastScenario> lstForecastScenario = (from Scenario in context.SP_ForecastScenario
                                                                 where Scenario.ForecastDraftID == _ForecastDraftID && Scenario.Scenario == _Scenario
                                                                 select Scenario).ToList();
                if (_Scenario == "Likely")
                {
                    if (_StationID != -1)
                    {
                        Objbalance = (from SPSce in context.SP_PlanScenario
                                      join SPBal in context.SP_PlanBalance on SPSce.ID equals SPBal.PlanScenarioID
                                      where SPSce.PlanDraftID == _PlanDraftID && SPSce.StationID == _StationID && SPSce.Scenario == _Scenario
                                      select SPBal).FirstOrDefault();

                        JhelumAtManglaRabi = Objbalance.JhelumRabi;
                        ChenabAtMaralaRabi = Objbalance.ChenabRabi;
                        IndusTarbelaRabi = Objbalance.IndusRabi;
                        KabulNowsheraRabi = Objbalance.KabulRabi;
                    }
                    else
                    {
                        List<SP_PlanBalance> lstBalance = (from SPSce in context.SP_PlanScenario
                                                           join SPBal in context.SP_PlanBalance on SPSce.ID equals SPBal.PlanScenarioID
                                                           where SPSce.PlanDraftID == _PlanDraftID && SPSce.Scenario == _Scenario
                                                           && (SPSce.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela ||
                                                           SPSce.StationID == (int)Constants.RimStationsIDs.JhelumATMangla)
                                                           select SPBal).ToList();

                        Objbalance = lstBalance.Where(q => q.StationID == (int)Constants.RimStationsIDs.JhelumATMangla).FirstOrDefault();
                        if (Objbalance != null)
                        {
                            JhelumAtManglaRabi = Objbalance.JhelumRabi;
                            ChenabAtMaralaRabi = Objbalance.ChenabRabi;
                        }

                        Objbalance = lstBalance.Where(q => q.StationID == (int)Constants.RimStationsIDs.IndusAtTarbela).FirstOrDefault();
                        if (Objbalance != null)
                        {
                            IndusTarbelaRabi = Objbalance.IndusRabi;
                            KabulNowsheraRabi = Objbalance.KabulRabi;
                        }
                    }

                    #region Rabi Percentage

                    var JhelumAtManglaRabiPercent =
                        lstForecastScenario.Where(q => q.StationID == 18).FirstOrDefault().RabiPercent;
                    var ChenabAtMaralaRabiPercent =
                        lstForecastScenario.Where(q => q.StationID == 5).FirstOrDefault().RabiPercent;
                    var IndusTarbelaRabiPercent =
                        lstForecastScenario.Where(q => q.StationID == 20).FirstOrDefault().RabiPercent;
                    var KabulNowsheraRabiPercent =
                        lstForecastScenario.Where(q => q.StationID == 24).FirstOrDefault().RabiPercent;

                    #endregion

                    lstScenario = new
                    {
                        JhelumAtManglaRabiValue = String.Format("{0:0.000}", JhelumAtManglaRabi),
                        ChenabAtMaralaRabiValue = String.Format("{0:0.000}", ChenabAtMaralaRabi),
                        IndusTarbelaRabiValue = String.Format("{0:0.000}", IndusTarbelaRabi),
                        KabulNowsheraRabiValue = String.Format("{0:0.000}", KabulNowsheraRabi),
                        JhelumAtManglaRabiPercentValue = JhelumAtManglaRabiPercent,
                        ChenabAtMaralaRabiPercentValue = ChenabAtMaralaRabiPercent,
                        IndusTarbelaRabiPercentvalue = IndusTarbelaRabiPercent,
                        KabulNowsheraRabiPercentValue = KabulNowsheraRabiPercent
                    };
                }
                else
                {
                    lstSavedValues = (from Data in context.SP_ForecastData
                                      where ((from Scenario in context.SP_ForecastScenario
                                              where Scenario.ForecastDraftID == _ForecastDraftID && Scenario.Scenario == _Scenario
                                              select Scenario.ID).ToList())
                                          .Contains(Data.ForecastScenarioID)
                                      select Data).ToList();

                    foreach (var ForecastScenario in lstForecastScenario)
                    {

                        foreach (var TDaily in lstSavedValues)
                        {
                            if (TDaily.TDailyID >= (int)Constants.SeasonDistribution.RabiStart &&
                                TDaily.TDailyID <= (int)Constants.SeasonDistribution.RabiEnd)
                            {
                                if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.OctTDaily ||
                                    TDaily.TDailyID == (int)Constants.TDAilySpecialCases.DecTDaily
                                    || TDaily.TDailyID == (int)Constants.TDAilySpecialCases.JanTDaily ||
                                    TDaily.TDailyID == (int)Constants.TDAilySpecialCases.MarTDaily)
                                {
                                    if (TDaily.ForecastScenarioID == ForecastScenario.ID && ForecastScenario.StationID == 18)
                                        JhelumAtManglaRabi += TDaily.Volume * (double)Constants.TDailyFactor;
                                    if (TDaily.ForecastScenarioID == ForecastScenario.ID && ForecastScenario.StationID == 5)
                                        ChenabAtMaralaRabi += TDaily.Volume * (double)Constants.TDailyFactor;
                                    if (TDaily.ForecastScenarioID == ForecastScenario.ID && ForecastScenario.StationID == 20)
                                        IndusTarbelaRabi += TDaily.Volume * (double)Constants.TDailyFactor;
                                    if (TDaily.ForecastScenarioID == ForecastScenario.ID && ForecastScenario.StationID == 24)
                                        KabulNowsheraRabi += TDaily.Volume * (double)Constants.TDailyFactor;

                                }
                                else if (TDaily.TDailyID == (int)Constants.TDAilySpecialCases.FebTDaily)
                                {
                                    string MonthName = DateTime.Now.ToString("MMM");

                                    if (MonthName == "Oct" || MonthName == "Nov" || MonthName == "Dec")
                                    {
                                        if (DateTime.IsLeapYear(DateTime.Now.Year + 1))
                                        {
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 18)
                                                JhelumAtManglaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearTrue;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 5)
                                                ChenabAtMaralaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearTrue;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 20)
                                                IndusTarbelaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearTrue;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 24)
                                                KabulNowsheraRabi += TDaily.Volume * (double)Constants.TDailyLeapYearTrue;
                                        }
                                        else
                                        {
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 18)
                                                JhelumAtManglaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 5)
                                                ChenabAtMaralaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 20)
                                                IndusTarbelaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 24)
                                                KabulNowsheraRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                        }
                                    }
                                    else
                                    {
                                        //if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                        //    ForecastScenario.StationID == 18)
                                        //    JhelumAtManglaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                        //if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                        //    ForecastScenario.StationID == 5)
                                        //    ChenabAtMaralaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                        //if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                        //    ForecastScenario.StationID == 20)
                                        //    IndusTarbelaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                        //if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                        //    ForecastScenario.StationID == 24)
                                        //    KabulNowsheraRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;

                                        if (DateTime.IsLeapYear(DateTime.Now.Year))
                                        {
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 18)
                                                JhelumAtManglaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearTrue;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 5)
                                                ChenabAtMaralaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearTrue;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 20)
                                                IndusTarbelaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearTrue;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 24)
                                                KabulNowsheraRabi += TDaily.Volume * (double)Constants.TDailyLeapYearTrue;
                                        }
                                        else
                                        {
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 18)
                                                JhelumAtManglaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 5)
                                                ChenabAtMaralaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 20)
                                                IndusTarbelaRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                            if (TDaily.ForecastScenarioID == ForecastScenario.ID &&
                                                ForecastScenario.StationID == 24)
                                                KabulNowsheraRabi += TDaily.Volume * (double)Constants.TDailyLeapYearFalse;
                                        }
                                    }
                                }
                                else
                                {

                                    if (TDaily.ForecastScenarioID == ForecastScenario.ID && ForecastScenario.StationID == 18)
                                        JhelumAtManglaRabi += TDaily.Volume;
                                    if (TDaily.ForecastScenarioID == ForecastScenario.ID && ForecastScenario.StationID == 5)
                                        ChenabAtMaralaRabi += TDaily.Volume;
                                    if (TDaily.ForecastScenarioID == ForecastScenario.ID && ForecastScenario.StationID == 20)
                                        IndusTarbelaRabi += TDaily.Volume;
                                    if (TDaily.ForecastScenarioID == ForecastScenario.ID && ForecastScenario.StationID == 24)
                                        KabulNowsheraRabi += TDaily.Volume;
                                }
                            }

                        }
                    }

                    #region Rabi Percentage

                    var JhelumAtManglaRabiPercent =
                        lstForecastScenario.Where(q => q.StationID == 18).FirstOrDefault().RabiPercent;
                    var ChenabAtMaralaRabiPercent =
                        lstForecastScenario.Where(q => q.StationID == 5).FirstOrDefault().RabiPercent;
                    var IndusTarbelaRabiPercent =
                        lstForecastScenario.Where(q => q.StationID == 20).FirstOrDefault().RabiPercent;
                    var KabulNowsheraRabiPercent =
                        lstForecastScenario.Where(q => q.StationID == 24).FirstOrDefault().RabiPercent;

                    #endregion

                    lstScenario = new
                    {
                        JhelumAtManglaRabiValue = String.Format("{0:0.000}", JhelumAtManglaRabi * Constants.MAFConversion),
                        ChenabAtMaralaRabiValue = String.Format("{0:0.000}", ChenabAtMaralaRabi * Constants.MAFConversion),
                        IndusTarbelaRabiValue = String.Format("{0:0.000}", IndusTarbelaRabi * Constants.MAFConversion),
                        KabulNowsheraRabiValue = String.Format("{0:0.000}", KabulNowsheraRabi * Constants.MAFConversion),
                        JhelumAtManglaRabiPercentValue = JhelumAtManglaRabiPercent,
                        ChenabAtMaralaRabiPercentValue = ChenabAtMaralaRabiPercent,
                        IndusTarbelaRabiPercentvalue = IndusTarbelaRabiPercent,
                        KabulNowsheraRabiPercentValue = KabulNowsheraRabiPercent
                    };
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstScenario;

        }

        public object ERComponents(long _DraftID, long _StationID, string _Scenario)
        {
            object lstERComponents = null;
            try
            {

                lstERComponents = (from PD in context.SP_PlanBalance
                                   join PS in context.SP_PlanScenario on PD.PlanScenarioID equals PS.ID
                                   where PS.PlanDraftID == _DraftID && PS.StationID == _StationID && PS.Scenario == _Scenario
                                   select new
                                   {
                                       EasternEK = PD.EasternEK,
                                       EasternLK = PD.EasternLK,
                                       EasternRabi = PD.Eastern

                                   }).Distinct().FirstOrDefault();


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstERComponents;
        }

        public bool? GetParaHistoricBit(long _PlanDraftID)
        {
            bool? IsPara = false;
            try
            {
                IsPara = (from PD in context.SP_PlanDraft
                          where PD.ID == +_PlanDraftID
                          select PD.IsPara2).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return IsPara;
        }


        #endregion
    }
}
