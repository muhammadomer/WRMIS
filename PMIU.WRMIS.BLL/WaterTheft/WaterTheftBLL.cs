using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.WaterTheft;
using PMIU.WRMIS.DAL.Repositories.WaterTheft;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.WaterTheft
{
    public class WaterTheftBLL : BaseBLL
    {
        WaterTheftDAL dalWaterTheft = new WaterTheftDAL();

        #region WaterTheft Case Outlet and channel
        public List<object> GetChannelByUserId(long _UserID, long _IrrigationLevelID)
        {
            return dalWaterTheft.GetChannelByUserId(_UserID, _IrrigationLevelID);
        }
        public List<object> GetChannelByDivisionID(long _DivisionID)
        {
            return dalWaterTheft.GetChannelByDivisionID(_DivisionID);
        }
        public List<object> GetOutletByChannelId(long _ChannelID, long _UserID, long _IrrigationLevelID)
        {
            return dalWaterTheft.GetOutletByChannelId(_ChannelID, _UserID, _IrrigationLevelID);
        }
        public List<object> GetAssetWorkOutletByChannelID(long _DivisionID)
        {
            return dalWaterTheft.GetAssetWorkOutletByChannelID(_DivisionID);
        }
        public object GetDatabyOutletId(int _OutletID)
        {
            return dalWaterTheft.GetDatabyOutletId(_OutletID);
        }

        public bool IsWaterTheftCaseExist(WT_WaterTheftCase _mdlWaterTheftCase, int _NoofFeet)
        {

            return dalWaterTheft.IsWaterTheftCaseExist(_mdlWaterTheftCase, _NoofFeet);

        }

        public List<object> GetAllTheftType(string _OffenceSiteId)
        {

            return dalWaterTheft.GetAllTheftType(_OffenceSiteId);

        }

        public List<object> GetOutletCondition(string _OffenceSiteId)
        {

            return dalWaterTheft.GetOutletCondition(_OffenceSiteId);

        }
        public List<object> GetDefectiveType()
        {

            return dalWaterTheft.GetDefectiveType();

        }

        public object GetRelevantSBE(long _ChannelID, decimal _ChannelRDs, long _UserId)
        {
            return dalWaterTheft.GetRelevantSBEInformation(_ChannelID, _ChannelRDs, _UserId);
        }
        #endregion


        public int AddOutletChannelIncidentCase(WT_WaterTheftCase _mdlWatertheftCase, WT_WaterTheftStatus _mdlWaterTheftStaus, WT_OutletDefectiveDetails _mdloutletDefectiveDetails)
        {
            int WaterTheftID = dalWaterTheft.AddOutletChannelIncidentCase(_mdlWatertheftCase, _mdlWaterTheftStaus, _mdloutletDefectiveDetails);
            return WaterTheftID;
        }

        //public long AddOutletDefectiveTypeDetails(WT_OutletDefectiveDetails _mdlOutletDefectiveDetails)
        //{
        //    return dalWaterTheft.AddOutletDefectiveTypeDetails(_mdlOutletDefectiveDetails);
        //}


   

        #region Ziledar Outlet Working

        public object GetZiladaarWorkingParentInformation(long _CaseID)
        {
            return dalWaterTheft.GetZiladaarWorkingParentInformation(_CaseID);
        }

        public List<WT_WaterTheftOffender> GetOffenders(long _CaseID)
        {
            return dalWaterTheft.GetOffenders(_CaseID);
        }

        public bool DeleteOffender(long _DeleteID, long _WTCaseID)
        {
            return dalWaterTheft.DeleteOffender(_DeleteID, _WTCaseID);
        }

        public bool AddOffender(WT_WaterTheftOffender _ObjSave)
        {
            return dalWaterTheft.AddOffender(_ObjSave);
        }

        public bool UpdateOffender(WT_WaterTheftOffender _ObjUpdate)
        {
            return dalWaterTheft.UpdateOffender(_ObjUpdate);
        }

        public List<WT_AreaType> GetAreaTypes()
        {
            return dalWaterTheft.GetAreaTypes();
        }

        public bool AddWaterTheftDecisionFineDetail(WT_WaterTheftDecisionFineDetail _ObjSave)
        {
            return dalWaterTheft.AddWaterTheftDecisionFineDetail(_ObjSave);
        }

        public bool AddZiladaarPoliceCase(WT_WaterTheftPoliceCase _ObjSave)
        {
            return dalWaterTheft.AddZiladaarPoliceCase(_ObjSave);
        }

        public bool AddWTStatus(long _WTCaseID, long _UserID, long _UserDesID)
        {
            return dalWaterTheft.AddWTStatus(_WTCaseID, _UserID, _UserDesID);
        }


        public int? SaveZiladaarWorking(long? _UserID, long? _DesignationID, long? _CaseID, double? _AreaBooked, long? _AreaUnit, int? _NoOfAccused, DateTime _ProcedingDate, string _ZildarRemarks)
        {
            return dalWaterTheft.SaveZiladaarWorking(_UserID, _DesignationID, _CaseID, _AreaBooked, _AreaUnit, _NoOfAccused, _ProcedingDate, _ZildarRemarks);
        }

        public object GetSavedZiladaarWorking(long _WTCaseID)
        {
            return dalWaterTheft.GetSavedZiladaarWorking(_WTCaseID);
        }

        public long GetCaseStatus(long _WTCaseID)
        {
            return dalWaterTheft.GetCaseStatus(_WTCaseID);
        }


        public long? GetManagerSubOrdinate(long _ManagerID, long _SubDesID)
        {
            return db.ExtRepositoryFor<WaterTheftRepository>().GetManagerSubOrdinate(_ManagerID, _SubDesID);
        }

        #endregion



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
            return dalWaterTheft.GetDivisionsByUserID(_UserID, _IrrigationLevelID, _IsActive);
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
            return dalWaterTheft.GetChannelsByUserIDAndDivisionID(_UserID, _IrrigationLevelID, _DivisionID, _IsActive);
        }

        /// <summary>
        /// This function fetches Water Theft Status
        /// Created On 10-05-2016
        /// </summary>
        /// <returns>List<WT_Status></returns>
        public List<WT_Status> GetStatus()
        {
            return dalWaterTheft.GetStatus();
        }

        /// <summary>
        /// This function fetches Water Theft Offence Type
        /// Created On 10-05-2016
        /// </summary>
        /// <param name="_OffenceSite"></param>
        /// <returns>List<WT_OffenceType></returns>
        public List<WT_OffenceType> GetOffenceType(string _OffenceSite)
        {
            return dalWaterTheft.GetOffenceType(_OffenceSite);
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
            return dalWaterTheft.GetWaterTheftCases(_UserID, _IrrigationLevelID, _DivisionID, _ChannelID, _CaseStatusID, _AssignedToID,
                _OffenceTypeID, _OffenceSite, _CaseID, _FromDate, _ToDate, _EnteredCanalWireNo, _CanalWireDesignationID);
        }

        #endregion

      
        #region "Channel Water Theft Incident (Working)"
        /// <summary>
        /// This function return Channel water theft incident information
        /// Date: 10-05-2016
        /// </summary>
        /// <param name="_WaterTheftID"></param>
        /// <returns></returns>
        public dynamic GetChannelWaterTheftIncidentWorking(long _WaterTheftID)
        {
            return dalWaterTheft.GetChannelWaterTheftIncidentWorking(_WaterTheftID);
        }
        public int AssignWaterTheftCaseToSDO(WT_WaterTheftCanalWire _WTCanelWire, string _ClosingRepairDate)
        {
            return dalWaterTheft.AssignWaterTheftCaseToSDO(_WTCanelWire, _ClosingRepairDate);
        }
        public int MarkWaterTheftCaseAsNA(WT_WaterTheftCanalWire _WTCanelWire)
        {
            return dalWaterTheft.MarkWaterTheftCaseAsNA(_WTCanelWire);
        }
        /// <summary>
        /// This function return SBE working information
        /// Date: 13-05-2016
        /// </summary>
        /// <param name="_WaterTheftID"></param>
        /// <returns></returns>
        public List<dynamic> GetSBEWorkingInformation(long _WaterTheftID)
        {
            return dalWaterTheft.GetSBEWorkingInformation(_WaterTheftID);
        }
        public int AssignWaterTheftCaseToSBEOrZiladar(WT_WaterTheftCanalWire _WTCanelWire)
        {
            return dalWaterTheft.AssignWaterTheftCaseToSBEOrZiladar(_WTCanelWire);
        }
        public List<object> GetWaterTheftCaseWorkingRemarks(long _WaterTheftID)
        {
            return dalWaterTheft.GetWaterTheftCaseWorkingRemarks(_WaterTheftID);
        }
        public List<object> GetWaterTheftCaseAttachment(long _WaterTheftID)
        {
            return dalWaterTheft.GetWaterTheftCaseAttachment(_WaterTheftID);
        }
        public dynamic GetWaterTheftCaseAssignee(long _WaterTheftID)
        {
            return dalWaterTheft.GetWaterTheftCaseAssignee(_WaterTheftID);
        }
        public dynamic GetSBESDOWorkingByWaterTheftID(long _WaterTheftID, long _DesignationID)
        {
            return dalWaterTheft.GetSBESDOWorkingByWaterTheftID(_WaterTheftID, _DesignationID);
        }
        #endregion


        #region SDO Working

        public List<object> GetAllCanalWaireNo(long _WTCaseID)
        {
            return dalWaterTheft.GetAllCanalWaireNo(_WTCaseID);
        }

        public WT_WaterTheftStatus GetLastStatus(long _WTCaseID)
        {
            return dalWaterTheft.GetLastStatus(_WTCaseID);
        }

        public bool AssignCasetoZiledar(WT_WaterTheftStatus _ObjSave)
        {
            return dalWaterTheft.AssignCasetoZiledar(_ObjSave);
        }

        public int UpdateTawaanRecord(long? _UserID, long? _DesignationID, long? _CaseID, string _DecisionType, DateTime? _DecisionDate, int? _SpecialCharges, double? _ProposedFine, string _SDOLetterNo, DateTime _SDOLetterDate, string _FIRNo, DateTime _FIRDate, bool _Imprisonment, int? _ImprisonmentDays, string _CanalWireNo, DateTime _CanalWireDate, string _SDORemarks)
        {
            return dalWaterTheft.UpdateTawaanRecord(_UserID, _DesignationID, _CaseID, _DecisionType, _DecisionDate, _SpecialCharges, _ProposedFine, _SDOLetterNo, _SDOLetterDate, _FIRNo, _FIRDate, _Imprisonment, _ImprisonmentDays, _CanalWireNo, _CanalWireDate, _SDORemarks);
        }

        public int? InsertCaseStatus(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, string _Remarks)
        {
            return dalWaterTheft.InsertCaseStatus(_CaseID, _AssignedByUserID, _AssignedByDesignationID, _Remarks);
        }

        #endregion


        #region Breach Case
        public long AddBreachIncidentCase(WT_Breach _mdlWaterTheftBreach)
        {
            return dalWaterTheft.AddBreachIncidentCase(_mdlWaterTheftBreach);
        }

        public DataTable GetBreachBySearchCriteria(string _CaseID, DateTime? _FromDate, DateTime? _ToDate, long _UserId, long _IrrigationLevelID)
        {
            return dalWaterTheft.GetBreachBySearchCriteria(_CaseID, _FromDate, _ToDate, _UserId, _IrrigationLevelID);
        }

        public WT_Breach GetBreachCaseDatebyID(long _BreachCaseId)
        {
            return dalWaterTheft.GetBreachCaseDatebyID(_BreachCaseId);
        }

        #endregion
        
        public WT_FeettoIgnore FeetToIgnore()
        {
            return dalWaterTheft.FeetToIgnore();
        }

        #region Tawaan Working

        public object GetFineCalculation(long _WTCaseID)
        {
            return dalWaterTheft.GetFineCalculation(_WTCaseID);
        }

        #endregion

        #region SE Working

        public int AppealFromSE(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, long _CaseStatus, string _Remarks)
        {
            return dalWaterTheft.AppealFromSE(_CaseID, _AssignedByUserID, _AssignedByDesignationID, _CaseStatus, _Remarks);
        }


        #endregion

        public WT_WaterTheftCase GetWaterTheftID()
        {
            return dalWaterTheft.GetWaterTheftID();
        }

        #region XEN Working

        public bool SaveSepcialCharges(long _WTCaseID, int? _SpecialCharges, DateTime _DecisionDate, bool _FromXEN)
        {
            return dalWaterTheft.SaveSepcialCharges(_WTCaseID, _SpecialCharges, _DecisionDate, _FromXEN);
        }

        public int? InsertCaseStatusForSubOrdinate(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, string _Remarks, long? _UserDesignation)
        {
            return dalWaterTheft.InsertCaseStatusForSubOrdinate(_CaseID, _AssignedByUserID, _AssignedByDesignationID, _Remarks, _UserDesignation);
        }

        public bool CaseClosed(long _WTCaseID, string _Comments)
        {
            return dalWaterTheft.CaseClosed(_WTCaseID, _Comments);
        }


        #endregion

        #region Chief Working

        public bool SaveFine(WT_ChiefAppealDetails _ObjSave)
        {
            return dalWaterTheft.SaveFine(_ObjSave);
        }

        #endregion


        #region Reference Data
        public List<object> GetOffenceType()
        {
            return dalWaterTheft.GetOffenceType();
        }

        public bool SaveOffenceType(WT_OffenceType _OffenceType)
        {
            return dalWaterTheft.SaveOffenceType(_OffenceType);
        }

        public bool IsExistOffenceType(string _OffenceType)
        {
            return dalWaterTheft.IsExistOffenceType(_OffenceType);
        }
        public bool DeleteOffenceType(long _OffenceID)
        {
            return dalWaterTheft.DeleteOffenceType(_OffenceID);
        }

        public List<object> GetListOfAbiana()
        {
            return dalWaterTheft.GetListOfAbiana();
        }

        public bool SaveWTAbiana(WT_Abiana _abiana)
        {
            return dalWaterTheft.SaveWTAbiana(_abiana);
        }

        public bool UpdateFeetToIgnor(WT_FeettoIgnore _NoOfFeet)
        {
            return dalWaterTheft.UpdateFeetToIgnor(_NoOfFeet);
        }
        #endregion
      

        public DataTable VerfiyChannelRDs(int _ChannelID, long _UserID, long _IrrigationLevelID)
        {
            return dalWaterTheft.VerfiyChannelRDs(_ChannelID, _UserID, _IrrigationLevelID);
        }

        public int AddWaterTheftAttachments(DataTable _WTAttachments)
        {
            return dalWaterTheft.AddWaterTheftAttachments(_WTAttachments);
        }

        public int SaveBreachAttachments(DataTable _WTAttachments)
        {
            return dalWaterTheft.SaveBreachAttachments(_WTAttachments);
        }

        public List<object> GetBreachCaseAttachment(long _breachCaseId)
        {
            return dalWaterTheft.GetBreachCaseAttachment(_breachCaseId);
        }
        public WT_Breach GetBreachCaseID()
        {
            return dalWaterTheft.GetBreachCaseID();
        }

        public bool IsBreachCaseExist(WT_Breach _mdlBreachCase, int _NoofFeet)
        {

            return dalWaterTheft.IsBreachCaseExist(_mdlBreachCase, _NoofFeet);

        }

        public bool IsTheftTypeExist(long _ID)
        {
            return dalWaterTheft.IsTheftTypeExist(_ID);
        }

        #region Android Application
        public bool RDLiesWithinUserJurisdiction(long _UserID, long _SectionID, long _ChannelID, int _SiteRD)
        {
            LoginBLL bllLogin = new LoginBLL();
            long? userDsgnID = bllLogin.GetAndroidUserDesignationID(_UserID);
            if (userDsgnID == null)
                return false;

            long? irrLvlID = bllLogin.GetIrrigationLevelID(Convert.ToInt64(userDsgnID));
            if (irrLvlID == null)
                return false;

            if (bllLogin.IsLocationAssigned(_UserID, _SectionID, Convert.ToInt64(irrLvlID)))
            {
                DataTable lstRDs = dalWaterTheft.RDsWithinSection(_ChannelID, _SectionID);
                if (lstRDs.Rows.Count > 0)
                {
                    int MinRDs = lstRDs.AsEnumerable().Min(r => r.Field<int>("MinRD"));
                    int MaxRDs = lstRDs.AsEnumerable().Max(r => r.Field<int>("MaxRD"));

                    if (_SiteRD >= MinRDs && _SiteRD <= MaxRDs)
                        return true;
                }
            }

            return false;
        }

        public long AvailableSBEID(long _UserID, long _SectionID)
        {
            return dalWaterTheft.AvailableSBEID(_UserID, _SectionID);

        }

        public bool WaterTheftCaseAlreadyExist(long _ChannelID, int _SiteRD, string _ChannelSide, long _OffenceTypeID)
        {
            WT_WaterTheftCase mdlCase = new WT_WaterTheftCase();
            mdlCase.IncidentDateTime = DateTime.Now;
            mdlCase.ChannelID = _ChannelID;
            mdlCase.TheftSiteRD = _SiteRD;
            mdlCase.OffenceSide = _ChannelSide;
            mdlCase.OffenceTypeID = _OffenceTypeID;

            WT_FeettoIgnore lstFeetToIgnor = new WaterTheftBLL().FeetToIgnore();
            int NoofFeet = lstFeetToIgnor.NoOfFeet;

            return dalWaterTheft.IsWaterTheftCaseExist(mdlCase, NoofFeet);
        }

        public long AddWaterTheftChannel_Android(long _UserID, long _SectionID, long _ChannelID, int _SiteRD, string _ChannelSide, long _OffenceTypeID, long? _CutConditionID, string _Remarks, string _Attachments)
        {
            long result = dalWaterTheft.AddWaterTheftChannel_Android(_UserID, _SectionID, _ChannelID, _SiteRD, _ChannelSide, _OffenceTypeID, _CutConditionID, _Remarks, _Attachments);

            return result;

        }

        public List<object> GetOffenceTypeBySite(string _SiteType)
        {
            return dalWaterTheft.GetOffenceType(_SiteType).Select(x => new { x.ID, x.Name }).ToList<object>();
        }


        public bool WaterTheftOutletCaseAlreadyExist(long _ChannelID, long _OutletID, long? _ConditionID, long? _OffenceTypeID)
        {
            WT_WaterTheftCase mdlCase = new WT_WaterTheftCase();
            mdlCase.IncidentDateTime = DateTime.Now;
            mdlCase.ChannelID = _ChannelID;
            mdlCase.OutletID = _OutletID;
            mdlCase.TheftSiteConditionID = _ConditionID;
            mdlCase.OffenceTypeID = _OffenceTypeID;

            return dalWaterTheft.WaterTheftOutletCaseAlreadyExist(mdlCase);
        }

        public long AddWaterTheftOutlet_Android(long _UserID, long _SectionID, long _ChannelID, long _OutletID, int _SiteRD, long? _OffenceTypeID, long? _CutConditionID, string _Remarks, string _Attachments, double _ValueOfH, long? _DefectID, double? _ValueOfB, double? _ValueOfY, double? _ValueOfDIA, string _ChannelSide)
        {
            return dalWaterTheft.AddWaterTheftOutlet_Android(_UserID, _SectionID, _ChannelID, _OutletID, _SiteRD, _OffenceTypeID, _CutConditionID, _Remarks, _Attachments, _ValueOfH, _DefectID, _ValueOfB, _ValueOfY, _ValueOfDIA, _ChannelSide);
        }

        public long AddBreach_Android(long _UserID, long _SectionID, long _ChannelID, int _BreachSiteRD, string _BreachSide, double? _HeadDischarge, double? _BreachLength, bool? _FieldStaff,string _Remarks, string _Attachments, double? _Longitude, double? _Latitude, string _Source,  DateTime breachdt)
        {
            long BreachCaseID = dalWaterTheft.AddBreach_Android(_UserID, _SectionID, _ChannelID, _BreachSiteRD, _BreachSide, _HeadDischarge, _BreachLength, _FieldStaff, _Remarks, _Attachments, _Longitude, _Latitude, _Source, breachdt);
            if (BreachCaseID > 0) {
                PMIU.WRMIS.BLL.Notifications.NotifyEvent _event = new PMIU.WRMIS.BLL.Notifications.NotifyEvent();
                _event.Parameters.Add("BreachID", BreachCaseID);
                _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.AddBreachCase, _UserID);
            }
            return BreachCaseID;
        }

        public bool BreachCaseAlreadyExist(long _ChannelID, int _SiteRD, string _BreachSide)
        {
            WT_Breach mdlCase = new WT_Breach();
            mdlCase.DateTime = DateTime.Now;
            mdlCase.ChannelID = _ChannelID;
            mdlCase.BreachSiteRD = _SiteRD;
            mdlCase.BreachSide = _BreachSide;

            WT_FeettoIgnore lstFeetToIgnor = new WaterTheftBLL().FeetToIgnore();
            int NoofFeet = lstFeetToIgnor.NoOfFeet;

            return dalWaterTheft.IsBreachCaseExist(mdlCase, NoofFeet);
        }
        #endregion

        #region  Notifications

        public object GetInformationbyCaseNo(String _WTCaseID)
        {
            return dalWaterTheft.GetInformationbyCaseNo(_WTCaseID);
        }

        #endregion

        #region Add WaterTheft Case from Web & Mobile
       public long AddWaterTheftCaseChannelorOutlet(long _UserID, WT_WaterTheftCase _mdlWatertheftCase, WT_WaterTheftStatus _mdlWaterTheftCaseStatus,
            WT_OutletDefectiveDetails _mdloutletDefectiveDetails, long _DivisionID, long _ChannelorOutlet, List<Tuple<string, string, string>> lstNameofFiles,
            string RefCode, Double GIS_X = 0, Double GIS_Y = 0, String Source = "W", string ChannelName = "")
        {
            long Channel = 1;
            string LeftRD = string.Empty;
            string RightRD = string.Empty;
            UA_Users mdlUser = db.Repository<UA_Users>().GetAll().FirstOrDefault(e => e.ID == _UserID);
            if (Channel == _ChannelorOutlet)
            {
                //Verify the Channel RDs
                int Status = VerfiyChannelRDs(_UserID, _mdlWatertheftCase);
                if (Status == 0)
                {
                    return -1;
                }
            }
            if (_mdlWaterTheftCaseStatus.AssignedToUserID == 0 && _mdlWatertheftCase.TheftSiteConditionID!=6)
            {
                return -2;
            }

            WT_FeettoIgnore lstFeetToIgnor = FeetToIgnore();
            int NoofFeet = lstFeetToIgnor.NoOfFeet;
            bool IsCaseExist = IsWaterTheftCaseExist(_mdlWatertheftCase, NoofFeet);
            //int Count = 1;
            // If Case is already exist
            if (IsCaseExist)
            {
                return -3;
            }
            _mdlWatertheftCase.GIS_X = GIS_X;
            _mdlWatertheftCase.GIS_Y = GIS_Y;
            long WaterTheftID = AddOutletChannelIncidentCase(_mdlWatertheftCase, _mdlWaterTheftCaseStatus, _mdloutletDefectiveDetails);
            if (WaterTheftID == 0)
            {
                return -4;
            }
            DataTable WTAttachmentTable = GetDataTable(WaterTheftID, mdlUser.ID, Convert.ToInt64(mdlUser.DesignationID), lstNameofFiles, Source, GIS_X, GIS_Y);

            if (IsCaseExist == false && mdlUser.DesignationID == (long)Constants.Designation.MA || IsCaseExist == false && mdlUser.DesignationID == (long)Constants.Designation.ADM)
            {
                ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
                string RDs = _mdlWatertheftCase.TheftSiteRD.ToString();
                if (!string.IsNullOrEmpty(RDs))
                {
                    Tuple<string, string> tupleRD = Calculations.GetRDValues(Convert.ToInt64(RDs));
                    LeftRD = tupleRD.Item1;
                    RightRD = tupleRD.Item2;
                }
                int StatusID = new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);



                if (_mdlWatertheftCase.TheftSiteConditionID != null && _mdlWatertheftCase.TheftSiteConditionID != (long)Constants.TheftSiteCondition.OK)
                {
                    String ComplaintID = string.Empty;
                    String ComplaintDetail = "WaterTheft Case log on channel: " + ChannelName + " and at RDs: " + LeftRD + " + " + RightRD;
                    ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaint(mdlUser.ID, RefCode, _DivisionID, WaterTheftID, Convert.ToInt64(_mdlWatertheftCase.ChannelID), ComplaintDetail);
                    Notifications.NotifyEvent _event = new Notifications.NotifyEvent();
                    _event.Parameters.Add("WaterTheftID", WaterTheftID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.AddWaterTheftCase, _UserID);
                }
               
            }


            else
            {
                int StatusID = new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);
                if (_mdlWatertheftCase.TheftSiteConditionID != null && _mdlWatertheftCase.TheftSiteConditionID != (long)Constants.TheftSiteCondition.OK)
                {
                    Notifications.NotifyEvent _event = new Notifications.NotifyEvent();
                    _event.Parameters.Add("WaterTheftID", WaterTheftID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.AddWaterTheftCase, _UserID);
                }
            }

            return WaterTheftID;

        }


        protected int VerfiyChannelRDs(long _UserID, WT_WaterTheftCase _mdlWatertheftCase)
        {
            UA_Users mdlUser = db.Repository<UA_Users>().GetAll().FirstOrDefault(e => e.ID == _UserID);
            long UserId = mdlUser.ID;
            int MinRDs;
            int MaxRDs;
            if (mdlUser.DesignationID != null && mdlUser.UA_Designations.IrrigationLevelID == null)
                return 1;
            long IrrigationLevelID = (long)mdlUser.UA_Designations.IrrigationLevelID;
            int ChannelId = Convert.ToInt32(_mdlWatertheftCase.ChannelID);
            DataTable ChannelRDs = new WaterTheftBLL().VerfiyChannelRDs(ChannelId, UserId, IrrigationLevelID);
            if (ChannelRDs == null)
            {
                MinRDs = 0;
                MaxRDs = 0;
            }
            Decimal ChannelRD = _mdlWatertheftCase.TheftSiteRD;
            MinRDs = ChannelRDs.AsEnumerable().Min(r => r.Field<int>("MinRD"));
            MaxRDs = ChannelRDs.AsEnumerable().Max(r => r.Field<int>("MaxRD"));

            if (ChannelRD >= MinRDs && ChannelRD <= MaxRDs)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        public static DataTable GetDataTable(long WaterTheftBreachID, long UserID, long DesignationID, List<Tuple<string, string, string>> lstNameofFiles, string Source, Double GIS_X, Double GIS_Y)
        {
            int Size = lstNameofFiles.Count;
            DataSet DataSet = new DataSet();
            DataTable DataTable = DataSet.Tables.Add("SampleData");
            DataTable.Columns.Add("WaterTheftID", typeof(long));
            DataTable.Columns.Add("FileName", typeof(string));
            DataTable.Columns.Add("FileType", typeof(string));
            DataTable.Columns.Add("AttachmentPath", typeof(string));
            DataTable.Columns.Add("AttachmentByUserID", typeof(long));
            DataTable.Columns.Add("IsActive", typeof(bool));
            DataTable.Columns.Add("CreatedBy", typeof(long));
            DataTable.Columns.Add("AttachmentByDesignationID", typeof(long));
            DataTable.Columns.Add("Source", typeof(string));
            DataTable.Columns.Add("GIS_X", typeof(Double));
            DataTable.Columns.Add("GIS_Y", typeof(Double));

            for (int i = 0; i < Size; ++i)
            {
                DataRow DataRow;
                DataRow = DataTable.NewRow();
                DataRow["WaterTheftID"] = WaterTheftBreachID;
                DataRow["FileName"] = lstNameofFiles[i].Item1;
                DataRow["FileType"] = lstNameofFiles[i].Item2;
                DataRow["AttachmentPath"] = lstNameofFiles[i].Item3;
                DataRow["AttachmentByUserID"] = UserID;
                DataRow["IsActive"] = true;
                DataRow["CreatedBy"] = UserID;
                DataRow["AttachmentByDesignationID"] = DesignationID;
                DataRow["Source"] = Source;
                DataRow["GIS_X"] = GIS_X;
                DataRow["GIS_Y"] = GIS_Y;
                DataTable.Rows.Add(DataRow);
            }
            return DataTable;
        }
        public long GetWTCaseStatusIDByName(string _Status)
        {
            return dalWaterTheft.GetWTCaseStatusIDByName(_Status);
        }

        public string GenerateWaterTheftCaseNo()
        {
            return dalWaterTheft.GenerateWaterTheftCaseNo();
        }
        #endregion

        public UA_RoleRights GetRoleRights(long _UserID, long _PageID)
        {
            return dalWaterTheft.GetRoleRights(_UserID, _PageID);
        }


    }
}
