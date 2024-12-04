using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Notifications.ModuleNotifyEvent
{
    class AuctionsNotifyEvent : BaseNotifyEvent
    {

        public bool AddAuctionsNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            if (_EventID == (long)NotificationEventConstants.Auctions.PendingForApprovalWhenBalanceZero)
            {
                AC_GetPendingApprovalNotifyData_Result mdlPendingApprovalNotify = new AuctionBLL().GetPendingApprovalNotifyData(Convert.ToInt64(_Parameters["AuctionNoticeID"]), Convert.ToInt64(_Parameters["AuctionAssetsID"]), Convert.ToInt64(_Parameters["AuctionPriceID"]));
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetAuctionNotifyRecievers(Convert.ToInt64(_Parameters["AuctionNoticeID"]), _EventID);
                return LogNotificationsIntoDatabase<AC_GetPendingApprovalNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlPendingApprovalNotify);
            }
            else if (_EventID == (long)NotificationEventConstants.Auctions.AuctionApproved)
            {
                AC_GetApprovedCanceledNotifyData_Result mdlFloodInspectionNotify = new AuctionBLL().GetApprovedCanceledNotifyData(Convert.ToInt64(_Parameters["AuctionNoticeID"]), Convert.ToInt64(_Parameters["AuctionAssetsID"]), Convert.ToInt64(_Parameters["AuctionPriceID"]), _UserID);
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetAuctionNotifyRecievers(Convert.ToInt64(_Parameters["AuctionNoticeID"]), _EventID);
                return LogNotificationsIntoDatabase<AC_GetApprovedCanceledNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlFloodInspectionNotify);
            }
            else if (_EventID == (long)NotificationEventConstants.Auctions.AuctionCancel)
            {
                AC_GetApprovedCanceledNotifyData_Result mdlFloodInspectionNotify = new AuctionBLL().GetApprovedCanceledNotifyData(Convert.ToInt64(_Parameters["AuctionNoticeID"]), Convert.ToInt64(_Parameters["AuctionAssetsID"]), Convert.ToInt64(_Parameters["AuctionPriceID"]), _UserID);
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetAuctionNotifyRecievers(Convert.ToInt64(_Parameters["AuctionNoticeID"]), _EventID);
                return LogNotificationsIntoDatabase<AC_GetApprovedCanceledNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlFloodInspectionNotify);
            }
            return true;

        }

    }
}
