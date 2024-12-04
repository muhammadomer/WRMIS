using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.DataAccess.ClosureOperations;
using System.Data.Entity;
using PMIU.WRMIS.DAL.Repositories.AssetsAndWorks;

namespace PMIU.WRMIS.DAL.DataAccess.AssetsAndWorks
{
    public class AssetsWorkDAL
    {
        ContextDB db = new ContextDB();

        #region Notifications and Alerts

        //public CW_GetCWPPublishNotifyData_Result GetClosureWorkPlanPuslishNotifyData(long _CWPID)
        //{
        //    return db.ExtRepositoryFor<ClosureOperationsRepository>().GetClosureWorkPlanPuslishNotifyData(_CWPID);
        //}
        public AM_GetAWAssetAssociationNotifyData_Result GetAW_Association_NotifyData(long _ProgressID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetAW_Association_NotifyData(_ProgressID);
        }
        public AM_GetAWProgressNotifyData_Result GetAW_Progress_NotifyData(long _ProgressID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetAW_Progress_NotifyData(_ProgressID);
        }
        public AM_GetAWPublishNotifyData_Result1 GetAW_Publish_NotifyData(long _AWID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetAW_Publish_NotifyData(_AWID);
        }

        #endregion

        #region "Refrence Data"

        public List<object> GetAssetCategoryList()
        {
            return
                db.Repository<AM_Category>()
                    .GetAll()
                    .Where(s => s.IsActive == true)
                    .Select(c => new { c.ID, c.Name, c.Description, c.IsActive, c.CreatedBy, c.CreatedDate })
                    .ToList<object>();
        }

        public List<object> GetAssetAllCategoryList()
        {
            return
                db.Repository<AM_Category>()
                    .GetAll()
                    .Select(c => new { c.ID, c.Name, c.Description, c.IsActive, c.CreatedBy, c.CreatedDate })
                    .ToList<object>();
        }

        public bool IsAssetCategoryNameExists(AM_Category _AssetCategory)
        {
            bool isAssetCategoryExists = db.Repository<AM_Category>().GetAll().Any(i => i.Name == _AssetCategory.Name
                                                                                        &&
                                                                                        (i.ID != _AssetCategory.ID ||
                                                                                         _AssetCategory.ID == 0));
            return isAssetCategoryExists;
        }

