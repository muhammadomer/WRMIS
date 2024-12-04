using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;



namespace PMIU.WRMIS.Web.Common
{
    public class BasePage : System.Web.UI.Page
    {
        //[DefaultValue(true)]
        public bool CanAdd { get; set; }
        //[DefaultValue(true)]
        public bool CanEdit { get; set; }
        //[DefaultValue(true)]
        public bool CanView { get; set; }
        //[DefaultValue(true)]
        public bool CanDelete { get; set; }
        //[DefaultValue(true)]
        public bool CanPrint { get; set; }
        //[DefaultValue(true)]
        public bool CanExport { get; set; }
        public long PageID { get; set; }

        protected Tuple<string, string, string> SetPageTitle(PageName pageName)
        {
            //PageTitle objPageTitle = new PageTitle();
            string moduleName = string.Empty;
            string pageTitle = string.Empty;
            string pageNav = string.Empty;

            switch (pageName)
            {
                case PageName.FloodEarlyWarningSystem:
                    moduleName = PageTitles.FloodEarlyWarningSystem;
                    pageTitle = PageTitles.FloodEarlyWarningSystemTitle;
                    pageNav = PageTitles.FloodEarlyWarningSystemNav;
                    break;

                case PageName.FloodOperation:
                    moduleName = PageTitles.FloodOperation;
                    pageTitle = PageTitles.FloodOperationTitle;
                    pageNav = PageTitles.FloodOperationNav;
                    break;

                case PageName.ChannelAddition:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.ChannelAddTitle;
                    pageNav = PageTitles.ChannelAdditionNav;
                    break;

                case PageName.ChannelSearch:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.ChannelSearchTitle;
                    pageNav = PageTitles.ChannelSearchNav;
                    break;

                case PageName.Zone:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.ZoneTitle;
                    pageNav = PageTitles.ZoneNav;
                    break;

                case PageName.Outlets:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.OutletViewTitle;
                    pageNav = PageTitles.OutletViewNav;
                    break;

                case PageName.AddOutlet:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.OutletAddTitle;
                    pageNav = PageTitles.OutletAddNav;
                    break;

                case PageName.AlterationOutlet:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.OutletAltTitle;
                    pageNav = PageTitles.OutletAltNav;
                    break;

                case PageName.GaugeType:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.GaugeTypeTitle;
                    pageNav = PageTitles.GaugeTypeNav;
                    break;

                case PageName.Circle:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.CircleTitle;
                    pageNav = PageTitles.CircleNav;
                    break;

                case PageName.Division:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.DivisionTitle;
                    pageNav = PageTitles.DivisionNav;
                    break;

                case PageName.OutletType:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.OutletTypeTitle;
                    pageNav = PageTitles.OutletTypeNav;
                    break;

                case PageName.District:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.DistrictTitle;
                    pageNav = PageTitles.DistrictNav;
                    break;

                case PageName.SubDivision:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.SubDivisionTitle;
                    pageNav = PageTitles.SubDivisionNav;
                    break;

                case PageName.Tehsil:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.TehsilTitle;
                    pageNav = PageTitles.TehsilNav;
                    break;

                case PageName.PoliceStation:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.PoliceStationTitle;
                    pageNav = PageTitles.PoliceStationNav;
                    break;

                case PageName.ChannelPhysicalLocation:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.ChannelPhysicalLocationTitle;
                    pageNav = PageTitles.ChannelPhysicalLocationNav;
                    break;

                case PageName.Village:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.VillageTitle;
                    pageNav = PageTitles.VillageNav;
                    break;

                case PageName.Section:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.SectionTitle;
                    pageNav = PageTitles.SectionNav;
                    break;

                case PageName.Structure:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.StructureTitle;
                    pageNav = PageTitles.StructureNav;
                    break;

                case PageName.Province:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.ProvinceTitle;
                    pageNav = PageTitles.ProvinceNav;
                    break;

                case PageName.DivisionDistrict:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.DivisionDistrictTitle;
                    pageNav = PageTitles.DivisionDistrictNav;
                    break;

                case PageName.Roles:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.RolesTitle;
                    pageNav = PageTitles.RolesNav;
                    break;

                case PageName.Office:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.OfficeTitle;
                    pageNav = PageTitles.OfficeNav;
                    break;

                case PageName.BedLevelDTParameters:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.BedLevelDTParamTitle;
                    pageNav = PageTitles.BedLevelDTParamNav;
                    break;

                case PageName.RoleRights:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.RoleRightsTitle;
                    pageNav = PageTitles.RoleRightsNav;
                    break;

                case PageName.ViewBedLevelDTHistory:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.ViewBedLevelDTHistoryTitle;
                    pageNav = PageTitles.ViewBedLevelDTHistoryNav;
                    break;

                case PageName.Designation:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.DesignationTitle;
                    pageNav = PageTitles.DesignationNav;
                    break;

                case PageName.CrestLevelDTParameters:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.CrestLevelDTParamTitle;
                    pageNav = PageTitles.CrestLevelDTParamNav;
                    break;

                case PageName.ViewCrestLevelDTHistory:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.ViewCrestLevelDTHistoryTitle;
                    pageNav = PageTitles.ViewCrestLevelDTHistoryNav;
                    break;

                case PageName.SearchUser:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.SearchUserTitle;
                    pageNav = PageTitles.SearchUserNav;
                    break;

                case PageName.StructureData:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.StructureDataTitle;
                    pageNav = PageTitles.StructureDataNav;
                    break;

                case PageName.SearchPlacingIndents:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.SearchPlacingIndentsTitle;
                    pageNav = PageTitles.SearchPlacingIndentsNav;
                    break;

                case PageName.AddIndent:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.AddIndentTitle;
                    pageNav = PageTitles.AddIndentNav;
                    break;

                case PageName.IndentHistory:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.IndentHistoryTitle;
                    pageNav = PageTitles.IndentHistoryNav;
                    break;

                case PageName.GaugeInformation:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.GaugeInformationTitle;
                    pageNav = PageTitles.GaugeInformationNav;
                    break;

                case PageName.ChannelParentFeeder:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.ParentFeederChannelTitle;
                    pageNav = PageTitles.ParentFeederChannelNav;
                    break;

                case PageName.ViewScheduleInspection:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.ViewScheduleInspectionTitle;
                    pageNav = PageTitles.ViewScheduleInspectionNav;
                    break;

                case PageName.AddUser:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.AddUserTitle;
                    pageNav = PageTitles.AddUserNav;
                    break;

                case PageName.EditUser:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.EditUserTitle;
                    pageNav = PageTitles.EditUserNav;
                    break;

                case PageName.AssignRoleToUser:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.AssignRoleToUserTitle;
                    pageNav = PageTitles.AssignRoleToUserNav;
                    break;

                case PageName.ViewUser:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.ViewUserTitle;
                    pageNav = PageTitles.ViewUserNav;
                    break;

                case PageName.SearchTempAssignment:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.SearchTempAssignTitle;
                    pageNav = PageTitles.SearchTempAssignNav;
                    break;

                case PageName.LocationToUser:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.AssociateLocationToUserTitle;
                    pageNav = PageTitles.AssociateLocationToUserNav;
                    break;

                case PageName.NotificationsLimit:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.NotificationsLimitTitle;
                    pageNav = PageTitles.NotificationsLimitNav;
                    break;

                case PageName.TempAssignment:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.TempAssignTitle;
                    pageNav = PageTitles.TempAssignNav;
                    break;

                case PageName.AlertConfiguration:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.AlertConfigurationTitle;
                    pageNav = PageTitles.AlertConfigurationNav;
                    break;

                case PageName.AssociateBarrageChannelOutlets:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.AssociateBarrageChannelOutlet;
                    pageNav = PageTitles.AssociateBarrageChannelOutletNav;
                    break;

                case PageName.LSectionParameter:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.LSectionParameterTitle;
                    pageNav = PageTitles.LSectionParameterNav;
                    break;

                case PageName.OutletVillages:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.OutletVillageTitle;
                    pageNav = PageTitles.OutletVillageNav;
                    break;

                case PageName.OutletAlterationHistory:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.OutletAlterationHistoryTitle;
                    pageNav = PageTitles.OutletAlterationHistoryNav;
                    break;

                case PageName.DefineChannelReach:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.DefineChannelReachTitle;
                    pageNav = PageTitles.DefineChannelReachNav;
                    break;

                case PageName.LSectionParametersHistory:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.LSectionParametersHistoryTitle;
                    pageNav = PageTitles.LSectionParametersHistoryNav;
                    break;

                case PageName.ReasonForChange:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.ReasonForChangeTitle;
                    pageNav = PageTitles.ReasonForChangeNav;
                    break;

                case PageName.BarrageDataFrequency:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.BarrageDataFrequencyTitle;
                    pageNav = PageTitles.BarrageDataFrequencyNav;
                    break;

                case PageName.OperationalData:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.OperationalDataTitle;
                    pageNav = PageTitles.OperationalDataNav;
                    break;

                case PageName.OutletPerformanceData:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.OutletPerformanceDataTitle;
                    pageNav = PageTitles.OutletPerformanceDataNav;
                    break;

                case PageName.SearchVriteriaForOutletPerformance:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.SearchCriteriaOutletPerformanceTitle;
                    pageNav = PageTitles.SearchCriteriaOutletPerformanceNav;
                    break;

                case PageName.OutletPerformanceHistory:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.OutletPerformanceHistoryTitle;
                    pageNav = PageTitles.OutletPerformanceHistoryNav;
                    break;

                case PageName.CriteriaforSpecificOutlet:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.LocatespecificOutletTitle;
                    pageNav = PageTitles.LocatespecificOutletNav;
                    break;

                case PageName.DailyGaugeSlip:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.DailyGaugeSlipTitle;
                    pageNav = PageTitles.DailyGaugeSlipNav;
                    break;

                case PageName.DischargeDataOfBarrage:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.DischargeDataOfBarrageHeadworkTitle;
                    pageNav = PageTitles.DischargeDataOfBarrageHeadworkNav;
                    break;

                case PageName.AccessDenied:
                    moduleName = PageTitles.AccessDenied;
                    pageTitle = PageTitles.AccessDeniedTitle;
                    pageNav = PageTitles.AccessDeniedNav;
                    break;

                case PageName.UnknownError:
                    moduleName = PageTitles.UnknownError;
                    pageTitle = PageTitles.UnknownErrorTitle;
                    pageNav = PageTitles.UnknownErrorNav;
                    break;

                case PageName.AddWaterTheft:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.AddWaterTheftTitle;
                    pageNav = PageTitles.AddwaterTheftNav;
                    break;

                case PageName.DailyOperationalData:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.DailyOperationalDataTitle;
                    pageNav = PageTitles.DailyOperationalDataNav;
                    break;

                case PageName.SearchBreachIncident:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.SearchBreachIncidentTitle;
                    pageNav = PageTitles.SearchBreachIncidentNav;
                    break;

                case PageName.AddBreachIncident:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.AddBreachIncidentTitle;
                    pageNav = PageTitles.AddBreachIncidentNav;
                    break;

                case PageName.BreachCaseView:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.BreachCaseViewTitle;
                    pageNav = PageTitles.BreachCaseViewNav;
                    break;

                case PageName.SDOWorking:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.SDOWorkingTitle;
                    pageNav = PageTitles.SDOWorkingNav;
                    break;

                case PageName.SBEWorking:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.SBEWorkingTitle;
                    pageNav = PageTitles.SBEWorkingNav;
                    break;

                case PageName.XENWorking:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.XENWorkingTitle;
                    pageNav = PageTitles.XENWorkingNav;
                    break;

                case PageName.SEWorking:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.SEWorkingTitle;
                    pageNav = PageTitles.SEWorkingNav;
                    break;

                case PageName.ChiefWorking:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.ChiefWorkingTitle;
                    pageNav = PageTitles.ChiefWorkingNav;
                    break;

                case PageName.SearchWaterTheft:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.SearchWaterTheftTitle;
                    pageNav = PageTitles.SearchWaterTheftNav;
                    break;

                case PageName.ReferenceData:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.ReferenceDataTitle;
                    pageNav = PageTitles.ReferenceDataNav;
                    break;

                case PageName.AddEditIrrigator:
                    moduleName = PageTitles.IrrigatorFeedback;
                    pageTitle = PageTitles.AddEditIrrigatorTitle;
                    pageNav = PageTitles.AddEditIrrigatorNav;
                    break;

                case PageName.AddIrrigatorFeedback:
                    moduleName = PageTitles.IrrigatorFeedback;
                    pageTitle = PageTitles.AddIrrigatorFeedbackTitle;
                    pageNav = PageTitles.AddIrrigatorFeedbackNav;
                    break;

                case PageName.ViewIrrigatorFeedback:
                    moduleName = PageTitles.IrrigatorFeedback;
                    pageTitle = PageTitles.ViewIrrigatorFeedbackTitle;
                    pageNav = PageTitles.ViewIrrigatorFeedbackNav;
                    break;

                case PageName.IrrigatorFeedbackHistory:
                    moduleName = PageTitles.IrrigatorFeedback;
                    pageTitle = PageTitles.IrrigatorFeedbackHistoryTitle;
                    pageNav = PageTitles.IrrigatorFeedbackHistoryNav;
                    break;

                case PageName.SearchIrrigator:
                    moduleName = PageTitles.IrrigatorFeedback;
                    pageTitle = PageTitles.SearchIrrigatorTitle;
                    pageNav = PageTitles.SearchIrrigatorNav;
                    break;

                case PageName.SearchIrrigatorFeedback:
                    moduleName = PageTitles.IrrigatorFeedback;
                    pageTitle = PageTitles.SearchIrrigatorFeedbackTitle;
                    pageNav = PageTitles.SearchIrrigatorFeedbackNav;
                    break;

                case PageName.ViewOffenders:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.ViewOffendersTitle;
                    pageNav = PageTitles.ViewOffendersNav;
                    break;

                case PageName.Ziladaar:
                    moduleName = PageTitles.WaterTheft;
                    pageTitle = PageTitles.ZiladaarTitle;
                    pageNav = PageTitles.ZiladaarNav;
                    break;

                case PageName.ActionOnSchedule:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.ActionOnScheduleTitle;
                    pageNav = PageTitles.ActionOnScheduleNav;
                    break;

                case PageName.AddOutletAlterationInspectionNotes:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddOutletAlterationInspectionNotesTitle;
                    pageNav = PageTitles.AddOutletAlterationInspectionNotesNav;
                    break;

                case PageName.AddOutletPerformanceInspectionNotes:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddOutletPerformanceInspectionNotesTitle;
                    pageNav = PageTitles.AddOutletPerformanceInspectionNotesNav;
                    break;
                case PageName.UserNotificationPreferences:
                    moduleName = PageTitles.Notification;
                    pageTitle = PageTitles.UserNotificationPreferencesTitle;
                    pageNav = PageTitles.UserNotificationPreferencesNav;
                    break;

                case PageName.Para2:
                    moduleName = PageTitles.SeasonalPlanning;
                    pageTitle = PageTitles.Para2Title;
                    pageNav = PageTitles.Para2Nav;
                    break;

                case PageName.Flow7782:
                    moduleName = PageTitles.SeasonalPlanning;
                    pageTitle = PageTitles.Flow7782Title;
                    pageNav = PageTitles.Flow7782Nav;
                    break;

                case PageName.FillingFraction:
                    moduleName = PageTitles.SeasonalPlanning;
                    pageTitle = PageTitles.FillingFractionTitle;
                    pageNav = PageTitles.FillingFractionNav;
                    break;

                case PageName.ShareDistribution:
                    moduleName = PageTitles.SeasonalPlanning;
                    pageTitle = PageTitles.ShareDistributionTitle;
                    pageNav = PageTitles.ShareDistributionNav;
                    break;

                case PageName.WaterDistrubution:
                    moduleName = PageTitles.SeasonalPlanning;
                    pageTitle = PageTitles.WaterDistributionTitle;
                    pageNav = PageTitles.WaterDistributionNav;
                    break;

                case PageName.EasterComponent:
                    moduleName = PageTitles.SeasonalPlanning;
                    pageTitle = PageTitles.EasternComponentTitle;
                    pageNav = PageTitles.EasternComponentNav;
                    break;

                case PageName.Notifications:
                    moduleName = PageTitles.Notification;
                    pageTitle = PageTitles.NotificationsTitle;
                    pageNav = PageTitles.NotificationsNav;
                    break;

                case PageName.IndentsLagTime:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.IndentsLagTimeTitle;
                    pageNav = PageTitles.IndentsLagTimeNav;
                    break;

                case PageName.DailyIndents:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.DailyIndentsTitle;
                    pageNav = PageTitles.DailyIndentsNav;
                    break;

                case PageName.IndentsHistory:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.IndentsHistoryTitle;
                    pageNav = PageTitles.IndentsHistoryNav;
                    break;

                case PageName.PlacingIndents:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.PlacingIndentsTitle;
                    pageNav = PageTitles.PoliceStationNav;
                    break;

                case PageName.ViewIndents:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.ViewIndentsTitle;
                    pageNav = PageTitles.ViewIndentsNav;
                    break;

                case PageName.Reports:
                    moduleName = string.Empty;//PageTitles.Reports;
                    pageTitle = PageTitles.ReportsTitle;
                    pageNav = PageTitles.ReportsNav;
                    break;

                case PageName.PerformanceEvaluation:
                    moduleName = PageTitles.PerformanceEvaluation;
                    pageTitle = PageTitles.PerformanceEvaluation;
                    pageNav = PageTitles.PerformanceEvaluation;
                    break;

                case PageName.ComplaintsType:
                    moduleName = PageTitles.ComplaintsManagement;
                    pageTitle = PageTitles.ComplaintsTypeTitle;
                    pageNav = PageTitles.ComplaintsTypeNav;
                    break;

                case PageName.AddNewSchedule:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddNewScheduleTitle;
                    pageNav = PageTitles.AddNewScheduleNav;
                    break;

                case PageName.SearchSchedule:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.SearchScheduleTitle;
                    pageNav = PageTitles.SearchScheduleNav;
                    break;

                case PageName.AddGaugeInspection:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddGaugeInspectionTitle;
                    pageNav = PageTitles.AddGaugeInspectionNav;
                    break;

                case PageName.AddGeneralInspection:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddGeneralInspectionTitle;
                    pageNav = PageTitles.AddGeneralInspectionNav;
                    break;

                case PageName.AddInspectionNotes:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddInspectionNotesTitle;
                    pageNav = PageTitles.AddInspectionNotesNav;
                    break;

                case PageName.AddDischargeTableCalculationBL:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddDischargeTableCalculationBLTitle;
                    pageNav = PageTitles.AddDischargeTableCalculationBLNav;
                    break;

                case PageName.AddDischargeTableCalculationCL:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddDischargeTableCalculationCLTitle;
                    pageNav = PageTitles.AddDischargeTableCalculationCLNav;
                    break;

                case PageName.SearchComplaints:
                    moduleName = PageTitles.ComplaintsManagement;
                    pageTitle = PageTitles.SearchComplaintsTitle;
                    pageNav = PageTitles.SearchComplaintsNav;
                    break;

                case PageName.AdditionalAccessibility:
                    moduleName = PageTitles.ComplaintsManagement;
                    pageTitle = PageTitles.AdditionalAccessibilityTitle;
                    pageNav = PageTitles.AdditionalAccessibilityNav;
                    break;

                case PageName.LagTime:
                    moduleName = PageTitles.WaterLosses;
                    pageTitle = PageTitles.LagTimeTitle;
                    pageNav = PageTitles.LagTimeTitleNav;
                    break;

                case PageName.CommandWaterLosses:
                    moduleName = PageTitles.WaterLosses;
                    pageTitle = PageTitles.CommandWLTitle;
                    pageNav = PageTitles.CommanWLTitleNav;
                    break;

                case PageName.InflowForecasting:
                    moduleName = PageTitles.SeasonalPlanning;
                    pageTitle = PageTitles.InflowForecastingTitle;
                    pageNav = PageTitles.InflowForecastingNav;
                    break;

                case PageName.ChannelWaterLosses:
                    moduleName = PageTitles.WaterLosses;
                    pageTitle = PageTitles.ChannelWLTitle;
                    pageNav = PageTitles.ChannelWLTitleNav;
                    break;

                case PageName.SearchInspection:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.SearchInspectionTitle;
                    pageNav = PageTitles.SearchInspectionNav;
                    break;

                case PageName.SubDivisionWaterLosses:
                    moduleName = PageTitles.WaterLosses;
                    pageTitle = PageTitles.SubDivWLTitle;
                    pageNav = PageTitles.SubDivWLTitleNav;
                    break;

                case PageName.SISearchCriteriaForOutletAlteration:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.SISearchCriteriaForOutletAlteration;
                    pageNav = PageTitles.SISearchCriteriaForOutletAlterationNav;
                    break;

                case PageName.SICriteriaforSpecificOutletAlt:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.SICriteriaforSpecificOutletAlt;
                    pageNav = PageTitles.SICriteriaforSpecificOutletAltNav;
                    break;

                case PageName.OutletAlterationData:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.OutletAlterationData;
                    pageNav = PageTitles.OutletAlterationDatatNav;
                    break;

                case PageName.SISearchCriteriaForOutletPerformance:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.SISearchCriteriaForOutletPerformance;
                    pageNav = PageTitles.SISearchCriteriaForOutletPerformancetNav;
                    break;

                case PageName.SICriteriaforSpecificOutlet:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.SICriteriaForLocateSpecificOutlet;
                    pageNav = PageTitles.SICriteriaForLocateSpecificOutletNav;
                    break;

                case PageName.SIOutletPerformanceData:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.SIOutletPerformanceData;
                    pageNav = PageTitles.SIOutletPerformanceDataNav;
                    break;

                case PageName.SIOutletPerformanceHistory:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.SIOutletPerformanceHistory;
                    pageNav = PageTitles.SIOutletPerformanceHistoryNav;
                    break;

                case PageName.ScheduleCalendar:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.ScheduleCalendarTitle;
                    pageNav = PageTitles.ScheduleCalendarTitleNav;
                    break;

                case PageName.AddComplaint:
                    moduleName = PageTitles.ComplaintsManagement;
                    pageTitle = PageTitles.AddComplaintsTitle;
                    pageNav = PageTitles.AddComplaintsNav;
                    break;

                case PageName.DivisionalWaterLosses:
                    moduleName = PageTitles.WaterLosses;
                    pageTitle = PageTitles.DivWLTitle;
                    pageNav = PageTitles.DivWLTitleNav;
                    break;

                case PageName.TechnicalSanctionUnits:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.TechnicalSanctionUnitsTitle;
                    pageNav = PageTitles.TechnicalSanctionUnitsNav;
                    break;

                case PageName.ClosureWorkType:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.ClosureWorkTypeTitle;
                    pageNav = PageTitles.ClosureWorkTypeNav;
                    break;

                case PageName.DailyDataOperationalReport:
                    moduleName = PageTitles.Reports;
                    pageTitle = PageTitles.DailyOperationalReportsTitle;
                    pageNav = PageTitles.DailyOperationalReportsNav;
                    break;

                case PageName.Tenders:
                    moduleName = PageTitles.TendersMonitoring;
                    pageTitle = PageTitles.TendersMonitoring;
                    pageNav = PageTitles.TendersMonitoring;
                    break;

                case PageName.AnnualCanalClosureProgram:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.ACCPTitle;
                    pageNav = PageTitles.ACCPNav;
                    break;

                case PageName.AddAnnualCanalClosureProgram:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.AddACCPTitle;
                    pageNav = PageTitles.AddACCPNav;
                    break;

                case PageName.DetailsAnnualCanalClosureProgram:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.DetailACCPTitle;
                    pageNav = PageTitles.DetailACCPNav;
                    break;

                case PageName.AddClosureWork:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.AddCWTitle;
                    pageNav = PageTitles.AddCWPNav;
                    break;

                case PageName.ClosureWorkPlan:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.CWPTitle;
                    pageNav = PageTitles.CWPNav;
                    break;

                case PageName.ClosureWorkItem:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.CWItemTitle;
                    pageNav = PageTitles.CWItemNav;
                    break;

                case PageName.DailyAnalysisReport:
                    moduleName = PageTitles.Reports;
                    pageTitle = PageTitles.DailyAnalysisReportTitle;
                    pageNav = PageTitles.DailyAnalysisReportNav;
                    break;

                case PageName.ScheduleInspectionReport:
                    moduleName = PageTitles.Reports;
                    pageTitle = PageTitles.ScheduleInspectionReportTitle;
                    pageNav = PageTitles.ScheduleInspectionReportNav;
                    break;

                case PageName.ViewWorkTenderDetails:
                    moduleName = PageTitles.TendersMonitoring;
                    pageTitle = PageTitles.ViewWTDetailsTitle;
                    pageNav = PageTitles.ViewWTDetailsNav;
                    break;

                case PageName.AnnualCanalClosureWorkPlanOrders:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.ACCWPOrderItemTitle;
                    pageNav = PageTitles.ACCWPOrderItemNav;
                    break;

                case PageName.AddWorkProgress:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.AddWorkPrgrsTitle;
                    pageNav = PageTitles.AddWorkPrgrsNav;
                    break;

                case PageName.WorkProgressHistory:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.WorkPrgrsHistoryTitle;
                    pageNav = PageTitles.WorkPrgrsHistoryNav;
                    break;

                case PageName.ExcludeChanneFromACCP:
                    moduleName = PageTitles.ClosureOperations;
                    pageTitle = PageTitles.ExcludeChannelTitle;
                    pageNav = PageTitles.ExcludeChannelNav;
                    break;

                case PageName.SearchRotationalPlan:
                    moduleName = PageTitles.RotationalProgram;
                    pageTitle = PageTitles.SearchRotationalPlanTitle;
                    pageNav = PageTitles.SearchRotationalPlanNav;
                    break;

                case PageName.AddRotationalPlan:
                    moduleName = PageTitles.RotationalProgram;
                    pageTitle = PageTitles.AddRotationalPlanTitle;
                    pageNav = PageTitles.AddRotationalPlanNav;
                    break;

                case PageName.EntitlementDelivery:
                    moduleName = PageTitles.EntitlementDelivery;
                    pageTitle = PageTitles.EntitlementDelivery;
                    pageNav = PageTitles.EntitlementDelivery;
                    break;
                case PageName.EffluentandWaterCharges:
                    moduleName = PageTitles.EWC;
                    pageTitle = "";
                    pageNav = "";
                    break;
                //AssetWorksManagement
                case PageName.AssetsCategory:
                    moduleName = PageTitles.AssetsWorksManagement;
                    pageTitle = PageTitles.AssetCategoryTitle;
                    pageNav = PageTitles.AssetCategoryNav;
                    break;

                case PageName.Auctions:
                    moduleName = PageTitles.Auctions;
                    pageTitle = PageTitles.AddAuctionNoticeTitle;
                    pageNav = PageTitles.AddAuctionNoticeNav;
                    break;

                case PageName.Accounts:
                    moduleName = PageTitles.Accounts;
                    pageTitle = PageTitles.Accounts;
                    pageNav = PageTitles.Accounts;
                    break;


                case PageName.SmallDams:
                    moduleName = PageTitles.SmallDams;
                    pageTitle = PageTitles.SmallDamsTitle;
                    pageNav = PageTitles.SmallDamsNav;
                    break;


                case PageName.AssestsAndWorks:
                    moduleName = PageTitles.AssetsWorksManagement;
                    pageTitle = PageTitles.AssetWorkSearch;
                    pageNav = PageTitles.AddAssetWorkNav;
                    break;
                case PageName.AddAssetWork:
                    moduleName = PageTitles.AssetsWorksManagement;
                    pageTitle = PageTitles.AddAssetWork;
                    pageNav = PageTitles.AddAssetWorkNav;
                    break;
                case PageName.AddAssetWorkItem:
                    moduleName = PageTitles.AssetsWorksManagement;
                    pageTitle = PageTitles.AddAssetWorkItem;
                    pageNav = PageTitles.AddAssetWorkItemNav;
                    break;
                case PageName.AddAssetWorkProgress:
                    moduleName = PageTitles.AssetsWorksManagement;
                    pageTitle = PageTitles.AddAssetWorkProgress;
                    pageNav = PageTitles.AddAssetWorkProgressNav;
                    break;
                case PageName.AssetWorkProgressHistory:
                    moduleName = PageTitles.AssetsWorksManagement;
                    pageTitle = PageTitles.AssetWorkProgressHistory;
                    pageNav = PageTitles.AssetWorkProgressHistoryNav;
                    break;
                case PageName.IMISCode:
                    moduleName = PageTitles.IrrigationModule;
                    pageTitle = PageTitles.IMISCodeTitle;
                    pageNav = PageTitles.IMISCodeNav;
                    break;

                case PageName.UserManuals:
                    moduleName = PageTitles.UserAdministration;
                    pageTitle = PageTitles.UserManualTitle;
                    pageNav = PageTitles.UserManualNav;
                    break;
                case PageName.MeterReadingAndFule:
                    moduleName = PageTitles.DailyData;
                    pageTitle = PageTitles.MeterReadingAndFuleTitle;
                    pageNav = PageTitles.MeterReadingAndFuleTitleNav;
                    break;
                case PageName.OutletChecking:
                    moduleName = PageTitles.ScheduleInspection;
                    pageTitle = PageTitles.AddOutletChecking;
                    pageNav = PageTitles.OutletCheckingDataNav;
                    break;
                default:
                    break;
            }

            Tuple<string, string, string> objpageTitle = Tuple.Create(moduleName, pageTitle, pageNav);
            return objpageTitle;
        }
        public List<TSource> GetPageData<TSource>(List<TSource> lst, int _PageIndex, int _PageSize)
        {
            return lst.Skip((_PageIndex - 1) * _PageSize).Take(_PageSize).ToList<TSource>();
        }
        public UA_RoleRights GetPageRoleRights()
        {
            try
            {
                List<UA_RoleRights> lstRoleRight = Session[SessionValues.RoleRightList] as List<UA_RoleRights>;
                if (lstRoleRight != null)
                {
                    string PageUrl = HttpContext.Current.Request.Url.AbsolutePath;

                    UA_RoleRights mdlRoleRights = lstRoleRight.FirstOrDefault(rr => rr.UA_Pages.Description.ToUpper().Trim() == PageUrl.ToUpper().Trim());

                    return mdlRoleRights;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exp)
            {
                return null;
            }

        }
        protected override void OnLoad(EventArgs e)
        {
            UA_RoleRights bllRole = this.GetPageRoleRights();
            HasPermissions(bllRole);
            base.OnLoad(e);
        }
        private void HasPermissions(UA_RoleRights _RoleRights)
        {
            if (_RoleRights != null)
            {
                this.CanAdd = Convert.ToBoolean(_RoleRights.BAdd);
                this.CanEdit = Convert.ToBoolean(_RoleRights.BEdit);
                this.CanDelete = Convert.ToBoolean(_RoleRights.BDelete);
                this.CanView = Convert.ToBoolean(_RoleRights.BView);
                this.CanPrint = Convert.ToBoolean(_RoleRights.BPrint);
                this.CanExport = Convert.ToBoolean(_RoleRights.BExport);
                this.PageID = Convert.ToInt64(_RoleRights.PageID);
            }
        }
        public string GetDynamicPropertyValue(dynamic _Object, string _PropertyName)
        {
            return Convert.ToString(_Object.GetType().GetProperty(_PropertyName).GetValue(_Object, null));
        }
    }
    public enum PageName
    {
        FloodEarlyWarningSystem,
        FloodOperation,
        ChannelAddition,
        ChannelSearch,
        Zone,
        District,
        Outlets,
        AddOutlet,
        AlterationOutlet,
        GaugeType,
        Circle,
        Division,
        OutletType,
        SubDivision,
        Tehsil,
        PoliceStation,
        Village,
        Section,
        Structure,
        Province,
        DivisionDistrict,
        Roles,
        ChannelPhysicalLocation,
        Office,
        BedLevelDTParameters,
        RoleRights,
        ViewBedLevelDTHistory,
        Designation,
        CrestLevelDTParameters,
        SearchUser,
        ViewCrestLevelDTHistory,
        StructureData,
        SearchPlacingIndents,
        AddIndent,
        IndentHistory,
        GaugeInformation,
        ChannelParentFeeder,
        ViewScheduleInspection,
        AddUser,
        EditUser,
        AssignRoleToUser,
        ViewUser,
        LocationToUser,
        SearchTempAssignment,
        NotificationsLimit,
        TempAssignment,
        AlertConfiguration,
        AssociateBarrageChannelOutlets,
        LSectionParameter,
        OutletVillages,
        OutletAlterationHistory,
        DefineChannelReach,
        LSectionParametersHistory,
        ReasonForChange,
        BarrageDataFrequency,
        OutletPerformanceData,
        OutletChecking,
        SIOutletPerformanceData,
        OutletAlterationData,
        OperationalData,
        SearchVriteriaForOutletPerformance,
        SISearchCriteriaForOutletPerformance,
        DailyGaugeSlip,
        CriteriaforSpecificOutlet,
        SICriteriaforSpecificOutlet,
        SICriteriaforSpecificOutletAlt,
        OutletPerformanceHistory,
        SIOutletPerformanceHistory,
        DischargeDataOfBarrage,
        AccessDenied,
        AddWaterTheft,
        DailyOperationalData,
        SearchBreachIncident,
        AddBreachIncident,
        BreachCaseView,
        SBEWorking,
        SDOWorking,
        XENWorking,
        SEWorking,
        ChiefWorking,
        SearchWaterTheft,
        ReferenceData,
        AddEditIrrigator,
        AddIrrigatorFeedback,
        ViewIrrigatorFeedback,
        IrrigatorFeedbackHistory,
        SearchIrrigator,
        SearchIrrigatorFeedback,
        ViewOffenders,
        Ziladaar,
        ActionOnSchedule,
        AddOutletPerformanceInspectionNotes,
        AddOutletAlterationInspectionNotes,
        UserNotificationPreferences,
        Para2,
        Flow7782,
        FillingFraction,
        ShareDistribution,
        WaterDistrubution,
        EasterComponent,
        Notifications,
        IndentsLagTime,
        DailyIndents,
        IndentsHistory,
        PlacingIndents,
        ViewIndents,
        Reports,
        PerformanceEvaluation,
        ComplaintsType,
        SearchComplaints,
        AdditionalAccessibility,
        AddNewSchedule,
        SearchSchedule,
        AddGaugeInspection,
        AddGeneralInspection,
        AddInspectionNotes,
        AddDischargeTableCalculationBL,
        AddDischargeTableCalculationCL,
        LagTime,
        ControlInfrastructureAddition,
        ControlInfrastructureSearch,
        ControlInfrastructurePhysicalLocation,
        CommandWaterLosses,
        ControlInfrastructureGauges,
        InflowForecasting,
        ChannelWaterLosses,
        SearchIndependent,
        SearchInspection,
        AddIndependent,
        SubDivisionWaterLosses,
        ScheduleCalendar,
        SISearchCriteriaForOutletAlteration,
        RDWiseConditionsIndependentInspection,
        AddComplaint,
        DivisionalWaterLosses,
        TechnicalSanctionUnits,
        ClosureWorkType,
        ClosureWorkPlan,
        SearchEmergencyPurchases,
        DailyDataOperationalReport,
        Tenders,
        AnnualCanalClosureProgram,
        AddAnnualCanalClosureProgram,
        DetailsAnnualCanalClosureProgram,
        AddClosureWork,
        AddClosureWorkPlan,
        ClosureWorkItem,
        SearchFloodFightingPlan,
        ClosureWorkPlanDetails,
        DailyAnalysisReport,
        ScheduleInspectionReport,
        AnnualCanalClosureWorkPlanOrders,
        ViewWorkTenderDetails,
        SearchDivisionStore,
        UnknownError,
        AddWorkProgress,
        WorkProgressHistory,
        ExcludeChanneFromACCP,
        SearchRotationalPlan,
        AddRotationalPlan,
        EntitlementDelivery,
        AssestsAndWorks,
        EffluentandWaterCharges,
        RotationalProgram,
        AssetsCategory,
        Auctions,
        Accounts,
        WorkItem,
        SearchSD,
        SmallDams,
        AddAssetWork,
        AssetWorkSearch,
        AddAssetWorkItem,
        AddAssetWorkProgress,
        AssetWorkProgressHistory,
        IMISCode,
        UserManuals,
        MeterReadingAndFule


    }
    public class PageTitles
    {

