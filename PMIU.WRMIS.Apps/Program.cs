using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.BLL.PerformanceEvaluation;
using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Logging;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Apps
{
    class Program
    {
        const string PARAM_Notification = "N";
        const string PARAM_PerformanceEvaluation = "P";
        const string PARAM_Tender = "T";
        static void Main(string[] args)
        {
            LogMessage.LogMessageNow(2, "============= Starting - Apps =================", Constants.MessageType.Info, Constants.MessageCategory.Apps);

            if (args.Length <= 0)
            {
                LogMessage.LogMessageNow(2, "No Parameter Found - Shutting Down", Constants.MessageType.Warning, Constants.MessageCategory.Apps);
                LogMessage.LogMessageNow(2, "============= Shutting Down - Apps =================", Constants.MessageType.Info, Constants.MessageCategory.Apps);
                return;
            }
            else
            {
                string sParameterName;

                sParameterName = args[0];
                //////////// below line must be commented (was only for testing) and should be used above in realtime ///////////////////
                //sParameterName = PARAM_PerformanceEvaluation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (sParameterName.ToUpper() == PARAM_Notification)
                    SendNotification();
                else if (sParameterName == PARAM_PerformanceEvaluation)
                {
                    DateTime dtInputDate = DateTime.Now;
                    //////////// below line must be commented (was only for testing) and should be used above in realtime ///////////////////
                    //DateTime dtInputDate = new DateTime(2017, 10, 16);
                    if (args.Length > 1)
                    {
                        dtInputDate = Convert.ToDateTime(args[1].ToString());
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    CalculatePerformEval(dtInputDate);
                }
                else if (sParameterName == PARAM_Tender)
                {
                    TenderNotificationOneDayBefore();
                }
            }

            LogMessage.LogMessageNow(2, "============= Shutting Down - Apps =================", Constants.MessageType.Info, Constants.MessageCategory.Apps);
        }


        public static void SendNotification()
        {
            LogMessage.LogMessageNow(2, "---------- SendNotification - Starting -----------", Constants.MessageType.Info, Constants.MessageCategory.Apps);
            SendSMS();
            SendEmails();
            LogMessage.LogMessageNow(2, "---------- SendNotification - Done -----------", Constants.MessageType.Info, Constants.MessageCategory.Apps);
        }

        public static string GetDesiredDateFormat(DateTime dt)
        {
            return dt.Year + "_" + dt.Month.ToString("00") + "_" + dt.Day.ToString("00");
        }
        public static void CalculatePerformEval(DateTime _InputDate)
        {
            try
            {
                LogMessage.LogMessageNow(2, "---------- Performance Evaluation - Starting -----------", Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));

                DateTime Now = DateTime.Now;
                DateTime dtStartDate = Now;
                DateTime dtEndDate = Now;

                //if (Now.Day >= 1 && Now.Day <= 15)
                //{
                //    if (Now.Month == 1)
                //    {
                //        int Year = Now.Year - 1;
                //        int Month = 12;

                //        dtStartDate = new DateTime(Year, Month, 16);
                //        dtEndDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
                //    }
                //    else
                //    {
                //        int Month = Now.Month - 1;

                //        dtStartDate = new DateTime(Now.Year, Month, 16);
                //        dtEndDate = new DateTime(Now.Year, Month, DateTime.DaysInMonth(Now.Year, Month));
                //    }
                //}
                //else
                //{
                //    dtStartDate = new DateTime(Now.Year, Now.Month, 1);
                //    dtEndDate = new DateTime(Now.Year, Now.Month, 15);
                //}

                //dtStartDate = new DateTime(2014, 12, 01);
                //dtEndDate = new DateTime(2014, 12, 15);

                //dtStartDate = new DateTime(2014, 12, 16);
                //dtEndDate = new DateTime(2014, 12, 31);

                //int 
                if (_InputDate.Day == 16)
                {
                    dtStartDate = _InputDate.AddDays(-15);
                    dtEndDate = dtStartDate.AddDays(14);
                }
                else if (_InputDate.Day == 1)
                {
                    dtEndDate = _InputDate.AddDays(-1);
                    dtStartDate = new DateTime(dtEndDate.Year, dtEndDate.Month, 16);
                }
                else
                {
                    LogMessage.LogMessageNow(2, "Job is scheduled for 1st & 16th of every month only. Quiting ...", Constants.MessageType.Error, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));
                    return;
                }


                LogMessage.LogMessageNow(2, "Start Date = " + GetDesiredDateFormat(dtStartDate) + " - End Date = " + GetDesiredDateFormat(dtEndDate), Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));
                //return;



                PerformanceEvaluationBLL bllPE = new PerformanceEvaluationBLL();

                if (bllPE.IsEvaluationExists(dtStartDate, dtEndDate))
                {
                    return;
                }

                bllPE.GetFieldTailStatus_Job(dtStartDate, dtEndDate);
                LogMessage.LogMessageNow(2, "GetFieldTailStatus_Job - Done", Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));


                bllPE.GetPMIUTailStatus_Job(dtStartDate, dtEndDate);
                LogMessage.LogMessageNow(2, "GetPMIUTailStatus_Job - Done", Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));


                bllPE.GetTailDifference_Job(dtStartDate, dtEndDate);
                LogMessage.LogMessageNow(2, "GetTailDifference_Job - Done", Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));


                bllPE.GetHeadDifference_Job(dtStartDate, dtEndDate);
                LogMessage.LogMessageNow(2, "GetHeadDifference_Job - Done", Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));

                foreach (Constants.IrrigationLevelID level in Enum.GetValues(typeof(Constants.IrrigationLevelID)))
                {
                    if (level != Constants.IrrigationLevelID.Office)
                    {
                        bllPE.CalculatePEScore((long)level, "M", dtStartDate, dtEndDate, 2);
                        LogMessage.LogMessageNow(2, "M - CalculatePEScore Done For - " + level.ToString(), Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));
                        bllPE.CalculatePEScore((long)level, "E", dtStartDate, dtEndDate, 2);
                        LogMessage.LogMessageNow(2, "E - CalculatePEScore Done For - " + level.ToString(), Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));
                        bllPE.CalculatePEScore((long)level, "A", dtStartDate, dtEndDate, 2);
                        LogMessage.LogMessageNow(2, "A - CalculatePEScore Done For - " + level.ToString(), Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));
                    }
                }

                LogMessage.LogMessageNow(2, "---------- Performance Evaluation - Done -----------", Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));
            }
            catch (Exception exp)
            {
                LogMessage.LogMessageNow(2, "Exception : " + exp.Message, Constants.MessageType.Info, Constants.MessageCategory.Apps, GetDesiredDateFormat(_InputDate));
            }
        }

        public static void SendEmails()
        {
            LogMessage.LogMessageNow(2, "--- SENDING EMAILS ---", Constants.MessageType.Info, Constants.MessageCategory.Apps);
            NotificationsBLL bllNotification = new NotificationsBLL();
            List<UA_EmailNotification> lstEmail = bllNotification.GetPendingEmails();
            LogMessage.LogMessageNow(2, "Pending Emails Found: " + lstEmail.Count, Constants.MessageType.Info, Constants.MessageCategory.Apps);
            foreach (UA_EmailNotification mdlEmail in lstEmail)
            {
                try
                {
                    LogMessage.LogMessageNow(2, "Sending Email To : " + mdlEmail.EmailTo, Constants.MessageType.Info, Constants.MessageCategory.Apps);
                    PMIU.WRMIS.Common.Utility.SendEmail(mdlEmail);
                }
                catch (Exception exp)
                {
                    LogMessage.LogMessageNow(2, "ERROR: Sending Email Failed To : " + mdlEmail.EmailTo, Constants.MessageType.Info, Constants.MessageCategory.Apps);
                }
            }
            LogMessage.LogMessageNow(2, "Sending Email Status Back", Constants.MessageType.Info, Constants.MessageCategory.Apps);
            bllNotification.MarkEmailsSent(lstEmail);
            LogMessage.LogMessageNow(2, "------ Email Status Back Done ------", Constants.MessageType.Info, Constants.MessageCategory.Apps);
        }

        public static void SendSMS()
        {
            LogMessage.LogMessageNow(2, "--- SENDING SMS ---");
            NotificationsBLL bllNotification = new NotificationsBLL();
            List<UA_SMSNotification> lstSMS = bllNotification.GetPendingSMS();
            LogMessage.LogMessageNow(2, "Pending SMS Found: " + lstSMS.Count);
            foreach (UA_SMSNotification mdlSMS in lstSMS)
            {
                try
                {
                    LogMessage.LogMessageNow(2, "Sending SMS To : " + mdlSMS.MobileNumber);
                    PMIU.WRMIS.Common.Utility.SendSMS(mdlSMS);
                }
                catch (Exception exp)
                {
                    LogMessage.LogMessageNow(2, "ERROR: Sending SMS Failed To : " + mdlSMS.MobileNumber);
                }
            }
            LogMessage.LogMessageNow(2, "Sending SMS Status Back");
            bllNotification.MarkSMSSent(lstSMS);
            LogMessage.LogMessageNow(2, "------ SMS Status Back Done ------");

        }

        public static void TenderNotificationOneDayBefore()
        {
            try
            {

                TenderManagementBLL TMBLL = new TenderManagementBLL();
                TMBLL.GetTenderNoticeID();

            }
            catch (Exception exp)
            {

                throw;
            }

        }

    }
}
