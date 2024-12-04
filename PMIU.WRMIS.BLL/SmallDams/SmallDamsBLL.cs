using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.SmallDams;
using PMIU.WRMIS.DAL.DataAccess.SmallDams;
using System.Data;

namespace PMIU.WRMIS.BLL.SmallDams
{
    public class SmallDamsBLL : BaseBLL
    {
        #region Initialize
        SmallDamsDAL smallDamsDal = new SmallDamsDAL();
        #endregion

        public bool IsGaugeValueAdded = true;

        #region UserControls
        public object GetDamNametype(Int64? _DAMID)
        {
            return smallDamsDal.GetDamDetails(_DAMID);
        }

        public object GetDamReadingsData(Int64? _DAMID)
        {
            return smallDamsDal.GetDamReadingsData(_DAMID);
        }
        
        public object GetDamChannels(Int64? _ID)
        {
            return smallDamsDal.GetDamChannels(_ID);
        }

        public object GetDamInfo(Int64? _DAMID)
        {
            return smallDamsDal.GetDamInfo(_DAMID);
        }
        #endregion
        #region SearchDam
        public UA_AssociatedLocation GetSubDivisionByUserID(long _UserID, long? _IrrigationLevelID)
        {

            return smallDamsDal.GetSubDivisionByUserID(_UserID, _IrrigationLevelID);
        }

        public List<object> GetDamTypeSearch(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, long _UserID, long? _IrrigationLevelID)
        {
            return smallDamsDal.GetDamTypeSearch(_DamID, _DivisionID, _SubDivisionID, _UserID, _IrrigationLevelID);
        }
        public bool IsSmallDamDependencyExists(Int64 _SmallDamID)
        {
            return smallDamsDal.IsSmallDamDependencyExists(_SmallDamID);
        }
        public bool DeleteSmallDam(long _ID)
        {
            return smallDamsDal.DeleteSmallDam(_ID);
        }

        public bool ISDamExist(SD_SmallDam _Dam)
        {
            return smallDamsDal.ISDamExist(_Dam);
        }
        
        #endregion

        #region Dropdown
        public List<SD_SmallDam> GetDamNameBySubDivisionID(Int64 _SubDivisionID)
        {

            return smallDamsDal.GetDamNameBySubDivisionID(_SubDivisionID);
        }

        public List<CO_Division> GetSDDivisionsByUserID(long _UserID, long _IrrigationLevelID, bool? _IsActive = null)
        {
            return smallDamsDal.GetSDDivisionsByUserID(_UserID, _IrrigationLevelID, _IsActive);
        }

        public List<CO_Division> GetSDDivisionsByUserAndDamID(long _UserID, long _DamID, long _IrrigationLevelID, bool? _IsActive = null)
        {
            return smallDamsDal.GetSDDivisionsByUserAndDamID(_UserID,_DamID, _IrrigationLevelID, _IsActive);
        }
        
        public List<CO_District> GetAllSDDistricts(long _DivisionID)
        {

            return smallDamsDal.GetAllSDDistricts(_DivisionID);
        }

        public List<CO_Tehsil> GetAllSDTehsil(long _DistrictID)
        {

            return smallDamsDal.GetTehsilByDistrictID(_DistrictID);
        }
        public List<CO_Village> GetAllSDVillages(long _TehsilID)
        {

            return smallDamsDal.GetVillagesByTehsilID(_TehsilID);
        }

        public List<SD_SmallDam> GetDamNameByDamID(Int64 _DamID)
        {

            return smallDamsDal.GetDamNameByDamID(_DamID);
        }
        public List<SD_SmallChannel> GetChannelByDamID(Int64 _DamID)
        {

            return smallDamsDal.GetChannelByDamID(_DamID);
        }
        #endregion
        #region AddNewDam

        public List<SD_DamType> GetAllActiveDamType()
        {
            return smallDamsDal.GetAllActiveDamType();
        }
        public bool SaveSmallDam(SD_SmallDam _DamType)
        {
            return smallDamsDal.SaveSmallDam(_DamType);
        }

        public object GetSmallDamByID(Int64 _DamID)
        {
            return smallDamsDal.GetSmallDamByID(_DamID);
        }
        public bool IsDamExists(long _DamID)
        {
            return false;//smallDamsDal.IsDamExists(_DamID);
        }

        #endregion

        #region Channels


        public List<object> GetChannels(Int64 _SmallDamID)
        {
            return smallDamsDal.GetChannels(_SmallDamID);
        }

        public object GetChannelsByID(Int64 _SmallDamID, Int64 _ChannelID)
        {
            return smallDamsDal.GetChannelsByID(_SmallDamID, _ChannelID);
        }


        public bool DeleteChannels(Int64 _ID)
        {
            return smallDamsDal.DeleteChannels(_ID);
        }

        public bool IsChannelsUnique(SD_SmallChannel _ObjModel)
        {
            return smallDamsDal.IsChannelsUnique(_ObjModel);
        }

        public bool SaveChannels(SD_SmallChannel _Channels)
        {
            return smallDamsDal.SaveChannels(_Channels);
        }


