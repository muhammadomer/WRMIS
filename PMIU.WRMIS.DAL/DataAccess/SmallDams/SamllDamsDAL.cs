using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.SmallDams;
using System.Data;
using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.Channel;

namespace PMIU.WRMIS.DAL.DataAccess.SmallDams
{
    public class SmallDamsDAL
    {
        #region Initialize
        public bool IsGaugeValueAdded = false;
        public SmallDamsDAL()
        {

        }

        ContextDB db = new ContextDB();

        #endregion
        #region UserControls
        public object GetDamDetails(Int64? _DAMID)
        {
            return db.ExtRepositoryFor<SmallDamsRepository>().GetDamDetails(_DAMID);
        }

        public object GetDamReadingsData(Int64? _DAMID)
        {
            return db.ExtRepositoryFor<SmallDamsRepository>().GetDamReadingsData(_DAMID);
        }
        
        public object GetDamChannels(Int64? _ID)
        {
            return db.ExtRepositoryFor<SmallDamsRepository>().GetDamChannels(_ID);
        }
        public object GetDamInfo(Int64? _DAMID)
        {
            return db.ExtRepositoryFor<SmallDamsRepository>().GetDamInfo(_DAMID);
        }

        #endregion
        #region SearchDam
        public UA_AssociatedLocation GetSubDivisionByUserID(long _UserID, long? _IrrigationLevelID)
        {
            UA_AssociatedLocation mdlSubDivision = db.Repository<UA_AssociatedLocation>().GetAll().Where(x => x.UserID == _UserID && x.IrrigationLevelID == _IrrigationLevelID).FirstOrDefault();
            return mdlSubDivision;
        }

