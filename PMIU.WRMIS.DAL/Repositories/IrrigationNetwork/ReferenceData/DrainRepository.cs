using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;


namespace PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.ReferenceData
{
    class DrainRepository : Repository<FO_Drain>
    {
        public DrainRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<FO_Drain>();
        }

        #region "Drain Search"

        public List<object> GetDrainBySearchCriteria(long _DrainID
          , long _ZoneID
          , long _CircleID
          , long _DivisionID
          , long _SubDivisionID
          , string _DrainName
          )
        {

            List<object> lstDrainSearch =

                                                    (from pi in context.FO_Drain
                                                     join it in context.CO_StructureType on pi.DrainTypeID equals it.ID
                                                     join ib in context.FO_StructureIrrigationBoundaries on new { x1 = pi.ID, x2 = pi.DrainTypeID } equals new { x1 = ib.StructureID, x2 = ib.StructureTypeID } into tempPI
                                                     from tempDrain in tempPI.DefaultIfEmpty()
                                                     join dv in context.CO_Division on tempDrain.DivisionID equals dv.ID into tempPI_DV
                                                     from tempDrainDivision in tempPI_DV.DefaultIfEmpty()
                                                     join cr in context.CO_Circle on tempDrainDivision.CircleID equals cr.ID into tempPI_CR
                                                     from tempDrainCircle in tempPI_CR.DefaultIfEmpty()
                                                     join zn in context.CO_Zone on tempDrainCircle.ZoneID equals zn.ID into tempPI_CR_ZN
                                                     from tempDrainCircleZone in tempPI_CR_ZN.DefaultIfEmpty()
                                                     join sbdiv in context.CO_SubDivision on tempDrainDivision.ID equals sbdiv.DivisionID into tempPI_sd_div
                                                     from tempDrainSubDivision in tempPI_sd_div.DefaultIfEmpty()
                                                     where (
                                                     (pi.ID == _DrainID || _DrainID == 0)
                                                     && (string.IsNullOrEmpty(_DrainName.Trim()) || pi.Name.Contains(_DrainName))
                                                     && (tempDrain.DivisionID == _DivisionID || _DivisionID == -1)
                                                     && (tempDrainCircle.ID == _CircleID || _CircleID == -1)
                                                     && (tempDrainCircleZone.ID == _ZoneID || _ZoneID == -1)
                                                     && (tempDrainSubDivision.ID == _SubDivisionID || _SubDivisionID == -1)
                                                     )
                                                     select new
                                                     {
                                                         DrainID = pi.ID
                                                         ,
                                                         DrainName = pi.Name
                                                         ,
                                                         DrainLength = pi.TotalLength
                                                         ,
                                                         CatchmentArea = pi.CatchmentArea
                                                         ,
                                                         MajorBuildUpArea = pi.BuildupArea
                                                         ,
                                                         DrainStatus = ((pi.IsActive == true) ? "Active" : "Inactive")
                                                         ,
                                                         DrainTypeName = it.Name
                                                     }).Distinct().OrderByDescending(x => x.DrainID)
                                                   .ToList<object>().Distinct().ToList<object>();

            return lstDrainSearch; ;

        }


        #endregion

        #region "Drain Physical Location"

