using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.DailyData;
using PMIU.WRMIS.DAL.DataAccess.DailyData;
using System.Data;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;

namespace PMIU.WRMIS.BLL.DailyData
{
    public class DailyDataBLL : BaseBLL
    {
        DailyDataDAL dalDailyData = new DailyDataDAL();
        public bool IsGaugeValueAdded = true;

        #region Barrage Data Frequency

        public List<CO_Station> GetAllBarrages()
        {
            return dalDailyData.GetAllBarrages();
        }

        public bool BarrageFrequencyExist(long _BarrageID, long _Frequency)
        {
            return dalDailyData.BarrageFrequencyExist(_BarrageID, _Frequency);
        }

        public long ViewBarrageFrequency(long _BarrageID)
        {
            return dalDailyData.ViewBarrageFrequency(_BarrageID);
        }

        public bool SavaFrequency(long _BarrageID, long _FrequencyID, DateTime _date)
        {
            return dalDailyData.SavaFrequency(_BarrageID, _FrequencyID, _date);
        }

        public bool IsValidActor(long UserID)
        {
            return dalDailyData.IsValidActor(UserID);
        }

        public List<CO_ReadingFrequency> FrequencyValues()
        {
            return dalDailyData.FrequencyValues();
        }

        #endregion

        #region Outlet Performance Data

        public object OutletInformation(long _OutletID)
        {
            return dalDailyData.OutletInformation(_OutletID);
        }

        public void SaveData(CO_ChannelOutletsPerformance _ObjSave)
        {
            dalDailyData.SaveData(_ObjSave);
        }

        public void SaveDataAlteration(SI_OutletAlterationHistroy _ObjSave)
        {
            dalDailyData.SaveDataAlteration(_ObjSave);
        }
        public bool HasPageRights(long _UserID, string _PageName)
        {
            return dalDailyData.HasPageRights(_UserID, _PageName);
        }

        #endregion

        #region Search Criteria For Outlet Performance

        public List<CO_ChannelType> GetAllChannelTypes()
        {
            return dalDailyData.GetAllChannelTypes();
        }

        public List<CO_ChannelFlowType> GetAllChannelFlowTypes()
        {
            return dalDailyData.GetAllChannelFlowTypes();
        }

        public List<CO_ChannelComndType> GetAllChannelCommands()
        {
            return dalDailyData.GetAllChannelCommands();
        }

        public List<object> GetUserChannles(long UserID)
        {
            return dalDailyData.GetUserChannles(UserID);
        }

        public List<object> GetSearchResult(long _UserID, long _CommandNameID, long _ChannelTypeID, long _FlowTypeID, long _ChannelNameID)
        {
            return dalDailyData.GetSearchResult(_UserID, _CommandNameID, _ChannelTypeID, _FlowTypeID, _ChannelNameID);
        }


        #endregion

        #region Criteria for specific outlet

        public object ChannelParentInformation(long _ChannelID)
        {
            return dalDailyData.ChannelParentInformation(_ChannelID);
        }

        public List<object> OutletsDetail(long _ChannelID)
        {
            return dalDailyData.OutletsDetail(_ChannelID);
        }


        #endregion

        #region Daily Slip Site

        /// <summary>
        /// This function return GaugeSlipSites for a particular Station
        /// and gives its daily data for the date.
        /// Created On 23-02-2016
        /// </summary>
        /// <param name="_RiverID"></param>
        /// <param name="_Date"></param>
        /// <returns>List<CO_GaugeSlipSite></returns>
        public List<dynamic> GetDailySlipData(long _RiverID, DateTime _Date, bool _LoadLatest = false)
        {
            return dalDailyData.GetDailySlipData(_RiverID, _Date, _LoadLatest);
        }

