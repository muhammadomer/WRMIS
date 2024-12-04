using PMIU.WRMIS.DAL.Repositories.Accounts;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.Accounts
{
    public class ReferenceDataDAL
    {
        ContextDB db = new ContextDB();

        #region Tax Rate

        /// <summary>
        /// This function adds the default tax data
        /// Created on 30-03-2017
        /// </summary>
        /// <param name="_LstTaxRates"></param>
        public void AddTaxData(List<AT_TaxRate> _LstTaxRates)
        {
            db.ExtRepositoryFor<ReferenceDataRepository>().AddTaxData(_LstTaxRates);
        }

        /// <summary>
        /// This function gets tax data for a particular Transaction and Vendor type
        /// Created On 30-03-2017
        /// </summary>
        /// <param name="_TransactionType"></param>
        /// <param name="_VendorType"></param>
        /// <returns>List<AT_TaxRate></returns>
        public List<AT_TaxRate> GetTaxData(string _TransactionType, string _VendorType)
        {
            return db.Repository<AT_TaxRate>().GetAll().Where(tr => tr.TransactionType == _TransactionType && tr.VendorType == _VendorType).ToList();
        }

        /// <summary>
        /// This function finds tax data by ID
        /// Created On 31-03-2017
        /// </summary>
        /// <param name="_TaxDataID"></param>
        /// <returns>AT_TaxRate</returns>
        public AT_TaxRate GetTaxDataByID(long _TaxDataID)
        {
            return db.Repository<AT_TaxRate>().FindById(_TaxDataID);
        }

        /// <summary>
        /// This function updates the tax data
        /// Created On 31-03-2017
        /// </summary>
        /// <param name="_MdlTaxRate"></param>        
        public void UpdateTaxData(AT_TaxRate _MdlTaxRate)
        {
            db.Repository<AT_TaxRate>().Update(_MdlTaxRate);
            db.Save();
        }

        #endregion

        #region Daily Rate

        /// <summary>
        /// This function get all the daily rates
        /// Created On 31-03-2017
        /// </summary>
        /// <returns>List<AT_DailyRate></returns>
        public List<AT_DailyRate> GetDailyRates()
        {
            return db.Repository<AT_DailyRate>().GetAll().ToList();
        }

        /// <summary>
        /// This function finds daily rate data by ID
        /// Created On 31-03-2017
        /// </summary>
        /// <param name="_DailyRateID"></param>
        /// <returns>AT_DailyRate</returns>
        public AT_DailyRate GetDailyRateDateByID(long _DailyRateID)
        {
            return db.Repository<AT_DailyRate>().FindById(_DailyRateID);
        }

        /// <summary>
        /// This function updates the daily rate data
        /// Created On 31-03-2017
        /// </summary>
        /// <param name="_MdlDailyRate"></param>        
        public void UpdateDailyRateData(AT_DailyRate _MdlDailyRate)
        {
            db.Repository<AT_DailyRate>().Update(_MdlDailyRate);
            db.Save();
        }

        /// <summary>
        /// This function get Daily Rate based on BPS
        /// Created On 03-04-2017
        /// </summary>
        /// <param name="_BPS"></param>
        /// <returns>AT_DailyRate</returns>
        public AT_DailyRate GetBPSRate(int _BPS)
        {
            return db.Repository<AT_DailyRate>().GetAll().Where(dr => dr.BPS == _BPS).FirstOrDefault();
        }

        #endregion

        #region Key Part

        /// <summary>
        /// This function gets the key parts based on vehicle type
        /// Created On 04-04-2017
        /// </summary>
        /// <param name="_AssetTypeID"></param>
        /// <returns>List<AT_KeyParts></returns>
        public List<AT_KeyParts> GetKeyPartsByVehicleType(long _AssetTypeID)
        {
            return db.Repository<AT_KeyParts>().GetAll().Where(kp => kp.AssetTypeID == _AssetTypeID).OrderBy(kp => kp.PartName).ToList();
        }

        /// <summary>
        /// This function gets thekey part by vehicle type and part name
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_AssetTypeID"></param>
        /// <param name="_PartName"></param>
        /// <returns>AT_KeyParts</returns>
        public AT_KeyParts GetPartByVehicleTypeAndName(long _AssetTypeID, string _PartName)
        {
            return db.Repository<AT_KeyParts>().GetAll().Where(kp => kp.AssetTypeID == _AssetTypeID &&
                kp.PartName.Replace(" ", "").ToUpper() == _PartName.Replace(" ", "").ToUpper()).FirstOrDefault();
        }

        /// <summary>
        /// This function gets the key parts by ID
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_KeyPartID"></param>
        /// <returns>AT_KeyParts</returns>
        public AT_KeyParts GetKeyPartsByID(long _KeyPartID)
        {
            return db.Repository<AT_KeyParts>().FindById(_KeyPartID);
        }

        /// <summary>
        /// This function adds Key Part
        /// Created on 05-04-2017
        /// </summary>
        /// <param name="_MdlKeyParts"></param>
        public void AddKeyPart(AT_KeyParts _MdlKeyParts)
        {
            db.Repository<AT_KeyParts>().Insert(_MdlKeyParts);

            db.Save();
        }

        /// <summary>
        /// This function update Key Part
        /// Created on 05-04-2017
        /// </summary>
        /// <param name="_MdlKeyParts"></param>
        public void UpdateKeyPart(AT_KeyParts _MdlKeyParts)
        {
            db.Repository<AT_KeyParts>().Update(_MdlKeyParts);

            db.Save();
        }

        /// <summary>
        /// This function checks if KeyPartID reference exists.
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_KeyPartID"></param>
        /// <returns>bool</returns>
        public bool IsKeyPartIDExists(long _KeyPartID)
        {
            return db.Repository<AT_ExpenseOfParts>().GetAll().Any(eof => eof.KeyPartsID == _KeyPartID);
        }

        /// <summary>
        /// This function deletes the KeyPart based on the ID
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_KeyPartID"></param>
        public void DeleteKeyPart(long _KeyPartID)
        {
            db.Repository<AT_KeyParts>().Delete(_KeyPartID);

            db.Save();
        }

        /// <summary>
        /// This function gets the assets type.
        /// Created On 10-04-2017
        /// </summary>
        /// <returns>List<AT_AssetType></returns>
        public List<AT_AssetType> GetAssetType()
        {
            return db.Repository<AT_AssetType>().GetAll().ToList();
        }

        #endregion

        #region Account Setup

        /// <summary>
        /// This function gets the Account Setups
        /// Created On 04-05-2017
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<AT_AccountsSetup></returns>
        public List<object> GetAccountSetups()
        {
            return db.Repository<AT_AccountsSetup>().GetAll().Select(x => new { ID = x.ID, ACRule = x.ACRule, Amount = x.Amount }).ToList<object>();
        }

        /// <summary>
        /// This function Update  Account Setups Amount
        /// Created On 04-05-2017
        /// </summary>
        /// <param name="_ID,_Ammount,_ModifiedBY,_ModifiedDate"></param>
        /// <returns>List<AT_AccountsSetup></returns>
        public bool UpdateAccountSetupsAmount(long _ID, Double _Amount, long _ModifiedBy, DateTime _ModifiedDate)
        {
            AT_AccountsSetup aas = (from acstp in db.Repository<AT_AccountsSetup>().GetAll() where acstp.ID == _ID select acstp).FirstOrDefault();
            aas.Amount = _Amount;
            aas.ModifiedBy = (int)_ModifiedBy;
            aas.ModifiedDate = _ModifiedDate;
            db.Repository<AT_AccountsSetup>().Update(aas);
            db.Save();
            return true;
        }

        #endregion

        #region Fund Release Type

        public List<object> GetFundsReleaseType()
        {
            return db.Repository<AT_FundReleaseTypes>().GetAll().Select(x => new { ID = x.ID, TypeName = x.TypeName, Description = x.Description, IsEdit = x.IsEdit }).ToList<object>();
        }

        public bool SaveFundsReleaseType(AT_FundReleaseTypes _FundsReleaseType)
        {
            if (_FundsReleaseType.ID == 0)
            {
                db.Repository<AT_FundReleaseTypes>().Insert(_FundsReleaseType);
            }
            else
            {
                AT_FundReleaseTypes frt = (from st in db.Repository<AT_FundReleaseTypes>().GetAll() where st.ID == _FundsReleaseType.ID select st).FirstOrDefault();
                frt.TypeName = _FundsReleaseType.TypeName;
                frt.Description = _FundsReleaseType.Description;
                frt.ModifiedDate = _FundsReleaseType.ModifiedDate;
                frt.ModifiedBy = _FundsReleaseType.ModifiedBy;
                db.Repository<AT_FundReleaseTypes>().Update(frt);
            }

            db.Save();

            return true;
        }

        public bool DeleteFundsReleaseType(AT_FundReleaseTypes _FundsReleaseType)
        {

            if (_FundsReleaseType.ID > 0)
            {
                AT_FundReleaseTypes frt = (from st in db.Repository<AT_FundReleaseTypes>().GetAll() where st.ID == _FundsReleaseType.ID select st).FirstOrDefault();
                db.Repository<AT_FundReleaseTypes>().Delete(frt);
                db.Save();
            }

            return true;
        }

        public AT_FundReleaseTypes GetFundsReleaseTypeByName(AT_FundReleaseTypes FundsReleaseTypeName)
        {

            AT_FundReleaseTypes frt = (from st in db.Repository<AT_FundReleaseTypes>().GetAll() where st.TypeName.ToLower().Trim() == FundsReleaseTypeName.TypeName.ToLower().Trim() select st).FirstOrDefault();
            return frt;

        }

        #endregion

        #region Object Classification

        public List<object> GetObjectClassificationByAccoundHeadID(long _AccountHeadID = 0)
        {
            if (_AccountHeadID != 0)
            {
                return db.Repository<AT_ObjectClassification>().GetAll().Where(p => p.AccountHeadID == _AccountHeadID).Select(x => new { ID = x.ID, AccountHeadID = x.AccountHeadID, AccountsCode = x.AccountsCode, ObjectClassification = x.ObjectClassification, isActive = x.IsActive }).OrderBy(p => p.AccountsCode).ToList<object>();
            }
            else
            {
                return db.Repository<AT_ObjectClassification>().GetAll().Select(x => new { ID = x.ID, AccountHeadID = x.AccountHeadID, AccountsCode = x.AccountsCode, ObjectClassification = x.ObjectClassification, isActive = x.IsActive }).OrderBy(p => p.AccountsCode).ToList<object>();
            }
        }

        public bool DeleteObjectClassification(AT_ObjectClassification _ObjectClassification)
        {
            if (_ObjectClassification.ID > 0)
            {
                AT_ObjectClassification frt = (from st in db.Repository<AT_ObjectClassification>().GetAll() where st.ID == _ObjectClassification.ID select st).FirstOrDefault();
                db.Repository<AT_ObjectClassification>().Delete(frt);
                db.Save();
            }

            return true;
        }

        public bool SaveObjectClassification(AT_ObjectClassification _ObjectClassification)
        {
            if (_ObjectClassification.ID == 0)
            {
                db.Repository<AT_ObjectClassification>().Insert(_ObjectClassification);
            }
            else
            {
                AT_ObjectClassification frt = (from st in db.Repository<AT_ObjectClassification>().GetAll() where st.ID == _ObjectClassification.ID select st).FirstOrDefault();
                frt.AccountHeadID = _ObjectClassification.AccountHeadID;
                frt.AccountsCode = _ObjectClassification.AccountsCode;
                frt.ObjectClassification = _ObjectClassification.ObjectClassification;
                frt.ModifiedDate = _ObjectClassification.ModifiedDate;
                frt.ModifiedBy = _ObjectClassification.ModifiedBy;
                db.Repository<AT_ObjectClassification>().Update(frt);
            }

            db.Save();

            return true;
        }

        public AT_ObjectClassification GetObjectClassificationByAccountCode(AT_ObjectClassification _ObjectClassification)
        {
            AT_ObjectClassification ObjClas = (from st in db.Repository<AT_ObjectClassification>().GetAll() where st.AccountsCode.ToLower().Trim() == _ObjectClassification.AccountsCode.ToLower().Trim() || st.ObjectClassification.ToLower().Trim() == _ObjectClassification.ObjectClassification.ToLower().Trim() select st).FirstOrDefault();

            if (ObjClas == null || ObjClas.ID == _ObjectClassification.ID)
            {
                ObjClas = new AT_ObjectClassification();
            }

            return ObjClas;
        }

        #endregion

        #region ResourceAllocation

        public List<object> GetADMUser()
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetADMUser();
        }
        public List<object> GetOfficeUsers(string StaffType)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetOfficeUsers(StaffType);
        }

        public List<object> GetADMDesignationID(int StaffType)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetADMDesignationID(StaffType);
        }

        public List<object> GetResourceAllocationSearch(Int64 _PmiuID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetResourceAllocationSearch(_PmiuID);
        }

        public bool DeleteResourceAllocation(Int64 _ID)
        {
            db.Repository<AT_ResourceAllocation>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsExistResourceAllocation(Int64 _ID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().IsExistResourceAllocation(_ID);
        }

        public bool SaveResourceAllocation(AT_ResourceAllocation _ResourceAllocation)
        {
            bool isSaved = false;

            if (_ResourceAllocation.ID == 0)
                db.Repository<AT_ResourceAllocation>().Insert(_ResourceAllocation);
            else
                db.Repository<AT_ResourceAllocation>().Update(_ResourceAllocation);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public bool IsUSerExits(Int64 _DesignationID)
        {
            return db.Repository<UA_Users>().GetAll().Where(u => u.DesignationID == _DesignationID).Any();
        }

        public List<object> GetNameofStaffByDesignationID(Int64 _DesignationID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetNameofStaffByDesignationID(_DesignationID);
        }

        public bool IsStaffUnique(AT_ResourceAllocation _ResourceAllocation)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().IsStaffUnique(_ResourceAllocation);
        }

        public object GetEmailContact(Int64 _UserID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetEmailContact(_UserID);
        }

        public bool IsUserExist(long _DesignationID)
        {
            bool IsUsrExist = db.Repository<UA_Users>().GetAll().Any(x => x.DesignationID == _DesignationID);
            return IsUsrExist;
        }
        #endregion

        #region AssetsAllocation
        public object GetResourceAllocationData(Int64? _ID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetResourceAllocationData(_ID);
        }

        public List<object> GetAACategory()
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAACategory();
        }
        public List<object> GetAASubCategory(Int64 _CategoryID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAASubCategory(_CategoryID);
        }
        public List<object> GetAAAssetType()
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAAAssetType();
        }
        public List<object> GetAAAssetName(Int64 _SubCategory)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAAAssetName(_SubCategory);
        }
        public List<object> GetAAAssetAttribute(Int64 _AssetID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAAAssetAttribute(_AssetID);
        }

        public List<object> GetAssetAllocation(Int64 _ResourceAllocationID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAssetAllocation(_ResourceAllocationID);
        }

        public AM_AssetItems GetAssetLotQuantity(Int64 ItemID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAssetLotQuantity(ItemID);

        }

        public string GetAttributeValue(Int64 AssetAttributeID, Int64 AssetID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAttributeValue(AssetAttributeID, AssetID);

        }

        public bool DeleteAssetsAllocation(Int64 _ID)
        {
            db.Repository<AT_AssetAllocation>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool SaveAssetsAllocation(AT_AssetAllocation _AssetAllocation)
        {
            bool isSaved = false;

            if (_AssetAllocation.ID == 0)
                db.Repository<AT_AssetAllocation>().Insert(_AssetAllocation);
            else
                db.Repository<AT_AssetAllocation>().Update(_AssetAllocation);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public string GetAssetItemType(Int64 AssetItemID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAssetItemType(AssetItemID);
        }

        public long? GetAssetAttributeID(Int64 AssetAttributeID, Int64 AssetItemID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetAssetAttributeID(AssetAttributeID, AssetItemID);
        }

        public bool IsAssetItemUnique(AT_AssetAllocation Assets)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().IsAssetItemUnique(Assets);
        }

        public bool IsAssetAllocationIDExists(long _AssetAllocationID)
        {
            bool qAssetAllocationID = db.Repository<AT_MonthlyExpenses>().GetAll().Any(d => d.AssetAllocationID == _AssetAllocationID);

            return qAssetAllocationID;
        }

        public bool IsAssetIDExists(long _AssetID)
        {
            bool qAssetItemID = db.Repository<AT_AssetAllocation>().GetAll().Any(d => d.AssetItemID == _AssetID);
            return qAssetItemID;
        }
        #endregion

        #region Accounts Head

        /// <summary>
        /// This function gets the Account Heads.
        /// Created On 06-04-2017
        /// </summary>
        /// <returns>List<AT_AccountsHead></returns>
        public List<AT_AccountsHead> GetAccountHeads(bool isActive = false)
        {
            if (!isActive)
            {
                return db.Repository<AT_AccountsHead>().GetAll().OrderBy(ah => ah.HeadName).ToList();
            }
            else
            {
                return db.Repository<AT_AccountsHead>().GetAll().Where(x => x.IsActive == true).OrderBy(ah => ah.HeadName).ToList();
            }

        }

        /// <summary>
        /// This function gets Account Head by name.
        /// Created On 06-04-2017
        /// </summary>
        /// <param name="_HeadName"></param>
        /// <returns>AT_AccountsHead</returns>
        public AT_AccountsHead GetAccountHeadByName(string _HeadName)
        {
            return db.Repository<AT_AccountsHead>().GetAll().Where(ah => ah.HeadName.Replace(" ", "").ToUpper() == _HeadName.Replace(" ", "").ToUpper()).FirstOrDefault();
        }

        /// <summary>
        /// This function gets the account head by ID.
        /// Created On 06-04-2017
        /// </summary>
        /// <param name="_AccountHeadID"></param>
        /// <returns>AT_AccountsHead</returns>
        public AT_AccountsHead GetAccountHeadByID(long _AccountHeadID)
        {
            return db.Repository<AT_AccountsHead>().FindById(_AccountHeadID);
        }

        /// <summary>
        /// This function adds Account Head.
        /// Created on 06-04-2017
        /// </summary>
        /// <param name="_MdlAccountsHead"></param>
        public void AddAccountHead(AT_AccountsHead _MdlAccountsHead)
        {
            db.Repository<AT_AccountsHead>().Insert(_MdlAccountsHead);

            db.Save();
        }

        /// <summary>
        /// This function updates Account Head.
        /// Created on 07-04-2017
        /// </summary>
        /// <param name="_MdlAccountHead"></param>
        public void UpdateAccountHead(AT_AccountsHead _MdlAccountHead)
        {
            db.Repository<AT_AccountsHead>().Update(_MdlAccountHead);

            db.Save();
        }

        /// <summary>
        /// This function checks if AccountHeadID reference exists.
        /// Created On 07-04-2017
        /// </summary>
        /// <param name="_AccountHeadID"></param>
        /// <returns>bool</returns>
        public bool IsAccountHeadIDExists(long _AccountHeadID)
        {
            bool IsExists = db.Repository<AT_ObjectClassification>().GetAll().Any(oc => oc.AccountHeadID == _AccountHeadID);

            if (!IsExists)
            {
                IsExists = db.Repository<AT_BudgetUtilization>().GetAll().Any(bu => bu.AccountHeadID == _AccountHeadID);
            }

            return IsExists;
        }

        /// <summary>
        /// This function deletes the Account Head based on the ID.
        /// Created On 07-04-2017
        /// </summary>
        /// <param name="_AccountHeadID"></param>
        public void DeleteAccountHead(long _AccountHeadID)
        {
            db.Repository<AT_AccountsHead>().Delete(_AccountHeadID);

            db.Save();
        }

        #endregion

        #region Budget Utilization
        public List<object> GetAccountHeadByID()
        {
            return db.Repository<AT_AccountsHead>().GetAll().ToList<object>();
        }

        public List<AT_GetBudgetUtilizationList_Result> GetBudgetUtilizationList(string _FinancialYear, string _Month, long _ObjectClassificationID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetBudgetUtilizationList(_FinancialYear, _Month, _ObjectClassificationID);
        }

        public bool SaveBudgetUtilization(AT_BudgetUtilization budgetUtilization)
        {
            bool isSaved = false;

            db.Repository<AT_BudgetUtilization>().Insert(budgetUtilization);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        #endregion

        #region AccountReports

        public List<object> GetACObjectClassification(Int64 _ACHeadID)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetACObjectClassification(_ACHeadID);
        }

        public List<object> GetObjectClassificationCode()
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetObjectClassificationCode();
        }

        public List<RPT_AT_SanctionOnDropdown_Result> GetSanctionNO(string Year, string Month, string SanctionOn)
        {
            return db.ExtRepositoryFor<ReferenceDataRepository>().GetSanctionNO(Year, Month, SanctionOn);

        }
        #endregion

        public AT_FundRelease GetFundRelease(long _FundReleaseID)
        {
            return db.Repository<AT_FundRelease>().GetAll().Where(t => t.FundReleaseTypeID == _FundReleaseID).FirstOrDefault();
        }

        public AT_FundReleaseDetails GetObjectClassification(long _ID)
        {
            return db.Repository<AT_FundReleaseDetails>().GetAll().Where(t => t.ObjectClassificationID == _ID).FirstOrDefault();
        }
    }
}
