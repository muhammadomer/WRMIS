using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Common
{
    public class Configuration
    {
        public const string IrrigationNetwork = "IrrigationNetwork";
        public const string DailyData = "DailyData";
        public const string WaterTheft = "WaterTheft";
        public const string ScheduleInspection = "ScheduleInspection";
        public const string TenderManagement = "TenderManagement";
        public const string Complaints = "Complaints";
        public const string FloodOperations = "FloodOperations";
        public const string ClosureOperations = "ClosureOperations";
        public const string RotationalProgram = "RotationalProgram";
        public const string Auctions = "Auctions";
        public const string AssetsWorks = "AssetsWorks";
        public const string EffluentWaterCharges = "EffluentWaterCharges";
        public const string Accounts = "Accounts";
        public const string VehicleReadings = "VehicleReadings";
        public const string EmployeeTracking = "EmployeeTracking";

        public class RequestSource
        {
            public const string RequestFromWeb = "W";
            public const string RequestFromMobile = "M";
        }
        public class Complaint
        {
            public const string Irrigation = "IRRIGATION";
            public const string dandf = "D & F";
            public const string SmallDamsDivision = "SMALL DAMS DIVISIONS";
            public const string Development = "DEVELOPMENT";
            public const string Channel = "CHANNEL";
            public const string Outlet = "OUTLET";
            public const string ProtectionStructure = "PROTECTION INFRASTRUCTURE";
            public const string BarrageHeadwork = "BARRAGE/HEADWORK";
            public const string Drain = "DRAIN";
            public const string SmallDam = "SMALL DAM";
            public const string ComplaintID = "ComplaintID";
        }

    }
}
