using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.Channel;
using PMIU.WRMIS.DAL.Repositories.ScheduleInspection;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.ScheduleInspection
{
    public class ScheduleInspectionDAL
    {
        ContextDB db = new ContextDB();

        #region "View Schedule"
        /// <summary>
        /// This function return Schedule by ID
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>SI_Schedule</returns>
        public SI_Schedule GetSchedule(long _ID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetSchedule(_ID);
        }
        /// <summary>
        /// This function return Schedule Status by Schedule ID
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public SI_ScheduleStatus GetScheduleStatusByScheduleID(long _ID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleStatusByScheduleID(_ID);
        }
        /// <summary>
        /// This function return Gauge Inspection 
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetGaugeInspectionsByScheduleID(long _ID)
        {
            List<object> lstGaugeInspection = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGaugeInspectionsByScheduleID(_ID);
            return lstGaugeInspection;
        }
        /// <summary>
        /// This function return Outlet Alteration
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletAlterationByScheduleID(long _ID)
        {
            List<object> lstOutletAlteration = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletAlterationByScheduleID(_ID);
            return lstOutletAlteration;
        }
        /// <summary>
        /// This function return schedule works
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetWorksByScheduleID(long _ID)
        {
            List<object> lstWork = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetWorksByScheduleID(_ID);
            return lstWork;
        }
        /// <summary>
        /// This function return Outlet Performance Register
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletPerformanceRegisterByScheduleID(long _ID)
        {
            List<object> lstOutletAlteration = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletPerformanceRegisterByScheduleID(_ID);
            return lstOutletAlteration;
        }
        public List<object> GetTenderByScheduleID(long _ID)
        {
            List<object> lstTender = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetTenderByScheduleID(_ID);
            return lstTender;
        }
        /// <summary>
        /// This function return Discharge Measurement
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDischargeMeasurementByScheduleID(long _ID)
        {
            List<object> lstDischargeMeasurement = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetDischargeMeasurementByScheduleID(_ID);

            return lstDischargeMeasurement;
        }

        /// <summary>
        /// This function return Schedule by UserID
        /// Created on: 08-09-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>

        public List<SI_GetUserSchedule_Result> GetUserSchedule(long _UserID)
        {
            List<SI_GetUserSchedule_Result> lstSchedule = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUserSchedule(_UserID).ToList();
            return lstSchedule;
        }
        /// <summary>
        /// This function return Schedule Inspection Types by Schedule ID
        /// Created on: 09-09-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <returns></returns>
        public List<SI_ScheduleTypes_Result> GetScheduleInspectionTypes(long _ScheduleID)
        {
            List<SI_ScheduleTypes_Result> lstInspectionTypes = new List<SI_ScheduleTypes_Result>();
            lstInspectionTypes = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleInspectionTypes(_ScheduleID);
            return lstInspectionTypes;
        }
        /// <summary>
        /// This function return Gauges Inspection Areas List by Schedule ID
        /// Created on:  09-09-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <param name="_InspectionTypeID"></param>
        /// <returns></returns>
        public List<SI_GetGaugeInspectionAreas_Result> GetGaugesInspectionAreasRecords(long _ScheduleID, long _InspectionTypeID)
        {
            List<SI_GetGaugeInspectionAreas_Result> lstInspectionTypes = new List<SI_GetGaugeInspectionAreas_Result>();
            lstInspectionTypes = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGaugesInspectionAreasRecords(_ScheduleID, _InspectionTypeID);
            return lstInspectionTypes;
        }
        /// <summary>
        /// This function return Outlet Inspection Areas List by Schedule ID
        /// Created on: 09-09-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <param name="_InspectionTypeID"></param>
        ///  <returns></returns>
        public List<SI_GetOutletInspectionAreas_Result> GetOutletsInspectionAreasRecord(long _ScheduleID, long _InspectionTypeID)
        {
            List<SI_GetOutletInspectionAreas_Result> lstInspectionTypes = new List<SI_GetOutletInspectionAreas_Result>();
            lstInspectionTypes = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletsInspectionAreasRecord(_ScheduleID, _InspectionTypeID);
            return lstInspectionTypes;
        }
        /// <summary>
        /// This function return Outlet Information by Outlet ID
        /// Created on: 27-09-2016
        /// </summary>
        /// <param name="_OutletID"></param>
        ///  <returns></returns>
        public List<SI_GetOutletDetail_Result> GetOutletByOutletID(long _OutletID)
        {
            ScheduleInspectionRepository repSI = db.ExtRepositoryFor<ScheduleInspectionRepository>();
            return repSI.GetOutletByOutletID(_OutletID);
        }
        #endregion

        #region AddInspection
        public bool AddGauge_BedLevel_DischargeMeasurements(long? _UserID, long _GaugeID, double _ParamN_B, double _ParamD_H, double _ObsrvdDschrg, int _GCorrectType, double? _GCorrectValue, long? _ScheduleDetailChannelID, String _Remarks, double? _Longitude, double? _Latitude, string _source, string _ObservationDate)
        {
            try
            {

                DateTime dt = Convert.ToDateTime(_ObservationDate);
                DateTime observDate = dt.Add(DateTime.Now.TimeOfDay);



                CO_ChannelGaugeDTPGatedStructure mdlChannelGaugeDT = new CO_ChannelGaugeDTPGatedStructure();
                mdlChannelGaugeDT.ReadingDate = Convert.ToDateTime("01/01/1950 0:00:00");
                mdlChannelGaugeDT.ExponentValue = _ParamN_B;
                mdlChannelGaugeDT.MeanDepth = _ParamD_H;
                mdlChannelGaugeDT.DischargeObserved = _ObsrvdDschrg;
                mdlChannelGaugeDT.GaugeID = _GaugeID;
                mdlChannelGaugeDT.CreatedBy = _UserID;
                mdlChannelGaugeDT.CreatedDate = DateTime.Now;
                mdlChannelGaugeDT.IsActive = true;
                mdlChannelGaugeDT.ObservationDate = observDate;

                mdlChannelGaugeDT.ScheduleDetailChannelID = _ScheduleDetailChannelID;
                mdlChannelGaugeDT.Remarks = _Remarks;
                mdlChannelGaugeDT.Source = _source;
                mdlChannelGaugeDT.GIS_X = _Longitude;
                mdlChannelGaugeDT.GIS_Y = _Latitude;
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

                //    bool IsRecordSaved = new ChannelDAL().AddBedLevelDTParameters(mdlChannelGaugeDT);

                db.Repository<CO_ChannelGaugeDTPGatedStructure>().Insert(mdlChannelGaugeDT);
                db.Save();

                //  if (IsRecordSaved)
                return true;

                //          return false;
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                return false;
            }
        }

        public bool AddGauge_CrestLevel_DischargeMeasurements(long _UserID, long _GaugeID, double _ParamN_B, double _ParamD_H, double _ObsrvdDschrg, long? _ScheduleDetailChannelID, String _Remarks, double? _Longitude, double? _Latitude, string _source, string _ObservationDate)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(_ObservationDate);
                DateTime observDate = dt.Add(DateTime.Now.TimeOfDay);

                CO_ChannelGaugeDTPFall mdlChannelGaugeDTFall = new CO_ChannelGaugeDTPFall();
                mdlChannelGaugeDTFall.ReadingDate = Convert.ToDateTime("01/01/1950 0:00:00");
                mdlChannelGaugeDTFall.CreatedBy = _UserID;
                mdlChannelGaugeDTFall.CreatedDate = DateTime.Now;
                mdlChannelGaugeDTFall.BreadthFall = _ParamN_B;
                mdlChannelGaugeDTFall.HeadAboveCrest = _ParamD_H;
                mdlChannelGaugeDTFall.DischargeObserved = _ObsrvdDschrg;
                mdlChannelGaugeDTFall.GaugeID = _GaugeID;
                mdlChannelGaugeDTFall.DischargeCoefficient = Calculations.GetCrestCoefficientOfDischarge((double)mdlChannelGaugeDTFall.BreadthFall,
                mdlChannelGaugeDTFall.HeadAboveCrest, mdlChannelGaugeDTFall.DischargeObserved);
                mdlChannelGaugeDTFall.ScheduleDetailChannelID = _ScheduleDetailChannelID;
                mdlChannelGaugeDTFall.Remarks = _Remarks;
                mdlChannelGaugeDTFall.Source = _source;
                mdlChannelGaugeDTFall.GIS_X = _Longitude;
                mdlChannelGaugeDTFall.GIS_Y = _Latitude;
                mdlChannelGaugeDTFall.ObservationDate = observDate;
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

        public bool AddOutletAlteration(int _UserID, long _OutletID, double? _HeightOfOutlet, double? _HeadAboveCrest, double? _WorkingHead, double? _DiameterBreadthWidth, long? _ScheduleDetailChannelID, string _Remarks, int? _OutletTypeID, string _OutletStatus, DateTime alterationdt)
        {
            try
            {

                SI_OutletAlterationHistroy mdlSIOutletPrfrmnc = new SI_OutletAlterationHistroy();
                mdlSIOutletPrfrmnc.OutletID = _OutletID;

                mdlSIOutletPrfrmnc.CreatedDate = DateTime.Now;
                mdlSIOutletPrfrmnc.CreatedBy = _UserID;
                mdlSIOutletPrfrmnc.AlterationDate = alterationdt;
                mdlSIOutletPrfrmnc.OutletCrest = _HeadAboveCrest;
                mdlSIOutletPrfrmnc.OutletWorkingHead = _WorkingHead;
                mdlSIOutletPrfrmnc.OutletHeight = _HeightOfOutlet;
                mdlSIOutletPrfrmnc.OutletWidth = _DiameterBreadthWidth;
                mdlSIOutletPrfrmnc.UserID = _UserID;
                mdlSIOutletPrfrmnc.OutletID = _OutletID;
                mdlSIOutletPrfrmnc.Remarks = _Remarks;
                mdlSIOutletPrfrmnc.OutletTypeID = _OutletTypeID;
                mdlSIOutletPrfrmnc.OutletStatus = _OutletStatus;

                mdlSIOutletPrfrmnc.Source = "A";
                mdlSIOutletPrfrmnc.ScheduleDetailChannelID = _ScheduleDetailChannelID;

                db.Repository<SI_OutletAlterationHistroy>().Insert(mdlSIOutletPrfrmnc);
                db.Save();


                return true;
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                return false;
            }
        }
        public bool AddOutletPerformance(long _UserID, long _OutletID, double? _HeadAboveCrest, double? _WorkingHead, double? _ObsrvdDschrg, double? _DiameterBreadthWidth, double? _HeightOfOutlet, string _Remarks, long? _SchID, double? _Longitude, double? _Latitude, string _Source, DateTime outletperfdt)
        {
            try
            {
                CO_ChannelOutletsPerformance mdlChnlOutletPrfrmnc = new CO_ChannelOutletsPerformance();
                mdlChnlOutletPrfrmnc.OutletID = _OutletID;
                mdlChnlOutletPrfrmnc.IsActive = true;
                mdlChnlOutletPrfrmnc.CreatedDate = DateTime.Now;
                mdlChnlOutletPrfrmnc.CreatedBy = _UserID;
                mdlChnlOutletPrfrmnc.ObservationDate = outletperfdt;
                mdlChnlOutletPrfrmnc.HeadAboveCrest = _HeadAboveCrest;
                mdlChnlOutletPrfrmnc.WorkingHead = _WorkingHead;
                mdlChnlOutletPrfrmnc.ObservedWidthB = _DiameterBreadthWidth;
                mdlChnlOutletPrfrmnc.ObservedHeightY = _HeightOfOutlet;
                mdlChnlOutletPrfrmnc.Discharge = _ObsrvdDschrg;
                mdlChnlOutletPrfrmnc.UserID = _UserID;
                mdlChnlOutletPrfrmnc.Source = _Source;
                mdlChnlOutletPrfrmnc.Remarks = _Remarks;
                mdlChnlOutletPrfrmnc.GIS_X = _Longitude;
                mdlChnlOutletPrfrmnc.GIS_Y = _Latitude;

                mdlChnlOutletPrfrmnc.ScheduleDetailChannelID = _SchID;
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




        //public List<CO_Division> getAllDivisions()
        //{
        //    List<CO_Division> listdiv = db.Repository<CO_Division>().GetAll().ToList();
        //    return listdiv;
        //}

        //public List<CO_Circle> getAllCircles()
        //{
        //    List<CO_Circle> listCir = db.Repository<CO_Circle>().GetAll().OrderBy(c => c.Name).ToList();
        //    return listCir;
        //}

        //public List<CO_Zone> getAllZones()
        //{
        //    List<CO_Zone> listCir = db.Repository<CO_Zone>().GetAll().OrderBy(c => c.Name).ToList();
        //    return listCir;
        //}

        //public List<UA_Office> getAllOffices()
        //{
        //    List<UA_Office> listOfc = db.Repository<UA_Office>().GetAll().OrderBy(c => c.Name).ToList();
        //    return listOfc;
        //}


        //public List<object> getChannelAndHeadWorks(int DvsnID)
        //{
        //    List<object> lstChnlHedWork = db.ExtRepositoryFor<ScheduleInspectionRepository>().getChnlAndHeadWrkForDvns(DvsnID);
        //    return lstChnlHedWork;
        //}


        //public List<CO_ChannelGauge> getInspectionForChannel(int ChnlID)
        //{
        //    List<CO_ChannelGauge> lstChnlInsp = db.ExtRepositoryFor<ScheduleInspectionRepository>().getInspectionAreaForChannel(ChnlID);
        //    return lstChnlInsp;
        //}


        //public List<CO_ChannelOutlets> getInspectionForHeadWork(int headWrkID)
        //{
        //    List<CO_ChannelOutlets> lstChnlInsp = db.ExtRepositoryFor<ScheduleInspectionRepository>().getInspectionAreaForHeadWorks(headWrkID);
        //    return lstChnlInsp;
        //}


        //public List<object> getChannelForDivision(int DvsnID)
        //{
        //    List<object> lstChannels = db.ExtRepositoryFor<ScheduleInspectionRepository>().getChannelsForDivisions(DvsnID);
        //    return lstChannels;
        //}

        #endregion

        //#region DeleteInspection

        //public bool DeleteGuageOutletPerformanceDischarge(int ID)
        //{
        //    return db.ExtRepositoryFor<ScheduleInspectionRepository>().DeleteGuageOutletPerformanceDischarge(ID);
        //}

        //public bool DeleteWork(int WorkID)
        //{
        //    return db.ExtRepositoryFor<ScheduleInspectionRepository>().DeleteWork(WorkID);
        //}

        //public bool DeleteTender(int TenderID)
        //{
        //    return db.ExtRepositoryFor<ScheduleInspectionRepository>().DeleteTender(TenderID);
        //}

        //#endregion

        //#region EditSchedule

        //public List<object> GetGuageAndDischargeRecord(int ScheduleID, int Guage_Discharge)
        //{
        //    List<object> lstGageDischrg = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGuageAndDischargeRecords(ScheduleID, Guage_Discharge).ToList();
        //    return lstGageDischrg;
        //}

        //public List<object> GetOutletAndPerformanceRecord(int ScheduleID, int Altration_Performance)
        //{
        //    List<object> lstOutltPerf = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletAndPerformanceRecords(ScheduleID, Altration_Performance).ToList();
        //    return lstOutltPerf;
        //}


        //public List<object> GetWorkRecord(int ScheduleID)
        //{
        //    return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetWorksRecords(ScheduleID).ToList();
        //}

        //public SI_Schedule GetScheduleDetail(int ScheduleID)
        //{
        //    return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetail(ScheduleID);
        //}

        //public bool AllowEditToUser(int ScheduleID, int UserID)
        //{
        //    return db.ExtRepositoryFor<ScheduleInspectionRepository>().EditAllowedToUser(ScheduleID, UserID);
        //}

        //public List<object> GetComments(int ScheduleID)
        //{
        //    return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetComments(ScheduleID);
        //}


        //#endregion

        #region Search Schedule

        public List<SI_ScheduleStatus> GetScehduleInspectionStatuses()
        {

            List<SI_ScheduleStatus> StatusList = db.Repository<SI_ScheduleStatus>().GetAll().ToList();

            return StatusList;

        }


        public List<dynamic> GetSchedulesBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _StatusID, bool _ApprovedCheck, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstDivs, List<long> _lstSubDivs)
        {

            List<long> CircleIDs = new List<long>();
            List<long> DivIDs = new List<long>();
            List<long> SubDivIDs = new List<long>();


            List<long> OverAlSubordinates = new List<long>();
            List<long> TotalSubs = new List<long>();
            if (_DesignationID == (long)Constants.Designation.ADM || _DesignationID == (long)Constants.Designation.XEN)
            {
                var subOrdinates = db.Repository<UA_UserManager>().GetAll().Where(x => x.ManagerID == _UserID).Select(x => x.UserID).ToList();
                OverAlSubordinates.AddRange(subOrdinates);
            }
            else if (_DesignationID == (long)Constants.Designation.DeputyDirector || _DesignationID == (long)Constants.Designation.SE)
            {
                var subOrdinates = db.Repository<UA_UserManager>().GetAll().Where(x => x.ManagerID == _UserID).Select(x => x.UserID).ToList();
                OverAlSubordinates.AddRange(subOrdinates);
                foreach (var item in subOrdinates)
                {
                    var subOrdinatesSDOADM = db.Repository<UA_UserManager>().GetAll().Where(x => x.ManagerID == item).Select(x => x.UserID).ToList();
                    OverAlSubordinates.AddRange(subOrdinatesSDOADM);
                }
            }
            else if (_DesignationID == (long)Constants.Designation.DirectorGauges)
            {

                var subOrdinates = db.Repository<UA_UserManager>().GetAll().Where(x => x.UserID == _UserID).Select(x => x.UserID).ToList();
                OverAlSubordinates.AddRange(subOrdinates);
                //foreach (var item in subOrdinates)
                //{
                //    var subOrdinatesSDOADM = db.Repository<UA_UserManager>().GetAll().Where(x => x.ManagerID == item).Select(x => x.UserID).ToList();
                //    OverAlSubordinates.AddRange(subOrdinatesSDOADM);
                //}
            }
            if (OverAlSubordinates.Count > 0)
            {
                TotalSubs = OverAlSubordinates.ConvertAll(x => (long)x);
            }

            List<long> lstDivsIDs = new List<long>();
            List<long> lstSubDivIDs = new List<long>();

            if (_lstDivs != null)
                lstDivsIDs.AddRange(_lstDivs);

            if (_lstSubDivs != null)
                lstSubDivIDs.AddRange(_lstSubDivs);

            // location wise 
            //if (_CircleID == -1 || _DivisionID == -1)
            //{
            //    if (lstDivsIDs.Count() == 0)
            //        lstDivsIDs.Add(-1);

            //    if (lstSubDivIDs.Count() == 0)
            //        lstSubDivIDs.Add(-1);
            //}

            if (_DivisionID != -1)
            {
                lstDivsIDs.Clear();
                lstDivsIDs.Add(_DivisionID);
            }
            else
            {
                if (_lstDivs == null || _lstDivs.Count() == 0)
                {
                    lstDivsIDs.Clear();
                    lstDivsIDs.Add(-1);
                }
            }

            if (_SubDivisionID != -1)
            {
                lstSubDivIDs.Clear();
                lstSubDivIDs.Add(_DivisionID);
            }
            else
            {
                if (_lstSubDivs == null || _lstSubDivs.Count() == 0)
                {
                    lstSubDivIDs.Clear();
                    lstSubDivIDs.Add(-1);
                }
            }
            //

            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetSchedulesBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _StatusID, _ApprovedCheck, _FromDate, _ToDate, _UserID, _DesignationID, TotalSubs, lstDivsIDs, lstSubDivIDs);
        }


        #endregion

        #region Add Schedule

        //public bool IsInspectionDateBetweenScheduleDates(DateTime InspectionDate)
        //{

        //}
        public int SaveSchedule(SI_Schedule _ObjSave, int UserID, int UserDesID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().SaveSchedule(_ObjSave, UserID, UserDesID);
        }

        public bool UpdateSchedule(SI_Schedule Schedule)
        {
            bool IsUpdated = false;
            try
            {
                db.Repository<SI_Schedule>().Update(Schedule);
                db.Save();
                IsUpdated = true;
            }
            catch (Exception)
            {

                throw;
            }
            return IsUpdated;
        }

        public SI_Schedule GetScheduleBasicInformation(long _ID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleBasicInformation(_ID);
        }

        public bool CheckScheduleDetailDates(long _ScheduleID, DateTime _FromDate, DateTime _ToDate)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().CheckScheduleDetailDates(_ScheduleID, _FromDate, _ToDate);
        }

        public bool CheckInspectionDatesCheck(long _ScheduleID, DateTime _InspectionDate)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().CheckInspectionDatesCheck(_ScheduleID, _InspectionDate);
        }


        public bool IsScheduleDependencyExists(long _ScheduleID)
        {
            bool isDependanceExists = false;

            bool isChannelScheduleExists = db.Repository<SI_ScheduleDetailChannel>().GetAll().Any(i => i.ScheduleID == _ScheduleID);

            bool isTenderExists = db.Repository<SI_ScheduleDetailTender>().GetAll().Any(i => i.ScheduleID == _ScheduleID);

            bool isGeneralExists = db.Repository<SI_ScheduleDetailWorks>().GetAll().Any(i => i.ScheduleID == _ScheduleID);

            bool isWorkExists = db.Repository<SI_ScheduleDetailGeneral>().GetAll().Any(i => i.ScheduleID == _ScheduleID);

            if (isChannelScheduleExists == true || isTenderExists == true || isWorkExists == true || isGeneralExists == true)
                isDependanceExists = true;

            return isDependanceExists;
        }

        public bool DeleteSchedule(long _ScheduleID)
        {
            //db.Repository<SI_ScheduleDetailChannel>().GetAll().Where(x => x.ScheduleID == _ScheduleID).ToList().Clear();
            //db.Repository<SI_ScheduleDetailTender>().GetAll().Where(x => x.ScheduleID == _ScheduleID).ToList().Clear();
            //db.Repository<SI_ScheduleDetailWorks>().GetAll().Where(x => x.ScheduleID == _ScheduleID).ToList().Clear();
            //db.Repository<SI_ScheduleDetailGeneral>().GetAll().Where(x => x.ScheduleID == _ScheduleID).ToList().Clear();

            //db.Repository<SI_ScheduleStatusDetail>().GetAll().Where(x => x.ScheduleID == _ScheduleID).ToList().Clear();
            //db.Repository<SI_Schedule>().Delete(_ScheduleID);

            //db.Save();
            //return true;

            return db.ExtRepositoryFor<ScheduleInspectionRepository>().DeleteSchedule(_ScheduleID);
        }

        #endregion

        #region Schedule Detail

        public List<object> GetGaugeInspectionArea(long _ChannelID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGaugeInspectionArea(_ChannelID);
        }

        public List<object> GetOutletsAgainstChannel(long _ChannelID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletsAgainstChannel(_ChannelID);
        }
        public List<object> GetScheduleDetailByScheduleIDInspectionTypeID(long _ScheduleID, long _InspectionTypeID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetailByScheduleIDInspectionTypeID(_ScheduleID, _InspectionTypeID);
        }

        public List<object> GetScheduleDetailForTenderMonitoring(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetailForTenderMonitoring(_ScheduleID);
        }

        public List<object> GetScheduleDetailForClosureOperations(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetailForClosureOperations(_ScheduleID);
        }
        public List<object> GetScheduleDetailForGeneralInspections(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetailForGeneralInspections(_ScheduleID);
        }
        public List<TM_TenderNotice> GetTenderNoticesByDivisionID(long _DivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            List<TM_TenderNotice> lstTenderNotices = db.Repository<TM_TenderNotice>().GetAll().Where(s => s.DivisionID == _DivisionID && DbFunctions.TruncateTime(s.BidOpeningDate) >= DbFunctions.TruncateTime(_FromDate) && DbFunctions.TruncateTime(s.BidOpeningDate) <= DbFunctions.TruncateTime(_ToDate)).OrderBy(s => s.Name).ToList<TM_TenderNotice>();
            return lstTenderNotices;
        }
        public List<TM_TenderNotice> GetTenderNoticesByDivisionID(long _DivisionID)
        {
            List<TM_TenderNotice> lstTenderNotices = db.Repository<TM_TenderNotice>().GetAll().Where(s => s.DivisionID == _DivisionID).OrderBy(s => s.Name).ToList<TM_TenderNotice>();
            return lstTenderNotices;
        }
        public List<SI_GeneralInspectionType> GetGeneralInspectionTypes()
        {
            List<SI_GeneralInspectionType> lstInspectionTypes = db.Repository<SI_GeneralInspectionType>().GetAll().Where(x => x.IsActive == true).ToList<SI_GeneralInspectionType>();
            return lstInspectionTypes;
        }
        public List<object> GetTenderWorksByTenderNoticeID(long _TenderNoticeID)
        {
            //List<TM_TenderWorks> lstTenderWorks = db.Repository<TM_TenderWorks>().GetAll().Where(s => s.TenderNoticeID == _TenderNoticeID).OrderBy(s => s.ID).ToList<TM_TenderWorks>();
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetTenderWorksByTenderNoticeID(_TenderNoticeID);
        }

        public List<object> GetClosureWorksByWorkSourceAndDivisionID(string _WorkSource, long _DivisionID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetClosureWorksByWorkSourceAndDivisionID(_WorkSource, _DivisionID);
        }
        public DateTime GetTenderNoticeOpeningDateBytenderNoticeID(long _TenderNoticeID)
        {
            DateTime OpeningDate = db.Repository<TM_TenderNotice>().GetAll().Where(s => s.ID == _TenderNoticeID).Select(x => x.BidOpeningDate).FirstOrDefault();
            return OpeningDate;
        }
        public SI_ScheduleDetailChannel GetSelectedRecordDetail(long _ID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetSelectedRecordDetail(_ID);
        }

        public List<object> GetOutletExistingRecords(long _ScheduleID, long _InspectionType)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletExistingRecords(_ScheduleID, _InspectionType);
        }

        public bool SaveGaugeInspectionScheduleDetail(SI_ScheduleDetailChannel _ScheduleDetailChannel)
        {
            bool Result = false;
            try
            {
                if (_ScheduleDetailChannel.ID == 0)
                    db.Repository<SI_ScheduleDetailChannel>().Insert(_ScheduleDetailChannel);
                else
                    db.Repository<SI_ScheduleDetailChannel>().Update(_ScheduleDetailChannel);



                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public bool SaveGaugeInspectionScheduleDetail(List<SI_ScheduleDetailChannel> _lstScheduleDetailChannel)
        {
            bool Result = false;
            try
            {
                if (_lstScheduleDetailChannel.Count > 0)
                    db.ExtRepositoryFor<ScheduleInspectionRepository>().SavelstInspectionScheduleDetail(_lstScheduleDetailChannel);


                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public bool DeleteGaugeRecord(long _ID)
        {
            bool Result = false;
            try
            {
                db.Repository<SI_ScheduleDetailChannel>().Delete(_ID);
                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public bool UpdateDetail(SI_ScheduleDetailChannel _ObjSave)
        {
            bool Result = false;
            try
            {
                db.Repository<SI_ScheduleDetailChannel>().Update(_ObjSave);
                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public bool IsGaugeInspectionNotesExist(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().IsGaugeInspectionNotesExist(_ScheduleID);
        }

        public bool IsOutletAltrationInspNotesExist(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().IsOutletAltrationInspNotesExist(_ScheduleID);
        }

        public bool IsOutletPerformanceInspNotesExist(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().IsOutletPerformanceInspNotesExist(_ScheduleID);
        }

        public bool IsDTParameterInspectionNotesExist(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().IsDTParameterInspectionNotesExist(_ScheduleID);
        }


        #endregion

        #region Action On Schedule

        /// <summary>
        /// This function fetches the Schedule data.
        /// Created By 15-07-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <returns>dynamic</returns>
        public dynamic GetScheduleData(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleData(_ScheduleID);
        }

        /// <summary>
        /// This function returns Schedule Status for a particular ScheduleID.
        /// Created By 19-07-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <returns>List<SI_ScheduleStatusDetail></returns>
        public List<SI_ScheduleStatusDetail> GetScheduleStatuses(long _ScheduleID)
        {
            return db.Repository<SI_ScheduleStatusDetail>().GetAll().Where(ss => ss.ScheduleID == _ScheduleID && ss.CommentsDate != null).OrderByDescending(ss => ss.CommentsDate).ToList();
        }

        /// <summary>
        /// This function adds new schedule status detail.
        /// Created On 22-07-2016.
        /// </summary>
        /// <param name="_ScheduleStatusDetail"></param>
        /// <returns>bool</returns>
        public bool AddScheduleStatusDetail(SI_ScheduleStatusDetail _ScheduleStatusDetail)
        {
            db.Repository<SI_ScheduleStatusDetail>().Insert(_ScheduleStatusDetail);
            db.Save();

            return true;
        }


        public dynamic GetScheduleDataForGeneralInspections(long _ScheduleDetailID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDataForGeneralInspection(_ScheduleDetailID);
        }
        #endregion

        #region Outlet Inspection
        public double GetOutletDischarge(long _OutletID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletDischarge(_OutletID);
        }
        public dynamic GetOutletInspection(long _ScheduleDetailChannelID, long? _InspectionTypeID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletInspection(_ScheduleDetailChannelID, _InspectionTypeID);
        }
        public bool AddOutletAlterationInspectionNotes(SI_OutletAlterationHistroy _OutletAlterationHistory)
        {
            db.Repository<SI_OutletAlterationHistroy>().Insert(_OutletAlterationHistory);
            db.Save();
            return true;
        }
        public bool AddOutletPerformanceInspection(CO_ChannelOutletsPerformance _OutletPerformance)
        {
            db.Repository<CO_ChannelOutletsPerformance>().Insert(_OutletPerformance);
            db.Save();
            return true;
        }
        public SI_OutletAlterationHistroy GetOutletAlterationInspection(long _ScheduleDetailChannleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletAlterationInspection(_ScheduleDetailChannleID);
        }
        public CO_ChannelOutletsPerformance GetOutletsPerformanceInspection(long _ScheduleDetailChannleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletsPerformanceInspection(_ScheduleDetailChannleID);
        }

        public CO_ChannelOutletsPerformance GetOutletsPerformanceInspectionByID(long _OutletPerformanceID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletPerformanceByID(_OutletPerformanceID);
        }

        public SI_OutletAlterationHistroy GetOutletsAlterationInspectionByID(long _OutletAlterationID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletAlterationInspoctionData(_OutletAlterationID);
        }
        #endregion

        #region Inspection Notes

        public dynamic GetScheduleDetailData(long _ScheduleDetailID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetailData(_ScheduleDetailID);
        }

        public dynamic GetScheduleDetailDataBL(long _ScheduleDetailID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetailDataAlterationBL(_ScheduleDetailID);
        }

        public dynamic GetScheduleDetailDataCL(long _ScheduleDetailID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetailDataAlterationCL(_ScheduleDetailID);
        }

        public dynamic GetGaugeLocation(long _GaugeID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGaugeLocation(_GaugeID);
        }

        public List<dynamic> GetComplaintsForGaugeInspection(long _ModuleID, long _RefCodeID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetComplaintsForGaugeInspection(_ModuleID, _RefCodeID);
        }
        public dynamic GetTailStatus(long _GaugeReadingID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetTailStatus(_GaugeReadingID);
        }
        public long SaveGaugeReading(SI_ChannelGaugeReading GaugeReading)
        {
            long GaugeReadingID = 0;
            try
            {
                //   GaugeReading.ChannelClosed = null;
                //GaugeReading.ReadingDateTime = DateTime.Now;
                GaugeReading.IsDPTableImplemented = null;
                //GaugeReading.GisX = null;
                // GaugeReading.GisY = null;
                //GaugeReading.Source = Configuration.RequestSource.RequestFromWeb;
                GaugeReading.ModifiedBy = null;
                GaugeReading.ModifiedDate = null;
                db.Repository<SI_ChannelGaugeReading>().Insert(GaugeReading);
                db.Save();
                GaugeReadingID = GaugeReading.ID;
            }
            catch (Exception)
            {

                throw;
            }

            return GaugeReadingID;



        }

        public bool IsGaugeRecordAlreadyExists(long _ScheduleDetailChannelID, int _GaugeID)
        {
            bool duplicate = db.Repository<SI_ChannelGaugeReading>().GetAll().Any(x => x.GaugeID == _GaugeID && x.ScheduleDetailChannelID == _ScheduleDetailChannelID);
            return duplicate;

        }

        public bool IsOutletAlterationRecordAlreadyExists(long _ScheduleDetailChannelID, int _OutletID)
        {
            bool duplicate = db.Repository<SI_OutletAlterationHistroy>().GetAll().Any(x => x.OutletID == _OutletID && x.ScheduleDetailChannelID == _ScheduleDetailChannelID);
            return duplicate;

        }

        public bool IsOutletPerformanceRecordAlreadyExists(long _ScheduleDetailChannelID, int _OutletID)
        {
            bool duplicate = db.Repository<CO_ChannelOutletsPerformance>().GetAll().Any(x => x.OutletID == _OutletID && x.ScheduleDetailChannelID == _ScheduleDetailChannelID);
            return duplicate;

        }


        public bool IsDischargeCalcBLRecordAlreadyExists(long _ScheduleDetailID, int _GaugeID)
        {
            bool duplicate = db.Repository<CO_ChannelGaugeDTPGatedStructure>().GetAll().Any(x => x.GaugeID == _GaugeID && x.ScheduleDetailChannelID == _ScheduleDetailID);
            return duplicate;

        }
        public bool IsDischargeCalcCLRecordAlreadyExists(long _ScheduleDetailID, int _GaugeID)
        {
            bool duplicate = db.Repository<CO_ChannelGaugeDTPFall>().GetAll().Any(x => x.GaugeID == _GaugeID && x.ScheduleDetailChannelID == _ScheduleDetailID);
            return duplicate;

        }

        public dynamic GetGaugeRecordbyID(int _GaugeID, long _ScheduleDetailChannelID)
        {
            return db.Repository<SI_ChannelGaugeReading>().GetAll().Where(x => x.GaugeID == _GaugeID && x.ScheduleDetailChannelID == _ScheduleDetailChannelID).FirstOrDefault();
        }

        public dynamic GetChannelGaugeRecordbyID(int ChannelGaugeID)
        {
            return db.Repository<SI_ChannelGaugeReading>().GetAll().Where(x => x.ID == ChannelGaugeID).FirstOrDefault();
        }

        public dynamic GetDischargeCalcBLDatabyID(int _GaugeID, long _ScheduleDetailID)
        {
            return db.Repository<CO_ChannelGaugeDTPGatedStructure>().GetAll().Where(x => x.GaugeID == _GaugeID && x.ScheduleDetailChannelID == _ScheduleDetailID).FirstOrDefault();
        }

        public dynamic GetDischargeCalcBLDatabyDischargeID(int DischargeID)
        {
            return db.Repository<CO_ChannelGaugeDTPGatedStructure>().GetAll().Where(x => x.ID == DischargeID).FirstOrDefault();
        }
        public dynamic GetDischargeCalcCLDatabyID(int _GaugeID, long _ScheduleDetailID)
        {
            return db.Repository<CO_ChannelGaugeDTPFall>().GetAll().Where(x => x.GaugeID == _GaugeID && x.ScheduleDetailChannelID == _ScheduleDetailID).FirstOrDefault();
        }

        public dynamic GetDischargeCalcCLDatabyDischargeID(int DischargeID)
        {
            return db.Repository<CO_ChannelGaugeDTPFall>().GetAll().Where(x => x.ID == DischargeID).FirstOrDefault();
        }
        public List<string> GetUploadedNames(int _GaugeID, long _GaugeReadingID)
        {
            return db.Repository<SI_ChannelGaugeReading>().GetAll().Where(x => x.GaugeID == _GaugeID && x.ID == _GaugeReadingID).Select(x => x.GaugePhoto).ToList();

        }

        public List<string> GetUploadedNamesForGeneralInspections(long _ID)
        {
            return db.Repository<SI_GeneralInspectionsAttachment>().GetAll().Where(x => x.GeneralInspectionsID == _ID).Select(x => x.Attachment).ToList();

        }
        public int GetDTSLevel(long GaugeID)
        {
            //return db.Repository<co>;
            return 1;

        }

        public long SaveGeneralInspection(SI_GeneralInspections _mdlGeneralInspection)
        {
            db.Repository<SI_GeneralInspections>().Insert(_mdlGeneralInspection);
            db.Save();
            return _mdlGeneralInspection.ID;
        }
        public void SaveGeneralInspectionAttachemnts(SI_GeneralInspectionsAttachment _mdlGeneralInspectionAttachemnt)
        {
            db.Repository<SI_GeneralInspectionsAttachment>().Insert(_mdlGeneralInspectionAttachemnt);
            db.Save();

        }

        #endregion


        public object GetOutletAlterationHistoryDetail(long _ScheduleChnlDetailID, long _OutletID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletAlterationHistoryDetail(_ScheduleChnlDetailID, _OutletID);
        }
        public object GetOutletAlterationPerformanceDetail(long _ScheduleChnlDetailID, long _OutletID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletAlterationPerformanceDetail(_ScheduleChnlDetailID, _OutletID);
        }

        #region Inspection Search

        public bool IsScheduleInspectionsExists(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().IsScheduleInspectionsExists(_ScheduleID);
        }

        public List<dynamic> GetGaugeInspectionBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGaugeInspectionBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetGaugeChannelNameByGaugeReadingID(long _GaugeReadingID)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUncsheduledChannelAreaByGaugereadingID(_GaugeReadingID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetGaugeChannelNameByDischargeID(long _DischargeID)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUncsheduledChannelAreaByDischargeTableBLID(_DischargeID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetGaugeChannelNameByOutletPerformanceID(long _OutletPerformanceID)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUncsheduledChannelAreaByOutletPerformanceID(_OutletPerformanceID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetGaugeChannelNameByOutletAlterationID(long _OutletAlterationID)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUncsheduledChannelAreaByOutletAlterationID(_OutletAlterationID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetGaugeChannelNameByDischargeCLID(long _DischargeID)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUncsheduledChannelAreaByDischargeTableCLID(_DischargeID);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetDischargeTableBedLevelBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetDischargeTableBedLevelBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetDischargeTableCrestLevelBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetDischargeTableCrestLevelBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetOutletPerformanceBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletPerformanceBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetOutletCheckingBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletCheckingBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetOutletAlterationHistoryBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletAlterationHistoryBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetTenderMonitoringBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetTenderMonitoringBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetClosureOperationsBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetClosureOperationsBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetGeneralInspectionsBySearchCriteria(

 long _InspectionCategory
, string _FromDate
, string _ToDate
, long _UserID
, long? _DesignationID)
        {
            try
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGeneralInspectionsBySearchCriteria(_InspectionCategory, _FromDate, _ToDate, _UserID, _DesignationID);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion


        /// <summary>
        /// This method return List of Scheduled Inspections by Month Specified
        /// Created On 19-10-2016.
        /// </summary>
        /// <param name="_MonthParam"></param>
        /// <param name="_UserParam"></param>
        /// <returns>List<object></returns>
        public List<object> GetScheduledInspectionsByMonth(string _MonthParam, string _UserParam)
        {
            DataSet dt = db.ExecuteStoredProcedureDataSet("[SI_GetScheduledInspectionsByMonth]", _MonthParam, _UserParam);
            List<object> lstScheduledInspections = (from DataRow dr in dt.Tables[0].Rows
                                                    select new
                                                    {
                                                        ID = Convert.ToInt32(dr["ID"]),
                                                        ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                                                        Date = dr["Date"].ToString()
                                                    }).ToList<object>();
            return lstScheduledInspections;
        }
        /// <summary>
        /// This method return List of Scheduled Inspections by Date Specified
        /// Created On 24-10-2016.
        /// </summary>
        /// <param name="_DateParam"></param>
        /// <param name="_UserParam"></param>
        /// <returns>List<object></returns>
        public List<object> GetScheduledInspectionsByDate(string _DateParam, string _UserParam)
        {
            string dateFormat = "MM.dd.yyyy";
            string _date = DateTime.Now.ToString(dateFormat);

            DateTime paramDate = DateTime.ParseExact(_DateParam, dateFormat, null);
            DateTime serverDate = DateTime.ParseExact(_date, dateFormat, null);
            int dateCompare = -2;
            if (paramDate < serverDate)
            {
                dateCompare = -1;
            }
            else if (paramDate > serverDate)
            {
                dateCompare = 1;
            }
            else if (paramDate == serverDate)
            {
                dateCompare = 0;
            }
            //   dateCompare = 1;
            DataSet dt = db.ExecuteStoredProcedureDataSet("[SI_GetScheduledInspectionsByDate]", _DateParam, _UserParam);
            List<object> lstScheduledInspections1 = (from DataRow dr in dt.Tables[0].Rows
                                                     select new
                                                     {
                                                         ID = Convert.ToInt32(dr["ID"]),
                                                         ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                                                         ScheduleName = dr["ScheduleName"].ToString(),
                                                         InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                         InspectionName = dr["InspectionName"].ToString(),
                                                         StatusName = dr["StatusName"].ToString(),
                                                         DivisionID = Convert.ToInt64(dr["DivisionID"].ToString()),
                                                         DivName = dr["DivName"].ToString(),
                                                         SubDivName = dr["SubDivName"].ToString(),
                                                         ChannelName = dr["ChannelName"].ToString(),
                                                         ChannelID = Convert.ToInt32(dr["ChannelID"].ToString()),
                                                         Date = dr["Date"].ToString(),
                                                         Remarks = dr["Remarks"].ToString(),
                                                         InspectionRD = dr["InspectionRD"].ToString(),
                                                         GaugeID = Convert.ToInt32(dr["GaugeID"]),
                                                         GaugeLevel = Convert.ToInt32(dr["GaugeLevel"]),
                                                         GaugeInspectionRecordAlreadyExists = dr["GaugeInspectionRecordAlreadyExists"].ToString(),
                                                         ChannelDTPRecordAlreadyExists = dr["ChannelDTPRecordAlreadyExists"].ToString(),
                                                         DateCheck = dateCompare

                                                     }).ToList<object>();
            List<object> lstScheduledInspections2 = (from DataRow dr in dt.Tables[1].Rows
                                                     select new
                                                     {
                                                         ID = Convert.ToInt32(dr["ID"]),
                                                         ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                                                         ScheduleName = dr["ScheduleName"].ToString(),
                                                         InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                         InspectionName = dr["InspectionName"].ToString(),
                                                         StatusName = dr["StatusName"].ToString(),
                                                         DivisionID = Convert.ToInt64(dr["DivisionID"].ToString()),
                                                         DivName = dr["DivName"].ToString(),
                                                         SubDivName = dr["SubDivName"].ToString(),
                                                         ChannelName = dr["ChannelName"].ToString(),
                                                         ChannelID = Convert.ToInt32(dr["ChannelID"].ToString()),
                                                         Date = dr["Date"].ToString(),
                                                         Remarks = dr["Remarks"].ToString(),
                                                         InspectionRD = dr["InspectionRD"].ToString(),
                                                         OutletID = Convert.ToInt32(dr["OutletID"]),
                                                         OutletAlterationRecordAlreadyExists = dr["OutletAlterationRecordAlreadyExists"].ToString(),
                                                         OutletPerformanceRecordAlreadyExists = dr["OutletPerformanceRecordAlreadyExists"].ToString(),
                                                         OutletCheckingRecordAlreadyExists = dr["OutletCheckingRecordAlreadyExists"].ToString(),
                                                         DateCheck = dateCompare,
                                                         ChannelSide = (dr["ChannelSide"] == DBNull.Value) ? "" : dr["ChannelSide"],
                                                         OutletType = (dr["OutletType"] == DBNull.Value) ? "" : dr["OutletType"],
                                                         OutletTypeID = (dr["OutletTypeID"] == DBNull.Value) ? "" : dr["OutletTypeID"],
                                                         DesignDischarge = (dr["DesignDischarge"] == DBNull.Value) ? "" : dr["DesignDischarge"],
                                                         OutletStatus = (dr["OutletStatus"] == DBNull.Value) ? "" : dr["OutletStatus"]

                                                     }).ToList<object>();
            List<object> lstScheduledInspections3 = (from DataRow dr in dt.Tables[2].Rows
                                                     select new
                                                     {
                                                         ID = Convert.ToInt32(dr["ID"]),
                                                         ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                                                         ScheduleName = dr["ScheduleName"].ToString(),
                                                         InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                         InspectionName = dr["InspectionName"].ToString(),
                                                         StatusName = dr["StatusName"].ToString(),
                                                         DivName = dr["DivName"].ToString(),
                                                         Date = dr["Date"].ToString(),
                                                         Remarks = dr["Remarks"].ToString(),
                                                         TenderNoticeID = Convert.ToInt64(dr["TenderNoticeID"]),
                                                         TenderWorkID = Convert.ToInt64(dr["TenderWorkID"]),
                                                         WorkStatus = dr["WorkStatus"].ToString(),
                                                         WorkName = dr["WorkName"].ToString(),
                                                         DivisionID = Convert.ToInt64(dr["DivisionID"]),
                                                         WorkTypeID = Convert.ToInt32(dr["WorkTypeID"]),
                                                         WorkTypeName = dr["WorkTypeName"].ToString(),
                                                         DateCheck = dateCompare
                                                     }).ToList<object>();
            List<object> lstScheduledInspections4 = (from DataRow dr in dt.Tables[3].Rows
                                                     select new
                                                     {
                                                         ID = Convert.ToInt32(dr["ID"]),
                                                         ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                                                         ScheduleName = dr["ScheduleName"].ToString(),
                                                         InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                         InspectionName = dr["InspectionName"].ToString(),
                                                         StatusName = dr["StatusName"].ToString(),
                                                         DivName = dr["DivName"].ToString(),
                                                         Date = dr["Date"].ToString(),
                                                         Remarks = dr["Remarks"].ToString(),
                                                         TenderNoticeID = Convert.ToInt64(dr["TenderNoticeID"]),
                                                         TenderWorkID = Convert.ToInt64(dr["TenderWorkID"]),
                                                         WorkStatus = dr["WorkStatus"].ToString(),
                                                         WorkName = dr["WorkName"].ToString(),
                                                         DivisionID = Convert.ToInt64(dr["DivisionID"]),
                                                         WorkTypeID = Convert.ToInt32(dr["WorkTypeID"]),
                                                         WorkTypeName = dr["WorkTypeName"].ToString(),
                                                         DateCheck = dateCompare
                                                     }).ToList<object>();
            List<object> lstScheduledInspections5 = (from DataRow dr in dt.Tables[4].Rows
                                                     select new
                                                     {
                                                         ID = Convert.ToInt32(dr["ID"]),
                                                         ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                                                         ScheduleName = dr["ScheduleName"].ToString(),
                                                         InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                         InspectionName = dr["InspectionName"].ToString(),
                                                         StatusName = dr["StatusName"].ToString(),
                                                         DivName = dr["DivName"].ToString(),
                                                         Date = dr["Date"].ToString(),
                                                         Remarks = dr["Remarks"].ToString(),
                                                         ClosureWorkID = Convert.ToInt64(dr["ClosureWorkID"]),
                                                         WorkTypeID = Convert.ToInt32(dr["WorkTypeID"]),
                                                         WorkTypeName = dr["WorkTypeName"].ToString(),
                                                         WorkName = dr["WorkName"].ToString(),
                                                         DivisionID = Convert.ToInt64(dr["DivisionID"]),
                                                         WorkProgressID = (dr["WorkProgressID"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["WorkProgressID"]),
                                                         DateCheck = dateCompare
                                                     }).ToList<object>();
            List<object> lstScheduledInspections6 = (from DataRow dr in dt.Tables[5].Rows
                                                     select new
                                                     {
                                                         ID = Convert.ToInt32(dr["ID"]),
                                                         ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                                                         ScheduleName = dr["ScheduleName"].ToString(),
                                                         InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                         InspectionName = dr["InspectionName"].ToString(),
                                                         StatusName = dr["StatusName"].ToString(),
                                                         DivName = dr["DivName"].ToString(),
                                                         Date = dr["Date"].ToString(),
                                                         Remarks = dr["Remarks"].ToString(),
                                                         AssetWorkID = Convert.ToInt64(dr["AssetWorkID"]),
                                                         WorkTypeID = Convert.ToInt32(dr["WorkTypeID"]),
                                                         WorkTypeName = dr["WorkTypeName"].ToString(),
                                                         WorkName = dr["WorkName"].ToString(),
                                                         DivisionID = Convert.ToInt64(dr["DivisionID"]),
                                                         WorkProgressID = (dr["WorkProgressID"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["WorkProgressID"]),
                                                         DateCheck = dateCompare
                                                     }).ToList<object>();
            List<object> lstScheduledInspections7 = (from DataRow dr in dt.Tables[6].Rows
                                                     select new
                                                     {
                                                         ID = Convert.ToInt32(dr["ID"]),
                                                         ScheduleID = Convert.ToInt32(dr["ScheduleID"]),
                                                         ScheduleName = dr["ScheduleName"].ToString(),
                                                         InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                         InspectionName = dr["InspectionName"].ToString(),
                                                         StatusName = dr["StatusName"].ToString(),
                                                         Location = dr["Location"].ToString(),
                                                         Date = dr["Date"].ToString(),
                                                         Remarks = dr["Remarks"].ToString(),
                                                         GeneralInspectionTypeID = Convert.ToInt64(dr["GeneralInspectionTypeID"]),
                                                         GeneralInspectionAlreadyExists = dr["GeneralInspectionAlreadyExists"].ToString(),
                                                         GeneralInspectionTypeName = dr["GeneralInspectionTypeName"].ToString(),
                                                         DateCheck = dateCompare

                                                     }).ToList<object>();

            List<object> lstScheduledInspections = lstScheduledInspections1.Concat(lstScheduledInspections2).Concat(lstScheduledInspections3).Concat(lstScheduledInspections4).Concat(lstScheduledInspections5).Concat(lstScheduledInspections6).Concat(lstScheduledInspections7).ToList<object>();

            return lstScheduledInspections;
        }

        #region Schedule Calendar
        public List<object> GetUserInspectionDates(long _UserID, long _Month, long _Year)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUserInspectionDates(_UserID, _Month, _Year);
        }
        public List<dynamic> GetUserInspectionsByDate(long _UserID, DateTime _Date)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUserInspectionsByDate(_UserID, _Date);
        }
        public List<dynamic> GetTenderInspectionsByDate(long _UserID, DateTime _Date)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetTenderInspectionsByDate(_UserID, _Date);
        }
        public List<dynamic> GetClosureInspectionsByDate(long _UserID, DateTime _Date)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetClosureInspectionsByDate(_UserID, _Date);
        }
        public List<dynamic> GetGeneralInspectionsByDate(long _UserID, DateTime _Date)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGeneralInspectionsByDate(_UserID, _Date);
        }
        #endregion
        public long? GetUserDesignationID(long _UserID)
        {
            return db.Repository<UA_Users>().Query().Get().Where(x => x.ID == _UserID).SingleOrDefault().DesignationID;
        }
        public long AddGaugeValue(long _UserID, long _GaugeID, bool _GaugeF, bool _GaugeP, double? _GaugeValue, string _GaugeImage, string _Remarks, double? _Longitude, double? _Latitude, long? _SchChnlDetID, long? _DesignationID, double? _dsrg, string _DataSource)
        {
            try
            {
                SI_ChannelGaugeReading mdlGaugeReading = new SI_ChannelGaugeReading();

                mdlGaugeReading.GaugeID = _GaugeID;
                mdlGaugeReading.ChannelClosed = null;
                mdlGaugeReading.ReadingDateTime = DateTime.Now;
                mdlGaugeReading.GaugeReaderID = _UserID;
                mdlGaugeReading.GaugeValue = _GaugeValue;
                mdlGaugeReading.Remarks = _Remarks;
                mdlGaugeReading.GisX = _Longitude;
                mdlGaugeReading.GisY = _Latitude;
                mdlGaugeReading.DesignationID = _DesignationID;
                mdlGaugeReading.Source = _DataSource;
                mdlGaugeReading.ScheduleDetailChannelID = _SchChnlDetID;

                double? dsrg = _dsrg;
                if (dsrg == null)
                    mdlGaugeReading.DailyDischarge = 0;
                else
                    mdlGaugeReading.DailyDischarge = Convert.ToDouble(dsrg);

                mdlGaugeReading.IsGaugeFixed = _GaugeF;
                mdlGaugeReading.IsGaugePainted = _GaugeP;
                mdlGaugeReading.GaugePhoto = _GaugeImage;
                mdlGaugeReading.CreatedDate = DateTime.Today;
                mdlGaugeReading.CreatedBy = Convert.ToInt32(_UserID);

                db.Repository<SI_ChannelGaugeReading>().Insert(mdlGaugeReading);
                db.Save();


                return mdlGaugeReading.ID;
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                return -1;
            }
        }
        public long GetDivisionIDByGaugeID(long _GaugeID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetDivisionIDByGaugeID(_GaugeID);
        }
        public SI_GetScheduleInspectionNotifyData_Result GetScheduleInspectionNotifyData(long _ScheduleID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleInspectionNotifyData(_ScheduleID);
        }
        public SI_GetDischargeTableCalcBLNotifyData_Result GetDischargeTableCalcBLNotifyData(long _ScheduleDetailsID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetDischargeTableCalcBLNotifyData(_ScheduleDetailsID);
        }
        public bool DeleteSIGaugeReading(long ID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().DeleteSIGaugeReading(ID);
        }
        public bool IsGaugeReadingExists(long ID)
        {
            bool duplicate = db.Repository<SI_ChannelGaugeReading>().GetAll().Any(x => x.ID == ID);
            return duplicate;

        }

        public long GetScheduleIDByScheduleDetailGeneralID(long _ID)
        {
            return db.Repository<SI_ScheduleDetailGeneral>().GetAll().Where(x => x.ID == _ID).Select(x => x.ScheduleID).FirstOrDefault();
        }

        /// <summary>
        /// This function gets Channel Gauge by ID.
        /// Created On 16-11-2016.
        /// </summary>
        /// <param name="_ChannelGaugeID"></param>
        /// <returns>ChannelData</returns>
        public object GetChannelByGaugeID(long _GaugeID)
        {
            object ChannelData = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetChannelNameAndRD(_GaugeID);

            return ChannelData;
        }

        public object GetInspectionDetailByTypeID(int inspectionType, int _GaugeID, long _ScheduleDetailChannelID, string serverUploadFolder)
        {
            if (inspectionType == 1)
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGaugeInsepctionDetail(_GaugeID, _ScheduleDetailChannelID, serverUploadFolder);
            }
            else if (inspectionType == 2)
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetDTPFallInsepctionDetail(_GaugeID, _ScheduleDetailChannelID);
            }
            else if (inspectionType == 3)
            {
                return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetDTPGatedStructureInsepctionDetail(_GaugeID, _ScheduleDetailChannelID);
            }
            else
            {
                return null;
            }
        }

        public SI_Schedule GetScheduleDetailByScheduleDetailID(long _ScheduleDetailChannelID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetScheduleDetailByScheduleDetailID(_ScheduleDetailChannelID);
        }

        public bool IsGaugeInspectionAlreadyExists(long _ScheduleDetailChannelID)
        {
            bool duplicate = db.Repository<SI_ChannelGaugeReading>().GetAll().Any(x => x.ScheduleDetailChannelID == _ScheduleDetailChannelID);
            return duplicate;

        }
        public bool IsDischargetblBedLevelInspectionAlreadyExists(long _ScheduleDetailChannelID)
        {
            bool duplicate = db.Repository<CO_ChannelGaugeDTPGatedStructure>().GetAll().Any(x => x.ScheduleDetailChannelID == _ScheduleDetailChannelID);
            return duplicate;

        }
        public bool IsDischargetblCrestLevelInspectionAlreadyExists(long _ScheduleDetailChannelID)
        {
            bool duplicate = db.Repository<CO_ChannelGaugeDTPFall>().GetAll().Any(x => x.ScheduleDetailChannelID == _ScheduleDetailChannelID);
            return duplicate;

        }
        public bool SaveTenderScheduleDetail(SI_ScheduleDetailTender _ScheduleDetailTender)
        {
            bool Result = false;
            try
            {
                if (_ScheduleDetailTender.ID == 0)
                    db.Repository<SI_ScheduleDetailTender>().Insert(_ScheduleDetailTender);
                else
                    db.Repository<SI_ScheduleDetailTender>().Update(_ScheduleDetailTender);


                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public bool SaveWorkScheduleDetail(SI_ScheduleDetailWorks _ScheduleDetailWork)
        {
            bool Result = false;
            try
            {
                if (_ScheduleDetailWork.ID == 0)
                    db.Repository<SI_ScheduleDetailWorks>().Insert(_ScheduleDetailWork);
                else
                    db.Repository<SI_ScheduleDetailWorks>().Update(_ScheduleDetailWork);


                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }
        public bool SaveGeneralScheduleDetail(SI_ScheduleDetailGeneral _ScheduleDetailGeneral)
        {
            bool Result = false;
            try
            {
                if (_ScheduleDetailGeneral.ID == 0)
                    db.Repository<SI_ScheduleDetailGeneral>().Insert(_ScheduleDetailGeneral);
                else
                    db.Repository<SI_ScheduleDetailGeneral>().Update(_ScheduleDetailGeneral);


                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }
        public bool IsADMRecordExists(long _TenderWorkID)
        {
            bool IsExists = false;
            string ADMObservations = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _TenderWorkID).Select(x => x.ADM_Observation).FirstOrDefault();
            if (!string.IsNullOrEmpty(ADMObservations))
            {
                IsExists = true;
            }
            return IsExists;
        }
        public bool IsGeneralInspectionAdded(long _ScheduleDetailID)
        {
            bool IsExists = false;
            long ID = db.Repository<SI_GeneralInspections>().GetAll().Where(x => x.ScheduleDetailGeneralID == _ScheduleDetailID).Select(x => x.ID).FirstOrDefault();
            if (ID > 0)
            {
                IsExists = true;
            }
            return IsExists;
        }
        public bool IsClosureProgressExists(long _ScheduleDetailID)
        {
            bool IsExists = false;
            long? RefMonitoringID = db.Repository<SI_ScheduleDetailWorks>().GetAll().Where(x => x.ID == _ScheduleDetailID).Select(x => x.RefMonitoringID).FirstOrDefault();
            if (RefMonitoringID != null)
            {
                IsExists = true;
            }
            return IsExists;
        }
        public bool DeleteTenderRecord(long _ID)
        {
            bool Result = false;
            try
            {
                db.Repository<SI_ScheduleDetailTender>().Delete(_ID);
                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }
        public bool DeleteClosureWorkRecord(long _ID)
        {
            bool Result = false;
            try
            {
                db.Repository<SI_ScheduleDetailWorks>().Delete(_ID);
                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public bool DeleteGeneralInspection(long _ID)
        {
            bool Result = false;
            try
            {
                db.Repository<SI_ScheduleDetailGeneral>().Delete(_ID);
                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public SI_GeneralInspections GetGeneralInspectionsByID(long _ID)
        {
            return db.Repository<SI_GeneralInspections>().GetAll().Where(x => x.ID == _ID).FirstOrDefault();
        }
        public bool IsScheduleDetailTenderExists(long _TenderNoticeID, long _TenderWorkID, long _ScheduleID)
        {
            bool IsExists = false;
            try
            {
                long ID = db.Repository<SI_ScheduleDetailTender>().GetAll().Where(x => x.TenderNoticeID == _TenderNoticeID && x.ScheduleID == _ScheduleID && x.TenderWorksID == _TenderWorkID).Select(x => x.ID).FirstOrDefault();
                if (ID > 0)
                {
                    IsExists = true;
                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return IsExists;
        }
        public bool IsScheduleDetailWorkExists(long _WorkSourceID, string _WorkSource, long _ScheduleID)
        {
            bool IsExists = false;
            try
            {
                long ID = db.Repository<SI_ScheduleDetailWorks>().GetAll().Where(x => x.WorkSourceID == _WorkSourceID && x.ScheduleID == _ScheduleID && x.WorkSource.ToUpper() == _WorkSource.ToUpper()).Select(x => x.ID).FirstOrDefault();
                if (ID > 0)
                {
                    IsExists = true;
                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return IsExists;
        }

        public bool IsScheduleDetailGeneralInspectionExists(long _ScheduleID, long _TypeID, string _Location)
        {
            bool IsExists = false;
            try
            {
                long ID = db.Repository<SI_ScheduleDetailGeneral>().GetAll().Where(x => x.GeneralInspectionTypeID == _TypeID && x.ScheduleID == _ScheduleID && x.Location.ToUpper() == _Location.ToUpper()).Select(x => x.ID).FirstOrDefault();
                if (ID > 0)
                {
                    IsExists = true;
                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return IsExists;
        }

        public List<SI_GeneralInspectionType> GetAllGeneralInspectionTypes()
        {
            return db.Repository<SI_GeneralInspectionType>().GetAll().ToList<SI_GeneralInspectionType>();
        }

        public SI_GeneralInspectionType GetGeneralInspectionTypeByName(string _Name)
        {
            return db.Repository<SI_GeneralInspectionType>().GetAll().Where(x => x.Name.Trim().ToUpper() == _Name.Trim().ToUpper() && x.IsActive == true).FirstOrDefault();
        }

        public bool AddgeneralInspectionType(SI_GeneralInspectionType _mdlGeneralInspectionType)
        {
            bool IsSaved = true;
            try
            {
                if (_mdlGeneralInspectionType.ID == 0)
                {
                    db.Repository<SI_GeneralInspectionType>().Insert(_mdlGeneralInspectionType);
                }
                else
                {
                    SI_GeneralInspectionType mdlGeneralInspectionType = db.Repository<SI_GeneralInspectionType>().FindById(_mdlGeneralInspectionType.ID);
                    mdlGeneralInspectionType.Name = _mdlGeneralInspectionType.Name;
                    mdlGeneralInspectionType.Description = _mdlGeneralInspectionType.Description;
                    mdlGeneralInspectionType.CreatedBy = _mdlGeneralInspectionType.CreatedBy;
                    mdlGeneralInspectionType.CreatedDate = _mdlGeneralInspectionType.CreatedDate;
                    mdlGeneralInspectionType.IsActive = _mdlGeneralInspectionType.IsActive;
                    mdlGeneralInspectionType.ModifiedBy = _mdlGeneralInspectionType.ModifiedBy;
                    mdlGeneralInspectionType.ModifiedDate = _mdlGeneralInspectionType.ModifiedDate;

                }
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;

            }
            return IsSaved;
        }

        public bool IsgeneralInspectionAssociated(long _ID)
        {
            return db.Repository<SI_ScheduleDetailGeneral>().GetAll().Any(x => x.GeneralInspectionTypeID == _ID);
        }

        public bool DeleteGeneralInspectionType(long _ID)
        {
            bool IsDeleted = true;
            try
            {
                db.Repository<SI_GeneralInspectionType>().Delete(_ID);
                db.Save();
            }
            catch (Exception)
            {
                IsDeleted = false;
                throw;

            }
            return IsDeleted;
        }
        public List<object> GetGeneralInspectionTypeList()
        {
            List<object> lstInspectionTypes = db.Repository<SI_GeneralInspectionType>().GetAll().Where(s => s.IsActive == true).Select(x => new { x.ID, x.Name, x.Description }).ToList<object>();
            return lstInspectionTypes;
        }
        public object GetGeneralInspectionByID(long _ScheduleIDParam, long _TypeParam)
        {
            DataSet dt = db.ExecuteStoredProcedureDataSet("[SI_GetGeneralInspectionByID]", _ScheduleIDParam, _TypeParam);
            object GeneralInspection = (from DataRow dr in dt.Tables[0].Rows
                                        select new
                                        {
                                            ID = Convert.ToInt32(dr["ID"]),
                                            ScheduleDetailGeneralID = Convert.ToInt32(dr["ScheduleDetailGeneralID"]),
                                            InspectionLocation = dr["InspectionLocation"].ToString(),
                                            GeneralInspectionTypeID = Convert.ToInt32(dr["GeneralInspectionTypeID"]),
                                            InspectionDetails = dr["InspectionDetails"].ToString(),
                                            Date = dr["Date"].ToString(),
                                            Remarks = dr["Remarks"].ToString(),
                                            AttachmentList = GetGeneralInspectionAttachment(Convert.ToInt32(dr["ID"]))
                                        }).FirstOrDefault();
            return GeneralInspection;
        }

        public List<object> GetGeneralInspectionAttachment(long _InspectionID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGeneralInspectionAttachment(_InspectionID);
        }

        public bool IsWorksInspectionExists(long _ScheduleID)
        {
            return db.Repository<SI_ScheduleDetailWorks>().GetAll().Where(x => x.ScheduleID == _ScheduleID).Any(x => x.RefMonitoringID != null);
        }

        public bool IsGeneralInspectionExists(long _ScheduleID)
        {
            List<long?> SDID = db.Repository<SI_ScheduleDetailChannel>().GetAll().Where(x => x.ScheduleID == _ScheduleID).Select(x => (long?)x.ID).ToList();
            return db.Repository<SI_GeneralInspections>().GetAll().Any(x => SDID.Contains(x.ScheduleDetailGeneralID));

        }
        public double? GetGaugeDesignDischargeByID(long _GaugeID)
        {
            return db.Repository<CO_ChannelGauge>().FindById(_GaugeID).DesignDischarge;
        }
        public bool IsDuplicateInspectionAreaExist(ref List<SI_ScheduleDetailChannel> lstScheduleDetailChannel)
        {
            bool isDuplicateFind = false;
            List<SI_ScheduleDetailChannel> lstSDC = new List<SI_ScheduleDetailChannel>();
            //lstSDC = lstScheduleDetailChannel;
            foreach (SI_ScheduleDetailChannel item in lstScheduleDetailChannel)
            {
                SI_ScheduleDetailChannel FindItem;
                if (item.InspectionTypeID == (long)Constants.SIInspectionType.GaugeReading || item.InspectionTypeID == (long)Constants.SIInspectionType.DischargeTableCalculation)
                {
                    //FindItem = (from chn in db.Repository<SI_ScheduleDetailChannel>().GetAll() where chn.DivisionID == item.DivisionID && chn.SubDivID == item.SubDivID && chn.ChannelID == item.ChannelID && chn.InspectionTypeID == item.InspectionTypeID && chn.ScheduleDate == item.ScheduleDate && chn.GaugeID == item.GaugeID select chn).FirstOrDefault();
                    FindItem = (from chn in db.Repository<SI_ScheduleDetailChannel>().GetAll() where chn.DivisionID == item.DivisionID && chn.ChannelID == item.ChannelID && chn.InspectionTypeID == item.InspectionTypeID && chn.ScheduleDate == item.ScheduleDate && chn.ScheduleID == item.ScheduleID && chn.GaugeID == item.GaugeID select chn).FirstOrDefault();

                }
                else
                {
                    FindItem = (from chn in db.Repository<SI_ScheduleDetailChannel>().GetAll() where chn.DivisionID == item.DivisionID && chn.ChannelID == item.ChannelID && chn.InspectionTypeID == item.InspectionTypeID && chn.ScheduleDate == item.ScheduleDate && chn.ScheduleID == item.ScheduleID && chn.OutletID == item.OutletID select chn).FirstOrDefault();
                }
                if (FindItem != null && FindItem.ID > 0)
                {
                    isDuplicateFind = false;
                    lstSDC.Add(item);
                    //break;
                }
            }
            foreach (SI_ScheduleDetailChannel item in lstSDC)
            {
                lstScheduleDetailChannel.Remove(item);
            }
            return isDuplicateFind;
        }
        public bool IsDuplicateInspectionAreaExist(SI_ScheduleDetailChannel item)
        {
            bool isDuplicateFind = false;
            SI_ScheduleDetailChannel FindItem;

            // if (item.InspectionTypeID == 1 || item.InspectionTypeID == 2)

            if (item.InspectionTypeID == (long)Constants.SIInspectionType.GaugeReading || item.InspectionTypeID == (long)Constants.SIInspectionType.DischargeTableCalculation)
            {
                // FindItem = (from chn in db.Repository<SI_ScheduleDetailChannel>().GetAll() where chn.DivisionID == item.DivisionID && chn.SubDivID == item.SubDivID && chn.ChannelID == item.ChannelID && chn.InspectionTypeID == item.InspectionTypeID && chn.ScheduleDate == item.ScheduleDate && chn.GaugeID == item.GaugeID && chn.ID != item.ID select chn).FirstOrDefault();
                FindItem = (from chn in db.Repository<SI_ScheduleDetailChannel>().GetAll() where chn.DivisionID == item.DivisionID && chn.ChannelID == item.ChannelID && chn.InspectionTypeID == item.InspectionTypeID && chn.ScheduleDate == item.ScheduleDate && chn.ScheduleID == item.ScheduleID && chn.GaugeID == item.GaugeID && chn.ID != item.ID select chn).FirstOrDefault();
            }
            else
            {
                //FindItem = (from chn in db.Repository<SI_ScheduleDetailChannel>().GetAll() where chn.DivisionID == item.DivisionID && chn.SubDivID == item.SubDivID && chn.ChannelID == item.ChannelID && chn.InspectionTypeID == item.InspectionTypeID && chn.ScheduleDate == item.ScheduleDate && chn.OutletID == item.OutletID && chn.ID != item.ID select chn).FirstOrDefault();
                FindItem = (from chn in db.Repository<SI_ScheduleDetailChannel>().GetAll() where chn.DivisionID == item.DivisionID && chn.ChannelID == item.ChannelID && chn.InspectionTypeID == item.InspectionTypeID && chn.ScheduleDate == item.ScheduleDate && chn.ScheduleID == item.ScheduleID && chn.OutletID == item.OutletID && chn.ID != item.ID select chn).FirstOrDefault();
            }
            if (FindItem != null && FindItem.ID > 0)
                isDuplicateFind = true;
            return isDuplicateFind;
        }
        public SI_GetDischargeTableCalcBLNotifyDataAndroid_Result GetDischargeTableCalcBLNotifyDataAndroid(long? _GaugeID, string _CalcType)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetDischargeTableCalcBLNotifyDataAndroid(_GaugeID, _CalcType);
        }


        public List<CO_Zone> GetUserZones(List<long> _lstZones)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUserZones(_lstZones);
        }

        public List<CO_Circle> GetUserCircles(List<long> _lstCircles)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUserCircles(_lstCircles);
        }

        public List<CO_Division> GetUserDivision(List<long> _lstDivision)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUserDivisions(_lstDivision);
        }

        public List<CO_SubDivision> GetUserSubDivision(List<long> _lstSubDivision)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetUserSubDivisions(_lstSubDivision);
        }

        public List<object> GetLeaveTypeList()
        {
            List<object> lstLeaveTypes = db.Repository<ET_LeaveTypes>().GetAll().Where(s => s.IsActive == true).Select(x => new { x.ID, x.Name, x.Description }).ToList<object>();
            return lstLeaveTypes;
        }

        public string AddLeaveForm(int _UserID, double? _GaugeValue, string _ImagePath, string _Remarks, double? _Longitude, double? _Latitude, DateTime mobdt, DateTime leavedate, short _LeaveTypeID)
        {

            try
            {
                ET_Leaves mdlDfctDtl = new ET_Leaves();
                mdlDfctDtl.LeaveTypeID = _LeaveTypeID;
                mdlDfctDtl.LeaveDate = leavedate;
                mdlDfctDtl.RainGauge = _GaugeValue;
                mdlDfctDtl.CreatedBy = _UserID;
                mdlDfctDtl.Attachment = _ImagePath;
                mdlDfctDtl.GisX = _Longitude;
                mdlDfctDtl.GisY = _Latitude;
                mdlDfctDtl.MobileEntryDatetime = mobdt;
                mdlDfctDtl.CreatedDate = DateTime.Now;
                mdlDfctDtl.Remarks = _Remarks;

                db.Repository<ET_Leaves>().Insert(mdlDfctDtl);
                db.Save();

                return "SUCCESS: Record saved successfully.-";
            }
            catch (Exception e)
            {
                string excetionMsg = e.Message;
                return "FAILURE: Unknown error occured.Please try again later.";
            }

        }

        public object GeOutletCheckingByScheduleID(long _ScheduleChnlDetailID, long _OutletID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GeOutletCheckingByScheduleID(_ScheduleChnlDetailID, _OutletID);
        }
        public List<object> GetOutletCheckingAttachment(long _OCID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletCheckingAttachment(_OCID);
        }

        public string AddRotationalViolations(int _UserID, double? _GaugeValue, string _ImagePath, string _Remarks, double? _Longitude, double? _Latitude, DateTime mobdt, long _ChannelID, bool? _isViolation, short _Grouppref)
        {

            try
            {
                ET_RotationalViolation mdlDfctDtl = new ET_RotationalViolation();
                mdlDfctDtl.ChannelID = _ChannelID;
                mdlDfctDtl.HeadGaugeValue = _GaugeValue;
                mdlDfctDtl.GroupPreference = _Grouppref;
                mdlDfctDtl.IsViolation = _isViolation;
                mdlDfctDtl.CreatedBy = _UserID;
                mdlDfctDtl.Attachment = _ImagePath;
                mdlDfctDtl.GisX = _Longitude;
                mdlDfctDtl.GisY = _Latitude;
                mdlDfctDtl.MobileEntryDatetime = mobdt;
                mdlDfctDtl.CreatedDate = DateTime.Now;
                mdlDfctDtl.Remarks = _Remarks;

                db.Repository<ET_RotationalViolation>().Insert(mdlDfctDtl);
                db.Save();

                return "SUCCESS: Record saved successfully.-";
            }
            catch (Exception e)
            {
                string excetionMsg = e.Message;
                return "FAILURE: Unknown error occured.Please try again later.";
            }

        }


        public bool AddOutletChecking(SI_OutletChecking _oc)
        {
            bool IsSaved = true;
            try
            {
                if (_oc.ID == 0)
                {
                    db.Repository<SI_OutletChecking>().Insert(_oc);
                }

                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;

            }
            return IsSaved;
        }

        public SI_OutletChecking GetOutletCheckingByID(long _OutletCheckingID)
        {
            SI_OutletChecking oc = db.Repository<SI_OutletChecking>().FindById(_OutletCheckingID);
            CO_ChannelOutlets Co = db.Repository<CO_ChannelOutlets>().FindById(oc.OutletID);
            oc.SI_ScheduleDetailChannel = new SI_ScheduleDetailChannel();
            oc.SI_ScheduleDetailChannel.ChannelID = (long)Co.ChannelID;
            return oc;
        }

        public List<object> GetOutletsByUserID(long _UserID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletsByUserID(_UserID);
        }

        public List<object> GetOutletByOutletID_OR_OutletName(long _OutletID = 0, string OutletName = "")
        {
            List<object> lst = new List<object>();
            if (_OutletID == 0)
            {
                string[] OutletParts = OutletName.Split('/');
                int OutletRD = Convert.ToInt32(OutletParts[0]);
                string ChannelSide = OutletParts[1];
                CO_ChannelOutlets co = db.Repository<CO_ChannelOutlets>().GetAll().Where(x => x.OutletRD == OutletRD && x.ChannelSide == ChannelSide).FirstOrDefault();
                lst.Add(new { ID = co.ID, Name = co.OutletRD.ToString().Length > 4 ? co.OutletRD.ToString().Substring(0, 2) + "+" + co.OutletRD.ToString().Substring(2) + "/" + co.ChannelSide : co.OutletRD.ToString().Substring(0, 1) + "+" + co.OutletRD.ToString().Substring(1) + "/" + co.ChannelSide });

            }
            else
            {
                CO_ChannelOutlets co = db.Repository<CO_ChannelOutlets>().FindById(_OutletID);
                lst.Add(new { ID = co.ID, Name = co.OutletRD.ToString().Length > 4 ? co.OutletRD.ToString().Substring(0, 2) + "+" + co.OutletRD.ToString().Substring(2) + "/" + co.ChannelSide : co.OutletRD.ToString().Substring(0, 1) + "+" + co.OutletRD.ToString().Substring(1) + "/" + co.ChannelSide });
            }

            return lst;
        }
    }


}
