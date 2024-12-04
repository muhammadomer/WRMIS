using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations.ReferenceData;
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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.ReferenceData
{
    public partial class Items : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    LoadItemCategory();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        //private bool IsGaugeInformationExists()
        //{
        //  return new ChannelBLL().IsGaugeInformationExists(Convert.ToInt64(hdnChannelID.Value));

        //}
        //private bool IsDependanceExists()
        //{
        //  return new ChannelBLL().IsItemsDependencyExists(Convert.ToInt64(hdnChannelID.Value));
        //}
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void LoadItemCategory()
        {
            try
            {
                Dropdownlist.DDLItemCategoryWithOutAsset(ddlItemCategory, false);
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteItems");

            //if (btnDelete != null && Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
            //{
            //  btnDelete.OnClientClick = "if (confirm('Are you sure you want to delete this record?')) {return confirm('All data would be deleted.')} else return false;";
            //}
            //else 
            if (btnDelete != null)//&& Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 0)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }

        }
        private void AddEditConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnEdit = (Button)_e.Row.FindControl("btnSaveItems");

            if (btnEdit != null)//&& Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
            {
                btnEdit.OnClientClick = "return confirm('All Data will be deleted and Gauge will be recalculate. Do you want to proced?');";
            }
        }
        #region "Items"

        #region "GridView Events"
        private void BindItemsGridView(long _ItemCategoryID)
        {
            try
            {
                List<object> lstItems = new ItemsBLL().GetItemsList(_ItemCategoryID);
                gvItems.DataSource = lstItems;
                gvItems.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "AddItems")
                {
                    List<object> lstItems = new ItemsBLL().GetItemsList(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));

                    lstItems.Add(
                    new
                    {
                        ID = 0,
                        Name = string.Empty,
                        Description = string.Empty,
                        MajorMinor = string.Empty,
                        ItemCategoryID = 0,
                        IsActive = 1,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvItems.PageIndex = gvItems.PageCount;
                    gvItems.DataSource = lstItems;
                    gvItems.DataBind();

                    gvItems.EditIndex = gvItems.Rows.Count - 1;
                    gvItems.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header) // If header created
                {
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    Button btnAddItems = (Button)e.Row.FindControl("btnAddItems");
                    if (mdlRoleRights != null)
                        btnAddItems.Enabled = (bool)mdlRoleRights.BAdd;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    Button btnEditItems = (Button)e.Row.FindControl("btnEditItems");
                    Button btnDeleteItems = (Button)e.Row.FindControl("btnDeleteItems");
                    if (btnEditItems != null)
                        btnEditItems.Enabled = (bool)mdlRoleRights.BEdit;
                    if (btnDeleteItems != null)
                        btnDeleteItems.Enabled = (bool)mdlRoleRights.BDelete;

                    if (gvItems.EditIndex == e.Row.RowIndex)
                    {
                        //if (Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
                        //AddEditConfirmMessage(e);

                        #region "Data Keys"
                        DataKey key = gvItems.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string ItemCategoryID = Convert.ToString(key.Values["ItemCategoryID"]);
                        string Name = Convert.ToString(key.Values["Name"]);
                        string Description = Convert.ToString(key.Values["Description"]);
                        string MajorMinor = Convert.ToString(key.Values["MajorMinor"]);
                        #endregion

                        #region "Controls"
                        //  DropDownList ddlMajorMinor = (DropDownList)e.Row.FindControl("ddlMajorMinor");

                        TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                        //TextBox txtDescription = (TextBox)e.Row.FindControl("Description");
                        //  Label lblMajorMinor = (Label)e.Row.FindControl("lblMajorMinor");
                        #endregion

                        //if (ddlMajorMinor != null)
                        //{
                        //  Dropdownlist.DDLMajorMinor(ddlMajorMinor);
                        //  if (!string.IsNullOrEmpty(MajorMinor))
                        //    Dropdownlist.SetSelectedValue(ddlMajorMinor, MajorMinor);
                        //}

                        if (txtName != null)
                            txtName.Text = Name;

                        //if (txtDescription != null)
                        //  txtDescription.Text = Description;

                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvItems.EditIndex = e.NewEditIndex;
                BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvItems.EditIndex = -1;
                BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvItems.Rows[e.RowIndex];

                DropDownList ddlMajorMinor = (DropDownList)row.FindControl("ddlMajorMinor");

                TextBox txtName = (TextBox)row.FindControl("txtName");

                #region "Datakeys"
                DataKey key = gvItems.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                string itemsID = Convert.ToString(gvItems.DataKeys[e.RowIndex].Values["ID"]);

                FO_Items items = new FO_Items();

                items.ID = Convert.ToInt64(itemsID);

                //if (ddlMajorMinor != null)
                //  items.MajorMinor = ddlMajorMinor.SelectedItem.Value;

                if (txtName != null)
                    items.Name = txtName.Text;

                items.ItemCategoryID = Convert.ToInt16(ddlItemCategory.SelectedItem.Value);

                if (new ItemsBLL().IsItemsNameExists(items))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (items.ID == 0)
                {
                    items.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    items.CreatedDate = DateTime.Now;
                }
                else
                {
                    items.CreatedBy = Convert.ToInt32(CreatedBy);
                    items.CreatedDate = Convert.ToDateTime(CreatedDate);
                    items.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    items.ModifiedDate = DateTime.Now;
                }

                items.IsActive = true;

                bool IsSave = new ItemsBLL().SaveItems(items);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(itemsID) == 0)
                        gvItems.PageIndex = 0;

                    gvItems.EditIndex = -1;
                    BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string itemsID = Convert.ToString(gvItems.DataKeys[e.RowIndex].Values[0]);

                if (!IsValidDelete(Convert.ToInt64(itemsID)))
                {
                    return;
                }

                bool IsDeleted = new ItemsBLL().DeleteItems(Convert.ToInt64(itemsID));
                if (IsDeleted)
                {
                    BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvItems.PageIndex = e.NewPageIndex;
                gvItems.EditIndex = -1;
                BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion "End GridView Events"

        #region "Dropdownlists Events"

        protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlItemCategory.SelectedItem.Value == "")
                    //BindItemsGridView(0);
                    gvItems.Visible = false;
                else
                {
                    gvItems.Visible = true;
                    BindItemsGridView(Convert.ToInt64(ddlItemCategory.SelectedItem.Value));
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }

        }
        #endregion "End Dropdownlists Events"

        private bool IsValidDelete(long _ItemsID)
        {
            ItemsBLL bllItems = new ItemsBLL();

            bool IsExist = bllItems.IsItemsIDExists(_ItemsID);

            //To do Wajahat
            ////if (!IsExist)
            ////{
            ////  long ZoneIrrigationLevelID = 4;

            ////  IsExist = new UserAdministrationBLL().IsRecordExist(ZoneIrrigationLevelID, _InfrastructureID);
            ////}

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        #endregion "End Items"
    }
}