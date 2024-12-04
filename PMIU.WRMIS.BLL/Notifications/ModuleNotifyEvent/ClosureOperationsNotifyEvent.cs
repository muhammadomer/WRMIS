using PMIU.WRMIS.BLL.ClosureOperations;
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
    public class ClosureOperationsNotifyEvent : BaseNotifyEvent
    {
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        AlertConfigurationBLL bllAC =  new AlertConfigurationBLL();
        public bool AddClosureOperationNotifyEvent(long _UserID, Dictionary<string, object> _Parameters, long _EventID)
        {
            bool result = false;
            List<UA_GetNotificationsRecievers_Result> rcivrs = bllAC.GetClosureOperationsNotifyReciever(Convert.ToInt64(_Parameters["ID"]), _EventID);
           
            switch (_EventID)
            {
                case 45: //Closure Work Plan published
                    CW_GetCWPPublishNotifyData_Result mdl = bllCO.GetClosureWorkPlanPuslishNotifyData(Convert.ToInt64(_Parameters["ID"])); 
                    result= LogNotificationsIntoDatabase<CW_GetCWPPublishNotifyData_Result>(_EventID, _UserID, rcivrs, mdl);
                    break;

                case 46: // Closure Progress
                    CW_GetCWProgressNotifyData_Result mdlPrgs = bllCO.GetCW_Progress_NotifyData(Convert.ToInt64(_Parameters["ID"]));
                    result = LogNotificationsIntoDatabase<CW_GetCWProgressNotifyData_Result>(_EventID, _UserID, rcivrs, mdlPrgs);
                    break;
                default:
                    break;
            }
           return result;
        }
    }
}
