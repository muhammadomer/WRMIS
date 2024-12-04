using PMIU.WRMIS.Web.Modules.WaterTheft.Controls;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Model;
using System.Data;
using PMIU.WRMIS.BLL.Notifications;

namespace PMIU.WRMIS.Web.Modules.WaterTheft
{
    public partial class ZiladaarOutletWorking : BasePage
    {
        private const string BasePath = "~/Modules/WaterTheft/Controls/";
        long DesID = -1;
        ControlToLoad Control = ControlToLoad.Channel;
        private string LastLoadedControl
        {
            get { return Convert.ToString(ViewState["LastLoadedControl"]); }
            set { ViewState["LastLoadedControl"] = value; }
        }
        private enum ControlToLoad
        {
            Channel = 1,
            Outlet
        }

        List<WT_WaterTheftOffender> lstOffenders = new List<WT_WaterTheftOffender>();

        //#region Viewstate

        //public string WTCaseID_VS = "CaseID";

        //#endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTitle();
                WaterTheftCase.GetWaterTheftCaseAssignee(Convert.ToInt64(Request.QueryString["WaterTheftID"]));
                hdnDateOfChecking.Value = WaterTheftCase.IncidentDateTime.ToString("MM/dd/yyyy");
                BindUnits();
                HeaderInformation(Convert.ToInt64(Request.QueryString["WaterTheftID"]));

                if (Request.QueryString["RP"] != null)
                    DesID = Convert.ToInt64(Request.QueryString["RP"]);

                ShowSavedInformation();
            }

            //if (Request.QueryString["OS"] != null)
            //{
            //    if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
            //        Control = ControlToLoad.Channel;
            //}

            if ((int)WaterTheftCase.ControlToLoad.Outlet == Convert.ToInt32(WaterTheftCase.OffenceSite))
                Control = ControlToLoad.Outlet;

