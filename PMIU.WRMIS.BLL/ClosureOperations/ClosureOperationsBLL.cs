using PMIU.WRMIS.Model;
using PMIU.WRMIS.DAL;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using PMIU.WRMIS.AppBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.DataAccess;
using PMIU.WRMIS.DAL.DataAccess.ClosureOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.BLL.Notifications;

namespace PMIU.WRMIS.BLL.ClosureOperations
{
    public class ClosureOperationsBLL : BaseBLL
    { 
        private ClosureOperationsDAL dalCO = new ClosureOperationsDAL();
        #region Notifications and Alerts
        public CW_GetCWPPublishNotifyData_Result GetClosureWorkPlanPuslishNotifyData(long _CWPID)
        {
            return dalCO.GetClosureWorkPlanPuslishNotifyData(_CWPID);
        }
        public CW_GetCWProgressNotifyData_Result GetCW_Progress_NotifyData(long _ProgressID)
        {
            return dalCO.GetCW_Progress_NotifyData(_ProgressID);
        }

        public bool SendNotifiaction (long _ID , long _UserID , long _EventID)        
        {
            NotifyEvent _event = new NotifyEvent();
            _event.Parameters.Add("ID", _ID); 
            return _event.AddNotifyEvent(_EventID, _UserID);
            
        }
        #endregion


        #region Reference Data

        public List<CW_TechnicalSanctionUnit> GetAllUnit()
        {
            return dalCO.GetAllUnit();
        }

        public bool UnitAssociationExists(long _UnitID)
        { 
            return dalCO.UnitAssociationExists(_UnitID);
        } 

        public object TechnicalSanctionUnit_Operations (int _OperationType , Dictionary<string , object> _Data) 
        {
            object status = false;
            switch(_OperationType)
            {
                case Constants.CRUD_CREATE:
                    CW_TechnicalSanctionUnit mdlNew = new CW_TechnicalSanctionUnit();
                    mdlNew.Name = _Data["Name"].ToString();
                    mdlNew.Description = _Data["Description"].ToString();
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdlNew.IsActive = (bool)_Data["IsActive"];
                    dalCO.AddTchnclSnctnUnit(mdlNew);
                    status = true; 
                    break;

                case Constants.CRUD_READ:
                    if(_Data.ContainsKey("Name")) {
                        status = dalCO.GetTchnclSnctnUnit_ByName(_Data["Name"].ToString());
                    }
                    break;

                case Constants.CRUD_UPDATE:
                    CW_TechnicalSanctionUnit mdl = new CW_TechnicalSanctionUnit();
                    mdl.ID = Convert.ToInt32(_Data["ID"]);
                    mdl.Name = _Data["Name"].ToString();
                    mdl.Description = _Data["Description"].ToString();
                    mdl.ModifiedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdl.IsActive = (bool)_Data["IsActive"];
                    dalCO.UpdateTchnclSnctnUnit(mdl);
                    status = true; 
                    break;

                case Constants.CRUD_DELETE: 
                    dalCO.DeleteTchnclSnctnUnit(Convert.ToInt64(_Data["ID"]));
                    status = true;
                    break;

                default:
                    break;
            } 

            return status;
        }

        public List<CW_WorkType> GetAllClosureWorkType()
        {
            return dalCO.GetAllClosureWorkType();
        }

        public bool ClosureWorkTypeAssiciatExists(long _ID)
        {
            return dalCO.ClosureWorkTypeAssiciatExists(_ID);
        } 

        public object ClosureWorkType_Operations(int _OperationType, Dictionary<string, object> _Data)
        {
            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE:
                    CW_WorkType mdlNew = new CW_WorkType();
                    mdlNew.Name = _Data["Name"].ToString();
                    mdlNew.Description = _Data["Description"].ToString();
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdlNew.IsActive = (bool)_Data["IsActive"];
                    dalCO.AddClsurWrkType(mdlNew);
                    status = true;
                    break;

                case Constants.CRUD_READ:
                    if (_Data.ContainsKey("Name"))
                    {
                        status = dalCO.GetClsurWrkType_ByName(_Data["Name"].ToString());
                    }
                    break;

