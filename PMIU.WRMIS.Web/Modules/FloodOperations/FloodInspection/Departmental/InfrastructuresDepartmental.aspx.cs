using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using PMIU.WRMIS.Web.Modules.FloodOperations.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FloodInspection.Departmental
{
    public partial class InfrastructuresDepartmental : BasePage
    {
        #region Initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long _FloodInspectionID = Utility.GetNumericValueFromQueryString("FloodInspectionID", 0);

                    if (_FloodInspectionID > 0)
                    {
                        DepartmentalInspectionDetail1.FloodInspectionIDProp = _FloodInspectionID;
                        hdnFloodInspectionsID.Value = Convert.ToString(_FloodInspectionID);
                        hdnInfrastructureType.Value = new FloodInspectionsBLL().GetInfrastructureType(_FloodInspectionID).ToString();
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(_FloodInspectionID).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/Departmental/SearchDepartmental.aspx?FloodInspectionID={0}", _FloodInspectionID);
                        BindInfrastructuresGrid(_FloodInspectionID);
                    }
                    //   hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/Departmental/SearchDepartmental.aspx?ShowHistory=true");
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion

        #region Functions

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindInfrastructuresGrid(long _FloodInspectionID)
        {
            try
            {
                bool CanEditDep = false;
                int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
                List<object> lstInfrastructures = new FloodInspectionsBLL().GetInfrastructuresByFloodInspectionID(_FloodInspectionID);

                gvInfrastructures.DataSource = lstInfrastructures;
                gvInfrastructures.DataBind();
                gvInfrastructures.Visible = true;

                Button btn = (Button)gvInfrastructures.HeaderRow.FindControl("btnAddInfrastructures");
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                {
                    btn.Enabled = false;
                    DisableEditDeleteColumn(gvInfrastructures);
                }
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 1)
                {
                    CanEditDep = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, 1);
                    if (CanEditDep)
                    {
                        btn.Enabled = CanEditDep;
                        EnableEditDeleteColumn(gvInfrastructures);
                    }
                    else
                    {
                        btn.Enabled = false;
                        DisableEditDeleteColumn(gvInfrastructures);

                    }
                }
                else if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                {
                    CanEditDep = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, 2);
                    if (CanEditDep)
                    {
                        btn.Enabled = CanEditDep;
                        EnableEditDeleteColumn(gvInfrastructures);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void AddDeletionConfirmMessage(GridViewRowEventArgs _e)
        {
            Button btnDelete = (Button)_e.Row.FindControl("btnDeleteInfrastructures");
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }

        private void DisableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditInfrastructures = (Button)r.FindControl("btnEditInfrastructures");
                btnEditInfrastructures.Enabled = false;

                Button btnDeleteInfrastructures = (Button)r.FindControl("btnDeleteInfrastructures");
                btnDeleteInfrastructures.Enabled = false;
            }
        }
        private void EnableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditInfrastructures = (Button)r.FindControl("btnEditInfrastructures");
                btnEditInfrastructures.Enabled = true;

                Button btnDeleteInfrastructures = (Button)r.FindControl("btnDeleteInfrastructures");
                btnDeleteInfrastructures.Enabled = true;
            }
        }

        #endregion

        #region Events

        protected void ddlInfrastructuresType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            int index = row.RowIndex;

            DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfrastructuresType");
            DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfrastructuresName");

            if (ddlInfrastructuresType.SelectedItem.Value != "")
            {
                UA_Users _Users = SessionManagerFacade.UserInformation;
                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value);
                if (InfrastructureTypeSelectedValue == 1)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 1);
                else if (InfrastructureTypeSelectedValue == 2)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 2);
                else if (InfrastructureTypeSelectedValue == 3)
                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 3);
            }
            else
            {
                ddlInfrastructuresName.Items.Clear();
            }

        }

        protected void gvInfrastructures_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvInfrastructures.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddInfrastructures")
                {
                    List<object> lstInfrastructures = new FloodInspectionsBLL().GetInfrastructuresByFloodInspectionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                    lstInfrastructures.Add(new
                    {
                        ID = 0,
                        StructureTypeID = string.Empty,
                        StructureID = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvInfrastructures.PageIndex = gvInfrastructures.PageCount;
                    gvInfrastructures.DataSource = lstInfrastructures;
                    gvInfrastructures.DataBind();

                    gvInfrastructures.EditIndex = gvInfrastructures.Rows.Count - 1;
                    gvInfrastructures.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);

                    DataKey key = gvInfrastructures.DataKeys[e.Row.RowIndex];
                    string InfrastructuresTypeID = Convert.ToString(key.Values["StructureTypeID"]);
                    string InfrastructuresNameID = Convert.ToString(key.Values["StructureID"]);

                    Label lblInfrastructuresType = (Label)e.Row.FindControl("lblInfrastructuresType");
                    Label lblInfrastructuresName = (Label)e.Row.FindControl("lblInfrastructuresName");

                    if (InfrastructuresTypeID != null && InfrastructuresTypeID != "" && InfrastructuresNameID != null && InfrastructuresNameID != "")
                    {
                        object res = new FloodOperationsBLL().GetStructureTypeIDByInsfrastructureValue(Convert.ToInt64(InfrastructuresTypeID), Convert.ToInt64(InfrastructuresNameID));
                        if (res != null)
                        {
                            if (lblInfrastructuresType != null)
                            {
                                //  lblInfrastructuresType.Text = res.GetType().GetProperty("InfrastructureTypeName").GetValue(res).ToString();
                                lblInfrastructuresType.Text = Convert.ToString(Utility.GetDynamicPropertyValue(res, "InfrastructureTypeName")) == "Control Structure1" ? "Barrage/Headwork" : Convert.ToString(Utility.GetDynamicPropertyValue(res, "InfrastructureTypeName"));
                            }
                            if (lblInfrastructuresName != null)
                            {
                                lblInfrastructuresName.Text = res.GetType().GetProperty("InfrastructureName").GetValue(res).ToString();
                            }
                        }
                    }

                    if (gvInfrastructures.EditIndex == e.Row.RowIndex)
                    {
                        string ID = Convert.ToString(key.Values["ID"]);

                        #region "Controls"
                        DropDownList ddlInfrastructuresType = (DropDownList)e.Row.FindControl("ddlInfrastructuresType");
                        DropDownList ddlInfrastructuresName = (DropDownList)e.Row.FindControl("ddlInfrastructuresName");
                        #endregion

                        if (ddlInfrastructuresType != null)
                        {
                            Dropdownlist.DDLInfrastructureType(ddlInfrastructuresType);
                            if (!string.IsNullOrEmpty(InfrastructuresTypeID))
                                Dropdownlist.SetSelectedValue(ddlInfrastructuresType, InfrastructuresTypeID);
                        }

                        if (ddlInfrastructuresType != null)
                        {
                            if (ddlInfrastructuresType.SelectedItem.Value != "")
                            {
                                UA_Users _Users = SessionManagerFacade.UserInformation;
                                long InfrastructureTypeSelectedValue = Convert.ToInt64(ddlInfrastructuresType.SelectedItem.Value);
                                if (InfrastructureTypeSelectedValue == 1)
                                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 1);
                                else if (InfrastructureTypeSelectedValue == 2)
                                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 2);
                                else if (InfrastructureTypeSelectedValue == 3)
                                    Dropdownlist.DDInfrastructureNameByType(ddlInfrastructuresName, _Users.ID, 3);
                            }

                            if (!string.IsNullOrEmpty(InfrastructuresNameID))
                                Dropdownlist.SetSelectedValue(ddlInfrastructuresName, InfrastructuresNameID);
                        }

                        if (hdnInfrastructureType.Value == "1" || hdnInfrastructureType.Value == "2")
                        {
                            ddlInfrastructuresType.Enabled = false;
                            ddlInfrastructuresName.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvInfrastructures.DataKeys[e.RowIndex].Values[0]);
                bool IsDeleted = new FloodInspectionsBLL().DeleteInfrastructure(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindInfrastructuresGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvInfrastructures.EditIndex = e.NewEditIndex;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;
                #region "Data Keys"
                DataKey key = gvInfrastructures.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvInfrastructures.Rows[e.RowIndex];
                DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfrastructuresType");
                DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfrastructuresName");
                #endregion

                FO_DInfrastructures dInfrastructures = new FO_DInfrastructures();

                dInfrastructures.ID = Convert.ToInt64(ID);
                dInfrastructures.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);

                if (ddlInfrastructuresType != null)
                    dInfrastructures.StructureTypeID = Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value);

                if (ddlInfrastructuresName != null)
                    dInfrastructures.StructureID = Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value);

                if (dInfrastructures.ID == 0)
                {
                    dInfrastructures.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    dInfrastructures.CreatedDate = DateTime.Now;
                }
                else
                {
                    dInfrastructures.CreatedBy = Convert.ToInt32(CreatedBy);
                    dInfrastructures.CreatedDate = Convert.ToDateTime(CreatedDate);
                    dInfrastructures.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    dInfrastructures.ModifiedDate = DateTime.Now;
                }

                if (new FloodInspectionsBLL().IsDInfrastructureUnique(dInfrastructures))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsSave = new FloodInspectionsBLL().SaveInfrastructures(dInfrastructures);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dInfrastructures.ID) == 0)
                        gvInfrastructures.PageIndex = 0;

                    gvInfrastructures.EditIndex = -1;
                    BindInfrastructuresGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvInfrastructures_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvInfrastructures.PageIndex = e.NewPageIndex;
                gvInfrastructures.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

    }
}