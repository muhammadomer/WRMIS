using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.FloodOperations.FFP
{
    public partial class CampSitesFFP : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    long _FFPID = Utility.GetNumericValueFromQueryString("FFPID", 0);

                    if (_FFPID > 0)
                    {
                        // DepartmentalInspectionDetail1.FloodInspectionIDProp = _FFPID;
                        hdnFFPID.Value = Convert.ToString(_FFPID);
                        hdnFFPStatus.Value = new FloodFightingPlanBLL().GetFFPStatus(_FFPID).ToString();

                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FFP/SearchFFP.aspx?FFPID={0}", _FFPID);
                        BindInfrastructuresGrid(_FFPID);
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.FloodOperation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindInfrastructuresGrid(long _FFPID)
        {
            try
            {
                List<object> lstInfrastructures = new FloodFightingPlanBLL().GetInfrastructures_CampSiteBy_FFPID(_FFPID);

                gvInfrastructures.DataSource = lstInfrastructures;
                gvInfrastructures.DataBind();
                gvInfrastructures.Visible = true;
                if (Convert.ToInt32(hdnFFPStatus.Value) == 2)
                {
                    Button btn = (Button)gvInfrastructures.HeaderRow.FindControl("btnAddInfrastructures");
                    btn.Enabled = false;
                    DisableEditDeleteColumn(gvInfrastructures);
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

        protected void gvInfrastructures_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvInfrastructures.EditIndex = -1;
                BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
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
                if (e.CommandName == "Add")
                {
                    List<object> lstInfrastructures = new FloodFightingPlanBLL().GetInfrastructures_CampSiteBy_FFPID(Convert.ToInt64(hdnFFPID.Value));
                    lstInfrastructures.Add(new
                    {
                        ID = 0,
                        FFPID = 0,
                        StructureTypeID = string.Empty,
                        StructureID = string.Empty,
                        Description = "",
                        RDtotal = string.Empty,
                        RD = string.Empty,
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
                                lblInfrastructuresType.Text = res.GetType().GetProperty("InfrastructureTypeName").GetValue(res).ToString();
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

                        #endregion "Controls"

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
                if (!IsValidDelete(Convert.ToInt64(ID)))
                {
                    return;
                }

                bool IsDeleted = new FloodFightingPlanBLL().DeleteFFPCampSites(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
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
                BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
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

                #endregion "Data Keys"

                #region "Controls"

                GridViewRow row = gvInfrastructures.Rows[e.RowIndex];
                DropDownList ddlInfrastructuresType = (DropDownList)row.FindControl("ddlInfrastructuresType");
                DropDownList ddlInfrastructuresName = (DropDownList)row.FindControl("ddlInfrastructuresName");

                TextBox txtRDLeft = (TextBox)row.FindControl("txtRDLeft");
                TextBox txtRDRight = (TextBox)row.FindControl("txtRDRight");
                TextBox txtDesc = (TextBox)row.FindControl("txtDesc");

                #endregion "Controls"

                FO_FFPCampSites dInfrastructures = new FO_FFPCampSites();

                dInfrastructures.ID = Convert.ToInt64(ID);
                dInfrastructures.FFPID = Convert.ToInt64(hdnFFPID.Value);

                if (ddlInfrastructuresType != null)
                    dInfrastructures.StructureTypeID = Convert.ToInt16(ddlInfrastructuresType.SelectedItem.Value);

                if (ddlInfrastructuresName != null)
                    dInfrastructures.StructureID = Convert.ToInt16(ddlInfrastructuresName.SelectedItem.Value);

                if (txtRDLeft != null & txtRDRight != null)
                {
                    dInfrastructures.RD = Calculations.CalculateTotalRDs(txtRDLeft.Text, txtRDRight.Text);
                }
                dInfrastructures.Description = txtDesc.Text;

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

                if (new FloodFightingPlanBLL().IsFFPCampSits_Unique(dInfrastructures))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }
                bool IsSave = new FloodFightingPlanBLL().SaveFFPCampSites(dInfrastructures);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dInfrastructures.ID) == 0)
                        gvInfrastructures.PageIndex = 0;

                    gvInfrastructures.EditIndex = -1;
                    BindInfrastructuresGrid(Convert.ToInt32(hdnFFPID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private bool IsValidDelete(long ID)
        {
            FloodFightingPlanBLL bl = new FloodFightingPlanBLL();
            bool IsExist = bl.IsFo_FFPCampSite_IDExists(ID);

            if (IsExist)
            {
                Master.ShowMessage(Message.RecordAssociationsNotDeleted.Description, SiteMaster.MessageType.Error);

                return false;
            }

            return true;
        }
    }
}