using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.RotationalProgram;
using PMIU.WRMIS.DAL.Repositories.WaterTheft;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.RotationalProgram
{
    public class RotationalProgramDAL
    {
        ContextDB db = new ContextDB();

        #region Search Plan

        public List<object> GetYears()
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetYears();
        }

        public List<dynamic> SearchRotationalPlan(long _CircleID, long _DivisionID, long _SubDivisionID, string _Year, long _SeasonID, long _UserIrrigationLevelID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().SearchRotationalPlan(_CircleID, _DivisionID, _SubDivisionID, _Year, _SeasonID, _UserIrrigationLevelID);
        }

        public long DeleteProgram(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().DeleteProgram(_RPID);
        }

        public long ApproveDraft(long _RPID)
        {
            long Result = -1;
            try
            {
                RP_RotationalProgram objProgram = new RP_RotationalProgram();
                objProgram = db.Repository<RP_RotationalProgram>().FindById(_RPID);
                if (objProgram.IsApproved)
                {
                    objProgram.IsApproved = false;
                    Result = 2;
                }
                else
                {
                    objProgram.IsApproved = true;
                    Result = 1;
                }
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return Result;
            }
            return Result;
        }


        #endregion

        #region Add Plan

        public long AddPlan(RP_RotationalProgram _ObjSave)
        {
            long Result = -1;
            try
            {
                db.Repository<RP_RotationalProgram>().Insert(_ObjSave);
                db.Save();
                Result = _ObjSave.ID;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Result = -1;
                return Result;
            }
            return Result;
        }

        public bool SaveAttachments(RP_Attachment _ObjSave)
        {
            try
            {
                long ID = _ObjSave.RPID;
                db.ExtRepositoryFor<RotationalProgramRepository>().DeleteExistingAttachments(ID);
                db.Repository<RP_Attachment>().Insert(_ObjSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public List<long?> GetEnteringChannels(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetEnteringChannels(_RPID);
        }

        public object GetBasicDetail(long _ID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetBasicDetail(_ID);
        }

        public List<object> GetDivisionsAgainstChannelID(long _CircleID, string _Group)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetDivisionsAgainstChannelID(_CircleID, _Group);
        }

        public List<object> GetChannelsAgainstDivision(long _DivisionID, long _DivisionSubDivisionID, List<long?> _lstChnlID, bool _PostBack)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetChannelsAgainstDivision(_DivisionID, _DivisionSubDivisionID, _lstChnlID, _PostBack);
        }

        public List<object> GetEnteringChannelsAgainstDivision(long _DivisionSubDivisionID, long _Designation)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetEnteringChannelsAgainstDivision(_DivisionSubDivisionID, _Designation);
        }

        public bool SaveDivisions(List<long> _lstIDs, long _RPID, int _UserID, int _NoOfGrps)
        {
            string GroupName = "A";
            long GroupID = -1;
            RP_Division ObjDivision;
            try
            {
                bool Result = db.ExtRepositoryFor<RotationalProgramRepository>().DeleteDivisionData(_RPID);
                if (Result)
                {
                    List<RP_Group> lstGrps = db.Repository<RP_Group>().GetAll().Where(q => q.Type == "G").Take(_NoOfGrps).ToList();
                    GroupID = lstGrps.Where(q => q.Name == GroupName).Select(w => w.ID).FirstOrDefault();
                    for (int i = 0; i < _lstIDs.Count(); i++)
                    {
                        if (_lstIDs[i] == -1)
                        {
                            GroupName = Utility.GetNextGroupName(GroupName);
                            GroupID = lstGrps.Where(q => q.Name == GroupName).Select(w => w.ID).FirstOrDefault();
                            continue;
                        }
                        else
                        {
                            ObjDivision = new RP_Division();
                            ObjDivision.RPID = _RPID;
                            ObjDivision.GroupID = GroupID;
                            ObjDivision.DivisionID = _lstIDs[i];
                            ObjDivision.CreatedDate = DateTime.Now;
                            ObjDivision.CreatedBy = _UserID;
                            db.Repository<RP_Division>().Insert(ObjDivision);
                        }
                    }
                    db.Save();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public List<dynamic> GetCircleGridData(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetCircleGridData(_RPID);
        }

        public bool SaveCirclePreferences(RP_Rotation_Circle _ObjSave)
        {
            try
            {
                if (_ObjSave.ID == -1)
                    db.Repository<RP_Rotation_Circle>().Insert(_ObjSave);
                else
                    db.Repository<RP_Rotation_Circle>().Update(_ObjSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public List<RP_Division> GetProgramMappedDivisions(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetProgramMappedDivisions(_RPID);
        }

        public string SaveChannels(List<dynamic> _lstChannels, long _RPID, int _UserID, int _NoOfGrps)
        {
            string GroupName = "A";
            long GroupID = -1;
            int SubGroupCount = 0;
            string DraftName = "";

            RP_Channel objSave = new RP_Channel();
            List<RP_Group> lstGrps = new List<RP_Group>();
            List<RP_SubGroup> lstSubGrps = new List<RP_SubGroup>();
            List<long> lstSubGrpIDs = new List<long>();
            string Selected = "";
            try
            {
                DraftName = db.ExtRepositoryFor<RotationalProgramRepository>().CheckConflictingChannels(_RPID, _lstChannels);
                if (DraftName == "")
                {
                    bool Result = db.ExtRepositoryFor<RotationalProgramRepository>().DeleteChannelData(_RPID);
                    if (Result)
                    {
                        lstGrps = db.Repository<RP_Group>().GetAll().Where(q => q.Type == "G").Take(_NoOfGrps).ToList();
                        GroupID = lstGrps.Where(q => q.Name == GroupName).Select(w => w.ID).FirstOrDefault();
                        lstSubGrps = db.Repository<RP_SubGroup>().GetAll().ToList();
                        lstSubGrpIDs = lstSubGrps.Where(q => q.GroupID == GroupID).Select(w => w.ID).ToList<long>();

                        foreach (dynamic item in _lstChannels)
                        {
                            objSave = new RP_Channel();
                            objSave.ChannelID = Convert.ToInt64(Utility.GetDynamicPropertyValue(item, "ChannelID"));
                            objSave.Discharge = Convert.ToDouble(Utility.GetDynamicPropertyValue(item, "Discharge"));
                            Selected = Utility.GetDynamicPropertyValue(item, "Selection");

                            if (objSave.ChannelID == -1) // sub group has changed                        
                                SubGroupCount++;
                            else if (objSave.ChannelID == -2) // group has changed
                            {
                                GroupName = Utility.GetNextGroupName(GroupName);
                                GroupID = lstGrps.Where(q => q.Name == GroupName).Select(w => w.ID).FirstOrDefault();
                                lstSubGrpIDs = lstSubGrps.Where(q => q.GroupID == GroupID).Select(w => w.ID).ToList<long>();
                                SubGroupCount = 0;
                            }
                            else
                            {
                                objSave.RPID = _RPID;
                                objSave.GroupID = GroupID;
                                if (Selected == Constants.SubGroup)
                                    objSave.SubGroupID = lstSubGrpIDs[SubGroupCount];
                                objSave.CreatedDate = DateTime.Now;
                                objSave.CreatedBy = _UserID;
                                db.Repository<RP_Channel>().Insert(objSave);
                            }
                        }
                        db.Save();
                        DraftName = "Success";
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return "Fail";
            }
            return DraftName;
        }

        public List<RP_Channel> GetProgramMappedChannels(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetProgramMappedChannels(_RPID);
        }

        public List<dynamic> GetDivSubDivPreferenceData(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetDivSubDivPreferenceData(_RPID);
        }

        public List<int> GetActualSubGroups(long _RPID, int _MaxGroups, int _MaxSubGroups)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetActualSubGroups(_RPID, _MaxGroups, _MaxSubGroups);
        }

        public bool SaveDivSubDivPreference(RP_Rotation_DivisionSubDivision _objSave)
        {
            try
            {
                if (_objSave.ID == -1)
                    db.Repository<RP_Rotation_DivisionSubDivision>().Insert(_objSave);
                else
                    db.Repository<RP_Rotation_DivisionSubDivision>().Update(_objSave);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool DeleteDivSubDivPref(long _ID)
        {
            try
            {
                db.Repository<RP_Rotation_DivisionSubDivision>().Delete(_ID);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool DeleteCirclePref(long _ID)
        {
            try
            {
                db.Repository<RP_Rotation_Circle>().Delete(_ID);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public long GetSubGroupsOfGroup(long _RPID, long _GroupID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetSubGroupsOfGroup(_RPID, _GroupID);
        }

        public bool? GetPriority(long _RPID)
        {
            return db.Repository<RP_RotationalProgram>().FindById(_RPID).IsPriority;
        }

        public string GetZoneName(long _ZoneID)
        {
            return db.Repository<CO_Zone>().FindById(_ZoneID).Name.ToString();
        }

        public string GetPlanCount(long _IrrigationLevelID, long _IrrigationBoundryID, long _SeasonID, string _Year)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetPlanCount(_IrrigationLevelID, _IrrigationBoundryID, _SeasonID, _Year);
        }

        public string GetAttachmentfiles(long _RPID)
        {
            string Name = "";
            try
            {
                Name = db.Repository<RP_Attachment>().GetAll().Where(q => q.RPID == _RPID).Select(w => w.FileURL).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Name;
        }

        public bool AddEnteringchannels(List<RP_EnteringChannel> _lstEC)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().AddEnteringchannels(_lstEC);
        }


        public bool AddClosureDates(long _ProgramID, DateTime _StartDate, DateTime _EndDate)
        {
            bool Result = false;
            try
            {
                RP_RotationalProgram objProgram = db.Repository<RP_RotationalProgram>().FindById(_ProgramID);
                if (objProgram != null)
                {
                    objProgram.ClosureStartDate = _StartDate;
                    objProgram.ClosureEndDate = _EndDate;
                    Result = db.ExtRepositoryFor<RotationalProgramRepository>().ManageClosureDates(_ProgramID, _StartDate, _EndDate);
                    if (Result)
                        db.Save();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }


        public bool AddClosureDatesCircle(long _ProgramID, DateTime _StartDate, DateTime _EndDate)
        {
            bool Result = false;
            try
            {
                RP_RotationalProgram objProgram = db.Repository<RP_RotationalProgram>().FindById(_ProgramID);
                if (objProgram != null)
                {
                    objProgram.ClosureStartDate = _StartDate;
                    objProgram.ClosureEndDate = _EndDate;
                    Result = db.ExtRepositoryFor<RotationalProgramRepository>().ManageClosureDatesCircle(_ProgramID, _StartDate, _EndDate);
                    if (Result)
                        db.Save();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }






        #endregion

        #region Export

        public dynamic GetBAsicInfoForSEExport(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetBasicInfoForSeExport(_RPID);
        }
        public dynamic GetBAsicInfoForXENSDOExport(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetBasicInfoForXENSDOExport(_RPID);
        }
        public List<dynamic> GetDivisionsByGroups(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetDivisionsByGroups(_RPID);
        }
        public List<dynamic> GetWaraInfoForSEExport(long _RPID, string _ClosureStartDate, string _ClosureEndDate)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetWaraInfoForSEExport(_RPID, _ClosureStartDate, _ClosureEndDate);
        }
        public List<dynamic> GetChannelsBySubGroups(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetChannelsBySubGroups(_RPID);
        }
        public List<dynamic> GetChannelsPreferences(List<dynamic> _WaraData)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetChannelPreferences(_WaraData);
        }
        public List<dynamic> GetChannelsWaraData(long _RPID, string _ClosureStartDate, string _ClosureEndDate)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetChannelsWaraData(_RPID, _ClosureStartDate, _ClosureEndDate);
        }
        #endregion

        #region View and Print

        public List<dynamic> GetGroupsSubGroupsChannels(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetGroupsSubGroupsChannels(_RPID);
        }

        public string GetSubGroupNameByID(int _ID)
        {
            return db.Repository<RP_SubGroup>().GetAll().Where(x => x.ID == _ID).Select(x => x.Name).FirstOrDefault();
        }
        public dynamic GetProgramNameAndDates(long _RPID, string _Value)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetProgramNameAndDates(_RPID, _Value);
        }
        public List<dynamic> GetPreferencesDataForView(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetPreferencesDataForView(_RPID);
        }
        public List<dynamic> GetPreferencesDataDivisionForView(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetPreferencesDataDivisionForView(_RPID);
        }

        /// <summary>
        /// Extension method to convert dynamic data to a DataTable. Useful for databinding.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>A DataTable with the copied dynamic data.</returns>
        public DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            var data = items.ToArray();
            if (data.Count() == 0) return null;

            var dt = new DataTable();

            for (int i = 0; i < data.Count(); i++)
            {
                foreach (var key in ((IDictionary<string, object>)data[i]).Keys)
                {
                    dt.Columns.Add(key);

                }
                if (dt.Columns.Count <= 3)
                {
                    if (i != data.Count() - 1)
                    {
                        dt.Columns.Clear();
                    }
                    else
                    {
                        continue;
                    }


                }
                else
                {
                    break;
                }
            }

            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }

        public long GetIrrigationLevelByRPID(long _RPID)
        {
            return db.Repository<RP_RotationalProgram>().GetAll().Where(x => x.ID == _RPID).Select(x => x.IrrigationLevelID).FirstOrDefault();
        }
        #endregion

        #region Model integration

        public List<List<dynamic>> GetWaraDates(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetWaraDates(_RPID);
        }

        public void AdjustDateDifference(long _RPID, DateTime _DiffstartDate, int _DaysDiff)
        {
            try
            {
                List<RP_Rotation_DivisionSubDivision> lstResult = db.ExtRepositoryFor<RotationalProgramRepository>().AdjustDateDifference(_RPID, _DiffstartDate, _DaysDiff);

                foreach (RP_Rotation_DivisionSubDivision obj in lstResult)
                {
                    obj.StartDate = obj.StartDate.Value.AddDays(_DaysDiff);
                    obj.EndDate = obj.EndDate.Value.AddDays(_DaysDiff);
                    db.Repository<RP_Rotation_DivisionSubDivision>().Update(obj);
                }
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        public bool GetPlanImplementation(long _RPID, double _MaxPrecentage, double _MinPercentage, DateTime _FromDate, DateTime _ToDate)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetPlanImplementation(_RPID, _MaxPrecentage, _MinPercentage, _FromDate, _ToDate);
        }

        public bool AddFootNotes(long _RPID, string _FootNotes)
        {
            try
            {
                RP_RotationalProgram objRP = db.Repository<RP_RotationalProgram>().FindById(_RPID);
                objRP.FootNote = _FootNotes;
                db.Repository<RP_RotationalProgram>().Update(objRP);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool SetApprovelStatus(RP_Approval _ObjApproval, bool _GetPrevDes)
        {
            bool Result = false;
            try
            {
                if (_GetPrevDes)
                {
                    long? DesID = db.ExtRepositoryFor<RotationalProgramRepository>().GetPreviousUserDesignationID(_ObjApproval.RPID);
                    _ObjApproval.DesignationToID = DesID;
                }

                db.Repository<RP_Approval>().Insert(_ObjApproval);

                if (_ObjApproval.Status == Constants.RP_Approved)
                {
                    RP_RotationalProgram objRP = db.Repository<RP_RotationalProgram>().FindById(_ObjApproval.RPID);
                    objRP.IsApproved = true;
                    db.Repository<RP_RotationalProgram>().Update(objRP);
                }
                db.Save();
                Result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return Result;
            }
            return Result;
        }

        public RP_Approval ShowhideApprovalButtons(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().ShowhideApprovalButtons(_RPID);
        }

        public string GetFootNotes(long _RPID)
        {
            string FootNotes = "";
            try
            {
                FootNotes = db.Repository<RP_RotationalProgram>().FindById(_RPID).FootNote.ToString();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return FootNotes;
        }

        public RP_RotationalProgram GetClonePlanDetail(long _RPID)
        {
            RP_RotationalProgram objPlan = new RP_RotationalProgram();
            try
            {
                objPlan = db.Repository<RP_RotationalProgram>().FindById(_RPID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objPlan;
        }

        public bool SaveCloneDraftData(long _CloneRPID, long _CurrentRPID, int _CreatedBy, long _IrrigationLevelID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().SaveCloneDraftData(_CloneRPID, _CurrentRPID, _CreatedBy, _IrrigationLevelID);
        }

        public List<dynamic> GetCommentsHistory(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetCommentsHistory(_RPID);
        }

        public List<RP_GetAvgDPRChannelName_Result> GetGraphData(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetGraphData(_RPID);
        }

        public List<RP_GetBandFrequency_Result> GetFrequencyTable(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetFrequencyTable(_RPID);
        }

        public double? GetMaxMinDPR(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetMaxMinDPR(_RPID);
        }

        public bool GetGini(long _RPID)
        {
            return db.ExtRepositoryFor<RotationalProgramRepository>().GetGini(_RPID);
        }

        #endregion

        #region Rotational Voilation Report
        public List<object> GetRotationalProgramYearBySessionID(int SeasonID)
        {

            List<RP_RotationalProgram> lstData = db.Repository<RP_RotationalProgram>().Query().Get().Where(X => X.IsApproved == true).ToList();
            if (lstData == null || lstData.Count <= 0)
                return null;
            List<object> listData = new List<object>();
            listData = lstData.OrderByDescending(x => x.RPYear).Where(s => s.SeasonID == SeasonID).Select(x => new { ID = x.RPYear, Name = x.RPYear }).Distinct().ToList<object>();
            return listData;
        }
        #endregion


    }
}
