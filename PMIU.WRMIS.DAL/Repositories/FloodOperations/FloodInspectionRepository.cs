using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;

namespace PMIU.WRMIS.DAL.Repositories.FloodOperations
{
    public class FloodInspectionRepository : Repository<FO_FloodInspection>
    {
        public FloodInspectionRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<FO_FloodInspection>();
        }

        #region "Flood Inspection"

        /// <summary>
        /// This function return Infrastructure Divisions
        /// Created on: 18-09-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>List<CO_Division></returns>
        public long? GetStructureTypeIDByFloodInspectionID(long _FloodInspectionID)
        {

            long? structureTypeID = (from fi in context.FO_FloodInspection
                                     join fid in context.FO_FloodInspectionDetail on fi.ID equals fid.FloodInspectionID
                                     where fi.ID == _FloodInspectionID
                                     select fid.StructureTypeID).Single();
            return structureTypeID;

        }

        public CO_StructureType GetStructureInformationByFloodInspectionID(long _FloodInspectionID)
        {
            CO_StructureType structureType = new CO_StructureType();

            structureType = (from fi in context.FO_FloodInspection
                             join fid in context.FO_FloodInspectionDetail on fi.ID equals fid.FloodInspectionID
                             join st in context.CO_StructureType on fid.StructureTypeID equals st.ID
                             where fi.ID == _FloodInspectionID
                             select st).Distinct().OrderBy(d => d.Name).Single();


            return structureType;
        }

        public object GetFloodInspectionByID(long _FloodInspectionID)
        {

            // First we will get StrucutreTypeID against _FloodInspectionID then we will make join with their respective tables
            // For StructureType = ProtectionInfrastructure
            //        We will join with FO_ProtectionInfrastructure
            // For StructureType = Barage and HeadWorkd
            //        We will join with CO_Station
            // For StructureType = Drain
            //        We will join with FO_Drain

            object floodInspectionDetail = new object();

            CO_StructureType structureType = GetStructureInformationByFloodInspectionID(_FloodInspectionID);

            if (structureType.Source == "Protection Infrastructure")
            {
                floodInspectionDetail = (from fi in context.FO_FloodInspection
                                         join fid in context.FO_FloodInspectionDetail on fi.ID equals fid.FloodInspectionID
                                         join fis in context.FO_InspectionStatus on fi.InspectionStatusID equals fis.ID
                                         join fit in context.FO_InspectionType on fid.InspectionTypeID equals fit.ID
                                         join st in context.CO_StructureType on fid.StructureTypeID equals st.ID
                                         join pi in context.FO_ProtectionInfrastructure on fid.StructureID equals pi.ID
                                         join d in context.CO_Division on fi.DivisionID equals d.ID
                                         where fi.ID == _FloodInspectionID
                                         select new
                                         {
                                             ID = fi.ID,
                                             InspectionDate = fi.InspectionDate,
                                             DivisionName = d.Name,
                                             InspectionTypeName = fit.Name,
                                             InfrastructureType = st.Name,
                                             InfrastructureName = pi.InfrastructureName,
                                             InspectionStatus = fis.Name,
                                             InspectionYear = fi.Year,
                                             StructureType = st.ID,
                                             DivisionID = d.ID,
                                             StructureID = pi.ID
                                         }).Distinct().FirstOrDefault();
            }
            else if (structureType.Source == "Control Structure1" && (structureType.ID == 1 || structureType.ID == 2))// For Barrage and headworks
            {
                floodInspectionDetail = (from fi in context.FO_FloodInspection
                                         join fid in context.FO_FloodInspectionDetail on fi.ID equals fid.FloodInspectionID
                                         join fis in context.FO_InspectionStatus on fi.InspectionStatusID equals fis.ID
                                         join fit in context.FO_InspectionType on fid.InspectionTypeID equals fit.ID
                                         join st in context.CO_StructureType on fid.StructureTypeID equals st.ID
                                         join stn in context.CO_Station on fid.StructureID equals stn.ID
                                         join d in context.CO_Division on fi.DivisionID equals d.ID
                                         where fi.ID == _FloodInspectionID
                                         select new
                                         {
                                             ID = fi.ID,
                                             InspectionDate = fi.InspectionDate,
                                             DivisionName = d.Name,
                                             InspectionTypeName = fit.Name,
                                             InfrastructureType = st.Name,
                                             InfrastructureName = stn.Name,
                                             InspectionStatus = fis.Name,
                                             InspectionYear = fi.Year,
                                             StructureType = st.ID,
                                             DivisionID = d.ID,
                                             StructureID = stn.ID
                                         }).Distinct().FirstOrDefault();
            }
            else if (structureType.Source == "Drain") // Drain
            {
                DrainRepository repDrain = new DrainRepository(this.context);// = this.context.DrainRepository();
                //string name = repDrain.GetDrainOutFallNameByDainID(1);

                floodInspectionDetail = (from fi in context.FO_FloodInspection
                                         join fid in context.FO_FloodInspectionDetail on fi.ID equals fid.FloodInspectionID
                                         join fis in context.FO_InspectionStatus on fi.InspectionStatusID equals fis.ID
                                         join fit in context.FO_InspectionType on fid.InspectionTypeID equals fit.ID
                                         join ic in context.FO_InspectionCategory on fi.InspectionCategoryID equals ic.ID
                                         join st in context.CO_StructureType on fid.StructureTypeID equals st.ID
                                         join drn in context.FO_Drain on fid.StructureID equals drn.ID
                                         join drnout in context.FO_DrainOutfall on drn.ID equals drnout.DrainID into tempDrainOutfall
                                         from tempDrainOutfallTable in tempDrainOutfall.DefaultIfEmpty()
                                         join d in context.CO_Division on fi.DivisionID equals d.ID
                                         where fi.ID == _FloodInspectionID
                                         select new
                                         {
                                             ID = fi.ID,
                                             InspectionDate = fi.InspectionDate,
                                             DivisionName = d.Name,
                                             InspectionTypeName = fit.Name,
                                             InfrastructureType = st.Name,
                                             InfrastructureName = drn.Name,
                                             InspectionStatus = fis.Name,
                                             InspectionCategoryName = ic.Name,
                                             OutFallType = tempDrainOutfallTable.OutfallType,
                                             DrainID = drn.ID,
                                             OutFallName = "",//repDrain.GetDrainOutFallNameByDainID(drn.ID)
                                             InspectionYear = fi.Year,
                                             StructureType = st.ID,
                                             DivisionID = d.ID,
                                             StructureID = drn.ID
                                         }).ToList().Select(x => new
                                         {
                                             ID = x.ID,
                                             InspectionDate = x.InspectionDate,
                                             DivisionName = x.DivisionName,
                                             InspectionTypeName = x.InspectionTypeName,
                                             InfrastructureType = x.InfrastructureType,
                                             InfrastructureName = x.InfrastructureName,
                                             InspectionStatus = x.InspectionStatus,
                                             InspectionCategoryName = x.InspectionCategoryName,
                                             OutFallType = x.OutFallType,
                                             OutFallName = repDrain.GetDrainOutFallNameByDainID(x.DrainID),
                                             InspectionYear = x.InspectionYear,
                                             StructureType = x.StructureType,
                                             DivisionID = x.DivisionID,
                                             StructureID = x.StructureID
                                         }).Distinct().FirstOrDefault();
            }
            else
            {
                floodInspectionDetail = (from fi in context.FO_FloodInspection
                                         join fis in context.FO_InspectionStatus on fi.InspectionStatusID equals fis.ID
                                         join d in context.CO_Division on fi.DivisionID equals d.ID
                                         where fi.ID == _FloodInspectionID
                                         select new
                                         {
                                             ID = fi.ID,
                                             InspectionDate = fi.InspectionDate,
                                             DivisionName = d.Name,
                                             InspectionTypeName = "",
                                             InfrastructureType = "",
                                             InfrastructureName = "",
                                             InspectionStatus = fis.Name
                                         }).Distinct().FirstOrDefault();
            }


