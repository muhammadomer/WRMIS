using PMIU.WRMIS.DAL.DataAccess.RotationalProgram;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.RotaionalProgram
{
    public class RotationalProgramBLL : BaseBLL
    {
        RotationalProgramDAL DALRotationalProgram = new RotationalProgramDAL();

        #region Search Plan
        public List<object> GetYears()
        {
            return DALRotationalProgram.GetYears();
        }

        public List<dynamic> SearchRotationalPlan(long _CircleID, long _DivisionID, long _SubDivisionID, string _Year, long _SeasonID, long _UserIrrigationLevelID)
        {
            return DALRotationalProgram.SearchRotationalPlan(_CircleID, _DivisionID, _SubDivisionID, _Year, _SeasonID, _UserIrrigationLevelID);
        }

        public long DeleteProgram(long _RPID)
        {
            return DALRotationalProgram.DeleteProgram(_RPID);
        }

        public long ApproveDraft(long _RPID)
        {
            return DALRotationalProgram.ApproveDraft(_RPID);
        }


        #endregion

        #region Add Plan

        public long AddPlan(RP_RotationalProgram _ObjSave)
        {
            return DALRotationalProgram.AddPlan(_ObjSave);
        }

        public object GetBasicDetail(long _ID)
        {
            return DALRotationalProgram.GetBasicDetail(_ID);
        }

        public List<long?> GetEnteringChannels(long _RPID)
        {
            return DALRotationalProgram.GetEnteringChannels(_RPID);
        }

        public List<object> GetDivisionsAgainstChannelID(long _CircleID, string _Group)
        {
            return DALRotationalProgram.GetDivisionsAgainstChannelID(_CircleID, _Group);
        }

        public List<object> GetChannelsAgainstDivision(long _DivisionID, long _DivisionSubDivisionID, List<long?> _lstChnlID, bool _PostBack)
        {
            return DALRotationalProgram.GetChannelsAgainstDivision(_DivisionID, _DivisionSubDivisionID, _lstChnlID, _PostBack);
        }

        public List<object> GetEnteringChannelsAgainstDivision(long _DivisionSubDivisionID, long _Designation)
        {
            return DALRotationalProgram.GetEnteringChannelsAgainstDivision(_DivisionSubDivisionID, _Designation);
        }

        public bool SaveDivisions(List<long> _lstIDs, long _RPID, int _UserID, int _NoOfGroups)
        {
            return DALRotationalProgram.SaveDivisions(_lstIDs, _RPID, _UserID, _NoOfGroups);
        }

        public List<dynamic> GetCircleGridData(long _RPID)
        {
            return DALRotationalProgram.GetCircleGridData(_RPID);
        }

        public bool SaveCirclePreferences(RP_Rotation_Circle _ObjSave)
        {
            return DALRotationalProgram.SaveCirclePreferences(_ObjSave);
        }

        public List<RP_Division> GetProgramMappedDivisions(long _RPID)
        {
            return DALRotationalProgram.GetProgramMappedDivisions(_RPID);
        }

        public string SaveChannels(List<dynamic> _lstChannels, long _RPID, int _UserID, int _NoOfGrps)
        {
            return DALRotationalProgram.SaveChannels(_lstChannels, _RPID, _UserID, _NoOfGrps);
        }

        public List<RP_Channel> GetProgramMappedChannels(long _RPID)
        {
            return DALRotationalProgram.GetProgramMappedChannels(_RPID);
        }

        public List<dynamic> GetDivSubDivPreferenceData(long _RPID)
        {
            return DALRotationalProgram.GetDivSubDivPreferenceData(_RPID);
        }

        public List<int> GetActualSubGroups(long _RPID, int _MaxGroups, int _MaxSubGroups)
        {
            return DALRotationalProgram.GetActualSubGroups(_RPID, _MaxGroups, _MaxSubGroups);
        }

        public bool SaveDivSubDivPreference(RP_Rotation_DivisionSubDivision _objSave)
        {
            return DALRotationalProgram.SaveDivSubDivPreference(_objSave);
        }

        public bool DeleteDivSubDivPref(long _ID)
        {
            return DALRotationalProgram.DeleteDivSubDivPref(_ID);
        }

        public bool DeleteCirclePref(long _ID)
        {
            return DALRotationalProgram.DeleteCirclePref(_ID);
        }

        public long GetSubGroupsOfGroup(long _RPID, long _GroupID)
        {
            return DALRotationalProgram.GetSubGroupsOfGroup(_RPID, _GroupID);
        }

        public bool? GetPriority(long _RPID)
        {
            return DALRotationalProgram.GetPriority(_RPID);
        }

        public string GetZoneName(long _ZoneID)
        {
            return DALRotationalProgram.GetZoneName(_ZoneID);
        }

        public string GetPlanCount(long _IrrigationLevelID, long _IrrigationBoundryID, long _SeasonID, string _Year)
        {
            return DALRotationalProgram.GetPlanCount(_IrrigationLevelID, _IrrigationBoundryID, _SeasonID, _Year);
        }

        public string GetAttachmentfiles(long _RPID)
        {
            return DALRotationalProgram.GetAttachmentfiles(_RPID);
        }

        public bool SaveAttachments(RP_Attachment _ObjSave)
        {
            return DALRotationalProgram.SaveAttachments(_ObjSave);
        }

        public bool AddEnteringchannels(List<RP_EnteringChannel> _lstEC)
        {
            return DALRotationalProgram.AddEnteringchannels(_lstEC);
        }

        public bool AddClosureDates(long _ProgramID, DateTime _StartDate, DateTime _EndDate)
        {
            return DALRotationalProgram.AddClosureDates(_ProgramID, _StartDate, _EndDate);
        }

        public bool AddClosureDatesCircle(long _ProgramID, DateTime _StartDate, DateTime _EndDate)
        {
            return DALRotationalProgram.AddClosureDatesCircle(_ProgramID, _StartDate, _EndDate);
        }

        #endregion

        #region Export
        public dynamic GetBasicInfoForSeExport(long _ID)
        {
            return DALRotationalProgram.GetBAsicInfoForSEExport(_ID);
        }
        public dynamic GetBasicInfoForXENSDOExport(long _ID)
        {
            return DALRotationalProgram.GetBAsicInfoForXENSDOExport(_ID);
        }
        public List<dynamic> GetDivisionsByGroups(long _ID)
        {
            return DALRotationalProgram.GetDivisionsByGroups(_ID);
        }
        public List<dynamic> GetWaraInfoForSEExport(long _ID, string _ClosureStartDate, string _ClosureEndDate)
        {
            return DALRotationalProgram.GetWaraInfoForSEExport(_ID, _ClosureStartDate, _ClosureEndDate);
        }
        public List<dynamic> GetChannelsBySubGroups(long _ID)
        {
            return DALRotationalProgram.GetChannelsBySubGroups(_ID);
        }

        public List<dynamic> GetChannelsPreferences(List<dynamic> _WaraData)
        {
            return DALRotationalProgram.GetChannelsPreferences(_WaraData);
        }
        public List<dynamic> GetChannelsWaraData(long _ID, string _ClosureStartDate, string _ClosureEndDate)
        {
            return DALRotationalProgram.GetChannelsWaraData(_ID, _ClosureStartDate, _ClosureEndDate);
        }
        #endregion

        #region View and Print

        public List<dynamic> GetGroupsSubGroupsChannels(long _ID)
        {
            return DALRotationalProgram.GetGroupsSubGroupsChannels(_ID);
        }

        public string GetSubGroupNameByID(int _ID)
        {
            return DALRotationalProgram.GetSubGroupNameByID(_ID);
        }
        public dynamic GetProgramNameAndDate(long _ID, string _Value)
        {
            return DALRotationalProgram.GetProgramNameAndDates(_ID, _Value);
        }
        public List<dynamic> GetPreferencesDataForView(long _RPID)
        {
            return DALRotationalProgram.GetPreferencesDataForView(_RPID);
        }
        public List<dynamic> GetPreferencesDataDivisionForView(long _RPID)
        {
            return DALRotationalProgram.GetPreferencesDataDivisionForView(_RPID);
        }
        public DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            return DALRotationalProgram.ToDataTable(items);
        }
        public long GetIrrigationLevelIDByRPID(long _RPID)
        {
            return DALRotationalProgram.GetIrrigationLevelByRPID(_RPID);
        }
        #endregion

        #region Model integration

        public List<List<dynamic>> GetWaraDates(long _RPID)
        {
            return DALRotationalProgram.GetWaraDates(_RPID);
        }

        public void AdjustDateDifference(long _RPID, DateTime _DiffstartDate, int _DaysDiff)
        {
            DALRotationalProgram.AdjustDateDifference(_RPID, _DiffstartDate, _DaysDiff);
        }

        public bool GetPlanImplementation(long _RPID, double _MaxPrecentage, double _MinPercentage, DateTime _FromDate, DateTime _ToDate)
        {
            return DALRotationalProgram.GetPlanImplementation(_RPID, _MaxPrecentage, _MinPercentage, _FromDate, _ToDate);
        }

        public bool AddFootNotes(long _RPID, string _FootNotes)
        {
            return DALRotationalProgram.AddFootNotes(_RPID, _FootNotes);
        }

        public bool SetApprovelStatus(RP_Approval _ObjApproval, bool _GetPrevDes)
        {
            return DALRotationalProgram.SetApprovelStatus(_ObjApproval, _GetPrevDes);
        }

        public RP_Approval ShowHideApprovalButtons(long _RPID)
        {
            return DALRotationalProgram.ShowhideApprovalButtons(_RPID);
        }

        public string GetFootNotes(long _RPID)
        {
            return DALRotationalProgram.GetFootNotes(_RPID);
        }

        public RP_RotationalProgram GetClonePlanDetail(long _RPID)
        {
            return DALRotationalProgram.GetClonePlanDetail(_RPID);
        }

        public bool SaveCloneDraftData(long _CloneRPID, long _CurrentRPID, int _CreatedBy, long _IrrigationLevelID)
        {
            return DALRotationalProgram.SaveCloneDraftData(_CloneRPID, _CurrentRPID, _CreatedBy, _IrrigationLevelID);
        }

        public List<dynamic> GetCommentsHistory(long _RPID)
        {
            return DALRotationalProgram.GetCommentsHistory(_RPID);
        }

        public List<RP_GetAvgDPRChannelName_Result> GetGraphData(long _RPID)
        {
            return DALRotationalProgram.GetGraphData(_RPID);
        }

        public List<RP_GetBandFrequency_Result> GetFrequencyTable(long _RPID)
        {
            return DALRotationalProgram.GetFrequencyTable(_RPID);
        }

        public double? GetMaxMinDPR(long _RPID)
        {
            return DALRotationalProgram.GetMaxMinDPR(_RPID);
        }

        public bool GetGini(long _RPID)
        {
            return DALRotationalProgram.GetGini(_RPID);
        }

        #endregion
        #region Rotational Voilation Report
        public List<object> GetRotationalProgramYearBySessionID(int SeasonID)
        {
            return DALRotationalProgram.GetRotationalProgramYearBySessionID(SeasonID);
        }
        #endregion

    }
}
