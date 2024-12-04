using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Tenders.TenderNotice
{
    public partial class ViewTenderEvaluationCommittee : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    if (!string.IsNullOrEmpty(Request.QueryString["TenderWorkID"]))
                    {

                        hdnTenderWorkID.Value = Request.QueryString["TenderWorkID"];
                        long TenderNoticeID = new TenderManagementBLL().GetTenderNoticeIDByTenderWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
                        hlBack.NavigateUrl = "~/Modules/Tenders/Works/AddWorks.aspx?TenderNoticeID=" + TenderNoticeID;
                        if (!string.IsNullOrEmpty(Request.QueryString["WorkSourceID"]))
                        {
                            hdnWorkSourceID.Value = Request.QueryString["WorkSourceID"];
                        }
                        BindEvalCommitteeData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));


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

        private void BindEvalCommitteeData(long _TenderWorkID, long _WorkSourceID)
        {
            try
            {
                dynamic WorkData = new TenderManagementBLL().GetClosureWorkDataByID(_WorkSourceID, _TenderWorkID);
                Tenders.Controls.ViewWorks.TenderNotice = Utility.GetDynamicPropertyValue(WorkData, "TenderNotice");
                Tenders.Controls.ViewWorks.WorkName = Utility.GetDynamicPropertyValue(WorkData, "WorkName");
                Tenders.Controls.ViewWorks.WorkType = Utility.GetDynamicPropertyValue(WorkData, "WorkType");

                List<dynamic> CommitteeMembersData = new TenderManagementBLL().GetCommitteeMembersListByWorkID(_TenderWorkID);
                gvViewTenderEvalCommittee.DataSource = CommitteeMembersData;
                gvViewTenderEvalCommittee.DataBind();

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvViewTenderEvalCommittee_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddEvalCommitteeItem")
                {
                    List<object> lstCommitteeMembers = new TenderManagementBLL().GetEvalCommitteeMember(Convert.ToInt64(hdnTenderWorkID.Value));

                    lstCommitteeMembers.Add(
                    new
                    {
                        ID = 0,
                        MembersName = string.Empty,
                        Designation = string.Empty,
                        Mobile = string.Empty,
                        Email = string.Empty

                    });

                    gvViewTenderEvalCommittee.PageIndex = gvViewTenderEvalCommittee.PageCount;
                    gvViewTenderEvalCommittee.DataSource = lstCommitteeMembers;
                    gvViewTenderEvalCommittee.DataBind();

                    gvViewTenderEvalCommittee.EditIndex = gvViewTenderEvalCommittee.Rows.Count - 1;
                    gvViewTenderEvalCommittee.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<dynamic> GetCommitteeMembersAutoComplete(string _Name)
        {
            try
            {
                _Name = _Name.Trim();
                List<dynamic> lstCommitteeMembers = new TenderManagementBLL().GetCommitteeMembersName(_Name);

                return lstCommitteeMembers;
            }
            catch (Exception exp)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);

                return null;
            }
        }

        protected void gvViewTenderEvalCommittee_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvViewTenderEvalCommittee.Rows[e.RowIndex];
                Label lblID = (Label)row.FindControl("lblID");
                TextBox txtMemberName = (TextBox)row.FindControl("txtMemberName");
                TextBox txtDesignation = (TextBox)row.FindControl("txtDesignation");
                TextBox txtMemberID = (TextBox)row.FindControl("txtMemberID");
                TextBox Mobile = (TextBox)row.FindControl("txtMobile");
                TextBox Email = (TextBox)row.FindControl("txtEmail");
                string MobilePhone = Mobile.Text;
                string EmailMember = Email.Text;


                TM_CommitteeMembers mdlCommitteeMembers = new TM_CommitteeMembers();
                TM_TenderCommitteeMembers mdlTenderCommitteeMembers = new TM_TenderCommitteeMembers();
                if (txtMemberID.Text == "-1")
                {
                    bool IsExist = new TenderManagementBLL().IsPhoneOrEmailExistInTendrCommittee(MobilePhone, EmailMember);
                    if (IsExist)
                    {
                        Master.ShowMessage("Phone or Email Already Exist", SiteMaster.MessageType.Error);
                        return;

                    }
                    mdlCommitteeMembers.ID = Convert.ToInt32(lblID.Text);
                    mdlCommitteeMembers.Name = txtMemberName.Text;
                    mdlCommitteeMembers.Designation = txtDesignation.Text;
                    mdlCommitteeMembers.MobilePhone = Mobile.Text;
                    mdlCommitteeMembers.Email = Email.Text;
                    mdlCommitteeMembers.IsActive = true;
                    mdlCommitteeMembers.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    mdlCommitteeMembers.CreatedDate = DateTime.Now;
                    long ID = new TenderManagementBLL().SaveCommitteeMember(mdlCommitteeMembers);

                    if (ID > 0 && lblID.Text == "0")
                    {
                        mdlTenderCommitteeMembers.CommitteeMembersID = ID;
                        mdlTenderCommitteeMembers.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
                        mdlTenderCommitteeMembers.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                        mdlTenderCommitteeMembers.CreatedDate = DateTime.Now;
                        bool IsSaved = new TenderManagementBLL().SaveTenderCommitteeMember(mdlTenderCommitteeMembers);
                    }
                }
                else
                {
                    bool IsExists = new TenderManagementBLL().IsMemberAlreadyAdded(Convert.ToInt64(txtMemberID.Text), Convert.ToInt64(hdnTenderWorkID.Value));
                    if (IsExists)
                    {
                        Master.ShowMessage(Message.TM_MemberAlreadyExists.Description, SiteMaster.MessageType.Error);
                        return;
                    }

                    mdlCommitteeMembers.ID = Convert.ToInt32(txtMemberID.Text);
                    mdlCommitteeMembers.Name = txtMemberName.Text;
                    mdlCommitteeMembers.Designation = txtDesignation.Text;
                    mdlCommitteeMembers.MobilePhone = Mobile.Text;
                    mdlCommitteeMembers.Email = Email.Text;
                    mdlCommitteeMembers.IsActive = true;
                    mdlCommitteeMembers.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    mdlCommitteeMembers.ModifiedDate = DateTime.Now;

                    bool IsSave = new TenderManagementBLL().UpdateCommitteeMembers(mdlCommitteeMembers);

                    mdlTenderCommitteeMembers.CommitteeMembersID = Convert.ToInt32(txtMemberID.Text);
                    mdlTenderCommitteeMembers.TenderWorksID = Convert.ToInt64(hdnTenderWorkID.Value);
                    mdlTenderCommitteeMembers.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    mdlTenderCommitteeMembers.CreatedDate = DateTime.Now;
                    bool IsSaved = new TenderManagementBLL().SaveTenderCommitteeMember(mdlTenderCommitteeMembers);
                }
                gvViewTenderEvalCommittee.EditIndex = -1;
                BindEvalCommitteeData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvViewTenderEvalCommittee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvViewTenderEvalCommittee.EditIndex = -1;
                BindEvalCommitteeData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvViewTenderEvalCommittee_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                gvViewTenderEvalCommittee.EditIndex = e.NewEditIndex;
                BindEvalCommitteeData(Convert.ToInt64(hdnTenderWorkID.Value), Convert.ToInt64(hdnWorkSourceID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvViewTenderEvalCommittee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool? IsAwarded = new TenderManagementBLL().GetAwardedTenderByWorkID(Convert.ToInt64(hdnTenderWorkID.Value));
            UA_Users mdlUsers = SessionManagerFacade.UserInformation;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long ID = Convert.ToInt64(gvViewTenderEvalCommittee.DataKeys[e.Row.RowIndex].Value);
                if (IsAwarded == true || mdlUsers.DesignationID != (long)Constants.Designation.ChiefMonitoring)
                {
                    Button btnEdit = (Button)e.Row.FindControl("btnEditEvalCommitteeGrid");
                    btnEdit.CssClass += " disabled";
                }

       

            }


            GridViewRow header = gvViewTenderEvalCommittee.HeaderRow;
            if (header != null)
            { 
                Button btnEvalCommitteeGrid = header.FindControl("btnEvalCommitteeGrid") as Button;

            if (btnEvalCommitteeGrid != null)
            {

                if (IsAwarded == true)
                {
                    btnEvalCommitteeGrid.CssClass += " disabled";
                }
            }
            }

        }
    }
}