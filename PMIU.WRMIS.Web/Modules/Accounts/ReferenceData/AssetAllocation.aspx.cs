using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.Accounts.Controls;
using System.Data;

namespace PMIU.WRMIS.Web.Modules.Accounts.ReferenceData
{
    public partial class AssetAllocation : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    long _ResourceAllocationID = Utility.GetNumericValueFromQueryString("ResourceAllocationID", 0);
                    hdnResourceAllocationID.Value = Convert.ToString(_ResourceAllocationID);
                    ResourceAllocationData._AssetID = _ResourceAllocationID;

                    //_UserID = SessionManagerFacade.UserAssociatedLocations.UserID;
                    //boundryLvlID = SessionManagerFacade.UserAssociatedLocations.IrrigationLevelID;

                    if (_ResourceAllocationID > 0)
                    {
                        //if (boundryLvlID == null)
                        //{
                        //    boundryLvlID = 0;
                        //}

                        BindAssetAllocationGrid();
                    }
                    hlBack.NavigateUrl = string.Format("~/Modules/Accounts/ReferenceData/ResourceAllocation.aspx?ShowHistory=true");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
        #region Events

        #region Grid
        protected void gvAssetAllocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAssetAllocation.PageIndex = e.NewPageIndex;
                gvAssetAllocation.EditIndex = -1;
                BindAssetAllocationGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvAssetAllocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                //GridViewRow row = gvAssetAllocation.Rows[e.RowIndex];
                gvAssetAllocation.EditIndex = -1;
                BindAssetAllocationGrid();

