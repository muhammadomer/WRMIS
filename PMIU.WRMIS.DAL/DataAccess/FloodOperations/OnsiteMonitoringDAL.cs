using PMIU.WRMIS.DAL.Repositories.FloodOperations;
using PMIU.WRMIS.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;

namespace PMIU.WRMIS.DAL.DataAccess.FloodOperations
{
    public class OnsiteMonitoringDAL : BaseDAL
    {
        public IEnumerable<DataRow> FO_OMStonePosition(long _FFPID, long _StructureTypeID, long _StructureID)
        {
            return db.ExecuteDataSet("Proc_FO_OMStonePosition", _FFPID, _StructureTypeID, _StructureID);
        }

        public object GETFO_OMStonePositionID(long SDID)
        {
            OnsiteMonitoringRepository repFloodOperations = this.db.ExtRepositoryFor<OnsiteMonitoringRepository>();
            return repFloodOperations.GETFO_OMStonePositionID(SDID);
        }


        public List<object> GETFO_OMStonePositionIDList(long _FFPID, long _StructureTypeID, long _StructureID)
        {


            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_OMStonePosition]", _FFPID, _StructureTypeID, _StructureID);
            List<object> lstomstonepos = (from DataRow dr in dt.Rows
                                                select new

                                                      {
                                                          SDID = Convert.ToInt64(dr["SDID"]),
                                                          QtyOfStone = Convert.ToInt64(dr["QtyOfStone"]),
                                                          RD = (dr["RD"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["RD"]),
                                                          OnSiteQty = Convert.ToInt64(dr["OnSiteQty"])

                                                      }).ToList<object>();
            return lstomstonepos;


        }


