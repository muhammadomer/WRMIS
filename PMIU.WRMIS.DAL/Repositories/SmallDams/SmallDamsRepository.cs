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

namespace PMIU.WRMIS.DAL.Repositories.SmallDams
{
    public class SmallDamsRepository : Repository<SD_SmallDam>
    {
        #region Initialize
        WRMIS_Entities _context;

        public SmallDamsRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<SD_SmallDam>();
            _context = context;
        }
        #endregion


        #region UserControls
        public object GetDamDetails(Int64? _DAMID)
        {
            object DamInfo = (from sd in _context.SD_SmallDam
                              join sdt in context.SD_DamType on sd.DamTypeID equals sdt.ID
                              where sd.ID == _DAMID
                              select new
                              {
                                  DamName = sd.Name,
                                  DamType = sdt.Name
                              }
                           ).Distinct().FirstOrDefault();

            return DamInfo;

        }

        public object GetDamReadingsData(Int64? _DAMID)
        {
            object damReading = (from sd in context.SD_SmallDam
                                 join sdiv in context.CO_SubDivision on sd.SubDivID equals sdiv.ID
                                 join cd in context.CO_Division on sdiv.DivisionID equals cd.ID
                                 where sd.ID == _DAMID
                                 select new
                                 {
                                     Division = cd.Name,
                                     SubDivision = sdiv.Name,
                                     DamName = sd.Name
                                 }).Distinct().FirstOrDefault();
            return damReading;
        }

        public object GetDamChannels(Int64? _ID)
        {
            object DamInfo = (from sdc in _context.SD_SmallChannel
                              where sdc.ID == _ID
                              select new
                              {
                                  Channel = sdc.Name,
                                  ChannelCapacity = sdc.Capacity
                              }
                           ).Distinct().FirstOrDefault();

            return DamInfo;

        }

        public object GetDamInfo(Int64? _DAMID)
        {


            object DamInfo = (from sd in _context.SD_SmallDam
                              join sdt in _context.SD_DamType on sd.DamTypeID equals sdt.ID
                              where sd.ID == _DAMID
                              select new
                              {
                                  DamNameText = sd.Name,
                                  DamTypeText = sdt.Name,
                                  CostProject = sd.Cost,
                                  YearCompletion = sd.CompletionYear

                              }
                           ).Distinct().FirstOrDefault();

            return DamInfo;

        }

        #endregion

        #region VillageType
        public List<object> GetVillageBenefitted(Int64 _ChannelID)
        {
            List<object> lstVillage = (from sc in context.SD_SmallChannel
                                       join sv in context.SD_Village on sc.ID equals sv.SmallChannelID
                                       join cv in context.CO_Village on sv.VillageID equals cv.ID
                                       join ct in context.CO_Tehsil on cv.TehsilID equals ct.ID
                                       join cdis in context.CO_District on ct.DistrictID equals cdis.ID
                                       join cdd in context.CO_DistrictDivision on cdis.ID equals cdd.DistrictID
                                       join cd in context.CO_Division on cdd.DivisionID equals cd.ID
                                       join dom in context.CO_Domain on cd.DomainID equals dom.ID
                                       join cc in context.CO_Circle on cd.CircleID equals cc.ID
                                       join cz in context.CO_Zone on cc.ZoneID equals cz.ID
                                       where dom.ID == 5 && sv.SmallChannelID == _ChannelID
                                       select new
                                       {
                                           sv.ID,
                                           DivisionID = cdd.DivisionID,
                                           DivisionName = cd.Name,
                                           DistrictID = ct.DistrictID,
                                           DistrictName = cdis.Name,
                                           TehsilID = cv.TehsilID,
                                           TehsilName = ct.Name,
                                           VillageID = sv.VillageID,
                                           VillageName = cv.Name,
                                           sv.CreatedBy,
                                           sv.CreatedDate

                                       }).ToList<object>();

            return lstVillage;
        }

