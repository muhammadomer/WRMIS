using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.UserAdministration;
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
    public partial class Division : BasePage
    {
        List<CO_Division> lstDivision = new List<CO_Division>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindZoneDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 26-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Division);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 28-10-2015
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds circles to the circle dropdown
        /// Created on 28-10-2015
        /// </summary>
        /// <param name="_ZoneID"></param>
        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds divisions to the grid based on the selected Circle ID
        /// and shows the page according to the added record.
        /// Created on 28-10-2015
        /// </summary>
        /// <param name="_Name"></param>
        private void BindGrid(string _Name = "")
        {
            long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);
            lstDivision = new DivisionBLL().GetDivisionsByCircleIDAndDomainID(CircleID);

            #region Code for showing correct position of new inserted record in sorted grid
            //if (_Name != String.Empty)
            //{
            //    List<string> lstName = lstDivision.Select(z => z.Name).ToList();
            //    int itemIndex = lstName.IndexOf(_Name);

            //    gvDivision.PageIndex = itemIndex / gvDivision.PageSize;
            //}
            #endregion

            gvDivision.DataSource = lstDivision;
            gvDivision.DataBind();
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created on 28-10-2015
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _DivisionID)
        {
            DivisionBLL bllDivision = new DivisionBLL();

            bool IsExist = bllDivision.IsDivisionIDExists(_DivisionID);

            if (!IsExist)
            {
                long ZoneIrrigationLevelID = 3;

                IsExist = new UserAdministrationBLL().IsRecordExist(ZoneIrrigationLevelID, _DivisionID);
            }

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This function check whether data is valid for add/edit operation.
        /// Created on 28-10-2015
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <param name="_DivisionID"></param>
        /// <param name="_DivisionName"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _CircleID, long _DivisionID, string _DivisionName)
        {
            DivisionBLL bllDivision = new DivisionBLL();

            CO_Division mdlSearchedDivision = bllDivision.GetDivisionByName(_DivisionName, _CircleID);

            if (mdlSearchedDivision != null && _DivisionID != mdlSearchedDivision.ID)
            {
                Master.ShowMessage(Message.DivisionNameExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    ddlCircle.SelectedIndex = 0;
                    ddlCircle.Enabled = false;
                }
                else
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    BindCircleDropdown(ZoneID);
                    ddlCircle.Enabled = true;
                }

                gvDivision.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCircle.SelectedItem.Value == String.Empty)
                {
                    gvDivision.Visible = false;
                }
                else
                {
                    gvDivision.EditIndex = -1;
                    gvDivision.PageIndex = 0;

                    BindGrid();

                    gvDivision.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDivision.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvDivision.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    lstDivision = new DivisionBLL().GetDivisionsByCircleIDAndDomainID(CircleID);

                    CO_Division mdlDivision = new CO_Division();

                    mdlDivision.ID = 0;
                    mdlDivision.Name = "";
                    mdlDivision.Description = "";
                    lstDivision.Add(mdlDivision);

                    gvDivision.PageIndex = gvDivision.PageCount;
                    gvDivision.DataSource = lstDivision;
                    gvDivision.DataBind();

                    gvDivision.EditIndex = gvDivision.Rows.Count - 1;
                    gvDivision.DataBind();

                    gvDivision.Rows[gvDivision.Rows.Count - 1].FindControl("txtName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long DivisionID = Convert.ToInt64(((Label)gvDivision.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (!IsValidDelete(DivisionID))
                {
                    return;
                }

                DivisionBLL bllDivision = new DivisionBLL();

                bool IsDeleted = bllDivision.DeleteDivision(DivisionID);

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

        protected void gvDivision_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvDivision.EditIndex = e.NewEditIndex;

                BindGrid();

                gvDivision.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                long DivisionID = Convert.ToInt64(((Label)gvDivision.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string DivisionName = ((TextBox)gvDivision.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                long DomainID = Convert.ToInt64(((DropDownList)gvDivision.Rows[RowIndex].Cells[2].FindControl("ddlDomain")).SelectedItem.Value);
                string DivisionDescription = ((TextBox)gvDivision.Rows[RowIndex].Cells[3].FindControl("txtDesc")).Text.Trim();

                if (!IsValidAddEdit(CircleID, DivisionID, DivisionName))
                {
                    return;
                }

                CO_Division mdlDivision = new CO_Division();

                mdlDivision.ID = DivisionID;
                mdlDivision.Name = DivisionName;
                mdlDivision.Description = DivisionDescription;
                mdlDivision.DomainID = DomainID;
                mdlDivision.CircleID = CircleID;

                DivisionBLL bllDivision = new DivisionBLL();

                bool IsRecordSaved = false;

                if (DivisionID == 0)
                {
                    IsRecordSaved = bllDivision.AddDivision(mdlDivision);
                }
                else
                {
                    IsRecordSaved = bllDivision.UpdateDivision(mdlDivision);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (DivisionID == 0)
                    {
                        gvDivision.PageIndex = 0;
                    }

                    gvDivision.EditIndex = -1;
                    BindGrid(mdlDivision.Name);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvDivision.EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddlDomain = (DropDownList)e.Row.FindControl("ddlDomain");

                    if (ddlDomain != null)
                    {
                        Dropdownlist.DDLDomains(ddlDomain);

                        Label lblDomainID = (Label)e.Row.FindControl("lblDomainID");
                        Dropdownlist.SetSelectedValue(ddlDomain, lblDomainID.Text);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDivision_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDivision.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}