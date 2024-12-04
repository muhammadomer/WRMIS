using System;
using System.Web;
namespace PMIU.WRMIS.Common
{
    public class SessionValues
    {
        private static long _UserID;
        public static string UserID
        {
            get
            {
                return "UserID";
            }
        }

        public static long LoggedInUserID
        {
            get
            {
                try
                {
                    return (long)HttpContext.Current.Session[SessionValues.UserID];
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session[SessionValues.UserID] = (long)value;
            }
        }
        public static string SearchUserCriteria
        {
            get
            {
                return "SearchUserCriteria";
            }
        }

        public static string RoleRightList
        {
            get
            {
                return "RoleRightList";
            }
        }

        public static string SearchIndentCriteria
        {
            get
            {
                return "SearchIndentCriteria";
            }
        }

        public static string CommandID
        {
            get
            {
                return "CommandID";
            }
        }

        public static string ChanneltypeID
        {
            get
            {
                return "ChanneltypeID";
            }
        }

        public static string FlowTypeID
        {
            get
            {
                return "FlowTypeID";
            }
        }

        public static string ChannelNameID
        {
            get
            {
                return "ChannelNameID";
            }
        }

        public static string ChannelID
        {
            get
            {
                return "ChannelID";
            }
        }

        public static string SearchWaterTheftCriteria
        {
            get
            {
                return "SearchWaterTheftCriteria";
            }
        }

        public static string IrrigatorFeedbackHistory
        {
            get
            {
                return "IrrigatorFeedbackHistory";
            }
        }

        public static string SearchIrrigator
        {
            get
            {
                return "SearchIrrigator";
            }
        }

        public static string SearchIrrigatorFeedback
        {
            get
            {
                return "SearchIrrigatorFeedback";
            }
        }

        public static string SearchBreachCriteria
        {
            get
            {
                return "SearchBreachCriteria";
            }
        }

        public static string PlacingIndents
        {
            get
            {
                return "PlacingIndents";
            }
        }

        public static string ReportData
        {
            get
            {
                return "ReportData";
            }
        }

        public static string ViewIndents
        {
            get
            {
                return "ViewIndents";
            }
        }

        public static string DailyIndents
        {
            get
            {
                return "DailyIndents";
            }
        }

        public static string SearchComplaintCriteria
        {
            get
            {
                return "SearchComplaintCriteria";
            }
        }

        public static string SearchFloodInspection
        {
            get
            {
                return "SearchFloodInspection";
            }
        }

        public static string SearchPEReportCriteria
        {
            get
            {
                return "SearchPEReportCriteria";
            }
        }

        public static string SearchEmergencyPurchase
        {
            get
            {
                return "SearchEmergencyPurchase";
            }
        }

        public static string SearchTenderNotice
        {
            get
            {
                return "SearchTenderNotice";
            }
        }


        public static string SearchFFP
        {
            get
            {
                return "SearchFFP";
            }
        }

        public static string SearchStoneDeployment
        {
            get
            {
                return "SearchStoneDeployment";
            }
        }

        public static string SearchDivisionStore
        {
            get
            {
                return "SearchDivisionStore";
            }
        }

        public static string SearchDepInspection
        {
            get
            {
                return "SearchDepInspection";
            }
        }

        public static string SearchJoinInspection
        {
            get
            {
                return "SearchJoinInspection";
            }
        }
        public static string SearchFloodBundRef
        {
            get
            {
                return "SearchFloodBundRef";
            }
        }

        public static string SearchOnsiteMonitoring
        {
            get { return "SearchOnsiteMonitoring"; }
        }
        public static string SearchIssueStockInfrastructure
        {
            get { return "SearchIssueStockInfrastructure"; }
        }
        public static string SearchIssueStockCampSite
        {
            get { return "SearchIssueStockCampSite"; }
        }
        public static string SearchPurchaseFloodFightingPlan
        {
            get { return "SearchPurchaseFloodFightingPlan"; }
        }
        public static string SearchReturnPurchaseDuringFlood
        {
            get { return "SearchReturnPurchaseDuringFlood"; }
        }
        public static string SearchReturnInfrastructure
        {
            get { return "SearchReturnInfrastructure"; }
        }
        public static string SearchReturnCampSite
        {
            get { return "SearchReturnCampSite"; }
        }
        public static string SearchAssets
        {
            get
            {
                return "SearchAssets";
            }
        }

        public static string IsSwitchUser
        {
            get { return "IsShwitchUser"; }
        }

        public static string OriginalUserID
        {
            get { return "OriginalUserID"; }
        }

        public static string SearchAuction
        {
            get { return "SearchAuction"; }
        }
        public static string SearchFloodBundGauges
        {
            get
            {
                return "SearchFloodBundGauges";
            }
        }

        public static string FundRelease
        {
            get
            {
                return "FundRelease";
            }
        }

        public static string ResourceAllocation
        {
            get
            {
                return "ResourceAllocation";
            }
        }

        public static string SearchChequeCriteria
        {
            get
            {
                return "SearchChequeCriteria";
            }
        }
        public static string SearchMeterReadingAndFuel
        {
            get
            {
                return "SearchMeterReadingAndFuel";
            }
        }
    }
}