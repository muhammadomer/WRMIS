using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class OtherExpense : BasePage
    {
        #region Screen Constants

        const string NewRow = "New Row";

        #endregion

        #region Global Variables

        public string FinancialYear = string.Empty;
        public string Month = string.Empty;
        public long ResourceAllocationID = 0;
        public double TotalClaim = 0;

        #endregion


        #region View State Constants

        public const string PMIUStaffKey = "PMIUStaff";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";

        #endregion

        #region Grid Data Keys

        const int IDKey = 0;
        const int FileNameKey = 1;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["MonthlyExpenseID"]))
                    {
                        ltlPageTitle.Text = "Edit Other Expense";

                        long MonthlyExpenseID = Convert.ToInt64(Request.QueryString["MonthlyExpenseID"]);
                        hdnMonthlyExpenseID.Value = MonthlyExpenseID.ToString();

                        BindExpenseForm();
                    }
                    else
                    {
                        ltlPageTitle.Text = "Add Other Expense";
                        GetQueryStringParameters();
                        SetTopPanel();

                        lbtnBack.PostBackUrl = "~/Modules/Accounts/SearchMonthlyExpenses.aspx?LoadHistory=true";
                    }
                    DateTime now = DateTime.Now;
                    if (FinancialYear != string.Empty)
                    {
                        int StartYear = Convert.ToInt32(FinancialYear.Split('-')[0]);
                        ViewState[StartDate] = string.Format("{0}-{1}-{2}", 1, 7, StartYear);

                        int EndYear = StartYear + 1;
                        ViewState[EndDate] = string.Format("{0}-{1}-{2}", 30, 6, EndYear);
                    }
                    else
                    {
                       

                        if (now.Month > 6)
                        {
                            int StartYear = now.Year;
                            ViewState[StartDate] = string.Format("{0}-{1}-{2}", 1, 7, StartYear);

                            int EndYear = StartYear + 1;
                            ViewState[EndDate] = string.Format("{0}-{1}-{2}", 30, 6, EndYear);
                        }
                        else
                        {
                            int EndYear = now.Year;
                            ViewState[EndDate] = string.Format("{0}-{1}-{2}", 30, 6, EndYear);

                            int StartYear = EndYear - 1;
                            ViewState[StartDate] = string.Format("{0}-{1}-{2}", 1, 7, StartYear);
                        }
                    }
                    UA_Users mdlUser = SessionManagerFacade.UserInformation;
                    if (mdlUser.DesignationID == (long)Constants.Designation.ADM)
                    {
                        int StartYear = now.Year;
                        int StartMonth = now.Month - 1;
                        if (StartMonth == 0)
                        {
                            StartYear = now.Year - 1;
                            StartMonth = 12;
                        }
                        ViewState[StartDate] = string.Format("{0}-{1}-{2}", 1, StartMonth, StartYear);

                        int EndYear = now.Month == 1 ? StartYear + 1 : StartYear;
                        int EndMonth = now.Month;
                        int EndDay = now.Day;
                        ViewState[EndDate] = string.Format("{0}-{1}-{2}", EndDay, EndMonth, EndYear);
                    }
                    if (mdlUser.DesignationID == (long)Constants.Designation.AccountOfficer)
                    {
                        int StartYear = now.Year - 1;
                        int StartMonth = now.Month;
                        ViewState[StartDate] = string.Format("{0}-{1}-{2}", 1, StartMonth, StartYear);

                        int EndYear = StartYear + 1;
                        int EndMonth = now.Month;
                        int EndDay = now.Day;
                        ViewState[EndDate] = string.Format("{0}-{1}-{2}", EndDay, EndMonth, EndYear);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function set the page title and description text in the master file.
        /// Created On 22-05-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function fetches the Query string parameters.
        /// Created On 22-05-2017
        /// </summary>
        private void GetQueryStringParameters()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["FinancialYear"]))
            {
                FinancialYear = Request.QueryString["FinancialYear"].ToString();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Month"]))
            {
                Month = Request.QueryString["Month"].ToString();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["ResourceID"]))
            {
                ResourceAllocationID = Convert.ToInt64(Request.QueryString["ResourceID"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
            {
                TotalClaim = Convert.ToDouble(Request.QueryString["TotalClaim"]);
            }
        }

        /// <summary>
        /// This function sets data in top panel.
        /// Created on 19-05-2017
        /// </summary>
        private void SetTopPanel()
        {
            lblFinancialYear.Text = FinancialYear;
            lblMonth.Text = Month;

            lblTotalClaim.Text = Utility.GetRoundOffValueAccounts(TotalClaim);

            AT_ResourceAllocation mdlResourceAllocation = new AccountsBLL().GetResourceByID(ResourceAllocationID);

            ViewState[PMIUStaffKey] = mdlResourceAllocation.PMIUFieldOffice;

            lblResourceType.Text = (mdlResourceAllocation.PMIUFieldOffice == "F" ? "Field" : "Office");
            lblDesignation.Text = mdlResourceAllocation.UA_Designations.Name;
            lblNameOfStaff.Text = mdlResourceAllocation.StaffUserName;
        }

        /// <summary>
        /// This function binds the Quotation Edit grid.
        /// Created On 22-05-2017
        /// </summary>
        private void BindQuotationGrid()
        {
            long MonthlyExpenseID = Convert.ToInt64(hdnMonthlyExpenseID.Value.Trim());

            List<AT_ExpenseQuotation> lstExpenseQuotation = new AccountsBLL().GetExpenseQuotations(MonthlyExpenseID);

            gvQuotationEdit.DataSource = lstExpenseQuotation;
            gvQuotationEdit.DataBind();
        }

        protected void txtExpenseAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtExpenseAmount.Text.Trim() != string.Empty)
                {
                    double ExpenseAmount = Convert.ToDouble(txtExpenseAmount.Text.Trim());

                    if (ExpenseAmount < 1 || ExpenseAmount > 999999)
                    {
                        pnlQuotation.Visible = false;
                        pnlQuotationEdit.Visible = false;
                        txtExpenseAmount.Text = string.Empty;
                        Master.ShowMessage(Message.AmountValidValue.Description, SiteMaster.MessageType.Error);
                    }
                    else
                    {
                        AccountsBLL bllAccounts = new AccountsBLL();

                        double QuotationLimit = bllAccounts.GetAccountSetupValue((long)Constants.AccountSetup.ExpenseLimitForQuotations);

                        if (ExpenseAmount > QuotationLimit)
                        {
                            long MonthlyExpenseID = Convert.ToInt64(hdnMonthlyExpenseID.Value.Trim());

                            if (MonthlyExpenseID == 0)
                            {
                                Master.ShowMessage(Message.QuotationRequired.Description, SiteMaster.MessageType.Warning);
                                pnlQuotation.Visible = true;
                            }
                            else
                            {
                                List<AT_ExpenseQuotation> lstExpenseQuotation = bllAccounts.GetExpenseQuotations(MonthlyExpenseID);

                                if (lstExpenseQuotation.Count() == 0)
                                {
                                    Master.ShowMessage(Message.QuotationRequired.Description, SiteMaster.MessageType.Warning);
                                    pnlQuotation.Visible = true;
                                }
                                else
                                {
                                    gvQuotationEdit.DataSource = lstExpenseQuotation;
                                    gvQuotationEdit.DataBind();
                                    pnlQuotationEdit.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            pnlQuotation.Visible = false;
                            pnlQuotationEdit.Visible = false;
                        }
                    }
                }
                else
                {
                    pnlQuotation.Visible = false;
                    pnlQuotationEdit.Visible = false;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlNoOfQuotations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlNoOfQuotations.SelectedItem.Value != string.Empty)
                {
                    int NoOfQuotations = Convert.ToInt32(ddlNoOfQuotations.SelectedItem.Value);

                    List<string> lstQuotationRows = new List<string>();

                    for (int i = 1; i <= NoOfQuotations; i++)
                    {
                        lstQuotationRows.Add(NewRow);
                    }

                    gvQuotation.DataSource = lstQuotationRows;
                    gvQuotation.DataBind();
                }
                else
                {
                    gvQuotation.DataSource = null;
                    gvQuotation.DataBind();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvQuotationEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int RowCount = gvQuotationEdit.Rows.Count;

                if (RowCount < 4)
                {
                    Master.ShowMessage(Message.QuotationRequired.Description, SiteMaster.MessageType.Error);
                    e.Cancel = true;

                    BindQuotationGrid();
                }
                else
                {
                    long QuotationID = Convert.ToInt64(gvQuotationEdit.DataKeys[e.RowIndex].Values[IDKey].ToString());

                    AccountsBLL bllAccounts = new AccountsBLL();

                    bllAccounts.DeleteQuotation(QuotationID);

                    BindQuotationGrid();
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotDeleted.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvQuotationEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex != gvQuotationEdit.EditIndex)
                    {
                        Label lblQuotationDate = (Label)e.Row.FindControl("lblQuotationDate");
                        Label lblQuotedPrice = (Label)e.Row.FindControl("lblQuotedPrice");
                        Label lblQuotedTaxPrice = (Label)e.Row.FindControl("lblQuotedTaxPrice");
                        WebFormsTest.FileUploadControl fupAttachQuotationEdit = (WebFormsTest.FileUploadControl)e.Row.FindControl("fupAttachQuotationEdit");

                        if (gvQuotationEdit.DataKeys[e.Row.RowIndex].Values[FileNameKey] != null)
                        {
                            string Attachment = gvQuotationEdit.DataKeys[e.Row.RowIndex].Values[FileNameKey].ToString();

                            string FileName = new System.IO.FileInfo(Attachment).Name;
                            List<string> lstName = new List<string> { FileName };

                            fupAttachQuotationEdit.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                            fupAttachQuotationEdit.Size = 1;
                            fupAttachQuotationEdit.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
                        }

                        DateTime QuotationDate = DateTime.Parse(lblQuotationDate.Text);

                        lblQuotationDate.Text = Utility.GetFormattedDate(QuotationDate);

                        if (lblQuotedPrice.Text != string.Empty)
                        {
                            lblQuotedPrice.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(lblQuotedPrice.Text));
                        }

                        lblQuotedTaxPrice.Text = Utility.GetRoundOffValueAccounts(Convert.ToDouble(lblQuotedTaxPrice.Text));
                    }
                    else
                    {
                        Button btnSave = (Button)e.Row.FindControl("btnSave");
                        ScriptManager.GetCurrent(this).RegisterPostBackControl(btnSave);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvQuotationEdit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    long MonthlyExpenseID = Convert.ToInt64(hdnMonthlyExpenseID.Value.Trim());

                    List<AT_ExpenseQuotation> lstExpenseQuotation = new AccountsBLL().GetExpenseQuotations(MonthlyExpenseID);

                    AT_ExpenseQuotation mdlExpenseQuotation = new AT_ExpenseQuotation();

                    lstExpenseQuotation.Add(mdlExpenseQuotation);

                    gvQuotationEdit.DataSource = lstExpenseQuotation;
                    gvQuotationEdit.EditIndex = lstExpenseQuotation.Count - 1;

                    gvQuotationEdit.DataBind();

                    gvQuotation.Rows[gvQuotation.Rows.Count - 1].FindControl("txtVendorName").Focus();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvQuotationEdit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvQuotationEdit.EditIndex = -1;

                BindQuotationGrid();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void gvQuotationEdit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int RowIndex = e.RowIndex;

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                AccountsBLL bllAccounts = new AccountsBLL();

                int UserID = Convert.ToInt32(mdlUsers.ID);

                long MonthlyExpenseID = Convert.ToInt64(hdnMonthlyExpenseID.Value.Trim());
                TextBox txtVendorName = (TextBox)gvQuotationEdit.Rows[RowIndex].FindControl("txtVendorName");
                TextBox txtQuotationDate = (TextBox)gvQuotationEdit.Rows[RowIndex].FindControl("txtQuotationDate");
                TextBox txtQuotedPrice = (TextBox)gvQuotationEdit.Rows[RowIndex].FindControl("txtQuotedPrice");
                TextBox txtQuotedTaxPrice = (TextBox)gvQuotationEdit.Rows[RowIndex].FindControl("txtQuotedTaxPrice");
                WebFormsTest.FileUploadControl fupAttachQuotation = (WebFormsTest.FileUploadControl)gvQuotationEdit.Rows[RowIndex].FindControl("fupAttachQuotation");

                List<Tuple<string, string, string>> lstNameofFiles = fupAttachQuotation.UploadNow(Configuration.Accounts, "BidCtrl");

                string FileName = null;

                if (lstNameofFiles.Count > 0)
                {
                    FileName = lstNameofFiles[0].Item3.ToString();
                }

                AT_ExpenseQuotation mdlExpenseQuotation = new AT_ExpenseQuotation
                {
                    MonthlyExpenseID = MonthlyExpenseID,
                    VendorName = txtVendorName.Text.Trim(),
                    QuotationDate = Utility.GetParsedDate(txtQuotationDate.Text),
                    QuotedPrice = (txtQuotedPrice.Text == string.Empty ? (double?)null : Convert.ToDouble(txtQuotedPrice.Text)),
                    QuotedPriceWithTax = Convert.ToDouble(txtQuotedTaxPrice.Text),
                    QuotedPriceAttachment = FileName,
                    CreatedBy = UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = UserID,
                    ModifiedDate = DateTime.Now
                };

                bllAccounts.AddQuotation(mdlExpenseQuotation);

                Master.ShowMessage(Message.RecordSaved.Description, SiteMaster.MessageType.Success);

                gvQuotationEdit.EditIndex = -1;
                BindQuotationGrid();

                AT_MonthlyExpenses mdlMonthlyExpenses = new AccountsBLL().GetMonthlyExpenseByID(MonthlyExpenseID);

                if (mdlMonthlyExpenses.Attachment != null)
                {
                    FileName = new System.IO.FileInfo(mdlMonthlyExpenses.Attachment).Name;
                    List<string> lstName = new List<string> { FileName };

                    fupOEAttachBillEdit.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                    fupOEAttachBillEdit.Size = 1;
                    fupOEAttachBillEdit.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
                }

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function generates bill number.
        /// Created On 22-05-2017
        /// </summary>
        /// <returns>string</returns>
        private string GenerateBillNo()
        {
            int Month = DateTime.ParseExact(lblMonth.Text, "MMMM", CultureInfo.InvariantCulture).Month;
            string[] years = lblFinancialYear.Text.Split('-');
            int Year = Convert.ToInt32(years[0]);

            if (Month < 7)
            {
                Year = Year + 1;
            }

            DateTime BillDate = new DateTime(Year, Month, 1);

            string Prefix = string.Format("{0}{1}-", BillDate.ToString("yy"), BillDate.Month.ToString("00"));
            string BillNo = string.Empty;

            BillNo = string.Format("{0}{1}-", Prefix, "OE");

            string OldBillNo = new AccountsBLL().GetBillNumber(BillNo);

            int Index = 1;

            if (OldBillNo == null)
            {
                BillNo = string.Format("{0}{1}", BillNo, Index.ToString("000"));
            }
            else
            {
                string[] parts = OldBillNo.Split('-');
                Index = Convert.ToInt32(parts[2]);
                Index++;
                BillNo = string.Format("{0}{1}", BillNo, Index.ToString("000"));
            }

            return BillNo;
        }

        /// <summary>
        /// This function binds data to expense form for Edit screen.
        /// Created On 22-05-2017
        /// </summary>
        private void BindExpenseForm()
        {
            long MonthlyExpenseID = Convert.ToInt64(hdnMonthlyExpenseID.Value.Trim());

            AT_MonthlyExpenses mdlMonthlyExpenses = new AccountsBLL().GetMonthlyExpenseByID(MonthlyExpenseID);

            ResourceAllocationID = mdlMonthlyExpenses.ResourceAllocationID.Value;
            FinancialYear = mdlMonthlyExpenses.FinancialYear;
            Month = mdlMonthlyExpenses.Month;

            if (!string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
            {
                TotalClaim = Convert.ToDouble(Request.QueryString["TotalClaim"]);
            }

            SetTopPanel();

            lbtnBack.PostBackUrl = string.Format("~/Modules/Accounts/ExpenseDetails.aspx?FinancialYear={0}&Month={1}&ResourceID={2}&TotalClaim={3}&ExpenseTypeID={4}", mdlMonthlyExpenses.FinancialYear, mdlMonthlyExpenses.Month, mdlMonthlyExpenses.ResourceAllocationID, TotalClaim, mdlMonthlyExpenses.ExpenseTypeID);

            lblOEBillNo.Text = mdlMonthlyExpenses.BillNo;
            dvBillNo.Visible = true;

            txtOEBillDate.Text = Utility.GetFormattedDate(mdlMonthlyExpenses.BillDate);
            txtExpenseName.Text = mdlMonthlyExpenses.ExpenseName;
            txtOEApprovalReference.Text = mdlMonthlyExpenses.ApprovalReference;
            txtExpenseAmount.Text = Utility.GetRoundOffValueAccounts(mdlMonthlyExpenses.ExpenseAmount);

            txtExpenseAmount_TextChanged(null, null);

            if (mdlMonthlyExpenses.Attachment != null)
            {
                string FileName = new System.IO.FileInfo(mdlMonthlyExpenses.Attachment).Name;
                List<string> lstName = new List<string> { FileName };

                fupOEAttachBillEdit.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                fupOEAttachBillEdit.Size = 1;
                fupOEAttachBillEdit.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GetQueryStringParameters();

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                int UserID = Convert.ToInt32(mdlUsers.ID);

                List<Tuple<string, string, string>> lstNameofFiles = null;

                if (pnlQuotation.Visible)
                {
                    lstNameofFiles = fupOEAttachBill.UploadNow(Configuration.Accounts, "FormCtrl", gvQuotation.Rows.Count);
                }
                else
                {
                    lstNameofFiles = fupOEAttachBill.UploadNow(Configuration.Accounts, "FormCtrl");
                }

                string FileName = null;

                if (lstNameofFiles.Count > 0)
                {
                    FileName = lstNameofFiles[0].Item3.ToString();
                }

                long MonthlyExpenseID = Convert.ToInt64(hdnMonthlyExpenseID.Value.Trim());

                AccountsBLL bllAccounts = new AccountsBLL();

                AT_MonthlyExpenses mdlMonthlyExpenses = bllAccounts.GetMonthlyExpenseByID(MonthlyExpenseID);

                if (mdlMonthlyExpenses == null)
                {
                    mdlMonthlyExpenses = new AT_MonthlyExpenses()
                    {
                        ResourceAllocationID = ResourceAllocationID,
                        PMIUFieldOffice = ViewState[PMIUStaffKey].ToString(),
                        FinancialYear = FinancialYear,
                        Month = Month,
                        ExpenseTypeID = (long)Constants.ExpenseType.OtherExpense,
                        BillNo = GenerateBillNo(),
                        BillDate = Utility.GetParsedDate(txtOEBillDate.Text),
                        ExpenseName = txtExpenseName.Text.Trim(),
                        ExpenseAmount = Convert.ToDouble(txtExpenseAmount.Text),
                        ApprovalReference = txtOEApprovalReference.Text.Trim(),
                        Attachment = FileName,
                        CreatedBy = UserID,
                        ModifiedBy = UserID,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    bllAccounts.AddMonthlyExpense(mdlMonthlyExpenses);

                    if (pnlQuotation.Visible)
                    {
                        List<AT_ExpenseQuotation> lstExpenseQuotation = new List<AT_ExpenseQuotation>();
                        AT_ExpenseQuotation mdlExpenseQuotation = null;

                        foreach (GridViewRow gvrQuotation in gvQuotation.Rows)
                        {
                            TextBox txtVendorName = (TextBox)gvrQuotation.FindControl("txtVendorName");
                            TextBox txtQuotationDate = (TextBox)gvrQuotation.FindControl("txtQuotationDate");
                            TextBox txtQuotedPrice = (TextBox)gvrQuotation.FindControl("txtQuotedPrice");
                            TextBox txtQuotedTaxPrice = (TextBox)gvrQuotation.FindControl("txtQuotedTaxPrice");
                            WebFormsTest.FileUploadControl fupAttachQuotation = (WebFormsTest.FileUploadControl)gvrQuotation.FindControl("fupAttachQuotation");

                            lstNameofFiles = fupAttachQuotation.UploadNow(Configuration.Accounts, "BidCtrl_" + gvrQuotation.RowIndex);

                            FileName = null;

                            if (lstNameofFiles.Count > 0)
                            {
                                FileName = lstNameofFiles[0].Item3.ToString();
                            }

                            mdlExpenseQuotation = new AT_ExpenseQuotation
                            {
                                VendorName = txtVendorName.Text.Trim(),
                                QuotationDate = Utility.GetParsedDate(txtQuotationDate.Text.Trim()),
                                QuotedPrice = (txtQuotedPrice.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtQuotedPrice.Text.Trim())),
                                QuotedPriceWithTax = Convert.ToDouble(txtQuotedTaxPrice.Text.Trim()),
                                QuotedPriceAttachment = FileName,
                                MonthlyExpenseID = mdlMonthlyExpenses.ID,
                                CreatedBy = UserID,
                                CreatedDate = DateTime.Now,
                                ModifiedBy = UserID,
                                ModifiedDate = DateTime.Now
                            };

                            lstExpenseQuotation.Add(mdlExpenseQuotation);
                        }

                        bllAccounts.SaveQuotationsOfExpense(lstExpenseQuotation);
                    }

                    Response.Redirect("SearchMonthlyExpenses.aspx?LoadHistory=true&RecordSaved=true&BillNo=" + mdlMonthlyExpenses.BillNo, false);
                }
                else
                {
                    TotalClaim = TotalClaim - mdlMonthlyExpenses.ExpenseAmount.Value;

                    mdlMonthlyExpenses.BillDate = Utility.GetParsedDate(txtOEBillDate.Text);
                    mdlMonthlyExpenses.ExpenseName = txtExpenseName.Text.Trim();
                    mdlMonthlyExpenses.ExpenseAmount = Convert.ToDouble(txtExpenseAmount.Text);
                    mdlMonthlyExpenses.ApprovalReference = txtOEApprovalReference.Text.Trim();

                    TotalClaim = TotalClaim + mdlMonthlyExpenses.ExpenseAmount.Value;

                    if (FileName != null)
                    {
                        mdlMonthlyExpenses.Attachment = FileName;
                    }

                    mdlMonthlyExpenses.ModifiedDate = DateTime.Now;
                    mdlMonthlyExpenses.ModifiedBy = UserID;

                    bllAccounts.UpdateMonthlyExpense(mdlMonthlyExpenses);

                    if (pnlQuotation.Visible)
                    {
                        List<AT_ExpenseQuotation> lstExpenseQuotation = new List<AT_ExpenseQuotation>();
                        AT_ExpenseQuotation mdlExpenseQuotation = null;

                        foreach (GridViewRow gvrQuotation in gvQuotation.Rows)
                        {
                            TextBox txtVendorName = (TextBox)gvrQuotation.FindControl("txtVendorName");
                            TextBox txtQuotationDate = (TextBox)gvrQuotation.FindControl("txtQuotationDate");
                            TextBox txtQuotedPrice = (TextBox)gvrQuotation.FindControl("txtQuotedPrice");
                            TextBox txtQuotedTaxPrice = (TextBox)gvrQuotation.FindControl("txtQuotedTaxPrice");
                            WebFormsTest.FileUploadControl fupAttachQuotation = (WebFormsTest.FileUploadControl)gvrQuotation.FindControl("fupAttachQuotation");

                            lstNameofFiles = fupAttachQuotation.UploadNow(Configuration.Accounts, "BidCtrl_" + gvrQuotation.RowIndex);

                            FileName = null;

                            if (lstNameofFiles.Count > 0)
                            {
                                FileName = lstNameofFiles[0].Item3.ToString();
                            }

                            mdlExpenseQuotation = new AT_ExpenseQuotation
                            {
                                VendorName = txtVendorName.Text.Trim(),
                                QuotationDate = Utility.GetParsedDate(txtQuotationDate.Text.Trim()),
                                QuotedPrice = (txtQuotedPrice.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtQuotedPrice.Text.Trim())),
                                QuotedPriceWithTax = Convert.ToDouble(txtQuotedTaxPrice.Text.Trim()),
                                QuotedPriceAttachment = FileName,
                                MonthlyExpenseID = mdlMonthlyExpenses.ID,
                                CreatedBy = UserID,
                                CreatedDate = DateTime.Now,
                                ModifiedBy = UserID,
                                ModifiedDate = DateTime.Now
                            };

                            lstExpenseQuotation.Add(mdlExpenseQuotation);
                        }

                        bllAccounts.SaveQuotationsOfExpense(lstExpenseQuotation);
                    }

                    Response.Redirect(string.Format("ExpenseDetails.aspx?FinancialYear={0}&Month={1}&ResourceID={2}&TotalClaim={3}&ExpenseTypeID={4}", mdlMonthlyExpenses.FinancialYear, mdlMonthlyExpenses.Month, mdlMonthlyExpenses.ResourceAllocationID, TotalClaim, mdlMonthlyExpenses.ExpenseTypeID), false);
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}