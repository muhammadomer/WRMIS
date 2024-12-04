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
    class FloodOperationsNotifyEvent : BaseNotifyEvent
    {
        public bool AddFloodOperationsNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            if (_EventID == (long)NotificationEventConstants.FloodOperations.FloodFightingPlan)
            {
                FO_GetFloodFightingPlanNotifyData_Result mdlFloodFightingPlanNotify = new FloodFightingPlanBLL().GetFloodFightingPlanNotifyData(Convert.ToInt64(_Parameters["ID"]));
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetFloodOperationsNotifyRecievers(Convert.ToInt64(_Parameters["ID"]), _EventID);
                return LogNotificationsIntoDatabase<FO_GetFloodFightingPlanNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlFloodFightingPlanNotify);
            }
            else if (_EventID == (long)NotificationEventConstants.FloodOperations.FloodInspections)
            
            {
                FO_GetFloodInspectionNotifyData_Result mdlFloodInspectionNotify = new FloodInspectionsBLL().GetFloodInspectionNotifyData(Convert.ToInt64(_Parameters["ID"]));
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetFloodOperationsNotifyRecievers(Convert.ToInt64(_Parameters["ID"]), _EventID);
                return LogNotificationsIntoDatabase<FO_GetFloodInspectionNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlFloodInspectionNotify);
            }
            else if (_EventID == (long)NotificationEventConstants.FloodOperations.EmergencyPurchase)
            {
            }

            return true;

        }
        
    }
}