        /// <summary>
        /// This function returns Station which are barrages or headworks in a particular province and on a specific river
        /// Created On 23-02-2016
        /// </summary>
        /// <param name="_ProvinceID"></param>
        /// <param name="_RiverID"></param>
        /// <param name="_Date"></param>
        /// <param name="_ChenabBarrageIDs"></param>
        /// <param name="_ShowSelectedBarrages"></param>
        /// <returns>List<CO_Station></returns>
        public List<dynamic> GetStationByProvinceIDAndRiverID(long _ProvinceID, long _RiverID, DateTime _Date, bool _LoadLatest = false, bool _ShowChenabOtherBarrages = false)
        {
            return dalDailyData.GetStationByProvinceIDAndRiverID(_ProvinceID, _RiverID, _Date, _LoadLatest, _ShowChenabOtherBarrages);
        }

        /// <summary>
        /// This function adds new gauge slip daily data.
        /// Created On 02-03-2016
        /// </summary>
        /// <param name="_GaugeSlipDailyData"></param>
        /// <returns>bool</returns>
        public bool AddGaugeSlipDailyData(CO_GaugeSlipDailyData _GaugeSlipDailyData)
        {
            return dalDailyData.AddGaugeSlipDailyData(_GaugeSlipDailyData);
        }

        /// <summary>
        /// This function updates new gauge slip daily data.
        /// Created On 02-03-2016
        /// </summary>
        /// <param name="_GaugeSlipDailyData"></param>
        /// <returns>bool</returns>
        public bool UpdateGaugeSlipDailyData(CO_GaugeSlipDailyData _GaugeSlipDailyData)
        {
            return dalDailyData.UpdateGaugeSlipDailyData(_GaugeSlipDailyData);
        }

        /// <summary>
        /// This function return GaugeSlipDailyData based on the GaugeSlipDailyData id.
        /// Created On 02-03-2016
        /// </summary>
        /// <param name="_GaugeSlipSiteID"></param>
        /// <param name="_Date"></param>
        /// <returns>CO_GaugeSlipDailyData</returns>
        public CO_GaugeSlipDailyData GetBySiteIDAndDate(long _GaugeSlipSiteID, DateTime _Date)
        {
            return dalDailyData.GetBySiteIDAndDate(_GaugeSlipSiteID, _Date);
        }

        /// <summary>
        /// This function returns data for the gauge slip.
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_LoadLatest"></param>
        /// <returns>DataSet</returns>
        public DataSet GetGaugeSlipData(DateTime _Date, bool _LoadLatest = false)
        {
            return dalDailyData.GetGaugeSlipData(_Date, _LoadLatest);
        }

        /// <summary>
        /// This function returns data for the gauge slip dam.
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_LoadLatest"></param>
        /// <param name="_GaugeSlipIndex"></param>
        /// <returns></returns>
        public DataTable GetGaugeSlipDamData(DateTime _Date, int _GaugeSlipIndex = 0, bool _LoadLatest = false)
        {
            return dalDailyData.GetGaugeSlipDamData(_Date, _GaugeSlipIndex, _LoadLatest);
        }

        /// <summary>
        /// This function returns data for the gauge slip other than dams.
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_GaugeSlipIndex"></param>
        /// <param name="_LoadLatest"></param>
        /// <returns>DataTable</returns>
        public DataTable GetGaugeSlipOtherData(DateTime _Date, int _GaugeSlipIndex = 1, bool _LoadLatest = false)
        {
            return dalDailyData.GetGaugeSlipOtherData(_Date, _GaugeSlipIndex, _LoadLatest);
        }

        /// <summary>
        /// This function fetches gauge slip record based on Reading Date and GaugeSlipSite ID.
        /// Created On 15-08-2017
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_GaugeSlipSiteID"></param>
        /// <returns>CO_GaugeSlipDailyData</returns>
        public CO_GaugeSlipDailyData GetGaugeSlipRecord(DateTime _Date, long _GaugeSlipSiteID)
        {
            return dalDailyData.GetGaugeSlipRecord(_Date, _GaugeSlipSiteID);
        }

        #endregion

        #region Daily Gauge Reading Data(Operational Data)
        public List<object> GetDailyGaugeReadingData(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, string _ChannelName, DateTime _Date, int _Session, int _PageIndex, int _PageSize)
        {
            return dalDailyData.GetDailyGaugeReadingData(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _ChannelName, _Date, _Session, _PageIndex, _PageSize);
        }

