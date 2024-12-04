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
    public partial class Circle : BasePage
    {
        List<CO_Circle> lstCircle = new List<CO_Circle>();
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
        /// Created on 22-10-2015
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Circle);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds zones to the zone dropdown
        /// Created on 22-10-2015
        /// </summary>
        private void BindZoneDropdown()
        {
            Dropdownlist.DDLZones(ddlZone, false, (int)Constants.DropDownFirstOption.Select, true);
        }

        /// <summary>
        /// This function binds circles to the grid based on the selected Zone ID
        /// and shows the page according to the added record.
        /// Created on 22-10-2015
        /// </summary>
        /// <param name="_Name"></param>
        private void BindGrid(string _Name = "")
        {
            long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);
            lstCircle = new CircleBLL().GetCirclesByZoneID(ZoneID);

            #region Code for showing correct position of new inserted record in sorted grid
            //if (_Name != String.Empty)
            //{
            //    List<string> lstName = lstCircle.Select(z => z.Name).ToList();
            //    int itemIndex = lstName.IndexOf(_Name);

            //    gvCircle.PageIndex = itemIndex / gvCircle.PageSize;
            //}
            #endregion

            gvCircle.DataSource = lstCircle;
            gvCircle.DataBind();
        }

        /// <summary>
        /// This function check whether data is valid for delete operation.
        /// Created on 22-10-2015
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _CircleID)
        {
            CircleBLL bllCircle = new CircleBLL();

            bool IsExist = bllCircle.IsCircleIDExists(_CircleID);

            if (!IsExist)
            {
                long ZoneIrrigationLevelID = 2;

                IsExist = new UserAdministrationBLL().IsRecordExist(ZoneIrrigationLevelID, _CircleID);
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
        /// Created on 22-10-2015
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <param name="_CircleID"></param>
        /// <param name="_CircleName"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _ZoneID, long _CircleID, string _CircleName)
        {
            CircleBLL bllCircle = new CircleBLL();

            CO_Circle mdlSearchedCircle = bllCircle.GetCircleByName(_CircleName, _ZoneID);

            if (mdlSearchedCircle != null && _CircleID != mdlSearchedCircle.ID)
            {
                Master.ShowMessage(Message.CircleNameExists.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }

        protected void gvCircle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCircle.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCircle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCircle.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCircle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                    lstCircle = new CircleBLL().GetCirclesByZoneID(ZoneID);

                    CO_Circle mdlCircle = new CO_Circle();

                    mdlCircle.ID = 0;
                    mdlCircle.Name = "";
                    mdlCircle.Description = "";
                    lstCircle.Add(mdlCircle);

                    gvCircle.PageIndex = gvCircle.PageCount;
                    gvCircle.DataSource = lstCircle;
                    gvCircle.DataBind();

                    gvCircle.EditIndex = gvCircle.Rows.Count - 1;
                    gvCircle.DataBind();

                    gvCircle.Rows[gvCircle.Rows.Count - 1].FindControl("txtName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCircle_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long CircleID = Convert.ToInt64(((Label)gvCircle.Rows[e.RowIndex].FindControl("lblID")).Text);

                if (!IsValidDelete(CircleID))
                {
                    return;
                }

                CircleBLL bllCircle = new CircleBLL();

                bool IsDeleted = bllCircle.DeleteCircle(CircleID);

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

        protected void gvCircle_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvCircle.EditIndex = e.NewEditIndex;

                BindGrid();

                gvCircle.Rows[e.NewEditIndex].FindControl("txtName").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCircle_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                long ZoneID = Convert.ToInt64(ddlZone.SelectedItem.Value);

                long CircleID = Convert.ToInt64(((Label)gvCircle.Rows[RowIndex].Cells[0].FindControl("lblID")).Text);
                string CircleName = ((TextBox)gvCircle.Rows[RowIndex].Cells[1].FindControl("txtName")).Text.Trim();
                string CircleDescription = ((TextBox)gvCircle.Rows[RowIndex].Cells[2].FindControl("txtDesc")).Text.Trim();

                if (!IsValidAddEdit(ZoneID, CircleID, CircleName))
                {
                    return;
                }

                CO_Circle mdlCircle = new CO_Circle();

                mdlCircle.ID = CircleID;
                mdlCircle.Name = CircleName;
                mdlCircle.Description = CircleDescription;
                mdlCircle.ZoneID = ZoneID;

                CircleBLL bllCircle = new CircleBLL();

                bool IsRecordSaved = false;

                if (CircleID == 0)
                {
                    IsRecordSaved = bllCircle.AddCircle(mdlCircle);
                }
                else
                {
                    IsRecordSaved = bllCircle.UpdateCircle(mdlCircle);
                }

                if (IsRecordSaved)
                {
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                    if (CircleID == 0)
                    {
                        gvCircle.PageIndex = 0;
                    }

                    gvCircle.EditIndex = -1;
                    BindGrid(mdlCircle.Name);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlZone.SelectedItem.Value == String.Empty)
                {
                    gvCircle.Visible = false;
                }
                else
                {
                    gvCircle.EditIndex = -1;
                    gvCircle.PageIndex = 0;

                    BindGrid();

                    gvCircle.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvCircle_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvCircle.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}