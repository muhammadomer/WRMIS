using PMIU.WRMIS.DAL.DataAccess.Accounts;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Accounts
{
    public class AccountsBLL : BaseBLL
    {
        AccountsDAL dalAccounts = new AccountsDAL();

        #region Expense Details

        public List<AT_MonthlyExpenses> GetMonthlyExpenses(long _ExpenseTypeID, string _Month, string _FinancialYear, long _ResourceID)
        {
            return dalAccounts.GetMonthlyExpenses(_ExpenseTypeID, _Month, _FinancialYear, _ResourceID);
        }

        public AT_MonthlyExpenses GetMonthlyExpenseByID(long _MonthlyExpenseID)
        {
            AT_MonthlyExpenses mdlMonthlyExpense = dalAccounts.GetMonthlyExpenseByID(_MonthlyExpenseID);
            return mdlMonthlyExpense;
        }

        public bool DeleteMonthlyExpense(long _ExpenseID)
        {
            return dalAccounts.DeleteMonthlyExpense(_ExpenseID);
        }

        public AT_ExpenseSanction IsMonthlyExpenseIDExist(long _MonthlyExpenseID)
        {
            return dalAccounts.IsMonthlyExpenseIDExist(_MonthlyExpenseID);
        }

        #endregion

        #region Search Expense

        public List<object> SearchMonthlyExpnses(Dictionary<string, object> _Data, string AccountOfficeer = "", long _CreatedBy = 0)
        {
            string _FinancialYear = "";
            string _Month = "";
            long _ExpenseMadeby = 0;
            string _PMIUStaff = "";
            _FinancialYear = Convert.ToString(_Data["_FinancialYear"]);
            _Month = Convert.ToString(_Data["_Month"]);
            _ExpenseMadeby = Convert.ToInt64(_Data["_ExpenseMadeby"]);
            _PMIUStaff = Convert.ToString(_Data["_PMIUStaff"]);
            if (!string.IsNullOrEmpty(AccountOfficeer))
            {
                return dalAccounts.SearchMonthlyExpnses(_FinancialYear, _Month, _ExpenseMadeby, _PMIUStaff, AccountOfficeer, _CreatedBy);
            }
            return dalAccounts.SearchMonthlyExpnses(_FinancialYear, _Month, _ExpenseMadeby, _PMIUStaff);
        }
        public bool IsExpenseSubmited(string _FinancialYear, string _Month, List<long> _ResourseAllocationID, int AccountOfficer = 0)
        {
            return dalAccounts.IsExpenseSubmited(_FinancialYear, _Month, _ResourseAllocationID, AccountOfficer);
        }



        public bool UpdateMonthlyExpenseStatus(long ResourceAllocationID, string FinancialYear, string Month, string PMIUStaff, long ExpenseMadeBy, long ModifiedBy, string _UserType = "AO")
        {

            return dalAccounts.UpdateMonthlyExpenseStatus(ResourceAllocationID, FinancialYear, Month, PMIUStaff, ExpenseMadeBy, ModifiedBy, _UserType);
        }


        #endregion

        #region Approve Budget

        public List<object> GetApprovedBudget(string _FinancialYear)
        {
            return dalAccounts.GetApprovedBudget(_FinancialYear);
        }

        public bool SaveApprovedBudget(List<AT_BudgetApprovel> _ListBudgetApproval)
        {
            return dalAccounts.SaveApprovedBudget(_ListBudgetApproval);
        }

        #endregion

        #region Add/Edit Expense

        /// <summary>
        /// This function gets the Expense Types
        /// Created On 12-04-2017
        /// </summary>
        /// <param name="_Source"></param>
        /// <returns>List<AT_ExpenseType></returns>
        public List<AT_ExpenseType> GetExpenseTypes(string _Source = null)
        {
            return dalAccounts.GetExpenseTypes(_Source);
        }

        /// <summary>
        /// This function gets the assets based on resource ID and Asset Types
        /// Created On 13-04-2017
        /// </summary>
        /// <param name="_ResourceID"></param>
        /// <param name="_AssetTypeIDs"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetAssetsByResourceIDAndType(long _ResourceID, List<long> _AssetTypeIDs = null)
        {
            return dalAccounts.GetAssetsByResourceIDAndType(_ResourceID, _AssetTypeIDs);
        }

        /// <summary>
        /// This function adds Monthly Expense.
        /// Created On 17-04-2017
        /// </summary>
        /// <param name="_MdlMonthlyExpenses"></param>
        public void AddMonthlyExpense(AT_MonthlyExpenses _MdlMonthlyExpenses)
        {
            dalAccounts.AddMonthlyExpense(_MdlMonthlyExpenses);
        }

        /// <summary>
        /// This function updates Monthly Expense.
        /// Created On 15-05-2017
        /// </summary>
        /// <param name="_MdlMonthlyExpenses"></param>
        public void UpdateMonthlyExpense(AT_MonthlyExpenses _MdlMonthlyExpenses)
        {
            dalAccounts.UpdateMonthlyExpense(_MdlMonthlyExpenses);
        }

        /// <summary>
        /// This function gets latest bill no by part of bill no.
        /// Created On 19-04-2017
        /// </summary>
        /// <param name="_Prefix"></param>
        /// <returns>string</returns>
        public string GetBillNumber(string _Prefix)
        {
            return dalAccounts.GetBillNumber(_Prefix);
        }

        /// <summary>
        /// This function get resource by ID.
        /// Created On 21-04-2017
        /// </summary>
        /// <param name="_ResourceAllocationID"></param>
        /// <returns>AT_ResourceAllocation</returns>
        public AT_ResourceAllocation GetResourceByID(long _ResourceAllocationID)
        {
            return dalAccounts.GetResourceByID(_ResourceAllocationID);
        }

        /// <summary>
        /// This function return the key parts that have not been assigned to any expense
        /// Created On 27-04-2017
        /// </summary>
        /// <param name="_AssetTypeID"></param>
        /// <param name="_ExpenseID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> BindUnAssignedKeyParts(long _AssetTypeID, long? _ExpenseID)
        {
            return dalAccounts.BindUnAssignedKeyParts(_AssetTypeID, _ExpenseID);
        }

        /// <summary>
        /// This function returns the key parts that have been assigned to any expense
        /// Created On 27-04-2017
        /// </summary>
        /// <param name="_AssetTypeID"></param>
        /// <param name="_ExpenseID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> BindAssignedKeyParts(long _AssetTypeID, long? _ExpenseID)
        {
            return dalAccounts.BindAssignedKeyParts(_AssetTypeID, _ExpenseID);
        }

        /// <summary>
        /// This function gets the Account Setup values based on ID
        /// Created By 08-05-2017
        /// </summary>
        /// <param name="_AccountSetupID"></param>
        /// <returns>double</returns>
        public double GetAccountSetupValue(long _AccountSetupID)
        {
            return dalAccounts.GetAccountSetupValue(_AccountSetupID);
        }

        /// <summary>
        /// This function saves the key parts related to expense.
        /// Created On 09-05-2017 
        /// </summary>
        /// <param name="_LstPartExpense"></param>
        public void SaveKeyPartsOfExpense(List<AT_ExpenseOfParts> _LstPartExpense)
        {
            dalAccounts.SaveKeyPartsOfExpense(_LstPartExpense);
        }

        /// <summary>
        /// This function saves the quotations related to expense.
        /// Created On 09-05-2017 
        /// </summary>
        /// <param name="_LstExpenseQuotation"></param>
        public void SaveQuotationsOfExpense(List<AT_ExpenseQuotation> _LstExpenseQuotation)
        {
            dalAccounts.SaveQuotationsOfExpense(_LstExpenseQuotation);
        }

        /// <summary>
        /// This function gets daily rate by resource ID
        /// Created On 12-05-2017
        /// </summary>
        /// <param name="_ResourceAllocationID"></param>
        /// <returns>AT_DailyRate</returns>
        public AT_DailyRate GetDailyRateByResourceID(long _ResourceAllocationID)
        {
            return dalAccounts.GetDailyRateByResourceID(_ResourceAllocationID);
        }

        /// <summary>
        /// This function gets the Quotations against an expense.
        /// Created On 19-05-2017
        /// </summary>
        /// <param name="_MonthlyExpenseID"></param>
        /// <returns>List<AT_ExpenseQuotation></returns>
        public List<AT_ExpenseQuotation> GetExpenseQuotations(long _MonthlyExpenseID)
        {
            return dalAccounts.GetExpenseQuotations(_MonthlyExpenseID);
        }

        /// <summary>
        /// This function deletes the quotation with given ID.
        /// Created On 19-05-2017
        /// </summary>
        /// <param name="_ExpenseQuotationID"></param>
        public void DeleteQuotation(long _ExpenseQuotationID)
        {
            dalAccounts.DeleteQuotation(_ExpenseQuotationID);
        }

        /// <summary>
        /// This function adds a new quotation.
        /// Created On 22-05-2017
        /// </summary>
        /// <param name="_MdlExpenseQuotation"></param>
        public void AddQuotation(AT_ExpenseQuotation _MdlExpenseQuotation)
        {
            dalAccounts.AddQuotation(_MdlExpenseQuotation);
        }

        /// <summary>
        /// This function gets Asset Allocation data based on ID
        /// Created On 23-05-2017
        /// </summary>
        /// <param name="_AssetAllocationID"></param>
        /// <returns>AT_AssetAllocation</returns>
        public AT_AssetAllocation GetAssetAllocationByID(long _AssetAllocationID)
        {
            return dalAccounts.GetAssetAllocationByID(_AssetAllocationID);
        }

        /// <summary>
        /// This function deletes key parts based on the Monthly Expense ID.
        /// Created On 23-05-2017
        /// </summary>        
        /// <returns>List<AT_ExpenseOfParts></returns>
        public void DeletePartsByExpenseID(long _MonthlyExpenseID)
        {
            dalAccounts.DeletePartsByExpenseID(_MonthlyExpenseID);
        }

        /// <summary>
        /// This function returns if 4 wheel or 2 wheel vehicle is assigned to resource
        /// Created on 17-07-2017
        /// </summary>
        /// <param name="_ResourseAllocationID"></param>
        /// <returns>bool</returns>
        public bool IsFourOrTwoWheelVehicleAssigned(long _ResourseAllocationID)
        {
            return dalAccounts.IsFourOrTwoWheelVehicleAssigned(_ResourseAllocationID);
        }

        #endregion

        #region FundRelease

        public List<dynamic> GetFundRelaseByFinancialYear(string _FinancialYear)
        {
            return dalAccounts.GetFundRelaseByFinancialYear(_FinancialYear);
        }

        public bool AddFundRelease(AT_FundRelease _FundRelease)
        {
            return dalAccounts.AddFundRelease(_FundRelease);
        }

        public bool UpdateFundRelease(AT_FundRelease _FundRelease)
        {
            return dalAccounts.UpdateFundRelease(_FundRelease);
        }

        public bool DeleteFundRelease(long _FundReleaseID)
        {
            return dalAccounts.DeleteFundRelease(_FundReleaseID);
        }

        public List<object> GetFundReleaseDetails(string _GetFundReleaseDetails, long _FundReleaseID)
        {
            return dalAccounts.GetFundReleaseDetails(_GetFundReleaseDetails, _FundReleaseID);
        }

        public bool AddFundReleaseDetails(AT_FundReleaseDetails _FundReleaseDetails)
        {
            return dalAccounts.AddFundReleaseDetails(_FundReleaseDetails);
        }

        public AT_FundRelease GetFundReleaseDetailsByID(long _FundReleaseID)
        {
            return dalAccounts.GetFundReleaseDetailsByID(_FundReleaseID);
        }

        public AT_FundRelease GetLatestReleaseByYear(string _FinancialYear, long _FundReleaseID)
        {
            return dalAccounts.GetLatestReleaseByYear(_FinancialYear, _FundReleaseID);
        }

        public bool GetFundReleaseDetailsByFundReleaseID(long _FundReleaseID)
        {
            return dalAccounts.GetFundReleaseDetailsByFundReleaseID(_FundReleaseID);
        }

        public bool IsFundReleaseUnique(long _FundReleaseTypeID, string _FinancialYear)
        {
            return dalAccounts.IsFundReleaseUnique(_FundReleaseTypeID, _FinancialYear);
        }


        public bool IsFundRelease(long _FundReleaseID, string _FinancialYear, DateTime FundReleaseDate)
        {
            return dalAccounts.IsFundRelease(_FundReleaseID, _FinancialYear, FundReleaseDate);
        }

        public bool CheckFundRelaseDetail(long _FundReleaseTypeID, string _FinancialYear, DateTime FundReleaseDate)
        {
            return dalAccounts.CheckFundRelaseDetail(_FundReleaseTypeID, _FinancialYear, FundReleaseDate);
        }

        #endregion

        #region Sanction
        public List<AT_Sanction> GetSanctions(string _SanctionNo, long _SanctionType, long _SanctionOn, string _FinancialYear, string _Month, long _SanctionStatus)
        {
            return dalAccounts.GetSanctions(_SanctionNo, _SanctionType, _SanctionOn, _FinancialYear, _Month, _SanctionStatus);
        }

        public List<AT_AssetType> GetAssetTypes()
        {
            return dalAccounts.GetAssetTypes();
        }

        public List<AT_SanctionStatus> GetSanctionStatus()
        {
            return dalAccounts.GetSanctionStatus();
        }

        public bool UpdateSanctionOnSearchSanction(AT_Sanction _Sanction)
        {
            return dalAccounts.UpdateSanctionOnSearchSanction(_Sanction);
        }

        /// <summary>
        /// This function update the payment date for a particular cheque.
        /// Created On 24-01-2018
        /// </summary>
        /// <param name="_MdlPaymentDetails"></param>
        /// <returns>bool</returns>
        public bool UpdateChequePaymentDate(AT_PaymentDetails _MdlPaymentDetails)
        {
            return dalAccounts.UpdateChequePaymentDate(_MdlPaymentDetails);
        }

        public List<AT_Sanction> GetAllSanctions(long _SanctionType)
        {
            return dalAccounts.GetSanctions(_SanctionType);
        }

        public bool AddPaymentDetails(AT_PaymentDetails _PaymentDetails)
        {
            return dalAccounts.AddPaymentDetails(_PaymentDetails);
        }

        public bool AddSanctionPayment(AT_SanctionPayment _SanctionPayment)
        {
            return dalAccounts.AddSanctionPayment(_SanctionPayment);
        }

        public void SetSanctionExpenseStatus(long _SanctionID)
        {
            dalAccounts.SetSanctionExpenseStatus(_SanctionID);
        }

        public AT_PaymentDetails GetLatestPaymentDetail()
        {
            return dalAccounts.GetLatestPaymentDetail();
        }

        public List<AT_ObjectClassification> GetAllObjectClassifications()
        {
            return dalAccounts.GetAllObjectClassifications();
        }

        public List<dynamic> GetAllObjectClassificationsAndCode()
        {
            return dalAccounts.GetAllObjectClassificationsAndCode();
        }

        public bool AddSanction(AT_Sanction _Sanction)
        {
            return dalAccounts.AddSanction(_Sanction);
        }

        public bool UpdateSanction(AT_Sanction _Sanction)
        {
            return dalAccounts.UpdateSanction(_Sanction);
        }

        public bool DeleteSanction(long _Sanction)
        {
            return dalAccounts.DeleteSanction(_Sanction);
        }

        public bool IsSanctionNoExist(string _SanctionNo)
        {
            return dalAccounts.IsSanctionNoExist(_SanctionNo);
        }

        public DataSet SearchCheque(string ChequeNo, string ChequeFromDate, string ChequeToDate, double? ChequeFromAmount, double? ChequeToAmount)
        {
            return dalAccounts.SearchCheque(ChequeNo, ChequeFromDate, ChequeToDate, ChequeFromAmount, ChequeToAmount);
        }

        public DataSet GetPaymentDetailsByID(long _PaymentDetailID)
        {
            return dalAccounts.GetPaymentDetailsByID(_PaymentDetailID);
        }

        public bool DeleteCheque(long _Cheque)
        {
            return dalAccounts.DeleteCheque(_Cheque);
        }

        public bool UpdateSanctionStatusInMonthlyExpense(long _SanctionID, long _StatusID, int _UserID)
        {
            return dalAccounts.UpdateSanctionStatusInMonthlyExpense(_SanctionID, _StatusID, _UserID);
        }
        #endregion

        #region New Sanction
        public string GetSanctionNumber(string _Prefix)
        {
            return dalAccounts.GetSanctionNumber(_Prefix);
        }
        public object GetApprovalBudget_ReleaseAmountByFinancialYear(string _FinancialYear)
        {
            return dalAccounts.GetApprovalBudget_ReleaseAmountByFinancialYear(_FinancialYear);
        }
        public List<object> RMGridData(string _FinancialYear, string _Month, long _ExpneseTypeID, string _AssetType, long _AccountOfficer)
        {
            return dalAccounts.RMGridData(_FinancialYear, _Month, _ExpneseTypeID, _AssetType, _AccountOfficer);
        }
        public List<object> POLGridData(string _FinancialYear, string _Month, long _ExpneseTypeID, string _AssetType, long _AccountOfficer)
        {
            return dalAccounts.POLGridData(_FinancialYear, _Month, _ExpneseTypeID, _AssetType, _AccountOfficer);
        }
        public List<object> TADAGridData(string _FianancialYear, string _Month, long ExpenseTypeID, long _AccountOfficer)
        {
            return dalAccounts.TADAGridData(_FianancialYear, _Month, ExpenseTypeID, _AccountOfficer);
        }
        public List<object> NPGridData(string _FianancialYear, string _Month, long _AssetTypeID, long _AccountOfficer)
        {
            return dalAccounts.NPGridData(_FianancialYear, _Month, _AssetTypeID, _AccountOfficer);
        }

        public List<object> OEGridData(string _FianancialYear, string _Month, long _AssetTypeID, long _AccountOfficer)
        {
            return dalAccounts.OEGridData(_FianancialYear, _Month, _AssetTypeID, _AccountOfficer);
        }
        public bool SaveSanctioned(List<AT_Sanction> _lstSanctioned, List<AT_ExpenseSanction> _es = null)
        {
            return dalAccounts.SaveSanctionedDAL(_lstSanctioned, _es);
        }



        #endregion

        #region Tax Sheet

        public List<dynamic> GetTaxSheet(long _SanctionID)
        {
            return dalAccounts.GetTaxSheet(_SanctionID);
        }

        public double? GetTaxRateByID(long _TaxRateID)
        {
            return dalAccounts.GetTaxRateByID(_TaxRateID);
        }

        public AT_Sanction GetSanctionByID(long _SanctionID)
        {
            return dalAccounts.GetSanctionByID(_SanctionID);
        }

        public double GetExpenseLimitForTax()
        {
            return dalAccounts.GetExpenseLimitForTax();
        }
        #endregion

        #region Budget Utilization
        public List<object> GetBudgetUtilizationGridData(string _FinancialYear, string _Month, long _ObjectClassificationID)
        {
            return dalAccounts.GetBudgetUtilizationGridData(_FinancialYear, _Month, _ObjectClassificationID);
        }
        #endregion

        public bool IsSanctionedAmountExceed(string _FinancialYear, double _SanctionedAmount, long _ObjectClassificationID)
        {
            return dalAccounts.IsSanctionedAmountExceed(_FinancialYear, _SanctionedAmount, _ObjectClassificationID);
        }
    }

}