        public const string IrrigationModule = "Irrigation Network";
        public const string UserAdministration = "User Administration";
        public const string DailyData = "Daily Operational Data";
        public const string ScheduleInspection = "Schedule and Inspections";
        public const string AccessDenied = "Access Denied";

        public const string UnknownError = "Error !";

        public const string WaterTheft = "Water Theft";
        public const string IrrigatorFeedback = "Irrigator Feedback";
        public const string Notification = "Notification";
        public const string SeasonalPlanning = "Seasonal Planning";
        public const string Reports = "Reports";
        public const string PerformanceEvaluation = "Performance Evaluation";
        public const string ComplaintsManagement = "Complaints Management";
        public const string WaterLosses = "Water Losses";
        public const string ClosureOperations = "Closure Operations";
        public const string TendersMonitoring = "Tenders Management";
        public const string RotationalProgram = "Rotational Program";
        public const string EntitlementDelivery = "Entitlements and Deliveries";
        public const string AssestandWorks = "Assets and Works";
        public const string EWC = "Effluent and Water Charges";
        public const string Auctions = "Auctions";
        public const string Accounts = "Accounts";

        public const string FloodEarlyWarningSystem = "Flood Early Warning System";
        public const string FloodEarlyWarningSystemTitle = "Flood Early Warning System";
        public const string FloodEarlyWarningSystemNav = FloodEarlyWarningSystemTitle;

