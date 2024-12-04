using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.WaterLosses
{
    public class WaterLossesRepository : Repository<WL_ChannelLG>
    {
        public WaterLossesRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<WL_ChannelLG>();
        }

        public List<GetWaterLosses_ChannelWise_Result> GetChannelWaterLosses(long _ChannelID, DateTime _FromDate, DateTime _ToDate)
        {
            var q = from g in context.GetWaterLosses_ChannelWise(_FromDate, _ToDate, _ChannelID)
                    select g;
            return q.ToList<GetWaterLosses_ChannelWise_Result>();
        }

        public List<GetSubDivisionalWL_Result> GetSubdivionalWaterLosses(long _SubDivisionID, int _Year, int _From, int _To)
        {
            var q = from g in context.GetSubDivisionalWL(_SubDivisionID, _Year, _From, _To)
                    select g;
            return q.ToList<GetSubDivisionalWL_Result>();
        }

        public List<GetSubDivDeliveredTo_Result> GetSubdivisionalWL(long _GaugeID, long _SubDivID, int _Year, int _From, int _To)
        {
            var q = from g in context.GetSubDivDeliveredTo(_SubDivID, _GaugeID, _Year, _From, _To)
                    select g;
            return q.ToList<GetSubDivDeliveredTo_Result>();
        }

        public List<WL_GetDivisionalLGData_Result> GetDivisionalWL(long _DivisionID, int _SearchType, string _FromDate, string _ToDate, int _Year, int _From, int _To)
        {

            var q = from g in context.WL_GetDivisionalLGData(_DivisionID, _SearchType, _FromDate, _ToDate, _Year, _From, _To)
                    orderby g.SubDivGaugeRD
                    select g;
            return q.ToList<WL_GetDivisionalLGData_Result>();
        }

        public List<WL_GetDivisionalWatertoNextDivisions_Result> GetDivisionalWatertoNextDivisions(long _DivisionID, long _ChannelID, int _SearchType, string _FromDate, string _ToDate, int _Year, int _From, int _To)
        {
            var q = from g in context.WL_GetDivisionalWatertoNextDivisions(_DivisionID, _ChannelID, _SearchType, _FromDate, _ToDate, _Year, _From, _To)
                    select g;
            return q.ToList<WL_GetDivisionalWatertoNextDivisions_Result>();
        }
        public double? GetOutletsDesignDischarge(long _ChannelID, int _FromRD, int _ToRD)
        {

            double? res = context.Database.SqlQuery<double?>("SELECT [dbo].[GetDirectOfftakeDischarge](@p0,@p1,@p2)", _ChannelID, _FromRD, _ToRD).FirstOrDefault();
            return res;
        }

        public List<object> GetChannelsByDivision(long _DivisionID)
        {

            List<object> lstChannels = (from channel in context.CO_Channel
                                        join boundry in context.CO_ChannelIrrigationBoundaries on channel.ID equals boundry.ChannelID
                                        join section in context.CO_Section on boundry.SectionID equals section.ID
                                        join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                        where subDivision.DivisionID == _DivisionID
                                        select new
                                        {
                                            ID = channel.ID,
                                            Name = channel.NAME
                                            //,
                                            //SectionID = section.ID,
                                            //SectionName = section.Name,
                                            //SubDivID = subDivision.ID,
                                            //SubDivName = subDivision.Name 
                                        }).Distinct().OrderBy(x => x.Name).ToList<object>();

            return lstChannels;
        }
        public List<object> GetChannelsBySubDivision(long _SubDivisionID)
        {

            List<object> lstChannels = (from channel in context.CO_Channel
                                        join boundry in context.CO_ChannelIrrigationBoundaries on channel.ID equals boundry.ChannelID
                                        join section in context.CO_Section on boundry.SectionID equals section.ID
                                        where section.SubDivID == _SubDivisionID
                                        select new
                                        {
                                            ID = channel.ID,
                                            Name = channel.NAME
                                            //,
                                            //SectionID = section.ID,
                                            //SectionName = section.Name,
                                            //SubDivID = section.ID
                                        }).Distinct().OrderBy(x => x.Name).ToList<object>();

            return lstChannels;
        }
        public List<object> GetChannelBySectionID(long _SectionID)
        {

            List<object> lstChannels = (from channel in context.CO_Channel
                                        join boundry in context.CO_ChannelIrrigationBoundaries on channel.ID equals boundry.ChannelID
                                        where boundry.SectionID == _SectionID
                                        select new
                                        {
                                            ID = channel.ID,
                                            Name = channel.NAME
                                        }).Distinct().OrderBy(x => x.Name).ToList<object>();

            return lstChannels;
        }

        public List<object> GetDistrictByDivision(long _DivisionID)
        {

            List<object> lstData = (from d in context.CO_District
                                    join dd in context.CO_DistrictDivision on d.ID equals dd.DistrictID
                                    where dd.DivisionID == _DivisionID && d.IsActive == true
                                    select new
                                    {
                                        ID = d.ID,
                                        Name = d.Name
                                    }).Distinct().OrderBy(x => x.Name).ToList<object>();

            return lstData;
        }

        public List<object> GetDrainsByDivision(long _DivisionID)
        {
            long drainStructureID = (long)Constants.StructureType.Drain;

            List<object> lstData = (from d in context.FO_Drain
                                    join dd in context.FO_StructureIrrigationBoundaries on d.ID equals dd.StructureID //d.DrainTypeID equals dd.StructureTypeID
                                    where dd.DivisionID == _DivisionID && dd.StructureTypeID == drainStructureID && d.IsActive == true
                                    select new
                                    {
                                        ID = d.ID,
                                        Name = d.Name
                                    }).Distinct().OrderBy(x => x.Name).ToList<object>();

            return lstData;
        }

        public List<object> GetDivisions()
        {
            List<object> lstDiv = (from Div in context.CO_Division
                                   where
                                       //Div.DomainID == 4 && // Domain ID of Drainage                                   
                                    ((from SIB in context.FO_StructureIrrigationBoundaries
                                      where SIB.StructureTypeID == (long)Constants.StructureType.Drain
                                      select (long)SIB.DivisionID).Distinct().ToList()).Contains(Div.ID)
                                   select new
                                   {
                                       ID = Div.ID,
                                       Name = Div.Name
                                   }).ToList<object>();
            return lstDiv;
        }
    }
}
