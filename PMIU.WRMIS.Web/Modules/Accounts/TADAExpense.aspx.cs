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
    public partial class TADAExpense : BasePage
    {
        #region Global Variables

        public string FinancialYear = string.Empty;
        public string Month = string.Empty;
        public long ResourceAllocationID = 0;
        public double TotalClaim = 0;

        #endregion

        #region View State Constants

        const string PMIUStaffKey = "PMIUStaff";

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
                        ltlPageTitle.Text = "Edit TADA Expense";

                        long MonthlyExpenseID = Convert.ToInt64(Request.QueryString["MonthlyExpenseID"]);
                        hdnMonthlyExpenseID.Value = MonthlyExpenseID.ToString();

                        BindExpenseForm();
                    }
                    else
                    {
                        ltlPageTitle.Text = "Add TADA Expense";
                        GetQueryStringParameters();
                        SetTopPanel();

                        lbtnBack.PostBackUrl = "~/Modules/Accounts/SearchMonthlyExpenses.aspx?LoadHistory=true";
                    }

                    IsEndReadingFieldRequired();
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
                hdnResourceAllocationID.Value = Request.QueryString["ResourceID"];
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
        /// This function makes End Reading field for resource who has been assigned 4 or 2 wheel asset.
        /// Created On 17-07-2017
        /// </summary>
        private void IsEndReadingFieldRequired()
        {
            bool IsAssigned = new AccountsBLL().IsFourOrTwoWheelVehicleAssigned(ResourceAllocationID);

            if (IsAssigned)
            {
                txtEndReading.CssClass = "form-control required decimalInput";
                txtEndReading.Attributes.Add("required", "true");
            }
        }

        protected void txtMiscExpenses_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtMiscExpenses.Text.Trim() != string.Empty)
                {
                    double MiscExpense = Convert.ToDouble(txtMiscExpenses.Text.Trim());

                    if (MiscExpense < 1 || MiscExpense > 999999)
                    {
                        txtMiscExpenses.Text = string.Empty;
                        Master.ShowMessage(Message.MiscExpenseValidValue.Description, SiteMaster.MessageType.Error);
                    }

                    CalculateTADA();

                    ApplyTADAValidation();
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtOrdinaryHalfDailies_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty)
                {
                    int OrdinaryHalfDailies = Convert.ToInt32(txtOrdinaryHalfDailies.Text.Trim());

                    if (OrdinaryHalfDailies < 1 || OrdinaryHalfDailies > 99)
                    {
                        txtOrdinaryHalfDailies.Text = string.Empty;
                        Master.ShowMessage(Message.DailiesValidValue.Description, SiteMaster.MessageType.Error);
                    }
                }

                SetDARequiredFields();

                CalculateTADA();

                ApplyTADAValidation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtOrdinaryFullDailies_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtOrdinaryFullDailies.Text.Trim() != string.Empty)
                {
                    int OrdinaryFullDailies = Convert.ToInt32(txtOrdinaryFullDailies.Text.Trim());

                    if (OrdinaryFullDailies < 1 || OrdinaryFullDailies > 99)
                    {
                        txtOrdinaryFullDailies.Text = string.Empty;
                        Master.ShowMessage(Message.DailiesValidValue.Description, SiteMaster.MessageType.Error);
                    }
                }

                SetDARequiredFields();

                CalculateTADA();

                ApplyTADAValidation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtSpecialHalfDailies_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSpecialHalfDailies.Text.Trim() != string.Empty)
                {
                    int SpecialHalfDailies = Convert.ToInt32(txtSpecialHalfDailies.Text.Trim());

                    if (SpecialHalfDailies < 1 || SpecialHalfDailies > 99)
                    {
                        txtSpecialHalfDailies.Text = string.Empty;
                        Master.ShowMessage(Message.DailiesValidValue.Description, SiteMaster.MessageType.Error);
                    }
                }

                SetDARequiredFields();

                CalculateTADA();

                ApplyTADAValidation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtSpecialFullDailies_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSpecialFullDailies.Text.Trim() != string.Empty)
                {
                    int SpecialFullDailies = Convert.ToInt32(txtSpecialFullDailies.Text.Trim());

                    if (SpecialFullDailies < 1 && SpecialFullDailies > 99)
                    {
                        txtSpecialFullDailies.Text = string.Empty;
                        Master.ShowMessage(Message.DailiesValidValue.Description, SiteMaster.MessageType.Error);
                    }
                }

                SetDARequiredFields();

                CalculateTADA();

                ApplyTADAValidation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtPublicTransport_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPublicTransport.Text.Trim() != string.Empty)
                {
                    double PublicTransport = Convert.ToDouble(txtPublicTransport.Text.Trim());

                    if (PublicTransport < 1 || PublicTransport > 99999)
                    {
                        txtPublicTransport.Text = string.Empty;
                        Master.ShowMessage(Message.DistancesValidValue.Description, SiteMaster.MessageType.Error);
                    }
                }

                SetTARequiredFields();

                CalculateTADA();

                ApplyTADAValidation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        protected void txtOfficialVehicle_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtOfficialVehicle.Text.Trim() != string.Empty)
                {
                    double OfficialVehicle = Convert.ToDouble(txtOfficialVehicle.Text.Trim());

                    if (OfficialVehicle < 1 || OfficialVehicle > 99999)
                    {
                        txtOfficialVehicle.Text = string.Empty;
                        Master.ShowMessage(Message.DistancesValidValue.Description, SiteMaster.MessageType.Error);
                    }
                }

                SetTARequiredFields();

                CalculateTADA();

                ApplyTADAValidation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function sets the required attribute on the DA fields
        /// Created On 18-05-2017
        /// </summary>
        private void SetDARequiredFields()
        {
            txtOrdinaryHalfDailies.CssClass = "form-control integerInput";
            txtOrdinaryFullDailies.CssClass = "form-control integerInput";
            txtSpecialHalfDailies.CssClass = "form-control integerInput";
            txtSpecialFullDailies.CssClass = "form-control integerInput";

            if (txtOrdinaryHalfDailies.Text.Trim() == string.Empty && txtOrdinaryFullDailies.Text.Trim() == string.Empty &&
                txtSpecialHalfDailies.Text.Trim() == string.Empty && txtSpecialFullDailies.Text.Trim() == string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
                txtOrdinaryFullDailies.CssClass = "form-control required integerInput";
                txtSpecialHalfDailies.CssClass = "form-control required integerInput";
                txtSpecialFullDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() == string.Empty &&
                txtSpecialHalfDailies.Text.Trim() == string.Empty && txtSpecialFullDailies.Text.Trim() == string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() == string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty &&
            txtSpecialHalfDailies.Text.Trim() == string.Empty && txtSpecialFullDailies.Text.Trim() == string.Empty)
            {
                txtOrdinaryFullDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() == string.Empty && txtOrdinaryFullDailies.Text.Trim() == string.Empty &&
            txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() == string.Empty)
            {
                txtSpecialHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() == string.Empty && txtOrdinaryFullDailies.Text.Trim() == string.Empty &&
            txtSpecialHalfDailies.Text.Trim() == string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                txtSpecialFullDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty &&
                txtSpecialHalfDailies.Text.Trim() == string.Empty && txtSpecialFullDailies.Text.Trim() == string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty &&
                txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() == string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty &&
                txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() == string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty &&
            txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() == string.Empty)
            {
                txtOrdinaryFullDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() == string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty &&
            txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                txtOrdinaryFullDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() == string.Empty && txtOrdinaryFullDailies.Text.Trim() == string.Empty &&
            txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                txtSpecialHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() == string.Empty &&
            txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() == string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() == string.Empty &&
            txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() == string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty &&
            txtSpecialHalfDailies.Text.Trim() == string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                txtOrdinaryFullDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() == string.Empty &&
            txtSpecialHalfDailies.Text.Trim() == string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
            }
            else if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty &&
            txtSpecialHalfDailies.Text.Trim() == string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";
            }
        }

        /// <summary>
        /// This function sets the required attribute on the TA fields
        /// Created On 18-05-2017
        /// </summary>
        private void SetTARequiredFields()
        {
            txtPublicTransport.CssClass = "form-control decimalInput";
            txtOfficialVehicle.CssClass = "form-control decimalInput";

            if (txtPublicTransport.Text.Trim() == string.Empty && txtOfficialVehicle.Text.Trim() == string.Empty)
            {
                txtPublicTransport.CssClass = "form-control required decimalInput";
                txtOfficialVehicle.CssClass = "form-control required decimalInput";
            }
            else if (txtPublicTransport.Text.Trim() != string.Empty && txtOfficialVehicle.Text.Trim() == string.Empty)
            {
                txtPublicTransport.CssClass = "form-control required decimalInput";
            }
            else if (txtPublicTransport.Text.Trim() == string.Empty && txtOfficialVehicle.Text.Trim() != string.Empty)
            {
                txtOfficialVehicle.CssClass = "form-control required decimalInput";
            }
            else if (txtPublicTransport.Text.Trim() != string.Empty && txtOfficialVehicle.Text.Trim() != string.Empty)
            {
                txtPublicTransport.CssClass = "form-control required decimalInput";
            }
        }

        /// <summary>
        /// This function calculates the TA and DA amount
        /// Created On 17-05-2017
        /// </summary>
        private void CalculateTADA()
        {
            ResourceAllocationID = Convert.ToInt64(hdnResourceAllocationID.Value);

            AT_DailyRate mdlDailyRate = new AccountsBLL().GetDailyRateByResourceID(ResourceAllocationID);

            double? DailyAllowance = null;
            double? TravelAllowance = null;
            double? TotalClaim = null;

            if (txtOrdinaryFullDailies.Text.Trim() != string.Empty)
            {
                int OrdinaryFullDailies = Convert.ToInt32(txtOrdinaryFullDailies.Text.Trim());
                double OrdinaryFullDailyAllowance = mdlDailyRate.OrdinaryRate * OrdinaryFullDailies;
                OrdinaryFullDailyAllowance = Math.Round(OrdinaryFullDailyAllowance, 2);
                DailyAllowance = OrdinaryFullDailyAllowance;
            }

            if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty)
            {
                int OrdinaryHalfDailies = Convert.ToInt32(txtOrdinaryHalfDailies.Text.Trim());
                double OrdinaryHalfDailyAllowance = (mdlDailyRate.OrdinaryRate * OrdinaryHalfDailies) / 2;
                OrdinaryHalfDailyAllowance = Math.Round(OrdinaryHalfDailyAllowance, 2);
                DailyAllowance = (DailyAllowance == null ? OrdinaryHalfDailyAllowance : DailyAllowance + OrdinaryHalfDailyAllowance);
            }

            if (txtSpecialFullDailies.Text.Trim() != string.Empty)
            {
                int SpecialFullDailies = Convert.ToInt32(txtSpecialFullDailies.Text.Trim());
                double SpecialFullDailyAllowance = mdlDailyRate.SpecialRate * SpecialFullDailies;
                SpecialFullDailyAllowance = Math.Round(SpecialFullDailyAllowance, 2);
                DailyAllowance = (DailyAllowance == null ? SpecialFullDailyAllowance : DailyAllowance + SpecialFullDailyAllowance);
            }

            if (txtSpecialHalfDailies.Text.Trim() != string.Empty)
            {
                int SpecialHalfDailies = Convert.ToInt32(txtSpecialHalfDailies.Text.Trim());
                double SpecialHalfDailyAllowance = (mdlDailyRate.SpecialRate * SpecialHalfDailies) / 2;
                SpecialHalfDailyAllowance = Math.Round(SpecialHalfDailyAllowance, 2);
                DailyAllowance = (DailyAllowance == null ? SpecialHalfDailyAllowance : DailyAllowance + SpecialHalfDailyAllowance);
            }

            double TARate = new AccountsBLL().GetAccountSetupValue((long)Constants.AccountSetup.PerKMRateForTA);

            if (txtPublicTransport.Text.Trim() != string.Empty)
            {
                double PublicTransport = Convert.ToDouble(txtPublicTransport.Text.Trim());
                TravelAllowance = Math.Round(PublicTransport * TARate, 2);
            }

            if (DailyAllowance != null)
            {
                txtDAAmount.Text = Utility.GetRoundOffValueAccounts(DailyAllowance);
                TotalClaim = DailyAllowance;
            }
            else
            {
                txtDAAmount.Text = string.Empty;
            }

            if (TravelAllowance != null)
            {
                txtTAAmount.Text = Utility.GetRoundOffValueAccounts(TravelAllowance);
                TotalClaim = (TotalClaim == null ? TravelAllowance : TotalClaim + TravelAllowance);
            }
            else
            {
                txtTAAmount.Text = string.Empty;
            }

            if (txtMiscExpenses.Text.Trim() != string.Empty)
            {
                double MiscExpenses = Convert.ToDouble(txtMiscExpenses.Text.Trim());
                TotalClaim = (TotalClaim == null ? MiscExpenses : TotalClaim + MiscExpenses);
            }

            if (TotalClaim != null)
            {
                txtClaimAmount.Text = Utility.GetRoundOffValueAccounts(TotalClaim);
            }
            else
            {
                txtClaimAmount.Text = string.Empty;
            }
        }

        /// <summary>
        /// This function removes required check from TADA fields.
        /// Created On 17-05-2017 
        /// </summary>
        private void ApplyTADAValidation()
        {
            if (!txtOrdinaryFullDailies.CssClass.Contains("required"))
            {
                txtOrdinaryFullDailies.Attributes.Remove("required");
            }

            if (!txtOrdinaryHalfDailies.CssClass.Contains("required"))
            {
                txtOrdinaryHalfDailies.Attributes.Remove("required");
            }

            if (!txtSpecialFullDailies.CssClass.Contains("required"))
            {
                txtSpecialFullDailies.Attributes.Remove("required");
            }

            if (!txtSpecialHalfDailies.CssClass.Contains("required"))
            {
                txtSpecialHalfDailies.Attributes.Remove("required");
            }

            if (!txtPublicTransport.CssClass.Contains("required"))
            {
                txtPublicTransport.Attributes.Remove("required");
            }

            if (!txtOfficialVehicle.CssClass.Contains("required"))
            {
                txtOfficialVehicle.Attributes.Remove("required");
            }
        }

        /// <summary>
        /// This function generates bill number
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

            BillNo = string.Format("{0}{1}-", Prefix, "TA");

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

            hdnResourceAllocationID.Value = mdlMonthlyExpenses.ResourceAllocationID.Value.ToString();

            SetTopPanel();

            lbtnBack.PostBackUrl = string.Format("~/Modules/Accounts/ExpenseDetails.aspx?FinancialYear={0}&Month={1}&ResourceID={2}&TotalClaim={3}&ExpenseTypeID={4}", mdlMonthlyExpenses.FinancialYear, mdlMonthlyExpenses.Month, mdlMonthlyExpenses.ResourceAllocationID, TotalClaim, mdlMonthlyExpenses.ExpenseTypeID);

            lblTDABillNo.Text = mdlMonthlyExpenses.BillNo;
            dvBillNo.Visible = true;

            txtEndReading.Text = mdlMonthlyExpenses.MonthEndReading;
            txtMiscExpenses.Text = Utility.GetRoundOffValueAccounts(mdlMonthlyExpenses.MiscExpenditures);

            if (mdlMonthlyExpenses.OrdinaryDailiesHalf.HasValue)
            {
                txtOrdinaryHalfDailies.Text = mdlMonthlyExpenses.OrdinaryDailiesHalf.Value.ToString();
            }
            else
            {
                txtOrdinaryHalfDailies.Text = string.Empty;
            }

            if (mdlMonthlyExpenses.OrdinaryDailiesFull.HasValue)
            {
                txtOrdinaryFullDailies.Text = mdlMonthlyExpenses.OrdinaryDailiesFull.Value.ToString();
            }
            else
            {
                txtOrdinaryFullDailies.Text = string.Empty;
            }

            if (mdlMonthlyExpenses.SpecialDailiesHalf.HasValue)
            {
                txtSpecialHalfDailies.Text = mdlMonthlyExpenses.SpecialDailiesHalf.Value.ToString();
            }
            else
            {
                txtSpecialHalfDailies.Text = string.Empty;
            }

            if (mdlMonthlyExpenses.SpecialDailiesFull.HasValue)
            {
                txtSpecialFullDailies.Text = mdlMonthlyExpenses.SpecialDailiesFull.Value.ToString();
            }
            else
            {
                txtSpecialFullDailies.Text = string.Empty;
            }

            if (mdlMonthlyExpenses.TotalKMPublicTransport.HasValue)
            {
                txtPublicTransport.Text = mdlMonthlyExpenses.TotalKMPublicTransport.Value.ToString();
            }
            else
            {
                txtPublicTransport.Text = string.Empty;
            }

            if (mdlMonthlyExpenses.TotalKMOfficialVehicle.HasValue)
            {
                txtOfficialVehicle.Text = mdlMonthlyExpenses.TotalKMOfficialVehicle.Value.ToString();
            }
            else
            {
                txtOfficialVehicle.Text = string.Empty;
            }

            SetDARequiredFields();
            SetTARequiredFields();
            ApplyTADAValidation();

            CalculateTADA();

            if (mdlMonthlyExpenses.Attachment != null)
            {
                string FileName = new System.IO.FileInfo(mdlMonthlyExpenses.Attachment).Name;
                List<string> lstName = new List<string> { FileName };

                fupAttachTADAProformaEdit.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                fupAttachTADAProformaEdit.Size = 1;
                fupAttachTADAProformaEdit.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GetQueryStringParameters();

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                int UserID = Convert.ToInt32(mdlUsers.ID);

                List<Tuple<string, string, string>> lstNameofFiles = fupAttachTADAProforma.UploadNow(Configuration.Accounts);

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
                        ExpenseTypeID = (long)Constants.ExpenseType.TADA,
                        BillNo = GenerateBillNo(),
                        MonthEndReading = txtEndReading.Text.Trim(),
                        OrdinaryDailiesHalf = (txtOrdinaryHalfDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtOrdinaryHalfDailies.Text.Trim())),
                        OrdinaryDailiesFull = (txtOrdinaryFullDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtOrdinaryFullDailies.Text.Trim())),
                        SpecialDailiesHalf = (txtSpecialHalfDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtSpecialHalfDailies.Text.Trim())),
                        SpecialDailiesFull = (txtSpecialFullDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtSpecialFullDailies.Text.Trim())),
                        TotalKMPublicTransport = (txtPublicTransport.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtPublicTransport.Text.Trim())),
                        TotalKMOfficialVehicle = (txtOfficialVehicle.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtOfficialVehicle.Text.Trim())),
                        MiscExpenditures = (txtMiscExpenses.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtMiscExpenses.Text.Trim())),
                        Attachment = FileName,
                        DAAmount = Convert.ToDouble(txtDAAmount.Text.Trim()),
                        TAAmount = (txtTAAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtTAAmount.Text.Trim())),
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
                    TotalClaim = TotalClaim - (mdlMonthlyExpenses.DAAmount.Value + (mdlMonthlyExpenses.TAAmount.HasValue ? mdlMonthlyExpenses.TAAmount.Value : 0) + (mdlMonthlyExpenses.MiscExpenditures.HasValue ? mdlMonthlyExpenses.MiscExpenditures.Value : 0));

                    mdlMonthlyExpenses.MonthEndReading = txtEndReading.Text.Trim();
                    mdlMonthlyExpenses.OrdinaryDailiesHalf = (txtOrdinaryHalfDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtOrdinaryHalfDailies.Text.Trim()));
                    mdlMonthlyExpenses.OrdinaryDailiesFull = (txtOrdinaryFullDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtOrdinaryFullDailies.Text.Trim()));
                    mdlMonthlyExpenses.SpecialDailiesHalf = (txtSpecialHalfDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtSpecialHalfDailies.Text.Trim()));
                    mdlMonthlyExpenses.SpecialDailiesFull = (txtSpecialFullDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtSpecialFullDailies.Text.Trim()));
                    mdlMonthlyExpenses.TotalKMPublicTransport = (txtPublicTransport.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtPublicTransport.Text.Trim()));
                    mdlMonthlyExpenses.TotalKMOfficialVehicle = (txtOfficialVehicle.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtOfficialVehicle.Text.Trim()));
                    mdlMonthlyExpenses.MiscExpenditures = (txtMiscExpenses.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtMiscExpenses.Text.Trim()));
                    mdlMonthlyExpenses.DAAmount = Convert.ToDouble(txtDAAmount.Text.Trim());
                    mdlMonthlyExpenses.TAAmount = (txtTAAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtTAAmount.Text.Trim()));

                    TotalClaim = TotalClaim + (mdlMonthlyExpenses.DAAmount.Value + (mdlMonthlyExpenses.TAAmount.HasValue ? mdlMonthlyExpenses.TAAmount.Value : 0) + (mdlMonthlyExpenses.MiscExpenditures.HasValue ? mdlMonthlyExpenses.MiscExpenditures.Value : 0));

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