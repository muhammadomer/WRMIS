using PMIU.WRMIS.BLL.ClosureOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsTest;

namespace PMIU.WRMIS.Web.Modules.ClosureOperations.CWP
{
    public partial class AddWorkProgress : BasePage
    {
        ClosureOperationsBLL bllCO = new ClosureOperationsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    bool IsScheduled = false;
                    ResetPage(); 
                    if (!string.IsNullOrEmpty(Request.QueryString["CWPID"]))
                        hlBack.NavigateUrl = "~/Modules/ClosureOperations/CWP/ClosureOperationPlanDetail.aspx?CWPID=" + Request.QueryString["CWPID"];

                    if (!string.IsNullOrEmpty(Request.QueryString["CWID"]))
                    {
                        long cwID = Convert.ToInt64(Request.QueryString["CWID"]);
                        hdnF_CWID.Value = "" + cwID;
                        divDesilting.Visible = bllCO.IsWorkOfDesiltingType(cwID);
                        
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["WItemID"]))
                    {
                        
                        lblMode.Text = "View";
                        lblPageTitle.Text = "Work Progress";
                        LoadWorkProgress(Convert.ToInt64(Request.QueryString["WItemID"].ToString()));
                        DisableControls();
                        hlBack.NavigateUrl = "~/Modules/ClosureOperations/CWP/WorkProgessHistory.aspx?CWID="
                        + Request.QueryString["CWID"] + "&CWPID=" + Request.QueryString["CWPID"] + "&RestoreState=1";
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        hdnScheduleDetailID.Value = Convert.ToString(Request.QueryString["ScheduleDetailID"]);
                        IsScheduled = true;
                        hdnIsScheduled.Value = Convert.ToString(IsScheduled);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["RefMonitoringID"]))
                    {
                        IsScheduled = true;
                        lblMode.Text = "View";
                        lblPageTitle.Text = "Work Progress";
                        LoadWorkProgress(Convert.ToInt64(Request.QueryString["RefMonitoringID"].ToString()));
                        DisableControls();
                        hlBack.NavigateUrl = "~/Modules/ClosureOperations/CWP/WorkProgessHistory.aspx?CWID="
                        + Request.QueryString["CWID"] + "&CWPID=" + Request.QueryString["CWPID"] + "&RestoreState=1";
                    }
                    LoadInfoControl(IsScheduled);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void LoadInfoControl(bool IsScheduled)
        {
            if (IsScheduled)
            {
                ucInfo._RefMonitoringID = Convert.ToInt64(Request.QueryString["RefMonitoringID"]);
            }
            ucInfo._CWID = Convert.ToInt64(Request.QueryString["CWID"]); ;
            ucInfo._UserID = SessionManagerFacade.UserInformation.ID;
            //ucInfo._IsScheduled = IsScheduled;
            if (IsScheduled)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["RefMonitoringID"]))
                    ucInfo._ShowProgress = false;
                else
                    ucInfo._ShowProgress = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["WItemID"]))
                    ucInfo._ShowProgress = false;
                else
                    ucInfo._ShowProgress = true;
            }
         
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AddWorkProgress);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void ResetPage()
        {
            txtPrg.Text = "";
            txtRmrks.Text = "";
            txtSiltToRmv.Text = "";
            txtChnlLnght.Text = "";
            txtDate.Text = Utility.GetFormattedDate(DateTime.Now);
            List<object> lst = bllCO.GetWorkStatusList();
            Dropdownlist.DDLLoading(ddlWrkStatus, false, (int)Constants.DropDownFirstOption.NoOption, lst);
            Dropdownlist.SetSelectedValue(ddlWrkStatus, "2");
            btnSave.Enabled = base.CanAdd;
        }
        private void DisableControls()
        {
            divDate.Disabled = true;
            txtDate.Enabled = false; 
            ddlWrkStatus.Enabled = false;
            txtPrg.Enabled = false;
            txtRmrks.Enabled = false;
            txtSiltToRmv.Enabled = false;
            txtChnlLnght.Enabled = false;
            FileUploadControl1.Visible = false;
            btnSave.Visible = false;
            divAttchmentAdd.Visible = false;
            divAttchmentView.Visible = true;
        }
        private void LoadWorkProgress(long _ID)
        { 
            object objDetail = bllCO.GetWorkProgress(_ID);
            if (objDetail != null)
            {
                string count = "";
                Type propertiesType = objDetail.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(objDetail, null);
                   
                    if (prop.ToString().Contains("Remarks"))
                        txtRmrks.Text = propValue + "";
                    else if (prop.ToString().Contains("ChannelDesiltedLen"))
                        txtChnlLnght.Text = propValue + "";
                    else if (prop.ToString().Contains("SiltRemovedQty"))
                        txtSiltToRmv.Text = propValue + "";
                    else if (prop.ToString().Contains("ProgressPercentage"))
                        txtPrg.Text = propValue + "";
                    else if (prop.ToString().Contains("WorkStatus"))
                        ddlWrkStatus.Text = propValue + "";
                    else if (prop.ToString().Contains("InspectionDate"))
                        txtDate.Text = propValue + ""; 
                    else if (prop.ToString().Contains("AttachmentCount"))
                            count = propValue + "";

                    else if (prop.ToString().Contains("Attchment"))
                    {
                        if (!count.Equals("0"))
                        {
                            string atcmnt = propValue + "";
                            string [] arry =  atcmnt.Split(';');
                            switch (arry.Length)
                            {
                                case 2:
                                    PreviewImage(arry[0], FileUploadControl2); 
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[0]);
                                    //HyperLink1.Visible = true;
                                    break;
                                case 3:
                                    PreviewImage(arry[0], FileUploadControl2);
                                    PreviewImage(arry[1], FileUploadControl3);  
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[1]);
                                    //HyperLink2.Visible = true;
                                    break;
                                case 4:
                                    PreviewImage(arry[0], FileUploadControl2);
                                    PreviewImage(arry[1], FileUploadControl3); 
                                    PreviewImage(arry[2], FileUploadControl4); 
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[2]);
                                    //HyperLink3.Visible = true;
                                    break;
                                case 5:
                                    PreviewImage(arry[0], FileUploadControl2);
                                    PreviewImage(arry[1], FileUploadControl3); 
                                    PreviewImage(arry[2], FileUploadControl4);
                                    PreviewImage(arry[3], FileUploadControl5); 
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[2]);
                                    //HyperLink3.Visible = true;
                                    //HyperLink4.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[3]);
                                    //HyperLink4.Visible = true;
                                    break;
                                case 6:
                                    PreviewImage(arry[0], FileUploadControl2);
                                    PreviewImage(arry[1], FileUploadControl3); 
                                    PreviewImage(arry[2], FileUploadControl4);
                                    PreviewImage(arry[3], FileUploadControl5);
                                    PreviewImage(arry[4], FileUploadControl6); 
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[2]);
                                    //HyperLink3.Visible = true;
                                    //HyperLink4.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[3]);
                                    //HyperLink4.Visible = true;
                                    //HyperLink5.NavigateUrl = Utility.GetImageURL("ClosureOperations", arry[4]);
                                    //HyperLink5.Visible = true;
                                    break; 
                            }
                        }
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try 
            {
                DateTime date = Utility.GetParsedDate(txtDate.Text);
                if (date > DateTime.Now)
                {
                    Master.ShowMessage(Message.InspectionDateCannotBeFutureDate.Description, SiteMaster.MessageType.Error);
                    return;
                }

                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl1.UploadNow(Configuration.ClosureOperations);
                if(lstNameofFiles == null || lstNameofFiles.Count() == 0 ) 
                {
                    Master.ShowMessage("Attachment is required.", SiteMaster.MessageType.Error);
                    return;
                }


                double progrs = Convert.ToDouble(txtPrg.Text);
                DateTime? lastPrgsDate = null;
                double? lastPrgrs = null;

                Object objDetail = new ClosureOperationsBLL().GetWorkProgressByUser(Convert.ToInt64(hdnF_CWID.Value), SessionManagerFacade.UserInformation.ID);
                if (objDetail != null)
                {
                    Type propertiesType = objDetail.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        object propValue = prop.GetValue(objDetail, null);
                        if (prop.ToString().Contains("Progress"))
                            lastPrgrs = Convert.ToDouble(propValue + "");

                        if (prop.ToString().Contains("Date")) 
                            lastPrgsDate = Utility.GetParsedDate(propValue + "");
                    }
                }

                if (lastPrgrs != null && lastPrgrs > progrs )
                {
                    Master.ShowMessage(Message.PreviousPercentageIsGreater.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (progrs > 100)
                {
                    Master.ShowMessage(Message.ProgressCannotBeMoreThan100.Description, SiteMaster.MessageType.Error);
                    return;
                }
                 
                CW_WorkProgress mdl = new CW_WorkProgress(); 
                mdl.ClosureWorkID = Convert.ToInt64 (hdnF_CWID.Value);
                mdl.InspectionDate = date;
                mdl.ProgressPercentage = progrs;
                mdl.WorkStatusID = Convert.ToInt32 (ddlWrkStatus.SelectedItem.Value);
                if (!string.IsNullOrEmpty(txtSiltToRmv.Text)) 
                    mdl.SiltRemovedQty =Convert.ToInt32(txtSiltToRmv.Text);
                 if (!string.IsNullOrEmpty(txtChnlLnght.Text)) 
                    mdl.ChannelDesiltedLen =Convert.ToDouble(txtChnlLnght.Text);
                mdl.Remarks = txtRmrks.Text;
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy =(int) SessionManagerFacade.UserInformation.ID;
                mdl.CreatedByDesigID =(long) SessionManagerFacade.UserInformation.DesignationID;

                bool isRecordSaved = (bool)bllCO.SaveWorkProgress(mdl, lstNameofFiles,Convert.ToBoolean(hdnIsScheduled.Value),Convert.ToInt64(hdnScheduleDetailID.Value));
                Response.Redirect("ClosureOperationPlanDetail.aspx?CWPID=" + Request.QueryString["CWPID"] + "&isRecordSaved=" + isRecordSaved, false);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void PreviewImage (string _Subject , FileUploadControl _Control)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath); 

            FileUploadControl temp = _Control;
            _Control.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            _Control.Size = 1;
            _Control.ViewUploadedFilesAsThumbnail(Configuration.ClosureOperations, lstName);
            _Control.Visible = true;
        }
    }
}