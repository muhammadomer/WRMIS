using PMIU.WRMIS.DAL.Repositories.Tenders;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.Tenders
{
    public class TenderManagementDAL
    {
        ContextDB db = new ContextDB();

        #region "Reference Data"
        public bool IsFundingSourceFixed(long _FundingSrcID)
        {
            TM_FundingSource mdl = db.Repository<TM_FundingSource>().FindById(_FundingSrcID);
            if (mdl != null)
                if (mdl.FundingType.ToUpper().Equals("FIXED"))
                    return true;
            return false;
        }

        public List<object> GetFundingSourceList()
        {
            return db.Repository<TM_FundingSource>().Query().Get()
                .Where(x => x.IsActive == true).OrderBy(n => n.FundingSource)
                .Select(x => new { ID = x.ID, Name = x.FundingSource })
                .ToList<object>();
        }
        public List<TM_FundingSource> GetAllFundingSources()
        {
            List<TM_FundingSource> lstFundingSource = db.Repository<TM_FundingSource>().GetAll().OrderBy(n => n.FundingSource).ToList<TM_FundingSource>();
            return lstFundingSource;
        }

        public TM_FundingSource GetFundingSourceByName(string _FundingSource)
        {
            TM_FundingSource mdlFundingSource = db.Repository<TM_FundingSource>().GetAll().Where(z => z.FundingSource.Trim().ToLower() == _FundingSource.Trim().ToLower()).FirstOrDefault();
            return mdlFundingSource;
        }

        public bool AddFundingSource(TM_FundingSource _FundingSource)
        {
            db.Repository<TM_FundingSource>().Insert(_FundingSource);
            db.Save();
            return true;
        }

        public bool UpdateFundingSource(TM_FundingSource _FundingSource)
        {
            TM_FundingSource mdlFundingSource = db.Repository<TM_FundingSource>().FindById(_FundingSource.ID);
            mdlFundingSource.FundingSource = _FundingSource.FundingSource;
            mdlFundingSource.FundingType = _FundingSource.FundingType;
            mdlFundingSource.Description = _FundingSource.Description;
            mdlFundingSource.IsActive = _FundingSource.IsActive;
            mdlFundingSource.ModifiedBy = _FundingSource.ModifiedBy;
            mdlFundingSource.ModifiedDate = _FundingSource.ModifiedDate;
            mdlFundingSource.IsActive = _FundingSource.IsActive;
            db.Repository<TM_FundingSource>().Update(mdlFundingSource);
            db.Save();
            return true;
        }

        public bool DeleteFundingSource(long _FundingSourceID)
        {
            db.Repository<TM_FundingSource>().Delete(_FundingSourceID);
            db.Save();
            return true;
        }

        public bool IsFundingSourceIDExists(long _FundingSourceID)
        {
            bool qIsExists = db.Repository<CW_ClosureWork>().GetAll().Any(s => s.FundingSourceID == _FundingSourceID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<AM_AssetWork>().GetAll().Any(s => s.FundingSourceID == _FundingSourceID);
            }
            return qIsExists;
        }

        public List<TM_TenderOpeningOffice> GetAllTenderOpeningOffices()
        {
            List<TM_TenderOpeningOffice> lstTenderOpeningOffice = db.Repository<TM_TenderOpeningOffice>().GetAll().OrderBy(n => n.Name).ToList<TM_TenderOpeningOffice>();
            return lstTenderOpeningOffice;
        }

        public TM_TenderOpeningOffice GetTenderOpeningOfficeByName(string _OfficeName)
        {
            TM_TenderOpeningOffice mdlTenderOpeningOffice = db.Repository<TM_TenderOpeningOffice>().GetAll().Where(z => z.Name.Trim().ToLower() == _OfficeName.Trim().ToLower()).FirstOrDefault();
            return mdlTenderOpeningOffice;
        }

        public bool AddTenderOpeningOffice(TM_TenderOpeningOffice _TenderOpeningOffice)
        {
            db.Repository<TM_TenderOpeningOffice>().Insert(_TenderOpeningOffice);
            db.Save();
            return true;
        }

        public bool UpdateTenderOpeningOffice(TM_TenderOpeningOffice _TenderOpeningOffice)
        {
            TM_TenderOpeningOffice mdlTenderOpeningOffice = db.Repository<TM_TenderOpeningOffice>().FindById(_TenderOpeningOffice.ID);
            mdlTenderOpeningOffice.Name = _TenderOpeningOffice.Name;
            mdlTenderOpeningOffice.Address = _TenderOpeningOffice.Address;
            mdlTenderOpeningOffice.IsActive = _TenderOpeningOffice.IsActive;
            //mdlTenderOpeningOffice.CreatedBy = _TenderOpeningOffice.CreatedBy;
            //mdlTenderOpeningOffice.CreatedDate = _TenderOpeningOffice.CreatedDate;
            mdlTenderOpeningOffice.ModifiedBy = _TenderOpeningOffice.ModifiedBy;
            mdlTenderOpeningOffice.ModifiedDate = _TenderOpeningOffice.ModifiedDate;

            db.Repository<TM_TenderOpeningOffice>().Update(mdlTenderOpeningOffice);
            db.Save();
            return true;
        }

        public bool DeleteTenderOpeningOffice(long _TenderOpeningOffice)
        {
            db.Repository<TM_TenderOpeningOffice>().Delete(_TenderOpeningOffice);
            db.Save();
            return true;
        }

        public bool IsTenderOpeningOfficeIDExists(long _TenderOpeningOfficeID)
        {
            bool qIsExists = db.Repository<TM_TenderWorks>().GetAll().Any(c => c.OpeningOfficeID == _TenderOpeningOfficeID);
            return qIsExists;
        }

        public List<TM_Contractors> GetAllContractors()
        {
            List<TM_Contractors> lstContractors = db.Repository<TM_Contractors>().GetAll().OrderBy(n => n.CompanyName).ToList<TM_Contractors>();
            return lstContractors;
        }

        public TM_Contractors GetContractorByName(string _Contractor)
        {
            TM_Contractors mdlContractors = db.Repository<TM_Contractors>().GetAll().Where(z => z.CompanyName.Trim().ToUpper() == _Contractor.Trim().ToUpper()).FirstOrDefault();
            return mdlContractors;
        }

        public bool AddContractor(TM_Contractors _Contractor)
        {
            db.Repository<TM_Contractors>().Insert(_Contractor);
            db.Save();
            return true;
        }

        public bool UpdateContractor(TM_Contractors _Contractor)
        {
            TM_Contractors mdlContractor = db.Repository<TM_Contractors>().FindById(_Contractor.ID);
            mdlContractor.CompanyName = _Contractor.CompanyName;
            mdlContractor.ContactPerson = _Contractor.ContactPerson;
            mdlContractor.ContactNo = _Contractor.ContactNo;
            mdlContractor.Address = _Contractor.Address;
            mdlContractor.IsActive = _Contractor.IsActive;
            mdlContractor.ModifiedBy = _Contractor.ModifiedBy;
            mdlContractor.ModifiedDate = _Contractor.ModifiedDate;

            db.Repository<TM_Contractors>().Update(mdlContractor);
            db.Save();
            return true;
        }

        public bool DeleteContractor(long _ContractorID)
        {
            db.Repository<TM_Contractors>().Delete(_ContractorID);
            db.Save();
            return true;
        }

        public List<TM_CommitteeMembers> GetAllCommitteeMembers()
        {
            List<TM_CommitteeMembers> lstCommitteeMember = db.Repository<TM_CommitteeMembers>().GetAll().OrderBy(n => n.Name).ToList<TM_CommitteeMembers>();
            return lstCommitteeMember;
        }
        public bool IsCommetteeMembersExist(TM_CommitteeMembers ObjCommitteeMembers)
        {
            bool qIsCommetteeMembersExist = false;
            if (ObjCommitteeMembers.ID == 0)
            {
                qIsCommetteeMembersExist =
                   db.Repository<TM_CommitteeMembers>()
                       .GetAll()
                       .Any(
                           q =>
                               q.Designation == ObjCommitteeMembers.Designation && q.MobilePhone == ObjCommitteeMembers.MobilePhone &&
                               q.Email == ObjCommitteeMembers.Email &&
                               (ObjCommitteeMembers.ID == 0));
            }
            else
            {
                qIsCommetteeMembersExist =
                db.Repository<TM_CommitteeMembers>()
                    .GetAll()
                    .Any(
                        q =>
                            q.MobilePhone == ObjCommitteeMembers.MobilePhone &&
                            q.Email == ObjCommitteeMembers.Email &&
                            (q.ID != ObjCommitteeMembers.ID));
            }
            return qIsCommetteeMembersExist;
        }

        public TM_CommitteeMembers GetCommitteeMembersByName(string _Contractor)
        {
            TM_CommitteeMembers mdlCommitteeMember = db.Repository<TM_CommitteeMembers>().GetAll().Where(z => z.Name.Trim().ToLower() == _Contractor.Trim().ToLower()).FirstOrDefault();
            return mdlCommitteeMember;
        }

        public bool AddCommitteeMembers(TM_CommitteeMembers _CommitteeMember)
        {
            db.Repository<TM_CommitteeMembers>().Insert(_CommitteeMember);
            db.Save();
            return true;
        }

        public bool UpdateCommitteeMembers(TM_CommitteeMembers _CommitteeMember)
        {
            TM_CommitteeMembers mdlCommitteeMember = db.Repository<TM_CommitteeMembers>().FindById(_CommitteeMember.ID);
            mdlCommitteeMember.Name = _CommitteeMember.Name;
            mdlCommitteeMember.Designation = _CommitteeMember.Designation;
            mdlCommitteeMember.MobilePhone = _CommitteeMember.MobilePhone;
            mdlCommitteeMember.Email = _CommitteeMember.Email;
            mdlCommitteeMember.IsActive = _CommitteeMember.IsActive;
            mdlCommitteeMember.ModifiedBy = _CommitteeMember.ModifiedBy;
            mdlCommitteeMember.ModifiedDate = _CommitteeMember.ModifiedDate;

            db.Repository<TM_CommitteeMembers>().Update(mdlCommitteeMember);
            db.Save();
            return true;
        }

        public bool DeleteCommitteeMembers(long _CommitteeMemberID)
        {
            db.Repository<TM_CommitteeMembers>().Delete(_CommitteeMemberID);
            db.Save();
            return true;
        }

        #endregion

        #region "Tender Notice"

        public List<dynamic> GetDomains()
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetDomains();
        }

        public List<dynamic> GetDivisionByDomainID(long _DomainID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetDivisionByDomainID(_DomainID);
        }

        public long SaveTenderNotice(TM_TenderNotice _TenderNoticeModel)
        {
            long TenderNoticeID = 0;
            try
            {
                if (_TenderNoticeModel.ID > 0)
                {
                    db.Repository<TM_TenderNotice>().Update(_TenderNoticeModel);
                    db.Save();
                    TenderNoticeID = _TenderNoticeModel.ID;
                }
                else
                {
                    db.Repository<TM_TenderNotice>().Insert(_TenderNoticeModel);
                    db.Save();
                    TenderNoticeID = _TenderNoticeModel.ID;
                }


            }
            catch (Exception)
            {

                throw;
            }

            return TenderNoticeID;

        }

        public bool SavePublishingSource(TM_TenderPublishedIn _TenderPublishingSource)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<TM_TenderPublishedIn>().Insert(_TenderPublishingSource);
                db.Save();

            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }

            return IsSaved;

        }

        public List<TM_TenderNotice> GetAllTenderNotice(string _TenderNoticeName, long _DomainID, long _DivisionID, DateTime? _SubmissionDateFrom, DateTime? _SubmissionDateTo, DateTime? _OpeningDateFrom, DateTime? _OpeningDateTo, long _Status)
        {
            List<TM_TenderNotice> lstTenderNotice = db.Repository<TM_TenderNotice>().GetAll().Where(x =>
                (x.Name.Contains(_TenderNoticeName) || _TenderNoticeName == string.Empty) &&
                (x.DomainID == _DomainID || _DomainID == -1) &&
                (x.DivisionID == _DivisionID || _DivisionID == -1) &&
                (_SubmissionDateFrom == null || x.BidSubmissionDate >= _SubmissionDateFrom) &&
                (_SubmissionDateTo == null || x.BidSubmissionDate <= _SubmissionDateTo) &&
                (_OpeningDateFrom == null || x.BidOpeningDate >= _OpeningDateFrom) &&
                (_OpeningDateTo == null || x.BidOpeningDate <= _OpeningDateTo)).OrderByDescending(x => x.ID).ToList<TM_TenderNotice>();

            if (_Status == 1)
            {
                lstTenderNotice = lstTenderNotice.Where(a => a.BidOpeningDate >= DateTime.Today).ToList();
            }
            else if (_Status == 2)
            {
                lstTenderNotice = lstTenderNotice.Where(a => a.BidOpeningDate < DateTime.Today).ToList();
            }

            return lstTenderNotice;
        }

        public TM_TenderNotice GetTenderNoticeByID(long _TenderNoticeID)
        {
            TM_TenderNotice data = db.Repository<TM_TenderNotice>().GetAll().Where(x => x.ID == _TenderNoticeID).FirstOrDefault();
            return data;
        }

        public List<dynamic> GetPublishingSourceByTenderNoticeID(long _TenderNoticeID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetPublishingSourceByTenderNoticeID(_TenderNoticeID);

        }
        public bool DeletePublishingSourceForUpdation(long _TenderNoticeID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().DeletePublishingSourceforUpdation(_TenderNoticeID);

        }

        public long SaveCommitteeMember(TM_CommitteeMembers _mdlCommitteeMembers)
        {
            if (_mdlCommitteeMembers.ID == 0)
                db.Repository<TM_CommitteeMembers>().Insert(_mdlCommitteeMembers);
            else
                db.Repository<TM_CommitteeMembers>().Update(_mdlCommitteeMembers);
            //  db.Repository<TM_CommitteeMembers>().Insert(_mdlCommitteeMembers);
            db.Save();
            return _mdlCommitteeMembers.ID;
        }

        public bool SaveTenderCommitteeMember(TM_TenderCommitteeMembers _mdlTenderCommitteeMembers)
        {
            db.Repository<TM_TenderCommitteeMembers>().Insert(_mdlTenderCommitteeMembers);
            db.Save();
            return true;
        }


        #endregion

        #region "Works"

        public dynamic GetTenderDataByID(long _TenderNoticeID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetTenderDataByID(_TenderNoticeID);
        }

        public List<dynamic> GetWorksDataByTenderNoticeID(long _TenderNoticeID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetTenderWorksByTenderNoticeID(_TenderNoticeID);
        }

        public List<dynamic> GetClosureWorks(long _DivisionID, string _Year, long _WorkType, long _TenderNoticeID, string _AssestorClosure)
        {
            List<long> lstWorkSourceToExclude;

            if (_AssestorClosure == "1")
                lstWorkSourceToExclude = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.TenderNoticeID == _TenderNoticeID || x.WorkStatusID == 5).Select(x => x.WorkSourceID).Distinct().ToList();
            else
                lstWorkSourceToExclude = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.TenderNoticeID == _TenderNoticeID && x.WorkStatusID == 5 && x.WorkSource == "ASSETWORK").Select(x => x.WorkSourceID).Distinct().ToList();

            return db.ExtRepositoryFor<TenderManagementRepository>().GetClosureWorks(_DivisionID, _Year, _WorkType, _TenderNoticeID, lstWorkSourceToExclude, _AssestorClosure);
        }
        public bool SaveClosureWorkByTenderNoticeID(TM_TenderWorks WorkData)
        {
            bool IsSaved = true;
            try
            {
                db.Repository<TM_TenderWorks>().Insert(WorkData);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public bool DeleteWorkByTenderWorkID(long _TenderWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().DeleteWorkByTenderWorkID(_TenderWorkID);
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
                return db.ExtRepositoryFor<TenderManagementRepository>().GetClosureWorkType();
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
                return db.ExtRepositoryFor<TenderManagementRepository>().GetYearByDivisionID(_DivisionID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetClosureWorkItemsByWorkID(long _WorkID, string _WorkSource)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetClosureWorkItemsByWorkID(_WorkID, _WorkSource);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<dynamic> GetSoldTenderListByWorkID(long _WorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetSoldTenderListByWorkID(_WorkID);
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
                return db.ExtRepositoryFor<TenderManagementRepository>().GetContractorsList(_Name);
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
        //        return db.ExtRepositoryFor<TenderManagementRepository>().GetContractorsList();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        public dynamic GetClosureWorkDataByID(long _WorkID, long _TenderWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetClosureWorkDataByID(_WorkID, _TenderWorkID);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<object> GetTenderWorksListByDivisionID(long? _DivisionID, long? _TenderWorkID)
        {
            DataTable dtTenderWorks = db.ExecuteStoredProcedureDataTable("[TM_GetTenderWorks]", _DivisionID, _TenderWorkID);
            List<object> lsTenderWorks = (from DataRow dr in dtTenderWorks.Rows
                                          select new
                                          {
                                              TenderNoticeID = Convert.ToInt64(dr["TenderNoticeID"].ToString()),
                                              TenderNoticeName = dr["TenderNoticeName"].ToString(),
                                              TenderWorkID = Convert.ToInt64(dr["TenderWorkID"].ToString()),
                                              TenderWorkName = dr["TenderWorkName"].ToString(),
                                              WorkSourceID = Convert.ToInt64(dr["WorkSourceID"].ToString()),
                                              WorkSource = dr["WorkSource"].ToString(),
                                              DivisionID = Convert.ToInt64(dr["DivisionID"].ToString()),
                                              DomainID = Convert.ToInt64(dr["DomainID"].ToString())
                                          }).ToList<object>();
            return lsTenderWorks;
        }

        public DataTable GetTenderWorkDataByWorkID(long _ClosureWorkID)
        {
            DataTable dtTenderWorksData = db.ExecuteStoredProcedureDataTable("[TM_ViewTendersWorksDetails]", _ClosureWorkID);
            return dtTenderWorksData;
        }

        public dynamic GetWorktypeIDandSourceByWorkID(long _WorkSourceID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetWorktypeIDandSourceByWorkID(_WorkSourceID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetElecMechInfoByWorkID(long _ClosureWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetElecMechInfoByWorkID(_ClosureWorkID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetDesiltingByWorkID(long _ClosureWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetDesiltingByWorkID(_ClosureWorkID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetBuildingWorksInfoByWorkID(long _ClosureWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetBuildingWorksInfoByWorkID(_ClosureWorkID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetOilGrePaintingInfoByWorkID(long _ClosureWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetOilGrePaintingInfoByWorkID(_ClosureWorkID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetOutletRepairingInfoByWorkID(long _ClosureWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetOutletRepairingInfoByWorkID(_ClosureWorkID);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public dynamic GetChannelStructWorkInfoByWorkID(long _ClosureWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetChannelStructWorkInfoByWorkID(_ClosureWorkID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetOtherWorkInfoByWorkID(long _ClosureWorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetOtherWorkInfoByWorkID(_ClosureWorkID);
            }
            catch (Exception)
            {

                throw;
            }
        }



        public List<TM_TenderOpeningOffice> GetAllOtherTenderOffices()
        {
            List<TM_TenderOpeningOffice> lstTenderOpeningOffice = db.Repository<TM_TenderOpeningOffice>().GetAll().OrderBy(c => c.Name).ToList<TM_TenderOpeningOffice>();

            return lstTenderOpeningOffice;
        }

        public bool UpdateOpeningOfficeByWorkSourceID(long _WorkSourceID, string _Office, long _OfficeId)
        {

            return db.ExtRepositoryFor<TenderManagementRepository>().UpdateOpeningOfficeByWorkSourceID(_WorkSourceID, _Office, _OfficeId);
        }
        public bool UpdateOpeningOfficeByTenderID(long _ID, string _Office, long _OfficeId)
        {

            return db.ExtRepositoryFor<TenderManagementRepository>().UpdateOpeningOfficeByTenderID(_ID, _Office, _OfficeId);
        }

        public bool CheckOpeningOfficeByWorkSourceID(long _ID)
        {
            bool isOpeningOfficeExists = db.Repository<TM_TenderWorks>().GetAll().Any(i => i.ID == _ID && i.OpeningOfficeID == null && i.OpeningOffice == null);
            return isOpeningOfficeExists;
        }


        public dynamic GetOfficeLocationByZoneID(long _OfficeLocationID, long _WorkSourceID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetOfficeLocationByZoneID(_OfficeLocationID, _WorkSourceID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetOfficeLocationByCircleID(long _OfficeLocationID, long _WorkSourceID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetOfficeLocationByCircleID(_OfficeLocationID, _WorkSourceID);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public dynamic GetOfficeLocationByDivisionID(long _OfficeLocationID, long _WorkSourceID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetOfficeLocationByDivisionID(_OfficeLocationID, _WorkSourceID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetOfficeLocationByOtherID(long _OfficeLocationID, long _WorkSourceID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetOfficeLocationByOtherID(_OfficeLocationID, _WorkSourceID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<object> GetEvalCommitteeMember(long _WorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetEvalCommitteeMember(_WorkID);
        }


        public List<dynamic> GetCommitteeMembersName(string _Name)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetCommitteeMembersName(_Name);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetCommitteeMembersListByWorkID(long _WorkID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetCommitteeMembersListByWorkID(_WorkID);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public long AddContractorFromSoldTenderList(TM_Contractors _Contractor)
        {
            db.Repository<TM_Contractors>().Insert(_Contractor);
            db.Save();
            long ID = _Contractor.ID;
            return ID;
        }

        public long GetTenderNoticeIDByTenderWorkID(long _TenderWorkID)
        {
            long TenderNoticeID = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _TenderWorkID).Select(x => x.TenderNoticeID).FirstOrDefault();
            return TenderNoticeID;
        }
        public dynamic GetTenderWorkContractor(long _TenderWorksContractorID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetTenderWorkContractor(_TenderWorksContractorID);
        }

        public bool SaveTenderWorkStatsuInformation(TM_TenderWorks _mdlTenderWork)
        {
            bool IsSaved = true;
            try
            {
                TM_TenderWorks mdlTenderWork = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _mdlTenderWork.ID).FirstOrDefault();
                mdlTenderWork.StatusBy = _mdlTenderWork.StatusBy;
                if (_mdlTenderWork.WorkStatusID == 0)
                {
                    mdlTenderWork.WorkStatusID = mdlTenderWork.WorkStatusID;
                }
                else
                {
                    mdlTenderWork.WorkStatusID = _mdlTenderWork.WorkStatusID;
                }
                mdlTenderWork.StatusDate = _mdlTenderWork.StatusDate;
                mdlTenderWork.StatusReason = _mdlTenderWork.StatusReason;
                mdlTenderWork.StatusAttachment = _mdlTenderWork.StatusAttachment;
                db.Repository<TM_TenderWorks>().Update(mdlTenderWork);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        #endregion

        #region "Tender Opening Process"
        public List<object> GetTenderCommitteeMembers(long _TenderWorkID)
        {
            DataTable dtMembers = db.ExecuteStoredProcedureDataTable("[TM_GetTenderCommitteeMembers]", _TenderWorkID);
            List<object> lstMembers = (from DataRow dr in dtMembers.Rows
                                       select new
                                       {
                                           AttendanceID = Convert.ToInt64(dr["AttendanceID"]),
                                           TenderWorkID = Convert.ToInt64(dr["TenderWorkID"]),
                                           MemberID = Convert.ToInt64(dr["MemberID"]),
                                           MemberName = dr["MemberName"].ToString(),
                                           MemberDesignation = dr["MemberDesignation"].ToString(),
                                           isAttended = (dr["isAttended"] == DBNull.Value) ? false : Convert.ToBoolean(dr["isAttended"]),
                                           AlternateName = dr["AlternateName"].ToString(),
                                           AlternateDesignation = dr["AlternateDesignation"].ToString(),
                                           MonitoredBy = dr["MonitoredBy"].ToString(),
                                           MonitoredName = dr["MonitoredName"].ToString(),
                                           OpenedBy = dr["OpenedBy"].ToString(),
                                           Attachment = dr["Attachment"].ToString(),
                                           AttachmentList = GetTenderAttendanceAttachment(_TenderWorkID, "ECA"),
                                           MonitoringUserDesig = dr["MonitoringUserDesig"].ToString(),
                                           MonitoringUserName = dr["MonitoringUserName"].ToString()
                                       }
                                           ).ToList<object>();
            return lstMembers;
        }

        public List<object> GetTenderContractors(long _TenderWorkID)
        {
            DataTable dtMembers = db.ExecuteStoredProcedureDataTable("[TM_GetTenderContractors]", _TenderWorkID);
            List<object> lstMembers = (from DataRow dr in dtMembers.Rows
                                       select new
                                       {
                                           AttendanceID = Convert.ToInt64(dr["AttendanceID"]),
                                           TenderWorkID = Convert.ToInt64(dr["TenderWorkID"]),
                                           ContractorID = Convert.ToInt64(dr["ContractorID"]),
                                           CompanyName = dr["CompanyName"].ToString(),
                                           ContractorName = dr["ContractorName"].ToString(),
                                           isAttended = (dr["isAttended"] == DBNull.Value) ? false : Convert.ToBoolean(dr["isAttended"]),
                                           AlternateName = dr["AlternateName"].ToString(),
                                           AlternateRemarks = dr["AlternateRemarks"].ToString(),
                                           MonitoredBy = dr["MonitoredBy"].ToString(),
                                           MonitoredName = dr["MonitoredName"].ToString(),
                                           Attachment = dr["Attachment"].ToString(),
                                           TenderPriced = (dr["TenderPriced"] == DBNull.Value) ? false : Convert.ToBoolean(dr["TenderPriced"]),
                                           TP_EstimateType = dr["TP_EstimateType"].ToString(),
                                           TP_EstimatePercentage = (dr["TP_EstimatePercentage"] == DBNull.Value) ? 0.0 : Convert.ToDouble(dr["TP_EstimatePercentage"]),
                                           Rejected = (dr["Rejected"] == DBNull.Value) ? false : Convert.ToBoolean(dr["Rejected"]),
                                           RejectedReason = dr["RejectedReason"].ToString(),
                                           MonitoringUserDesig = dr["MonitoringUserDesig"].ToString(),
                                           MonitoringUserName = dr["MonitoringUserName"].ToString()

                                       }).AsEnumerable().Select(q => new
                                       {
                                           AttendanceID = q.AttendanceID,
                                           TenderWorkID = q.TenderWorkID,
                                           ContractorID = q.ContractorID,
                                           CompanyName = q.CompanyName,
                                           ContractorName = q.ContractorName,
                                           isAttended = q.isAttended,
                                           AlternateName = q.AlternateName,
                                           AlternateRemarks = q.AlternateRemarks,
                                           MonitoredBy = q.MonitoredBy,
                                           MonitoredName = q.MonitoredName,
                                           Attachment = q.MonitoredName,
                                           AttachmentList = GetTenderAttendanceAttachment(_TenderWorkID, "CA"),
                                           TenderPriced = q.TenderPriced,
                                           TP_EstimateType = q.TP_EstimateType,
                                           TP_EstimatePercentage = q.TP_EstimatePercentage,
                                           Rejected = q.Rejected,
                                           RejectedReason = q.RejectedReason,
                                           MonitoringUserDesig = q.MonitoringUserDesig,
                                           MonitoringUserName = q.MonitoringUserName
                                       }).ToList<object>();
            return lstMembers;
        }

        public bool SaveTenderWorkContractors(TM_TenderWorksContractors _mdlTenderWorkContractor)
        {
            bool IsSaved = true;
            try
            {
                if (_mdlTenderWorkContractor.ID != 0)
                {
                    TM_TenderWorksContractors mdlTenderWorksContractor = db.Repository<TM_TenderWorksContractors>().FindById(_mdlTenderWorkContractor.ID);
                    mdlTenderWorksContractor.BankReceipt = _mdlTenderWorkContractor.BankReceipt;
                    mdlTenderWorksContractor.ContractorsID = _mdlTenderWorkContractor.ContractorsID;
                    db.Repository<TM_TenderWorksContractors>().Update(mdlTenderWorksContractor);
                    db.Save();
                }
                else
                {
                    db.Repository<TM_TenderWorksContractors>().Insert(_mdlTenderWorkContractor);
                    db.Save();
                }

            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public void UpdateTenderWorkStatusByTenderWorkID(long _TenderWorkID, int _WorkStatusID)
        {

            try
            {
                TM_TenderWorks mdlTenderWorks = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _TenderWorkID).FirstOrDefault();
                mdlTenderWorks.WorkStatusID = _WorkStatusID;
                db.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<dynamic> GetUsersByDivisionID(long _DivisionID, long _DesignationID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetUsersByDivisionID(_DivisionID, _DesignationID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int UpdateTenderCommitteeMembers(string _MembersXml)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().UpdateTenderCommitteeMembers(_MembersXml);
        }


        public object GetTenderCommitteeMemberByID(long _ID)
        {
            return db.Repository<TM_TenderCommitteeMembers>().GetAll().Where(s => s.ID == _ID).Select(c => new { c.ID, c.CommitteeMembersID, c.TenderWorksID, c.CreatedBy, c.CreatedDate }).FirstOrDefault();

        }

        public bool SaveEvaluationCommitteeAttendance(List<TM_TenderCommitteeMembers> lstAttendance, List<string> lstNameofFiles, long _TenderWorkID, string _MonitoredBy, string _MonitoredByName, string _OpenedBy, string ECA_Attachment, long _UserID)
        {
            bool IsSaved = true;
            try
            {
                foreach (var ls in lstAttendance)
                {
                    UpdateTenderEvaluationCommittee(ls);

                }
                IsSaved = true;
                // Attachments work left
                if (IsSaved)
                {
                    TM_TenderWorks tenderWork = db.Repository<TM_TenderWorks>().FindById(_TenderWorkID);
                    tenderWork.TenderNoticeID = Convert.ToInt64(tenderWork.TenderNoticeID);
                    tenderWork.WorkSource = tenderWork.WorkSource;
                    tenderWork.WorkSourceID = Convert.ToInt64(tenderWork.WorkSourceID);

                    tenderWork.OpeningOffice = tenderWork.OpeningOffice;
                    tenderWork.OpeningOfficeID = Convert.ToInt64(tenderWork.OpeningOfficeID);

                    tenderWork.WorkStatusID = Convert.ToInt32(tenderWork.WorkStatusID);
                    tenderWork.StatusReason = tenderWork.StatusReason;
                    tenderWork.StatusAttachment = tenderWork.StatusAttachment;
                    tenderWork.StatusDate = (tenderWork.StatusDate != null) ? Convert.ToDateTime(tenderWork.StatusDate) : (tenderWork.StatusDate);
                    tenderWork.StatusBy = Convert.ToInt32(tenderWork.StatusBy);


                    tenderWork.ADM_ActualSubmissionDate = (tenderWork.ADM_ActualSubmissionDate != null || tenderWork.ADM_ActualSubmissionDate.HasValue) ? Convert.ToDateTime(tenderWork.ADM_ActualSubmissionDate) : (tenderWork.ADM_ActualSubmissionDate);
                    tenderWork.ADM_ActualSubmissionReason = tenderWork.ADM_ActualSubmissionReason;
                    tenderWork.ADM_ActualOpeningDate = (tenderWork.ADM_ActualOpeningDate != null || tenderWork.ADM_ActualOpeningDate.HasValue) ? Convert.ToDateTime(tenderWork.ADM_ActualOpeningDate) : (tenderWork.ADM_ActualOpeningDate); ;
                    tenderWork.ADM_ActualOpeningReason = tenderWork.ADM_ActualOpeningReason;
                    tenderWork.ADM_Observation = tenderWork.ADM_Observation;
                    tenderWork.ADM_TenderBoxEmpty = tenderWork.ADM_TenderBoxEmpty;
                    tenderWork.ADM_RateNoted = tenderWork.ADM_RateNoted;
                    tenderWork.ADM_MebersPresent = tenderWork.ADM_MebersPresent;
                    tenderWork.ADM_Photography = tenderWork.ADM_Photography;
                    tenderWork.ADM_XENRequested = tenderWork.ADM_XENRequested;

                    tenderWork.CreatedDate = Convert.ToDateTime(tenderWork.CreatedDate);
                    tenderWork.CreatedBy = Convert.ToInt32(tenderWork.CreatedBy);

                    tenderWork.CA_MonitoredBy = tenderWork.CA_MonitoredBy;
                    tenderWork.CA_MonitoredName = tenderWork.CA_MonitoredName;
                    tenderWork.CA_Attachment = tenderWork.CA_Attachment;

                    tenderWork.ECA_MonitoredBy = _MonitoredBy;
                    tenderWork.ECA_MonitoredName = _MonitoredByName;
                    tenderWork.ECA_OpenedBy = _OpenedBy;
                    tenderWork.ECA_Attachment = tenderWork.ECA_Attachment;

                    tenderWork.ModifiedBy = Convert.ToInt32(_UserID);
                    tenderWork.ModifiedDate = DateTime.Now;

                    try
                    {
                        db.Repository<TM_TenderWorks>().Update(tenderWork);
                        db.Save();
                        IsSaved = true;
                        if (IsSaved && DeleteAttendanceAttachmentByTenderWorkID(_TenderWorkID, "ECA"))
                        {
                            for (int i = 0; i < lstNameofFiles.Count; ++i)
                            {
                                TM_TenderWorkAttachment mdlTWAtchmnt = new TM_TenderWorkAttachment();

                                mdlTWAtchmnt.TenderWorksID = _TenderWorkID;
                                mdlTWAtchmnt.Attachment = lstNameofFiles[i];
                                mdlTWAtchmnt.AttachmentType = Path.GetExtension(lstNameofFiles[i]).TrimStart('.').ToString().ToUpper();
                                mdlTWAtchmnt.Source = "ECA";
                                mdlTWAtchmnt.CreatedBy = Convert.ToInt32(_UserID);
                                mdlTWAtchmnt.CreatedDate = DateTime.Now;
                                db.Repository<TM_TenderWorkAttachment>().Insert(mdlTWAtchmnt);
                                db.Save();
                                IsSaved = true;
                            }


                        }
                    }
                    catch (Exception)
                    {
                        IsSaved = false;
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public object GetTenderContractorByID(long _ID)
        {
            return db.Repository<TM_TenderWorksContractors>().GetAll().Where(s => s.ID == _ID).Select(c => new { c.ID, c.ContractorsID, c.TenderWorksID, c.TenderPriced, c.CreatedBy, c.CreatedDate }).FirstOrDefault();
        }

        public bool UpdateTenderEvaluationCommittee(TM_TenderCommitteeMembers _TenderCommittee)
        {
            TM_TenderCommitteeMembers mdlTenderCommittee = db.Repository<TM_TenderCommitteeMembers>().FindById(_TenderCommittee.ID);
            mdlTenderCommittee.Attended = _TenderCommittee.Attended;
            mdlTenderCommittee.AlternateName = _TenderCommittee.AlternateName;
            mdlTenderCommittee.AlternateDesignation = _TenderCommittee.AlternateDesignation;
            mdlTenderCommittee.ModifiedBy = _TenderCommittee.ModifiedBy;
            mdlTenderCommittee.ModifiedDate = DateTime.Now;
            db.Repository<TM_TenderCommitteeMembers>().Update(mdlTenderCommittee);
            db.Save();
            return true;
        }

        public bool SaveTenderWorkAttachments(TM_TenderWorkAttachment _TenderWorkAttachment)
        {
            db.Repository<TM_TenderWorkAttachment>().Insert(_TenderWorkAttachment);
            db.Save();
            return true;
        }
        public bool UpdateTenderContractors(TM_TenderWorksContractors _TenderContractors)
        {
            TM_TenderWorksContractors mdlTenderContractors = db.Repository<TM_TenderWorksContractors>().FindById(_TenderContractors.ID);
            mdlTenderContractors.Attended = _TenderContractors.Attended;
            mdlTenderContractors.AlternateName = _TenderContractors.AlternateName;
            mdlTenderContractors.AlternateRemarks = _TenderContractors.AlternateRemarks;
            mdlTenderContractors.ModifiedBy = _TenderContractors.ModifiedBy;
            mdlTenderContractors.ModifiedDate = DateTime.Now;
            db.Repository<TM_TenderWorksContractors>().Update(mdlTenderContractors);
            db.Save();
            return true;
        }
        public bool UpdateTenderContractorsForADMReport(TM_TenderWorksContractors _TenderContractors)
        {
            TM_TenderWorksContractors mdlTenderContractors = db.Repository<TM_TenderWorksContractors>().FindById(_TenderContractors.ID);
            mdlTenderContractors.Rejected = _TenderContractors.Rejected;
            mdlTenderContractors.RejectedReason = _TenderContractors.RejectedReason;
            mdlTenderContractors.ModifiedBy = _TenderContractors.ModifiedBy;
            mdlTenderContractors.ModifiedDate = _TenderContractors.ModifiedDate;
            db.Repository<TM_TenderWorksContractors>().Update(mdlTenderContractors);
            db.Save();
            return true;
        }

        public bool SaveADMAttachemnt(TM_ADMAttachment _mdlADMAttachment)
        {
            db.Repository<TM_ADMAttachment>().Insert(_mdlADMAttachment);
            db.Save();
            return true;
        }
        public bool UpdateTenderWorksByWorkID(TM_TenderWorks _TenderWorks)
        {
            TM_TenderWorks mdlTenderWorks = db.Repository<TM_TenderWorks>().FindById(_TenderWorks.ID);
            mdlTenderWorks.ECA_Attachment = _TenderWorks.ECA_Attachment;
            mdlTenderWorks.ECA_MonitoredBy = _TenderWorks.ECA_MonitoredBy;
            mdlTenderWorks.ECA_MonitoredName = _TenderWorks.ECA_MonitoredName;
            mdlTenderWorks.ECA_OpenedBy = _TenderWorks.ECA_OpenedBy;
            mdlTenderWorks.ModifiedBy = _TenderWorks.ModifiedBy;
            mdlTenderWorks.ModifiedDate = DateTime.Now;
            db.Repository<TM_TenderWorks>().Update(mdlTenderWorks);
            db.Save();
            return true;
        }

        public bool UpdateTenderWorksforContractorsByWorkID(TM_TenderWorks _TenderWorks)
        {
            TM_TenderWorks mdlTenderWorks = db.Repository<TM_TenderWorks>().FindById(_TenderWorks.ID);
            mdlTenderWorks.CA_Attachment = _TenderWorks.CA_Attachment;
            mdlTenderWorks.CA_MonitoredBy = _TenderWorks.CA_MonitoredBy;
            mdlTenderWorks.CA_MonitoredName = _TenderWorks.CA_MonitoredName;
            mdlTenderWorks.ModifiedBy = _TenderWorks.ModifiedBy;
            mdlTenderWorks.ModifiedDate = DateTime.Now;
            db.Repository<TM_TenderWorks>().Update(mdlTenderWorks);
            db.Save();
            return true;
        }
        public bool UpdateTenderWorksforADMReport(TM_TenderWorks _TenderWorks)
        {
            TM_TenderWorks mdlTenderWorks = db.Repository<TM_TenderWorks>().FindById(_TenderWorks.ID);
            mdlTenderWorks.ADM_ActualOpeningDate = _TenderWorks.ADM_ActualOpeningDate;
            mdlTenderWorks.ADM_ActualOpeningReason = _TenderWorks.ADM_ActualOpeningReason;
            mdlTenderWorks.ADM_ActualSubmissionDate = _TenderWorks.ADM_ActualSubmissionDate;
            mdlTenderWorks.ADM_ActualSubmissionReason = _TenderWorks.ADM_ActualSubmissionReason;
            mdlTenderWorks.ADM_MebersPresent = _TenderWorks.ADM_MebersPresent;
            mdlTenderWorks.ADM_Observation = _TenderWorks.ADM_Observation;
            mdlTenderWorks.ADM_Photography = _TenderWorks.ADM_Photography;
            mdlTenderWorks.ADM_RateNoted = _TenderWorks.ADM_RateNoted;
            mdlTenderWorks.ADM_TenderBoxEmpty = _TenderWorks.ADM_TenderBoxEmpty;
            mdlTenderWorks.ADM_XENRequested = _TenderWorks.ADM_XENRequested;
            if (_TenderWorks.WorkStatusID != 0)
            {
                mdlTenderWorks.WorkStatusID = _TenderWorks.WorkStatusID;
                mdlTenderWorks.StatusReason = _TenderWorks.StatusReason;
            }
            mdlTenderWorks.ModifiedBy = _TenderWorks.ModifiedBy;
            mdlTenderWorks.ModifiedDate = DateTime.Now;
            db.Repository<TM_TenderWorks>().Update(mdlTenderWorks);
            db.Save();
            return true;
        }
        public dynamic GetECMDataByWorkID(long _WorkID)
        {
            return db.Repository<TM_TenderWorks>().GetAll().Where(s => s.ID == _WorkID).Select(c => new { c.ID, c.ECA_MonitoredBy, c.ECA_MonitoredName, c.ECA_OpenedBy, c.ECA_Attachment }).FirstOrDefault();

        }

        public dynamic GetCMDataByTenderWorkID(long _TenderWorkID)
        {
            return db.Repository<TM_TenderWorks>().GetAll().Where(s => s.ID == _TenderWorkID).Select(c => new { c.ID, c.ECA_MonitoredBy, c.ECA_MonitoredName, c.CA_Attachment }).FirstOrDefault();

        }


        public bool SaveContractorAttendance(List<TM_TenderWorksContractors> lstAttendance, List<string> lstNameofFiles, long _TenderWorkID, string _MonitoredBy, string _MonitoredByName, string ECA_Attachment, long _UserID)
        {
            bool IsSaved = true;
            try
            {
                foreach (var ls in lstAttendance)
                {
                    UpdateTenderContractors(ls);

                }
                IsSaved = true;
                // Attachments work left
                if (IsSaved)
                {
                    TM_TenderWorks tenderWork = db.Repository<TM_TenderWorks>().FindById(_TenderWorkID);
                    tenderWork.TenderNoticeID = Convert.ToInt64(tenderWork.TenderNoticeID);
                    tenderWork.WorkSource = tenderWork.WorkSource;
                    tenderWork.WorkSourceID = Convert.ToInt64(tenderWork.WorkSourceID);

                    tenderWork.OpeningOffice = tenderWork.OpeningOffice;
                    tenderWork.OpeningOfficeID = Convert.ToInt64(tenderWork.OpeningOfficeID);

                    tenderWork.WorkStatusID = Convert.ToInt32(tenderWork.WorkStatusID);
                    tenderWork.StatusReason = tenderWork.StatusReason;
                    tenderWork.StatusAttachment = tenderWork.StatusAttachment;
                    tenderWork.StatusDate = (tenderWork.StatusDate != null) ? Convert.ToDateTime(tenderWork.StatusDate) : (tenderWork.StatusDate);
                    tenderWork.StatusBy = Convert.ToInt32(tenderWork.StatusBy);

                    tenderWork.ADM_ActualSubmissionDate = (tenderWork.ADM_ActualSubmissionDate != null || tenderWork.ADM_ActualSubmissionDate.HasValue) ? Convert.ToDateTime(tenderWork.ADM_ActualSubmissionDate) : (tenderWork.ADM_ActualSubmissionDate);
                    tenderWork.ADM_ActualSubmissionReason = tenderWork.ADM_ActualSubmissionReason;
                    tenderWork.ADM_ActualOpeningDate = (tenderWork.ADM_ActualOpeningDate != null || tenderWork.ADM_ActualOpeningDate.HasValue) ? Convert.ToDateTime(tenderWork.ADM_ActualOpeningDate) : (tenderWork.ADM_ActualOpeningDate); ;
                    tenderWork.ADM_ActualOpeningReason = tenderWork.ADM_ActualOpeningReason;
                    tenderWork.ADM_Observation = tenderWork.ADM_Observation;
                    tenderWork.ADM_TenderBoxEmpty = tenderWork.ADM_TenderBoxEmpty;
                    tenderWork.ADM_RateNoted = tenderWork.ADM_RateNoted;
                    tenderWork.ADM_MebersPresent = tenderWork.ADM_MebersPresent;
                    tenderWork.ADM_Photography = tenderWork.ADM_Photography;
                    tenderWork.ADM_XENRequested = tenderWork.ADM_XENRequested;


                    tenderWork.CreatedDate = Convert.ToDateTime(tenderWork.CreatedDate);
                    tenderWork.CreatedBy = Convert.ToInt32(tenderWork.CreatedBy);

                    tenderWork.CA_MonitoredBy = _MonitoredBy;
                    tenderWork.CA_MonitoredName = _MonitoredByName;
                    tenderWork.CA_Attachment = ECA_Attachment;

                    tenderWork.ECA_MonitoredBy = tenderWork.ECA_MonitoredBy;
                    tenderWork.ECA_MonitoredName = tenderWork.ECA_MonitoredName;
                    tenderWork.ECA_OpenedBy = tenderWork.ECA_OpenedBy;
                    tenderWork.ECA_Attachment = tenderWork.ECA_Attachment;

                    tenderWork.ModifiedBy = Convert.ToInt32(_UserID);
                    tenderWork.ModifiedDate = DateTime.Now;

                    try
                    {
                        db.Repository<TM_TenderWorks>().Update(tenderWork);
                        db.Save();
                        IsSaved = true;

                        if (IsSaved && DeleteAttendanceAttachmentByTenderWorkID(_TenderWorkID, "CA"))
                        {

                            for (int i = 0; i < lstNameofFiles.Count; ++i)
                            {
                                TM_TenderWorkAttachment mdlTWAtchmnt = new TM_TenderWorkAttachment();
                                mdlTWAtchmnt.ID = db.Repository<TM_TenderWorkAttachment>().GetAll().Count() + 1;
                                mdlTWAtchmnt.TenderWorksID = _TenderWorkID;
                                mdlTWAtchmnt.Attachment = lstNameofFiles[i].ToString();
                                mdlTWAtchmnt.AttachmentType = Path.GetExtension(lstNameofFiles[i]).TrimStart('.').ToString().ToUpper();
                                mdlTWAtchmnt.Source = "CA";
                                mdlTWAtchmnt.CreatedBy = Convert.ToInt32(_UserID);
                                mdlTWAtchmnt.CreatedDate = DateTime.Now;
                                db.Repository<TM_TenderWorkAttachment>().Insert(mdlTWAtchmnt);
                                db.Save();
                                IsSaved = true;

                            }


                        }
                    }

                    catch (Exception e)
                    {
                        IsSaved = false;
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public List<object> GetTenderWorkItemsByTenderWorKID(long? _TenderWorkID)
        {
            DataTable dtTenderItems = db.ExecuteStoredProcedureDataTable("[TM_GetTenderWorkItemsByTenderWorKID]", _TenderWorkID);
            List<object> lsTenderItems = (from DataRow dr in dtTenderItems.Rows
                                          select new
                                          {
                                              ID = Convert.ToInt64(dr["ID"].ToString()),
                                              WorkType = dr["WorkType"].ToString(),
                                              workID = Convert.ToInt64(dr["workID"].ToString()),
                                              ItemDescription = dr["ItemDescription"].ToString(),
                                              SanctionedQty = Convert.ToInt64(dr["SanctionedQty"].ToString()),
                                              TSUnitID = Convert.ToInt32(dr["TSUnitID"].ToString()),
                                              TSUnitName = dr["TSUnitName"].ToString(),
                                              TSRate = Convert.ToDouble(dr["TSRate"].ToString())
                                          }).ToList<object>();
            return lsTenderItems;
        }

        public List<object> GetWorkItemsDetailsByWorKID(long _TenderWorkID, long _CompanyID, long _TenderNoticeID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetWorkItemsDetailsByWorKID(_TenderWorkID, _CompanyID, _TenderNoticeID);
        }

        public List<object> GetCompanyNameByWorkID(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetCompanyNameByWorkID(_TenderWorkID);
        }

        public List<dynamic> GetContractorsListBytenderWorkID(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().getContractorsListBytenderWorkID(_TenderWorkID);
        }
        public List<object> GetTenderAttendanceAttachment(long _TenderWorkID, string source)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetTenderAttendanceAttachment(_TenderWorkID, source);

        }
        public bool DeleteAttendanceAttachmentByTenderWorkID(long _TenderWorkID, string _source)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().DeleteAttendanceAttachmentByTenderWorkID(_TenderWorkID, _source);
        }
        public List<dynamic> GetADMReportDataByTenderWorkID(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetADMReportDataByTenderWorkID(_TenderWorkID);
        }

        public dynamic GetTenderInformationForADMReport(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetTenderInformationForADMReport(_TenderWorkID);
        }

        public List<object> GetCallDepositDataByWorKID(long _TenderWorkID, long _CompanyID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetCallDepositDataByWorKID(_TenderWorkID, _CompanyID);
        }

        public long UpdateTenderWorkContractorDataByID(TM_TenderWorksContractors _TenderWorksContractor)
        {
            long TWContractorID = db.Repository<TM_TenderWorksContractors>().GetAll().Where(s => s.TenderWorksID == _TenderWorksContractor.TenderWorksID && s.ContractorsID == _TenderWorksContractor.ContractorsID).Select(s => s.ID).FirstOrDefault();

            TM_TenderWorksContractors mdlTenderWorksContractor = db.Repository<TM_TenderWorksContractors>().FindById(TWContractorID);
            mdlTenderWorksContractor.TenderPriced = _TenderWorksContractor.TenderPriced;
            mdlTenderWorksContractor.TP_EstimatePercentage = _TenderWorksContractor.TP_EstimatePercentage;
            mdlTenderWorksContractor.TP_EstimateType = _TenderWorksContractor.TP_EstimateType;
            mdlTenderWorksContractor.TenderWorkAmount = _TenderWorksContractor.TenderWorkAmount;
            db.Repository<TM_TenderWorksContractors>().Update(mdlTenderWorksContractor);
            db.Save();
            return mdlTenderWorksContractor.ID;
        }

        public bool AddTenderPriceValue(TM_TenderPrice _TenderPrice)
        {
            bool IsSaved = true;
            try
            {
                if (_TenderPrice.ID == 0)
                    db.Repository<TM_TenderPrice>().Insert(_TenderPrice);
                else
                    db.Repository<TM_TenderPrice>().Update(_TenderPrice);
                db.Save();
                IsSaved = true;

            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }


        public bool AddorUpdateTenderPriceCDR(TM_TenderPriceCDR _TenderPriceCDR)
        {
            bool IsSaved = true;
            try
            {
                long ID = db.Repository<TM_TenderPriceCDR>().GetAll().Where(s => s.ID == _TenderPriceCDR.ID).Select(s => s.ID).FirstOrDefault();
                if (ID != 0)
                {
                    TM_TenderPriceCDR mdlTenderPriceCDR = db.Repository<TM_TenderPriceCDR>().FindById(ID);
                    mdlTenderPriceCDR.CDRNo = _TenderPriceCDR.CDRNo;
                    mdlTenderPriceCDR.Amount = _TenderPriceCDR.Amount;
                    mdlTenderPriceCDR.Attachment = _TenderPriceCDR.Attachment;
                    mdlTenderPriceCDR.ModifiedBy = _TenderPriceCDR.CreatedBy;
                    mdlTenderPriceCDR.ModifiedDate = DateTime.Now;
                    mdlTenderPriceCDR.TWContractorID = _TenderPriceCDR.TWContractorID;
                    db.Repository<TM_TenderPriceCDR>().Update(mdlTenderPriceCDR);
                }



                else
                    db.Repository<TM_TenderPriceCDR>().Insert(_TenderPriceCDR);

                db.Save();
                IsSaved = true;


            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public bool AddUpdateTenderPriceCDR(TM_TenderPriceCDR _TenderPriceCDR)
        {
            bool IsSaved = true;
            try
            {
                long ID = db.Repository<TM_TenderPriceCDR>().GetAll().Where(s => s.ID == _TenderPriceCDR.ID).Select(s => s.ID).FirstOrDefault();
                if (ID != 0)
                {
                    TM_TenderPriceCDR mdlTenderPriceCDR = db.Repository<TM_TenderPriceCDR>().FindById(ID);
                    mdlTenderPriceCDR.CDRNo = _TenderPriceCDR.CDRNo;
                    mdlTenderPriceCDR.Amount = _TenderPriceCDR.Amount;
                    mdlTenderPriceCDR.Attachment = _TenderPriceCDR.Attachment;
                    mdlTenderPriceCDR.ModifiedBy = _TenderPriceCDR.CreatedBy;
                    mdlTenderPriceCDR.ModifiedDate = DateTime.Now;
                    mdlTenderPriceCDR.TWContractorID = _TenderPriceCDR.TWContractorID;
                    db.Repository<TM_TenderPriceCDR>().Update(mdlTenderPriceCDR);
                }



                else
                    db.Repository<TM_TenderPriceCDR>().Insert(_TenderPriceCDR);

                db.Save();
                IsSaved = true;


            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }

        public bool UpdateTenderPriceCDR(TM_TenderPriceCDR _TenderPriceCDR)
        {
            bool IsSaved = true;
            try
            {
                long ID = db.Repository<TM_TenderPriceCDR>().GetAll().Where(s => s.ID == _TenderPriceCDR.ID).Select(s => s.ID).FirstOrDefault();
                if (ID != 0)
                {
                    TM_TenderPriceCDR mdlTenderPriceCDR = db.Repository<TM_TenderPriceCDR>().FindById(ID);
                    mdlTenderPriceCDR.CDRNo = _TenderPriceCDR.CDRNo;
                    mdlTenderPriceCDR.Amount = _TenderPriceCDR.Amount;
                    mdlTenderPriceCDR.Attachment = _TenderPriceCDR.Attachment;
                    mdlTenderPriceCDR.ModifiedBy = _TenderPriceCDR.CreatedBy;
                    mdlTenderPriceCDR.ModifiedDate = DateTime.Now;
                    //  mdlTenderPriceCDR.TWContractorID = _TenderPriceCDR.TWContractorID;
                    db.Repository<TM_TenderPriceCDR>().Update(mdlTenderPriceCDR);
                }


                db.Save();
                IsSaved = true;

            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }


        public List<object> GetContractorCallDeposit(long _TWContractorID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetContractorCallDeposit(_TWContractorID);
        }

        public List<object> GetWorkItemsforViewByWorKID(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetWorkItemsforViewByWorKID(_TenderWorkID);
        }

        public List<object> GetCallDepositDataforViewByWorKID(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetCallDepositDataforViewByWorKID(_TenderWorkID);
        }

        public List<TM_ADMAttachment> GetAllADMAttachemnts(long _TenderWorkID)
        {
            return db.Repository<TM_ADMAttachment>().GetAll().Where(x => x.TenderWorksID == _TenderWorkID).ToList();
        }

        public List<TM_TenderWorkAttachment> GetAllTenderWorkAttachments(long _TenderWorkID, string _Source)
        {
            return db.Repository<TM_TenderWorkAttachment>().GetAll().Where(x => x.TenderWorksID == _TenderWorkID && x.Source.ToUpper().Trim() == _Source.ToUpper().Trim()).ToList();
        }
        public TM_TenderWorks GetADMReportData(long _TenderWorkID)
        {
            return db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _TenderWorkID).FirstOrDefault();
        }
        public object GetTenderPriceByWorkItemID(long _WorkItemId, long _TWContractorID)
        {
            object tenderPriceObj = db.Repository<TM_TenderPrice>().GetAll().Where(c => c.WorkItemID == _WorkItemId && c.TWContractorID == _TWContractorID).Select(x => new { x.ID, x.TWContractorID, x.WorkItemID, x.ContractorRate, x.CreatedBy, x.CreatedDate }).FirstOrDefault();
            return tenderPriceObj;
        }

        public bool IsTenderPriceAdded(long _WorkItemId, long _TWContractorID)
        {
            bool qIsExists = db.Repository<TM_TenderPrice>().GetAll().Any(c => c.WorkItemID == _WorkItemId && c.TWContractorID == _TWContractorID);
            return qIsExists;//qIsExists;
        }
        public object GetTenderPriceCDRByID(long _ID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetTenderPriceCDRByID(_ID);
        }
        public long? GetContractorIDFromTWContractorID(long _TWContractorID)
        {
            long? TWContractorID = db.Repository<TM_TenderWorksContractors>().GetAll().Where(s => s.ID == _TWContractorID).Select(s => s.ContractorsID).FirstOrDefault();
            return TWContractorID;
        }
        public DataTable GetComparativeStatementDataByWorkandNoticeID(long _WorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetComparativeStatementDataByWorkandNoticeID(_WorkID);
        }
        public bool SaveADMAttachments(long _TenderWorkID, string AttachmentName, long _UserID)
        {
            string AttachmentType = Path.GetExtension(AttachmentName).TrimStart('.').ToString().ToUpper();

            TM_ADMAttachment mdlADMAttachemnt = new TM_ADMAttachment();
            mdlADMAttachemnt.Attachment = AttachmentName;
            mdlADMAttachemnt.TenderWorksID = _TenderWorkID;
            mdlADMAttachemnt.CreatedBy = Convert.ToInt32(_UserID);
            mdlADMAttachemnt.CreatedDate = DateTime.Now;
            mdlADMAttachemnt.AttachmentType = AttachmentType;
            db.Repository<TM_ADMAttachment>().Insert(mdlADMAttachemnt);
            db.Save();

            return true;
        }

        public bool DeleteADMAttachmentByTenderWorkID(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().DeleteADMAttachmentByTenderWorkID(_TenderWorkID);
        }
        public bool IsContractorAlreadyAdded(long _ContractorID, long _TenderWorkID)
        {
            bool IsExists = db.Repository<TM_TenderWorksContractors>().GetAll().Any(x => x.ContractorsID == _ContractorID && x.TenderWorksID == _TenderWorkID);
            return IsExists;

        }
        public bool IsMemberAlreadyAdded(long _MemberID, long _TenderWorkID)
        {
            bool IsExists = db.Repository<TM_TenderCommitteeMembers>().GetAll().Any(x => x.CommitteeMembersID == _MemberID && x.TenderWorksID == _TenderWorkID);
            return IsExists;

        }

        public List<object> GetContractorandAmountByWorkID(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetContractorandAmountByWorkID(_TenderWorkID);
        }

        public long UpdateTenderWorkContractorAwardFieldByID(TM_TenderWorksContractors _TenderWorksContractor)
        {
            //long TWContractorID = db.Repository<TM_TenderWorksContractors>().GetAll().Where(s => s.TenderWorksID == _TenderWorksContractor.TenderWorksID && s.ContractorsID == _TenderWorksContractor.ContractorsID).Select(s => s.ID).FirstOrDefault();

            TM_TenderWorksContractors mdlTenderWorksContractor = db.Repository<TM_TenderWorksContractors>().FindById(_TenderWorksContractor.ID);
            if (_TenderWorksContractor.Awarded == true)
            {
                mdlTenderWorksContractor.Awarded = true;
            }
            else
            {
                mdlTenderWorksContractor.Awarded = false;
            }
            mdlTenderWorksContractor.AwardedRemarks = _TenderWorksContractor.AwardedRemarks;
            db.Repository<TM_TenderWorksContractors>().Update(mdlTenderWorksContractor);
            db.Save();
            return mdlTenderWorksContractor.ID;
        }

        public object GetWorkItemRateBySourceID(long _WorkSourceID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetWorkItemRateBySourceID(_WorkSourceID);
        }
        public List<dynamic> GetMonitoringUsersByDivisionID(long _DivisionID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetMonitoringUsersByDivisionID(_DivisionID);
        }
        public long UpdateTenderWorkStatusIDByWorkID(TM_TenderWorks _TenderWorks)
        {
            TM_TenderWorks mdlTenderWorks = db.Repository<TM_TenderWorks>().FindById(_TenderWorks.ID);
            mdlTenderWorks.WorkStatusID = 5;
            db.Repository<TM_TenderWorks>().Update(mdlTenderWorks);
            db.Save();
            return mdlTenderWorks.ID;

        }

        public bool UpdateAssetWorkStatus(long _WorkSourceID)
        {
            bool IsSaved = true;
            try
            {
                AM_AssetWork mdlAssetWork = db.Repository<AM_AssetWork>().GetAll().Where(x => x.ID == _WorkSourceID).FirstOrDefault();
                mdlAssetWork.WorkStatus = "Contract Awarded";
                db.Repository<AM_AssetWork>().Update(mdlAssetWork);
                db.Save();
            }
            catch (Exception)
            {
                IsSaved = false;
                throw;
            }
            return IsSaved;
        }
        public bool IsMemberExistInTendrCommittee(long _CommitteeMemberID)
        {
            bool qIsExists = db.Repository<TM_TenderCommitteeMembers>().GetAll().Any(c => c.CommitteeMembersID == _CommitteeMemberID);
            return qIsExists;
        }

        public bool IsContractorExist(long _ContractorID)
        {
            bool qIsExists = db.Repository<TM_TenderWorksContractors>().GetAll().Any(c => c.ContractorsID == _ContractorID);
            return qIsExists;
        }
        public bool IsPhoneOrEmailExistInTendrCommittee(string _MobilePhone, string _Email)
        {
            bool qIsExists = db.Repository<TM_CommitteeMembers>().GetAll().Any(c => c.MobilePhone == _MobilePhone || c.Email.ToUpper().Trim() == _Email.ToUpper().Trim());
            return qIsExists;
        }

        public bool IsPhoneExistInContractor(string _MobilePhone)
        {
            bool qIsExists = db.Repository<TM_Contractors>().GetAll().Any(c => c.ContactNo == _MobilePhone);
            return qIsExists;
        }

        /// <summary>
        /// This function searches Contractors based on Contact Number.
        /// Created On 18-08-2017
        /// </summary>
        /// <param name="_ContactNumber"></param>
        /// <returns>TM_Contractors</returns>
        public TM_Contractors GetContractorByContactNumber(string _ContactNumber)
        {
            return db.Repository<TM_Contractors>().GetAll().Where(c => c.ContactNo.Trim() == _ContactNumber.Trim()).FirstOrDefault();
        }

        public bool? GetAwardedTenderByWorkID(long _TenderWorkID)
        {
            bool? IsAwarded = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _TenderWorkID).Select(x => x.WorkStatusID == 5 || x.WorkStatusID == 4).FirstOrDefault();
            return IsAwarded;
        }
        #endregion


        public object GetTenderWorkByID(long? _TenderWorkID)
        {
            DataTable dtTenderWorks = db.ExecuteStoredProcedureDataTable("[TM_GetTenderWorkByID]", _TenderWorkID);
            object lsTenderWorks = (from DataRow dr in dtTenderWorks.Rows
                                    select new
                                    {
                                        TenderNoticeID = Convert.ToInt64(dr["TenderNoticeID"].ToString()),
                                        TenderNoticeName = dr["TenderNoticeName"].ToString(),
                                        TenderWorkID = Convert.ToInt64(dr["TenderWorkID"].ToString()),
                                        TenderWorkName = dr["TenderWorkName"].ToString(),
                                        WorkSourceID = Convert.ToInt64(dr["WorkSourceID"].ToString()),
                                        WorkSource = dr["WorkSource"].ToString(),
                                        DivisionID = Convert.ToInt64(dr["DivisionID"].ToString()),
                                        DomainID = Convert.ToInt64(dr["DomainID"].ToString())
                                    }).FirstOrDefault();
            return lsTenderWorks;
        }


        public List<TM_TenderPriceCDR> DeleteCallDepositByTenderWorkContractorID(long _TendWContractorID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().DeleteCallDepositByTenderWorkContractorID(_TendWContractorID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool IsADMPreparedReport(long _TendWorkID)
        {
            bool IsAwarded = false;
            String Observation = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _TendWorkID).Select(x => x.ADM_Observation).FirstOrDefault();
            if (!string.IsNullOrEmpty(Observation))
            {
                IsAwarded = true;
            }
            return IsAwarded;

        }

        #region
        public List<dynamic> GetAssetTypes()
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetAssetTypes();
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
                return db.ExtRepositoryFor<TenderManagementRepository>().GetAssetYearByDivisionID(_DivisionID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public dynamic GetAssetWorkInfoByWorkID(long _WorkSourceID, long _TenderNoticeID)
        {
            try
            {
                return db.ExtRepositoryFor<TenderManagementRepository>().GetAssetWorkInfoByWorkID(_WorkSourceID, _TenderNoticeID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region

        public List<long> GetTenderNoticeID()
        {
            List<long> TenderNoticeIDs;
            DateTime today = System.DateTime.Now;
            DateTime tomarrow = today.AddDays(1);
            try
            {
                TenderNoticeIDs = db.Repository<TM_TenderNotice>().GetAll().Where(x => DbFunctions.TruncateTime(x.BidOpeningDate) == DbFunctions.TruncateTime(tomarrow)).Select(z => z.ID).ToList<long>();
            }
            catch (Exception)
            {

                throw;
            }

            return TenderNoticeIDs;
        }

        public List<dynamic> GetWorksByTenderID(long _TenderNoticeID)
        {
            List<dynamic> lstWorks;
            try
            {
                lstWorks = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.TenderNoticeID == _TenderNoticeID).ToList<dynamic>();


            }
            catch (Exception)
            {

                throw;
            }

            return lstWorks;
        }

        public List<dynamic> GetClouserWorksByTenderID(long _CWID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetClouserWorksByTenderID(_CWID);
        }

        public List<dynamic> GetAssetWorksByTenderID(long _AWID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetAssetWorksByTenderID(_AWID);
        }

        public string GetWorkSourceByWorkID(long _TenderWorkID)
        {
            string Source = db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _TenderWorkID).Select(z => z.WorkSource).First().ToString();
            return Source;
        }

        public dynamic GetAssetTenderInformationForADMReport(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetAssetTenderInformationForADMReport(_TenderWorkID);
        }

        public string GetDivisionByTenderID(long _TenderID)
        {
            return db.Repository<TM_TenderNotice>().GetAll().Where(x => x.ID == _TenderID).Select(z => z.DivisionID).First().ToString();
        }
        #endregion

        public TM_TenderWorks GetWorkByTenderWorkID(long _TenderWorkID)
        {
            return db.ExtRepositoryFor<TenderManagementRepository>().GetWorkByTenderWorkID(_TenderWorkID);
        }

        public TM_TenderWorks GetTenderWorkEntity(long _TenderWorkID)
        {
            return db.Repository<TM_TenderWorks>().GetAll().Where(x => x.ID == _TenderWorkID).FirstOrDefault();
        }
        public string GetStructureTypeNameByID(long _StructureTypeID)
        {
            return db.Repository<CO_StructureType>().GetAll().Where(x => x.ID == _StructureTypeID).Select(x => x.Name).FirstOrDefault();
        }
    }
}