        /// <summary>
        /// This function return Drain Irrigation Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIrrigationBoundariesByDrainID(long _DrainID, long _DrainTypeID)
        {

            List<object> lstIrrigationBoundaries = (from IB in context.FO_StructureIrrigationBoundaries
                                                    join division in context.CO_Division on IB.DivisionID equals division.ID
                                                    join sec in context.CO_Section on IB.SectionID equals sec.ID into sect
                                                    from section in sect.DefaultIfEmpty()
                                                    join sd in context.CO_SubDivision on section.SubDivID equals sd.ID into subdiv
                                                    from subdivision in subdiv.DefaultIfEmpty()
                                                    join circle in context.CO_Circle on division.CircleID equals circle.ID
                                                    join zone in context.CO_Zone on circle.ZoneID equals zone.ID
                                                    where IB.StructureID == _DrainID
                                                    && IB.StructureTypeID == _DrainTypeID
                                                    select new
                                                    {
                                                        ID = IB.ID,
                                                        ZoneName = zone.Name,
                                                        ZoneID = zone.ID,
                                                        CircleName = circle.Name,
                                                        CircleID = circle.ID,
                                                        DivisionName = division.Name,
                                                        DivisionID = division.ID,
                                                        SubDivisionName = subdivision.Name,
                                                        SubDivisionID = (long?)subdivision.ID,
                                                        SectionName = section.Name,
                                                        SectionID = (long?)section.ID,
                                                        SectionRD = IB.SectionRD,
                                                        SectionToRD = IB.SectionToRD,
                                                        CreatedBy = IB.CreatedBy,
                                                        CreatedDate = IB.CreatedDate
                                                    }).Distinct().ToList().Select(c => new
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
                                                        SectionID = c.ID,
                                                        SectionRD = c.SectionRD,
                                                        FromRDTotal = c.SectionRD,
                                                        ToRDTotal = c.SectionToRD,
                                                        FromRD = Calculations.GetRDText(Convert.ToInt64(c.SectionRD)),
                                                        ToRD = Calculations.GetRDText(Convert.ToInt64(c.SectionToRD)),
                                                        CreatedBy = c.CreatedBy,
                                                        CreatedDate = c.CreatedDate
                                                        //TotalRDs = Calculations.GetRDText(Convert.ToDouble(c.SectionRD))
                                                    }).OrderBy(c => c.SectionRD).Distinct().ToList<object>();
            return lstIrrigationBoundaries;

        }


        /// <summary>
        /// This function return Drain Administrative Boundaries
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByDrainID(long _DrainID, long _DrainTypeID)
        {
            List<object> lstAdminBoundaries = (from admin in context.FO_StructureAdminBoundaries
                                               join village in context.CO_Village on admin.VillageID equals village.ID
                                               join police in context.CO_PoliceStation on admin.PoliceStationID equals police.ID
                                               join tehsil in context.CO_Tehsil on police.TehsilID equals tehsil.ID
                                               join district in context.CO_District on tehsil.DistrictID equals district.ID
                                               where admin.StructureID == _DrainID
                                               && admin.StructureTypeID == _DrainTypeID
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



        #endregion

        #region "Outfall Drain Details"

        /// <summary>
        /// This function return Circle List for Drain Domain
        /// Created on: 15-09-2016
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<object></returns>
        public List<dynamic> GetCirclesforDrainDomain(long _ZoneID)
        {
            List<dynamic> lstCircles = (from Cir in context.CO_Circle
                                        join Zone in context.CO_Zone on Cir.ZoneID equals Zone.ID
                                        join Div in
                                            (from x in context.CO_Division where x.DomainID == 4 select x) on Cir.ID equals Div.CircleID
                                        where Cir.ZoneID == _ZoneID
                                        select new { ID = Cir.ID, Name = Cir.Name }
                             ).Distinct().ToList<dynamic>();

            return lstCircles;
        }


        /// <summary>
        /// This function return Outfall Name in case of Rivers
        /// Created on: 29-09-2016
        /// </summary>
        /// <param name="_DrainID"></param>
        /// <returns>List<object></returns>
        public List<dynamic> GetRiverNamesByStructureType()
        {
            List<dynamic> lstRivers = (from Station in context.CO_Station
                                       where Station.StructureTypeID == 18
                                       select new { ID = Station.ID, Name = Station.Name }
                             ).Distinct().ToList<dynamic>();

            return lstRivers;
        }

        /// <summary>
        /// This function return outfall Name dropdown against zone ID
        /// Created on: 29-09-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>List<object></returns>
        public List<dynamic> GetDrainNamesByID(long _ID, int Type)
        {
            List<dynamic> lstRivers = (from Drain in context.FO_Drain
                                       join DrainIrrigationB in context.FO_StructureIrrigationBoundaries on Drain.ID equals DrainIrrigationB.StructureID
                                       join Section in context.CO_Section on DrainIrrigationB.SectionID equals Section.ID
                                       join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                                       join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                                       join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                       join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                                       where
                                            (Type == (int)Constants.IrrigationLevelID.Zone && Zone.ID == _ID)
                                            || (Type == (int)Constants.IrrigationLevelID.Circle && Circle.ID == _ID)
                                            || (Type == (int)Constants.IrrigationLevelID.Division && Division.ID == _ID)
                                            || (Type == (int)Constants.IrrigationLevelID.SubDivision && SubDivision.ID == _ID)
                                       select new { ID = Drain.ID, Name = Drain.Name }
                             ).Distinct().ToList<dynamic>();

            return lstRivers;
        }

        /// <summary>
        /// This function returnselected dropdowns IDs for Zone,Circle,Division.
        /// Created on: 29-09-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>object<object></returns>
        public object GetSelectedDropdownsHeirarchyIDs(long? _SubDivisionID)
        {
            object lstIDS = (from SubDivision in context.CO_SubDivision
                             join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                             join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                             join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                             where SubDivision.ID == _SubDivisionID
                             select new
                             {
                                 ZoneID = Zone.ID,
                                 CircleID = Circle.ID,
                                 DivisionID = Division.ID
                             }
                             ).Distinct().FirstOrDefault();

            return lstIDS;
        }


        /// <summary>
        /// This function return outfall Name dropdown against zone ID
        /// Created on: 24-10-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>List<object></returns>
        public string GetDrainOutFallNameByDainID(long _DrainID)
        {

            FO_DrainOutfall drainOutfall = GetDrainOutFallByDrainID(_DrainID);

            //object outFallInformation = new object();

            string outFallInformation = "";

            if (drainOutfall != null && drainOutfall.OutfallType != null && drainOutfall.OutfallType.ToLower() == "drain")
            {
                outFallInformation = (from dro in context.FO_DrainOutfall
                                      join dr in context.FO_Drain on dro.OutfallID equals dr.ID
                                      where dro.DrainID == _DrainID
                                      select new
                                         {
                                             DrainOutFallName = dr.Name
                                         }).Distinct().FirstOrDefault().DrainOutFallName;
            }

            if (drainOutfall != null && drainOutfall.OutfallType != null && drainOutfall.OutfallType.ToLower() == "river")
            {

                outFallInformation = (from dro in context.FO_DrainOutfall
                                      join rv in context.CO_Station on dro.OutfallID equals rv.ID
                                      where dro.DrainID == _DrainID
                                      && rv.StructureTypeID == 18
                                      select new
                                      {
                                          DrainOutFallName = rv.Name
                                      }).Distinct().FirstOrDefault().DrainOutFallName;
            }

            return outFallInformation;
        }
        public FO_DrainOutfall GetDrainOutFallByDrainID(long _DrainID)
        {
            FO_DrainOutfall drainOutfall = new FO_DrainOutfall();

            drainOutfall = (from dot in context.FO_DrainOutfall
                            where dot.DrainID == _DrainID
                            select dot).Distinct().SingleOrDefault();

            return drainOutfall;
        }

        #endregion


    }
}
