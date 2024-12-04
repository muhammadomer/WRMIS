using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Notifications.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            //int abc = 543;
            //string s = abc.ToString("000");

            //s += "";
            
            //SendEmails();

            //PMIU.WRMIS.Common.Utility.SendEmail(new System.Net.Mail.MailAddress("safiullah.bhatti@gmail.com"), new System.Net.Mail.MailAddress("safiullahbhatti@outlook.com"), "Subject Testing", "Body Testing");

            //new PMIU.WRMIS.BLL.UserAdministration.UserAdministrationBLL().GetAllHierarchies(150014);

            //System.Threading.Thread thrEmails = new System.Threading.Thread(SendEmails);
            //System.Threading.Thread thrSMS = new System.Threading.Thread(SendSMS);

            //thrEmails.Start();
            //thrSMS.Start();
            //////////Console.WriteLine("----------------- Press Any Key to Start  ----------------------");

            //////////Console.ReadKey();

            //////////Console.WriteLine("----------------- Process Started ----------------------");

            //////////SendSMS();
            
            //////////SendEmails();

            ////////////Task.Factory.StartNew(() => SendEmails());
            ////////////Task.Factory.StartNew(() => SendSMS());

            //////////Console.WriteLine("----------------- Press Any Key to Exit ----------------------");
            //////////Console.ReadKey();





            //Schedule(() => Console.WriteLine("Timed Task - Will run every day at 9:15pm: " + DateTime.Now)).ToRunEvery(1).Days().At(21, 15);


    //        SchedulerResponse response = WindowTaskScheduler
    //.Configure()
    //.CreateTask("TaskName", "C:\\Test.bat")
    //.RunDaily()
    //.RunEveryXMinutes(10)   
    //.RunDurationFor(new TimeSpan(18, 0, 0))
    //.SetStartDate(new DateTime(2015, 8, 8))
    //.SetStartTime(new TimeSpan(8, 0, 0))
    //.Execute();


        }


        
        public static void SendEmails()
        {
            Console.WriteLine("--- SENDING EMAILS ---");
            NotificationsBLL bllNotification = new NotificationsBLL();
            List<UA_EmailNotification> lstEmail = bllNotification.GetPendingEmails();
            Console.WriteLine("Pending Emails Found: " + lstEmail.Count);
            foreach (UA_EmailNotification mdlEmail in lstEmail)
            {
                try
                {
                    Console.WriteLine("Sending Email To : " + mdlEmail.EmailTo);
                    PMIU.WRMIS.Common.Utility.SendEmail(mdlEmail);
                }
                catch(Exception exp)
                {
                    Console.WriteLine("ERROR: Sending Email Failed To : " + mdlEmail.EmailTo);
                }
            }
            Console.WriteLine("Sending Email Status Back");
            bllNotification.MarkEmailsSent(lstEmail);
            Console.WriteLine("------ Email Status Back Done ------");
        }

        public static void SendSMS()
        {
            Console.WriteLine("--- SENDING SMS ---");
            NotificationsBLL bllNotification = new NotificationsBLL();
            List<UA_SMSNotification> lstSMS = bllNotification.GetPendingSMS();
            Console.WriteLine("Pending SMS Found: " + lstSMS.Count);
            foreach (UA_SMSNotification mdlSMS in lstSMS)
            {
                try 
                {
                    Console.WriteLine("Sending SMS To : " + mdlSMS.MobileNumber);
                    PMIU.WRMIS.Common.Utility.SendSMS(mdlSMS);
                }
                catch(Exception exp)
                {
                    Console.WriteLine("ERROR: Sending SMS Failed To : " + mdlSMS.MobileNumber);
                }
            }
            Console.WriteLine("Sending SMS Status Back");
            bllNotification.MarkSMSSent(lstSMS);
            Console.WriteLine("------ SMS Status Back Done ------");
           
        }


    }
}
