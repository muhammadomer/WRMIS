using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.WaterTheft;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.WaterTheft
{
    public class WaterTheftDAL
    {

        ContextDB db = new ContextDB();

        public List<object> GetChannelByUserId(long _UserID, long _IrrigationLevelID)
        {
            List<object> lstChannel = db.ExtRepositoryFor<WaterTheftRepository>().GetChannelByUserID(_UserID, _IrrigationLevelID).ToList();
            return lstChannel;
        }
        public List<object> GetChannelByDivisionID(long _DivisionID)
        {
            List<object> lstChannel = db.ExtRepositoryFor<WaterTheftRepository>().GetChannelByDivisionID(_DivisionID).ToList();
            return lstChannel;
        }
        public List<object> GetOutletByChannelId(long _ChannelID, long _UserID, long _IrrigationLevelID)
        {
            List<object> lstChannel = db.ExtRepositoryFor<WaterTheftRepository>().GetOutletByChannelID(_ChannelID, _UserID, _IrrigationLevelID).ToList();
            return lstChannel;
        }
        public List<object> GetAssetWorkOutletByChannelID(long _DivisionID)
        {
            List<object> lstChannel = db.ExtRepositoryFor<WaterTheftRepository>().GetAssetWorkOutletByChannelID(_DivisionID).ToList();
            return lstChannel;
        }
        public object GetDatabyOutletId(int _OutletID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetDataByOutletID(_OutletID);
        }

        public bool IsWaterTheftCaseExist(WT_WaterTheftCase _mdlWatertheftCase, int _NoofFeet)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().IsWaterTheftCaseExist(_mdlWatertheftCase, _NoofFeet);
        }

        public List<object> GetAllTheftType(string _OffenceSiteId)
        {
            List<object> lstChannel = db.ExtRepositoryFor<WaterTheftRepository>().GetAllTheftType(_OffenceSiteId).ToList();
            return lstChannel;
        }

        public List<object> GetOutletCondition(string _OffenceSiteId)
        {
            List<object> lstChannel = db.ExtRepositoryFor<WaterTheftRepository>().GetOutletCondition(_OffenceSiteId).ToList();
            return lstChannel;
        }

        public List<object> GetDefectiveType()
        {
            List<object> lstChannel = db.ExtRepositoryFor<WaterTheftRepository>().GetDefectiveType().ToList();
            return lstChannel;
        }

        #region Ziledar Outlet Working

        public object GetZiladaarWorkingParentInformation(long _CaseID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetZiladaarWorkingParentInformation(_CaseID);
        }

        public List<WT_WaterTheftOffender> GetOffenders(long _CaseID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetOffenders(_CaseID);
        }

        public bool DeleteOffender(long _DeleteID, long _WTCaseID)
        {
            bool Result = false;
            try
            {
                if (_DeleteID > 0)
                {
                    db.Repository<WT_WaterTheftOffender>().Delete(_DeleteID);
                    db.ExtRepositoryFor<WaterTheftRepository>().UpdateNoOfOffenders(_WTCaseID);
                    db.Save();
                    Result = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public bool AddOffender(WT_WaterTheftOffender _ObjSave)
        {
            try
            {
                db.Repository<WT_WaterTheftOffender>().Insert(_ObjSave);
                db.ExtRepositoryFor<WaterTheftRepository>().IncrementOffenders(_ObjSave.WaterTheftID);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool UpdateOffender(WT_WaterTheftOffender _ObjUpdate)
        {
            try
            {
                WT_WaterTheftOffender ObjOffender = db.Repository<WT_WaterTheftOffender>().FindById(_ObjUpdate.ID);
                ObjOffender.OffenderName = _ObjUpdate.OffenderName;
                ObjOffender.CNIC = _ObjUpdate.CNIC;
                ObjOffender.Address = _ObjUpdate.Address;
                db.Repository<WT_WaterTheftOffender>().Update(ObjOffender);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public List<WT_AreaType> GetAreaTypes()
        {
            List<WT_AreaType> lstAreaType = new List<WT_AreaType>();
            try
            {
                lstAreaType = db.Repository<WT_AreaType>().GetAll().ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstAreaType;
        }

        public bool AddWaterTheftDecisionFineDetail(WT_WaterTheftDecisionFineDetail _ObjSave)
        {
            try
            {
                db.Repository<WT_WaterTheftDecisionFineDetail>().Insert(_ObjSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool AddZiladaarPoliceCase(WT_WaterTheftPoliceCase _ObjSave)
        {
            try
            {
                db.Repository<WT_WaterTheftPoliceCase>().Insert(_ObjSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool AddWTStatus(long _WTCaseID, long _UserID, long _UserDesID)
        {
            try
            {
                object Manager = db.ExtRepositoryFor<WaterTheftRepository>().GetManager(_UserID);
                WT_WaterTheftStatus WTStatus = new WT_WaterTheftStatus();
                WTStatus.WaterTheftID = _WTCaseID;
                if (Manager.GetType().GetProperty("ManagerID").GetValue(Manager) != null)
                    WTStatus.AssignedToUserID = Convert.ToInt64(Manager.GetType().GetProperty("ManagerID").GetValue(Manager));
                else
                    WTStatus.AssignedToUserID = -1;
                if (Manager.GetType().GetProperty("ManagerDesID").GetValue(Manager) != null)
                    WTStatus.AssignedToDesignationID = Convert.ToInt64(Manager.GetType().GetProperty("ManagerDesID").GetValue(Manager));
                else
                    WTStatus.AssignedToDesignationID = -1;

                WTStatus.AssignedByUserID = _UserID;
                if (_UserDesID > 0)
                    WTStatus.AssignedByDesignationID = _UserDesID;
                WTStatus.AssignedDate = DateTime.Now;
                db.Repository<WT_WaterTheftStatus>().Insert(WTStatus);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public int? SaveZiladaarWorking(long? _UserID, long? _DesignationID, long? _CaseID, double? _AreaBooked, long? _AreaUnit, int? _NoOfAccused, DateTime _ProcedingDate, string _ZildarRemarks)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().SaveZiladaarWorking(_UserID, _DesignationID, _CaseID, _AreaBooked, _AreaUnit, _NoOfAccused, _ProcedingDate, _ZildarRemarks);
        }

        public object GetSavedZiladaarWorking(long _WTCaseID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetSavedZiladaarWorking(_WTCaseID);
        }

        public long? GetManagerSubOrdinate(long _ManagerID, long _SubDesID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetManagerSubOrdinate(_ManagerID, _SubDesID);
        }

        public long GetCaseStatus(long _WTCaseID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetCaseStatus(_WTCaseID);
        }

        #endregion


        //public long AddOutletChannelIncidentCase(WT_WaterTheftCase _mdlWatertheftCase)
        //{
        //    try
        //    {
        //        db.Repository<WT_WaterTheftCase>().Insert(_mdlWatertheftCase);
        //        db.Save();
        //        return _mdlWatertheftCase.ID;
        //    }

        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        public int AddOutletChannelIncidentCase(WT_WaterTheftCase _mdlWatertheftCase, WT_WaterTheftStatus _mdlWaterTheftStaus, WT_OutletDefectiveDetails _mdloutletDefectiveDetails)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().SaveOutletChannelIncidentCase(_mdlWatertheftCase, _mdlWaterTheftStaus, _mdloutletDefectiveDetails);
        }


        //public long AddOutletDefectiveTypeDetails(WT_OutletDefectiveDetails _mdlOutletDefectiveDetails)
        //{
        //    try
        //    {
        //        db.Repository<WT_OutletDefectiveDetails>().Insert(_mdlOutletDefectiveDetails);
        //        db.Save();
        //        return _mdlOutletDefectiveDetails.ID;
        //    }

        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        public long AddWaterTheftCaseStatus(WT_WaterTheftStatus _mdlWaterTheftCaseStatus)
        {
            try
            {
                db.Repository<WT_WaterTheftStatus>().Insert(_mdlWaterTheftCaseStatus);
                db.Save();
                return _mdlWaterTheftCaseStatus.ID;
            }

            catch (Exception ex)
            {
                return 0;
            }
        }



        #region "Search Water Theft"

        /// <summary>
        /// This function fetches Division based on the UserID and User Irrigation Level.
        /// Created On 06-05-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByUserID(long _UserID, long _IrrigationLevelID, bool? _IsActive = null)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetDivisionsByUserID(_UserID, _IrrigationLevelID, _IsActive);
        }

        /// <summary>
        /// This function fetches Channel based on the UserID ,User Irrigation Level and DivisionID.
        /// Created On 12-05-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_DivisionID"></param>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetChannelsByUserIDAndDivisionID(long _UserID, long? _IrrigationLevelID, long _DivisionID, bool? _IsActive = null)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetChannelsByUserIDAndDivisionID(_UserID, _IrrigationLevelID, _DivisionID, _IsActive);
        }

        /// <summary>
        /// This function fetches Water Theft Status
        /// Created On 10-05-2016
        /// </summary>
        /// <returns>List<WT_Status></returns>
        public List<WT_Status> GetStatus()
        {
            return db.Repository<WT_Status>().GetAll().ToList();
        }

        /// <summary>
        /// This function fetches Water Theft Offence Type
        /// Created On 10-05-2016
        /// </summary>
        /// <param name="_OffenceSite"></param>
        /// <returns>List<WT_OffenceType></returns>
        public List<WT_OffenceType> GetOffenceType(string _OffenceSite)
        {
            return db.Repository<WT_OffenceType>().GetAll().Where(ot => ot.ChannelOutlet.Trim().ToUpper() == _OffenceSite.Trim().ToUpper()).ToList();
        }

        /// <summary>
        /// This function executes the "SearchWaterTheftCases" stored procedure with
        /// the provided parameters.
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_DivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_CaseStatusID"></param>
        /// <param name="_AssignedToID"></param>
        /// <param name="_OffenceTypeID"></param>
        /// <param name="_OffenceSite"></param>
        /// <param name="_CaseID"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns>DataTable</returns>
        public IEnumerable<DataRow> GetWaterTheftCases(long _UserID, long? _IrrigationLevelID, long _DivisionID, long _ChannelID, long _CaseStatusID,
            long _AssignedToID, long _OffenceTypeID, string _OffenceSite, string _CaseID, DateTime? _FromDate, DateTime? _ToDate,
            string _EnteredCanalWireNo, long _CanalWireDesignationID)
        {
            return db.ExecuteDataSet("WT_SearchWaterTheftCases", _UserID, _IrrigationLevelID, _DivisionID, _ChannelID,
                _CaseStatusID, _AssignedToID, _OffenceTypeID, _OffenceSite, _CaseID, _FromDate, _ToDate, _EnteredCanalWireNo, _CanalWireDesignationID);
        }

        #endregion


        public object GetRelevantSBEInformation(long _ChannelID, decimal _ChannelRDs, long _UserId)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetRelevantSBEInformation(_ChannelID, _ChannelRDs, _UserId);
        }

        #region "Channel Water Theft Incident (Working)"
        /// <summary>
        /// This function return Channel water theft incident information
        /// Date: 10-05-2016
        /// </summary>
        /// <param name="_WaterTheftID"></param>
        /// <returns></returns>
        public dynamic GetChannelWaterTheftIncidentWorking(long _WaterTheftID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetChannelWaterTheftIncidentWorking(_WaterTheftID);
        }
        public int AssignWaterTheftCaseToSDO(WT_WaterTheftCanalWire _WTCanelWire, string _ClosingRepairDate)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().AssignWaterTheftCaseToSDO(_WTCanelWire, _ClosingRepairDate);
        }
        public int MarkWaterTheftCaseAsNA(WT_WaterTheftCanalWire _WTCanelWire)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().MarkWaterTheftCaseAsNA(_WTCanelWire);
        }
        /// <summary>
        /// This function return SBE working information
        /// Date: 13-05-2016
        /// </summary>
        /// <param name="_WaterTheftID"></param>
        /// <returns></returns>
        public List<dynamic> GetSBEWorkingInformation(long _WaterTheftID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetWTCaseWorkingInformation(_WaterTheftID);
        }
        public WT_WaterTheftCase GetWaterTheftCaseByID(long _WaterTheftID)
        {
            WT_WaterTheftCase qWaterTheftCase = (from WTC in db.Repository<WT_WaterTheftCase>().Query().Get()
                                                 where WTC.ID == _WaterTheftID
                                                 select WTC).FirstOrDefault();
            return qWaterTheftCase;
        }
        public int AssignWaterTheftCaseToSBEOrZiladar(WT_WaterTheftCanalWire _WTCanelWire)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().AssignWaterTheftCaseToSBEOrZiladar(_WTCanelWire);
        }

        public List<object> GetWaterTheftCaseWorkingRemarks(long _WaterTheftID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetWaterTheftCaseWorkingRemarks(_WaterTheftID);
        }
        public List<object> GetWaterTheftCaseAttachment(long _WaterTheftID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetWaterTheftCaseAttachment(_WaterTheftID);
        }
        public dynamic GetWaterTheftCaseAssignee(long _WaterTheftID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetWaterTheftCaseAssignee(_WaterTheftID);
        }
        public dynamic GetSBESDOWorkingByWaterTheftID(long _WaterTheftID, long _DesignationID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetSBESDOWorkingByWaterTheftID(_WaterTheftID, _DesignationID);
        }
        #endregion


        #region SDO Working

        public List<object> GetAllCanalWaireNo(long _WTCaseID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetAllCanalWaireNo(_WTCaseID);
        }

        public WT_WaterTheftStatus GetLastStatus(long _WTCaseID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetLastStatus(_WTCaseID);
        }

        public bool AssignCasetoZiledar(WT_WaterTheftStatus _ObjSave)
        {
            try
            {
                db.Repository<WT_WaterTheftStatus>().Insert(_ObjSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public int UpdateTawaanRecord(long? _UserID, long? _DesignationID, long? _CaseID, string _DecisionType, DateTime? _DecisionDate, int? _SpecialCharges, double? _ProposedFine, string _SDOLetterNo, DateTime _SDOLetterDate, string _FIRNo, DateTime _FIRDate, bool _Imprisonment, int? _ImprisonmentDays, string _CanalWireNo, DateTime _CanalWireDate, string _SDORemarks)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().UpdateTawaanRecord(_UserID, _DesignationID, _CaseID, _DecisionType, _DecisionDate, _SpecialCharges, _ProposedFine, _SDOLetterNo, _SDOLetterDate, _FIRNo, _FIRDate, _Imprisonment, _ImprisonmentDays, _CanalWireNo, _CanalWireDate, _SDORemarks);
        }

        public int? InsertCaseStatus(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, string _Remarks)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().InsertCaseStatus(_CaseID, _AssignedByUserID, _AssignedByDesignationID, _Remarks);
        }

        #endregion


        public long AddBreachIncidentCase(WT_Breach _mdlWaterTheftBreach)
        {
            try
            {
                db.Repository<WT_Breach>().Insert(_mdlWaterTheftBreach);
                db.Save();
                return _mdlWaterTheftBreach.ID;
            }

            catch (Exception ex)
            {
                return 0;
            }
        }


        public DataTable GetBreachBySearchCriteria(string _CaseID, DateTime? _FromDate, DateTime? _ToDate, long _UserId, long _IrrigationLevelID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetBreachSearchCriteria(_CaseID, _FromDate, _ToDate, _UserId, _IrrigationLevelID);
        }

        public WT_Breach GetBreachCaseDatebyID(long _BreachCaseId)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetBreachCaseDatebyID(_BreachCaseId);
        }
        public WT_FeettoIgnore FeetToIgnore()
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().FeetToIgnore();
        }


        #region Tawaan Working

        public object GetFineCalculation(long _WTCaseID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetFineCalculation(_WTCaseID);
        }

        #endregion

        public WT_WaterTheftCase GetWaterTheftID()
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetWaterTheftID();
        }

        #region SE Working
        public int AppealFromSE(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, long _CaseStatus, string _Remarks)
        {
            int Result = db.ExtRepositoryFor<WaterTheftRepository>().AppealFromSE(_CaseID, _AssignedByUserID, _AssignedByDesignationID, _CaseStatus, _Remarks);
            if (Result == 1)
            {
                WT_WaterTheftCase ObjWTCase = db.Repository<WT_WaterTheftCase>().GetAll().Where(q => q.ID == _CaseID).FirstOrDefault();
                ObjWTCase.CaseStatusID = _CaseStatus;
                db.Repository<WT_WaterTheftCase>().Update(ObjWTCase);
                db.Save();
            }
            return Result;
        }

        #endregion

        #region XEN Working

        public bool SaveSepcialCharges(long _WTCaseID, int? _SpecialCharges, DateTime _DecisionDate, bool _FromXEN)
        {
            try
            {
                WT_WaterTheftDecisionFineDetail ObjFineDetail = db.Repository<WT_WaterTheftDecisionFineDetail>().GetAll().Where(q => q.WaterTheftID == _WTCaseID).FirstOrDefault();
                ObjFineDetail.SpecialCharges = _SpecialCharges;
                if (_FromXEN)
                    ObjFineDetail.DecisionDate = _DecisionDate;
                //ObjFineDetail.DecisionDate = DateTime.Now;
                db.Repository<WT_WaterTheftDecisionFineDetail>().Update(ObjFineDetail);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool CaseClosed(long _WTCaseID, string _Comments)
        {
            try
            {
                WT_WaterTheftStatus ObjWaterTheft = db.Repository<WT_WaterTheftStatus>().GetAll().Where(q => q.WaterTheftID == _WTCaseID).OrderByDescending(w => w.AssignedDate).FirstOrDefault();

                WT_WaterTheftStatus SaveStatus = new WT_WaterTheftStatus();
                SaveStatus.WaterTheftID = _WTCaseID;
                SaveStatus.AssignedToUserID = ObjWaterTheft.AssignedToUserID;
                SaveStatus.AssignedByUserID = ObjWaterTheft.AssignedToUserID;
                SaveStatus.AssignedByDesignationID = ObjWaterTheft.AssignedToDesignationID;
                SaveStatus.AssignedToDesignationID = ObjWaterTheft.AssignedToDesignationID;
                SaveStatus.AssignedDate = DateTime.Now;
                SaveStatus.CaseStatusID = (Int64)Constants.WTCaseStatus.Closed;
                SaveStatus.Remarks = _Comments;
                db.Repository<WT_WaterTheftStatus>().Insert(SaveStatus);
                //db.Save();

                WT_WaterTheftCase ObjWTCase = db.Repository<WT_WaterTheftCase>().GetAll().Where(q => q.ID == _WTCaseID).FirstOrDefault();
                ObjWTCase.CaseStatusID = (Int64)Constants.WTCaseStatus.Closed;
                db.Repository<WT_WaterTheftCase>().Update(ObjWTCase);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public int? InsertCaseStatusForSubOrdinate(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, string _Remarks, long? _UserDesignation)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().InsertCaseStatusForSubOrdinate(_CaseID, _AssignedByUserID, _AssignedByDesignationID, _Remarks, _UserDesignation);
        }

        #endregion

        #region Chief Working

        public bool SaveFine(WT_ChiefAppealDetails _ObjSave)
        {
            try
            {
                WT_ChiefAppealDetails ObjAppeal = db.Repository<WT_ChiefAppealDetails>().GetAll().Where(q => q.WatertheftID == _ObjSave.WatertheftID).FirstOrDefault();

                if (ObjAppeal != null)
                {
                    ObjAppeal.FeeAmount = _ObjSave.FeeAmount;
                    db.Repository<WT_ChiefAppealDetails>().Update(ObjAppeal);
                }
                else
                {
                    db.Repository<WT_ChiefAppealDetails>().Insert(_ObjSave);
                }

                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        #endregion

        public List<object> GetOffenceType()
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetOffenceType();
        }

        public bool SaveOffenceType(WT_OffenceType _OffenceType)
        {
            if (_OffenceType.ID == 0)
                db.Repository<WT_OffenceType>().Insert(_OffenceType);
            else
                db.Repository<WT_OffenceType>().Update(_OffenceType);

            db.Save();
            return true;
        }

        public bool IsExistOffenceType(string _OffenceType)
        {
            bool qOTStatus = db.Repository<WT_OffenceType>().GetAll().Any(i => i.Name == _OffenceType);
            return qOTStatus;
        }
        public bool DeleteOffenceType(long _OffenceID)
        {
            db.Repository<WT_OffenceType>().Delete(_OffenceID);
            db.Save();
            return true;
        }

        public List<object> GetListOfAbiana()
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetListOfAbiana();
        }

        public bool SaveWTAbiana(WT_Abiana _abiana)
        {
            db.Repository<WT_Abiana>().Update(_abiana);
            db.Save();
            return true;
        }
        public bool UpdateFeetToIgnor(WT_FeettoIgnore _FeetToIgnor)
        {
            db.Repository<WT_FeettoIgnore>().Update(_FeetToIgnor);
            db.Save();
            return true;
        }

        public DataTable VerfiyChannelRDs(int _ChannelID, long _UserID, long _IrrigationLevelID)
        {
            DataTable lstChannelRDS = db.ExtRepositoryFor<WaterTheftRepository>().VerfiyChannelRDs(_ChannelID, _UserID, _IrrigationLevelID);
            return lstChannelRDS;
        }

        public int AddWaterTheftAttachments(DataTable _WTAttachments)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().AddWaterTheftAttachments(_WTAttachments);
        }

        public int SaveBreachAttachments(DataTable _WTAttachments)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().SaveBreachAttachments(_WTAttachments);
        }

        public List<object> GetBreachCaseAttachment(long _breachCaseId)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetBreachCaseAttachment(_breachCaseId);
        }

        public WT_Breach GetBreachCaseID()
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetBreachCaseID();
        }

        public bool IsBreachCaseExist(WT_Breach _mdlBreachCase, int _NoofFeet)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().IsBreachCaseExist(_mdlBreachCase, _NoofFeet);
        }

        public bool IsTheftTypeExist(long _ID)
        {
            bool qWTStatus = db.Repository<WT_WaterTheftCase>().GetAll().Any(i => i.OffenceTypeID == _ID);
            return qWTStatus;
        }

        #region Android Application

        /// Coded By : Hira Iqbal 
        public long AddWaterTheftChannel_Android(long _UserID, long _SectionID, long _ChannelID, int _SiteRD, string _ChannelSide, long _OffenceTypeID, long? _CutConditionID, string _Remarks, string _Attachments)
        {
            long caseID_WT = AddWaterTheftCase(_UserID, _ChannelID, null, _SiteRD, _ChannelSide, _OffenceTypeID, _CutConditionID, _Remarks, "C", null);
            if (caseID_WT > 0)
            {
                if (_Attachments != null)
                {
                    //linke attachments to water theft case id if there any
                    string[] lstAttachments = _Attachments.Split(',');
                    if (lstAttachments != null && lstAttachments.Length > 0)
                    {
                        foreach (string attchmntName in lstAttachments)
                        {
                            if (attchmntName.Length > 0)
                                AddWaterTheftCaseAttachment(caseID_WT, _UserID, "image/png", attchmntName);
                        }
                    }
                }
                //Assign case to SBE and update the relevant table
                AddWaterTheftCaseStatus(_UserID, _SectionID, caseID_WT);
                GenerateComplaint(_UserID, _SectionID);
                return caseID_WT;
            }
            return 0;
        }

        public void GenerateComplaint(long _UserID, long _SectionID)
        {
            long userDesignationID = Convert.ToInt64(new LoginDAL().GetAndroidUserDesignationID(_UserID));
            if (userDesignationID == Convert.ToInt64(Common.Constants.Designation.MA) || userDesignationID == Convert.ToInt64(Common.Constants.Designation.ADM))
            {
                long sbeID = AvailableSBEID(_UserID, _SectionID);
                //TODO : Complaint is to be generated
            }
        }

        public void AddWaterTheftCaseStatus(long _UserID, long _SectionID, long _CaseID)
        {
            long usrDsgnID = Convert.ToInt64(new LoginDAL().GetAndroidUserDesignationID(_UserID));
            long usrSBEID = AvailableSBEID(_UserID, _SectionID);

            WT_WaterTheftStatus mdlCaseStatus = new WT_WaterTheftStatus();
            mdlCaseStatus.WaterTheftID = _CaseID;
            mdlCaseStatus.AssignedToUserID = usrSBEID;
            mdlCaseStatus.AssignedToDesignationID = Convert.ToInt64(new LoginDAL().GetAndroidUserDesignationID(usrSBEID));
            mdlCaseStatus.AssignedByUserID = _UserID;
            mdlCaseStatus.AssignedByDesignationID = usrDsgnID;
            mdlCaseStatus.AssignedDate = DateTime.Now;
            mdlCaseStatus.CaseStatusID = Convert.ToInt32(Constants.WTCaseStatus.InProgress);
            mdlCaseStatus.Remarks = "Case started";
            mdlCaseStatus.IsActive = true;
            mdlCaseStatus.CreatedDate = DateTime.Now;
            mdlCaseStatus.CreatedBy = _UserID;

            db.Repository<WT_WaterTheftStatus>().Insert(mdlCaseStatus);
            db.Save();

        }

        public void AddWaterTheftCaseAttachment(long _WTCaseID, long _UserID, string _AttachmentType, string _AttachmentPath)
        {
            WT_WaterTheftAttachments mdlWTAtchmnt = new WT_WaterTheftAttachments();
            mdlWTAtchmnt.WaterTheftID = _WTCaseID;
            mdlWTAtchmnt.FileType = _AttachmentType;
            mdlWTAtchmnt.AttachmentPath = _AttachmentPath;
            mdlWTAtchmnt.AttachmentByUserID = _UserID;
            mdlWTAtchmnt.CreatedDate = DateTime.Now;
            mdlWTAtchmnt.CreatedBy = _UserID;
            mdlWTAtchmnt.AttachmentByDesignationID = new LoginDAL().GetAndroidUserDesignationID(_UserID);
            mdlWTAtchmnt.IsActive = true;

            db.Repository<WT_WaterTheftAttachments>().Insert(mdlWTAtchmnt);
            db.Save();
        }

        public long AddWaterTheftCase(long _UserID, long _ChannelID, long? _OutletID, int _SiteRD, string _ChannelSide, long? _OffenceTypeID, long? _CutConditionID, string _Remarks, string _SiteType, double? _ValueOfH)
        {
            WT_WaterTheftCase mdlWTCase = new WT_WaterTheftCase();

            mdlWTCase.OffenceSite = _SiteType;
            mdlWTCase.ChannelID = _ChannelID;
            mdlWTCase.TheftSiteRD = _SiteRD;
            mdlWTCase.OutletID = _OutletID;
            mdlWTCase.ValueofH = _ValueOfH;
            mdlWTCase.OffenceTypeID = _OffenceTypeID;
            mdlWTCase.OffenceSide = _ChannelSide;
            mdlWTCase.TheftSiteConditionID = _CutConditionID;
            mdlWTCase.IncidentDateTime = DateTime.Now;
            mdlWTCase.Source = "A";
            mdlWTCase.UserID = _UserID;
            mdlWTCase.LogDateTime = DateTime.Now;
            mdlWTCase.Remarks = _Remarks;
            mdlWTCase.CaseStatusID = GetWTCaseStatusIDByName("In Progress");
            mdlWTCase.CaseNo = GenerateWaterTheftCaseNo();
            mdlWTCase.IsActive = true;
            mdlWTCase.CreatedDate = DateTime.Now;
            mdlWTCase.CreatedBy = _UserID;

            db.Repository<WT_WaterTheftCase>().Insert(mdlWTCase);
            db.Save();

            return mdlWTCase.ID;
        }

        public long GetWTCaseStatusIDByName(string _Status)
        {
            return db.Repository<WT_Status>().Query().Get().Where(x => x.Name.Equals(_Status) && x.IsActive == true).SingleOrDefault().ID;
        }

        public string GenerateWaterTheftCaseNo()
        {
            WT_WaterTheftCase lstofCaseID = GetWaterTheftID();
            long? identityNo = 0;
            if (lstofCaseID == null)
            {
                lstofCaseID.ID = 1;
            }
            else
            {
                identityNo = lstofCaseID.ID + 1;
            }

            string caseNo = "";
            if (identityNo < 99)
            {
                caseNo = "WT0000" + identityNo;
            }
            if (identityNo > 99 && identityNo < 999)
            {
                caseNo = "WT000" + identityNo;
            }
            if (identityNo > 999 && identityNo < 9999)
            {
                caseNo = "WT00" + identityNo;
            }
            if (identityNo > 9999 && identityNo < 99999)
            {
                caseNo = "WT0" + identityNo;
            }
            if (identityNo > 99999 && identityNo < 999999)
            {
                caseNo = "WT" + identityNo;
            }

            return caseNo;
        }

        public DataTable RDsWithinSection(long _ChannelID, long _SectionID)
        {
            DataTable lstChannelRDS = db.ExtRepositoryFor<WaterTheftRepository>().RDsWithinSection(_ChannelID, _SectionID);
            return lstChannelRDS;
        }


        private long SBERoleID()
        {
            return
                db.Repository<UA_Roles>().Query().Get().Where(x => x.Name.ToUpper().Equals("SBE")).SingleOrDefault().ID;
        }

        public long AvailableSBEID(long _UserID, long _SectionID)
        {
            //Verify the current user is an SBE 
            UA_AssociatedLocation mdlLoc = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _UserID && x.IrrigationLevelID == 5 && x.IrrigationBoundryID == _SectionID).FirstOrDefault();
            if (mdlLoc == null)
            {
                //if Current user is not an SBE get the SBE for the section
                var q = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.IrrigationLevelID == 5 && x.IrrigationBoundryID == _SectionID).ToList().OrderByDescending(x => x.ID).ToList();
                if (q == null)
                    return 0;

                foreach (UA_AssociatedLocation loc in q)
                {
                    mdlLoc = loc;
                    if (mdlLoc == null)
                        return 0;
                    else
                    {
                        long ID = mdlLoc.UserID;
                        long? userDesig = db.Repository<UA_Users>().Query().Get().Where(x => x.ID == ID).SingleOrDefault().RoleID;
                        if (userDesig == SBERoleID())
                            return ID;
                    }
                }

            }
            else
            {
                long ID = mdlLoc.UserID;
                long? userDesig = db.Repository<UA_Users>().Query().Get().Where(x => x.ID == ID).SingleOrDefault().RoleID;
                if (userDesig == SBERoleID())
                    return ID;
                else
                {
                    //if Current user is not an SBE get the SBE for the section
                    var q = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.IrrigationLevelID == 5 && x.IrrigationBoundryID == _SectionID).ToList().OrderByDescending(x => x.ID).ToList();
                    if (q == null)
                        return 0;

                    foreach (UA_AssociatedLocation loc in q)
                    {
                        mdlLoc = loc;
                        if (mdlLoc == null)
                            return 0;
                        else
                        {
                            long IDUser = mdlLoc.UserID;
                            long? userDsg = db.Repository<UA_Users>().Query().Get().Where(x => x.ID == ID).SingleOrDefault().RoleID;
                            if (userDsg == SBERoleID())
                                return IDUser;
                        }
                    }
                }
            }
            return 0;
        }

        public bool WaterTheftOutletCaseAlreadyExist(WT_WaterTheftCase _OutletTheftCase)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().WaterTheftOutletCaseAlreadyExist(_OutletTheftCase);
        }

        public long AddWaterTheftOutlet_Android(long _UserID, long _SectionID, long _ChannelID, long _OutletID, int _SiteRD, long? _OffenceTypeID, long? _CutConditionID, string _Remarks, string _Attachments, double _ValueOfH, long? _DefectID, double? _ValueOfB, double? _ValueOfY, double? _ValueOfDIA, string _ChannelSide)
        {
            long caseID_WT = AddWaterTheftCase(_UserID, _ChannelID, _OutletID, _SiteRD, _ChannelSide, _OffenceTypeID, _CutConditionID, _Remarks, "O", _ValueOfH);
            if (caseID_WT > 0)
            {
                //linke attachments to water theft case id if there any
                if (_Attachments != null && _Attachments.Length > 0)
                {
                    string[] lstAttachments = _Attachments.Split(',');
                    if (lstAttachments != null && lstAttachments.Length > 0)
                    {
                        foreach (string attchmntName in lstAttachments)
                        {
                            if (attchmntName.Length > 0)
                                AddWaterTheftCaseAttachment(caseID_WT, _UserID, "image/png", attchmntName);
                        }
                    }
                }

                //enter theft details
                if (_DefectID != null)
                {
                    AddOutletTheftDetails(_UserID, caseID_WT, Convert.ToInt64(_DefectID), _ValueOfB, _ValueOfY, _ValueOfDIA);
                }

                //Assign case to SBE and update the relevant table
                AddWaterTheftCaseStatus(_UserID, _SectionID, caseID_WT);

                GenerateComplaint(_UserID, _SectionID);
                return caseID_WT;
            }
            return 0;
        }

        public void AddOutletTheftDetails(long _UserID, long _TheftCaseID, long _DefectID, double? _ValueOfB, double? _ValueOfY, double? _ValueOfDIA)
        {
            WT_OutletDefectiveDetails mdlDfctDtl = new WT_OutletDefectiveDetails();
            mdlDfctDtl.DefectiveTypeID = _DefectID;
            mdlDfctDtl.IsActive = true;
            mdlDfctDtl.CreatedDate = DateTime.Now;
            mdlDfctDtl.CreatedBy = _UserID;
            mdlDfctDtl.ValueOfB = _ValueOfB;
            mdlDfctDtl.ValueOfY = _ValueOfY;
            mdlDfctDtl.ValueOfDia = _ValueOfDIA;
            mdlDfctDtl.WaterTheftID = _TheftCaseID;


            db.Repository<WT_OutletDefectiveDetails>().Insert(mdlDfctDtl);
            db.Save();

        }

        public long AddBreach_Android(long _UserID, long _SectionID, long _ChannelID, int _BreachSiteRD, string _BreachSide, double? _HeadDischarge, double? _BreachLength, bool? _FieldStaff, string _Remarks, string _Attachments, double? _Longitude, double? _Latitude, string _Source, DateTime breachdt)
        {
            long caseID_WT = -1;
            caseID_WT = AddBreachCase(_UserID, _SectionID, _ChannelID, _BreachSiteRD, _BreachSide, _HeadDischarge, _BreachLength, _FieldStaff, _Remarks, _Attachments, _Longitude, _Latitude, _Source, breachdt);
            if (caseID_WT > 0)
            {
                //linke attachments to water theft case id if there any
                string[] lstAttachments = _Attachments.Split(',');
                if (lstAttachments != null && lstAttachments.Length > 0)
                {
                    foreach (string attchmntName in lstAttachments)
                    {
                        if (attchmntName.Length > 0)
                            AddBreachCaseAttachment(caseID_WT, _UserID, "image/png", attchmntName, _Longitude, _Latitude, _Source);
                    }
                }
               
            }
            return caseID_WT;
        }

        public string GenerateBreachCaseNo()
        {
            WT_Breach lstofCaseID = GetBreachCaseID();
            long? identityNo = 0;
            if (lstofCaseID == null)
            {
                lstofCaseID = new WT_Breach();
                lstofCaseID.ID = 1;
            }
            else
            {
                identityNo = lstofCaseID.ID + 1;
            }

            string caseNo = "";
            if (identityNo < 99)
            {
                caseNo = "BR0000" + identityNo;
            }
            if (identityNo > 99 && identityNo < 999)
            {
                caseNo = "BR000" + identityNo;
            }
            if (identityNo > 999 && identityNo < 9999)
            {
                caseNo = "BR00" + identityNo;
            }
            if (identityNo > 9999 && identityNo < 99999)
            {
                caseNo = "BR0" + identityNo;
            }
            if (identityNo > 99999 && identityNo < 999999)
            {
                caseNo = "BR" + identityNo;
            }

            return caseNo;
        }

        public long AddBreachCase(long _UserID, long _SectionID, long _ChannelID, int _BreachSiteRD, string _BreachSide, double? _HeadDischarge, double? _BreachLength, bool? _FieldStaff, string _Remarks, string _Attachments, double? _Longitude, double? _Latitude, string _Source,  DateTime breachdt)
        {
            WT_Breach mdlBreachCase = new WT_Breach();

            mdlBreachCase.ChannelID = _ChannelID;
            mdlBreachCase.BreachSiteRD = _BreachSiteRD;
            mdlBreachCase.BreachSide = _BreachSide;
            mdlBreachCase.DateTime = breachdt;
            mdlBreachCase.Source = _Source;
            //mdlBreachCase.LogBy = "";
            mdlBreachCase.UserID = _UserID;
            mdlBreachCase.LogDateTime = breachdt;
            //mdlBreachCase.SelectedEntry 
            mdlBreachCase.FieldStaff = _FieldStaff;
            mdlBreachCase.Remarks = _Remarks;
            mdlBreachCase.IsActive = true;
            mdlBreachCase.CreatedDate = DateTime.Now;
            mdlBreachCase.CreatedBy = _UserID;
            mdlBreachCase.BreachLength = _BreachLength;
            mdlBreachCase.HeadDischarge = _HeadDischarge;
            mdlBreachCase.CaseNo = GenerateBreachCaseNo();
            mdlBreachCase.DivisionID = GetDivisionIDBySection(_SectionID);
            mdlBreachCase.GIS_X = _Longitude;
            mdlBreachCase.GIS_Y = _Latitude;
            db.Repository<WT_Breach>().Insert(mdlBreachCase);
            db.Save();

            return mdlBreachCase.ID;
        }

        public long? GetDivisionIDBySection(long _SectionID)
        {
            long? subDivID = db.Repository<CO_Section>().Query().Get().Where(x => x.ID == _SectionID).SingleOrDefault().SubDivID;
            if (subDivID != null)
            {
                long subDiv = Convert.ToInt64(subDivID);
                return db.Repository<CO_SubDivision>().Query().Get().Where(x => x.ID == subDiv).SingleOrDefault().DivisionID;
            }
            else
                return null;
        }

        public void AddBreachCaseAttachment(long _BreachCaseID, long _UserID, string _AttachmentType, string _AttachmentPath, double? _Longitude, double? _Latitude, string _Source)
        {
            WT_BreachAttachments mdlBreachAtchmnt = new WT_BreachAttachments();
            mdlBreachAtchmnt.BreachID = _BreachCaseID;
            mdlBreachAtchmnt.FileType = _AttachmentType;
            mdlBreachAtchmnt.AttachmentPath = _AttachmentPath;
            mdlBreachAtchmnt.FileName = _AttachmentPath;
            mdlBreachAtchmnt.AttachmentByUserID = _UserID;
            mdlBreachAtchmnt.CreatedDate = DateTime.Now;
            mdlBreachAtchmnt.CreatedBy = _UserID;
            mdlBreachAtchmnt.AttachmentByDesignationID = new LoginDAL().GetAndroidUserDesignationID(_UserID);
            mdlBreachAtchmnt.IsActive = true;
            //mdlBreachAtchmnt.GIS_X = _Longitude;
            //mdlBreachAtchmnt.GIS_Y = _Latitude;
            //mdlBreachAtchmnt.Source = _Source;
            db.Repository<WT_BreachAttachments>().Insert(mdlBreachAtchmnt);
            db.Save();
        }

        /// Coded By : Hira Iqbal
        #endregion

        #region Notifications

        public object GetInformationbyCaseNo(String _WTCaseID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetInformationbyCaseNo(_WTCaseID);
        }

        #endregion

        public UA_RoleRights GetRoleRights(long _UserID, long _PageID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetRoleRights(_UserID, _PageID);
        }

    }
}
