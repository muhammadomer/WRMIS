using PMIU.WRMIS.BLL.DailyData;
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
    public class DailyDataNotifyEvent : BaseNotifyEvent
    {
        public bool AddDailyDataNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            if (_EventID == (long)NotificationEventConstants.DailyData.ChangeIndent)

            {
                DD_GetDailyIndentNotifyData_Result mdlDailyDataNotify = new DailyDataBLL().GetDailyIndentNotifyData(Convert.ToInt64(_Parameters["GaugeID"]));
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new DailyDataBLL().GetDailyIndentNotifyRecievers(_EventID, _UserID);
                return LogNotificationsIntoDatabase<DD_GetDailyIndentNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlDailyDataNotify);
            }
            else
            {
                DD_GetDailyGaugeReadingNotifyData_Result mdlDailyDataNotify = new DailyDataBLL().GetDailyGaugeReadingNotifyData(Convert.ToInt64(_Parameters["DailyGaugeReadingID"]));
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new DailyDataBLL().GetDailyDataNotifyRecievers(_EventID, Convert.ToInt64(_Parameters["DailyGaugeReadingID"]));
                return LogNotificationsIntoDatabase<DD_GetDailyGaugeReadingNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlDailyDataNotify);
            }


        }




    }
}