        /// <summary>
        /// This function returns Reason for change.
        /// Created On 18-02-2016.
        /// </summary>
        /// <param name="_ZoneId"></param>
        /// <returns>List<CO_Circle>()</returns>
        public List<CO_ReasonForChange> GetReasonForChange(long _ID = -1)
        {
            return dalDailyData.GetReasonForChange(_ID);
        }

        public List<object> GetAuditTrail(DateTime _Date, int _Session, long _DailyGaugeReadingID, int _PageIndex, int _PageSize)
        {
            return dalDailyData.GetAuditTrail(_Date, _Session, _DailyGaugeReadingID, _PageIndex, _PageSize);
        }

        public Tuple<double?, double?> GetChannelTypeMinMaxValueByChannelID(long _ChannelID)
        {
            return dalDailyData.GetChannelTypeMinMaxValueByChannelID(_ChannelID);
        }

        public DD_GetDailyGaugeReadingNotifyData_Result GetDailyGaugeReadingNotifyData(long _DailyGaugeReadingID)
        {
            return dalDailyData.GetDailyGaugeReadingNotifyData(_DailyGaugeReadingID);
        }

        public DD_GetDailyIndentNotifyData_Result GetDailyIndentNotifyData(long _GaugeID)
        {
            return dalDailyData.GetDailyIndentNotifyData(_GaugeID);
        }

        public List<UA_GetNotificationsRecievers_Result> GetDailyDataNotifyRecievers(long _EventID, long _DailyGaugeReadingID)
        {
            return dalDailyData.GetDailyDataNotifyRecievers(_EventID, _DailyGaugeReadingID);
        }

        public List<UA_GetNotificationsRecievers_Result> GetDailyIndentNotifyRecievers(long _EventID, long _UserID)
        {
            return dalDailyData.GetDailyIndentNotifyRecievers(_EventID, _UserID);
        }
        //public void SaveGaugeValue(string _Date, string _GaugeValue, string _ReasonForChange, string _GaugeReadingID, string _IsToUpdate)
        //{
        //    db.ExtRepositoryFor<DailyDataRepository>().GetAuditTrail
        //}

        /// <summary>
        /// Salman's Work
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        /// 
        public object GetChannelLimit(long _GaugeID)
        {
            return dalDailyData.GetChannelLimit(_GaugeID);
        }

        public double? CalculateDischarge(long _GaugeID, double _NewDischargeValue)
        {
            return dalDailyData.CalculateDischarge(_GaugeID, _NewDischargeValue);
        }

        public bool UpdateDischarge(long _ID, double _Discharge, long _ReasonForChangeID, double _NewGaugeValue, long _UserID)
        {
            return dalDailyData.UpdateDischarge(_ID, _Discharge, _ReasonForChangeID, _NewGaugeValue, _UserID);
        }

        public double? GetDefaultDesignDischarge(long GaugeID)
        {
            return dalDailyData.GetDefaultDesignDischarge(GaugeID);
        }

        public bool HasOffTake(long _GaugeID)
        {
            return dalDailyData.HasOffTake(_GaugeID);
        }

        /// <summary>
        /// Salman's Work end here 
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>



        #endregion

        #region Outlet history

        public object OutletHistoryInformation(long _ChannelID, long _OutletID)
        {
            return dalDailyData.OutletHistoryInformation(_ChannelID, _OutletID);
        }

        public List<object> GetHistory(long OutletID, DateTime? FromDate, DateTime? ToDate)
        {
            return dalDailyData.GetHistory(OutletID, FromDate, ToDate);
        }

        public long GetChannelID(long _OutletID)
        {
            return dalDailyData.GetChannelID(_OutletID);
        }


        #endregion

        #region Daily Barrage Discharge Data

