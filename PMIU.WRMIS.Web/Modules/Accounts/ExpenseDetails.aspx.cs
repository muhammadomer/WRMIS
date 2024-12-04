using PMIU.WRMIS.BLL.Accounts;
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

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class ExpenseDetails : BasePage
    {
        AccountsBLL bllAcounts = new AccountsBLL();
        double? RepairMaintainanceTotal = 0;
        double? Sum = 0;
        double? TAAmount = 0;
        double? DAAmount = 0;
        double? TADA = 0;
        double? NewPurchaseTotal = 0;
        double? ExpenseAmount = 0;
        public string TotalClaim = string.Empty;
        bool ShowStatus = false;

        List<AT_MonthlyExpenses> lstMonthlyExpenses = new List<AT_MonthlyExpenses>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();
                    hlBack.NavigateUrl = "~/Modules/Accounts/SearchMonthlyExpenses.aspx?LoadHistory=true";
                    if (!string.IsNullOrEmpty(Request.QueryString["Month"]) && !string.IsNullOrEmpty(Request.QueryString["FinancialYear"]) && !string.IsNullOrEmpty(Request.QueryString["ResourceID"]) && !string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
                    {
                        string Month = Convert.ToString(Request.QueryString["Month"]);
                        string FinancialYear = Convert.ToString(Request.QueryString["FinancialYear"]);
                        long ResourceID = Convert.ToInt64(Request.QueryString["ResourceID"]);
                        double? TotalClaim = Convert.ToInt64(Request.QueryString["TotalClaim"]);
                        BindTableData(Month, FinancialYear, ResourceID, TotalClaim);
                    }

                    BindExpenseTypeDropdown();

                    if (!string.IsNullOrEmpty(Request.QueryString["ExpenseTypeID"]))
                    {
                        string ExpenseTypeID = Convert.ToString(Request.QueryString["ExpenseTypeID"]);
                        Dropdownlist.SetSelectedValue(ddlExpenseType, ExpenseTypeID);
                        BindGrid();
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["RecordSaved"]))
                    {
                        Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        private void BindTableData(string _Month, string _FinancialYear, long _ResourceID, double? _TotalClaim)
        {
            AT_ResourceAllocation mdlResourceAllocation = new AccountsBLL().GetResourceByID(_ResourceID);

            lblResourceType.Text = (mdlResourceAllocation.PMIUFieldOffice == "F" ? "Field" : "Office");
            lblDesignation.Text = mdlResourceAllocation.UA_Designations.Name;
            lblNameofStaff.Text = mdlResourceAllocation.StaffUserName;

            lblFinancialYear.Text = _FinancialYear;
            lblMonth.Text = _Month;
            lblTotalClaim.Text = Utility.GetRoundOffValueAccounts(_TotalClaim);
        }

        private void BindGrid()
        {
            long ExpenseTypeID = 0;
            if (ddlExpenseType.SelectedItem.Value != "")
            {
                ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value);
                if (!string.IsNullOrEmpty(Request.QueryString["Month"]) && !string.IsNullOrEmpty(Request.QueryString["FinancialYear"]) && !string.IsNullOrEmpty(Request.QueryString["ResourceID"]) && !string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
                {
                    string Month = Convert.ToString(Request.QueryString["Month"]);
                    string FinancialYear = Convert.ToString(Request.QueryString["FinancialYear"]);
                    long ResourceID = Convert.ToInt64(Request.QueryString["ResourceID"]);
                    long TotalClaim = Convert.ToInt64(Request.QueryString["TotalClaim"]);

                    lstMonthlyExpenses = bllAcounts.GetMonthlyExpenses(ExpenseTypeID, Month, FinancialYear, ResourceID);
                }

            }

            UA_Users mdlUsers = SessionManagerFacade.UserInformation;

            if (mdlUsers.DesignationID != (long)Constants.Designation.AccountOfficer)
            {
                ShowStatus = lstMonthlyExpenses.Any(me => me.ExpenseStatusID != null);
            }
            else
            {
                if (lblResourceType.Text == "Office")
                {
                    ShowStatus = lstMonthlyExpenses.Any(me => me.ExpenseStatusID != null);
                }
                else
                {
                    lstMonthlyExpenses = lstMonthlyExpenses.Where(me => me.ExpenseStatusID != null || me.CreatedBy == mdlUsers.ID).ToList();

                    ShowStatus = lstMonthlyExpenses.Any(me => me.ExpenseSubmitted == "AO");
                }
            }

            if (ddlExpenseType.SelectedItem.Value == "")
            {
                gvRepairMaintenance.Visible = false;
                gvPOLRecipts.Visible = false;
                gvTADA.Visible = false;
                gvNewPurchase.Visible = false;
                gvOtherExpenses.Visible = false;
            }
            if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
            {
                gvRepairMaintenance.Visible = true;
                gvPOLRecipts.Visible = false;
                gvTADA.Visible = false;
                gvNewPurchase.Visible = false;
                gvOtherExpenses.Visible = false;

                double? PurchaseItemAmount = lstMonthlyExpenses.Select(x => x.PurchaseAmount).Sum();
                double? RepaitItemAmount = lstMonthlyExpenses.Select(x => x.RepairAmount).Sum();
                RepairMaintainanceTotal = PurchaseItemAmount + RepaitItemAmount;

                gvRepairMaintenance.DataSource = lstMonthlyExpenses;
                gvRepairMaintenance.DataBind();

                if (ShowStatus)
                {
                    gvRepairMaintenance.HeaderRow.Cells[gvRepairMaintenance.Columns.Count - 1].Text = "Status";
                }
                else
                {
                    gvRepairMaintenance.HeaderRow.Cells[gvRepairMaintenance.Columns.Count - 1].Text = "Action";
                }
            }
            if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
            {
                gvRepairMaintenance.Visible = false;
                gvPOLRecipts.Visible = true;
                gvTADA.Visible = false;
                gvNewPurchase.Visible = false;
                gvOtherExpenses.Visible = false;

                Sum = lstMonthlyExpenses.Select(x => x.POLAmount).Sum();

                gvPOLRecipts.DataSource = lstMonthlyExpenses;
                gvPOLRecipts.DataBind();

                if (ShowStatus)
                {
                    gvPOLRecipts.HeaderRow.Cells[gvPOLRecipts.Columns.Count - 1].Text = "Status";
                }
                else
                {
                    gvPOLRecipts.HeaderRow.Cells[gvPOLRecipts.Columns.Count - 1].Text = "Action";
                }
            }
            if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
            {
                gvRepairMaintenance.Visible = false;
                gvPOLRecipts.Visible = false;
                gvTADA.Visible = true;
                gvNewPurchase.Visible = false;
                gvOtherExpenses.Visible = false;

                TAAmount = lstMonthlyExpenses.Select(x => x.TAAmount).Sum();
                DAAmount = lstMonthlyExpenses.Select(x => x.DAAmount).Sum();
                TADA = TAAmount + DAAmount;

                gvTADA.DataSource = lstMonthlyExpenses;
                gvTADA.DataBind();
            }
            if (ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
            {
                gvRepairMaintenance.Visible = false;
                gvPOLRecipts.Visible = false;
                gvTADA.Visible = false;
                gvNewPurchase.Visible = true;
                gvOtherExpenses.Visible = false;

                NewPurchaseTotal = lstMonthlyExpenses.Select(x => x.PurchaseAmount).Sum();

                gvNewPurchase.DataSource = lstMonthlyExpenses;
                gvNewPurchase.DataBind();

                if (ShowStatus)
                {
                    gvNewPurchase.HeaderRow.Cells[gvNewPurchase.Columns.Count - 1].Text = "Status";
                }
                else
                {
                    gvNewPurchase.HeaderRow.Cells[gvNewPurchase.Columns.Count - 1].Text = "Action";
                }
            }

            if (ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense)
            {
                gvRepairMaintenance.Visible = false;
                gvPOLRecipts.Visible = false;
                gvTADA.Visible = false;
                gvNewPurchase.Visible = false;
                gvOtherExpenses.Visible = true;

                ExpenseAmount = lstMonthlyExpenses.Select(x => x.ExpenseAmount).Sum();

                gvOtherExpenses.DataSource = lstMonthlyExpenses;
                gvOtherExpenses.DataBind();

                if (ShowStatus)
                {
                    gvOtherExpenses.HeaderRow.Cells[gvOtherExpenses.Columns.Count - 1].Text = "Status";
                }
                else
                {
                    gvOtherExpenses.HeaderRow.Cells[gvOtherExpenses.Columns.Count - 1].Text = "Action";
                }
            }
        }

        private void BindExpenseTypeDropdown()
        {
            UA_Users mdlUser = SessionManagerFacade.UserInformation;

            if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
            {
                Dropdownlist.DDLExpenseType(ddlExpenseType);
            }
            else
            {
                Dropdownlist.DDLExpenseType(ddlExpenseType, "O");
            }
        }

        #region Repair And Maintenance

        protected void gvRepairMaintenance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblID = (Label)e.Row.FindControl("lblID");
                    Label lblBillDate = (Label)e.Row.FindControl("lblBillDate");
                    Label lblPurchaseAmount = (Label)e.Row.FindControl("lblPurchaseAmount");
                    Label lblRepairAmount = (Label)e.Row.FindControl("lblRepairAmount");
                    Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    if (!string.IsNullOrEmpty(lblBillDate.Text))
                        lblBillDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblBillDate.Text));

                    double RepairAmount = 0;
                    double PurchaseAmount = 0;

                    if (lblRepairAmount.Text != string.Empty)
                    {
                        RepairAmount = Convert.ToDouble(lblRepairAmount.Text);
                    }

                    if (lblPurchaseAmount.Text != string.Empty)
                    {
                        PurchaseAmount = Convert.ToDouble(lblPurchaseAmount.Text);
                    }

                    lblTotal.Text = Utility.GetRoundOffValueAccounts(PurchaseAmount + RepairAmount);

                    if (!string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
                    {
                        TotalClaim = Request.QueryString["TotalClaim"].ToString();
                    }

                    hlEdit.NavigateUrl = string.Format("~/Modules/Accounts/RepairMaintainanceExpense.aspx?TotalClaim={0}&MonthlyExpenseID={1}", TotalClaim, lblID.Text);

                    if (ShowStatus)
                    {
                        lblStatus.Visible = true;
                        hlEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        lblStatus.Visible = false;
                        hlEdit.Visible = true;
                        btnDelete.Visible = true;
                    }

                    double? lblPurchaseAmountNullable = lblPurchaseAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblPurchaseAmount.Text);
                    double? lblRepairAmountNullable = lblRepairAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblRepairAmount.Text);

                    lblPurchaseAmount.Text = Utility.GetRoundOffValueAccounts(lblPurchaseAmountNullable);
                    lblRepairAmount.Text = Utility.GetRoundOffValueAccounts(lblRepairAmountNullable);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalClaimForTheMonth = (Label)e.Row.FindControl("lblTotalClaimForTheMonth");
                    lblTotalClaimForTheMonth.Text = Utility.GetRoundOffValueAccounts(RepairMaintainanceTotal);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvRepairMaintenance_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ExpenseID = Convert.ToInt64(((Label)gvRepairMaintenance.Rows[e.RowIndex].FindControl("lblID")).Text);
                bool IsDeleted = bllAcounts.DeleteMonthlyExpense(ExpenseID);
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

        protected void gvRepairMaintenance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRepairMaintenance.PageIndex = e.NewPageIndex;
                gvRepairMaintenance.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region gvPOLRecipts
        protected void gvPOLRecipts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblID = (Label)e.Row.FindControl("lblID");
                    Label lblPOLDatetime = (Label)e.Row.FindControl("lblPOLDatetime");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    Label lblPOLAmount = (Label)e.Row.FindControl("lblPOLAmount");

                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    if (lblPOLDatetime.Text != "")
                        lblPOLDatetime.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblPOLDatetime.Text));

                    if (!string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
                    {
                        TotalClaim = Request.QueryString["TotalClaim"].ToString();
                    }

                    hlEdit.NavigateUrl = string.Format("~/Modules/Accounts/POLExpense.aspx?TotalClaim={0}&MonthlyExpenseID={1}", TotalClaim, lblID.Text);

                    if (ShowStatus)
                    {
                        lblStatus.Visible = true;
                        hlEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        lblStatus.Visible = false;
                        hlEdit.Visible = true;
                        btnDelete.Visible = true;
                    }

                    double? lblPOLAmountNullable = lblPOLAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblPOLAmount.Text);
                    lblPOLAmount.Text = Utility.GetRoundOffValueAccounts(lblPOLAmountNullable);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalClaimForTheMonth = (Label)e.Row.FindControl("lblTotalClaimForTheMonth");
                    lblTotalClaimForTheMonth.Text = Utility.GetRoundOffValueAccounts(Sum);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvPOLRecipts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ExpenseID = Convert.ToInt64(((Label)gvPOLRecipts.Rows[e.RowIndex].FindControl("lblID")).Text);
                bool IsDeleted = bllAcounts.DeleteMonthlyExpense(ExpenseID);
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

        protected void gvPOLRecipts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPOLRecipts.PageIndex = e.NewPageIndex;
                gvPOLRecipts.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region gv TADA
        protected void gvTADA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //e.Row.TableSection = TableRowSection.TableHeader;
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblTAAmount = (Label)e.Row.FindControl("lblTAAmount");
                    Label lblDAAmount = (Label)e.Row.FindControl("lblDAAmount");
                    Label lblMiscExpenditures = (Label)e.Row.FindControl("lblMiscExpenditures");
                    Label lblTADASum = (Label)e.Row.FindControl("lblTADASum");
                    Label lblID = (Label)e.Row.FindControl("lblID");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    double TAAmount = 0;
                    double DAAmount = 0;
                    double MiscExpenditures = 0;
                    double TADASum = 0;

                    if (lblTAAmount.Text != "")
                        TAAmount = Convert.ToDouble(lblTAAmount.Text);
                    if (lblDAAmount.Text != "")
                        DAAmount = Convert.ToDouble(lblDAAmount.Text);
                    if (lblMiscExpenditures.Text != "")
                        MiscExpenditures = Convert.ToDouble(lblMiscExpenditures.Text);

                    TADASum = TAAmount + DAAmount + MiscExpenditures;
                    lblTADASum.Text = TADASum.ToString();

                    if (!string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
                    {
                        TotalClaim = Request.QueryString["TotalClaim"].ToString();
                    }

                    hlEdit.NavigateUrl = string.Format("~/Modules/Accounts/TADAExpense.aspx?TotalClaim={0}&MonthlyExpenseID={1}", TotalClaim, lblID.Text);

                    if (ShowStatus)
                    {
                        lblStatus.Visible = true;
                        hlEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        lblStatus.Visible = false;
                        hlEdit.Visible = true;
                        btnDelete.Visible = true;
                    }

                    double? lblTAAmountNullable = lblTAAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblTAAmount.Text);
                    double? lblDAAmountNullable = lblDAAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblDAAmount.Text);
                    double? lblMiscExpendituresNullable = lblMiscExpenditures.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblMiscExpenditures.Text);
                    double? lblTADASumNullable = lblTADASum.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblTADASum.Text);

                    lblTAAmount.Text = Utility.GetRoundOffValueAccounts(lblTAAmountNullable);
                    lblDAAmount.Text = Utility.GetRoundOffValueAccounts(lblDAAmountNullable);
                    lblMiscExpenditures.Text = Utility.GetRoundOffValueAccounts(lblMiscExpendituresNullable);
                    lblTADASum.Text = Utility.GetRoundOffValueAccounts(lblTADASumNullable);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalClaimForTheMonth = (Label)e.Row.FindControl("lblTotalClaimForTheMonth");
                    lblTotalClaimForTheMonth.Text = Utility.GetRoundOffValueAccounts(TADA);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTADA_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ExpenseID = Convert.ToInt64(((Label)gvTADA.Rows[e.RowIndex].FindControl("lblID")).Text);
                bool IsDeleted = bllAcounts.DeleteMonthlyExpense(ExpenseID);
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

        protected void gvTADA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTADA.PageIndex = e.NewPageIndex;
                gvTADA.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvTADA_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Adding a column manually once the header created
                if (e.Row.RowType == DataControlRowType.Header) // If header created
                {
                    GridView TADAGrid = (GridView)sender;

                    // Creating a Row
                    GridViewRow HeaderRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);


                    TableHeaderCell HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Bill No";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2; // For merging first, second row cells to one
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Half Dailies";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderCell.Style["text-align"] = "center";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Full Dailies";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderCell.Style["text-align"] = "center";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Total KM Travelled (km)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderCell.Style["text-align"] = "center";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Misc. Expense (Rs.)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-2";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();
                    HeaderCell.Text = "Total Claim (Rs.)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableHeaderCell();

                    if (ShowStatus)
                    {
                        HeaderCell.Text = "Status";
                    }
                    else
                    {
                        HeaderCell.Text = "Action";
                    }

                    HeaderCell.HorizontalAlign = HorizontalAlign.Left;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "col-md-1";
                    HeaderCell.Font.Bold = true;
                    HeaderRow.Cells.Add(HeaderCell);

                    TADAGrid.Controls[0].Controls.AddAt(0, HeaderRow);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region gvNewPurchase
        protected void gvNewPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBillDate = (Label)e.Row.FindControl("lblBillDate");
                    Label lblID = (Label)e.Row.FindControl("lblID");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    Label lblPurchaseAmount = (Label)e.Row.FindControl("lblPurchaseAmount");

                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    lblBillDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblBillDate.Text));

                    if (!string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
                    {
                        TotalClaim = Request.QueryString["TotalClaim"].ToString();
                    }

                    hlEdit.NavigateUrl = string.Format("~/Modules/Accounts/NewPurchaseExpense.aspx?TotalClaim={0}&MonthlyExpenseID={1}", TotalClaim, lblID.Text);

                    if (ShowStatus)
                    {
                        lblStatus.Visible = true;
                        hlEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        lblStatus.Visible = false;
                        hlEdit.Visible = true;
                        btnDelete.Visible = true;
                    }

                    double? lblPurchaseAmountNullable = lblPurchaseAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblPurchaseAmount.Text);
                    lblPurchaseAmount.Text = Utility.GetRoundOffValueAccounts(lblPurchaseAmountNullable);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalClaimForTheMonth = (Label)e.Row.FindControl("lblTotalClaimForTheMonth");
                    lblTotalClaimForTheMonth.Text = Utility.GetRoundOffValueAccounts(NewPurchaseTotal);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvNewPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ExpenseID = Convert.ToInt64(((Label)gvNewPurchase.Rows[e.RowIndex].FindControl("lblID")).Text);
                bool IsDeleted = bllAcounts.DeleteMonthlyExpense(ExpenseID);
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

        protected void gvNewPurchase_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvNewPurchase.PageIndex = e.NewPageIndex;
                gvNewPurchase.EditIndex = -1;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        #region gv Others
        protected void gvOtherExpenses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblBillDate = (Label)e.Row.FindControl("lblBillDate");
                    Label lblID = (Label)e.Row.FindControl("lblID");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    Label lblPurchaseAmount = (Label)e.Row.FindControl("lblPurchaseAmount");

                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                    lblBillDate.Text = Utility.GetFormattedDate(Convert.ToDateTime(lblBillDate.Text));

                    if (!string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
                    {
                        TotalClaim = Request.QueryString["TotalClaim"].ToString();
                    }

                    hlEdit.NavigateUrl = string.Format("~/Modules/Accounts/OtherExpense.aspx?TotalClaim={0}&MonthlyExpenseID={1}", TotalClaim, lblID.Text);

                    if (ShowStatus)
                    {
                        lblStatus.Visible = true;
                        hlEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        lblStatus.Visible = false;
                        hlEdit.Visible = true;
                        btnDelete.Visible = true;
                    }

                    double? lblPurchaseAmountNullable = lblPurchaseAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblPurchaseAmount.Text);
                    lblPurchaseAmount.Text = Utility.GetRoundOffValueAccounts(lblPurchaseAmountNullable);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTotalClaimForTheMonth = (Label)e.Row.FindControl("lblTotalClaimForTheMonth");
                    lblTotalClaimForTheMonth.Text = Utility.GetRoundOffValueAccounts(ExpenseAmount);
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOtherExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long ExpenseID = Convert.ToInt64(((Label)gvOtherExpenses.Rows[e.RowIndex].FindControl("lblID")).Text);
                bool IsDeleted = bllAcounts.DeleteMonthlyExpense(ExpenseID);
                if (IsDeleted)
                {
                    Master.ShowMessage(Message.RecordDeleted.Description, SiteMaster.MessageType.Success);
                    BindGrid();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvOtherExpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvNewPurchase.EditIndex = -1;
                gvNewPurchase.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion

        protected void ddlExpenseType_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}