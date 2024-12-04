using PMIU.WRMIS.DAL;
using PMIU.WRMIS.DAL.Repositories.ScheduleInspection;
using PMIU.WRMIS.DAL.DataAccess.ScheduleInspection;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.Channel;
using System.Transactions;


namespace PMIU.WRMIS.BLL.ScheduleInspection
{
    public class ScheduleInspectionBLL : BaseBLL
    {
        ScheduleInspectionDAL dalScheduleInspection = new ScheduleInspectionDAL();

        #region "View Schedule"
        /// <summary>
        /// This function return Schedule by ID
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>SI_Schedule</returns>
        public SI_Schedule GetSchedule(long _ID)
        {
            return dalScheduleInspection.GetSchedule(_ID);
        }
        /// <summary>
        /// This function return Schedule Status by Schedule ID
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public SI_ScheduleStatus GetScheduleStatusByScheduleID(long _ID)
        {
            return dalScheduleInspection.GetScheduleStatusByScheduleID(_ID);
        }
        /// <summary>
        /// This function return Gauge Inspection 
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetGaugeInspectionsByScheduleID(long _ID)
        {
            return dalScheduleInspection.GetGaugeInspectionsByScheduleID(_ID);
        }
        /// <summary>
        /// This function return Outlet Alteration
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletAlterationByScheduleID(long _ID)
        {
            return dalScheduleInspection.GetOutletAlterationByScheduleID(_ID);
        }
        /// <summary>
        /// This function return schedule works
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetWorksByScheduleID(long _ID)
        {
            return dalScheduleInspection.GetWorksByScheduleID(_ID);
        }
        /// <summary>
        /// This function return Outlet Performance Register
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletPerformanceRegisterByScheduleID(long _ID)
        {
            return dalScheduleInspection.GetOutletPerformanceRegisterByScheduleID(_ID);
        }
        public List<object> GetTenderByScheduleID(long _ID)
        {
            return dalScheduleInspection.GetTenderByScheduleID(_ID);
        }
        /// <summary>
        /// This function return Discharge Measurement
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDischargeMeasurementByScheduleID(long _ID)
        {
            return dalScheduleInspection.GetDischargeMeasurementByScheduleID(_ID);
        }

        public List<object> GetOutletsExistingRecords(long _ScheduleID, long _InspectionTypeID)
        {
            return dalScheduleInspection.GetOutletExistingRecords(_ScheduleID, _InspectionTypeID);
        }

        /// <summary>
        /// This function return Schedule by UserID
        /// Created on: 08-09-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>

        public List<SI_GetUserSchedule_Result> GetUserSchedule(long _UserID)
        {

            return dalScheduleInspection.GetUserSchedule(_UserID);

        }
        /// <summary>
        /// This function return Schedule Inspection Types by Schedule ID
        /// Created on: 09-09-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <returns></returns>
        public List<SI_ScheduleTypes_Result> GetScheduleInspectionTypes(long _ScheduleID)
        {
            return dalScheduleInspection.GetScheduleInspectionTypes(_ScheduleID);
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
            return dalScheduleInspection.GetGaugesInspectionAreasRecords(_ScheduleID, _InspectionTypeID);
        }
        /// <summary>
        /// This function return Outlet Inspection Areas List by Schedule ID
        /// Created on: 09-09-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <param name="_InspectionTypeID"></param>
        /// <returns></returns>
        public List<SI_GetOutletInspectionAreas_Result> GetOutletsInspectionAreasRecord(long _ScheduleID, long _InspectionTypeID)
        {
            return dalScheduleInspection.GetOutletsInspectionAreasRecord(_ScheduleID, _InspectionTypeID);
        }
        /// <summary>
        /// This function return Outlet Information by OutletID ID
        /// Created on: 27-09-2016
        /// </summary>
        /// <param name="_OutletID"></param>
        /// <returns></returns>
        public List<SI_GetOutletDetail_Result> GetOutletByOutletID(long _OutletID)
        {
            ScheduleInspectionDAL dalSI = new ScheduleInspectionDAL();
            return dalSI.GetOutletByOutletID(_OutletID);
        }


        #endregion

        #region AddInspection
        public bool AddGauge_BedLevel_DischargeMeasurements(long _UserID, long _GaugeID, double _ParamN_B, double _ParamD_H, double _ObsrvdDschrg, int _GCorrectType, double? _GCorrectValue, long? _ScheduleDetailChannelID, String _Remarks, double? _Longitude, double? _Latitude, string _source, string _ObservationDate)
        {
            return dalScheduleInspection.AddGauge_BedLevel_DischargeMeasurements(_UserID, _GaugeID, _ParamN_B, _ParamD_H, _ObsrvdDschrg, _GCorrectType, _GCorrectValue, _ScheduleDetailChannelID, _Remarks, _Longitude, _Latitude, _source, _ObservationDate);
        }

        public bool AddGauge_CrestLevel_DischargeMeasurements(long _UserID, long _GaugeID, double _ParamN_B, double _ParamD_H, double _ObsrvdDschrg, long? _ScheduleDetailChannelID, String _Remarks, double? _Longitude, double? _Latitude, string _source, string _ObservationDate)
        {
            return dalScheduleInspection.AddGauge_CrestLevel_DischargeMeasurements(_UserID, _GaugeID, _ParamN_B, _ParamD_H, _ObsrvdDschrg, _ScheduleDetailChannelID, _Remarks, _Longitude, _Latitude, _source, _ObservationDate);
        }

        public bool AddOutletAlteration(int _UserID, long _OutletID, double? _HeightOfOutlet, double? _HeadAboveCrest, double? _WorkingHead, double? _DiameterBreadthWidth, long? _ScheduleDetailChannelID, string _Remarks, int? _OutletTypeID, string _OutletStatus,DateTime alterationdt)
        {
            return dalScheduleInspection.AddOutletAlteration(_UserID, _OutletID, _HeightOfOutlet, _HeadAboveCrest, _WorkingHead, _DiameterBreadthWidth, _ScheduleDetailChannelID, _Remarks, _OutletTypeID, _OutletStatus, alterationdt);
        }