        /// <summary>
        /// this function return Daily Barrage Discharge Data
        /// Created on:03/03/2016
        /// </summary>
        /// <returns></returns>
        public DataTable GetBarrageDischargeData(long _BarrageID, DateTime _Date)
        {
            //return db.ExtRepositoryFor<DailyDataRepository>().GetBarrageDischargeData();           

            return dalDailyData.GetBarrageDischargeData(_BarrageID, _Date);
        }

        /// <summary>
        /// this function return all attributes based on BarrageID
        /// Created On:29/04/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetBarrageAttribute(long _BarrageID)
        {
            return dalDailyData.GetBarrageAttribute(_BarrageID);
        }


        /// <summary>
        /// this function return Station By User ID
        /// Created On:08/03/2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>UA_AssociatedStations</returns>
        public UA_AssociatedStations GetStationByUserID(long _UserID)
        {
            return dalDailyData.GetStationByUserID(_UserID);

        }

        /// <summary>
        /// this function will get design discharge by channel ID
        /// Created On: 08/03/2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>CO_ChannelGauge</returns>
        public CO_ChannelGauge GetDesignDischargeByChannelID(long _ChannelID)
        {
            return dalDailyData.GetDesignDischargeByChannelID(_ChannelID);
        }

        /// <summary>
        /// this function insert values into stored procedure
        /// Created On: 6/4/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <param name="_ReadingDateTime"></param>
        /// <param name="_IDGaugeDischarge"></param>
        /// <param name="_UserID"></param>
        /// <param name="_ReasonForChange"></param>
        /// <returns>int</returns>
        public int UpdateBarrageDischargeData(long _BarrageID, string _ReadingDateTime, string _IDGaugeDischarge, long _UserID, string _ReasonForChange, string _Source)
        {
            return dalDailyData.UpdateBarrageDischargeData(_BarrageID, _ReadingDateTime, _IDGaugeDischarge, _UserID, _ReasonForChange, _Source);
        }


        /// <summary>
        /// this function Add values into stored procedure
        /// Created On:22/04/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <param name="_ReadingDateTime"></param>
        /// <param name="_IDGaugeDischarge"></param>
        /// <param name="_UserID"></param>
        /// <returns>int</returns>
        public int AddBarrageDischargeData(long _BarrageID, DateTime _ReadingDate, string _ReadingTime, string _IDGaugeDischarge, long _UserID, bool _GRMissed, string _Source)
        {
            return dalDailyData.AddBarrageDischargeData(_BarrageID, _ReadingDate, _ReadingTime, _IDGaugeDischarge, _UserID, _GRMissed, _Source);
        }

        public int AddBarrageDischargeDataAndroid(long _BarrageID, DateTime _ReadingDate, string _ReadingTime, string _IDGaugeDischarge, long _UserID, bool _GRMissed, string _Source)
        {
            return dalDailyData.AddBarrageDischargeDataAndroid(_BarrageID, _ReadingDate, _ReadingTime, _IDGaugeDischarge, _UserID, _GRMissed, _Source);
        }

        /// <summary>
        /// this function return Barrage Gauge Reading frequency on the basis of barrage ID
        /// Created On: 20/04/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <returns>CO_BarrageGaugeReadingFrequency</returns>
        public CO_BarrageGaugeReadingFrequency GetBarrageGaugeReadingFrequency(long _BarrageID)
        {
            return dalDailyData.GetBarrageGaugeReadingFrequency(_BarrageID);
        }

        /// <summary>
        /// this function return all barrages based on punjab province
        /// Created On:26/04/2016
        /// </summary>
        /// <returns>List<CO_Station></returns>
        public List<CO_Station> GetAllPunjabBarrages()
        {
            return dalDailyData.GetAllPunjabBarrages();
        }

        /// <summary>
        /// this function will return gauge id on the basis of attribute id
        /// Created On:26/04/2016
        /// </summary>
        /// <param name="_AttributeID"></param>
        /// <returns>CO_Attribute</returns>
        public CO_Attribute GetAttribute(long _AttributeID)
        {
            return dalDailyData.GetAttribute(_AttributeID);
        }

