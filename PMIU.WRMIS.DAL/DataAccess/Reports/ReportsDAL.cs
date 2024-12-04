using PMIU.WRMIS.DAL.Repositories.Reports;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.Reports
{
    public class ReportsDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns reports dashboard
        /// Created On 27-Jan-2017
        /// </summary>
        /// <returns>Dictionary<string, List<object>></returns>
        //public Dictionary<string, List<object>> GetReportsDashboard(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        //{
        //    ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
        //    return repReportsRepository.GetReportsDashboard(_ZoneID, _CircleID, _DivisionID, _ToDate);
        //}
        public List<object> GetTailStatusPMIUStaff(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetTailStatusPMIUStaff(_ZoneID, _CircleID, _DivisionID, _ToDate);
        }
        public List<object> GetTailStatusFieldStaff(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetTailStatusFieldStaff(_ZoneID, _CircleID, _DivisionID, _ToDate);
        }
        public List<object> GetComplaintStatus(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate)
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetComplaintStatus(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate);
        }
        public List<object> GetWaterTheftStatuse(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate)
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetWaterTheftStatuse(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate);
        }
        public List<object> GetPerformanceEvaluation(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate, string _Session)
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetPerformanceEvaluation(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate, _Session);
        }
        public List<object> GetPerformanceEvaluationPeriod()
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetPerformanceEvaluationPeriod();
        }
        public DateTime? GetTailStatuFieldLatestReadingDate(long _ZoneID, long _CircleID, long _DivisionID)
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetTailStatuFieldLatestReadingDate(_ZoneID, _CircleID, _DivisionID);
        }
        public DateTime? GetTailStatusPMIULatestReadingDate(long _ZoneID, long _CircleID, long _DivisionID)
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetTailStatusPMIULatestReadingDate(_ZoneID, _CircleID, _DivisionID);
        }

        #region EntitlementAndDeliveries

        public List<object> GetEntitlementAndDeliveriesYearBySessionID(int SeasonID, int ReportID)
        {
            ReportsRepository repReportsRepository = this.db.ExtRepositoryFor<ReportsRepository>();
            return repReportsRepository.GetEntitlementAndDeliveriesYearBySessionID(SeasonID,ReportID);
        
        }
        #endregion
    }
}