        public bool AddOutletPerformance(long _UserID, long _OutletID, double? _HeadAboveCrest, double? _WorkingHead, double? _ObsrvdDschrg, double? _DiameterBreadthWidth, double? _HeightOfOutlet, string _Remarks, long? _SchID, double? _Longitude, double? _Latitude, string _Source, DateTime outletperfdt)
        {
            return dalScheduleInspection.AddOutletPerformance(_UserID, _OutletID, _HeadAboveCrest, _WorkingHead, _ObsrvdDschrg, _DiameterBreadthWidth, _HeightOfOutlet, _Remarks, _SchID, _Longitude, _Latitude, _Source,outletperfdt);
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
        //  List<object> lstGageDischrg = db.ExtRepositoryFor<ScheduleInspectionRepository>().GetGuageAndDischargeRecords(ScheduleID, Guage_Discharge).ToList();
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
            return dalScheduleInspection.GetScehduleInspectionStatuses();
        }


        public List<dynamic> GetSchedulesBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _StatusID, bool _ApprovedCheck, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstDivs, List<long> _lstSubDivs)
        {
            ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
            return dalSchedule.GetSchedulesBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _StatusID, _ApprovedCheck, _FromDate, _ToDate, _UserID, _DesignationID, _lstDivs, _lstSubDivs);
        }



        #endregion

        #region Add Schedule

        public int AddSchedule(SI_Schedule _ObjSave, int _UserId, int _UserDesID)
        {
            return dalScheduleInspection.SaveSchedule(_ObjSave, _UserId, _UserDesID);
        }

        public bool UpdateSchedule(SI_Schedule Schedule)
        {
            try
            {
                return dalScheduleInspection.UpdateSchedule(Schedule);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public SI_Schedule GetScheduleBasicInformation(long _ID)
        {
            return dalScheduleInspection.GetScheduleBasicInformation(_ID);
        }

        public bool CheckScheduleDetailDates(long _ScheduleID, DateTime _FromDate, DateTime _ToDate)
        {
            return dalScheduleInspection.CheckScheduleDetailDates(_ScheduleID, _FromDate, _ToDate);
        }

        public bool CheckInspectionDateCheck(long _ScheduleID, DateTime _InspectionDate)
        {
            return dalScheduleInspection.CheckInspectionDatesCheck(_ScheduleID, _InspectionDate);
        }


        public bool IsScheduleDependencyExists(long _ScheduleID)
        {

            return dalScheduleInspection.IsScheduleDependencyExists(_ScheduleID);
        }

        public bool DeleteSchedule(long _ScheduleID)
        {
            return dalScheduleInspection.DeleteSchedule(_ScheduleID);
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
            return dalScheduleInspection.GetScheduleData(_ScheduleID);
        }

        /// <summary>
        /// This function returns Schedule Status for a particular ScheduleID.
        /// Created By 19-07-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <returns>List<SI_ScheduleStatusDetail></returns>
        public List<SI_ScheduleStatusDetail> GetScheduleStatuses(long _ScheduleID)
        {
            return dalScheduleInspection.GetScheduleStatuses(_ScheduleID);
        }

        /// <summary>
        /// This function adds new schedule status detail.
        /// Created On 22-07-2016.
        /// </summary>
        /// <param name="_ScheduleStatusDetail"></param>
        /// <returns>bool</returns>
        public bool AddScheduleStatusDetail(SI_ScheduleStatusDetail _ScheduleStatusDetail, long _OldScheduleStatusID, long _SchedulePreparedByID)
        {
            bool isAdded = dalScheduleInspection.AddScheduleStatusDetail(_ScheduleStatusDetail);
            return LogScheduleInspectionNotifiation(_ScheduleStatusDetail.ScheduleID.Value, _ScheduleStatusDetail.ScheduleStatusID, _OldScheduleStatusID, _SchedulePreparedByID, _ScheduleStatusDetail.DesignationID.Value, _ScheduleStatusDetail.UserID.Value);

        }
        public bool LogScheduleInspectionNotifiation(long _ScheduleID, long _CurrentScheduleStatusID, long _OldScheduleStatusID, long _SchedulePreparedByID, long _DesignationID, long _UserID)
        {
            long eventID = 0;
            bool isLogged = false;
            NotifyEvent _event = new NotifyEvent();

            _event.Parameters.Add("ScheduleID", _ScheduleID);

            switch (_DesignationID)
            {
                case (long)Constants.Designation.SDO:

                    if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                    {
                        eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfSDOIsAssignedToXENForApproval;
                    }
                    break;
                case (long)Constants.Designation.XEN:
                    if (_SchedulePreparedByID == _UserID) // Schedule prepared by XEN himself is only send for approval
                    {
                        if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfXENIsAssignedToSEForApproval;
                        }
                    }
                    else // SDO sent schedule to XEN for approval, XEN can approve, reject, send back for rework
                    {
                        if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfSDOIsApprovedByXEN;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfSDOIsRejectedByXEN;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfSDOIsSendBackForReworkByXEN;
                        }
                    }

                    break;
                case (long)Constants.Designation.SE:
                    if (_SchedulePreparedByID != _UserID) // XEN Prepared Schedule
                    {
                        // XEN send schedule to SE for approval, SE can approve, reject, send back for rework
                        if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfXENIsApprovedBySE;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfXENIsRejectedBySE;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfXENIsSendBackForReworkBySE;
                        }
                    }
                    break;
                case (long)Constants.Designation.MA:
                    if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                    {
                        eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsAssignedToADMForApproval;
                    }
                    break;
                case (long)Constants.Designation.ADM:
                    if (_SchedulePreparedByID == _UserID) // Schedule prepared by ADM himself is only send for approval
                    {
                        if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsAssignedToDDForApproval;
                        }
                    }
                    // Schedule of MA, ADM can forward MA schedule to DD for approval

                    else if (_SchedulePreparedByID != _UserID && _OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                        eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsAssignedToDDForApproval;

                    else // MA sent schedule to ADM for approval, ADM can approve, reject, send back for rework
                    {
                        if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsApprovedByADM;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsRejectedByADM;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsSendBackForReworkByADM;
                        }
                    }

                    break;
                case (long)Constants.Designation.DeputyDirector:
                    if (_SchedulePreparedByID != _UserID && _OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                    {
                        eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsAssignedToDDForApproval; // && _DesignationID == (long)Constants.Designation.ADM) // ADM Prepared Schedule
                    }
                    else
                    {
                        // ADM send schedule to DD for approval, DD can approve, reject, send back for rework
                        if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsApprovedByDD;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsRejectedByDD;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsSendBackForReworkByDD;
                        }
                    }
                    break;
                case (long)Constants.Designation.DirectorGauges:
                    if (_SchedulePreparedByID != _UserID)
                    {
                        // DD send schedule to DG for approval, DG can approve, reject, send back for rework
                        if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsApprovedByDD;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsRejectedByDD;
                        }
                        else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsSendBackForReworkByDD;
                        }
                    }
                    //else if (_SchedulePreparedByID != _UserID && _DesignationID == (long)Constants.Designation.MA) // MA Prepared Schedule
                    //{
                    //    // MA send schedule to DD for approval, DD can approve, reject, send back for rework
                    //    if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                    //    {
                    //        eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsApprovedByDD;
                    //    }
                    //    else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                    //    {
                    //        eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsRejectedByDD;
                    //    }
                    //    else if (_OldScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _CurrentScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                    //    {
                    //        eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsSendBackForReworkByDD;
                    //    }
                    //}
                    break;
                default:
                    break;
            }
            if (eventID > 0)
                isLogged = _event.AddNotifyEvent(eventID, _UserID);

            return isLogged;
        }

        public dynamic GetScheduleDataForGeneralIsnpections(long _ScheduleDetailID)
        {
            return dalScheduleInspection.GetScheduleDataForGeneralInspections(_ScheduleDetailID);
        }

        #endregion

        #region Schedule Deatil

        public List<object> GetGaugeInspectionArea(long _ChannelID)
        {
            return dalScheduleInspection.GetGaugeInspectionArea(_ChannelID);
        }

        public List<object> GetOutletsAgainstChannel(long _ChannelID)
        {
            return dalScheduleInspection.GetOutletsAgainstChannel(_ChannelID);
        }
        public List<object> GetScheduleDetailByScheduleIDInspectionTypeID(long _ScheduleID, long _InspectionTypeID)
        {
            return dalScheduleInspection.GetScheduleDetailByScheduleIDInspectionTypeID(_ScheduleID, _InspectionTypeID);
        }

        public List<object> GetScheduleDetailForTenderMonitoring(long _ScheduleID)
        {
            return dalScheduleInspection.GetScheduleDetailForTenderMonitoring(_ScheduleID);
        }
        public List<object> GetScheduleDetailForClosureOperations(long _ScheduleID)
        {
            return dalScheduleInspection.GetScheduleDetailForClosureOperations(_ScheduleID);
        }
        public List<object> GetScheduleDetailForGeneralInspections(long _ScheduleID)
        {
            return dalScheduleInspection.GetScheduleDetailForGeneralInspections(_ScheduleID);
        }
        public List<TM_TenderNotice> GetTenderNoticesByDivisionID(long _DivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            return dalScheduleInspection.GetTenderNoticesByDivisionID(_DivisionID, _FromDate, _ToDate);
        }
        public List<TM_TenderNotice> GetTenderNoticesByDivisionID(long _DivisionID)
        {
            return dalScheduleInspection.GetTenderNoticesByDivisionID(_DivisionID);
        }
        public List<SI_GeneralInspectionType> GetGeneralInspectionTypes()
        {
            return dalScheduleInspection.GetGeneralInspectionTypes();
        }
        public List<object> GetTenderWorksByTenderNoticeID(long _TenderNoticeID)
        {
            return dalScheduleInspection.GetTenderWorksByTenderNoticeID(_TenderNoticeID);
        }
        public List<object> GetClosureWorksByWorkSourceAndDivisionID(string _WorkSource, long _DivisionID)
        {
            return dalScheduleInspection.GetClosureWorksByWorkSourceAndDivisionID(_WorkSource, _DivisionID);
        }
        public DateTime GetOpeningDateByTenderNoticeID(long _TenderNoticeID)
        {
            return dalScheduleInspection.GetTenderNoticeOpeningDateBytenderNoticeID(_TenderNoticeID);
        }

        public SI_ScheduleDetailChannel GetSelectedRecordDetail(long _ID)
        {
            return dalScheduleInspection.GetSelectedRecordDetail(_ID);
        }

        public bool SaveGaugeInspectionScheduleDetail(SI_ScheduleDetailChannel _ScheduleDetailChannel)
        {
            return dalScheduleInspection.SaveGaugeInspectionScheduleDetail(_ScheduleDetailChannel);
        }
        public bool SaveGaugeInspectionScheduleDetail(List<SI_ScheduleDetailChannel> _lstScheduleDetailChannel)
        {
            return dalScheduleInspection.SaveGaugeInspectionScheduleDetail(_lstScheduleDetailChannel);
        }
        public bool IsDuplicateInspectionAreaExist(ref List<SI_ScheduleDetailChannel> lstScheduleDetailChannel)
        {
            return dalScheduleInspection.IsDuplicateInspectionAreaExist(ref lstScheduleDetailChannel);
        }
        public bool IsDuplicateInspectionAreaExist(SI_ScheduleDetailChannel ScheduleDetailChannel)
        {
            return dalScheduleInspection.IsDuplicateInspectionAreaExist(ScheduleDetailChannel);
        }
        public bool DeleteGaugeRecord(long _ID)
        {
            return dalScheduleInspection.DeleteGaugeRecord(_ID);
        }

        public bool UpdateDetail(SI_ScheduleDetailChannel _ObjSave)
        {
            return dalScheduleInspection.UpdateDetail(_ObjSave);
        }

        public bool IsGaugeInspectionNotesExist(long _ScheduleID)
        {
            return dalScheduleInspection.IsGaugeInspectionNotesExist(_ScheduleID);
        }

        public bool IsOutletAltrationInspNotesExist(long _ScheduleID)
        {
            return dalScheduleInspection.IsOutletAltrationInspNotesExist(_ScheduleID);
        }

        public bool IsOutletPerformanceInspNotesExist(long _ScheduleID)
        {
            return dalScheduleInspection.IsOutletPerformanceInspNotesExist(_ScheduleID);
        }

        public bool IsDTParameterInspectionNotesExist(long _ScheduleID)
        {
            return dalScheduleInspection.IsDTParameterInspectionNotesExist(_ScheduleID);
        }

        #endregion

        #region Outlet Inspection
        public double GetOutletDischarge(long _OutletID)
        {
            return dalScheduleInspection.GetOutletDischarge(_OutletID);
        }
        public dynamic GetOutletInspection(long _ScheduleDetailChannelID, long? _InspectionTypeID)
        {
            return dalScheduleInspection.GetOutletInspection(_ScheduleDetailChannelID, _InspectionTypeID);
        }
        public bool AddOutletAlterationInspectionNotes(SI_OutletAlterationHistroy _OutletAlterationHistory)
        {
            return dalScheduleInspection.AddOutletAlterationInspectionNotes(_OutletAlterationHistory);
        }
        public bool AddOutletPerformanceInspection(CO_ChannelOutletsPerformance _OutletPerformance)
        {
            return dalScheduleInspection.AddOutletPerformanceInspection(_OutletPerformance);
        }
        public SI_OutletAlterationHistroy GetOutletAlterationInspection(long _ScheduleDetailChannleID)
        {
            return dalScheduleInspection.GetOutletAlterationInspection(_ScheduleDetailChannleID);
        }
        public CO_ChannelOutletsPerformance GetOutletsPerformanceInspection(long _ScheduleDetailChannleID)
        {
            return dalScheduleInspection.GetOutletsPerformanceInspection(_ScheduleDetailChannleID);
        }

        public CO_ChannelOutletsPerformance GetOutletsPerformanceInspectionByID(long _OutletPerformanceID)
        {
            return dalScheduleInspection.GetOutletsPerformanceInspectionByID(_OutletPerformanceID);
        }

        public SI_OutletAlterationHistroy GetOutletsAlterationInspectionByID(long _OutletAlterationID)
        {
            return dalScheduleInspection.GetOutletsAlterationInspectionByID(_OutletAlterationID);
        }
        #endregion

        #region Inspection Notes

        public dynamic GetScheduleDetailData(long _ScheduleDetailID)
        {
            return dalScheduleInspection.GetScheduleDetailData(_ScheduleDetailID);
        }

        public dynamic GetScheduleDetailDataBL(long _ScheduleDetailID)
        {
            return dalScheduleInspection.GetScheduleDetailDataBL(_ScheduleDetailID);
        }

        public dynamic GetScheduleDetailDataCL(long _ScheduleDetailID)
        {
            return dalScheduleInspection.GetScheduleDetailDataCL(_ScheduleDetailID);
        }

        public dynamic GetGaugeLocation(long _GaugeID)
        {
            return dalScheduleInspection.GetGaugeLocation(_GaugeID);
        }


        public List<dynamic> GetComplaintsForGaugeInspection(long _ModuleID, long _RefCodeID)
        {
            return dalScheduleInspection.GetComplaintsForGaugeInspection(_ModuleID, _RefCodeID);
        }
        public dynamic GetTailStatus(long _GaugeReadingID)
        {
            return dalScheduleInspection.GetTailStatus(_GaugeReadingID);
        }

        public long SaveGaugeReadingData(SI_ChannelGaugeReading _GaugeReading, UA_Users _mdlUser, long _DivisionID, ref bool? _IsComplaintGenerated, ref List<string> _ComplaintIDs)
        {

            long GaugeReadingID = dalScheduleInspection.SaveGaugeReading(_GaugeReading);

            object _ChannelData = GetChannelGaugeByID(_GaugeReading.GaugeID);
            long _ChannelID = Convert.ToInt64(_ChannelData.GetType().GetProperty("ChannelID").GetValue(_ChannelData, null));
            string _ChannelName = Convert.ToString(_ChannelData.GetType().GetProperty("ChannelName").GetValue(_ChannelData, null));
            string _GaugeAtRD = Convert.ToString(_ChannelData.GetType().GetProperty("GaugeRD").GetValue(_ChannelData, null));
            string Description = "";
            if (GaugeReadingID != 0)
            {

                if (_mdlUser.DesignationID == (long)Constants.Designation.ADM || _mdlUser.DesignationID == (long)Constants.Designation.MA)
                {
                    dynamic GaugeLocation = GetGaugeLocation(_GaugeReading.GaugeID);
                    var GaugeLocationID = GaugeLocation.GetType().GetProperty("GaugeLocationID").GetValue(GaugeLocation, null);
                    //Head Gauge Section
                    if (GaugeLocationID == 1)
                    {
                        if (_GaugeReading.IsGaugeFixed == false || _GaugeReading.IsGaugePainted == false)
                        {
                            //Head Gauge not Fixed
                            if (_GaugeReading.IsGaugeFixed == false)
                            {
                                ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();

                                Description = "Complaint generated at RD " + _GaugeAtRD + " for Channel " + _ChannelName + " where Head Gauge is not Fixed.";
                                string ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(_mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.SI_HGNF), _DivisionID, GaugeReadingID, _ChannelID, Description);
                                if (string.IsNullOrEmpty(ComplaintID))
                                {
                                    _IsComplaintGenerated = false;
                                }
                                else
                                {
                                    _IsComplaintGenerated = true;
                                    _ComplaintIDs.Add(ComplaintID);
                                }


                            }
                            //Head Gauge not Painted
                            if (_GaugeReading.IsGaugePainted == false)
                            {
                                ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
                                Description = "Complaint generated at RD " + _GaugeAtRD + " for Channel " + _ChannelName + " where Head Gauge is not Painted.";
                                string ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(_mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.SI_HGNP), _DivisionID, GaugeReadingID, _ChannelID, Description);
                                if (string.IsNullOrEmpty(ComplaintID))
                                {
                                    _IsComplaintGenerated = false;
                                }
                                else
                                {
                                    _IsComplaintGenerated = true;
                                    _ComplaintIDs.Add(ComplaintID);
                                }


                            }
                        }
                    }

                     //Tail Gaugen Section
                    else if (GaugeLocationID == 2)
                    {
                        var TailStatusData = GetTailStatus(GaugeReadingID);
                        var GaugeValue = TailStatusData.GetType().GetProperty("GaugeValue").GetValue(TailStatusData, null);
                        var AuthorizedGaugeValue = TailStatusData.GetType().GetProperty("AuthorizedGaugeValue").GetValue(TailStatusData, null);

                        if (GaugeValue != null && AuthorizedGaugeValue != null)
                        {
                            double TailPercentage = (GaugeValue / AuthorizedGaugeValue) * 100;
                            if (TailPercentage > 0)
                            {
                                //Short Tail
                                if (TailPercentage > 30 && TailPercentage < 90)
                                {
                                    ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
                                    Description = "Complaint generated at RD " + _GaugeAtRD + " for Channel " + _ChannelName + " where Tail is Short.";
                                    string ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(_mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.SI_ST), _DivisionID, GaugeReadingID, _ChannelID, Description);
                                    if (string.IsNullOrEmpty(ComplaintID))
                                    {
                                        _IsComplaintGenerated = false;
                                    }
                                    else
                                    {
                                        _IsComplaintGenerated = true;
                                        _ComplaintIDs.Add(ComplaintID);
                                    }

                                }
                                //Dry Tail
                                else if (TailPercentage <= 30)
                                {
                                    ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
                                    Description = "Complaint generated at RD " + _GaugeAtRD + " for Channel " + _ChannelName + " where Tail is Dry.";
                                    string ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(_mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.SI_DT), _DivisionID, GaugeReadingID, _ChannelID, Description);
                                    if (string.IsNullOrEmpty(ComplaintID))
                                    {
                                        _IsComplaintGenerated = false;
                                    }
                                    else
                                    {
                                        _IsComplaintGenerated = true;
                                        _ComplaintIDs.Add(ComplaintID);
                                    }

                                }

                            }
                        }

                        //Tail Gauge not fixed
                        if (_GaugeReading.IsGaugeFixed == false)
                        {
                            ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
                            Description = "Complaint generated at RD " + _GaugeAtRD + " for Channel " + _ChannelName + " where Tail Gauge is not Fixed.";
                            string ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(_mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.SI_TGNF), _DivisionID, GaugeReadingID, _ChannelID, Description);
                            if (string.IsNullOrEmpty(ComplaintID))
                            {
                                _IsComplaintGenerated = false;
                            }
                            else
                            {
                                _IsComplaintGenerated = true;
                                _ComplaintIDs.Add(ComplaintID);
                            }

                        }
                        //Tail Gauge not painted
                        if (_GaugeReading.IsGaugePainted == false)
                        {
                            ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
                            Description = "Complaint generated at RD " + _GaugeAtRD + " for Channel " + _ChannelName + " where Tail Gauge is not Painted.";
                            string ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(_mdlUser.ID, Convert.ToString(Constants.ComplaintModuleReference.SI_TGNP), _DivisionID, GaugeReadingID, _ChannelID, Description);
                            if (string.IsNullOrEmpty(ComplaintID))
                            {
                                _IsComplaintGenerated = false;
                            }
                            else
                            {
                                _IsComplaintGenerated = true;
                                _ComplaintIDs.Add(ComplaintID);
                            }

                        }
                    }
                }
            }

            return GaugeReadingID;
        }

        public bool IsGaugeRecordAlreadyExists(long _ScheduleDetailChannelID, int _GaugeID)
        {
            return dalScheduleInspection.IsGaugeRecordAlreadyExists(_ScheduleDetailChannelID, _GaugeID);

        }

        public bool IsGaugeInspectionAlreadyExists(long _ScheduleDetailChannelID)
        {
            return dalScheduleInspection.IsGaugeInspectionAlreadyExists(_ScheduleDetailChannelID);

        }

        public bool IsDischargeTblBedLevelInspectionAlreadyExists(long _ScheduleDetailChannelID)
        {
            return dalScheduleInspection.IsDischargetblBedLevelInspectionAlreadyExists(_ScheduleDetailChannelID);

        }
        public bool IsDischargeTblCrestLevelInspectionAlreadyExists(long _ScheduleDetailChannelID)
        {
            return dalScheduleInspection.IsDischargetblCrestLevelInspectionAlreadyExists(_ScheduleDetailChannelID);

        }
        public bool IsOutletAlterationRecordAlreadyExists(long _ScheduleDetailChannelID, int _OutletID)
        {
            return dalScheduleInspection.IsOutletAlterationRecordAlreadyExists(_ScheduleDetailChannelID, _OutletID);

        }

        public bool IsOutletPerformanceRecordAlreadyExists(long _ScheduleDetailChannelID, int _OutletID)
        {
            return dalScheduleInspection.IsOutletPerformanceRecordAlreadyExists(_ScheduleDetailChannelID, _OutletID);

        }

        public bool IsDischargeCalcBLRecordAlreadyExists(long _ScheduleDetailID, int _GaugeID)
        {
            return dalScheduleInspection.IsDischargeCalcBLRecordAlreadyExists(_ScheduleDetailID, _GaugeID);

        }
        public bool IsDischargeCalcCLRecordAlreadyExists(long _ScheduleDetailID, int _GaugeID)
        {
            return dalScheduleInspection.IsDischargeCalcCLRecordAlreadyExists(_ScheduleDetailID, _GaugeID);

        }


        public dynamic GetGaugeRecordbyID(int _GaugeID, long _ScheduleDetailID)
        {
            return dalScheduleInspection.GetGaugeRecordbyID(_GaugeID, _ScheduleDetailID);

        }

        public dynamic GetChannelGaugeRecordbyID(int ChannelGaugeID)
        {
            return dalScheduleInspection.GetChannelGaugeRecordbyID(ChannelGaugeID);

        }
        public dynamic GetDischargeCalcBLDatabyID(int _GaugeID, long _ScheduleDetailID)
        {
            return dalScheduleInspection.GetDischargeCalcBLDatabyID(_GaugeID, _ScheduleDetailID);

        }

        public dynamic GetDischargeCalcBLDatabyDischargeID(int DischargeID)
        {
            return dalScheduleInspection.GetDischargeCalcBLDatabyDischargeID(DischargeID);

        }
        public dynamic GetDischargeCalcCLDatabyID(int _GaugeID, long _ScheduleDetailID)
        {
            return dalScheduleInspection.GetDischargeCalcCLDatabyID(_GaugeID, _ScheduleDetailID);

        }
        public dynamic GetDischargeCalcCLDatabyDischargeID(int DischargeID)
        {
            return dalScheduleInspection.GetDischargeCalcCLDatabyDischargeID(DischargeID);

        }

        public List<string> GetUploadedFileNames(int _GaugeID, long _GaugeReadingID)
        {

            return dalScheduleInspection.GetUploadedNames(_GaugeID, _GaugeReadingID);
        }
        public List<string> GetUploadedFileNamesForGeneralInspections(long _ID)
        {

            return dalScheduleInspection.GetUploadedNamesForGeneralInspections(_ID);
        }

        public long SaveGeneralInspection(SI_GeneralInspections _mdlGeneralInspection)
        {
            return dalScheduleInspection.SaveGeneralInspection(_mdlGeneralInspection);
        }

        public void SaveGeneralInspectionAttachments(SI_GeneralInspectionsAttachment _mdlGeneralInspectionAttachments)
        {
            dalScheduleInspection.SaveGeneralInspectionAttachemnts(_mdlGeneralInspectionAttachments);
        }

        #endregion

        #region Search Inspection

        public bool IsScheduleInspectionsExists(long _ScheduleID)
        {
            return dalScheduleInspection.IsScheduleInspectionsExists(_ScheduleID);
        }

        public List<dynamic> GetGaugeInspectionBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionType, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetGaugeInspectionBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionType, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public dynamic GetGaugeChannelByGaugeReadingID(long _GaugeReadingID)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetGaugeChannelNameByGaugeReadingID(_GaugeReadingID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetGaugeChannelByDischargeID(long _DischargeID)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetGaugeChannelNameByDischargeID(_DischargeID);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public dynamic GetGaugeChannelByOutletPerformanceID(long _OutletPerformanceID)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetGaugeChannelNameByOutletPerformanceID(_OutletPerformanceID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetGaugeChannelByOutletAlterationID(long _OutletAlterationID)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetGaugeChannelNameByOutletAlterationID(_OutletAlterationID);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public dynamic GetGaugeChannelByDischargeCLID(long _DischargeID)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetGaugeChannelNameByDischargeCLID(_DischargeID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetDischargeTableBedLevelBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionType, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetDischargeTableBedLevelBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionType, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public List<dynamic> GetDischargeTableCrestLevelBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionType, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetDischargeTableCrestLevelBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionType, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetOutletPerformanceBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionType, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetOutletPerformanceBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionType, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetOutletCheckingBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionType, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetOutletCheckingBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionType, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        public List<dynamic> GetOutletAlterationHistoryBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionType, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetOutletAlterationHistoryBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionType, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetTenderMonitoringBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionType, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetTenderMonitoringBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionType, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetClosureOperationsBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionType, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetClosureOperationsBySearchCriteria(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _InspectionType, _FromDate, _ToDate, _UserID, _DesignationID, _lstUserZone, _lstUserCircle, _lstUserDiv, _lstUserSubDiv);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetGeneralInspectionsBySearchCriteria(
 long _InspectionType
, string _FromDate
, string _ToDate
, long _UserID
, long? _DesignationID)
        {
            try
            {
                ScheduleInspectionDAL dalSchedule = new ScheduleInspectionDAL();
                return dalSchedule.GetGeneralInspectionsBySearchCriteria(
                  _InspectionType
                    , _FromDate
                    , _ToDate
                    , _UserID
                    , _DesignationID);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion


        /// <summary>
        /// This function return Outlet Alteration History
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public object GetOutletAlterationHistoryDetail(long _ScheduleChnlDetailID, long _OutletID)
        {
            return dalScheduleInspection.GetOutletAlterationHistoryDetail(_ScheduleChnlDetailID, _OutletID);
        }
        /// <summary>
        /// This function return Outlet Alteration History
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns><object></returns>
        public object GetOutletAlterationPerformanceDetail(long _ScheduleChnlDetailID, long _OutletID)
        {
            return dalScheduleInspection.GetOutletAlterationPerformanceDetail(_ScheduleChnlDetailID, _OutletID);
        }


        public List<object> GetScheduledInspectionsByMonth(string _MonthParam, string _UserParam)
        {
            return dalScheduleInspection.GetScheduledInspectionsByMonth(_MonthParam, _UserParam);
        }
        public List<object> GetScheduledInspectionsByDate(string _DateParam, string _UserParam)
        {
            return dalScheduleInspection.GetScheduledInspectionsByDate(_DateParam, _UserParam);
        }

        #region Schedule Calendar
        public List<object> GetUserInspectionDates(long _UserID, long _Month, long _Year)
        {
            return dalScheduleInspection.GetUserInspectionDates(_UserID, _Month, _Year);
        }
        public List<dynamic> GetUserInspectionsByDate(long _UserID, DateTime _Date)
        {
            return dalScheduleInspection.GetUserInspectionsByDate(_UserID, _Date);
        }
        public List<dynamic> GetTenderInspectionsByDate(long _UserID, DateTime _Date)
        {
            return dalScheduleInspection.GetTenderInspectionsByDate(_UserID, _Date);
        }
        public List<dynamic> GetClosureInspectionsByDate(long _UserID, DateTime _Date)
        {
            return dalScheduleInspection.GetClosureInspectionsByDate(_UserID, _Date);
        }
        public List<dynamic> GetGeneralInspectionsByDate(long _UserID, DateTime _Date)
        {
            return dalScheduleInspection.GetGeneralInspectionsByDate(_UserID, _Date);
        }
        #endregion

        public long AddGaugeValue(long _UserID, long _GaugeID, bool _GaugeF, bool _GaugeP, double? _GaugeValue, string _GaugeImage, string _Remarks, double? _Longitude, double? _Latitude, long? _SchChnlDetID, long? _DesignationID, double? _dsrg, string _DataSource)
        {
            return dalScheduleInspection.AddGaugeValue(_UserID, _GaugeID, _GaugeF, _GaugeP, _GaugeValue, _GaugeImage, _Remarks, _Longitude, _Latitude, _SchChnlDetID, _DesignationID, _dsrg, _DataSource);
        }

        public long? GetUserDesignationID(long _UserID)
        {
            return dalScheduleInspection.GetUserDesignationID(_UserID);
        }

        public long GetDivisionIDByGaugeID(long _GaugeID)
        {
            return dalScheduleInspection.GetDivisionIDByGaugeID(_GaugeID);
        }
        public SI_GetScheduleInspectionNotifyData_Result GetScheduleInspectionNotifyData(long _ScheduleID)
        {
            return dalScheduleInspection.GetScheduleInspectionNotifyData(_ScheduleID);
        }
        public SI_GetDischargeTableCalcBLNotifyData_Result GetDischargeTableCalcBLNotifyData(long _ScheduleDetailsID)
        {
            return dalScheduleInspection.GetDischargeTableCalcBLNotifyData(_ScheduleDetailsID);
        }

        public bool DeleteSIGaugeReading(long ID)
        {
            return dalScheduleInspection.DeleteSIGaugeReading(ID);
        }
        public bool IsGaugeReadingExists(long ID)
        {
            return dalScheduleInspection.IsGaugeReadingExists(ID);
        }
        public long GetScheduleIDByScheduleDetailGeneralID(long _ID)
        {
            return dalScheduleInspection.GetScheduleIDByScheduleDetailGeneralID(_ID);
        }
        /// <summary>
        /// This function gets Channel Gauge by ID.
        /// Created On 13-11-2015.
        /// </summary>
        /// <param name="_ChannelGaugeID"></param>
        /// <returns>CO_ChannelGauge</returns>
        public object GetChannelGaugeByID(long _GaugeID)
        {
            return dalScheduleInspection.GetChannelByGaugeID(_GaugeID);
        }

        public object GetInspectionDetailByTypeID(int _InspectionType, int _GaugeID, long _ScheduleDetailChannelID, String serverUploadFolder)
        {
            return dalScheduleInspection.GetInspectionDetailByTypeID(_InspectionType, _GaugeID, _ScheduleDetailChannelID, serverUploadFolder);
        }

        public SI_Schedule GetScheduleDetailByScheduleDetailID(long _ScheduleDetailChannelID)
        {
            return dalScheduleInspection.GetScheduleDetailByScheduleDetailID(_ScheduleDetailChannelID);
        }

        public bool SaveTenderScheduleDetail(SI_ScheduleDetailTender _ScheduleDetailTender)
        {
            return dalScheduleInspection.SaveTenderScheduleDetail(_ScheduleDetailTender);

        }
        public bool SaveWorkScheduleDetail(SI_ScheduleDetailWorks _ScheduleDetailWork)
        {
            return dalScheduleInspection.SaveWorkScheduleDetail(_ScheduleDetailWork);

        }
        public bool SaveGeneralScheduleDetail(SI_ScheduleDetailGeneral _ScheduleDetailGeneral)
        {
            return dalScheduleInspection.SaveGeneralScheduleDetail(_ScheduleDetailGeneral);

        }
        public bool IsADMRecordExists(long _TenderWorkID)
        {
            return dalScheduleInspection.IsADMRecordExists(_TenderWorkID);

        }
        public bool IsGeneralInspectionAdded(long _ScheduleDetailID)
        {
            return dalScheduleInspection.IsGeneralInspectionAdded(_ScheduleDetailID);

        }
        public bool IsClosureProgressExists(long _ScheduleDetailID)
        {
            return dalScheduleInspection.IsClosureProgressExists(_ScheduleDetailID);

        }
        public bool DeleteTenderRecord(long _ID)
        {
            return dalScheduleInspection.DeleteTenderRecord(_ID);

        }
        public bool DeleteCLosureWorkRecord(long _ID)
        {
            return dalScheduleInspection.DeleteClosureWorkRecord(_ID);

        }
        public bool DeleteGeneralInspection(long _ID)
        {
            return dalScheduleInspection.DeleteGeneralInspection(_ID);

        }
        public SI_GeneralInspections GetGeneralInspectionsByID(long _ID)
        {
            return dalScheduleInspection.GetGeneralInspectionsByID(_ID);

        }
        public bool IsScheduleDetailTenderExists(long _TenderNoticeID, long _TenderWorkID, long _ScheduleID)
        {
            return dalScheduleInspection.IsScheduleDetailTenderExists(_TenderNoticeID, _TenderWorkID, _ScheduleID);

        }
        public bool IsScheduleDetailWorkExists(long _WorkSourceID, string _WorkSource, long _ScheduleID)
        {
            return dalScheduleInspection.IsScheduleDetailWorkExists(_WorkSourceID, _WorkSource, _ScheduleID);

        }
        public bool IsScheduleDetailGeneralExists(long _ScheduleID, long _TypeID, string _Location)
        {
            return dalScheduleInspection.IsScheduleDetailGeneralInspectionExists(_ScheduleID, _TypeID, _Location);

        }

        public List<SI_GeneralInspectionType> GetAllGeneralInspectionTypes()
        {
            return dalScheduleInspection.GetAllGeneralInspectionTypes();

        }
        public SI_GeneralInspectionType GetGeneralInspectionTypeByName(string _Name)
        {
            return dalScheduleInspection.GetGeneralInspectionTypeByName(_Name);

        }
        public bool AddGeneralInspectionType(SI_GeneralInspectionType _mdlGeneralInspectionType)
        {
            return dalScheduleInspection.AddgeneralInspectionType(_mdlGeneralInspectionType);

        }

        public bool IsGeneralInspectionTypeAssociated(long _ID)
        {
            return dalScheduleInspection.IsgeneralInspectionAssociated(_ID);

        }
        public bool DeleteGeneralInspectionType(long _ID)
        {
            return dalScheduleInspection.DeleteGeneralInspectionType(_ID);

        }

        //public List<object> GetAllGeneralInspectionTypes()
        //{
        //    return dalScheduleInspection.GetAllGeneralInspectionTypes();


        //}
        public object GetGeneralInspectionByID(long _ScheduleIDParam, long _TypeParam)
        {
            return dalScheduleInspection.GetGeneralInspectionByID(_ScheduleIDParam, _TypeParam);
        }

        public long AddGeneralInspection(long? _ScheduleDetailID, long _GeneralInspectionTypeID, string _InspectionLocation, DateTime _InspectionDate, string _InspectionDetails, string _Remarks, List<Tuple<string, string, string>> _Attachment, int _UserID)
        {
            SI_GeneralInspections _mdlGeneralInspection = new SI_GeneralInspections();

            using (TransactionScope transaction = new TransactionScope())
            {
                _mdlGeneralInspection.ScheduleDetailGeneralID = _ScheduleDetailID;
                _mdlGeneralInspection.GeneralInspectionTypeID = _GeneralInspectionTypeID;
                _mdlGeneralInspection.InspectionLocation = _InspectionLocation;
                _mdlGeneralInspection.InspectionDetails = _InspectionDetails;

                //DateTime dt = Convert.ToDateTime(_InspectionDate);
                //DateTime result = dt.Add(DateTime.Now.TimeOfDay);

                _mdlGeneralInspection.InspectionDate = _InspectionDate;
                _mdlGeneralInspection.Remarks = _Remarks;
                _mdlGeneralInspection.CreatedBy = _UserID;
                _mdlGeneralInspection.CreatedDate = DateTime.Now;

                SaveGeneralInspection(_mdlGeneralInspection);

                if (_mdlGeneralInspection.ID > 0)
                {
                    foreach (var atchmnt in _Attachment)
                    {
                        SI_GeneralInspectionsAttachment mdl = new SI_GeneralInspectionsAttachment();
                        mdl.GeneralInspectionsID = _mdlGeneralInspection.ID;
                        mdl.Attachment = atchmnt.Item3;
                        mdl.CreatedDate = DateTime.Now;
                        mdl.CreatedBy = _mdlGeneralInspection.CreatedBy;

                        SaveGeneralInspectionAttachments(mdl);
                    }

                }
                transaction.Complete();
            }
            return _mdlGeneralInspection.ID;
        }
        public List<object> GetGeneralInspectionTypeList()
        {
            return dalScheduleInspection.GetGeneralInspectionTypeList();
        }
        public bool IsWorkInspectionExists(long _ScheduleID)
        {
            return dalScheduleInspection.IsWorksInspectionExists(_ScheduleID);
        }
        public bool IsGeneralInspectionExists(long _ScheduleID)
        {
            return dalScheduleInspection.IsGeneralInspectionExists(_ScheduleID);
        }
        public double? GetGaugeDesignDischargeByID(long _GaugeID)
        {
            return dalScheduleInspection.GetGaugeDesignDischargeByID(_GaugeID);
        }
        public SI_GetDischargeTableCalcBLNotifyDataAndroid_Result GetDischargeTableCalcBLNotifyDataAndroid(long? _GaugeID, string _CalcType)
        {
            return dalScheduleInspection.GetDischargeTableCalcBLNotifyDataAndroid(_GaugeID, _CalcType);
        }

        public List<CO_Zone> GetUserZones(List<long> _lstZones)
        {
            return dalScheduleInspection.GetUserZones(_lstZones);
        }

        public List<CO_Circle> GetUserCircles(List<long> _lstCircles)
        {
            return dalScheduleInspection.GetUserCircles(_lstCircles);
        }

        public List<CO_Division> GetUserDivision(List<long> _lstDivision)
        {
            return dalScheduleInspection.GetUserDivision(_lstDivision);
        }

        public List<CO_SubDivision> GetUserSubDivision(List<long> _lstSubDivision)
        {
            return dalScheduleInspection.GetUserSubDivision(_lstSubDivision);
        }

        public List<object> GetLeavesTypes()
        {
            return dalScheduleInspection.GetLeaveTypeList();
        }

        public string AddLeaveForm(int _UserID, double? _GaugeValue, string _ImagePath, string _Remarks, double? _Longitude, double? _Latitude, DateTime mobdt, DateTime leavedate, short _LeaveTypeID)
        {
            return dalScheduleInspection.AddLeaveForm(_UserID, _GaugeValue, _ImagePath, _Remarks, _Longitude, _Latitude, mobdt, leavedate, _LeaveTypeID);
        }
        public object GeOutletCheckingByScheduleID(long _ScheduleChnlDetailID, long _OutletID)
        {
            return dalScheduleInspection.GeOutletCheckingByScheduleID(_ScheduleChnlDetailID, _OutletID);
        }
        public List<object> GetOutletCheckingAttachment(long _OCID)
        {
            return db.ExtRepositoryFor<ScheduleInspectionRepository>().GetOutletCheckingAttachment(_OCID);
        }
        
        public string AddRotationalViolations(int _UserID, double? _GaugeValue, string _ImagePath, string _Remarks, double? _Longitude, double? _Latitude, DateTime mobdt, long _ChannelID, bool? _isViolation, short _Grouppref)
        {
            return dalScheduleInspection.AddRotationalViolations(_UserID, _GaugeValue, _ImagePath, _Remarks, _Longitude, _Latitude, mobdt, _ChannelID, _isViolation, _Grouppref);
        }


        public bool AddOutletChecking(SI_OutletChecking _oc)
        {
            return dalScheduleInspection.AddOutletChecking(_oc);
        }

        public SI_OutletChecking GetOutletCheckingByID(long _OutletCheckingID)
        {
            return dalScheduleInspection.GetOutletCheckingByID(_OutletCheckingID);
        }

        public List<object> GetOutletsByUserID(long _UserID)
        {
            return dalScheduleInspection.GetOutletsByUserID(_UserID);
        }
        public List<object> GetOutletByOutletID_OR_OutletName(long _OutletID=0,string OutletName="")
        {
            return dalScheduleInspection.GetOutletByOutletID_OR_OutletName(_OutletID, OutletName);
        }

        //public SI_OutletChecking GetOutletCheckingByScheduleDetailChannelID()
    }
}