            return floodInspectionDetail;
        }

        public List<FO_InspectionConditions> GetInspectionConditionsByGroup(string _CoditionGroup)
        {
            List<FO_InspectionConditions> lstInspectionConditions = null;

            lstInspectionConditions = (from cib in context.FO_InspectionConditions
                                       where cib.CoditionGroup == _CoditionGroup
                                       //select cib).Distinct().OrderBy(c => c.Name).ToList();
                                       select cib).Distinct().OrderBy(c => c.ID).ToList();

            return lstInspectionConditions;
        }

        public object GetIGCBarrageHWInformationByInspectionID(long _FloodInspectionID)
        {
            object oIGCBarrageHWInformation = new object();


            oIGCBarrageHWInformation = (from bhw in context.FO_IGCBarrageHW
                                        from ic1 in context.FO_InspectionConditions
                                                .Where(v => v.ID == bhw.LightConditionID)
                                                .DefaultIfEmpty()
                                        from ic2 in context.FO_InspectionConditions
                                                .Where(v => v.ID == bhw.DataBoardConditionID)
                                                .DefaultIfEmpty()
                                        from ic3 in context.FO_InspectionConditions
                                                .Where(v => v.ID == bhw.TollHutConditionID)
                                                .DefaultIfEmpty()
                                        from ic4 in context.FO_InspectionConditions
                                                .Where(v => v.ID == bhw.ArmyCPConditionID)
                                                .DefaultIfEmpty()
                                        where
                                                bhw.FloodInspectionID == _FloodInspectionID
                                        select new
                                        {
                                            IGCBarrageHWID = bhw.ID,
                                            TotalCameras = bhw.TotalCameras,
                                            OperationalCameras = bhw.OperationalCameras,
                                            NonOperationalCameras = bhw.TotalCameras - bhw.OperationalCameras,
                                            CCTVIncharge = bhw.CCTVIncharge,
                                            CCTVInchargePhone = bhw.CCTVInchargePhone,
                                            PoliceMonitory = bhw.PoliceMonitory,
                                            LightConditionID = bhw.LightConditionID,
                                            DataBoardConditionID = bhw.DataBoardConditionID,
                                            TollHutConditionID = bhw.TollHutConditionID,
                                            ArmyCPConditionID = bhw.ArmyCPConditionID,
                                            OperationalDeckElevated = bhw.OperationalDeckElevated,
                                            Remarks = bhw.Remarks,
                                            CreatedBy = bhw.CreatedBy,
                                            CreatedDate = bhw.CreatedDate
                                        }).Distinct().FirstOrDefault();

            return oIGCBarrageHWInformation;
        }
        public object GetIGCBarrageHWGInformationByInspectionID(long _FloodInspectionID)
        {
            object oIGCBarrageHWInformation = new object();
            List<object> oIGCBarrageHWGatesInformation = GetIGCBarrageHWGatesInformationByBarrageHWID(Convert.ToInt64(_FloodInspectionID));


            oIGCBarrageHWInformation = (from bhw in context.FO_IGCBarrageHW
                                        from ic1 in context.FO_InspectionConditions
                                                .Where(v => v.ID == bhw.LightConditionID)
                                                .DefaultIfEmpty()
                                        from ic2 in context.FO_InspectionConditions
                                                .Where(v => v.ID == bhw.DataBoardConditionID)
                                                .DefaultIfEmpty()
                                        from ic3 in context.FO_InspectionConditions
                                                .Where(v => v.ID == bhw.TollHutConditionID)
                                                .DefaultIfEmpty()
                                        from ic4 in context.FO_InspectionConditions
                                                .Where(v => v.ID == bhw.ArmyCPConditionID)
                                                .DefaultIfEmpty()
                                        where
                                                bhw.FloodInspectionID == _FloodInspectionID
                                        select new
                                        {
                                            IGCBarrageHWID = bhw.ID,
                                            TotalCameras = bhw.TotalCameras,
                                            OperationalCameras = bhw.OperationalCameras,
                                            NonOperationalCameras = bhw.TotalCameras - bhw.OperationalCameras,
                                            CCTVIncharge = bhw.CCTVIncharge,
                                            CCTVInchargePhone = bhw.CCTVInchargePhone,
                                            PoliceMonitory = bhw.PoliceMonitory,
                                            LightConditionID = bhw.LightConditionID,
                                            DataBoardConditionID = bhw.DataBoardConditionID,
                                            TollHutConditionID = bhw.TollHutConditionID,
                                            ArmyCPConditionID = bhw.ArmyCPConditionID,
                                            OperationalDeckElevated = bhw.OperationalDeckElevated,
                                            Remarks = bhw.Remarks,
                                            CreatedBy = bhw.CreatedBy,
                                            CreatedDate = bhw.CreatedDate

                                        }).Distinct().AsEnumerable().Select(c => new
                                        {
                                            IGCBarrageHWID = c.IGCBarrageHWID,
                                            TotalCameras = c.TotalCameras,
                                            OperationalCameras = c.OperationalCameras,
                                            NonOperationalCameras = c.NonOperationalCameras,
                                            CCTVIncharge = c.CCTVIncharge,
                                            CCTVInchargePhone = c.CCTVInchargePhone,
                                            PoliceMonitory = c.PoliceMonitory,
                                            LightConditionID = c.LightConditionID,
                                            DataBoardConditionID = c.DataBoardConditionID,
                                            TollHutConditionID = c.TollHutConditionID,
                                            ArmyCPConditionID = c.ArmyCPConditionID,
                                            OperationalDeckElevated = c.OperationalDeckElevated,
                                            Remarks = c.Remarks,
                                            CreatedBy = c.CreatedBy,
                                            CreatedDate = c.CreatedDate,
                                            GatesInfo = GetIGCBarrageHWGatesInformationByBarrageHWID(c.IGCBarrageHWID)
                                        }).FirstOrDefault();

            return oIGCBarrageHWInformation;
        }

