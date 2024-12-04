using PMIU.WRMIS.DAL;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{
    public class NotificationsLimitDAL
    {
        ContextDB db = new ContextDB();
        //public bool UpdateNotification(UA_NotificationLimit _NotificationLimit)
        //{
        //    UA_NotificationLimit mdlNotificationLimit = db.Repository<UA_NotificationLimit>().FindById(_NotificationLimit.ID);
        //    mdlNotificationLimit.NotificationLimit = _NotificationLimit.NotificationLimit;

        //    db.Repository<UA_NotificationLimit>().Update(mdlNotificationLimit);
        //    db.Save();
        //    return true;
        //}

        //public UA_NotificationLimit GetNotificationByID(long _NotificationID)
        //{
        //    UA_NotificationLimit mdlNotificationLimit = db.Repository<UA_NotificationLimit>().FindById(_NotificationID);
        //    return mdlNotificationLimit;
        //}

        //public bool AddNotification(UA_NotificationLimit _NotificationLimit)
        //{
        //    db.Repository<UA_NotificationLimit>().Insert(_NotificationLimit);
        //    db.Save();
        //    return true;
        //}

        //public UA_NotificationLimit GetNotification()
        //{
        //    UA_NotificationLimit mdlNotificationLimit = db.Repository<UA_NotificationLimit>().GetAll().FirstOrDefault();
        //    return mdlNotificationLimit;
        //}
    }
}
