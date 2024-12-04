using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.BLL.Notifications.ModuleNotifyEvent;
using PMIU.WRMIS.BLL.ScheduleInspection;

namespace PMIU.WRMIS.BLL.Notifications
{
    public class NotifyEvent
    {
        public Dictionary<string, object> Parameters { get; set; }
        public NotifyEvent()
        {
            this.Parameters = new Dictionary<string, object>();
        }
        public bool AddNotifyEvent(long _EventID, long _UserID)
        {
            //  UA_GetNotifications_Result bllNotificationEvent = new AlertConfigurationBLL().GetNotifications(_EventID, _UserID);
            UA_NotificationEvents mdlNotification = new AlertConfigurationBLL().GetModuleIDByEventID(_EventID);

            bool isAdded = false;
            if (mdlNotification.ModulesID == (long)Constants.ModuleName.IrrigationNetwork)
            {
                try
                {
                    isAdded = new IrrigationNetworkNotifyEvent().AddIrrigationNetworkNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.WaterTheft)
            {
                try
                {
                    isAdded = new WaterTheftNotifyEvent().AddWaterTheftNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.DailyOperationalData)
            {
                try
                {


                    isAdded = new DailyDataNotifyEvent().AddDailyDataNotifyEvent(_UserID, this.Parameters, _EventID);

                }
                catch (Exception)
                {

                    throw;
                }

            }
            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.ComplaintsCMDirectives)
            {
                try
                {
                    if (_EventID == (long)NotificationEventConstants.Complaints.AcommentisaddedbyAdditionalAccessibilityuser)
                        isAdded = new ComplaintsNotifyEvent().AddComplaintCommentsNotifyEvent(_UserID, this.Parameters, _EventID);
                    else
                        isAdded = new ComplaintsNotifyEvent().AddComplaintNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.ScheduleInspections)
            {
                try
                {
                    isAdded = new ScheduleInspectionNotifyEvent().AddScheduleInspectionNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.ClosureOperations)
            {
                try
                {
                    isAdded = new ClosureOperationsNotifyEvent().AddClosureOperationNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.AssetsManagement)
            {
                try
                {
                    isAdded = new AssetWorksNotifyEvent().AddAssetWorkNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.TendersMonitoring)
            {
                try
                {
                    isAdded = new TenderNotifyEvent().AddTenderNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.FloodOperations)
            {
                try
                {
                    isAdded = new FloodOperationsNotifyEvent().AddFloodOperationsNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            else if (mdlNotification.ModulesID == (long)Constants.ModuleName.Auction)
            {
                try
                {
                    isAdded = new AuctionsNotifyEvent().AddAuctionsNotifyEvent(_UserID, this.Parameters, _EventID);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            // if (bllNotificationEvent != null)
            ///  {
            // switch (bllNotificationEvent.ModulesID.Value)
            // {
            // case (long)Constants.ModuleName.IrrigationNetwork:

            //    break;
            //case (long)Constants.ModuleName.WaterTheft:

            //    break;
            //default:
            //    break;
            //}
            //}
            return isAdded;
        }
    }
}
