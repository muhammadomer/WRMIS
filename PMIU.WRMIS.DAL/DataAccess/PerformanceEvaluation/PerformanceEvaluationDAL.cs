using PMIU.WRMIS.DAL.Repositories.PerformanceEvaluation;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using System.Data.Entity.Core.Objects;
using System.Runtime.CompilerServices;

namespace PMIU.WRMIS.DAL.DataAccess.PerformanceEvaluation
{
    public class PerformanceEvaluationDAL : BaseDAL
    {
        /// <summary>
        /// This function returns Complexity levels of all divisions in a particular domain
        /// Created On 22-09-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <param name="_DomainID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionalComplexityLevel(long _CircleID, long _DomainID)
        {
            return db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetDivisionalComplexityLevel(_CircleID, _DomainID);
        }

        /// <summary>
        /// This function returns Division Complexity Level based on ID
        /// Created On 23-09-2016
        /// </summary>
        /// <param name="_DivisionComplexityLevelID"></param>
        /// <returns>PE_DivisionComplexityLevel</returns>
        public PE_DivisionComplexityLevel GetDivisionComplexityLevelByID(long _DivisionComplexityLevelID)
        {
            return db.Repository<PE_DivisionComplexityLevel>().FindById(_DivisionComplexityLevelID);
        }