        public List<object> GetIGCBarrageHWGatesInformationByBarrageHWID(long _IGCBarrageHWID)
        {
            List<object> oIGCBarrageHWGatesInformation = new List<object>();

            oIGCBarrageHWGatesInformation = (from bhw in context.FO_IGCBarrageHW
                                             join bhwg in context.FO_IGCBarrageHWGates on bhw.ID equals bhwg.IGCBarrageHWID
                                             join gt in context.FO_GateType on bhwg.GateTypeID equals gt.ID
                                             join fid in context.FO_FloodInspectionDetail on bhw.FloodInspectionID equals fid.FloodInspectionID
                                             join st in context.CO_Station on new { fid.StructureTypeID, ID = (long)(fid.StructureID == null ? 0 : fid.StructureID) }
                                                                            equals new { st.StructureTypeID, st.ID } into tempStation
                                             from costation in tempStation.DefaultIfEmpty()
                                             join stp in context.CO_StructureTechPara on costation.ID equals stp.StationID into tempStructureTechPara
                                             from finalstrtp in tempStructureTechPara.DefaultIfEmpty()
                                             where
                                             bhwg.IGCBarrageHWID == _IGCBarrageHWID

                                             select new
                                             {
                                                 ID = bhwg.ID,
                                                 GateTypeID = bhwg.GateTypeID,
                                                 GateTypeName = gt.Name,
                                                 WorkingGates = bhwg.WorkingGates,
                                                 TotalGates = finalstrtp.StationID == null ? 0 : bhwg.GateTypeID == 1 ? finalstrtp.ElectricalGatesNo : bhwg.GateTypeID == 2 ? finalstrtp.ElectronicGatesNo : bhwg.GateTypeID == 3 ? finalstrtp.ManualGatesNo : 0,// finalstrtp. .bhwg.TotalGates,
                                                 NotWorkingGates = (finalstrtp.StationID == null ? 0 : bhwg.GateTypeID == 1 ? finalstrtp.ElectricalGatesNo : bhwg.GateTypeID == 2 ? finalstrtp.ElectronicGatesNo : bhwg.GateTypeID == 3 ? finalstrtp.ManualGatesNo : 0) - bhwg.WorkingGates
                                             }).ToList<object>();

            return oIGCBarrageHWGatesInformation;
        }

        public List<object> GetBarrageGatesInformationByFloodInspectionID(long _FloodInspectionID)
        {
            List<object> oBarrageGatesInformation = new List<object>();

            long? StructureTypeID = GetStructureTypeIDByFloodInspectionID(_FloodInspectionID);
            long StructureID = (long)GetStructureIDByFloodInspectionID(_FloodInspectionID);

            oBarrageGatesInformation = (from gt in context.FO_GateType
                                        from st in context.CO_Station.Where(v => v.StructureTypeID == StructureTypeID && v.ID == StructureID).DefaultIfEmpty()
                                        join stp in context.CO_StructureTechPara on st.ID equals stp.StationID into tempStructureTechPara
                                        from finalstrtp in tempStructureTechPara.DefaultIfEmpty()
                                        select new
                                            {
                                                ID = 0,
                                                GateTypeID = gt.ID,
                                                GateTypeName = gt.Name,
                                                WorkingGates = 0,
                                                TotalGates = gt.ID == 1 ? finalstrtp.ElectricalGatesNo : gt.ID == 2 ? finalstrtp.ElectronicGatesNo : gt.ID == 3 ? finalstrtp.ManualGatesNo : 0,
                                                NotWorkingGates = 0
                                            }).ToList<object>();

            return oBarrageGatesInformation;
        }

        public object GetIGCDrainInformationByInspectionID(long _FloodInspectionID)
        {
            object oIGCDrainInformation = new object();

            oIGCDrainInformation = (from dr in context.FO_IGCDrain
                                    //from ic1 in context.FO_InspectionConditions
                                    //        .Where(v => v.ID == bhw.LightConditionID)
                                    //        .DefaultIfEmpty()
                                    //from ic2 in context.FO_InspectionConditions
                                    //        .Where(v => v.ID == bhw.DataBoardConditionID)
                                    //        .DefaultIfEmpty()
                                    //from ic3 in context.FO_InspectionConditions
                                    //        .Where(v => v.ID == bhw.TollHutConditionID)
                                    //        .DefaultIfEmpty()
                                    //from ic4 in context.FO_InspectionConditions
                                    //        .Where(v => v.ID == bhw.ArmyCPConditionID)
                                    //        .DefaultIfEmpty()
                                    where
                                            dr.FloodInspectionID == _FloodInspectionID
                                    select new
                                    {
                                        IGCDrainID = dr.ID,
                                        ExistingCapacity = dr.ExistingCapacity,
                                        ImprovedCapacity = dr.ImprovedCapacity,
                                        CurrentLevel = dr.CurrentLevel,
                                        DrainWaterET = dr.DrainWaterET,
                                        OutfallBedWidth = dr.OutfallBedWidth,
                                        OutfallFullSupplyDepth = dr.OutfallFullSupplyDepth,
                                        BridgeGovtNo = dr.BridgeGovtNo,
                                        BridgePvtNo = dr.BridgePvtNo,
                                        Remarks = dr.Remarks,
                                        CreatedBy = dr.CreatedBy,
                                        CreatedDate = dr.CreatedDate
                                    }).Distinct().FirstOrDefault();

            return oIGCDrainInformation;
        }

        /// <summary>
        /// This function get IBreachingSection information by _FloodInspectionID from FO_IBreachingSection
        /// Created on 11-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIBreachingSectionByInspectionID(long _FloodInspectionID)
        {

            long? StructureID = GetStructureIDByFloodInspectionID(_FloodInspectionID);

            List<object> lstIBreachingSection = (from ibs in context.FO_InfrastructureBreachingSection
                                                 join fid in context.FO_FloodInspectionDetail on new { ibs.ProtectionInfrastructureID, StructureTypeID = ibs.FO_ProtectionInfrastructure.InfrastructureTypeID }
                                                                                              equals new { ProtectionInfrastructureID = fid.StructureID, fid.StructureTypeID }
                                                 join fbs in context.FO_IBreachingSection on new { fid.FloodInspectionID, ibs.FromRD, ibs.ToRD }
                                                                                          equals new { fbs.FloodInspectionID, fbs.FromRD, fbs.ToRD } into tempIBreachingSectionTable
                                                 from fbs in tempIBreachingSectionTable.DefaultIfEmpty()
                                                 where ibs.ProtectionInfrastructureID == StructureID
                                                 && fid.FloodInspectionID == _FloodInspectionID
                                                 select new
                                                 {
                                                     InfraBreachingSectionID = ibs.ID,
                                                     IBreachingSectionID = (long?)fbs.ID,
                                                     FromRD = ibs.FromRD,
                                                     ToRD = ibs.ToRD,
                                                     AffectedRowsNo = (int?)fbs.AffectedRowsNo,
                                                     AffectedLinersNo = (int?)fbs.AffectedLinersNo,
                                                     RecommendedSolution = fbs.RecommendedSolution,
                                                     RestorationCost = (int?)fbs.RestorationCost,
                                                     CreatedBy = (int?)fbs.CreatedBy,
                                                     CreatedDate = (DateTime?)fbs.CreatedDate
                                                 }).ToList().Select(c => new
                                                    {
                                                        InfraBreachingSectionID = c.InfraBreachingSectionID,
                                                        IBreachingSectionID = c.IBreachingSectionID,
                                                        FromRDTotal = c.FromRD,
                                                        ToRDTotal = c.ToRD,
                                                        FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                                        ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                                        AffectedRowsNo = c.AffectedRowsNo,
                                                        AffectedLinersNo = c.AffectedLinersNo,
                                                        RecommendedSolution = c.RecommendedSolution,
                                                        RestorationCost = c.RestorationCost,
                                                        CreatedBy = c.CreatedBy,
                                                        CreatedDate = c.CreatedDate
                                                    }).ToList<object>();
            return lstIBreachingSection;
        }

