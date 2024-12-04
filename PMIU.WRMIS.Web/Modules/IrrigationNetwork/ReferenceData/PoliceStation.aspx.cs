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
    public partial class PoliceStation : BasePage
    {
        List<CO_PoliceStation> lstPoliceStation = new List<CO_PoliceStation>();
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
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long DistrictID = ddlDistrict.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlDistrict.SelectedItem.Value);
                if (DistrictID == -1)
                {
                    ddlTehsil.SelectedIndex = -1;
                    ddlTehsil.Enabled = false;
                }
                else
                {
                    BindTehsilDropdown(DistrictID);
                    ddlTehsil.Enabled = true;
                }
                gvPoliceStation.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long TehsilID = ddlTehsil.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlTehsil.SelectedItem.Value);
                if (TehsilID == -1)
                {
                    gvPoliceStation.Visible = false;
                }
                else
                {
                    gvPoliceStation.EditIndex = -1;
                    BindGrid();
                    gvPoliceStation.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvPoliceStation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Add")
                {

                    long TehsilID = Convert.ToInt32(ddlTehsil.SelectedItem.Value);

                    lstPoliceStation = new PoliceStationBLL().GetPoliceStationsByTehsilID(TehsilID,true);

                    CO_PoliceStation mdlPoliceStation = new CO_PoliceStation();

                    mdlPoliceStation.ID = 0;
                    mdlPoliceStation.Name = "";
                    mdlPoliceStation.Description = "";
                    lstPoliceStation.Add(mdlPoliceStation);

                    gvPoliceStation.PageIndex = gvPoliceStation.PageCount;
                    gvPoliceStation.DataSource = lstPoliceStation;
                    gvPoliceStation.DataBind();

                    gvPoliceStation.EditIndex = gvPoliceStation.Rows.Count - 1;
                    gvPoliceStation.DataBind();
                    gvPoliceStation.Rows[gvPoliceStation.Rows.Count - 1].FindControl("txtPoliceStationName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvPoliceStation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvPoliceStation.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvPoliceStation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long TehsilID = Convert.ToInt32(ddlTehsil.SelectedItem.Value);

                long PoliceStationID = Convert.ToInt32(((Label)gvPoliceStation.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string PoliceStationName = ((TextBox)gvPoliceStation.Rows[RowIndex].Cells[1].FindControl("txtPoliceStationName")).Text.Trim();
                string PoliceStationDescription = ((TextBox)gvPoliceStation.Rows[RowIndex].Cells[2].FindControl("txtPoliceStationDesc")).Text.Trim();

                PoliceStationBLL bllPoliceStation = new PoliceStationBLL();

                CO_PoliceStation mdlSearchedPoliceStation = bllPoliceStation.GetPoliceStationByName(PoliceStationName, TehsilID);

                if (mdlSearchedPoliceStation != null && PoliceStationID != mdlSearchedPoliceStation.ID)
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                CO_PoliceStation mdlPoliceStation = new CO_PoliceStation();
                mdlPoliceStation.ID = PoliceStationID;
                mdlPoliceStation.Name = PoliceStationName;
                mdlPoliceStation.Description = PoliceStationDescription;
                mdlPoliceStation.TehsilID = TehsilID;

                bool IsRecordSaved = false;

                if (PoliceStationID == 0)
                {
                    IsRecordSaved = bllPoliceStation.AddPoliceStation(mdlPoliceStation);
                }
                else
                {
                    IsRecordSaved = bllPoliceStation.UpdatePoliceStation(mdlPoliceStation);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (PoliceStationID == 0)
                    {
                        gvPoliceStation.PageIndex = 0;
                    }
                    gvPoliceStation.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvPoliceStation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvPoliceStation.EditIndex = e.NewEditIndex;
                BindGrid();
                gvPoliceStation.Rows[e.NewEditIndex].FindControl("txtPoliceStationName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvPoliceStation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long PoliceStationID = Convert.ToInt32(((Label)gvPoliceStation.Rows[e.RowIndex].FindControl("lblID")).Text);
                PoliceStationBLL bllPoliceStation = new PoliceStationBLL();
                bool IsExist = bllPoliceStation.IsPoliceStationIDExists(PoliceStationID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllPoliceStation.DeletePoliceStation(PoliceStationID);

                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.Business);
            }
        }

        protected void gvPoliceStation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPoliceStation.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        // <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 30-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.PoliceStation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function binds district to the district dropdown
        /// Created On:30-10-2015
        /// </summary>
        private void BindDistrictDropdown()
        {
            Dropdownlist.DDLDistricts(ddlDistrict, false, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// this function binds Tehsil to the Tehsil Dropdown
        /// Created On:30-10-2015
        /// </summary>
        /// <param name="_DistrictId"></param>
        private void BindTehsilDropdown(long _DistrictID)
        {
            Dropdownlist.DDLTehsils(ddlTehsil, false, _DistrictID, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// this function binds police station to the grid based on the provided tehsil id
        /// Created On:30-10-2015
        /// </summary>
        private void BindGrid()
        {
            long TehsilID = Convert.ToInt32(ddlTehsil.SelectedItem.Value);

            lstPoliceStation = new PoliceStationBLL().GetPoliceStationsByTehsilID(TehsilID, true);
            gvPoliceStation.DataSource = lstPoliceStation;
            gvPoliceStation.DataBind();
        }

        protected void gvPoliceStation_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvPoliceStation.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPoliceStation_RowCreated(object sender, GridViewRowEventArgs e)
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