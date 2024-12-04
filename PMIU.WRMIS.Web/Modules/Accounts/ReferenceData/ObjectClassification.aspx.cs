using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts.ReferenceData
{
    public partial class ObjectClassification : BasePage
    {
        ReferenceDataBLL ACBLL = new ReferenceDataBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDDLAccountHead();
                    gvObjectClassification.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindDDLAccountHead()
        {
            List<AT_AccountsHead> listOAH = ACBLL.GetAccountHeads(true);
            List<object> lOAH = (from item in listOAH where item.ID == item.ID select new { ID = item.ID, Name = item.HeadName }).ToList<object>();
            Dropdownlist.BindDropdownlist<List<object>>(ddlAccountsHead, lOAH);
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvObjectClassification_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvObjectClassification.EditIndex = e.NewEditIndex;
                BindGrid();

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvObjectClassification_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                List<object> lstUnits = ACBLL.GetObjectClassificationByAccoundHeadID(Convert.ToInt64(hfAccountHeadID.Value));
                object mdlObj = new { ID = 0, AccountsCode = "", ObjectClassification = "" };
                lstUnits.Add(mdlObj);
                gvObjectClassification.PageIndex = gvObjectClassification.PageCount;
                gvObjectClassification.DataSource = lstUnits;
                gvObjectClassification.DataBind();
                gvObjectClassification.EditIndex = gvObjectClassification.Rows.Count - 1;
                gvObjectClassification.DataBind();
            }

        }

        protected void gvObjectClassification_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvObjectClassification.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindGrid()
        {
            List<object> lstAS = ACBLL.GetObjectClassificationByAccoundHeadID(Convert.ToInt64(hfAccountHeadID.Value));
            gvObjectClassification.DataSource = lstAS;
            gvObjectClassification.DataBind();
        }

        protected void gvObjectClassification_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                DataKey key = gvObjectClassification.DataKeys[e.RowIndex];
                long ModifiedBy = SessionManagerFacade.UserInformation.ID;
                long ID = Convert.ToInt64(key.Values["ID"]);
                TextBox txtAccountCode = (TextBox)gvObjectClassification.Rows[RowIndex].FindControl("txtAccountCode");
                TextBox txtObjectClassification = (TextBox)gvObjectClassification.Rows[RowIndex].FindControl("txtObjectClassification");
                string AccountCode = txtAccountCode == null ? "" : txtAccountCode.Text;
                string ObjectClassification = txtObjectClassification == null ? "" : txtObjectClassification.Text;

                AT_ObjectClassification frt = new AT_ObjectClassification();
                frt.ID = ID;
                frt.AccountsCode = AccountCode;
                frt.ObjectClassification = ObjectClassification;
                frt.AccountHeadID = Convert.ToInt64(hfAccountHeadID.Value);
                AT_ObjectClassification Unique = ACBLL.GetObjectClassificationByAccountCode(frt);
                if (Unique == null || Unique.ID == 0)
                {
                    if (ID == 0)
                    {
                        frt.CreatedDate = DateTime.Now;
                        frt.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                    }
                    else
                    {
                        frt.ModifiedDate = DateTime.Now;
                        frt.ModifiedBy = (int)SessionManagerFacade.UserInformation.ID;
                    }
                    if (ACBLL.SaveObjectClassification(frt))
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                    }
                    gvObjectClassification.EditIndex = -1;
                    BindGrid();
                }
                else
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvObjectClassification_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                AT_ObjectClassification mdl = new AT_ObjectClassification();
                mdl.ID = Convert.ToInt64(((Label)gvObjectClassification.Rows[e.RowIndex].FindControl("lblID")).Text); ;
                AT_FundReleaseDetails associ = ACBLL.GetObjectClassification(mdl.ID);
                if (associ == null || associ.ID == 0)
                {
                    bool IsDeleted = ACBLL.DeleteObjectClassification(mdl);
                    if (IsDeleted)
                    {
                        Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                        BindGrid();
                    }
                    else
                        Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                }
                else
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlAccountsHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfAccountHeadID.Value = ddlAccountsHead.SelectedItem.Value;
            if (!string.IsNullOrEmpty(hfAccountHeadID.Value))
            {
                gvObjectClassification.DataSource = ACBLL.GetObjectClassificationByAccoundHeadID(Convert.ToInt64(hfAccountHeadID.Value));
                gvObjectClassification.DataBind();
                gvObjectClassification.Visible = true;
            }
            else
            {
                //gvObjectClassification.DataSource = new List<object>();
                //gvObjectClassification.DataBind();
                gvObjectClassification.Visible = false;
            }

        }



    }
}