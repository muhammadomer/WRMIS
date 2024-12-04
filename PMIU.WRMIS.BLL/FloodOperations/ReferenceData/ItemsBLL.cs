using PMIU.WRMIS.DAL.DataAccess.FloodOperations.ReferenceData;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.FloodOperations.ReferenceData
{
    public class ItemsBLL : BaseBLL
    {

        #region "Items"


        /// <summary>
        /// This method return List of Items by Item Category
        /// </summary>
        /// <param name="_ItemCategory"></param>
        /// <returns>List<object></returns>
        public List<object> GetItemsList(long _ItemCategory)
        {
            ItemsDAL dalItems = new ItemsDAL();
            return dalItems.GetItemsList(_ItemCategory);
        }
        //GetItemsCatg

        public List<FO_Items> GetItemsCatg(long catid)
        {
            ItemsDAL dalItems = new ItemsDAL();

            return dalItems.GetItemsCatg(catid);
        }
        public List<FO_Items> GetItemsCatgByID(long itemid)
        {
            ItemsDAL dalItems = new ItemsDAL();

            return dalItems.GetItemsCatgByID(itemid);
        }
        public List<object> GetAllItemsList()
        {
            ItemsDAL dalItems = new ItemsDAL();
            return dalItems.GetAllItemsList();
        }

        /// <summary>
        /// This function retun Items addition success along with message
        /// Created on 29-Sep-2016
        /// </summary>
        /// <param name="_Items"></param>
        /// <returns>bool</returns>
        public bool SaveItems(FO_Items _Items)
        {
            ItemsDAL dalItems = new ItemsDAL();
            return dalItems.SaveItems(_Items);
        }

        /// <summary>
        /// This function deletes a Items with the provided ID.
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteItems(long _ID)
        {
            ItemsDAL dalItems = new ItemsDAL();
            return dalItems.DeleteItems(_ID);
        }

        /// <summary>
        /// This function check Name existance in Item Category
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ItemCategory"></param>
        /// <returns>bool</returns>
        public bool IsItemsNameExists(FO_Items _Items)
        {
            ItemsDAL dalItems = new ItemsDAL();
            return dalItems.IsItemsNameExists(_Items);
        }

        /// <summary>
        /// This function checks in all related tables for given Items ID.
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ItemsID"></param>
        /// <returns>bool</returns>
        public bool IsItemsIDExists(long _ItemsID)
        {
            ItemsDAL dalItems = new ItemsDAL();

            return dalItems.IsItemsIDExists(_ItemsID);
        }


        #endregion "Items"

        #region "Item Category"

        /// <summary>
        /// This method return List of Item Category
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<object></returns>
        public List<object> GetItemCategoryList()
        {
            ItemsDAL dalItemCategory = new ItemsDAL();
            return dalItemCategory.GetItemCategoryList();
        }

        /// <summary>
        /// This function returns list of all Item Category with out Department Machinery/Equipment.
        /// Created On 29-08-2016.
        /// </summary>
        /// <returns>List<FO_ItemCategory>()</returns>
        public List<FO_ItemCategory> GetAllItemCategoryListWithOutAsset()
        {
            ItemsDAL dalItems = new ItemsDAL();

            return dalItems.GetAllItemCategoryListWithOutAsset();
        }
        /// <summary>
        /// This function returns list of all Item Category.
        /// Created On 29-08-2016.
        /// </summary>
        /// <returns>List<FO_ItemCategory>()</returns>
        public List<FO_ItemCategory> GetAllItemCategoryList()
        {
            ItemsDAL dalItems = new ItemsDAL();

            return dalItems.GetAllItemCategoryList();
        }
        public List<FO_ItemCategory> GetAllItemCategoryListByID(long _Catid)
        {
            ItemsDAL dalItems = new ItemsDAL();

            return dalItems.GetAllItemCategoryListByID(_Catid);
        }

        /// <summary>
        /// This function retun Item Category addition success along with message
        /// Created on 29-Sep-2016
        /// </summary>
        /// <param name="_ItemCategory"></param>
        /// <returns>bool</returns>
        public bool SaveItemCategory(FO_ItemCategory _ItemCategory)
        {
            ItemsDAL dalItemCategory = new ItemsDAL();
            return dalItemCategory.SaveItemCategory(_ItemCategory);
        }

        /// <summary>
        /// This function deletes a Item Category with the provided ID.
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteItemCategory(long _ID)
        {
            ItemsDAL dalItemCategory = new ItemsDAL();
            return dalItemCategory.DeleteItemCategory(_ID);
        }

        /// <summary>
        /// This function check Name existance in Item Category
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ItemCategory"></param>
        /// <returns>bool</returns>
        public bool IsItemCategoryNameExists(FO_ItemCategory _ItemCategory)
        {
            ItemsDAL dalItemCategory = new ItemsDAL();
            return dalItemCategory.IsItemCategoryNameExists(_ItemCategory);
        }

        /// <summary>
        /// This function checks in all related tables for given ItemCategory ID.
        /// Created On 29-09-2016.
        /// </summary>
        /// <param name="_ItemCategoryID"></param>
        /// <returns>bool</returns>
        public bool IsItemCategoryIDExists(long _ItemCategoryID)
        {
            ItemsDAL dalItems = new ItemsDAL();

            return dalItems.IsItemCategoryIDExists(_ItemCategoryID);
        }
        /// <summary>
        /// This function returns list of all Item Category.
        /// Created On 29-08-2016.
        /// </summary>
        /// <returns>List<object>()</returns>
        public List<object> GetAllActiveItemCategoryList()
        {
            ItemsDAL dalItems = new ItemsDAL();

            return dalItems.GetAllActiveItemCategoryList();
        }
        #endregion "Item Category"
    }
}
