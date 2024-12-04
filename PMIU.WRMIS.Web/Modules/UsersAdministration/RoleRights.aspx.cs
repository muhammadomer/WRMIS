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
using System.Data;
using System.Web.UI.HtmlControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class RoleRights : BasePage
    {
        #region ViewStates

        public string Disable = "Disable";
        public string Check_VS = "Check";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetTitle();
                    BindGrid();
                    ddlrModule.Enabled = false;
                }
            }
            catch (WRException exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        public void BindGrid()
        {
            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLRole(ddlRole, (int)Constants.DropDownFirstOption.Select);
            PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLGetModules(ddlrModule, (int)Constants.DropDownFirstOption.Select);

            //gvRoleRights.Visible = false;
            //btnCancel.Visible = false;
            //btnSave.Visible = false;

            //List<UA_Roles> lstRoles= new RoleRightsBLL().getAllRoles();
            //ddlRole.DataSource = lstRoles;
            //ddlRole.DataTextField = "Name";
            //ddlRole.DataValueField = "ID";
            //ddlRole.DataBind();

            //ddlrModule.DataSource = new RoleRightsBLL().getAllModules();
            //ddlrModule.DataTextField = "Name";
            //ddlrModule.DataValueField = "ID";
            //ddlrModule.DataBind();
        }

        public void BindRoleRightGrid()
        {
            int moduleID = Convert.ToInt32(ddlrModule.SelectedItem.Value);
            int roleID = Convert.ToInt32(ddlRole.SelectedItem.Value);
            string moduleName = ddlrModule.SelectedItem.Text.ToString();
            btnCancel.Visible = true;
            List<object> lstRoleRights = new RoleRightsBLL().GetModulePages(roleID, moduleID);

            //long LoggedUser = Convert.ToInt64(HttpContext.Current.Session[SessionValues.UserID]);
            //UA_RoleRights objRR = new RoleRightsBLL().CheckUserEditrights(LoggedUser, 45);  // 45 is the page id for RoleRights
            //if (objRR != null)
            //{
            if (base.CanEdit == false)
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
                ViewState[Disable] = 1;
            }
            else
            {
                btnSave.Visible = true;
                btnCancel.Visible = true;
            }
            //}

            if (lstRoleRights.Count() > 0)
            {
                bool Check = false;
                foreach (var rr in lstRoleRights)
                {

                    string Add = rr.GetType().GetProperty("BAdd").GetValue(rr).ToString();
                    string Edit = rr.GetType().GetProperty("BEdit").GetValue(rr).ToString();
                    string Delete = rr.GetType().GetProperty("BDelete").GetValue(rr).ToString();
                    string View = rr.GetType().GetProperty("BView").GetValue(rr).ToString();
                    string PageName = rr.GetType().GetProperty("Name").GetValue(rr).ToString();

                    if (Add == "False" || Edit == "False" || Delete == "False" || View == "False")
                    {
                        Check = false;
                        break;
                    }
                    else
                        Check = true;
                }

                ViewState[Check_VS] = Check;

                lstRoleRights.Insert(0, new
                {
                    ID = -1,
                    RoleID = -1,
                    PageID = -1,
                    BAdd = false,
                    BEdit = false,
                    BDelete = false,
                    BPrint = false,
                    BView = false,
                    BExport = false,
                    AddVisible = false,
                    EditVisible = false,
                    DeleteVisible = false,
                    PrintVisible = false,
                    ViewVisible = false,
                    ExportVisible = false,
                    Name = moduleName,
                    Description = "",
                    ParentID = -1
                });

            }


            gvRoleRights.DataSource = lstRoleRights;
            gvRoleRights.DataBind();
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState[Disable] = 0;
            if (ddlRole.SelectedItem.Text == "Administrator")
            {
                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLGetModules(ddlrModule, (int)Constants.DropDownFirstOption.Select, "Name", "ID");
            }
            else
            {
                PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLGetModules(ddlrModule, (int)Constants.DropDownFirstOption.Select, "Name", "ID", true);
            }

            gvRoleRights.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            if (ddlRole.SelectedIndex == 0)
            {
                ddlrModule.Enabled = false;
            }
            else
            {
                ddlrModule.Enabled = true;
            }
        }

        protected void ddlrModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLGetRole(ddlRole, "Name", "ID");
            //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLGetModules(ddlrModule, "Name", "ID");

            //int roleIndex = ddlRole.SelectedIndex;
            //int moduleIndex = ddlrModule.SelectedIndex;

            if (ddlrModule.SelectedIndex == 0)
            {
                gvRoleRights.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
            }
            else
            {
                BindRoleRightGrid();
                gvRoleRights.Visible = true;
                //btnSave.Visible = true; 
                //btnCancel.Visible = true;
            }


            //if (ddlRole.SelectedItem.Text != "Select")
            //{


            //    if (ddlrModule.SelectedItem.Text.ToString() != "Select")
            //    {
            //        BindRoleRightGrid();
            //        gvRoleRights.Visible = true;
            //        Master.HideMessageInstantly();
            //    }
            //}
            //else
            //{
            //    Master.ShowMessage(Message.SelectRole.Description, SiteMaster.MessageType.Warning);
            //}
        }

        private void SetTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.RoleRights);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        protected void gvRoleRights_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState[Check_VS] != null && ViewState[Check_VS].ToString() == "True")
                {
                    CheckBox CB = e.Row.FindControl("cbSelectAll") as CheckBox;
                    CB.Checked = true;
                }

                if (ViewState[Disable] != null && ViewState[Disable].ToString() == "1")
                {
                    CheckBox CB = e.Row.FindControl("cbSelectAll") as CheckBox;
                    CB.Enabled = false;

                    CB = e.Row.FindControl("AllAddCheckBox") as CheckBox;
                    CB.Enabled = false;

                    CB = e.Row.FindControl("AllEditCheckBox") as CheckBox;
                    CB.Enabled = false;

                    CB = e.Row.FindControl("AllDeleteCheckBox") as CheckBox;
                    CB.Enabled = false;

                    CB = e.Row.FindControl("AllViewCheckBox") as CheckBox;
                    CB.Enabled = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (ViewState[Disable] != null && ViewState[Disable].ToString() == "1")
                {
                    CheckBox CBAdd = e.Row.FindControl("cbAdd") as CheckBox;
                    CBAdd.Enabled = false;
                    CheckBox CBEdit = e.Row.FindControl("cbEdit") as CheckBox;
                    CBEdit.Enabled = false;
                    CheckBox CBDelete = e.Row.FindControl("cbDelete") as CheckBox;
                    CBDelete.Enabled = false;
                    CheckBox CBView = e.Row.FindControl("cbView") as CheckBox;
                    CBView.Enabled = false;
                }
            }



            Label id = (Label)e.Row.FindControl("lblID");
            if (id != null && id.Text == "-1")
            {
                e.Row.Font.Bold = true;
            }
        }

        protected void gvRoleRights_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindRoleRightGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool result = false;
            int RoleID = Convert.ToInt32(ddlRole.SelectedItem.Value);
            foreach (GridViewRow row in gvRoleRights.Rows)
            {
                Label id = (Label)(row.FindControl("lblID"));
                if (id.Text == "-1")
                {
                    continue;
                }

                CheckBox add = (CheckBox)row.FindControl("cbAdd");
                CheckBox edit = (CheckBox)row.FindControl("cbEdit");
                CheckBox delete = (CheckBox)row.FindControl("cbDelete");
                CheckBox view = (CheckBox)row.FindControl("cbView");
                int RoleRightID = Convert.ToInt32(((Label)row.Cells[0].FindControl("lblID")).Text);
                int PageID = Convert.ToInt32(((Label)row.Cells[1].FindControl("lblPageID")).Text);

                UA_RoleRights objSave = new UA_RoleRights();
                objSave.RoleID = RoleID;
                objSave.PageID = PageID;
                objSave.BAdd = add.Checked;
                objSave.BEdit = edit.Checked;
                objSave.BDelete = delete.Checked;
                objSave.BView = view.Checked;
                result = new RoleRightsBLL().updateRoleRight(RoleRightID, objSave);
            }

            if (result)
            {
                ddlRole.SelectedIndex = 0;
                ddlrModule.SelectedIndex = 0;
                ddlrModule.Enabled = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                gvRoleRights.Visible = false;
                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
            }
            //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLGetRole(ddlRole, "Name", "ID");
            //PMIU.WRMIS.Web.Common.Controls.Dropdownlist.DDLGetModules(ddlrModule, "Name", "ID");
            //gvRoleRights.Visible = false;
            //btnSave.Visible = false;
            //btnCancel.Visible = false;

        }

        #region Commented Code
        //protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    bool selectAll = ((CheckBox)sender).Checked;

        //    if (selectAll)
        //    {
        //        foreach (GridViewRow row in gvRoleRights.Rows)
        //        {
        //            CheckBox add = (CheckBox)row.FindControl("cbAdd");
        //            CheckBox edit = (CheckBox)row.FindControl("cbEdit");
        //            CheckBox delete = (CheckBox)row.FindControl("cbDelete");
        //            CheckBox view = (CheckBox)row.FindControl("cbView");

        //            add.Checked = true;
        //            edit.Checked = true;
        //            delete.Checked = true;
        //            view.Checked = true;
        //        }
        //    }
        //    else
        //    {
        //        foreach (GridViewRow row in gvRoleRights.Rows)
        //        {
        //            CheckBox add = (CheckBox)row.FindControl("cbAdd");
        //            CheckBox edit = (CheckBox)row.FindControl("cbEdit");
        //            CheckBox delete = (CheckBox)row.FindControl("cbDelete");
        //            CheckBox view = (CheckBox)row.FindControl("cbView");

        //            add.Checked = false;
        //            edit.Checked = false;
        //            delete.Checked = false;
        //            view.Checked = false;
        //        }
        //    }

        //}

        #endregion

        protected void gvRoleRights_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRoleRights.PageIndex = e.NewPageIndex;
                BindRoleRightGrid();
                //BindGrid();
                //gvRoleRights.Visible = true;
                //btnCancel.Visible = true;
                //btnSave.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRoleRights_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRoleRights.EditIndex = -1;
                BindRoleRightGrid();
                //BindGrid();
                //gvRoleRights.Visible = true;
                //btnCancel.Visible = true;
                //btnSave.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

    }
}