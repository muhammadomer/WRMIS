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
    public partial class Village : BasePage
    {
        List<CO_Village> lstVillage = new List<CO_Village>();
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
                    ddlTehsil.SelectedIndex = -1;
                    ddlTehsil.Enabled = false;
                }
                else
                {
                    BindTehsilDropdown(DistrictID);
                    ddlTehsil.Enabled = true;
                }
                ddlPoliceStation.SelectedIndex = -1;
                ddlPoliceStation.Enabled = false;
                gvVillage.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long TehsilID = ddlTehsil.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlTehsil.SelectedItem.Value);

                if (TehsilID == -1)
                {
                    ddlPoliceStation.SelectedIndex = 0;
                    ddlPoliceStation.Enabled = false;
                }
                else
                {
                    BindPoliceStationDropdown(TehsilID);
                    ddlPoliceStation.Enabled = true;
                }
                gvVillage.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlPoliceStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long PoliceStationID = ddlPoliceStation.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlPoliceStation.SelectedItem.Value);

                if (PoliceStationID == -1)
                {
                    gvVillage.Visible = false;
                }
                else
                {
                    gvVillage.EditIndex = -1;
                    gvVillage.PageIndex = 0;
                    BindGrid();
                    gvVillage.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long PoliceStationID = Convert.ToInt32(ddlPoliceStation.SelectedItem.Value);
                    lstVillage = new VillageBLL().GetVillagesByPoliceStationID(PoliceStationID, true);
                    CO_Village mdlVillage = new CO_Village();

                    mdlVillage.ID = 0;
                    mdlVillage.Name = "";
                    mdlVillage.Description = "";
                    lstVillage.Add(mdlVillage);

                    gvVillage.PageIndex = gvVillage.PageCount;
                    gvVillage.DataSource = lstVillage;
                    gvVillage.DataBind();

                    gvVillage.EditIndex = gvVillage.Rows.Count - 1;
                    gvVillage.DataBind();
                    gvVillage.Rows[gvVillage.Rows.Count - 1].FindControl("txtVillageName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillage_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;
                long TehsilID = Convert.ToInt32(ddlTehsil.SelectedItem.Value);
                long PoliceStationID = Convert.ToInt32(ddlPoliceStation.SelectedItem.Value);
                long VillageID = Convert.ToInt32(((Label)gvVillage.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string VillageName = ((TextBox)gvVillage.Rows[RowIndex].Cells[1].FindControl("txtVillageName")).Text.Trim();
                string VillageDescription = ((TextBox)gvVillage.Rows[RowIndex].Cells[2].FindControl("txtVillageDesc")).Text.Trim();

                VillageBLL bllVillage = new VillageBLL();
                CO_Village mdlSearchedVillage = bllVillage.GetVillageByName(VillageName, PoliceStationID);

                if (mdlSearchedVillage != null && VillageID != mdlSearchedVillage.ID)
                {
                    Master.ShowMessage(Message.VillageNameExists.Description, SiteMaster.MessageType.Error);
                    return;
                }

                CO_Village mdlVillage = new CO_Village();

                mdlVillage.ID = VillageID;
                mdlVillage.Name = VillageName;
                mdlVillage.Description = VillageDescription;
                mdlVillage.PoliceStationID = PoliceStationID;
                mdlVillage.TehsilID = TehsilID;

                bool IsRecordSaved = false;

                if (VillageID == 0)
                {
                    IsRecordSaved = bllVillage.AddVillage(mdlVillage);
                }
                else
                {
                    IsRecordSaved = bllVillage.UpdateVillage(mdlVillage);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    if (VillageID == 0)
                    {
                        gvVillage.PageIndex = 0;
                    }
                    gvVillage.EditIndex = -1;
                    BindGrid();
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillage_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvVillage.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillage_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvVillage.EditIndex = e.NewEditIndex;
                BindGrid();
                gvVillage.Rows[e.NewEditIndex].FindControl("txtVillageName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillage_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long VillageID = Convert.ToInt32(((Label)gvVillage.Rows[e.RowIndex].FindControl("lblID")).Text);
                VillageBLL bllVillage = new VillageBLL();
                bool IsExist = bllVillage.IsVillageIDExists(VillageID);

                if (IsExist)
                {
                    Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsDeleted = bllVillage.DeleteVillage(VillageID);

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

        protected void gvVillage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvVillage.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 02-11-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Village);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// this function binds districts to the district dropdown
        /// Created On:02-11-2015
        /// </summary>
        private void BindDistrictDropdown()
        {
            Dropdownlist.DDLDistricts(ddlDistrict, false, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// this function binds Tehsil to Tehsil Dropdown
        /// Created On:02-11-2015
        /// </summary>
        /// <param name="_DistrictId"></param>
        private void BindTehsilDropdown(long _DistrictID)
        {
            Dropdownlist.DDLTehsils(ddlTehsil, false, _DistrictID, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// this function binds Police Station to Police Station Dropdown
        /// </summary>
        /// <param name="_TehsilId"></param>
        private void BindPoliceStationDropdown(long _TehsilID)
        {
            Dropdownlist.DDLPoliceStations(ddlPoliceStation, false, _TehsilID, (int)Constants.DropDownFirstOption.Select, true);
        }

        private void BindGrid()
        {
            long PoliceStationID = Convert.ToInt32(ddlPoliceStation.SelectedItem.Value);

            lstVillage = new VillageBLL().GetVillagesByPoliceStationID(PoliceStationID, true);
            gvVillage.DataSource = lstVillage;
            gvVillage.DataBind();
        }

        protected void gvVillage_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvVillage.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvVillage_RowCreated(object sender, GridViewRowEventArgs e)
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