using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.DAL.DataAccess.FloodOperations;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace PMIU.WRMIS.BLL.FloodOperations
{
    public class FloodOperationsBLL : BaseBLL
    {
        private FloodOperationsDAL dalFloodOperations = new FloodOperationsDAL();

        #region "Division Summmary"

        /// <summary>
        /// This method return List of Division Summmary
        /// Created on 03-Oct-2016
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<object></returns>
        public List<object> GetDivisionSummaryList()
        {
            FloodOperationsDAL dalFloodOperations = new FloodOperationsDAL();
            return dalFloodOperations.GetDivisionSummaryList();
        }

        public List<object> GetDivisionSummaryBySearchCriteria(long _DivisionSummaryID
          , long _ZoneID
          , long _CircleID
          , long _DivisionID
          , long _Year
          , string _DivisionSummaryStatus)
        {
            FloodOperationsDAL dalFloodOperations = new FloodOperationsDAL();
            return dalFloodOperations.GetDivisionSummaryBySearchCriteria(_DivisionSummaryID
            , _ZoneID
            , _CircleID
            , _DivisionID
            , _Year
            , _DivisionSummaryStatus);
        }

        /// <summary>
        /// This function retun Division Summary addition success along with message
        /// Created on 03-Oct-2016
        /// </summary>
        /// <param name="_DivisionSummary"></param>
        /// <returns>bool</returns>
        public bool SaveDivisionSummary(FO_DivisionSummary _DivisionSummary)
        {
            FloodOperationsDAL dalFloodOperations = new FloodOperationsDAL();
            return dalFloodOperations.SaveDivisionSummary(_DivisionSummary);
        }

        /// <summary>
        /// This function deletes a Division Summary with the provided ID.
        /// Created On 03-Oct-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteDivisionSummary(long _ID)
        {
            FloodOperationsDAL dalFloodOperations = new FloodOperationsDAL();
            return dalFloodOperations.DeleteDivisionSummary(_ID);
        }

        /// <summary>
        /// This function will validate if the combination of Division and Year has already been entered
        /// Created On 03-Oct-2016
        /// </summary>
        /// <param name="_DivisionSummary"></param>
        /// <returns>bool</returns>
        public bool IsDivisionSummaryAlreadyExists(FO_DivisionSummary _DivisionSummary)
        {
            FloodOperationsDAL dalFloodOperations = new FloodOperationsDAL();
            return dalFloodOperations.IsDivisionSummaryAlreadyExists(_DivisionSummary);
        }

        /// <summary>
        /// This function checks in all related tables for given Division SummaryID.
        /// Created On 03-Oct-2016
        /// </summary>
        /// <param name="_DivisionSummaryID"></param>
        /// <returns>bool</returns>
        public bool IsDivisionSummaryIDExists(long _DivisionSummaryID)
        {
            FloodOperationsDAL dalFloodOperations = new FloodOperationsDAL();
            return dalFloodOperations.IsDivisionSummaryIDExists(_DivisionSummaryID);
        }

        //
        public DataSet GetDivisionStoreItemsQty(long? DivisionSummaryID, long? Categoryid)
        {
            return dalFloodOperations.GetDivisionStoreItemsQty(DivisionSummaryID, Categoryid);
        }

        //
        public DataSet GetDivisionSummaryItemsQty(long? DivisionSummaryID, long? Categoryid, long _InfrastructureTypeID, long _InfrastructureID)
        {
            return dalFloodOperations.GetDivisionSummaryItemsQty(DivisionSummaryID, Categoryid, _InfrastructureTypeID, _InfrastructureID);
        }

        public object GetDivisionSummaryDetailByID(long _DivisionSummaryID)
        {
            return dalFloodOperations.GetDivisionSummaryDetailByID(_DivisionSummaryID);
        }

        public IEnumerable<DataRow> GetDivisionSummayInfrastructure(long _DivisionID)
        {
            return dalFloodOperations.GetDivisionSummayInfrastructure(_DivisionID);
        }

        public DataSet DivisionSummayInfrastructure(long _DivisionID)
        {
            return dalFloodOperations.DivisionSummayInfrastructure(_DivisionID);
        }

        public IEnumerable<DataRow> GetDivisionSummayStonePosition(long _DivisionID, int _Year)
        {
            return dalFloodOperations.GetDivisionSummayStonePosition(_DivisionID, _Year);
        }

        public dynamic GetDivisionSummaryInfrastructure(long _DivisionID)
        {
            return dalFloodOperations.GetDivisionSummaryInfrastructure(_DivisionID);
        }

        public FO_DivisionSummary GetDivisionSummaryID(long _ID)
        {
            return dalFloodOperations.GetDivisionSummaryID(_ID);
        }

        public bool CheckDivisionSummaryStatusByID(long _ID)
        {
            return dalFloodOperations.CheckDivisionSummaryStatusByID(_ID);
        }

        #endregion "Division Summmary"

        #region "Flood Inspection"

        /// <summary>
        /// This function fetches Division based on the UserID and User Irrigation Level and Circle ID.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_CircleID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByUserIDAndCircleID(long _UserID, long _IrrigationLevelID, long? _CircleID)
        {
            return dalFloodOperations.GetDivisionsByUserIDAndCircleID(_UserID, _IrrigationLevelID, _CircleID);
        }

        /// This function fetches Circles based on the UserID and User Irrigation Level and Zone ID.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_ZoneID"></param>
        /// <returns>List<CO_Circle></returns>
        public List<CO_Circle> GetCircleByUserIDAndZoneID(long _UserID, long _IrrigationLevelID, long? _ZoneID)
        {
            return dalFloodOperations.GetCircleByUserIDAndZoneID(_UserID, _IrrigationLevelID, _ZoneID);
        }

        /// This function fetches Zones based on the UserID and User Irrigation Level.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_Zone></returns>
        public List<CO_Zone> GetZoneByUserID(long _UserID, long _IrrigationLevelID)
        {
            return dalFloodOperations.GetZoneByUserID(_UserID, _IrrigationLevelID);
        }

        /// <summary>
        /// This method return List of Flood Inspection Conditions by Group
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_GroupID"></param>
        /// <returns>List<FO_InspectionConditions></returns>
        public List<FO_InspectionConditions> GetFloodInspectionConditions(string _Group)
        {
            return dalFloodOperations.GetFloodInspectionConditions(_Group);
        }

        /// <summary>
        /// This method return List of Flood Inspection Types
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_Group"></param>
        /// <returns>List<FO_InspectionConditions></returns>
        public List<object> GetAllFloodInspectionTypes()
        {
            return dalFloodOperations.GetAllFloodInspectionTypes();
        }

        /// <summary>
        /// This method return List of Flood Inspection Status
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_Group"></param>
        /// <returns>List<FO_InspectionConditions></returns>
        public List<object> GetAllFloodInspectionStatus()
        {
            return dalFloodOperations.GetAllFloodInspectionStatus();
        }

        /// <summary>
        /// This method return List of InfraStructure Types
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_Group"></param>
        /// <returns>List<CO_StructureType></returns>
        public List<object> GetAllStructureTypes()
        {
            return dalFloodOperations.GetAllStructureTypes();
        }

        /// <summary>
        /// This function return List of all active Inspection Type
        /// </summary>
        /// <returns></returns>
        /// <created>10/07/2016</created>
        /// <changed>10/07/2016</changed>
        public List<FO_InspectionType> GetAllActiveInspectionType()
        {
            return dalFloodOperations.GetAllActiveInspectionType();
        }

        public List<FO_InspectionType> GetPrePostInspectionByID(Int16 InspectionID)
        {
            return dalFloodOperations.GetPrePostInspectionByID(InspectionID);
        }

        /// <summary>
        /// This function return List of all active Inspection Status
        /// </summary>
        /// <returns></returns>
        /// <created>10/07/2016</created>
        /// <changed>10/07/2016</changed>
        public List<FO_InspectionStatus> GetAllActiveInspectionStatus()
        {
            return dalFloodOperations.GetAllActiveInspectionStatus();
        }

        public List<CO_StructureType> GetInfrastructureTypeName(String InfrastructureName)
        {
            return dalFloodOperations.GetInfrastructureTypeName(InfrastructureName);
        }

        public List<object> GetProtectionInfrastructureName(long UserId, long InfrastructureType)
        {
            return dalFloodOperations.GetProtectionInfrastructureName(UserId, InfrastructureType);
        }

        public List<object> GetInfrastructureName(long _UserId, long _InfrastructureType)
        {
            return dalFloodOperations.GetInfrastructureName(_UserId, _InfrastructureType);
        }

        public List<object> GetProtectionInfrastructureNameByType(string InfrastructureType)
        {
            return dalFloodOperations.GetInfrastructureDetailsByType(InfrastructureType);
        }

        public bool IsFloodInspectionDataAlreadyExists(FO_FloodInspection _FloodInspection, FO_FloodInspectionDetail FloodInspectionDetail)
        {
            return dalFloodOperations.IsFloodInspectionDataAlreadyExists(_FloodInspection, FloodInspectionDetail);
        }

        public bool DeleteInspectionByFloodInspectionID(long _FloodInspection)
        {
            return dalFloodOperations.DeleteInspectionByFloodInspectionID(_FloodInspection);
        }

        public bool SaveFloodInspection(FO_FloodInspection _FloodInspection)
        {
            return dalFloodOperations.SaveFloodInspection(_FloodInspection);
        }

        public DataSet FO_irrigationRDs(long? DivisionID, long? StructID, long? StructTypeID)
        {
            return new FloodOperationsDAL().FO_irrigationRDs(DivisionID, StructID, StructTypeID);
        }

        public bool SaveFloodInspectionDetail(FO_FloodInspectionDetail _FloodInspectionDetail)
        {
            return dalFloodOperations.SaveFloodInspectionDetail(_FloodInspectionDetail);
        }

        public bool SavaFloodInspection(long _FloodInspectionID, long _DivisionID, short? _CategoryID, short _StatusID, short _TypeID, long _StructureID, long _StructureTypeID, int _UserID)
        {
            return dalFloodOperations.SavaFloodInspection(_FloodInspectionID, _DivisionID, _CategoryID, _StatusID, _TypeID, _StructureID, _StructureTypeID, _UserID);
        }

        public bool IsFloodInspectionDataAlreadyExist(long _DivisioID, string _Year, string _InspectionDate, long _InspectionTypeID, long _StructureTypeID, long _StructureID, long _floodInspectionID)
        {
            return dalFloodOperations.IsFloodInspectionDataAlreadyExists(_DivisioID, _Year, _InspectionDate, _InspectionTypeID, _StructureTypeID, _StructureID, _floodInspectionID);
        }

        public bool IsFloodInspectionDependencyExists(long _FloodInspectionID)
        {
            return dalFloodOperations.IsFloodInspectionDependencyExists(_FloodInspectionID);
        }

        public bool DeleteFloodInspection(long _ID)
        {
            return dalFloodOperations.DeleteFloodInspection(_ID);
        }

        public List<object> GetUserFloodInspections(string _UserID, string _InspectionID)
        {
            return dalFloodOperations.GetUserFloodInspections(_UserID, _InspectionID);
        }

        public object GetUserFloodInspectionsObject(string _UserID)
        {
            return dalFloodOperations.GetUserFloodInspectionsObject(_UserID, null);
        }

        public List<object> GetAllActiveRDWiseTypes()
        {
            return dalFloodOperations.GetAllActiveRDWiseTypes();
        }

        public List<object> GetAllActiveStonePitchSides()
        {
            return dalFloodOperations.GetAllActiveStonePitchSides();
        }

        public List<object> GetIRDWiseConditionsByRDType(string _FloodInspectionID, string _RDWiseTypeID)
        {
            return dalFloodOperations.GetIRDWiseConditionsByRDType(_FloodInspectionID, _RDWiseTypeID);
        }

        public List<object> GetInfrastructureDetailsByType(string _InfrastructureType)
        {
            return dalFloodOperations.GetInfrastructureDetailsByType(_InfrastructureType);
        }

        public bool IsFloodInspectionParentExists(long _FloodInspectionID)
        {
            return dalFloodOperations.IsFloodInspectionParentExists(_FloodInspectionID);
        }

        public object GetInfrastructureTypeByID(long _FloodInspectionID)
        {
            return dalFloodOperations.GetInfrastructureTypeByID(_FloodInspectionID);
        }

        public FO_GetFloodInspectionsDetailByID_Result2 GetFloodInspectionsDetail(string _InfrastructureType, long _InspectionID)
        {
            return dalFloodOperations.GetFloodInspectionsDetail(_InfrastructureType, _InspectionID);
        }

        public object GetStructureTypeIDByInsfrastructureValue(long _InfrastructureType, long _InfrastructureNamevalue)
        {
            return dalFloodOperations.GetStructureTypeIDByInsfrastructureValue(_InfrastructureType, _InfrastructureNamevalue);
        }

        public FO_FloodInspectionDetail IsFloodInspectionDetailaAlreadyExists(long _FloodInspectionID)
        {
            return dalFloodOperations.IsFloodInspectionDetailaAlreadyExists(_FloodInspectionID);
        }

        public IEnumerable<DataRow> GetFloodInspectionSearch(string _InfrastructureType, long? _FloodInspectionID, long? _ZoneID, long? _CircleID, long? _DivisionID,
           long? _InspectionType, long? _Status, string _InfrastructureName, DateTime? _FromDate, DateTime? _ToDate, long? _UserID)
        {
            return dalFloodOperations.GetFloodInspectionSearch(_InfrastructureType, _FloodInspectionID, _ZoneID, _CircleID, _DivisionID, _InspectionType, _Status, _InfrastructureName, _FromDate, _ToDate, _UserID);
        }

        public List<object> GetBarrageGatesDetailByStationID(string _StationID)
        {
            return dalFloodOperations.GetBarrageGatesDetailByStationID(_StationID);
        }

        public List<object> GetAllEncroachmentTypes()
        {
            return dalFloodOperations.GetAllEncroachmentTypes();
        }

        #region DepartmentalInspection

        public bool IsDepartmentalInspectionDependencyExists(long _FloodInspectionID)
        {
            return dalFloodOperations.IsDepartmentalInspectionDependencyExists(_FloodInspectionID);
        }

        #endregion DepartmentalInspection

        #endregion "Flood Inspection"

        #region Flood Departmental

        public object GetDepartmentalByID(long _FloodInspectionID)
        {
            return dalFloodOperations.GetDepartmentalByID(_FloodInspectionID);
        }

        #endregion Flood Departmental

        #region Emergency Purchases

        public bool IsEmergencyPurchasesParentExists(long _EmergencyPurchasesID)
        {
            return dalFloodOperations.IsEmergencyPurchasesParentExists(_EmergencyPurchasesID);
        }

        public bool SaveEmergencyPurchases(FO_EmergencyPurchase _EmergencyPurchases)
        {
            return dalFloodOperations.SaveEmergencyPurchases(_EmergencyPurchases);
        }

        public long SaveEmergencyPurchase(long _PurchaseID, long _DivisionID, long? _StructureTypeID, long _StructureID, bool? IsCampSite, string _RDLeft, string _RDRight, long _UserID)
        {
            return dalFloodOperations.SaveEmergencyPurchase(_PurchaseID, _DivisionID, _StructureTypeID, _StructureID, IsCampSite, _RDLeft, _RDRight, _UserID);
        }

        public bool DeleteEmergencyPurchase(long _ID)
        {
            FloodOperationsDAL dalFloodOperations = new FloodOperationsDAL();
            return dalFloodOperations.DeleteEmergencyPurchase(_ID);
        }

        public List<FO_EmergencyPurchase> GetAllEmergencyPurchasesYear()
        {
            return dalFloodOperations.GetAllEmergencyPurchasesYear();
        }

        public CO_StructureType GetStructureInformationByEmergencyPurchaseID(long? _EmergencyPurchaseID)
        {
            return dalFloodOperations.GetStructureInformationByEmergencyPurchaseID(_EmergencyPurchaseID);
        }

        public IEnumerable<DataRow> GetEmergencyPurchasesSearch(string _InfrastructureType, long? _EmergencyPurchaseID, long? _ZoneID, long? _CircleID, long? _DivisionID, int? _PurchaseYear, bool? _IsCompSite)
        {
            return dalFloodOperations.GetEmergencyPurchasesSearch(_InfrastructureType, _EmergencyPurchaseID, _ZoneID, _CircleID, _DivisionID, _PurchaseYear, _IsCompSite);
        }

        public List<object> SearchEmergencyPurchase(String _InfrastructureType, long? _EmergencyPurchasesID, long? _ZoneID, long? CircleID, long? _DivisionID, long? _PurchasesYear, long? _CompSite, int _UserID)
        {
            return dalFloodOperations.SearchEmergencyPurchase(_InfrastructureType, _EmergencyPurchasesID, _ZoneID, CircleID, _DivisionID, _PurchasesYear, _CompSite, _UserID);
        }

        public bool IsEmergencyPurchaseDependencyExists(long _EmergencyPurchaseID)
        {
            return dalFloodOperations.IsEmergencyPurchaseDependencyExists(_EmergencyPurchaseID);
        }

        public bool DeleteJointEmergencyPurchase(long _EmergencyPurchaseID)
        {
            return dalFloodOperations.DeleteJointEmergencyPurchase(_EmergencyPurchaseID);
        }

        public FO_EmergencyPurchase GetEmergencyPurchaseByID(long _ID)
        {
            return dalFloodOperations.GetEmergencyPurchaseByID(_ID);
        }

        public bool IsEmergencyPurchasesExist(FO_EmergencyPurchase ObjEmergencyPurchase)
        {
            return dalFloodOperations.IsEmergencyPurchasesExist(ObjEmergencyPurchase);
        }

        public object GetInfrastructureTypeByEmergencyPurchaseID(long _EmergencyPurchaseID)
        {
            return dalFloodOperations.GetInfrastructureTypeByEmergencyPurchaseID(_EmergencyPurchaseID);
        }

        public bool SaveFloodFightingWorks(FO_EPWork _Obj)
        {
            return dalFloodOperations.SaveFloodFightingWorks(_Obj);
        }

        public long SaveFloodFightingWorks(long UserID, long _WorkID, long _EmergencyID, long? _NatureOfWorkID, int? _RD, string _Descp)
        {
            return dalFloodOperations.SaveFloodFightingWorks(UserID, _WorkID, _EmergencyID, _NatureOfWorkID, _RD, _Descp);
        }

        public int Get_EP_Item_PreviousQuantity(FO_EPItem _FloodFighting)
        {
            return dalFloodOperations.Get_EP_Item_PreviousQuantity(_FloodFighting);
        }

        public int Get_EP_Item_PreviousQuantity(long _EPItemID)
        {
            return dalFloodOperations.Get_EP_Item_PreviousQuantity(_EPItemID);
        }

        public bool SaveFo_EP_Item(FO_EPItem _Obj)
        {
            return dalFloodOperations.SaveFo_EP_Item(_Obj);
        }

        public bool SaveDisposalEmergency_Purchase(FO_MaterialDisposal _Obj)
        {
            return dalFloodOperations.SaveDisposalEmergency_Purchase(_Obj);
        }

        public bool SaveDM_Attachment(FO_MaterialDisposalAttachment _Obj)
        {
            return dalFloodOperations.SaveDM_Attachment(_Obj);
        }

        public object GetFo_EmergencyPurchase_ID(long EmergencyPurchaseID)
        {
            return dalFloodOperations.GetFo_EmergencyPurchase_ID(EmergencyPurchaseID);
        }

        public List<object> GetFo_EPWorkBy_ID(int EmergencyPurchaseID)
        {
            return dalFloodOperations.GetFloodFightingWorksByID(EmergencyPurchaseID);
        }

        public object GetFo_EPWorkByObject_ID(long EmergencyPurchaseID)
        {
            return dalFloodOperations.GetFo_EPWorkByObject_ID(EmergencyPurchaseID);
        }

        public List<object> GetF_EmergencyDisposal_Attachment_ID(long MaterialDisposalID)
        {
            return dalFloodOperations.GetF_EmergencyDisposal_Attachment_ID(MaterialDisposalID);
        }

        public object GetFloodFightingDivision_CampSite_By_ID(long EmergencyPurchaseID)
        {
            return dalFloodOperations.GetFloodFightingDivision_CampSite_By_ID(EmergencyPurchaseID);
        }

        public object GetIsexistFo_EPItem_ID(long EmergencyPurchaseID, int itemid)
        {
            return dalFloodOperations.GetIsexistFo_EPItem_ID(EmergencyPurchaseID, itemid);
        }

        public IEnumerable<DataRow> GetFO_EMItemsPurchasingQty(long EmergencyPurchaseID, long Categoryid)
        {
            return dalFloodOperations.GetFO_EMItemsPurchasingQty(EmergencyPurchaseID, Categoryid);
        }

        public object GetItemPurchasing_EmergcnyP_ID(long EmergencyPurchaseID)
        {
            return dalFloodOperations.GetItemPurchasing_EmergcnyP_ID(EmergencyPurchaseID);
        }

        public object GetEmergncyPurches_ItemQty_ID(long EmergencyPurchaseID, long itemid)
        {
            return dalFloodOperations.GetEmergncyPurches_ItemQty_ID(EmergencyPurchaseID, itemid);
        }

        public object GetFloodFightingInsfrastructureName(int StructureTypeID, int structid)
        {
            return dalFloodOperations.GetFloodFightingInsfrastructureName(StructureTypeID, structid);
        }

        //
        public object GetItemPurchas_Type_InsfrastructureName(int StructureTypeID, int structid)
        {
            return dalFloodOperations.GetItemPurchas_Type_InsfrastructureName(StructureTypeID, structid);
        }

        public object GetMaterialDisposal_Header_By_ID(long EPworkID)
        {
            return dalFloodOperations.GetMaterialDisposal_Header_By_ID(EPworkID);
        }

        public object GetF_MaterialDisposal_Attachement_Header_By_ID(long MaterialDisposalID)
        {
            return dalFloodOperations.GetF_MaterialDisposal_Attachement_Header_By_ID(MaterialDisposalID);
        }

        public List<object> GetF_EmergencyDisposalByID(long EPWorkID)
        {
            return dalFloodOperations.GetF_EmergencyDisposalByID(EPWorkID);
        }

        public object GetF_EmergencyDisposalObjectByID(long DisposalID)
        {
            return dalFloodOperations.GetF_EmergencyDisposalObjectByID(DisposalID);
        }

        public bool DeleteFo_EPWork(long _ID)
        {
            return dalFloodOperations.DeleteFo_EPWork(_ID);
        }

        public bool DeleteFo_MaterialDisposal(long _ID)
        {
            return dalFloodOperations.DeleteFo_MaterialDisposal(_ID);
        }

        public bool DeleteFo_MaterialDisposal_Attachement(long _ID)
        {
            return dalFloodOperations.DeleteFo_MaterialDisposal_Attachement(_ID);
        }

        public bool DeleteFo_MaterialDisposal_Attachement_List(long _ID)
        {
            return dalFloodOperations.DeleteFo_MaterialDisposal_Attachement_List(_ID);
        }

        public List<FO_NatureOfWork> GetAllActiveFO_natureofWork()
        {
            return dalFloodOperations.GetAllActiveFO_natureofWork();
        }

        public bool IsFo_EpworkIDExists(long _ID)
        {
            return dalFloodOperations.IsFo_EpworkIDExists(_ID);
        }

        public bool IsFo_MaterialDisposalIDExists(long _ID)
        {
            return dalFloodOperations.IsFo_MaterialDisposalIDExists(_ID);
        }

        public object GetEmergency_DivisionStructtypeID(long EmergencyPurchaseID)
        {
            return dalFloodOperations.GetEmergency_DivisionStructtypeID(EmergencyPurchaseID);
        }

        public long AddDisposalEmergencyPurchase(int _DisposalID, int _EPWorkID, string _VehicleNumber, string _BuiltyNumber, int? _Quantity, long? _Cost, string _Unit, List<string> lstNameofFiles, int _UserID)
        {
            return dalFloodOperations.AddDisposalEmergencyPurchase(_DisposalID, _EPWorkID, _VehicleNumber, _BuiltyNumber, _Quantity, _Cost, _Unit, lstNameofFiles, _UserID);
        }

        public object SearchEmergencyPurchaseByID(long _InfrastructureTypeID, long _EmergencyPurchasesID)
        {
            return dalFloodOperations.SearchEmergencyPurchaseByID(_InfrastructureTypeID, _EmergencyPurchasesID);
        }

        public List<object> GetFo_EP_NatureofWork()
        {
            return dalFloodOperations.GetFo_EP_NatureofWork();
        }

        #endregion Emergency Purchases

        #region RoleRights

        public Int16 CanAddFloodInspections()
        {
            return dalFloodOperations.CanAddFloodInspections();
        }

        public bool CanAddEditFloodInspections(int _Year, long _DesignationID, int _InspectionStatus, int InspectionTypeID)
        {
            return dalFloodOperations.CanAddEditFloodInspections(_Year, _DesignationID, _InspectionStatus, InspectionTypeID);
        }

        public bool CanAddEditStoneDeployment(int _Year, long _DesignationID)
        {
            return dalFloodOperations.CanAddEditStoneDeployment(_Year, _DesignationID);
        }

        public bool CanAddEditEmergencyPurchase(int _Year, long _DesignationID)
        {
            return dalFloodOperations.CanAddEditEmergencyPurchase(_Year, _DesignationID);
        }

        public bool CanAddEditOnSiteMonitoring(int _Year, long _DesignationID)
        {
            return dalFloodOperations.CanAddEditOnSiteMonitoring(_Year, _DesignationID);
        }

        #endregion RoleRights

        #region Reference Data

        public List<object> GetProblemNatureList()
        {
            return dalFloodOperations.GetProblemNatureList();
        }

        public bool IsProblemNatureExists(FO_ProblemNature _ProblemNature)
        {
            return dalFloodOperations.IsProblemNatureExists(_ProblemNature);
        }

        public bool SaveProblemNature(FO_ProblemNature _ProblemNature)
        {
            return dalFloodOperations.SaveProblemNature(_ProblemNature);
        }

        public bool DeleteProblemNature(long _ID)
        {
            return dalFloodOperations.DeleteProblemNature(_ID);
        }

        public bool IsProblemNatureIDExists(long _ProblemNatureID)
        {
            return dalFloodOperations.IsProblemNatureIDExists(_ProblemNatureID);
        }

        #region FloodBund

        public FO_StructureNalaHillTorant GetStructureNalaHillTorantByID(long _StructureNalaHillToranID)
        {
            return dalFloodOperations.GetStructureNalaHillTorantByID(_StructureNalaHillToranID);
        }

        public long StructureNallahHillTorantInsertion(long _ID, string _StructureType, string _StructureName, long? _DivisionID, long? _DistrictID, long? _TehsilID, long? _VillageID, double _DesignedDischarge, bool _Status, int _CreatedBy, int _ModifiedBy, long _DSID)
        {
            return dalFloodOperations.StructureNallahHillTorantInsertion(_ID, _StructureType, _StructureName, _DivisionID, _DistrictID, _TehsilID, _VillageID, _DesignedDischarge, _Status, _CreatedBy, _ModifiedBy, _DSID);
        }

        public IEnumerable<DataRow> GetFloodBundRefSearch(long? _StructureNalaHillTorantID, long? _ZoneID, long? _CircleID, long? _DivisionID, string _StructureType, string _InfrastructureName)
        {
            return dalFloodOperations.GetFloodBundRefSearch(_StructureNalaHillTorantID, _ZoneID, _CircleID, _DivisionID, _StructureType, _InfrastructureName);
        }

        public List<object> GetStructureNameFloodBund(long _UserId, string _StructureType)
        {
            return dalFloodOperations.GetStructureNameFloodBund(_UserId, _StructureType);
        }

        public List<string> GetTimeListFloodBund()
        {
            return dalFloodOperations.GetTimeListFloodBund();
        }

        public IEnumerable<DataRow> GetFloodBundAddGauges(string _StructureType, string _StructureName, string _Time, DateTime? _Date)
        {
            return dalFloodOperations.GetFloodBundAddGauges(_StructureType, _StructureName, _Time, _Date);
        }

        public bool SaveFloodGaugeReading(UA_Users mdlUser, int ModifiedBy, List<Tuple<string, string, string, string, string, string, string>> _listHillTorrent)
        {
            return dalFloodOperations.SaveFloodGaugeReading(mdlUser, ModifiedBy, _listHillTorrent);
        }

        public bool SaveFloodGaugeReadingBund(UA_Users mdlUser, int ModifiedBy, List<Tuple<string, string, string, string, string, string>> lstGaugeBund)
        {
            return dalFloodOperations.SaveFloodGaugeReadingBund(mdlUser, ModifiedBy, lstGaugeBund);
        }

        public List<object> GetAllFloodGaugesRDs(long _StructureID)
        {
            return dalFloodOperations.GetAllFloodGaugesRDs(_StructureID);
        }

        public IEnumerable<DataRow> GetBundGaugesDataSearch(long? _DivisionID, string _StructureType, string _StructureName, long? GaugeRD, string _Time, DateTime? _FromDate, DateTime? _ToDate)
        {
            return dalFloodOperations.GetBundGaugesDataSearch(_DivisionID, _StructureType, _StructureName, GaugeRD, _Time, _FromDate, _ToDate);
        }

        public CO_Circle GetCirlceByDivisionID(long _DivisionID)
        {
            return dalFloodOperations.GetCirlceByDivisionID(_DivisionID);
        }

        public CO_Zone GetZoneByCirlceID(long _CircleID)
        {
            return dalFloodOperations.GetZoneByCirlceID(_CircleID);
        }

        public bool IsStructureNalaHillTorantExists(long _ID, long _DivisionID, string _StructureType, string _StructureName)
        {
            return dalFloodOperations.IsStructureNalaHillTorantExists(_ID, _DivisionID, _StructureType, _StructureName);
        }

        #endregion FloodBund

        #endregion Reference Data

        #region Notification

        /// <summary>
        /// </summary>
        /// <param name="_ID">This ID may be FFPID, FloodInspectionID or Emergency ID</param>
        /// <param name="_UserID"></param>
        /// <param name="_EventID"></param>
        /// <returns></returns>
        public bool SendNotifiactions(long _ID, long _UserID, long _EventID)
        {
            NotifyEvent _event = new NotifyEvent();
            _event.Parameters.Add("ID", _ID);
            return _event.AddNotifyEvent(_EventID, _UserID);
        }

        #endregion Notification


    }
}