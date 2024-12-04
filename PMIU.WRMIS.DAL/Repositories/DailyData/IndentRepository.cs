using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.DailyData
{
    public class IndentRepository : Repository<CO_ChannelIndent>
    {
        WRMIS_Entities _context;

        public IndentRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<CO_ChannelIndent>();
            _context = context;
        }

        /// <summary>
        /// this function get Channel data and max date from Channel Indent
        /// Created On: 10/12/2015
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_CommandTypeID"></param>
        /// <param name="_ChannelTypeID"></param>
        /// <param name="_FlowTypeID"></param>
        /// <param name="_ChannelNameID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetChannelData(long _SubDivID, long _CommandTypeID, long _ChannelTypeID, long _FlowTypeID, long _ChannelNameID)
        {
            //List<dynamic> lstChannel = (from c in context.CO_Channel
            //                            join cib in context.CO_ChannelIrrigationBoundaries on c.ID equals cib.ChannelID
            //                            join s in context.CO_Section on cib.SectionID equals s.ID
            //                            join sd in context.CO_SubDivision on s.SubDivID equals sd.ID
            //                            where sd.ID == _SubDivID
            //                            && (c.ComndTypeID == _CommandTypeID || _CommandTypeID == -1)
            //                            && (c.ChannelTypeID == _ChannelTypeID || _ChannelTypeID == -1)
            //                            && (c.FlowTypeID == _FlowTypeID || _FlowTypeID == -1)
            //                            && (c.ID == _ChannelNameID || _ChannelNameID == -1)
            //                            select new
            //                            {
            //                                Channel = c,
            //                                SubDivId = sd.ID,
            //                                //Date = context.CO_ChannelIndent.Where(i => i.ChannelID == c.ID && i.SubDivID == _SubDivID).Max(i => (DateTime?)i.FromDate)
            //                                Date = context.CO_ChannelIndent.Where(i => i.ChannelID == c.ID && i.SubDivID == _SubDivID && i.FromDate <= DateTime.Today).Select(x => x.FromDate).OrderByDescending(x => x).FirstOrDefault()
            //                            }).Distinct().ToList<dynamic>();
            List<dynamic> lstChannel = new List<dynamic>();
            return lstChannel;
        }


        /// <summary>
        /// this function return channel by subdiv id
        /// Created On: 11/12/2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetChannelsNameBySubDivisionIDOrDivisionID(long? _SubDivisionID, long? _DivisionID)
        {
            List<CO_Channel> lstChannelSearch = (from c in context.CO_Channel
                                                 join cib in context.CO_ChannelIrrigationBoundaries on c.ID equals cib.ChannelID
                                                 join s in context.CO_Section on cib.SectionID equals s.ID
                                                 join sd in context.CO_SubDivision on s.SubDivID equals sd.ID
                                                 join cg in context.CO_ChannelGauge on c.ID equals cg.ChannelID
                                                 where (sd.ID == _SubDivisionID || _SubDivisionID == -1)
                                                 && (sd.DivisionID == _DivisionID || _DivisionID == -1)
                                                 && (cg.GaugeSubDivID != null)
                                                 select c
                                              ).Distinct().ToList<CO_Channel>();
            return lstChannelSearch;
        }

        /// <summary>
        /// this function return sections by subdiv id
        /// Created on: 14/12/2015
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>CO_ChannelIrrigationBoundaries</returns>
        public CO_ChannelIrrigationBoundaries GetSectionBySubDivID(long _SubDivID, long _ChannelID)
        {
            CO_ChannelIrrigationBoundaries qChannelIB = (from cib in context.CO_ChannelIrrigationBoundaries
                                                         where cib.ChannelID == _ChannelID && (from s in context.CO_Section
                                                                                               where s.SubDivID == _SubDivID
                                                                                               select s.ID).ToList().Contains((long)cib.SectionID)
                                                         orderby cib.SectionRD
                                                         select cib).FirstOrDefault();
            return qChannelIB;
        }

        /// <summary>
        /// this function return list of sub divisions on the basis of XEN User ID
        /// Created On: 02-08-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_SubDivision></returns>
        public List<CO_SubDivision> GetSubDivisionsOFXENByUserID(long _UserID, long? _IrrigationLevelID)
        {
            List<CO_SubDivision> lstSubDivisions = (from al in context.UA_AssociatedLocation
                                                    join sd in context.CO_SubDivision on al.IrrigationBoundryID equals sd.DivisionID
                                                    where (al.UserID == _UserID && al.IrrigationLevelID == _IrrigationLevelID)
                                                    select sd).ToList<CO_SubDivision>();
            return lstSubDivisions;
        }

        /// <summary>
        /// this function return sub division on the basis of SDO User ID
        /// Created On: 02-08-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_SubDivision></returns>
        public List<CO_SubDivision> GetSubDivisionOFSDOByUserID(long _UserID, long? _IrrigationLevelID)
        {
            List<CO_SubDivision> lstSubDivisions = (from al in context.UA_AssociatedLocation
                                                    join sd in context.CO_SubDivision on al.IrrigationBoundryID equals sd.ID
                                                    where (al.UserID == _UserID && al.IrrigationLevelID == _IrrigationLevelID)
                                                    select sd).ToList<CO_SubDivision>();
            return lstSubDivisions;
        }

        /// <summary>
        /// this function return all parent and channels against specific Subdivision
        /// Created On: 9/8/2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_SubDivID"></param>
        /// <param name="_ParentChild"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetChannelsByUserIDAndSubDivID(long _UserID, long _IrrigationLevelID, long _SubDivID, string _ParentChild)
        {
            List<dynamic> qChannelsByUserIDAndSubDivID = context.DD_GetChannelsByUserIDForIndents(_UserID, _IrrigationLevelID, _SubDivID).Where(x => x.ParentChild == _ParentChild || _ParentChild == null).Select(s => new
            {
                s.ChannelID,
                s.ChannelName,
                s.SortOrder,
                s.ParentChild
            }).ToList<dynamic>();
            return qChannelsByUserIDAndSubDivID;
        }

        public List<DD_GetChannelsByUserIDForIndents_Result> GetChannelsByUserIDAndSubDivIDTest(long _UserID, long _IrrigationLevelID, long _SubDivID, string _ParentChild)
        {
            List<DD_GetChannelsByUserIDForIndents_Result> qChannelsByUserIDAndSubDivID = context.DD_GetChannelsByUserIDForIndents(_UserID, _IrrigationLevelID, _SubDivID).ToList<DD_GetChannelsByUserIDForIndents_Result>();
            //    .Select(s => new
            //{
            //    s.ChannelID,
            //    s.ChannelName,
            //    s.SortOrder,
            //    s.ParentChild
            //}).ToList<dynamic>();
            return qChannelsByUserIDAndSubDivID;
        }

        public List<dynamic> GetChannelIndentsOffTakesFromOffTakesTable(long _IndentID, DateTime _Date)
        {
            List<dynamic> lstChannel = (from cio in context.CO_ChannelIndentOfftakes
                                        //join gl in context.CO_GaugeLag on cio.ChannelID equals gl.ChannelID
                                        where (cio.IndentID == _IndentID)
                                        select new
                                        {
                                            ID = cio.ID,
                                            IndentID = cio.IndentID,
                                            DirectOfftakeID = cio.CO_Channel.ID,
                                            DirectOfftakeName = cio.CO_Channel.NAME,
                                            ChannelType = cio.CO_Channel.CO_ChannelType.Name,
                                            ChannelRD = cio.ChannelRD,
                                            OfftakeIndentDate = _Date,
                                            OfftakeIndentTime = _Date.TimeOfDay,
                                            ChannelIndent = cio.ChannelIndent,
                                            Remarks = cio.Remarks
                                        }).ToList<dynamic>();
            lstChannel = lstChannel.Where(x => x.OfftakeIndentDate.Date <= _Date.Date).ToList();
            return lstChannel;
        }


        public dynamic GetChannelIndentsOffTakesFromChannelTable(long _ChannelID, DateTime _Date)
        {
            dynamic mdlChannel = (from c in context.CO_Channel
                                  join gl in context.CO_GaugeLag on c.ID equals gl.ChannelID
                                  where (c.ID == _ChannelID)
                                  select new
                                  {
                                      ID = 0,
                                      IndentID = 0,
                                      DirectOfftakeID = c.ID,
                                      DirectOfftakeName = c.NAME,
                                      ChannelType = c.CO_ChannelType.Name,
                                      ChannelRD = context.CO_ChannelParentFeeder.Where(x => x.ChannelID == _ChannelID).Select(y => y.ParrentFeederRDS).FirstOrDefault(),//c.TotalRDs,
                                      OfftakeIndentDate = _Date,
                                      OfftakeIndentTime = gl.LagTime,
                                      ChannelIndent = context.CO_ChannelGauge.Where(x => x.ChannelID == _ChannelID && x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge).Select(x => x.DesignDischarge).FirstOrDefault(),
                                      Remarks = string.Empty
                                  }).FirstOrDefault();
            return mdlChannel;
        }

        /// <summary>
        /// this function return outlet indent on the basis of SubDivID and ChannelID
        /// Created On: 12/08/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public double? GetOutletIndent(long _SubDivID, long _ChannelID)
        {
            double? fltGetOutletIndent = context.DD_GetOutletIndent(_SubDivID, _ChannelID).First();
            if (fltGetOutletIndent == null)
            {
                return fltGetOutletIndent = 0;
            }
            else
            {
                return fltGetOutletIndent;
            }
        }

        /// <summary>
        /// this function will return Indents By Sub Division ID Or Channel ID and indent Date
        /// Created On:17/08/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_IndentPlacementDate"></param>
        /// <returns>List<DD_GetDailyIndentsData_Result></returns>
        public List<DD_GetDailyIndentsData_Result> GetIndentsBySubDivisionIDOrChannelID(long _SubDivID, long _ChannelID, DateTime _IndentPlacementDate)
        {
            List<DD_GetDailyIndentsData_Result> qChannelsByUserIDAndSubDivID = context.DD_GetDailyIndentsData(_SubDivID, _ChannelID, _IndentPlacementDate).ToList<DD_GetDailyIndentsData_Result>();
            return qChannelsByUserIDAndSubDivID;
        }

        /// <summary>
        /// this query return  Gauge ID by SubbDivID and ChannelID
        /// Created ON: 19/08/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>long</returns>
        public long GetGaugeIDByChannelIDAndSubDivID(long _SubDivID, long _ChannelID)
        {
            //long GaugeID = (from cg in context.CO_ChannelGauge
            //                where cg.ChannelID == _ChannelID && (cg.GaugeCategoryID == 4 || cg.GaugeCategoryID == 1) && cg.CO_Section.SubDivID == _SubDivID
            //                select cg.ID).First();
            
            long GaugeID = (from cg in context.CO_ChannelGauge
                            where cg.ChannelID == _ChannelID && (cg.GaugeSubDivID == _SubDivID || (cg.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge && cg.CO_Section.SubDivID == _SubDivID)) 
                            select cg.ID).FirstOrDefault();
            return GaugeID;
        }

        /// <summary>
        /// this function return indent date
        /// Created On: 19/8/2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_GaugeID"></param>
        /// <param name="_Date"></param>
        /// <returns>DateTime?</returns>
        public DateTime? CalculateIndentDate(int _ChannelID, int _GaugeID, DateTime _Date)
        {
            DateTime? IndentDate = context.CalculateIndentDate(_ChannelID, _GaugeID, _Date).First();
            return IndentDate;
        }
    }
}
