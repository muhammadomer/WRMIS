using PMIU.WRMIS.BLL.Auctions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Auctions
{
    public partial class AddAuctionAssets : BasePage
    {
        #region ViewState Constants

        string VSAuctionAssetID = "AuctionAssetID";
        string VSSubCategoryID = "SubCategoryID";
        string VSLevelID = "LevelID";

        #endregion

        private static bool _IsSaved = false;
        public static bool IsSaved
        {
            get
            {
                return _IsSaved;
            }
            set
            {
                _IsSaved = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["AuctionNoticeID"]))
                    {
                        if (_IsSaved)
                        {
                            Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                            _IsSaved = false; // Reset flag after displaying message.
                        }
                        hdnAuctionNoticeID.Value = Convert.ToString(Request.QueryString["AuctionNoticeID"]);
                        BindAuctionDetailData(Convert.ToInt64(hdnAuctionNoticeID.Value));
                        BindAuctionAssetsGridView();
                        hlBack.NavigateUrl = "~/Modules/Auctions/SearchAuctions.aspx?ShowHistory=true";
                    }
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindAuctionDetailData(long _AuctionNoticeID)
        {
            try
            {
                AC_AuctionNotice mdlAuctionData = new AuctionBLL().GetAuctionDetailsByID(_AuctionNoticeID);
                string AuctionNotice = mdlAuctionData.AuctionTitle;
                string OpeningDate = Utility.GetFormattedDate(mdlAuctionData.OpeningDate);
                string SubmissionDate = Utility.GetFormattedDate(mdlAuctionData.SubmissionDate);

                Auctions.Controls.AuctionNotice.AuctionNoticeName = AuctionNotice;
                Auctions.Controls.AuctionNotice.OpeningDate = OpeningDate;
                Auctions.Controls.AuctionNotice.SubmissionDate = SubmissionDate;

            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #region Individual
        protected void gvAuctionAssets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string GroupIndividual = GetDataKeyValue(gvAuctionAssets, "GroupIndividual", e.Row.RowIndex);

                    if (GroupIndividual.ToUpper() == "GROUP" && base.CanAdd)
                    {
                        Button btnAssets = (Button)e.Row.FindControl("btnAssets");
                        if (btnAssets != null)
                        {
                            btnAssets.Visible = true;
                        }

                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow && gvAuctionAssets.EditIndex == e.Row.RowIndex)
                {


                    long AuctionAssetID = Convert.ToInt64(GetDataKeyValue(gvAuctionAssets, "AuctionAssetID", e.Row.RowIndex));

                    #region "Datakeys"
                    string LevelID = GetDataKeyValue(gvAuctionAssets, "LevelID", e.Row.RowIndex);
                    string GroupIndividual = GetDataKeyValue(gvAuctionAssets, "GroupIndividual", e.Row.RowIndex);
                    string CategoryID = GetDataKeyValue(gvAuctionAssets, "CategoryID", e.Row.RowIndex);
                    string SubCategoryID = GetDataKeyValue(gvAuctionAssets, "SubCategoryID", e.Row.RowIndex);
                    string NameID = GetDataKeyValue(gvAuctionAssets, "NameID", e.Row.RowIndex);
                    string Name = GetDataKeyValue(gvAuctionAssets, "Name", e.Row.RowIndex);
                    string AttributeTypeID = GetDataKeyValue(gvAuctionAssets, "AttributeTypeID", e.Row.RowIndex);
                    string AttributeValue = GetDataKeyValue(gvAuctionAssets, "AttributeValue", e.Row.RowIndex);
                    string CreatedBy = GetDataKeyValue(gvAuctionAssets, "CreatedBy", e.Row.RowIndex);
                    string CreatedDate = GetDataKeyValue(gvAuctionAssets, "CreatedDate", e.Row.RowIndex);
                    #endregion

                    #region "Controls"
                    DropDownList ddlLevel = (DropDownList)e.Row.FindControl("ddlLevel");
                    DropDownList ddlGroupIndividual = (DropDownList)e.Row.FindControl("ddlGroupIndividual");
                    DropDownList ddlCategory = (DropDownList)e.Row.FindControl("ddlCategory");
                    DropDownList ddlSubCategory = (DropDownList)e.Row.FindControl("ddlSubCategory");
                    DropDownList ddlName = (DropDownList)e.Row.FindControl("ddlName");
                    DropDownList ddlAttributeType = (DropDownList)e.Row.FindControl("ddlAttributeType");
                    DropDownList ddlAttributeValue = (DropDownList)e.Row.FindControl("ddlAttributeValue");
                    TextBox txtGroupName = (TextBox)e.Row.FindControl("txtGroupName");
                    TextBox txtAttributeValue = (TextBox)e.Row.FindControl("txtAttributeValue");

                    #endregion
                    bool IsHeadQuarterDivision = new AuctionBLL().IsHeadQuarterDivision(SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                    if (IsHeadQuarterDivision)
                    {
                        if (ddlLevel != null)
                        {
                            ddlLevel.Enabled = true;
                            ddlLevel.CssClass = "form-control required";
                            ddlLevel.Attributes.Add("required", "required");
                            BindAuctionLevelDropDown(ddlLevel);

                            Dropdownlist.SetSelectedValue(ddlLevel, LevelID);
                        }
                    }
                    else
                    {
                        BindAuctionLevelDropDown(ddlLevel);
                        ddlLevel.Enabled = false;
                        ddlLevel.CssClass = "form-control";
                        ddlLevel.Attributes.Remove("required");
                    }

                    if (ddlGroupIndividual != null)
                    {
                        BindIndividualOrGroupDropDown(ddlGroupIndividual);
                        if (!string.IsNullOrEmpty(GroupIndividual))
                        {
                            if (GroupIndividual.ToUpper() == "INDIVIDUAL")
                            {
                                Dropdownlist.SetSelectedValue(ddlGroupIndividual, "1");
                            }
                            else
                            {
                                Dropdownlist.SetSelectedValue(ddlGroupIndividual, "2");

                            }
                        }

                    }
                    if (ddlCategory != null)
                    {
                        Dropdownlist.BindDropdownlist<List<object>>(ddlCategory, new AuctionBLL().GetAssetCategories(), (int)Constants.DropDownFirstOption.Select);
                        if (!string.IsNullOrEmpty(CategoryID))
                        {
                            Dropdownlist.SetSelectedValue(ddlCategory, CategoryID);
                        }

                    }
                    if (ddlSubCategory != null)
                    {
                        Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, new AuctionBLL().GetAssetSubCategoriesByCategoryID(Convert.ToInt64(CategoryID)), (int)Constants.DropDownFirstOption.Select);
                        Dropdownlist.SetSelectedValue(ddlSubCategory, SubCategoryID);

                    }

                    if (GroupIndividual.ToUpper() == "INDIVIDUAL")
                    {
                        if (NameID != null)
                        {
                            List<object> lstAttributes = new AuctionBLL().GetAttributesBySubCategoryID(Convert.ToInt64(NameID));
                            Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeType, lstAttributes);
                            if (!string.IsNullOrEmpty(AttributeTypeID))
                            {
                                Dropdownlist.SetSelectedValue(ddlAttributeType, AttributeTypeID);
                            }

                            List<long> AssetItemIDs = new List<long>();
                            //List<dynamic> AssetItems = new AuctionBLL().GetAllAssetsAddedAgainstTenderNotice(Convert.ToInt64(hdnAuctionNoticeID.Value));
                            //foreach (var item in AssetItems)
                            //{
                            //    AssetItemIDs.Add(Convert.ToInt64(Utility.GetDynamicPropertyValue(item,"ID")));
                            //}
                            if (GroupIndividual.ToUpper() == "INDIVIDUAL")
                            {
                                if (!string.IsNullOrEmpty(LevelID))
                                {
                                    if (LevelID == "1")
                                    {
                                        long? CircleID = new AuctionBLL().GetCircleIDByDivisionID((long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                                        long ZoneID = new AuctionBLL().GetZoneIDByCircleID((long)CircleID);
                                        List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryIDForZoneLevel(Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, (long)CircleID, ZoneID, AssetItemIDs);
                                        Dropdownlist.BindDropdownlist<List<object>>(ddlName, lstAssetNames);
                                        Dropdownlist.SetSelectedValue(ddlName, NameID);
                                    }
                                    else if (LevelID == "2")
                                    {
                                        long? CircleID = new AuctionBLL().GetCircleIDByDivisionID((long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                                        List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryIDForCircleLevel(Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, (long)CircleID, AssetItemIDs);
                                        Dropdownlist.BindDropdownlist<List<object>>(ddlName, lstAssetNames);
                                        Dropdownlist.SetSelectedValue(ddlName, NameID);
                                    }
                                    else
                                    {
                                        List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryID(Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, AssetItemIDs);

                                        Dropdownlist.BindDropdownlist<List<object>>(ddlName, lstAssetNames);
                                        Dropdownlist.SetSelectedValue(ddlName, NameID);
                                    }
                                }
                                else
                                {
                                    List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryID(Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, AssetItemIDs);
                                    Dropdownlist.BindDropdownlist<List<object>>(ddlName, lstAssetNames);
                                    Dropdownlist.SetSelectedValue(ddlName, NameID);
                                }

                            }


                        }
                    }
                    else if(GroupIndividual.ToUpper() == "GROUP")
                    {
                        if (txtGroupName != null)
                        {
                            ddlName.Visible = false;
                            txtGroupName.Visible = true;
                            ddlAttributeType.Visible = false;
                            //ddlAttributeValue.Visible = false;
                            txtGroupName.Text = Name;
                        }
                    }
                   
                    if (!string.IsNullOrEmpty(AttributeValue))
                    {
                        txtAttributeValue.Text = AttributeValue;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAuctionAssets_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                long AuctionAssetID = Convert.ToInt64(GetDataKeyValue(gvAuctionAssets, "AuctionAssetID", e.NewEditIndex));
                string GroupIndividual = GetDataKeyValue(gvAuctionAssets, "GroupIndividual", e.NewEditIndex);
                bool IsExists = new AuctionBLL().IfItemsExistsAgainstGroup(AuctionAssetID);
                if (IsExists && GroupIndividual.ToUpper() == "GROUP")
                {
                    Master.ShowMessage(Message.CannotEdit.Description, SiteMaster.MessageType.Error);
                    //gvAuctionAssets.EditIndex = -1;
                    //BindAuctionAssetsGridView();
                    //return;
                    e.Cancel = true;
                }
                else
                {
                    gvAuctionAssets.EditIndex = e.NewEditIndex;
                    BindAuctionAssetsGridView();
                }




            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAuctionAssets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddAuctionAsset")
                {
                    List<object> lstAuctionAssets = new AuctionBLL().GetAuctionAssetsByNoticeID(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    //lstGaugeInspection.Add(GetNewScheduleDetail());
                    lstAuctionAssets.Insert(0, GetNewAuctionAsset());
                    gvAuctionAssets.PageIndex = gvAuctionAssets.PageCount;
                    gvAuctionAssets.DataSource = lstAuctionAssets;
                    gvAuctionAssets.DataBind();

                    gvAuctionAssets.EditIndex = 0;//gvGaugeInspection.Rows.Count - 1;
                    gvAuctionAssets.DataBind();
                }
                if (e.CommandName == "Assets")
                {
                    GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;
                    long AuctionAssetID = Convert.ToInt64(GetDataKeyValue(gvAuctionAssets, "AuctionAssetID", row.RowIndex));
                    long SubCategoryID = Convert.ToInt64(GetDataKeyValue(gvAuctionAssets, "SubCategoryID", row.RowIndex));
                    string LevelVal = GetDataKeyValue(gvAuctionAssets, "LevelID", row.RowIndex);
                    long LevelID = LevelVal == "" ? 0 : Convert.ToInt64(LevelVal);
                    ViewState[VSAuctionAssetID] = Convert.ToString(AuctionAssetID);
                    ViewState[VSSubCategoryID] = Convert.ToString(SubCategoryID);
                    ViewState[VSLevelID] = Convert.ToString(LevelID);
                    List<dynamic> lstAssetsItems = new AuctionBLL().GetAuctionAssetItemsByID(AuctionAssetID);
                    gvGroupAsset.DataSource = lstAssetsItems;
                    gvGroupAsset.DataBind();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AssetsGrouped", "$('#AssetsGrouped').modal();", true);
                }
                if (e.CommandName == "ItemDetails")
                {
                    GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;
                    long AuctionAssetID = Convert.ToInt64(GetDataKeyValue(gvAuctionAssets, "AuctionAssetID", row.RowIndex));
                    long AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                    Response.Redirect("~/Modules/Auctions/AuctionAssetDetails.aspx?AuctionNoticeID=" + AuctionNoticeID + "&AuctionAssetID=" + AuctionAssetID, false);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAuctionAssets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long AuctionAssetID = Convert.ToInt64(GetDataKeyValue(gvAuctionAssets, "AuctionAssetID", e.RowIndex));
                string GroupIndividual = GetDataKeyValue(gvAuctionAssets, "AuctionAssetID", e.RowIndex);
                bool IsItemDeleted = new AuctionBLL().DeleteAuctionAssetItemByAuctionAssetID(AuctionAssetID);
                if (IsItemDeleted)
                {
                    bool IsDeleted = new AuctionBLL().DeleteAuctionAssetByID(AuctionAssetID);
                    if (IsDeleted)
                    {
                        Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                        BindAuctionAssetsGridView();
                    }
                    else
                    {
                        Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    }
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvAuctionAssets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAuctionAssets.PageIndex = e.NewPageIndex;
                gvAuctionAssets.EditIndex = -1;
                BindAuctionAssetsGridView();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
        protected void gvAuctionAssets_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long AuctionAssetID = Convert.ToInt64(GetDataKeyValue(gvAuctionAssets, "AuctionAssetID", e.RowIndex));
                GridViewRow row = gvAuctionAssets.Rows[e.RowIndex];
                #region "Datakeys"
                string LevelID = GetDataKeyValue(gvAuctionAssets, "LevelID", e.RowIndex);
                string GroupIndividual = GetDataKeyValue(gvAuctionAssets, "GroupIndividual", e.RowIndex);
                string CategoryID = GetDataKeyValue(gvAuctionAssets, "CategoryID", e.RowIndex);
                string SubCategoryID = GetDataKeyValue(gvAuctionAssets, "SubCategoryID", e.RowIndex);
                string NameID = GetDataKeyValue(gvAuctionAssets, "NameID", e.RowIndex);
                string AttributeTypeID = GetDataKeyValue(gvAuctionAssets, "AttributeTypeID", e.RowIndex);
                string AttributeValue = GetDataKeyValue(gvAuctionAssets, "AttributeValue", e.RowIndex);
                string CreatedBy = GetDataKeyValue(gvAuctionAssets, "CreatedBy", e.RowIndex);
                string CreatedDate = GetDataKeyValue(gvAuctionAssets, "CreatedDate", e.RowIndex);
                #endregion

                #region "Controls"
                DropDownList ddlLevel = (DropDownList)row.FindControl("ddlLevel");
                DropDownList ddlGroupIndividual = (DropDownList)row.FindControl("ddlGroupIndividual");
                DropDownList ddlCategory = (DropDownList)row.FindControl("ddlCategory");
                DropDownList ddlSubCategory = (DropDownList)row.FindControl("ddlSubCategory");
                DropDownList ddlName = (DropDownList)row.FindControl("ddlName");
                DropDownList ddlAttributeType = (DropDownList)row.FindControl("ddlAttributeType");
                DropDownList ddlAttributeValue = (DropDownList)row.FindControl("ddlAttributeValue");
                TextBox txtAttributeValue = (TextBox)row.FindControl("txtAttributeValue");
                TextBox txtGroupName = (TextBox)row.FindControl("txtGroupName");

                #endregion



                AC_AuctionAssets AssetEntity = new AC_AuctionAssets();
                if (ddlGroupIndividual.SelectedItem.Value == "2")
                {
                    AssetEntity.GroupIndividual = "Group";
                    AssetEntity.GroupName = txtGroupName.Text;
                }
                else
                {
                    AssetEntity.GroupIndividual = "Individual";
                }
                AssetEntity.AssetCategoryID = Convert.ToInt64(ddlCategory.SelectedItem.Value);
                AssetEntity.AssetSubCategoryID = Convert.ToInt64(ddlSubCategory.SelectedItem.Value);
                AssetEntity.AuctionNoticeID = Convert.ToInt64(hdnAuctionNoticeID.Value);
                if (AuctionAssetID > 0)
                {
                    AssetEntity.ID = AuctionAssetID;
                    AssetEntity.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    AssetEntity.ModifiedDate = DateTime.Now;
                }
                else
                {
                    AssetEntity.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                    AssetEntity.CreatedDate = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(ddlLevel.SelectedItem.Value))
                {
                    AssetEntity.IrrigationLevelID = Convert.ToInt64(ddlLevel.SelectedItem.Value);
                }

                using (TransactionScope transaction = new TransactionScope())
                {
                    long AssetID = new AuctionBLL().SaveAssetParent(AssetEntity);
                    long AssetItemID = 0;
                    if (AuctionAssetID > 0)
                    {
                        AssetItemID = new AuctionBLL().GetAssetItemIDByAuctionAssetID(AssetID);
                    }
                    hdnAuctionAssetID.Value = Convert.ToString(AssetID);
                    if (ddlGroupIndividual.SelectedItem.Value == "1")
                    {
                        AC_AuctionAssetItems mdlAssetItem = new AC_AuctionAssetItems();
                        mdlAssetItem.AuctionAssetID = AssetID;
                        if (!string.IsNullOrEmpty(ddlAttributeType.SelectedItem.Value) && ddlAttributeType.SelectedItem.Value != "0")
                        {
                            mdlAssetItem.AssetAttributeID = Convert.ToInt64(ddlAttributeType.SelectedItem.Value);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(txtAttributeValue.Text))
                            {
                                mdlAssetItem.AssetQuantity = Convert.ToInt32(txtAttributeValue.Text);
                            }

                        }
                        mdlAssetItem.AssetItemID = Convert.ToInt64(ddlName.SelectedItem.Value);
                        if (AssetItemID > 0)
                        {
                            mdlAssetItem.ID = AssetItemID;
                            mdlAssetItem.ModifiedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                            mdlAssetItem.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            mdlAssetItem.CreatedBy = Convert.ToInt32(SessionManagerFacade.UserInformation.ID);
                            mdlAssetItem.CreatedDate = DateTime.Now;
                        }

                        bool IsSaved = new AuctionBLL().SaveAssetItem(mdlAssetItem);
                    }

                    transaction.Complete();


                }

                // if (isSaved)
                // {
                // Redirect user to first page.
                if (Convert.ToInt64(AuctionAssetID) == 0)
                    gvAuctionAssets.PageIndex = 0;

                gvAuctionAssets.EditIndex = -1;
                BindAuctionAssetsGridView();
                Master.ShowMessage(Message.RecordSaved.Description);
                // }


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAuctionAssets_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvAuctionAssets.EditIndex = -1;
                BindAuctionAssetsGridView();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GetSubCategories(sender, "ddlCategory", "ddlSubCategory");
        }
        protected void ddlSubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GetAssetNamesandAttributes(sender, "ddlSubCategory", "ddlName", "ddlAttributeType");
        }
        protected void ddlAttributeType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GetAttributeValue(sender, "ddlAttributeType", "txtAttributeValue");
        }
        protected void ddlGroupIndividual_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DisableAttributeFields(sender, "ddlGroupIndividual", "ddlName", "ddlAttributeType");
        }
        protected void ddlName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            IndividualLotCase(sender, "ddlName", "ddlAttributeType", "txtAttributeValue");
        }
        private void GetSubCategories(object sender, string _DDLCategory, string _DDLSubCategory)
        {
            try
            {
                DropDownList DDLControl = (DropDownList)sender;
                DropDownList ddlCategory = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLCategory);
                DropDownList ddlSubCategory = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLSubCategory);
                if (string.IsNullOrEmpty(ddlCategory.SelectedItem.Value))
                {
                    List<object> lstEmpty = new List<object>();
                    Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, lstEmpty);
                    // Dropdownlist.DDLStructureChannels(ddlInspectionAreas, 0, true);
                }
                else
                {
                    List<object> lstSubCategories = new AuctionBLL().GetAssetSubCategoriesByCategoryID(Convert.ToInt64(ddlCategory.SelectedItem.Value));
                    Dropdownlist.BindDropdownlist<List<object>>(ddlSubCategory, lstSubCategories);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void GetAssetNamesandAttributes(object sender, string _DDLSubCategory, string _DDLName, string _DDLAttributeType)
        {
            try
            {
                DropDownList DDLControl = (DropDownList)sender;
                DropDownList ddlLevel = (DropDownList)DDLControl.NamingContainer.FindControl("ddlLevel");
                DropDownList ddlSubCategory = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLSubCategory);
                DropDownList ddlAssetName = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLName);
                DropDownList ddlAttributeType = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLAttributeType);
                DropDownList ddlGroupIndividual = (DropDownList)DDLControl.NamingContainer.FindControl("ddlGroupIndividual");
                TextBox lblAttributeValue = (TextBox)DDLControl.NamingContainer.FindControl("txtAttributeValue");
                if (string.IsNullOrEmpty(ddlSubCategory.SelectedItem.Value))
                {
                    List<object> lstEmpty = new List<object>();
                    Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstEmpty);
                    //Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeType, lstEmpty);
                    //ddlAttributeType.Enabled = true;
                    //ddlAttributeType.Attributes.Add("required", "required");
                    //ddlAttributeType.CssClass = "form-control required";
                    lblAttributeValue.Text = "";
                    lblAttributeValue.Attributes.Remove("required");
                    lblAttributeValue.CssClass = "form-control decimalIntegerInput";
                    lblAttributeValue.Enabled = false;
                    // Dropdownlist.DDLStructureChannels(ddlInspectionAreas, 0, true);
                }
                else
                {
                    List<long> AssetItemIDs = new List<long>();
                    List<dynamic> AssetItems = new AuctionBLL().GetAllAssetsAddedAgainstTenderNotice(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    foreach (var item in AssetItems)
                    {
                        AssetItemIDs.Add(Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ID")));
                    }
                    if (ddlGroupIndividual.SelectedItem.Value == "1")
                    {
                        if (!string.IsNullOrEmpty(ddlLevel.SelectedItem.Value))
                        {
                            if (ddlLevel.SelectedItem.Value == "1")
                            {
                                long? CircleID = new AuctionBLL().GetCircleIDByDivisionID((long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                                long ZoneID = new AuctionBLL().GetZoneIDByCircleID((long)CircleID);
                                List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryIDForZoneLevel(Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, (long)CircleID, ZoneID, AssetItemIDs);
                                Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstAssetNames);
                            }
                            else if (ddlLevel.SelectedItem.Value == "2")
                            {
                                long? CircleID = new AuctionBLL().GetCircleIDByDivisionID((long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                                List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryIDForCircleLevel(Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, (long)CircleID, AssetItemIDs);
                                Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstAssetNames);
                            }
                            else
                            {
                                List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryID(Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, AssetItemIDs);

                                Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstAssetNames);
                            }
                        }
                        else
                        {
                            List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryID(Convert.ToInt64(ddlSubCategory.SelectedItem.Value), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, AssetItemIDs);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstAssetNames);
                        }



                        //List<object> lstAttributes = new AuctionBLL().GetAttributesBySubCategoryID(Convert.ToInt64(ddlSubCategory.SelectedItem.Value));
                        //Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeType, lstAttributes);
                        //ddlAttributeType.Enabled = true;
                        //ddlAttributeType.Attributes.Add("required", "required");
                        //ddlAttributeType.CssClass = "form-control required";
                        lblAttributeValue.Text = "";
                        lblAttributeValue.Attributes.Remove("required");
                        lblAttributeValue.CssClass = "form-control decimalIntegerInput";
                        lblAttributeValue.Enabled = false;
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void GetAttributeValue(object sender, string _DDLAttributeType, string _TXTAttributeValue)
        {
            try
            {
                DropDownList DDLControl = (DropDownList)sender;
                //Label LBLControl = (Label)sender;
                DropDownList ddlAttributeType = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLAttributeType);
                TextBox lblAttributeValue = (TextBox)DDLControl.NamingContainer.FindControl(_TXTAttributeValue);
                if (string.IsNullOrEmpty(ddlAttributeType.SelectedItem.Value))
                {
                    List<object> lstEmpty = new List<object>();
                    lblAttributeValue.Text = "";

                    // Dropdownlist.DDLStructureChannels(ddlInspectionAreas, 0, true);
                }
                else
                {
                    string AttributeValue = new AuctionBLL().GetAttributeValueByID(Convert.ToInt64(ddlAttributeType.SelectedItem.Value));
                    lblAttributeValue.Text = AttributeValue;

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void DisableAttributeFields(object sender, string _DDLGroupIndividual, string _DDLName, string _DDLAttributeType)
        {
            try
            {
                DropDownList DDLControl = (DropDownList)sender;
                DropDownList ddlGroupIndividual = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLGroupIndividual);
                DropDownList ddlAssetName = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLName);
                DropDownList ddlAttributeType = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLAttributeType);
                DropDownList ddlCategory = (DropDownList)DDLControl.NamingContainer.FindControl("ddlCategory");
                DropDownList ddlSubCategory = (DropDownList)DDLControl.NamingContainer.FindControl("ddlSubCategory");
                TextBox lblAttributeValue = (TextBox)DDLControl.NamingContainer.FindControl("txtAttributeValue");
                TextBox txtGroupName = (TextBox)DDLControl.NamingContainer.FindControl("txtGroupName");
                // Button BtnAsset = (Button)DDLControl.NamingContainer.FindControl("btnAssets");
                ddlCategory.SelectedIndex = 0;
                ddlSubCategory.SelectedIndex = 0;
                lblAttributeValue.Text = "";

                if (string.IsNullOrEmpty(ddlGroupIndividual.SelectedItem.Value))
                {
                    List<object> lstEmpty = new List<object>();
                    Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstEmpty);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeType, lstEmpty);
                    ddlAssetName.Enabled = true;
                    ddlAssetName.Visible = true;
                    ddlAssetName.Attributes.Add("required", "required");
                    ddlAssetName.CssClass = "form-control required";
                    ddlAttributeType.Enabled = true;
                    //ddlAttributeType.Attributes.Add("required", "required");
                    //ddlAttributeType.CssClass = "form-control required";
                    txtGroupName.Visible = false;
                    txtGroupName.Attributes.Remove("required");
                    txtGroupName.CssClass = "form-control";
                    // Dropdownlist.DDLStructureChannels(ddlInspectionAreas, 0, true);
                }
                else if (ddlGroupIndividual.SelectedItem.Value == "2")
                {
                    List<object> lstEmpty = new List<object>();
                    Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstEmpty);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeType, lstEmpty);
                    ddlAssetName.Enabled = false;
                    ddlAssetName.Visible = false;
                    ddlAssetName.Attributes.Remove("required");
                    ddlAssetName.CssClass = "form-control";
                    ddlAttributeType.Enabled = false;
                    //ddlAttributeType.Attributes.Remove("required");
                    //ddlAttributeType.CssClass = "form-control";
                    txtGroupName.Visible = true;
                    txtGroupName.Attributes.Add("required", "required");
                    txtGroupName.CssClass = "form-control required";
                    //BtnAsset.Visible = true;
                }
                else
                {
                    List<object> lstEmpty = new List<object>();
                    Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstEmpty);
                    Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeType, lstEmpty);
                    ddlAssetName.Enabled = true;
                    ddlAssetName.Visible = true;
                    ddlAssetName.Attributes.Add("required", "required");
                    ddlAssetName.CssClass = "form-control required";
                    ddlAttributeType.Enabled = true;
                    //ddlAttributeType.Attributes.Add("required", "required");
                    //ddlAttributeType.CssClass = "form-control required";
                    txtGroupName.Visible = false;
                    txtGroupName.Attributes.Remove("remove");
                    txtGroupName.CssClass = "form-control";
                    //BtnAsset.Visible = false;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void IndividualLotCase(object sender, string _DDLName, string _DDLAttributeType, string _TXTAttributeValue)
        {
            try
            {
                DropDownList DDLControl = (DropDownList)sender;
                DropDownList ddlAssetName = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLName);
                DropDownList ddlAttributeType = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLAttributeType);
                TextBox lblAttributeValue = (TextBox)DDLControl.NamingContainer.FindControl(_TXTAttributeValue);

                string AssetType = new AuctionBLL().GetAssetTypeByID(Convert.ToInt64(ddlAssetName.SelectedItem.Value));
                if (AssetType.ToUpper() == "LOT")
                {
                    ddlAttributeType.Items.Insert(0, new ListItem("Quantity", "0"));
                    ddlAttributeType.SelectedIndex = 0;
                    //ddlAttributeType.Attributes.Remove("required");
                    //ddlAttributeType.CssClass = "form-control";
                    ddlAttributeType.Enabled = false;
                    lblAttributeValue.Text = "";
                    lblAttributeValue.Enabled = true;
                    lblAttributeValue.Attributes.Add("required", "required");
                    lblAttributeValue.CssClass = "form-control decimalIntegerInput required";
                }
                else
                {
                    //ddlAttributeType.SelectedIndex = 0;
                    //ddlAttributeType.Items.RemoveAt(0);
                    List<object> lstAttributes = new AuctionBLL().GetAttributesBySubCategoryID(Convert.ToInt64(ddlAssetName.SelectedItem.Value));
                    Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeType, lstAttributes);
                    //ddlAttributeType.Attributes.Add("required", "required");
                    //ddlAttributeType.CssClass = "form-control required";
                    ddlAttributeType.Enabled = true;
                    lblAttributeValue.Text = "";
                    lblAttributeValue.Enabled = false;
                    lblAttributeValue.Attributes.Remove("required");
                    lblAttributeValue.CssClass = "form-control decimalIntegerInput";
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindIndividualOrGroupDropDown(DropDownList DDL)
        {
            try
            {
                DDL.DataSource = CommonLists.GetAuctionIndividualGrouped();
                DDL.DataTextField = "Name";
                DDL.DataValueField = "ID";
                DDL.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void BindAuctionLevelDropDown(DropDownList DDL)
        {
            try
            {
                DDL.DataSource = CommonLists.GetAuctionLevels();
                DDL.DataTextField = "Name";
                DDL.DataValueField = "ID";
                DDL.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindAuctionAssetsGridView()
        {
            try
            {
                gvAuctionAssets.DataSource = new AuctionBLL().GetAuctionAssetsByNoticeID(Convert.ToInt64(hdnAuctionNoticeID.Value));
                gvAuctionAssets.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        private void OnAssetNameChanged(object sender, string _DDLName, string _DDLAttributeType, string _TXTAttributeValue)
        {
            try
            {
                DropDownList DDLControl = (DropDownList)sender;
                DropDownList ddlAssetName = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLName);
                DropDownList ddlAttributeType = (DropDownList)DDLControl.NamingContainer.FindControl(_DDLAttributeType);
                TextBox lblAttributeValue = (TextBox)DDLControl.NamingContainer.FindControl(_TXTAttributeValue);


                if (string.IsNullOrEmpty(ddlAssetName.SelectedItem.Value))
                {
                    ddlAttributeType.Items.RemoveAt(0);
                    ddlAttributeType.SelectedIndex = 0;
                    //List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryID(Convert.ToInt64(ViewState[VSSubCategoryID]));
                    //Dropdownlist.BindDropdownlist<List<object>>(ddlAssetName, lstAssetNames);
                    ddlAttributeType.Enabled = true;
                    lblAttributeValue.Enabled = false;
                    lblAttributeValue.Attributes.Remove("required");
                    lblAttributeValue.CssClass = "form-control decimalIntegerInput";
                }
                else
                {
                    string AssetType = new AuctionBLL().GetAssetTypeByID(Convert.ToInt64(ddlAssetName.SelectedItem.Value));
                    if (AssetType.ToUpper() == "LOT")
                    {
                        ddlAttributeType.Items.Insert(0, new ListItem("Quantity", "0"));
                        ddlAttributeType.SelectedIndex = 0;
                        //ddlAttributeType.Attributes.Remove("required");
                        //ddlAttributeType.CssClass = "form-control";
                        ddlAttributeType.Enabled = false;
                        lblAttributeValue.Text = "";
                        lblAttributeValue.Enabled = true;
                        lblAttributeValue.Attributes.Add("required", "required");
                        lblAttributeValue.CssClass = "form-control decimalIntegerInput required";
                    }
                    else
                    {
                        //ddlAttributeType.SelectedIndex = 0;
                        //ddlAttributeType.Items.RemoveAt(0);
                        List<object> lstAttributes = new AuctionBLL().GetAttributesBySubCategoryID(Convert.ToInt64(ddlAssetName.SelectedItem.Value));
                        Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeType, lstAttributes);
                        //ddlAttributeType.Attributes.Add("required", "required");
                        //ddlAttributeType.CssClass = "form-control required";
                        ddlAttributeType.Enabled = true;
                        lblAttributeValue.Text = "";
                        lblAttributeValue.Enabled = false;
                        lblAttributeValue.Attributes.Remove("required");
                        lblAttributeValue.CssClass = "form-control decimalIntegerInput";
                    }
                }


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void txtAttributeValue_OnTextChanged(object sender, EventArgs e)
        {
            TextBox DDLControl = (TextBox)sender;
            DropDownList ddlAssetName = (DropDownList)DDLControl.NamingContainer.FindControl("ddlName");
            TextBox lblAttributeValue = (TextBox)DDLControl.NamingContainer.FindControl("txtAttributeValue");
            int Value;
            if (int.TryParse(lblAttributeValue.Text, out Value))
            {
                bool Isvalid = new AuctionBLL().CompareLotQuantity(Value, Convert.ToInt64(ddlAssetName.SelectedItem.Value));
                if (!Isvalid)
                {
                    lblAttributeValue.Text = "";
                    Master.ShowMessage(Message.AssetQuantityNotValid.Description, SiteMaster.MessageType.Error);
                }
            }
            else
            {
                lblAttributeValue.Text = "";
                Master.ShowMessage(Message.NumericCharactersOnly.Description, SiteMaster.MessageType.Error);
            }
           
           
            //GetAssetNamesandAttributes(sender, "ddlSubCategory", "ddlName", "ddlAttributeType");
        }
        #endregion

        #region Group
        protected void gvGroupAsset_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddGroupedAssets")
                {
                    List<object> lstAuctionAssets = new AuctionBLL().GetAuctionAssetItemsByID(Convert.ToInt64(ViewState[VSAuctionAssetID]));
                    //lstGaugeInspection.Add(GetNewScheduleDetail());
                    lstAuctionAssets.Insert(0, GetNewGroupedAuctionAsset());
                    gvGroupAsset.PageIndex = gvGroupAsset.PageCount;
                    gvGroupAsset.DataSource = lstAuctionAssets;
                    gvGroupAsset.DataBind();

                    gvGroupAsset.EditIndex = 0;//gvGaugeInspection.Rows.Count - 1;
                    gvGroupAsset.DataBind();


                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGroupAsset_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvGroupAsset.EditIndex == e.Row.RowIndex)
                {
                    long AuctionAssetID = Convert.ToInt64(GetDataKeyValue(gvGroupAsset, "AuctionAssetID", e.Row.RowIndex));

                    #region "Datakeys"
                    string AuctionAssetItemID = GetDataKeyValue(gvGroupAsset, "AuctionAssetItemID", e.Row.RowIndex);
                    string AssetItemID = GetDataKeyValue(gvGroupAsset, "AssetItemID", e.Row.RowIndex);
                    string AttributeTypeID = GetDataKeyValue(gvGroupAsset, "AttributeTypeID", e.Row.RowIndex);
                    string AttributeValue = GetDataKeyValue(gvGroupAsset, "AttributeValue", e.Row.RowIndex);
                    string LotQuantity = GetDataKeyValue(gvGroupAsset, "LotQuantity", e.Row.RowIndex);
                    string CreatedBy = GetDataKeyValue(gvGroupAsset, "CreatedBy", e.Row.RowIndex);
                    string CreatedDate = GetDataKeyValue(gvGroupAsset, "CreatedDate", e.Row.RowIndex);
                    #endregion


                    DropDownList ddlNameGrouped = (DropDownList)e.Row.FindControl("ddlNameGrouped");
                    DropDownList ddlAttributeTypeGrouped = (DropDownList)e.Row.FindControl("ddlAttributeTypeGrouped");
                    TextBox txtAttributeValueGrouped = (TextBox)e.Row.FindControl("txtAttributeValueGrouped");


                    //ToDO:

                    List<long> AssetItemIDs = new List<long>();
                    List<dynamic> AssetItems = new AuctionBLL().GetAllAssetsAddedAgainstTenderNotice(Convert.ToInt64(hdnAuctionNoticeID.Value));
                    foreach (var item in AssetItems)
                    {
                        AssetItemIDs.Add(Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ID")));
                    }


                    int IrrigationLevelID = Convert.ToInt32(ViewState[VSLevelID]);
                    if (IrrigationLevelID == 0)
                    {
                        List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryID(Convert.ToInt64(ViewState[VSSubCategoryID]), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, AssetItemIDs);
                        Dropdownlist.BindDropdownlist<List<object>>(ddlNameGrouped, lstAssetNames);
                    }
                    else
                    {
                        if (IrrigationLevelID == 1)
                        {
                            long? CircleID = new AuctionBLL().GetCircleIDByDivisionID((long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                            long ZoneID = new AuctionBLL().GetZoneIDByCircleID((long)CircleID);
                            List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryIDForZoneLevel(Convert.ToInt64(ViewState[VSSubCategoryID]), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, (long)CircleID, ZoneID, AssetItemIDs);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlNameGrouped, lstAssetNames);
                        }
                        else if (IrrigationLevelID == 2)
                        {
                            long? CircleID = new AuctionBLL().GetCircleIDByDivisionID((long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID);
                            List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryIDForCircleLevel(Convert.ToInt64(ViewState[VSSubCategoryID]), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, (long)CircleID, AssetItemIDs);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlNameGrouped, lstAssetNames);
                        }
                        else
                        {
                            List<object> lstAssetNames = new AuctionBLL().GetAssetsBySubCategoryID(Convert.ToInt64(ViewState[VSSubCategoryID]), (long)SessionManagerFacade.UserAssociatedLocations.IrrigationBoundryID, AssetItemIDs);
                            Dropdownlist.BindDropdownlist<List<object>>(ddlNameGrouped, lstAssetNames);
                        }
                    }


                    //List<object> lstAttributes = new AuctionBLL().GetAttributesBySubCategoryID(Convert.ToInt64(ViewState[VSSubCategoryID]));
                    //Dropdownlist.BindDropdownlist<List<object>>(ddlAttributeTypeGrouped, lstAttributes);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGroupAsset_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                long AuctionAssetID = Convert.ToInt64(GetDataKeyValue(gvGroupAsset, "AuctionAssetID", e.RowIndex));
                GridViewRow row = gvGroupAsset.Rows[e.RowIndex];
                #region "Datakeys"
                string AuctionAssetItemID = GetDataKeyValue(gvGroupAsset, "AuctionAssetItemID", e.RowIndex);
                string AssetItemID = GetDataKeyValue(gvGroupAsset, "AssetItemID", e.RowIndex);
                string AttributeTypeID = GetDataKeyValue(gvGroupAsset, "AttributeTypeID", e.RowIndex);
                string AttributeValue = GetDataKeyValue(gvGroupAsset, "AttributeValue", e.RowIndex);
                string LotQuantity = GetDataKeyValue(gvGroupAsset, "LotQuantity", e.RowIndex);
                string CreatedBy = GetDataKeyValue(gvGroupAsset, "CreatedBy", e.RowIndex);
                string CreatedDate = GetDataKeyValue(gvGroupAsset, "CreatedDate", e.RowIndex);
                #endregion

                #region "Controls"
                DropDownList ddlNameGrouped = (DropDownList)row.FindControl("ddlNameGrouped");
                DropDownList ddlAttributeTypeGrouped = (DropDownList)row.FindControl("ddlAttributeTypeGrouped");
                TextBox txtAttributeValueGrouped = (TextBox)row.FindControl("txtAttributeValueGrouped");

                #endregion

                AC_AuctionAssetItems mdlAssetItem = new AC_AuctionAssetItems();
                mdlAssetItem.AssetItemID = Convert.ToInt64(ddlNameGrouped.SelectedItem.Value);
                mdlAssetItem.AuctionAssetID = Convert.ToInt64(ViewState[VSAuctionAssetID]);
                mdlAssetItem.CreatedBy = (int)SessionManagerFacade.UserInformation.ID;
                mdlAssetItem.CreatedDate = DateTime.Now;
                if (ddlAttributeTypeGrouped.SelectedItem.Value == "0")
                {
                    mdlAssetItem.AssetQuantity = Convert.ToInt32(txtAttributeValueGrouped.Text);
                }
                else
                {
                    if (!string.IsNullOrEmpty(ddlAttributeTypeGrouped.SelectedItem.Value))
                    {
                        mdlAssetItem.AssetAttributeID = Convert.ToInt64(ddlAttributeTypeGrouped.SelectedItem.Value);
                    }

                }

                bool isSaved = new AuctionBLL().SaveGroupedAssetitem(mdlAssetItem);
                if (isSaved)
                {
                    gvGroupAsset.EditIndex = -1;
                    BindAuctionGroupedAssetsGridView();
                    Master.ShowMessage(Message.RecordSaved.Description);
                    // Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                }
                else
                {
                    Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGroupAsset_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long AuctionAssetItemID = Convert.ToInt64(GetDataKeyValue(gvGroupAsset, "AuctionAssetItemID", e.RowIndex));
                bool IsDeleted = new AuctionBLL().DeleteAuctionAssetItemByID(AuctionAssetItemID);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindAuctionGroupedAssetsGridView();
                }
                else
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvGroupAsset_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvGroupAsset.EditIndex = -1;
                BindAuctionGroupedAssetsGridView();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        public void BindAuctionGroupedAssetsGridView()
        {
            try
            {
                gvGroupAsset.DataSource = new AuctionBLL().GetAuctionAssetItemsByID(Convert.ToInt64(ViewState[VSAuctionAssetID]));
                gvGroupAsset.DataBind();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void ddlAttributeTypeGrouped_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            GetAttributeValue(sender, "ddlAttributeTypeGrouped", "txtAttributeValueGrouped");
        }
        protected void ddlNameGrouped_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OnAssetNameChanged(sender, "ddlNameGrouped", "ddlAttributeTypeGrouped", "txtAttributeValueGrouped");
        }
        protected void txtAttributeValueGrouped_OnTextChanged(object sender, EventArgs e)
        {
            TextBox DDLControl = (TextBox)sender;
            DropDownList ddlAssetName = (DropDownList)DDLControl.NamingContainer.FindControl("ddlNameGrouped");
            TextBox lblAttributeValue = (TextBox)DDLControl.NamingContainer.FindControl("txtAttributeValueGrouped");
            bool Isvalid = new AuctionBLL().CompareLotQuantity(Convert.ToInt32(lblAttributeValue.Text), Convert.ToInt64(ddlAssetName.SelectedItem.Value));
            if (!Isvalid)
            {
                lblAttributeValue.Text = "";
                Master.ShowMessage(Message.AssetQuantityNotValid.Description, SiteMaster.MessageType.Error);
            }
            //GetAssetNamesandAttributes(sender, "ddlSubCategory", "ddlName", "ddlAttributeType");
        }
        #endregion
        private string GetDataKeyValue(GridView _GridView, string _DataKeyName, int _RowIndex)
        {
            DataKey key = _GridView.DataKeys[_RowIndex];
            return Convert.ToString(key.Values[_DataKeyName]);
        }
        private object GetNewAuctionAsset()
        {
            object AuctionAsset = new
            {
                AuctionAssetID = 0,
                Level = string.Empty,
                LevelID = 0,
                GroupIndividual = string.Empty,
                Category = string.Empty,
                CategoryID = 0,
                SubCategory = string.Empty,
                SubCategoryID = 0,
                Name = string.Empty,
                NameID = 0,
                AttributeType = string.Empty,
                AttributeTypeID = 0,
                AttributeValue = string.Empty,
                CreatedBy = string.Empty,
                CreatedDate = string.Empty
            };
            return AuctionAsset;
        }
        private object GetNewGroupedAuctionAsset()
        {
            object GroupedAuctionAsset = new
            {
                AuctionAssetItemID = 0,
                AuctionAssetID = 0,
                AssetItemID = 0,
                AttributeTypeID = 0,
                AttributeValue = string.Empty,
                LotQuantity = 0,
                Name = string.Empty,
                AttributeType = string.Empty,
                CreatedBy = string.Empty,
                CreatedDate = string.Empty
            };
            return GroupedAuctionAsset;
        }
        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 14-07-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Auctions);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}