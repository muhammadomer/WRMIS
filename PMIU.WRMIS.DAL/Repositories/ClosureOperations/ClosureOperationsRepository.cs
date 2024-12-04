using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.ClosureOperations
{
    class ClosureOperationsRepository : Repository<CW_ClosureWorkPlan>
    {
        public ClosureOperationsRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<CW_ClosureWorkPlan>();

        }
        #region Notification and Alerts
        public CW_GetCWPPublishNotifyData_Result GetClosureWorkPlanPuslishNotifyData(long _CWPID)
        {
            CW_GetCWPPublishNotifyData_Result data = context.CW_GetCWPPublishNotifyData(_CWPID).FirstOrDefault<CW_GetCWPPublishNotifyData_Result>();

            return data;
        }
        public CW_GetCWProgressNotifyData_Result GetCW_Progress_NotifyData(long _ProgressID)
        {
            return context.CW_GetCWProgressNotifyData(_ProgressID).FirstOrDefault<CW_GetCWProgressNotifyData_Result>();
        }
        #endregion
        #region closure Work Plan
        public int GetStartRDRangeByDivision(long _DivisionID, long _ChannelID)
        {

            int data = (from boundry in context.CO_ChannelIrrigationBoundaries
                        join section in context.CO_Section on boundry.SectionID equals section.ID
                        join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                        where subDivision.DivisionID == _DivisionID && boundry.ChannelID == _ChannelID
                        select new
                        {
                            FromRD = boundry.SectionRD
                        })
                                        .ToList().Min(x => x.FromRD);


            return data;
        }

        public int? GetEndRDRangeByDivision(long _DivisionID, long _ChannelID)
        {

            int? data = (from boundry in context.CO_ChannelIrrigationBoundaries
                         join section in context.CO_Section on boundry.SectionID equals section.ID
                         join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                         where subDivision.DivisionID == _DivisionID && boundry.ChannelID == _ChannelID
                         select new
                         {
                             ToRD = boundry.SectionToRD
                         })
                        .ToList().Max(x => x.ToRD);


            return data;
        }
        public List<object> GetClosureWorkPlans()
        {
            List<object> lstCWP = new List<object>();
            try
            {
                lstCWP = (from cwp in context.CW_ClosureWorkPlan
                          join Div in context.CO_Division on cwp.DivisionID equals Div.ID
                          select new
                          {
                              // ClosureWrokPlanTitle=cwp.Title;
                              DivisionName = Div.Name,
                              DivID = Div.ID,
                              CWPID = cwp.ID,
                              CreatedBy = cwp.CreatedBy,
                              CreatedDate = cwp.CreatedDate,
                              CWPTitle = cwp.Title,
                              CWPYear = cwp.Year,
                              CWPStatus = cwp.Status,
                          }).ToList()
                                     .Select(u => new
                                     {
                                         DivisionName = u.DivisionName,
                                         ID = u.CWPID,
                                         Title = u.CWPTitle,
                                         Year = u.CWPYear,
                                         Status = u.CWPStatus,
                                         CreatedBy = u.CreatedBy,
                                         CreatedDate = u.CreatedDate,
                                         DivisionID = u.DivID
                                     }).OrderBy(x => x.CreatedDate).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstCWP;
        }


        //public IEnumerable<DataRow> CopyACCPDetialWithExcludedChannels(string _ACCPTitle, string _Year, string _Attachment, long _CreatedBy)
        //{
        //    return ContextDB.ExecuteDataSet("CO_ACCPCopyLastYearDetailWithExcluded", _ACCPTitle, _Year, _Attachment, _CreatedBy);
        //}
        #endregion

        #region closure Work
        public object GetClosureWorkProgressByID(long _WorkPorgressID)
        {
            try
            {
                object lstCWP = (from cwp in context.CW_ClosureWorkPlan
                                 join cw in context.CW_ClosureWork on cwp.ID equals cw.CWPID
                                 join ct in context.CW_WorkType on cw.WorkTypeID equals ct.ID
                                 join wp in context.CW_WorkProgress on cw.ID equals wp.ClosureWorkID
                                 where wp.ID == _WorkPorgressID
                                 select new
                                 {
                                     WorkID = cw.ID,
                                     WorkName = cw.WorkName,
                                     WorkPlanID = cw.CWPID,
                                     WorkPlanTitle = cwp.Title,
                                     DivisionID = cwp.DivisionID,
                                     YEAR = cwp.Year,
                                     Status = cwp.Status,
                                     WorkTypeID = cw.WorkTypeID,
                                     WorkTypeName = ct.Name,
                                     EstimatedCost = cw.EstimatedCost,
                                     WorkProgressID = wp.ID,
                                     WorkStatusID = wp.WorkStatusID,
                                     ProgressPercentage = wp.ProgressPercentage,
                                     ModifiedDate = wp.ModifiedDate
                                 }).AsEnumerable()
                                       .Select(u => new
                                       {
                                           WorkID = u.WorkID,
                                           WorkName = u.WorkName,
                                           WorkPlanID = u.WorkPlanID,
                                           WorkPlanTitle = u.WorkPlanTitle,
                                           DivisionID = u.DivisionID,
                                           YEAR = u.YEAR,
                                           Status = u.Status,
                                           WorkTypeID = u.WorkTypeID,
                                           WorkTypeName = u.WorkTypeName,
                                           EstimatedCost = @String.Format(new CultureInfo("ur-PK"), "{0:c}", u.EstimatedCost),
                                           WorkProgressID = u.WorkProgressID,
                                           WorkStatusID = u.WorkStatusID,
                                           ProgressPercentage = u.ProgressPercentage,
                                           ModifiedDate = Utility.GetFormattedDate(u.ModifiedDate)
                                       }).FirstOrDefault();
                return lstCWP;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return null;
            }

        }
        #endregion


        public List<object> GetWorkProgressAttachment(long? _WorkProgressID)
        {
            List<object> Attendance_Attachment = (from TA in context.CW_WorkProgressAttachment
                                                  where TA.WorkProgressID == _WorkProgressID
                                                  select new
                                                  {
                                                      ID = TA.ID,
                                                      WorkProgressID = TA.WorkProgressID,
                                                      FileName = TA.Attachment

                                                  })
                                                  .AsEnumerable().Select
                                                    (q => new
                                                    {
                                                        ID = q.ID,
                                                        WorkProgressID = q.WorkProgressID,
                                                        FileName = Utility.GetImageURL(Configuration.ClosureOperations, q.FileName)

                                                    }).ToList<object>();
            return Attendance_Attachment;

        }

        public int GetDrainStartRDRangeByDivision(long _DivisionID, long _DrainID)
        {
            long drainStructureID = (long)Constants.StructureType.Drain;
            int data = (from boundry in context.FO_StructureIrrigationBoundaries
                        join division in context.CO_Division on boundry.DivisionID equals division.ID
                        //join section in context.CO_Section on boundry.SectionID equals section.ID
                        //join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                        where division.ID == _DivisionID && boundry.StructureID == _DrainID
                        && boundry.StructureTypeID == drainStructureID
                        select new
                        {
                            FromRD = boundry.SectionRD
                        })
                        .ToList().Min(x => x.FromRD);
            return data;
        }

        public int? GetDrainEndRDRangeByDivision(long _DivisionID, long _DrainID)
        {
            long drainStructureID = (long)Constants.StructureType.Drain;
            int? data = (from boundry in context.FO_StructureIrrigationBoundaries
                         join division in context.CO_Division on boundry.DivisionID equals division.ID
                         //join section in context.CO_Section on boundry.SectionID equals section.ID
                         //join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                         where division.ID == _DivisionID && boundry.StructureID == _DrainID
                         && boundry.StructureTypeID == drainStructureID
                         select new
                         {
                             ToRD = boundry.SectionToRD
                         })
                        .ToList().Max(x => x.ToRD);
            return data;
        }
    }
}