        public const string FloodOperation = "Flood Operations";
        public const string FloodOperationTitle = "Flood Operations";
        public const string FloodOperationNav = FloodOperationTitle;


        public const string ChannelAddTitle = "Add Channel";
        public const string ChannelAdditionNav = ChannelAddTitle;
        public const string ChannelEditTitle = "Edit Channel";

        public const string ZoneTitle = "Zone";
        public const string ZoneNav = ZoneTitle;

        public const string ChannelSearchTitle = "Channel Search";
        public const string ChannelSearchNav = ChannelSearchTitle;

        public const string OutletAddTitle = "Add Outlet";
        public const string OutletAddNav = OutletAddTitle;

        public const string OutletViewTitle = "Outlets";
        public const string OutletViewNav = OutletViewTitle;

        public const string OutletAltTitle = "Outlet Alteration";
        public const string OutletAltNav = OutletAltTitle;

        public const string GaugeTypeTitle = "Gauge Type";
        public const string GaugeTypeNav = GaugeTypeTitle;

        public const string CircleTitle = "Circle";
        public const string CircleNav = CircleTitle;

        public const string DivisionTitle = "Division";
        public const string DivisionNav = DivisionTitle;

        public const string OutletTypeTitle = "Outlet Type";
        public const string OutletTypeNav = OutletTypeTitle;

