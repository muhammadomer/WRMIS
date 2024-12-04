using PMIU.WRMIS.DAL.DataAccess.UserAdministration;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.UserAdministration
{
    public class AlertConfigurationBLL : BaseBLL
    {

        /// <summary>
        /// This function return events by module ID
        /// Created On: 13/01/2016
        /// </summary>
        /// <param name="_ModuleID"></param>
        /// <returns>List<UA_ActionStep></returns>
        public List<UA_NotificationEvents> GetNotificationEventsByModuleID(long _ModuleID)
        {
            AlertConfigurationDAL dalAlertConfiguration = new AlertConfigurationDAL();
            return dalAlertConfiguration.GetNotificationEventsByModuleID(_ModuleID);
        }
        /// <summary>
        /// This function add alert configuration
        /// Created On: 19/01/2016
        /// </summary>
        /// <param name="_NotificationConfiguration"></param>
        /// <returns>bool</returns>
        public bool AddNotificationCofiguration(UA_NotificationConfiguration _NotificationConfiguration)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.AddNotificationCofiguration(_NotificationConfiguration);
        }

        public List<dynamic> GetDefaultAlertConfigurations(long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetDefaultAlertConfigurations(_EventID);
        }

        #region "User Notification Preferences"
        public List<dynamic> GetUserNotificationPreferences(long _UserID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetUserNotificationPreferences(_UserID);
        }
        public bool AddUserNotificationPreferences(UA_UserNotificationConfiguration _UserNotificationConfiguration)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.AddUserNotificationPreferences(_UserNotificationConfiguration);
        }
        public UA_GetNotifications_Result GetNotifications(long _EventID, long _UserID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetNotifications(_EventID, _UserID);
        }
        public UA_GetCrestBedLevelNotifyDTParameters_Result GetCrestBedLevelNotifyDTParameters(long _GaugeID, bool _IsCrestParameters)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetCrestBedLevelNotifyDTParameters(_GaugeID, _IsCrestParameters);
        }

        public WT_GetWaterTheftNotifyDTParameters_Result GetWaterTheftNotify(long _WaterTheftID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetWaterTheftNotify(_WaterTheftID);
        }

        public WT_GetBreachNotifyParameters_Result GetBreachNotifyParameter(long _BreachID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetBreachNotifyParameter(_BreachID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetIrrigationNetworkNotificationRecievers(long _EventID, long _GaugeID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetIrrigationNetworkNotificationRecievers(_EventID, _GaugeID);
        }

        public List<UA_GetNotificationsRecievers_Result> GetNotificationRecievers(long _EventID, long _UserID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetNotificationRecievers(_EventID, _UserID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetScheduleInspectionNotifyReciever(long _ScheduleID, long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetScheduleInspectionNotifyReciever(_ScheduleID, _EventID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetDischargeTableCalcBLNotifyReciever(long _ScheduleDetailsID, long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetDischargeTableCalcBLNotifyReciever(_ScheduleDetailsID, _EventID);

        }
        public CM_GetComplaintsNotifyParameters_Result GetComplaintNotificationParameters(long _ComplaintID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetComplaintNotificationParameters(_ComplaintID);
        }

        public CM_GetComplaintsCommentsNotifyParameters_Result GetComplaintCommentsNotificationParameters(long _ComplaintID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetComplaintCommentsNotificationParameters(_ComplaintID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetClosureOperationsNotifyReciever(long _CWPID, long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetClosureOperationNotifyReciever(_CWPID, _EventID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetAssetWorksNotifyReciever(long _CWPID, long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetAssetWorksNotifyReciever(_CWPID, _EventID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetAssetWorksAssociationNotifyReciever(long _CWPID, long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetAssetWorksAssociationNotifyReciever(_CWPID, _EventID);
        }
        public TM_GetTenderNotifyParameters_Result GetTenderNotificationParameters(long _TenderNoticeID, long _TenderWorkID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetTenderNotificationParameters(_TenderNoticeID, _TenderWorkID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetFloodOperationsNotifyRecievers(long _ID, long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetFloodOperationsNotifyRecievers(_ID, _EventID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetAuctionNotifyRecievers(long _ID, long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetAuctionNotifyRecievers(_ID, _EventID);
        }
        #endregion

        #region "Push Data into outbox table"
        public bool AddAlertNotificationData(UA_AlertNotification _AlertNotification)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.AddAlertNotificationData(_AlertNotification);
        }

        public bool AddSMSNotificationData(UA_SMSNotification _SMSNotification)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.AddSMSNotificationData(_SMSNotification);
        }

        public bool AddEmailNotificationData(UA_EmailNotification _EmailNotification)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.AddEmailNotificationData(_EmailNotification);
        }

        public UA_NotificationEvents GetModuleIDByEventID(long _EventID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetModuleIDByEventID(_EventID);
        }

        public long GetUserIDByGaugeID(long _GaugeID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetUserIDByGaugeID(_GaugeID);
        }

        public DD_GetPlaceDataDailyData_Result GetDataForPlaceHolderDailyData()
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetDataForPlaceHolderDailyData();
        }

        public long GetUserIDBySubDivisionID(long _SubDivisionID)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            long _SDOUserID = dalAlertConfig.GetUserIDBySubDivisionID(_SubDivisionID);
            return _SDOUserID;

        }
        public List<UA_GetNotificationsRecievers_Result> GetDischargeTableCalcBLNotifyRecieverAndroid(long _ScheduleDetailsID, long _EventID,string _CalcType)
        {
            AlertConfigurationDAL dalAlertConfig = new AlertConfigurationDAL();
            return dalAlertConfig.GetDischargeTableCalcBLNotifyRecieverAndroid(_ScheduleDetailsID, _EventID, _CalcType);

        }
        #endregion
    }
}
