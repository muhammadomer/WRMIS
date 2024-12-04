using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.Tenders
{
    public class TenderManagementRepository : Repository<TM_TenderNotice>
    {
        WRMIS_Entities _context;

        public TenderManagementRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<TM_TenderNotice>();
            _context = context;
        }

        #region "Tender Notice"

        public List<dynamic> GetDomains()
        {
            List<dynamic> Domains = (from D in context.CO_Domain
                                     where D.IsActive == true
                                     select new
                                     {
                                         ID = D.ID,
                                         Name = D.Name,

                                     }).OrderBy(x => x.Name).ToList<dynamic>();

            return Domains;
        }

        public List<dynamic> GetDivisionByDomainID(long _DomainID)
        {
            List<dynamic> Divisions = (from D in context.CO_Division
                                       where D.DomainID == _DomainID
                                       select new
                                       {
                                           ID = D.ID,
                                           Name = D.Name,

                                       }).OrderBy(x => x.Name).ToList<dynamic>();

            return Divisions;
        }

        public List<dynamic> GetPublishingSourceByTenderNoticeID(long _TenderNoticeID)
        {
            List<dynamic> Sources = (from D in context.TM_TenderPublishedIn
                                     where D.TenderNoticeID == _TenderNoticeID
                                     select new
                                     {
                                         ID = D.ID,
                                         AdvertisementSource = D.PublisingSource,
                                         AdvertisementDate = D.PublishedDate

                                     }).ToList<dynamic>();

            return Sources;
        }


        public bool DeletePublishingSourceforUpdation(long _TenderNoticeID)
        {
            bool IsDeleted = false;
            try
            {
                context.TM_TenderPublishedIn.RemoveRange(context.TM_TenderPublishedIn.Where(q => q.TenderNoticeID == _TenderNoticeID));
                context.SaveChanges();
                IsDeleted = true;
            }
            catch (Exception)
            {

                throw;
            }
            return IsDeleted;
        }

        public bool DeleteTenderWorksID(long _TenderNoticeID)
        {
            bool IsDeleted = false;
            try
            {
                context.TM_TenderPublishedIn.RemoveRange(context.TM_TenderPublishedIn.Where(q => q.TenderNoticeID == _TenderNoticeID));
                context.SaveChanges();
                IsDeleted = true;
            }
            catch (Exception)
            {

                throw;
            }
            return IsDeleted;
        }
        #endregion

        #region "Works"

        public dynamic GetTenderDataByID(long _TenderNoticeID)
        {
            dynamic TenderData = (from Tender in context.TM_TenderNotice
                                  join domain in context.CO_Domain on Tender.DomainID equals domain.ID
                                  join Division in context.CO_Division on Tender.DivisionID equals Division.ID
                                  where Tender.ID == _TenderNoticeID
                                  select new
                                  {
                                      Name = Tender.Name,
                                      Domain = domain.Name,
                                      Division = Division.Name,
                                      DivisionID = Tender.DivisionID

                                  }).FirstOrDefault();

            return TenderData;
        }

        public List<dynamic> GetTenderWorksByTenderNoticeID(long _TenderNoticeID)
        {
            List<dynamic> WorksData = (from TenderWorks in context.TM_TenderWorks
                                       join TenderNotice in context.TM_TenderNotice on TenderWorks.TenderNoticeID equals TenderNotice.ID

                                       join ClosureWorks in context.CW_ClosureWork on TenderWorks.WorkSourceID equals ClosureWorks.ID into tW1
                                       from tw2 in tW1.DefaultIfEmpty()
                                       join WorkType in context.CW_WorkType on tw2.WorkTypeID equals WorkType.ID into wt
                                       from wt1 in wt.DefaultIfEmpty()

                                       join WorkStatus in context.TM_WorkStatus on TenderWorks.WorkStatusID equals WorkStatus.ID

                                       //asset   
                                       join Assest in context.AM_AssetWork on TenderWorks.WorkSourceID equals Assest.ID into Ass
                                       from Ass1 in Ass.DefaultIfEmpty()
                                       join type in context.AM_AssetWorkType on Ass1.AssetWorkTypeID equals type.ID into typ
                                       from typ1 in typ.DefaultIfEmpty()

                                       where TenderWorks.TenderNoticeID == _TenderNoticeID
                                       select new
                                       {
                                           ID = TenderWorks.ID,
                                           TenderWorkID = TenderWorks.ID,
                                           WorkSourceID = TenderWorks.WorkSourceID,
                                           WorkSource = TenderWorks.WorkSource,
                                           TenderNoticeID = TenderNotice.ID,
                                           WorkName = tw2.WorkName != null ? tw2.WorkName : Ass1.WorkName,



                                           WorkType = wt1.Name != null ? wt1.Name : typ1.Name,
                                           EstimatedCost = tw2.EstimatedCost != null ? tw2.EstimatedCost : Ass1.EstimatedCost,
                                           WorkStatus = WorkStatus.Name,
                                           WorkStatusID = WorkStatus.ID

                                       }).OrderByDescending(x => x.TenderWorkID).ToList<dynamic>();

            return WorksData;
        }

        public List<dynamic> GetClosureWorks(long _DivisionID, string _Year, long _WorkType, long _TenderNoticeID, List<long> _WorkSourceToExclude, string _AssestorClosure)
        {
            List<dynamic> ClosureWorks = new List<dynamic>();
            if (_AssestorClosure == "1")
            {
                ClosureWorks = (from CWorks in context.CW_ClosureWork
                                join CPlan in context.CW_ClosureWorkPlan on CWorks.CWPID equals CPlan.ID
                                join CType in context.CW_WorkType on CWorks.WorkTypeID equals CType.ID
                                join TenderWorks in context.TM_TenderWorks on CWorks.ID equals TenderWorks.WorkSourceID into tW1
                                from tw2 in tW1.DefaultIfEmpty()
                                where (CPlan.Status.ToUpper() == "PUBLISH")
                                && (tw2 == null || tw2.TenderNoticeID != _TenderNoticeID)
                                && (_DivisionID == -1 || CPlan.DivisionID == _DivisionID)
                                && (_Year == "" || CPlan.Year == _Year)
                                && (_WorkType == -1 || CWorks.WorkTypeID == _WorkType)
                                select new
                                {
                                    ClosureWorkID = CWorks.ID,
                                    ClosureWorkName = CWorks.WorkName,
                                    ClosureWorkType = CType.Name,
                                    EstimatedCost = CWorks.EstimatedCost
                                }).Distinct().ToList<dynamic>();


            }

            else
            {
                ClosureWorks = (from AWorks in context.AM_AssetWork
                                join CType in context.AM_AssetWorkType on AWorks.AssetWorkTypeID equals CType.ID
                                join TenderWorks in context.TM_TenderWorks on AWorks.ID equals TenderWorks.WorkSourceID into tW1
                                from tw2 in tW1.DefaultIfEmpty()
                                where (AWorks.WorkStatus.ToUpper() == "Published")
                                && (tw2 == null || tw2.TenderNoticeID != _TenderNoticeID)
                                && (_DivisionID == -1 || AWorks.DivisionID == _DivisionID)
                                && (_Year == "" || AWorks.FinancialYear == _Year)
                                && (_WorkType == -1 || AWorks.AssetWorkTypeID == _WorkType)
                                select new
                                {
                                    ClosureWorkID = AWorks.ID,
                                    ClosureWorkName = AWorks.WorkName,
                                    ClosureWorkType = CType.Name,
                                    EstimatedCost = AWorks.EstimatedCost
                                }).Distinct().ToList<dynamic>();
            }

            foreach (var item in _WorkSourceToExclude)
            {
                ClosureWorks = ClosureWorks.Where(x => x.ClosureWorkID != item).ToList<dynamic>();
            }
            return ClosureWorks;
        }

        public bool DeleteWorkByTenderWorkID(long _TenderWorkID)
        {
            bool DelResult = false;

            TM_TenderWorks TWObj = (from TW in context.TM_TenderWorks
                                    where TW.ID == _TenderWorkID
                                    select TW).FirstOrDefault();
            if (TWObj != null)
            {
                context.TM_TenderWorks.Remove(TWObj);
                context.SaveChanges();
                DelResult = true;
            }

            return DelResult;
        }

        public List<dynamic> GetClosureWorkType()
        {
            List<dynamic> ClosureWorkType = (from Work in context.CW_WorkType
                                             select new
                               {
                                   Name = Work.Name,
                                   ID = Work.ID,
                               }).ToList<dynamic>();

            return ClosureWorkType;
        }

        public List<dynamic> GetYearByDivisionID(long _DivisionID)
        {
            List<dynamic> Years = (from WP in context.CW_ClosureWorkPlan
                                   where WP.DivisionID == _DivisionID
                                   select new
                                   {
                                       Name = WP.Year,
                                       ID = WP.ID

                                   }).ToList<dynamic>();

            return Years;
        }

        public dynamic GetClosureWorkDataByID(long _WorkSourceID, long _TenderWorkID)
        {
            dynamic WorkData = (from Work in context.TM_TenderWorks
                                join Notice in context.TM_TenderNotice on Work.TenderNoticeID equals Notice.ID

                                join CWork in context.CW_ClosureWork on Work.WorkSourceID equals CWork.ID into cw
                                from cw1 in cw.DefaultIfEmpty()
                                join WT in context.CW_WorkType on cw1.WorkTypeID equals WT.ID into wtype
                                from wtype1 in wtype.DefaultIfEmpty()

                                join AssetWork in context.AM_AssetWork on Work.WorkSourceID equals AssetWork.ID into aw
                                from aw1 in aw.DefaultIfEmpty()

                                join AssetType in context.AM_AssetWorkType on aw1.AssetWorkTypeID equals AssetType.ID into at
                                from at1 in at.DefaultIfEmpty()


                                where Work.WorkSourceID == _WorkSourceID && Work.ID == _TenderWorkID
                                select new
                                {
                                    TenderNotice = Notice.Name,
                                    WorkName = cw1.WorkName != null ? cw1.WorkName : aw1.WorkName,
                                    WorkType = wtype1.Name != null ? wtype1.Name : at1.Name,
                                    DivisionID = Notice.DivisionID,
                                    WorkStatusID = Work.WorkStatusID,
                                    SubmissionDate = Notice.BidSubmissionDate
                                }).FirstOrDefault();

            return WorkData;
        }

        public List<dynamic> GetClosureWorkItemsByWorkID(long _WorkSourceID, string _WorkSource)
        {
            List<dynamic> WorkItems = new List<dynamic>();
            if (_WorkSource == "CLOSURE")
            {
                WorkItems = (from Cworks in context.CW_ClosureWork
                             join WItems in context.CW_WorkItem on Cworks.ID equals WItems.WorkID
                             join WUnit in context.CW_TechnicalSanctionUnit on WItems.TSUnitID equals WUnit.ID
                             where Cworks.ID == _WorkSourceID
                             select new
                             {
                                 WorkItemID = WItems.ID,
                                 ItemDescription = WItems.ItemDescription,
                                 SanctionedQuantity = WItems.SanctionedQty,
                                 Unit = WUnit.Name,
                                 TechnicalSanctionedRate = WItems.TSRate,
                                 TechnicalSanctionedAmount = WItems.SanctionedQty * WItems.TSRate

                             }).ToList<dynamic>();
            }
            else
            {
                WorkItems = (from Asset in context.AM_AssetWork
                             join WItems in context.CW_WorkItem on Asset.ID equals WItems.WorkID
                             join WUnit in context.CW_TechnicalSanctionUnit on WItems.TSUnitID equals WUnit.ID
                             where Asset.ID == _WorkSourceID
                             select new
                             {
                                 WorkItemID = WItems.ID,
                                 ItemDescription = WItems.ItemDescription,
                                 SanctionedQuantity = WItems.SanctionedQty,
                                 Unit = WUnit.Name,
                                 TechnicalSanctionedRate = WItems.TSRate,
                                 TechnicalSanctionedAmount = WItems.SanctionedQty * WItems.TSRate

                             }).ToList<dynamic>();
            }
            return WorkItems;
        }

        public dynamic GetWorktypeIDandSourceByWorkID(long _WorkSourceID)
        {
            dynamic WorkData = (from TN in context.TM_TenderNotice
                                join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID

                                join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID into cwork
                                from cwork1 in cwork.DefaultIfEmpty()

                                join Asset in context.AM_AssetWork on TW.WorkSourceID equals Asset.ID into AssetWork
                                from AssetWork1 in AssetWork.DefaultIfEmpty()

                                where TW.WorkSourceID == _WorkSourceID
                                select new
                                {
                                    WorkTypeID = cwork1.WorkTypeID != null ? cwork1.WorkTypeID : AssetWork1.AssetWorkTypeID,
                                    Source = TW.WorkSource,
                                    Office = TW.OpeningOffice,
                                    OfficeID = TW.OpeningOfficeID

                                }).FirstOrDefault();

            return WorkData;
        }

        public dynamic GetElecMechInfoByWorkID(long _ClosureWorkID)
        {
            dynamic ElecMechDivData = (from TN in context.TM_TenderNotice
                                       join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID
                                       join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                                       join div in context.CO_Division on TN.DivisionID equals div.ID
                                       into ps
                                       from p in ps.DefaultIfEmpty()
                                       join dom in context.CO_Domain on TN.DomainID equals dom.ID
                                       into qs
                                       from q in qs.DefaultIfEmpty()
                                       join wt in context.CW_WorkType on CW.WorkTypeID equals wt.ID
                                       join chan in context.CO_Channel on CW.EM_ChannelID equals chan.ID
                                       into cs
                                       from c in cs.DefaultIfEmpty()
                                       join fs in context.TM_FundingSource on CW.FundingSourceID equals fs.ID
                                       into fs
                                       from f in fs.DefaultIfEmpty()
                                       where TW.WorkSourceID == _ClosureWorkID
                                       select new
                                       {
                                           TenderNotice = TN.Name,
                                           WorkType = wt.Name,
                                           ChannelName = c.NAME,
                                           DomainName = q.Name,
                                           DivisionName = p.Name,
                                           FundingSource = f.FundingSource,
                                           EstimatedCost = CW.EstimatedCost,
                                           CompletionPeriod = CW.CompletionPeriod,
                                           CompletionPeriodUnit = CW.CompletionPeriodUnit,
                                           CompletionPeriodFlag = CW.CompletionPeriodFlag,
                                           StartDate = CW.StartDate,
                                           EndDate = CW.EndDate,
                                           SanctionNo = CW.SanctionNo,
                                           SanctionDate = CW.SanctionDate,
                                           EarnestMoney = CW.EarnestMoney,
                                           TenderFees = CW.TenderFees,
                                           Description = CW.Description,
                                           HeadWork = CW.EM_HBChannel,
                                           WorkName = CW.WorkName

                                       }).FirstOrDefault();

            return ElecMechDivData;
        }

        public dynamic GetDesiltingByWorkID(long _ClosureWorkID)
        {
            dynamic DesiltingDivData = (from TN in context.TM_TenderNotice
                                        join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID
                                        join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                                        join div in context.CO_Division on TN.DivisionID equals div.ID
                                        into ps
                                        from p in ps.DefaultIfEmpty()
                                        join dom in context.CO_Domain on TN.DomainID equals dom.ID
                                        into qs
                                        from q in qs.DefaultIfEmpty()
                                        join wt in context.CW_WorkType on CW.WorkTypeID equals wt.ID
                                        join chan in context.CO_Channel on CW.DS_ChannelID equals chan.ID
                                        into cs
                                        from c in cs.DefaultIfEmpty()
                                        join fs in context.TM_FundingSource on CW.FundingSourceID equals fs.ID
                                        into fs
                                        from f in fs.DefaultIfEmpty()
                                        join dist in context.CO_District on CW.DS_DistrictID equals dist.ID
                                        into dis
                                        from di in dis.DefaultIfEmpty()
                                        join teh in context.CO_Tehsil on CW.DS_TehsilID equals teh.ID
                                        into tehsil
                                        from tehs in tehsil.DefaultIfEmpty()
                                        where TW.WorkSourceID == _ClosureWorkID
                                        select new
                                        {
                                            TenderNotice = TN.Name,
                                            WorkType = wt.Name,
                                            ChannelName = c.NAME,
                                            FromRD = CW.DS_FromRD,
                                            ToRD = CW.DS_ToRD,
                                            DomainName = q.Name,
                                            SiltRemoved = CW.DS_SiltRemoved,
                                            DistrictName = di.Name,
                                            TehsilName = tehs.Name,
                                            DivisionName = p.Name,
                                            FundingSource = f.FundingSource,
                                            EstimatedCost = CW.EstimatedCost,
                                            CompletionPeriod = CW.CompletionPeriod,
                                            CompletionPeriodUnit = CW.CompletionPeriodUnit,
                                            CompletionPeriodFlag = CW.CompletionPeriodFlag,
                                            StartDate = CW.StartDate,
                                            EndDate = CW.EndDate,
                                            SanctionNo = CW.SanctionNo,
                                            SanctionDate = CW.SanctionDate,
                                            EarnestMoney = CW.EarnestMoney,
                                            TenderFees = CW.TenderFees,
                                            Description = CW.Description,
                                            HeadWork = CW.EM_HBChannel,
                                            WorkName = CW.WorkName

                                        }).FirstOrDefault();

            return DesiltingDivData;
        }


        public dynamic GetBuildingWorksInfoByWorkID(long _ClosureWorkID)
        {
            dynamic BuildingWorksDivData = (from TN in context.TM_TenderNotice
                                            join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID
                                            join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                                            join div in context.CO_Division on TN.DivisionID equals div.ID
                                            into ps
                                            from p in ps.DefaultIfEmpty()
                                            join dom in context.CO_Domain on TN.DomainID equals dom.ID
                                            into qs
                                            from q in qs.DefaultIfEmpty()
                                            join wt in context.CW_WorkType on CW.WorkTypeID equals wt.ID
                                            join fs in context.TM_FundingSource on CW.FundingSourceID equals fs.ID
                                            into fs
                                            from f in fs.DefaultIfEmpty()
                                            where TW.WorkSourceID == _ClosureWorkID
                                            select new
                                            {
                                                TenderNotice = TN.Name,
                                                WorkType = wt.Name,
                                                DomainName = q.Name,
                                                DivisionName = p.Name,
                                                FundingSource = f.FundingSource,
                                                EstimatedCost = CW.EstimatedCost,
                                                CompletionPeriod = CW.CompletionPeriod,
                                                CompletionPeriodUnit = CW.CompletionPeriodUnit,
                                                CompletionPeriodFlag = CW.CompletionPeriodFlag,
                                                StartDate = CW.StartDate,
                                                EndDate = CW.EndDate,
                                                SanctionNo = CW.SanctionNo,
                                                SanctionDate = CW.SanctionDate,
                                                EarnestMoney = CW.EarnestMoney,
                                                TenderFees = CW.TenderFees,
                                                Description = CW.Description,
                                                HeadWork = CW.EM_HBChannel,
                                                WorkName = CW.WorkName,
                                                BWOffice = CW.BW_Office,
                                                BWResidence = CW.BW_Residence,
                                                BWRestHouse = CW.BW_RestHouse,
                                                BWGRHut = CW.BW_GRHut,
                                                BWOthers = CW.BW_Others


                                            }).FirstOrDefault();

            return BuildingWorksDivData;
        }

        public dynamic GetOilGrePaintingInfoByWorkID(long _ClosureWorkID)
        {
            dynamic OilGrePaintingDivData = (from TN in context.TM_TenderNotice
                                             join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID
                                             join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                                             join div in context.CO_Division on TN.DivisionID equals div.ID
                                             into ps
                                             from p in ps.DefaultIfEmpty()
                                             join dom in context.CO_Domain on TN.DomainID equals dom.ID
                                             into qs
                                             from q in qs.DefaultIfEmpty()
                                             join wt in context.CW_WorkType on CW.WorkTypeID equals wt.ID
                                             join fs in context.TM_FundingSource on CW.FundingSourceID equals fs.ID
                                             into fs
                                             from f in fs.DefaultIfEmpty()

                                             join subdiv in context.CO_SubDivision on CW.OP_SubDivID equals subdiv.ID
                                             into sub
                                             from su in sub.DefaultIfEmpty()
                                             join sec in context.CO_Section on CW.OP_SectionID equals sec.ID
                                             into sect
                                             from se in sect.DefaultIfEmpty()
                                             where TW.WorkSourceID == _ClosureWorkID
                                             select new
                                             {
                                                 TenderNotice = TN.Name,
                                                 WorkType = wt.Name,
                                                 DomainName = q.Name,
                                                 DivisionName = p.Name,
                                                 FundingSource = f.FundingSource,
                                                 EstimatedCost = CW.EstimatedCost,
                                                 CompletionPeriod = CW.CompletionPeriod,
                                                 CompletionPeriodUnit = CW.CompletionPeriodUnit,
                                                 CompletionPeriodFlag = CW.CompletionPeriodFlag,
                                                 StartDate = CW.StartDate,
                                                 EndDate = CW.EndDate,
                                                 SanctionNo = CW.SanctionNo,
                                                 SanctionDate = CW.SanctionDate,
                                                 EarnestMoney = CW.EarnestMoney,
                                                 TenderFees = CW.TenderFees,
                                                 Description = CW.Description,
                                                 HeadWork = CW.EM_HBChannel,
                                                 WorkName = CW.WorkName,
                                                 GaugePainting = CW.OP_GaugePainting,
                                                 GaugeFixing = CW.OP_GaugeFixing,
                                                 OilGreasePaint = CW.OP_OilGreasePaint,
                                                 OPothers = CW.OP_Others,
                                                 SubDivisionName = su.Name,
                                                 SectionName = se.Name
                                             }).FirstOrDefault();

            return OilGrePaintingDivData;
        }



        public dynamic GetOutletRepairingInfoByWorkID(long _ClosureWorkID)
        {
            dynamic OutletRepairingDivData = (from TN in context.TM_TenderNotice
                                              join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID
                                              join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                                              join div in context.CO_Division on TN.DivisionID equals div.ID
                                              into ps
                                              from p in ps.DefaultIfEmpty()
                                              join dom in context.CO_Domain on TN.DomainID equals dom.ID
                                              into qs
                                              from q in qs.DefaultIfEmpty()
                                              join wt in context.CW_WorkType on CW.WorkTypeID equals wt.ID
                                              join fs in context.TM_FundingSource on CW.FundingSourceID equals fs.ID
                                              into fs
                                              from f in fs.DefaultIfEmpty()

                                              join subdiv in context.CO_SubDivision on CW.OR_SubDivID equals subdiv.ID
                                              into sub
                                              from su in sub.DefaultIfEmpty()
                                              join Chan in context.CO_Channel on CW.OR_ChannelID equals Chan.ID
                                             into sect
                                              from se in sect.DefaultIfEmpty()

                                              join section in context.CO_Section on CW.OR_SectionID equals section.ID
                                              into sec
                                              from sections in sec.DefaultIfEmpty()

                                              where TW.WorkSourceID == _ClosureWorkID
                                              select new
                                              {
                                                  TenderNotice = TN.Name,
                                                  WorkType = wt.Name,
                                                  DomainName = q.Name,
                                                  DivisionName = p.Name,
                                                  FundingSource = f.FundingSource,
                                                  EstimatedCost = CW.EstimatedCost,
                                                  CompletionPeriod = CW.CompletionPeriod,
                                                  CompletionPeriodUnit = CW.CompletionPeriodUnit,
                                                  CompletionPeriodFlag = CW.CompletionPeriodFlag,
                                                  StartDate = CW.StartDate,
                                                  EndDate = CW.EndDate,
                                                  SanctionNo = CW.SanctionNo,
                                                  SanctionDate = CW.SanctionDate,
                                                  EarnestMoney = CW.EarnestMoney,
                                                  TenderFees = CW.TenderFees,
                                                  Description = CW.Description,
                                                  WorkName = CW.WorkName,
                                                  ChannelName = se.NAME,
                                                  SubDivisionName = su.Name,
                                                  SectionName = sections.Name
                                              }).FirstOrDefault();

            return OutletRepairingDivData;
        }


        public dynamic GetChannelStructWorkInfoByWorkID(long _ClosureWorkID)
        {
            dynamic ChannelStructWorkDivData = (from TN in context.TM_TenderNotice
                                                join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID
                                                join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                                                join div in context.CO_Division on TN.DivisionID equals div.ID
                                                into ps
                                                from p in ps.DefaultIfEmpty()
                                                join dom in context.CO_Domain on TN.DomainID equals dom.ID
                                                into qs
                                                from q in qs.DefaultIfEmpty()
                                                join wt in context.CW_WorkType on CW.WorkTypeID equals wt.ID
                                                join fs in context.TM_FundingSource on CW.FundingSourceID equals fs.ID
                                                into fs
                                                from f in fs.DefaultIfEmpty()
                                                join Chan in context.CO_Channel on CW.OR_ChannelID equals Chan.ID
                                               into sect
                                                from se in sect.DefaultIfEmpty()
                                                where TW.WorkSourceID == _ClosureWorkID
                                                select new
                                                {
                                                    TenderNotice = TN.Name,
                                                    WorkType = wt.Name,
                                                    DomainName = q.Name,
                                                    DivisionName = p.Name,
                                                    FundingSource = f.FundingSource,
                                                    EstimatedCost = CW.EstimatedCost,
                                                    CompletionPeriod = CW.CompletionPeriod,
                                                    CompletionPeriodUnit = CW.CompletionPeriodUnit,
                                                    CompletionPeriodFlag = CW.CompletionPeriodFlag,
                                                    StartDate = CW.StartDate,
                                                    EndDate = CW.EndDate,
                                                    SanctionNo = CW.SanctionNo,
                                                    SanctionDate = CW.SanctionDate,
                                                    EarnestMoneyType = CW.EarnestMoneyType,
                                                    EarnestMoney = CW.EarnestMoney,
                                                    TenderFees = CW.TenderFees,
                                                    Description = CW.Description,
                                                    WorkName = CW.WorkName,
                                                    ChannelName = se.NAME
                                                }).FirstOrDefault();

            return ChannelStructWorkDivData;
        }

        public dynamic GetOtherWorkInfoByWorkID(long _ClosureWorkID)
        {
            dynamic OtherWorkDivData = (from TN in context.TM_TenderNotice
                                        join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID
                                        join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                                        join div in context.CO_Division on TN.DivisionID equals div.ID
                                        into ps
                                        from p in ps.DefaultIfEmpty()
                                        join dom in context.CO_Domain on TN.DomainID equals dom.ID
                                        into qs
                                        from q in qs.DefaultIfEmpty()
                                        join wt in context.CW_WorkType on CW.WorkTypeID equals wt.ID
                                        join fs in context.TM_FundingSource on CW.FundingSourceID equals fs.ID
                                        into fs
                                        from f in fs.DefaultIfEmpty()

                                        join subdiv in context.CO_SubDivision on CW.OW_SubDivID equals subdiv.ID
                                        into sub
                                        from su in sub.DefaultIfEmpty()

                                        join section in context.CO_Section on CW.OW_SectionID equals section.ID
                                        into sec
                                        from sections in sec.DefaultIfEmpty()

                                        where TW.WorkSourceID == _ClosureWorkID
                                        select new
                                        {
                                            TenderNotice = TN.Name,
                                            WorkType = wt.Name,
                                            DomainName = q.Name,
                                            DivisionName = p.Name,
                                            FundingSource = f.FundingSource,
                                            EstimatedCost = CW.EstimatedCost,
                                            CompletionPeriod = CW.CompletionPeriod,
                                            CompletionPeriodUnit = CW.CompletionPeriodUnit,
                                            CompletionPeriodFlag = CW.CompletionPeriodFlag,
                                            StartDate = CW.StartDate,
                                            EndDate = CW.EndDate,
                                            SanctionNo = CW.SanctionNo,
                                            SanctionDate = CW.SanctionDate,
                                            EarnestMoney = CW.EarnestMoney,
                                            TenderFees = CW.TenderFees,
                                            Description = CW.Description,
                                            WorkName = CW.WorkName,
                                            SubDivisionName = su.Name,
                                            SectionName = sections.Name
                                        }).FirstOrDefault();

            return OtherWorkDivData;
        }

        public bool UpdateOpeningOfficeByWorkSourceID(long _WorkSourceID, string _Office, long _OfficeId)
        {
            TM_TenderWorks c = (from x in context.TM_TenderWorks
                                where x.WorkSourceID == _WorkSourceID
                                select x).First();
            c.OpeningOfficeID = _OfficeId;
            c.OpeningOffice = _Office;
            context.SaveChanges();
            return true;
        }
        public bool UpdateOpeningOfficeByTenderID(long _ID, string _Office, long _OfficeId)
        {
            TM_TenderWorks c = (from x in context.TM_TenderWorks
                                where x.ID == _ID
                                select x).First();
            c.OpeningOfficeID = _OfficeId;
            c.OpeningOffice = _Office;
            context.SaveChanges();
            return true;
        }

        public dynamic GetOfficeLocationByZoneID(long _OfficeLocationID, long _WorkSourceID)
        {
            dynamic ZoneData = (from tw in context.TM_TenderWorks
                                join zone in context.CO_Zone on tw.OpeningOfficeID equals zone.ID
                                where tw.WorkSourceID == _WorkSourceID
                                select new
                                {
                                    ZoneOfficeName = zone.Name
                                }).FirstOrDefault();

            return ZoneData;

        }

        public dynamic GetOfficeLocationByCircleID(long _OfficeLocationID, long _WorkSourceID)
        {
            dynamic CircleData = (from tw in context.TM_TenderWorks
                                  join cir in context.CO_Circle on tw.OpeningOfficeID equals cir.ID
                                  where tw.WorkSourceID == _WorkSourceID
                                  select new
                                  {
                                      CircleOfficeName = cir.Name
                                  }).FirstOrDefault();

            return CircleData;

        }

        public dynamic GetOfficeLocationByDivisionID(long _OfficeLocationID, long _WorkSourceID)
        {
            dynamic DivisionData = (from tw in context.TM_TenderWorks
                                    join div in context.CO_Division on tw.OpeningOfficeID equals div.ID
                                    where tw.WorkSourceID == _WorkSourceID
                                    select new
                                    {
                                        DivisionOfficeName = div.Name
                                    }).FirstOrDefault();

            return DivisionData;

        }


        public dynamic GetOfficeLocationByOtherID(long _OfficeLocationID, long _WorkSourceID)
        {
            dynamic OtherData = (from tw in context.TM_TenderWorks
                                 join too in context.TM_TenderOpeningOffice on tw.OpeningOfficeID equals too.ID
                                 where tw.WorkSourceID == _WorkSourceID
                                 select new
                                 {
                                     OtherOfficeName = too.Name
                                 }).FirstOrDefault();

            return OtherData;

        }

        public List<dynamic> GetSoldTenderListByWorkID(long _WorkID)
        {
            List<dynamic> SoldTenderList = (from SList in context.TM_TenderWorksContractors
                                            join C in context.TM_Contractors on SList.ContractorsID equals C.ID
                                            where SList.TenderWorksID == _WorkID
                                            select new
                                            {
                                                ID = SList.ID,
                                                CompanyName = C.CompanyName,
                                                BankReceipt = SList.BankReceipt
                                            }).ToList<dynamic>();

            return SoldTenderList;
        }

        //public List<dynamic> GetContractorsList()
        //{


        //    List<dynamic> Contractors = (from C in context.TM_Contractors
        //                                 select new
        //                                 {
        //                                     id = C.ID,
        //                                     name = C.CompanyName
        //                                 }).ToList<dynamic>();

        //    return Contractors;
        //}

        public List<dynamic> GetContractorsList(string _Name)
        {


            if (_Name != String.Empty)
            {
                List<TM_Contractors> lstContractors = null;

                lstContractors = (from Contractors in context.TM_Contractors
                                  select Contractors).ToList<TM_Contractors>();

                lstContractors = lstContractors.Where(Con => Con.CompanyName.ToUpper().Contains(_Name.ToUpper()) || _Name == "*").ToList();
                List<dynamic> lstContractors2 = new List<dynamic>();

                foreach (TM_Contractors mdlContractors in lstContractors)
                {

                    lstContractors2.Add(new
                    {
                        ContractorID = mdlContractors.ID,
                        CompanyName = mdlContractors.CompanyName

                    });

                }
                return lstContractors2;
            }
            else
            {
                return new List<dynamic>();
            }

            //List<dynamic> Contractors = (from C in context.TM_Contractors
            //                             select new
            //                             {
            //                                 id = C.ID,
            //                                 name = C.CompanyName
            //                             }).ToList<dynamic>();

            //return Contractors;
        }

        public dynamic GetTenderWorkContractor(long _TenderWorkContractorID)
        {
            dynamic mdlTenderWorkContractor = (from SList in context.TM_TenderWorksContractors
                                               join C in context.TM_Contractors on SList.ContractorsID equals C.ID
                                               where SList.ID == _TenderWorkContractorID
                                               select new
                                               {
                                                   ID = SList.ID,
                                                   CompanyName = C.CompanyName,
                                                   CompanyID = C.ID,
                                                   BankReceipt = SList.BankReceipt
                                               }).FirstOrDefault();

            return mdlTenderWorkContractor;

        }

        public List<object> GetEvalCommitteeMember(long _WorkID)
        {
            List<object> lstOfCommitteeMembers = (from val in context.TM_CommitteeMembers
                                                  join ecm in context.TM_TenderCommitteeMembers on val.ID equals ecm.CommitteeMembersID
                                                  where ecm.TenderWorksID == _WorkID
                                                  select new
                                                 {
                                                     ID = val.ID,
                                                     MembersName = val.Name,
                                                     Designation = val.Designation,
                                                     Mobile = val.MobilePhone,
                                                     Email = val.Email
                                                 }).ToList<object>();

            return lstOfCommitteeMembers;
        }

        public List<dynamic> GetCommitteeMembersName(string _Name)
        {


            List<dynamic> lstMembers = (from CM in context.TM_CommitteeMembers
                                        .Where(Con => Con.Name.ToUpper().Contains(_Name.ToUpper()) || _Name == "*").ToList()
                                        select new
                                        {
                                            MemberID = CM.ID,
                                            MemberName = CM.Name,
                                            Designation = CM.Designation,
                                            Mobile = CM.MobilePhone,
                                            Email = CM.Email
                                        }).ToList<dynamic>();

            return lstMembers;
        }

        public List<dynamic> GetCommitteeMembersListByWorkID(long _WorkID)
        {
            List<dynamic> lstCommitteeMembers = (from tcm in context.TM_TenderCommitteeMembers
                                                 join cm in context.TM_CommitteeMembers on tcm.CommitteeMembersID equals cm.ID
                                                 where tcm.TenderWorksID == _WorkID
                                                 select new
                                                 {
                                                     ID = cm.ID,
                                                     MembersName = cm.Name,
                                                     Designation = cm.Designation,
                                                     Mobile = cm.MobilePhone,
                                                     Email = cm.Email

                                                 }).OrderBy(x => x.MembersName).ToList<dynamic>();

            return lstCommitteeMembers;
        }
        #endregion

        #region Tender Opening Process

        public List<dynamic> GetUsersByDivisionID(long _DivisionID, long _DesignationID)
        {
            List<dynamic> lstOfUsers = null;
            if (_DesignationID == (long)Constants.Designation.ADM)
            {
                lstOfUsers = (from al in context.UA_AssociatedLocation
                              join users in context.UA_Users on al.UserID equals users.ID
                              where al.IrrigationBoundryID == _DivisionID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division
                              && al.DesignationID == (long)Constants.Designation.ADM
                              select new
                              {
                                  ID = users.ID,
                                  Name = users.FirstName + " " + users.LastName,
                                  DivisionID = al.IrrigationBoundryID,
                                  Designation = "ADM"

                              }).ToList<dynamic>();

            }
            else
            {
                lstOfUsers = (from al in context.UA_AssociatedLocation
                              join users in context.UA_Users on al.UserID equals users.ID
                              where al.IrrigationBoundryID == _DivisionID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division
                              && al.DesignationID == (long)Constants.Designation.MA
                              select new
                              {
                                  ID = users.ID,
                                  Name = users.FirstName + " " + users.LastName,
                                  DivisionID = al.IrrigationBoundryID,
                                  Designation = "MA"
                              }).ToList<dynamic>();

            }

            return lstOfUsers;

        }

        public int UpdateTenderCommitteeMembers(string _CommitteeMembers)
        {
            //  WRMIS_Entities context = new WRMIS_Entities();
            // var param = new SqlParameter("@_CommitteeMembers", SqlDbType.Xml, -1);
            //   param.Value = _CommitteeMembers;
            //  param.TypeName = CommandType.StoredProcedure;
            // param.TypeName = "dbo.WTAttachments";
            // string sql = String.Format("EXEC {0} {1};", "dbo.WT_SaveAttachments", "@_CommitteeMembers");
            //return context.Database.ExecuteSqlCommand(sql, param);


            WRMIS_Entities context = new WRMIS_Entities();
            var param = new SqlParameter("@_CommitteeMembers", SqlDbType.Xml, -1);
            param.Value = _CommitteeMembers;
            // param.TypeName = "dbo.WTAttachments";
            string sql = String.Format("EXEC {0} {1};", "dbo.TM_SaveBulkDataOfCommitteeMembers", "@_CommitteeMembers");
            return context.Database.ExecuteSqlCommand(sql, param);
        }

        public List<object> GetWorkItemsDetailsByWorKID(long _WorkID, long _CompanyID, long _TenderNoticeID)
        {

            List<object> lstWorkItemDetails = (from wi in context.CW_WorkItem
                                               //join tw in context.TM_TenderWorks on wi.WorkID equals tw.WorkSourceID
                                               join tw in context.TM_TenderWorks on new
                                               {
                                                   ID = wi.WorkID,
                                                   NID = _TenderNoticeID
                                               } equals new { ID = tw.WorkSourceID, NID = tw.TenderNoticeID }
                                               join con in context.TM_TenderWorksContractors on tw.ID equals con.TenderWorksID
                                               join unit in context.CW_TechnicalSanctionUnit on wi.TSUnitID equals unit.ID
                                               //join tp in context.TM_TenderPrice on con.ID equals tp.TWContractorID
                                               join tp in context.TM_TenderPrice on new
                                               {
                                                   CID = con.ID,
                                                   WID = wi.ID
                                               } equals new { CID = tp.TWContractorID.Value, WID = tp.WorkItemID.Value }

                                               where tw.ID == _WorkID && con.ContractorsID == _CompanyID
                                               select new
                                               {
                                                   ID = wi.ID,
                                                   TPID = tp.ID,
                                                   ItemDescription = wi.ItemDescription,
                                                   SanctionedQty = wi.SanctionedQty,
                                                   TSUnitName = unit.Name,
                                                   TSRate = wi.TSRate,
                                                   ContractorID = con.ID,
                                                   ContractorRate = tp.ContractorRate,
                                                   TotalItemAmount = tp.ContractorRate * wi.SanctionedQty,
                                                   EstimateType = con.TP_EstimateType,
                                                   EstimatePercentage = con.TP_EstimatePercentage
                                               }).Distinct().ToList<object>();
            return lstWorkItemDetails;
        }

        public List<object> GetCompanyNameByWorkID(long _WorkID)
        {
            List<object> lstCompanyName = (from twc in context.TM_TenderWorksContractors
                                           join tc in context.TM_Contractors on twc.ContractorsID equals tc.ID
                                           where twc.TenderWorksID == _WorkID
                                           select new
                                           {
                                               ID = tc.ID,
                                               Name = tc.CompanyName
                                           }).ToList<object>();
            return lstCompanyName;
        }

        public List<dynamic> getContractorsListBytenderWorkID(long _TenderWorksID)
        {
            List<dynamic> lstContractors = (from WC in context.TM_TenderWorksContractors
                                            join C in context.TM_Contractors on WC.ContractorsID equals C.ID
                                            where WC.TenderWorksID == _TenderWorksID
                                            select new
                                            {
                                                AttendanceID = WC.ID,
                                                isAttended = WC.Attended,
                                                CompanyName = C.CompanyName,
                                                AlternateName = WC.AlternateName,
                                                AlternateRemarks = WC.AlternateRemarks
                                            }).ToList<dynamic>();

            return lstContractors;
        }
        public List<object> GetTenderAttendanceAttachment(long _TenderWorkID, string source)
        {
            List<object> Attendance_Attachment = (from TA in context.TM_TenderWorkAttachment
                                                  where TA.TenderWorksID == _TenderWorkID && TA.Source == source
                                                  select new
                                                  {
                                                      ID = TA.ID,
                                                      TenderWorksID = TA.TenderWorksID,
                                                      FileName = TA.Attachment,
                                                      FileType = TA.AttachmentType
                                                  })
                                                  .AsEnumerable().Select
                                                    (q => new
                                                    {
                                                        ID = q.ID,
                                                        TenderWorksID = q.TenderWorksID,
                                                        FileName = Utility.GetImageURL(Configuration.TenderManagement, q.FileName),
                                                        FileType = q.FileType
                                                    }).ToList<object>();
            return Attendance_Attachment;

        }
        public bool DeleteAttendanceAttachmentByTenderWorkID(long _TenderWorkID, string _source)
        {
            context.TM_TenderWorkAttachment.Where(p => p.TenderWorksID == _TenderWorkID && p.Source == _source)
               .ToList().ForEach(p => context.TM_TenderWorkAttachment.Remove(p));
            context.SaveChanges();
            return true;
        }

        public List<dynamic> GetADMReportDataByTenderWorkID(long _TenderWorkID)
        {
            TM_TenderWorks tw = GetWorkByTenderWorkID(_TenderWorkID);
            List<dynamic> ADMReportData = null;
            if (tw != null)
            {
                if (tw.WorkSource.ToUpper().Equals("CLOSURE"))
                {
                    ADMReportData = (from WCon in context.TM_TenderWorksContractors
                                     join COn in context.TM_Contractors on WCon.ContractorsID equals COn.ID
                                     join TW in context.TM_TenderWorks on WCon.TenderWorksID equals TW.ID
                                     join CWork in context.CW_ClosureWork on TW.WorkSourceID equals CWork.ID
                                     where WCon.TenderWorksID == _TenderWorkID && WCon.TenderPriced == true
                                     select new
                                     {
                                         WorkContractorID = WCon.ID,
                                         ContractorName = COn.CompanyName,
                                         Deposit = (context.TM_TenderPriceCDR.Where(tp => tp.TWContractorID == WCon.ID).Sum(tp => tp.Amount)),

                                         EarnestMoney = (CWork.EarnestMoneyType == "Lumpsum" ? CWork.EarnestMoney :
                                                         CWork.EarnestMoneyType == "% of Financial Bid" ? CWork.EstimatedCost * CWork.EarnestMoney / 100 :
                                                         CWork.EarnestMoney),
                                         //EarnestMoney = CWork.EarnestMoney,
                                         RejectedReason = WCon.RejectedReason,
                                         IsRejected = WCon.Rejected == null ? false : WCon.Rejected,
                                         IsContractorRejected = WCon.Rejected == null ? -1 : WCon.Rejected == true ? 1 : 0
                                     }).ToList<dynamic>();
                }
                else if (tw.WorkSource.ToUpper().Equals("ASSET"))
                {
                    ADMReportData = (from WCon in context.TM_TenderWorksContractors
                                     join COn in context.TM_Contractors on WCon.ContractorsID equals COn.ID
                                     join TW in context.TM_TenderWorks on WCon.TenderWorksID equals TW.ID
                                     join CWork in context.AM_AssetWork on TW.WorkSourceID equals CWork.ID
                                     where WCon.TenderWorksID == _TenderWorkID && WCon.TenderPriced == true
                                     select new
                                     {
                                         WorkContractorID = WCon.ID,
                                         ContractorName = COn.CompanyName,
                                         Deposit = (context.TM_TenderPriceCDR.Where(tp => tp.TWContractorID == WCon.ID).Sum(tp => tp.Amount)),

                                         EarnestMoney = (CWork.EarnestMoneyType == "Lumpsum" ? CWork.EarnestMoney :
                                                         CWork.EarnestMoneyType == "% of Financial Bid" ? CWork.EstimatedCost * CWork.EarnestMoney / 100 :
                                                         CWork.EarnestMoney),
                                         //EarnestMoney = CWork.EarnestMoney,
                                         RejectedReason = WCon.RejectedReason,
                                         IsRejected = WCon.Rejected == null ? false : WCon.Rejected,
                                         IsContractorRejected = WCon.Rejected == null ? -1 : WCon.Rejected == true ? 1 : 0
                                     }).ToList<dynamic>();

                }
            }

            //List<dynamic> ADMReportData = (from WCon in context.TM_TenderWorksContractors
            //                               join COn in context.TM_Contractors on WCon.ContractorsID equals COn.ID
            //                               join TW in context.TM_TenderWorks on WCon.TenderWorksID equals TW.ID
            //                               join CWork in context.CW_ClosureWork on TW.WorkSourceID equals CWork.ID
            //                               join TP in context.TM_TenderPriceCDR on WCon.ID equals TP.TWContractorID
            //                               where WCon.TenderWorksID == _TenderWorkID

            //                               group TP by new { WCon, CWork } into odg

            //                               select new
            //                               {
            //                                   ContractorName = odg.Key,
            //                                   Deposit = odg.Sum(x => x.Amount),




            //                               }).ToList<dynamic>();



            return ADMReportData;
        }

        public dynamic GetTenderInformationForADMReport(long _TenderWorkID)
        {
            TM_TenderWorks tw = GetWorkByTenderWorkID(_TenderWorkID);
            dynamic Info = null;
            if (tw != null)
            {
                if (tw.WorkSource.ToUpper().Equals("CLOSURE"))
                {
                    Info = (from TWork in context.TM_TenderWorks
                            join Notice in context.TM_TenderNotice on TWork.TenderNoticeID equals Notice.ID
                            join CWork in context.CW_ClosureWork on TWork.WorkSourceID equals CWork.ID
                            join WT in context.CW_WorkType on CWork.WorkTypeID equals WT.ID
                            where TWork.ID == _TenderWorkID
                            select new
                            {
                                TenderNotice = Notice.Name,
                                WorkName = CWork.WorkName,
                                WorkType = WT.Name,
                                SubmissionTime = Notice.BidSubmissionDate,
                                OpeningTime = Notice.BidOpeningDate,
                                ActualSubmissionDate = TWork.ADM_ActualSubmissionDate,
                                ActualOpeningDate = TWork.ADM_ActualOpeningDate,
                                ActualSubmissionReason = TWork.ADM_ActualSubmissionReason,
                                ActualOpeningReason = TWork.ADM_ActualOpeningReason,
                                StatusID = TWork.WorkStatusID,
                                StatusReason = TWork.StatusReason,
                                SoldTenderCount = (context.TM_TenderWorksContractors.Where(x => x.TenderWorksID == _TenderWorkID).Count(x => x.TenderWorksID != null)),
                                SubmittedTender = (context.TM_TenderWorksContractors.Where(x => x.TenderWorksID == _TenderWorkID).Count(x => x.TenderPriced == true))

                            }).ToList()
                                   .Select(u => new
                                   {
                                       TenderNotice = u.TenderNotice,
                                       WorkName = u.WorkName,
                                       WorkType = u.WorkType,
                                       ActualSubmissionDate = Utility.GetTimeFormatted(u.ActualSubmissionDate),
                                       ActualOpeningDate = Utility.GetTimeFormatted(u.ActualOpeningDate),
                                       ActualSubmissionReason = u.ActualSubmissionReason,
                                       ActualOpeningReason = u.ActualOpeningReason,
                                       WorkStatusID = u.StatusID,
                                       StatusReason = u.StatusReason,
                                       SubmissionDate = Utility.GetFormattedDate(u.SubmissionTime),
                                       SubmissionTime = Utility.GetFormattedTime(u.SubmissionTime),
                                       OpeningTime = Utility.GetFormattedTime(u.OpeningTime),
                                       OpeningDate = Utility.GetFormattedDate(u.OpeningTime),
                                       SoldTenderCount = u.SoldTenderCount,
                                       SubmittedTender = u.SubmittedTender,
                                       AttachmentList = GetADMAttachment(_TenderWorkID)

                                   }).FirstOrDefault();
                }
                else
                    if (tw.WorkSource.ToUpper().Equals("ASSET"))
                    {
                        Info = (from TWork in context.TM_TenderWorks
                                join Notice in context.TM_TenderNotice on TWork.TenderNoticeID equals Notice.ID
                                join CWork in context.AM_AssetWork on TWork.WorkSourceID equals CWork.ID
                                join WT in context.AM_AssetWorkType on CWork.AssetWorkTypeID equals WT.ID
                                where TWork.ID == _TenderWorkID
                                select new
                                {
                                    TenderNotice = Notice.Name,
                                    WorkName = CWork.WorkName,
                                    WorkType = WT.Name,
                                    SubmissionTime = Notice.BidSubmissionDate,
                                    OpeningTime = Notice.BidOpeningDate,
                                    ActualSubmissionDate = TWork.ADM_ActualSubmissionDate,
                                    ActualOpeningDate = TWork.ADM_ActualOpeningDate,
                                    ActualSubmissionReason = TWork.ADM_ActualSubmissionReason,
                                    ActualOpeningReason = TWork.ADM_ActualOpeningReason,
                                    StatusID = TWork.WorkStatusID,
                                    StatusReason = TWork.StatusReason,
                                    SoldTenderCount = (context.TM_TenderWorksContractors.Where(x => x.TenderWorksID == _TenderWorkID).Count(x => x.TenderWorksID != null)),
                                    SubmittedTender = (context.TM_TenderWorksContractors.Where(x => x.TenderWorksID == _TenderWorkID).Count(x => x.TenderPriced == true))

                                }).ToList()
                                    .Select(u => new
                                    {
                                        TenderNotice = u.TenderNotice,
                                        WorkName = u.WorkName,
                                        WorkType = u.WorkType,
                                        ActualSubmissionDate = Utility.GetTimeFormatted(u.ActualSubmissionDate),
                                        ActualOpeningDate = Utility.GetTimeFormatted(u.ActualOpeningDate),
                                        ActualSubmissionReason = u.ActualSubmissionReason,
                                        ActualOpeningReason = u.ActualOpeningReason,
                                        WorkStatusID = u.StatusID,
                                        StatusReason = u.StatusReason,
                                        SubmissionDate = Utility.GetFormattedDate(u.SubmissionTime),
                                        SubmissionTime = Utility.GetFormattedTime(u.SubmissionTime),
                                        OpeningTime = Utility.GetFormattedTime(u.OpeningTime),
                                        OpeningDate = Utility.GetFormattedDate(u.OpeningTime),
                                        SoldTenderCount = u.SoldTenderCount,
                                        SubmittedTender = u.SubmittedTender,
                                        AttachmentList = GetADMAttachment(_TenderWorkID)

                                    }).FirstOrDefault();
                    }

            }

            return Info;
        }


        public List<object> GetCallDepositDataByWorKID(long _WorkID, long _CompanyID)
        {
            List<object> lstWorkItemDetails = (from tw in context.TM_TenderWorks
                                               join twc in context.TM_TenderWorksContractors on tw.ID equals twc.TenderWorksID
                                               join tp in context.TM_TenderPriceCDR on twc.ID equals tp.TWContractorID
                                               where tw.ID == _WorkID && twc.ContractorsID == _CompanyID
                                               select new
                                               {
                                                   ID = tp.ID,
                                                   TenerWContractorID = twc.ID,
                                                   CDRNO = tp.CDRNo,
                                                   BankDetail = tp.BankDetail,
                                                   Amount = tp.Amount,
                                                   Attachment = tp.Attachment
                                               }).ToList<object>();
            return lstWorkItemDetails;
        }
        public List<object> GetContractorCallDeposit(long _TWContractorID)
        {
            List<object> callDepositList = (from TA in context.TM_TenderPriceCDR
                                            where TA.TWContractorID == _TWContractorID
                                            select new
                                            {
                                                ID = TA.ID,
                                                TWContractorID = TA.TWContractorID,
                                                CDRNo = TA.CDRNo,
                                                BankDetail = TA.BankDetail,
                                                Amount = TA.Amount,
                                                Attachment = TA.Attachment
                                            })
                                                .AsEnumerable().Select
                                                  (q => new
                                                  {
                                                      ID = q.ID,
                                                      TWContractorID = q.TWContractorID,
                                                      CDRNo = q.CDRNo,
                                                      BankDetail = q.BankDetail,
                                                      Amount = q.Amount,
                                                      FileName = q.Attachment,
                                                      FileURL = Utility.GetImageURL(Configuration.TenderManagement, q.Attachment)

                                                  }).ToList<object>();
            return callDepositList;
        }


        public List<object> GetWorkItemsforViewByWorKID(long _WorkID)
        {
            List<object> lstWorkItemDetails = (from tw in context.TM_TenderWorks
                                               join wi in context.CW_WorkItem on tw.WorkSourceID equals wi.WorkID
                                               // join con in context.TM_TenderWorksContractors on tw.ID equals con.TenderWorksID
                                               join unit in context.CW_TechnicalSanctionUnit on wi.TSUnitID equals unit.ID
                                               where tw.ID == _WorkID
                                               select new
                                               {
                                                   ID = wi.ID,
                                                   ItemDescription = wi.ItemDescription,
                                                   SanctionedQty = wi.SanctionedQty,
                                                   TSUnitName = unit.Name,
                                                   TSRate = wi.TSRate,
                                                   ContractorRate = "",
                                                   TPID = "",
                                                   // ContractorID = con.ID
                                                   TotalItemAmount = "",
                                                   EstimateType = "",
                                                   EstimatePercentage = ""
                                               }).Distinct().OrderBy(x => x.ItemDescription).ToList<object>();
            return lstWorkItemDetails;
        }


        public List<object> GetCallDepositDataforViewByWorKID(long _WorkID)
        {
            List<object> lstWorkItemDetails = (from tw in context.TM_TenderWorks
                                               join twc in context.TM_TenderWorksContractors on tw.ID equals twc.TenderWorksID
                                               join tp in context.TM_TenderPriceCDR on twc.ID equals tp.TWContractorID
                                               where tw.ID == _WorkID
                                               select new
                                               {
                                                   ID = tp.ID,
                                                   TenerWContractorID = twc.ID,
                                                   CDRNO = tp.CDRNo,
                                                   BankDetail = tp.BankDetail,
                                                   Amount = tp.Amount,
                                                   Attachment = tp.Attachment
                                               }).Distinct().ToList<object>();
            return lstWorkItemDetails;
        }
        public object GetTenderPriceCDRByID(long _ID)
        {
            object callDepositList = (from TA in context.TM_TenderPriceCDR
                                      where TA.ID == _ID
                                      select new
                                      {
                                          ID = TA.ID,
                                          TWContractorID = TA.TWContractorID,
                                          CDRNo = TA.CDRNo,
                                          BankDetail = TA.BankDetail,
                                          Amount = TA.Amount,
                                          Attachment = TA.Attachment
                                      })
                                                .AsEnumerable().Select
                                                  (q => new
                                                  {
                                                      ID = q.ID,
                                                      TWContractorID = q.TWContractorID,
                                                      CDRNo = q.CDRNo,
                                                      BankDetail = q.BankDetail,
                                                      Amount = q.Amount,
                                                      FileName = q.Attachment,
                                                      FileURL = Utility.GetImageURL(Configuration.TenderManagement, q.Attachment)

                                                  }).FirstOrDefault();
            return callDepositList;
        }


        public DataTable GetComparativeStatementDataByWorkandNoticeID(long _WorkID)
        {
            string TenderWorkID = Convert.ToString(_WorkID);
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteStoredProcedureDataTable("TM_GetComparativeStatementData", _WorkID);
        }
        public bool DeleteADMAttachmentByTenderWorkID(long _TenderWorkID)
        {
            //context.TM_ADMAttachment.Where(p => p.TenderWorksID == _TenderWorkID && (p.AttachmentType.ToUpper() == "IMAGE/JPEG" || p.AttachmentType.ToUpper() == "PNG" || p.AttachmentType.ToUpper() == "IMAGE/PNG"))
            //   .ToList().ForEach(p => context.TM_ADMAttachment.Remove(p));
            //context.SaveChanges();
            return true;
        }
        public List<object> GetADMAttachment(long _TenderWorkID)
        {
            List<object> Attendance_Attachment = (from TA in context.TM_ADMAttachment
                                                  where TA.TenderWorksID == _TenderWorkID
                                                  select new
                                                  {
                                                      ID = TA.ID,
                                                      TenderWorksID = TA.TenderWorksID,
                                                      FileName = TA.Attachment,
                                                      FileType = TA.AttachmentType
                                                  })
                                                  .AsEnumerable().Select
                                                    (q => new
                                                    {
                                                        ID = q.ID,
                                                        TenderWorksID = q.TenderWorksID,
                                                        FileName = Utility.GetImageURL(Configuration.TenderManagement, q.FileName),
                                                        FileType = q.FileType
                                                    }).ToList<object>();
            return Attendance_Attachment;

        }


        public List<object> GetContractorandAmountByWorkID(long _TenderWorkID)
        {
            List<object> lstContractorAmount = (from twc in context.TM_TenderWorksContractors
                                                join con in context.TM_Contractors on twc.ContractorsID equals con.ID
                                                where twc.TenderWorksID == _TenderWorkID && twc.Rejected != true && twc.TenderPriced == true
                                                select new
                                                {
                                                    ID = twc.ID,
                                                    CompanyName = con.CompanyName,
                                                    Amount = twc.TenderWorkAmount,
                                                    Checked = twc.Awarded,
                                                    Remarks = twc.AwardedRemarks
                                                }).AsEnumerable().Select
                                                    (q => new
                                                    {
                                                        ID = q.ID,
                                                        CompanyName = q.CompanyName,
                                                        Amount = q.Amount,
                                                        Checked = q.Checked == null ? false : q.Checked,
                                                        Remarks = q.Remarks
                                                    }).OrderBy(x => x.Amount).ToList<object>();

            return lstContractorAmount;

        }
        //,
        public object GetWorkItemRateBySourceID(long _WorkSourceID)
        {
            object WorkItemRate = (from tw in context.CW_WorkItem
                                   where tw.WorkID == _WorkSourceID
                                   select new
                                   {
                                       Rate = (context.CW_WorkItem.Where(tp => tp.WorkID == _WorkSourceID).Sum(tp => tp.TSRate * tp.SanctionedQty)),
                                   }).FirstOrDefault();
            return WorkItemRate;
        }


        public List<dynamic> GetMonitoringUsersByDivisionID(long _DivisionID)
        {
            List<dynamic> lstOfUsers = null;

            lstOfUsers = (from al in context.UA_AssociatedLocation
                          join users in context.UA_Users on al.UserID equals users.ID
                          where al.IrrigationBoundryID == _DivisionID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division
                            && (users.DesignationID == (long)Constants.Designation.ADM || users.DesignationID == (long)Constants.Designation.MA)
                          select new
                          {
                              ID = users.ID,
                              Name = users.FirstName + " " + users.LastName,
                              DivisionID = al.IrrigationBoundryID,
                              Designation = (users.DesignationID == (long)Constants.Designation.ADM) ? "ADM" : "MA"

                          }).ToList<dynamic>();




            return lstOfUsers;

        }

        public List<TM_TenderPriceCDR> DeleteCallDepositByTenderWorkContractorID(long _TendWContractorID)
        {
            List<TM_TenderPriceCDR> CDR = null;

            CDR = (from TW in context.TM_TenderPriceCDR
                   where TW.TWContractorID == _TendWContractorID
                   select TW).ToList<TM_TenderPriceCDR>();

            if (CDR != null)
            {
                context.TM_TenderPriceCDR.RemoveRange(CDR);
                context.SaveChanges();

            }
            return CDR;
        }
        //public List<CO_Domain> GetDomainsByUserID(long _UserID, long _IrrigationLevelID)
        //{
        //    List<CO_Domain> lstDomain = null;

        //    List<long?> lstLocationIDs = (from assloc in context.UA_AssociatedLocation
        //                                  where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
        //                                  select assloc.IrrigationBoundryID).ToList();

        //    if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
        //    {
        //        var query = from div in context.CO_Division
        //                    where lstLocationIDs.Contains(div.ID)
        //                    orderby div.Name
        //                    select div;
        //        lstDomain = (from dom in context.CO_Domain
        //                     join div in context.CO_Division on dom.ID equals div.DomainID
        //                     where lstLocationIDs.Contains(div.ID)
        //                     orderby dom.Name
        //                     select dom).Distinct().ToList();
        //    }
        //    else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
        //    {
        //        lstDomain = (from dom in context.CO_Domain
        //                     join div in context.CO_Division on dom.ID equals div.DomainID
        //                     join cir in context.CO_Circle on div.CircleID equals cir.ID
        //                     where lstLocationIDs.Contains(cir.ID)
        //                     orderby dom.Name
        //                     select dom).Distinct().ToList();
        //    }
        //    else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
        //    {
        //        lstDomain = (from dom in context.CO_Domain
        //                     join div in context.CO_Division on dom.ID equals div.DomainID
        //                     join cir in context.CO_Circle on div.CircleID equals cir.ID
        //                     join zon in context.CO_Zone on cir.ZoneID equals zon.ID
        //                     where lstLocationIDs.Contains(zon.ID)
        //                     orderby dom.Name
        //                     select dom).Distinct().ToList();
        //    }
        //    else if (_IrrigationLevelID == 0)
        //    {
        //        lstDomain = (from dom in context.CO_Domain
        //                     select dom).ToList();
        //    }
        //    return lstDomain;
        //}

        #endregion

        #region

        public List<dynamic> GetAssetTypes()
        {
            List<dynamic> AssetType = (from Work in context.AM_AssetWorkType
                                       select new
                                       {
                                           Name = Work.Name,
                                           ID = Work.ID,
                                       }).ToList<dynamic>();

            return AssetType;
        }

        public List<dynamic> GetAssetYearByDivisionID(long _DivisionID)
        {
            List<dynamic> Years = (from WP in context.AM_AssetWork
                                   where WP.DivisionID == _DivisionID
                                   select new
                                   {
                                       Name = WP.FinancialYear,
                                       ID = WP.ID

                                   }).GroupBy(c => c.Name.Trim()).ToList().Select(g => g.First()).ToList<dynamic>();

            return Years;
        }


        public dynamic GetAssetWorkInfoByWorkID(long _WorkSourceID, long _TenderNoticeID)
        {
            dynamic AssetWorkData = (from TN in context.TM_TenderNotice
                                     join TW in context.TM_TenderWorks on TN.ID equals TW.TenderNoticeID into tws
                                     from twss in tws.DefaultIfEmpty()
                                     join AW in context.AM_AssetWork on twss.WorkSourceID equals AW.ID into assetwork
                                     from assetworks in assetwork.DefaultIfEmpty()
                                     join div in context.CO_Division on TN.DivisionID equals div.ID
                                     into ps
                                     from p in ps.DefaultIfEmpty()
                                     join dom in context.CO_Domain on TN.DomainID equals dom.ID
                                     into qs
                                     from q in qs.DefaultIfEmpty()
                                     join workt in context.AM_AssetWorkType on assetworks.AssetWorkTypeID equals workt.ID into wtype
                                     from wtypes in wtype.DefaultIfEmpty()
                                     join fs in context.TM_FundingSource on assetworks.FundingSourceID equals fs.ID
                                     into fs
                                     from f in fs.DefaultIfEmpty()

                                     where twss.WorkSourceID == _WorkSourceID && TN.ID == _TenderNoticeID
                                     select new
                                     {
                                         TenderNotice = TN.Name,
                                         WorkType = wtypes.Name,
                                         DomainName = q.Name,
                                         DivisionName = p.Name,
                                         FundingSource = f.FundingSource,
                                         EstimatedCost = assetworks.EstimatedCost,
                                         CompletionPeriod = assetworks.CompletionPeriod,
                                         CompletionPeriodUnit = assetworks.CompletionPeriodUnit,
                                         StartDate = assetworks.StartDate,
                                         EndDate = assetworks.EndDate,
                                         SanctionNo = assetworks.SanctionNo,
                                         SanctionDate = assetworks.SanctionDate,
                                         EarnestMoney = assetworks.EarnestMoney,
                                         TenderFees = assetworks.TenderFees,
                                         Description = assetworks.Description,
                                         WorkName = assetworks.WorkName,
                                     }).FirstOrDefault();

            return AssetWorkData;
        }
        #endregion

        #region

        public List<dynamic> GetClouserWorksByTenderID(long _CWID)
        {
            List<dynamic> lstWorks;

            lstWorks = (from tn in context.TM_TenderNotice
                        join tw in context.TM_TenderWorks on tn.ID equals tw.TenderNoticeID
                        join cw in context.CW_ClosureWork on tw.WorkSourceID equals cw.ID
                        // join wi in context.CW_WorkItem on cw.ID equals wi.WorkID
                        where cw.ID == _CWID
                        select new
                        {
                            OpeningDate = tn.BidOpeningDate != null ? tn.BidOpeningDate : tw.ADM_ActualOpeningDate,
                            WorkName = cw.WorkName,
                            OpeningOffice = tw.OpeningOffice,
                            WorkID = cw.ID

                        }).ToList<dynamic>();

            return lstWorks;
        }


        public List<dynamic> GetAssetWorksByTenderID(long _AWID)
        {
            List<dynamic> lstWorks;
            //  select count(ContractorsID) from TM_TenderWorksContractors where TenderWorksID = @TenderWorkID

            lstWorks = (from tn in context.TM_TenderNotice
                        join tw in context.TM_TenderWorks on tn.ID equals tw.TenderNoticeID
                        join aw in context.AM_AssetWork on tw.WorkSourceID equals aw.ID
                        where tn.ID == _AWID
                        select new
                        {
                            OpeningDate = tn.BidOpeningDate != null ? tn.BidOpeningDate : tw.ADM_ActualOpeningDate,
                            WorkName = aw.WorkName,
                            OpeningOffice = tw.OpeningOffice,
                        }).ToList<dynamic>();




            return lstWorks;
        }



        public dynamic GetAssetTenderInformationForADMReport(long _TenderWorkID)
        {
            dynamic Info = (from TWork in context.TM_TenderWorks
                            join Notice in context.TM_TenderNotice on TWork.TenderNoticeID equals Notice.ID
                            join AW in context.AM_AssetWork on TWork.WorkSourceID equals AW.ID into assetwork
                            from assetworks in assetwork.DefaultIfEmpty()

                            join WT in context.CW_WorkType on assetworks.AssetWorkTypeID equals WT.ID into worktype
                            from worktyp in worktype.DefaultIfEmpty()

                            where TWork.ID == _TenderWorkID
                            select new
                            {
                                TenderNotice = Notice.Name,
                                WorkName = assetworks.WorkName,
                                WorkType = worktyp.Name,
                                SubmissionTime = Notice.BidSubmissionDate,
                                OpeningTime = Notice.BidOpeningDate,
                                ActualSubmissionDate = TWork.ADM_ActualSubmissionDate,
                                ActualOpeningDate = TWork.ADM_ActualOpeningDate,
                                ActualSubmissionReason = TWork.ADM_ActualSubmissionReason,
                                ActualOpeningReason = TWork.ADM_ActualOpeningReason,
                                StatusID = TWork.WorkStatusID,
                                StatusReason = TWork.StatusReason,
                                SoldTenderCount = (context.TM_TenderWorksContractors.Where(x => x.TenderWorksID == _TenderWorkID).Count(x => x.TenderWorksID != null)),
                                SubmittedTender = (context.TM_TenderWorksContractors.Where(x => x.TenderWorksID == _TenderWorkID).Count(x => x.TenderPriced == true))

                            }).ToList()
                                .Select(u => new
                                {
                                    TenderNotice = u.TenderNotice,
                                    WorkName = u.WorkName,
                                    WorkType = u.WorkType,
                                    ActualSubmissionDate = Utility.GetTimeFormatted(u.ActualSubmissionDate),
                                    ActualOpeningDate = Utility.GetTimeFormatted(u.ActualOpeningDate),
                                    ActualSubmissionReason = u.ActualSubmissionReason,
                                    ActualOpeningReason = u.ActualOpeningReason,
                                    WorkStatusID = u.StatusID,
                                    StatusReason = u.StatusReason,
                                    SubmissionDate = Utility.GetFormattedDate(u.SubmissionTime),
                                    SubmissionTime = Utility.GetFormattedTime(u.SubmissionTime),
                                    OpeningTime = Utility.GetFormattedTime(u.OpeningTime),
                                    OpeningDate = Utility.GetFormattedDate(u.OpeningTime),
                                    SoldTenderCount = u.SoldTenderCount,
                                    SubmittedTender = u.SubmittedTender,
                                    AttachmentList = GetADMAttachment(_TenderWorkID)

                                }).FirstOrDefault();

            return Info;
        }
        #endregion

        public TM_TenderWorks GetWorkByTenderWorkID(long _TenderWorkID)
        {
            TM_TenderWorks TWObj = (from TW in context.TM_TenderWorks
                                    where TW.ID == _TenderWorkID
                                    select TW).FirstOrDefault();


            return TWObj;
        }

    }
}
