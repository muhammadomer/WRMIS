using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.FloodOperations;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.FloodOperations
{
    public class FloodInspectionsDAL : BaseDAL
    {
        #region "Independent Inspection"
        /// <summary>
        /// This method return Protection Infrastructure details
        /// Created on 13-Oct-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>FO_ProtectionInfrastructure</returns>
        public object GetFloodInspectionByID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetFloodInspectionByID(_FloodInspectionID);
        }

        /// <summary>
        /// This method return list of InspectionConditions By Codition Group
        /// Created on 13-Oct-2016
        /// </summary>
        /// <param name="_CoditionGroup"></param>
        /// <returns>List<FO_InspectionConditions></returns>
        public List<FO_InspectionConditions> GetInspectionConditionsByGroup(string _CoditionGroup)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetInspectionConditionsByGroup(_CoditionGroup);
        }

        /// <summary>
        /// This method return object of IGCProtectionInfrastructure By FloodInspectionID
        /// Created on 24-Oct-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>FO_IGCProtectionInfrastructure</returns>
        public FO_IGCProtectionInfrastructure GetIGCProtectionInfrastructureByInspectionID(long _FloodInspectionID)
        {
            return db.Repository<FO_IGCProtectionInfrastructure>().GetAll().Where(s => s.FloodInspectionID == _FloodInspectionID).FirstOrDefault();
        }

        public bool IsIGCProtectionInfrastructureDataAlreadyExists(long _FloodInspectionID)
        {
            bool qIsIGCProtectionInfrastructureDataAlreadyExists = db.Repository<FO_IGCProtectionInfrastructure>().GetAll().Any(i => i.FloodInspectionID == _FloodInspectionID);
            return qIsIGCProtectionInfrastructureDataAlreadyExists;
        }

        public bool SaveIGCProtectionInfrastructure(FO_IGCProtectionInfrastructure _IGCProtectionInfrastructure)
        {
            bool isSaved = false;

            if (IsIGCProtectionInfrastructureDataAlreadyExists(_IGCProtectionInfrastructure.FloodInspectionID) == false && _IGCProtectionInfrastructure.ID == 0)
            {
                db.Repository<FO_IGCProtectionInfrastructure>().Insert(_IGCProtectionInfrastructure);
                db.Save();
                isSaved = true;
            }
            else if (_IGCProtectionInfrastructure.ID != 0)
            {
                db.Repository<FO_IGCProtectionInfrastructure>().Update(_IGCProtectionInfrastructure);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }
        public bool SaveFloodIGCProtectionInfrastructure(long _FloodInspectionID, int _InspectionID, short? _GRHutConditionID, short? _ServiceRoadConditionID, short? _WatchingHutConditionID, bool _RiverGauge, string _Remarks, int _UserID)
        {
            bool Saved = false;

            try
            {
                FO_IGCProtectionInfrastructure IGCInspection = new FO_IGCProtectionInfrastructure();


                if (_InspectionID != 0)
                {
                    IGCInspection = db.Repository<FO_IGCProtectionInfrastructure>().FindById(_InspectionID);
                    IGCInspection.CreatedBy = Convert.ToInt32(IGCInspection.CreatedBy);
                    IGCInspection.CreatedDate = Convert.ToDateTime(IGCInspection.CreatedDate);
                    IGCInspection.ModifiedBy = Convert.ToInt32(_UserID);
                    IGCInspection.ModifiedDate = DateTime.Now;
                }
                else
                {
                    IGCInspection.CreatedBy = Convert.ToInt32(_UserID);
                    IGCInspection.CreatedDate = DateTime.Now;
                }
                IGCInspection.FloodInspectionID = _FloodInspectionID;
                IGCInspection.GRHutConditionID = _GRHutConditionID;
                IGCInspection.ServiceRoadConditionID = _ServiceRoadConditionID;
                IGCInspection.WatchingHutConditionID = _WatchingHutConditionID;
                IGCInspection.RiverGauge = _RiverGauge;
                IGCInspection.Remarks = _Remarks;
                Saved = SaveIGCProtectionInfrastructure(IGCInspection);

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Saved;
        }

        /// <summary>
        /// This method return object of type object By FloodInspectionID
        /// Created on 3-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>object</returns>
        public object GetIGCBarrageHWInformationByInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetIGCBarrageHWInformationByInspectionID(_FloodInspectionID);
        }
        /// <summary>
        /// This method return object of type object By FloodInspectionID
        /// Created on 3-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>object</returns>
        public object GetIGCBarrageHWGInformationByInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetIGCBarrageHWGInformationByInspectionID(_FloodInspectionID);
        }
        /// <summary>
        /// This method return object of type object By BarrageHWID
        /// Created on 4-Nov-2016
        /// </summary>
        /// <param name="_IGCBarrageHWID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIGCBarrageHWGatesInformationByBarrageHWID(long _IGCBarrageHWID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetIGCBarrageHWGatesInformationByBarrageHWID(_IGCBarrageHWID);
        }

        public List<object> GetBarrageGatesInformationByFloodInspectionID(long _IGCBarrageHWID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetBarrageGatesInformationByFloodInspectionID(_IGCBarrageHWID);
        }


        public object GetIGCBarrageHWGatesInfoByBarrageHWID(long _IGCBarrageGateHWID)
        {
            return db.Repository<FO_IGCBarrageHWGates>().GetAll().Where(s => s.ID == _IGCBarrageGateHWID).Select(c => new { c.ID, c.IGCBarrageHWID, c.TotalGates, c.WorkingGates, c.GateTypeID, c.CreatedBy, c.CreatedDate }).FirstOrDefault();
        }
        public long SaveIGCBarrageHWInformation(FO_IGCBarrageHW _IGCBarrageHW)
        {
            bool isSaved = false;
            long _IGCBarrageHWID = 0;

            if (IsIGCBarrageHWDataAlreadyExists(_IGCBarrageHW.FloodInspectionID) == false)
            {
                db.Repository<FO_IGCBarrageHW>().Insert(_IGCBarrageHW);
                db.Save();

                _IGCBarrageHWID = _IGCBarrageHW.ID;

                isSaved = true;
            }
            else
            {
                db.Repository<FO_IGCBarrageHW>().Update(_IGCBarrageHW);
                db.Save();

                _IGCBarrageHWID = _IGCBarrageHW.ID;

                isSaved = true;
            }

            if (isSaved == false)
            {
                _IGCBarrageHWID = 0;
            }

            return _IGCBarrageHWID;
        }

        public bool IsIGCBarrageHWDataAlreadyExists(long _FloodInspectionID)
        {
            bool qIsIGCBarrageHWDataAlreadyExists = db.Repository<FO_IGCBarrageHW>().GetAll().Any(i => i.FloodInspectionID == _FloodInspectionID);
            return qIsIGCBarrageHWDataAlreadyExists;
        }
        public bool IsIGCBarrageHWGatesAlreadyExists(long _BarrageHWID)
        {
            bool qIsIGCBarrageHWGatesAlreadyExists = db.Repository<FO_IGCBarrageHWGates>().GetAll().Any(i => i.IGCBarrageHWID == _BarrageHWID);
            return qIsIGCBarrageHWGatesAlreadyExists;
        }

        public bool SaveIGCBarrageHWGatesInformation(List<FO_IGCBarrageHWGates> _IGCBarrageHWGatesList)
        {
            bool isSaved = false;

            foreach (var ls in _IGCBarrageHWGatesList)
            {

                if (IsIGCBarrageHWGatesataAlreadyExists(ls.IGCBarrageHWID, ls.GateTypeID) == false)
                {
                    db.Repository<FO_IGCBarrageHWGates>().Insert(ls);
                    db.Save();
                    isSaved = true;
                }
                else
                {
                    db.Repository<FO_IGCBarrageHWGates>().Update(ls);
                    db.Save();
                    isSaved = true;
                }

            }
            return isSaved;
        }

        public bool IsIGCBarrageHWGatesataAlreadyExists(long _IGCBarrageHWID, short? _GateTypeID)
        {
            bool qIsIGCBarrageHWGatesataAlreadyExists = db.Repository<FO_IGCBarrageHWGates>().GetAll().Any(i => i.IGCBarrageHWID == _IGCBarrageHWID && i.GateTypeID == _GateTypeID);
            return qIsIGCBarrageHWGatesataAlreadyExists;
        }

        /// <summary>
        /// This method return object of type object of FO_IGCDrain By FloodInspectionID
        /// Created on 8-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>object</returns>
        public object GetIGCDrainInformationByInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetIGCDrainInformationByInspectionID(_FloodInspectionID);
        }

        public bool SaveIGCDrain(FO_IGCDrain _IGCDrain)
        {
            bool isSaved = false;

            if (IsIGCDrainDataAlreadyExists(_IGCDrain.ID) == false)
            {
                db.Repository<FO_IGCDrain>().Insert(_IGCDrain);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_IGCDrain>().Update(_IGCDrain);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }
        public bool SaveFloodIGCDrain(long _FloodInspectionID, int _InspectionID, double? _ExistingCapacity, double? _ImprovedCapacity, short? _CurrentLevel, bool _DrainWaterET, double? _OutfallBedWidth, double? _OutfallSupplyWidth, short? _BridgeGovtNo, short? _BridgePvtNo, string _Remarks, int _UserID)
        {
            bool Saved = false;

            try
            {
                FO_IGCDrain IGCInspection = new FO_IGCDrain();

                if (_InspectionID != 0)
                {
                    IGCInspection = db.Repository<FO_IGCDrain>().FindById(_InspectionID);
                    IGCInspection.CreatedBy = Convert.ToInt32(IGCInspection.CreatedBy);
                    IGCInspection.CreatedDate = Convert.ToDateTime(IGCInspection.CreatedDate);
                    IGCInspection.ModifiedBy = Convert.ToInt32(_UserID);
                    IGCInspection.ModifiedDate = DateTime.Now;
                }
                else
                {
                    IGCInspection.CreatedBy = Convert.ToInt32(_UserID);
                    IGCInspection.CreatedDate = DateTime.Now;
                }
                IGCInspection.FloodInspectionID = _FloodInspectionID;
                IGCInspection.ExistingCapacity = _ExistingCapacity;
                IGCInspection.ImprovedCapacity = _ImprovedCapacity;
                IGCInspection.CurrentLevel = _CurrentLevel;
                IGCInspection.DrainWaterET = _DrainWaterET;
                IGCInspection.OutfallBedWidth = _OutfallBedWidth;
                IGCInspection.OutfallFullSupplyDepth = _OutfallSupplyWidth;
                IGCInspection.BridgeGovtNo = _BridgeGovtNo;
                IGCInspection.BridgePvtNo = _BridgePvtNo;
                IGCInspection.Remarks = _Remarks;
                Saved = SaveIGCDrain(IGCInspection);


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Saved;
        }
        public long SaveFloodIGCBarrage(long _FloodInspectionID, int _InspectionID, short? _ArmyCPConditionID, string _CCTVIncharge, string _CCTVInchargePhone, short? _DataBoardConditionID, short? _LightConditionID, short? _OperationalCameras, double? _OperationalDeckElevated, bool? _PoliceMonitory, short? _TollHutConditionID, short? _TotalCameras, String _Remarks, int _UserID)
        {
            long ID = 0;

            try
            {
                FO_IGCBarrageHW IGCInspection = new FO_IGCBarrageHW();
                if (_InspectionID != 0)
                {
                    IGCInspection = db.Repository<FO_IGCBarrageHW>().FindById(_InspectionID);
                    IGCInspection.ModifiedBy = _UserID;
                    IGCInspection.ModifiedDate = DateTime.Now;
                }
                else
                {
                    IGCInspection.CreatedBy = _UserID;
                    IGCInspection.CreatedDate = DateTime.Now;
                }
                IGCInspection.FloodInspectionID = _FloodInspectionID;
                IGCInspection.ArmyCPConditionID = _ArmyCPConditionID;
                IGCInspection.CCTVIncharge = _CCTVIncharge;
                IGCInspection.CCTVInchargePhone = _CCTVInchargePhone;
                IGCInspection.DataBoardConditionID = _DataBoardConditionID;
                IGCInspection.LightConditionID = _LightConditionID;
                IGCInspection.OperationalCameras = _OperationalCameras;
                IGCInspection.OperationalDeckElevated = _OperationalDeckElevated;
                IGCInspection.PoliceMonitory = _PoliceMonitory;
                IGCInspection.TollHutConditionID = _TollHutConditionID;
                IGCInspection.TotalCameras = _TotalCameras;
                IGCInspection.Remarks = _Remarks;

                ID = SaveIGCBarrageHWInformation(IGCInspection);


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ID;
        }

        public bool IsIGCDrainDataAlreadyExists(long _IGCDrainID)
        {
            bool qIsIGCBarrageHWGatesataAlreadyExists = db.Repository<FO_IGCDrain>().GetAll().Any(i => i.ID == _IGCDrainID);
            return qIsIGCBarrageHWGatesataAlreadyExists;
        }


        public bool CheckInspectionStatusByID(long _FloodInspectionID)
        {
            FO_FloodInspection floodInspection = db.Repository<FO_FloodInspection>().FindById(_FloodInspectionID);
            floodInspection.InspectionStatusID = 2;
            db.Repository<FO_FloodInspection>().Update(floodInspection);
            db.Save();
            return true;
        }

        /// <summary>
        /// This method return object of type object of FO_IBreachingSection By FloodInspectionID
        /// Created on 11-Nov-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>object</returns>
        public List<object> GetIBreachingSectionByInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetIBreachingSectionByInspectionID(_FloodInspectionID);
        }

        /// <summary>
        /// This method return true or false on the base of weather _IBreachingSection saved or not
        /// Created on 11-Nov-2016
        /// </summary>
        /// <param name="_IBreachingSection"></param>
        /// <returns>bool</returns>
        public bool SaveIBreachingSection(FO_IBreachingSection _IBreachingSection)
        {
            bool isSaved = false;

            if (IsIBreachingSectionDataAlreadyExists(_IBreachingSection.FloodInspectionID, _IBreachingSection.FromRD, _IBreachingSection.ToRD) == false)
            {
                if (_IBreachingSection.AffectedLinersNo != null || _IBreachingSection.AffectedRowsNo != null || _IBreachingSection.RecommendedSolution != null || _IBreachingSection.RestorationCost != null)

                    db.Repository<FO_IBreachingSection>().Insert(_IBreachingSection);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<FO_IBreachingSection>().Update(_IBreachingSection);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }

        public bool IsIBreachingSectionDataAlreadyExists(long _FloodInspectionID, int _FromRD, int _ToRD)
        {
            bool qIsIBreachingSectionDataAlreadyExists = db.Repository<FO_IBreachingSection>().GetAll().Any(i => i.FloodInspectionID == _FloodInspectionID && i.FromRD == _FromRD && i.ToRD == _ToRD);
            return qIsIBreachingSectionDataAlreadyExists;
        }

        #region R D Wise Condition
        public List<FO_StonePitchSide> GetAllActiveStonePitchSide()
        {
            return db.Repository<FO_StonePitchSide>().GetAll().Where(d => d.IsActive == true).ToList();
        }
        public List<FO_EncroachmentType> GetAllActiveEncroachmentType()
        {
            return db.Repository<FO_EncroachmentType>().GetAll().Where(d => d.IsActive == true).ToList();
        }
        public bool SaveIRDWiseConditionForStonePitching(FO_IRDWiseCondition IRDWiseCondition)
        {
            bool isSaved = false;

            if (IRDWiseCondition.ID == 0)
                db.Repository<FO_IRDWiseCondition>().Insert(IRDWiseCondition);
            else
                db.Repository<FO_IRDWiseCondition>().Update(IRDWiseCondition);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        public long SaveIRDWiseConditions(long _FloodInspectionID, int _InspectionID, int _FromRD, int _ToRD, short _RDWiseTypeID, short _ConditionID, short? _StonePitchSideID, short? _EncroachmentTypeID, string _Remarks, int _UserID)
        {
            bool Saved = false;
            FO_IRDWiseCondition IRDInspection = new FO_IRDWiseCondition();
            try
            {


                if (_InspectionID == 0)
                {
                    IRDInspection.CreatedBy = Convert.ToInt32(_UserID);
                    IRDInspection.CreatedDate = DateTime.Now;
                }
                else
                {
                    IRDInspection = db.Repository<FO_IRDWiseCondition>().FindById(_InspectionID);
                    IRDInspection.CreatedBy = Convert.ToInt32(IRDInspection.CreatedBy);
                    IRDInspection.CreatedDate = Convert.ToDateTime(IRDInspection.CreatedDate);
                    IRDInspection.ModifiedBy = Convert.ToInt32(_UserID);
                    IRDInspection.ModifiedDate = DateTime.Now;
                }
                IRDInspection.FloodInspectionID = _FloodInspectionID;
                IRDInspection.FromRD = _FromRD;
                IRDInspection.ToRD = _ToRD;
                IRDInspection.ConditionID = _ConditionID;
                IRDInspection.RDWiseTypeID = _RDWiseTypeID;
                IRDInspection.StonePitchSideID = _StonePitchSideID;
                IRDInspection.EncroachmentTypeID = _EncroachmentTypeID;
                IRDInspection.Remarks = _Remarks;
                IRDInspection.CreatedBy = _UserID;
                IRDInspection.CreatedDate = DateTime.Now;
                Saved = SaveIRDWiseConditionForStonePitching(IRDInspection);


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return IRDInspection.ID;
        }
        public bool DeleteRDWiseCondition(long _ID)
        {
            db.Repository<FO_IRDWiseCondition>().Delete(_ID);
            db.Save();

            return true;
        }
        public List<object> GetRDWiseConditionForStonePitchingByID(long _FloodInspectionID, int _RDWiseTypeID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetRDWiseConditionForStonePitchingByID(_FloodInspectionID, _RDWiseTypeID);
        }
        public List<object> GetRDWiseConditionForStonePitchinApronByID(long _FloodInspectionID, int _RDWiseTypeID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetRDWiseConditionForStonePitchingApronByID(_FloodInspectionID, _RDWiseTypeID);
        }

        public List<object> GetEncroachmentByID(long _FloodInspectionD, int _RDWiseTypeID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetEncroachmentByID(_FloodInspectionD, _RDWiseTypeID);
        }
        public object GetStonePitchingRDConditionForByID(long _InspectionD)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetStonePitchingRDConditionForByID(_InspectionD);
        }
        public object GetIRDConditionForByID(long _InspectionD)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetIRDConditionForByID(_InspectionD);
        }
        public object GetEncroachmentRDConditionForByID(long _InspectionD)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetEncroachmentRDConditionForByID(_InspectionD);
        }
        #endregion R D Wise Condition

        #region MeasuringBookStatus
        public List<object> GetMBStatusPreItemList(long _DivisionID, int _Year, int _ItemCategoryID, long _StructureTypeID, long StructureID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_PreMBStatusLoad]", _DivisionID, _Year, _ItemCategoryID, _StructureTypeID, StructureID);
            List<object> lstPostMBDetails = (from DataRow dr in dt.Rows
                                             select new
                                             {


                                                 //         PreMBStatusID = Convert.ToInt64(dr["PreMBStatusID"]),
                                                 PreMBStatusID = (dr["PreMBStatusID"] == DBNull.Value) ? 0 : Convert.ToInt64(dr["PreMBStatusID"]),
                                                 OverAllDivisionItemID = (dr["OverAllDivisionItemID"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["OverAllDivisionItemID"]),
                                                 ItemID = Convert.ToInt64(dr["ItemID"]),
                                                 ItemName = dr["ItemName"].ToString(),
                                                 LYQty = (dr["LYQty"] == DBNull.Value) ? -489 : Convert.ToInt32(dr["LYQty"]),
                                                 DIVIssueQty = (dr["DIVIssueQty"] == DBNull.Value) ? -489 : Convert.ToInt32(dr["DIVIssueQty"]),
                                                 AvaQty = (dr["AvaQty"] == DBNull.Value) ? -489 : Convert.ToInt32(dr["AvaQty"]),
                                                 BalanceQty = (dr["BalanceQty"] == DBNull.Value) ? -489 : Convert.ToInt32(dr["BalanceQty"])


                                             }).ToList<object>();
            return lstPostMBDetails;
        }

        public List<object> GetMBStatusPostItemList(long _DivisionID, int _Year, int _ItemCategoryID, long _StructureTypeID, long _StructureID)
        {


            //        @pDivisionID		BIGINT	= Null,
            //@pYear				INT		= Null,
            //@pItemCategoryID	SMALLINT= Null,
            //@pStructureTypeID	Bigint	= Null,
            //@pStructureID		Bigint  = NULL 

            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_PostMBStatusLoad]", _DivisionID, _Year, _ItemCategoryID, _StructureTypeID, _StructureID);
            List<object> lstPostMBDetails = (from DataRow dr in dt.Rows
                                             select new
                                             {
                                                 OverallDivItemsID = Convert.ToInt64(dr["OverallDivItemsID"]),
                                                 ItemsID = Convert.ToInt64(dr["ItemsID"]),
                                                 ItemName = dr["ItemName"].ToString(),
                                                 QtyPreFloodInspection = Convert.ToInt32(dr["QtyPreFloodInspection"]),

                                                 QuantityAvailable = (dr["QuantityAvailable"] == DBNull.Value) ? -489 : Convert.ToInt32(dr["QuantityAvailable"]),

                                                // QuantityAvailable = Convert.ToInt32(dr["QuantityAvailable"]),
                                                 QuantityConsumed = Convert.ToInt32(dr["QuantityConsumed"]),

                                                 QuantityRequired = (dr["QuantityRequired"] == DBNull.Value) ? -489 : Convert.ToInt32(dr["QuantityRequired"])


                                                // QuantityRequired = Convert.ToInt32(dr["QuantityRequired"])

                                             }).ToList<object>();
            return lstPostMBDetails;

            //FloodInspectionRepository repFloodInspectionRepository = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            //return repFloodInspectionRepository.GetMBStatusPostItemList(_CategoryID, _FloodInspectionID);
        }
        public object GetMBStatusByID(long _StatusID, int _StatusType)
        {
            FloodInspectionRepository repFloodInspectionRepository = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspectionRepository.GetMBStatusByID(_StatusID, _StatusType);
        }
        public bool SavePreMBStatus(FO_PreMBStatus _PreMBStatus)
        {
            bool isSaved = false;

            if (_PreMBStatus.ID == 0)
                db.Repository<FO_PreMBStatus>().Insert(_PreMBStatus);
            else
                db.Repository<FO_PreMBStatus>().Update(_PreMBStatus);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public DataSet GetPostMBStatus(long _DivisionID, int _Year, Int16 _CategoryID, long _StructureTypeID, long _StructureID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_PostMBStatusLoad", _DivisionID, _Year, _CategoryID, _StructureTypeID,
                _StructureID);
        }

        public IEnumerable<DataRow> GetPreMBStatus(long _DivisionID, int _Year, Int16 _CategoryID, long _StructureTypeID, long _StructureID)
        {
            return db.ExecuteDataSet("Proc_FO_PreMBStatusLoad", _DivisionID, _Year, _CategoryID, _StructureTypeID,
                _StructureID);
        }


        public bool SavePostMBStatus(FO_OverallDivItems Obj)
        {
            bool isSaved = false;

            if (Obj.ID == 0)
                db.Repository<FO_OverallDivItems>().Insert(Obj);
            else
                db.Repository<FO_OverallDivItems>().Update(Obj);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        //public object GetFloodInspectionDetailID_ByInspectionID(long _FloodInspectionID)
        //{
        //    object Obj = new object();
        //    Obj = (from obj in db.Repository<FO_FloodInspectionDetail>().GetAll()
        //           where obj.FloodInspectionID == _FloodInspectionID
        //           select new
        //           {
        //               FloodInspectionDetailID = obj.ID
        //           }
        //        ).FirstOrDefault<object>();
        //    return Obj;

        //}

        public int GetInspectionStatus(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspectionRepository = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspectionRepository.GetInspectionStatus(_FloodInspectionID);
        }

        #endregion

        #region ProblemForFloodInspection

        public List<object> GetProblemForFIByFloodInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetProblemForFIByFloodInspectionID(_FloodInspectionID);
        }

        public object GetProblemForFIByInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetProblemForFIByInspectionID(_FloodInspectionID);
        }

        public List<FO_ProblemNature> GetAllActiveProblemNature()
        {
            return db.Repository<FO_ProblemNature>().GetAll().Where(d => d.IsActive == true).ToList();
        }

        public bool SaveProFI(FO_IProblems _ObjFPI)
        {
            bool isSaved = false;

            if (_ObjFPI.ID == 0)
                db.Repository<FO_IProblems>().Insert(_ObjFPI);
            else
                db.Repository<FO_IProblems>().Update(_ObjFPI);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public bool DeleteProblemFI(long _ID)
        {
            db.Repository<FO_IProblems>().Delete(_ID);
            db.Save();

            return true;
        }

        public long? GetInfrastructureType(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetInfrastructureType(_FloodInspectionID);
        }
        public List<object> GetAllProblemNature()
        {
            return db.Repository<FO_ProblemNature>().GetAll().Where(d => d.IsActive == true).Select(x => new { x.ID, x.Name, x.Description, x.IsActive }).ToList<object>();
        }
        public long SaveProblemForFI(long _FloodInspectionID, int _InspectionID, int? _FromRD, int? _ToRD, short _ProblemID, string _Solution, long? _RestorationCost, long _UserID)
        {
            FO_IProblems ObjProFI = new FO_IProblems();

            if (_InspectionID == 0)
            {
                ObjProFI.CreatedBy = Convert.ToInt32(_UserID);
                ObjProFI.CreatedDate = DateTime.Now;
            }
            else
            {
                ObjProFI = db.Repository<FO_IProblems>().FindById(_InspectionID);
                ObjProFI.CreatedBy = Convert.ToInt32(ObjProFI.CreatedBy);
                ObjProFI.CreatedDate = Convert.ToDateTime(ObjProFI.CreatedDate);
                ObjProFI.ModifiedBy = Convert.ToInt32(_UserID);
                ObjProFI.ModifiedDate = DateTime.Now;
            }
            ObjProFI.FloodInspectionID = _FloodInspectionID;
            ObjProFI.FromRD = _FromRD;
            ObjProFI.ToRD = _ToRD;
            ObjProFI.ProblemID = _ProblemID;
            ObjProFI.Solution = _Solution;
            ObjProFI.RestorationCost = _RestorationCost;
            SaveProFI(ObjProFI);
            return ObjProFI.ID;
        }
        public bool SaveBreachSectionForFI(long _FloodInspectionID, int _InspectionID, int _FromRD, int _ToRD, short? _AffectedRowsNo, short? _AffectedLinersNo, string _Solution, long? _RestorationCost, long _UserID)
        {
            FO_IBreachingSection ObjBreachFI = new FO_IBreachingSection();

            if (_InspectionID == 0)
            {

                ObjBreachFI.CreatedDate = DateTime.Now;
                ObjBreachFI.CreatedBy = Convert.ToInt32(_UserID);
            }
            else
            {
                ObjBreachFI = db.Repository<FO_IBreachingSection>().FindById(_InspectionID);
                ObjBreachFI.CreatedDate = Convert.ToDateTime(ObjBreachFI.CreatedDate);
                ObjBreachFI.CreatedBy = Convert.ToInt32(ObjBreachFI.CreatedBy);
                ObjBreachFI.ModifiedDate = DateTime.Now;
                ObjBreachFI.ModifiedBy = Convert.ToInt32(_UserID);
            }
            ObjBreachFI.FloodInspectionID = _FloodInspectionID;
            ObjBreachFI.FromRD = _FromRD;
            ObjBreachFI.ToRD = _ToRD;
            ObjBreachFI.AffectedRowsNo = _AffectedRowsNo;
            ObjBreachFI.AffectedLinersNo = _AffectedLinersNo;
            ObjBreachFI.RecommendedSolution = _Solution;
            ObjBreachFI.RestorationCost = _RestorationCost;

            return SaveIBreachingSection(ObjBreachFI);
        }
        public long SaveIStonePosition(long _FloodInspectionID, long _InspectionID, int _RD, int _BeforeFloodQty, int? _AvailableQty, long _UserID)
        {
            FO_IStonePosition ObjStonePositionFI = new FO_IStonePosition();

            if (_InspectionID == 0)
            {
                ObjStonePositionFI.CreatedBy = Convert.ToInt32(_UserID);
                ObjStonePositionFI.CreatedDate = DateTime.Now;
            }
            else
            {
                ObjStonePositionFI = db.Repository<FO_IStonePosition>().FindById(_InspectionID);
                ObjStonePositionFI.CreatedBy = Convert.ToInt32(ObjStonePositionFI.CreatedBy);
                ObjStonePositionFI.CreatedDate = Convert.ToDateTime(ObjStonePositionFI.CreatedDate);
                ObjStonePositionFI.ModifiedBy = Convert.ToInt32(_UserID);
                ObjStonePositionFI.ModifiedDate = DateTime.Now;
            }
            ObjStonePositionFI.FloodInspectionID = _FloodInspectionID;
            ObjStonePositionFI.RD = _RD;
            ObjStonePositionFI.BeforeFloodQty = _BeforeFloodQty;
            ObjStonePositionFI.AvailableQty = _AvailableQty;

             SaveIStonePosition(ObjStonePositionFI);
             return ObjStonePositionFI.ID;
        }
        /// <summary>
        /// This method return object of IGCProtectionInfrastructure By FloodInspectionID
        /// Created on 24-Oct-2016
        /// </summary>
        /// <param name="_FloodInspectionID"></param>
        /// <returns>FO_IGCProtectionInfrastructure</returns>
        public object GetProtectionInfrastructureInspectionByInspectionID(long _FloodInspectionID)
        {
            return db.Repository<FO_IGCProtectionInfrastructure>().GetAll().Where(s => s.FloodInspectionID == _FloodInspectionID).Select(x => new { x.ID, x.FloodInspectionID, x.GRHutConditionID, x.RiverGauge, x.ServiceRoadConditionID, x.Remarks, x.WatchingHutConditionID }).FirstOrDefault();
        }
        #endregion

        #region IStonePosition
        public List<object> GetIStonePositionByFloodInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetIStonePositionByFloodInspectionID(_FloodInspectionID);
        }

        public bool SaveIStonePosition(FO_IStonePosition _ObjModel)
        {
            bool isSaved = false;

            if (_ObjModel.ID == 0)
                db.Repository<FO_IStonePosition>().Insert(_ObjModel);
            else
                db.Repository<FO_IStonePosition>().Update(_ObjModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }



        public List<object> GetiStonePosList(long _FloodInspID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_FIStonePosition]", _FloodInspID);
            List<object> lstgetitems = (from DataRow dr in dt.Rows
                                        select new

                                        {

                                            IStonePositionID = Convert.ToInt64(dr["IStonePositionID"]),
                                            RD = (dr["RD"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["RD"]),
                                            BeforeFloodQty = Convert.ToInt64(dr["BeforeFloodQty"]),
                                            AvailableQty = Convert.ToInt64(dr["AvailableQty"]),
                                            ConsumedQty = Convert.ToInt64(dr["ConsumedQty"])
                                            
                                            


                                        }).ToList<object>();
            return lstgetitems;



        }


        public object GetiStonePosObjbyID(long _ID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetiStonePosObjbyID(_ID);
        }


        #endregion

        public object GetInspectionDivisionID(long _FloodInspectionID)
        {
            return db.ExtRepositoryFor<FloodInspectionRepository>().GetInspectionDivisionID(_FloodInspectionID);
        }
        public DataSet GetPreMBStatusID(int _year, long _DivisionID, long _StructureTypeID, long _StructureID, long _ItemID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_FO_GetPreMBStatusID", _year, _DivisionID, _StructureTypeID, _StructureID, _ItemID);
        }
        public long PreMBStatusInsertion(long _PreMBStatusID, long? _FloodInspectionID, long? _ItemID, int? _LastYrQty, int? _DivStrIssuedQty, int? _AvailableQty, int _CreatedBy, int _ModifiedBy, long _ODIID)
        {
            long _ODIIDOut = 0;
            ContextDB db = new ContextDB();
            DataSet DS = db.ExecuteStoredProcedureDataSet("Proc_FO_PreMBStatusInsertion", _PreMBStatusID, _FloodInspectionID, _ItemID, _LastYrQty, _DivStrIssuedQty, _AvailableQty, _CreatedBy, _ModifiedBy, _ODIID);
            return _ODIID;
        }


        #endregion "Independent Inspection"

        #region Departmental Inspection
        public object GetDepartmentalInspectionByID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetDepartmentalInspectionByID(_FloodInspectionID);
        }


        #region Attachments
        public bool SaveAttachments(FO_DAttachments _DAttachments)
        {
            bool isSaved = false;

            if (_DAttachments.ID == 0)
                db.Repository<FO_DAttachments>().Insert(_DAttachments);
            else
                db.Repository<FO_DAttachments>().Update(_DAttachments);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        public List<object> GetAttachmentsByFloodInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetAttachmentsByFloodInspectionID(_FloodInspectionID);
        }

        public bool DeleteAttachments(long _ID)
        {
            db.Repository<FO_DAttachments>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool AttachmentDublication(long _ID, string _FileName)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.AttachmentDublication(_ID, _FileName);
        }

        #endregion

        #region MemberDetails
        public bool SaveMemberDetails(FO_DMemberDetails _ObjModel)
        {
            bool isSaved = false;

            if (_ObjModel.ID == 0)
                db.Repository<FO_DMemberDetails>().Insert(_ObjModel);
            else
                db.Repository<FO_DMemberDetails>().Update(_ObjModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        public List<object> GetMemberDetailsByFloodInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetMemberDetailsByFloodInspectionID(_FloodInspectionID);
        }

        public bool DeleteMemberDetails(long _ID)
        {
            db.Repository<FO_DMemberDetails>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsDMembersUnique(FO_DMemberDetails _ObjModel)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.IsDMembersUnique(_ObjModel);
        }
        //public long SavePreFloodMBStatus(long _PreMBStatusID, long _FloodInspectionsID, long _ItemID, int _LastYrQty, int _DivStrIssuedQty, int _AvailableQty, int _UserID, int _UserID, 0)
        //{
        //    FO_PreMBStatus _ObjModel = new FO_PreMBStatus();
        //    _ObjModel.FloodInspectionID = _FloodInspectionsID;
        //    _ObjModel.ItemID = _ItemID;
        //    _ObjModel.LastYrQty = _PreviousYearQuantity;
        //    _ObjModel.DivStrIssuedQty = _QuantityIssuedFromDivisionStore;

        //    if (_MBStatusID != 0)
        //    {
        //        _ObjModel = db.Repository<FO_PreMBStatus>().FindById(_MBStatusID);
        //        _ObjModel.ModifiedBy = Convert.ToInt32(_UserID);
        //        _ObjModel.ModifiedDate = System.DateTime.Now;

        //    }
        //    else
        //    {
        //        _ObjModel.CreatedDate = System.DateTime.Now;
        //        _ObjModel.CreatedBy = Convert.ToInt32(_UserID);
        //    }
        //    _ObjModel.FloodInspectionID = _FloodInspectionsID;
        //    _ObjModel.ItemID = _ItemID;
        //    _ObjModel.LastYrQty = _PreviousYearQuantity;
        //    _ObjModel.DivStrIssuedQty = _QuantityIssuedFromDivisionStore;
        //    _ObjModel.AvailableQty = _QuantityAvalible;

        //    bool saved = SavePreMBStatus(_ObjModel);
        //    return _ObjModel.ID;
        //}
        //public List<object> SavePostFloodMBStatus(long _pOverallDivItemID, int _pYear, long _pDivisionID, long? _pItemCategoryID, long? _pItemSubcategoryID, long? _pStructureTypeID, long? _pStructureID, long? _pPreMBStatusID, long? _pFloodInspectionDetailID, int? _pPostAvailableQty, long? _pPostRequiredQty, long? _pCS_CampSiteID, int? _pCS_RequiredQty, int? _pOD_AdditionalQty, long _pODIIDOut, int _UserID)
        //{
        //    //--To DO FO Team


        //    DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_OverallDivItemsInsertion]", _pOverallDivItemID, _pYear, _pDivisionID, _pItemCategoryID, _pItemSubcategoryID, _pStructureTypeID, _pStructureID, _pPreMBStatusID, _pFloodInspectionDetailID, _pPostAvailableQty, _pPostRequiredQty, _pCS_CampSiteID, _pCS_RequiredQty, _pOD_AdditionalQty, _UserID, _UserID, 0);
        //    List<object> lstInfrastructureDetails = (from DataRow dr in dt.Rows
        //                                             select new
        //                                             {
        //                                                 @pODIIDOut = Convert.ToInt32(dr["@pODIIDOut"])

        //                                             }).ToList<object>();
        //    return lstInfrastructureDetails;




        //    return IsExists;




        public long SavePreFloodMBStatus(long _PreMBStatusID, long _FloodInspectionsID, long _ItemID, int _LastYrQty, int _DivStrIssuedQty, int _AvailableQty, int _UserID)
        {
            long _ODIIDOut = 0;

            //            @PreMBStatusID       BIGINT=0,
            //@FloodInspectionID   BIGINT=NULL,
            //@ItemID				BIGINT=NULL,
            //@LastYrQty			INT=NULL,
            //@DivStrIssuedQty		INT=NULL,
            //@AvailableQty		INT=NULL,
            //@CreatedBy			INT=NULL,
            //@ModifiedBy			INT=NULL,
            //@pODIIDOut			Bigint Output

            ContextDB dbADO = new ContextDB();

            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_FO_PreMBStatusInsertion", _PreMBStatusID, _FloodInspectionsID, _ItemID, _LastYrQty, _DivStrIssuedQty, _AvailableQty, _UserID, _UserID, _ODIIDOut);



            if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            {
                DataRow DR = DS.Tables[0].Rows[0];
                _ODIIDOut = Convert.ToInt64(DS.Tables[0].Rows[0]["pODIIDOut"]);
            }




            return _ODIIDOut;
        }




        public long SavePostFloodMBStatus(long _OverallDivItemID, int _Year, long? _DivisionID,
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



            if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            {
                DataRow DR = DS.Tables[0].Rows[0];
                _ODIIDOut = Convert.ToInt64(DS.Tables[0].Rows[0]["pODIIDOut"]);
            }


            return _ODIIDOut;
        }


        public object GET_PostMBStatus_Object(long ID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GET_PostMBStatus_Object(ID);
        }


        public object GET_PreMBStatus_Object(long ID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GET_PreMBStatus_Object(ID);
        }





        public long GetFOPreMbStatusID(int _Year, long _DivisionID, long _StructureTypeID, long _StructureID, long _ItemID)
        {
            //DataTable dt = db.ExecuteStoredProcedureDataTable("[Proc_FO_GetPreMBStatusID]", _Year, _DivisionID, _StructureTypeID, _StructureID);
            //List<object> lstsearchonsitemonitoring = (from DataRow dr in dt.Rows
            //                                          select new

            //                                          {
            //                                              FFPStonePositionID = Convert.ToInt64(dr["FFPStonePositionID"]),
            //                                              CampSiteID = Convert.ToInt64(dr["CampSiteID"]),
            //                                              Year = Convert.ToInt32(dr["Year"]),
            //                                              Zone = dr["Zone"].ToString(),
            //                                              Circle = dr["Circle"].ToString(),
            //                                              Division = dr["Division"].ToString(),
            //                                              InfrastructureType = dr["InfrastructureType"].ToString(),
            //                                              InfrastructureName = dr["InfrastructureName"].ToString()

            //                                          }).ToList<object>();


            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_FO_GetPreMBStatusID", _Year, _DivisionID, _StructureTypeID, _StructureID, _ItemID);

            // long PreMBStatusID = Convert.ToInt64(DS.Tables[0].Rows[0]["PreMBStatusID"]);

            long PreMBStatusID = 0;


            if (DS != null && DS.Tables != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            {
                PreMBStatusID = Convert.ToInt64(DS.Tables[0].Rows[0]["PreMBStatusID"]);
            }
            else
            {
                PreMBStatusID = 0;
            }

            return PreMBStatusID;
        }

        #endregion

        #region Infrastructures
        public List<object> GetInfrastructuresByFloodInspectionID(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetInfrastructuresByFloodInspectionID(_FloodInspectionID);
        }

        public bool SaveInfrastructures(FO_DInfrastructures _ObjModel)
        {
            bool isSaved = false;

            if (_ObjModel.ID == 0)
                db.Repository<FO_DInfrastructures>().Insert(_ObjModel);
            else
                db.Repository<FO_DInfrastructures>().Update(_ObjModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public List<CO_StructureType> GetAllInfrastructuresTypes()
        {
            return db.Repository<CO_StructureType>().GetAll().Where(d => d.IsActive == true).ToList();
        }

        public bool DeleteInfrastructure(long _ID)
        {
            db.Repository<FO_DInfrastructures>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsDInfrastructureUnique(FO_DInfrastructures _ObjModel)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.IsDInfrastructureUnique(_ObjModel);
        }
        #endregion


        #region SearechDepartmentalInspection

        public List<object> GetDepartmentalInspectionSearch(long? _FloodInspectionID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _Status, DateTime? _FromDate, DateTime? _ToDate)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetDepartmentalInspectionSearch(_FloodInspectionID, _ZoneID, _CircleID, _DivisionID, _Status, _FromDate, _ToDate);
        }

        #endregion

        #endregion

        #region JointInspection
        public bool IsJIFInfrastructureExist(FO_JInfrastructures _ObjModel)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.IsJIFInfrastructureExist(_ObjModel);
        }
        public bool IsJIFInfrastructureExistUpdate(FO_JInfrastructures _ObjModel)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.IsJIFInfrastructureExist(_ObjModel);
        }
        public List<object> GetjointInspectionSearch(long? _FloodInspectionID, long? _ZoneID, long? _CircleID, long? _DivisionID, long? _Status, DateTime? _FromDate, DateTime? _ToDate)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetjointInspectionSearch(_FloodInspectionID, _ZoneID, _CircleID, _DivisionID, _Status, _FromDate, _ToDate);
        }
        public FO_FloodInspection GetInspectionDetailsByID(long _FloodInspectionID)
        {
            FO_FloodInspection qFloodInspection = db.Repository<FO_FloodInspection>().GetAll().Where(s => s.ID == _FloodInspectionID).FirstOrDefault();
            return qFloodInspection;
        }
        public bool IsFloodInspectionDependencyExists(long _FloodInspectionID)
        {

            bool qIsExists = db.Repository<FO_JAttachments>().GetAll().Any(s => s.FloodInspectionID == _FloodInspectionID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_JInfrastructures>().GetAll().Any(s => s.FloodInspectionID == _FloodInspectionID);
            }
            if (!qIsExists)
            {
                qIsExists = db.Repository<FO_JMemberDetails>().GetAll().Any(s => s.FloodInspectionID == _FloodInspectionID);
            }
            return qIsExists;
        }
        public bool DeleteJointFloodInspection(long _FloodInspectionID)
        {
            db.Repository<FO_FloodInspection>().Delete(_FloodInspectionID);
            db.Save();

            return true;
        }

        public object GetjointInspectionDetail(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetjointInspectionDetail(_FloodInspectionID);
        }

        public List<object> GetInfrastructuresForJointInspection(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetInfrastructuresForJointInspection(_FloodInspectionID);
        }
        public bool SaveJointInfrastructure(FO_JInfrastructures _ObjJInfrastructures)
        {
            bool isSaved = false;

            if (_ObjJInfrastructures.ID == 0)
                db.Repository<FO_JInfrastructures>().Insert(_ObjJInfrastructures);
            else
                db.Repository<FO_JInfrastructures>().Update(_ObjJInfrastructures);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        public bool DeleteJointInspectionInfrastructure(long _ID)
        {
            db.Repository<FO_JInfrastructures>().Delete(_ID);
            db.Save();

            return true;
        }

        public List<object> GetJointMemberDetails(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetJointMemberDetails(_FloodInspectionID);

        }
        public bool SaveJointMemberDetails(FO_JMemberDetails _ObjJointDetails)
        {
            bool isSaved = false;

            if (_ObjJointDetails.ID == 0)
                db.Repository<FO_JMemberDetails>().Insert(_ObjJointDetails);
            else
                db.Repository<FO_JMemberDetails>().Update(_ObjJointDetails);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        public bool DeleteJointMemberDetails(long _ID)
        {
            db.Repository<FO_JMemberDetails>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsJointMemberDetailsExist(FO_JMemberDetails ObjMemberDetail)
        {
            bool qIsMemberDetailsExist = db.Repository<FO_JMemberDetails>().GetAll().Any(q => q.Name == ObjMemberDetail.Name && q.Department == ObjMemberDetail.Department &&
                            (q.ID != ObjMemberDetail.ID || ObjMemberDetail.ID == 0));
            return qIsMemberDetailsExist;
        }

        public List<object> GetJointAttachmentsDetails(long _FloodInspectionID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetJointAttachmentsDetails(_FloodInspectionID);
        }
        public bool DeleteJointAttachments(long _ID)
        {
            db.Repository<FO_JAttachments>().Delete(_ID);
            db.Save();

            return true;
        }
        public bool SaveJointAttachments(FO_JAttachments _ObjModel)
        {
            bool isSaved = false;

            if (_ObjModel.ID == 0)
                db.Repository<FO_JAttachments>().Insert(_ObjModel);
            else
                db.Repository<FO_JAttachments>().Update(_ObjModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        #endregion JointInspection



        public object GetFIDetailIDbyFIID(long FDID)
        {
            FloodInspectionRepository repFloodInspection = this.db.ExtRepositoryFor<FloodInspectionRepository>();
            return repFloodInspection.GetFIDetailIDbyFIID(FDID);
        }
        public IEnumerable<DataRow> FO_LoadStonePosition(long _FloodInspectionID)
        {
            return db.ExecuteDataSet("Proc_FO_FIStonePosition", _FloodInspectionID);
        }

        /// <summary>
        /// This method return list of All Inspection Conditions 
        /// Created on 13-Oct-2016
        /// </summary>
        /// <param name="_CoditionGroup"></param>
        /// <returns>List<FO_InspectionConditions></returns>
        public List<FO_InspectionConditions> GetAllInspectionConditions()
        {
            return db.Repository<FO_InspectionConditions>().GetAll().Where(s => s.IsActive == true).OrderBy(s => s.CoditionGroup).ToList<FO_InspectionConditions>();
        }


        #region Notification

        public FO_GetFloodInspectionNotifyData_Result GetFloodInspectionNotifyData(long _FloodInspectionID)
        {
            return db.ExtRepositoryFor<FloodInspectionRepository>().GetFloodInspectionNotifyData(_FloodInspectionID);
        }

        public long GetFloodInspectionCreatedUserIDByID(long _FloodInspectionID)
        {
            return db.Repository<FO_FloodInspection>().GetAll().Where(s => s.ID== _FloodInspectionID).FirstOrDefault().CreatedBy;
        }

        public short? GetFloodInspectionCategoryByID(long _FloodInspectionID)
        {
            return db.Repository<FO_FloodInspection>().GetAll().Where(s => s.ID == _FloodInspectionID).FirstOrDefault().InspectionCategoryID;
        }

        #endregion  Notification

    }
}