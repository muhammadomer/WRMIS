using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.Tenders;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PMIU.WRMIS.BLL.Tenders
{
    public class TenderManagementBLL : BaseBLL
    {
        TenderManagementDAL dalTenderManagement = new TenderManagementDAL();
        #region "Reference Data"
        public bool IsFundingSourceFixed(long _FundingSrcID)
        {
            return dalTenderManagement.IsFundingSourceFixed(_FundingSrcID);
        }
        public List<object> GetFundingSourceList()
        {
            return dalTenderManagement.GetFundingSourceList();
        }
        public List<TM_FundingSource> GetAllFundingSources()
        {
            return dalTenderManagement.GetAllFundingSources();
        }

        public TM_FundingSource GetFundingSourceByName(string _FundingSource)
        {
            return dalTenderManagement.GetFundingSourceByName(_FundingSource);
        }

        public bool AddFundingSource(TM_FundingSource _FundingSource)
        {
            return dalTenderManagement.AddFundingSource(_FundingSource);
        }

        public bool UpdateFundingSource(TM_FundingSource _FundingSource)
        {
            return dalTenderManagement.UpdateFundingSource(_FundingSource);
        }

        public bool DeleteFundingSource(long _FundingSourceID)
        {
            return dalTenderManagement.DeleteFundingSource(_FundingSourceID);
        }

        public bool IsFundingSourceIDExists(long _FundingSourceID)
        {
            return dalTenderManagement.IsFundingSourceIDExists(_FundingSourceID);
        }

        public List<TM_TenderOpeningOffice> GetAllTenderOpeningOffices()
        {
            return dalTenderManagement.GetAllTenderOpeningOffices();
        }

        public TM_TenderOpeningOffice GetTenderOpeningOfficeByName(string _OfficeName)
        {
            return dalTenderManagement.GetTenderOpeningOfficeByName(_OfficeName);
        }

        public bool AddTenderOpeningOffice(TM_TenderOpeningOffice _TenderOpeningOffice)
        {
            return dalTenderManagement.AddTenderOpeningOffice(_TenderOpeningOffice);
        }

        public bool UpdateTenderOpeningOffice(TM_TenderOpeningOffice _TenderOpeningOffice)
        {
            return dalTenderManagement.UpdateTenderOpeningOffice(_TenderOpeningOffice);
        }

        public bool DeleteTenderOpeningOffice(long _TenderOpeningOffice)
        {
            return dalTenderManagement.DeleteTenderOpeningOffice(_TenderOpeningOffice);
        }

        public bool IsTenderOpeningOfficeIDExists(long _TenderOpeningOfficeID)
        {
            return dalTenderManagement.IsTenderOpeningOfficeIDExists(_TenderOpeningOfficeID);
        }

        public List<TM_Contractors> GetAllContractors()
        {
            return dalTenderManagement.GetAllContractors();
        }

        public TM_Contractors GetContractorByName(string _Contractor)
        {
            return dalTenderManagement.GetContractorByName(_Contractor);
        }

        public bool AddContractor(TM_Contractors _Contractor)
        {
            return dalTenderManagement.AddContractor(_Contractor);
        }

        public bool UpdateContractor(TM_Contractors _Contractor)
        {
            return dalTenderManagement.UpdateContractor(_Contractor);
        }

        public bool DeleteContractor(long _ContractorID)
        {
            return dalTenderManagement.DeleteContractor(_ContractorID);
        }

        public List<TM_CommitteeMembers> GetAllCommitteeMembers()
        {
            return dalTenderManagement.GetAllCommitteeMembers();
        }

        public TM_CommitteeMembers GetCommitteeMembersByName(string _Contractor)
        {
            return dalTenderManagement.GetCommitteeMembersByName(_Contractor);
        }
        public bool IsCommetteeMembersExist(TM_CommitteeMembers ObjCommitteeMembers)
        {
            return dalTenderManagement.IsCommetteeMembersExist(ObjCommitteeMembers);
        }

        public bool AddCommitteeMembers(TM_CommitteeMembers _CommitteeMember)
        {
            return dalTenderManagement.AddCommitteeMembers(_CommitteeMember);
        }

        public bool UpdateCommitteeMembers(TM_CommitteeMembers _CommitteeMember)
        {
            return dalTenderManagement.UpdateCommitteeMembers(_CommitteeMember);
        }

        public bool DeleteCommitteeMembers(long _CommitteeMemberID)
        {

            return dalTenderManagement.DeleteCommitteeMembers(_CommitteeMemberID);
        }
        #endregion

        #region "Tender Notice"

        public List<dynamic> GetDomains()
        {
            try
            {
                return dalTenderManagement.GetDomains();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetDivisionByDomainID(long _DomainID)
        {
            try
            {
                return dalTenderManagement.GetDivisionByDomainID(_DomainID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public long SaveTenderNotice(TM_TenderNotice _TenderNoticeModel)
        {
            try
            {
                return dalTenderManagement.SaveTenderNotice(_TenderNoticeModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SavePublishingSource(TM_TenderPublishedIn _TenderPublishingSource)
        {
            try
            {
                return dalTenderManagement.SavePublishingSource(_TenderPublishingSource);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TM_TenderNotice> GetAllTenderNotice(string _TenderNoticeName, long _DomainID, long _DivisionID, DateTime? _SubmissionDateFrom, DateTime? _SubmissionDateTo, DateTime? _OpeningDateFrom, DateTime? _OpeningDateTo, long _Status)
        {
            return dalTenderManagement.GetAllTenderNotice(_TenderNoticeName, _DomainID, _DivisionID, _SubmissionDateFrom, _SubmissionDateTo, _OpeningDateFrom, _OpeningDateTo, _Status);
        }

        public TM_TenderNotice GetTenderNoticeByID(long _TenderNotice)
        {
            try
            {
                return dalTenderManagement.GetTenderNoticeByID(_TenderNotice);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<dynamic> GetPublishingSourceByTenderNoticeID(long _TenderNotice)
        {
            try
            {
                return dalTenderManagement.GetPublishingSourceByTenderNoticeID(_TenderNotice);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeletePublishingSourceForUpdation(long _TenderNoticeID)
        {
            try
            {
                return dalTenderManagement.DeletePublishingSourceForUpdation(_TenderNoticeID);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public long SaveCommitteeMember(TM_CommitteeMembers _mdlCommitteeMembers)
        {
            try
            {
                return dalTenderManagement.SaveCommitteeMember(_mdlCommitteeMembers);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool SaveTenderCommitteeMember(TM_TenderCommitteeMembers _mdlTenderCommitteeMembers)
        {
            try
            {
                return dalTenderManagement.SaveTenderCommitteeMember(_mdlTenderCommitteeMembers);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region "Works"

        public dynamic GetTenderDataByID(long _TenderNoticeID)
        {
            try
            {
                return dalTenderManagement.GetTenderDataByID(_TenderNoticeID);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetWorksDataByTenderNoticeID(long _TenderNoticeID)
        {
            try
            {
                return dalTenderManagement.GetWorksDataByTenderNoticeID(_TenderNoticeID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetClosureWorks(long _DivisionID, string _Year, long _WorkType, long _TenderNoticeID, string _AssestorClosure)
        {
            try
            {
                return dalTenderManagement.GetClosureWorks(_DivisionID, _Year, _WorkType, _TenderNoticeID, _AssestorClosure);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool SaveClosureWorkByTenderNoticeID(long _TenderNoticeID, long _CLosureWorkID, string _WorkSource, int _WorkStatusID, int _CreatedBy)
        {
            try
            {
                TM_TenderWorks WorkData = new TM_TenderWorks();
                WorkData.TenderNoticeID = _TenderNoticeID;
                WorkData.WorkSourceID = _CLosureWorkID;
                WorkData.WorkSource = _WorkSource;
                WorkData.WorkStatusID = _WorkStatusID;
                WorkData.CreatedDate = DateTime.Now;
                WorkData.CreatedBy = _CreatedBy;

                return dalTenderManagement.SaveClosureWorkByTenderNoticeID(WorkData);


            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteWorkByTenderWorkID(long _TenderWorkID)
        {
            try
            {
                return dalTenderManagement.DeleteWorkByTenderWorkID(_TenderWorkID);


            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetClosureWorkTypes()
        {
            try
            {
                return dalTenderManagement.GetClosureWorkTypes();


            }
            catch (Exception)
            {

                throw;
            }

        }


        public List<dynamic> GetYearByDivisionID(long _DivisionID)
        {
            try
            {
                return dalTenderManagement.GetYearByDivisionID(_DivisionID);


            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetClosureWorkItemsByWorkID(long _WorkSourceID, string _WorkSource)
        {
            try
            {
                return dalTenderManagement.GetClosureWorkItemsByWorkID(_WorkSourceID, _WorkSource);


            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetClosureWorkDataByID(long _WorkSourceID, long _TenderWorkID)
        {
            try
            {
                return dalTenderManagement.GetClosureWorkDataByID(_WorkSourceID, _TenderWorkID);


            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<object> GetTenderWorksListByDivisionID(long? _DivisionID, long? _TenderWorkID)
        {
            return dalTenderManagement.GetTenderWorksListByDivisionID(_DivisionID, _TenderWorkID);
        }
        public long GetTenderNoticeIDByTenderWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.GetTenderNoticeIDByTenderWorkID(_TenderWorkID);
        }

        public DataTable GetTenderWorkDataByWorkID(long _ClosureWorkID)
        {
            return dalTenderManagement.GetTenderWorkDataByWorkID(_ClosureWorkID);
        }
        public dynamic GetWorktypeIDandSourceByWorkID(long _WorkSourceID)
        {
            return dalTenderManagement.GetWorktypeIDandSourceByWorkID(_WorkSourceID);
        }

        public dynamic GetElecMechInfoByWorkID(long _ClosureWorkID)
        {
            return dalTenderManagement.GetElecMechInfoByWorkID(_ClosureWorkID);
        }

        public dynamic GetDesiltingByWorkID(long _ClosureWorkID)
        {
            return dalTenderManagement.GetDesiltingByWorkID(_ClosureWorkID);
        }

        public dynamic GetBuildingWorksInfoByWorkID(long _ClosureWorkID)
        {
            return dalTenderManagement.GetBuildingWorksInfoByWorkID(_ClosureWorkID);
        }

        public dynamic GetOilGrePaintingInfoByWorkID(long _ClosureWorkID)
        {
            return dalTenderManagement.GetOilGrePaintingInfoByWorkID(_ClosureWorkID);
        }

        public dynamic GetOutletRepairingInfoByWorkID(long _ClosureWorkID)
        {
            return dalTenderManagement.GetOutletRepairingInfoByWorkID(_ClosureWorkID);
        }

        public dynamic GetChannelStructWorkInfoByWorkID(long _ClosureWorkID)
        {
            return dalTenderManagement.GetChannelStructWorkInfoByWorkID(_ClosureWorkID);
        }

        public dynamic GetOtherWorkInfoByWorkID(long _ClosureWorkID)
        {
            return dalTenderManagement.GetOtherWorkInfoByWorkID(_ClosureWorkID);
        }

        public List<TM_TenderOpeningOffice> GetAllOtherTenderOffices()
        {
            return dalTenderManagement.GetAllOtherTenderOffices();
        }

        public bool UpdateOpeningOfficeByWorkSourceID(long _WorkSourceID, string _Office, long _OfficeId)
        {
            return dalTenderManagement.UpdateOpeningOfficeByWorkSourceID(_WorkSourceID, _Office, _OfficeId);
        }
        public bool UpdateOpeningOfficeByTenderID(long _ID, string _Office, long _OfficeId)
        {
            return dalTenderManagement.UpdateOpeningOfficeByTenderID(_ID, _Office, _OfficeId);
        }
        public bool CheckOpeningOfficeByWorkSourceID(long _ID)
        {
            return dalTenderManagement.CheckOpeningOfficeByWorkSourceID(_ID);
        }

        public dynamic GetOfficeLocationByZoneID(long _OfficeLocationID, long _WorkSourceID)
        {
            return dalTenderManagement.GetOfficeLocationByZoneID(_OfficeLocationID, _WorkSourceID);
        }
        public dynamic GetOfficeLocationByCircleID(long _OfficeLocationID, long _WorkSourceID)
        {
            return dalTenderManagement.GetOfficeLocationByCircleID(_OfficeLocationID, _WorkSourceID);
        }

        public dynamic GetOfficeLocationByDivisionID(long _OfficeLocationID, long _WorkSourceID)
        {
            return dalTenderManagement.GetOfficeLocationByDivisionID(_OfficeLocationID, _WorkSourceID);
        }

        public dynamic GetOfficeLocationByOtherID(long _OfficeLocationID, long _WorkSourceID)
        {
            return dalTenderManagement.GetOfficeLocationByOtherID(_OfficeLocationID, _WorkSourceID);
        }


        public List<object> GetEvalCommitteeMember(long _WorkID)
        {
            return dalTenderManagement.GetEvalCommitteeMember(_WorkID);
        }

        public List<dynamic> GetCommitteeMembersName(string _Name)
        {
            try
            {
                return dalTenderManagement.GetCommitteeMembersName(_Name);


            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetCommitteeMembersListByWorkID(long _WorkID)
        {

            return dalTenderManagement.GetCommitteeMembersListByWorkID(_WorkID);

        }
        public List<dynamic> GetSoldTenderListByWorkID(long _WorkID)
        {
            try
            {
                return dalTenderManagement.GetSoldTenderListByWorkID(_WorkID);


            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetContractorsList(string _Name)
        {
            try
            {
                return dalTenderManagement.GetContractorsList(_Name);


            }
            catch (Exception)
            {

                throw;
            }

        }

        //public List<dynamic> GetContractorsList()
        //{
        //    try
        //    {
        //        return dalTenderManagement.GetContractorsList();


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        public long AddContractorFromSoldTenderList(TM_Contractors _Contractor)
        {
            return dalTenderManagement.AddContractorFromSoldTenderList(_Contractor);
        }


        #endregion

        #region "Tender Opening Process"
        public List<object> GetTenderCommitteeMembers(long _TenderWorkID)
        {
            return dalTenderManagement.GetTenderCommitteeMembers(_TenderWorkID);
        }

        public List<object> GetTenderContractors(long _TenderWorkID)
        {
            return dalTenderManagement.GetTenderContractors(_TenderWorkID);
        }
        public bool SaveTenderWorkContractor(TM_TenderWorksContractors _mdlTenderWorkContractor)
        {
            return dalTenderManagement.SaveTenderWorkContractors(_mdlTenderWorkContractor);
        }
        public void UpdateTenderWorkStatusByTenderWorkID(long _TenderWorkID, int _WorkStatusID)
        {
            dalTenderManagement.UpdateTenderWorkStatusByTenderWorkID(_TenderWorkID, _WorkStatusID);
        }
        public List<dynamic> GetUsersByDivisionID(long _DivisionID, long _DesignationID)
        {
            return dalTenderManagement.GetUsersByDivisionID(_DivisionID, _DesignationID);
        }
        public object GetTenderCommitteeMemberByID(long _ID)
        {
            return dalTenderManagement.GetTenderCommitteeMemberByID(_ID);
        }
        public bool SaveEvaluationCommitteeAttendance(List<TM_TenderCommitteeMembers> lstAttendance, List<string> lstNameofFiles, long _TenderWorkID, string _MonitoredBy, string _MonitoredByName, string _OpenedBy, string ECA_Attachment, long _UserID)
        {
            return dalTenderManagement.SaveEvaluationCommitteeAttendance(lstAttendance, lstNameofFiles, _TenderWorkID, _MonitoredBy, _MonitoredByName, _OpenedBy, ECA_Attachment, _UserID);
        }

        public dynamic GetTenderWorksContractor(long _TenderWorksContractorID)
        {
            return dalTenderManagement.GetTenderWorkContractor(_TenderWorksContractorID);
        }

        public int UpdateTenderCommitteeMembers(string _MembersXml)
        {
            return dalTenderManagement.UpdateTenderCommitteeMembers(_MembersXml);
        }
        public object GetTenderContractorByID(long _ID)
        {
            return dalTenderManagement.GetTenderContractorByID(_ID);
        }
        public bool SaveContractorAttendance(List<TM_TenderWorksContractors> lstAttendance, List<string> lstNameofFiles, long _TenderWorkID, string _MonitoredBy, string _MonitoredByName, string ECA_Attachment, long _UserID)
        {
            return dalTenderManagement.SaveContractorAttendance(lstAttendance, lstNameofFiles, _TenderWorkID, _MonitoredBy, _MonitoredByName, ECA_Attachment, _UserID);
        }
        public bool UpdateTenderEvaluationCommittee(TM_TenderCommitteeMembers _TenderCommittee)
        {
            return dalTenderManagement.UpdateTenderEvaluationCommittee(_TenderCommittee);
        }

        public bool SaveTenderWorkAttachments(TM_TenderWorkAttachment _TenderWorkAttachment)
        {
            return dalTenderManagement.SaveTenderWorkAttachments(_TenderWorkAttachment);
        }
        public bool UpdateTenderContractors(TM_TenderWorksContractors _TenderContractors)
        {
            return dalTenderManagement.UpdateTenderContractors(_TenderContractors);
        }
        public bool UpdateTenderContractorsForADMReport(TM_TenderWorksContractors _TenderContractors)
        {
            return dalTenderManagement.UpdateTenderContractorsForADMReport(_TenderContractors);
        }
        public bool SaveADMAttachemnt(TM_ADMAttachment _mdlADMAttachemnt)
        {
            return dalTenderManagement.SaveADMAttachemnt(_mdlADMAttachemnt);
        }
        public bool SaveTenderWorkStatusInfromation(TM_TenderWorks _mdlTenderWork)
        {
            return dalTenderManagement.SaveTenderWorkStatsuInformation(_mdlTenderWork);
        }

        public bool UpdateTenderWorksByWorkID(TM_TenderWorks _TenderWorks)
        {
            return dalTenderManagement.UpdateTenderWorksByWorkID(_TenderWorks);
        }

        public bool UpdateTenderWorksForCntractorsByWorkID(TM_TenderWorks _TenderWorks)
        {
            return dalTenderManagement.UpdateTenderWorksforContractorsByWorkID(_TenderWorks);
        }
        public bool UpdateTenderWorksForADMReport(TM_TenderWorks _TenderWorks)
        {
            return dalTenderManagement.UpdateTenderWorksforADMReport(_TenderWorks);
        }
        public dynamic GetECMDataByWorkID(long _WorkID)
        {
            return dalTenderManagement.GetECMDataByWorkID(_WorkID);
        }

        public dynamic GetCMDataByTenderWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.GetCMDataByTenderWorkID(_TenderWorkID);
        }
        public object GetTenderWorkItems(long _ID)
        {
            return dalTenderManagement.GetTenderContractorByID(_ID);
        }
        public List<object> GetWorkItemsDetailsByWorKID(long _TenderWorkID, long _CompanyID, long _TenderNoticeID)
        {
            return dalTenderManagement.GetWorkItemsDetailsByWorKID(_TenderWorkID, _CompanyID, _TenderNoticeID);
        }
        public List<object> GetTenderAttendanceAttachment(long _TenderWorkID, string source)
        {
            return dalTenderManagement.GetTenderAttendanceAttachment(_TenderWorkID, source);
        }

        public List<object> GetCompanyNameByWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.GetCompanyNameByWorkID(_TenderWorkID);
        }

        public List<dynamic> GetContractorsByTenderWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.GetContractorsListBytenderWorkID(_TenderWorkID);
        }
        public List<dynamic> GetADMReportDataByTenderWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.GetADMReportDataByTenderWorkID(_TenderWorkID);
        }
        public dynamic GetTenderInformationForADMReport(long _TenderWorkID)
        {
            return dalTenderManagement.GetTenderInformationForADMReport(_TenderWorkID);
        }
        public List<object> GetContractorCallDeposit(long _TWContractorID)
        {
            return dalTenderManagement.GetContractorCallDeposit(_TWContractorID);
        }
        public List<object> GetCallDepositDataByWorKID(long _TenderWorkID, long _CompanyID)
        {
            return dalTenderManagement.GetCallDepositDataByWorKID(_TenderWorkID, _CompanyID);
        }

        public long UpdateTenderWorkContractorDataByID(TM_TenderWorksContractors _TenderWorksContractor)
        {
            return dalTenderManagement.UpdateTenderWorkContractorDataByID(_TenderWorksContractor);
        }

        public bool AddTenderPriceValue(TM_TenderPrice _TenderPrice)
        {
            return dalTenderManagement.AddTenderPriceValue(_TenderPrice);
        }
        public bool AddorUpdateTenderPriceCDR(TM_TenderPriceCDR _TenderPriceCDR)
        {
            return dalTenderManagement.AddorUpdateTenderPriceCDR(_TenderPriceCDR);
        }
        public bool AddUpdateTenderPriceCDR(TM_TenderPriceCDR _TenderPriceCDR)
        {
            return dalTenderManagement.AddUpdateTenderPriceCDR(_TenderPriceCDR);
        }
        public bool UpdateTenderPriceCDR(TM_TenderPriceCDR _TenderPriceCDR)
        {
            return dalTenderManagement.UpdateTenderPriceCDR(_TenderPriceCDR);
        }
        public List<object> GetWorkItemsforViewByWorKID(long _TenderWorkID)
        {
            return dalTenderManagement.GetWorkItemsforViewByWorKID(_TenderWorkID);
        }

        public List<object> GetCallDepositDataforViewByWorKID(long _TenderWorkID)
        {
            return dalTenderManagement.GetCallDepositDataforViewByWorKID(_TenderWorkID);
        }
        public List<TM_ADMAttachment> getAllADMAttachemnts(long _tenderWorkID)
        {
            return dalTenderManagement.GetAllADMAttachemnts(_tenderWorkID);
        }
        public List<TM_TenderWorkAttachment> GetAllTenderWorkAttachments(long _tenderWorkID, string _Source)
        {
            return dalTenderManagement.GetAllTenderWorkAttachments(_tenderWorkID, _Source);
        }
        public TM_TenderWorks getADMReportData(long _tenderWorkID)
        {
            return dalTenderManagement.GetADMReportData(_tenderWorkID);
        }
        public object GetTenderPriceByWorkItemID(long _WorkItemId, long _TWContractorID)
        {
            return dalTenderManagement.GetTenderPriceByWorkItemID(_WorkItemId, _TWContractorID);
        }
        public bool IsTenderPriceAdded(long _WorkItemId, long _TWContractorID)
        {
            return dalTenderManagement.IsTenderPriceAdded(_WorkItemId, _TWContractorID);
        }
        public long SaveTenderPrice(List<TM_TenderPrice> _lstTenderPrice, TM_TenderPriceCDR _TenderPriceCDR, TM_TenderWorksContractors _tenderContractor, long _UserID)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                bool isAdded = false;
                if (_lstTenderPrice.Count > 0)
                {
                    for (int i = 0; i < _lstTenderPrice.Count; i++)
                    {
                        isAdded = dalTenderManagement.AddTenderPriceValue(_lstTenderPrice.ElementAt(i));
                    }

                    if (isAdded)
                    {
                        if (_TenderPriceCDR != null)
                        {
                            dalTenderManagement.AddorUpdateTenderPriceCDR(_TenderPriceCDR);
                        }
                        dalTenderManagement.UpdateTenderWorkContractorDataByID(_tenderContractor);
                    }


                }


                transaction.Complete();
            }
            if (_TenderPriceCDR != null)
            {

                TM_TenderWorks _tenderWork = dalTenderManagement.GetWorkByTenderWorkID(Convert.ToInt64(_tenderContractor.TenderWorksID));
                if (_tenderWork != null)
                {
                    PMIU.WRMIS.BLL.Notifications.NotifyEvent _event = new PMIU.WRMIS.BLL.Notifications.NotifyEvent();
                    _event.Parameters.Add("TenderNoticeID", Convert.ToInt64(_tenderWork.TenderNoticeID));
                    _event.Parameters.Add("TenderWorkID", Convert.ToInt64(_tenderWork.ID));
                    _event.AddNotifyEvent((long)NotificationEventConstants.TenderMgmt.AddComparativeStatement, _UserID);
                }

                return _TenderPriceCDR.ID;
            }
            else
            {
                return 0;
            }

        }
        public object GetTenderPriceCDRByID(long _ID)
        {
            return dalTenderManagement.GetTenderPriceCDRByID(_ID);
        }
        public long? GetContractorIDFromTWContractorID(long _TWContractorID)
        {
            return dalTenderManagement.GetContractorIDFromTWContractorID(_TWContractorID);
        }

        public DataTable GetComparativeStatementDataByWorkandNoticeID(long _WorkID)
        {
            return dalTenderManagement.GetComparativeStatementDataByWorkandNoticeID(_WorkID);
        }
        public bool IsContractorAlreadyAdded(long _ContractorID, long _TenderWorkID)
        {
            return dalTenderManagement.IsContractorAlreadyAdded(_ContractorID, _TenderWorkID);
        }
        public bool IsMemberAlreadyAdded(long _MemberID, long _TenderWorkID)
        {
            return dalTenderManagement.IsMemberAlreadyAdded(_MemberID, _TenderWorkID);
        }
        public bool SaveADMReportData(List<TM_TenderWorksContractors> lstRejectedContractors, TM_TenderWorks mdlTenderWorks, List<string> lstofAttachment, long _UserID)
        {
            using (TransactionScope transaction = new TransactionScope())
            {

                if (lstofAttachment.Count > 0)
                {
                    //for Android Delete png's only before Adding
                    if (DeleteADMAttachmentByTenderWorkID(mdlTenderWorks.ID))
                    {
                        for (int i = 0; i < lstofAttachment.Count; i++)
                        {
                            SaveADMAttachments(mdlTenderWorks.ID, lstofAttachment[i].ToString(), _UserID);
                        }
                    }

                }
                if (lstRejectedContractors.Count > 0)
                {
                    for (int i = 0; i < lstRejectedContractors.Count; i++)
                    {
                        UpdateTenderContractorsForADMReport(lstRejectedContractors.ElementAt(i));
                    }
                }
                UpdateTenderWorksForADMReport(mdlTenderWorks);
                transaction.Complete();
            }
            return true;
        }
        public bool SaveADMAttachments(long _TenderWorkID, string AttachmentName, long _UserID)
        {
            return dalTenderManagement.SaveADMAttachments(_TenderWorkID, AttachmentName, _UserID);
        }
        public bool DeleteADMAttachmentByTenderWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.DeleteADMAttachmentByTenderWorkID(_TenderWorkID);
        }

        public List<object> GetContractorandAmountByWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.GetContractorandAmountByWorkID(_TenderWorkID);
        }

        public long UpdateTenderWorkContractorAwardFieldByID(TM_TenderWorksContractors _TenderWorksContractor)
        {
            return dalTenderManagement.UpdateTenderWorkContractorAwardFieldByID(_TenderWorksContractor);
        }

        public object GetWorkItemRateBySourceID(long _WorkSourceID)
        {
            return dalTenderManagement.GetWorkItemRateBySourceID(_WorkSourceID);
        }
        public List<dynamic> GetMonitoringUsersByDivisionID(long _DivisionID)
        {
            return dalTenderManagement.GetMonitoringUsersByDivisionID(_DivisionID);
        }

        public long UpdateTenderWorkStatusIDByWorkID(TM_TenderWorks _TenderWorks)
        {
            return dalTenderManagement.UpdateTenderWorkStatusIDByWorkID(_TenderWorks);
        }

        public bool IsMemberExistInTendrCommittee(long _CommitteeMemberID)
        {
            return dalTenderManagement.IsMemberExistInTendrCommittee(_CommitteeMemberID);
        }
        public bool IsContractorExist(long _ContractorID)
        {
            return dalTenderManagement.IsContractorExist(_ContractorID);
        }
        public bool IsPhoneOrEmailExistInTendrCommittee(string _MobilePhone, string _Email)
        {
            return dalTenderManagement.IsPhoneOrEmailExistInTendrCommittee(_MobilePhone, _Email);
        }
        public bool IsPhoneExistInContractor(string _MobilePhone)
        {
            return dalTenderManagement.IsPhoneExistInContractor(_MobilePhone);
        }

        /// <summary>
        /// This function searches Contractors based on Contact Number.
        /// Created On 18-08-2017
        /// </summary>
        /// <param name="_ContactNumber"></param>
        /// <returns>TM_Contractors</returns>
        public TM_Contractors GetContractorByContactNumber(string _ContactNumber)
        {
            return dalTenderManagement.GetContractorByContactNumber(_ContactNumber);
        }

        public bool? GetAwardedTenderByWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.GetAwardedTenderByWorkID(_TenderWorkID);
        }
        #endregion

        public object GetTenderWorkByID(long? _TenderWorkID)
        {
            return dalTenderManagement.GetTenderWorkByID(_TenderWorkID);
        }

        public List<TM_TenderPriceCDR> DeleteCallDepositByTenderWorkContractorID(long _TendWContractorID)
        {
            try
            {
                return dalTenderManagement.DeleteCallDepositByTenderWorkContractorID(_TendWContractorID);


            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool IsADMPreparedReport(long _TendWorkID)
        {
            try
            {
                return dalTenderManagement.IsADMPreparedReport(_TendWorkID);


            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Asset

        public List<dynamic> GetAssetTypes()
        {
            try
            {
                return dalTenderManagement.GetAssetTypes();


            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetAssetYearByDivisionID(long _DivisionID)
        {
            try
            {
                return dalTenderManagement.GetAssetYearByDivisionID(_DivisionID);


            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetAssetWorkInfoByWorkID(long _WorkSourceID, long _TenderNoticeID)
        {
            return dalTenderManagement.GetAssetWorkInfoByWorkID(_WorkSourceID, _TenderNoticeID);
        }

        public bool UpdateAssetWorkStatus(long _WorkSourceID)
        {
            return dalTenderManagement.UpdateAssetWorkStatus(_WorkSourceID);
        }

        public TM_TenderWorks GetTenderWorkEntity(long _TenderWorkID)
        {
            return dalTenderManagement.GetTenderWorkEntity(_TenderWorkID);
        }
        public string GetStructureTypeNameByID(long _StructureTypeID)
        {
            return dalTenderManagement.GetStructureTypeNameByID(_StructureTypeID);
        }
        #endregion

        #region Send Notification One Day Before

        public void GetTenderNoticeID()
        {
            List<long> lstTenderIDs;
            long DivisionID;
            try
            {
                lstTenderIDs = dalTenderManagement.GetTenderNoticeID();
                foreach (var item in lstTenderIDs)
                {
                    long value = item;
                    string Division = dalTenderManagement.GetDivisionByTenderID(value);
                    DivisionID = Convert.ToInt64(Division);
                    GetWorksByTenderID(value, DivisionID);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetWorksByTenderID(long _TenderNoticeID, long _DivisionID)
        {
            List<dynamic> lstOfWorks;
            try
            {
                lstOfWorks = dalTenderManagement.GetWorksByTenderID(_TenderNoticeID);
                foreach (var item in lstOfWorks)
                {
                    string Source = item.GetType().GetProperty("WorkSource").GetValue(item).ToString();
                    string SourceID = item.GetType().GetProperty("ID").GetValue(item).ToString();
                    SentAssetNotification(_TenderNoticeID, Convert.ToInt64(SourceID));
                }


            }
            catch (Exception)
            {

                throw;
            }
            return lstOfWorks;
        }


        //public void GetClouserWorkInformation(long _ClouserWorkID)
        //{
        //    List<dynamic> lst  =  dalTenderManagement.GetClouserWorksByTenderID(_ClouserWorkID); 
        //}
        //public void GetAssetWorkInformation (long _AssetWorkID, long _DivisionID)
        //{
        //    List<dynamic> lst = dalTenderManagement.GetAssetWorksByTenderID(_AssetWorkID);
        //    foreach (var item in lst)
        //    {
        //        string OpeningDate = item.GetType().GetProperty("OpeningDate").GetValue(item).ToString();
        //        string WorkName = item.GetType().GetProperty("WorkName").GetValue(item).ToString();
        //        string OpeningOffice = item.GetType().GetProperty("OpeningOffice").GetValue(item).ToString();

        //        //SentAssetNotification(OpeningDate, WorkName, OpeningOffice, _DivisionID);

        //    }
        //}

        public string GetWorkSourceByWorkID(long _TenderWorkID)
        {
            return dalTenderManagement.GetWorkSourceByWorkID(_TenderWorkID);
        }

        public dynamic GetAssetTenderInformationForADMReport(long _TenderWorkID)
        {
            return dalTenderManagement.GetAssetTenderInformationForADMReport(_TenderWorkID);
        }

        public bool SentAssetNotification(long TenderNoticeID, long TenderWorkID)
        {
            PMIU.WRMIS.BLL.Notifications.NotifyEvent _event = new PMIU.WRMIS.BLL.Notifications.NotifyEvent();
            _event.Parameters.Add("TenderNoticeID", TenderNoticeID);
            _event.Parameters.Add("TenderWorkID", TenderWorkID);
            _event.AddNotifyEvent((long)NotificationEventConstants.TenderMgmt.TenderWorkOpening, 0);

            return true;
        }
        #endregion


    }
}
