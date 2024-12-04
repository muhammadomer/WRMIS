using PMIU.WRMIS.BLL.Accounts;
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

namespace PMIU.WRMIS.Web.Modules.Accounts.ReferenceData
{
    public partial class HeadOfAccount : BasePage
    {
        #region Grid Data Key Index

        public const int AccountsHeadIDIndex = 0;

        #endregion

        List<AT_AccountsHead> lstAccountsHead = null;

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

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 06-04-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds data to the grid
        /// Created On 06-04-2017
        /// </summary>
        private void BindGrid()
        {
            lstAccountsHead = new ReferenceDataBLL().GetAccountHeads();

            gvAccountHead.DataSource = lstAccountsHead;
            gvAccountHead.DataBind();
        }

        protected void gvAccountHead_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAccountHead.EditIndex = e.NewEditIndex;

                BindGrid();

                gvAccountHead.Rows[e.NewEditIndex].FindControl("txtHeadName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAccountHead_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAccountHead.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAccountHead_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAccountHead.PageIndex = e.NewPageIndex;

                gvAccountHead.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAccountHead_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstAccountsHead = new ReferenceDataBLL().GetAccountHeads();

                    AT_AccountsHead mdlAccountsHead = new AT_AccountsHead();

                    lstAccountsHead.Add(mdlAccountsHead);

                    gvAccountHead.PageIndex = gvAccountHead.PageCount;
                    gvAccountHead.DataSource = lstAccountsHead;
                    gvAccountHead.EditIndex = (lstAccountsHead.Count - 1) % gvAccountHead.PageSize;
                    gvAccountHead.DataBind();
                    gvAccountHead.Rows[gvAccountHead.Rows.Count - 1].FindControl("txtPartName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAccountHead_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long AccountsHeadID = Convert.ToInt64(gvAccountHead.DataKeys[RowIndex].Values[AccountsHeadIDIndex]);
                string HeadName = ((TextBox)gvAccountHead.Rows[RowIndex].FindControl("txtHeadName")).Text.Trim();
                string Description = ((TextBox)gvAccountHead.Rows[RowIndex].FindControl("txtDescription")).Text.Trim();

                if (!IsValidAddEdit(HeadName, AccountsHeadID))
                {
                    return;
                }

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                ReferenceDataBLL bllReferenceData = new ReferenceDataBLL();

                AT_AccountsHead mdlAccountsHead = bllReferenceData.GetAccountHeadByID(AccountsHeadID);

                int UserID = Convert.ToInt32(mdlUsers.ID);

                if (mdlAccountsHead == null)
                {
                    mdlAccountsHead = new AT_AccountsHead
                    {
                        HeadName = HeadName,
                        Description = Description,
                        CreatedBy = UserID,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = UserID,
                        ModifiedDate = DateTime.Now,
                        IsActive = true
                    };

                    bllReferenceData.AddAccountHead(mdlAccountsHead);
                }
                else
                {
                    mdlAccountsHead.HeadName = HeadName;
                    mdlAccountsHead.Description = Description;
                    mdlAccountsHead.ModifiedBy = UserID;
                    mdlAccountsHead.ModifiedDate = DateTime.Now;

                    bllReferenceData.UpdateAccountHead(mdlAccountsHead);
                }

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                if (AccountsHeadID == 0)
                {
                    gvAccountHead.PageIndex = 0;
                }

                gvAccountHead.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This functions checks if the record is valid
        /// Created On 06-04-2017
        /// </summary>
        /// <param name="_VehicleType"></param>
        /// <param name="_PartName"></param>
        /// <param name="_KeyPartsID"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(string _HeadName, long _AccountsHeadID)
        {
            AT_AccountsHead mdlAccountsHead = new ReferenceDataBLL().GetAccountHeadByName(_HeadName);

            if (mdlAccountsHead != null && _AccountsHeadID != mdlAccountsHead.ID)
            {
                Master.ShowMessage(Message.HeadNameExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        protected void gvAccountHead_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long AccountsHeadID = Convert.ToInt64(gvAccountHead.DataKeys[e.RowIndex].Values[AccountsHeadIDIndex]);

                if (!IsValidDelete(AccountsHeadID))
                {
                    return;
                }

                new ReferenceDataBLL().DeleteAccountHead(AccountsHeadID);

                Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);

                BindGrid();
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _AccountHeadID)
        {
            bool IsExist = new ReferenceDataBLL().IsAccountHeadIDExists(_AccountHeadID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }
    }
}