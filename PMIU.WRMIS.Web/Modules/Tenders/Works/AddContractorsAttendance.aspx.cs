using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace PMIU.WRMIS.Web.Modules.Tenders.Works
{
    public partial class AddContractorsAttendance : BasePage
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
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
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
                        hlBack.NavigateUrl = "~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value);
                        BindContractorsAttendanceData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchCommittee.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderOpeningProcess.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchContractor.HRef = string.Format("~/Modules/Tenders/Works/AddContractorsAttendance.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));



                        if (mdlUser.DesignationID == (long)Constants.Designation.XEN || mdlUser.DesignationID == (long)Constants.Designation.CE || mdlUser.DesignationID == (long)Constants.Designation.SE)
                        {
                            anchReport.Disabled = true;
                            anchReport.Attributes.Add("style", "color:black");
                        }
                        else
                        {
                            anchReport.Disabled = false;
                            anchReport.HRef = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        }
                        //if (base.CanView)
                        //anchReport.HRef = string.Format("~/Modules/Tenders/Works/AddADMReport.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchstatement.HRef = string.Format("~/Modules/Tenders/TenderNotice/ViewComparativeStatement.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));
                        if (base.CanView)
                            anchTenderPrice.HRef = string.Format("~/Modules/Tenders/TenderNotice/TenderPrice.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value));

                        bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));

                        if (IsAwarded == true)
                        {
                            btnSave.Visible = false;
                            ddlMonitoredBy.Enabled = false;
                            ddlName.Enabled = false;

                        }

                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tenders);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindContractorsAttendanceData(long _TenderWorkID, long _WorkSourceID)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                dynamic WorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _TenderWorkID);
                Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(WorkData, "TenderNotice");
                Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(WorkData, "WorkName");
                Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(WorkData, "WorkType");
                hdnDivisionID.Value = Utility.GetDynamicPropertyValue(WorkData, "DivisionID");

                List<object> ContractorsData = new TenderManagementBLL().GetContractorsByTenderWorkID(_TenderWorkID);
                gvContractorsAttendance.DataSource = ContractorsData;
                gvContractorsAttendance.DataBind();
                dynamic CMData = new TenderManagementBLL().GetCMDataByTenderWorkID(_TenderWorkID);
                string MonitoredBy = Utility.GetDynamicPropertyValue(CMData, "ECA_MonitoredBy");
                long DesignationID = 0;
                if (MonitoredBy == null)
                {
                    ddlMonitoredBy.SelectedIndex = -1;
                    ddlMonitoredBy.Enabled = false;
                    ddlName.Enabled = false;
                }

                if (!string.IsNullOrEmpty(MonitoredBy))
                {
                    if (MonitoredBy.Trim().ToUpper() == "MA")
                        ddlMonitoredBy.SelectedIndex = 2;
                    else
                        ddlMonitoredBy.SelectedIndex = 1;

                    // ddlMonitoredBy.SelectedItem.Text = MonitoredBy;
                    long DivisionId = Convert.ToInt64(hdnDivisionID.Value);
                    if (ddlMonitoredBy.SelectedItem.Text.Trim().ToUpper() == "MA")
                        DesignationID = (long)Constants.Designation.MA;
                    else if (ddlMonitoredBy.SelectedItem.Text.Trim().ToUpper() == "ADM")
                        DesignationID = (long)Constants.Designation.ADM;
                    if (DesignationID != 0)
                    {
                        Dropdownlist.DDLGetUserByDivisionandDesignationID(ddlName, DivisionId, DesignationID, (int)Constants.DropDownFirstOption.Select);
                        string Name = Utility.GetDynamicPropertyValue(CMData, "ECA_MonitoredName");
                        Dropdownlist.SetSelectedText(ddlName, Name);
                        ddlName.Enabled = false;
                        ddlMonitoredBy.Enabled = false;
                    }


                }
                else
                {
                    ddlMonitoredBy.SelectedIndex = -1;
                    ddlName.Enabled = false;
                    ddlMonitoredBy.Enabled = false;
                }

                System.Web.UI.HtmlControls.HtmlTableRow TableRow = new System.Web.UI.HtmlControls.HtmlTableRow();
                List<TM_TenderWorkAttachment> lstTenderWorkAttachments = new TenderManagementBLL().GetAllTenderWorkAttachments(_TenderWorkID, "CA");

                if (lstTenderWorkAttachments.Count > 0)
                {
                    List<string> lstName = new List<string>();

                    foreach (var item in lstTenderWorkAttachments)
                    {
                        lstName.Add(item.Attachment);
                    }
                    string AttachmentPath = Convert.ToString(lstName);
                    FileUploadControl1.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                    FileUploadControl1.Size = lstName.Count;
                    FileUploadControl1.ViewUploadedFilesAsThumbnail(Configuration.TenderManagement, lstName);
                    if (mdlUser.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                    {
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnSave.Visible = true;
                    }
                }

                // int Count = 0;
                //foreach (var item in lstTenderWorkAttachments)
                //{
                //    System.Web.UI.HtmlControls.HtmlTableCell TableCell = new System.Web.UI.HtmlControls.HtmlTableCell();
                //    HyperLink AttachmentHyperLink = new HyperLink();
                //    AttachmentHyperLink.ID = "lnk" + Count;
                //    AttachmentHyperLink.CssClass = "ADMLinks" + Count;
                //    AttachmentHyperLink.Text = item.Attachment.Substring(item.Attachment.LastIndexOf('_') + 1);
                //    AttachmentHyperLink.Attributes.Add("href", Utility.GetImageURL(Configuration.TenderManagement, item.Attachment));
                //    TableCell.Controls.Add(AttachmentHyperLink);
                //    TableCell.Style.Add("padding-left", "12px");
                //    TableRow.Controls.Add(TableCell);
                //    tblHyperlinks.Rows.Add(TableRow);
                //    Count++;
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "RemoveRequired();", true);
                //}
                if (lstTenderWorkAttachments.Count > 0)
                {
                    HyperLinksDiv.Visible = true;
                }
                else
                {
                    HyperLinksDiv.Visible = false;

                }

                if (mdlUser.DesignationID == (long)Constants.Designation.ChiefMonitoring)
                {
                    btnSave.Visible = true;
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlMonitoredBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            long DivisionId = Convert.ToInt64(hdnDivisionID.Value);
            long DesignationID = 0;
            if (ddlMonitoredBy.SelectedItem.Value != "")
            {
                DesignationID = Convert.ToInt64(ddlMonitoredBy.SelectedItem.Value);
            }
            else
            {
                ddlName.SelectedIndex = -1;
            }

            if (DesignationID != 0)
            {
                Dropdownlist.DDLGetUserByDivisionandDesignationID(ddlName, DivisionId, DesignationID, (int)Constants.DropDownFirstOption.Select);
            }

        }

        protected void gvContractorsAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtAlternateName = (TextBox)e.Row.FindControl("txtAlternateName");
                    TextBox txtAlternateDesignation = (TextBox)e.Row.FindControl("txtAlternateDesignation");
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkAlternate");
                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    if (chk.Checked)
                    {
                        chkSelect.Checked = true;
                    }
                    if (txtAlternateName.Text == "")
                    {
                        txtAlternateDesignation.Attributes.Add("disabled", "disabled");
                        txtAlternateName.Attributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        chk.Checked = true;
                    }

                    bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));

                    if (IsAwarded == true)
                    {
                        chk.Enabled = false;
                        chkSelect.Enabled = false;
                    }
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            HttpFileCollection uploadedFiles = Request.Files;
            List<TM_TenderWorkAttachment> FileList = new List<TM_TenderWorkAttachment>();

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

                        TM_TenderWorkAttachment mdlTenderWorkAttachments = new TM_TenderWorkAttachment();
                        mdlTenderWorkAttachments.Attachment = NewFileName;
                        mdlTenderWorkAttachments.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
                        mdlTenderWorkAttachments.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                        mdlTenderWorkAttachments.CreatedDate = DateTime.Now;
                        mdlTenderWorkAttachments.AttachmentType = FileExt;
                        mdlTenderWorkAttachments.Source = "CA";
                        FileList.Add(mdlTenderWorkAttachments);
                    }
                }

            }

            TenderManagementBLL TenderMgmtBLL = new TenderManagementBLL();
            List<TM_TenderWorksContractors> lstNew = new List<TM_TenderWorksContractors>();
            //  TM_TenderWorks mdlTenderWorks = new TM_TenderWorks();
            foreach (GridViewRow gvrow in gvContractorsAttendance.Rows)
            {
                TM_TenderWorksContractors mdlTenderContractor = new TM_TenderWorksContractors();
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                CheckBox chkSelect = (CheckBox)gvrow.FindControl("chkAlternate");
                if (chk.Checked)
                {
                    mdlTenderContractor.Attended = true;
                }
                else
                {
                    mdlTenderContractor.Attended = false;
                }
                TextBox AlternateName = (TextBox)gvrow.FindControl("txtAlternateName");
                TextBox AlternateRemarks = (TextBox)gvrow.FindControl("txtAlternateRemarks");
                Label ID = (Label)gvrow.FindControl("ID");

                mdlTenderContractor.ID = Convert.ToInt64(ID.Text);
                mdlTenderContractor.AlternateName = AlternateName.Text;
                mdlTenderContractor.AlternateRemarks = AlternateRemarks.Text;
                mdlTenderContractor.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                lstNew.Add(mdlTenderContractor);

            }
            //List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.TenderManagement);
            //if (lstNameofFiles.Count > 0)
            //{
            //    mdlTenderWorks.CA_Attachment = lstNameofFiles[0].Item3;
            //}
            //else
            //{
            //    mdlTenderWorks.CA_Attachment = hlAttachment.Attributes["FileName"];
            //}
            //mdlTenderWorks.CA_MonitoredBy = ddlMonitoredBy.SelectedItem.Text;
            //mdlTenderWorks.CA_MonitoredName = ddlName.SelectedItem.Text;
            //mdlTenderWorks.ID = Convert.ToInt64(hdnTenderWorkID.Value);
            //mdlTenderWorks.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);

            using (TransactionScope transaction = new TransactionScope())
            {

                for (int i = 0; i < lstNew.Count; i++)
                {
                    TenderMgmtBLL.UpdateTenderContractors(lstNew.ElementAt(i));
                }
                if (FileList.Count > 0)
                {
                    for (int i = 0; i < FileList.Count; i++)
                    {
                        TenderMgmtBLL.SaveTenderWorkAttachments(FileList.ElementAt(i));
                    }
                }
                //  TenderMgmtBLL.UpdateTenderWorksForCntractorsByWorkID(mdlTenderWorks);
                transaction.Complete();
                //Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                TenderNotice.TenderPrice.IsSaved = true;
                Response.Redirect("~/Modules/Tenders/TenderNotice/TenderPrice.aspx?TenderWorkID=" + Convert.ToInt64(hdnTenderWorkID.Value) + "&WorkSourceID=" + Convert.ToInt64(hdnWorkSourceID.Value), false);

            }

        }
    }
}