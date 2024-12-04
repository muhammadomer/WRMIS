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
    public partial class POLExpense : BasePage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetPageTitle();

                    if (!string.IsNullOrEmpty(Request.QueryString["MonthlyExpenseID"]))
                    {
                        ltlPageTitle.Text = "Edit POL Expense";

                        long MonthlyExpenseID = Convert.ToInt64(Request.QueryString["MonthlyExpenseID"]);
                        hdnMonthlyExpenseID.Value = MonthlyExpenseID.ToString();

                        BindExpenseForm();
                    }
                    else
                    {
                        ltlPageTitle.Text = "Add POL Expense";
                        GetQueryStringParameters();
                        SetTopPanel();
                        BindVehicleDropdown();

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
        /// Created On 17-05-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the vehicle dropdown
        /// Created On 17-05-2017
        /// </summary>
        private void BindVehicleDropdown()
        {
            List<long> AssetTypeIDs = new List<long>() { (long)Constants.AssetType.FourWheelVehicle, (long)Constants.AssetType.TwoWheelVehicle };

            Dropdownlist.DDLResourceAssets(ddlVehicle, ResourceAllocationID, AssetTypeIDs);
        }

        /// <summary>
        /// This function fetches the Query string parameters.
        /// Created On 17-05-2017
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
        /// Created on 17-05-2017
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
        /// This function generates bill number.
        /// Created On 17-05-2017
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

            BillNo = string.Format("{0}{1}-", Prefix, "POL");

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
        /// Created On 17-05-2017
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

            BindVehicleDropdown();

            lbtnBack.PostBackUrl = string.Format("~/Modules/Accounts/ExpenseDetails.aspx?FinancialYear={0}&Month={1}&ResourceID={2}&TotalClaim={3}&ExpenseTypeID={4}", mdlMonthlyExpenses.FinancialYear, mdlMonthlyExpenses.Month, mdlMonthlyExpenses.ResourceAllocationID, TotalClaim, mdlMonthlyExpenses.ExpenseTypeID);

            lblPRBillNo.Text = mdlMonthlyExpenses.BillNo;
            dvBillNo.Visible = true;

            Dropdownlist.SetSelectedValue(ddlVehicle, mdlMonthlyExpenses.POLVehicleNo);
            txtAmount.Text = Utility.GetRoundOffValueAccounts(mdlMonthlyExpenses.POLAmount);
            txtReceiptNo.Text = mdlMonthlyExpenses.POLReceiptNo;
            txtDate.Text = Utility.GetFormattedDate(mdlMonthlyExpenses.POLDatetime);
            txtMeterReading.Text = mdlMonthlyExpenses.POLMeterReading;

            if (mdlMonthlyExpenses.Attachment != null)
            {
                string FileName = new System.IO.FileInfo(mdlMonthlyExpenses.Attachment).Name;
                List<string> lstName = new List<string> { FileName };

                fupAttachReceiptEdit.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                fupAttachReceiptEdit.Size = 1;
                fupAttachReceiptEdit.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GetQueryStringParameters();

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                int UserID = Convert.ToInt32(mdlUsers.ID);

                List<Tuple<string, string, string>> lstNameofFiles = fupAttachReceipt.UploadNow(Configuration.Accounts);

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
                        ExpenseTypeID = (long)Constants.ExpenseType.POLReceipts,
                        BillNo = GenerateBillNo(),
                        POLVehicleNo = ddlVehicle.SelectedItem.Value,
                        POLAmount = Convert.ToDouble(txtAmount.Text),
                        POLReceiptNo = txtReceiptNo.Text,
                        POLDatetime = Utility.GetParsedDate(txtDate.Text),
                        POLMeterReading = txtMeterReading.Text,
                        Attachment = FileName,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = UserID,
                        ModifiedBy = UserID
                    };

                    bllAccounts.AddMonthlyExpense(mdlMonthlyExpenses);

                    Response.Redirect("SearchMonthlyExpenses.aspx?LoadHistory=true&RecordSaved=true&BillNo=" + mdlMonthlyExpenses.BillNo, false);
                }
                else
                {
                    TotalClaim = TotalClaim - mdlMonthlyExpenses.POLAmount.Value;

                    mdlMonthlyExpenses.POLVehicleNo = ddlVehicle.SelectedItem.Value;
                    mdlMonthlyExpenses.POLAmount = Convert.ToDouble(txtAmount.Text);
                    mdlMonthlyExpenses.POLReceiptNo = txtReceiptNo.Text;
                    mdlMonthlyExpenses.POLDatetime = Utility.GetParsedDate(txtDate.Text);
                    mdlMonthlyExpenses.POLMeterReading = txtMeterReading.Text;

                    TotalClaim = TotalClaim + mdlMonthlyExpenses.POLAmount.Value;

                    if (FileName != null)
                    {
                        mdlMonthlyExpenses.Attachment = FileName;
                    }

                    mdlMonthlyExpenses.ModifiedDate = DateTime.Now;
                    mdlMonthlyExpenses.ModifiedBy = UserID;

                    bllAccounts.UpdateMonthlyExpense(mdlMonthlyExpenses);

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