                //DropDownList ddlSubCategory = (DropDownList)row.FindControl("ddlSubCategory");
                //ddlSubCategory.Attributes.Remove("required");


            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssetAllocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "AddAsset")
                {
                    List<object> lstAssets = new ReferenceDataBLL().GetAssetAllocation(Convert.ToInt64(hdnResourceAllocationID.Value));
                    //ID,CategoryID,CategoryName,SubCategoryID,SubCategoryName,AssetTypeID,AssetTypeName,AssetItemID,AssetItemName,AssetAttributeID,AssetAttributeName,AssetAttributeValue,CreatedBy,CreatedDate"
                    lstAssets.Add(new
                    {
                        ID = 0,
                        CategoryID = 0,
                        CategoryName = string.Empty,
                        SubCategoryID = 0,
                        SubCategoryName = string.Empty,
                        AssetTypeID = 0,
                        AssetTypeName = string.Empty,
                        AssetItemID = 0,
                        AssetItemName = string.Empty,
                        AssetAttributeID = 0,
                        AssetAttributeName = string.Empty,
                        AssetAttributeValue = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now,
                        ResourceAllocationID = 0,
                        AssetType = string.Empty,
                        LotQuantity = 0,
                    });
                    gvAssetAllocation.PageIndex = gvAssetAllocation.PageCount;
                    gvAssetAllocation.DataSource = lstAssets;
                    gvAssetAllocation.DataBind();

                    gvAssetAllocation.EditIndex = gvAssetAllocation.Rows.Count - 1;
                    gvAssetAllocation.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssetAllocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);

                    if (gvAssetAllocation.EditIndex == e.Row.RowIndex)
                    {
                        //ID,CategoryID,CategoryName,SubCategoryID,SubCategoryName,AssetTypeID,AssetTypeName,AssetItemID,AssetItemName,AssetAttributeID,AssetAttributeName,AssetAttributeValue,CreatedBy,CreatedDate"
                        #region "Data Keys"
                        DataKey key = gvAssetAllocation.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string CategoryID = Convert.ToString(key.Values["CategoryID"]);
                        string SubCategoryID = Convert.ToString(key.Values["SubCategoryID"]);
                        string AssetTypeID = Convert.ToString(key.Values["AssetTypeID"]);
                        string AssetItemID = Convert.ToString(key.Values["AssetItemID"]);
                        string AssetAttributeID = Convert.ToString(key.Values["AssetAttributeID"]);
                        string AssetAttributeValue = Convert.ToString(key.Values["AssetAttributeValue"]);
                        string ResourceAllocationID = Convert.ToString(key.Values["ResourceAllocationID"]);
                        string AssetType = Convert.ToString(key.Values["AssetType"]);
                        string LotQuantity = Convert.ToString(key.Values["LotQuantity"]);
                        #endregion

                        #region Control
                        DropDownList ddlCategory = (DropDownList)e.Row.FindControl("ddlCategory");
                        DropDownList ddlSubCategory = (DropDownList)e.Row.FindControl("ddlSubCategory");
                        DropDownList ddlAssetType = (DropDownList)e.Row.FindControl("ddlAssetType");
                        DropDownList ddlAsset = (DropDownList)e.Row.FindControl("ddlAsset");
                        DropDownList ddlAssetAttribute = (DropDownList)e.Row.FindControl("ddlAssetAttribute");
                        Label lblAssetAttributeValue = (Label)e.Row.FindControl("lblAssetAttributeValue");
                        TextBox txtAssetAttributeValue = (TextBox)e.Row.FindControl("txtAssetAttributeValue");

                        Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");
                        #endregion


                        if (ddlCategory != null)
                        {
                            Dropdownlist.DDLAACategory(ddlCategory, false);
                            if (!string.IsNullOrEmpty(CategoryID))
                            {
                                Dropdownlist.SetSelectedValue(ddlCategory, CategoryID);
                            }

                        }

                        if (ddlSubCategory != null)
                        {
                            if (string.IsNullOrEmpty(ddlCategory.SelectedItem.Value))
                            {
                                Dropdownlist.DDLAASubCategory(ddlSubCategory, true);
                                ddlSubCategory.Enabled = false;
                            }
                            else
                            {
                                Dropdownlist.DDLAASubCategory(ddlSubCategory, false, Convert.ToInt64(ddlCategory.SelectedItem.Value));
                                Dropdownlist.SetSelectedValue(ddlSubCategory, SubCategoryID);
                                ddlSubCategory.Enabled = true;

                                ddlSubCategory.CssClass = "form-control required";
                                ddlSubCategory.Attributes.Add("required", "required");
                            }

                        }

                        if (ddlAssetType != null)
                        {
                            Dropdownlist.DDLAAAssetType(ddlAssetType, false);
                            if (!string.IsNullOrEmpty(AssetTypeID))
                                Dropdownlist.SetSelectedValue(ddlAssetType, AssetTypeID);
                        }


                        if (ddlAsset != null)
                        {
                            if (string.IsNullOrEmpty(ddlSubCategory.SelectedItem.Value))
                            {
                                Dropdownlist.DDLAAAssetName(ddlAsset, true);
                                ddlAsset.Enabled = false;
                            }

                            else
                            {
                                Dropdownlist.DDLAAAssetName(ddlAsset, false, Convert.ToInt64(ddlSubCategory.SelectedItem.Value));
                                Dropdownlist.SetSelectedValue(ddlAsset, AssetItemID);
                                ddlAsset.Enabled = true;

                                ddlAsset.CssClass = "form-control required";
                                ddlAsset.Attributes.Add("required", "required");
                            }

                        }


                        if (AssetType.ToLower() == "lot" && LotQuantity != null)
                        {

                            if (ID == "0")
                            {
                                txtAssetAttributeValue.Text = LotQuantity;
                                lblAssetAttributeValue.Text = LotQuantity;
                            }
                            else
                            {
                                txtAssetAttributeValue.Text = AssetAttributeValue;
                                lblAssetAttributeValue.Text = AssetAttributeValue;
                            }

                            lblAssetAttributeValue.Visible = false;
                            txtAssetAttributeValue.Visible = true;

                            lblQuantity.Visible = true;
                            ddlAssetAttribute.Visible = false;
                        }
                        else
                        {
                            if (ddlAssetAttribute != null)
                            {
                                if (string.IsNullOrEmpty(ddlSubCategory.SelectedItem.Value))
                                {
                                    Dropdownlist.DDLAAAssetAttribute(ddlAssetAttribute, true);
                                    ddlAssetAttribute.Enabled = false;
                                }
                                else
                                {
                                    if (ddlAsset.SelectedItem.Value != "")
                                    {
                                        Dropdownlist.DDLAAAssetAttribute(ddlAssetAttribute, false, Convert.ToInt64(ddlAsset.SelectedItem.Value));
                                        Dropdownlist.SetSelectedValue(ddlAssetAttribute, AssetAttributeID);
                                        ddlAssetAttribute.Enabled = true;

                                        //ddlAssetAttribute.CssClass = "form-control required";
                                        //ddlAssetAttribute.Attributes.Add("required", "required");
                                    }
                                    else
                                    {
                                        ddlAssetAttribute.Enabled = false;
                                        ddlAssetAttribute.Items.Insert(0, new ListItem("Select", ""));
                                        //Dropdownlist.SetSelectedValue(ddlAssetAttribute, "");
                                    }
                                }
                            }
                            if (lblAssetAttributeValue != null)
                            {
                                lblAssetAttributeValue.Visible = true;
                                txtAssetAttributeValue.Visible = false;
                                lblAssetAttributeValue.Text = AssetAttributeValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssetAllocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvAssetAllocation.DataKeys[e.RowIndex].Values["ID"]);

                bool IsAssetAllocationIDExist = new ReferenceDataBLL().IsAssetAllocationIDExists(Convert.ToInt64(ID));
                if (IsAssetAllocationIDExist)
                {
                    Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = new ReferenceDataBLL().DeleteAssetsAllocation(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindAssetAllocationGrid();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssetAllocation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAssetAllocation.EditIndex = e.NewEditIndex;
                BindAssetAllocationGrid();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssetAllocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                #region "Data Keys"
                //ID,AssetTypeID,AssetItemID,AssetAttributeID,AssetAttributeValue,CreatedBy,CreatedDate"
                DataKey key = gvAssetAllocation.DataKeys[e.RowIndex];

                string ID = Convert.ToString(key.Values["ID"]);
                string AssetTypeID = Convert.ToString(key.Values["AssetTypeID"]);
                string AssetItemID = Convert.ToString(key.Values["AssetItemID"]);
                string AssetAttributeID = Convert.ToString(key.Values["AssetAttributeID"]);
                string AssetAttributeValue = Convert.ToString(key.Values["AssetAttributeValue"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                string ResourceAllocationID = Convert.ToString(key.Values["ResourceAllocationID"]);
                string AssetType = Convert.ToString(key.Values["AssetType"]);
                string LotQuantity = Convert.ToString(key.Values["LotQuantity"]);
                #endregion

                #region Control
                GridViewRow row = gvAssetAllocation.Rows[e.RowIndex];
                DropDownList ddlAssetType = (DropDownList)row.FindControl("ddlAssetType");
                DropDownList ddlAsset = (DropDownList)row.FindControl("ddlAsset");
                DropDownList ddlAssetAttribute = (DropDownList)row.FindControl("ddlAssetAttribute");
                Label lblAssetAttributeValue = (Label)row.FindControl("lblAssetAttributeValue");
                TextBox txtAssetAttributeValue = (TextBox)row.FindControl("txtAssetAttributeValue");
                Label lblLotValue = (Label)row.FindControl("lblLotValue");
                #endregion
                AT_AssetAllocation Assets = new AT_AssetAllocation();

                Assets.ID = Convert.ToInt64(ID);
                if (ID == "0")
                {
                    Assets.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    Assets.CreatedDate = DateTime.Now;
                    Assets.ModifiedBy = null;
                    Assets.ModifiedDate = null;
                    LotQuantity = lblAssetAttributeValue.Text;
                }
                else
                {
                    Assets.CreatedBy = Convert.ToInt32(CreatedBy);
                    Assets.CreatedDate = Convert.ToDateTime(CreatedDate);
                    Assets.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    Assets.ModifiedDate = DateTime.Now;
                }

                if (AssetTypeID == ddlAssetType.SelectedItem.Value)
                    Assets.AssetTypeID = Convert.ToInt64(AssetTypeID);
                else
                {
                    Assets.AssetTypeID = Convert.ToInt64(ddlAssetType.SelectedItem.Value);
                }

                if (AssetItemID == ddlAsset.SelectedItem.Value)
                    Assets.AssetItemID = Convert.ToInt64(AssetItemID);
                else
                {
                    Assets.AssetItemID = Convert.ToInt64(ddlAsset.SelectedItem.Value);
                }

                string AssetItemType = new ReferenceDataBLL().GetAssetItemType(Assets.AssetItemID);
                if (AssetItemType.ToLower() != "lot")
                {

                    //if (AssetAttributeID != "0")
                    //{
                    if (!string.IsNullOrEmpty(ddlAssetAttribute.SelectedItem.Value))
                        Assets.AssetAttributeID = Convert.ToInt64(ddlAssetAttribute.SelectedItem.Value);
                    //}
                    //else
                    //{
                    //    long? AssetAttID = new ReferenceDataBLL().GetAssetAttributeID(Convert.ToInt64(ddlAssetAttribute.SelectedItem.Value), Assets.AssetItemID);
                    //    Assets.AssetAttributeID = AssetAttID;
                    //}
                }
                else
                {
                    if (AssetAttributeID != "0")
                    {
                        Assets.AssetAttributeID = null;
                    }

                    if (!string.IsNullOrEmpty(txtAssetAttributeValue.Text))
                    {
                        long LotValue = lblLotValue.Text == string.Empty ? Convert.ToInt64(LotQuantity) : Convert.ToInt64(lblLotValue.Text);

                        if (Convert.ToInt32(txtAssetAttributeValue.Text) < 0 || Convert.ToInt32(txtAssetAttributeValue.Text) > Convert.ToInt32(LotValue))
                        {
                            Master.ShowMessage(Message.TheAllocatedQuantityShouldBeLessThanOrEqualToTheTotalQuantityOfTheAsset.Description, SiteMaster.MessageType.Error);
                            return;
                        }
                        else
                        {
                            Assets.AssetAllocationValue = Convert.ToInt32(txtAssetAttributeValue.Text);
                        }
                    }
                }
                Assets.ResourceAllocationID = Convert.ToInt64(hdnResourceAllocationID.Value);



                if (new ReferenceDataBLL().IsAssetItemUnique(Assets))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }


                if (new ReferenceDataBLL().IsAssetIDExists(Convert.ToInt64(ddlAsset.SelectedItem.Value)) && ID == "0")
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }



                if (txtAssetAttributeValue.Text != "")
                {

                    string s = txtAssetAttributeValue.Text;
                    int result;
                    if (int.TryParse(s, out result))
                    {
                        if (Convert.ToInt64(txtAssetAttributeValue.Text) <= 0)
                        {
                            Master.ShowMessage("Attribute Value Should be Greater than 0", SiteMaster.MessageType.Error);
                            return;
                        }
                    }
                    else
                    {

                    }
                }



                bool IsSave = new ReferenceDataBLL().SaveAssetsAllocation(Assets);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(Assets.ID) == 0)
                        gvAssetAllocation.PageIndex = 0;

                    gvAssetAllocation.EditIndex = -1;
                    BindAssetAllocationGrid();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region DropDown
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlCategory = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlCategory.NamingContainer;

                DropDownList ddlSubCategory = (DropDownList)gvRow.FindControl("ddlSubCategory");
                DropDownList ddlAsset = (DropDownList)gvRow.FindControl("ddlAsset");
                DropDownList ddlAssetAttribute = (DropDownList)gvRow.FindControl("ddlAssetAttribute");

                Label lblAssetAttributeValue = (Label)gvRow.FindControl("lblAssetAttributeValue");
                Label lblQuantity = (Label)gvRow.FindControl("lblQuantity");
                TextBox txtAssetAttributeValue = (TextBox)gvRow.FindControl("txtAssetAttributeValue");

                //populate empty dropdown
                Dropdownlist.DDLAASubCategory(ddlSubCategory, true);
                Dropdownlist.DDLAAAssetName(ddlAsset, true);
                Dropdownlist.DDLAAAssetAttribute(ddlAssetAttribute, true);
                ddlSubCategory.Enabled = false;
                ddlAsset.Enabled = false;
                ddlAssetAttribute.Enabled = false;

                if (gvRow != null)
                {
                    if (ddlCategory.SelectedItem.Value != "")
                    {
                        Dropdownlist.DDLAASubCategory(ddlSubCategory, false, Convert.ToInt64(ddlCategory.SelectedItem.Value));
                        ddlSubCategory.Enabled = true;

                        ddlSubCategory.CssClass = "form-control required";
                        ddlSubCategory.Attributes.Add("required", "required");

                        lblAssetAttributeValue.Visible = false;
                        txtAssetAttributeValue.Visible = false;
                        lblQuantity.Visible = false;

                        ddlAssetAttribute.SelectedItem.Value = "";
                        ddlAssetAttribute.Enabled = false;
                        ddlAssetAttribute.Visible = true;
                        //ddlAssetAttribute.CssClass = "form-control";
                        //ddlAssetAttribute.Attributes.Remove("required");

                        ddlAsset.SelectedItem.Value = "";
                        ddlAsset.CssClass = "form-control";
                        ddlAsset.Attributes.Remove("required");

                        ddlAssetAttribute.ClearSelection();
                    }
                    else
                    {
                        lblAssetAttributeValue.Visible = false;
                        txtAssetAttributeValue.Visible = false;
                        lblQuantity.Visible = false;

                        ddlAssetAttribute.SelectedItem.Value = "";
                        ddlAssetAttribute.Enabled = false;
                        ddlAssetAttribute.Visible = true;
                        //ddlAssetAttribute.CssClass = "form-control";
                        //ddlAssetAttribute.Attributes.Remove("required");

                        ddlAsset.SelectedItem.Value = "";
                        ddlAsset.CssClass = "form-control";
                        ddlAsset.Attributes.Remove("required");

                        ddlSubCategory.SelectedItem.Value = "";
                        ddlSubCategory.CssClass = "form-control";
                        ddlSubCategory.Attributes.Remove("required");

                        ddlAssetAttribute.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSubCategory = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlSubCategory.NamingContainer;

                DropDownList ddlAsset = (DropDownList)gvRow.FindControl("ddlAsset");
                DropDownList ddlAssetAttribute = (DropDownList)gvRow.FindControl("ddlAssetAttribute");

                Label lblAssetAttributeValue = (Label)gvRow.FindControl("lblAssetAttributeValue");
                Label lblQuantity = (Label)gvRow.FindControl("lblQuantity");
                TextBox txtAssetAttributeValue = (TextBox)gvRow.FindControl("txtAssetAttributeValue");

                //populate empty dropdown
                Dropdownlist.DDLAAAssetName(ddlAsset, true);
                //Dropdownlist.DDLAAAssetAttribute(ddlAssetAttribute, true);
                ddlAsset.Enabled = false;
                ddlAssetAttribute.Enabled = false;

                if (gvRow != null)
                {
                    if (ddlSubCategory.SelectedItem.Value != "")
                    {
                        Dropdownlist.DDLAAAssetName(ddlAsset, false, Convert.ToInt64(ddlSubCategory.SelectedItem.Value));
                        //Dropdownlist.DDLAAAssetAttribute(ddlAssetAttribute, false, Convert.ToInt64(ddlSubCategory.SelectedItem.Value));
                        ddlAsset.Enabled = true;
                        ddlAsset.CssClass = "form-control required";
                        ddlAsset.Attributes.Add("required", "required");

                        ddlAssetAttribute.CssClass = "form-control";
                        ddlAssetAttribute.Attributes.Remove("required");
                        ddlAssetAttribute.ClearSelection();

                        lblAssetAttributeValue.Visible = false;
                        txtAssetAttributeValue.Visible = false;
                        lblQuantity.Visible = false;
                        //ddlAssetAttribute.Enabled = true;
                    }
                    else
                    {
                        lblAssetAttributeValue.Visible = false;
                        txtAssetAttributeValue.Visible = false;
                        lblQuantity.Visible = false;

                        ddlAssetAttribute.SelectedItem.Value = "";
                        ddlAssetAttribute.Enabled = false;
                        ddlAssetAttribute.Visible = true;
                        //ddlAssetAttribute.CssClass = "form-control";
                        //ddlAssetAttribute.Attributes.Remove("required");

                        ddlAsset.SelectedItem.Value = "";
                        ddlAsset.CssClass = "form-control";
                        ddlAsset.Attributes.Remove("required");

                        ddlAssetAttribute.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        protected void ddlAsset_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlAsset = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlAsset.NamingContainer;

                DropDownList ddlSubCategory = (DropDownList)gvRow.FindControl("ddlSubCategory");
                DropDownList ddlAssetAttribute = (DropDownList)gvRow.FindControl("ddlAssetAttribute");

                //populate empty dropdown
                Dropdownlist.DDLAAAssetAttribute(ddlAssetAttribute, true);
                ddlAssetAttribute.Enabled = false;
                Label lblAssetAttributeValue = (Label)gvRow.FindControl("lblAssetAttributeValue");
                Label lblQuantity = (Label)gvRow.FindControl("lblQuantity");
                TextBox txtAssetAttributeValue = (TextBox)gvRow.FindControl("txtAssetAttributeValue");
                Label lblLotValue = (Label)gvRow.FindControl("lblLotValue");

                if (gvRow != null)
                {
                    if (ddlAsset.SelectedItem.Value != "")
                    {
                        AM_AssetItems Items = new ReferenceDataBLL().GetAssetLotQuantity(Convert.ToInt64(ddlAsset.SelectedItem.Value));
                        lblLotValue.Text = Items.LotQuantity.ToString();
                        if (Items.AssetType.ToLower() == "lot")
                        {
                            Dropdownlist.DDLAAAssetAttribute(ddlAssetAttribute, false, -2);

                            lblAssetAttributeValue.Text = Convert.ToString(Items.LotQuantity);
                            lblAssetAttributeValue.Visible = false;

                            txtAssetAttributeValue.Visible = true;
                            txtAssetAttributeValue.Text = Convert.ToString(Items.LotQuantity);

                            lblQuantity.Visible = true;
                            ddlAssetAttribute.Visible = false;
                        }
                        else
                        {
                            int ss = ddlAsset.SelectedIndex;
                            string aa = ddlAsset.SelectedItem.Value;
                            Dropdownlist.DDLAAAssetAttribute(ddlAssetAttribute, false, Convert.ToInt64(ddlAsset.SelectedItem.Value));
                            ddlAssetAttribute.Enabled = true;
                            txtAssetAttributeValue.Visible = false;

                            lblQuantity.Visible = false;
                            ddlAssetAttribute.Visible = true;
                            //ddlAssetAttribute.CssClass = "form-control required";
                            //ddlAssetAttribute.Attributes.Add("required", "required");
                        }
                    }
                    else
                    {

                        lblAssetAttributeValue.Visible = false;
                        txtAssetAttributeValue.Visible = false;
                        lblQuantity.Visible = false;

                        ddlAssetAttribute.SelectedItem.Value = "";
                        ddlAssetAttribute.Enabled = false;
                        ddlAssetAttribute.Visible = true;
                        //ddlAssetAttribute.CssClass = "form-control";
                        //ddlAssetAttribute.Attributes.Remove("required");

                    }

                }



            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlAssetAttribute_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlSubCategory = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlSubCategory.NamingContainer;

                DropDownList ddlAssetAttribute = (DropDownList)gvRow.FindControl("ddlAssetAttribute");
                DropDownList ddlAsset = (DropDownList)gvRow.FindControl("ddlAsset");
                Label lblAssetAttributeValue = (Label)gvRow.FindControl("lblAssetAttributeValue");
                TextBox txtAssetAttributeValue = (TextBox)gvRow.FindControl("txtAssetAttributeValue");
                Label lblLotValue = (Label)gvRow.FindControl("lblLotValue");

                if (gvRow != null)
                {
                    if (ddlAsset.SelectedIndex != 0 && ddlAssetAttribute.SelectedIndex != 0)
                    {

                        AM_AssetItems Items = new ReferenceDataBLL().GetAssetLotQuantity(Convert.ToInt64(ddlAsset.SelectedItem.Value));
                        lblLotValue.Text = Items.LotQuantity.ToString();
                        if (Items.AssetType.ToLower() == "lot")
                        {
                            lblAssetAttributeValue.Text = Convert.ToString(Items.LotQuantity);
                            lblAssetAttributeValue.Visible = false;

                            txtAssetAttributeValue.Visible = true;
                            txtAssetAttributeValue.Text = Convert.ToString(Items.LotQuantity);
                        }
                        else
                        {
                            txtAssetAttributeValue.Visible = false;
                            lblAssetAttributeValue.Visible = true;
                            lblAssetAttributeValue.Text = Convert.ToString(new ReferenceDataBLL().GetAttributeValue(Convert.ToInt64(ddlAssetAttribute.SelectedItem.Value), Convert.ToInt64(ddlAsset.SelectedItem.Value)));
                        }
                    }

                }

                if (ddlAssetAttribute.SelectedItem.Value == "")
                {
                    lblAssetAttributeValue.Visible = false;
                    txtAssetAttributeValue.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion
        #endregion


        #region Functions
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindAssetAllocationGrid()
        {
            try
            {
                List<object> lstAssetAllocation = new ReferenceDataBLL().GetAssetAllocation(Convert.ToInt64(hdnResourceAllocationID.Value));
                gvAssetAllocation.DataSource = lstAssetAllocation;
                gvAssetAllocation.DataBind();

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteVillage");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }


        #endregion



    }
}