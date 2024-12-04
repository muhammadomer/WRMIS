using PMIU.WRMIS.DAL.DataAccess.PerformanceEvaluation;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.PerformanceEvaluation
{
    public class PerformanceEvaluationBLL : BaseBLL
    {
        PerformanceEvaluationDAL dalPerformanceEvaluation = new PerformanceEvaluationDAL();

        /// <summary>
        /// This function returns Complexity levels of all divisions in a particular domain
        /// Created On 22-09-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <param name="_DomainID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionalComplexityLevel(long _CircleID, long _DomainID)
        {
            return dalPerformanceEvaluation.GetDivisionalComplexityLevel(_CircleID, _DomainID);
        }

        /// <summary>
        /// This function returns Division Complexity Level based on ID
        /// Created On 23-09-2016
        /// </summary>
        /// <param name="_DivisionComplexityLevelID"></param>
        /// <returns>PE_DivisionComplexityLevel</returns>
        public PE_DivisionComplexityLevel GetDivisionComplexityLevelByID(long _DivisionComplexityLevelID)
        {
            return dalPerformanceEvaluation.GetDivisionComplexityLevelByID(_DivisionComplexityLevelID);
        }

        /// <summary>
        /// This function adds new Division Complexity Level.
        /// Created By 23-09-2016
        /// </summary>
        /// <param name="_DivisionComplexityLevel"></param>
        /// <returns>bool</returns>
        public bool AddDivisionComplexityLevel(PE_DivisionComplexityLevel _DivisionComplexityLevel)
        {
            return dalPerformanceEvaluation.AddDivisionComplexityLevel(_DivisionComplexityLevel);
        }

        /// <summary>
        /// This function updates a Division Complexity Level.
        /// Created By 23-09-2016
        /// </summary>
        /// <param name="_DivisionComplexityLevel"></param>
        /// <returns>bool</returns>
        public bool UpdateDivisionComplexityLevel(PE_DivisionComplexityLevel _DivisionComplexityLevel)
        {
            return dalPerformanceEvaluation.UpdateDivisionComplexityLevel(_DivisionComplexityLevel);
        }

        /// <summary>
        /// This function returns all channel types valid for performance evaluation.
        /// Created On 26-09-2016
        /// </summary>
        /// <returns>List<CO_ChannelType></returns>
        public List<CO_ChannelType> GetChannelTypesForPE()
        {
            return dalPerformanceEvaluation.GetChannelTypesForPE();
        }

        /// <summary>
        /// This function returns all channels that are parent to some other channel.
        /// Created On 26-09-2016
        /// </summary>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetParentChannels()
        {
            return dalPerformanceEvaluation.GetParentChannels();
        }

        /// <summary>
        /// This function returns the Division data along with exclusion bit for performance evaluation.
        /// Created By 07-Sep-2017
        /// </summary>
        /// /// <param name="_CircleID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionsReadyToExclude(long ZoneID, long _CircleID, bool _IsExcludedDivisions)
        {
            return dalPerformanceEvaluation.GetDivisionsReadyToExclude(ZoneID, _CircleID, _IsExcludedDivisions);
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
            return dalPerformanceEvaluation.GetExcludedChannels(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _CommandNameID,
                _ChannelTypeID, _FlowTypeID, _ParentChannelID, _ChannelName, _IMISCode, _GetExcludedChannels, _GetZeroAuthorizedTail);
        }
        public PE_DivisionExcluded GetChannelExcludedByDivisionID(long _DivisionID)
        {
            return dalPerformanceEvaluation.GetChannelExcludedByDivisionID(_DivisionID);
        }
        /// <summary>
        /// This function returns Channel Excluded data based on ID
        /// Created On 29-09-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>PE_ChannelExcluded</returns>
        public PE_ChannelExcluded GetChannelExcludedByChannelID(long _ChannelID)
        {
            return dalPerformanceEvaluation.GetChannelExcludedByChannelID(_ChannelID);
        }
        public bool AddDivisionExcluded(PE_DivisionExcluded _DivisionExcluded)
        {
            return dalPerformanceEvaluation.AddDivisionExcluded(_DivisionExcluded);
        }
        /// <summary>
        /// This function adds new Channel Excluded data.
        /// Created By 29-09-2016
        /// </summary>
        /// <param name="_ChannelExcluded"></param>
        /// <returns>bool</returns>
        public bool AddChannelExcluded(PE_ChannelExcluded _ChannelExcluded)
        {
            return dalPerformanceEvaluation.AddChannelExcluded(_ChannelExcluded);
        }
        public bool UpdateDivisionExcluded(PE_DivisionExcluded _DivisionExcluded)
        {
            return dalPerformanceEvaluation.UpdateDivisionExcluded(_DivisionExcluded);
        }
        /// <summary>
        /// This function updates Channel Excluded data.
        /// Created By 29-09-2016
        /// </summary>
        /// <param name="_ChannelExcluded"></param>
        /// <returns>bool</returns>
        public bool UpdateChannelExcluded(PE_ChannelExcluded _ChannelExcluded)
        {
            return dalPerformanceEvaluation.UpdateChannelExcluded(_ChannelExcluded);
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
            dalPerformanceEvaluation.CalculatePEScore(_IrrigationLevelID, _Session, _FromDate, _ToDate, _UserID);
        }

        /// <summary>
        /// This function returns the PE Score based of EvalID
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetPEScoreDetailByReportID(long _ReportID)
        {
            return dalPerformanceEvaluation.GetPEScoreDetailByReportID(_ReportID);
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
            return dalPerformanceEvaluation.GetEvaluationScoresDetail(_ReportID, _IrrigationBoundaryID, _CatID);
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
            return dalPerformanceEvaluation.GetCategoryWeightage(_ReportID, _IrrigationBoundaryID);
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
            return dalPerformanceEvaluation.GetSubCategoryWeightage(_ReportID, _IrrigationBoundaryID, _CatID);
        }

        /// <summary>
        /// This function check if Evaluation with a particular Name exists
        /// Created on 02-11-2016
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns>bool</returns>
        public bool GetEvaluationByName(string _Name)
        {
            return dalPerformanceEvaluation.GetEvaluationByName(_Name);
        }

        /// <summary>
        /// This function gets Evaluation with a particular ID
        /// Created on 02-11-2016
        /// </summary>
        /// <param name="_ReportID"></param>
        /// <returns>PE_EvaluationScores</returns>
        public PE_EvaluationReports GetReportByID(long _ReportID)
        {
            return dalPerformanceEvaluation.GetReportByID(_ReportID);
        }

        /// <summary>
        /// This function updates a particular Report
        /// Created on 02-11-2016
        /// </summary>
        /// <param name="_EvaluationReports"></param>
        /// <returns>bool</returns>
        public bool UpdateReport(PE_EvaluationReports _EvaluationReports)
        {
            return dalPerformanceEvaluation.UpdateReport(_EvaluationReports);
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
            return dalPerformanceEvaluation.GetEvaluationReports(_FromDate, _ToDate, _ReportName);
        }
        public List<object> GetEvaluationReportsByDateRange(DateTime? _FromDate, DateTime? _ToDate)
        {
            return dalPerformanceEvaluation.GetEvaluationReportsByDateRange(_FromDate, _ToDate);
        }
        /// <summary>
        /// This function deletes the Evaluation report based on EvalScoreID
        /// Created On 04-11-2016
        /// </summary>
        /// <param name="_EvalScoreID"></param>
        /// <returns>bool</returns>
        public bool DeleteEvaluationReport(long _ReportID)
        {
            return dalPerformanceEvaluation.DeleteEvaluationReport(_ReportID);
        }

        /// <summary>
        /// This function returns the Tail Status of Channels for Field staff for the scheduled job
        /// Created On 28-10-2016
        /// </summary>
        /// <param name="_StartDate"></param>
        /// <param name="_EndDate"></param>                
        public void GetFieldTailStatus_Job(DateTime _StartDate, DateTime _EndDate)
        {
            dalPerformanceEvaluation.GetFieldTailStatus_Job(_StartDate, _EndDate);
        }

        /// <summary>
        /// This function returns the Tail Status of the selected Channels from PMIU staff for scheduled job
        /// Created On 16-11-2016
        /// </summary>
        /// <param name="_StartDate"></param>
        /// <param name="_EndDate"></param>        
        public void GetPMIUTailStatus_Job(DateTime _StartDate, DateTime _EndDate)
        {
            dalPerformanceEvaluation.GetPMIUTailStatus_Job(_StartDate, _EndDate);
        }

        /// <summary>
        /// This function returns the Tail Difference for the provided Channels for the scheduled job
        /// Created On 16-11-2016
        /// </summary>
        /// <param name="_StartDate"></param>
        /// <param name="_EndDate"></param>                
        public void GetTailDifference_Job(DateTime _StartDate, DateTime _EndDate)
        {
            dalPerformanceEvaluation.GetTailDifference_Job(_StartDate, _EndDate);
        }

        /// <summary>
        /// This function returns the Head Difference for the provided Channels for the scheduled job
        /// Created On 16-11-2016
        /// </summary>
        /// <param name="_StartDate"></param>
        /// <param name="_EndDate"></param>                
        public void GetHeadDifference_Job(DateTime _StartDate, DateTime _EndDate)
        {
            dalPerformanceEvaluation.GetHeadDifference_Job(_StartDate, _EndDate);
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
            return dalPerformanceEvaluation.GetGeneralEvaluationScore(_IrrigationLevelID, _Session, _FromDate, _ToDate, _UserID, _ReportSpan, out _ReportID);
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
            return dalPerformanceEvaluation.GetSpecificEvaluationScore(_IrrigationLevelID, _IrrigationBoundaryID, _Session, _FromDate, _ToDate, _UserID, _ReportSpan, out _ReportID);
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
            return dalPerformanceEvaluation.IsEvaluationExists(_FromDate, _ToDate);
        }

        public bool IsTailAnalysisExists(long ChannelID, long ChannelRD)
        {
            return dalPerformanceEvaluation.IsTailAnalysisExists(ChannelID, ChannelRD);
        }
    }
}
