using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.BLL.FloodOperations;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.ReferenceData
{
    public partial class ProblemNature : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindGridView();
                }
                SetPageTitle();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindGridView()
        {
            try
            {
                List<object> lstProblemNature = new FloodOperationsBLL().GetProblemNatureList();
                gvProblemNature.DataSource = lstProblemNature;
                gvProblemNature.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvProblemNature_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Add")
                {
                    List<object> lstRepresentative = new FloodOperationsBLL().GetProblemNatureList();

                    lstRepresentative.Add(
                    new
                    {
                        ID = 0,
                        Name = string.Empty,
                        Description = string.Empty,
                        IsActive = 1,
                        CreatedDate = DateTime.Now,
                        CreatedBy = string.Empty
                    });

                    gvProblemNature.PageIndex = gvProblemNature.PageCount;
                    gvProblemNature.DataSource = lstRepresentative;
                    gvProblemNature.DataBind();

                    gvProblemNature.EditIndex = gvProblemNature.Rows.Count - 1;
                    gvProblemNature.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemNature_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header) // If header created
                {
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    Button btnAddProblem = (Button)e.Row.FindControl("btnAddProblem");
                    if (mdlRoleRights != null)
                        btnAddProblem.Enabled = (bool)mdlRoleRights.BAdd;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    Button btnEditProblem = (Button)e.Row.FindControl("btnEditProblem");
                    Button btnDeleteProblem = (Button)e.Row.FindControl("btnDeleteProblem");
                    if (btnEditProblem != null)
                        btnEditProblem.Enabled = (bool)mdlRoleRights.BEdit;
                    if (btnDeleteProblem != null)
                        btnDeleteProblem.Enabled = (bool)mdlRoleRights.BDelete;

                    if (gvProblemNature.EditIndex == e.Row.RowIndex)
                    {
                        //if (Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
                        //AddEditConfirmMessage(e);

                        #region "Data Keys"
                        DataKey key = gvProblemNature.DataKeys[e.Row.RowIndex];
                        string name = Convert.ToString(key.Values["Name"]);
                        string description = Convert.ToString(key.Values["Description"]);
                        #endregion

                        #region "Controls"
                        TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                        TextBox txtDetail = (TextBox)e.Row.FindControl("txtDetail");
                        TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");
                        #endregion

                        if (txtName != null)
                        {
                            if (!string.IsNullOrEmpty(name))
                                txtName.Text = name;
                        }

                        if (txtDescription != null)
                        {
                            if (!string.IsNullOrEmpty(description))
                                txtDescription.Text = description;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvProblemNature_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvProblemNature.EditIndex = e.NewEditIndex;
                BindGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvProblemNature_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvProblemNature.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemNature_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvProblemNature.Rows[e.RowIndex];

                #region "Controls"
                TextBox txtName = (TextBox)row.FindControl("txtName");
                TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                #endregion

                #region "Datakeys"
                DataKey key = gvProblemNature.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                string ProblemNatureID = Convert.ToString(gvProblemNature.DataKeys[e.RowIndex].Values[0]);

                FO_ProblemNature NatureProblem = new FO_ProblemNature();

                NatureProblem.ID = Convert.ToInt16(ProblemNatureID);

                if (txtName != null)
                    NatureProblem.Name = txtName.Text;

                if (txtDescription != null && txtDescription.Text != "")
                    NatureProblem.Description = txtDescription.Text;

                if (new FloodOperationsBLL().IsProblemNatureExists(NatureProblem))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (NatureProblem.ID == 0)
                {
                    NatureProblem.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    NatureProblem.CreatedDate = DateTime.Now;
                }
                else
                {
                    NatureProblem.CreatedBy = Convert.ToInt32(CreatedBy);
                    NatureProblem.CreatedDate = Convert.ToDateTime(CreatedDate);
                    NatureProblem.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    NatureProblem.ModifiedDate = DateTime.Now;
                }

                NatureProblem.IsActive = true;

                bool IsSave = new FloodOperationsBLL().SaveProblemNature(NatureProblem);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(ProblemNatureID) == 0)
                        gvProblemNature.PageIndex = 0;

                    gvProblemNature.EditIndex = -1;
                    BindGridView();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemNature_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ProblemNatureID = Convert.ToString(gvProblemNature.DataKeys[e.RowIndex].Values[0]);

                if (!IsValidDelete(Convert.ToInt64(ProblemNatureID)))
                {
                    return;
                }

                bool IsDeleted = new FloodOperationsBLL().DeleteProblemNature(Convert.ToInt64(ProblemNatureID));
                if (IsDeleted)
                {
                    BindGridView();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvProblemNature_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvProblemNature.PageIndex = e.NewPageIndex;
                gvProblemNature.EditIndex = -1;
                BindGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDeleteProblem = (Button)_e.Row.FindControl("btnDeleteProblem");

            if (btnDeleteProblem != null)
            {
                btnDeleteProblem.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }

        }

        private bool IsValidDelete(long _ProblemNatureID)
        {
           FloodOperationsBLL bl= new FloodOperationsBLL();
            bool IsExist = bl.IsProblemNatureIDExists(_ProblemNatureID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }
    }
}