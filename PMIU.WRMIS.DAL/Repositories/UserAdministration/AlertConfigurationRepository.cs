using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.UserAdministration
{
    public class AlertConfigurationRepository : Repository<UA_AlertNotification>
    {
        WRMIS_Entities _context;
        public AlertConfigurationRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<UA_AlertNotification>();
            _context = context;
        }

        public List<dynamic> GetDefaultAlertConfigurations(long _EventID)
        {
            List<UA_NotificationConfiguration> lstNotification = (from unc in context.UA_NotificationConfiguration
                                                                  where unc.NotificationEventID == _EventID
                                                                  select unc).ToList<UA_NotificationConfiguration>();

            List<dynamic> lstAltertConfiguration = (from ud in context.UA_Designations
                                                    join uo in context.UA_Organization on ud.OrganizationID equals uo.ID
                                                    select new
                                                    {
                                                        DesignationID = ud.ID,
                                                        OrganizationName = uo.Name,
                                                        DesignationName = ud.Name
                                                    }).ToList<dynamic>();

            List<dynamic> lstDefaultAlertConfiguration = (from ac in lstAltertConfiguration
                                                          join unc in lstNotification on ac.DesignationID equals unc.DesignationID into alrt
                                                          from alertNoti in alrt.DefaultIfEmpty()
                                                          orderby ac.OrganizationName ascending
                                                          select new
                                                          {
                                                              NotificationConfigID = alertNoti != null ? alertNoti.ID : 0,
                                                              ac.DesignationID,
                                                              ac.OrganizationName,
                                                              ac.DesignationName,
                                                              NotificationName = alertNoti != null ? alertNoti.Alert.Value : false,
                                                              SMS = alertNoti != null ? alertNoti.SMS.Value : false,
                                                              Email = alertNoti != null ? alertNoti.Email.Value : false
                                                          }).ToList<dynamic>();
            return lstDefaultAlertConfiguration;
        }

        #region "User Notification Preferences"
        public List<dynamic> GetUserNotificationPreferences(long _UserID)
        {
            List<dynamic> lstUserNotificationPreferences = context.UA_GetUserNotificationPreferences(_UserID)
                                                            .Select(p => new
                                                            {
                                                                p.ModuleName,
                                                                p.UserNotificationConfigID,
                                                                p.EventID,
                                                                p.EventName,
                                                                p.Alert,
                                                                p.SMS,
                                                                p.Email,
                                                                IsSMSByDefaultEnabled = Convert.ToBoolean(p.IsSMSByDefaultEnabled),
                                                                IsEmailByDefaultEnabled = Convert.ToBoolean(p.IsEmailByDefaultEnabled)
                                                            }).ToList<dynamic>();

            return lstUserNotificationPreferences;
        }

        public UA_GetNotifications_Result GetNotifications(long _EventID, long _UserID)
        {
            UA_GetNotifications_Result lstNotification = context.UA_GetNotifications(_EventID, _UserID).FirstOrDefault<UA_GetNotifications_Result>();
            return lstNotification;
        }
        public UA_GetCrestBedLevelNotifyDTParameters_Result GetCrestBedLevelNotifyDTParameters(long _GaugeID, bool _IsCrestParameters)
        {
            UA_GetCrestBedLevelNotifyDTParameters_Result lstCrestBedLevelNotifyDTParameters = context.UA_GetCrestBedLevelNotifyDTParameters(_GaugeID, _IsCrestParameters).FirstOrDefault<UA_GetCrestBedLevelNotifyDTParameters_Result>();
            return lstCrestBedLevelNotifyDTParameters;
        }

        public WT_GetWaterTheftNotifyDTParameters_Result GetWaterTheftNotify(long _WaterTheftID)
        {
            WT_GetWaterTheftNotifyDTParameters_Result lstWaterTheftNotify = context.WT_GetWaterTheftNotifyDTParameters(_WaterTheftID).FirstOrDefault<WT_GetWaterTheftNotifyDTParameters_Result>();

            return lstWaterTheftNotify;
        }

        public WT_GetBreachNotifyParameters_Result GetBreachNotifyParameter(long _BreachID)
        {
            WT_GetBreachNotifyParameters_Result lstBreachNotify = context.WT_GetBreachNotifyParameters(_BreachID).FirstOrDefault<WT_GetBreachNotifyParameters_Result>();

            return lstBreachNotify;
        }
        public List<UA_GetNotificationsRecievers_Result> GetIrrigationNetworkNotificationRecievers(long _EventID, long _GaugeID)
        {
            List<UA_GetIrrigationNetworkNotificationRecievers_Result> lstNotificationRecievers = context.UA_GetIrrigationNetworkNotificationRecievers(_EventID, _GaugeID).ToList<UA_GetIrrigationNetworkNotificationRecievers_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }

        public List<UA_GetNotificationsRecievers_Result> GetNotificationRecievers(long _EventID, long _UserID)
        {
            List<UA_GetNotificationsRecievers_Result> lstNotificationRecievers = context.UA_GetNotificationsRecievers(_EventID, _UserID).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotificationRecievers;
        }
        public List<UA_GetNotificationsRecievers_Result> GetScheduleInspectionNotifyReciever(long _ScheduleID, long _EventID)
        {
            List<SI_GetScheduleInspectionNotifyReciever_Result> lstNotificationRecievers = context.SI_GetScheduleInspectionNotifyReciever(_ScheduleID, _EventID).ToList<SI_GetScheduleInspectionNotifyReciever_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }
        public List<UA_GetNotificationsRecievers_Result> GetDischargeTableCalcBLNotifyReciever(long _ScheduleDetailsID, long _EventID)
        {
            List<SI_GetDischargeTableCalcBLOrCLNotifyReciever_Result> lstNotificationRecievers = context.SI_GetDischargeTableCalcBLOrCLNotifyReciever(_ScheduleDetailsID, _EventID).ToList<SI_GetDischargeTableCalcBLOrCLNotifyReciever_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }
        public DD_GetPlaceDataDailyData_Result GetDataForPlaceHolderDailyData()
        {
            DD_GetPlaceDataDailyData_Result lstDailyDataNotify = context.DD_GetPlaceDataDailyData().FirstOrDefault<DD_GetPlaceDataDailyData_Result>();

            return lstDailyDataNotify;
        }

        public long GetUserIDBySubDivisionID(long _SubDivisionID)
        {
            long _SDOUserID = (from al in context.UA_AssociatedLocation
                               join codiv in context.CO_SubDivision on al.IrrigationBoundryID equals codiv.ID
                               where codiv.ID == _SubDivisionID
                               select al.UserID).FirstOrDefault();
            return _SDOUserID;
        }

        public long GetUserIDByGaugeID(long _GaugeID)
        {
            long _UserID = (from cg in context.CO_ChannelGauge
                            join al in context.UA_AssociatedLocation on cg.SectionID equals al.IrrigationBoundryID
                            where cg.ID == _GaugeID && al.IrrigationLevelID == 5
                            select al.UserID).FirstOrDefault();
            return _UserID;
        }

        public CM_GetComplaintsNotifyParameters_Result GetComplaintNotificationParameters(long _ComplaintID)
        {
            CM_GetComplaintsNotifyParameters_Result lstWaterTheftNotify = context.CM_GetComplaintsNotifyParameters(_ComplaintID).FirstOrDefault<CM_GetComplaintsNotifyParameters_Result>();

            return lstWaterTheftNotify;
        }
        public CM_GetComplaintsCommentsNotifyParameters_Result GetComplaintCommentsNotificationParameters(long _ComplaintID)
        {
            CM_GetComplaintsCommentsNotifyParameters_Result lstWaterTheftNotify = context.CM_GetComplaintsCommentsNotifyParameters(_ComplaintID).FirstOrDefault<CM_GetComplaintsCommentsNotifyParameters_Result>();

            return lstWaterTheftNotify;
        }
        public List<UA_GetNotificationsRecievers_Result> GetClosureOperationNotifyReciever(long _CWPID, long _EventID)
        {
            List<CW_GetClosureOperationsNotifyReciever_Result> lstNotificationRecievers = context.CW_GetClosureOperationsNotifyReciever(_CWPID, _EventID).ToList<CW_GetClosureOperationsNotifyReciever_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }
        public List<UA_GetNotificationsRecievers_Result> GetAssetWorksNotifyReciever(long _CWPID, long _EventID)
        {
            List<AM_GetAssetWorksNotifyReciever_Result> lstNotificationRecievers = context.AM_GetAssetWorksNotifyReciever(_CWPID, _EventID).ToList<AM_GetAssetWorksNotifyReciever_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }
        public List<UA_GetNotificationsRecievers_Result> GetAssetWorksAssociationNotifyReciever(long _AWID, long _EventID)
        {
            List<AM_GetWorkAssociationNotifyReciever_Result> lstNotificationRecievers = context.AM_GetWorkAssociationNotifyReciever(_AWID, _EventID).ToList<AM_GetWorkAssociationNotifyReciever_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }

        public TM_GetTenderNotifyParameters_Result GetTenderNotificationParameters(long _TenderNoticeID, long _TenderWorkID)
        {
            TM_GetTenderNotifyParameters_Result lstWaterTheftNotify = context.TM_GetTenderNotifyParameters(_TenderNoticeID, _TenderWorkID).FirstOrDefault<TM_GetTenderNotifyParameters_Result>();

            return lstWaterTheftNotify;
        }

        public List<UA_GetNotificationsRecievers_Result> GetFloodOperationsNotifyRecievers(long _ID, long _EventID)
        {
            List<FO_GetFloodOperationsNotifyReciever_Result> lstNotificationRecievers = context.FO_GetFloodOperationsNotifyReciever(_ID, _EventID).ToList<FO_GetFloodOperationsNotifyReciever_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }

        public List<UA_GetNotificationsRecievers_Result> GetAuctionNotifyRecievers(long _ID, long _EventID)
        {
            List<AC_GetAuctionsNotifyReciever_Result> lstNotificationRecievers = context.AC_GetAuctionsNotifyReciever(_ID, _EventID).ToList<AC_GetAuctionsNotifyReciever_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }
        public List<UA_GetNotificationsRecievers_Result> GetDischargeTableCalcBLNotifyRecieverAndroid(long _GaugeID, long _EventID, string _CalcType)
        {
            List<SI_GetDischargeTableCalcBLOrCLNotifyRecieverAndroid_Result> lstNotificationRecievers = context.SI_GetDischargeTableCalcBLOrCLNotifyRecieverAndroid(_GaugeID, _EventID, _CalcType).ToList<SI_GetDischargeTableCalcBLOrCLNotifyRecieverAndroid_Result>();
            List<UA_GetNotificationsRecievers_Result> lstNotify = lstNotificationRecievers.Select(i => new UA_GetNotificationsRecievers_Result
            {
                UserID = i.UserID,
                UserName = i.UserName,
                UserEmail = i.UserEmail,
                MobilePhone = i.MobilePhone,
                NotificationID = i.NotificationID,
                UserConfigID = i.UserConfigID,
                EventID = i.EventID,
                EventName = i.EventName,
                Description = i.Description,
                ModulesID = i.ModulesID,
                SMSTemplate = i.SMSTemplate,
                AlertTemplate = i.AlertTemplate,
                PageID = i.PageID,
                URLQueryString = i.URLQueryString,
                EmailTemplateSubject = i.EmailTemplateSubject,
                EmailTemplateBody = i.EmailTemplateBody,
                CCSMS = i.CCSMS,
                CCEmail = i.CCEmail,
                Alert = i.Alert,
                SMS = i.SMS,
                Email = i.Email,
                IsAlertByDefaultEnabled = i.IsAlertByDefaultEnabled,
                IsSMSByDefaultEnabled = i.IsSMSByDefaultEnabled,
                IsEmailByDefaultEnabled = i.IsEmailByDefaultEnabled
            }).ToList<UA_GetNotificationsRecievers_Result>();
            return lstNotify;
        }
        #endregion
    }
}