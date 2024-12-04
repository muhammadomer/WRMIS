using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Notifications.ModuleNotifyEvent
{
    public class TenderNotifyEvent : BaseNotifyEvent
    {
        public bool AddTenderNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            TM_GetTenderNotifyParameters_Result mdlTenderNotifyParameters = new AlertConfigurationBLL().GetTenderNotificationParameters(Convert.ToInt64(_Parameters["TenderNoticeID"]), Convert.ToInt64(_Parameters["TenderWorkID"]));
            List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetNotificationRecievers(_EventID, Convert.ToInt64(_Parameters["TenderNoticeID"]));
            String Emails = String.Empty;
            if (lstNotificationRecievers != null)
            {
                foreach (var lstofUsers in lstNotificationRecievers)
                {
                    if (lstofUsers.IsAlertByDefaultEnabled == 1)
                    {
                        if (lstofUsers.Alert == true)
                        {
                            try
                            {
                                UA_AlertNotification alertNotification = PrepareAlertNotifications<TM_GetTenderNotifyParameters_Result>(lstofUsers, mdlTenderNotifyParameters, _UserID);
                                new AlertConfigurationBLL().AddAlertNotificationData(alertNotification);
                            }
                            catch (Exception)
                            {


                            }
                        }
                    }
                    if (lstofUsers.IsSMSByDefaultEnabled == 1)
                    {
                        if (lstofUsers.SMS == true)
                        {
                            try
                            {
                                UA_SMSNotification smsNotification = PrepareSMSNotification<TM_GetTenderNotifyParameters_Result>(lstofUsers, mdlTenderNotifyParameters, _UserID);
                                new AlertConfigurationBLL().AddSMSNotificationData(smsNotification);

                            }
                            catch (Exception)
                            {

                            }
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
            }
            if (Emails != "")
            {
                UA_EmailNotification emailNotification = PrepareEmailNotification<TM_GetTenderNotifyParameters_Result>(lstNotificationRecievers[0], mdlTenderNotifyParameters, _UserID, Emails);
                new AlertConfigurationBLL().AddEmailNotificationData(emailNotification);
            }
            return true;
        }
    }
}
