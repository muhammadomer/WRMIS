using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Common
{
    public class Constants
    {
        public const long UserID = 20005;
        public const long PunjabProvinceID = 1;
        public const long IrrigationDomainID = 1;
        public const long AdministratorRoleID = 1;
        public const bool GaugeCorrectionScouredType = true;
        public const bool GaugeCorrectionSiltedType = false;
        public const string UpStream = "Upstream";
        public const string DownStream = "Downstream";
        public const string Tick = "&#10003";
        public const string Dash = "-";
        public const Decimal TDailyFactor = 1.1M;
        public const Decimal TDailyLeapYearFalse = 0.8M;
        public const Decimal TDailyLeapYearTrue = 0.9M;
        public const Decimal MAFFactor = 0.01983471M;
        public const double CusecsConvertion = 1000;
        public const string NormalComplexityLevelName = "Normal";
        public const string StatisticalDraft = "Statistical";
        public const string SRMDraft = "SRM";
        public const double ElevationCapInitialStoragesMangla = 0.79;
        public const double ElevationCapInitialStoragesTarbela = 1.38;
        public const double JCStorageDepretion = 12.98;
        public const double ICStorageDepretion = 13.17;
        public const double ElevationCapInitialStoragesManglaRabi = 7.406;
        public const double ElevationCapInitialStoragesTarbelaRabi = 6.434;
        public const double JCStorageDepretionRabi = 90;
        public const double ICStorageDepretionRabi = 90;
        public const double MAFConversion = 0.01983471;
        public const double TDailyConversion = 1.1;
        public const double LeapYearFalse = 0.8;
        public const double LeapYearTrue = 0.9;
        public const short Oct1TDailyID = 19;
        public const short Dec3TDailyID = 27;
        public const short Jan1TDailyID = 28;
        public const short Mar3TDailyID = 36;
        public const short Apr1TDailyID = 1;
        public const short Jun1TDailyID = 7;
        public const short Jun2TDailyID = 8;
        public const short Sep3TDailyID = 18;

        public const string Prepared = "Prepared";
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string Rework = "Rework";
        public const string Group = "G";
        public const string SubGroup = "SG";

        public const string RP_Draft = "Draft";
        public const string RP_SendToSE = "Send To SE";
        public const string RP_Approved = "Approved";

        public const int KharifEntitlementStartDay = 11;
        public const int KharifEntitlementStartMonth = 3;
        public const int KharifEntitlementEndDay = 10;
        public const int KharifEntitlementEndMonth = 9;

        public const string WaterTheft = "Water Theft";
        public const string ChannelObservation = "Channel Observation";
        public const string OutletChecking = "Outlet Checking";
        public const string CutBreach = "Cut & Breach";
        public const string RotationalViolation = "Rotational Violation";
        public const string Leaves = "Leaves";
        public const string ET_All = "All";
        public const string Fuel = "Fuel";
        public const string UFuel = "FUEL";
        public const string Meter = "Meter";
        public const string UMeter = "METER";

        // TODO: Move below reports path to ReportConstants file
        #region Reports
        public static string ACCPReport { get { return ReportPath + "CW_AnnualCanalClosureProgramme"; } }
        public static string ReportPath { get { return "/PMIU.WRMIS/" + Common.Utility.ReadConfiguration("ENV") + "/Reports/"; } }
        public static string ClosureWorkPlanReport { get { return ReportPath + "CW_PrintClosureWorkPlan"; } }
        public static string WaterTheftChannelCase { get { return ReportPath + "WT_CaseInformationChannel"; } }
        public static string WaterTheftOutletCase { get { return ReportPath + "WT_CaseInformationOutlet"; } }
        public static string DailyGaugeData { get { return ReportPath + "DD_DailyGaugeData"; } }
        public static string DailyGaugeSlip { get { return ReportPath + "DD_DailyGaugeSlip"; } }
        public static string ComplaintInformation { get { return ReportPath + "CM_ComplaintInformation"; } }
        public static string IrrigatorFeedBack { get { return ReportPath + "IF_IrrigatorFeedback"; } }
        public static string rptChannelDischargeHistory { get { return ReportPath + "CO_ChannelDischargeHistory"; } }
        public static string PerformanceEvaluationReport { get { return ReportPath + "PE_Evaluation"; } }
        public static string EntitlementReport { get { return ReportPath + "ED_EntitlementData"; } }
        public static string DischargeTableCrestReport { get { return ReportPath + "CO_DischargeTableCrestLevel"; } }
        public static string DischargeTableBedReport { get { return ReportPath + "CO_DischargeTableBedLevel"; } }
        public static string DivisionSummaryReport { get { return ReportPath + "FO_DivisionSummary_Printable"; } }
        public static string FloodFightingPlanReport { get { return ReportPath + "FO_FloodFightingPlanPrintable"; } }
        public static string IndependentInspectionReport { get { return ReportPath + "FO_IndependentProtectionInfrastructureInspectionPrintable"; } }
        public static string RotationalProgramReport { get { return ReportPath + "RP_ShowRotationalProgramme"; } }
        public static string RotationalProgramReportforCircle { get { return ReportPath + "RP_ShowDivisionalRotationalProgramme"; } }
        public static string EffluentWaterBill { get { return ReportPath + "EC_EffluentWaterBill"; } }
        public static string CanalWaterBill { get { return ReportPath + "EC_SpecialCanalWaterBill"; } }

        public const string ReportsUrl = "../../Modules/Reports/ViewReport.aspx";
        public const string ReportsNestedUrl = "../../../Modules/Reports/ViewReport.aspx";

        public static string SDDamReading { get { return ReportPath + "SD_DamReading"; } }
        public static string SDChannelReading { get { return ReportPath + "SD_ChannelReading"; } }

        public static string AcquaintanceRollReport { get { return ReportPath + "AT_AcquaintanceRoll"; } }

        public static string TaxSheetReport { get { return ReportPath + "AT_TaxSheetBySanctionID"; } }

        #endregion

        public class Auditing
        {
            public const string CreatedDate = "CreatedDate";
            public const string CreatedBy = "CreatedBy";
            public const string ModifiedDate = "ModifiedDate";
            public const string ModifiedBy = "ModifiedBy";
            public const string IsActive = "IsActive";
        }
        public enum MessageCategory
        {
            WebApp,
            Business,
            DataAccess,
            WebServices,
            Apps
        }

        public enum MessageType
        {
            Info,
            Warning,
            Error
        }
        public enum ChannelRelation
        {
            P, // Parent Channel
            F // Feeder
        }

        public enum StructureType
        {
            Barrage = 1,
            Headwork = 2,
            Dam = 3,
            HeadRegulator = 4,
            CrossRegulator = 5,
            Channel = 6,
            Drain = 17,
            River = 18

        }
        public enum InspectionType
        {
            GaugeInsepction = 1,
            DischargeTableCalculationBedLevel = 2,
            DischargeTableCalculationCrestLevel = 3,
            OutletPerformance = 4,
            OutletChecking = 9,
            OutletAlteration = 5,
            TenderMonitoring = 6,
            WorksInpections = 7,
            GeneralInspections = 8
        }

        public enum ChannelSide
        {
            Left,
            Right,
            LeftRight
        }

        public enum ChannelType
        {
            MainCanal = 1,
            LinkCanal = 2,
            BranchCanal = 3,
            DistributaryMajor = 4,
            DistributaryMinor = 5,
            DistributarySubMinor = 6,
            EscapeChannel = 7
        }
        public enum TheftSiteCondition
        {
            FreshlyClosedCut = 1,
            RunningCut = 2,
            Tempered = 3,
            Repaired = 4,
            Defective = 5,
            OK = 6,
            PreviouslyClosedCut = 7,
            OverSize = 8
        }

        public enum GaugeCategory
        {
            HeadGauge = 1,
            TailGauge = 2,
            DivisionalGauge = 3,
            SubDivisionalGauge = 4,
            SectionalGauge = 5,
            CriticalGauge = 6
        }

        public enum GaugeLevel
        {
            BedLevel = 1,
            CrestLevel
        }

        public enum UserStatus
        {
            Active = 1,
            Inactive = 2
        }

        public enum AlertTypes
        {
            Notification = 1,
            SMS = 2,
            Email = 3
        }

        public enum IrrigationLevelID
        {
            Zone = 1,
            Circle = 2,
            Division = 3,
            SubDivision = 4,
            Section = 5,
            Office = 6
        }

        public enum Designation
        {
            Secretary = 1,
            ChiefIrrigation = 2,
            SE = 3,
            XEN = 4,
            SDO = 5,
            SBE = 6,
            Ziladaar = 7,
            GaugeReader = 8,
            ChiefMonitoring = 9,
            DirectorGauges = 10,
            DeputyDirector = 11,
            ADM = 12,
            MA = 13,
            DataAnalyst = 14,
            DeputyDirectorHelpline = 15,
            HelplineOperator = 16,
            AdditionalSecretaryAdmin = 17,
            Chiefpmo = 18,
            DataEntryOperator = 19,
            DF = 29,
            AccountOfficer = 31,
            CE = 21
        }

        public enum River
        {
            Chenab = 1,
            Indus = 2,
            Jhelum = 3,
            Ravi = 4,
            Sutlej = 5,
            Ghagra = 6
        }

        public enum DropDownFirstOption
        {
            NoOption = 1,
            Select = 2,
            All = 3
        }

        public enum OutletConditionOnOutletChecking
        {
            WaterTheft = 1,
            Ok = 2
        }

        public enum SessionOrShift
        {
            Morning = 1,
            Evening = 2
        }

        public enum EnableGaugeDischarge
        {
            B, // Both Enabled
            G, // Gauge Enabled
            D  // Discharge Enabled
        }

        public enum WTOffenceSite
        {
            Channel,
            Outlet
        }

        public enum WTAreaType
        {
            Acre = 1,
            Kanal = 2
        }

        public enum WTOffenceType
        {
            CrestTempered = 1,
            Gurlu = 2,
            RoofBlockTempered = 3,
            SocketRemoved = 4,
            MachineRemoved = 5,
            Daff = 6,
            Pipe = 7,
            Takki = 8
        }

        public enum WTChannelSides
        {
            Left,
            Right,
            TailLeft,
            TailRight,
            TailFront

        }

        public enum WTCaseStatus
        {
            Closed = 1,
            InProgress = 2,
            NA = 3,
            Appealed = 4,
            ReAppealed = 5
        }

        public enum WTBreach
        {
            Breach
        }

        public enum ChannelorOutlet
        {
            C,
            O
        }

        public enum SIScheduleStatus
        {
            Prepared = 1,
            PendingForApproval = 2,
            Approved = 3,
            Rejected = 4
        }

        public enum SIInspectionType
        {
            GaugeReading = 1,
            DischargeTableCalculation = 2,
            OutletPerformance = 3,
            OutletAlteration = 4,
            OutletChecking = 5
        }

        public enum AlertStatus
        {

            Inbox = 0,
            Unread = 1,
            Read = 2
        }

        public enum Seasons
        {
            Rabi = 1,
            Kharif = 2,
            EarlyKharif = 3,
            LateKharif = 4
        }

        public enum SeasonDistribution
        {
            KharifStart = 1,
            KharifEnd = 18,
            EKStart = 1,
            EKEnd = 7,
            LKStart = 8,
            LKEnd = 18,
            RabiStart = 19,
            RabiEnd = 36
        }

        public enum TDAilySpecialCases
        {
            MaySPecialTDaily = 6,
            JulyTDaily = 12,
            AugTDaily = 15,
            OctTDaily = 21,
            DecTDaily = 27,
            JanTDaily = 30,
            FebTDaily = 33,
            MarTDaily = 36
        }

        public enum PlanningMonthsAndDays
        {
            KharifPlanningMarch = 3,
            KharifPlanningApril = 4,
            PlanningDay = 11,
            RabiPlanningSeptember = 9,
            RabiPlanningOctober = 10
        }

        public enum RimStationsIDs
        {
            JhelumATMangla = 18,
            IndusAtTarbela = 20,
            ChenabAtMarala = 5,
            KabulAtNowshera = 24,
            Chashma = 2
        }

        public enum InflowForecstDrafts
        {
            StatisticalDraft = 1,
            SRMDraft = 2,
            SelectedDraft = 3
        }

        public enum ModuleName
        {
            UserAdministration = 1,
            IrrigationNetwork,
            DailyOperationalData,
            WaterTheft = 4,
            ScheduleInspections = 5,
            MiscellaneousTasksDomainSmallDams,
            IrrigatorFeedback = 7,
            ComplaintsCMDirectives = 8,
            TendersMonitoring,
            FloodOperations,
            DesiltingClosureWorks,
            PublicWebsite,
            RevenueEffluentwater,
            PerformanceEvaluation,
            AssetsManagement,
            Auction,
            Reports,
            Accounts,
            SpecialMonitoring,
            SeasonalPlanning,
            WaterLosses,
            FloodEarlyWarningSystem,
            EntitlementsDeliveries,
            RotationalPrograms,
            WaterCharges,
            ClosureOperations = 11
        }

        public enum SystemParameter
        {
            IndentPercentage = 1,
            AlertTimeInterval = 2,
            ReportFooter = 3
        }

        public enum ModeValue
        {
            Edit = 0,
            View = 1,
            RemoveValidation = 2,
            Thumbnail = 3
        }

        public enum ParentType
        {
            Barrage = 1,
            Headwork = 2,
            Channel = 6,
            River = 18

        }

        public enum ShareDistribution
        {
            SulemankiID = 12,
            BSOne = 2213,
            BSTwo = 2154,
            BallokiID = 11,
            UCC = 1060,
            MR = 1022,
            QB = 1623
        }

        public enum Organization
        {
            PMIU = 1,
            Secretariat = 2,
            Irrigation = 3,
            PIDA = 4,
            DandF = 5,
            PandR = 6,
            Development = 7,
            LBDC = 8,
            HydroPower = 9
        }

        public enum ReservoirLevels
        {
            Mangla = 1242,
            Tarbela = 1550,
            TarbelaFillingLimit = 1510,
            Chashma = 645,
            ChashmaMinOptLevel = 642
        }

        //public enum ComplaintType
        //{
        //    WaterTheft = 2,
        //    DryTail = 11,
        //    ShortTail = 12,
        //    RotationalViolation = 13,
        //    HeadGaugeNotFixed = 14,
        //    HeadGaugeNotPainted = 15,
        //    TailGaugeNotFixed = 16,
        //    TailGaugeNotPainted = 17
        //}
        public enum ComplaintSource
        {
            DefaultHelpline = 1,
            ChiefMinister = 2,
            ChiefSecretary = 3,
            SecretaryIrrigation = 4,
            TehsilProgramme = 5,
            Automatic = 6,
            MinisterIrrigation = 7

        }

        public enum StorageToFill
        {
            JhelumEK = 60,
            JhelumLK = 40,
            IndusEK = 58,
            IndusLK = 42
        }

        public enum LossGain
        {
            JhelumEK = -5,
            JhelumLK = -5,
            JhelumRabi = 5,
            IndusEK = -15,
            IndusLK = -25,
            IndusRabi = 10
        }

        public enum ComplaintModuleReference
        {
            WT_C,
            WT_O,
            IF_ST,
            IF_DT,
            IF_WT,
            IF_RV,
            SI_ST,
            SI_DT,
            SI_HGNF,
            SI_HGNP,
            SI_TGNF,
            SI_TGNP
        }

        public enum WorkType
        {
            CLOSURE,
            ASSET
        }

        public enum WorkStatus
        {
            NotSold = 1,
            Sold = 2,
            Closed = 3,
            Cancelled = 4,
            Awarded = 5
        }

        public enum WaterDistributionPercentiles
        {
            ZeroPercent = 0,
            FivePercent = 5,
            TenPercent = 10,
            FifteenPercent = 15,
            TwentyPercent = 20,
            TwentyFivePercent = 25,
            ThirtyPercent = 30
        }

        public enum ComplaintType
        {
            WaterTheft = 1,
            RotationalProgram = 2,
            ShortTail = 3,
            DryTail = 4,
            HeadGaugeNotFixed = 5,
            HeadGaugeNotPainted = 6,
            TailGaugeNotFixed = 7,
            TailGaugeNotPainted = 8

        }
        public enum RDWiseType
        {
            StonePitching = 1,
            ErodingAnimalHoles = 2,
            PitchStoneAppron = 3,
            RainCuts = 4,
            RDMarks = 5,
            Enchrochment = 6

        }

        public enum GaugeCorrectionType
        {
            BedSilted = 1,
            BedSourced = 2
        }

        public enum TailStatusByIrrigator
        {
            Dry = 1,
            ShortTail = 2,
            AuthorizedTail = 3,
            ExcessiveTail = 4
        }

        public enum YesNo
        {
            Yes = 1,
            No = 2
        }

        public enum ReportSpan
        {
            W, // Weekly
            F, // Fornightly
            M, // Monthly
            S // Seasonal
        }

        public enum ReservoirLevelsLimits
        {
            JhelumMin = 1040,
            JhelumMax = 1242,
            IndusMin = 1380,
            IndusMax = 1550,
            ChashmaMin = 638,
            ChashmaMax = 649
        }

        public enum Commands
        {
            IndusCommand = 1,
            JhelumChenabCommand = 2
        }

        #region ClosureOperations
        public const int CRUD_CREATE = 1, CRUD_READ = 2, CRUD_UPDATE = 3, CRUD_DELETE = 4, CHECK_DUPLICATION = 5, CHECK_ASSOCIATION = 6;
        #endregion


        public enum CWWorkType
        {
            Desilting = 1,
            ElectricalMechanical = 2,
            BuildingWorks = 3,
            OilingGreasingPainting = 4,
            OutletRepairing = 5,
            ChannelStructureWork = 6,
            OtherWork = 7
        }
        public enum DivisionStoreEntryType
        {
            //Received
            PurchaseFloodFightingPlan = 1,
            ReturnsPurchasedduringFlood = 2,
            ReturnsInfrastructures = 3,
            ReturnsCampSites = 4,
            //Issued
            IssuedInfrastructure = 5,
            IssuedCampSites = 6,
            //Updated
            Damaged = 7,
            Stolen = 8,
            Missing = 9,
            Nonusable = 10,
            Reversal = 11
        }

        public enum NoOfGroups
        {

            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        public enum NoOfSubGroups
        {
            zero = 0,
            One = 1,
            Two = 2,
            Three = 3
        }

        #region AssetsWorks
        public class AssetCategory
        {
            public const string Infrastructure = "Infrastructure";
            public const string MachineryEquipment = "Machinery/Equipment";
        }
        public class AssetAttribute
        {
            public const string Channel = "Channels";
            public const string Outlet = "Outlets";
            public const string BarrageHeadwork = "Barrages/Headworks";
            public const string ProtectionInfrastructure = "Protection Infrastructures";
            public const string Drain = "Drains";
            public const string SmallDams = "Small Dams";
            public const string SmallDamsChannels = "Small Dams Channels";
        }
        public class AssetsLevel
        {
            public const string Zonal = "Zone";
            public const string Circle = "Circle";
            public const string Divisional = "Division";
            public const string Office = "Office";
        }

        public const int AssetCRUD_CREATE = 1, AssetCRUD_READ = 2, AssetCRUD_UPDATE = 3, AssetCRUD_DELETE = 4, AssetCHECK_DUPLICATION = 5, AssetCHECK_ASSOCIATION = 6;

        #endregion

        #region Effluent and Water Charges
        public enum ECWServiceType { EFFLUENT = 1, CANAL = 2 }
        #endregion

        #region "Auctions"
        public enum ApprovalAuthorities
        {
            SE = 1,
            CE = 2

        }

        public enum AuctionTypes
        {
            OpenBidding = 1,
            SealedBidding = 2

        }

        public enum AuctionCategories
        {
            TemporaryORLease = 1,
            Permanent = 2

        }
        #endregion

        #region Accounts

        public enum ExpenseType
        {
            RepairMaintainance = 1,
            POLReceipts = 2,
            TADA = 3,
            NewPurchase = 4,
            OtherExpense = 5
        }

        public enum AssetType
        {
            FourWheelVehicle = 1,
            TwoWheelVehicle = 2,
            Other = 3
        }

        public enum AccountSetup
        {
            ExpenseLimitForTax = 1,
            ExpenseLimitForQuotations = 2,
            ExpenseLimitForTenders = 3,
            PerKMRateForTA = 4,
            AnnualSanctionPowerOfDDO = 5
        }

        public enum TaxRates
        {
            IncomeTaxFilerPurchase = 1,
            SalesTaxFilerPurchase = 2,
            PunjabSalesTaxFilerPurchase = 3,
            OtherTaxesFilerPurchase = 4,
            IncomeTaxNonFilerPurchase = 5,
            SalesTaxNonFilerPurchase = 6,
            PunjabSalesTaxNonFilerPurchase = 7,
            OtherTaxesNonFilerPurchase = 8,
            IncomeTaxNonFilerRepair = 9,
            SalesTaxNonFilerRepair = 10,
            PunjabSalesTaxNonFilerRepair = 11,
            OtherTaxesNonFilerRepair = 12,
            IncomeTaxFilerRepair = 13,
            SalesTaxFilerRepair = 14,
            PunjabSalesTaxFilerRepair = 15,
            OtherTaxesFilerRepair = 16,
        }

        public enum SanctionStatus
        {
            WaitingforApproval = 1,
            Sanctioned = 2,
            //Pending = 3,
            Rejected = 4,
            SenttoAGOffice = 5,
            //PassedbyAGOffice = 6,
            PaymentReleased = 7
        }
        public enum FundsReleaseType
        {
            FirstRelease = 1,
            SecondRelease = 2,
            ThirdRelease = 4,
            FourthRelease = 6,
            SupplementaryGrant = 7,
            Reappropriation = 8,
            TESTING = 9
        }
        #endregion

        #region tender

        public enum TenderSource
        {
            ASSETWORK,
            CLOSURE
        }
        #endregion

        #region EntitlementAndDeliveriesPrintable
        public static string EntitlementAndDeliveriesRabi { get { return ReportPath + "ED_EntitlementsAndDeliveriesRabiPrintable"; } }
        public static string EntitlementAndDeliveriesKharif { get { return ReportPath + "ED_EntitlementsAndDeliveriesKharifPrintable"; } }
        #endregion
    }
}