        public const string DistrictTitle = "District";
        public const string DistrictNav = DistrictTitle;

        public const string SubDivisionTitle = "Sub Division";
        public const string SubDivisionNav = SubDivisionTitle;

        public const string TehsilTitle = "Tehsil";
        public const string TehsilNav = TehsilTitle;

        public const string PoliceStationTitle = "Police Station";
        public const string PoliceStationNav = PoliceStationTitle;

        public const string VillageTitle = "Village";
        public const string VillageNav = VillageTitle;

        public const string SectionTitle = "Section";
        public const string SectionNav = SectionTitle;

        public const string StructureTitle = "Structure";
        public const string StructureNav = StructureTitle;

        public const string ProvinceTitle = "Province";
        public const string ProvinceNav = ProvinceTitle;

        public const string DivisionDistrictTitle = "Division District Relation";
        public const string DivisionDistrictNav = DivisionDistrictTitle;

        public const string RolesTitle = "Roles";
        public const string RolesNav = RolesTitle;

        public const string ChannelPhysicalLocationTitle = "Physical Location of Channel Data";
        public const string ChannelPhysicalLocationNav = ChannelPhysicalLocationTitle;

        public const string OfficeTitle = "Organization";
        public const string OfficeNav = OfficeTitle;

        public const string BedLevelDTParamTitle = "Discharge Table Parameters for Gauge on Bed Level";
        public const string BedLevelDTParamNav = BedLevelDTParamTitle;

