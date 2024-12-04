using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Services.HelperClasses;
using PMIU.WRMIS.Services.Models;
using System;
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
    /// <summary>
    /// Coded by : Hira Iqbal 
    /// 
    /// </summary>
     [RoutePrefix("api/WaterTheft")]
    public class WaterTheftController : ApiController
    {
        PMIU.WRMIS.BLL.WaterTheft.WaterTheftBLL bllWaterTheft = new BLL.WaterTheft.WaterTheftBLL();
        ServiceHelper srvcHlpr = new ServiceHelper();

        [HttpGet]
        [Route("GetOffenceType")]
        public ServiceResponse GetOffenceType(string _SiteType)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstOffenseType = bllWaterTheft.GetOffenceTypeBySite(_SiteType);
                if (lstOffenseType.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstOffenseType, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetOffenceType", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetConditionType")]
        public ServiceResponse GetCutType(string _SiteType)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstOffenseType = bllWaterTheft.GetOutletCondition(_SiteType);
                if (lstOffenseType.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstOffenseType, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetCutType", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetDefectType")]
        public ServiceResponse GetDefectType()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstDefectType = bllWaterTheft.GetDefectiveType();
                if (lstDefectType.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstDefectType, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetDefectType", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;

        }

        /// <summary>
        /// Add water theft case in systems and prform necassary steps 
        /// </summary>
        /// <param name="_TheftDetail"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddWaterTheftChannel")]
        public ServiceResponse AddWaterTheftChannel([FromBody] Dictionary<string, object> _TheftDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_TheftDetail))
                    return srvcHlpr.CreateResponse(1004, "Parameters are null", null);

                long userID = Convert.ToInt64(_TheftDetail["_UserID"].ToString());
                long sectionID = Convert.ToInt64(_TheftDetail["_SectionID"].ToString());
                long channelID = Convert.ToInt64(_TheftDetail["_ChannelID"].ToString());
                int siteRD = Convert.ToInt32(_TheftDetail["_SiteRD"].ToString());

                if (!bllWaterTheft.RDLiesWithinUserJurisdiction(userID, sectionID, channelID, siteRD))
                    return srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_NotWithinUserJurisdiction, null);

                if (bllWaterTheft.AvailableSBEID(userID, sectionID) > 0)
                {
                    string channelSide = _TheftDetail["_ChannelSide"].ToString(); //Channel cut param 
                    long offenceTypeID = Convert.ToInt64(_TheftDetail["_OffenceTypeID"].ToString());

                    //check for duplication of case
                    if (bllWaterTheft.WaterTheftCaseAlreadyExist(channelID, siteRD, channelSide, offenceTypeID))
                        return srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_CaseExists, null);

                    long? cutConditionID = null;
                    if (_TheftDetail.ContainsKey("_CutConditionID"))
                    {
                        string cutCondition = _TheftDetail["_CutConditionID"].ToString();
                        if (!string.IsNullOrEmpty(cutCondition) && !cutCondition.Equals("null"))
                            cutConditionID = Convert.ToInt64(cutCondition);
                    }

                    string remarks = null;
                    if (_TheftDetail["_Remarks"] != null)
                        remarks = _TheftDetail["_Remarks"].ToString();

                    string attachments = null;
                    string atchmnts = _TheftDetail["_Attachments"].ToString();
                    if (!string.IsNullOrEmpty(atchmnts) || !atchmnts.Equals("null"))
                        attachments = atchmnts;


                    long _DivisionID = Convert.ToInt64(_TheftDetail["_DivisionID"].ToString());

                    double? _Longitude = null, _Latitude = null;
                    if (_TheftDetail["_Longitude"] != null && !string.IsNullOrEmpty(_TheftDetail["_Longitude"].ToString()))
                        _Longitude = Convert.ToDouble(_TheftDetail["_Longitude"].ToString());

                    if (_TheftDetail["_Latitude"] != null && !string.IsNullOrEmpty(_TheftDetail["_Latitude"].ToString()))
                        _Latitude = Convert.ToDouble(_TheftDetail["_Latitude"].ToString());

                    int _isOnline = Convert.ToInt32(_TheftDetail["_isOnline"].ToString());
                    string _DateTime = _TheftDetail["_DateTime"].ToString();

                    DateTime channeldt;

                    if (_isOnline == 1)
                    {
                        channeldt = DateTime.Now;
                    }
                    else
                    {
                        channeldt = Convert.ToDateTime(_DateTime);
                    }
                 //   long valueSaved = bllWaterTheft.AddWaterTheftChannel_Android(userID, sectionID, channelID, siteRD, channelSide, offenceTypeID, cutConditionID, remarks, attachments);
                    WT_WaterTheftCase mdlWTCase = new WT_WaterTheftCase();

                    mdlWTCase.OffenceSite = "C";
                    mdlWTCase.ChannelID = channelID;
                    mdlWTCase.TheftSiteRD = siteRD;
                    mdlWTCase.OutletID = null;
                    mdlWTCase.ValueofH = null;
                    mdlWTCase.OffenceTypeID = offenceTypeID;
                    mdlWTCase.OffenceSide = channelSide;
                    mdlWTCase.TheftSiteConditionID = cutConditionID;
                    mdlWTCase.IncidentDateTime = channeldt;
                    mdlWTCase.UserID = userID;
                    mdlWTCase.LogDateTime = channeldt;
                    mdlWTCase.Remarks = remarks;
                    mdlWTCase.CaseStatusID = bllWaterTheft.GetWTCaseStatusIDByName("In Progress");
                    mdlWTCase.CaseNo = bllWaterTheft.GenerateWaterTheftCaseNo();
                    mdlWTCase.IsActive = true;
                    mdlWTCase.CreatedDate = DateTime.Now;
                    mdlWTCase.CreatedBy = userID;
                    mdlWTCase.GIS_X = _Longitude;
                    mdlWTCase.GIS_Y = _Latitude;
                    mdlWTCase.Source = "A";

                    long usrDsgnID = Convert.ToInt64(new LoginBLL().GetAndroidUserDesignationID(userID));
                    long usrSBEID = bllWaterTheft.AvailableSBEID(userID, sectionID);
                    
                    WT_WaterTheftStatus mdlCaseStatus = new WT_WaterTheftStatus();
                
                    mdlCaseStatus.AssignedToUserID = usrSBEID;
                    mdlCaseStatus.AssignedToDesignationID = Convert.ToInt64(new LoginBLL().GetAndroidUserDesignationID(usrSBEID));
                    mdlCaseStatus.AssignedByUserID = userID;
                    mdlCaseStatus.AssignedByDesignationID = usrDsgnID;
                    mdlCaseStatus.AssignedDate = channeldt;
                    mdlCaseStatus.CaseStatusID = Convert.ToInt32(Constants.WTCaseStatus.InProgress);
                    mdlCaseStatus.Remarks = "Case started";
                    mdlCaseStatus.IsActive = true;
                    mdlCaseStatus.CreatedDate = DateTime.Now;
                    mdlCaseStatus.CreatedBy = userID;
                   

                    WT_OutletDefectiveDetails mdlDfctDtl = new WT_OutletDefectiveDetails();
                    List<Tuple<string, string, string>> lstNameofFiles = new List<Tuple<string, string, string>>();
                     //linke attachments to water theft case id if there any
                    string dateFormat = "yyyyMMdd-hhmmss";
                    string _date = DateTime.Now.ToString(dateFormat);
                    
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
                                       
                                        string guid = attchmntName.Split('.')[0];
                                        string _NewImageName = guid + "_" + "WaterTheftChannel" + "-" + _date;
                                        string filePath = Utility.GetImagePath(Common.Configuration.WaterTheft);
                                        filePath = filePath + "\\" + attchmntName;
                                        String newfilePath = Utility.GetImagePath(Common.Configuration.WaterTheft) + "\\" + _NewImageName + ".png";
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
                                        lstNameofFiles.Add(new Tuple<string, string, string>(_imagePath, "image/png", _imagePath));
                                    }
                                }
                            }
                        }
                     PMIU.WRMIS.BLL.IrrigationNetwork.Channel.ChannelBLL channelBll=new BLL.IrrigationNetwork.Channel.ChannelBLL();
                     CO_Channel channelInfo = channelBll.GetChannelByID(channelID);
                     string channelName = channelInfo.NAME;
                     long StatusID = bllWaterTheft.AddWaterTheftCaseChannelorOutlet(userID, mdlWTCase, mdlCaseStatus, mdlDfctDtl, _DivisionID, 1, lstNameofFiles,Convert.ToString(Constants.ComplaintModuleReference.WT_C), Convert.ToDouble(_Longitude), Convert.ToDouble(_Latitude), "A", channelName);

                    if (StatusID == -1)
                    {
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_NotWithinUserJurisdiction, null);
                       
                    }
                    else if (StatusID == -2)
                    {
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_NoSBEAvailable, null);
                        
                    }
                    else if (StatusID == -3)
                    {

                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_CaseExists, null);
                    }
                    else if (StatusID == -4)
                    {
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                    }
                    else {
                        svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_SAVED, null);
                    }
                   
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_NoSBEAvailable, null);
                }
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddWaterTheftChannel", exp.InnerException.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
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
        [Route("AddWaterTheftOutlet")]
        public ServiceResponse AddWaterTheftOutlet([FromBody] Dictionary<string, object> _TheftDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                 if (srvcHlpr.IsDictionaryEmptyOrNull(_TheftDetail))
                    return srvcHlpr.CreateResponse(1004, "Parameters are null", null);

                long userID = Convert.ToInt64(_TheftDetail["_UserID"].ToString());
                long sectionID = Convert.ToInt64(_TheftDetail["_SectionID"].ToString());
                long channelID = Convert.ToInt64(_TheftDetail["_ChannelID"].ToString());
                int siteRD = Convert.ToInt32(_TheftDetail["_SiteRD"].ToString());
                long outletID = Convert.ToInt64(_TheftDetail["_OutletID"].ToString());
                string channelSide = _TheftDetail["_ChannelSide"].ToString(); 

                if (bllWaterTheft.AvailableSBEID(userID, sectionID) > 0)
                {  
                    long? offenceTypeID = null;
                    if (_TheftDetail.ContainsKey("_OffenceTypeID") && (!_TheftDetail["_OffenceTypeID"].ToString().Equals("null")))
                        offenceTypeID = Convert.ToInt64(_TheftDetail["_OffenceTypeID"].ToString());

                    long? conditionID = null;
                    if (_TheftDetail.ContainsKey("_ConditionID") && (!_TheftDetail["_ConditionID"].ToString().Equals("null")))
                        conditionID = Convert.ToInt64(_TheftDetail["_ConditionID"].ToString());

                    if (bllWaterTheft.WaterTheftOutletCaseAlreadyExist(channelID, outletID,conditionID,offenceTypeID))
                        return srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_CaseExists, null);

                    string remarks = null;
                    if (_TheftDetail["_Remarks"] != null)
                        remarks = _TheftDetail["_Remarks"].ToString();
                    
                    string attachments = null;
                    string atchmnts = _TheftDetail["_Attachments"].ToString();
                    if (!string.IsNullOrEmpty(atchmnts) &&  !atchmnts.Equals("null"))
                        attachments = atchmnts;

                    double valueOfH = Convert.ToDouble(_TheftDetail["_ValueOfH"].ToString());

                    long? defectID = null;
                    double? valueOfB = null, valueOfY = null, valueOfDIA = null;
                    string defect = _TheftDetail["_DefectID"].ToString();
                    if (_TheftDetail["_DefectID"] != null  && !string.IsNullOrEmpty(defect) && !defect.Equals("null"))
                    {
                        defectID = Convert.ToInt64(_TheftDetail["_DefectID"].ToString());
                        valueOfB = GetValue(_TheftDetail, "_ValueOfB");
                        valueOfY = GetValue(_TheftDetail, "_ValueOfY");
                        valueOfDIA = GetValue(_TheftDetail, "_ValueOfDIA");
                    }
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
                 //   long valueSaved = bllWaterTheft.AddWaterTheftOutlet_Android(userID, sectionID, channelID, outletID, siteRD, offenceTypeID, conditionID, remarks, attachments, valueOfH, defectID, valueOfB, valueOfY, valueOfDIA,channelSide);
                    long _DivisionID = Convert.ToInt64(_TheftDetail["_DivisionID"].ToString());
                    WT_WaterTheftCase mdlWTCase = new WT_WaterTheftCase();

                    mdlWTCase.OffenceSite = "O";
                    mdlWTCase.ChannelID = channelID;
                    mdlWTCase.TheftSiteRD = siteRD;
                    mdlWTCase.OutletID = outletID;
                    mdlWTCase.ValueofH = valueOfH;
                    mdlWTCase.OffenceTypeID = offenceTypeID;
                    mdlWTCase.OffenceSide = channelSide;
                    mdlWTCase.TheftSiteConditionID = conditionID;
                    mdlWTCase.IncidentDateTime = Outletdt;
                    mdlWTCase.UserID = userID;
                    mdlWTCase.LogDateTime = Outletdt;
                    mdlWTCase.Remarks = remarks;
                    mdlWTCase.CaseStatusID = bllWaterTheft.GetWTCaseStatusIDByName("In Progress");
                    mdlWTCase.CaseNo = bllWaterTheft.GenerateWaterTheftCaseNo();
                    mdlWTCase.IsActive = true;
                    mdlWTCase.CreatedDate = DateTime.Now;
                    mdlWTCase.CreatedBy = userID;
                    mdlWTCase.GIS_X = _Longitude;
                    mdlWTCase.GIS_Y = _Latitude;
                    mdlWTCase.Source = "A";

                    long usrDsgnID = Convert.ToInt64(new LoginBLL().GetAndroidUserDesignationID(userID));
                    long usrSBEID = bllWaterTheft.AvailableSBEID(userID, sectionID);

                    WT_WaterTheftStatus mdlCaseStatus = new WT_WaterTheftStatus();

                    mdlCaseStatus.AssignedToUserID = usrSBEID;
                    mdlCaseStatus.AssignedToDesignationID = Convert.ToInt64(new LoginBLL().GetAndroidUserDesignationID(usrSBEID));
                    mdlCaseStatus.AssignedByUserID = userID;
                    mdlCaseStatus.AssignedByDesignationID = usrDsgnID;
                    mdlCaseStatus.AssignedDate = Outletdt;
                    mdlCaseStatus.CaseStatusID = Convert.ToInt32(Constants.WTCaseStatus.InProgress);
                    mdlCaseStatus.Remarks = "Case started";
                    mdlCaseStatus.IsActive = true;
                    mdlCaseStatus.CreatedDate = DateTime.Now;
                    mdlCaseStatus.CreatedBy = userID;

                    List<Tuple<string, string, string>> lstNameofFiles = new List<Tuple<string, string, string>>();
                    //linke attachments to water theft case id if there any
                    string dateFormat = "yyyyMMdd-hhmmss";
                    string _date = DateTime.Now.ToString(dateFormat);
                    
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

                                    string guid = attchmntName.Split('.')[0];
                                    string _NewImageName = guid + "_" + "WaterTheftOutlet" + "-" + _date;
                                    string filePath = Utility.GetImagePath(Common.Configuration.WaterTheft);
                                    filePath = filePath + "\\" + attchmntName;
                                    String newfilePath = Utility.GetImagePath(Common.Configuration.WaterTheft) + "\\" + _NewImageName + ".png";
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
                                    lstNameofFiles.Add(new Tuple<string, string, string>(_imagePath, "image/png", _imagePath));
                                }
                            }
                        }
                    }

                    WT_OutletDefectiveDetails mdlDfctDtl = new WT_OutletDefectiveDetails();
                    mdlDfctDtl.DefectiveTypeID = Convert.ToInt64(defectID);
                    mdlDfctDtl.IsActive = true;
                    mdlDfctDtl.CreatedDate = DateTime.Now;
                    mdlDfctDtl.CreatedBy = userID;
                    mdlDfctDtl.ValueOfB = valueOfB;
                    mdlDfctDtl.ValueOfY = valueOfY;
                    mdlDfctDtl.ValueOfDia = valueOfDIA;

                    PMIU.WRMIS.BLL.IrrigationNetwork.Channel.ChannelBLL channelBll = new BLL.IrrigationNetwork.Channel.ChannelBLL();
                    CO_Channel channelInfo = channelBll.GetChannelByID(channelID);
                    string channelName = channelInfo.NAME;
                    long StatusID = bllWaterTheft.AddWaterTheftCaseChannelorOutlet(userID, mdlWTCase, mdlCaseStatus, mdlDfctDtl, _DivisionID, 2, lstNameofFiles,  Convert.ToString(Convert.ToString(Constants.ComplaintModuleReference.WT_O)), Convert.ToDouble(_Longitude), Convert.ToDouble(_Latitude),"A", channelName);
                    if (StatusID == -1)
                    {
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_NotWithinUserJurisdiction, null);

                    }
                    else if (StatusID == -2)
                    {
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_NoSBEAvailable, null);

                    }
                    else if (StatusID == -3)
                    {

                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_CaseExists, null);
                    }
                    else if (StatusID == -4)
                    {
                        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                    }
                    else
                    {
                        svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_SAVED, null);
                    }
                    
                }
                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_NoSBEAvailable, null);
                }
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddWaterTheftOutlet", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR + "\n" + exp.Message, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Add water breach to system
        /// </summary>
        /// <param name="_TheftDetail"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddBreach")]
        public ServiceResponse AddBreach([FromBody] Dictionary<string, object> _TheftDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (srvcHlpr.IsDictionaryEmptyOrNull(_TheftDetail))
                    return srvcHlpr.CreateResponse(1004, "Parameters are null", null);

                long userID = Convert.ToInt64(_TheftDetail["_UserID"].ToString());
                long sectionID = Convert.ToInt64(_TheftDetail["_SectionID"].ToString());
                long channelID = Convert.ToInt64(_TheftDetail["_ChannelID"].ToString());
                int breachSiteRD = Convert.ToInt32(_TheftDetail["_SiteRD"].ToString());

                int _isOnline = Convert.ToInt32(_TheftDetail["_isOnline"].ToString());
                string _DateTime = _TheftDetail["_DateTime"].ToString();

                DateTime breachdt;
                
                if (_isOnline == 1)
                {
                    breachdt = DateTime.Now;
                }
                else
                {
                    breachdt = Convert.ToDateTime(_DateTime);
                }
                 
                string breachSide = _TheftDetail["_SiteSide"].ToString();

                if (!bllWaterTheft.RDLiesWithinUserJurisdiction(userID, sectionID, channelID, breachSiteRD))
                    return srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_NotWithinUserJurisdiction, null); 
                
                if(bllWaterTheft.BreachCaseAlreadyExist(channelID, breachSiteRD, breachSide))
                    return srvcHlpr.CreateResponse(1004, ServiceHelper.WT_MSG_CaseExists, null); 

                double? headDischarge = null , breachLength = null;
                if (!string.IsNullOrEmpty(_TheftDetail["_HeadDischarge"].ToString()))
                    headDischarge = Convert.ToDouble(_TheftDetail["_HeadDischarge"].ToString());

                if (!string.IsNullOrEmpty(_TheftDetail["_BreachLength"].ToString()))
                    breachLength = Convert.ToDouble(_TheftDetail["_BreachLength"].ToString());

                string remarks = null;
                if (_TheftDetail["_Remarks"] != null)
                    remarks = _TheftDetail["_Remarks"].ToString();
                bool? _FieldStaff = null;
                if (_TheftDetail.ContainsKey("_FieldStaff"))
                {
                    _FieldStaff = Convert.ToBoolean(_TheftDetail["_FieldStaff"].ToString());
                }
                
                double? _Longitude = null, _Latitude = null;
                if (_TheftDetail["_Longitude"] != null && !string.IsNullOrEmpty(_TheftDetail["_Longitude"].ToString()))
                    _Longitude = Convert.ToDouble(_TheftDetail["_Longitude"].ToString());

                if (_TheftDetail["_Latitude"] != null && !string.IsNullOrEmpty(_TheftDetail["_Latitude"].ToString()))
                    _Latitude = Convert.ToDouble(_TheftDetail["_Latitude"].ToString());

                string attachments = null;
                if (_TheftDetail["_Attachments"] != null)
                    attachments = _TheftDetail["_Attachments"].ToString();

                string lstNameofFiles = "";
                //linke attachments to water theft case id if there any
                string dateFormat = "yyyyMMdd-hhmmss";
                string _date = DateTime.Now.ToString(dateFormat);

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

                                string guid = attchmntName.Split('.')[0];
                                string _NewImageName = guid + "_" + "WaterTheftBreach" + "-" + _date;
                                string filePath = Utility.GetImagePath(Common.Configuration.WaterTheft);
                                filePath = filePath + "\\" + attchmntName;
                                String newfilePath = Utility.GetImagePath(Common.Configuration.WaterTheft) + "\\" + _NewImageName + ".png";
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
                               lstNameofFiles= lstNameofFiles + _imagePath + ",";
                            }
                        }
                    }
                }
                long valueSaved = bllWaterTheft.AddBreach_Android(userID, sectionID, channelID, breachSiteRD, breachSide, headDischarge, breachLength, _FieldStaff, remarks, lstNameofFiles, _Longitude, _Latitude, "A", breachdt);

               if (valueSaved > 0)
               {
                  
                   svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_SAVED, null);
               }
               else
               {
                   svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
               }
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("AddBreach", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR + "\n" + exp.Message, null);
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
                string filePath = Utility.GetImagePath(Common.Configuration.WaterTheft);
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
                svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, fileName);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("WaterTheft:UploadFiles : - ", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1001, "Image Not Uploaded.", "" + exp.Message);
            }
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
                string filePath = Utility.GetImagePath(Common.Configuration.WaterTheft);
                filePath = filePath + "\\" + _ImageID + ".png";

                if (File.Exists(filePath))
                    new Bitmap(filePath).Save(ms, ImageFormat.Png);

                httpResponse.Content = new ByteArrayContent(ms.ToArray());
                httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            }
            catch (Exception ex)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                httpResponse.Content = null;
                string exceptionMsg = ex.Message;
            }
            return httpResponse;
        }

        private double? GetValue(Dictionary<string, object> _DataContainer, string _Key)
        {
            if (_DataContainer.ContainsKey(_Key))
            {
                if (_DataContainer[_Key] != null )
                {
                    string key = _DataContainer[_Key].ToString();

                    if (string.IsNullOrEmpty(key) || key.Equals("null"))
                        return null;

                    return Convert.ToDouble(_DataContainer[_Key].ToString());
                }
            }
            return null;
        }



    }
}
