using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.Notifications;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection
{
    public partial class ActionOnSchedule : BasePage
    {
        #region ViewState Constants

        string PreparedByIDString = "PreparedByID";
        string StatusIDString = "StatusID";
        string PreparedByDesignationIDString = "PreparedByDesignationID";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    txtComments.Attributes.Add("maxlength", txtComments.MaxLength.ToString());

                    if (!string.IsNullOrEmpty(Request.QueryString["ScheduleID"]))
                    {
                        long ScheduleID = Convert.ToInt64(Request.QueryString["ScheduleID"]);
                        hlScheduleDetail.NavigateUrl = "~/Modules/ScheduleInspection/ScheduleDetail.aspx?ScheduleID=" + ScheduleID;
                        btnSave.Visible = base.CanAdd;
                        long PreparedByID = 0;
                        long ScheduleStatusID = 0;
                        long DesignationID = 0;
                        BindScheduleData(ScheduleID, ref PreparedByID, ref ScheduleStatusID, ref DesignationID);
                        ViewState[PreparedByIDString] = PreparedByID;
                        ViewState[StatusIDString] = ScheduleStatusID;
                        hdnScheduleStatusID.Value = Convert.ToString(ScheduleStatusID);
                        hdnSchedulePreparedByID.Value = Convert.ToString(PreparedByID);
                        hdnSchedulePreparedByDesignationID.Value = Convert.ToString(DesignationID);
                        BindScreenControls(PreparedByID, ScheduleStatusID, DesignationID);
                        BindCommentHistory(ScheduleID);
                        hlBack.NavigateUrl = "~/Modules/ScheduleInspection/SearchSchedule.aspx";
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function binds the Schedule data to the top panel.
        /// It also returns PreparedByID and StatusID.
        /// Created On 15-07-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <param name="_PreparedByID"></param>
        /// <param name="_ScheduleStatusID"></param>

        private void BindScheduleData(long _ScheduleID, ref long _PreparedByID, ref long _ScheduleStatusID, ref long _DesignationID)
        {
            dynamic ScheduleData = new ScheduleInspectionBLL().GetScheduleData(_ScheduleID);
            string Title = ScheduleData.GetType().GetProperty("Name").GetValue(ScheduleData, null);
            string Status = ScheduleData.GetType().GetProperty("Status").GetValue(ScheduleData, null);
            string PreparedBy = ScheduleData.GetType().GetProperty("PreparedBy").GetValue(ScheduleData, null);
            _PreparedByID = ScheduleData.GetType().GetProperty("PreparedByID").GetValue(ScheduleData, null);
            _DesignationID = ScheduleData.GetType().GetProperty("PreparedByDesignationID").GetValue(ScheduleData, null);
            DateTime FromDate = ScheduleData.GetType().GetProperty("FromDate").GetValue(ScheduleData, null);
            DateTime ToDate = ScheduleData.GetType().GetProperty("ToDate").GetValue(ScheduleData, null);
            _ScheduleStatusID = ScheduleData.GetType().GetProperty("StatusID").GetValue(ScheduleData, null);
            ViewState[PreparedByDesignationIDString] = ScheduleData.GetType().GetProperty("PreparedByDesignationID").GetValue(ScheduleData, null);
            PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls.ScheduleDetail.Title = Title;
            PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls.ScheduleDetail.Status = Status;
            PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls.ScheduleDetail.PreparedBy = PreparedBy;
            PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls.ScheduleDetail.FromDate = Utility.GetFormattedDate(FromDate);
            PMIU.WRMIS.Web.Modules.ScheduleInspection.Controls.ScheduleDetail.ToDate = Utility.GetFormattedDate(ToDate);
        }

        /// <summary>
        /// This function binds the screen controls
        /// Created on 14-07-2016
        /// </summary>
        /// <param name="_PreparedByID"></param>
        /// <param name="_ScheduleStatusID"></param>
        private void BindScreenControls(long _PreparedByID, long _ScheduleStatusID, long _DesignationID)
        {
            bool IsScheduleInspectiosnsExists = new ScheduleInspectionBLL().IsScheduleInspectionsExists(Convert.ToInt64(Request.QueryString["ScheduleID"]));
            bool IsWorksInspectionExists = new ScheduleInspectionBLL().IsWorkInspectionExists(Convert.ToInt64(Request.QueryString["ScheduleID"]));
            bool IsgeneralInspectionExists = new ScheduleInspectionBLL().IsGeneralInspectionExists(Convert.ToInt64(Request.QueryString["ScheduleID"]));
            if (IsScheduleInspectiosnsExists == true || IsWorksInspectionExists == true || IsgeneralInspectionExists == true)
            {
                txtComments.Enabled = false;
                ddlAction.CssClass = "form-control";
                ddlAction.Enabled = false;
                btnSave.Enabled = false;
                return;
            }


            UA_Users mdlUser = SessionManagerFacade.UserInformation;
            if (mdlUser.DesignationID == (long)Constants.Designation.MA || mdlUser.DesignationID == (long)Constants.Designation.SDO)
            {
                if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                {
                    txtComments.Enabled = true;
                    ddlAction.Enabled = true;
                    ddlAction.Items.Add(new ListItem("Send for Approval", ((long)Constants.SIScheduleStatus.PendingForApproval).ToString()));
                    ddlAction.CssClass = "form-control required";
                    btnSave.Enabled = true;
                }
                else if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                {
                    txtComments.Enabled = false;
                    ddlAction.Enabled = false;
                    ddlAction.CssClass = "form-control";
                    btnSave.Enabled = false;
                }
                else if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                {
                    txtComments.Enabled = true;
                    ddlAction.Enabled = true;
                    ddlAction.Items.Add(new ListItem("Reschedule", ((long)Constants.SIScheduleStatus.Prepared).ToString()));
                    ddlAction.CssClass = "form-control required";
                    btnSave.Enabled = true;
                }
                else if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                {
                    txtComments.Enabled = false;
                    ddlAction.Enabled = false;
                    ddlAction.CssClass = "form-control";
                    btnSave.Enabled = false;
                }
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.ADM || mdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                {
                    txtComments.Enabled = true;
                    ddlAction.Enabled = true;
                    ddlAction.Items.Add(new ListItem("Send for Approval", ((long)Constants.SIScheduleStatus.PendingForApproval).ToString()));
                    ddlAction.CssClass = "form-control required";
                    btnSave.Enabled = true;
                }
                else if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                {
                    if (mdlUser.ID == _PreparedByID)
                    {
                        txtComments.Enabled = false;
                        ddlAction.Enabled = false;
                        ddlAction.CssClass = "form-control";
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
                        {
                            txtComments.Enabled = true;
                            ddlAction.Enabled = true;
                            ddlAction.Items.Add(new ListItem("Approve", ((long)Constants.SIScheduleStatus.Approved).ToString()));
                            ddlAction.Items.Add(new ListItem("Reject", ((long)Constants.SIScheduleStatus.Rejected).ToString()));
                            ddlAction.Items.Add(new ListItem("Send for Approval", ((long)Constants.SIScheduleStatus.PendingForApproval).ToString()));
                            ddlAction.Items.Add(new ListItem("Send Back for Rework", ((long)Constants.SIScheduleStatus.Prepared).ToString()));
                            ddlAction.CssClass = "form-control required";
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            txtComments.Enabled = true;
                            ddlAction.Enabled = true;
                            ddlAction.Items.Add(new ListItem("Approve", ((long)Constants.SIScheduleStatus.Approved).ToString()));
                            ddlAction.Items.Add(new ListItem("Reject", ((long)Constants.SIScheduleStatus.Rejected).ToString()));
                            ddlAction.Items.Add(new ListItem("Send Back for Rework", ((long)Constants.SIScheduleStatus.Prepared).ToString()));
                            ddlAction.CssClass = "form-control required";
                            btnSave.Enabled = true;
                        }
                    }
                }
                else if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                {
                    if (mdlUser.ID == _PreparedByID)
                    {
                        txtComments.Enabled = true;
                        ddlAction.Enabled = true;
                        ddlAction.Items.Add(new ListItem("Reschedule", ((long)Constants.SIScheduleStatus.Prepared).ToString()));
                        ddlAction.CssClass = "form-control required";
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        txtComments.Enabled = false;
                        ddlAction.Enabled = false;
                        ddlAction.CssClass = "form-control";
                        btnSave.Enabled = false;
                    }
                }
                else if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected)
                {
                    txtComments.Enabled = false;
                    ddlAction.Enabled = false;
                    ddlAction.CssClass = "form-control";
                    btnSave.Enabled = false;
                }
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.DeputyDirector || mdlUser.DesignationID == (long)Constants.Designation.SE)
            {
                if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                {
                    if (mdlUser.ID != _PreparedByID)
                    {
                        if (_DesignationID == (long)Constants.Designation.SDO)
                        {
                            txtComments.Enabled = false;
                            ddlAction.Enabled = false;
                            ddlAction.CssClass = "form-control";
                            btnSave.Enabled = false;
                        }
                        else
                        {
                            txtComments.Enabled = true;
                            ddlAction.Enabled = true;
                            ddlAction.Items.Add(new ListItem("Approve", ((long)Constants.SIScheduleStatus.Approved).ToString()));
                            ddlAction.Items.Add(new ListItem("Reject", ((long)Constants.SIScheduleStatus.Rejected).ToString()));
                            ddlAction.Items.Add(new ListItem("Send Back for Rework", ((long)Constants.SIScheduleStatus.Prepared).ToString()));
                            ddlAction.Items.Add(new ListItem("Send for Approval to Director Gauges", ((long)Constants.SIScheduleStatus.PendingForApproval).ToString()));
                            ddlAction.CssClass = "form-control required";
                            btnSave.Enabled = true;
                        }

                    }
                }
                else if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                {
                    if (mdlUser.ID != _PreparedByID)
                    {
                        txtComments.Enabled = false;
                        ddlAction.Enabled = false;
                        ddlAction.CssClass = "form-control";
                        btnSave.Enabled = false;
                    }
                }
            }
            else if (mdlUser.DesignationID == (long)Constants.Designation.DirectorGauges)
            {
                if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                {
                    if (mdlUser.ID != _PreparedByID)
                    {
                        if (_DesignationID == (long)Constants.Designation.SDO)
                        {
                            txtComments.Enabled = false;
                            ddlAction.Enabled = false;
                            ddlAction.CssClass = "form-control";
                            btnSave.Enabled = false;
                        }
                        else
                        {
                            txtComments.Enabled = true;
                            ddlAction.Enabled = true;
                            ddlAction.Items.Add(new ListItem("Approve", ((long)Constants.SIScheduleStatus.Approved).ToString()));
                            ddlAction.Items.Add(new ListItem("Reject", ((long)Constants.SIScheduleStatus.Rejected).ToString()));
                            ddlAction.Items.Add(new ListItem("Send Back for Rework", ((long)Constants.SIScheduleStatus.Prepared).ToString()));
                            ddlAction.CssClass = "form-control required";
                            btnSave.Enabled = true;
                        }

                    }
                }
                else if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                {
                    if (mdlUser.ID != _PreparedByID)
                    {
                        txtComments.Enabled = false;
                        ddlAction.Enabled = false;
                        ddlAction.CssClass = "form-control";
                        btnSave.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ActionOnSchedule);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the comment history
        /// Created on 20-07-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        private void BindCommentHistory(long _ScheduleID)
        {
            List<SI_ScheduleStatusDetail> lstScheduleStatus = new ScheduleInspectionBLL().GetScheduleStatuses(_ScheduleID);
            if (lstScheduleStatus.Count > 0)
            {
                gvrptCommentHistory.DataSource = lstScheduleStatus;
                gvrptCommentHistory.DataBind();
                litCommentHistoryTitle.Visible = true;
                gvrptCommentHistory.Visible = true;
            }

        }

        public string GetFormattedDate(string _CommentsDate)
        {
            var CommentsDate = "";
            try
            {
                DateTime CommentDate = DateTime.Parse(_CommentsDate);
                CommentsDate = Convert.ToString(Utility.GetFormattedDate(CommentDate));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
            return CommentsDate;
        }
        //protected void rptCommentHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    try
        //    {
        //        Literal litCommentDate = (Literal)e.Item.FindControl("litCommentDate");
        //        DateTime CommentDate = DateTime.Parse(litCommentDate.Text);
        //        litCommentDate.Text = Utility.GetFormattedDate(CommentDate);
        //    }
        //    catch (WRException exp)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                long ScheduleID = Convert.ToInt64(Request.QueryString["ScheduleID"]);

                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                long SendToUserID = 0;
                long SendToDesignationID = 0;
                string SuccessMessage = string.Empty;

                long ScheduleStatusID = Convert.ToInt64(ddlAction.SelectedItem.Value);
                string Comments = txtComments.Text;

                GetSendToUserAndDesignationIDs(ScheduleStatusID, mdlUser, out SendToUserID, out SendToDesignationID, out SuccessMessage);

                if (SendToUserID != 0)
                {
                    SI_ScheduleStatusDetail mdlScheduleStatusDetail = new SI_ScheduleStatusDetail
                    {
                        SendToUserID = SendToUserID,
                        SendToUserDesignationID = SendToDesignationID,
                        ScheduleID = ScheduleID,
                        ScheduleStatusID = ScheduleStatusID,
                        Comments = Comments,
                        CommentsDate = DateTime.Now,
                        UserID = mdlUser.ID,
                        DesignationID = mdlUser.DesignationID,
                        CreatedDate = DateTime.Now,
                        CreatedBy = mdlUser.ID
                    };
                    long scheduleStatusID = Convert.ToInt64(hdnScheduleStatusID.Value);
                    new ScheduleInspectionBLL().AddScheduleStatusDetail(mdlScheduleStatusDetail, Convert.ToInt64(hdnScheduleStatusID.Value), Convert.ToInt64(hdnSchedulePreparedByID.Value));
                    //LogScheduleInspectionNotifiation(Convert.ToInt64(Request.QueryString["ScheduleID"]));
                    SearchSchedule.IsSaved = true;
                    SearchSchedule.ActionMessage = SuccessMessage;
                    Response.Redirect("SearchSchedule.aspx", false);
                    //Master.ShowMessage(SuccessMessage, SiteMaster.MessageType.Success);
                }
                else
                {
                    Master.ShowMessage(Message.AssignedToUserNotAvailable.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function finds and returns the SendTo UserID and DesignationID
        /// It also provides the message and will indicate the notification type.
        /// Created On 22-07-2016
        /// </summary>
        /// <param name="_ScheduleStatusID"></param>
        /// <param name="_MdlUser"></param>
        /// <param name="_SendToUserID"></param>
        /// <param name="_SendToDesignationID"></param>
        /// <param name="_Message"></param>
        private void GetSendToUserAndDesignationIDs(long _ScheduleStatusID, UA_Users _MdlUser, out long _SendToUserID, out long _SendToDesignationID, out string _Message)
        {
            _SendToUserID = 0;
            _SendToDesignationID = 0;
            _Message = string.Empty;

            UA_UserManager mdlUserManager = new UserBLL().GetUserManager(_MdlUser.ID);

            long PreparedByID = Convert.ToInt64(ViewState[PreparedByIDString]);
            long StatusID = Convert.ToInt64(ViewState[StatusIDString]);
            long PreparedByDesignationID = Convert.ToInt64(ViewState[PreparedByDesignationIDString]);

            if (_MdlUser.DesignationID == (long)Constants.Designation.MA || _MdlUser.DesignationID == (long)Constants.Designation.SDO)
            {
                if (StatusID == (long)Constants.SIScheduleStatus.Prepared)
                {
                    if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                    {
                        if (mdlUserManager != null)
                        {
                            _SendToUserID = mdlUserManager.ManagerID.Value;
                            _SendToDesignationID = mdlUserManager.ManagerDesignationID.Value;
                            _Message = Message.ScheduleApprovalSuccess.Description;
                        }
                    }
                }
                else if (StatusID == (long)Constants.SIScheduleStatus.Approved)
                {
                    if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                    {
                        _SendToUserID = _MdlUser.ID;
                        _SendToDesignationID = _MdlUser.DesignationID.Value;
                        _Message = Message.RecordSaved.Description;
                    }
                }
            }
            else if (_MdlUser.DesignationID == (long)Constants.Designation.ADM || _MdlUser.DesignationID == (long)Constants.Designation.XEN)
            {
                if (StatusID == (long)Constants.SIScheduleStatus.Prepared)
                {
                    if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                    {
                        _SendToUserID = mdlUserManager.ManagerID.Value;
                        _SendToDesignationID = mdlUserManager.ManagerDesignationID.Value;
                        _Message = Message.ScheduleApprovalSuccess.Description;
                    }
                }
                else if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                {
                    if (_MdlUser.ID != PreparedByID)
                    {
                        if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                        {
                            if (mdlUserManager != null)
                            {
                                _SendToUserID = mdlUserManager.ManagerID.Value;
                                _SendToDesignationID = mdlUserManager.ManagerDesignationID.Value;
                                _Message = Message.ScheduleApprovalSuccess.Description;
                            }
                        }
                        else
                        {
                            _SendToUserID = PreparedByID;
                            _SendToDesignationID = PreparedByDesignationID;

                            if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                _Message = Message.ScheduleApproved.Description;
                            }
                            else
                            {
                                _Message = Message.RecordSaved.Description;
                            }
                        }
                    }
                }
                else if (StatusID == (long)Constants.SIScheduleStatus.Approved)
                {
                    if (_MdlUser.ID == PreparedByID)
                    {
                        if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                        {
                            _SendToUserID = _MdlUser.ID;
                            _SendToDesignationID = _MdlUser.DesignationID.Value;
                            _Message = Message.RecordSaved.Description;
                        }
                    }
                }
            }
            else if (_MdlUser.DesignationID == (long)Constants.Designation.DeputyDirector || _MdlUser.DesignationID == (long)Constants.Designation.SE)
            {
                if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                {
                    if (_MdlUser.ID != PreparedByID)
                    {

                        if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                        {
                            if (mdlUserManager != null)
                            {
                                _SendToUserID = mdlUserManager.ManagerID.Value;
                                _SendToDesignationID = mdlUserManager.ManagerDesignationID.Value;
                                _Message = Message.ScheduleApprovalSuccess.Description;
                            }
                        }
                        else
                        {
                            _SendToUserID = PreparedByID;
                            _SendToDesignationID = PreparedByDesignationID;

                            if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                _Message = Message.ScheduleApproved.Description;
                            }
                            else
                            {
                                _Message = Message.RecordSaved.Description;
                            }
                        }
                    }

                    #region Changed by Rizwan alvi
                    //if (_MdlUser.ID != PreparedByID)
                    //{

                    //    if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved || _ScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected ||
                    //        _ScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                    //    {
                    //        _SendToUserID = PreparedByID;
                    //        _SendToDesignationID = PreparedByDesignationID;

                    //        if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                    //        {
                    //            _Message = Message.ScheduleApproved.Description;
                    //        }
                    //        else
                    //        {
                    //            _Message = Message.RecordSaved.Description;
                    //        }
                    //    }
                    //}
                    #endregion Changed by Rizwan alvi
                }
            }
            else if (_MdlUser.DesignationID == (long)Constants.Designation.DirectorGauges)
            {
                if (StatusID == (long)Constants.SIScheduleStatus.PendingForApproval)
                {
                    if (_MdlUser.ID != PreparedByID)
                    {
                        if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved || _ScheduleStatusID == (long)Constants.SIScheduleStatus.Rejected ||
                            _ScheduleStatusID == (long)Constants.SIScheduleStatus.Prepared)
                        {

                            _SendToUserID = PreparedByID;
                            _SendToDesignationID = PreparedByDesignationID;

                            if (_ScheduleStatusID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                _Message = Message.ScheduleApproved.Description;
                            }
                            else
                            {
                                _Message = Message.RecordSaved.Description;
                            }
                        }
                    }
                }
            }
        }

        private void LogScheduleInspectionNotifiation(long _ScheduleID)
        {
            try
            {
                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ScheduleID", _ScheduleID);
                long _ActionID = Convert.ToInt64(ddlAction.SelectedItem.Value);
                long scheduleStatusID = Convert.ToInt64(hdnScheduleStatusID.Value);
                long schedulePreparedByID = Convert.ToInt64(hdnSchedulePreparedByID.Value);
                long schedulePreparedByDesignationID = Convert.ToInt64(hdnSchedulePreparedByDesignationID.Value);
                long eventID = 0;
                switch (SessionManagerFacade.UserInformation.DesignationID)
                {
                    case (long)Constants.Designation.SDO:

                        if (scheduleStatusID == (long)Constants.SIScheduleStatus.Prepared && _ActionID == (long)Constants.SIScheduleStatus.PendingForApproval)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfSDOIsAssignedToXENForApproval;
                        }
                        break;
                    case (long)Constants.Designation.XEN:
                        if (schedulePreparedByID == SessionManagerFacade.UserInformation.ID) // Schedule prepared by XEN himself is only send for approval
                        {
                            if (scheduleStatusID == (long)Constants.SIScheduleStatus.Prepared && _ActionID == (long)Constants.SIScheduleStatus.PendingForApproval)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfXENIsAssignedToSEForApproval;
                            }
                        }
                        else // SDO sent schedule to XEN for approval, XEN can approve, reject, send back for rework
                        {
                            if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfSDOIsApprovedByXEN;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Rejected)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfSDOIsRejectedByXEN;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Prepared)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfSDOIsSendBackForReworkByXEN;
                            }
                        }

                        break;
                    case (long)Constants.Designation.SE:
                        if (schedulePreparedByID != SessionManagerFacade.UserInformation.ID) // XEN Prepared Schedule
                        {
                            // XEN send schedule to SE for approval, SE can approve, reject, send back for rework
                            if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfXENIsApprovedBySE;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Rejected)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfXENIsRejectedBySE;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Prepared)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfXENIsSendBackForReworkBySE;
                            }
                        }
                        break;
                    case (long)Constants.Designation.MA:
                        if (scheduleStatusID == (long)Constants.SIScheduleStatus.Prepared && _ActionID == (long)Constants.SIScheduleStatus.PendingForApproval)
                        {
                            eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsAssignedToADMForApproval;
                        }
                        break;
                    case (long)Constants.Designation.ADM:
                        if (schedulePreparedByID == SessionManagerFacade.UserInformation.ID) // Schedule prepared by ADM himself is only send for approval
                        {
                            if (scheduleStatusID == (long)Constants.SIScheduleStatus.Prepared && _ActionID == (long)Constants.SIScheduleStatus.PendingForApproval)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsAssignedToDDForApproval;
                            }
                        }
                        else if (schedulePreparedByID != SessionManagerFacade.UserInformation.ID) // Schedule of MA, ADM can forward MA schedule to DD for approval
                        {
                            if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.PendingForApproval)
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsAssignedToDDForApproval;
                        }
                        else // MA sent schedule to ADM for approval, ADM can approve, reject, send back for rework
                        {
                            if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsApprovedByADM;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Rejected)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsRejectedByADM;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Prepared)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsSendBackForReworkByADM;
                            }
                        }

                        break;
                    case (long)Constants.Designation.DeputyDirector:
                        if (schedulePreparedByID != SessionManagerFacade.UserInformation.ID && schedulePreparedByDesignationID != (int)Constants.Designation.ADM) // ADM Prepared Schedule
                        {
                            // ADM send schedule to DD for approval, DD can approve, reject, send back for rework
                            if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsApprovedByDD;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Rejected)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsRejectedByDD;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Prepared)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfADMIsSendBackForReworkByDD;
                            }
                        }
                        else
                        {
                            // ADM forward MA schedule to DD for approval, DD can approve, reject, send back for rework
                            if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Approved)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsApprovedByDD;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Rejected)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsRejectedByDD;
                            }
                            else if (scheduleStatusID == (long)Constants.SIScheduleStatus.PendingForApproval && _ActionID == (long)Constants.SIScheduleStatus.Prepared)
                            {
                                eventID = (long)NotificationEventConstants.ScheduleInspection.ScheduleOfMAIsSendBackForReworkByDD;
                            }
                        }
                        break;
                    default:
                        break;
                }
                if (eventID > 0)
                    _event.AddNotifyEvent(eventID, SessionManagerFacade.UserInformation.ID);

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}