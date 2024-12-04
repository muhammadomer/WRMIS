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
    public partial class MembersDepartmental : BasePage
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
                        hdnInspectionStatus.Value = new FloodInspectionsBLL().GetInspectionStatus(_FloodInspectionID).ToString();
                        hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/Departmental/SearchDepartmental.aspx?FloodInspectionID={0}", _FloodInspectionID);
                        BindMembersGrid(_FloodInspectionID);
                    }
                    //  hlBack.NavigateUrl = string.Format("~/Modules/FloodOperations/FloodInspection/Departmental/SearchDepartmental.aspx?ShowHistory=true");
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

        private void BindMembersGrid(long _FloodInspectionID)
        {
            try
            {
                bool CanEditDep = false;
                int _InspectionYear = Utility.GetNumericValueFromQueryString("InspectionYear", 0);
                List<object> lstMembers = new FloodInspectionsBLL().GetMemberDetailsByFloodInspectionID(_FloodInspectionID);

                gvMembersDFI.DataSource = lstMembers;
                gvMembersDFI.DataBind();
                gvMembersDFI.Visible = true;

                Button btn = (Button)gvMembersDFI.HeaderRow.FindControl("btnAddMembers");
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                {
                    btn.Enabled = false;
                    DisableEditDeleteColumn(gvMembersDFI);
                }
                if (Convert.ToInt32(hdnInspectionStatus.Value) == 1)
                {
                    CanEditDep = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, 1);
                    if (CanEditDep)
                    {
                        btn.Enabled = CanEditDep;
                        EnableEditDeleteColumn(gvMembersDFI);
                    }
                    else
                    {
                        btn.Enabled = false;
                        DisableEditDeleteColumn(gvMembersDFI);

                    }
                }
                else if (Convert.ToInt32(hdnInspectionStatus.Value) == 2)
                {
                    CanEditDep = new FloodInspectionsBLL().CanAddEditJointDepartmentalInspection(_InspectionYear, SessionManagerFacade.UserInformation.UA_Designations.ID, 2);
                    if (CanEditDep)
                    {
                        btn.Enabled = CanEditDep;
                        EnableEditDeleteColumn(gvMembersDFI);
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
            Button btnDeleteMembers = (Button)_e.Row.FindControl("btnDeleteMembers");
            if (btnDeleteMembers != null)
            {
                btnDeleteMembers.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
            }
        }

        private void DisableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditMembers = (Button)r.FindControl("btnEditMembers");
                btnEditMembers.Enabled = false;

                Button btnDeleteMembers = (Button)r.FindControl("btnDeleteMembers");
                btnDeleteMembers.Enabled = false;
            }
        }
        private void EnableEditDeleteColumn(GridView grid)
        {
            foreach (GridViewRow r in grid.Rows)
            {
                Button btnEditMembers = (Button)r.FindControl("btnEditMembers");
                btnEditMembers.Enabled = true;

                Button btnDeleteMembers = (Button)r.FindControl("btnDeleteMembers");
                btnDeleteMembers.Enabled = true;
            }
        }
        #endregion

        #region Events

        protected void gvMembersDFI_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvMembersDFI.EditIndex = -1;
                BindMembersGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMembersDFI_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AddMembers")
                {
                    List<object> lstMembers = new FloodInspectionsBLL().GetMemberDetailsByFloodInspectionID(Convert.ToInt64(hdnFloodInspectionsID.Value));
                    lstMembers.Add(new
                    {
                        ID = 0,
                        Name = string.Empty,
                        Designation = string.Empty,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    });

                    gvMembersDFI.PageIndex = gvMembersDFI.PageCount;
                    gvMembersDFI.DataSource = lstMembers;
                    gvMembersDFI.DataBind();

                    gvMembersDFI.EditIndex = gvMembersDFI.Rows.Count - 1;
                    gvMembersDFI.DataBind();
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMembersDFI_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AddDeletionConfirmMessage(e);

                    if (gvMembersDFI.EditIndex == e.Row.RowIndex)
                    {
                        #region "Data Keys"
                        DataKey key = gvMembersDFI.DataKeys[e.Row.RowIndex];
                        string ID = Convert.ToString(key.Values["ID"]);
                        string Name = Convert.ToString(key.Values["Name"]);
                        string Designation = Convert.ToString(key.Values["Designation"]);
                        #endregion

                        #region "Controls"
                        Label lblName = (Label)e.Row.FindControl("llblName");
                        Label lblDesignation = (Label)e.Row.FindControl("lblDesignation");
                        TextBox txtName = (TextBox)e.Row.FindControl("txtName");
                        TextBox txtDesignation = (TextBox)e.Row.FindControl("txtDesignation");
                        #endregion

                        if (txtName != null)
                        {
                            txtName.Text = Name;
                        }
                        if (txtDesignation != null)
                        {
                            txtDesignation.Text = Designation;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMembersDFI_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string ID = Convert.ToString(gvMembersDFI.DataKeys[e.RowIndex].Values[0]);
                bool IsDeleted = new FloodInspectionsBLL().DeleteMemberDetails(Convert.ToInt64(ID));
                if (IsDeleted)
                {
                    BindMembersGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordDeleted.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMembersDFI_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvMembersDFI.EditIndex = e.NewEditIndex;
                BindMembersGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMembersDFI_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UA_Users mdlUser = SessionManagerFacade.UserInformation;

                #region "Data Keys"
                DataKey key = gvMembersDFI.DataKeys[e.RowIndex];
                string ID = Convert.ToString(key.Values["ID"]);
                string CreatedBy = Convert.ToString(key.Values["CreatedBy"]);
                string CreatedDate = Convert.ToString(key.Values["CreatedDate"]);
                #endregion

                #region "Controls"
                GridViewRow row = gvMembersDFI.Rows[e.RowIndex];
                TextBox txtName = (TextBox)row.FindControl("txtName");
                TextBox txtDesignation = (TextBox)row.FindControl("txtDesignation");
                #endregion

                FO_DMemberDetails dMemberDetails = new FO_DMemberDetails();

                dMemberDetails.ID = Convert.ToInt64(ID);
                dMemberDetails.FloodInspectionID = Convert.ToInt64(hdnFloodInspectionsID.Value);

                if (txtName != null)
                    dMemberDetails.Name = txtName.Text;

                if (txtDesignation != null)
                    dMemberDetails.Designation = txtDesignation.Text;

                if (dMemberDetails.ID == 0)
                {
                    dMemberDetails.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    dMemberDetails.CreatedDate = DateTime.Now;
                }
                else
                {
                    dMemberDetails.CreatedBy = Convert.ToInt32(CreatedBy);
                    dMemberDetails.CreatedDate = Convert.ToDateTime(CreatedDate);
                    dMemberDetails.ModifiedBy = Convert.ToInt32(mdlUser.ID);
                    dMemberDetails.ModifiedDate = DateTime.Now;
                }

                if (new FloodInspectionsBLL().IsDMembersUnique(dMemberDetails))
                {
                    Master.ShowMessage(Message.UniqueValueRequired.Description, SiteMaster.MessageType.Error);
                    return;
                }

                bool IsSave = new FloodInspectionsBLL().SaveMemberDetails(dMemberDetails);

                if (IsSave)
                {
                    // Redirect user to first page.
                    if (Convert.ToInt64(dMemberDetails.ID) == 0)
                        gvMembersDFI.PageIndex = 0;

                    gvMembersDFI.EditIndex = -1;
                    BindMembersGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
                    Master.ShowMessage(Message.RecordSaved.Description);
                }
            }
            catch (Exception ex)
            {
                new WRException((long)Session[SessionValues.UserID], ex).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvMembersDFI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMembersDFI.PageIndex = e.NewPageIndex;
                gvMembersDFI.EditIndex = -1;
                BindMembersGrid(Convert.ToInt32(hdnFloodInspectionsID.Value));
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
        #endregion
    }
}