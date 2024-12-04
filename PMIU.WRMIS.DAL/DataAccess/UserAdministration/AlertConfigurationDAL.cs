using PMIU.WRMIS.DAL;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{
    public class AlertConfigurationDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// This function return Notification Events by module ID
        /// Created On: 13/01/2016
        /// </summary>
        /// <param name="_ModuleID"></param>
        /// <returns>List<UA_NotificationEvents></returns>
        public List<UA_NotificationEvents> GetNotificationEventsByModuleID(long _ModuleID)
        {
            List<UA_NotificationEvents> lstEvents = db.Repository<UA_NotificationEvents>().GetAll().Where(a => a.ModulesID == _ModuleID).ToList<UA_NotificationEvents>();
            return lstEvents;
        }

        /// <summary>
        /// this function add alert configuration
        /// Created On: 19/01/2016
        /// </summary>
        /// <param name="_NotificationConfiguration"></param>
        /// <returns>bool</returns>
        public bool AddNotificationCofiguration(UA_NotificationConfiguration _NotificationConfiguration)
        {
            if (_NotificationConfiguration.ID > 0)
                db.Repository<UA_NotificationConfiguration>().Delete(_NotificationConfiguration.ID);

            db.Repository<UA_NotificationConfiguration>().Insert(_NotificationConfiguration);

            db.Save();
            return true;
        }
        public List<dynamic> GetDefaultAlertConfigurations(long _EventID)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetDefaultAlertConfigurations(_EventID);
        }

        #region "User Notification Preferences"
        public List<dynamic> GetUserNotificationPreferences(long _UserID)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetUserNotificationPreferences(_UserID);
        }
        public bool AddUserNotificationPreferences(UA_UserNotificationConfiguration _UserNotificationConfiguration)
        {
            if (_UserNotificationConfiguration.ID > 0)
                db.Repository<UA_UserNotificationConfiguration>().Delete(_UserNotificationConfiguration.ID);

            db.Repository<UA_UserNotificationConfiguration>().Insert(_UserNotificationConfiguration);

            db.Save();
            return true;
        }

        public UA_GetNotifications_Result GetNotifications(long _EventID, long _UserID)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetNotifications(_EventID, _UserID);
        }
        public UA_GetCrestBedLevelNotifyDTParameters_Result GetCrestBedLevelNotifyDTParameters(long _GaugeID, bool _IsCrestParameters)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetCrestBedLevelNotifyDTParameters(_GaugeID, _IsCrestParameters);
        }

        public WT_GetWaterTheftNotifyDTParameters_Result GetWaterTheftNotify(long _WaterTheftID)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetWaterTheftNotify(_WaterTheftID);
        }

        public WT_GetBreachNotifyParameters_Result GetBreachNotifyParameter(long _BreachID)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetBreachNotifyParameter(_BreachID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetIrrigationNetworkNotificationRecievers(long _EventID, long _GaugeID)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetIrrigationNetworkNotificationRecievers(_EventID, _GaugeID);
        }

        public List<UA_GetNotificationsRecievers_Result> GetNotificationRecievers(long _EventID, long _UserID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetNotificationRecievers(_EventID, _UserID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetScheduleInspectionNotifyReciever(long _ScheduleID, long _EventID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetScheduleInspectionNotifyReciever(_ScheduleID, _EventID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetDischargeTableCalcBLNotifyReciever(long _ScheduleDetailsID, long _EventID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetDischargeTableCalcBLNotifyReciever(_ScheduleDetailsID, _EventID);
        }
        public CM_GetComplaintsNotifyParameters_Result GetComplaintNotificationParameters(long _ComplaintID)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetComplaintNotificationParameters(_ComplaintID);
        }

        public CM_GetComplaintsCommentsNotifyParameters_Result GetComplaintCommentsNotificationParameters(long _ComplaintID)
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetComplaintCommentsNotificationParameters(_ComplaintID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetClosureOperationNotifyReciever(long _CWPID, long _EventID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetClosureOperationNotifyReciever(_CWPID, _EventID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetAssetWorksNotifyReciever(long _CWPID, long _EventID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetAssetWorksNotifyReciever(_CWPID, _EventID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetAssetWorksAssociationNotifyReciever(long _CWPID, long _EventID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetAssetWorksAssociationNotifyReciever(_CWPID, _EventID);
        }
        public TM_GetTenderNotifyParameters_Result GetTenderNotificationParameters(long _TenderNoticeID, long _TenderWorkID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetTenderNotificationParameters(_TenderNoticeID, _TenderWorkID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetFloodOperationsNotifyRecievers(long _ID, long _EventID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetFloodOperationsNotifyRecievers(_ID, _EventID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetAuctionNotifyRecievers(long _ID, long _EventID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetAuctionNotifyRecievers(_ID, _EventID);
        }
        #endregion

        #region Add Alert Notification Data in outbox

        public bool AddAlertNotificationData(UA_AlertNotification _AlertNotification)
        {
            db.Repository<UA_AlertNotification>().Insert(_AlertNotification);
            db.Save();
            return true;
        }

        public bool AddSMSNotificationData(UA_SMSNotification _SMSNotification)
        {
            db.Repository<UA_SMSNotification>().Insert(_SMSNotification);
            db.Save();
            return true;
        }

        public bool AddEmailNotificationData(UA_EmailNotification _EmailNotification)
        {
            db.Repository<UA_EmailNotification>().Insert(_EmailNotification);
            db.Save();
            return true;
        }

        public UA_NotificationEvents GetModuleIDByEventID(long _EventID)
        {
            UA_NotificationEvents mdlNotification = db.Repository<UA_NotificationEvents>().GetAll().Where(a => a.ID == _EventID).FirstOrDefault();
            return mdlNotification;
        }

        public long GetUserIDByGaugeID(long _GaugeID)
        {
            //UA_AssociatedStations mdlAssociatedStations = db.Repository<UA_AssociatedStations>().GetAll().Where(a => a.GaugeOutlet == _GaugeID && a.StationSite == "G" && a.StractureTypeID== 6).FirstOrDefault();
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetUserIDByGaugeID(_GaugeID);
        }

        public DD_GetPlaceDataDailyData_Result GetDataForPlaceHolderDailyData()
        {
            AlertConfigurationRepository mdlAlertConfiguration = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return mdlAlertConfiguration.GetDataForPlaceHolderDailyData();
        }

        public long GetUserIDBySubDivisionID(long _SubDivisionID)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetUserIDBySubDivisionID(_SubDivisionID);
        }
        public List<UA_GetNotificationsRecievers_Result> GetDischargeTableCalcBLNotifyRecieverAndroid(long _GaugeID, long _EventID, string _CalcType)
        {
            AlertConfigurationRepository lstNotificationRecievers = this.db.ExtRepositoryFor<AlertConfigurationRepository>();
            return lstNotificationRecievers.GetDischargeTableCalcBLNotifyRecieverAndroid(_GaugeID, _EventID, _CalcType);
        }
        #endregion

    }
}
