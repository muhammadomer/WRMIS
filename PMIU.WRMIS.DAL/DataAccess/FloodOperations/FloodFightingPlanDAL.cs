using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.FloodOperations;
using PMIU.WRMIS.Exceptions;

namespace PMIU.WRMIS.DAL.DataAccess.FloodOperations
{
    public class FloodFightingPlanDAL : BaseDAL
    {

        #region Flood Fighting Plan

        public bool IsFightingPlanAlreadyExists(FO_FloodFightingPlan _FightingPlan)
        {
            bool qIsFloodFightingPlanAlreadyExists =
                db.Repository<FO_FloodFightingPlan>().GetAll().Any(i => i.Year == _FightingPlan.Year
                                                                        && i.DivisionID == _FightingPlan.DivisionID &&
                                                                        (i.ID != _FightingPlan.ID ||
                                                                         _FightingPlan.ID == 0));
            return qIsFloodFightingPlanAlreadyExists;
        }



        public bool SaveFloodFightingPlan(FO_FloodFightingPlan _FloodFightingPlan)
        {
            bool isSaved = false;

            if (_FloodFightingPlan.ID == 0)
            {
                db.Repository<FO_FloodFightingPlan>().Insert(_FloodFightingPlan);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_FloodFightingPlan>().Update(_FloodFightingPlan);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public List<object> GetInfrastructures_CampSiteBy_FFPID(long FFPID)
        {
            FloodFightingPlanRepository repFloodOperations = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodOperations.GetInfrastructures_CampSiteBy_FFPID(FFPID);
        }

        public object GetFFPDivisionID(long FFPID)
        {
            FloodFightingPlanRepository repFloodOperations = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodOperations.GetFFPDivisionID(FFPID);
        }

        public string GetFFPStatus(long _FFPID)
        {
            FloodFightingPlanRepository repFloodOperations = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodOperations.GetFFPStatus(_FFPID);
        }

        public bool DeleteFFPCampSites(long _ID)
        {
            db.Repository<FO_FFPCampSites>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsFo_FFPCampSite_IDExists(long _ID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<FO_OverallDivItems>().GetAll().Any(s => s.CS_CampSiteID == _ID);
            return qIsExists;
        }

        public bool IsFFPCampSits_Unique(FO_FFPCampSites _ObjModel)
        {
            FloodFightingPlanRepository repFloodInspection = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodInspection.IsFFPCampSits_Unique(_ObjModel);
        }
        public bool IsFFPCampSitsWithoutRD_Unique(FO_FFPCampSites _ObjModel)
        {
            FloodFightingPlanRepository repFloodInspection = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodInspection.IsFFPCampSitsWithoutRD_Unique(_ObjModel);
        }

        public bool IsFFPCampSits_UniqueByID(FO_FFPCampSites _ObjModel)
        {
            FloodFightingPlanRepository repFloodInspection = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodInspection.IsFFPCampSits_UniqueByID(_ObjModel);
        }
        public bool IsFFPCampSitsWithoutRD_UniqueByID(FO_FFPCampSites _ObjModel)
        {
            FloodFightingPlanRepository repFloodInspection = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodInspection.IsFFPCampSitsWithoutRD_UniqueByID(_ObjModel);
        }

        public bool SaveFFPCampSites(FO_FFPCampSites _ObjModel)
        {
            bool isSaved = false;

            if (_ObjModel.ID == 0)
                db.Repository<FO_FFPCampSites>().Insert(_ObjModel);
            else
                db.Repository<FO_FFPCampSites>().Update(_ObjModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        //public bool IsFFPStonePosition_Unique(FO_FFPStonePosition _ObjModel)
        //{
        //    FloodFightingPlanRepository repFloodInspection = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
        //    return repFloodInspection.IsFFPStonePosition_Unique(_ObjModel);
        //}
        public bool IsFFPStonePosition_Unique(FO_FFPStonePosition _ObjModel)
        {
            bool qIsFFPStonePosition_Unique = db.Repository<FO_FFPStonePosition>().GetAll().Any(q => q.FFPID == _ObjModel.FFPID && q.StructureID == _ObjModel.StructureID && q.StructureTypeID == _ObjModel.StructureTypeID && q.RD == _ObjModel.RD
                         && (q.ID != _ObjModel.ID || _ObjModel.ID == 0));
            return qIsFFPStonePosition_Unique;
        }

        public bool SaveFFPStonePosition(FO_FFPStonePosition _ObjModel)
        {
            bool isSaved = false;

            if (_ObjModel.ID == 0)
                db.Repository<FO_FFPStonePosition>().Insert(_ObjModel);
            else
                db.Repository<FO_FFPStonePosition>().Update(_ObjModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public bool DeleteFFP(long _ID)
        {
            db.Repository<FO_FloodFightingPlan>().Delete(_ID);
            db.Save();
            return true;
        }

        public bool IsFFPDependencyExists(long _FFPID)
        {
            bool isDependanceExists = false;
            try
            {
                isDependanceExists = db.Repository<FO_FFPStonePosition>().GetAll().Any(g => g.FFPID == _FFPID);
                if (!isDependanceExists)
                    isDependanceExists = db.Repository<FO_FFPArrangements>().GetAll().Any(g => g.FFPID == _FFPID);
                if (!isDependanceExists)
                    isDependanceExists = db.Repository<FO_FFPCampSites>().GetAll().Any(g => g.FFPID == _FFPID);
                //--To DO FO Team
                //if (!isDependanceExists)
                //    isDependanceExists = db.Repository<FO_FFPOverallDivItems>().GetAll().Any(g => g.FFPID == _FFPID);
                return isDependanceExists;
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.Business);
                return false;
            }
        }

        public long SaveFO_FFPCampSiteItems(long _ItemID, long _OMCampSiteID, int _OnSiteQty, int _UserID, long _OverallDivItemID)
        {
            //--To DO FO Mobile Team 
            FO_OMCampSiteItems mdlom_campsitesitems = new FO_OMCampSiteItems();
            if (_ItemID != 0)
            {
                mdlom_campsitesitems = db.Repository<FO_OMCampSiteItems>().FindById(_ItemID);
                mdlom_campsitesitems.CreatedBy = Convert.ToInt32(mdlom_campsitesitems.CreatedBy);
                mdlom_campsitesitems.CreatedDate = Convert.ToDateTime(mdlom_campsitesitems.CreatedDate);

                mdlom_campsitesitems.ModifiedBy = Convert.ToInt32(_UserID);
                mdlom_campsitesitems.ModifiedDate = DateTime.Now;
            }
            else
            {
                mdlom_campsitesitems.CreatedBy = Convert.ToInt32(_UserID);
                //  mdlom_campsitesitems.ModifiedDate = DateTime.Now;
                mdlom_campsitesitems.CreatedDate = DateTime.Now;

            }


            mdlom_campsitesitems.OMCampSiteID = Convert.ToInt64(_OMCampSiteID);
            mdlom_campsitesitems.OnSiteQty = Convert.ToInt32(_OnSiteQty);
            mdlom_campsitesitems.OverallDivItemID = Convert.ToInt64(_OverallDivItemID);



            SavetoDbFO_FFPCampSiteItems(mdlom_campsitesitems);



            return mdlom_campsitesitems.ID;

        }



        public long SaveFO_EPItems(long _ItemID, long EPItemID, int _PurchasedQty, int _UserID, int _CurrentQty,
            long EmergencyPurchaseID)
        {
            FO_EPItem mdlom_epitems = new FO_EPItem();
            if (EPItemID != 0)
            {
                mdlom_epitems = db.Repository<FO_EPItem>().FindById(EPItemID);
                mdlom_epitems.CreatedBy = Convert.ToInt32(mdlom_epitems.CreatedBy);
                mdlom_epitems.CreatedDate = Convert.ToDateTime(mdlom_epitems.CreatedDate);

                mdlom_epitems.ModifiedBy = Convert.ToInt32(_UserID);
                mdlom_epitems.ModifiedDate = DateTime.Now;
            }
            else
            {
                mdlom_epitems.CreatedBy = Convert.ToInt32(_UserID);
                //  mdlom_campsitesitems.ModifiedDate = DateTime.Now;
                mdlom_epitems.CreatedDate = DateTime.Now;

            }


            mdlom_epitems.EmergencyPurchaseID = Convert.ToInt64(EmergencyPurchaseID);
            mdlom_epitems.CurrentQty = Convert.ToInt32(_CurrentQty);
            mdlom_epitems.ItemID = Convert.ToInt64(_ItemID);
            mdlom_epitems.PurchasedQty = Convert.ToInt32(_PurchasedQty);



            SavetoDbFO_EPItems(mdlom_epitems);



            return mdlom_epitems.ID;

        }


        public bool SavetoDbFO_EPItems(FO_EPItem _Obj)
        {
            bool isSaved = false;

            if (_Obj.ID == 0)
            {
                db.Repository<FO_EPItem>().Insert(_Obj);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_EPItem>().Update(_Obj);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }


        //--To DO FO Mobile Team 
        //public bool SavetoDbFO_FFPCampSiteItems(FO_FFPCampSiteItems _Obj)
        public bool SavetoDbFO_FFPCampSiteItems(FO_OMCampSiteItems _Obj)
        {
            bool isSaved = false;

            if (_Obj.ID == 0)
            {
                db.Repository<FO_OMCampSiteItems>().Insert(_Obj);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_OMCampSiteItems>().Update(_Obj);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }
        public IEnumerable<DataRow> GetFFPItemsQty(long? _DivisionID, long? _FFPID, long? _Categoryid, int _year, long? _StructureTypeID, long? _StructureID)
        {
            return db.ExecuteDataSet("Proc_FO_FFPCampsiteItem", _DivisionID, _FFPID, _Categoryid, _year, _StructureTypeID, _StructureID);
        }

        //public List<object> GetItemsQtyList(long FFPCampSiteID)
        //{
        //    DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_FFPCampsiteItemWithoutCatID]", FFPCampSiteID);
        //    List<object> lstgetitems = (from DataRow dr in dt.Rows
        //                                select new

        //                                {
        //                                    ItemName = dr["ItemName"].ToString(),
        //                                    ItemId = Convert.ToInt64(dr["ItemId"]),
        //                                    RequiredQty = Convert.ToInt32(dr["RequiredQty"]),
        //                                    Description = dr["Description"].ToString(),
        //                                    ItemCategoryID = Convert.ToInt32(dr["ItemCategoryID"]),
        //                                    CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
        //                                    DivisionQty = Convert.ToInt64(dr["DivisionQty"]),
        //                                    FFPCampSiteItemID = Convert.ToInt64(dr["FFPCampSiteItemID"])


        //                                }).ToList<object>();
        //    return lstgetitems;



        //}

        public List<object> GetItemsQtyList(long FFPCampSiteID, int year, long divisionID, long itemCatID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_OMCampsiteItem]", divisionID, year, FFPCampSiteID, itemCatID);
            List<object> lstgetitems = (from DataRow dr in dt.Rows
                                        select new

                                        {
                                            ItemName = dr["ItemName"].ToString(),
                                            ItemId = Convert.ToInt64(dr["ItemId"]),
                                            OverallDivItemID = Convert.ToInt64(dr["OverallDivItemID"]),
                                            OMID = Convert.ToInt64(dr["OMID"]),
                                            OMCamsiteID = Convert.ToInt64(dr["OMCamsiteID"]),
                                            OMQty = Convert.ToInt32(dr["OMQty"]),
                                            QuantityApprovedFFP = Convert.ToInt32(dr["QuantityApprovedFFP"])
                                            //CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                                            //CreatedDate = Convert.ToDateTime(dr["CreatedDate"])

                                        }).ToList<object>();
            return lstgetitems;



        }

        public List<object> GetItemsQtyEPList(long EPPurchaseID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_EPItemPurchasing]", EPPurchaseID, null);
            List<object> lstgetitems = (from DataRow dr in dt.Rows
                                        select new

                                        {
                                            ItemName = dr["ItemName"].ToString(),
                                            ItemId = Convert.ToInt64(dr["ItemId"]),
                                            PurchasedQty = Convert.ToInt32(dr["PurchasedQty"]),
                                            Purchased_Flood_seasonQty = Convert.ToInt32(dr["Purchased_Flood_seasonQty"]),
                                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                                            EPItemID = (dr["EPItemID"] == DBNull.Value) ? 0 : Convert.ToInt64(dr["EPItemID"]),
                                            ItemCategoryID = Convert.ToInt32(dr["ItemCategoryID"])


                                        }).ToList<object>();
            return lstgetitems;



        }

        public object GetEPItemsByID(long _EPItemID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_EPItemPurchasingByID]", _EPItemID);
            object lstgetitems = (from DataRow dr in dt.Rows
                                  select new

                                  {
                                      ItemName = dr["ItemName"].ToString(),
                                      ItemID = Convert.ToInt64(dr["ItemID"]),
                                      PurchasedQty = Convert.ToInt32(dr["PurchasedQty"]),
                                      Purchased_Flood_seasonQty = Convert.ToInt32(dr["Purchased_Flood_seasonQty"]),
                                      CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                                      EPItemID = (dr["EPItemID"] == DBNull.Value) ? 0 : Convert.ToInt64(dr["EPItemID"]),
                                      ItemCategoryID = Convert.ToInt32(dr["ItemCategoryID"]),
                                      CurrentQty = Convert.ToInt32(dr["CurrentQty"])

                                  }).FirstOrDefault();
            return lstgetitems;

        }


        public DataSet GetFFPCampInfraStrucure(long? FFPID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_FFPCampsiteItemInfraStructure", FFPID);
        }

        public IEnumerable<DataRow> GetFFPGetStonePositionByID(long? _FloodFightingPlanID)
        {
            return db.ExecuteDataSet("Proc_FO_FFPGetStonePosition", _FloodFightingPlanID);
        }

        public DataSet GetSDAttache(long? FFPStonePositionID, long? SDID, string InfrastructureType,
            string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_SDAttachement", FFPStonePositionID, SDID, InfrastructureType,
                InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        }
        public DataSet GetSDAddHeader(long? FFPStonePositionID, string InfrastructureType,
            string InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID, int? Year)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_SDAddHeader", FFPStonePositionID, InfrastructureType,
                InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        }
        //////public DataSet GetSDAttachementHeaderInfo(long? FFPStonePositionID, long )
        //////{
        //////    return db.ExecuteStoredProcedureDataSet("Proc_FO_SDAttachmentHeaderInfo", FFPStonePositionID, SDID, InfrastructureType,
        //////        InfrastructureName, _ZoneID, _CircleID, _DivisionID, Year);
        //////}

        public DataSet GetFFPCampRDs(long? FFPID, long? StructID, long? StructTypeID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_FFPCampsiteCheck_irrigationRDs", FFPID, StructID,
                StructTypeID);
        }

        public IEnumerable<DataRow> GetFFPSearch(long? _FFPID, long? _ZoneID, long? _CircleID, long? _DivisionID,
            int? _FFPYear, string _Status)
        {
            return db.ExecuteDataSet("Proc_FO_FFPSearch", _FFPID, _ZoneID, _CircleID, _DivisionID, _FFPYear, _Status);
        }

        public dynamic GetFFPDetails(long? _FFPID, long? _ZoneID, long? _CircleID, long? _DivisionID, int? _FFPYear,
            string _Status)
        {
            return db.ExtRepositoryFor<FloodFightingPlanRepository>()
                .GetFFPDetails(_FFPID, _ZoneID, _CircleID, _DivisionID, _FFPYear, _Status);
        }
        public IEnumerable<DataRow> GetFFPArrangement(long _FFPID)
        {
            return db.ExecuteDataSet("Proc_FO_FFPArrangementTypeByFFPID", _FFPID);
        }
        public List<FO_FFPArrangements> GetAllArangements()
        {
            List<FO_FFPArrangements> lstArrangements = db.Repository<FO_FFPArrangements>().GetAll().ToList();
            return lstArrangements;
        }

        public List<FO_FFPArrangements> GetAllArangementsByFFPID(long _FFPID)
        {
            List<FO_FFPArrangements> lstArrangements =
                db.Repository<FO_FFPArrangements>().GetAll().Where(a => a.FFPID == _FFPID).ToList();
            return lstArrangements;
        }

        public List<FO_FFPArrangementType> GetAllArrangementType()
        {
            List<FO_FFPArrangementType> lstArrangementType = db.Repository<FO_FFPArrangementType>().GetAll().ToList();
            return lstArrangementType;
        }

        public bool AddArrangements(FO_FFPArrangements _Arrangements)
        {
            db.Repository<FO_FFPArrangements>().Insert(_Arrangements);
            db.Save();
            return true;
        }

        public bool UpdateArrangements(FO_FFPArrangements _Arrangements)
        {
            FO_FFPArrangements mdlArrangements = db.Repository<FO_FFPArrangements>().FindById(_Arrangements.ID);
            mdlArrangements.FFPID = _Arrangements.FFPID;
            mdlArrangements.FFPArrangementTypeID = _Arrangements.FFPArrangementTypeID;
            mdlArrangements.Description = _Arrangements.Description;
            mdlArrangements.ModifiedBy = _Arrangements.ModifiedBy;
            mdlArrangements.ModifiedDate = _Arrangements.ModifiedDate;

            db.Repository<FO_FFPArrangements>().Update(mdlArrangements);
            db.Save();
            return true;
        }

        public bool DeleteArrangements(long _Arrangements)
        {
            db.Repository<FO_FFPArrangements>().Delete(_Arrangements);
            db.Save();
            return true;
        }

        public FO_FloodFightingPlan GetFFPID(long _ID)
        {
            FO_FloodFightingPlan qFFPID =
                db.Repository<FO_FloodFightingPlan>().GetAll().Where(s => s.ID == _ID).FirstOrDefault();
            return qFFPID;
        }

        public bool CheckFFPStatusByID(long _FFPID)
        {
            FO_FloodFightingPlan ObjFFP = db.Repository<FO_FloodFightingPlan>().FindById(_FFPID);
            ObjFFP.Status = "Published";
            db.Repository<FO_FloodFightingPlan>().Update(ObjFFP);
            db.Save();
            return true;
        }

        public IEnumerable<DataRow> GetFO_FFPCampSitesByIDs(string _InfrastructureType, long? _FFPID)
        {
            return db.ExecuteDataSet("Proc_FO_FFPCampSites", _InfrastructureType, _FFPID);
        }

        public IEnumerable<DataRow> GetFO_GetOverallDivItems(long _DivisionID, int _Year, Int16 _CategID)
        {
            return db.ExecuteDataSet("Proc_FO_OverallDivItemsLoad", _DivisionID, _Year, _CategID);
        }

        public List<object> GetFO_SD_Attachment_ID(long SDID)
        {
            FloodFightingPlanRepository repFloodOperations = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodOperations.GetFO_SD_Attachment_ID(SDID);
        }

        public bool SaveSD_Attachment(FO_SDImages _SDAttachement)
        {
            bool isSaved = false;

            if (_SDAttachement.ID == 0)
            {
                db.Repository<FO_SDImages>().Insert(_SDAttachement);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_SDImages>().Update(_SDAttachement);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool DeleteFo_SD_Attachement(long _ID)
        {
            db.Repository<FO_SDImages>().Delete(_ID);
            db.Save();

            return true;
        }


        public List<FO_StoneDeployment> GetStoneDeploymentByStonePositionID(int _StonePositionID)
        {
            List<FO_StoneDeployment> lstStoneDeployment =
                db.Repository<FO_StoneDeployment>()
                    .GetAll()
                    .Where(x => x.FFPStonePositionID == _StonePositionID)
                    .ToList();
            return lstStoneDeployment;
        }


        public List<object> GetStoneDeploymentByStonePositionID(long _StonePositionID)
        {
            FloodFightingPlanRepository repFloodFighting = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodFighting.GetStoneDeploymentByStonePositionID(_StonePositionID);
        }







        public int FO_SDGetBalancedByFFPStonePositionID(int _FFPStonePositionID, int _StoneDeploymentID)
        {
            int DisposedQty = 0;
            int? newstonedepid = _StoneDeploymentID;

            if (newstonedepid == 0)
            {
                newstonedepid = null;
            }

            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_SDGetBalancedByFFPStonePositionID]", _FFPStonePositionID, newstonedepid);
            object lstgetitems = (from DataRow dr in dt.Rows
                                  select new

                                  {

                                      StoneDeploymentID = Convert.ToInt64(dr["StoneDeploymentID"]),
                                      DisposedQty = Convert.ToInt32(dr["DisposedQty"])
                                  }).FirstOrDefault();



            if (lstgetitems != null)
            {
                DisposedQty = Convert.ToInt32(lstgetitems.GetType().GetProperty("DisposedQty").GetValue(lstgetitems));

            }
            return DisposedQty;

        }


        public long AddStoneDeployment(int _StoneDeploymentID, int _StonePosID, string _VehicleNumber,
            string _BuiltyNumber, string _Quantity, string _Cost, List<string> lstNameofFiles, int _UserID, int? _Balance)
        {
            FO_StoneDeployment mdlStone_Deployment = new FO_StoneDeployment();
            if (_StoneDeploymentID != 0)
            {
                mdlStone_Deployment = db.Repository<FO_StoneDeployment>().FindById(_StoneDeploymentID);
                mdlStone_Deployment.CreatedBy = Convert.ToInt32(mdlStone_Deployment.CreatedBy);
                mdlStone_Deployment.CreatedDate = Convert.ToDateTime(mdlStone_Deployment.CreatedDate);

                mdlStone_Deployment.ModifiedBy = Convert.ToInt32(_UserID);
                mdlStone_Deployment.ModifiedDate = DateTime.Now;
            }
            else
            {
                mdlStone_Deployment.CreatedBy = Convert.ToInt32(_UserID);
                mdlStone_Deployment.ModifiedDate = DateTime.Now;
                mdlStone_Deployment.CreatedDate = DateTime.Now;

            }
            mdlStone_Deployment.FFPStonePositionID = _StonePosID;
            mdlStone_Deployment.DisposedDate = DateTime.Now;
            mdlStone_Deployment.VehicleNumber = _VehicleNumber;

            mdlStone_Deployment.BuiltyNo = _BuiltyNumber;

            mdlStone_Deployment.QtyOfStoneDisposed = Convert.ToInt32(_Quantity);

            mdlStone_Deployment.Cost = Convert.ToInt32(_Cost);

            mdlStone_Deployment.Balance = _Balance;

            if (SaveStone_Deployment(mdlStone_Deployment))
            {
                if (IsStoneDeploymentAttachmentExists(mdlStone_Deployment.ID))
                {
                    DeleteStoneDeploymentAttachmentBySDID(mdlStone_Deployment.ID);
                }
                for (int i = 0; i < lstNameofFiles.Count; ++i)
                {
                    AddStoneDeploymentAttachment(mdlStone_Deployment.ID, _UserID, lstNameofFiles[i]);
                }
                //link attachments to Material Disposal id if there any

            }




            return mdlStone_Deployment.ID;

        }

        public void AddStoneDeploymentAttachment(long _SDID, int _UserID, string _AttachmentPath)
        {
            FO_SDImages mdlSDAtchmnt = new FO_SDImages();

            mdlSDAtchmnt.SDID = _SDID;
            mdlSDAtchmnt.ImageName = _AttachmentPath;
            mdlSDAtchmnt.ImageURL = _AttachmentPath;
            mdlSDAtchmnt.CreatedBy = _UserID;
            mdlSDAtchmnt.CreatedDate = DateTime.Now;
            // mdlWPAtchmnt.ModifiedBy = Convert.ToInt32(_UserID);
            // mdlWPAtchmnt.ModifiedDate = DateTime.Now;

            db.Repository<FO_SDImages>().Insert(mdlSDAtchmnt);
            db.Save();
        }


        public bool SaveStone_Deployment(FO_StoneDeployment _Obj)
        {
            bool isSaved = false;

            if (_Obj.ID == 0)
            {
                db.Repository<FO_StoneDeployment>().Insert(_Obj);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_StoneDeployment>().Update(_Obj);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }



        public bool IsStoneDeploymentAttachmentExists(long _SDID)
        {
            bool qIsAttachmentExists = db.Repository<FO_SDImages>().GetAll().Any(s => s.SDID == _SDID);
            return qIsAttachmentExists;
        }

        public bool DeleteStoneDeploymentAttachmentBySDID(long _SDID)
        {
            FloodFightingPlanRepository repFloodFightingPlan = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodFightingPlan.DeleteStoneDeploymentAttachmentBySDID(_SDID);


        }

        public List<object> GetAttachmentStoneDeploymentByID(long SDID)
        {
            FloodFightingPlanRepository repFloodFightingPlan = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodFightingPlan.GetAttachmentStoneDeploymentByID(SDID);
        }


        public List<object> SearchStoneDeployment(long? _FFPStonePositionID, long? _StoneDeploymentID,
            string _InfrastructureType, string _InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID,
            int? _Yea, int _UserID)
        {
            long DesignationID = Convert.ToInt64(new LoginDAL().GetAndroidUserDesignationID(Convert.ToInt64(_UserID)));
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_SDSearchPage]", _FFPStonePositionID,
                _StoneDeploymentID, _InfrastructureType, _InfrastructureName, _ZoneID, _CircleID, _DivisionID, _Yea);
            List<object> lstsearchemergencypurchase = (from DataRow dr in dt.Rows
                                                       select new
                                                       {
                                                           FFPStonePositionID = Convert.ToInt32(dr["FFPStonePositionID"]),
                                                           //StoneDeploymentID =
                                                           //(dr["StoneDeploymentID"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["StoneDeploymentID"]),

                                                           InfrastructureType = dr["InfrastructureType"].ToString(),
                                                           InfrastructureName = dr["InfrastructureName"].ToString(),
                                                           RD = Convert.ToInt32(dr["RD"]),
                                                           RequiredQty = Convert.ToInt32(dr["RequiredQty"]),
                                                           QtyOfStoneDisposed =
                                                           (dr["QtyOfStoneDisposed"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["QtyOfStoneDisposed"]),
                                                           StoneDeploymentYear = Convert.ToInt32(dr["StoneDeploymentYear"]),

                                                           Division = dr["Division"].ToString(),

                                                           DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                           CanEdit = new FloodOperationsDAL().CanAddEditStoneDeployment(Convert.ToInt32(dr["StoneDeploymentYear"]), DesignationID)


                                                       }).ToList<object>();
            return lstsearchemergencypurchase;



        }


        public List<object> SearchEmergencyPurchase(String _InfrastructureType, long? _EmergencyPurchasesID,
            long? _ZoneID, long? CircleID, long? _DivisionID, long? _PurchasesYear, long? _CompSite)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[FO_SearchEmergencyPurchases]", _InfrastructureType,
                _EmergencyPurchasesID, _ZoneID, CircleID, _DivisionID, _PurchasesYear, _CompSite);
            List<object> lstsearchemergencypurchase = (from DataRow dr in dt.Rows
                                                       select new
                                                       {
                                                           EmergencyPurchaseID = Convert.ToInt32(dr["EmergencyPurchaseID"]),
                                                           InfrastructureType = dr["InfrastructureType"].ToString(),
                                                           InfrastructureName = dr["InfrastructureName"].ToString(),
                                                           CompSite = Convert.ToInt32(dr["CompSite"]),
                                                           DivisionID = Convert.ToInt32(dr["DivisionID"])

                                                       }).ToList<object>();
            return lstsearchemergencypurchase;

        }

        public bool AddZone(CO_Zone _Zone)
        {
            db.Repository<CO_Zone>().Insert(_Zone);
            db.Save();

            return true;
        }

        public DataSet GetBalancedByFFPStonePositionID(long? _FFPStonePositionID, long? _StoneDeploymentID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_SDGetBalancedByFFPStonePositionID", _FFPStonePositionID, _StoneDeploymentID);
        }

        public bool AddStoneDeployment(FO_StoneDeployment _StoneDeployment)
        {

            db.Repository<FO_StoneDeployment>().Insert(_StoneDeployment);
            db.Save();
            return true;
        }

        public bool UpdateStoneDeployment(FO_StoneDeployment _StoneDeployment)
        {
            FO_StoneDeployment mdlStoneDeployment = db.Repository<FO_StoneDeployment>().FindById(_StoneDeployment.ID);

            mdlStoneDeployment.FFPStonePositionID = _StoneDeployment.FFPStonePositionID;
            mdlStoneDeployment.DisposedDate = _StoneDeployment.DisposedDate;
            mdlStoneDeployment.VehicleNumber = _StoneDeployment.VehicleNumber;
            mdlStoneDeployment.BuiltyNo = _StoneDeployment.BuiltyNo;
            mdlStoneDeployment.QtyOfStoneDisposed = _StoneDeployment.QtyOfStoneDisposed;
            mdlStoneDeployment.Cost = _StoneDeployment.Cost;
            mdlStoneDeployment.ModifiedBy = _StoneDeployment.ModifiedBy;
            mdlStoneDeployment.ModifiedDate = _StoneDeployment.ModifiedDate;

            db.Repository<FO_StoneDeployment>().Update(mdlStoneDeployment);
            db.Save();

            return true;
        }



        public bool DeleteStoneDeployment(long _ID)
        {
            try
            {
                db.Repository<FO_StoneDeployment>().Delete(_ID);
                db.Save();
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        public bool IsStoneDeploymentIDExists(long _ID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<FO_SDImages>().GetAll().Any(s => s.SDID == _ID);
            return qIsExists;
        }

        public bool IsStoneDeploymentExists(long _ID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<FO_StoneDeployment>().GetAll().Any(s => s.ID == _ID);
            return qIsExists;
        }

        public IEnumerable<DataRow> GetSearchStoneDeployment(long? _FFPStonePositionID, long? _StoneDeploymentID,
            string _InfrastructureType, string _InfrastructureName, long? _ZoneID, long? _CircleID, long? _DivisionID,
            int? _Year)
        {
            return db.ExecuteDataSet("Proc_FO_SDSearchPage", _FFPStonePositionID, _StoneDeploymentID, _InfrastructureType,
                _InfrastructureName, _ZoneID, _CircleID, _DivisionID, _Year);
        }


        public int GetYearByStonePositioID(long _StonePositioID)
        {
            int Year =
                db.Repository<FO_FFPStonePosition>()
                    .GetAll()
                    .Where(x => x.ID == _StonePositioID)
                    .Select(a => a.FO_FloodFightingPlan.Year)
                    .FirstOrDefault();
            return Year;
        }

        public object GetStoneDeploymentByID(long _ID)
        {
            FloodFightingPlanRepository repFloodFightingPlan = this.db.ExtRepositoryFor<FloodFightingPlanRepository>();
            return repFloodFightingPlan.GetStoneDeploymentByID(_ID);
        }

        public bool DeleteFFPStonePosition(long _ID)
        {
            try
            {
                db.Repository<FO_FFPStonePosition>().Delete(_ID);
                db.Save();
                return true;
            }
            catch (Exception)
            {

                return false;
            }



        }

        public long OverallDivItemsInsertion(long _OverallDivItemID, int _Year, long? _DivisionID,
                                          long? _ItemCategoryID, long? _ItemSubcategoryID, long? _StructureTypeID, long? _StructureID, long? _PreMBStatusID,
                                          long? _FloodInspectionDetailID, int? _PostAvailableQty, long? _PostRequiredQty,
                                          long? _CS_CampSiteID, int? _CS_RequiredQty, int? _OD_AdditionalQty, int _CreatedBy, int _ModifiedBy, long _ODIID)
        {
            long _ODIIDOut = 0;

            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_FO_OverallDivItemsInsertion", _OverallDivItemID, _Year, _DivisionID,
                                          _ItemCategoryID, _ItemSubcategoryID, _StructureTypeID, _StructureID, _PreMBStatusID,
                                           _FloodInspectionDetailID, _PostAvailableQty, _PostRequiredQty,
                                           _CS_CampSiteID, _CS_RequiredQty, _OD_AdditionalQty, _CreatedBy, _ModifiedBy, _ODIID);


            //if (DS.Tables != null && DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow DR in DS.Tables[0].Rows)
            //    {
            //        _DSIDOut = Convert.ToInt64(DR["@pDSIDOut"].ToString());


            //    }
            //}

            return _ODIIDOut;
        }
        public long OverallDivItemsUpdation(int _Year, long _DivisionID, long _ItemCategoryID, long _ItemSubcategoryID, int _AdditionalQty, long _UserID)
        {
            long _ODIIDOut = 0;
            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_FO_OverallDivItemsUpdation", _Year, _DivisionID, _ItemCategoryID, _ItemSubcategoryID, _AdditionalQty, _UserID);
            return _ODIIDOut;
        }
        public bool IsFFPArrangementsExist(long _FFPID, Int16 _ArrangementType)
        {
            bool qIsExist =
                db.Repository<FO_FFPArrangements>()
                    .GetAll()
                    .Any(q => q.FFPID == _FFPID && q.FFPArrangementTypeID == _ArrangementType);
            return qIsExist;
        }
        public bool IsFFPArrangementsExistOnUpdate(long _FFPID, Int16 _ArrangementType, long _ArrangementID)
        {
            bool qIsExist =
                db.Repository<FO_FFPArrangements>()
                    .GetAll()
                    .Any(q => q.FFPID == _FFPID && q.FFPArrangementTypeID == _ArrangementType && q.ID != _ArrangementID);
            return qIsExist;
        }

        public UA_SystemParameters SystemParameterValue(string _PKey, string _PType)
        {
            return
                db.Repository<UA_SystemParameters>()
                    .GetAll()
                    .Where(a => a.ParameterKey == _PKey && a.ParameterType == _PType).FirstOrDefault();


        }

        public IEnumerable<DataRow> GetFFPLastYearRestorationWork(long? _FFPDivisionID, int _FFPYear)
        {
            return db.ExecuteDataSet("Proc_Fo_FFPLastYearRestorationWorks", _FFPDivisionID, _FFPYear);
        }
        #endregion

        #region Notification

        public FO_GetFloodFightingPlanNotifyData_Result GetFloodFightingPlanNotifyData(long _FloodFightingPlanID)
        {
            return db.ExtRepositoryFor<FloodFightingPlanRepository>().GetFloodFightingPlanNotifyData(_FloodFightingPlanID);
        }

        #endregion  Notification
    }
}