                case Constants.CRUD_UPDATE:
                    CW_WorkType mdl = new CW_WorkType();
                    mdl.ID = Convert.ToInt32(_Data["ID"]);
                    mdl.Name = _Data["Name"].ToString();
                    mdl.Description = _Data["Description"].ToString();
                    mdl.ModifiedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdl.IsActive = (bool)_Data["IsActive"];
                    dalCO.UpdateClsurWrkType(mdl);
                    status = true;
                    break;

                case Constants.CRUD_DELETE:
                    dalCO.DeleteClsurWrkType(Convert.ToInt64(_Data["ID"]));
                    status = true;
                    break;

                default:
                    break;
            }

            return status;
        }
        public List<object> GetAllClosureWorkTypes()
        {
            return dalCO.GetAllClosureWorkTypes();
        }
        public List<object> GetAllClosureWorkStatus()
        {
            return dalCO.GetAllClosureWorkStatus();
        }
        #endregion

        #region Annual Canal Closure Program


        public List<object> GetAllACCP()
        {
            return dalCO.GetAllACCP();
        }
            public List<object> GetACCP(string _Year)
        {
            return dalCO.GetACCP(_Year);
        }
        public List<object> GetACCPYears()
        {
            return dalCO.GetACCPYears();
        }
        public object ACCP_Operations(int _OperationType, Dictionary<string, object> _Data,out int _acpID) 
        {

            object status = false;
            _acpID = 0;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE:
                    CW_AnnualClosureProgram mdlNew = new CW_AnnualClosureProgram();
                    mdlNew.Title = _Data["Name"].ToString();
                    mdlNew.Year = _Data["Year"].ToString();
                    mdlNew.Attachment = _Data["Attachment"].ToString();
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdlNew.CreatedDate = DateTime.Now;
                    dalCO.AddACCP(mdlNew,out _acpID);
                    status = true;
                    break;
                case Constants.CRUD_READ:
                    if (_Data.ContainsKey("ID"))
                    {
                        status = dalCO.GetACCP_ByID(Convert.ToInt64 (_Data["ID"].ToString()));
                    } 
                    else if (_Data.ContainsKey("Year")  )
                    {
                        status = dalCO.GetACCP_ByYear ( _Data["Year"].ToString());
                    } 
                    break;
                case Constants.CRUD_UPDATE:
                   CW_AnnualClosureProgram mdl = new CW_AnnualClosureProgram();
                    mdl.ID = Convert.ToInt64(_Data["ID"].ToString());
                    mdl.Title = _Data["Name"].ToString();
                    mdl.Year = _Data["Year"].ToString();
                    mdl.Attachment = _Data["Attachment"].ToString();
                    mdl.ModifiedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    dalCO.UpdateACCP(mdl);
                    status = true;
                    break;

                default:
                    break;
            }

