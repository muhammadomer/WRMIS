using PMIU.WRMIS.DAL.DataAccess.FloodOperations;
using PMIU.WRMIS.Model;
using System.Collections.Generic;
using System.Data;

namespace PMIU.WRMIS.BLL.FloodOperations
{
    public class OnsiteMonitoringBLL : BaseBLL
    {
        private OnsiteMonitoringDAL dalOnsiteMonitoring = new OnsiteMonitoringDAL();

        public IEnumerable<DataRow> FO_OMStonePosition(long _FFPID, long _StructureTypeID, long _StructureID)
        {
            return dalOnsiteMonitoring.FO_OMStonePosition(_FFPID,  _StructureTypeID,  _StructureID);
        }

        public object GETFO_OMStonePositionID(long SDID)
        {
            return dalOnsiteMonitoring.GETFO_OMStonePositionID(SDID);
        }

        public List<object> GETFO_OMStonePositionIDList(long _FFPID, long _StructureTypeID, long _StructureID)
        {
            return dalOnsiteMonitoring.GETFO_OMStonePositionIDList(_FFPID, _StructureTypeID, _StructureID);
        }

        

        public bool SaveFO_OMStonePosition(FO_OMStonePosition _ObjModel)
        {
            return dalOnsiteMonitoring.SaveFO_OMStonePosition(_ObjModel);
        }

        public DataSet GetOMDetail(long? FFPSPID, long? CampSiteID, string InfrastructureType, string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        {
            return dalOnsiteMonitoring.GetOMDetail(FFPSPID, CampSiteID, InfrastructureType, InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        }

        public IEnumerable<DataRow> GetOnsiteMonitoringSearch(long? FFPSPID, string InfrastructureType,
            string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        {
            return dalOnsiteMonitoring.GetOnsiteMonitoringSearch(FFPSPID, InfrastructureType,
                InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        }

        public List<FO_FFPCampSites> GetFFPCampSiteByFFPCampSiteID(long _FFPID, long _StructureID, long _StructureTypeID)
        {
            return dalOnsiteMonitoring.GetFFPCampSiteByFFPCampSiteID( _FFPID,  _StructureID,  _StructureTypeID);
        }

        public bool AddCampSite(FO_OMCampSites _OMCampSite)
        {
            return dalOnsiteMonitoring.AddCampSite(_OMCampSite);
        }

        public bool UpdateCampSite(FO_OMCampSites _CampSite)
        {
            return dalOnsiteMonitoring.UpdateCampSite(_CampSite);
        }

        public List<object> GetOnsiteMonitoringSearchListObject(long? FFPID, string InfrastructureType, string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year, int _UserID)
        {
            return dalOnsiteMonitoring.GetOnsiteMonitoringSearchListObject(FFPID, InfrastructureType, InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year, _UserID);
        }


        //public object GetOnsiteMonitoringCampSiteList(long _FFPID)
        //{
        //    return dalOnsiteMonitoring.GetOnsiteMonitoringCampSiteList(_FFPID);
        //}

        public object GETFO_OMStonePositionObject(long _FFPSPID)
        {
            return dalOnsiteMonitoring.GETFO_OMStonePositionObject(_FFPSPID);
        }

        public List<object> GETFO_OMCampsites(long _FFPCampSite, long _StructureTypeID, long _StructureID)
        {
            return dalOnsiteMonitoring.GetOnsiteMonitoringCampSiteList(_FFPCampSite , _StructureTypeID ,  _StructureID);
        }

        public long SaveFO_OMStonePosition(long _StoneDeploymentID, int _OnSiteQty, int _UserID)
        {
            return dalOnsiteMonitoring.SaveFO_OMStonePosition(_StoneDeploymentID, _OnSiteQty, _UserID);

        }

        public FO_OMCampSites GetOMCampSiteByFFPCampSite(long _FFPCampSite)
        {
            return dalOnsiteMonitoring.GetOMCampSiteByFFPCampSite(_FFPCampSite);
        }


        public long SaveFO_OMCampSites(long _CampSiteID, int _isAvailable, int _UserID, long? _FFPCampSiteID)
        {
            return dalOnsiteMonitoring.SaveFO_OMCampSites(_CampSiteID, _isAvailable, _UserID, _FFPCampSiteID);

        }

        public object GetOnSiteMonitoringCampSitesObjectByID(long ID)
        {
            return dalOnsiteMonitoring.GetOnSiteMonitoringCampSitesObjectByID(ID);

        }


        public List<FO_Items> GetItemsByItemCategory(long _ItemCategory)
        {
            return dalOnsiteMonitoring.GetItemsByItemCategory(_ItemCategory);
        }

        public bool SaveOMCampSiteItems(FO_OMCampSiteItems _OMCampSiteItems)
        {
            return dalOnsiteMonitoring.SaveOMCampSiteItems(_OMCampSiteItems);
        }

        public FO_OMCampSiteItems GetOMCampSiteItemByItemIDAndCampSiteID(long _ItemID, long _CampSiteID)
        {
            return dalOnsiteMonitoring.GetOMCampSiteItemByItemIDAndCampSiteID(_ItemID, _CampSiteID);
        }
        public IEnumerable<DataRow> OMCamSiteItems(long? _DivisionID, int _Year, long? _CampsiteID, long? _Categoryid)
        {
            return dalOnsiteMonitoring.OMCamSiteItems(_DivisionID, _Year, _CampsiteID, _Categoryid);
        }



    }
}