        public const string RoleRightsTitle = "Role Rights";
        public const string RoleRightsNav = RoleRightsTitle;

        public const string ViewBedLevelDTHistoryTitle = "Discharge Table Parameters for Gauge on Bed Level History";
        public const string ViewBedLevelDTHistoryNav = ViewBedLevelDTHistoryTitle;

        public const string DesignationTitle = "Designation";
        public const string DesignationNav = DesignationTitle;

        public const string CrestLevelDTParamTitle = "Discharge Table Parameters for Crest Level Gauges";
        public const string CrestLevelDTParamNav = CrestLevelDTParamTitle;

        public const string ViewCrestLevelDTHistoryTitle = "Discharge Table Parameters for Crest Level Gauges History";
        public const string ViewCrestLevelDTHistoryNav = ViewCrestLevelDTHistoryTitle;

        public const string SearchUserTitle = "Search User";
        public const string SearchUserNav = SearchUserTitle;

        public const string StructureDataTitle = "Sites for Structure/Barrage/Dam/Channel";
        public const string StructureDataNav = StructureDataTitle;

        public const string SearchPlacingIndentsTitle = "Search Placing Indents";
        public const string SearchPlacingIndentsNav = SearchPlacingIndentsTitle;

        public const string AddIndentTitle = "Add Indent";
        public const string AddIndentNav = AddIndentTitle;

