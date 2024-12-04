using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.FloodOperations.ReferenceData
{
    public class ItemsDAL : BaseDAL
    {
        #region "Items"

        /// <summary>
        /// This method return List of Items by Item Category
        /// Created on 29-Sep-2016
        /// </summary>
        /// <param name="_ItemCategory"></param>
        /// <returns>List<object></returns>
        public List<object> GetItemsList(long _ItemCategory)
        {
            List<object> lstItems = null;
            lstItems = db.Repository<FO_Items>().GetAll().Where(d => d.ItemCategoryID == _ItemCategory && d.IsActive == true).ToList<FO_Items>().ToList<object>();
            return lstItems;
        }
        public List<FO_Items> GetItemsCatg(long _ItemCategory)
        {
            List<FO_Items> lstItems = null;
            lstItems = db.Repository<FO_Items>().GetAll().Where(d => d.ItemCategoryID == _ItemCategory && d.IsActive == true).ToList<FO_Items>().ToList<FO_Items>();
            return lstItems;
        }
        public List<FO_Items> GetItemsCatgByID(long _ItemID)
        {
            List<FO_Items> lstItems = null;
            lstItems = db.Repository<FO_Items>().GetAll().Where(d => d.ID == _ItemID && d.IsActive == true).ToList<FO_Items>().ToList<FO_Items>();
            return lstItems;
        }
        public List<object> GetAllItemsList()
        {
            List<object> lstItems = null;
            lstItems = db.Repository<FO_Items>().GetAll().Where(d => d.IsActive == true).Select(x => new { x.ID, x.Name, x.Description, x.ItemCategoryID }).ToList<object>();
            return lstItems;
        }

        /// <summary>
        /// This function retun Items addition success along with message
        /// Created on 29-Sep-2016
        /// </summary>
        /// <param name="_Items"></param>
        /// <returns>bool</returns>
        public bool SaveItems(FO_Items _Items)
        {
            bool isSaved = false;

            if (_Items.ID == 0)
            {
                db.Repository<FO_Items>().Insert(_Items);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_Items>().Update(_Items);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        /// <summary>
        /// This function deletes a Items with the provided ID.
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteItems(long _ID)
        {
            db.Repository<FO_Items>().Delete(_ID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function check Name existance in Items
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_Items"></param>
        /// <returns>bool</returns>
        public bool IsItemsNameExists(FO_Items _Items)
        {
            bool qIsItemsNameExists = db.Repository<FO_Items>().GetAll().Any(i => i.Name == _Items.Name
                             && (i.ID != _Items.ID || _Items.ID == 0));
            return qIsItemsNameExists;
        }

        /// <summary>
        /// This function checks in all related tables for given Items ID.
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ItemsID"></param>
        /// <returns>bool</returns>
        public bool IsItemsIDExists(long _ItemsID)
        {

            long itemsID = 0;
            bool qIsExists = false;

            //itemsID = db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && d.Name == "Protection Structure").Single().ID;

            //qIsExists =  db.Repository<FO_InfrastructureParent>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_InfrastructureBreachingSection>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            //}

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_StructureIrrigationBoundaries>().GetAll().Any(s => s.StructureTypeID == InfrastructureTypeID && s.StructureID == _InfrastructureID);
            //}

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_StructureAdminBoundaries>().GetAll().Any(s => s.StructureTypeID == InfrastructureTypeID && s.StructureID == _InfrastructureID);
            //}

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_InfrastructureRepresentative>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            //}

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_InfrastructureStoneStock>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            //}

            return qIsExists;
        }

        #endregion "Items"

        #region "Item Category"

        /// <summary>
        /// This method return List of Item Category
        /// Created on 29-Sep-2016
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<object></returns>
        public List<object> GetItemCategoryList()
        {
            List<object> lstItemCategory = null;
            lstItemCategory = db.Repository<FO_ItemCategory>().GetAll().ToList<FO_ItemCategory>().ToList<object>();
            return lstItemCategory;
        }

        /// <summary>
        /// This function returns list of all Item Category with out Department Machinery/Equipment.
        /// Created On 29-08-2016.
        /// </summary>
        /// <returns>List<FO_ItemCategory>()</returns>
        public List<FO_ItemCategory> GetAllItemCategoryListWithOutAsset()
        {
            List<FO_ItemCategory> lstItemCategory = null;
            lstItemCategory = db.Repository<FO_ItemCategory>().GetAll().Where(i=>i.ID != 4).ToList<FO_ItemCategory>();
            return lstItemCategory;
        }

        /// <summary>
        /// This function returns list of all Item Category.
        /// Created On 29-08-2016.
        /// </summary>
        /// <returns>List<FO_ItemCategory>()</returns>
        public List<FO_ItemCategory> GetAllItemCategoryList()
        {
            List<FO_ItemCategory> lstItemCategory = null;
            lstItemCategory = db.Repository<FO_ItemCategory>().GetAll().ToList<FO_ItemCategory>().ToList<FO_ItemCategory>();
            return lstItemCategory;
        }

        public List<FO_ItemCategory> GetAllItemCategoryListByID(long _CatID)
        {
            List<FO_ItemCategory> lstItemCategory = null;
            lstItemCategory = db.Repository<FO_ItemCategory>().GetAll().Where(d => d.ID == _CatID).ToList<FO_ItemCategory>();
            return lstItemCategory;
        }
        /// <summary>
        /// This function retun Item Category addition success along with message
        /// Created on 29-Sep-2016
        /// </summary>
        /// <param name="_ItemCategory"></param>
        /// <returns>bool</returns>
        public bool SaveItemCategory(FO_ItemCategory _ItemCategory)
        {
            bool isSaved = false;

            if (_ItemCategory.ID == 0)
            {
                db.Repository<FO_ItemCategory>().Insert(_ItemCategory);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_ItemCategory>().Update(_ItemCategory);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        /// <summary>
        /// This function deletes a Item Category with the provided ID.
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteItemCategory(long _ID)
        {
            db.Repository<FO_ItemCategory>().Delete(_ID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function check Name existance in Item Category
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ItemCategory"></param>
        /// <returns>bool</returns>
        public bool IsItemCategoryNameExists(FO_ItemCategory _ItemCategory)
        {
            bool qIsItemCategoryNameExists = db.Repository<FO_ItemCategory>().GetAll().Any(i => i.Name == _ItemCategory.Name
                             && (i.ID != _ItemCategory.ID || _ItemCategory.ID == 0));
            return qIsItemCategoryNameExists;
        }

        /// <summary>
        /// This function checks in all related tables for given ItemCategory ID.
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ItemCategoryID"></param>
        /// <returns>bool</returns>
        public bool IsItemCategoryIDExists(long _ItemCategoryID)
        {
            long itemCategoryID = 0;
            bool qIsExists = false;

            //itemCategoryID  = db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && d.Name == "Protection Structure").Single().ID;

            qIsExists = db.Repository<FO_Items>().GetAll().Any(s => s.ItemCategoryID == _ItemCategoryID);

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_InfrastructureBreachingSection>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            //}

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_StructureIrrigationBoundaries>().GetAll().Any(s => s.StructureTypeID == InfrastructureTypeID && s.StructureID == _InfrastructureID);
            //}

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_StructureAdminBoundaries>().GetAll().Any(s => s.StructureTypeID == InfrastructureTypeID && s.StructureID == _InfrastructureID);
            //}

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_InfrastructureRepresentative>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            //}

            //if (!qIsExists)
            //{
            //  qIsExists = db.Repository<FO_InfrastructureStoneStock>().GetAll().Any(s => s.ProtectionInfrastructureID == _InfrastructureID);
            //}

            return qIsExists;
        }
        public List<object> GetAllActiveItemCategoryList()
        {
            List<object> lstItemCategory = null;
            lstItemCategory = db.Repository<FO_ItemCategory>().GetAll().Where(d => d.IsActive == true && d.ID != 4).Select(x => new { x.ID, x.Name, x.Description, x.IsActive }).ToList<object>();

            return lstItemCategory;
        }

        #endregion "Item Category"
    }
}
