using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.FloodOperations;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.FloodOperations
{

    public class FloodInspectionsBLL : BaseBLL
    {
        FloodInspectionsDAL dalFloodInspections = new FloodInspectionsDAL();

        #region "Independent Inspection"
        /// <summary>
        /// This method return Detail of Independant Inspection
        /// Created on 13-Oct-2016
        /// </summary>
        /// <param name=""></param>
        /// <returns>object</returns>
        public object GetFloodInspectionByID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetFloodInspectionByID(_FloodInspectionID);
        }

        /// <summary>
        /// This method return list of InspectionConditions By Codition Group
        /// Created on 13-Oct-2016
        /// </summary>
        /// <param name="_CoditionGroup"></param>
        /// <returns>List<FO_InspectionConditions></returns>
        public List<FO_InspectionConditions> GetInspectionConditionsByGroup(string _CoditionGroup)
        {
            return dalFloodInspections.GetInspectionConditionsByGroup(_CoditionGroup);
        }

        /// <summary>
        /// This method return object of IGCProtectionInfrastructure By FloodInspectionID
        /// Created on 24-Oct-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>FO_IGCProtectionInfrastructure</returns>
        public FO_IGCProtectionInfrastructure GetIGCProtectionInfrastructureByInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetIGCProtectionInfrastructureByInspectionID(_FloodInspectionID);
        }
        public object GetIGCBarrageHWGInformationByInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetIGCBarrageHWGInformationByInspectionID(_FloodInspectionID);
        }
        /// <summary>
        /// This method return object of IGCProtectionInfrastructure By FloodInspectionID
        /// Created on 24-Oct-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>object</returns>
        public object GetIGConditionsByInspectionID(string _structureTypeID, long _InspectionID)
        {
            object objIGC = null;
            switch (_structureTypeID.ToUpper())
            {

                case "PROTECTION_INFRASTRUCTURE":
                    objIGC = dalFloodInspections.GetProtectionInfrastructureInspectionByInspectionID(_InspectionID);
                    break;
                case "DRAIN":
                    objIGC = GetIGCDrainInformationByInspectionID(_InspectionID);
                    break;
                case "CONTROL_STRUCTURE1":
                    objIGC = GetIGCBarrageHWGInformationByInspectionID(_InspectionID);
                    break;
                default:
                    break;
            }
            return objIGC;
        }
        /// <summary>
        /// This method return true or false on the base of weather IGCProtectionInfrastructure saved or not
        /// Created on 24-Oct-2016
        /// </summary>
        /// <param name="_IGCProtectionInfrastructure"></param>
        /// <returns>bool</returns>
        public bool SaveIGCProtectionInfrastructure(FO_IGCProtectionInfrastructure _IGCProtectionInfrastructure)
        {
            return dalFloodInspections.SaveIGCProtectionInfrastructure(_IGCProtectionInfrastructure);
        }



        /// <summary>
        /// This method return object of type object By FloodInspectionID, which holds FO_IGCBarrageHW and FO_IGCBarrageHWGates
        /// Created on 3-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>object</returns>
        public object GetIGCBarrageHWInformationByInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetIGCBarrageHWInformationByInspectionID(_FloodInspectionID);
        }
        public bool IsIGCBarrageHWGatesAlreadyExists(long _BarrageHWID)
        {
            return dalFloodInspections.IsIGCBarrageHWGatesAlreadyExists(_BarrageHWID);
        }
        /// <summary>
        /// This method return list of object of type object By BarrageHWID
        /// Created on 4-Nov-2016
        /// </summary>
        /// <param name="_IGCBarrageHWID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIGCBarrageHWGatesInformationByBarrageHWID(long _IGCBarrageHWID)
        {
            return dalFloodInspections.GetIGCBarrageHWGatesInformationByBarrageHWID(_IGCBarrageHWID);
        }


        public List<object> GetBarrageGatesInformationByFloodInspectionID(long _IGCBarrageHWID)
        {
            return dalFloodInspections.GetBarrageGatesInformationByFloodInspectionID(_IGCBarrageHWID);
        }


        public object GetIGCBarrageHWGatesInfoByBarrageHWID(long _IGCBarrageHWGateID)
        {
            return dalFloodInspections.GetIGCBarrageHWGatesInfoByBarrageHWID(_IGCBarrageHWGateID);
        }
        /// <summary>
        /// This method return true or false on the base of weather IGCBarrageHW saved or not
        /// Created on 03-Nov-2016
        /// </summary>
        /// <param name="_IGCBarrageHW"></param>
        /// <returns>bool</returns>
        public long SaveIGCBarrageHWInformation(FO_IGCBarrageHW _IGCBarrageHW)
        {
            return dalFloodInspections.SaveIGCBarrageHWInformation(_IGCBarrageHW);
        }

        /// <summary>
        /// This method return true or false on the base of weather IGCBarrageHWGates saved or not
        /// Created on 03-Nov-2016
        /// </summary>
        /// <param name="_IGCBarrageHW"></param>
        /// <returns>bool</returns>
        public bool SaveIGCBarrageHWGatesInformation(List<FO_IGCBarrageHWGates> _IGCBarrageHWGatesList)
        {
            return dalFloodInspections.SaveIGCBarrageHWGatesInformation(_IGCBarrageHWGatesList);
        }

        /// <summary>
        /// This method return object of type object By FloodInspectionID, which holds FO_IGCDrain
        /// Created on 8-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>object</returns>
        public object GetIGCDrainInformationByInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetIGCDrainInformationByInspectionID(_FloodInspectionID);
        }

        /// <summary>
        /// This method return true or false on the base of weather IGCDrain saved or not
        /// Created on 08-Nov-2016
        /// </summary>
        /// <param name="_IGCDrain"></param>
        /// <returns>bool</returns>
        public bool SaveIGCDrain(FO_IGCDrain _IGCDrain)
        {
            return dalFloodInspections.SaveIGCDrain(_IGCDrain);
        }

        /// <summary>
        /// This method return object of type object By FloodInspectionID, which holds FO_IBreachingSection
        /// Created on 11-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>object</returns>
        public List<object> GetIBreachingSectionByInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetIBreachingSectionByInspectionID(_FloodInspectionID);
        }

        /// <summary>
        /// This method return true or false on the base of weather _IBreachingSection saved or not
        /// Created on 11-Nov-2016
        /// </summary>
        /// <param name="_IBreachingSection"></param>
        /// <returns>bool</returns>
        public bool SaveIBreachingSection(FO_IBreachingSection _IBreachingSection)
        {
            return dalFloodInspections.SaveIBreachingSection(_IBreachingSection);
        }
        public bool SaveFloodIGCProtectionInfrastructure(long _FloodInspectionID, int _InspectionID, short? _GRHutConditionID, short? _ServiceRoadConditionID, short? _WatchingHutConditionID, bool _RiverGauge, string _Remarks, int _UserID)
        {
            return dalFloodInspections.SaveFloodIGCProtectionInfrastructure(_FloodInspectionID, _InspectionID, _GRHutConditionID, _ServiceRoadConditionID, _WatchingHutConditionID, _RiverGauge, _Remarks, _UserID);

        }
        public long SaveFloodIGCBarrage(long _FloodInspectionID, int _InspectionID, short? _ArmyCPConditionID, string _CCTVIncharge, string _CCTVInchargePhone, short? _DataBoardConditionID, short? _LightConditionID, short? _OperationalCameras, double? _OperationalDeckElevated, bool? _PoliceMonitory, short? _TollHutConditionID, short? _TotalCameras, String _Remarks, int _UserID)
        {
            return dalFloodInspections.SaveFloodIGCBarrage(_FloodInspectionID, _InspectionID, _ArmyCPConditionID, _CCTVIncharge, _CCTVInchargePhone, _DataBoardConditionID, _LightConditionID, _OperationalCameras, _OperationalDeckElevated, _PoliceMonitory, _TollHutConditionID, _TotalCameras, _Remarks, _UserID);
        }

        public bool SaveFloodIGCDrain(long _FloodInspectionID, int _InspectionID, double? _ExistingCapacity, double? _ImprovedCapacity, short? _CurrentLevel, bool _DrainWaterET, double? _OutfallBedWidth, double? _OutfallSupplyWidth, short? _BridgeGovtNo, short? _BridgePvtNo, string _Remarks, int _UserID)
        {
            return dalFloodInspections.SaveFloodIGCDrain(_FloodInspectionID, _InspectionID, _ExistingCapacity, _ImprovedCapacity, _CurrentLevel, _DrainWaterET, _OutfallBedWidth, _OutfallSupplyWidth, _BridgeGovtNo, _BridgePvtNo, _Remarks, _UserID);

        }

        #region R D Wise Condition
        public List<FO_StonePitchSide> GetAllActiveStonePitchSide()
        {
            return dalFloodInspections.GetAllActiveStonePitchSide();
        }
        public List<FO_EncroachmentType> GetAllActiveEncroachmentType()
        {
            return dalFloodInspections.GetAllActiveEncroachmentType();
        }
        public bool SaveIRDWiseConditionForStonePitching(FO_IRDWiseCondition IRDWiseCondition)
        {
            return dalFloodInspections.SaveIRDWiseConditionForStonePitching(IRDWiseCondition);
        }
        public bool DeleteRDWiseCondition(long _ID)
        {
            return dalFloodInspections.DeleteRDWiseCondition(_ID);
        }
        public List<object> GetRDWiseConditionForStonePitchingByID(long _FloodInspectionID, int _RDWiseTypeID)
        {
            return dalFloodInspections.GetRDWiseConditionForStonePitchingByID(_FloodInspectionID, _RDWiseTypeID);
        }
        public List<object> GetRDWiseConditionForStonePitchingApronByID(long _FloodInspectionID, int _RDWiseTypeID)
        {
            return dalFloodInspections.GetRDWiseConditionForStonePitchinApronByID(_FloodInspectionID, _RDWiseTypeID);
        }

        public List<object> GetEncroachmentByID(long _FloodInspectionD, int _RDWiseTypeID)
        {
            return dalFloodInspections.GetEncroachmentByID(_FloodInspectionD, _RDWiseTypeID);

        }
        public long SaveIRDWiseConditions(long _FloodInspectionID, int _InspectionID, int _FromRD, int _ToRD, short _RDWiseTypeID, short _ConditionID, short? _StonePitchSideID, short? _EncroachmentTypeID, string _Remarks, int _UserID)
        {
            return dalFloodInspections.SaveIRDWiseConditions(_FloodInspectionID, _InspectionID, _FromRD, _ToRD, _RDWiseTypeID, _ConditionID, _StonePitchSideID, _EncroachmentTypeID, _Remarks, _UserID);
        }
        public bool CheckInspectionStatusByID(long _FloodInspectionID)
        {
            bool boolValue = dalFloodInspections.CheckInspectionStatusByID(_FloodInspectionID);
            short? inspectionCategoryID = GetFloodInspectionCategoryByID(_FloodInspectionID);

            if (boolValue == true && (inspectionCategoryID != null && inspectionCategoryID == 1))
            {
                new FloodOperationsBLL().SendNotifiactions(_FloodInspectionID, GetFloodInspectionCreatedUserIDByID(_FloodInspectionID), (long)NotificationEventConstants.FloodOperations.FloodInspections);
            }

            return boolValue;
        }
        public object GetRDWiseConditionForByID(long _InspectionID, int _RDWiseTypeID)
        {
            object RDcondition = null;
            if (_RDWiseTypeID == (int)Constants.RDWiseType.StonePitching)
            {
                RDcondition = dalFloodInspections.GetStonePitchingRDConditionForByID(_InspectionID);
            }
            else if (_RDWiseTypeID == (int)Constants.RDWiseType.ErodingAnimalHoles)
            {
                RDcondition = dalFloodInspections.GetIRDConditionForByID(_InspectionID);
            }
            else if (_RDWiseTypeID == (int)Constants.RDWiseType.PitchStoneAppron)
            {
                RDcondition = dalFloodInspections.GetIRDConditionForByID(_InspectionID);
            }
            else if (_RDWiseTypeID == (int)Constants.RDWiseType.RainCuts)
            {
                RDcondition = dalFloodInspections.GetIRDConditionForByID(_InspectionID);
            }
            else if (_RDWiseTypeID == (int)Constants.RDWiseType.RDMarks)
            {
                RDcondition = dalFloodInspections.GetIRDConditionForByID(_InspectionID);
            }
            else if (_RDWiseTypeID == (int)Constants.RDWiseType.Enchrochment)
            {
                RDcondition = dalFloodInspections.GetEncroachmentRDConditionForByID(_InspectionID);
            }
            return RDcondition;
        }
        #endregion R D Wise Condition

        #region MeasuringBookStatus
        public List<object> GetMBStatusPreItemList(long _DivisionID, int _Year, int _ItemCategoryID, long _StructureTypeID, long StructureID)
        {
            return dalFloodInspections.GetMBStatusPreItemList(_DivisionID, _Year, _ItemCategoryID, _StructureTypeID, StructureID);
        }

        public List<object> GetMBStatusPostItemList(long _DivisionID, int _Year, int _ItemCategoryID, long _StructureTypeID, long _StructureID)
        {
            return dalFloodInspections.GetMBStatusPostItemList(_DivisionID, _Year, _ItemCategoryID, _StructureTypeID, _StructureID);
        }
        public object GetMBStatusByID(long _StatusID, int _StatusType)
        {
            return dalFloodInspections.GetMBStatusByID(_StatusID, _StatusType);
        }

        //public List<object> GetMBStatusPostItemList(long _CategoryID, int _FloodInspectionID)
        //{

        //    return dalFloodInspections.GetMBStatusPostItemList(_CategoryID, _FloodInspectionID);
        //}

        public bool SavePreMBStatus(FO_PreMBStatus preMBStatus)
        {
            return dalFloodInspections.SavePreMBStatus(preMBStatus);
        }

        public IEnumerable<DataRow> GetPreMBStatus(long _DivisionID, int _Year, Int16 _CategoryID,
            long _StructureTypeID, long _StructureID)
        {
            return dalFloodInspections.GetPreMBStatus(_DivisionID, _Year, _CategoryID, _StructureTypeID, _StructureID);
        }

        public DataSet GetPostMBStatus(long _DivisionID, int _Year, Int16 _CategoryID,
            long _StructureTypeID, long _StructureID)
        {
            return dalFloodInspections.GetPostMBStatus(_DivisionID, _Year, _CategoryID, _StructureTypeID, _StructureID);
        }

        public bool SavePostMBStatus(FO_OverallDivItems Obj)
        {
            return dalFloodInspections.SavePostMBStatus(Obj);
        }

        //public object GetFloodInspectionDetailID_ByInspectionID(long _FloodInspectionID)
        //{
        //    return dalFloodInspections.GetFloodInspectionDetailID_ByInspectionID(_FloodInspectionID);
        //}


        public int GetInspectionStatus(long _FloodInspectionID)
        {

            return dalFloodInspections.GetInspectionStatus(_FloodInspectionID);
        }

        #endregion

        #region ProblemForFloodInspection

        public List<object> GetProblemForFIByFloodInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetProblemForFIByFloodInspectionID(_FloodInspectionID);
        }
        public object GetProblemForFIByInspectionID(long _InspectionID)
        {
            return dalFloodInspections.GetProblemForFIByInspectionID(_InspectionID);
        }
        public List<FO_ProblemNature> GetAllActiveProblemNature()
        {
            return dalFloodInspections.GetAllActiveProblemNature();
        }

        public bool SaveProblemForFI(FO_IProblems _ObjProFI)
        {
            return dalFloodInspections.SaveProFI(_ObjProFI);
        }

        public bool DeleteProblemFI(long _ID)
        {
            return dalFloodInspections.DeleteProblemFI(_ID);
        }

        public long? GetInfrastructureType(long _FloodInspectionID)
        {

            return dalFloodInspections.GetInfrastructureType(_FloodInspectionID);
        }
        public List<object> GetAllProblemNature()
        {
            return dalFloodInspections.GetAllProblemNature();
        }

        public long SaveProblemForFI(long _FloodInspectionID, int _InspectionID, int? _FromRD, int? _ToRD, short _ProblemID, string _Solution, long? _RestorationCost, long _UserID)
        {
            return dalFloodInspections.SaveProblemForFI(_FloodInspectionID, _InspectionID, _FromRD, _ToRD, _ProblemID, _Solution, _RestorationCost, _UserID);
        }
        public bool SaveBreachSectionForFI(long _FloodInspectionID, int _InspectionID, int _FromRD, int _ToRD, short? _AffectedRowsNo, short? _AffectedLinersNo, string _Solution, long? _RestorationCost, long _UserID)
        {
            return dalFloodInspections.SaveBreachSectionForFI(_FloodInspectionID, _InspectionID, _FromRD, _ToRD, _AffectedRowsNo, _AffectedLinersNo, _Solution, _RestorationCost, _UserID);
        }
        public long SaveIStonePosition(long _FloodInspectionID, long _InspectionID, int _RD, int _BeforeFloodQty, int? _AvailableQty, long _UserID)
        {
            return dalFloodInspections.SaveIStonePosition(_FloodInspectionID, _InspectionID, _RD, _BeforeFloodQty, _AvailableQty, _UserID);
        }
        public long SavePreFloodMBStatus(long _PreMBStatusID, long _FloodInspectionsID, long _ItemID, int _LastYrQty, int _DivStrIssuedQty, int _AvailableQty, int _UserID)
        {
            return dalFloodInspections.SavePreFloodMBStatus(_PreMBStatusID, _FloodInspectionsID, _ItemID, _LastYrQty, _DivStrIssuedQty, _AvailableQty, _UserID);
        }





        public long SavePostFloodMBStatus(long _OverallDivItemID, int _Year, long? _DivisionID,
                                          long? _ItemCategoryID, long? _ItemSubcategoryID, long? _StructureTypeID, long? _StructureID, long? _PreMBStatusID,
                                          long? _FloodInspectionDetailID, int? _PostAvailableQty, long? _PostRequiredQty,
                                          long? _CS_CampSiteID, int? _CS_RequiredQty, int? _OD_AdditionalQty, int _CreatedBy, int _ModifiedBy, long _ODIID)
        {
            return dalFloodInspections.SavePostFloodMBStatus(_OverallDivItemID, _Year, _DivisionID, _ItemCategoryID, _ItemSubcategoryID, _StructureTypeID, _StructureID, _PreMBStatusID, _FloodInspectionDetailID, _PostAvailableQty, _PostRequiredQty, _CS_CampSiteID, _CS_RequiredQty, _OD_AdditionalQty, _CreatedBy, _ModifiedBy, _ODIID);
        }



        public object GET_PostMBStatus_Object(long ID)
        {
            return dalFloodInspections.GET_PostMBStatus_Object(ID);
        }

        public object GET_PreMBStatus_Object(long ID)
        {
            return dalFloodInspections.GET_PreMBStatus_Object(ID);
        }


        public long GetFOPreMbStatusID(int _Year, long _DivisionID, long _StructureTypeID, long _StructureID, long _ItemID)
        {
            return dalFloodInspections.GetFOPreMbStatusID(_Year, _DivisionID, _StructureTypeID, _StructureID, _ItemID);
        }

        #endregion

        #region IStonePosition

        public List<object> GetIStonePositionByFloodInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetIStonePositionByFloodInspectionID(_FloodInspectionID);
        }

        public bool SaveIStonePosition(FO_IStonePosition _Obj)
        {
            return dalFloodInspections.SaveIStonePosition(_Obj);
        }


        public List<object> GetiStonePosList(long _FloodInspID)
        {
            return dalFloodInspections.GetiStonePosList(_FloodInspID);
        }

        public object GetiStonePosObjbyID(long _ID)
        {
            return dalFloodInspections.GetiStonePosObjbyID(_ID);
        }



        #endregion




        public object GetFIDetailIDbyFIID(long _FIID)
        {
            return dalFloodInspections.GetFIDetailIDbyFIID(_FIID);
        }

        public object GetInspectionDivisionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetInspectionDivisionID(_FloodInspectionID);
        }

        public DataSet GetPreMBStatusID(int _year, long _DivisionID, long _StructureTypeID, long _StructureID, long _ItemID)
        {
            return dalFloodInspections.GetPreMBStatusID(_year, _DivisionID, _StructureTypeID, _StructureID, _ItemID);
        }

        public long PreMBStatusInsertion(long _PreMBStatusID, long? _FloodInspectionID, long? _ItemID, int? _LastYrQty, int? _DivStrIssuedQty, int? _AvailableQty, int _CreatedBy, int _ModifiedBy, long _ODIID)
        {
            return dalFloodInspections.PreMBStatusInsertion(_PreMBStatusID, _FloodInspectionID, _ItemID, _LastYrQty, _DivStrIssuedQty, _AvailableQty, _CreatedBy, _ModifiedBy, _ODIID);
        }

        #endregion "Independent Inspection"

        #region Departmental Inspection
        public object GetDepartmentalInspectionByID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetDepartmentalInspectionByID(_FloodInspectionID);
        }


        #region Attachments
        public List<object> GetAttachmentsByFloodInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetAttachmentsByFloodInspectionID(_FloodInspectionID);
        }

        public bool SaveAttachments(FO_DAttachments _DAttachments)
        {
            return dalFloodInspections.SaveAttachments(_DAttachments);
        }

        public bool DeleteAttachments(long _ID)
        {
            return dalFloodInspections.DeleteAttachments(_ID);
        }

        public bool AttachmentDublication(long _ID, string _FileName)
        {
            return dalFloodInspections.AttachmentDublication(_ID, _FileName);

        }

        #endregion


        #region MemberDetails
        public List<object> GetMemberDetailsByFloodInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetMemberDetailsByFloodInspectionID(_FloodInspectionID);
        }

        public bool SaveMemberDetails(FO_DMemberDetails _DMemberDetails)
        {
            return dalFloodInspections.SaveMemberDetails(_DMemberDetails);
        }

        public bool DeleteMemberDetails(long _ID)
        {
            return dalFloodInspections.DeleteMemberDetails(_ID);
        }

        public bool IsDMembersUnique(FO_DMemberDetails _DMemberDetails)
        {
            return dalFloodInspections.IsDMembersUnique(_DMemberDetails);
        }

        #endregion

        #region Infrastructures
        public List<object> GetInfrastructuresByFloodInspectionID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetInfrastructuresByFloodInspectionID(_FloodInspectionID);
        }

        public bool SaveInfrastructures(FO_DInfrastructures _DInfrastructures)
        {
            return dalFloodInspections.SaveInfrastructures(_DInfrastructures);
        }

        public List<CO_StructureType> GetAllInfrastructuresTypes()
        {
            return dalFloodInspections.GetAllInfrastructuresTypes();
        }

        public bool DeleteInfrastructure(long _ID)
        {
            return dalFloodInspections.DeleteInfrastructure(_ID);
        }

        public bool IsDInfrastructureUnique(FO_DInfrastructures _DInfrastructures)
        {
            return dalFloodInspections.IsDInfrastructureUnique(_DInfrastructures);
        }

        #endregion

        #region SearchDepartmentalInspection
        public List<object> GetDepartmentalInspectionSearch(long? _FloodInspectionID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _Status, DateTime? _FromDate, DateTime? _ToDate)
        {
            FloodInspectionsDAL dalFloodInspection = new FloodInspectionsDAL();
            return dalFloodInspection.GetDepartmentalInspectionSearch(_FloodInspectionID, _ZoneID, _CircleID, _DivisionID, _Status, _FromDate, _ToDate);
        }


        #endregion


        #endregion

        #region JointInspection
        public bool IsJIFInfrastructureExist(FO_JInfrastructures _ObjModel)
        {
            return dalFloodInspections.IsJIFInfrastructureExist(_ObjModel);
        }
        public bool IsJIFInfrastructureExistUpdate(FO_JInfrastructures _ObjModel)
        {
            return dalFloodInspections.IsJIFInfrastructureExist(_ObjModel);
        }
        public List<object> GetjointInspectionSearch(long? _FloodInspectionID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _Status, DateTime? _FromDate, DateTime? _ToDate)
        {
            return dalFloodInspections.GetjointInspectionSearch(_FloodInspectionID, _ZoneID, _CircleID, _DivisionID, _Status, _FromDate, _ToDate);
        }

        public FO_FloodInspection GetInspectionDetailsByID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetInspectionDetailsByID(_FloodInspectionID);
        }

        public bool IsFloodInspectionDependencyExists(long _FloodInspectionID)
        {
            return dalFloodInspections.IsFloodInspectionDependencyExists(_FloodInspectionID);
        }

        public bool DeleteJointFloodInspection(long _FloodInspectionID)
        {
            return dalFloodInspections.DeleteJointFloodInspection(_FloodInspectionID);
        }

        public object GetjointInspectionDetail(long _FloodInspectionID)
        {
            return dalFloodInspections.GetjointInspectionDetail(_FloodInspectionID);
        }

        public List<object> GetInfrastructuresForJointInspection(long _FloodInspectionID)
        {
            return dalFloodInspections.GetInfrastructuresForJointInspection(_FloodInspectionID);
        }

        public bool SaveJointInfrastructure(FO_JInfrastructures _objJInfrastructures)
        {
            return dalFloodInspections.SaveJointInfrastructure(_objJInfrastructures);
        }

        public bool DeleteJointInspectionInfrastructure(long _ID)
        {
            return dalFloodInspections.DeleteJointInspectionInfrastructure(_ID);
        }

        public List<object> GetJointMemberDetails(long _FloodInspectionID)
        {
            return dalFloodInspections.GetJointMemberDetails(_FloodInspectionID);
        }

        public bool SaveJointMemberDetails(FO_JMemberDetails _ObjJointDetails)
        {
            return dalFloodInspections.SaveJointMemberDetails(_ObjJointDetails);
        }

        public bool DeleteJointMemberDetails(long _ID)
        {
            return dalFloodInspections.DeleteJointMemberDetails(_ID);
        }

        public bool IsJointMemberDetailsExist(FO_JMemberDetails ObjMemberDetail)
        {
            return dalFloodInspections.IsJointMemberDetailsExist(ObjMemberDetail);
        }

        public List<object> GetJointAttachmentsDetails(long _FloodInspectionID)
        {
            return dalFloodInspections.GetJointAttachmentsDetails(_FloodInspectionID);
        }

        public bool DeleteJointAttachments(long _ID)
        {
            return dalFloodInspections.DeleteJointAttachments(_ID);
        }

        public bool SaveJointAttachments(FO_JAttachments _ObjModel)
        {
            return dalFloodInspections.SaveJointAttachments(_ObjModel);
        }

        public IEnumerable<DataRow> FO_LoadStonePosition(long _FloodInspectionID)
        {
            return dalFloodInspections.FO_LoadStonePosition(_FloodInspectionID);
        }
        public List<FO_InspectionConditions> GetAllInspectionConditions()
        {
            return dalFloodInspections.GetAllInspectionConditions();
        }
        #endregion


        #region Notification
        public FO_GetFloodInspectionNotifyData_Result GetFloodInspectionNotifyData(long _FloodInspectionID)
        {
            return dalFloodInspections.GetFloodInspectionNotifyData(_FloodInspectionID);
        }
        public long GetFloodInspectionCreatedUserIDByID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetFloodInspectionCreatedUserIDByID(_FloodInspectionID);
        }

        public short? GetFloodInspectionCategoryByID(long _FloodInspectionID)
        {
            return dalFloodInspections.GetFloodInspectionCategoryByID(_FloodInspectionID);
        }
        #endregion  Notification




        #region RoleRights
        public bool CanAddEditJointDepartmentalInspection(int _Year, long _DesignationID, int _InspectionStatus)
        {
            bool returnVal = false;
            if (_DesignationID == Convert.ToInt64(Constants.Designation.XEN))
            {
                if (_InspectionStatus == 2)
                {
                    returnVal = false;
                }
                else if ((DateTime.Now.Year == _Year || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO")))) && _InspectionStatus == 1)
                {
                    returnVal = true;
                }
            }
            else if (_DesignationID == Convert.ToInt64(Constants.Designation.DF))
            {
                if ((DateTime.Now.Year == _Year || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO")))) && _InspectionStatus == 2)
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }

        #endregion RoleRights


    }
}

