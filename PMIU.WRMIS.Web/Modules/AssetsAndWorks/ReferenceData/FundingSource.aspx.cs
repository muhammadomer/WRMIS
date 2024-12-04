using PMIU.WRMIS.BLL.Tenders;
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

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData
{
    public partial class FundingSource : BasePage
    {
        List<TM_FundingSource> lstFundingSource = new List<TM_FundingSource>();
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

        protected void gvFundingSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstFundingSource = new TenderManagementBLL().GetAllFundingSources();
                    TM_FundingSource mdlFundingSource = new TM_FundingSource();

                    mdlFundingSource.ID = 0;
                    mdlFundingSource.FundingSource = "";
                    mdlFundingSource.FundingType = "";
                    mdlFundingSource.Description = "";
                    mdlFundingSource.IsActive = true;
                    lstFundingSource.Add(mdlFundingSource);

                    gvFundingSource.PageIndex = gvFundingSource.PageCount;
                    gvFundingSource.DataSource = lstFundingSource;
                    gvFundingSource.DataBind();

                    gvFundingSource.EditIndex = gvFundingSource.Rows.Count - 1;
                    gvFundingSource.DataBind();
                    gvFundingSource.Rows[gvFundingSource.Rows.Count - 1].FindControl("txtFundingSource").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundingSource_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvFundingSource.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundingSource_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                int RowIndex = e.RowIndex;
                long FundingSourceID = Convert.ToInt32(((Label)gvFundingSource.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string FundingSource = ((TextBox)gvFundingSource.Rows[RowIndex].Cells[1].FindControl("txtFundingSource")).Text.Trim();
                string FundingType = ((DropDownList)gvFundingSource.Rows[RowIndex].Cells[2].FindControl("ddlFundingType")).SelectedValue == string.Empty ? null : ((DropDownList)gvFundingSource.Rows[RowIndex].Cells[2].FindControl("ddlFundingType")).SelectedItem.Text;
                string Description = ((TextBox)gvFundingSource.Rows[RowIndex].Cells[3].FindControl("txtDescription")).Text.Trim();

                bool isActive = ((CheckBox)gvFundingSource.Rows[RowIndex].FindControl("cb_Active")).Checked;

                TenderManagementBLL bllTenderManagement = new TenderManagementBLL();
                TM_FundingSource mdlSearchedFundingSource = bllTenderManagement.GetFundingSourceByName(FundingSource);

                if (mdlSearchedFundingSource != null && FundingSourceID != mdlSearchedFundingSource.ID)
                {
                    Master.ShowMessage(Message.RecordAlreadyExist.Description, SiteMaster.MessageType.Error);
                    return;
                }
                TM_FundingSource mdlFundingSource = new TM_FundingSource();

                mdlFundingSource.ID = FundingSourceID;
                mdlFundingSource.FundingSource = FundingSource;
                mdlFundingSource.FundingType = FundingType;
                mdlFundingSource.Description = Description;
                mdlFundingSource.IsActive = isActive;

                bool IsRecordSaved = false;

                if (FundingSourceID == 0)
                {
                    mdlFundingSource.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlFundingSource.CreatedDate = DateTime.Today;
                    IsRecordSaved = bllTenderManagement.AddFundingSource(mdlFundingSource);
                }
                else
                {
                    mdlFundingSource.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    mdlFundingSource.ModifiedDate = DateTime.Today;
                    IsRecordSaved = bllTenderManagement.UpdateFundingSource(mdlFundingSource);
                }
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (FundingSourceID == 0)
                    {
                        gvFundingSource.PageIndex = 0;
                    }
                    gvFundingSource.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundingSource_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvFundingSource.EditIndex = e.NewEditIndex;
                BindGrid();
                gvFundingSource.Rows[e.NewEditIndex].FindControl("txtFundingSource").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundingSource_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int FundingSourceID = Convert.ToInt32(((Label)gvFundingSource.Rows[e.RowIndex].FindControl("lblID")).Text);
                TenderManagementBLL bllTenderManagement = new TenderManagementBLL();
                bool IsExist = bllTenderManagement.IsFundingSourceIDExists(FundingSourceID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllTenderManagement.DeleteFundingSource(FundingSourceID);

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



        protected void gvFundingSource_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFundingSource.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundingSource_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvFundingSource.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvFundingSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (gvFundingSource.EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddlFundingType = (DropDownList)e.Row.FindControl("ddlFundingType");
                    Label lblHdnLevel = (Label)e.Row.FindControl("lblHdnLevel");

                    if (lblHdnLevel != null)
                    {
                        ddlFundingType.ClearSelection();
                        if (lblHdnLevel.Text.Trim().ToUpper() == "FIXED")
                        {
                            Dropdownlist.SetSelectedValue(ddlFundingType, lblHdnLevel.Text);
                        }
                        else if (lblHdnLevel.Text.Trim().ToUpper() == "NOT FIXED")
                        {
                            Dropdownlist.SetSelectedValue(ddlFundingType, lblHdnLevel.Text);
                        }

                    }

                }
            }

            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void BindGrid()
        {
            lstFundingSource = new TenderManagementBLL().GetAllFundingSources();
            gvFundingSource.DataSource = lstFundingSource;
            gvFundingSource.DataBind();
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }


    }
}