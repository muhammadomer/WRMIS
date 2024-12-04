using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Tenders.ReferenceData
{
    public partial class TenderEvaluationCommittee : BasePage
    {
        List<TM_CommitteeMembers> lstCommitteeMembers = new List<TM_CommitteeMembers>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderEvaluationCommittee_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstCommitteeMembers = new TenderManagementBLL().GetAllCommitteeMembers();
                    TM_CommitteeMembers mdlCommitteeMembers = new TM_CommitteeMembers();

                    mdlCommitteeMembers.ID = 0;
                    mdlCommitteeMembers.Name = "";
                    mdlCommitteeMembers.Designation = "";
                    mdlCommitteeMembers.MobilePhone = "";
                    mdlCommitteeMembers.Email = "";
                    lstCommitteeMembers.Add(mdlCommitteeMembers);

                    gvTenderEvaluationCommittee.PageIndex = gvTenderEvaluationCommittee.PageCount;
                    gvTenderEvaluationCommittee.DataSource = lstCommitteeMembers;
                    gvTenderEvaluationCommittee.DataBind();

                    gvTenderEvaluationCommittee.EditIndex = gvTenderEvaluationCommittee.Rows.Count - 1;
                    gvTenderEvaluationCommittee.DataBind();
                    gvTenderEvaluationCommittee.Rows[gvTenderEvaluationCommittee.Rows.Count - 1].FindControl("txtName").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderEvaluationCommittee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvTenderEvaluationCommittee.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderEvaluationCommittee_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;
                long CommitteeMemberID = Convert.ToInt32(((Label)gvTenderEvaluationCommittee.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string CommitteeMemberName = ((TextBox)gvTenderEvaluationCommittee.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string Designation = ((TextBox)gvTenderEvaluationCommittee.Rows[RowIndex].Cells[2].FindControl("txtDesignation")).Text.Trim();
                string MobilePhone = ((TextBox)gvTenderEvaluationCommittee.Rows[RowIndex].Cells[3].FindControl("txtMobilePhone")).Text.Trim();
                string Email = ((TextBox)gvTenderEvaluationCommittee.Rows[RowIndex].Cells[4].FindControl("txtEmail")).Text.Trim();

                TenderManagementBLL bllTenderManagement = new TenderManagementBLL();
                TM_CommitteeMembers mdlSearchedCommitteeMember = bllTenderManagement.GetCommitteeMembersByName(CommitteeMemberName);

                if (mdlSearchedCommitteeMember != null && CommitteeMemberID != mdlSearchedCommitteeMember.ID)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }

                //bool IsExist = bllTenderManagement.IsPhoneOrEmailExistInTendrCommittee(MobilePhone, Email);
                //if (IsExist)
                //{
                //    Master.ShowMessage("Phone or Email Already Exist", SiteMaster.MessageType.Error);
                //    return;

                //}
                TM_CommitteeMembers mdlCommitteeMember = new TM_CommitteeMembers();

                mdlCommitteeMember.ID = CommitteeMemberID;
                mdlCommitteeMember.Name = CommitteeMemberName;
                mdlCommitteeMember.Designation = Designation;
                mdlCommitteeMember.MobilePhone = MobilePhone;
                mdlCommitteeMember.Email = Email;
                mdlCommitteeMember.IsActive = true;

                if (bllTenderManagement.IsCommetteeMembersExist(mdlCommitteeMember))
                {
                    if (mdlCommitteeMember.ID == 0)
                        Master.ShowMessage("Designation or Phone or Email Already Exist", SiteMaster.MessageType.Error);
                    else
                        Master.ShowMessage("Phone or Email Already Exist", SiteMaster.MessageType.Error);
                    return;
                }
                bool IsRecordSaved = false;

                if (CommitteeMemberID == 0)
                {
                    mdlCommitteeMember.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlCommitteeMember.CreatedDate = DateTime.Today;
                    IsRecordSaved = bllTenderManagement.AddCommitteeMembers(mdlCommitteeMember);
                }
                else
                {
                    mdlCommitteeMember.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlCommitteeMember.ModifiedDate = DateTime.Today;
                    IsRecordSaved = bllTenderManagement.UpdateCommitteeMembers(mdlCommitteeMember);
                }
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (CommitteeMemberID == 0)
                    {
                        gvTenderEvaluationCommittee.PageIndex = 0;
                    }
                    gvTenderEvaluationCommittee.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderEvaluationCommittee_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvTenderEvaluationCommittee.EditIndex = e.NewEditIndex;
                BindGrid();
                gvTenderEvaluationCommittee.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderEvaluationCommittee_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long CommitteeMemberID = Convert.ToInt32(((Label)gvTenderEvaluationCommittee.Rows[e.RowIndex].FindControl("lblID")).Text);
                TenderManagementBLL bllTenderManagement = new TenderManagementBLL();
                bool IsExist = bllTenderManagement.IsMemberExistInTendrCommittee(CommitteeMemberID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllTenderManagement.DeleteCommitteeMembers(CommitteeMemberID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderEvaluationCommittee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTenderEvaluationCommittee.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTenderEvaluationCommittee_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvTenderEvaluationCommittee.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            lstCommitteeMembers = new TenderManagementBLL().GetAllCommitteeMembers();
            gvTenderEvaluationCommittee.DataSource = lstCommitteeMembers;
            gvTenderEvaluationCommittee.DataBind();
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