        public bool IsVillageTypeUnique(SD_Village _VillageTypeModel)
        {
            bool chkInfrastructure = (from sv in context.SD_Village
                                      where sv.SmallChannelID == _VillageTypeModel.SmallChannelID && sv.VillageID == _VillageTypeModel.VillageID
                                      select sv).Any();

            return chkInfrastructure;
        }

        public List<CO_Division> GetSDDivisionsByUserAndDamID(long _UserID, long _DamID, long _IrrigationLevelID, bool? _IsActive = null)
        {
            List<CO_Division> lstDivision = null;


            List<long?> lstLocationIDs = (from assloc in context.UA_AssociatedLocation
                                          where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
                                          select assloc.IrrigationBoundryID).ToList();

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                var query = from cz in context.CO_Zone
                            join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                            join div in context.CO_Division on cc.ID equals div.CircleID
                            join dom in context.CO_Domain on div.DomainID equals dom.ID
                            join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                            join sd in context.SD_SmallDam on subdiv.ID equals sd.SubDivID
                            where lstLocationIDs.Contains(subdiv.ID) && (div.IsActive == _IsActive || _IsActive == null) && dom.ID == 5 && sd.ID == _DamID
                            select div;

                lstDivision = (from cz in context.CO_Zone
                               join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                               join div in context.CO_Division on cc.ID equals div.CircleID
                               join dom in context.CO_Domain on div.DomainID equals dom.ID
                               join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                               join sd in context.SD_SmallDam on subdiv.ID equals sd.SubDivID
                               where lstLocationIDs.Contains(subdiv.ID) && (div.IsActive == _IsActive || _IsActive == null) && dom.ID == 5 && sd.ID == _DamID
                               select div).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                var query = from cz in context.CO_Zone
                            join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                            join div in context.CO_Division on cc.ID equals div.CircleID
                            join dom in context.CO_Domain on div.DomainID equals dom.ID
                            join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                            join sd in context.SD_SmallDam on subdiv.ID equals sd.SubDivID
                            where lstLocationIDs.Contains(div.ID) && (div.IsActive == _IsActive || _IsActive == null) && dom.ID == 5 && sd.ID == _DamID
                            orderby div.Name
                            select div;

                lstDivision = (from cz in context.CO_Zone
                               join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                               join div in context.CO_Division on cc.ID equals div.CircleID
                               join dom in context.CO_Domain on div.DomainID equals dom.ID
                               join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                               join sd in context.SD_SmallDam on subdiv.ID equals sd.SubDivID
                               where lstLocationIDs.Contains(div.ID) && (div.IsActive == _IsActive || _IsActive == null) && dom.ID == 5 && sd.ID == _DamID
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                lstDivision = (from cz in context.CO_Zone
                               join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                               join div in context.CO_Division on cc.ID equals div.CircleID
                               join dom in context.CO_Domain on div.DomainID equals dom.ID
                               join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                               join sd in context.SD_SmallDam on subdiv.ID equals sd.SubDivID
                               where lstLocationIDs.Contains(cc.ID) && (div.IsActive == _IsActive || _IsActive == null) && dom.ID == 5 && sd.ID == _DamID
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == 0)
            {
                lstDivision = (from cz in context.CO_Zone
                               join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                               join cd in context.CO_Division on cc.ID equals cd.CircleID
                               join dom in context.CO_Domain on cd.DomainID equals dom.ID
                               join subdiv in context.CO_SubDivision on cd.ID equals subdiv.DivisionID
                               join sd in context.SD_SmallDam on subdiv.ID equals sd.SubDivID
                               where (cd.IsActive == _IsActive || _IsActive == null) && dom.ID == 5 && sd.ID == _DamID
                               select cd).ToList();
            }
            return lstDivision;
        }
        #endregion

        #region ReferenceData

        #region DamType
        public List<object> GetDamType()
        {
            List<object> lstDamType = (from sdt in context.SD_DamType
                                       //where sdt.IsActive == true
                                       select new
                                       {
                                           DamTypeID = sdt.ID,
                                           Name = sdt.Name,
                                           Description = sdt.Description,
                                           IsActive = sdt.IsActive,
                                           CreatedBy = sdt.CreatedBy,
                                           CreatedDate = sdt.CreatedDate
                                       }).ToList<object>();

            return lstDamType;

        }

        public bool IsDamTypeUnique(SD_DamType _DamTypeModel)
        {
            bool damType = false;
            if (_DamTypeModel.ID == 0)
            {
                damType = (from sdt in context.SD_DamType
                           where sdt.Name.ToLower() == _DamTypeModel.Name.ToLower()
                           select sdt).Any();
            }
            else
            {
                damType = (from sdt in context.SD_DamType
                           where sdt.Name.ToLower() == _DamTypeModel.Name.ToLower()
                           select sdt).Any();
                if (damType)
                {
                    bool update = context.SD_DamType.Any(dt => dt.ID == _DamTypeModel.ID && dt.Name == _DamTypeModel.Name);
                    if (update)
                    {
                        damType = false;
                    }
                }
            }
            return damType;
        }
        #endregion

        #region SpillwayType
        public List<object> GetSpillwayType()
        {
            List<object> lstSpillwayType = (from sdt in context.SD_SpillwayType
                                            //where sdt.IsActive == true
                                            select new
                                            {
                                                SpillwayTypeID = sdt.ID,
                                                Name = sdt.Name,
                                                Description = sdt.Description,
                                                IsActive = sdt.IsActive,
                                                CreatedBy = sdt.CreatedBy,
                                                CreatedDate = sdt.CreatedDate
                                            }).ToList<object>();

            return lstSpillwayType;

        }

        public bool IsSpillwayTypeUnique(SD_SpillwayType _SpillwayTypeModel)
        {
            bool spillwayType = false;
            if (_SpillwayTypeModel.ID == 0)
            {
                spillwayType = (from sdt in context.SD_SpillwayType
                                where sdt.Name.ToLower() == _SpillwayTypeModel.Name.ToLower()
                                select sdt).Any();
            }
            else
            {
                spillwayType = (from sdt in context.SD_SpillwayType
                                where sdt.Name.ToLower() == _SpillwayTypeModel.Name.ToLower()
                                select sdt).Any();

                if (spillwayType)
                {
                    bool update = context.SD_DamType.Any(dt => dt.ID == _SpillwayTypeModel.ID && dt.Name == _SpillwayTypeModel.Name);
                    if (update)
                    {
                        spillwayType = false;
                    }
                }
            }
            return spillwayType;
        }
        #endregion


        #endregion

        #region Channels


        public List<object> GetChannels(Int64 _SmallDamID)
        {
            List<object> lstChannel = (from ch in context.SD_SmallChannel
                                       where ch.SmallDamID == _SmallDamID
                                       select new
                                       {
                                           ch.ID,
                                           ChannelName = ch.Name,
                                           ParentName = (ch.ParentType == "Dam" ? ch.SD_SmallDam.Name : (
                                               from ch1 in context.SD_SmallChannel
                                               where ch1.SmallDamID == _SmallDamID && ch1.ID == ch.ParentID
                                               select ch1.Name
                                               ).FirstOrDefault()
                                           ),
                                           ChannelCode = ch.ChannelCode,
                                           ChannelCapacity = ch.Capacity,
                                           DesignedCCA = ch.DesignedCCA,
                                           ch.ParentType,
                                           ch.ParentID,
                                           OffTakingRD = ch.ParentRD,
                                           OffTakingSide = ch.ParentSide,
                                           TailRD = ch.TailRD,
                                           ch.AuthorizedGauge,
                                           ch.MaxGauge,
                                           ch.MaxDischarge,
                                           ch.IsActive,
                                           ch.CreatedBy,
                                           ch.CreatedDate,
                                       }).ToList().Select(c => new
                                       {
                                           c.ID,
                                           ChannelName = c.ChannelName,
                                           ParentName = c.ParentName,
                                           ChannelCode = c.ChannelCode,
                                           ChannelCapacity = c.ChannelCapacity,
                                           DesignedCCA = c.DesignedCCA,
                                           c.ParentType,
                                           c.ParentID,
                                           OffTakingRD = Calculations.GetRDText(Convert.ToInt64(c.OffTakingRD)),
                                           OffTakingSide = c.OffTakingSide,
                                           TailRD = Calculations.GetRDText(Convert.ToInt64(c.TailRD)),
                                           c.AuthorizedGauge,
                                           c.MaxGauge,
                                           c.MaxDischarge,
                                           c.IsActive,
                                           c.CreatedBy,
                                           c.CreatedDate,
                                       }).ToList<object>();
            return lstChannel;
        }

        public object GetChannelsByID(Int64 _SmallDamID, Int64 _ChannelID)
        {
            object Channel = (from ch in context.SD_SmallChannel
                              where ch.SmallDamID == _SmallDamID && ch.ID == _ChannelID
                              select new
                              {
                                  ch.ID,
                                  ch.Name,
                                  ch.ChannelCode,
                                  ch.Capacity,
                                  ch.DesignedCCA,
                                  ch.ParentType,
                                  ch.ParentID,
                                  ch.ParentRD,
                                  ParentSide = (ch.ParentSide == null ? "0" : ch.ParentSide),
                                  ch.TailRD,
                                  ch.AuthorizedGauge,
                                  ch.MaxGauge,
                                  ch.MaxDischarge,
                                  ch.IsActive,
                                  ch.CreatedBy,
                                  ch.CreatedDate
                              }).FirstOrDefault();

            return Channel;

        }
        public bool IsChannelsUnique(SD_SmallChannel _Channels)
        {
            bool chkInfrastructure = false;
            if (_Channels.ID == 0)
            {
                chkInfrastructure = (from ch in context.SD_SmallChannel
                                     where ch.ChannelCode.ToLower() == _Channels.ChannelCode.ToLower()
                                     select ch).Any();
            }
            else
            {
                chkInfrastructure = (from ch in context.SD_SmallChannel
                                     where ch.ChannelCode.ToLower() == _Channels.ChannelCode.ToLower()
                                     select ch).Any();

                if (chkInfrastructure)
                {
                    bool update = context.SD_SmallChannel.Any(sc => sc.ID == _Channels.ID && sc.ChannelCode == _Channels.ChannelCode);
                    if (update)
                    {
                        chkInfrastructure = false;
                    }
                }
            }

            return chkInfrastructure;
        }
        #endregion

        #region OMCost
        public List<object> GetOMCost(Int64 _SmallDamID)
        {
            List<object> lstVillageType = (from om in context.SD_OMCost
                                           where om.SmallDamID == _SmallDamID
                                           select new
                                           {
                                               om.ID,
                                               om.SmallDamID,
                                               om.ApprovedDate,
                                               om.Cost,
                                               om.Description,
                                               om.CreatedBy,
                                               om.CreatedDate
                                           }).ToList().Select(c => new
                                           {
                                               c.ID,
                                               c.SmallDamID,
                                               ApprovedDate = String.Format("{0:dd-MMM-yyyy}", c.ApprovedDate),
                                               c.Cost,
                                               c.Description,
                                               c.CreatedBy,
                                               c.CreatedDate
                                           }).ToList<object>();

            return lstVillageType;

        }
        public bool IsOMCostUnique(SD_OMCost _OMCost)
        {
            bool chkInfrastructure = false;
            if (_OMCost.ID == 0)
            {
                chkInfrastructure = (from sdt in context.SD_OMCost
                                     where sdt.ApprovedDate == _OMCost.ApprovedDate && sdt.SmallDamID == _OMCost.SmallDamID
                                     select sdt).Any();
            }
            else
            {
                chkInfrastructure = (from sdt in context.SD_OMCost
                                     where sdt.ApprovedDate == _OMCost.ApprovedDate && sdt.SmallDamID == _OMCost.SmallDamID
                                     select sdt).Any();
                if (chkInfrastructure)
                {
                    bool update = context.SD_OMCost.Any(oc => oc.ID == _OMCost.ID && oc.ApprovedDate == _OMCost.ApprovedDate);
                    if (update)
                    {
                        chkInfrastructure = false;
                    }
                }
            }

            return chkInfrastructure;
        }
        #endregion

        #region SearchDam
        public List<object> GetDamTypeSearch(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, long _UserID, long? _IrrigationLevelID)
        {

            // return context.SDSearch2(_DivisionID, _SubDivisionID, _ID, userID, boundryLvlID).ToList<SDSearch2_Result>();

            List<object> lstDamType = null;

            List<long?> lstLocationIDs = (from assloc in context.UA_AssociatedLocation
                                          where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
                                          select assloc.IrrigationBoundryID).ToList();

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {

                lstDamType = (from sdt in context.SD_SmallDam
                              join dt in context.SD_DamType on sdt.DamTypeID equals dt.ID
                              join sd in context.CO_SubDivision on sdt.SubDivID equals sd.ID
                              join cd in context.CO_Division on sd.DivisionID equals cd.ID
                              join tp in context.SD_DamTechPara on sdt.ID equals tp.SmallDamID into TempData
                              from res in TempData.DefaultIfEmpty()
                              where lstLocationIDs.Contains(sd.ID)
                              && ((sdt.ID == _DamID && _DamID > 0) || _DamID == 0)
                              && ((sdt.SubDivID == _SubDivisionID && _SubDivisionID > 0) || _SubDivisionID == 0)
                              && ((sd.DivisionID == _DivisionID && _DivisionID > 0) || _DivisionID == 0)
                              select new
                              {
                                  sdt.ID,
                                  Division = cd.Name,
                                  SubDivision = sd.Name,
                                  DamName = sdt.Name,
                                  DamType = dt.Name,
                                  Status = (sdt.IsActive == true ? "Yes" : "No"),
                                  TechParaID = (res == null ? 0 : res.ID)

                              }).OrderByDescending(x => x.ID).ToList<object>();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                lstDamType = (from sdt in context.SD_SmallDam
                              join dt in context.SD_DamType on sdt.DamTypeID equals dt.ID
                              join sd in context.CO_SubDivision on sdt.SubDivID equals sd.ID
                              join cd in context.CO_Division on sd.DivisionID equals cd.ID
                              join tp in context.SD_DamTechPara on sdt.ID equals tp.SmallDamID into TempData
                              from res in TempData.DefaultIfEmpty()
                              where lstLocationIDs.Contains(cd.ID)
                              && ((sdt.ID == _DamID && _DamID > 0) || _DamID == 0)
                              && ((sdt.SubDivID == _SubDivisionID && _SubDivisionID > 0) || _SubDivisionID == 0)
                              && ((sd.DivisionID == _DivisionID && _DivisionID > 0) || _DivisionID == 0)
                              select new
                              {
                                  sdt.ID,
                                  Division = cd.Name,
                                  SubDivision = sd.Name,
                                  DamName = sdt.Name,
                                  DamType = dt.Name,
                                  Status = (sdt.IsActive == true ? "Yes" : "No"),
                                  TechParaID = (res == null ? 0 : res.ID)

                              }).OrderByDescending(x => x.ID).ToList<object>();
            }
            else if (_IrrigationLevelID == 0)
            {
                lstDamType = (from sdt in context.SD_SmallDam
                              join dt in context.SD_DamType on sdt.DamTypeID equals dt.ID
                              join sd in context.CO_SubDivision on sdt.SubDivID equals sd.ID
                              join cd in context.CO_Division on sd.DivisionID equals cd.ID
                              join tp in context.SD_DamTechPara on sdt.ID equals tp.SmallDamID into TempData
                              from res in TempData.DefaultIfEmpty()
                              where ((sdt.ID == _DamID && _DamID > 0) || _DamID == 0)
                              && ((sdt.SubDivID == _SubDivisionID && _SubDivisionID > 0) || _SubDivisionID == 0)
                              && ((sd.DivisionID == _DivisionID && _DivisionID > 0) || _DivisionID == 0)
                              select new
                              {
                                  sdt.ID,
                                  Division = cd.Name,
                                  SubDivision = sd.Name,
                                  DamName = sdt.Name,
                                  DamType = dt.Name,
                                  Status = (sdt.IsActive == true ? "Yes" : "No"),
                                  TechParaID = (res == null ? 0 : res.ID)

                              }).OrderByDescending(x => x.ID).ToList<object>();
            }
            return lstDamType;
        }

        public List<CO_Division> GetSDDivisionsByUserID(long _UserID, long _IrrigationLevelID, bool? _IsActive = null)
        {
            List<CO_Division> lstDivision = null;


            List<long?> lstLocationIDs = (from assloc in context.UA_AssociatedLocation
                                          where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
                                          select assloc.IrrigationBoundryID).ToList();

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {

                lstDivision = (from cz in context.CO_Zone
                               join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                               join div in context.CO_Division on cc.ID equals div.CircleID
                               join dom in context.CO_Domain on div.DomainID equals dom.ID
                               join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                               where lstLocationIDs.Contains(subdiv.ID) && (div.IsActive == _IsActive || _IsActive == null) && dom.ID == 5
                               select div).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {

                lstDivision = (from cz in context.CO_Zone
                               join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                               join div in context.CO_Division on cc.ID equals div.CircleID
                               join dom in context.CO_Domain on div.DomainID equals dom.ID
                               where lstLocationIDs.Contains(div.ID) && (div.IsActive == _IsActive || _IsActive == null) && dom.ID == 5
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                lstDivision = (from cz in context.CO_Zone
                               join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                               join div in context.CO_Division on cc.ID equals div.CircleID
                               join dom in context.CO_Domain on div.DomainID equals dom.ID
                               where lstLocationIDs.Contains(cc.ID) && (div.IsActive == _IsActive || _IsActive == null) && dom.ID == 5
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == 0)
            {
                lstDivision = (from cz in context.CO_Zone
                               join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                               join cd in context.CO_Division on cc.ID equals cd.CircleID
                               join dom in context.CO_Domain on cd.DomainID equals dom.ID
                               where (cd.IsActive == _IsActive || _IsActive == null) && dom.ID == 5
                               select cd).ToList();
            }
            return lstDivision;
        }

        public List<CO_District> GetAllSDDistricts(long _DivisionID)
        {
            List<CO_District> SDDistrict = (from cz in context.CO_Zone
                                            join cc in context.CO_Circle on cz.ID equals cc.ZoneID
                                            join cd in context.CO_Division on cc.ID equals cd.CircleID
                                            join dom in context.CO_Domain on cd.DomainID equals dom.ID
                                            join cdd in context.CO_DistrictDivision on cd.ID equals cdd.DivisionID
                                            join cdis in context.CO_District on cdd.DistrictID equals cdis.ID
                                            where cd.ID == _DivisionID && dom.ID == 5
                                            select cdis).ToList();
            return SDDistrict;
        }


        #endregion

        #region AddDam
        public object GetSmallDamByID(Int64 _DamID)
        {
            object smallDam = null;

            smallDam = (from sd in context.SD_SmallDam
                        where sd.ID == _DamID
                        select new
                        {
                            ID = sd.ID,
                            Name = sd.Name,
                            DamTypeID = sd.DamTypeID,
                            Cost = sd.Cost,
                            CompletionYear = sd.CompletionYear,
                            Description = sd.Description,
                            SubDivID = sd.SubDivID,
                            VillageID = sd.VillageID,
                            Road = sd.Road,
                            Location = sd.Location,
                            StreamNullah = sd.StreamNullah,
                            NA = sd.NA,
                            PP = sd.PP,
                            UC = sd.UC,
                            IsActive = sd.IsActive,
                            DivisionID = sd.CO_SubDivision.DivisionID,
                            TehsilID = sd.CO_Village.TehsilID,
                            DistrictID = sd.CO_Village.CO_Tehsil.DistrictID

                        }).Distinct().FirstOrDefault();


            return smallDam;
        }

        public bool ISDamExist(SD_SmallDam _Dam)
        {
            bool Exits = false;

            if (_Dam.ID == 0)
            {
                Exits = (from sd in context.SD_SmallDam
                         where sd.Name == _Dam.Name && sd.SubDivID == _Dam.SubDivID
                         select sd).Any();
            }
            else
            {
                Exits = (from sd in context.SD_SmallDam
                         where sd.Name == _Dam.Name && sd.SubDivID == _Dam.SubDivID
                         select sd).Any();

                if (Exits)
                {
                    bool update = context.SD_SmallDam.Any(sd => sd.ID == _Dam.ID && sd.Name == _Dam.Name && sd.SubDivID == _Dam.SubDivID);
                    if (update)
                    {
                        Exits = false;
                    }
                }
            }
            return Exits;
        }
        #endregion

        #region Technical
        public object GetTechParaByID(Int64 _SmallDamID, Int64 _TechParaID)
        {
            object smallDam = (from stp in context.SD_DamTechPara
                               where stp.ID == _TechParaID && stp.SmallDamID == _SmallDamID
                               select new
                               {
                                   stp.ID,
                                   stp.SmallDamID,
                                   stp.MaxHeight,
                                   stp.DamLength,
                                   stp.TopRL,
                                   stp.CatchmentArea,
                                   stp.GSC,
                                   stp.LSC,
                                   stp.PondLevel,
                                   stp.HFL,
                                   stp.DesignedCA,
                                   stp.SpillwayTypeID,
                                   stp.SpillwayLength,
                                   stp.SpillwayDesignDischarge,
                                   stp.ClearWaterWay,
                                   stp.WaterSupply,
                                   stp.LandAcquiredPond,
                                   stp.LandAcquiredChannel,
                                   stp.CreatedBy,
                                   stp.CreatedDate
                               }).Distinct().FirstOrDefault();


            return smallDam;
        }
        #endregion

        #region Reading

        #region SearchReadings


        public List<SDChannelReading1_Result> GetChannelReading(Int64? _ID, Int64? _DivisionID, Int64? _SubDivisionID, DateTime _ReadingDate)
        {

            return context.SDChannelReading1(_ID, _DivisionID, _SubDivisionID, _ReadingDate).ToList<SDChannelReading1_Result>();
        }

        public SDDamReading_Result GetDamReading(Int64? _ID, Int64? _DivisionID, Int64? _SubDivisionID, DateTime _ReadingDate)
        {

            return context.SDDamReading(_ID, _DivisionID, _SubDivisionID, _ReadingDate).FirstOrDefault<SDDamReading_Result>();
        }


        public object GetSmallDamReadingByID(Int64 _DamDataID)
        {
            object smallDam = null;

            smallDam = (from sdd in context.SD_SmallDamData
                        where sdd.ID == _DamDataID
                        select new
                        {
                            sdd.ID,
                            sdd.SmallDamID,
                            sdd.DamLevel,
                            sdd.Discharge,
                            sdd.LiveStorage,
                            sdd.ReadingDate,
                            sdd.ReaderName,
                            sdd.Remarks,
                            sdd.CreatedBy,
                            sdd.CreatedDate
                        }).Distinct().FirstOrDefault();


            return smallDam;
        }

        public Int64 DamIsExits(Int64 _DamID, DateTime _ReadingDate)
        {
            Int64 DamData = (from sdd in context.SD_SmallDamData
                             where sdd.SmallDamID == _DamID && sdd.ReadingDate == _ReadingDate
                             select (sdd.ID == null ? 0 : sdd.ID)).FirstOrDefault();

            return DamData;

        }
        #endregion
        #region ViewReadings
        public List<SDViewChannelReading_Result> GetChannelReadingView(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, Int64? _ChannelID, DateTime _FromDate, DateTime _ToDate)
        {

            return context.SDViewChannelReading(_DamID, _DivisionID, _SubDivisionID, _ChannelID, _FromDate, _ToDate).ToList<SDViewChannelReading_Result>();
        }
        public List<SDViewDamReading_Result> GetDamReadingView(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, DateTime _FromDate, DateTime _ToDate)
        {

            return context.SDViewDamReading(_DamID, _DivisionID, _SubDivisionID, _FromDate, _ToDate).ToList<SDViewDamReading_Result>();
        }
        #endregion
        #endregion
    }
}
