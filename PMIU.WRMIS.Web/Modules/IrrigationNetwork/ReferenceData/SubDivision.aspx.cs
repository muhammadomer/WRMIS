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
    public partial class SubDivision : BasePage
    {
        List<CO_SubDivision> lstSubDivision = new List<CO_SubDivision>();
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
        /// Created on 29-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SubDivision);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 29-10-2015
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds circles to the circle dropdown
        /// Created on 29-10-2015
        /// </summary>
        /// <param name="_ZoneID"></param>
        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds divisions to the division dropdown
        /// Created on 29-10-2015
        /// </summary>
        /// <param name="_CircleID"></param>
        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID, -1, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds sub divisions to the grid based on the selected Division ID
        /// and shows the page according to the added record.
        /// Created on 29-10-2015
        /// </summary>
        /// <param name="_Name"></param>
        private void BindGrid(string _Name = "")
        {
            long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);
            lstSubDivision = new SubDivisionBLL().GetSubDivisionsByDivisionID(DivisionID);

            #region Code for showing correct position of new inserted record in sorted grid
            //if (_Name != String.Empty)
            //{
            //    List<string> lstName = lstSubDivision.Select(z => z.Name).ToList();
            //    int itemIndex = lstName.IndexOf(_Name);

            //    gvSubDivision.PageIndex = itemIndex / gvSubDivision.PageSize;
            //}
            #endregion

            gvSubDivision.DataSource = lstSubDivision;
            gvSubDivision.DataBind();
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created on 29-10-2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _SubDivisionID)
        {
            SubDivisionBLL bllSubDivision = new SubDivisionBLL();

            bool IsExist = bllSubDivision.IsSubDivisionIDExists(_SubDivisionID);

            if (!IsExist)
            {
                long ZoneIrrigationLevelID = 4;

                IsExist = new UserAdministrationBLL().IsRecordExist(ZoneIrrigationLevelID, _SubDivisionID);
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
        /// Created on 29-10-2015
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_SubDivisionName"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _DivisionID, long _SubDivisionID, string _SubDivisionName)
        {
            SubDivisionBLL bllSubDivision = new SubDivisionBLL();

            CO_SubDivision mdlSearchedSubDivision = bllSubDivision.GetSubDivisionByName(_SubDivisionName, _DivisionID);

            if (mdlSearchedSubDivision != null && _SubDivisionID != mdlSearchedSubDivision.ID)
            {
                Master.ShowMessage(Message.SubDivisionNameExists.Description, SiteMaster.MessageType.Error);

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

                ddlDivision.SelectedIndex = 0;
                ddlDivision.Enabled = false;

                gvSubDivision.Visible = false;
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
                    ddlDivision.SelectedIndex = 0;
                    ddlDivision.Enabled = false;
                }
                else
                {
                    long CircleID = Convert.ToInt64(ddlCircle.SelectedItem.Value);

                    BindDivisionDropdown(CircleID);
                    ddlDivision.Enabled = true;
                }

                gvSubDivision.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDivision.SelectedItem.Value == String.Empty)
                {
                    gvSubDivision.Visible = false;
                }
                else
                {
                    gvSubDivision.EditIndex = -1;
                    gvSubDivision.PageIndex = 0;

                    BindGrid();

                    gvSubDivision.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSubDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSubDivision.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSubDivision_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvSubDivision.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSubDivision_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                    lstSubDivision = new SubDivisionBLL().GetSubDivisionsByDivisionID(DivisionID);

                    CO_SubDivision mdlSubDivision = new CO_SubDivision();

                    mdlSubDivision.ID = 0;
                    mdlSubDivision.Name = "";
                    mdlSubDivision.Description = "";
                    lstSubDivision.Add(mdlSubDivision);

                    gvSubDivision.PageIndex = gvSubDivision.PageCount;
                    gvSubDivision.DataSource = lstSubDivision;
                    gvSubDivision.DataBind();

                    gvSubDivision.EditIndex = gvSubDivision.Rows.Count - 1;
                    gvSubDivision.DataBind();

                    gvSubDivision.Rows[gvSubDivision.Rows.Count - 1].FindControl("txtName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSubDivision_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long SubDivisionID = Convert.ToInt64(((Label)gvSubDivision.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (!IsValidDelete(SubDivisionID))
                {
                    return;
                }

                SubDivisionBLL bllSubDivision = new SubDivisionBLL();

                bool IsDeleted = bllSubDivision.DeleteSubDivision(SubDivisionID);

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

        protected void gvSubDivision_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvSubDivision.EditIndex = e.NewEditIndex;

                BindGrid();

                gvSubDivision.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSubDivision_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                long SubDivisionID = Convert.ToInt64(((Label)gvSubDivision.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string SubDivisionName = ((TextBox)gvSubDivision.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string SubDivisionDescription = ((TextBox)gvSubDivision.Rows[RowIndex].Cells[2].FindControl("txtDesc")).Text.Trim();

                if (!IsValidAddEdit(DivisionID, SubDivisionID, SubDivisionName))
                {
                    return;
                }

                CO_SubDivision mdlSubDivision = new CO_SubDivision();

                mdlSubDivision.ID = SubDivisionID;
                mdlSubDivision.Name = SubDivisionName;
                mdlSubDivision.Description = SubDivisionDescription;
                mdlSubDivision.DivisionID = DivisionID;

                SubDivisionBLL bllSubDivision = new SubDivisionBLL();

                bool IsRecordSaved = false;

                if (SubDivisionID == 0)
                {
                    IsRecordSaved = bllSubDivision.AddSubDivision(mdlSubDivision);
                }
                else
                {
                    IsRecordSaved = bllSubDivision.UpdateSubDivision(mdlSubDivision);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (SubDivisionID == 0)
                    {
                        gvSubDivision.PageIndex = 0;
                    }

                    gvSubDivision.EditIndex = -1;
                    BindGrid(mdlSubDivision.Name);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSubDivision_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvSubDivision.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }
    }
}