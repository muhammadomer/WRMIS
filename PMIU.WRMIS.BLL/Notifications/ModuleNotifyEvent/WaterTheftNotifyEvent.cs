using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Notifications.ModuleNotifyEvent
{
    public class WaterTheftNotifyEvent : BaseNotifyEvent
    {
        public bool AddWaterTheftNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            if (_EventID == (long)NotificationEventConstants.WaterTheft.AddBreachCase)
            {
                WT_GetBreachNotifyParameters_Result mdlWTNotifyParameters = new AlertConfigurationBLL().GetBreachNotifyParameter(Convert.ToInt64(_Parameters["BreachID"]));
                decimal ChannelOutletRDs = Convert.ToDecimal(mdlWTNotifyParameters.BreachSiteRD);
                long ChannelID = Convert.ToInt64(mdlWTNotifyParameters.ChannelID);

                object GetSBEInformation = new WaterTheftBLL().GetRelevantSBE(ChannelID, ChannelOutletRDs, _UserID);
                long UserID = 0;
                if (GetSBEInformation != null)
                {
                    UserID = Convert.ToInt64(GetSBEInformation.GetType().GetProperty("UserID").GetValue(GetSBEInformation));
                }
                else
                    UserID = _UserID; // ADM/MA Case (New Implementation RA)

                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetNotificationRecievers(_EventID, UserID);
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
                                    UA_AlertNotification alertNotification = PrepareAlertNotifications<WT_GetBreachNotifyParameters_Result>(lstofUsers, mdlWTNotifyParameters, _UserID);
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
                                    UA_SMSNotification smsNotification = PrepareSMSNotification<WT_GetBreachNotifyParameters_Result>(lstofUsers, mdlWTNotifyParameters, _UserID);
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
                    UA_EmailNotification emailNotification = PrepareEmailNotification<WT_GetBreachNotifyParameters_Result>(lstNotificationRecievers[0], mdlWTNotifyParameters, _UserID, Emails);
                    new AlertConfigurationBLL().AddEmailNotificationData(emailNotification);
                }

            }
            else
            {
                WT_GetWaterTheftNotifyDTParameters_Result mdlWTNotifyParameters = new AlertConfigurationBLL().GetWaterTheftNotify(Convert.ToInt64(_Parameters["WaterTheftID"]));
                decimal ChannelOutletRDs = mdlWTNotifyParameters.TheftSideRDs;
                long ChannelID = Convert.ToInt64(mdlWTNotifyParameters.ChannelID);

                object GetSBEInformation = new WaterTheftBLL().GetRelevantSBE(ChannelID, ChannelOutletRDs, _UserID);
                long UserID = 0;

                if (_EventID != 10 && _EventID != 15 && _EventID != 16)
                {
                    UserID = mdlWTNotifyParameters.AssignedToUserID;
                }
                else
                {
                    if (GetSBEInformation != null)
                    {
                        UserID = Convert.ToInt64(GetSBEInformation.GetType().GetProperty("UserID").GetValue(GetSBEInformation));
                    }
                }
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetNotificationRecievers(_EventID, UserID);
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
                                    UA_AlertNotification alertNotification = PrepareAlertNotifications<WT_GetWaterTheftNotifyDTParameters_Result>(lstofUsers, mdlWTNotifyParameters, _UserID);
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
                                    UA_SMSNotification smsNotification = PrepareSMSNotification<WT_GetWaterTheftNotifyDTParameters_Result>(lstofUsers, mdlWTNotifyParameters, _UserID);
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
                    UA_EmailNotification emailNotification = PrepareEmailNotification<WT_GetWaterTheftNotifyDTParameters_Result>(lstNotificationRecievers[0], mdlWTNotifyParameters, _UserID, Emails);
                    new AlertConfigurationBLL().AddEmailNotificationData(emailNotification);
                }

            }
            return true;
        }

    }
}