        /// <summary>
        /// This function get StructureID by _FloodInspectionID from FO_FloodInspectionDetail
        /// Created on 11-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>long?</returns>
        public long? GetStructureIDByFloodInspectionID(long _FloodInspectionID)
        {
            long? structureID = null;
            structureID = (from fid in context.FO_FloodInspectionDetail.Where(d => d.FloodInspectionID == _FloodInspectionID) select new { StructureID = fid.StructureID }).Distinct().FirstOrDefault().StructureID;

            return structureID;
        }


        #region R D Wise Conditon
        public List<object> GetRDWiseConditionForStonePitchingByID(long _FloodInspectionID, int _RDWiseTypeID)
        {

            List<object> lstDWiseConditionForStonePitching = (from ird in context.FO_IRDWiseCondition
                                                              join sp in context.FO_StonePitchSide on ird.StonePitchSideID equals sp.ID
                                                              join ic in context.FO_InspectionConditions on ird.ConditionID equals ic.ID
                                                              where ird.FloodInspectionID == _FloodInspectionID && ird.RDWiseTypeID == _RDWiseTypeID
                                                              select new
                                                              {
                                                                  ID = ird.ID,
                                                                  SideID = ird.StonePitchSideID,
                                                                  SideName = sp.Name,
                                                                  ConditionID = ird.ConditionID,
                                                                  ConditionName = ic.Name,
                                                                  FromRD = ird.FromRD,
                                                                  ToRD = ird.ToRD,
                                                                  Remarks = ird.Remarks,
                                                                  CreatedBy = ird.CreatedBy,
                                                                  CreatedDate = ird.CreatedDate

                                                              }).ToList().Select(c => new
                                                            {
                                                                ID = c.ID,
                                                                SideID = c.SideID,
                                                                SideName = c.SideName,
                                                                ConditionID = c.ConditionID,
                                                                ConditionName = c.ConditionName,
                                                                Remarks = c.Remarks,
                                                                FromRDTotal = c.FromRD,
                                                                ToRDTotal = c.ToRD,
                                                                FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                                                ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                                                CreatedBy = c.CreatedBy,
                                                                CreatedDate = c.CreatedDate

                                                            }).ToList<object>();
            return lstDWiseConditionForStonePitching;

        }

        public List<object> GetRDWiseConditionForStonePitchingApronByID(long _FloodInspectionID, int _RDWiseTypeID)
        {

            List<object> lstDWiseConditionForStonePitching = (from ird in context.FO_IRDWiseCondition
                                                              join ic in context.FO_InspectionConditions on ird.ConditionID equals ic.ID
                                                              where ird.FloodInspectionID == _FloodInspectionID && ird.RDWiseTypeID == _RDWiseTypeID
                                                              select new
                                                              {
                                                                  ID = ird.ID,
                                                                  ConditionID = ird.ConditionID,
                                                                  ConditionName = ic.Name,
                                                                  FromRD = ird.FromRD,
                                                                  ToRD = ird.ToRD,
                                                                  Remarks = ird.Remarks,
                                                                  CreatedBy = ird.CreatedBy,
                                                                  CreatedDate = ird.CreatedDate

                                                              }).ToList().Select(c => new
                                                              {
                                                                  ID = c.ID,
                                                                  ConditionID = c.ConditionID,
                                                                  ConditionName = c.ConditionName,
                                                                  Remarks = c.Remarks,
                                                                  FromRDTotal = c.FromRD,
                                                                  ToRDTotal = c.ToRD,
                                                                  FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                                                  ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                                                  CreatedBy = c.CreatedBy,
                                                                  CreatedDate = c.CreatedDate

                                                              }).ToList<object>();
            return lstDWiseConditionForStonePitching;

        }
        public List<object> GetEncroachmentByID(long _FloodInspectionID, int _RDWiseTypeID)
        {

            List<object> lstEncroachment = (from ird in context.FO_IRDWiseCondition
                                            join et in context.FO_EncroachmentType on ird.EncroachmentTypeID equals et.ID
                                            where ird.FloodInspectionID == _FloodInspectionID && ird.RDWiseTypeID == _RDWiseTypeID
                                            select new
                                            {
                                                ID = ird.ID,
                                                EncroachmentTypeId = ird.EncroachmentTypeID,
                                                EncroachmentTypeName = et.Name,
                                                FromRD = ird.FromRD,
                                                ToRD = ird.ToRD,
                                                Remarks = ird.Remarks,
                                                CreatedBy = ird.CreatedBy,
                                                CreatedDate = ird.CreatedDate

                                            }).ToList().Select(c => new
                                                              {
                                                                  ID = c.ID,
                                                                  EncroachmentTypeId = c.EncroachmentTypeId,
                                                                  EncroachmentTypeName = c.EncroachmentTypeName,
                                                                  Remarks = c.Remarks,
                                                                  FromRDTotal = c.FromRD,
                                                                  ToRDTotal = c.ToRD,
                                                                  FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                                                  ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                                                  CreatedBy = c.CreatedBy,
                                                                  CreatedDate = c.CreatedDate

                                                              }).ToList<object>();
            return lstEncroachment;

        }
        public object GetStonePitchingRDConditionForByID(long _InspectionID)
        {

            object StonePitchingCondition = (from ird in context.FO_IRDWiseCondition
                                             join sp in context.FO_StonePitchSide on ird.StonePitchSideID equals sp.ID
                                             join ic in context.FO_InspectionConditions on ird.ConditionID equals ic.ID
                                             where ird.ID == _InspectionID
                                             select new
                                             {
                                                 ID = ird.ID,
                                                 SideID = ird.StonePitchSideID,
                                                 SideName = sp.Name,
                                                 ConditionID = ird.ConditionID,
                                                 ConditionName = ic.Name,
                                                 FromRD = ird.FromRD,
                                                 ToRD = ird.ToRD,
                                                 Remarks = ird.Remarks,
                                                 CreatedBy = ird.CreatedBy,
                                                 CreatedDate = ird.CreatedDate

                                             }).AsEnumerable().Select(c => new
                                                              {
                                                                  ID = c.ID,
                                                                  SideID = c.SideID,
                                                                  SideName = c.SideName,
                                                                  ConditionID = c.ConditionID,
                                                                  ConditionName = c.ConditionName,
                                                                  Remarks = c.Remarks,
                                                                  FromRDTotal = c.FromRD,
                                                                  ToRDTotal = c.ToRD,
                                                                  FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                                                  ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                                                  CreatedBy = c.CreatedBy,
                                                                  CreatedDate = c.CreatedDate

                                                              }).FirstOrDefault();
            return StonePitchingCondition;

        }

        public object GetIRDConditionForByID(long _InspectionID)
        {

