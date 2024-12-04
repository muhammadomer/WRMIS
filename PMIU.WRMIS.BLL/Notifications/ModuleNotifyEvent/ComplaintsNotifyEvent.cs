using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Notifications.ModuleNotifyEvent
{
    public class ComplaintsNotifyEvent : BaseNotifyEvent
    {
        public bool AddComplaintNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            CM_GetComplaintsNotifyParameters_Result mdlCMNotifyParameters = new AlertConfigurationBLL().GetComplaintNotificationParameters(Convert.ToInt64(_Parameters["ComplaintID"]));
            List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetNotificationRecievers(_EventID, Convert.ToInt64(_Parameters["ComplaintID"]));
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
                                UA_AlertNotification alertNotification = PrepareAlertNotifications<CM_GetComplaintsNotifyParameters_Result>(lstofUsers, mdlCMNotifyParameters, _UserID);
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
                                UA_SMSNotification smsNotification = PrepareSMSNotification<CM_GetComplaintsNotifyParameters_Result>(lstofUsers, mdlCMNotifyParameters, _UserID);
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
                UA_EmailNotification emailNotification = PrepareEmailNotification<CM_GetComplaintsNotifyParameters_Result>(lstNotificationRecievers[0], mdlCMNotifyParameters, _UserID, Emails);
                new AlertConfigurationBLL().AddEmailNotificationData(emailNotification);
            }
            return true;
        }
        public bool AddComplaintCommentsNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            CM_GetComplaintsCommentsNotifyParameters_Result mdlCMNotifyParameters = new AlertConfigurationBLL().GetComplaintCommentsNotificationParameters(Convert.ToInt64(_Parameters["ComplaintID"]));
            List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetNotificationRecievers(_EventID, Convert.ToInt64(_Parameters["ComplaintID"]));
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
                                UA_AlertNotification alertNotification = PrepareAlertNotifications<CM_GetComplaintsCommentsNotifyParameters_Result>(lstofUsers, mdlCMNotifyParameters, _UserID);
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
                                UA_SMSNotification smsNotification = PrepareSMSNotification<CM_GetComplaintsCommentsNotifyParameters_Result>(lstofUsers, mdlCMNotifyParameters, _UserID);
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
                UA_EmailNotification emailNotification = PrepareEmailNotification<CM_GetComplaintsCommentsNotifyParameters_Result>(lstNotificationRecievers[0], mdlCMNotifyParameters, _UserID, Emails);
                new AlertConfigurationBLL().AddEmailNotificationData(emailNotification);
            }
            return true;
        }
    }
}
