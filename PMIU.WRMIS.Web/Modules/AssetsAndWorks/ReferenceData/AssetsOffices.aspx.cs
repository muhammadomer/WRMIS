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
    public partial class AssetsOffices : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindAssetOffices();
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
        private void BindAssetOffices()
        {
            try
            {
                List<object> lstAssetOffice = new AssetsWorkBLL().GetAssetAllOfficeList();
                gvAssetOffice.DataSource = lstAssetOffice;
                gvAssetOffice.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetOffice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "AddOffice")
                {
                    List<object> lstAddOffice = new AssetsWorkBLL().GetAssetAllOfficeList();

                    lstAddOffice.Add(
                    new
                    {
                        ID = 0,
                        OfficeName = string.Empty,
                        Description = string.Empty,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = string.Empty
                    });

                    gvAssetOffice.PageIndex = gvAssetOffice.PageCount;
                    gvAssetOffice.DataSource = lstAddOffice;
                    gvAssetOffice.DataBind();

                    gvAssetOffice.EditIndex = gvAssetOffice.Rows.Count - 1;
                    gvAssetOffice.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetOffice_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gvAssetOffice.DataKeys[e.Row.RowIndex];
                    int ID = Convert.ToInt32(key.Values["ID"]);
                    string name = Convert.ToString(key.Values["OfficeName"]);
                    string description = Convert.ToString(key.Values["Description"]);
                    #endregion

                    #region "Controls"
                    TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                    TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");
                    #endregion

                    if (gvAssetOffice.EditIndex == e.Row.RowIndex)
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
        protected void gvAssetOffice_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAssetOffice.EditIndex = e.NewEditIndex;
                BindAssetOffices();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetOffice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAssetOffice.EditIndex = -1;
                BindAssetOffices();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetOffice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvAssetOffice.Rows[e.RowIndex];

                #region "Controls"
                TextBox txtName = (TextBox)row.FindControl("txtName");
                TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                CheckBox ChkActive = ((CheckBox)row.FindControl("ChkActive"));
                #endregion

                #region "Datakeys"
                DataKey key = gvAssetOffice.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                string OfficeID = Convert.ToString(gvAssetOffice.DataKeys[e.RowIndex].Values[0]);

                AM_Offices AssetOffice = new AM_Offices();

                AssetOffice.ID = Convert.ToInt32(OfficeID);

                if (txtName.Text != "")
                    AssetOffice.OfficeName = txtName.Text.Trim();

                if (txtDescription.Text != null && txtDescription.Text != "")
                    AssetOffice.Description = txtDescription.Text.Trim();

                if (new AssetsWorkBLL().IsAssetOfficeNameExists(AssetOffice))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (AssetOffice.ID == 0)
                {
                    AssetOffice.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    AssetOffice.CreatedDate = DateTime.Now;

                }
                else
                {
                    AssetOffice.CreatedBy = Convert.ToInt32(CreatedBy);
                    AssetOffice.CreatedDate = Convert.ToDateTime(CreatedDate);
                    //AssetCategory.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    //AssetCategory.ModifiedDate = DateTime.Now;
                }
                if (ChkActive.Checked)
                    AssetOffice.IsActive = true;
                else
                    AssetOffice.IsActive = false;

                bool IsSave = new AssetsWorkBLL().SaveAssetOffice(AssetOffice);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(OfficeID) == 0)
                        gvAssetOffice.PageIndex = 0;

                    gvAssetOffice.EditIndex = -1;
                    BindAssetOffices();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetOffice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string AssetofficeID = Convert.ToString(gvAssetOffice.DataKeys[e.RowIndex].Values[0]);

                if (!IsValidDelete(Convert.ToInt64(AssetofficeID)))
                {
                    return;
                }

                bool IsDeleted = new AssetsWorkBLL().DeleteAssetOffice(Convert.ToInt64(AssetofficeID));
                if (IsDeleted)
                {
                    BindAssetOffices();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetOffice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAssetOffice.PageIndex = e.NewPageIndex;
                gvAssetOffice.EditIndex = -1;
                BindAssetOffices();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        private bool IsValidDelete(long _AssetofficeID)
        {
            try
            {
                AssetsWorkBLL bblAssetsCat = new AssetsWorkBLL();

                bool IsExist = bblAssetsCat.IsAssetOfficeIDExists(_AssetofficeID);
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

        #endregion "End GridView Events"
    }
}