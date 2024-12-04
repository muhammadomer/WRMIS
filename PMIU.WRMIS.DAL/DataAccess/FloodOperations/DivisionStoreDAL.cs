using System;
using System.Collections.Generic;
using System.Data;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.DAL.Repositories.FloodOperations;

namespace PMIU.WRMIS.DAL.DataAccess.FloodOperations
{
    public class DivisionStoreDAL : BaseDAL
    {

        public DataSet GetDivisionStoreSearch(long? _DivisionStoreID, long? _DivisionID, int? _Year,
            Int16? _ItemCategory)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DivisionStoreSearch", _DivisionStoreID, _DivisionID, _Year, _ItemCategory);
        }

        public List<FO_DSEntryType> GetDSStockType(string _Source)
        {
            return db.ExtRepositoryFor<DivisionStoreRepository>().GetDSStockType(_Source);
        }

        public IEnumerable<DataRow> GetDSSearchIssuedStock(long? _DivisionID, int? EntryTypeID, int? Year, string InfrastructureType, string InfrastructureName, long? _StructureTypeID, long? _StructureID)
        {
            return db.ExecuteDataSet("Proc_FO_DSSearchIssuedStock", _DivisionID, EntryTypeID, Year, InfrastructureType, InfrastructureName, _StructureTypeID, _StructureID);
        }

        public long DivisionStoreInsertion(long _DivisionStoreID, DateTime? _EntryDate, long? _DivisionID,
            Int32? _ItemCategoryID, long? _ItemID, int? _ItemSubcategoryID, Int32? _EntryTypeID, int? _Quantity,
            long? _StructureTypeID, long? _StructureID, long? _FFPCampSitesID, string _Reason, long? _ReversalRefID,
            int _CreatedBy, int _ModifiedBy, long _DSID)
        {
            long _DSIDOut = 0;

            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_FO_DivisionStoreInsertion", _DivisionStoreID,
                _EntryDate, _DivisionID, _ItemCategoryID, _ItemID, _ItemSubcategoryID, _EntryTypeID, _Quantity,
                _StructureTypeID, _StructureID, _FFPCampSitesID, _Reason, _ReversalRefID, _CreatedBy, _ModifiedBy, _DSID);

            //if (DS.Tables != null && DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow DR in DS.Tables[0].Rows)
            //    {
            //        _DSIDOut = Convert.ToInt64(DR["@pDSIDOut"].ToString());


            //    }
            //}

            return _DSIDOut;
        }
        public IEnumerable<DataRow> DSReceivedStockPurchased(long _DivisionID, long _StructureTypeID, long _StructureID, long _EntryTypeID, long _Categoryid)
        {
            return db.ExecuteDataSet("Proc_FO_DSReceivedStockPurchased", _DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }
        //
        public DataSet DSReceivedStockInfraStructure(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSReceivedStockInfraStructure", _DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }
        public IEnumerable<DataRow> DSReceivedStockCampSite(long? _DivisionID, Int32? _EntryTypeID, Int32? _Year, Int32? _Categoryid, string _InfrastructureType, string _InfrastructureName)
        {
            return db.ExecuteDataSet("Proc_FO_DSReceivedStockCampSite", _DivisionID, _EntryTypeID, _Year, _Categoryid, _InfrastructureType, _InfrastructureName);
        }
        public DataSet DSReceivedStockCampSiteItems(long? _DivisionID, Int32? _EntryTypeID, Int32? _Categoryid, int FFPCampSitesID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSReceivedStockCampSiteItems", _DivisionID, _EntryTypeID, _Categoryid, FFPCampSitesID);
        }
        public DataSet DSIssuedStockViewCampSiteItems(long? _DivisionID, Int32? _EntryTypeID, Int32? _Year, Int32? _Categoryid, int _FFPCampSitesID, long? _StructureTypeID, long? _StructureID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSIssuedStockCampSiteSearch", _DivisionID, _EntryTypeID, _Year, _Categoryid, _FFPCampSitesID, _StructureTypeID, _StructureID);
        }
        public DataSet GetDSSearchReceivedStock(long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year, int? EntryTypeID, int? ItemCategoryID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSSearchReceivedStock", _ZoneID, _CircleID, _DivisionID, Year, EntryTypeID, ItemCategoryID);
        }
        public DataSet DSIssuedStockInfraStructure(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSIssuedStockInfraStructure", _DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }

        public DataSet DSIssuedStockViewInfraStructure(long? _DivisionID, long? _Year, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSIssuedStockViewInfraStructure", _DivisionID, _Year, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }

        public IEnumerable<DataRow> DSIssuedStockFFP(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid)
        {
            return db.ExecuteDataSet("Proc_FO_DSReceivedStockFFP", _DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }
        public DataSet DSIssuedStockCamSiteItems(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid, long? _FFPcampSiteID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSIssuedStockCamSiteItems", _DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid, _FFPcampSiteID);
        }
        public IEnumerable<DataRow> DSIssuedStockCampSite(long? _DivisionID, string _InfrastructureType, string _InfrastructureName)
        {
            return db.ExecuteDataSet("Proc_FO_DSIssuedStockCampSite", _DivisionID, _InfrastructureType, _InfrastructureName);
        }
        public IEnumerable<DataRow> GetDivisionStoreViewDetail(long? _DivisionID, int? _Year, Int16? _ItemID)
        {
            return db.ExecuteDataSet("Proc_FO_DivisionStoreViewDetail", _DivisionID, _Year, _ItemID);
        }
        public DataSet DSViewReceivedStockInfrastructure(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _Categoryid)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSViewReceivedStockInfrastructure", _DivisionID, _StructureTypeID, _StructureID, _Categoryid);
        }
        public DataSet DSViewReceivedStockCamSite(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _Categoryid, long? _FFPcampSiteID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSViewReceivedStockCamSite", _DivisionID, _StructureTypeID, _StructureID, _Categoryid, _FFPcampSiteID);
        }
        public IEnumerable<DataRow> GetDSMinorItemViewDetail(long? _DivisionID, int? _Year, Int16? _ItemID)
        {
            return db.ExecuteDataSet("Proc_FO_DSViewDetailMinorItems", _DivisionID, _Year, _ItemID);
        }
        public DataSet DSAvailableDivisionQtyItemWise(long? _DivisionID, int? _Year, int? _ItemCategoryID, int? _itemid)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DSAvailableDivisionQtyItemWise", _DivisionID, _Year, _ItemCategoryID, _itemid);
        }

        public DataSet GetDivisionStoreItemHistory(long? _DivisionID, int? _Year, int? _ItemID, int? _ItemCategoryID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DivisionStoreHistory", _DivisionID, _Year, _ItemID, _ItemCategoryID);
        }
    }
}