        public List<object> GetDamTypeSearch(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, long _UserID, long? _IrrigationLevelID)
        {

            return db.ExtRepositoryFor<SmallDamsRepository>().GetDamTypeSearch(_DamID, _DivisionID, _SubDivisionID, _UserID, _IrrigationLevelID);
        }

        
        public bool IsSmallDamDependencyExists(Int64 _SmallDamID)
        {
            //bool dDamTechPara = db.Repository<SD_DamTechPara>().GetAll().Any(s => s.SmallDamID == _SmallDamID);
            bool dOMCost = db.Repository<SD_OMCost>().GetAll().Any(s => s.SmallDamID == _SmallDamID);
            bool dSmallChannel = db.Repository<SD_SmallChannel>().GetAll().Any(s => s.SmallDamID == _SmallDamID);
            bool dSmallDamData = db.Repository<SD_SmallDamData>().GetAll().Any(s => s.SmallDamID == _SmallDamID);

            if (dOMCost == true || dSmallChannel == true || dSmallDamData == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteSmallDam(long _ID)
        {
            bool isDelete = false;
            SD_DamTechPara tecPara =  db.Repository<SD_DamTechPara>().GetAll().Where(t => t.SmallDamID == _ID).FirstOrDefault();
            if (tecPara != null)
            {
                db.Repository<SD_DamTechPara>().Delete(tecPara.ID);    
            }
            db.Repository<SD_SmallDam>().Delete(_ID);

            db.Save();
            isDelete = true;

            return isDelete;
        }

        public bool ISDamExist(SD_SmallDam _Dam)
        {
            //return db.Repository<SD_SmallDam>().GetAll().Where(d => d.Name == _Dam.Name).Any();
            return db.ExtRepositoryFor<SmallDamsRepository>().ISDamExist(_Dam);
        }

        #region Dropdown
        public List<SD_SmallDam> GetDamNameBySubDivisionID(Int64 _SubDivisionID)
        {

            return db.Repository<SD_SmallDam>().GetAll().Where(d => d.IsActive == true && (_SubDivisionID == -1 ? d.SubDivID == d.SubDivID : d.SubDivID == _SubDivisionID)).ToList();
            //return db.ExtRepositoryFor<SmallDamsRepository>().GetDamNameBySubDivisionID(_SubDivisionID);
        }
        public List<CO_Division> GetSDDivisionsByUserID(long _UserID, long _IrrigationLevelID, bool? _IsActive = null)
        {
            return db.ExtRepositoryFor<SmallDamsRepository>().GetSDDivisionsByUserID(_UserID, _IrrigationLevelID, _IsActive);
        }

        public List<CO_Division> GetSDDivisionsByUserAndDamID(long _UserID,long _DamID, long _IrrigationLevelID, bool? _IsActive = null)
        {
            return db.ExtRepositoryFor<SmallDamsRepository>().GetSDDivisionsByUserAndDamID(_UserID,_DamID, _IrrigationLevelID, _IsActive);
        }
        

        public List<CO_District> GetAllSDDistricts(long _DivisionID)
        {

            return db.ExtRepositoryFor<SmallDamsRepository>().GetAllSDDistricts(_DivisionID);
        }

        public List<CO_Tehsil> GetTehsilByDistrictID(long _DistrictID)
        {
            List<CO_Tehsil> lstVillage = db.Repository<CO_Tehsil>().GetAll().Where(x => x.DistrictID == _DistrictID).OrderBy(x => x.Name).ToList<CO_Tehsil>();
            return lstVillage;
        }

        public List<CO_Village> GetVillagesByTehsilID(long _TehsilID)
        {
            List<CO_Village> lstVillage = db.Repository<CO_Village>().GetAll().Where(x => x.TehsilID == _TehsilID).OrderBy(x => x.Name).ToList<CO_Village>();
            return lstVillage;
        }


        public List<SD_SmallDam> GetDamNameByDamID(Int64 _DamID)
        {

            return db.Repository<SD_SmallDam>().GetAll().Where(d => d.IsActive == true && d.ID == _DamID).ToList();
        }

        public List<SD_SmallChannel> GetChannelByDamID(Int64 _DamID)
        {

            return db.Repository<SD_SmallChannel>().GetAll().Where(d => d.IsActive == true && d.SmallDamID == _DamID).ToList();
        }
        #endregion
        
        
        #endregion
        
        #region AddNewDam
        public List<SD_DamType> GetAllActiveDamType()
        {
            return db.Repository<SD_DamType>().GetAll().Where(d => d.IsActive == true).ToList();
        }

        public bool SaveSmallDam(SD_SmallDam _DamTypeModel)
        {
            bool isSaved = false;

            if (_DamTypeModel.ID == 0)
                db.Repository<SD_SmallDam>().Insert(_DamTypeModel);
            else
                db.Repository<SD_SmallDam>().Update(_DamTypeModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public object GetSmallDamByID(Int64 _DamID)
        {
            //SmallDamsRepository DamRep = this.db.ExtRepositoryFor<SmallDamsRepository>();
            //return DamRep.GetSmallDamByID(_DamID);
            return db.ExtRepositoryFor<SmallDamsRepository>().GetSmallDamByID(_DamID);
        }
        public bool IsDamExists(SD_SmallDam _SmallDam, SD_DamTechPara DamTechPara)
        {
            //DataTable dt = db.ExecuteStoredProcedureDataTable("FO_IsFloodInspectionExistsWRTYear", _FloodInspection.DivisionID, _FloodInspection.Year, _FloodInspection.InspectionDate, FloodInspectionDetail.InspectionTypeID, FloodInspectionDetail.StructureTypeID, FloodInspectionDetail.StructureID, _FloodInspection.ID);
            bool IsExists = false;
            //foreach (DataRow DR in dt.Rows)
            //{
            //    IsExists = Convert.ToBoolean(DR["IsExists"].ToString());
            //}

            return IsExists;
        }

        #endregion
        
        #region VillageType
        public List<object> GetVillageBenefitted(Int64 _ChannelID)
        {

            return db.ExtRepositoryFor<SmallDamsRepository>().GetVillageBenefitted( _ChannelID);
        }

        public bool DeleteVillageType(Int64 _ID)
        {
            db.Repository<SD_Village>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsVillageTypeUnique(SD_Village _VillageTypeModel)
        {
            SmallDamsRepository repSmallVillages = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repSmallVillages.IsVillageTypeUnique(_VillageTypeModel);
        }
        public bool SaveVillageType(SD_Village _VillageTypeModel)
        {
            bool isSaved = false;

            if (_VillageTypeModel.ID == 0)
                db.Repository<SD_Village>().Insert(_VillageTypeModel);
            else
                db.Repository<SD_Village>().Update(_VillageTypeModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        #endregion
        
        #region Channels
        public List<object> GetChannels(Int64 _SmallDamID)
        {
            SmallDamsRepository repChannels = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repChannels.GetChannels(_SmallDamID);
        }

        public object GetChannelsByID(Int64 _SmallDamID, Int64 _ChannelID)
        {
            SmallDamsRepository repChannels = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repChannels.GetChannelsByID(_SmallDamID,_ChannelID);
        }
        

        public bool DeleteChannels(Int64 _ID)
        {
            db.Repository<SD_SmallChannel>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsChannelsUnique(SD_SmallChannel _ChannelsModel)
        {
            SmallDamsRepository repSmallVillages = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repSmallVillages.IsChannelsUnique(_ChannelsModel);
        }

        public bool ISChannelDependancyExits(Int64 _ID)
        {
            bool ChannelExit = false;
            bool cdata = db.Repository<SD_SmallChannelData>().GetAll().Where(c => c.SmallChannelID == _ID).Any();
            bool village = db.Repository<SD_Village>().GetAll().Where(v => v.SmallChannelID == _ID).Any();
            if (cdata)
                ChannelExit = true;
            
            if (village)
                ChannelExit = true;

            return ChannelExit;

        }
        
        public bool SaveChannels(SD_SmallChannel _ChannelsModel)
        {
            bool isSaved = false;

            if (_ChannelsModel.ID == 0)
                db.Repository<SD_SmallChannel>().Insert(_ChannelsModel);
            else
                db.Repository<SD_SmallChannel>().Update(_ChannelsModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        #endregion
        
        #region OMCost
        public List<object> GetOMCost(Int64 _SmallDamID)
        {
            SmallDamsRepository repOMCost = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repOMCost.GetOMCost(_SmallDamID);
        }

        public bool DeleteOMCost(Int64 _ID)
        {
            db.Repository<SD_OMCost>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsOMCostUnique(SD_OMCost _OMCostModel)
        {
            SmallDamsRepository repSmallVillages = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repSmallVillages.IsOMCostUnique(_OMCostModel);
        }
        public bool SaveOMCost(SD_OMCost _OMCostModel)
        {
            bool isSaved = false;

            if (_OMCostModel.ID == 0)
                db.Repository<SD_OMCost>().Insert(_OMCostModel);
            else
                db.Repository<SD_OMCost>().Update(_OMCostModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        #endregion
        
        #region Technical
         public List<SD_SpillwayType> GetAllSpillwayType()
        {
            return db.Repository<SD_SpillwayType>().GetAll().Where(d => d.IsActive == true).ToList();
        }

         public object GetTechParaByID(Int64 _SmallDamID, Int64 _TechParaID)
         {
             //SmallDamsRepository DamRep = this.db.ExtRepositoryFor<SmallDamsRepository>();
             //return DamRep.GetSmallDamByID(_DamID);
             return db.ExtRepositoryFor<SmallDamsRepository>().GetTechParaByID(_SmallDamID,_TechParaID);
         }


         public bool SaveTechPara(SD_DamTechPara _DamTechPara)
         {
             bool isSaved = false;

             if (_DamTechPara.ID == 0)
                 db.Repository<SD_DamTechPara>().Insert(_DamTechPara);
             else
                 db.Repository<SD_DamTechPara>().Update(_DamTechPara);

             db.Save();
             isSaved = true;

             return isSaved;
         }
        #endregion
        
        #region ReferenceData

        #region DamType
        public List<object> GetDamType()
        {
            SmallDamsRepository repDamType = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repDamType.GetDamType();
        }

        public bool DeleteDamType(Int16 _ID)
        {
            db.Repository<SD_DamType>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsDamTypeUnique(SD_DamType _DamTypeModel)
        {
            SmallDamsRepository repSmallDams = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repSmallDams.IsDamTypeUnique(_DamTypeModel);
        }
        public bool SaveDamType(SD_DamType _DamTypeModel)
        {
            bool isSaved = false;

            if (_DamTypeModel.ID == 0)
                db.Repository<SD_DamType>().Insert(_DamTypeModel);
            else
                db.Repository<SD_DamType>().Update(_DamTypeModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        #endregion

        #region SpillwayType
        public List<object> GetSpillwayType()
        {
            SmallDamsRepository repSpillwayType = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repSpillwayType.GetSpillwayType();
        }

        public bool DeleteSpillwayType(Int16 _ID)
        {
            db.Repository<SD_SpillwayType>().Delete(_ID);
            db.Save();

            return true;
        }

        public bool IsSpillwayTypeUnique(SD_SpillwayType _SpillwayTypeModel)
        {
            SmallDamsRepository repSmallSpillways = this.db.ExtRepositoryFor<SmallDamsRepository>();
            return repSmallSpillways.IsSpillwayTypeUnique(_SpillwayTypeModel);
        }
        public bool SaveSpillwayType(SD_SpillwayType _SpillwayTypeModel)
        {
            bool isSaved = false;

            if (_SpillwayTypeModel.ID == 0)
                db.Repository<SD_SpillwayType>().Insert(_SpillwayTypeModel);
            else
                db.Repository<SD_SpillwayType>().Update(_SpillwayTypeModel);

            db.Save();
            isSaved = true;

            return isSaved;
        }
        #endregion

        #endregion
        
        #region Reading
        #region SearchReadings


        public List<SDChannelReading1_Result> GetChannelReading(Int64? _ID, Int64? _DivisionID, Int64? _SubDivisionID, DateTime _ReadingDate)
        {

            return db.ExtRepositoryFor<SmallDamsRepository>().GetChannelReading(_ID, _DivisionID, _SubDivisionID, _ReadingDate);
        }

        public SDDamReading_Result GetDamReading(Int64? _ID, Int64? _DivisionID, Int64? _SubDivisionID, DateTime _ReadingDate)
        {

            return db.ExtRepositoryFor<SmallDamsRepository>().GetDamReading(_ID, _DivisionID, _SubDivisionID, _ReadingDate);
        }



        public object GetSmallDamReadingByID(Int64 _DamDataID)
        {
            return db.ExtRepositoryFor<SmallDamsRepository>().GetSmallDamReadingByID(_DamDataID);
        }

        public bool SaveSmallDamReading(SD_SmallDamData _SmallDamData)
        {
            bool isSaved = false;

            if (_SmallDamData.ID == 0)
                db.Repository<SD_SmallDamData>().Insert(_SmallDamData);
            else
                db.Repository<SD_SmallDamData>().Update(_SmallDamData);

            db.Save();
            isSaved = true;

            return isSaved;
        }

        public Int64 DamIsExits(Int64 _DamID, DateTime _ReadingDate) 
        {
            return db.ExtRepositoryFor<SmallDamsRepository>().DamIsExits(_DamID, _ReadingDate);
        
        }

        public bool SaveChannelData(SD_SmallChannelData _SmallChannelData)
        {
            bool isSaved = false;

            if (_SmallChannelData.ID == 0)
                db.Repository<SD_SmallChannelData>().Insert(_SmallChannelData);
            else
                db.Repository<SD_SmallChannelData>().Update(_SmallChannelData);

            db.Save();
            isSaved = true;

            return isSaved;
        }


        public bool SaveChannelData(List<SD_SmallChannelData> _lstSmallChannelData)
        {
            bool isSaved = false;

            foreach (SD_SmallChannelData mdlSmallDam in _lstSmallChannelData)
            {
                if (mdlSmallDam.ID == 0)
                    db.Repository<SD_SmallChannelData>().Insert(mdlSmallDam);
                else
                    db.Repository<SD_SmallChannelData>().Update(mdlSmallDam);
            }

            db.Save();
            isSaved = true;

            return isSaved;
        }



        #endregion
        #region ViewReadings
        public List<SDViewChannelReading_Result> GetChannelReadingView(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, Int64? _ChannelID, DateTime _FromDate, DateTime _ToDate)
        {

            return db.ExtRepositoryFor<SmallDamsRepository>().GetChannelReadingView(_DamID, _DivisionID, _SubDivisionID, _ChannelID, _FromDate, _ToDate);
        }
        public List<SDViewDamReading_Result> GetDamReadingView(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID,  DateTime _FromDate, DateTime _ToDate)
        {

            return db.ExtRepositoryFor<SmallDamsRepository>().GetDamReadingView(_DamID, _DivisionID, _SubDivisionID, _FromDate, _ToDate);
        }
        #endregion
        #endregion
    }
}
