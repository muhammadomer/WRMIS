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
    public partial class UserNotificationPreferences : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindUserNotificationPreferencesGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.UserNotificationPreferences);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindUserNotificationPreferencesGrid()
        {
            try
            {
                List<dynamic> bllLstUserNotificationPreferences = new AlertConfigurationBLL().GetUserNotificationPreferences(SessionManagerFacade.UserInformation.ID);
                if (bllLstUserNotificationPreferences == null || bllLstUserNotificationPreferences.Count <= 0)
                {
                    btnSave.Visible = false;
                    lnkCancel.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                    lnkCancel.Visible = true;
                }
                gvUserNotificationPreferences.DataSource = bllLstUserNotificationPreferences;
                gvUserNotificationPreferences.DataBind();

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
                AlertConfigurationBLL bllAlertConfiguration = new AlertConfigurationBLL();

                bool isRecordSaved = false;

                for (int rowIndex = 0; rowIndex < gvUserNotificationPreferences.Rows.Count; rowIndex++)
                {
                    DataKey key = gvUserNotificationPreferences.DataKeys[rowIndex];
                    long UserNotificationConfigID = Convert.ToInt64(key.Values["UserNotificationConfigID"]);
                    long NotificationEventID = Convert.ToInt64(key.Values["EventID"]);
                    bool chkNotification = ((CheckBox)gvUserNotificationPreferences.Rows[rowIndex].Cells[1].FindControl("chkNotification")).Checked;
                    bool chkSms = ((CheckBox)gvUserNotificationPreferences.Rows[rowIndex].Cells[2].FindControl("chkSMS")).Checked;
                    bool chkEmail = ((CheckBox)gvUserNotificationPreferences.Rows[rowIndex].Cells[3].FindControl("chkEmail")).Checked;

                    if (chkNotification || chkSms || chkEmail)
                    {
                        UA_UserNotificationConfiguration mdlUserNotificationConfiguration = new UA_UserNotificationConfiguration();
                        mdlUserNotificationConfiguration.ID = UserNotificationConfigID;
                        mdlUserNotificationConfiguration.NotificationEventID = NotificationEventID;
                        mdlUserNotificationConfiguration.Alert = Convert.ToBoolean(chkNotification);
                        mdlUserNotificationConfiguration.SMS = Convert.ToBoolean(chkSms);
                        mdlUserNotificationConfiguration.Email = Convert.ToBoolean(chkEmail);
                        mdlUserNotificationConfiguration.UserID = SessionManagerFacade.UserInformation.ID;

                        isRecordSaved = bllAlertConfiguration.AddUserNotificationPreferences(mdlUserNotificationConfiguration);

                        if (isRecordSaved)
                        {
                            gvUserNotificationPreferences.Visible = false;
                            btnSave.Visible = false;
                            lnkCancel.Visible = false;
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                        }
                        else
                            Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            try
            {
                BindUserNotificationPreferencesGrid();
                btnSave.Visible = true;
                lnkCancel.Visible = true;
                gvUserNotificationPreferences.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}