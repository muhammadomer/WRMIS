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

namespace PMIU.WRMIS.DAL.Repositories.AssetsAndWorks
{
    class AssetsWorkRepository : Repository<AM_Category>
    {
        public AssetsWorkRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<AM_Category>();

        }
        #region Work
        public object GetStructureTypeIDByInsfrastructureValue(long _InfrastructureType, long _InfrastructureNamevalue)
        {
            object InfrastructureType = null;

            switch (_InfrastructureType)
            {
                case 1:
                    InfrastructureType = (from p in context.FO_ProtectionInfrastructure
                                          join s in context.CO_StructureType on p.InfrastructureTypeID equals s.ID
                                          where p.ID.Equals(_InfrastructureNamevalue)
                                          select new
                                          {
                                              ProtectionInfrastructureID = p.ID,
                                              InfrastructureTypeID = p.InfrastructureTypeID,
                                              InfrastructureName = p.InfrastructureName,
                                              InfrastructureTypeName = s.Source,
                                              TotalLength = p.TotalLength

                                          }).Distinct().FirstOrDefault();

                    break;
                case 2:
                    InfrastructureType = (from p in context.CO_Station
                                          join s in context.CO_StructureType on p.StructureTypeID equals s.ID
                                          where p.ID.Equals(_InfrastructureNamevalue)
                                          select new
                                          {
                                              StationID = p.ID,
                                              InfrastructureTypeID = p.StructureTypeID,
                                              InfrastructureName = p.Name,
                                              InfrastructureTypeName = s.Source

                                          }).Distinct().FirstOrDefault();
                    break;
                case 3:
                    InfrastructureType = (from p in context.FO_Drain
                                          join s in context.CO_StructureType on p.DrainTypeID equals s.ID
                                          where p.ID.Equals(_InfrastructureNamevalue)
                                          select new
                                          {
                                              DrainID = p.ID,
                                              InfrastructureTypeID = p.DrainTypeID,
                                              InfrastructureName = p.Name,
                                              InfrastructureTypeName = s.Source,
                                              TotalLength = p.TotalLength

                                          }).Distinct().FirstOrDefault();
                    break;
                default:
                    break;

            }
            return InfrastructureType;
        }

        public List<object> GetSmallDamByDivisionID(long _DivisionID)
        {
            List<long> lstSubDivisionIDs = (from SubDivision in context.CO_SubDivision
                                            where SubDivision.DivisionID == _DivisionID
                                            select SubDivision.ID).ToList();
            List<object> lstSmallDams = (from SS in context.SD_SmallDam
                                         where lstSubDivisionIDs.Contains(SS.SubDivID)
                                         select SS).Distinct().ToList<object>();
            return lstSmallDams;
        }
        public List<object> GetSmallDamChannelByDivisionID(long _DivisionID)
        {
            List<long> lstSubDivisionIDs = (from SubDivision in context.CO_SubDivision
                                            where SubDivision.DivisionID == _DivisionID
                                            select SubDivision.ID).ToList();
            List<long> lstSmallDamsIDs = (from SS in context.SD_SmallDam
                                          where lstSubDivisionIDs.Contains(SS.SubDivID)
                                          select SS.ID).ToList();

            List<object> lstSmallDamsChannel = (from SSC in context.SD_SmallChannel
                                                where lstSmallDamsIDs.Contains(SSC.SmallDamID)
                                                select SSC).Distinct().ToList<object>();
            return lstSmallDamsChannel;
        }
        public object GetLastWorkProgressID(long AssetWorkID,long userid)
        {
            object WorkProgress = (from u in context.AM_AssetWorkProgress
                                   where u.AssetWorkID == AssetWorkID && u.CreatedBy==userid
                                   orderby u.ID descending
                                   select new
                                   {
                                       WorkProgressID = u.WorkProgressID

                                   }).FirstOrDefault();
            return WorkProgress;
        }
        #endregion
        #region Notification and Alerts
        public AM_GetAWAssetAssociationNotifyData_Result GetAW_Association_NotifyData(long _AWID)
        {
            return context.AM_GetAWAssetAssociationNotifyData(_AWID).FirstOrDefault<AM_GetAWAssetAssociationNotifyData_Result>();
        }
        public AM_GetAWProgressNotifyData_Result GetAW_Progress_NotifyData(long _ProgressID)
        {
            return context.AM_GetAWProgressNotifyData(_ProgressID).FirstOrDefault<AM_GetAWProgressNotifyData_Result>();
        }
        public AM_GetAWPublishNotifyData_Result1 GetAW_Publish_NotifyData(long _AWID)
        {
            return context.AM_GetAWPublishNotifyData(_AWID).FirstOrDefault<AM_GetAWPublishNotifyData_Result1>();
        }
        #endregion

