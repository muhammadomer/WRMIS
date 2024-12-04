using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.ReferenceData
{
    class InfrastructureRepository : Repository<FO_ProtectionInfrastructure>
    {
        public InfrastructureRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<FO_ProtectionInfrastructure>();
        }

        #region "Infrastructure Search"
        public List<object> GetInfrastructureBySearchCriteria(long _InfrastructureID
          , long _ZoneID
          , long _CircleID
          , long _DivisionID
          , Int16 _InfrastructureTypeID
          , string _InfrastructureName
          , Int64 _InfrastructureStatus)
        {

            List<object> lstInfrastructureSearch =
                //(from pi in context.FO_ProtectionInfrastructure
                // join it in context.FO_InfrastructureType on pi.InfrastructureTypeID equals it.ID
                // where
                // ((string.IsNullOrEmpty(_InfrastructureName.Trim()) || pi.InfrastructureName.Contains(_InfrastructureName))
                // && (pi.InfrastructureTypeID == ((_InfrastructureTypeID == -1) ? pi.InfrastructureTypeID : _InfrastructureTypeID))
                // && (pi.IsActive == ((_InfrastructureStatus == -1) ? pi.IsActive : (_InfrastructureStatus == 1 ? true : false)))
                // )
                                                    (from pi in context.FO_ProtectionInfrastructure
                                                     join it in context.CO_StructureType on pi.InfrastructureTypeID equals it.ID  //into tempPI
                                                     //join ib in context.FO_StructureIrrigationBoundaries on pi.ID equals ib.StructureID into tempPI
                                                     join ib in context.FO_StructureIrrigationBoundaries on new { x1 = pi.ID, x2 = pi.InfrastructureTypeID } equals new { x1 = ib.StructureID, x2 = ib.StructureTypeID } into tempPI
                                                     from tempInfrastructure in tempPI.DefaultIfEmpty()
                                                     join dv in context.CO_Division on tempInfrastructure.DivisionID equals dv.ID into tempPI_DV
                                                     from tempInfrastructureDivision in tempPI_DV.DefaultIfEmpty()
                                                     join cr in context.CO_Circle on tempInfrastructureDivision.CircleID equals cr.ID into tempPI_CR
                                                     from tempInfrastructureCircle in tempPI_CR.DefaultIfEmpty()
                                                     join zn in context.CO_Zone on tempInfrastructureCircle.ZoneID equals zn.ID into tempPI_ZN
                                                     from tempInfrastructureZone in tempPI_ZN.DefaultIfEmpty()
                                                     where
                                                     (
                                                     (pi.ID == _InfrastructureID || _InfrastructureID == 0)
                                                     && (string.IsNullOrEmpty(_InfrastructureName.Trim()) || pi.InfrastructureName.Contains(_InfrastructureName))
                                                     && (pi.InfrastructureTypeID == ((_InfrastructureTypeID == -1) ? pi.InfrastructureTypeID : _InfrastructureTypeID))
                                                     && (pi.IsActive == ((_InfrastructureStatus == -1) ? pi.IsActive : (_InfrastructureStatus == 1 ? true : false)))
                                                     && (tempInfrastructureDivision.ID == _DivisionID || _DivisionID == -1)
                                                     && (tempInfrastructureCircle.ID == _CircleID || _CircleID == -1)
                                                     && (tempInfrastructureZone.ID == _ZoneID || _ZoneID == -1)
                                                     )
                                                     select new
                                                     {
                                                         InfrastructureID = pi.ID
                                                         ,
                                                         InfrastructureName = pi.InfrastructureName
                                                         ,
                                                         TotalLength = pi.TotalLength
                                                         ,
                                                         DesignedTopWidth = pi.DesignedTopWidth
                                                         ,
                                                         DesignedFreeBoard = pi.DesignedFreeBoard
                                                         ,
                                                         InfrastructureStatus = ((pi.IsActive == true) ? "Active" : "In Active")
                                                         ,
                                                         InfrastructureTypeName = it.Name
                                                         ,
                                                         InfrastructureTypeID = it.ID
                                                     })
                                                   .ToList<object>().Distinct().ToList<object>();

            return lstInfrastructureSearch;

        }

        #endregion

        #region "Infrastructure Physical Location"
        /// <summary>
        /// This function return Infrastructure Irrigation Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIrrigationBoundariesByInfrastructureID(long _InfrastructureID, long _InfrastructureTypeID)
        {

            List<object> lstIrrigationBoundaries = (from IB in context.FO_StructureIrrigationBoundaries
                                                    join division in context.CO_Division on IB.DivisionID equals division.ID
                                                    join circle in context.CO_Circle on division.CircleID equals circle.ID
                                                    join zone in context.CO_Zone on circle.ZoneID equals zone.ID
                                                    where IB.StructureID == _InfrastructureID
                                                    && IB.StructureTypeID == _InfrastructureTypeID
                                                    select new
                                                    {
                                                        ID = IB.ID,
                                                        ZoneName = zone.Name,
                                                        ZoneID = zone.ID,
                                                        CircleName = circle.Name,
                                                        CircleID = circle.ID,
                                                        DivisionName = division.Name,
                                                        DivisionID = division.ID,
                                                        SectionRD = IB.SectionRD,
                                                        SectionToRD = IB.SectionToRD,
                                                        CreatedBy = IB.CreatedBy,
                                                        CreatedDate = IB.CreatedDate
                                                    }).ToList().Select(c => new
                                                    {
                                                        ID = c.ID,
                                                        ZoneName = c.ZoneName,
                                                        ZoneID = c.ZoneID,
                                                        CircleName = c.CircleName,
                                                        CircleID = c.CircleID,
                                                        DivisionName = c.DivisionName,
                                                        DivisionID = c.DivisionID,
                                                        SectionRD = c.SectionRD,
                                                        FromRDTotal = c.SectionRD,
                                                        ToRDTotal = c.SectionToRD,
                                                        FromRD = Calculations.GetRDText(Convert.ToInt64(c.SectionRD)),
                                                        ToRD = Calculations.GetRDText(Convert.ToInt64(c.SectionToRD)),
                                                        CreatedBy = c.CreatedBy,
                                                        CreatedDate = c.CreatedDate
                                                        //TotalRDs = Calculations.GetRDText(Convert.ToDouble(c.SectionRD))
                                                    }).OrderBy(c => c.SectionRD).ToList<object>();
            return lstIrrigationBoundaries;

        }
        /// <summary>
        /// This function return Infrastructure Administrative Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByInfrastructureID(long _InfrastructureID, long _InfrastructureTypeID)
        {
            List<object> lstAdminBoundaries = (from admin in context.FO_StructureAdminBoundaries
                                               join village in context.CO_Village on admin.VillageID equals village.ID
                                               join police in context.CO_PoliceStation on admin.PoliceStationID equals police.ID
                                               join tehsil in context.CO_Tehsil on police.TehsilID equals tehsil.ID
                                               join district in context.CO_District on tehsil.DistrictID equals district.ID
                                               where admin.StructureID == _InfrastructureID
                                               && admin.StructureTypeID == _InfrastructureTypeID
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
                                                   CreatedBy = admin.CreatedBy,
                                                   CreatedDate = admin.CreatedDate
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
                                          CreatedBy = admin.CreatedBy,
                                          CreatedDate = admin.CreatedDate
                                      }).ToList<object>();

            return lstAdminBoundaries;
        }
        /// <summary>
        /// This function return Infrastructure Divisions
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByInfrastructureID(long _InfrastructureID, long _InfrastructureTypeID)
        {
            List<CO_Division> lstDivisions = (from i in context.FO_StructureIrrigationBoundaries
                                              join division in context.CO_Division on i.DivisionID equals division.ID
                                              where i.StructureID == _InfrastructureID
                                               && i.StructureTypeID == _InfrastructureTypeID
                                              select division).Distinct().ToList();
            return lstDivisions;

        }
        /// <summary>
        /// This function return Infrastructure Districts
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_InfrastructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByInfrastructureID(long _InfrastructureID, long _InfrastructureTypeID)
        {
            List<object> lstDistricts = null;
            List<long> lstDivisionsIDs = GetDivisionsByInfrastructureID(_InfrastructureID, _InfrastructureTypeID).Select(d => d.ID).ToList<long>();

            if (lstDivisionsIDs != null && lstDivisionsIDs.Count > 0)
            {
                lstDistricts = (from dd in context.CO_DistrictDivision
                                join d in context.CO_District on dd.DistrictID equals d.ID
                                where lstDivisionsIDs.Contains(dd.DivisionID.Value)
                                select new
                                {
                                    ID = d.ID,
                                    Name = d.Name
                                }).Distinct().ToList<object>();
            }
            return lstDistricts;

        }
        #endregion

        #region Infrastructure Breaching Section
        public List<object> GetInfrastructureBreachingSection(long _InfrastructureID)
        {
            List<object> lstBreachingSection = (from r in context.FO_InfrastructureBreachingSection
                                                where r.ProtectionInfrastructureID == _InfrastructureID
                                                orderby r.ToRD ascending
                                                select r).ToList()
                                                .Select(r => new
                                                {
                                                    ID = r.ID,
                                                    StartingRDTotal = r.FromRD,
                                                    EndingRDTotal = r.ToRD,
                                                    Rows = r.Rows,
                                                    Liners = r.Liners,
                                                    StartingRD = Calculations.GetRDText(r.FromRD),
                                                    EndingRD = Calculations.GetRDText(r.ToRD),
                                                    CreatedBy = r.CreatedBy,
                                                    CreatedDate = r.CreatedDate

                                                }).ToList<object>();
            return lstBreachingSection;
        }
        public List<object> GetBreachingSectionExplosives(long _ExplosivesMatetialID)
        {
            List<object> lstExplosivesMatetial = (from r in context.FO_BreachingSectionExplosives
                                                  join c in context.FO_ExplosivesCustody on r.ExplosiveCustodyID equals c.ID
                                                  where r.BreachingSectionID == _ExplosivesMatetialID
                                                  orderby r.CreatedDate ascending
                                                  select new
                                                  {
                                                      ID = r.ID,
                                                      Custody = r.ExplosiveCustodyID,
                                                      CustodyName = c.Name,
                                                      Quantity = r.Quantity,
                                                      LocationDescription = r.LocationDescription,
                                                      CreatedBy = r.CreatedBy,
                                                      CreatedDate = r.CreatedDate
                                                  }).ToList<object>();

            return lstExplosivesMatetial;
        }
        #endregion Infrastructure Breaching Section
        public object GetInfrastructureParentNameByID(long _InfrastructureID)
        {

            object lstinfrastructureParent = (from ip in context.FO_InfrastructureParent
                                              join st in context.CO_StructureType on ip.StructureTypeID equals st.ID
                                              join s in context.CO_Station on ip.StructureID equals s.ID
                                              join d in context.CO_Division on ip.DivisionID equals d.ID into dip
                                              from dip1 in dip.DefaultIfEmpty()
                                              join c in context.CO_Circle on dip1.CircleID equals c.ID into cd
                                              from cd1 in cd.DefaultIfEmpty()
                                              join z in context.CO_Zone on cd1.ZoneID equals z.ID into zc
                                              from zc1 in zc.DefaultIfEmpty()
                                              where ip.ProtectionInfrastructureID == _InfrastructureID && st.ID == ip.StructureTypeID && st.DomainID == 1
                                              select new
                                              {
                                                  ID = ip.ID,
                                                  InfrastructureID = ip.ProtectionInfrastructureID,
                                                  ParentType = st.Name,
                                                  ParentID = st.ID,
                                                  ParentName = s.Name,
                                                  DivisionName = dip1.Name,
                                                  DivisionID = dip1.ID,
                                                  ZoneName = zc1.Name,
                                                  ZoneID = zc1.ID,
                                                  CircleName = cd1.Name,
                                                  CircleID = cd1.ID,
                                                  OffTakingRD = ip.OfftakeRD
                                              }).Distinct().FirstOrDefault();


            return lstinfrastructureParent;
        }


        public bool IsInfrastructureExists(FO_ProtectionInfrastructure _Infrastructure)
        {
            bool qIsInfrastructureExists = false;
            if (_Infrastructure.ID == 0)
            {
                qIsInfrastructureExists = context.FO_ProtectionInfrastructure.Any(c => c.InfrastructureName.ToLower() == _Infrastructure.InfrastructureName.ToLower()
                    && c.TotalLength == _Infrastructure.TotalLength);
            }
            else
            {

                qIsInfrastructureExists = context.FO_ProtectionInfrastructure.Any(c => c.InfrastructureName.ToLower() == _Infrastructure.InfrastructureName.ToLower()
                    && c.TotalLength == _Infrastructure.TotalLength && c.ID != _Infrastructure.ID);
            }
            return qIsInfrastructureExists;



        }

        #region "Control Infrastructure"

        #region Search
        public List<object> GetControlledInfrastructureBySearchCriteria(long _ControlInfrastructureID
          , long _ZoneID
          , long _CircleID
          , long _DivisionID
          , string _ControlInfrastructureName
          , Int64 _ControlInfrastructureStatus)
        {

            List<object> lstControlInfrastructureSearch =
                                                    (from s in context.CO_Station
                                                     join st in context.CO_StructureType on s.StructureTypeID equals st.ID
                                                     join p in context.CO_Province on s.ProvinceID equals p.ID into temp1_PS
                                                     from temp_province in temp1_PS.DefaultIfEmpty()
                                                     join r in context.CO_River on s.RiverID equals r.ID into temp_RS
                                                     from temp_River in temp_RS.DefaultIfEmpty()
                                                     join sib in context.FO_StructureIrrigationBoundaries on new { x1 = s.ID, x2 = s.StructureTypeID } equals new { x1 = sib.StructureID, x2 = sib.StructureTypeID } into tempPI
                                                     from tempInfrastructure in tempPI.DefaultIfEmpty()
                                                     join dv in context.CO_Division on tempInfrastructure.DivisionID equals dv.ID into tempPI_DV
                                                     from tempDivision in tempPI_DV.DefaultIfEmpty()
                                                     join cr in context.CO_Circle on tempDivision.CircleID equals cr.ID into tempPI_CR
                                                     from tempCircle in tempPI_CR.DefaultIfEmpty()
                                                     join zn in context.CO_Zone on tempCircle.ZoneID equals zn.ID into tempPI_ZN
                                                     from tempZone in tempPI_ZN.DefaultIfEmpty()
                                                     where
                                                     (
                                                     (s.ID == _ControlInfrastructureID || _ControlInfrastructureID == 0)
                                                     && (string.IsNullOrEmpty(_ControlInfrastructureName.Trim()) || s.Name.Contains(_ControlInfrastructureName))
                                                     && (s.IsActive == ((_ControlInfrastructureStatus == -1) ? s.IsActive : (_ControlInfrastructureStatus == 1 ? true : false)))
                                                     && (tempDivision.ID == _DivisionID || _DivisionID == -1)
                                                     && (tempCircle.ID == _CircleID || _CircleID == -1)
                                                     && (tempZone.ID == _ZoneID || _ZoneID == -1)
                                                     )
                                                     select new
                                                     {
                                                         ControlInfrastructureID = s.ID,
                                                         ControlInfrastructureName = s.Name,
                                                         InfrastructureTypeName = st.Name,
                                                         ProvinceName = temp_province.Name,
                                                         RiverName = temp_River.Name,
                                                         ControlInfrastructureStatus = ((s.IsActive == true) ? "Active" : "In Active")

                                                     })
                                                   .ToList<object>().Distinct().ToList<object>();

            return lstControlInfrastructureSearch;

        }
        #endregion Search

        #region Gauges
        public List<object> GetGaugesByControlInfrastructureID(long _ControlInfrastructureID)
        {
            List<object> lstGaugesByID = (from r in context.CO_StructureGauge
                                          join gt in context.CO_GaugeType on r.GaugeTypeID equals gt.ID into gtr
                                          from temp_gtr in gtr.DefaultIfEmpty()
                                          where r.StationID == _ControlInfrastructureID
                                          orderby r.ID ascending
                                          select new
                                                 {
                                                     ID = r.ID,
                                                     GaugesTypeID = r.GaugeTypeID,
                                                     GaugesTypeName = temp_gtr.Name,
                                                     NoOfGauges = r.NoOfGauges,
                                                     //UpstreamDownstream = r.UsDs == "1" ? "Yes" : "No",
                                                     UpstreamDownstream = r.UsDs == "1" ? "Upstream" : "Downstream",
                                                     Side = r.Side == "L" ? "Left" : "Right",
                                                     Remarks = r.Remarks,
                                                     CreatedBy = r.CreatedBy,
                                                     CreatedDate = r.CreatedDate

                                                 }).ToList<object>();
            return lstGaugesByID;
        }
        public List<object> GetGaugesByProtectionInfrastructureID(long ID)
        {
            List<object> lstGaugesByID = (from r in context.FO_FloodGauge
                                          join gt in context.FO_FloodGaugeType on r.GaugeTypeID equals gt.ID into gtr
                                          from temp_gtr in gtr.DefaultIfEmpty()
                                          where r.StructureID == ID
                                          orderby r.ID ascending
                                          select new
                                          {
                                              ID = r.ID,
                                              GaugesTypeID = r.GaugeTypeID,
                                              GaugesTypeName = temp_gtr.Name,
                                              GaugeRD = r.GaugeRD,
                                              CreatedBy = r.CreatedBy,
                                              CreatedDate = r.CreatedDate

                                          }).ToList().Select(Gauges => new
                                          {
                                              ID = Gauges.ID,
                                              GaugesTypeID = Gauges.GaugesTypeID,
                                              GaugesTypeName = Gauges.GaugesTypeName,
                                              GaugeRDTotal = Gauges.GaugeRD,
                                              GaugeRD = Calculations.GetRDText(Convert.ToInt64(Gauges.GaugeRD)),
                                              CreatedBy = Gauges.CreatedBy,
                                              CreatedDate = Gauges.CreatedDate


                                          }).ToList<object>();
            return lstGaugesByID;
        }

        #endregion Gauges


        /// <summary>
        /// This function return Structure Divisions
        /// Created on: 24-03-2017
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByStructureID(long _StructureID)
        {
            List<CO_Division> lstDivisions = (from f in context.FO_StructureIrrigationBoundaries
                                              //join section in context.CO_Section on f.SectionID equals section.ID
                                              //join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                              join division in context.CO_Division on f.DivisionID equals division.ID
                                              //join st in context.CO_StructureType on f.StructureTypeID equals st.ID
                                              where f.StructureID == _StructureID
                                              select division).Distinct().ToList();
            return lstDivisions;

        }
        /// <summary>
        /// This function return Structure Districts
        /// Created on: 24-03-2017
        /// </summary>
        /// <param name="_StructureID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByStructureID(long _StructureID, bool? _IsActive = null)
        {
            List<object> lstDistricts = null;
            List<long> lstDivisionsIDs = GetDivisionsByStructureID(_StructureID).Select(d => d.ID).ToList<long>();

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

        #endregion "Control Infrastructure"
    }
}