            return status;
        }

        #endregion
        
        #region Annual Cananl Closure Progamme Orders/Latters
        public List<object> GetClosureWorksByCWPID(long _CWPID)
        {
            return dalCO.GetClosureWorksByCWPID(_CWPID);
        }
        public bool isCWP_Published(long _CWPID)
        {
            return dalCO.isCWP_Published(_CWPID);
        }
        public object GetCWPDetailByID(long _CWPID)
        {
            return dalCO.GetCWPDetailByID(_CWPID);
        }
        public object GetCWDetailByID(long _CWID)
        {
            return dalCO.GetCWDetailByID(_CWID) ;
        }

        public CW_AnnualClosureProgram GetACCP_ByID(long _ACCPID)
        {
            return new ClosureOperationsDAL().GetACCP_ByID(_ACCPID);
        }
        public List<object> GetACCPOrdersByACCPID(long _ACCPID)
        {
            return dalCO.GetACCPOrdersByACCPID(_ACCPID);
        }
        public object ACCPOrders_Operations(int _OperationType, Dictionary<string, object> _Data)
        {

            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE:
                    CW_AnnualClosureProgramOrder mdlNew = new CW_AnnualClosureProgramOrder();
                    mdlNew.LetterNo = _Data["LatterNo"].ToString();
                    mdlNew.LetterDate = Convert.ToDateTime(_Data["LatterDate"]);
                    mdlNew.Attachment = _Data["FileName"].ToString();
                    mdlNew.ACPID = Convert.ToInt64(_Data["ACCPID"]);
                    mdlNew.ID = Convert.ToInt64(_Data["ID"]);
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdlNew.CreatedDate = DateTime.Now;
                    dalCO.AddACCPOrders(mdlNew);
                    status = true;
                    break;
                case Constants.CRUD_READ:
                    if (_Data.ContainsKey("ID"))
                    {
                        status = dalCO.GetACCPOrder_ByID(Convert.ToInt64(_Data["ID"].ToString()));
                        return status;
                    }
                    break;
                case Constants.CRUD_UPDATE:
                    CW_AnnualClosureProgramOrder mdl = new CW_AnnualClosureProgramOrder();
                    mdl.ID = Convert.ToInt64(_Data["ID"].ToString());
                    mdl.LetterNo = _Data["LatterNo"].ToString();
                    mdl.LetterDate = Convert.ToDateTime(_Data["LatterDate"]);
                    mdl.Attachment = _Data["FileName"].ToString();
                    mdl.ModifiedBy = Convert.ToInt32(_Data["UserID"].ToString());
                    mdl.ModifiedDate = DateTime.Now;
                    dalCO.UpdateACCPOrder(mdl);
                    status = true;
                    break;
                case Constants.CRUD_DELETE:
                    CW_AnnualClosureProgramOrder mdld = new CW_AnnualClosureProgramOrder();
                    mdld.ID = Convert.ToInt64(_Data["ID"].ToString());
                    dalCO.DeleteACCPOrders(mdld);
                    status = true;
                    break;
                default:
                    break;
            }

            return status;
        }
        #endregion
        #region Annual Canal Closure Work Programme  Detail
        public bool AssociationExists(long _ACPDID)
        {
            int count = db.Repository<CW_ACPExcludedChannel>().Query().Get().Where(x => x.ACPDetailID == _ACPDID).Count();
            if (count > 0)
                return true;

            return false;
        }

        public List<object> GetChannelsByChannelTypeandChannelCommandType(int _ChannelType, int _ChannelCommandType)
        {
            return dalCO.GetChannelsByChannelTypeandChannelCommandType(_ChannelType, _ChannelCommandType);
        }
        public List<object> GetTCandMCByACCPID(long _ACCPID)
        {
            return dalCO.GetTCandMCByACCPID(_ACCPID);
        }
        public object ACCPDetail_Operations(int _OperationType, Dictionary<string, object> _Data)
        {

            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE:
                    CW_AnnualClosureProgramDetail mdlNew = new CW_AnnualClosureProgramDetail();
                    mdlNew.ChannelID = Convert.ToInt64(_Data["ChannelID"]);
                    mdlNew.CommandType = Convert.ToInt32(_Data["CommandType"]);
                    mdlNew.ACPID = Convert.ToInt64(_Data["ACPID"]);
                    mdlNew.FromDate = Convert.ToDateTime(_Data["FromDate"]);
                    mdlNew.ToDate = Convert.ToDateTime(_Data["ToDate"]);
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["CreatedBy"].ToString());
                    mdlNew.CreatedDate = DateTime.Now;
                    dalCO.AddACCPDetail(mdlNew);
                    status = true;
                    break;
                case Constants.CRUD_READ:
                    if (_Data.ContainsKey("ID"))
                    {
                        status = dalCO.GetACCPDetail_ByID(Convert.ToInt64(_Data["ID"].ToString()));
                        return status;
                    }
                    break;
                case Constants.CRUD_UPDATE:
                    CW_AnnualClosureProgramDetail mdl = new CW_AnnualClosureProgramDetail();
                    mdl.ID = Convert.ToInt64(_Data["ID"]);
                    mdl.ChannelID = Convert.ToInt64(_Data["ChannelID"]);
                    mdl.CommandType = Convert.ToInt32(_Data["CommandType"]);
                    mdl.ACPID = Convert.ToInt64(_Data["ACPID"]);
                    mdl.FromDate = Convert.ToDateTime(_Data["FromDate"]);
                    mdl.ToDate = Convert.ToDateTime(_Data["ToDate"]);
                    mdl.ModifiedBy = Convert.ToInt32(_Data["CreatedBy"].ToString());
                    mdl.ModifiedDate = DateTime.Now;
                    dalCO.UpdateACCPDetail(mdl);
                    status = true;
                    break;
                case Constants.CRUD_DELETE:
                    CW_AnnualClosureProgramDetail mdld = new CW_AnnualClosureProgramDetail();
                    mdld.ID = Convert.ToInt64(_Data["ID"].ToString());
                    dalCO.DeleteACCPDetail(mdld);
                    status = true;
                    break;
                default:
                    break;
            }

            return status;
        }
        #endregion
        #region ACCP Detail
        public bool CopyLastYearACCP_Detial_ExcludedChannels(string _ACCPTitle, string _Year, string _Attachment, long _CreatedBy)
        {
            return dalCO.CopyLastYearACCP_Detial_ExcludedChannels(_ACCPTitle, _Year, _Attachment, _CreatedBy);
        }
        #endregion
        #region Exclude Channels From Annual Canal Closure Programme
        public List<object> GetChannelsforExclude(long _MainChannelID)
        {
            return dalCO.GetChannelsforExclude(_MainChannelID);
        }
        public List<object> GetExcludedChannels(long _MainChannelID)
        {
            return dalCO.GetExcludedChannels(_MainChannelID);
        }

        public object ACCPExclude_Operations(int _OperationType, Dictionary<string, object> _Data)
        {

            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE:
                    CW_ACPExcludedChannel mdlAccpXChannel = new CW_ACPExcludedChannel();

                    mdlAccpXChannel.ACPDetailID = Convert.ToInt64(_Data["DetailID"]);
                    mdlAccpXChannel.ChannelID = Convert.ToInt64(_Data["ChannelID"]);
                    mdlAccpXChannel.CreatedBy = Convert.ToInt32(_Data["CreatedBy"]);
                    mdlAccpXChannel.CreatedDate = DateTime.Now;
                    dalCO.AddACCPExcludeChannels(mdlAccpXChannel);
                    status = true;
                    break;
                case Constants.CRUD_DELETE:
                    CW_ACPExcludedChannel mdld = new CW_ACPExcludedChannel();
                    mdld.ChannelID = Convert.ToInt64(_Data["ID"].ToString());
                    mdld.ACPDetailID = Convert.ToInt64(_Data["DetailID"]);
                    dalCO.DeleteAccpXchannels(mdld);
                    status = true;
                    break;
                default:
                    break;
            }

            return status;
        }

        #endregion

        #region Closure Work Plan
        public string GetFinancialYearbyCWP(long _CWPID)
        {
            return dalCO.GetFinancialYearbyCWP(_CWPID);
        }
        public void UnPublishCWPlan(long _CWPID)
        {
            CW_ClosureWorkPlan mdl = db.Repository<CW_ClosureWorkPlan>().FindById(_CWPID);
            if (mdl != null)
                mdl.Status = "Draft";

            db.Repository<CW_ClosureWorkPlan>().Update(mdl);
            db.Save();

        }
        public bool IsCWPInTender(long _CWPID)
        {
            return dalCO.IsCWPInTender(_CWPID);
        }

        public string IsClosureWorkPlanCostEstimationCorrect(long _CWPID)
        {
            string result = "";
            List<CW_ClosureWork> lst = db.Repository<CW_ClosureWork>().Query().Get().Where(x => x.CWPID == _CWPID).ToList();
            if (lst != null || lst.Count() > 0)
            {
                foreach (var item in lst)
                {
                    bool status = dalCO.IsClosureWorkCostEstimationCorrect(item.ID);
                    if (!status)
                    {
                        result = item.WorkName;
                        break;
                    }
                }
            }
            return result;
        }
        public CW_ClosureWorkPlan GetCWPByID(long _CWPID)
        {
            return db.Repository<CW_ClosureWorkPlan>().Query().Get().Where(x => x.ID == _CWPID).FirstOrDefault();
        }
        public object GetWorkProgress(long _ID)
        {
            return dalCO.GetWorkProgress(_ID);
        }
        public bool IsCWPComplete(long _CWPID)
        {
            return dalCO.IsCWPComplete(_CWPID);
        }
        public List<object> GetWorkProgressHistory(int _WorkStatusID, List<long> _InspectedBy, long _WorkID, DateTime? _FromDate, DateTime? _ToDate)
        {
            return dalCO.GetWorkProgressHistory(_WorkStatusID, _InspectedBy, _WorkID, _FromDate, _ToDate);
        }
        public bool SaveWorkProgress(CW_WorkProgress _Mdl, List<Tuple<string, string, string>> _Attachment,bool? _IsScheduled,long? _ScheduleDetailID)
        {
            dalCO.SaveWorkProgress(_Mdl,_Attachment);
            if (_IsScheduled == true)
            {
                dalCO.UpdateScheduleDetailWorks(_Mdl.ID, _ScheduleDetailID.Value);
            }
            return SendNotifiaction(_Mdl.ID, _Mdl.CreatedBy, (long)NotificationEventConstants.ClosureOperations.ClosureWorkProgress); 
        }
        public object GetWorkProgressByUser(long _CWID , long _UserID)
        {
            return dalCO.GetWorkProgressByUser(_CWID ,_UserID);
        }
        //public object GetWorkProgressByUserScheduled(long _CWID, long _UserID, long _RefMonitoringID)
        //{
        //    return dalCO.GetWorkProgressByUserScheduled(_CWID, _UserID, _RefMonitoringID);
        //}
        public bool IsWorkOfDesiltingType(long _CWID)
        {
            return dalCO.IsWorkOfDesiltingType(_CWID);
        }
        public List<object> GetWorkStatusList ()
        {
            return dalCO.GetWorkStatusList();
        }
        public double? ContractorAmountPerWorkItem(long _ID)
        {
            return dalCO.ContractorAmountPerWorkItem(_ID);
        }
        public object CWItems_Operations(int _OperationType, CW_WorkItem _Data, string _AssetWork = "")
        {
            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_DELETE:
                    dalCO.DeleteWorkItem(_Data.ID);
                    status = true;
                    break;
                case Constants.CRUD_UPDATE:
                    dalCO.UpdateWorkItem(_Data);
                    status = true;
                    break;
                case Constants.CRUD_CREATE:
                    _Data.WorkType = _AssetWork == "" ? "CLOSURE" : _AssetWork;
                    dalCO.SaveWorkItem(_Data);
                    status = true;
                    break;
                case Constants.CHECK_DUPLICATION:
                    status = dalCO.GetClosureWorkItem(_Data.ItemDescription, 0, _Data.WorkID, "ByName","ASSETWORK"); 
                    break;
                default:
                    break;
            }
            return status;
        }
        public List<object> GetClosureWorkItems_ByCWID(long _CWID, string _SourceType = "")
        {
            return dalCO.GetClosureWorkItems_ByCWID(_CWID, _SourceType);
        }
        public double? ContractorAmount(long _CWID)
        {
            return dalCO.ContractorAmount(_CWID);
        }
        public bool IsTenderAwarded(long _CWID)
        {
            return dalCO.IsTenderAwarded(_CWID);
        }
        public bool PublishCWP(long _CWPID, long _UserID)
        {
            bool result = dalCO.PublishCWP(_CWPID, _UserID);
            if (result)
            {
                bool notify = SendNotifiaction(_CWPID, _UserID,(long) NotificationEventConstants.ClosureOperations.PublishClosureWorkPlan);
            }
            return result;
        }
        public object CWP_Operations(int _OperationType, Dictionary<string, object> _Data)
        {

            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE:
                    CW_ClosureWorkPlan mdlNew = new CW_ClosureWorkPlan();
                    mdlNew.DivisionID = Convert.ToInt64(_Data["DivisionID"]);
                    mdlNew.Comments = _Data["Comments"].ToString();
                    mdlNew.Title = _Data["Title"].ToString();
                    mdlNew.Year = _Data["Year"].ToString();
                    mdlNew.ACPID = Convert.ToInt64(_Data["ACPID"].ToString()) ; 
                    mdlNew.CreatedBy = Convert.ToInt32(_Data["CreatedBy"].ToString());
                    mdlNew.CreatedDate = DateTime.Now;
                    mdlNew.Status = "Draft"; 
                    dalCO.AddCWP(mdlNew);
                    status = true;
                    break;
                case Constants.CRUD_READ:
                    if (_Data.ContainsKey("DivisionID") && _Data.ContainsKey("Year"))
                    {
                        status = dalCO.GetCWPBy_Year_Name(_Data["Year"].ToString(),Convert.ToInt64(_Data["DivisionID"]));
                    }
                    else if (_Data.ContainsKey("ID"))
                    {
                        status = dalCO.GetClosureWorkPlanByID(Convert.ToInt64(_Data.ContainsKey("ID")));
                    }
                    break;
                case Constants.CRUD_UPDATE:
                    CW_ClosureWorkPlan mdl = new CW_ClosureWorkPlan();
                    mdl.ID = Convert.ToInt64(_Data["ID"]);
                    mdl.ACPID = Convert.ToInt64(_Data["ACPID"]);
                    mdl.DivisionID = Convert.ToInt64(_Data["DivisionID"]);
                    mdl.Comments = _Data["Comments"].ToString();
                    mdl.Title = _Data["Title"].ToString();
                    mdl.Year = _Data["Year"].ToString(); 
                    mdl.ModifiedBy = Convert.ToInt32(_Data["ModifiedBy"].ToString());
                    mdl.ModifiedDate = DateTime.Now;
                    dalCO.UpdateCWP(mdl);
                    status = true;
                    break;
                case Constants.CRUD_DELETE:
                    CW_ClosureWorkPlan mddlt = new CW_ClosureWorkPlan();
                    mddlt.ID = Convert.ToInt64(_Data["DivisionID"]);
                    dalCO.DeleteCWP(mddlt);
                    status = true;
                    break;

                default:
                    break;
            }

            return status;
        }
        public object ClosureWork_Operations(int _OperationType, CW_ClosureWork _MdlClosueWork)
        {
            object status = false;
            switch (_OperationType)
            {
                case Constants.CRUD_CREATE: 
                    dalCO.AddClosureWork(_MdlClosueWork);
                    status = true;
                    break;
                case Constants.CRUD_UPDATE:
                    dalCO.UpdateClosureWork(_MdlClosueWork);
                    status = true;
                    break;
                case Constants.CRUD_READ:
                    status = dalCO.GetCW_ByID(_MdlClosueWork.ID);
                    break;

                case Constants.CHECK_DUPLICATION:
                    status = dalCO.GetCW_ByName_CWPID_WorkType(_MdlClosueWork.CWPID, _MdlClosueWork.WorkName, _MdlClosueWork.WorkTypeID);
                    break;

                case Constants.CRUD_DELETE:
                    status = dalCO.DeleteClosureWork(_MdlClosueWork.ID);
                    break;
                case Constants.CHECK_ASSOCIATION:
                    status = dalCO.ClosureWorkAssociationExists(_MdlClosueWork.ID);
                    break;
                default:
                    break;
            }

            return status;
        }
        public bool ClosureWorkPlandDependency(long _CWPID)
        {
            return new ClosureOperationsDAL().ClosureWorkPlandDependency(_CWPID);
        }
        public bool DeleteClosureWorkPlan(long _CWPID)
        {
            return new ClosureOperationsDAL().DeleteClosureWorkPlan(_CWPID);
        }

        public List<object> GetClosureClosureWorkPlans()
        {
            return new ClosureOperationsDAL().GetClosureClosureWorkPlans();
        }

        public long? GetCirleIdByDivisionID(int divID)
        {
            return new ClosureOperationsDAL().GetCirleIdByDivisionID(divID);
 
        }

        public CW_ClosureWorkPlan GetCWPBy_Year_Name(string year , long DivisionID)
        {
            return new ClosureOperationsDAL().GetCWPBy_Year_Name(year,DivisionID);
        }
        public CW_ClosureWorkPlan GetClosureWorkPlanByID(long _CWPID)
        {
            return new ClosureOperationsDAL().GetClosureWorkPlanByID(_CWPID);
        }
        public List<object> GetOldestClosureWorkPlan()
        {
            return new ClosureOperationsDAL().GetOldestClosureWorkPlan();
        }
        public List<object> GetUniqueYearFromCWP( )
        {
            return new ClosureOperationsDAL().GetUniqueYearFromCWP( );
        }
        public bool IsClosureWorkPlanAlreadyExist(CW_ClosureWorkPlan _CWP)
        {
            return new ClosureOperationsDAL().IsClosureWorkPlanAlreadyExist(_CWP);
        }

        public List<object> GetCWP_By_DivID_Year(List<long?> _ZoneID, List<long?> _CircleID, List<long> _DivisionID, string _Year, string _Status)
        {
            List<long> lstDivisionIDs = new List<long>();

            if (_DivisionID.Count() > 0)
                lstDivisionIDs = _DivisionID;
            else if (_CircleID.Count() > 0)
                lstDivisionIDs = db.Repository<CO_Division>().Query().Get().Where(x => _CircleID.Contains(x.CircleID)).Select(x => x.ID).ToList<long>();
            else if (_ZoneID.Count() > 0)
            {
                List<long?> lstC = new List<long?>();
                List<long> lstCircles = db.Repository<CO_Circle>().Query().Get().Where(x => _ZoneID.Contains(x.ZoneID)).Select(x => x.ID).ToList<long>();
                foreach (var c in lstCircles)
                    lstC.Add(c);
                lstDivisionIDs = db.Repository<CO_Division>().Query().Get().Where(x => lstC.Contains(x.CircleID)).Select(x => x.ID).ToList<long>();
            }

            return dalCO.GetCWP_By_DivID_Year(lstDivisionIDs, _Year, _Status);
        }
        #endregion


        

        #region Monitoring Closure Work Plan
        #endregion

        #region Helper Methods
        
        public string GetDesignationByID(long _DesiganationID)
        {
            return dalCO.GetDesignationByID(_DesiganationID);
        }

        public UA_Designations GetDesignationByName(string _DesiganationName)
        {
            return db.Repository<UA_Designations>().Query().Get().Where(x => x.Name.ToUpper().Equals(_DesiganationName)).FirstOrDefault();
        }
        public bool IsRDWithinUserDivision(int RD, long _XENUserID , long _ChannelID)
        {
            int? startRD = dalCO.GetStartRDofChannelForDivision(_XENUserID, _ChannelID);
            int? endRD = dalCO.GetEndRDofChannelForDivision(_XENUserID, _ChannelID);

            if (startRD == null || endRD == null)
                return false;

            if (RD >= startRD && RD <= endRD) 
                return true;
            
            return false;
        }
        public List<object> GetBarragesByXEN(long _XENUserID)
        {
            return dalCO.GetBarragesByXEN(_XENUserID);
        }
        public List<object> GetSubdivisionByXEN(long _XENUserID)
        {
            return dalCO.GetSubdivisionByXEN(_XENUserID);
        }

        public List<object> GetSectionBySubDivID(long _SubDivID)
        {
            return dalCO.GetSectionBySubDivID(_SubDivID);
        }
        public List<object> GetChannelByXEN(long _XENUserID)
        {
            return dalCO.GetChannelByXEN(_XENUserID);
        }
        public List<object> GetChannelBySubDivID(long _SubDivID)
        {
            return dalCO.GetChannelBySubDivID(_SubDivID);
        }
        public List<object> GetChannelBySectionID(long _SecID)
        {
            return dalCO.GetChannelBySectionID(_SecID);
        }
        public List<object> GetStructureByXEN(long _XENUserID)
        {
            return dalCO.GetStructureByXEN(_XENUserID);
        }
        public List<object> GetDistrictByXEN(long _XENUserID)
        {
            return dalCO.GetDistrictByXEN(_XENUserID);
        }
        public List<object> GetTehsilByDistrict(long _DistrictID)
        {
            return dalCO.GetTehsilByDistrict(_DistrictID);
        }
        #endregion

        #region Closure Works

        public List<object> GetClosureWorksByDivisionID(long? _DivisionID, int? _WorkTypeID,string _Year,int _UserID)
        {
            return dalCO.GetClosureWorksByDivisionID(_DivisionID, _WorkTypeID, _Year,_UserID);
        }

        public long AddClosureWorkProgress(int _WorkProgressID, long _ClosureWorkID, double _ProgressPercentage, int _WorkStatusID, int? _SiltRemovedQty, double? _ChannelDesiltedLen, string _Remarks, List<Tuple<string, string, string>> lstNameofFiles, int _UserID, long _ScheduleDetailID)
        {
            long ID = dalCO.AddClosureWorkProgress(_WorkProgressID, _ClosureWorkID, _ProgressPercentage, _WorkStatusID, _SiltRemovedQty, _ChannelDesiltedLen, _Remarks, lstNameofFiles, _UserID, _ScheduleDetailID);
            SendNotifiaction(ID, _UserID, (long)NotificationEventConstants.ClosureOperations.ClosureWorkProgress);
            return ID;
        }

        public object GetClosureWorkProgressByID(long _WorkPorgressID)
        {
            return dalCO.GetClosureWorkProgressByID(_WorkPorgressID);
        }
        public object GetClosureWorkByID(long _WorkID, long? WorkProgressID, int _UserID)
        {
            return dalCO.GetClosureWorkByID(_WorkID, WorkProgressID, _UserID);
        }
        #endregion
    }
    
}
