using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.EntitlementDelivery
{
    class EntitlementDeliveryRepository : Repository<ED_ProvincialEntitlement>
    {
        public EntitlementDeliveryRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<ED_ProvincialEntitlement>();

        }

        #region Header Information

        public List<object> GetYears(long _CommandID)
        {
            List<object> lstYears = new List<object>();
            try
            {
                if (_CommandID == (int)Constants.Commands.IndusCommand)
                {
                    lstYears = (from Indus in context.ED_ProvincialEntitlement
                                select new
                                {
                                    ID = Indus.Year,
                                    Name = Indus.Year
                                }).Distinct().ToList<object>();
                }
                else
                {
                    lstYears = (from Indus in context.ED_ProvincialEntitlement
                                select new
                                {
                                    ID = Indus.Year,
                                    Name = Indus.Year
                                }).Distinct().ToList<object>();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstYears;
        }


        public List<object> GetMainCanals(long _CommandID)
        {
            List<object> lstCanals = new List<object>();
            try
            {
                lstCanals = (from Chnl in context.CO_Channel
                             where Chnl.ComndTypeID == _CommandID && Chnl.ChannelTypeID == (int)Constants.ChannelType.MainCanal
                             select new
                             {
                                 ID = Chnl.ID,
                                 Name = Chnl.NAME
                             }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstCanals;
        }


        #endregion

        public double GetDistributionMAF(long _ChannelID, long _SeasonID, long _Year)
        {
            double Distribution = 0;
            if (_SeasonID == 1)
            {
                Distribution = (from cdgr in context.CO_ChannelDailyGaugeReading
                                join cg in context.CO_ChannelGauge on cdgr.GaugeID equals cg.ID
                                where cg.ChannelID == _ChannelID && cg.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge
                                && cdgr.ReadingDateTime.Year == _Year
                                && cdgr.ReadingDateTime.Month >= 4
                                && cdgr.ReadingDateTime.Month <= 9
                                select cdgr.DailyDischarge).Sum();
            }
            else
            {
                Distribution = (from cdgr in context.CO_ChannelDailyGaugeReading
                                join cg in context.CO_ChannelGauge on cdgr.GaugeID equals cg.ID
                                where cg.ChannelID == _ChannelID && cg.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge
                                && (cdgr.ReadingDateTime.Year == _Year && cdgr.ReadingDateTime.Month >= 10)
                                && (cdgr.ReadingDateTime.Year == _Year + 1 && cdgr.ReadingDateTime.Month <= 3)
                                select cdgr.DailyDischarge).Sum();
            }

            return Distribution = Distribution * 0.000001983471;
        }

        public List<dynamic> GetChannelEntitlementBySearchCriteria(long _ChannelID, List<long> _lstSeasonIDs, long _Year)
        {
            List<dynamic> lstRabiEntitlements = (from ce in context.ED_ChannelEntitlement
                                                 join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
                                                 where (ce.Year == _Year || _Year == -1)
                                                 && ce.SeasonID == (long)Constants.Seasons.Rabi
                                                 && ce.ED_CommandChannel.ID == _ChannelID
                                                 && ((tdc.Year == ce.Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
                                                     || (tdc.Year == (ce.Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
                                                 group ce by new { ce.Year, ce.CommandChannelID } into Season
                                                 select new
                                                 {
                                                     ParentChild = "P",
                                                     ChannelID = Season.FirstOrDefault().CommandChannelID,
                                                     ChannelName = Season.FirstOrDefault().ED_CommandChannel.CO_Channel.NAME,
                                                     Season = Season.FirstOrDefault().SeasonID,
                                                     Year = Season.FirstOrDefault().Year,
                                                     ChannelType = Season.FirstOrDefault().ED_CommandChannel.CO_Channel.CO_ChannelType.Name,
                                                     FlowType = Season.FirstOrDefault().ED_CommandChannel.CO_Channel.CO_ChannelFlowType.Name,
                                                     MAFEntitlement = context.ED_ChannelEntitlement.Where(x => x.ED_CommandChannel.ID == _ChannelID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                        && ((x.Year == Season.FirstOrDefault().Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                        || (x.Year == Season.FirstOrDefault().Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => (double?)x.MAFEntitlement).Sum(),
                                                     MAFDistribution = context.ED_ChannelDeliveries.Where(x => x.ED_CommandChannel.ID == _ChannelID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                        && ((x.Year == Season.FirstOrDefault().Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                        || (x.Year == Season.FirstOrDefault().Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => (double?)x.MAFDeliveries).Sum()
                                                 }).OrderByDescending(X => X.Year).ToList<dynamic>().Select(a => new
                                                 {
                                                     ParentChild = a.ParentChild,
                                                     ChannelID = a.ChannelID,
                                                     ChannelName = a.ChannelName,
                                                     Season = a.Season,
                                                     Year = a.Year,
                                                     ChannelType = a.ChannelType,
                                                     FlowType = a.FlowType,
                                                     MAFEntitlement = a.MAFEntitlement,
                                                     MAFDistribution = a.MAFDistribution //GetDeliveriesRabi(a.ChannelID, a.Year)
                                                 }).ToList<dynamic>();
            //);

            List<dynamic> lstKharifEntitlements = (from ce in context.ED_ChannelEntitlement
                                                   where (ce.Year == _Year || _Year == -1)
                                                   && (ce.SeasonID == (long)Constants.Seasons.EarlyKharif || ce.SeasonID == (long)Constants.Seasons.LateKharif)
                                                   && ce.ED_CommandChannel.ID == _ChannelID
                                                   group ce by new { ce.Year, ce.CommandChannelID } into Season
                                                   select new
                                                   {
                                                       ParentChild = "P",
                                                       ChannelID = Season.FirstOrDefault().CommandChannelID,
                                                       ChannelName = Season.FirstOrDefault().ED_CommandChannel.CO_Channel.NAME,
                                                       Season = (long)Constants.Seasons.Kharif,
                                                       Year = Season.FirstOrDefault().Year,
                                                       ChannelType = Season.FirstOrDefault().ED_CommandChannel.CO_Channel.CO_ChannelType.Name,
                                                       FlowType = Season.FirstOrDefault().ED_CommandChannel.CO_Channel.CO_ChannelFlowType.Name,
                                                       MAFEntitlement = Season.Sum(x => (double?)x.MAFEntitlement),
                                                       MAFDistribution = context.ED_ChannelDeliveries.Where(x => x.CommandChannelID == _ChannelID && x.SeasonID != (long)Constants.Seasons.Rabi && x.Year == Season.FirstOrDefault().Year).Select(x => (double?)x.MAFDeliveries).Sum()
                                                   }).OrderByDescending(X => X.Year).ToList<dynamic>()
                                                    .Select(a => new
                                                    {
                                                        ParentChild = a.ParentChild,
                                                        ChannelID = a.ChannelID,
                                                        ChannelName = a.ChannelName,
                                                        Season = a.Season,
                                                        Year = a.Year,
                                                        ChannelType = a.ChannelType,
                                                        FlowType = a.FlowType,
                                                        MAFEntitlement = a.MAFEntitlement,
                                                        MAFDistribution = a.MAFDistribution    //GetDeliveriesKharif(a.ChannelID, a.Year)
                                                    }).ToList<dynamic>();

            if (_lstSeasonIDs.Contains((long)Constants.Seasons.Rabi))
            {
                return lstRabiEntitlements;
            }
            else if (_lstSeasonIDs.Contains((long)Constants.Seasons.EarlyKharif) && _lstSeasonIDs.Contains((long)Constants.Seasons.LateKharif))
            {
                return lstKharifEntitlements;
            }
            else
            {
                return lstRabiEntitlements.Union(lstKharifEntitlements).OrderByDescending(x => x.Year).ToList<dynamic>();
            }


        }
        public double? GetMAFbyChannelIDRabi(long _channelID, long _Year)
        {
            return context.ED_ChildChannelEntitlement.Where(x => x.ChannelID == _channelID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                       && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                       || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => (double?)x.MAFEntitlement).Sum();
        }
        public List<dynamic> GetChildChannelByMainChannel(long _MainChannelID, List<long> _lstSeasonIDs, long _Year, long _CommandID)
        {
            List<dynamic> lstRabiEntitlements = (from ccc in context.ED_CommandChannelChilds
                                                 join ce in context.ED_ChildChannelEntitlement on ccc.ChannelID equals ce.ChannelID
                                                 join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
                                                 where (ce.Year == _Year)
                                                 && ce.SeasonID == (long)Constants.Seasons.Rabi
                                                 && ccc.CommandChannelID == _MainChannelID
                                                 && ccc.IsExcluded == false
                                                 && ((tdc.Year == _Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
                                                     || (tdc.Year == (_Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
                                                 group ce by new { ce.ChannelID, ce.Year } into Channel
                                                 select new
                                                 {
                                                     // ParentChild = "C",
                                                     ChannelID = Channel.FirstOrDefault().ChannelID,
                                                     ChannelName = Channel.FirstOrDefault().CO_Channel.NAME,
                                                     Season = Channel.FirstOrDefault().SeasonID,
                                                     Year = Channel.FirstOrDefault().Year,
                                                     //ChannelType = Channel.FirstOrDefault().CO_Channel.CO_ChannelType.Name,
                                                     // FlowType = Channel.FirstOrDefault().CO_Channel.CO_ChannelFlowType.Name,
                                                     //MAFEntitlement = context.ED_ChildChannelEntitlement.Where(x => x.ChannelID == Channel.FirstOrDefault().ChannelID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                     //    && ((x.Year == Channel.FirstOrDefault().Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                     //    || (x.Year == Channel.FirstOrDefault().Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => (double?)x.MAFEntitlement).Sum(),
                                                     //MAFDistribution = context.ED_TDailyGaugeReading.Where(x => x.ChannelID == _ChannelID && x.SeasonID == Channel.FirstOrDefault().SeasonID && x.Year == Channel.FirstOrDefault().Year).Select(x => x.DischargeMAF).Sum()
                                                     //MAFDistribution = context.ED_TDailyGaugeReading.Where(x => x.ChannelID == _ChannelID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                     //&& ((x.Year == Channel.FirstOrDefault().Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                     //|| (x.Year == Channel.FirstOrDefault().Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.DischargeMAF).Sum()
                                                 }).OrderByDescending(x => x.Year).ToList<dynamic>().Select(a => new
                                                 {
                                                     //ParentChild = a.ParentChild,
                                                     ChannelID = a.ChannelID,
                                                     ChannelName = a.ChannelName,
                                                     Season = a.Season,
                                                     Year = a.Year,
                                                     CommandID = _CommandID,
                                                     // ChannelType = a.ChannelType,
                                                     // FlowType = a.FlowType,
                                                     MAFEntitlement = GetMAFbyChannelIDRabi(a.ChannelID, a.Year),
                                                     //MAFDistribution = GetDeliveriesRabi(a.ChannelID, a.Year)
                                                 }).OrderBy(x => x.ChannelName).ToList<dynamic>();//.ToList<dynamic>();

            //List<dynamic> lstRabiEntitlements = (from ccc in context.ED_CommandChannelChilds
            //                                     join ce in context.ED_ChildChannelEntitlement on ccc.CommandChannelID equals ce.ChannelID
            //                                     join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
            //                                     where (ce.Year == _Year || _Year == -1)
            //                                     && ce.SeasonID == (long)Constants.Seasons.Rabi
            //                                     && ccc.CommandChannelID == _MainChannelID
            //                                     && ((tdc.Year == ce.Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
            //                                         || (tdc.Year == (ce.Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
            //                                     group ce by new { ce.ChannelID, ce.Year } into Channel
            //                                     select new
            //                                     {
            //                                         ParentChild = "C",
            //                                         ChannelID = Channel.FirstOrDefault().ChannelID,
            //                                         ChannelName = Channel.FirstOrDefault().CO_Channel.NAME,
            //                                         Season = Channel.FirstOrDefault().SeasonID,
            //                                         Year = Channel.FirstOrDefault().Year,
            //                                         ChannelType = Channel.FirstOrDefault().CO_Channel.CO_ChannelType.Name,
            //                                         FlowType = Channel.FirstOrDefault().CO_Channel.CO_ChannelFlowType.Name,
            //                                         MAFEntitlement = context.ED_ChildChannelEntitlement.Where(x => x.ChannelID == ccc.ChannelID && x.SeasonID == (long)Constants.Seasons.Rabi
            //                                             && ((x.Year == Channel.FirstOrDefault().Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
            //                                             || (x.Year == Channel.FirstOrDefault().Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => (double?)x.MAFEntitlement).Sum(),
            //                                         //MAFDistribution = context.ED_TDailyGaugeReading.Where(x => x.ChannelID == _ChannelID && x.SeasonID == Channel.FirstOrDefault().SeasonID && x.Year == Channel.FirstOrDefault().Year).Select(x => x.DischargeMAF).Sum()
            //                                         //MAFDistribution = context.ED_TDailyGaugeReading.Where(x => x.ChannelID == _ChannelID && x.SeasonID == (long)Constants.Seasons.Rabi
            //                                         //&& ((x.Year == Channel.FirstOrDefault().Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
            //                                         //|| (x.Year == Channel.FirstOrDefault().Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.DischargeMAF).Sum()
            //                                     }).OrderByDescending(x => x.Year).ToList<dynamic>().Select(a => new
            //                                     {
            //                                         ParentChild = a.ParentChild,
            //                                         ChannelID = a.ChannelID,
            //                                         ChannelName = a.ChannelName,
            //                                         Season = a.Season,
            //                                         Year = a.Year,
            //                                         ChannelType = a.ChannelType,
            //                                         FlowType = a.FlowType,
            //                                         MAFEntitlement = a.MAFEntitlement,
            //                                         MAFDistribution = GetDeliveriesRabi(a.ChannelID, a.Year)
            //                                     }).ToList<dynamic>();

            List<dynamic> lstKharifEntitlements = (from ccc in context.ED_CommandChannelChilds
                                                   join ce in context.ED_ChildChannelEntitlement on ccc.ChannelID equals ce.ChannelID
                                                   where (ce.Year == _Year || _Year == -1)
                                                   && (ce.SeasonID == (long)Constants.Seasons.EarlyKharif || ce.SeasonID == (long)Constants.Seasons.LateKharif)
                                                   && ccc.CommandChannelID == _MainChannelID
                                                   && ccc.IsExcluded == false
                                                   group ce by new { ce.ChannelID, ce.Year } into Channel
                                                   select new
                                                   {
                                                       //ParentChild = "C",
                                                       ChannelID = Channel.FirstOrDefault().ChannelID,
                                                       ChannelName = Channel.FirstOrDefault().CO_Channel.NAME,
                                                       Season = (long)Constants.Seasons.Kharif,
                                                       Year = Channel.FirstOrDefault().Year,
                                                       //ChannelType = Channel.FirstOrDefault().CO_Channel.CO_ChannelType.Name,
                                                       //FlowType = Channel.FirstOrDefault().CO_Channel.CO_ChannelFlowType.Name,
                                                       MAFEntitlement = Channel.Sum(x => (double?)x.MAFEntitlement),
                                                       //MAFDistribution = context.ED_TDailyGaugeReading.Where(x => x.ChannelID == _ChannelID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == Channel.FirstOrDefault().Year).Select(x => x.DischargeMAF).Sum()
                                                   }).OrderByDescending(x => x.Year).ToList<dynamic>().ToList<dynamic>().Select(a => new
                                                   {
                                                       //ParentChild = a.ParentChild,
                                                       ChannelID = a.ChannelID,
                                                       ChannelName = a.ChannelName,
                                                       Season = a.Season,
                                                       Year = a.Year,
                                                       CommandID = _CommandID,
                                                       //ChannelType = a.ChannelType,
                                                       //FlowType = a.FlowType,
                                                       MAFEntitlement = a.MAFEntitlement
                                                       // MAFDistribution = GetDeliveriesKharif(a.ChannelID, a.Year)
                                                   }).OrderBy(x => x.ChannelName).ToList<dynamic>();//.ToList<dynamic>();

            if (_lstSeasonIDs.Contains((long)Constants.Seasons.Rabi))
            {
                return lstRabiEntitlements;
            }
            else if (_lstSeasonIDs.Contains((long)Constants.Seasons.EarlyKharif) && _lstSeasonIDs.Contains((long)Constants.Seasons.LateKharif))
            {
                return lstKharifEntitlements;
            }
            else
            {
                return lstRabiEntitlements.Union(lstKharifEntitlements).OrderByDescending(x => x.ChannelName).ToList<dynamic>();
            }
        }
        public List<dynamic> GetChildChannelEntitlementBySearchCriteria(long _ChannelID, List<long> _lstSeasonIDs, long _Year)
        {

            List<dynamic> lstRabiEntitlements = (from ce in context.ED_ChildChannelEntitlement
                                                 join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
                                                 where (ce.Year == _Year || _Year == -1)
                                                 && ce.SeasonID == (long)Constants.Seasons.Rabi
                                                 && ce.ChannelID == _ChannelID
                                                 && ((tdc.Year == ce.Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
                                                     || (tdc.Year == (ce.Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
                                                 group ce by new { ce.ChannelID, ce.Year } into Channel
                                                 select new
                                                 {
                                                     ParentChild = "C",
                                                     ChannelID = Channel.FirstOrDefault().ChannelID,
                                                     ChannelName = Channel.FirstOrDefault().CO_Channel.NAME,
                                                     Season = Channel.FirstOrDefault().SeasonID,
                                                     Year = Channel.FirstOrDefault().Year,
                                                     ChannelType = Channel.FirstOrDefault().CO_Channel.CO_ChannelType.Name,
                                                     FlowType = Channel.FirstOrDefault().CO_Channel.CO_ChannelFlowType.Name,
                                                     MAFEntitlement = context.ED_ChildChannelEntitlement.Where(x => x.ChannelID == _ChannelID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                         && ((x.Year == Channel.FirstOrDefault().Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                         || (x.Year == Channel.FirstOrDefault().Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => (double?)x.MAFEntitlement).Sum(),
                                                     //MAFDistribution = context.ED_TDailyGaugeReading.Where(x => x.ChannelID == _ChannelID && x.SeasonID == Channel.FirstOrDefault().SeasonID && x.Year == Channel.FirstOrDefault().Year).Select(x => x.DischargeMAF).Sum()
                                                     //MAFDistribution = context.ED_TDailyGaugeReading.Where(x => x.ChannelID == _ChannelID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                     //&& ((x.Year == Channel.FirstOrDefault().Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                     //|| (x.Year == Channel.FirstOrDefault().Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.DischargeMAF).Sum()
                                                 }).OrderByDescending(x => x.Year).ToList<dynamic>().Select(a => new
                                                 {
                                                     ParentChild = a.ParentChild,
                                                     ChannelID = a.ChannelID,
                                                     ChannelName = a.ChannelName,
                                                     Season = a.Season,
                                                     Year = a.Year,
                                                     ChannelType = a.ChannelType,
                                                     FlowType = a.FlowType,
                                                     MAFEntitlement = a.MAFEntitlement,
                                                     MAFDistribution = GetDeliveriesRabi(a.ChannelID, a.Year)
                                                 }).ToList<dynamic>();

            List<dynamic> lstKharifEntitlements = (from ce in context.ED_ChildChannelEntitlement
                                                   where (ce.Year == _Year || _Year == -1)
                                                   && (ce.SeasonID == (long)Constants.Seasons.EarlyKharif || ce.SeasonID == (long)Constants.Seasons.LateKharif)
                                                   && ce.ChannelID == _ChannelID
                                                   group ce by new { ce.ChannelID, ce.Year } into Channel
                                                   select new
                                                   {
                                                       ParentChild = "C",
                                                       ChannelID = Channel.FirstOrDefault().ChannelID,
                                                       ChannelName = Channel.FirstOrDefault().CO_Channel.NAME,
                                                       Season = (long)Constants.Seasons.Kharif,
                                                       Year = Channel.FirstOrDefault().Year,
                                                       ChannelType = Channel.FirstOrDefault().CO_Channel.CO_ChannelType.Name,
                                                       FlowType = Channel.FirstOrDefault().CO_Channel.CO_ChannelFlowType.Name,
                                                       MAFEntitlement = Channel.Sum(x => (double?)x.MAFEntitlement),
                                                       //MAFDistribution = context.ED_TDailyGaugeReading.Where(x => x.ChannelID == _ChannelID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == Channel.FirstOrDefault().Year).Select(x => x.DischargeMAF).Sum()
                                                   }).OrderByDescending(x => x.Year).ToList<dynamic>().ToList<dynamic>().Select(a => new
                                                   {
                                                       ParentChild = a.ParentChild,
                                                       ChannelID = a.ChannelID,
                                                       ChannelName = a.ChannelName,
                                                       Season = a.Season,
                                                       Year = a.Year,
                                                       ChannelType = a.ChannelType,
                                                       FlowType = a.FlowType,
                                                       MAFEntitlement = a.MAFEntitlement,
                                                       MAFDistribution = GetDeliveriesKharif(a.ChannelID, a.Year)
                                                   }).ToList<dynamic>();

            if (_lstSeasonIDs.Contains((long)Constants.Seasons.Rabi))
            {
                return lstRabiEntitlements;
            }
            else if (_lstSeasonIDs.Contains((long)Constants.Seasons.EarlyKharif) && _lstSeasonIDs.Contains((long)Constants.Seasons.LateKharif))
            {
                return lstKharifEntitlements;
            }
            else
            {
                return lstRabiEntitlements.Union(lstKharifEntitlements).OrderByDescending(x => x.Year).ToList<dynamic>();
            }
        }

        public List<dynamic> ViewEntitlementsBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            List<dynamic> lstEntitlements = new List<dynamic>();
            if (_SeasonID == (long)Constants.Seasons.Rabi)
            {
                lstEntitlements = (from ce in context.ED_ChannelEntitlement
                                   join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
                                   where ((tdc.Year == _Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
                                                     || (tdc.Year == (_Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
                                   && ce.SeasonID == (long)Constants.Seasons.Rabi
                                   && ce.CommandChannelID == _ChannelID
                                   select new
                                   {
                                       TenDailyID = tdc.ID,
                                       TenDaily = tdc.ShortName,
                                       EntitlementCs = ce.CsEntitlement,
                                       ToDate = tdc.ToDate,
                                       EntitlementMAF = ce.MAFEntitlement
                                   }).OrderBy(x => x.TenDailyID).ToList<dynamic>();

                List<ED_ChannelDeliveries> lstDelivery = (from cd in context.ED_ChannelDeliveries
                                                          join tdc in context.SP_RefTDailyCalendar on cd.TDailyCalendarID equals tdc.ID
                                                          where ((tdc.Year == _Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
                                                                            || (tdc.Year == (_Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
                                                          && cd.SeasonID == (long)Constants.Seasons.Rabi
                                                          where cd.CommandChannelID == _ChannelID
                                                          select cd).ToList();


                lstEntitlements = (from ent in lstEntitlements
                                   join dev in lstDelivery on ent.TenDailyID equals dev.TDailyCalendarID into de
                                   from d in de.DefaultIfEmpty()
                                   select new
                                   {
                                       TenDailyID = ent.TenDailyID,
                                       TenDaily = ent.TenDaily,
                                       EntitlementCs = ent.EntitlementCs,
                                       DeliveriesCs = (double?)(d != null ? d.CsDeliveries : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       DeliveriesMAF = (double?)(d != null ? d.MAFDeliveries : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       EntitlementMAF = ent.EntitlementMAF
                                   }).ToList<dynamic>();
            }
            else
            {
                lstEntitlements = (from ce in context.ED_ChannelEntitlement
                                   join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
                                   where ce.Year == _Year
                                   && (ce.SeasonID == (long)Constants.Seasons.EarlyKharif || ce.SeasonID == (long)Constants.Seasons.LateKharif)
                                   && ce.CommandChannelID == _ChannelID
                                   select new
                                   {
                                       TenDailyID = tdc.ID,
                                       TenDaily = tdc.ShortName,
                                       EntitlementCs = ce.CsEntitlement,
                                       ToDate = tdc.ToDate,
                                       EntitlementMAF = ce.MAFEntitlement
                                   }).OrderBy(x => x.TenDailyID).ToList<dynamic>();

                List<ED_ChannelDeliveries> lstDelivery = (from cd in context.ED_ChannelDeliveries
                                                          join tdc in context.SP_RefTDailyCalendar on cd.TDailyCalendarID equals tdc.ID
                                                          where cd.Year == _Year
                                                           && (cd.SeasonID == (long)Constants.Seasons.EarlyKharif || cd.SeasonID == (long)Constants.Seasons.LateKharif)
                                                          where cd.CommandChannelID == _ChannelID && cd.Year == _Year
                                                          select cd).ToList();

                lstEntitlements = (from ent in lstEntitlements
                                   join dev in lstDelivery on ent.TenDailyID equals dev.TDailyCalendarID into de
                                   from d in de.DefaultIfEmpty()
                                   select new
                                   {
                                       TenDailyID = ent.TenDailyID,
                                       TenDaily = ent.TenDaily,
                                       EntitlementCs = ent.EntitlementCs,
                                       DeliveriesCs = (double?)(d != null ? d.CsDeliveries : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       DeliveriesMAF = (double?)(d != null ? d.MAFDeliveries : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       EntitlementMAF = ent.EntitlementMAF
                                   }).ToList<dynamic>();
            }
            return lstEntitlements;



            //List<dynamic> lstEntitlements = new List<dynamic>();
            //if (_SeasonID == (long)Constants.Seasons.Rabi)
            //{
            //    lstEntitlements = (from ce in context.ED_ChannelEntitlement
            //                       join cc in context.ED_CommandChannel on ce.CommandChannelID equals cc.ID
            //                       join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
            //                       where ((tdc.Year == _Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
            //                                         || (tdc.Year == (_Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
            //                       && ce.SeasonID == (long)Constants.Seasons.Rabi
            //                       && cc.ChannelID == _ChannelID
            //                       select new
            //                       {
            //                           TenDailyID = tdc.ID,
            //                           TenDaily = tdc.ShortName,
            //                           EntitlementCs = ce.CsEntitlement,
            //                           ToDate = tdc.ToDate,
            //                           EntitlementMAF = ce.MAFEntitlement
            //                       }).OrderBy(x => x.TenDailyID).ToList<dynamic>();

            //    List<dynamic> lstDeliveries = GetDeliveriesRabiForViewAndApprove(_ChannelID, _Year);

            //    lstEntitlements = (from ent in lstEntitlements
            //                       join dev in lstDeliveries on ent.TenDailyID equals dev.TdailyCalendarID into de
            //                       from d in de.DefaultIfEmpty()
            //                       select new
            //                       {
            //                           TenDailyID = ent.TenDailyID,
            //                           TenDaily = ent.TenDaily,
            //                           EntitlementCs = ent.EntitlementCs,
            //                           DeliveriesCs = (double?)(d != null ? d.DischargeCs : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
            //                           DeliveriesMAF = (double?)(d != null ? d.DischargeMAF : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
            //                           EntitlementMAF = ent.EntitlementMAF
            //                       }).ToList<dynamic>();
            //}
            //else
            //{
            //    lstEntitlements = (from ce in context.ED_ChannelEntitlement
            //                       join cc in context.ED_CommandChannel on ce.CommandChannelID equals cc.ID
            //                       join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
            //                       where ce.Year == _Year
            //                       && (ce.SeasonID == (long)Constants.Seasons.EarlyKharif || ce.SeasonID == (long)Constants.Seasons.LateKharif)
            //                       && cc.ChannelID == _ChannelID
            //                       select new
            //                       {
            //                           TenDailyID = tdc.ID,
            //                           TenDaily = tdc.ShortName,
            //                           EntitlementCs = ce.CsEntitlement,
            //                           ToDate = tdc.ToDate,
            //                           EntitlementMAF = ce.MAFEntitlement
            //                       }).OrderBy(x => x.TenDailyID).ToList<dynamic>();

            //    List<dynamic> lstDeliveries = GetDeliveriesKharifForViewAndApprove(_ChannelID, _Year);

            //    lstEntitlements = (from ent in lstEntitlements
            //                       join dev in lstDeliveries on ent.TenDailyID equals dev.TdailyCalendarID into de
            //                       from d in de.DefaultIfEmpty()
            //                       select new
            //                       {
            //                           TenDailyID = ent.TenDailyID,
            //                           TenDaily = ent.TenDaily,
            //                           EntitlementCs = ent.EntitlementCs,
            //                           DeliveriesCs = (double?)(d != null ? d.DischargeCs : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
            //                           DeliveriesMAF = (double?)(d != null ? d.DischargeMAF : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
            //                           EntitlementMAF = ent.EntitlementMAF
            //                       }).ToList<dynamic>();
            //}
            //return lstEntitlements;
        }

        /// <summary>
        /// This function gets the Rabi Season Average of channels with a particular command.
        /// Created On 30-01-2017
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_UserID"></param>
        /// <param name="_LstProvincialEntitlements"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> SaveRetrieveRabiAverageData(long _CommandID, int _Year, long _UserID, List<double> _LstProvincialEntitlements)
        {
            int UserID = Convert.ToInt32(_UserID);
            DateTime Now = DateTime.Now;
            short RabiSeasonID = (short)Constants.Seasons.Rabi;
            short Year = Convert.ToInt16(_Year);

            List<dynamic> lstSeasonalAverage = (from cc in context.ED_CommandChannel
                                                where cc.ChannelComndTypeID == _CommandID
                                                select new
                                                {
                                                    CommandTypeID = cc.ChannelComndTypeID,
                                                    CommandChannelID = cc.ID,
                                                    MAFFiveYr = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                 se.Year >= (Year - 5) && se.SeasonID == RabiSeasonID).Average(se => (double?)se.MAFEntitlement) ?? 0),
                                                    MAFTenYr = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                 se.Year >= (Year - 10) && se.SeasonID == RabiSeasonID).Average(se => (double?)se.MAFEntitlement) ?? 0),
                                                    MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == RabiSeasonID)
                                                                 .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                }).ToList<dynamic>();

            List<ED_SeasonalWeightedAvg> lstSeasonalWeightedAvg = lstSeasonalAverage.Select(lswa => new ED_SeasonalWeightedAvg
                                                                                                    {
                                                                                                        CommandChannelID = lswa.CommandChannelID,
                                                                                                        Year = Year,
                                                                                                        SeasonID = RabiSeasonID,
                                                                                                        PercentageTenYr = (lswa.MAFTenYr != 0 ? ((lswa.MAFTenYr / lstSeasonalAverage.Where(lsa => lsa.CommandTypeID == _CommandID).Sum(lsa => (double)lsa.MAFTenYr)) * 100) : 0),
                                                                                                        MAFTenYr = lswa.MAFTenYr,
                                                                                                        PercentageFiveYr = (lswa.MAFFiveYr != 0 ? ((lswa.MAFFiveYr / lstSeasonalAverage.Where(lsa => lsa.CommandTypeID == _CommandID).Sum(lsa => (double)lsa.MAFFiveYr)) * 100) : 0),
                                                                                                        MAFFiveYr = lswa.MAFFiveYr,
                                                                                                        SelectedYear = Convert.ToInt16(Year - 1),
                                                                                                        Percentage7782 = (lswa.MAF7782 != 0 ? ((lswa.MAF7782 / lstSeasonalAverage.Where(lsa => lsa.CommandTypeID == _CommandID).Sum(lsa => (double)lsa.MAF7782)) * 100) : 0),
                                                                                                        MAF7782 = lswa.MAF7782,
                                                                                                        CreatedBy = UserID,
                                                                                                        ModifiedBy = UserID,
                                                                                                        CreatedDate = Now,
                                                                                                        ModifiedDate = Now
                                                                                                    }).ToList<ED_SeasonalWeightedAvg>();

            context.ED_SeasonalWeightedAvg.AddRange(lstSeasonalWeightedAvg);

            List<dynamic> lstChannelAverage = (from cc in context.ED_CommandChannel
                                               from rtdc in (context.SP_RefTDailyCalendar.Where(rtdc => rtdc.SeasonID == RabiSeasonID && ((rtdc.Year == Year && (rtdc.TDailyID >= Constants.Oct1TDailyID && rtdc.TDailyID <= Constants.Dec3TDailyID)) ||
                                                              (rtdc.Year == (Year + 1) && (rtdc.TDailyID >= Constants.Jan1TDailyID && rtdc.TDailyID <= Constants.Mar3TDailyID)))).ToList())
                                               where cc.ChannelComndTypeID == _CommandID
                                               select new
                                               {
                                                   CommandChannelID = cc.ID,
                                                   TDailyCalendarID = rtdc.ID,
                                                   Year = rtdc.Year.Value,
                                                   MAFFiveYr = (context.ED_ChannelEntitlement.Where(ce => ce.CommandChannelID == cc.ID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID && ((ce.Year <= (Year - 1) && ce.Year >= (Year - 5) &&
                                                                  ce.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && ce.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID) || (ce.Year <= Year && ce.Year >= (Year - 4) &&
                                                                  ce.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && ce.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID)) && ce.SeasonID == RabiSeasonID).Average(ce => (double?)ce.MAFEntitlement) ?? 0),
                                                   MAFTenYr = (context.ED_ChannelEntitlement.Where(ce => ce.CommandChannelID == cc.ID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID && ((ce.Year <= (Year - 1) && ce.Year >= (Year - 10) &&
                                                                  ce.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && ce.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID) || (ce.Year <= Year && ce.Year >= (Year - 9) &&
                                                                  ce.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && ce.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID)) && ce.SeasonID == RabiSeasonID).Average(ce => (double?)ce.MAFEntitlement) ?? 0),
                                                   MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == RabiSeasonID && cf.TDailyID == rtdc.TDailyID)
                                                                .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                               }).ToList<dynamic>();

            List<ED_ChannelWeightedAvg> lstChannelWeightedAvg = lstChannelAverage.Select(lcwa => new ED_ChannelWeightedAvg
                                                                                                {
                                                                                                    CommandChannelID = lcwa.CommandChannelID,
                                                                                                    Year = lcwa.Year,
                                                                                                    SeasonID = RabiSeasonID,
                                                                                                    TDailyCalendarID = lcwa.TDailyCalendarID,
                                                                                                    PercentageTenYr = (lcwa.MAFTenYr != 0 ? ((lcwa.MAFTenYr / lstChannelAverage.Where(lca => lca.CommandChannelID == lcwa.CommandChannelID).Sum(lca => (double)lca.MAFTenYr)) * 100) : 0),
                                                                                                    MAFTenYr = lcwa.MAFTenYr,
                                                                                                    PercentageFiveYr = (lcwa.MAFFiveYr != 0 ? ((lcwa.MAFFiveYr / lstChannelAverage.Where(lca => lca.CommandChannelID == lcwa.CommandChannelID).Sum(lca => (double)lca.MAFFiveYr)) * 100) : 0),
                                                                                                    MAFFiveYr = lcwa.MAFFiveYr,
                                                                                                    Percentage7782 = (lcwa.MAF7782 != 0 ? ((lcwa.MAF7782 / lstChannelAverage.Where(lca => lca.CommandChannelID == lcwa.CommandChannelID).Sum(lca => (double)lca.MAF7782)) * 100) : 0),
                                                                                                    MAF7782 = lcwa.MAF7782,
                                                                                                    CreatedBy = UserID,
                                                                                                    ModifiedBy = UserID,
                                                                                                    CreatedDate = Now,
                                                                                                    ModifiedDate = Now
                                                                                                }).ToList<ED_ChannelWeightedAvg>();

            context.ED_ChannelWeightedAvg.AddRange(lstChannelWeightedAvg);

            context.SaveChanges();

            List<dynamic> lstAveragedData = GetAverageData(_CommandID, _Year, RabiSeasonID, _LstProvincialEntitlements);

            return lstAveragedData;
        }

        /// <summary>
        /// This function gets average data record for Command, Season and Year
        /// Created On 31-01-2017
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_SeasonID"></param>
        /// <param name="_LstProvincialEntitlements"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetAverageData(long _CommandID, int _Year, long _SeasonID, List<double> _LstProvincialEntitlements)
        {
            short SeasonID = Convert.ToInt16(_SeasonID);
            short Year = Convert.ToInt16(_Year);
            List<dynamic> lstAveragedData = null;

            if (SeasonID == (short)Constants.Seasons.Rabi)
            {
                double RabiProvincialEntitlement = _LstProvincialEntitlements.ElementAt(0);

                lstAveragedData = (from cc in context.ED_CommandChannel
                                   //join c in context.CO_Channel on cc.ChannelID equals c.ID
                                   join swa in context.ED_SeasonalWeightedAvg on cc.ID equals swa.CommandChannelID
                                   where cc.ChannelComndTypeID == _CommandID && swa.SeasonID == SeasonID && swa.Year == Year
                                   select new
                                   {
                                       SeasonalAverageID = swa.ID,
                                       CommandChannelID = cc.ID,
                                       ChannelName = cc.ChannelName,//c.NAME,
                                       MAFFiveYr = swa.MAFFiveYr,
                                       PercentageFiveYr = swa.PercentageFiveYr,
                                       MAFTenYr = swa.MAFTenYr,
                                       PercentageTenYr = swa.PercentageTenYr,
                                       MAF7782 = swa.MAF7782,
                                       Percentage7782 = swa.Percentage7782,
                                       ChannelEntitlement = ((swa.SelectedAvg == null) ? (RabiProvincialEntitlement * (swa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == SeasonID &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       SelectedYear = swa.SelectedYear,
                                       SelectedYearMAF = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year == swa.SelectedYear && se.SeasonID == _SeasonID)).Select(g => g.MAFEntitlement).FirstOrDefault(),
                                       SelectedYearPercentage = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year == swa.SelectedYear && se.SeasonID == _SeasonID)).Select(g => g.PercentageEntitlement).FirstOrDefault(),
                                       SelectedAvg = swa.SelectedAvg
                                   }).ToList<dynamic>();
            }
            else if (SeasonID == (short)Constants.Seasons.Kharif)
            {
                double EKProvincialEntitlement = _LstProvincialEntitlements.ElementAt(0);
                double LKProvincialEntitlement = _LstProvincialEntitlements.ElementAt(1);

                lstAveragedData = (from cc in context.ED_CommandChannel
                                   //join c in context.CO_Channel on cc.ChannelID equals c.ID
                                   join ekswa in
                                       (from swa in context.ED_SeasonalWeightedAvg
                                        where swa.ED_CommandChannel.ChannelComndTypeID == _CommandID && swa.SeasonID == (short)Constants.Seasons.EarlyKharif && swa.Year == Year
                                        select swa) on cc.ID equals ekswa.CommandChannelID
                                   join lkswa in
                                       (from swa in context.ED_SeasonalWeightedAvg
                                        where swa.ED_CommandChannel.ChannelComndTypeID == _CommandID && swa.SeasonID == (short)Constants.Seasons.LateKharif && swa.Year == Year
                                        select swa) on ekswa.CommandChannelID equals lkswa.CommandChannelID
                                   where cc.ChannelComndTypeID == _CommandID
                                   select new
                                   {
                                       EKSeasonalAverageID = ekswa.ID,
                                       LKSeasonalAverageID = lkswa.ID,
                                       CommandChannelID = cc.ID,
                                       ChannelName = cc.ChannelName,//c.NAME,
                                       EKMAFFiveYr = ekswa.MAFFiveYr,
                                       EKPercentageFiveYr = ekswa.PercentageFiveYr,
                                       LKMAFFiveYr = lkswa.MAFFiveYr,
                                       LKPercentageFiveYr = lkswa.PercentageFiveYr,
                                       EKMAFTenYr = ekswa.MAFTenYr,
                                       EKPercentageTenYr = ekswa.PercentageTenYr,
                                       LKMAFTenYr = lkswa.MAFTenYr,
                                       LKPercentageTenYr = lkswa.PercentageTenYr,
                                       EKMAF7782 = ekswa.MAF7782,
                                       EKPercentage7782 = ekswa.Percentage7782,
                                       LKMAF7782 = lkswa.MAF7782,
                                       LKPercentage7782 = lkswa.Percentage7782,
                                       EKChannelEntitlement = ((ekswa.SelectedAvg == null) ? (EKProvincialEntitlement * (ekswa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == (short)Constants.Seasons.EarlyKharif &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       LKChannelEntitlement = ((lkswa.SelectedAvg == null) ? (LKProvincialEntitlement * (lkswa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == (short)Constants.Seasons.LateKharif &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       EKSelectedAvg = ekswa.SelectedAvg,
                                       LKSelectedAvg = lkswa.SelectedAvg,
                                       EKSelectedYear = ekswa.SelectedYear,
                                       EKSelectedYearMAF = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year == ekswa.SelectedYear && se.SeasonID == (short)Constants.Seasons.EarlyKharif)).Select(g => g.MAFEntitlement).FirstOrDefault(),
                                       EKSelectedYearPercentage = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year == ekswa.SelectedYear && se.SeasonID == (short)Constants.Seasons.EarlyKharif)).Select(g => g.PercentageEntitlement).FirstOrDefault(),
                                       LKSelectedYear = lkswa.SelectedYear,
                                       LKSelectedYearMAF = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year == lkswa.SelectedYear && se.SeasonID == (short)Constants.Seasons.LateKharif)).Select(g => g.MAFEntitlement).FirstOrDefault(),
                                       LKSelectedYearPercentage = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year == lkswa.SelectedYear && se.SeasonID == (short)Constants.Seasons.LateKharif)).Select(g => g.PercentageEntitlement).FirstOrDefault(),
                                   }).ToList<dynamic>();
            }

            return lstAveragedData;
        }

        /// <summary>
        /// This function gets average data record for Command, Season and Year
        /// Created On 25-10-2017  (CR) 
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_SeasonID"></param>
        /// <param name="_LstProvincialEntitlements"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetAverageDataDeliveries(long _CommandID, int _Year, long _SeasonID, List<double> _LstProvincialEntitlements)
        {
            short SeasonID = Convert.ToInt16(_SeasonID);
            short Year = Convert.ToInt16(_Year);
            List<dynamic> lstAveragedData = null;

            if (SeasonID == (short)Constants.Seasons.Rabi)
            {
                double RabiProvincialEntitlement = _LstProvincialEntitlements.ElementAt(0);

                lstAveragedData = (from cc in context.ED_CommandChannel
                                   //join c in context.CO_Channel on cc.ChannelID equals c.ID
                                   join swa in context.ED_SeasonalWeightedAvgDeliveries on cc.ID equals swa.CommandChannelID
                                   where cc.ChannelComndTypeID == _CommandID && swa.SeasonID == SeasonID && swa.Year == Year
                                   select new
                                   {
                                       SeasonalAverageID = swa.ID,
                                       CommandChannelID = cc.ID,
                                       ChannelName = cc.ChannelName,//c.NAME,
                                       MAFFiveYr = swa.MAFFiveYr,
                                       PercentageFiveYr = swa.PercentageFiveYr,
                                       MAFTenYr = swa.MAFTenYr,
                                       PercentageTenYr = swa.PercentageTenYr,
                                       MAF7782 = swa.MAF7782,
                                       Percentage7782 = swa.Percentage7782,
                                       ChannelEntitlement = ((swa.SelectedAvg == null) ? (RabiProvincialEntitlement * (swa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == SeasonID &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       SelectedAvg = swa.SelectedAvg,
                                       SelectedYear = swa.SelectedYear,
                                       SelectedYearMAF = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year == swa.SelectedYear && se.SeasonID == _SeasonID)).Select(g => g.MAFDeliveries).FirstOrDefault(),
                                       SelectedYearPercentage = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year == swa.SelectedYear && se.SeasonID == _SeasonID)).Select(g => g.PercentageDeliveries).FirstOrDefault()
                                   }).ToList<dynamic>();
            }
            else if (SeasonID == (short)Constants.Seasons.Kharif)
            {
                double EKProvincialEntitlement = _LstProvincialEntitlements.ElementAt(0);
                double LKProvincialEntitlement = _LstProvincialEntitlements.ElementAt(1);

                lstAveragedData = (from cc in context.ED_CommandChannel
                                   //join c in context.CO_Channel on cc.ChannelID equals c.ID
                                   join ekswa in
                                       (from swa in context.ED_SeasonalWeightedAvgDeliveries
                                        where swa.ED_CommandChannel.ChannelComndTypeID == _CommandID && swa.SeasonID == (short)Constants.Seasons.EarlyKharif && swa.Year == Year
                                        select swa) on cc.ID equals ekswa.CommandChannelID
                                   join lkswa in
                                       (from swa in context.ED_SeasonalWeightedAvgDeliveries
                                        where swa.ED_CommandChannel.ChannelComndTypeID == _CommandID && swa.SeasonID == (short)Constants.Seasons.LateKharif && swa.Year == Year
                                        select swa) on ekswa.CommandChannelID equals lkswa.CommandChannelID
                                   where cc.ChannelComndTypeID == _CommandID
                                   select new
                                   {
                                       EKSeasonalAverageID = ekswa.ID,
                                       LKSeasonalAverageID = lkswa.ID,
                                       CommandChannelID = cc.ID,
                                       ChannelName = cc.ChannelName,//c.NAME,
                                       EKMAFFiveYr = ekswa.MAFFiveYr,
                                       EKPercentageFiveYr = ekswa.PercentageFiveYr,
                                       LKMAFFiveYr = lkswa.MAFFiveYr,
                                       LKPercentageFiveYr = lkswa.PercentageFiveYr,
                                       EKMAFTenYr = ekswa.MAFTenYr,
                                       EKPercentageTenYr = ekswa.PercentageTenYr,
                                       LKMAFTenYr = lkswa.MAFTenYr,
                                       LKPercentageTenYr = lkswa.PercentageTenYr,
                                       EKMAF7782 = ekswa.MAF7782,
                                       EKPercentage7782 = ekswa.Percentage7782,
                                       LKMAF7782 = lkswa.MAF7782,
                                       LKPercentage7782 = lkswa.Percentage7782,
                                       EKChannelEntitlement = ((ekswa.SelectedAvg == null) ? (EKProvincialEntitlement * (ekswa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == (short)Constants.Seasons.EarlyKharif &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       LKChannelEntitlement = ((lkswa.SelectedAvg == null) ? (LKProvincialEntitlement * (lkswa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == (short)Constants.Seasons.LateKharif &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       EKSelectedAvg = ekswa.SelectedAvg,
                                       LKSelectedAvg = lkswa.SelectedAvg,
                                       SelectedYearEK = ekswa.SelectedYear,
                                       SelectedYearMAFEK = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year == ekswa.SelectedYear && se.SeasonID == (short)Constants.Seasons.EarlyKharif)).Select(g => g.MAFDeliveries).FirstOrDefault(),
                                       SelectedYearPercentageEK = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year == ekswa.SelectedYear && se.SeasonID == (short)Constants.Seasons.EarlyKharif)).Select(g => g.PercentageDeliveries).FirstOrDefault(),
                                       SelectedYearLK = lkswa.SelectedYear,
                                       SelectedYearMAFLK = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year == lkswa.SelectedYear && se.SeasonID == (short)Constants.Seasons.LateKharif)).Select(g => g.MAFDeliveries).FirstOrDefault(),
                                       SelectedYearPercentageLK = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year == lkswa.SelectedYear && se.SeasonID == (short)Constants.Seasons.LateKharif)).Select(g => g.PercentageDeliveries).FirstOrDefault()
                                   }).ToList<dynamic>();
            }

            return lstAveragedData;
        }

        /// <summary>
        /// This function gets the Rabi Season Average of channels with a particular command.
        /// Created On 25-10-2017  (CR) 
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_UserID"></param>
        /// <param name="_LstProvincialEntitlements"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> SaveRetrieveRabiAverageDataDeliveries(long _CommandID, int _Year, long _UserID, List<double> _LstProvincialEntitlements)
        {
            int UserID = Convert.ToInt32(_UserID);
            DateTime Now = DateTime.Now;
            short RabiSeasonID = (short)Constants.Seasons.Rabi;
            short Year = Convert.ToInt16(_Year);

            List<dynamic> lstSeasonalAverage = (from cc in context.ED_CommandChannel
                                                where cc.ChannelComndTypeID == _CommandID
                                                select new
                                                {
                                                    CommandTypeID = cc.ChannelComndTypeID,
                                                    CommandChannelID = cc.ID,
                                                    MAFFiveYr = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                 se.Year >= (Year - 5) && se.SeasonID == RabiSeasonID).Average(se => (double?)se.MAFDeliveries) ?? 0),
                                                    MAFTenYr = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                 se.Year >= (Year - 10) && se.SeasonID == RabiSeasonID).Average(se => (double?)se.MAFDeliveries) ?? 0),
                                                    MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == RabiSeasonID)
                                                                 .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                }).ToList<dynamic>();

            List<ED_SeasonalWeightedAvgDeliveries> lstSeasonalWeightedAvg = lstSeasonalAverage.Select(lswa => new ED_SeasonalWeightedAvgDeliveries
                                                                                                                {
                                                                                                                    CommandChannelID = lswa.CommandChannelID,
                                                                                                                    Year = Year,
                                                                                                                    SeasonID = RabiSeasonID,
                                                                                                                    PercentageTenYr = (lswa.MAFTenYr != 0 ? ((lswa.MAFTenYr / lstSeasonalAverage.Where(lsa => lsa.CommandTypeID == _CommandID).Sum(lsa => (double)lsa.MAFTenYr)) * 100) : 0),
                                                                                                                    MAFTenYr = lswa.MAFTenYr,
                                                                                                                    PercentageFiveYr = (lswa.MAFFiveYr != 0 ? ((lswa.MAFFiveYr / lstSeasonalAverage.Where(lsa => lsa.CommandTypeID == _CommandID).Sum(lsa => (double)lsa.MAFFiveYr)) * 100) : 0),
                                                                                                                    MAFFiveYr = lswa.MAFFiveYr,
                                                                                                                    Percentage7782 = (lswa.MAF7782 != 0 ? ((lswa.MAF7782 / lstSeasonalAverage.Where(lsa => lsa.CommandTypeID == _CommandID).Sum(lsa => (double)lsa.MAF7782)) * 100) : 0),
                                                                                                                    MAF7782 = lswa.MAF7782,
                                                                                                                    SelectedYear = Convert.ToInt16(Year - 1),
                                                                                                                    CreatedBy = UserID,
                                                                                                                    ModifiedBy = UserID,
                                                                                                                    CreatedDate = Now,
                                                                                                                    ModifiedDate = Now
                                                                                                                }).ToList<ED_SeasonalWeightedAvgDeliveries>();

            context.ED_SeasonalWeightedAvgDeliveries.AddRange(lstSeasonalWeightedAvg);

            List<dynamic> lstChannelAverage = (from cc in context.ED_CommandChannel
                                               from rtdc in (context.SP_RefTDailyCalendar.Where(rtdc => rtdc.SeasonID == RabiSeasonID && ((rtdc.Year == Year && (rtdc.TDailyID >= Constants.Oct1TDailyID && rtdc.TDailyID <= Constants.Dec3TDailyID)) ||
                                                              (rtdc.Year == (Year + 1) && (rtdc.TDailyID >= Constants.Jan1TDailyID && rtdc.TDailyID <= Constants.Mar3TDailyID)))).ToList())
                                               where cc.ChannelComndTypeID == _CommandID
                                               select new
                                               {
                                                   CommandChannelID = cc.ID,
                                                   TDailyCalendarID = rtdc.ID,
                                                   Year = rtdc.Year.Value,
                                                   MAFFiveYr = (context.ED_ChannelDeliveries.Where(ce => ce.CommandChannelID == cc.ID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID && ((ce.Year <= (Year - 1) && ce.Year >= (Year - 5) &&
                                                                  ce.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && ce.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID) || (ce.Year <= Year && ce.Year >= (Year - 4) &&
                                                                  ce.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && ce.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID)) && ce.SeasonID == RabiSeasonID).Average(ce => (double?)ce.MAFDeliveries) ?? 0),
                                                   MAFTenYr = (context.ED_ChannelDeliveries.Where(ce => ce.CommandChannelID == cc.ID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID && ((ce.Year <= (Year - 1) && ce.Year >= (Year - 10) &&
                                                                  ce.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && ce.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID) || (ce.Year <= Year && ce.Year >= (Year - 9) &&
                                                                  ce.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && ce.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID)) && ce.SeasonID == RabiSeasonID).Average(ce => (double?)ce.MAFDeliveries) ?? 0),
                                                   MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == RabiSeasonID && cf.TDailyID == rtdc.TDailyID)
                                                                .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                               }).ToList<dynamic>();

            List<ED_ChannelWeightedAvgDeliveries> lstChannelWeightedAvg = lstChannelAverage.Select(lcwa => new ED_ChannelWeightedAvgDeliveries
                                                                                                            {
                                                                                                                CommandChannelID = lcwa.CommandChannelID,
                                                                                                                Year = lcwa.Year,
                                                                                                                SeasonID = RabiSeasonID,
                                                                                                                TDailyCalendarID = lcwa.TDailyCalendarID,
                                                                                                                PercentageTenYr = (lcwa.MAFTenYr != 0 ? ((lcwa.MAFTenYr / lstChannelAverage.Where(lca => lca.CommandChannelID == lcwa.CommandChannelID).Sum(lca => (double)lca.MAFTenYr)) * 100) : 0),
                                                                                                                MAFTenYr = lcwa.MAFTenYr,
                                                                                                                PercentageFiveYr = (lcwa.MAFFiveYr != 0 ? ((lcwa.MAFFiveYr / lstChannelAverage.Where(lca => lca.CommandChannelID == lcwa.CommandChannelID).Sum(lca => (double)lca.MAFFiveYr)) * 100) : 0),
                                                                                                                MAFFiveYr = lcwa.MAFFiveYr,
                                                                                                                Percentage7782 = (lcwa.MAF7782 != 0 ? ((lcwa.MAF7782 / lstChannelAverage.Where(lca => lca.CommandChannelID == lcwa.CommandChannelID).Sum(lca => (double)lca.MAF7782)) * 100) : 0),
                                                                                                                MAF7782 = lcwa.MAF7782,
                                                                                                                CreatedBy = UserID,
                                                                                                                ModifiedBy = UserID,
                                                                                                                CreatedDate = Now,
                                                                                                                ModifiedDate = Now
                                                                                                            }).ToList<ED_ChannelWeightedAvgDeliveries>();

            context.ED_ChannelWeightedAvgDeliveries.AddRange(lstChannelWeightedAvg);

            context.SaveChanges();

            List<dynamic> lstAveragedData = GetAverageDataDeliveries(_CommandID, _Year, RabiSeasonID, _LstProvincialEntitlements);

            return lstAveragedData;
        }

        /// <summary>
        /// This function gets average data record for Command, Season and Year
        /// Created On 31-01-2017
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_SeasonID"></param>
        /// <param name="_LstProvincialEntitlements"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetAverageDeliveriesData(long _CommandID, int _Year, long _SeasonID, List<double> _LstProvincialEntitlements)
        {
            short SeasonID = Convert.ToInt16(_SeasonID);
            short Year = Convert.ToInt16(_Year);
            List<dynamic> lstAveragedData = null;

            if (SeasonID == (short)Constants.Seasons.Rabi)
            {
                double RabiProvincialEntitlement = _LstProvincialEntitlements.ElementAt(0);

                lstAveragedData = (from cc in context.ED_CommandChannel
                                   // join c in context.CO_Channel on cc.ChannelID equals c.ID
                                   join swa in context.ED_SeasonalWeightedAvg on cc.ID equals swa.CommandChannelID
                                   where cc.ChannelComndTypeID == _CommandID && swa.SeasonID == SeasonID && swa.Year == Year
                                   select new
                                   {
                                       SeasonalAverageID = swa.ID,
                                       CommandChannelID = cc.ID,
                                       ChannelName = cc.ChannelName,//c.NAME,
                                       MAFFiveYr = swa.MAFFiveYr,
                                       PercentageFiveYr = swa.PercentageFiveYr,
                                       MAFTenYr = swa.MAFTenYr,
                                       PercentageTenYr = swa.PercentageTenYr,
                                       MAF7782 = swa.MAF7782,
                                       Percentage7782 = swa.Percentage7782,
                                       ChannelEntitlement = ((swa.SelectedAvg == null) ? (RabiProvincialEntitlement * (swa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == SeasonID &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       SelectedAvg = swa.SelectedAvg
                                   }).ToList<dynamic>();
            }
            else if (SeasonID == (short)Constants.Seasons.Kharif)
            {
                double EKProvincialEntitlement = _LstProvincialEntitlements.ElementAt(0);
                double LKProvincialEntitlement = _LstProvincialEntitlements.ElementAt(1);

                lstAveragedData = (from cc in context.ED_CommandChannel
                                   //join c in context.CO_Channel on cc.ChannelID equals c.ID
                                   join ekswa in
                                       (from swa in context.ED_SeasonalWeightedAvg
                                        where swa.ED_CommandChannel.ChannelComndTypeID == _CommandID && swa.SeasonID == (short)Constants.Seasons.EarlyKharif && swa.Year == Year
                                        select swa) on cc.ID equals ekswa.CommandChannelID
                                   join lkswa in
                                       (from swa in context.ED_SeasonalWeightedAvg
                                        where swa.ED_CommandChannel.ChannelComndTypeID == _CommandID && swa.SeasonID == (short)Constants.Seasons.LateKharif && swa.Year == Year
                                        select swa) on ekswa.CommandChannelID equals lkswa.CommandChannelID
                                   where cc.ChannelComndTypeID == _CommandID
                                   select new
                                   {
                                       EKSeasonalAverageID = ekswa.ID,
                                       LKSeasonalAverageID = lkswa.ID,
                                       CommandChannelID = cc.ID,
                                       ChannelName = cc.ChannelName,//c.NAME,
                                       EKMAFFiveYr = ekswa.MAFFiveYr,
                                       EKPercentageFiveYr = ekswa.PercentageFiveYr,
                                       LKMAFFiveYr = lkswa.MAFFiveYr,
                                       LKPercentageFiveYr = lkswa.PercentageFiveYr,
                                       EKMAFTenYr = ekswa.MAFTenYr,
                                       EKPercentageTenYr = ekswa.PercentageTenYr,
                                       LKMAFTenYr = lkswa.MAFTenYr,
                                       LKPercentageTenYr = lkswa.PercentageTenYr,
                                       EKMAF7782 = ekswa.MAF7782,
                                       EKPercentage7782 = ekswa.Percentage7782,
                                       LKMAF7782 = lkswa.MAF7782,
                                       LKPercentage7782 = lkswa.Percentage7782,
                                       EKChannelEntitlement = ((ekswa.SelectedAvg == null) ? (EKProvincialEntitlement * (ekswa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == (short)Constants.Seasons.EarlyKharif &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       LKChannelEntitlement = ((lkswa.SelectedAvg == null) ? (LKProvincialEntitlement * (lkswa.PercentageFiveYr / 100)) :
                                                            (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.SeasonID == (short)Constants.Seasons.LateKharif &&
                                                                se.Year == Year && se.IsApproved == false).Select(se => se.MAFEntitlement).FirstOrDefault())),
                                       EKSelectedAvg = ekswa.SelectedAvg,
                                       LKSelectedAvg = lkswa.SelectedAvg
                                   }).ToList<dynamic>();
            }

            return lstAveragedData;
        }

        /// <summary>
        /// This function gets the Kharif Season Average of channels with a particular command.
        /// Created On 31-01-2017
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_UserID"></param>
        /// <param name="_LstProvincialEntitlements"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> SaveRetrieveKharifAverageData(long _CommandID, int _Year, long _UserID, List<double> _LstProvincialEntitlements)
        {
            int UserID = Convert.ToInt32(_UserID);
            DateTime Now = DateTime.Now;
            short EarlyKharifSeasonID = (short)Constants.Seasons.EarlyKharif;
            short LateKharifSeasonID = (short)Constants.Seasons.LateKharif;
            short KharifSeasonID = (short)Constants.Seasons.Kharif;
            short Year = Convert.ToInt16(_Year);

            List<dynamic> lstEKSeasonalAverage = (from cc in context.ED_CommandChannel
                                                  where cc.ChannelComndTypeID == _CommandID
                                                  select new
                                                  {
                                                      CommandTypeID = cc.ChannelComndTypeID,
                                                      CommandChannelID = cc.ID,
                                                      MAFFiveYr = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                         se.Year >= (Year - 5) && se.SeasonID == EarlyKharifSeasonID).Average(se => (double?)se.MAFEntitlement) ?? 0),
                                                      MAFTenYr = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                         se.Year >= (Year - 10) && se.SeasonID == EarlyKharifSeasonID).Average(se => (double?)se.MAFEntitlement) ?? 0),
                                                      MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == EarlyKharifSeasonID)
                                                                 .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                  }).ToList<dynamic>();

            List<ED_SeasonalWeightedAvg> lstEKSeasonalWeightedAvg = lstEKSeasonalAverage.Select(lswa => new ED_SeasonalWeightedAvg
                                                                                                        {
                                                                                                            CommandChannelID = lswa.CommandChannelID,
                                                                                                            Year = Year,
                                                                                                            SeasonID = EarlyKharifSeasonID,
                                                                                                            PercentageTenYr = (lswa.MAFTenYr != 0 ? ((lswa.MAFTenYr / lstEKSeasonalAverage.Where(leksa => leksa.CommandTypeID == _CommandID).Sum(leksa => (double)leksa.MAFTenYr)) * 100) : 0),
                                                                                                            MAFTenYr = lswa.MAFTenYr,
                                                                                                            PercentageFiveYr = (lswa.MAFFiveYr != 0 ? ((lswa.MAFFiveYr / lstEKSeasonalAverage.Where(leksa => leksa.CommandTypeID == _CommandID).Sum(leksa => (double)leksa.MAFFiveYr)) * 100) : 0),
                                                                                                            MAFFiveYr = lswa.MAFFiveYr,
                                                                                                            Percentage7782 = (lswa.MAF7782 != 0 ? ((lswa.MAF7782 / lstEKSeasonalAverage.Where(leksa => leksa.CommandTypeID == _CommandID).Sum(leksa => (double)leksa.MAF7782)) * 100) : 0),
                                                                                                            MAF7782 = lswa.MAF7782,
                                                                                                            SelectedYear = Convert.ToInt16(Year - 1),
                                                                                                            CreatedBy = UserID,
                                                                                                            ModifiedBy = UserID,
                                                                                                            CreatedDate = Now,
                                                                                                            ModifiedDate = Now
                                                                                                        }).ToList<ED_SeasonalWeightedAvg>();

            context.ED_SeasonalWeightedAvg.AddRange(lstEKSeasonalWeightedAvg);

            List<dynamic> lstEKChannelAverage = (from cc in context.ED_CommandChannel
                                                 from rtdc in (context.SP_RefTDailyCalendar.Where(rtdc => rtdc.SeasonID == KharifSeasonID && rtdc.Year == Year && (rtdc.TDailyID >= Constants.Apr1TDailyID && rtdc.TDailyID <= Constants.Jun1TDailyID)).ToList())
                                                 where cc.ChannelComndTypeID == _CommandID
                                                 select new
                                                 {
                                                     CommandChannelID = cc.ID,
                                                     TDailyCalendarID = rtdc.ID,
                                                     MAFFiveYr = (context.ED_ChannelEntitlement.Where(ce => ce.CommandChannelID == cc.ID && ce.Year <= (Year - 1) && ce.Year >= (Year - 5) &&
                                                                    ce.SeasonID == EarlyKharifSeasonID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID).Average(ce => (double?)ce.MAFEntitlement) ?? 0),
                                                     MAFTenYr = (context.ED_ChannelEntitlement.Where(ce => ce.CommandChannelID == cc.ID && ce.Year <= (Year - 1) && ce.Year >= (Year - 10) &&
                                                                    ce.SeasonID == EarlyKharifSeasonID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID).Average(ce => (double?)ce.MAFEntitlement) ?? 0),
                                                     MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == EarlyKharifSeasonID && cf.TDailyID == rtdc.TDailyID)
                                                                  .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                 }).ToList<dynamic>();

            List<ED_ChannelWeightedAvg> lstEKChannelWeightedAvg = lstEKChannelAverage.Select(lcwa => new ED_ChannelWeightedAvg
                                                                                                    {
                                                                                                        CommandChannelID = lcwa.CommandChannelID,
                                                                                                        Year = Year,
                                                                                                        SeasonID = EarlyKharifSeasonID,
                                                                                                        TDailyCalendarID = lcwa.TDailyCalendarID,
                                                                                                        PercentageTenYr = (lcwa.MAFTenYr != 0 ? ((lcwa.MAFTenYr / lstEKChannelAverage.Where(lekca => lekca.CommandChannelID == lcwa.CommandChannelID).Sum(lekca => (double)lekca.MAFTenYr)) * 100) : 0),
                                                                                                        MAFTenYr = lcwa.MAFTenYr,
                                                                                                        PercentageFiveYr = (lcwa.MAFFiveYr != 0 ? ((lcwa.MAFFiveYr / lstEKChannelAverage.Where(lekca => lekca.CommandChannelID == lcwa.CommandChannelID).Sum(lekca => (double)lekca.MAFFiveYr)) * 100) : 0),
                                                                                                        MAFFiveYr = lcwa.MAFFiveYr,
                                                                                                        Percentage7782 = (lcwa.MAF7782 != 0 ? ((lcwa.MAF7782 / lstEKChannelAverage.Where(lekca => lekca.CommandChannelID == lcwa.CommandChannelID).Sum(lekca => (double)lekca.MAF7782)) * 100) : 0),
                                                                                                        MAF7782 = lcwa.MAF7782,
                                                                                                        CreatedBy = UserID,
                                                                                                        ModifiedBy = UserID,
                                                                                                        CreatedDate = Now,
                                                                                                        ModifiedDate = Now
                                                                                                    }).ToList<ED_ChannelWeightedAvg>();

            context.ED_ChannelWeightedAvg.AddRange(lstEKChannelWeightedAvg);

            List<dynamic> lstLKSeasonalAverage = (from cc in context.ED_CommandChannel
                                                  where cc.ChannelComndTypeID == _CommandID
                                                  select new
                                                  {
                                                      CommandTypeID = cc.ChannelComndTypeID,
                                                      CommandChannelID = cc.ID,
                                                      MAFFiveYr = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                         se.Year >= (Year - 5) && se.SeasonID == LateKharifSeasonID).Average(se => (double?)se.MAFEntitlement) ?? 0),
                                                      MAFTenYr = (context.ED_SeasonalEntitlement.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                         se.Year >= (Year - 10) && se.SeasonID == LateKharifSeasonID).Average(se => (double?)se.MAFEntitlement) ?? 0),
                                                      MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == LateKharifSeasonID)
                                                                 .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                  }).ToList<dynamic>();

            List<ED_SeasonalWeightedAvg> lstLKSeasonalWeightedAvg = lstLKSeasonalAverage.Select(lswa => new ED_SeasonalWeightedAvg
                                                                                                        {
                                                                                                            CommandChannelID = lswa.CommandChannelID,
                                                                                                            Year = Year,
                                                                                                            SeasonID = LateKharifSeasonID,
                                                                                                            PercentageTenYr = (lswa.MAFTenYr != 0 ? ((lswa.MAFTenYr / lstLKSeasonalAverage.Where(llksa => llksa.CommandTypeID == _CommandID).Sum(llksa => (double)llksa.MAFTenYr)) * 100) : 0),
                                                                                                            MAFTenYr = lswa.MAFTenYr,
                                                                                                            PercentageFiveYr = (lswa.MAFFiveYr != 0 ? ((lswa.MAFFiveYr / lstLKSeasonalAverage.Where(llksa => llksa.CommandTypeID == _CommandID).Sum(llksa => (double)llksa.MAFFiveYr)) * 100) : 0),
                                                                                                            MAFFiveYr = lswa.MAFFiveYr,
                                                                                                            Percentage7782 = (lswa.MAF7782 != 0 ? ((lswa.MAF7782 / lstLKSeasonalAverage.Where(llksa => llksa.CommandTypeID == _CommandID).Sum(llksa => (double)llksa.MAF7782)) * 100) : 0),
                                                                                                            MAF7782 = lswa.MAF7782,
                                                                                                            SelectedYear = Convert.ToInt16(Year - 1),
                                                                                                            CreatedBy = UserID,
                                                                                                            ModifiedBy = UserID,
                                                                                                            CreatedDate = Now,
                                                                                                            ModifiedDate = Now
                                                                                                        }).ToList<ED_SeasonalWeightedAvg>();

            context.ED_SeasonalWeightedAvg.AddRange(lstLKSeasonalWeightedAvg);

            List<dynamic> lstLKChannelAverage = (from cc in context.ED_CommandChannel
                                                 from rtdc in (context.SP_RefTDailyCalendar.Where(rtdc => rtdc.SeasonID == KharifSeasonID && rtdc.Year == Year && (rtdc.TDailyID >= Constants.Jun2TDailyID && rtdc.TDailyID <= Constants.Sep3TDailyID)).ToList())
                                                 where cc.ChannelComndTypeID == _CommandID
                                                 select new
                                                 {
                                                     CommandChannelID = cc.ID,
                                                     TDailyCalendarID = rtdc.ID,
                                                     MAFFiveYr = (context.ED_ChannelEntitlement.Where(ce => ce.CommandChannelID == cc.ID && ce.Year <= (Year - 1) && ce.Year >= (Year - 5) &&
                                                                    ce.SeasonID == LateKharifSeasonID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID).Average(ce => (double?)ce.MAFEntitlement) ?? 0),
                                                     MAFTenYr = (context.ED_ChannelEntitlement.Where(ce => ce.CommandChannelID == cc.ID && ce.Year <= (Year - 1) && ce.Year >= (Year - 10) &&
                                                                    ce.SeasonID == LateKharifSeasonID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID).Average(ce => (double?)ce.MAFEntitlement) ?? 0),
                                                     MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == LateKharifSeasonID && cf.TDailyID == rtdc.TDailyID)
                                                                  .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                 }).ToList<dynamic>();

            List<ED_ChannelWeightedAvg> lstLKChannelWeightedAvg = lstLKChannelAverage.Select(lcwa => new ED_ChannelWeightedAvg
                                                                                                       {
                                                                                                           CommandChannelID = lcwa.CommandChannelID,
                                                                                                           Year = Year,
                                                                                                           SeasonID = LateKharifSeasonID,
                                                                                                           TDailyCalendarID = lcwa.TDailyCalendarID,
                                                                                                           PercentageTenYr = (lcwa.MAFTenYr != 0 ? ((lcwa.MAFTenYr / lstLKChannelAverage.Where(llkca => llkca.CommandChannelID == lcwa.CommandChannelID).Sum(llkca => (double)llkca.MAFTenYr)) * 100) : 0),
                                                                                                           MAFTenYr = lcwa.MAFTenYr,
                                                                                                           PercentageFiveYr = (lcwa.MAFFiveYr != 0 ? ((lcwa.MAFFiveYr / lstLKChannelAverage.Where(llkca => llkca.CommandChannelID == lcwa.CommandChannelID).Sum(llkca => (double)llkca.MAFFiveYr)) * 100) : 0),
                                                                                                           MAFFiveYr = lcwa.MAFFiveYr,
                                                                                                           Percentage7782 = (lcwa.MAF7782 != 0 ? ((lcwa.MAF7782 / lstLKChannelAverage.Where(llkca => llkca.CommandChannelID == lcwa.CommandChannelID).Sum(llkca => (double)llkca.MAF7782)) * 100) : 0),
                                                                                                           MAF7782 = lcwa.MAF7782,
                                                                                                           CreatedBy = UserID,
                                                                                                           ModifiedBy = UserID,
                                                                                                           CreatedDate = Now,
                                                                                                           ModifiedDate = Now
                                                                                                       }).ToList<ED_ChannelWeightedAvg>();

            context.ED_ChannelWeightedAvg.AddRange(lstLKChannelWeightedAvg);

            context.SaveChanges();

            List<dynamic> lstAveragedData = GetAverageData(_CommandID, _Year, (long)Constants.Seasons.Kharif, _LstProvincialEntitlements);

            return lstAveragedData;
        }

        /// <summary>
        /// This function gets the Kharif Season Average of channels with a particular command.
        /// Created On 25-10-2017 (CR)
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_UserID"></param>
        /// <param name="_LstProvincialEntitlements"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> SaveRetrieveKharifAverageDataDeliveries(long _CommandID, int _Year, long _UserID, List<double> _LstProvincialEntitlements)
        {
            int UserID = Convert.ToInt32(_UserID);
            DateTime Now = DateTime.Now;
            short EarlyKharifSeasonID = (short)Constants.Seasons.EarlyKharif;
            short LateKharifSeasonID = (short)Constants.Seasons.LateKharif;
            short KharifSeasonID = (short)Constants.Seasons.Kharif;
            short Year = Convert.ToInt16(_Year);

            List<dynamic> lstEKSeasonalAverage = (from cc in context.ED_CommandChannel
                                                  where cc.ChannelComndTypeID == _CommandID
                                                  select new
                                                  {
                                                      CommandTypeID = cc.ChannelComndTypeID,
                                                      CommandChannelID = cc.ID,
                                                      MAFFiveYr = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                         se.Year >= (Year - 5) && se.SeasonID == EarlyKharifSeasonID).Average(se => (double?)se.MAFDeliveries) ?? 0),
                                                      MAFTenYr = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                         se.Year >= (Year - 10) && se.SeasonID == EarlyKharifSeasonID).Average(se => (double?)se.MAFDeliveries) ?? 0),
                                                      MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == EarlyKharifSeasonID)
                                                                 .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                  }).ToList<dynamic>();

            List<ED_SeasonalWeightedAvgDeliveries> lstEKSeasonalWeightedAvg = lstEKSeasonalAverage.Select(lswa => new ED_SeasonalWeightedAvgDeliveries
                                                                                                                    {
                                                                                                                        CommandChannelID = lswa.CommandChannelID,
                                                                                                                        Year = Year,
                                                                                                                        SeasonID = EarlyKharifSeasonID,
                                                                                                                        PercentageTenYr = (lswa.MAFTenYr != 0 ? ((lswa.MAFTenYr / lstEKSeasonalAverage.Where(leksa => leksa.CommandTypeID == _CommandID).Sum(leksa => (double)leksa.MAFTenYr)) * 100) : 0),
                                                                                                                        MAFTenYr = lswa.MAFTenYr,
                                                                                                                        PercentageFiveYr = (lswa.MAFFiveYr != 0 ? ((lswa.MAFFiveYr / lstEKSeasonalAverage.Where(leksa => leksa.CommandTypeID == _CommandID).Sum(leksa => (double)leksa.MAFFiveYr)) * 100) : 0),
                                                                                                                        MAFFiveYr = lswa.MAFFiveYr,
                                                                                                                        Percentage7782 = (lswa.MAF7782 != 0 ? ((lswa.MAF7782 / lstEKSeasonalAverage.Where(leksa => leksa.CommandTypeID == _CommandID).Sum(leksa => (double)leksa.MAF7782)) * 100) : 0),
                                                                                                                        MAF7782 = lswa.MAF7782,
                                                                                                                        SelectedYear = Convert.ToInt16(Year - 1),
                                                                                                                        CreatedBy = UserID,
                                                                                                                        ModifiedBy = UserID,
                                                                                                                        CreatedDate = Now,
                                                                                                                        ModifiedDate = Now
                                                                                                                    }).ToList<ED_SeasonalWeightedAvgDeliveries>();

            context.ED_SeasonalWeightedAvgDeliveries.AddRange(lstEKSeasonalWeightedAvg);

            List<dynamic> lstEKChannelAverage = (from cc in context.ED_CommandChannel
                                                 from rtdc in (context.SP_RefTDailyCalendar.Where(rtdc => rtdc.SeasonID == KharifSeasonID && rtdc.Year == Year && (rtdc.TDailyID >= Constants.Apr1TDailyID && rtdc.TDailyID <= Constants.Jun1TDailyID)).ToList())
                                                 where cc.ChannelComndTypeID == _CommandID
                                                 select new
                                                 {
                                                     CommandChannelID = cc.ID,
                                                     TDailyCalendarID = rtdc.ID,
                                                     MAFFiveYr = (context.ED_ChannelDeliveries.Where(ce => ce.CommandChannelID == cc.ID && ce.Year <= (Year - 1) && ce.Year >= (Year - 5) &&
                                                                    ce.SeasonID == EarlyKharifSeasonID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID).Average(ce => (double?)ce.MAFDeliveries) ?? 0),
                                                     MAFTenYr = (context.ED_ChannelDeliveries.Where(ce => ce.CommandChannelID == cc.ID && ce.Year <= (Year - 1) && ce.Year >= (Year - 10) &&
                                                                    ce.SeasonID == EarlyKharifSeasonID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID).Average(ce => (double?)ce.MAFDeliveries) ?? 0),
                                                     MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == EarlyKharifSeasonID && cf.TDailyID == rtdc.TDailyID)
                                                                  .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                 }).ToList<dynamic>();

            List<ED_ChannelWeightedAvgDeliveries> lstEKChannelWeightedAvg = lstEKChannelAverage.Select(lcwa => new ED_ChannelWeightedAvgDeliveries
                                                                                                                {
                                                                                                                    CommandChannelID = lcwa.CommandChannelID,
                                                                                                                    Year = Year,
                                                                                                                    SeasonID = EarlyKharifSeasonID,
                                                                                                                    TDailyCalendarID = lcwa.TDailyCalendarID,
                                                                                                                    PercentageTenYr = (lcwa.MAFTenYr != 0 ? ((lcwa.MAFTenYr / lstEKChannelAverage.Where(lekca => lekca.CommandChannelID == lcwa.CommandChannelID).Sum(lekca => (double)lekca.MAFTenYr)) * 100) : 0),
                                                                                                                    MAFTenYr = lcwa.MAFTenYr,
                                                                                                                    PercentageFiveYr = (lcwa.MAFFiveYr != 0 ? ((lcwa.MAFFiveYr / lstEKChannelAverage.Where(lekca => lekca.CommandChannelID == lcwa.CommandChannelID).Sum(lekca => (double)lekca.MAFFiveYr)) * 100) : 0),
                                                                                                                    MAFFiveYr = lcwa.MAFFiveYr,
                                                                                                                    Percentage7782 = (lcwa.MAF7782 != 0 ? ((lcwa.MAF7782 / lstEKChannelAverage.Where(lekca => lekca.CommandChannelID == lcwa.CommandChannelID).Sum(lekca => (double)lekca.MAF7782)) * 100) : 0),
                                                                                                                    MAF7782 = lcwa.MAF7782,
                                                                                                                    CreatedBy = UserID,
                                                                                                                    ModifiedBy = UserID,
                                                                                                                    CreatedDate = Now,
                                                                                                                    ModifiedDate = Now
                                                                                                                }).ToList<ED_ChannelWeightedAvgDeliveries>();

            context.ED_ChannelWeightedAvgDeliveries.AddRange(lstEKChannelWeightedAvg);

            List<dynamic> lstLKSeasonalAverage = (from cc in context.ED_CommandChannel
                                                  where cc.ChannelComndTypeID == _CommandID
                                                  select new
                                                  {
                                                      CommandTypeID = cc.ChannelComndTypeID,
                                                      CommandChannelID = cc.ID,
                                                      MAFFiveYr = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                         se.Year >= (Year - 5) && se.SeasonID == LateKharifSeasonID).Average(se => (double?)se.MAFDeliveries) ?? 0),
                                                      MAFTenYr = (context.ED_SeasonalDeliveries.Where(se => se.CommandChannelID == cc.ID && se.Year <= (Year - 1) &&
                                                                         se.Year >= (Year - 10) && se.SeasonID == LateKharifSeasonID).Average(se => (double?)se.MAFDeliveries) ?? 0),
                                                      MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == LateKharifSeasonID)
                                                                 .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                  }).ToList<dynamic>();

            List<ED_SeasonalWeightedAvgDeliveries> lstLKSeasonalWeightedAvg = lstLKSeasonalAverage.Select(lswa => new ED_SeasonalWeightedAvgDeliveries
                                                                                                                    {
                                                                                                                        CommandChannelID = lswa.CommandChannelID,
                                                                                                                        Year = Year,
                                                                                                                        SeasonID = LateKharifSeasonID,
                                                                                                                        PercentageTenYr = (lswa.MAFTenYr != 0 ? ((lswa.MAFTenYr / lstLKSeasonalAverage.Where(llksa => llksa.CommandTypeID == _CommandID).Sum(llksa => (double)llksa.MAFTenYr)) * 100) : 0),
                                                                                                                        MAFTenYr = lswa.MAFTenYr,
                                                                                                                        PercentageFiveYr = (lswa.MAFFiveYr != 0 ? ((lswa.MAFFiveYr / lstLKSeasonalAverage.Where(llksa => llksa.CommandTypeID == _CommandID).Sum(llksa => (double)llksa.MAFFiveYr)) * 100) : 0),
                                                                                                                        MAFFiveYr = lswa.MAFFiveYr,
                                                                                                                        Percentage7782 = (lswa.MAF7782 != 0 ? ((lswa.MAF7782 / lstLKSeasonalAverage.Where(llksa => llksa.CommandTypeID == _CommandID).Sum(llksa => (double)llksa.MAF7782)) * 100) : 0),
                                                                                                                        MAF7782 = lswa.MAF7782,
                                                                                                                        SelectedYear = Convert.ToInt16(Year - 1),
                                                                                                                        CreatedBy = UserID,
                                                                                                                        ModifiedBy = UserID,
                                                                                                                        CreatedDate = Now,
                                                                                                                        ModifiedDate = Now
                                                                                                                    }).ToList<ED_SeasonalWeightedAvgDeliveries>();

            context.ED_SeasonalWeightedAvgDeliveries.AddRange(lstLKSeasonalWeightedAvg);

            List<dynamic> lstLKChannelAverage = (from cc in context.ED_CommandChannel
                                                 from rtdc in (context.SP_RefTDailyCalendar.Where(rtdc => rtdc.SeasonID == KharifSeasonID && rtdc.Year == Year && (rtdc.TDailyID >= Constants.Jun2TDailyID && rtdc.TDailyID <= Constants.Sep3TDailyID)).ToList())
                                                 where cc.ChannelComndTypeID == _CommandID
                                                 select new
                                                 {
                                                     CommandChannelID = cc.ID,
                                                     TDailyCalendarID = rtdc.ID,
                                                     MAFFiveYr = (context.ED_ChannelDeliveries.Where(ce => ce.CommandChannelID == cc.ID && ce.Year <= (Year - 1) && ce.Year >= (Year - 5) &&
                                                                    ce.SeasonID == LateKharifSeasonID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID).Average(ce => (double?)ce.MAFDeliveries) ?? 0),
                                                     MAFTenYr = (context.ED_ChannelDeliveries.Where(ce => ce.CommandChannelID == cc.ID && ce.Year <= (Year - 1) && ce.Year >= (Year - 10) &&
                                                                    ce.SeasonID == LateKharifSeasonID && ce.SP_RefTDailyCalendar.TDailyID == rtdc.TDailyID).Average(ce => (double?)ce.MAFDeliveries) ?? 0),
                                                     MAF7782 = (context.ED_ChannelFlow7782.Where(cf => cf.CommandChannelID == cc.ID && cf.SeasonID == LateKharifSeasonID && cf.TDailyID == rtdc.TDailyID)
                                                                  .Sum(cf => (double?)cf.DischargeMAF) ?? 0)
                                                 }).ToList<dynamic>();

            List<ED_ChannelWeightedAvgDeliveries> lstLKChannelWeightedAvg = lstLKChannelAverage.Select(lcwa => new ED_ChannelWeightedAvgDeliveries
                                                                                                                {
                                                                                                                    CommandChannelID = lcwa.CommandChannelID,
                                                                                                                    Year = Year,
                                                                                                                    SeasonID = LateKharifSeasonID,
                                                                                                                    TDailyCalendarID = lcwa.TDailyCalendarID,
                                                                                                                    PercentageTenYr = (lcwa.MAFTenYr != 0 ? ((lcwa.MAFTenYr / lstLKChannelAverage.Where(llkca => llkca.CommandChannelID == lcwa.CommandChannelID).Sum(llkca => (double)llkca.MAFTenYr)) * 100) : 0),
                                                                                                                    MAFTenYr = lcwa.MAFTenYr,
                                                                                                                    PercentageFiveYr = (lcwa.MAFFiveYr != 0 ? ((lcwa.MAFFiveYr / lstLKChannelAverage.Where(llkca => llkca.CommandChannelID == lcwa.CommandChannelID).Sum(llkca => (double)llkca.MAFFiveYr)) * 100) : 0),
                                                                                                                    MAFFiveYr = lcwa.MAFFiveYr,
                                                                                                                    Percentage7782 = (lcwa.MAF7782 != 0 ? ((lcwa.MAF7782 / lstLKChannelAverage.Where(llkca => llkca.CommandChannelID == lcwa.CommandChannelID).Sum(llkca => (double)llkca.MAF7782)) * 100) : 0),
                                                                                                                    MAF7782 = lcwa.MAF7782,
                                                                                                                    CreatedBy = UserID,
                                                                                                                    ModifiedBy = UserID,
                                                                                                                    CreatedDate = Now,
                                                                                                                    ModifiedDate = Now
                                                                                                                }).ToList<ED_ChannelWeightedAvgDeliveries>();

            context.ED_ChannelWeightedAvgDeliveries.AddRange(lstLKChannelWeightedAvg);

            context.SaveChanges();

            List<dynamic> lstAveragedData = GetAverageDataDeliveries(_CommandID, _Year, (long)Constants.Seasons.Kharif, _LstProvincialEntitlements);

            return lstAveragedData;
        }

        public List<dynamic> ViewChildEntitlementsBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            List<dynamic> lstEntitlements = new List<dynamic>();

            if (_SeasonID == (long)Constants.Seasons.Rabi)
            {
                lstEntitlements = (from ce in context.ED_ChildChannelEntitlement
                                   join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
                                   //join tdg in context.ED_TDailyGaugeReading on new { ID = ce.TDailyCalendarID, ChannelID = ce.ChannelID } equals new { ID = tdg.TDailyCalendarID, ChannelID = tdg.ChannelID } into ps
                                   //from tdgr in ps.DefaultIfEmpty()
                                   where ((tdc.Year == _Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
                                                     || (tdc.Year == (_Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
                                   && ce.SeasonID == (long)Constants.Seasons.Rabi
                                   && ce.ChannelID == _ChannelID
                                   select new
                                   {
                                       TenDailyID = tdc.ID,
                                       TenDaily = tdc.ShortName,
                                       EntitlementCs = ce.CsEntitlement,
                                       ToDate = tdc.ToDate,
                                       //DeliveriesCs = (double?)(tdgr.DischargeCs != null ? tdgr.DischargeCs : (tdc.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       //DeliveriesMAF = (tdgr.DischargeMAF != null ? tdgr.DischargeMAF : (tdc.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       EntitlementMAF = ce.MAFEntitlement
                                   }).OrderBy(x => x.TenDailyID).ToList<dynamic>();

                List<dynamic> lstDeliveries = GetDeliveriesRabiForViewAndApprove(_ChannelID, _Year);

                lstEntitlements = (from ent in lstEntitlements
                                   join dev in lstDeliveries on ent.TenDailyID equals dev.TdailyCalendarID into de
                                   from d in de.DefaultIfEmpty()
                                   select new
                                   {
                                       TenDailyID = ent.TenDailyID,
                                       TenDaily = ent.TenDaily,
                                       EntitlementCs = ent.EntitlementCs,
                                       DeliveriesCs = (double?)(d != null ? d.DischargeCs : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       DeliveriesMAF = (double?)(d != null ? d.DischargeMAF * 1000 : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       EntitlementMAF = ent.EntitlementMAF
                                   }).ToList<dynamic>();
            }
            else
            {
                lstEntitlements = (from ce in context.ED_ChildChannelEntitlement
                                   join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
                                   //join tdg in context.ED_TDailyGaugeReading on new { ID = ce.TDailyCalendarID, ChannelID = ce.ChannelID } equals new { ID = tdg.TDailyCalendarID, ChannelID = tdg.ChannelID } into ps
                                   //from tdgr in ps.DefaultIfEmpty()
                                   where ce.Year == _Year
                                   && (ce.SeasonID == (long)Constants.Seasons.EarlyKharif || ce.SeasonID == (long)Constants.Seasons.LateKharif)
                                   && ce.ChannelID == _ChannelID
                                   select new
                                   {
                                       TenDailyID = tdc.ID,
                                       TenDaily = tdc.ShortName,
                                       EntitlementCs = ce.CsEntitlement,
                                       ToDate = tdc.ToDate,
                                       //DeliveriesCs = (double?)(tdgr.DischargeCs != null ? tdgr.DischargeCs : (tdc.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       //DeliveriesMAF = (tdgr.DischargeMAF != null ? tdgr.DischargeMAF : (tdc.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       EntitlementMAF = ce.MAFEntitlement
                                   }).OrderBy(x => x.TenDailyID).ToList<dynamic>();

                List<dynamic> lstDeliveries = GetDeliveriesKharifForViewAndApprove(_ChannelID, _Year);

                lstEntitlements = (from ent in lstEntitlements
                                   join dev in lstDeliveries on ent.TenDailyID equals dev.TdailyCalendarID into de
                                   from d in de.DefaultIfEmpty()
                                   select new
                                   {
                                       TenDailyID = ent.TenDailyID,
                                       TenDaily = ent.TenDaily,
                                       EntitlementCs = ent.EntitlementCs,
                                       DeliveriesCs = (double?)(d != null ? d.DischargeCs : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       DeliveriesMAF = (double?)(d != null ? d.DischargeMAF * 1000 : (ent.ToDate >= DateTime.Now ? (double?)null : 0)),
                                       EntitlementMAF = ent.EntitlementMAF
                                   }).ToList<dynamic>();
            }
            return lstEntitlements;
        }

        public DataTable GetTenDailyBySeasonYearCommand(long _Year, long _SeasonID, long _CommandID)
        {
            List<ED_ChannelEntitlement> lstEntitlements = null;

            if (_SeasonID == (long)Constants.Seasons.Rabi)
            {
                lstEntitlements = context.ED_ChannelEntitlement.Where(x => ((x.SP_RefTDailyCalendar.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                     || (x.SP_RefTDailyCalendar.Year == (_Year + 1) && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID)) && x.SeasonID == (long)Constants.Seasons.Rabi && x.ED_CommandChannel.ChannelComndTypeID == _CommandID).ToList();
            }
            else if (_SeasonID == (long)Constants.Seasons.Kharif)
            {
                lstEntitlements = context.ED_ChannelEntitlement.Where(x => x.Year == _Year && (x.SeasonID == (long)Constants.Seasons.EarlyKharif || x.SeasonID == (long)Constants.Seasons.LateKharif) && x.ED_CommandChannel.ChannelComndTypeID == _CommandID).ToList();
            }

            List<ED_CommandChannel> lstChannels = context.ED_CommandChannel.Where(x => x.ChannelComndTypeID == _CommandID).ToList<ED_CommandChannel>();

            DataTable table = new DataTable();

            double RowSum = 0;
            double TotalSum = 0;

            table.Columns.Add("TenDay", typeof(string));
            table.Columns.Add("TenDailyID", typeof(int));
            for (int i = 0; i < lstChannels.Count; i++)
            {
                table.Columns.Add(lstChannels[i].CO_Channel.ChannelAbbreviation, typeof(float));
                table.Columns.Add(lstChannels[i].CO_Channel.ChannelAbbreviation + "ID", typeof(int));
            }

            table.Columns.Add("Total", typeof(float));

            int z = 0;
            double Sum7782 = 0;

            double Sum7782EarlyKharif = 0;
            double Sum7782LateKharif = 0;


            double SumEarlyKharif = 0;
            double SumLateKharif = 0;

            DataRow drDesignDischarge = table.NewRow();
            DataRow dr7782Earlykharif = table.NewRow();
            DataRow dr7782Latekharif = table.NewRow();
            DataRow dr7782 = table.NewRow();


            drDesignDischarge[0] = "Design Dis.";
            dr7782[0] = "77-82 (MAF)";

            dr7782Earlykharif[0] = "E.K 77-82 (MAF)";
            dr7782Latekharif[0] = "L.K 77-82 (MAF)";

            List<dynamic> lstTenDailyIDs = lstEntitlements.Select(x => new { ID = x.TDailyCalendarID, Name = x.SP_RefTDailyCalendar.ShortName, TDailyID = x.SP_RefTDailyCalendar.TDailyID }).Distinct().OrderBy(x => x.TDailyID).ToList<dynamic>();

            DataRow drTotalRow = table.NewRow();
            drTotalRow[0] = _SeasonID != (long)Constants.Seasons.Rabi ? "Kharif (MAF)" : "Total (MAF)";

            DataRow drShortage = table.NewRow();
            drShortage[0] = "Shortage (%)";

            DataRow drShortageEarlyKharif = table.NewRow();
            DataRow drShortageLateKharif = table.NewRow();

            drShortageEarlyKharif[0] = "Shortage E.K (%)";
            drShortageLateKharif[0] = "Shortage L.K (%)";

            DataRow drEarlyKharif = table.NewRow();
            drEarlyKharif[0] = "E.K (MAF)";

            DataRow drLateKharif = table.NewRow();
            drLateKharif[0] = "L.K (MAF)";

            DataRow drRow = table.NewRow();

            int ChannelPlusIDCount = lstChannels.Count * 2;

            for (int i = 0; i < lstTenDailyIDs.Count; i++)
            {
                drRow[0] = lstTenDailyIDs[i].Name;
                drRow[1] = lstTenDailyIDs[i].ID;

                for (int j = 0; j < ChannelPlusIDCount; j = j + 2)
                {
                    ED_ChannelEntitlement mdlChannelEntitlement = lstEntitlements.Where(x => x.ED_CommandChannel.CO_Channel.ChannelAbbreviation == lstChannels[j / 2].CO_Channel.ChannelAbbreviation && x.TDailyCalendarID == lstTenDailyIDs.ElementAt(i).ID).OrderByDescending(x => x.ID).FirstOrDefault();
                    double Cusecs = mdlChannelEntitlement.CsEntitlement;
                    double MAF = mdlChannelEntitlement.MAFEntitlement;
                    long ID = mdlChannelEntitlement.ID;

                    drRow[j + 2] = Cusecs;
                    drRow[j + 3] = ID;



                    //MAF = Math.Round(MAF, 3);
                    RowSum = RowSum + MAF;

                    var a = drTotalRow[j + 2].ToString();
                    if (a != null && a != string.Empty)
                    {
                        drTotalRow[j + 2] = MAF + Convert.ToDouble(a);

                    }
                    else
                    {
                        drTotalRow[j + 2] = MAF;
                    }
                    if (i <= 6)
                    {
                        var b = drEarlyKharif[j + 2].ToString();
                        if (b != null && b != string.Empty)
                        {
                            drEarlyKharif[j + 2] = MAF + Convert.ToDouble(b);
                        }
                        else
                        {
                            drEarlyKharif[j + 2] = MAF;
                        }

                    }
                    else
                    {
                        var c = drLateKharif[j + 2].ToString();
                        if (c != null && c != string.Empty)
                        {
                            drLateKharif[j + 2] = MAF + Convert.ToDouble(c);
                        }
                        else
                        {
                            drLateKharif[j + 2] = MAF;
                        }
                    }
                }

                drRow[ChannelPlusIDCount + 2] = RowSum;
                SumLateKharif = RowSum - SumEarlyKharif;
                TotalSum = TotalSum + RowSum;
                drTotalRow[ChannelPlusIDCount + 2] = TotalSum;





                table.Rows.Add(drRow);

                drRow = table.NewRow();
                RowSum = 0;
            }

            foreach (DataColumn column in table.Columns)
            {
                ED_CommandChannel mdlCommandChannel = lstChannels.Where(x => x.CO_Channel.ChannelAbbreviation.ToUpper().Trim() == column.ColumnName.ToUpper().Trim()).FirstOrDefault();

                if (mdlCommandChannel != null)
                {
                    if (_SeasonID == (long)Constants.Seasons.Rabi)
                    {
                        Sum7782 = context.ED_ChannelFlow7782.Where(x => x.SeasonID == (long)Constants.Seasons.Rabi && x.CommandChannelID == mdlCommandChannel.ID).Select(x => x.DischargeMAF).Sum();
                        drDesignDischarge[z] = mdlCommandChannel.DesignDischargeRabi;
                        dr7782[z] = Sum7782;
                        drShortage[z] = GetShortage(Convert.ToDouble(drTotalRow[z]), Sum7782);
                    }
                    else
                    {
                        Sum7782EarlyKharif = context.ED_ChannelFlow7782.Where(x => x.SeasonID == (long)Constants.Seasons.EarlyKharif && x.CommandChannelID == mdlCommandChannel.ID).Select(x => x.DischargeMAF).Sum();
                        Sum7782LateKharif = context.ED_ChannelFlow7782.Where(x => x.SeasonID == (long)Constants.Seasons.LateKharif && x.CommandChannelID == mdlCommandChannel.ID).Select(x => x.DischargeMAF).Sum();


                        drDesignDischarge[z] = mdlCommandChannel.DesignDischarge;
                        dr7782Earlykharif[z] = Sum7782EarlyKharif;
                        dr7782Latekharif[z] = Sum7782LateKharif;
                        drShortageEarlyKharif[z] = GetShortage(Convert.ToDouble(drEarlyKharif[z]), Sum7782EarlyKharif);
                        drShortageLateKharif[z] = GetShortage(Convert.ToDouble(drLateKharif[z]), Sum7782LateKharif);
                    }


                }
                z++;
            }
            table.Rows.InsertAt(drDesignDischarge, 0);
            if (_SeasonID != (long)Constants.Seasons.Rabi)
            {

                table.Rows.Add(drEarlyKharif);
                table.Rows.Add(drLateKharif);
                table.Rows.Add(drTotalRow);
                table.Rows.Add(dr7782Earlykharif);
                table.Rows.Add(dr7782Latekharif);
                table.Rows.Add(drShortageEarlyKharif);
                table.Rows.Add(drShortageLateKharif);
            }
            else
            {
                table.Rows.Add(drTotalRow);
                table.Rows.Add(dr7782);
                table.Rows.Add(drShortage);
            }


            //int rowCounter = 1;
            //for (int i = 0; i < drTotalRow.Table.Columns.Count; i++)
            //{
            //    double price;
            //    bool isVTDouble = Double.TryParse(drTotalRow[i].ToString(), out price);
            //    bool isV7782Double = Double.TryParse(dr7782[i].ToString(), out price);
            //    if (isVTDouble)
            //    {
            //        if (isV7782Double)
            //        {
            //            if (i%2==0)
            //            {
            //                double tValue = Convert.ToDouble(drTotalRow[i].ToString());
            //                double Value7782 = Convert.ToDouble(dr7782[i]);
            //                double calculateShortageValue = ((tValue / Value7782) * 100) - 100;
            //                drShortage[rowCounter] = calculateShortageValue;
            //                rowCounter++;
            //            }

            //        }
            //    }






            //}
            //table.Rows.Add(drShortage);
            return table;
        }

        public double GetShortage(double total, double value7782)
        {
            double result = 0;

            if (value7782 != 0)
            {
                result = (((Math.Round(total, 3) / (Math.Round(value7782, 3))) * 100) - 100);
            }

            return result;
        }

        /// <summary>
        /// This function saves the list of Seasonal Entitlements
        /// Created On 08-02-2017
        /// </summary>
        /// <param name="_LstSeasonalEntitlement"></param>
        /// <returns></returns>
        public bool AddSeasonalEntitlements(List<ED_SeasonalEntitlement> _LstSeasonalEntitlement)
        {
            context.ED_SeasonalEntitlement.AddRange(_LstSeasonalEntitlement);
            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// This function sets Rabi Selected Average value in ED_SeasonalWeightedAvg table
        /// Created On 09-02-2017
        /// </summary>
        /// <param name="_LstSeasonalAverageIDs"></param>
        /// <param name="_SelectedAverage"></param>
        /// <returns>bool</returns>
        public bool UpdateRabiAverageSelected(List<long> _LstSeasonalAverageIDs, string _SelectedAverage, short _SelectedYear)
        {
            List<ED_SeasonalWeightedAvg> lstSeasonalWeightedAvg = context.ED_SeasonalWeightedAvg.Where(swa => _LstSeasonalAverageIDs.Contains(swa.ID)).ToList();

            foreach (ED_SeasonalWeightedAvg mdlSeasonalWeightedAvg in lstSeasonalWeightedAvg)
            {
                mdlSeasonalWeightedAvg.SelectedAvg = _SelectedAverage;
                mdlSeasonalWeightedAvg.SelectedYear = _SelectedYear;
            }

            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// This function sets Deliveries Rabi Selected Average value in ED_SeasonalWeightedAvg table
        /// Created On 27-10-2017
        /// </summary>
        /// <param name="_LstSeasonalAverageIDs"></param>
        /// <param name="_SelectedAverage"></param>
        /// <returns>bool</returns>
        public bool UpdateDeliveriesRabiAverageSelected(List<long> _LstSeasonalAverageIDs, string _SelectedAverage, short _SelectedYear)
        {
            List<ED_SeasonalWeightedAvgDeliveries> lstSeasonalWeightedAvgDeliveries = context.ED_SeasonalWeightedAvgDeliveries.Where(swad => _LstSeasonalAverageIDs.Contains(swad.ID)).ToList();

            foreach (ED_SeasonalWeightedAvgDeliveries mdlSeasonalWeightedAvgDeliveries in lstSeasonalWeightedAvgDeliveries)
            {
                mdlSeasonalWeightedAvgDeliveries.SelectedAvg = _SelectedAverage;
                mdlSeasonalWeightedAvgDeliveries.SelectedYear = _SelectedYear;
            }

            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// This function sets Kharif Selected Average value in ED_SeasonalWeightedAvg table
        /// Created On 09-02-2017
        /// </summary>
        /// <param name="_LstSeasonalAverageIDs"></param>
        /// <param name="_EKSelectedAverage"></param>
        /// <param name="_LKSelectedAverage"></param>
        /// <returns>bool</returns>
        public bool UpdateKharifAverageSelected(List<long> _LstSeasonalAverageIDs, string _EKSelectedAverage, string _LKSelectedAverage, short _EKSelectedYear, short _LKSelectedYear)
        {
            List<ED_SeasonalWeightedAvg> lstSeasonalWeightedAvg = context.ED_SeasonalWeightedAvg.Where(swa => _LstSeasonalAverageIDs.Contains(swa.ID)).ToList();

            foreach (ED_SeasonalWeightedAvg mdlSeasonalWeightedAvg in lstSeasonalWeightedAvg)
            {
                if (mdlSeasonalWeightedAvg.SeasonID == (short)Constants.Seasons.EarlyKharif)
                {
                    mdlSeasonalWeightedAvg.SelectedAvg = _EKSelectedAverage;
                    mdlSeasonalWeightedAvg.SelectedYear = _EKSelectedYear;
                }
                else if (mdlSeasonalWeightedAvg.SeasonID == (short)Constants.Seasons.LateKharif)
                {
                    mdlSeasonalWeightedAvg.SelectedAvg = _LKSelectedAverage;
                    mdlSeasonalWeightedAvg.SelectedYear = _LKSelectedYear;
                }
            }

            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// This function sets Kharif Selected Average value in ED_SeasonalWeightedAvg table
        /// Created On 09-02-2017
        /// </summary>
        /// <param name="_LstSeasonalAverageIDs"></param>
        /// <param name="_EKSelectedAverage"></param>
        /// <param name="_LKSelectedAverage"></param>
        /// <returns>bool</returns>
        public bool UpdateKharifAverageSelectedDeliveries(List<long> _LstSeasonalAverageIDs, string _EKSelectedAverage, string _LKSelectedAverage, short _EKYear, short _LKYear)
        {
            List<ED_SeasonalWeightedAvgDeliveries> lstSeasonalWeightedAvg = context.ED_SeasonalWeightedAvgDeliveries.Where(swa => _LstSeasonalAverageIDs.Contains(swa.ID)).ToList();

            foreach (ED_SeasonalWeightedAvgDeliveries mdlSeasonalWeightedAvg in lstSeasonalWeightedAvg)
            {
                if (mdlSeasonalWeightedAvg.SeasonID == (short)Constants.Seasons.EarlyKharif)
                {
                    mdlSeasonalWeightedAvg.SelectedAvg = _EKSelectedAverage;
                    mdlSeasonalWeightedAvg.SelectedYear = _EKYear;
                }
                else if (mdlSeasonalWeightedAvg.SeasonID == (short)Constants.Seasons.LateKharif)
                {
                    mdlSeasonalWeightedAvg.SelectedAvg = _LKSelectedAverage;
                    mdlSeasonalWeightedAvg.SelectedYear = _LKYear;
                }
            }

            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// This function returns edit information data
        /// Created On 15-02-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_SeasonID"></param>
        /// <param name="_CommandID"></param>
        /// <returns>dynamic</returns>
        public dynamic GetEditInformation(int _Year, long _SeasonID, long _CommandID)
        {
            List<ED_SeasonalEntitlement> ListOfEntitlement = (from se in context.ED_SeasonalEntitlement where se.Year == _Year && se.SeasonID == _SeasonID && se.ED_CommandChannel.ChannelComndTypeID == _CommandID select se).ToList();

            dynamic mdlEditData = null;

            if (ListOfEntitlement.Count != 0)
            {
                if (ListOfEntitlement[0].EntitlementSource == "Entitlement")
                {
                    mdlEditData = (from se in ListOfEntitlement
                                   join u in context.UA_Users on new { ID = (long)se.ModifiedBy.Value } equals new { ID = u.ID }
                                   join cc in context.ED_CommandChannel on se.CommandChannelID equals cc.ID
                                   join swa in context.ED_SeasonalWeightedAvg on new { CommandChannelID = se.CommandChannelID, Year = se.Year, SeasonID = se.SeasonID } equals new { CommandChannelID = swa.CommandChannelID, Year = swa.Year, SeasonID = swa.SeasonID }
                                   where cc.ChannelComndTypeID == _CommandID
                                   orderby se.ID descending
                                   select new
                                   {
                                       SelectedAvg = (swa.SelectedAvg == "5y" ? "5 year" : swa.SelectedAvg == "sy" ? _SeasonID != (int)Constants.Seasons.Rabi ? "Year " + Convert.ToString(swa.SelectedYear) : "Year " + Convert.ToString(swa.SelectedYear + "-" + (swa.SelectedYear + (short)1)) : swa.SelectedAvg == "10y" ? "10 year" : "1977-1982"),
                                       SelectedYear = (swa.SelectedAvg == "sy" ? _SeasonID != (int)Constants.Seasons.Rabi ? "Year " + Convert.ToString(swa.SelectedYear) : "Year " + Convert.ToString(swa.SelectedYear + "-" + (swa.SelectedYear + (short)1)) : ""),
                                       Date = se.ModifiedDate,
                                       UserName = u.FirstName + " " + u.LastName,
                                       ESource = se.EntitlementSource
                                   }).FirstOrDefault();
                }
                else
                {
                    mdlEditData = (from se in ListOfEntitlement
                                   join u in context.UA_Users on new { ID = (long)se.ModifiedBy.Value } equals new { ID = u.ID }
                                   join cc in context.ED_CommandChannel on se.CommandChannelID equals cc.ID
                                   join swa in context.ED_SeasonalWeightedAvgDeliveries on new { CommandChannelID = se.CommandChannelID, Year = se.Year, SeasonID = se.SeasonID } equals new { CommandChannelID = swa.CommandChannelID, Year = swa.Year, SeasonID = swa.SeasonID }
                                   where cc.ChannelComndTypeID == _CommandID
                                   orderby se.ID descending
                                   select new
                                   {
                                       SelectedAvg = (swa.SelectedAvg == "5y" ? "5 year" : swa.SelectedAvg == "sy" ? _SeasonID != (int)Constants.Seasons.Rabi ? "Year " + Convert.ToString(swa.SelectedYear) : "Year " + Convert.ToString(swa.SelectedYear + "-" + (swa.SelectedYear + (short)1)) : swa.SelectedAvg == "10y" ? "10 year" : "1977-1982"),
                                       SelectedYear = (swa.SelectedAvg == "sy" ? _SeasonID != (int)Constants.Seasons.Rabi ? "Year " + Convert.ToString(swa.SelectedYear) : "Year " + Convert.ToString(swa.SelectedYear + "-" + (swa.SelectedYear + (short)1)) : ""),
                                       Date = se.ModifiedDate,
                                       UserName = u.FirstName + " " + u.LastName,
                                       ESource = se.EntitlementSource
                                   }).FirstOrDefault();
                }
            }

            return mdlEditData;
        }

        private double? GetDeliveriesRabi(long _ChannelID, long _Year)
        {
            //1831 is channel id of Haveli
            //652 is channel id of ujc
            //784 is channel id of RPC
            //1022 is channel id of MR Link Canal
            //1059 is channel id of MR Sub link Canal

            double? MAFDistribution = 0;
            double? MAFHeadDistribution = 0;
            double? MAFTailDistribution = 0;

            CO_ChannelGauge mdlHeadGauge = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();
            CO_ChannelGauge mdlTailGauge = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.TailGauge).FirstOrDefault();

            MAFHeadDistribution = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGauge.ID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                     && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                     || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.DischargeMAF).Sum();

            MAFTailDistribution = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlTailGauge.ID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                     && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                     || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.DischargeMAF).Sum();


            if (_ChannelID == 1831)
            {

                MAFDistribution = MAFHeadDistribution - MAFTailDistribution;
            }

            else if (_ChannelID == 652)
            {

                CO_ChannelGauge mdlHeadGaugeForRPC = context.CO_ChannelGauge.Where(x => x.ChannelID == 784 && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();

                double? MAFTailDistributionForRPC = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGaugeForRPC.ID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                     && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                     || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.DischargeMAF).Sum();

                MAFDistribution = MAFHeadDistribution - MAFTailDistribution - MAFTailDistributionForRPC;
            }
            else if (_ChannelID == 1022)
            {
                CO_ChannelGauge mdlHeadGaugeForMRSUB = context.CO_ChannelGauge.Where(x => x.ChannelID == 1059 && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();

                double? MAFTailDistributionForMRSUB = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGaugeForMRSUB.ID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                     && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                     || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.DischargeMAF).Sum();

                MAFDistribution = MAFHeadDistribution - MAFTailDistribution - MAFTailDistributionForMRSUB;
            }
            else
            {
                MAFDistribution = MAFHeadDistribution;
            }

            return MAFDistribution;
        }

        private double? GetDeliveriesKharif(long _ChannelID, long _Year)
        {
            //1831 is channel id of Haveli
            //652 is channel id of ujc
            //784 is channel id of RPC
            //1022 is channel id of MR Link Canal
            //1059 is channel id of MR Sub link Canal

            double? MAFDistribution = 0;
            double? MAFHeadDistribution = 0;
            double? MAFTailDistribution = 0;

            CO_ChannelGauge mdlHeadGauge = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();
            CO_ChannelGauge mdlTailGauge = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.TailGauge).FirstOrDefault();

            MAFHeadDistribution = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGauge.ID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == _Year).Select(x => x.DischargeMAF).Sum();

            MAFTailDistribution = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlTailGauge.ID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == _Year).Select(x => x.DischargeMAF).Sum();

            if (_ChannelID == 1831)
            {

                MAFDistribution = MAFHeadDistribution - MAFTailDistribution;
            }

            else if (_ChannelID == 652)
            {

                CO_ChannelGauge mdlHeadGaugeForRPC = context.CO_ChannelGauge.Where(x => x.ChannelID == 784 && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();

                double? MAFTailDistributionForRPC = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGaugeForRPC.ID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == _Year).Select(x => x.DischargeMAF).Sum();

                MAFDistribution = MAFHeadDistribution - MAFTailDistribution - MAFTailDistributionForRPC;
            }
            else if (_ChannelID == 1022)
            {
                CO_ChannelGauge mdlHeadGaugeForMRSUB = context.CO_ChannelGauge.Where(x => x.ChannelID == 1059 && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();

                double? MAFTailDistributionForMRSUB = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGaugeForMRSUB.ID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == _Year).Select(x => x.DischargeMAF).Sum();

                MAFDistribution = MAFHeadDistribution - MAFTailDistribution - MAFTailDistributionForMRSUB;
            }
            else
            {
                MAFDistribution = MAFHeadDistribution;
            }

            return MAFDistribution;
        }

        private List<dynamic> GetDeliveriesRabiForViewAndApprove(long _ChannelID, long _Year)
        {
            //1831 is channel id of Haveli
            //652 is channel id of UJC
            //784 is channel id of RPC
            //1022 is channel id of MR Link Canal
            //1059 is channel id of MR Sub link Canal

            List<dynamic> MAFDistribution = new List<dynamic>();
            List<ED_TDailyGaugeReading> MAFHeadDistribution = null;
            List<ED_TDailyGaugeReading> MAFTailDistribution = null;

            CO_ChannelGauge mdlHeadGauge = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();
            CO_ChannelGauge mdlTailGauge = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.TailGauge).FirstOrDefault();

            if (mdlHeadGauge != null)
            {
                MAFHeadDistribution = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGauge.ID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                         && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                         || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).ToList();
            }

            if (mdlTailGauge != null)
            {
                MAFTailDistribution = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlTailGauge.ID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                         && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                         || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).ToList();
            }

            if (_ChannelID == 1831)
            {
                MAFDistribution = (from Head in MAFHeadDistribution
                                   join Tail in MAFTailDistribution on Head.TDailyCalendarID equals Tail.TDailyCalendarID into ta
                                   from t in ta.DefaultIfEmpty()
                                   select new
                                   {
                                       TdailyCalendarID = Head.TDailyCalendarID,
                                       DischargeMAF = (Head.DischargeMAF - t.DischargeMAF),
                                       DischargeCs = (Head.DischargeCs - t.DischargeCs)
                                   }).ToList<dynamic>();
            }

            else if (_ChannelID == 652)
            {
                if (MAFHeadDistribution != null)
                {
                    CO_ChannelGauge mdlHeadGaugeForRPC = context.CO_ChannelGauge.Where(x => x.ChannelID == 784 && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();

                    List<ED_TDailyGaugeReading> MAFTailDistributionForRPC = null;

                    if (mdlHeadGaugeForRPC != null)
                    {
                        MAFTailDistributionForRPC = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGaugeForRPC.ID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                        && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                        || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).ToList();
                    }

                    MAFDistribution = (from Head in MAFHeadDistribution
                                       join Tail in MAFTailDistribution on Head.TDailyCalendarID equals Tail.TDailyCalendarID into ta
                                       from t in ta.DefaultIfEmpty()
                                       join RPC in MAFTailDistributionForRPC on Head.TDailyCalendarID equals RPC.TDailyCalendarID into rp
                                       from r in rp.DefaultIfEmpty()
                                       select new
                                       {
                                           TdailyCalendarID = Head.TDailyCalendarID,
                                           DischargeMAF = (Head.DischargeMAF - t.DischargeMAF - r.DischargeMAF),
                                           DischargeCs = (Head.DischargeCs - t.DischargeCs - r.DischargeCs)
                                       }).ToList<dynamic>();
                }
            }
            else if (_ChannelID == 1022)
            {
                if (MAFHeadDistribution != null)
                {
                    CO_ChannelGauge mdlHeadGaugeForMRSUB = context.CO_ChannelGauge.Where(x => x.ChannelID == 1059 && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();

                    List<ED_TDailyGaugeReading> MAFTailDistributionForMRSUB = null;

                    if (mdlHeadGaugeForMRSUB != null)
                    {
                        MAFTailDistributionForMRSUB = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGaugeForMRSUB.ID && x.SeasonID == (long)Constants.Seasons.Rabi
                                                            && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                                                            || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).ToList();
                    }

                    MAFDistribution = (from Head in MAFHeadDistribution
                                       join Tail in MAFTailDistribution on Head.TDailyCalendarID equals Tail.TDailyCalendarID into ta
                                       from t in ta.DefaultIfEmpty()
                                       join MRSUB in MAFTailDistributionForMRSUB on Head.TDailyCalendarID equals MRSUB.TDailyCalendarID into mrsu
                                       from mr in mrsu.DefaultIfEmpty()
                                       select new
                                       {
                                           TdailyCalendarID = Head.TDailyCalendarID,
                                           DischargeMAF = (Head.DischargeMAF - t.DischargeMAF - mr.DischargeMAF),
                                           DischargeCs = (Head.DischargeCs - t.DischargeCs - mr.DischargeCs)
                                       }).ToList<dynamic>();
                }
            }
            else
            {
                if (MAFHeadDistribution != null)
                {
                    MAFDistribution = MAFHeadDistribution.Select(x => new { TdailyCalendarID = x.TDailyCalendarID, DischargeMAF = x.DischargeMAF, DischargeCs = x.DischargeCs }).ToList<dynamic>();
                }
            }

            return MAFDistribution;
        }

        private List<dynamic> GetDeliveriesKharifForViewAndApprove(long _ChannelID, long _Year)
        {
            //1831 is channel id of Haveli
            //652 is channel id of ujc
            //784 is channel id of RPC
            //1022 is channel id of MR Link Canal
            //1059 is channel id of MR Sub link Canal

            List<dynamic> MAFDistribution = new List<dynamic>();
            List<ED_TDailyGaugeReading> MAFHeadDistribution = null;
            List<ED_TDailyGaugeReading> MAFTailDistribution = null;

            CO_ChannelGauge mdlHeadGauge = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();
            CO_ChannelGauge mdlTailGauge = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.TailGauge).FirstOrDefault();

            MAFHeadDistribution = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGauge.ID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == _Year).ToList();

            MAFTailDistribution = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlTailGauge.ID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == _Year).ToList();


            if (_ChannelID == 1831)
            {
                MAFDistribution = (from Head in MAFHeadDistribution
                                   join Tail in MAFTailDistribution on Head.TDailyCalendarID equals Tail.TDailyCalendarID into ta
                                   from t in ta.DefaultIfEmpty()
                                   select new
                                   {
                                       TdailyCalendarID = Head.TDailyCalendarID,
                                       DischargeMAF = (Head.DischargeMAF - t.DischargeMAF),
                                       DischargeCs = (Head.DischargeCs - t.DischargeCs)
                                   }).ToList<dynamic>();
                //MAFDistribution = MAFHeadDistribution - MAFTailDistribution;
            }

            else if (_ChannelID == 652)
            {

                CO_ChannelGauge mdlHeadGaugeForRPC = context.CO_ChannelGauge.Where(x => x.ChannelID == 784 && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();

                List<ED_TDailyGaugeReading> MAFTailDistributionForRPC = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGaugeForRPC.ID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == _Year).ToList();

                MAFDistribution = (from Head in MAFHeadDistribution
                                   join Tail in MAFTailDistribution on Head.TDailyCalendarID equals Tail.TDailyCalendarID into ta
                                   from t in ta.DefaultIfEmpty()
                                   join RPC in MAFTailDistributionForRPC on Head.TDailyCalendarID equals RPC.TDailyCalendarID into rp
                                   from r in rp.DefaultIfEmpty()
                                   select new
                                   {
                                       TdailyCalendarID = Head.TDailyCalendarID,
                                       DischargeMAF = (Head.DischargeMAF - t.DischargeMAF - r.DischargeMAF),
                                       DischargeCs = (Head.DischargeCs - t.DischargeCs - r.DischargeCs)
                                   }).ToList<dynamic>();

                //MAFDistribution = MAFHeadDistribution - MAFTailDistribution - MAFTailDistributionForRPC;
            }
            else if (_ChannelID == 1022)
            {
                CO_ChannelGauge mdlHeadGaugeForMRSUB = context.CO_ChannelGauge.Where(x => x.ChannelID == 1059 && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();

                List<ED_TDailyGaugeReading> MAFTailDistributionForMRSUB = context.ED_TDailyGaugeReading.Where(x => x.GaugeID == mdlHeadGaugeForMRSUB.ID && x.SeasonID == (long)Constants.Seasons.Kharif && x.Year == _Year).ToList();


                MAFDistribution = (from Head in MAFHeadDistribution
                                   join Tail in MAFTailDistribution on Head.TDailyCalendarID equals Tail.TDailyCalendarID into ta
                                   from t in ta.DefaultIfEmpty()
                                   join MRSUB in MAFTailDistributionForMRSUB on Head.TDailyCalendarID equals MRSUB.TDailyCalendarID into mrsu
                                   from mr in mrsu.DefaultIfEmpty()
                                   select new
                                   {
                                       TdailyCalendarID = Head.TDailyCalendarID,
                                       DischargeMAF = (Head.DischargeMAF - t.DischargeMAF - mr.DischargeMAF),
                                       DischargeCs = (Head.DischargeCs - t.DischargeCs - mr.DischargeCs)
                                   }).ToList<dynamic>();

                //MAFDistribution = MAFHeadDistribution - MAFTailDistribution - MAFTailDistributionForMRSUB;
            }
            else
            {
                MAFDistribution = MAFHeadDistribution.Select(x => new { TdailyCalendarID = x.TDailyCalendarID, DischargeMAF = x.DischargeMAF, DischargeCs = x.DischargeCs }).ToList<dynamic>();
            }

            return MAFDistribution;
        }

        /// <summary>
        /// This function gets Seasonal Entitlement for a particular season, year and command
        /// Created On 28-03-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_LstSeasonID"></param>
        /// <param name="_CommandID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetCommandSeasonalEntitlement(int _Year, List<long> _LstSeasonID, long _CommandID)
        {
            return (from se in context.ED_SeasonalEntitlement
                    where se.Year == _Year && _LstSeasonID.Contains(se.SeasonID) && se.ED_CommandChannel.ChannelComndTypeID == _CommandID && se.IsApproved == true
                    group se by se.CommandChannelID into sed
                    select new
                    {
                        ChannelName = sed.FirstOrDefault().ED_CommandChannel.CO_Channel.NAME,
                        ChannelID = sed.FirstOrDefault().CommandChannelID,//.ChannelID,
                        Entitlement = sed.Sum(se => se.MAFEntitlement),
                        Season = _LstSeasonID.Contains((long)Constants.Seasons.Rabi) ? (long)Constants.Seasons.Rabi : (long)Constants.Seasons.Kharif,
                        Year = _Year
                    }).OrderBy(q => q.ChannelName).ToList<dynamic>();
        }

        /// <summary>
        /// This function gets Seasonal Entitlement for a particular season, year and command
        /// Created On 28-03-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_LstSeasonID"></param>
        /// <param name="_CommandID"></param>
        /// <returns>List<dynamic></returns>
        public List<object> GetEntitlementYear(int _SeasonID, int _CurrentYear)
        {
            List<dynamic> ey = new List<dynamic>();

            if (_SeasonID == (int)Constants.Seasons.Rabi)
            {
                ey = (from se in context.ED_SeasonalEntitlement
                      where se.SeasonID == (int)Constants.Seasons.Rabi
                      select new
                      {
                          ID = se.Year,
                          Name = se.Year
                      }).Distinct().OrderByDescending(x => x.ID).ToList<dynamic>();

                ey = ey.Select(y => new
                {
                    ID = y.ID,
                    Name = y.Name + "-" + (y.Name + 1)
                }).ToList<dynamic>();
            }
            else
            {
                ey = (from se in context.ED_SeasonalEntitlement
                      where se.SeasonID != (int)Constants.Seasons.Rabi
                      select new
                      {
                          ID = se.Year,
                          Name = se.Year
                      }).Distinct().OrderByDescending(x => x.ID).ToList<dynamic>();
            }

            ey = ey.Where(x => x.ID != _CurrentYear).ToList<dynamic>();

            return ey;
        }

        public List<object> GetDeliveryYear(int _SeasonID, int _CurrentYear)
        {
            List<dynamic> ey = new List<dynamic>();

            if (_SeasonID == (int)Constants.Seasons.Rabi)
            {
                ey = (from se in context.ED_SeasonalDeliveries
                      where se.SeasonID == (int)Constants.Seasons.Rabi
                      select new
                      {
                          ID = se.Year,
                          Name = se.Year
                      }).Distinct().OrderByDescending(x => x.ID).ToList<dynamic>();

                ey = ey.Select(y => new
                {
                    ID = y.ID,
                    Name = y.Name + "-" + (y.Name + 1)
                }).ToList<dynamic>();
            }
            else
            {
                ey = (from se in context.ED_SeasonalDeliveries
                      where se.SeasonID != (int)Constants.Seasons.Rabi
                      select new
                      {
                          ID = se.Year,
                          Name = se.Year
                      }).Distinct().OrderByDescending(x => x.ID).ToList<dynamic>();
            }

            ey = ey.Where(x => x.ID != _CurrentYear).ToList<dynamic>();

            return ey;
        }

        public List<dynamic> GetSelectedYearDeliveries(long _Year, long _SeasonID, long _CommandID)
        {
            short SeasonID = Convert.ToInt16(_SeasonID);
            short Year = Convert.ToInt16(_Year);
            List<dynamic> lstAveragedData = null;

            if (SeasonID == (short)Constants.Seasons.Rabi)
            {

                lstAveragedData = (from cc in context.ED_CommandChannel
                                   join sd in context.ED_SeasonalDeliveries on cc.ID equals sd.CommandChannelID
                                   where cc.ChannelComndTypeID == _CommandID && sd.SeasonID == SeasonID && sd.Year == Year
                                   select new
                                   {
                                       CommandChannelID = cc.ID,
                                       SelectedYearMAF = sd.MAFDeliveries,
                                       SelectedYearPercentage = sd.PercentageDeliveries,
                                   }).ToList<dynamic>();
            }
            else //if (SeasonID == (short)Constants.Seasons.Kharif)
            {
                lstAveragedData = (from cc in context.ED_CommandChannel
                                   //join c in context.CO_Channel on cc.ChannelID equals c.ID
                                   join sd in context.ED_SeasonalDeliveries on cc.ID equals sd.CommandChannelID
                                   where cc.ChannelComndTypeID == _CommandID && sd.SeasonID == SeasonID && sd.Year == Year
                                   select new
                                   {
                                       CommandChannelID = cc.ID,
                                       ChannelName = cc.ChannelName,//c.NAME,
                                       SelectedYearMAF = sd.MAFDeliveries,
                                       SelectedYearPercentage = sd.PercentageDeliveries,
                                   }).ToList<dynamic>();
            }

            return lstAveragedData;

        }

        public List<object> GetAverageDataForSelectedYear(long _CommandID, int _Year, long _SeasonID)
        {
            short SeasonID = Convert.ToInt16(_SeasonID);
            short Year = Convert.ToInt16(_Year);
            List<object> lstAveragedData = (from cc in context.ED_CommandChannel
                                            join se in context.ED_SeasonalEntitlement on cc.ID equals se.CommandChannelID
                                            where cc.ChannelComndTypeID == _CommandID && se.Year == Year && se.SeasonID == _SeasonID
                                            select new
                                            {
                                                CommandChannelID = cc.ID,
                                                SelectedYearMAF = se.MAFEntitlement,
                                                SelectedYearPercentage = se.PercentageEntitlement
                                            }).ToList<object>();

            return lstAveragedData;
        }

        #region Channel ID Removed Work (CR)

        public List<dynamic> GetChannels(long _CommandID, List<long?> lstChnlID)
        {
            List<dynamic> lstAllCanals = new List<dynamic>();
            try
            {
                lstAllCanals = (from CC in context.ED_CommandChannel
                                join CCC in context.ED_CommandChannelChilds on CC.ID equals CCC.CommandChannelID
                                orderby CC.ChannelName
                                where CC.ChannelComndTypeID == _CommandID && lstChnlID.Contains(CCC.ChannelID)
                                select new
                                {
                                    ID = CC.ID,
                                    Name = CC.ChannelName
                                }).OrderBy(q => q.Name).ToList<dynamic>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstAllCanals;
        }



        #endregion



        public List<dynamic> GetChildChannelFromParentFeederByChannelID(long _ChannelID, List<long> _lstSeasonIDs, long _Year, long _CommandID)
        {
            List<dynamic> lstRabiEntitlements = new List<dynamic>();
            List<dynamic> lstKharifEntitlements = new List<dynamic>();
            if (_lstSeasonIDs.FirstOrDefault() == 1)
            {
                lstRabiEntitlements = (from ccc in context.CO_ChannelParentFeeder
                                       join ce in context.ED_ChildChannelEntitlement on ccc.ChannelID equals ce.ChannelID
                                       join tdc in context.SP_RefTDailyCalendar on ce.TDailyCalendarID equals tdc.ID
                                       where (ce.Year == _Year)
                                       && ce.SeasonID == (long)Constants.Seasons.Rabi
                                       && ccc.ParrentFeederID == _ChannelID
                                       && ((tdc.Year == _Year && tdc.TDailyID >= Constants.Oct1TDailyID && tdc.TDailyID <= Constants.Dec3TDailyID)
                                           || (tdc.Year == (_Year + 1) && tdc.TDailyID >= Constants.Jan1TDailyID && tdc.TDailyID <= Constants.Mar3TDailyID))
                                       group ce by new { ce.ChannelID, ce.Year } into Channel
                                       select new
                                       {
                                           ChannelID = Channel.FirstOrDefault().ChannelID,
                                           ChannelName = Channel.FirstOrDefault().CO_Channel.NAME,
                                           Season = Channel.FirstOrDefault().SeasonID,
                                           Year = Channel.FirstOrDefault().Year,
                                       }).OrderByDescending(x => x.Year).ToList<dynamic>().Select(a => new
                                                      {
                                                          ChannelID = a.ChannelID,
                                                          ChannelName = a.ChannelName,
                                                          Season = a.Season,
                                                          Year = a.Year,
                                                          CommandID = _CommandID,
                                                          MAFEntitlement = GetMAFbyChannelIDRabi(a.ChannelID, a.Year),
                                                      }).OrderBy(w => w.ChannelName).ToList<dynamic>();
            }
            else
            {
                lstKharifEntitlements = (from ccc in context.CO_ChannelParentFeeder
                                         join ce in context.ED_ChildChannelEntitlement on ccc.ChannelID equals ce.ChannelID
                                         where (ce.Year == _Year || _Year == -1)
                                         && (ce.SeasonID == (long)Constants.Seasons.EarlyKharif || ce.SeasonID == (long)Constants.Seasons.LateKharif)
                                         && ccc.ParrentFeederID == _ChannelID
                                         group ce by new { ce.ChannelID, ce.Year } into Channel
                                         select new
                                         {
                                             ChannelID = Channel.FirstOrDefault().ChannelID,
                                             ChannelName = Channel.FirstOrDefault().CO_Channel.NAME,
                                             Season = (long)Constants.Seasons.Kharif,
                                             Year = Channel.FirstOrDefault().Year,
                                             MAFEntitlement = Channel.Sum(x => (double?)x.MAFEntitlement),
                                         }).OrderByDescending(x => x.Year).ToList<dynamic>().ToList<dynamic>().Select(a => new
                                                        {
                                                            ChannelID = a.ChannelID,
                                                            ChannelName = a.ChannelName,
                                                            Season = a.Season,
                                                            Year = a.Year,
                                                            CommandID = _CommandID,
                                                            MAFEntitlement = a.MAFEntitlement
                                                        }).OrderBy(w => w.ChannelName).ToList<dynamic>();
            }

            if (_lstSeasonIDs.Contains((long)Constants.Seasons.Rabi))
            {
                return lstRabiEntitlements;
            }
            else if (_lstSeasonIDs.Contains((long)Constants.Seasons.EarlyKharif) && _lstSeasonIDs.Contains((long)Constants.Seasons.LateKharif))
            {
                return lstKharifEntitlements;
            }
            else
            {
                return lstRabiEntitlements.Union(lstKharifEntitlements).OrderByDescending(x => x.ChannelName).ToList<dynamic>();
            }
        }
    }
}
