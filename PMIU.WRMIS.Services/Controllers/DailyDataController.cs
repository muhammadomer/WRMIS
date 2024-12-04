using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Services.HelperClasses;
using PMIU.WRMIS.Services.Models;
using PMIU.WRMIS.Notifications;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PMIU.WRMIS.BLL.Notifications;
using System.Globalization;

namespace PMIU.WRMIS.Services.Controllers
{
    [RoutePrefix("api/DailyData")]
    public class DailyDataController : ApiController
    {

        private const string MSG_UNKNOWN_ERROR = "Some unknown error occurred, please try again later";
        private const string MSG_NO_RECORD_FOUND = "FAILURE: No Records Found";
        private const string MSG_RECORD_SAVED = "SUCCESS: Record saved successfully";
        private const string MSG_SUCCESS = "SUCCESS:";

        PMIU.WRMIS.BLL.DailyData.DailyDataBLL bllDailyData = new BLL.DailyData.DailyDataBLL();
        PMIU.WRMIS.BLL.IrrigationNetwork.Channel.ChannelBLL bllChannel = new BLL.IrrigationNetwork.Channel.ChannelBLL();
        ServiceHelper srvcHlpr = new ServiceHelper();

        #region User Daily Operational Get Queries
        /// <summary>
        /// For Gauge Readers only
        /// returns the detail of assigned gauges 
        /// along with discharge parameter info
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>ServiceResponse</returns>
        //[HttpGet]
        //[Route("GetUserGauges")]
        //public ServiceResponse GetUserGauges(long _UserID)
        //{
        //    ServiceResponse svcResponse = new ServiceResponse();
        //    try
        //    {
        //        List<GetUserGauges_Result> lstUserGauges = bllDailyData.GetUserGauges(_UserID);
        //        if (lstUserGauges.Count > 0)
        //            svcResponse = srvcHlpr.CreateResponse<GetUserGauges_Result>(lstUserGauges, 0, MSG_SUCCESS);
        //        else
        //            svcResponse = srvcHlpr.CreateResponse(1004, MSG_NO_RECORD_FOUND, null);
        //    }
        //    catch (Exception exp)
        //    {
        //        svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
        //        string excetionMsg = exp.Message;
        //    }
        //    return svcResponse;
        //}

