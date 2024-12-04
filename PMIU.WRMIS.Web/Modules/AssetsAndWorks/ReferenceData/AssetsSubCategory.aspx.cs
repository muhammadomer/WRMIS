using PMIU.WRMIS.BLL.AssetsAndWorks;
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
    public partial class AssetsSubCategory : BasePage
    {
        //Data Members 
        Dictionary<string, object> dd = new Dictionary<string, object>();
        List<object> lstSubCategory = new List<object>();
        AssetsWorkBLL balAW = new AssetsWorkBLL();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                
                    SetPageTitle();
                    bindCategories();
                    if (!string.IsNullOrEmpty(Request.QueryString["RestoreState"]))
                    {
                        SetControlsValues();
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #region SearchCriteria
        protected void SetControlsValues()
        {
            try
            {
                object currentObj = Session["CurrentControlsValues"] as object;
                if (currentObj != null)
                {
                    if (Convert.ToString(currentObj.GetType().GetProperty("CategoryID").GetValue(currentObj)) != "")
                    {
                        ddlCategory.SelectedValue = Convert.ToString(currentObj.GetType().GetProperty("CategoryID").GetValue(currentObj));
                    }
                    int CategoryID = Convert.ToInt32(ddlCategory.SelectedItem.Value == string.Empty ? "0" : ddlCategory.SelectedItem.Value);
                    if (CategoryID > 0)
                    {
                        lstSubCategory = balAW.GetAllSubCategoriesByCategoryID(CategoryID);
                        Session["SubCategorirs"] = lstSubCategory;
                        BindGrid(lstSubCategory);
                    }
                    else
                    {
                        lstSubCategory = new List<object>();
                        BindGrid(lstSubCategory);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void SaveSearchCriteriaInSession()
        {
            try
            {
                Session["CurrentControlsValues"] = null;
                object obj = new
                {
                    CategoryID = ddlCategory.SelectedItem.Value
                };
                Session["CurrentControlsValues"] = obj;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion SearchCriteria

        protected void bindCategories()
        {
            List<object> lst = balAW.GetAssetCategoryList();
            Dropdownlist.BindDropdownlist<List<object>>(ddlCategory, lst, (int)Constants.DropDownFirstOption.Select);
        }

        protected void gvAssetSubCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<object> lstSC = new List<object>();
                    lstSC = Session["SubCategorirs"] as List<object>;
                    int CategoryID = Convert.ToInt32(ddlCategory.SelectedItem.Value == string.Empty ? "0" : ddlCategory.SelectedItem.Value);
                    List<object> lstSubCat = balAW.GetAllSubCategoriesByCategoryID(CategoryID);
                    Session["SubCategorirs"] = lstSubCat;
                    BindGrid(lstSubCat);

                    lstSubCat.Add(new { ID = 0, Name = "", Description = "", FOBit = false, IsActive = true });
                    gvAssetSubCategory.PageIndex = gvAssetSubCategory.PageCount;
                    gvAssetSubCategory.DataSource = lstSubCat;
                    gvAssetSubCategory.DataBind();
                    gvAssetSubCategory.EditIndex = gvAssetSubCategory.Rows.Count - 1;
                    gvAssetSubCategory.DataBind();
                    gvAssetSubCategory.Rows[gvAssetSubCategory.Rows.Count - 1].FindControl("txtCategoryName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetSubCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAssetSubCategory.PageIndex = e.NewPageIndex;
                ddlCategory_SelectedIndexChanged(null, null);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetSubCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                long id = Convert.ToInt32(((Label)gvAssetSubCategory.Rows[e.RowIndex].FindControl("lblID")).Text);
                dd.Clear();
                dd.Add("ID", id);
                if ((bool)balAW.SubCategory_Operations(Constants.CRUD_DELETE, dd))
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    ddlCategory_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetSubCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                #region "Get Values for save"
                AssetsWorkBLL bllAW = new AssetsWorkBLL();
                GridViewRow row = gvAssetSubCategory.Rows[e.RowIndex];
                long id = Convert.ToInt32(((Label)row.Cells[0].FindControl("lblID")).Text);
                string name = ((TextBox)row.Cells[1].FindControl("txtCategoryName")).Text.Trim();
                string desc = ((TextBox)row.Cells[2].FindControl("txtDescription")).Text.Trim();
                CheckBox ChkActive = ((CheckBox)row.FindControl("ChkActive"));
                CheckBox ChkFOBit = ((CheckBox)row.FindControl("ChkFOBit"));
                long CategoryID = Convert.ToInt64(ddlCategory.SelectedItem.Value);
                #endregion
                dd.Clear();
                dd.Add("Name", name);
                #region "check  subCategory already exist or not"
                AM_SubCategory sc = new AM_SubCategory();
                sc.Name = name;
                sc.ID = id;
                sc.CategoryID = CategoryID;
                bool isExist = bllAW.IsAssetSubCategoryNameExists(sc);
                if (isExist)
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                #endregion
                dd.Add("ID", id);
                dd.Add("CategoryID", CategoryID);
                dd.Add("Description", desc);
                if (ChkActive.Checked)
                    dd.Add("IsActive", true);
                else
                    dd.Add("IsActive", false);

                if (ChkFOBit.Checked)
                    dd.Add("FOBit", true);
                else
                    dd.Add("FOBit", false);

                dd.Add("UserID", SessionManagerFacade.UserInformation.ID);
                //dd.Add("IsActive", isActive);

                bool status = false;
                if (id == 0)
                {
                    status = (bool)bllAW.SubCategory_Operations(Constants.CRUD_CREATE, dd);
                }
                else
                {
                    status = (bool)bllAW.SubCategory_Operations(Constants.CRUD_UPDATE, dd);
                }
                if (status)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (id == 0)
                    {
                        gvAssetSubCategory.PageIndex = 0;
                    }
                    gvAssetSubCategory.EditIndex = -1;
                    ddlCategory_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetSubCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAssetSubCategory.EditIndex = -1;
                ddlCategory_SelectedIndexChanged(null, null);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetSubCategory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAssetSubCategory.EditIndex = e.NewEditIndex;
                ddlCategory_SelectedIndexChanged(null, null);
                gvAssetSubCategory.Rows[e.NewEditIndex].FindControl("txtCategoryName").Focus();

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetSubCategory_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvAssetSubCategory.EditIndex = -1;
                ddlCategory_SelectedIndexChanged(null, null);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetSubCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblID = (Label)e.Row.FindControl("lblID");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    HyperLink hlAssetsAttribute = (HyperLink)e.Row.FindControl("hlAssetsAttribute");
                    Label lblCategoryName = (Label)e.Row.FindControl("lblCategoryName");
                    Label lblActive = (Label)e.Row.FindControl("lblActive");
                    CheckBox ChkFOBit = (CheckBox)e.Row.FindControl("ChkFOBit");
                    if (gvAssetSubCategory.EditIndex == e.Row.RowIndex)
                    {
                        bool IsExist = balAW.SubCategoryIDExistsAsset(Convert.ToInt64(lblID.Text));
                        if (IsExist)
                        {
                            ChkFOBit.Enabled = false;
                        }
                    }
                    if (lblActive.Text == "False")
                    {
                        hlAssetsAttribute.Enabled = false;
                    }
                    if (isRowHaveBtnz(lblCategoryName.Text))
                    {
                        btnDelete.Visible = false;
                        btnEdit.Visible = false;
                        hlAssetsAttribute.Visible = false;
                    }
                    else
                    {
                        btnDelete.Visible = base.CanDelete;
                        btnEdit.Visible = base.CanEdit;
                        hlAssetsAttribute.Visible = true;
                    }


                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private bool isRowHaveBtnz(string CategoryName)
        {
            if (CategoryName.ToLower().Trim() == "channels" || CategoryName.ToLower().Trim() == "outlets" || CategoryName.ToLower().Trim() == "barrages/headworks" || CategoryName.ToLower().Trim() == "protection infrastructures" || CategoryName.ToLower().Trim() == "drains" || CategoryName.ToLower().Trim() == "small dams" || CategoryName.ToLower().Trim() == "small dams channels")
            {
                return true;
            }
            return false;
        }
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.AssetsCategory);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
        private void BindGrid(List<object> lstOfSubCategory)
        {
            //lstWorkType = new ClosureOperationsBLL().GetAllClosureWorkType();

            //object ob = new object();
            //ob = new { ID = 1, AssetCategory = "Infrastructure", Description = "Infrastructure of Irrigation" };
            //List<object> lstObj = new List<object>();
            //lstObj.Add(ob);
            gvAssetSubCategory.DataSource = lstOfSubCategory;
            gvAssetSubCategory.DataBind();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveSearchCriteriaInSession();
            int CategoryID = Convert.ToInt32(ddlCategory.SelectedItem.Value == string.Empty ? "0" : ddlCategory.SelectedItem.Value);
            if (CategoryID > 0)
            {
                lstSubCategory = balAW.GetAllSubCategoriesByCategoryID(CategoryID);
                Session["SubCategorirs"] = lstSubCategory;
                BindGrid(lstSubCategory);
            }
            else
            {
                lstSubCategory = new List<object>();
                BindGrid(lstSubCategory);
            }

        }
    }
}