            LoadWaterTheftUserControl(Control);
            txtComments.Attributes.Add("maxlength", "250");
        }

        private string GetWaterTheftIncident(dynamic _WaterTheftIncident, string _PropertyName)
        {
            return Convert.ToString(_WaterTheftIncident.GetType().GetProperty(_PropertyName).GetValue(_WaterTheftIncident, null));
        }

        public void ShowSavedInformation()
        {
            try
            {
                object ZiladaarWorking = new WaterTheftBLL().GetSavedZiladaarWorking(WaterTheftCase.WaterTheftID);
                if (ZiladaarWorking != null)
                {
                    txtAreaBooked.Text = Convert.ToString(ZiladaarWorking.GetType().GetProperty("AreaBooked").GetValue(ZiladaarWorking));
                    //txtAreaBooked.Enabled = false;
                    string Unit = Convert.ToString(ZiladaarWorking.GetType().GetProperty("Unit").GetValue(ZiladaarWorking));
                    ddlUnit.ClearSelection();
                    ddlUnit.Items.FindByText(Unit).Selected = true;
                    // ddlUnit.Enabled = false;
                    txtNoOfAccused.Text = Convert.ToString(ZiladaarWorking.GetType().GetProperty("NoOfAccused").GetValue(ZiladaarWorking));
                    txtNoOfAccused.Enabled = false;
                    txtProcessedDate.Text = Convert.ToString(ZiladaarWorking.GetType().GetProperty("ProcessedDate").GetValue(ZiladaarWorking));
                    spnProcessedDate.Attributes.Add("class", "input-group-addon");
                    //  txtProcessedDate.Enabled = false;
                   // txtComments.Text = Convert.ToString(ZiladaarWorking.GetType().GetProperty("Comments").GetValue(ZiladaarWorking));
                    // txtComments.Enabled = false;
                }
                else
                    txtProcessedDate.Text = Utility.GetFormattedDate(DateTime.Now);

                GetSavedOffenders();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void GetSavedOffenders()
        {
            gvOffender.DataSource = new WaterTheftBLL().GetOffenders(WaterTheftCase.WaterTheftID);
            gvOffender.DataBind();
        }

        private void LoadWaterTheftUserControl(ControlToLoad _ControlToLoad = ControlToLoad.Outlet)
        {
            string controlPath = string.Empty;
            switch (_ControlToLoad)
            {
                case ControlToLoad.Channel:
                    controlPath = BasePath + "ChannelWaterTheftIncident.ascx";
                    break;
                case ControlToLoad.Outlet:
                    controlPath = BasePath + "OutletWaterTheftIncident.ascx";
                    break;
            }

            WaterTheftIncidentInformation.Controls.Clear();
            UserControl userControl = (UserControl)LoadControl(controlPath);
            WaterTheftIncidentInformation.Controls.Add(userControl);
        }

        public void HeaderInformation(long _WTCaseID)
        {
            try
            {
                //object ParentInfo = new WaterTheftBLL().GetZiladaarWorkingParentInformation(Convert.ToInt64(_CaseID));   // case id to be provided from search screen 

                //if (ParentInfo != null)
                //{
                //    lblChnlName.Text = Convert.ToString(ParentInfo.GetType().GetProperty("ChannelName").GetValue(ParentInfo));
                //    lblOutlet.Text = Convert.ToString(ParentInfo.GetType().GetProperty("Outlet").GetValue(ParentInfo));
                //    lblType.Text = Convert.ToString(ParentInfo.GetType().GetProperty("OutletType").GetValue(ParentInfo));
                //    lblRD.Text = Convert.ToString(ParentInfo.GetType().GetProperty("RD").GetValue(ParentInfo));
                //    lblSide.Text = Convert.ToString(ParentInfo.GetType().GetProperty("Side").GetValue(ParentInfo));
                //    lblTime.Text = Convert.ToString(ParentInfo.GetType().GetProperty("IncidentDate").GetValue(ParentInfo));
                //    lblDate.Text = Convert.ToString(ParentInfo.GetType().GetProperty("IncidentTime").GetValue(ParentInfo));
                //    lblSMSDate.Text = Convert.ToString(ParentInfo.GetType().GetProperty("SMSDate").GetValue(ParentInfo));
                //    lblTheftType.Text = Convert.ToString(ParentInfo.GetType().GetProperty("TheftType").GetValue(ParentInfo));
                //    lblConditonOutlet.Text = Convert.ToString(ParentInfo.GetType().GetProperty("ConditionOfOutlet").GetValue(ParentInfo));
                //    lblH.Text = Convert.ToString(ParentInfo.GetType().GetProperty("H").GetValue(ParentInfo));
                //    lblDefectiveType.Text = Convert.ToString(ParentInfo.GetType().GetProperty("Defectivetype").GetValue(ParentInfo));
                //    lblB.Text = Convert.ToString(ParentInfo.GetType().GetProperty("B").GetValue(ParentInfo));
                //    lblY.Text = Convert.ToString(ParentInfo.GetType().GetProperty("Y").GetValue(ParentInfo));
                //    lblDIA.Text = Convert.ToString(ParentInfo.GetType().GetProperty("DIA").GetValue(ParentInfo));
                //    lblRemarks.Text = Convert.ToString(ParentInfo.GetType().GetProperty("Remarks").GetValue(ParentInfo));
                //}

                object ObjWorking = new WaterTheftBLL().GetFineCalculation(_WTCaseID);
                if (ObjWorking != null)
                {
                    txtAreaBooked.Text = Convert.ToString(ObjWorking.GetType().GetProperty("AreaBooked").GetValue(ObjWorking));
                    string Unit = Convert.ToString(ObjWorking.GetType().GetProperty("AreaName").GetValue(ObjWorking));
                    ddlUnit.Items.FindByText(Unit).Selected = true;
                    txtNoOfAccused.Text = Convert.ToString(ObjWorking.GetType().GetProperty("NoOfAccussed").GetValue(ObjWorking));
                    txtNoOfAccused.Enabled = false;
                    txtProcessedDate.Text = Convert.ToString(ObjWorking.GetType().GetProperty("ProcessedDate").GetValue(ObjWorking));
                    txtComments.Text = Convert.ToString(ObjWorking.GetType().GetProperty("ZiladaarRemarks").GetValue(ObjWorking));
                }

                lstOffenders = new WaterTheftBLL().GetOffenders(_WTCaseID);
                if (lstOffenders != null)
                {
                    gvOffender.DataSource = lstOffenders;
                    gvOffender.DataBind();
                    txtNoOfAccused.Text = lstOffenders.Count().ToString();
                    txtNoOfAccused.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        public void BindUnits()
        {
            try
            {
                ddlUnit.DataSource = new WaterTheftBLL().GetAreaTypes();
                ddlUnit.DataValueField = "ID";
                ddlUnit.DataTextField = "Name";
                ddlUnit.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvOffender.EditIndex = -1;
                if (Request.QueryString["OS"] != null)
                {
                    if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
                        Control = ControlToLoad.Channel;
                }
                LoadWaterTheftUserControl(Control);
                GetSavedOffenders();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOffender.PageIndex = e.NewPageIndex;
                if (Request.QueryString["OS"] != null)
                {
                    if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
                        Control = ControlToLoad.Channel;
                }
                LoadWaterTheftUserControl(Control);
                GetSavedOffenders();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstOffenders = new WaterTheftBLL().GetOffenders(WaterTheftCase.WaterTheftID);

                    WT_WaterTheftOffender Offender = new WT_WaterTheftOffender();

                    Offender.ID = -1;
                    Offender.OffenderName = "";
                    Offender.CNIC = "";
                    Offender.Address = "";
                    lstOffenders.Add(Offender);
                    gvOffender.PageIndex = gvOffender.PageCount;
                    gvOffender.DataSource = lstOffenders;
                    gvOffender.DataBind();
                    gvOffender.EditIndex = gvOffender.Rows.Count - 1;
                    gvOffender.DataBind();

                    if (Request.QueryString["OS"] != null)
                    {
                        if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
                            Control = ControlToLoad.Channel;
                    }
                    LoadWaterTheftUserControl(Control);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOffender.EditIndex = -1;
                if (Request.QueryString["OS"] != null)
                {
                    if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
                        Control = ControlToLoad.Channel;
                }
                LoadWaterTheftUserControl(Control);
                GetSavedOffenders();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                //if (mdlRoleRights != null)
                //{
                //    if (e.Row.RowType == DataControlRowType.Header)
                //    {
                //        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                //        if (btnAdd != null)
                //        {
                //            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                //        }
                //    }
                //    else if (e.Row.RowType == DataControlRowType.DataRow)
                //    {
                //        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                //        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

                //        if (btnEdit != null)
                //        {
                //            btnEdit.Visible = (bool)mdlRoleRights.BEdit;
                //        }

                //        if (btnDelete != null)
                //        {
                //            btnDelete.Visible = (bool)mdlRoleRights.BDelete;
                //        }
                //    }

                //    btnAssignSDO.Visible = (bool)mdlRoleRights.BAdd;
                //}
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOffender.EditIndex = e.NewEditIndex;
                if (Request.QueryString["OS"] != null)
                {
                    if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
                        Control = ControlToLoad.Channel;
                }
                LoadWaterTheftUserControl(Control);
                GetSavedOffenders();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long ID = Convert.ToInt32(((Label)gvOffender.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool result = new WaterTheftBLL().DeleteOffender(ID, WaterTheftCase.WaterTheftID);
                if (result)
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                if (Request.QueryString["OS"] != null)
                {
                    if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
                        Control = ControlToLoad.Channel;
                }
                LoadWaterTheftUserControl(Control);
                SetOffendersGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int ID = Convert.ToInt32(((Label)gvOffender.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                string Name = ((TextBox)gvOffender.Rows[rowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string CNIC = ((TextBox)gvOffender.Rows[rowIndex].Cells[2].FindControl("txtCNIC")).Text.Trim();
                string Address = ((TextBox)gvOffender.Rows[rowIndex].Cells[2].FindControl("txtAddress")).Text.Trim();

                if (Name != "" && Address != "")
                {
                    WT_WaterTheftOffender ObjSave = new WT_WaterTheftOffender();
                    ObjSave.WaterTheftID = WaterTheftCase.WaterTheftID;
                    ObjSave.OffenderName = Name;
                    ObjSave.CNIC = CNIC;
                    ObjSave.Address = Address;

                    if (ID == -1)  // add new record
                    {
                        new WaterTheftBLL().AddOffender(ObjSave);
                        gvOffender.PageIndex = 0;
                        gvOffender.EditIndex = -1;
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    else  // update record 
                    {
                        ObjSave.ID = ID;
                        new WaterTheftBLL().UpdateOffender(ObjSave);
                        gvOffender.PageIndex = 0;
                        gvOffender.EditIndex = -1;
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }

                    if (Request.QueryString["OS"] != null)
                    {
                        if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
                            Control = ControlToLoad.Channel;
                    }
                    LoadWaterTheftUserControl(Control);
                    SetOffendersGrid();
                }
                else
                {
                    Master.ShowMessage(Message.RequiredFields.Description, SiteMaster.MessageType.Success);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void SetOffendersGrid()
        {
            try
            {
                lstOffenders = new WaterTheftBLL().GetOffenders(WaterTheftCase.WaterTheftID);
                gvOffender.DataSource = lstOffenders;
                gvOffender.DataBind();
                txtNoOfAccused.Text = lstOffenders.Count().ToString();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAssignSDO_Click(object sender, EventArgs e)
        {
            try
            {
                int? Result = new WaterTheftBLL().SaveZiladaarWorking(Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), WaterTheftCase.WaterTheftID,
                     Convert.ToDouble(txtAreaBooked.Text), Convert.ToInt64(ddlUnit.SelectedItem.Value), Convert.ToInt32(txtNoOfAccused.Text), Convert.ToDateTime(txtProcessedDate.Text), txtComments.Text);
                if (Result == 3)
                {
                    List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.WaterTheft);
                    DataTable WTAttachmentTable = WaterTheftCase.GetDataTable(WaterTheftCase.WaterTheftID, Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);                 
                    new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.ZiladarAssignToSDO, SessionManagerFacade.UserInformation.ID);
                    Response.Redirect("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true",false);                    
                }
                else
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                if (Request.QueryString["OS"] != null)
                {
                    if (Convert.ToInt32(Request.QueryString["OS"]) == 1)
                        Control = ControlToLoad.Channel;
                }
                LoadWaterTheftUserControl(Control);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        //protected void btnBack_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Ziladaar);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

    }
}