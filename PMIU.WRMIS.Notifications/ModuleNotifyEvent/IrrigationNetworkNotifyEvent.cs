using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Notifications.ModuleNotifyEvent.Old
{
    public class IrrigationNetworkNotifyEvent : BaseNotifyEvent
    {
        public bool AddIrrigationNetworkNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            UA_GetCrestBedLevelNotifyDTParameters_Result bllCrestBedNotifyParameters = new AlertConfigurationBLL().GetCrestBedLevelNotifyDTParameters(Convert.ToInt64(_Parameters["GaugeID"]), Convert.ToBoolean(_Parameters["IsCrestParameters"]));
            long _GaugeID = Convert.ToInt64(bllCrestBedNotifyParameters.GAUGEID);
            long UserID = new AlertConfigurationBLL().GetUserIDByGaugeID(_GaugeID);
            List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetNotificationRecievers(_EventID, UserID);
            String Emails = String.Empty;
            foreach (var lstofUsers in lstNotificationRecievers)
            {
                if (lstofUsers.IsAlertByDefaultEnabled == 1)
                {
                    if (lstofUsers.Alert == true)
                    {
                        UA_AlertNotification alertNotification = PrepareAlertNotifications<UA_GetCrestBedLevelNotifyDTParameters_Result>(lstofUsers, bllCrestBedNotifyParameters, _UserID);
                        new AlertConfigurationBLL().AddAlertNotificationData(alertNotification);
                    }
                }
                if (lstofUsers.IsSMSByDefaultEnabled == 1)
                {
                    if (lstofUsers.SMS == true)
                    {
                        UA_SMSNotification smsNotification = PrepareSMSNotification<UA_GetCrestBedLevelNotifyDTParameters_Result>(lstofUsers, bllCrestBedNotifyParameters, _UserID);
                        new AlertConfigurationBLL().AddSMSNotificationData(smsNotification);
                    }
                }

                if (lstofUsers.IsEmailByDefaultEnabled == 1)
                {
                    if (lstofUsers.Email == true)
                    {
                        Emails += lstofUsers.UserEmail + ";";
                    }
                }

            }
            if (Emails != "")
            {
                UA_EmailNotification emailNotification = PrepareEmailNotification<UA_GetCrestBedLevelNotifyDTParameters_Result>(lstNotificationRecievers[0], bllCrestBedNotifyParameters, _UserID, Emails);
                new AlertConfigurationBLL().AddEmailNotificationData(emailNotification);
            }

            return true;
        }
    }
}
