using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.AssetsAndWorks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.AssetsAndWorks
{
    public class AssetsWorkBLL : BaseBLL
    {
        private AssetsWorkDAL dalAW = new AssetsWorkDAL();

        #region Notifications and Alerts

        public AM_GetAWAssetAssociationNotifyData_Result GetAW_Association_NotifyData(long _AWID)
        {
            return dalAW.GetAW_Association_NotifyData(_AWID);
        }
        public AM_GetAWProgressNotifyData_Result GetAW_Progress_NotifyData(long _ProgressID)
        {
            return dalAW.GetAW_Progress_NotifyData(_ProgressID);
        }
        public AM_GetAWPublishNotifyData_Result1 GetAW_Publish_NotifyData(long _AWID)
        {
            return dalAW.GetAW_Publish_NotifyData(_AWID);
        }
        public bool SendNotifiaction(long _ID, long _UserID, long _EventID)
        {
            NotifyEvent _event = new NotifyEvent();
            _event.Parameters.Add("ID", _ID);
            return _event.AddNotifyEvent(_EventID, _UserID);

        }
        public bool SendNotifiactionAssociation(long _ID, long _UserID, long _EventID)
        {
            NotifyEvent _event = new NotifyEvent();
            _event.Parameters.Add("ID", _ID);
            return _event.AddNotifyEvent(_EventID, _UserID);

        }
        #endregion

        #region Refrence Data
        public List<object> GetAssetCategoryList()
        {
            return dalAW.GetAssetCategoryList();
        }
        public List<object> GetAssetAllCategoryList()
        {
            return dalAW.GetAssetAllCategoryList();
        }
        public bool IsAssetCategoryNameExists(AM_Category _AssetCategory)
        {
            return dalAW.IsAssetCategoryNameExists(_AssetCategory);
        }
        public bool SaveAssetCategory(AM_Category _AssetCategory)
        {
            return dalAW.SaveAssetCategory(_AssetCategory);
        }
        public bool DeleteAssetCategory(long _ID)
        {
            return dalAW.DeleteAssetCategory(_ID);
        }
        public bool IsAssetCategoryIDExists(long _CategoryID)
        {

            return dalAW.IsAssetCategoryIDExists(_CategoryID);
        }
        public bool SubCategoryIDExistsAsset(long _SubCategoryID)
        {
            return dalAW.SubCategoryIDExistsAsset(_SubCategoryID);
        }
        public List<object> GetAssetSubCategoryList(long _CategoryID)
        {
            return dalAW.GetAssetSubCategoryList(_CategoryID);
        }
        public List<object> GetAssetAttributeList(long _SubCatID)
        {
            return dalAW.GetAssetAttributeList(_SubCatID);
        }
        public List<object> GetAssetAllAttributeList(long _SubCatID)
        {
            return dalAW.GetAssetAllAttributeList(_SubCatID);
        }
        public bool IsAssetAttributeNameExists(AM_Attributes _AssetAttributes)
        {
            return dalAW.IsAssetAttributeNameExists(_AssetAttributes);
        }
        public bool SaveAssetAttribute(AM_Attributes _AssetAttributes)
        {
            return dalAW.SaveAssetAttribute(_AssetAttributes);
        }
        public bool DeleteAssetAttribute(long _ID)
        {
            return dalAW.DeleteAssetAttribute(_ID);
        }
        public bool IsAssetAttributeIDExists(long _AssetAttributeID)
        {

            return dalAW.IsAssetAttributeIDExists(_AssetAttributeID);
        }
        public object GetCategorySubCategoryByID(long _SubCatID)
        {
            return dalAW.GetCategorySubCategoryByID(_SubCatID);
        }
        public List<object> GetAssetOfficeList()
        {
            return dalAW.GetAssetOfficeList();
        }
        public List<object> GetAssetAllOfficeList()
        {
            return dalAW.GetAssetAllOfficeList();
        }
        public List<object> GetAssetConditionList()
        {
            return dalAW.GetAssetConditionList();
        }
        public bool SaveAssetOffice(AM_Offices _AssetOffice)
        {
            return dalAW.SaveAssetOffice(_AssetOffice);
        }
        public bool IsAssetOfficeNameExists(AM_Offices _AssetOffice)
        {
            return dalAW.IsAssetOfficeNameExists(_AssetOffice);
        }
        public bool IsAssetOfficeIDExists(long _OfficeID)
        {

            return dalAW.IsAssetOfficeIDExists(_OfficeID);
        }
        public bool DeleteAssetOffice(long _ID)
        {
            return dalAW.DeleteAssetOffice(_ID);
        }
        public List<AM_AssetWorkType> GetAllAssetWorkType()
        {
            return dalAW.GetAllAssetWorkType();
        }
        public bool AssetWorkTypeAssiciatExists(long _ID)
        {
            return dalAW.AssetWorkTypeAssiciatExists(_ID);
        }
        public object AssetWorkType_Operations(int _OperationType, Dictionary<string, object> _Data)
        {
            object status = false;
            switch (_OperationType)
            {
                case Constants.AssetCRUD_CREATE:
                    AM_AssetWorkType mdlNew = new AM_AssetWorkType();
                    mdlNew.Name = _Data["Name"].ToString();
                    mdlNew.Description = _Data["Description"].ToString();
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdlNew.IsActive = (bool)_Data["IsActive"];
                    dalAW.AddAssetWrkType(mdlNew);
                    status = true;
                    break;

                case Constants.AssetCRUD_READ:
                    if (_Data.ContainsKey("Name"))
                    {
                        status = dalAW.GetAssetWrkType_ByName(_Data["Name"].ToString());
                    }
                    break;

                case Constants.AssetCRUD_UPDATE:
                    AM_AssetWorkType mdl = new AM_AssetWorkType();
                    mdl.ID = Convert.ToInt32(_Data["ID"]);
                    mdl.Name = _Data["Name"].ToString();
                    mdl.Description = _Data["Description"].ToString();
                    mdl.ModifiedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdl.IsActive = (bool)_Data["IsActive"];
                    dalAW.UpdateAssetWrkType(mdl);
                    status = true;
                    break;

                case Constants.AssetCRUD_DELETE:
                    dalAW.DeleteAssetWrkType(Convert.ToInt64(_Data["ID"]));
                    status = true;
                    break;

                default:
                    break;
            }

            return status;
        }


        #endregion


        #region Sub-Category
        public object SubCategory_Operations(int _OperationType, Dictionary<string, object> _Data)
        {

            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE:
                    AM_SubCategory mdlNew = new AM_SubCategory();
                    mdlNew.CategoryID = Convert.ToInt32(_Data["CategoryID"]);
                    mdlNew.Name = Convert.ToString(_Data["Name"]);
                    mdlNew.Description = _Data["Description"].ToString();
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdlNew.CreatedDate = DateTime.Now;
                    //mdlNew.ACPID = Convert.ToInt64(_Data["ACCPID"]);
                    //mdlNew.ID = Convert.ToInt64(_Data["ID"]);
                    mdlNew.FOBit = Convert.ToBoolean(_Data["FOBit"]);
                    mdlNew.IsActive = Convert.ToBoolean(_Data["IsActive"]);
                    dalAW.SaveAssetSubCategory(mdlNew);
                    status = true;
                    break;
                case Constants.CRUD_READ:
                    if (_Data.ContainsKey("ID"))
                    {
                        //status = dalCO.GetACCPOrder_ByID(Convert.ToInt64(_Data["ID"].ToString()));
                        return status;
                    }
                    break;
                case Constants.CRUD_UPDATE:
                    AM_SubCategory mdl = new AM_SubCategory();
                    mdl.ID = Convert.ToInt32(_Data["ID"]);
                    mdl.Name = Convert.ToString(_Data["Name"]);
                    mdl.Description = _Data["Description"].ToString();
                    mdl.FOBit = Convert.ToBoolean(_Data["FOBit"]);
                    mdl.IsActive = Convert.ToBoolean(_Data["IsActive"]);
                    mdl.ModifiedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdl.ModifiedDate = DateTime.Now;
                    dalAW.SaveAssetSubCategory(mdl);
                    status = true;
                    break;
                case Constants.CRUD_DELETE:
                    AM_SubCategory mdld = new AM_SubCategory();
                    mdld.ID = Convert.ToInt32(_Data["ID"].ToString());
                    dalAW.DeleteAssetSubCategory(mdld.ID);
                    status = true;
                    break;
                default:
                    break;
            }

            return status;
        }

        public bool IsAssetSubCategoryNameExists(AM_SubCategory _AssetCategory)
        {
            return dalAW.IsAssetSubCategoryNameExists(_AssetCategory);
        }

        public List<object> GetSubCategoriesByCategoryID(int _CategoryID)
        {
            return dalAW.GetSubCategoriesByCategoryID(_CategoryID);
        }
        public List<object> GetAllSubCategoriesByCategoryID(int _CategoryID)
        {
            return dalAW.GetAllSubCategoriesByCategoryID(_CategoryID);
        }
        #endregion
        #region "Headquarter Division"
        public List<object> GetDivisionzByZoneId(int _ZoneID)
        {
            return dalAW.GetDivisionzByZoneId(_ZoneID);
        }
        public object GetHeadQDivisionByID(long _ZoneID)
        {
            return dalAW.GetHeadQDivisionByID(_ZoneID);
        }
        public bool SaveHQDivision(AM_HeadquarterDivision _HeadquarterDivision)
        {
            return dalAW.SaveHQDivision(_HeadquarterDivision);
        }
        public bool DeleteHQDivision(long _HQDivisionID)
        {
            return dalAW.DeleteHQDivision(_HQDivisionID);
        }
        #endregion
        #region "Assets"
        public IEnumerable<DataRow> GetAssetsSearch(string _Level, long? _AssetsID, long? _IrrigationLevelID, long? _IrrigationBoundryID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _CategoryID, long? _SubCategoryID, bool? _FloodAssociationID, string _AssetType, string _Status, string _AssetName)
        {
            return dalAW.GetAssetsSearch(_Level, _AssetsID, _IrrigationLevelID, _IrrigationBoundryID, _ZoneID, _CircleID, _DivisionID, _CategoryID, _SubCategoryID, _FloodAssociationID, _AssetType, _Status, _AssetName);
        }
        public bool IsHeadQuarterDivision(long? _IrrigationBoundryID)
        {
            return dalAW.IsHeadQuarterDivision(_IrrigationBoundryID);
        }
        public List<UA_IrrigationLevel> GetIrrigationLevelByDesignation(long DesignationID, bool isHeadQuarter)
        {
            return dalAW.GetIrrigationLevelByDesignation(DesignationID, isHeadQuarter);
        }
        public List<UA_IrrigationLevel> GetViewIrrigationLevelByDesignation(long DesignationID)
        {
            return dalAW.GetViewIrrigationLevelByDesignation(DesignationID);
        }
        public List<object> GetIrrigationLevel()
        {
            return dalAW.GetIrrigationLevel();
        }
        public List<object> GetAssetNameBySubCategoryAndLevelID(long subCategoryID, long irrigationLevelID, long AssetWorkID)
        {
            return dalAW.GetAssetNameBySubCategoryAndLevelID(subCategoryID, irrigationLevelID, AssetWorkID);
        }
        public bool DeleteAssetByID(long _AssetID)
        {
            return dalAW.DeleteAssetByID(_AssetID);
        }
        public AM_AssetItems GetAssetID(long _ID)
        {
            return dalAW.GetAssetID(_ID);
        }
        public List<CO_Zone> GetAllZone()
        {
            return dalAW.GetAllZone();
        }
        public List<CO_Circle> GetAllCircle()
        {
            return dalAW.GetAllCircle();
        }
        public List<CO_Division> GetAllDivision()
        {
            return dalAW.GetAllDivision();
        }
        public List<CO_Circle> GetCirlceByDivisionID(long _DivisionID)
        {
            return dalAW.GetCirlceByDivisionID(_DivisionID);
        }
        public List<CO_Zone> GetZoneByCirlceID(long _CircleID)
        {
            return dalAW.GetZoneByCirlceID(_CircleID);
        }
        public IEnumerable<DataRow> GetAm_AssetAtrtributeDetail(long AssetsID, long SubCategoryid)
        {
            return dalAW.GetAm_AssetAtrtributeDetail(AssetsID, SubCategoryid);
        }
        public bool SaveAssets(AM_AssetItems _AssetItems)
        {
            return dalAW.SaveAssets(_AssetItems);
        }
        public bool SaveAssetsAttributeDetail(AM_AssetAttributes _AssetAttributes)
        {
            return dalAW.SaveAssetsAttributeDetail(_AssetAttributes);
        }
        public AM_SubCategory GetcatID(long _SubCategoryID)
        {
            return dalAW.GetcatID(_SubCategoryID);
        }
        public bool isSubCatgFlood(long _SubCategoryID)
        {
            return dalAW.isSubCatgFlood(_SubCategoryID);
        }
        public bool IsAssetsExist(AM_AssetItems ObjAssets)
        {
            return dalAW.IsAssetsExist(ObjAssets);
        }
        public bool DeleteAssetsAttribute(long _AssetsAttributeID)
        {
            return dalAW.DeleteAssetsAttribute(_AssetsAttributeID);
        }
        public object GeAssetInspectionInd(long _ID)
        {
            return dalAW.GeAssetInspectionInd(_ID);
        }
        public object GeAssetInspectionLot(long _ID)
        {
            return dalAW.GeAssetInspectionLot(_ID);
        }
        public List<object> GetAssetshistoryLotCondition(long _InspectionLotID)
        {
            return dalAW.GetAssetshistoryLotCondition(_InspectionLotID);
        }
        public bool SaveAssetInspectionInd(AM_AssetInspectionInd _Mdl, List<Tuple<string, string, string>> _Attachment)
        {
            return dalAW.SaveAssetInspectionInd(_Mdl, _Attachment);

        }
        public DataSet GetAssetsHeader(string _Level, long? _AssetsID, long? _IrrigationLevelID, long? _IrrigationBoundryID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _CategoryID, long? _SubCategoryID, bool? _FloodAssociationID, string _AssetType, string _Status, string _AssetName)
        {
            return dalAW.GetAssetsHeader(_Level, _AssetsID, _IrrigationLevelID, _IrrigationBoundryID, _ZoneID, _CircleID, _DivisionID, _CategoryID, _SubCategoryID, _FloodAssociationID, _AssetType, _Status, _AssetName);
        }
        public long AssetStatusLotUpdation(long _AssetID, bool _AssetStatus, int _AvailableQty, int _Quantity)
        {
            return dalAW.AssetStatusLotUpdation(_AssetID, _AssetStatus, _AvailableQty, _Quantity);
        }
        public long AssetStatusIndividualUpdation(long _AssetID, bool _AssetStatus)
        {
            return dalAW.AssetStatusIndividualUpdation(_AssetID, _AssetStatus);
        }
        public long AssetWorkDetailDelete(long _AssetWorkID)
        {
            return dalAW.AssetWorkDetailDelete(_AssetWorkID);
        }
        public bool SaveAssetInspectionLot(AM_AssetInspectionLot _Mdl, List<Tuple<string, string, string>> _Attachment, List<Tuple<string, string, string>> _Condition)
        {
            return dalAW.SaveAssetInspectionLot(_Mdl, _Attachment, _Condition);

        }
        public string GetDesignationByID(long _DesiganationID)
        {
            return dalAW.GetDesignationByID(_DesiganationID);
        }
        public List<object> GetIndividualHistoryInd(string _StatusID, List<long> _InspectedBy, long _AssetsID, DateTime? _FromDate, DateTime? _ToDate)
        {
            return dalAW.GetIndividualHistoryInd(_StatusID, _InspectedBy, _AssetsID, _FromDate, _ToDate);
        }
        public List<object> GetIndividualHistoryLot(List<long> _InspectedBy, long _AssetsID, DateTime? _FromDate, DateTime? _ToDate)
        {
            return dalAW.GetIndividualHistoryLot(_InspectedBy, _AssetsID, _FromDate, _ToDate);
        }
        public bool IsAssetInspectionExists(long _AssetsID)
        {
            return dalAW.IsAssetInspectionExists(_AssetsID);
        }

        #endregion


        #region "Work"

        public List<object> GetSmallDamByDivisionID(long _DivisionID)
        {
            return dalAW.GetSmallDamByDivisionID(_DivisionID);
        }
        public List<object> GetSmallDamChannelByDivisionID(long _DivisionID)
        {
            return dalAW.GetSmallDamChannelByDivisionID(_DivisionID);
        }
        public List<object> GetInfrastructureName(long _UserId, long _InfrastructureType)
        {
            return dalAW.GetInfrastructureName(_UserId, _InfrastructureType);
        }
        public object GetStructureTypeIDByInsfrastructureValue(long _InfrastructureType, long _InfrastructureNamevalue)
        {
            return dalAW.GetStructureTypeIDByInsfrastructureValue(_InfrastructureType, _InfrastructureNamevalue);
        }
        public object GetLastWorkProgressID(long AssetWorkID, long userid)
        {
            return dalAW.GetLastWorkProgressID(AssetWorkID, userid);
        }
        public object GetAssetWorkByID(long _workID)
        {
            return dalAW.GetAssetWorkByID(_workID);
        }
        public List<object> GetAssetWorkDetailByWorkID(long _WorkID)
        {
            return dalAW.GetAssetWorkDetailByWorkID(_WorkID);
        }
        public List<object> GetFloodWorkDetailByWorkID(long? structuretype, long _WorkID)
        {
            return dalAW.GetFloodWorkDetailByWorkID(structuretype, _WorkID);
        }
        public List<object> GetInfraWorkDetailByWorkID(long _WorkID)
        {
            return dalAW.GetInfraWorkDetailByWorkID(_WorkID);
        }
        public List<object> GetIrrigationLevel(long XENID, bool IsHeadQuarterDivision)
        {
            return dalAW.GetIrrigationLevel(XENID, IsHeadQuarterDivision);
        }
        public bool DeleteWorkByID(long _AssetWorkID)
        {
            return dalAW.DeleteWorkByID(_AssetWorkID);
        }
        public bool IsAWInTender(long _AWPID)
        {
            return dalAW.IsAWInTender(_AWPID);
        }

        public string IsAssetWorkPlanCostEstimationCorrect(long _AWID)
        {
            string result = "";
            List<AM_AssetWork> lst = db.Repository<AM_AssetWork>().Query().Get().Where(x => x.ID == _AWID).ToList();
            if (lst != null || lst.Count() > 0)
            {
                foreach (var item in lst)
                {
                    bool status = dalAW.IsAssetWorkCostEstimationCorrect(item.ID);
                    if (!status)
                    {
                        result = item.WorkName;
                        break;
                    }
                }
            }
            return result;
        }
        public object GetContractorNameWithAmountByWorkID(long _WorkID)
        {
            try
            {
                return dalAW.GetContractorNameWithAmountByWorkID(_WorkID);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public double? ContractorAmountPerWorkItem(long _ID)
        {
            return dalAW.ContractorAmountPerWorkItem(_ID);
        }
        public bool IsWorkUnique(Dictionary<string, object> _Data)
        {
            AM_AssetWork mdlNew = new AM_AssetWork();
            mdlNew.ID = Convert.ToInt64(_Data["ID"]);
            mdlNew.FinancialYear = Convert.ToString(_Data["FinancialYear"]);
            mdlNew.DivisionID = Convert.ToInt64(_Data["DivisionID"]);
            mdlNew.WorkName = Convert.ToString(_Data["WorkName"]);
            mdlNew.AssetWorkTypeID = Convert.ToInt64(_Data["AssetWorkTypeID"]);
            return dalAW.IsWorkUnique(mdlNew);
        }
        public List<object> GetWorkbySearchCriteria(Dictionary<string, object> _Data)
        {
            try
            {
                long ZoneID = Convert.ToInt64(_Data["ZoneID"]);
                long CircleID = Convert.ToInt64(_Data["CircleID"]);
                long DivisionID = Convert.ToInt64(_Data["DivisionID"]);
                string FinancialYear = Convert.ToString(_Data["FinancialYear"]);
                long WorkType = Convert.ToInt64(_Data["WorkType"]);
                string WorkStatus = Convert.ToString(_Data["WorkStatus"]);
                long ProgressStatusID = Convert.ToInt64(_Data["ProgressStatus"]);
                string WorkName = Convert.ToString(_Data["WorkName"].ToString());
                return dalAW.GetWorkbySearchCriteria(ZoneID, CircleID, DivisionID, FinancialYear, WorkType, WorkStatus, ProgressStatusID, WorkName);
            }
            catch (Exception ex)
            {
                throw;
                // new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public List<object> GetWorkType(bool isActive = true)
        {

            return dalAW.GetWorkType(isActive);
        }
        public List<object> GetProgressStatus()
        {
            return dalAW.GetProgressStatus();
        }
        public bool UpdateWorkStatus(long _ID, long _UserID)
        {
            bool result = dalAW.UpdateWorkStatus(_ID, _UserID);
            if (result)
            {
                bool notify = SendNotifiaction(_ID, _UserID, (long)NotificationEventConstants.AssetWorks.PublishAssetWorkPlan);
            }
            return result;
        }
        //
        public bool UpdateWorkStatusDraft(long _ID)
        {
            return dalAW.UpdateWorkStatusDraft(_ID);
        }
        public List<object> GetFinancialYear()
        {
            return dalAW.GetFinancialYear();
        }
        public object GetWorkByID(long _workID)
        {
            return dalAW.GetWorkByID(_workID);
        }
        public bool isWorkItemExist(long _WorkID)
        {
            return dalAW.isWorkItemExist(_WorkID);
        }
        public List<CO_Division> GetByID(long _Division)
        {
            return dalAW.GetByID(_Division);
        }


        public List<AM_AssetWorkDetail> GetListAM_WorkDetail(List<object> lstAM_WorkDetail, int CreatedBy, bool isUpdate = false)
        {
            List<AM_AssetWorkDetail> lstWorkDetail = new List<Model.AM_AssetWorkDetail>();
            if (lstAM_WorkDetail.Count > 0)
            {
                foreach (object item in lstAM_WorkDetail)
                {
                    AM_AssetWorkDetail assetWorkDetail = new AM_AssetWorkDetail();
                    assetWorkDetail.ID = Convert.ToInt64((item.GetType().GetProperty("ID").GetValue(item)).ToString() == string.Empty ? "0" : (item.GetType().GetProperty("ID").GetValue(item)));
                    assetWorkDetail.AssetItemsID = Convert.ToInt64(item.GetType().GetProperty("AssetItemsID").GetValue(item));
                    if (isUpdate && assetWorkDetail.ID > 0)
                    {
                        assetWorkDetail.ModifiedDate = DateTime.Now;
                        assetWorkDetail.ModifiedBy = CreatedBy;
                    }
                    else
                    {
                        assetWorkDetail.CreatedDate = DateTime.Now;
                        assetWorkDetail.CreatedBy = CreatedBy;
                    }
                    lstWorkDetail.Add(assetWorkDetail);
                }
            }
            return lstWorkDetail;

        }
        public List<AM_AssetWorkDetail> GetListFlood_WorkDetail(List<object> lstFlood_WorkDetail, int CreatedBy, bool isUpdate = false)
        {
            List<AM_AssetWorkDetail> lstWorkDetail = new List<Model.AM_AssetWorkDetail>();
            if (lstFlood_WorkDetail.Count > 0)
            {
                foreach (object item in lstFlood_WorkDetail)
                {
                    AM_AssetWorkDetail assetWorkDetail = new AM_AssetWorkDetail();
                    assetWorkDetail.ID = Convert.ToInt64((item.GetType().GetProperty("ID").GetValue(item)).ToString() == string.Empty ? "0" : (item.GetType().GetProperty("ID").GetValue(item)));
                    assetWorkDetail.StructureTypeID = Convert.ToInt64(item.GetType().GetProperty("StructureTypeID").GetValue(item));
                    assetWorkDetail.StructureID = Convert.ToInt64(item.GetType().GetProperty("StructureID").GetValue(item));
                    if (isUpdate && assetWorkDetail.ID > 0)
                    {
                        assetWorkDetail.ModifiedDate = DateTime.Now;
                        assetWorkDetail.ModifiedBy = CreatedBy;
                    }
                    else
                    {
                        assetWorkDetail.CreatedDate = DateTime.Now;
                        assetWorkDetail.CreatedBy = CreatedBy;
                    }
                    lstWorkDetail.Add(assetWorkDetail);
                }
            }
            return lstWorkDetail;

        }
        public List<AM_AssetWorkDetail> GetListInfra_WorkDetail(List<object> lstInfra_WorkDetail, int CreatedBy, bool isUpdate = false)
        {
            List<AM_AssetWorkDetail> lstWorkDetail = new List<Model.AM_AssetWorkDetail>();
            if (lstInfra_WorkDetail.Count > 0)
            {
                foreach (object item in lstInfra_WorkDetail)
                {
                    AM_AssetWorkDetail assetWorkDetail = new AM_AssetWorkDetail();
                    assetWorkDetail.ID = Convert.ToInt64((item.GetType().GetProperty("ID").GetValue(item)).ToString() == string.Empty ? "0" : (item.GetType().GetProperty("ID").GetValue(item)));
                    assetWorkDetail.StructureTypeID = Convert.ToInt64(item.GetType().GetProperty("StructureTypeID").GetValue(item));
                    assetWorkDetail.StructureID = Convert.ToInt64(item.GetType().GetProperty("StructureID").GetValue(item));
                    if (isUpdate && assetWorkDetail.ID > 0)
                    {
                        assetWorkDetail.ModifiedDate = DateTime.Now;
                        assetWorkDetail.ModifiedBy = CreatedBy;
                    }
                    else
                    {
                        assetWorkDetail.CreatedDate = DateTime.Now;
                        assetWorkDetail.CreatedBy = CreatedBy;
                    }
                    lstWorkDetail.Add(assetWorkDetail);
                }
            }
            return lstWorkDetail;

        }
        public object Work_Operations(int _OperationType, Dictionary<string, object> _Data)
        {

            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE:
                    AM_AssetWork mdlNew = new AM_AssetWork();

                    mdlNew.FinancialYear = Convert.ToString(_Data["FinancialYear"]);
                    mdlNew.DivisionID = Convert.ToInt64(_Data["DivisionID"]);
                    mdlNew.WorkName = Convert.ToString(_Data["WorkName"]);
                    mdlNew.FundingSourceID = Convert.ToInt32(_Data["FundingSource"]);
                    mdlNew.AssetWorkTypeID = Convert.ToInt64(_Data["AssetWorkTypeID"]);
                    mdlNew.AssetType = Convert.ToString(_Data["AssetType"]);
                    mdlNew.EstimatedCost = Convert.ToInt64(_Data["EstimatedCost"]);
                    mdlNew.CompletionPeriodFlag = Convert.ToBoolean(_Data["CompletionPeriodFlag"]);
                    mdlNew.CompletionPeriod = Convert.ToInt32(_Data["CompletionPeriod"]);
                    mdlNew.CompletionPeriodUnit = Convert.ToString(_Data["CompletionPeriodUnit"]);
                    if (Convert.ToDateTime(_Data["StartDate"]).Year != 0001)
                        mdlNew.StartDate = Convert.ToDateTime(_Data["StartDate"]);
                    if (Convert.ToDateTime(_Data["EndDate"]).Year != 0001)
                        mdlNew.EndDate = Convert.ToDateTime(_Data["EndDate"]);
                    mdlNew.SanctionNo = Convert.ToString(_Data["SanctionNo"]);
                    mdlNew.SanctionDate = Convert.ToDateTime(_Data["SanctionDate"]);
                    mdlNew.EarnestMoneyType = Convert.ToString(_Data["EarnestMoneyType"]);
                    mdlNew.EarnestMoney = Convert.ToDouble(_Data["EarnestMoney"]);
                    mdlNew.TenderFees = Convert.ToInt32(_Data["TenderFees"]);
                    mdlNew.Description = Convert.ToString(_Data["Description"]);
                    mdlNew.WorkStatus = Convert.ToString(_Data["WorkStatus"]);
                    mdlNew.WorkStatusDate = Convert.ToDateTime(_Data["WorkStatusDate"]);
                    mdlNew.WorkStatusBy = Convert.ToInt32(_Data["WorkStatusBy"]);
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["CreatedBy"]);
                    mdlNew.AM_AssetWorkDetail = new List<AM_AssetWorkDetail>();

                    if (Convert.ToString(_Data["AssetType"]) == "Asset")
                    {
                        mdlNew.AM_AssetWorkDetail = GetListAM_WorkDetail((_Data["AssetWorkDetail"]) as List<object>, mdlNew.CreatedBy);
                    }
                    else if (Convert.ToString(_Data["AssetType"]) == "Flood")
                    {
                        mdlNew.AM_AssetWorkDetail = GetListFlood_WorkDetail((_Data["FloodWorkDetail"]) as List<object>, mdlNew.CreatedBy);
                    }
                    else if (Convert.ToString(_Data["AssetType"]) == "Infrastructure")
                    {
                        mdlNew.AM_AssetWorkDetail = GetListInfra_WorkDetail((_Data["InfraWorkDetail"]) as List<object>, mdlNew.CreatedBy);
                    }

                    mdlNew.CreatedDate = DateTime.Now;
                    //mdlNew.ACPID = Convert.ToInt64(_Data["ACCPID"]);
                    //mdlNew.ID = Convert.ToInt64(_Data["ID"]);
                    dalAW.SaveWork(mdlNew);
                    status = true;
                    break;
                case Constants.CRUD_READ:
                    if (_Data.ContainsKey("ID"))
                    {
                        // status = dalAW.GetACCPOrder_ByID(Convert.ToInt64(_Data["ID"].ToString()));
                        return status;
                    }
                    break;
                case Constants.CRUD_UPDATE:
                    AM_AssetWork mdlUpdate = new AM_AssetWork();
                    mdlUpdate.ID = Convert.ToInt64(_Data["ID"]);
                    mdlUpdate.FinancialYear = Convert.ToString(_Data["FinancialYear"]);
                    mdlUpdate.DivisionID = Convert.ToInt64(_Data["DivisionID"]);
                    mdlUpdate.WorkName = Convert.ToString(_Data["WorkName"]);
                    mdlUpdate.FundingSourceID = Convert.ToInt32(_Data["FundingSource"]);
                    mdlUpdate.AssetWorkTypeID = Convert.ToInt64(_Data["AssetWorkTypeID"]);
                    mdlUpdate.AssetType = Convert.ToString(_Data["AssetType"]);
                    mdlUpdate.EstimatedCost = Convert.ToInt64(_Data["EstimatedCost"]);
                    mdlUpdate.CompletionPeriodFlag = Convert.ToBoolean(_Data["CompletionPeriodFlag"]);
                    mdlUpdate.CompletionPeriod = Convert.ToInt32(_Data["CompletionPeriod"]);
                    mdlUpdate.CompletionPeriodUnit = Convert.ToString(_Data["CompletionPeriodUnit"]);
                    if (Convert.ToDateTime(_Data["StartDate"]).Year != 0001)
                        mdlUpdate.StartDate = Convert.ToDateTime(_Data["StartDate"]);
                    if (Convert.ToDateTime(_Data["EndDate"]).Year != 0001)
                        mdlUpdate.EndDate = Convert.ToDateTime(_Data["EndDate"]);
                    mdlUpdate.SanctionNo = Convert.ToString(_Data["SanctionNo"]);
                    mdlUpdate.SanctionDate = Convert.ToDateTime(_Data["SanctionDate"]);
                    mdlUpdate.EarnestMoneyType = Convert.ToString(_Data["EarnestMoneyType"]);
                    mdlUpdate.EarnestMoney = Convert.ToDouble(_Data["EarnestMoney"]);
                    mdlUpdate.TenderFees = Convert.ToInt32(_Data["TenderFees"]);
                    mdlUpdate.Description = Convert.ToString(_Data["Description"]);
                    mdlUpdate.ModifiedBy = Convert.ToInt32(_Data["CreatedBy"].ToString());
                    mdlUpdate.ModifiedDate = DateTime.Now;

                    if (Convert.ToString(_Data["AssetType"]) == "Asset")
                    {

                        mdlUpdate.AM_AssetWorkDetail = GetListAM_WorkDetail((_Data["AssetWorkDetail"]) as List<object>, Convert.ToInt32(mdlUpdate.ModifiedBy), true);
                    }
                    else if (Convert.ToString(_Data["AssetType"]) == "Flood")
                    {

                        mdlUpdate.AM_AssetWorkDetail = GetListFlood_WorkDetail((_Data["FloodWorkDetail"]) as List<object>, Convert.ToInt32(mdlUpdate.ModifiedBy), true);
                    }
                    else if (Convert.ToString(_Data["AssetType"]) == "Infrastructure")
                    {
                        mdlUpdate.AM_AssetWorkDetail = GetListInfra_WorkDetail((_Data["InfraWorkDetail"]) as List<object>, Convert.ToInt32(mdlUpdate.ModifiedBy), true);
                    }

                    //  mdlUpdate.AM_AssetWorkDetail = GetListAM_WorkDetail((_Data["AssetWorkDetail"]) as List<object>, Convert.ToInt32(mdlUpdate.ModifiedBy), true);
                    dalAW.SaveWork(mdlUpdate);
                    status = true;
                    break;
                case Constants.CRUD_DELETE:
                    AM_AssetWork mdld = new AM_AssetWork();
                    mdld.ID = Convert.ToInt32(_Data["ID"].ToString());
                    dalAW.DeleteAssetSubCategory(mdld.ID);
                    status = true;
                    break;
                default:
                    break;
            }

            return status;
        }
        public bool IsAssetExistInWork(long _AWID)
        {
            return dalAW.IsAssetExistInWork(_AWID);
        }
        public bool IsAssetTypeWork(long _AWID)
        {
            return dalAW.IsAssetTypeWork(_AWID);
        }
        public List<object> GetWorkProgressHistory(int _WorkStatusID, List<long> _InspectedBy, long _WorkID, DateTime? _FromDate, DateTime? _ToDate)
        {
            return dalAW.GetWorkProgressHistory(_WorkStatusID, _InspectedBy, _WorkID, _FromDate, _ToDate);
        }
        public object GetWorkProgressByUser(long _AWID, long _UserID)
        {
            return dalAW.GetWorkProgressByUser(_AWID, _UserID);
        }
        public object GetCWDetailByID(long _AWID)
        {
            return dalAW.GetCWDetailByID(_AWID);
        }
        public object GetWorkProgress(long _ID)
        {
            return dalAW.GetWorkProgress(_ID);
        }
        public List<object> GetWorkStatusList()
        {
            return dalAW.GetWorkStatusList();
        }
        public bool SaveWorkProgressW(AM_AssetWorkProgress _Mdl, List<Tuple<string, string, string>> _Attachment, bool association, long _AWID, bool? _IsScheduled, long? _ScheduleDetailID)
        {
            dalAW.SaveWorkProgressW(_Mdl, _Attachment);
            if (_IsScheduled == true)
            {
                dalAW.UpdateScheduleDetailWorks(_Mdl.ID, _ScheduleDetailID.Value);
            }
            if (association == true)
            {
                SendNotifiactionAssociation(_AWID, _Mdl.CreatedBy, (long)NotificationEventConstants.AssetWorks.AssociateAssetWork);
            }

            return SendNotifiaction(_Mdl.ID, _Mdl.CreatedBy, (long)NotificationEventConstants.AssetWorks.AssetWorkProgress);
        }
        public List<object> GetAssetWorkDetailByWorkIDForTender(long _WorkID)
        {
            return dalAW.GetAssetWorkDetailByWorkIDForTender(_WorkID);
        }

        #endregion

        #region "Android"
        public List<object> GetAllYears()
        {
            return dalAW.GetAllYears();
        }

        public List<object> GetActiveWorkTypes()
        {
            return dalAW.GetActiveWorkTypes();
        }
        public List<object> GetWorkProgressStatus()
        {
            return dalAW.GetWorkProgressStatus();
        }
        public List<object> GetAssetWorksByDivisionID(long? _DivisionID, int? _WorkTypeID, string _Year, int _UserID)
        {
            return dalAW.GetAssetWorksByDivisionID(_DivisionID, _WorkTypeID, _Year, _UserID);
        }
        public long AddAssetWorkProgress(long _WorkProgressID, long _AssetWorkID, double _ProgressPercentage, double _FinancialPercentage, int _WorkStatusID, double? longitude, double? latitude, string _Source, string _Remarks, List<Tuple<string, string, string>> lstNameofFiles, int _UserID, long _ScheduleDetailID)
        {
            long ProgressID=dalAW.AddAssetWorkProgress(_WorkProgressID, _AssetWorkID, _ProgressPercentage, _FinancialPercentage, _WorkStatusID, longitude, latitude, _Source, _Remarks, lstNameofFiles, _UserID, _ScheduleDetailID);
            if (ProgressID > 0) {
                bool IsAssetType = IsAssetTypeWork(Convert.ToInt64(_AssetWorkID));
                bool IsExist = IsAssetExistInWork(Convert.ToInt64(_AssetWorkID));
                if (IsExist == false && IsAssetType == true && _ProgressPercentage == 100)
                {
                    SendNotifiactionAssociation(_AssetWorkID, _UserID, (long)NotificationEventConstants.AssetWorks.AssociateAssetWork);
                }
                SendNotifiaction(ProgressID, _UserID, (long)NotificationEventConstants.AssetWorks.AssetWorkProgress);
            }
            return ProgressID;
        }
        public object GetAssetWorkProgressByID(long _WorkPorgressID)
        {
            return dalAW.GetAssetWorkProgressByID(_WorkPorgressID);
        }
        public object GetAssetWorkByID(long _WorkID, long? WorkProgressID, int _UserID)
        {
            return dalAW.GetAssetWorkByID(_WorkID, WorkProgressID, _UserID);
        }
        #endregion

        #region Reports

        public List<UA_IrrigationLevel> GetIrrigationLevelForReports()
        {
            return dalAW.GetIrrigationLevelForReports();
        }

        public List<object> GetAttributeList(long? _SubcatID)
        {
            return dalAW.GetAttributeList(_SubcatID);
        }

        public List<object> GetAttributeValueList(long? _AttributeID)
        {
            return dalAW.GetAttributeValueList(_AttributeID);
        }

        public List<object> GetAssetName(long? _SubcatID)
        {
            return dalAW.GetAssetName(_SubcatID);
        }

        public List<object> GetAssetName(long? _SubcatID, long? IrrigationLevelID, long? IrrigationBoundaryID)
        {
            return dalAW.GetAssetName(_SubcatID, IrrigationLevelID, IrrigationBoundaryID);
        }







        #endregion

    }


}