        /// <summary>
        /// this function get Get Barrage Daily Discharge Data History
        /// Created On: 7/13/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <param name="_ReadingDateTime"></param>
        /// <returns>DataTable</returns>
        public DataTable GetBarrageDailyDischargeDataHistory(long _BarrageID, string _ReadingDateTime)
        {
            return dalDailyData.GetBarrageDailyDischargeDataHistory(_BarrageID, _ReadingDateTime);
        }

        #endregion

        #region Daily Operational Data - Mobile

        public List<GetUserGauges_Result> GetUserGauges(long _UserID)
        {
            return dalDailyData.GetUserGauges(_UserID);
        }

        public string AddGaugeReading(long _UserID, long _GaugeID, bool _GaugeF, bool _GaugeP, double _GaugeValue, string _ImagePath, string _Remarks, double? _Longitude, double? _Latitude, string _Source, DateTime? _ReadingDateTime = null)
        {
            CO_ChannelGauge mdlChannelGauge = new ChannelBLL().GetChannelGaugeByID(_GaugeID);

            bool IsChannelClosed = IsChannelClosedNow(mdlChannelGauge.ChannelID.Value, (_ReadingDateTime == null ? DateTime.Now : _ReadingDateTime.Value));

            string result = dalDailyData.AddUserGauges(_UserID, _GaugeID, _GaugeF, _GaugeP, _GaugeValue, _ImagePath, _Remarks, _Longitude, _Latitude, _Source, IsChannelClosed, _ReadingDateTime);
            this.IsGaugeValueAdded = dalDailyData.IsGaugeValueAdded;
            return result;
        }

        public Double? GetTailGaugeDischargeWithOffTakes(long _GaugeID, double GaugeValue)
        {
            return dalDailyData.GetTailGaugeDischargeWithOffTakes(_GaugeID, GaugeValue);
        }

        public List<GetUserGaugesStationBaised_Result> GetUserGaugesByLocation(long _UserID)
        {
            return dalDailyData.GetUserGaugesByLocation(_UserID);
        }

        //public List<GetUserDivisions_Result> GetUserDivisions(long _UserID)
        //{
        //    DailyDataDAL dalDailyData = new DailyDataDAL();
        //    return dalDailyData.GetUserDivisions(_UserID);
        //}

        public List<GetUserSubDivisions_Result> GetUserSubDivisions(long _UserID)
        {
            return dalDailyData.GetUserSubDivisions(_UserID);
        }

        public List<GetUserSection_Result> GetUserSections(long _UserID)
        {
            return dalDailyData.GetUserSections(_UserID);
        }

        public List<GetGauges_Result> GetUserChannelAndGauges(long _UserID)
        {
            return dalDailyData.GetUserChannelAndGauges(_UserID);
        }

        public List<GetDailyGaugeReadingAndroid_Result> GetDailyGaugeReading(long _SectionID, long _ChannelID, DateTime _Date)
        {
            return dalDailyData.GetDailyGaugeReading(_SectionID, _ChannelID, _Date);
        }

        public long GetDailyGaugeReadingID(DateTime _Date, long _GaugeID, long _UserID)
        {

            CO_ChannelDailyGaugeReading _reading = db.Repository<CO_ChannelDailyGaugeReading>().Query().Get().Where(x => x.GaugeID == _GaugeID && x.GaugeReaderID == _UserID && x.ReadingDateTime == _Date).ToList().OrderByDescending(x => x.ReadingDateTime).ToList().ElementAt(0);
            if (_reading != null)
                return _reading.ID;
            return 0;
        }

        public long UpdateGaugeReading(long _GaugeReadingID, double _Discharge, long _ReasonForChangeID, double _NewGaugeValue, long _UserID, double? _Longitude, double? _Latitude, string _source)
        {

            return dalDailyData.UpdateGaugeReading(_GaugeReadingID, _Discharge, _ReasonForChangeID, _NewGaugeValue, _UserID, _Longitude, _Latitude, _source);
        }

