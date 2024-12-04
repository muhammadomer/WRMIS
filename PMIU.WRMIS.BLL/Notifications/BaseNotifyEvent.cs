using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Notifications
{
    public class BaseNotifyEvent
    {
        protected bool LogNotificationsIntoDatabase<T>(long _EventID, long _UserID, List<UA_GetNotificationsRecievers_Result> _lstNotificationRecievers, T _Obj)
        {
            String Emails = String.Empty;
            if (_lstNotificationRecievers != null)
            {
                foreach (var lstofUsers in _lstNotificationRecievers)
                {
                    if (lstofUsers.IsAlertByDefaultEnabled == 1)
                    {
                        if (lstofUsers.Alert == true)
                        {
                            try
                            {
                                UA_AlertNotification alertNotification = PrepareAlertNotifications<T>(lstofUsers, _Obj, _UserID);
                                new AlertConfigurationBLL().AddAlertNotificationData(alertNotification);
                            }
                            catch (Exception)
                            { }
                        }
                    }
                    if (lstofUsers.IsSMSByDefaultEnabled == 1)
                    {
                        if (lstofUsers.SMS == true)
                        {
                            try
                            {
                                UA_SMSNotification smsNotification = PrepareSMSNotification<T>(lstofUsers, _Obj, _UserID);
                                new AlertConfigurationBLL().AddSMSNotificationData(smsNotification);
                            }
                            catch (Exception)
                            { }
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
                UA_EmailNotification emailNotification = PrepareEmailNotification<T>(_lstNotificationRecievers[0], _Obj, _UserID, Emails);
                new AlertConfigurationBLL().AddEmailNotificationData(emailNotification);
            }
            return true;
        }
        public UA_AlertNotification PrepareAlertNotifications<T>(UA_GetNotificationsRecievers_Result _Event, T _Obj, long _UserID)
        {
            UA_AlertNotification mdlAlertNotification = new UA_AlertNotification();
            mdlAlertNotification.NotificationEventID = _Event.EventID;
            mdlAlertNotification.UserID = _Event.UserID;
            mdlAlertNotification.AlertText = FillPlaceHoldersWithAlertSMSEmailValues<T>(_Event.AlertTemplate, _Obj);
            mdlAlertNotification.AlertURL = FillPlaceHoldersWithAlertSMSEmailValues<T>(_Event.URLQueryString, _Obj);
            mdlAlertNotification.Status = 0;
            mdlAlertNotification.CreatedDate = DateTime.Now;
            mdlAlertNotification.CreatedBy = _UserID;
            return mdlAlertNotification;
        }
        public UA_SMSNotification PrepareSMSNotification<T>(UA_GetNotificationsRecievers_Result _Event, T _Obj, long _UserID)
        {
            UA_SMSNotification mdlSMSNotification = new UA_SMSNotification();
            mdlSMSNotification.NotificationEventID = _Event.EventID;
            mdlSMSNotification.UserID = _Event.UserID;
            mdlSMSNotification.SMSText = FillPlaceHoldersWithAlertSMSEmailValues<T>(_Event.SMSTemplate, _Obj);
            mdlSMSNotification.MobileNumber = _Event.MobilePhone;
            mdlSMSNotification.Status = 0;
            mdlSMSNotification.TryCount = 0;
            mdlSMSNotification.CreatedBy = _UserID;
            mdlSMSNotification.CreatedDate = DateTime.Now;
            return mdlSMSNotification;

        }
        public UA_EmailNotification PrepareEmailNotification<T>(UA_GetNotificationsRecievers_Result _Event, T _Obj, long _UserID, string emails)
        {
            UA_EmailNotification mdlEmailNotification = new UA_EmailNotification();
            mdlEmailNotification.NotificationEventID = _Event.EventID;
            mdlEmailNotification.EmailTo = emails;
            mdlEmailNotification.EmailCC = _Event.CCEmail;
            mdlEmailNotification.EmailSubject = FillPlaceHoldersWithAlertSMSEmailValues<T>(_Event.EmailTemplateSubject, _Obj);
            mdlEmailNotification.EmailBody = FillPlaceHoldersWithAlertSMSEmailValues<T>(_Event.EmailTemplateBody, _Obj);
            mdlEmailNotification.Status = 0;
            mdlEmailNotification.TryCount = 0;
            mdlEmailNotification.CreatedBy = _UserID;
            mdlEmailNotification.CreatedDate = DateTime.Now;
            return mdlEmailNotification;

        }

        /// <summary>
        /// This function fill Alert, SMS, Email place holders with actual values
        /// Created on: 15-08-2016
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_Template"></param>
        /// <param name="_Obj"></param>
        /// <returns>string</returns>
        public string FillPlaceHoldersWithAlertSMSEmailValues<T>(string _Template, T _Obj)
        {
            foreach (var item in typeof(T).GetProperties())
            {
                if (_Template != null && _Template.Contains(item.Name))
                    _Template = _Template.Replace("$$" + item.Name + "$$", Convert.ToString(item.GetValue(_Obj)));
            }
            return _Template;
        }


    }
}
