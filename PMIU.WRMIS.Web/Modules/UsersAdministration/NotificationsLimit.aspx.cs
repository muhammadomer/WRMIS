using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class NotificationsLimit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //UA_NotificationLimit mdlNotificationLimit2 = new UA_NotificationLimit();
                    //NotificationsLimitBLL bllNotificationLimit = new NotificationsLimitBLL();
                    //SetPageTitle();
                    //Dropdownlist.BindDropdownlist<List<object>>(ddlNotificationLimit, new UserAdministrationBLL().GetNotificationLimit());
                    //mdlNotificationLimit2 = bllNotificationLimit.GetNotification();
                    //Dropdownlist.SetSelectedText(ddlNotificationLimit, Convert.ToString(mdlNotificationLimit2.NotificationLimit));
                    //if (!base.CanEdit)
                    //{
                    //    ddlNotificationLimit.Enabled = false;
                    //    btnSave.Visible = false;
                    //    lbtnCancel.Visible = false;
                    //}
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //UA_NotificationLimit mdlNotificationLimit = new UA_NotificationLimit();
                //UA_NotificationLimit mdlNotificationLimit2 = new UA_NotificationLimit();
                //NotificationsLimitBLL bllNotificationLimit = new NotificationsLimitBLL();

                //mdlNotificationLimit = bllNotificationLimit.GetNotification();

                
                //bool IsRecordSaved;
                //if (mdlNotificationLimit == null)
                //{
                //    mdlNotificationLimit2.NotificationLimit = Convert.ToInt32(ddlNotificationLimit.SelectedItem.Text);
                //    IsRecordSaved = bllNotificationLimit.AddNotificationLimit(mdlNotificationLimit2);
                //}
                //else
                //{
                //    mdlNotificationLimit2.ID = mdlNotificationLimit.ID;
                //    mdlNotificationLimit2.NotificationLimit = Convert.ToInt32(ddlNotificationLimit.SelectedItem.Text);
                //    IsRecordSaved = bllNotificationLimit.UpdateNotification(mdlNotificationLimit2);
                //}

                //Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function sets title on Page
        /// Created On: 07/01/2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.NotificationsLimit);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //UA_NotificationLimit mdlNotificationLimit = new UA_NotificationLimit();
                //UA_NotificationLimit mdlNotificationLimit2 = new UA_NotificationLimit();
                //NotificationsLimitBLL bllNotificationLimit = new NotificationsLimitBLL();
                //mdlNotificationLimit2 = bllNotificationLimit.GetNotification();
                //long NotificationID = mdlNotificationLimit2.ID;
                //mdlNotificationLimit = bllNotificationLimit.GetNotificationByID(NotificationID);
                //ddlNotificationLimit.ClearSelection();
                //Dropdownlist.SetSelectedValue(ddlNotificationLimit, mdlNotificationLimit.NotificationLimit.ToString());
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}