        /// <summary>
        /// This function adds new Division Complexity Level.
        /// Created By 23-09-2016
        /// </summary>
        /// <param name="_DivisionComplexityLevel"></param>
        /// <returns>bool</returns>
        public bool AddDivisionComplexityLevel(PE_DivisionComplexityLevel _DivisionComplexityLevel)
        {
            db.Repository<PE_DivisionComplexityLevel>().Insert(_DivisionComplexityLevel);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a Division Complexity Level.
        /// Created By 23-09-2016
        /// </summary>
        /// <param name="_DivisionComplexityLevel"></param>
        /// <returns>bool</returns>
        public bool UpdateDivisionComplexityLevel(PE_DivisionComplexityLevel _DivisionComplexityLevel)
        {
            db.Repository<PE_DivisionComplexityLevel>().Update(_DivisionComplexityLevel);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function returns all channel types valid for performance evaluation.
        /// Created On 26-09-2016
        /// </summary>
        /// <returns>List<CO_ChannelType></returns>
        public List<CO_ChannelType> GetChannelTypesForPE()
        {
            return db.Repository<CO_ChannelType>().GetAll().Where(ct => ct.ID == (long)Constants.ChannelType.DistributaryMajor ||
                ct.ID == (long)Constants.ChannelType.DistributaryMinor || ct.ID == (long)Constants.ChannelType.DistributarySubMinor).ToList();
        }

        /// <summary>
        /// This function returns all channels that are parent to some other channel.
        /// Created On 26-09-2016
        /// </summary>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetParentChannels()
        {
            return db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetParentChannels();
        }



        /// <summary>
        /// This function returns the Division data along with exclusion bit for performance evaluation.
        /// Created By 07-Sep-2017
        /// </summary>
        /// /// <param name="_CircleID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionsReadyToExclude(long _ZoneID, long _CircleID, bool _IsExcludedDivisions)
        {
            return db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetDivisionsReadyToExclude(_ZoneID, _CircleID, _IsExcludedDivisions);
        }

        /// <summary>
        /// This function returns the channel data along with exclusion bit for performance evaluation.
        /// Created By 27-09-2016
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
            return db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetExcludedChannels(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _CommandNameID,
                _ChannelTypeID, _FlowTypeID, _ParentChannelID, _ChannelName, _IMISCode, _GetExcludedChannels, _GetZeroAuthorizedTail);
        }
        public PE_DivisionExcluded GetChannelExcludedByDivisionID(long _DivisionID)
        {
            return db.Repository<PE_DivisionExcluded>().GetAll().Where(ce => ce.DivisionID == _DivisionID).FirstOrDefault();
        }
        /// <summary>
        /// This function returns Channel Excluded data based on Channel ID
        /// Created On 29-09-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>PE_ChannelExcluded</returns>
        public PE_ChannelExcluded GetChannelExcludedByChannelID(long _ChannelID)
        {
            return db.Repository<PE_ChannelExcluded>().GetAll().Where(ce => ce.ChannelID == _ChannelID).FirstOrDefault();
        }

        /// <summary>
        /// This function adds new Channel Excluded data.
        /// Created On 29-09-2016
        /// </summary>
        /// <param name="_ChannelExcluded"></param>
        /// <returns>bool</returns>
        public bool AddChannelExcluded(PE_ChannelExcluded _ChannelExcluded)
        {
            db.Repository<PE_ChannelExcluded>().Insert(_ChannelExcluded);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates Channel Excluded data.
        /// Created On 29-09-2016
        /// </summary>
        /// <param name="_ChannelExcluded"></param>
        /// <returns>bool</returns>
        public bool UpdateChannelExcluded(PE_ChannelExcluded _ChannelExcluded)
        {
            db.Repository<PE_ChannelExcluded>().Update(_ChannelExcluded);
            db.Save();

            return true;
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
            db.ExtRepositoryFor<PerformanceEvaluationRepository>().CalculatePEScore(_IrrigationLevelID, _Session, _FromDate, _ToDate, _UserID);
        }

        /// <summary>
        /// This function returns the PE Score based of EvalID
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetPEScoreDetailByReportID(long _ReportID)
        {
            return db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetPEScoreDetailByReportID(_ReportID);
        }

        /// <summary>
        /// This method return Detail of Evaluation Scores Detail
        /// Created on 28-Oct-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <param name="_IrrigationBoundaryID"></param>
        /// <param name="_CatID"></param>
        /// <returns>dynamic</returns>
        public dynamic GetEvaluationScoresDetail(long _ReportID, long _IrrigationBoundaryID, long _CatID)
        {
            return this.db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetEvaluationScoresDetail(_ReportID, _IrrigationBoundaryID, _CatID);
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
            return this.db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetCategoryWeightage(_ReportID, _IrrigationBoundaryID);
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
            return this.db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetSubCategoryWeightage(_ReportID, _IrrigationBoundaryID, _CatID);
        }

        /// <summary>
        /// This function check if Evaluation with a particular Name exists
        /// Created on 02-11-2016
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns>bool</returns>
        public bool GetEvaluationByName(string _Name)
        {
            return db.Repository<PE_EvaluationReports>().GetAll().Any(er => er.ReportName.ToUpper().Trim() == _Name.ToUpper().Trim());
        }

        /// <summary>
        /// This function gets Report with a particular ID
        /// Created on 02-11-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>PE_EvaluationScores</returns>
        public PE_EvaluationReports GetReportByID(long _ReportID)
        {
            return db.Repository<PE_EvaluationReports>().FindById(_ReportID);
        }

        /// <summary>
        /// This function updates a particular Report
        /// Created on 02-11-2016
        /// </summary>
        /// <param name="_EvaluationReports"></param>
        /// <returns>bool</returns>
        public bool UpdateReport(PE_EvaluationReports _EvaluationReports)
        {
            db.Repository<PE_EvaluationReports>().Update(_EvaluationReports);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function fetches the Report records according to the selection criteria
        /// Created On 03-11-2016
        /// </summary>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <param name="_ReportName"></param>        
        /// <returns>List<PE_EvaluationReports></returns>
        public List<PE_EvaluationReports> GetEvaluationReports(DateTime? _FromDate, DateTime? _ToDate, string _ReportName)
        {
            return db.Repository<PE_EvaluationReports>().GetAll().Where(er => er.ReportName != null && (_FromDate <= EntityFunctions.TruncateTime(er.ModifiedDate) || _FromDate == null) &&
                (_ToDate >= EntityFunctions.TruncateTime(er.ModifiedDate) || _ToDate == null) && (_ReportName == er.ReportName || _ReportName == string.Empty)).ToList();
        }

        public List<object> GetEvaluationReportsByDateRange(DateTime? _FromDate, DateTime? _ToDate)
        {
            List<object> PElist = (from item in db.Repository<PE_EvaluationReports>().GetAll() where (item.ModifiedDate <= _ToDate || _ToDate == null) && (item.ModifiedDate >= _FromDate || _FromDate == null) select new { ID = item.ID, Name = item.ReportName == null ? "-No Name-" : item.ReportName }).ToList<object>();
            //List<object> PLElist = (from item in db.Repository<PE_EvaluationReports>().GetAll() where (item.ModifiedDate <= _ToDate || _ToDate == null) && (item.ModifiedDate >= _FromDate || _FromDate == null)  select new { ID = item.ID, Name = item.ReportName}).ToList<object>();
            //List<object> FPEList = new List<object>();

            //foreach (var item in PElist)
            //{
            //    if (item.GetType().GetProperty("Name").GetValue(item)==string.Empty)
            //    {
            //        FPEList.Add(new {ID=item.GetType().GetProperty("ID").GetValue(item),Name="-No Name-"});
            //    }
            //    else
            //    {
            //        FPEList.Add(item);
            //    }
            //}
            return PElist;
            //Where(er => (_FromDate <= EntityFunctions.TruncateTime(er.ModifiedDate) || _FromDate == null) &&
            //(_ToDate >= EntityFunctions.TruncateTime(er.ModifiedDate) || _ToDate == null)).ToList();
        }

        /// <summary>
        /// This function deletes the Evaluation report based on EvalScoreID
        /// Created On 04-11-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>bool</returns>
        public bool DeleteEvaluationReport(long _ReportID)
        {
            return db.ExtRepositoryFor<PerformanceEvaluationRepository>().DeleteEvaluationReport(_ReportID);
        }

        /// <summary>
        /// This function returns the Tail Status of Channels for Field staff for the scheduled job
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_StartDate"></param>
        /// <param name="_EndDate"></param>                
        public void GetFieldTailStatus_Job(DateTime _StartDate, DateTime _EndDate)
        {
            db.ExecuteNonQuery("PE_GetFieldData", _StartDate, _EndDate);
        }

        /// <summary>
        /// This function returns the Tail Status of the selected Channels from PMIU staff for scheduled job
        /// Created On 16-11-2016
        /// </summary>
        /// <param name="_StartDate"></param>
        /// <param name="_EndDate"></param>        
        public void GetPMIUTailStatus_Job(DateTime _StartDate, DateTime _EndDate)
        {
            db.ExecuteNonQuery("PE_GetPMIUData", _StartDate, _EndDate);
        }

        /// <summary>
        /// This function returns the Tail Difference for the provided Channels for the scheduled job
        /// Created On 16-11-2016
        /// </summary>
        /// <param name="_StartDate"></param>
        /// <param name="_EndDate"></param>                
        public void GetTailDifference_Job(DateTime _StartDate, DateTime _EndDate)
        {
            db.ExecuteNonQuery("PE_GetTailDifference", _StartDate, _EndDate);
        }

        /// <summary>
        /// This function returns the Head Difference for the provided Channels for the scheduled job
        /// Created On 16-11-2016
        /// </summary>
        /// <param name="_StartDate"></param>
        /// <param name="_EndDate"></param>                
        public void GetHeadDifference_Job(DateTime _StartDate, DateTime _EndDate)
        {
            db.ExecuteNonQuery("PE_GetHeadDifference", _StartDate, _EndDate);
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
            return db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetGeneralEvaluationScore(_IrrigationLevelID, _Session, _FromDate, _ToDate, _UserID, _ReportSpan, out _ReportID);
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
            return db.ExtRepositoryFor<PerformanceEvaluationRepository>().GetSpecificEvaluationScore(_IrrigationLevelID, _IrrigationBoundaryID, _Session, _FromDate, _ToDate, _UserID, _ReportSpan, out _ReportID);
        }

        /// <summary>
        /// This function checks if Evalaution for a particular date range exists.
        /// Created On 05-01-2017
        /// </summary>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns>bool</returns>
        public bool IsEvaluationExists(DateTime _FromDate, DateTime _ToDate)
        {
            return db.Repository<PE_EvaluationScores>().GetAll().Any(es => es.FromDate == _FromDate && es.ToDate == _ToDate);
        }
        public bool AddDivisionExcluded(PE_DivisionExcluded _DivisionExcluded)
        {
            db.Repository<PE_DivisionExcluded>().Insert(_DivisionExcluded);
            db.Save();
            return true;
        }
        public bool UpdateDivisionExcluded(PE_DivisionExcluded _DivisionExcluded)
        {
            db.Repository<PE_DivisionExcluded>().Update(_DivisionExcluded);
            db.Save();
            return true;
        }

        public bool IsTailAnalysisExists(long ChannelID, long ChannelRD)
        {
            return db.Repository<CO_ChannelParentFeeder>().GetAll().Any(es => es.ParrentFeederID == ChannelID && es.StructureTypeID == (int)Constants.StructureType.Channel && es.ParrentFeederRDS == ChannelRD);
        }
    }
}