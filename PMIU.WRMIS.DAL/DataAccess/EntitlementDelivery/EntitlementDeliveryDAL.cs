using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using PMIU.WRMIS.DAL.Repositories.EntitlementDelivery;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.EntitlementDelivery
{
    public class EntitlementDeliveryDAL : BaseDAL
    {
        public List<object> GetYears(long _CommandID)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetYears(_CommandID);
        }

        public List<object> GetMainCanals(long _CommandID)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetMainCanals(_CommandID);
        }

        public List<dynamic> GetCannalSystemByCommandTypeID(long _UserID, bool _UserBaseLoading, long? _IrrigationLevel, long _CommandTypeID)
        {
            List<object> lstCanals = new List<object>();
            List<dynamic> lstAllCanals = new List<dynamic>();
            List<long> lstChillChnlID = new List<long>();

            if (_UserBaseLoading)
            {
                List<long?> lstSections = new WaterLossesDAL().GetSectionListByUser(_UserID, _IrrigationLevel);

                List<long?> lstChnlID = db.Repository<CO_ChannelIrrigationBoundaries>().Query().Get()
                    .Where(x => lstSections.Contains(x.SectionID)).Select(x => x.ChannelID).ToList<long?>();

                //lstChillChnlID = db.Repository<ED_CommandChannel>().Query().Get()
                //    .Where(x => x.ChannelComndTypeID == _CommandTypeID && lstChnlID.Contains(x.ChannelID)).Select(x => x.ChannelID).ToList<long>();

                lstAllCanals = db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetChannels(_CommandTypeID, lstChnlID);
            }
            else
            {
                //lstChillChnlID = db.Repository<ED_CommandChannel>().Query().Get()
                //    .Where(x => x.ChannelComndTypeID == _CommandTypeID).Select(x => x.ChannelID).ToList<long>();

                lstAllCanals = db.Repository<ED_CommandChannel>().Query().Get()
                    .Where(x => x.ChannelComndTypeID == _CommandTypeID).Select(x => new { ID = x.ID, Name = x.ChannelName }).OrderBy(x => x.Name).ToList<dynamic>();
            }

            //lstAllCanals = db.Repository<CO_Channel>().Query().Get().Where(x => lstChillChnlID.Contains(x.ID))
            //        .Select(x => new { x.ID, x.NAME, x.ChannelTypeID }).ToList().Select(x => new { ID = x.ID, Name = x.NAME, Type = x.ChannelTypeID }).OrderBy(x => x.Name).ToList<dynamic>();
            return lstAllCanals;
        }

        public List<dynamic> GetDistinctYearsBySeason(long _SeasonID)
        {
            List<dynamic> lstYearsString = null;

            if (_SeasonID == (long)Constants.Seasons.Rabi)
            {
                lstYearsString = db.Repository<ED_ChannelEntitlement>().GetAll().Where(ce => ce.SeasonID == (long)Constants.Seasons.Rabi).Select(x => x.Year).Distinct().ToList().Select(x => new { ID = x, Name = x + "-" + (x + 1) }).OrderByDescending(x => x.ID).ToList<dynamic>();
            }
            else
            {
                lstYearsString = db.Repository<ED_ChannelEntitlement>().GetAll().Where(ce => ce.SeasonID != (long)Constants.Seasons.Rabi).Select(x => x.Year).Distinct().ToList().Select(x => new { ID = x, Name = x.ToString() }).OrderByDescending(x => x.ID).ToList<dynamic>();
            }

            return lstYearsString;
        }

        /// <summary>
        /// This function returns distinct latest approved years.
        /// Created On 06-07-2016
        /// </summary>
        /// <param name="_SeasonID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDistinctApprovedYearsBySeason(long _SeasonID)
        {
            List<dynamic> lstYearsString = null;

            if (_SeasonID == (long)Constants.Seasons.Rabi)
            {
                lstYearsString = db.Repository<ED_SeasonalEntitlement>().GetAll().Where(se => se.SeasonID == (long)Constants.Seasons.Rabi && se.IsApproved == true).Select(x => x.Year).Distinct().ToList().Select(x => new { ID = x, Name = x + "-" + (x + 1) }).OrderByDescending(x => x.ID).Take(10).ToList<dynamic>();
            }
            else
            {
                lstYearsString = db.Repository<ED_SeasonalEntitlement>().GetAll().Where(se => se.SeasonID != (long)Constants.Seasons.Rabi && se.IsApproved == true).Select(x => x.Year).Distinct().ToList().Select(x => new { ID = x, Name = x.ToString() }).OrderByDescending(x => x.ID).Take(10).ToList<dynamic>();
            }

            return lstYearsString;
        }

        /// <summary>
        /// This function gets water distribution in particular year for Rabi on Indus river.
        /// Created On 24-01-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_Scenario"></param>
        /// <returns>IEnumerable<DataRow></returns>
        public IEnumerable<DataRow> GetIndusRabiShare(int _Year, int _Scenario)
        {
            return db.ExecuteDataSet("RPT_SP_DistributionRabi", _Year, _Scenario);
        }

        /// <summary>
        /// This function gets water distribution in particular year for Kharif on Indus river.
        /// Created On 25-01-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_Scenario"></param>
        /// <returns>IEnumerable<DataRow></returns>
        public IEnumerable<DataRow> GetIndusKharifShare(int _Year, int _Scenario)
        {
            return db.ExecuteDataSet("RPT_SP_DistributionKharif", _Year, _Scenario);
        }

        public List<dynamic> GetChannelEntitlementBySearchCriteria(long _ChannelID, List<long> _lstSeasonIDs, long _Year)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetChannelEntitlementBySearchCriteria(_ChannelID, _lstSeasonIDs, _Year);
        }
        public List<dynamic> GetChildChannelEntitlementBySearchCriteria(long _ChannelID, List<long> _lstSeasonIDs, long _Year)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetChildChannelEntitlementBySearchCriteria(_ChannelID, _lstSeasonIDs, _Year);
        }
        public List<dynamic> GetChildChannelByMainChannel(long _ChannelID, List<long> _lstSeasonIDs, long _Year, long _CommandID)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetChildChannelByMainChannel(_ChannelID, _lstSeasonIDs, _Year, _CommandID);
        }
        
        public List<dynamic> ViewEntitlementsBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().ViewEntitlementsBySearchCriteria(_ChannelID, _SeasonID, _Year);
        }

        /// <summary>
        /// This function returns the list of 7782 Cusecs for a particular Season and Command
        /// Created On 30-01-2017
        /// </summary>
        /// <param name="_SeasonID"></param>
        /// <param name="_CommandID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> Get7782AverageCommandSeasonalCusecs(long _SeasonID, long _CommandID)
        {
            //List<dynamic> lstCusecs = null;

            //if (_CommandID == (long)Constants.Commands.IndusCommand)
            //{
            //    if (_SeasonID == (long)Constants.Seasons.Rabi)
            //    {
            //        //lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == _SeasonID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.PunjabHistoric.Value }).ToList<dynamic>();
            //        lstCusecs = db.Repository<ED_ChannelFlow7782>().GetAll().Where(rsd => rsd.SeasonID == _SeasonID && rsd.ED_CommandChannel.ChannelComndTypeID == _CommandID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID, Cusecs = rsd.DischargeCs }).ToList<dynamic>();
            //    }
            //    else if (_SeasonID == (long)Constants.Seasons.EarlyKharif)
            //    {
            //        //lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.Kharif && (rsd.TDailyID >= Constants.Apr1TDailyID && rsd.TDailyID <= Constants.Jun1TDailyID)).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.PunjabHistoric.Value }).ToList<dynamic>();
            //        lstCusecs = db.Repository<ED_ChannelFlow7782>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.EarlyKharif && rsd.ED_CommandChannel.ChannelComndTypeID == _CommandID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID, Cusecs = rsd.DischargeCs }).ToList<dynamic>();
            //    }
            //    else
            //    {
            //        //lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.Kharif && (rsd.TDailyID >= Constants.Jun2TDailyID && rsd.TDailyID <= Constants.Sep3TDailyID)).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.PunjabHistoric.Value }).ToList<dynamic>();
            //        lstCusecs = db.Repository<ED_ChannelFlow7782>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.LateKharif && rsd.ED_CommandChannel.ChannelComndTypeID == _CommandID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID, Cusecs = rsd.DischargeCs }).ToList<dynamic>();
            //    }
            //}
            //else
            //{
            //    if (_SeasonID == (long)Constants.Seasons.Rabi)
            //    {
            //        //lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == _SeasonID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.Jc7782.Value }).ToList<dynamic>();
            //        lstCusecs = db.Repository<ED_ChannelFlow7782>().GetAll().Where(rsd => rsd.SeasonID == _SeasonID && rsd.ED_CommandChannel.ChannelComndTypeID == _CommandID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID, Cusecs = rsd.DischargeCs }).ToList<dynamic>();
            //    }
            //    else if (_SeasonID == (long)Constants.Seasons.EarlyKharif)
            //    {
            //        //lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.Kharif && (rsd.TDailyID >= Constants.Apr1TDailyID && rsd.TDailyID <= Constants.Jun1TDailyID)).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.Jc7782.Value }).ToList<dynamic>();
            //        lstCusecs = db.Repository<ED_ChannelFlow7782>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.EarlyKharif && rsd.ED_CommandChannel.ChannelComndTypeID == _CommandID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID, Cusecs = rsd.DischargeCs }).ToList<dynamic>();
            //    }
            //    else
            //    {
            //        //lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.Kharif && (rsd.TDailyID >= Constants.Jun2TDailyID && rsd.TDailyID <= Constants.Sep3TDailyID)).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.Jc7782.Value }).ToList<dynamic>();
            //        lstCusecs = db.Repository<ED_ChannelFlow7782>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.LateKharif && rsd.ED_CommandChannel.ChannelComndTypeID == _CommandID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID, Cusecs = rsd.DischargeCs }).ToList<dynamic>();
            //    }
            //}

            List<dynamic> lstCusecs = db.Repository<ED_ChannelFlow7782>().GetAll().Where(rsd => rsd.SeasonID == _SeasonID && rsd.ED_CommandChannel.ChannelComndTypeID == _CommandID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = rsd.SeasonID, Cusecs = rsd.DischargeCs }).ToList<dynamic>();
            lstCusecs = lstCusecs.Select(lc => new { TDailyID = lc.TDailyID, ShortName = Utility.GetTDailyShortName(lc.TDailyID, (_SeasonID != (long)Constants.Seasons.Rabi ? (int)Constants.Seasons.Kharif : (int)Constants.Seasons.Rabi)), Cusecs = lc.Cusecs / 1000 }).ToList<dynamic>();

            return lstCusecs;
        }

        /// <summary>
        /// This function returns the list of Para2 Cusecs for a particular Season and Command
        /// Created On 24-02-2017
        /// </summary>
        /// <param name="_SeasonID"></param>
        /// <param name="_CommandID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetPara2AverageCommandSeasonalMAF(long _SeasonID, long _CommandID)
        {
            List<dynamic> lstCusecs = null;

            if (_CommandID == (long)Constants.Commands.IndusCommand)
            {
                if (_SeasonID == (long)Constants.Seasons.Rabi)
                {
                    lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == _SeasonID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.PunjabHistoric.Value }).ToList<dynamic>();
                }
                else if (_SeasonID == (long)Constants.Seasons.EarlyKharif)
                {
                    lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.Kharif && (rsd.TDailyID >= Constants.Apr1TDailyID && rsd.TDailyID <= Constants.Jun1TDailyID)).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.PunjabPara2.Value }).ToList<dynamic>();
                }
                else
                {
                    lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.Kharif && (rsd.TDailyID >= Constants.Jun2TDailyID && rsd.TDailyID <= Constants.Sep3TDailyID)).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.PunjabPara2.Value }).ToList<dynamic>();
                }
            }
            else
            {
                if (_SeasonID == (long)Constants.Seasons.Rabi)
                {
                    lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == _SeasonID).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.Jc7782.Value }).ToList<dynamic>();
                }
                else if (_SeasonID == (long)Constants.Seasons.EarlyKharif)
                {
                    lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.Kharif && (rsd.TDailyID >= Constants.Apr1TDailyID && rsd.TDailyID <= Constants.Jun1TDailyID)).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.Jc7782.Value }).ToList<dynamic>();
                }
                else
                {
                    lstCusecs = db.Repository<SP_RefShareDistribution>().GetAll().Where(rsd => rsd.SeasonID == (long)Constants.Seasons.Kharif && (rsd.TDailyID >= Constants.Jun2TDailyID && rsd.TDailyID <= Constants.Sep3TDailyID)).Select(rsd => new { TDailyID = rsd.TDailyID, SeasonID = (int)rsd.SeasonID.Value, Cusecs = rsd.Jc7782.Value }).ToList<dynamic>();
                }
            }

            lstCusecs = lstCusecs.Select(lc => new { TDailyID = lc.TDailyID, ShortName = Utility.GetTDailyShortName(lc.TDailyID, lc.SeasonID), Cusecs = lc.Cusecs }).ToList<dynamic>();

            return lstCusecs;
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
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().SaveRetrieveRabiAverageData(_CommandID, _Year, _UserID, _LstProvincialEntitlements);
        }


        public List<dynamic> SaveRetrieveRabiAverageDataDeliveries(long _CommandID, int _Year, long _UserID, List<double> _LstProvincialEntitlements)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().SaveRetrieveRabiAverageDataDeliveries(_CommandID, _Year, _UserID, _LstProvincialEntitlements);
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
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetAverageData(_CommandID, _Year, _SeasonID, _LstProvincialEntitlements);
        }
        /// <summary>
        /// This function gets average data record for Command, Season and Year
        /// Created On 26-10-2017
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_SeasonID"></param>
        /// <param name="_LstProvincialEntitlements"></param>
        /// <returns>List<dynamic></returns>
        public List<object> GetAverageDataForSelectedYear(long _CommandID, int _Year, long _SeasonID)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetAverageDataForSelectedYear(_CommandID, _Year, _SeasonID);
        }

        public List<dynamic> GetAverageDataDeliveries(long _CommandID, int _Year, long _SeasonID, List<double> _LstProvincialEntitlements)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetAverageDataDeliveries(_CommandID, _Year, _SeasonID, _LstProvincialEntitlements);
        }

        public double GetMAFEntitlementBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            List<double> lstEntitlement = null;

            if (_SeasonID == (long)Constants.Seasons.Rabi)
            {
                //lstEntitlement = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.ED_CommandChannel.ChannelID == _ChannelID && x.SeasonID == _SeasonID
                //&& ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                //|| (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.MAFEntitlement).ToList();

                lstEntitlement = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.ED_CommandChannel.ID == _ChannelID && x.SeasonID == _SeasonID
                && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                || (x.Year == _Year + 1 && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.MAFEntitlement).ToList();
            }
            else
            {
                //lstEntitlement = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.ED_CommandChannel.ChannelID == _ChannelID && (x.SeasonID == (long)Constants.Seasons.EarlyKharif || x.SeasonID == (long)Constants.Seasons.LateKharif) && x.Year == _Year).Select(x => x.MAFEntitlement).ToList();
                lstEntitlement = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.ED_CommandChannel.ID == _ChannelID && (x.SeasonID == (long)Constants.Seasons.EarlyKharif || x.SeasonID == (long)Constants.Seasons.LateKharif) && x.Year == _Year).Select(x => x.MAFEntitlement).ToList();
            }

            return lstEntitlement.Sum();
        }

        /// <summary>
        /// This function returns the 7782 MAF for a particular Season and Channel
        /// Created On 30-01-2017
        /// </summary>
        /// <param name="_LstSeasonIDs"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>double</returns>
        public double Get7782ChannelAverageMAF(List<long> _LstSeasonIDs, long _ChannelID)
        {
            //return db.Repository<ED_ChannelFlow7782>().GetAll().Where(cf => _LstSeasonIDs.Contains(cf.SeasonID) && cf.ED_CommandChannel.ChannelID == _ChannelID).Sum(cf => cf.DischargeMAF);
            return db.Repository<ED_ChannelFlow7782>().GetAll().Where(cf => _LstSeasonIDs.Contains(cf.SeasonID) && cf.ED_CommandChannel.ID == _ChannelID).Sum(cf => cf.DischargeMAF);
        }

        /// <summary>
        /// This function returns the Para2 MAF for a particular Season and Channel
        /// Created On 24-02-2017
        /// </summary>
        /// <param name="_LstSeasonIDs"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>double</returns>
        public double GetPara2ChannelAverageMAF(List<long> _LstSeasonIDs, long _ChannelID)
        {
            return db.Repository<ED_ChannelFlowPara2>().GetAll().Where(cf => _LstSeasonIDs.Contains(cf.SeasonID) && cf.ED_CommandChannel.ID == _ChannelID).Sum(cf => cf.DischargeMAF);
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
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().SaveRetrieveKharifAverageData(_CommandID, _Year, _UserID, _LstProvincialEntitlements);
        }

        public List<dynamic> SaveRetrieveKharifAverageDataNew(long _CommandID, int _Year, long _UserID, List<double> _LstProvincialEntitlements)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().SaveRetrieveKharifAverageDataDeliveries(_CommandID, _Year, _UserID, _LstProvincialEntitlements);
        }


        public List<dynamic> ViewChildEntitlementsBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().ViewChildEntitlementsBySearchCriteria(_ChannelID, _SeasonID, _Year);
        }

        public double GetMAFChildEntitlementBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            //double MAFEntitlementSum = db.Repository<ED_ChildChannelEntitlement>().GetAll().Where(x => x.ChannelID == _ChannelID && x.SeasonID == _SeasonID && x.Year == _Year).Select(x => x.MAFEntitlement).Sum();
            //return MAFEntitlementSum;
            List<double> lst = new List<double>();

            if (_SeasonID == (long)Constants.Seasons.Rabi)
            {
                lst = db.Repository<ED_ChildChannelEntitlement>().GetAll().Where(x => x.ChannelID == _ChannelID && x.SeasonID == _SeasonID
                && ((x.SP_RefTDailyCalendar.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                || (x.SP_RefTDailyCalendar.Year == (_Year + 1) && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Select(x => x.MAFEntitlement).ToList<double>();
            }
            else
            {
                lst = db.Repository<ED_ChildChannelEntitlement>().GetAll().Where(x => x.ChannelID == _ChannelID && (x.SeasonID == (long)Constants.Seasons.EarlyKharif || x.SeasonID == (long)Constants.Seasons.LateKharif) && x.Year == _Year).Select(x => x.MAFEntitlement).ToList<double>();
            }


            if (lst.Count != 0)
            {
                return lst.Sum();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function check if there are any Entitlements approved for particular year and season.
        /// Created on 31-08-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_LstSeasonIDs"></param>
        /// <returns></returns>
        public bool IsEntitlementApprove(long _Year, List<long> _LstSeasonIDs)
        {
            return db.Repository<ED_SeasonalEntitlement>().GetAll().Where(x => x.Year == _Year && _LstSeasonIDs.Contains(x.SeasonID)).Select(x => x.IsApproved).FirstOrDefault();
        }

        public List<ED_SeasonalEntitlement> GetAllCommandChannelsByYearAndSeason(long _Year, long _SeasonID)
        {
            List<ED_SeasonalEntitlement> lstSeasonalEntitlement = new List<ED_SeasonalEntitlement>();
            if (_SeasonID == (long)Constants.Seasons.Rabi)
            {
                lstSeasonalEntitlement = db.Repository<ED_SeasonalEntitlement>().GetAll().Where(x => x.Year == _Year && x.SeasonID == _SeasonID).ToList();
            }
            else
            {
                lstSeasonalEntitlement = db.Repository<ED_SeasonalEntitlement>().GetAll().Where(x => x.Year == _Year && (x.SeasonID == (long)Constants.Seasons.EarlyKharif || x.SeasonID == (long)Constants.Seasons.LateKharif)).ToList();
            }

            return lstSeasonalEntitlement;
        }

        /// <summary>
        /// This function updates Seasonal Entitlement
        /// Created On 08-02-2017
        /// </summary>
        /// <param name="_MdlSeasonalEntitlement"></param>
        /// <returns>bool</returns>
        public bool UpdateSeasonalEntitlement(ED_SeasonalEntitlement _MdlSeasonalEntitlement)
        {
            db.Repository<ED_SeasonalEntitlement>().Update(_MdlSeasonalEntitlement);
            db.Save();

            return true;
        }

        public double GetShortage(double _total, double _value7782)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetShortage(_total, _value7782);
        }


        public DataTable GetTenDailyBySeasonYearCommand(long _Year, long _SeasonID, long _CommandID)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetTenDailyBySeasonYearCommand(_Year, _SeasonID, _CommandID);
        }



        /// <summary>
        /// This function fetches the plan scenario based on the parameters
        /// Created On 08-02-2017
        /// </summary>
        /// <param name="_SeasonID"></param>
        /// <param name="_Year"></param>
        /// <param name="_Scenario"></param>
        /// <returns>SP_PlanScenario</returns>
        public SP_PlanScenario GetPlanScenario(long _SeasonID, int _Year, string _Scenario)
        {
            return db.Repository<SP_PlanScenario>().GetAll().Where(ps => ps.Scenario.Trim().ToUpper() == _Scenario.Trim().ToUpper() &&
                        ps.SP_PlanDraft.SeasonID == _SeasonID && ps.SP_PlanDraft.Year == _Year && ps.SP_PlanDraft.IsApproved == true).FirstOrDefault();
        }

        /// <summary>
        /// This function adds new Provincial Entitlement.
        /// Created By 08-02-2017
        /// </summary>
        /// <param name="_ProvincialEntitlement"></param>
        /// <returns>bool</returns>
        public bool AddProvincialEntitlement(ED_ProvincialEntitlement _ProvincialEntitlement)
        {
            db.Repository<ED_ProvincialEntitlement>().Insert(_ProvincialEntitlement);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates Provincial Entitlement.
        /// Created By 15-02-2017
        /// </summary>
        /// <param name="_ProvincialEntitlement"></param>
        /// <returns>bool</returns>
        public bool UpdateProvincialEntitlement(ED_ProvincialEntitlement _ProvincialEntitlement)
        {
            ED_ProvincialEntitlement mdlProvincialEntitlement = db.Repository<ED_ProvincialEntitlement>().GetAll().Where(pe => pe.ID == _ProvincialEntitlement.ID).FirstOrDefault();

            mdlProvincialEntitlement.PlanDraftID = _ProvincialEntitlement.PlanDraftID;
            mdlProvincialEntitlement.PlanScenarioID = _ProvincialEntitlement.PlanScenarioID;

            if (mdlProvincialEntitlement.RabiMAF != null)
            {
                mdlProvincialEntitlement.RabiMAF = _ProvincialEntitlement.RabiMAF;
            }
            else
            {
                mdlProvincialEntitlement.EKMAF = _ProvincialEntitlement.EKMAF;
                mdlProvincialEntitlement.LKMAF = _ProvincialEntitlement.LKMAF;
            }

            mdlProvincialEntitlement.ModifiedBy = _ProvincialEntitlement.ModifiedBy;
            mdlProvincialEntitlement.ModifiedDate = _ProvincialEntitlement.ModifiedDate;

            db.Save();

            return true;
        }

        /// <summary>
        /// This function saves Seasonal Entitlement
        /// Created On 08-02-2017
        /// </summary>
        /// <param name="_LstSeasonalEntitlement"></param>
        /// <returns>bool</returns>
        public bool AddSeasonalEntitlements(List<ED_SeasonalEntitlement> _LstSeasonalEntitlement)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().AddSeasonalEntitlements(_LstSeasonalEntitlement);
        }

        /// <summary>
        /// This function gets the Provincial Entitlement based on the parameters
        /// Created On 09-02-2017
        /// </summary>
        /// <param name="_CommandID"></param>
        /// <param name="_Year"></param>
        /// <param name="_Season"></param>
        /// <param name="_ProvinceID"></param>
        /// <returns>ED_ProvincialEntitlement</returns>
        public ED_ProvincialEntitlement GetProvincialEntitlement(long _CommandID, int _Year, long _Season, long _ProvinceID)
        {
            return db.Repository<ED_ProvincialEntitlement>().GetAll().Where(pe => pe.ChannelComndTypeID == _CommandID && pe.Year == _Year &&
                        pe.SeasonID == _Season && pe.ProvinceID == _ProvinceID).FirstOrDefault();
        }

        /// <summary>
        /// This function sets Selected Average value in ED_SeasonalWeightedAvg table
        /// Created On 09-02-2017
        /// </summary>
        /// <param name="_LstSeasonalAverageIDs"></param>
        /// <param name="_SelectedAverage"></param>
        /// <returns>bool</returns>
        public bool UpdateRabiAverageSelected(List<long> _LstSeasonalAverageIDs, string _SelectedAverage, short _SelectedYear)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().UpdateRabiAverageSelected(_LstSeasonalAverageIDs, _SelectedAverage, _SelectedYear);
        }

        /// <summary>
        /// This function sets Selected Average value in ED_SeasonalWeightedAvgDeliveries table
        /// Created On 27-10-2017
        /// </summary>
        /// <param name="_LstSeasonalAverageIDs"></param>
        /// <param name="_SelectedAverage"></param>
        /// <returns>bool</returns>
        public bool UpdateDeliveriesRabiAverageSelected(List<long> _LstSeasonalAverageIDs, string _SelectedAverage, short _SelectedYear)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().UpdateDeliveriesRabiAverageSelected(_LstSeasonalAverageIDs, _SelectedAverage, _SelectedYear);
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
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().UpdateKharifAverageSelected(_LstSeasonalAverageIDs, _EKSelectedAverage, _LKSelectedAverage, _EKSelectedYear, _LKSelectedYear);
        }


        public bool UpdateKharifAverageSelectedDeliveries(List<long> _LstSeasonalAverageIDs, string _EKSelectedAverage, string _LKSelectedAverage, short _EKYear, short _LKYear)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().UpdateKharifAverageSelectedDeliveries(_LstSeasonalAverageIDs, _EKSelectedAverage, _LKSelectedAverage, _EKYear, _LKYear);
        }

        public double GetMAFByCusecs(long _TenDailyID, double _CusecVal, char _ParentChild)
        {
            int TenDaily = Convert.ToInt32(db.Repository<SP_RefTDailyCalendar>().GetAll().Where(x => x.ID == _TenDailyID).Select(c => c.NoOfDays).First());
            double MAF = 0;
            if (_ParentChild == 'P')
            {

                MAF = _CusecVal * 0.000001983471 * TenDaily;
            }
            else
            {

                MAF = _CusecVal * 0.001983471 * TenDaily;
            }

            //MAF = Math.Round(MAF, 3);
            return MAF;
        }

        /// <summary>
        /// This function calls the generate entitlement stored procedure.
        /// Created On 13-02-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_SeasonID"></param>
        /// <param name="_CommandID"></param>
        /// <param name="_UserID"></param>
        /// <returns>int</returns>
        public int GenerateEntitlements(short _Year, short _SeasonID, long _CommandID, long _UserID)
        {
            return db.ExecuteNonQuery("ED_SaveChannelEntitlement", _Year, _SeasonID, _CommandID, _UserID);
        }

        public double GetMAFFromSeasonalEntitlement(long _Year, List<long> _lstSeasonID, long _CommandChannelD)
        {
            double MAF = db.Repository<ED_SeasonalEntitlement>().GetAll().Where(x => x.CommandChannelID == _CommandChannelD && _lstSeasonID.Contains(x.SeasonID) && x.Year == _Year && x.IsApproved == false).Sum(x => x.MAFEntitlement);
            return MAF;
        }

        public ED_ChannelEntitlement GetChannelEntitlementByID(long _ID)
        {
            ED_ChannelEntitlement mdlChannelEntitlement = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.ID == _ID).FirstOrDefault();
            return mdlChannelEntitlement;
        }

        public bool UpdateChannelEntitlement(ED_ChannelEntitlement _ChannelEntitlement)
        {
            ED_ChannelEntitlement mdlChannelEntitlement = db.Repository<ED_ChannelEntitlement>().FindById(_ChannelEntitlement.ID);

            mdlChannelEntitlement.CsEntitlement = _ChannelEntitlement.CsEntitlement;
            mdlChannelEntitlement.MAFEntitlement = _ChannelEntitlement.MAFEntitlement;
            //mdlChannelEntitlement.PercentageEntitlement = _ChannelEntitlement.PercentageEntitlement;
            mdlChannelEntitlement.ModifiedBy = _ChannelEntitlement.ModifiedBy;
            mdlChannelEntitlement.ModifiedDate = _ChannelEntitlement.ModifiedDate;
            db.Repository<ED_ChannelEntitlement>().Update(mdlChannelEntitlement);
            db.Save();

            return true;
        }

        public double GetSumOfRabiTenDailies(long _Year, long _SeasonID, long _CommandChannelID)
        {
            double Percentage = 0;
            Percentage = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.SeasonID == _SeasonID && x.CommandChannelID == _CommandChannelID
                && ((x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Oct1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Dec3TDailyID)
                || (x.Year <= (_Year + 1) && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jan1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Mar3TDailyID))).Sum(x => x.CsEntitlement);

            return Percentage;
        }

        public double GetSumOfEKharifTenDailies(long _Year, long _SeasonID, long _CommandChannelID)
        {
            double Percentage = 0;
            Percentage = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.SeasonID == _SeasonID && x.CommandChannelID == _CommandChannelID
                && (x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Apr1TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Jun1TDailyID)).Sum(x => x.CsEntitlement);
            return Percentage;
        }

        public double GetSumOfLKharifTenDailies(long _Year, long _SeasonID, long _CommandChannelID)
        {
            double Percentage = 0;
            Percentage = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.SeasonID == _SeasonID && x.CommandChannelID == _CommandChannelID
                && (x.Year == _Year && x.SP_RefTDailyCalendar.TDailyID >= Constants.Jun2TDailyID && x.SP_RefTDailyCalendar.TDailyID <= Constants.Sep3TDailyID)).Sum(x => x.CsEntitlement);
            return Percentage;
        }

        public List<ED_ChannelEntitlement> ChannelEntitlementForPercentageUpdate(long _CommandID, long _SeasonID, long _Year)
        {
            List<ED_ChannelEntitlement> lst = db.Repository<ED_ChannelEntitlement>().GetAll().Where(x => x.CommandChannelID == _CommandID && x.SeasonID == _SeasonID && x.Year == _Year).ToList();
            return lst;
        }

        public bool UpdateChannelEntitlementPercentage(ED_ChannelEntitlement _ChannelEntitlement)
        {
            ED_ChannelEntitlement mdlChannelEntitlement = db.Repository<ED_ChannelEntitlement>().FindById(_ChannelEntitlement.ID);
            mdlChannelEntitlement.PercentageEntitlement = _ChannelEntitlement.PercentageEntitlement;
            db.Repository<ED_ChannelEntitlement>().Update(mdlChannelEntitlement);
            db.Save();

            return true;
        }

        public int UpdatePercentageAndGenerateEntitlements(long _CommandID, short _Year, short _SeasonID, long _UserID)
        {
            return db.ExecuteNonQuery("ED_UpdateChannelPercentage", _CommandID, _Year, _SeasonID, _UserID);
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
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetEditInformation(_Year, _SeasonID, _CommandID);
        }

        /// <summary>
        /// This function fetches the Seasonal Entitlement for a particular Channel
        /// Created On 15-08-2017
        /// </summary>
        /// <param name="_CommandChannelID"></param>
        /// <param name="_Year"></param>
        /// <param name="_SeasonID"></param>
        /// <returns>ED_SeasonalEntitlement</returns>
        public ED_SeasonalEntitlement GetCommandSeasonalEntitlement(long _CommandChannelID, int _Year, long _SeasonID)
        {
            //return db.Repository<ED_SeasonalEntitlement>().GetAll().Where(se => se.CommandChannelID == _CommandChannelID && se.Year == _Year &&
            //                                                                se.SeasonID == _SeasonID && se.IsApproved == false).FirstOrDefault();
            return db.Repository<ED_SeasonalEntitlement>().GetAll().Where(se => se.CommandChannelID == _CommandChannelID && se.Year == _Year &&
                                                                            se.SeasonID == _SeasonID).FirstOrDefault();
        }

        /// <summary>
        /// This function gets water distribution in particular year for Rabi on Jhelum Chenab river.
        /// Created On 15-02-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_Scenario"></param>
        /// <returns>IEnumerable<DataRow></returns>
        public IEnumerable<DataRow> GetRabiShare(int _Year, int _Scenario)
        {
            return db.ExecuteDataSet("RPT_SP_ProvincialSharesRabi", _Year, _Scenario, (int)Constants.Seasons.Rabi);
        }

        /// <summary>
        /// This function gets water distribution in particular year for Kharif on Jhelum Chenab river.
        /// Created On 15-02-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_Scenario"></param>
        /// <returns>IEnumerable<DataRow></returns>
        public IEnumerable<DataRow> GetKharifShare(int _Year, int _Scenario)
        {
            return db.ExecuteDataSet("RPT_SP_ProvincialSharesKharif", _Year, _Scenario);
        }

        public CO_ChannelGauge GetChannelGaugeByChannelID(long _ChannelID)
        {
            CO_ChannelGauge mdlGauge = db.Repository<CO_ChannelGauge>().GetAll().Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).FirstOrDefault();
            return mdlGauge;
        }

        public ED_CommandChannel GetCommandChannelByID(long _ChannelID)
        {
            ED_CommandChannel mdlCommandChannel = db.Repository<ED_CommandChannel>().GetAll().Where(x => x.ID == _ChannelID).FirstOrDefault();//Where(x => x.ChannelID == _ChannelID).FirstOrDefault();
            return mdlCommandChannel;
        }

        /// <summary>
        /// This function returns the 7782 MAF for Season
        /// Created On 27-03-2017
        /// </summary>
        /// <param name="_LstSeasonID"></param>        
        /// <returns>double</returns>
        public double Get7782AverageSeasonalMAF(List<long> _LstSeasonID)
        {
            return db.Repository<ED_ChannelFlow7782>().GetAll().Where(cf => _LstSeasonID.Contains(cf.SeasonID)).Sum(cf => cf.DischargeMAF);
        }

        /// <summary>
        /// This function returns the Para2 MAF for Season
        /// Created On 27-03-2017
        /// </summary>
        /// <param name="_LstSeasonID"></param>
        /// <returns>double</returns>
        public double GetPara2AverageSeasonalMAF(List<long> _LstSeasonID)
        {
            return db.Repository<ED_ChannelFlowPara2>().GetAll().Where(cf => _LstSeasonID.Contains(cf.SeasonID)).Sum(cf => cf.DischargeMAF);
        }

        /// <summary>
        /// This function fetches the Seasonal Entitlement
        /// Created On 27-03-2017
        /// </summary>        
        /// <param name="_Year"></param>
        /// <param name="_LstSeasonID"></param>
        /// <param name="_CommandID"></param>
        /// <returns>double</returns>
        public double GetSeasonalEntitlement(int _Year, List<long> _LstSeasonID, long? _CommandID = null)
        {
            double? Sum = db.Repository<ED_SeasonalEntitlement>().GetAll().Where(se => se.Year == _Year && _LstSeasonID.Contains(se.SeasonID) &&
                (se.ED_CommandChannel.ChannelComndTypeID == _CommandID || _CommandID == null)).Sum(se => (double?)se.MAFEntitlement);

            if (Sum == null)
            {
                return 0;
            }
            else
            {
                return Sum.Value;
            }
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
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetCommandSeasonalEntitlement(_Year, _LstSeasonID, _CommandID);
        }

        public List<object> GetEntitlementYear(int _SeasonID, int _CurrentYear)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetEntitlementYear(_SeasonID, _CurrentYear);
        }

        public List<object> GetDeliveryYear(int _SeasonID, int _CurrentYear)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetDeliveryYear(_SeasonID, _CurrentYear);
        }

        public List<dynamic> GetSelectedYearDeliveries(long _Year, long _SeasonID, long _CommandID)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetSelectedYearDeliveries(_Year, _SeasonID, _CommandID);
        }
        public List<dynamic> GetChildChannelFromParentFeederByChannelID(long _ChannelID, List<long> _lstSeasonIDs, long _Year, long _CommandID)
        {
            return db.ExtRepositoryFor<EntitlementDeliveryRepository>().GetChildChannelFromParentFeederByChannelID(_ChannelID, _lstSeasonIDs, _Year, _CommandID);
        }

        public long AddUpdatePunjabIndent(IW_PunjabIndent _pi)
        {
            if (_pi.ID==0)
            {
                db.Repository<IW_PunjabIndent>().Insert(_pi);   
            }
            else
            {
                IW_PunjabIndent pi = db.Repository<IW_PunjabIndent>().FindById(_pi.ID);
                pi.FromIndentDate = _pi.FromIndentDate;
                pi.ToIndentDate = _pi.ToIndentDate;
                pi.Thal = _pi.Thal;
                pi.CJ = _pi.CJ;
                pi.CRBCPunjab = _pi.CRBCPunjab;
                pi.ChashmaDS = _pi.ChashmaDS;
                pi.GTC = _pi.GTC;
                pi.Mangla = _pi.Mangla;
                pi.Remarks = _pi.Remarks;
                pi.ModifiedBy = _pi.ModifiedBy;
                pi.ModifiedDate = _pi.ModifiedDate;
                
                db.Repository<IW_PunjabIndent>().Update(pi);   
            }
            db.Save();
            return _pi.ID;
        }
        public bool DeletePunjabIndent(IW_PunjabIndent _pi)
        {
            

            db.Repository<IW_PunjabIndent>().Update(_pi);
            db.Save();
            return true;
        }

        public List<dynamic> GetAllPunjabIndent(DateTime _ToDate, DateTime _FromDate)
        {
            List<dynamic> lstObj= db.Repository<IW_PunjabIndent>().GetAll().Where(x=>x.ToIndentDate>=_ToDate && x.FromIndentDate<=_FromDate).Select
                (x=> new 
                       {
                             ID=x.ID,
                             FromDate=x.FromIndentDate,
                             ToDate=x.ToIndentDate,
                             Thal=x.Thal,
                             CJLinkAtHead = x.CJ,
                             GreaterThal = x.GTC,
                             BelowChashmaBarrage = x.ChashmaDS,
                             CRBC=x.CRBCPunjab,
                             Mangla = x.Mangla,
                             Remarks=x.Remarks
                       }).ToList<dynamic>();
            return lstObj;
        }

        public bool IsPunjabIndentExist(long PunjabIndentID)
        {
           IW_PunjabIndent ip=db.Repository<IW_PunjabIndent>().FindById(PunjabIndentID);
           if (ip.ID>0)
           {
               return true;
           }
           else
           {
               return false;
           }

        }

        public bool DeletePunjabIndent(long PunjabIndentID)
        {
            IW_PunjabIndent ip=db.Repository<IW_PunjabIndent>().FindById(PunjabIndentID);
            db.Repository<IW_PunjabIndent>().Delete(ip);
            db.Save();
            return true;
         
        }

        public IW_PunjabIndent GetPunjabIndentByID(long PID)
        {
            IW_PunjabIndent ip = db.Repository<IW_PunjabIndent>().FindById(PID);
            return ip;
        }

      


       
    }
}
