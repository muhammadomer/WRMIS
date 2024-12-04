using Newtonsoft.Json;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Services.HelperClasses;
using PMIU.WRMIS.Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
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


    [RoutePrefix("api/FloodInspectionControllerApi")]
    public class FloodInspectionController : ApiController
    {
        private const string MSG_UNKNOWN_ERROR = "Some unknown error occurred, please try again later";
        private const string MSG_NO_RECORD_FOUND = "FAILURE: No Records Found";
        private const string MSG_RECORD_SAVED = "SUCCESS: Record saved successfully";
        private const string MSG_SUCCESS = "SUCCESS:";
        private const string MSG_NOT_DELETED = "Record cannot be deleted.";

        PMIU.WRMIS.BLL.FloodOperations.FloodInspectionsBLL bllFloodInspection = new BLL.FloodOperations.FloodInspectionsBLL();

        PMIU.WRMIS.BLL.FloodOperations.FloodOperationsBLL bllFloodOperations = new BLL.FloodOperations.FloodOperationsBLL();

        PMIU.WRMIS.BLL.FloodOperations.FloodFightingPlanBLL bllFloodFightingPlan = new BLL.FloodOperations.FloodFightingPlanBLL();

        PMIU.WRMIS.BLL.FloodOperations.ReferenceData.ItemsBLL bllItems = new BLL.FloodOperations.ReferenceData.ItemsBLL();

        PMIU.WRMIS.BLL.FloodOperations.OnsiteMonitoringBLL bllOnsiteMonitoring = new BLL.FloodOperations.OnsiteMonitoringBLL();

        ServiceHelper srvcHlpr = new ServiceHelper();

        [HttpGet]
        [Route("GetInspectionConditions")]
        public ServiceResponse GetInspectionConditions()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
              //  string _ConditionGroup = "Common";

               // List<FO_InspectionConditions> lstInspectionConditions = bllFloodInspection.GetInspectionConditionsByGroup(_ConditionGroup);
                List<FO_InspectionConditions> lstInspectionConditions = bllFloodInspection.GetAllInspectionConditions();
                
                if (lstInspectionConditions.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<FO_InspectionConditions>(lstInspectionConditions, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetInspectionConditions", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetAllFloodInspectionTypes")]
        public ServiceResponse GetAllFloodInspectionTypes()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstInspectionType = bllFloodOperations.GetAllFloodInspectionTypes();
                if (lstInspectionType.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionType, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllFloodInspectionTypes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetAllFloodInspectionStatus")]
        public ServiceResponse GetAllFloodInspectionStatus()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstInspectionType = bllFloodOperations.GetAllFloodInspectionStatus();
                if (lstInspectionType.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionType, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllFloodInspectionStatus", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetInfrastructureDetailsByType")]
        public ServiceResponse GetInfrastructureDetailsByType(long _UserID, long _StructureTypeID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstInspectionType = bllFloodOperations.GetProtectionInfrastructureName(_UserID, _StructureTypeID);
                if (lstInspectionType.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionType, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetInfrastructureDetailsByType", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetUserFloodInspections")]
        public ServiceResponse GetUserFloodInspections(string _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                _UserID = !string.IsNullOrEmpty(_UserID) && !_UserID.Equals("null") ? _UserID.ToString() : null;
                List<object> lstInspectionType = bllFloodOperations.GetUserFloodInspections(_UserID, null);
                if (lstInspectionType.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionType, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetUserFloodInspections", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetAllRDWiseTypes")]
        public ServiceResponse GetAllRDWiseTypes()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstInspectionConditions = bllFloodOperations.GetAllActiveRDWiseTypes();
                if (lstInspectionConditions.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionConditions, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllRDWiseTypes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetAllStonePitchSide")]
        public ServiceResponse GetAllStonePitchSide()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstInspectionConditions = bllFloodOperations.GetAllActiveStonePitchSides();
                if (lstInspectionConditions.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionConditions, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllStonePitchSide", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetIRDWiseConditionsByRDType")]
        public ServiceResponse GetIRDWiseConditionsByRDType(long _FloodInspectionID, int _RDWiseTypeID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                List<object> lstInspectionConditions = null;
                if (_RDWiseTypeID == (int)Constants.RDWiseType.StonePitching)
                {
                    lstInspectionConditions = bllFloodInspection.GetRDWiseConditionForStonePitchingByID(_FloodInspectionID, _RDWiseTypeID);
                }
                else if (_RDWiseTypeID == (int)Constants.RDWiseType.ErodingAnimalHoles)
                {
                    lstInspectionConditions = bllFloodInspection.GetRDWiseConditionForStonePitchingApronByID(_FloodInspectionID, _RDWiseTypeID);
                }
                else if (_RDWiseTypeID == (int)Constants.RDWiseType.PitchStoneAppron)
                {
                    lstInspectionConditions = bllFloodInspection.GetRDWiseConditionForStonePitchingApronByID(_FloodInspectionID, _RDWiseTypeID);
                }
                else if (_RDWiseTypeID == (int)Constants.RDWiseType.RainCuts)
                {
                    lstInspectionConditions = bllFloodInspection.GetRDWiseConditionForStonePitchingApronByID(_FloodInspectionID, _RDWiseTypeID);
                }
                else if (_RDWiseTypeID == (int)Constants.RDWiseType.RDMarks)
                {
                    lstInspectionConditions = bllFloodInspection.GetRDWiseConditionForStonePitchingApronByID(_FloodInspectionID, _RDWiseTypeID);
                }
                else if (_RDWiseTypeID == (int)Constants.RDWiseType.Enchrochment)
                {
                    lstInspectionConditions = bllFloodInspection.GetEncroachmentByID(_FloodInspectionID, _RDWiseTypeID);
                }
                if (lstInspectionConditions.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionConditions, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetIRDWiseConditionsByRDType", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetBarrageGatesDetailByStationID")]
        public ServiceResponse GetBarrageGatesDetailByStationID(string _StationID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstInspectionConditions = bllFloodOperations.GetBarrageGatesDetailByStationID(_StationID);
                if (lstInspectionConditions.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionConditions, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetBarrageGatesDetailByStationID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetAllEncroachmentTypes")]
        public ServiceResponse GetAllEncroachmentTypes()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstInspectionConditions = bllFloodOperations.GetAllEncroachmentTypes();
                if (lstInspectionConditions.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionConditions, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllEncroachmentTypes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }


    //@DivisionID			BIGINT,
    //@Year					NVARCHAR(10),
    //@InspectionDate		DATETIME,
    //@InspectionTypeID		BIGINT,
    //@StructureTypeID		BIGINT,
    //@StructureID			BIGINT,
    //@floodInspectionID    BIGINT=0


        [HttpGet]
        [Route("IsFloodInspectionDataAlreadyExist")]
        public ServiceResponse IsFloodInspectionDataAlreadyExist(long _DivisioID, string _Year, string _InspectionDate, long _InspectionTypeID, long _StructureTypeID, long _StructureID, long _floodInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
               //_floodInspectionID = 0;

               //string inspectionID = _floodInspectionID.ToString();
               //     if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

               //         _FloodInspectionID = Convert.ToInt16(_FloodInspectionParams["_FloodInspectionID"].ToString());

                
                bool _isfloodexist = bllFloodOperations.IsFloodInspectionDataAlreadyExist(_DivisioID, _Year, _InspectionDate, _InspectionTypeID, _StructureTypeID, _StructureID, _floodInspectionID);
                svcResponse = srvcHlpr.CreateResponse(0, "Record Fetched Successfully", _isfloodexist);
            //    if (lstInspectionConditions.Count > 0)
            //        svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionConditions, 0, ServiceHelper.MSG_SUCCESS);
            //    else
            //        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
           }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("IsFloodInspectionDataAlreadyExist", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        /// <summary>
        /// Add Flood Inspection parameters in DB server
        /// </summary>
        /// <param name="_FloodInspectionParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddFloodInspection")]
        public ServiceResponse AddFloodInspection([FromBody] Dictionary<string, object> _FloodInspectionParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_FloodInspectionParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_FloodInspectionParams["_UserID"].ToString());
                long _DivisionID = Convert.ToInt64(_FloodInspectionParams["_DivisionID"].ToString());

                long _FloodInspectionID = 0;
                if (_FloodInspectionParams.ContainsKey("_FloodInspectionID"))
                {
                    string inspectionID = _FloodInspectionParams["_FloodInspectionID"].ToString();
                    if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

                        _FloodInspectionID = Convert.ToInt64(_FloodInspectionParams["_FloodInspectionID"].ToString());

                }

                short? _CategoryID = null;
                string catID = _FloodInspectionParams["_CategoryID"].ToString();
                if (!string.IsNullOrEmpty(catID) && !catID.Equals("null"))
                    _CategoryID = Convert.ToInt16(_FloodInspectionParams["_CategoryID"].ToString());

                long _StructureID = Convert.ToInt64(_FloodInspectionParams["_StructureID"].ToString());

                long _StructureTypeID = 0;
                if (_FloodInspectionParams.ContainsKey("_StructureTypeID"))
                {

                    string typeID = _FloodInspectionParams["_StructureTypeID"].ToString();
                    if (!string.IsNullOrEmpty(typeID) && !typeID.Equals("null"))
                        _StructureTypeID = Convert.ToInt64(_FloodInspectionParams["_StructureTypeID"].ToString());

                }
                short _InspectionTypeID = 0;
                string inspectionTypeID = _FloodInspectionParams["_InspectionTypeID"].ToString();
                if (!string.IsNullOrEmpty(inspectionTypeID) && !inspectionTypeID.Equals("null"))
                    _InspectionTypeID = Convert.ToInt16(_FloodInspectionParams["_InspectionTypeID"].ToString());


                short _StatusID = Convert.ToInt16(_FloodInspectionParams["_StatusID"].ToString());
              
                bool result = false;

                bool _isfloodexist = bllFloodOperations.IsFloodInspectionDataAlreadyExist(_DivisionID, DateTime.Now.Year.ToString(), DateTime.Now.ToString(), Convert.ToInt64(_InspectionTypeID), _StructureTypeID, _StructureID, _FloodInspectionID);

                if (!_isfloodexist)
                {
                    result = bllFloodOperations.SavaFloodInspection(_FloodInspectionID, _DivisionID, _CategoryID, _StatusID, _InspectionTypeID, _StructureID, _StructureTypeID, _UserID);
                }

                else
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, "Inspection already Exists", null);
                    return svcResponse;
                }


               
                

                if (result)
                {
                    //    List<object> lstInspectionType = bllFloodOperations.GetUserFloodInspections(_UserID.ToString(), null);
                    //     svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, lstInspectionType);

                    object inspectionObj = bllFloodOperations.GetUserFloodInspectionsObject(_UserID.ToString());
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, inspectionObj);
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
        /// <summary>
        /// Add Flood Protection Infrastructure Inspection parameters in DB server
        /// </summary>
        /// <param name="_IGCProtectionStrucParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddIGCProtectionInfrastructure")]
        public ServiceResponse AddIGCProtectionInfrastructure([FromBody] Dictionary<string, object> _IGCProtectionStrucParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_IGCProtectionStrucParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_IGCProtectionStrucParams["_UserID"].ToString());
                long _FloodInspectionID = Convert.ToInt64(_IGCProtectionStrucParams["_FloodInspectionID"].ToString());

                int _InspectionID = 0;
                if (_IGCProtectionStrucParams.ContainsKey("_InspectionID"))
                {
                    string inspectionID = _IGCProtectionStrucParams["_InspectionID"].ToString();
                    if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

                        _InspectionID = Convert.ToInt16(_IGCProtectionStrucParams["_InspectionID"].ToString());

                }
                short? _GRHutConditionID = null;
                string grHutID = _IGCProtectionStrucParams["_GRHutConditionID"].ToString();
                if (!string.IsNullOrEmpty(grHutID) && !grHutID.Equals("null"))
                    _GRHutConditionID = Convert.ToInt16(_IGCProtectionStrucParams["_GRHutConditionID"].ToString());


                short? _ServiceRoadConditionID = null;
                string serviceRoadId = _IGCProtectionStrucParams["_ServiceRoadConditionID"].ToString();
                if (!string.IsNullOrEmpty(serviceRoadId) && !serviceRoadId.Equals("null"))
                    _ServiceRoadConditionID = Convert.ToInt16(_IGCProtectionStrucParams["_ServiceRoadConditionID"].ToString());

                short? _WatchingHutConditionID = null;
                string watchingHutId = _IGCProtectionStrucParams["_WatchingHutConditionID"].ToString();
                if (!string.IsNullOrEmpty(watchingHutId) && !watchingHutId.Equals("null"))
                    _WatchingHutConditionID = Convert.ToInt16(_IGCProtectionStrucParams["_WatchingHutConditionID"].ToString());
                bool _RiverGauge = Convert.ToBoolean(_IGCProtectionStrucParams["_RiverGauge"].ToString());

                string _Remarks = _IGCProtectionStrucParams["_Remarks"].ToString();
                bool result = false;
                result = bllFloodInspection.SaveFloodIGCProtectionInfrastructure(_FloodInspectionID, _InspectionID, _GRHutConditionID, _ServiceRoadConditionID, _WatchingHutConditionID, _RiverGauge, _Remarks, _UserID);


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
        [Route("GetIGCBarrageHWGatesInformationByBarrageHWID")]
        public ServiceResponse GetIGCBarrageHWGatesInformationByBarrageHWID(long _IGCBarrageHWID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstInspectionConditions = bllFloodInspection.GetIGCBarrageHWGatesInformationByBarrageHWID(_IGCBarrageHWID);
                if (lstInspectionConditions.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstInspectionConditions, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllEncroachmentTypes", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Add Flood Barrage/Headwork Inspection parameters in DB server
        /// </summary>
        /// <param name="_IGCBarrageParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddIGCBarrageHeadwork")]
        public ServiceResponse AddIGCBarrageHeadwork([FromBody] Dictionary<string, object> _IGCBarrageParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();

            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_IGCBarrageParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_IGCBarrageParams["_UserID"].ToString());
                long _FloodInspectionID = Convert.ToInt64(_IGCBarrageParams["_FloodInspectionID"].ToString());
                object gate_info = JsonConvert.SerializeObject(_IGCBarrageParams["_GatesInfo"]);
                List<FO_IGCBarrageHWGates> GatesInfo = JsonConvert.DeserializeObject<List<FO_IGCBarrageHWGates>>(_IGCBarrageParams["_GatesInfo"].ToString()).ToList<FO_IGCBarrageHWGates>();

                int _InspectionID = 0;
                if (_IGCBarrageParams.ContainsKey("_InspectionID"))
                {
                    string inspectionID = _IGCBarrageParams["_InspectionID"].ToString();
                    if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

                        _InspectionID = Convert.ToInt16(_IGCBarrageParams["_InspectionID"].ToString());

                }
                short? _ArmyCPConditionID = null;
                string armyConditionID = _IGCBarrageParams["_ArmyCPConditionID"].ToString();
                if (!string.IsNullOrEmpty(armyConditionID) && !armyConditionID.Equals("null"))
                    _ArmyCPConditionID = Convert.ToInt16(_IGCBarrageParams["_ArmyCPConditionID"].ToString());


                short? _DataBoardConditionID = null;
                string boardConditionID = _IGCBarrageParams["_DataBoardConditionID"].ToString();
                if (!string.IsNullOrEmpty(boardConditionID) && !boardConditionID.Equals("null"))
                    _DataBoardConditionID = Convert.ToInt16(_IGCBarrageParams["_DataBoardConditionID"].ToString());

                short? _LightConditionID = null;
                string lightConditionID = _IGCBarrageParams["_LightConditionID"].ToString();
                if (!string.IsNullOrEmpty(lightConditionID) && !lightConditionID.Equals("null"))
                    _LightConditionID = Convert.ToInt16(_IGCBarrageParams["_LightConditionID"].ToString());

                short? _OperationalCameras = null;
                string optCameras = _IGCBarrageParams["_OperationalCameras"].ToString();
                if (!string.IsNullOrEmpty(optCameras) && !optCameras.Equals("null"))
                    _OperationalCameras = Convert.ToInt16(_IGCBarrageParams["_OperationalCameras"].ToString());

                short? _OperationalDeckElevated = null;
                string optDeck = _IGCBarrageParams["_OperationalDeckElevated"].ToString();
                if (!string.IsNullOrEmpty(optDeck) && !optDeck.Equals("null"))
                    _OperationalDeckElevated = Convert.ToInt16(_IGCBarrageParams["_OperationalDeckElevated"].ToString());


                short? _TollHutConditionID = null;
                string tollConditionID = _IGCBarrageParams["_TollHutConditionID"].ToString();
                if (!string.IsNullOrEmpty(tollConditionID) && !tollConditionID.Equals("null"))
                    _TollHutConditionID = Convert.ToInt16(_IGCBarrageParams["_TollHutConditionID"].ToString());

                short? _TotalCameras = null;
                string totalCon = _IGCBarrageParams["_TotalCameras"].ToString();
                if (!string.IsNullOrEmpty(optDeck) && !optDeck.Equals("null"))
                    _TotalCameras = Convert.ToInt16(_IGCBarrageParams["_TotalCameras"].ToString());

bool? _PoliceMonitory = null;

if (_IGCBarrageParams.ContainsKey("_PoliceMonitory"))
                {


                    _PoliceMonitory = Convert.ToBoolean(_IGCBarrageParams["_PoliceMonitory"].ToString());

                }
                

              //  bool _PoliceMonitory = Convert.ToBoolean(_IGCBarrageParams["_PoliceMonitory"].ToString());



                string _CCTVIncharge = _IGCBarrageParams["_CCTVIncharge"].ToString();
                string _CCTVInchargePhone = _IGCBarrageParams["_CCTVInchargePhone"].ToString();
                string _Remarks = _IGCBarrageParams["_Remarks"].ToString();

                bool result = false;
                long InspectionID = bllFloodInspection.SaveFloodIGCBarrage(_FloodInspectionID, _InspectionID, _ArmyCPConditionID, _CCTVIncharge, _CCTVInchargePhone, _DataBoardConditionID, _LightConditionID, _OperationalCameras, _OperationalDeckElevated, _PoliceMonitory, _TollHutConditionID, _TotalCameras, _Remarks, _UserID);
                if (InspectionID > 0)
                {
                    foreach (var ls in GatesInfo)
                    {

                        if (ls.ID != 0)
                        {
                            object gatesInfo = bllFloodInspection.GetIGCBarrageHWGatesInfoByBarrageHWID(ls.ID);

                            ls.CreatedBy = Convert.ToInt32(Convert.ToString(gatesInfo.GetType().GetProperty("CreatedBy").GetValue(gatesInfo)));
                            ls.CreatedDate = Convert.ToDateTime(Convert.ToString(gatesInfo.GetType().GetProperty("CreatedDate").GetValue(gatesInfo)));
                            ls.ModifiedBy = Convert.ToInt32(_UserID);
                            ls.ModifiedDate = DateTime.Now;

                        }
                        else
                        {
                            ls.CreatedBy = Convert.ToInt32(_UserID);
                            ls.CreatedDate = DateTime.Now;
                        }
                        ls.IGCBarrageHWID = InspectionID;

                    }

                    result = bllFloodInspection.SaveIGCBarrageHWGatesInformation(GatesInfo);
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
        /// Add Flood Drain Inspection parameters in DB server
        /// </summary>
        /// <param name="_IGCDrainParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddIGCDrainInfrastructure")]
        public ServiceResponse AddIGCDrainInfrastructure([FromBody] Dictionary<string, object> _IGCDrainParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_IGCDrainParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_IGCDrainParams["_UserID"].ToString());
                long _FloodInspectionID = Convert.ToInt64(_IGCDrainParams["_FloodInspectionID"].ToString());

                int _InspectionID = 0;
                if (_IGCDrainParams.ContainsKey("_InspectionID"))
                {
                    string inspectionID = _IGCDrainParams["_InspectionID"].ToString();
                    if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

                        _InspectionID = Convert.ToInt16(_IGCDrainParams["_InspectionID"].ToString());

                }
                double? _ExistingCapacity = null;
                string exstCap = _IGCDrainParams["_ExistingCapacity"].ToString();
                if (!string.IsNullOrEmpty(exstCap) && !exstCap.Equals("null"))
                    _ExistingCapacity = Convert.ToInt16(_IGCDrainParams["_ExistingCapacity"].ToString());

                double? _ImprovedCapacity = null;
                string impCapacity = _IGCDrainParams["_ImprovedCapacity"].ToString();
                if (!string.IsNullOrEmpty(impCapacity) && !impCapacity.Equals("null"))
                    _ImprovedCapacity = Convert.ToInt16(_IGCDrainParams["_ImprovedCapacity"].ToString());

                bool _DrainWaterET = Convert.ToBoolean(_IGCDrainParams["_DrainWaterET"].ToString());

                short? _CurrentLevel = null;
                string currLevel = _IGCDrainParams["_CurrentLevel"].ToString();
                if (!string.IsNullOrEmpty(currLevel) && !currLevel.Equals("null"))
                    _CurrentLevel = Convert.ToInt16(_IGCDrainParams["_CurrentLevel"].ToString());


                double? _OutfallBedWidth = null;
                string bedWidth = _IGCDrainParams["_OutfallBedWidth"].ToString();
                if (!string.IsNullOrEmpty(bedWidth) && !bedWidth.Equals("null"))
                    _OutfallBedWidth = Convert.ToInt16(_IGCDrainParams["_OutfallBedWidth"].ToString());

                double? _OutfallSupplyWidth = null;
                string supplyWidth = _IGCDrainParams["_OutfallSupplyWidth"].ToString();
                if (!string.IsNullOrEmpty(supplyWidth) && !supplyWidth.Equals("null"))
                    _OutfallSupplyWidth = Convert.ToInt16(_IGCDrainParams["_OutfallSupplyWidth"].ToString());

                short? _BridgeGovtNo = null;
                string govtNo = _IGCDrainParams["_BridgeGovtNo"].ToString();
                if (!string.IsNullOrEmpty(govtNo) && !govtNo.Equals("null"))
                    _BridgeGovtNo = Convert.ToInt16(_IGCDrainParams["_BridgeGovtNo"].ToString());

                short? _BridgePvtNo = null;
                string pvtNo = _IGCDrainParams["_BridgePvtNo"].ToString();
                if (!string.IsNullOrEmpty(pvtNo) && !pvtNo.Equals("null"))
                    _BridgePvtNo = Convert.ToInt16(_IGCDrainParams["_BridgePvtNo"].ToString());


                string _Remarks = _IGCDrainParams["_Remarks"].ToString();
                bool result = false;
                result = bllFloodInspection.SaveFloodIGCDrain(_FloodInspectionID, _InspectionID, _ExistingCapacity, _ImprovedCapacity, _CurrentLevel, _DrainWaterET, _OutfallBedWidth, _OutfallSupplyWidth, _BridgeGovtNo, _BridgePvtNo, _Remarks, _UserID);


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
        /// Add Flood RDWise Inspection parameters in DB server
        /// </summary>
        /// <param name="_IGCProtectionStrucParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddIRDWiseConditions")]
        public ServiceResponse AddIRDWiseConditions([FromBody] Dictionary<string, object> _IGCProtectionStrucParams)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_IGCProtectionStrucParams))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_IGCProtectionStrucParams["_UserID"].ToString());
                long _FloodInspectionID = Convert.ToInt64(_IGCProtectionStrucParams["_FloodInspectionID"].ToString());

                int _InspectionID = 0;
                if (_IGCProtectionStrucParams.ContainsKey("_InspectionID"))
                {
                    string inspectionID = _IGCProtectionStrucParams["_InspectionID"].ToString();
                    if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

                        _InspectionID = Convert.ToInt16(_IGCProtectionStrucParams["_InspectionID"].ToString());

                }

                int _FromRD = Convert.ToInt32(_IGCProtectionStrucParams["_FromRD"].ToString());
                int _ToRD = Convert.ToInt32(_IGCProtectionStrucParams["_ToRD"].ToString());

                short _ConditionID = Convert.ToInt16(_IGCProtectionStrucParams["_ConditionID"].ToString());
                short _RDWiseTypeID = Convert.ToInt16(_IGCProtectionStrucParams["_RDWiseTypeID"].ToString());

                short? _StonePitchSideID = null;
                string sideID = _IGCProtectionStrucParams["_StonePitchSideID"].ToString();
                if (!string.IsNullOrEmpty(sideID) && !sideID.Equals("null"))
                    _StonePitchSideID = Convert.ToInt16(_IGCProtectionStrucParams["_StonePitchSideID"].ToString());


                short? _EncroachmentTypeID = null;
                string encroachmentID = _IGCProtectionStrucParams["_EncroachmentTypeID"].ToString();
                if (!string.IsNullOrEmpty(encroachmentID) && !encroachmentID.Equals("null"))
                    _EncroachmentTypeID = Convert.ToInt16(_IGCProtectionStrucParams["_EncroachmentTypeID"].ToString());



                string _Remarks = _IGCProtectionStrucParams["_Remarks"].ToString();
                long _InspectionId = bllFloodInspection.SaveIRDWiseConditions(_FloodInspectionID, _InspectionID, _FromRD, _ToRD, _RDWiseTypeID, _ConditionID, _StonePitchSideID, _EncroachmentTypeID, _Remarks, _UserID);

                if (_InspectionId > 0)
                {
                    object InspectionConditions = bllFloodInspection.GetRDWiseConditionForByID(_InspectionId, _RDWiseTypeID);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, InspectionConditions);
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
        [Route("GetAllItemCategories")]
        public ServiceResponse GetAllItemCategories()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> ItemCategoryList = new BLL.FloodOperations.ReferenceData.ItemsBLL().GetAllActiveItemCategoryList();
                if (ItemCategoryList.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(ItemCategoryList, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllItemCategories", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetMeasuringBookStatusPreFlood")]
        public ServiceResponse GetMeasuringBookStatusPreFlood(long _DivisionID, int _Year, int _ItemCategoryID, long _StructureTypeID, long _StructureID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                //             @pDivisionID			BIGINT	= Null
                //,@pYear					INT		= Null
                //,@pItemCategoryID		SMALLINT= Null
                //,@pStructureTypeID		Bigint	= Null
                //,@pStructureID			Bigint	= NULL

                List<object> lstMBStatus = bllFloodInspection.GetMBStatusPreItemList(_DivisionID, _Year, _ItemCategoryID, _StructureTypeID, _StructureID);
                if (lstMBStatus.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstMBStatus, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetMeasuringBookStatusPreFlood", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetMeasuringBookStatusPostFlood")]
        public ServiceResponse GetMeasuringBookStatusPostFlood(long _DivisionID, int _Year, int _ItemCategoryID, long _StructureTypeID, long _StructureID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstMBStatus = bllFloodInspection.GetMBStatusPostItemList(_DivisionID, _Year, _ItemCategoryID, _StructureTypeID, _StructureID);
                if (lstMBStatus.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstMBStatus, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetMeasuringBookStatusPostFlood", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetAllProblemNature")]
        public ServiceResponse GetAllProblemNature()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstMBStatus = bllFloodInspection.GetAllProblemNature();
                if (lstMBStatus.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstMBStatus, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllProblemNature", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetProblemsListByFloodInspectionID")]
        public ServiceResponse GetProblemsListByFloodInspectionID(int _floodInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstProblemForFI = bllFloodInspection.GetProblemForFIByFloodInspectionID(_floodInspectionID);
                if (lstProblemForFI.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstProblemForFI, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetProblemsListByFloodInspectionID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Add Flood Problem parameters in DB server
        /// </summary>
        /// <param name="_IGCProtectionStrucParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddFOProblem")]
        public ServiceResponse AddFOProblem([FromBody] Dictionary<string, object> _IProblem)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_IProblem))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_IProblem["_UserID"].ToString());
                long _FloodInspectionID = Convert.ToInt64(_IProblem["_FloodInspectionID"].ToString());

                int _InspectionID = 0;
                if (_IProblem.ContainsKey("_InspectionID"))
                {
                    string inspectionID = _IProblem["_InspectionID"].ToString();
                    if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

                        _InspectionID = Convert.ToInt16(_IProblem["_InspectionID"].ToString());

                }

                int? _FromRD = null;
                string fromrd = _IProblem["_FromRD"].ToString();
                if (!string.IsNullOrEmpty(fromrd) && !fromrd.Equals("null"))
                    _FromRD = Convert.ToInt32(_IProblem["_FromRD"].ToString());


                int? _ToRD = null;
                string tord = _IProblem["_ToRD"].ToString();
                if (!string.IsNullOrEmpty(tord) && !tord.Equals("null"))
                    _ToRD = Convert.ToInt32(_IProblem["_ToRD"].ToString());

                // int _FromRD = Convert.ToInt32(_IProblem["_FromRD"].ToString());
                //int  = Convert.ToInt32(_IProblem["_ToRD"].ToString());

                short _ProblemNatureID = Convert.ToInt16(_IProblem["_ProblemNatureID"].ToString());
                string _Solution = _IProblem["_Solution"].ToString();

                long? _RestorationCost = null;
                string cost = _IProblem["_RestorationCost"].ToString();
                if (!string.IsNullOrEmpty(cost) && !cost.Equals("null"))
                    _RestorationCost = Convert.ToInt64(_IProblem["_RestorationCost"].ToString());




                long _ProblemId = bllFloodInspection.SaveProblemForFI(_FloodInspectionID, _InspectionID, _FromRD, _ToRD, _ProblemNatureID, _Solution, _RestorationCost, _UserID);

                if (_ProblemId > 0)
                {
                    object foProblem = bllFloodInspection.GetProblemForFIByInspectionID(_ProblemId);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, foProblem);

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
        /// Add Flood Problem parameters in DB server
        /// </summary>
        /// <param name="_IGCProtectionStrucParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddFOBreachingSection")]
        public ServiceResponse AddFOBreachingSection([FromBody] Dictionary<string, object> _IBreachSection)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_IBreachSection))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_IBreachSection["_UserID"].ToString());
                long _FloodInspectionID = Convert.ToInt64(_IBreachSection["_FloodInspectionID"].ToString());

                int _InspectionID = 0;
                if (_IBreachSection.ContainsKey("_InspectionID"))
                {
                    string inspectionID = _IBreachSection["_InspectionID"].ToString();
                    if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

                        _InspectionID = Convert.ToInt16(_IBreachSection["_InspectionID"].ToString());

                }
                int _FromRD = Convert.ToInt32(_IBreachSection["_FromRD"].ToString());
                int _ToRD = Convert.ToInt32(_IBreachSection["_ToRD"].ToString());

                string _Solution = null;
                string sol = _IBreachSection["_Solution"].ToString();
                if (!string.IsNullOrEmpty(sol) && !sol.Equals("null"))
                    _Solution = _IBreachSection["_Solution"].ToString();

                long? _RestorationCost = null; 
                if (_IBreachSection.ContainsKey("_RestorationCost"))
                {
                    string cost = _IBreachSection["_RestorationCost"].ToString();
                    if (!string.IsNullOrEmpty(cost) && !cost.Equals("-1"))
                        _RestorationCost = Convert.ToInt64(_IBreachSection["_RestorationCost"].ToString());
                }
              
                short? _AffectedRowsNo = null;
                string rowsNo = _IBreachSection["_AffectedRowsNo"].ToString();
                if (!string.IsNullOrEmpty(rowsNo) && !rowsNo.Equals("null"))
                    _AffectedRowsNo = Convert.ToInt16(_IBreachSection["_AffectedRowsNo"].ToString());

                short? _AffectedLinersNo = null;
                string linerNo = _IBreachSection["_AffectedLinersNo"].ToString();
                if (!string.IsNullOrEmpty(linerNo) && !linerNo.Equals("null"))
                    _AffectedLinersNo = Convert.ToInt16(_IBreachSection["_AffectedLinersNo"].ToString());


                bool result = bllFloodInspection.SaveBreachSectionForFI(_FloodInspectionID, _InspectionID, _FromRD, _ToRD, _AffectedRowsNo, _AffectedLinersNo, _Solution, _RestorationCost, _UserID);

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
        /// Add Flood Problem parameters in DB server
        /// </summary>
        /// <param name="_IGCProtectionStrucParams"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("AddIStonePosition")]
        //public ServiceResponse AddIStonePosition([FromBody] Dictionary<string, object> _IBreachSection)
        //{
        //    ServiceResponse svcResponse = new ServiceResponse();
        //    try
        //    {

        //        if (srvcHlpr.IsDictionaryEmptyOrNull(_IBreachSection))
        //            return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

        //        int _UserID = Convert.ToInt32(_IBreachSection["_UserID"].ToString());
        //        long _FloodInspectionID = Convert.ToInt64(_IBreachSection["_FloodInspectionID"].ToString());
        //        int _InspectionID = 0;
        //        if (_IBreachSection.ContainsKey("_InspectionID"))
        //        {
        //            string inspectionID = _IBreachSection["_InspectionID"].ToString();
        //            if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

        //                _InspectionID = Convert.ToInt16(_IBreachSection["_InspectionID"].ToString());

        //        }
        //        int _RD = Convert.ToInt32(_IBreachSection["_RD"].ToString());

        //        int _BeforeFloodQty = Convert.ToInt16(_IBreachSection["_BeforeFloodQty"].ToString());

        //        int? _AvailableQty = null;
        //        string avlblQty = _IBreachSection["_AvailableQty"].ToString();
        //        if (!string.IsNullOrEmpty(avlblQty) && !avlblQty.Equals("null"))
        //            _AvailableQty = Convert.ToInt16(_IBreachSection["_AvailableQty"].ToString());


        //        bool result = bllFloodInspection.SaveIStonePosition(_FloodInspectionID, _InspectionID, _RD, _BeforeFloodQty, _AvailableQty, _UserID);

        //        if (result)
        //            svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
        //        else
        //            svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
        //    }
        //    catch (Exception exp)
        //    {
        //        //TODO: Log exception
        //        svcResponse = srvcHlpr.CreateResponse(1004, MSG_UNKNOWN_ERROR, null);
        //        string excetionMsg = exp.Message;
        //    }
        //    return svcResponse;
        //}
        [HttpGet]
        [Route("GetStonePositionListByFloodInspectionID")]
        public ServiceResponse GetStonePositionListByFloodInspectionID(int _floodInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstStonePostion = bllFloodInspection.GetIStonePositionByFloodInspectionID(_floodInspectionID);
                if (lstStonePostion.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstStonePostion, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetStonePositionListByFloodInspectionID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("GetBreachingSectionListByFloodInspectionID")]
        public ServiceResponse GetBreachingSectionListByFloodInspectionID(int _floodInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> oIBreachingSection = bllFloodInspection.GetIBreachingSectionByInspectionID(_floodInspectionID);
                if (oIBreachingSection.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(oIBreachingSection, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetStonePositionListByFloodInspectionID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("GetIGConditionsByFloodInspectionID")]
        public ServiceResponse GetIGConditionsByFloodInspectionID(string _structureType, int _floodInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                object objIGC = bllFloodInspection.GetIGConditionsByInspectionID(_structureType, _floodInspectionID);

                if (objIGC != null)
                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, objIGC);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetIGConditionsByFloodInspectionID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("DeleteFloodInspection")]
        public ServiceResponse DeleteFloodInspection(int _floodInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (bllFloodInspection.IsFloodInspectionDependencyExists(Convert.ToInt64(_floodInspectionID)))
                {
                    return srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);

                }

                // bool IsDeleted = bllFloodOperations.DeleteFloodInspection(_floodInspectionID);

                //if (IsDeleted)

                //    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_DELETED, null);

                //else
                //    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("DeleteFloodInspection", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("PublishFloodInspection")]
        public ServiceResponse PublishFloodInspection(int _floodInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                bool IsPublished = bllFloodInspection.CheckInspectionStatusByID(_floodInspectionID);

                if (IsPublished)

                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, null);

                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("PublishFloodInspection", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("DeleteRDWiseInspection")]
        public ServiceResponse DeleteRDWiseInspection(int _RDWiseInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                bool IsDeleted = bllFloodInspection.DeleteRDWiseCondition(_RDWiseInspectionID);

                if (IsDeleted)

                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_DELETED, null);

                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("DeleteRDWiseInspection", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("DeleteProblemInspection")]
        public ServiceResponse DeleteProblemInspection(int _ProblemInspectionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                bool IsDeleted = bllFloodInspection.DeleteProblemFI(_ProblemInspectionID);

                if (IsDeleted)

                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_DELETED, null);

                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("DeleteProblemInspection", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        /// <summary>
        /// Add Flood MBStatus parameters in DB server
        /// </summary>
        /// <param name="_PreMBStatus"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePreFloodMBStatus")]
        public ServiceResponse UpdatePreFloodMBStatus([FromBody] Dictionary<string, object> _PreMBStatus)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {


                //@PreMBStatusID       BIGINT=0,
                //@FloodInspectionID   BIGINT=NULL,
                //@ItemID				BIGINT=NULL,
                //@LastYrQty			INT=NULL,
                //@DivStrIssuedQty		INT=NULL,
                //@AvailableQty		INT=NULL,
                //@CreatedBy			INT=NULL,
                //@ModifiedBy			INT=NULL,
                //@pODIIDOut			Bigint Output



                if (srvcHlpr.IsDictionaryEmptyOrNull(_PreMBStatus))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_PreMBStatus["_UserID"].ToString());
                long _FloodInspectionsID = Convert.ToInt64(_PreMBStatus["_FloodInspectionID"].ToString());
                long _PreMBStatusID = 0;
                if (_PreMBStatus.ContainsKey("_PreMBStatusID"))
                {
                    string purchaseID = _PreMBStatus["_PreMBStatusID"].ToString();
                    if (!string.IsNullOrEmpty(purchaseID) && !purchaseID.Equals("null"))

                        _PreMBStatusID = Convert.ToInt16(_PreMBStatus["_PreMBStatusID"].ToString());

                }
                long _ItemID = Convert.ToInt64(_PreMBStatus["_ItemID"].ToString());
                int _LastYrQty = Convert.ToInt32(_PreMBStatus["_LastYrQty"].ToString());
                int _DivStrIssuedQty = Convert.ToInt32(_PreMBStatus["_DivStrIssuedQty"].ToString());
                int _AvailableQty = Convert.ToInt32(_PreMBStatus["_AvailableQty"].ToString());


                long _StatusId = bllFloodInspection.SavePreFloodMBStatus(_PreMBStatusID, _FloodInspectionsID, _ItemID, _LastYrQty, _DivStrIssuedQty, _AvailableQty, _UserID);

                if (_StatusId > 0)
                {

                    object _MBStausObj = bllFloodInspection.GET_PreMBStatus_Object(_StatusId);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _MBStausObj);


                    //object _MBStausObj = bllFloodInspection.GetMBStatusByID(_StatusId, 1);
                    //svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _MBStausObj);
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
        /// <summary>
        /// Add Flood MBStatus parameters in DB server
        /// </summary>
        /// <param name="_PreMBStatus"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePostFloodMBStatus")]
        public ServiceResponse UpdatePostFloodMBStatus([FromBody] Dictionary<string, object> _PostMBStatus)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                // @pOverallDivItemID			Bigint=0
                //,@pYear						Int
                //,@pDivisionID				Bigint
                //,@pItemCategoryID			Bigint=Null
                //,@pItemSubcategoryID		Bigint=Null
                //,@pStructureTypeID			Bigint=Null
                //,@pStructureID				Bigint=Null
                //,@pPreMBStatusID			Bigint=Null
                //,@pFloodInspectionDetailID	Bigint=Null	
                //,@pPostAvailableQty			Int=Null
                //,@pPostRequiredQty			Int=Null
                //,@pCS_CampSiteID			Bigint=Null
                //,@pCS_RequiredQty			Int=Null
                //,@pOD_AdditionalQty			Int=Null			
                //,@pCreatedBy				Int
                //,@pModifiedBy				Int
                //,@pODIIDOut					Bigint Output

                if (srvcHlpr.IsDictionaryEmptyOrNull(_PostMBStatus))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                //t _UserID = Convert.ToInt32(_PostMBStatus["_UserID"].ToString());
                //long _pOverallDivItemID	 = Convert.ToInt64(_PostMBStatus["_pOverallDivItemID"].ToString());




                long _OverallDivItemID = 0;
                if (_PostMBStatus.ContainsKey("_OverallDivItemID"))
                {
                    string overalldivitemid = _PostMBStatus["_OverallDivItemID"].ToString();
                    if (!string.IsNullOrEmpty(overalldivitemid) && !overalldivitemid.Equals("null"))

                        _OverallDivItemID = Convert.ToInt16(_PostMBStatus["_OverallDivItemID"].ToString());

                }

                int _Year = Convert.ToInt32(_PostMBStatus["_Year"].ToString());
                long _DivisionID = Convert.ToInt64(_PostMBStatus["_DivisionID"].ToString());
                long? _ItemCategoryID = Convert.ToInt64(_PostMBStatus["_ItemCategoryID"].ToString());
                long? _ItemSubcategoryID = Convert.ToInt64(_PostMBStatus["_ItemSubcategoryID"].ToString());
                long? _StructureTypeID = Convert.ToInt64(_PostMBStatus["_StructureTypeID"].ToString());
                long? _StructureID = Convert.ToInt64(_PostMBStatus["_StructureID"].ToString());
                //  long? _PreMBStatusID = Convert.ToInt64(_PostMBStatus["_PreMBStatusID"].ToString());
                long? _PreMBStatusID = null;
                long _FloodInspectionID = Convert.ToInt64(_PostMBStatus["_FloodInspectionID"].ToString());
                int? _PostAvailableQty = Convert.ToInt32(_PostMBStatus["_PostAvailableQty"].ToString());
                int? _PostRequiredQty = Convert.ToInt32(_PostMBStatus["_PostRequiredQty"].ToString());
                //    long? _CS_CampSiteID = Convert.ToInt64(_PostMBStatus["_CS_CampSiteID"].ToString());
                //     int? _CS_RequiredQty = Convert.ToInt32(_PostMBStatus["_CS_RequiredQty"].ToString());
                //    int? _OD_AdditionalQty = Convert.ToInt32(_PostMBStatus["_OD_AdditionalQty"].ToString());
                // long _pODIIDOut = Convert.ToInt64(_PostMBStatus["_pODIIDOut"].ToString());

                int _CreatedBy = Convert.ToInt32(_PostMBStatus["_CreatedBy"].ToString());
                int _ModifiedBy = Convert.ToInt32(_PostMBStatus["_ModifiedBy"].ToString());

                long _ODIID = 0;

                long? _FLoodInspectionDetailID = 0;

                try
                {

                    object PreMBStatusID = bllFloodInspection.GetFIDetailIDbyFIID(_FloodInspectionID);

                    if (PreMBStatusID != null)
                    {

                        _FLoodInspectionDetailID = Convert.ToInt64(PreMBStatusID.GetType().GetProperty("ID").GetValue(PreMBStatusID));


                    }
                }
                catch (Exception exp)
                {

                }



                //     long _StatusId = bllFloodInspection.SavePostFloodMBStatus(_OverallDivItemID, _Year, _DivisionID, _ItemCategoryID, _ItemSubcategoryID, _StructureTypeID, _StructureID, _PreMBStatusID, _FLoodInspectionDetailID, _PostAvailableQty, _PostRequiredQty, _CS_CampSiteID, _CS_RequiredQty, _OD_AdditionalQty, _CreatedBy, _ModifiedBy, _ODIID);

                long Id = bllFloodInspection.SavePostFloodMBStatus(_OverallDivItemID, _Year, _DivisionID, _ItemCategoryID, _ItemSubcategoryID, _StructureTypeID, _StructureID, _PreMBStatusID, _FLoodInspectionDetailID, _PostAvailableQty, _PostRequiredQty, null, null, null, _CreatedBy, _ModifiedBy, _ODIID);

                //           long _StatusId = bllFloodInspection.SavePostFloodMBStatus(_OverallDivItemID, _Year, _DivisionID, null, null, _StructureTypeID, _StructureID, null, null, null, null, null, null, null, _CreatedBy, _ModifiedBy, _ODIID);
                //         long _StatusId = 0;
                if (Id > 0)
                {

                    object _MBStausObj = bllFloodInspection.GET_PostMBStatus_Object(Id);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _MBStausObj);
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
        [Route("GetPreMBStatusID")]
        public ServiceResponse GetPreMBStatusID(int _Year, long _DivisionID, long _StructureTypeID, long _StructureID, long _ItemID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {


                long PreMBStatusID;

                PreMBStatusID = bllFloodInspection.GetFOPreMbStatusID(_Year, _DivisionID, _StructureTypeID, _StructureID, _ItemID);
                svcResponse = srvcHlpr.CreateResponse(0, MSG_SUCCESS, PreMBStatusID);
                //if (PreMBStatusID >  0)
                //    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, PreMBStatusID);
                //else
                //    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetPreMBStatusID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }





        /// <summary>
        /// Add Flood Problem parameters in DB server
        /// </summary>
        /// <param name="_IGCProtectionStrucParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddEmergencyPurchases")]
        public ServiceResponse AddEmergencyPurchases([FromBody] Dictionary<string, object> _EmergencyPurchase)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_EmergencyPurchase))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_EmergencyPurchase["_UserID"].ToString());
                int _PurchaseID = 0;
                if (_EmergencyPurchase.ContainsKey("_PurchaseID"))
                {
                    string purchaseID = _EmergencyPurchase["_PurchaseID"].ToString();
                    if (!string.IsNullOrEmpty(purchaseID) && !purchaseID.Equals("null"))

                        _PurchaseID = Convert.ToInt16(_EmergencyPurchase["_PurchaseID"].ToString());

                }
                long _DivisionID = Convert.ToInt64(_EmergencyPurchase["_DivisionID"].ToString());
                long _StructureTypeID = Convert.ToInt64(_EmergencyPurchase["_StructureTypeID"].ToString());
                long _StructureID = Convert.ToInt64(_EmergencyPurchase["_StructureID"].ToString());
                bool IsCampSite = Convert.ToBoolean(_EmergencyPurchase["IsCampSite"].ToString());
                string _RDLeft = _EmergencyPurchase["_RDLeft"].ToString();
                string _RDRight = _EmergencyPurchase["_RDRight"].ToString();

                // int _BeforeFloodQty = Convert.ToInt32(_EmergencyPurchase["_BeforeFloodQty"].ToString());

                //int? _AvailableQty = null;
                //string avlblQty = _EmergencyPurchase["_AvailableQty"].ToString();
                //if (!string.IsNullOrEmpty(avlblQty) && !avlblQty.Equals("null"))
                //    _AvailableQty = Convert.ToInt16(_EmergencyPurchase["_AvailableQty"].ToString());

                long ID = bllFloodOperations.SaveEmergencyPurchase(_PurchaseID, _DivisionID, _StructureTypeID, _StructureID, IsCampSite, _RDLeft, _RDRight, _UserID);

                if (ID > 0)
                {

                    object _EPObj = bllFloodOperations.SearchEmergencyPurchaseByID(_StructureTypeID, ID);

                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _EPObj);
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

        [HttpGet]
        [Route("DeleteEmergencyPurchaseByID")]
        public ServiceResponse DeleteEmergencyPurchaseByID(long _PurchaseID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                bool IsDeleted = bllFloodOperations.DeleteEmergencyPurchase(_PurchaseID);

                if (IsDeleted)

                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_DELETED, null);

                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("DeleteEmergencyPurchaseByID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }



        [HttpGet]
        [Route("SearchEmergencyPurchase")]
        public ServiceResponse SearchEmergencyPurchase(long? _ZoneID, int _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {



                List<object> lstsearchemergencypurchase = null;

                lstsearchemergencypurchase = bllFloodOperations.SearchEmergencyPurchase(null, null, _ZoneID, null, null, null, null, _UserID);

                if (lstsearchemergencypurchase.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstsearchemergencypurchase, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("SearchEmergencyPurchase", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }




        [HttpGet]
        [Route("SearchStoneDeployment")]
        public ServiceResponse SearchStoneDeployment(long? _ZoneID,int _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {



                List<object> lstsearchstonedeploment = null;

                lstsearchstonedeploment = bllFloodFightingPlan.SearchStoneDeployment(null, null, null, null, _ZoneID, null, null, null, _UserID);

                if (lstsearchstonedeploment.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstsearchstonedeploment, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("SearchStoneDeployment", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }



        [HttpGet]
        [Route("GetWorksByEmergencyPurchaseID")]
        public ServiceResponse GetWorksByEmergencyPurchaseID(int _EmergencyID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstworksbyemergencypurchaseid = null;

                lstworksbyemergencypurchaseid = bllFloodOperations.GetFo_EPWorkBy_ID(_EmergencyID);

                if (lstworksbyemergencypurchaseid.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstworksbyemergencypurchaseid, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetWorksByEmergencyPurchaseID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }


        [HttpGet]
        [Route("GetDisposalDetailByWorksID")]
        public ServiceResponse GetDisposalDetailByWorksID(long _WorksID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstdisposaldetailbyworkid = null;

                lstdisposaldetailbyworkid = bllFloodOperations.GetF_EmergencyDisposalByID(_WorksID);

                if (lstdisposaldetailbyworkid.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstdisposaldetailbyworkid, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetDisposalDetailByWorksID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }



        [HttpGet]
        [Route("GetAttachmentDisposalDetailByID")]
        public ServiceResponse GetAttachmentDisposalDetailByID(long _ID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstattachmentdisposaldetail = null;

                lstattachmentdisposaldetail = bllFloodOperations.GetF_EmergencyDisposal_Attachment_ID(_ID);

                if (lstattachmentdisposaldetail.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstattachmentdisposaldetail, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAttachmentDisposalDetailByID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }


        [HttpGet]
        [Route("GetAllItemCategoryList")]
        public ServiceResponse GetAllItemCategoryList()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstfoitemscategory = null;

                lstfoitemscategory = bllItems.GetAllActiveItemCategoryList();

                if (lstfoitemscategory.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstfoitemscategory, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAllItemCategoryList", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }



        [HttpGet]
        [Route("GetItemsPurchaseListbyItemCatID")]
        public ServiceResponse GetItemsPurchaseListbyItemCatID()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstfoitemscategory = null;

                lstfoitemscategory = bllItems.GetAllItemsList();
                if (lstfoitemscategory.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstfoitemscategory, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetItemsPurchaseListbyItemCatID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpPost]
        [Route("AddWorks")]
        public ServiceResponse AddWorks([FromBody] Dictionary<string, object> _Works)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_Works))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                long _UserID = Convert.ToInt64(_Works["_UserID"].ToString());

                long _WorkID = 0;
                if (_Works.ContainsKey("_EPWorkID"))
                {
                    string workID = _Works["_EPWorkID"].ToString();
                    if (!string.IsNullOrEmpty(workID) && !workID.Equals("null"))

                        _WorkID = Convert.ToInt64(_Works["_EPWorkID"].ToString());

                }


                long _EmergencyID = Convert.ToInt64(_Works["_EmergencyID"].ToString());

                long? _NatureOfWorkID = null;
                string natureofworkid = _Works["_NatureOfWorkID"].ToString();
                if (!string.IsNullOrEmpty(natureofworkid) && !natureofworkid.Equals("null"))
                    _NatureOfWorkID = Convert.ToInt32(_Works["_NatureOfWorkID"].ToString());




                int? _RD = null;
                string rd = _Works["_RD"].ToString();
                if (!string.IsNullOrEmpty(natureofworkid) && !natureofworkid.Equals("null"))
                    _RD = Convert.ToInt32(_Works["_RD"].ToString());



                string _Descp = null;
                string descp = _Works["_Descp"].ToString();
                if (!string.IsNullOrEmpty(natureofworkid) && !natureofworkid.Equals("null"))
                    _Descp = _Works["_Descp"].ToString();



                long ID = bllFloodOperations.SaveFloodFightingWorks(_UserID, _WorkID, _EmergencyID, _NatureOfWorkID, _RD, _Descp);

                if (ID > 0)
                {

                    object _workObj = bllFloodOperations.GetFo_EPWorkByObject_ID(ID);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _workObj);


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

        [HttpPost]
        [Route("AddDisposalDetail")]
        public ServiceResponse AddDisposalDetail([FromBody] Dictionary<string, object> _DisposalDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_DisposalDetail))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _EPWorkID = Convert.ToInt32(_DisposalDetail["_EPWorkID"].ToString());
                int _UserID = Convert.ToInt32(_DisposalDetail["_UserID"].ToString());


                int _DisposalID = 0;
                if (_DisposalDetail.ContainsKey("_DisposalID"))
                {
                    string disposalID = _DisposalDetail["_DisposalID"].ToString();
                    if (!string.IsNullOrEmpty(disposalID) && !disposalID.Equals("null"))

                        _DisposalID = Convert.ToInt16(_DisposalDetail["_DisposalID"].ToString());

                }

                string _VehicleNumber = _DisposalDetail["_VehicleNumber"].ToString();

                string _BuiltyNumber = (_DisposalDetail["_BuiltyNumber"].ToString());



                int? _Quantity = null;
                string quantity = _DisposalDetail["_Quantity"].ToString();
                if (!string.IsNullOrEmpty(quantity) && !quantity.Equals("null"))
                    _Quantity = Convert.ToInt32(_DisposalDetail["_Quantity"].ToString());


                string _Unit = _DisposalDetail["_Unit"].ToString();



                long? _Cost = null;
                string cost = _DisposalDetail["_Cost"].ToString();
                if (!string.IsNullOrEmpty(cost) && !cost.Equals("null"))
                    _Cost = Convert.ToInt64(_DisposalDetail["_Cost"].ToString());



                string attachments = null;
                string atchmnts = _DisposalDetail["_Attachments"].ToString();
                if (!string.IsNullOrEmpty(atchmnts) && !atchmnts.Equals("null"))
                    attachments = atchmnts;



                //link attachments to ClosureWorkProgress if there any
                string dateFormat = "yyyyMMdd-hhmmss";
                string _date = DateTime.Now.ToString(dateFormat);
                List<string> lstNameofFiles = new List<string>();
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
                                string _NewImageName = guid + "_" + "MaterialDisposal" + "-" + _date;
                                string filePath = Utility.GetImagePath(Common.Configuration.FloodOperations);
                                filePath = filePath + "\\" + attchmntName;
                                String newfilePath = Utility.GetImagePath(Common.Configuration.FloodOperations) + "\\" + _NewImageName + ".png";
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
                                lstNameofFiles.Add((_imagePath));

                            }
                        }
                    }
                }
                long ID = bllFloodOperations.AddDisposalEmergencyPurchase(_DisposalID, _EPWorkID, _VehicleNumber, _BuiltyNumber, _Quantity, _Cost, _Unit, lstNameofFiles, _UserID);

                if (ID > 0)
                {
                    object _disposalObj = bllFloodOperations.GetF_EmergencyDisposalObjectByID(ID);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _disposalObj);
                }
                //    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
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
        [Route("GetStoneDeploymentListbyStonePositionID")]
        public ServiceResponse GetStoneDeploymentListbyStonePositionID(long _StonePositionID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lststonedep = null;

                lststonedep = bllFloodFightingPlan.GetStoneDeploymentByStonePositionID(_StonePositionID);
                if (lststonedep.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lststonedep, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetStoneDeploymentListbyStonePositionID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }







        [HttpGet]
        [Route("GetNatureofWork")]
        public ServiceResponse GetNatureofWork()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstnatureofwork = null;

                lstnatureofwork = bllFloodOperations.GetFo_EP_NatureofWork();

                if (lstnatureofwork.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstnatureofwork, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetNatureofWork", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }








        [HttpPost]
        [Route("AddStoneDeployment")]
        public ServiceResponse AddStoneDeployment([FromBody] Dictionary<string, object> _StoneDeploymentDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_StoneDeploymentDetail))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _StonePosID = Convert.ToInt32(_StoneDeploymentDetail["_StonePosID"].ToString());
                int _UserID = Convert.ToInt32(_StoneDeploymentDetail["_UserID"].ToString());


                int _StoneDeploymentID = 0;
                if (_StoneDeploymentDetail.ContainsKey("_StoneDepID"))
                {
                    string stoneDepID = _StoneDeploymentDetail["_StoneDepID"].ToString();
                    if (!string.IsNullOrEmpty(stoneDepID) && !stoneDepID.Equals("null"))

                        _StoneDeploymentID = Convert.ToInt16(_StoneDeploymentDetail["_StoneDepID"].ToString());

                }

                string _VehicleNumber = _StoneDeploymentDetail["_VehicleNumber"].ToString();

                string _BuiltyNumber = (_StoneDeploymentDetail["_BuiltyNumber"].ToString());



                string _Quantity = (_StoneDeploymentDetail["_Quantity"].ToString());

                string _Cost = (_StoneDeploymentDetail["_Cost"].ToString());


                string attachments = null;
                string atchmnts = _StoneDeploymentDetail["_Attachments"].ToString();
                if (!string.IsNullOrEmpty(atchmnts) && !atchmnts.Equals("null"))
                    attachments = atchmnts;



                //link attachments to ClosureWorkProgress if there any
                string dateFormat = "yyyyMMdd-hhmmss";
                string _date = DateTime.Now.ToString(dateFormat);
                List<string> lstNameofFiles = new List<string>();
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
                                string _NewImageName = guid + "_" + "StoneDeployment" + "-" + _date;
                                string filePath = Utility.GetImagePath(Common.Configuration.FloodOperations);
                                filePath = filePath + "\\" + attchmntName;
                                String newfilePath = Utility.GetImagePath(Common.Configuration.FloodOperations) + "\\" + _NewImageName + ".png";
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
                                lstNameofFiles.Add((_imagePath));

                            }
                        }
                    }
                }

                
                long ID = bllFloodFightingPlan.AddStoneDeployment(_StoneDeploymentID, _StonePosID, _VehicleNumber, _BuiltyNumber, _Quantity, _Cost, lstNameofFiles, _UserID, bllFloodFightingPlan.FO_SDGetBalancedByFFPStonePositionID(_StonePosID, _StoneDeploymentID) + Convert.ToInt32(_Quantity));


                if (ID > 0)
                {
                    object _StoneDepObj = bllFloodFightingPlan.GetStoneDeploymentByID(ID);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _StoneDepObj);
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
        [Route("GetAttachmentStoneDeploymentByID")]
        public ServiceResponse GetAttachmentStoneDeploymentByID(long _ID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                List<object> lstattachmentstonedeployment = null;

                lstattachmentstonedeployment = bllFloodFightingPlan.GetAttachmentStoneDeploymentByID(_ID);

                if (lstattachmentstonedeployment.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstattachmentstonedeployment, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetAttachmentStoneDeploymentByID", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
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
                string filePath = Utility.GetImagePath(Common.Configuration.FloodOperations);
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
                srvcHlpr.LogNow_WebServices("FloodOperations:UploadFiles : - ", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1001, "Image Not Uploaded.", "" + exp.Message);
            }
            return svcResponse;
        }



        [HttpGet]
        [Route("SearchOnsiteMonitoring")]
        public ServiceResponse SearchOnsiteMonitoring(long? _ZoneID, int _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {


                List<object> lstsearchonsitemonitoring = null;

                lstsearchonsitemonitoring = bllOnsiteMonitoring.GetOnsiteMonitoringSearchListObject(null, null, null, _ZoneID, null, null, null, _UserID);

                if (lstsearchonsitemonitoring.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstsearchonsitemonitoring, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("SearchOnsiteMonitoring", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }


    //    GETFO_OMStonePositionIDList
        [HttpGet]
        [Route("GetOnsiteMonitoringStonePositionList")]
        public ServiceResponse GetOnsiteMonitoringStonePositionList(long _FFPID, long _StructureTypeID, long _StructureID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {


                List<object> objstoneposonsitemonitoringlist = null;

                objstoneposonsitemonitoringlist = bllOnsiteMonitoring.GETFO_OMStonePositionIDList(_FFPID, _StructureTypeID, _StructureID);

                if (objstoneposonsitemonitoringlist != null)
                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, objstoneposonsitemonitoringlist);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetOnsiteMonitoringStonePositionList", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }




        [HttpGet]
        [Route("GetOnsiteMonitoringCampSites")]
        public ServiceResponse GetOnsiteMonitoringCampSites(long _FFPID , long _StructureTypeID , long _StructureID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            
            {

                List<object> lstonsitemonitoringcampsites = null;

                lstonsitemonitoringcampsites = bllOnsiteMonitoring.GETFO_OMCampsites(_FFPID , _StructureTypeID , _StructureID);

                if (lstonsitemonitoringcampsites.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstonsitemonitoringcampsites, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetOnsiteMonitoringCampSites", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }



        [HttpPost]
        [Route("AddOnsiteMonitoringStonePosition")]
        public ServiceResponse AddOnsiteMonitoringStonePosition([FromBody] Dictionary<string, object> _OMStonePositionDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_OMStonePositionDetail))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                //long _StoneDepID = Convert.ToInt64(_OMStonePositionDetail["_StoneDepID"].ToString());
                int _UserID = Convert.ToInt32(_OMStonePositionDetail["_UserID"].ToString());

                long _StoneDeploymentID = Convert.ToInt64(_OMStonePositionDetail["_StoneDeploymentID"].ToString());


                //long OM_SPID = 0;
                //if (_OMStonePositionDetail.ContainsKey("OM_SPID"))
                //{
                //    string id = _OMStonePositionDetail["OM_SPID"].ToString();
                //    if (!string.IsNullOrEmpty(id) && !id.Equals("null"))

                //        OM_SPID = Convert.ToInt64(_OMStonePositionDetail["OM_SPID"].ToString());

                //}


                int _OnSiteQty = Convert.ToInt32(_OMStonePositionDetail["_OnSiteQty"].ToString());

                long ID = bllOnsiteMonitoring.SaveFO_OMStonePosition(_StoneDeploymentID, _OnSiteQty, _UserID);


                if (ID > 0)
                {
                    //  object _StoneDepObj = bllFloodFightingPlan.GetStoneDeploymentByID(ID);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, ServiceHelper.MSG_SUCCESS);
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


        [HttpPost]
        [Route("AddOnsiteMonitoringCampSites")]
        public ServiceResponse AddOnsiteMonitoringCampSites([FromBody] Dictionary<string, object> _OMCampSitesDetail)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_OMCampSitesDetail))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                //long _StoneDepID = Convert.ToInt64(_OMStonePositionDetail["_StoneDepID"].ToString());
                int _UserID = Convert.ToInt32(_OMCampSitesDetail["_UserID"].ToString());


                long _CampSiteID = 0;
                if (_OMCampSitesDetail.ContainsKey("_CampSiteID"))
                {
                    string campsiteID = _OMCampSitesDetail["_CampSiteID"].ToString();
                    if (!string.IsNullOrEmpty(campsiteID) && !campsiteID.Equals("null"))

                        _CampSiteID = Convert.ToInt64(_OMCampSitesDetail["_CampSiteID"].ToString());

                }



                long? _FFPCampSiteID = Convert.ToInt64(_OMCampSitesDetail["_FFPCampSiteID"].ToString());


                int _isAvailable = Convert.ToInt32(_OMCampSitesDetail["_isAvailable"].ToString());

                long ID = bllOnsiteMonitoring.SaveFO_OMCampSites(_CampSiteID, _isAvailable, _UserID, _FFPCampSiteID);


                if (ID > 0)
                {
                    object _OMcampsiteobj = bllOnsiteMonitoring.GetOnSiteMonitoringCampSitesObjectByID(ID);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _OMcampsiteobj);
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
        [Route("DeleteStoneDeployment")]
        public ServiceResponse DeleteStoneDeployment(int _SDID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (!bllFloodFightingPlan.IsStoneDeploymentExists(Convert.ToInt64(_SDID)))
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);

                }


                try
                {
                    bool IsAttachmentDeleted = bllFloodFightingPlan.DeleteStoneDeploymentAttachmentBySDID(_SDID);
                }
                catch (Exception e)
                {

                }


                bool IsDeleted = bllFloodFightingPlan.DeleteStoneDeployment(_SDID);

                if (IsDeleted)

                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_DELETED, null);

                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("DeleteStoneDeployment", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }


        [HttpGet]
        [Route("DeleteFO_EP_DisposalDetail")]
        public ServiceResponse DeleteFO_EP_DisposalDetail(int _DisposalDetailID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (!bllFloodOperations.IsFo_MaterialDisposalIDExists(Convert.ToInt64(_DisposalDetailID)))
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);

                }


                try
                {
                    bool IsAttachmentDeleted = bllFloodOperations.DeleteFo_MaterialDisposal_Attachement_List(_DisposalDetailID);
                }
                catch (Exception e)
                {


                }

                bool IsDeleted = bllFloodOperations.DeleteFo_MaterialDisposal(_DisposalDetailID);

                if (IsDeleted)

                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_DELETED, null);

                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("DeleteFO_EP_DisposalDetail", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }


        [HttpGet]
        [Route("DeleteFo_EPWork")]
        public ServiceResponse DeleteFo_EPWork(int _EPWorkID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {
                if (bllFloodOperations.IsFo_EpworkIDExists(Convert.ToInt64(_EPWorkID)))
                {
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);

                }

                bool IsDeleted = bllFloodOperations.DeleteFo_EPWork(_EPWorkID);

                if (IsDeleted)

                    svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_RECORD_DELETED, null);

                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_RECORD_NOT_DELETED, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("DeleteFo_EPWork", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }



        [HttpGet]
        [Route("GetItemsQtyList")]
        public ServiceResponse GetItemsQtyList(long FFPCampSitesID, int year, long divisionID, long itemCatID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {


                List<object> lstitemsqty = null;

                lstitemsqty = bllFloodFightingPlan.GetItemsQtyList(FFPCampSitesID, year, divisionID, itemCatID);

                if (lstitemsqty.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstitemsqty, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetItemsQtyList", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        //[HttpGet]
        //[Route("GetOMCampsiteItemList")]
        //public ServiceResponse GetOMCampsiteItemList(long FFPCampSitesID)
        //{
        //    ServiceResponse svcResponse = new ServiceResponse();
        //    try
        //    {


        //        List<object> lstitemsqty = null;

        //        lstitemsqty = bllFloodFightingPlan.GetItemsQtyList(FFPCampSitesID);

        //        if (lstitemsqty.Count > 0)
        //            svcResponse = srvcHlpr.CreateResponse<object>(lstitemsqty, 0, ServiceHelper.MSG_SUCCESS);
        //        else
        //            svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
        //    }
        //    catch (Exception exp)
        //    {
        //        srvcHlpr.LogNow_WebServices("GetOMCampsiteItemList", exp.Message);
        //        svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
        //        string excetionMsg = exp.Message;
        //    }
        //    return svcResponse;
        //}




        [HttpPost]
        [Route("AddEditCampSiteItems")]
        public ServiceResponse AddEditCampSiteItems([FromBody] Dictionary<string, object> _FOCampSiteItems)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_FOCampSiteItems))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                //long _StoneDepID = Convert.ToInt64(_OMStonePositionDetail["_StoneDepID"].ToString());
                int _UserID = Convert.ToInt32(_FOCampSiteItems["_UserID"].ToString());

                long _ItemID = Convert.ToInt64(_FOCampSiteItems["_OMCampSiteItems"].ToString());

                long _OMCampSiteID = Convert.ToInt64(_FOCampSiteItems["_OMCampSiteID"].ToString());


                long _OverallDivItemID = Convert.ToInt64(_FOCampSiteItems["_OverallDivItemID"].ToString());


                int _OnSiteQty = Convert.ToInt32(_FOCampSiteItems["_OnSiteQty"].ToString());


                // long _FFPCampSiteItemID = Convert.ToInt64(_FOCampSiteItems["_FFPCampSiteItemID"].ToString());


                long ID = bllFloodFightingPlan.SaveFO_FFPCampSiteItems(_ItemID, _OMCampSiteID, _OnSiteQty, _UserID, _OverallDivItemID);


                if (ID > 0)
                {
                    // object _StoneDepObj = bllFloodFightingPlan.GetStoneDeploymentByID(ID);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, ServiceHelper.MSG_SUCCESS);
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
        [Route("GetItemsQtyEPList")]
        public ServiceResponse GetItemsQtyEPList(long EPPurchaseID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {


                List<object> lstitemsepqty = null;

                lstitemsepqty = bllFloodFightingPlan.GetItemsQtyEPList(EPPurchaseID);

                if (lstitemsepqty.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstitemsepqty, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetItemsQtyEPList", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpPost]
        [Route("AddEditEPItems")]
        public ServiceResponse AddEditEPItems([FromBody] Dictionary<string, object> _FOEPItems)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_FOEPItems))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                //long _StoneDepID = Convert.ToInt64(_OMStonePositionDetail["_StoneDepID"].ToString());
                int _UserID = Convert.ToInt32(_FOEPItems["_UserID"].ToString());

                long _ItemID = Convert.ToInt64(_FOEPItems["_ItemID"].ToString());



                int _PurchasedQty = Convert.ToInt32(_FOEPItems["_PurchasedQty"].ToString());


                int _CurrentQty = Convert.ToInt32(_FOEPItems["_CurrentQty"].ToString());


                long EmergencyPurchaseID = Convert.ToInt64(_FOEPItems["EmergencyPurchaseID"].ToString());


                long EPItemID = 0;
                if (_FOEPItems.ContainsKey("EPItemID"))
                {
                    string _epitemID = _FOEPItems["EPItemID"].ToString();
                    if (!string.IsNullOrEmpty(_epitemID) && !_epitemID.Equals("null"))

                        EPItemID = Convert.ToInt64(_FOEPItems["EPItemID"].ToString());

                }

                int _preqty = bllFloodOperations.Get_EP_Item_PreviousQuantity(EPItemID);

                long ID = bllFloodFightingPlan.SaveFO_EPItems(_ItemID, EPItemID, _PurchasedQty + _preqty, _UserID, _CurrentQty, EmergencyPurchaseID);


                if (ID > 0)
                {

                    object _EPItemObj = bllFloodFightingPlan.GetEPItemsByID(ID);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _EPItemObj);
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
        [Route("GetiStonePosList")]
        public ServiceResponse GetiStonePosList(long _FloodInspID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {


                List<object> lstitemsistonepos = null;

                lstitemsistonepos = bllFloodInspection.GetiStonePosList(_FloodInspID);

                if (lstitemsistonepos.Count > 0)
                    svcResponse = srvcHlpr.CreateResponse<object>(lstitemsistonepos, 0, ServiceHelper.MSG_SUCCESS);
                else
                    svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_NO_RECORD_FOUND, null);
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("GetiStonePosList", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }




        [HttpPost]
        [Route("AddEditIStonePosition")]
        public ServiceResponse AddIStonePosition([FromBody] Dictionary<string, object> _IStonePosition)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                if (srvcHlpr.IsDictionaryEmptyOrNull(_IStonePosition))
                    return srvcHlpr.CreateResponse(1004, "FAILURE:Parameters are null.", null);

                int _UserID = Convert.ToInt32(_IStonePosition["_UserID"].ToString());
                long _FloodInspectionID = Convert.ToInt64(_IStonePosition["_FloodInspectionID"].ToString());
                long _IStonePositionID = 0;
                if (_IStonePosition.ContainsKey("_IStonePositionID"))
                {
                    string inspectionID = _IStonePosition["_IStonePositionID"].ToString();
                    if (!string.IsNullOrEmpty(inspectionID) && !inspectionID.Equals("null"))

                        _IStonePositionID = Convert.ToInt64(_IStonePosition["_IStonePositionID"].ToString());

                }
                int _RD = Convert.ToInt32(_IStonePosition["_RD"].ToString());

                int _BeforeFloodQty = Convert.ToInt32(_IStonePosition["_BeforeFloodQty"].ToString());

                int? _AvailableQty = null;
                string avlblQty = _IStonePosition["_AvailableQty"].ToString();
                if (!string.IsNullOrEmpty(avlblQty) && !avlblQty.Equals("null"))
                    _AvailableQty = Convert.ToInt32(_IStonePosition["_AvailableQty"].ToString());


                long id = bllFloodInspection.SaveIStonePosition(_FloodInspectionID, _IStonePositionID, _RD, _BeforeFloodQty, _AvailableQty, _UserID);

                if (id > 0)
                {

                    object _iStonePosObj = bllFloodInspection.GetiStonePosObjbyID(id);
                    svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, _iStonePosObj);
                   // svcResponse = srvcHlpr.CreateResponse(0, MSG_RECORD_SAVED, null);
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


        [HttpGet]
        [Route("CanAddFloodInspection")]
        public ServiceResponse CanAddFloodInspection()
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {



                short canAdd = bllFloodOperations.CanAddFloodInspections();

                svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, canAdd);
                
            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("CanAddFloodInspection", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
        [HttpGet]
        [Route("CanAddEmergencyPurchase")]
        public ServiceResponse CanAddEmergencyPurchase(int _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                int year = DateTime.Now.Year;
                long DesignationID = Convert.ToInt64(new LoginBLL().GetAndroidUserDesignationID(Convert.ToInt64(_UserID)));

                bool canAdd = bllFloodOperations.CanAddEditEmergencyPurchase(year,DesignationID);

                svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, canAdd);

            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("CanAddEmergencyPurchase", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }

        [HttpGet]
        [Route("CanAddStoneDeployment")]
        public ServiceResponse CanAddStoneDeployment(int _UserID)
        {
            ServiceResponse svcResponse = new ServiceResponse();
            try
            {

                int year = DateTime.Now.Year;
                long DesignationID = Convert.ToInt64(new LoginBLL().GetAndroidUserDesignationID(Convert.ToInt64(_UserID)));

                bool canAdd = bllFloodOperations.CanAddEditStoneDeployment(year, DesignationID);

                svcResponse = srvcHlpr.CreateResponse(0, ServiceHelper.MSG_SUCCESS, canAdd);

            }
            catch (Exception exp)
            {
                srvcHlpr.LogNow_WebServices("CanAddStoneDeployment", exp.Message);
                svcResponse = srvcHlpr.CreateResponse(1004, ServiceHelper.MSG_UNKNOWN_ERROR, null);
                string excetionMsg = exp.Message;
            }
            return svcResponse;
        }
    }

}