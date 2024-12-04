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
using PMIU.WRMIS.Web.Common.Controls;

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData
{
    public partial class Tehsil : BasePage
    {
        List<CO_Tehsil> lstTehsil = new List<CO_Tehsil>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindDistrictDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long DistrictID = ddlDistrict.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDistrict.SelectedItem.Value);

                if (DistrictID == -1)
                {
                    gvTehsil.Visible = false;
                }
                else
                {
                    gvTehsil.EditIndex = -1;
                    BindGrid();
                    gvTehsil.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }


        protected void gvTehsil_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long DistrictID = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
                    lstTehsil = new TehsilBLL().GetTehsilsByDistrictID(DistrictID, true);

                    CO_Tehsil mdlTehsil = new CO_Tehsil();


                    mdlTehsil.ID = 0;
                    mdlTehsil.Name = "";
                    mdlTehsil.Description = "";
                    lstTehsil.Add(mdlTehsil);

                    gvTehsil.PageIndex = gvTehsil.PageCount;
                    gvTehsil.DataSource = lstTehsil;
                    gvTehsil.DataBind();

                    gvTehsil.EditIndex = gvTehsil.Rows.Count - 1;
                    gvTehsil.DataBind();
                    gvTehsil.Rows[gvTehsil.Rows.Count - 1].FindControl("txtTehsilName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTehsil_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvTehsil.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTehsil_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                long DistrictID = Convert.ToInt32(ddlDistrict.SelectedItem.Value);

                long TehsilID = Convert.ToInt32(((Label)gvTehsil.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string TehsilName = ((TextBox)gvTehsil.Rows[RowIndex].Cells[1].FindControl("txtTehsilName")).Text.Trim();
                string Tehsildescription = ((TextBox)gvTehsil.Rows[RowIndex].Cells[2].FindControl("txtTehsilDesc")).Text.Trim();

                TehsilBLL bllTehsil = new TehsilBLL();
                CO_Tehsil mdlSearchedTehsil = bllTehsil.GetTehsilByName(TehsilName, DistrictID);

                if (mdlSearchedTehsil != null && TehsilID != mdlSearchedTehsil.ID)
                {
                    Master.ShowMessage(Message.TehsilNameExists.Description, SiteMaster.MessageType.Error);
                    return;
                }

                CO_Tehsil mdlTehsil = new CO_Tehsil();

                mdlTehsil.ID = TehsilID;
                mdlTehsil.Name = TehsilName;
                mdlTehsil.Description = Tehsildescription;
                mdlTehsil.DistrictID = DistrictID;

                bool IsRecordSaved = false;

                if (TehsilID == 0)
                {
                    IsRecordSaved = bllTehsil.AddTehsil(mdlTehsil);
                }
                else
                {
                    IsRecordSaved = bllTehsil.UpdateTehsil(mdlTehsil);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (TehsilID == 0)
                    {
                        gvTehsil.PageIndex = 0;
                    }
                    gvTehsil.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTehsil_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvTehsil.EditIndex = e.NewEditIndex;
                BindGrid();
                gvTehsil.Rows[e.NewEditIndex].FindControl("txtTehsilName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTehsil_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long TehsilID = Convert.ToInt32(((Label)gvTehsil.Rows[e.RowIndex].FindControl("lblID")).Text);
                TehsilBLL bllTehsil = new TehsilBLL();
                bool IsExist = bllTehsil.IsTehsilIDExists(TehsilID);
                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsDeleted = bllTehsil.DeleteTehsil(TehsilID);
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

        protected void gvTehsil_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTehsil.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created On:29-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Tehsil);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds Tehsil to the grid based on the provided District ID
        /// Created on 29-10-2015
        /// </summary>        
        private void BindGrid()
        {
            int DistrictID = Convert.ToInt32(ddlDistrict.SelectedItem.Value);

            lstTehsil = new TehsilBLL().GetTehsilsByDistrictID(DistrictID, true);
            gvTehsil.DataSource = lstTehsil;
            gvTehsil.DataBind();
        }

        /// <summary>
        /// this function binds District to the District Dropdown
        /// Created On:29-10-2015
        /// </summary>
        private void BindDistrictDropdown()
        {
            //Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
            Dropdownlist.DDLDistricts(ddlDistrict, false, (int)Constants.DropDownFirstOption.Select, true);
        }

        protected void gvTehsil_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvTehsil.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTehsil_RowCreated(object sender, GridViewRowEventArgs e)
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