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

namespace PMIU.WRMIS.Web.Modules.SmallSpillways.ReferenceData
{
    public partial class SpillwayTypeSD : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindSpillwayTypeGrid();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion
        #region Events
        protected void gvSpillwayType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSpillwayType.PageIndex = e.NewPageIndex;
                gvSpillwayType.EditIndex = -1;
                BindSpillwayTypeGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSpillwayType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvSpillwayType.EditIndex = -1;
                BindSpillwayTypeGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSpillwayType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddSpillway")
                {
                    List<object> lstSpillwayType = new SmallDamsBLL().GetSpillwayType();
                    lstSpillwayType.Add(new
                    {
                        SpillwayTypeID = 0,
                        Name = string.Empty,
                        Description = string.Empty,
                        IsActive = true,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvSpillwayType.PageIndex = gvSpillwayType.PageCount;
                    gvSpillwayType.DataSource = lstSpillwayType;
                    gvSpillwayType.DataBind();

                    gvSpillwayType.EditIndex = gvSpillwayType.Rows.Count - 1;
                    gvSpillwayType.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSpillwayType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);
                    //SpillwayTypeID,Name,Description,IsActive,CreatedBy,CreatedDate
                    #region "Data Keys"
                    DataKey key = gvSpillwayType.DataKeys[e.Row.RowIndex];
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
                    if (gvSpillwayType.EditIndex == e.Row.RowIndex)
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

        protected void gvSpillwayType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvSpillwayType.DataKeys[e.RowIndex].Values[0]);
                bool IsDeleted = new SmallDamsBLL().DeleteSpillwayType(Convert.ToInt16(ID));
                if (IsDeleted)
                {
                    BindSpillwayTypeGrid();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSpillwayType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvSpillwayType.EditIndex = e.NewEditIndex;
                BindSpillwayTypeGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSpillwayType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                #region "Data Keys"
                //SpillwayTypeID,Name,Description,IsActive,CreatedBy,CreatedDate
                DataKey key = gvSpillwayType.DataKeys[e.RowIndex];
                string SpillwayTypeID = Convert.ToString(key.Values["SpillwayTypeID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvSpillwayType.Rows[e.RowIndex];
                TextBox Name = (TextBox)row.FindControl("txtSpillwayName");
                TextBox Description = (TextBox)row.FindControl("txtDescription");
                CheckBox IsActive = (CheckBox)row.FindControl("cb_Active");
                #endregion

                SD_SpillwayType dSpillwayType = new SD_SpillwayType();

                dSpillwayType.ID = Convert.ToInt16(SpillwayTypeID);
                if (SpillwayTypeID == "0")
                {
                    dSpillwayType.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    dSpillwayType.CreatedDate = DateTime.Now;
                }
                else
                {
                    dSpillwayType.CreatedBy = Convert.ToInt32(CreatedBy);
                    dSpillwayType.CreatedDate = Convert.ToDateTime(CreatedDate);
                    dSpillwayType.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    dSpillwayType.ModifiedDate = DateTime.Now;
                }

                if (Name != null)
                    dSpillwayType.Name = Name.Text;

                if (Description != null)
                    dSpillwayType.Description = Description.Text;
                
                dSpillwayType.IsActive = IsActive.Checked;



                if (new SmallDamsBLL().IsSpillwayTypeUnique(dSpillwayType))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsSave = new SmallDamsBLL().SaveSpillwayType(dSpillwayType);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dSpillwayType.ID) == 0)
                        gvSpillwayType.PageIndex = 0;

                    gvSpillwayType.EditIndex = -1;
                    BindSpillwayTypeGrid();
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

        private void BindSpillwayTypeGrid()
        {
            try
            {
                List<object> lstSpillwayType = new SmallDamsBLL().GetSpillwayType();

                gvSpillwayType.DataSource = lstSpillwayType;
                gvSpillwayType.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteSpillway");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }
        #endregion

    }
}



