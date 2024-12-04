using Newtonsoft.Json;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Services.HelperClasses;
using PMIU.WRMIS.Services.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PMIU.WRMIS.Services.Controllers
{
    [RoutePrefix("api/TendersMonitoringControllerApi")]
    public class TendersMonitoringController : ApiController
    {
        private const string MSG_UNKNOWN_ERROR = "Some unknown error occurred, please try again later";
        private const string MSG_NO_RECORD_FOUND = "FAILURE: No Records Found";
        private const string MSG_RECORD_SAVED = "SUCCESS: Record saved successfully";
        private const string MSG_SUCCESS = "SUCCESS:";
        private const string MSG_NOT_DELETED = "Record cannot be deleted.";

        PMIU.WRMIS.BLL.Tenders.TenderManagementBLL bllTendersManagment = new BLL.Tenders.TenderManagementBLL();
        ServiceHelper srvcHlpr = new ServiceHelper();
        [HttpGet]
        [Route("GetAllDomains")]
        public ServiceResponse GetAllDomains()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<dynamic> lstDomains = bllTendersManagment.GetDomains();
                if (lstDomains.Count > 0)
                {
                    svcResponse = srvcHlpr.CreateResponse<dynamic>(lstDomains, 0, ServiceHelper.MSG_SUCCESS);
                }
                else 
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch(Exception e) 
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetMonitoringUsersByDivisionID")]
        public ServiceResponse GetMonitoringUsersByDivisionID(long _DivisionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
               
                List<dynamic> lstUsers = bllTendersManagment.GetMonitoringUsersByDivisionID(_DivisionID);
                if (lstUsers.Count > 0)
                {
                    svcResponse = srvcHlpr.CreateResponse<dynamic>(lstUsers, 0, ServiceHelper.MSG_SUCCESS);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetTenderWorks")]
        public ServiceResponse GetTenderWorks(long _DivisionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                long? _TenderWorkID = null;
                long? DivisionID = null;
                if (_DivisionID != 0) {
                    DivisionID = _DivisionID;
                }

                List<object> lstTenderWorks = bllTendersManagment.GetTenderWorksListByDivisionID(DivisionID, _TenderWorkID);
                if (lstTenderWorks.Count > 0)
                {
                    svcResponse = srvcHlpr.CreateResponse<object>(lstTenderWorks, 0, ServiceHelper.MSG_SUCCESS);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetTenderCommitteMembers")]
        public ServiceResponse GetTenderCommitteMembers(long _TenderWorkID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstTCommitteeMembers = bllTendersManagment.GetTenderCommitteeMembers(_TenderWorkID);
                if (lstTCommitteeMembers.Count > 0)
                {
                    svcResponse = srvcHlpr.CreateResponse<object>(lstTCommitteeMembers, 0, ServiceHelper.MSG_SUCCESS);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetTenderContractors")]
        public ServiceResponse GetTenderContractors(long _TenderWorkID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            
            try
            {

                List<object> lstTCommitteeMembers = bllTendersManagment.GetTenderContractors(_TenderWorkID);
                if (lstTCommitteeMembers.Count > 0)
                {
                    svcResponse = srvcHlpr.CreateResponse<object>(lstTCommitteeMembers, 0, ServiceHelper.MSG_SUCCESS);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Add Evalutation Committee Member Attendance parameters in DB server
        /// </summary>
        /// <param name="_TMAttendanceParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTenderCommitteeAttendance")]
        public ServiceResponse AddTenderCommitteeAttendance([FromBody] Dictionary<string, object> _TMAttendanceParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_TMAttendanceParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                string dateFormat = "yyyyMMdd-hhmmss";
                string _date = DateTime.Now.ToString(dateFormat);
                
                int _UserID = Convert.ToInt32(_TMAttendanceParams["_UserID"].ToString());
                long _TenderWorkID = Convert.ToInt64(_TMAttendanceParams["_TenderWorkID"].ToString());
                List<TM_TenderCommitteeMembers> attendanceList = JsonConvert.DeserializeObject<List<TM_TenderCommitteeMembers>>(_TMAttendanceParams["_AttendeeList"].ToString()).ToList<TM_TenderCommitteeMembers>();              
                
                string _MonitoredBy = _TMAttendanceParams["_MonitoredBy"].ToString();
                string _MonitoredByName = _TMAttendanceParams["_MonitoredByName"].ToString();
                string _OpenedBy = _TMAttendanceParams["_OpenedBy"].ToString();
                string ECA_Attachment = _TMAttendanceParams["ECA_Attachment"].ToString();



                string pdf_guid = ECA_Attachment.Split('.')[0];
                string _pdfPath = "";
                string _NewPDFName = pdf_guid +  "_TenderEvalCommitteeAttendance-" + _date + ".png";
                string pdfFilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + ECA_Attachment;
                String newPdfFilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + _NewPDFName ;
                if (File.Exists(pdfFilePath))
                    System.IO.File.Move(@pdfFilePath, @newPdfFilePath);

                _pdfPath = (File.Exists(newPdfFilePath)) ? _NewPDFName : ECA_Attachment;

                string attachments = null;
                string atchmnts = _TMAttendanceParams["_Attachments"].ToString();
                if (!string.IsNullOrEmpty(atchmnts) && !atchmnts.Equals("null"))
                    attachments = atchmnts;



                //link attachments to TenderEvalCommitteeAttendance if there any
                
                List<string> lstNameofFiles = new List<string>();
                string _imagePath = "";
                if (attachments != null && attachments.Length > 0)
                {
                    string[] lstAttachments = attachments.Split(',');
                    if (lstAttachments != null && lstAttachments.Length > 0)
                    {
                        foreach (string attchmntName in lstAttachments)
                        {
                             if (attchmntName.Length > 0 && !attchmntName.Contains("TenderEvalCommitteeAttendance"))
                                {
                                    string guid = attchmntName.Split('.')[0];
                                    string _NewImageFileName = guid + "_TenderEvalCommitteeAttendance-" + _date + ".png";

                                    string imgfilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + attchmntName;
                                    String newImgfilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + _NewImageFileName  ;
                                    if (File.Exists(imgfilePath))
                                        System.IO.File.Move(@imgfilePath, @newImgfilePath);

                                    _imagePath =  (File.Exists(newImgfilePath)) ? _NewImageFileName : attchmntName;
                                   
                                }
                                lstNameofFiles.Add((_imagePath));

                            
                        }
                    }
                }
                bool result = false;
                if (attendanceList.Count > 0)
                {
                    foreach (var ls in attendanceList)
                    {
                        if (ls.ID != 0)
                        {
                            object _mdlMemberAttendance = bllTendersManagment.GetTenderCommitteeMemberByID(ls.ID);
                            ls.CreatedBy = Convert.ToInt32(Convert.ToString(_mdlMemberAttendance.GetType().GetProperty("CreatedBy").GetValue(_mdlMemberAttendance)));
                            ls.CreatedDate = Convert.ToDateTime(Convert.ToString(_mdlMemberAttendance.GetType().GetProperty("CreatedDate").GetValue(_mdlMemberAttendance)));
                            ls.CommitteeMembersID = Convert.ToInt64(Convert.ToString(_mdlMemberAttendance.GetType().GetProperty("CommitteeMembersID").GetValue(_mdlMemberAttendance)));
                            ls.TenderWorksID = Convert.ToInt64(Convert.ToString(_mdlMemberAttendance.GetType().GetProperty("TenderWorksID").GetValue(_mdlMemberAttendance)));
                            ls.ModifiedBy = Convert.ToInt32(_UserID);
                            ls.ModifiedDate = DateTime.Now;

                        }
                    }
                    result = bllTendersManagment.SaveEvaluationCommitteeAttendance(attendanceList, lstNameofFiles, _TenderWorkID, _MonitoredBy, _MonitoredByName, _OpenedBy, _pdfPath, _UserID);
                }
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
        /// Add Contractor Attendance parameters in DB server
        /// </summary>
        /// <param name="_TMAttendanceParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddContractorAttendance")]
        public ServiceResponse AddContractorAttendance([FromBody] Dictionary<string, object> _TMAttendanceParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_TMAttendanceParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                string dateFormat = "yyyyMMdd-hhmmss";
                string _date = DateTime.Now.ToString(dateFormat);

                int _UserID = Convert.ToInt32(_TMAttendanceParams["_UserID"].ToString());
                long _TenderWorkID = Convert.ToInt64(_TMAttendanceParams["_TenderWorkID"].ToString());
                List<TM_TenderWorksContractors> attendanceList = JsonConvert.DeserializeObject<List<TM_TenderWorksContractors>>(_TMAttendanceParams["_AttendeeList"].ToString()).ToList<TM_TenderWorksContractors>();

                string _MonitoredBy = _TMAttendanceParams["_MonitoredBy"].ToString();
                string _MonitoredByName = _TMAttendanceParams["_MonitoredByName"].ToString();
                string CA_Attachment = _TMAttendanceParams["CA_Attachment"].ToString();
                string _pdfPath = "";


                string pdf_guid = CA_Attachment.Split('.')[0];
                
                string _NewPDFName = pdf_guid + "_TenderEvalCommitteeAttendance-" + _date + ".png";
                string pdfFilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + CA_Attachment;
                String newPdfFilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + _NewPDFName ;
                if (File.Exists(pdfFilePath))
                    System.IO.File.Move(@pdfFilePath, @newPdfFilePath);

                _pdfPath = (File.Exists(newPdfFilePath)) ? _NewPDFName : CA_Attachment;
              
         
                string attachments = null;
                string atchmnts = _TMAttendanceParams["_Attachments"].ToString();
                if (!string.IsNullOrEmpty(atchmnts) && !atchmnts.Equals("null"))
                    attachments = atchmnts;



                //link attachments to TenderContractorsAttendance if there any

                List<string> lstNameofFiles = new List<string>();
                string _imagePath = "";
                if (attachments != null && attachments.Length > 0)
                {
                    string[] lstAttachments = attachments.Split(',');
                    if (lstAttachments != null && lstAttachments.Length > 0)
                    {
                        foreach (string attchmntName in lstAttachments)
                        {
                            
                                if (attchmntName.Length > 0 && !attchmntName.Contains("TenderContractorAttendance"))
                                {
                                    string guid = attchmntName.Split('.')[0];
                                    string _NewImageFileName = guid + "_TenderContractorAttendance-" + _date + ".png";

                                    string imgfilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + attchmntName;
                                    String newImgfilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + _NewImageFileName;
                                    if (File.Exists(imgfilePath))
                                        System.IO.File.Move(@imgfilePath, @newImgfilePath);

                                    _imagePath = (File.Exists(newImgfilePath)) ? _NewImageFileName : attchmntName;
                                }
                               
                                lstNameofFiles.Add((_imagePath));
                            
                        }
                    }
                }
                bool result = false;
                if (attendanceList.Count > 0)
                {
                    foreach (var ls in attendanceList)
                    {
                        if (ls.ID != 0)
                        {
                            object _mdlMemberAttendance = bllTendersManagment.GetTenderContractorByID(ls.ID);
                            ls.CreatedBy = Convert.ToInt32(Convert.ToString(_mdlMemberAttendance.GetType().GetProperty("CreatedBy").GetValue(_mdlMemberAttendance)));
                            ls.CreatedDate = Convert.ToDateTime(Convert.ToString(_mdlMemberAttendance.GetType().GetProperty("CreatedDate").GetValue(_mdlMemberAttendance)));
                            ls.ContractorsID = Convert.ToInt64(Convert.ToString(_mdlMemberAttendance.GetType().GetProperty("ContractorsID").GetValue(_mdlMemberAttendance)));
                            ls.TenderWorksID = Convert.ToInt64(Convert.ToString(_mdlMemberAttendance.GetType().GetProperty("TenderWorksID").GetValue(_mdlMemberAttendance)));
                            ls.ModifiedBy = Convert.ToInt32(_UserID);
                            ls.ModifiedDate = DateTime.Now;

                        }
                    }
                    result = bllTendersManagment.SaveContractorAttendance(attendanceList, lstNameofFiles, _TenderWorkID, _MonitoredBy, _MonitoredByName, null, _UserID);
                }
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
        /// Add Contractor Attendance parameters in DB server
        /// </summary>
        /// <param name="_TMAttendanceParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTenderPrice")]
        public ServiceResponse AddTenderPrice([FromBody] Dictionary<string, object> _TMPriceParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_TMPriceParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                string dateFormat = "yyyyMMdd-hhmmss";
                string _date = DateTime.Now.ToString(dateFormat);

                int _UserID = Convert.ToInt32(_TMPriceParams["_UserID"].ToString());
                long _TenderWorkID = Convert.ToInt64(_TMPriceParams["_TenderWorkID"].ToString());
                List<TM_TenderPrice> tenderPriceList = JsonConvert.DeserializeObject<List<TM_TenderPrice>>(_TMPriceParams["_tenderPriceList"].ToString()).ToList<TM_TenderPrice>();

                string _CDRNo = "";
                if (_TMPriceParams.ContainsKey("_CDRNo") && (!_TMPriceParams["_CDRNo"].ToString().Equals("null")))
                {
                    _CDRNo = _TMPriceParams["_CDRNo"].ToString();                    

                }
                string _BankDetail = "";
                if (_TMPriceParams.ContainsKey("_BankDetail") && (!_TMPriceParams["_BankDetail"].ToString().Equals("null")))
                {
                    _BankDetail = _TMPriceParams["_BankDetail"].ToString();

                }
                
                Double? _Amount = null;
                if (_TMPriceParams.ContainsKey("_Amount") && !string.IsNullOrEmpty(_TMPriceParams["_Amount"].ToString()) && !_TMPriceParams["_Amount"].ToString().Equals("null"))
                    _Amount = Convert.ToDouble(_TMPriceParams["_Amount"].ToString());

                Double? _TenderWorkAmount = null;
                if (_TMPriceParams.ContainsKey("_TenderWorkAmount") && !string.IsNullOrEmpty(_TMPriceParams["_TenderWorkAmount"].ToString()) && !_TMPriceParams["_TenderWorkAmount"].ToString().Equals("null"))
                    _TenderWorkAmount = Convert.ToDouble(_TMPriceParams["_TenderWorkAmount"].ToString());

               long _TWContractorID = Convert.ToInt64(_TMPriceParams["_TWContractorID"].ToString());

                Double? _EstimatePercentage = null;
                string percentage = _TMPriceParams["_EstimatePercentage"].ToString();
                if (!string.IsNullOrEmpty(percentage) && !percentage.Equals("null"))
                    _EstimatePercentage = Convert.ToDouble(_TMPriceParams["_EstimatePercentage"].ToString());

                bool _TenderPriced = Convert.ToBoolean(_TMPriceParams["_TenderPriced"].ToString());

                string CA_Attachment = "";
                 if (_TMPriceParams.ContainsKey("_Attachment") && !string.IsNullOrEmpty(_TMPriceParams["_Attachment"].ToString()) && !_TMPriceParams["_Attachment"].ToString().Equals("null"))
                            CA_Attachment= _TMPriceParams["_Attachment"].ToString();

                string _EstimateType = _TMPriceParams["_EstimateType"].ToString();
                string _pdfPath = "";

                string pdf_guid = CA_Attachment.Split('.')[0];

                string _NewPDFName = pdf_guid + "_TenderPrice-" + _date + ".png";
                string pdfFilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + CA_Attachment;
                String newPdfFilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + _NewPDFName;
                if (File.Exists(pdfFilePath))
                    System.IO.File.Move(@pdfFilePath, @newPdfFilePath);

                _pdfPath = (File.Exists(newPdfFilePath)) ? _NewPDFName : CA_Attachment;

                TM_TenderPriceCDR tenderPriceCDR = null;
                if (!_CDRNo.ToString().Equals("")) {

                    tenderPriceCDR = new TM_TenderPriceCDR();

                    tenderPriceCDR.CDRNo = _CDRNo;
                    tenderPriceCDR.BankDetail = _BankDetail;
                    tenderPriceCDR.Amount = _Amount;
                    tenderPriceCDR.Attachment = _pdfPath;
                    tenderPriceCDR.TWContractorID = _TWContractorID;
                    tenderPriceCDR.CreatedDate = DateTime.Now;
                    tenderPriceCDR.CreatedBy = Convert.ToInt32(_UserID);
                    tenderPriceCDR.ModifiedDate = DateTime.Now;
                    tenderPriceCDR.ModifiedBy = Convert.ToInt32(_UserID);
                }
               
               

                TM_TenderWorksContractors tenderContractor = new TM_TenderWorksContractors();
                tenderContractor.TP_EstimateType = _EstimateType;
                tenderContractor.TP_EstimatePercentage = _EstimatePercentage;
                tenderContractor.TenderPriced = _TenderPriced;
                tenderContractor.ContractorsID = bllTendersManagment.GetContractorIDFromTWContractorID(_TWContractorID);
                tenderContractor.TenderWorksID = _TenderWorkID;
                tenderContractor.TenderWorkAmount = _TenderWorkAmount;
                if (tenderPriceList.Count > 0)
                {
                    foreach (var ls in tenderPriceList)
                    {
                        if (bllTendersManagment.IsTenderPriceAdded(Convert.ToInt64(ls.WorkItemID), Convert.ToInt64(ls.TWContractorID)))
                        {
                            object _mdlTenderPrice = bllTendersManagment.GetTenderPriceByWorkItemID(Convert.ToInt64(ls.WorkItemID), Convert.ToInt64(ls.TWContractorID));
                            ls.CreatedBy = Convert.ToInt32(Convert.ToString(_mdlTenderPrice.GetType().GetProperty("CreatedBy").GetValue(_mdlTenderPrice)));
                            ls.CreatedDate = Convert.ToDateTime(Convert.ToString(_mdlTenderPrice.GetType().GetProperty("CreatedDate").GetValue(_mdlTenderPrice)));
                            ls.ModifiedBy = Convert.ToInt32(_UserID);
                            ls.ModifiedDate = DateTime.Now;
                            ls.ID = Convert.ToInt64(Convert.ToString(_mdlTenderPrice.GetType().GetProperty("ID").GetValue(_mdlTenderPrice)));

                        }
                        else
                        {
                            ls.ModifiedBy = Convert.ToInt32(_UserID);
                            ls.ModifiedDate = DateTime.Now;
                            ls.CreatedBy = Convert.ToInt32(_UserID);
                            ls.CreatedDate = DateTime.Now;
                        }


                    }
                }
                    long CDRID = bllTendersManagment.SaveTenderPrice(tenderPriceList, tenderPriceCDR, tenderContractor,_UserID);
                    if (tenderPriceCDR != null)
                    {
                        if (CDRID > 0)
                        {
                            object cdrDetail = bllTendersManagment.GetTenderPriceCDRByID(CDRID);
                            svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, cdrDetail);
                        }

                        else
                        {
                            svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                        }
                    }
                    else {
                        if (CDRID == 0)
                        {
                            svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                        }

                        else
                        {
                            svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                        }
                    }
                    

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
                string filePath = Utility.GetImagePath(Common.Configuration.TenderManagement);
                var httpRequest = HttpContext.Current.Request;
                var fileName = string.Empty;
                foreach (string file in httpRequest.Files)
                {
                    string guid = Guid.NewGuid().ToString();
                    var postedFile = httpRequest.Files[file];
                    if (postedFile.FileName.ToLower().EndsWith(".jpeg") || postedFile.FileName.ToLower().EndsWith(".png") || postedFile.FileName.ToLower().EndsWith(".jpg"))
                    {
                        fileName = guid + ".png";
                    }
                    else if (postedFile.FileName.ToLower().EndsWith(".pdf"))
                    {
                        fileName = guid + ".pdf";
                    }
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    filePath = filePath + "\\" + fileName;
                    postedFile.SaveAs(filePath);
                }
                svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, fileName);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("TenderManagement:UploadFiles : - ", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1001, "Image Not Uploaded.", "" + exp.Message);
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetTenderWorkItems")]
        public ServiceResponse GetTenderWorkItems(long _TenderWorkID,long _ContractorID,long _TenderNoticeID)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {
                
                List<object> lstWorkItems = new List<object>();
                if (_ContractorID != 0)
                {
                    lstWorkItems = bllTendersManagment.GetWorkItemsDetailsByWorKID(_TenderWorkID, _ContractorID, _TenderNoticeID);
                }
                else
                {
                    lstWorkItems = bllTendersManagment.GetWorkItemsforViewByWorKID(_TenderWorkID);
                    
                }

               
                if (lstWorkItems.Count > 0)
                {
                    svcResponse = srvcHlpr.CreateResponse<object>(lstWorkItems, 0, ServiceHelper.MSG_SUCCESS);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }
        
        [HttpGet]
        [Route("GetTenderCallDeposits")]
        public ServiceResponse GetTenderCallDeposit(long _TWContractorID)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                List<object> lstCallDeposit = bllTendersManagment.GetContractorCallDeposit(_TWContractorID);
                if (lstCallDeposit.Count > 0)
                {
                    svcResponse = srvcHlpr.CreateResponse<object>(lstCallDeposit, 0, ServiceHelper.MSG_SUCCESS);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetTenderContractorsListForADMReport")]
        public ServiceResponse GetTenderContractorsListForADMReport(long _TenderWorkID)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                List<dynamic> lstContractors = bllTendersManagment.GetADMReportDataByTenderWorkID(_TenderWorkID);
                if (lstContractors!=null && lstContractors.Count > 0)
                {
                    svcResponse = srvcHlpr.CreateResponse<object>(lstContractors, 0, ServiceHelper.MSG_SUCCESS);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }
        
        [HttpGet]
        [Route("GetADMReportData")]
        public ServiceResponse GetADMReportData(long _TenderWorkID)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                dynamic admData = bllTendersManagment.GetTenderInformationForADMReport(_TenderWorkID);
                if (admData!=null)
                {
                    svcResponse = srvcHlpr.CreateResponse( 0, ServiceHelper.MSG_SUCCESS, admData);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }

        ///// <summary>
        ///// Add Contractor Attendance parameters in DB server
        ///// </summary>
        ///// <param name="_TMAttendanceParams"></param>
        ///// <returns></returns>
        [HttpPost]
        [Route("UpdateADMReportData")]
        public ServiceResponse UpdateADMReportData([FromBody] Dictionary<string, object> _TMPriceParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_TMPriceParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                string dateFormat = "yyyyMMdd-hhmmss";
                string _date = DateTime.Now.ToString(dateFormat);

                long _UserID = Convert.ToInt64(_TMPriceParams["_UserID"].ToString());
                long _TenderWorkID = Convert.ToInt64(_TMPriceParams["_TenderWorkID"].ToString());
                List<TM_TenderWorksContractors> _RejectedContractorsList = JsonConvert.DeserializeObject<List<TM_TenderWorksContractors>>(_TMPriceParams["_RejectedContractorsList"].ToString()).ToList<TM_TenderWorksContractors>();

                
                string _SubmissionTime = null;
                DateTime? ActualSubmissionDate=null;
                if (_TMPriceParams.ContainsKey("_SubmissionTime") && (!_TMPriceParams["_SubmissionTime"].ToString().Equals("null"))){
                    _SubmissionTime = _TMPriceParams["_SubmissionTime"].ToString();
                    ActualSubmissionDate = DateTime.ParseExact(_SubmissionTime, "dd-MMM-yyyy h:m tt", CultureInfo.InvariantCulture);

                }
                    

                
                string _OpeningTime = null;
                DateTime? ActualOpeningDate = null;
                if (_TMPriceParams.ContainsKey("_OpeningTime") && (!_TMPriceParams["_OpeningTime"].ToString().Equals("null")))
                {
                    _OpeningTime = _TMPriceParams["_OpeningTime"].ToString();
                    ActualOpeningDate = DateTime.ParseExact(_OpeningTime, "dd-MMM-yyyy h:m tt", CultureInfo.InvariantCulture);
                }
                
                string _SubmissionReason = null;
                if (_TMPriceParams.ContainsKey("_SubmissionReason") && (!_TMPriceParams["_SubmissionReason"].ToString().Equals("null")))
                    _SubmissionReason = _TMPriceParams["_SubmissionReason"].ToString();

                string _OpeningReason = null;
                if (_TMPriceParams.ContainsKey("_OpeningReason") && (!_TMPriceParams["_OpeningReason"].ToString().Equals("null")))
                    _OpeningReason = _TMPriceParams["_OpeningReason"].ToString();


               

                bool _TenderCancelled = Convert.ToBoolean(_TMPriceParams["_TenderCancelled"].ToString());
                string _CancellationReason = null;
                if (_TMPriceParams.ContainsKey("_CancellationReason") && (!_TMPriceParams["_CancellationReason"].ToString().Equals("null")))
                    _CancellationReason = _TMPriceParams["_CancellationReason"].ToString();

               


                string attachments = null;
                string atchmnts = _TMPriceParams["_Attachments"].ToString();
                if (!string.IsNullOrEmpty(atchmnts) && !atchmnts.Equals("null"))
                    attachments = atchmnts;



                //link attachments to TenderContractorsAttendance if there any

                List<string> lstNameofFiles = new List<string>();
                string _imagePath = "";
                if (attachments != null && attachments.Length > 0)
                {
                    string[] lstAttachments = attachments.Split(',');
                    if (lstAttachments != null && lstAttachments.Length > 0)
                    {
                        foreach (string attchmntName in lstAttachments)
                        {

                            if (attchmntName.Length > 0 && !attchmntName.Contains("ADMReport"))
                            {
                                string guid = attchmntName.Split('.')[0];
                                string _NewImageFileName = guid + "_ADMReport-" + _date + ".png";

                                string imgfilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + attchmntName;
                                String newImgfilePath = Utility.GetImagePath(Common.Configuration.TenderManagement) + "\\" + _NewImageFileName;
                                if (File.Exists(imgfilePath))
                                    System.IO.File.Move(@imgfilePath, @newImgfilePath);

                                _imagePath = (File.Exists(newImgfilePath)) ? _NewImageFileName : attchmntName;
                            }

                            lstNameofFiles.Add((_imagePath));

                        }
                    }
                }

                TM_TenderWorks mdlTenderWorks = new TM_TenderWorks();
                mdlTenderWorks.ID = _TenderWorkID;
                if (_TenderCancelled)
                {
                    mdlTenderWorks.WorkStatusID = 4;
                    mdlTenderWorks.StatusReason = _CancellationReason;
                }
                mdlTenderWorks.ADM_ActualOpeningDate = ActualOpeningDate;
                mdlTenderWorks.ADM_ActualSubmissionDate = ActualSubmissionDate;
                mdlTenderWorks.ADM_ActualOpeningReason = _OpeningReason;
                mdlTenderWorks.ADM_ActualSubmissionReason = _SubmissionReason;

                if (_RejectedContractorsList.Count > 0)
                {
                    foreach (var ls in _RejectedContractorsList)
                    {
                            
                            ls.ModifiedBy = Convert.ToInt32(_UserID);
                            ls.ModifiedDate = DateTime.Now;

                       
                    }
                }
                bool _result = bllTendersManagment.SaveADMReportData(_RejectedContractorsList, mdlTenderWorks, lstNameofFiles, _UserID);

                if (_result)
                {
                    
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
                }

                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
                }

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
        [Route("DeleteADMAttachment")]
        public ServiceResponse DeleteADMAttachment(long _TenderWorkID,string AttachmentName)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                bool result = bllTendersManagment.DeleteADMAttachmentByTenderWorkID(_TenderWorkID);
                if (result)
                {
                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, null);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse;
        }


        [HttpGet]
        [Route("GetTenderWorkByID")]
        public ServiceResponse GetTenderWorkByID(long _TenderWorkID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                long? TenderWorkID = null;
                
                if (_TenderWorkID != 0)
                {
                    TenderWorkID = _TenderWorkID;
                }

                object ObjTenderWorks = bllTendersManagment.GetTenderWorkByID( TenderWorkID);
                if (ObjTenderWorks!=null)
                {
                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, ObjTenderWorks);
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
                }
            }
            catch (Exception e)
            {
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = e.Message;
            }
            return svcResponse; 
        }
    }
}
