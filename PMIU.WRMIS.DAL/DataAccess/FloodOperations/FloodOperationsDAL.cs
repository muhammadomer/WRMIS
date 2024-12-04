using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.FloodOperations;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace PMIU.WRMIS.DAL.DataAccess.FloodOperations
{
    public class FloodOperationsDAL : BaseDAL
    {
        #region "Division Summmary"

        /// <summary>
        /// This method return List of Division Summmary
        /// Created on 03-Oct-2016
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<object></returns>
        public List<object> GetDivisionSummaryList()
        {
            List<object> lstDivisionSummary = null;
            lstDivisionSummary = db.Repository<FO_DivisionSummary>().GetAll().ToList<FO_DivisionSummary>().ToList<object>();
            return lstDivisionSummary;
        }

        public List<object> GetDivisionSummaryBySearchCriteria(long _DivisionSummaryID
          , long _ZoneID
          , long _CircleID
          , long _DivisionID
          , long _Year
          , string _DivisionSummaryStatus)
        {
            FloodOperationsRepository repFloodOperationsRepository = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperationsRepository.GetDivisionSummaryBySearchCriteria(_DivisionSummaryID
            , _ZoneID
            , _CircleID
            , _DivisionID
            , _Year
            , _DivisionSummaryStatus);
        }

        /// <summary>
        /// This function retun Division Summary addition success along with message
        /// Created on 03-Oct-2016
        /// </summary>
        /// <param name="_DivisionSummary"></param>
        /// <returns>bool</returns>
        public bool SaveDivisionSummary(FO_DivisionSummary _DivisionSummary)
        {
            bool isSaved = false;

            if (_DivisionSummary.ID == 0)
            {
                db.Repository<FO_DivisionSummary>().Insert(_DivisionSummary);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_DivisionSummary>().Update(_DivisionSummary);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        /// <summary>
        /// This function deletes a Division Summary with the provided ID.
        /// Created on 03-Oct-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteDivisionSummary(long _ID)
        {
            db.Repository<FO_DivisionSummary>().Delete(_ID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function will validate if the combination of Division and Year has already been entered
        /// Created On 03-Oct-2016
        /// </summary>
        /// <param name="_DivisionSummary"></param>
        /// <returns>bool</returns>
        public bool IsDivisionSummaryAlreadyExists(FO_DivisionSummary _DivisionSummary)
        {
            bool qIsDivisionSummaryAlreadyExists = db.Repository<FO_DivisionSummary>().GetAll().Any(i => i.Year == _DivisionSummary.Year
                             && i.DivisionID == _DivisionSummary.DivisionID && (i.ID != _DivisionSummary.ID || _DivisionSummary.ID == 0));
            return qIsDivisionSummaryAlreadyExists;
        }

        /// <summary>
        /// This function checks in all related tables for given Division SummaryID.
        /// Created On 03-Oct-2016
        /// </summary>
        /// <param name="_DivisionSummaryID"></param>
        /// <returns>bool</returns>
        public bool IsDivisionSummaryIDExists(long _DivisionSummaryID)
        {
            long divisionSummaryID = 0;
            bool qIsExists = false;

            //itemCategoryID  = db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && d.Name == "Protection Structure").Single().ID;

            //qIsExists = db.Repository<FO_Items>().GetAll().Any(s => s.ItemCategoryID == _ItemCategoryID);

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

        public object GetDivisionSummaryDetailByID(long _DivisionSummaryID)
        {
            return db.ExtRepositoryFor<FloodOperationsRepository>().GetDivisionSummaryDetailByID(_DivisionSummaryID);
        }

        public IEnumerable<DataRow> GetDivisionSummayInfrastructure(long _DivisionID)
        {
            return db.ExecuteDataSet("FO_InfrastructureForDivisionSummary", _DivisionID);
        }

        public DataSet DivisionSummayInfrastructure(long _DivisionID)
        {
            return db.ExecuteStoredProcedureDataSet("FO_InfrastructureForDivisionSummary", _DivisionID);
        }

        public IEnumerable<DataRow> GetDivisionSummayStonePosition(long _DivisionID, int _Year)
        {
            return db.ExecuteDataSet("FO_DivisionSummary_GetStonePosition", _DivisionID, _Year);
        }

        public DataSet GetDivisionStoreItemsQty(long? DivisionSummaryID, long? Categoryid)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DivisionSummaryAvailableQtyByDivision", DivisionSummaryID, Categoryid);
        }

        public DataSet GetDivisionSummaryItemsQty(long? DivisionSummaryID, long? Categoryid, long _InfrastructureTypeID, long _InfrastructureID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_DivisionSummaryAvailableQty", DivisionSummaryID, Categoryid, _InfrastructureTypeID, _InfrastructureID);
        }

        public dynamic GetDivisionSummaryInfrastructure(long _DivisionID)
        {
            return db.ExtRepositoryFor<FloodOperationsRepository>().GetDivisionSummaryInfrastructure(_DivisionID);
        }

        public FO_DivisionSummary GetDivisionSummaryID(long _ID)
        {
            FO_DivisionSummary qDivisionSummary = db.Repository<FO_DivisionSummary>().GetAll().Where(s => s.ID == _ID).FirstOrDefault();
            return qDivisionSummary;
        }

        public bool CheckDivisionSummaryStatusByID(long _ID)
        {
            FO_DivisionSummary Obj = db.Repository<FO_DivisionSummary>().FindById(_ID);
            Obj.Status = "Published";
            db.Repository<FO_DivisionSummary>().Update(Obj);
            db.Save();
            return true;
        }

        #endregion "Division Summmary"

        #region "Flood Inspection"

        /// This function fetches Division based on the UserID and User Irrigation Level and Circle ID.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_CircleID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByUserIDAndCircleID(long _UserID, long _IrrigationLevelID, long? _CircleID)
        {
            return db.ExtRepositoryFor<FloodOperationsRepository>().GetDivisionsByUserIDAndCircleID(_UserID, _IrrigationLevelID, _CircleID);
        }

        /// This function fetches Circles based on the UserID and User Irrigation Level and Zone ID.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_ZoneID"></param>
        /// <returns>List<CO_Circle></returns>
        public List<CO_Circle> GetCircleByUserIDAndZoneID(long _UserID, long _IrrigationLevelID, long? _ZoneID)
        {
            return db.ExtRepositoryFor<FloodOperationsRepository>().GetCircleByUserIDAndZoneID(_UserID, _IrrigationLevelID, _ZoneID);
        }

        /// This function fetches Zones based on the UserID and User Irrigation Level.
        /// Created On 07-10-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_Zone></returns>
        public List<CO_Zone> GetZoneByUserID(long _UserID, long _IrrigationLevelID)
        {
            return db.ExtRepositoryFor<FloodOperationsRepository>().GetZoneByUserID(_UserID, _IrrigationLevelID);
        }

        /// <summary>
        /// This method return List of Flood Inspection Conditions by Group
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_Group"></param>
        /// <returns>List<FO_InspectionConditions></returns>
        public List<FO_InspectionConditions> GetFloodInspectionConditions(string _Group)
        {
            List<FO_InspectionConditions> lstInspectionConditionsByGroupID = null;
            if (DBNull.Value.Equals(_Group) || _Group == null)
            {
                lstInspectionConditionsByGroupID = db.Repository<FO_InspectionConditions>().GetAll().ToList();
            }
            else
            {
                lstInspectionConditionsByGroupID = db.Repository<FO_InspectionConditions>().GetAll().Where(d => (d.CoditionGroup).ToUpper() == _Group.ToUpper()).ToList();
            }

            return lstInspectionConditionsByGroupID;
        }

        /// <summary>
        /// This method return List of Flood Inspection Types
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_Group"></param>
        /// <returns>List<object></returns>
        public List<object> GetAllFloodInspectionTypes()
        {
            List<object> lstInspectionTypes = null;

            lstInspectionTypes = db.Repository<FO_InspectionType>().GetAll().Where(d => d.IsActive == true).Select(x => new { x.ID, x.Name, x.Description, x.IsActive }).ToList<object>();

            return lstInspectionTypes;
        }

        /// <summary>
        /// This method return List of Flood Inspection Status
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_Group"></param>
        /// <returns>List<object></returns>
        public List<object> GetAllFloodInspectionStatus()
        {
            List<object> lstInspectionStatus = null;

            lstInspectionStatus = db.Repository<FO_InspectionStatus>().GetAll().Where(d => d.IsActive == true).Select(x => new { x.ID, x.Name, x.Description, x.IsActive }).ToList<object>();

            return lstInspectionStatus;
        }

        /// <summary>
        /// This method return List of InfraStructure Types
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_Group"></param>
        /// <returns>List<CO_StructureType></returns>
        public List<object> GetAllStructureTypes()
        {
            List<object> lstStructureType = db.Repository<CO_StructureType>().GetAll().Where(s => s.ID != (int)Constants.StructureType.Channel).Select(x => new { x.ID, x.Name, x.Description, x.DomainID, x.Source, x.IsActive }).ToList<object>();

            return lstStructureType;
        }

        /// <summary>
        /// This method return List of InfraStructure Types
        /// Created On 06-10-2016.
        /// </summary>
        /// <param name="_Group"></param>
        /// <returns>List<CO_StructureType></returns>
        public List<object> GetAllProtectionInfrastructures()
        {
            List<object> lstStructureType = db.Repository<CO_StructureType>().GetAll().Where(s => s.ID != (int)Constants.StructureType.Channel).Select(x => new { x.ID, x.Name, x.Description, x.DomainID, x.Source, x.IsActive }).ToList<object>();

            return lstStructureType;
        }

        /// <summary>
        /// This function return List of all active Inspection Type
        /// </summary>
        /// <returns></returns>
        /// <created>10/07/2016</created>
        /// <changed>10/07/2016</changed>
        public List<FO_InspectionType> GetAllActiveInspectionType()
        {
            return db.Repository<FO_InspectionType>().GetAll().Where(d => d.IsActive == true).ToList();
        }

        public List<FO_InspectionType> GetPrePostInspectionByID(Int16 InspectionID)
        {
            return db.Repository<FO_InspectionType>().GetAll().Where(d => d.IsActive == true && d.ID == InspectionID).ToList();
        }

        /// <summary>
        /// This function return List of all active Inspection Status
        /// </summary>
        /// <returns></returns>
        /// <created>10/07/2016</created>
        /// <changed>10/07/2016</changed>
        public List<FO_InspectionStatus> GetAllActiveInspectionStatus()
        {
            return db.Repository<FO_InspectionStatus>().GetAll().Where(d => d.IsActive == true).ToList();
        }

        public List<CO_StructureType> GetInfrastructureTypeName(String InfrastructureName)
        {
            return db.Repository<CO_StructureType>().GetAll().Where(d => d.IsActive == true && d.DomainID == 1 && d.Source.Equals(InfrastructureName)).ToList();
        }

        public List<object> GetProtectionInfrastructureName(long UserId, long InfrastructureType)
        {
            FloodOperationsRepository repFloodOperationsRepository = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperationsRepository.GetProtectionInfrastructureName(UserId, InfrastructureType);
        }

        public List<object> GetInfrastructureName(long _UserId, long _InfrastructureType)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_FO_GetInfrastructureByUserID", _UserId, _InfrastructureType);
            List<object> lstInfrastructureName = (from DataRow dr in dt.Rows
                                                  select new
                                                  {
                                                      ID = Convert.ToInt64(dr["ID"]),
                                                      NAME = Convert.ToString(dr["NAME"]),
                                                      InfrastructureTypeID = Convert.ToInt64(dr["InfrastructureTypeID"]),
                                                  }).ToList<object>();
            return lstInfrastructureName;
        }

        public bool IsFloodInspectionDataAlreadyExists(FO_FloodInspection _FloodInspection, FO_FloodInspectionDetail FloodInspectionDetail)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("FO_IsFloodInspectionExistsWRTYear", _FloodInspection.DivisionID, _FloodInspection.Year, _FloodInspection.InspectionDate, FloodInspectionDetail.InspectionTypeID, FloodInspectionDetail.StructureTypeID, FloodInspectionDetail.StructureID, _FloodInspection.ID);
            bool IsExists = false;
            foreach (DataRow DR in dt.Rows)
            {
                IsExists = Convert.ToBoolean(DR["IsExists"].ToString());
            }

            return IsExists;
            //bool qIsFloodInspectionDataAlreadyExists = db.Repository<FO_FloodInspection>().GetAll().Any(i => i.InspectionCategoryID == _FloodInspection.InspectionCategoryID
            //                 && i.DivisionID == _FloodInspection.DivisionID && (i.ID != _FloodInspection.ID || _FloodInspection.ID == 0));
            //return qIsFloodInspectionDataAlreadyExists;
        }

        public bool IsFloodInspectionDataAlreadyExists(long _DivisioID, string _Year, string _InspectionDate, long _InspectionTypeID, long _StructureTypeID, long _StructureID, long _floodInspectionID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("FO_IsFloodInspectionExistsWRTYear", _DivisioID, _Year, _InspectionDate, _InspectionTypeID, _StructureTypeID, _StructureID, _floodInspectionID);
            bool IsExists = false;
            foreach (DataRow DR in dt.Rows)
            {
                IsExists = Convert.ToBoolean(DR["IsExists"].ToString());
            }

            return IsExists;
            //bool qIsFloodInspectionDataAlreadyExists = db.Repository<FO_FloodInspection>().GetAll().Any(i => i.InspectionCategoryID == _FloodInspection.InspectionCategoryID
            //                 && i.DivisionID == _FloodInspection.DivisionID && (i.ID != _FloodInspection.ID || _FloodInspection.ID == 0));
            //return qIsFloodInspectionDataAlreadyExists;
        }

        public bool DeleteInspectionByFloodInspectionID(long _FloodInspection)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_DeleteFloodInspection]", _FloodInspection);
            bool IsExists = false;
            foreach (DataRow DR in dt.Rows)
            {
                IsExists = Convert.ToBoolean(DR["IsExists"].ToString());
            }
            return IsExists;
        }

        public bool SaveFloodInspection(FO_FloodInspection _FloodInspection)
        {
            bool isSaved = false;

            if (_FloodInspection.ID == 0)
            {
                db.Repository<FO_FloodInspection>().Insert(_FloodInspection);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_FloodInspection>().Update(_FloodInspection);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool SaveFloodInspectionDetail(FO_FloodInspectionDetail _FloodInspectionDetail)
        {
            bool isSaved = false;

            if (_FloodInspectionDetail.ID == 0)
            {
                db.Repository<FO_FloodInspectionDetail>().Insert(_FloodInspectionDetail);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_FloodInspectionDetail>().Update(_FloodInspectionDetail);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool SavaFloodInspection(long _FloodInspectionID, long _DivisionID, short? _CategoryID, short _StatusID, short _TypeID, long _StructureID, long _StructureTypeID, int _UserID)
        {
            bool Saved = false;

            try
            {
                FO_FloodInspection floodInspection = new FO_FloodInspection();

                if (_FloodInspectionID == 0)
                {
                    floodInspection.CreatedDate = DateTime.Now;
                    floodInspection.CreatedBy = Convert.ToInt32(_UserID);
                }
                else
                {
                    floodInspection = GetFloodInspectionByID(_FloodInspectionID);
                    floodInspection.ID = floodInspection.ID;
                    floodInspection.CreatedDate = Convert.ToDateTime(floodInspection.CreatedDate);
                    floodInspection.ModifiedDate = DateTime.Now;
                    floodInspection.CreatedBy = Convert.ToInt32(floodInspection.CreatedBy);
                    floodInspection.ModifiedBy = Convert.ToInt32(_UserID);
                }
                floodInspection.DivisionID = _DivisionID;
                floodInspection.InspectionCategoryID = _CategoryID;
                floodInspection.InspectionStatusID = _StatusID;
                floodInspection.InspectionDate = DateTime.Now;
                floodInspection.Year = DateTime.Now.Year.ToString();

                if (SaveFloodInspection(floodInspection))
                {
                    FO_FloodInspectionDetail floodInspectionDetail = new FO_FloodInspectionDetail();

                    if (_FloodInspectionID != 0)
                    {
                        floodInspectionDetail = IsFloodInspectionDetailaAlreadyExists(floodInspection.ID);
                        floodInspectionDetail.ID = floodInspectionDetail.ID;
                        floodInspectionDetail.CreatedDate = Convert.ToDateTime(floodInspectionDetail.CreatedDate);
                        floodInspectionDetail.ModifiedDate = DateTime.Now;
                        floodInspectionDetail.CreatedBy = Convert.ToInt32(_UserID);
                        floodInspectionDetail.ModifiedBy = Convert.ToInt32(floodInspectionDetail.ModifiedBy);
                    }
                    else
                    {
                        floodInspectionDetail.CreatedDate = DateTime.Now;
                        floodInspectionDetail.CreatedBy = Convert.ToInt32(_UserID);
                    }

                    floodInspectionDetail.FloodInspectionID = floodInspection.ID;
                    floodInspectionDetail.InspectionTypeID = _TypeID;
                    floodInspectionDetail.StructureID = _StructureID;
                    floodInspectionDetail.StructureTypeID = _StructureTypeID;
                    Saved = SaveFloodInspectionDetail(floodInspectionDetail);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Saved;
        }

        /// This function fetches All Active RD Wise Types.
        /// Created On 14-10-2016
        /// </summary>
        /// <returns>List<object></returns>
        public List<object> GetAllActiveRDWiseTypes()
        {
            return db.Repository<FO_RDWiseType>().GetAll().Where(d => d.IsActive == true).Select(x => new { x.ID, x.Name, x.Description, x.IsActive }).ToList<object>();
        }

        /// This function fetches All Active Encroachment Types.
        /// Created On 14-10-2016
        /// </summary>
        /// <returns>List<object></returns>
        public List<object> GetAllEncroachmentTypes()
        {
            return db.Repository<FO_EncroachmentType>().GetAll().Where(d => d.IsActive == true).Select(x => new { x.ID, x.Name, x.Description, x.IsActive }).ToList<object>();
        }

        public bool IsFloodInspectionDependencyExists(long _FloodInspectionID)
        {
            // long ControlInfrastructureTypeID = 0;

            // ControlInfrastructureTypeID = db.Repository<CO_StructureType>().GetAll().Where(d => d.DomainID == 1 && d.IsActive == true && d.Source.Equals("Control Structure")).FirstOrDefault().ID;

            //bool qIsExists = db.Repository<CO_Station>().GetAll().Any(s => s.ID == _ControlInfrastructureID);
            //if (!qIsExists)
            //{
            bool qIsExists = db.Repository<FO_FloodInspectionDetail>().GetAll().Any(s => s.ID == _FloodInspectionID);
            //}
            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_IGCDrain>().GetAll().Any(s => s.ID == _FloodInspectionID);
            }
            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_IGCBarrageHWGates>().GetAll().Any(s => s.ID == _FloodInspectionID);
            }

            //if (!qIsExists)
            //{
            //    qIsExists = db.Repository<FO_StructureAdminBoundaries>().GetAll().Any(s => s.StructureTypeID == ControlInfrastructureTypeID && s.StructureID == _ControlInfrastructureID);
            //}
            return qIsExists;
        }

        public bool DeleteFloodInspection(long _ID)
        {
            //db.Repository<FO_FloodInspectionDetail>().Delete(GetInspectionDetailID(_ID));
            //db.Save();

            db.Repository<FO_FloodInspection>().Delete(_ID);
            db.Save();

            return true;
        }

        public long GetInspectionDetailID(long _FloodInspectionID)
        {
            long _FloodInspectionDetailID = 0;
            _FloodInspectionDetailID = db.Repository<FO_FloodInspectionDetail>().GetAll().Where(f => f.FloodInspectionID == _FloodInspectionID).FirstOrDefault().ID;
            return _FloodInspectionDetailID;
        }

        /// <summary>
        /// This method return List of InfraStructure Details by Infrasturcture Type
        /// Created On 17-10-2016.
        /// </summary>
        /// <returns> List<object></returns>
        ///
        public List<object> GetUserFloodInspections(string _UserID, string _InspectionID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[FO_GetFloodInspections]", _UserID, _InspectionID);
            long DesignationID = Convert.ToInt64(new LoginDAL().GetAndroidUserDesignationID(Convert.ToInt64(_UserID)));
            List<object> lstInfrastructureDetails = (from DataRow dr in dt.Rows
                                                     select new
                                                     {
                                                         InspectionID = Convert.ToInt32(dr["InspectionID"]),
                                                         StructureID = dr["StructureID"].ToString(),
                                                         StructureName = dr["StructureName"].ToString(),
                                                         InspectionDate = dr["InspectionDate"].ToString(),
                                                         InspectionStatus = dr["InspectionStatus"].ToString(),
                                                         DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                         InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                         InspectionStatusID = Convert.ToInt32(dr["InspectionStatusID"]),
                                                         InsfrastructureType = dr["InsfrastructureType"].ToString(),
                                                         InsfrastructureTypeID = dr["InsfrastructureTypeID"].ToString(),
                                                         Year = dr["Year"].ToString(),
                                                         CanEdit = CanAddEditFloodInspections(Convert.ToInt32(dr["Year"]), DesignationID, Convert.ToInt32(dr["InspectionStatusID"]), Convert.ToInt32(dr["InspectionTypeID"]))
                                                     }).ToList<object>();
            return lstInfrastructureDetails;
        }

        public object GetUserFloodInspectionsObject(string _UserID, string _InspectionID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[FO_GetFloodInspections]", _UserID, _InspectionID);
            object lstInfrastructureDetails = (from DataRow dr in dt.Rows
                                               select new
                                               {
                                                   InspectionID = Convert.ToInt32(dr["InspectionID"]),
                                                   StructureID = dr["StructureID"].ToString(),
                                                   StructureName = dr["StructureName"].ToString(),
                                                   InspectionDate = dr["InspectionDate"].ToString(),
                                                   InspectionStatus = dr["InspectionStatus"].ToString(),
                                                   DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                   InspectionTypeID = Convert.ToInt32(dr["InspectionTypeID"]),
                                                   InsfrastructureType = dr["InsfrastructureType"].ToString(),
                                                   InsfrastructureTypeID = dr["InsfrastructureTypeID"].ToString()
                                               }).ToList<object>().LastOrDefault();
            return lstInfrastructureDetails;
        }

        /// This function fetches All Active RD Wise Types.
        /// Created On 17-10-2016
        /// </summary>
        /// <returns>List<FO_StonePitchSide></returns>
        public List<object> GetAllActiveStonePitchSides()
        {
            return db.Repository<FO_StonePitchSide>().GetAll().Where(d => d.IsActive == true).Select(x => new { x.ID, x.Name, x.Description, x.IsActive }).ToList<object>();
        }

        /// <summary>
        /// This method return List of RD Wise Inspection by RDWiseTypeID
        /// Created On 18-10-2016.
        /// </summary>
        /// <returns> List<object></returns>
        public List<object> GetIRDWiseConditionsByRDType(string _FloodInspectionID, string _RDWiseTypeID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[FO_GetIRDWiseConditions]", _RDWiseTypeID, _FloodInspectionID);
            List<object> lstRDWiseConditionDetails = (from DataRow dr in dt.Rows
                                                      select new
                                                      {
                                                          ID = Convert.ToInt32(dr["ID"]),
                                                          FloodInspectionID = Convert.ToInt32(dr["FloodInspectionID"]),
                                                          FromRD = Convert.ToInt32(dr["FromRD"]),
                                                          ToRD = Convert.ToInt32(dr["ToRD"]),
                                                          ConditionID = Convert.ToInt32(dr["ConditionID"]),
                                                          EncroachmentTypeID = Convert.ToInt32(dr["EncroachmentTypeID"]),
                                                          EncroachmentName = dr["EncroachmentName"].ToString(),
                                                          StonePitchSideID = Convert.ToInt32(dr["StonePitchSideID"]),
                                                          PitchSideName = dr["PitchSideName"].ToString()
                                                      }).ToList<object>();
            return lstRDWiseConditionDetails;
        }

        /// <summary>
        /// This method return List of InfraStructure Details by Infrasturcture Type
        /// Created On 17-10-2016.
        /// </summary>
        /// <param name="_InfrastructureType"></param>
        /// <returns>List<object></returns>
        public List<object> GetInfrastructureDetailsByType(string _InfrastructureType)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[FO_GetInfrastructureNamesByType]", _InfrastructureType);
            List<object> lstInfrastructureDetails = (from DataRow dr in dt.Rows
                                                     select new
                                                     {
                                                         ID = Convert.ToInt32(dr["ID"]),
                                                         InfrastructureName = dr["InfrastructureName"].ToString(),
                                                         Description = dr["Description"].ToString(),
                                                         InfrastructureTypeID = dr["InfrastructureTypeID"].ToString()
                                                     }).ToList<object>();
            return lstInfrastructureDetails;
        }

        public DataSet FO_irrigationRDs(long? DivisionID, long? StructID, long? StructTypeID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_Check_irrigationRDs", DivisionID, StructID, StructTypeID);
        }

        public bool IsFloodInspectionParentExists(long _FloodInspectionID)
        {
            bool qIsExists = db.Repository<FO_FloodInspection>().GetAll().Any(s => s.ID == _FloodInspectionID);
            return qIsExists;
        }

        public object GetInfrastructureTypeByID(long _FloodInspectionID)
        {
            return db.ExtRepositoryFor<FloodOperationsRepository>().GetInfrastructureTypeByID(_FloodInspectionID);
        }

        public FO_GetFloodInspectionsDetailByID_Result2 GetFloodInspectionsDetail(string _InfrastructureType, long _InspectionID)
        {
            FO_GetFloodInspectionsDetailByID_Result2 lstInspectionTypes = new FO_GetFloodInspectionsDetailByID_Result2();
            lstInspectionTypes = db.ExtRepositoryFor<FloodOperationsRepository>().GetFloodInspectionsDetail(_InfrastructureType, _InspectionID);
            return lstInspectionTypes;
        }

        public object GetStructureTypeIDByInsfrastructureValue(long _InfrastructureType, long _InfrastructureNamevalue)
        {
            FloodOperationsRepository repFloodOperationsRepository = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperationsRepository.GetStructureTypeIDByInsfrastructureValue(_InfrastructureType, _InfrastructureNamevalue);
        }

        public FO_FloodInspectionDetail IsFloodInspectionDetailaAlreadyExists(long _FloodInspectionID)
        {
            return db.Repository<FO_FloodInspectionDetail>().GetAll().Where(s => s.FloodInspectionID == _FloodInspectionID).FirstOrDefault();
        }

        public FO_FloodInspection GetFloodInspectionByID(long _FloodInspectionID)
        {
            return db.Repository<FO_FloodInspection>().GetAll().Where(s => s.ID == _FloodInspectionID).FirstOrDefault();
        }

        /// <summary>
        /// This method return List of Barrage Gates Detail By StationID
        /// Created On 17-10-2016.
        /// <_param>_StationID</_param>
        /// </summary>
        /// <returns> List<object></returns>
        public List<object> GetBarrageGatesDetailByStationID(string _StationID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[FO_GetBarrageGatesDetailByStationID]", _StationID);
            List<object> lstBarrageGatesDetail = (from DataRow dr in dt.Rows
                                                  select new
                                                  {
                                                      GateTypeID = Convert.ToInt32(dr["GateTypeID"]),
                                                      GateTypeName = dr["GateTypeName"].ToString(),
                                                      StationID = Convert.ToInt32(dr["StationID"]),
                                                      TotalGates = Convert.ToInt32(dr["TotalGates"])
                                                  }).ToList<object>();
            return lstBarrageGatesDetail;
        }

        public IEnumerable<DataRow> GetFloodInspectionSearch(string _InfrastructureType, long? _FloodInspectionID, long? _ZoneID, long? _CircleID, long? _DivisionID,
                long? _InspectionType, long? _Status, string _InfrastructureName, DateTime? _FromDate, DateTime? _ToDate, long? _UserID)
        {
            return db.ExecuteDataSet("FO_SearchIndependentInspection", _InfrastructureType, _FloodInspectionID, _ZoneID, _CircleID, _DivisionID,
               _InspectionType, _Status, _InfrastructureName, _FromDate, _ToDate, _UserID);
        }

        #region DepartmentalInspection

        public bool IsDepartmentalInspectionDependencyExists(long _FloodInspectionID)
        {
            bool dAttachments = db.Repository<FO_DAttachments>().GetAll().Any(s => s.ID == _FloodInspectionID);
            bool dMemberDetails = db.Repository<FO_DMemberDetails>().GetAll().Any(s => s.ID == _FloodInspectionID);
            bool dInfrastructures = db.Repository<FO_DInfrastructures>().GetAll().Any(s => s.ID == _FloodInspectionID);

            if (dAttachments == true || dMemberDetails == true || dInfrastructures == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion DepartmentalInspection

        #endregion "Flood Inspection"

        #region Flood Departmental

        public object GetDepartmentalByID(long _FloodInspectionID)
        {
            return db.ExtRepositoryFor<FloodOperationsRepository>().GetDepartmentalByID(_FloodInspectionID);
        }

        #endregion Flood Departmental

        #region Emergency Purchases

        public bool IsEmergencyPurchasesParentExists(long _EmergencyPurchasesID)
        {
            bool qIsExists = db.Repository<FO_EmergencyPurchase>().GetAll().Any(s => s.ID == _EmergencyPurchasesID);
            return qIsExists;
        }

        public bool SaveEmergencyPurchases(FO_EmergencyPurchase _EmergencyPurchases)
        {
            bool isSaved = false;

            if (_EmergencyPurchases.ID == 0)
            {
                db.Repository<FO_EmergencyPurchase>().Insert(_EmergencyPurchases);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_EmergencyPurchase>().Update(_EmergencyPurchases);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public long SaveEmergencyPurchase(long _PurchaseID, long _DivisionID, long? _StructureTypeID, long _StructureID, bool? IsCampSite, string _RDLeft, string _RDRight, long _UserID)
        {
            FO_EmergencyPurchase ObjEmergencyPurchase = new FO_EmergencyPurchase();

            if (_PurchaseID == 0)
            {
                ObjEmergencyPurchase.CreatedDate = DateTime.Now;
                ObjEmergencyPurchase.CreatedBy = Convert.ToInt32(_UserID);
            }
            else
            {
                ObjEmergencyPurchase = db.Repository<FO_EmergencyPurchase>().FindById(_PurchaseID);
                ObjEmergencyPurchase.CreatedDate = Convert.ToDateTime(ObjEmergencyPurchase.CreatedDate);
                ObjEmergencyPurchase.ModifiedDate = DateTime.Now;
                ObjEmergencyPurchase.CreatedBy = Convert.ToInt32(ObjEmergencyPurchase.CreatedBy);
                ObjEmergencyPurchase.ModifiedBy = Convert.ToInt32(_UserID);
            }

            ObjEmergencyPurchase.DivisionID = _DivisionID;
            ObjEmergencyPurchase.Year = Convert.ToInt16(DateTime.Now.Year);
            ObjEmergencyPurchase.StructureTypeID = _StructureTypeID;
            ObjEmergencyPurchase.StructureID = _StructureID;
            ObjEmergencyPurchase.IsCampSite = IsCampSite;

            if (_RDLeft == "null" || _RDLeft == "null")
            {
                ObjEmergencyPurchase.RD = null;
            }
            else
            {
                ObjEmergencyPurchase.RD = Calculations.CalculateTotalRDs(_RDLeft, _RDRight);
            }

            bool isSaved = SaveEmergencyPurchases(ObjEmergencyPurchase);

            return ObjEmergencyPurchase.ID;
        }

        public bool DeleteEmergencyPurchase(long _ID)
        {
            db.Repository<FO_EmergencyPurchase>().Delete(_ID);
            db.Save();

            return true;
        }

        public List<FO_EmergencyPurchase> GetAllEmergencyPurchasesYear()
        {
            return db.Repository<FO_EmergencyPurchase>().GetAll().Distinct().ToList<FO_EmergencyPurchase>();
        }

        public List<object> SearchEmergencyPurchase(String _InfrastructureType, long? _EmergencyPurchasesID, long? _ZoneID, long? CircleID, long? _DivisionID, long? _PurchasesYear, long? _CompSite, int _UserID)
        {
            long DesignationID = Convert.ToInt64(new LoginDAL().GetAndroidUserDesignationID(Convert.ToInt64(_UserID)));
            DataTable dt = db.ExecuteStoredProcedureDataTable("[FO_SearchEmergencyPurchases]", _InfrastructureType, _EmergencyPurchasesID, _ZoneID, CircleID, _DivisionID, _PurchasesYear, _CompSite);
            List<object> lstsearchemergencypurchase = (from DataRow dr in dt.Rows
                                                       select new
                                                       {
                                                           EmergencyPurchaseID = Convert.ToInt32(dr["EmergencyPurchaseID"]),
                                                           InfrastructureType = dr["InfrastructureType"].ToString(),
                                                           InfrastructureName = dr["InfrastructureName"].ToString(),
                                                           CompSite = Convert.ToInt32(dr["CompSite"]),
                                                           DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                           RD = (dr["RD"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["RD"]),
                                                           EmergencyCreatedDate = dr["Year"].ToString(),
                                                           CanEdit = new FloodOperationsDAL().CanAddEditEmergencyPurchase(Convert.ToInt32(dr["Year"].ToString()), DesignationID)
                                                       }).ToList<object>();
            return lstsearchemergencypurchase;
        }

        public CO_StructureType GetStructureInformationByEmergencyPurchaseID(long? _EmergencyPurchaseID)
        {
            FloodOperationsRepository repFloodOperationsRepository = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperationsRepository.GetStructureInformationByEmergencyPurchaseID(_EmergencyPurchaseID);
        }

        public IEnumerable<DataRow> GetEmergencyPurchasesSearch(string _InfrastructureType, long? _EmergencyPurchaseID, long? _ZoneID, long? _CircleID, long? _DivisionID,
               int? _PurchaseYear, bool? _IsCompSite)
        {
            return db.ExecuteDataSet("FO_SearchEmergencyPurchases", _InfrastructureType, _EmergencyPurchaseID, _ZoneID, _CircleID, _DivisionID,
               _PurchaseYear, _IsCompSite);
        }

        public bool IsEmergencyPurchaseDependencyExists(long _EmergencyPurchaseID)
        {
            bool qIsExists = db.Repository<FO_EPWork>().GetAll().Any(s => s.EmergencyPurchaseID == _EmergencyPurchaseID);
            return qIsExists;
        }

        public bool DeleteJointEmergencyPurchase(long _EmergencyPurchaseID)
        {
            db.Repository<FO_EmergencyPurchase>().Delete(_EmergencyPurchaseID);
            db.Save();

            return true;
        }

        public FO_EmergencyPurchase GetEmergencyPurchaseByID(long _ID)
        {
            FO_EmergencyPurchase qEmergencyPurchase = db.Repository<FO_EmergencyPurchase>().GetAll().Where(s => s.ID == _ID).FirstOrDefault();
            return qEmergencyPurchase;
        }

        public bool IsEmergencyPurchasesExist(FO_EmergencyPurchase ObjEmergencyPurchase)
        {
            bool qIsEmergencyPurchasesExist = db.Repository<FO_EmergencyPurchase>().GetAll().Any(q => q.RD == ObjEmergencyPurchase.RD && q.DivisionID == ObjEmergencyPurchase.DivisionID && q.StructureID == ObjEmergencyPurchase.StructureID && q.StructureTypeID == ObjEmergencyPurchase.StructureTypeID && q.Year == ObjEmergencyPurchase.Year && (q.ID != ObjEmergencyPurchase.ID || ObjEmergencyPurchase.ID == 0));
            return qIsEmergencyPurchasesExist;
        }

        public object GetInfrastructureTypeByEmergencyPurchaseID(long _EmergencyPurchaseID)
        {
            FloodOperationsRepository repFloodOperationsRepository = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperationsRepository.GetInfrastructureTypeByEmergencyPurchaseID(_EmergencyPurchaseID);
        }

        public bool SaveFloodFightingWorks(FO_EPWork _FloodFighting)
        {
            bool isSaved = false;

            if (_FloodFighting.ID == 0)
            {
                db.Repository<FO_EPWork>().Insert(_FloodFighting);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_EPWork>().Update(_FloodFighting);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public long SaveFloodFightingWorks(long UserID, long _WorkID, long _EmergencyID, long? _NatureOfWorkID, int? _RD, string _Descp)
        {
            FO_EPWork mdlFO_EPWork = new FO_EPWork();
            if (_WorkID != 0)
            {
                mdlFO_EPWork = db.Repository<FO_EPWork>().FindById(_WorkID);
                mdlFO_EPWork.CreatedBy = Convert.ToInt32(mdlFO_EPWork.CreatedBy);
                mdlFO_EPWork.CreatedDate = Convert.ToDateTime(mdlFO_EPWork.CreatedDate);

                mdlFO_EPWork.ModifiedBy = Convert.ToInt32(UserID);
                mdlFO_EPWork.ModifiedDate = DateTime.Now;
            }
            else
            {
                mdlFO_EPWork.CreatedBy = Convert.ToInt32(UserID);
                mdlFO_EPWork.ModifiedDate = DateTime.Now;
                mdlFO_EPWork.CreatedDate = DateTime.Now;
            }
            mdlFO_EPWork.EmergencyPurchaseID = _EmergencyID;
            //mdlFO_EPWork.DisposalDate = DateTime.Now;
            mdlFO_EPWork.NatureOfWorkID = _NatureOfWorkID;

            mdlFO_EPWork.RD = _RD;

            mdlFO_EPWork.Description = _Descp;

            SaveFloodFightingWorks(mdlFO_EPWork);

            return mdlFO_EPWork.ID;
        }

        public int Get_EP_Item_PreviousQuantity(FO_EPItem _FloodFighting)
        {
            int? purchasedQty = 0;
            try
            {
                purchasedQty = db.Repository<FO_EPItem>().GetAll().Where(f => f.ID == _FloodFighting.ID).FirstOrDefault().PurchasedQty;
            }
            catch (Exception)
            { }
            return purchasedQty == null ? 0 : Convert.ToInt32(purchasedQty);
        }

        public int Get_EP_Item_PreviousQuantity(long _EPItemID)
        {
            int? purchasedQty = 0;

            try
            {
                purchasedQty = db.Repository<FO_EPItem>().GetAll().Where(f => f.ID == _EPItemID).FirstOrDefault().PurchasedQty;
            }
            catch (Exception)
            { }

            return purchasedQty == null ? 0 : Convert.ToInt32(purchasedQty);
        }

        public bool SaveFo_EP_Item(FO_EPItem _FloodFighting)
        {
            bool isSaved = false;

            _FloodFighting.PurchasedQty = _FloodFighting.PurchasedQty;

            if (_FloodFighting.ID == 0)
            {
                db.Repository<FO_EPItem>().Insert(_FloodFighting);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_EPItem>().Update(_FloodFighting);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public object GetFo_EmergencyPurchase_ID(long EmergencyPurchaseID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetFo_EmergencyPurchase_ID(EmergencyPurchaseID);
        }

        public List<object> GetFloodFightingWorksByID(int EmergencyPurchaseID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetFloodFightingWorksByID(EmergencyPurchaseID);
        }

        public object GetFo_EPWorkByObject_ID(long EmergencyPurchaseID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetFo_EPWorkByObject_ID(EmergencyPurchaseID);
        }

        public List<object> GetF_EmergencyDisposal_Attachment_ID(long MaterialDisposalID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetF_EmergencyDisposal_Attachment_ID(MaterialDisposalID);
        }

        public bool DeleteFo_EPWork(long _ID)
        {
            db.Repository<FO_EPWork>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsFo_EpworkIDExists(long _ID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<FO_MaterialDisposal>().GetAll().Any(s => s.EPWorkID == _ID);
            return qIsExists;
        }

        public bool IsFo_MaterialDisposalIDExists(long _ID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<FO_MaterialDisposalAttachment>().GetAll().Any(s => s.MaterialDisposalID == _ID);
            return qIsExists;
        }

        public List<FO_NatureOfWork> GetAllActiveFO_natureofWork()
        {
            return db.Repository<FO_NatureOfWork>().GetAll().Where(d => d.IsActive == true).ToList();
        }

        public List<object> GetF_EmergencyDisposalByID(long EPWorkID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetF_EmergencyDisposalByID(EPWorkID);
        }

        public object GetF_EmergencyDisposalObjectByID(long DisposalID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetF_EmergencyDisposalObjectByID(DisposalID);
        }

        public bool SaveDisposalEmergency_Purchase(FO_MaterialDisposal _Obj)
        {
            bool isSaved = false;

            if (_Obj.ID == 0)
            {
                db.Repository<FO_MaterialDisposal>().Insert(_Obj);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_MaterialDisposal>().Update(_Obj);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool SaveDM_Attachment(FO_MaterialDisposalAttachment _MaterialDisposalAttachment)
        {
            bool isSaved = false;

            if (_MaterialDisposalAttachment.ID == 0)
            {
                db.Repository<FO_MaterialDisposalAttachment>().Insert(_MaterialDisposalAttachment);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_MaterialDisposalAttachment>().Update(_MaterialDisposalAttachment);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public object GetFloodFightingDivision_CampSite_By_ID(long EmergencyPurchaseID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetFloodFightingDivision_CampSite_By_ID(EmergencyPurchaseID);
        }

        public object GetIsexistFo_EPItem_ID(long EmergencyPurchaseID, int itemid)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetIsexistFo_EPItem_ID(EmergencyPurchaseID, itemid);
        }

        public IEnumerable<DataRow> GetFO_EMItemsPurchasingQty(long EmergencyPurchaseID, long Categoryid)
        {
            return db.ExecuteDataSet("Proc_FO_EPItemPurchasing", EmergencyPurchaseID, Categoryid);
        }

        public object GetItemPurchasing_EmergcnyP_ID(long EmergencyPurchaseID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetItemPurchasing_EmergcnyP_ID(EmergencyPurchaseID);
        }

        //
        public object GetEmergncyPurches_ItemQty_ID(long EmergencyPurchaseID, long itemid)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetEmergncyPurches_ItemQty_ID(EmergencyPurchaseID, itemid);
        }

        public object GetFloodFightingInsfrastructureName(int StructureTypeID, int structid)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetFloodFightingInsfrastructureName(StructureTypeID, structid);
        }

        //
        public object GetItemPurchas_Type_InsfrastructureName(int StructureTypeID, int structid)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetItemPurchas_Type_InsfrastructureName(StructureTypeID, structid);
        }

        public object GetMaterialDisposal_Header_By_ID(long EPworkID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetMaterialDisposal_Header_By_ID(EPworkID);
        }

        public object GetF_MaterialDisposal_Attachement_Header_By_ID(long MaterialDisposalID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetF_MaterialDisposal_Attachement_Header_By_ID(MaterialDisposalID);
        }

        public bool DeleteFo_MaterialDisposal(long _ID)
        {
            db.Repository<FO_MaterialDisposal>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool DeleteFo_MaterialDisposal_Attachement(long _ID)
        {
            db.Repository<FO_MaterialDisposalAttachment>().Delete(_ID);
            db.Save();

            return true;
        }

        //public bool DeleteFo_MaterialDisposal_Attachement(long _ID)
        //{
        //    db.Repository<FO_MaterialDisposalAttachment>().GetAll().Where(x => x.MaterialDisposalID == _ID).ToList().Clear();
        //    db.Repository<FO_MaterialDisposalAttachment>().Delete(_ID);
        //    db.Save();
        //    return true;
        //}

        public bool DeleteFo_MaterialDisposal_Attachement_List(long _DisposalDetailID)
        {
            FloodOperationsRepository repFloodOppRep = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOppRep.DeleteDisposalDetailbyDisposalDetailID(_DisposalDetailID);
        }

        public object GetEmergency_DivisionStructtypeID(long EmergencyPurchaseID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetEmergency_DivisionStructtypeID(EmergencyPurchaseID);
        }

        public long AddDisposalEmergencyPurchase(int _DisposalID, int _EPWorkID, string _VehicleNumber, string _BuiltyNumber, int? _Quantity, long? _Cost, string _Unit, List<string> lstNameofFiles, int _UserID)
        {
            FO_MaterialDisposal mdlFO_MaterialDisposal = new FO_MaterialDisposal();
            if (_DisposalID != 0)
            {
                mdlFO_MaterialDisposal = db.Repository<FO_MaterialDisposal>().FindById(_DisposalID);
                mdlFO_MaterialDisposal.CreatedBy = Convert.ToInt32(mdlFO_MaterialDisposal.CreatedBy);
                mdlFO_MaterialDisposal.CreatedDate = Convert.ToDateTime(mdlFO_MaterialDisposal.CreatedDate);

                mdlFO_MaterialDisposal.ModifiedBy = Convert.ToInt32(_UserID);
                mdlFO_MaterialDisposal.ModifiedDate = DateTime.Now;
            }
            else
            {
                mdlFO_MaterialDisposal.CreatedBy = Convert.ToInt32(_UserID);
                mdlFO_MaterialDisposal.ModifiedDate = DateTime.Now;
                mdlFO_MaterialDisposal.CreatedDate = DateTime.Now;
            }
            mdlFO_MaterialDisposal.EPWorkID = _EPWorkID;
            mdlFO_MaterialDisposal.DisposalDate = DateTime.Now;
            mdlFO_MaterialDisposal.VehicleNumber = _VehicleNumber;

            mdlFO_MaterialDisposal.BuiltyNumber = _BuiltyNumber;

            mdlFO_MaterialDisposal.QtyMaterial = _Quantity;
            mdlFO_MaterialDisposal.Unit = _Unit;
            mdlFO_MaterialDisposal.Cost = _Cost;

            if (SaveDisposalEmergency_Purchase(mdlFO_MaterialDisposal))
            {
                if (IsDisposalAttachmentExists(mdlFO_MaterialDisposal.ID))
                {
                    DeleteDisposalAttachmentByDisposalID(mdlFO_MaterialDisposal.ID);
                }
                for (int i = 0; i < lstNameofFiles.Count; ++i)
                {
                    AddDisposalAttachment(mdlFO_MaterialDisposal.ID, _UserID, lstNameofFiles[i]);
                }
                //link attachments to Material Disposal id if there any
            }
            return mdlFO_MaterialDisposal.ID;
        }

        public void AddDisposalAttachment(long _DisposalID, int _UserID, string _AttachmentPath)
        {
            FO_MaterialDisposalAttachment mdlWPAtchmnt = new FO_MaterialDisposalAttachment();

            mdlWPAtchmnt.MaterialDisposalID = _DisposalID;
            mdlWPAtchmnt.FileName = _AttachmentPath;
            mdlWPAtchmnt.FileURL = _AttachmentPath;
            mdlWPAtchmnt.CreatedBy = _UserID;
            mdlWPAtchmnt.CreatedDate = DateTime.Now;
            mdlWPAtchmnt.ModifiedBy = Convert.ToInt32(_UserID);
            mdlWPAtchmnt.ModifiedDate = DateTime.Now;

            db.Repository<FO_MaterialDisposalAttachment>().Insert(mdlWPAtchmnt);
            db.Save();
        }

        public bool IsDisposalAttachmentExists(long _DisposalID)
        {
            bool qIsAttachmentExists = db.Repository<FO_MaterialDisposalAttachment>().GetAll().Any(s => s.MaterialDisposalID == _DisposalID);
            return qIsAttachmentExists;
        }

        public bool DeleteDisposalAttachmentByDisposalID(long _DisposalID)
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.DeleteDisposalAttachmentByDisposalID(_DisposalID);
        }

        public object SearchEmergencyPurchaseByID(long _InfrastructureTypeID, long _EmergencyPurchasesID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[FO_SearchEmergencyPurchaseByID]", _InfrastructureTypeID, _EmergencyPurchasesID);
            object lstsearchemergencypurchase = (from DataRow dr in dt.Rows
                                                 select new
                                                 {
                                                     EmergencyPurchaseID = Convert.ToInt32(dr["EmergencyPurchaseID"]),
                                                     InfrastructureType = dr["InfrastructureType"].ToString(),
                                                     InfrastructureName = dr["InfrastructureName"].ToString(),
                                                     CompSite = Convert.ToInt32(dr["CompSite"]),
                                                     DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                     RD = (dr["RD"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["RD"]),

                                                     EmergencyCreatedDate = dr["EmergencyCreatedDate"].ToString()
                                                 }).FirstOrDefault();
            return lstsearchemergencypurchase;
        }

        public List<object> GetFo_EP_NatureofWork()
        {
            FloodOperationsRepository repFloodOperations = this.db.ExtRepositoryFor<FloodOperationsRepository>();
            return repFloodOperations.GetFo_EP_NatureofWork();
        }

        #endregion Emergency Purchases

        #region Reference Data

        public List<object> GetProblemNatureList()
        {
            List<object> lstProblemNature = null;
            lstProblemNature = db.Repository<FO_ProblemNature>().GetAll().ToList<FO_ProblemNature>().ToList<object>();
            return lstProblemNature;
        }

        public bool IsProblemNatureExists(FO_ProblemNature _ProblemNature)
        {
            bool qIsNameExists = db.Repository<FO_ProblemNature>().GetAll().Any(i => i.Name == _ProblemNature.Name
                             && (i.ID != _ProblemNature.ID || _ProblemNature.ID == 0));
            return qIsNameExists;
        }

        public bool SaveProblemNature(FO_ProblemNature _ProblemNature)
        {
            bool isSaved = false;

            if (_ProblemNature.ID == 0)
            {
                db.Repository<FO_ProblemNature>().Insert(_ProblemNature);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_ProblemNature>().Update(_ProblemNature);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool DeleteProblemNature(long _ID)
        {
            db.Repository<FO_ProblemNature>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsProblemNatureIDExists(long _ProblemNatureID)
        {
            bool qIsExists = false;
            qIsExists = db.Repository<FO_IProblems>().GetAll().Any(s => s.ProblemID == _ProblemNatureID);
            return qIsExists;
        }

        #region Flood Bund Gauges

        public FO_StructureNalaHillTorant GetStructureNalaHillTorantByID(long _StructureNalaHillToranID)
        {
            FO_StructureNalaHillTorant qStructureNalaHillToran = db.Repository<FO_StructureNalaHillTorant>().GetAll().Where(s => s.ID == _StructureNalaHillToranID).FirstOrDefault();
            return qStructureNalaHillToran;
        }

        public long StructureNallahHillTorantInsertion(long _ID, string _StructureType, string _StructureName, long? _DivisionID, long? _DistrictID, long? _TehsilID, long? _VillageID, double _DesignedDischarge, bool _Status, int _CreatedBy, int _ModifiedBy, long _DSID)
        {
            long _DSIDOut = 0;
            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("FO_StructureNalaHillInsertion", _ID, _StructureType, _StructureName, _DivisionID, _DistrictID, _TehsilID, _VillageID, _DesignedDischarge, _Status, _CreatedBy, _ModifiedBy, _DSID);
            return _DSIDOut;
        }

        public IEnumerable<DataRow> GetFloodBundRefSearch(long? StructureNalaHillTorantID, long? _ZoneID, long? _CircleID, long? _DivisionID, string _StructureType, string _InfrastructureName)
        {
            return db.ExecuteDataSet("FO_SearchStructureNalaHill", StructureNalaHillTorantID, _ZoneID, _CircleID, _DivisionID, _StructureType, _InfrastructureName);
        }

        public List<object> GetStructureNameFloodBund(long _UserId, string _StructureType)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("Proc_FO_GetStructureFloodBundByUserID", _UserId, _StructureType);
            List<object> lstStructureName = (from DataRow dr in dt.Rows
                                             select new
                                             {
                                                 ID = Convert.ToInt64(dr["ID"]),
                                                 NAME = Convert.ToString(dr["NAME"]),
                                             }).ToList<object>();
            return lstStructureName;
        }

        public List<string> GetTimeListFloodBund()
        {
            DateTime start = DateTime.ParseExact("00:00", "HH:mm", null);
            DateTime end = DateTime.ParseExact("23:59", "HH:mm", null);
            int interval = 60;
            List<string> lstTimeIntervals = new List<string>();
            for (DateTime i = start; i <= end; i = i.AddMinutes(interval))
            {
                lstTimeIntervals.Add(i.ToString("hh:mm tt"));
            }
            return lstTimeIntervals;
        }

        public IEnumerable<DataRow> GetFloodBundAddGauges(string _StructureType, string _StructureName, string _Time, DateTime? _Date)
        {
            return db.ExecuteDataSet("Proc_FO_GDHillTorrentNallah", _StructureType, _StructureName, _Time, _Date);
        }

        public bool SaveFloodGaugeReading(UA_Users mdlUser, int ModifiedBy, List<Tuple<string, string, string, string, string, string, string>> _listHillTorrent)
        {
            bool isSaved = false;
            foreach (var HillTorrent in _listHillTorrent)
            {
                FO_FloodGaugeReading ObjeFloodGaugeReading = new FO_FloodGaugeReading();
                ObjeFloodGaugeReading.StructureType = Convert.ToString(HillTorrent.Item1);
                ObjeFloodGaugeReading.StructureID = Convert.ToInt64(HillTorrent.Item2);
                //DateTime dt = Convert.ToDateTime(HillTorrent.Item3);
                ObjeFloodGaugeReading.ReadingDatetime = DateTime.ParseExact(HillTorrent.Item3, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
                ObjeFloodGaugeReading.GuageReading = Convert.ToDouble(HillTorrent.Item4);
                ObjeFloodGaugeReading.DischargeValue = Convert.ToDouble(HillTorrent.Item5);
                //ObjeFloodGaugeReading.DischargeValue = Convert.ToDouble(HillTorrent.Item5);
                if (Convert.ToInt64(HillTorrent.Item7) == 0)
                {
                    ObjeFloodGaugeReading.CreatedDate = DateTime.Now;
                    ObjeFloodGaugeReading.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    db.Repository<FO_FloodGaugeReading>().Insert(ObjeFloodGaugeReading);
                }
                else
                {
                    ObjeFloodGaugeReading.ID = Convert.ToInt64(HillTorrent.Item7);
                    ObjeFloodGaugeReading.CreatedDate = Convert.ToDateTime(HillTorrent.Item6);
                    ObjeFloodGaugeReading.ModifiedDate = DateTime.Now;
                    ObjeFloodGaugeReading.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjeFloodGaugeReading.ModifiedBy = Convert.ToInt32(ModifiedBy);
                    db.Repository<FO_FloodGaugeReading>().Update(ObjeFloodGaugeReading);
                }
            }
            db.Save();
            isSaved = true;
            return isSaved;
        }

        public bool SaveFloodGaugeReadingBund(UA_Users mdlUser, int ModifiedBy, List<Tuple<string, string, string, string, string, string>> lstGaugeBund)
        {
            bool isSaved = false;
            foreach (var lstBund in lstGaugeBund)
            {
                FO_FloodGaugeReading ObjeFloodGaugeReadingBund = new FO_FloodGaugeReading();
                ObjeFloodGaugeReadingBund.StructureType = Convert.ToString(lstBund.Item1);
                ObjeFloodGaugeReadingBund.FloodGaugeID = Convert.ToInt64(lstBund.Item2);
                ObjeFloodGaugeReadingBund.ReadingDatetime = DateTime.ParseExact(lstBund.Item3, "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
                ObjeFloodGaugeReadingBund.GuageReading = Convert.ToDouble(lstBund.Item4);
                if (Convert.ToInt64(lstBund.Item6) == 0)
                {
                    ObjeFloodGaugeReadingBund.CreatedDate = DateTime.Now;
                    ObjeFloodGaugeReadingBund.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    db.Repository<FO_FloodGaugeReading>().Insert(ObjeFloodGaugeReadingBund);
                }
                else
                {
                    ObjeFloodGaugeReadingBund.ID = Convert.ToInt64(lstBund.Item6);
                    ObjeFloodGaugeReadingBund.CreatedDate = Convert.ToDateTime(lstBund.Item5);
                    ObjeFloodGaugeReadingBund.ModifiedDate = DateTime.Now;
                    ObjeFloodGaugeReadingBund.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    ObjeFloodGaugeReadingBund.ModifiedBy = Convert.ToInt32(ModifiedBy);
                    db.Repository<FO_FloodGaugeReading>().Update(ObjeFloodGaugeReadingBund);
                }
            }
            db.Save();
            isSaved = true;
            return isSaved;
        }

        public List<object> GetAllFloodGaugesRDs(long _StructureID)
        {
            return db.ExtRepositoryFor<FloodOperationsRepository>().GetFloodBundGaugesRD(_StructureID);
        }

        public IEnumerable<DataRow> GetBundGaugesDataSearch(long? _DivisionID, string _StructureType, string _StructureName, long? GaugeRD, string _Time, DateTime? _FromDate, DateTime? _ToDate)
        {
            return db.ExecuteDataSet("Proc_FO_SearchBundGaugesData", _DivisionID, _StructureType, _StructureName, GaugeRD, _Time, _FromDate, _ToDate);
        }

        public CO_Circle GetCirlceByDivisionID(long _DivisionID)
        {
            CO_Division mdlDivision = db.Repository<CO_Division>().GetAll().Where(x => x.ID == _DivisionID).FirstOrDefault();
            CO_Circle lstCircle =
                db.Repository<CO_Circle>()
                    .GetAll()
                    .Where(x => x.ID == mdlDivision.CircleID)
                    .OrderBy(x => x.Name)
                    .FirstOrDefault();
            return lstCircle;
        }
        public CO_Zone GetZoneByCirlceID(long _CircleID)
        {
            CO_Circle mdlCircle = db.Repository<CO_Circle>().GetAll().Where(x => x.ID == _CircleID).FirstOrDefault();
            CO_Zone lstZone = db.Repository<CO_Zone>().GetAll().Where(x => x.ID == mdlCircle.ZoneID).OrderBy(x => x.Name).FirstOrDefault();
            return lstZone;
        }
        public bool IsStructureNalaHillTorantExists(long _ID, long _DivisionID, string _StructureType, string _StructureName)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("FO_IsStructureNalaHillTorantExists", _ID, _DivisionID, _StructureType, _StructureName);
            bool IsExists = false;
            foreach (DataRow DR in dt.Rows)
            {
                IsExists = Convert.ToBoolean(DR["IsExists"].ToString());
            }
            return IsExists;

        }
        #endregion Flood Bund Gauges

        #endregion Reference Data

        #region RoleRights

        public Int16 CanAddFloodInspections()
        {
            Int16 InspectionTypeToAdd = 0;

            UA_SystemParameters systemParameters = null;
            systemParameters = new FloodFightingPlanDAL().SystemParameterValue("PreFloodInspection", "StartDate");// 01-Apr
            string PreStartDate = systemParameters.ParameterValue + "-" + DateTime.Now.Year;
            systemParameters = new FloodFightingPlanDAL().SystemParameterValue("PreFloodInspection", "EndDate");// 14-Jun
            string PreEndDate = systemParameters.ParameterValue + "-" + DateTime.Now.Year;

            if ((DateTime.Now >= Convert.ToDateTime(PreStartDate) && DateTime.Now <= Convert.ToDateTime(PreEndDate)) || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
            {
                InspectionTypeToAdd = 1;
            }
            else
            {
                systemParameters = new FloodFightingPlanDAL().SystemParameterValue("PostFloodInspection", "StartDate"); // 16-Oct
                string PostStartDate = systemParameters.ParameterValue + "-" + DateTime.Now.Year;
                systemParameters = new FloodFightingPlanDAL().SystemParameterValue("PostFloodInspection", "EndDate");// 31-Dec
                string PostEndDate = systemParameters.ParameterValue + "-" + DateTime.Now.Year;

                if ((DateTime.Now >= Convert.ToDateTime(PostStartDate) && DateTime.Now <= Convert.ToDateTime(PostEndDate)) || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
                {
                    InspectionTypeToAdd = 2;
                }
            }

            return InspectionTypeToAdd;
        }

        public bool CanAddEditFloodInspections(int _Year, long _DesignationID, int _InspectionStatus, int InspectionTypeID)
        {
            UA_SystemParameters systemParameters = null;
            string startDate = string.Empty;
            string endDate = string.Empty;
            bool returnVal = false;

            if (InspectionTypeID == 1) // For preInspection
            {
                systemParameters = new FloodFightingPlanDAL().SystemParameterValue("PreFloodInspection", "StartDate"); // 01-Apr
                startDate = systemParameters.ParameterValue + "-" + _Year.ToString();
                systemParameters = new FloodFightingPlanDAL().SystemParameterValue("PreFloodInspection", "EndDate"); // 14-Jun
                endDate = systemParameters.ParameterValue + "-" + _Year.ToString();
            }
            else if (InspectionTypeID == 2) // For PostInspection
            {
                systemParameters = new FloodFightingPlanDAL().SystemParameterValue("PostFloodInspection", "StartDate"); // 16-Oct
                startDate = systemParameters.ParameterValue + "-" + _Year.ToString();
                systemParameters = new FloodFightingPlanDAL().SystemParameterValue("PostFloodInspection", "EndDate"); //31-Dec
                endDate = systemParameters.ParameterValue + "-" + _Year.ToString();
            }

            if (_DesignationID == Convert.ToInt64(Constants.Designation.XEN))
            {
                if (_InspectionStatus == 2)
                {
                    returnVal = false;
                }
                else if ((DateTime.Now >= Convert.ToDateTime(startDate) && DateTime.Now <= Convert.ToDateTime(endDate)) || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
                {
                    returnVal = true;
                }
            }
            else if (_DesignationID == Convert.ToInt64(Constants.Designation.DF))
            {
                if ((DateTime.Now.Year == _Year || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO")))) && _InspectionStatus == 2)
                {
                    returnVal = true;
                }
            }

            return returnVal;
        }

        public bool CanAddEditStoneDeployment(int _Year, long _DesignationID)
        {
            bool returnVal = false;

            if (_DesignationID == Convert.ToInt64(Constants.Designation.XEN))
            {
                if (DateTime.Now.Year == _Year || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }

        public bool CanAddEditEmergencyPurchase(int _Year, long _DesignationID)
        {
            UA_SystemParameters systemParameters = null;
            string startDate = string.Empty;
            string endDate = string.Empty;
            bool returnVal = false;
            systemParameters = new FloodFightingPlanDAL().SystemParameterValue("FloodSeason", "StartDate");
            startDate = systemParameters.ParameterValue + "-" + _Year; // 01-Jan
            systemParameters = new FloodFightingPlanDAL().SystemParameterValue("FloodSeason", "EndDate"); // 31-Mar
            endDate = systemParameters.ParameterValue + "-" + _Year;

            if (_DesignationID == Convert.ToInt64(Constants.Designation.XEN))
            {
                if ((DateTime.Now >= Convert.ToDateTime(startDate) && DateTime.Now <= Convert.ToDateTime(endDate)) || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }

        public bool CanAddEditOnSiteMonitoring(int _Year, long _DesignationID)
        {
            UA_SystemParameters systemParameters = null;
            string startDate = string.Empty;
            string endDate = string.Empty;
            bool returnVal = false;

            systemParameters = new FloodFightingPlanDAL().SystemParameterValue("FloodSeason", "StartDate");
            startDate = systemParameters.ParameterValue + "-" + _Year; // 01-Jan
            systemParameters = new FloodFightingPlanDAL().SystemParameterValue("FloodSeason", "EndDate"); // 31-Mar
            endDate = systemParameters.ParameterValue + "-" + _Year;

            if (_DesignationID == Convert.ToInt64(Constants.Designation.ADM) || _DesignationID == Convert.ToInt64(Constants.Designation.MA))
            {
                if ((DateTime.Now >= Convert.ToDateTime(startDate) && DateTime.Now <= Convert.ToDateTime(endDate)) || Convert.ToBoolean(Convert.ToInt16(Utility.ReadConfiguration("RemoveYearCheckFO"))))
                {
                    returnVal = true;
                }
            }

            return returnVal;
        }

        #endregion RoleRights
    }
}