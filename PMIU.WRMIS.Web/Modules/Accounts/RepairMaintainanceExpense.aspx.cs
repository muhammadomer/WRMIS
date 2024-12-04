using PMIU.WRMIS.BLL.Accounts;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Web.Common;
using PMIU.WRMIS.Web.Common.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PMIU.WRMIS.Web.Modules.Accounts
{
    public partial class RepairMaintainanceExpense : BasePage
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
                        ltlPageTitle.Text = "Edit Repair & Maintainance Expense";

                        long MonthlyExpenseID = Convert.ToInt64(Request.QueryString["MonthlyExpenseID"]);
                        hdnMonthlyExpenseID.Value = MonthlyExpenseID.ToString();

                        BindExpenseForm();
                    }
                    else
                    {
                        ltlPageTitle.Text = "Add Repair & Maintainance Expense";
                        GetQueryStringParameters();
                        SetTopPanel();

                        BindAssetDropdown();

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
        /// This function binds the assets dropdown.
        /// Created On 22-05-2017
        /// </summary>
        private void BindAssetDropdown()
        {
            Dropdownlist.DDLResourceAssets(ddlAsset, ResourceAllocationID, null, (int)Constants.DropDownFirstOption.Select, "AssetName", "AssetAllocationID");
        }

        /// <summary>
        /// This function binds the unassigned key parts.
        /// Created On 22-05-2017
        /// </summary>
        /// <param name="_AssetTypeID"></param>
        /// <param name="_ExpenseID"></param>
        private void BindUnAssignedKeyParts(long _AssetTypeID, long? _ExpenseID = null)
        {
            List<dynamic> lstUnAssignedParts = new AccountsBLL().BindUnAssignedKeyParts(_AssetTypeID, _ExpenseID);
            lbKeyPart.DataTextField = "Name";
            lbKeyPart.DataValueField = "ID";
            lbKeyPart.DataSource = lstUnAssignedParts;
            lbKeyPart.DataBind();
        }

        /// <summary>
        /// This function binds the assigned key parts.
        /// Created On 22-05-2017
        /// </summary>
        /// <param name="_AssetTypeID"></param>
        /// <param name="_ExpenseID"></param>
        private void BindAssignedKeyParts(long _AssetTypeID, long? _ExpenseID = null)
        {
            List<dynamic> lstAssignedParts = new AccountsBLL().BindAssignedKeyParts(_AssetTypeID, _ExpenseID);
            lbAssignedKeyPart.DataTextField = "Name";
            lbAssignedKeyPart.DataValueField = "ID";
            lbAssignedKeyPart.DataSource = lstAssignedParts;
            lbAssignedKeyPart.DataBind();
        }

        /// <summary>
        /// This function removes required check from amount fields.
        /// Created On 23-05-2017 
        /// </summary>
        private void ApplyValidation()
        {
            if (!txtRMPurchaseAmount.CssClass.Contains("required"))
            {
                txtRMPurchaseAmount.Attributes.Remove("required");
            }

            if (!txtRepairAmount.CssClass.Contains("required"))
            {
                txtRepairAmount.Attributes.Remove("required");
            }
        }

        /// <summary>
        /// This function sets the required attribute on the amount fields
        /// Created On 23-05-2017
        /// </summary>
        private void SetRequiredFields()
        {
            txtRMPurchaseAmount.CssClass = "form-control decimalInput";
            txtRepairAmount.CssClass = "form-control decimalInput";

            if (txtRMPurchaseAmount.Text.Trim() == string.Empty && txtRepairAmount.Text.Trim() == string.Empty)
            {
                txtRMPurchaseAmount.CssClass = "form-control required decimalInput";
                txtRepairAmount.CssClass = "form-control required decimalInput";
            }
            else if (txtRMPurchaseAmount.Text.Trim() != string.Empty && txtRepairAmount.Text.Trim() == string.Empty)
            {
                txtRMPurchaseAmount.CssClass = "form-control required decimalInput";
            }
            else if (txtRMPurchaseAmount.Text.Trim() == string.Empty && txtRepairAmount.Text.Trim() != string.Empty)
            {
                txtRepairAmount.CssClass = "form-control required decimalInput";
            }
            else if (txtRMPurchaseAmount.Text.Trim() != string.Empty && txtRepairAmount.Text.Trim() != string.Empty)
            {
                txtRMPurchaseAmount.CssClass = "form-control required decimalInput";
            }
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

        /// <summary>
        /// This function calculates the total amount.
        /// Created On 23-05-2017
        /// </summary>
        private void CalculateTotalAmount()
        {
            double? TotalAmount = null;

            if (txtRMPurchaseAmount.Text.Trim() != string.Empty)
            {
                double PurchaseAmount = Convert.ToDouble(txtRMPurchaseAmount.Text.Trim());
                TotalAmount = PurchaseAmount;
            }

            if (txtRepairAmount.Text.Trim() != string.Empty)
            {
                double RepairAmount = Convert.ToDouble(txtRepairAmount.Text.Trim());
                TotalAmount = (TotalAmount == null ? RepairAmount : TotalAmount + RepairAmount);
            }

            if (TotalAmount.HasValue)
            {
                lblTotalAmount.Text = Utility.GetRoundOffValueAccounts(TotalAmount);
            }
            else
            {
                lblTotalAmount.Text = string.Empty;
            }
        }

        /// <summary>
        /// This function shows/hides the Quotation grid
        /// Created On 23-05-2017
        /// </summary>
        private void ShowHideQuotation()
        {
            if (lblTotalAmount.Text.Trim() != string.Empty)
            {
                double TotalAmount = Convert.ToDouble(lblTotalAmount.Text.Trim());

                AccountsBLL bllAccounts = new AccountsBLL();

                double QuotationLimit = bllAccounts.GetAccountSetupValue((long)Constants.AccountSetup.ExpenseLimitForQuotations);

                if (TotalAmount > QuotationLimit)
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
            else
            {
                pnlQuotation.Visible = false;
                pnlQuotationEdit.Visible = false;
            }
        }

        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<ListItem> SelectedItems = (from item in lbKeyPart.Items.OfType<ListItem>()
                                                where item.Selected
                                                select item).ToList<ListItem>();

                foreach (ListItem mdlListItem in SelectedItems)
                {
                    lbKeyPart.Items.Remove(mdlListItem);

                    mdlListItem.Selected = false;

                    lbAssignedKeyPart.Items.Add(mdlListItem);
                }

                List<ListItem> AssignedItems = (from item in lbAssignedKeyPart.Items.OfType<ListItem>() select item).ToList<ListItem>();

                AssignedItems = AssignedItems.OrderBy(i => i.Text).ToList();

                lbAssignedKeyPart.Items.Clear();
                lbAssignedKeyPart.Items.AddRange(AssignedItems.ToArray());
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void btnRemove_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<ListItem> SelectedItems = (from item in lbAssignedKeyPart.Items.OfType<ListItem>()
                                                where item.Selected
                                                select item).ToList<ListItem>();

                foreach (ListItem mdlListItem in SelectedItems)
                {
                    lbAssignedKeyPart.Items.Remove(mdlListItem);

                    mdlListItem.Selected = false;

                    lbKeyPart.Items.Add(mdlListItem);
                }

                List<ListItem> AssignedItems = (from item in lbKeyPart.Items.OfType<ListItem>() select item).ToList<ListItem>();

                AssignedItems = AssignedItems.OrderBy(i => i.Text).ToList();

                lbKeyPart.Items.Clear();
                lbKeyPart.Items.AddRange(AssignedItems.ToArray());
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtRMPurchaseAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtRepairAmount.Focus();

                if (txtRMPurchaseAmount.Text.Trim() != string.Empty)
                {
                    double PurchaseAmount = Convert.ToDouble(txtRMPurchaseAmount.Text.Trim());

                    if (PurchaseAmount < 1 || PurchaseAmount > 999999)
                    {
                        txtRMPurchaseAmount.Focus();
                        txtRMPurchaseAmount.Text = string.Empty;
                        Master.ShowMessage(Message.AmountValidValue.Description, SiteMaster.MessageType.Error);
                    }
                }

                SetRequiredFields();

                CalculateTotalAmount();

                ApplyValidation();

                ShowHideQuotation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtRepairAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ddlAsset.Focus();

                if (txtRepairAmount.Text.Trim() != string.Empty)
                {
                    double RepairAmount = Convert.ToDouble(txtRepairAmount.Text.Trim());

                    if (RepairAmount < 1 || RepairAmount > 999999)
                    {
                        txtRepairAmount.Focus();
                        txtRepairAmount.Text = string.Empty;
                        Master.ShowMessage(Message.AmountValidValue.Description, SiteMaster.MessageType.Error);
                    }
                }

                SetRequiredFields();

                CalculateTotalAmount();

                ApplyValidation();

                ShowHideQuotation();
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

                ApplyValidation();
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

                ApplyValidation();
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

                    gvQuotationEdit.Rows[gvQuotationEdit.Rows.Count - 1].FindControl("txtVendorName").Focus();
                }

                ApplyValidation();
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

                ApplyValidation();
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

                    fupRMAttachBillEdit.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                    fupRMAttachBillEdit.Size = 1;
                    fupRMAttachBillEdit.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
                }

                ApplyValidation();

            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);

                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void ddlAsset_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlAsset.SelectedItem.Value != string.Empty)
                {
                    long AssetAllocationID = Convert.ToInt64(ddlAsset.SelectedItem.Value);

                    AT_AssetAllocation mdlAssetAllocation = new AccountsBLL().GetAssetAllocationByID(AssetAllocationID);

                    long MonthlyExpenseID = Convert.ToInt64(hdnMonthlyExpenseID.Value.Trim());

                    long? ExpenseID = (MonthlyExpenseID != 0 ? MonthlyExpenseID : (long?)null);

                    BindUnAssignedKeyParts(mdlAssetAllocation.AssetTypeID, ExpenseID);
                    BindAssignedKeyParts(mdlAssetAllocation.AssetTypeID, ExpenseID);
                    pnlKeyParts.Visible = true;
                }
                else
                {
                    pnlKeyParts.Visible = false;
                }
            }
            catch (Exception exp)
            {
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

            BillNo = string.Format("{0}{1}-", Prefix, "RM");

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

            lblRMBillNo.Text = mdlMonthlyExpenses.BillNo;
            dvBillNo.Visible = true;

            txtRMBillDate.Text = Utility.GetFormattedDate(mdlMonthlyExpenses.BillDate);
            txtRMApprovalReference.Text = mdlMonthlyExpenses.ApprovalReference;

            if (mdlMonthlyExpenses.PurchaseAmount.HasValue)
            {
                txtRMPurchaseAmount.Text = Utility.GetRoundOffValueAccounts(mdlMonthlyExpenses.PurchaseAmount);
            }

            if (mdlMonthlyExpenses.RepairAmount.HasValue)
            {
                txtRepairAmount.Text = Utility.GetRoundOffValueAccounts(mdlMonthlyExpenses.RepairAmount);
            }

            if (txtRMPurchaseAmount.Text.Trim() != string.Empty)
            {
                txtRMPurchaseAmount_TextChanged(null, null);
            }
            else
            {
                txtRepairAmount_TextChanged(null, null);
            }

            BindAssetDropdown();

            Dropdownlist.SetSelectedValue(ddlAsset, mdlMonthlyExpenses.AssetAllocationID.ToString());

            ddlAsset_SelectedIndexChanged(null, null);

            ddlAsset.Enabled = false;
            ddlAsset.Attributes.Remove("required");
            ddlAsset.CssClass = "form-control";

            if (mdlMonthlyExpenses.Attachment != null)
            {
                string FileName = new System.IO.FileInfo(mdlMonthlyExpenses.Attachment).Name;
                List<string> lstName = new List<string> { FileName };

                fupRMAttachBillEdit.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                fupRMAttachBillEdit.Size = 1;
                fupRMAttachBillEdit.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
            }

            txtRMBillDate.Focus();
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
                    lstNameofFiles = fupRMAttachBill.UploadNow(Configuration.Accounts, "FormCtrl", gvQuotation.Rows.Count);
                }
                else
                {
                    lstNameofFiles = fupRMAttachBill.UploadNow(Configuration.Accounts, "FormCtrl");
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
                        ExpenseTypeID = (long)Constants.ExpenseType.RepairMaintainance,
                        BillNo = GenerateBillNo(),
                        BillDate = Utility.GetParsedDate(txtRMBillDate.Text),
                        AssetAllocationID = Convert.ToInt64(ddlAsset.SelectedItem.Value),
                        RepairAmount = (txtRepairAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtRepairAmount.Text)),
                        PurchaseAmount = (txtRMPurchaseAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtRMPurchaseAmount.Text)),
                        ApprovalReference = txtRMApprovalReference.Text.Trim(),
                        Attachment = FileName,
                        CreatedBy = UserID,
                        ModifiedBy = UserID,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    bllAccounts.AddMonthlyExpense(mdlMonthlyExpenses);

                    List<AT_ExpenseOfParts> lstPartExpense = new List<AT_ExpenseOfParts>();
                    AT_ExpenseOfParts mdlPartExpense = null;

                    foreach (ListItem mdlListItem in lbAssignedKeyPart.Items.OfType<ListItem>())
                    {
                        mdlPartExpense = new AT_ExpenseOfParts
                        {
                            MonthlyExpenseID = mdlMonthlyExpenses.ID,
                            KeyPartsID = Convert.ToInt64(mdlListItem.Value),
                            CreatedBy = UserID,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = UserID,
                            ModifiedDate = DateTime.Now
                        };

                        lstPartExpense.Add(mdlPartExpense);
                    }

                    if (lstPartExpense.Count > 0)
                    {
                        bllAccounts.SaveKeyPartsOfExpense(lstPartExpense);
                    }

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
                    TotalClaim = TotalClaim - ((mdlMonthlyExpenses.RepairAmount.HasValue ? mdlMonthlyExpenses.RepairAmount.Value : 0) + (mdlMonthlyExpenses.PurchaseAmount.HasValue ? mdlMonthlyExpenses.PurchaseAmount.Value : 0));

                    mdlMonthlyExpenses.BillDate = Utility.GetParsedDate(txtRMBillDate.Text);
                    mdlMonthlyExpenses.RepairAmount = (txtRepairAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtRepairAmount.Text));
                    mdlMonthlyExpenses.PurchaseAmount = (txtRMPurchaseAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtRMPurchaseAmount.Text));
                    mdlMonthlyExpenses.ApprovalReference = txtRMApprovalReference.Text.Trim();

                    TotalClaim = TotalClaim + ((mdlMonthlyExpenses.RepairAmount.HasValue ? mdlMonthlyExpenses.RepairAmount.Value : 0) + (mdlMonthlyExpenses.PurchaseAmount.HasValue ? mdlMonthlyExpenses.PurchaseAmount.Value : 0));

                    if (FileName != null)
                    {
                        mdlMonthlyExpenses.Attachment = FileName;
                    }

                    mdlMonthlyExpenses.ModifiedDate = DateTime.Now;
                    mdlMonthlyExpenses.ModifiedBy = UserID;

                    bllAccounts.UpdateMonthlyExpense(mdlMonthlyExpenses);

                    bllAccounts.DeletePartsByExpenseID(mdlMonthlyExpenses.ID);

                    List<AT_ExpenseOfParts> lstPartExpense = new List<AT_ExpenseOfParts>();
                    AT_ExpenseOfParts mdlPartExpense = null;

                    foreach (ListItem mdlListItem in lbAssignedKeyPart.Items.OfType<ListItem>())
                    {
                        mdlPartExpense = new AT_ExpenseOfParts
                        {
                            MonthlyExpenseID = mdlMonthlyExpenses.ID,
                            KeyPartsID = Convert.ToInt64(mdlListItem.Value),
                            CreatedBy = UserID,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = UserID,
                            ModifiedDate = DateTime.Now
                        };

                        lstPartExpense.Add(mdlPartExpense);
                    }

                    if (lstPartExpense.Count > 0)
                    {
                        bllAccounts.SaveKeyPartsOfExpense(lstPartExpense);
                    }

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