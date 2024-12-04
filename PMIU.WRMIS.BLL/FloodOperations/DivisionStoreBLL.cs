using PMIU.WRMIS.DAL.DataAccess.FloodOperations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.BLL.FloodOperations
{
    public class DivisionStoreBLL : BaseBLL
    {
        DivisionStoreDAL dalDivisionStore = new DivisionStoreDAL();
        public List<FO_DSEntryType> GetDSStockType(string _Source)
        {
            return dalDivisionStore.GetDSStockType(_Source);
        }

        public DataSet GetDivisionStoreSearch(long? _DivisionStoreID, long? _DivisionID, int? _Year, Int16? _ItemCategory)
        {
            return dalDivisionStore.GetDivisionStoreSearch(_DivisionStoreID, _DivisionID, _Year, _ItemCategory);
        }

        public long DivisionStoreInsertion(long _DivisionStoreID, DateTime? _EntryDate, long? _DivisionID, Int32? _ItemCategoryID, long? _ItemID, int? _ItemSubcategoryID, Int32? _EntryTypeID, int? _Quantity, long? _StructureTypeID, long? _StructureID, long? _FFPCampSitesID, string _Reason, long? _ReversalRefID, int _CreatedBy, int _ModifiedBy, long _DSID)
        {
            return dalDivisionStore.DivisionStoreInsertion(_DivisionStoreID, _EntryDate, _DivisionID, _ItemCategoryID, _ItemID, _ItemSubcategoryID, _EntryTypeID, _Quantity, _StructureTypeID, _StructureID, _FFPCampSitesID, _Reason, _ReversalRefID, _CreatedBy, _ModifiedBy, _DSID);
        }
        public IEnumerable<DataRow> DSReceivedStockPurchased(long _DivisionID, long _StructureTypeID, long _StructureID, long _EntryTypeID, long _Categoryid)
        {
            return dalDivisionStore.DSReceivedStockPurchased(_DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }
        public DataSet DSReceivedStockInfraStructure(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid)
        {
            return dalDivisionStore.DSReceivedStockInfraStructure(_DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }
        public IEnumerable<DataRow> DSReceivedStockCampSite(long? _DivisionID, Int32? _EntryTypeID, Int32? _Year, Int32? _Categoryid, string _InfrastructureType, string _InfrastructureName)
        {
            return dalDivisionStore.DSReceivedStockCampSite(_DivisionID, _EntryTypeID, _Year, _Categoryid, _InfrastructureType, _InfrastructureName);
        }
        public IEnumerable<DataRow> GetDSSearchIssuedStock(long? _DivisionID, int? EntryTypeID, int? Year, string InfrastructureType, string InfrastructureName, long? _StructureTypeID, long? _StructureID)
        {
            return dalDivisionStore.GetDSSearchIssuedStock(_DivisionID, EntryTypeID, Year, InfrastructureType, InfrastructureName, _StructureTypeID, _StructureID);
        }
        public DataSet DSReceivedStockCampSiteItems(long? _DivisionID, Int32? _EntryTypeID, Int32? _Categoryid, int FFPCampSitesID)
        {
            return dalDivisionStore.DSReceivedStockCampSiteItems(_DivisionID, _EntryTypeID, _Categoryid, FFPCampSitesID);
        }
        public DataSet DSIssuedStockViewCampSiteItems(long? _DivisionID, Int32? _EntryTypeID, Int32? _Year, Int32? _Categoryid, int _FFPCampSitesID, long? _StructureTypeID, long? _StructureID)
        {
            return dalDivisionStore.DSIssuedStockViewCampSiteItems(_DivisionID, _EntryTypeID, _Year, _Categoryid, _FFPCampSitesID, _StructureTypeID, _StructureID);
        }
        public DataSet DSIssuedStockInfraStructure(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid)
        {
            return dalDivisionStore.DSIssuedStockInfraStructure(_DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }

        public DataSet DSIssuedStockViewInfraStructure(long? _DivisionID, long? _Year, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid)
        {
            return dalDivisionStore.DSIssuedStockViewInfraStructure(_DivisionID, _Year, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }

        public IEnumerable<DataRow> DSIssuedStockFFP(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid)
        {
            return dalDivisionStore.DSIssuedStockFFP(_DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid);
        }
        public DataSet DSIssuedStockCamSiteItems(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _EntryTypeID, long? _Categoryid, long? _FFPcampSiteID)
        {
            return dalDivisionStore.DSIssuedStockCamSiteItems(_DivisionID, _StructureTypeID, _StructureID, _EntryTypeID, _Categoryid, _FFPcampSiteID);
        }
        public IEnumerable<DataRow> DSIssuedStockCampSite(long? _DivisionID, string _InfrastructureType, string _InfrastructureName)
        {
            return dalDivisionStore.DSIssuedStockCampSite(_DivisionID, _InfrastructureType, _InfrastructureName);
        }

        public DataSet GetDSSearchReceivedStock(long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year, int? EntryTypeID, int? ItemCategoryID)
        {
            return dalDivisionStore.GetDSSearchReceivedStock(_ZoneID, _CircleID, _DivisionID, Year, EntryTypeID, ItemCategoryID);
        }
        public IEnumerable<DataRow> GetDivisionStoreViewDetail(long? _DivisionID, int? _Year, Int16? _ItemID)
        {
            return dalDivisionStore.GetDivisionStoreViewDetail(_DivisionID, _Year, _ItemID);
        }
        public DataSet DSViewReceivedStockInfrastructure(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _Categoryid)
        {
            return dalDivisionStore.DSViewReceivedStockInfrastructure(_DivisionID, _StructureTypeID, _StructureID, _Categoryid);
        }
        public DataSet DSViewReceivedStockCamSite(long? _DivisionID, long? _StructureTypeID, long? _StructureID, long? _Categoryid, long? _FFPcampSiteID)
        {
            return dalDivisionStore.DSViewReceivedStockCamSite(_DivisionID, _StructureTypeID, _StructureID, _Categoryid, _FFPcampSiteID);
        }
        public DataSet DSAvailableDivisionQtyItemWise(long? _DivisionID, int? _Year, int? _ItemCategoryID, int? _itemid)
        {
            return dalDivisionStore.DSAvailableDivisionQtyItemWise(_DivisionID, _Year, _ItemCategoryID, _itemid);
        }

        public IEnumerable<DataRow> GetDSMinorItemViewDetail(long? _DivisionID, int? _Year, Int16? _ItemID)
        {
            return dalDivisionStore.GetDSMinorItemViewDetail(_DivisionID, _Year, _ItemID);
        }
        public DataSet GetDivisionStoreItemHistory(long? _DivisionID, int? _Year, int? _ItemID, int? _ItemCategoryID)
        {
            return dalDivisionStore.GetDivisionStoreItemHistory(_DivisionID, _Year, _ItemID, _ItemCategoryID);
        }
    }
}
