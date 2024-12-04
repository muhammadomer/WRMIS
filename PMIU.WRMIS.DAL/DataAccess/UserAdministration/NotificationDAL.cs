using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{
    public class NotificationDAL
    {
        ContextDB db = new ContextDB();
        public NotificationDAL()
        {

        }

        public void NotifyEvent(int _EventID, int _UserID)
        {
            

            UA_Users mdlUser = db.Repository<UA_Users>().GetAll().FirstOrDefault(e => e.ID == _UserID);

            UA_NotificationEvents mdl_NotificationEvent = db.Repository<UA_NotificationEvents>().GetAll().FirstOrDefault(e => e.ID == _EventID);


            UA_NotificationConfiguration mdl_NC = db.Repository<UA_NotificationConfiguration>().GetAll().FirstOrDefault(e => e.DesignationID == mdlUser.DesignationID && e.NotificationEventID == _EventID);
            UA_UserNotificationConfiguration mdl_UNC = db.Repository<UA_UserNotificationConfiguration>().GetAll().FirstOrDefault(e => e.UserID == _UserID && e.NotificationEventID == _EventID);

            bool IsEmail = false;
            bool IsSMS = false;
            bool IsAlert = false;

            if (mdl_UNC != null)
            {
                //if (mdl_UNC.Email.Value)
                //{
                    
                //}

                IsEmail = mdl_UNC.Email.Value;
                IsSMS = mdl_UNC.SMS.Value;
                IsAlert = mdl_UNC.Alert.Value;

            }
            else
            {
                IsEmail = mdl_NC.Email.Value;
                IsSMS = mdl_NC.SMS.Value;
                IsAlert = mdl_NC.Alert.Value;
            }

            if (IsEmail)
            {

                string subject = mdl_NotificationEvent.EmailTemplateSubject;
                string body = mdl_NotificationEvent.EmailTemplateBody;
            }
        }
    }
}
