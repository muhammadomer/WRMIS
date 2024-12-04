using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;

namespace PMIU.WRMIS.DAL.Repositories.Accounts
{
    public class AccountsRepository : Repository<AT_MonthlyExpenses>
    {
        public AccountsRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<AT_MonthlyExpenses>();
        }

        #region FundRelease

        public List<object> GetBudgetApproval(string _FinancialYear, long _FundReleaseID)
        {
            List<object> lstBudgetApproval = (from ba in context.AT_BudgetApprovel
                                              join innerQuery in
                                                  (from ba2 in context.AT_BudgetApprovel group ba2 by new { ba2.ObjectClassificationID, ba2.FinancialYear } into s select new { BudgetApprovalID = s.Max(x => x.ID) }) on ba.ID equals innerQuery.BudgetApprovalID
                                              where ba.ID == innerQuery.BudgetApprovalID && ba.FinancialYear == _FinancialYear
                                              select new
                                              {
                                                  ID = ba.ID,
                                                  ObjectClassificationID = ba.AT_ObjectClassification.ID,
                                                  AccountCode = ba.AT_ObjectClassification.AccountsCode,
                                                  ObjectClassification = ba.AT_ObjectClassification.ObjectClassification,
                                                  BudgetAmount = ba.BudgetAmount,
                                                  SupplyMentoryGrant = context.AT_FundReleaseDetails.Where(x => x.BudgetApprovelID == ba.ID && x.AT_FundRelease.FundReleaseTypeID == (long)(Common.Constants.FundsReleaseType.SupplementaryGrant) && x.FundReleaseID <= _FundReleaseID).Select(x => x.CurrentReleaseAmount).Sum(),
                                                  ReAppropriation = context.AT_FundReleaseDetails.Where(x => x.BudgetApprovelID == ba.ID && x.AT_FundRelease.FundReleaseTypeID == (long)(Common.Constants.FundsReleaseType.Reappropriation) && x.FundReleaseID <= _FundReleaseID ).Select(x => x.CurrentReleaseAmount).Sum(),
                                                  CurrentReleaseAmount = context.AT_FundReleaseDetails.Where(x => x.FundReleaseID == _FundReleaseID && x.ObjectClassificationID == ba.AT_ObjectClassification.ID && x.BudgetApprovelID == ba.ID && ba.BudgetAmount>0).Select(x => x.CurrentReleaseAmount).FirstOrDefault(),
                                                  PreviouslyReleasedAmount = context.AT_FundReleaseDetails.Where(x => x.ObjectClassificationID == ba.AT_ObjectClassification.ID && x.BudgetApprovelID == ba.ID && x.FundReleaseID <= _FundReleaseID && x.CurrentReleaseAmount>0).Select(x => x.CurrentReleaseAmount).Sum()
                                              }).ToList<object>();
            return lstBudgetApproval;
        }

        public List<dynamic> GetFundRelaseByFinancialYear(string _FinancialYear)
        {
            List<dynamic> lstFundRelease = (from fr in context.AT_FundRelease
                                            join frt in context.AT_FundReleaseTypes on fr.FundReleaseTypeID equals frt.ID
                                            where fr.FinancialYear == _FinancialYear
                                            orderby fr.FundReleaseDate
                                            select new
                                            {
                                                ID = fr.ID,
                                                FundReleaseType = frt.TypeName,
                                                FundReleaseTypeID = frt.ID,
                                                FundReleaseDate = fr.FundReleaseDate,
                                                Description = fr.Description
                                            }).ToList<dynamic>();
            return lstFundRelease;
        }
        public bool CheckFundRelaseDetail(long _FundReleaseTypeID, string _FinancialYear, DateTime FundReleaseDate)
        {
            bool lstFundRelease = (from frt in context.AT_FundReleaseTypes
                                   join fr in context.AT_FundRelease on frt.ID equals fr.FundReleaseTypeID
                                   join frd in context.AT_FundReleaseDetails on fr.ID equals frd.FundReleaseID
                                   where
                                   fr.FinancialYear == _FinancialYear && frt.TypeName == "Supplementary Grant" || frt.TypeName == "Reappropriation"
                                   select frd).Any();
            return lstFundRelease;
        }
       

        #endregion

        #region Add Edit Expenses

        /// <summary>
        /// This function returns the key parts that have not been assigned to any expense
        /// Created On 27-04-2017
        /// </summary>
        /// <param name="_AssetTypeID"></param>
        /// <param name="_ExpenseID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> BindUnAssignedKeyParts(long _AssetTypeID, long? _ExpenseID)
        {
            List<dynamic> lstUnAssignedParts = (from kp in context.AT_KeyParts
                                                join eop in context.AT_ExpenseOfParts on new { KeyPartID = kp.ID, ExpenseID = _ExpenseID } equals new { KeyPartID = (long)eop.KeyPartsID, ExpenseID = (long?)eop.MonthlyExpenseID } into ep
                                                from eo in ep.DefaultIfEmpty()
                                                where eo.ID == null && kp.AssetTypeID == _AssetTypeID
                                                orderby kp.PartName
                                                select new
                                                {
                                                    ID = kp.ID,
                                                    Name = kp.PartName
                                                }).ToList<dynamic>();

            return lstUnAssignedParts;
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
            List<dynamic> lstAssignedParts = (from kp in context.AT_KeyParts
                                              join eop in context.AT_ExpenseOfParts on new { KeyPartID = kp.ID, ExpenseID = _ExpenseID } equals new { KeyPartID = (long)eop.KeyPartsID, ExpenseID = (long?)eop.MonthlyExpenseID }
                                              where kp.AssetTypeID == _AssetTypeID
                                              orderby kp.PartName
                                              select new
                                              {
                                                  ID = kp.ID,
                                                  Name = kp.PartName
                                              }).ToList<dynamic>();

            return lstAssignedParts;
        }