        public long AddGaugeReading_SDOXEN(long _GaugeID, long _ReasonForChangeID, double _GaugeValue, long _UserID, double? _Longitude, double? _Latitude, string _source)
        {
            CO_ChannelGauge mdlChannelGauge = new ChannelBLL().GetChannelGaugeByID(_GaugeID);

            bool IsChannelClosed = IsChannelClosedNow(mdlChannelGauge.ChannelID.Value, DateTime.Now);

            return dalDailyData.AddGaugeReading_SDOXEN(_UserID, _GaugeID, _GaugeValue, _ReasonForChangeID, _Longitude, _Latitude, _source, IsChannelClosed);
        }

        public bool AddGauge_BedLevel_DischargeMeasurements(long _UserID, long _GaugeID, double _ParamN_B, double _ParamD_H, double _ObsrvdDschrg, int _GCorrectType, double? _GCorrectValue, double? _Longitude, double? _Latitude, string _Source)
        {
            return dalDailyData.AddGauge_BedLevel_DischargeMeasurements(_UserID, _GaugeID, _ParamN_B, _ParamD_H, _ObsrvdDschrg, _GCorrectType, _GCorrectValue, _Longitude, _Latitude, _Source);
        }

        public bool AddGauge_CrestLevel_DischargeMeasurements(long _UserID, long _GaugeID, double _ParamN_B, double _ParamD_H, double _ObsrvdDschrg, double? _Longitude, double? _Latitude, string _Source)
        {
            return dalDailyData.AddGauge_CrestLevel_DischargeMeasurements(_UserID, _GaugeID, _ParamN_B, _ParamD_H, _ObsrvdDschrg, _Longitude, _Latitude, _Source);
        }

        public bool AddOutletPerformance(long _UserID, long _OutletID, double? _HeadAboveCrest, double? _WorkingHead, double _ObsrvdDschrg, double? _Longitude, double? _Latitude, string _Source)
        {
            return dalDailyData.AddOutletPerformance(_UserID, _OutletID, _HeadAboveCrest, _WorkingHead, _ObsrvdDschrg, _Longitude, _Latitude, _Source);
        }

        public string AddGaugeValue_Scheduled(long _UserID, long _GaugeID, bool _GaugeF, bool _GaugeP, double _GaugeValue, string _ImagePath, string _Remarks, double? _Longitude, double? _Latitude, long? _ScheduledID, string _DataSource)
        {
            CO_ChannelGauge mdlChannelGauge = new ChannelBLL().GetChannelGaugeByID(_GaugeID);

            bool IsChannelClosed = IsChannelClosedNow(mdlChannelGauge.ChannelID.Value, DateTime.Now);

            return dalDailyData.AddGaugeValue_Scheduled(_UserID, _GaugeID, _GaugeF, _GaugeP, _GaugeValue, _ImagePath, _Remarks, _Longitude, _Latitude, _ScheduledID, _DataSource, IsChannelClosed);
        }

        /// <summary>
        /// returns list of Outlets againts a channel within a section
        /// Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public List<GetChannelSectionOutlets_Result> GetOutletBySectionAndChannelID(long _SectionID, long _ChannelID)
        {
            return dalDailyData.GetOutletBySectionAndChannelID(_SectionID, _ChannelID);
        }

        public CO_Station GetBarrageByUser(long _UserID)
        {
            return dalDailyData.GetBarrageByUser(_UserID);
        }

        public List<string> GetBarrageTimeStamps(long _BarrageID)
        {
            return dalDailyData.GetBarrageTimeStamps(_BarrageID);
        }

        public List<GetBarrageDailyDischargeDataMobile_Result> GetBarrageAttributes(long _BarrageID, DateTime _Date)
        {
            return dalDailyData.GetBarrageAttributes(_BarrageID, _Date);
        }

        public long? GetGaugeIDByAttributeID(long _AttributeID)
        {
            return dalDailyData.GetGaugeIDByAttributeID(_AttributeID);
        }

        public bool HasDischargeParameters(long _GaugeID)
        {
            return dalDailyData.HasDischargeParameters(_GaugeID);
        }

