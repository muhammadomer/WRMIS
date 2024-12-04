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
    public partial class AssetsWorkType : BasePage
    {
        Dictionary<string, object> workTypeData = new Dictionary<string, object>();
        List<AM_AssetWorkType> lstWorkType = new List<AM_AssetWorkType>();

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
        protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstWorkType = new AssetsWorkBLL().GetAllAssetWorkType();
                    AM_AssetWorkType mdl = new AM_AssetWorkType();

                    mdl.ID = 0;
                    mdl.Name = "";
                    mdl.Description = "";
                    mdl.IsActive = true;

                    lstWorkType.Add(mdl);

                    gv.PageIndex = gv.PageCount;
                    gv.DataSource = lstWorkType;
                    gv.DataBind();

                    gv.EditIndex = gv.Rows.Count - 1;
                    gv.DataBind();
                    gv.Rows[gv.Rows.Count - 1].FindControl("txtName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gv.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                AssetsWorkBLL bllAsset = new AssetsWorkBLL();
                long id = Convert.ToInt32(((Label)gv.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (bllAsset.AssetWorkTypeAssiciatExists(id))
                {// check for closure work type associated with other modules 
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                workTypeData.Clear();
                workTypeData.Add("ID", id);

                if ((bool)bllAsset.AssetWorkType_Operations(Constants.AssetCRUD_DELETE, workTypeData))
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
        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                AssetsWorkBLL bllAsset = new AssetsWorkBLL();
                GridViewRow row = gv.Rows[e.RowIndex];
                long id = Convert.ToInt32(((Label)row.Cells[0].FindControl("lblID")).Text);
                string name = ((TextBox)row.Cells[1].FindControl("txtName")).Text.Trim();
                bool isActive = ((CheckBox)row.FindControl("cb_Active")).Checked;

                workTypeData.Clear();
                workTypeData.Add("Name", name);

                AM_AssetWorkType mdlWork = (AM_AssetWorkType)bllAsset.AssetWorkType_Operations(Constants.AssetCRUD_READ, workTypeData);

                if (mdlWork != null && id != mdlWork.ID)
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                string desc = ((TextBox)row.Cells[2].FindControl("txtDesc")).Text.Trim();
                workTypeData.Clear();
                workTypeData.Add("ID", id);
                workTypeData.Add("Name", name);
                workTypeData.Add("Description", desc);
                workTypeData.Add("UserID", SessionManagerFacade.UserInformation.ID);
                workTypeData.Add("IsActive", isActive);

                bool status = false;
                if (id == 0)
                {
                    status = (bool)bllAsset.AssetWorkType_Operations(Constants.AssetCRUD_CREATE, workTypeData);
                }
                else
                {
                    status = (bool)bllAsset.AssetWorkType_Operations(Constants.AssetCRUD_UPDATE, workTypeData);
                }

                if (status)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (id == 0)
                    {
                        gv.PageIndex = 0;
                    }
                    gv.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gv.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv.EditIndex = e.NewEditIndex;
                BindGrid();
                gv.Rows[e.NewEditIndex].FindControl("txtName").Focus();

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gv.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Get the current ROw and c its and edit able record or not 
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Button btnAdd = (Button)e.Row.FindControl("btnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Enabled = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Enabled = (bool)mdlRoleRights.BEdit;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Enabled = (bool)mdlRoleRights.BDelete;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        Button btnDelete = (Button)e.Row.FindControl("btnDelete");
            //        if (btnDelete != null)
            //            btnDelete.Visible = false;
            //    }
            //}
            //catch (Exception exp)
            //{
            //    new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            //}
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindGrid()
        {
            lstWorkType = new AssetsWorkBLL().GetAllAssetWorkType();
            gv.DataSource = lstWorkType;
            gv.DataBind();
        }
    }
}