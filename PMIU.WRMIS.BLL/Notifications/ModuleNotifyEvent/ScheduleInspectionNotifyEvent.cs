using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Notifications.ModuleNotifyEvent
{
    public class ScheduleInspectionNotifyEvent : BaseNotifyEvent
    {
        public bool AddScheduleInspectionNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            if (_EventID == (long)NotificationEventConstants.ScheduleInspection.InspectionOfADMAssignedToDDForApproval || _EventID == (long)NotificationEventConstants.ScheduleInspection.InspectionOfADMCrestLevelAssignedToDDForApproval)
            {

                
                    SI_GetDischargeTableCalcBLNotifyData_Result mdlDischargeTableCalcBLNotifyData = new ScheduleInspectionBLL().GetDischargeTableCalcBLNotifyData(Convert.ToInt64(_Parameters["ScheduleDetailID"]));
                    List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetDischargeTableCalcBLNotifyReciever(Convert.ToInt64(_Parameters["ScheduleDetailID"]), _EventID);
                    return LogNotificationsIntoDatabase<SI_GetDischargeTableCalcBLNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlDischargeTableCalcBLNotifyData);
                
            }




            else if (_EventID == (long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMAssignedToDDForApproval || _EventID == (long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMCrestLevelAssignedToDDForApproval
                || _EventID == (long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMisapprovedbyDD || _EventID == (long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMCLisapprovedbyDD)
            {

                long? _ScheduleDetailChannelID = Convert.ToInt64(_Parameters["ScheduleDetailID"]);
                long? GaugeID = Convert.ToInt64(_Parameters["_GaugeID"]);
                string _CalcType = "";
                if (_EventID == (long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMAssignedToDDForApproval || _EventID == (long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMisapprovedbyDD)
                {
                    _CalcType = "BED";
                }
                else if (_EventID == (long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMCrestLevelAssignedToDDForApproval || _EventID == (long)NotificationEventConstants.ScheduleInspection.UnscheduleInspectionOfADMCLisapprovedbyDD)
                {
                    _CalcType = "CREST";
                }

                if (_ScheduleDetailChannelID == null || _ScheduleDetailChannelID == 0)
                {
                    SI_GetDischargeTableCalcBLNotifyDataAndroid_Result mdlDischargeTableCalcBLNotifyData = new ScheduleInspectionBLL().GetDischargeTableCalcBLNotifyDataAndroid(GaugeID, _CalcType);
                    List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetDischargeTableCalcBLNotifyRecieverAndroid(Convert.ToInt64(_Parameters["_GaugeID"]), _EventID, _CalcType);
                    return LogNotificationsIntoDatabase<SI_GetDischargeTableCalcBLNotifyDataAndroid_Result>(_EventID, _UserID, lstNotificationRecievers, mdlDischargeTableCalcBLNotifyData);
                }
                else
                {
                    SI_GetDischargeTableCalcBLNotifyData_Result mdlDischargeTableCalcBLNotifyData = new ScheduleInspectionBLL().GetDischargeTableCalcBLNotifyData(Convert.ToInt64(_Parameters["ScheduleDetailID"]));
                    List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetDischargeTableCalcBLNotifyReciever(Convert.ToInt64(_Parameters["ScheduleDetailID"]), _EventID);
                    return LogNotificationsIntoDatabase<SI_GetDischargeTableCalcBLNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlDischargeTableCalcBLNotifyData);
                }
            }
            else if (_EventID == (long)NotificationEventConstants.ScheduleInspection.InspectionOfADMIsApprovedByDD || _EventID == (long)NotificationEventConstants.ScheduleInspection.InspectionOfADMCrestLevelIsApprovedByDD)
            {
                SI_GetDischargeTableCalcBLNotifyData_Result mdlDischargeTableCalcBLNotifyData = new ScheduleInspectionBLL().GetDischargeTableCalcBLNotifyData(Convert.ToInt64(_Parameters["ScheduleDetailID"]));
                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetDischargeTableCalcBLNotifyReciever(Convert.ToInt64(_Parameters["ScheduleDetailID"]), _EventID);
                return LogNotificationsIntoDatabase<SI_GetDischargeTableCalcBLNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlDischargeTableCalcBLNotifyData); ;
            }
            else
            {
                SI_GetScheduleInspectionNotifyData_Result mdlScheduleInspectionNotifyData = new ScheduleInspectionBLL().GetScheduleInspectionNotifyData(Convert.ToInt64(_Parameters["ScheduleID"]));

                List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = new AlertConfigurationBLL().GetScheduleInspectionNotifyReciever(Convert.ToInt64(_Parameters["ScheduleID"]), _EventID);

                return LogNotificationsIntoDatabase<SI_GetScheduleInspectionNotifyData_Result>(_EventID, _UserID, lstNotificationRecievers, mdlScheduleInspectionNotifyData);
            }

        }
    }
}
