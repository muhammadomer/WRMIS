using PMIU.WRMIS.DAL.DataAccess.EntitlementDelivery;
using PMIU.WRMIS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using System.Data;

namespace PMIU.WRMIS.BLL.EntitlementDelivery
{
    public class EntitlementDeliveryBLL : BaseBLL
    {
        EntitlementDeliveryDAL dalEntitlementDelivery = new EntitlementDeliveryDAL();

        public List<object> GetYears(long _CommandID)
        {
            return dalEntitlementDelivery.GetYears(_CommandID);
        }

        public List<object> GetMainCanals(long _CommandID)
        {
            return dalEntitlementDelivery.GetMainCanals(_CommandID);
        }

        public List<dynamic> GetCannalSystemByCommandTypeID(long _UserID, bool _UserBaseLoading, long? _IrrigationLevel, long _CommandTypeID)
        {
            return dalEntitlementDelivery.GetCannalSystemByCommandTypeID(_UserID, _UserBaseLoading, _IrrigationLevel, _CommandTypeID);
        }

        public List<dynamic> GetDistinctYearsBySeason(long _SeasonID)
        {
            return dalEntitlementDelivery.GetDistinctYearsBySeason(_SeasonID);
        }

        /// <summary>
        /// This function returns distinct latest approved years.
        /// Created On 06-07-2016
        /// </summary>
        /// <param name="_SeasonID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDistinctApprovedYearsBySeason(long _SeasonID)
        {
            return dalEntitlementDelivery.GetDistinctApprovedYearsBySeason(_SeasonID);
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
            return dalEntitlementDelivery.GetIndusRabiShare(_Year, _Scenario);
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
            return dalEntitlementDelivery.GetIndusKharifShare(_Year, _Scenario);
        }

        public List<dynamic> GetChannelEntitlementBySearchCriteria(long _ChannelID, List<long> _lstSeasonIDs, long _Year)
        {
            return dalEntitlementDelivery.GetChannelEntitlementBySearchCriteria(_ChannelID, _lstSeasonIDs, _Year);
        }

        public List<dynamic> GetChildChannelEntitlementBySearchCriteria(long _ChannelID, List<long> _lstSeasonIDs, long _Year)
        {
            return dalEntitlementDelivery.GetChildChannelEntitlementBySearchCriteria(_ChannelID, _lstSeasonIDs, _Year);
        }
        public List<dynamic> GetChildChannelByMainChannel(long _ChannelID, List<long> _lstSeasonIDs, long _Year, long _CommandID)
        {
            return dalEntitlementDelivery.GetChildChannelByMainChannel(_ChannelID, _lstSeasonIDs, _Year, _CommandID);
        }
        
        public List<dynamic> ViewEntitlementsBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            return dalEntitlementDelivery.ViewEntitlementsBySearchCriteria(_ChannelID, _SeasonID, _Year);
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
            return dalEntitlementDelivery.Get7782AverageCommandSeasonalCusecs(_SeasonID, _CommandID);
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
            return dalEntitlementDelivery.GetPara2AverageCommandSeasonalMAF(_SeasonID, _CommandID);
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
            return dalEntitlementDelivery.SaveRetrieveRabiAverageData(_CommandID, _Year, _UserID, _LstProvincialEntitlements);
        }

        public List<dynamic> SaveRetrieveRabiAverageDataDeliveries(long _CommandID, int _Year, long _UserID, List<double> _LstProvincialEntitlements)
        {
            return dalEntitlementDelivery.SaveRetrieveRabiAverageDataDeliveries(_CommandID, _Year, _UserID, _LstProvincialEntitlements);
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
            return dalEntitlementDelivery.GetAverageData(_CommandID, _Year, _SeasonID, _LstProvincialEntitlements);
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
        public List<object> GetAverageDataForSelectedYear(long _CommandID, int _Year, long _SeasonID)
        {
            return dalEntitlementDelivery.GetAverageDataForSelectedYear(_CommandID, _Year, _SeasonID);
        }

        public List<dynamic> GetAverageDataDeliveries(long _CommandID, int _Year, long _SeasonID, List<double> _LstProvincialEntitlements)
        {
            return dalEntitlementDelivery.GetAverageDataDeliveries(_CommandID, _Year, _SeasonID, _LstProvincialEntitlements);
        }

        public double GetMAFEntitlementBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            return dalEntitlementDelivery.GetMAFEntitlementBySearchCriteria(_ChannelID, _SeasonID, _Year);
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
            return dalEntitlementDelivery.Get7782ChannelAverageMAF(_LstSeasonIDs, _ChannelID);
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
            return dalEntitlementDelivery.GetPara2ChannelAverageMAF(_LstSeasonIDs, _ChannelID);
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
            return dalEntitlementDelivery.SaveRetrieveKharifAverageData(_CommandID, _Year, _UserID, _LstProvincialEntitlements);
        }

