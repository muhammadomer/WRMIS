using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.BLL.Notifications;
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

namespace PMIU.WRMIS.Web.Modules.ComplaintsManagement
{
    public partial class AddComplaintComments : BasePage
    {
        public const int ComplaintCommentsIDIndex = 0;
        long ComplaintId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    hlBack.NavigateUrl = "~/Modules/ComplaintsManagement/SearchComplaints.aspx?ShowHistory=true";
                    if (!string.IsNullOrEmpty(Request.QueryString["ComplaintDashboardID"]))
                    {
                        ComplaintId = Convert.ToInt64(Request.QueryString["ComplaintDashboardID"]);
                        GetComplaintDataByComplaintID(ComplaintId);
                        BindCommentsResultsGrid(ComplaintId);
                        hdnComplaintID.Value = Convert.ToString(ComplaintId);
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["ComplaintSearchID"]))
                    {
                        ComplaintId = Convert.ToInt64(Request.QueryString["ComplaintSearchID"]);
                        GetComplaintDataByComplaintID(ComplaintId);
                        BindCommentsResultsGrid(ComplaintId);
                        hdnComplaintID.Value = Convert.ToString(ComplaintId);

                    }
                    ComplaintsManagementBLL bllComplaints = new ComplaintsManagementBLL();
                    bool IsUpdated = bllComplaints.UpdateComplaintMarkedAndComplaint(ComplaintId, SessionManagerFacade.UserInformation.ID, SessionManagerFacade.UserInformation.UA_Designations.ID);


                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    //   commentsdiv.Visible = true;
                    if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
                    {
                        btnSaveForward.Visible = base.CanAdd;
                        btnAssignADM.Visible = false;
                        btnAssignXEN.Visible = false;
                        btnSave.Visible = false;
                        //ddlNotify.Visible = true;
                        //lblNotify.Visible = true;
                        btnResolved.Visible = false;
                        //btnNotifySDO.Visible = true;
                    }
                    else if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
                    {
                        btnAssignXEN.Visible = base.CanAdd;
                        btnSave.Visible = false;
                        btnSaveForward.Visible = false;
                        btnAssignADM.Visible = false;
                        //ddlNotify.Visible = false;
                        //lblNotify.Visible = false;
                        btnResolved.Visible = base.CanAdd;
                        NotifyArea.Visible = false;
                        //btnNotifySDO.Visible = false;
                    }
                    else if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirectorHelpline)
                    {
                        btnAssignADM.Visible = base.CanAdd;
                        btnAssignXEN.Visible = base.CanAdd;
                        btnSave.Visible = false;
                        btnSaveForward.Visible = false;
                        //ddlNotify.Visible = false;
                        //lblNotify.Visible = false;
                        btnResolved.Visible = base.CanAdd;
                        NotifyArea.Visible = false;
                        //btnNotifySDO.Visible = false;
                    }
                    else
                    {
                        btnAssignADM.Visible = false;
                        btnAssignXEN.Visible = false;
                        btnSaveForward.Visible = false;
                        btnSave.Visible = true;
                        ddlNotify.Visible = false;
                        lblNotify.Visible = false;
                        btnResolved.Visible = false;
                        btnNotifySDO.Visible = false;
                        NotifyArea.Visible = false;
                    }

                    bool lsResolved = bllComplaints.IsComplaintResolved(ComplaintId);
                    if (lsResolved)
                    {
                        commentsdiv.Visible = false;
                        btnSave.Visible = false;
                        hlBack.Visible = true;
                        btnSaveForward.Visible = false;
                        btnAssignADM.Visible = false;
                        btnAssignXEN.Visible = false;
                        btnResolved.Visible = false;

                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchComplaints);

            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        protected void GetComplaintDataByComplaintID(long ComplaintID)
        {
            ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
            dynamic ComplaintInformation = bllComplaint.GetComplaintInformationByComplaintID(ComplaintID);
            lblComplaintNumber.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintNumber");
            lblComplaintSource.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintSource");
            lblDomain.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "Domain");

            lblDivision.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "Division");
            DateTime date = Convert.ToDateTime(Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintDate"));
            lblComplaintDate.Text = Convert.ToString(Utility.GetFormattedDate(date));
            lblComplainantName.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplainantName");
            lblComplaintType.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintType");
            lblCell.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaiantCell");
            lblAddress.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplainantAddress");
            lblComplaintDetails.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ComplaintDetails");
            lblPMIUFileNo.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "PMIUFileNo");
            lblResponseDuration.Text = Utility.GetDynamicPropertyValue(ComplaintInformation, "ResponseDuration");
            string Attachment = Utility.GetDynamicPropertyValue(ComplaintInformation, "Attachment");
            if (!string.IsNullOrEmpty(Attachment))
            {
                //hlAttachment.NavigateUrl = Utility.GetImageURL(Configuration.Complaints, Attachment);
                //hlAttachment.Visible = true;
                string AttachmentPath = Attachment;
                List<string> lstName = new List<string>();
                lstName.Add(AttachmentPath);
                FileUploadControl2.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                FileUploadControl2.Size = lstName.Count;
                FileUploadControl2.ViewUploadedFilesAsThumbnail(Configuration.Complaints, lstName);
            }
            string DivisionID = Utility.GetDynamicPropertyValue(ComplaintInformation, "DivisionID");
            hdnDivisionID.Value = DivisionID;
            hdnComplaintStatus.Value = Utility.GetDynamicPropertyValue(ComplaintInformation, "Status");
            BindDropdowns(Convert.ToInt64(DivisionID));
        }

        private void BindDropdowns(long DivisionID)
        {
            Dropdownlist.DDLGetNotifySDO(ddlNotify, DivisionID, (int)Constants.DropDownFirstOption.Select);


        }
        private void BindCommentsResultsGrid(long _ComplaintID)
        {
            try
            {
                ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
                List<dynamic> lstComments = bllComplaint.GetAllCommentsByComplaintID(_ComplaintID);

                gvComplaintComments.DataSource = lstComments;
                gvComplaintComments.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvComplaintComments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblComplaintDate = (Label)e.Row.FindControl("lblComplaintDate");
                    Label lblAttachment = (Label)e.Row.FindControl("lblAttachment");
                    HyperLink hlImage = (HyperLink)e.Row.FindControl("hlImage");
                    DateTime Date = DateTime.Parse(lblComplaintDate.Text);
                    lblComplaintDate.Text = Utility.GetFormattedDate(Date) + " " + Utility.GetFormattedTime(Date);
                    if (lblAttachment.Text == "")
                    {
                        lblAttachment.Visible = true;
                        hlImage.Visible = false;
                        //lblAttachment.Text = "- - -";
                    }

                    string AttachmentPath = lblAttachment.Text;
                    List<string> lstName = new List<string>();
                    lstName.Add(AttachmentPath);
                    WebFormsTest.FileUploadControl FileUploadControl = (WebFormsTest.FileUploadControl)e.Row.FindControl("FileUploadControl1");
                    FileUploadControl.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                    FileUploadControl.Size = lstName.Count;
                    FileUploadControl.ViewUploadedFilesAsThumbnail(Configuration.Complaints, lstName);
                }


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSearchComplaints_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvComplaintComments.PageIndex = e.NewPageIndex;

                BindCommentsResultsGrid(Convert.ToInt64(hdnComplaintID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //protected void btnAddComments_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        UA_Users mdlUser = SessionManagerFacade.UserInformation;
        //        commentsdiv.Visible = true;
        //        if (mdlUser.DesignationID == (long)Constants.Designation.XEN)
        //        {
        //            btnSaveForward.Visible = true;
        //            btnAssignADM.Visible = false;
        //            btnAssignXEN.Visible = false;
        //            btnSave.Visible = false;
        //            ddlNotify.Visible = true;
        //            lblNotify.Visible = true;
        //            btnResolved.Visible = false;
        //            btnNotifySDO.Visible = true;
        //        }
        //        else if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
        //        {
        //            btnAssignXEN.Visible = true;
        //            btnSave.Visible = false;
        //            btnSaveForward.Visible = false;
        //            btnAssignADM.Visible = false;
        //            ddlNotify.Visible = false;
        //            lblNotify.Visible = false;
        //            btnResolved.Visible = true;
        //            btnNotifySDO.Visible = false;
        //        }
        //        else if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirectorHelpline)
        //        {
        //            btnAssignADM.Visible = true;
        //            btnAssignXEN.Visible = true;
        //            btnSave.Visible = false;
        //            btnSaveForward.Visible = false;
        //            ddlNotify.Visible = false;
        //            lblNotify.Visible = false;
        //            btnResolved.Visible = true;
        //            btnNotifySDO.Visible = false;
        //        }
        //        else
        //        {
        //            btnAssignADM.Visible = false;
        //            btnAssignXEN.Visible = false;
        //            btnSaveForward.Visible = false;
        //            btnSave.Visible = true;
        //            ddlNotify.Visible = false;
        //            lblNotify.Visible = false;
        //            btnResolved.Visible = false;
        //            btnNotifySDO.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        protected void btnNotifySDO_Click(object sender, EventArgs e)
        {
            try
            {
                //  txtComments.Attributes.Remove("required");
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                CM_ComplaintNotification mdlCompNotification = new CM_ComplaintNotification();
                ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
                CM_ComplaintMarked mdlCompMarked = new CM_ComplaintMarked();
                //  long _DivisionID = Convert.ToInt64(hdnDivisionID.Value);
                //List<long> UserList = bllComplaint.GetUsersForPmiuAndIrrigation((long)Constants.Designation.ADM, (long)Constants.IrrigationLevelID.Division, _DivisionID, (long)Constants.Organization.PMIU);
                //for (int i = 0; i < UserList.Count; i++)
                //{

                //    mdlCompNotification.UserID = UserList.ElementAt(i);


                //}
                mdlCompNotification.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                mdlCompNotification.UserID = Convert.ToInt64(ddlNotify.SelectedItem.Value);
                mdlCompNotification.UserDesigID = (long)Constants.Designation.SDO;
                mdlCompNotification.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlCompNotification.CreatedDate = DateTime.Now;
                mdlCompNotification.ModifiedDate = DateTime.Now;
                mdlCompNotification.ModifiedBy = Convert.ToInt32(mdlUser.ID);


                mdlCompMarked.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                mdlCompMarked.UserID = Convert.ToInt64(ddlNotify.SelectedItem.Value);
                mdlCompMarked.UserDesigID = (long)Constants.Designation.SDO;
                mdlCompMarked.MarkRead = false;
                mdlCompMarked.Starred = false;
                mdlCompMarked.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlCompMarked.CreatedDate = DateTime.Now;
                mdlCompMarked.ModifiedDate = DateTime.Now;
                mdlCompMarked.ModifiedBy = Convert.ToInt32(mdlUser.ID);


                using (TransactionScope transaction = new TransactionScope())
                {
                    bool IsSave = bllComplaint.AddComplaintNotification(mdlCompNotification);
                    bool IsSaved = bllComplaint.AddComplaintMarked(mdlCompMarked);
                    transaction.Complete();
                }
                BindCommentsResultsGrid(Convert.ToInt64(hdnComplaintID.Value));
                Master.ShowMessage("The concerned person has been notified", SiteMaster.MessageType.Success);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                String FileName = "";
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                CM_ComplaintComments mdlComplaintComment = new CM_ComplaintComments();
                CM_ComplaintAssignmentHistory mdlCompAssignHistory = new CM_ComplaintAssignmentHistory();
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Complaints);
                ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
                CM_ComplaintMarked mdlCompMarked = new CM_ComplaintMarked();
                long _DivisionID = Convert.ToInt64(hdnDivisionID.Value);
                List<long> UserList = bllComplaint.GetUsersForPmiuAndIrrigation((long)Constants.Designation.ADM, (long)Constants.IrrigationLevelID.Division, _DivisionID, (long)Constants.Organization.PMIU);

                if (UserList.Count == 0)
                {
                    Master.ShowMessage("The concerned ADM not found.", SiteMaster.MessageType.Success);
                    return;
                }
                foreach (var item in lstNameofFiles)
                {
                    FileName = item.Item3.ToString();

                }
                mdlComplaintComment.Comments = txtComments.Text;
                mdlComplaintComment.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                mdlComplaintComment.Attachment = FileName;
                mdlComplaintComment.CommentsDateTime = DateTime.Now;
                mdlComplaintComment.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserID = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserDesigID = mdlUser.DesignationID;
                mdlComplaintComment.CreatedDate = DateTime.Now;


                for (int i = 0; i < UserList.Count; i++)
                {

                    mdlCompAssignHistory.AssignedToUser = UserList.ElementAt(i);


                }
                mdlCompAssignHistory.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                // mdlCompNotification.UserID = Convert.ToInt64(ddlNotify.SelectedItem.Value);
                mdlCompAssignHistory.AssignedToDesig = (long)Constants.Designation.ADM;
                mdlCompAssignHistory.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlCompAssignHistory.CreatedDate = DateTime.Now;
                mdlCompAssignHistory.AssignedByDesig = mdlUser.DesignationID;
                mdlCompAssignHistory.AssignedByUser = Convert.ToInt32(mdlUser.ID);
                mdlCompAssignHistory.ComplaintStatusID = 2;
                mdlCompAssignHistory.AssignedDate = DateTime.Now;

                using (TransactionScope transaction = new TransactionScope())
                {
                    bool IsSaved = bllComplaint.AddQuickComplaintData(mdlComplaintComment);
                    bool IsSave = bllComplaint.AddComplaintAssignmentHistory(mdlCompAssignHistory);
                    transaction.Complete();
                }
                bool IsExistMarked = bllComplaint.GetComplaintMarkedByUserID(Convert.ToInt64(mdlCompAssignHistory.AssignedToUser), Convert.ToInt64(hdnComplaintID.Value));
                if (!IsExistMarked)
                {
                    mdlCompMarked.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                    mdlCompMarked.UserID = mdlCompAssignHistory.AssignedToUser;
                    mdlCompMarked.UserDesigID = (long)Constants.Designation.ADM;
                    mdlCompMarked.MarkRead = false;
                    mdlCompMarked.Starred = false;
                    mdlCompMarked.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlCompMarked.CreatedDate = DateTime.Now;
                    mdlCompMarked.ModifiedDate = DateTime.Now;
                    mdlCompMarked.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    bool Saved = bllComplaint.AddComplaintMarked(mdlCompMarked);
                }

                BindCommentsResultsGrid(Convert.ToInt64(hdnComplaintID.Value));
                GetComplaintDataByComplaintID(Convert.ToInt64(hdnComplaintID.Value));
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ComplaintID", Convert.ToInt32(hdnComplaintID.Value));
                _event.AddNotifyEvent((long)NotificationEventConstants.Complaints.XENAssignsthecomplainttoADM, SessionManagerFacade.UserInformation.ID);
                txtComments.Text = "";
                Master.ShowMessage("The concerned person has been notified", SiteMaster.MessageType.Success);
                // Response.Redirect("~/Modules/ComplaintsManagement/SearchComplaints.aspx",false);

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void btnSaveComments_Click(object sender, EventArgs e)
        {
            try
            {
                String FileName = "";
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                CM_ComplaintComments mdlComplaintComment = new CM_ComplaintComments();
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Complaints);
                ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();

                foreach (var item in lstNameofFiles)
                {
                    FileName = item.Item3.ToString();

                }
                mdlComplaintComment.Comments = txtComments.Text;
                mdlComplaintComment.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                mdlComplaintComment.Attachment = FileName;
                mdlComplaintComment.CommentsDateTime = DateTime.Now;
                mdlComplaintComment.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserID = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserDesigID = mdlUser.DesignationID;
                mdlComplaintComment.CreatedDate = DateTime.Now;
                bool IsSaved = bllComplaint.AddQuickComplaintData(mdlComplaintComment);

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ComplaintID", Convert.ToInt32(hdnComplaintID.Value));
                _event.AddNotifyEvent((long)NotificationEventConstants.Complaints.AcommentisaddedbyAdditionalAccessibilityuser, SessionManagerFacade.UserInformation.ID);

                BindCommentsResultsGrid(Convert.ToInt64(hdnComplaintID.Value));
                GetComplaintDataByComplaintID(Convert.ToInt64(hdnComplaintID.Value));
                Master.ShowMessage("Comments added successfully", SiteMaster.MessageType.Success);
            }


            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAssignToXEN_Click(object sender, EventArgs e)
        {
            try
            {
                String FileName = "";
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                CM_ComplaintComments mdlComplaintComment = new CM_ComplaintComments();
                CM_ComplaintAssignmentHistory mdlCompAssignHistory = new CM_ComplaintAssignmentHistory();
                UA_AssociatedLocation mdlAssociatedLocation = null;
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Complaints);
                ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
                long _DivisionID = Convert.ToInt64(hdnDivisionID.Value);
                mdlAssociatedLocation = bllComplaint.GetUserByDivisionID(_DivisionID);

                foreach (var item in lstNameofFiles)
                {
                    FileName = item.Item3.ToString();

                }
                // Comment 

                mdlComplaintComment.Comments = txtComments.Text;
                mdlComplaintComment.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                mdlComplaintComment.Attachment = FileName;
                mdlComplaintComment.CommentsDateTime = DateTime.Now;
                mdlComplaintComment.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserID = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserDesigID = mdlUser.DesignationID;
                mdlComplaintComment.CreatedDate = DateTime.Now;

                // Assignment History
                mdlCompAssignHistory.AssignedByUser = mdlUser.ID;
                mdlCompAssignHistory.AssignedByDesig = mdlUser.DesignationID;
                mdlCompAssignHistory.AssignedToUser = mdlAssociatedLocation.UserID;
                mdlCompAssignHistory.AssignedToDesig = mdlAssociatedLocation.DesignationID;
                mdlCompAssignHistory.ComplaintStatusID = 2;
                mdlCompAssignHistory.ComplaintID = Convert.ToInt32(hdnComplaintID.Value);
                mdlCompAssignHistory.AssignedDate = DateTime.Now;
                mdlCompAssignHistory.Remarks = txtComments.Text;
                mdlCompAssignHistory.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlCompAssignHistory.CreatedDate = DateTime.Now;



                using (TransactionScope transaction = new TransactionScope())
                {
                    bool IsSaved = bllComplaint.AddQuickComplaintData(mdlComplaintComment);
                    bool IsSave = bllComplaint.AddComplaintAssignmentHistory(mdlCompAssignHistory);
                    // Status Change if DDDH
                    if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirectorHelpline && hdnComplaintStatus.Value == "Resolved")
                    {
                        bool IsRecordUpdated = bllComplaint.UpdateComplaintStatusByComplaintID(Convert.ToInt64(hdnComplaintID.Value));
                    }
                    transaction.Complete();
                }

                if (mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("ComplaintID", Convert.ToInt32(hdnComplaintID.Value));
                    _event.AddNotifyEvent((long)NotificationEventConstants.Complaints.ADMassignsbacktoXEN, SessionManagerFacade.UserInformation.ID);
                }
                BindCommentsResultsGrid(Convert.ToInt64(hdnComplaintID.Value));
                GetComplaintDataByComplaintID(Convert.ToInt64(hdnComplaintID.Value));
                Master.ShowMessage("The Complaint successfully assigned to XEN", SiteMaster.MessageType.Success);
                txtComments.Text = "";
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnResolved_Click(object sender, EventArgs e)
        {
            try
            {
                String FileName = "";
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                CM_ComplaintComments mdlComplaintComment = new CM_ComplaintComments();
                CM_Complaint mdlComplaint = new CM_Complaint();
                UA_AssociatedLocation mdlAssociatedLocation = null;
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Complaints);
                ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
                long _DivisionID = Convert.ToInt64(hdnDivisionID.Value);
                mdlAssociatedLocation = bllComplaint.GetUserByDivisionID(_DivisionID);

                foreach (var item in lstNameofFiles)
                {
                    FileName = item.Item3.ToString();

                }
                // Comment 

                mdlComplaintComment.Comments = txtComments.Text;
                mdlComplaintComment.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                mdlComplaintComment.Attachment = FileName;
                mdlComplaintComment.CommentsDateTime = DateTime.Now;
                mdlComplaintComment.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserID = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserDesigID = mdlUser.DesignationID;
                mdlComplaintComment.CreatedDate = DateTime.Now;


                using (TransactionScope transaction = new TransactionScope())
                {
                    bool IsSaved = bllComplaint.AddQuickComplaintData(mdlComplaintComment);
                    bool IsSave = bllComplaint.CompalintResolvedByID(Convert.ToInt64(hdnComplaintID.Value));
                    transaction.Complete();
                }
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ComplaintID", Convert.ToInt32(hdnComplaintID.Value));
                _event.AddNotifyEvent((long)NotificationEventConstants.Complaints.ADMclosesthecomplaint, SessionManagerFacade.UserInformation.ID);
                BindCommentsResultsGrid(Convert.ToInt64(hdnComplaintID.Value));
                GetComplaintDataByComplaintID(Convert.ToInt64(hdnComplaintID.Value));
                Master.ShowMessage("The Complaint has been resolved", SiteMaster.MessageType.Success);
                txtComments.Text = "";
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAssignADM_Click(object sender, EventArgs e)
        {
            try
            {
                String FileName = "";
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                CM_ComplaintComments mdlComplaintComment = new CM_ComplaintComments();
                CM_ComplaintAssignmentHistory mdlCompAssignHistory = new CM_ComplaintAssignmentHistory();
                // UA_AssociatedLocation mdlAssociatedLocation = null;
                List<Tuple<string, string, string>> lstNameofFiles = FileUploadControl.UploadNow(Configuration.Complaints);
                ComplaintsManagementBLL bllComplaint = new ComplaintsManagementBLL();
                long _DivisionID = Convert.ToInt64(hdnDivisionID.Value);
                List<long> UserList = bllComplaint.GetUsersForPmiuAndIrrigation((long)Constants.Designation.ADM, (long)Constants.IrrigationLevelID.Division, _DivisionID, (long)Constants.Organization.PMIU);

                foreach (var item in lstNameofFiles)
                {
                    FileName = item.Item3.ToString();

                }
                // Comment 

                mdlComplaintComment.Comments = txtComments.Text;
                mdlComplaintComment.ComplaintID = Convert.ToInt64(hdnComplaintID.Value);
                mdlComplaintComment.Attachment = FileName;
                mdlComplaintComment.CommentsDateTime = DateTime.Now;
                mdlComplaintComment.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserID = Convert.ToInt32(mdlUser.ID);
                mdlComplaintComment.UserDesigID = mdlUser.DesignationID;
                mdlComplaintComment.CreatedDate = DateTime.Now;

                // Assignment History

                for (int i = 0; i < UserList.Count; i++)
                {

                    mdlCompAssignHistory.AssignedToUser = UserList.ElementAt(i);


                }

                mdlCompAssignHistory.AssignedByUser = mdlUser.ID;
                mdlCompAssignHistory.AssignedByDesig = mdlUser.DesignationID;
                // mdlCompAssignHistory.AssignedToUser = mdlAssociatedLocation.UserID;
                mdlCompAssignHistory.AssignedToDesig = (long)Constants.Designation.ADM;
                mdlCompAssignHistory.ComplaintStatusID = 2;
                mdlCompAssignHistory.ComplaintID = Convert.ToInt32(hdnComplaintID.Value);
                mdlCompAssignHistory.AssignedDate = DateTime.Now;
                mdlCompAssignHistory.Remarks = txtComments.Text;
                mdlCompAssignHistory.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlCompAssignHistory.CreatedDate = DateTime.Now;

                using (TransactionScope transaction = new TransactionScope())
                {
                    bool IsSaved = bllComplaint.AddQuickComplaintData(mdlComplaintComment);
                    bool IsSave = bllComplaint.AddComplaintAssignmentHistory(mdlCompAssignHistory);
                    // Status Change if DDDH
                    if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirectorHelpline && hdnComplaintStatus.Value == "Resolved")
                    {
                        bool IsRecordUpdated = bllComplaint.UpdateComplaintStatusByComplaintID(Convert.ToInt64(hdnComplaintID.Value));
                    }
                    transaction.Complete();
                }


                if (mdlUser.DesignationID != (long)Constants.Designation.DeputyDirectorHelpline)
                {
                    NotifyEvent _event = new NotifyEvent();
                    _event.Parameters.Add("ComplaintID", Convert.ToInt32(hdnComplaintID.Value));
                    _event.AddNotifyEvent((long)NotificationEventConstants.Complaints.XENAssignsthecomplainttoADM, SessionManagerFacade.UserInformation.ID);
                }
                BindCommentsResultsGrid(Convert.ToInt64(hdnComplaintID.Value));
                GetComplaintDataByComplaintID(Convert.ToInt64(hdnComplaintID.Value));
                Master.ShowMessage("The Complaint successfully assigned to ADM", SiteMaster.MessageType.Success);
                txtComments.Text = "";
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void lnkViewAttachments_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrCurrent = ((GridViewRow)((Control)sender).NamingContainer);
                long ID = Convert.ToInt64(gvComplaintComments.DataKeys[gvrCurrent.RowIndex].Values[ComplaintCommentsIDIndex]);
                List<CM_ComplaintComments> lstComplaintComments = new ComplaintsManagementBLL().GetAllComplaintCommentsByID(ID);
                gvViewAttachment.DataSource = lstComplaintComments;
                gvViewAttachment.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Attachments", "$('#viewAttachment').modal();", true);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}