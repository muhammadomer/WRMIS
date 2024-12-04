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
    public partial class AlertConfiguration : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDropdownLists();
                    btnSave.Visible = false;
                    lbtnCancel.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long ModuleID = ddlModule.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlModule.SelectedItem.Value);
                Dropdownlist.DDLNotificationEvents(ddlEvent);

                if (ModuleID == -1)
                {
                    gvAlertConfiguration.Visible = false;
                    btnSave.Visible = false;
                    lbtnCancel.Visible = false;
                }
                else
                {
                    Dropdownlist.DDLNotificationEvents(ddlEvent, false, ModuleID);
                    gvAlertConfiguration.Visible = false;
                    btnSave.Visible = false;
                    lbtnCancel.Visible = false;
                }
            }
            catch (Exception exp)
            {                
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long eventID = ddlEvent.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlEvent.SelectedItem.Value);
                if (eventID == -1)
                {
                    btnSave.Visible = false;
                    lbtnCancel.Visible = false;
                    gvAlertConfiguration.Visible = false;
                }
                else
                {
                    BindAlertConfigurationGrid(eventID);
                    btnSave.Visible = true;
                    lbtnCancel.Visible = true;

                    if (!base.CanEdit)
                    {
                        btnSave.Visible = false;
                        lbtnCancel.Visible = false;
                    }
                    gvAlertConfiguration.Visible = true;
                }
            }
            catch (Exception exp)
            {                
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindDropdownLists()
        {
            try
            {
                Dropdownlist.DDLGetModules(ddlModule);
                Dropdownlist.DDLNotificationEvents(ddlEvent);
            }
            catch (Exception exp)
            {                
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindAlertConfigurationGrid(long _eventID)
        {
            try
            {
                List<dynamic> lstDesignations = new AlertConfigurationBLL().GetDefaultAlertConfigurations(_eventID);
                gvAlertConfiguration.DataSource = lstDesignations;
                gvAlertConfiguration.DataBind();
            }
            catch (Exception exp)
            {                
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        /// <summary>
        /// This function sets page title.
        /// Created On : 19/01/2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AlertConfiguration);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                long eventID = ddlEvent.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlEvent.SelectedItem.Value);
                AlertConfigurationBLL bllAlertConfiguration = new AlertConfigurationBLL();

                bool isRecordSaved = false;

                for (int rowIndex = 0; rowIndex < gvAlertConfiguration.Rows.Count; rowIndex++)
                {
                    DataKey key = gvAlertConfiguration.DataKeys[rowIndex];
                    long NotificationConfigID = Convert.ToInt64(key.Values["NotificationConfigID"]);
                    long DesignationID = Convert.ToInt64(key.Values["DesignationID"]);
                    bool chkNotification = ((CheckBox)gvAlertConfiguration.Rows[rowIndex].Cells[2].FindControl("chkNotification")).Checked;
                    bool chkSms = ((CheckBox)gvAlertConfiguration.Rows[rowIndex].Cells[3].FindControl("chkSMS")).Checked;
                    bool chkEmail = ((CheckBox)gvAlertConfiguration.Rows[rowIndex].Cells[4].FindControl("chkEmail")).Checked;

                    UA_NotificationConfiguration mdlNotificationConfiguration = new UA_NotificationConfiguration();
                    mdlNotificationConfiguration.ID = NotificationConfigID;
                    mdlNotificationConfiguration.NotificationEventID = eventID;
                    mdlNotificationConfiguration.DesignationID = DesignationID;
                    mdlNotificationConfiguration.Alert = Convert.ToBoolean(chkNotification);
                    mdlNotificationConfiguration.SMS = Convert.ToBoolean(chkSms);
                    mdlNotificationConfiguration.Email = Convert.ToBoolean(chkEmail);

                    isRecordSaved = bllAlertConfiguration.AddNotificationCofiguration(mdlNotificationConfiguration);

                    if (isRecordSaved)
                    {
                        ddlEvent.SelectedIndex = 0;
                        gvAlertConfiguration.Visible = false;
                        btnSave.Visible = false;
                        lbtnCancel.Visible = false;
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    else
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                long eventID = ddlEvent.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlEvent.SelectedItem.Value);
                if (eventID == -1)
                {
                    btnSave.Visible = false;
                    lbtnCancel.Visible = false;
                    gvAlertConfiguration.Visible = false;
                }
                else
                {
                    BindAlertConfigurationGrid(eventID);
                    btnSave.Visible = true;
                    lbtnCancel.Visible = true;

                    if (!base.CanEdit)
                    {
                        btnSave.Visible = false;
                        lbtnCancel.Visible = false;
                    }
                    gvAlertConfiguration.Visible = true;
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
    }
}