        public const string IndentHistoryTitle = "Indent History";
        public const string IndentHistoryNav = IndentHistoryTitle;

        public const string GaugeInformationTitle = "Gauge Information";
        public const string GaugeInformationNav = GaugeInformationTitle;

        public const string ParentFeederChannelTitle = "Parent Channels and Channel Feeders";
        public const string ParentFeederChannelNav = ParentFeederChannelTitle;

        public const string ViewScheduleInspectionTitle = "View Schedule Details";
        public const string ViewScheduleInspectionNav = ViewScheduleInspectionTitle;

        public const string AddUserTitle = "Add User";
        public const string AddUserNav = AddUserTitle;

        public const string EditUserTitle = "Edit User";
        public const string EditUserNav = EditUserTitle;

        public const string AssignRoleToUserTitle = "Assign Role to Users";
        public const string AssignRoleToUserNav = AssignRoleToUserTitle;

        public const string ViewUserTitle = "View User";
        public const string ViewUserNav = ViewUserTitle;

        public const string SearchTempAssignTitle = "Search Temporary Assignments";
        public const string SearchTempAssignNav = SearchTempAssignTitle;

        public const string AssociateLocationToUserTitle = "Associate Location To User";
        public const string AssociateLocationToUserNav = AssociateLocationToUserTitle;

        public const string NotificationsLimitTitle = "Notifications Limit";
        public const string NotificationsLimitNav = NotificationsLimitTitle;

        public const string TempAssignTitle = "Temporary Assignments";
        public const string TempAssignNav = TempAssignTitle;

        public const string AlertConfigurationTitle = "Alert Configuration";
        public const string AlertConfigurationNav = AlertConfigurationTitle;

        public const string AssociateBarrageChannelOutlet = "Associate Barrages, Channels and Outlets";
        public const string AssociateBarrageChannelOutletNav = AssociateBarrageChannelOutlet;

        public const string LSectionParameterTitle = "Add/Edit L Section Parameter";
        public const string LSectionParameterNav = LSectionParameterTitle;

        public const string OutletVillageTitle = "Outlets Villages";
        public const string OutletVillageNav = OutletVillageTitle;

        public const string OutletAlterationHistoryTitle = "Outlet Alteration History";
        public const string OutletAlterationHistoryNav = OutletAlterationHistoryTitle;

        public const string DefineChannelReachTitle = "Define Channel Reach";
        public const string DefineChannelReachNav = DefineChannelReachTitle;

        public const string LSectionParametersHistoryTitle = "L Section Parameters History";
        public const string LSectionParametersHistoryNav = LSectionParametersHistoryTitle;

        public const string ReasonForChangeTitle = "Reason For Change";
        public const string ReasonForChangeNav = ReasonForChangeTitle;

        public const string BarrageDataFrequencyTitle = "Barrage Data Frequency";
        public const string BarrageDataFrequencyNav = BarrageDataFrequencyTitle;

        public const string OutletPerformanceDataTitle = "Outlet Performance Data";
        public const string OutletPerformanceDataNav = OutletPerformanceDataTitle;

        public const string OperationalDataTitle = "Operational Data";
        public const string OperationalDataNav = OperationalDataTitle;

        public const string SearchCriteriaOutletPerformanceTitle = "Search Criteria For Outlet Performance";
        public const string SearchCriteriaOutletPerformanceNav = SearchCriteriaOutletPerformanceTitle;

        public const string LocatespecificOutletTitle = "Search Criteria For Locate Specific Outlet";
        public const string LocatespecificOutletNav = LocatespecificOutletTitle;

        public const string DailyGaugeSlipTitle = "Daily Gauge Slip";
        public const string DailyGaugeSlipNav = DailyGaugeSlipTitle;

        public const string OutletPerformanceHistoryTitle = "Outlet Performance History";
        public const string OutletPerformanceHistoryNav = OutletPerformanceHistoryTitle;

        public const string DischargeDataOfBarrageHeadworkTitle = "Discharge Data of Barrage/Headwork";
        public const string DischargeDataOfBarrageHeadworkNav = DischargeDataOfBarrageHeadworkTitle;

        public const string AccessDeniedTitle = "Access Denied";
        public const string AccessDeniedNav = AccessDeniedTitle;

