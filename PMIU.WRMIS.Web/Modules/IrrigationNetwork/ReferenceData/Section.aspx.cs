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
    public partial class Section : BasePage
    {
        List<CO_Section> lstSection = new List<CO_Section>();
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
        /// Created on 02-11-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Section);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 02-11-2015
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds circles to the circle dropdown
        /// Created on 02-11-2015
        /// </summary>
        /// <param name="_ZoneID"></param>
        private void BindCircleDropdown(long _ZoneID)
        {
            Dropdownlist.DDLCircles(ddlCircle, false, _ZoneID, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds divisions to the division dropdown
        /// Created on 02-11-2015
        /// </summary>
        /// <param name="_CircleID"></param>
        private void BindDivisionDropdown(long _CircleID)
        {
            Dropdownlist.DDLDivisions(ddlDivision, false, _CircleID, -1, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds sub divisions to the sub division dropdown
        /// Created on 02-11-2015
        /// </summary>
        /// <param name="_DivisionID"></param>
        private void BindSubDivisionDropdown(long _DivisionID)
        {
            Dropdownlist.DDLSubDivisions(ddlSubDivision, false, _DivisionID);
        }

        /// <summary>
        /// This function binds sections to the grid based on the selected Sub Division ID
        /// and shows the page according to the added record.
        /// Created on 02-11-2015
        /// </summary>
        /// <param name="_Name"></param>
        private void BindGrid(string _Name = "")
        {
            int SubDivisionID = Convert.ToInt32(ddlSubDivision.SelectedItem.Value);
            lstSection = new SectionBLL().GetSectionsBySubDivisionID(SubDivisionID);

            #region Code for showing correct position of new inserted record in sorted grid
            //if (_Name != String.Empty)
            //{
            //    List<string> lstName = lstSection.Select(z => z.Name).ToList();
            //    int itemIndex = lstName.IndexOf(_Name);

            //    gvSection.PageIndex = itemIndex / gvSection.PageSize;
            //}
            #endregion

            gvSection.DataSource = lstSection;
            gvSection.DataBind();
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created on 02-11-2015
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _SectionID)
        {
            SectionBLL bllSection = new SectionBLL();

            bool IsExist = bllSection.IsSectionIDExists(_SectionID);

            if (!IsExist)
            {
                long ZoneIrrigationLevelID = 5;

                IsExist = new UserAdministrationBLL().IsRecordExist(ZoneIrrigationLevelID, _SectionID);
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
        /// Created on 02-11-2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_SectionID"></param>
        /// <param name="_SectionName"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _SubDivisionID, long _SectionID, string _SectionName)
        {
            SectionBLL bllSection = new SectionBLL();

            CO_Section mdlSearchedSection = bllSection.GetSectionByName(_SectionName, _SubDivisionID);

            if (mdlSearchedSection != null && _SectionID != mdlSearchedSection.ID)
            {
                Master.ShowMessage(Message.SectionNameExists.Description, SiteMaster.MessageType.Error);

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

                ddlSubDivision.SelectedIndex = 0;
                ddlSubDivision.Enabled = false;

                gvSection.Visible = false;
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

                ddlSubDivision.SelectedIndex = 0;
                ddlSubDivision.Enabled = false;

                gvSection.Visible = false;
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
                    ddlSubDivision.SelectedIndex = 0;
                    ddlSubDivision.Enabled = false;
                }
                else
                {
                    long DivisionID = Convert.ToInt64(ddlDivision.SelectedItem.Value);

                    BindSubDivisionDropdown(DivisionID);
                    ddlSubDivision.Enabled = true;
                }

                gvSection.Visible = false;
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlSubDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSubDivision.SelectedItem.Value == String.Empty)
            {
                gvSection.Visible = false;
            }
            else
            {
                gvSection.EditIndex = -1;
                gvSection.PageIndex = 0;

                BindGrid();

                gvSection.Visible = true;
            }
        }

        protected void gvSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSection.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvSection.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);

                    lstSection = new SectionBLL().GetSectionsBySubDivisionID(SubDivisionID);

                    CO_Section mdlSection = new CO_Section();

                    mdlSection.ID = 0;
                    mdlSection.Name = "";
                    mdlSection.Description = "";
                    lstSection.Add(mdlSection);

                    gvSection.PageIndex = gvSection.PageCount;
                    gvSection.DataSource = lstSection;
                    gvSection.DataBind();

                    gvSection.EditIndex = gvSection.Rows.Count - 1;
                    gvSection.DataBind();

                    gvSection.Rows[gvSection.Rows.Count - 1].FindControl("txtName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long SectionID = Convert.ToInt64(((Label)gvSection.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (!IsValidDelete(SectionID))
                {
                    return;
                }

                SectionBLL bllSection = new SectionBLL();

                bool IsDeleted = bllSection.DeleteSection(SectionID);

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

        protected void gvSection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvSection.EditIndex = e.NewEditIndex;

                BindGrid();

                gvSection.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long SubDivisionID = Convert.ToInt64(ddlSubDivision.SelectedItem.Value);

                long SectionID = Convert.ToInt64(((Label)gvSection.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string SectionName = ((TextBox)gvSection.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string SectionDescription = ((TextBox)gvSection.Rows[RowIndex].Cells[2].FindControl("txtDesc")).Text.Trim();

                if (!IsValidAddEdit(SubDivisionID, SectionID, SectionName))
                {
                    return;
                }

                CO_Section mdlSection = new CO_Section();

                mdlSection.ID = SectionID;
                mdlSection.Name = SectionName;
                mdlSection.Description = SectionDescription;
                mdlSection.SubDivID = SubDivisionID;

                SectionBLL bllSection = new SectionBLL();

                bool IsRecordSaved = false;

                if (SectionID == 0)
                {
                    IsRecordSaved = bllSection.AddSection(mdlSection);
                }
                else
                {
                    IsRecordSaved = bllSection.UpdateSection(mdlSection);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (SectionID == 0)
                    {
                        gvSection.PageIndex = 0;
                    }

                    gvSection.EditIndex = -1;
                    BindGrid(mdlSection.Name);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvSection_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvSection.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}