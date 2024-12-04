using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Tenders.Works
{
    public partial class AddADMReport : BasePage
    {
        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    btnSave.Visible = base.CanAdd;
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderWorkID"]))
                    {
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        hdnTenderWorkID.Value = Request.QueryString["TenderWorkID"];
                        //long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                        long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                        hlBackToWork.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID;
                        if (!string.IsNullOrEmpty(Request.QueryString["WorkSourceID"]))
                        {
                            hdnWorkSourceID.Value = Request.QueryString["WorkSourceID"];
                        }
                        hlBack.NavigateUrl = "~/Modules/Tenders/TenderNotice/TenderPrice.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value);
                        BindADMReportData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchCommittee.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchContractor.HRef = string.Format("~/Modules/Tenders/Works/AddContractorsAttendance.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchReport.HRef = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchstatement.HRef = string.Format("~/Modules/Tenders/TenderNotice/ViewComparativeStatement.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchTenderPrice.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderPrice.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        BindADMDataWithAttachemnts(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));

                        bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));

                        if (IsAwarded == true)
                        {
                            btnSave.Visible = false;
                        }
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindADMReportData(long _TenderWorkID, long _WorkSourceID)
        {
            try
            {

                dynamic WorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _TenderWorkID);
                Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(WorkData, "TenderNotice");
                Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(WorkData, "WorkName");
                Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(WorkData, "WorkType");

                string WorkSource = new TenderManagementBLL().GetWorkSourceByWorkID(_TenderWorkID);

                dynamic mdlADMReport;

                if (WorkSource == Convert.ToString(Constants.WorkType.ASSET))
                    mdlADMReport = new TenderManagementBLL().GetAssetTenderInformationForADMReport(_TenderWorkID);
                else
                    mdlADMReport = new TenderManagementBLL().GetTenderInformationForADMReport(_TenderWorkID);

                lblOpeningTime.Text = Utility.GetDynamicPropertyValue(mdlADMReport, "OpeningTime");
                lblSoldTenderCount.Text = Utility.GetDynamicPropertyValue(mdlADMReport, "SoldTenderCount");
                lblTenderSubmittedCount.Text = Utility.GetDynamicPropertyValue(mdlADMReport, "SubmittedTender");
                lblSubmissionTime.Text = Utility.GetDynamicPropertyValue(mdlADMReport, "SubmissionTime");
                hdnOpeningDate.Value = Utility.GetDynamicPropertyValue(mdlADMReport, "OpeningDate");
                hdnSubmissionDate.Value = Utility.GetDynamicPropertyValue(mdlADMReport, "SubmissionDate");
                List<dynamic> lstADMReport = new TenderManagementBLL().GetADMReportDataByTenderWorkID(_TenderWorkID);

                gvRejectedTenders.DataSource = lstADMReport;
                gvRejectedTenders.DataBind();


            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindADMDataWithAttachemnts(long _TenderWorkID, long _WorkSourceID)
        {
            try
            {

                TM_TenderWorks mdlTenderWorks = new TenderManagementBLL().getADMReportData(_TenderWorkID);
                txtObservations.Text = mdlTenderWorks.ADM_Observation;
                if (mdlTenderWorks.WorkStatusID == (long)Constants.WorkStatus.Cancelled)
                {
                    ChkBoxCancel.Checked = true;
                    txtTenderCancelReason.Text = mdlTenderWorks.StatusReason;
                }

                if (mdlTenderWorks.ADM_XENRequested == true)
                {
                    ChkBoxXEN.Checked = true;
                }
                if (mdlTenderWorks.ADM_TenderBoxEmpty == true)
                {
                    ChkBoxtenderEmpty.Checked = true;
                }
                if (mdlTenderWorks.ADM_RateNoted == true)
                {
                    ChkBoxRate.Checked = true;
                }
                if (mdlTenderWorks.ADM_Photography == true)
                {
                    ChkBoxSnaps.Checked = true;
                }
                if (mdlTenderWorks.ADM_MebersPresent == true)
                {
                    ChkBoxTenderingProcessmembers.Checked = true;
                }
                if (!string.IsNullOrEmpty(mdlTenderWorks.ADM_ActualOpeningReason))
                {
                    txtAlternateOpeningReason.Text = mdlTenderWorks.ADM_ActualOpeningReason;
                    string OpeningTime = Utility.GetFormattedTime(mdlTenderWorks.ADM_ActualOpeningDate.Value);
                    var OT = Convert.ToDateTime(OpeningTime);
                    TimePickerOpening.SetTime(OT.ToString("hh"), OT.ToString("mm"), OT.ToString("tt", System.Globalization.CultureInfo.InvariantCulture));

                }
                else
                {
                    CheckBoxA.Checked = true;
                    txtAlternateOpeningReason.Enabled = false;
                    txtAlternateOpeningReason.CssClass = "form-control";
                    txtAlternateOpeningReason.Attributes.Remove("required");
                    TimePickerOpening.DisbaleTimePicker();
                }
                if (!string.IsNullOrEmpty(mdlTenderWorks.ADM_ActualSubmissionReason))
                {
                    txtAlternateSubmissionReason.Text = mdlTenderWorks.ADM_ActualSubmissionReason;
                    string SubmissionTime = Utility.GetFormattedTime(mdlTenderWorks.ADM_ActualSubmissionDate.Value);
                    var ST = Convert.ToDateTime(SubmissionTime);
                    TimePickerSubmission.SetTime(ST.ToString("hh"), ST.ToString("mm"), ST.ToString("tt", System.Globalization.CultureInfo.InvariantCulture));
                }
                else
                {
                    chkBoxSameS.Checked = true;
                    txtAlternateSubmissionReason.Enabled = false;
                    txtAlternateSubmissionReason.CssClass = "form-control";
                    txtAlternateSubmissionReason.Attributes.Remove("required");
                    TimePickerSubmission.DisbaleTimePicker();
                }


                System.Web.UI.HtmlControls.HtmlTableRow TableRow = new System.Web.UI.HtmlControls.HtmlTableRow();
                List<TM_ADMAttachment> lstADMAttachments = new TenderManagementBLL().getAllADMAttachemnts(_TenderWorkID);
                int Count = 0;
                foreach (var item in lstADMAttachments)
                {
                    System.Web.UI.HtmlControls.HtmlTableCell TableCell = new System.Web.UI.HtmlControls.HtmlTableCell();
                    HyperLink AttachmentHyperLink = new HyperLink();
                    AttachmentHyperLink.ID = "lnk" + Count;
                    AttachmentHyperLink.CssClass = "ADMLinks" + Count;
                    AttachmentHyperLink.Text = item.Attachment.Substring(item.Attachment.LastIndexOf('_') + 1);
                    AttachmentHyperLink.Attributes.Add("href", Utility.GetImageURL(Configuration.TenderManagement, item.Attachment));
                    TableCell.Controls.Add(AttachmentHyperLink);
                    TableCell.Style.Add("padding-left", "12px");
                    TableRow.Controls.Add(TableCell);
                    tblHyperlinks.Rows.Add(TableRow);
                    Count++;
                }
                if (lstADMAttachments.Count > 0)
                {
                    HyperLinksDiv.Visible = true;
                }
                else
                {
                    HyperLinksDiv.Visible = false;

                }

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvRejectedTenders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtReason = (TextBox)e.Row.FindControl("txtRejectedReason");
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkRejected");
                    Label lblEarnestMoney = (Label)e.Row.FindControl("lblEarnestMoney");
                    Label lblDeposit = (Label)e.Row.FindControl("lblDeposit");
                    //TextBox txtRejectedReason = (TextBox)e.Row.FindControl("txtRejectedReason");

                    long EarnestMoney = long.Parse(lblEarnestMoney.Text, NumberStyles.Currency);
                    long Deposit = long.Parse(lblDeposit.Text, NumberStyles.Currency);
                    if (chk.Checked == false)
                    {
                        if (Deposit < EarnestMoney)
                        {
                            chk.Checked = true;
                            // txtReason.Attributes.Add("Required", "Required");
                            txtReason.Text = "Call Deposit is less than Earnest Money.";
                        }
                        else
                        {
                            chk.Checked = false;
                        }

                        if (txtReason.Text == "")
                        {
                            txtReason.Attributes.Add("disabled", "disabled");

                        }
                    }
                    


                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void chkBoxSameA_CheckedChanged(Object sender, EventArgs args)
        {
            try
            {
                if (CheckBoxA.Checked)
                {
                    txtAlternateOpeningReason.Enabled = false;
                    txtAlternateOpeningReason.CssClass = "form-control";
                    txtAlternateOpeningReason.Attributes.Remove("required");
                    TimePickerOpening.DisbaleTimePicker();
                    //txtAlternateOpeningTime.Enabled = false;
                    //txtAlternateOpeningTime.CssClass = "form-control timepicker1";
                    //txtAlternateOpeningTime.Attributes.Remove("required");
                }
                else
                {
                    txtAlternateOpeningReason.Enabled = true;
                    txtAlternateOpeningReason.CssClass = "form-control required";
                    txtAlternateOpeningReason.Attributes.Add("required", "required");
                    TimePickerOpening.EnableTimePicker();
                    //txtAlternateOpeningTime.Enabled = true;
                    //txtAlternateOpeningTime.CssClass = "form-control required timepicker1";
                    //txtAlternateOpeningTime.Attributes.Add("required", "required");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void chkBoxSameS_CheckedChanged(Object sender, EventArgs args)
        {
            try
            {
                if (chkBoxSameS.Checked)
                {
                    txtAlternateSubmissionReason.Enabled = false;
                    txtAlternateSubmissionReason.CssClass = "form-control";
                    txtAlternateSubmissionReason.Attributes.Remove("required");
                    TimePickerSubmission.DisbaleTimePicker();
                    //txtAlternateSubmissionTime.Enabled = false;
                    //txtAlternateSubmissionTime.CssClass = "form-control timepicker1";
                    //txtAlternateSubmissionTime.Attributes.Remove("required");
                }
                else
                {
                    txtAlternateSubmissionReason.Enabled = true;
                    txtAlternateSubmissionReason.CssClass = "form-control required";
                    txtAlternateSubmissionReason.Attributes.Add("required", "required");
                    TimePickerSubmission.EnableTimePicker();
                    //txtAlternateSubmissionTime.Enabled = true;
                    //txtAlternateSubmissionTime.CssClass = "form-control required timepicker1";
                    //txtAlternateSubmissionTime.Attributes.Add("required", "required");

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ChkBoxCancel_CheckedChanged(Object sender, EventArgs args)
        {
            try
            {
                if (ChkBoxCancel.Checked)
                {
                    txtTenderCancelReason.Enabled = true;
                    txtTenderCancelReason.CssClass = "form-control commentsMaxLengthRow multiline-no-resize required";
                    txtTenderCancelReason.Attributes.Add("required", "required");
                }
                else
                {
                    txtTenderCancelReason.Enabled = false;
                    txtTenderCancelReason.CssClass = "form-control commentsMaxLengthRow multiline-no-resize";
                    txtTenderCancelReason.Attributes.Remove("required");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                TenderManagementBLL TenderMgmtBLL = new TenderManagementBLL();
                HttpFileCollection uploadedFiles = Request.Files;
                List<TM_ADMAttachment> FileList = new List<TM_ADMAttachment>();

                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile userPostedFile = uploadedFiles[i];
                    if (userPostedFile.ContentLength > 0)
                    {
                        string FullFileName = string.Empty;
                        string FileName = string.Empty;
                        string FileExt = string.Empty;
                        string sGuid = Guid.NewGuid().ToString();
                        FullFileName = userPostedFile.FileName;
                        FileExt = userPostedFile.ContentType;

                        System.IO.FileInfo oFileInfo = new System.IO.FileInfo(FullFileName);
                        string NewFileName = sGuid + "_" + oFileInfo.Name;
                        FileName = Path.GetFileNameWithoutExtension(oFileInfo.Name);

                        string filePath = Utility.GetImagePath(Configuration.TenderManagement);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            if (!Directory.Exists(filePath))
                                Directory.CreateDirectory(filePath);

                            filePath = filePath + "\\" + NewFileName;
                            userPostedFile.SaveAs(filePath);

                            TM_ADMAttachment mdlADMAttachemnt = new TM_ADMAttachment();
                            mdlADMAttachemnt.Attachment = NewFileName;
                            mdlADMAttachemnt.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
                            mdlADMAttachemnt.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                            mdlADMAttachemnt.CreatedDate = DateTime.Now;
                            mdlADMAttachemnt.AttachmentType = FileExt;
                            FileList.Add(mdlADMAttachemnt);

                            //bool IsSaved = TenderMgmtBLL.SaveADMAttachemnt(mdlADMAttachemnt);

                            //if (!IsSaved)
                            //{
                            //    Master.ShowMessage(Message.RoleNotSaved.Description, SiteMaster.MessageType.Error);
                            //    return;
                            //}

                        }
                    }

                }
                TM_TenderWorks mdlTenderWorks = new TM_TenderWorks();
                List<TM_TenderWorksContractors> lstNew = new List<TM_TenderWorksContractors>();

                if (!chkBoxSameS.Checked)
                {
                    var AlternateSubmissionDate = Utility.GetParsedDateTime(Convert.ToString(hdnSubmissionDate.Value), TimePickerSubmission.GetTime());
                    mdlTenderWorks.ADM_ActualSubmissionDate = AlternateSubmissionDate;
                    mdlTenderWorks.ADM_ActualSubmissionReason = txtAlternateSubmissionReason.Text;
                }
                if (!CheckBoxA.Checked)
                {
                    var AlternateOpeningDate = Utility.GetParsedDateTime(Convert.ToString(hdnOpeningDate.Value), TimePickerOpening.GetTime());
                    mdlTenderWorks.ADM_ActualOpeningDate = AlternateOpeningDate;
                    mdlTenderWorks.ADM_ActualOpeningReason = txtAlternateOpeningReason.Text;
                }
                mdlTenderWorks.ADM_TenderBoxEmpty = ChkBoxtenderEmpty.Checked;
                mdlTenderWorks.ADM_RateNoted = ChkBoxRate.Checked;
                mdlTenderWorks.ADM_MebersPresent = ChkBoxTenderingProcessmembers.Checked;
                mdlTenderWorks.ADM_Photography = ChkBoxSnaps.Checked;
                mdlTenderWorks.ADM_XENRequested = ChkBoxXEN.Checked;
                mdlTenderWorks.ADM_Observation = txtObservations.Text;
                mdlTenderWorks.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                mdlTenderWorks.ModifiedDate = DateTime.Now.Date;
                mdlTenderWorks.ID = Convert.ToInt64(hdnTenderWorkID.Value);
                if (ChkBoxCancel.Checked)
                {
                    mdlTenderWorks.WorkStatusID = (int)Constants.WorkStatus.Cancelled;
                    mdlTenderWorks.StatusReason = txtTenderCancelReason.Text;
                }

                foreach (GridViewRow gvrow in gvRejectedTenders.Rows)
                {
                    TM_TenderWorksContractors mdlRejectedContractors = new TM_TenderWorksContractors();
                    TextBox RejectedReason = (TextBox)gvrow.FindControl("txtRejectedReason");
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkRejected");
                    Label ID = (Label)gvrow.FindControl("ID");
                    if (chk.Checked)
                    {
                        mdlRejectedContractors.Rejected = true;
                        mdlRejectedContractors.RejectedReason = RejectedReason.Text;
                        mdlRejectedContractors.ID = Convert.ToInt64(ID.Text);
                        mdlRejectedContractors.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                        mdlRejectedContractors.ModifiedDate = DateTime.Now.Date;
                        lstNew.Add(mdlRejectedContractors);
                    }
                    else
                    {
                        mdlRejectedContractors.Rejected = false;
                        mdlRejectedContractors.RejectedReason = RejectedReason.Text;
                        mdlRejectedContractors.ID = Convert.ToInt64(ID.Text);
                        mdlRejectedContractors.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                        mdlRejectedContractors.ModifiedDate = DateTime.Now.Date;
                        lstNew.Add(mdlRejectedContractors);
                    }

                }
                using (TransactionScope transaction = new TransactionScope())
                {
                    if (lstNew.Count > 0)
                    {
                        for (int i = 0; i < lstNew.Count; i++)
                        {
                            TenderMgmtBLL.UpdateTenderContractorsForADMReport(lstNew.ElementAt(i));
                        }
                    }
                    if (FileList.Count > 0)
                    {
                        for (int i = 0; i < FileList.Count; i++)
                        {
                            TenderMgmtBLL.SaveADMAttachemnt(FileList.ElementAt(i));
                        }
                    }

                    TenderMgmtBLL.UpdateTenderWorksForADMReport(mdlTenderWorks);
                    transaction.Complete();
                    Response.Redirect("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value), false);
                    Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);

                    //   TenderNotice.ViewComparativeStatement.IsSaved = true;


                }
            }
            catch (Exception exp)
            {

                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}