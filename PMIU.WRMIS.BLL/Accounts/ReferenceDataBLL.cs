using PMIU.WRMIS.DAL.DataAccess.Accounts;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.Accounts
{
    public class ReferenceDataBLL : BaseBLL
    {
        ReferenceDataDAL dalReferenceData = new ReferenceDataDAL();

        #region Tax Rate

        /// <summary>
        /// This function adds the default tax data
        /// Created on 30-03-2017
        /// </summary>
        /// <param name="_LstTaxRates"></param>
        public void AddTaxData(List<AT_TaxRate> _LstTaxRates)
        {
            dalReferenceData.AddTaxData(_LstTaxRates);
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
            return dalReferenceData.GetTaxData(_TransactionType, _VendorType);
        }

        /// <summary>
        /// This function finds tax data by ID
        /// Created On 31-03-2017
        /// </summary>
        /// <param name="_TaxDataID"></param>
        /// <returns>AT_TaxRate</returns>
        public AT_TaxRate GetTaxDataByID(long _TaxDataID)
        {
            return dalReferenceData.GetTaxDataByID(_TaxDataID);
        }

        /// <summary>
        /// This function updates the tax data
        /// Created On 31-03-2017
        /// </summary>
        /// <param name="_MdlTaxRate"></param>        
        public void UpdateTaxData(AT_TaxRate _MdlTaxRate)
        {
            dalReferenceData.UpdateTaxData(_MdlTaxRate);
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
            return dalReferenceData.GetDailyRates();
        }

        /// <summary>
        /// This function finds daily rate data by ID
        /// Created On 31-03-2017
        /// </summary>
        /// <param name="_DailyRateID"></param>
        /// <returns>AT_DailyRate</returns>
        public AT_DailyRate GetDailyRateDateByID(long _DailyRateID)
        {
            return dalReferenceData.GetDailyRateDateByID(_DailyRateID);
        }

        /// <summary>
        /// This function updates the daily rate data
        /// Created On 31-03-2017
        /// </summary>
        /// <param name="_MdlDailyRate"></param>        
        public void UpdateDailyRateData(AT_DailyRate _MdlDailyRate)
        {
            dalReferenceData.UpdateDailyRateData(_MdlDailyRate);
        }

        /// <summary>
        /// This function get Daily Rate based on BPS
        /// Created On 03-04-2017
        /// </summary>
        /// <param name="_BPS"></param>
        /// <returns>AT_DailyRate</returns>
        public AT_DailyRate GetBPSRate(int _BPS)
        {
            return dalReferenceData.GetBPSRate(_BPS);
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
            return dalReferenceData.GetKeyPartsByVehicleType(_AssetTypeID);
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
            return dalReferenceData.GetPartByVehicleTypeAndName(_AssetTypeID, _PartName);
        }

        /// <summary>
        /// This function gets the key parts by ID
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_KeyPartID"></param>
        /// <returns>AT_KeyParts</returns>
        public AT_KeyParts GetKeyPartsByID(long _KeyPartID)
        {
            return dalReferenceData.GetKeyPartsByID(_KeyPartID);
        }

        /// <summary>
        /// This function adds Key Part
        /// Created on 05-04-2017
        /// </summary>
        /// <param name="_MdlKeyParts"></param>
        public void AddKeyPart(AT_KeyParts _MdlKeyParts)
        {
            dalReferenceData.AddKeyPart(_MdlKeyParts);
        }

        /// <summary>
        /// This function update Key Part
        /// Created on 05-04-2017
        /// </summary>
        /// <param name="_MdlKeyParts"></param>
        public void UpdateKeyPart(AT_KeyParts _MdlKeyParts)
        {
            dalReferenceData.UpdateKeyPart(_MdlKeyParts);
        }

        /// <summary>
        /// This function checks if KeyPartID reference exists.
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_KeyPartID"></param>
        /// <returns>bool</returns>
        public bool IsKeyPartIDExists(long _KeyPartID)
        {
            return dalReferenceData.IsKeyPartIDExists(_KeyPartID);
        }

        /// <summary>
        /// This function deletes the KeyPart based on the ID
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_KeyPartID"></param>
        public void DeleteKeyPart(long _KeyPartID)
        {
            dalReferenceData.DeleteKeyPart(_KeyPartID);
        }

        /// <summary>
        /// This function gets the assets type.
        /// Created On 10-04-2017
        /// </summary>
        /// <returns>List<AT_AssetType></returns>
        public List<AT_AssetType> GetAssetType()
        {
            return dalReferenceData.GetAssetType();
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
            return dalReferenceData.GetAccountSetups();
        }

        /// <summary>
        /// This function Update  Account Setups Amount
        /// Created On 04-05-2017
        /// </summary>
        /// <param name="_ID,_Ammount,_ModifiedBY,_ModifiedDate"></param>
        /// <returns>List<object></returns>
        public bool UpdateAccountSetupsAmount(long _ID, Double _Amount, long _ModifiedBy, DateTime _ModifiedDate)
        {
            return dalReferenceData.UpdateAccountSetupsAmount(_ID, _Amount, _ModifiedBy, _ModifiedDate);
        }

        #endregion

        #region Fund Release Type

        public List<object> GetFundsReleaseType()
        {
            return dalReferenceData.GetFundsReleaseType();
        }

        public bool SaveUpdateFundsReleaseType(AT_FundReleaseTypes _FundsReleaseType)
        {
            return dalReferenceData.SaveFundsReleaseType(_FundsReleaseType);
        }

        public bool DeleteFundsReleaseType(AT_FundReleaseTypes _FundsReleaseType)
        {
            return dalReferenceData.DeleteFundsReleaseType(_FundsReleaseType);
        }

        public AT_FundReleaseTypes GetFundsReleaseTypeByName(AT_FundReleaseTypes _FundsReleaseType)
        {
            return dalReferenceData.GetFundsReleaseTypeByName(_FundsReleaseType);
        }

        #endregion

        #region Object Classification

        public List<object> GetObjectClassificationByAccoundHeadID(long _AccountHeadID = 0)
        {
            return dalReferenceData.GetObjectClassificationByAccoundHeadID(_AccountHeadID);
        }

        public bool DeleteObjectClassification(AT_ObjectClassification _ObjectClassification)
        {
            return dalReferenceData.DeleteObjectClassification(_ObjectClassification);
        }

        public bool SaveObjectClassification(AT_ObjectClassification _FundsReleaseType)
        {
            return dalReferenceData.SaveObjectClassification(_FundsReleaseType);
        }

        public AT_ObjectClassification GetObjectClassificationByAccountCode(AT_ObjectClassification _ObjectClassification)
        {
            return dalReferenceData.GetObjectClassificationByAccountCode(_ObjectClassification);
        }

        #endregion

        #region ResourceAllocation

        public List<object> GetADMUser()
        {
            return dalReferenceData.GetADMUser();
        }

        public List<object> GetOfficeUsers(string StaffType)
        {
            return dalReferenceData.GetOfficeUsers(StaffType);
        }

        public List<object> GetADMDesignationID(int StaffType)
        {
            return dalReferenceData.GetADMDesignationID(StaffType);
        }

        public List<object> GetResourceAllocationSearch(Int64 _PmiuID)
        {
            return dalReferenceData.GetResourceAllocationSearch(_PmiuID);
        }

        public bool DeleteResourceAllocation(Int64 _ID)
        {

            return dalReferenceData.DeleteResourceAllocation(_ID);
        }
        public bool IsExistResourceAllocation(Int64 _ID) 
        {

            return dalReferenceData.IsExistResourceAllocation(_ID);
        }
        
        public bool SaveResourceAllocation(AT_ResourceAllocation _ResourceAllocation)
        {
            return dalReferenceData.SaveResourceAllocation(_ResourceAllocation);
        }

        public bool IsUserExits(Int64 _DesignationID)
        {
            return dalReferenceData.IsUSerExits(_DesignationID);
        }

        public List<object> GetNameofStaffByDesignationID(Int64 _DesignationID)
        {
            return dalReferenceData.GetNameofStaffByDesignationID(_DesignationID);
        }

        public bool IsStaffUnique(AT_ResourceAllocation _ResourceAllocation)
        {
            return dalReferenceData.IsStaffUnique(_ResourceAllocation);
        }

        public object GetEmailContact(Int64 _UserID)
        {
            return dalReferenceData.GetEmailContact(_UserID);
        }

        public bool IsUserExist(long _DesignationID)
        {
            return dalReferenceData.IsUserExist(_DesignationID);
        }
        #endregion

        #region AssetsAllocation
        public object GetResourceAllocationData(Int64? _ID)
        {
            return dalReferenceData.GetResourceAllocationData(_ID);
        }

        public List<object> GetAACategory()
        {
            return dalReferenceData.GetAACategory();
        }

        public List<object> GetAASubCategory(Int64 _CategoryID)
        {
            return dalReferenceData.GetAASubCategory(_CategoryID);
        }

        public List<object> GetAAAssetType()
        {
            return dalReferenceData.GetAAAssetType();
        }

        public List<object> GetAAAssetName(Int64 _SubCategory)
        {
            return dalReferenceData.GetAAAssetName(_SubCategory);
        }

        public List<object> GetAAAssetAttribute(Int64 _AssetID)
        {
            return dalReferenceData.GetAAAssetAttribute(_AssetID);
        }

        public List<object> GetAssetAllocation(Int64 _ResourceAllocationID)
        {
            return dalReferenceData.GetAssetAllocation(_ResourceAllocationID);
        }

        public AM_AssetItems GetAssetLotQuantity(Int64 ItemID)
        {
            return dalReferenceData.GetAssetLotQuantity(ItemID);

        }

        public string GetAttributeValue(Int64 AssetAttributeID, Int64 AssetID)
        {
            return dalReferenceData.GetAttributeValue(AssetAttributeID, AssetID);

        }

        public bool DeleteAssetsAllocation(Int64 _ID)
        {

            return dalReferenceData.DeleteAssetsAllocation(_ID);
        }

        public bool SaveAssetsAllocation(AT_AssetAllocation _AssetAllocation)
        {
            return dalReferenceData.SaveAssetsAllocation(_AssetAllocation);
        }

        public string GetAssetItemType(Int64 AssetItemID)
        {
            return dalReferenceData.GetAssetItemType(AssetItemID);
        }

        public long? GetAssetAttributeID(Int64 AssetAttributeID, Int64 AssetItemID)
        {
            return dalReferenceData.GetAssetAttributeID(AssetAttributeID, AssetItemID);
        }

        public bool IsAssetItemUnique(AT_AssetAllocation Assets)
        {
            return dalReferenceData.IsAssetItemUnique(Assets);
        }

        public bool IsAssetAllocationIDExists(long _AssetAllocationID)
        {
            return dalReferenceData.IsAssetAllocationIDExists(_AssetAllocationID);
        }

        public bool IsAssetIDExists(long _AssetID)
        {
            return dalReferenceData.IsAssetIDExists(_AssetID);
        }
        #endregion

        #region Account Head

        /// <summary>
        /// This function gets the Account Heads.
        /// Created On 06-04-2017
        /// </summary>
        /// <returns>List<AT_AccountsHead></returns>
        public List<AT_AccountsHead> GetAccountHeads(bool isActive = false)
        {
            return dalReferenceData.GetAccountHeads(isActive);
        }

        /// <summary>
        /// This function gets Account Head by name.
        /// Created On 06-04-2017
        /// </summary>
        /// <param name="_HeadName"></param>
        /// <returns>AT_AccountsHead</returns>
        public AT_AccountsHead GetAccountHeadByName(string _HeadName)
        {
            return dalReferenceData.GetAccountHeadByName(_HeadName);
        }

        /// <summary>
        /// This function gets the account head by ID
        /// Created On 06-04-2017
        /// </summary>
        /// <param name="_AccountHeadID"></param>
        /// <returns>AT_AccountsHead</returns>
        public AT_AccountsHead GetAccountHeadByID(long _AccountHeadID)
        {
            return dalReferenceData.GetAccountHeadByID(_AccountHeadID);
        }

        /// <summary>
        /// This function adds Account Head.
        /// Created on 06-04-2017
        /// </summary>
        /// <param name="_MdlAccountsHead"></param>
        public void AddAccountHead(AT_AccountsHead _MdlAccountsHead)
        {
            dalReferenceData.AddAccountHead(_MdlAccountsHead);
        }

        /// <summary>
        /// This function updates Account Head
        /// Created on 07-04-2017
        /// </summary>
        /// <param name="_MdlAccountHead"></param>
        public void UpdateAccountHead(AT_AccountsHead _MdlAccountHead)
        {
            dalReferenceData.UpdateAccountHead(_MdlAccountHead);
        }

        /// <summary>
        /// This function checks if AccountHeadID reference exists.
        /// Created On 07-04-2017
        /// </summary>
        /// <param name="_AccountHeadID"></param>
        /// <returns>bool</returns>
        public bool IsAccountHeadIDExists(long _AccountHeadID)
        {

            return dalReferenceData.IsAccountHeadIDExists(_AccountHeadID);
        }

        /// <summary>
        /// This function deletes the Account Head based on the ID.
        /// Created On 07-04-2017
        /// </summary>
        /// <param name="_AccountHeadID"></param>
        public void DeleteAccountHead(long _AccountHeadID)
        {
            dalReferenceData.DeleteAccountHead(_AccountHeadID);
        }

        #endregion

        #region Budget Utilization

        public List<object> GetAccountHeadByID()
        {
            return dalReferenceData.GetAccountHeadByID();
        }

        public List<AT_GetBudgetUtilizationList_Result> GetBudgetUtilizationList(string _FinancialYear, string _Month, long _ObjectClassificationID)
        {
            return dalReferenceData.GetBudgetUtilizationList(_FinancialYear, _Month, _ObjectClassificationID);
        }

        public bool SaveBudgetUtilization(AT_BudgetUtilization budgetUtilization)
        {
            return dalReferenceData.SaveBudgetUtilization(budgetUtilization);
        }

        #endregion

        #region AccountReports

        public List<object> GetACObjectClassification(Int64 _ACHeadID)
        {
            return dalReferenceData.GetACObjectClassification(_ACHeadID);
        }
        public List<object> GetObjectClassificationCode()
        {
            return dalReferenceData.GetObjectClassificationCode();
        }

        public List<RPT_AT_SanctionOnDropdown_Result> GetSanctionNO(string Year, string Month, string SanctionOn)
        {
            return dalReferenceData.GetSanctionNO(Year, Month, SanctionOn);
        }
        #endregion

        public AT_FundRelease GetFundRelease(long _FundReleaseID)
        {
            return dalReferenceData.GetFundRelease(_FundReleaseID);
        }

        public AT_FundReleaseDetails GetObjectClassification(long _ID)
        {
            return dalReferenceData.GetObjectClassification(_ID);
        }
    }
}
