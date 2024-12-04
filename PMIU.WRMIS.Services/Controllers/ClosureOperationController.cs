using PMIU.WRMIS.Model;
using PMIU.WRMIS.Services.HelperClasses;
using PMIU.WRMIS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Globalization;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using System.IO;
namespace PMIU.WRMIS.Services.Controllers
{

    [RoutePrefix("api/ClosureOperationControllerApi")]
    public class ClosureOperationController : ApiController
    {

        private const string MSG_UNKNOWN_ERROR = "Some unknown error occurred, please try again later";
        private const string MSG_NO_RECORD_FOUND = "FAILURE: No Records Found";
        private const string MSG_RECORD_SAVED = "SUCCESS: Record saved successfully";
        private const string MSG_SUCCESS = "SUCCESS:";
        private const string MSG_NOT_DELETED = "Record cannot be deleted.";

        PMIU.WRMIS.BLL.ClosureOperations.ClosureOperationsBLL bllClosureOperation = new BLL.ClosureOperations.ClosureOperationsBLL();

        ServiceHelper srvcHlpr = new ServiceHelper();

        [HttpGet]
        [Route("GetAllClosureWorkType")]
        public ServiceResponse GetAllClosureWorkType()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstWorkTypes = bllClosureOperation.GetAllClosureWorkTypes();
                if (lstWorkTypes.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstWorkTypes, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllClosureWorkType", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetAllACCPYears")]
        public ServiceResponse GetAllACCPYears()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lsACCPYears = bllClosureOperation.GetACCPYears();
                if (lsACCPYears.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lsACCPYears, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllACCPYears", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetAllClosureWorkStatus")]
        public ServiceResponse GetAllClosureWorkStatus()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstWorkStatus = bllClosureOperation.GetAllClosureWorkStatus();
                if (lstWorkStatus.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstWorkStatus, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllClosureWorkStatus", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetClosureWorksList")]
        public ServiceResponse GetClosureWorksList(long _DivisionID, int _TypeID, string _Year, int _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                int? _WorkTypeID = null;
                if (_TypeID != 0)
                {
                    _WorkTypeID = Convert.ToInt32(_TypeID);
                }
                long? _divisionID = null;
                if (_DivisionID != 0)
                {
                    _divisionID = Convert.ToInt64(_DivisionID);
                }
                if (_Year == null || _Year.ToString().Equals(""))
                {
                    _Year = null;
                }
                List<object> lstClosureWorkDetails = bllClosureOperation.GetClosureWorksByDivisionID(_divisionID, _WorkTypeID,_Year, _UserID);
                if (lstClosureWorkDetails.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstClosureWorkDetails, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetClosureWorksList", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Add Work Porgress in systems and prform necassary steps 
        /// </summary>
        /// <param name="_TheftDetail"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddWorkPorgress")]
        public ServiceResponse AddWorkPorgress([FromBody] Dictionary<string, object> _TheftDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_TheftDetail))
                    return srvcHlpr.CreateResponse(1004, "Parameters are null", null);

                int _UserID = Convert.ToInt32(_TheftDetail["_UserID"].ToString());
                long _DivisionID = Convert.ToInt64(_TheftDetail["_DivisionID"].ToString());
                int _WorkStatusID = Convert.ToInt32(_TheftDetail["_WorkStatusID"].ToString());
                int _WorkTypeID = Convert.ToInt32(_TheftDetail["_WorkTypeID"].ToString());
                long _ClosureWorkID = Convert.ToInt64(_TheftDetail["_ClosureWorkID"].ToString());
              

                Double _ProgressPercentage = 0;
                if (_TheftDetail.ContainsKey("_ProgressPercentage") && (!_TheftDetail["_ProgressPercentage"].ToString().Equals("null")))
                    _ProgressPercentage = Convert.ToDouble(_TheftDetail["_ProgressPercentage"].ToString());

                int? _SiltRemovedQty = null;
                if (_TheftDetail.ContainsKey("_Silt_quantity") && (!_TheftDetail["_Silt_quantity"].ToString().Equals("null")))
                    _SiltRemovedQty = Convert.ToInt32(_TheftDetail["_Silt_quantity"].ToString());

                double? _ChannelDesiltedLen = null;
                if (_TheftDetail.ContainsKey("_ChannelDesiltedLen") && (!_TheftDetail["_ChannelDesiltedLen"].ToString().Equals("null")))
                    _ChannelDesiltedLen = Convert.ToDouble(_TheftDetail["_ChannelDesiltedLen"].ToString());

                int _WorkProgressID = 0;
                if (_TheftDetail.ContainsKey("_WorkProgressID") && (!_TheftDetail["_WorkProgressID"].ToString().Equals("null")))
                    _WorkProgressID = Convert.ToInt32(_TheftDetail["_WorkProgressID"].ToString());

                string _Remarks = _TheftDetail["_Remarks"].ToString();
                string _WorkTypeName = _TheftDetail["_WorkTypeName"].ToString();


                long _ScheduleDetailID = -1;
                if (_TheftDetail.ContainsKey("_ScheduleDetailID") )
                    _ScheduleDetailID = Convert.ToInt64(_TheftDetail["_ScheduleDetailID"].ToString());


                string attachments = null;
                string atchmnts = _TheftDetail["_Attachments"].ToString();
                if (!string.IsNullOrEmpty(atchmnts) && !atchmnts.Equals("null"))
                    attachments = atchmnts;

                    
                
                    //link attachments to ClosureWorkProgress if there any
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
                                    string _NewImageName = guid + "_" + "ClosureOperation" + "-" + _date;
                                    string filePath = Utility.GetImagePath(Common.Configuration.ClosureOperations);
                                    filePath = filePath + "\\" + attchmntName;
                                    String newfilePath = Utility.GetImagePath(Common.Configuration.ClosureOperations) + "\\" + _NewImageName + ".png";
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
                                    catch (Exception e) {
                                        _imagePath = attchmntName;
                                    }
                                    
                                    lstNameofFiles.Add(new Tuple<string, string, string>("","",_imagePath));
                                    
                                }
                            }
                        }
                    }
                    long _ProgressID = bllClosureOperation.AddClosureWorkProgress(_WorkProgressID, _ClosureWorkID, _ProgressPercentage, _WorkStatusID, _SiltRemovedQty, _ChannelDesiltedLen, _Remarks, lstNameofFiles, _UserID, _ScheduleDetailID);
                    
                    if (_ProgressID>0)
                    {
                        object closureWorkDetails = bllClosureOperation.GetClosureWorkProgressByID(_ProgressID);
                        if (closureWorkDetails!=null)
                            svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_SAVED, closureWorkDetails);
                        else
                            svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                
                    }
                    else
                    {
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                    }               
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddWorkPorgress", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR + "\n" + exp.Message, null);
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
        public async Task<object> UploadFiles()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                string serverUploadFolder = System.Configuration.ConfigurationManager.AppSettings["BasePath"].ToString();
                string filePath = Utility.GetImagePath(Common.Configuration.ClosureOperations);
                var httpRequest = HttpContext.Current.Request;
                var fileName = string.Empty;
                foreach (string file in httpRequest.Files)
                {
                    string guid = Guid.NewGuid().ToString();
                    var postedFile = httpRequest.Files[file];

                    fileName = guid + ".png";
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    filePath = filePath + "\\" + fileName;
                    postedFile.SaveAs(filePath);
                }
                svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, fileName);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("ClosureOperations:UploadFiles : - ", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1001, "Image Not Uploaded.", "" + exp.Message);
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetClosureWorkByID")]
        public ServiceResponse GetClosureWorkByID(long _WorkID, long _WorkProgressID, int _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                long? WorkProgressID = null;
                if (_WorkProgressID != 0)
                {
                    WorkProgressID = Convert.ToInt64(_WorkProgressID);
                }

                object objClosureWorkDetails = bllClosureOperation.GetClosureWorkByID(_WorkID, WorkProgressID, _UserID);
                if (objClosureWorkDetails != null)
                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, objClosureWorkDetails);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetClosureWorkByID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
    }
}