        /// <summary>
        /// This function saves the key parts related to expense.
        /// Created On 09-05-2017 
        /// </summary>
        /// <param name="_LstPartExpense"></param>
        public void SaveKeyPartsOfExpense(List<AT_ExpenseOfParts> _LstPartExpense)
        {
            context.AT_ExpenseOfParts.AddRange(_LstPartExpense);

            context.SaveChanges();
        }

        /// <summary>
        /// This function saves the quotations related to expense.
        /// Created On 09-05-2017 
        /// </summary>
        /// <param name="_LstExpenseQuotation"></param>
        public void SaveQuotationsOfExpense(List<AT_ExpenseQuotation> _LstExpenseQuotation)
        {
            context.AT_ExpenseQuotation.AddRange(_LstExpenseQuotation);

            context.SaveChanges();
        }

        /// <summary>
        /// This function gets daily rate by resource ID
        /// Created On 12-05-2017
        /// </summary>
        /// <param name="_ResourceAllocationID"></param>
        /// <returns>AT_DailyRate</returns>
        public AT_DailyRate GetDailyRateByResourceID(long _ResourceAllocationID)
        {
            return (from ra in context.AT_ResourceAllocation
                    join dr in context.AT_DailyRate on ra.BPS equals dr.BPS
                    where ra.ID == _ResourceAllocationID
                    select dr).FirstOrDefault();
        }

        #endregion

        #region Expense Detail

        public bool DeleteMonthlyExpense(long _ExpenseID)
        {
            List<AT_ExpenseQuotation> lstAT_ExpenseQuotation = context.AT_ExpenseQuotation.Where(eq => eq.MonthlyExpenseID == _ExpenseID).ToList();

            context.AT_ExpenseQuotation.RemoveRange(lstAT_ExpenseQuotation);

            List<AT_ExpenseOfParts> lstExpenseOfParts = context.AT_ExpenseOfParts.Where(eop => eop.MonthlyExpenseID == _ExpenseID).ToList();

            context.AT_ExpenseOfParts.RemoveRange(lstExpenseOfParts);

            AT_MonthlyExpenses mdlMonthlyExpenses = context.AT_MonthlyExpenses.Where(me => me.ID == _ExpenseID).FirstOrDefault();
            context.AT_MonthlyExpenses.Remove(mdlMonthlyExpenses);

            context.SaveChanges();

            return true;
        }

        #endregion

        #region Tax Sheet

        public List<dynamic> GetTaxSheet(long _SanctionID)
        {
            List<dynamic> lstExpenseSanction = (from ES in context.AT_ExpenseSanction
                                                join ME in context.AT_MonthlyExpenses on ES.MonthlyExpenseID equals ME.ID
                                                join RA in context.AT_ResourceAllocation on ME.ResourceAllocationID equals RA.ID
                                                //join AA in context.AT_AssetAllocation on ME.AssetAllocationID equals AA.ResourceAllocationID
                                                where ES.SanctionID == _SanctionID
                                                select new
                                                {
                                                    NameOfStaff = RA.StaffUserName,
                                                    Designation = RA.UA_Designations.Name,
                                                    VehicleNo = ME.AT_AssetAllocation.AM_AssetItems.AssetName,
                                                    TotalBill = (ME.PurchaseAmount == null ? 0 : ME.PurchaseAmount) + (ME.RepairAmount == null ? 0 : ME.RepairAmount),
                                                    PurchaseItem = ME.PurchaseAmount, //== null ? 0 : ME.PurchaseAmount,
                                                    RepairItem = ME.RepairAmount,// == null ? 0 : ME.RepairAmount,
                                                    VendorType = ES.VendorType
                                                }).ToList<dynamic>();

            return lstExpenseSanction;
        }

        #endregion

        #region Sanction
        public bool UpdateSanctionStatusInMonthlyExpense(long _SanctionID, long _StatusID, int _UserID)
        {
            List<long?> lstAT_ExpenseSanction = context.AT_ExpenseSanction.Where(es => es.SanctionID == _SanctionID).Select(x => x.MonthlyExpenseID).ToList();

            List<AT_MonthlyExpenses> lstAT_MonthlyExpenses = context.AT_MonthlyExpenses.Where(x => lstAT_ExpenseSanction.Contains(x.ID)).ToList();

            foreach (AT_MonthlyExpenses mdlMonthlyExpenses in lstAT_MonthlyExpenses)
            {
                mdlMonthlyExpenses.ExpenseStatusID = _StatusID;
                mdlMonthlyExpenses.ModifiedDate = DateTime.Now;
                mdlMonthlyExpenses.ModifiedBy = _UserID;
            }

            context.SaveChanges();

            return true;
        }
        #endregion
    }
}
