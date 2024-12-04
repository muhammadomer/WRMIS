using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.BLL;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class Roles : BasePage
    {
        List<UA_Roles> lstRoles = new List<UA_Roles>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    BindGrid();
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public void BindGrid()
        {
            lstRoles = new RolesBLL().GetAllRoles();
            gvRoles.DataSource = lstRoles;
            gvRoles.DataBind();

        }


        protected void gvRoles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            bool dataSave = false;
            int rowIndex = e.RowIndex;
            int roleId = Convert.ToInt32(((Label)gvRoles.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
            string roleName = ((TextBox)gvRoles.Rows[rowIndex].Cells[1].FindControl("txtName")).Text.Trim();
            string roleDescription = ((TextBox)gvRoles.Rows[rowIndex].Cells[2].FindControl("txtDesc")).Text.Trim();

            if (roleName == "")
            {
                Master.ShowMessage(Message.RoleNameRequired.Description, SiteMaster.MessageType.Error);
            }
            else
            {
                UA_Roles roleObj = new UA_Roles();
                roleObj.ID = roleId;
                roleObj.Name = roleName;
                roleObj.Description = roleDescription;

                if (roleId == 0)
                {
                    bool roleExist = new RolesBLL().IsRoleExist(roleObj);
                    if (!roleExist)
                    {
                        dataSave = new RolesBLL().AddRole(roleObj);
                        if (dataSave)
                        {
                            gvRoles.PageIndex = 0;
                            gvRoles.EditIndex = -1;
                            BindGrid();
                            Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                        }
                    }
                    else
                    {
                        Master.ShowMessage(Message.RoleNameExists.Description, SiteMaster.MessageType.Error);
                    }
                }
                else
                {
                    bool exist = new RolesBLL().IsRoleExistEdit(roleObj);
                    if (!exist)
                    {
                        dataSave = new RolesBLL().UpdateRole(roleObj);
                        gvRoles.EditIndex = -1;
                        BindGrid();
                        Master.ShowMessage(Message.RoleSaved.Description, SiteMaster.MessageType.Success);
                    }
                    else
                    {
                        Master.ShowMessage(Message.RoleNameExists.Description, SiteMaster.MessageType.Error);
                    }
                }
            }

        }

        protected void gvRoles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRoles.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvRoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int RoleId = Convert.ToInt32(((Label)gvRoles.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                bool result = new RolesBLL().DeleteRole(RoleId);
                if (result)
                    Master.ShowMessage(Message.RoleDeleted.Description, SiteMaster.MessageType.Success);
                else
                    Master.ShowMessage(Message.RoleNotDeleted.Description, SiteMaster.MessageType.Error);

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRoles.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                lstRoles = new RolesBLL().GetAllRoles();
                UA_Roles RoleObj = new UA_Roles();

                RoleObj.ID = 0;
                RoleObj.Name = "";
                RoleObj.Description = "";
                lstRoles.Add(RoleObj);

                gvRoles.PageIndex = gvRoles.PageCount;
                gvRoles.DataSource = lstRoles;
                gvRoles.DataBind();

                gvRoles.EditIndex = gvRoles.Rows.Count - 1;
                gvRoles.DataBind();
                gvRoles.Rows[gvRoles.Rows.Count - 1].FindControl("txtName").Focus();
            }
        }

        protected void gvRoles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRoles.EditIndex = -1;
            lblMessage.Visible = false;
            BindGrid();
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Roles);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvRoles_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRoles.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRoles_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();

                if (mdlRoleRights != null)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        LinkButton btnAdd = (LinkButton)e.Row.FindControl("lbtnAdd");

                        if (btnAdd != null)
                        {
                            btnAdd.Visible = (bool)mdlRoleRights.BAdd;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

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