        public List<dynamic> SaveRetrieveKharifAverageDataNew(long _CommandID, int _Year, long _UserID, List<double> _LstProvincialEntitlements)
        {
            return dalEntitlementDelivery.SaveRetrieveKharifAverageDataNew(_CommandID, _Year, _UserID, _LstProvincialEntitlements);
        }

        public List<dynamic> ViewChildEntitlementsBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            return dalEntitlementDelivery.ViewChildEntitlementsBySearchCriteria(_ChannelID, _SeasonID, _Year);
        }

        public double GetMAFChildEntitlementBySearchCriteria(long _ChannelID, long _SeasonID, long _Year)
        {
            return dalEntitlementDelivery.GetMAFChildEntitlementBySearchCriteria(_ChannelID, _SeasonID, _Year);
        }

        /// <summary>
        /// This function check if there are any Entitlements approved for particular year and season.
        /// Created on 31-08-2017
        /// </summary>
        /// <param name="_Year"></param>
        /// <param name="_LstSeasonIDs"></param>
        /// <returns>bool</returns>
        public bool IsEntitlementApprove(long _Year, List<long> _LstSeasonIDs)
        {
            return dalEntitlementDelivery.IsEntitlementApprove(_Year, _LstSeasonIDs);
        }

        public List<ED_SeasonalEntitlement> GetAllCommandChannelsByYearAndSeason(long _Year, long _SeasonID)
        {
            return dalEntitlementDelivery.GetAllCommandChannelsByYearAndSeason(_Year, _SeasonID);
        }

        /// <summary>
        /// This function updates Seasonal Entitlement
        /// Created On 08-02-2017
        /// </summary>
        /// <param name="_MdlSeasonalEntitlement"></param>
        /// <returns>bool</returns>
        public bool UpdateSeasonalEntitlement(ED_SeasonalEntitlement _MdlSeasonalEntitlement)
        {
            return dalEntitlementDelivery.UpdateSeasonalEntitlement(_MdlSeasonalEntitlement);
        }
        public double GetShortage(double _total, double _value7782)
        {
            return dalEntitlementDelivery.GetShortage(_total, _value7782);
        }
        public DataTable GetTenDailyBySeasonYearCommand(long _Year, long _SeasonID, long _CommandID)
        {
            return dalEntitlementDelivery.GetTenDailyBySeasonYearCommand(_Year, _SeasonID, _CommandID);
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
            return dalEntitlementDelivery.GetPlanScenario(_SeasonID, _Year, _Scenario);
        }

        /// <summary>
        /// This function adds new Provincial Entitlement.
        /// Created By 08-02-2017
        /// </summary>
        /// <param name="_ProvincialEntitlement"></param>
        /// <returns>bool</returns>
        public bool AddProvincialEntitlement(ED_ProvincialEntitlement _ProvincialEntitlement)
        {
            return dalEntitlementDelivery.AddProvincialEntitlement(_ProvincialEntitlement);
        }

        /// <summary>
        /// This function updates Provincial Entitlement.
        /// Created By 15-02-2017
        /// </summary>
        /// <param name="_ProvincialEntitlement"></param>
        /// <returns>bool</returns>
        public bool UpdateProvincialEntitlement(ED_ProvincialEntitlement _ProvincialEntitlement)
        {
            return dalEntitlementDelivery.UpdateProvincialEntitlement(_ProvincialEntitlement);
        }

        /// <summary>
        /// This function saves Seasonal Entitlement
        /// Created On 08-02-2017
        /// </summary>
        /// <param name="_LstSeasonalEntitlement"></param>
        /// <returns>bool</returns>
        public bool AddSeasonalEntitlements(List<ED_SeasonalEntitlement> _LstSeasonalEntitlement)
        {
            return dalEntitlementDelivery.AddSeasonalEntitlements(_LstSeasonalEntitlement);
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
            return dalEntitlementDelivery.GetProvincialEntitlement(_CommandID, _Year, _Season, _ProvinceID);
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
            return dalEntitlementDelivery.UpdateRabiAverageSelected(_LstSeasonalAverageIDs, _SelectedAverage, _SelectedYear);
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
            return dalEntitlementDelivery.UpdateDeliveriesRabiAverageSelected(_LstSeasonalAverageIDs, _SelectedAverage, _SelectedYear);
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
            return dalEntitlementDelivery.UpdateKharifAverageSelected(_LstSeasonalAverageIDs, _EKSelectedAverage, _LKSelectedAverage, _EKSelectedYear, _LKSelectedYear);
        }

        public bool UpdateKharifAverageSelectedDeliveries(List<long> _LstSeasonalAverageIDs, string _EKSelectedAverage, string _LKSelectedAverage, short _EKYear, short _LKYear)
        {
            return dalEntitlementDelivery.UpdateKharifAverageSelectedDeliveries(_LstSeasonalAverageIDs, _EKSelectedAverage, _LKSelectedAverage, _EKYear, _LKYear);
        }

        public double GetMAFByCusecs(long _TenDailyID, double _CusecVal, char _ParentChild = 'P')
        {
            return dalEntitlementDelivery.GetMAFByCusecs(_TenDailyID, _CusecVal, _ParentChild);
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
            return dalEntitlementDelivery.GenerateEntitlements(_Year, _SeasonID, _CommandID, _UserID);
        }

        public double GetMAFFromSeasonalEntitlement(long _Year, List<long> _lstSeasonID, long _CommandChannelD)
        {
            return dalEntitlementDelivery.GetMAFFromSeasonalEntitlement(_Year, _lstSeasonID, _CommandChannelD);
        }

        public ED_ChannelEntitlement GetChannelEntitlementByID(long _ID)
        {
            return dalEntitlementDelivery.GetChannelEntitlementByID(_ID);
        }

        public bool UpdateChannelEntitlement(ED_ChannelEntitlement _ChannelEntitlement)
        {
            return dalEntitlementDelivery.UpdateChannelEntitlement(_ChannelEntitlement);
        }

        public double GetSumOfRabiTenDailies(long _Year, long _SeasonID, long _CommandChannelID)
        {
            return dalEntitlementDelivery.GetSumOfRabiTenDailies(_Year, _SeasonID, _CommandChannelID);
        }

        public double GetSumOfEKharifTenDailies(long _Year, long _SeasonID, long _CommandChannelID)
        {
            return dalEntitlementDelivery.GetSumOfEKharifTenDailies(_Year, _SeasonID, _CommandChannelID);
        }

        public double GetSumOfLKharifTenDailies(long _Year, long _SeasonID, long _CommandChannelID)
        {
            return dalEntitlementDelivery.GetSumOfLKharifTenDailies(_Year, _SeasonID, _CommandChannelID);
        }

        public List<ED_ChannelEntitlement> ChannelEntitlementForPercentageUpdate(long _Year, long _SeasonID, long _CommandChannelID)
        {
            return dalEntitlementDelivery.ChannelEntitlementForPercentageUpdate(_Year, _SeasonID, _CommandChannelID);
        }

        public bool UpdateChannelEntitlementPercentage(ED_ChannelEntitlement _ChannelEntitlement)
        {
            return dalEntitlementDelivery.UpdateChannelEntitlementPercentage(_ChannelEntitlement);
        }

        public int UpdatePercentageAndGenerateEntitlements(long _CommandID, short _Year, short _SeasonID, long _UserID)
        {
            return dalEntitlementDelivery.UpdatePercentageAndGenerateEntitlements(_CommandID, _Year, _SeasonID, _UserID);
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
            return dalEntitlementDelivery.GetEditInformation(_Year, _SeasonID, _CommandID);
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
            return dalEntitlementDelivery.GetCommandSeasonalEntitlement(_CommandChannelID, _Year, _SeasonID);
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
            return dalEntitlementDelivery.GetRabiShare(_Year, _Scenario);
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
            return dalEntitlementDelivery.GetKharifShare(_Year, _Scenario);
        }

        public CO_ChannelGauge GetChannelGaugeByChannelID(long _ChannelID)
        {
            return dalEntitlementDelivery.GetChannelGaugeByChannelID(_ChannelID);
        }

        public ED_CommandChannel GetCommandChannelByID(long _ChannelID)
        {
            return dalEntitlementDelivery.GetCommandChannelByID(_ChannelID);
        }

        /// <summary>
        /// This function returns the 7782 MAF for a Season
        /// Created On 27-03-2017
        /// </summary>
        /// <param name="_LstSeasonID"></param>        
        /// <returns>double</returns>
        public double Get7782AverageSeasonalMAF(List<long> _LstSeasonID)
        {
            return dalEntitlementDelivery.Get7782AverageSeasonalMAF(_LstSeasonID);
        }

        /// <summary>
        /// This function returns the Para2 MAF for a Season
        /// Created On 27-03-2017
        /// </summary>
        /// <param name="_LstSeasonID"></param>        
        /// <returns>double</returns>
        public double GetPara2AverageSeasonalMAF(List<long> _LstSeasonID)
        {
            return dalEntitlementDelivery.GetPara2AverageSeasonalMAF(_LstSeasonID);
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
            return dalEntitlementDelivery.GetSeasonalEntitlement(_Year, _LstSeasonID, _CommandID);
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
            return dalEntitlementDelivery.GetCommandSeasonalEntitlement(_Year, _LstSeasonID, _CommandID);
        }

        public List<object> GetEntitlementYear(int _SeasonID, int _CurrentYear)
        {
            return dalEntitlementDelivery.GetEntitlementYear(_SeasonID, _CurrentYear);
        }

        public List<object> GetDeliveryYear(int _SeasonID, int _CurrentYear)
        {
            return dalEntitlementDelivery.GetDeliveryYear(_SeasonID, _CurrentYear);
        }

        public List<dynamic> GetSelectedYearDeliveries(long _Year, long _SeasonID, long _CommandID)
        {
            return dalEntitlementDelivery.GetSelectedYearDeliveries(_Year, _SeasonID, _CommandID);
        }

        public List<dynamic> GetChildChannelFromParentFeederByChannelID(long _ChannelID, List<long> _lstSeasonIDs, long _Year, long _CommandID)
        {
            return dalEntitlementDelivery.GetChildChannelFromParentFeederByChannelID(_ChannelID, _lstSeasonIDs, _Year, _CommandID);
        }

        public long AddUpdatePunjabIndent(IW_PunjabIndent pi)
        {
            return dalEntitlementDelivery.AddUpdatePunjabIndent(pi);
        }
        public bool DeletePunjabIndent(IW_PunjabIndent pi)
        {
            return dalEntitlementDelivery.DeletePunjabIndent(pi);
        }

        public List<dynamic> GetAllPunjabIndent(DateTime _ToDate, DateTime _FromDate)
        {
            return dalEntitlementDelivery.GetAllPunjabIndent(_ToDate,_FromDate);
        }

        public bool IsPunjabIndentExist(long PunjabIndentID)
        {
            return dalEntitlementDelivery.IsPunjabIndentExist(PunjabIndentID);
        }

        public bool DeletePunjabIndent(long PunjabIndentID)
        {
            return dalEntitlementDelivery.DeletePunjabIndent(PunjabIndentID);
        }

        public IW_PunjabIndent GetPunjabIndentByID(long PID)
        {
            return dalEntitlementDelivery.GetPunjabIndentByID(PID);
        }

       
    }
}
