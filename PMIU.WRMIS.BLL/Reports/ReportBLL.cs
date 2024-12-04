
using PMIU.WRMIS.DAL.DataAccess.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PMIU.WRMIS.BLL.Reports
{
    public class ReportsBLL : BaseBLL
    {
        //public Dictionary<string, List<object>> GetReportsDashboard(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        //{
        //    ReportsDAL dalReports = new ReportsDAL();
        //    return dalReports.GetReportsDashboard(_ZoneID, _CircleID, _DivisionID, _ToDate);
        //}
        public List<object> GetTailStatusPMIUStaff(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetTailStatusPMIUStaff(_ZoneID, _CircleID, _DivisionID, _ToDate);
        }
        public List<object> GetTailStatusFieldStaff(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetTailStatusFieldStaff(_ZoneID, _CircleID, _DivisionID, _ToDate);
        }
        public List<object> GetComplaintStatus(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate)
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetComplaintStatus(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate);
        }
        public List<object> GetWaterTheftStatuse(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate)
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetWaterTheftStatuse(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate);
        }
        public List<object> GetPerformanceEvaluation(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate, string _Session)
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetPerformanceEvaluation(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate, _Session);
        }
        public List<object> GetPerformanceEvaluationPeriod()
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetPerformanceEvaluationPeriod();
        }
        public DateTime? GetTailStatuFieldLatestReadingDate(long _ZoneID, long _CircleID, long _DivisionID)
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetTailStatuFieldLatestReadingDate(_ZoneID, _CircleID, _DivisionID);
        }
        public DateTime? GetTailStatusPMIULatestReadingDate(long _ZoneID, long _CircleID, long _DivisionID)
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetTailStatusPMIULatestReadingDate(_ZoneID, _CircleID, _DivisionID);
        }

        #region EntitlementAndDeliveries
        public List<object> GetEntitlementAndDeliveriesYearBySessionID(int SeasonID, int ReportID)
        {
            ReportsDAL dalReports = new ReportsDAL();
            return dalReports.GetEntitlementAndDeliveriesYearBySessionID(SeasonID, ReportID);
        }
        #endregion
    }
}
