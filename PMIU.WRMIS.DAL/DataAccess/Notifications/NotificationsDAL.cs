using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.Notifications
{
    public class NotificationsDAL
    {
        ContextDB db = new ContextDB();
        public NotificationsDAL()
        {

        }

        public List<UA_EmailNotification> GetPendingEmails()
        {
            List<UA_EmailNotification> lstEmailNotifications = db.Repository<UA_EmailNotification>().GetAll().Where(e => e.Status == 0 && e.TryCount <= 3).ToList<UA_EmailNotification>();
            return lstEmailNotifications;
        }

        public List<UA_SMSNotification> GetPendingSMS()
        {
            List<UA_SMSNotification> lstSMSNotifications = db.Repository<UA_SMSNotification>().GetAll().Where(e => e.Status == 0 && e.TryCount <= 3).ToList<UA_SMSNotification>();
            return lstSMSNotifications;
        }

        public bool MarkEmailsSent(List<long> _lstIDs, bool _Sent)
        {
            try
            {
                //List<UA_EmailNotification> lstEmails = db.Repository<UA_EmailNotification>().GetAll().Where(e => _lstIDs.Contains(e.ID)).ToList<UA_EmailNotification>();
                //if (_Sent)
                //    lstEmails.ForEach(e => e.Status = 1);
                //else
                //    lstEmails.ForEach(e => e.Status = 2);

                List<UA_EmailNotification> lstEmails = new List<UA_EmailNotification>();


                lstEmails.ForEach(e => e.TryCount = ++e.TryCount);
                lstEmails.ForEach(e => e.ServerMessage = "");
                db.Save();

                return true;
            }
            catch(Exception exp)
            {
                return false;
            }
        }

        public bool MarkEmailsSent(List<UA_EmailNotification> lstEmails)
        {
            try
            {
                List<long> EmailIDs = new List<long>();
                foreach (UA_EmailNotification mdlEmail in lstEmails)
                    EmailIDs.Add(mdlEmail.ID);

                List<UA_EmailNotification> lstNewEmails = db.Repository<UA_EmailNotification>().GetAll().Where(e => EmailIDs.Contains(e.ID)).ToList<UA_EmailNotification>();


                lstNewEmails.ForEach(e => e.ServerMessage = lstEmails.FirstOrDefault(d => d.ID == e.ID).ServerMessage);
                lstNewEmails.ForEach(e => e.TryCount = lstEmails.FirstOrDefault(d => d.ID == e.ID).TryCount);
                lstNewEmails.ForEach(e => e.Status = lstEmails.FirstOrDefault(d => d.ID == e.ID).Status);

                db.Save();

                //db.ExtRepositoryFor<TokenRepository>().UpdateEmails(lstEmails);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }



        public bool MarkSMSSent(List<UA_SMSNotification> _lstSMS)
        {
            try
            {
                List<long> SMSIDs = new List<long>();
                foreach (UA_SMSNotification mdlSMS in _lstSMS)
                    SMSIDs.Add(mdlSMS.ID);

                List<UA_SMSNotification> lstNewSMS = db.Repository<UA_SMSNotification>().GetAll().Where(e => SMSIDs.Contains(e.ID)).ToList<UA_SMSNotification>();

                lstNewSMS.ForEach(e => e.ServerMessage = _lstSMS.FirstOrDefault(d => d.ID == e.ID).ServerMessage);
                lstNewSMS.ForEach(e => e.TryCount = _lstSMS.FirstOrDefault(d => d.ID == e.ID).TryCount);
                lstNewSMS.ForEach(e => e.Status = _lstSMS.FirstOrDefault(d => d.ID == e.ID).Status);
                lstNewSMS.ForEach(e => e.SMSSentDate = _lstSMS.FirstOrDefault(d => d.ID == e.ID).SMSSentDate);

                db.Save();

                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        

        
    }
}
