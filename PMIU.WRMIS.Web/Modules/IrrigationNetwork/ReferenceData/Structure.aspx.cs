using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
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

namespace PMIU.WRMIS.Web.Modules.IrrigationNetwork.ReferenceData
{
    public partial class Structure : BasePage
    {
        List<CO_Station> lstStation = new List<CO_Station>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindProvinceDropdown();
                    BindRiverDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 03-11-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Structure);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds provinces to the province dropdown
        /// Created on 04-11-2015
        /// </summary>
        private void BindProvinceDropdown()
        {
            Dropdownlist.DDLProvince(ddlProvince, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds rivers to the river dropdown
        /// Created on 04-11-2015
        /// </summary>
        private void BindRiverDropdown()
        {
            Dropdownlist.DDLRiver(ddlRiver, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds structures to the grid
        /// and shows the page according to the added record.
        /// Created on 05-11-2015
        /// </summary>
        /// <param name="_Name"></param>
        private void BindGrid(string _Name = "")
        {
            long ProvinceID = Convert.ToInt64(ddlProvince.SelectedItem.Value);
            long RiverID = Convert.ToInt64(ddlRiver.SelectedItem.Value);

            lstStation = new StructureBLL().GetStations(ProvinceID, RiverID);

            #region Code for showing correct position of new inserted record in sorted grid
            //if (_Name != String.Empty)
            //{
            //    List<string> lstName = lstStation.Select(p => p.Name).ToList();
            //    int itemIndex = lstName.IndexOf(_Name);

            //    gvStructure.PageIndex = itemIndex / gvStructure.PageSize;
            //}
            #endregion

            gvStructure.DataSource = lstStation;
            gvStructure.DataBind();
        }

        /// <summary>
        /// This function check whether data is valid for add/edit operation.
        /// Created On 05-11-2015
        /// </summary>
        /// <param name="_StructureID"></param>
        /// <param name="_StructureName"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _StructureID, string _StructureName)
        {
            StructureBLL bllStructure = new StructureBLL();

            CO_Station mdlSearchedStation = bllStructure.GetStationByName(_StructureName);

            if (mdlSearchedStation != null && _StructureID != mdlSearchedStation.ID)
            {
                Master.ShowMessage(Message.StructureNameExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        protected void gvStructure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvStructure.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructure_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvStructure.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructure_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long ProvinceID = Convert.ToInt64(ddlProvince.SelectedItem.Value);
                    long RiverID = Convert.ToInt64(ddlRiver.SelectedItem.Value);

                    lstStation = new StructureBLL().GetStations(ProvinceID, RiverID);

                    CO_Station mdlStation = new CO_Station();

                    mdlStation.ID = 0;
                    mdlStation.Name = "";
                    mdlStation.StructureTypeID = 1;
                    lstStation.Add(mdlStation);

                    gvStructure.PageIndex = gvStructure.PageCount;
                    gvStructure.DataSource = lstStation;
                    gvStructure.DataBind();

                    gvStructure.EditIndex = gvStructure.Rows.Count - 1;
                    gvStructure.DataBind();

                    gvStructure.Rows[gvStructure.Rows.Count - 1].FindControl("txtName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructure_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long StructureID = Convert.ToInt64(((Label)gvStructure.Rows[e.RowIndex].FindControl("lblID")).Text);

                StructureBLL bllStructure = new StructureBLL();

                bool IsDeleted = bllStructure.DeleteStation(StructureID);

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

        protected void gvStructure_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvStructure.EditIndex = e.NewEditIndex;

                BindGrid();

                gvStructure.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructure_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long ProvinceID = Convert.ToInt64(ddlProvince.SelectedItem.Value);
                long RiverID = Convert.ToInt64(ddlRiver.SelectedItem.Value);

                long StructureID = Convert.ToInt64(((Label)gvStructure.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string StructureName = ((TextBox)gvStructure.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                long StructureTypeID = Convert.ToInt64(((DropDownList)gvStructure.Rows[RowIndex].Cells[2].FindControl("ddlStructureType")).SelectedItem.Value);

                if (!IsValidAddEdit(StructureID, StructureName))
                {
                    return;
                }

                CO_Station mdlStation = new CO_Station();

                mdlStation.ID = StructureID;
                mdlStation.Name = StructureName;
                mdlStation.StructureTypeID = StructureTypeID;
                mdlStation.ProvinceID = ProvinceID;
                mdlStation.RiverID = RiverID;

                StructureBLL bllStructure = new StructureBLL();

                bool IsRecordSaved = false;

                if (StructureID == 0)
                {
                    IsRecordSaved = bllStructure.AddStation(mdlStation);
                }
                else
                {
                    IsRecordSaved = bllStructure.UpdateStation(mdlStation);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (StructureID == 0)
                    {
                        gvStructure.PageIndex = 0;
                    }

                    gvStructure.EditIndex = -1;
                    BindGrid(mdlStation.Name);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructure_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvStructure.EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddlStructureType = (DropDownList)e.Row.FindControl("ddlStructureType");

                    if (ddlStructureType != null)
                    {
                        Dropdownlist.DDLStructureTypes(ddlStructureType, (int)Constants.DropDownFirstOption.NoOption);

                        Label lblStructureTypeID = (Label)e.Row.FindControl("lblStructureTypeID");
                        Dropdownlist.SetSelectedValue(ddlStructureType, lblStructureTypeID.Text);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                gvStructure.EditIndex = -1;
                gvStructure.PageIndex = 0;

                BindGrid();

                gvStructure.Visible = true;
                litGridTitle.Visible = true;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                gvStructure.Visible = false;
                litGridTitle.Visible = false;

                btnLoad_Click(null, null);

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlRiver_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvStructure.Visible = false;
                litGridTitle.Visible = false;

                btnLoad_Click(null, null);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvStructure_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvStructure.EditIndex = -1;

                BindGrid();

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}