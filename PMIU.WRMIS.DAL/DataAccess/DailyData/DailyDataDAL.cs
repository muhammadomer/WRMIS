using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.DailyData;
using System.Data;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.Channel;
using System.Data.Entity;

namespace PMIU.WRMIS.DAL.DataAccess.DailyData
{
    public class DailyDataDAL
    {
        public bool IsGaugeValueAdded = false;
        public DailyDataDAL()
        {

        }

        ContextDB db = new ContextDB();

        #region Barrage Data Frequency

        public List<CO_Station> GetAllBarrages()
        {
            List<CO_Station> lstBarrages = db.Repository<CO_Station>().GetAll().Where(x => x.StructureTypeID == 1 || x.StructureTypeID == 2).ToList();
            return lstBarrages;
        }

        public bool BarrageFrequencyExist(long _BarrageID, long _Frequency)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().BarrageFrequencyExist(_BarrageID, _Frequency);
        }

        public long ViewBarrageFrequency(long _BarrageID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().ViewBarrageFrequency(_BarrageID);
        }

        public bool SavaFrequency(long _BarrageID, long _FrequencyID, DateTime _date)
        {
            bool Saved = false;

            try
            {
                Saved = db.ExtRepositoryFor<DailyDataRepository>().BarrageValueExist(_BarrageID, _FrequencyID, _date);
                if (!Saved)
                {
                    CO_BarrageGaugeReadingFrequency objSave = new CO_BarrageGaugeReadingFrequency();
                    objSave.StationID = _BarrageID;

                    //if (_FrequencyID == 24)
                    //    objSave.ReadingFrequencyID = 1;
                    //else if (_FrequencyID == 8)
                    //    objSave.ReadingFrequencyID = 2;
                    //else if (_FrequencyID == 4)
                    //    objSave.ReadingFrequencyID = 3;
                    //else if (_FrequencyID == 2)
                    //    objSave.ReadingFrequencyID = 4;

                    objSave.ReadingFrequencyID = _FrequencyID;
                    objSave.FrequencyDateTime = _date;
                    objSave.ID = db.Repository<CO_BarrageGaugeReadingFrequency>().GetAll().Count();
                    db.Repository<CO_BarrageGaugeReadingFrequency>().Insert(objSave);
                    db.Save();
                    Saved = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Saved;
        }

        public bool IsValidActor(long UserID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().IsValidActor(UserID);
        }

        public List<CO_ReadingFrequency> FrequencyValues()
        {
            return db.ExtRepositoryFor<DailyDataRepository>().FrequencyValues();
        }

        #endregion

        #region Outlet Performance Data

        public object OutletInformation(long _OutletID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().OutletInformation(_OutletID);
        }

        public void SaveData(CO_ChannelOutletsPerformance _ObjSave)
        {
            try
            {
                _ObjSave.ID = db.Repository<CO_ChannelOutletsPerformance>().GetAll().Count();
                db.Repository<CO_ChannelOutletsPerformance>().Insert(_ObjSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SaveDataAlteration(SI_OutletAlterationHistroy _ObjSave)
        {
            try
            {
                db.Repository<SI_OutletAlterationHistroy>().Insert(_ObjSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public bool HasPageRights(long _UserID, string _PageName)
        {
            bool Rights = false;
            try
            {
                long? RoleID = db.ExtRepositoryFor<DailyDataRepository>().GetRoleIDOfLoggedOnUser(_UserID);
                long PageID = db.ExtRepositoryFor<DailyDataRepository>().GetPageIDByName(_PageName);
                bool? result = db.ExtRepositoryFor<DailyDataRepository>().GetUserAccess(RoleID, PageID);

                if (result == null || result == false)
                    Rights = false;
                else
                    Rights = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Rights;
        }

        #endregion

        #region Search Criteria For Outlet Performance

        public List<CO_ChannelType> GetAllChannelTypes()
        {
            List<CO_ChannelType> lstChnltypes = new List<CO_ChannelType>();
            try
            {
                lstChnltypes = db.Repository<CO_ChannelType>().GetAll().ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstChnltypes;
        }

        public List<CO_ChannelFlowType> GetAllChannelFlowTypes()
        {
            List<CO_ChannelFlowType> lstChnltypes = new List<CO_ChannelFlowType>();
            try
            {
                lstChnltypes = db.Repository<CO_ChannelFlowType>().GetAll().ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstChnltypes;
        }

        public List<CO_ChannelComndType> GetAllChannelCommands()
        {
            List<CO_ChannelComndType> lstChnltypes = new List<CO_ChannelComndType>();
            try
            {
                lstChnltypes = db.Repository<CO_ChannelComndType>().GetAll().ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstChnltypes;
        }

        public List<object> GetUserChannles(long UserID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetUserChannles(UserID);
        }

        public List<object> GetSearchResult(long _UserID, long _CommandNameID, long _ChannelTypeID, long _FlowTypeID, long _ChannelNameID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetSearchResult(_UserID, _CommandNameID, _ChannelTypeID, _FlowTypeID, _ChannelNameID);
        }


        #endregion

        #region Criteria for specific outlet

        public object ChannelParentInformation(long _ChannelID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().ChannelInformation(_ChannelID);
        }

        public List<object> OutletsDetail(long _ChannelID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().OutletsDetails(_ChannelID);
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
            List<dynamic> lstDailyGaugeSlip = db.ExtRepositoryFor<DailyDataRepository>().GetDailySlipData(_RiverID, _Date, _LoadLatest);

            return lstDailyGaugeSlip;
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
            List<dynamic> lstStations = db.ExtRepositoryFor<DailyDataRepository>().GetStationByProvinceIDAndRiverID(_ProvinceID, _RiverID, _Date, _LoadLatest, _ShowChenabOtherBarrages);

            return lstStations;
        }

        /// <summary>
        /// This function adds new gauge slip daily data.
        /// Created On 02-03-2016
        /// </summary>
        /// <param name="_GaugeSlipDailyData"></param>
        /// <returns>bool</returns>
        public bool AddGaugeSlipDailyData(CO_GaugeSlipDailyData _GaugeSlipDailyData)
        {
            db.Repository<CO_GaugeSlipDailyData>().Insert(_GaugeSlipDailyData);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates new gauge slip daily data.
        /// Created On 02-03-2016
        /// </summary>
        /// <param name="_GaugeSlipDailyData"></param>
        /// <returns>bool</returns>
        public bool UpdateGaugeSlipDailyData(CO_GaugeSlipDailyData _GaugeSlipDailyData)
        {
            //CO_GaugeSlipDailyData mdlGaugeSlipDailyData = db.Repository<CO_GaugeSlipDailyData>().FindById(_GaugeSlipDailyData.ID);

            //mdlGaugeSlipDailyData.DailyGauge = _GaugeSlipDailyData.DailyGauge;
            //mdlGaugeSlipDailyData.DailyIndent = _GaugeSlipDailyData.DailyIndent;
            //mdlGaugeSlipDailyData.DailyDischarge = _GaugeSlipDailyData.DailyDischarge;

            db.Repository<CO_GaugeSlipDailyData>().Update(_GaugeSlipDailyData);
            db.Save();

            return true;
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
            CO_GaugeSlipDailyData mdlGaugeSlipDailyData = db.Repository<CO_GaugeSlipDailyData>().GetAll().Where(gsdd => gsdd.GaugeSlipSiteID == _GaugeSlipSiteID
                && gsdd.ReadingDate == _Date).FirstOrDefault();

            return mdlGaugeSlipDailyData;
        }

        /// <summary>
        /// This function returns data for the gauge slip.
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_LoadLatest"></param>
        /// <returns>DataSet</returns>
        public DataSet GetGaugeSlipData(DateTime _Date, bool _LoadLatest = false)
        {
            return db.ExecuteStoredProcedureDataSet("DD_GetGaugeSlip", _Date, _LoadLatest);
        }

        /// <summary>
        /// This function returns data for the gauge slip dam. 
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_LoadLatest"></param>
        /// <param name="_GaugeSlipIndex"></param>
        /// <returns>DataTable</returns>
        public DataTable GetGaugeSlipDamData(DateTime _Date, int _GaugeSlipIndex = 0, bool _LoadLatest = false)
        {
            return db.ExecuteStoredProcedureDataTable("DD_GetGaugeSlipDam", _Date, _LoadLatest, _GaugeSlipIndex);
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
            return db.ExecuteStoredProcedureDataTable("DD_GetGaugeSlipOther", _Date, _LoadLatest, _GaugeSlipIndex);
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
            return db.Repository<CO_GaugeSlipDailyData>().GetAll().Where(gsdd => gsdd.GaugeSlipSiteID == _GaugeSlipSiteID && DbFunctions.TruncateTime(gsdd.ReadingDate) == DbFunctions.TruncateTime(_Date)).FirstOrDefault();
        }

        #endregion

        #region Daily Gauge Reading Data(Operational Data)
        public List<object> GetDailyGaugeReadingData(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, string _ChannelName, DateTime _Date, int _Session, int _PageIndex, int _PageSize)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetDailyGaugeReadingData(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _ChannelName, _Date, _Session, _PageIndex, _PageSize);
        }

        /// <summary>
        /// This function returns Reason for change.
        /// Created On 18-02-2016.
        /// </summary>
        /// <param name="_ZoneId"></param>
        /// <returns>List<CO_Circle>()</returns>
        public List<CO_ReasonForChange> GetReasonForChange(long _ID = -1)
        {
            List<CO_ReasonForChange> lstReason = db.Repository<CO_ReasonForChange>().GetAll().Where(c => c.ID == _ID || _ID == -1).OrderBy(c => c.Name).ToList<CO_ReasonForChange>();

            return lstReason;
        }
        public List<object> GetAuditTrail(DateTime _Date, int _Session, long _DailyGaugeReadingID, int _PageIndex, int _PageSize)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetAuditTrail(_Date, _Session, _DailyGaugeReadingID, _PageIndex, _PageSize);
        }

        public Tuple<double?, double?> GetChannelTypeMinMaxValueByChannelID(long _ChannelID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetChannelTypeMinMaxValueByChannelID(_ChannelID);
        }
        public DD_GetDailyGaugeReadingNotifyData_Result GetDailyGaugeReadingNotifyData(long _DailyGaugeReadingID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetDailyGaugeReadingNotifyData(_DailyGaugeReadingID);
        }
        public DD_GetDailyIndentNotifyData_Result GetDailyIndentNotifyData(long _GaugeID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetDailyIndentNotifyData(_GaugeID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetDailyDataNotifyRecievers(long _EventID, long _DailyGaugeReadingID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetDailyDataNotifyRecievers(_EventID, _DailyGaugeReadingID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetDailyIndentNotifyRecievers(long _EventID, long _UserID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetDailyIndentNotifyRecievers(_EventID, _UserID);
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
            return db.ExtRepositoryFor<DailyDataRepository>().GetChannelLimit(_GaugeID);
        }

        public double? CalculateDischarge(long _GaugeID, double _NewDischargeValue)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().CalculateDischarge(_GaugeID, _NewDischargeValue);
        }

        public bool UpdateDischarge(long _ID, double _Discharge, long _ReasonForChangeID, double _NewGaugeValue, long _UserID)
        {
            bool result = false;
            CO_ChannelDailyGaugeReading CurrValue = db.ExtRepositoryFor<DailyDataRepository>().UpdateDischarge(_ID);

            if (CurrValue != null)
            {
                CO_ChannelDailyGaugeHistory hist = new CO_ChannelDailyGaugeHistory();
                hist.GaugeReadingID = CurrValue.ID;
                hist.ReadingDateTime = CurrValue.ReadingDateTime;
                hist.GaugeReaderID = CurrValue.GaugeReaderID;
                hist.GaugeValue = CurrValue.GaugeValue;
                hist.DailyDischarge = CurrValue.DailyDischarge;
                hist.IsGaugeFixed = CurrValue.IsGaugeFixed;
                hist.IsGaugePainted = CurrValue.IsGaugePainted;
                hist.IsDPTableImplemented = CurrValue.IsDPTableImplemented;
                hist.GaugePhoto = null;
                hist.Remarks = CurrValue.Remarks;
                //hist.ScheduleDetailID = mdlDailyGReading.ScheduleDetailID;
                hist.Approved = CurrValue.Approved;
                hist.ApprovedByID = CurrValue.ApprovedByID;
                hist.ReasonForChangeID = CurrValue.ReasonForChangeID;
                db.Repository<CO_ChannelDailyGaugeHistory>().Insert(hist);

                if (_Discharge == 0)
                    CurrValue.ChannelClosed = true;

                CurrValue.DailyDischarge = _Discharge;
                CurrValue.GaugeValue = _NewGaugeValue;
                CurrValue.ReasonForChangeID = _ReasonForChangeID;
                // This line has been commented so that reading date time is not modified when gauge value is updated.
                //=============================================
                //CurrValue.ReadingDateTime = DateTime.Now;
                //=============================================
                CurrValue.GaugeReaderID = _UserID;
                db.Repository<CO_ChannelDailyGaugeReading>().Update(CurrValue);
                db.Save();
                result = true;
            }
            return result;
        }

        public double? GetDefaultDesignDischarge(long GaugeID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetDefaultDesignDischarge(GaugeID);
        }

        public bool HasOffTake(long _GaugeID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().HasOffTake(_GaugeID);
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
            return db.ExtRepositoryFor<DailyDataRepository>().OutletHistoryInformation(_ChannelID, _OutletID);
        }

        public List<object> GetHistory(long OutletID, DateTime? FromDate, DateTime? ToDate)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetHistory(OutletID, FromDate, ToDate);
        }

        public long GetChannelID(long _OutletID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetChannelID(_OutletID);
        }


        #endregion

        #region Daily Barrage Discharge Data

        /// <summary>
        /// this function return Daily Barrage Discharge Data
        /// Created on:03/03/2016
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetBarrageDischargeData(long _BarrageID, DateTime _Date)
        {
            return db.ExecuteStoredProcedureDataTable("GetBarrageDailyDischargeData", _BarrageID, _Date);
            //return dbADO.ExecuteBarrageDataSet("GetBarrageDailyDischargeData", 12, "2015-05-15");
            //return db.ExtRepositoryFor<DailyDataRepository>().GetBarrageDischargeData();
        }

        /// <summary>
        /// this function return all Attributes based on barrageID
        /// Created On: 29/04/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetBarrageAttribute(long _BarrageID)
        {
            return db.ExecuteStoredProcedureDataTable("GetBarrageAttribute", _BarrageID);
            //return dbADO.ExecuteBarrageDataSet("GetBarrageDailyDischargeData", 12, "2015-05-15");
            //return db.ExtRepositoryFor<DailyDataRepository>().GetBarrageDischargeData();
        }

        /// <summary>
        /// this function insert values into stored procedure
        /// Created On:6/4/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <param name="_ReadingDateTime"></param>
        /// <param name="_IDGaugeDischarge"></param>
        /// <param name="_UserID"></param>
        /// <param name="_ReasonForChange"></param>
        /// <returns>int</returns>
        public int UpdateBarrageDischargeData(long _BarrageID, string _ReadingDateTime, string _IDGaugeDischarge, long _UserID, string _ReasonForChange, string _Source)
        {
            return db.ExecuteNonQuery("EditBarrageDailyDischargeData", _BarrageID, _ReadingDateTime, _IDGaugeDischarge, _UserID, _ReasonForChange, _Source);
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
        public int AddBarrageDischargeDataAndroid(long _BarrageID, DateTime _ReadingDate, string _ReadingTime, string _IDGaugeDischarge, long _UserID, bool _GRMissed, string _Source)
        {
            return db.ExecuteNonQuery("AddBarrageDailyDischargeData", _BarrageID, _ReadingDate, _ReadingTime, _IDGaugeDischarge, _UserID, _GRMissed, _Source);
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
            return db.ExecuteNonQuery("AddBarrageDailyDischargeData", _BarrageID, _ReadingDate, _ReadingTime, _IDGaugeDischarge, _UserID, _GRMissed, _Source);
        }
        /// <summary>
        /// this function return Station By User ID
        /// Created On:08/03/2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>UA_AssociatedStations</returns>
        public UA_AssociatedStations GetStationByUserID(long _UserID)
        {
            UA_AssociatedStations mdlStation = db.Repository<UA_AssociatedStations>().GetAll().Where(x => x.UserID == _UserID && x.StractureTypeID == (long)Constants.StructureType.Barrage).FirstOrDefault();
            return mdlStation;
        }

        public CO_ChannelGauge GetDesignDischargeByChannelID(long _ChannelID)
        {
            long GaugeAtRD = db.Repository<CO_ChannelGauge>().GetAll().Where(x => x.ChannelID == _ChannelID).Min(i => (long)i.GaugeAtRD);
            CO_ChannelGauge mdlChannelGauge = db.Repository<CO_ChannelGauge>().GetAll().Where(s => s.ChannelID == _ChannelID && s.GaugeAtRD == GaugeAtRD).FirstOrDefault();
            return mdlChannelGauge;
        }

        /// <summary>
        /// this function return Barrage Gauge Reading frequency on the basis of barrage ID
        /// Created On: 20/04/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <returns>CO_BarrageGaugeReadingFrequency</returns>
        public CO_BarrageGaugeReadingFrequency GetBarrageGaugeReadingFrequency(long _BarrageID)
        {
            CO_BarrageGaugeReadingFrequency mdlBarrageGaugeReadingFrequency = db.Repository<CO_BarrageGaugeReadingFrequency>().GetAll().Where(x => x.StationID == _BarrageID).FirstOrDefault();
            return mdlBarrageGaugeReadingFrequency;
        }

        /// <summary>
        /// this function return all barrages based on punjab province
        /// Created On: 26/04/2016
        /// </summary>
        /// <returns>List<CO_Station></returns>
        public List<CO_Station> GetAllPunjabBarrages()
        {
            List<CO_Station> lstBarrages = db.Repository<CO_Station>().GetAll().Where(x => (x.StructureTypeID == 1 || x.StructureTypeID == 2) && x.ProvinceID == 1).ToList();
            return lstBarrages;
        }

        /// <summary>
        /// this function will return gauge id on the basis of attribute id
        /// Created On:26/04/2016
        /// </summary>
        /// <param name="_AttributeID"></param>
        /// <returns>CO_Attribute</returns>
        public CO_Attribute GetAttribute(long _AttributeID)
        {
            CO_Attribute mdlAttribute = db.Repository<CO_Attribute>().GetAll().Where(x => x.ID == _AttributeID).FirstOrDefault();
            return mdlAttribute;
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
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteStoredProcedureDataTable("GetBarrageDailyDischargeDataHistory", _BarrageID, _ReadingDateTime);
        }

        #endregion

        #region Gauge Info - Mobile
        public List<GetUserGauges_Result> GetUserGauges(long _UserID)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            List<GetUserGauges_Result> lstUserGauges = new List<GetUserGauges_Result>();
            lstUserGauges = repDailyData.GetUserGauges(_UserID);
            return lstUserGauges;
        }

        private CO_ChannelDailyGaugeReading CurrentDateGaugeReadingID(DateTime _Date, long _GaugeID, long _UserID)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            List<CO_ChannelDailyGaugeReading> lstGValues = repDailyData.CurrentDateGaugeReadingID(_Date, _GaugeID, _UserID);
            if (lstGValues == null || lstGValues.Count() <= 0)
                return null;
            else
                return lstGValues.ElementAt(0);

        }

        public bool isDailyGaugeRecordLocked(long _UserID)
        {
            long orgnztn_ID = db.Repository<UA_Organization>().Query().Get().Where(x => x.Name.Equals("PMIU")).SingleOrDefault().ID;
            List<long> lstDesig = db.Repository<UA_Designations>().Query().Get().Where(x => x.OrganizationID == orgnztn_ID).ToList().Select(x => x.ID).ToList();
            long? userDesig_ID = db.Repository<UA_Users>().Query().Get().Where(x => x.ID == _UserID).SingleOrDefault().DesignationID;

            if (userDesig_ID == null)
                return false;

            if (lstDesig.Contains(Convert.ToInt64(userDesig_ID)))
                return true;

            return false;

        }

        public bool areDailyGaugeMorningValuesLocked(DateTime _ReadingDate)
        {
            TimeSpan existing = _ReadingDate.TimeOfDay;
            DateTime _date = DateTime.Now;

            TimeSpan span = _date.Subtract(_ReadingDate);
            if (span.Hours >= 2)
                return true;

            int minutes = span.Minutes;
            if (minutes > 118)
                return true;

            return false;
        }

        public bool areDailyGaugeEveningValuesLocked(DateTime _ReadingDate)
        {
            TimeSpan existingTime = _ReadingDate.TimeOfDay;
            TimeSpan _time = DateTime.Now.TimeOfDay;

            DateTime _date = DateTime.Now;
            TimeSpan span = _date.Subtract(_ReadingDate);
            int minutes = span.Minutes;

            if (span.Hours >= 2)
                return true;

            if (minutes > 118)
                return true;

            return false;
        }

        private long? GetUserDesignationID(long _UserID)
        {
            return db.Repository<UA_Users>().Query().Get().Where(x => x.ID == _UserID).SingleOrDefault().DesignationID;
        }

        public CO_ChannelDailyGaugeReading updateGaugeReadingValues(long _UserID, long _GaugeID, bool _GaugeF, bool _GaugeP, double _GaugeValue,
            string _GaugeImage, string _Remarks, double? longitude, double? latitude, CO_ChannelDailyGaugeReading _GaugeValuesObj, string _Source, bool _IsChannelClosed)
        {
            CO_ChannelDailyGaugeReading _currentData = _GaugeValuesObj;
            _currentData.ReadingDateTime = DateTime.Now;
            _currentData.GaugeReaderID = _UserID;
            _currentData.DesignationID = GetUserDesignationID(_UserID);
            _currentData.GaugeValue = _GaugeValue;
            _currentData.Remarks = _Remarks;
            _currentData.GIS_X = longitude;
            _currentData.GIS_Y = latitude;
            _currentData.GRMissed = false;

            _currentData.Source = _Source;

            double? dsrg = CalculateDischarge(_GaugeID, _GaugeValue);
            if (Convert.ToDouble(dsrg) == 0 || Convert.ToDouble(dsrg) == 0.0 || dsrg == 0 || dsrg == 0.0 || dsrg == null)
            {
                _currentData.DailyDischarge = 0;
                _currentData.ChannelClosed = true;
            }
            else
            {
                _currentData.DailyDischarge = Convert.ToDouble(dsrg);
                _currentData.ChannelClosed = false;
            }

            //double? dsrg = CalculateDischarge(_GaugeID, _GaugeValue);
            //if (dsrg == null)
            //    _currentData.DailyDischarge = 0;
            //else
            //    _currentData.DailyDischarge = Convert.ToDouble(dsrg);

            _currentData.IsGaugeFixed = _GaugeF;
            _currentData.IsGaugePainted = _GaugeP;
            _currentData.GaugePhoto = _GaugeImage;
            _currentData.CreatedDate = DateTime.Today;
            _currentData.CreatedBy = _UserID;
            //_currentData.ChannelClosed = _IsChannelClosed;

            return _currentData;
        }

        public string AddUserGauges(long _UserID, long _GaugeID, bool _GaugeF, bool _GaugeP, double _GaugeValue, string _GaugeImage, string _Remarks, double? longitude, double? latitude, string _Source, bool _IsChannelClosed, DateTime? _ReadingDateTime = null)
        {
            long GaugeReadingID = 0;
            try
            {
                CO_ChannelDailyGaugeReading mdlExistingReading = CurrentDateGaugeReadingID(DateTime.Now, _GaugeID, _UserID);

                if (mdlExistingReading != null && _ReadingDateTime == null)
                {
                    Constants.SessionOrShift DataSession = Utility.GetSession(mdlExistingReading.ReadingDateTime);
                    Constants.SessionOrShift session = Utility.GetSession(DateTime.Now);
                    CO_ChannelDailyGaugeReading _currentData = mdlExistingReading;//db.Repository<CO_ChannelDailyGaugeReading>().Query().Get().Where(x => x.ID == mdlExistingReading.ID).SingleOrDefault();
                    if (session == Constants.SessionOrShift.Morning)
                    {

                        if (DataSession == Constants.SessionOrShift.Morning)
                        {
                            if (isDailyGaugeRecordLocked(mdlExistingReading.GaugeReaderID)) //edited by PMIU staff
                            {
                                return "Record has been locked.";
                            }
                            if (areDailyGaugeMorningValuesLocked(mdlExistingReading.ReadingDateTime))
                            {
                                return " Record has been locked.";
                            }



                            _currentData = updateGaugeReadingValues(_UserID, _GaugeID, _GaugeF, _GaugeP, _GaugeValue, _GaugeImage, _Remarks, longitude, latitude, _currentData, _Source, _IsChannelClosed);

                            db.Repository<CO_ChannelDailyGaugeReading>().Update(_currentData);
                            db.Save();
                            GaugeReadingID = _currentData.ID;
                        }
                    }
                    else
                    {
                        if (DataSession == Constants.SessionOrShift.Evening)
                        {
                            if (isDailyGaugeRecordLocked(mdlExistingReading.GaugeReaderID)) //edited by PMIU staff
                            {
                                return "Record has been locked.";
                            }

                            if (areDailyGaugeEveningValuesLocked(mdlExistingReading.ReadingDateTime))
                            {
                                return "Record has been locked.";
                            }
                            else
                            {
                                if (_currentData.DesignationID != (long)Constants.Designation.GaugeReader)
                                {
                                    //TODO: save audit trail
                                }

                                _currentData = updateGaugeReadingValues(_UserID, _GaugeID, _GaugeF, _GaugeP, _GaugeValue, _GaugeImage, _Remarks, longitude, latitude, _currentData, _Source, _IsChannelClosed);

                                db.Repository<CO_ChannelDailyGaugeReading>().Update(_currentData);
                                db.Save();
                                GaugeReadingID = _currentData.ID;
                            }
                        }
                        else //  simple add values for current day
                        {
                            CO_ChannelDailyGaugeReading _Data = new CO_ChannelDailyGaugeReading();

                            _Data.GaugeID = _GaugeID;
                            _Data.ChannelClosed = null;
                            _Data.ReadingDateTime = (_ReadingDateTime == null ? DateTime.Now : _ReadingDateTime.Value);
                            _Data.GaugeReaderID = _UserID;
                            _Data.GaugeValue = _GaugeValue;
                            _Data.Remarks = _Remarks;
                            _Data.GIS_X = longitude;
                            _Data.GIS_Y = latitude;
                            _Data.GRMissed = false;
                            _Data.DesignationID = GetUserDesignationID(_UserID);
                            //_Data.ChannelClosed = _IsChannelClosed;

                            double? dsrg = CalculateDischarge(_GaugeID, _GaugeValue);
                            //if (dsrg == null)
                            //    _Data.DailyDischarge = 0;
                            //else
                            //    _Data.DailyDischarge = Convert.ToDouble(dsrg);

                            if (Convert.ToDouble(dsrg) == 0 || Convert.ToDouble(dsrg) == 0.0 || dsrg == 0 || dsrg == 0.0 || dsrg == null)
                            {
                                _Data.DailyDischarge = 0;
                                _Data.ChannelClosed = true;
                            }
                            else
                            {
                                _Data.DailyDischarge = Convert.ToDouble(dsrg);
                                _Data.ChannelClosed = false;
                            }

                            _Data.IsGaugeFixed = _GaugeF;
                            _Data.IsGaugePainted = _GaugeP;
                            _Data.GaugePhoto = _GaugeImage;
                            _Data.CreatedDate = DateTime.Today;
                            _Data.CreatedBy = _UserID;
                            _Data.Source = _Source;
                            db.Repository<CO_ChannelDailyGaugeReading>().Insert(_Data);
                            db.Save();
                            IsGaugeValueAdded = true;
                            GaugeReadingID = _Data.ID;
                        }
                    }
                }
                else //  simple add values for current day
                {
                    CO_ChannelDailyGaugeReading _currentData = new CO_ChannelDailyGaugeReading();

                    _currentData.GaugeID = _GaugeID;

                    _currentData.ReadingDateTime = (_ReadingDateTime == null ? DateTime.Now : _ReadingDateTime.Value);
                    _currentData.GaugeReaderID = _UserID;
                    _currentData.GaugeValue = _GaugeValue;
                    _currentData.Remarks = _Remarks;
                    _currentData.GIS_X = longitude;
                    _currentData.GIS_Y = latitude;
                    _currentData.GRMissed = false;
                    _currentData.DesignationID = GetUserDesignationID(_UserID);
                    //_currentData.ChannelClosed = _IsChannelClosed;

                    double? dsrg = CalculateDischarge(_GaugeID, _GaugeValue);
                    if (Convert.ToDouble(dsrg) == 0 || Convert.ToDouble(dsrg) == 0.0 || dsrg == 0 || dsrg == 0.0 || dsrg == null)
                    {
                        _currentData.DailyDischarge = 0;
                        _currentData.ChannelClosed = true;
                    }
                    else
                    {
                        _currentData.DailyDischarge = Convert.ToDouble(dsrg);
                        _currentData.ChannelClosed = false;
                    }

                    //if (dsrg == null)
                    //    _currentData.DailyDischarge = 0;
                    //else
                    //    _currentData.DailyDischarge = Convert.ToDouble(dsrg);

                    _currentData.IsGaugeFixed = _GaugeF;
                    _currentData.IsGaugePainted = _GaugeP;
                    _currentData.GaugePhoto = _GaugeImage;
                    _currentData.CreatedDate = DateTime.Today;
                    _currentData.CreatedBy = _UserID;
                    _currentData.Source = _Source;
                    db.Repository<CO_ChannelDailyGaugeReading>().Insert(_currentData);
                    db.Save();
                    IsGaugeValueAdded = true;
                    GaugeReadingID = _currentData.ID;
                }

                return "SUCCESS: Record saved successfully.-" + GaugeReadingID;
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                return "FAILURE: Unknown error occured.Please try again later.";
            }
        }

        public Double? GetTailGaugeDischargeWithOffTakes(long _GaugeID, double GaugeValue)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetTailDischrge(_GaugeID, GaugeValue);
        }

        public List<GetUserGaugesStationBaised_Result> GetUserGaugesByLocation(long _UserID)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetUserGaugesByLocation(_UserID);
        }

        public List<GetUserDivisions_Result> GetUserDivisions(long _UserID)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetUserDivisions(_UserID);
        }

        public List<GetUserSubDivisions_Result> GetUserSubDivisions(long _UserID)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetUserSubDivisions(_UserID);
        }

        public List<GetUserSection_Result> GetUserSections(long _UserID)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetUserSections(_UserID);
        }

        public List<GetGauges_Result> GetUserChannelAndGauges(long _UserID)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetUserChannelAndGauges(_UserID);
        }

        public List<GetDailyGaugeReadingAndroid_Result> GetDailyGaugeReading(long _SectionID, long _ChannelID, DateTime _Date)
        {
            int _session = 0;

            Constants.SessionOrShift session = Utility.GetSession(_Date);
            if (session == Constants.SessionOrShift.Morning)
                _session = 1;

            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetDailyGaugeReading(_SectionID, _ChannelID, _Date, _session);
        }

        public long UpdateGaugeReading(long _GaugeReadingID, double _Discharge, long _ReasonForChangeID, double _NewGaugeValue, long _UserID, double? _Longitude, double? _Latitude, string _source)
        {
            // bool result = false;
            CO_ChannelDailyGaugeReading CurrValue = db.ExtRepositoryFor<DailyDataRepository>().GetGaugeReadingByID(_GaugeReadingID);

            if (CurrValue != null)
            {
                CO_ChannelDailyGaugeHistory hist = new CO_ChannelDailyGaugeHistory();
                hist.GaugeReadingID = CurrValue.ID;
                hist.ReadingDateTime = CurrValue.ReadingDateTime;
                hist.GaugeReaderID = CurrValue.GaugeReaderID;
                hist.GaugeValue = CurrValue.GaugeValue;
                hist.DailyDischarge = CurrValue.DailyDischarge;
                hist.IsGaugeFixed = CurrValue.IsGaugeFixed;
                hist.IsGaugePainted = CurrValue.IsGaugePainted;
                hist.IsDPTableImplemented = CurrValue.IsDPTableImplemented;
                hist.GaugePhoto = CurrValue.GaugePhoto;
                hist.Remarks = CurrValue.Remarks;
                //hist.ScheduleDetailID = mdlDailyGReading.ScheduleDetailID;
                hist.Approved = CurrValue.Approved;
                hist.ApprovedByID = CurrValue.ApprovedByID;
                hist.ReasonForChangeID = CurrValue.ReasonForChangeID;
                hist.GIS_X = CurrValue.GIS_X;
                hist.GIS_Y = CurrValue.GIS_Y;
                hist.GRMissed = CurrValue.GRMissed;
                db.Repository<CO_ChannelDailyGaugeHistory>().Insert(hist);

                CurrValue.DailyDischarge = _Discharge;
                CurrValue.GaugeValue = _NewGaugeValue;
                CurrValue.ReasonForChangeID = _ReasonForChangeID;
                CurrValue.ReadingDateTime = DateTime.Now;
                CurrValue.GaugeReaderID = _UserID;
                CurrValue.DesignationID = GetUserDesignationID(_UserID);
                CurrValue.GIS_X = _Longitude;
                CurrValue.GIS_Y = _Latitude;
                CurrValue.Source = _source;
                CurrValue.GRMissed = true;
                CurrValue.ModifiedBy = _UserID;
                CurrValue.ModifiedDate = DateTime.Now;
                db.Repository<CO_ChannelDailyGaugeReading>().Update(CurrValue);

                db.Save();
                //   result = true;
            }
            else
            {
                //  result = false;
                return 0;
            }
            return CurrValue.ID;
        }

        public long AddGaugeReading_SDOXEN(long _UserID, long _GaugeID, double _GaugeValue, long _ReasonForChangeID, double? _Longitude, double? _Latitude, string _source, bool _isChannelClosed)
        {
            // bool result = false;

            CO_ChannelDailyGaugeReading currentReading = new CO_ChannelDailyGaugeReading();
            currentReading.GaugeID = _GaugeID;
            currentReading.GaugeReaderID = _UserID;
            currentReading.ReadingDateTime = DateTime.Now;
            currentReading.GaugeValue = _GaugeValue;

            double? dsrg = CalculateDischarge(_GaugeID, _GaugeValue);
            if (Convert.ToDouble(dsrg) == 0 || Convert.ToDouble(dsrg) == 0.0 || dsrg == 0 || dsrg == 0.0 || dsrg == null)
            {
                currentReading.DailyDischarge = 0;
                currentReading.ChannelClosed = true;
            }
            else
            {
                currentReading.DailyDischarge = Convert.ToDouble(dsrg);
                currentReading.ChannelClosed = false;
            }

            //currentReading.DailyDischarge = Convert.ToDouble(CalculateDischarge(_GaugeID, _GaugeValue));
            currentReading.IsGaugeFixed = true;
            currentReading.IsGaugePainted = true;
            currentReading.CreatedDate = DateTime.Now;
            currentReading.CreatedBy = _UserID;
            currentReading.ReasonForChangeID = _ReasonForChangeID;
            currentReading.GIS_X = _Longitude;

            currentReading.GIS_Y = _Latitude;
            currentReading.DesignationID = GetUserDesignationID(_UserID);
            currentReading.GRMissed = true;
            currentReading.Source = _source;
            //currentReading.ChannelClosed = _isChannelClosed;
            db.Repository<CO_ChannelDailyGaugeReading>().Insert(currentReading);
            db.Save();
            //result = true;

            return currentReading.ID;
        }

        public bool AddGauge_BedLevel_DischargeMeasurements(long _UserID, long _GaugeID, double _ParamN_B, double _ParamD_H, double _ObsrvdDschrg, int _GCorrectType, double? _GCorrectValue, double? _Longitude, double? _Latitude, string _Source)
        {
            try
            {
                CO_ChannelGaugeDTPGatedStructure mdlChannelGaugeDT = new CO_ChannelGaugeDTPGatedStructure();
                mdlChannelGaugeDT.ReadingDate = DateTime.Now;
                mdlChannelGaugeDT.ExponentValue = _ParamN_B;
                mdlChannelGaugeDT.MeanDepth = _ParamD_H;
                mdlChannelGaugeDT.DischargeObserved = _ObsrvdDschrg;
                mdlChannelGaugeDT.GaugeID = _GaugeID;
                mdlChannelGaugeDT.CreatedBy = _UserID;
                mdlChannelGaugeDT.CreatedDate = DateTime.Now;
                mdlChannelGaugeDT.IsActive = true;
                mdlChannelGaugeDT.Source = _Source;
                mdlChannelGaugeDT.GIS_X = _Longitude;
                mdlChannelGaugeDT.GIS_Y = _Latitude;
                mdlChannelGaugeDT.ScheduleDetailChannelID = null;

                if (_GCorrectType != -1)
                {
                    mdlChannelGaugeDT.GaugeValueCorrection = _GCorrectValue;

                    if (_GCorrectType == 1) // Bed Sourced
                        mdlChannelGaugeDT.GaugeCorrectionType = Constants.GaugeCorrectionScouredType;
                    else // Bed Silted
                        mdlChannelGaugeDT.GaugeCorrectionType = Constants.GaugeCorrectionSiltedType;
                }

                mdlChannelGaugeDT.DischargeCoefficient = Calculations.GetBedCoefficientOfDischarge(mdlChannelGaugeDT.ExponentValue, mdlChannelGaugeDT.MeanDepth,
                    mdlChannelGaugeDT.DischargeObserved, mdlChannelGaugeDT.GaugeCorrectionType, mdlChannelGaugeDT.GaugeValueCorrection);


                db.Repository<CO_ChannelGaugeDTPGatedStructure>().Insert(mdlChannelGaugeDT);
                db.Save();



                // bool IsRecordSaved = new ChannelDAL().AddBedLevelDTParameters(mdlChannelGaugeDT);
                //if (IsRecordSaved)
                return true;

                //    return false;
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                return false;
            }
        }

        public bool AddGauge_CrestLevel_DischargeMeasurements(long _UserID, long _GaugeID, double _ParamN_B, double _ParamD_H, double _ObsrvdDschrg, double? _Longitude, double? _Latitude, string _Source)
        {
            try
            {
                CO_ChannelGaugeDTPFall mdlChannelGaugeDTFall = new CO_ChannelGaugeDTPFall();
                mdlChannelGaugeDTFall.ReadingDate = DateTime.Now;
                mdlChannelGaugeDTFall.CreatedBy = _UserID;
                mdlChannelGaugeDTFall.CreatedDate = DateTime.Now;
                mdlChannelGaugeDTFall.BreadthFall = _ParamN_B;
                mdlChannelGaugeDTFall.HeadAboveCrest = _ParamD_H;
                mdlChannelGaugeDTFall.DischargeObserved = _ObsrvdDschrg;
                mdlChannelGaugeDTFall.GaugeID = _GaugeID;
                mdlChannelGaugeDTFall.DischargeCoefficient = Calculations.GetCrestCoefficientOfDischarge((double)mdlChannelGaugeDTFall.BreadthFall,
                mdlChannelGaugeDTFall.HeadAboveCrest, mdlChannelGaugeDTFall.DischargeObserved);
                mdlChannelGaugeDTFall.Source = _Source;
                mdlChannelGaugeDTFall.GIS_X = _Longitude;
                mdlChannelGaugeDTFall.GIS_Y = _Latitude;
                mdlChannelGaugeDTFall.ScheduleDetailChannelID = null;

                bool IsRecordSaved = new ChannelDAL().AddCrestLevelDTParameters(mdlChannelGaugeDTFall);
                if (IsRecordSaved)
                    return true;

                return false;
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                return false;
            }
        }

        public bool AddOutletPerformance(long _UserID, long _OutletID, double? _HeadAboveCrest, double? _WorkingHead, double _ObsrvdDschrg, double? _Longitude, double? _Latitude, string _Source)
        {
            try
            {
                CO_ChannelOutletsPerformance mdlChnlOutletPrfrmnc = new CO_ChannelOutletsPerformance();
                mdlChnlOutletPrfrmnc.OutletID = _OutletID;
                mdlChnlOutletPrfrmnc.IsActive = true;
                mdlChnlOutletPrfrmnc.CreatedDate = DateTime.Now;
                mdlChnlOutletPrfrmnc.CreatedBy = _UserID;
                mdlChnlOutletPrfrmnc.ObservationDate = DateTime.Now;
                mdlChnlOutletPrfrmnc.HeadAboveCrest = _HeadAboveCrest;
                mdlChnlOutletPrfrmnc.WorkingHead = _WorkingHead;
                mdlChnlOutletPrfrmnc.Discharge = _ObsrvdDschrg;
                mdlChnlOutletPrfrmnc.UserID = _UserID;
                mdlChnlOutletPrfrmnc.Source = _Source;
                mdlChnlOutletPrfrmnc.GIS_X = _Longitude;
                mdlChnlOutletPrfrmnc.GIS_Y = _Latitude;
                mdlChnlOutletPrfrmnc.ScheduleDetailChannelID = null;
                bool IsRecordSaved = new ChannelDAL().AddOutletPeroformance(mdlChnlOutletPrfrmnc);
                if (IsRecordSaved)
                    return true;

                return false;
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                return false;
            }
        }

        public string AddGaugeValue_Scheduled(long _UserID, long _GaugeID, bool _GaugeF, bool _GaugeP, double _GaugeValue, string _GaugeImage, string _Remarks, double? _Longitude, double? _Latitude, long? _ScheduledID, string _DataSource, bool _isChannelClosed)
        {
            try
            {


                SI_ChannelGaugeReading mdlGaugeReading = new SI_ChannelGaugeReading();

                mdlGaugeReading.GaugeID = _GaugeID;
                //mdlGaugeReading.ChannelClosed = _isChannelClosed;
                mdlGaugeReading.ReadingDateTime = DateTime.Now;
                mdlGaugeReading.GaugeReaderID = _UserID;
                mdlGaugeReading.GaugeValue = _GaugeValue;
                mdlGaugeReading.Remarks = _Remarks;
                mdlGaugeReading.GisX = _Longitude;
                mdlGaugeReading.GisY = _Latitude;
                mdlGaugeReading.DesignationID = GetUserDesignationID(_UserID);
                mdlGaugeReading.Source = _DataSource;
                mdlGaugeReading.ScheduleDetailChannelID = _ScheduledID;

                //double? dsrg = CalculateDischarge(_GaugeID, _GaugeValue);
                //if (dsrg == null)
                //    mdlGaugeReading.DailyDischarge = 0;
                //else
                //    mdlGaugeReading.DailyDischarge = Convert.ToDouble(dsrg);

                double? dsrg = CalculateDischarge(_GaugeID, _GaugeValue);
                if (Convert.ToDouble(dsrg) == 0 || Convert.ToDouble(dsrg) == 0.0 || dsrg == 0 || dsrg == 0.0 || dsrg == null)
                {
                    mdlGaugeReading.DailyDischarge = 0;
                    mdlGaugeReading.ChannelClosed = true;
                }
                else
                {
                    mdlGaugeReading.DailyDischarge = Convert.ToDouble(dsrg);
                    mdlGaugeReading.ChannelClosed = false;
                }

                mdlGaugeReading.IsGaugeFixed = _GaugeF;
                mdlGaugeReading.IsGaugePainted = _GaugeP;
                mdlGaugeReading.GaugePhoto = _GaugeImage;
                mdlGaugeReading.CreatedDate = DateTime.Today;
                mdlGaugeReading.CreatedBy = Convert.ToInt32(_UserID);

                db.Repository<SI_ChannelGaugeReading>().Insert(mdlGaugeReading);
                db.Save();

                return "SUCCESS: Record saved successfully.";
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                return "FAILURE: Unknown error occured.Please try again later.";
            }
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
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetChannelSectionOutlets(_SectionID, _ChannelID);
        }

        public CO_Station GetBarrageByUser(long _UserID)
        {
            UA_AssociatedStations objStation = db.Repository<UA_AssociatedStations>().Query().Get().Where(x => x.UserID == _UserID && x.StractureTypeID == 1).FirstOrDefault();
            long? stationID = null;
            if (objStation != null)
                stationID = objStation.StationID;
            var mdlStation = db.Repository<CO_Station>().Query().Get().Where(x => x.ID == stationID).SingleOrDefault();
            return mdlStation;
        }

        public List<string> GetBarrageTimeStamps(long _BarrageID)
        {

            CO_BarrageGaugeReadingFrequency obj = db.Repository<CO_BarrageGaugeReadingFrequency>().Query().Get().Where(x => x.StationID == _BarrageID).SingleOrDefault();
            long? frqncyID = null;
            if (obj != null)
                frqncyID = obj.ReadingFrequencyID;
            List<string> lstTimestamps = db.Repository<CO_ReadingFrequencyTimeStamp>().Query().Get().Where(x => x.ReadingFrequencyID == frqncyID).ToList().Select(x => x.TimeStamp).ToList<string>();
            return lstTimestamps;

        }

        public List<GetBarrageDailyDischargeDataMobile_Result> GetBarrageAttributes(long _BarrageID, DateTime _Date)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetBarrageAttributes(_BarrageID, _Date);
        }

        public long? GetGaugeIDByAttributeID(long _AttributeID)
        {
            long? gaugeID = db.Repository<CO_Attribute>().Query().Get().Where(x => x.ID == _AttributeID).SingleOrDefault().GaugeID;
            return gaugeID;
        }

        public bool HasDischargeParameters(long _GaugeID)
        {

            long gaugeLevelID = db.Repository<CO_ChannelGauge>().Query().Get().Where(x => x.ID == _GaugeID).SingleOrDefault().GaugeLevelID;

            if (gaugeLevelID == 1) // bed level
            {
                List<CO_ChannelGaugeDTPGatedStructure> lst = db.Repository<CO_ChannelGaugeDTPGatedStructure>().Query().Get()
                     .Where(x => x.GaugeID == _GaugeID).ToList().OrderByDescending(x => x.ReadingDate).ToList<CO_ChannelGaugeDTPGatedStructure>();

                if (lst == null || lst.Count() <= 0)
                    return false;

                CO_ChannelGaugeDTPGatedStructure mdlDTParamStrctr = lst.ElementAt(0);

                if (mdlDTParamStrctr != null)
                {
                    if (mdlDTParamStrctr.DischargeCoefficient != null)
                    {
                        if (mdlDTParamStrctr.ExponentValue != null)
                        {
                            return true;
                        }
                    }
                }
            }
            else //crest level
            {

                List<CO_ChannelGaugeDTPFall> lst = db.Repository<CO_ChannelGaugeDTPFall>().Query().Get()
                    .Where(x => x.GaugeID == _GaugeID).ToList().OrderByDescending(x => x.ReadingDate).ToList<CO_ChannelGaugeDTPFall>();

                if (lst == null || lst.Count() <= 0)
                    return false;

                CO_ChannelGaugeDTPFall mdlDTParamFall = lst.ElementAt(0);

                if (mdlDTParamFall != null)
                {
                    if (mdlDTParamFall.DischargeCoefficient != null)
                    {
                        if (mdlDTParamFall.BreadthFall == null != null)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsRecordLocked(long _GaugeReadingID)
        {
            CO_ChannelDailyGaugeReading mdlReading = db.Repository<CO_ChannelDailyGaugeReading>().Query().Get().Where(x => x.ID == _GaugeReadingID).SingleOrDefault();
            if (mdlReading != null)
            {
                return isDailyGaugeRecordLocked(mdlReading.GaugeReaderID);
            }
            return false;
        }
        public List<object> GetDivisionsByUserID(long _UserID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[GetUserDivisions]", _UserID);
            List<object> lstClosureWorkDetails = (from DataRow dr in dt.Rows
                                                  select new
                                                  {
                                                      ZoneID = Convert.ToInt32(dr["ZoneID"]),
                                                      ZoneName = dr["ZoneName"].ToString(),
                                                      CircleID = Convert.ToInt32(dr["CircleID"]),
                                                      CircleName = dr["CircleName"].ToString(),
                                                      DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                      DivisionName = dr["DivisionName"].ToString(),
                                                      DomainID = Convert.ToInt32(dr["DomainID"]),
                                                      DomainName = dr["DomainName"].ToString(),
                                                  }).ToList<object>();
            return lstClosureWorkDetails;
        }
        #endregion

        #region Gauge Bulk Entry

        public List<DD_GetGaugesBulkEntry_Result> GetGaugesBulkData(long _SubDivisionID, long _SectionID, int _Session, DateTime _ReadingDate)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetGaugesBulkData(_SubDivisionID, _SectionID, _Session, _ReadingDate);
        }

        #endregion

        #region MeterReading And Fuel
        public List<object> GetMA_ADMUser(long _IrrigationBoundryID, long _Designations)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetMA_ADMUser(_IrrigationBoundryID, _Designations);
        }

        public List<object> GetMAUser(long _UseriD)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetMAUser(_UseriD);
        }
        public List<object> GetUser(long _UseriD)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetUser(_UseriD);
        }
        public DataSet GetMeterReadingSearch(string _ReadingType, DateTime? _FromDate, DateTime? _ToDate, long? _UserID, long? _ActivityByID)
        {
            return db.ExecuteStoredProcedureDataSet("CO_SearchMeterReadingAndFuel", _ReadingType, _FromDate, _ToDate, _UserID, _ActivityByID);
        }

        #endregion

        public int GetChannelParentID(long _ChannelID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetChannelParentID(_ChannelID);
        }

        public bool IsChannelClosedNow(long _ChannelID)
        {
            DateTime Now = DateTime.Now;

            int Count = db.Repository<CW_AnnualClosureProgramDetail>().GetAll().Where(e => e.ChannelID == _ChannelID && e.FromDate <= Now && Now <= e.ToDate).Count();
            if (Count > 0)
                return true;
            return false;
        }


        public string AddMeterReading(long _UserID, string _ReadingType, int _MeterReading, float? _PetrolQuantity, string _remarks, string _Attachment1, string _Attachment2, DateTime _MobileDate, double? _Longitude, double? _Latitude)
        {

            try
            {
                ET_VehicleReading mdlDfctDtl = new ET_VehicleReading();
                mdlDfctDtl.ReadingType = _ReadingType;
                mdlDfctDtl.MeterReading = _MeterReading;
                mdlDfctDtl.PetrolQuantity = _PetrolQuantity;
                mdlDfctDtl.ModifiedBy = _UserID;
                mdlDfctDtl.UserID = _UserID;
                mdlDfctDtl.AttachmentFile1 = _Attachment1;
                mdlDfctDtl.AttachmentFile2 = _Attachment2;
                mdlDfctDtl.ReadingMobileDate = _MobileDate;
                mdlDfctDtl.ReadingServerDate = DateTime.Now;
                mdlDfctDtl.ModificationDate = DateTime.Now;
                mdlDfctDtl.Remarks = _remarks;
                mdlDfctDtl.GisX = _Longitude;
                mdlDfctDtl.GisY = _Latitude;




                db.Repository<ET_VehicleReading>().Insert(mdlDfctDtl);
                db.Save();

                return "SUCCESS: Record saved successfully.-";
            }
            catch (Exception e)
            {
                string excetionMsg = e.Message;
                return "FAILURE: Unknown error occured.Please try again later.";
            }

        }

        #region Water theft for Monitoring


        public DataSet GetWaterTheftSearch(DateTime? _FromDate, DateTime? _ToDate, long? _UserID, string _From, long? _SelectedActivityBy)
        {
            DataSet DS = new DataSet();

            if (_From == Constants.WaterTheft)
                DS = db.ExecuteStoredProcedureDataSet("ET_GetWaterTheftSearch", _FromDate, _ToDate, _UserID,_SelectedActivityBy);
            else if (_From == Constants.CutBreach)
                DS = db.ExecuteStoredProcedureDataSet("ET_GetBreachSearch", _FromDate, _ToDate, _UserID, _SelectedActivityBy);
            else if (_From == Constants.ChannelObservation)
                DS = db.ExecuteStoredProcedureDataSet("ET_GetChannelObservationSearch", _FromDate, _ToDate, _UserID, _SelectedActivityBy);
            else if (_From == Constants.RotationalViolation)
                DS = db.ExecuteStoredProcedureDataSet("ET_GetRotationalViolationSearch", _FromDate, _ToDate, _UserID, _SelectedActivityBy);
            else if (_From == Constants.OutletChecking)
                DS = db.ExecuteStoredProcedureDataSet("ET_GetOutletCheckingSearch", _FromDate, _ToDate, _UserID, _SelectedActivityBy);
            else if (_From == Constants.Leaves)
                DS = db.ExecuteStoredProcedureDataSet("ET_GetLeavesSearch", _FromDate, _ToDate, _UserID, _SelectedActivityBy);
            else if (_From == Constants.ET_All)
                DS = db.ExecuteStoredProcedureDataSet("ET_GetEmployeeTrackingSearch", _FromDate, _ToDate, _UserID, _SelectedActivityBy);
            return DS;
        }

        public object GetRotationalDetail(long _CaseID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetRotationalDetail(_CaseID);
        }

        public object GetLeavesDetail(long _CaseID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetLeavesDetail(_CaseID);
        }

        public object GetFuelMeterDetail(long _CaseID)
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetFuelMeterDetail(_CaseID);
        }

        public string GetGISURL()
        {
            return db.ExtRepositoryFor<DailyDataRepository>().GetGISURL();
        }

        #endregion
        public List<GetChannelSectionOutlet_Result> GetChannelSectionOutlet(long _UserID)
        {
            DailyDataRepository repDailyData = db.ExtRepositoryFor<DailyDataRepository>();
            return repDailyData.GetChannelSectionOutlet(_UserID);
        }
        public List<dynamic> GetAllGaugeSlipSiteDateForDiagram(DateTime _readingDate)
        {

            try
            {
                DataTable dt = db.ExecuteStoredProcedureDataTable("DD_GetGaugeSlipDataforStationDiagrame", _readingDate);
                List<dynamic> lstGaugeSlilpSiteData = (from DataRow dr in dt.Rows
                                                       select new
                                                       {
                                                           ID = Convert.ToInt32(dr["ID"]),
                                                           Station = Convert.ToString(dr["Name"] == DBNull.Value ? "" : dr["Name"]),
                                                           DailyGauge = Convert.ToDouble(dr["DailyGauge"] == DBNull.Value ? "0" : dr["DailyGauge"]),
                                                           DailyDischarge = Convert.ToInt32(dr["DailyDischarge"] == DBNull.Value ? "0" : dr["DailyDischarge"]),
                                                           DailyIndent = Convert.ToInt32(dr["DailyIndent"] == DBNull.Value ? "0" : dr["DailyIndent"]),
                                                           AFSQ = Convert.ToInt32(dr["AFSQ"] == DBNull.Value ? "0" : dr["AFSQ"])
                                                       }).ToList<dynamic>();
                return lstGaugeSlilpSiteData;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public List<CO_GaugeSlipDailyData> GetMissedeGaugeSlipRecord(double _year, double   _month, double   _GaugeSlipSiteID)
        {

            List<CO_GaugeSlipDailyData> MissedGaugeData = db.Repository<CO_GaugeSlipDailyData>().GetAll().Where(gsdd => gsdd.GaugeSlipSiteID == _GaugeSlipSiteID && gsdd.ReadingDate.Value.Year == _year && gsdd.ReadingDate.Value.Month == _month && gsdd.DailyDischarge == null).ToList();

            return MissedGaugeData;
        }
    }
}
