using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Common
{
    public class ReportConstants
    {
        public static string ReportPath { get { return "/PMIU.WRMIS/" + Common.Utility.ReadConfiguration("ENV") + "/Reports/"; } }
        public static string rptChannelDischargeHistory { get { return ReportPath + "CO_ChannelDischargeHistory"; } }
        public static string rptChannelIndent { get { return ReportPath + "CO_ChannelIndent"; } }
        public static string rptClosedChannels { get { return ReportPath + "CO_ClosedChannels"; } }
        public static string rptGaugeReadingComparison { get { return ReportPath + "CO_GaugeReadingComparison"; } }
        public static string rptGaugeStatusComparison { get { return ReportPath + "CO_GaugeStatusComparison"; } }
        public static string rptGaugeStatusFieldStaff { get { return ReportPath + "CO_GaugeStatusFieldStaff"; } }
        public static string rptTailStatusFieldStaff { get { return ReportPath + "CO_TailStatusFieldStaff"; } }
        public static string rptTailStatusPMIUStaff { get { return ReportPath + "CO_TailStatusPMIUStaff"; } }
        public static string rptChangesInGaugeReadings { get { return ReportPath + "CO_ChangeInGaugeReadings"; } }
        public static string rptDailyAnalysis { get { return ReportPath + "CO_DailyAnalysisReport"; } }
        public static string rptDailyChannelStatus { get { return ReportPath + "CO_DailyChannelStatus"; } }
        public static string rptComparisonShortDryTails { get { return ReportPath + "CO_ComparisonShortDryTails"; } }

        #region "Schedule Inspection Reports Path"
        public static string rptGaugeInspection { get { return ReportPath + "SI_GaugeInspection"; } }
        public static string rptOutletPerformance { get { return ReportPath + "SI_OutletPerformance"; } }
        public static string rptOutletAlteration { get { return ReportPath + "SI_OutletAlteration"; } }
        public static string rptDischargeTableCalculation { get { return ReportPath + "SI_DischargeTableCalculation"; } }
        #endregion

        #region "Water Theft Reports Path"
        public static string rptWaterTheftChannel { get { return ReportPath + "WT_WaterTheftChannels"; } }
        public static string rptWaterTheftOutlet { get { return ReportPath + "WT_WaterTheftOutlet"; } }
        public static string rptBreach { get { return ReportPath + "WT_Breach"; } }
        #endregion
        public static string rptWaterLosses { get { return ReportPath + "WL_DivisionLG"; } }


        #region "Flood Operations Reports"
        public static string rptStoneInformation { get { return ReportPath + "FO_StoneInformation"; } }
        public static string rptDivisionSummary_Printable { get { return ReportPath + "FO_DivisionSummary_Printable"; } }
        public static string rptEmergencyPurchases { get { return ReportPath + "FO_EmergencyPurchases"; } }
        public static string rptEmergencyWorks { get { return ReportPath + "FO_EmergencyWork"; } }
        public static string rptFloodFightingPlanPrintable { get { return ReportPath + "FO_FloodFightingPlanPrintable"; } }
        public static string rptFloodInspection { get { return ReportPath + "FO_FloodInspection"; } }
        public static string rptOnsiteCampSiteMonitoring { get { return ReportPath + "FO_OnsiteCampSiteMonitoring"; } }
        public static string rptOnsiteStoneMonitoring { get { return ReportPath + "FO_OnsiteStoneMonitoring"; } }

        #endregion


        #region "Complaint Reports"
        public static string rptComplaintSource { get { return ReportPath + "CM_ComplaintSource"; } }
        public static string rptComplaintStatus { get { return ReportPath + "CM_ComplaintStatus"; } }
        public static string rptComplaintType { get { return ReportPath + "CM_ComplaintType"; } }
        public static string rptComplaintDetail { get { return ReportPath + "CM_ComplaintDetail"; } }
        public static string rptComplaintDirective { get { return ReportPath + "CM_ComplaintDirective"; } }
        public static string rptComplaintStructure { get { return ReportPath + "CM_ComplaintStructure"; } }
        #endregion

        #region "Daily Data"
        public static string rptMeterReading { get { return ReportPath + "SM_MeterReading"; } }
        public static string rptFuel { get { return ReportPath + "SM_FuelReading"; } }
        public static string rptGaugeObservation { get { return ReportPath + "SM_ChannelObservation"; } }
        public static string rptOutletChecking { get { return ReportPath + "SM_OutletChecking"; } }
        public static string rptCutBreach { get { return ReportPath + "SM_CutAndBreach"; } }
        public static string rptSMRotationalViolation { get { return ReportPath + "SM_RotationalViolation"; } }
        public static string rptLeaves { get { return ReportPath + "SM_Leaves"; } }
        public static string rptSMWaterTheftOutlet { get { return ReportPath + "DD_WaterTheftOutlet"; } }
        public static string rptSMWaterTheftChannel { get { return ReportPath + "DD_WaterTheftChannel"; } }
        public static string rptAll { get { return ReportPath + "SM_AllAciivity"; } }
        #endregion

        #region Tender and Monitring
        public static string rptSoldTenderList { get { return ReportPath + "TM_SoldTenderList"; } }
        #endregion



        #region "Water Losses"
        public static string rptDivisionLG { get { return ReportPath + "WL_DivisionLG"; } }
        #endregion

        #region "Seasonal Planning"

        #region Rabi

        public static string rptAnticipatedWaterAvailabilityBasinRabi { get { return ReportPath + "SP_AnticipatedWaterAvailabilityBasinRabi"; } } //2
        public static string rptAnticipatedWaterAvailabilityRiverRabi { get { return ReportPath + "SP_AnticipatedWaterAvailabilityRiverRabi"; } } //2

        public static string rptWaterAvailbilityJCRabi { get { return ReportPath + "SP_WaterAvailbilityJCRabi"; } } //2

        public static string rptDistributionRABI_MinMax { get { return ReportPath + "SP_DistributionRABI"; } } //2
        public static string rptInflowForecastRabi { get { return ReportPath + "SP_InflowForecastRabi"; } }//1
        public static string rptReservoirOperationsRabiKharifTarbelaMangla { get { return ReportPath + "SP_ReservoirOperations"; } }//4
        public static string rptDamRuleCurveRabiKharifTarbelaMangla { get { return ReportPath + "SP_DamRuleCurve"; } }//4
        public static string rptProvincialSharesRABI_MinMaxMostLikely { get { return ReportPath + "SP_ProvincialSharesRABI"; } }//3

        public static string rptSystemOperationOfJCRabi { get { return ReportPath + "SP_SystemOperationOfJCRabi"; } }//1
        public static string rptSystemOperationOfIndusRabi { get { return ReportPath + "SP_SystemOperationOfIndusRabi"; } }//1

        public static string rptProbabilityFlowsRabi { get { return ReportPath + "SP_ProbabilityFlows"; } }//1

        #endregion
        #region Kharif

        public static string rptAnticipatedWaterAvailabilityBasinKharif { get { return ReportPath + "SP_AnticipatedWaterAvailabilityBasinKharif"; } } //2
        public static string rptAnticipatedWaterAvailabilityRiverKharif { get { return ReportPath + "SP_AnticipatedWaterAvailabilityRiverKharif"; } } //2
        public static string rptWaterAvailbilityJCKharif { get { return ReportPath + "SP_WaterAvailbilityJCKharif"; } } //2


        public static string rptDistributionKharif_MinMax { get { return ReportPath + "SP_DistributionKharif"; } } //2
        public static string rptInflowForecastKharif { get { return ReportPath + "SP_InflowForecastKharif"; } }//1

        public static string rptProvincialSharesKharif_MinMaxMostLikely { get { return ReportPath + "SP_ProvincialSharesKHARIF"; } }//3
        public static string rptSystemOperationOfIndusKharif { get { return ReportPath + "SP_SystemOperationOfIndusKharif"; } }//1
        public static string rptSystemOperationOfJCKharif { get { return ReportPath + "SP_SystemOperationOfJCKharif"; } }//1

        public static string rptProbabilityFlowsKharif { get { return ReportPath + "SP_ProbabilityFlows"; } }//1

        #endregion

        #region RiverFlow
        public static string rptSP_RiverFlowsAtRIMStationRabi { get { return ReportPath + "SP_RiverFlowsAtRIMStationRabi"; } } //2
        public static string rptSP_RiverFlowsAtRIMStationKharif { get { return ReportPath + "SP_RiverFlowsAtRIMStationKharif"; } } //2
        #endregion
        #endregion

        #region Closure Work Plan
        public static string rptPrintClosureWorkPlan { get { return ReportPath + "CW_PrintClosureWorkPlan"; } }

        public static string rptClosureWorksProgress { get { return ReportPath + "CW_ClosureWorksProgress"; } }
        #endregion

        #region "Irrigation Feed Back Reports"
        public static string rptIrrigatorFeedback { get { return ReportPath + "IF_IrrigatorFeedback"; } }
        #endregion

        #region "Seasonal Planning Reports"
        #endregion

        #region "Dashboard detailed reports"
        public static string rptDBTailStatusField { get { return ReportPath + "DB_TailStatusField"; } }
        public static string rptDBTailStatusPMIU { get { return ReportPath + "DB_TailStatusPMIU"; } }
        public static string rptDBComplaintAnalysis { get { return ReportPath + "DB_ComplaintAnalysis"; } }
        public static string rptDBWaterTheftCases { get { return ReportPath + "DB_WaterTheftCases"; } }
        #endregion

        #region EffluentReport
        public static string rptECIndustries { get { return ReportPath + "EC_Industries"; } }
        public static string rptECRecoveryOfAccount { get { return ReportPath + "EC_RecoveryOfAccount"; } }

        public static string rptECPaymentsReport { get { return ReportPath + "EC_Payments"; } }
        #endregion

        #region AccountReport
        public static string rptACBudgetUtilization { get { return ReportPath + "AT_BudgetUtlitization"; } }
        public static string rptACTaxSheet { get { return ReportPath + "AT_TaxSheet"; } }
        public static string rptACHeadWiseExpenditure { get { return ReportPath + "AT_HeadWiseExpenditure"; } }
        public static string rptACBudgetUtilizationDetails { get { return ReportPath + "AT_HeadWiseExpenditureDetail"; } }


        #endregion

        #region DesiltingProgress
        public static string rptCWDesiltingProgress { get { return ReportPath + "CW_DesiltingProgress"; } }

        #endregion

        #region AssetsReport
        public static string rptAMAssetWorkProgress { get { return ReportPath + "AM_AssetWorkProgress"; } }

        public static string rptAMAssetInspectionIndividual { get { return ReportPath + "AM_AssetInspectionIndividual"; } }

        public static string rptAMAssetInspectionLot { get { return ReportPath + "AM_AssetInspectionLot"; } }
        public static string rptAMAssetAttributesDetail { get { return ReportPath + "AM_AssetAttributesDetail"; } }
        public static string rptAMAssetWorksDetail { get { return ReportPath + "AM_AssetWorksDetail"; } }
        public static string rptACPrintSanction4Wheel { get { return ReportPath + "AT_PrintSanctions"; } }
        public static string rptACPrintSanction2Wheel { get { return ReportPath + "AT_PrintSanctions2Wheel"; } }
        #endregion

        #region EntitlementDeliveries
        public static string rptEDPlannedVsActualDeliveries { get { return ReportPath + "ED_PlannedVsActualDeliveries"; } }
        //TentativeDistribution
        public static string rptEDTentativeDisProIndusZoneRabi { get { return ReportPath + "ED_TentativeDisProIndusZoneRabi"; } }
        public static string rptEDTentativeDisProJhelumChenabKharif { get { return ReportPath + "ED_TentativeDisProJhelumChenabKharif"; } }
        public static string rptEDTentativeDisProJhelumChenabRabi { get { return ReportPath + "ED_TentativeDisProJhelumChenabRabi"; } }
        public static string rptEDTentativeDisProIndusZoneKharif { get { return ReportPath + "ED_TentativeDisProIndusZoneKharif"; } }
        //ED_TentativeEntitlement
        public static string rptEDTentativeEntitlementProIndusZoneRabi { get { return ReportPath + "ED_TentativeEntitlementProIndusZoneRabi"; } }
        public static string rptEDTentativeEntitlementProJhelumChenabKharif { get { return ReportPath + "ED_TentativeEntitlementProJhelumChenabKharif"; } }
        public static string rptEDTentativeEntitlementProJhelumChenabRabi { get { return ReportPath + "ED_TentativeEntitlementProJhelumChenabRabi"; } }
        public static string rptEDTentativeEntitlementProIndusZoneKharif { get { return ReportPath + "ED_TentativeEntitlementProIndusZoneKharif"; } }

        //Punjab Canal Withdrawals
        public static string rptEDPunjabCanalWithdrawalsIndusJC { get { return ReportPath + "ED_PunjabCanalWithdrawalsIndus-JC"; } }

        public static string rptEDTentativePunjabCanalsWithdrawalsJC { get { return ReportPath + "ED_TentativePunjabCanalsWithdrawalsJC"; } }
        #endregion

        #region Rotational Violation
        public static string rptRotationalViolation { get { return ReportPath + "RP_RotationalVoilation"; } }

        #endregion

        public enum DailyOperationalDataReports
        {
            ChannelDischargeHistory = 1,
            ClosedChannels = 2,
            TailStatusFieldStaffData = 3,
            TailStatusPMIUData = 4,
            GaugeReadingComparison = 5,
            ChangesInGaugeReadings = 6,
            GaugeStatusFieldStaffData = 7,
            GaugeStatusComparison = 8,
            Indents = 9,
        }
        public enum DailyDataAnalysisReports
        {
            DailyAnalysis = 1,
            DailyChannelsStatus = 2,
            ComparisonOfShortDryTails = 3
        }
        public enum ScheduleInspectionReports
        {
            GaugeInspection = 1,
            OutletPerformance = 2,
            DischargeTableCalculations = 3,
            InspectionOfOutletAlteration = 4,
            Tenders = 5,
            Works = 6
        }
        public enum SmartMonitoring
        {
            MeterReading=1,
            Fuel=2,
            GaugeObservation=3,
            OutletChecking=4,
            CutBreach=5,
            RotationalViolation=6,
            Leaves=7,
            WaterTheftOutlet=8,
            WaterTheftChannel=9,
            All=10

        }
        public enum WaterTheftReports
        {
            WaterTheftChannels = 1,
            WaterTheftOutlets = 2,
            Breach = 3
        }
        public enum ComplaintReports
        {
            ComplaintSource = 1,
            ComplaintType = 2,
            ComplaintStatus = 3,
            Directives = 4,
            Structure = 5,
            ComplaintDetails = 6
        }
        public enum FloodOperationReports
        {
            StoneInformation = 1,
            MachineryEquipment = 2,
            EmergencyPurchasesofItems = 3,
            EmergencyWorks = 4,
            OnsiteStoneMonitoring = 5,
            OnsiteCampSiteMonitoring = 6,
            FloodInspections = 7,
        }
        public enum Dashboard
        {
            ZoneWise = 1,
            CircleWise = 2,
            DivisionWise = 3,
            SubDivisionWise = 4,
        }
        public enum AssestsAndWorksReports
        {
            WorkProgress = 1,
            AssestsInspections = 2,
            AssestsDetails = 3,
        }


        #region "Daily Water Position"
        public static string rptDailyWaterAvailibityPosition { get { return ReportPath + "DailyWaterAvailibityPosition"; } }
        public static string rptCanalsWithDrwals { get { return ReportPath + "DD_CanalsWithDrwals"; } }
        public static string rptDailyGauge { get { return ReportPath + "CO_DailyGauge"; } }
        public static string rptCanalWithdrwals_ManglaAndTarbela { get { return ReportPath + "CO_CanalWithdrwal"; } }
        public static string rptSupplyPosition { get { return ReportPath + "SupplyPosition"; } }
        public static string rptCanalWireIndus { get { return ReportPath + "ED_CanalWireIndus"; } }
        public static string rptCanalWireJC { get { return ReportPath + "ED_CanalWireJC"; } }                
        #endregion





    }
}
