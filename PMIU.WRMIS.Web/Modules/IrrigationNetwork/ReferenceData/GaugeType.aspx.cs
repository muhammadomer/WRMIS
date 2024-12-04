using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData
{
    public partial class GaugeType : BasePage
    {
        List<CO_GaugeType> lstGaugeType = new List<CO_GaugeType>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPageTitle();
                BindGrid();
            }
        }

        /// <summary>
        /// this function set the page title and description text in the master file.
        /// Created On: 22-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.GaugeType);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvGaugeType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstGaugeType = new GaugeTypeBLL().GetAllGaugeTypes(true);
                    CO_GaugeType mdlGaugeType = new CO_GaugeType();

                    mdlGaugeType.ID = 0;
                    mdlGaugeType.Name = "";
                    mdlGaugeType.Description = "";
                    lstGaugeType.Add(mdlGaugeType);

                    gvGaugeType.PageIndex = gvGaugeType.PageCount;
                    gvGaugeType.DataSource = lstGaugeType;
                    gvGaugeType.DataBind();

                    gvGaugeType.EditIndex = gvGaugeType.Rows.Count - 1;
                    gvGaugeType.DataBind();
                    gvGaugeType.Rows[gvGaugeType.Rows.Count-1].FindControl("txtGaugeTypeName").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvGaugeType.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                long GaugeTypeID = Convert.ToInt32(((Label)gvGaugeType.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string GaugeTypeName = ((TextBox)gvGaugeType.Rows[RowIndex].Cells[1].FindControl("txtGaugeTypeName")).Text.Trim();
                string GaugeTypeDesc = ((TextBox)gvGaugeType.Rows[RowIndex].Cells[2].FindControl("txtGaugeTypeDesc")).Text.Trim();

                GaugeTypeBLL bllGaugeType = new GaugeTypeBLL();
                CO_GaugeType mdlSearchedGaugeType = bllGaugeType.GetGaugeTypeByName(GaugeTypeName);

                if (mdlSearchedGaugeType != null && GaugeTypeID != mdlSearchedGaugeType.ID)
                {
                    Master.ShowMessage(Message.GaugeTypeNameExists.Description, SiteMaster.MessageType.Error);
                    return;
                }

                CO_GaugeType mdlGaugeType = new CO_GaugeType();
                mdlGaugeType.ID = GaugeTypeID;
                mdlGaugeType.Name = GaugeTypeName;
                mdlGaugeType.Description = GaugeTypeDesc;

                bool IsRecordSaved = false;

                if (GaugeTypeID == 0)
                {
                    IsRecordSaved = bllGaugeType.AddGaugeType(mdlGaugeType);
                }
                else
                {
                    IsRecordSaved = bllGaugeType.UpdateGaugeType(mdlGaugeType);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (GaugeTypeID == 0)
                    {
                        gvGaugeType.PageIndex = 0;
                    }
                    gvGaugeType.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long GaugeTypeID = Convert.ToInt32(((Label)gvGaugeType.Rows[e.RowIndex].FindControl("lblID")).Text);

                GaugeTypeBLL bllGaugeType = new GaugeTypeBLL();

                bool IsExist = bllGaugeType.IsGaugeTypeIDExists(GaugeTypeID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllGaugeType.DeleteGaugeType(GaugeTypeID);
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

        protected void gvGaugeType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvGaugeType.EditIndex = e.NewEditIndex;
                BindGrid();
                gvGaugeType.Rows[e.NewEditIndex].FindControl("txtGaugeTypeName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvGaugeType.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function bind Gauge Types to the grid.
        /// Created On:22-10-2015
        /// </summary>
        private void BindGrid()
        {
            lstGaugeType = new GaugeTypeBLL().GetAllGaugeTypes(true);
            gvGaugeType.DataSource = lstGaugeType;
            gvGaugeType.DataBind();
        }

        protected void gvGaugeType_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvGaugeType.EditIndex = -1;
                BindGrid();
            }
            catch(Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvGaugeType_RowCreated(object sender, GridViewRowEventArgs e)
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