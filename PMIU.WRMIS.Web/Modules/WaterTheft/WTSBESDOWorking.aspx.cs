using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Modules.WaterTheft.Controls;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.WaterTheft;
using System.Data;
using PMIU.WRMIS.BLL.Notifications;
namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class WTSBESDOWorking : BasePage
    {
        private const string BasePath = "~/Modules/WaterTheft/Controls/";
        private string CurrentControl
        {
            get { return ViewState["CurrentControl"] == null ? string.Empty : Convert.ToString(ViewState["CurrentControl"]); }
            set { ViewState["CurrentControl"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            WaterTheftCase.ControlToLoad controlToLoad = WaterTheftCase.ControlToLoad.Channel;
            try
            {
                if (!IsPostBack)
                {
                    hlBack.NavigateUrl = string.Format("~/Modules/WaterTheft/SearchWaterTheft.aspx?ShowHistory=true");

                    if (string.IsNullOrEmpty(Request.QueryString["ET"]) && // shows it comes from Water theft module else Employee tracking page                         
                        !string.IsNullOrEmpty(Request.QueryString["WaterTheftID"]))
                    {
                        WaterTheftCase.GetWaterTheftCaseAssignee(Convert.ToInt64(Request.QueryString["WaterTheftID"]));
                        hdnDateOfChecking.Value = WaterTheftCase.IncidentDateTime.ToString("MM/dd/yyyy");
                        SetPageTitle();
                        DisplaySDOWorkingFields();
                        txtCanalWireDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                        txtClosingRepairDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                        PrepopulateSBESDOWorking();

                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["ET"]) && !string.IsNullOrEmpty(Request.QueryString["WaterTheftID"]))
                    {
                        hlBack.NavigateUrl = string.Format("~/Modules/DailyData/MeterReadingAndFuel.aspx?ShowSearched=true");
                        WaterTheftCase.GetWaterTheftCaseAssignee(Convert.ToInt64(Request.QueryString["WaterTheftID"]));
                        // page title
                        Master.ModuleTitle = "Water Theft";
                        pageTitleID.InnerText = "Case Information";
                        //
                        WorkingFieldsID.Visible = false; // Hide Closing/Repair date field
                        btnAssignToSDO.Visible = false; // Hide Assign to SDO button
                        pnlIsSDOWorking.Visible = false;
                        hWorkingTitle.Visible = false;
                    }
                }
                if ((int)WaterTheftCase.ControlToLoad.Outlet == Convert.ToInt32(WaterTheftCase.OffenceSite))
                    controlToLoad = WaterTheftCase.ControlToLoad.Outlet;
                LoadWaterTheftUserControl(controlToLoad);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            long designationID = WaterTheftCase.AssignedToDesignationID; //SessionManagerFacade.UserInformation.DesignationID.Value;
            Tuple<string, string, string> pageTitle = null;

            if (designationID == (long)Constants.Designation.SBE)
            {
                pageTitle = base.SetPageTitle(PageName.SBEWorking);
                hWorkingTitle.InnerText = "SBE Working";
            }
            else if (designationID == (long)Constants.Designation.SDO)
            {
                pageTitle = base.SetPageTitle(PageName.SDOWorking);
                hWorkingTitle.InnerText = "SDO Working";

            }
            else
            {
                pageTitle = base.SetPageTitle(PageName.SearchWaterTheft);
            }
            if (pageTitle != null)
            {
                Master.ModuleTitle = pageTitle.Item1;
                pageTitleID.InnerText = pageTitle.Item2;
                Master.NavigationBar = pageTitle.Item3;
            }
        }
        private void DisplaySDOWorkingFields()
        {

            // UA_RoleRights RR = new WaterTheftBLL().GetRoleRights(Convert.ToInt64(WaterTheftCase.AssignedToUserID), 96);  // 96 is page id 




            if ((int)Constants.WTCaseStatus.NA == Convert.ToInt32(WaterTheftCase.CaseStatusID))
            {
                WorkingFieldsID.Visible = false;
                btnAssignToSDO.Visible = false;
                pnlIsSDOWorking.Visible = false;
                hWorkingTitle.Visible = false;
            }
            else if ((int)Constants.Designation.SBE == SessionManagerFacade.UserInformation.DesignationID.Value && (int)Constants.Designation.SBE == Convert.ToInt32(WaterTheftCase.AssignedToDesignationID))
            {
                WorkingFieldsID.Visible = true;
                pnlIsSDOWorking.Visible = false;

                if (base.CanAdd == true) //(RR.BAdd == true)
                    btnAssignToSDO.Visible = true;
                else
                    btnAssignToSDO.Visible = false;
            }
            else if ((int)Constants.Designation.SDO == SessionManagerFacade.UserInformation.DesignationID.Value && (int)Constants.Designation.SDO == Convert.ToInt32(WaterTheftCase.AssignedToDesignationID))
            {
                DivClosingRepairDate.Visible = false; // Hide Closing/Repair date field
                btnAssignToSDO.Visible = false; // Hide Assign to SDO button
                if (base.CanAdd == true)//(RR.BAdd == true)
                    pnlIsSDOWorking.Visible = true;
                else
                    pnlIsSDOWorking.Visible = false;
            }
            else
            {
                WorkingFieldsID.Visible = false; // Hide Closing/Repair date field
                btnAssignToSDO.Visible = false; // Hide Assign to SDO button
                pnlIsSDOWorking.Visible = false;
                hWorkingTitle.Visible = false;
            }
        }
        private void LoadWaterTheftUserControl(WaterTheftCase.ControlToLoad _ControlToLoad = WaterTheftCase.ControlToLoad.Channel)
        {
            string controlPath = string.Empty;
            switch (_ControlToLoad)
            {
                case WaterTheftCase.ControlToLoad.Channel:
                    controlPath = BasePath + "ChannelWaterTheftIncident.ascx";
                    break;
                case WaterTheftCase.ControlToLoad.Outlet:
                    controlPath = BasePath + "OutletWaterTheftIncident.ascx";
                    break;
            }
            PhWaterTheftIncidentInformation.Controls.Clear();
            UserControl userControl = (UserControl)LoadControl(controlPath);
            PhWaterTheftIncidentInformation.Controls.Add(userControl);
            this.CurrentControl = controlPath;
        }
        protected void btnAssignToSDO_Click(object sender, EventArgs e)
        {
            try
            {
                WT_WaterTheftCanalWire WTCanalWire = GetWTCanalWireEntity();
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                long UserID = mdlUser.ID;
                long DesignationID = Convert.ToInt64(mdlUser.DesignationID);

                if (WTCanalWire.CanalWireDate != null && WaterTheftCase.IncidentDateTime != null && WTCanalWire.CanalWireDate < Convert.ToDateTime(Utility.GetFormattedDate(WaterTheftCase.IncidentDateTime)))
                {
                    Master.ShowMessage("Date should be greater then " + Utility.GetFormattedDate(WaterTheftCase.IncidentDateTime), SiteMaster.MessageType.Error);
                    txtCanalWireDate.Text = string.Empty;
                    return;
                }
                if (!string.IsNullOrEmpty(txtClosingRepairDate.Text) && WaterTheftCase.IncidentDateTime != null && Utility.GetParsedDate(txtClosingRepairDate.Text.Trim()) < Convert.ToDateTime(Utility.GetFormattedDate(WaterTheftCase.IncidentDateTime)))
                {
                    Master.ShowMessage("Date should be greater then " + Utility.GetFormattedDate(WaterTheftCase.IncidentDateTime), SiteMaster.MessageType.Error);
                    txtClosingRepairDate.Text = string.Empty;
                    return;
                }

                int isAssigned = new WaterTheftBLL().AssignWaterTheftCaseToSDO(WTCanalWire, txtClosingRepairDate.Text);
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.WaterTheft);
                DataTable WTAttachmentTable = WaterTheftCase.GetDataTable(WaterTheftCase.WaterTheftID, UserID, DesignationID, lstNameofFiles, "W", 0, 0);
                //DataSet ds = FileUploadControl.UploadNow(WaterTheftCase.WaterTheftID, UserID, DesignationID);
                //DataTable WTAttachmentTable = ds.Tables[0];

                int StatusID = new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);

                if (isAssigned == 1)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.SBEAssignedToSDO, SessionManagerFacade.UserInformation.ID);
                    Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true");
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private WT_WaterTheftCanalWire GetWTCanalWireEntity()
        {
            WT_WaterTheftCanalWire WTCanalWire = new WT_WaterTheftCanalWire();
            DateTime? canalWireDate = null;
            string canalWireNo = null;
            if (!string.IsNullOrEmpty(txtCanalWireDate.Text.Trim()))
                canalWireDate = Utility.GetParsedDate(txtCanalWireDate.Text.Trim());
            if (!string.IsNullOrEmpty(txtCanalWireNo.Text))
                canalWireNo = Convert.ToString(txtCanalWireNo.Text.Trim());

            WTCanalWire.WaterTheftID = WaterTheftCase.WaterTheftID;
            WTCanalWire.CanalWireNo = canalWireNo;
            WTCanalWire.CanalWireDate = canalWireDate;
            WTCanalWire.Remarks = txtComments.Text.Trim();
            WTCanalWire.UserID = Convert.ToInt64(Session[SessionValues.UserID]);
            WTCanalWire.ID = Convert.ToInt64(hdnCanalWireID.Value);
            return WTCanalWire;
        }
        protected void btnMarkCaseNA_Click(object sender, EventArgs e)
        {
            try
            {
                WT_WaterTheftCanalWire WTCanalWire = GetWTCanalWireEntity();
                WTCanalWire.DesignationID = (long)Constants.Designation.SBE;
                int isMarkedASNA = new WaterTheftBLL().MarkWaterTheftCaseAsNA(WTCanalWire);
                if (isMarkedASNA == 1)
                    Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true");
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnAssignToZiladar_Click(object sender, EventArgs e)
        {
            try
            {
                WT_WaterTheftCanalWire WTCanalWire = GetWTCanalWireEntity();
                WTCanalWire.DesignationID = (long)Constants.Designation.Ziladaar;

                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                long UserID = mdlUser.ID;
                long DesignationID = Convert.ToInt64(mdlUser.DesignationID);

                if (WTCanalWire.CanalWireDate != null && WaterTheftCase.IncidentDateTime != null && WTCanalWire.CanalWireDate < Convert.ToDateTime(Utility.GetFormattedDate(WaterTheftCase.IncidentDateTime)))
                {
                    Master.ShowMessage("Date should be greater then " + Utility.GetFormattedDate(WaterTheftCase.IncidentDateTime), SiteMaster.MessageType.Error);
                    txtCanalWireDate.Text = string.Empty;
                    return;
                }
                int isAssigned = new WaterTheftBLL().AssignWaterTheftCaseToSBEOrZiladar(WTCanalWire);

                //DataSet ds = FileUploadControl.UploadNow(WaterTheftCase.WaterTheftID, UserID, DesignationID);
                //DataTable WTAttachmentTable = ds.Tables[0];
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.WaterTheft);
                DataTable WTAttachmentTable = WaterTheftCase.GetDataTable(WaterTheftCase.WaterTheftID, UserID, DesignationID, lstNameofFiles, "W", 0, 0);

                int StatusID = new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);

                if (isAssigned == 1)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.SDOAssignedTOZiladaar, SessionManagerFacade.UserInformation.ID);
                    Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true");
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnAssignToSBE_Click(object sender, EventArgs e)
        {
            try
            {
                WT_WaterTheftCanalWire WTCanalWire = GetWTCanalWireEntity();
                WTCanalWire.DesignationID = (long)Constants.Designation.SBE;
                if (WTCanalWire.CanalWireDate != null && WaterTheftCase.IncidentDateTime != null && WTCanalWire.CanalWireDate < Convert.ToDateTime(Utility.GetFormattedDate(WaterTheftCase.IncidentDateTime)))
                {
                    Master.ShowMessage("Date should be greater then " + Utility.GetFormattedDate(WaterTheftCase.IncidentDateTime), SiteMaster.MessageType.Error);
                    txtCanalWireDate.Text = string.Empty;
                    return;
                }

                //Bug fix before UAT 
                //int isAssigned = new WaterTheftBLL().AssignWaterTheftCaseToSBEOrZiladar(WTCanalWire);
                int? isAssigned = new WaterTheftBLL().InsertCaseStatusForSubOrdinate(WaterTheftCase.WaterTheftID, Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), txtComments.Text.Trim(), (Int64)Constants.Designation.SBE);

                if (isAssigned == 2) // 2 means success :)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.SDOAssignedBackSBE, SessionManagerFacade.UserInformation.ID);
                    Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true");
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void PrepopulateSBESDOWorking()
        {
            try
            {
                dynamic bllSBEWorking = new WaterTheftBLL().GetSBESDOWorkingByWaterTheftID(WaterTheftCase.WaterTheftID, SessionManagerFacade.UserInformation.DesignationID.Value);
                if (bllSBEWorking != null)
                {
                    txtCanalWireNo.Text = Utility.GetDynamicPropertyValue(bllSBEWorking, "CanalWireNo");
                    txtCanalWireDate.Text = Utility.GetDynamicPropertyValue(bllSBEWorking, "CanalWireDate");
                    txtClosingRepairDate.Text = Utility.GetDynamicPropertyValue(bllSBEWorking, "ClosingRepairDate");
                    hdnCanalWireID.Value = Utility.GetDynamicPropertyValue(bllSBEWorking, "CanalWireID");
                    DisableWorkingFields();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void DisableWorkingFields()
        {
            if ((int)Constants.Designation.SBE == SessionManagerFacade.UserInformation.DesignationID.Value && (int)Constants.Designation.SBE == Convert.ToInt32(WaterTheftCase.AssignedToDesignationID))
            {
                txtCanalWireNo.Enabled = false;
                txtCanalWireDate.Enabled = false;
                txtClosingRepairDate.Enabled = false;
                RemoveRequiredClass();
            }
            else if ((int)Constants.Designation.SDO == SessionManagerFacade.UserInformation.DesignationID.Value && (int)Constants.Designation.SDO == Convert.ToInt32(WaterTheftCase.AssignedToDesignationID))
            {
                if (!string.IsNullOrEmpty(txtCanalWireNo.Text.Trim()))
                    txtCanalWireNo.Enabled = false;
                else
                    txtCanalWireNo.Enabled = true;

                if (!string.IsNullOrEmpty(txtCanalWireDate.Text.Trim()))
                    txtCanalWireDate.Enabled = false;
                else
                {
                    txtCanalWireDate.Enabled = true;
                    txtCanalWireDate.Text = Convert.ToString(Utility.GetFormattedDate(DateTime.Now));
                }

                RemoveRequiredClass();
            }
            else
            {
                txtCanalWireNo.Enabled = true;
                txtCanalWireDate.Enabled = true;
                txtClosingRepairDate.Enabled = true;
            }
        }
        private void RemoveRequiredClass()
        {
            txtCanalWireNo.CssClass = "form-control";
            txtCanalWireDate.CssClass = "form-control";
            txtClosingRepairDate.CssClass = "form-control";
        }
    }
}