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
using PMIU.WRMIS.BLL.DailyData;

namespace PMIU.WRMIS.Web.Modules.ScheduleInspection.ReferenceData
{
    public partial class ReasonForChange : BasePage
    {
        List<CO_ReasonForChange> lstReasonForChange = new List<CO_ReasonForChange>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    SetPageTitle();
                    BindGrid();
                }
                
            }
            catch(Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReasonForChange_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstReasonForChange = new ReferenceDataBLL().GetAllReasonForChange();
                    CO_ReasonForChange mdlReasonForChange = new CO_ReasonForChange();

                    mdlReasonForChange.ID = 0;
                    mdlReasonForChange.Name = "";
                    mdlReasonForChange.Description = "";
                    lstReasonForChange.Add(mdlReasonForChange);

                    gvReasonForChange.PageIndex = gvReasonForChange.PageCount;
                    gvReasonForChange.DataSource = lstReasonForChange;
                    gvReasonForChange.DataBind();

                    gvReasonForChange.EditIndex = gvReasonForChange.Rows.Count - 1;
                    gvReasonForChange.DataBind();
                    gvReasonForChange.Rows[gvReasonForChange.Rows.Count - 1].FindControl("txtReasonForChange").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReasonForChange_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvReasonForChange.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReasonForChange_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ReasonForChangeID = Convert.ToInt32(((Label)gvReasonForChange.Rows[e.RowIndex].FindControl("lblID")).Text);

                ReferenceDataBLL bllReasonForChange = new ReferenceDataBLL();

                //bool IsExist = bllReasonForChange.IsGaugeTypeIDExists(ReasonForChangeID);

                //if (IsExist)
                //{
                //    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                //    return;
                //}

                bool IsDeleted = bllReasonForChange.DeleteReasonForChange(ReasonForChangeID);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReasonForChange_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                long ReasonForChangeID = Convert.ToInt32(((Label)gvReasonForChange.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string ReasonForChange = ((TextBox)gvReasonForChange.Rows[RowIndex].Cells[1].FindControl("txtReasonForChange")).Text.Trim();
                string ReasonForChangeDesc = ((TextBox)gvReasonForChange.Rows[RowIndex].Cells[2].FindControl("txtReasonForChangeDesc")).Text.Trim();

                ReferenceDataBLL bllReasonForChange = new ReferenceDataBLL();
                CO_ReasonForChange mdlSearchedReasonForChange = bllReasonForChange.GetReasonForChangeByName(ReasonForChange);

                if (mdlSearchedReasonForChange != null && ReasonForChangeID != mdlSearchedReasonForChange.ID)
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                CO_ReasonForChange mdlReasonForChange = new CO_ReasonForChange();
                mdlReasonForChange.ID = ReasonForChangeID;
                mdlReasonForChange.Name = ReasonForChange;
                mdlReasonForChange.Description = ReasonForChangeDesc;

                bool IsRecordSaved = false;

                if (ReasonForChangeID == 0)
                {
                    IsRecordSaved = bllReasonForChange.AddReasonForChange(mdlReasonForChange);
                }
                else
                {
                    IsRecordSaved = bllReasonForChange.UpdateReasonForChange(mdlReasonForChange);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (ReasonForChangeID == 0)
                    {
                        gvReasonForChange.PageIndex = 0;
                    }
                    gvReasonForChange.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReasonForChange_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvReasonForChange.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReasonForChange_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvReasonForChange.EditIndex = e.NewEditIndex;
                BindGrid();
                gvReasonForChange.Rows[e.NewEditIndex].FindControl("txtReasonForChange").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvReasonForChange_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvReasonForChange.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 09-02-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.ReasonForChange);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function bind Reason for change to the grid.
        /// Created On 09-02-2016
        /// </summary>
        private void BindGrid()
        {
            lstReasonForChange = new ReferenceDataBLL().GetAllReasonForChange();
            gvReasonForChange.DataSource = lstReasonForChange;
            gvReasonForChange.DataBind();
        }

        protected void gvReasonForChange_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Button btnAdd = (Button)e.Row.FindControl("btnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Visible = (bool)mdlRoleRights.BEdit;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = (bool)mdlRoleRights.BDelete;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}