using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Notifications.ModuleNotifyEvent
{
    public class AssetWorksNotifyEvent : BaseNotifyEvent
    {
        AssetsWorkBLL bllCO = new AssetsWorkBLL();
        AlertConfigurationBLL bllAC = new AlertConfigurationBLL();
        public bool AddAssetWorkNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            bool result = false;
            List<UA_GetNotificationsRecievers_Result> rcivrs = new List<UA_GetNotificationsRecievers_Result>();
            if (_EventID == 57)
                rcivrs = bllAC.GetAssetWorksAssociationNotifyReciever(Convert.ToInt64(_Parameters["ID"]), _EventID);
            else
                rcivrs = bllAC.GetAssetWorksNotifyReciever(Convert.ToInt64(_Parameters["ID"]), _EventID);

            switch (_EventID)
            {
                case 56: //Work published
                    AM_GetAWPublishNotifyData_Result1 mdl = bllCO.GetAW_Publish_NotifyData(Convert.ToInt64(_Parameters["ID"]));
                    result = LogNotificationsIntoDatabase<AM_GetAWPublishNotifyData_Result1>(_EventID, _UserID, rcivrs, mdl);
                    break;

                case 54: // Asset Progress
                    AM_GetAWProgressNotifyData_Result mdlPrgs = bllCO.GetAW_Progress_NotifyData(Convert.ToInt64(_Parameters["ID"]));
                    result = LogNotificationsIntoDatabase<AM_GetAWProgressNotifyData_Result>(_EventID, _UserID, rcivrs, mdlPrgs);
                    break;
                case 57: // Work Association
                    AM_GetAWAssetAssociationNotifyData_Result mdlAssociation = bllCO.GetAW_Association_NotifyData(Convert.ToInt64(_Parameters["ID"]));
                    result = LogNotificationsIntoDatabase<AM_GetAWAssetAssociationNotifyData_Result>(_EventID, _UserID, rcivrs, mdlAssociation);
                    break;
                default:
                    break;
            }
            return result;
        }

        //
    }
}
