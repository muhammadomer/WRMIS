using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class Designation : BasePage
    {
        List<dynamic> lstDesignation = new List<dynamic>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    BindOrganizationDropdown();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function binds all Designation to the grid on the basis of Organization ID
        /// Created On: 22/12/2015
        /// </summary>
        private void BindDesignationGrid()
        {
            long OrganizationID = ddlOrganization.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlOrganization.SelectedItem.Value);
            lstDesignation = new DesignationBLL().GetAllDesignationsByOrganizationID(OrganizationID);
            gvDesignation.DataSource = lstDesignation;
            gvDesignation.DataBind();
        }

        /// <summary>
        /// this function binds Organizations to Dropdown
        /// Created On: 23/12/2015
        /// </summary>
        private void BindOrganizationDropdown()
        {
            Dropdownlist.DDLOrganizations(ddlOrganization);
        }

        protected void gvDesignation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long OrganizationID = Convert.ToInt64(ddlOrganization.SelectedItem.Value);
                    lstDesignation = new DesignationBLL().GetAllDesignationsByOrganizationID(OrganizationID);
                    UA_Designations mdlDesignation = new UA_Designations();

                    mdlDesignation.ID = 0;
                    mdlDesignation.Name = "";
                    mdlDesignation.Description = "";
                    mdlDesignation.IrrigationLevelID = 0;
                    mdlDesignation.ReportingToDesignationID = 0;
                    mdlDesignation.AuthorityRights = false;
                    mdlDesignation.TempAssignment = false;
                    dynamic a = new
                    {
                        designation = mdlDesignation,
                        reportingToDesignation = "",
                        reportingToDesignationID = "",
                        reportingToOrganization = "",
                        reportingToOrganizationID = ""
                    };
                    lstDesignation.Add(a);

                    gvDesignation.PageIndex = gvDesignation.PageCount;
                    gvDesignation.DataSource = lstDesignation;
                    gvDesignation.DataBind();

                    gvDesignation.EditIndex = gvDesignation.Rows.Count - 1;
                    gvDesignation.DataBind();
                    gvDesignation.Rows[gvDesignation.Rows.Count - 1].FindControl("txtDesignation").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDesignation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDesignation.PageIndex = e.NewPageIndex;
                BindDesignationGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDesignation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                long OrganizationID = ddlOrganization.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlOrganization.SelectedItem.Value);
                long DesignationID = Convert.ToInt64(((Label)gvDesignation.Rows[rowIndex].Cells[0].FindControl("lblID")).Text);
                string DesignationName = ((TextBox)gvDesignation.Rows[rowIndex].Cells[1].FindControl("txtDesignation")).Text.Trim();
                long HdnLevelID = ((DropDownList)gvDesignation.Rows[rowIndex].Cells[2].FindControl("ddlLevel")).SelectedValue == string.Empty ? -1 : Convert.ToInt64(((DropDownList)gvDesignation.Rows[rowIndex].Cells[2].FindControl("ddlLevel")).SelectedValue);
                long Level = ((DropDownList)gvDesignation.Rows[rowIndex].Cells[2].FindControl("ddlLevel")).SelectedValue == string.Empty ? -1 : Convert.ToInt64(((DropDownList)gvDesignation.Rows[rowIndex].Cells[2].FindControl("ddlLevel")).SelectedValue);
                long ReportingToOrganization = ((DropDownList)gvDesignation.Rows[rowIndex].Cells[3].FindControl("ddlReportingToOrganization")).SelectedValue == string.Empty ? -1 : Convert.ToInt64(((DropDownList)gvDesignation.Rows[rowIndex].Cells[3].FindControl("ddlReportingToOrganization")).SelectedValue);
                long ReportingToDesignation = ((DropDownList)gvDesignation.Rows[rowIndex].Cells[4].FindControl("ddlReportingToDesignation")).SelectedValue == string.Empty ? -1 : Convert.ToInt64(((DropDownList)gvDesignation.Rows[rowIndex].Cells[4].FindControl("ddlReportingToDesignation")).SelectedValue);
                bool AuthorityRights = ((CheckBox)gvDesignation.Rows[rowIndex].Cells[5].FindControl("chkLoc")).Checked;
                bool TempAssignment = ((CheckBox)gvDesignation.Rows[rowIndex].Cells[6].FindControl("chkTempAssignment")).Checked;

                if (!IsValidAddEdit(DesignationID, DesignationName, OrganizationID))
                {
                    Master.ShowMessage(Message.DesignationOrganizationDuplication.Description, SiteMaster.MessageType.Error);
                    return;
                }


                UA_Designations mdlDesignation = new UA_Designations();
                mdlDesignation.ID = DesignationID;
                mdlDesignation.Name = DesignationName;
                mdlDesignation.OrganizationID = OrganizationID;
                if (Level != -1)
                    mdlDesignation.IrrigationLevelID = Level;
                else if (HdnLevelID != -1)
                    mdlDesignation.IrrigationLevelID = HdnLevelID;

                if (ReportingToDesignation != -1)
                    mdlDesignation.ReportingToDesignationID = ReportingToDesignation;

                mdlDesignation.AuthorityRights = AuthorityRights;
                mdlDesignation.TempAssignment = TempAssignment;
                mdlDesignation.WorkflowDesignation = false;


                DesignationBLL bllDesignation = new DesignationBLL();
                bool isRecordSaved = false;

                if (DesignationID == 0)
                {
                    isRecordSaved = bllDesignation.AddDesignation(mdlDesignation);

                }
                else
                {
                    isRecordSaved = bllDesignation.UpdateDesignation(mdlDesignation);

                }

                if (isRecordSaved)
                {
                    if (DesignationID == 0)
                    {
                        gvDesignation.PageIndex = 0;
                    }
                    gvDesignation.EditIndex = -1;
                    BindDesignationGrid();
                    Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDesignation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long DesignationID = Convert.ToInt64(((Label)gvDesignation.Rows[e.RowIndex].FindControl("lblID")).Text);
                bool WorkflowDesignation = Convert.ToBoolean(((Label)gvDesignation.Rows[e.RowIndex].FindControl("lblWorkFlowDesignation")).Text);



                if (WorkflowDesignation == true)
                {
                    Master.ShowMessage(Message.DesignationPartOfWorkflow.Description, SiteMaster.MessageType.Error);
                    return;
                }

                if (!IsValidDelete(DesignationID))
                {
                    Master.ShowMessage(Message.DesignationReferenceExists.Description, SiteMaster.MessageType.Error);
                    return;
                }

                DesignationBLL bllDesignation = new DesignationBLL();
                bool IsDelete = bllDesignation.DeleteDesignation(DesignationID);

                if (IsDelete)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindDesignationGrid();
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDesignation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //e.Row.TableSection = TableRowSection.TableHeader;
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[7].Visible = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (gvDesignation.EditIndex != e.Row.RowIndex)
                    {
                        Label lblAuthorityRight = (Label)e.Row.FindControl("lblAuthorityRights");
                        Label lblTempAssignment = (Label)e.Row.FindControl("lblTempAssignemnt");
                        CheckBox chkAuhtorityRight = (CheckBox)e.Row.FindControl("chkLoc");
                        CheckBox chkTempAssignment = (CheckBox)e.Row.FindControl("chkTempAssignment");



                        bool AuthorityRights = Convert.ToBoolean(lblAuthorityRight.Text);
                        bool TempAssignment = Convert.ToBoolean(lblTempAssignment.Text);

                        if (AuthorityRights)
                        {
                            chkAuhtorityRight.Checked = true;
                        }
                        else
                        {
                            chkAuhtorityRight.Checked = false;
                        }

                        if (TempAssignment)
                        {
                            chkTempAssignment.Checked = true;
                        }
                        else
                        {
                            chkTempAssignment.Checked = false;
                        }
                        chkAuhtorityRight.Enabled = false;
                        chkTempAssignment.Enabled = false;

                        bool WorkflowDesignation = Convert.ToBoolean(((Label)e.Row.FindControl("lblWorkFlowDesignation")).Text);
                        if (WorkflowDesignation == true)
                        {
                            Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                            btnDelete.OnClientClick = null;
                        }
                    }


                    if (gvDesignation.EditIndex == e.Row.RowIndex)
                    {
                        string lblWorkFlowDesignation = Convert.ToString(((Label)e.Row.FindControl("lblWorkFlowDesignation")).Text);
                        TextBox txtDesignation = (TextBox)e.Row.FindControl("txtDesignation");
                        DropDownList ddlLevel = (DropDownList)e.Row.FindControl("ddlLevel");
                        DropDownList ddlReportingToOrganization = (DropDownList)e.Row.FindControl("ddlReportingToOrganization");
                        DropDownList ddlReportingToDesignation = (DropDownList)e.Row.FindControl("ddlReportingToDesignation");

                        Dropdownlist.DDLOrganizations(ddlReportingToOrganization);
                        Dropdownlist.DDLIrrigationLevel(ddlLevel);


                        Label lblHdnLevel = (Label)e.Row.FindControl("lblHdnLevel");
                        Dropdownlist.SetSelectedValue(ddlLevel, lblHdnLevel.Text);

                        Label lblHdnReportingToOrganization = (Label)e.Row.FindControl("lblHdnReportingToOrganization");
                        Dropdownlist.SetSelectedValue(ddlReportingToOrganization, lblHdnReportingToOrganization.Text);

                        string strReportingToOrganization = lblHdnReportingToOrganization.Text;
                        long longReportingToOrganization = strReportingToOrganization == string.Empty ? -1 : Convert.ToInt64(strReportingToOrganization);

                        Dropdownlist.DDLDesignations(ddlReportingToDesignation, longReportingToOrganization);
                        ddlReportingToDesignation.Items.Remove(ddlReportingToDesignation.Items.FindByText(txtDesignation.Text));

                        if (longReportingToOrganization == -1)
                        {
                            ddlReportingToDesignation.Enabled = false;
                        }

                        Label lblHdnReportingToDesignation = (Label)e.Row.FindControl("lblHdnReportingToDesignation");
                        Dropdownlist.SetSelectedValue(ddlReportingToDesignation, lblHdnReportingToDesignation.Text);

                        CheckBox chkAuhtorityRight = (CheckBox)e.Row.FindControl("chkLoc");
                        CheckBox chkTempAssignment = (CheckBox)e.Row.FindControl("chkTempAssignment");


                        Label lblAuthorityRight = (Label)e.Row.FindControl("lblAuthorityRights");
                        Label lblTempAssignment = (Label)e.Row.FindControl("lblTempAssignemnt");
                        bool AuthorityRights = Convert.ToBoolean(lblAuthorityRight.Text);
                        bool TempAssignment = Convert.ToBoolean(lblTempAssignment.Text);

                        if (AuthorityRights)
                        {
                            chkAuhtorityRight.Checked = true;
                        }
                        else
                        {
                            chkAuhtorityRight.Checked = false;
                        }

                        if (TempAssignment)
                        {
                            chkTempAssignment.Checked = true;
                        }
                        else
                        {
                            chkTempAssignment.Checked = false;
                        }


                        if (lblWorkFlowDesignation.ToLower() == "true")
                        {
                            txtDesignation.Enabled = false;
                            ddlLevel.Enabled = false;
                            txtDesignation.CssClass = "form-control";
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        protected void gvDesignation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvDesignation.EditIndex = -1;
                BindDesignationGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDesignation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvDesignation.EditIndex = e.NewEditIndex;
                BindDesignationGrid();
                gvDesignation.Rows[e.NewEditIndex].FindControl("txtDesignation").Focus();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvDesignation_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Adding a column manually once the header created
                if (e.Row.RowType == DataControlRowType.Header) // If header created
                {
                    GridView DesignationGrid = (GridView)sender;

                    // Creating a Row
                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Designation";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2; // For merging first, second row cells to one
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Organization";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    //HeaderCell.RowSpan = 2;
                    //HeaderCell.CssClass = "col-md-2";
                    //HeaderCell.Font.Bold = true;
                    //HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Level";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Reporting To";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 2; // For merging two columns
                    HeaderCell.CssClass = "col-md-4";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Authority Rights";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2; // For merging two columns
                    HeaderCell.CssClass = "col-md-4";
                    HeaderCell.Style["text-align"] = "center";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2; // For merging two columns
                    HeaderCell.CssClass = "col-md-1";
                    //HeaderCell.Font.Bold = true;
                    HeaderCell.Style["text-align"] = "center";
                    Button btn = new Button();
                    btn.ID = "btnAdd";

                    btn.CommandName = "Add";
                    btn.CssClass = "btn btn-success btn_add plus";
                    btn.ToolTip = "Add";
                    HeaderCell.Controls.Add(btn);
                    HeaderRow.Cells.Add(HeaderCell);

                    //HeaderRow.CssClass = "theader";

                    //Adding the Row at the 0th position (first row) in the Grid
                    DesignationGrid.Controls[0].Controls.AddAt(0, HeaderRow);



                    /*************************implementation of roles rigts**********************************/
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
                    if (mdlRoleRights != null)
                    {
                        btn.Visible = (bool)mdlRoleRights.BAdd;
                    }

                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    UA_RoleRights mdlRoleRights = Master.GetPageRoleRights();
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
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long OrganizationID = ddlOrganization.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlOrganization.SelectedItem.Value);

                if (OrganizationID == -1)
                {
                    gvDesignation.Visible = false;
                }
                else
                {
                    gvDesignation.EditIndex = -1;
                    BindDesignationGrid();
                    gvDesignation.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlReportingToOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlReportingToOrganization = (DropDownList)sender;
                GridViewRow gvRow2 = (GridViewRow)ddlReportingToOrganization.NamingContainer;

                DropDownList ddlReportingToDesignation = (DropDownList)gvRow2.FindControl("ddlReportingToDesignation");
                TextBox txtDesignation = (TextBox)gvRow2.FindControl("txtDesignation");
                long OrganizationID = ddlReportingToOrganization.SelectedItem.Value == string.Empty ? -1 : Convert.ToInt64(ddlReportingToOrganization.SelectedItem.Value);
                if (OrganizationID == -1)
                {
                    ddlReportingToDesignation.Enabled = false;
                    ddlReportingToDesignation.SelectedIndex = -1;
                }
                else
                {
                    ddlReportingToDesignation.Enabled = true;
                    Dropdownlist.DDLDesignationAgainstOrganization(ddlReportingToDesignation, OrganizationID);
                    ddlReportingToDesignation.Items.Remove(ddlReportingToDesignation.Items.FindByText(txtDesignation.Text));
                }

            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// this function checks that designation name and organization id are unique or not?
        /// Created On: 4/01/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <param name="_DesignationName"></param>
        /// <param name="_OrganizationID"></param>
        /// <returns>bool</returns>
        private bool IsValidAddEdit(long _DesignationID, string _DesignationName, long _OrganizationID)
        {
            DesignationBLL bllDesignation = new DesignationBLL();
            UA_Designations mdlSearchedDesignation = bllDesignation.GetOtherDesignationByName(_DesignationName, _OrganizationID);
            if (mdlSearchedDesignation != null && _DesignationID != mdlSearchedDesignation.ID)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// this function checks that Designation ID exist in users table or not?
        /// Create On: 04/01/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>bool</returns>
        private bool IsValidDelete(long _DesignationID)
        {
            DesignationBLL bllDesignation = new DesignationBLL();

            bool IsExist = bllDesignation.IsDesignationIDExists(_DesignationID);

            if (IsExist)
            {
                return false;
            }

            return true;
        }

        protected void gvDesignation_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvDesignation.EditIndex = -1;
                BindDesignationGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 6-01-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Designation);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }
    }
}