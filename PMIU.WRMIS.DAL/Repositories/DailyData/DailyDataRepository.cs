using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.DailyData
{
    public class DailyDataRepository : Repository<CO_BarrageGaugeReadingFrequency>
    {
        WRMIS_Entities _context;

        public DailyDataRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<CO_BarrageGaugeReadingFrequency>();
            _context = context;
        }

        #region Add Barrage Data Frequency
        public bool IsValidActor(long UserID)
        {
            bool Allowed = false;
            try
            {
                UA_Designations objDesignation = (from usr in context.UA_Users
                                                  join des in context.UA_Designations on usr.DesignationID equals des.ID
                                                  where usr.ID == UserID
                                                  select des).FirstOrDefault();

                if (objDesignation != null)
                {
                    string Designation = objDesignation.Name;
                    long? IrrigationLevel = objDesignation.IrrigationLevelID;

                    if ((IrrigationLevel != null && IrrigationLevel == 3) || Designation == "Data Analyst")
                    {
                        Allowed = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return Allowed;
        }
        public long ViewBarrageFrequency(long _BarrageID)
        {
            long barrExist = -1;
            try
            {
                barrExist = (from barrFreq in context.CO_BarrageGaugeReadingFrequency
                             join readingFreq in context.CO_ReadingFrequency on barrFreq.ReadingFrequencyID equals readingFreq.ID
                             where barrFreq.StationID == _BarrageID
                             orderby barrFreq.FrequencyDateTime descending
                             select readingFreq.ID).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return barrExist;
        }
        public bool BarrageFrequencyExist(long _BarrageID, long _Frequency)
        {
            bool result = false;
            try
            {
                CO_BarrageGaugeReadingFrequency barrExist = (from barrFreq in context.CO_BarrageGaugeReadingFrequency
                                                             join readingFreq in context.CO_ReadingFrequency on barrFreq.ReadingFrequencyID equals readingFreq.ID
                                                             where barrFreq.StationID == _BarrageID &&
                                                             readingFreq.ReadingFrequencyValue == _Frequency
                                                             orderby barrFreq.FrequencyDateTime descending
                                                             select barrFreq).FirstOrDefault();

                if (barrExist != null)
                {
                    result = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return result;
        }
        public bool BarrageValueExist(long BarrageID, long FrequencyID, DateTime date)
        {
            bool result = false;
            try
            {
                CO_BarrageGaugeReadingFrequency objBarrageFreq = (from barrFreq in context.CO_BarrageGaugeReadingFrequency
                                                                  where barrFreq.StationID == BarrageID
                                                                  select barrFreq).FirstOrDefault();
                if (objBarrageFreq != null)
                {
                    //if (FrequencyID == 24)
                    //    objBarrageFreq.ReadingFrequencyID = 1;
                    //else if (FrequencyID == 8)
                    //    objBarrageFreq.ReadingFrequencyID = 2;
                    //else if (FrequencyID == 4)
                    //    objBarrageFreq.ReadingFrequencyID = 3;
                    //else if (FrequencyID == 2)
                    //    objBarrageFreq.ReadingFrequencyID = 4;

                    objBarrageFreq.ReadingFrequencyID = FrequencyID;
                    objBarrageFreq.FrequencyDateTime = date;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return result;
        }

        public List<CO_ReadingFrequency> FrequencyValues()
        {
            List<CO_ReadingFrequency> lstReadFreq = new List<CO_ReadingFrequency>();
            try
            {
                lstReadFreq = (from rf in context.CO_ReadingFrequency
                               select rf).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstReadFreq;
        }

        #endregion

        #region Outlet Performance Data
        public string GetOutletside(string _side)
        {
            string Outletside = "";
            try
            {
                if (_side != "")
                {
                    if (_side == "L")
                        Outletside = "Left";
                    else
                        Outletside = "Right";
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Outletside;
        }

        public string GetVillageName(long? _VillageID)
        {
            string VillageName = "";
            try
            {
                if (_VillageID != null)
                {
                    VillageName = (from vil in context.CO_Village
                                   where vil.ID == _VillageID
                                   select vil.Name).FirstOrDefault();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return VillageName;
        }

        public string GetPoliceStationName(long? _VillageID)
        {
            string PoliceStationName = "";
            try
            {
                if (_VillageID != null)
                {
                    PoliceStationName = (from vil in context.CO_Village
                                         where vil.ID == _VillageID
                                         select vil.CO_PoliceStation.Name).FirstOrDefault();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return PoliceStationName;
        }

        public string GetOutletMMH(double? _MMH, long _OutletID)
        {
            string sMMH = "";
            try
            {
                if (_MMH == null)
                {
                    double? OutletMMH = (from histry in context.CO_OutletAlterationHistroy
                                         where histry.OutletID == _OutletID
                                         select histry.OutletMMH).FirstOrDefault();

                    if (OutletMMH != null)
                    {
                        sMMH = OutletMMH.Value.ToString();
                    }

                }
                else
                {
                    sMMH = _MMH.ToString();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return sMMH;
        }

        public string GetOutletDischarge(double? _Discharge, long _OutletID)
        {
            string sMMH = "";
            try
            {
                if (_Discharge == null)
                {
                    double? OutletMMH = (from histry in context.CO_OutletAlterationHistroy
                                         where histry.OutletID == _OutletID
                                         select histry.OutletMMH).FirstOrDefault();

                    if (OutletMMH != null)
                    {
                        sMMH = OutletMMH.Value.ToString();
                    }

                }
                else
                {
                    sMMH = _Discharge.ToString();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return sMMH;
        }

        public object OutletInformation(long _OutletID)
        {
            object lstResult = new object();
            try
            {

                lstResult = (from chnlOtlts in context.CO_ChannelOutlets
                             join chnl in context.CO_Channel on chnlOtlts.ChannelID equals chnl.ID into A
                             from a in A.DefaultIfEmpty()
                             join chnlType in context.CO_ChannelType on a.ChannelTypeID equals chnlType.ID
                             join chnlOutletLoc in context.CO_ChannelOutletsLocation on chnlOtlts.ID equals chnlOutletLoc.OutletID into B
                             from b in B.DefaultIfEmpty()
                             join outletAltHis in context.CO_OutletAlterationHistroy on chnlOtlts.ID equals outletAltHis.OutletID into D
                             from d in D.DefaultIfEmpty()
                             join outletType in context.CO_OutletType on d.OutletTypeID equals outletType.ID
                             where chnlOtlts.ID == _OutletID
                             select new
                             {
                                 ChannelName = a.NAME,
                                 ChannelType = chnlType.Name,
                                 OutletRDs = chnlOtlts.OutletRD,
                                 Outletside = chnlOtlts.ChannelSide,
                                 Village = b.VillageID,
                                 OutletType = outletType.Name,
                                 DesignDischarge = d.DesignDischarge,
                                 MMH = d.OutletMMH,
                                 HeadAboveCrest = d.OutletCrest,
                                 ChannelID = a.ID
                             }).ToList()
                 .Select(u => new
                 {
                     ChannelName = u.ChannelName,
                     ChannelType = u.ChannelType,
                     OutletRDs = Calculations.GetRDText(u.OutletRDs),
                     Outletside = GetOutletside(u.Outletside),
                     PoliceStation = GetPoliceStationName(u.Village),
                     Village = GetVillageName(u.Village),
                     OutletType = u.OutletType,
                     DesignDischarge = GetOutletDischarge(u.DesignDischarge, _OutletID),
                     MMH = GetOutletMMH(u.MMH, _OutletID),
                     HeadAboveCrest = u.HeadAboveCrest,
                     ChannelID = u.ChannelID
                 }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstResult;
        }

        public long GetPageIDByName(string PageName)
        {
            long pageID = -1;
            try
            {
                pageID = (from page in context.UA_Pages
                          where page.Description == PageName
                          select page.ID).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return pageID;
        }

        public long? GetRoleIDOfLoggedOnUser(long UserID)
        {
            long? roleID = -1;
            try
            {
                roleID = (from usr in context.UA_Users
                          where usr.ID == UserID
                          select usr.RoleID).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return roleID;
        }

        public bool? GetUserAccess(long? RoleID, long PageID)
        {
            bool? hasrights = false;
            try
            {
                hasrights = (from rr in context.UA_RoleRights
                             where rr.RoleID == RoleID && rr.PageID == PageID
                             select rr.ViewVisible).FirstOrDefault();


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return hasrights;
        }




        #endregion

        #region Search Criteria For Outlet Performance

        public List<object> GetUserChannles(long UserID)
        {
            List<object> lstChannels = new List<object>();
            List<object> lstChannelsDistinct = new List<object>();
            try
            {
                List<UA_AssociatedLocation> lstLocations = (from assLoc in context.UA_AssociatedLocation
                                                            where assLoc.UserID == UserID
                                                            select assLoc).ToList();

                foreach (var loc in lstLocations)
                {
                    if (loc.IrrigationLevelID == 5) // for section user
                    {
                        List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                     where chnlIrrBndry.SectionID == loc.IrrigationBoundryID
                                                     select chnlIrrBndry.ChannelID).ToList<long?>();

                        foreach (var chnl in lstChannelIDs)
                        {
                            if (chnl.Value != null)
                            {
                                object channelName = (from channel in context.CO_Channel
                                                      where channel.ID == chnl.Value
                                                      select new
                                                      {
                                                          ID = channel.ID,
                                                          Name = channel.NAME
                                                      }).FirstOrDefault();

                                lstChannels.Add(channelName);
                            }
                        }
                    }
                    else if (loc.IrrigationLevelID == 4) // for sub Division
                    {
                        List<long> lstSectionID = (from sec in context.CO_Section
                                                   where sec.SubDivID == loc.IrrigationBoundryID
                                                   select sec.ID).ToList<long>();

                        foreach (var lsec in lstSectionID)
                        {
                            List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                         where chnlIrrBndry.SectionID == lsec
                                                         select chnlIrrBndry.ChannelID).ToList<long?>();

                            foreach (var chnl in lstChannelIDs)
                            {
                                if (chnl.Value != null)
                                {
                                    object channelName = (from channel in context.CO_Channel
                                                          where channel.ID == chnl.Value
                                                          select new
                                                          {
                                                              ID = channel.ID,
                                                              Name = channel.NAME
                                                          }).FirstOrDefault();

                                    lstChannels.Add(channelName);
                                }
                            }
                        }
                    }
                    else if (loc.IrrigationLevelID == 3)
                    {
                        List<long> lstSectionID = (from sec in context.CO_Section
                                                   join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                                   join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                                   where div.ID == loc.IrrigationBoundryID
                                                   select sec.ID).ToList<long>();

                        foreach (var lsec in lstSectionID)
                        {
                            List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                         where chnlIrrBndry.SectionID == lsec
                                                         select chnlIrrBndry.ChannelID).ToList<long?>();

                            foreach (var chnl in lstChannelIDs)
                            {
                                if (chnl.Value != null)
                                {
                                    object channelName = (from channel in context.CO_Channel
                                                          where channel.ID == chnl.Value
                                                          select new
                                                          {
                                                              ID = channel.ID,
                                                              Name = channel.NAME
                                                          }).FirstOrDefault();

                                    lstChannels.Add(channelName);
                                }
                            }
                        }
                    }
                    else if (loc.IrrigationLevelID == 2)
                    {
                        List<long> lstSectionID = (from sec in context.CO_Section
                                                   join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                                   join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                                   join cir in context.CO_Circle on div.CircleID equals cir.ID
                                                   where cir.ID == loc.IrrigationBoundryID
                                                   select sec.ID).ToList<long>();

                        foreach (var lsec in lstSectionID)
                        {
                            List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                         where chnlIrrBndry.SectionID == lsec
                                                         select chnlIrrBndry.ChannelID).ToList<long?>();

                            foreach (var chnl in lstChannelIDs)
                            {
                                if (chnl.Value != null)
                                {
                                    object channelName = (from channel in context.CO_Channel
                                                          where channel.ID == chnl.Value
                                                          select new
                                                          {
                                                              ID = channel.ID,
                                                              Name = channel.NAME
                                                          }).FirstOrDefault();

                                    lstChannels.Add(channelName);
                                }
                            }
                        }
                    }
                    else if (loc.IrrigationLevelID == 1)
                    {
                        List<long> lstSectionID = (from sec in context.CO_Section
                                                   join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                                   join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                                   join cir in context.CO_Circle on div.CircleID equals cir.ID
                                                   join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                                   where zon.ID == loc.IrrigationBoundryID
                                                   select sec.ID).ToList<long>();

                        foreach (var lsec in lstSectionID)
                        {
                            List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                         where chnlIrrBndry.SectionID == lsec
                                                         select chnlIrrBndry.ChannelID).ToList<long?>();

                            foreach (var chnl in lstChannelIDs)
                            {
                                if (chnl.Value != null)
                                {
                                    object channelName = (from channel in context.CO_Channel
                                                          where channel.ID == chnl.Value
                                                          select new
                                                          {
                                                              ID = channel.ID,
                                                              Name = channel.NAME
                                                          }).FirstOrDefault();

                                    lstChannels.Add(channelName);
                                }
                            }
                        }
                    }
                }

                if (lstChannels.Count() > 0)
                {
                    lstChannelsDistinct = lstChannels.Distinct().ToList();
                }


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstChannelsDistinct;
        }

        public List<object> GetSearchResult(long _UserID, long _CommandNameID, long _ChannelTypeID, long _FlowTypeID, long _ChannelNameID)
        {
            List<object> lstchannels = new List<object>();
            List<object> lstChannels = new List<object>();
            List<object> lstChannelsDistinct = new List<object>();

            try
            {

                List<UA_AssociatedLocation> lstLocations = (from assLoc in context.UA_AssociatedLocation
                                                            where assLoc.UserID == _UserID
                                                            select assLoc).ToList();

                foreach (var loc in lstLocations)
                {
                    if (loc.IrrigationLevelID == 5) // for section user
                    {
                        List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                     where chnlIrrBndry.SectionID == loc.IrrigationBoundryID
                                                     select chnlIrrBndry.ChannelID).ToList<long?>();

                        foreach (var chnl in lstChannelIDs)
                        {
                            if (chnl.Value != null)
                            {
                                //object channelName = (from channel in context.CO_Channel
                                //                      where channel.ID == chnl.Value
                                //                      select new
                                //                      {
                                //                          ID = channel.ID,
                                //                          Name = channel.NAME
                                //                      }).FirstOrDefault();

                                //lstChannels.Add(channelName);

                                object channelName = (from channel in context.CO_Channel
                                                      join command in context.CO_ChannelComndType on channel.ComndTypeID equals command.ID
                                                      join chnlType in context.CO_ChannelType on channel.ChannelTypeID equals chnlType.ID
                                                      join flowTyp in context.CO_ChannelFlowType on channel.FlowTypeID equals flowTyp.ID
                                                      where (channel.ID == chnl.Value)
                                                      && (channel.ComndTypeID == _CommandNameID || _CommandNameID == -1)
                                                      && (channel.ChannelTypeID == _ChannelTypeID || _ChannelTypeID == -1)
                                                      && (channel.FlowTypeID == _FlowTypeID || _FlowTypeID == -1)
                                                      && (channel.ID == _ChannelNameID || _ChannelNameID == -1)
                                                      select new
                                                      {
                                                          ID = channel.ID,
                                                          Name = channel.NAME,
                                                          ChannelType = chnlType.Name,
                                                          FlowType = flowTyp.Name,
                                                          TotalRds = channel.TotalRDs,
                                                          CommandName = command.Name
                                                      }).ToList()
                                                         .Select(u => new
                                                         {
                                                             ID = u.ID,
                                                             Name = u.Name,
                                                             ChannelType = u.ChannelType,
                                                             FlowType = u.FlowType,
                                                             TotalRds = Calculations.GetRDText(u.TotalRds),
                                                             CommandName = u.CommandName
                                                         }).FirstOrDefault();

                                if (channelName != null)
                                    lstChannels.Add(channelName);


                            }
                        }
                    }
                    else if (loc.IrrigationLevelID == 4) // for sub Division
                    {
                        List<long> lstSectionID = (from sec in context.CO_Section
                                                   where sec.SubDivID == loc.IrrigationBoundryID
                                                   select sec.ID).ToList<long>();

                        foreach (var lsec in lstSectionID)
                        {
                            List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                         where chnlIrrBndry.SectionID == lsec
                                                         select chnlIrrBndry.ChannelID).ToList<long?>();

                            foreach (var chnl in lstChannelIDs)
                            {
                                if (chnl.Value != null)
                                {
                                    //object channelName = (from channel in context.CO_Channel
                                    //                      where channel.ID == chnl.Value
                                    //                      select new
                                    //                      {
                                    //                          ID = channel.ID,
                                    //                          Name = channel.NAME
                                    //                      }).FirstOrDefault();

                                    //lstChannels.Add(channelName);

                                    object channelName = (from channel in context.CO_Channel
                                                          join command in context.CO_ChannelComndType on channel.ComndTypeID equals command.ID
                                                          join chnlType in context.CO_ChannelType on channel.ChannelTypeID equals chnlType.ID
                                                          join flowTyp in context.CO_ChannelFlowType on channel.FlowTypeID equals flowTyp.ID
                                                          where (channel.ID == chnl.Value)
                                                          && (channel.ComndTypeID == _CommandNameID || _CommandNameID == -1)
                                                          && (channel.ChannelTypeID == _ChannelTypeID || _ChannelTypeID == -1)
                                                          && (channel.FlowTypeID == _FlowTypeID || _FlowTypeID == -1)
                                                          && (channel.ID == _ChannelNameID || _ChannelNameID == -1)
                                                          select new
                                                          {
                                                              ID = channel.ID,
                                                              Name = channel.NAME,
                                                              ChannelType = chnlType.Name,
                                                              FlowType = flowTyp.Name,
                                                              TotalRds = channel.TotalRDs,
                                                              CommandName = command.Name
                                                          }).ToList()
                                                         .Select(u => new
                                                         {
                                                             ID = u.ID,
                                                             Name = u.Name,
                                                             ChannelType = u.ChannelType,
                                                             FlowType = u.FlowType,
                                                             TotalRds = Calculations.GetRDText(u.TotalRds),
                                                             CommandName = u.CommandName
                                                         }).FirstOrDefault();

                                    if (channelName != null)
                                        lstChannels.Add(channelName);

                                }
                            }
                        }
                    }
                    else if (loc.IrrigationLevelID == 3)
                    {
                        List<long> lstSectionID = (from sec in context.CO_Section
                                                   join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                                   join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                                   where div.ID == loc.IrrigationBoundryID
                                                   select sec.ID).Distinct().ToList<long>();

                        foreach (var lsec in lstSectionID)
                        {
                            List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                         where chnlIrrBndry.SectionID == lsec
                                                         select chnlIrrBndry.ChannelID).Distinct().ToList<long?>();



                            foreach (var chnl in lstChannelIDs)
                            {
                                if (chnl.Value != null)
                                {
                                    object channelName = (from channel in context.CO_Channel
                                                          join command in context.CO_ChannelComndType on channel.ComndTypeID equals command.ID
                                                          join chnlType in context.CO_ChannelType on channel.ChannelTypeID equals chnlType.ID
                                                          join flowTyp in context.CO_ChannelFlowType on channel.FlowTypeID equals flowTyp.ID
                                                          where (channel.ID == chnl.Value)
                                                          && (channel.ComndTypeID == _CommandNameID || _CommandNameID == -1)
                                                          && (channel.ChannelTypeID == _ChannelTypeID || _ChannelTypeID == -1)
                                                          && (channel.FlowTypeID == _FlowTypeID || _FlowTypeID == -1)
                                                          && (channel.ID == _ChannelNameID || _ChannelNameID == -1)
                                                          select new
                                                          {
                                                              ID = channel.ID,
                                                              Name = channel.NAME,
                                                              ChannelType = chnlType.Name,
                                                              FlowType = flowTyp.Name,
                                                              TotalRds = channel.TotalRDs,
                                                              CommandName = command.Name
                                                          }).ToList()
                                                          .Select(u => new
                                                            {
                                                                ID = u.ID,
                                                                Name = u.Name,
                                                                ChannelType = u.ChannelType,
                                                                FlowType = u.FlowType,
                                                                TotalRds = Calculations.GetRDText(u.TotalRds),
                                                                CommandName = u.CommandName
                                                            }).FirstOrDefault();

                                    if (channelName != null)
                                        lstChannels.Add(channelName);
                                }
                            }
                        }
                    }
                    else if (loc.IrrigationLevelID == 2)
                    {
                        List<long> lstSectionID = (from sec in context.CO_Section
                                                   join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                                   join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                                   join cir in context.CO_Circle on div.CircleID equals cir.ID
                                                   where cir.ID == loc.IrrigationBoundryID
                                                   select sec.ID).ToList<long>();

                        foreach (var lsec in lstSectionID)
                        {
                            List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                         where chnlIrrBndry.SectionID == lsec
                                                         select chnlIrrBndry.ChannelID).ToList<long?>();

                            foreach (var chnl in lstChannelIDs)
                            {
                                if (chnl.Value != null)
                                {
                                    //object channelName = (from channel in context.CO_Channel
                                    //                      where channel.ID == chnl.Value
                                    //                      select new
                                    //                      {
                                    //                          ID = channel.ID,
                                    //                          Name = channel.NAME
                                    //                      }).FirstOrDefault();

                                    //lstChannels.Add(channelName);

                                    object channelName = (from channel in context.CO_Channel
                                                          join command in context.CO_ChannelComndType on channel.ComndTypeID equals command.ID
                                                          join chnlType in context.CO_ChannelType on channel.ChannelTypeID equals chnlType.ID
                                                          join flowTyp in context.CO_ChannelFlowType on channel.FlowTypeID equals flowTyp.ID
                                                          where (channel.ID == chnl.Value)
                                                          && (channel.ComndTypeID == _CommandNameID || _CommandNameID == -1)
                                                          && (channel.ChannelTypeID == _ChannelTypeID || _ChannelTypeID == -1)
                                                          && (channel.FlowTypeID == _FlowTypeID || _FlowTypeID == -1)
                                                          && (channel.ID == _ChannelNameID || _ChannelNameID == -1)
                                                          select new
                                                          {
                                                              ID = channel.ID,
                                                              Name = channel.NAME,
                                                              ChannelType = chnlType.Name,
                                                              FlowType = flowTyp.Name,
                                                              TotalRds = channel.TotalRDs,
                                                              CommandName = command.Name
                                                          }).ToList()
                                                         .Select(u => new
                                                         {
                                                             ID = u.ID,
                                                             Name = u.Name,
                                                             ChannelType = u.ChannelType,
                                                             FlowType = u.FlowType,
                                                             TotalRds = Calculations.GetRDText(u.TotalRds),
                                                             CommandName = u.CommandName
                                                         }).FirstOrDefault();

                                    if (channelName != null)
                                        lstChannels.Add(channelName);
                                }
                            }
                        }
                    }
                    else if (loc.IrrigationLevelID == 1)
                    {
                        List<long> lstSectionID = (from sec in context.CO_Section
                                                   join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                                   join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                                   join cir in context.CO_Circle on div.CircleID equals cir.ID
                                                   join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                                   where cir.ID == loc.IrrigationBoundryID
                                                   select sec.ID).ToList<long>();

                        foreach (var lsec in lstSectionID)
                        {
                            List<long?> lstChannelIDs = (from chnlIrrBndry in context.CO_ChannelIrrigationBoundaries
                                                         where chnlIrrBndry.SectionID == lsec
                                                         select chnlIrrBndry.ChannelID).ToList<long?>();

                            foreach (var chnl in lstChannelIDs)
                            {
                                if (chnl.Value != null)
                                {
                                    object channelName = (from channel in context.CO_Channel
                                                          join command in context.CO_ChannelComndType on channel.ComndTypeID equals command.ID
                                                          join chnlType in context.CO_ChannelType on channel.ChannelTypeID equals chnlType.ID
                                                          join flowTyp in context.CO_ChannelFlowType on channel.FlowTypeID equals flowTyp.ID
                                                          where (channel.ID == chnl.Value)
                                                          && (channel.ComndTypeID == _CommandNameID || _CommandNameID == -1)
                                                          && (channel.ChannelTypeID == _ChannelTypeID || _ChannelTypeID == -1)
                                                          && (channel.FlowTypeID == _FlowTypeID || _FlowTypeID == -1)
                                                          && (channel.ID == _ChannelNameID || _ChannelNameID == -1)
                                                          select new
                                                          {
                                                              ID = channel.ID,
                                                              Name = channel.NAME,
                                                              ChannelType = chnlType.Name,
                                                              FlowType = flowTyp.Name,
                                                              TotalRds = channel.TotalRDs,
                                                              CommandName = command.Name
                                                          }).ToList()
                                                         .Select(u => new
                                                         {
                                                             ID = u.ID,
                                                             Name = u.Name,
                                                             ChannelType = u.ChannelType,
                                                             FlowType = u.FlowType,
                                                             TotalRds = Calculations.GetRDText(u.TotalRds),
                                                             CommandName = u.CommandName
                                                         }).FirstOrDefault();

                                    if (channelName != null)
                                        lstChannels.Add(channelName);
                                }
                            }
                        }
                    }
                }

                if (lstChannels.Count() > 0)
                {
                    lstChannelsDistinct = lstChannels.Distinct().ToList();
                }



                /////////////////////////////////////////////////////

                //lstchannels = (from chnl in context.CO_Channel
                //               join command in context.CO_ChannelComndType on chnl.ComndTypeID equals command.ID
                //               join chnlType in context.CO_ChannelType on chnl.ChannelTypeID equals chnlType.ID
                //               join flowTyp in context.CO_ChannelFlowType on chnl.FlowTypeID equals flowTyp.ID
                //               where (chnl.ID == ChannelNameID || ChannelNameID == -1)
                //               && (chnl.ComndTypeID == CommandNameID || CommandNameID == -1)
                //               && (chnl.ChannelTypeID == ChannelTypeID || ChannelTypeID == -1)
                //               && (chnl.FlowTypeID == FlowTypeID || FlowTypeID == -1)
                //               select new
                //               {
                //                   ID = chnl.ID,
                //                   Name = chnl.NAME,
                //                   ChannelType = chnlType.Name,
                //                   FlowType = flowTyp.Name,
                //                   TotalRds = chnl.TotalRDs,
                //                   CommandName = command.Name
                //               }).ToList()
                //               .Select(u => new
                //               {
                //                   ID = u.ID,
                //                   Name = u.Name,
                //                   ChannelType = u.ChannelType,
                //                   FlowType = u.FlowType,
                //                   TotalRds = GetRDs(u.TotalRds),
                //                   CommandName = u.CommandName
                //               }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstChannelsDistinct;
        }

        #endregion

        #region Criteria for specific outlet

        public object ChannelInformation(long _ChannelID)
        {
            object lstResult = new object();
            try
            {
                lstResult = (from chnl in context.CO_Channel
                             join chnlType in context.CO_ChannelType on chnl.ChannelTypeID equals chnlType.ID
                             join flowtype in context.CO_ChannelFlowType on chnl.FlowTypeID equals flowtype.ID
                             join command in context.CO_ChannelComndType on chnl.ComndTypeID equals command.ID
                             join chnlIrrBndry in context.CO_ChannelIrrigationBoundaries on chnl.ID equals chnlIrrBndry.ChannelID into A
                             from a in A.DefaultIfEmpty()
                             join sec in context.CO_Section on a.SectionID equals sec.ID
                             where chnl.ID == _ChannelID
                             select new
                             {
                                 ID = chnl.ID,
                                 ChannelName = chnl.NAME,
                                 ChannelType = chnlType.Name,
                                 TotalRDs = chnl.TotalRDs,
                                 FlowType = flowtype.Name,
                                 CommandName = command.Name,
                                 IMISCode = chnl.IMISCode,
                                 SectionName = sec.Name
                             }).ToList()
                            .Select(u => new
                            {
                                u.ID,
                                u.ChannelName,
                                u.ChannelType,
                                TotalRDs = Calculations.GetRDText(u.TotalRDs),
                                u.FlowType,
                                u.CommandName,
                                u.IMISCode,
                                u.SectionName
                            }).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstResult;
        }

        public double? GetSubmergence(double? _H, double? _Y)
        {
            double? Submergence = null;
            try
            {

                if (_H != null && _Y != null)
                {
                    Submergence = _H - _Y;
                }

                //if (H == null)
                //{
                //    Submergence = (Y * -1);
                //}
                //else if (Y == null)
                //{
                //    Submergence = H;
                //}
                //else
                //{
                //    Submergence = H - Y;
                //}
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Submergence;
        }
        public string GetVillageName(long _OutletID)
        {
            string VillageNames = null;
            try
            {
                if (_OutletID != null)
                {
                    List<string> lstVillage = (from chnlOutlet in context.CO_ChannelOutletsLocation
                                               where chnlOutlet.OutletID == _OutletID
                                               select chnlOutlet.CO_Village.Name).ToList();

                    foreach (var vil in lstVillage)
                    {
                        if (VillageNames == null)
                            VillageNames = vil.ToString();
                        else
                            VillageNames = VillageNames + ", " + vil.ToString();
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return VillageNames;
        }

        public string GetDesignDischarge(double _DesignDis)
        {
            string Discharge = null;
            if (_DesignDis != -1)
                Discharge = _DesignDis.ToString();

            return Discharge;
        }
        public List<dynamic> OutletsDetails(long _ChannelID)
        {
            List<dynamic> lstResult = new List<object>();
            try
            {
                lstResult = (from chnl in context.CO_Channel
                             join chnlOtlts in context.CO_ChannelOutlets on chnl.ID equals chnlOtlts.ChannelID into A
                             from a in A.DefaultIfEmpty()
                             join outlHist in context.CO_OutletAlterationHistroy on a.ID equals outlHist.OutletID.Value into B
                             from b in B.DefaultIfEmpty()
                             where chnl.ID == _ChannelID
                             select new
                             {
                                 ID = a.ID == null ? -1 : a.ID,
                                 OutletRD = a.OutletRD,
                                 ChannelSide = a.ChannelSide,
                                 OutletType = b.CO_OutletType.Description,
                                 DesignDischarge = b.DesignDischarge == null ? -1 : b.DesignDischarge,
                                 DesignDiameterWidth = b.OutletWidth,
                                 HeightOfOutlet = b.OutletHeight,
                                 HeadAboveCrest = b.OutletCrest,
                                 CrestReducedLevel = b.OutletCrestRL,
                                 MMH = b.OutletMMH,
                                 WorkingHead = b.OutletWorkingHead
                             }).ToList<dynamic>()
                             .Select(u => new
                            {
                                u.ID,
                                RDSide = Calculations.GetRDText(u.OutletRD) + "/" + u.ChannelSide,
                                u.OutletType,
                                VillageName = GetVillageName(u.ID),
                                DesignDischarge = GetDesignDischarge(u.DesignDischarge),
                                u.DesignDiameterWidth,
                                u.HeightOfOutlet,
                                u.HeadAboveCrest,
                                Submergence = GetSubmergence(u.HeadAboveCrest, u.HeightOfOutlet),
                                u.CrestReducedLevel,
                                u.MMH,
                                u.WorkingHead
                            }).Where(q => q.ID != -1).ToList<dynamic>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstResult;
        }

        #endregion

        #region Outlet History

        public string GetTehsilDistrictVillageName(long? _VillageID, int _Call)
        {
            string Name = "";
            try
            {
                if (_VillageID != null)
                {
                    CO_Village objVillage = (from vil in context.CO_Village
                                             where vil.ID == _VillageID
                                             select vil).FirstOrDefault();

                    if (objVillage != null)
                    {
                        if (_Call == 1)
                            Name = objVillage.Name.ToString();

                        if (_Call == 2 && objVillage.TehsilID != null)
                        {
                            CO_Tehsil objTehsil = (from tehsil in context.CO_Tehsil
                                                   where tehsil.ID == objVillage.TehsilID
                                                   select tehsil).FirstOrDefault();
                            if (objTehsil != null)
                                Name = objTehsil.Name;
                        }
                        else if (_Call == 3 && objVillage.TehsilID != null)
                        {
                            CO_District objDistrict = (from tehsil in context.CO_Tehsil
                                                       join dist in context.CO_District on tehsil.DistrictID equals dist.ID
                                                       where tehsil.ID == objVillage.TehsilID
                                                       select dist).FirstOrDefault();
                            if (objDistrict != null)
                                Name = objDistrict.Name;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Name;
        }
        public long GetChannelID(long _OutletID)
        {
            long ChannelID = -1;
            try
            {
                CO_ChannelOutlets ChannelOutlets = (from gage in context.CO_ChannelOutlets
                                                    where gage.ID == _OutletID
                                                    select gage).FirstOrDefault();

                if (ChannelOutlets != null && ChannelOutlets.ChannelID != null)
                    ChannelID = Convert.ToInt64(ChannelOutlets.ChannelID);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ChannelID;
        }


        public object OutletHistoryInformation(long _ChannelID, long _OutletID)
        {
            object Result = new object();
            try
            {
                Result = (from chnl in context.CO_Channel
                          join chnlOtlts in context.CO_ChannelOutlets on chnl.ID equals chnlOtlts.ChannelID into A
                          from a in A.DefaultIfEmpty()
                          join chnlOutletLoc in context.CO_ChannelOutletsLocation on a.ID equals chnlOutletLoc.OutletID into B
                          from b in B.DefaultIfEmpty()
                          where chnl.ID == _ChannelID && a.ID == _OutletID
                          select new
                          {
                              ChannelName = chnl.NAME,
                              ChannelType = chnl.CO_ChannelType.Name,
                              TotalRDs = chnl.TotalRDs,
                              FlowType = chnl.CO_ChannelFlowType.Name,
                              CommandName = chnl.CO_ChannelComndType.Name,
                              OutletRDs = a.OutletRD,
                              Outletside = a.ChannelSide,
                              VillageID = b.VillageID,
                              IMISCode = chnl.IMISCode,
                              ChannelID = chnl.ID
                          }).ToList()
                 .Select(u => new
                 {
                     u.ChannelName,
                     u.ChannelType,
                     TotalRDs = Calculations.GetRDText(u.TotalRDs),
                     u.FlowType,
                     u.CommandName,
                     OutletRDs = Calculations.GetRDText(u.OutletRDs),
                     Outletside = GetOutletside(u.Outletside),
                     TehsilName = GetTehsilDistrictVillageName(u.VillageID, 2),
                     DistrictName = GetTehsilDistrictVillageName(u.VillageID, 3),
                     PoliceStation = GetPoliceStationName(u.VillageID),
                     VillageName = GetTehsilDistrictVillageName(u.VillageID, 1),
                     u.IMISCode,
                     ChannelID = u.ChannelID
                 }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public string CalculateEfficiencty(double? ObservedDesignDischarge, long OutletID)
        {
            string Efficiency = "";
            try
            {
                CO_OutletAlterationHistroy objOutlet = (from OtltAltHis in context.CO_OutletAlterationHistroy
                                                        where OtltAltHis.OutletID == OutletID
                                                        select OtltAltHis).ToList<CO_OutletAlterationHistroy>().Last();

                if (objOutlet != null && objOutlet.DesignDischarge != 0 && ObservedDesignDischarge != null)
                    Efficiency = Convert.ToString(Math.Round((Convert.ToDecimal((ObservedDesignDischarge / objOutlet.DesignDischarge) * 100)), 2));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Efficiency;
        }


        public List<object> GetHistory(long OutletID, DateTime? FromDate, DateTime? ToDate)
        {
            List<object> lstResult = new List<object>();
            try
            {
                lstResult = (from otltPer in context.CO_ChannelOutletsPerformance
                             where otltPer.OutletID == OutletID
                             && (DbFunctions.TruncateTime(otltPer.ObservationDate) >= FromDate || FromDate == null)
                             && (DbFunctions.TruncateTime(otltPer.ObservationDate) <= ToDate || ToDate == null)
                             select new
                             {
                                 ID = otltPer.ID,
                                 HeightAboveCrest = otltPer.HeadAboveCrest,
                                 WorkingHead = otltPer.WorkingHead,
                                 ObservedDischarge = otltPer.Discharge,
                                 HeightOrifice = otltPer.ObservedHeightY,
                                 DiameterB = otltPer.ObservedWidthB
                             }).ToList().
                             Select(u => new
                             {
                                 u.ID,
                                 u.HeightAboveCrest,
                                 u.WorkingHead,
                                 u.DiameterB,
                                 u.HeightOrifice,
                                 u.ObservedDischarge,
                                 Efficiency = CalculateEfficiencty(u.ObservedDischarge, OutletID)
                             }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstResult;
        }


        #endregion

        #region Daily Gauge Reading Data(Operational Data)
        public List<object> GetDailyGaugeReadingData(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, string _ChannelName, DateTime _Date, int _Session, int _PageIndex, int _PageSize)
        {
            List<dynamic> lstData = context.usp_GetDailyGaugeData(_ZoneID, _CircleID, _DivisionID, _SubDivisionID, _ChannelName, _Date, _Session, _PageIndex, _PageSize)
                                    .ToList()
                                    .Select(g => new
                                    {
                                        RowNumber = g.RowNumber.HasValue ? g.RowNumber.Value : 0,
                                        DailyGaugeReadingID = g.DailyGaugeReadingID.HasValue ? g.DailyGaugeReadingID.Value : 0,
                                        g.SubDivisionName,
                                        g.ChannelName,
                                        Close = g.Close.HasValue ? (g.Close.Value == true ? "Yes" : "No") : "No",
                                        ReadingDateTime = g.ReadingDateTime.HasValue ? Convert.ToString(Utility.GetFormattedTime(g.ReadingDateTime.Value)) : "-",
                                        g.GaugeName,
                                        RDs = Calculations.GetRDText(g.GaugeAtRD),
                                        g.SectionName,
                                        GaugeValue = g.GaugeValue.HasValue ? Convert.ToString(g.GaugeValue.Value) : "-",
                                        DailyDischarge = g.DailyDischarge.HasValue ? Convert.ToString(g.DailyDischarge.Value) : "-",
                                        SubmittedBy = g.SubmittedBy,
                                        GaugePhoto = Utility.IsImageExists(g.GaugePhoto),
                                        g.IsCurrent,
                                        g.GaugeID,
                                        channelNameForAuditTrail = g.ChannelName,
                                        DesignationID = g.DesignationID.HasValue ? g.DesignationID.Value : 0,
                                        TotalRecords = g.TotalRecords.HasValue ? g.TotalRecords.Value : 0,
                                        g.ChannelID,
                                        ReadingDate = g.ReadingDateTime,
                                        g.Designation,
                                        g.GIS_X,
                                        g.GIS_Y
                                    })
                                    .ToList<dynamic>();
            return lstData;
        }
        /// <summary>
        /// //////// Salman's work 
        /// </summary>
        /// <param name="GaugeID"></param>
        /// <param name="Level"></param>
        /// <returns></returns>

        private double? GetBedCrestDischarge(long _GaugeID, long _Level, double _NewGaugeValue)
        {
            double? Discharge = null;
            try
            {
                if (_Level == 2)  //  crest level 
                {
                    CO_ChannelGaugeDTPFall objResult = (from crest in context.CO_ChannelGaugeDTPFall
                                                        where crest.GaugeID == _GaugeID
                                                        orderby crest.ReadingDate descending
                                                        select crest).FirstOrDefault();

                    if (objResult != null)
                    {
                        double C = objResult.DischargeCoefficient;
                        double? B = objResult.BreadthFall == null ? 0 : objResult.BreadthFall;
                        double H = Math.Pow(_NewGaugeValue, 1.5);
                        Discharge = C * Convert.ToDouble(B) * H;
                    }
                }
                else if (_Level == 1) // bed level
                {
                    CO_ChannelGaugeDTPGatedStructure objResult = (from bed in context.CO_ChannelGaugeDTPGatedStructure
                                                                  where bed.GaugeID == _GaugeID
                                                                  orderby bed.ReadingDate descending
                                                                  select bed).FirstOrDefault();

                    if (objResult != null)
                    {
                        if (_NewGaugeValue == 0)
                            return 0;

                        if (objResult.GaugeCorrectionType != null)
                        {
                            if (objResult.GaugeCorrectionType == Constants.GaugeCorrectionSiltedType)
                            {
                                _NewGaugeValue = _NewGaugeValue - Convert.ToDouble(objResult.GaugeValueCorrection);
                            }
                            else
                            {
                                _NewGaugeValue = _NewGaugeValue + Convert.ToDouble(objResult.GaugeValueCorrection);
                            }
                        }

                        if (_NewGaugeValue <= 0)
                        {
                            return 0;
                        }
                        else
                        {
                            double K = objResult.DischargeCoefficient;
                            double N = objResult.ExponentValue;
                            double D = Math.Pow(_NewGaugeValue, N);
                            Discharge = K * D;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Discharge;
        }


        public double? GetTailDischrge(long _GaugeID, double _NewTailGauge)
        {
            double? TailDischarge = 0;
            try
            {
                object chnlGauge = (from gage in context.CO_ChannelGauge
                                    where gage.ID == _GaugeID
                                    select new
                                     {
                                         ChannelID = gage.ChannelID,
                                         TailGauge = gage.GaugeAtRD
                                     }).FirstOrDefault();

                if (chnlGauge != null)
                {
                    long ChannelID = Convert.ToInt64(chnlGauge.GetType().GetProperty("ChannelID").GetValue(chnlGauge));
                    double? TailGauge = Convert.ToInt64(chnlGauge.GetType().GetProperty("TailGauge").GetValue(chnlGauge));


                    List<long?> lstParentIDs = (from chnlPrntFdr in context.CO_ChannelParentFeeder
                                                where chnlPrntFdr.ParrentFeederID == ChannelID && chnlPrntFdr.RelationType == "P"
                                                && chnlPrntFdr.StructureTypeID == 6 && chnlPrntFdr.ParrentFeederRDS == TailGauge
                                                select chnlPrntFdr.ChannelID).ToList();

                    if (lstParentIDs.Count() > 0)
                    {

                        foreach (var parent in lstParentIDs)
                        {
                            long ID = (from gage in context.CO_ChannelGauge
                                       where gage.ChannelID == parent.Value && gage.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge  // only take which are head gauge
                                       select gage.ID).FirstOrDefault();

                            if (ID != 0)
                            {
                                double objDailyDis = (from dailygage in context.CO_ChannelDailyGaugeReading
                                                      where dailygage.GaugeID == ID
                                                      orderby dailygage.ReadingDateTime descending
                                                      select dailygage.DailyDischarge).FirstOrDefault();

                                TailDischarge = TailDischarge + objDailyDis;
                            }
                        }
                    }
                    else
                    {
                        double? objAuthorizedTailDis = (from chnlGage in context.CO_ChannelGauge
                                                        where chnlGage.ID == _GaugeID
                                                        select chnlGage.DesignDischarge).FirstOrDefault();

                        if (objAuthorizedTailDis != null)
                        {
                            double? objAuthorizedTailGauge = (from chnl in context.CO_Channel
                                                              where chnl.ID == ChannelID
                                                              select chnl.AuthorizedTailGauge).FirstOrDefault();

                            if (objAuthorizedTailGauge != null && objAuthorizedTailGauge > 0)
                            {
                                double AuthorizedTailGauge = Convert.ToDouble(objAuthorizedTailGauge);
                                TailDischarge = Convert.ToDouble(objAuthorizedTailDis) * Math.Pow((_NewTailGauge / AuthorizedTailGauge), 1.5);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return TailDischarge == 0 ? null : TailDischarge;
        }



        public object GetChannelLimit(long _GaugeID)
        {
            object objResult = new object();
            try
            {
                objResult = (from gauge in context.CO_ChannelGauge
                             join Chnl in context.CO_Channel on gauge.ChannelID equals Chnl.ID
                             join Chnltype in context.CO_ChannelType on Chnl.ChannelTypeID equals Chnltype.ID
                             where gauge.ID == _GaugeID
                             select new
                             {
                                 MinValue = Chnltype.MinValue,
                                 MaxValue = Chnltype.MaxValue
                             }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objResult;
        }





        public double? CalculateDischarge(long _GaugeID, double _NewDischargeValue)
        {
            double? Discharge = null;

            try
            {
                object objResult = (from gauge in context.CO_ChannelGauge
                                    where gauge.ID == _GaugeID
                                    select new
                                    {
                                        GaugeCategory = gauge.CO_GaugeCategory.ID,
                                        GaugeLevel = gauge.CO_GaugeLevel.ID,
                                    }).FirstOrDefault();

                if (objResult != null)
                {
                    long Category = Convert.ToInt64(objResult.GetType().GetProperty("GaugeCategory").GetValue(objResult));
                    long LevelID = Convert.ToInt64(objResult.GetType().GetProperty("GaugeLevel").GetValue(objResult));

                    if (Category != 2)  // other than tail gauge
                    {
                        Discharge = GetBedCrestDischarge(_GaugeID, LevelID, _NewDischargeValue);
                    }
                    else
                    {
                        Discharge = GetTailDischrge(_GaugeID, _NewDischargeValue);
                    }
                }

                if (Discharge != null)
                    Discharge = Math.Round(Convert.ToDouble(Discharge), 2);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return Discharge;
        }


        public CO_ChannelDailyGaugeReading UpdateDischarge(long ID)
        {
            CO_ChannelDailyGaugeReading CurrValue = new CO_ChannelDailyGaugeReading();
            try
            {
                CurrValue = (from gageReading in context.CO_ChannelDailyGaugeReading
                             where gageReading.ID == ID
                             select gageReading).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return CurrValue;
        }


        public double? GetDefaultDesignDischarge(long GaugeID)
        {
            double? DesignDisc = null;
            try
            {
                DesignDisc = (from gage in context.CO_ChannelGauge
                              where gage.ID == GaugeID
                              select gage.DesignDischarge).FirstOrDefault();


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return DesignDisc;
        }

        public bool HasOffTake(long _GaugeID)
        {
            bool multipleOffTakes = false;
            try
            {
                object objResult = (from gauge in context.CO_ChannelGauge
                                    where gauge.ID == _GaugeID
                                    select new
                                    {
                                        GaugeCategory = gauge.CO_GaugeCategory.ID,
                                        GaugeLevel = gauge.CO_GaugeLevel.ID,
                                        TailGauge = gauge.GaugeAtRD
                                    }).FirstOrDefault();

                if (objResult != null)
                {
                    long Category = Convert.ToInt64(objResult.GetType().GetProperty("GaugeCategory").GetValue(objResult));

                    if (Category == 2)
                    {
                        double Tailgauge = Convert.ToInt64(objResult.GetType().GetProperty("TailGauge").GetValue(objResult));

                        long? ChannelID = (from gage in context.CO_ChannelGauge
                                           where gage.ID == _GaugeID
                                           select gage.ChannelID).FirstOrDefault();

                        if (ChannelID != null)
                        {
                            List<long?> lstParentIDs = (from chnlPrntFdr in context.CO_ChannelParentFeeder
                                                        where chnlPrntFdr.ParrentFeederID == ChannelID && chnlPrntFdr.RelationType == "P"
                                                        && chnlPrntFdr.StructureTypeID == 6 && chnlPrntFdr.ParrentFeederRDS == Tailgauge
                                                        select chnlPrntFdr.ParrentFeederID).ToList();

                            if (lstParentIDs.Count() > 0)
                            {
                                multipleOffTakes = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return multipleOffTakes;
        }

        /// <summary>
        /// //////// salman's work end 
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_Session"></param>
        /// <param name="_DailyGaugeReadingID"></param>
        /// <param name="_PageIndex"></param>
        /// <param name="_PageSize"></param>
        /// <returns></returns>

        public List<object> GetAuditTrail(DateTime _Date, int _Session, long _DailyGaugeReadingID, int _PageIndex, int _PageSize)
        {
            List<dynamic> lstHistoryData = context.usp_GetDailyDataAuditTrail(_Date, _Session, _DailyGaugeReadingID, _PageIndex, _PageSize)
                                    .ToList()
                                    .Select(h => new
                                    {
                                        ReadingDateTime = h.ReadingDateTime.HasValue ? Convert.ToString(Utility.GetFormattedTime(h.ReadingDateTime.Value)) : "-",
                                        GaugeValue = h.GaugeValue.HasValue ? Convert.ToString(h.GaugeValue.Value) : "-",
                                        DailyDischarge = h.DailyDischarge.HasValue ? Convert.ToString(h.DailyDischarge.Value) : "-",
                                        SubmittedBy = h.SubmittedBy,
                                        Designation = h.Designation,
                                        h.ReasonForChange,
                                        h.IsCurrent,
                                        TotalRecords = h.TotalRecords.HasValue ? h.TotalRecords.Value : 0
                                    })
                                    .ToList<dynamic>();


            //var d = new Dictionary<String, Object>();
            //var count = Convert.ToDecimal(lstHistoryData[0].TotalRecords);
            //d.Add("Count", count);
            //d.Add("Data", lstHistoryData);
            //var cpage = Convert.ToDecimal(_PageIndex / _PageSize);
            //d.Add("page", cpage + 1);
            //d.Add("total", Math.Ceiling(Convert.ToDecimal(count / _PageSize)));
            return lstHistoryData;
        }
        //public bool SaveGaugeValue(DateTime _Date, Int64 _GaugeValue, Int64 _ReasonForChangeID, Int64 _GaugeReadingID, bool _IsToUpdate)
        //{
        //    var isSaved = false;
        //    CO_ChannelDailyGaugeReading dailyGaugeReading = new CO_ChannelDailyGaugeReading();
        //    dailyGaugeReading.GaugeValue = _GaugeValue;
        //    dailyGaugeReading.ReadingDateTime = DateTime.Now;
        //    //dailyGaugeReading.ReasonForChangeID = _ReasonForChangeID; // Add this ReasonForChangeID in database

        //    if (_IsToUpdate)
        //    {
        //        dailyGaugeReading.ID = _GaugeReadingID;
        //        context.
        //    }
        //    else
        //    {
        //        context.CO_ChannelDailyGaugeReading.Add(dailyGaugeReading);
        //        context.SaveChanges();
        //        isSaved = true;
        //    }
        //    return isSaved;
        //}

        public Tuple<double?, double?> GetChannelTypeMinMaxValueByChannelID(long _ChannelID)
        {
            CO_ChannelType qChannelTypeMinMaxValues = (from c in context.CO_Channel
                                                       join t in context.CO_ChannelType on c.ChannelTypeID equals t.ID
                                                       where c.ID == _ChannelID
                                                       select t).FirstOrDefault();

            return Tuple.Create(qChannelTypeMinMaxValues.MinValue, qChannelTypeMinMaxValues.MaxValue);
        }
        public DD_GetDailyGaugeReadingNotifyData_Result GetDailyGaugeReadingNotifyData(long _DailyGaugeReadingID)
        {
            DD_GetDailyGaugeReadingNotifyData_Result dailyGaugeReadingNotifyData = context.DD_GetDailyGaugeReadingNotifyData(_DailyGaugeReadingID).FirstOrDefault<DD_GetDailyGaugeReadingNotifyData_Result>();

            return dailyGaugeReadingNotifyData;
        }

        public DD_GetDailyIndentNotifyData_Result GetDailyIndentNotifyData(long _GaugeID)
        {
            DD_GetDailyIndentNotifyData_Result dailyIndentNotifyData = context.DD_GetDailyIndentNotifyData(_GaugeID).FirstOrDefault<DD_GetDailyIndentNotifyData_Result>();

            return dailyIndentNotifyData;
        }
        public List<UA_GetNotificationsRecievers_Result> GetDailyDataNotifyRecievers(long _EventID, long _DailyGaugeReadingID)
        {
            List<DD_GetDailyDataNotifyRecievers_Result> lstDailyDataNotifyRecievers = context.DD_GetDailyDataNotifyRecievers(_EventID, _DailyGaugeReadingID).ToList<DD_GetDailyDataNotifyRecievers_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstDailyDataNotifyRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }

        public List<UA_GetNotificationsRecievers_Result> GetDailyIndentNotifyRecievers(long _EventID, long _UserID)
        {
            List<UA_GetNotificationsRecievers_Result> lstDailyIndentNotifyRecievers = context.UA_GetNotificationsRecievers(_EventID, _UserID).ToList<UA_GetNotificationsRecievers_Result>();

            return lstDailyIndentNotifyRecievers;
        }
        #endregion

        #region Daily Slip Site

        /// <summary>
        /// This function returns data for GaugeSlipSite for a particular StationID
        /// Created On 24-02-2016
        /// </summary>
        /// <param name="_RiverID"></param>
        /// <param name="_Date"></param>
        /// <param name="_LoadLatest"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDailySlipData(long _RiverID, DateTime _Date, bool _LoadLatest = false)
        {
            long Dam = Convert.ToInt64(Constants.StructureType.Dam);

            List<CO_GaugeSlipSite> lstGuageSlipSite = (from gss in context.CO_GaugeSlipSite
                                                       where gss.CO_Station.RiverID == _RiverID && gss.CO_Station.StructureTypeID == Dam && gss.ChannelID == null
                                                       orderby gss.SortOrder
                                                       select gss).ToList();

            List<dynamic> lstDailyGaugeSlip = new List<dynamic>();

            foreach (CO_GaugeSlipSite mdlGaugeSlipSite in lstGuageSlipSite)
            {
                CO_GaugeSlipDailyData mdlGaugeSlipDailyData = (from gsdd in context.CO_GaugeSlipDailyData
                                                               where gsdd.GaugeSlipSiteID == mdlGaugeSlipSite.ID && DbFunctions.TruncateTime(gsdd.ReadingDate) == _Date.Date
                                                               select gsdd).FirstOrDefault();

                CO_ChannelDailyGaugeReading mdlChannelDailyGaugeReading = null;

                if (!_LoadLatest)
                {
                    mdlChannelDailyGaugeReading = (from cdgr in context.CO_ChannelDailyGaugeReading
                                                   where cdgr.GaugeID == mdlGaugeSlipSite.GaugeID && DbFunctions.TruncateTime(cdgr.ReadingDateTime) == _Date.Date
                                                   orderby cdgr.ReadingDateTime descending
                                                   select cdgr).FirstOrDefault();
                }

                if (mdlChannelDailyGaugeReading != null)
                {
                    if (mdlGaugeSlipDailyData != null)
                    {
                        lstDailyGaugeSlip.Add(new
                        {
                            ID = mdlGaugeSlipDailyData.ID,
                            GaugeSlipSiteID = mdlGaugeSlipSite.ID,
                            Name = mdlGaugeSlipSite.Name,
                            AFSQ = mdlGaugeSlipSite.AFSQ,
                            Gauge = (double?)mdlChannelDailyGaugeReading.GaugeValue,
                            Discharge = (double?)mdlChannelDailyGaugeReading.DailyDischarge,
                            MinValueGauge = mdlGaugeSlipSite.MinValueGauge,
                            MaxValueGauge = mdlGaugeSlipSite.MaxValueGauge,
                            MinValueDischarge = mdlGaugeSlipSite.MinValueDischarge,
                            MaxValueDischarge = mdlGaugeSlipSite.MaxValueDischarge,
                            EnableGaugeDischarge = mdlGaugeSlipSite.EnableGaugeDischarge
                        });
                    }
                    else
                    {
                        lstDailyGaugeSlip.Add(new
                        {
                            ID = 0,
                            GaugeSlipSiteID = mdlGaugeSlipSite.ID,
                            Name = mdlGaugeSlipSite.Name,
                            AFSQ = mdlGaugeSlipSite.AFSQ,
                            Gauge = (double?)mdlChannelDailyGaugeReading.GaugeValue,
                            Discharge = (double?)mdlChannelDailyGaugeReading.DailyDischarge,
                            MinValueGauge = mdlGaugeSlipSite.MinValueGauge,
                            MaxValueGauge = mdlGaugeSlipSite.MaxValueGauge,
                            MinValueDischarge = mdlGaugeSlipSite.MinValueDischarge,
                            MaxValueDischarge = mdlGaugeSlipSite.MaxValueDischarge,
                            EnableGaugeDischarge = mdlGaugeSlipSite.EnableGaugeDischarge
                        });
                    }
                }
                else
                {
                    if (mdlGaugeSlipDailyData != null)
                    {
                        lstDailyGaugeSlip.Add(new
                        {
                            ID = mdlGaugeSlipDailyData.ID,
                            GaugeSlipSiteID = mdlGaugeSlipSite.ID,
                            Name = mdlGaugeSlipSite.Name,
                            AFSQ = mdlGaugeSlipSite.AFSQ,
                            Gauge = (double?)mdlGaugeSlipDailyData.DailyGauge,
                            Discharge = (double?)mdlGaugeSlipDailyData.DailyDischarge,
                            MinValueGauge = mdlGaugeSlipSite.MinValueGauge,
                            MaxValueGauge = mdlGaugeSlipSite.MaxValueGauge,
                            MinValueDischarge = mdlGaugeSlipSite.MinValueDischarge,
                            MaxValueDischarge = mdlGaugeSlipSite.MaxValueDischarge,
                            EnableGaugeDischarge = mdlGaugeSlipSite.EnableGaugeDischarge
                        });
                    }
                    else
                    {
                        lstDailyGaugeSlip.Add(new
                        {
                            ID = 0,
                            GaugeSlipSiteID = mdlGaugeSlipSite.ID,
                            Name = mdlGaugeSlipSite.Name,
                            AFSQ = mdlGaugeSlipSite.AFSQ,
                            Gauge = (double?)null,
                            Discharge = (double?)null,
                            MinValueGauge = mdlGaugeSlipSite.MinValueGauge,
                            MaxValueGauge = mdlGaugeSlipSite.MaxValueGauge,
                            MinValueDischarge = mdlGaugeSlipSite.MinValueDischarge,
                            MaxValueDischarge = mdlGaugeSlipSite.MaxValueDischarge,
                            EnableGaugeDischarge = mdlGaugeSlipSite.EnableGaugeDischarge
                        });
                    }
                }
            }

            return lstDailyGaugeSlip;
        }

        /// <summary>
        /// This function returns Station which are barrages or headworks in a particular province and on a specific river
        /// Created On 24-02-2016
        /// </summary>
        /// <param name="_ProvinceID"></param>
        /// <param name="_RiverID"></param>
        /// <param name="_Date"></param>
        /// <param name="_LoadLatest"></param>
        /// <param name="_ShowChenabOtherBarrages"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetStationByProvinceIDAndRiverID(long _ProvinceID, long _RiverID, DateTime _Date, bool _LoadLatest = false, bool _ShowChenabOtherBarrages = false)
        {
            long Barrage = Convert.ToInt64(Constants.StructureType.Barrage);
            long Headworks = Convert.ToInt64(Constants.StructureType.Headwork);
            long Dam = Convert.ToInt64(Constants.StructureType.Dam);
            long HeadRegulator = Convert.ToInt64(Constants.StructureType.HeadRegulator);
            long JhelumRiver = Convert.ToInt64(Constants.River.Jhelum);
            long SutlejRiver = Convert.ToInt64(Constants.River.Sutlej);
            long RaviRiver = Convert.ToInt64(Constants.River.Ravi);

            List<CO_GaugeSlipSite> lstGuageSlipSite = (from gss in context.CO_GaugeSlipSite
                                                       where (gss.CO_Station.ProvinceID == _ProvinceID || _ProvinceID == -1) && gss.CO_Station.RiverID == _RiverID
                                                       orderby gss.SortOrder
                                                       select gss).ToList();

            if (_RiverID == JhelumRiver)
            {
                lstGuageSlipSite = lstGuageSlipSite.Where(gss => (gss.CO_Station.StructureTypeID == Barrage || gss.CO_Station.StructureTypeID == Headworks ||
                    (gss.CO_Station.StructureTypeID == Dam && gss.ChannelID != null))).ToList<CO_GaugeSlipSite>();
            }
            else if (_RiverID == RaviRiver)
            {
                lstGuageSlipSite = lstGuageSlipSite.Where(gss => (gss.CO_Station.StructureTypeID == Barrage || gss.CO_Station.StructureTypeID == Headworks ||
                    gss.CO_Station.StructureTypeID == null)).ToList<CO_GaugeSlipSite>();
            }
            else if (_RiverID == SutlejRiver)
            {
                lstGuageSlipSite = lstGuageSlipSite.Where(gss => (gss.CO_Station.StructureTypeID == Barrage || gss.CO_Station.StructureTypeID == Headworks ||
                    gss.CO_Station.StructureTypeID == HeadRegulator || gss.CO_Station.StructureTypeID == null)).ToList<CO_GaugeSlipSite>();
            }
            else
            {
                if ((bool)_ShowChenabOtherBarrages)
                {
                    lstGuageSlipSite = lstGuageSlipSite.Where(gss => (gss.CO_Station.StructureTypeID != Barrage && gss.CO_Station.StructureTypeID != Headworks &&
                                                           gss.CO_Station.StructureTypeID != Dam)).ToList<CO_GaugeSlipSite>();
                }
                else
                {
                    lstGuageSlipSite = lstGuageSlipSite.Where(gss => (gss.CO_Station.StructureTypeID == Barrage || gss.CO_Station.StructureTypeID == Headworks)).ToList<CO_GaugeSlipSite>();
                }
            }

            List<dynamic> lstDailyGaugeSlip = new List<dynamic>();

            foreach (CO_GaugeSlipSite mdlGaugeSlipSite in lstGuageSlipSite)
            {
                CO_GaugeSlipDailyData mdlGaugeSlipDailyData = (from gsdd in context.CO_GaugeSlipDailyData
                                                               where gsdd.GaugeSlipSiteID == mdlGaugeSlipSite.ID && DbFunctions.TruncateTime(gsdd.ReadingDate) == _Date.Date
                                                               select gsdd).FirstOrDefault();

                CO_ChannelDailyGaugeReading mdlChannelDailyGaugeReading = null;

                if (!_LoadLatest)
                {
                    mdlChannelDailyGaugeReading = (from cdgr in context.CO_ChannelDailyGaugeReading
                                                   where cdgr.GaugeID == mdlGaugeSlipSite.GaugeID && DbFunctions.TruncateTime(cdgr.ReadingDateTime) == _Date.Date
                                                   orderby cdgr.ReadingDateTime descending
                                                   select cdgr).FirstOrDefault();
                }

                CO_ChannelGauge mdlChannelGauge = (from cg in context.CO_ChannelGauge
                                                   where cg.ID == mdlGaugeSlipSite.GaugeID
                                                   select cg).FirstOrDefault();

                CO_ChannelIndent mdlChannelIndent = null;

                if (mdlChannelGauge != null)
                {
                    CO_Section mdlSection = (from s in context.CO_Section
                                             where s.ID == mdlChannelGauge.SectionID
                                             select s).FirstOrDefault();

                    if (mdlSection != null)
                    {
                        mdlChannelIndent = (from ci in context.CO_ChannelIndent
                                            where ci.ParentChannelID == mdlChannelGauge.ChannelID && ci.SubDivID == mdlSection.SubDivID
                                            //orderby ci.FromDate, ci.ID descending
                                            select ci).FirstOrDefault();
                    }
                }

                if (mdlChannelDailyGaugeReading != null)
                {
                    if (mdlGaugeSlipDailyData != null)
                    {
                        lstDailyGaugeSlip.Add(new
                        {
                            ID = mdlGaugeSlipDailyData.ID,
                            GaugeSlipSiteID = mdlGaugeSlipSite.ID,
                            StationName = (mdlGaugeSlipSite.StationID != null ? mdlGaugeSlipSite.CO_Station.Name : String.Empty),
                            ChannelName = (mdlGaugeSlipSite.ChannelID != null ? mdlGaugeSlipSite.CO_Channel.NAME : String.Empty),
                            SiteName = mdlGaugeSlipSite.Name,
                            AFSQ = mdlGaugeSlipSite.AFSQ,
                            GaugeID = (mdlGaugeSlipSite.GaugeID == null ? 0 : mdlGaugeSlipSite.GaugeID),
                            Gauge = (double?)mdlChannelDailyGaugeReading.GaugeValue,
                            Indent = (double?)null,//(mdlChannelIndent != null ? (double?)mdlChannelIndent.IndentValue : null),
                            Discharge = (double?)mdlChannelDailyGaugeReading.DailyDischarge,
                            MinValueGauge = mdlGaugeSlipSite.MinValueGauge,
                            MaxValueGauge = mdlGaugeSlipSite.MaxValueGauge,
                            MinValueDischarge = mdlGaugeSlipSite.MinValueDischarge,
                            MaxValueDischarge = mdlGaugeSlipSite.MaxValueDischarge,
                            EnableGaugeDischarge = mdlGaugeSlipSite.EnableGaugeDischarge
                        });
                    }
                    else
                    {
                        lstDailyGaugeSlip.Add(new
                        {
                            ID = 0,
                            GaugeSlipSiteID = mdlGaugeSlipSite.ID,
                            StationName = (mdlGaugeSlipSite.StationID != null ? mdlGaugeSlipSite.CO_Station.Name : String.Empty),
                            ChannelName = (mdlGaugeSlipSite.ChannelID != null ? mdlGaugeSlipSite.CO_Channel.NAME : String.Empty),
                            SiteName = mdlGaugeSlipSite.Name,
                            AFSQ = mdlGaugeSlipSite.AFSQ,
                            GaugeID = (mdlGaugeSlipSite.GaugeID == null ? 0 : mdlGaugeSlipSite.GaugeID),
                            Gauge = (double?)mdlChannelDailyGaugeReading.GaugeValue,
                            Indent = (double?)null,//(mdlChannelIndent != null ? (double?)mdlChannelIndent.IndentValue : null),
                            Discharge = (double?)mdlChannelDailyGaugeReading.DailyDischarge,
                            MinValueGauge = mdlGaugeSlipSite.MinValueGauge,
                            MaxValueGauge = mdlGaugeSlipSite.MaxValueGauge,
                            MinValueDischarge = mdlGaugeSlipSite.MinValueDischarge,
                            MaxValueDischarge = mdlGaugeSlipSite.MaxValueDischarge,
                            EnableGaugeDischarge = mdlGaugeSlipSite.EnableGaugeDischarge
                        });
                    }
                }
                else
                {
                    if (mdlGaugeSlipDailyData != null)
                    {
                        lstDailyGaugeSlip.Add(new
                        {
                            ID = mdlGaugeSlipDailyData.ID,
                            GaugeSlipSiteID = mdlGaugeSlipSite.ID,
                            StationName = (mdlGaugeSlipSite.StationID != null ? mdlGaugeSlipSite.CO_Station.Name : String.Empty),
                            ChannelName = (mdlGaugeSlipSite.ChannelID != null ? mdlGaugeSlipSite.CO_Channel.NAME : String.Empty),
                            SiteName = mdlGaugeSlipSite.Name,
                            AFSQ = mdlGaugeSlipSite.AFSQ,
                            GaugeID = (mdlGaugeSlipSite.GaugeID == null ? 0 : mdlGaugeSlipSite.GaugeID),
                            Gauge = (double?)mdlGaugeSlipDailyData.DailyGauge,
                            Indent = (double?)null,//(mdlChannelIndent != null ? (double?)mdlChannelIndent.IndentValue : null),
                            Discharge = (double?)mdlGaugeSlipDailyData.DailyDischarge,
                            MinValueGauge = mdlGaugeSlipSite.MinValueGauge,
                            MaxValueGauge = mdlGaugeSlipSite.MaxValueGauge,
                            MinValueDischarge = mdlGaugeSlipSite.MinValueDischarge,
                            MaxValueDischarge = mdlGaugeSlipSite.MaxValueDischarge,
                            EnableGaugeDischarge = mdlGaugeSlipSite.EnableGaugeDischarge
                        });
                    }
                    else
                    {
                        lstDailyGaugeSlip.Add(new
                        {
                            ID = 0,
                            GaugeSlipSiteID = mdlGaugeSlipSite.ID,
                            StationName = (mdlGaugeSlipSite.StationID != null ? mdlGaugeSlipSite.CO_Station.Name : String.Empty),
                            ChannelName = (mdlGaugeSlipSite.ChannelID != null ? mdlGaugeSlipSite.CO_Channel.NAME : String.Empty),
                            SiteName = mdlGaugeSlipSite.Name,
                            AFSQ = mdlGaugeSlipSite.AFSQ,
                            GaugeID = (mdlGaugeSlipSite.GaugeID == null ? 0 : mdlGaugeSlipSite.GaugeID),
                            Gauge = (double?)null,
                            Indent = (double?)null,//(mdlChannelIndent != null ? (double?)mdlChannelIndent.IndentValue : null),
                            Discharge = (double?)null,
                            MinValueGauge = mdlGaugeSlipSite.MinValueGauge,
                            MaxValueGauge = mdlGaugeSlipSite.MaxValueGauge,
                            MinValueDischarge = mdlGaugeSlipSite.MinValueDischarge,
                            MaxValueDischarge = mdlGaugeSlipSite.MaxValueDischarge,
                            EnableGaugeDischarge = mdlGaugeSlipSite.EnableGaugeDischarge
                        });
                    }
                }
            }

            return lstDailyGaugeSlip;
        }

        #endregion

        #region Gauge Info - Mobile
        public List<GetUserGauges_Result> GetUserGauges(long _UserID)
        {
            var q = from g in context.GetUserGauges(_UserID)
                    select g;

            return q.ToList<GetUserGauges_Result>();
        }

        public List<CO_ChannelDailyGaugeReading> CurrentDateGaugeReadingID(DateTime _Date, long _GaugeID, long _UserID)
        {
            var q = from g in context.CO_ChannelDailyGaugeReading
                        .Where(x => x.GaugeID == _GaugeID && DbFunctions.TruncateTime(x.ReadingDateTime) == DbFunctions.TruncateTime(_Date))
                    select g;

            return q.OrderByDescending(x => x.ID).ToList<CO_ChannelDailyGaugeReading>();
        }

        public Double? GetTailGaugeDischargeWithOffTakes(long _ChannelID, int _RDat)
        {
            var q = from g in context.GetTailDischargeFromTailOfftakes(_ChannelID, _RDat) select g;
            if (q == null)
                return null;
            else
                return q.ToList<Double?>().ElementAt(0);

        }
        public List<GetUserGaugesStationBaised_Result> GetUserGaugesByLocation(long _UserID)
        {
            var q = from g in context.GetUserGaugesStationBaised(_UserID)
                    select g;

            return q.ToList<GetUserGaugesStationBaised_Result>();

        }

        public List<GetUserDivisions_Result> GetUserDivisions(long _UserID)
        {
            var q = from g in context.GetUserDivisions(_UserID) select g;
            return q.ToList<GetUserDivisions_Result>();
        }

        public List<GetUserSubDivisions_Result> GetUserSubDivisions(long _UserID)
        {
            var q = from g in context.GetUserSubDivisions(_UserID) select g;
            return q.ToList<GetUserSubDivisions_Result>();
        }

        public List<GetUserSection_Result> GetUserSections(long _UserID)
        {
            var q = from g in context.GetUserSection(_UserID) select g;
            return q.ToList<GetUserSection_Result>();
        }

        public List<GetGauges_Result> GetUserChannelAndGauges(long _UserID)
        {
            var q = from g in context.GetGauges(_UserID) select g;
            return q.ToList<GetGauges_Result>();
        }

        public List<GetDailyGaugeReadingAndroid_Result> GetDailyGaugeReading(long _SectionID, long _ChannelID, DateTime _Date, int _Session)
        {
            var q = from g in context.GetDailyGaugeReadingAndroid(_SectionID, _ChannelID, _Date, _Session) select g;
            return q.ToList<GetDailyGaugeReadingAndroid_Result>();
        }

        /// <summary>
        /// checks is provied user is part of PMIU organization or not
        /// Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        public bool isPMIUStaff(long _UserID)
        {
            bool val = false;
            using (context)
            {
                val = isPMIUStaff(_UserID);
            }
            return val;
        }

        public CO_ChannelDailyGaugeReading GetGaugeReadingByID(long ID)
        {
            CO_ChannelDailyGaugeReading mdlDailyGReading = new CO_ChannelDailyGaugeReading();
            try
            {
                mdlDailyGReading = (from gageReading in context.CO_ChannelDailyGaugeReading
                                    where gageReading.ID == ID
                                    select gageReading).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return mdlDailyGReading;
        }

        /// <summary>
        /// returns list of Outlets againts a channel within a section
        /// Calling a store procedure
        /// Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public List<GetChannelSectionOutlets_Result> GetChannelSectionOutlets(long _SectionID, long _ChannelID)
        {
            var q = from g in context.GetChannelSectionOutlets(_ChannelID, _SectionID) select g;
            return q.ToList<GetChannelSectionOutlets_Result>();
        }

        /// <summary>
        /// Get list of attributes against a barrage ID
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_Date"></param>
        /// <param name="_Session"></param>
        /// <returns></returns>
        public List<GetBarrageDailyDischargeDataMobile_Result> GetBarrageAttributes(long _BarrageID, DateTime _Date)
        {
            var q = from g in context.GetBarrageDailyDischargeDataMobile(_BarrageID, _Date) select g;
            return q.ToList();
        }

        #endregion

        #region Gauge Bulk Entry

        public List<DD_GetGaugesBulkEntry_Result> GetGaugesBulkData(long _SubDivisionID, long _SectionID, int _Session, DateTime _ReadingDate)
        {
            return context.DD_GetGaugesBulkEntry(_SubDivisionID, _SectionID, _Session, _ReadingDate).ToList<DD_GetGaugesBulkEntry_Result>();
        }

        #endregion

        #region MeterReading And Fule
        public List<object> GetMA_ADMUser(long _IrrigationBoundryID, long _Designations)
        {
            List<object> admUser = null;
            if (_Designations == (long)PMIU.WRMIS.Common.Constants.Designation.ADM)
            {
                if (_IrrigationBoundryID == 0)
                {
                    admUser = (from u in context.UA_Users
                               where
                                     u.DesignationID == (long)PMIU.WRMIS.Common.Constants.Designation.ADM
                               select new
                               {
                                   u.ID,
                                   Name = u.FirstName + " " + u.LastName

                               }).ToList<object>();
                }
                else
                {
                    admUser = (from u in context.UA_Users
                               join asl in context.UA_AssociatedLocation on u.ID equals asl.UserID
                               where u.UA_Designations.OrganizationID == (long)PMIU.WRMIS.Common.Constants.Organization.PMIU
                                     && asl.IrrigationLevelID == 3
                                     && asl.IrrigationBoundryID == _IrrigationBoundryID
                                     && asl.DesignationID == (long)PMIU.WRMIS.Common.Constants.Designation.ADM
                               select new
                               {
                                   u.ID,
                                   Name = u.FirstName + " " + u.LastName

                               }).ToList<object>();
                }
            }
            else if (_Designations == (long)PMIU.WRMIS.Common.Constants.Designation.MA)
            {
                if (_IrrigationBoundryID == 0)
                {
                    admUser = (from u in context.UA_Users
                               where
                                   u.DesignationID == (long)PMIU.WRMIS.Common.Constants.Designation.MA

                               select new
                               {
                                   u.ID,
                                   Name = u.FirstName + " " + u.LastName

                               }).ToList<object>();
                }
                else
                {
                    admUser = (from u in context.UA_Users
                               join asl in context.UA_AssociatedLocation on u.ID equals asl.UserID
                               where u.UA_Designations.OrganizationID == (long)PMIU.WRMIS.Common.Constants.Organization.PMIU
                                     && asl.IrrigationLevelID == 3
                                     && asl.IrrigationBoundryID == _IrrigationBoundryID
                                     && asl.DesignationID == (long)PMIU.WRMIS.Common.Constants.Designation.MA
                               select new
                               {
                                   u.ID,
                                   Name = u.FirstName + " " + u.LastName

                               }).ToList<object>();
                }

            }

            return admUser;
        }


        public List<object> GetMAUser(long _UseriD)
        {
            List<object> admUser = null;

            // long? _IrrigationBoundryID = GetID(_UseriD);
            List<long> lstUserID = (from asl in context.UA_UserManager
                                    where asl.ManagerID == _UseriD
                                    select asl.UserID).ToList<long>();

            admUser = (from usr in context.UA_Users
                       where lstUserID.Contains(usr.ID)
                       select new
                        {
                            usr.ID,
                            Name = usr.FirstName + " " + usr.LastName

                        }).ToList<object>();

            return admUser;
            //admUser = (from usr in context.UA_Users
            //           where ((from asl in context.UA_UserManager
            //                   where asl.ManagerID == _UseriD
            //                   select asl.UserID).ToList<long>())

            //           .Contains(usr.ID)
            //           select new
            //           {
            //               usr.ID,
            //               Name = usr.FirstName + " " + usr.LastName

            //           }).ToList<object>();
        }
        public List<object> GetUser(long _UseriD)
        {
            List<object> admUser = null;

            admUser = (from usr in context.UA_Users
                       where usr.ID == _UseriD
                       select new
                       {
                           usr.ID,
                           Name = usr.FirstName + " " + usr.LastName

                       }).ToList<object>();

            return admUser;
        }



        #endregion
        public long? GetID(long _UserID)
        {
            long? Result = (from c in context.UA_AssociatedLocation
                            where c.UserID == _UserID
                            select c.IrrigationBoundryID).FirstOrDefault();

            return Result;
        }
        public int GetChannelParentID(long _ChannelID)
        {
            //string parentstring = "";

            //CO_ChannelParentFeeder parent = (from cpf in context.CO_ChannelParentFeeder
            //                                 where cpf.ChannelID == _ChannelID
            //                                 select cpf).FirstOrDefault();

            //if (parent.CO_Channel.CO_ChannelType.ID == (long)Common.Constants.ChannelType.MainCanal /*Common.Constants.StructureType.Channel*/)
            //{
            //    return parent.ParrentFeederID.Value;
            //}
            //else
            //    return GetChannelParentID(parent.ParrentFeederID.Value);


            int Result = (from c in context.IsChannelClosedNow(_ChannelID, DateTime.Now)
                          select c).ToList().FirstOrDefault().Result.Value; ;

            return Result;


        }

        public bool IsChannelClosedNow(long _ChannelID)
        {
            DateTime Now = DateTime.Now;

            int qChannelClosedCount = (from acpd in context.CW_AnnualClosureProgramDetail
                                       where acpd.ChannelID == _ChannelID
                                       && (acpd.FromDate <= Now && Now <= acpd.ToDate)
                                       select acpd).Count();

            if (qChannelClosedCount > 0)
                return true;
            return false;

        }

        #region Water Theft For Monitoring

        public void GetWaterTheftLoggedRecordsForADMMA(long _ADM, List<long?> _lstMA, DateTime? _FromDate, DateTime? _ToDate)
        {
            var res = (from wt in context.WT_WaterTheftCase
                       where
                       (_ADM == -1 || wt.CreatedBy == _ADM) &&
                       (_lstMA.FirstOrDefault() == -1 || _lstMA.Contains(wt.CreatedBy)) &&
                        wt.CreatedDate >= _FromDate && wt.CreatedDate <= _ToDate
                       select wt).ToList();
        }


        public object GetRotationalDetail(long _CaseID)
        {
            object objResult = (from rp in context.ET_RotationalViolation
                                join usr in context.UA_Users on rp.CreatedBy equals usr.ID
                                where rp.ID == _CaseID
                                select new
                                {
                                    ObservedBy = usr.FirstName + " " + usr.LastName,
                                    Date = rp.MobileEntryDatetime,
                                    HeadGauge = rp.HeadGaugeValue,
                                    Violation = rp.IsViolation == true ? "yes" : "No",
                                    GroupPreference = rp.GroupPreference,
                                    Remarks = rp.Remarks,
                                    Attachment = rp.Attachment,
                                    ChannelName = rp.CO_Channel.NAME,
                                    SectionID = rp.SectionID
                                }).ToList().Select(q => new
                                {
                                    ObservedBy = q.ObservedBy,
                                    Date = q.Date,
                                    HeadGauge = q.HeadGauge,
                                    Violation = q.Violation,
                                    GroupPreference = q.GroupPreference,
                                    Remarks = q.Remarks,
                                    Attachment = q.Attachment,
                                    ChannelName = q.ChannelName,
                                    DivisionName = q.SectionID == null ? "" : GetDivisionName(q.SectionID)
                                }).FirstOrDefault();

            return objResult;
        }

        public string GetDivisionName(long? _SectionID)
        {
            string Name = "";

            Name = (from sec in context.CO_Section
                    where sec.ID == _SectionID
                    select sec.CO_SubDivision.CO_Division.Name).FirstOrDefault();

            //Name = (from chnlgage in context.CO_ChannelGauge
            //        join sec in context.CO_Section on chnlgage.SectionID equals sec.ID
            //        join sdiv in context.CO_SubDivision on sec.SubDivID equals sdiv.ID
            //        join div in context.CO_Division on sdiv.DivisionID equals div.ID
            //        where chnlgage.ChannelID == _ChannelID && chnlgage.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge
            //        select div.Name).FirstOrDefault();
            return Name;
        }


        public object GetLeavesDetail(long _CaseID)
        {
            object objResult = (from leave in context.ET_Leaves
                                join usr in context.UA_Users on leave.CreatedBy equals usr.ID
                                where leave.ID == _CaseID
                                select new
                                {
                                    ObservedBy = usr.FirstName + " " + usr.LastName,
                                    Date = leave.MobileEntryDatetime,
                                    LeaveType = leave.ET_LeaveTypes.Name,
                                    RainGauge = leave.RainGauge,
                                    Remarks = leave.Remarks,
                                    Attachment = leave.Attachment
                                }).ToList().Select(q => new
                                {
                                    ObservedBy = q.ObservedBy,
                                    Date = q.Date,
                                    LeaveType = q.LeaveType,
                                    RainGauge = q.RainGauge == null ? "-" : Convert.ToString(q.RainGauge),
                                    Remarks = q.Remarks,
                                    Attachment = q.Attachment
                                }).FirstOrDefault();

            return objResult;

        }

        public object GetFuelMeterDetail(long _CaseID)
        {
            object objResult = (from vr in context.ET_VehicleReading
                                join usr in context.UA_Users on vr.UserID equals usr.ID
                                where vr.VehicleReadingID == _CaseID
                                select new
                                {
                                    ObservedBy = usr.FirstName + " " + usr.LastName,
                                    Date = vr.ReadingMobileDate,
                                    MeterReading = vr.MeterReading,
                                    Petrol = vr.PetrolQuantity,
                                    Attachment = vr.AttachmentFile1
                                }).ToList().Select(q => new
                                {
                                    ObservedBy = q.ObservedBy,
                                    Date = q.Date,
                                    q.MeterReading,
                                    q.Petrol,
                                    Attachment = q.Attachment
                                }).FirstOrDefault();

            return objResult;

        }


        public string GetGISURL()
        {
            string Link = (from page in context.UA_Pages
                           where page.ModuleID == 6 && page.Description != "#"
                           select page.Description).FirstOrDefault();
            return Link;
        }

        #endregion
        public List<GetChannelSectionOutlet_Result> GetChannelSectionOutlet(long _UserID)
        {
            var q = from g in context.GetChannelSectionOutlet(_UserID) select g;
            return q.ToList<GetChannelSectionOutlet_Result>();
        }
    }
}
