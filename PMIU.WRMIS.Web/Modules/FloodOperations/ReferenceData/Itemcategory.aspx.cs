using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations.ReferenceData;
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

namespace PMIU.WRMIS.Web.Modules.FloodOperations.ReferenceData
{
    public partial class Itemcategory : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindItemCategoryGridView();
                }
                SetPageTitle();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }


        #region "GridView Events"
        private void BindItemCategoryGridView()
        {
            try
            {
                List<object> lstItemCategory = new ItemsBLL().GetItemCategoryList();
                gvItemCategory.DataSource = lstItemCategory;
                gvItemCategory.DataBind();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItemCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "AddItemCategory")
                {
                    List<object> lstRepresentative = new ItemsBLL().GetItemCategoryList();

                    lstRepresentative.Add(
                    new
                    {
                        ID = 0,
                        Name = string.Empty,
                        Description = string.Empty,
                        IsActive = 1,
                        CreatedDate = DateTime.Now,
                        CreatedBy = string.Empty
                    });

                    gvItemCategory.PageIndex = gvItemCategory.PageCount;
                    gvItemCategory.DataSource = lstRepresentative;
                    gvItemCategory.DataBind();

                    gvItemCategory.EditIndex = gvItemCategory.Rows.Count - 1;
                    gvItemCategory.DataBind();

                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItemCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header) // If header created
                {
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    Button btnAddItemCategory = (Button)e.Row.FindControl("btnAddItemCategory");
                    if (mdlRoleRights != null)
                        btnAddItemCategory.Enabled = (bool)mdlRoleRights.BAdd;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    Button btnEditItemCategory = (Button)e.Row.FindControl("btnEditItemCategory");
                    Button btnDeleteItemCategory = (Button)e.Row.FindControl("btnDeleteItemCategory");
                    if (btnEditItemCategory != null)
                        btnEditItemCategory.Enabled = (bool)mdlRoleRights.BEdit;
                    if (btnDeleteItemCategory != null)
                        btnDeleteItemCategory.Enabled = (bool)mdlRoleRights.BDelete;

                    if (gvItemCategory.EditIndex == e.Row.RowIndex)
                    {
                        //if (Convert.ToInt16(hdnDependanceExists.Value) == 0 && Convert.ToInt16(hdnIsGaugesCalculated.Value) == 1)
                        //AddEditConfirmMessage(e);

                        #region "Data Keys"
                        DataKey key = gvItemCategory.DataKeys[e.Row.RowIndex];
                        string name = Convert.ToString(key.Values["Name"]);
                        string description = Convert.ToString(key.Values["Description"]);
                        #endregion

                        #region "Controls"
                        TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                        TextBox txtDetail = (TextBox)e.Row.FindControl("txtDetail");
                        TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");
                        #endregion

                        if (txtName != null)
                        {
                            if (!string.IsNullOrEmpty(name))
                                txtName.Text = name;
                        }

                        if (txtDescription != null)
                        {
                            if (!string.IsNullOrEmpty(description))
                                txtDescription.Text = description;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItemCategory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvItemCategory.EditIndex = e.NewEditIndex;
                BindItemCategoryGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItemCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvItemCategory.EditIndex = -1;
                BindItemCategoryGridView();
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItemCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                GridViewRow row = gvItemCategory.Rows[e.RowIndex];

                #region "Controls"
                TextBox txtName = (TextBox)row.FindControl("txtName");
                TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                #endregion

                #region "Datakeys"
                DataKey key = gvItemCategory.DataKeys[e.RowIndex];
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                string itemCategoryID = Convert.ToString(gvItemCategory.DataKeys[e.RowIndex].Values[0]);

                FO_ItemCategory itemCategory = new FO_ItemCategory();

                itemCategory.ID = Convert.ToInt16(itemCategoryID);

                if (txtName != null)
                    itemCategory.Name = txtName.Text;

                if (txtDescription != null && txtDescription.Text != "")
                    itemCategory.Description = txtDescription.Text;

                if (new ItemsBLL().IsItemCategoryNameExists(itemCategory))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (itemCategory.ID == 0)
                {
                    itemCategory.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    itemCategory.CreatedDate = DateTime.Now;
                }
                else
                {
                    itemCategory.CreatedBy = Convert.ToInt32(CreatedBy);
                    itemCategory.CreatedDate = Convert.ToDateTime(CreatedDate);
                    itemCategory.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    itemCategory.ModifiedDate = DateTime.Now;
                }

                itemCategory.IsActive = true;

                bool IsSave = new ItemsBLL().SaveItemCategory(itemCategory);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(itemCategoryID) == 0)
                        gvItemCategory.PageIndex = 0;

                    gvItemCategory.EditIndex = -1;
                    BindItemCategoryGridView();
                    Master.ShowMessage(Message.RecordSaved.Description);
                }

            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItemCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string itemCategoryID = Convert.ToString(gvItemCategory.DataKeys[e.RowIndex].Values[0]);

                if (!IsValidDelete(Convert.ToInt64(itemCategoryID)))
                {
                    return;
                }

                bool IsDeleted = new ItemsBLL().DeleteItemCategory(Convert.ToInt64(itemCategoryID));
                if (IsDeleted)
                {
                    BindItemCategoryGridView();
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        protected void gvItemCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvItemCategory.PageIndex = e.NewPageIndex;
                gvItemCategory.EditIndex = -1;
                BindItemCategoryGridView();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        #endregion "End GridView Events"

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteItemCategory");

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

        private bool IsValidDelete(long _ItemCategoryID)
        {
            ItemsBLL bllItems = new ItemsBLL();

            bool IsExist = bllItems.IsItemCategoryIDExists(_ItemCategoryID);

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

    }

}