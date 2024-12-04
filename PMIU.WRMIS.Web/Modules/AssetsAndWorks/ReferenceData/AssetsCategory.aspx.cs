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
using PMIU.WRMIS.BLL.AssetsAndWorks;

namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData
{
    public partial class AssetsCategory : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindAssetCategory();
                    SetPageTitle();
                }
               
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        #region "GridView Events"
        private void BindAssetCategory()
        {
            try
            {
                List<object> lstAssetCategory = new AssetsWorkBLL().GetAssetAllCategoryList();
                gvAssetCategory.DataSource = lstAssetCategory;
                gvAssetCategory.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "AddCategory")
                {
                    List<object> lstAddCategory = new AssetsWorkBLL().GetAssetAllCategoryList();

                    lstAddCategory.Add(
                    new
                    {
                        ID = 0,
                        Name = string.Empty,
                        Description = string.Empty,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = string.Empty
                    });

                    gvAssetCategory.PageIndex = gvAssetCategory.PageCount;
                    gvAssetCategory.DataSource = lstAddCategory;
                    gvAssetCategory.DataBind();

                    gvAssetCategory.EditIndex = gvAssetCategory.Rows.Count - 1;
                    gvAssetCategory.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gvAssetCategory.DataKeys[e.Row.RowIndex];
                    int ID = Convert.ToInt32(key.Values["ID"]);
                    string name = Convert.ToString(key.Values["Name"]);
                    string description = Convert.ToString(key.Values["Description"]);
                    #endregion

                    #region "Controls"
                    TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                    TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");
                    Button btnEditCategory = (Button)e.Row.FindControl("btnEditCategory");
                    Button btnDeleteCategory = (Button)e.Row.FindControl("btnDeleteCategory");
                    #endregion
                    if (name == Convert.ToString(Constants.AssetCategory.Infrastructure) || name == Convert.ToString(Constants.AssetCategory.MachineryEquipment))
                    {
                        btnEditCategory.Visible = false;
                        btnDeleteCategory.Visible = false;
                    }
                    if (gvAssetCategory.EditIndex == e.Row.RowIndex)
                    {

                        if (!string.IsNullOrEmpty(name))
                            txtName.Text = name;

                        if (!string.IsNullOrEmpty(description))
                            txtDescription.Text = description;



                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetCategory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAssetCategory.EditIndex = e.NewEditIndex;
                BindAssetCategory();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAssetCategory.EditIndex = -1;
                BindAssetCategory();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvAssetCategory.Rows[e.RowIndex];

                #region "Controls"
                TextBox txtName = (TextBox)row.FindControl("txtName");
                TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                CheckBox ChkActive = ((CheckBox)row.FindControl("ChkActive"));
                #endregion

                #region "Datakeys"
                DataKey key = gvAssetCategory.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                string CategoryID = Convert.ToString(gvAssetCategory.DataKeys[e.RowIndex].Values[0]);

                AM_Category AssetCategory = new AM_Category();

                AssetCategory.ID = Convert.ToInt32(CategoryID);

                if (txtName.Text != "")
                    AssetCategory.Name = txtName.Text.Trim();

                if (txtDescription.Text != null && txtDescription.Text != "")
                    AssetCategory.Description = txtDescription.Text.Trim();

                if (new AssetsWorkBLL().IsAssetCategoryNameExists(AssetCategory))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (AssetCategory.ID == 0)
                {
                    AssetCategory.CreatedBy =  Convert.ToInt32(mdlUser.ID);
                    AssetCategory.CreatedDate = DateTime.Now;
                  
                }
                else
                {
                    AssetCategory.CreatedBy = Convert.ToInt32(CreatedBy);
                    AssetCategory.CreatedDate = Convert.ToDateTime(CreatedDate);
                    AssetCategory.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    AssetCategory.ModifiedDate = DateTime.Now;
                }

                if (ChkActive.Checked)
                AssetCategory.IsActive = true;
                else
                    AssetCategory.IsActive = false;

                bool IsSave = new AssetsWorkBLL().SaveAssetCategory(AssetCategory);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(CategoryID) == 0)
                        gvAssetCategory.PageIndex = 0;

                    gvAssetCategory.EditIndex = -1;
                    BindAssetCategory();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string AssetCategoryID = Convert.ToString(gvAssetCategory.DataKeys[e.RowIndex].Values[0]);

                if (!IsValidDelete(Convert.ToInt64(AssetCategoryID)))
                {
                    return;
                }

                bool IsDeleted = new AssetsWorkBLL().DeleteAssetCategory(Convert.ToInt64(AssetCategoryID));
                if (IsDeleted)
                {
                    BindAssetCategory();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAssetCategory.PageIndex = e.NewPageIndex;
                gvAssetCategory.EditIndex = -1;
                BindAssetCategory();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion "End GridView Events"
        private bool IsValidDelete(long _AssetCategoryID)
        {
            try
            {
                AssetsWorkBLL bblAssetsCat = new AssetsWorkBLL();

                bool IsExist = bblAssetsCat.IsAssetCategoryIDExists(_AssetCategoryID);
                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                    return false;
                }
                
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
            return true;
            
        }
    }
}