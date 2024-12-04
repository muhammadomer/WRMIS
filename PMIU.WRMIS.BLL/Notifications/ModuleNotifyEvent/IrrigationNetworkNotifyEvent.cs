using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Notifications.ModuleNotifyEvent
{
    public class IrrigationNetworkNotifyEvent : BaseNotifyEvent
    {
        public bool AddIrrigationNetworkNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            UA_GetCrestBedLevelNotifyDTParameters_Result mdlIrrigationNetworkNotify = new AlertConfigurationBLL().GetCrestBedLevelNotifyDTParameters(Convert.ToInt64(_Parameters["GaugeID"]), Convert.ToBoolean(_Parameters["IsCrestParameters"]));

            List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetIrrigationNetworkNotificationRecievers(_EventID, Convert.ToInt64(_Parameters["GaugeID"]));
            return LogNotificationsIntoDatabase<UA_GetCrestBedLevelNotifyDTParameters_Result>(_EventID, _UserID, lstNotificationRecievers, mdlIrrigationNetworkNotify);
        }
    }
}