        /// <summary>
        /// No in user right now
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserGaugesByLocation")]
        public ServiceResponse GetUserGaugesByLocation(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {
                PMIU.WRMIS.BLL.DailyData.DailyDataBLL bllDailyData = new BLL.DailyData.DailyDataBLL();
                List<GetUserGaugesStationBaised_Result> lstUserGauges = bllDailyData.GetUserGaugesByLocation(_UserID);

                if (lstUserGauges.Count > 0)
                {
                    svcResponse.StatusCode = 0;
                    svcResponse.StatusMessage = "SUCCESS:";
                    svcResponse.Data = lstUserGauges;
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
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Provide USERID
        /// return the assoicated Divisions(Name,ID)
        /// along with their circle(Name,ID),zone(Name,ID)
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserDivisions")]
        public ServiceResponse GetUserDivisions(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstUserDivisions = bllDailyData.GetDivisionsByUserID(_UserID);
                svcResponse = srvcHlpr.CreateResponse<object>(lstUserDivisions, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Provide USERID
        /// return the assoicated Subdivisions(Name,ID)
        /// with their associated divisionID
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserSubDivisions")]
        public ServiceResponse GetUserSubDivisions(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<GetUserSubDivisions_Result> lstUserSubDiv = bllDailyData.GetUserSubDivisions(_UserID);
                svcResponse = srvcHlpr.CreateResponse<GetUserSubDivisions_Result>(lstUserSubDiv, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Provide USERID
        /// return the assoicated Sections(Name,ID)
        /// with their associated subdivisionID
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserSections")]
        public ServiceResponse GetUserSections(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<GetUserSection_Result> LstUserSections = bllDailyData.GetUserSections(_UserID);
                svcResponse = srvcHlpr.CreateResponse<GetUserSection_Result>(LstUserSections, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
            }
            return svcResponse;
        }

        /// <summary>
        /// Provide user ID
        /// returns all Gauges inside user range 
        /// along with details in their discharge parameters,
        /// channel (ID,Name)
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllUserGauges")]
        public ServiceResponse GetAllUserGauges(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<GetGauges_Result> LstUserSections = bllDailyData.GetUserChannelAndGauges(_UserID);
                svcResponse = srvcHlpr.CreateResponse<GetGauges_Result>(LstUserSections, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Retruns List of CO_ReasonForChange
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetReasonForChange")]
        public ServiceResponse GetReasonForChange()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<CO_ReasonForChange> lstReadings = bllDailyData.GetReasonForChange();
                svcResponse = srvcHlpr.CreateResponse<CO_ReasonForChange>(lstReadings, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Provided GaugeID, Gauge value
        /// calculate the Tail Gauge disacharge for the gauge 
        /// </summary>
        /// <param name="_GaugeID"></param>
        /// <param name="_GaugeValue"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTailDischarge")]
        public ServiceResponse GetTailDischarge(long _GaugeID, double _GaugeValue)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                Double? value = bllDailyData.GetTailGaugeDischargeWithOffTakes(_GaugeID, _GaugeValue);
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, value);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Provided GaugeID, Gauge value
        /// calculate the Tail Gauge disacharge for the gauge 
        /// </summary>
        /// <param name="_GaugeID"></param>
        /// <param name="_GaugeValue"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBedDischarge")]
        public ServiceResponse GetBedDischarge(long _GaugeID, double _GaugeValue)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                Double? value = bllDailyData.CalculateDischarge(_GaugeID, _GaugeValue);
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, value);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Get gauge current date reading values 
        /// for the provided section and channel
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDailyGaugeReading")]
        public ServiceResponse GetDailyGaugeReading(long _SectionID, long _ChannelID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<GetDailyGaugeReadingAndroid_Result> lstReadings = bllDailyData.GetDailyGaugeReading(_SectionID, _ChannelID, DateTime.Now);
                svcResponse = srvcHlpr.CreateResponse<GetDailyGaugeReadingAndroid_Result>(lstReadings, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                string excetionMsg = exp.Message;
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
            }
            return svcResponse;
        }

        /// <summary>
        /// Fucntionality in grey area for now
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetIndents")]
        public ServiceResponse GetIndents(long _SubDivisionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            //TODO: 
            return svcResponse;
        }

        /// <summary>
        /// Return the Image if any is there agains for the provided ImageID
        /// </summary>
        /// <param name="_ImageID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetImage")]
        public HttpResponseMessage GetImage([FromBody]string _ImageID)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                MemoryStream ms = new MemoryStream();
                string serverUploadFolder = System.Configuration.ConfigurationManager.AppSettings["BasePath"].ToString();
                string filePath = Utility.GetImagePath(Common.Configuration.DailyData);
                filePath = filePath + "\\" + _ImageID + ".png";
                if (File.Exists(filePath))
                    new Bitmap(filePath).Save(ms, ImageFormat.Png);

                httpResponse.Content = new ByteArrayContent(ms.ToArray());
                httpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            }
            catch (Exception ex)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                httpResponse.Content = null;
                string exceptionMsg = ex.Message;
            }
            return httpResponse;
        }

        /// <summary>
        /// returns list of Outlets againts a channel within a section 
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOutlets")]
        public ServiceResponse GetOutlets(long _SectionID, long _ChannelID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<GetChannelSectionOutlets_Result> LstUserSections = bllDailyData.GetOutletBySectionAndChannelID(_SectionID, _ChannelID);
                svcResponse = srvcHlpr.CreateResponse<GetChannelSectionOutlets_Result>(LstUserSections, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Return details on barrage associated with a user 
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBarrageByUser")]
        public ServiceResponse GetBarrageByUser(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                CO_Station mdlStation = bllDailyData.GetBarrageByUser(_UserID);
                if (mdlStation != null)
                {
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, "ID:" + mdlStation.ID + ",Name:" + mdlStation.Name);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, MSG_NO_RECORD_FOUND, null);
                }

            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Return List of timestamps for barrage as per current frequency 
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBarrageTimeStamps")]
        public ServiceResponse GetBarrageTimeStamps(long _BarrageID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<string> mdlStation = bllDailyData.GetBarrageTimeStamps(_BarrageID);
                svcResponse = srvcHlpr.CreateResponse<string>(mdlStation, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        ///// <summary>
        ///// Return List of Barrage related Sites/Attirbutes list 
        ///// </summary>
        ///// <param name="_UserID"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("GetBarrageAttributes")]
        //public ServiceResponse GetBarrageAttributes(long _BarrageID)
        //{
        //    ServiceResponse svcResponse = new ServiceResponse();
        //    try
        //    {
        //        DateTime TimeFormat = Convert.ToDateTime("01:00");
        //        TimeFormat.ToString("HH:mm");

        //        DateTime Date = DateTime.Now;
        //        DateTime ReadingDateTime = Convert.ToDateTime(Date.Year + "-" + Date.Month + "-" + Date.Day + " " + TimeFormat.Hour + ":" + TimeFormat.Minute);

        //        List<GetBarrageDailyDischargeDataMobile_Result> LstUserSections = bllDailyData.GetBarrageAttributes(_BarrageID, ReadingDateTime);
        //        svcResponse = srvcHlpr.CreateResponse<GetBarrageDailyDischargeDataMobile_Result>(LstUserSections, 0, MSG_SUCCESS);
        //    }
        //    catch (Exception exp)
        //    {
        //        svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
        //        string excetionMsg = exp.Message;
        //    }
        //    return svcResponse;
        //}

        /// <summary>
        /// Return List of Barrage related Sites/Attirbutes list 
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBarrageAttributes")]
        public ServiceResponse GetBarrageAttributes(long _BarrageID, string _TimeStamp)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                DateTime TimeFormat = Convert.ToDateTime(_TimeStamp);
                TimeFormat.ToString("HH:mm");

                DateTime Date = DateTime.Now;
                DateTime ReadingDateTime = Convert.ToDateTime(Date.Year + "-" + Date.Month + "-" + Date.Day + " " + TimeFormat.Hour + ":" + TimeFormat.Minute);

                List<GetBarrageDailyDischargeDataMobile_Result> LstUserSections = bllDailyData.GetBarrageAttributes(_BarrageID, ReadingDateTime);
                svcResponse = srvcHlpr.CreateResponse<GetBarrageDailyDischargeDataMobile_Result>(LstUserSections, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Return discharge for a channel associated with a barrage 
        /// </summary>
        /// <param name="_AttibuteID"></param>
        /// <param name="_GaugeValue"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CalculateDischargeForBarrageSite")]
        public ServiceResponse CalculateDischargeForBarrageSite(long _AttibuteID, double _GaugeValue)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                long? gaugeID = bllDailyData.GetGaugeIDByAttributeID(_AttibuteID);
                if (gaugeID == null)
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                }
                else if (bllDailyData.HasDischargeParameters(Convert.ToInt64(gaugeID)))
                {
                    Double? value = bllDailyData.CalculateDischarge(Convert.ToInt64(gaugeID), _GaugeValue);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, value);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, "Discharge Table Parameter are not present hence Discharge cannot be calculated", null);
                }
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("CalculateUpstream")]
        public ServiceResponse CalculateUpstream(string _GaugesData, double _DownstreamDischarge)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                double channelDischargeSum = 0;
                string[] _dataList = _GaugesData.Split(',');

                if (_dataList.Length > 0)
                {
                    foreach (var data in _dataList)
                    {
                        if (!string.IsNullOrEmpty(data))
                        {
                            string[] dataList = data.Split(':');
                            long attributeID = Convert.ToInt64(dataList[0]);
                            long? gaugeID = bllDailyData.GetGaugeIDByAttributeID(attributeID);
                            if (gaugeID != null)
                            {
                                double gaugeReading = Convert.ToDouble(dataList[1]);
                                double? discharge = bllDailyData.CalculateDischarge(Convert.ToInt64(gaugeID), gaugeReading);

                                if (discharge == null)
                                    channelDischargeSum = channelDischargeSum + 0;
                                else
                                    channelDischargeSum = channelDischargeSum + Convert.ToDouble(discharge);
                            }
                        }
                    }
                }
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, (_DownstreamDischarge + channelDischargeSum));
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        #endregion

        #region User Daily Operational CRUD like operations
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

            PMIU.WRMIS.BLL.DailyData.DailyDataBLL bllDailyDataGauge = new BLL.DailyData.DailyDataBLL();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_GaugeValues))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null", null);

                long _UserID = Convert.ToInt64(_GaugeValues["_UserID"].ToString());
                long _GaugeID = Convert.ToInt64(_GaugeValues["_GaugeID"].ToString());
                bool _GaugeF = Convert.ToBoolean(_GaugeValues["_GaugeF"].ToString());
                bool _GaugeP = Convert.ToBoolean(_GaugeValues["_GaugeP"].ToString());
                double _GaugeValue = Convert.ToDouble(_GaugeValues["_GaugeValue"].ToString());
                string _Remarks = _GaugeValues["_Remarks"].ToString();

                int _isOnline = Convert.ToInt32(_GaugeValues["_isOnline"].ToString());
                string _ReadingTime = _GaugeValues["_ReadingTime"].ToString();

                DateTime readingdt;


                if (_isOnline == 1)
                {
                    readingdt = DateTime.Now;
                }
                else
                {
                    readingdt = Convert.ToDateTime(_ReadingTime);
                }
                

                
                //double? _DailyGaugeReadingID =null;
                //if (_GaugeValues["_DailyGaugeReadingID"] != null && !string.IsNullOrEmpty(_GaugeValues["_DailyGaugeReadingID"].ToString()))
                //    _DailyGaugeReadingID = Convert.ToDouble(_GaugeValues["_DailyGaugeReadingID"].ToString());
                

                double? _Longitude = null, _Latitude = null;
                if (_GaugeValues["_Longitude"] != null && !string.IsNullOrEmpty(_GaugeValues["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_GaugeValues["_Longitude"].ToString());

                if (_GaugeValues["_Latitude"] != null && !string.IsNullOrEmpty(_GaugeValues["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_GaugeValues["_Latitude"].ToString());

                string _ImagePath = _GaugeValues["_ImagePath"].ToString();
                string decodedText = _Remarks;
                string valueSaved = "FAILURE";

                if (_GaugeValues.ContainsKey("_ScheduledID"))
                {
                    long? value = null;
                    string scheduleID = _GaugeValues["_ScheduledID"].ToString();
                    if (!string.IsNullOrEmpty(scheduleID) && !scheduleID.Equals("null"))
                        value = Convert.ToInt64(scheduleID);
                    valueSaved = bllDailyDataGauge.AddGaugeValue_Scheduled(_UserID, _GaugeID, _GaugeF, _GaugeP, _GaugeValue, _ImagePath, decodedText, _Longitude, _Latitude, value, "A");
                }
                else
                {
                    valueSaved = bllDailyDataGauge.AddGaugeReading(_UserID, _GaugeID, _GaugeF, _GaugeP, _GaugeValue, _ImagePath, decodedText, _Longitude, _Latitude, "A", readingdt);

                }


                if (valueSaved.StartsWith("SUCCESS"))
                {
                    string[] response = valueSaved.Split('-');
                    string GaugeReadingID = response[1];
                    if (bllDailyDataGauge.IsGaugeValueAdded)
                    {
                       
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("DailyGaugeReadingID", GaugeReadingID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.DailyData.AddDailyDate, _UserID);
                    }
                    else
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("DailyGaugeReadingID", GaugeReadingID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.DailyData.EditDailyData, _UserID);
                    }
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                }
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, valueSaved, null);
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
                string filePath = Utility.GetImagePath(Common.Configuration.DailyData);
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

        /// <summary>
        /// Update or insert the gauge reading values 
        /// as per requireded condition
        /// </summary>
        /// <param name="_GaugeValues"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateGaugeReading")]
        public ServiceResponse UpdateGaugeReading([FromBody] Dictionary<string, object> _GaugeValues)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_GaugeValues))
                    return srvcHlpr.CreateResponse(1004, "Parameter are null.", null);
                //Read and parse paramtere from dictionary
                long _UserID = Convert.ToInt64(_GaugeValues["_UserID"].ToString());
                long _GaugeID = Convert.ToInt64(_GaugeValues["_GaugeID"].ToString());
                long _ReasonForChangeID = Convert.ToInt64(_GaugeValues["_ReasonForChangeID"].ToString());

                Double _GaugeValue = Convert.ToDouble(_GaugeValues["_GaugeValue"].ToString());

                double? _Longitude = null, _Latitude = null;
                if (_GaugeValues["_Longitude"] != null && !string.IsNullOrEmpty(_GaugeValues["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_GaugeValues["_Longitude"].ToString());

                if (_GaugeValues["_Latitude"] != null && !string.IsNullOrEmpty(_GaugeValues["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_GaugeValues["_Latitude"].ToString());

                
                bool _IsUpdate = Convert.ToBoolean(_GaugeValues["_IsUpdate"].ToString());

                //continue with saving record
                if (_IsUpdate) // Record exists already
                {
                    long _GaugeReadingID = Convert.ToInt64(_GaugeValues["_GaugeReadingID"].ToString());
                    double? discharge = new DailyDataBLL().CalculateDischarge(_GaugeID, _GaugeValue);
                    double val = (discharge == null ? 0.0 : Convert.ToDouble(discharge));
                    if (bllDailyData.IsRecordLocked(_GaugeReadingID))
                    {
                        return srvcHlpr.CreateResponse(1004, "Record has been locked.", null);
                    }

                    long _ReadingID = bllDailyData.UpdateGaugeReading(_GaugeReadingID, val, _ReasonForChangeID, _GaugeValue, _UserID, _Longitude, _Latitude,"A");
                    if (_ReadingID!=0)
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("DailyGaugeReadingID", _ReadingID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.DailyData.EditDailyData, _UserID);

                        svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                    }
                    else
                        svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                }
                else // Add to be performed
                {
                    long DailyGaugeReadingID = new DailyDataBLL().AddGaugeReading_SDOXEN(_GaugeID, _ReasonForChangeID, _GaugeValue, _UserID, _Longitude, _Latitude, "A");
                    if (DailyGaugeReadingID!=0)
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("DailyGaugeReadingID", DailyGaugeReadingID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.DailyData.EditDailyData, _UserID);

                        svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                    }
                    else
                        svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                }
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("UpdateGaugeReading", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, exp.Message, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Add discharge measurement parameters for a gauge in DB server
        /// depending on gauge level insertion in different tables occurs
        /// </summary>
        /// <param name="_DschrgMeasurementParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddGDischargeMeasurements")]
        public ServiceResponse AddGaugeDischargeMeasurements([FromBody] Dictionary<string, object> _DschrgMeasurementParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_DschrgMeasurementParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                long _UserID = Convert.ToInt64(_DschrgMeasurementParams["_UserID"].ToString());
                long _GaugeID = Convert.ToInt64(_DschrgMeasurementParams["_GaugeID"].ToString());
                long _GaugeLevel = Convert.ToInt64(_DschrgMeasurementParams["_GaugeLevel"].ToString());

                double _ParamN_B = Convert.ToDouble(_DschrgMeasurementParams["_ParamN_B"].ToString());
                double _ParamD_H = Convert.ToDouble(_DschrgMeasurementParams["_ParamD_H"].ToString());
                double _ObsrvdDschrg = Convert.ToDouble(_DschrgMeasurementParams["_ObsrvdDschrg"].ToString());

                int _GCorrectType = Convert.ToInt32(_DschrgMeasurementParams["_GCorrectType"].ToString());

                double? _Longitude = null, _Latitude = null;
                if (_DschrgMeasurementParams["_Longitude"] != null && !string.IsNullOrEmpty(_DschrgMeasurementParams["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_DschrgMeasurementParams["_Longitude"].ToString());

                if (_DschrgMeasurementParams["_Latitude"] != null && !string.IsNullOrEmpty(_DschrgMeasurementParams["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_DschrgMeasurementParams["_Latitude"].ToString());


                bool result = false;
                if (_GaugeLevel == 1) //Bed Level Gauge
                {
                    double? _GCorrectValue = null;
                    if (!string.IsNullOrEmpty(_DschrgMeasurementParams["_GCorrectValue"].ToString()))
                        _GCorrectValue = Convert.ToDouble(_DschrgMeasurementParams["_GCorrectValue"].ToString());

                    result = bllDailyData.AddGauge_BedLevel_DischargeMeasurements(_UserID, _GaugeID, _ParamN_B, _ParamD_H, _ObsrvdDschrg, _GCorrectType, _GCorrectValue, _Longitude, _Latitude,"A");
                }
                else // Crest Level Gauge
                {
                    result = bllDailyData.AddGauge_CrestLevel_DischargeMeasurements(_UserID, _GaugeID, _ParamN_B, _ParamD_H, _ObsrvdDschrg, _Longitude, _Latitude, "A");
                }

                if (result)
                {
                    if (_GaugeLevel == 1) //Bed Level Gauge
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("GaugeID", _GaugeID);
                        _event.Parameters.Add("IsCrestParameters", "false");
                        _event.AddNotifyEvent((long)NotificationEventConstants.IrrigationNetwork.EditBedLevelParameters, _UserID);

                    }
                    else //Crest Level Gauge
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("GaugeID", _GaugeID);
                        _event.Parameters.Add("IsCrestParameters", "true");
                        _event.AddNotifyEvent((long)NotificationEventConstants.IrrigationNetwork.EditCrestLevelParameters, _UserID);
                    }

                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
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
        /// Add the outlet performance values in DB server
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

                double? _HeadAboveCrest = null, _WorkingHead = null;
                string crest = _OutletPrformncParams["_HeadAboveCrest"].ToString();
                if (!string.IsNullOrEmpty(crest) && !crest.Equals("null"))
                    _HeadAboveCrest = Convert.ToDouble(_OutletPrformncParams["_HeadAboveCrest"].ToString());

                string head = _OutletPrformncParams["_WorkingHead"].ToString();
                if (!string.IsNullOrEmpty(head) && !head.Equals("null"))
                    _WorkingHead = Convert.ToDouble(_OutletPrformncParams["_WorkingHead"].ToString());

                double _ObsrvdDschrg = Convert.ToDouble(_OutletPrformncParams["_ObsrvdDschrg"].ToString());

                double? _Longitude = null, _Latitude = null;
                if (_OutletPrformncParams["_Longitude"] != null && !string.IsNullOrEmpty(_OutletPrformncParams["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_OutletPrformncParams["_Longitude"].ToString());

                if (_OutletPrformncParams["_Latitude"] != null && !string.IsNullOrEmpty(_OutletPrformncParams["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_OutletPrformncParams["_Latitude"].ToString());


                bool result = false;
                result = bllDailyData.AddOutletPerformance(_UserID, _OutletID, _HeadAboveCrest, _WorkingHead, _ObsrvdDschrg, _Longitude, _Latitude, "A");

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

        /// <summary>
        /// Add barrage site daily discharge data
        /// </summary>
        /// <param name="_BarrageDischargeData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddBarrageDischargeData")]
        public ServiceResponse AddBarrageDischargeData([FromBody] Dictionary<string, object> _BarrageDischargeData)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_BarrageDischargeData))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                long _UserID = Convert.ToInt64(_BarrageDischargeData["_UserID"].ToString());
                long _BarrageID = Convert.ToInt64(_BarrageDischargeData["_BarrageID"].ToString());
                string _DataList = _BarrageDischargeData["_DataList"].ToString();
                string _TimeStamp = _BarrageDischargeData["_TimeStamp"].ToString();

                //double? _Longitude = null, _Latitude = null;
                //if (_BarrageDischargeData["_Longitude"] != null && !string.IsNullOrEmpty(_BarrageDischargeData["_Longitude"].ToString()))
                //    _Longitude = Convert.ToDouble(_BarrageDischargeData["_Longitude"].ToString());

                //if (_BarrageDischargeData["_Latitude"] != null && !string.IsNullOrEmpty(_BarrageDischargeData["_Latitude"].ToString()))
                //    _Latitude = Convert.ToDouble(_BarrageDischargeData["_Latitude"].ToString());
                DateTime Date = DateTime.Now.Date;
           //     DateTime ReadingDateTime = Convert.ToDateTime(Date.Year + "-" + Date.Month + "-" + Date.Day + " " + TimeFormat.Hour + ":" + TimeFormat.Minute);

                bllDailyData.AddBarrageDischargeDataAndroid(_BarrageID,DateTime.Now.Date, _TimeStamp, _DataList, _UserID, false, "A");

                //Notifications.NotifyEvent _event = new Notifications.NotifyEvent();
                //_event.Parameters.Add("GaugeID", mdlChannelGauge.ID);
                //_event.AddNotifyEvent((long)NotificationEventConstants.DailyData.AddDailyHourlyDataForBarrage, _UserID);

                svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
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
        /// Edit barrage site daily discharge data
        /// </summary>
        /// <param name="_BarrageDischargeData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditBarrageDischargeData")]
        public ServiceResponse EditBarrageDischargeData([FromBody] Dictionary<string, object> _BarrageDischargeData)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_BarrageDischargeData))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                long _UserID = Convert.ToInt64(_BarrageDischargeData["_UserID"].ToString());
                long _BarrageID = Convert.ToInt64(_BarrageDischargeData["_BarrageID"].ToString());
                string _DataList = _BarrageDischargeData["_DataList"].ToString();
                string _TimeStamp = _BarrageDischargeData["_TimeStamp"].ToString();

              

                DateTime TimeFormat = Convert.ToDateTime(_TimeStamp);
                TimeFormat.ToString("HH:mm");

                DateTime Date = DateTime.Now;
                DateTime ReadingDateTime = Convert.ToDateTime(Date.Year + "-" + Date.Month + "-" + Date.Day + " " + TimeFormat.Hour + ":" + TimeFormat.Minute);
                string FormatedDateTime = ReadingDateTime.ToString("yyyy-MM-dd" + " " + "HH:mm");

                bllDailyData.UpdateBarrageDischargeData(_BarrageID, FormatedDateTime, _DataList, _UserID, "Updated from Mobile", "A");
                svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
            }
            catch (Exception exp)
            {
                //TODO: Log exception
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }


        #endregion

        [HttpPost]
        [Route("AddMeterReading")]
        public ServiceResponse AddMeterReading([FromBody] Dictionary<string, object> _MeterReadingValues)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            PMIU.WRMIS.BLL.DailyData.DailyDataBLL bllDailyDataGauge = new BLL.DailyData.DailyDataBLL();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_MeterReadingValues))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null", null);

                long _UserID = Convert.ToInt64(_MeterReadingValues["_UserID"].ToString());
                int _MeterReading = Convert.ToInt32(_MeterReadingValues["_MeterReading"].ToString());
                string _ReadingType = _MeterReadingValues["_ReadingType"].ToString();
               
                //double? _fuel = Convert.ToDouble(_MeterReadingValues["_FuelQty"].ToString());
                float? _FuelQty = null;
                try
                {
                    _FuelQty = (float)Convert.ToDouble(_MeterReadingValues["_FuelQty"].ToString());
                }
                catch (Exception e)
                {
                    
                }
                
                
                string _ReadDate = _MeterReadingValues["_ReadDate"].ToString();
                string  _ReadTime = _MeterReadingValues["_ReadTime"].ToString();
                //string s = "2011-03-21 13:26";
                string s = _ReadDate + " " +_ReadTime;
                DateTime dt =
                DateTime.ParseExact(s, "dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture);

                //DateTime parsedDate = DateTime.ParseExact(s, "dd-MM-yyyy hh:mm att", CultureInfo.InvariantCulture);

                //double _GaugeValue = Convert.ToDouble(_GaugeValues["_GaugeValue"].ToString())

                string _Remarks = null;

                try
                {
                    _Remarks = _MeterReadingValues["_Remarks"].ToString();
                }
                catch (Exception e)
                {
                    
                }

                

                //double? _DailyGaugeReadingID =null;
                //if (_GaugeValues["_DailyGaugeReadingID"] != null && !string.IsNullOrEmpty(_GaugeValues["_DailyGaugeReadingID"].ToString()))
                //    _DailyGaugeReadingID = Convert.ToDouble(_GaugeValues["_DailyGaugeReadingID"].ToString());

                double? _Longitude = null, _Latitude = null;
                if (_MeterReadingValues["_Longitude"] != null && !string.IsNullOrEmpty(_MeterReadingValues["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_MeterReadingValues["_Longitude"].ToString());

                if (_MeterReadingValues["_Latitude"] != null && !string.IsNullOrEmpty(_MeterReadingValues["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_MeterReadingValues["_Latitude"].ToString());

                string _Attachment1 = null;

                try
                {
                    _Attachment1 = _MeterReadingValues["_Attachment1"].ToString();
                }
                catch (Exception e)
                {
                   
                }

                string _Attachment2 = null;

                try
                {
                    _Attachment2 = _MeterReadingValues["_Attachment2"].ToString();
                }
                catch (Exception e)
                {
                    
                }
                
                
                string valueSaved = "FAILURE";

                valueSaved = bllDailyDataGauge.AddMeterReading(_UserID, _ReadingType, _MeterReading, _FuelQty, _Remarks, _Attachment1, _Attachment2, dt, _Longitude, _Latitude);

                if (valueSaved.StartsWith("SUCCESS"))
                {                                
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                }
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, valueSaved, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("MeterReading", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, exp.Message, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [Route("UploadFilesMeter")]
        [HttpPost]
        public async Task<object> UploadMultipleFilesMeter()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string serverUploadFolder = System.Configuration.ConfigurationManager.AppSettings["BasePath"].ToString();
                string filePath = Utility.GetImagePath(Common.Configuration.VehicleReadings);
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
        /// <summary>
        /// Provide user ID
        /// returns all Gauges inside user range 
        /// along with details in their discharge parameters,
        /// channel (ID,Name)
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllUserOutlets")]
        public ServiceResponse GetAllUserOutlets(long _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<GetChannelSectionOutlet_Result> LstUserSections = bllDailyData.GetChannelSectionOutlet(_UserID);
                svcResponse = srvcHlpr.CreateResponse<GetChannelSectionOutlet_Result>(LstUserSections, 0, MSG_SUCCESS);
            }
            catch (Exception exp)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
    }

}