        public bool SaveFO_OMStonePosition(FO_OMStonePosition _ObjModel)
        {
            bool isSaved = false;

            if (_ObjModel.ID == 0)
                db.Repository<FO_OMStonePosition>().Insert(_ObjModel);
            else
                db.Repository<FO_OMStonePosition>().Update(_ObjModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }


        public long SaveFO_OMStonePosition(long _StoneDeploymentID, int _OnSiteQty, int _UserID)
        {
            object lstF_OMSPID = GETFO_OMStonePositionID(Convert.ToInt64(_StoneDeploymentID));
            int OMSPID = 0;
            string StructypeName = string.Empty;

            if (lstF_OMSPID != null)
            {

                OMSPID = Convert.ToInt32(lstF_OMSPID.GetType().GetProperty("ID").GetValue(lstF_OMSPID));
            }
            FO_OMStonePosition mdlom_stoneposition = new FO_OMStonePosition();
            if (OMSPID != 0)
            {
                mdlom_stoneposition = db.Repository<FO_OMStonePosition>().FindById(OMSPID);
                mdlom_stoneposition.CreatedBy = Convert.ToInt32(mdlom_stoneposition.CreatedBy);
                mdlom_stoneposition.CreatedDate = Convert.ToDateTime(mdlom_stoneposition.CreatedDate);

                mdlom_stoneposition.ModifiedBy = Convert.ToInt32(_UserID);
                mdlom_stoneposition.ModifiedDate = DateTime.Now;
            }
            else
            {
                mdlom_stoneposition.CreatedBy = Convert.ToInt32(_UserID);
               
                mdlom_stoneposition.CreatedDate = DateTime.Now;
            }
            mdlom_stoneposition.StoneDeploymentID = _StoneDeploymentID;
            mdlom_stoneposition.OnSiteQty = _OnSiteQty;
            mdlom_stoneposition.MonitoringDate = DateTime.Now;


            SaveOM_StonePosition(mdlom_stoneposition);

            return mdlom_stoneposition.ID;

        }

        public bool SaveOM_StonePosition(FO_OMStonePosition _Obj)
        {
            bool isSaved = false;

            if (_Obj.ID == 0)
            {
                db.Repository<FO_OMStonePosition>().Insert(_Obj);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_OMStonePosition>().Update(_Obj);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }



        public DataSet GetOMDetail(long? FFPSPID, long? CampSiteID, string InfrastructureType, string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_OMHeader", FFPSPID, CampSiteID, InfrastructureType, InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        }

        public IEnumerable<DataRow> GetOnsiteMonitoringSearch(long? FFPSPID, string InfrastructureType, string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        {
            return db.ExecuteDataSet("Proc_FO_OMSearch", FFPSPID, InfrastructureType, InfrastructureName,
                _ZoneID, _CircleID, _DivisionID, Year);
        }
        //public List<FO_FFPCampSites> GetFFPCampSiteByFFPCampSiteID(long _FFPCampSite)
        //{
        //    List<FO_FFPCampSites> lstOMCampSite = db.Repository<FO_FFPCampSites>().GetAll().Where(x => x.ID == _FFPCampSite).ToList();
        //    return lstOMCampSite;
        //}
        public List<FO_FFPCampSites> GetFFPCampSiteByFFPCampSiteID(long _FFPID, long _StructureID, long _StructureTypeID)
        {
            List<FO_FFPCampSites> lstOMCampSite = db.Repository<FO_FFPCampSites>().GetAll().Where(x => x.FFPID == _FFPID && x.StructureID==_StructureID && x.StructureTypeID==_StructureTypeID).ToList();
            return lstOMCampSite;
        }

        public List<object> GETFO_OMCampsites(long _FFPCampSite)
        {
            OnsiteMonitoringRepository repOnsiteMonitoring = this.db.ExtRepositoryFor<OnsiteMonitoringRepository>();
            return repOnsiteMonitoring.GETFO_OMCampsites(_FFPCampSite);
        }

        public bool AddCampSite(FO_OMCampSites _OMCampSite)
        {
            db.Repository<FO_OMCampSites>().Insert(_OMCampSite);
            db.Save();
            return true;
        }

        public bool UpdateCampSite(FO_OMCampSites _CampSite)
        {
            FO_OMCampSites mdlCampSite = db.Repository<FO_OMCampSites>().FindById(_CampSite.ID);
            mdlCampSite.MonitoringDate = _CampSite.MonitoringDate;
            mdlCampSite.CampSiteID = _CampSite.CampSiteID;
            mdlCampSite.IsAvailable = _CampSite.IsAvailable;
            mdlCampSite.ModifiedBy = _CampSite.ModifiedBy;
            mdlCampSite.ModifiedDate = _CampSite.ModifiedDate;

            db.Repository<FO_OMCampSites>().Update(mdlCampSite);
            db.Save();
            return true;
        }




        public long SaveFO_OMCampSites(long _CampSiteID, int _isAvailable, int _UserID, long? _FFPCampSiteID)
        {

            
            FO_OMCampSites mdlom_campsites = new FO_OMCampSites();
            if (_CampSiteID != 0)
            {
                mdlom_campsites = db.Repository<FO_OMCampSites>().FindById(_CampSiteID);
                mdlom_campsites.CreatedBy = Convert.ToInt32(mdlom_campsites.CreatedBy);
                mdlom_campsites.CreatedDate = Convert.ToDateTime(mdlom_campsites.CreatedDate);
                mdlom_campsites.ModifiedBy = Convert.ToInt32(_UserID);
                mdlom_campsites.ModifiedDate = DateTime.Now;
            }
            else
            {
                mdlom_campsites.CreatedBy = Convert.ToInt32(_UserID);
               // mdlom_campsites.ModifiedDate = DateTime.Now;
                mdlom_campsites.CreatedDate = DateTime.Now;

            }
            mdlom_campsites.CampSiteID = Convert.ToInt64(_FFPCampSiteID);
            mdlom_campsites.IsAvailable = Convert.ToBoolean(_isAvailable);
            mdlom_campsites.MonitoringDate = DateTime.Now;


            SaveFO_OMCampSites(mdlom_campsites);



            return mdlom_campsites.ID;

        }

        public bool SaveFO_OMCampSites(FO_OMCampSites _Obj)
        {
            bool isSaved = false;

            if (_Obj.ID == 0)
            {
                db.Repository<FO_OMCampSites>().Insert(_Obj);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_OMCampSites>().Update(_Obj);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }


        public object GetOnSiteMonitoringCampSitesObjectByID(long ID)
        {
            OnsiteMonitoringRepository repOnsiteMonitoring = this.db.ExtRepositoryFor<OnsiteMonitoringRepository>();
            return repOnsiteMonitoring.GetOnSiteMonitoringCampSitesObjectByID(ID);
        }


        //public List<object> GetOnsiteMonitoringSearchListObject(long? FFPSPID, long? CampSiteID, string InfrastructureType, string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        //{
        //    DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_OMSearch]", FFPSPID, CampSiteID, InfrastructureType, InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        //    List<object> lstsearchonsitemonitoring = (from DataRow dr in dt.Rows
        //                                              select new

        //                                              {
        //                                                  FFPStonePositionID = Convert.ToInt64(dr["FFPStonePositionID"]),
        //                                                  CampSiteID = Convert.ToInt64(dr["CampSiteID"]),
        //                                                  Year = Convert.ToInt32(dr["Year"]),
        //                                                  Zone = dr["Zone"].ToString(),
        //                                                  Circle = dr["Circle"].ToString(),
        //                                                  Division = dr["Division"].ToString(),
        //                                                  InfrastructureType = dr["InfrastructureType"].ToString(),
        //                                                  InfrastructureName = dr["InfrastructureName"].ToString(),
        //                                                  FFPID = Convert.ToInt64(dr["FFPID"])
                                                          

        //                                              }).ToList<object>();
        //    return lstsearchonsitemonitoring;



        //}


        public List<object> GetOnsiteMonitoringSearchListObject(long? FFPID, string InfrastructureType, string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year, int _UserID)
        {
            long DesignationID = Convert.ToInt64(new LoginDAL().GetAndroidUserDesignationID(Convert.ToInt64(_UserID)));
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_OMSearch]", FFPID, InfrastructureType, InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
            List<object> lstsearchonsitemonitoring = (from DataRow dr in dt.Rows
                                                      select new

                                                      {
                                                          FFPID = Convert.ToInt64(dr["FFPID"]),
                                                          Year = Convert.ToInt32(dr["Year"]),
                                                         
                                                          Zone = dr["Zone"].ToString(),
                                                          Circle = dr["Circle"].ToString(),
                                                          Division = dr["Division"].ToString(),
                                                          InfrastructureType = dr["InfrastructureType"].ToString(),
                                                          InfrastructureName = dr["InfrastructureName"].ToString(),
                                                          DivisionID = Convert.ToInt64(dr["DivisionID"]),
                                                          StructureTypeID = Convert.ToInt64(dr["StructureTypeID"]),
                                                          StructureID = Convert.ToInt64(dr["StructureID"]),
                                                          CanEdit = new FloodOperationsDAL().CanAddEditOnSiteMonitoring(Convert.ToInt32(dr["Year"]), DesignationID)

                                                    }).ToList<object>();
            return lstsearchonsitemonitoring;



        }


        public List<object> GetOnsiteMonitoringCampSiteList(long FFPID, long _StructureTypeID, long _StructureID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_OMCampsites]", FFPID , _StructureTypeID ,  _StructureID);
            List<object> lstsearchonsitemonitoring = (from DataRow dr in dt.Rows
                                                      select new

                                                      {
                                                          FFPCampSiteID = Convert.ToInt64(dr["FFPCampSiteID"]),
                                                          FFPID = Convert.ToInt64(dr["FFPID"]),
                                                          StructureID = Convert.ToInt64(dr["StructureID"]),
                                                          StructureTypeID = Convert.ToInt64(dr["StructureTypeID"]),
                                                          RD = Convert.ToInt64(dr["RD"]),
                                                          Description = dr["Description"].ToString(),
                                                          CreatedBy = Convert.ToInt64(dr["CreatedBy"]),
                                                          isAvailable = (dr["isAvailable"] == DBNull.Value) ? -1 : Convert.ToInt32(dr["isAvailable"]),
                                                          CreatedDate = dr["CreatedDate"].ToString(),
                                                          OMCampSiteID = (dr["OMCampSiteID"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["OMCampSiteID"])                                                      

                                                      }).ToList<object>();
            return lstsearchonsitemonitoring;



        }


        public object GETFO_OMStonePositionObject(long _FFPID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_OMStonePosition]", _FFPID);
            object lstsearchonsitemonitoring = (from DataRow dr in dt.Rows
                                                select new

                                                      {
                                                          SDID = Convert.ToInt64(dr["SDID"]),
                                                          QtyOfStone = Convert.ToInt64(dr["QtyOfStone"]),
                                                          RD = Convert.ToInt64(dr["RD"]),
                                                          OnSiteQty = Convert.ToInt64(dr["OnSiteQty"])

                                                      }).FirstOrDefault();
            return lstsearchonsitemonitoring;



        }

        public FO_OMCampSites GetOMCampSiteByFFPCampSite(long _FFPCampSite)
        {
            FO_OMCampSites mdlOMCampSite = db.Repository<FO_OMCampSites>().GetAll().Where(x => x.CampSiteID == _FFPCampSite).FirstOrDefault();
            return mdlOMCampSite;
        }


        public List<FO_Items> GetItemsByItemCategory(long _ItemCategory)
        {
            List<FO_Items> lstItems = db.Repository<FO_Items>().GetAll().Where(x => x.ItemCategoryID == _ItemCategory).ToList();
            return lstItems;
        }

        public bool SaveOMCampSiteItems(FO_OMCampSiteItems _OMCampSiteItems)
        {
            bool isSaved = false;

            if (_OMCampSiteItems.ID == 0)
            {
                db.Repository<FO_OMCampSiteItems>().Insert(_OMCampSiteItems);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_OMCampSiteItems>().Update(_OMCampSiteItems);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public FO_OMCampSiteItems GetOMCampSiteItemByItemIDAndCampSiteID(long _ItemID, long _CampSiteID)
        {
            FO_OMCampSiteItems mdlOMCampSiteItem = db.Repository<FO_OMCampSiteItems>().GetAll().Where(x => x.OverallDivItemID == _ItemID && x.OMCampSiteID == _CampSiteID).FirstOrDefault();
            return mdlOMCampSiteItem;
        }
        public IEnumerable<DataRow> OMCamSiteItems(long? _DivisionID, int _Year, long? _CampsiteID, long? _Categoryid)
        {
            return db.ExecuteDataSet("Proc_FO_OMCampsiteItem", _DivisionID, _Year, _CampsiteID, _Categoryid);
        }
    }
}