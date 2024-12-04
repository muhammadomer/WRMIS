using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Notifications.ModuleNotifyEvent.Old
{
    class ScheduleInspectionNotifyEvent : BaseNotifyEvent
    {
        public bool AddScheduleInspectionNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            SI_GetScheduleInspectionNotifyData_Result mdlScheduleInspectionNotifyData = new ScheduleInspectionBLL().GetScheduleInspectionNotifyData(Convert.ToInt64(_Parameters["ScheduleID"]));

            List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetScheduleInspectionNotifyReciever(Convert.ToInt64(_Parameters["ScheduleID"]), _EventID);

            return LogNotificationsIntoDatabase<SI_GetScheduleInspectionNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlScheduleInspectionNotifyData);
        }
    }
}