            object RDWiseCondition = (from ird in context.FO_IRDWiseCondition
                                      join ic in context.FO_InspectionConditions on ird.ConditionID equals ic.ID
                                      where ird.ID == _InspectionID
                                      select new
                                      {
                                          ID = ird.ID,
                                          ConditionID = ird.ConditionID,
                                          ConditionName = ic.Name,
                                          FromRD = ird.FromRD,
                                          ToRD = ird.ToRD,
                                          Remarks = ird.Remarks,
                                          CreatedBy = ird.CreatedBy,
                                          CreatedDate = ird.CreatedDate

                                      }).AsEnumerable().Select(c => new
                                                              {
                                                                  ID = c.ID,
                                                                  ConditionID = c.ConditionID,
                                                                  ConditionName = c.ConditionName,
                                                                  Remarks = c.Remarks,
                                                                  FromRDTotal = c.FromRD,
                                                                  ToRDTotal = c.ToRD,
                                                                  FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                                                  ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                                                  CreatedBy = c.CreatedBy,
                                                                  CreatedDate = c.CreatedDate

                                                              }).FirstOrDefault();
            return RDWiseCondition;

        }
        public object GetEncroachmentRDConditionForByID(long _InspectionID)
        {

            object Encroachment = (from ird in context.FO_IRDWiseCondition
                                   join et in context.FO_EncroachmentType on ird.EncroachmentTypeID equals et.ID
                                   where ird.ID == _InspectionID
                                   select new
                                   {
                                       ID = ird.ID,
                                       EncroachmentTypeId = ird.EncroachmentTypeID,
                                       EncroachmentTypeName = et.Name,
                                       FromRD = ird.FromRD,
                                       ToRD = ird.ToRD,
                                       Remarks = ird.Remarks,
                                       CreatedBy = ird.CreatedBy,
                                       CreatedDate = ird.CreatedDate

                                   }).AsEnumerable().Select(c => new
                                            {
                                                ID = c.ID,
                                                EncroachmentTypeId = c.EncroachmentTypeId,
                                                EncroachmentTypeName = c.EncroachmentTypeName,
                                                Remarks = c.Remarks,
                                                FromRDTotal = c.FromRD,
                                                ToRDTotal = c.ToRD,
                                                FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                                ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                                CreatedBy = c.CreatedBy,
                                                CreatedDate = c.CreatedDate

                                            }).FirstOrDefault();
            return Encroachment;

        }
        #endregion R D Wise Conditon


        #region MeasuringBookStatus

        //public List<object> GetMBStatusPreItemList(long _CategoryID, int _FloodInspectionID)
        //{

        //    var lstMBPreSearch = (from fi in context.FO_Items
        //                          join fc in context.FO_ItemCategory on fi.ItemCategoryID equals fc.ID
        //                          join PMBS in context.FO_PreMBStatus on fi.ID equals PMBS.ItemID
        //                          where
        //                          (
        //                          (fc.ID == _CategoryID)
        //                          && (PMBS.FloodInspectionID == _FloodInspectionID)

        //                          )
        //                          select new
        //                          {
        //                              ID = PMBS.ID,
        //                              ItemID = fi.ID,
        //                              LYQty = PMBS.LastYrQty,
        //                              DIVIssueQty = PMBS.DivStrIssuedQty,
        //                              AvaQty = PMBS.AvailableQty,
        //                              BalanceQty = PMBS.DivStrIssuedQty - PMBS.AvailableQty
        //                          }).ToList();

        //    var ItemList = (from fi in context.FO_Items
        //                    join fc in context.FO_ItemCategory on fi.ItemCategoryID equals fc.ID
        //                    where (fc.ID == _CategoryID)
        //                    select new
        //                    {
        //                        ID = 0,
        //                        ItemID = fi.ID,
        //                        MajorMinor = fi.MajorMinor,
        //                        ItemName = fi.Name,
        //                        LYQty = 0,
        //                        DIVIssueQty = 0,
        //                        AvaQty = 0,
        //                        BalanceQty = 0
        //                    }).ToList();

        //    List<object> Result = new List<object>();

