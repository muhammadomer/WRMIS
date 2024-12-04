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
using PMIU.WRMIS.Web.Common.Controls;
namespace PMIU.WRMIS.Web.Modules.AssetsAndWorks.ReferenceData
{
    public partial class AssetsAttributes : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int SubCategID = 0;
                if (!IsPostBack)
                {
                    SetPageTitle();
                    SubCategID = Utility.GetNumericValueFromQueryString("SubCatgID", 0);
                    hdnSubCategID.Value = Convert.ToString(SubCategID);
                    hdnSubCategName.Value = Utility.GetStringValueFromQueryString("SubName", "");
                    Header_Show(Convert.ToInt64(hdnSubCategID.Value));

                    if (Convert.ToString(hdnSubCategName.Value) == Convert.ToString(Constants.AssetCategory.Infrastructure) && 
                        (Convert.ToString(hdnSubCategName.Value) == Convert.ToString(Constants.AssetAttribute.Channel)
                        || Convert.ToString(hdnSubCategName.Value) == Convert.ToString(Constants.AssetAttribute.Outlet) || Convert.ToString(hdnSubCategName.Value) == Convert.ToString(Constants.AssetAttribute.BarrageHeadwork)
                        || Convert.ToString(hdnSubCategName.Value) == Convert.ToString(Constants.AssetAttribute.ProtectionInfrastructure) || Convert.ToString(hdnSubCategName.Value) == Convert.ToString(Constants.AssetAttribute.Drain)
                        || Convert.ToString(hdnSubCategName.Value) == Convert.ToString(Constants.AssetAttribute.SmallDams) || Convert.ToString(hdnSubCategName.Value) == Convert.ToString(Constants.AssetAttribute.SmallDamsChannels)))
                    {
                        gvAssetAttribute.Visible = false;
                        Master.ShowMessage("For the attributes of this Sub-Category, Please refer to Irrigation Network.", SiteMaster.MessageType.Error);
                        return;
                    }
                    else
                    {
                        BindAssetSubCategory(Convert.ToInt64(hdnSubCategID.Value));
                        gvAssetAttribute.Visible = true;
                    }
                    hlBack.NavigateUrl = string.Format("~/Modules/AssetsAndWorks/ReferenceData/AssetsSubCategory.aspx?RestoreState=1");
                    
                
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
        //#region DropDownList
        //private void BindCategory()
        //{
        //    try
        //    {
        //        Dropdownlist.DDLAssetCategory(ddlCategory, false, (int)Constants.DropDownFirstOption.Select);
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        //private void BindSubCategory()
        //{
        //    try
        //    {
        //        Dropdownlist.DDLAssetSubCategory(ddlSubCategory, Convert.ToInt64(ddlCategory.SelectedValue), false, (int)Constants.DropDownFirstOption.Select);
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        //#endregion
        //#region DropDownList SelectedIndex
        //protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BindSubCategory();
        //        gvAssetAttribute.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        //protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (ddlCategory.SelectedItem.Text == Convert.ToString(Constants.AssetCategory.Infrastructure) && (ddlSubCategory.SelectedItem.Text == Convert.ToString(Constants.AssetAttribute.Channel)
        //            || ddlSubCategory.SelectedItem.Text == Convert.ToString(Constants.AssetAttribute.Outlet) || ddlSubCategory.SelectedItem.Text == Convert.ToString(Constants.AssetAttribute.BarrageHeadwork)
        //            || ddlSubCategory.SelectedItem.Text == Convert.ToString(Constants.AssetAttribute.ProtectionInfrastructure) || ddlSubCategory.SelectedItem.Text == Convert.ToString(Constants.AssetAttribute.Drain)
        //            || ddlSubCategory.SelectedItem.Text == Convert.ToString(Constants.AssetAttribute.SmallDams) || ddlSubCategory.SelectedItem.Text == Convert.ToString(Constants.AssetAttribute.SmallDamsChannels)))
        //        {
        //            Master.ShowMessage("For the attributes of this Sub-Category, Please refer to Irrigation Network.", SiteMaster.MessageType.Error);
        //            return;
        //        }
        //        else
        //        {
        //            BindAssetSubCategory();
        //            gvAssetAttribute.Visible = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
        //    }
        //}
        //#endregion

        private void Header_Show(long _SubCatid)
        {
            try
            {

                object lstHeader = new AssetsWorkBLL().GetCategorySubCategoryByID(_SubCatid);
                if (lstHeader != null)
                {

                    lblCategory.Text = Convert.ToString(lstHeader.GetType().GetProperty("CatName").GetValue(lstHeader));
                    lblSubcategory.Text = Convert.ToString(lstHeader.GetType().GetProperty("SubCatName").GetValue(lstHeader));

                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #region GridView Events
        private void BindAssetSubCategory(long SubcatgID)
        {
            try
            {
                List<object> lstAssetCategory = new AssetsWorkBLL().GetAssetAllAttributeList(Convert.ToInt64(SubcatgID));
                gvAssetAttribute.DataSource = lstAssetCategory;
                gvAssetAttribute.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetAttribute_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "AddAtrribute")
                {
                    List<object> lstAddAttribute = new AssetsWorkBLL().GetAssetAllAttributeList(Convert.ToInt64(hdnSubCategID.Value));

                    lstAddAttribute.Add(
                    new
                    {
                        ID = 0,
                        SubCategoryID = 0,
                        AttributeName = string.Empty,
                        AttributeDataType = string.Empty,
                        Description = string.Empty,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = string.Empty
                    });

                    gvAssetAttribute.PageIndex = gvAssetAttribute.PageCount;
                    gvAssetAttribute.DataSource = lstAddAttribute;
                    gvAssetAttribute.DataBind();

                    gvAssetAttribute.EditIndex = gvAssetAttribute.Rows.Count - 1;
                    gvAssetAttribute.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetAttribute_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    #region "Data Keys"
                    DataKey key = gvAssetAttribute.DataKeys[e.Row.RowIndex];
                    int ID = Convert.ToInt32(key.Values["ID"]);
                    string name = Convert.ToString(key.Values["AttributeName"]);
                    string description = Convert.ToString(key.Values["Description"]);
                    string AttributeType = Convert.ToString(key.Values["AttributeDataType"]);
                    #endregion

                    #region "Controls"
                    TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                    TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");
                    DropDownList ddlAttributeType = (DropDownList)e.Row.FindControl("ddlAttributeType");

                    #endregion

                    if (gvAssetAttribute.EditIndex == e.Row.RowIndex)
                    {
                        if (ddlAttributeType != null)
                        {
                            Dropdownlist.DDLAttributeType(ddlAttributeType, (int)Constants.DropDownFirstOption.Select);
                            if (!string.IsNullOrEmpty(AttributeType))
                                Dropdownlist.SetSelectedText(ddlAttributeType, AttributeType);
                        }

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
        protected void gvAssetAttribute_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAssetAttribute.EditIndex = e.NewEditIndex;
                BindAssetSubCategory(Convert.ToInt64(hdnSubCategID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetAttribute_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAssetAttribute.EditIndex = -1;
                BindAssetSubCategory(Convert.ToInt64(hdnSubCategID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetAttribute_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvAssetAttribute.Rows[e.RowIndex];

                #region "Controls"
                TextBox txtName = (TextBox)row.FindControl("txtName");
                TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                DropDownList ddlAttributeType = (DropDownList)row.FindControl("ddlAttributeType");
                CheckBox ChkActive = ((CheckBox)row.FindControl("ChkActive"));
                #endregion

                #region "Datakeys"
                DataKey key = gvAssetAttribute.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                string AttributeID = Convert.ToString(gvAssetAttribute.DataKeys[e.RowIndex].Values[0]);

                AM_Attributes AssetAttributes = new AM_Attributes();

                AssetAttributes.ID = Convert.ToInt32(AttributeID);

                if (txtName.Text != "")
                    AssetAttributes.AttributeName = txtName.Text.Trim();

                AssetAttributes.AttributeDataType = ddlAttributeType.SelectedItem.Text;
                AssetAttributes.SubCategoryID = Convert.ToInt64(hdnSubCategID.Value);

                if (txtDescription.Text != null && txtDescription.Text != "")
                    AssetAttributes.Description = txtDescription.Text.Trim();

                if (new AssetsWorkBLL().IsAssetAttributeNameExists(AssetAttributes))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (AssetAttributes.ID == 0)
                {
                    AssetAttributes.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    AssetAttributes.CreatedDate = DateTime.Now;

                }
                else
                {
                    AssetAttributes.CreatedBy = Convert.ToInt32(CreatedBy);
                    AssetAttributes.CreatedDate = Convert.ToDateTime(CreatedDate);
                    AssetAttributes.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    AssetAttributes.ModifiedDate = DateTime.Now;
                }

                if (ChkActive.Checked)
                    AssetAttributes.IsActive = true;
                else
                    AssetAttributes.IsActive = false;

                bool IsSave = new AssetsWorkBLL().SaveAssetAttribute(AssetAttributes);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(AttributeID) == 0)
                        gvAssetAttribute.PageIndex = 0;

                    gvAssetAttribute.EditIndex = -1;
                    BindAssetSubCategory(Convert.ToInt64(hdnSubCategID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetAttribute_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string AssetAttributeID = Convert.ToString(gvAssetAttribute.DataKeys[e.RowIndex].Values[0]);

                if (!IsValidDelete(Convert.ToInt64(AssetAttributeID)))
                {
                    return;
                }

                bool IsDeleted = new AssetsWorkBLL().DeleteAssetAttribute(Convert.ToInt64(AssetAttributeID));
                if (IsDeleted)
                {
                    BindAssetSubCategory(Convert.ToInt64(hdnSubCategID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAssetAttribute_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAssetAttribute.PageIndex = e.NewPageIndex;
                gvAssetAttribute.EditIndex = -1;
                BindAssetSubCategory(Convert.ToInt64(hdnSubCategID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }


        private bool IsValidDelete(long _AssetAttributeID)
        {
            try
            {
                AssetsWorkBLL bblAssetsAttribute = new AssetsWorkBLL();

                bool IsExist = bblAssetsAttribute.IsAssetAttributeIDExists(_AssetAttributeID);
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