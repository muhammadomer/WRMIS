using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.Channel
{
    public class ChannelRepository : Repository<CO_Channel>
    {
        public ChannelRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<CO_Channel>();
        }
        private string GetChannelSide(string _ChannelSide)
        {
            string Name = string.Empty;
            if (!string.IsNullOrEmpty(_ChannelSide))
            {
                dynamic qChannelSide = CommonLists.GetChannelSides(_ChannelSide)[0];
                Name = Convert.ToString(qChannelSide.GetType().GetProperty("Name").GetValue(qChannelSide, null));
            }
            return Name;
        }
        public List<CO_Channel> GetChannelsByIrrigationBoundary(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID)
        {
            List<CO_Channel> lstChannels = (from c in context.CO_Channel
                                            join ib in context.CO_ChannelIrrigationBoundaries on c.ID equals ib.ChannelID into IBoundary
                                            from irrigationBoundary in IBoundary.DefaultIfEmpty()
                                            join section in context.CO_Section on irrigationBoundary.SectionID equals section.ID into sction
                                            from s in sction.DefaultIfEmpty()
                                            join subDivision in context.CO_SubDivision on s.SubDivID equals subDivision.ID into SD
                                            from sDivision in SD.DefaultIfEmpty()
                                            join division in context.CO_Division on sDivision.DivisionID equals division.ID into dvision
                                            from dv in dvision.DefaultIfEmpty()
                                            join circle in context.CO_Circle on dv.CircleID equals circle.ID into crcel
                                            from cr in crcel.DefaultIfEmpty()
                                            join zone in context.CO_Zone on cr.ZoneID equals zone.ID into zne
                                            from zn in zne.DefaultIfEmpty()
                                            where (zn.ID == _ZoneID || _ZoneID == -1 || _ZoneID == 0)
                                            && (cr.ID == _CircleID || _CircleID == -1 || _CircleID == 0)
                                            && (dv.ID == _DivisionID || _DivisionID == -1 || _DivisionID == 0)
                                            && (sDivision.ID == _SubDivisionID || _SubDivisionID == -1 || _SubDivisionID == 0)


                                            select c).Distinct().OrderBy(c => c.NAME).ToList();

            return lstChannels; ;
        }
        private string GetChannelRelationType(string _ChannelRelationType)
        {
            string Name = string.Empty;
            if (!string.IsNullOrEmpty(_ChannelRelationType))
            {
                dynamic qChannelRelationType = CommonLists.GetChannelRelationshipTypes(_ChannelRelationType)[0];
                Name = Convert.ToString(qChannelRelationType.GetType().GetProperty("Name").GetValue(qChannelRelationType, null));
            }
            return Name;
        }
        public List<dynamic> GetIrrigationBoundaries(long _ChannelID)
        {

            List<dynamic> lstIrrigationBoundaries = (from i in context.CO_ChannelIrrigationBoundaries
                                                     join section in context.CO_Section on i.SectionID equals section.ID
                                                     join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                                     join division in context.CO_Division on subDivision.DivisionID equals division.ID
                                                     where i.ChannelID == _ChannelID
                                                     select new
                                                     {
                                                         ChannelID = i.ChannelID,
                                                         DivisionID = division.ID,
                                                         DivisionName = division.Name,
                                                         SubDivisionID = subDivision.ID,
                                                         SubDivisionName = subDivision.Name,
                                                         SectionID = section.ID,
                                                         SectionName = section.Name,
                                                         SectionRD = i.SectionRD
                                                     }).OrderBy(i => i.SectionRD)
                                     .ToList<dynamic>();

            return lstIrrigationBoundaries;
        }

        #region "Channel Addition"
        /// <summary>
        /// This function check added Channel name already exists in the system.
        /// Created on: 19-10-2015
        /// </summary>
        /// <param name="_ChannelName"></param>
        /// <param name="_ChannelTypeID"></param>
        /// <param name="_ChannelFlowTypeID"></param>
        /// <param name="_ChannelCommandTypeID"></param>
        /// <returns>bool</returns>
        public bool IsChannelExists(CO_Channel _Channel)
        {
            bool qIsChannelExists = false;
            if (_Channel.ID == 0)
            {
                qIsChannelExists = context.CO_Channel.Any(c => c.NAME.ToLower() == _Channel.NAME.ToLower()
                    && c.TotalRDs == _Channel.TotalRDs);
            }
            else
            {

                qIsChannelExists = context.CO_Channel.Any(c => c.NAME.ToLower() == _Channel.NAME.ToLower()
                    && c.TotalRDs == _Channel.TotalRDs
                    && c.ID != _Channel.ID);
            }
            return qIsChannelExists;
        }


        public bool IsChannelIMISExists(CO_Channel _Channel)
        {
            bool qIsChannelExists = false;
            if (_Channel.ID == 0)
            {
                qIsChannelExists = context.CO_Channel.Any(c => c.IMISCode.Trim() == _Channel.IMISCode.Trim());
            }
            else
            {

                qIsChannelExists = context.CO_Channel.Any(c => c.IMISCode.Trim() == _Channel.IMISCode.Trim()
                    && c.ID != _Channel.ID);
            }
            return qIsChannelExists;
        }

        #endregion

        #region "Channel Search"
        public List<object> GetParentChannels()
        {
            List<object> lstParentChannel = null;
            string channelRelation = Convert.ToString(Constants.ChannelRelation.P); // Parent channel

            lstParentChannel = (
                                (from c in context.CO_Channel
                                 join p in context.CO_ChannelParentFeeder
                                 on c.ID equals p.ParrentFeederID
                                 where p.RelationType == channelRelation && p.StructureTypeID == (long)Constants.StructureType.Channel
                                 select new { ID = c.ID, Name = c.NAME, p.StructureTypeID })
                                 .Union
                                 (from s in context.CO_Station
                                  join p in context.CO_ChannelParentFeeder
                                  on s.ID equals p.ParrentFeederID
                                  // where s.StructureTypeID == (long)Constants.StructureType.Barrage
                                  where p.RelationType == channelRelation && p.StructureTypeID != (long)Constants.StructureType.Channel
                                  select new { ID = s.ID, Name = s.Name, p.StructureTypeID })
                                 ).ToList().Select(p => new
                                     {
                                         ID = Convert.ToString(p.ID) + ";" + Convert.ToString(p.StructureTypeID),
                                         Name = p.Name
                                     }).ToList<object>();

            return lstParentChannel;

        }

        public List<object> GetChannelsBySearchCriteria(long _ChannelID, long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _CommandNameID, long _ChannelTypeID, long _FlowTypeID, string _ChannelName, string _IMISCode, long _ParentChannelID, long _StructureTypeID, List<long> _lstUserZone, List<long> _lstUserCircles, List<long> _lstUserDivisions, List<long> _lstUserSubDiv)
        {

            List<long> UserZone = new List<long>();
            List<long> UserCircle = new List<long>();
            List<long> UserDivisions = new List<long>();
            List<long> UserSubDivisions = new List<long>();

            if (_lstUserZone != null)
                UserZone.AddRange(_lstUserZone);

            if (_lstUserCircles != null)
                UserCircle.AddRange(_lstUserCircles);

            if (_lstUserDivisions != null)
                UserDivisions.AddRange(_lstUserDivisions);

            if (_lstUserSubDiv != null)
                UserSubDivisions.AddRange(_lstUserSubDiv);

            if (_ZoneID != -1)
            {
                UserZone.Clear();
                UserZone.Add(_ZoneID); // get selected Zone 
            }
            else
            {
                if (_lstUserZone.Count() == 0)
                {
                    UserZone.Clear();
                    UserZone.Add(-1); // get all zones 
                }
                //else retain user zones
            }

            if (_CircleID != -1)
            {
                UserCircle.Clear();
                UserCircle.Add(_CircleID);
            }
            else
            {
                if (_lstUserCircles.Count() == 0)
                {
                    UserCircle.Clear();
                    UserCircle.Add(-1);
                }
            }

            if (_DivisionID != -1)
            {
                UserDivisions.Clear();
                UserDivisions.Add(_DivisionID);
            }
            else
            {
                if (_lstUserDivisions.Count() == 0)
                {
                    UserDivisions.Clear();
                    UserDivisions.Add(-1);
                }
            }

            if (_SubDivisionID != -1)
            {
                UserSubDivisions.Clear();
                UserSubDivisions.Add(_SubDivisionID);
            }
            else
            {
                if (_lstUserSubDiv.Count() == 0)
                {

                    UserSubDivisions.Clear();
                    UserSubDivisions.Add(-1);
                }
            }

            string channelRelation = Convert.ToString(Constants.ChannelRelation.P);

            //List<object> lstChannelSearch = (from c in context.CO_Channel
            //                                 join t in context.CO_ChannelType on c.ChannelTypeID equals t.ID
            //                                 join f in context.CO_ChannelFlowType on c.FlowTypeID equals f.ID
            //                                 join n in context.CO_ChannelComndType on c.ComndTypeID equals n.ID
            //                                 join p in context.CO_ChannelParentFeeder on c.ID equals p.ChannelID into PF
            //                                 from parentFeeder in PF.DefaultIfEmpty()
            //                                 join ib in context.CO_ChannelIrrigationBoundaries on c.ID equals ib.ChannelID into IBoundary
            //                                 from irrigationBoundary in IBoundary.DefaultIfEmpty()
            //                                 join section in context.CO_Section on irrigationBoundary.SectionID equals section.ID into sction
            //                                 from s in sction.DefaultIfEmpty()
            //                                 join subDivision in context.CO_SubDivision on s.SubDivID equals subDivision.ID into SD
            //                                 from sDivision in SD.DefaultIfEmpty()
            //                                 join division in context.CO_Division on sDivision.DivisionID equals division.ID into dvision
            //                                 from dv in dvision.DefaultIfEmpty()
            //                                 join circle in context.CO_Circle on dv.CircleID equals circle.ID into crcel
            //                                 from cr in crcel.DefaultIfEmpty()
            //                                 join zone in context.CO_Zone on cr.ZoneID equals zone.ID into zne
            //                                 from zn in zne.DefaultIfEmpty()
            //                                 where //pf.RelationType == channelRelation &&
            //                                 (c.ID == _ChannelID || _ChannelID == 0)
            //                                 && (zn.ID == _ZoneID || _ZoneID == -1)
            //                                 && (cr.ID == _CircleID || _CircleID == -1)
            //                                 && (dv.ID == _DivisionID || _DivisionID == -1)
            //                                 && (sDivision.ID == _SubDivisionID || _SubDivisionID == -1)
            //                                 && (t.ID == _ChannelTypeID || _ChannelTypeID == -1)
            //                                 && (f.ID == _FlowTypeID || _FlowTypeID == -1)
            //                                 && (n.ID == _CommandNameID || _CommandNameID == -1)
            //                                 && (parentFeeder.ParrentFeederID == _ParentChannelID || _ParentChannelID == -1)
            //                                 && (parentFeeder.StructureTypeID == _StructureTypeID || _StructureTypeID == -1)
            //                                 && (string.IsNullOrEmpty(_ChannelName) || c.NAME.ToLower().Contains(_ChannelName.ToLower()))
            //                                 && (string.IsNullOrEmpty(_IMISCode.Trim()) || c.IMISCode.Contains(_IMISCode))
            //                                 select new
            //                                 {
            //                                     ChannelID = c.ID
            //                                     ,
            //                                     ChannelName = c.NAME
            //                                     ,
            //                                     ChannelType = t.Name
            //                                     ,
            //                                     FlowType = f.Name
            //                                     ,
            //                                     TotalRDs = c.TotalRDs
            //                                     ,
            //                                     CommandName = n.Name
            //                                     ,
            //                                     CCAAcre = c.CCAAcre
            //                                     ,
            //                                     GCAAcre = c.GCAAcre
            //                                     ,
            //                                     ParentFeederID = f.ID

            //                                 }).Distinct().ToList().Select(c => new
            //                      {
            //                          ChannelID = c.ChannelID
            //                          ,
            //                          ChannelName = c.ChannelName
            //                          ,
            //                          ChannelType = c.ChannelType
            //                          ,
            //                          FlowType = c.FlowType
            //                          ,
            //                          TotalRDs = Calculations.GetRDText(Convert.ToDouble(c.TotalRDs))
            //                          ,
            //                          CommandName = c.CommandName
            //                          ,
            //                          CCAAcre = c.CCAAcre
            //                          ,
            //                          GCAAcre = c.GCAAcre
            //                          ,
            //                          ParentFeederID = c.ParentFeederID

            //                      }).ToList<object>();

            List<object> lstChannelSearch = (from c in context.CO_Channel
                                             join t in context.CO_ChannelType on c.ChannelTypeID equals t.ID
                                             join f in context.CO_ChannelFlowType on c.FlowTypeID equals f.ID
                                             join n in context.CO_ChannelComndType on c.ComndTypeID equals n.ID
                                             join p in context.CO_ChannelParentFeeder on c.ID equals p.ChannelID into PF
                                             from parentFeeder in PF.DefaultIfEmpty()
                                             join ib in context.CO_ChannelIrrigationBoundaries on c.ID equals ib.ChannelID into IBoundary
                                             from irrigationBoundary in IBoundary.DefaultIfEmpty()
                                             join section in context.CO_Section on irrigationBoundary.SectionID equals section.ID into sction
                                             from s in sction.DefaultIfEmpty()
                                             join subDivision in context.CO_SubDivision on s.SubDivID equals subDivision.ID into SD
                                             from sDivision in SD.DefaultIfEmpty()
                                             join division in context.CO_Division on sDivision.DivisionID equals division.ID into dvision
                                             from dv in dvision.DefaultIfEmpty()
                                             join circle in context.CO_Circle on dv.CircleID equals circle.ID into crcel
                                             from cr in crcel.DefaultIfEmpty()
                                             join zone in context.CO_Zone on cr.ZoneID equals zone.ID into zne
                                             from zn in zne.DefaultIfEmpty()
                                             where //pf.RelationType == channelRelation &&
                                             (c.ID == _ChannelID || _ChannelID == 0)
                                                 //&& (zn.ID == _ZoneID || _ZoneID == -1)
                                                 //&& (cr.ID == _CircleID || _CircleID == -1)
                                                 //&& (dv.ID == _DivisionID || _DivisionID == -1)
                                                 //&& (sDivision.ID == _SubDivisionID || _SubDivisionID == -1)
                                             && (UserZone.FirstOrDefault() == -1 || UserZone.Contains(zn.ID))// == _ZoneID || _ZoneID == -1)
                                             && (UserCircle.FirstOrDefault() == -1 || UserCircle.Contains(cr.ID))// == _CircleID || _CircleID == -1)
                                             && (UserDivisions.FirstOrDefault() == -1 || UserDivisions.Contains(dv.ID))// == _DivisionID || _DivisionID == -1)
                                             && (UserSubDivisions.FirstOrDefault() == -1 || UserSubDivisions.Contains(sDivision.ID))// == _SubDivisionID || _SubDivisionID == -1)
                                             && (t.ID == _ChannelTypeID || _ChannelTypeID == -1)
                                             && (f.ID == _FlowTypeID || _FlowTypeID == -1)
                                             && (n.ID == _CommandNameID || _CommandNameID == -1)
                                             && (parentFeeder.ParrentFeederID == _ParentChannelID || _ParentChannelID == -1)
                                             && (parentFeeder.StructureTypeID == _StructureTypeID || _StructureTypeID == -1)
                                             && (string.IsNullOrEmpty(_ChannelName) || c.NAME.ToLower().Contains(_ChannelName.ToLower()))
                                             && (string.IsNullOrEmpty(_IMISCode.Trim()) || c.IMISCode.Contains(_IMISCode))
                                             select new
                                             {
                                                 ChannelID = c.ID
                                                 ,
                                                 ChannelName = c.NAME
                                                 ,
                                                 ChannelType = t.Name
                                                 ,
                                                 FlowType = f.Name
                                                 ,
                                                 TotalRDs = c.TotalRDs
                                                 ,
                                                 CommandName = n.Name
                                                 ,
                                                 CCAAcre = c.CCAAcre
                                                 ,
                                                 GCAAcre = c.GCAAcre
                                                 ,
                                                 ParentFeederID = f.ID

                                             }).Distinct().ToList().Select(c => new
                                             {
                                                 ChannelID = c.ChannelID
                                                 ,
                                                 ChannelName = c.ChannelName
                                                 ,
                                                 ChannelType = c.ChannelType
                                                 ,
                                                 FlowType = c.FlowType
                                                 ,
                                                 TotalRDs = Calculations.GetRDText(Convert.ToDouble(c.TotalRDs))
                                                 ,
                                                 CommandName = c.CommandName
                                                 ,
                                                 CCAAcre = c.CCAAcre
                                                 ,
                                                 GCAAcre = c.GCAAcre
                                                 ,
                                                 ParentFeederID = c.ParentFeederID

                                             }).ToList<object>();

            return lstChannelSearch; 

        }

        #endregion

        #region "Channel Physical Location"
        /// <summary>
        /// This function return Channel Irrigation Boundaries
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIrrigationBoundariesByChannelID(long _ChannelID)
        {
            List<object> lstIrrigationBoundaries = (from IB in context.CO_ChannelIrrigationBoundaries
                                                    join section in context.CO_Section on IB.SectionID equals section.ID
                                                    join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                                    join division in context.CO_Division on subDivision.DivisionID equals division.ID
                                                    join circle in context.CO_Circle on division.CircleID equals circle.ID
                                                    join zone in context.CO_Zone on circle.ZoneID equals zone.ID
                                                    where IB.ChannelID == _ChannelID
                                                    select new
                                                    {
                                                        ID = IB.ID,
                                                        ZoneName = zone.Name,
                                                        ZoneID = zone.ID,
                                                        CircleName = circle.Name,
                                                        CircleID = circle.ID,
                                                        DivisionName = division.Name,
                                                        DivisionID = division.ID,
                                                        SubDivisionName = subDivision.Name,
                                                        SubDivisionID = subDivision.ID,
                                                        SectionName = section.Name,
                                                        SectionID = section.ID,
                                                        SectionRD = IB.SectionRD

                                                    }).ToList().Select(c => new
                                           {
                                               ID = c.ID,
                                               ZoneName = c.ZoneName,
                                               ZoneID = c.ZoneID,
                                               CircleName = c.CircleName,
                                               CircleID = c.CircleID,
                                               DivisionName = c.DivisionName,
                                               DivisionID = c.DivisionID,
                                               SubDivisionName = c.SubDivisionName,
                                               SubDivisionID = c.SubDivisionID,
                                               SectionName = c.SectionName,
                                               SectionID = c.SectionID,
                                               SectionRD = c.SectionRD,
                                               TotalRDs = Calculations.GetRDText(Convert.ToDouble(c.SectionRD))
                                           }).OrderBy(c => c.SectionRD).ToList<object>();
            return lstIrrigationBoundaries;

        }
        /// <summary>
        /// This function return Channel Administrative Boundaries
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByChannelID(long _ChannelID)
        {
            List<object> lstAdminBoundaries = (from admin in context.CO_ChannelAdminBoundries
                                               join village in context.CO_Village on admin.VillageID equals village.ID
                                               join police in context.CO_PoliceStation on admin.PoliceStationID equals police.ID
                                               join tehsil in context.CO_Tehsil on police.TehsilID equals tehsil.ID
                                               join district in context.CO_District on tehsil.DistrictID equals district.ID
                                               where admin.ChannelID == _ChannelID && admin.IsActive.Value == true
                                               select new
                                               {
                                                   ID = admin.ID,
                                                   DistrictID = district.ID,
                                                   DistrictName = district.Name,
                                                   TehsilID = tehsil.ID,
                                                   TehsilName = tehsil.Name,
                                                   PoliceStationID = police.ID,
                                                   PoliceStationName = police.Name,
                                                   VillageID = village.ID,
                                                   VillageName = village.Name,
                                                   FromRD = admin.FromRD,
                                                   ToRD = admin.ToRD,
                                                   ChannelSide = admin.ChannelSide
                                               }).ToList()
                                      .Select(admin => new
                                      {
                                          ID = admin.ID,
                                          DistrictID = admin.DistrictID,
                                          DistrictName = admin.DistrictName,
                                          TehsilID = admin.TehsilID,
                                          TehsilName = admin.TehsilName,
                                          PoliceStationID = admin.PoliceStationID,
                                          PoliceStationName = admin.PoliceStationName,
                                          VillageID = admin.VillageID,
                                          VillageName = admin.VillageName,
                                          FromRDTotal = admin.FromRD,
                                          ToRDTotal = admin.ToRD,
                                          FromRD = Calculations.GetRDText(Convert.ToInt64(admin.FromRD)),
                                          ToRD = Calculations.GetRDText(Convert.ToInt64(admin.ToRD)),
                                          ChannelSideID = admin.ChannelSide,
                                          ChannelSide = GetChannelSide(admin.ChannelSide)
                                      }).ToList<object>();

            return lstAdminBoundaries;
        }
        /// <summary>
        /// This function return Channel Divisions
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByChannelID(long _ChannelID)
        {
            List<CO_Division> lstDivisions = (from i in context.CO_ChannelIrrigationBoundaries
                                              join section in context.CO_Section on i.SectionID equals section.ID
                                              join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                              join division in context.CO_Division on subDivision.DivisionID equals division.ID
                                              where i.ChannelID == _ChannelID
                                              select division).Distinct().ToList();
            return lstDivisions;

        }
        /// <summary>
        /// This function return Channel Districts
        /// Created on: 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByChannelID(long _ChannelID, bool? _IsActive = null)
        {
            List<object> lstDistricts = null;
            List<long> lstDivisionsIDs = GetDivisionsByChannelID(_ChannelID).Select(d => d.ID).ToList<long>();

            if (lstDivisionsIDs != null && lstDivisionsIDs.Count > 0)
            {
                lstDistricts = (from dd in context.CO_DistrictDivision
                                join d in context.CO_District on dd.DistrictID equals d.ID
                                where lstDivisionsIDs.Contains(dd.DivisionID.Value) && (d.IsActive == _IsActive || _IsActive == null)
                                select new
                                {
                                    ID = d.ID,
                                    Name = d.Name
                                }).Distinct().ToList<object>();
            }
            return lstDistricts;

        }
        #endregion

        #region "Gauge Information"
        /// <summary>
        /// This function returns Channel Sub Divisions in Irrigational Boundaries.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<CO_SubDivision>()</returns>
        public List<CO_SubDivision> GetSubDivisionsByChannelID(long _ChannelID, bool? _IsActive = null)
        {
            var lstSubDivisions = (from i in context.CO_ChannelIrrigationBoundaries
                                   join section in context.CO_Section on i.SectionID equals section.ID
                                   join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                   where (i.ChannelID == _ChannelID || _ChannelID == -1)
                                   && (subDivision.IsActive == _IsActive || _IsActive == null)
                                   select subDivision).Distinct().ToList();
            return lstSubDivisions;
        }

        /// <summary>
        /// This function returns Channel Sections in Irrigational Boundaries.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<CO_Section>()</returns>
        public List<CO_Section> GetSectionsBySubDivisionChannelID(long _SubDivisionID, long _ChannelID, bool? _IsActive = null)
        {
            var lstSections = (from i in context.CO_ChannelIrrigationBoundaries
                               join section in context.CO_Section on i.SectionID equals section.ID
                               join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                               where (i.ChannelID == _ChannelID || _ChannelID == -1) && (subDivision.ID == _SubDivisionID || _SubDivisionID == -1)
                               && (section.IsActive == _IsActive || _IsActive == null)
                               select section).Distinct().ToList();
            return lstSections;
        }

        /// <summary>
        /// This function returns Channel Gauge Informations.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<object>()</returns>
        public List<object> GetGaugeInformationsByChannelID(long _ChannelID)
        {
            var lstGaugeInformations = (from gauge in context.CO_ChannelGauge
                                        join section in context.CO_Section on gauge.SectionID equals section.ID
                                        join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                        join gaugeType in context.CO_GaugeType on gauge.GaugeTypeID equals gaugeType.ID into GT
                                        from g in GT.DefaultIfEmpty()
                                        join gaugeCategory in context.CO_GaugeCategory on gauge.GaugeCategoryID equals gaugeCategory.ID
                                        join gaugeLevel in context.CO_GaugeLevel on gauge.GaugeLevelID equals gaugeLevel.ID
                                        where gauge.ChannelID == _ChannelID
                                        orderby gauge.GaugeAtRD ascending
                                        select new
                                        {
                                            ID = gauge.ID,
                                            SubDivisionID = subDivision.ID,
                                            SubDivisionName = subDivision.Name,
                                            SectionID = section.ID,
                                            SectionName = section.Name,
                                            GaugeTypeID = (long?)g.ID,
                                            GaugeTypeName = g.Name,
                                            GaugeCategoryID = gaugeCategory.ID,
                                            GaugeCategoryName = gaugeCategory.Name,
                                            GaugeRD = gauge.GaugeAtRD,
                                            DesignDischarge = gauge.DesignDischarge,
                                            GaugeAtBedID = gaugeLevel.ID,
                                            GaugeAtBedName = gaugeLevel.Name,
                                            DivisionName = subDivision.CO_Division.Name,
                                            DivisionID = subDivision.CO_Division.ID,
                                            CircleID = subDivision.CO_Division.CircleID

                                        }).ToList().Select(gauge => new
                                       {
                                           ID = gauge.ID,
                                           SubDivisionID = gauge.SubDivisionID,
                                           SubDivisionName = gauge.SubDivisionName,
                                           SectionID = gauge.SectionID,
                                           SectionName = gauge.SectionName,
                                           GaugeTypeID = gauge.GaugeTypeID,
                                           GaugeTypeName = gauge.GaugeTypeName,
                                           GaugeCategoryID = gauge.GaugeCategoryID,
                                           GaugeCategoryName = gauge.GaugeCategoryName,
                                           GaugeRD = Calculations.GetRDText(gauge.GaugeRD),
                                           TotalGaugeRD = gauge.GaugeRD,
                                           DesignDischarge = gauge.DesignDischarge,
                                           GaugeAtBedID = gauge.GaugeAtBedID,
                                           GaugeAtBedName = gauge.GaugeAtBedName,
                                           DivisionName = gauge.DivisionName,
                                           DivisionID = gauge.DivisionID,
                                           CircleID = gauge.CircleID

                                       }).ToList<object>();
            return lstGaugeInformations;
        }

        public object GetDivisionNameBySubDivisonID(long _SubDivisionID)
        {
            var qDivisionName = (from i in context.CO_SubDivision
                                 where i.ID == _SubDivisionID
                                 select new
                                 {
                                     DivisionName = i.CO_Division.Name

                                 }).Distinct().SingleOrDefault();
            return qDivisionName;
        }

        public List<dynamic> GetDivisionNameByIrrigationDomain()
        {
            List<dynamic> qDivision_ = (from i in context.CO_Division
                                        where (i.DomainID == (long)Constants.IrrigationDomainID)
                                        select new
                                        {
                                            ID = i.ID,
                                            Name = i.Name
                                        }).OrderBy(x => x.Name).Distinct().OrderBy(x=>x.Name).ToList<object>();

            return qDivision_;
        }
        public List<dynamic> GetDivisionNameByChannel_ID(long _ChannelID)
        {
            List<dynamic> qDivision_ = (from i in context.CO_Channel
                                        join c in  context.CO_ChannelGauge on i.ID equals c.ChannelID
                                        join s in context.CO_Section on c.SectionID equals s.ID
                                        join sd in context.CO_SubDivision on s.SubDivID equals sd.ID
                                        join d in context.CO_Division on sd.DivisionID equals d.ID
                                        where (i.ID == _ChannelID || _ChannelID == -1)
                                        select new
                                        {
                                            ID = d.ID,
                                            Name = d.Name
                                        }).OrderBy(x => x.Name).Distinct().ToList<object>();

            return qDivision_;
        }
        public List<dynamic> GetSubDivisionNameByChannelID(long DivID)
        {

            List<dynamic> qSubDiv = (from i in context.CO_SubDivision
                                     where (i.DivisionID == DivID || DivID == -1)
                                     select new
                                     {
                                         ID = i.ID,
                                         Name = i.Name
                                     }).OrderBy(x => x.Name).Distinct().ToList<object>();

            return qSubDiv;

            //List<dynamic> qSubDiv = (from i in context.CO_ChannelIrrigationBoundaries
            //                         join section in context.CO_Section on i.SectionID equals section.ID
            //                         join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
            //                         where (i.ChannelID == _ChannelID || _ChannelID == -1) && (subDivision.DivisionID == DivID)
            //                         select new
            //                         {
            //                             ID = subDivision.ID,
            //                             Name = subDivision.Name
            //                         }).OrderBy(x => x.Name).Distinct().ToList<object>();

            //return qSubDiv;
        }
        public List<dynamic> GetSectionNameByChannelID(long SubDivID)
        {
            List<dynamic> qSection = (from i in context.CO_Section
                                      where (i.SubDivID == SubDivID || SubDivID == -1)
                                      select new
                                      {
                                          ID = i.ID,
                                          Name = i.Name
                                      }).OrderBy(x => x.Name).Distinct().ToList<object>();

            return qSection;


            //List<dynamic> qSection = (from i in context.CO_ChannelIrrigationBoundaries
            //                          join section in context.CO_Section on i.SectionID equals section.ID
            //                          where (i.ChannelID == _ChannelID || _ChannelID == -1) && (section.SubDivID == SubDivID)
            //                          select new
            //                          {
            //                              ID = section.ID,
            //                              Name = section.Name
            //                          }).OrderBy(x => x.Name).Distinct().ToList<object>();

            //return qSection;
        }

        //    else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
        //    {
        //        qDivision_SubDiv_SectionName = (from i in context.CO_ChannelIrrigationBoundaries
        //                                        where (i.ChannelID == _ChannelID || _ChannelID == -1)
        //                                        select new
        //                                        {
        //                                            ID = i.CO_Section.CO_SubDivision.ID,
        //                                            Name = i.CO_Section.CO_SubDivision.Name
        //                                        }).OrderBy(x => x.Name).ToList<object>();

        //    }
        //    else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
        //    {
        //        qDivision_SubDiv_SectionName = (from i in context.CO_ChannelIrrigationBoundaries
        //                                        where (i.ChannelID == _ChannelID || _ChannelID == -1)
        //                                        select new
        //                                        {
        //                                            ID = i.CO_Section.ID,
        //                                            Name = i.CO_Section.Name
        //                                        }).OrderBy(x => x.Name).ToList<object>();

        //    }
        //    //join section in context.CO_Section on i.SectionID equals section.ID
        //    //join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
        //    //join division in context.CO_Division on subDivision.DivisionID equals division.ID
        //    //where (i.ChannelID == _ChannelID || _ChannelID == -1)
        //    //select new { ID = division.ID, Name = division.Name }).OrderBy()
        //    return qDivision_SubDiv_SectionName;
        //}

        public bool UpdateChannelGaugeByChannelID(long _ID, long? _GaugeDivID, long? _GaugeSubDivID, long? _GaugeSectionID)
        {
            CO_ChannelGauge c = (from x in context.CO_ChannelGauge
                                 where x.ID == _ID
                                 select x).FirstOrDefault();
            c.GaugeDivID = _GaugeDivID;
            c.GaugeSubDivID = _GaugeSubDivID;
            c.GaugeSecID = _GaugeSectionID;
            context.SaveChanges();
            return true;
        }

        #endregion

        #region "Parent Channels and Channel Feeders"
        /// <summary>
        /// This function returns Channel parent feeders
        /// Created on: 10-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetChannelParentFeedersByChannelID(long _ChannelID)
        {
            Int64 structureID = (Int64)Constants.StructureType.Channel;
            List<dynamic> lstChannelParentsFeeders = (from f in context.CO_ChannelParentFeeder
                                                      join c in context.CO_Channel on f.ParrentFeederID equals c.ID
                                                      where f.ChannelID == _ChannelID && f.StructureTypeID == structureID
                                                      select new
                                                      {
                                                          ID = f.ID,
                                                          ParentFeederChannelID = c.ID,
                                                          ParentFeederChannelName = c.NAME,
                                                          RelationshipTypeID = f.RelationType,
                                                          RelationshipTypeName = f.RelationType,
                                                          SideID = f.ChannelSide,
                                                          SideName = f.ChannelSide,
                                                          ChannelRD = f.ChannelRDS,
                                                          ParentFeederChannelRD = f.ParrentFeederRDS,
                                                          StructureTypeID = f.StructureTypeID
                                                      }).
                                       Union(from f in context.CO_ChannelParentFeeder
                                             join s in context.CO_Station on f.ParrentFeederID equals s.ID
                                             where f.ChannelID == _ChannelID && f.StructureTypeID != structureID
                                             select new
                                             {
                                                 ID = f.ID,
                                                 ParentFeederChannelID = s.ID,
                                                 ParentFeederChannelName = s.Name,
                                                 RelationshipTypeID = f.RelationType,
                                                 RelationshipTypeName = f.RelationType,
                                                 SideID = f.ChannelSide,
                                                 SideName = f.ChannelSide,
                                                 ChannelRD = f.ChannelRDS,
                                                 ParentFeederChannelRD = f.ParrentFeederRDS,
                                                 StructureTypeID = f.StructureTypeID
                                             }).ToList<dynamic>();


            List<object> lstChannelParentFeeders = lstChannelParentsFeeders.Select(p => new
            {
                ID = p.ID,
                ParentFeederChannelID = p.ParentFeederChannelID,
                ParentFeederChannelName = p.ParentFeederChannelName,
                RelationshipTypeID = p.RelationshipTypeID,
                RelationshipTypeName = GetChannelRelationType(p.RelationshipTypeName),
                SideID = p.SideID,
                SideName = GetChannelSide(p.SideName),
                TotalChannelRD = p.ChannelRD,
                ChannelRD = Calculations.GetRDText(p.ChannelRD),
                ParentFeederChannelRD = Calculations.GetRDText(p.ParentFeederChannelRD),
                TotalParentFeederChannelRD = p.ParentFeederChannelRD,
                StructureTypeID = p.StructureTypeID
            }).ToList<object>();

            return lstChannelParentFeeders;
        }
        public List<Object> GetStructures()
        {
            List<object> lstStructures = (from s in context.CO_Station
                                          where s.IsActive.Value == true && s.StructureTypeID != (long)Constants.StructureType.River
                                          select new
                                          {
                                              ID = s.ID,
                                              Name = s.Name,
                                              StructureTypeID = s.StructureTypeID
                                          }).ToList()
                                         .Select(c => new
                                         {
                                             ID = Convert.ToString(c.ID) + ";" + Convert.ToString(c.StructureTypeID),
                                             Name = c.Name
                                         }).ToList<object>();
            return lstStructures;
        }
        /// <summary>
        /// This function returns Channel Structures
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public List<object> GetChannelStructures(long _ChannelID)
        {
            List<long> lstChannelDivisions = GetDivisionsByChannelID(_ChannelID).Select(d => d.ID).ToList<long>();

            // Must be removed if Rivers are excluded in parent feeder.
            List<object> lstStructures = GetStructures();

            List<object> lstChannel = (from c in context.CO_Channel
                                       join i in context.CO_ChannelIrrigationBoundaries on c.ID equals i.ChannelID
                                       join section in context.CO_Section on i.SectionID equals section.ID
                                       join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                       join division in context.CO_Division on subDivision.DivisionID equals division.ID
                                       where lstChannelDivisions.Contains(division.ID) && c.ID != _ChannelID
                                       //&&  (
                                       //            c.ChannelTypeID     == (long)Constants.ChannelType.MainCanal 
                                       //         || c.ChannelTypeID  == (long)Constants.ChannelType.LinkCanal
                                       //         || c.ChannelTypeID  == (long)Constants.ChannelType.BranchCanal
                                       //     )    
                                       // where c.ID != _ChannelID
                                       select new
                                       {
                                           ID = c.ID,
                                           Name = c.NAME
                                       }).ToList()
                                       .Select(c => new
                                       {
                                           ID = Convert.ToString(c.ID) + ";" + Convert.ToString((int)Constants.StructureType.Channel),
                                           Name = c.Name
                                       })
                                       .Distinct().ToList<object>();

            List<object> lstParentFeeder = lstChannel.Union(lstStructures).ToList<object>();

            return lstParentFeeder;
        }
        /// <summary>
        /// This function returs Head Gauge Details
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_IrrigationBoundaries"></param>
        /// <returns>List<dynamic></returns>
        private List<dynamic> GetHeadGaugeDetails(List<dynamic> _IrrigationBoundaries)
        {
            // Smallest RDs in Irrigation Boundaries is the Head Gauge
            List<dynamic> lstChannelGaugeDetails = _IrrigationBoundaries.GroupBy(g => new { g.ChannelID, g.DivisionID, g.SubDivisionID, g.SectionID, g.SectionRD })
                                 .Where(h => h.Key.SectionRD == _IrrigationBoundaries.Min(m => m.SectionRD))
                                 .Select(j => new
                                 {
                                     ChannelID = j.Key.ChannelID,
                                     DivisionID = j.Key.DivisionID,
                                     SubDivisionID = j.Key.SubDivisionID,
                                     SectionID = j.Key.SectionID,
                                     SectionRD = j.Key.SectionRD,
                                     GaugeCategoryID = (long)Constants.GaugeCategory.HeadGauge
                                 })
                                .ToList<dynamic>();
            return lstChannelGaugeDetails;
        }
        /// <summary>
        /// This function returns RD at Channel
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public double GetRDAtChannel(long _ChannelID)
        {
            double rdAtChannel = 0;
            List<dynamic> lstIrrigationBoundaries = GetIrrigationBoundaries(_ChannelID);
            //List<dynamic> lstIrrigationBoundaries = (from i in context.CO_ChannelIrrigationBoundaries
            //                                         join section in context.CO_Section on i.SectionID equals section.ID
            //                                         join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
            //                                         join division in context.CO_Division on subDivision.DivisionID equals division.ID
            //                                         where i.ChannelID == _ChannelID
            //                                         select new
            //                                         {
            //                                             ChannelID = i.ChannelID,
            //                                             DivisionID = division.ID,
            //                                             DivisionName = division.Name,
            //                                             SubDivisionID = subDivision.ID,
            //                                             SubDivisionName = subDivision.Name,
            //                                             SectionID = section.ID,
            //                                             SectionName = section.Name,
            //                                             SectionRD = i.SectionRD
            //                                         }).OrderBy(i => i.SectionRD)
            //             .ToList<dynamic>();

            List<dynamic> lstHeadGaugeDetails = GetHeadGaugeDetails(lstIrrigationBoundaries);
            rdAtChannel = lstHeadGaugeDetails[0].SectionRD;
            return rdAtChannel;
        }

        public List<dynamic> GetChannelParentFeedersNameByChannelID(long _ChannelID)
        {
            //Int64 structureID = (Int64)Constants.StructureType.Channel;
            List<dynamic> lstChannelParentsFeeders = (from f in context.CO_ChannelParentFeeder
                                                      join c in context.CO_Channel on f.ParrentFeederID equals c.ID
                                                      where f.ChannelID == _ChannelID
                                                      select new
                                                      {
                                                          ID = f.ParrentFeederID,
                                                          Name = c.NAME
                                                      }).Distinct().ToList<dynamic>();
            return lstChannelParentsFeeders;
        }
        public List<dynamic> BindIMISCode(long _ParentFeederID)
        {
            //Int64 structureID = (Int64)Constants.StructureType.Channel;
            List<dynamic> lstChannelParentsFeeders = (from f in context.CO_ChannelParentFeeder
                                                      join c in context.CO_Channel on f.ChannelID equals c.ID
                                                      where f.ParrentFeederID == _ParentFeederID
                                                      select new
                                                      {
                                                          ID = f.ID,
                                                          ParentFeederChannelID = c.ID,
                                                          ChannelName = c.NAME,
                                                          IMISCode = c.IMISCode
                                                      }).ToList<dynamic>();

            return lstChannelParentsFeeders;
        }

        public string GetStructureTypeID(long _ParentFeederID)
        {
            string IMISCode = string.Empty;
            long? lstChannelParentsFeeders = (from f in context.CO_ChannelParentFeeder
                                              where f.ParrentFeederID == _ParentFeederID
                                              select f.StructureTypeID).FirstOrDefault();

            if (lstChannelParentsFeeders == (Int64)Constants.StructureType.Channel)
            {
                IMISCode = (from c in context.CO_Channel
                            where c.ID == _ParentFeederID
                            select c.IMISCode).FirstOrDefault();
            }
            else
            {
                IMISCode = (from c in context.CO_Station
                            where c.ID == _ParentFeederID
                            select c.IMISCode).FirstOrDefault();
            }
            return IMISCode;
        }
        public bool UpdateIMISCodeChannelID(long _ID, string _IMISCode)
        {
            CO_Channel c = (from x in context.CO_Channel
                            where x.ID == _ID
                            select x).FirstOrDefault();
            c.IMISCode = _IMISCode;
            context.SaveChanges();
            return true;
        }
        #endregion

        public List<object> GetMainBranchLinkChannels(long _DivID)
        {
            List<long?> lstChnlIDs = (from div in context.CO_Division
                                      join sDiv in context.CO_SubDivision on div.ID equals sDiv.DivisionID
                                      join sec in context.CO_Section on sDiv.ID equals sec.SubDivID
                                      join cib in context.CO_ChannelIrrigationBoundaries on sec.ID equals cib.SectionID
                                      where div.ID == _DivID
                                      select cib.ChannelID).Distinct().ToList();

            List<object> lstChnls = (from chnl in context.CO_Channel
                                     where lstChnlIDs.Contains(chnl.ID) &&
                                     (chnl.ChannelTypeID == (long)Constants.ChannelType.MainCanal || chnl.ChannelTypeID == (long)Constants.ChannelType.BranchCanal || chnl.ChannelTypeID == (long)Constants.ChannelType.LinkCanal)
                                     select new
                                     {
                                         ID = chnl.ID,
                                         Name = chnl.NAME
                                     }).ToList<object>();
            return lstChnls;
        }

        //public List<PW_LineDiagram_Result> GetChannelLineDiagram(string _Division, string _Channel, DateTime? _FromDate)
        //{
        //    List<PW_LineDiagram_Result> lstResult = context.PW_LineDiagram(_Division, _Channel, _FromDate).ToList<PW_LineDiagram_Result>();
        //    return lstResult;
        //}


    }
}