        public const string UnknownErrorTitle = "Error !!!";
        public const string UnknownErrorNav = UnknownErrorTitle;

        public const string AddWaterTheftTitle = "Water Theft Identification";
        public const string AddwaterTheftNav = AddWaterTheftTitle;

        public const string AddOutletChecking = "Outlet Checking";
        public const string OutletChecking = AddOutletChecking;

        public const string DailyOperationalDataTitle = "Daily Operational Data";
        public const string DailyOperationalDataNav = DailyOperationalDataTitle;

        public const string SearchBreachIncidentTitle = "Breach Incident";
        public const string SearchBreachIncidentNav = SearchBreachIncidentTitle;

        public const string AddBreachIncidentTitle = "Add Breach Incident";
        public const string AddBreachIncidentNav = AddBreachIncidentTitle;

        public const string BreachCaseViewTitle = "View Breach Incident";
        public const string BreachCaseViewNav = BreachCaseViewTitle;

        public const string SBEWorkingTitle = "Water Theft Incident (Working) - Sub Engineer";
        public const string SBEWorkingNav = SBEWorkingTitle;

        public const string SDOWorkingTitle = "Water Theft Incident (Working) - SDO";
        public const string SDOWorkingNav = SDOWorkingTitle;

        public const string XENWorkingTitle = "Water Theft Incident (Working) - XEN";
        public const string XENWorkingNav = XENWorkingTitle;

        public const string SEWorkingTitle = "Water Theft Incident (Working) - SE";
        public const string SEWorkingNav = SEWorkingTitle;

        public const string ChiefWorkingTitle = "Water Theft Incident (Working) - Chief";
        public const string ChiefWorkingNav = ChiefWorkingTitle;

        public const string SearchWaterTheftTitle = "Water Theft Incident";
        public const string SearchWaterTheftNav = SearchWaterTheftTitle;

        public const string ReferenceDataTitle = "Reference Data";
        public const string ReferenceDataNav = ReferenceDataTitle;

        public const string AddEditIrrigatorTitle = "Add/Edit Irrigator";
        public const string AddEditIrrigatorNav = AddEditIrrigatorTitle;

        public const string AddIrrigatorFeedbackTitle = "Add Irrigator Feedback";
        public const string AddIrrigatorFeedbackNav = AddIrrigatorFeedbackTitle;

        public const string ViewIrrigatorFeedbackTitle = "View Irrigator Feedback";
        public const string ViewIrrigatorFeedbackNav = ViewIrrigatorFeedbackTitle;

        public const string IrrigatorFeedbackHistoryTitle = "Irrigator Feedback History";
        public const string IrrigatorFeedbackHistoryNav = IrrigatorFeedbackHistoryTitle;

        public const string SearchIrrigatorTitle = "Search Irrigator";
        public const string SearchIrrigatorNav = SearchIrrigatorTitle;

        public const string SearchIrrigatorFeedbackTitle = "Search Irrigator Feedback";
        public const string SearchIrrigatorFeedbackNav = SearchIrrigatorFeedbackTitle;

        public const string ViewOffendersTitle = "View Offenders";
        public const string ViewOffendersNav = ViewOffendersTitle;

        public const string ZiladaarTitle = "Water theft Incident (Working) - Ziladaar";
        public const string ZiladaarNav = ZiladaarTitle;

        public const string ActionOnScheduleTitle = "Action On Schedule";
        public const string ActionOnScheduleNav = ActionOnScheduleTitle;

        public const string AddOutletAlterationInspectionNotesTitle = "Add Outlet Alteration - Inspection Notes";
        public const string AddOutletAlterationInspectionNotesNav = AddOutletAlterationInspectionNotesTitle;

        public const string AddOutletPerformanceInspectionNotesTitle = "Add Outlet Performance - Inspection Notes";
        public const string AddOutletPerformanceInspectionNotesNav = AddOutletPerformanceInspectionNotesTitle;

        public const string UserNotificationPreferencesTitle = "My Preferences";
        public const string UserNotificationPreferencesNav = UserNotificationPreferencesTitle;

        public const string Para2Title = "Para 2";
        public const string Para2Nav = Para2Title;

        public const string Flow7782Title = "Flow7782";
        public const string Flow7782Nav = Flow7782Title;

        public const string FillingFractionTitle = "Filling Fraction";
        public const string FillingFractionNav = FillingFractionTitle;

        public const string ShareDistributionTitle = "Shared Distribution";
        public const string ShareDistributionNav = ShareDistributionTitle;

        public const string WaterDistributionTitle = "Water Distribution";
        public const string WaterDistributionNav = WaterDistributionTitle;

        public const string EasternComponentTitle = "Eastern River Component";
        public const string EasternComponentNav = EasternComponentTitle;

        public const string NotificationsTitle = "All Notifcations";
        public const string NotificationsNav = NotificationsTitle;

        public const string IndentsLagTimeTitle = "Indents Lag Time";
        public const string IndentsLagTimeNav = IndentsLagTimeTitle;

        public const string DailyIndentsTitle = "Daily Indents";
        public const string DailyIndentsNav = DailyIndentsTitle;

        public const string IndentsHistoryTitle = "Indents History";
        public const string IndentsHistoryNav = IndentsHistoryTitle;

        public const string PlacingIndentsTitle = "Placing Indents";
        public const string PlacingIndentsNav = PlacingIndentsTitle;

        public const string ViewIndentsTitle = "View Indents";
        public const string ViewIndentsNav = ViewIndentsTitle;

        public const string ReportsTitle = "Reports";
        public const string ReportsNav = ReportsTitle;

        public const string ComplaintsTypeTitle = "Complaints Type";
        public const string ComplaintsTypeNav = ComplaintsTypeTitle;

        public const string SearchComplaintsTitle = "Search Complaints";
        public const string SearchComplaintsNav = SearchComplaintsTitle;

        public const string AdditionalAccessibilityTitle = "Additional Accessibility";
        public const string AdditionalAccessibilityNav = AdditionalAccessibilityTitle;


        public const string AddNewScheduleTitle = "Add New Schedule";
        public const string AddNewScheduleNav = AddNewScheduleTitle;

        public const string SearchScheduleTitle = "Search Schedule";
        public const string SearchScheduleNav = SearchScheduleTitle;

        public const string AddGaugeInspectionTitle = "Add Inspection Notes - Gauge Inspection";
        public const string AddGaugeInspectionNav = AddGaugeInspectionTitle;

        public const string AddGeneralInspectionTitle = "Add General Inspection";
        public const string AddGeneralInspectionNav = AddGeneralInspectionTitle;


        public const string AddInspectionNotesTitle = "Add Inspection Notes";
        public const string AddInspectionNotesNav = AddInspectionNotesTitle;

        public const string AddDischargeTableCalculationBLTitle = "Add Inspection Notes - Discharge Table Calculation for Bed Level Gauges";
        public const string AddDischargeTableCalculationBLNav = AddDischargeTableCalculationBLTitle;

        public const string AddDischargeTableCalculationCLTitle = "Add Inspection Notes - Discharge Table Calculation for Crest Level Gauges";
        public const string AddDischargeTableCalculationCLNav = AddDischargeTableCalculationCLTitle;


        public const string LagTimeTitle = "Lag Time";
        public const string LagTimeTitleNav = LagTimeTitle;

        public const string CommandWLTitle = "Command Water Losses";
        public const string CommanWLTitleNav = CommandWLTitle;

        public const string InflowForecastingTitle = "Inflow Forecasting";
        public const string InflowForecastingNav = InflowForecastingTitle;

        public const string ChannelWLTitle = "Channel Water Losses";
        public const string ChannelWLTitleNav = ChannelWLTitle;

        public const string SearchInspectionTitle = "Search Inspection";
        public const string SearchInspectionNav = InflowForecastingTitle;

