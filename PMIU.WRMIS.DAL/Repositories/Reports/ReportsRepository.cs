using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.Reports
{
    class ReportsRepository : Repository<RPT_GetTailStatusFieldStaff_Result>
    {
        public ReportsRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<RPT_GetTailStatusFieldStaff_Result>();

        }

        #region "Reports Dashboard"
        //public Dictionary<string, List<object>> GetReportsDashboard(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        //{
        //    Dictionary<string, List<object>> dictDashboard = new Dictionary<string, List<object>>();
        //    dictDashboard.Add("FieldStaff", GetTailStatusFieldStaff(_ZoneID, _CircleID, _DivisionID, _ToDate));
        //    dictDashboard.Add("PMIUStaff", GetTailStatusFieldStaff(_ZoneID, _CircleID, _DivisionID, _ToDate));
        //    dictDashboard.Add("ComplaintStatus", GetComplaintStatus(_ZoneID, _CircleID, _DivisionID, _ToDate));
        //    dictDashboard.Add("WaterTheftStatuse", GetWaterTheftStatuse(_ZoneID, _CircleID, _DivisionID, _ToDate));
        //    dictDashboard.Add("PerformanceEvaluation", GetPerformanceEvaluation(_ZoneID, _CircleID, _DivisionID, null, _ToDate));
        //    return dictDashboard;
        //}
        public List<object> GetTailStatusPMIUStaff(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        {
            List<object> lstResult = (from RP in context.RPT_GetTailStatusPMIUStaff(_ZoneID, _CircleID, _DivisionID, _ToDate)
                                      select RP).ToList().
                         Select(q =>
                         new
                         {
                             q.DryCount,
                             q.ShortCount,
                             q.Authorized,
                             q.Excessive

                         }).ToList<object>();

            return lstResult;
        }
        public List<object> GetTailStatusFieldStaff(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _ToDate)
        {
            List<object> lstResult = (from RP in context.RPT_GetTailStatusFieldStaff(_ZoneID, _CircleID, _DivisionID, _ToDate)
                                      select RP).ToList().
                         Select(q =>
                         new
                         {
                             q.DryCount,
                             q.ShortCount,
                             q.Authorized,
                             q.Excessive

                         }).ToList<object>();

            return lstResult;
        }
        public List<object> GetComplaintStatus(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate)
        {
            List<object> lstResult = (from RP in context.RPT_GetComplaintStatus(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate)
                                      select RP).ToList().
                         Select(q =>
                         new
                         {
                             q.NewCount,
                             q.InProgressCount,
                             q.ResolvedCount
                         }).ToList<object>();

            return lstResult;
        }
        public List<object> GetWaterTheftStatuse(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate)
        {
            List<object> lstResult = (from RP in context.RPT_GetWaterTheftStatuses(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate)
                                      select RP).ToList().
                         Select(q =>
                         new
                         {
                             q.OutletTemperedPMIU,
                             q.OutletTemperedField,
                             q.WaterTheftCasesField,
                             q.WaterTheftCasesPMIU
                         }).ToList<object>();

            return lstResult;
        }
        public List<object> GetPerformanceEvaluation(long _ZoneID, long _CircleID, long _DivisionID, DateTime? _FromDate, DateTime? _ToDate, string _Session)
        {
            List<object> lstResult = (from RP in context.RPT_GetPerformanceEvaluation(_ZoneID, _CircleID, _DivisionID, _FromDate, _ToDate, _Session)
                                      select RP).ToList<object>();
            return lstResult;
        }
        public List<object> GetPerformanceEvaluationPeriod()
        {
            List<object> lstPeriod = (from p in context.PE_EvaluationScores
                                      select new { p.FromDate, p.ToDate }).Distinct().OrderByDescending(f => f.FromDate).Take(12).ToList().
                                      Select(s => new
                                      {
                                          ID = string.Concat(Utility.GetFormattedDate(s.FromDate), "|", Utility.GetFormattedDate(s.ToDate)),
                                          Name = string.Concat(Utility.GetFormattedDate(s.FromDate), " to ", Utility.GetFormattedDate(s.ToDate))
                                      }).ToList<object>();
            return lstPeriod;
        }

        public DateTime? GetTailStatuFieldLatestReadingDate(long _ZoneID, long _CircleID, long _DivisionID)
        {
            return (from d in context.RPT_GetTailStatusFieldLatestReadingDate(_ZoneID, _CircleID, _DivisionID)
                    select d).FirstOrDefault();
        }
        public DateTime? GetTailStatusPMIULatestReadingDate(long _ZoneID, long _CircleID, long _DivisionID)
        {
            return (from d in context.RPT_GetTailStatusPMIULatestReadingDate(_ZoneID, _CircleID, _DivisionID)
                    select d).FirstOrDefault();
        }
        #endregion

        #region EntitlementAndDeliveries
        public List<object> GetEntitlementAndDeliveriesYearBySessionID(int SeasonID, int ReportID)
        {
            ContextDB db = new ContextDB();
            List<ED_ChannelEntitlement> lstChannelEntitlement = db.Repository<ED_ChannelEntitlement>().Query().Get().GroupBy(x => x.Year).Select(x => x.FirstOrDefault()).ToList();
            List<ED_TDailyGaugeReading> lstTDailyGaugeReading = db.Repository<ED_TDailyGaugeReading>().Query().Get().GroupBy(x => x.Year).Select(x => x.FirstOrDefault()).ToList();

            
            List<object> listData = new List<object>();
            if (SeasonID == 1)
            {
                if (ReportID == 1 || ReportID == 3)
                {
                    if (lstChannelEntitlement == null || lstChannelEntitlement.Count <= 0)
                        return null;
                    listData = lstChannelEntitlement.Select(x => new { ID = x.Year, Name = x.Year + " - " + (x.Year + 1) }).OrderBy(x => x.ID).ToList<object>();
                }
                else if (ReportID == 2 || ReportID == 4 || ReportID == 5)
                {
                    if (lstTDailyGaugeReading == null || lstTDailyGaugeReading.Count <= 0)
                        return null;
                    listData = lstTDailyGaugeReading.Select(x => new { ID = x.Year, Name = x.Year + " - " + (x.Year + 1) }).OrderBy(x => x.ID).ToList<object>();
                }

            }
            else
            {
                if (ReportID == 1 || ReportID == 3)
                {
                    if (lstChannelEntitlement == null || lstChannelEntitlement.Count <= 0)
                        return null;
                    listData = lstChannelEntitlement.Select(x => new { ID = x.Year, Name = x.Year }).OrderBy(x => x.ID).ToList<object>();
                }
                else if (ReportID == 2 || ReportID == 4)
                {
                    if (lstTDailyGaugeReading == null || lstTDailyGaugeReading.Count <= 0)
                        return null;
                    listData = lstTDailyGaugeReading.Select(x => new { ID = x.Year, Name = x.Year }).OrderBy(x => x.ID).ToList<object>();
                }
            }

            return listData;
        }
        #endregion

    }
}
