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
    public partial class District : BasePage
    {
        List<CO_District> lstDistrict = new List<CO_District>();
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

        protected void gvDistrict_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    lstDistrict = new DistrictBLL().GetAllDistricts(true);
                    CO_District mdlDistrict = new CO_District();

                    mdlDistrict.ID = 0;
                    mdlDistrict.Name = "";
                    mdlDistrict.Description = "";
                    lstDistrict.Add(mdlDistrict);

                    gvDistrict.PageIndex = gvDistrict.PageCount;
                    gvDistrict.DataSource = lstDistrict;
                    gvDistrict.DataBind();

                    gvDistrict.EditIndex = gvDistrict.Rows.Count - 1;
                    gvDistrict.DataBind();
                    gvDistrict.Rows[gvDistrict.Rows.Count - 1].FindControl("txtDistrictName").Focus();

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDistrict_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvDistrict.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDistrict_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                long DistrictID = Convert.ToInt32(((Label)gvDistrict.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string DistrictName = ((TextBox)gvDistrict.Rows[RowIndex].Cells[1].FindControl("txtDistrictName")).Text.Trim();
                string DistrictDescription = ((TextBox)gvDistrict.Rows[RowIndex].Cells[2].FindControl("txtDistrictDescription")).Text.Trim();

                DistrictBLL bllDistrict = new DistrictBLL();
                CO_District mdlSearchedDistrict = bllDistrict.GetDistrictByName(DistrictName);

                if (mdlSearchedDistrict != null && DistrictID != mdlSearchedDistrict.ID)
                {
                    Master.ShowMessage(Message.DistrictNameExists.Description, SiteMaster.MessageType.Error);
                    return;
                }
                CO_District mdlDistrict = new CO_District();

                mdlDistrict.ID = DistrictID;
                mdlDistrict.Name = DistrictName;
                mdlDistrict.Description = DistrictDescription;

                bool IsRecordSaved = false;

                if (DistrictID == 0)
                {
                    mdlDistrict.ProvinceID = Constants.PunjabProvinceID;
                    IsRecordSaved = bllDistrict.AddDistrict(mdlDistrict);
                }
                else
                {
                    IsRecordSaved = bllDistrict.UpdateDistrict(mdlDistrict);
                }
                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (DistrictID == 0)
                    {
                        gvDistrict.PageIndex = 0;
                    }
                    gvDistrict.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDistrict_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvDistrict.EditIndex = e.NewEditIndex;
                BindGrid();
                gvDistrict.Rows[e.NewEditIndex].FindControl("txtDistrictName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDistrict_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int DistrictID = Convert.ToInt32(((Label)gvDistrict.Rows[e.RowIndex].FindControl("lblID")).Text);
                DistrictBLL bllDistrict = new DistrictBLL();
                bool IsExist = bllDistrict.IsDistrictIDExists(DistrictID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllDistrict.DeleteDistrict(DistrictID);

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

        protected void gvDistrict_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDistrict.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 28-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.District);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function binds District to the grid
        /// Created on 28-10-2015
        /// </summary>
        private void BindGrid()
        {
            lstDistrict = new DistrictBLL().GetAllDistricts(true);
            gvDistrict.DataSource = lstDistrict;
            gvDistrict.DataBind();
        }

        protected void gvDistrict_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDistrict.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDistrict_RowCreated(object sender, GridViewRowEventArgs e)
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