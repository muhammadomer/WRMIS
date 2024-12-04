using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.Accounts
{
    public class ReferenceDataRepository : Repository<AT_TaxRate>
    {
        public ReferenceDataRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<AT_TaxRate>();

        }

        #region Tax Rate

        /// <summary>
        /// This function adds the default tax data
        /// Created on 30-03-2017
        /// </summary>
        /// <param name="_LstTaxRates"></param>
        public void AddTaxData(List<AT_TaxRate> _LstTaxRates)
        {
            context.AT_TaxRate.AddRange(_LstTaxRates);

            context.SaveChanges();
        }

        #endregion

        #region ResourceAllocation
        public List<object> GetADMUser()
        {
            List<object> admUser = (from u in context.UA_Users
                                    join des in context.UA_Designations on u.DesignationID equals des.ID
                                    where des.OrganizationID == (long)PMIU.WRMIS.Common.Constants.Organization.PMIU && des.ID == (long)PMIU.WRMIS.Common.Constants.Designation.ADM
                                    select new
                                    {
                                        u.ID,
                                        Name = u.FirstName + " " + u.LastName

                                    }).ToList<object>();
            return admUser;
        }
        public List<object> GetOfficeUsers(string StaffType)
        {
            List<object> officeUser = (from u in context.AT_ResourceAllocation
                                       where u.PMIUFieldOffice == StaffType
                                       select new
                                       {
                                           u.ID,
                                           Name = u.StaffUserName

                                       }).ToList<object>();
            return officeUser;
        }
        public List<object> GetADMDesignationID(int StaffType)
        {
            List<object> admDesignation = (from des in context.UA_Designations
                                           where des.OrganizationID == (long)Constants.Organization.PMIU &&
                                           (
                                           (StaffType == 1 && (des.IrrigationLevelID != null))  // 1 for Field Staff
                                           ||
                                           (StaffType == 0 && (des.IrrigationLevelID == null)) //0 for Office Staff
                                           )

                                           select new
                                           {
                                               des.ID,
                                               des.Name

                                           }).ToList<object>();
            return admDesignation;

        }
        public List<object> SearchMonthlyExpnses(Dictionary<string, object> _Data)
        {
            string _FinancialYear = "";
            string _Month = "";
            long _ExpenseMadeby = 0;
            string _PMIUStaff = "";


            _FinancialYear = Convert.ToString(_Data["_FinancialYear"]);
            _Month = Convert.ToString(_Data["_Month"]);
            _ExpenseMadeby = Convert.ToInt64(_Data["_ExpenseMadeby"]);
            _PMIUStaff = Convert.ToString(_Data["_PMIUStaff"]);


            List<object> lstObject = (from me in context.AT_MonthlyExpenses
                                      join ra in context.AT_ResourceAllocation on me.ResourceAllocationID equals ra.ID
                                      join user in context.UA_Users on ra.StaffUserID equals user.ID
                                      join desig in context.UA_Designations on ra.DesignationID equals desig.ID
                                      join ExpnsType in context.AT_ExpenseType on me.ExpenseTypeID equals ExpnsType.ID
                                      join aa in context.AT_AssetAllocation on ra.ID equals aa.ResourceAllocationID
                                      where
                                      ((me.Month == _Month) && (me.FinancialYear == _FinancialYear) && (me.AT_ResourceAllocation.StaffUserID == _ExpenseMadeby || _ExpenseMadeby == 0) && (_PMIUStaff.ToUpper() == me.PMIUFieldOffice.ToUpper()))
                                      select new
                                      {
                                          ID = me.ID,
                                          ResourceAllocationID = me.ResourceAllocationID,
                                          Name = user.FirstName + " " + user.LastName,
                                          Designation = desig.Name,
                                          VehicleNo = me.POLVehicleNo == null ? "-" : me.POLVehicleNo,
                                          TADA = me.TAAmount == null ? 0 : me.TAAmount + me.DAAmount == null ? 0 : me.DAAmount,
                                          RepairMaintainance = me.RepairAmount == null ? 0 : me.RepairAmount + me.PurchaseAmount == null ? 0 : me.PurchaseAmount,
                                          POL = me.POLAmount == null ? 0 : me.POLAmount,
                                          TotalClaim = (me.TAAmount == null ? 0 : me.TAAmount + me.DAAmount == null ? 0 : me.DAAmount) +
                                                       (me.RepairAmount == null ? 0 : me.RepairAmount + me.PurchaseAmount == null ? 0 : me.PurchaseAmount) +
                                                       (me.POLAmount == null ? 0 : me.POLAmount)
                                      }).ToList<object>();
            return lstObject;
        }
        public List<object> GetResourceAllocationSearch(Int64 _PmiuID)
        {
            List<object> Search = (from ra in context.AT_ResourceAllocation
                                   join d in context.UA_Designations on ra.DesignationID equals d.ID

                                   where (_PmiuID > 0 && ra.ADMUserID == _PmiuID && ra.PMIUFieldOffice.ToLower() == "f") || (_PmiuID == -1 && ra.PMIUFieldOffice.ToLower() == "o")
                                   select new
                                   {
                                       ra.ID,
                                       ra.PMIUFieldOffice,
                                       ra.ADMUserID,
                                       ra.StaffUserID,
                                       ra.StaffUserName,
                                       //ra.StaffUserName,
                                       ra.DesignationID,
                                       DesignationName = d.Name,
                                       ra.EmailAddress,
                                       ra.ContactNumber,
                                       ra.BPS,
                                       ra.IsActive,
                                       ra.CreatedBy,
                                       ra.CreatedDate

                                   }).ToList<object>();
            return Search;
        }
        public List<object> GetNameofStaffByDesignationID(Int64 _DesignationID)
        {
            List<object> StaffName = (from des in context.UA_Designations
                                      join u in context.UA_Users on des.ID equals u.DesignationID
                                      where des.OrganizationID == 1 && des.ID == _DesignationID
                                      select new
                                      {
                                          u.ID,
                                          Name = u.FirstName + " " + u.LastName
                                      }).ToList<object>();

            return StaffName;
        }
        public bool IsStaffUnique(AT_ResourceAllocation _ResourceAllocation)
        {
            bool result = false;
            if (_ResourceAllocation.ID == 0)
            {
                if (_ResourceAllocation.StaffUserID != null)
                {
                    result = (from ra in context.AT_ResourceAllocation
                              where (ra.StaffUserID == _ResourceAllocation.StaffUserID)
                              select ra).Any();
                }
                else
                {
                    result = (from ra in context.AT_ResourceAllocation
                              where (ra.StaffUserName == _ResourceAllocation.StaffUserName && ra.DesignationID == _ResourceAllocation.DesignationID && ra.ContactNumber == _ResourceAllocation.ContactNumber)
                              select ra).Any();
                }

            }
            else
            {
                if (_ResourceAllocation.StaffUserID != null)
                {
                    result = (from ra in context.AT_ResourceAllocation
                              where (ra.StaffUserID == _ResourceAllocation.StaffUserID) && ra.ID != _ResourceAllocation.ID
                              select ra).Any();
                }
                else
                {
                    result = (from ra in context.AT_ResourceAllocation
                              where (ra.StaffUserName == _ResourceAllocation.StaffUserName && ra.DesignationID == _ResourceAllocation.DesignationID && ra.ContactNumber == _ResourceAllocation.ContactNumber) && ra.ID != _ResourceAllocation.ID
                              select ra).Any();
                }
            }
            //if (_ResourceAllocation.ID == 0)
            //{
            //    result = (from ra in context.AT_ResourceAllocation
            //              where (_ResourceAllocation.DesignationID == 12 && ra.StaffUserName == _ResourceAllocation.StaffUserName)
            //              || (ra.StaffUserName == _ResourceAllocation.StaffUserName)
            //              select ra).Any();
            //}
            //else
            //{
            //    result = (from ra in context.AT_ResourceAllocation
            //              where (_ResourceAllocation.DesignationID == 12 && ra.StaffUserName == _ResourceAllocation.StaffUserName)
            //              || (ra.StaffUserName == _ResourceAllocation.StaffUserName)
            //              select ra).Any();
            //    if (result)
            //    {
            //        result = (from ra in context.AT_ResourceAllocation
            //                  where ra.EmailAddress == _ResourceAllocation.EmailAddress &&
            //                  ra.ContactNumber == _ResourceAllocation.ContactNumber &&
            //                  ra.BPS == _ResourceAllocation.BPS &&
            //                  ra.IsActive == _ResourceAllocation.IsActive
            //                  select ra).Any();

            //    }
            //}

            return result;
        }
        public object GetEmailContact(Int64 _UserID)
        {
            object emailContact = (from u in context.UA_Users
                                   where u.ID == _UserID
                                   select new
                                   {
                                       u.Email,
                                       u.MobilePhone
                                   }).FirstOrDefault();

            return emailContact;
        }
        public bool IsExistResourceAllocation(Int64 _ID)
        {
            bool IsExistResourceAllocation = false;

            bool AsAllocation = (from aa in context.AT_AssetAllocation
                                 where aa.ResourceAllocationID == _ID
                                 select aa).Any();

            bool AtMonthlyExpenses = (from am in context.AT_MonthlyExpenses
                                      where am.ResourceAllocationID == _ID
                                      select am).Any();
            if (AsAllocation || AtMonthlyExpenses)
            {
                IsExistResourceAllocation = true;
            }

            return IsExistResourceAllocation;
        }

        #endregion

        #region AssetsAllocation
        public object GetResourceAllocationData(Int64? _ID)
        {
            object Resource = (from ra in context.AT_ResourceAllocation
                               join d in context.UA_Designations on ra.DesignationID equals d.ID
                               where ra.ID == _ID
                               select new
                               {
                                   ResourceType = (ra.PMIUFieldOffice == "F" ? "PMIU Field Staff" : "PMIU Office"),
                                   Designation = d.Name,
                                   StaffName = ra.StaffUserName,
                                   ra.BPS
                               }).FirstOrDefault();
            return Resource;
        }

        public List<object> GetAACategory()
        {
            List<object> Category = (from c in context.AM_Category
                                     select new
                                     {
                                         c.ID,
                                         c.Name
                                     }).ToList<object>();
            return Category;
        }

        public List<object> GetAASubCategory(Int64 _CategoryID)
        {
            List<object> SubCategory = (from sc in context.AM_SubCategory
                                        where sc.CategoryID == _CategoryID
                                        select new
                                        {
                                            sc.ID,
                                            sc.Name
                                        }).ToList<object>();
            return SubCategory;
        }

        public List<object> GetAAAssetType()
        {
            List<object> AssetType = (from at in context.AT_AssetType
                                      select new
                                      {
                                          at.ID,
                                          Name = at.AssetType
                                      }).ToList<object>();
            return AssetType;
        }

        public List<object> GetAAAssetName(Int64 _SubCategory)
        {
            List<object> AssetItems = (from ai in context.AM_AssetItems
                                       where ai.SubCategoryID == _SubCategory
                                       && ai.IrrigationLevelID == (long)Constants.IrrigationLevelID.Office && (ai.IrrigationBoundryID == (from o in context.AM_Offices where o.OfficeName.ToUpper().Trim() == "PMIU OFFICE" select o.ID).FirstOrDefault())
                                       select new
                                       {
                                           ai.ID,
                                           Name = ai.AssetName
                                       }).ToList<object>();
            return AssetItems;
        }

        public List<object> GetAAAssetAttribute(Int64 _AssetID)
        {
            List<object> Attributes = (from att in context.AM_AssetAttributes
                                       where att.AssetItemID == _AssetID
                                       select new
                                       {
                                           att.ID,
                                           Name = att.AM_Attributes.AttributeName

                                       }).ToList<object>();
            return Attributes;
        }

        public List<object> GetAssetAllocation(Int64 _ResourceAllocationID)
        {

            List<object> lstAssets = (from aa in context.AT_AssetAllocation
                                      join aat in context.AT_AssetType on aa.AssetTypeID equals aat.ID
                                      join ai in context.AM_AssetItems on aa.AssetItemID equals ai.ID
                                      join asca in context.AM_SubCategory on ai.SubCategoryID equals asca.ID
                                      join ac in context.AM_Category on asca.CategoryID equals ac.ID

                                      join asat in context.AM_AssetAttributes on aa.AssetAttributeID.Value equals asat.ID into Group1
                                      from g1 in Group1.DefaultIfEmpty()

                                      join aatt in context.AM_Attributes on g1.AttributeID equals aatt.ID into Group2
                                      from g2 in Group2.DefaultIfEmpty()

                                      where aa.ResourceAllocationID == _ResourceAllocationID
                                      //&& g1.AssetItemID == ai.ID
                                      //&& g2.SubCategoryID == asca.ID
                                      select new
                                      {
                                          aa.ID,
                                          aa.ResourceAllocationID,
                                          aa.CreatedBy,
                                          aa.CreatedDate,
                                          AssetTypeID = aat.ID,
                                          AssetTypeName = aat.AssetType,
                                          AssetItemID = ai.ID,
                                          AssetItemName = ai.AssetName,
                                          AssetType = ai.AssetType,
                                          ai.LotQuantity,
                                          SubCategoryID = asca.ID,
                                          SubCategoryName = asca.Name,
                                          CategoryID = ac.ID,
                                          CategoryName = ac.Name,
                                          AssetAllocationValue = aa.AssetAllocationValue,
                                          AttributeValue = g1.AttributeValue,
                                          AssetAttributeID = (long?)g1.ID,
                                          AssetAttributeName = g2.AttributeName
                                      }).ToList().Select(c => new
                                      {
                                          c.ID,
                                          c.ResourceAllocationID,
                                          c.CreatedBy,
                                          c.CreatedDate,
                                          AssetTypeID = c.AssetTypeID,
                                          AssetTypeName = c.AssetTypeName,
                                          AssetItemID = c.AssetItemID,
                                          AssetItemName = c.AssetItemName,
                                          c.AssetType,
                                          c.LotQuantity,
                                          SubCategoryID = c.SubCategoryID,
                                          SubCategoryName = c.SubCategoryName,
                                          CategoryID = c.CategoryID,
                                          CategoryName = c.CategoryName,
                                          AssetAttributeValue = (c.AssetType == "Lot" ? (c.AssetAllocationValue.HasValue ? c.AssetAllocationValue.Value.ToString() + "" : null) : c.AttributeValue),
                                          AssetAttributeID = (c.AssetAttributeID == null ? 0 : c.AssetAttributeID),
                                          AssetAttributeName = (c.AssetType == "Lot" ? "Quantity" : c.AssetAttributeName)
                                      }).ToList<object>();
            return lstAssets;
        }

        public AM_AssetItems GetAssetLotQuantity(Int64 ItemID)
        {
            AM_AssetItems Item = (from ai in context.AM_AssetItems
                                  where ai.ID == ItemID
                                  select ai).FirstOrDefault();
            return Item;

        }

        public string GetAttributeValue(Int64 AssetAttributeID, Int64 AssetID)
        {

            string value = (from ai in context.AM_AssetItems
                            where ai.ID == AssetID
                            select ai.AssetType).FirstOrDefault();

            if (value.ToLower() == "lot")
            {
                value = (from ai in context.AM_AssetItems
                         where ai.ID == AssetID && ai.AssetType.ToLower() == "lot"
                         select ai.LotQuantity).FirstOrDefault().ToString();
            }
            else
            {
                value = (from aa in context.AM_AssetAttributes
                         where aa.ID == AssetAttributeID
                         select aa.AttributeValue).FirstOrDefault();
            }

            return value;
        }

        public string GetAssetItemType(Int64 AssetItemID)
        {
            string assetType = (from ai in context.AM_AssetItems
                                where ai.ID == AssetItemID
                                select ai.AssetType).FirstOrDefault();

            return assetType;
        }

        public long? GetAssetAttributeID(Int64 AssetAttributeID, Int64 AssetItemID)
        {
            AM_AssetAttributes mdlAssetAttribute = (from aa in context.AM_AssetAttributes
                                                    where aa.ID == AssetAttributeID
                                                    select aa).FirstOrDefault();

            if (mdlAssetAttribute != null)
                return mdlAssetAttribute.ID;
            else
                return null;
            //return AttributeID;
        }

        public bool IsAssetItemUnique(AT_AssetAllocation Assets)
        {
            bool result = false;
            if (Assets.ID == 0)
            {
                result = (from aa in context.AT_AssetAllocation
                          where (aa.AssetItemID == Assets.AssetItemID && aa.ResourceAllocationID == Assets.ResourceAllocationID)
                          select aa).Any();
            }
            else
            {
                result = (from aa in context.AT_AssetAllocation
                          where (aa.AssetItemID == Assets.AssetItemID && aa.ResourceAllocationID == Assets.ResourceAllocationID && Assets.ID != aa.ID)
                          select aa).Any();
                if (result)
                {
                    result = context.AT_AssetAllocation.Any(aa => aa.AssetAttributeID == Assets.AssetAttributeID && aa.AssetTypeID == Assets.AssetTypeID && aa.AssetAllocationValue == Assets.AssetAllocationValue);


                }
            }

            return result;
        }
        #endregion

        #region Budget Utilization

        public List<AT_GetBudgetUtilizationList_Result> GetBudgetUtilizationList(string _FinancialYear, string _Month, long _ObjectClassificationID)
        {
            return context.AT_GetBudgetUtilizationList(_FinancialYear, _Month, _ObjectClassificationID).ToList<AT_GetBudgetUtilizationList_Result>();
        }
        #endregion

        #region AccountReports

        public List<object> GetACObjectClassification(Int64 _ACHeadID)
        {

            List<object> ObjClassification = (from c in context.AT_ObjectClassification
                                              where c.AccountHeadID == _ACHeadID
                                              select new
                                              {
                                                  c.ID,
                                                  Name = c.ObjectClassification
                                              }).ToList<object>();
            return ObjClassification;
        }

        public List<object> GetObjectClassificationCode()
        {
            List<object> ObjectClassificationCode = (from oc in context.AT_ObjectClassification
                                                     select new
                                                     {
                                                         oc.ID,
                                                         Name = oc.AccountsCode + " - " + oc.ObjectClassification
                                                     }).ToList<object>();
            return ObjectClassificationCode;
        }

        public List<RPT_AT_SanctionOnDropdown_Result> GetSanctionNO(string Year, string Month, string SanctionOn)
        {

            //List<object> SanctionNO = (from s in context.AT_Sanction
            //                           where s.FinancialYear == Year && s.Month == Month 

            //                           select new
            //                           {
            //                               s.ID,
            //                               Name = s.SanctionNo
            //                           }).ToList<object>();
            //return SanctionNO;

            return context.RPT_AT_SanctionOnDropdown(Year, Month, Convert.ToInt32(SanctionOn)).ToList<RPT_AT_SanctionOnDropdown_Result>();
        }
        #endregion
    }
}
