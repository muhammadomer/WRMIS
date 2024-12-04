using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.Accounts;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.Accounts
{
    public class AccountsDAL
    {
        ContextDB db = new ContextDB();

        #region Expense Details

        public List<AT_MonthlyExpenses> GetMonthlyExpenses(long _ExpenseTypeID, string _Month, string _FinancialYear, long _ResourceID)
        {
            List<AT_MonthlyExpenses> lstMonthlyExpenses = db.Repository<AT_MonthlyExpenses>().GetAll().Where(me => me.ExpenseTypeID == _ExpenseTypeID
                && me.Month.Trim().ToUpper() == _Month.Trim().ToUpper()
                && me.FinancialYear == _FinancialYear
                && me.ResourceAllocationID == _ResourceID).ToList();
            return lstMonthlyExpenses;
        }

        public AT_MonthlyExpenses GetMonthlyExpenseByID(long _MonthlyExpenseID)
        {
            AT_MonthlyExpenses mdlMonthlyExpense = db.Repository<AT_MonthlyExpenses>().GetAll().Where(x => x.ID == _MonthlyExpenseID).FirstOrDefault();
            return mdlMonthlyExpense;
        }

        public bool DeleteMonthlyExpense(long _ExpenseID)
        {
            return db.ExtRepositoryFor<AccountsRepository>().DeleteMonthlyExpense(_ExpenseID);
        }

        public AT_ExpenseSanction IsMonthlyExpenseIDExist(long _MonthlyExpenseID)
        {
            AT_ExpenseSanction mdlExpenseSanction = db.Repository<AT_ExpenseSanction>().GetAll().Where(x => x.MonthlyExpenseID == _MonthlyExpenseID).FirstOrDefault();
            return mdlExpenseSanction;
        }

        #endregion

        #region Search Expense

        public List<object> SearchMonthlyExpnses(string _FinancialYear, string _Month, long _ExpenseMadeby, string _PMIUStaff, string _UserType = "ADM", long _CreatedBy = 0)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("AT_GetMonthlyExpenses", _FinancialYear, _Month, _ExpenseMadeby, _PMIUStaff, _UserType, _CreatedBy);
            List<object> lstObject = new List<object>();
            foreach (DataRow DR in dt.Rows)
            {
                lstObject.Add(
                    new
                    {

                        ResourceAllocationID = Convert.ToString(DR["ResourceAllocationID"] == DBNull.Value ? "" : DR["ResourceAllocationID"]),
                        Name = Convert.ToString(DR["Name"] == DBNull.Value ? "" : DR["Name"]),
                        Designation = Convert.ToString(DR["Designation"] == DBNull.Value ? "" : DR["Designation"]),
                        VehicleNo = Convert.ToString(DR["VehicleNo"] == DBNull.Value ? "" : DR["VehicleNo"]),
                        TADA = Convert.ToString(DR["TADA"]) == "0" ? "-" : Convert.ToString(DR["TADA"]),
                        RepairMaintainance = Convert.ToString(DR["RepairMaintainance"]) == "0" ? "-" : Convert.ToString(DR["RepairMaintainance"]),
                        POL = Convert.ToString(DR["POL"]) == "0" ? "-" : Convert.ToString(DR["POL"]),
                        OtherExpenses = Convert.ToString(DR["OtherExpenses"]) == "0" ? "-" : Convert.ToString(DR["OtherExpenses"]),
                        NewPurchase = Convert.ToString(DR["NewPurchase"]) == "0" ? "-" : Convert.ToString(DR["NewPurchase"]),
                        TotalClaim = Convert.ToInt64(DR["TotalClaim"] == DBNull.Value ? 0 : DR["TotalClaim"])
                    }
                    );
            }
            return lstObject;
        }





        public bool IsExpenseSubmited(string _FinancialYear, string _Month, List<long> _ResourseAllocationID, int AccountOfficer = 0)
        {

            if (AccountOfficer == 0)
            {


                for (int i = 0; i < _ResourseAllocationID.Count; i++)
                {
                    long RAID = _ResourseAllocationID[i];
                    AT_MonthlyExpenses ame = (from item in db.Repository<AT_MonthlyExpenses>().GetAll() where item.ResourceAllocationID == RAID && item.Month == _Month && item.FinancialYear == _FinancialYear select item).FirstOrDefault();
                    if (ame != null && ame.ID > 0 && ame.ExpenseStatusID != null)
                    {
                        return true;
                    }

                }
                return false;
            }
            else
            {
                for (int i = 0; i < _ResourseAllocationID.Count; i++)
                {
                    long RAID = _ResourseAllocationID[i];
                    AT_MonthlyExpenses ame = (from item in db.Repository<AT_MonthlyExpenses>().GetAll() where item.ResourceAllocationID == RAID && item.FinancialYear == _FinancialYear && item.Month == _Month select item).FirstOrDefault();

                    if (ame != null && ame.ID > 0 && ame.ExpenseSubmitted == "AO" && ame.ExpenseStatusID != null)
                    {
                        return true;
                    }

                }
                return false;
            }
        }

        #endregion

        #region Approve Budget

        public List<object> GetApprovedBudget(string _FinancialYear)
        {
            AT_BudgetApprovel ab = (from aba in db.Repository<AT_BudgetApprovel>().GetAll() where aba.FinancialYear == _FinancialYear select aba).FirstOrDefault();
            List<object> lstBudgetApproved = new List<object>();
            List<object> lstObjectClassfication = new List<object>();
            if (ab != null && ab.ID > 0)
            {


                DataTable dt = db.ExecuteStoredProcedureDataTable("AT_GetBudgetApproval", _FinancialYear);
                foreach (DataRow DR in dt.Rows)
                {
                    lstObjectClassfication.Add(
                        new
                        {
                            ID = Convert.ToString(DR["ID"] == DBNull.Value ? "" : DR["ID"]),
                            AccoundCode = Convert.ToString(DR["AccountsCode"] == DBNull.Value ? "" : DR["AccountsCode"]),
                            ObjectClassification = Convert.ToString(DR["ObjectClassification"] == DBNull.Value ? "" : DR["ObjectClassification"]),
                            ApprovedBudget = Convert.ToInt64(DR["ApprovedBudget"] == DBNull.Value ? 0 : DR["ApprovedBudget"]),
                            BudgetDate = Convert.ToDateTime(DR["BudgetDate"] == DBNull.Value ? "01-01-0002" : DR["BudgetDate"]),
                            alreadyExist = true
                        }
                        );
                }
                //TotalApprovedBudget
                long totalAmount = 0;
                foreach (var item in lstObjectClassfication)
                {
                    long Amount = 0;
                    Amount = Convert.ToInt64(item.GetType().GetProperty("ApprovedBudget").GetValue(item));
                    totalAmount += Amount;
                }
                lstObjectClassfication = (from item in lstObjectClassfication
                                          where 1 == 1
                                          select new
                                      {
                                          ID = item.GetType().GetProperty("ID").GetValue(item),
                                          AccoundCode = item.GetType().GetProperty("AccoundCode").GetValue(item),
                                          ObjectClassification = item.GetType().GetProperty("ObjectClassification").GetValue(item),
                                          ApprovedBudget = (item.GetType().GetProperty("ApprovedBudget").GetValue(item) == string.Empty ? 0 : item.GetType().GetProperty("ApprovedBudget").GetValue(item)),
                                          BudgetDate = item.GetType().GetProperty("BudgetDate").GetValue(item),
                                          alreadyExist = true,
                                          TotalApprovedBudget = totalAmount
                                      }
                                            ).ToList<object>();
            }
            else
            {
                lstObjectClassfication = (from oc in db.Repository<AT_ObjectClassification>().GetAll()
                                          select new
                                          {
                                              ID = oc.ID,
                                              AccoundCode = oc.AccountsCode,
                                              ObjectClassification = oc.ObjectClassification,
                                              ApprovedBudget = 0,
                                              alreadyExist = false
                                          }).ToList<object>();

            }
            return lstObjectClassfication;

        }

        public bool SaveApprovedBudget(List<AT_BudgetApprovel> _ListBudgetApproval)
        {
            if (_ListBudgetApproval[0].BudgetType.ToUpper() == "REAPRO" || _ListBudgetApproval[0].BudgetType.ToUpper() == "REVISED" || _ListBudgetApproval[0].BudgetType.ToUpper() == "SUPPLY")
            {
                foreach (AT_BudgetApprovel item in _ListBudgetApproval)
                {
                    AT_BudgetApprovel mdlUpdate = db.Repository<AT_BudgetApprovel>().FindById(item.ID);
                    mdlUpdate.BudgetAmount = item.BudgetAmount;
                    mdlUpdate.BudgetDate = item.BudgetDate;
                    mdlUpdate.BudgetType = item.BudgetType;
                    mdlUpdate.ModifiedBy = item.ModifiedBy;
                    mdlUpdate.ModifiedDate = item.ModifiedDate;
                    db.Repository<AT_BudgetApprovel>().Update(mdlUpdate);
                    db.Save();
                }
            }

            if (_ListBudgetApproval[0].BudgetType.ToUpper() == "APROVAL")
            {
                foreach (AT_BudgetApprovel item in _ListBudgetApproval)
                {
                    db.Repository<AT_BudgetApprovel>().Insert(item);
                    db.Save();
                }
            }
            return true;

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
            List<string> lstSources = new List<string>() { "B" };

            if (_Source != null)
            {
                lstSources.Add(_Source);
            }

            return db.Repository<AT_ExpenseType>().GetAll().Where(et => lstSources.Contains(et.Source)).ToList();

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
            if (_AssetTypeIDs != null)
            {
                return db.Repository<AT_AssetAllocation>().GetAll().Where(aa => aa.ResourceAllocationID == _ResourceID && _AssetTypeIDs.Contains(aa.AssetTypeID)).Select(aa => new { AssetAllocationID = aa.ID, AssetItemID = aa.AssetItemID, AssetName = aa.AM_AssetItems.AssetName }).ToList<dynamic>();
            }
            else
            {
                return db.Repository<AT_AssetAllocation>().GetAll().Where(aa => aa.ResourceAllocationID == _ResourceID).Select(aa => new { AssetAllocationID = aa.ID, AssetItemID = aa.AssetItemID, AssetName = aa.AM_AssetItems.AssetName }).ToList<dynamic>();
            }
        }

        /// <summary>
        /// This function adds Monthly Expense.
        /// Created On 17-04-2017
        /// </summary>
        /// <param name="_MdlMonthlyExpenses"></param>
        public void AddMonthlyExpense(AT_MonthlyExpenses _MdlMonthlyExpenses)
        {
            db.Repository<AT_MonthlyExpenses>().Insert(_MdlMonthlyExpenses);
            db.Save();
        }

        /// <summary>
        /// This function updates Monthly Expense.
        /// Created On 15-05-2017
        /// </summary>
        /// <param name="_MdlMonthlyExpenses"></param>
        public void UpdateMonthlyExpense(AT_MonthlyExpenses _MdlMonthlyExpenses)
        {
            db.Repository<AT_MonthlyExpenses>().Update(_MdlMonthlyExpenses);
            db.Save();
        }

        /// <summary>
        /// This function gets latest bill no by part of bill no.
        /// Created On 19-04-2017
        /// </summary>
        /// <param name="_Prefix"></param>
        /// <returns>string</returns>
        public string GetBillNumber(string _Prefix)
        {
            return db.Repository<AT_MonthlyExpenses>().GetAll().Where(me => me.BillNo.Contains(_Prefix)).OrderByDescending(me => me.ID).Select(me => me.BillNo).FirstOrDefault();
        }

        /// <summary>
        /// This function get resource by ID.
        /// Created On 21-04-2017
        /// </summary>
        /// <param name="_ResourceAllocationID"></param>
        /// <returns>AT_ResourceAllocation</returns>
        public AT_ResourceAllocation GetResourceByID(long _ResourceAllocationID)
        {
            return db.Repository<AT_ResourceAllocation>().FindById(_ResourceAllocationID);
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
            return db.ExtRepositoryFor<AccountsRepository>().BindUnAssignedKeyParts(_AssetTypeID, _ExpenseID);
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
            return db.ExtRepositoryFor<AccountsRepository>().BindAssignedKeyParts(_AssetTypeID, _ExpenseID);
        }

        /// <summary>
        /// This function gets the Account Setup values based on ID
        /// Created By 08-05-2017
        /// </summary>
        /// <param name="_AccountSetupID"></param>
        /// <returns>double</returns>
        public double GetAccountSetupValue(long _AccountSetupID)
        {
            return db.Repository<AT_AccountsSetup>().FindById(_AccountSetupID).Amount;
        }

        /// <summary>
        /// This function saves the key parts related to expense.
        /// Created On 09-05-2017 
        /// </summary>
        /// <param name="_LstPartExpense"></param>
        public void SaveKeyPartsOfExpense(List<AT_ExpenseOfParts> _LstPartExpense)
        {
            db.ExtRepositoryFor<AccountsRepository>().SaveKeyPartsOfExpense(_LstPartExpense);
        }

        /// <summary>
        /// This function saves the quotations related to expense.
        /// Created On 09-05-2017 
        /// </summary>
        /// <param name="_LstExpenseQuotation"></param>
        public void SaveQuotationsOfExpense(List<AT_ExpenseQuotation> _LstExpenseQuotation)
        {
            db.ExtRepositoryFor<AccountsRepository>().SaveQuotationsOfExpense(_LstExpenseQuotation);
        }

        /// <summary>
        /// This function gets daily rate by resource ID
        /// Created On 12-05-2017
        /// </summary>
        /// <param name="_ResourceAllocationID"></param>
        /// <returns>AT_DailyRate</returns>
        public AT_DailyRate GetDailyRateByResourceID(long _ResourceAllocationID)
        {
            return db.ExtRepositoryFor<AccountsRepository>().GetDailyRateByResourceID(_ResourceAllocationID);
        }

        /// <summary>
        /// This function gets the Quotations against an expense.
        /// Created On 19-05-2017
        /// </summary>
        /// <param name="_MonthlyExpenseID"></param>
        /// <returns>List<AT_ExpenseQuotation></returns>
        public List<AT_ExpenseQuotation> GetExpenseQuotations(long _MonthlyExpenseID)
        {
            return db.Repository<AT_ExpenseQuotation>().GetAll().Where(eq => eq.MonthlyExpenseID == _MonthlyExpenseID).ToList();
        }

        /// <summary>
        /// This function deletes the quotation with given ID.
        /// Created On 19-05-2017
        /// </summary>
        /// <param name="_ExpenseQuotationID"></param>
        public void DeleteQuotation(long _ExpenseQuotationID)
        {
            db.Repository<AT_ExpenseQuotation>().Delete(_ExpenseQuotationID);

            db.Save();
        }

        /// <summary>
        /// This function adds a new quotation.
        /// Created On 22-05-2017
        /// </summary>
        /// <param name="_MdlExpenseQuotation"></param>
        public void AddQuotation(AT_ExpenseQuotation _MdlExpenseQuotation)
        {
            db.Repository<AT_ExpenseQuotation>().Insert(_MdlExpenseQuotation);

            db.Save();
        }

        /// <summary>
        /// This function gets Asset Allocation data based on ID
        /// Created On 23-05-2017
        /// </summary>
        /// <param name="_AssetAllocationID"></param>
        /// <returns>AT_AssetAllocation</returns>
        public AT_AssetAllocation GetAssetAllocationByID(long _AssetAllocationID)
        {
            return db.Repository<AT_AssetAllocation>().FindById(_AssetAllocationID);
        }

        /// <summary>
        /// This function deletes key parts based on the Monthly Expense ID.
        /// Created On 23-05-2017
        /// </summary>        
        /// <returns>List<AT_ExpenseOfParts></returns>
        public void DeletePartsByExpenseID(long _MonthlyExpenseID)
        {
            List<AT_ExpenseOfParts> lstExpenseOfParts = db.Repository<AT_ExpenseOfParts>().GetAll().Where(eop => eop.MonthlyExpenseID == _MonthlyExpenseID).ToList();

            foreach (AT_ExpenseOfParts mdlExpenseOfParts in lstExpenseOfParts)
            {
                db.Repository<AT_ExpenseOfParts>().Delete(mdlExpenseOfParts);

                db.Save();
            }
        }

        /// <summary>
        /// This function returns if 4 wheel or 2 wheel vehicle is assigned to resource
        /// Created on 17-07-2017
        /// </summary>
        /// <param name="_ResourseAllocationID"></param>
        /// <returns>bool</returns>
        public bool IsFourOrTwoWheelVehicleAssigned(long _ResourseAllocationID)
        {
            return db.Repository<AT_AssetAllocation>().GetAll().Any(aa => aa.ResourceAllocationID == _ResourseAllocationID && aa.AssetTypeID != (long)Constants.AssetType.Other);
        }

        #endregion

        #region FundRelease

        public List<dynamic> GetFundRelaseByFinancialYear(string _FinancialYear)
        {
            //List<AT_FundRelease> lstFundRelease = db.Repository<AT_FundRelease>().GetAll().Where(x => x.FinancialYear == _FinancialYear).ToList<AT_FundRelease>();
            //return lstFundRelease;
            List<dynamic> lstFundRelease = db.ExtRepositoryFor<AccountsRepository>().GetFundRelaseByFinancialYear(_FinancialYear);
            return lstFundRelease;
        }

        public bool CheckFundRelaseDetail(long _FundReleaseTypeID, string _FinancialYear, DateTime FundReleaseDate)
        {
            bool FundRelaseDetail = db.ExtRepositoryFor<AccountsRepository>().CheckFundRelaseDetail(_FundReleaseTypeID, _FinancialYear, FundReleaseDate);
            return FundRelaseDetail;

        }

        public bool AddFundRelease(AT_FundRelease _FundRelease)
        {
            db.Repository<AT_FundRelease>().Insert(_FundRelease);
            db.Save();
            return true;
        }

        public bool UpdateFundRelease(AT_FundRelease _FundRelease)
        {
            AT_FundRelease mdlFundRelease = db.Repository<AT_FundRelease>().FindById(_FundRelease.ID);
            mdlFundRelease.FinancialYear = _FundRelease.FinancialYear;
            mdlFundRelease.FundReleaseTypeID = _FundRelease.FundReleaseTypeID;
            mdlFundRelease.FundReleaseDate = _FundRelease.FundReleaseDate;
            mdlFundRelease.Description = _FundRelease.Description;
            mdlFundRelease.ModifiedBy = _FundRelease.ModifiedBy;
            mdlFundRelease.ModifiedDate = _FundRelease.ModifiedDate;

            db.Repository<AT_FundRelease>().Update(mdlFundRelease);
            db.Save();
            return true;
        }

        public bool DeleteFundRelease(long _FundReleaseID)
        {
            db.Repository<AT_FundRelease>().Delete(_FundReleaseID);
            db.Save();
            return true;
        }

        public List<object> GetFundReleaseDetails(string _FinancialYear, long _FundReleaseID)
        {
            //List<long?> lstObjectClassificationID = db.Repository<AT_BudgetApprovel>().GetAll().Where(x => x.FinancialYear == _FinancialYear).Select(x => x.ObjectClassificationID).Distinct().ToList();

            //List<AT_BudgetApprovel> lstFundReleaseDetails = db.Repository<AT_BudgetApprovel>().GetAll().Where(x => x.FinancialYear == _FinancialYear && lstObjectClassificationID.Contains(x.ObjectClassificationID)).Distinct().OrderBy(x => x.BudgetDate).ToList();
            //return lstFundReleaseDetails;
            return db.ExtRepositoryFor<AccountsRepository>().GetBudgetApproval(_FinancialYear, _FundReleaseID);

        }

        public bool AddFundReleaseDetails(AT_FundReleaseDetails _FundReleaseDetails)
        {
            db.Repository<AT_FundReleaseDetails>().Insert(_FundReleaseDetails);
            db.Save();
            return true;
        }

        public AT_FundRelease GetFundReleaseDetailsByID(long _FundReleaseID)
        {
            AT_FundRelease mdlFundReleaseDetails = db.Repository<AT_FundRelease>().GetAll().Where(x => x.ID == _FundReleaseID).FirstOrDefault();
            return mdlFundReleaseDetails;
        }

        public AT_FundRelease GetLatestReleaseByYear(string _FinancialYear, long _FundReleaseID)
        {
            return db.Repository<AT_FundRelease>().GetAll().Where(fr => fr.FinancialYear == _FinancialYear && (fr.ID != _FundReleaseID || _FundReleaseID == 0)).OrderByDescending(fr => fr.FundReleaseDate).FirstOrDefault();
        }

        public bool GetFundReleaseDetailsByFundReleaseID(long _FundReleaseID)
        {
            bool IsFundReleaseIDExist = db.Repository<AT_FundReleaseDetails>().GetAll().Any(x => x.FundReleaseID == _FundReleaseID);
            return IsFundReleaseIDExist;
        }
        public bool IsFundReleaseUnique(long _FundReleaseTypeID, string _FinancialYear)
        {
            bool IsFundReleaseUnique = db.Repository<AT_FundRelease>().GetAll().Any(x => x.FundReleaseTypeID == _FundReleaseTypeID && x.FinancialYear == _FinancialYear && x.AT_FundReleaseTypes.TypeName != "Supplementary Grant" && x.AT_FundReleaseTypes.TypeName != "Reappropriation");
            // bool IsFundReleaseUnique = db.Repository<AT_FundRelease>().GetAll().Any(x => x.FundReleaseTypeID == _FundReleaseTypeID && x.FinancialYear == _FinancialYearbool IsFundReleaseUnique = db.Repository<AT_FundRelease>().GetAll().Any(x => x.FundReleaseTypeID == _FundReleaseTypeID && x.FinancialYear == _FinancialYear && x.FundReleaseDate.Value.Year == FundReleaseDate.Year && x.FundReleaseDate.Value.Month == FundReleaseDate.Month && x.FundReleaseDate.Value.Day == FundReleaseDate.Day && x.AT_FundReleaseTypes.TypeName != "Supplementary Grant" && x.AT_FundReleaseTypes.TypeName != "Reappropriation"););
            return IsFundReleaseUnique;

        }
        public bool IsFundRelease(long _FundReleaseID, string _FinancialYear, DateTime FundReleaseDate)
        {
            // bool IsFundReleaseUnique = db.Repository<AT_FundRelease>().GetAll().Any(x => (x.AT_FundReleaseTypes.TypeName == "Supplementary Grant" || x.AT_FundReleaseTypes.TypeName == "Reappropriation") && x.FinancialYear == _FinancialYear && x.FundReleaseDate.Value.Year == FundReleaseDate.Year && x.FundReleaseDate.Value.Month == FundReleaseDate.Month && x.FundReleaseDate.Value.Day == FundReleaseDate.Day);
            bool IsFundReleaseUnique = db.Repository<AT_FundRelease>().GetAll().Any(x => x.FinancialYear == _FinancialYear && x.FundReleaseDate.Value.Year == FundReleaseDate.Year && x.FundReleaseDate.Value.Month == FundReleaseDate.Month && x.FundReleaseDate.Value.Day == FundReleaseDate.Day && (x.ID != _FundReleaseID || _FundReleaseID == 0));
            return IsFundReleaseUnique;
        }
        #endregion

        #region Sanction

        public List<AT_Sanction> GetSanctions(string _SanctionNo, long _SanctionType, long _SanctionOn, string _FinancialYear, string _Month, long _SanctionStatus)
        {
            List<AT_Sanction> lstSanctions = db.Repository<AT_Sanction>().GetAll().Where(x => (x.SanctionNo == _SanctionNo || _SanctionNo == "") &&
                ((x.ExpenseTypeID == _SanctionType || _SanctionType == 0) && (x.SanctionTypeName.Contains("Misc. – ") || _SanctionType != 0) || _SanctionType == -1) &&
                (x.AssetTypeID == _SanctionOn || _SanctionOn == -1) &&
                (x.FinancialYear == _FinancialYear || _FinancialYear == "") &&
                (x.Month.ToUpper().Trim() == _Month.ToUpper().Trim() || _Month == "") &&
                (x.SanctionStatusID == _SanctionStatus || _SanctionStatus == -1)).ToList<AT_Sanction>();
            return lstSanctions;
        }

        public List<AT_AssetType> GetAssetTypes()
        {
            List<AT_AssetType> lstAssetType = db.Repository<AT_AssetType>().GetAll().ToList<AT_AssetType>();
            return lstAssetType;
        }

        public List<AT_SanctionStatus> GetSanctionStatus()
        {
            List<AT_SanctionStatus> lstSanctionStatus = db.Repository<AT_SanctionStatus>().GetAll().Where(x => x.ID != (long)Constants.SanctionStatus.WaitingforApproval && x.ID != (long)Constants.SanctionStatus.PaymentReleased).ToList<AT_SanctionStatus>();
            return lstSanctionStatus;
        }

        public bool UpdateSanctionOnSearchSanction(AT_Sanction _Sanction)
        {
            AT_Sanction mdlSanction = db.Repository<AT_Sanction>().FindById(_Sanction.ID);
            mdlSanction.SanctionStatusDate = _Sanction.SanctionStatusDate;
            mdlSanction.SanctionStatusID = _Sanction.SanctionStatusID;

            if (_Sanction.SanctionStatusID == (long)Constants.SanctionStatus.SenttoAGOffice)
            {
                mdlSanction.TokenNumber = _Sanction.TokenNumber;
            }

            mdlSanction.ModifiedBy = _Sanction.ModifiedBy;
            mdlSanction.ModifiedDate = _Sanction.ModifiedDate;

            db.Repository<AT_Sanction>().Update(mdlSanction);
            db.Save();
            return true;
        }

        /// <summary>
        /// This function update the payment date for a particular cheque.
        /// Created On 24-01-2018
        /// </summary>
        /// <param name="_MdlPaymentDetails"></param>
        /// <returns>bool</returns>
        public bool UpdateChequePaymentDate(AT_PaymentDetails _MdlPaymentDetails)
        {
            AT_PaymentDetails mdlPaymentDetails = db.Repository<AT_PaymentDetails>().FindById(_MdlPaymentDetails.ID);

            mdlPaymentDetails.PaymentDate = _MdlPaymentDetails.PaymentDate;
            mdlPaymentDetails.ModifiedBy = _MdlPaymentDetails.ModifiedBy;
            mdlPaymentDetails.ModifiedDate = _MdlPaymentDetails.ModifiedDate;

            db.Repository<AT_PaymentDetails>().Update(mdlPaymentDetails);
            db.Save();

            return true;
        }

        public List<AT_Sanction> GetSanctions(long _SanctionType)
        {
            List<AT_Sanction> lstSanctions = db.Repository<AT_Sanction>().GetAll().Where(x => (x.ExpenseTypeID == _SanctionType || _SanctionType == -1) && (x.SanctionStatusID == (long)Constants.SanctionStatus.SenttoAGOffice)).ToList<AT_Sanction>();
            return lstSanctions;
        }

        public bool AddPaymentDetails(AT_PaymentDetails _PaymentDetails)
        {
            db.Repository<AT_PaymentDetails>().Insert(_PaymentDetails);
            db.Save();
            return true;
        }

        public bool AddSanctionPayment(AT_SanctionPayment _SanctionPayment)
        {
            db.Repository<AT_SanctionPayment>().Insert(_SanctionPayment);
            db.Save();
            return true;
        }

        public void SetSanctionExpenseStatus(long _SanctionID)
        {
            List<AT_ExpenseSanction> lstExpSanc = db.Repository<AT_ExpenseSanction>().GetAll().Where(x => x.SanctionID == _SanctionID).ToList<AT_ExpenseSanction>();
            foreach (AT_ExpenseSanction mdlExpsanc in lstExpSanc)
            {
                AT_MonthlyExpenses mdlMonthlyExpense = db.Repository<AT_MonthlyExpenses>().FindById(mdlExpsanc.MonthlyExpenseID);
                mdlMonthlyExpense.ExpenseStatusID = (long)Constants.SanctionStatus.PaymentReleased;
                db.Repository<AT_MonthlyExpenses>().Update(mdlMonthlyExpense);
                db.Save();
            }
        }

        public AT_PaymentDetails GetLatestPaymentDetail()
        {
            AT_PaymentDetails mdlPaymentDetail = db.Repository<AT_PaymentDetails>().GetAll().OrderByDescending(x => x.ID).FirstOrDefault();
            return mdlPaymentDetail;
        }

        public List<AT_ObjectClassification> GetAllObjectClassifications()
        {
            List<AT_ObjectClassification> lstPaymentDetail = db.Repository<AT_ObjectClassification>().GetAll().ToList<AT_ObjectClassification>();
            return lstPaymentDetail;
        }

        public List<dynamic> GetAllObjectClassificationsAndCode()
        {
            List<dynamic> lstPaymentDetail = db.Repository<AT_ObjectClassification>().GetAll().Select(x => new { ID = x.ID, Name = x.AccountsCode + " (" + x.ObjectClassification + ")" }).ToList<dynamic>();
            return lstPaymentDetail;
        }

        public bool AddSanction(AT_Sanction _Sanction)
        {
            db.Repository<AT_Sanction>().Insert(_Sanction);
            db.Save();
            return true;
        }

        public bool UpdateSanction(AT_Sanction _Sanction)
        {
            AT_Sanction mdlSanction = db.Repository<AT_Sanction>().FindById(_Sanction.ID);
            mdlSanction.Month = _Sanction.Month;
            mdlSanction.SanctionNo = _Sanction.SanctionNo;
            mdlSanction.SanctionTypeName = _Sanction.SanctionTypeName;
            mdlSanction.ObjectClassificationID = _Sanction.ObjectClassificationID;
            mdlSanction.SanctionAmount = _Sanction.SanctionAmount;
            mdlSanction.SanctionStatusID = _Sanction.SanctionStatusID;
            mdlSanction.SanctionStatusDate = _Sanction.SanctionStatusDate;
            mdlSanction.ModifiedBy = _Sanction.ModifiedBy;
            mdlSanction.ModifiedDate = _Sanction.ModifiedDate;

            db.Repository<AT_Sanction>().Update(mdlSanction);
            db.Save();
            return true;
        }

        public bool DeleteSanction(long _Sanction)
        {
            db.Repository<AT_Sanction>().Delete(_Sanction);
            db.Save();
            return true;
        }

        public bool IsSanctionNoExist(string _SanctionNo)
        {
            bool IsExist = db.Repository<AT_Sanction>().GetAll().Any(x => x.SanctionNo.Trim().ToUpper() == _SanctionNo.Trim().ToUpper());
            return IsExist;
        }

        public DataSet SearchCheque(string ChequeNo, string ChequeFromDate, string ChequeToDate, double? ChequeFromAmount, double? ChequeToAmount)
        {
            return db.ExecuteStoredProcedureDataSet("AT_SearchCheque", ChequeNo, ChequeFromDate, ChequeToDate, ChequeFromAmount, ChequeToAmount);
        }

        public DataSet GetPaymentDetailsByID(long _PaymentDetailID)
        {
            return db.ExecuteStoredProcedureDataSet("AT_GetPaymentDetailsByID", _PaymentDetailID);
        }

        public bool DeleteCheque(long _Cheque)
        {
            db.Repository<AT_PaymentDetails>().Delete(_Cheque);
            db.Save();
            return true;
        }

        public bool UpdateSanctionStatusInMonthlyExpense(long _SanctionID, long _StatusID, int _UserID)
        {
            return db.ExtRepositoryFor<AccountsRepository>().UpdateSanctionStatusInMonthlyExpense(_SanctionID, _StatusID, _UserID);
        }
        #endregion

        #region New Sanction
        public string GetSanctionNumber(string _Prefix)
        {
            return db.Repository<AT_Sanction>().GetAll().Where(me => me.SanctionNo.Contains(_Prefix)).OrderByDescending(me => me.ID).Select(me => me.SanctionNo).FirstOrDefault();
        }
        public object GetApprovalBudget_ReleaseAmountByFinancialYear(string _FinancialYear)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("AT_GetApprovalBudget_ReleaseAmount_ByFinancialYear", _FinancialYear);
            long BudgetApprove = 0;
            long CurrentBudgetRelease = 0;
            foreach (DataRow DR in dt.Rows)
            {
                BudgetApprove = Convert.ToInt64(DR["BudgetApproval"]);
                BudgetApprove = Convert.ToInt64(DR["BudgetApproval"]);
                CurrentBudgetRelease = Convert.ToInt64(DR["CurrentBudgetRelease"]);
            }
            return new { BA = BudgetApprove, CBR = CurrentBudgetRelease };
        }
        public List<object> RMGridData(string _FinancialYear, string _Month, long _ExpneseTypeID, string _AssetType, long _AccountOfficer)
        {

            DataTable dt = db.ExecuteStoredProcedureDataTable("AT_GetSanction", _FinancialYear, _Month, _ExpneseTypeID, _AssetType, "RepairMaintenance", _AccountOfficer);
            List<object> lstObject = new List<object>();
            foreach (DataRow DR in dt.Rows)
            {
                lstObject.Add(
                    new
                    {
                        ID = Convert.ToInt64(DR["ID"]),
                        NameOfStaff = Convert.ToString(DR["NameOfStaff"] == DBNull.Value ? "" : DR["NameOfStaff"]),
                        Designation = Convert.ToString(DR["Designation"] == DBNull.Value ? "" : DR["Designation"]),
                        BillNo = Convert.ToString(DR["BillNo"] == DBNull.Value ? "" : DR["BillNo"]),
                        BillDate = DR["BillDate"] == DBNull.Value ? "" : Utility.GetFormattedDate(Convert.ToDateTime(DR["BillDate"])),
                        AssetName = Convert.ToString(DR["AssetName"] == DBNull.Value ? "" : DR["AssetName"]),
                        AssetType = Convert.ToString(DR["AssetType"] == DBNull.Value ? "" : DR["AssetType"]),
                        PurchaseItems = Convert.ToInt64(DR["PurchaseItems"] == DBNull.Value ? 0 : DR["PurchaseItems"]),
                        RepairItems = Convert.ToInt64(DR["RepairItems"] == DBNull.Value ? 0 : DR["RepairItems"]),
                        TotalClaim = Convert.ToInt64(DR["TotalClaim"] == DBNull.Value ? 0 : DR["TotalClaim"]).ToString(),
                        SanctionStatus = Convert.ToString(DR["SanctionStatus"] == DBNull.Value ? "" : DR["SanctionStatus"]),
                        VendorType = Convert.ToString(DR["VendorType"] == DBNull.Value ? "" : DR["VendorType"]),
                        ObjectClassification = Convert.ToString(DR["ObjectClassification"] == DBNull.Value ? "" : DR["ObjectClassification"]),
                        AssetTypeID = Convert.ToString(DR["AssetTypeID"] == DBNull.Value ? "" : DR["AssetTypeID"])
                    }
                    );
            }

            return lstObject;
            //long GrandTotal = 0;
            //foreach (object item in lstObject)
            //{
            //    GrandTotal = GrandTotal + Convert.ToInt64(item.GetType().GetProperty("TotalClaim").GetValue(item));
            //}

        }
        public List<object> POLGridData(string _FinancialYear, string _Month, long _ExpneseTypeID, string _AssetType, long _AccountOfficer)
        {

            DataTable dt = db.ExecuteStoredProcedureDataTable("AT_GetSanction", _FinancialYear, _Month, _ExpneseTypeID, _AssetType, "POLRcpt", _AccountOfficer);
            List<object> lstObject = new List<object>();
            foreach (DataRow DR in dt.Rows)
            {
                lstObject.Add(
                    new
                    {
                        ID = Convert.ToInt64(DR["ID"]),
                        NameOfStaff = Convert.ToString(DR["NameOfStaff"] == DBNull.Value ? "" : DR["NameOfStaff"]),
                        Designation = Convert.ToString(DR["Designation"] == DBNull.Value ? "" : DR["Designation"]),
                        BillNo = Convert.ToString(DR["BillNo"] == DBNull.Value ? "" : DR["BillNo"]),
                        MeterReading = Convert.ToString(DR["MeterReading"] == DBNull.Value ? "" : DR["MeterReading"]),
                        POLReceiptNo = Convert.ToString(DR["POLReceiptNo"] == DBNull.Value ? "" : DR["POLReceiptNo"]),
                        POLDatetime = DR["POLDatetime"] == DBNull.Value ? "" : Utility.GetFormattedDate(Convert.ToDateTime(DR["POLDatetime"])),
                        SanctionStatus = "Waiting For Approval",
                        AmountRs = Convert.ToInt64(DR["Amount"] == DBNull.Value ? 0 : DR["Amount"])
                    }
                    );
            }
            return lstObject;
        }
        public List<object> TADAGridData(string _FinancialYear, string _Month, long _ExpenseTypeID, long _AccountOfficer)
        {

            DataTable dt = db.ExecuteStoredProcedureDataTable("AT_GetSanction", _FinancialYear, _Month, _ExpenseTypeID, "", "TADA", _AccountOfficer);
            List<object> lstObject = new List<object>();
            foreach (DataRow DR in dt.Rows)
            {
                lstObject.Add(
                    new
                    {
                        ID = Convert.ToInt64(DR["ID"]),
                        NameOfStaff = Convert.ToString(DR["NameOfStaff"] == DBNull.Value ? "" : DR["NameOfStaff"]),
                        Designation = Convert.ToString(DR["Designation"] == DBNull.Value ? "" : DR["Designation"]),
                        BillNo = Convert.ToString(DR["BillNo"] == DBNull.Value ? "" : DR["BillNo"]),
                        HalfDailiesOrdinary = Convert.ToString(DR["HalfDailiesOrdinary"] == DBNull.Value ? "" : DR["HalfDailiesOrdinary"]),
                        FullDailiesOrdinary = Convert.ToString(DR["FullDailiesOrdinary"] == DBNull.Value ? "" : DR["FullDailiesOrdinary"]),
                        HalfDailiesSpecial = Convert.ToInt64(DR["HalfDailiesSpecial"] == DBNull.Value ? 0 : DR["HalfDailiesSpecial"]),
                        FullDailiesSpecial = Convert.ToInt64(DR["FullDailiesSpecial"] == DBNull.Value ? 0 : DR["FullDailiesSpecial"]),
                        TotalKMPublicTransport = Convert.ToInt64(DR["TotalKMPublicTransport"] == DBNull.Value ? 0 : DR["TotalKMPublicTransport"]),
                        TotalKMIrrigationVehicle = Convert.ToString(DR["TotalKMIrrigationVehicle"] == DBNull.Value ? "" : DR["TotalKMIrrigationVehicle"]),
                        MiscExpenses = Convert.ToString(DR["MiscExpenses"] == DBNull.Value ? 0 : DR["MiscExpenses"]),
                        SanctionStatus = "Waiting For Approval",
                        TotalTADA = Convert.ToString(DR["TotalTADA"] == DBNull.Value ? 0 : DR["TotalTADA"])
                    }
                    );
            }
            return lstObject;
        }
        public List<object> NPGridData(string _FinancialYear, string _Month, long _AssetTypeID, long _AccountOfficer)
        {

            DataTable dt = db.ExecuteStoredProcedureDataTable("AT_GetSanction", _FinancialYear, _Month, _AssetTypeID, "", "NewPurchase", _AccountOfficer);
            List<object> lstObject = new List<object>();
            foreach (DataRow DR in dt.Rows)
            {
                lstObject.Add(
                    new
                    {
                        ID = Convert.ToInt64(DR["ID"]),
                        NameOfStaff = Convert.ToString(DR["NameOfStaff"] == DBNull.Value ? "" : DR["NameOfStaff"]),
                        Designation = Convert.ToString(DR["Designation"] == DBNull.Value ? "" : DR["Designation"]),
                        BillNo = Convert.ToString(DR["BillNo"] == DBNull.Value ? "" : DR["BillNo"]),
                        BillDate = DR["BillDate"] == DBNull.Value ? "" : Utility.GetFormattedDate(Convert.ToDateTime(DR["BillDate"])),
                        SanctionStatus = "Waiting For Approval",
                        PurchaseItemAmount = Convert.ToString(DR["PurchaseItemAmount"] == DBNull.Value ? "" : DR["PurchaseItemAmount"]),
                        PurchaseItemName = Convert.ToString(DR["PurchaseItemName"] == DBNull.Value ? "" : DR["PurchaseItemName"])
                    }
                    );
            }
            return lstObject;
        }

        public List<object> OEGridData(string _FinancialYear, string _Month, long _AssetTypeID, long _AccountOfficer)
        {

            DataTable dt = db.ExecuteStoredProcedureDataTable("AT_GetSanction", _FinancialYear, _Month, _AssetTypeID, "", "OtherExpense", _AccountOfficer);
            List<object> lstObject = new List<object>();
            foreach (DataRow DR in dt.Rows)
            {
                lstObject.Add(
                    new
                    {
                        ID = Convert.ToInt64(DR["ID"]),
                        NameOfStaff = Convert.ToString(DR["NameOfStaff"] == DBNull.Value ? "" : DR["NameOfStaff"]),
                        Designation = Convert.ToString(DR["Designation"] == DBNull.Value ? "" : DR["Designation"]),
                        BillNo = Convert.ToString(DR["BillNo"] == DBNull.Value ? "" : DR["BillNo"]),
                        BillDate = DR["BillDate"] == DBNull.Value ? "" : Utility.GetFormattedDate(Convert.ToDateTime(DR["BillDate"])),
                        SanctionStatus = "Waiting For Approval",
                        ExpenseName = Convert.ToString(DR["ExpenseName"] == DBNull.Value ? "" : DR["ExpenseName"]),
                        ExpenseAmount = Convert.ToString(DR["ExpenseAmount"] == DBNull.Value ? "" : DR["ExpenseAmount"])
                    }
                    );
            }
            return lstObject;
        }


        public bool SaveSanctionedDAL(List<AT_Sanction> _lstSanctioned, List<AT_ExpenseSanction> _es)
        {

            db.Repository<AT_Sanction>().Insert(_lstSanctioned[0]);
            db.Save();





            for (int i = 0; i < _es.Count; i++)
            {
                _es[i].SanctionID = _lstSanctioned[0].ID;
                AT_MonthlyExpenses ame = db.Repository<AT_MonthlyExpenses>().FindById(_es[i].MonthlyExpenseID);
                ame.ExpenseStatusID = Convert.ToInt64(_es[i].Status);
                if (!string.IsNullOrEmpty(_es[i].AT_MonthlyExpenses.ReasonOfRejection))
                {
                    ame.ReasonOfRejection = _es[i].AT_MonthlyExpenses.ReasonOfRejection;
                }

                db.Repository<AT_MonthlyExpenses>().Update(ame);
                db.Save();
                _es[i].AT_MonthlyExpenses = null;
                if (_es != null)
                {
                    if (ame.ExpenseStatusID == (long)Constants.SanctionStatus.Sanctioned)
                    {
                        db.Repository<AT_ExpenseSanction>().Insert(_es[i]);
                        db.Save();
                    }

                }



            }


            return true;


        }
        #endregion

        #region Tax Sheet

        public List<dynamic> GetTaxSheet(long _SanctionID)
        {
            return db.ExtRepositoryFor<AccountsRepository>().GetTaxSheet(_SanctionID);
        }

        public double? GetTaxRateByID(long _TaxRateID)
        {
            double? TaxRate = db.Repository<AT_TaxRate>().GetAll().Where(x => x.ID == _TaxRateID).Select(x => x.TaxRateInPercentage).FirstOrDefault();
            return TaxRate;
        }

        public AT_Sanction GetSanctionByID(long _SanctionID)
        {
            AT_Sanction mdlSanction = db.Repository<AT_Sanction>().GetAll().Where(x => x.ID == _SanctionID).FirstOrDefault();
            return mdlSanction;
        }

        public double GetExpenseLimitForTax()
        {
            double ExpenseLimitForTax = db.Repository<AT_AccountsSetup>().GetAll().Where(x => x.ID == 1).Select(x => x.Amount).FirstOrDefault();
            return ExpenseLimitForTax;
        }
        #endregion

        public bool UpdateMonthlyExpenseStatus(long _ResourceAllocationID, string _FinancialYear, string _Month, string _PMIUStaff, long _ExpenseMadeBy, long _ModifiedBy, string _UserType = "AO")
        {


            int value = db.ExecuteScalar("AT_UpdateExpenseStatusID", _FinancialYear, _Month, _ExpenseMadeBy, _PMIUStaff, _ModifiedBy, _UserType, _ResourceAllocationID);
            if (value > 0)
            {
                return true;
            }
            return false;
        }


        #region Budget Utilization
        public List<object> GetBudgetUtilizationGridData(string _FinancialYear, string _Month, long _ObjectClassificationID)
        {
            //Constants.SanctionStatus.WaitingforApproval.GetType();           
            List<object> lstBudgetUtilization = (from bu in db.Repository<AT_Sanction>().GetAll() where bu.FinancialYear == _FinancialYear && bu.Month == _Month && bu.ObjectClassificationID == _ObjectClassificationID && (bu.SanctionStatusID == (long)Constants.SanctionStatus.Sanctioned || bu.SanctionStatusID == (long)Constants.SanctionStatus.SenttoAGOffice || bu.SanctionStatusID == (long)Constants.SanctionStatus.WaitingforApproval) select new { SanctionDate = bu.CreatedDate, SancionAmount = bu.SanctionAmount, SanctionStatus = bu.AT_SanctionStatus.Name }).ToList<object>();
            return lstBudgetUtilization;
        }
        #endregion

        public bool IsSanctionedAmountExceed(string _FinancialYear, double _SanctionedAmount, long _ObjectClassificationID)
        {
            double? SanctionedAmount = (from sanc in db.Repository<AT_Sanction>().GetAll() where sanc.FinancialYear == _FinancialYear && sanc.ObjectClassificationID == _ObjectClassificationID && (sanc.SanctionStatusID == (long)Constants.SanctionStatus.Sanctioned || sanc.SanctionStatusID == (long)Constants.SanctionStatus.SenttoAGOffice || sanc.SanctionStatusID == (long)Constants.SanctionStatus.WaitingforApproval) select sanc.SanctionAmount).Sum();
            double? ReleaseAmount = (from fundRelease in db.Repository<AT_FundRelease>().GetAll()
                                     join relDetail in db.Repository<AT_FundReleaseDetails>().GetAll()
                                         on fundRelease.ID equals relDetail.FundReleaseID
                                     where fundRelease.FinancialYear == _FinancialYear && relDetail.ObjectClassificationID == _ObjectClassificationID && relDetail.CurrentReleaseAmount > 0
                                     select relDetail.CurrentReleaseAmount).Sum();

            SanctionedAmount = SanctionedAmount.HasValue ? SanctionedAmount.Value : 0;
            ReleaseAmount = ReleaseAmount.HasValue ? ReleaseAmount.Value : 0;

            double difference = ReleaseAmount.Value - SanctionedAmount.Value;

            if (difference >= _SanctionedAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