        public bool IsRecordLocked(long _GaugeReadingID)
        {
            return dalDailyData.IsRecordLocked(_GaugeReadingID);
        }

        public List<object> GetDivisionsByUserID(long _UserID)
        {
            return dalDailyData.GetDivisionsByUserID(_UserID);
        }

        #endregion

        #region Gauge Bulk Entry

        public List<DD_GetGaugesBulkEntry_Result> GetGaugesBulkData(long _SubDivisionID, long _SectionID, int _Session, DateTime _ReadingDate)
        {
            return dalDailyData.GetGaugesBulkData(_SubDivisionID, _SectionID, _Session, _ReadingDate);
        }

        #endregion

        //public long GetChannelParentID(long _ChannelID)
        //{

        //    return dalDD.GetChannelParentID(_ChannelID);
        //}

        public List<object> GetMA_ADMUser(long _IrrigationBoundryID, long _Designations)
        {
            return dalDailyData.GetMA_ADMUser(_IrrigationBoundryID, _Designations);
        }

        public List<object> GetMAUser(long _UseriD)
        {
            return dalDailyData.GetMAUser(_UseriD);
        }
        public List<object> GetUser(long _UseriD)
        {
            return dalDailyData.GetUser(_UseriD);
        }

        public DataSet GetMeterReadingSearch(string _ReadingType, DateTime? _FromDate, DateTime? _ToDate, long? _UserID, long? _ActivityByID)
        {
            return dalDailyData.GetMeterReadingSearch(_ReadingType, _FromDate, _ToDate, _UserID, _ActivityByID);
        }

        public bool IsChannelClosedNow(long _ChannelID, DateTime _dtNow)
        {
            int Count = dalDailyData.GetChannelParentID(_ChannelID);

            if (Count > 0)
                return true;
            return false;

        }

        public string AddMeterReading(long _UserID, string _ReadingType, int _MeterReading, float? _PetrolQuantity, string _remarks, string _Attachment1, string _Attachment2, DateTime _MobileDate, double? _Longitude, double? _Latitude)
        {


            return dalDailyData.AddMeterReading(_UserID, _ReadingType, _MeterReading, _PetrolQuantity, _remarks, _Attachment1, _Attachment2, _MobileDate, _Longitude, _Latitude);
        }


        #region Water Theft for Monitoring

        public DataSet GetWaterTheftSearch(DateTime? _FromDate, DateTime? _ToDate, long? _UserID, string _From, long? _SelectedActivityBy)
        {
            return dalDailyData.GetWaterTheftSearch(_FromDate, _ToDate, _UserID, _From, _SelectedActivityBy);
        }

        public object GetRotationalDetail(long _CaseID)
        {
            return dalDailyData.GetRotationalDetail(_CaseID);
        }

        public object GetLeavesDetail(long _CaseID)
        {
            return dalDailyData.GetLeavesDetail(_CaseID);
        }

        public object GetFuelMeterDetail(long _CaseID)
        {
            return dalDailyData.GetFuelMeterDetail(_CaseID);
        }

        public string GetGISURL()
        {
            return dalDailyData.GetGISURL();
        }

        #endregion

        public List<GetChannelSectionOutlet_Result> GetChannelSectionOutlet(long _UserID)
        {
            return dalDailyData.GetChannelSectionOutlet(_UserID);
        }

        public List<dynamic> GetAllGaugeSlipSiteDateForDiagram(DateTime _readingDate)
        {
            return dalDailyData.GetAllGaugeSlipSiteDateForDiagram(_readingDate);
        }

        public List<CO_GaugeSlipDailyData> GetMissedGaugeSlipRecord(double _year, double   _month, double    _GaugeSlipSiteID)
        {

            return dalDailyData.GetMissedeGaugeSlipRecord(_year, _month, _GaugeSlipSiteID);

            // return dalDailyData.GetMissedeGaugeSlipRecord(_Date, _GaugeSlipSiteID);   

        }


    }
}
