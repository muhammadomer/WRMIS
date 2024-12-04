using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Common
{

    public class ErrorMessage
    {
        public int Code;
        public string Description;

        public ErrorMessage(int _Code, string _Description)
        {
            Code = _Code;
            Description = _Description;
        }
    }
    public class Message
    {
        public static ErrorMessage GeneralError
        {
            get { return new ErrorMessage(101, "Some unknown error occured. Please try again later."); }
        }
        public static ErrorMessage LoginFailedError
        {
            get { return new ErrorMessage(102, "Username or Password is invalid."); }
        }
        public static ErrorMessage ItemSaved
        {
            get { return new ErrorMessage(103, "Item has been Saved successfully."); }
        }

        #region Zone

        public static ErrorMessage ZoneNameExists
        {
            get { return new ErrorMessage(107, "Zone Name already exists."); }
        }

        #endregion

        #region Gauge Type

        public static ErrorMessage GaugeTypeNameExists
        {
            get { return new ErrorMessage(114, "Gauge Type Name already Exists."); }
        }

        #endregion

        #region Circle

        public static ErrorMessage CircleNameExists
        {
            get { return new ErrorMessage(122, "Circle Name already exists."); }
        }

        #endregion

        #region Outlet Type Messages

        public static ErrorMessage OutletTypeExists
        {
            get { return new ErrorMessage(128, "Outlet Type Name already Exist."); }
        }
        public static ErrorMessage OutletTypeDescriptionExists
        {
            get { return new ErrorMessage(132, "Outlet Type Abbreviation exists."); }
        }

        #endregion

        #region Outlet View

        public static ErrorMessage OutletDeleted
        {
            get { return new ErrorMessage(134, "Outlet Record has been deleted successfully."); }
        }

        #endregion

        #region Division

        public static ErrorMessage DivisionNameExists
        {
            get { return new ErrorMessage(140, "Division Name already exists."); }
        }

        #endregion

        #region District Messages

        public static ErrorMessage DistrictNameExists
        {
            get { return new ErrorMessage(146, "District Name already exists."); }
        }

        #endregion

        #region Tehsil Messages

        public static ErrorMessage TehsilDeleted
        {
            get { return new ErrorMessage(151, "Tehsil has been deleted successfully."); }
        }
        public static ErrorMessage TehsilNotDeleted
        {
            get { return new ErrorMessage(152, "Tehsil could not be deleted."); }
        }
        public static ErrorMessage TehsilNameRequired
        {
            get { return new ErrorMessage(153, "Tehsil Name is required."); }
        }
        public static ErrorMessage TehsilNameExists
        {
            get { return new ErrorMessage(154, "Tehsil Name already exists."); }
        }
        public static ErrorMessage TehsilSaved
        {
            get { return new ErrorMessage(155, "Tehsil has been saved successfully."); }
        }
        public static ErrorMessage TehsilNotSaved
        {
            get { return new ErrorMessage(156, "Tehsil could not be saved."); }
        }

        #endregion

        #region Police Station Messages

        public static ErrorMessage PoliceStationReferenceExists
        {
            get { return new ErrorMessage(157, "Police Station cannot be deleted as its relevant data exists."); }
        }
        public static ErrorMessage PoliceStationDeleted
        {
            get { return new ErrorMessage(158, "Police Station has been deleted successfully."); }
        }
        public static ErrorMessage PoliceStationNotDeleted
        {
            get { return new ErrorMessage(159, "Police Station could not be deleted."); }
        }
        public static ErrorMessage PoliceStationNameRequired
        {
            get { return new ErrorMessage(160, "Police Station Name is required."); }
        }
        public static ErrorMessage PoliceStationNameExists
        {
            get { return new ErrorMessage(161, "Police Station Name already exists."); }
        }
        public static ErrorMessage PoliceStationSaved
        {
            get { return new ErrorMessage(162, "Police Station has been saved successfully."); }
        }
        public static ErrorMessage PoliceStationNotSaved
        {
            get { return new ErrorMessage(163, "Police Station could not be saved."); }
        }

        #endregion

        #region Sub Division

        public static ErrorMessage SubDivisionNameRequired
        {
            get { return new ErrorMessage(167, "Sub Division Name is required."); }
        }
        public static ErrorMessage SubDivisionNameExists
        {
            get { return new ErrorMessage(168, "Sub Division Name already exists."); }
        }
        public static ErrorMessage SubDivisionSaved
        {
            get { return new ErrorMessage(169, "Sub Division has been saved successfully."); }
        }
        public static ErrorMessage SubDivisionNotSaved
        {
            get { return new ErrorMessage(170, "Sub Division could not be saved."); }
        }

        #endregion

        #region Village Messages

        public static ErrorMessage VillageReferenceExists
        {
            get { return new ErrorMessage(171, "Village cannot be deleted as its relevant data exists."); }
        }
        public static ErrorMessage VillageDeleted
        {
            get { return new ErrorMessage(172, "Village has been deleted successfully."); }
        }
        public static ErrorMessage VillageNotDeleted
        {
            get { return new ErrorMessage(173, "Village could not be deleted."); }
        }
        public static ErrorMessage VillageNameRequired
        {
            get { return new ErrorMessage(174, "Village Name is required."); }
        }
        public static ErrorMessage VillageNameExists
        {
            get { return new ErrorMessage(175, "Village Name already exists."); }
        }
        public static ErrorMessage VillageSaved
        {
            get { return new ErrorMessage(176, "Village has been saved successfully."); }
        }
        public static ErrorMessage VillageNotSaved
        {
            get { return new ErrorMessage(177, "Village could not be saved."); }
        }

        #endregion

        #region Section

        public static ErrorMessage SectionReferenceExists
        {
            get { return new ErrorMessage(178, "Section cannot be deleted as its relevant data exists."); }
        }
        public static ErrorMessage SectionDeleted
        {
            get { return new ErrorMessage(179, "Section has been deleted successfully."); }
        }
        public static ErrorMessage SectionNotDeleted
        {
            get { return new ErrorMessage(180, "Section could not be deleted."); }
        }
        public static ErrorMessage SectionNameRequired
        {
            get { return new ErrorMessage(181, "Section Name is required."); }
        }
        public static ErrorMessage SectionNameExists
        {
            get { return new ErrorMessage(182, "Section Name already exists."); }
        }
        public static ErrorMessage SectionSaved
        {
            get { return new ErrorMessage(183, "Section has been saved successfully."); }
        }
        public static ErrorMessage SectionNotSaved
        {
            get { return new ErrorMessage(184, "Section could not be saved."); }
        }

        #endregion

        #region Province

        public static ErrorMessage ProvinceReferenceExists
        {
            get { return new ErrorMessage(185, "Province cannot be deleted as its relevant data exists."); }
        }
        public static ErrorMessage ProvinceDeleted
        {
            get { return new ErrorMessage(186, "Province has been deleted successfully."); }
        }
        public static ErrorMessage ProvinceNotDeleted
        {
            get { return new ErrorMessage(187, "Province could not be deleted."); }
        }
        public static ErrorMessage ProvinceNameRequired
        {
            get { return new ErrorMessage(188, "Province Name is required."); }
        }
        public static ErrorMessage ProvinceNameExists
        {
            get { return new ErrorMessage(189, "Province Name already exists."); }
        }
        public static ErrorMessage ProvinceSaved
        {
            get { return new ErrorMessage(190, "Province has been saved successfully."); }
        }
        public static ErrorMessage ProvinceNotSaved
        {
            get { return new ErrorMessage(191, "Province could not be saved."); }
        }

        #endregion

        #region Structure

        public static ErrorMessage StructureDeleted
        {
            get { return new ErrorMessage(192, "Structure has been deleted successfully."); }
        }
        public static ErrorMessage StructureNotDeleted
        {
            get { return new ErrorMessage(193, "Structure could not be deleted."); }
        }
        public static ErrorMessage StructureNameRequired
        {
            get { return new ErrorMessage(194, "Structure Name is required."); }
        }
        public static ErrorMessage StructureNameExists
        {
            get { return new ErrorMessage(195, "Structure Name already exists."); }
        }
        public static ErrorMessage StructureSaved
        {
            get { return new ErrorMessage(196, "Structure has been saved successfully."); }
        }
        public static ErrorMessage StructureNotSaved
        {
            get { return new ErrorMessage(197, "Structure could not be saved."); }
        }
        public static ErrorMessage ProvinceRequired
        {
            get { return new ErrorMessage(198, "Province is required."); }
        }
        public static ErrorMessage RiverRequired
        {
            get { return new ErrorMessage(199, "River is required."); }
        }

        #endregion

        #region District Division
        public static ErrorMessage DistrictNotSelected
        {
            get { return new ErrorMessage(200, "No District is selected."); }
        }

        public static ErrorMessage DistrictUpdated
        {
            get { return new ErrorMessage(201, "District Division Relation Updated."); }
        }

        #endregion

        #region Office Messages

        public static ErrorMessage OfficeSaved
        {
            get { return new ErrorMessage(202, "Organization has been saved successfully."); }
        }
        public static ErrorMessage OfficeNotSaved
        {
            get { return new ErrorMessage(203, "Organization could not be saved."); }
        }
        public static ErrorMessage OfficeNameRequired
        {
            get { return new ErrorMessage(204, "Organization Name is required."); }
        }
        public static ErrorMessage OfficeNameExists
        {
            get { return new ErrorMessage(205, "Office Name already exists."); }
        }
        public static ErrorMessage OfficeNotDeleted
        {
            get { return new ErrorMessage(206, "Organization could not be deleted."); }
        }
        public static ErrorMessage OfficeDeleted
        {
            get { return new ErrorMessage(207, "Organization has been deleted successfully."); }
        }
        public static ErrorMessage OfficeReferenceExists
        {
            get { return new ErrorMessage(208, "Office cannot be deleted as its relevant data exists."); }
        }

        #endregion

        # region Roles

        public static ErrorMessage RoleSaved
        {
            get { return new ErrorMessage(209, "Record saved successfully."); }
        }
        public static ErrorMessage RoleDeleted
        {
            get { return new ErrorMessage(210, "Record deleted successfully."); }
        }
        public static ErrorMessage RoleNotSaved
        {
            get { return new ErrorMessage(211, "Record could not be saved."); }
        }
        public static ErrorMessage RoleNameRequired
        {
            get { return new ErrorMessage(212, "Role Name is required."); }
        }
        public static ErrorMessage RoleNameExists
        {
            get { return new ErrorMessage(213, "Unique value is required."); }
        }
        public static ErrorMessage RoleNotDeleted
        {
            get { return new ErrorMessage(214, "Role cannot be deleted as it is associated with a user."); }
        }

        #endregion

        #region Bed Level DT Params

        public static ErrorMessage OldDatesNotAllowed
        {
            get { return new ErrorMessage(215, "Past dates cannot be entered."); }
        }

        public static ErrorMessage CannotBeGreaterThanEqualToBedSiltedCorrection
        {
            get { return new ErrorMessage(216, "Gauge Correction Value cannot be greater that or equal to Mean Depth for Bed Silted Correction."); }
        }

        public static ErrorMessage ExponentValueValid
        {
            get { return new ErrorMessage(217, "Please enter valid Exponent Value."); }
        }

        public static ErrorMessage MeanDepthRequired
        {
            get { return new ErrorMessage(218, "Mean Depth is required."); }
        }

        public static ErrorMessage MeanDepthValid
        {
            get { return new ErrorMessage(219, "Please enter valid Mean Depth."); }
        }

        public static ErrorMessage ObservedDischargeRequired
        {
            get { return new ErrorMessage(220, "Observed Discharge is required."); }
        }

        public static ErrorMessage ObservedDischargeValid
        {
            get { return new ErrorMessage(221, "Please enter valid Observed Discharge."); }
        }

        public static ErrorMessage GaugeValueCorrectionRequired
        {
            get { return new ErrorMessage(222, "Gauge Value Correction is required."); }
        }

        public static ErrorMessage GaugeValueCorrectionValid
        {
            get { return new ErrorMessage(223, "Please enter valid Gauge Value Correction."); }
        }

        public static ErrorMessage BedLevelDTParamatersSaved
        {
            get { return new ErrorMessage(224, "Bed Level DT Paramater has been saved successfully."); }
        }
        public static ErrorMessage BedLevelDTParamatersNotSaved
        {
            get { return new ErrorMessage(225, "Bed Level DT Paramater could not be saved."); }
        }
        public static ErrorMessage DateCannotBeGreater
        {
            get { return new ErrorMessage(106, "To Date should be greater than or equal to From date."); }
        }
        public static ErrorMessage ObservationDateCannotBeGreater
        {
            get { return new ErrorMessage(106, "Observation Date should be less than or equal to Approval date."); }
        }
        #endregion

        #region Designation

        public static ErrorMessage DesignationSaved
        {
            get { return new ErrorMessage(226, "Designation has been saved successfully."); }
        }
        public static ErrorMessage DesignationNotSaved
        {
            get { return new ErrorMessage(227, "Designation could not be saved."); }
        }
        public static ErrorMessage DesignationOrganizationDuplication
        {
            get { return new ErrorMessage(228, "Designation and Organization combination already exists."); }
        }
        public static ErrorMessage DesignationExists
        {
            get { return new ErrorMessage(229, "Designation already exists."); }
        }
        public static ErrorMessage DesignationNotDeleted
        {
            get { return new ErrorMessage(230, "Designation could not be deleted."); }
        }
        public static ErrorMessage DesignationDeleted
        {
            get { return new ErrorMessage(231, "Designation has been deleted successfully."); }
        }
        public static ErrorMessage DesignationReferenceExists
        {
            get { return new ErrorMessage(232, "Designation cannot be deleted as it is associated with User."); }
        }
        public static ErrorMessage AuthorityRightsAllocated
        {
            get { return new ErrorMessage(233, "Authority Rights Allocated to the User."); }
        }
        public static ErrorMessage DesignationPartOfWorkflow
        {
            get { return new ErrorMessage(234, "Designation cannot be deleted as it is associated with application workflow."); }
        }

        #endregion

        #region RoleRights

        public static ErrorMessage RoleRightSaved
        {
            get { return new ErrorMessage(235, "Record saved successfully."); }
        }

        public static ErrorMessage SelectRole
        {
            get { return new ErrorMessage(274, "Please select role."); }
        }



        #endregion

        #region Bed Level DT Params

        public static ErrorMessage FallBreathRequired
        {
            get { return new ErrorMessage(236, "Breadth of Fall is required."); }
        }
        public static ErrorMessage FallBreathValid
        {
            get { return new ErrorMessage(237, "Please enter valid Breadth of Fall."); }
        }
        public static ErrorMessage HeadCrestRequired
        {
            get { return new ErrorMessage(238, "Head Above Crest is required."); }
        }
        public static ErrorMessage HeadCrestValid
        {
            get { return new ErrorMessage(239, "Please enter valid Head Above Crest."); }
        }
        public static ErrorMessage CrestLevelDTParamatersSaved
        {
            get { return new ErrorMessage(240, "Crest Level DT Paramater has been saved successfully."); }
        }
        public static ErrorMessage CrestLevelDTParamatersNotSaved
        {
            get { return new ErrorMessage(241, "Crest Level DT Paramater could not be saved."); }
        }

        #endregion

        #region Structure Data

        public static ErrorMessage StructureDataReferenceExists
        {
            get { return new ErrorMessage(243, "Structure Data cannot be deleted as its relevant data exists."); }
        }
        public static ErrorMessage StructureDataDeleted
        {
            get { return new ErrorMessage(244, "Structure Data been deleted successfully."); }
        }
        public static ErrorMessage StructureDataNotDeleted
        {
            get { return new ErrorMessage(245, "Structure Data could not be deleted."); }
        }
        public static ErrorMessage SiteNameRequired
        {
            get { return new ErrorMessage(246, "Site Name is required."); }
        }
        public static ErrorMessage SiteNameExists
        {
            get { return new ErrorMessage(247, "Site Name already exists."); }
        }
        public static ErrorMessage SiteChannelNameExists
        {
            get { return new ErrorMessage(248, "Site Name for the selected Channel already exists."); }
        }
        public static ErrorMessage SiteNotSaved
        {
            get { return new ErrorMessage(249, "Site could not be saved."); }
        }
        public static ErrorMessage GaugeRDRequired
        {
            get { return new ErrorMessage(250, "Gauge RDs are required."); }
        }

        #endregion

        #region Add Indent

        public static ErrorMessage IndentSaved
        {
            get { return new ErrorMessage(251, "Indent Saved Successfully."); }
        }

        public static ErrorMessage IndentNotSaved
        {
            get { return new ErrorMessage(252, "Indent canot be Saved."); }
        }

        public static ErrorMessage NegativeValueNotAllowed
        {
            get { return new ErrorMessage(253, "Negative Value Not Allowed."); }
        }

        public static ErrorMessage IndentValueMoreThanDesignDischarge
        {
            get { return new ErrorMessage(254, "Discharge on the given Gauge is Excessive’ but will allow to save the value."); }
        }

        public static ErrorMessage DateShouldBeGreaterThanOrEqualToCurrentDate
        {
            get { return new ErrorMessage(285, "Date Should be greater than or equal to current date"); }
        }

        #endregion

        #region User

        public static ErrorMessage NoAuthorityRights
        {
            get { return new ErrorMessage(255, "You do not have authority rights to access these users."); }
        }

        public static ErrorMessage UserNameExists
        {
            get { return new ErrorMessage(256, "User Name already exists."); }
        }

        public static ErrorMessage UserEmailExists
        {
            get { return new ErrorMessage(257, "Email Address already exists."); }
        }

        public static ErrorMessage UserMobileExists
        {
            get { return new ErrorMessage(257, "Mobile Number already exists."); }
        }

        public static ErrorMessage ZoneAlreadyAssigned
        {
            get { return new ErrorMessage(258, "Zone has already been assigned to user."); }
        }

        public static ErrorMessage CircleAlreadyAssigned
        {
            get { return new ErrorMessage(259, "Circle has already been assigned to user."); }
        }

        public static ErrorMessage DivisionAlreadyAssigned
        {
            get { return new ErrorMessage(260, "Division has already been assigned to user."); }
        }

        public static ErrorMessage SubDivisionAlreadyAssigned
        {
            get { return new ErrorMessage(261, "Sub Division has already been assigned to user."); }
        }

        public static ErrorMessage SectionAlreadyAssigned
        {
            get { return new ErrorMessage(262, "Section has already been assigned to user."); }
        }

        public static ErrorMessage NoIrrigationLevel
        {
            get { return new ErrorMessage(263, "No irrigation Level has been assigned to user."); }
        }

        public static ErrorMessage SelectionTillLeafLevel
        {
            get { return new ErrorMessage(264, "Selection Upto Leaf level is mandatory."); }
        }

        public static ErrorMessage OneLocationMustBeAssigned
        {
            get { return new ErrorMessage(265, "Location already assigned."); }
        }

        public static ErrorMessage RemovalSelect
        {
            get { return new ErrorMessage(266, "Select any level for removal."); }
        }

        public static ErrorMessage LocationAssigned
        {
            get { return new ErrorMessage(267, "Location has been assigned to user."); }
        }

        public static ErrorMessage SelectUserManager
        {
            get { return new ErrorMessage(267, "Please select user manager."); }
        }

        public static ErrorMessage LocationNotSelected
        {
            get { return new ErrorMessage(267, "First associate location to user then proceed."); }
        }

        #endregion

        #region Temporary Assignment

        public static ErrorMessage SamePersonAssignment
        {
            get { return new ErrorMessage(113, "Delegation From and Delegating To cannot be same."); }
        }
        public static ErrorMessage UserAssignedPreviously
        {
            get { return new ErrorMessage(121, "The Delegating From user has been assigned another user’s responsibilities. By delegating the responsibilities, you will delegate all his responsibilities. Do you want to continue?"); }
        }
        public static ErrorMessage DateBeforeToday
        {
            get { return new ErrorMessage(127, "Date should not be less than current date."); }
        }
        public static ErrorMessage EditPastRecord
        {
            get { return new ErrorMessage(131, "You cannot edit current and past date record."); }
        }
        public static ErrorMessage DeletePastRecord
        {
            get { return new ErrorMessage(135, "You cannot delete current and past date record."); }
        }

        #endregion

        #region Gauge Slip Site

        public static ErrorMessage FutureDateNotAllowed
        {
            get { return new ErrorMessage(110, "Future dates are not allowed."); }
        }

        #endregion

        #region Common Messages

        public static ErrorMessage FieldRequired
        {
            get { return new ErrorMessage(264, "This field is required."); }
        }

        public static ErrorMessage UniqueValueRequired
        {
            get { return new ErrorMessage(265, "Unique value is required."); }
        }

        public static ErrorMessage UniqueIMISValueRequired
        {
            get { return new ErrorMessage(265, "Unique IMIS value is required."); }
        }

        public static ErrorMessage RecordDeleted
        {
            get { return new ErrorMessage(266, "Record deleted successfully."); }
        }

        public static ErrorMessage RecordSaved
        {
            get { return new ErrorMessage(267, "Record saved successfully."); }
        }
        public static ErrorMessage PasswordChange
        {
            get { return new ErrorMessage(268, "Password changed successfully."); }
        }
        public static ErrorMessage RecordNotSaved
        {
            get { return new ErrorMessage(269, "Record not saved."); }
        }

        public static ErrorMessage RecordNotDeleted
        {
            get { return new ErrorMessage(270, "Record cannot be deleted."); }
        }

        public static ErrorMessage RecordAssociationsNotDeleted
        {
            get { return new ErrorMessage(270, "Record cannot be deleted. Associated with other record(s)."); }
        }

        public static ErrorMessage RecordAssociationsNotEdited
        {
            get { return new ErrorMessage(300, "Record cannot be edited. Associated with other record(s)."); }
        }


        #endregion

        #region Associate barrage Chanel Outlet and Location

        public static ErrorMessage SelectRD
        {
            get { return new ErrorMessage(271, "Please select RD."); }
        }

        public static ErrorMessage SelectOutlet
        {
            get { return new ErrorMessage(272, "Please select outlet."); }
        }

        public static ErrorMessage SelectSite
        {
            get { return new ErrorMessage(272, "Please select chanel site."); }
        }

        public static ErrorMessage SelectBarrage
        {
            get { return new ErrorMessage(276, "Please select barrage."); }
        }

        public static ErrorMessage NotAllowedToAdd
        {
            get { return new ErrorMessage(273, "You are not allowed to make changes."); }
        }
        public static ErrorMessage NoLocationAssigned
        {
            get { return new ErrorMessage(288, "This user has not been assigned any location. Please assign the location to associate Barrages, Channels, Outlets to this user."); }
        }

        #endregion

        #region Daily Data Messages

        public static ErrorMessage ValidRecordsSaved
        {
            get { return new ErrorMessage(165, "All valid records have been saved successfully."); }
        }
        public static ErrorMessage SomeRecordsNotSaved
        {
            get { return new ErrorMessage(166, "Some records could not be saved."); }
        }

        public static ErrorMessage DivideByZero
        {
            get { return new ErrorMessage(280, "Cannot divide by Zero."); }
        }

        public static ErrorMessage NoOutlet
        {
            get { return new ErrorMessage(281, "No Outlet is available for the selected Channel under Jurisdiction."); }
        }

        public static ErrorMessage ExcessiveDischarge
        {
            get { return new ErrorMessage(282, "Discharge on the given Gauge is Excessive"); }
        }

        public static ErrorMessage NegativeGaugeValue
        {
            get { return new ErrorMessage(283, "Negative Gauge Value is not possible"); }
        }


        public static ErrorMessage ValueAlreadySet
        {
            get { return new ErrorMessage(284, "The selected Data Frequency for Barrage is already set"); }
        }

        public static ErrorMessage ValueDoesNotExist
        {
            get { return new ErrorMessage(284, "Design discharge does not exist "); }
        }

        public static ErrorMessage DischargeCannotBeCalculated
        {
            get { return new ErrorMessage(285, "In absence of discharge table parameter, discharge cannot be calculated"); }
        }

        public static ErrorMessage ValueCannotBeGreaterThanMaxValue
        {
            get { return new ErrorMessage(421, "Gauge value must be between 0 to 30!"); }
        }

        public static ErrorMessage MultipleOffTakes
        {
            get { return new ErrorMessage(286, "Not allowed to edit because tail has multiple offtakes."); }
        }

        public static ErrorMessage DTParametersNotAvailable
        {
            get { return new ErrorMessage(287, "Please Add DT Parameters against gauges to add this record."); }
        }

        public static ErrorMessage EnterFrequency
        {
            get { return new ErrorMessage(288, "Please Enter Gauge Reading Frequency of Barrage"); }
        }

        public static ErrorMessage SuggestedDate
        {
            get { return new ErrorMessage(289, "Suggested Indent Date is populated in TextBox"); }
        }

        public static ErrorMessage VelocitySpeed
        {
            get { return new ErrorMessage(289, "Velocity should be greater than 0 and less than 15"); }
        }

        public static ErrorMessage ReasonForChange
        {
            get { return new ErrorMessage(289, "Select Reason for change."); }
        }

        public static ErrorMessage GaugeValueOutOFRange
        {
            get { return new ErrorMessage(289, "Gauge value should be between 0 to 30"); }
        }
        public static ErrorMessage SubDivisinalGaugeNotFound
        {
            get { return new ErrorMessage(289, "No Subdivisional or head gauge found for the selected channel"); }
        }
        #region Irrigator Feedback

        public static ErrorMessage FromDateCannotBeGreaterThanToDate
        {
            get { return new ErrorMessage(289, "From date cannot be greater than To date"); }
        }

        public static ErrorMessage FromAndToDateCannotBeGreaterThanCurrentDate
        {
            get { return new ErrorMessage(290, "From and To date cannot be greater than current date"); }
        }


        #endregion


        #endregion

        #region Irrigator Feedback
        public static ErrorMessage Mobile1Exist
        {
            get { return new ErrorMessage(291, "Mobile No 1 should b unique"); }
        }

        public static ErrorMessage Mobile2Exist
        {
            get { return new ErrorMessage(292, "Mobile No 2 should b unique"); }
        }

        public static ErrorMessage RecordSavedComplaintGenerated
        {
            get { return new ErrorMessage(292, "Record is saved and Complaint is Generated"); }
        }
        #endregion

        #region Schedule and Inspection
        public static ErrorMessage TODateAndFromDate
        {
            get { return new ErrorMessage(293, "From or To date cannot be less than Current date."); }
        }
        public static ErrorMessage FileFormatNotSupported
        {
            get { return new ErrorMessage(420, "File Format not Supported."); }
        }
        public static ErrorMessage TODateCannotBeLessFromDate
        {
            get { return new ErrorMessage(293, "To date cannot be less than From date."); }
        }
        public static ErrorMessage FromDateCannotBeGreaterToDate
        {
            get { return new ErrorMessage(400, "From date should not be greater than To date."); }
        }
        public static ErrorMessage ScheduleApprovalSuccess
        {
            get { return new ErrorMessage(295, "Schedule has been successfully Send for Approval."); }
        }

        public static ErrorMessage ScheduleApproved
        {
            get { return new ErrorMessage(296, "Schedule has been Approved."); }
        }

        public static ErrorMessage AssignedToUserNotAvailable
        {
            get { return new ErrorMessage(297, "Assigned to user is not available."); }
        }

        public static ErrorMessage NotAllowedToDelete
        {
            get { return new ErrorMessage(298, "Not allowed to delete record."); }
        }

        public static ErrorMessage VisitNotInScheduledDates
        {
            get { return new ErrorMessage(299, "Date of Visit must be between From & To date of the Schedule."); }
        }
        public static ErrorMessage DuplicateRecord
        {
            get { return new ErrorMessage(299, "Duplicate records will not save on same date."); }
        }
        
        public static ErrorMessage DateRangeWasInUse
        {
            get { return new ErrorMessage(301, "Date of Visit in Schedule Details does not fall in this range of From & To date."); }
        }

        public static ErrorMessage InspectionDateInvalid
        {
            get { return new ErrorMessage(301, "Date of Inspection does not fall in Schedule range of From & To date."); }
        }


        public static ErrorMessage BothDatesRequired
        {
            get { return new ErrorMessage(302, "Both dates are required."); }
        }

        public static ErrorMessage Inspectiondate
        {
            get { return new ErrorMessage(302, "Inspection date should not be greater than current date."); }
        }
        public static ErrorMessage GaugeValuesRequired
        {
            get { return new ErrorMessage(302, "Gauge Fixed and Gauge Painted are required."); }
        }
        public static ErrorMessage gaugeRequired
        {
            get { return new ErrorMessage(302, "Gauge value is required."); }
        }

        #endregion

        #region Water Theft

        public static ErrorMessage WaterTheftRecordAdded
        {
            get { return new ErrorMessage(302, "has been logged into the system. Please refer to this ID for further Processing."); }
        }
        public static ErrorMessage EnterCorrectRangeRD
        {
            get { return new ErrorMessage(303, "Please enter the correct range of Channel RDs."); }
        }

        public static ErrorMessage EnterCorrectDate
        {
            get { return new ErrorMessage(304, "Case is not entered in future Date."); }
        }

        public static ErrorMessage ReleventSBE
        {
            get { return new ErrorMessage(305, "Relevent SBE not found."); }
        }

        public static ErrorMessage RecordAlreadyExist
        {
            get { return new ErrorMessage(306, "Record Already Exist."); }
        }

        public static ErrorMessage RequiredFields
        {
            get { return new ErrorMessage(306, "Name and Address is required."); }
        }



        #endregion

        #region Peformance Evaluation

        public static ErrorMessage ComplexityFactorReset
        {
            get { return new ErrorMessage(104, "All Complexity Factors have been reset to default values."); }
        }
        public static ErrorMessage ComplexityFactorNotReset
        {
            get { return new ErrorMessage(105, "Complexity Factors could not be reset to default values."); }
        }
        public static ErrorMessage KPICategoriesReset
        {
            get { return new ErrorMessage(108, "All KPI Categories and Sub Categories have been set to default values."); }
        }
        public static ErrorMessage KPICategoriesNotReset
        {
            get { return new ErrorMessage(109, "KPI Categories and Sub Categories could not be reset to default values."); }
        }
        public static ErrorMessage KPISubCategoriesReset
        {
            get { return new ErrorMessage(118, "All KPI Sub Categories have been set to default values."); }
        }
        public static ErrorMessage KPISubCategoriesNotReset
        {
            get { return new ErrorMessage(119, "KPI Sub Categories could not be reset to default values."); }
        }
        public static ErrorMessage DivisionalComplexitySaved
        {
            get { return new ErrorMessage(120, "Divisional Complexity Level has been set for the selected Division."); }
        }
        public static ErrorMessage DivisionalComplexityNotSaved
        {
            get { return new ErrorMessage(123, "Divisional Complexity Level could not be set for the selected Division."); }
        }
        public static ErrorMessage IrrigationDivisionNotAvailable
        {
            get { return new ErrorMessage(124, "No Irrigation Division is Available for the Selected Circle."); }
        }
        public static ErrorMessage ExcludedChannelsNotExists
        {
            get { return new ErrorMessage(136, "There is no channel which is excluded from Performance Evaluation for given criteria."); }
        }
        public static ErrorMessage CurrentFutureDatesNotAllowed
        {
            get { return new ErrorMessage(137, "You cannot select the current or future date for Performance Evaluation."); }
        }
        public static ErrorMessage ReportNameExists
        {
            get { return new ErrorMessage(138, "‘The Evaluation Report for the given name already exists, please rename it."); }
        }
        public static ErrorMessage KPICategoryEmpty
        {
            get { return new ErrorMessage(270, "No KPI Category is included"); }
        }
        public static ErrorMessage KPISubCategoryEmpty
        {
            get { return new ErrorMessage(270, "No KPI Sub Category is included"); }
        }
        public static ErrorMessage NoReportExists
        {
            get { return new ErrorMessage(139, "No matching Report exists."); }
        }
        public static ErrorMessage ExcludedDivisionsNotExists
        {
            get { return new ErrorMessage(136, "There is no Division which is excluded from Performance Evaluation for given criteria."); }
        }
        #endregion

        #region Complaints Management
        public static ErrorMessage ComplaintsTypeExists
        {
            get { return new ErrorMessage(146, "Complaints Type already exists."); }
        }
        #endregion

        #region Seasonal Planning
        public static ErrorMessage MoreDraftsNotAllowed
        {
            get { return new ErrorMessage(147, "Not allowed to add more drafts."); }
        }

        public static ErrorMessage NotallowedToDelete
        {
            get { return new ErrorMessage(147, "Not allowed to delete this draft. Since it is linked with other draft(s)."); }
        }

        public static ErrorMessage ForecastDraftNotSelect
        {
            get { return new ErrorMessage(147, "Please select forecast draft."); }
        }

        public static ErrorMessage MCLMustBeLess
        {
            get { return new ErrorMessage(147, "Initial Storage cannot be greater than MCL storage."); }
        }

        public static ErrorMessage StrFillPercentage
        {
            get { return new ErrorMessage(147, "Early Kharif and Late Kharif Storage to fill % must be 100."); }
        }

        public static ErrorMessage FillingLimitCannotBeGreater
        {
            get { return new ErrorMessage(147, "Filling limit cannot be greater than MCL Tarbela."); }
        }

        public static ErrorMessage MaxMinScenariosNotExist
        {
            get { return new ErrorMessage(147, "First save Maximum and Minimum scenarios."); }
        }

        public static ErrorMessage DraftApproved
        {
            get { return new ErrorMessage(147, "Draft has been approved."); }
        }

        public static ErrorMessage DraftUnApproved
        {
            get { return new ErrorMessage(147, "Draft has been un-approved."); }
        }

        public static ErrorMessage StorageDepressionLimit
        {
            get { return new ErrorMessage(147, "Storage Depression cannot be greater than 100."); }
        }

        public static ErrorMessage LossGainLimit
        {
            get { return new ErrorMessage(147, "Loss and Gain value must be between 100 to -100."); }
        }

        public static ErrorMessage ReservoirLimitExceeded
        {
            get { return new ErrorMessage(147, "Please enter Reservoir level within limit."); }
        }

        public static ErrorMessage AllRimStationSelection
        {
            get { return new ErrorMessage(147, "Please select all Rim Stations."); }
        }

        #endregion

        #region Water Losses
        public static ErrorMessage DateRangeCannotBeMoreThan15Days
        {
            get { return new ErrorMessage(389, "Date range cannot be more than 31 days."); }
        }
        public static ErrorMessage DateRangeCannotBeMoreThan6Months
        {
            get { return new ErrorMessage(389, "Date range cannot be more than 6 months."); }
        }

        public static ErrorMessage SelectASubDivision
        {
            get { return new ErrorMessage(389, "Select a Sub-division."); }
        }

        public static ErrorMessage SelectADivision
        {
            get { return new ErrorMessage(389, "Select a Division."); }
        }

        public static ErrorMessage FromToDateBothRequired
        {
            get { return new ErrorMessage(389, "From and To date both are required."); }
        }

        public static ErrorMessage YearRequired
        {
            get { return new ErrorMessage(389, "Year is a reqruied field."); }
        }

        #endregion

        #region Closure Operation
        public static ErrorMessage InspectionDateCannotBeFutureDate
        {
            get { return new ErrorMessage(390, "Inspection Date cannot be greater than current date."); }
        }
        public static ErrorMessage ProgressCannotBeMoreThan100
        {
            get { return new ErrorMessage(390, "Progress Percentage cannot be greater than 100."); }
        }
        public static ErrorMessage PreviousPercentageIsGreater
        {
            get { return new ErrorMessage(390, "The entered progress percentage is less than the previous one."); }
        }
        public static ErrorMessage FromRDNotInUserJurisdiction
        {
            get { return new ErrorMessage(390, "From RD not within user jurisdiction."); }
        }
        public static ErrorMessage ToRDNotInUserJurisdiction
        {
            get { return new ErrorMessage(390, "To RD not within user jurisdiction."); }
        }
        public static ErrorMessage StartDateCannotBeGreaterThanEndDate
        {
            get { return new ErrorMessage(289, "Start date cannot be greater than End date"); }
        }
        public static ErrorMessage FromRDGreaterThanToRD
        {
            get { return new ErrorMessage(390, "From R.D cannot be greater than the To R.D."); }
        }
        public static ErrorMessage SelectSubDivOrChannel
        {
            get { return new ErrorMessage(390, "Select a sub-division or a channel."); }
        }
        public static ErrorMessage UniqueYearandDivision
        {
            get { return new ErrorMessage(390, "Closure Work plan with current Division and Year combination already exsit."); }
        }
        public static ErrorMessage UniqueLetterNoandLetterDate
        {
            get { return new ErrorMessage(390, "Closure Work plan Order Letter No and Letter Date combination already exsit."); }
        }
        public static ErrorMessage UniqueMainCanal
        {
            get { return new ErrorMessage(390, "Main Canal Name already exsit."); }
        }
        public static ErrorMessage CWP_Published
        {
            get { return new ErrorMessage(390, "This plan has successfully been published."); }
        }
        #endregion

        #region Tender Management

        public static ErrorMessage TM_OpeningDateDiff
        {
            get { return new ErrorMessage(420, "Submission date must be less than Opening date."); }
        }
        public static ErrorMessage TM_OpeningTimeDiff
        {
            get { return new ErrorMessage(420, "Submission time must be less than Opening time."); }
        }
        public static ErrorMessage TM_ContractorAlreadyExists
        {
            get { return new ErrorMessage(420, "Contractor Already Added."); }
        }
        public static ErrorMessage TM_MemberAlreadyExists
        {
            get { return new ErrorMessage(420, "Committee member Already Added."); }
        }
        public static ErrorMessage TM_EnterDCR
        {
            get { return new ErrorMessage(420, "Please Enter CDR."); }
        }
        public static ErrorMessage TM_TenderClosed
        {
            get { return new ErrorMessage(420, "Tender Closed Successfully."); }
        }

        public static ErrorMessage TM_EnterAdvertismentSource
        {
            get { return new ErrorMessage(420, "Please Enter Advertisment Source."); }
        }
        public static ErrorMessage ContactNoExists
        {
            get { return new ErrorMessage(150, "Contact No already exists."); }
        }
        #endregion

        #region Flood Operation
        public static ErrorMessage FO_GCBarrageHW_Cameras
        {
            get { return new ErrorMessage(213, "Operation Cameras must be less than Total Cameras."); }
        }
        public static ErrorMessage FO_GCBarrageHW_Manual
        {
            get { return new ErrorMessage(213, "Manual Working Gate must be less than Total Working Gate."); }
        }
        public static ErrorMessage FO_GCBarrageHW_Electrical
        {
            get { return new ErrorMessage(213, "Electrical Working Gate must be less than Total Working Gate."); }
        }
        public static ErrorMessage FO_GCBarrageHW_Electronic
        {
            get { return new ErrorMessage(213, "Electronic Working Gate must be less than Total Working Gate."); }
        }
        #endregion

        #region Rotational Program

        public static ErrorMessage MultipleGroup
        {
            get { return new ErrorMessage(214, "Now Allowed to add one Division in multiple groups."); }
        }

        public static ErrorMessage MultiplePreferences
        {
            get { return new ErrorMessage(215, "One Group/SubGroup cannot have multiple preferences."); }
        }

        public static ErrorMessage GroupAndSubGroupSelection
        {
            get { return new ErrorMessage(216, "Either Group or SubGroup should be selected in any scenario."); }
        }

        public static ErrorMessage NoChannelSelected
        {
            get { return new ErrorMessage(216, "Alteast one Channel must be selected from each Group/SubGroup."); }
        }

        public static ErrorMessage FromDateShouldBeLess
        {
            get { return new ErrorMessage(217, "From Date should be greater than last Start Date."); }
        }

        public static ErrorMessage FromDateShouldBeEqual
        {
            get { return new ErrorMessage(217, "From Date should be equal to Start Date."); }
        }

        public static ErrorMessage EndDateMustBeGreater
        {
            get { return new ErrorMessage(217, "End Date must be greater than start Date."); }
        }

        public static ErrorMessage ClosureDatesCannotBeSelected
        {
            get { return new ErrorMessage(217, "Closure Date Cannot be included."); }
        }

        public static ErrorMessage ClosureStartDateCannotBeLess
        {
            get { return new ErrorMessage(217, "Closure start date cannot be less than program start date."); }
        }

        public static ErrorMessage ClosureStartEndDate
        {
            get { return new ErrorMessage(217, "Closure start date cannot be less than Closure end date."); }
        }

        public static ErrorMessage MoreThanOneSubGroupSelection
        {
            get { return new ErrorMessage(217, "More than one subGroup must be selected."); }
        }

        public static ErrorMessage ClosureDatesSelected
        {
            get { return new ErrorMessage(217, "Closure dates has been selected."); }
        }

        public static ErrorMessage DraftsLimit
        {
            get { return new ErrorMessage(218, "Not allowed to add more plans."); }
        }

        public static ErrorMessage SubGroupSelection
        {
            get { return new ErrorMessage(218, "Select subGroups in sequence."); }
        }

        public static ErrorMessage MultipleChannelSelection
        {
            get { return new ErrorMessage(214, "Now Allowed to add one channel in multiple groups/subgroups."); }
        }

        public static ErrorMessage SkipDays
        {
            get { return new ErrorMessage(214, "Not allowed to skip days in rotational program."); }
        }

        public static ErrorMessage ApprovedDraftCannotBeDeleted
        {
            get { return new ErrorMessage(214, "Not allowed to delete Approved Draft."); }
        }

        public static ErrorMessage SendToSEForApproval
        {
            get { return new ErrorMessage(214, "Draft has been send to SE for approval."); }
        }

        public static ErrorMessage DraftSendBack
        {
            get { return new ErrorMessage(214, "Draft has been send back to XEN/SDO for changes."); }
        }

        public static ErrorMessage ChannelsAlreadyInApprovedDraft
        {
            get { return new ErrorMessage(214, "Channle(s) already used in approved draft: "); }
        }

        #endregion

        #region Entitlement and Deliveries

        public static ErrorMessage RabiShareGreater
        {
            get { return new ErrorMessage(141, "Entered Rabi Entitlement cannot be greater than its Provincial Entitlement."); }
        }
        public static ErrorMessage RecordNotSavedInPerviousDate
        {
            get { return new ErrorMessage(141, "To and From Date must be greater than from current date"); }
        }
        public static ErrorMessage FromDateMustbGreaterThan
        {
            get { return new ErrorMessage(141, "From date must b greater than from To Date."); }
        }
        public static ErrorMessage KharifShareGreater
        {
            get { return new ErrorMessage(142, "Entered Early and Late Kharif Entitlements cannot be greater than their Provincial Entitlements."); }
        }

        public static ErrorMessage EKShareGreater
        {
            get { return new ErrorMessage(242, "Entered Early Kharif Entitlement cannot be greater than its Provincial Entitlement."); }
        }

        public static ErrorMessage LKShareGreater
        {
            get { return new ErrorMessage(164, "Entered Late Kharif Entitlement cannot be greater than its Provincial Entitlement."); }
        }

        public static ErrorMessage NoDataExists
        {
            get { return new ErrorMessage(125, "Entitlement data does not exist."); }
        }

        public static ErrorMessage EntitlementAlreadySaved
        {
            get { return new ErrorMessage(125, "Entitlement for selected criteria has already been saved and approved."); }
        }

        #endregion

        #region Effluent and Water Charges
        public static ErrorMessage SelectAService
        {
            get { return new ErrorMessage(13, "Select a service type."); }
        }

        public static ErrorMessage IncompleteNTN
        {
            get { return new ErrorMessage(13, "NTN No. is incomplete."); }
        }
        public static ErrorMessage RDNotInDivision
        {
            get { return new ErrorMessage(13, "Given RD value does not lie in the selected divison."); }
        }
        public static ErrorMessage AgreementEndDateShouldBeGreaterThanInstalltionDate
        {
            get { return new ErrorMessage(13, "Agreement Signed on date should be less than Agreement End date."); }
        }
        public static ErrorMessage IndustryDuplication
        {
            get { return new ErrorMessage(13, "An Industry with same Name and type already exists."); }
        }
        public static ErrorMessage DateShouldBeGreaterThanLastValue
        {
            get { return new ErrorMessage(13, "Date  should be greater than the last entered date."); }
        }

        public static ErrorMessage ChildRecordExist
        {
            get { return new ErrorMessage(13, "Record cannot be deleted because Sanctioned Discharge Supply exist."); }
        }
        #endregion

        #region AssetsWorks
        public static ErrorMessage AssetInspectionDateCannotBeFutureDate
        {
            get { return new ErrorMessage(390, "Inspection Date cannot be greater than current date."); }
        }
        public static ErrorMessage AssetInspectionLotQuantityActiveZero
        {
            get { return new ErrorMessage(390, "Number of Active asset cannot be zero"); }
        }
        public static ErrorMessage AssetInspectionLotQuantityActive
        {
            get { return new ErrorMessage(390, "Number of Active asset cannot be greater than Quantity"); }
        }
        public static ErrorMessage AssetInspectionLotQuantityInActive
        {
            get { return new ErrorMessage(390, "Number of Inactive asset cannot be greater than Quantity"); }
        }
        public static ErrorMessage AssetInspectionIndividualConditionQuantity
        {
            get { return new ErrorMessage(390, "Number in Condition of Asset cannot be greater than Quantity"); }
        }
        public static ErrorMessage AssetInspectionSumConditionQuantity
        {
            get { return new ErrorMessage(390, "The sum of Number in Condition of Asset cannot be greater than Quantity"); }
        }
        public static ErrorMessage AtLestOneWorkItem
        {
            get { return new ErrorMessage(390, "At least one entry in Work Items is required to publish a work."); }
        }
        public static ErrorMessage DateMustLieInFinancialYear
        {
            get { return new ErrorMessage(390, "Start/To/Sanctioned Date should be in the selected financial year."); }
        }
        public static ErrorMessage PreviousProgressPercentageIsGreater
        {
            get { return new ErrorMessage(390, "The previous Physical Progress %age is greater than the entered one"); }
        }
        public static ErrorMessage PreviousFinancialPercentageIsGreater
        {
            get { return new ErrorMessage(390, "The previous Financial Progress %age is greater than the entered one"); }
        }
        public static ErrorMessage AssetWorkProgressCannotBeMoreThan100
        {
            get { return new ErrorMessage(390, "Progress Percentage cannot be greater than 100."); }
        }
        public static ErrorMessage AlreadyExistAsset
        {
            get { return new ErrorMessage(390, "Asset with selected category in selected location already exists"); }
        }
        public static ErrorMessage AssetWorks_Published
        {
            get { return new ErrorMessage(390, "This work has successfully been published."); }
        }
        public static ErrorMessage AssetWorks_unpublish
        {
            get { return new ErrorMessage(390, "This work has successfully been un-publish."); }
        }
        #endregion

        #region Auctions
        public static ErrorMessage AssetQuantityNotValid
        {
            get { return new ErrorMessage(16, "Asset Quantity is greater than available quantity"); }
        }
        public static ErrorMessage EnterAuctionCommitteMembers
        {
            get { return new ErrorMessage(16, "Enter Committe Members"); }
        }
        public static ErrorMessage BidderDuplication
        {
            get { return new ErrorMessage(16, "Bidder Already Exists"); }
        }
        public static ErrorMessage BidderAddedSuccessfully
        {
            get { return new ErrorMessage(16, "Bidder Added Successfully"); }
        }
        public static ErrorMessage EarnestMoneyNotEqual
        {
            get { return new ErrorMessage(16, "Submitted Money must be equal to Earnest Money"); }
        }
        public static ErrorMessage AwardedSuccessfully
        {
            get { return new ErrorMessage(16, "Asset Awarded Successfully"); }
        }
        public static ErrorMessage ExcessiveTokenMoney
        {
            get { return new ErrorMessage(16, "Token Money is Excessive"); }
        }
        public static ErrorMessage ExcessiveRemainingAmount
        {
            get { return new ErrorMessage(16, "Amount is Excessive"); }
        }
        public static ErrorMessage AuctionClosed
        {
            get { return new ErrorMessage(16, "Auction closed Successfully"); }
        }
        public static ErrorMessage CannotEdit
        {
            get { return new ErrorMessage(16, "Cannot edit due to dependency"); }
        }
        public static ErrorMessage NumericCharactersOnly
        {
            get { return new ErrorMessage(16, "Pleae enter numeric value"); }
        }
        #endregion

        #region Accounts

        public static ErrorMessage SpecialRateCannotBeLess
        {
            get { return new ErrorMessage(111, "Special Rate cannot be less than Ordinary Rate."); }
        }
        public static ErrorMessage OrdinaryRateLessThanPrevious
        {
            get { return new ErrorMessage(112, "Ordinary Rate cannot be less than Ordinary Rate of the previous BPS."); }
        }
        public static ErrorMessage SpecialRateLessThanPrevious
        {
            get { return new ErrorMessage(115, "Special Rate cannot be less than Special Rate of the previous BPS."); }
        }
        public static ErrorMessage PartNameExists
        {
            get { return new ErrorMessage(116, "Part Name against the selected Vehicle Type already exists."); }
        }
        public static ErrorMessage HeadNameExists
        {
            get { return new ErrorMessage(117, "Head Name already exists."); }
        }
        public static ErrorMessage ApproveBudgetShouldEqualAllocatedBudget
        {
            get { return new ErrorMessage(117, "Sum of Approved Budget should be equal to Total Allocated Budget"); }
        }
        public static ErrorMessage SumofBudgetaryProvisionshouldbetosumofNewValue
        {
            get { return new ErrorMessage(117, "Sum of Budgetary Provision should be equal to sum of New Value."); }
        }
        public static ErrorMessage RevisedTotalBudgetNotEqualNewValue
        {
            get { return new ErrorMessage(117, "Revised Total Budget should be equal to sum of New Value"); }
        }
        public static ErrorMessage ReAppropriationAmountConflict
        {
            get { return new ErrorMessage(117, "Difference of Re-Appropriation Budget Amount should be equal to zero."); }
        }
        public static ErrorMessage BudgetAmountCanNotbeZeroOrLessThanZero
        {
            get { return new ErrorMessage(117, "Budget Amount can not be zero or less than zero."); }
        }
        public static ErrorMessage txtBudgetAmountNotBEmpty
        {
            get { return new ErrorMessage(117, "Please enter amout atleast against one Object Classification"); }
        }
        public static ErrorMessage ApproveDateShouldbetweenFinancialYear
        {
            get { return new ErrorMessage(117, "Date should be from selected financial year"); }
        }
        public static ErrorMessage ReApropriateDateMustBGreaterThanLastReAppropriateDate
        {
            get { return new ErrorMessage(117, "Current Budget Date sould be greater than last budget Date"); }
        }
        public static ErrorMessage ValidTotalAmount
        {
            get { return new ErrorMessage(126, "Total amount should be greater than zero."); }
        }
        public static ErrorMessage ValidDAAmount
        {
            get { return new ErrorMessage(129, "DA amount should be greater than zero."); }
        }
        public static ErrorMessage ExpenseLimitforTenders
        {
            get { return new ErrorMessage(129, "Expense Limit for Tenders’ cannot be less than or equals to the ‘Expense Limit for Quotation."); }
        }
        public static ErrorMessage DailiesValidValue
        {
            get { return new ErrorMessage(130, "The value of dailies should be between 0 and 100."); }
        }
        public static ErrorMessage DistancesValidValue
        {
            get { return new ErrorMessage(133, "The value of distances should be between 0 and 100,000."); }
        }
        public static ErrorMessage MiscExpenseValidValue
        {
            get { return new ErrorMessage(143, "The value of misc expenses should be between 0 and 1,000,000."); }
        }
        public static ErrorMessage AmountValidValue
        {
            get { return new ErrorMessage(144, "The value of amount should be between 0 and 1,000,000."); }
        }
        public static ErrorMessage QuotationRequired
        {
            get { return new ErrorMessage(145, "At least 3 quotations are required for this much expense."); }
        }
        public static ErrorMessage ExpenseSubmitToAccountOffice
        {
            get { return new ErrorMessage(145, "Expenses of this month have been submitted to Accounts Office for sanction."); }
        }
        public static ErrorMessage AccountOfficerSubmitExpenses
        {
            get { return new ErrorMessage(145, "Expenses of this month have been submitted for sanction."); }
        }

        public static ErrorMessage FundReleaseDatePreviouslyAdded
        {
            get { return new ErrorMessage(145, "Fund Release Date cannot be less than the previously added Fund Release Date."); }
        }

        public static ErrorMessage FundReleaseTypeUnique
        {
            get { return new ErrorMessage(145, "This Fund Release Type already exists."); }
        }

        public static ErrorMessage PreviousFundReleaseDetail
        {
            get { return new ErrorMessage(145, "Please enter detail of previous release before entering new one."); }
        }

        public static ErrorMessage ReleaseDateUnique
        {
            get { return new ErrorMessage(145, "Only one Fund Release can be done for this date."); }
        }

        public static ErrorMessage CashDateCanNotBeLessThanChequeDate
        {
            get { return new ErrorMessage(145, "Cash Date cannot be less than Cheque Date."); }
        }
        public static ErrorMessage AtleastOneSanctionMustBeAssociatedWithTheCheque
        {
            get { return new ErrorMessage(145, "At least one sanction must be associated with the Cheque."); }
        }
        public static ErrorMessage CurrentExpenseValidValue
        {
            get { return new ErrorMessage(147, "The value of expense should be between 0 and 10,000,000,000."); }
        }
        public static ErrorMessage NoBudgetoryProvision
        {
            get { return new ErrorMessage(148, "Budget cannot be utilized against the Object/Classification whose budgetary provision does not exist."); }
        }
        public static ErrorMessage SumGreaterThanBudgetoryProvision
        {
            get { return new ErrorMessage(149, "Sum of Current Expense and Previous Expenses cannot be greater than Budgetary Provision."); }
        }

        public static ErrorMessage TheAllocatedQuantityShouldBeLessThanOrEqualToTheTotalQuantityOfTheAsset
        {
            get { return new ErrorMessage(149, "The allocated quantity should be less than or equal to the Total quantity of the asset."); }
        }

        #endregion

        public static ErrorMessage FilePathProblem
        {
            get { return new ErrorMessage(300, "Path/File does not exist."); }
        }
        // more messages would be added here
    }
}
