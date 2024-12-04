using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.FloodOperations
{
    class FloodFightingPlanRepository : Repository<FO_FloodFightingPlan>
    {
        public FloodFightingPlanRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<FO_FloodFightingPlan>();
        }

        #region Flood Fighting Plan

        public List<object> GetInfrastructures_CampSiteBy_FFPID(long FFPID)
        {
            List<object> lstInfrastructure = (from FFPC in context.FO_FFPCampSites
                                              where FFPC.FFPID == FFPID
                                              select new
                                              {
                                                  FFPCampSitesID = FFPC.ID,
                                                  FFPID = FFPC.FFPID,
                                                  StructureID = FFPC.StructureID,
                                                  InfrastructureTypeID = FFPC.StructureTypeID,
                                                  InfrastructureType = "",
                                                  InfrastructureName = "",
                                                  RD = FFPC.RD,
                                                  Description = FFPC.Description,
                                                  CreatedBy = FFPC.CreatedBy,
                                                  CreatedDate = FFPC.CreatedDate
                                              }).ToList().Select(c => new
            {
                FFPCampSitesID = c.FFPCampSitesID,
                FFPID = c.FFPID,
                StructureID = c.StructureID,
                InfrastructureTypeID = c.InfrastructureTypeID,
                InfrastructureType = c.InfrastructureType,
                InfrastructureName = c.InfrastructureName,
                RDtotal = c.RD,
                RD = Calculations.GetRDText(Convert.ToInt64(c.RD)),
                Description = c.Description,
                CreatedDate = c.CreatedDate,
                CreatedBy = c.CreatedBy


            }).ToList<object>();

            return lstInfrastructure;

        }

        public object GetFFPDivisionID(long FFPID)
        {
            object lstDiv = (from FFP in context.FO_FloodFightingPlan
                             where FFP.ID == FFPID
                             select new
                             {
                                 DivisionID = FFP.DivisionID
                             }).ToList().Select(c => new
                                              {
                                                  DivisionID = c.DivisionID

                                              }).Distinct().FirstOrDefault();

            return lstDiv;

        }

        public string GetFFPStatus(long _FFPID)
        {
            string Status = (from s in context.FO_FloodFightingPlan
                             where (s.ID == _FFPID)
                             select s.Status).FirstOrDefault();

            return Status;

        }

        public bool IsFFPCampSits_Unique(FO_FFPCampSites _FFPCampSites)
        {

            bool chkInfrastructure = (from FFPC in context.FO_FFPCampSites
                                      where FFPC.FFPID == _FFPCampSites.FFPID
                                            && FFPC.StructureID == _FFPCampSites.StructureID
                                            && FFPC.StructureTypeID == _FFPCampSites.StructureTypeID
                                            && FFPC.RD == _FFPCampSites.RD
                                      select FFPC).Any();

            return chkInfrastructure;
        }
        public bool IsFFPCampSitsWithoutRD_Unique(FO_FFPCampSites _FFPCampSites)
        {

            bool chkInfrastructure = (from FFPC in context.FO_FFPCampSites
                                      where FFPC.FFPID == _FFPCampSites.FFPID
                                            && FFPC.StructureID == _FFPCampSites.StructureID
                                            && FFPC.StructureTypeID == _FFPCampSites.StructureTypeID
                                      select FFPC).Any();

            return chkInfrastructure;
        }
        public bool IsFFPCampSits_UniqueByID(FO_FFPCampSites _FFPCampSites)
        {

            bool chkFFPcamsite = (from FFPC in context.FO_FFPCampSites
                                  where FFPC.FFPID == _FFPCampSites.FFPID
                                        && FFPC.StructureID == _FFPCampSites.StructureID
                                        && FFPC.StructureTypeID == _FFPCampSites.StructureTypeID
                                        && FFPC.RD == _FFPCampSites.RD
                                        && FFPC.ID != _FFPCampSites.ID
                                  select FFPC).Any();

            return chkFFPcamsite;
        }
        public bool IsFFPCampSitsWithoutRD_UniqueByID(FO_FFPCampSites _FFPCampSites)
        {

            bool chkFFPcamsite = (from FFPC in context.FO_FFPCampSites
                                  where FFPC.FFPID == _FFPCampSites.FFPID
                                        && FFPC.StructureID == _FFPCampSites.StructureID
                                        && FFPC.StructureTypeID == _FFPCampSites.StructureTypeID
                                        && FFPC.ID != _FFPCampSites.ID
                                  select FFPC).Any();

            return chkFFPcamsite;
        }

        public dynamic GetFFPDetails(long? _FFPID, long? _ZoneID, long? _CircleID, long? _DivisionID, int? _FFPYear,
            string _Status)
        {
            dynamic qDivisionSummaryInfrastructure =
                context.Proc_FO_FFPSearch(_FFPID, _ZoneID, _CircleID, _DivisionID, _FFPYear, _Status).Select(o => new
                {
                    o.FFPID,
                    o.FFPYear,
                    o.FFPZone,
                    o.FFPCircle,
                    o.FFPDivision,
                    o.FFPDivisionID,
                    o.FFPStatus
                }).Distinct().SingleOrDefault();
            return qDivisionSummaryInfrastructure;

        }

        public bool IsFFPStonePosition_Unique(FO_FFPStonePosition _FFPStonePosition)
        {

            bool chkInfrastructure = (from FFPC in context.FO_FFPStonePosition
                                      where FFPC.FFPID == _FFPStonePosition.FFPID
                                            && FFPC.StructureID == _FFPStonePosition.StructureID
                                            && FFPC.StructureTypeID == _FFPStonePosition.StructureTypeID
                                            && FFPC.RD == _FFPStonePosition.RD
                                      select FFPC).Any();

            return chkInfrastructure;
        }

        public List<object> GetFO_SD_Attachment_ID(long SDID)
        {
            List<object> F_SD_Attachment = (from SDA in context.FO_SDImages
                                            where SDA.SDID == SDID
                                            select new
                                            {
                                                ID = SDA.ID,
                                                SDID = SDA.SDID,
                                                ImageURL = SDA.ImageURL,
                                                CreatedDate = SDA.CreatedDate,
                                                CreatedBy = SDA.CreatedBy


                                            }).ToList<object>();
            return F_SD_Attachment;

        }



        public List<object> GetStoneDeploymentByStonePositionID(long _StonePositionID)
        {
            List<object> StoneDeplotmentList = (from MDA in context.FO_StoneDeployment
                                                where MDA.FFPStonePositionID == _StonePositionID
                                                select new
                                                {
                                                    ID = MDA.ID,
                                                    FFPStonePositionID = MDA.FFPStonePositionID,
                                                    DisposedDate = MDA.DisposedDate,
                                                    VehicleNumber = MDA.VehicleNumber,
                                                    BuiltyNo = MDA.BuiltyNo,
                                                    QtyOfStoneDisposed = MDA.QtyOfStoneDisposed,
                                                    Cost = MDA.Cost,
                                                    CreatedDate = MDA.CreatedDate,
                                                    CreatedBy = MDA.CreatedBy


                                                }).ToList<object>();
            return StoneDeplotmentList;

        }



        public List<object> GetAttachmentStoneDeploymentByID(long SDID)
        {

            List<object> F_StoneDeployment_Attachment = (from MDA in context.FO_SDImages
                                                         where MDA.SDID == SDID
                                                         select new
                                                         {
                                                             ID = MDA.ID,
                                                             SDID = MDA.SDID,
                                                             ImageName = MDA.ImageName,
                                                             ImageURL = MDA.ImageURL,
                                                             CreatedDate = MDA.CreatedDate,
                                                             CreatedBy = MDA.CreatedBy


                                                         }).ToList<object>();
            return F_StoneDeployment_Attachment;

        }
        public bool DeleteStoneDeploymentAttachmentBySDID(long _SDID)
        {
            context.FO_SDImages.Where(p => p.SDID == _SDID)
               .ToList().ForEach(p => context.FO_SDImages.Remove(p));
            context.SaveChanges();
            return true;
        }


        public object GetStoneDeploymentByID(long _ID)
        {
            object StoneDeplotment = (from MDA in context.FO_StoneDeployment
                                      where MDA.ID == _ID
                                      select new
                                      {
                                          ID = MDA.ID,
                                          FFPStonePositionID = MDA.FFPStonePositionID,
                                          DisposedDate = MDA.DisposedDate,
                                          VehicleNumber = MDA.VehicleNumber,
                                          BuiltyNo = MDA.BuiltyNo,
                                          QtyOfStoneDisposed = MDA.QtyOfStoneDisposed,
                                          Cost = MDA.Cost,
                                          CreatedDate = MDA.CreatedDate,
                                          CreatedBy = MDA.CreatedBy


                                      }).FirstOrDefault();
            return StoneDeplotment;

        }
  


        #endregion

        #region Notification

        public FO_GetFloodFightingPlanNotifyData_Result GetFloodFightingPlanNotifyData(long _FloodFightingPlanID)
        {
            FO_GetFloodFightingPlanNotifyData_Result lstFloodFightingPlanNotifyData = context.FO_GetFloodFightingPlanNotifyData(_FloodFightingPlanID).FirstOrDefault<FO_GetFloodFightingPlanNotifyData_Result>();

            return lstFloodFightingPlanNotifyData;
        }
        
        #endregion  Notification
    }
}
