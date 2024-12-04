using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.FloodOperations
{
    public class FloodOperationsRepository : Repository<FO_DivisionSummary>
    {
        public FloodOperationsRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<FO_DivisionSummary>();
        }

        #region "Division Summary Search"
        public List<object> GetDivisionSummaryBySearchCriteria(long _DivisionSummaryID
          , long _ZoneID
          , long _CircleID
          , long _DivisionID
          , long _Year
          , string _DivisionSummaryStatus)
        {

            List<object> lstDivisionSummarySearch = (from ds in context.FO_DivisionSummary
                                                     join dv in context.CO_Division on ds.DivisionID equals dv.ID into tempPI_DV
                                                     from tempDivision in tempPI_DV.DefaultIfEmpty()
                                                     join cr in context.CO_Circle on tempDivision.CircleID equals cr.ID into tempPI_CR
                                                     from tempCircle in tempPI_CR.DefaultIfEmpty()
                                                     join zn in context.CO_Zone on tempCircle.ZoneID equals zn.ID into tempPI_ZN
                                                     from tempZone in tempPI_ZN.DefaultIfEmpty()
                                                     where
                                                     (
                                                     (ds.ID == (_DivisionSummaryID == 0 ? ds.ID : _DivisionSummaryID))
                                                     && (ds.Year == _Year || _Year == -1)
                                                     && (string.IsNullOrEmpty(_DivisionSummaryStatus.Trim()) || ds.Status.Contains(_DivisionSummaryStatus))
                                                     && (tempDivision.ID == _DivisionID || _DivisionID == -1)
                                                     && (tempCircle.ID == _CircleID || _CircleID == -1)
                                                     && (tempZone.ID == _ZoneID || _ZoneID == -1)
                                                     )
                                                     select new
                                                     {
                                                         DivisionSummaryID = ds.ID
                                                         ,
                                                         Zone = tempZone.Name
                                                         ,
                                                         Circle = tempCircle.Name
                                                         ,
                                                         Division = tempDivision.Name
                                                         ,
                                                         Status = ds.Status
                                                       ,
                                                         Year = ds.Year
                                                         ,
                                                         DivisionID = tempDivision.ID

                                                     }).OrderByDescending(d => d.Year).ToList<object>();

            return lstDivisionSummarySearch;

        }
        public object GetDivisionSummaryDetailByID(long _DivisionSummaryID)
        {

            object lstDivisionSummarySearch = (from ds in context.FO_DivisionSummary
                                               join dv in context.CO_Division on ds.DivisionID equals dv.ID into tempPI_DV
                                               from tempDivision in tempPI_DV.DefaultIfEmpty()
                                               join cr in context.CO_Circle on tempDivision.CircleID equals cr.ID into tempPI_CR
                                               from tempCircle in tempPI_CR.DefaultIfEmpty()
                                               join zn in context.CO_Zone on tempCircle.ZoneID equals zn.ID into tempPI_ZN
                                               from tempZone in tempPI_ZN.DefaultIfEmpty()
                                               where ds.ID == _DivisionSummaryID
                                               select new
                                               {
                                                   DivisionSummaryID = ds.ID
                                                   ,
                                                   Zone = tempZone.Name
                                                   ,
                                                   Circle = tempCircle.Name
                                                   ,
                                                   Division = tempDivision.Name
                                                   ,
                                                   DivisionID = tempDivision.ID
                                                   ,
                                                   Status = ds.Status
                                                 ,
                                                   Year = ds.Year
                                               }).Distinct().SingleOrDefault();

            return lstDivisionSummarySearch;

        }
        public dynamic GetDivisionSummaryInfrastructure(long _DivisionID)
        {
            dynamic qDivisionSummaryInfrastructure = context.FO_InfrastructureForDivisionSummary(_DivisionID).Select(o => new
                {
                    o.StructureName,
                    o.StructureType,
                    o.StructureTypeID,
                    o.StructureID
                }).SingleOrDefault();
            return qDivisionSummaryInfrastructure;
        }



        #endregion

        #region "Flood Inspection"

        /// This function fetches Division based on the UserID and User Irrigation Level and Circle ID.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_CircleID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByUserIDAndCircleID(long _UserID, long _IrrigationLevelID, long? _CircleID)
        {
            List<CO_Division> lstDivision = null;

            List<long?> lstLocationIDs = (from assloc in context.UA_AssociatedLocation
                                          where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
                                          select assloc.IrrigationBoundryID).ToList();

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
            {
                lstDivision = (from div in context.CO_Division
                               join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                               join sec in context.CO_Section on subdiv.ID equals sec.SubDivID
                               where lstLocationIDs.Contains(sec.ID)
                               select div).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                lstDivision = (from div in context.CO_Division
                               join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                               where lstLocationIDs.Contains(subdiv.ID)
                               select div).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                lstDivision = (from div in context.CO_Division
                               where lstLocationIDs.Contains(div.ID)
                                   //&& (div.CircleID == ((_CircleID == -1) ? div.CircleID : (_CircleID == 0 ? div.CircleID : _CircleID)))
                               && (div.CircleID == ((_CircleID == -1 || _CircleID == 0) ? div.CircleID : _CircleID))
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                lstDivision = (from div in context.CO_Division
                               join cir in context.CO_Circle on div.CircleID equals cir.ID
                               where lstLocationIDs.Contains(cir.ID)
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
            {
                lstDivision = (from div in context.CO_Division
                               join cir in context.CO_Circle on div.CircleID equals cir.ID
                               join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                               where lstLocationIDs.Contains(zon.ID)
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == 0)
            {
                lstDivision = (from div in context.CO_Division
                               select div).ToList();
            }
            return lstDivision;
        }

        /// This function fetches Circles based on the UserID and User Irrigation Level and Zone ID.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_ZoneID"></param>
        /// <returns>List<CO_Circle></returns>
        public List<CO_Circle> GetCircleByUserIDAndZoneID(long _UserID, long _IrrigationLevelID, long? _ZoneID)
        {
            List<CO_Circle> lstCircle = null;

            List<long?> lstLocationIDs = (from assloc in context.UA_AssociatedLocation
                                          where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
                                          select assloc.IrrigationBoundryID).ToList();

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
            {
                lstCircle = (from cir in context.CO_Circle
                             join div in context.CO_Division on cir.ID equals div.CircleID
                             join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                             join sec in context.CO_Section on subdiv.ID equals sec.SubDivID
                             where lstLocationIDs.Contains(sec.ID)
                             select cir).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                lstCircle = (from cir in context.CO_Circle
                             join div in context.CO_Division on cir.ID equals div.CircleID
                             join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                             where lstLocationIDs.Contains(subdiv.ID)
                             select cir).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                lstCircle = (from cir in context.CO_Circle
                             join div in context.CO_Division on cir.ID equals div.CircleID
                             where lstLocationIDs.Contains(div.ID)
                             && (cir.ZoneID == ((_ZoneID == -1 || _ZoneID == 0) ? cir.ZoneID : _ZoneID))  // due to multiple circle show in dropdown
                             orderby cir.Name
                             select cir).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                lstCircle = (from cir in context.CO_Circle
                             where lstLocationIDs.Contains(cir.ID)
                             && (cir.ZoneID == ((_ZoneID == -1 || _ZoneID == 0) ? cir.ZoneID : _ZoneID))
                             orderby cir.Name
                             select cir).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
            {
                lstCircle = (from cir in context.CO_Circle
                             join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                             where lstLocationIDs.Contains(zon.ID)
                             orderby cir.Name
                             select cir).ToList();
            }
            else if (_IrrigationLevelID == 0)
            {
                lstCircle = (from cir in context.CO_Circle
                             select cir).ToList();
            }
            return lstCircle;
        }

        /// This function fetches Zones based on the UserID and User Irrigation Level.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_Zone></returns>
        public List<CO_Zone> GetZoneByUserID(long _UserID, long _IrrigationLevelID)
        {
            List<CO_Zone> lstZone = null;

            List<long?> lstLocationIDs = (from assloc in context.UA_AssociatedLocation
                                          where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
                                          select assloc.IrrigationBoundryID).ToList();

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
            {
                lstZone = (from zon in context.CO_Zone
                           join cir in context.CO_Circle on zon.ID equals cir.ZoneID
                           join div in context.CO_Division on cir.ID equals div.CircleID
                           join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                           join sec in context.CO_Section on subdiv.ID equals sec.SubDivID
                           where lstLocationIDs.Contains(sec.ID)
                           select zon).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                lstZone = (from zon in context.CO_Zone
                           join cir in context.CO_Circle on zon.ID equals cir.ZoneID
                           join div in context.CO_Division on cir.ID equals div.CircleID
                           join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                           where lstLocationIDs.Contains(subdiv.ID)
                           select zon).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                lstZone = (from zon in context.CO_Zone
                           join cir in context.CO_Circle on zon.ID equals cir.ZoneID
                           join div in context.CO_Division on cir.ID equals div.CircleID
                           where lstLocationIDs.Contains(div.ID)
                           orderby zon.Name
                           select zon).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                lstZone = (from zon in context.CO_Zone
                           join cir in context.CO_Circle on zon.ID equals cir.ZoneID
                           where lstLocationIDs.Contains(cir.ID)
                           orderby zon.Name
                           select zon).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
            {
                lstZone = (from zon in context.CO_Zone
                           where lstLocationIDs.Contains(zon.ID)
                           //&& (zon.ZoneID == ((_ZoneID == -1 || _ZoneID == 0) ? cir.ZoneID : _ZoneID))
                           orderby zon.Name
                           select zon).ToList();
            }
            else if (_IrrigationLevelID == 0)
            {
                lstZone = (from zon in context.CO_Zone
                           select zon).ToList();
            }
            return lstZone;
        }

        public List<object> GetProtectionInfrastructureName(long _UserID, long _InfrastructureType)
        {
            List<object> lstProtectionInfrastructureName = null;
            switch (_InfrastructureType)
            {
                case 1:
                    lstProtectionInfrastructureName = (from pi in context.FO_ProtectionInfrastructure
                                                       join st in context.CO_StructureType on pi.InfrastructureTypeID equals st.ID
                                                       join sib in context.FO_StructureIrrigationBoundaries on new { x1 = pi.ID, x2 = pi.InfrastructureTypeID } equals new { x1 = sib.StructureID, x2 = sib.StructureTypeID }
                                                       join ac in context.UA_AssociatedLocation on sib.DivisionID equals ac.IrrigationBoundryID
                                                       where st.Source.Equals("Protection Infrastructure") && ac.IrrigationLevelID == 3 && ac.UserID == _UserID && pi.IsActive == true
                                                       select new
                                                       {
                                                           ID = pi.ID,
                                                           Name = pi.InfrastructureName,
                                                           InfrastructureTypeID = pi.InfrastructureTypeID
                                                       })
                                                       .ToList<object>();
                    break;
                case 2:
                    lstProtectionInfrastructureName = (from s in context.CO_Station
                                                       join st in context.CO_StructureType on s.StructureTypeID equals st.ID
                                                       join sib in context.FO_StructureIrrigationBoundaries on new { x1 = s.ID, x2 = s.StructureTypeID } equals new { x1 = sib.StructureID, x2 = sib.StructureTypeID }
                                                       join ac in context.UA_AssociatedLocation on sib.DivisionID equals ac.IrrigationBoundryID
                                                       where st.Source.Equals("Control Structure1") && ac.IrrigationLevelID == 3 && ac.UserID == _UserID && s.IsActive == true
                                                       select new
                                                       {
                                                           ID = s.ID,
                                                           Name = s.Name,
                                                           InfrastructureTypeID = s.StructureTypeID
                                                       })
                                                       .ToList<object>();
                    break;
                case 3:
                    lstProtectionInfrastructureName = (from d in context.FO_Drain
                                                       join st in context.CO_StructureType on d.DrainTypeID equals st.ID
                                                       join sib in context.FO_StructureIrrigationBoundaries on new { x1 = d.ID, x2 = d.DrainTypeID } equals new { x1 = sib.StructureID, x2 = sib.StructureTypeID }
                                                       join ac in context.UA_AssociatedLocation on sib.DivisionID equals ac.IrrigationBoundryID
                                                       where st.Source.Equals("Drain") && ac.IrrigationLevelID == 3 && ac.UserID == _UserID && d.IsActive == true
                                                       select new
                                                       {
                                                           ID = d.ID,
                                                           Name = d.Name,
                                                           InfrastructureTypeID = d.DrainTypeID
                                                       })
                                                       .ToList<object>();
                    break;
                default:
                    break;
            }
            return lstProtectionInfrastructureName;
        }


        public object GetInfrastructureTypeByID(long _FlodInspectinoID)
        {
            object InfrastructureType = null;

            InfrastructureType = (from fi in context.FO_FloodInspection
                                  join fid in context.FO_FloodInspectionDetail on fi.ID equals fid.FloodInspectionID into fid_temp
                                  from temp_InspectionDetail in fid_temp.DefaultIfEmpty()
                                  join st in context.CO_StructureType on temp_InspectionDetail.StructureTypeID equals st.ID into st_temp
                                  from temp_StructureType in st_temp.DefaultIfEmpty()
                                  where fi.ID.Equals(_FlodInspectinoID)
                                  select new
                                  {
                                      InfrastructureType = temp_StructureType.Source,
                                      CreatedDate = fi.CreatedDate,
                                      InspectionDate = fi.InspectionDate

                                  }).Distinct().FirstOrDefault();


            return InfrastructureType;
        }
        public FO_GetFloodInspectionsDetailByID_Result2 GetFloodInspectionsDetail(string _InfrastructureType, long _InspectionID)
        {

            var lstFloodInspectionsDetail = (from g in context.FO_GetFloodInspectionsDetailByID(_InfrastructureType, _InspectionID)
                                             select g).FirstOrDefault();

            return lstFloodInspectionsDetail;
        }
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

        #endregion

        #region Flood Departmental
        public object GetDepartmentalByID(long _FlodInspectinoID)
        {
            object InfrastructureType = null;

            InfrastructureType = (from fi in context.FO_FloodInspection
                                  where fi.ID.Equals(_FlodInspectinoID)
                                  select new
                                  {
                                      DivisionID = fi.DivisionID,
                                      CreatedDate = fi.CreatedDate,
                                      InspectionDate = fi.InspectionDate

                                  }).Distinct().FirstOrDefault();


            return InfrastructureType;
        }
        #endregion

        public CO_StructureType GetStructureInformationByEmergencyPurchaseID(long? _EmergencyPurchaseID)
        {
            CO_StructureType structureType = new CO_StructureType();

            structureType = (from ep in context.FO_EmergencyPurchase
                             join st in context.CO_StructureType on ep.StructureTypeID equals st.ID
                             where ep.ID == (_EmergencyPurchaseID == null ? 0 : _EmergencyPurchaseID)
                             select st).Distinct().OrderBy(d => d.Name).FirstOrDefault();


            return structureType;
        }
        public object GetInfrastructureTypeByEmergencyPurchaseID(long _EmergencyPurchaseID)
        {
            object InfrastructureType = null;

            InfrastructureType = (from fi in context.FO_EmergencyPurchase
                                  join st in context.CO_StructureType on fi.StructureTypeID equals st.ID into st_temp
                                  from temp_StructureType in st_temp.DefaultIfEmpty()
                                  where fi.ID.Equals(_EmergencyPurchaseID)
                                  select new
                                  {
                                      InfrastructureType = temp_StructureType.Source,
                                      CreatedDate = fi.CreatedDate
                                  }).Distinct().FirstOrDefault();


            return InfrastructureType;
        }

        #region Flood Emergency Purchasee
        public List<object> GetFloodFightingWorksByID(int EmergencyPurchaseID)
        {
            List<object> F_lstEP_works = (from Ep in context.FO_EPWork
                                          join NW in context.FO_NatureOfWork on Ep.NatureOfWorkID equals NW.ID
                                          where Ep.EmergencyPurchaseID == EmergencyPurchaseID
                                          select new
                                          {
                                              ID = Ep.ID,
                                              EmergencyPurchaseID = Ep.EmergencyPurchaseID,
                                              NatureOfWorkID = Ep.NatureOfWorkID,
                                              NatureOfWorkName = NW.Name,
                                              RD = Ep.RD,
                                              Description = Ep.Description,
                                              CreatedDate = Ep.CreatedDate,
                                              CreatedBy = Ep.CreatedBy

                                          }).ToList().Select(c => new
                                          {
                                              ID = c.ID,
                                              EmergencyPurchaseID = c.EmergencyPurchaseID,
                                              NatureOfWorkID = c.NatureOfWorkID,
                                              NatureOfWorkName = c.NatureOfWorkName,
                                              RDtotal = c.RD,
                                              RD = Calculations.GetRDText(Convert.ToInt64(c.RD)),
                                              Description = c.Description,
                                              CreatedDate = c.CreatedDate,
                                              CreatedBy = c.CreatedBy


                                          }).ToList<object>();
            return F_lstEP_works;

        }

        public object GetFo_EPWorkByObject_ID(long EmergencyPurchaseID)
        {
            object F_lstEP_works = (from Ep in context.FO_EPWork
                                    join NW in context.FO_NatureOfWork on Ep.NatureOfWorkID equals NW.ID
                                    where Ep.ID == EmergencyPurchaseID
                                    select new
                                    {
                                        ID = Ep.ID,
                                        EmergencyPurchaseID = Ep.EmergencyPurchaseID,
                                        NatureOfWorkID = Ep.NatureOfWorkID,
                                        NatureOfWorkName = NW.Name,
                                        RD = Ep.RD,
                                        Description = Ep.Description,
                                        CreatedDate = Ep.CreatedDate,
                                        CreatedBy = Ep.CreatedBy

                                    }).ToList().Select(c => new
                                          {
                                              ID = c.ID,
                                              EmergencyPurchaseID = c.EmergencyPurchaseID,
                                              NatureOfWorkID = c.NatureOfWorkID,
                                              NatureOfWorkName = c.NatureOfWorkName,
                                              RDtotal = c.RD,
                                              RD = Calculations.GetRDText(Convert.ToInt32(c.RD == null ? 0 : c.RD)),
                                              Description = c.Description,
                                              CreatedDate = c.CreatedDate,
                                              CreatedBy = c.CreatedBy


                                          }).FirstOrDefault();

            return F_lstEP_works;

        }


        public object GetFloodFightingDivision_CampSite_By_ID(long EmergencyPurchaseID)
        {
            object F_lstDivision_Campsite = (from EP in context.FO_EmergencyPurchase
                                             join CD in context.CO_Division on EP.DivisionID equals CD.ID
                                             join ST in context.CO_StructureType on EP.StructureTypeID equals ST.ID
                                             where EP.ID == EmergencyPurchaseID
                                             select new
                                             {
                                                 Name = CD.Name,
                                                 Campsite = EP.IsCampSite,
                                                 Struct_type_id = EP.StructureTypeID,
                                                 StructureID = EP.StructureID,
                                                 StructypeName = ST.Source

                                             }).Distinct().FirstOrDefault();
            return F_lstDivision_Campsite;

        }

        public List<object> GetFo_EP_NatureofWork()
        {
            List<object> F_lstFo_EP_natureofwork = (from EP in context.FO_NatureOfWork


                                                    select new
                                                    {
                                                        ID = EP.ID,
                                                        Name = EP.Name,
                                                        Description = EP.Description,
                                                        IsActive = EP.IsActive


                                                    }).ToList<object>();
            return F_lstFo_EP_natureofwork;

        }




        public object GetIsexistFo_EPItem_ID(long EmergencyPurchaseID, int ItemID)
        {
            object F_lstExistItemId = (from EPI in context.FO_EPItem
                                       where EPI.EmergencyPurchaseID == EmergencyPurchaseID && EPI.ItemID == ItemID
                                       select new
                                       {
                                           FO_EPItem_ID = EPI.ID


                                       }).ToList().Select(c => new
                                             {
                                                 FO_EPItem_ID = c.FO_EPItem_ID

                                             }).Distinct().FirstOrDefault();
            return F_lstExistItemId;

        }

        public object GetItemPurchasing_EmergcnyP_ID(long EmergencyPurchaseID)
        {
            object F_lstItemPurchasing_EmergcnyP = (from EP in context.FO_EmergencyPurchase
                                                    join ST in context.CO_StructureType on EP.StructureTypeID equals ST.ID
                                                    where EP.ID == EmergencyPurchaseID
                                                    select new
                                                    {
                                                        Struct_type_id = EP.StructureTypeID,
                                                        StructureID = EP.StructureID,
                                                        RD = EP.RD,
                                                        StructypeName = ST.Source


                                                    }).ToList().Select(c => new
                                          {
                                              Struct_type_id = c.Struct_type_id,
                                              StructureID = c.StructureID,
                                              RD = Calculations.GetRDText(Convert.ToInt64(c.RD)),
                                              StructypeName = c.StructypeName

                                          }).Distinct().FirstOrDefault(); ;
            return F_lstItemPurchasing_EmergcnyP;

        }

        public object GetEmergncyPurches_ItemQty_ID(long EmergencyPurchaseID, long itemid)
        {
            object F_lstEmergncy_itemQty = (from EPI in context.FO_EPItem
                                            where EPI.EmergencyPurchaseID == EmergencyPurchaseID && EPI.ItemID == itemid
                                            select new
                                            {
                                                PurchasedQty = EPI.PurchasedQty

                                            }).Distinct().FirstOrDefault();
            return F_lstEmergncy_itemQty;

        }
        public object GetFloodFightingInsfrastructureName(int StructureTypeID, int structid)
        {
            object F_lstInsfrastructureName = null;

            switch (StructureTypeID)
            {
                case 1:
                    F_lstInsfrastructureName = (from PI in context.FO_ProtectionInfrastructure
                                                where PI.ID == structid
                                                select new
                                                {
                                                    infraName = PI.InfrastructureName,
                                                    TotalLength = PI.TotalLength

                                                }).Distinct().FirstOrDefault();
                    break;
                case 2:

                    F_lstInsfrastructureName = (from COS in context.CO_Station
                                                where COS.ID == structid
                                                select new
                                                {
                                                    infraName = COS.Name

                                                }).Distinct().FirstOrDefault();
                    break;
                case 3:
                    F_lstInsfrastructureName = (from D in context.FO_Drain
                                                where D.ID == structid
                                                select new
                                                {
                                                    infraName = D.Name,
                                                    TotalLength = D.TotalLength

                                                }).Distinct().FirstOrDefault();
                    break;
                default:
                    break;


            }
            return F_lstInsfrastructureName;


        }


        public object GetItemPurchas_Type_InsfrastructureName(int StructureTypeID, int structid)
        {
            object F_lstInsfrastructureName = null;

            switch (StructureTypeID)
            {
                case 1:
                    F_lstInsfrastructureName = (from PI in context.FO_ProtectionInfrastructure
                                                join ST in context.CO_StructureType on PI.InfrastructureTypeID equals ST.ID
                                                where PI.ID == structid
                                                select new
                                                {
                                                    infraName = PI.InfrastructureName,
                                                    structypeName = ST.Name

                                                }).Distinct().FirstOrDefault();
                    break;
                case 2:

                    F_lstInsfrastructureName = (from COS in context.CO_Station
                                                join ST in context.CO_StructureType on COS.StructureTypeID equals ST.ID
                                                where COS.ID == structid
                                                select new
                                                {
                                                    infraName = COS.Name,
                                                    structypeName = ST.Name

                                                }).Distinct().FirstOrDefault();
                    break;
                case 3:
                    F_lstInsfrastructureName = (from D in context.FO_Drain
                                                join ST in context.CO_StructureType on D.DrainTypeID equals ST.ID
                                                where D.ID == structid
                                                select new
                                                {
                                                    infraName = D.Name,
                                                    structypeName = ST.Name

                                                }).Distinct().FirstOrDefault();
                    break;
                default:
                    break;


            }
            return F_lstInsfrastructureName;


        }

        public object GetMaterialDisposal_Header_By_ID(long EPworkID)
        {
            object F_lstMDisposal = (from EP in context.FO_EPWork
                                     join NW in context.FO_NatureOfWork on EP.NatureOfWorkID equals NW.ID
                                     join FEP in context.FO_EmergencyPurchase on EP.EmergencyPurchaseID equals FEP.ID
                                     join COD in context.CO_Division on FEP.DivisionID equals COD.ID
                                     join CC in context.CO_Circle on COD.CircleID equals CC.ID
                                     join Zone in context.CO_Zone on CC.ZoneID equals Zone.ID
                                     where EP.ID == EPworkID
                                     select new
                                     {
                                         Year = FEP.Year,
                                         ZoneName = Zone.Name,
                                         CircleName = CC.Name,
                                         NatureWorkName = NW.Name,
                                         RD = EP.RD,

                                     }).ToList().Select(c => new
                                          {
                                              Year = c.Year,
                                              ZoneName = c.ZoneName,
                                              CircleName = c.CircleName,
                                              NatureWorkName = c.NatureWorkName,
                                              RD = Calculations.GetRDText(Convert.ToInt64(c.RD))

                                          }).Distinct().FirstOrDefault();
            return F_lstMDisposal;

        }

        public object GetF_MaterialDisposal_Attachement_Header_By_ID(long MaterialDisposalID)
        {
            object F_lstMD_Attachment = (from MD in context.FO_MaterialDisposal
                                         where MD.ID == MaterialDisposalID
                                         select new
                                         {
                                             DisposalDate = MD.DisposalDate,
                                             VehicleNumber = MD.VehicleNumber,
                                             BuiltyNumber = MD.BuiltyNumber

                                         }).Distinct().FirstOrDefault();
            return F_lstMD_Attachment;

        }

        public List<object> GetF_EmergencyDisposalByID(long EPWorkID)
        {
            List<object> F_lstEP_works = (from MD in context.FO_MaterialDisposal
                                          where MD.EPWorkID == EPWorkID
                                          select new
                                          {
                                              ID = MD.ID,
                                              EPWorkID = MD.EPWorkID,
                                              DisposalDate = MD.DisposalDate,
                                              VehicleNumber = MD.VehicleNumber,
                                              QtyMaterial = MD.QtyMaterial,
                                              BuiltyNumber = MD.BuiltyNumber,
                                              Unit = MD.Unit,
                                              Cost = MD.Cost,
                                              CreatedDate = MD.CreatedDate,
                                              CreatedBy = MD.CreatedBy

                                          }).ToList<object>();
            return F_lstEP_works;

        }



        public object GetF_EmergencyDisposalObjectByID(long DisposalID)
        {
            object F_disposal_detail = (from MD in context.FO_MaterialDisposal
                                        where MD.ID == DisposalID
                                        select new
                                        {
                                            ID = MD.ID,
                                            EPWorkID = MD.EPWorkID,
                                            DisposalDate = MD.DisposalDate,
                                            VehicleNumber = MD.VehicleNumber,
                                            QtyMaterial = MD.QtyMaterial,
                                            BuiltyNumber = MD.BuiltyNumber,
                                            Unit = MD.Unit,
                                            Cost = MD.Cost,
                                            CreatedDate = MD.CreatedDate,
                                            CreatedBy = MD.CreatedBy

                                        }).FirstOrDefault();
            return F_disposal_detail;

        }

        public List<object> GetF_EmergencyDisposal_Attachment_ID(long MaterialDisposalID)
        {
            List<object> F_Disposal_Attachment = (from MDA in context.FO_MaterialDisposalAttachment
                                                  where MDA.MaterialDisposalID == MaterialDisposalID
                                                  select new
                                                  {
                                                      ID = MDA.ID,
                                                      MaterialDisposalID = MDA.MaterialDisposalID,
                                                      FileName = MDA.FileName,
                                                      FileURL = MDA.FileURL,
                                                      CreatedDate = MDA.CreatedDate,
                                                      CreatedBy = MDA.CreatedBy


                                                  }).ToList<object>();
            return F_Disposal_Attachment;

        }

        public object GetEmergency_DivisionStructtypeID(long EmergencyPurchaseID)
        {
            object lstDiv = (from EP in context.FO_EmergencyPurchase
                             where EP.ID == EmergencyPurchaseID
                             select new
                             {
                                 DivisionID = EP.DivisionID,
                                 StructureTypeID = EP.StructureTypeID,
                                 StructureID = EP.StructureID

                             }).Distinct().FirstOrDefault();

            return lstDiv;

        }
        public bool DeleteDisposalAttachmentByDisposalID(long _DisposalID)
        {
            context.FO_MaterialDisposalAttachment.Where(p => p.MaterialDisposalID == _DisposalID)
               .ToList().ForEach(p => context.FO_MaterialDisposalAttachment.Remove(p));
            context.SaveChanges();
            return true;
        }


        public object GetEmergncyPurches_ItemQty_ID(long EmergencyPurchaseID)
        {
            object F_lstEmergncy_itemQty = (from EPI in context.FO_EPItem
                                            where EPI.EmergencyPurchaseID == EmergencyPurchaseID
                                            select new
                                            {
                                                PurchasedQty = EPI.PurchasedQty

                                            }).Distinct().FirstOrDefault();
            return F_lstEmergncy_itemQty;

        }


        public bool DeleteDisposalDetailbyDisposalDetailID(long _DisposalDetailID)
        {
            context.FO_MaterialDisposalAttachment.Where(p => p.MaterialDisposalID == _DisposalDetailID)
               .ToList().ForEach(p => context.FO_MaterialDisposalAttachment.Remove(p));
            context.SaveChanges();
            return true;
        }



        public object GetFo_EmergencyPurchase_ID(long EmergencyPurchaseID)
        {
            object F_lstEP = (from Ep in context.FO_EmergencyPurchase

                              where Ep.ID == EmergencyPurchaseID
                              select new
                              {
                                  ID = Ep.ID,
                                  DivisionID = Ep.DivisionID,
                                  Year = Ep.Year,
                                  StructureID = Ep.StructureID,
                                  IsCampSite = Ep.IsCampSite,
                                  RD = Ep.RD,
                                  CreatedDate = Ep.CreatedDate,
                                  CreatedBy = Ep.CreatedBy

                              }).ToList().Select(c => new
                                    {
                                        ID = c.ID,
                                        DivisionID = c.DivisionID,
                                        Year = c.Year,
                                        StructureID = c.StructureID,
                                        IsCampSite = c.IsCampSite,
                                        RD = Calculations.GetRDText(Convert.ToInt32(c.RD == null ? 0 : c.RD)),
                                        CreatedDate = c.CreatedDate,
                                        CreatedBy = c.CreatedBy


                                    }).FirstOrDefault();

            return F_lstEP;

        }
        #endregion

        #region Flood Bund Gauges

        public List<object> GetFloodBundGaugesRD(long _StructureID)
        {
            List<object> lstRD = (from FG in context.FO_FloodGauge
                                  where FG.StructureID == _StructureID && (FG.StructureTypeID == 11 || FG.StructureTypeID == 12)
                                  select new
                                  {
                                      ID = FG.ID,
                                      Name = FG.GaugeRD
                                  }).ToList().Select(c => new
                             {
                                 ID = c.ID,
                                 Name = Calculations.GetRDText(Convert.ToInt32(c.Name == null ? 0 : c.Name)),
                             }).ToList<object>();
            return lstRD;

        #endregion Flood Bund Gauges
        }
    }
}
