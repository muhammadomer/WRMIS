using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData
{
    public partial class OutletType : BasePage
    {
        List<CO_OutletType> lstOutletType = new List<CO_OutletType>();
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

        protected void gvOutletType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstOutletType = new OutletTypeBLL().GetAllOutletTypes(true);
                    CO_OutletType mdlOutletType = new CO_OutletType();

                    mdlOutletType.ID = 0;
                    mdlOutletType.Name = "";
                    mdlOutletType.Description = "";
                    lstOutletType.Add(mdlOutletType);

                    gvOutletType.PageIndex = gvOutletType.PageCount;
                    gvOutletType.DataSource = lstOutletType;
                    gvOutletType.DataBind();

                    gvOutletType.EditIndex = gvOutletType.Rows.Count - 1;
                    gvOutletType.DataBind();
                    gvOutletType.Rows[gvOutletType.Rows.Count - 1].FindControl("txtOutletTypeName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutletType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvOutletType.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvOutletType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                long OutletTypeID = Convert.ToInt32(((Label)gvOutletType.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string OutletTypeName = ((TextBox)gvOutletType.Rows[RowIndex].Cells[1].FindControl("txtOutletTypeName")).Text.Trim();
                string OutletTypeDescription = ((TextBox)gvOutletType.Rows[RowIndex].Cells[2].FindControl("txtOutletTypeDesc")).Text.Trim();

                OutletTypeBLL bllOutletType = new OutletTypeBLL();

                if (!CheckValidations(OutletTypeID, OutletTypeName, OutletTypeDescription))
                {
                    return;
                }

                CO_OutletType mdlOutletType = new CO_OutletType();

                mdlOutletType.ID = OutletTypeID;
                mdlOutletType.Name = OutletTypeName;
                mdlOutletType.Description = OutletTypeDescription;
                //line to be deleted after db updation
                mdlOutletType.Abbrivation = "";
                bool IsRecordSaved = false;

                if (OutletTypeID == 0)
                {
                    IsRecordSaved = bllOutletType.AddOutletType(mdlOutletType);
                }
                else
                {
                    IsRecordSaved = bllOutletType.UpdateOutletType(mdlOutletType);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (OutletTypeID == 0)
                    {
                        gvOutletType.PageIndex = 0;
                    }
                    gvOutletType.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvOutletType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long OutletTypeID = Convert.ToInt32(((Label)gvOutletType.Rows[e.RowIndex].FindControl("lblID")).Text);
                OutletTypeBLL bllOutletType = new OutletTypeBLL();
                bool IsExist = bllOutletType.IsOutletTypeIDExists(OutletTypeID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllOutletType.DeleteOutletType(OutletTypeID);
                if (IsDeleted)
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

        protected void gvOutletType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvOutletType.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutletType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvOutletType.EditIndex = e.NewEditIndex;
                BindGrid();
                gvOutletType.Rows[e.NewEditIndex].FindControl("txtOutletTypeName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function binds Outlet Types to the Grid
        /// Created On:26-10-2015
        /// </summary>
        private void BindGrid()
        {
            lstOutletType = new OutletTypeBLL().GetAllOutletTypes(true);
            gvOutletType.DataSource = lstOutletType;
            gvOutletType.DataBind();
        }

        /// <summary>
        /// this function set the page title and description text in the master file.
        /// Created On:26-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.OutletType);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function checks all required validations
        /// Created On:28-10-2015
        /// </summary>
        /// <param name="_OutletTypeID"></param>
        /// <param name="_OutletTypeName"></param>
        /// <param name="_OutletTypeDescription"></param>
        /// <returns>bool</returns>
        private bool CheckValidations(long _OutletTypeID, string _OutletTypeName, string _OutletTypeDescription)
        {
            OutletTypeBLL bllOutletType = new OutletTypeBLL();

            CO_OutletType mdlSearchedOutletTypeByName = bllOutletType.GetOutletTypeByName(_OutletTypeName);

            if (mdlSearchedOutletTypeByName != null && _OutletTypeID != mdlSearchedOutletTypeByName.ID)
            {
                Master.ShowMessage(Message.OutletTypeExists.Description, SiteMaster.MessageType.Error);
                return false;
            }

            CO_OutletType mdlSearchedOutletTypeByDescription = bllOutletType.GetOutletTypeByDescription(_OutletTypeDescription);

            if (mdlSearchedOutletTypeByDescription != null && _OutletTypeID != mdlSearchedOutletTypeByDescription.ID)
            {
                Master.ShowMessage(Message.OutletTypeDescriptionExists.Description, SiteMaster.MessageType.Error);
                return false;
            }

            return true;
        }

        protected void gvOutletType_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvOutletType.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOutletType_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        Button btnAdd = (Button)e.Row.FindControl("btnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                        if (btnEdit != null)
                        {
                            btnEdit.Visible = (bool)mdlRoleRights.BEdit;
                        }

                        if (btnDelete != null)
                        {
                            btnDelete.Visible = (bool)mdlRoleRights.BDelete;
                        }
                    }
                }


            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}