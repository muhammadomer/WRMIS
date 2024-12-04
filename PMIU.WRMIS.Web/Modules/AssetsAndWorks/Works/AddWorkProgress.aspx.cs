using PMIU.WRMIS.BLL.AssetsAndWorks;
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

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.Works
{
    public partial class AddWorkProgress : BasePage
    {
        AssetsWorkBLL bllWork = new AssetsWorkBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    bool IsScheduled = false;
                    ResetPage();
                    if (!string.IsNullOrEmpty(Request.QueryString["AWID"]))
                        hlBack.NavigateUrl = "~/Modules/AssetsAndWorks/Works/SearchWork.aspx?CWID="
                        + Request.QueryString["AWID"] + "&RestoreState=1";

                    if (!string.IsNullOrEmpty(Request.QueryString["AWID"]))
                    {
                        long cwID = Convert.ToInt64(Request.QueryString["AWID"]);
                        hdnF_CWID.Value = "" + cwID;

                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["WItemID"]))
                    {
                        string AWPID = "";
                        if (!string.IsNullOrEmpty(Request.QueryString["WItemID"]))
                            AWPID = Request.QueryString["WItemID"];
                        else
                            AWPID = Request.QueryString["AWPID"];
                        lblMode.Text = "View";
                        lblPageTitle.Text = "Work Progress";
                        LoadWorkProgress(Convert.ToInt64(AWPID));
                        DisableControls();
                        hlBack.NavigateUrl = "~/Modules/AssetsAndWorks/Works/WorkProgessHistory.aspx?AWID="
                        + Request.QueryString["AWID"] + "&AWPID=" + AWPID + "&RestoreState=1";
                    }
                    ////////

                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleDetailID"]))
                    {
                        hdnScheduleDetailID.Value = Convert.ToString(Request.QueryString["ScheduleDetailID"]);
                        IsScheduled = true;
                        hdnIsScheduled.Value = Convert.ToString(IsScheduled);
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["RefMonitoringID"]))
                    {
                        string AWPID = "";
                        if (!string.IsNullOrEmpty(Request.QueryString["WItemID"]))
                            AWPID = Request.QueryString["WItemID"];
                        else
                            AWPID = Request.QueryString["RefMonitoringID"];
                        IsScheduled = true;
                        lblMode.Text = "View";
                        lblPageTitle.Text = "Work Progress";
                        LoadWorkProgress(Convert.ToInt64(Request.QueryString["RefMonitoringID"].ToString()));
                        DisableControls();
                        //hlBack.NavigateUrl = "~/Modules/AssetsAndWorks/Works/WorkProgessHistory.aspx?AWID="
                        //+ Request.QueryString["AWID"] + "&AWPID=" + AWPID + "&RestoreState=1";
                        hlBack.Attributes.Add("onclick", "javascript:history.go(-1);");
                    }


                    LoadInfoControl(IsScheduled);
                    FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.RemoveValidation);
                }
                FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.RemoveValidation);
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
                ucInfoAdd._RefMonitoringID = Convert.ToInt64(Request.QueryString["RefMonitoringID"]);
            }
            ucInfoAdd._AWID = Convert.ToInt64(Request.QueryString["AWID"]); ;
            ucInfoAdd._UserID = SessionManagerFacade.UserInformation.ID;
            if (IsScheduled)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["RefMonitoringID"]))
                    ucInfoAdd._ShowProgress = false;
                else
                    ucInfoAdd._ShowProgress = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["WItemID"]))
                    ucInfoAdd._ShowProgress = false;
                else
                    ucInfoAdd._ShowProgress = true;
            }

        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void ResetPage()
        {
            txtfinancialprg.Text = "";
            txtPrg.Text = "";
            txtRmrks.Text = "";
            txtDate.Text = Utility.GetFormattedDate(DateTime.Now);
            List<object> lst = bllWork.GetWorkStatusList();
            Dropdownlist.DDLLoading(ddlWrkStatus, false, (int)Constants.DropDownFirstOption.NoOption, lst);
            if (string.IsNullOrEmpty(Request.QueryString["WItemID"]))
            {
                object bllGetLastWorkprogressID = bllWork.GetLastWorkProgressID(Convert.ToInt64(Request.QueryString["AWID"]), SessionManagerFacade.UserInformation.ID);
                if (bllGetLastWorkprogressID != null)
                {
                    string InfrastructureTypeID = Convert.ToString(bllGetLastWorkprogressID.GetType().GetProperty("WorkProgressID").GetValue(bllGetLastWorkprogressID));
                    Dropdownlist.SetSelectedValue(ddlWrkStatus, InfrastructureTypeID);
                }

            }
            btnSave.Enabled = base.CanAdd;
        }
        private void DisableControls()
        {
            divDate.Disabled = true;
            txtDate.Enabled = false;
            ddlWrkStatus.Enabled = false;
            txtfinancialprg.Enabled = false;
            txtPrg.Enabled = false;
            txtRmrks.Enabled = false;
            FileUploadControl1.Visible = false;
            btnSave.Visible = false;
            divAttchmentAdd.Visible = false;
            divAttchmentView.Visible = true;
        }
        private void LoadWorkProgress(long _ID)
        {
            object objDetail = bllWork.GetWorkProgress(_ID);
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
                    else if (prop.ToString().Contains("FinancialPercentage"))
                        txtfinancialprg.Text = propValue + "";
                    else if (prop.ToString().Contains("ProgressPercentage"))
                        txtPrg.Text = propValue + "";
                    else if (prop.ToString().Contains("WorkStatus"))
                        Dropdownlist.SetSelectedValue(ddlWrkStatus, propValue + "");
                    else if (prop.ToString().Contains("InspectionDate"))
                        txtDate.Text = propValue + "";
                    else if (prop.ToString().Contains("AttachmentCount"))
                        count = propValue + "";

                    else if (prop.ToString().Contains("Attchment"))
                    {
                        if (!count.Equals("0"))
                        {
                            string atcmnt = propValue + "";
                            string[] arry = atcmnt.Split(';');
                            switch (arry.Length)
                            {
                                case 2:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;
                                    break;
                                case 3:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[1]);
                                    //HyperLink2.Visible = true;

                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;

                                    PreviewImage3(arry[1]);
                                    FileUploadControl3.Visible = true;

                                    break;
                                case 4:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[2]);
                                    //HyperLink3.Visible = true;

                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;

                                    PreviewImage3(arry[1]);
                                    FileUploadControl3.Visible = true;

                                    PreviewImage4(arry[2]);
                                    FileUploadControl4.Visible = true;

                                    break;
                                case 5:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[2]);
                                    //HyperLink3.Visible = true;
                                    //HyperLink4.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[3]);
                                    //HyperLink4.Visible = true;

                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;

                                    PreviewImage3(arry[1]);
                                    FileUploadControl3.Visible = true;

                                    PreviewImage4(arry[2]);
                                    FileUploadControl4.Visible = true;

                                    PreviewImage5(arry[3]);
                                    FileUploadControl5.Visible = true;


                                    break;
                                case 6:
                                    //HyperLink1.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[0]);
                                    //HyperLink1.Visible = true;
                                    //HyperLink2.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[1]);
                                    //HyperLink2.Visible = true;
                                    //HyperLink3.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[2]);
                                    //HyperLink3.Visible = true;
                                    //HyperLink4.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[3]);
                                    //HyperLink4.Visible = true;
                                    //HyperLink5.NavigateUrl = Utility.GetImageURL("AssetsWorks", arry[4]);
                                    //HyperLink5.Visible = true;

                                    PreviewImage2(arry[0]);
                                    FileUploadControl2.Visible = true;

                                    PreviewImage3(arry[1]);
                                    FileUploadControl3.Visible = true;

                                    PreviewImage4(arry[2]);
                                    FileUploadControl4.Visible = true;

                                    PreviewImage5(arry[3]);
                                    FileUploadControl5.Visible = true;

                                    PreviewImage6(arry[4]);
                                    FileUploadControl6.Visible = true;

                                    break;
                            }
                        }
                    }
                }
            }
        }
        private void PreviewImage2(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl2.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl2.Size = 1;
            FileUploadControl2.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }
        private void PreviewImage3(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl3.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl3.Size = 1;
            FileUploadControl3.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }
        private void PreviewImage4(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl4.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl4.Size = 1;
            FileUploadControl4.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }
        private void PreviewImage5(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl5.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl5.Size = 1;
            FileUploadControl5.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

        }
        private void PreviewImage6(string _Subject)
        {
            string filename = new System.IO.FileInfo(_Subject).Name;
            string AttachmentPath = filename;
            List<string> lstName = new List<string>();
            lstName.Add(AttachmentPath);
            FileUploadControl6.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
            FileUploadControl6.Size = 1;
            FileUploadControl6.ViewUploadedFilesAsThumbnail(Configuration.AssetsWorks, lstName);

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

                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl1.UploadNow(Configuration.AssetsWorks);
                //if(lstNameofFiles == null || lstNameofFiles.Count() == 0 ) 
                //{
                //    Master.ShowMessage("Attachment is required.", SiteMaster.MessageType.Error);
                //    return;
                //}


                double progrs = Convert.ToDouble(txtPrg.Text);
                double Financpercentg = Convert.ToDouble(txtfinancialprg.Text);
                DateTime? lastPrgsDate = null;
                double? lastPrgrs = null;
                double? lastFinancialPercentage = null;

                Object objDetail = new AssetsWorkBLL().GetWorkProgressByUser(Convert.ToInt64(hdnF_CWID.Value), SessionManagerFacade.UserInformation.ID);
                if (objDetail != null)
                {
                    Type propertiesType = objDetail.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(propertiesType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        object propValue = prop.GetValue(objDetail, null);
                        if (prop.ToString().Contains("Progress"))
                            lastPrgrs = Convert.ToDouble(propValue + "");

                        if (prop.ToString().Contains("FinancialPercentage"))
                            lastFinancialPercentage = Convert.ToDouble(propValue + "");

                        if (prop.ToString().Contains("Date"))
                            lastPrgsDate = Utility.GetParsedDate(propValue + "");

                    }
                }

                if (lastPrgrs != null && lastPrgrs > progrs)
                {
                    Master.ShowMessage(Message.PreviousProgressPercentageIsGreater.Description, SiteMaster.MessageType.Error);
                    return;
                }
                if (lastFinancialPercentage != null && lastFinancialPercentage > Financpercentg)
                {
                    Master.ShowMessage(Message.PreviousFinancialPercentageIsGreater.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (progrs > 100 || Financpercentg > 100)
                {
                    Master.ShowMessage(Message.AssetWorkProgressCannotBeMoreThan100.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool Chkassociation = false;
                bool IsAssetType = bllWork.IsAssetTypeWork(Convert.ToInt64(hdnF_CWID.Value));
                bool IsExist = bllWork.IsAssetExistInWork(Convert.ToInt64(hdnF_CWID.Value));
                if (IsExist == false && IsAssetType==true && progrs == 100)
                {
                    Chkassociation = true;
                }

                AM_AssetWorkProgress mdl = new AM_AssetWorkProgress();
                mdl.AssetWorkID = Convert.ToInt64(hdnF_CWID.Value);
                mdl.InspectionDate = date;
                mdl.ProgressPercentage = progrs;
                mdl.FinancialPercentage = Financpercentg;
                mdl.WorkProgressID = Convert.ToInt32(ddlWrkStatus.SelectedItem.Value);

                mdl.Remarks = txtRmrks.Text;
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdl.CreatedByDesigID = (long)SessionManagerFacade.UserInformation.DesignationID;
                mdl.Source = "W";

                bool isRecordSaved = (bool)bllWork.SaveWorkProgressW(mdl, lstNameofFiles, Chkassociation, Convert.ToInt64(hdnF_CWID.Value), Convert.ToBoolean(hdnIsScheduled.Value), Convert.ToInt64(hdnScheduleDetailID.Value));
                Response.Redirect("SearchWork.aspx?CWID=" + Request.QueryString["AWID"] + "&RrecordSaved=" + isRecordSaved, false);
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}