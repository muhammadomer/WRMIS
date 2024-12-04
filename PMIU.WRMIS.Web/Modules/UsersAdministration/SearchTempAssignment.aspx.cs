using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.UsersAdministration
{
    public partial class TemporaryAssignment : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    string Now = Utility.GetFormattedDate(DateTime.Now);
                    txtFrom.Text = Now;
                    txtTo.Text = Now;
                    BindGrid();
                    txtDelegatingFrom.Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created on 01-01-2016
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.SearchTempAssignment);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function returns user data for the user dropdowns
        /// Created On 04-01-2016
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns>List<dynamic></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<dynamic> GetUserInfo(string _Name)
        {
            try
            {
                _Name = _Name.Trim();

                long UserID = (long)HttpContext.Current.Session[SessionValues.UserID];

                List<dynamic> lstUserInfo = new UserBLL().GetUserInfo(_Name, UserID);

                return lstUserInfo;
            }
            catch (Exception exp)
            {
                new WRException((long)HttpContext.Current.Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);

                return null;
            }
        }

        /// <summary>
        /// This function binds records to the grid based on the selected criteria.
        /// Created on 07-01-2016
        /// </summary>
        public void BindGrid()
        {
            btnAdd.Visible = base.CanAdd;

            long FromUserID = Convert.ToInt64(txtFromID.Text);
            long ToUserID = Convert.ToInt64(txtToID.Text);
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (txtFrom.Text.Trim() != String.Empty)
            {
                FromDate = Utility.GetParsedDate(txtFrom.Text.Trim());
            }

            if (txtTo.Text != String.Empty)
            {
                ToDate = Utility.GetParsedDate(txtTo.Text.Trim());
            }

            if (FromDate != null && ToDate != null && FromDate > ToDate)
            {
                Master.ShowMessage(Message.DateCannotBeGreater.Description, SiteMaster.MessageType.Error);
                txtTo.Text = String.Empty;
                return;
            }

            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            List<dynamic> lstActingRole = new UserBLL().GetActingRoles(FromUserID, ToUserID, FromDate, ToDate, mdlUser);

            gvAssignment.DataSource = lstActingRole;
            gvAssignment.DataBind();
        }

        protected void gvAssignment_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvAssignment.EditIndex = -1;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssignment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAssignment.PageIndex = e.NewPageIndex;

                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssignment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Label lblFromDate = (Label)gvAssignment.Rows[e.RowIndex].FindControl("lblFromDate");

                DateTime FromDate = DateTime.Parse(lblFromDate.Text);

                if (FromDate <= DateTime.Now.Date)
                {
                    Master.ShowMessage(Message.DeletePastRecord.Description, SiteMaster.MessageType.Error);
                    return;
                }

                long ActingRoleID = Convert.ToInt64(((Label)gvAssignment.Rows[e.RowIndex].FindControl("lblID")).Text);

                UserBLL bllUser = new UserBLL();

                bool IsDeleted = bllUser.DeleteActingRole(ActingRoleID);

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssignment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblFromDate = (Label)e.Row.FindControl("lblFromDate");
                    DateTime FromDate = DateTime.Parse(lblFromDate.Text);
                    lblFromDate.Text = Utility.GetFormattedDate(FromDate);

                    Label lblToDate = (Label)e.Row.FindControl("lblToDate");
                    DateTime ToDate = DateTime.Parse(lblToDate.Text);
                    lblToDate.Text = Utility.GetFormattedDate(ToDate);

                    Label lblFromLocation = (Label)e.Row.FindControl("lblFromLocation");
                    string FromLocation = lblFromLocation.Text;

                    if (FromLocation.Length > 25)
                    {
                        FromLocation = FromLocation.Substring(0, 25);
                        lblFromLocation.Text = String.Format("{0}{1}", FromLocation, "...");
                    }

                    Label lblToLocation = (Label)e.Row.FindControl("lblToLocation");
                    string ToLocation = lblToLocation.Text;

                    if (ToLocation.Length > 25)
                    {
                        ToLocation = ToLocation.Substring(0, 25);
                        lblToLocation.Text = String.Format("{0}{1}", ToLocation, "...");
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TempAssignment.aspx?PageID=" + base.PageID, false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnEdit = (Button)sender;
                GridViewRow gvRow = (GridViewRow)btnEdit.NamingContainer;

                Label lblFromDate = (Label)gvRow.FindControl("lblFromDate");

                DateTime FromDate = DateTime.Parse(lblFromDate.Text);

                if (FromDate <= DateTime.Now.Date)
                {
                    Master.ShowMessage(Message.EditPastRecord.Description, SiteMaster.MessageType.Error);
                    return;
                }

                Label lblID = (Label)gvRow.FindControl("lblID");

                Response.Redirect("TempAssignment.aspx?ID=" + lblID.Text + "&PageID=" + base.PageID, false);
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvAssignment_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Adding a column manually once the header created
                if (e.Row.RowType == DataControlRowType.Header) // If header created
                {
                    GridView DesignationGrid = (GridView)sender;

                    // Creating a Row
                    GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Delegating From";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 3; // For merging first, second and third column cells to one
                    HeaderCell.CssClass = "col-md-3";
                    HeaderCell.Font.Bold = true;
                    HeaderCell.Style["text-align"] = "center";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Delegating To";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 3;
                    HeaderCell.CssClass = "col-md-3";
                    HeaderCell.Font.Bold = true;
                    HeaderCell.Style["text-align"] = "center";
                    HeaderRow.Cells.Add(HeaderCell);


                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Date From";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.RowSpan = 2;
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Date To";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.RowSpan = 2;
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.RowSpan = 2;
                    HeaderRow.Cells.Add(HeaderCell);

                    //Adding the Row at the 0th position (first row) in the Grid
                    DesignationGrid.Controls[0].Controls.AddAt(0, HeaderRow);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}