        public bool ISChannelDependancyExits(Int64 _ID)
        {
            return smallDamsDal.ISChannelDependancyExits(_ID);
        }
        #endregion

        #region OMCost
        public List<object> GetOMCost(Int64 _SmallDamID)
        {
            return smallDamsDal.GetOMCost(_SmallDamID);
        }

        public bool DeleteOMCost(Int64 _ID)
        {
            return smallDamsDal.DeleteOMCost(_ID);
        }

        public bool IsOMCostUnique(SD_OMCost _ObjModel)
        {
            return smallDamsDal.IsOMCostUnique(_ObjModel);
        }

        public bool SaveOMCost(SD_OMCost _OMCost)
        {
            return smallDamsDal.SaveOMCost(_OMCost);
        }
        #endregion
        #region Villages
        public List<object> GetVillageBenefitted(Int64 _ChannelID)
        {
            return smallDamsDal.GetVillageBenefitted(_ChannelID);
        }
        public bool DeleteVillageType(Int64 _ID)
        {
            return smallDamsDal.DeleteVillageType(_ID);
        }

        public bool IsVillageTypeUnique(SD_Village _ObjModel)
        {
            return smallDamsDal.IsVillageTypeUnique(_ObjModel);
        }

        public bool SaveVillageType(SD_Village _VillageType)
        {
            return smallDamsDal.SaveVillageType(_VillageType);
        }
        #endregion




        #region Technical
        public List<SD_SpillwayType> GetAllSpillwayType()
        {
            return smallDamsDal.GetAllSpillwayType();
        }

        public object GetTechParaByID(Int64 _SmallDamID, Int64 _TechParaID)
        {
            return smallDamsDal.GetTechParaByID(_SmallDamID, _TechParaID);
        }

        public bool SaveTechPara(SD_DamTechPara _DamTechPara)
        {
            return smallDamsDal.SaveTechPara(_DamTechPara);
        }
        #endregion

        #region ReferenceData

        #region DamType
        public List<object> GetDamType()
        {
            return smallDamsDal.GetDamType();
        }
        public bool DeleteDamType(Int16 _ID)
        {
            return smallDamsDal.DeleteDamType(_ID);
        }

        public bool IsDamTypeUnique(SD_DamType _ObjModel)
        {
            return smallDamsDal.IsDamTypeUnique(_ObjModel);
        }

        public bool SaveDamType(SD_DamType _DamType)
        {
            return smallDamsDal.SaveDamType(_DamType);
        }
        #endregion
        #region SpillwayType
        public List<object> GetSpillwayType()
        {
            return smallDamsDal.GetSpillwayType();
        }
        public bool DeleteSpillwayType(Int16 _ID)
        {
            return smallDamsDal.DeleteSpillwayType(_ID);
        }

        public bool IsSpillwayTypeUnique(SD_SpillwayType _ObjModel)
        {
            return smallDamsDal.IsSpillwayTypeUnique(_ObjModel);
        }
        public bool SaveSpillwayType(SD_SpillwayType _SpillwayType)
        {
            return smallDamsDal.SaveSpillwayType(_SpillwayType);
        }

        #endregion
        #endregion

        #region Readings

        #region SearchReadings
        public List<SDChannelReading1_Result> GetChannelReading(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, DateTime _ReadingDate)
        {
            return smallDamsDal.GetChannelReading(_DamID, _DivisionID, _SubDivisionID, _ReadingDate);
        }

        public SDDamReading_Result GetDamReading(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, DateTime _ReadingDate)
        {
            return smallDamsDal.GetDamReading(_DamID, _DivisionID, _SubDivisionID, _ReadingDate);
        }

        public object GetSmallDamReadingByID(Int64 _DamDataID)
        {
            return smallDamsDal.GetSmallDamReadingByID(_DamDataID);
        }

        public bool SaveSmallDamReading(SD_SmallDamData _SmallDamData)
        {
            return smallDamsDal.SaveSmallDamReading(_SmallDamData);
        }

        public Int64 DamIsExits(Int64 _DamID, DateTime _ReadingDate)
        {
            return smallDamsDal.DamIsExits(_DamID, _ReadingDate);
        }

        public bool SaveChannelData(SD_SmallChannelData _SmallChannelData)
        {
            return smallDamsDal.SaveChannelData(_SmallChannelData);
        }

        public bool SaveChannelData(List<SD_SmallChannelData> _lstSmallChannelData)
        {
            return smallDamsDal.SaveChannelData(_lstSmallChannelData);
        }


        #endregion
        #region ViewReadings
        public List<SDViewChannelReading_Result> GetChannelReadingView(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, Int64? _ChannelID, DateTime _FromDate, DateTime _ToDate)
        {
            return smallDamsDal.GetChannelReadingView(_DamID, _DivisionID, _SubDivisionID, _ChannelID, _FromDate, _ToDate);
        }

        public List<SDViewDamReading_Result> GetDamReadingView(Int64? _DamID, Int64? _DivisionID, Int64? _SubDivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            return smallDamsDal.GetDamReadingView(_DamID, _DivisionID, _SubDivisionID, _FromDate, _ToDate);
        }
        #endregion

        #endregion

    }
}
