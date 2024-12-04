using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System.Collections.Generic;
using System.Linq;

namespace PMIU.WRMIS.DAL.Repositories.FloodOperations
{
    internal class OnsiteMonitoringRepository : Repository<FO_FloodFightingPlan>
    {
        public OnsiteMonitoringRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<FO_FloodFightingPlan>();
        }

        public object GETFO_OMStonePositionID(long SDID)
        {
            object F_lstOMStonePositione = (from SP in context.FO_OMStonePosition
                                            where SP.StoneDeploymentID == SDID
                                            select new
                                            {
                                                ID = SP.ID,
                                                CreatedBy = SP.CreatedBy,
                                                CreatedDate = SP.CreatedDate,
                                                MonitoringDate = SP.MonitoringDate
                                            }).Distinct().FirstOrDefault();
            return F_lstOMStonePositione;
        }

        //public List<object> GETFO_OMCampsites(long _FFPCampSite)
        //{
        //    List<object> OM_Campsite_List = (from MDA in context.FO_FFPCampSites
        //                                     join OM in context.FO_OMCampSites on 
        //                                     MDA.ID equals OM.CampSiteID
        //                                     where MDA.FFPID == _FFPCampSite

        //                                        select new
        //                                        {
        //                                            ID = MDA.ID,
        //                                            FFPID = MDA.FFPID,
        //                                            StructureTypeID = MDA.StructureTypeID,
        //                                            StructureID = MDA.StructureID,
        //                                            RD = MDA.RD,
        //                                            isAvailable = OM.IsAvailable,
        //                                            Description = MDA.Description,                                                    
        //                                            CreatedDate = MDA.CreatedDate,
        //                                            CreatedBy = MDA.CreatedBy


        //                                        }).ToList<object>();



       

        //    return OM_Campsite_List;

        //}


        public List<object> GETFO_OMCampsites(long _FFPCampSite)
        {
            List<object> OM_Campsite_List = (from MDA in context.FO_FFPCampSites
                                             
                                             where MDA.ID == _FFPCampSite

                                             select new
                                             {
                                                 ID = MDA.ID,
                                                 FFPID = MDA.FFPID,
                                                 StructureTypeID = MDA.StructureTypeID,
                                                 StructureID = MDA.StructureID,
                                                 RD = MDA.RD,
                                                
                                                 Description = MDA.Description,
                                                 CreatedDate = MDA.CreatedDate,
                                                 CreatedBy = MDA.CreatedBy


                                             }).ToList<object>();



           // List<FO_FFPCampSites> lstOMCampSite = db.Repository<FO_FFPCampSites>().GetAll().Where(x => x.ID == _FFPCampSite).ToList();


            return OM_Campsite_List;

        }


        public object GetOnSiteMonitoringCampSitesObjectByID(long ID)
        {
            object OMCampSiteObj = (from omc in context.FO_OMCampSites
                                    where omc.ID == ID
                                             select new
                                             {
                                                 ID = omc.ID,
                                                 MonitoringDate = omc.MonitoringDate,
                                                 CampSiteID = omc.CampSiteID,
                                                 isAvailable = omc.IsAvailable,
                                                 CreatedBy = omc.CreatedBy,
                                                 CreatedDate = omc.CreatedDate
                                             }).FirstOrDefault();

            return OMCampSiteObj;

        }
       

    }
}