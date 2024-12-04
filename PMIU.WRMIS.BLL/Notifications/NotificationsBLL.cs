using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.DataAccess.Notifications;

namespace PMIU.WRMIS.BLL.Notifications
{
    public class NotificationsBLL
    {
        public NotificationsBLL()
        {

        }
        public List<UA_EmailNotification> GetPendingEmails()
        {

            NotificationsDAL dalNotifications = new NotificationsDAL();
            return dalNotifications.GetPendingEmails();
        }
        public List<UA_SMSNotification> GetPendingSMS()
        {
            NotificationsDAL dalNotifications = new NotificationsDAL();
            return dalNotifications.GetPendingSMS();
        }
        public bool MarkEmailsSent(List<UA_EmailNotification> lstEmails)
        {
            NotificationsDAL dalNotifications = new NotificationsDAL();
            return dalNotifications.MarkEmailsSent(lstEmails);
        }
        public bool MarkSMSSent(List<UA_SMSNotification> _lstSMS)
        {
            NotificationsDAL dalNotifications = new NotificationsDAL();
            return dalNotifications.MarkSMSSent(_lstSMS);
        }
    }
}