        //    if (lstMBPreSearch.Count != 0)
        //    {
        //        foreach (var item in ItemList)
        //        {
        //            foreach (var MBVal in lstMBPreSearch)
        //            {
        //                if (item.ItemID == MBVal.ItemID)
        //                {
        //                    Result.Add(new { MBVal.ID, item.ItemID, item.MajorMinor, item.ItemName, MBVal.LYQty, MBVal.DIVIssueQty, MBVal.AvaQty, MBVal.BalanceQty });
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var item in ItemList)
        //        {
        //            Result.Add(new { item.ID, item.ItemID, item.MajorMinor, item.ItemName, item.LYQty, item.DIVIssueQty, item.AvaQty, item.BalanceQty });
        //        }
        //    }

        //    return Result;

        //}

        public List<object> GetMBStatusPostItemList(long _CategoryID, int _FloodInspectionID)
        {
            //long FloodInspectionDetailID = 0;
            //object Obj = GetInspectionDivisionID(_FloodInspectionID);
            //if (Obj != null)
            //{
            //    FloodInspectionDetailID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Obj, "FloodInspectionDetailID"));
            //}
            ////--To DO FO Team
            //var lstMBPostSearch = (from fi in context.FO_Items
            //                       join fc in context.FO_ItemCategory on fi.ItemCategoryID equals fc.ID
            //                       join PMBS in context.FO_OverallDivItems on fi.ID equals PMBS.ItemSubcategoryID

            //                       where
            //                       (
            //                       (fc.ID == _CategoryID)
            //                       && (PMBS.FloodInspectionDetailID == FloodInspectionDetailID)

            //                       )
            //                       select new
            //                       {
            //                           ID = PMBS.ID,
            //                           ItemID = fi.ID,
            //                           MajorMinor = fi.MajorMinor,
            //                           ItemName = fi.Name,
            //                           AvaQty = PMBS.PostAvailableQty,
            //                           ConsumedQty = PMBS.PreFloodQty - PMBS.PostAvailableQty,
            //                           RequiredQty = PMBS.PostRequiredQty
            //                       }).ToList();


            //var ItemList = (from fi in context.FO_Items
            //                join fc in context.FO_ItemCategory on fi.ItemCategoryID equals fc.ID
            //                where (fc.ID == _CategoryID)
            //                select new
            //                {
            //                    ID = 0,
            //                    ItemID = fi.ID,
            //                    MajorMinor = fi.MajorMinor,
            //                    ItemName = fi.Name,
            //                    PreFloodQty = 0,
            //                    AvaQty = 0,
            //                    ConsumedQty = 0,
            //                    RequiredQty = 0

            //                }).ToList();

            List<object> Result = new List<object>();

            //if (lstMBPostSearch.Count != 0)
            //{
            //    foreach (var item in ItemList)
            //    {
            //        foreach (var MBVal in lstMBPostSearch)
            //        {
            //            if (item.ItemID == MBVal.ItemID)
            //            {
            //                Result.Add(new { MBVal.ID, item.ItemID, item.MajorMinor, item.ItemName, MBVal.PreFloodQty, MBVal.AvaQty, MBVal.ConsumedQty, MBVal.RequiredQty });
            //            }

            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var item in ItemList)
            //    {
            //        Result.Add(new { item.ID, item.ItemID, item.MajorMinor, item.ItemName, item.PreFloodQty, item.AvaQty, item.ConsumedQty, item.RequiredQty });
            //    }
            //}

            return Result;

        }
        public object GetMBStatusByID(long _StatusID, int _StatusType)
        {
            object _MBStatus = null;
            //if (_StatusType == 1)
            //{
            //    _MBStatus = (from fi in context.FO_Items
            //                 join fc in context.FO_ItemCategory on fi.ItemCategoryID equals fc.ID
            //                 join PMBS in context.FO_PreMBStatus on fi.ID equals PMBS.ItemID
            //                 where PMBS.ID == _StatusID
            //                 select new
            //                 {
            //                     ID = PMBS.ID,
            //                     ItemID = fi.ID,
            //                     LYQty = PMBS.LastYrQty,
            //                     DIVIssueQty = PMBS.DivStrIssuedQty,
            //                     AvaQty = PMBS.AvailableQty,
            //                     BalanceQty = PMBS.DivStrIssuedQty - PMBS.AvailableQty
            //                 }).AsEnumerable().FirstOrDefault();
            //}
            //else if (_StatusType == 2)
            //{
            //    _MBStatus = (from fi in context.FO_Items
            //                 join fc in context.FO_ItemCategory on fi.ItemCategoryID equals fc.ID
            //                 join PMBS in context.FO_PostMBStatus on fi.ID equals PMBS.ItemID
            //                 where PMBS.ID == _StatusID
            //                 select new
            //                 {
            //                     ID = PMBS.ID,
            //                     ItemID = fi.ID,
            //                     MajorMinor = fi.MajorMinor,
            //                     ItemName = fi.Name,
            //                     PreFloodQty = PMBS.PreFloodQty,
            //                     AvaQty = PMBS.AvailableQtyPost,
            //                     ConsumedQty = PMBS.PreFloodQty - PMBS.AvailableQtyPost,
            //                     RequiredQty = PMBS.RequiredQtyPost
            //                 }).AsEnumerable().FirstOrDefault();
            //}
            return _MBStatus;

        }

        public int GetInspectionStatus(long _FloodInspectionID)
        {
            int Status = (from s in context.FO_FloodInspection
                          where (s.ID == _FloodInspectionID)
                          select s.InspectionStatusID).FirstOrDefault();

            return Status;

        }


        #endregion

        #region ProblemForFI
        public List<object> GetProblemForFIByFloodInspectionID(long _FloodInspectionID)
        {
            string res = Calculations.GetRDText(Convert.ToInt64(1001));


            List<object> lstProlemForFI = (from fi in context.FO_FloodInspection
                                           join Ip in context.FO_IProblems on fi.ID equals Ip.FloodInspectionID
                                           join Pn in context.FO_ProblemNature on Ip.ProblemID equals Pn.ID
                                           where fi.ID == _FloodInspectionID
                                           select new
                                           {
                                               ID = Ip.ID,
                                               FloodInspectionID = Ip.FloodInspectionID,
                                               FromRD = Ip.FromRD,
                                               ToRD = Ip.ToRD,
                                               NatureofProblemID = Ip.ProblemID,
                                               NatureofProblem = Pn.Name,
                                               RecommendedSolution = Ip.Solution,
                                               TentativeCostofRestoration = Ip.RestorationCost,
                                               CreatedDate = Ip.CreatedDate,
                                               CreatedBy = Ip.CreatedBy
                                           }).ToList().Select(c => new
                                                            {
                                                                ID = c.ID,
                                                                FloodInspectionID = c.FloodInspectionID,
                                                                FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                                                ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                                                FromRDTotal = c.FromRD,
                                                                ToRDTotal = c.ToRD,
                                                                NatureofProblemID = c.NatureofProblemID,
                                                                NatureofProblem = c.NatureofProblem,
                                                                RecommendedSolution = c.RecommendedSolution,
                                                                TentativeCostofRestoration = c.TentativeCostofRestoration,
                                                                CreatedDate = c.CreatedDate,
                                                                CreatedBy = c.CreatedBy
                                                            }).ToList<object>();
            return lstProlemForFI;

        }
        public object GetProblemForFIByInspectionID(long _InspectionID)
        {
            // string res = Calculations.GetRDText(Convert.ToInt64(1001));


            object ProlemForFI = (from fi in context.FO_FloodInspection
                                  join Ip in context.FO_IProblems on fi.ID equals Ip.FloodInspectionID
                                  join Pn in context.FO_ProblemNature on Ip.ProblemID equals Pn.ID
                                  where Ip.ID == _InspectionID
                                  select new
                                  {
                                      ID = Ip.ID,
                                      FloodInspectionID = Ip.FloodInspectionID,
                                      FromRD = Ip.FromRD,
                                      ToRD = Ip.ToRD,
                                      NatureofProblemID = Ip.ProblemID,
                                      NatureofProblem = Pn.Name,
                                      RecommendedSolution = Ip.Solution,
                                      TentativeCostofRestoration = Ip.RestorationCost,
                                      CreatedDate = Ip.CreatedDate,
                                      CreatedBy = Ip.CreatedBy
                                  }).AsEnumerable().Select(c => new
                                           {
                                               ID = c.ID,
                                               FloodInspectionID = c.FloodInspectionID,
                                               FromRD = Calculations.GetRDText(Convert.ToInt64(c.FromRD)),
                                               ToRD = Calculations.GetRDText(Convert.ToInt64(c.ToRD)),
                                               FromRDTotal = c.FromRD,
                                               ToRDTotal = c.ToRD,
                                               NatureofProblemID = c.NatureofProblemID,
                                               NatureofProblem = c.NatureofProblem,
                                               RecommendedSolution = c.RecommendedSolution,
                                               TentativeCostofRestoration = c.TentativeCostofRestoration,
                                               CreatedDate = c.CreatedDate,
                                               CreatedBy = c.CreatedBy
                                           }).FirstOrDefault();
            return ProlemForFI;

        }
        public long? GetInfrastructureType(long _FloodInspectionID)
        {
            long? StructureTypeID = (from fid in context.FO_FloodInspectionDetail
                                     where (fid.FloodInspectionID == _FloodInspectionID)
                                     select fid.StructureTypeID).FirstOrDefault();

            return StructureTypeID;


        }
        #endregion

        #region IStonePosition
        public List<object> GetIStonePositionByFloodInspectionID(long _FloodInspectionID)
        {
            List<object> lstProlemForFI = (from Sp in context.FO_IStonePosition
                                           where Sp.FloodInspectionID == _FloodInspectionID
                                           select new
                                           {
                                               ID = Sp.ID,
                                               RD = Sp.RD,
                                               QuantityRegistered = Sp.BeforeFloodQty,
                                               AvailableQty = Sp.AvailableQty,
                                               QuantityConsumed = Sp.BeforeFloodQty - Sp.AvailableQty,
                                               CreatedDate = Sp.CreatedDate,
                                               CreatedBy = Sp.CreatedBy
                                           }).ToList().Select(c => new
                                           {
                                               ID = c.ID,
                                               RD = (c.RD == null) ? "" : Calculations.GetRDText(Convert.ToInt64(c.RD)),
                                               QuantityRegistered = c.QuantityRegistered,
                                               AvailableQty = c.AvailableQty,
                                               QuantityConsumed = c.QuantityConsumed,
                                               CreatedDate = c.CreatedDate,
                                               CreatedBy = c.CreatedBy
                                           }).ToList<object>();
            return lstProlemForFI;

        }




        public object GetiStonePosObjbyID(long ID)
        {
            object objistonepos = (from fi in context.FO_IStonePosition

                                   where fi.ID == ID
                                   select new
                                   {
                                       ID = fi.ID,
                                       FloodInspectionID = fi.FloodInspectionID,
                                       RD = fi.RD,
                                       BeforeFloodQty = fi.BeforeFloodQty,
                                       AvailableQty = fi.AvailableQty,
                                       CreatedBy = fi.CreatedBy,
                                       ModifiedBy = fi.ModifiedBy


                                   }
                                                       ).Distinct().FirstOrDefault();



            return objistonepos;
        }





        #endregion

        #endregion

        #region DepartmentalInspection

        public object GetDepartmentalInspectionByID(long _FloodInspectionID)
        {
            object departmentalInspectionDetail = (from fi in context.FO_FloodInspection
                                                   join dn in context.CO_Division
                                                   on fi.DivisionID equals dn.ID
                                                   where fi.ID == _FloodInspectionID && fi.InspectionCategoryID == 2
                                                   select new
                                                   {
                                                       DivisionName = dn.Name,
                                                       InspectionDate = fi.InspectionDate

                                                   }
                                                       ).Distinct().FirstOrDefault();



            return departmentalInspectionDetail;
        }

        #region SearchDepartmentalInspection

        public List<object> GetDepartmentalInspectionSearch(long? _FloodInspectionID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _Status, DateTime? _FromDate, DateTime? _ToDate)
        {
            List<object> SearchList = (from fi in context.FO_FloodInspection
                                       join fis in context.FO_InspectionStatus on fi.InspectionStatusID equals fis.ID
                                       join cd in context.CO_Division on fi.DivisionID equals cd.ID into COD
                                       from CODR in COD.DefaultIfEmpty()
                                       join cc in context.CO_Circle on CODR.CircleID equals cc.ID into COC
                                       from COCR in COC.DefaultIfEmpty()
                                       join cz in context.CO_Zone on COCR.ZoneID equals cz.ID

                                       where (fi.ID == (_FloodInspectionID == null ? fi.ID : _FloodInspectionID)
                                       && fis.ID == (_Status == null ? fis.ID : _Status)
                                       && fi.InspectionCategoryID == 2
                                       && CODR.ID == (_DivisionID == null ? CODR.ID : _DivisionID)
                                       && COCR.ID == (_CircleID == null ? COCR.ID : _CircleID)
                                       && cz.ID == (_ZoneID == null ? cz.ID : _ZoneID)
                                       && fi.InspectionDate >= (_FromDate == null ? fi.InspectionDate : _FromDate)
                                       && fi.InspectionDate <= (_ToDate == null ? fi.InspectionDate : _ToDate)
                                       )
                                       select new
                                       {
                                           FloodInspectionID = fi.ID,
                                           Division = CODR.Name,
                                           InspectionDate = fi.InspectionDate,
                                           InfrastructureStatus = fis.Name,
                                           StatusID = fis.ID,
                                           InspectionYear = fi.Year
                                       }).ToList<object>();

            return SearchList;
        }

        #endregion

        #region Infrastructure


        public List<object> GetInfrastructuresByFloodInspectionID(long _FloodInspectionID)
        {
            List<object> lstInfrastructure = (from fdi in context.FO_DInfrastructures
                                              where fdi.FloodInspectionID == _FloodInspectionID
                                              select new
                                              {
                                                  ID = fdi.ID,
                                                  StructureID = fdi.StructureID,
                                                  StructureTypeID = fdi.StructureTypeID,
                                                  CreatedBy = fdi.CreatedBy,
                                                  CreatedDate = fdi.CreatedDate
                                              }).ToList<object>();

            return lstInfrastructure;

        }

        public bool IsDInfrastructureUnique(FO_DInfrastructures _DInfrastructures)
        {
            bool chkInfrastructure = (from fdi in context.FO_DInfrastructures
                                      where fdi.FloodInspectionID == _DInfrastructures.FloodInspectionID
                                      && fdi.StructureID == _DInfrastructures.StructureID
                                      && fdi.StructureTypeID == _DInfrastructures.StructureTypeID
                                      select fdi).Any();

            return chkInfrastructure;
        }
        #endregion

        #region MemberDetails
        public List<object> GetMemberDetailsByFloodInspectionID(long _FloodInspectionID)
        {
            List<object> lstMemberDetails = (from fmd in context.FO_DMemberDetails
                                             where fmd.FloodInspectionID == _FloodInspectionID
                                             select new
                                             {
                                                 ID = fmd.ID,
                                                 Name = fmd.Name,
                                                 Designation = fmd.Designation,
                                                 CreatedBy = fmd.CreatedBy,
                                                 CreatedDate = fmd.CreatedDate
                                             }).ToList<object>();

            return lstMemberDetails;

        }

        public bool IsDMembersUnique(FO_DMemberDetails _DMemberDetails)
        {

            bool chkMembers = (from fmd in context.FO_DMemberDetails
                               where fmd.FloodInspectionID == _DMemberDetails.FloodInspectionID
                               && fmd.Name.Contains(_DMemberDetails.Name)
                               && fmd.Designation.Contains(_DMemberDetails.Designation)
                               && (_DMemberDetails.ID == 0 || fmd.ID != _DMemberDetails.ID)
                               select fmd).Any();

            return chkMembers;
        }

        #endregion

        #region Attachments
        public List<object> GetAttachmentsByFloodInspectionID(long _FloodInspectionID)
        {
            List<object> lstAttachments = (from fda in context.FO_DAttachments
                                           where fda.FloodInspectionID == _FloodInspectionID
                                           select new
                                           {
                                               ID = fda.ID,
                                               FileName = fda.FileURL,
                                               CreatedBy = fda.CreatedBy,
                                               CreatedDate = fda.CreatedDate
                                           }).ToList<object>();

            return lstAttachments;

        }

        public bool AttachmentDublication(long _ID, string _FileName)
        {
            bool result = (from fda in context.FO_DAttachments
                           where fda.FloodInspectionID == _ID
                           && fda.FileURL == _FileName
                           select
                               fda).Any();

            return result;
        }
        #endregion

        #endregion


        #region JointInspection
        public object GetjointInspectionDetail(long _FloodInspectionID)
        {
            object JoinListDetail = (from fi in context.FO_FloodInspection
                                     join cd in context.CO_Division on fi.DivisionID equals cd.ID
                                     where fi.ID == _FloodInspectionID && fi.InspectionCategoryID == 3
                                     select new
                                     {
                                         FloodInspectionID = fi.ID,
                                         DivisionID = fi.DivisionID,
                                         DivisionName = cd.Name,
                                         InspectionDate = fi.InspectionDate,
                                     }).Distinct().FirstOrDefault();

            return JoinListDetail;
        }

        public List<object> GetjointInspectionSearch(long? _FloodInspectionID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _Status, DateTime? _FromDate, DateTime? _ToDate)
        {
            List<object> SearchJoinList = (from fi in context.FO_FloodInspection
                                           join fis in context.FO_InspectionStatus on fi.InspectionStatusID equals fis.ID
                                           join cd in context.CO_Division on fi.DivisionID equals cd.ID into COD
                                           from CODR in COD.DefaultIfEmpty()
                                           join cc in context.CO_Circle on CODR.CircleID equals cc.ID into COC
                                           from COCR in COC.DefaultIfEmpty()
                                           join cz in context.CO_Zone on COCR.ZoneID equals cz.ID

                                           where (fi.ID == (_FloodInspectionID == null ? fi.ID : _FloodInspectionID)
                                           && fis.ID == (_Status == null ? fis.ID : _Status)
                                           && fi.InspectionCategoryID == 3
                                           && CODR.ID == (_DivisionID == null ? CODR.ID : _DivisionID)
                                           && COCR.ID == (_CircleID == null ? COCR.ID : _CircleID)
                                           && cz.ID == (_ZoneID == null ? cz.ID : _ZoneID)
                                           && fi.InspectionDate >= (_FromDate == null ? fi.InspectionDate : _FromDate)
                                           && fi.InspectionDate <= (_ToDate == null ? fi.InspectionDate : _ToDate)
                                           )
                                           select new
                                           {
                                               FloodInspectionID = fi.ID,
                                               DivisionID = fi.DivisionID,
                                               DivisionName = CODR.Name,
                                               InspectionDate = fi.InspectionDate,
                                               InfrastructureStatusID = fi.InspectionStatusID,
                                               InfrastructureStatusName = fis.Name,
                                               InspectionYear = fi.Year
                                           }).ToList<object>().Distinct().ToList<object>();

            return SearchJoinList;
        }
        public List<object> GetInfrastructuresForJointInspection(long _FloodInspectionID)
        {
            List<object> lstInfrastructure = (from fdi in context.FO_JInfrastructures
                                              where fdi.FloodInspectionID == _FloodInspectionID
                                              select new
                                              {
                                                  ID = fdi.ID,
                                                  StructureID = fdi.StructureID,
                                                  StructureTypeID = fdi.StructureTypeID,
                                                  CreatedBy = fdi.CreatedBy,
                                                  CreatedDate = fdi.CreatedDate
                                              }).ToList<object>();

            return lstInfrastructure;

        }
        public List<object> GetJointMemberDetails(long _FloodInspectionID)
        {
            List<object> lstJointMemberDetails = (from fmd in context.FO_JMemberDetails
                                                  where fmd.FloodInspectionID == _FloodInspectionID
                                                  select new
                                                  {
                                                      ID = fmd.ID,
                                                      Name = fmd.Name,
                                                      Department = fmd.Department,
                                                      CreatedBy = fmd.CreatedBy,
                                                      CreatedDate = fmd.CreatedDate
                                                  }).ToList<object>();

            return lstJointMemberDetails;

        }
        public List<object> GetJointAttachmentsDetails(long _FloodInspectionID)
        {
            List<object> lstJointMemberDetails = (from fa in context.FO_JAttachments
                                                  where fa.FloodInspectionID == _FloodInspectionID
                                                  select new
                                                  {
                                                      ID = fa.ID,
                                                      FileURL = fa.FileURL,
                                                      CreatedBy = fa.CreatedBy,
                                                      CreatedDate = fa.CreatedDate
                                                  }).ToList<object>();

            return lstJointMemberDetails;

        }
        public bool IsJIFInfrastructureExist(FO_JInfrastructures _FO_JInfrastructures)
        {

            bool chkInfrastructure = (from JIF in context.FO_JInfrastructures
                                      where JIF.FloodInspectionID == _FO_JInfrastructures.FloodInspectionID
                                            && JIF.StructureID == _FO_JInfrastructures.StructureID
                                            && JIF.StructureTypeID == _FO_JInfrastructures.StructureTypeID
                                      select JIF).Any();

            return chkInfrastructure;
        }
        public bool IsJIFInfrastructureExistUpdate(FO_JInfrastructures _FO_JInfrastructures)
        {

            bool chkInfrastructure = (from JIF in context.FO_JInfrastructures
                                      where JIF.FloodInspectionID == _FO_JInfrastructures.FloodInspectionID
                                            && JIF.StructureID == _FO_JInfrastructures.StructureID
                                            && JIF.StructureTypeID == _FO_JInfrastructures.StructureTypeID
                                            && JIF.ID != _FO_JInfrastructures.ID
                                      select JIF).Any();

            return chkInfrastructure;
        }

        #endregion JointInspection

        public object GetInspectionDivisionID(long _FloodInspectionID)
        {
            object Obj = (from fi in context.FO_FloodInspection
                          join fid in context.FO_FloodInspectionDetail
                          on fi.ID equals fid.FloodInspectionID
                          where fi.ID == _FloodInspectionID
                          select new
                          {
                              DivisionID = fi.DivisionID,
                              year = fi.Year,
                              FloodInspectionDetailID = fid.ID,
                              StructureTypeID = fid.StructureTypeID,
                              StructureID = fid.StructureID

                          }
                            ).Distinct().FirstOrDefault();



            return Obj;
        }



        public object GET_PostMBStatus_Object(long ID)
        {
            object Post_MB_List = (from MDA in context.FO_OverallDivItems

                                   where MDA.ID == ID

                                   select new
                                   {
                                       ID = MDA.ID,
                                       Year = MDA.Year,
                                       DivisionID = MDA.DivisionID,
                                       ItemCategoryID = MDA.ItemCategoryID,
                                       ItemSubcategoryID = MDA.ItemSubcategoryID,

                                       StructureTypeID = MDA.StructureTypeID,

                                       StructureID = MDA.StructureID,
                                       PreMBStatusID = MDA.PreMBStatusID,
                                       FloodInspectionDetailID = MDA.FloodInspectionDetailID,
                                       PostAvailableQty = MDA.PostAvailableQty,
                                       PostRequiredQty = MDA.PostRequiredQty,
                                       CS_CampSiteID = MDA.CS_CampSiteID,
                                       CS_RequiredQty = MDA.CS_RequiredQty,
                                       OD_AdditionalQty = MDA.OD_AdditionalQty,


                                       CreatedDate = MDA.CreatedDate,
                                       CreatedBy = MDA.CreatedBy


                                   }).FirstOrDefault();






            return Post_MB_List;


        }



        public object GET_PreMBStatus_Object(long ID)
        {
            object Pre_MB_List = (from MDA in context.FO_PreMBStatus
                                  where MDA.ID == ID

                                  select new
                                  {
                                      ID = MDA.ID,
                                      FloodInspectionID = MDA.FloodInspectionID,
                                      ItemID = MDA.ItemID,
                                      LastYrQty = MDA.LastYrQty,
                                      DivStrIssuedQty = MDA.DivStrIssuedQty,
                                      AvailableQty = MDA.AvailableQty,
                                      CreatedBy = MDA.CreatedBy,

                                      CreatedDate = MDA.CreatedDate



                                  }).FirstOrDefault();






            return Pre_MB_List;


        }



        public object GetFIDetailIDbyFIID(long FDID)
        {
            object Post_MB_List = (from MDA in context.FO_FloodInspectionDetail

                                   where MDA.FloodInspectionID == FDID

                                   select new
                                   {

                                       ID = MDA.ID

                                   }).FirstOrDefault();




            return Post_MB_List;


        }


        #region Notification

        public FO_GetFloodInspectionNotifyData_Result GetFloodInspectionNotifyData(long _FloodInspectionID)
        {
            FO_GetFloodInspectionNotifyData_Result lstFloodInspectionNotifyData = context.FO_GetFloodInspectionNotifyData(_FloodInspectionID).FirstOrDefault<FO_GetFloodInspectionNotifyData_Result>();

            return lstFloodInspectionNotifyData;
        }

        #endregion  Notification

    }
}