using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Common
{
    public class NotificationEventConstants
    {
        public enum IrrigationNetwork
        {
            EditBedLevelParameters = 1, // Edit Discharge Parameters For Gauge at Bed Level
            EditCrestLevelParameters = 2 // Edit Discharge Parameters For Gauge at Crest Level
        }

        public enum WaterTheft
        {
            AddWaterTheftCase = 10, // Water theft incident is logged into the system
            ZiladarAssignToSDO = 13, //Ziladar assigns the case to SDO
            SBEAssignedToSDO = 11, //SBE assigns the case to SDO
            SDOAssignedTOZiladaar = 12, //SDO assigns the case to Ziladaar
            SDOAssignedToXEN = 14, //SDO assigns the case to XEN
            XENFinalizeCase = 15, // XEN finalizes the case
            SDOAssignedBackSBE = 16, // SDO assigns the case back to SBE
            SDOAssignedBackZiladdar = 17,  //SDO assigns the case back to Ziladaar
            XENAssignedBackToSDO = 18, // XEN assigns the case back to SDO
            ChieftAssignedBackXEN = 19, //Chief/SE assigns the case back to XEN
            AddBreachCase = 53 //Breach case is logged into the system
        }

        public enum DailyData
        {
            ChangeDataFrequencyForBarrage = 3,    //Change Data Frequency for Barrage
            AddDailyDate = 4,                        //Add Daily Data (Morning & Evening data)
            EditDailyData = 5,                      //Edit Daily Data (Morning & Evening data)
            AddDailyHourlyDataForBarrage = 6,       //Add Daily, Hourly, 3 hourly, 6 hourly data for Barrage
            ChangeIndent = 7,                                     //Change Indent

        }

        public enum Complaints
        {
            Complaintisloggedintothesystem = 20,
            XENAssignsthecomplainttoADM = 21,
            ADMclosesthecomplaint = 22,
            ADMassignsbacktoXEN = 23,
            AcommentisaddedbyAdditionalAccessibilityuser = 24
        }
        public enum ScheduleInspection
        {
            ScheduleOfMAIsAssignedToADMForApproval = 25,
            ScheduleOfMAIsApprovedByADM = 26,
            ScheduleOfMAIsRejectedByADM = 27,
            ScheduleOfMAIsSendBackForReworkByADM = 28,
            ScheduleOfSDOIsAssignedToXENForApproval = 29,
            ScheduleOfSDOIsApprovedByXEN = 30,
            ScheduleOfSDOIsRejectedByXEN = 31,
            ScheduleOfSDOIsSendBackForReworkByXEN = 32,
            ScheduleOfXENIsAssignedToSEForApproval = 33,
            ScheduleOfXENIsApprovedBySE = 34,
            ScheduleOfXENIsRejectedBySE = 35,
            ScheduleOfXENIsSendBackForReworkBySE = 36,
            ScheduleOfADMIsAssignedToDDForApproval = 37,
            ScheduleOfADMIsApprovedByDD = 38,
            ScheduleOfADMIsRejectedByDD = 39,
            ScheduleOfADMIsSendBackForReworkByDD = 40,
            ScheduleOfMAIsAssignedToDDForApproval = 41,
            ScheduleOfMAIsApprovedByDD = 42,
            ScheduleOfMAIsRejectedByDD = 43,
            ScheduleOfMAIsSendBackForReworkByDD = 44,
            InspectionOfADMAssignedToDDForApproval = 61,
            InspectionOfADMIsApprovedByDD = 62,
            InspectionOfADMCrestLevelAssignedToDDForApproval = 63,
            InspectionOfADMCrestLevelIsApprovedByDD = 64,
            UnscheduleInspectionOfADMAssignedToDDForApproval = 65,
            UnscheduleInspectionOfADMCrestLevelAssignedToDDForApproval = 66,
            UnscheduleInspectionOfADMisapprovedbyDD = 67,
            UnscheduleInspectionOfADMCLisapprovedbyDD = 68
        }
        public enum ClosureOperations
        {
            PublishClosureWorkPlan = 45,
            ClosureWorkProgress = 46
        }

        public enum TenderMgmt
        {
            AddTenderNotice = 47,
            TenderWorkOpening = 48,
            AddComparativeStatement = 49,
            ContractorisAwarded = 55
        }

        public enum FloodOperations
        {
            FloodFightingPlan = 50,
            FloodInspections=51,
            EmergencyPurchase = 52
        }
        public enum AssetWorks
        {
            PublishAssetWorkPlan = 56,
            AssetWorkProgress = 54,
            AssociateAssetWork = 57
        }

        public enum Auctions
        {
            PendingForApprovalWhenBalanceZero = 58,
            AuctionApproved = 59,
            AuctionCancel = 60
        }

    }
}
