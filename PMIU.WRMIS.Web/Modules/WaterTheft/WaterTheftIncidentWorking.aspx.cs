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
    public partial class WaterTheftIncidentWorking : BasePage
    {
        #region ViewState

        public string UserControlSelection_VS = "UserControlSelection";

        #endregion

        private const string BasePath = "~/Modules/WaterTheft/Controls/";

        List<WT_WaterTheftOffender> lstOffenders = new List<WT_WaterTheftOffender>();

        private string LastLoadedControl
        {
            get { return Convert.ToString(ViewState["LastLoadedControl"]); }
            set { ViewState["LastLoadedControl"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WaterTheftCase.ControlToLoad Control = WaterTheftCase.ControlToLoad.Outlet;

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["WaterTheftID"]) && !string.IsNullOrEmpty(Request.QueryString["AA"]))
                {
                    TawaanWorking.AA = Convert.ToInt64(Request.QueryString["AA"]);
                    TawaanWorking.WTCaseID = Convert.ToInt64(Request.QueryString["WaterTheftID"]);
                    WaterTheftCase.GetWaterTheftCaseAssignee(Convert.ToInt64(Request.QueryString["WaterTheftID"]));
                    SetAccordingToDesignation(WaterTheftCase.AssignedToDesignationID);
                    SetTitle(WaterTheftCase.AssignedToDesignationID);
                }
            }

            if ((int)WaterTheftCase.ControlToLoad.Channel == Convert.ToInt32(WaterTheftCase.OffenceSite))
                Control = WaterTheftCase.ControlToLoad.Channel;
            LoadWaterTheftUserControl(Control);
        }
        /// <summary>
        /// This function load page according to designation of 
        /// user i.e. show hide details that are irrelevent to user 
        /// </summary>
        /// <param name="_DesigntionID"></param>
        public void SetAccordingToDesignation(long _DesigntionID)
        {
            try
            {
                //UA_RoleRights RR = new WaterTheftBLL().GetRoleRights(Convert.ToInt64(WaterTheftCase.AssignedToUserID), 97);  // 97 is page id 

                if (SessionManagerFacade.UserInformation.DesignationID != null && WaterTheftCase.CaseStatusID != 1)  // when case is closed it is for view only 
                {
                    if (SessionManagerFacade.UserInformation.DesignationID.Value != _DesigntionID)
                    {
                        divNone.Visible = true;
                    }
                    else if (_DesigntionID == (long)Constants.Designation.SDO)
                    {
                        if (base.CanAdd) //(RR.BAdd == true)
                            divSDOBtn.Visible = true;
                        else
                            divSDOBtn.Visible = false;
                    }
                    else if (_DesigntionID == (long)Constants.Designation.XEN)
                    {
                        if (base.CanAdd) //(RR.BAdd == true)
                            divXENBtn.Visible = true;
                        else
                            divXENBtn.Visible = false;
                    }
                    else if (_DesigntionID == (long)Constants.Designation.SE)
                    {
                        if (base.CanAdd) //(RR.BAdd == true)
                            divSEBtn.Visible = true;
                        else
                            divSEBtn.Visible = false;

                    }
                    else if (_DesigntionID == (long)Constants.Designation.ChiefIrrigation)
                    {
                        if (base.CanAdd) //(RR.BAdd == true)
                            divChiefBtn.Visible = true;
                        else
                            divChiefBtn.Visible = false;
                    }
                }
                else if ((!string.IsNullOrEmpty(Request.QueryString["AA"])) && (Convert.ToInt32(Request.QueryString["AA"]) != 0))  //scenario: when case is closed and SE or chief is opening it for appeal/ assignback 
                {
                    if (SessionManagerFacade.UserInformation.DesignationID.Value == (long)Constants.Designation.SE)
                    {
                        if (base.CanAdd) //(RR.BAdd == true)
                            divSEBtn.Visible = true;
                        else
                            divSEBtn.Visible = false;
                    }
                    else if (SessionManagerFacade.UserInformation.DesignationID.Value == (long)Constants.Designation.ChiefIrrigation)
                    {
                        if (base.CanAdd) //(RR.BAdd == true)
                            divChiefBtn.Visible = true;
                        else
                            divChiefBtn.Visible = false;
                    }
                }
                else
                {
                    divNone.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void LoadWaterTheftUserControl(WaterTheftCase.ControlToLoad _ControlToLoad = WaterTheftCase.ControlToLoad.Outlet)
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

            WaterTheftIncidentInformation.Controls.Clear();
            UserControl userControl = (UserControl)LoadControl(controlPath);
            WaterTheftIncidentInformation.Controls.Add(userControl);
        }

        private void SetTitle(long _DesignationID)
        {
            if (WaterTheftCase.CaseStatusID == (long)Constants.WTCaseStatus.Closed &&    // when case is close and SE opens it for appeal/ assign back 
                Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID) == (long)Constants.Designation.SE)
            {
                Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SEWorking);
                Master.ModuleTitle = pageTitle.Item1;
                Master.PageTitle = pageTitle.Item2;
                Master.NavigationBar = pageTitle.Item3;
                hTitle.InnerText = "SE Working";
            }
            else if (WaterTheftCase.CaseStatusID == (long)Constants.WTCaseStatus.Closed &&  // when case is close and chief opens it for appeal/ assign back 
                Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID) == (long)Constants.Designation.ChiefIrrigation)
            {
                Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChiefWorking);
                Master.ModuleTitle = pageTitle.Item1;
                Master.PageTitle = pageTitle.Item2;
                Master.NavigationBar = pageTitle.Item3;
                hTitle.InnerText = "Chief Working";
            }
            else if (_DesignationID == (long)Constants.Designation.Ziladaar)
            {
                Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Ziladaar);
                Master.ModuleTitle = pageTitle.Item1;
                Master.PageTitle = pageTitle.Item2;
                Master.NavigationBar = pageTitle.Item3;
                hTitle.InnerText = "Ziladaar Working";
            }
            else if (_DesignationID == (long)Constants.Designation.SDO)
            {
                Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SDOWorking);
                Master.ModuleTitle = pageTitle.Item1;
                Master.PageTitle = pageTitle.Item2;
                Master.NavigationBar = pageTitle.Item3;
                hTitle.InnerText = "SDO Working";
            }
            else if (_DesignationID == (long)Constants.Designation.XEN)
            {
                Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.XENWorking);
                Master.ModuleTitle = pageTitle.Item1;
                Master.PageTitle = pageTitle.Item2;
                Master.NavigationBar = pageTitle.Item3;
                hTitle.InnerText = "XEN Working";
            }
            else if (_DesignationID == (long)Constants.Designation.SE)
            {
                Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SEWorking);
                Master.ModuleTitle = pageTitle.Item1;
                Master.PageTitle = pageTitle.Item2;
                Master.NavigationBar = pageTitle.Item3;
                hTitle.InnerText = "SE Working";
            }
            else if (_DesignationID == (long)Constants.Designation.ChiefIrrigation)
            {
                Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ChiefWorking);
                Master.ModuleTitle = pageTitle.Item1;
                Master.PageTitle = pageTitle.Item2;
                Master.NavigationBar = pageTitle.Item3;
                hTitle.InnerText = "Chief Working";
            }

        }

        protected void btnAssignToZilardaar_Click(object sender, EventArgs e)
        {
            try
            {
                long WTCaseID = Convert.ToInt64(Request.QueryString["WaterTheftID"]);
                long? AssignedToID = new WaterTheftBLL().GetManagerSubOrdinate(Convert.ToInt64(Session[SessionValues.UserID]), (Int64)Constants.Designation.Ziladaar);

                if (AssignedToID != null)
                {
                    TextBox txComments = (TextBox)TawaanWorking.FindControl("taComments");

                    WT_WaterTheftStatus ObjSave = new WT_WaterTheftStatus();
                    ObjSave.WaterTheftID = WTCaseID;
                    ObjSave.AssignedToUserID = (Int64)AssignedToID;
                    ObjSave.AssignedToDesignationID = (Int64)Constants.Designation.Ziladaar;
                    ObjSave.AssignedByUserID = Convert.ToInt64(Session[SessionValues.UserID]);
                    ObjSave.AssignedByDesignationID = Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID);
                    ObjSave.AssignedDate = DateTime.Now;
                    ObjSave.CaseStatusID = new WaterTheftBLL().GetCaseStatus(WTCaseID); //(Int64)Constants.WTCaseStatus.InProgress;
                    ObjSave.Remarks = txComments.Text;
                    bool Result = new WaterTheftBLL().AssignCasetoZiledar(ObjSave);
                    if (Result)
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.SDOAssignedTOZiladaar, SessionManagerFacade.UserInformation.ID);
                        Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true", false);
                    }
                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAssignToXEN_Click(object sender, EventArgs e)
        {
            try
            {
                long? WTCaseID = Convert.ToInt64(Request.QueryString["WaterTheftID"]);
                DropDownList dlDecison = (DropDownList)TawaanWorking.FindControl("ddlDecisiontype");
                string SDecision = dlDecison.SelectedItem.Text;
                TextBox txDecDate = (TextBox)TawaanWorking.FindControl("txtDecisionDate");
                DateTime? DecDate = null;
                if (txDecDate.Text != "")
                    DecDate = Convert.ToDateTime(txDecDate.Text);

                TextBox txSpecialCharges = (TextBox)TawaanWorking.FindControl("txtSpecialCharges");
                int? SpecialCharges = null;
                if (txSpecialCharges.Text != "")
                    SpecialCharges = Convert.ToInt32(txSpecialCharges.Text);
                TextBox txFine = (TextBox)TawaanWorking.FindControl("txtFine");
                double Fine = Convert.ToDouble(txFine.Text);
                TextBox txSDOToPolice = (TextBox)TawaanWorking.FindControl("txtLetterSDOToPolice");
                string SDOTOPolice = txSDOToPolice.Text;
                TextBox txFIR = (TextBox)TawaanWorking.FindControl("txtFirNo");
                string FIR = txFIR.Text;
                TextBox txFIRDate = (TextBox)TawaanWorking.FindControl("txtFIRDate");
                DateTime FIRDate = Convert.ToDateTime(txFIRDate.Text);
                DropDownList txImprisonment = (DropDownList)TawaanWorking.FindControl("ddlImprisonment");

                bool Imprisonment = false;
                int? ImprisonmentDays = null;
                if (txImprisonment.SelectedItem.Text.ToUpper() == "YES")
                {
                    Imprisonment = true;
                    TextBox txImpDays = (TextBox)TawaanWorking.FindControl("txtDays");
                    if (!string.IsNullOrEmpty(txImpDays.Text))
                        ImprisonmentDays = Convert.ToInt32(txImpDays.Text);
                }

                TextBox txXENNo = (TextBox)TawaanWorking.FindControl("txtCaseToXEN");
                string XENNo = txXENNo.Text;

                TextBox txXENDate = (TextBox)TawaanWorking.FindControl("txtCaseToXENDate");
                DateTime XENDate = Convert.ToDateTime(txXENDate.Text);

                TextBox txComments = (TextBox)TawaanWorking.FindControl("taComments");
                string SDOComments = txComments.Text;

                int? Result = new WaterTheftBLL().UpdateTawaanRecord(Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID),
                   WTCaseID, SDecision, DecDate, SpecialCharges, Fine, SDOTOPolice, DateTime.Now, FIR, FIRDate, Imprisonment, ImprisonmentDays, XENNo, XENDate, SDOComments);

                if (Result == 4) // 4 means success
                {
                    WebFormsTest.FileUploadControl FileControl = (WebFormsTest.FileUploadControl)TawaanWorking.FindControl("FileUploadControlCL");
                    List<Tuple<string, string, string>> lstNameofFiles = FileControl.UploadNow(Configuration.WaterTheft);
                    DataTable WTAttachmentTable = WaterTheftCase.GetDataTable((long)WTCaseID, Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);
                    new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);

                    FileControl = (WebFormsTest.FileUploadControl)TawaanWorking.FindControl("FileUploadControlCF");
                    //ds = FileControl.UploadNow((long)WTCaseID, Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID));
                    //WTAttachmentTable = ds.Tables[0];
                    lstNameofFiles = FileControl.UploadNow(Configuration.WaterTheft, "FIRCtrl", 5);
                    WTAttachmentTable = WaterTheftCase.GetDataTable((long)WTCaseID, Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);
                    new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);

                    FileControl = (WebFormsTest.FileUploadControl)TawaanWorking.FindControl("FileUploadControlProof");
                    //ds = FileControl.UploadNow((long)WTCaseID, Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID));
                    //WTAttachmentTable = ds.Tables[0];
                    lstNameofFiles = FileControl.UploadNow(Configuration.WaterTheft, "DesnCtrl", 10);
                    WTAttachmentTable = WaterTheftCase.GetDataTable((long)WTCaseID, Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);
                    new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);

                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                    _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.SDOAssignedToXEN, SessionManagerFacade.UserInformation.ID);

                    Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true", false);
                    //Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    //Result = new WaterTheftBLL().InsertCaseStatus(WTCaseID, Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), SDOComments);

                    //if (Result == 1)
                    //    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    //else
                    //    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
                else
                    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        protected void btnAssignToSDO_Click(object sender, EventArgs e)
        {
            try
            {
                int? SpecialCharges = null;
                TextBox DecisionDate = (TextBox)TawaanWorking.FindControl("txtDecisionDate");
                TextBox txSpecialCharges = (TextBox)TawaanWorking.FindControl("txtSpecialCharges");
                if (txSpecialCharges.Text != "")
                    SpecialCharges = Convert.ToInt32(txSpecialCharges.Text);
                bool Result = new WaterTheftBLL().SaveSepcialCharges(Convert.ToInt64(Request.QueryString["WaterTheftID"]), SpecialCharges, Convert.ToDateTime(DecisionDate.Text), false);
                if (Result)
                {
                    TextBox txComments = (TextBox)TawaanWorking.FindControl("taComments");
                    string XENComments = txComments.Text;
                    int? Status = new WaterTheftBLL().InsertCaseStatusForSubOrdinate(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), XENComments, (Int64)Constants.Designation.SDO);
                    if (Status == 2)
                    {
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.XENAssignedBackToSDO, SessionManagerFacade.UserInformation.ID);
                        Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true", false);
                    }

                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        protected void btnFinalizeClose_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox DecisionDate = (TextBox)TawaanWorking.FindControl("txtDecisionDate");
                TextBox txSpecialCharges = (TextBox)TawaanWorking.FindControl("txtSpecialCharges");
                int SpecialCharges = Convert.ToInt32(txSpecialCharges.Text);
                TextBox txComments = (TextBox)TawaanWorking.FindControl("taComments");
                bool Result = new WaterTheftBLL().SaveSepcialCharges(Convert.ToInt64(Request.QueryString["WaterTheftID"]), SpecialCharges, Convert.ToDateTime(DecisionDate.Text), true);
                if (Result)
                {
                    Result = new WaterTheftBLL().CaseClosed(Convert.ToInt64(Request.QueryString["WaterTheftID"]), txComments.Text);
                    if (Result)
                    {
                        WebFormsTest.FileUploadControl FileControl = (WebFormsTest.FileUploadControl)TawaanWorking.FindControl("FileUploadControlProof");
                        List<Tuple<string, string, string>> lstNameofFiles = FileControl.UploadNow(Configuration.WaterTheft, "DesnCtrl");
                        DataTable WTAttachmentTable = WaterTheftCase.GetDataTable(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);
                        new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);
                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.XENFinalizeCase, SessionManagerFacade.UserInformation.ID);

                        Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true", false);
                    }
                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        protected void btnAssignToXENFromSE_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox txComments = (TextBox)TawaanWorking.FindControl("taComments");
                string SEComments = txComments.Text;
                long CaseStatus = 4; // id for appeal is 4 
                if (Request.QueryString["AA"] != null)
                {
                    if (Convert.ToInt32(Request.QueryString["AA"]) == 2) // Case: Assign back from SE without appeal
                        CaseStatus = 2;
                }

                new WaterTheftBLL().AppealFromSE(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), CaseStatus, SEComments);
                WebFormsTest.FileUploadControl FileControl = (WebFormsTest.FileUploadControl)TawaanWorking.FindControl("FileUploadControlProof");
                //System.Data.DataSet ds = FileControl.UploadNow(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID));
                //System.Data.DataTable WTAttachmentTable = ds.Tables[0];
                List<Tuple<string, string, string>> lstNameofFiles = FileControl.UploadNow(Configuration.WaterTheft, "DesnCtrl");
                DataTable WTAttachmentTable = WaterTheftCase.GetDataTable(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);
                new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.ChieftAssignedBackXEN, SessionManagerFacade.UserInformation.ID);

                Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true", false);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAssignToXENFromChief_Click(object sender, EventArgs e)
        {
            try
            {
                long CaseStatus = 4; // id for appeal is 4 
                bool Result = false;
                TextBox txComments = (TextBox)TawaanWorking.FindControl("taComments");
                TextBox txFine = (TextBox)TawaanWorking.FindControl("txtAmountPaidForAppeal");

                if (!string.IsNullOrEmpty(Request.QueryString["AA"]))
                {
                    if (Convert.ToInt32(Request.QueryString["AA"]) == 2) // Case: Assign back from chief without appeal
                    {
                        CaseStatus = (long)Constants.WTCaseStatus.InProgress;
                        Result = true;
                    }
                    else // Case: Assign back from Chief appeal
                    {
                        WT_ChiefAppealDetails ObjChief = new WT_ChiefAppealDetails();
                        ObjChief.WatertheftID = Convert.ToInt64(Request.QueryString["WaterTheftID"]);
                        ObjChief.ChiefID = Convert.ToInt64(Session[SessionValues.UserID]);
                        ObjChief.FeeAmount = Convert.ToInt32(txFine.Text);
                        Result = new WaterTheftBLL().SaveFine(ObjChief);
                    }

                    if (Result)
                    {
                        new WaterTheftBLL().AppealFromSE(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), CaseStatus, txComments.Text);


                        if (Convert.ToInt32(Request.QueryString["AA"]) == 2) // Case: Assign back from chief without appeal
                        {
                            WebFormsTest.FileUploadControl FileControl = (WebFormsTest.FileUploadControl)TawaanWorking.FindControl("FileUploadControlAppeal");
                            List<Tuple<string, string, string>> lstNameofFiles = FileControl.UploadNow(Configuration.WaterTheft, "AttchCtrl");
                            DataTable WTAttachmentTable = WaterTheftCase.GetDataTable(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);
                            new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);
                        }
                        else
                        {
                            WebFormsTest.FileUploadControl FileControl = (WebFormsTest.FileUploadControl)TawaanWorking.FindControl("FileUploadControlAppeal");
                            List<Tuple<string, string, string>> lstNameofFiles = FileControl.UploadNow(Configuration.WaterTheft, "AttchCtrl");
                            DataTable WTAttachmentTable = WaterTheftCase.GetDataTable(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);
                            new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);

                            FileControl = (WebFormsTest.FileUploadControl)TawaanWorking.FindControl("FileUploadControlProof");
                            lstNameofFiles = FileControl.UploadNow(Configuration.WaterTheft, "DesnCtrl", 5);
                            WTAttachmentTable = WaterTheftCase.GetDataTable(Convert.ToInt64(Request.QueryString["WaterTheftID"]), Convert.ToInt64(Session[SessionValues.UserID]), Convert.ToInt64(SessionManagerFacade.UserInformation.DesignationID), lstNameofFiles, "W", 0, 0);
                            new WaterTheftBLL().AddWaterTheftAttachments(WTAttachmentTable);
                        }

                        NotifyEvent _event = new NotifyEvent();
                        _event.Parameters.Add("WaterTheftID", WaterTheftCase.WaterTheftID);
                        _event.AddNotifyEvent((long)NotificationEventConstants.WaterTheft.ChieftAssignedBackXEN, SessionManagerFacade.UserInformation.ID);

                        Response.RedirectPermanent("SearchWaterTheft.aspx?ShowHistory=true&ShowMessage=true", false);
                    }
                    else
                        Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
            }
        }

        protected void btnOffenders_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Remarks", "$('#ViewOffenders').modal();", true);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGrid()
        {
            try
            {
                gvOffender.DataSource = new WaterTheftBLL().GetOffenders(WaterTheftCase.WaterTheftID);
                gvOffender.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ViewOffenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvOffender_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvOffender.EditIndex = -1;
                BindGrid();

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
                BindGrid();
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
                BindGrid();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOffender.EditIndex = e.NewEditIndex;
                BindGrid();
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
                {
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
                else
                    Master.ShowMessage(Message.RoleNotDeleted.Description, SiteMaster.MessageType.Error);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RoleNotDeleted.Description, SiteMaster.MessageType.Error);
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
                    BindGrid();
                }
                else
                {
                    Master.ShowMessage(Message.RequiredFields.Description, SiteMaster.MessageType.Success);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Success);
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOffender_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvOffender.EditIndex)
                {
                    TextBox txCNIC = (TextBox)e.Row.FindControl("txtCNIC");
                    txCNIC.CssClass = txCNIC.CssClass.Replace("form-control required", "form-control required phoneNoInput");
                }



                if (SessionManagerFacade.UserInformation.DesignationID.Value == (Int64)Constants.Designation.SDO
                    && WaterTheftCase.AssignedToDesignationID == (Int64)Constants.Designation.SDO)
                {
                    //UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                    //if (mdlRoleRights != null)
                    //{
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = true; //(bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Visible = true; //(bool)mdlRoleRights.BEdit;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = true; //(bool)mdlRoleRights.BDelete;
                        }
                    }
                    // }

                }
                else  // it is only view case 
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Visible = false;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = false;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        //protected void btnViewOffenders_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Response.Redirect("ViewOffenders.aspx?WaterTheftID=" + Request.QueryString["WaterTheftID"] + "&OS=" + Request.QueryString["OS"]
        //            + "&RP=" + Request.QueryString["RP"] + "&AA=" + Request.QueryString["AA"] + "&Status=" + Request.QueryString["Status"]
        //            + "&View=" + Request.QueryString["View"]);
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}


    }
}