        public bool SaveAssetCategory(AM_Category _AssetCategory)
        {
            bool isSaved = false;

            if (_AssetCategory.ID == 0)
            {
                db.Repository<AM_Category>().Insert(_AssetCategory);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<AM_Category>().Update(_AssetCategory);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool DeleteAssetCategory(long _ID)
        {
            db.Repository<AM_Category>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsAssetCategoryIDExists(long _AssetCategoryID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<AM_SubCategory>().GetAll().Any(s => s.CategoryID == _AssetCategoryID);
            return qIsExists;
        }

        public bool SubCategoryIDExistsAsset(long _SubCategoryID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<AM_AssetItems>().GetAll().Any(s => s.SubCategoryID == _SubCategoryID);
            return qIsExists;
        }

        public List<object> GetAssetSubCategoryList(long _CategoryID)
        {

            //  return db.Repository<AM_SubCategory>().GetAll().Where(s => s.IsActive == true && s.CategoryID == _CategoryID ).Select(c => new { c.ID, c.Name, c.Description, c.IsActive }).ToList<object>();

            String[] ids =
            {
                Constants.AssetAttribute.Channel, Constants.AssetAttribute.Outlet,
                Constants.AssetAttribute.BarrageHeadwork
                , Constants.AssetAttribute.ProtectionInfrastructure, Constants.AssetAttribute.Drain,
                Constants.AssetAttribute.SmallDams,
                Constants.AssetAttribute.SmallDamsChannels
            };
            return
                db.Repository<AM_SubCategory>()
                    .GetAll()
                    .Where(s => s.IsActive == true && s.CategoryID == _CategoryID && !ids.Contains(s.Name))
                    .Select(c => new { c.ID, c.Name, c.Description, c.IsActive })
                    .ToList<object>();
        }

        public List<object> GetAssetAttributeList(long _SubCatID)
        {
            return
                db.Repository<AM_Attributes>()
                    .GetAll()
                    .Where(s => s.IsActive == true && s.SubCategoryID == _SubCatID)
                    .Select(
                        c =>
                            new
                            {
                                c.ID,
                                c.SubCategoryID,
                                c.AttributeDataType,
                                c.AttributeName,
                                c.Description,
                                c.IsActive,
                                c.CreatedBy,
                                c.CreatedDate
                            })
                    .ToList<object>();
        }

        public List<object> GetAssetAllAttributeList(long _SubCatID)
        {
            return
                db.Repository<AM_Attributes>()
                    .GetAll()
                    .Where(s => s.SubCategoryID == _SubCatID)
                    .Select(
                        c =>
                            new
                            {
                                c.ID,
                                c.SubCategoryID,
                                c.AttributeDataType,
                                c.AttributeName,
                                c.Description,
                                c.IsActive,
                                c.CreatedBy,
                                c.CreatedDate
                            })
                    .ToList<object>();
        }

        public bool IsAssetAttributeNameExists(AM_Attributes _AssetAttributes)
        {
            bool isAssetAttributeExists =
                db.Repository<AM_Attributes>()
                    .GetAll()
                    .Any(
                        i =>
                            i.AttributeName == _AssetAttributes.AttributeName &&
                            i.SubCategoryID == _AssetAttributes.SubCategoryID
                            && (i.ID != _AssetAttributes.ID || _AssetAttributes.ID == 0));
            return isAssetAttributeExists;
        }

        public bool SaveAssetAttribute(AM_Attributes _AssetAttributes)
        {
            bool isSaved = false;

            if (_AssetAttributes.ID == 0)
            {
                db.Repository<AM_Attributes>().Insert(_AssetAttributes);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<AM_Attributes>().Update(_AssetAttributes);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool DeleteAssetAttribute(long _ID)
        {
            db.Repository<AM_Attributes>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsAssetAttributeIDExists(long _AssetAttributeID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<AM_AssetAttributes>().GetAll().Any(s => s.AttributeID == _AssetAttributeID);
            return qIsExists;
        }

        public object GetCategorySubCategoryByID(long _SubCatID)
        {
            object lstCat = (from subCat in db.Repository<AM_SubCategory>().GetAll()
                             where subCat.ID == _SubCatID
                             select new
                             {
                                 SubCatName = subCat.Name,
                                 CatName = subCat.AM_Category.Name

                             }).Distinct().FirstOrDefault();
            return lstCat;

        }

        public List<object> GetAssetOfficeList()
        {
            return
                db.Repository<AM_Offices>()
                    .GetAll()
                    .Where(s => s.IsActive == true)
                    .Select(c => new { c.ID, c.OfficeName, c.Description, c.IsActive, c.CreatedBy, c.CreatedDate })
                    .ToList<object>();
        }

        public List<object> GetAssetAllOfficeList()
        {
            return
                db.Repository<AM_Offices>()
                    .GetAll()
                    .Select(c => new { c.ID, c.OfficeName, c.Description, c.IsActive, c.CreatedBy, c.CreatedDate })
                    .ToList<object>();
        }

        public List<object> GetAssetConditionList()
        {
            return
                db.Repository<AM_AssetCondition>()
                    .GetAll()
                    .Where(s => s.IsActive == true)
                    .Select(c => new { c.ID, c.Condition })
                    .ToList<object>();
        }

        public bool SaveAssetOffice(AM_Offices _AssetOffice)
        {
            bool isSaved = false;

            if (_AssetOffice.ID == 0)
            {
                db.Repository<AM_Offices>().Insert(_AssetOffice);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<AM_Offices>().Update(_AssetOffice);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool IsAssetOfficeNameExists(AM_Offices _AssetOffice)
        {
            bool isAssetCategoryExists =
                db.Repository<AM_Offices>().GetAll().Any(i => i.OfficeName == _AssetOffice.OfficeName
                                                              && (i.ID != _AssetOffice.ID || _AssetOffice.ID == 0));
            return isAssetCategoryExists;
        }

        public bool IsAssetOfficeIDExists(long _AssetOfficeID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<AM_AssetItems>().GetAll().Any(s => s.IrrigationBoundryID == _AssetOfficeID);
            return qIsExists;
        }

        public bool DeleteAssetOffice(long _ID)
        {
            db.Repository<AM_Offices>().Delete(_ID);
            db.Save();

            return true;
        }

        public List<AM_AssetWorkType> GetAllAssetWorkType()
        {
            return db.Repository<AM_AssetWorkType>().Query().Get().ToList();
        }

        public bool AssetWorkTypeAssiciatExists(long _ID)
        {
            List<AM_AssetWork> lstWork =
                db.Repository<AM_AssetWork>().Query().Get().Where(x => x.AssetWorkTypeID == _ID).ToList();

            if (lstWork != null && lstWork.Count() > 0)
                return true;

            return false;
        }

        public bool AddAssetWrkType(AM_AssetWorkType _Mdl)
        {
            //  long maxID = db.Repository<AM_AssetWorkType>().Query().Get().ToList().Max(x => x.ID);
            _Mdl.ID = (0);
            _Mdl.CreatedDate = DateTime.Now;
            //   _Mdl.IsOther = true;
            db.Repository<AM_AssetWorkType>().Insert(_Mdl);
            db.Save();

            return true;
        }

        public AM_AssetWorkType GetAssetWrkType_ByName(string _Name)
        {
            return db.Repository<AM_AssetWorkType>().GetAll()
                .Where(z => z.IsActive == true && z.Name.Trim().ToLower() == _Name.Trim().ToLower())
                .FirstOrDefault();
        }

        public bool UpdateAssetWrkType(AM_AssetWorkType _Work)
        {
            AM_AssetWorkType mdl = db.Repository<AM_AssetWorkType>().FindById(_Work.ID);
            mdl.Name = _Work.Name;
            mdl.Description = _Work.Description;
            mdl.ModifiedBy = _Work.ModifiedBy;
            mdl.ModifiedDate = DateTime.Now;
            mdl.IsActive = _Work.IsActive;
            db.Repository<AM_AssetWorkType>().Update(mdl);
            db.Save();

            return true;
        }

        public bool DeleteAssetWrkType(long _ID)
        {
            db.Repository<AM_AssetWorkType>().Delete(_ID);
            db.Save();

            return true;
        }

        #region "Asset Sub Category"

        public bool SaveAssetSubCategory(AM_SubCategory _AssetSubCategory)
        {
            bool isSaved = false;

            if (_AssetSubCategory.ID == 0)
            {
                db.Repository<AM_SubCategory>().Insert(_AssetSubCategory);
                db.Save();
                isSaved = true;
            }
            else
            {
                AM_SubCategory sc =
                (from item in db.Repository<AM_SubCategory>().GetAll()
                 where item.ID == _AssetSubCategory.ID
                 select item).FirstOrDefault();
                sc.Name = _AssetSubCategory.Name;
                sc.Description = _AssetSubCategory.Description;
                sc.FOBit = _AssetSubCategory.FOBit;
                sc.IsActive = _AssetSubCategory.IsActive;
                db.Repository<AM_SubCategory>().Update(sc);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool IsAssetSubCategoryNameExists(AM_SubCategory _AssetSubCategory)
        {
            bool isAssetCategoryExists =
                db.Repository<AM_SubCategory>().GetAll().Any(i => i.Name == _AssetSubCategory.Name
                                                                  &&  i.CategoryID==_AssetSubCategory.CategoryID &&
                                                                  (i.ID != _AssetSubCategory.ID ||
                                                                   _AssetSubCategory.ID == 0));
            return isAssetCategoryExists;
        }

        public bool DeleteAssetSubCategory(long _ID)
        {
            db.Repository<AM_SubCategory>().Delete(_ID);
            db.Save();
            return true;
        }

        public List<object> GetSubCategoriesByCategoryID(int CategoryID)
        {

            String[] ids =
            {
                Constants.AssetAttribute.Channel, Constants.AssetAttribute.Outlet,
                Constants.AssetAttribute.BarrageHeadwork
                , Constants.AssetAttribute.ProtectionInfrastructure, Constants.AssetAttribute.Drain,
                Constants.AssetAttribute.SmallDams,
                Constants.AssetAttribute.SmallDamsChannels
            };
            return
                db.Repository<AM_SubCategory>()
                    .GetAll()
                    .Where(i => i.IsActive == true && i.CategoryID == CategoryID && !ids.Contains(i.Name))
                    .Select(c => new { ID = c.ID, Name = c.Name, Description = c.Description, IsActive = c.IsActive })
                    .ToList<object>();

        }

        public List<object> GetAllSubCategoriesByCategoryID(int CategoryID)
        {
            return
                db.Repository<AM_SubCategory>()
                    .GetAll()
                    .Where(i => i.CategoryID == CategoryID)
                    .Select(
                        c =>
                            new
                            {
                                ID = c.ID,
                                Name = c.Name,
                                Description = c.Description,
                                FOBit = c.FOBit,
                                IsActive = c.IsActive
                            })
                    .ToList<object>();

        }

        #endregion

        #region "HeadquarterDivision"


        public List<object> GetDivisionzByZoneId(long ZoneID)
        {
            List<object> lstObj = new List<object>();
            List<CO_Circle> crcl =
                db.Repository<CO_Circle>().GetAll().Where(x => x.ZoneID == ZoneID).ToList<CO_Circle>();

            lstObj = (from circlez in crcl
                      join dvn in db.Repository<CO_Division>().GetAll() on circlez.ID equals dvn.CircleID
                      select new { ID = dvn.ID, Name = dvn.Name }).ToList<object>();



            //lstObj = (from dvzn in db.Repository<CO_Division>().GetAll()
            //          join circle in db.Repository<CO_Circle>().GetAll() on zone.ID equals circle.ZoneID
            //          join zone in db.Repository<CO_Zone>().GetAll() on circle.CO_Zone.ID equals dvzn.ID
            //          where zone.ID == ZoneID
            //          select new
            //          {
            //              ID = dvzn.ID,
            //              Name = dvzn.Name,
            //          }
            //  ).ToList<object>();
            return lstObj;
        }

        public object GetHeadQDivisionByID(long _ZoneID)
        {
            object lstheadQ = (from HQ in db.Repository<AM_HeadquarterDivision>().GetAll()
                               where HQ.ZoneID == _ZoneID
                               select new
                               {
                                   HQID = HQ.ID,
                                   DivisionID = HQ.DivisionID,
                                   CreatedBy = HQ.CreatedBy,
                                   CreatedDate = HQ.CreatedDate

                               }).Distinct().FirstOrDefault();
            return lstheadQ;

        }

        public bool SaveHQDivision(AM_HeadquarterDivision _HeadquarterDivision)
        {
            bool isSaved = false;

            if (_HeadquarterDivision.ID == 0)
            {
                db.Repository<AM_HeadquarterDivision>().Insert(_HeadquarterDivision);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<AM_HeadquarterDivision>().Update(_HeadquarterDivision);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool DeleteHQDivision(long _HQDivisionID)
        {
            db.Repository<AM_HeadquarterDivision>().Delete(_HQDivisionID);
            db.Save();
            return true;
        }

        #endregion

        #endregion "Refrence Data"

        #region "Assets"

        public IEnumerable<DataRow> GetAssetsSearch(string _Level, long? _AssetsID, long? _IrrigationLevelID,
            long? _IrrigationBoundryID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _CategoryID,
            long? _SubCategoryID, bool? _FloodAssociationID, string _AssetType, string _Status, string _AssetName)
        {
            return db.ExecuteDataSet("Proc_AM_SearchAssets", _Level, _AssetsID, _IrrigationLevelID, _IrrigationBoundryID,
                _ZoneID, _CircleID, _DivisionID, _CategoryID, _SubCategoryID, _FloodAssociationID, _AssetType, _Status,
                _AssetName);
        }

        public bool IsHeadQuarterDivision(long? _IrrigationBoundryID)
        {
            return db.Repository<AM_HeadquarterDivision>().GetAll().Any(x => x.DivisionID == _IrrigationBoundryID);
        }

        public List<UA_IrrigationLevel> GetIrrigationLevelByDesignation(long DesignationID, bool isHeadQuarter)
        {
            List<UA_IrrigationLevel> lstIrrigationLevel = null;
            if (isHeadQuarter == true)
            {
                Int64[] ids = { 1, 2, 3 };
                lstIrrigationLevel =
                    db.Repository<UA_IrrigationLevel>()
                        .GetAll()
                        .Where(x => ids.Contains(x.ID))
                        .ToList<UA_IrrigationLevel>();
            }
            else
            {
                if (DesignationID == Convert.ToInt64(Constants.Designation.XEN))
                {
                    Int64[] ids = { 3 };
                    lstIrrigationLevel =
                        db.Repository<UA_IrrigationLevel>()
                            .GetAll()
                            .Where(x => ids.Contains(x.ID))
                            .ToList<UA_IrrigationLevel>();
                }
                else if (DesignationID == Convert.ToInt64(Constants.Designation.SE))
                {
                    Int64[] ids = { 2, 3 };
                    lstIrrigationLevel =
                        db.Repository<UA_IrrigationLevel>()
                            .GetAll()
                            .Where(x => ids.Contains(x.ID))
                            .ToList<UA_IrrigationLevel>();
                }
                else if (DesignationID == Convert.ToInt64(Constants.Designation.ChiefIrrigation))
                {
                    Int64[] ids = { 1, 2, 3 };
                    lstIrrigationLevel =
                        db.Repository<UA_IrrigationLevel>()
                            .GetAll()
                            .Where(x => ids.Contains(x.ID))
                            .ToList<UA_IrrigationLevel>();
                }
                //else if (DesignationID == Convert.ToInt64(Constants.Designation.DirectorGauges)
                //    || DesignationID == Convert.ToInt64(Constants.Designation.DeputyDirector)
                //    || DesignationID == Convert.ToInt64(Constants.Designation.ADM)
                //    || DesignationID == Convert.ToInt64(Constants.Designation.MA)
                //    || DesignationID == Convert.ToInt64(Constants.Designation.DeputyDirectorHelpline)
                //    || DesignationID == Convert.ToInt64(Constants.Designation.HelplineOperator)
                //    || DesignationID == Convert.ToInt64(Constants.Designation.AdditionalSecretaryAdmin)
                //    || DesignationID == Convert.ToInt64(Constants.Designation.DataEntryOperator)
                //    || DesignationID == Convert.ToInt64(Constants.Designation.AccountOfficer))
                //{
                //    Int64[] ids = { 6 };
                //    lstIrrigationLevel = db.Repository<UA_IrrigationLevel>().GetAll().Where(x => ids.Contains(x.ID)).ToList<UA_IrrigationLevel>();
                //}
                else if (DesignationID == Convert.ToInt64(Constants.Designation.ChiefMonitoring)
                         || DesignationID == Convert.ToInt64(Constants.Designation.DataAnalyst)
                         || DesignationID == Convert.ToInt64(Constants.Designation.Chiefpmo))
                {
                    Int64[] ids = { 1, 2, 3, 6 };
                    lstIrrigationLevel =
                        db.Repository<UA_IrrigationLevel>()
                            .GetAll()
                            .Where(x => ids.Contains(x.ID))
                            .ToList<UA_IrrigationLevel>();
                }
                else
                {
                    Int64[] ids = { 6 };
                    lstIrrigationLevel =
                        db.Repository<UA_IrrigationLevel>()
                            .GetAll()
                            .Where(x => ids.Contains(x.ID))
                            .ToList<UA_IrrigationLevel>();
                }
            }

            return lstIrrigationLevel;
        }

        public List<UA_IrrigationLevel> GetViewIrrigationLevelByDesignation(long DesignationID)
        {
            List<UA_IrrigationLevel> lstIrrigationLevel = null;
            Int64[] ids = { 1, 2, 3, 6 };
            lstIrrigationLevel =
                db.Repository<UA_IrrigationLevel>().GetAll().Where(x => ids.Contains(x.ID)).ToList<UA_IrrigationLevel>();
            return lstIrrigationLevel;
        }

        public List<object> GetIrrigationLevel()
        {

            List<object> lstIrrigationLevel = null;
            lstIrrigationLevel =
                db.Repository<UA_IrrigationLevel>()
                    .GetAll()
                    .Where(
                        x =>
                            x.Name.ToUpper().Equals("ZONE") || x.Name.ToUpper().Equals("CIRCLE") ||
                            x.Name.ToUpper().Equals("DIVISION"))
                    .Select(y => new { ID = y.ID, Name = y.Name })
                    .ToList<object>();
            return lstIrrigationLevel;
        }

        public List<CO_Zone> GetAllZone()
        {
            List<CO_Zone> lstZone = db.Repository<CO_Zone>().GetAll().OrderBy(z => z.Name).ToList<CO_Zone>();

            return lstZone;
        }

        public List<CO_Circle> GetAllCircle()
        {
            List<CO_Circle> lstCircle = db.Repository<CO_Circle>().GetAll().OrderBy(z => z.Name).ToList<CO_Circle>();

            return lstCircle;
        }

        public List<CO_Division> GetAllDivision()
        {
            List<CO_Division> lstDivision =
                db.Repository<CO_Division>().GetAll().OrderBy(z => z.Name).ToList<CO_Division>();

            return lstDivision;
        }

        public List<CO_Circle> GetCirlceByDivisionID(long _DivisionID)
        {
            CO_Division mdlDivision =
                db.Repository<CO_Division>().GetAll().Where(x => x.ID == _DivisionID).FirstOrDefault();
            List<CO_Circle> lstCircle =
                db.Repository<CO_Circle>()
                    .GetAll()
                    .Where(x => x.ID == mdlDivision.CircleID)
                    .OrderBy(x => x.Name)
                    .ToList<CO_Circle>();
            return lstCircle;
        }

        public List<CO_Zone> GetZoneByCirlceID(long _CircleID)
        {
            CO_Circle mdlCircle = db.Repository<CO_Circle>().GetAll().Where(x => x.ID == _CircleID).FirstOrDefault();
            List<CO_Zone> lstZone =
                db.Repository<CO_Zone>()
                    .GetAll()
                    .Where(x => x.ID == mdlCircle.ZoneID)
                    .OrderBy(x => x.Name)
                    .ToList<CO_Zone>();
            return lstZone;
        }

        public bool DeleteAssetByID(long _AssetID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_AM_DeleteAssests", _AssetID);
            bool IsExists = false;
            foreach (DataRow DR in dt.Rows)
            {
                IsExists = Convert.ToBoolean(DR["IsExists"].ToString());
            }
            return IsExists;
        }

        public bool IsWorkUnique(AM_AssetWork _Work)
        {
            bool work_ =
                db.Repository<AM_AssetWork>()
                    .GetAll()
                    .Any(
                        x =>
                            x.FinancialYear == _Work.FinancialYear &&
                            x.WorkName.ToLower().Trim() == _Work.WorkName.ToLower().Trim() &&
                            x.DivisionID == _Work.DivisionID && x.AssetWorkTypeID == _Work.AssetWorkTypeID &&
                            (x.ID != _Work.ID || _Work.ID == 0));
              return work_;

        }

        public AM_AssetItems GetAssetID(long _ID)
        {
            AM_AssetItems qAssetItem = db.Repository<AM_AssetItems>().GetAll().Where(s => s.ID == _ID).FirstOrDefault();
            return qAssetItem;
        }

        public IEnumerable<DataRow> GetAm_AssetAtrtributeDetail(long AssetsID, long SubCategoryid)
        {
            return db.ExecuteDataSet("Proc_AM_AssetAttributeDetail", AssetsID, SubCategoryid);
        }

        public bool SaveAssets(AM_AssetItems _AssetItems)
        {
            bool isSaved = false;

            if (_AssetItems.ID == 0)
            {
                db.Repository<AM_AssetItems>().Insert(_AssetItems);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<AM_AssetItems>().Update(_AssetItems);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool SaveAssetsAttributeDetail(AM_AssetAttributes _AssetAttributes)
        {
            bool isSaved = false;

            if (_AssetAttributes.ID == 0)
            {
                db.Repository<AM_AssetAttributes>().Insert(_AssetAttributes);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<AM_AssetAttributes>().Update(_AssetAttributes);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public List<object> GetcategoryID(long _SubCategoryID)
        {
            return
                db.Repository<AM_SubCategory>()
                    .GetAll()
                    .Where(s => s.IsActive == true && s.ID == _SubCategoryID)
                    .Select(c => new { c.ID, c.Name, c.CategoryID })
                    .ToList<object>();
        }
        public bool isSubCatgFlood(long _SubCategoryID)
        {
            AM_SubCategory SC = (from SubCategory in db.Repository<AM_SubCategory>().GetAll()
                              where SubCategory.ID == _SubCategoryID && SubCategory.FOBit == false
                              select SubCategory).FirstOrDefault();
            if (SC != null && SC.ID > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public AM_SubCategory GetcatID(long _SubCategoryID)
        {
            AM_SubCategory qCat =
                db.Repository<AM_SubCategory>().GetAll().Where(s => s.ID == _SubCategoryID).FirstOrDefault();
            return qCat;
        }

        public bool IsAssetsExist(AM_AssetItems ObjAssets)
        {
            bool qIsAssetExist =
                db.Repository<AM_AssetItems>()
                    .GetAll()
                    .Any(
                        q =>
                            q.AssetName == ObjAssets.AssetName && q.SubCategoryID == ObjAssets.SubCategoryID &&
                            q.IrrigationLevelID == ObjAssets.IrrigationLevelID &&
                            q.IrrigationBoundryID == ObjAssets.IrrigationBoundryID &&
                            (q.ID != ObjAssets.ID || ObjAssets.ID == 0));
            return qIsAssetExist;
        }

        public bool DeleteAssetsAttribute(long _AssetsAttributeID)
        {
            db.Repository<AM_AssetAttributes>().Delete(_AssetsAttributeID);
            db.Save();
            return true;
        }

        public object GeAssetInspectionInd(long _ID)
        {
            AM_AssetInspectionInd mdl =
                db.Repository<AM_AssetInspectionInd>().Query().Get().Where(x => x.ID == _ID).FirstOrDefault();
            if (mdl != null)
            {
                List<AM_Attachment> lstAssetAtchmnt = db.Repository<AM_Attachment>().Query().Get()
                    .Where(x => x.AssetInspectionIndID == mdl.ID).ToList();
                int count = 0;
                string attchemnt = "";
                if (lstAssetAtchmnt != null && lstAssetAtchmnt.Count > 0)
                {
                    count = lstAssetAtchmnt.Count();
                    foreach (var i in lstAssetAtchmnt)
                        attchemnt = attchemnt + i.Attachment + ";";
                }
                object obj = new
                {
                    InspectionDate = Utility.GetFormattedDate(mdl.InspectionDate),
                    InspectionIndID = mdl.ID,
                    AssetItemID = mdl.AssetItemID,
                    ConditionID = mdl.ConditionID,
                    Status = mdl.Status,
                    //Status = Convert.ToString(mdl.Status) == true.ToString() ? "1" : "2",
                    CurrentAssetValue = mdl.CurrentAssetValue,
                    Remarks = mdl.Remarks,
                    CreatedBy = mdl.CreatedBy,
                    CreatedDate = mdl.CreatedDate,
                    AttachmentCount = count,
                    Attchment = attchemnt

                };
                return obj;
            }
            return null;
        }

        public object GeAssetInspectionLot(long _ID)
        {
            AM_AssetInspectionLot mdl =
                db.Repository<AM_AssetInspectionLot>().Query().Get().Where(x => x.ID == _ID).FirstOrDefault();
            if (mdl != null)
            {
                List<AM_Attachment> lstAssetAtchmnt = db.Repository<AM_Attachment>().Query().Get()
                    .Where(x => x.AssetInspectLotID == mdl.ID).ToList();
                int count = 0;
                string attchemnt = "";
                if (lstAssetAtchmnt != null && lstAssetAtchmnt.Count > 0)
                {
                    count = lstAssetAtchmnt.Count();
                    foreach (var i in lstAssetAtchmnt)
                        attchemnt = attchemnt + i.Attachment + ";";
                }
                object obj = new
                {
                    InspectionDate = Utility.GetFormattedDate(mdl.InspectionDate),
                    Quantity = mdl.Quantity,
                    InspectionLotID = mdl.ID,
                    AssetItemID = mdl.AssetItemID,
                    Status = mdl.Status,
                    //Status = Convert.ToString(mdl.Status) == true.ToString() ? "1" : "2",
                    CurrentAssetValue = mdl.CurrentAssetValue,
                    Remarks = mdl.Remarks,
                    CreatedBy = mdl.CreatedBy,
                    CreatedDate = mdl.CreatedDate,
                    AttachmentCount = count,
                    Attchment = attchemnt

                };
                return obj;
            }
            return null;
        }

        public bool SaveAssetInspectionInd(AM_AssetInspectionInd _Mdl, List<Tuple<string, string, string>> _Attachment)
        {
            bool isSaved = false;
            db.Repository<AM_AssetInspectionInd>().Insert(_Mdl);

            foreach (var atchmnt in _Attachment)
            {
                AM_Attachment mdl = new AM_Attachment();
                mdl.AssetInspectionIndID = _Mdl.ID;
                mdl.Attachment = atchmnt.Item3;
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy = _Mdl.CreatedBy;
                mdl.Source = "W";
                db.Repository<AM_Attachment>().Insert(mdl);
            }

            db.Save();
            return isSaved;
        }

        public DataSet GetAssetsHeader(string _Level, long? _AssetsID, long? _IrrigationLevelID,
            long? _IrrigationBoundryID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _CategoryID,
            long? _SubCategoryID, bool? _FloodAssociationID, string _AssetType, string _Status, string _AssetName)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_AM_SearchAssets", _Level, _AssetsID, _IrrigationLevelID,
                _IrrigationBoundryID, _ZoneID, _CircleID, _DivisionID, _CategoryID, _SubCategoryID, _FloodAssociationID,
                _AssetType, _Status, _AssetName);
        }

        public long AssetStatusLotUpdation(long _AssetID, bool _AssetStatus, int _AvailableQty, int _Quantity)
        {
            long _ODIIDOut = 0;
            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_AM_AssetStatusLotUpdate", _AssetID, _AssetStatus,
                _AvailableQty, _Quantity);
            return _ODIIDOut;
        }

        //Proc_AM_AssetStatusIndividualUpdate
        public long AssetStatusIndividualUpdation(long _AssetID, bool _AssetStatus)
        {
            long _ODIIDOut = 0;
            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_AM_AssetStatusIndividualUpdate", _AssetID,
                _AssetStatus);
            return _ODIIDOut;
        }
        public long AssetWorkDetailDelete(long _AssetWorkID)
        {
            long _ODIIDOut = 0;
            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_AM_AssetWorkDetailDelete", _AssetWorkID);
            return _ODIIDOut;
        }
        public bool SaveAssetInspectionLot(AM_AssetInspectionLot _Mdl, List<Tuple<string, string, string>> _Attachment,
            List<Tuple<string, string, string>> _Condition)
        {
            bool isSaved = false;
            db.Repository<AM_AssetInspectionLot>().Insert(_Mdl);

            foreach (var condtn in _Condition)
            {
                AM_AssetInspectLotCondition mdlCondition = new AM_AssetInspectLotCondition();
                mdlCondition.AssetInspectLotID = _Mdl.ID;
                mdlCondition.Quantity = Convert.ToInt32(condtn.Item1);
                mdlCondition.ConditionID = Convert.ToInt64(condtn.Item2);
                mdlCondition.Remarks = condtn.Item3;
                mdlCondition.CreatedDate = DateTime.Now;
                mdlCondition.CreatedBy = _Mdl.CreatedBy;

                db.Repository<AM_AssetInspectLotCondition>().Insert(mdlCondition);
            }

            foreach (var atchmnt in _Attachment)
            {
                AM_Attachment mdl = new AM_Attachment();
                mdl.AssetInspectLotID = _Mdl.ID;
                mdl.Attachment = atchmnt.Item3;
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy = _Mdl.CreatedBy;
                mdl.Source = "W";
                db.Repository<AM_Attachment>().Insert(mdl);
            }

            db.Save();
            return isSaved;
        }

        public string GetDesignationByID(long _DesiganationID)
        {
            return db.Repository<UA_Designations>().Query().Get()
                .Where(x => x.ID == _DesiganationID)
                .FirstOrDefault().Name;
        }

        public List<object> GetIndividualHistoryInd(string _StatusID, List<long> _InspectedBy, long _AssetsID,
            DateTime? _FromDate, DateTime? _ToDate)
        {

            List<AM_AssetInspectionInd> lstInspectionInd = db.Repository<AM_AssetInspectionInd>().Query().Get().
                Where(x => x.AssetItemID == _AssetsID
                           &&
                           ((_FromDate == null ||
                             DbFunctions.TruncateTime(x.InspectionDate) >= DbFunctions.TruncateTime(_FromDate)))
                           &&
                           (_ToDate == null ||
                            (DbFunctions.TruncateTime(x.InspectionDate) <= DbFunctions.TruncateTime(_ToDate)))
                ).OrderByDescending(x => x.ID).ToList();

            if (lstInspectionInd != null && lstInspectionInd.Count() > 0)
                lstInspectionInd =
                    lstInspectionInd.Where(x => _InspectedBy.Contains(x.CreatedByDesigID))
                        .OrderByDescending(x => x.ID)
                        .ToList(); // && _InspectedBy.Contains(x.CreatedByDesigID)

            if (lstInspectionInd != null && lstInspectionInd.Count() > 0 && _StatusID != "All")
                lstInspectionInd =
                    lstInspectionInd.Where(x => x.Status == _StatusID && _InspectedBy.Contains(x.CreatedByDesigID))
                        .OrderByDescending(x => x.ID)
                        .ToList();

            List<object> lstData = new List<object>();

            if (lstInspectionInd != null && lstInspectionInd.Count() > 0)
            {
                lstData = lstInspectionInd.Select(x => new
                {
                    AssetInspectionIndID = x.ID,
                    InspectionDate = Utility.GetFormattedDate(x.InspectionDate),
                    InspectedBy = GetUserNameAndDesignation(x.CreatedBy),
                    //Status = Convert.ToBoolean(x.Status) ? "Active" : "Inactive",
                    Status = x.Status,
                    //Condition = x.AM_AssetCondition.Condition,
                    Condition = x.ConditionID == null ? "" : x.AM_AssetCondition.Condition,

                }).ToList<object>();
            }
            return lstData;
        }

        public List<object> GetIndividualHistoryLot(List<long> _InspectedBy, long _AssetsID, DateTime? _FromDate,
            DateTime? _ToDate)
        {

            List<AM_AssetInspectionLot> lstInspectionInd = db.Repository<AM_AssetInspectionLot>().Query().Get().
                Where(x => x.AssetItemID == _AssetsID
                           &&
                           ((_FromDate == null ||
                             DbFunctions.TruncateTime(x.InspectionDate) >= DbFunctions.TruncateTime(_FromDate)))
                           &&
                           (_ToDate == null ||
                            (DbFunctions.TruncateTime(x.InspectionDate) <= DbFunctions.TruncateTime(_ToDate)))
                ).OrderByDescending(x => x.ID).ToList();

            if (lstInspectionInd != null && lstInspectionInd.Count() > 0)
                lstInspectionInd =
                    lstInspectionInd.Where(x => _InspectedBy.Contains(x.CreatedByDesigID))
                        .OrderByDescending(x => x.ID)
                        .ToList();

            List<object> lstData = new List<object>();

            if (lstInspectionInd != null && lstInspectionInd.Count() > 0)
            {
                lstData = lstInspectionInd.Select(x => new
                {
                    AssetInspectionLotID = x.ID,
                    InspectionDate = Utility.GetFormattedDate(x.InspectionDate),
                    InspectedBy = GetUserNameAndDesignation(x.CreatedBy),

                }).ToList<object>();
            }
            return lstData;
        }

        public List<object> GetAssetshistoryLotCondition(long _InspectionLotID)
        {
            List<AM_AssetInspectLotCondition> lstInspectionLot =
                db.Repository<AM_AssetInspectLotCondition>().Query().Get().
                    Where(x => x.AssetInspectLotID == _InspectionLotID).ToList();

            List<object> lstCondition = new List<object>();

            if (lstInspectionLot != null && lstInspectionLot.Count() > 0)
            {
                lstCondition = lstInspectionLot.Select(x => new
                {
                    AssetInspectLotID = x.AssetInspectLotID,
                    Quantity = x.Quantity,
                    Condition = x.AM_AssetCondition.Condition,
                    Remarks = x.Remarks

                }).ToList<object>();
            }
            return lstCondition;

        }

        public string GetUserNameAndDesignation(long _UserID)
        {
            string name = "";
            UA_Users mdl = db.Repository<UA_Users>().Query().Get().Where(u => u.ID == _UserID).FirstOrDefault();
            if (mdl != null)
            {
                name = mdl.FirstName + " " + mdl.LastName;
                name = name + " (" +
                       db.Repository<UA_Designations>()
                           .Query()
                           .Get()
                           .Where(u => u.ID == mdl.DesignationID)
                           .FirstOrDefault()
                           .Name + ")";
            }
            return name;
        }

        public bool IsAssetInspectionExists(long _AssetsID)
        {

            bool qIsExists = db.Repository<AM_AssetInspectionInd>().GetAll().Any(s => s.AssetItemID == _AssetsID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<AM_AssetInspectionLot>().GetAll().Any(s => s.AssetItemID == _AssetsID);
            }
            if (!qIsExists)
            {
                qIsExists = db.Repository<AM_AssetWorkDetail>().GetAll().Any(s => s.AssetItemsID == _AssetsID);
            }

            return qIsExists;
        }

        #endregion


        #region "Work"

        public List<object> GetSmallDamByDivisionID(long _DivisionID)
        {
            List<object> lstSmallDam = db.ExtRepositoryFor<AssetsWorkRepository>().GetSmallDamByDivisionID(_DivisionID).ToList();
            return lstSmallDam;
        }
        public List<object> GetSmallDamChannelByDivisionID(long _DivisionID)
        {
            List<object> lstSmallDamChannel = db.ExtRepositoryFor<AssetsWorkRepository>().GetSmallDamChannelByDivisionID(_DivisionID).ToList();
            return lstSmallDamChannel;
        }
        public List<object> GetWorkbySearchCriteria(long ZoneID, long CircleID, long DivisionID, string FinancialYear,
            long WorkTypeID, string WorkStatus, long ProgressStatusID, string WorkName)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_AM_AssetWorkSearch", ZoneID, CircleID, DivisionID,
                FinancialYear, WorkTypeID, WorkStatus, ProgressStatusID, WorkName);
            List<object> lstgetitems = (from DataRow dr in dt.Rows
                                        select new
                                        {
                                            ID = Convert.ToInt64(dr["ID"]),
                                            FinancialYear = Convert.ToString(dr["FinancialYear"]),
                                            WorkName = Convert.ToString(dr["WorkName"]),
                                            Division = Convert.ToString(dr["Division"]),
                                            WorkType = Convert.ToString(dr["WorkType"]),
                                            WorkStatus = Convert.ToString(dr["WorkStatus"]),
                                            ProgressStatus = Convert.ToString(dr["ProgressStatus"]),
                                            ProgressPercentage=Convert.ToString(dr["ProgressPercentage"]),
                                            EstimateCost = Convert.ToInt64(dr["EstimateCost"]),
                                            ContractorName = Convert.ToString(dr["ContractorName"]),
                                            ContractorAmount = Convert.ToString(dr["ContractorAmount"])
                                        }).ToList<object>();
            return lstgetitems;


        }

        public bool IsAWInTender(long _AWID)
        {
            List<long> lstWIDs =
                db.Repository<AM_AssetWork>().Query().Get().Where(x => x.ID == _AWID).Select(x => x.ID).ToList<long>();

            List<TM_TenderWorks> lst =
                db.Repository<TM_TenderWorks>()
                    .Query()
                    .Get()
                    .Where(x => x.WorkSource.Equals("ASSET") && lstWIDs.Contains(x.WorkSourceID))
                    .ToList();

            if (lst == null || lst.Count() == 0)
                return false;
            else
                return true;

        }

        public List<object> GetWorkType(bool isActive = true)
        {
            if (isActive)
            {
                return
                    db.Repository<AM_AssetWorkType>()
                        .GetAll()
                        .Select(e => new { ID = e.ID, Name = e.Name })
                        .ToList<object>();
            }
            else
            {
                return
                    db.Repository<AM_AssetWorkType>()
                        .GetAll()
                        .Where(o => o.IsActive == true)
                        .Select(e => new { ID = e.ID, Name = e.Name })
                        .ToList<object>();
            }

        }

        public bool DeleteWorkByID(long _AssetWorkID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_AM_DeleteAssestWork", _AssetWorkID);
            bool IsExists = false;
            foreach (DataRow DR in dt.Rows)
            {
                IsExists = Convert.ToBoolean(DR["IsExists"].ToString());
            }
            return IsExists;
        }

        public bool IsAssetWorkCostEstimationCorrect(long _WorkID)
        {
            bool result = false;
            long? workCost =
                db.Repository<AM_AssetWork>().Query().Get().Where(x => x.ID == _WorkID).FirstOrDefault().EstimatedCost;
            double workItemsCostSum = 0;
            if (workCost != null)
            {
                List<CW_WorkItem> lst = db.Repository<CW_WorkItem>().Query().Get()
                    .Where(x => x.WorkType.Equals("ASSETWORK") && x.WorkID == _WorkID).ToList();
                foreach (var item in lst)
                {
                    double amount = item.SanctionedQty * item.TSRate;
                    workItemsCostSum = workItemsCostSum + amount;
                }
                if (workCost == workItemsCostSum)
                    result = true;
            }

            return result;
        }

        public double? ContractorAmountPerWorkItem(long _ID)
        {
            double? rate = null;
            CW_WorkItem cwItem = db.Repository<CW_WorkItem>().Query().Get()
                .Where(x => x.ID == _ID && x.WorkType.Equals("ASSETWORK"))
                .FirstOrDefault();

            TM_TenderWorks mdlTndr = db.Repository<TM_TenderWorks>().Query().Get()
                .Where(x => x.WorkSource.Equals("ASSET") && x.WorkSourceID == cwItem.WorkID && x.WorkStatusID == 5)
                .FirstOrDefault();

            if (mdlTndr != null)
            {
                long tndrID = mdlTndr.ID;

                TM_TenderWorksContractors mdlCntrctr = db.Repository<TM_TenderWorksContractors>().Query().Get()
                    .Where(x => x.TenderWorksID == tndrID && x.Awarded == true)
                    .FirstOrDefault();
                if (mdlCntrctr != null)
                {
                    double? cRate = db.Repository<TM_TenderPrice>().Query().Get()
                        .Where(x => x.TWContractorID == mdlCntrctr.ID && x.WorkItemID == _ID)
                        .FirstOrDefault().ContractorRate;

                    if (cRate != null)
                    {
                        rate = cwItem.SanctionedQty * cRate;
                    }
                }

            }
            return rate;
        }

        public List<object> GetProgressStatus()
        {
            return db.Repository<CW_WorkStatus>().GetAll().Select(e => new { ID = e.ID, Name = e.Name }).ToList<object>();
        }

        public List<object> GetFinancialYear()
        {
            List<object> lstObj = new List<object>();
            lstObj =
                db.Repository<AM_AssetWork>()
                    .Query()
                    .Get()
                    .ToList()
                    .OrderByDescending(x => x.FinancialYear)
                    .Select(x => new { ID = x.ID, Name = x.FinancialYear })
                    .Select(x => x.Name)
                    .Distinct()
                    .ToList<object>();
            List<object> lst = new List<object>();
            int counter = 1;
            foreach (object item in lstObj)
            {
                lst.Add(new { ID = counter, Name = item.ToString() });
                counter++;
            }
            return lst;
        }

        public object GetWorkByID(long _WorkID)
        {
            object work = new object();


            return db.Repository<AM_AssetWork>().Query().Get().Where(x => x.ID == _WorkID)
                .Select(x => new { x.ID, x.FinancialYear, x.AssetWorkTypeID, x.WorkName, x.DivisionID, x.EstimatedCost })
                .ToList()
                .Select(x =>
                        new
                        {
                            ID = x.ID,
                            FinancialYear = x.FinancialYear,
                            AssetWorkType = GetWorkTypeNameByID(Convert.ToInt64(x.AssetWorkTypeID)),
                            Division = GetDivisionNameByID(Convert.ToInt64(x.DivisionID)),
                            WorkName = x.WorkName,
                            EstimatedCost = String.Format("{0:#,##0.##}", x.EstimatedCost)
                        }
                ).ToList().ElementAt(0);
            return work;
        }

        public string GetDivisionNameByID(long _DivisionID)
        {
            return db.Repository<CO_Division>().FindById(_DivisionID).Name;
        }

        public string GetWorkTypeNameByID(long _DivisionID)
        {
            return db.Repository<AM_AssetWorkType>().FindById(_DivisionID).Name;
        }

        public List<object> GetWorkProgressHistory(int _WorkStatusID, List<long> _InspectedBy, long _WorkID,
            DateTime? _FromDate, DateTime? _ToDate)
        {
            List<AM_AssetWorkProgress> lstWorksProgress = db.Repository<AM_AssetWorkProgress>().Query().Get().
                Where(x => x.AssetWorkID == _WorkID
                           &&
                           ((_FromDate == null ||
                             DbFunctions.TruncateTime(x.InspectionDate) >= DbFunctions.TruncateTime(_FromDate)))
                           &&
                           (_ToDate == null ||
                            (DbFunctions.TruncateTime(x.InspectionDate) <= DbFunctions.TruncateTime(_ToDate)))
                ).OrderByDescending(x => x.ID).ToList();

            if (lstWorksProgress != null && lstWorksProgress.Count() > 0)
                lstWorksProgress =
                    lstWorksProgress.Where(x => _InspectedBy.Contains(x.CreatedByDesigID))
                        .OrderByDescending(x => x.ID)
                        .ToList();
            List<object> lstData = new List<object>();

            if (lstWorksProgress != null && lstWorksProgress.Count() > 0)
            {
                lstData = lstWorksProgress.Select(x => new
                {
                    ID = x.ID,
                    Date = Utility.GetFormattedDate(x.InspectionDate),
                    InspectedBy = GetUserNameAndDesignation(x.CreatedBy, x.CreatedByDesigID),
                    WorkStatus = x.CW_WorkStatus.Name,
                    PrgPrcntg = x.ProgressPercentage,
                    FinancialPercentage = x.FinancialPercentage

                }).ToList<object>();
            }
            return lstData;
        }

        public object GetWorkProgressByUser(long _AWID, long _UserID)
        {
            List<AM_AssetWorkProgress> lst = db.Repository<AM_AssetWorkProgress>().Query().Get()
                .Where(x => x.AssetWorkID == _AWID && x.CreatedBy == _UserID).OrderByDescending(x => x.ID).ToList();
            if (lst != null && lst.Count() > 0)
            {
                AM_AssetWorkProgress mdl = lst.ElementAt(0);
                object obj = new
                {
                    Date = Common.Utility.GetFormattedDate(mdl.InspectionDate),
                    Progress = mdl.ProgressPercentage,
                    FinancialPercentage = mdl.FinancialPercentage
                };

                return obj;
            }
            return null;
        }

        public object GetCWDetailByID(long _AWID)
        {
            AM_AssetWork mdlCW = db.Repository<AM_AssetWork>().FindById(_AWID);

            object obj = new
            {
                AWName = mdlCW.WorkName,
                FinancialYear = mdlCW.FinancialYear,
                AWWorkType = mdlCW.AM_AssetWorkType.Name

            };

            return obj;
        }

        public string GetUserNameAndDesignation(long _UserID, long _DesignationID)
        {
            string name = "";
            UA_Users mdl = db.Repository<UA_Users>().Query().Get().Where(u => u.ID == _UserID).FirstOrDefault();
            if (mdl != null)
            {
                name = mdl.FirstName + " " + mdl.LastName;
                name = name + " (" +
                       db.Repository<UA_Designations>()
                           .Query()
                           .Get()
                           .Where(u => u.ID == _DesignationID)
                           .FirstOrDefault()
                           .Name + ")";
            }
            return name;
        }

        public object GetWorkProgress(long _ID)
        {
            AM_AssetWorkProgress mdl =
                db.Repository<AM_AssetWorkProgress>().Query().Get().Where(x => x.ID == _ID).FirstOrDefault();
            if (mdl != null)
            {
                List<AM_AssetWorkProgressAttachment> lstPrgrsAtchmnt =
                    db.Repository<AM_AssetWorkProgressAttachment>().Query().Get()
                        .Where(x => x.AssetWorkProgressID == mdl.ID).ToList();
                int count = 0;
                string attchemnt = "";
                if (lstPrgrsAtchmnt != null && lstPrgrsAtchmnt.Count > 0)
                {
                    count = lstPrgrsAtchmnt.Count();
                    foreach (var i in lstPrgrsAtchmnt)
                        attchemnt = attchemnt + i.Attachment + ";";
                }
                object obj = new
                {
                    InspectionDate = Utility.GetFormattedDate(mdl.InspectionDate),
                    ProgressPercentage = mdl.ProgressPercentage,
                    //WorkStatus = mdl.CW_WorkStatus.Name,
                    WorkStatus = mdl.WorkProgressID,
                    FinancialPercentage = mdl.FinancialPercentage,
                    Remarks = mdl.Remarks,
                    AttachmentCount = count,
                    Attchment = attchemnt
                };
                return obj;
            }
            return null;
        }

        public List<object> GetWorkStatusList()
        {
            return db.Repository<CW_WorkStatus>().GetAll().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
        }

        public void SaveWorkProgressW(AM_AssetWorkProgress _Mdl, List<Tuple<string, string, string>> _Attachment)
        {
            db.Repository<AM_AssetWorkProgress>().Insert(_Mdl);

            foreach (var atchmnt in _Attachment)
            {
                AM_AssetWorkProgressAttachment mdl = new AM_AssetWorkProgressAttachment();
                mdl.AssetWorkProgressID = _Mdl.ID;
                mdl.Attachment = atchmnt.Item3;
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy = _Mdl.CreatedBy;

                db.Repository<AM_AssetWorkProgressAttachment>().Insert(mdl);
            }

            db.Save();

        }
        public void UpdateScheduleDetailWorks(long _RefMonitoringID, long _ScheduleDetailID)
        {
            SI_ScheduleDetailWorks mdlWorks = db.Repository<SI_ScheduleDetailWorks>().GetAll().Where(x => x.ID == _ScheduleDetailID).FirstOrDefault();
            mdlWorks.RefMonitoringID = _RefMonitoringID;
            db.Repository<SI_ScheduleDetailWorks>().Update(mdlWorks);
            db.Save();
        }
        #endregion

        #region "Add Work"

        public List<CO_Division> GetByID(long _DivisionID)
        {
            List<CO_Division> lstDivision =
                db.Repository<CO_Division>()
                    .GetAll()
                    .Where(x => x.ID == _DivisionID)
                    .OrderBy(x => x.Name)
                    .ToList<CO_Division>();

            return lstDivision;
        }

        public List<object> GetInfrastructureName(long _UserId, long _InfrastructureType)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_AM_GetInfrastructureByUserID", _UserId,
                _InfrastructureType);
            List<object> lstInfrastructureName = (from DataRow dr in dt.Rows
                                                  select new
                                                  {
                                                      ID = Convert.ToInt64(dr["ID"]),
                                                      NAME = Convert.ToString(dr["NAME"]),
                                                      InfrastructureTypeID = Convert.ToInt64(dr["InfrastructureTypeID"]),
                                                  }).ToList<object>();
            return lstInfrastructureName;
        }

        public List<object> GetFloodWorkDetailByWorkID(long? structuretype, long _WorkID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_AM_FloodWorkDetail", structuretype, _WorkID);
            List<object> lstWorkDetail = (from DataRow dr in dt.Rows
                                          select new
                                          {
                                              ID = Convert.ToInt64(dr["ID"]),
                                              StructureType = Convert.ToInt64(dr["InfrastructureTypeID"]),
                                              Source = Convert.ToString(dr["InfrastructureType"]),
                                              StructureName = Convert.ToInt64(dr["StructureID"]),
                                              StructureNameText = Convert.ToString(dr["InfrastructureName"])
                                          }).ToList<object>();
            return lstWorkDetail;
        }
        public List<object> GetInfraWorkDetailByWorkID(long _WorkID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_AM_InfrasStructureWorkDetail", _WorkID);
            List<object> lstWorkDetail = (from DataRow dr in dt.Rows
                                          select new
                                          {
                                              ID = Convert.ToInt64(dr["ID"]),
                                              StructureType = Convert.ToInt64(dr["InfrastructureTypeID"]),
                                              Source = Convert.ToString(dr["InfrastructureType"]),
                                              StructureName = Convert.ToInt64(dr["StructureID"]),
                                              StructureNameText = Convert.ToString(dr["InfrastructureName"])
                                          }).ToList<object>();
            return lstWorkDetail;
        }

        public object GetStructureTypeIDByInsfrastructureValue(long _InfrastructureType, long _InfrastructureNamevalue)
        {
            AssetsWorkRepository repFloodOperationsRepository = this.db.ExtRepositoryFor<AssetsWorkRepository>();
            return repFloodOperationsRepository.GetStructureTypeIDByInsfrastructureValue(_InfrastructureType,
                _InfrastructureNamevalue);
        }
        public object GetLastWorkProgressID(long AssetWorkID, long userid)
        {
            AssetsWorkRepository repFloodOperationsRepository = this.db.ExtRepositoryFor<AssetsWorkRepository>();
            return repFloodOperationsRepository.GetLastWorkProgressID(AssetWorkID, userid);
        }
        public object GetAssetWorkByID(long _WorkID)
        {
            object oWork = db.Repository<AM_AssetWork>().GetAll().Where(d => d.ID == _WorkID).Select(p =>
                new
                {
                    ID = p.ID,
                    FinancialYear = p.FinancialYear,
                    DivisionID = p.DivisionID,
                    WorkName = p.WorkName,
                    AssetWorkTypeID = p.AssetWorkTypeID,
                    FundingSourceID = p.FundingSourceID,
                    AssetType = p.AssetType,
                    EstimatedCost = p.EstimatedCost,
                    CompletionPeriod = p.CompletionPeriod,
                    CompletionPeriodUnit = p.CompletionPeriodUnit,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    SanctionNo = p.SanctionNo,
                    SanctionDate = p.SanctionDate,
                    EarnestMoneyType = p.EarnestMoneyType,
                    EarnestMoney = p.EarnestMoney,
                    TenderFees = p.TenderFees,
                    Description = p.Description,
                    WorkStatus = p.WorkStatus,
                    WorkStatusDate = p.WorkStatusDate
                }).Distinct().FirstOrDefault();
            return oWork;


        }

        public object GetContractorNameWithAmountByWorkID(long _WorkID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_GetContractorNameAmount]", _WorkID);
            object item = (from DataRow dr in dt.Rows
                           select new
                           {
                               ContractorName = Convert.ToString(dr["ContractorName"]),
                               ContractorAmount = Convert.ToString(dr["ContractorAmount"])
                           }).FirstOrDefault();
            return item;
        }

        public List<object> GetIrrigationLevel(long XENID, bool IsHeadQuarterDivision)
        {

            List<object> lstIrrigationLevel = null;
            if (IsHeadQuarterDivision == true)
            {
                lstIrrigationLevel =
                    db.Repository<UA_IrrigationLevel>()
                        .GetAll()
                        .Where(
                            x =>
                                x.Name.ToUpper().Equals("ZONE") || x.Name.ToUpper().Equals("CIRCLE") ||
                                x.Name.ToUpper().Equals("DIVISION"))
                        .Select(y => new { ID = y.ID, Name = y.Name })
                        .ToList<object>();
            }
            else
            {
                lstIrrigationLevel =
                    db.Repository<UA_IrrigationLevel>()
                        .GetAll()
                        .Where(x => x.Name.ToUpper().Equals("DIVISION"))
                        .Select(y => new { ID = y.ID, Name = y.Name })
                        .ToList<object>();
            }

            return lstIrrigationLevel;
        }

        public List<object> GetAssetWorkDetailByWorkID(long _WorkID)
        {
            List<object> lstWorkDetail = new List<object>();
            if (_WorkID > 0)
            {
                lstWorkDetail =
                (from awDetail in db.Repository<AM_AssetWorkDetail>().GetAll()
                 join assetItem in db.Repository<AM_AssetItems>().GetAll() on awDetail.AssetItemsID equals
                 assetItem.ID
                 join subCate in db.Repository<AM_SubCategory>().GetAll() on assetItem.SubCategoryID equals
                 subCate.ID
                 join Categ in db.Repository<AM_Category>().GetAll() on subCate.CategoryID equals Categ.ID
                 join level in db.Repository<UA_IrrigationLevel>().GetAll() on assetItem.IrrigationLevelID equals
                 level.ID
                 where awDetail.AssetWorkID == _WorkID
                 select new
                 {
                     ID = awDetail.ID,
                     AssetName = assetItem.ID,
                     Category = Categ.ID,
                     SubCategory = subCate.ID,
                     Level = level.ID
                 }
                ).ToList<object>();

                return lstWorkDetail;
            }
            return lstWorkDetail;
        }

        public bool SaveWork(AM_AssetWork _AssetWork)
        {
            bool isSaved = false;

            if (_AssetWork.ID == 0)
            {
                db.Repository<AM_AssetWork>().Insert(_AssetWork);
                // db.Save();
                foreach (AM_AssetWorkDetail item in _AssetWork.AM_AssetWorkDetail)
                {
                    item.AssetWorkID = _AssetWork.ID;
                    db.Repository<AM_AssetWorkDetail>().Insert(item);
                }
                db.Save();
                isSaved = true;
            }
            else
            {
                AM_AssetWork aw =
                    (from item in db.Repository<AM_AssetWork>().GetAll() where item.ID == _AssetWork.ID select item)
                        .FirstOrDefault();
                aw.FinancialYear = _AssetWork.FinancialYear;
                aw.DivisionID = _AssetWork.DivisionID;
                aw.WorkName = _AssetWork.WorkName;
                aw.FundingSourceID = _AssetWork.FundingSourceID;
                aw.AssetWorkTypeID = _AssetWork.AssetWorkTypeID;
                aw.AssetType = _AssetWork.AssetType;
                aw.EstimatedCost = _AssetWork.EstimatedCost;
                aw.CompletionPeriodFlag = _AssetWork.CompletionPeriodFlag;
                aw.CompletionPeriod = _AssetWork.CompletionPeriod;
                aw.CompletionPeriodUnit = _AssetWork.CompletionPeriodUnit;
                aw.StartDate = _AssetWork.StartDate;
                aw.EndDate = _AssetWork.EndDate;
                aw.SanctionNo = _AssetWork.SanctionNo;
                aw.SanctionDate = _AssetWork.SanctionDate;
                aw.EarnestMoneyType = _AssetWork.EarnestMoneyType;
                aw.EarnestMoney = _AssetWork.EarnestMoney;
                aw.TenderFees = _AssetWork.TenderFees;
                aw.Description = _AssetWork.Description;
                aw.ModifiedBy = _AssetWork.ModifiedBy;
                aw.ModifiedDate = _AssetWork.ModifiedDate;
                db.Repository<AM_AssetWork>().Update(aw);
                db.Save();
                List<AM_AssetWorkDetail> alreadySavelstWD =
                (from item in db.Repository<AM_AssetWorkDetail>().GetAll()
                 where item.AssetWorkID == _AssetWork.ID
                 select item).ToList();
                List<AM_AssetWorkDetail> NewAdded =
                    (from item in _AssetWork.AM_AssetWorkDetail where item.ID == 0 select item).ToList();
                foreach (AM_AssetWorkDetail item in NewAdded)
                {
                    item.AssetWorkID = aw.ID;
                    db.Repository<AM_AssetWorkDetail>().Insert(item);
                    db.Save();
                }
                bool isFind = false;
                if (alreadySavelstWD.Count == _AssetWork.AM_AssetWorkDetail.Count)
                {
                    foreach (AM_AssetWorkDetail item in alreadySavelstWD)
                    {
                        if (_AssetWork.AssetType == "Asset")
                        {
                            AM_AssetWorkDetail ReadyToSaveDetail =
                            (from readyToSave in _AssetWork.AM_AssetWorkDetail
                             where readyToSave.ID == item.ID
                             select readyToSave).FirstOrDefault();
                            item.AssetItemsID = ReadyToSaveDetail.AssetItemsID;
                            item.ModifiedBy = ReadyToSaveDetail.ModifiedBy;
                            item.ModifiedDate = ReadyToSaveDetail.ModifiedDate;
                            db.Repository<AM_AssetWorkDetail>().Update(item);
                        }
                        else if (_AssetWork.AssetType == "Flood")
                        {
                            AM_AssetWorkDetail ReadyToSaveDetail =
                            (from readyToSave in _AssetWork.AM_AssetWorkDetail
                             where readyToSave.ID == item.ID
                             select readyToSave).FirstOrDefault();
                            //item.AssetItemsID = ReadyToSaveDetail.AssetItemsID;
                            item.StructureTypeID = ReadyToSaveDetail.StructureTypeID;
                            item.StructureID = ReadyToSaveDetail.StructureID;
                            item.ModifiedBy = ReadyToSaveDetail.ModifiedBy;
                            item.ModifiedDate = ReadyToSaveDetail.ModifiedDate;
                            db.Repository<AM_AssetWorkDetail>().Update(item);
                        }
                        else if (_AssetWork.AssetType == "Infrastructure")
                        {
                            AM_AssetWorkDetail ReadyToSaveDetail =
                            (from readyToSave in _AssetWork.AM_AssetWorkDetail
                             where readyToSave.ID == item.ID
                             select readyToSave).FirstOrDefault();
                            //item.AssetItemsID = ReadyToSaveDetail.AssetItemsID;
                            item.StructureTypeID = ReadyToSaveDetail.StructureTypeID;
                            item.StructureID = ReadyToSaveDetail.StructureID;
                            item.ModifiedBy = ReadyToSaveDetail.ModifiedBy;
                            item.ModifiedDate = ReadyToSaveDetail.ModifiedDate;
                            db.Repository<AM_AssetWorkDetail>().Update(item);
                        }

                    }
                    db.Save();
                }
                else
                {
                    foreach (AM_AssetWorkDetail si in alreadySavelstWD)
                    {
                        isFind = false;
                        AM_AssetWorkDetail readyToDelete = new AM_AssetWorkDetail();
                        foreach (AM_AssetWorkDetail rtsi in _AssetWork.AM_AssetWorkDetail)
                        {
                            readyToDelete = si;
                            if (rtsi.ID == si.ID)
                            {
                                isFind = true;
                                break;
                            }
                            else
                            {
                                isFind = false;
                            }

                        }
                        if (!isFind)
                        {
                            if (readyToDelete.ID > 0)
                            {
                                db.Repository<AM_AssetWorkDetail>().Delete(readyToDelete);
                                db.Save();
                            }

                        }

                    }
                    //for (int i = 0; i < aw.AM_AssetWorkDetail.Count; i++)
                    //    {
                    //        List<AM_AssetWorkDetail> IsExist = (from item in db.Repository<AM_AssetWorkDetail>().GetAll() where item.ID == aw.AM_AssetWorkDetail[i].ID select item).ToList();
                    //    }
                }


                isSaved = true;
            }

            return isSaved;
        }

        public bool DeleteWork(long _ID)
        {
            db.Repository<AM_AssetWork>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool UpdateWorkStatus(long _ID, long _UserID)
        {
            AM_AssetWork aw = db.Repository<AM_AssetWork>().FindById(_ID);
            aw.WorkStatus = "Published";
            db.Save();
            return true;
        }

        public bool UpdateWorkStatusDraft(long _ID)
        {
            AM_AssetWork aw = db.Repository<AM_AssetWork>().FindById(_ID);
            aw.WorkStatus = "Draft";
            db.Save();
            return true;
        }

        public bool isWorkItemExist(long _WorkID)
        {
            CW_WorkItem wi = (from workitem in db.Repository<CW_WorkItem>().GetAll()
                              where workitem.WorkID == _WorkID && workitem.WorkType == "ASSETWORK"
                              select workitem).FirstOrDefault();
            if (wi != null && wi.ID > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<object> GetAssetNameBySubCategoryAndLevelID(long _subCategoryID, long _irrigationLevelID,
            long AssetWorkID)
        {
            if (AssetWorkID == 0)
                return
                    db.Repository<AM_AssetItems>()
                        .GetAll()
                        .Where(
                            x =>
                                x.SubCategoryID == _subCategoryID && x.IrrigationLevelID == _irrigationLevelID &&
                                x.IsAuctioned == false)
                        .Select(x => new { ID = x.ID, Name = x.AssetName })
                        .ToList<object>();
            else
                return
                    db.Repository<AM_AssetItems>()
                        .GetAll()
                        .Where(x => x.SubCategoryID == _subCategoryID && x.IrrigationLevelID == _irrigationLevelID &&
                            x.IsAuctioned == false)
                        .Select(x => new { ID = x.ID, Name = x.AssetName })
                        .ToList<object>();
        }

        #endregion

        #region "Add Work Progress"
        public bool IsAssetExistInWork(long _AWID)
        {
            bool qIsExists = db.Repository<AM_AssetWorkDetail>().GetAll().Any(s => s.AssetWorkID == _AWID && s.AssetItemsID!=null);
            return qIsExists;
        }
        public bool IsAssetTypeWork(long _AWID)
        {
            bool qIsExists = db.Repository<AM_AssetWork>().GetAll().Any(s => s.ID == _AWID && s.AssetType == "Asset");
            return qIsExists;
        }
        public void SaveWorkProgress(AM_AssetWorkProgress _Mdl, List<Tuple<string, string, string>> _Attachment)
        {
            db.Repository<AM_AssetWorkProgress>().Insert(_Mdl);

            foreach (var atchmnt in _Attachment)
            {
                AM_AssetWorkProgressAttachment mdl = new AM_AssetWorkProgressAttachment();
                //  mdl.ID = db.Repository<AM_AssetWorkProgress>().GetAll().Count() + 1;
                mdl.AssetWorkProgressID = _Mdl.ID;
                mdl.Attachment = atchmnt.Item3;
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy = _Mdl.CreatedBy;

                db.Repository<AM_AssetWorkProgressAttachment>().Insert(mdl);
            }

            db.Save();
        }

        #endregion

        #region "Android"

        public List<object> GetAllYears()
        {
            List<object> lstObj = new List<object>();
            lstObj =
                db.Repository<AM_AssetWork>()
                    .Query()
                    .Get()
                    .ToList()
                    .OrderByDescending(x => x.ID)
                    .Select(x => new { ID = x.FinancialYear, Name = x.FinancialYear, Type = "A" })
                    .Distinct()
                    .ToList<object>();
            return lstObj.OrderByDescending(x => Convert.ToString(x.GetType().GetProperty("Name").GetValue(x))).ToList();
        }

        public List<object> GetActiveWorkTypes()
        {

            return
                db.Repository<AM_AssetWorkType>()
                    .GetAll()
                    .Where(x => x.IsActive == true)
                    .Select(e => new { ID = e.ID, Name = e.Name, Type = "A" })
                    .ToList<object>();
        }

        public List<object> GetWorkProgressStatus()
        {
            return
                db.Repository<CW_WorkStatus>()
                    .GetAll()
                    .Where(x => x.IsActive == true)
                    .Select(e => new { ID = e.ID, Name = e.Name, Type = "A" })
                    .ToList<object>();
        }

        /// <summary>
        /// This method return List of Asset Works by DivisionID and WorkTypeID
        /// Created On 21-03-2017.
        /// </summary>
        /// <returns> List<object></returns>
        public List<object> GetAssetWorksByDivisionID(long? _DivisionID, int? _WorkTypeID, string _Year, int _UserID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[AM_GetAssetWorksData]", _DivisionID, _WorkTypeID, _Year,
                _UserID);
            List<object> lstClosureWorkDetails = (from DataRow dr in dt.Rows
                                                  select new
                                                  {
                                                      WorkID = Convert.ToInt32(dr["WorkID"]),
                                                      WorkName = dr["WorkName"].ToString(),
                                                      DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                      YEAR = dr["YEAR"].ToString(),
                                                      Status = dr["Status"].ToString(),
                                                      WorkTypeID = Convert.ToInt32(dr["WorkTypeID"]),
                                                      WorkTypeName = dr["WorkTypeName"].ToString(),
                                                      EstimatedCost =
                                                      @String.Format(new CultureInfo("ur-PK"), "{0:c}", Convert.ToInt64(dr["EstimatedCost"])),
                                                      WorkProgressID =
                                                      (dr["WorkProgressID"] == DBNull.Value)
                                                          ? dr["WorkProgressID"]
                                                          : Convert.ToInt32(dr["WorkProgressID"]),
                                                      WorkStatusID =
                                                      (dr["WorkStatusID"] == DBNull.Value) ? dr["WorkStatusID"] : Convert.ToInt32(dr["WorkStatusID"]),
                                                      ProgressPercentage =
                                                      (dr["ProgressPercentage"] == DBNull.Value)
                                                          ? dr["ProgressPercentage"]
                                                          : Convert.ToDouble(dr["ProgressPercentage"]),
                                                      FinancialPercentage =
                                                      (dr["FinancialPercentage"] == DBNull.Value)
                                                          ? dr["FinancialPercentage"]
                                                          : Convert.ToDouble(dr["FinancialPercentage"]),
                                                      ModifiedDate =
                                                      (dr["ModifiedDate"] == DBNull.Value) ? dr["ModifiedDate"] : dr["ModifiedDate"].ToString()


                                                  }).ToList<object>();
            return lstClosureWorkDetails;
        }

        public long AddAssetWorkProgress(long _WorkProgressID, long _AssetWorkID, double _ProgressPercentage,
            double _FinancialPercentage, int _WorkStatusID, double? longitude, double? latitude, string _Source,
            string _Remarks, List<Tuple<string, string, string>> lstNameofFiles, int _UserID, long _ScheduleDetailID)
        {
            AM_AssetWorkProgress mdlAMProgress = new AM_AssetWorkProgress();
            //  mdlAMProgress.ID = db.Repository<AM_AssetWorkProgress>().GetAll().Count() + 1;
            mdlAMProgress.ModifiedBy = Convert.ToInt32(_UserID);
            mdlAMProgress.CreatedBy = Convert.ToInt32(_UserID);
            mdlAMProgress.ModifiedDate = DateTime.Now;
            mdlAMProgress.CreatedDate = DateTime.Now;
            mdlAMProgress.CreatedByDesigID = Convert.ToInt32(new LoginDAL().GetAndroidUserDesignationID(_UserID));

            mdlAMProgress.AssetWorkID = _AssetWorkID;
            mdlAMProgress.ProgressPercentage = _ProgressPercentage;
            mdlAMProgress.FinancialPercentage = _FinancialPercentage;
            mdlAMProgress.WorkProgressID = _WorkStatusID;
            mdlAMProgress.GIS_X = longitude;
            mdlAMProgress.GIS_Y = latitude;
            mdlAMProgress.InspectionDate = DateTime.Now;
            mdlAMProgress.Remarks = _Remarks;
            mdlAMProgress.Source = _Source;
            SaveWorkProgress(mdlAMProgress, lstNameofFiles);
            //if (_ScheduleDetailID != -1)
            //{
            //    UpdateScheduleDetailWorks(mdlCWProgress.ID, _ScheduleDetailID);
            //}
            return mdlAMProgress.ID;
        }

        public object GetAssetWorkProgressByID(long _WorkPorgressID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetAssetWorkProgressByID(_WorkPorgressID);
        }

        public object GetAssetWorkByID(long _WorkID, long? WorkProgressID, int _UserID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[AM_GetAssetWorkByID]", _WorkID, WorkProgressID, _UserID);
            object ObjAssetWorkDetails = (from DataRow dr in dt.Rows
                                          select new
                                          {
                                              WorkID = Convert.ToInt32(dr["WorkID"]),
                                              WorkName = dr["WorkName"].ToString(),
                                              DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                              YEAR = dr["FinancialYear"].ToString(),
                                              Status = dr["Status"].ToString(),
                                              WorkTypeID = Convert.ToInt32(dr["WorkTypeID"]),
                                              WorkTypeName = dr["WorkTypeName"].ToString(),
                                              EstimatedCost =
                                              @String.Format(new CultureInfo("ur-PK"), "{0:c}", Convert.ToInt64(dr["EstimatedCost"])),
                                              WorkProgressID =
                                              (dr["WorkProgressID"] == DBNull.Value)
                                                  ? dr["WorkProgressID"]
                                                  : Convert.ToInt32(dr["WorkProgressID"]),
                                              WorkStatusID =
                                              (dr["WorkStatusID"] == DBNull.Value) ? dr["WorkStatusID"] : Convert.ToInt32(dr["WorkStatusID"]),
                                              ProgressPercentage =
                                              (dr["ProgressPercentage"] == DBNull.Value)
                                                  ? dr["ProgressPercentage"]
                                                  : Convert.ToDouble(dr["ProgressPercentage"]),
                                              FinancialPercentage =
                                              (dr["FinancialPercentage"] == DBNull.Value)
                                                  ? dr["FinancialPercentage"]
                                                  : Convert.ToDouble(dr["FinancialPercentage"]),
                                              ModifiedDate =
                                              (dr["ModifiedDate"] == DBNull.Value) ? dr["ModifiedDate"] : dr["ModifiedDate"].ToString(),
                                              Remarks = dr["Remarks"].ToString(),
                                              IsInspectionAdded = Convert.ToBoolean(dr["IsInspectionAdded"].ToString())
                                          }).AsEnumerable().Select(q => new
            {
                WorkID = q.WorkID,
                WorkName = q.WorkName,
                DivisionID = q.DivisionID,
                YEAR = q.YEAR,
                Status = q.Status,
                WorkTypeID = q.WorkTypeID,
                WorkTypeName = q.WorkTypeName,
                EstimatedCost = q.EstimatedCost,
                WorkProgressID = q.WorkProgressID,
                WorkStatusID = q.WorkStatusID,
                ProgressPercentage = q.ProgressPercentage,
                FinancialPercentage = q.FinancialPercentage,
                ModifiedDate = q.ModifiedDate,
                IsInspectionAdded = q.IsInspectionAdded,
                AttachmentList =
                GetWorkProgressAttachment(
                    Convert.ToInt64((q.WorkProgressID == DBNull.Value) ? -1 : Convert.ToInt32(q.WorkProgressID))),
                Remarks = q.Remarks


            }).FirstOrDefault();
            return ObjAssetWorkDetails;
        }

        public List<object> GetWorkProgressAttachment(long? _WorkProgressID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetWorkProgressAttachment(_WorkProgressID);

        }

        #endregion

        #region Reports

        public List<UA_IrrigationLevel> GetIrrigationLevelForReports()
        {
            Int64[] ids = { 1, 2, 3, 6 };
            List<UA_IrrigationLevel> lstIrrigationLevel =
                db.Repository<UA_IrrigationLevel>().GetAll().Where(x => ids.Contains(x.ID)).ToList<UA_IrrigationLevel>();

            return lstIrrigationLevel;

        }

        public List<object> GetAttributeList(long? _SubcatID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetAttributeList(_SubcatID);
        }

        public List<object> GetAttributeValueList(long? _AttributeID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetAttributeValueList(_AttributeID);
        }

        public List<object> GetAssetName(long? _SubcatID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetAssetName(_SubcatID);
        }

        public List<object> GetAssetName(long? _SubcatID, long? IrrigationLevelID, long? IrrigationBoundaryID)
        {
            return db.ExtRepositoryFor<AssetsWorkRepository>().GetAssetName(_SubcatID, IrrigationLevelID, IrrigationBoundaryID);
        }

        public List<object> GetAssetWorkDetailByWorkIDForTender(long _WorkID)
        {
            List<object> lstWorkDetail = new List<object>();
            if (_WorkID > 0)
            {
                lstWorkDetail =
                (from awDetail in db.Repository<AM_AssetWorkDetail>().GetAll()
                 join assetItem in db.Repository<AM_AssetItems>().GetAll() on awDetail.AssetItemsID equals
                 assetItem.ID
                 join subCate in db.Repository<AM_SubCategory>().GetAll() on assetItem.SubCategoryID equals
                 subCate.ID
                 join Categ in db.Repository<AM_Category>().GetAll() on subCate.CategoryID equals Categ.ID
                 join level in db.Repository<UA_IrrigationLevel>().GetAll() on assetItem.IrrigationLevelID equals
                 level.ID
                 where awDetail.AssetWorkID == _WorkID
                 select new
                 {
                     ID = awDetail.ID,
                     AssetName = assetItem.AssetName,
                     Category = Categ.Name,
                     SubCategory = subCate.Name,
                     Level = level.Name
                 }
                ).ToList<object>();

                return lstWorkDetail;
            }
            return lstWorkDetail;
        }
      
        #endregion
    }
}

