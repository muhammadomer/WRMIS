using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using PMIU.WRMIS.DAL.Repositories.WaterLosses;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.PerformanceEvaluation
{
    public class PerformanceEvaluationRepository : Repository<PE_DivisionComplexityLevel>
    {
        PE_SubCategoryWeightageRepository repSubCategoryWeightage = new ContextDB().ExtRepositoryFor<PE_SubCategoryWeightageRepository>();
        PE_CategoryWeightageRepository repCategoryWeightage = new ContextDB().ExtRepositoryFor<PE_CategoryWeightageRepository>();
        PE_EvaluationScoresDetailRepository repScoreDetails = new ContextDB().ExtRepositoryFor<PE_EvaluationScoresDetailRepository>();
        PE_FieldChannelDataRepository repFieldChannelData = new ContextDB().ExtRepositoryFor<PE_FieldChannelDataRepository>();
        PE_PMIUChannelDataRepository repPMIUChannelData = new ContextDB().ExtRepositoryFor<PE_PMIUChannelDataRepository>();
        PE_HeadGaugeDifferenceRepository repHeadGaugeDifference = new ContextDB().ExtRepositoryFor<PE_HeadGaugeDifferenceRepository>();
        PE_TailGaugeDifferenceRepository repTailGaugeDifference = new ContextDB().ExtRepositoryFor<PE_TailGaugeDifferenceRepository>();
        WaterLossesRepository repWaterLosses = new ContextDB().ExtRepositoryFor<WaterLossesRepository>();

        int Rank = 1;
        double? NetTotal = null;

        public PerformanceEvaluationRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<PE_DivisionComplexityLevel>();

        }

        /// <summary>
        /// This function returns Complexity levels of all divisions in a particular domain
        /// Created On 22-09-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <param name="_DomainID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionalComplexityLevel(long _CircleID, long _DomainID)
        {
            PE_ComplexityFactor mdlComplexityFactor = (from cfd in context.PE_ComplexityFactor
                                                       where cfd.ComplexityLevel.Trim().ToUpper() == Constants.NormalComplexityLevelName.Trim().ToUpper()
                                                       select cfd).FirstOrDefault();

            List<dynamic> lstDivisionalComplexities = (from div in context.CO_Division
                                                       join dc in context.PE_DivisionComplexityLevel on div.ID equals dc.DivisionID into divisionComplexity
                                                       from dcl in divisionComplexity.DefaultIfEmpty()
                                                       join c in context.PE_ComplexityFactor on dcl.ComplexityFactorID equals c.ID into complexityFactor
                                                       from cf in complexityFactor.DefaultIfEmpty()
                                                       where div.CircleID == _CircleID && div.DomainID == _DomainID
                                                       select new
                                                       {
                                                           ID = (dcl.ID != null ? dcl.ID : 0),
                                                           DivisionID = div.ID,
                                                           DivisionName = div.Name,
                                                           ComplexityID = (cf.ID != null ? cf.ID : mdlComplexityFactor.ID),
                                                           ComplexityName = (cf.ComplexityLevel != null ? cf.ComplexityLevel : Constants.NormalComplexityLevelName),
                                                           MultiplicationFactor = (cf.MultiplicationFactor != null ? cf.MultiplicationFactor : mdlComplexityFactor.MultiplicationFactor),
                                                           Remarks = dcl.Remarks
                                                       }).ToList<dynamic>();

            return lstDivisionalComplexities;
        }

        /// <summary>
        /// This function returns all channels that are parent to some other channel.
        /// Created On 26-09-2016
        /// </summary>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetParentChannels()
        {
            string ParentRelation = Convert.ToString(Constants.ChannelRelation.P);

            List<CO_Channel> lstChannel = (from cpf in context.CO_ChannelParentFeeder
                                           join c in context.CO_Channel on cpf.ParrentFeederID equals c.ID
                                           where cpf.RelationType == ParentRelation && cpf.StructureTypeID == (long)Constants.StructureType.Channel
                                           orderby c.NAME
                                           select c).Distinct().ToList();

            return lstChannel;
        }


        public List<dynamic> GetDivisionsReadyToExclude(long _ZoneID, long _CircleID, bool _IsExcludedDivisions)
        {
            string ParentRelation = Convert.ToString(Constants.ChannelRelation.P);

            List<dynamic> lstExcludedDivision = (from c in context.CO_Division
                                                 join exch in context.PE_DivisionExcluded on c.ID equals exch.DivisionID into exc
                                                 from ce in exc.DefaultIfEmpty()
                                                 join ecircle in context.CO_Circle on c.CircleID equals ecircle.ID into crcl
                                                 from ecrcl in crcl.DefaultIfEmpty()
                                                 join eZone in context.CO_Zone on ecrcl.ZoneID equals eZone.ID into zone
                                                 from eZne in zone.DefaultIfEmpty()
                                                 where
                                                 (ecrcl.ZoneID == _ZoneID || _ZoneID == -1) &&
                                                 (c.CircleID == _CircleID || _CircleID == -1) &&
                                                 (c.IsActive == true) &&
                                                 (ce.IsExcluded == _IsExcludedDivisions || _IsExcludedDivisions == false) &&
                                                 (c.DomainID == Constants.IrrigationDomainID)
                                                 orderby c.Name
                                                 select new
                                                 {
                                                     DivisionID = c.ID,
                                                     Division = c.Name,
                                                     Circle = ecrcl.Name,
                                                     Zone = eZne.Name,
                                                     CircleID = c.CircleID,
                                                     IsActive = c.IsActive,
                                                     IsExcluded = ce.IsExcluded == null ? false : ce.IsExcluded,
                                                 }).Distinct().ToList<dynamic>();
            return lstExcludedDivision;
        }

        /// <summary>
        /// This function returns the channel data along with exclusion bit for performance evaluation.
        /// Created On 27-09-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <param name="_CircleID"></param>
        /// <param name="_DivisionID"></param>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_CommandNameID"></param>
        /// <param name="_ChannelTypeID"></param>
        /// <param name="_FlowTypeID"></param>
        /// <param name="_ParentChannelID"></param>
        /// <param name="_ChannelName"></param>
        /// <param name="_IMISCode"></param>
        /// <param name="_GetExcludedChannels"></param>
        /// <param name="_GetZeroAuthorizedTail"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetExcludedChannels(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _CommandNameID, long _ChannelTypeID,
            long _FlowTypeID, long _ParentChannelID, string _ChannelName, string _IMISCode, bool _GetExcludedChannels, bool _GetZeroAuthorizedTail)
        {
            string ParentRelation = Convert.ToString(Constants.ChannelRelation.P);

            List<dynamic> lstExcludedChannel = (from c in context.CO_Channel
                                                join ct in context.CO_ChannelType on c.ChannelTypeID equals ct.ID
                                                join ft in context.CO_ChannelFlowType on c.FlowTypeID equals ft.ID
                                                join cmt in context.CO_ChannelComndType on c.ComndTypeID equals cmt.ID
                                                join pf in context.CO_ChannelParentFeeder on new { ChannelID = c.ID, Relation = ParentRelation, StructureTypeID = (long)Constants.StructureType.Channel } equals new { ChannelID = pf.ChannelID.Value, Relation = pf.RelationType, StructureTypeID = pf.StructureTypeID.Value } into p
                                                from cpf in p.DefaultIfEmpty()
                                                join ib in context.CO_ChannelIrrigationBoundaries on c.ID equals ib.ChannelID into i
                                                from cib in i.DefaultIfEmpty()
                                                join csc in context.CO_Section on cib.SectionID equals csc.ID into cs
                                                from s in cs.DefaultIfEmpty()
                                                join csd in context.CO_SubDivision on s.SubDivID equals csd.ID into cd
                                                from sd in cd.DefaultIfEmpty()
                                                join cdiv in context.CO_Division on new { DivisionID = sd.DivisionID.Value, DomainID = Constants.IrrigationDomainID } equals new { DivisionID = cdiv.ID, DomainID = cdiv.DomainID.Value } into cdi
                                                from di in cdi.DefaultIfEmpty()
                                                join cci in context.CO_Circle on di.CircleID equals cci.ID into cc
                                                from ci in cc.DefaultIfEmpty()
                                                join exch in context.PE_ChannelExcluded on c.ID equals exch.ChannelID into exc
                                                from ce in exc.DefaultIfEmpty()
                                                join cg in context.CO_ChannelGauge on c.ID equals cg.ChannelID
                                                join section in context.CO_Section on cg.SectionID equals section.ID into SectX
                                                from secnX in SectX.DefaultIfEmpty()
                                                join subDiv in context.CO_SubDivision on secnX.SubDivID equals subDiv.ID into SubdivX
                                                from sub in SubdivX.DefaultIfEmpty()
                                                join div in context.CO_Division on sub.DivisionID equals div.ID into DivX
                                                from divsion in DivX.DefaultIfEmpty()
                                                where
                                                       (ci.ZoneID == _ZoneID || _ZoneID == -1) &&
                                                       (di.CircleID == _CircleID || _CircleID == -1) &&
                                                       (sd.DivisionID == _DivisionID || _DivisionID == -1) &&
                                                       (s.SubDivID == _SubDivisionID || _SubDivisionID == -1) &&
                                                       (c.ComndTypeID == _CommandNameID || _CommandNameID == -1) &&
                                                       (c.ChannelTypeID == _ChannelTypeID || _ChannelTypeID == -1) &&
                                                       (c.FlowTypeID == _FlowTypeID || _FlowTypeID == -1) &&
                                                       (c.NAME.ToUpper().Trim().Contains(_ChannelName.ToUpper()) || string.IsNullOrEmpty(_ChannelName)) &&
                                                       (cpf.ParrentFeederID == _ParentChannelID || _ParentChannelID == -1) &&
                                                       (c.IMISCode == _IMISCode || string.IsNullOrEmpty(_IMISCode)) &&
                                                       (ce.IsExcluded == _GetExcludedChannels || _GetExcludedChannels == false) &&
                                                       (ct.ID == (long)Constants.ChannelType.DistributaryMajor || ct.ID == (long)Constants.ChannelType.DistributaryMinor || ct.ID == (long)Constants.ChannelType.DistributarySubMinor) &&
                                                       (c.AuthorizedTailGauge == 0 || _GetZeroAuthorizedTail == false) &&
                                                       (cg.GaugeCategoryID == (long)(Constants.GaugeCategory.HeadGauge))
                                                orderby c.NAME
                                                select new
                                                {
                                                    ID = (ce.ID != null ? ce.ID : 0),
                                                    ChannelID = c.ID,
                                                    ChannelName = c.NAME,
                                                    IMISCode = c.IMISCode,
                                                    ChannelType = ct.Name,
                                                    FlowType = ft.Name,
                                                    TotalRDs = c.TotalRDs,
                                                    CommandName = cmt.Name,
                                                    CCA = c.CCAAcre,
                                                    GCA = c.GCAAcre,
                                                    AuthorizedTail = c.AuthorizedTailGauge,
                                                    IsExcluded = (ce.IsExcluded != null ? ce.IsExcluded : false),
                                                    Division = divsion.Name
                                                }).Distinct().ToList<dynamic>();
            return lstExcludedChannel;
        }

        /// <summary>
        /// This function executes the performance evaluation query and saves results to database.
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_Session"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <param name="_UserID"></param>               
        public void CalculatePEScore(long _IrrigationLevelID, string _Session, DateTime _FromDate, DateTime _ToDate, long _UserID)
        {
            int DaysCount = (_ToDate - _FromDate).Days + 1;

            #region Channels Query

            List<dynamic> lstChannels = (from c in context.CO_Channel
                                         join cg in context.CO_ChannelGauge on new { ChannelID = c.ID, CategoryID = (long)Constants.GaugeCategory.TailGauge } equals new { ChannelID = cg.ChannelID.Value, CategoryID = cg.GaugeCategoryID }
                                         join s in context.CO_Section on cg.SectionID equals s.ID
                                         join sd in context.CO_SubDivision on s.SubDivID equals sd.ID
                                         join d in context.CO_Division on new { DivisionID = sd.DivisionID.Value, DomainID = Constants.IrrigationDomainID } equals new { DivisionID = d.ID, DomainID = d.DomainID.Value }
                                         join cir in context.CO_Circle on d.CircleID equals cir.ID
                                         join z in context.CO_Zone on cir.ZoneID equals z.ID
                                         join de in context.PE_DivisionExcluded on d.ID equals de.DivisionID into dex
                                         from dx in dex.DefaultIfEmpty()
                                         join e in context.PE_ChannelExcluded on c.ID equals e.ChannelID into ec
                                         from ce in ec.DefaultIfEmpty()
                                         where (new List<long>() { (long)Constants.ChannelType.DistributaryMajor, (long)Constants.ChannelType.DistributaryMinor, (long)Constants.ChannelType.DistributarySubMinor }).Contains(c.ChannelTypeID) &&
                                                (dx.IsExcluded == null || dx.IsExcluded == false)

                                         orderby s.ID, sd.ID, d.ID, cir.ID, z.ID
                                         select new
                                         {
                                             IrrigationBoundryID = (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone ? z.ID : _IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle ? cir.ID : _IrrigationLevelID == (long)Constants.IrrigationLevelID.Division ? d.ID : _IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision ? sd.ID : s.ID),
                                             IrrigationBoundryName = (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone ? z.Name : _IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle ? cir.Name : _IrrigationLevelID == (long)Constants.IrrigationLevelID.Division ? d.Name : _IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision ? sd.Name : s.Name),
                                             ChannelID = c.ID,
                                             ChannelName = c.NAME,
                                             IsExcluded = ce.IsExcluded
                                         }).ToList<dynamic>();

            #endregion

            List<long> lstBoundryIDs = lstChannels.Select(s => (long)s.IrrigationBoundryID).Distinct().ToList<long>();

            List<long> lstChannelIDs = null;

            #region Save Evaluation Score record

            PE_EvaluationScoresRepository repEvaluationScores = new ContextDB().ExtRepositoryFor<PE_EvaluationScoresRepository>();
            PE_EvaluationScores mdlEvaluationScores = new PE_EvaluationScores
            {
                IrrigationLevelID = _IrrigationLevelID,
                Session = _Session,
                FromDate = _FromDate.Date,
                ToDate = _ToDate.Date,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = _UserID,
                ModifiedBy = _UserID
            };

            repEvaluationScores.Insert(mdlEvaluationScores);
            repEvaluationScores.Save();


            long EvalID = mdlEvaluationScores.ID;

            #endregion

            #region Queries and Data Perparation

            List<PE_FieldChannelData> lstFieldChannelData = context.PE_FieldChannelData.Where(fcd => DbFunctions.TruncateTime(fcd.ReadingDate) >= DbFunctions.TruncateTime(_FromDate) && DbFunctions.TruncateTime(fcd.ReadingDate) <= DbFunctions.TruncateTime(_ToDate)).ToList();
            List<PE_PMIUChannelData> lstPMIUChannelData = context.PE_PMIUChannelData.Where(pcd => DbFunctions.TruncateTime(pcd.ReadingDate) >= DbFunctions.TruncateTime(_FromDate) && DbFunctions.TruncateTime(pcd.ReadingDate) <= DbFunctions.TruncateTime(_ToDate)).ToList();
            List<PE_TailGaugeDifference> lstTailGaugeDifference = context.PE_TailGaugeDifference.Where(tgd => DbFunctions.TruncateTime(tgd.ReadingDate) >= DbFunctions.TruncateTime(_FromDate) && DbFunctions.TruncateTime(tgd.ReadingDate) <= DbFunctions.TruncateTime(_ToDate)).ToList();
            List<PE_HeadGaugeDifference> lstHeadGaugeDifference = context.PE_HeadGaugeDifference.Where(hgd => DbFunctions.TruncateTime(hgd.ReadingDate) >= DbFunctions.TruncateTime(_FromDate) && DbFunctions.TruncateTime(hgd.ReadingDate) <= DbFunctions.TruncateTime(_ToDate)).ToList();

            List<PE_KPISubCategories> lstSubCategories = context.PE_KPISubCategories.ToList();
            List<CO_Division> lstDivision = context.CO_Division.ToList();
            List<CO_SubDivision> lstSubDivision = context.CO_SubDivision.ToList();
            List<CO_Section> lstSection = context.CO_Section.ToList();
            List<PE_DivisionComplexityLevel> lstDivisionComplexityLevel = context.PE_DivisionComplexityLevel.ToList();

            List<dynamic> lstWaterTheftCases = (from wtc in context.WT_WaterTheftCase
                                                join u in context.UA_Users on wtc.UserID equals u.ID
                                                join d in context.UA_Designations on u.DesignationID equals d.ID
                                                where DbFunctions.TruncateTime(wtc.IncidentDateTime) >= DbFunctions.TruncateTime(_FromDate) &&
                                                DbFunctions.TruncateTime(wtc.IncidentDateTime) <= DbFunctions.TruncateTime(_ToDate)
                                                select new
                                                {
                                                    WaterTheftID = wtc.ID,
                                                    ChannelID = wtc.ChannelID,
                                                    OrganizationID = d.OrganizationID
                                                }).ToList<dynamic>();

            List<dynamic> lstComplaintData = (from c in context.CM_Complaint
                                              where DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) && DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate)
                                              select new
                                              {
                                                  Complaint = c,
                                                  AssignmentHistory = context.CM_ComplaintAssignmentHistory.Where(cah => cah.ComplaintID == c.ID &&
                                                      (cah.AssignedToDesig == (long)Constants.Designation.XEN || cah.AssignedToDesig == (long)Constants.Designation.ADM)).ToList()
                                              }).ToList<dynamic>();

            List<dynamic> lstFieldChannel = null;
            List<dynamic> lstTailDifference = null;
            List<dynamic> lstHeadDifference = null;

            if (_Session.ToUpper().Trim() == "M")
            {
                lstFieldChannel = (from lfcd in lstFieldChannelData
                                   join lsc in lstSubCategories on lfcd.TailStatusM equals lsc.ID into nlsc
                                   from sc in nlsc.DefaultIfEmpty()
                                   where (sc == null || sc.PEInclude == true)
                                   select new
                                   {
                                       ChannelID = lfcd.ChannelID,
                                       ReadingDate = lfcd.ReadingDate,
                                       TailStatus = lfcd.TailStatusM,
                                       IsGaugePainted = lfcd.IsGaugePaintedM,
                                       IsGaugeFixed = lfcd.IsGaugeFixedM
                                   }).ToList<dynamic>();

                lstTailDifference = (from ltgd in lstTailGaugeDifference
                                     join lsc in lstSubCategories on ltgd.DifferenceStatusM equals lsc.ID into nlsc
                                     from sc in nlsc.DefaultIfEmpty()
                                     where (sc == null || sc.PEInclude == true)
                                     select new
                                     {
                                         ChannelID = ltgd.ChannelID,
                                         ReadingDate = ltgd.ReadingDate,
                                         Status = ltgd.DifferenceStatusM
                                     }).ToList<dynamic>();

                lstHeadDifference = (from lhgd in lstHeadGaugeDifference
                                     join lsc in lstSubCategories on lhgd.DifferenceStatusM equals lsc.ID into nlsc
                                     from sc in nlsc.DefaultIfEmpty()
                                     where (sc == null || sc.PEInclude == true)
                                     select new
                                     {
                                         ChannelID = lhgd.ChannelID,
                                         ReadingDate = lhgd.ReadingDate,
                                         Status = lhgd.DifferenceStatusM
                                     }).ToList<dynamic>();
            }
            else if (_Session.ToUpper().Trim() == "E")
            {
                lstFieldChannel = (from lfcd in lstFieldChannelData
                                   join lsc in lstSubCategories on lfcd.TailStatusE equals lsc.ID into nlsc
                                   from sc in nlsc.DefaultIfEmpty()
                                   where (sc == null || sc.PEInclude == true)
                                   select new
                                   {
                                       ChannelID = lfcd.ChannelID,
                                       ReadingDate = lfcd.ReadingDate,
                                       TailStatus = lfcd.TailStatusE,
                                       IsGaugePainted = lfcd.IsGaugePaintedE,
                                       IsGaugeFixed = lfcd.IsGaugeFixedE
                                   }).ToList<dynamic>();

                lstTailDifference = (from ltgd in lstTailGaugeDifference
                                     join lsc in lstSubCategories on ltgd.DifferenceStatusE equals lsc.ID into nlsc
                                     from sc in nlsc.DefaultIfEmpty()
                                     where (sc == null || sc.PEInclude == true)
                                     select new
                                     {
                                         ChannelID = ltgd.ChannelID,
                                         ReadingDate = ltgd.ReadingDate,
                                         Status = ltgd.DifferenceStatusE
                                     }).ToList<dynamic>();

                lstHeadDifference = (from lhgd in lstHeadGaugeDifference
                                     join lsc in lstSubCategories on lhgd.DifferenceStatusE equals lsc.ID into nlsc
                                     from sc in nlsc.DefaultIfEmpty()
                                     where (sc == null || sc.PEInclude == true)
                                     select new
                                     {
                                         ChannelID = lhgd.ChannelID,
                                         ReadingDate = lhgd.ReadingDate,
                                         Status = lhgd.DifferenceStatusE
                                     }).ToList<dynamic>();
            }
            else
            {
                lstFieldChannel = (from lfcd in lstFieldChannelData
                                   join lsc in lstSubCategories on lfcd.TailStatusA equals lsc.ID into nlsc
                                   from sc in nlsc.DefaultIfEmpty()
                                   where (sc == null || sc.PEInclude == true)
                                   select new
                                   {
                                       ChannelID = lfcd.ChannelID,
                                       ReadingDate = lfcd.ReadingDate,
                                       TailStatus = lfcd.TailStatusA,
                                       IsGaugePaintedM = lfcd.IsGaugePaintedM,
                                       IsGaugePaintedE = lfcd.IsGaugePaintedE,
                                       IsGaugeFixedM = lfcd.IsGaugeFixedM,
                                       IsGaugeFixedE = lfcd.IsGaugeFixedE
                                   }).ToList<dynamic>();

                lstTailDifference = (from ltgd in lstTailGaugeDifference
                                     join lsc in lstSubCategories on ltgd.DifferenceStatusA equals lsc.ID into nlsc
                                     from sc in nlsc.DefaultIfEmpty()
                                     where (sc == null || sc.PEInclude == true)
                                     select new
                                     {
                                         ChannelID = ltgd.ChannelID,
                                         ReadingDate = ltgd.ReadingDate,
                                         Status = ltgd.DifferenceStatusA
                                     }).ToList<dynamic>();

                lstHeadDifference = (from lhgd in lstHeadGaugeDifference
                                     join lsc in lstSubCategories on lhgd.DifferenceStatusA equals lsc.ID into nlsc
                                     from sc in nlsc.DefaultIfEmpty()
                                     where (sc == null || sc.PEInclude == true)
                                     select new
                                     {
                                         ChannelID = lhgd.ChannelID,
                                         ReadingDate = lhgd.ReadingDate,
                                         Status = lhgd.DifferenceStatusA
                                     }).ToList<dynamic>();
            }

            List<dynamic> lstPMIUChannel = (from lpcd in lstPMIUChannelData
                                            join lsc in lstSubCategories on lpcd.TailStatus equals lsc.ID into nlsc
                                            from sc in nlsc.DefaultIfEmpty()
                                            where (sc == null || sc.PEInclude == true)
                                            select new
                                            {
                                                ChannelID = lpcd.ChannelID,
                                                ReadingDate = lpcd.ReadingDate,
                                                TailStatus = lpcd.TailStatus,
                                                IsGaugePainted = lpcd.IsGaugePainted,
                                                IsGaugeFixed = lpcd.IsGaugeFixed
                                            }).ToList<dynamic>();

            #endregion

            List<dynamic> lstChannelTailStatus = null;
            List<dynamic> lstChannelTailPMIUStatus = null;
            List<dynamic> lstTailDifferenceStatus = null;
            List<dynamic> lstHeadDifferenceStatus = null;

            foreach (long BoundryID in lstBoundryIDs)
            {
                lstChannelIDs = lstChannels.Where(c => c.IrrigationBoundryID == BoundryID && c.IsExcluded != true).Select(c => (long)c.ChannelID).ToList<long>();

                int AllChannels = lstChannels.Where(c => c.IrrigationBoundryID == BoundryID).Count();

                /******************************************************************************************
                 * Closed Channels need to be found and excluded from the list of Channels on daily basis.
                 * Currently, this has been done in the SP by using ID "-1".
                 ******************************************************************************************/

                #region Dataset filtering queries

                lstChannelTailStatus = lstFieldChannel.Where(lcts => lstChannelIDs.Contains(lcts.ChannelID)).ToList<dynamic>();
                lstChannelTailPMIUStatus = lstPMIUChannel.Where(lctps => lstChannelIDs.Contains(lctps.ChannelID)).ToList<dynamic>();
                lstTailDifferenceStatus = lstTailDifference.Where(ltd => lstChannelIDs.Contains(ltd.ChannelID)).ToList<dynamic>();
                lstHeadDifferenceStatus = lstHeadDifference.Where(lhd => lstChannelIDs.Contains(lhd.ChannelID)).ToList<dynamic>();

                int ChannelCount = lstChannelIDs.Count();

                int AggregatedChannels = (ChannelCount * DaysCount);
                int PMIUCheckedChannels = lstChannelTailPMIUStatus.Count();

                int ClosedChannelCount = lstChannelTailStatus.Where(cts => cts.TailStatus == -1).Count();

                List<dynamic> lstComplaintCases = null;

                if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                {
                    List<long> lstDivisionIDs = lstDivision.Where(d => d.CO_Circle.ZoneID == BoundryID).Select(d => d.ID).ToList<long>();

                    lstComplaintCases = lstComplaintData.Where(lcd => lstDivisionIDs.Contains(lcd.Complaint.DivisionID)).ToList<dynamic>();
                }
                else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                {
                    List<long> lstDivisionIDs = lstDivision.Where(d => d.CircleID == BoundryID).Select(d => d.ID).ToList<long>();

                    lstComplaintCases = lstComplaintData.Where(lcd => lstDivisionIDs.Contains(lcd.Complaint.DivisionID)).ToList<dynamic>();
                }
                else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                {
                    lstComplaintCases = lstComplaintData.Where(lcd => lcd.Complaint.DivisionID == BoundryID).ToList<dynamic>();
                }
                else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                {
                    long DivisionID = lstSubDivision.Where(lsd => lsd.ID == BoundryID).Select(lsd => lsd.DivisionID.Value).FirstOrDefault();

                    lstComplaintCases = lstComplaintData.Where(lcd => lcd.Complaint.DivisionID == DivisionID).ToList<dynamic>();
                }
                else
                {
                    long DivisionID = lstSection.Where(ls => ls.ID == BoundryID).Select(ls => ls.CO_SubDivision.DivisionID.Value).FirstOrDefault();

                    lstComplaintCases = lstComplaintData.Where(lcd => lcd.Complaint.DivisionID == DivisionID).ToList<dynamic>();
                }

                #endregion

                double Score = GetDryTailScore(lstChannelTailStatus, lstChannelTailPMIUStatus, lstSubCategories, AggregatedChannels, PMIUCheckedChannels, EvalID, _UserID, BoundryID);

                Score = Score + GetShortTailScore(lstChannelTailStatus, lstChannelTailPMIUStatus, lstSubCategories, AggregatedChannels, PMIUCheckedChannels, EvalID, _UserID, BoundryID);

                Score = Score + GetAuthorizedTailScore(lstChannelTailStatus, lstChannelTailPMIUStatus, lstSubCategories, AggregatedChannels, PMIUCheckedChannels, EvalID, _UserID, BoundryID);

                Score = Score + GetExcessiveTailScore(lstChannelTailStatus, lstChannelTailPMIUStatus, lstSubCategories, AggregatedChannels, PMIUCheckedChannels, EvalID, _UserID, BoundryID);

                #region Data Not Recorded Score Calculation

                PE_KPISubCategories DataNotRecordedCategory = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "DNR").FirstOrDefault();

                if (DataNotRecordedCategory.PEInclude.Value)
                {
                    double DataNotRecorded = ((ChannelCount * DaysCount) - lstChannelTailStatus.Where(cts => cts.TailStatus != null).Count());
                    double DataNotRecordedPercentage = 0;
                    double DataNotRecordedScore = 0;

                    if (AggregatedChannels != 0)
                    {
                        DataNotRecordedPercentage = (DataNotRecorded / AggregatedChannels) * 100;
                        DataNotRecordedScore = (DataNotRecordedPercentage * DataNotRecordedCategory.WeightCurrent.Value) / 100;
                    }

                    PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = DataNotRecordedCategory.KPICategoryID,
                        SubCatID = DataNotRecordedCategory.ID,
                        SubCatName = DataNotRecordedCategory.Name,
                        SubCatDescription = DataNotRecordedCategory.Description,
                        TotalPoints = DataNotRecordedCategory.WeightCurrent,
                        Source = DataNotRecordedCategory.Source,
                        Score = Math.Round(DataNotRecordedScore, 2),
                        PS = "P",
                        PSValue = Math.Round(DataNotRecordedPercentage, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);

                    PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = DataNotRecordedCategory.KPICategoryID,
                        CatName = DataNotRecordedCategory.PE_KPICategories.Name,
                        CatDescription = DataNotRecordedCategory.PE_KPICategories.Description,
                        FDTotalPoints = DataNotRecordedCategory.WeightCurrent,
                        FDWeightage = Math.Round(DataNotRecordedScore, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repCategoryWeightage.Insert(mdlCategoryWeightage);

                    Score = Score + DataNotRecordedScore;
                }

                #endregion

                #region Water Losses Calculation

                PE_KPISubCategories WaterLossesCategory = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "WL").FirstOrDefault();

                if (WaterLossesCategory.PEInclude.Value)
                {
                    double TotalLoss = 0;
                    double TotalDischarge = 0;

                    if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone || _IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle
                        || _IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                    {
                        List<long> lstDivisionIDs = new List<long>();

                        if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                        {
                            lstDivisionIDs = (from d in context.CO_Division
                                              join c in context.CO_Circle on d.CircleID equals c.ID
                                              where c.ZoneID == BoundryID
                                              select d.ID).ToList<long>();
                        }
                        else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        {
                            lstDivisionIDs = (from d in context.CO_Division
                                              where d.CircleID == BoundryID
                                              select d.ID).ToList<long>();
                        }
                        else
                        {
                            lstDivisionIDs.Add(BoundryID);
                        }

                        string day = (_FromDate.Day < 10 ? ("0" + _FromDate.Day) : "" + _FromDate.Day);
                        string month = (_FromDate.Month < 10 ? ("0" + _FromDate.Month) : "" + _FromDate.Month);
                        string fromDay = _FromDate.Year + "-" + month + "-" + day;

                        day = (_ToDate.Day < 10 ? ("0" + _ToDate.Day) : "" + _ToDate.Day);
                        month = (_ToDate.Month < 10 ? ("0" + _ToDate.Month) : "" + _ToDate.Month);
                        string toDay = _ToDate.Year + "-" + month + "-" + day;

                        foreach (long DivisionID in lstDivisionIDs)
                        {
                            List<WL_GetDivisionalLGData_Result> lstDivisionalData = (from dlgd in context.WL_GetDivisionalLGData(DivisionID, 1, fromDay, toDay, 0, 0, 0)
                                                                                     select dlgd).ToList<WL_GetDivisionalLGData_Result>();

                            List<long> lstSubDivisionIDs = lstDivisionalData.Select(ldd => ldd.SubDivGaugeID).Distinct().ToList<long>();

                            double TotalDiversions = 0;

                            foreach (long SubDivisionID in lstSubDivisionIDs)
                            {
                                double Offtakes = lstDivisionalData.Where(ldd => ldd.SubDivGaugeID == SubDivisionID).Average(ldd => (ldd.Offtakes == null ? 0 : ldd.Offtakes.Value));
                                double DirectOutlet = lstDivisionalData.Where(ldd => ldd.SubDivGaugeID == SubDivisionID).Average(ldd => (ldd.OutletDDSum == null ? 0 : ldd.OutletDDSum.Value));
                                double Diversions = Offtakes + DirectOutlet;

                                TotalDiversions = TotalDiversions + Diversions;
                            }

                            DataTable DischargeData = new ContextDB().ExecuteStoredProcedureDataTable("WL_GetCurrentAndNextDivisionalDischarge", DivisionID, _FromDate, _ToDate);

                            double DivisionalDischarge = Convert.ToDouble(DischargeData.Rows[0][0]);
                            double NextDivisionDischarge = Convert.ToDouble(DischargeData.Rows[0][1]);

                            double Losses = TotalDiversions - DivisionalDischarge; // TotalDiversions + NextDivisionDischarge - DivisionalDischarge;

                            TotalLoss = TotalLoss + Losses;
                            TotalDischarge = TotalDischarge + DivisionalDischarge;
                        }
                    }
                    else
                    {
                        long SubDivisionID = 0;

                        if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                        {
                            SubDivisionID = BoundryID;
                        }
                        else
                        {
                            SubDivisionID = (from s in context.CO_Section where s.ID == BoundryID select s.SubDivID.Value).FirstOrDefault();
                        }

                        List<WaterLossesDAL.SubDiv_WL> lstSubDiv = new WaterLossesDAL().GetSubDivisionalLoss_Daily(SubDivisionID, _FromDate, _ToDate);

                        double TotalDiversions = 0;

                        foreach (WaterLossesDAL.SubDiv_WL SubDiv in lstSubDiv)
                        {
                            double Offtakes = (SubDiv.LstAttributes_offtakes == null ? 0 : SubDiv.LstAttributes_offtakes.Average(of => (of.Value == null ? 0 : of.Value.Value)));
                            double DirectOutlet = (SubDiv.LstAttributes_Outlets == null ? 0 : SubDiv.LstAttributes_Outlets.Average(of => (of.Value == null ? 0 : of.Value.Value)));
                            double Diversions = Offtakes + DirectOutlet;

                            TotalDiversions = TotalDiversions + Diversions;
                        }

                        DataTable DischargeData = new ContextDB().ExecuteStoredProcedureDataTable("WL_GetCurrentAndNextSubDivisionalDischarge", SubDivisionID, _FromDate, _ToDate);

                        double SubDivisionalDischarge = Convert.ToDouble(DischargeData.Rows[0][0]);
                        double NextSubDivisionDischarge = Convert.ToDouble(DischargeData.Rows[0][1]);

                        TotalLoss = TotalDiversions - SubDivisionalDischarge; // TotalDiversions + NextSubDivisionDischarge - SubDivisionalDischarge;
                        TotalDischarge = SubDivisionalDischarge;
                    }

                    //double? FromDischarge = 0;
                    //double? Difference = 0;

                    //foreach (long ChannelID in lstChannelIDs)
                    //{
                    //    List<GetWaterLosses_ChannelWise_Result> lstChannelWaterLosses = repWaterLosses.GetChannelWaterLosses(ChannelID, _FromDate, _ToDate);

                    //    if (lstChannelWaterLosses.Count() != 0)
                    //    {
                    //        FromDischarge = FromDischarge + lstChannelWaterLosses.ElementAt(0).FromDischarge;
                    //        Difference = Difference + lstChannelWaterLosses.Select(lwl => lwl.Diff).Sum();
                    //    }
                    //}

                    double WaterLossesPercentage = 0;
                    double WaterLossesScore = 0;

                    if (TotalDischarge != 0)
                    {
                        WaterLossesPercentage = (TotalLoss / TotalDischarge) * 100;
                        WaterLossesScore = -1 * (WaterLossesPercentage * WaterLossesCategory.WeightCurrent.Value) / 100;
                    }

                    //if (FromDischarge != 0 && FromDischarge != null && Difference != null)
                    //{
                    //    WaterLossesPercentage = (Difference.Value / FromDischarge.Value) * 100;
                    //    WaterLossesScore = (WaterLossesPercentage * WaterLossesCategory.WeightCurrent.Value) / 100;
                    //}

                    PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = WaterLossesCategory.KPICategoryID,
                        SubCatID = WaterLossesCategory.ID,
                        SubCatName = WaterLossesCategory.Name,
                        SubCatDescription = WaterLossesCategory.Description,
                        TotalPoints = WaterLossesCategory.WeightCurrent,
                        Source = WaterLossesCategory.Source,
                        Score = Math.Round(WaterLossesScore, 2),
                        PS = "P",
                        PSValue = Math.Round(WaterLossesPercentage, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);

                    PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = WaterLossesCategory.KPICategoryID,
                        CatName = WaterLossesCategory.PE_KPICategories.Name,
                        CatDescription = WaterLossesCategory.PE_KPICategories.Description,
                        FDTotalPoints = WaterLossesCategory.WeightCurrent,
                        FDWeightage = Math.Round(WaterLossesScore, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repCategoryWeightage.Insert(mdlCategoryWeightage);

                    Score = Score + WaterLossesScore;
                }

                #endregion

                Score = Score + GetDifferenceHeadGaugeScore(lstHeadDifferenceStatus, lstSubCategories, EvalID, _UserID, BoundryID);

                Score = Score + GetDifferenceTailGaugeScore(lstTailDifferenceStatus, lstSubCategories, EvalID, _UserID, BoundryID);

                #region Water Theft Identification Score

                if (lstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "WATER THEFT IDENTIFICATION").Select(lsc => lsc.PE_KPICategories.PEInclude.Value).FirstOrDefault())
                {
                    double WTI1Score = 0;
                    double WTI2Score = 0;
                    double FieldTotalPoints = 0;
                    double PMIUTotalPoints = 0;

                    PE_KPISubCategories WTI1Category = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "WTI1").FirstOrDefault();

                    if (WTI1Category.PEInclude.Value)
                    {
                        FieldTotalPoints = WTI1Category.WeightCurrent.Value;

                        double WTI1Count = lstWaterTheftCases.Where(lwtc => lwtc.OrganizationID == (long)Constants.Organization.Irrigation && lstChannelIDs.Contains(lwtc.ChannelID)).Count();
                        double WTI1CalculatedScore = WTI1Count * WTI1Category.BaseWeight.Value;
                        WTI1Score = (WTI1CalculatedScore > WTI1Category.WeightCurrent.Value ? WTI1Category.WeightCurrent.Value : WTI1CalculatedScore);

                        PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                        {
                            EvalScoreID = EvalID,
                            IrrigationBoundaryID = BoundryID,
                            CatID = WTI1Category.KPICategoryID,
                            SubCatID = WTI1Category.ID,
                            SubCatName = WTI1Category.Name,
                            SubCatDescription = WTI1Category.Description,
                            TotalPoints = WTI1Category.WeightCurrent,
                            Source = WTI1Category.Source,
                            Score = Math.Round(WTI1Score, 2),
                            PS = "S",
                            PSValue = Math.Round(WTI1Count, 2),
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = _UserID,
                            ModifiedBy = _UserID
                        };

                        repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);
                    }

                    PE_KPISubCategories WTI2Category = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "WTI2").FirstOrDefault();

                    if (WTI2Category.PEInclude.Value)
                    {
                        PMIUTotalPoints = WTI2Category.WeightCurrent.Value;

                        double WTI2Count = lstWaterTheftCases.Where(lwtc => lwtc.OrganizationID == (long)Constants.Organization.PMIU && lstChannelIDs.Contains(lwtc.ChannelID)).Count();
                        double WTI2CalculatedScore = WTI2Count * WTI2Category.BaseWeight.Value;
                        WTI2Score = (WTI2CalculatedScore < WTI2Category.WeightCurrent.Value ? WTI2Category.WeightCurrent.Value : WTI2CalculatedScore);

                        PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                        {
                            EvalScoreID = EvalID,
                            IrrigationBoundaryID = BoundryID,
                            CatID = WTI2Category.KPICategoryID,
                            SubCatID = WTI2Category.ID,
                            SubCatName = WTI2Category.Name,
                            SubCatDescription = WTI2Category.Description,
                            TotalPoints = WTI2Category.WeightCurrent,
                            Source = WTI2Category.Source,
                            Score = Math.Round(WTI2Score, 2),
                            PS = "S",
                            PSValue = Math.Round(WTI2Count, 2),
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = _UserID,
                            ModifiedBy = _UserID
                        };

                        repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);
                    }

                    PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = WTI1Category.KPICategoryID,
                        CatName = WTI1Category.PE_KPICategories.Name,
                        CatDescription = WTI1Category.PE_KPICategories.Description,
                        FDTotalPoints = FieldTotalPoints,
                        FDWeightage = Math.Round(WTI1Score, 2),
                        PMIUTotalPoints = PMIUTotalPoints,
                        PMIUWeightage = Math.Round(WTI2Score, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repCategoryWeightage.Insert(mdlCategoryWeightage);

                    Score = Score + WTI1Score + WTI2Score;
                }

                #endregion

                #region Complain Resolution Efficiency Score

                if (lstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "COMPLAINT RESOLUTION EFFICIENCY").Select(lsc => lsc.PE_KPICategories.PEInclude.Value).FirstOrDefault())
                {
                    int DelayDaysCount = 0;
                    double CRE1Score = 0;

                    PE_KPISubCategories CRE1Category = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "CRE1").FirstOrDefault();

                    if (CRE1Category.PEInclude.Value)
                    {
                        foreach (dynamic data in lstComplaintCases)
                        {
                            CM_Complaint mdlComplaint = (CM_Complaint)data.Complaint;
                            List<CM_ComplaintAssignmentHistory> lstComplaintAssignmentHistory = (List<CM_ComplaintAssignmentHistory>)data.AssignmentHistory;

                            if (lstComplaintAssignmentHistory.Count != 0)
                            {
                                CM_ComplaintAssignmentHistory mdlADMAssigned = lstComplaintAssignmentHistory.Where(cah => cah.AssignedToDesig == (long)Constants.Designation.ADM).FirstOrDefault();

                                if (mdlADMAssigned != null)
                                {
                                    CM_ComplaintAssignmentHistory mdlXENAssigned = lstComplaintAssignmentHistory.Where(cah => cah.AssignedToDesig == (long)Constants.Designation.XEN).FirstOrDefault();

                                    if (mdlXENAssigned != null)
                                    {
                                        int ResponseDuration = Convert.ToInt32(mdlComplaint.ResponseDuration);

                                        Debug.WriteLine("Step 6 at Time : " + DateTime.Now.ToString("hh:mm:ss"));

                                        if (mdlADMAssigned.AssignedDate != null && mdlXENAssigned.AssignedDate != null)
                                        {
                                            DateTime ADMAssignedDate = mdlADMAssigned.AssignedDate.Value;
                                            DateTime XENAssignedDate = mdlXENAssigned.AssignedDate.Value;

                                            int XENResponseTime = (ADMAssignedDate - XENAssignedDate).Days + 1;

                                            if (XENResponseTime > ResponseDuration)
                                            {
                                                int Difference = XENResponseTime - ResponseDuration;

                                                DelayDaysCount = DelayDaysCount + Difference;

                                                double ComplaintScore = (Difference * CRE1Category.BaseSubWeight.Value > CRE1Category.BaseWeight.Value ? CRE1Category.BaseWeight.Value : Difference * CRE1Category.BaseSubWeight.Value);

                                                CRE1Score = CRE1Score + ComplaintScore;
                                            }
                                        }
                                    }
                                }
                            }

                            if (Score > CRE1Category.WeightCurrent.Value)
                            {
                                CRE1Score = CRE1Category.WeightCurrent.Value;
                                break;
                            }
                        }

                        PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                        {
                            EvalScoreID = EvalID,
                            IrrigationBoundaryID = BoundryID,
                            CatID = CRE1Category.KPICategoryID,
                            SubCatID = CRE1Category.ID,
                            SubCatName = CRE1Category.Name,
                            SubCatDescription = CRE1Category.Description,
                            TotalPoints = CRE1Category.WeightCurrent,
                            Source = CRE1Category.Source,
                            Score = Math.Round(CRE1Score, 2),
                            PS = "S",
                            PSValue = DelayDaysCount,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = _UserID,
                            ModifiedBy = _UserID
                        };

                        repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);
                    }

                    /****************************************************************
                     * KPI Sub Category CRE2 is not being calculated as 
                     * it is not feasible to divide complains between field and PMIU.
                     * Many complains will be auto generated. 
                     * This has been suggested by BA (Shehzad Sahib)
                     ***************************************************************/

                    PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = CRE1Category.KPICategoryID,
                        CatName = CRE1Category.PE_KPICategories.Name,
                        CatDescription = CRE1Category.PE_KPICategories.Description,
                        FDTotalPoints = CRE1Category.WeightCurrent,
                        FDWeightage = Math.Round(CRE1Score, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repCategoryWeightage.Insert(mdlCategoryWeightage);
                }

                #endregion

                #region Gauge Not Painted Score Calculation

                if (lstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "GAUGING STATUS (GAUGE NOT PAINTED)").Select(lsc => lsc.PE_KPICategories.PEInclude.Value).FirstOrDefault())
                {
                    double GNP1Score = 0;
                    double GNP2Score = 0;
                    double FieldTotalPoints = 0;
                    double PMIUTotalPoints = 0;
                    PE_KPISubCategories GNP1Category = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "GNP1").FirstOrDefault();

                    if (GNP1Category.PEInclude.Value)
                    {
                        FieldTotalPoints = GNP1Category.WeightCurrent.Value;

                        double GNP1Count = 0;
                        double GNP1Percentage = 0;

                        if (_Session.ToUpper().Trim() != "A")
                        {
                            GNP1Count = lstChannelTailStatus.Where(cts => cts.IsGaugePainted == false && cts.TailStatus != null && cts.TailStatus != -1).Count();

                            if (AggregatedChannels != 0)
                            {
                                GNP1Percentage = (GNP1Count / AggregatedChannels) * 100;
                            }
                        }
                        else
                        {
                            GNP1Count = lstChannelTailStatus.Where(cts => cts.IsGaugePaintedM == false && cts.TailStatus != null && cts.TailStatus != -1).Count();
                            GNP1Count = GNP1Count + lstChannelTailStatus.Where(cts => cts.IsGaugePaintedE == false && cts.TailStatus != null && cts.TailStatus != -1).Count();

                            if (AggregatedChannels != 0)
                            {
                                GNP1Percentage = (GNP1Count / (AggregatedChannels * 2)) * 100;
                            }
                        }

                        GNP1Score = (GNP1Percentage * GNP1Category.WeightCurrent.Value) / 100;

                        PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                        {
                            EvalScoreID = EvalID,
                            IrrigationBoundaryID = BoundryID,
                            CatID = GNP1Category.KPICategoryID,
                            SubCatID = GNP1Category.ID,
                            SubCatName = GNP1Category.Name,
                            SubCatDescription = GNP1Category.Description,
                            TotalPoints = GNP1Category.WeightCurrent,
                            Source = GNP1Category.Source,
                            Score = Math.Round(GNP1Score, 2),
                            PS = "P",
                            PSValue = Math.Round(GNP1Percentage, 2),
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = _UserID,
                            ModifiedBy = _UserID
                        };

                        repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);

                        Debug.WriteLine("Step 9 at Time : " + DateTime.Now.ToString("hh:mm:ss"));
                    }

                    if (PMIUCheckedChannels != 0)
                    {
                        PE_KPISubCategories GNP2Category = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "GNP2").FirstOrDefault();

                        if (GNP2Category.PEInclude.Value)
                        {
                            PMIUTotalPoints = GNP2Category.WeightCurrent.Value;

                            double GNP2Count = lstChannelTailPMIUStatus.Where(ctps => ctps.IsGaugePainted == false && ctps.TailStatus != null && ctps.TailStatus != -1).Count();
                            double GNP2Percentage = 0;

                            if (PMIUCheckedChannels != 0)
                            {
                                GNP2Percentage = (GNP2Count / PMIUCheckedChannels) * 100;
                                GNP2Score = (GNP2Percentage * GNP2Category.WeightCurrent.Value) / 100;
                            }

                            PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                            {
                                EvalScoreID = EvalID,
                                IrrigationBoundaryID = BoundryID,
                                CatID = GNP2Category.KPICategoryID,
                                SubCatID = GNP2Category.ID,
                                SubCatName = GNP2Category.Name,
                                SubCatDescription = GNP2Category.Description,
                                TotalPoints = GNP2Category.WeightCurrent,
                                Source = GNP2Category.Source,
                                Score = Math.Round(GNP2Score, 2),
                                PS = "P",
                                PSValue = Math.Round(GNP2Percentage, 2),
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                CreatedBy = _UserID,
                                ModifiedBy = _UserID
                            };

                            repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);
                        }
                    }

                    PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = GNP1Category.KPICategoryID,
                        CatName = GNP1Category.PE_KPICategories.Name,
                        CatDescription = GNP1Category.PE_KPICategories.Description,
                        FDTotalPoints = FieldTotalPoints,
                        FDWeightage = Math.Round(GNP1Score, 2),
                        PMIUTotalPoints = PMIUTotalPoints,
                        PMIUWeightage = Math.Round(GNP2Score, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repCategoryWeightage.Insert(mdlCategoryWeightage);

                    Score = Score + GNP1Score + GNP2Score;
                }

                #endregion

                #region Gauge Not Fixed Score Calculation

                if (lstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "GAUGING STATUS (GAUGE NOT FIXED)").Select(lsc => lsc.PE_KPICategories.PEInclude.Value).FirstOrDefault())
                {
                    double GNF1Score = 0;
                    double GNF2Score = 0;
                    double FieldTotalPoints = 0;
                    double PMIUTotalPoints = 0;

                    PE_KPISubCategories GNF1Category = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "GNF1").FirstOrDefault();

                    if (GNF1Category.PEInclude.Value)
                    {
                        FieldTotalPoints = GNF1Category.WeightCurrent.Value;

                        double GNF1Count = 0;
                        double GNF1Percentage = 0;

                        if (_Session.ToUpper().Trim() != "A")
                        {
                            GNF1Count = lstChannelTailStatus.Where(cts => cts.IsGaugeFixed == false && cts.TailStatus != null && cts.TailStatus != -1).Count();

                            if (AggregatedChannels != 0)
                            {
                                GNF1Percentage = (GNF1Count / AggregatedChannels) * 100;
                            }
                        }
                        else
                        {
                            GNF1Count = lstChannelTailStatus.Where(cts => cts.IsGaugeFixedM == false && cts.TailStatus != null && cts.TailStatus != -1).Count();
                            GNF1Count = GNF1Count + lstChannelTailStatus.Where(cts => cts.IsGaugeFixedE == false && cts.TailStatus != null && cts.TailStatus != -1).Count();

                            if (AggregatedChannels != 0)
                            {
                                GNF1Percentage = (GNF1Count / (AggregatedChannels * 2)) * 100;
                            }
                        }

                        GNF1Score = (GNF1Percentage * GNF1Category.WeightCurrent.Value) / 100;

                        PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                        {
                            EvalScoreID = EvalID,
                            IrrigationBoundaryID = BoundryID,
                            CatID = GNF1Category.KPICategoryID,
                            SubCatID = GNF1Category.ID,
                            SubCatName = GNF1Category.Name,
                            SubCatDescription = GNF1Category.Description,
                            TotalPoints = GNF1Category.WeightCurrent,
                            Source = GNF1Category.Source,
                            Score = Math.Round(GNF1Score, 2),
                            PS = "P",
                            PSValue = Math.Round(GNF1Percentage, 2),
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = _UserID,
                            ModifiedBy = _UserID
                        };

                        repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);
                    }

                    PE_KPISubCategories GNF2Category = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "GNF2").FirstOrDefault();

                    if (GNF2Category.PEInclude.Value)
                    {
                        PMIUTotalPoints = GNF2Category.WeightCurrent.Value;

                        double GNF2Count = lstChannelTailPMIUStatus.Where(ctps => ctps.IsGaugeFixed == false && ctps.TailStatus != null && ctps.TailStatus != -1).Count();
                        double GNF2Percentage = 0;

                        if (PMIUCheckedChannels != 0)
                        {
                            GNF2Percentage = (GNF2Count / PMIUCheckedChannels) * 100;
                            GNF2Score = (GNF2Percentage * GNF2Category.WeightCurrent.Value) / 100;
                        }

                        PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                        {
                            EvalScoreID = EvalID,
                            IrrigationBoundaryID = BoundryID,
                            CatID = GNF2Category.KPICategoryID,
                            SubCatID = GNF2Category.ID,
                            SubCatName = GNF2Category.Name,
                            SubCatDescription = GNF2Category.Description,
                            TotalPoints = GNF2Category.WeightCurrent,
                            Source = GNF2Category.Source,
                            Score = Math.Round(GNF2Score, 2),
                            PS = "P",
                            PSValue = Math.Round(GNF2Percentage, 2),
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = _UserID,
                            ModifiedBy = _UserID
                        };

                        repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);
                    }

                    PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = GNF1Category.KPICategoryID,
                        CatName = GNF1Category.PE_KPICategories.Name,
                        CatDescription = GNF1Category.PE_KPICategories.Description,
                        FDTotalPoints = FieldTotalPoints,
                        FDWeightage = Math.Round(GNF1Score, 2),
                        PMIUTotalPoints = PMIUTotalPoints,
                        PMIUWeightage = Math.Round(GNF2Score, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repCategoryWeightage.Insert(mdlCategoryWeightage);

                    Score = Score + GNF1Score + GNF2Score;
                }

                #endregion

                #region Rotational Violation

                if (lstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "ROTATIONAL VIOLATION").Select(lsc => lsc.PE_KPICategories.PEInclude.Value).FirstOrDefault())
                {
                    double FScore = 0;
                    double PMIUScore = 0;

                    PE_KPISubCategories Category = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "RV1").FirstOrDefault();

                    if (Category.PEInclude.Value)
                    {
                        long ViolationCount = (from rv in context.RP_RotationalViolation
                                               where rv.IrrigationLevelID == _IrrigationLevelID && rv.IrrigationBoundaryID == BoundryID &&
                                               DbFunctions.TruncateTime(rv.ReadingDate) <= DbFunctions.TruncateTime(_FromDate) &&
                                               DbFunctions.TruncateTime(rv.ReadingDate) <= DbFunctions.TruncateTime(_ToDate)
                                               select rv).Count();

                        if (ViolationCount >= 3)
                            FScore = Category.WeightCurrent.Value;
                        else
                            FScore = Category.BaseWeight.Value * ViolationCount;

                        PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                        {
                            EvalScoreID = EvalID,
                            IrrigationBoundaryID = BoundryID,
                            CatID = Category.KPICategoryID,
                            SubCatID = Category.ID,
                            SubCatName = Category.Name,
                            SubCatDescription = Category.Description,
                            TotalPoints = Category.WeightCurrent,
                            Source = Category.Source,
                            Score = Math.Round(FScore, 2),
                            PS = "S",
                            PSValue = (double?)ViolationCount,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = _UserID,
                            ModifiedBy = _UserID
                        };

                        repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);
                    }


                    PE_KPISubCategories Category2 = lstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == "RV2").FirstOrDefault();

                    if (Category2.PEInclude.Value)
                    {
                        List<long?> lstSecIDs = new List<long?>();
                        if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
                        {
                            lstSecIDs = (from sec in context.CO_Section
                                         where sec.CO_SubDivision.CO_Division.CO_Circle.ZoneID == BoundryID
                                         select (long?)sec.ID).ToList<long?>();
                        }
                        else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                        {
                            lstSecIDs = (from sec in context.CO_Section
                                         where sec.CO_SubDivision.CO_Division.CircleID == BoundryID
                                         select (long?)sec.ID).ToList<long?>();
                        }
                        else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                        {
                            lstSecIDs = (from sec in context.CO_Section
                                         where sec.CO_SubDivision.DivisionID == BoundryID
                                         select (long?)sec.ID).ToList<long?>();
                        }
                        else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                        {
                            lstSecIDs = (from sec in context.CO_Section
                                         where sec.SubDivID == BoundryID
                                         select (long?)sec.ID).ToList<long?>();
                        }
                        else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
                            lstSecIDs.Add(BoundryID);


                        long ViolationCount = (from rv in context.ET_RotationalViolation
                                               where lstSecIDs.Contains(rv.SectionID) &&
                                               DbFunctions.TruncateTime(rv.MobileEntryDatetime) <= DbFunctions.TruncateTime(_FromDate) &&
                                               DbFunctions.TruncateTime(rv.MobileEntryDatetime) <= DbFunctions.TruncateTime(_ToDate)
                                               select rv).Count();

                        if (ViolationCount >= 3)
                            PMIUScore = Category2.WeightCurrent.Value;
                        else
                            PMIUScore = Category2.BaseWeight.Value * ViolationCount;


                        PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                        {
                            EvalScoreID = EvalID,
                            IrrigationBoundaryID = BoundryID,
                            CatID = Category2.KPICategoryID,
                            SubCatID = Category2.ID,
                            SubCatName = Category2.Name,
                            SubCatDescription = Category2.Description,
                            TotalPoints = Category2.WeightCurrent,
                            Source = Category2.Source,
                            Score = Math.Round(PMIUScore, 2),
                            PS = "S",
                            PSValue = (double?)ViolationCount,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = _UserID,
                            ModifiedBy = _UserID
                        };

                        repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);
                    }

                    PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                    {
                        EvalScoreID = EvalID,
                        IrrigationBoundaryID = BoundryID,
                        CatID = Category.KPICategoryID,
                        CatName = Category.PE_KPICategories.Name,
                        CatDescription = Category.PE_KPICategories.Description,
                        FDTotalPoints = Category.WeightCurrent,
                        FDWeightage = Math.Round(FScore, 2),
                        PMIUTotalPoints = Category2.WeightCurrent,
                        PMIUWeightage = Math.Round(PMIUScore, 2),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };

                    repCategoryWeightage.Insert(mdlCategoryWeightage);

                    Score = Score + FScore + PMIUScore;
                }

                #endregion

                #region Saving Score Details

                PE_EvaluationScoresDetail mdlEvaluationScoresDetail = null;

                if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone || _IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
                {
                    mdlEvaluationScoresDetail = new PE_EvaluationScoresDetail
                    {
                        EvalScoreID = EvalID,
                        IrrigationLevelID = _IrrigationLevelID,
                        IrrigationBoundaryID = BoundryID,
                        TotalChannels = AllChannels,
                        AnalyzedGauges = AggregatedChannels,
                        ClosedGauges = ClosedChannelCount,
                        PMIUCheckedChannels = PMIUCheckedChannels,
                        ObtainedPoints = Math.Round(Score, 2),
                        Rank = 0,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };
                }
                else
                {
                    PE_DivisionComplexityLevel mdlDivisionComplexityLevel = null;

                    if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
                    {
                        mdlDivisionComplexityLevel = lstDivisionComplexityLevel.Where(dcl => dcl.DivisionID == BoundryID).FirstOrDefault();
                    }
                    else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                    {
                        CO_SubDivision mdlSubDivision = lstSubDivision.Where(sd => sd.ID == BoundryID).FirstOrDefault();

                        mdlDivisionComplexityLevel = lstDivisionComplexityLevel.Where(dcl => dcl.DivisionID == mdlSubDivision.DivisionID.Value).FirstOrDefault();
                    }
                    else
                    {
                        CO_Section mdlSection = lstSection.Where(s => s.ID == BoundryID).FirstOrDefault();

                        mdlDivisionComplexityLevel = lstDivisionComplexityLevel.Where(dcl => dcl.DivisionID == mdlSection.CO_SubDivision.DivisionID.Value).FirstOrDefault();
                    }

                    double ComplexityScore = Score;

                    if (mdlDivisionComplexityLevel != null && mdlDivisionComplexityLevel.PE_ComplexityFactor.ComplexityFactor.Value != 0)
                    {
                        ComplexityScore = ComplexityScore + Math.Abs((Score * mdlDivisionComplexityLevel.PE_ComplexityFactor.MultiplicationFactor.Value));
                    }

                    mdlEvaluationScoresDetail = new PE_EvaluationScoresDetail
                    {
                        EvalScoreID = EvalID,
                        IrrigationLevelID = _IrrigationLevelID,
                        IrrigationBoundaryID = BoundryID,
                        TotalChannels = AllChannels,
                        AnalyzedGauges = AggregatedChannels,
                        ClosedGauges = ClosedChannelCount,
                        PMIUCheckedChannels = PMIUCheckedChannels,
                        ObtainedPoints = Math.Round(Score, 2),
                        ComplexityLevel = (mdlDivisionComplexityLevel != null ? mdlDivisionComplexityLevel.PE_ComplexityFactor.ComplexityLevel : "Normal"),
                        ObtainedPointsComplexity = Math.Round(ComplexityScore, 2),
                        Rank = 0,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = _UserID,
                        ModifiedBy = _UserID
                    };
                }

                repScoreDetails.Insert(mdlEvaluationScoresDetail);

                #endregion
            }

            repSubCategoryWeightage.Save();
            repCategoryWeightage.Save();
            repScoreDetails.Save();
        }

        /// <summary>
        /// This function returns the Dry Tail score
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_LstChannelTailStatus"></param>
        /// <param name="_LstChannelTailPMIUStatus"></param>
        /// <param name="_LstSubCategories"></param>
        /// <param name="_AggregatedChannels"></param>
        /// <param name="_PMIUCheckedChannels"></param>
        /// <param name="_EvalScoreID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <returns>double</returns>
        private double GetDryTailScore(List<dynamic> _LstChannelTailStatus, List<dynamic> _LstChannelTailPMIUStatus, List<PE_KPISubCategories> _LstSubCategories, int _AggregatedChannels, int _PMIUCheckedChannels, long _EvalScoreID, long _UserID, long _BoundryID)
        {
            PE_KPICategories mdlKPICategory = _LstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "DRY TAIL" && lsc.PE_KPICategories.PEInclude.Value == true).Select(lsc => lsc.PE_KPICategories).FirstOrDefault();

            if (mdlKPICategory != null)
            {
                double DryTail1Score = 0;
                double DryTail2Score = 0;
                double DryTail3Score = 0;
                double DryTail4Score = 0;
                double DryTail5Score = 0;
                double DryTail6Score = 0;
                double DryTail7Score = 0;
                double DryTail8Score = 0;
                double DryTail9Score = 0;
                double DryTail10Score = 0;
                double DryTail11Score = 0;
                double DryTail12Score = 0;
                double FieldTotalPoints = 0;
                double PMIUTotalPoints = 0;

                #region Dry Tail Field Calculation

                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 1", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail1Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 2", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail2Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 3", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail3Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 4", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail4Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 5", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail5Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 6", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail6Score);

                #endregion

                double FieldDryTailScore = DryTail1Score + DryTail2Score + DryTail3Score + DryTail4Score + DryTail5Score + DryTail6Score;

                #region Dry Tail PMIU Calculation

                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 7", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail7Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 8", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail8Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 9", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail9Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 10", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail10Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 11", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail11Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "DRY TAIL 12", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out DryTail12Score);

                #endregion

                double PMIUDryTailScore = DryTail7Score + DryTail8Score + DryTail9Score + DryTail10Score + DryTail11Score + DryTail12Score;

                PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                {
                    EvalScoreID = _EvalScoreID,
                    IrrigationBoundaryID = _BoundryID,
                    CatID = mdlKPICategory.ID,
                    CatName = mdlKPICategory.Name,
                    CatDescription = mdlKPICategory.Description,
                    FDTotalPoints = FieldTotalPoints,
                    FDWeightage = Math.Round(FieldDryTailScore, 2),
                    PMIUTotalPoints = PMIUTotalPoints,
                    PMIUWeightage = Math.Round(PMIUDryTailScore, 2),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                repCategoryWeightage.Insert(mdlCategoryWeightage);

                return FieldDryTailScore + PMIUDryTailScore;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function returns the Short Tail score
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_LstChannelTailStatus"></param>
        /// <param name="_LstChannelTailPMIUStatus"></param>
        /// <param name="_LstSubCategories"></param>
        /// <param name="_AggregatedChannels"></param>
        /// <param name="_PMIUCheckedChannels"></param>
        /// <param name="_EvalScoreID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <returns>double</returns>
        private double GetShortTailScore(List<dynamic> _LstChannelTailStatus, List<dynamic> _LstChannelTailPMIUStatus, List<PE_KPISubCategories> _LstSubCategories, int _AggregatedChannels, int _PMIUCheckedChannels, long _EvalScoreID, long _UserID, long _BoundryID)
        {
            PE_KPICategories mdlKPICategory = _LstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "SHORT TAIL" && lsc.PE_KPICategories.PEInclude.Value == true).Select(lsc => lsc.PE_KPICategories).FirstOrDefault();

            if (mdlKPICategory != null)
            {
                double ShortTail1Score = 0;
                double ShortTail2Score = 0;
                double ShortTail3Score = 0;
                double ShortTail4Score = 0;
                double ShortTail5Score = 0;
                double ShortTail6Score = 0;
                double ShortTail7Score = 0;
                double ShortTail8Score = 0;
                double ShortTail9Score = 0;
                double ShortTail10Score = 0;
                double ShortTail11Score = 0;
                double ShortTail12Score = 0;
                double ShortTail13Score = 0;
                double ShortTail14Score = 0;
                double ShortTail15Score = 0;
                double ShortTail16Score = 0;
                double FieldTotalPoints = 0;
                double PMIUTotalPoints = 0;

                #region Short Tail Field Calculation

                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 1", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail1Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 2", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail2Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 3", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail3Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 4", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail4Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 5", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail5Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 6", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail6Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 7", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail7Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 8", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail8Score);

                #endregion

                double FieldShortTailScore = ShortTail1Score + ShortTail2Score + ShortTail3Score + ShortTail4Score +
                    ShortTail5Score + ShortTail6Score + ShortTail7Score + ShortTail8Score;

                #region Short Tail PMIU Calculation

                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 9", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail9Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 10", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail10Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 11", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail11Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 12", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail12Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 13", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail13Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 14", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail14Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 15", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail15Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "SHORT TAIL 16", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ShortTail16Score);

                #endregion

                double PMIUShortTailScore = ShortTail9Score + ShortTail10Score + ShortTail11Score + ShortTail12Score +
                    ShortTail13Score + ShortTail14Score + ShortTail15Score + ShortTail16Score;

                PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                {
                    EvalScoreID = _EvalScoreID,
                    IrrigationBoundaryID = _BoundryID,
                    CatID = mdlKPICategory.ID,
                    CatName = mdlKPICategory.Name,
                    CatDescription = mdlKPICategory.Description,
                    FDTotalPoints = FieldTotalPoints,
                    FDWeightage = Math.Round(FieldShortTailScore, 2),
                    PMIUTotalPoints = PMIUTotalPoints,
                    PMIUWeightage = Math.Round(PMIUShortTailScore, 2),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                repCategoryWeightage.Insert(mdlCategoryWeightage);

                return FieldShortTailScore + PMIUShortTailScore;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function returns the Authorized Tail score
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_LstChannelTailStatus"></param>
        /// <param name="_LstChannelTailPMIUStatus"></param>
        /// <param name="_LstSubCategories"></param>
        /// <param name="_AggregatedChannels"></param>
        /// <param name="_PMIUCheckedChannels"></param>
        /// <param name="_EvalScoreID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <returns>double</returns>
        private double GetAuthorizedTailScore(List<dynamic> _LstChannelTailStatus, List<dynamic> _LstChannelTailPMIUStatus, List<PE_KPISubCategories> _LstSubCategories, int _AggregatedChannels, int _PMIUCheckedChannels, long _EvalScoreID, long _UserID, long _BoundryID)
        {
            PE_KPICategories mdlKPICategory = _LstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "AUTHORIZED TAIL" && lsc.PE_KPICategories.PEInclude.Value == true).Select(lsc => lsc.PE_KPICategories).FirstOrDefault();

            if (mdlKPICategory != null)
            {
                double AT1Score = 0;
                double AT2Score = 0;

                double FieldTotalPoints = SaveTailSubCategoryScore(_LstSubCategories, "AT1", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out AT1Score);

                double PMIUTotalPoints = SaveTailSubCategoryScore(_LstSubCategories, "AT2", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out AT2Score);

                PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                {
                    EvalScoreID = _EvalScoreID,
                    IrrigationBoundaryID = _BoundryID,
                    CatID = mdlKPICategory.ID,
                    CatName = mdlKPICategory.Name,
                    CatDescription = mdlKPICategory.Description,
                    FDTotalPoints = FieldTotalPoints,
                    FDWeightage = Math.Round(AT1Score, 2),
                    PMIUTotalPoints = PMIUTotalPoints,
                    PMIUWeightage = Math.Round(AT2Score, 2),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                repCategoryWeightage.Insert(mdlCategoryWeightage);

                return AT1Score + AT2Score;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function returns the Excessive Tail score
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_LstChannelTailStatus"></param>
        /// <param name="_LstChannelTailPMIUStatus"></param>
        /// <param name="_LstSubCategories"></param>
        /// <param name="_AggregatedChannels"></param>
        /// <param name="_PMIUCheckedChannels"></param>
        /// <param name="_EvalScoreID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <returns>double</returns>
        private double GetExcessiveTailScore(List<dynamic> _LstChannelTailStatus, List<dynamic> _LstChannelTailPMIUStatus, List<PE_KPISubCategories> _LstSubCategories, int _AggregatedChannels, int _PMIUCheckedChannels, long _EvalScoreID, long _UserID, long _BoundryID)
        {
            PE_KPICategories mdlKPICategory = _LstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "EXCESSIVE TAIL" && lsc.PE_KPICategories.PEInclude.Value == true).Select(lsc => lsc.PE_KPICategories).FirstOrDefault();

            if (mdlKPICategory != null)
            {
                double ExcessiveTail1Score = 0;
                double ExcessiveTail2Score = 0;
                double ExcessiveTail3Score = 0;
                double ExcessiveTail4Score = 0;
                double ExcessiveTail5Score = 0;
                double ExcessiveTail6Score = 0;
                double ExcessiveTail7Score = 0;
                double ExcessiveTail8Score = 0;
                double ExcessiveTail9Score = 0;
                double ExcessiveTail10Score = 0;
                double ExcessiveTail11Score = 0;
                double ExcessiveTail12Score = 0;
                double ExcessiveTail13Score = 0;
                double ExcessiveTail14Score = 0;
                double ExcessiveTail15Score = 0;
                double ExcessiveTail16Score = 0;
                double FieldTotalPoints = 0;
                double PMIUTotalPoints = 0;

                #region Excessive Tail Field Calculation

                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 1", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail1Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 2", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail2Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 3", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail3Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 4", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail4Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 5", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail5Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 6", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail6Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 7", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail7Score);
                FieldTotalPoints = FieldTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 8", _LstChannelTailStatus, _AggregatedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail8Score);

                #endregion

                double FieldExcessiveTailScore = ExcessiveTail1Score + ExcessiveTail2Score + ExcessiveTail3Score + ExcessiveTail4Score +
                    ExcessiveTail5Score + ExcessiveTail6Score + ExcessiveTail7Score + ExcessiveTail8Score;

                #region Excessive Tail PMIU Calculation

                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 9", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail9Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 10", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail10Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 11", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail11Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 12", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail12Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 13", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail13Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 14", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail14Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 15", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail15Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveTailSubCategoryScore(_LstSubCategories, "EXCESSIVE TAIL 16", _LstChannelTailPMIUStatus, _PMIUCheckedChannels, _EvalScoreID, _UserID, _BoundryID, out ExcessiveTail16Score);

                #endregion

                double PMIUExcessiveTailScore = ExcessiveTail9Score + ExcessiveTail10Score + ExcessiveTail11Score + ExcessiveTail12Score +
                    ExcessiveTail13Score + ExcessiveTail14Score + ExcessiveTail15Score + ExcessiveTail16Score;

                PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                {
                    EvalScoreID = _EvalScoreID,
                    IrrigationBoundaryID = _BoundryID,
                    CatID = mdlKPICategory.ID,
                    CatName = mdlKPICategory.Name,
                    CatDescription = mdlKPICategory.Description,
                    FDTotalPoints = FieldTotalPoints,
                    FDWeightage = Math.Round(FieldExcessiveTailScore, 2),
                    PMIUTotalPoints = PMIUTotalPoints,
                    PMIUWeightage = Math.Round(PMIUExcessiveTailScore, 2),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                repCategoryWeightage.Insert(mdlCategoryWeightage);

                return FieldExcessiveTailScore + PMIUExcessiveTailScore;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function returns the Head Gauge Difference score
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_LstHeadDifferenceStatus"></param>
        /// <param name="_LstSubCategories"></param>
        /// <param name="_EvalScoreID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <returns>double</returns>
        private double GetDifferenceHeadGaugeScore(List<dynamic> _LstHeadDifferenceStatus, List<PE_KPISubCategories> _LstSubCategories, long _EvalScoreID, long _UserID, long _BoundryID)
        {
            PE_KPICategories mdlKPICategory = _LstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "DIFFERENCE OF HEAD GAUGES CHECKED BY PMIU STAFF WITH GAUGES CONVEYED BY FIELD DIVISION" && lsc.PE_KPICategories.PEInclude.Value == true).Select(lsc => lsc.PE_KPICategories).FirstOrDefault();

            if (mdlKPICategory != null)
            {
                double DHG1Score = 0;
                double DHG2Score = 0;
                double DHG3Score = 0;
                double DHG4Score = 0;
                double PMIUTotalPoints = 0;

                int TotalComparisions = _LstHeadDifferenceStatus.Count();

                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DHG1", _LstHeadDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DHG1Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DHG2", _LstHeadDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DHG2Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DHG3", _LstHeadDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DHG3Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DHG4", _LstHeadDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DHG4Score);

                double DHGScore = DHG1Score + DHG2Score + DHG3Score + DHG4Score;

                PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                {
                    EvalScoreID = _EvalScoreID,
                    IrrigationBoundaryID = _BoundryID,
                    CatID = mdlKPICategory.ID,
                    CatName = mdlKPICategory.Name,
                    CatDescription = mdlKPICategory.Description,
                    PMIUTotalPoints = PMIUTotalPoints,
                    PMIUWeightage = Math.Round(DHGScore, 2),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                repCategoryWeightage.Insert(mdlCategoryWeightage);

                return DHGScore;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function returns the Tail Gauge Difference score
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_LstTailDifferenceStatus"></param>
        /// <param name="_LstSubCategories"></param>
        /// <param name="_EvalScoreID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <returns>double</returns>
        private double GetDifferenceTailGaugeScore(List<dynamic> _LstTailDifferenceStatus, List<PE_KPISubCategories> _LstSubCategories, long _EvalScoreID, long _UserID, long _BoundryID)
        {
            PE_KPICategories mdlKPICategory = _LstSubCategories.Where(lsc => lsc.PE_KPICategories.Name.ToUpper().Trim() == "DIFFERENCE OF TAIL GAUGES CHECKED BY PMIU STAFF WITH GAUGES CONVEYED BY FIELD DIVISION" && lsc.PE_KPICategories.PEInclude.Value == true).Select(lsc => lsc.PE_KPICategories).FirstOrDefault();

            if (mdlKPICategory != null)
            {
                double DTG1Score = 0;
                double DTG2Score = 0;
                double DTG3Score = 0;
                double DTG4Score = 0;
                double DTG5Score = 0;
                double PMIUTotalPoints = 0;

                int TotalComparisions = _LstTailDifferenceStatus.Count();

                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DTG1", _LstTailDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DTG1Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DTG2", _LstTailDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DTG2Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DTG3", _LstTailDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DTG3Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DTG4", _LstTailDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DTG4Score);
                PMIUTotalPoints = PMIUTotalPoints + SaveGaugeDifferenceScore(_LstSubCategories, "DTG5", _LstTailDifferenceStatus, TotalComparisions, _EvalScoreID, _UserID, _BoundryID, out DTG5Score);

                double DTGScore = DTG1Score + DTG2Score + DTG3Score + DTG4Score + DTG5Score;

                PE_CategoryWeightage mdlCategoryWeightage = new PE_CategoryWeightage
                {
                    EvalScoreID = _EvalScoreID,
                    IrrigationBoundaryID = _BoundryID,
                    CatID = mdlKPICategory.ID,
                    CatName = mdlKPICategory.Name,
                    CatDescription = mdlKPICategory.Description,
                    PMIUTotalPoints = PMIUTotalPoints,
                    PMIUWeightage = Math.Round(DTGScore, 2),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                repCategoryWeightage.Insert(mdlCategoryWeightage);

                return DTGScore;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function gets the Irrigation Level ID of the report
        /// Created on 28-10-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>long</returns>
        public long GetIrrigationLevelByReportID(long _ReportID)
        {
            PE_EvaluationReports mdlEvaluationReport = context.PE_EvaluationReports.Where(er => er.ID == _ReportID).FirstOrDefault();

            long IrrigationLevelID = mdlEvaluationReport.IrrigationLevelID.Value;

            if (mdlEvaluationReport.EvaluationType == "S")
            {
                IrrigationLevelID++;
            }

            return IrrigationLevelID;
        }

        /// <summary>
        /// This function generates the Irrigation Level Path to be shown on screen
        /// Created on 28-10-2016
        /// </summary>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <param name="_IrrigationLevelPath"></param>
        /// <returns>string</returns>
        public string GetIrrigationLevelPath(long? _IrrigationLevelID, long? _IrrigationBoundaryID, string _IrrigationLevelPath)
        {
            string IrrigationLevelPath = _IrrigationLevelPath;
            string LocalIrrigationLevelPath = "";

            if (_IrrigationLevelID == Convert.ToInt64(Constants.IrrigationLevelID.Zone))
            {
                LocalIrrigationLevelPath = (from z in context.CO_Zone
                                            where z.ID == _IrrigationBoundaryID
                                            select new { Name = z.Name }).ToList().Select(x => new { x.Name }).FirstOrDefault().Name;

                LocalIrrigationLevelPath = LocalIrrigationLevelPath + " (Zone) -> ";

                IrrigationLevelPath = LocalIrrigationLevelPath + IrrigationLevelPath;

                return IrrigationLevelPath;
            }
            else if (_IrrigationLevelID == Convert.ToInt64(Constants.IrrigationLevelID.Circle))
            {
                LocalIrrigationLevelPath = (from c in context.CO_Circle
                                            where c.ID == _IrrigationBoundaryID
                                            select new { Name = c.Name }).ToList().Select(x => new { x.Name }).FirstOrDefault().Name;

                _IrrigationBoundaryID = (from c in context.CO_Circle
                                         where c.ID == _IrrigationBoundaryID
                                         select new { ZoneID = c.ZoneID }).ToList().Select(x => new { x.ZoneID }).FirstOrDefault().ZoneID;

                LocalIrrigationLevelPath = LocalIrrigationLevelPath + " (Circle) -> ";

                IrrigationLevelPath = LocalIrrigationLevelPath + IrrigationLevelPath;

                IrrigationLevelPath = GetIrrigationLevelPath(Convert.ToInt64(Constants.IrrigationLevelID.Zone), _IrrigationBoundaryID, IrrigationLevelPath);
            }
            else if (_IrrigationLevelID == Convert.ToInt64(Constants.IrrigationLevelID.Division))
            {
                LocalIrrigationLevelPath = (from c in context.CO_Division
                                            where c.ID == _IrrigationBoundaryID
                                            select new { Name = c.Name }).ToList().Select(x => new { x.Name }).FirstOrDefault().Name;

                _IrrigationBoundaryID = (from c in context.CO_Division
                                         where c.ID == _IrrigationBoundaryID
                                         select new { CircleID = c.CircleID }).ToList().Select(x => new { x.CircleID }).FirstOrDefault().CircleID;

                LocalIrrigationLevelPath = LocalIrrigationLevelPath + " (Division) -> ";

                IrrigationLevelPath = LocalIrrigationLevelPath + IrrigationLevelPath;

                IrrigationLevelPath = GetIrrigationLevelPath(Convert.ToInt64(Constants.IrrigationLevelID.Circle), _IrrigationBoundaryID, IrrigationLevelPath);
            }
            else if (_IrrigationLevelID == Convert.ToInt64(Constants.IrrigationLevelID.SubDivision))
            {
                LocalIrrigationLevelPath = (from c in context.CO_SubDivision
                                            where c.ID == _IrrigationBoundaryID
                                            select new { Name = c.Name }).ToList().Select(x => new { x.Name }).FirstOrDefault().Name;

                _IrrigationBoundaryID = (from c in context.CO_SubDivision
                                         where c.ID == _IrrigationBoundaryID
                                         select new { DivisionID = c.DivisionID }).ToList().Select(x => new { x.DivisionID }).FirstOrDefault().DivisionID;

                LocalIrrigationLevelPath = LocalIrrigationLevelPath + " (Sub Division) -> ";

                IrrigationLevelPath = LocalIrrigationLevelPath + IrrigationLevelPath;

                IrrigationLevelPath = GetIrrigationLevelPath(Convert.ToInt64(Constants.IrrigationLevelID.Division), _IrrigationBoundaryID, IrrigationLevelPath);
            }
            else if (_IrrigationLevelID == Convert.ToInt64(Constants.IrrigationLevelID.Section))
            {
                LocalIrrigationLevelPath = (from c in context.CO_Section
                                            where c.ID == _IrrigationBoundaryID
                                            select new { Name = c.Name }).ToList().Select(x => new { x.Name }).FirstOrDefault().Name;

                _IrrigationBoundaryID = (from c in context.CO_Section
                                         where c.ID == _IrrigationBoundaryID
                                         select new { SubDivID = c.SubDivID }).ToList().Select(x => new { x.SubDivID }).FirstOrDefault().SubDivID;

                LocalIrrigationLevelPath = LocalIrrigationLevelPath + " (Section)";

                IrrigationLevelPath = LocalIrrigationLevelPath + IrrigationLevelPath;

                IrrigationLevelPath = GetIrrigationLevelPath(Convert.ToInt64(Constants.IrrigationLevelID.SubDivision), _IrrigationBoundaryID, IrrigationLevelPath);
            }

            return IrrigationLevelPath;
        }

        /// <summary>
        /// This method return Detail of Evaluation Scores Detail
        /// Created on 28-10-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <param name="_CatID"></param>
        /// <returns>dynamic</returns>
        public dynamic GetEvaluationScoresDetail(long _ReportID, long _IrrigationBoundaryID, long _CatID)
        {
            long IrrigationLevelID = GetIrrigationLevelByReportID(_ReportID);

            string EvaluationScoresDetailPath = GetIrrigationLevelPath(IrrigationLevelID, _IrrigationBoundaryID, "");

            if ((long)Constants.IrrigationLevelID.Section != IrrigationLevelID)
            {
                EvaluationScoresDetailPath = EvaluationScoresDetailPath.Remove(EvaluationScoresDetailPath.Length - 4);
            }

            List<dynamic> lstEvaluationDetails = GetPEScoreDetailByReportID(_ReportID);

            dynamic mdlEvaluationDetail = lstEvaluationDetails.Where(lsd => lsd.BoundryID == _IrrigationBoundaryID).FirstOrDefault();

            int AnalyzedGauges = mdlEvaluationDetail.AnalyzedGauges;
            int ClosedGauges = mdlEvaluationDetail.ClosedGauges;
            int AggregatedGauges = mdlEvaluationDetail.AggregatedGauges;
            int PMIUTotalChannels = mdlEvaluationDetail.PMIUTotalChannels;
            double ObtainedPointsTotal = 0;

            if ((long)Constants.IrrigationLevelID.Zone == IrrigationLevelID || (long)Constants.IrrigationLevelID.Circle == IrrigationLevelID)
            {
                ObtainedPointsTotal = mdlEvaluationDetail.Score;
            }
            else
            {
                ObtainedPointsTotal = mdlEvaluationDetail.ComplexityScore;
            }

            dynamic EvaluationScoresDetail = null;

            if (_CatID == 0)
            {
                EvaluationScoresDetail = (from er in context.PE_EvaluationReports
                                          join il in context.UA_IrrigationLevel on er.IrrigationLevelID equals il.ID
                                          where er.ID == _ReportID
                                          select new
                                          {
                                              PerformanceEvaluationLevel = il.Name,
                                              FromDate = er.FromDate,
                                              ToDate = er.ToDate,
                                              Session = (er.Session == "M" ? "Morning" : (er.Session == "E" ? "Evening" : "Average")),
                                              AnalyzedGauges = AnalyzedGauges,
                                              ClosedGauges = ClosedGauges,
                                              AggregatedGauges = AggregatedGauges,
                                              PMIUTotalChannels = PMIUTotalChannels,
                                              ObtainedPointsTotal = ObtainedPointsTotal,
                                              EvaluationScoresDetailPath = EvaluationScoresDetailPath
                                          }).FirstOrDefault();
            }
            else
            {
                List<dynamic> lstCategoryDetails = GetCategoryWeightage(_ReportID, _IrrigationBoundaryID);

                dynamic mdlCategoryDetail = lstCategoryDetails.Where(lcd => lcd.CatID == _CatID).FirstOrDefault();

                string CatName = mdlCategoryDetail.CatName;
                double WeightageInKPICategory = mdlCategoryDetail.TotalWeightage;

                EvaluationScoresDetail = (from er in context.PE_EvaluationReports
                                          join il in context.UA_IrrigationLevel on er.IrrigationLevelID equals il.ID
                                          where er.ID == _ReportID
                                          select new
                                          {
                                              PerformanceEvaluationLevel = il.Name,
                                              FromDate = er.FromDate,
                                              ToDate = er.ToDate,
                                              Session = (er.Session == "M" ? "Morning" : (er.Session == "E" ? "Evening" : "Average")),
                                              AnalyzedGauges = AnalyzedGauges,
                                              ClosedGauges = ClosedGauges,
                                              AggregatedGauges = AggregatedGauges,
                                              PMIUTotalChannels = PMIUTotalChannels,
                                              ObtainedPointsTotal = ObtainedPointsTotal,
                                              CatName = CatName,
                                              WeightageInKPICategory = WeightageInKPICategory,
                                              EvaluationScoresDetailPath = EvaluationScoresDetailPath
                                          }).FirstOrDefault();
            }

            return EvaluationScoresDetail;
        }

        /// <summary>
        /// This function get data for the Category Detail grid
        /// Created on 03-01-2017
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetCategoryWeightage(long _ReportID, long _IrrigationBoundaryID)
        {
            List<dynamic> lstScreenData = null;

            PE_EvaluationReports mdlEvaluationReports = context.PE_EvaluationReports.Where(er => er.ID == _ReportID).FirstOrDefault();

            long IrrigationLevelID = mdlEvaluationReports.IrrigationLevelID.Value;

            if (mdlEvaluationReports.EvaluationType == "S")
            {
                IrrigationLevelID++;
            }

            List<long> lstEvaluationScoreIDs = context.PE_EvaluationScores.Where(es => es.IrrigationLevelID == IrrigationLevelID && es.Session == mdlEvaluationReports.Session &&
                                                            es.FromDate >= mdlEvaluationReports.FromDate && es.ToDate <= mdlEvaluationReports.ToDate).OrderByDescending(es => es.ToDate).Select(es => es.ID).ToList();

            int FortnightlyCount = 1;

            if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.M.ToString())
            {
                FortnightlyCount = 2;
            }
            else if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.S.ToString())
            {
                FortnightlyCount = 12;
            }

            if (FortnightlyCount != lstEvaluationScoreIDs.Count)
            {
                return lstScreenData;
            }

            lstScreenData = (from cw in context.PE_CategoryWeightage
                             where lstEvaluationScoreIDs.Contains(cw.EvalScoreID.Value) && cw.IrrigationBoundaryID == _IrrigationBoundaryID
                             group cw by cw.CatID into cwg
                             select new
                             {
                                 CatID = cwg.FirstOrDefault().CatID,
                                 CatName = cwg.FirstOrDefault().CatName,
                                 CatDescription = ((from cw in context.PE_CategoryWeightage
                                                    where lstEvaluationScoreIDs.Contains(cw.EvalScoreID.Value) && cw.IrrigationBoundaryID == _IrrigationBoundaryID
                                                    && cw.CatID == cwg.FirstOrDefault().CatID && cw.CatDescription == cwg.FirstOrDefault().CatDescription
                                                    select cw).Count() == cwg.Count() ? cwg.FirstOrDefault().CatDescription : "Description varies"),
                                 FDTotalPoints = cwg.Average(cw => cw.FDTotalPoints),
                                 FDWeightage = cwg.Average(cw => cw.FDWeightage),
                                 PMIUTotalPoints = cwg.Average(cw => cw.PMIUTotalPoints),
                                 PMIUWeightage = cwg.Average(cw => cw.PMIUWeightage)
                             }).ToList<dynamic>()
                             .Select(lsd => new
                             {
                                 CatID = lsd.CatID,
                                 CatName = lsd.CatName,
                                 CatDescription = lsd.CatDescription,
                                 FDTotalPoints = (lsd.FDTotalPoints == null ? null : Math.Round(lsd.FDTotalPoints, 2)),
                                 FDWeightage = (lsd.FDWeightage == null ? null : Math.Round(lsd.FDWeightage, 2)),
                                 PMIUTotalPoints = (lsd.PMIUTotalPoints == null ? null : Math.Round(lsd.PMIUTotalPoints, 2)),
                                 PMIUWeightage = (lsd.PMIUWeightage == null ? null : Math.Round(lsd.PMIUWeightage, 2)),
                                 TotalWeightage = (lsd.FDWeightage == null ? 0 : Math.Round(lsd.FDWeightage, 2)) + (lsd.PMIUWeightage == null ? 0 : Math.Round(lsd.PMIUWeightage, 2))
                             }).OrderBy(lsd => lsd.CatID).ToList<dynamic>();

            return lstScreenData;
        }

        /// <summary>
        /// This function get data for the Sub Category Detail grid
        /// Created on 04-01-2017
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <param name="_CatID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetSubCategoryWeightage(long _ReportID, long _IrrigationBoundaryID, long _CatID)
        {
            List<dynamic> lstScreenData = null;

            PE_EvaluationReports mdlEvaluationReports = context.PE_EvaluationReports.Where(er => er.ID == _ReportID).FirstOrDefault();

            long IrrigationLevelID = mdlEvaluationReports.IrrigationLevelID.Value;

            if (mdlEvaluationReports.EvaluationType == "S")
            {
                IrrigationLevelID++;
            }

            List<long> lstEvaluationScoreIDs = context.PE_EvaluationScores.Where(es => es.IrrigationLevelID == IrrigationLevelID && es.Session == mdlEvaluationReports.Session &&
                                                            es.FromDate >= mdlEvaluationReports.FromDate && es.ToDate <= mdlEvaluationReports.ToDate).OrderByDescending(es => es.ToDate).Select(es => es.ID).ToList();

            int FortnightlyCount = 1;

            if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.M.ToString())
            {
                FortnightlyCount = 2;
            }
            else if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.S.ToString())
            {
                FortnightlyCount = 12;
            }

            if (FortnightlyCount != lstEvaluationScoreIDs.Count)
            {
                return lstScreenData;
            }

            lstScreenData = (from scw in context.PE_SubCategoryWeightage
                             where lstEvaluationScoreIDs.Contains(scw.EvalScoreID.Value) && scw.IrrigationBoundaryID == _IrrigationBoundaryID
                             && scw.CatID == _CatID
                             group scw by scw.SubCatID into scwg
                             select new
                             {
                                 SubCatID = scwg.FirstOrDefault().SubCatID,
                                 SubCatName = scwg.FirstOrDefault().SubCatName,
                                 SubCatDescription = ((from scw in context.PE_SubCategoryWeightage
                                                       where lstEvaluationScoreIDs.Contains(scw.EvalScoreID.Value) && scw.IrrigationBoundaryID == _IrrigationBoundaryID
                                                       && scw.CatID == _CatID && scw.SubCatID == scwg.FirstOrDefault().SubCatID && scw.SubCatDescription == scwg.FirstOrDefault().SubCatDescription
                                                       select scw).Count() == scwg.Count() ? scwg.FirstOrDefault().SubCatDescription : "Description varies"),
                                 TotalPoints = scwg.Average(scw => scw.TotalPoints),
                                 Weightage = scwg.Average(scw => scw.Score),
                                 Source = scwg.FirstOrDefault().Source
                             }).ToList<dynamic>()
                             .Select(lsd => new
                             {
                                 SubCatID = lsd.SubCatID,
                                 SubCatName = lsd.SubCatName,
                                 SubCatDescription = lsd.SubCatDescription,
                                 TotalPoints = Math.Round(lsd.TotalPoints, 2),
                                 Weightage = Math.Round(lsd.Weightage, 2),
                                 Source = lsd.Source
                             }).OrderBy(lsd => lsd.SubCatID).ToList<dynamic>();

            return lstScreenData;

        }

        /// <summary>
        /// This function deletes the Evaluation report based on EvalScoreID
        /// Created On 04-11-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>bool</returns>
        public bool DeleteEvaluationReport(long _ReportID)
        {
            context.PE_EvaluationReports.Remove(context.PE_EvaluationReports.Where(er => er.ID == _ReportID).FirstOrDefault());

            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// This function saves the Tail SubCategory Score to database
        /// Created On 05-01-2017
        /// </summary>
        /// <param name="_LstSubCategories"></param>
        /// <param name="_SubCategoryName"></param>
        /// <param name="_LstTailStatus"></param>
        /// <param name="_ChannelCount"></param>
        /// <param name="_EvalScoreID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <param name="_TailScore"></param>
        /// <returns>double</returns>
        private double SaveTailSubCategoryScore(List<PE_KPISubCategories> _LstSubCategories, string _SubCategoryName, List<dynamic> _LstTailStatus, int _ChannelCount, long _EvalScoreID, long _UserID, long _BoundryID, out double _TailScore)
        {
            PE_KPISubCategories mdlKPISubCategory = _LstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == _SubCategoryName).FirstOrDefault();

            _TailScore = 0;

            if (mdlKPISubCategory.PEInclude.Value)
            {
                double TailStatus = _LstTailStatus.Where(lts => lts.TailStatus == mdlKPISubCategory.ID).Count();
                double TailPercentage = 0;

                if (_ChannelCount != 0)
                {
                    TailPercentage = (TailStatus / _ChannelCount) * 100;
                    _TailScore = (TailPercentage * mdlKPISubCategory.WeightCurrent.Value) / 100;
                }

                PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                {
                    EvalScoreID = _EvalScoreID,
                    IrrigationBoundaryID = _BoundryID,
                    CatID = mdlKPISubCategory.KPICategoryID,
                    SubCatID = mdlKPISubCategory.ID,
                    SubCatName = mdlKPISubCategory.Name,
                    SubCatDescription = mdlKPISubCategory.Description,
                    TotalPoints = mdlKPISubCategory.WeightCurrent,
                    Source = mdlKPISubCategory.Source,
                    Score = Math.Round(_TailScore, 2),
                    PS = "P",
                    PSValue = Math.Round(TailPercentage, 2),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);

                return mdlKPISubCategory.WeightCurrent.Value;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function saves the Head Gauge Difference Score to database
        /// Created On 05-01-2017
        /// </summary>
        /// <param name="_LstSubCategories"></param>
        /// <param name="_SubCategoryName"></param>
        /// <param name="_LstDifferenceStatus"></param>
        /// <param name="_TotalComparisions"></param>
        /// <param name="_EvalScoreID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <param name="_Score"></param>
        /// <returns>double</returns>
        private double SaveGaugeDifferenceScore(List<PE_KPISubCategories> _LstSubCategories, string _SubCategoryName, List<dynamic> _LstDifferenceStatus, int _TotalComparisions, long _EvalScoreID, long _UserID, long _BoundryID, out double _Score)
        {
            PE_KPISubCategories mdlKPISubCategory = _LstSubCategories.Where(lsc => lsc.Name.ToUpper().Trim() == _SubCategoryName).FirstOrDefault();

            _Score = 0;

            if (mdlKPISubCategory.PEInclude.Value)
            {
                double Status = _LstDifferenceStatus.Where(lds => lds.Status == mdlKPISubCategory.ID).Count();
                double Percentage = 0;

                if (_TotalComparisions != 0)
                {
                    Percentage = (Status / _TotalComparisions) * 100;
                    _Score = (Percentage * mdlKPISubCategory.WeightCurrent.Value) / 100;
                }

                PE_SubCategoryWeightage mdlSubCategoryWeightage = new PE_SubCategoryWeightage
                {
                    EvalScoreID = _EvalScoreID,
                    IrrigationBoundaryID = _BoundryID,
                    CatID = mdlKPISubCategory.KPICategoryID,
                    SubCatID = mdlKPISubCategory.ID,
                    SubCatName = mdlKPISubCategory.Name,
                    SubCatDescription = mdlKPISubCategory.Description,
                    TotalPoints = mdlKPISubCategory.WeightCurrent,
                    Source = mdlKPISubCategory.Source,
                    Score = Math.Round(_Score, 2),
                    PS = "P",
                    PSValue = Math.Round(Percentage, 2),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                repSubCategoryWeightage.Insert(mdlSubCategoryWeightage);

                return mdlKPISubCategory.WeightCurrent.Value;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This function gets the General Evaluation Score
        /// Created On 24-11-2016
        /// </summary>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_Session"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <param name="_UserID"></param>
        /// <param name="_ReportSpan"></param>
        /// <param name="_ReportID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetGeneralEvaluationScore(long _IrrigationLevelID, string _Session, DateTime? _FromDate, DateTime? _ToDate, long _UserID, string _ReportSpan, out long _ReportID)
        {
            List<dynamic> lstScreenData = null;
            _ReportID = 0;

            List<long> lstEvaluationScoreIDs = context.PE_EvaluationScores.Where(es => es.IrrigationLevelID == _IrrigationLevelID && es.Session == _Session &&
                                               es.FromDate >= _FromDate && es.ToDate <= _ToDate).OrderByDescending(es => es.ToDate).Select(es => es.ID).ToList();

            int FortnightlyCount = 1;

            if (_ReportSpan == Constants.ReportSpan.M.ToString())
            {
                FortnightlyCount = 2;
            }
            else if (_ReportSpan == Constants.ReportSpan.S.ToString())
            {
                FortnightlyCount = 12;
            }

            if (FortnightlyCount != lstEvaluationScoreIDs.Count)
            {
                return lstScreenData;
            }

            lstScreenData = GetEvaluationScoreData(lstEvaluationScoreIDs, _IrrigationLevelID, "G");

            PE_EvaluationReports mdlEvaluationReports = context.PE_EvaluationReports.Where(er => er.IrrigationLevelID == _IrrigationLevelID && er.Session == _Session &&
                er.ReportSpan == _ReportSpan && er.FromDate == _FromDate && er.ToDate == _ToDate && er.IrrigationBoundaryID == null).FirstOrDefault();

            if (mdlEvaluationReports == null)
            {
                mdlEvaluationReports = new PE_EvaluationReports
                {
                    EvalScoreID = lstScreenData.ElementAt(0).EvalID,
                    EvaluationType = "G",
                    IrrigationLevelID = _IrrigationLevelID,
                    Session = _Session,
                    FromDate = _FromDate,
                    ToDate = _ToDate,
                    ReportSpan = _ReportSpan,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                context.PE_EvaluationReports.Add(mdlEvaluationReports);
                context.SaveChanges();
            }

            _ReportID = mdlEvaluationReports.ID;

            return lstScreenData;
        }

        /// <summary>
        /// This function gets the Specific Evaluation Score
        /// Created On 24-11-2016
        /// </summary>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <param name="_Session"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <param name="_UserID"></param>
        /// <param name="_ReportSpan"></param>
        /// <param name="_ReportID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetSpecificEvaluationScore(long _IrrigationLevelID, long _IrrigationBoundaryID, string _Session, DateTime? _FromDate, DateTime? _ToDate, long _UserID, string _ReportSpan, out long _ReportID)
        {
            List<dynamic> lstScreenData = null;
            _ReportID = 0;

            long ScoreIrrigationLevelID = _IrrigationLevelID + 1;

            List<long> lstEvaluationScoreIDs = context.PE_EvaluationScores.Where(es => es.IrrigationLevelID == ScoreIrrigationLevelID && es.Session == _Session &&
                                               es.FromDate >= _FromDate && es.ToDate <= _ToDate).OrderByDescending(es => es.ToDate).Select(es => es.ID).ToList();

            int FortnightlyCount = 1;

            if (_ReportSpan == Constants.ReportSpan.M.ToString())
            {
                FortnightlyCount = 2;
            }
            else if (_ReportSpan == Constants.ReportSpan.S.ToString())
            {
                FortnightlyCount = 12;
            }

            if (FortnightlyCount != lstEvaluationScoreIDs.Count)
            {
                return lstScreenData;
            }

            lstScreenData = GetEvaluationScoreData(lstEvaluationScoreIDs, ScoreIrrigationLevelID, "S", _IrrigationBoundaryID);

            PE_EvaluationReports mdlEvaluationReports = context.PE_EvaluationReports.Where(er => er.IrrigationLevelID == _IrrigationLevelID && er.Session == _Session &&
                    er.ReportSpan == _ReportSpan && er.FromDate == _FromDate && er.ToDate == _ToDate && er.IrrigationBoundaryID == _IrrigationBoundaryID).FirstOrDefault();

            if (mdlEvaluationReports == null)
            {
                mdlEvaluationReports = new PE_EvaluationReports
                {
                    EvalScoreID = lstScreenData.ElementAt(0).EvalID,
                    EvaluationType = "S",
                    IrrigationLevelID = _IrrigationLevelID,
                    IrrigationBoundaryID = _IrrigationBoundaryID,
                    Session = _Session,
                    FromDate = _FromDate,
                    ToDate = _ToDate,
                    ReportSpan = _ReportSpan,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                context.PE_EvaluationReports.Add(mdlEvaluationReports);
                context.SaveChanges();
            }

            _ReportID = mdlEvaluationReports.ID;

            return lstScreenData;
        }

        /// <summary>
        /// This function returns the PE Score based of EvalID
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetPEScoreDetailByReportID(long _ReportID)
        {
            List<dynamic> lstScreenData = null;

            PE_EvaluationReports mdlEvaluationReports = context.PE_EvaluationReports.Where(er => er.ID == _ReportID).FirstOrDefault();

            long IrrigationLevelID = mdlEvaluationReports.IrrigationLevelID.Value;

            if (mdlEvaluationReports.EvaluationType == "S")
            {
                IrrigationLevelID++;
            }

            List<long> lstEvaluationScoreIDs = context.PE_EvaluationScores.Where(es => es.IrrigationLevelID == IrrigationLevelID && es.Session == mdlEvaluationReports.Session &&
                                               es.FromDate >= mdlEvaluationReports.FromDate && es.ToDate <= mdlEvaluationReports.ToDate).OrderByDescending(es => es.ToDate).Select(es => es.ID).ToList();

            int FortnightlyCount = 1;

            if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.M.ToString())
            {
                FortnightlyCount = 2;
            }
            else if (mdlEvaluationReports.ReportSpan == Constants.ReportSpan.S.ToString())
            {
                FortnightlyCount = 12;
            }

            if (FortnightlyCount != lstEvaluationScoreIDs.Count)
            {
                return lstScreenData;
            }

            lstScreenData = GetEvaluationScoreData(lstEvaluationScoreIDs, IrrigationLevelID, mdlEvaluationReports.EvaluationType, mdlEvaluationReports.IrrigationBoundaryID);

            return lstScreenData;
        }

        /// <summary>
        /// This function returns the rank
        /// Created On 02-12-2016
        /// </summary>
        /// <param name="_Score"></param>
        /// <returns>int</returns>
        private int GetRank(double _Score)
        {
            if (NetTotal != null && NetTotal != _Score)
            {
                Rank++;
            }

            NetTotal = _Score;

            return Rank;
        }

        /// <summary>
        /// This function returns Score Details for the grid on web page
        /// Created On 26-12-2016
        /// </summary>
        /// <param name="_LstEvaluationScoreIDs"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_EvaluationType"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <returns>List<dynamic></returns>
        private List<dynamic> GetEvaluationScoreData(List<long> _LstEvaluationScoreIDs, long _IrrigationLevelID, string _EvaluationType, long? _IrrigationBoundaryID = null)
        {
            List<dynamic> lstEvaluationScoresDetail = (from esd in context.PE_EvaluationScoresDetail
                                                       where _LstEvaluationScoreIDs.Contains(esd.EvalScoreID.Value)
                                                       group esd by esd.IrrigationBoundaryID into esdt
                                                       select new
                                                       {
                                                           EvalScoreID = esdt.FirstOrDefault().EvalScoreID,
                                                           TotalChannels = esdt.FirstOrDefault().TotalChannels,
                                                           AnalyzedGauges = esdt.Sum(esd => esd.AnalyzedGauges),
                                                           ClosedGauges = esdt.Sum(esd => esd.ClosedGauges),
                                                           PMIUChannelTotal = esdt.Sum(esd => esd.PMIUCheckedChannels),
                                                           IrrigationBoundaryID = esdt.FirstOrDefault().IrrigationBoundaryID,
                                                           ObtainedPoints = esdt.Average(esd => esd.ObtainedPoints),
                                                           ComplexityLevel = (((from esd in context.PE_EvaluationScoresDetail
                                                                                where _LstEvaluationScoreIDs.Contains(esd.EvalScoreID.Value) && esd.IrrigationBoundaryID == esdt.FirstOrDefault().IrrigationBoundaryID
                                                                                && esd.ComplexityLevel == esdt.FirstOrDefault().ComplexityLevel
                                                                                select esd).Count()) == esdt.Count() ? esdt.FirstOrDefault().ComplexityLevel : "Level varies"),
                                                           ObtainedPointsComplexity = esdt.Average(esd => esd.ObtainedPointsComplexity)
                                                       }).ToList<dynamic>()
                                                       .Select(lesd => new
                                                       {
                                                           EvalScoreID = lesd.EvalScoreID,
                                                           TotalChannels = lesd.TotalChannels,
                                                           AnalyzedGauges = lesd.AnalyzedGauges,
                                                           ClosedGauges = lesd.ClosedGauges,
                                                           PMIUChannelTotal = lesd.PMIUChannelTotal,
                                                           IrrigationBoundaryID = lesd.IrrigationBoundaryID,
                                                           ObtainedPoints = Math.Round(lesd.ObtainedPoints, 2),
                                                           ComplexityLevel = lesd.ComplexityLevel,
                                                           ObtainedPointsComplexity = (lesd.ObtainedPointsComplexity == null ? lesd.ObtainedPointsComplexity : Math.Round(lesd.ObtainedPointsComplexity, 2))
                                                       }).ToList<dynamic>();

            List<dynamic> lstScreenData = null;

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
            {
                lstScreenData = (from esd in lstEvaluationScoresDetail
                                 join z in context.CO_Zone on esd.IrrigationBoundaryID equals z.ID
                                 select new
                                 {
                                     EvalID = esd.EvalScoreID,
                                     BoundryID = esd.IrrigationBoundaryID,
                                     BoundryName = z.Name,
                                     TotalChannels = esd.TotalChannels,
                                     AnalyzedGauges = esd.AnalyzedGauges,
                                     ClosedGauges = esd.ClosedGauges,
                                     AggregatedGauges = esd.AnalyzedGauges - esd.ClosedGauges,
                                     PMIUTotalChannels = esd.PMIUChannelTotal,
                                     Score = esd.ObtainedPoints
                                 }).OrderByDescending(lbd => lbd.Score).ThenBy(lbd => lbd.BoundryName).ToList<dynamic>()
                                  .Select(lbd => new
                                  {
                                      EvalID = lbd.EvalID,
                                      BoundryID = lbd.BoundryID,
                                      BoundryName = lbd.BoundryName,
                                      TotalChannels = lbd.TotalChannels,
                                      AnalyzedGauges = lbd.AnalyzedGauges,
                                      ClosedGauges = lbd.ClosedGauges,
                                      AggregatedGauges = lbd.AggregatedGauges,
                                      PMIUTotalChannels = lbd.PMIUTotalChannels,
                                      Score = lbd.Score,
                                      Rating = GetRank(lbd.Score)
                                  }).ToList<dynamic>();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                lstScreenData = (from esd in lstEvaluationScoresDetail
                                 join c in context.CO_Circle on esd.IrrigationBoundaryID equals c.ID
                                 join z in context.CO_Zone on c.ZoneID equals z.ID
                                 where (_EvaluationType == "G" || z.ID == _IrrigationBoundaryID)
                                 select new
                                 {
                                     EvalID = esd.EvalScoreID,
                                     BoundryID = esd.IrrigationBoundaryID,
                                     BoundryName = c.Name,
                                     ZoneName = z.Name,
                                     TotalChannels = esd.TotalChannels,
                                     AnalyzedGauges = esd.AnalyzedGauges,
                                     ClosedGauges = esd.ClosedGauges,
                                     AggregatedGauges = esd.AnalyzedGauges - esd.ClosedGauges,
                                     PMIUTotalChannels = esd.PMIUChannelTotal,
                                     Score = esd.ObtainedPoints
                                 }).OrderByDescending(lbd => lbd.Score).ThenBy(lbd => lbd.BoundryName).ToList<dynamic>()
                                  .Select(lbd => new
                                  {
                                      EvalID = lbd.EvalID,
                                      BoundryID = lbd.BoundryID,
                                      BoundryName = lbd.BoundryName,
                                      ZoneName = lbd.ZoneName,
                                      TotalChannels = lbd.TotalChannels,
                                      AnalyzedGauges = lbd.AnalyzedGauges,
                                      ClosedGauges = lbd.ClosedGauges,
                                      AggregatedGauges = lbd.AggregatedGauges,
                                      PMIUTotalChannels = lbd.PMIUTotalChannels,
                                      Score = lbd.Score,
                                      Rating = GetRank(lbd.Score)
                                  }).ToList<dynamic>();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                lstScreenData = (from esd in lstEvaluationScoresDetail
                                 join d in context.CO_Division on esd.IrrigationBoundaryID equals d.ID
                                 join c in context.CO_Circle on d.CircleID equals c.ID
                                 join z in context.CO_Zone on c.ZoneID equals z.ID
                                 where (_EvaluationType == "G" || c.ID == _IrrigationBoundaryID)
                                 select new
                                 {
                                     EvalID = esd.EvalScoreID,
                                     BoundryID = esd.IrrigationBoundaryID,
                                     BoundryName = d.Name,
                                     CircleName = c.Name,
                                     ZoneName = z.Name,
                                     TotalChannels = esd.TotalChannels,
                                     AnalyzedGauges = esd.AnalyzedGauges,
                                     ClosedGauges = esd.ClosedGauges,
                                     AggregatedGauges = esd.AnalyzedGauges - esd.ClosedGauges,
                                     PMIUTotalChannels = esd.PMIUChannelTotal,
                                     Score = esd.ObtainedPoints,
                                     ComplexityLevel = esd.ComplexityLevel,
                                     ComplexityScore = esd.ObtainedPointsComplexity
                                 }).OrderByDescending(lbd => lbd.ComplexityScore).ThenBy(lbd => lbd.BoundryName).ToList<dynamic>()
                                  .Select(lbd => new
                                  {
                                      EvalID = lbd.EvalID,
                                      BoundryID = lbd.BoundryID,
                                      BoundryName = lbd.BoundryName,
                                      CircleName = lbd.CircleName,
                                      ZoneName = lbd.ZoneName,
                                      TotalChannels = lbd.TotalChannels,
                                      AnalyzedGauges = lbd.AnalyzedGauges,
                                      ClosedGauges = lbd.ClosedGauges,
                                      AggregatedGauges = lbd.AggregatedGauges,
                                      PMIUTotalChannels = lbd.PMIUTotalChannels,
                                      Score = lbd.Score,
                                      ComplexityLevel = lbd.ComplexityLevel,
                                      ComplexityScore = lbd.ComplexityScore,
                                      Rating = GetRank(lbd.ComplexityScore)
                                  }).ToList<dynamic>();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                lstScreenData = (from esd in lstEvaluationScoresDetail
                                 join sd in context.CO_SubDivision on esd.IrrigationBoundaryID equals sd.ID
                                 join d in context.CO_Division on sd.DivisionID equals d.ID
                                 join c in context.CO_Circle on d.CircleID equals c.ID
                                 join z in context.CO_Zone on c.ZoneID equals z.ID
                                 where (_EvaluationType == "G" || d.ID == _IrrigationBoundaryID)
                                 select new
                                 {
                                     EvalID = esd.EvalScoreID,
                                     BoundryID = esd.IrrigationBoundaryID,
                                     BoundryName = sd.Name,
                                     DivisionName = d.Name,
                                     CircleName = c.Name,
                                     ZoneName = z.Name,
                                     TotalChannels = esd.TotalChannels,
                                     AnalyzedGauges = esd.AnalyzedGauges,
                                     ClosedGauges = esd.ClosedGauges,
                                     AggregatedGauges = esd.AnalyzedGauges - esd.ClosedGauges,
                                     PMIUTotalChannels = esd.PMIUChannelTotal,
                                     Score = esd.ObtainedPoints,
                                     ComplexityLevel = esd.ComplexityLevel,
                                     ComplexityScore = esd.ObtainedPointsComplexity
                                 }).OrderByDescending(lbd => lbd.ComplexityScore).ThenBy(lbd => lbd.BoundryName).ToList<dynamic>()
                                  .Select(lbd => new
                                  {
                                      EvalID = lbd.EvalID,
                                      BoundryID = lbd.BoundryID,
                                      BoundryName = lbd.BoundryName,
                                      DivisionName = lbd.DivisionName,
                                      CircleName = lbd.CircleName,
                                      ZoneName = lbd.ZoneName,
                                      TotalChannels = lbd.TotalChannels,
                                      AnalyzedGauges = lbd.AnalyzedGauges,
                                      ClosedGauges = lbd.ClosedGauges,
                                      AggregatedGauges = lbd.AggregatedGauges,
                                      PMIUTotalChannels = lbd.PMIUTotalChannels,
                                      Score = lbd.Score,
                                      ComplexityLevel = lbd.ComplexityLevel,
                                      ComplexityScore = lbd.ComplexityScore,
                                      Rating = GetRank(lbd.ComplexityScore)
                                  }).ToList<dynamic>();
            }
            else
            {
                lstScreenData = (from esd in lstEvaluationScoresDetail
                                 join s in context.CO_Section on esd.IrrigationBoundaryID equals s.ID
                                 join sd in context.CO_SubDivision on s.SubDivID equals sd.ID
                                 join d in context.CO_Division on sd.DivisionID equals d.ID
                                 join c in context.CO_Circle on d.CircleID equals c.ID
                                 join z in context.CO_Zone on c.ZoneID equals z.ID
                                 where (_EvaluationType == "G" || sd.ID == _IrrigationBoundaryID)
                                 select new
                                 {
                                     EvalID = esd.EvalScoreID,
                                     BoundryID = esd.IrrigationBoundaryID,
                                     BoundryName = s.Name,
                                     SubDivisionName = sd.Name,
                                     DivisionName = d.Name,
                                     CircleName = c.Name,
                                     ZoneName = z.Name,
                                     TotalChannels = esd.TotalChannels,
                                     AnalyzedGauges = esd.AnalyzedGauges,
                                     ClosedGauges = esd.ClosedGauges,
                                     AggregatedGauges = esd.AnalyzedGauges - esd.ClosedGauges,
                                     PMIUTotalChannels = esd.PMIUChannelTotal,
                                     Score = esd.ObtainedPoints,
                                     ComplexityLevel = esd.ComplexityLevel,
                                     ComplexityScore = esd.ObtainedPointsComplexity
                                 }).OrderByDescending(lbd => lbd.ComplexityScore).ThenBy(lbd => lbd.BoundryName).ToList<dynamic>()
                                  .Select(lbd => new
                                  {
                                      EvalID = lbd.EvalID,
                                      BoundryID = lbd.BoundryID,
                                      BoundryName = lbd.BoundryName,
                                      SubDivisionName = lbd.SubDivisionName,
                                      DivisionName = lbd.DivisionName,
                                      CircleName = lbd.CircleName,
                                      ZoneName = lbd.ZoneName,
                                      TotalChannels = lbd.TotalChannels,
                                      AnalyzedGauges = lbd.AnalyzedGauges,
                                      ClosedGauges = lbd.ClosedGauges,
                                      AggregatedGauges = lbd.AggregatedGauges,
                                      PMIUTotalChannels = lbd.PMIUTotalChannels,
                                      Score = lbd.Score,
                                      ComplexityLevel = lbd.ComplexityLevel,
                                      ComplexityScore = lbd.ComplexityScore,
                                      Rating = GetRank(lbd.ComplexityScore)
                                  }).ToList<dynamic>();
            }

            return lstScreenData;
        }
    }
}