        #region "Android"
        public object GetAssetWorkProgressByID(long _WorkPorgressID)
        {
            try
            {
                object lstCWP = (from aw in context.AM_AssetWork
                                 join wt in context.AM_AssetWorkType on aw.AssetWorkTypeID equals wt.ID
                                 join wp in context.AM_AssetWorkProgress on aw.ID equals wp.AssetWorkID
                                 where wp.ID == _WorkPorgressID
                                 select new
                                 {
                                     WorkID = aw.ID,
                                     WorkName = aw.WorkName,
                                     DivisionID = aw.DivisionID,
                                     YEAR = aw.FinancialYear,
                                     Status = aw.WorkStatus,
                                     WorkTypeID = aw.AssetWorkTypeID,
                                     WorkTypeName = wt.Name,
                                     EstimatedCost = aw.EstimatedCost,
                                     WorkProgressID = wp.ID,
                                     WorkStatusID = wp.WorkProgressID,
                                     ProgressPercentage = wp.ProgressPercentage,
                                     FinancialPercentage = wp.FinancialPercentage,
                                     ModifiedDate = wp.ModifiedDate
                                 }).AsEnumerable()
                                       .Select(u => new
                                       {
                                           WorkID = u.WorkID,
                                           WorkName = u.WorkName,
                                           DivisionID = u.DivisionID,
                                           YEAR = u.YEAR,
                                           Status = u.Status,
                                           WorkTypeID = u.WorkTypeID,
                                           WorkTypeName = u.WorkTypeName,
                                           EstimatedCost = @String.Format(new CultureInfo("ur-PK"), "{0:c}", u.EstimatedCost),
                                           WorkProgressID = u.WorkProgressID,
                                           WorkStatusID = u.WorkStatusID,
                                           ProgressPercentage = u.ProgressPercentage,
                                           FinancialPercentage = u.FinancialPercentage,
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
        public List<object> GetWorkProgressAttachment(long? _WorkProgressID)
        {
            List<object> Attendance_Attachment = (from TA in context.AM_AssetWorkProgressAttachment
                                                  where TA.AssetWorkProgressID == _WorkProgressID
                                                  select new
                                                  {
                                                      ID = TA.ID,
                                                      WorkProgressID = TA.AssetWorkProgressID,
                                                      FileName = TA.Attachment

                                                  })
                                                  .AsEnumerable().Select
                                                    (q => new
                                                    {
                                                        ID = q.ID,
                                                        WorkProgressID = q.WorkProgressID,
                                                        FileName = Utility.GetImageURL(Configuration.AssetsWorks, q.FileName)

                                                    }).ToList<object>();
            return Attendance_Attachment;

        }
        #endregion

        #region Reports
        public List<object> GetAttributeList(long? _SubcatID)
        {
            List<object> Attributelist = null;
            Attributelist = (from p in context.AM_Attributes
                             join s in context.AM_SubCategory on p.SubCategoryID equals s.ID
                             where s.ID == _SubcatID
                             select new
                             {
                                 ID = p.ID,
                                 Name = p.AttributeName,
                                 Catname = s.Name
                             }).ToList().Select(p => new
                                                    {
                                                        ID = p.ID,
                                                        AttributeName = p.Name
                                                        //AttributeName = p.Catname + "_" + p.Name

                                                    }).ToList<object>();

            return Attributelist;

        }
        public List<object> GetAttributeValueList(long? _AttributeID)
        {
            List<object> AttributeValue = null;
            AttributeValue = (from p in context.AM_AssetAttributes
                              join s in context.AM_Attributes on p.AttributeID equals s.ID
                              where s.ID == _AttributeID
                              select new
                              {
                                  ID = p.ID,
                                  Name = p.AttributeValue,
                              }).ToList().Select(p => new
                             {
                                 ID = p.ID,
                                 AttributeValue = p.Name
                                 //AttributeName = p.Catname + "_" + p.Name

                             }).ToList<object>();

            return AttributeValue;

        }
        public List<object> GetAssetName(long? _SubcatID)
        {
            List<object> Attributelist = null;
            Attributelist = (from p in context.AM_AssetItems
                             join s in context.AM_SubCategory on p.SubCategoryID equals s.ID
                             where s.ID == _SubcatID                               
                             select new
                             {
                                 ID = p.ID,
                                 Name = p.AssetName,
                                 Catname = s.Name
                             }).ToList().Select(p => new
                             {
                                 ID = p.ID,
                                 AssetName = p.Name
                                 //AttributeName = p.Catname + "_" + p.Name

                             }).ToList<object>();

            return Attributelist;

        }
        public List<object> GetAssetName(long? _SubcatID, long? IrrigationLevelID, long? IrrigationBoundaryID)
        {
            List<object> Attributelist = null;
            Attributelist = (from p in context.AM_AssetItems
                             join s in context.AM_SubCategory on p.SubCategoryID equals s.ID
                             where s.ID == _SubcatID && p.IrrigationLevelID == IrrigationLevelID && p.IrrigationBoundryID == IrrigationBoundaryID
                             select new
                             {
                                 ID = p.ID,
                                 Name = p.AssetName,
                                 Catname = s.Name
                             }).ToList().Select(p => new
                             {
                                 ID = p.ID,
                                 AssetName = p.Name
                                 //AttributeName = p.Catname + "_" + p.Name

                             }).ToList<object>();

            return Attributelist;

        }


        #endregion
    }
}
