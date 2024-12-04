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
    public partial class AddEditExpenses : BasePage
    {
        #region Screen Constants

        const string RepairMaintainence = "RM";
        const string POLReceipts = "POL";
        const string TADA = "TA";
        const string NewPurchase = "NP";
        const string OtherExpense = "OE";
        const string NewRow = "New Row";

        #endregion

        #region Global Variables

        public string FinancialYear = string.Empty;
        public string Month = string.Empty;
        public long ResourceAllocationID = 0;
        public string TotalClaim = string.Empty;

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
                    BindExpenseTypeDropdown();

                    if (!string.IsNullOrEmpty(Request.QueryString["MonthlyExpenseID"]))
                    {
                        ltlPageTitle.Text = "Edit Expenses";

                        long MonthlyExpenseID = Convert.ToInt64(Request.QueryString["MonthlyExpenseID"]);
                        hdnMonthlyExpenseID.Value = MonthlyExpenseID.ToString();

                        BindExpenseForm();
                    }
                    else
                    {
                        ltlPageTitle.Text = "Edit Expenses";
                        GetQueryStringParameters();
                        SetTopPanel();

                        ddlNoOfQuotations_SelectedIndexChanged(null, null);

                        lbtnBack.PostBackUrl = "~/Modules/Accounts/SearchMonthlyExpenses.aspx?LoadHistory=true";
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
        /// Created On 07-04-2017
        /// </summary>
        private void SetPageTitle()
        {
            Tuple<string, string, string> pageTitle = base.SetPageTitle(PageName.Accounts);
            Master.ModuleTitle = pageTitle.Item1;
            Master.PageTitle = pageTitle.Item2;
            Master.NavigationBar = pageTitle.Item3;
        }

        /// <summary>
        /// This function binds the expense type dropdown
        /// Created On 11-04-2017 
        /// </summary>
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

        protected void ddlExpenseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetControls();
                GetQueryStringParameters();

                long ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value);

                btnSave.Visible = (hdnMonthlyExpenseID.Value.Trim() != "0" ? base.CanEdit : base.CanAdd);

                if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
                {
                    BindAssetDropdown();
                    BindUnAssignedKeyParts();
                    BindAssignedKeyParts();

                    pnlRepairMaintainence.Visible = true;
                }
                else if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
                {
                    BindVehicleDropdown();

                    pnlPOLReceipts.Visible = true;
                }
                else if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
                {
                    pnlTADA.Visible = true;
                }
                else if (ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
                {
                    BindObjectClassificationDropdown(0);
                    dvObjectClassification.Visible = true;

                    pnlNewPurchase.Visible = true;
                }
                else if (ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense)
                {
                    BindObjectClassificationDropdown(0);
                    dvObjectClassification.Visible = true;

                    pnlOtherExpense.Visible = true;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function hides all the dependent controls and resets the screen
        /// Created On 13-04-2017
        /// </summary>
        private void ResetControls()
        {
            dvObjectClassification.Visible = false;
            pnlRepairMaintainence.Visible = false;
            pnlPOLReceipts.Visible = false;
            pnlTADA.Visible = false;
            pnlNewPurchase.Visible = false;
            pnlOtherExpense.Visible = false;
            pnlQuotation.Visible = false;
            btnSave.Visible = false;

            ddlNoOfQuotations_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// This function binds the object classification dropdown
        /// Created On 13-04-2017
        /// </summary>
        /// <param name="_AccountHeadID"></param>
        private void BindObjectClassificationDropdown(long _AccountHeadID)
        {
            Dropdownlist.BindDropdownlist<List<dynamic>>(ddlObjectClassification, new ReferenceDataBLL().GetObjectClassificationByAccoundHeadID(_AccountHeadID), (int)Constants.DropDownFirstOption.Select, "ObjectClassification", "ID");
        }

        /// <summary>
        /// This function binds the vehicle dropdown
        /// Created On 13-04-2017
        /// </summary>
        private void BindVehicleDropdown()
        {
            List<long> AssetTypeIDs = new List<long>() { (long)Constants.AssetType.FourWheelVehicle, (long)Constants.AssetType.TwoWheelVehicle };

            Dropdownlist.DDLResourceAssets(ddlVehicle, ResourceAllocationID, AssetTypeIDs);
        }

        /// <summary>
        /// This function binds the asset dropdown
        /// Created On 04-05-2017
        /// </summary>
        private void BindAssetDropdown()
        {
            Dropdownlist.DDLResourceAssets(ddlAsset, ResourceAllocationID, null, (int)Constants.DropDownFirstOption.Select, "AssetName", "AssetAllocationID");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GetQueryStringParameters();

                long ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value);

                UA_Users mdlUsers = SessionManagerFacade.UserInformation;

                int UserID = Convert.ToInt32(mdlUsers.ID);

                bool IsQuotationRequired = false;

                if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
                {
                    ApplyRMValidation();

                    if (!ValidateRepairMaintainenceTotal())
                    {
                        return;
                    }

                    IsQuotationRequired = SaveRepairMaintainenceBill(UserID);
                }
                else if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
                {
                    SavePOLReceipts(UserID);
                }
                else if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
                {
                    ApplyTADAValidation();

                    if (!ValidateDAAmount())
                    {
                        return;
                    }

                    SaveTADABills(UserID);
                }
                else if (ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
                {
                    IsQuotationRequired = SaveNewPurchaseBill(UserID);
                }
                else if (ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense)
                {
                    IsQuotationRequired = SaveOtherExpensesBill(UserID);
                }

                if (!IsQuotationRequired)
                {
                    if (hdnMonthlyExpenseID.Value.Trim() == "0")
                    {
                        Response.Redirect("SearchMonthlyExpenses.aspx?LoadHistory=true&RecordSaved=true", false);
                    }
                    else
                    {
                        string Url = string.Format("ExpenseDetails.aspx?FinancialYear={0}&Month={1}&ResourceID={2}&TotalClaim={3}&ExpenseTypeID={4}&RecordSaved={5}", lblFinancialYear.Text, lblMonth.Text, ResourceAllocationID, TotalClaim, 1);
                        Response.Redirect(Url, false);
                    }
                }
            }
            catch (Exception exp)
            {
                Master.ShowMessage(Message.RecordNotSaved.Description, SiteMaster.MessageType.Error);
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function saves the POL Receipts
        /// Created On 14-04-2017
        /// </summary>
        private void SavePOLReceipts(int _UserID)
        {
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
                    ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value),
                    BillNo = GenerateBillNo(),
                    POLVehicleNo = ddlVehicle.SelectedItem.Value,
                    POLAmount = Convert.ToDouble(txtAmount.Text),
                    POLReceiptNo = txtReceiptNo.Text,
                    POLDatetime = Utility.GetParsedDate(txtDate.Text),
                    POLMeterReading = txtMeterReading.Text,
                    Attachment = FileName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = _UserID,
                    ModifiedBy = _UserID
                };

                bllAccounts.AddMonthlyExpense(mdlMonthlyExpenses);
            }
            else
            {
                mdlMonthlyExpenses.POLVehicleNo = ddlVehicle.SelectedItem.Value;
                mdlMonthlyExpenses.POLAmount = Convert.ToDouble(txtAmount.Text);
                mdlMonthlyExpenses.POLReceiptNo = txtReceiptNo.Text;
                mdlMonthlyExpenses.POLDatetime = Utility.GetParsedDate(txtDate.Text);
                mdlMonthlyExpenses.POLMeterReading = txtMeterReading.Text;

                if (FileName != null)
                {
                    mdlMonthlyExpenses.Attachment = FileName;
                }

                mdlMonthlyExpenses.ModifiedDate = DateTime.Now;
                mdlMonthlyExpenses.ModifiedBy = _UserID;

                bllAccounts.UpdateMonthlyExpense(mdlMonthlyExpenses);
            }
        }

        /// <summary>
        /// This function generates bill number
        /// Created On 24-04-2017
        /// </summary>
        /// <returns>string</returns>
        private string GenerateBillNo()
        {
            long ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value);
            DateTime Now = DateTime.Now;

            string Prefix = string.Format("{0}{1}-", Now.ToString("yy"), Now.Month.ToString("00"));
            string BillNo = string.Empty;

            if (ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
            {
                BillNo = string.Format("{0}{1}-", Prefix, RepairMaintainence);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
            {
                BillNo = string.Format("{0}{1}-", Prefix, POLReceipts);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.TADA)
            {
                BillNo = string.Format("{0}{1}-", Prefix, TADA);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
            {
                BillNo = string.Format("{0}{1}-", Prefix, NewPurchase);
            }
            else if (ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense)
            {
                BillNo = string.Format("{0}{1}-", Prefix, OtherExpense);
            }

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
        /// This function fetches the Query string parameters.
        /// Created On 21-04-2017
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
                TotalClaim = Request.QueryString["TotalClaim"].ToString();
            }
        }

        /// <summary>
        /// This function sets data in top panel
        /// Created on 21-04-2017
        /// </summary>
        private void SetTopPanel()
        {
            lblFinancialYear.Text = FinancialYear;
            lblMonth.Text = Month;
            lblTotalClaim.Text = TotalClaim;

            AT_ResourceAllocation mdlResourceAllocation = new AccountsBLL().GetResourceByID(ResourceAllocationID);

            ViewState[PMIUStaffKey] = mdlResourceAllocation.PMIUFieldOffice;

            lblResourceType.Text = (mdlResourceAllocation.PMIUFieldOffice == "F" ? "Field" : "Office");
            lblDesignation.Text = mdlResourceAllocation.UA_Designations.Name;
            lblNameOfStaff.Text = mdlResourceAllocation.StaffUserName;
        }

        /// <summary>
        /// This function binds the unassigned key parts
        /// Created On 27-04-2017
        /// </summary>
        /// <param name="_ExpenseID"></param>
        private void BindUnAssignedKeyParts(long? _ExpenseID = null)
        {
            List<dynamic> lstUnAssignedParts = new AccountsBLL().BindUnAssignedKeyParts(0, _ExpenseID);
            lbKeyPart.DataTextField = "Name";
            lbKeyPart.DataValueField = "ID";
            lbKeyPart.DataSource = lstUnAssignedParts;
            lbKeyPart.DataBind();
        }

        /// <summary>
        /// This function binds the assigned key parts
        /// Created On 27-04-2017
        /// </summary>
        /// <param name="_ExpenseID"></param>
        private void BindAssignedKeyParts(long? _ExpenseID = null)
        {
            List<dynamic> lstAssignedParts = new AccountsBLL().BindAssignedKeyParts(0, _ExpenseID);
            lbAssignedKeyPart.DataTextField = "Name";
            lbAssignedKeyPart.DataValueField = "ID";
            lbAssignedKeyPart.DataSource = lstAssignedParts;
            lbAssignedKeyPart.DataBind();
        }

        protected void ddlNoOfQuotations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ApplyRMValidation();

                int NoOfQuotations = Convert.ToInt32(ddlNoOfQuotations.SelectedItem.Value);

                List<string> lstQuotationRows = new List<string>();

                for (int i = 1; i <= NoOfQuotations; i++)
                {
                    lstQuotationRows.Add(NewRow);
                }

                gvQuotation.DataSource = lstQuotationRows;
                gvQuotation.DataBind();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function saves the repair and maintainence bill
        /// Created On 05-05-2017
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>bool</returns>
        private bool SaveRepairMaintainenceBill(int _UserID)
        {
            bool IsQuotationRequired = false;

            if (pnlQuotation.Visible)
            {
                long MonthlyExpenseID = SaveRepairMaintainenceData(_UserID);
                SaveQuotationData(MonthlyExpenseID, _UserID);
            }
            else
            {
                double QuotationLimit = new AccountsBLL().GetAccountSetupValue((long)Constants.AccountSetup.ExpenseLimitForQuotations);

                double TotalAmount = Convert.ToDouble(txtTotalAmount.Text);

                if (TotalAmount > QuotationLimit)
                {
                    pnlQuotation.Visible = true;
                    IsQuotationRequired = true;
                }
                else
                {
                    pnlQuotation.Visible = false;
                    SaveRepairMaintainenceData(_UserID);
                }
            }

            return IsQuotationRequired;
        }

        /// <summary>
        /// This function saves the new purchase bill
        /// Created On 10-05-2017
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>bool</returns>
        private bool SaveNewPurchaseBill(int _UserID)
        {
            bool IsQuotationRequired = false;

            if (pnlQuotation.Visible)
            {
                long MonthlyExpenseID = SaveNewPurchaseData(_UserID);
                SaveQuotationData(MonthlyExpenseID, _UserID);
            }
            else
            {
                double QuotationLimit = new AccountsBLL().GetAccountSetupValue((long)Constants.AccountSetup.ExpenseLimitForQuotations);

                double PurchaseAmount = Convert.ToDouble(txtNPPurchaseAmount.Text);

                if (PurchaseAmount > QuotationLimit)
                {
                    pnlQuotation.Visible = true;
                    IsQuotationRequired = true;
                }
                else
                {
                    pnlQuotation.Visible = false;
                    SaveNewPurchaseData(_UserID);
                }
            }

            return IsQuotationRequired;
        }

        /// <summary>
        /// This function saves the other expense bill
        /// Created On 10-05-2017
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>bool</returns>
        private bool SaveOtherExpensesBill(int _UserID)
        {
            bool IsQuotationRequired = false;

            if (pnlQuotation.Visible)
            {
                long MonthlyExpenseID = SaveOtherExpenseData(_UserID);
                SaveQuotationData(MonthlyExpenseID, _UserID);
            }
            else
            {
                double QuotationLimit = new AccountsBLL().GetAccountSetupValue((long)Constants.AccountSetup.ExpenseLimitForQuotations);

                double TotalAmount = Convert.ToDouble(txtExpenseAmount.Text);

                if (TotalAmount > QuotationLimit)
                {
                    pnlQuotation.Visible = true;
                    IsQuotationRequired = true;
                }
                else
                {
                    pnlQuotation.Visible = false;
                    SaveOtherExpenseData(_UserID);
                }
            }

            return IsQuotationRequired;
        }

        /// <summary>
        /// This function saves the repair and maintainence part data along with key parts
        /// Created On 08-05-2017
        /// </summary>
        private long SaveRepairMaintainenceData(int _UserID)
        {
            AccountsBLL bllAccounts = new AccountsBLL();

            List<Tuple<string, string, string>> lstNameofFiles = fupRMAttachBill.UploadNow(Configuration.Accounts);

            string FileName = null;

            if (lstNameofFiles.Count > 0)
            {
                FileName = lstNameofFiles[0].Item3.ToString();
            }

            AT_MonthlyExpenses mdlMonthlyExpenses = new AT_MonthlyExpenses()
            {
                ResourceAllocationID = ResourceAllocationID,
                PMIUFieldOffice = ViewState[PMIUStaffKey].ToString(),
                FinancialYear = FinancialYear,
                Month = Month,
                ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value),
                BillNo = GenerateBillNo(),
                BillDate = (txtRMBillDate.Text.Trim() == string.Empty ? (DateTime?)null : Utility.GetParsedDate(txtRMBillDate.Text)),
                AssetAllocationID = Convert.ToInt64(ddlAsset.SelectedItem.Value),
                RepairAmount = (txtRepairAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtRepairAmount.Text)),
                PurchaseAmount = (txtRMPurchaseAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtRMPurchaseAmount.Text)),
                ApprovalReference = txtRMApprovalReference.Text.Trim(),
                Attachment = FileName,
                CreatedBy = _UserID,
                ModifiedBy = _UserID,
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
                    CreatedBy = _UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = _UserID,
                    ModifiedDate = DateTime.Now
                };

                lstPartExpense.Add(mdlPartExpense);
            }

            if (lstPartExpense.Count > 0)
            {
                bllAccounts.SaveKeyPartsOfExpense(lstPartExpense);
            }

            return mdlMonthlyExpenses.ID;
        }

        /// <summary>
        /// This function save New Purchase Data
        /// Created On 10-05-2017
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>long</returns>
        private long SaveNewPurchaseData(int _UserID)
        {
            List<Tuple<string, string, string>> lstNameofFiles = fupNPAttachBill.UploadNow(Configuration.Accounts);

            string FileName = null;

            if (lstNameofFiles.Count > 0)
            {
                FileName = lstNameofFiles[0].Item3.ToString();
            }

            AT_MonthlyExpenses mdlMonthlyExpenses = new AT_MonthlyExpenses()
            {
                ResourceAllocationID = ResourceAllocationID,
                PMIUFieldOffice = ViewState[PMIUStaffKey].ToString(),
                FinancialYear = FinancialYear,
                Month = Month,
                ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value),
                BillNo = GenerateBillNo(),
                BillDate = Utility.GetParsedDate(txtNPBillDate.Text),
                ObjectClassificationID = Convert.ToInt64(ddlObjectClassification.SelectedItem.Value),
                PurchasedItemName = txtPurchaseItemName.Text.Trim(),
                PurchaseAmount = Convert.ToDouble(txtNPPurchaseAmount.Text),
                ApprovalReference = txtNPApprovalReference.Text.Trim(),
                Attachment = FileName,
                CreatedBy = _UserID,
                ModifiedBy = _UserID,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            new AccountsBLL().AddMonthlyExpense(mdlMonthlyExpenses);

            return mdlMonthlyExpenses.ID;
        }

        /// <summary>
        /// This function save the other expense part data
        /// Created On 10-05-2017
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>long</returns>
        private long SaveOtherExpenseData(int _UserID)
        {
            List<Tuple<string, string, string>> lstNameofFiles = fupOEAttachBill.UploadNow(Configuration.Accounts);

            string FileName = null;

            if (lstNameofFiles.Count > 0)
            {
                FileName = lstNameofFiles[0].Item3.ToString();
            }

            AT_MonthlyExpenses mdlMonthlyExpenses = new AT_MonthlyExpenses()
            {
                ResourceAllocationID = ResourceAllocationID,
                PMIUFieldOffice = ViewState[PMIUStaffKey].ToString(),
                FinancialYear = FinancialYear,
                Month = Month,
                ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value),
                BillNo = GenerateBillNo(),
                BillDate = Utility.GetParsedDate(txtOEBillDate.Text),
                ObjectClassificationID = Convert.ToInt64(ddlObjectClassification.SelectedItem.Value),
                ExpenseName = txtExpenseName.Text.Trim(),
                ExpenseAmount = (txtExpenseAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtExpenseAmount.Text)),
                ApprovalReference = txtOEApprovalReference.Text.Trim(),
                Attachment = FileName,
                CreatedBy = _UserID,
                ModifiedBy = _UserID,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            new AccountsBLL().AddMonthlyExpense(mdlMonthlyExpenses);

            return mdlMonthlyExpenses.ID;
        }

        /// <summary>
        /// This function saves the TADA Receipts
        /// Created On 12-05-2017
        /// </summary>
        private void SaveTADABills(int _UserID)
        {
            List<Tuple<string, string, string>> lstNameofFiles = fupAttachTADAProforma.UploadNow(Configuration.Accounts);

            string FileName = null;

            if (lstNameofFiles.Count > 0)
            {
                FileName = lstNameofFiles[0].Item3.ToString();
            }

            AT_MonthlyExpenses mdlMonthlyExpenses = new AT_MonthlyExpenses()
            {
                ResourceAllocationID = ResourceAllocationID,
                PMIUFieldOffice = ViewState[PMIUStaffKey].ToString(),
                FinancialYear = FinancialYear,
                Month = Month,
                ExpenseTypeID = Convert.ToInt64(ddlExpenseType.SelectedItem.Value),
                BillNo = GenerateBillNo(),
                MonthEndReading = txtEndReading.Text.Trim(),
                OrdinaryDailiesHalf = (txtOrdinaryHalfDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtOrdinaryHalfDailies.Text.Trim())),
                OrdinaryDailiesFull = (txtOrdinaryFullDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtOrdinaryFullDailies.Text.Trim())),
                SpecialDailiesHalf = (txtSpecialHalfDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtSpecialHalfDailies.Text.Trim())),
                SpecialDailiesFull = (txtSpecialFullDailies.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtSpecialFullDailies.Text.Trim())),
                TotalKMPublicTransport = (txtPublicTransport.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtPublicTransport.Text.Trim())),
                TotalKMOfficialVehicle = (txtOfficialVehicle.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(txtOfficialVehicle.Text.Trim())),
                MiscExpenditures = (txtMiscExpenses.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtMiscExpenses.Text.Trim())),
                Attachment = FileName,
                DAAmount = Convert.ToDouble(txtDAAmount.Text.Trim()),
                TAAmount = (txtTAAmount.Text.Trim() == string.Empty ? (double?)null : Convert.ToDouble(txtTAAmount.Text.Trim())),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = _UserID,
                ModifiedBy = _UserID
            };

            new AccountsBLL().AddMonthlyExpense(mdlMonthlyExpenses);
        }

        protected void btnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ApplyRMValidation();

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
                ApplyRMValidation();

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
                if (txtRMPurchaseAmount.Text.Trim() != string.Empty)
                {
                    txtRepairAmount.Attributes.Remove("required");
                    txtRepairAmount.CssClass = "form-control decimal2PInput";
                }
                else
                {
                    txtRepairAmount.Attributes.Add("required", "true");
                    txtRepairAmount.CssClass = "form-control required decimal2PInput";
                }

                double PurchaseAmount = 0;
                double RepairAmount = 0;
                bool ShowTotal = false;

                if (txtRMPurchaseAmount.Text.Trim() != string.Empty)
                {
                    PurchaseAmount = Convert.ToDouble(txtRMPurchaseAmount.Text.Trim());
                    ShowTotal = true;
                }

                if (txtRepairAmount.Text.Trim() != string.Empty)
                {
                    RepairAmount = Convert.ToDouble(txtRepairAmount.Text.Trim());
                    ShowTotal = true;
                }

                double TotalAmount = PurchaseAmount + RepairAmount;

                if (ShowTotal)
                {
                    txtTotalAmount.Text = TotalAmount.ToString();
                }
                else
                {
                    txtTotalAmount.Text = string.Empty;
                }
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
                if (txtRepairAmount.Text.Trim() != string.Empty)
                {
                    txtRMPurchaseAmount.Attributes.Remove("required");
                    txtRMPurchaseAmount.CssClass = "form-control decimal2PInput";
                }
                else
                {
                    txtRMPurchaseAmount.Attributes.Add("required", "true");
                    txtRMPurchaseAmount.CssClass = "form-control required decimal2PInput";
                }

                double PurchaseAmount = 0;
                double RepairAmount = 0;
                bool ShowTotal = false;

                if (txtRMPurchaseAmount.Text.Trim() != string.Empty)
                {
                    PurchaseAmount = Convert.ToDouble(txtRMPurchaseAmount.Text.Trim());
                    ShowTotal = true;
                }

                if (txtRepairAmount.Text.Trim() != string.Empty)
                {
                    RepairAmount = Convert.ToDouble(txtRepairAmount.Text.Trim());
                    ShowTotal = true;
                }

                double TotalAmount = PurchaseAmount + RepairAmount;

                if (ShowTotal)
                {
                    txtTotalAmount.Text = TotalAmount.ToString();
                }
                else
                {
                    txtTotalAmount.Text = string.Empty;
                }
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function saves the quotation data.
        /// Created On 09-05-2017
        /// </summary>
        /// <param name="_MonthlyExpenseID"></param>
        /// <param name="_UserID"></param>
        private void SaveQuotationData(long _MonthlyExpenseID, int _UserID)
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

                List<Tuple<string, string, string>> lstNameofFiles = fupAttachQuotation.UploadNow();

                string FileName = null;

                if (lstNameofFiles.Count > 0)
                {
                    FileName = lstNameofFiles[0].Item3.ToString();
                }

                mdlExpenseQuotation = new AT_ExpenseQuotation
                {
                    VendorName = txtVendorName.Text,
                    QuotationDate = Utility.GetParsedDate(txtQuotationDate.Text),
                    QuotedPrice = Convert.ToDouble(txtQuotedPrice.Text),
                    QuotedPriceWithTax = Convert.ToDouble(txtQuotedTaxPrice.Text),
                    QuotedPriceAttachment = FileName,
                    MonthlyExpenseID = _MonthlyExpenseID,
                    CreatedBy = _UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = _UserID,
                    ModifiedDate = DateTime.Now
                };

                lstExpenseQuotation.Add(mdlExpenseQuotation);
            }

            new AccountsBLL().SaveQuotationsOfExpense(lstExpenseQuotation);
        }

        /// <summary>
        /// This function calculates the TA and DA amount
        /// Created On 12-05-2017
        /// </summary>
        private void CalculateTADA()
        {
            GetQueryStringParameters();

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
                txtDAAmount.Text = DailyAllowance.ToString();
                TotalClaim = DailyAllowance;
            }
            else
            {
                txtDAAmount.Text = string.Empty;
            }

            if (TravelAllowance != null)
            {
                txtTAAmount.Text = TravelAllowance.ToString();
                TotalClaim = (TotalClaim == null ? TravelAllowance : TotalClaim + TravelAllowance);
            }
            else
            {
                txtTAAmount.Text = string.Empty;
            }

            if (TotalClaim != null)
            {
                txtClaimAmount.Text = TotalClaim.ToString();
            }
            else
            {
                txtClaimAmount.Text = string.Empty;
            }
        }

        protected void txtOrdinaryHalfDailies_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty)
                {
                    txtOrdinaryFullDailies.Attributes.Remove("required");
                    txtOrdinaryFullDailies.CssClass = "form-control integerInput";

                    txtSpecialHalfDailies.Attributes.Remove("required");
                    txtSpecialHalfDailies.CssClass = "form-control integerInput";

                    txtSpecialFullDailies.Attributes.Remove("required");
                    txtSpecialFullDailies.CssClass = "form-control integerInput";
                }
                else
                {
                    if (txtOrdinaryFullDailies.Text.Trim() != string.Empty && txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialHalfDailies.Text.Trim() != string.Empty)
                    {
                        txtOrdinaryFullDailies.Attributes.Add("required", "true");
                        txtOrdinaryFullDailies.CssClass = "form-control required integerInput";

                        txtSpecialHalfDailies.Attributes.Add("required", "true");
                        txtSpecialHalfDailies.CssClass = "form-control required integerInput";

                        txtSpecialFullDailies.Attributes.Add("required", "true");
                        txtSpecialFullDailies.CssClass = "form-control required integerInput";
                    }
                }

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
                    txtOrdinaryHalfDailies.Attributes.Remove("required");
                    txtOrdinaryHalfDailies.CssClass = "form-control integerInput";

                    txtSpecialHalfDailies.Attributes.Remove("required");
                    txtSpecialHalfDailies.CssClass = "form-control integerInput";

                    txtSpecialFullDailies.Attributes.Remove("required");
                    txtSpecialFullDailies.CssClass = "form-control integerInput";
                }
                else
                {
                    if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtSpecialHalfDailies.Text.Trim() != string.Empty && txtSpecialHalfDailies.Text.Trim() != string.Empty)
                    {
                        txtOrdinaryHalfDailies.Attributes.Add("required", "true");
                        txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";

                        txtSpecialHalfDailies.Attributes.Add("required", "true");
                        txtSpecialHalfDailies.CssClass = "form-control required integerInput";

                        txtSpecialFullDailies.Attributes.Add("required", "true");
                        txtSpecialFullDailies.CssClass = "form-control required integerInput";
                    }
                }

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
                    txtOrdinaryHalfDailies.Attributes.Remove("required");
                    txtOrdinaryHalfDailies.CssClass = "form-control integerInput";

                    txtOrdinaryFullDailies.Attributes.Remove("required");
                    txtOrdinaryFullDailies.CssClass = "form-control integerInput";

                    txtSpecialFullDailies.Attributes.Remove("required");
                    txtSpecialFullDailies.CssClass = "form-control integerInput";
                }
                else
                {
                    if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty && txtSpecialFullDailies.Text.Trim() != string.Empty)
                    {
                        txtOrdinaryHalfDailies.Attributes.Add("required", "true");
                        txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";

                        txtOrdinaryFullDailies.Attributes.Add("required", "true");
                        txtOrdinaryFullDailies.CssClass = "form-control required integerInput";

                        txtSpecialFullDailies.Attributes.Add("required", "true");
                        txtSpecialFullDailies.CssClass = "form-control required integerInput";
                    }
                }

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
                    txtOrdinaryHalfDailies.Attributes.Remove("required");
                    txtOrdinaryHalfDailies.CssClass = "form-control integerInput";

                    txtOrdinaryFullDailies.Attributes.Remove("required");
                    txtOrdinaryFullDailies.CssClass = "form-control integerInput";

                    txtSpecialHalfDailies.Attributes.Remove("required");
                    txtSpecialHalfDailies.CssClass = "form-control integerInput";
                }
                else
                {
                    if (txtOrdinaryHalfDailies.Text.Trim() != string.Empty && txtOrdinaryFullDailies.Text.Trim() != string.Empty && txtSpecialHalfDailies.Text.Trim() != string.Empty)
                    {
                        txtOrdinaryHalfDailies.Attributes.Add("required", "true");
                        txtOrdinaryHalfDailies.CssClass = "form-control required integerInput";

                        txtOrdinaryFullDailies.Attributes.Add("required", "true");
                        txtOrdinaryFullDailies.CssClass = "form-control required integerInput";

                        txtSpecialHalfDailies.Attributes.Add("required", "true");
                        txtSpecialHalfDailies.CssClass = "form-control required integerInput";
                    }
                }

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
                    txtOfficialVehicle.Attributes.Remove("required");
                    txtOfficialVehicle.CssClass = "form-control integerInput";
                }
                else
                {
                    txtOfficialVehicle.Attributes.Add("required", "true");
                    txtOfficialVehicle.CssClass = "form-control required integerInput";
                }

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
                    txtPublicTransport.Attributes.Remove("required");
                    txtPublicTransport.CssClass = "form-control integerInput";
                }
                else
                {
                    txtPublicTransport.Attributes.Add("required", "true");
                    txtPublicTransport.CssClass = "form-control required integerInput";
                }

                CalculateTADA();

                ApplyTADAValidation();
            }
            catch (Exception exp)
            {
                new WRException((long)Session[SessionValues.UserID], exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        /// <summary>
        /// This function checks if the total amount in repair and maintainence bill is valid
        /// Created On 12-05-2017
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidateRepairMaintainenceTotal()
        {
            if (!(txtTotalAmount.Text.Trim() != string.Empty && Convert.ToDouble(txtTotalAmount.Text.Trim()) != 0))
            {
                Master.ShowMessage(Message.ValidTotalAmount.Description, SiteMaster.MessageType.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function removes required check from RM fields.
        /// Created On 12-05-2017 
        /// </summary>
        private void ApplyRMValidation()
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
        /// This function checks if the TA total is valid
        /// Created On 12-05-2017
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidateDAAmount()
        {
            if (!(txtDAAmount.Text.Trim() != string.Empty && Convert.ToDouble(txtDAAmount.Text.Trim()) != 0))
            {
                Master.ShowMessage(Message.ValidDAAmount.Description, SiteMaster.MessageType.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// This function removes required check from TADA fields.
        /// Created On 12-05-2017 
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
        /// This function binds data to expense form for Edit screen.
        /// Created On 15-05-2017
        /// </summary>
        private void BindExpenseForm()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["TotalClaim"]))
            {
                TotalClaim = Request.QueryString["TotalClaim"].ToString();
            }

            long MonthlyExpenseID = Convert.ToInt64(hdnMonthlyExpenseID.Value.Trim());

            AT_MonthlyExpenses mdlMonthlyExpenses = new AccountsBLL().GetMonthlyExpenseByID(MonthlyExpenseID);

            ResourceAllocationID = mdlMonthlyExpenses.ResourceAllocationID.Value;

            lblFinancialYear.Text = mdlMonthlyExpenses.FinancialYear;
            lblMonth.Text = mdlMonthlyExpenses.Month;
            lblTotalClaim.Text = TotalClaim;
            lblResourceType.Text = (mdlMonthlyExpenses.PMIUFieldOffice == "F" ? "Field" : "Office");
            lblDesignation.Text = mdlMonthlyExpenses.AT_ResourceAllocation.UA_Designations.Name;
            lblNameOfStaff.Text = mdlMonthlyExpenses.AT_ResourceAllocation.StaffUserName;

            lbtnBack.PostBackUrl = string.Format("~/Modules/Accounts/ExpenseDetails.aspx?FinancialYear={0}&Month={1}&ResourceID={2}&TotalClaim={3}&ExpenseTypeID={4}", mdlMonthlyExpenses.FinancialYear, mdlMonthlyExpenses.Month, mdlMonthlyExpenses.ResourceAllocationID, TotalClaim, mdlMonthlyExpenses.ExpenseTypeID);

            Dropdownlist.SetSelectedValue(ddlExpenseType, mdlMonthlyExpenses.ExpenseTypeID.ToString());

            ddlExpenseType_SelectedIndexChanged(null, null);

            ddlExpenseType.Enabled = false;
            ddlExpenseType.Attributes.Remove("required");
            ddlExpenseType.CssClass = "form-control";

            if (mdlMonthlyExpenses.ExpenseTypeID == (long)Constants.ExpenseType.RepairMaintainance)
            {

            }
            else if (mdlMonthlyExpenses.ExpenseTypeID == (long)Constants.ExpenseType.POLReceipts)
            {
                BindPOLExpenseForm(mdlMonthlyExpenses);
            }
            else if (mdlMonthlyExpenses.ExpenseTypeID == (long)Constants.ExpenseType.TADA)
            {

            }
            else if (mdlMonthlyExpenses.ExpenseTypeID == (long)Constants.ExpenseType.NewPurchase)
            {

            }
            else if (mdlMonthlyExpenses.ExpenseTypeID == (long)Constants.ExpenseType.OtherExpense)
            {

            }
        }

        /// <summary>
        /// THis function binds the POL form.
        /// Created On 15-05-2017
        /// </summary>
        /// <param name="_MdlMonthlyExpenses"></param>
        private void BindPOLExpenseForm(AT_MonthlyExpenses _MdlMonthlyExpenses)
        {
            txtPRBillNo.Text = _MdlMonthlyExpenses.BillNo;
            Dropdownlist.SetSelectedValue(ddlVehicle, _MdlMonthlyExpenses.POLVehicleNo);
            txtAmount.Text = (_MdlMonthlyExpenses.POLAmount.HasValue ? _MdlMonthlyExpenses.POLAmount.ToString() : string.Empty);
            txtReceiptNo.Text = _MdlMonthlyExpenses.POLReceiptNo;
            txtDate.Text = Utility.GetFormattedDate(_MdlMonthlyExpenses.POLDatetime);
            txtMeterReading.Text = _MdlMonthlyExpenses.POLMeterReading;

            if (_MdlMonthlyExpenses.Attachment != null)
            {
                string FileName = new System.IO.FileInfo(_MdlMonthlyExpenses.Attachment).Name;
                List<string> lstName = new List<string> { FileName };

                fupAttachReceiptEdit.Mode = Convert.ToInt32(Constants.ModeValue.Thumbnail);
                fupAttachReceiptEdit.Size = 1;
                fupAttachReceiptEdit.ViewUploadedFilesAsThumbnail(Configuration.Accounts, lstName);
            }
        }
    }
}