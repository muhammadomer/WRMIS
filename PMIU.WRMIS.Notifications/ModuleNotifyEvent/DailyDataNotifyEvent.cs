using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Notifications.ModuleNotifyEvent.Old
{
    public class DailyDataNotifyEvent : BaseNotifyEvent
    {
        public bool AddDailyDataNotifyEvent( long _UserID , Dictionary<string, object> _Parameters, long _EventID)
        {
            DD_GetPlaceDataDailyData_Result mdlDailyDataNotify = new AlertConfigurationBLL().GetDataForPlaceHolderDailyData();
            //long _GaugeID = Convert.ToInt64(bllCrestBedNotifyParameters.GAUGEID);
            long UserID = 0;
            if (_EventID == 7)
            {
                UserID = new AlertConfigurationBLL().GetUserIDBySubDivisionID(Convert.ToInt64(_Parameters["SubDivID"]));
            }
            //if (_EventID == 6)
            //{
            //    UserID = _UserID;
            //}
            //else
            //{
                UserID = new AlertConfigurationBLL().GetUserIDByGaugeID(Convert.ToInt64(_Parameters["GaugeID"]));
              
            //}
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetNotificationRecievers(_EventID, UserID);
            String Emails = String.Empty;
            foreach (var lstofUsers in lstNotificationRecievers)
            {
                if (lstofUsers.IsAlertByDefaultEnabled == 1)
                {
                    if (lstofUsers.Alert == true)
                    {
                        UA_AlertNotification alertNotification = PrepareAlertNotifications<DD_GetPlaceDataDailyData_Result>(lstofUsers, mdlDailyDataNotify, _UserID);
                        new AlertConfigurationBLL().AddAlertNotificationData(alertNotification);
                    }
                }
                if (lstofUsers.IsSMSByDefaultEnabled == 1)
                {
                    if (lstofUsers.SMS == true)
                    {
                        UA_SMSNotification smsNotification = PrepareSMSNotification<DD_GetPlaceDataDailyData_Result>(lstofUsers, mdlDailyDataNotify, _UserID);
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
                UA_EmailNotification emailNotification = PrepareEmailNotification<DD_GetPlaceDataDailyData_Result>(lstNotificationRecievers[0], mdlDailyDataNotify, _UserID, Emails);
                new AlertConfigurationBLL().AddEmailNotificationData(emailNotification);
            }

            return true;
        }
    }
}
