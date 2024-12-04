using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Services.HelperClasses;
using System;
using PMIU.WRMIS.Services.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace PMIU.WRMIS.Services.Controllers
{


    [RoutePrefix("api/ScheduleInspectionApi")]
    public class ScheduleInspectionController : ApiController
    {
        private const string MSG_UNKNOWN_ERROR = "Some unknown error occurred, please try again later";
        private const string MSG_NO_RECORD_FOUND = "FAILURE: No Records Found";
        private const string MSG_RECORD_SAVED = "SUCCESS: Record saved successfully";
        private const string MSG_SUCCESS = "SUCCESS:";

        PMIU.WRMIS.BLL.ScheduleInspection.ScheduleInspectionBLL bllScheduleInspection = new BLL.ScheduleInspection.ScheduleInspectionBLL();
        PMIU.WRMIS.BLL.WaterTheft.WaterTheftBLL bllWaterTheft = new BLL.WaterTheft.WaterTheftBLL();
        ServiceHelper srvcHlpr = new ServiceHelper();

        [HttpGet]
        [Route("GetUserSchedule")]
        public ServiceResponse GetUserSchedule(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<SI_GetUserSchedule_Result> lstSchedule = bllScheduleInspection.GetUserSchedule(_UserID);
                if (lstSchedule.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<SI_GetUserSchedule_Result>(lstSchedule, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetUserSchedule", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetInspectionDetailByTypeID")]
        public ServiceResponse GetInspectionDetailByTypeID(int _InspectionType, int _GaugeID, long _ScheduleDetailID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string serverUploadFolder = System.Configuration.ConfigurationManager.AppSettings["BasePath"].ToString();
                object GaugeData = bllScheduleInspection.GetInspectionDetailByTypeID(_InspectionType, _GaugeID, _ScheduleDetailID, serverUploadFolder);
             //   List<SI_GetUserSchedule_Result> lstSchedule = bllScheduleInspection.GetUserSchedule(_UserID);
                if (GaugeData!=null)
                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, GaugeData);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetUserSchedule", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetInspectionTypes")]
        public ServiceResponse GetInspectionTypes(long _ScheduleID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<SI_ScheduleTypes_Result> lstInspectionTypes = bllScheduleInspection.GetScheduleInspectionTypes(_ScheduleID);
                if (lstInspectionTypes.Count > 0)
                {
                    svcResponse.StatusCode = 0;
                    svcResponse.StatusMessage = "SUCCESS:";
                    svcResponse.Data = lstInspectionTypes;
                }
                else
                {
                    svcResponse.StatusCode = 1001;
                    svcResponse.StatusMessage = "FAILURE: No Records Found.";
                    svcResponse.Data = null;
                }
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetInspectionTypes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetInspectionNotes")]
        public ServiceResponse GetInspectionNotesByInspectionType(long _ScheduleID, long _InspectionTypeID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
               
                if (_InspectionTypeID == 1 || _InspectionTypeID == 2)
                {
                    List<SI_GetGaugeInspectionAreas_Result> lstInspectionNotes = new List<SI_GetGaugeInspectionAreas_Result>();
                    lstInspectionNotes = bllScheduleInspection.GetGaugesInspectionAreasRecords(_ScheduleID, _InspectionTypeID);
                    if (lstInspectionNotes.Count > 0)
                        svcResponse = srvcHlpr.CreateResponse<SI_GetGaugeInspectionAreas_Result>(lstInspectionNotes, 0, ServiceHelper.MSG_SUCCESS);
                    else
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
                else if (_InspectionTypeID == 3 || _InspectionTypeID == 4)
                {
                    List<SI_GetOutletInspectionAreas_Result> lstInspectionNotes = new List<SI_GetOutletInspectionAreas_Result>();
                    lstInspectionNotes = bllScheduleInspection.GetOutletsInspectionAreasRecord(_ScheduleID, _InspectionTypeID);
                    if (lstInspectionNotes.Count > 0)
                        svcResponse = srvcHlpr.CreateResponse<SI_GetOutletInspectionAreas_Result>(lstInspectionNotes, 0, ServiceHelper.MSG_SUCCESS);
                    else
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }

                
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetInspectionNotes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetOutletDetails")]
        public ServiceResponse GetOutlets(long _OutletID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<SI_GetOutletDetail_Result> LstOutletDetail = bllScheduleInspection.GetOutletByOutletID(_OutletID);
                svcResponse = srvcHlpr.CreateResponse<SI_GetOutletDetail_Result>(LstOutletDetail, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Add discharge measurement inspection parameters for a gauge in DB server
        /// depending on gauge level insertion in different tables occurs
        /// </summary>
        /// <param name="_DschrgTableCalcParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDischargeTableCalculation")]
        public ServiceResponse AddGaugeDischargeMeasurements([FromBody] Dictionary<string, object> _DschrgTableCalcParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_DschrgTableCalcParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);
                string _ObservationDate = _DschrgTableCalcParams["_ObservationDate"].ToString();
                

                long _UserID = Convert.ToInt64(_DschrgTableCalcParams["_UserID"].ToString());
                long _GaugeID = Convert.ToInt64(_DschrgTableCalcParams["_GaugeID"].ToString());
                long _GaugeLevel = Convert.ToInt64(_DschrgTableCalcParams["_GaugeLevel"].ToString());
                
                long? _ScheduleDetailChannelID = null;
                string schChnlDeID = _DschrgTableCalcParams["_ScheduleChannelDetailID"].ToString();
                if (!string.IsNullOrEmpty(schChnlDeID) && !schChnlDeID.Equals("null"))
                    _ScheduleDetailChannelID = Convert.ToInt64(_DschrgTableCalcParams["_ScheduleChannelDetailID"].ToString());

                double _ParamN_B = Convert.ToDouble(_DschrgTableCalcParams["_ParamN_B"].ToString());
                double _ParamD_H = Convert.ToDouble(_DschrgTableCalcParams["_ParamD_H"].ToString());
                double _ObsrvdDschrg = Convert.ToDouble(_DschrgTableCalcParams["_ObsrvdDschrg"].ToString());

                String _Remarks = _DschrgTableCalcParams["_Remarks"].ToString();

                int _GCorrectType =  Convert.ToInt32(_DschrgTableCalcParams["_GCorrectType"].ToString());
               
                double? _Longitude = null, _Latitude = null;
                if (_DschrgTableCalcParams["_Longitude"] != null && !string.IsNullOrEmpty(_DschrgTableCalcParams["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_DschrgTableCalcParams["_Longitude"].ToString());

                if (_DschrgTableCalcParams["_Latitude"] != null && !string.IsNullOrEmpty(_DschrgTableCalcParams["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_DschrgTableCalcParams["_Latitude"].ToString());

                bool result = false;
                
                if (_DschrgTableCalcParams["_Longitude"] != null && !string.IsNullOrEmpty(_DschrgTableCalcParams["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_DschrgTableCalcParams["_Longitude"].ToString());

                if (_DschrgTableCalcParams["_Latitude"] != null && !string.IsNullOrEmpty(_DschrgTableCalcParams["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_DschrgTableCalcParams["_Latitude"].ToString());
                if (_GaugeLevel == 1) //Bed Level Gauge
                {
                    double? _GCorrectValue = null;
                    string correctVal = _DschrgTableCalcParams["_GCorrectValue"].ToString();
                    if (!string.IsNullOrEmpty(correctVal) && !correctVal.Equals("null"))
                        _GCorrectValue = Convert.ToDouble(_DschrgTableCalcParams["_GCorrectValue"].ToString());


                    result = bllScheduleInspection.AddGauge_BedLevel_DischargeMeasurements(_UserID, _GaugeID, _ParamN_B, _ParamD_H, _ObsrvdDschrg, _GCorrectType, _GCorrectValue, _ScheduleDetailChannelID, _Remarks, _Longitude, _Latitude, "A", _ObservationDate);
                }
                else // Crest Level Gauge
                {
                    result = bllScheduleInspection.AddGauge_CrestLevel_DischargeMeasurements(_UserID, _GaugeID, _ParamN_B, _ParamD_H, _ObsrvdDschrg, _ScheduleDetailChannelID, _Remarks, _Longitude, _Latitude, "A", _ObservationDate);
                }

                if (result) {
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);

                    if (_GaugeLevel == 1) //Bed Level Gauge
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("GaugeID", _GaugeID);
                        _event.Parameters.Add("IsCrestParameters", "false");
                        _event.AddNotifyEvent((long)NotificationEventConstants.IrrigationNetwork.EditBedLevelParameters, _UserID);


                        if (_ScheduleDetailChannelID == null || _ScheduleDetailChannelID == 0)
                        {
                            NotifyEvent schnotify = new NotifyEvent();
                            schnotify.Parameters.Add("_GaugeID", _GaugeID);
                            schnotify.Parameters.Add("ScheduleDetailID", _ScheduleDetailChannelID);
                            schnotify.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMAssignedToDDForApproval, _UserID);
                        }


                        else
                        {
                            NotifyEvent schnotify = new NotifyEvent();
                            schnotify.Parameters.Add("ScheduleDetailID", _ScheduleDetailChannelID);
                            schnotify.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.InspectionOfADMAssignedToDDForApproval, _UserID);
                        }

                    }
                    else
                    {
                        if (_ScheduleDetailChannelID == null || _ScheduleDetailChannelID == 0)
                        {
                            NotifyEvent schnotify = new NotifyEvent();
                            schnotify.Parameters.Add("_GaugeID", _GaugeID);
                            schnotify.Parameters.Add("ScheduleDetailID", _ScheduleDetailChannelID);
                            schnotify.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMCrestLevelAssignedToDDForApproval, _UserID);
                        }
                        else
                        {
                            NotifyEvent schnotify = new NotifyEvent();
                            schnotify.Parameters.Add("ScheduleDetailID", _ScheduleDetailChannelID);
                            schnotify.AddNotifyEvent((long)NotificationEventConstants.ScheduleInspection.InspectionOfADMCrestLevelAssignedToDDForApproval, _UserID);
                            
                        }

                        
                    }
                    
                }
                   
               else
                   svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
            }
            catch (Exception exp)
            {
                //TODO: Log exception
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Add outlet alteration inspection parameters for a outlet in DB server
        /// </summary>
        /// <param name="_OutletAlterParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOutletAlteration")]

         public ServiceResponse AddOutletAlteration([FromBody] Dictionary<string, object> _OutletAlterParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_OutletAlterParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_OutletAlterParams["_UserID"].ToString());

                
                long _OutletID = Convert.ToInt64(_OutletAlterParams["_OutletID"].ToString());

                long? _ScheduleDetailChannelID = null;
                string schChnlDeID = _OutletAlterParams["_ScheduleDetailChannelID"].ToString();
                if (!string.IsNullOrEmpty(schChnlDeID) && !schChnlDeID.Equals("null"))
                    _ScheduleDetailChannelID = Convert.ToInt64(_OutletAlterParams["_ScheduleDetailChannelID"].ToString());

                int _isOnline = Convert.ToInt32(_OutletAlterParams["_isOnline"].ToString());
                string _AlterationDT = _OutletAlterParams["_AlterationDT"].ToString();
                DateTime alterationdt;

                if (_isOnline == 1)
                {
                    alterationdt = DateTime.Now;
                }
                else
                {
                    alterationdt = Convert.ToDateTime(_AlterationDT);
                }
                
                double? _HeadAboveCrest = null, _WorkingHead = null, _HeightOfOutlet = null, _DiameterBreadthWidth = null;
                int? _OutletTypeID = null;
                string _Remarks = null, _OutletStatus = null;

                string crest = _OutletAlterParams["_HeadAboveCrest"].ToString();
                if (!string.IsNullOrEmpty(crest) && !crest.Equals("null"))
                    _HeadAboveCrest = Convert.ToDouble(_OutletAlterParams["_HeadAboveCrest"].ToString());

                string head = _OutletAlterParams["_WorkingHead"].ToString();
                if (!string.IsNullOrEmpty(head) && !head.Equals("null"))
                    _WorkingHead = Convert.ToDouble(_OutletAlterParams["_WorkingHead"].ToString());

                string height = _OutletAlterParams["_HeightOfOutlet"].ToString();
                if (!string.IsNullOrEmpty(height) && !height.Equals("null"))
                    _HeightOfOutlet = Convert.ToDouble(_OutletAlterParams["_HeightOfOutlet"].ToString());

                string width = _OutletAlterParams["_DiameterBreadthWidth"].ToString();
                if (!string.IsNullOrEmpty(width) && !width.Equals("null"))
                    _DiameterBreadthWidth = Convert.ToDouble(_OutletAlterParams["_DiameterBreadthWidth"].ToString());

                string outletType = _OutletAlterParams["_OutletTypeID"].ToString();
                if (!string.IsNullOrEmpty(outletType) && !outletType.Equals("null"))
                    _OutletTypeID = Convert.ToInt32(_OutletAlterParams["_OutletTypeID"].ToString());

                _Remarks = _OutletAlterParams["_Remarks"].ToString();
                _OutletStatus = "";

                 if (!string.IsNullOrEmpty(_OutletAlterParams["_OutletStatus"].ToString()) && !_OutletAlterParams["_OutletStatus"].ToString().Equals("null"))
                     _OutletStatus=_OutletAlterParams["_OutletStatus"].ToString();

                bool result = false;

                double? _Longitude = null, _Latitude = null;
                if (_OutletAlterParams["_Longitude"] != null && !string.IsNullOrEmpty(_OutletAlterParams["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_OutletAlterParams["_Longitude"].ToString());

                if (_OutletAlterParams["_Latitude"] != null && !string.IsNullOrEmpty(_OutletAlterParams["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_OutletAlterParams["_Latitude"].ToString());

                result = bllScheduleInspection.AddOutletAlteration(_UserID, _OutletID, _HeightOfOutlet, _HeadAboveCrest, _WorkingHead, _DiameterBreadthWidth, _ScheduleDetailChannelID, _Remarks, _OutletTypeID, _OutletStatus, alterationdt);

                if (result)
                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_SAVED, null);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
            }
            catch (Exception exp)
            {
                //TODO: Log exception
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }


        /// <summary>
        /// Add outlet performance inspection parameters for a outlet in DB server
        /// </summary>
        /// <param name="_OutletPrformncParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOutletPerformance")]
        public ServiceResponse AddOutletPerformance([FromBody] Dictionary<string, object> _OutletPrformncParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
               
                if (srvcHlpr.IsDictionaryEmptyOrNull(_OutletPrformncParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                long _UserID = Convert.ToInt64(_OutletPrformncParams["_UserID"].ToString());
                long _OutletID = Convert.ToInt64(_OutletPrformncParams["_OutletID"].ToString());
                long? _ScheduleDetailChannelID = null;
                string schChnlDeID = _OutletPrformncParams["_ScheduleDetailChannelID"].ToString();
                if (!string.IsNullOrEmpty(schChnlDeID) && !schChnlDeID.Equals("null"))
                    _ScheduleDetailChannelID = Convert.ToInt64(_OutletPrformncParams["_ScheduleDetailChannelID"].ToString());

                int _isOnline = Convert.ToInt32(_OutletPrformncParams["_isOnline"].ToString());
                string _OutletPerDT = _OutletPrformncParams["_OutletPerDT"].ToString();
                DateTime outletperfdt;

                if (_isOnline == 1)
                {
                    outletperfdt = DateTime.Now;
                }
                else
                {
                    outletperfdt = Convert.ToDateTime(_OutletPerDT);
                }



                double? _HeadAboveCrest = null, _WorkingHead = null, _DiameterBreadthWidth = null, _HeightOfOutlet = null, _ObsrvdDschrg = null; 
                string crest = _OutletPrformncParams["_HeadAboveCrest"].ToString();
                if (!string.IsNullOrEmpty(crest) && !crest.Equals("null"))
                    _HeadAboveCrest = Convert.ToDouble(_OutletPrformncParams["_HeadAboveCrest"].ToString());

                string width = _OutletPrformncParams["_DiameterBreadthWidth"].ToString();
                if (!string.IsNullOrEmpty(width) && !width.Equals("null"))
                    _DiameterBreadthWidth = Convert.ToDouble(_OutletPrformncParams["_DiameterBreadthWidth"].ToString());

                string height = _OutletPrformncParams["_HeightOfOutlet"].ToString();
                if (!string.IsNullOrEmpty(height) && !height.Equals("null"))
                    _HeightOfOutlet = Convert.ToDouble(_OutletPrformncParams["_HeightOfOutlet"].ToString());

                string workingHead = _OutletPrformncParams["_WorkingHead"].ToString();
                if (!string.IsNullOrEmpty(workingHead) && !workingHead.Equals("null"))
                    _WorkingHead = Convert.ToDouble(_OutletPrformncParams["_WorkingHead"].ToString());

                string observedDischarge = _OutletPrformncParams["_ObsrvdDschrg"].ToString();
                if (!string.IsNullOrEmpty(workingHead) && !workingHead.Equals("null"))
                    _ObsrvdDschrg = Convert.ToDouble(_OutletPrformncParams["_ObsrvdDschrg"].ToString());

             //   _ObsrvdDschrg = Convert.ToDouble(_OutletPrformncParams["_ObsrvdDschrg"].ToString());

                string _Remarks = _OutletPrformncParams["_Remarks"].ToString();

                bool result = false;

                double? _Longitude = null, _Latitude = null;
                if (_OutletPrformncParams["_Longitude"] != null && !string.IsNullOrEmpty(_OutletPrformncParams["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_OutletPrformncParams["_Longitude"].ToString());

                if (_OutletPrformncParams["_Latitude"] != null && !string.IsNullOrEmpty(_OutletPrformncParams["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_OutletPrformncParams["_Latitude"].ToString());

                result = bllScheduleInspection.AddOutletPerformance(_UserID, _OutletID, _HeadAboveCrest, _WorkingHead, _ObsrvdDschrg, _DiameterBreadthWidth, _HeightOfOutlet, _Remarks, _ScheduleDetailChannelID, _Longitude, _Latitude, "A", outletperfdt);

                if (result)
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
            }
            catch (Exception exp)
            {
                //TODO: Log exception
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }






        [HttpGet]
        [Route("GetOutletAlterationHistory")]
        public ServiceResponse GetOutletAlterationHistory(long _OutletID, long _ScheduleDetailChannelID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                object OutletDetail = bllScheduleInspection.GetOutletAlterationHistoryDetail(_ScheduleDetailChannelID, _OutletID);
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, OutletDetail);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetOutletPerformaceDetail")]
        public ServiceResponse GetOutletPerformaceDetail(long _OutletID, long _ScheduleDetailChannelID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                object OutletDetail = bllScheduleInspection.GetOutletAlterationPerformanceDetail(_ScheduleDetailChannelID, _OutletID);
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, OutletDetail);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }




        [HttpGet]
        [Route("GetScheduledInspectionsByMonth")]
        public ServiceResponse GetScheduledInspectionsByMonth(string _MonthParam,string _UserParam)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstSchedule = bllScheduleInspection.GetScheduledInspectionsByMonth(_MonthParam, _UserParam);
                if (lstSchedule.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstSchedule, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetScheduledInspectionsByMonth", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetScheduledInspectionsByDate")]
        public ServiceResponse GetScheduledInspectionsByDate(string _DateParam, string _UserParam)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstSchedule = bllScheduleInspection.GetScheduledInspectionsByDate(_DateParam, _UserParam);
                if (lstSchedule.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstSchedule, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetScheduledInspectionsByDate", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// provide with the values required for a daily gauge reading value
        /// data is saved in the valid table
        /// </summary>
        /// <param name="_GaugeValues"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddGaugeValue")]
        public ServiceResponse AddGaugeValue([FromBody] Dictionary<string, object> _GaugeValues)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            PMIU.WRMIS.BLL.ComplaintsManagement.ComplaintsManagementBLL bllComplaintManagement = new BLL.ComplaintsManagement.ComplaintsManagementBLL();
            PMIU.WRMIS.BLL.DailyData.DailyDataBLL bllDailyDataGauge = new BLL.DailyData.DailyDataBLL();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_GaugeValues))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null", null);

                long _UserID = Convert.ToInt64(_GaugeValues["_UserID"].ToString());
                long _GaugeID = Convert.ToInt64(_GaugeValues["_GaugeID"].ToString());
                bool _GaugeF = Convert.ToBoolean(_GaugeValues["_GaugeF"].ToString());
                bool _GaugeP = Convert.ToBoolean(_GaugeValues["_GaugeP"].ToString());
                double? _GaugeValue=null;
                double? _dsrg = null;
                if (_GaugeF && _GaugeP)
                {
                    string gaugeVal = _GaugeValues["_GaugeValue"].ToString();
                    if (!string.IsNullOrEmpty(gaugeVal) && !gaugeVal.Equals("null"))
                    {
                        _GaugeValue = Convert.ToDouble(gaugeVal);
                        _dsrg = bllDailyDataGauge.CalculateDischarge(_GaugeID, Convert.ToDouble(gaugeVal));
                    }

                    
                }

                int _isOnline = Convert.ToInt32(_GaugeValues["_isOnline"].ToString());
                string _ReadingTime = _GaugeValues["_ReadingTime"].ToString();

                DateTime readingdt = Convert.ToDateTime(_ReadingTime);

                //string iString = "2005-05-05 22:12 PM";
                //DateTime readingdt = DateTime.ParseExact(_ReadingTime, "yyyy-MM-dd HH:mm tt", null);
                //MessageBox.Show(oDate.ToString());

                //DateTime readingdt = dt.Add(DateTime.Now.TimeOfDay);
                
                
                string _Remarks = _GaugeValues["_Remarks"].ToString();

                double? _Longitude = null, _Latitude = null;
                if (_GaugeValues["_Longitude"] != null && !string.IsNullOrEmpty(_GaugeValues["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_GaugeValues["_Longitude"].ToString());

                if (_GaugeValues["_Latitude"] != null && !string.IsNullOrEmpty(_GaugeValues["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_GaugeValues["_Latitude"].ToString());

                string _ImagePath = _GaugeValues["_ImagePath"].ToString();
                string decodedText = _Remarks;
                long gaugeReadingID =-1 ;

                
               string _NewImageName="";
                long? _SchChnlDetID = null;
                string _schchnlID = _GaugeValues["_ScheduledID"].ToString();
                if (!string.IsNullOrEmpty(_schchnlID) && !_schchnlID.Equals("null"))
                    _SchChnlDetID = Convert.ToInt64(_schchnlID);

                CO_ChannelGauge _ObjChannelGauge = new ChannelBLL().GetChannelGaugeByID(_GaugeID);
                bool isChannelClosed = new PMIU.WRMIS.BLL.DailyData.DailyDataBLL().IsChannelClosedNow(Convert.ToInt64(_ObjChannelGauge.ChannelID), System.DateTime.Now);

                

               long? _DesignationID = bllScheduleInspection.GetUserDesignationID(_UserID);
               // long? _DesignationID = 12;
                SI_ChannelGaugeReading mdlGaugeReading = new SI_ChannelGaugeReading();
                mdlGaugeReading.GaugeID = _GaugeID;
                mdlGaugeReading.ChannelClosed = isChannelClosed;
                if (_isOnline == 1)
                {
                    mdlGaugeReading.ReadingDateTime = DateTime.Now;
                }
                else
                {
                    mdlGaugeReading.ReadingDateTime = readingdt;
                }
                mdlGaugeReading.GaugeReaderID = _UserID;
                mdlGaugeReading.GaugeValue = _GaugeValue;
                mdlGaugeReading.Remarks = _Remarks;
                mdlGaugeReading.GisX = _Longitude;
                mdlGaugeReading.GisY = _Latitude;
                mdlGaugeReading.DesignationID = _DesignationID;
                mdlGaugeReading.Source = "A";
                mdlGaugeReading.ScheduleDetailChannelID = _SchChnlDetID;
              
                
        /*        double? dsrg = _dsrg;
                if (dsrg == null)
                    mdlGaugeReading.DailyDischarge = 0;
                else
                    mdlGaugeReading.DailyDischarge = Convert.ToDouble(dsrg);
                */
                string dateFormat = "yyyyMMdd-hhmmss";
                string guid = _ImagePath.Split('.')[0];
                string _date = DateTime.Now.ToString(dateFormat);
                if (_SchChnlDetID != null)
                {
                    SI_Schedule schDetail = bllScheduleInspection.GetScheduleDetailByScheduleDetailID(Convert.ToInt64(_SchChnlDetID));
                    _NewImageName = guid+"_"+(schDetail.Name).Replace(" ", "") + "-" + _date;
                }
                else
                {
                    _NewImageName = guid + "_" + "GaugeInspection" + "-" + _date;
                }
              
                string filePath = Utility.GetImagePath(Common.Configuration.ScheduleInspection);
                filePath = filePath + "\\" + _ImagePath;
                String newfilePath = Utility.GetImagePath(Common.Configuration.ScheduleInspection) +"\\"+_NewImageName+".png";
                if (File.Exists(filePath))
                    System.IO.File.Move(@filePath, @newfilePath);

                if (File.Exists(newfilePath)) {
                    _ImagePath = _NewImageName + ".png";
                }
                //mdlGaugeReading.ChannelClosed = isChannelClosed;
                //mdlGaugeReading.DailyDischarge = _dsrg;
                mdlGaugeReading.IsGaugeFixed = _GaugeF;
                mdlGaugeReading.IsGaugePainted = _GaugeP;
                mdlGaugeReading.GaugePhoto = _ImagePath;
                mdlGaugeReading.CreatedDate = DateTime.Today;
                mdlGaugeReading.CreatedBy = Convert.ToInt32(_UserID);

                if (Convert.ToDouble(_dsrg) == 0 || Convert.ToDouble(_dsrg) == 0.0 || _dsrg == 0 || _dsrg == 0.0 || _dsrg == null)
                {
                    mdlGaugeReading.DailyDischarge = 0;
                    mdlGaugeReading.ChannelClosed = true;
                }
                else
                {
                    mdlGaugeReading.DailyDischarge = Convert.ToDouble(_dsrg);
                    mdlGaugeReading.ChannelClosed = false;
                }

                PMIU.WRMIS.BLL.UserAdministration.UserBLL bllUser = new UserBLL();
                UA_Users mdlUser = bllUser.GetUserByID(_UserID);
                long _DivisionID = Convert.ToInt64(bllScheduleInspection.GetDivisionIDByGaugeID(_GaugeID));
                bool? IsComplaintGenerated = false;
                List<string> ComplaintIDs = new List<string>();

                gaugeReadingID = bllScheduleInspection.SaveGaugeReadingData(mdlGaugeReading, mdlUser, _DivisionID, ref IsComplaintGenerated, ref ComplaintIDs);
               /* if (IsComplaintGenerated == true)
                {
                    string IDs = string.Join(",", ComplaintIDs);
                    string Message = "Complaint(s) : " + IDs + " has been generated.";
                    
                } */
                if(bllScheduleInspection.IsGaugeReadingExists(gaugeReadingID))
                     svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddGaugeValue", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, exp.Message, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Use to upload files to server
        /// Recieve file from client and after assigning a name
        /// write it to app server
        /// </summary>
        /// <returns></returns>
        [Route("UploadFiles")]
        [HttpPost]
        public async Task<object> UploadMultipleFiles()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string serverUploadFolder = System.Configuration.ConfigurationManager.AppSettings["BasePath"].ToString();
                string filePath = Utility.GetImagePath(Common.Configuration.ScheduleInspection);
                var httpRequest = HttpContext.Current.Request;
                var fileName = string.Empty;
                foreach (string file in httpRequest.Files)
                {
                    string guid = Guid.NewGuid().ToString();
                    var postedFile = httpRequest.Files[file];
  
                    fileName = guid + ".png";// +postedFile.FileName;
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    filePath = filePath + "\\" + fileName;
                    postedFile.SaveAs(filePath);
                }
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, fileName);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("UploadFiles : - ", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1001, "Image Not Uploaded.", "" + exp.Message);
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetCurrentTime")]
        public ServiceResponse GetInspectionTypes()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                string dateFormat = "MM.dd.yyyy";
                string _date = DateTime.Now.ToString(dateFormat);
                if (_date != null)
                {
                    svcResponse.StatusCode = 0;
                    svcResponse.StatusMessage = "SUCCESS:";
                    svcResponse.Data = _date;
                }
                else
                {
                    svcResponse.StatusCode = 1001;
                    svcResponse.StatusMessage = "FAILURE: No Records Found.";
                    svcResponse.Data = null;
                }
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetInspectionTypes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetAllGeneralInspectionTypes")]
        public ServiceResponse GetAllGeneralInspectionTypes()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstInspectionTypes = bllScheduleInspection.GetGeneralInspectionTypeList();
                if (lstInspectionTypes.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionTypes, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllGeneralInspectionTypes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetGeneralInspectionByID")]
        public ServiceResponse GetGeneralInspectionByID(long _ScheduleIDParam, long _TypeParam)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                object Inspection = bllScheduleInspection.GetGeneralInspectionByID(_ScheduleIDParam, _TypeParam);
                if (Inspection != null)
                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS,Inspection);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetGeneralInspectionByID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// provide with the values required for a daily gauge reading value
        /// data is saved in the valid table
        /// </summary>
        /// <param name="_GaugeValues"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddGeneralInspection")]
        public ServiceResponse AddGeneralInspection([FromBody] Dictionary<string, object> _InspectionValues)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            PMIU.WRMIS.BLL.ComplaintsManagement.ComplaintsManagementBLL bllComplaintManagement = new BLL.ComplaintsManagement.ComplaintsManagementBLL();
            PMIU.WRMIS.BLL.DailyData.DailyDataBLL bllDailyDataGauge = new BLL.DailyData.DailyDataBLL();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_InspectionValues))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null", null);


                int _UserID = Convert.ToInt32(_InspectionValues["_UserID"].ToString());
                long _GeneralInspectionTypeID = Convert.ToInt64(_InspectionValues["_GeneralInspectionTypeID"].ToString());
                
                string _GeneralInspectionTypeName = _InspectionValues["_GeneralInspectionTypeName"].ToString();

                int _isOnline = Convert.ToInt32(_InspectionValues["_isOnline"].ToString());

                long? _ScheduleDetailID = null;

                if (_InspectionValues.ContainsKey("_ScheduleDetailID"))
                {
                    string schDetailID = _InspectionValues["_ScheduleDetailID"].ToString();
                    if (!string.IsNullOrEmpty(schDetailID) && !schDetailID.Equals("null"))
                    {
                        _ScheduleDetailID = Convert.ToInt64(schDetailID);

                    }
                }

                string _InspectionDetails = _InspectionValues["_InspectionDetails"].ToString();
                string _InspectionLocation = _InspectionValues["_InspectionLocation"].ToString();
                string _Remarks = _InspectionValues["_Remarks"].ToString();

                
                string _InspectionDate = _InspectionValues["_InspectionDate"].ToString();
                DateTime readingdt;
                if (_isOnline == 1)
                {
                    readingdt = DateTime.Now;
                }
                else
                {
                    readingdt = Convert.ToDateTime(_InspectionDate);
                }

                string attachments = null;
                string atchmnts = _InspectionValues["_Attachments"].ToString();
                if (!string.IsNullOrEmpty(atchmnts) && !atchmnts.Equals("null"))
                    attachments = atchmnts;



                //link attachments to GeneralInspection if there any
                string dateFormat = "yyyyMMdd-hhmmss";
                string _date = DateTime.Now.ToString(dateFormat);
                List<Tuple<string, string, string>> lstNameofFiles = new List<Tuple<string, string, string>>();
                string _imagePath = "";
                if (attachments != null && attachments.Length > 0)
                {
                    string[] lstAttachments = attachments.Split(',');
                    if (lstAttachments != null && lstAttachments.Length > 0)
                    {
                        foreach (string attchmntName in lstAttachments)
                        {
                            if (attchmntName.Length > 0)
                            {
                                try
                                {
                                    string guid = attchmntName.Split('.')[0];
                                    string _NewImageName = guid + "_" + _GeneralInspectionTypeName + "-" + _date;
                                    string filePath = Utility.GetImagePath(Common.Configuration.ScheduleInspection);
                                    filePath = filePath + "\\" + attchmntName;
                                    String newfilePath = Utility.GetImagePath(Common.Configuration.ScheduleInspection) + "\\" + _NewImageName + ".png";
                                    if (File.Exists(filePath))
                                        System.IO.File.Move(@filePath, @newfilePath);

                                    if (File.Exists(newfilePath))
                                    {
                                        _imagePath = _NewImageName + ".png";
                                    }
                                    else
                                    {
                                        _imagePath = attchmntName;
                                    }
                                }
                                catch (Exception e)
                                {
                                    _imagePath = attchmntName;
                                }

                                lstNameofFiles.Add(new Tuple<string, string, string>("", "", _imagePath));

                            }
                        }
                    }
                }

                long InspectionID = bllScheduleInspection.AddGeneralInspection(_ScheduleDetailID, _GeneralInspectionTypeID, _InspectionLocation, readingdt, _InspectionDetails, _Remarks, lstNameofFiles, _UserID);

                if (InspectionID>0)
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddGeneralInspection", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, exp.Message, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Add water theft case for an outlet in systems and prform necassary steps 
        /// </summary>
        /// <param name="_TheftDetail"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOutletChecking")]
        public ServiceResponse AddOutletChecking([FromBody] Dictionary<string, object> _TheftDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_TheftDetail))
                    return srvcHlpr.CreateResponse(1004, "Parameters are null", null);

                long userID = Convert.ToInt64(_TheftDetail["_UserID"].ToString());
                long channelID = Convert.ToInt64(_TheftDetail["_ChannelID"].ToString());
                int siteRD = Convert.ToInt32(_TheftDetail["_SiteRD"].ToString());
                long outletID = Convert.ToInt64(_TheftDetail["_OutletID"].ToString());
                string channelSide = _TheftDetail["_ChannelSide"].ToString();
                
                string   conditionName = _TheftDetail["conditionName"].ToString();

                

                    string remarks = null;
                    if (_TheftDetail["_Remarks"] != null)
                        remarks = _TheftDetail["_Remarks"].ToString();

                    string attachments = null;
                    string atchmnts = _TheftDetail["_Attachments"].ToString();
                    if (!string.IsNullOrEmpty(atchmnts) && !atchmnts.Equals("null"))
                        attachments = atchmnts;

                    double? valueOfH = null;
                    if (_TheftDetail["_ValueOfH"] != null && !_TheftDetail["_ValueOfH"].ToString().Equals("null") && !string.IsNullOrEmpty(_TheftDetail["_ValueOfH"].ToString()))
                        valueOfH = Convert.ToDouble(_TheftDetail["_ValueOfH"].ToString());


                    double? _Longitude = null, _Latitude = null;
                    if (_TheftDetail["_Longitude"] != null && !string.IsNullOrEmpty(_TheftDetail["_Longitude"].ToString()))
                        _Longitude = Convert.ToDouble(_TheftDetail["_Longitude"].ToString());

                    if (_TheftDetail["_Latitude"] != null && !string.IsNullOrEmpty(_TheftDetail["_Latitude"].ToString()))
                        _Latitude = Convert.ToDouble(_TheftDetail["_Latitude"].ToString());

                    int _isOnline = Convert.ToInt32(_TheftDetail["_isOnline"].ToString());
                    string _DateTime = _TheftDetail["_DateTime"].ToString();

                    DateTime Outletdt;

                    if (_isOnline == 1)
                    {
                        Outletdt = DateTime.Now;
                    }
                    else
                    {
                        Outletdt = Convert.ToDateTime(_DateTime);
                    }

                    long? _ScheduleDetailChannelID = null;
                    string schChnlDeID = _TheftDetail["_ScheduleDetailChannelID"].ToString();
                    if (!string.IsNullOrEmpty(schChnlDeID) && !schChnlDeID.Equals("null"))
                        _ScheduleDetailChannelID = Convert.ToInt64(_TheftDetail["_ScheduleDetailChannelID"].ToString());


                    long _DivisionID = Convert.ToInt64(_TheftDetail["_DivisionID"].ToString());
                    string Attachment="";

                     if (attachments != null && attachments.Length > 0)
                    {
                        string[] lstAttachments = attachments.Split(',');
                         Attachment=lstAttachments[0];
                    }
                    SI_OutletChecking mdlOutletChecking = new SI_OutletChecking();

                    mdlOutletChecking.OutletCheckCondition = conditionName;
                    mdlOutletChecking.HValue=valueOfH;
                    mdlOutletChecking.OutletID =outletID;
                    mdlOutletChecking.ReadingMobileDate=Outletdt;
                    mdlOutletChecking.Remarks=remarks;
                    mdlOutletChecking.ScheduleDetailChannelID=_ScheduleDetailChannelID;
                    mdlOutletChecking.Attachment=Attachment;             
                    mdlOutletChecking.CreatedDate = DateTime.Now;
                    mdlOutletChecking.CreatedBy = userID;
                    mdlOutletChecking.GisX = _Longitude;
                    mdlOutletChecking.GisY = _Latitude;
                    mdlOutletChecking.Source = "A";
                    bool result=bllScheduleInspection.AddOutletChecking(mdlOutletChecking);
                    if (result)
	                
		                 svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_SAVED, null);
	                else
                         svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                   
                    

               
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddOutletChecking", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR + "\n" + exp.Message, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        private double? GetValue(Dictionary<string, object> _DataContainer, string _Key)
        {
            if (_DataContainer.ContainsKey(_Key))
            {
                if (_DataContainer[_Key] != null)
                {
                    string key = _DataContainer[_Key].ToString();

                    if (string.IsNullOrEmpty(key) || key.Equals("null"))
                        return null;

                    return Convert.ToDouble(_DataContainer[_Key].ToString());
                }
            }
            return null;
        }

        [HttpGet]
        [Route("GetLeaveTypes")]
        public ServiceResponse GetLeaveTypes()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstLeaveTypes = bllScheduleInspection.GetLeavesTypes();
                if (lstLeaveTypes.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstLeaveTypes, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetLeaveTypes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpPost]
        [Route("AddLeaveForm")]
        public ServiceResponse AddLeaveForm([FromBody] Dictionary<string, object> _LeaveValues)
        {
            ServiceResponse svcResponse = new ServiceResponse();

           
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_LeaveValues))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null", null);
                
                int _UserID = Convert.ToInt32(_LeaveValues["_UserID"].ToString());
                short _LeaveTypeID = Convert.ToInt16(_LeaveValues["_LeaveTypeID"].ToString());
                double? _GaugeValue = null;
                try
                {
                    _GaugeValue = Convert.ToDouble(_LeaveValues["_GaugeValue"].ToString());
                }
                catch (Exception e)
                {
                }

                string _Remarks = _LeaveValues["_Remarks"].ToString();

                int _isOnline = Convert.ToInt32(_LeaveValues["_isOnline"].ToString());

                string _LeaveDate = _LeaveValues["_LeaveDate"].ToString();
                DateTime leavedate = DateTime.ParseExact(_LeaveDate, "dd-MM-yyyy", null);

                //DateTime leavedate = Convert.ToDateTime(_LeaveDate);

                string _MobDateTime = _LeaveValues["_MobDateTime"].ToString();

                DateTime mobdt;


                if (_isOnline == 1)
                {
                    mobdt = DateTime.Now;
                }
                else
                {
                    mobdt = Convert.ToDateTime(_MobDateTime);
                }



                //double? _DailyGaugeReadingID =null;
                //if (_GaugeValues["_DailyGaugeReadingID"] != null && !string.IsNullOrEmpty(_GaugeValues["_DailyGaugeReadingID"].ToString()))
                //    _DailyGaugeReadingID = Convert.ToDouble(_GaugeValues["_DailyGaugeReadingID"].ToString());


                double? _Longitude = null, _Latitude = null;
                if (_LeaveValues["_Longitude"] != null && !string.IsNullOrEmpty(_LeaveValues["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_LeaveValues["_Longitude"].ToString());

                if (_LeaveValues["_Latitude"] != null && !string.IsNullOrEmpty(_LeaveValues["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_LeaveValues["_Latitude"].ToString());

                string _ImagePath = _LeaveValues["_ImagePath"].ToString();
                string decodedText = _Remarks;
                string valueSaved = "FAILURE";
                
               
                valueSaved = bllScheduleInspection.AddLeaveForm(_UserID, _GaugeValue, _ImagePath, _Remarks, _Longitude, _Latitude, mobdt, leavedate, _LeaveTypeID);




                if (valueSaved.StartsWith("SUCCESS"))
                {

                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                }
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, valueSaved, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddLeaveForm", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, exp.Message, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [Route("UploadETFile")]
        [HttpPost]
        public async Task<object> UploadETFile()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string serverUploadFolder = System.Configuration.ConfigurationManager.AppSettings["BasePath"].ToString();
                string filePath = Utility.GetImagePath(Common.Configuration.EmployeeTracking);
                var httpRequest = HttpContext.Current.Request;
                var fileName = string.Empty;
                foreach (string file in httpRequest.Files)
                {
                    string guid = Guid.NewGuid().ToString();
                    var postedFile = httpRequest.Files[file];

                    fileName = guid + ".png";// +postedFile.FileName;
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    filePath = filePath + "\\" + fileName;
                    postedFile.SaveAs(filePath);
                }
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, fileName);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("UploadETFile : - ", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1001, "Image Not Uploaded.", "" + exp.Message);
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetOutletCheckingDetail")]
        public ServiceResponse GetOutletCheckingDetail(long _OutletID, long _ScheduleDetailChannelID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                object OutletCheckingDetail = bllScheduleInspection.GeOutletCheckingByScheduleID(_ScheduleDetailChannelID, _OutletID);                   
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, OutletCheckingDetail);
                
               
           }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpPost]
        [Route("AddRotationalViolation")]
        public ServiceResponse AddRotationalViolation([FromBody] Dictionary<string, object> _RotationalValues)
        {
            ServiceResponse svcResponse = new ServiceResponse();


            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_RotationalValues))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null", null);

                int _UserID = Convert.ToInt32(_RotationalValues["_UserID"].ToString());
                long _ChannelID = Convert.ToInt64(_RotationalValues["_ChannelID"].ToString());
                double? _GaugeValue = null;
                try
                {
                    _GaugeValue = Convert.ToDouble(_RotationalValues["_GaugeValue"].ToString());
                }
                catch (Exception e)
                {
                }

                bool? _isViolation = null;
                if (_RotationalValues.ContainsKey("_isViolation"))
                {
                    _isViolation = Convert.ToBoolean(_RotationalValues["_isViolation"].ToString());
                }

                //short _isViolation = Convert.ToInt16(_RotationalValues["_isViolation"].ToString());

                short _Grouppref = Convert.ToInt16(_RotationalValues["_Grouppref"].ToString());

                string _Remarks = _RotationalValues["_Remarks"].ToString();

                int _isOnline = Convert.ToInt32(_RotationalValues["_isOnline"].ToString());

                string _MobDateTime = _RotationalValues["_MobDateTime"].ToString();

                DateTime mobdt;


                if (_isOnline == 1)
                {
                    mobdt = DateTime.Now;
                }
                else
                {
                    mobdt = Convert.ToDateTime(_MobDateTime);
                }

                double? _Longitude = null, _Latitude = null;
                if (_RotationalValues["_Longitude"] != null && !string.IsNullOrEmpty(_RotationalValues["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_RotationalValues["_Longitude"].ToString());

                if (_RotationalValues["_Latitude"] != null && !string.IsNullOrEmpty(_RotationalValues["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_RotationalValues["_Latitude"].ToString());

                string _ImagePath = _RotationalValues["_ImagePath"].ToString();
                string decodedText = _Remarks;
                string valueSaved = "FAILURE";
            
                valueSaved = bllScheduleInspection.AddRotationalViolations(_UserID, _GaugeValue, _ImagePath, _Remarks, _Longitude, _Latitude, mobdt, _ChannelID, _isViolation, _Grouppref);

                if (valueSaved.StartsWith("SUCCESS"))
                {

                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                }
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, valueSaved, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddRotationalViolations", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, exp.Message, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetCurrentMonthScheduledInspectionsOffline")]
        public ServiceResponse GetCurrentMonthScheduledInspectionsOffline(string _UserParam)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string dateFormat = "MMMM-yyyy";
                string _date = DateTime.Now.ToString(dateFormat);
                
                List<object> lstSchedule = bllScheduleInspection.GetScheduledInspectionsByMonth(_date, _UserParam);
                List<object> lstScheduleByDate=new List<object>();
                if (lstSchedule.Count > 0)
                {
                    string prevdate = "";
                    foreach (var schedule in lstSchedule)
                    {
                        string scheduleDate = (string)schedule.GetType().GetProperty("Date").GetValue(schedule);
                        if (!prevdate.Equals(scheduleDate))
                        {
                            List<object> list = bllScheduleInspection.GetScheduledInspectionsByDate(scheduleDate, _UserParam);
                            lstScheduleByDate = lstScheduleByDate.Concat(list).ToList<object>();
                        }
                        prevdate = scheduleDate;
                        
                        
                    }
                    svcResponse = srvcHlpr.CreateResponse<object>(lstScheduleByDate, 0, ServiceHelper.MSG_SUCCESS);
                }
                 
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetCurrentMonthScheduledInspectionsOffline", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

    }
}