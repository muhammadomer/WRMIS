using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.SmallDams.ReferenceData
{
    public partial class DamTypeSD : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDamTypeGrid();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion
        #region Events
        protected void gvDamType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDamType.PageIndex = e.NewPageIndex;
                gvDamType.EditIndex = -1;
                BindDamTypeGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvDamType.EditIndex = -1;
                BindDamTypeGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddDam")
                {
                    List<object> lstDamType = new SmallDamsBLL().GetDamType();
                    lstDamType.Add(new
                    {
                        DamTypeID = 0,
                        Name = string.Empty,
                        Description = string.Empty,
                        IsActive = true,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvDamType.PageIndex = gvDamType.PageCount;
                    gvDamType.DataSource = lstDamType;
                    gvDamType.DataBind();

                    gvDamType.EditIndex = gvDamType.Rows.Count - 1;
                    gvDamType.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);
                    //DamTypeID,Name,Description,IsActive,CreatedBy,CreatedDate
                    #region "Data Keys"
                    DataKey key = gvDamType.DataKeys[e.Row.RowIndex];
                    int ID = Convert.ToInt32(key.Values["ID"]);
                    string name = Convert.ToString(key.Values["Name"]);
                    string description = Convert.ToString(key.Values["Description"]);
                    bool isActive = Convert.ToBoolean(key.Values["IsActive"]);
                    
                    #endregion

                    #region "Controls"
                    TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                    TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");
                    CheckBox cbIsActive = (CheckBox)e.Row.FindControl("IsActive");
                    Label lblActive = (Label)e.Row.FindControl("lblActive");
                    #endregion
                    if (key.Values["IsActive"].Equals(true))
                    {
                        lblActive.Text = "Yes";
                    }
                    else
                    {
                        lblActive.Text = "No";
                    }
                    if (gvDamType.EditIndex == e.Row.RowIndex)
                    {

                        if (!string.IsNullOrEmpty(name))
                            txtName.Text = name;

                        if (!string.IsNullOrEmpty(description))
                            txtDescription.Text = description;
                        
                        if (key.Values["IsActive"].Equals(true))
                        {
                            cbIsActive.Checked = true;
                        }
                        else
                        {
                            cbIsActive.Checked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvDamType.DataKeys[e.RowIndex].Values[0]);
                bool IsDeleted = new SmallDamsBLL().DeleteDamType(Convert.ToInt16(ID));
                if (IsDeleted)
                {
                    BindDamTypeGrid();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvDamType.EditIndex = e.NewEditIndex;
                BindDamTypeGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDamType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                #region "Data Keys"
                //DamTypeID,Name,Description,IsActive,CreatedBy,CreatedDate
                DataKey key = gvDamType.DataKeys[e.RowIndex];
                string DamTypeID = Convert.ToString(key.Values["DamTypeID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvDamType.Rows[e.RowIndex];
                TextBox Name = (TextBox)row.FindControl("txtDamName");
                TextBox Description = (TextBox)row.FindControl("txtDescription");
                CheckBox IsActive = (CheckBox)row.FindControl("cb_Active");
                #endregion

                SD_DamType dDamType = new SD_DamType();

                dDamType.ID = Convert.ToInt16(DamTypeID);
                if (DamTypeID == "0")
                {
                    dDamType.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    dDamType.CreatedDate = DateTime.Now;
                }
                else
                {
                    dDamType.CreatedBy = Convert.ToInt32(CreatedBy);
                    dDamType.CreatedDate = Convert.ToDateTime(CreatedDate);
                    dDamType.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    dDamType.ModifiedDate = DateTime.Now;
                }

                if (Name != null)
                    dDamType.Name = Name.Text;

                if (Description != null)
                    dDamType.Description = Description.Text;

                    dDamType.IsActive = IsActive.Checked;



                if (new SmallDamsBLL().IsDamTypeUnique(dDamType))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsSave = new SmallDamsBLL().SaveDamType(dDamType);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dDamType.ID) == 0)
                        gvDamType.PageIndex = 0;

                    gvDamType.EditIndex = -1;
                    BindDamTypeGrid();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion

        #region Funtions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SmallDams);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindDamTypeGrid()
        {
            try
            {
                List<object> lstDamType = new SmallDamsBLL().GetDamType();

                gvDamType.DataSource = lstDamType;
                gvDamType.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteDam");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }
        #endregion

    }
}