        public const string SubDivWLTitle = "Sub-Divisional Water Losses";
        public const string SubDivWLTitleNav = SubDivWLTitle;

        public const string ScheduleCalendarTitle = "Schedule Calendar";
        public const string ScheduleCalendarTitleNav = ScheduleCalendarTitle;


        public const string SISearchCriteriaForOutletAlteration = "Search Criteria For Outlet Alteration";
        public const string SISearchCriteriaForOutletAlterationNav = SISearchCriteriaForOutletAlteration;

        public const string SICriteriaforSpecificOutletAlt = "Search Criteria For Locate Specific Outlet";
        public const string SICriteriaforSpecificOutletAltNav = SICriteriaforSpecificOutletAlt;

        public const string OutletAlterationData = "Outlet Performance Data";
        public const string OutletAlterationDatatNav = OutletAlterationData;

        public const string OutletCheckingData = "Outlet Checking";
        public const string OutletCheckingDataNav = OutletCheckingData;

        public const string SISearchCriteriaForOutletPerformance = "Search Criteria For Outlet Performance";
        public const string SISearchCriteriaForOutletPerformancetNav = SISearchCriteriaForOutletPerformance;

        public const string SICriteriaForLocateSpecificOutlet = "Search Criteria For Locate Specific Outlet";
        public const string SICriteriaForLocateSpecificOutletNav = SICriteriaForLocateSpecificOutlet;

        public const string SIOutletPerformanceData = "Outlet Performance Data";
        public const string SIOutletPerformanceDataNav = SIOutletPerformanceData;

        public const string SIOutletPerformanceHistory = "Outlet Performance History";
        public const string SIOutletPerformanceHistoryNav = SIOutletPerformanceHistory;

        public const string AddComplaintsTitle = "Add Complaint";
        public const string AddComplaintsNav = AddComplaintsTitle;

        public const string DivWLTitle = "Divisional Water Losses";
        public const string DivWLTitleNav = DivWLTitle;

        #region Closure Opertations
        public const string TechnicalSanctionUnitsTitle = "Technical Sanction Units";
        public const string TechnicalSanctionUnitsNav = TechnicalSanctionUnitsTitle;

        //AssetsWorksManagement
        public const string AssetsWorksManagement = "Assets and Works Management";
        public const string AssetCategoryTitle = "Asset Category";
        public const string AssetCategoryNav = AssetCategoryTitle;



        public const string AddAssetWork = "Add Asset Work";
        public const string AddAssetWorkNav = AddAssetWork;

        public const string AssetWorkSearch = "Asset Work Search";
        public const string AssetWorkSearchNav = AssetWorkSearch;


        public const string AddAssetWorkItem = "Add Asset Work Item";
        public const string AddAssetWorkItemNav = AddAssetWorkItem;

        public const string AddAssetWorkProgress = "Add Asset Work Progress";
        public const string AddAssetWorkProgressNav = AddAssetWorkProgress;


        public const string AssetWorkProgressHistory = "Asset Work Progress History";
        public const string AssetWorkProgressHistoryNav = AssetWorkProgressHistory;


        public const string ClosureWorkTypeTitle = "Closure Work Type";
        public const string ClosureWorkTypeNav = ClosureWorkTypeTitle;

        public const string ACCPTitle = "Annual Canal Closure Program";
        public const string ACCPNav = ACCPTitle;

        public const string AddACCPTitle = "Add Annual Canal Closure Program";
        public const string AddACCPNav = AddACCPTitle;

        public const string CWPTitle = "Closure Work Plan";
        public const string CWPNav = CWPTitle;

        public const string ACWPTitle = "Add Closure Work Plan";
        public const string ACWPNav = ACWPTitle;


        public const string DetailACCPTitle = "Annual Canal Closure Program Details";
        public const string DetailACCPNav = DetailACCPTitle;

        public const string AddCWTitle = "Add Closure Work";
        public const string AddCWPNav = AddCWTitle;

        public const string CWItemTitle = "Closure Work Item";
        public const string CWItemNav = CWItemTitle;

        public const string ACCWPOrderItemTitle = "Annual Canal Closure Work Plan Order";
        public const string ACCWPOrderItemNav = ACCWPOrderItemTitle;

        public const string AddWorkPrgrsTitle = "Add Work Progress";
        public const string AddWorkPrgrsNav = AddWorkPrgrsTitle;

        public const string WorkPrgrsHistoryTitle = "Work Progress History";
        public const string WorkPrgrsHistoryNav = WorkPrgrsHistoryTitle;


        public const string ExcludeChannelTitle = "Exclude Channel From Annual Canal Closure Programme";
        public const string ExcludeChannelNav = ExcludeChannelTitle;
        #endregion

        #region "Reports"
        public const string DailyOperationalReportsTitle = "Daily Operational Data Reports";
        public const string DailyOperationalReportsNav = DailyOperationalReportsTitle;

        public const string DailyAnalysisReportTitle = "Daily Analysis Report";
        public const string DailyAnalysisReportNav = DailyAnalysisReportTitle;

        public const string ScheduleInspectionReportTitle = "Schedule and Inspections Reports";
        public const string ScheduleInspectionReportNav = ScheduleInspectionReportTitle;
        #endregion

        public const string ViewWTDetailsTitle = "View Work & Tender Details";
        public const string ViewWTDetailsNav = ViewWTDetailsTitle;

        public const string SearchRotationalPlanTitle = "Search Rotatioal plan";
        public const string SearchRotationalPlanNav = SearchRotationalPlanTitle;

        public const string AddRotationalPlanTitle = "Add Rotatioal plan";
        public const string AddRotationalPlanNav = AddRotationalPlanTitle;

        public const string IMISCodeTitle = "IMIS Code";
        public const string IMISCodeNav = IMISCodeTitle;

        public const string UserManualTitle = "User Manuals";
        public const string UserManualNav = UserManualTitle;

        public const string MeterReadingAndFuleTitle = "Meter Reading And Fule";
        public const string MeterReadingAndFuleTitleNav = MeterReadingAndFuleTitle;

        #region "Auctions"
        public const string AddAuctionNoticeTitle = "Auction Notice";
        public const string AddAuctionNoticeNav = AddAuctionNoticeTitle;
        #endregion

        #region Small Dams
        public const string SmallDams = "Small Dams";
        public const string SmallDamsTitle = "Add New Dam";
        public const string SmallDamsNav = SmallDamsTitle;

        //public const string TechnicalParametersSDTitle = "Technical Parameters";
        //public const string TechnicalParametersSDNav = TechnicalParametersSDTitle;

        //public const string ChannelsSDTitle = "Channels";
        //public const string ChannelsSDNav = ChannelsSDTitle;

        //public const string VillageBenefittedSDTitle = "Village Benefitted";
        //public const string VillageBenefittedSDNav = VillageBenefittedSDTitle;

        //public const string AnnualOMCostSDTitle = "Annual Operation and Maintenance Cost";
        //public const string AnnualOMCostSDNav = AnnualOMCostSDTitle;

        //public const string SearchSmallDamReadingSDTitle = "Search Small Dam Reading";
        //public const string SearchSmallDamReadingSDNav = SearchSmallDamReadingSDTitle;


        //public const string ViewSmallDamReadingSDTitle = "View Small Dam Reading";
        //public const string ViewSmallDamReadingSDNav = ViewSmallDamReadingSDTitle;

        //public const string DamTypeSDTitle = "Dam Type";
        //public const string DamTypeSDNav = DamTypeSDTitle;

        //public const string SpillwayTypeSDTitle = "Spillway Type";
        //public const string SpillwayTypeSDNav = SpillwayTypeSDTitle;

        //public const string DamReadingSDTitle = "SmallDam Reading";
        //public const string DamReadingSDNav = DamReadingSDTitle;
        #endregion


    }

}