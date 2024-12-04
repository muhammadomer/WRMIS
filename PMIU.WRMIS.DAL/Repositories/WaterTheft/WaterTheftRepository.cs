using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
﻿using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.AppBlocks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

namespace PMIU.WRMIS.DAL.Repositories.WaterTheft
{
    public class WaterTheftRepository : Repository<WT_WaterTheftCase>
    {
        public WaterTheftRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<WT_WaterTheftCase>();

        }
        // #region "Load Channel"
        /// <summary>
        /// Get Channel By User ID
        /// Created on: 16-03-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>Channel</returns>
        public List<object> GetChannelByUserID(long _UserID, long _IrrigationLevelID)
        {
            List<long?> lstSectionIDs = (from assloc in context.UA_AssociatedLocation
                                         where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
                                         select assloc.IrrigationBoundryID).ToList();
            List<object> lstChannel = (from cib in context.CO_ChannelIrrigationBoundaries
                                       join channel in context.CO_Channel on cib.ChannelID equals channel.ID
                                       where lstSectionIDs.Contains(cib.SectionID)
                                       select channel).Distinct().ToList<object>();

            return lstChannel;
        }
        public List<object> GetChannelByDivisionID(long _DivisionID)
        {

            List<object> lstChannel = (from cib in context.CO_ChannelIrrigationBoundaries
                                       where cib.CO_Section.CO_SubDivision.DivisionID == _DivisionID
                                       select new { cib.ChannelID, cib.CO_Channel.NAME }
                                      ).Distinct().OrderBy(d => d.NAME).ToList<object>();
            return lstChannel;
        }

        public List<object> GetOutletByDivisionID(long _DivisionID)
        {


            List<object> lstChannel = (from cib in context.CO_ChannelIrrigationBoundaries
                                       where cib.CO_Section.CO_SubDivision.DivisionID == _DivisionID
                                       select new { cib.ChannelID, cib.CO_Channel.NAME }
                                      ).Distinct().OrderBy(d => d.NAME).ToList<object>();
            return lstChannel;
        }
        // #region "Load Outlet"
        /// <summary>
        /// Get Outlet By Channel ID
        /// Created on: 16-03-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>Outlet</returns>
        public List<object> GetOutletByChannelID(long _ChannelId, long _UserID, long _IrrigationLevelID)
        {
            List<object> lstOutlet;
            string ChannelId = _ChannelId.ToString();

            if (_IrrigationLevelID == 0)
            {
                lstOutlet = (from outlet in context.CO_ChannelOutlets
                             where outlet.ChannelID == _ChannelId
                             select new
                             {
                                 ID = outlet.ID,
                                 Name = outlet.OutletRD,
                                 ChannelSide = outlet.ChannelSide,
                             }).ToList().Select(lstout => new
                             {
                                 lstout.ID,
                                 Name = lstout.Name.HasValue ? Calculations.GetRDText(lstout.Name.Value) + "/" + lstout.ChannelSide : "",
                             }).ToList<object>();
            }
            else
            {
                ContextDB dbADO = new ContextDB();
                string GetSection = "SELECT [dbo].[WT_GetSectionIDsByUserAndDivision] ({0},{1},{2})";
                Object[] parameters = { _UserID, _IrrigationLevelID, -1 };
                var lstSectionIDsComma = context.Database.SqlQuery<string>(GetSection, parameters).FirstOrDefault();
                var result = dbADO.ExecuteStoredProcedureDataTable("WT_GetChannelIDAndRDsBySectionIDs", lstSectionIDsComma, ChannelId);
                int MinRD = result.AsEnumerable().Min(r => r.Field<int>("MinRD"));
                int MaxRD = result.AsEnumerable().Max(r => r.Field<int>("MaxRD"));
                lstOutlet = (from outlet in context.CO_ChannelOutlets
                             where (outlet.OutletRD >= MinRD && outlet.OutletRD <= MaxRD) && outlet.ChannelID == _ChannelId
                             select new
                                         {
                                             ID = outlet.ID,
                                             Name = outlet.OutletRD,
                                             ChannelSide = outlet.ChannelSide

                                         }).ToList().Select(lstout => new
                                                     {
                                                         lstout.ID,
                                                         Name = lstout.Name.HasValue ? Calculations.GetRDText(lstout.Name.Value) + "/" + lstout.ChannelSide : "",
                                                     }).ToList<object>();
            }
            return lstOutlet;
        }
        public List<object> GetAssetWorkOutletByChannelID(long _DivisionID)
        {
            List<object> lstOutlet= new List<object>();
            List<long> lstChannelIDs = (from cib in context.CO_ChannelIrrigationBoundaries
                                       join c in context.CO_Channel on cib.ChannelID equals c.ID
                                       join s in context.CO_Section on cib.SectionID equals s.ID
                                       join sub in context.CO_SubDivision on s.SubDivID equals sub.ID
                                       where sub.DivisionID == _DivisionID
                                       select c.ID).ToList();

            foreach (var item in lstChannelIDs)
            {
                    var q = (from outlet in context.CO_ChannelOutlets
                                 where  outlet.ChannelID == item
                                 select new
                                 {
                                     ID = outlet.ID,
                                     Name = outlet.OutletRD,
                                     ChannelSide = outlet.ChannelSide

                                 }).ToList().Select(lstout => new
                                 {
                                     lstout.ID,
                                     Name = lstout.Name.HasValue ? Calculations.GetRDText(lstout.Name.Value) + "/" + lstout.ChannelSide : "",
                                    // Name = lstout.Name,
                                 }).ToList<object>();

                    lstOutlet.AddRange(q);
              
            }
            return lstOutlet;
        }

        // #region "Load Outlet Information"
        /// <summary>
        /// Get Outlet Information By Outlet ID
        /// Created on: 16-03-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>Outlet Information</returns>
        public object GetDataByOutletID(int _OutletID)
        {
            object qOutletData = (from co in context.CO_ChannelOutlets
                                  join coah in context.CO_OutletAlterationHistroy on co.ID equals coah.OutletID into C
                                  from c in C.DefaultIfEmpty()
                                  join outtype in context.CO_OutletType on c.OutletTypeID equals outtype.ID into E
                                  from e in E.DefaultIfEmpty()
                                  where co.ID == _OutletID
                                  select new
                                  {
                                      OutletRD = co.OutletRD,
                                      ChannelSide = co.ChannelSide,
                                      OutletType = c.CO_OutletType.Name
                                  }).ToList().Select(u => new
                                  {
                                      OutletRDs = Calculations.GetRDText(Convert.ToDouble(u.OutletRD)),
                                      OutletRDsDB = u.OutletRD,
                                      ChannelSide = u.ChannelSide,
                                      OutletType = u.OutletType
                                  }).FirstOrDefault();

            return qOutletData;
        }

        // #region "Check Theft Case"
        /// <summary>
        /// Check theft Case 
        /// Created on: 16-03-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>Count</returns>
        public bool IsWaterTheftCaseExist(WT_WaterTheftCase _mdlWatertheftCase, int _NoofFeet)
        {
            bool qIsCaseExists = false;
            qIsCaseExists = context.WT_WaterTheftCase.Any(c => DbFunctions.TruncateTime(c.IncidentDateTime) == DbFunctions.TruncateTime(_mdlWatertheftCase.IncidentDateTime)
                             && (c.TheftSiteRD >= _mdlWatertheftCase.TheftSiteRD - _NoofFeet
                             && c.TheftSiteRD <= _mdlWatertheftCase.TheftSiteRD + _NoofFeet)
                             && c.OffenceSide == _mdlWatertheftCase.OffenceSide
                             && c.OffenceTypeID == _mdlWatertheftCase.OffenceTypeID
                             && c.ChannelID == _mdlWatertheftCase.ChannelID);
            return qIsCaseExists;
        }

        public List<object> GetAllTheftType(string _OffenceSiteId)
        {
            List<object> lstOutlet = (from co in context.WT_OffenceType
                                      where co.ChannelOutlet == _OffenceSiteId
                                       && co.IsActive == true
                                      select new
                                      {
                                          ID = co.ID,
                                          Name = co.Name,

                                      }).ToList<object>();
            return lstOutlet;
        }

        public List<object> GetOutletCondition(string _OffenceSiteId)
        {
            List<object> lstTheftSiteCondition = (from co in context.WT_TheftSiteCondition
                                                  where co.ChannelOutlet == _OffenceSiteId
                                                  && co.IsActive == true
                                                  select new
                                                  {
                                                      ID = co.ID,
                                                      Name = co.Name,

                                                  }).ToList<object>();
            return lstTheftSiteCondition;
        }

        public List<object> GetDefectiveType()
        {
            List<object> lstDefectiveType = (from co in context.WT_OutletDefectiveType
                                             where  co.IsActive == true
                                             select new
                                             {
                                                 ID = co.ID,
                                                 Name = co.Name,

                                             }).ToList<object>();
            return lstDefectiveType;
        }

        #region Ziledar Outlet Working

        public string GetOtletType(long _OutletID)
        {
            string OutletType = "";
            try
            {
                long? OutletTypeID = (from outletHist in context.CO_OutletAlterationHistroy
                                      where outletHist.OutletID == _OutletID
                                      orderby outletHist.AlterationDate descending
                                      select outletHist.OutletTypeID).FirstOrDefault();

                if (OutletTypeID != null)
                {
                    OutletType = (from ot in context.CO_OutletType
                                  where ot.ID == OutletTypeID
                                  select ot.Name).FirstOrDefault();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return OutletType;
        }

        public string GetDateAndTime(DateTime _DateTime, int _Check)
        {
            string DT = "";
            try
            {
                if (_DateTime != null)
                {
                    if (_Check == 1)
                    {
                        DT = _DateTime.ToString().Substring(0, 9);
                    }
                    else
                    {
                        DT = _DateTime.ToString().Substring(10, 9);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return DT;
        }

        public string GetChannelName(long? _ChannelID)
        {
            string ChannelName = "";
            try
            {
                ChannelName = (from chnl in context.CO_Channel
                               where chnl.ID == _ChannelID
                               select chnl.NAME).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ChannelName;
        }

        public string GetChannelSide(string _Side)
        {
            string Side = "Right " + "(" + _Side + ")";
            if (_Side == "L")
                Side = "Left " + "(" + _Side + ")";
            return Side;
        }

        public object GetZiladaarWorkingParentInformation(long _CaseID)
        {
            object ParentInfo = new object();
            try
            {
                ParentInfo = (from wtc in context.WT_WaterTheftCase
                              join status in context.WT_Status on wtc.CaseStatusID equals status.ID
                              join ot in context.WT_OffenceType on wtc.OffenceTypeID equals ot.ID into C
                              from c in C.DefaultIfEmpty()
                              join tsc in context.WT_TheftSiteCondition on wtc.TheftSiteConditionID equals tsc.ID
                              join dd in context.WT_OutletDefectiveDetails on wtc.ID equals dd.WaterTheftID into A
                              from a in A.DefaultIfEmpty()
                              join odt in context.WT_OutletDefectiveType on a.DefectiveTypeID equals odt.ID into B
                              from b in B.DefaultIfEmpty()
                              join outlet in context.CO_ChannelOutlets on wtc.OutletID equals outlet.ID
                              where wtc.ID == _CaseID
                              select new
                              {
                                  ChannelID = wtc.ChannelID,
                                  ChannelRD = outlet.OutletRD,
                                  ChannelSide = outlet.ChannelSide,
                                  OutletID = wtc.OutletID,
                                  RD = wtc.TheftSiteRD,
                                  Side = wtc.OffenceSide,
                                  Datetime = wtc.IncidentDateTime,
                                  LogDateTime = wtc.LogDateTime,
                                  TheftType = c.Name,
                                  ConditionOfOutlet = tsc.Name,
                                  H = wtc.ValueofH,
                                  Defectivetype = b.Name,
                                  B = a.ValueOfB,
                                  Y = a.ValueOfY,
                                  DIA = a.ValueOfDia,
                                  Remarks = wtc.Remarks,
                                  wtc.CaseNo,
                                  status.Name
                              })
                              .ToList()
                              .Select(u => new
                            {
                                ChannelName = GetChannelName(u.ChannelID),
                                Outlet = GetOutletside(u.ChannelRD, u.ChannelSide),
                                OutletType = GetOtletType(u.OutletID.Value),
                                RD = Calculations.GetRDText(u.RD),
                                Side = GetChannelSide(u.Side),
                                IncidentDate = Utility.GetFormattedDate(u.Datetime),
                                IncidentTime = Utility.GetFormattedTime(u.Datetime),
                                DateTimeOfChecking = Utility.GetFormattedDate(u.Datetime) + " " + Utility.GetFormattedTime(u.Datetime),
                                SMSDate = Utility.GetFormattedDate(u.LogDateTime),
                                u.TheftType,
                                u.ConditionOfOutlet,
                                u.H,
                                u.Defectivetype,
                                u.B,
                                u.Y,
                                u.DIA,
                                u.Remarks,
                                u.CaseNo,
                                u.Name
                            })
                            .FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ParentInfo;
        }

        public List<WT_WaterTheftOffender> GetOffenders(long _CaseID)
        {
            List<WT_WaterTheftOffender> lstOffenders = new List<WT_WaterTheftOffender>();
            try
            {
                lstOffenders = (from offndr in context.WT_WaterTheftOffender
                                where offndr.WaterTheftID == _CaseID
                                select offndr).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstOffenders;
        }

        public object GetManager(long _UserID)
        {
            object Manager = new object();
            try
            {
                Manager = (from UM in context.UA_UserManager
                           where UM.UserID == _UserID
                           select new
                           {
                               ManagerID = UM.ManagerID,
                               ManagerDesID = UM.ManagerDesignationID
                           }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Manager;
        }

        public int? SaveZiladaarWorking(long? _UserID, long? _DesignationID, long? _CaseID, double? _AreaBooked, long? _AreaUnit, int? _NoOfAccused, DateTime _ProcedingDate, string _ZildarRemarks)
        {
            int? Result = null;
            try
            {
                object ObjManager = GetManager((long)_UserID);
                if (ObjManager != null)
                {
                    long CaseID = GetCaseStatus((long)_CaseID);

                    long AssignedToUserID = Convert.ToInt64(ObjManager.GetType().GetProperty("ManagerID").GetValue(ObjManager));
                    long AssignedToUserDesID = Convert.ToInt64(ObjManager.GetType().GetProperty("ManagerDesID").GetValue(ObjManager));
                    Result = context.InsertTawaanPoiliceCaseWorking(_UserID, _DesignationID, _CaseID, _AreaBooked, _AreaUnit, _NoOfAccused, _ProcedingDate, _ZildarRemarks
                        , AssignedToUserID, AssignedToUserDesID, DateTime.Now, CaseID, "");
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return Result;
            }
            return Result;
        }

        public object GetSavedZiladaarWorking(long _WTCaseID)
        {
            object ObjZiladaarRecord = new object();
            try
            {
                ObjZiladaarRecord = (from FineDetail in context.WT_WaterTheftDecisionFineDetail
                                     join PoliceCase in context.WT_WaterTheftPoliceCase on FineDetail.WaterTheftID equals PoliceCase.WaterTheftID
                                     join Area in context.WT_AreaType on FineDetail.AreaTypeID equals Area.ID
                                     where FineDetail.WaterTheftID == _WTCaseID
                                     select new
                                     {
                                         AreaBooked = FineDetail.AreaBooked,
                                         Unit = Area.Name,
                                         NoOfAccused = PoliceCase.NoOfAccused,
                                         ProcessedDate = PoliceCase.ProcedingDate,
                                         Comments = PoliceCase.ZildarRemarks
                                     }).ToList().Select(q => new
                                     {
                                         q.AreaBooked,
                                         q.Unit,
                                         q.NoOfAccused,
                                         ProcessedDate = Utility.GetFormattedDate(q.ProcessedDate),
                                         q.Comments
                                     }).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ObjZiladaarRecord;
        }

        public long? GetManagerSubOrdinate(long _ManagerID, long _SubDesID)
        {
            //object Manager = new object();
            long? SubOrdinate = null;
            try
            {
                SubOrdinate = (from UM in context.UA_UserManager
                               where UM.ManagerID == _ManagerID && UM.UserDesignationID == _SubDesID
                               select UM.UserID).FirstOrDefault();
                //select new
                //{
                //    UserID = UM.ManagerID,
                //    ManagerDesID = UM.ManagerDesignationID
                //}).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return SubOrdinate;
        }

        public void UpdateNoOfOffenders(long _WTCaseID)
        {
            try
            {
                WT_WaterTheftPoliceCase objPoliceCase = (from pc in context.WT_WaterTheftPoliceCase
                                                         where pc.WaterTheftID == _WTCaseID
                                                         select pc).FirstOrDefault();

                if (objPoliceCase != null)
                {
                    objPoliceCase.NoOfAccused = objPoliceCase.NoOfAccused - 1;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

        }

        public long GetCaseStatus(long _WTCaseID)
        {
            long CaseStatusID = 2;
            try
            {
                CaseStatusID = (from WTC in context.WT_WaterTheftCase
                                where WTC.ID == _WTCaseID
                                select WTC.CaseStatusID).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return CaseStatusID;
        }

        public void IncrementOffenders(long? _WTCaseID)
        {
            WT_WaterTheftPoliceCase NoOfOffenders = new WT_WaterTheftPoliceCase();
            try
            {
                NoOfOffenders = (from WTPC in context.WT_WaterTheftPoliceCase
                                 where WTPC.WaterTheftID == _WTCaseID
                                 select WTPC).FirstOrDefault();

                if (NoOfOffenders != null)
                {
                    NoOfOffenders.NoOfAccused = NoOfOffenders.NoOfAccused + 1;
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }



        #endregion

        #region "Search Water Theft"

        /// <summary>
        /// This function fetches Division based on the UserID and User Irrigation Level.
        /// Created On 06-05-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByUserID(long _UserID, long _IrrigationLevelID, bool? _IsActive = null)
        {
            List<CO_Division> lstDivision = null;

            List<long?> lstLocationIDs = (from assloc in context.UA_AssociatedLocation
                                          where assloc.UserID == _UserID && assloc.IrrigationLevelID == _IrrigationLevelID
                                          select assloc.IrrigationBoundryID).ToList();

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
            {
                lstDivision = (from div in context.CO_Division
                               join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                               join sec in context.CO_Section on subdiv.ID equals sec.SubDivID
                               where lstLocationIDs.Contains(sec.ID) && (div.IsActive == _IsActive || _IsActive == null)
                               select div).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                var query = from div in context.CO_Division
                            join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                            where lstLocationIDs.Contains(subdiv.ID)
                            select div;
                lstDivision = (from div in context.CO_Division
                               join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                               where lstLocationIDs.Contains(subdiv.ID) && (div.IsActive == _IsActive || _IsActive == null)
                               select div).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                var query = from div in context.CO_Division
                            where lstLocationIDs.Contains(div.ID)
                            orderby div.Name
                            select div;
                lstDivision = (from div in context.CO_Division
                               where lstLocationIDs.Contains(div.ID) && (div.IsActive == _IsActive || _IsActive == null)
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                lstDivision = (from div in context.CO_Division
                               join cir in context.CO_Circle on div.CircleID equals cir.ID
                               where lstLocationIDs.Contains(cir.ID) && (div.IsActive == _IsActive || _IsActive == null)
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
            {
                lstDivision = (from div in context.CO_Division
                               join cir in context.CO_Circle on div.CircleID equals cir.ID
                               join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                               where lstLocationIDs.Contains(zon.ID) && (div.IsActive == _IsActive || _IsActive == null)
                               orderby div.Name
                               select div).ToList();
            }
            else if (_IrrigationLevelID == 0)
            {
                lstDivision = (from div in context.CO_Division
                               where (div.IsActive == _IsActive || _IsActive == null)
                               select div).ToList();
            }
            return lstDivision;
        }

        public List<CO_Channel> GetChannelsByUserIDAndDivisionID(long _UserID, long? _IrrigationLevelID, long _DivisionID, bool? _IsActive = null)
        {
            List<CO_Channel> lstChannel = null;

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
            {
                lstChannel = (from cib in context.CO_ChannelIrrigationBoundaries
                              join c in context.CO_Channel on cib.ChannelID equals c.ID
                              where (from al in context.UA_AssociatedLocation
                                     where al.IrrigationLevelID == _IrrigationLevelID && al.UserID == _UserID
                                      && (c.IsActive == _IsActive || _IsActive == null)
                                     select al.IrrigationBoundryID).ToList().Contains(cib.SectionID)
                              select c).Distinct().OrderBy(c => c.NAME).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                lstChannel = (from cib in context.CO_ChannelIrrigationBoundaries
                              join c in context.CO_Channel on cib.ChannelID equals c.ID
                              join sec in context.CO_Section on cib.SectionID equals sec.ID
                              where (from al in context.UA_AssociatedLocation
                                     where al.IrrigationLevelID == _IrrigationLevelID && al.UserID == _UserID
                                      && (c.IsActive == _IsActive || _IsActive == null)
                                     select al.IrrigationBoundryID).ToList().Contains(sec.SubDivID)
                              select c).Distinct().OrderBy(c => c.NAME).ToList();
            }
            else
            {
                lstChannel = (from cib in context.CO_ChannelIrrigationBoundaries
                              join c in context.CO_Channel on cib.ChannelID equals c.ID
                              join sec in context.CO_Section on cib.SectionID equals sec.ID
                              join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                              where subdiv.DivisionID == _DivisionID && (c.IsActive == _IsActive || _IsActive == null)
                              select c).Distinct().OrderBy(c => c.NAME).ToList();
            }

            return lstChannel;
        }

        #endregion

        public object GetRelevantSBEInformation(long _ChannelID, decimal _ChannelRDs, long _UserId)
        {
            ContextDB dbADO = new ContextDB();
            var value = (dbADO.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Where(i => i.ChannelID == _ChannelID && i.SectionRD <= _ChannelRDs).Max(i => (int?)i.SectionRD));
            object IrrgBound = (from chanbound in context.CO_ChannelIrrigationBoundaries
                                where chanbound.ChannelID == _ChannelID &&
                                chanbound.SectionRD == value
                                select new
                                {
                                    chanbound.SectionID
                                }).ToList().Select(n => new
                                 {
                                     SectionID = n.SectionID.Value
                                 }).FirstOrDefault();
            if (IrrgBound == null)
            {
                IrrgBound = (from channel in context.CO_Channel
                             join chanbound in context.CO_ChannelIrrigationBoundaries on channel.ID equals chanbound.ChannelID
                             where channel.ID == _ChannelID && _ChannelRDs <= channel.TotalRDs
                             select new
                             {
                                 channel.TotalRDs,
                                 chanbound.SectionID
                             }).ToList().Select(n => new
                 {
                     SectionID = n.SectionID.Value
                 }).FirstOrDefault();
            }

            long IrrgBoundID = Convert.ToInt64(IrrgBound.GetType().GetProperty("SectionID").GetValue(IrrgBound));

            object qReleventSBEInfo = (from al in context.UA_AssociatedLocation
                                       where al.IrrigationBoundryID == IrrgBoundID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section
                                       && al.DesignationID == (long)Constants.Designation.SBE
                                       select new
                                       {
                                           UserID = al.UserID,
                                           DesignationID = al.DesignationID,

                                       }).ToList()
                                          .Select(u => new
                                          {
                                              UserID = u.UserID,
                                              DesignationID = u.DesignationID,
                                          }).FirstOrDefault();
            return qReleventSBEInfo;
        }

        #region "Channel Water Theft Incident (Working)"
        /// <summary>
        /// This function return Channel water theft incident information
        /// Date: 10-05-2016
        /// </summary>
        /// <param name="_WaterTheftID"></param>
        /// <returns></returns>
        public dynamic GetChannelWaterTheftIncidentWorking(long _WaterTheftID)
        {
            dynamic qChannelWTIncident = (from wwtc in context.WT_WaterTheftCase
                                          join status in context.WT_Status on wwtc.CaseStatusID equals status.ID
                                          join wot in context.WT_OffenceType on wwtc.OffenceTypeID equals wot.ID
                                          join wtsc in context.WT_TheftSiteCondition on wwtc.TheftSiteConditionID equals wtsc.ID into A
                                          from a in A.DefaultIfEmpty()
                                          join cc in context.CO_Channel on wwtc.ChannelID equals cc.ID
                                          where wwtc.ID == _WaterTheftID
                                          select new
                                        {
                                            WaterTheftID = wwtc.ID,
                                            ChannelName = cc.NAME,
                                            OffenceType = wot.Name,
                                            CutCondition = a.Name,
                                            wwtc.OffenceSide,
                                            wwtc.IncidentDateTime,
                                            wwtc.TheftSiteRD,
                                            SMSDate = wwtc.LogDateTime,
                                            wwtc.Remarks,
                                            wwtc.CaseNo,
                                            status.Name
                                        }).ToList().Select(CWT => new
                                        {
                                            CWT.WaterTheftID,
                                            CWT.ChannelName,
                                            CWT.OffenceType,
                                            CWT.CutCondition,
                                            CWT.OffenceSide,
                                            CheckingDate = Utility.GetFormattedDate(CWT.IncidentDateTime),
                                            CheckingTime = Utility.GetFormattedTime(CWT.IncidentDateTime),
                                            DateTimeOfChecking = Utility.GetFormattedDate(CWT.IncidentDateTime) + " " + Utility.GetFormattedTime(CWT.IncidentDateTime),
                                            TheftSiteRD = Calculations.GetRDText(CWT.TheftSiteRD),
                                            SMSDate = Utility.GetFormattedDate(CWT.SMSDate),
                                            CWT.Remarks,
                                            CWT.CaseNo,
                                            CWT.Name
                                        }).FirstOrDefault();
            return qChannelWTIncident;
        }

        /// <summary>
        /// this function Add values into stored procedure
        /// Created On:22/04/2016
        /// </summary>
        /// <param name="_BarrageID"></param>
        /// <param name="_ReadingDateTime"></param>
        /// <param name="_IDGaugeDischarge"></param>
        /// <param name="_UserID"></param>
        /// <returns>int</returns>
        public int AssignWaterTheftCaseToSDO(WT_WaterTheftCanalWire _WTCanelWire, string _ClosingRepairDate)
        {
            ContextDB dbADO = new ContextDB();
            DateTime? closingRepairDate = null;
            if (!string.IsNullOrEmpty(_ClosingRepairDate))
                closingRepairDate = Utility.GetParsedDate(_ClosingRepairDate);

            return dbADO.ExecuteScalar("usp_AssignWaterTheftCaseToSDO", _WTCanelWire.WaterTheftID, _WTCanelWire.CanalWireNo, _WTCanelWire.CanalWireDate, closingRepairDate, _WTCanelWire.Remarks, _WTCanelWire.UserID, _WTCanelWire.ID);
        }
        /// <summary>
        /// Mark water theft case as NA
        /// Created on: 25/05/2016
        /// </summary>
        /// <param name="_WTCanelWire"></param>
        /// <returns></returns>
        public int MarkWaterTheftCaseAsNA(WT_WaterTheftCanalWire _WTCanelWire)
        {
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteScalar("usp_MarkWaterTheftCaseAsNA", _WTCanelWire.WaterTheftID, _WTCanelWire.CanalWireNo, _WTCanelWire.CanalWireDate, _WTCanelWire.Remarks, _WTCanelWire.UserID, _WTCanelWire.DesignationID);
        }
        /// <summary>
        /// This function return SBE working information
        /// Date: 13-05-2016
        /// </summary>
        /// <param name="_WaterTheftID"></param>
        /// <returns></returns>
        public List<dynamic> GetWTCaseWorkingInformation(long _WaterTheftID)
        {

            List<dynamic> lstWTCaseWorking = (from WTCase in context.WT_WaterTheftCase
                                              join WTCW in context.WT_WaterTheftCanalWire on WTCase.ID equals WTCW.WaterTheftID
                                              where WTCase.ID == _WaterTheftID
                                              && (WTCW.DesignationID == (long)Constants.Designation.SBE
                                              || WTCW.DesignationID == (long)Constants.Designation.SDO)
                                              orderby WTCW.DesignationID descending
                                              select new
                                              {
                                                  WaterTheftID = WTCW.WaterTheftID,
                                                  CanalWireNo = WTCW.CanalWireNo,
                                                  CanalWireDate = WTCW.CanalWireDate,
                                                  ClosingRepairDate = WTCase.FixDate,
                                                  DesignationID = WTCW.DesignationID
                                              }).ToList()
                                       .Select(s => new
                                       {
                                           WaterTheftID = s.WaterTheftID.Value,
                                           CanalWireNo = s.CanalWireNo,
                                           CanalWireDate = Utility.GetFormattedDate(s.CanalWireDate),
                                           ClosingRepairDate = s.ClosingRepairDate.HasValue == true ? Utility.GetFormattedDate(s.ClosingRepairDate.Value) : string.Empty,
                                           Remarks = s.DesignationID == (int)Constants.Designation.SDO ? GetSBESDORemarks(s.WaterTheftID.Value, s.DesignationID) : string.Empty,
                                           DesignationID = s.DesignationID
                                       }).ToList<dynamic>();
            return lstWTCaseWorking;
        }
        private string GetSBESDORemarks(long _WaterTheftID, long _DesignationID)
        {
            var qRemarks = (from status in context.WT_WaterTheftStatus
                            where status.WaterTheftID == _WaterTheftID
                            && (status.AssignedByDesignationID == _DesignationID && (status.AssignedToDesignationID == (long)Constants.Designation.Ziladaar || status.AssignedToDesignationID == (long)Constants.Designation.SBE))
                            orderby status.ID descending
                            select status.Remarks
                            ).FirstOrDefault();
            return qRemarks;
        }
        public int AssignWaterTheftCaseToSBEOrZiladar(WT_WaterTheftCanalWire _WTCanelWire)
        {
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteScalar("usp_AssignWaterTheftCaseToSBEOrZiladar", _WTCanelWire.WaterTheftID, _WTCanelWire.CanalWireNo, _WTCanelWire.CanalWireDate, _WTCanelWire.Remarks, _WTCanelWire.UserID, _WTCanelWire.DesignationID, _WTCanelWire.ID);
        }
        public List<object> GetWaterTheftCaseWorkingRemarks(long _WaterTheftID)
        {
            List<object> lstWaterTheftCaseWorkingRemarks = (from r in context.WT_WaterTheftStatus
                                                            join d in context.UA_Designations on r.AssignedByDesignationID equals d.ID
                                                            join u in context.UA_Users on r.AssignedByUserID equals u.ID
                                                            where r.WaterTheftID == _WaterTheftID && !string.IsNullOrEmpty(r.Remarks)
                                                            select new
                                                            {
                                                                r.Remarks,
                                                                d.Name,
                                                                u.FirstName,
                                                                u.LastName,
                                                                r.AssignedDate
                                                            }).ToList()
                                                            .Select(w => new
                                                                {
                                                                    w.Remarks,
                                                                    LoggedBy = w.FirstName + " " + w.LastName + ", " + w.Name,
                                                                    LogDateTime = Utility.GetFormattedDateTime(w.AssignedDate)
                                                                }).ToList<object>();
            return lstWaterTheftCaseWorkingRemarks;

        }
        public List<dynamic> GetWaterTheftCaseAttachment(long _WaterTheftID)
        {
            List<dynamic> lstAttachments = (from WTA in context.WT_WaterTheftAttachments
                                            join d in context.UA_Designations on WTA.AttachmentByDesignationID equals d.ID
                                            join u in context.UA_Users on WTA.AttachmentByUserID equals u.ID
                                            where WTA.WaterTheftID == _WaterTheftID && WTA.IsActive == true
                                            orderby new { WTA.ID, WTA.AttachmentByDesignationID } descending
                                            select new
                                            {
                                                WTA.AttachmentPath,
                                                WTA.FileName,
                                                d.Name,
                                                u.FirstName,
                                                u.LastName
                                            }).ToList()
                                           .Select(w => new
                                            {
                                                AttachmentPath = w.AttachmentPath,
                                                FileName = w.FileName,
                                                UploadedBy = w.FirstName + " " + w.LastName + ", " + w.Name
                                            }).ToList<dynamic>();
            return lstAttachments;

        }
        public dynamic GetWaterTheftCaseAssignee(long _WaterTheftID)
        {
            dynamic qWTStatus = (from WTS in context.WT_WaterTheftStatus
                                 join WTC in context.WT_WaterTheftCase on WTS.WaterTheftID equals WTC.ID
                                 where WTS.WaterTheftID == _WaterTheftID //&& WTS.IsActive == true && WTC.IsActive == true
                                 select new
                                 {
                                     WTS.ID,
                                     WTS.WaterTheftID,
                                     WTS.AssignedToUserID,
                                     WTS.AssignedToDesignationID,
                                     WTS.AssignedByUserID,
                                     WTS.AssignedByDesignationID,
                                     WTS.CanalWireID,
                                     WTC.OffenceSite,
                                     WTC.CaseStatusID,
                                     WTC.IncidentDateTime
                                 }).OrderByDescending(w => w.ID).FirstOrDefault();
            return qWTStatus;
        }
        public dynamic GetSBESDOWorkingByWaterTheftID(long _WaterTheftID, long _DesignationID)
        {
            dynamic qSBESDOWorking = context.WT_GetSBESDOWorkingByWaterTheftID(_WaterTheftID, _DesignationID)
                                    .Select(s => new
                                               {
                                                   s.CanalWireID,
                                                   s.WaterTheftID,
                                                   ClosingRepairDate = s.ClosingRepairDate.HasValue ? Utility.GetFormattedDate(s.ClosingRepairDate) : string.Empty,
                                                   s.CanalWireNo,
                                                   CanalWireDate = s.CanalWireDate.HasValue ? Utility.GetFormattedDate(s.CanalWireDate.Value) : string.Empty,
                                                   s.AssignedByDesignationID,
                                                   s.AssignedToDesignationID
                                               }).FirstOrDefault();
            return qSBESDOWorking;
        }
        #endregion


        #region SDO WOrking

        public WT_WaterTheftCanalWire GetUserCanalWaireNo(long _UserID, long _WTCaseID)
        {
            WT_WaterTheftCanalWire ObjCanalWaire = new WT_WaterTheftCanalWire();
            try
            {
                ObjCanalWaire = (from CanalWire in context.WT_WaterTheftCanalWire
                                 where CanalWire.WaterTheftID == _WTCaseID && CanalWire.UserID == _UserID
                                 orderby CanalWire.CanalWireDate ascending
                                 select CanalWire).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return ObjCanalWaire;
        }

        public List<object> GetAllCanalWaireNo(long _WTCaseID)
        {
            List<object> lstCanalWires = new List<object>();
            try
            {
                lstCanalWires = (from WTCase in context.WT_WaterTheftCase
                                 join CanalWire in context.WT_WaterTheftCanalWire on WTCase.ID equals CanalWire.WaterTheftID
                                 join WTStatus in context.WT_WaterTheftStatus on WTCase.ID equals WTStatus.WaterTheftID
                                 where WTCase.ID == _WTCaseID
                                 orderby CanalWire.CanalWireDate ascending
                                 select new
                                 {
                                     CanaWireNo = CanalWire.CanalWireNo,
                                     CDate = CanalWire.CanalWireDate,
                                     FxDate = WTCase.FixDate,
                                     Remarks = WTStatus.Remarks
                                 }).ToList()
                 .Select
                 (u => new
                {
                    u.CanaWireNo,
                    CanalWireDate = Utility.GetFormattedDate(u.CDate),
                    FixDate = Utility.GetFormattedDate(u.FxDate),
                    u.Remarks
                }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstCanalWires;
        }

        public WT_WaterTheftStatus GetLastStatus(long _WTCaseID)
        {
            WT_WaterTheftStatus ObjLastAssignment = new WT_WaterTheftStatus();
            try
            {
                ObjLastAssignment = (from status in context.WT_WaterTheftStatus
                                     where status.WaterTheftID == _WTCaseID
                                     orderby status.AssignedDate descending
                                     select status).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return ObjLastAssignment;
        }

        public int UpdateTawaanRecord(long? _UserID, long? _DesignationID, long? _CaseID, string _DecisionType, DateTime? _DecisionDate, int? _SpecialCharges, double? _ProposedFine, string _SDOLetterNo, DateTime _SDOLetterDate, string _FIRNo, DateTime _FIRDate, bool _Imprisonment, int? _ImprisonmentDays, string _CanalWireNo, DateTime _CanalWireDate, string _SDORemarks)
        {
            int Result = 0;
            try
            {
                object ObjManager = GetManager((long)_UserID);
                if (ObjManager != null)
                {
                    long AssignedToUserID = Convert.ToInt64(ObjManager.GetType().GetProperty("ManagerID").GetValue(ObjManager));
                    long AssignedToUserDesID = Convert.ToInt64(ObjManager.GetType().GetProperty("ManagerDesID").GetValue(ObjManager));
                    long CaseStatusID = GetCaseStatus((long)_CaseID);
                    Result = context.UpdateTawaanPoiliceCaseWorking(_UserID, _DesignationID, _CaseID, _DecisionType, _DecisionDate, _SpecialCharges, _ProposedFine,
                        _SDOLetterNo, _SDOLetterDate, _FIRNo, _FIRDate, _Imprisonment, _ImprisonmentDays, _CanalWireNo, _CanalWireDate, AssignedToUserID,
                        AssignedToUserDesID, DateTime.Now, CaseStatusID, _SDORemarks);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return Result;
            }
            return Result;
        }

        public int? InsertCaseStatus(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, string _Remarks)
        {
            int? Result = null;
            try
            {
                UA_UserManager ObjManager = (from manager in context.UA_UserManager
                                             where manager.UserID == _AssignedByUserID
                                             select manager).FirstOrDefault();
                if (ObjManager != null)
                {
                    Result = context.InsertWaterTheftStatus(_CaseID, ObjManager.ManagerID, ObjManager.ManagerDesignationID, _AssignedByUserID, _AssignedByDesignationID, DateTime.Now, 2, _Remarks);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return Result;
            }
            return Result;
        }

        public string GetOutletside(int? _RD, string _side)
        {
            string Outletside = "";
            try
            {
                if (_RD != null && _side != "")
                {
                    if (_side == "L")
                        Outletside = "Left";
                    else
                        Outletside = "Right";

                    Outletside = _RD.ToString() + " / " + Outletside;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Outletside;
        }

        #endregion

        public DataTable GetBreachSearchCriteria(string _CaseID, DateTime? _FromDate, DateTime? _ToDate, long _UserId, long _IrrigationLevelID)
        {
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteStoredProcedureDataTable("WT_SearchBreach", _UserId, _IrrigationLevelID, -1, -1, _CaseID, _FromDate, _ToDate);
        }

        public WT_Breach GetBreachCaseDatebyID(long _BreachCaseId)
        {
            WT_Breach ObjBreachData = new WT_Breach();
            try
            {
                ObjBreachData = (from val in context.WT_Breach
                                 where val.ID == _BreachCaseId
                                 select val).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return ObjBreachData;
        }

        public WT_FeettoIgnore FeetToIgnore()
        {
            WT_FeettoIgnore lstFeetToIgnore = new WT_FeettoIgnore();

            lstFeetToIgnore = (from feetignor in context.WT_FeettoIgnore
                               select feetignor).FirstOrDefault();
            return lstFeetToIgnore;

        }

        #region Tawaan Working


        public double? FineCalculation(double? _Area, int? _Rate, int? _Percentage)
        {
            double? Fine = null;
            try
            {
                if (_Area != null && _Rate != null && _Percentage != null)
                {
                    Fine = _Area * Convert.ToDouble(_Rate);
                    Fine = Fine + (Fine * (Convert.ToDouble(_Percentage) / 100));
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Fine;
        }

        public string GetCanalWire(long _WTCaseID, long _Check)
        {
            string CanalNODate = "";
            try
            {
                WT_WaterTheftCanalWire objCanalWires = (from CanalWire in context.WT_WaterTheftCanalWire
                                                        where CanalWire.WaterTheftID == _WTCaseID && CanalWire.DesignationID == (Int64)Constants.Designation.XEN
                                                        select CanalWire).FirstOrDefault();

                if (objCanalWires != null)
                {
                    if (_Check == 1)
                        CanalNODate = objCanalWires.CanalWireNo;
                    else
                        CanalNODate = (Convert.ToString(Utility.GetFormattedDate(objCanalWires.CanalWireDate)));
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return CanalNODate;
        }

        public string GetImprisonment(bool? _Imprisonment)
        {
            string YesNO = "No";
            if (_Imprisonment == true)
                YesNO = "Yes";

            return YesNO;
        }

        //public object GetFineCalculation(long _WTCaseID)
        //{
        //    object FineValues = new object();
        //    try
        //    {
        //        FineValues = (from WTFineDetail in context.WT_WaterTheftDecisionFineDetail
        //                      join AreaTyp in context.WT_AreaType on WTFineDetail.AreaTypeID equals AreaTyp.ID
        //                      join Abiana in context.WT_Abiana on AreaTyp.ID equals Abiana.AreaTypeID
        //                      where WTFineDetail.WatertTheftID == _WTCaseID
        //                      select new
        //                      {
        //                          DecisionType = WTFineDetail.DecisionType,
        //                          DecisionDate = WTFineDetail.DecisionDate,
        //                          AreaBooked = WTFineDetail.AreaBooked,
        //                          SpecialCharges = WTFineDetail.SpecialCharges,
        //                          ProposedFine = WTFineDetail.ProposedFine,
        //                          AreaName = AreaTyp.Name,
        //                          AbianaRate = Abiana.AbianaRate,
        //                          MaxPercentage = Abiana.MaxPercentage
        //                      }).ToList().
        //                            Select(u => new
        //                            {
        //                                u.DecisionType,
        //                                DecisionDate = Utility.GetFormattedDate(u.DecisionDate),
        //                                u.AreaBooked,
        //                                u.SpecialCharges,
        //                                u.ProposedFine,
        //                                u.AreaName,
        //                                Fine = FineCalculation(u.AreaBooked, u.AbianaRate, u.MaxPercentage)
        //                            }).FirstOrDefault();
        //    }
        //    catch (Exception exp)
        //    {
        //        new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
        //    }

        //    return FineValues;
        //}

        public int? GetChiefAppealAmount(long _WTCaseID)
        {
            int? FeeAmount = null;
            try
            {
                FeeAmount = (from CAD in context.WT_ChiefAppealDetails
                             where CAD.WatertheftID == _WTCaseID
                             select CAD.FeeAmount).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return FeeAmount;
        }

        public object GetFineCalculation(long _WTCaseID)
        {
            object FineValues = new object();
            try
            {
                FineValues = (from WTFineDetail in context.WT_WaterTheftDecisionFineDetail
                              join Police in context.WT_WaterTheftPoliceCase on WTFineDetail.WaterTheftID equals Police.WaterTheftID into A
                              from a in A.DefaultIfEmpty()
                              join AreaTyp in context.WT_AreaType on WTFineDetail.AreaTypeID equals AreaTyp.ID
                              join Abiana in context.WT_Abiana on AreaTyp.ID equals Abiana.AreaTypeID
                              where WTFineDetail.WaterTheftID == _WTCaseID
                              select new
                              {
                                  DecisionType = WTFineDetail.DecisionType,
                                  DecisionDate = WTFineDetail.DecisionDate,
                                  AreaBooked = WTFineDetail.AreaBooked,
                                  SpecialCharges = WTFineDetail.SpecialCharges,
                                  ProposedFine = WTFineDetail.ProposedFine,
                                  AreaName = AreaTyp.Name,
                                  AbianaRate = Abiana.AbianaRate,
                                  MaxPercentage = Abiana.MaxPercentage,
                                  LetterSDOToPolice = a.SDOLetterNo,
                                  FIRNumber = a.FIRNo,
                                  FIRDate = a.FIRDate,
                                  NoOfAccussed = a.NoOfAccused,
                                  ProcessedDate = a.ProcedingDate,
                                  Inprisonment = a.Imprisonment,
                                  InprisonmentDays = a.ImprisonmentDays,
                                  a.SDORemarks,
                                  a.ZildarRemarks,
                              }).ToList().
                                    Select(u => new
                                    {
                                        u.DecisionType,
                                        DecisionDate = Utility.GetFormattedDate(u.DecisionDate),
                                        u.AreaBooked,
                                        u.SpecialCharges,
                                        u.ProposedFine,
                                        u.AreaName,
                                        Fine = FineCalculation(u.AreaBooked, u.AbianaRate, u.MaxPercentage),
                                        u.LetterSDOToPolice,
                                        u.FIRNumber,
                                        FIRDate = Utility.GetFormattedDate(u.FIRDate),
                                        CaseToXENNo = GetCanalWire(_WTCaseID, 1),
                                        CaseToXENDate = GetCanalWire(_WTCaseID, 2),
                                        u.NoOfAccussed,
                                        ProcessedDate = Utility.GetFormattedDate(u.ProcessedDate),
                                        u.ZildarRemarks,
                                        Imprisonment = GetImprisonment(u.Inprisonment),
                                        u.InprisonmentDays,
                                        FeeAmount = GetChiefAppealAmount(_WTCaseID),
                                        u.SDORemarks
                                    }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return FineValues;
        }

        #endregion


        #region SE Working


        public int AppealFromSE(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, long _CaseStatus, string _Remarks)
        {
            int Result = 0;
            try
            {
                WT_WaterTheftStatus LastStatus = GetLastStatus((long)_CaseID);
                Result = context.InsertWaterTheftStatus(_CaseID, LastStatus.AssignedToUserID, LastStatus.AssignedToDesignationID, _AssignedByUserID, _AssignedByDesignationID, DateTime.Now, _CaseStatus, _Remarks);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return Result;
            }
            return Result;
        }

        #endregion

        public WT_WaterTheftCase GetWaterTheftID()
        {
            WT_WaterTheftCase lstWaterTheftCase = new WT_WaterTheftCase();

            lstWaterTheftCase = (from wtcase in context.WT_WaterTheftCase
                                 select wtcase).OrderByDescending(c => c.ID).FirstOrDefault();
            return lstWaterTheftCase;

        }

        #region SE Working

        public int? InsertCaseStatusForSubOrdinate(long? _CaseID, long? _AssignedByUserID, long? _AssignedByDesignationID, string _Remarks, long? _UserDesignation)
        {
            int? Result = null;
            try
            {
                UA_UserManager SubOrdinate = (from UM in context.UA_UserManager
                                              where UM.ManagerID == _AssignedByUserID && UM.UserDesignationID == _UserDesignation
                                              select UM).FirstOrDefault();

                if (SubOrdinate != null)
                {
                    long CaseStatusID = (from WTC in context.WT_WaterTheftCase
                                         where WTC.ID == _CaseID
                                         select WTC.CaseStatusID).FirstOrDefault();

                    Result = context.InsertWaterTheftStatus(_CaseID, SubOrdinate.UserID, SubOrdinate.UserDesignationID, _AssignedByUserID, _AssignedByDesignationID, DateTime.Now, CaseStatusID, _Remarks);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return Result;
            }
            return Result;
        }

        #endregion

        #region Chief Working



        #endregion
        public List<object> GetOffenceType()
        {
            List<object> lstOfOffenceType = (from val in context.WT_OffenceType
                                             where val.ChannelOutlet == "C"
                                             select new
                                             {
                                                 ID = val.ID,
                                                 Name = val.Name,
                                             }).ToList<object>();

            return lstOfOffenceType;
        }
        public List<object> GetListOfAbiana()
        {
            List<object> lstOfAbiana = (from abiana in context.WT_Abiana
                                        join areatype in context.WT_AreaType on abiana.AreaTypeID equals areatype.ID
                                        select new
                                        {
                                            ID = abiana.ID,
                                            Name = areatype.Name,
                                            AbianaRate = abiana.AbianaRate,
                                            MaxPercentage = abiana.MaxPercentage
                                        }).ToList<object>();

            return lstOfAbiana;
        }

        public int SaveOutletChannelIncidentCase(WT_WaterTheftCase _mdlWatertheftCase, WT_WaterTheftStatus _mdlWaterTheftStaus, WT_OutletDefectiveDetails _mdloutletDefectiveDetails)
        {
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteScalar("WT_InsertWaterTheftCase",
                                            _mdlWatertheftCase.OffenceSite,
                                            _mdlWatertheftCase.ChannelID,
                                            _mdlWatertheftCase.TheftSiteRD,
                                            _mdlWatertheftCase.OutletID,
                                            _mdlWatertheftCase.OffenceTypeID,
                                            _mdlWatertheftCase.OffenceSide,
                                            _mdlWatertheftCase.TheftSiteConditionID,
                                            _mdlWatertheftCase.IncidentDateTime,
                                            _mdlWatertheftCase.SitePhoto,
                                            _mdlWatertheftCase.Source,
                                            _mdlWatertheftCase.ValueofH,
                                            _mdlWatertheftCase.UserID,
                                            _mdlWatertheftCase.LogDateTime,
                                            _mdlWatertheftCase.Remarks,
                                            _mdlWatertheftCase.FixDate,
                                            _mdlWatertheftCase.CaseStatusID,
                                            _mdlWatertheftCase.IsActive,
                                            _mdlWatertheftCase.CreatedDate,
                                            _mdlWatertheftCase.CreatedBy,
                                            _mdlWatertheftCase.ModifiedDate, _mdlWatertheftCase.ModifiedBy, _mdlWatertheftCase.CaseNo, _mdlWatertheftCase.GIS_X, _mdlWatertheftCase.GIS_Y,
                // Water Theft Status Propterty
                                       _mdlWaterTheftStaus.AssignedToUserID,
                                       _mdlWaterTheftStaus.AssignedToDesignationID,
                                       _mdlWaterTheftStaus.AssignedByUserID,
                                       _mdlWaterTheftStaus.AssignedByDesignationID,
                                       _mdlWaterTheftStaus.AssignedDate, _mdlWaterTheftStaus.CanalWireID,
                // Outlet Defective Details
                                       _mdloutletDefectiveDetails.DefectiveTypeID,
                                       _mdloutletDefectiveDetails.ValueOfB,
                                       _mdloutletDefectiveDetails.ValueOfY, _mdloutletDefectiveDetails.ValueOfDia);
        }

        public DataTable VerfiyChannelRDs(int _ChannelId, long _UserID, long _IrrigationLevelID)
        {
            string ChannelId = _ChannelId.ToString();
            ContextDB dbADO = new ContextDB();
            string GetSection = "SELECT [dbo].[WT_GetSectionIDsByUserAndDivision] ({0},{1},{2})";
            Object[] parameters = { _UserID, _IrrigationLevelID, -1 };
            var lstSectionIDsComma = context.Database.SqlQuery<string>(GetSection, parameters).FirstOrDefault();
            var result = dbADO.ExecuteStoredProcedureDataTable("WT_GetChannelIDAndRDsBySectionIDs", lstSectionIDsComma, ChannelId);
            return result;
        }

        public int AddWaterTheftAttachments(DataTable _WTAttachments)
        {
            WRMIS_Entities context = new WRMIS_Entities();
            var param = new SqlParameter("@tblAttachments", SqlDbType.Structured);
            param.Value = _WTAttachments;
            param.TypeName = "dbo.WTAttachments";
            string sql = String.Format("EXEC {0} {1};", "dbo.WT_SaveAttachments", "@tblAttachments");
            return context.Database.ExecuteSqlCommand(sql, param);
        }

        public int SaveBreachAttachments(DataTable _WTAttachments)
        {
            WRMIS_Entities context = new WRMIS_Entities();
            var param = new SqlParameter("@tblAttachments", SqlDbType.Structured);
            param.Value = _WTAttachments;
            param.TypeName = "dbo.WTBreachAttachments";
            string sql = String.Format("EXEC {0} {1};", "dbo.WT_SaveBreachAttachments", "@tblAttachments");
            return context.Database.ExecuteSqlCommand(sql, param);
        }

        public List<object> GetBreachCaseAttachment(long _breachCaseId)
        {
            List<object> lstAttachments = (from BTA in context.WT_BreachAttachments
                                           join d in context.UA_Designations on BTA.AttachmentByDesignationID equals d.ID
                                           join u in context.UA_Users on BTA.AttachmentByUserID equals u.ID
                                           where BTA.BreachID == _breachCaseId && BTA.IsActive == true
                                           select new
                                           {
                                               BTA.AttachmentPath,
                                               BTA.FileName,
                                               d.Name,
                                               u.FirstName,
                                               u.LastName
                                           }).ToList()
                                           .Select(w => new
                                           {
                                               FileName = w.FileName,
                                               AttachmentPath = w.AttachmentPath,
                                               UploadedBy = w.FirstName + " " + w.LastName + ", " + w.Name
                                           }).ToList<object>();
            return lstAttachments;

        }

        public WT_Breach GetBreachCaseID()
        {
            WT_Breach lstBreachCase = new WT_Breach();

            lstBreachCase = (from brcase in context.WT_Breach
                             select brcase).OrderByDescending(c => c.ID).FirstOrDefault();
            return lstBreachCase;

        }

        public bool IsBreachCaseExist(WT_Breach _mdlBreachCase, int _NoofFeet)
        {
            bool qIsCaseExists = false;
            qIsCaseExists = context.WT_Breach.Any(c => DbFunctions.TruncateTime(c.DateTime) == DbFunctions.TruncateTime(_mdlBreachCase.DateTime)
                             && (c.BreachSiteRD >= _mdlBreachCase.BreachSiteRD - _NoofFeet
                             && c.BreachSiteRD <= _mdlBreachCase.BreachSiteRD + _NoofFeet)
                             && c.BreachSide == _mdlBreachCase.BreachSide
                             && c.ChannelID == _mdlBreachCase.ChannelID);
            return qIsCaseExists;
        }

        #region Android Application
        /// Coded by : Hira Iqbal 
        public DataTable RDsWithinSection(long _ChannelID, long _SectionID)
        {
            ContextDB dbADO = new ContextDB();
            var result = dbADO.ExecuteStoredProcedureDataTable("WT_GetChannelIDAndRDsBySectionIDs", _SectionID.ToString(), _ChannelID.ToString());
            return result;
        }

        public bool WaterTheftOutletCaseAlreadyExist(WT_WaterTheftCase _mdlWatertheftCase)
        {
            bool qIsCaseExists = false;
            qIsCaseExists = context.WT_WaterTheftCase.Any(c => DbFunctions.TruncateTime(c.IncidentDateTime) == DbFunctions.TruncateTime(_mdlWatertheftCase.IncidentDateTime)
                             && c.OffenceTypeID == _mdlWatertheftCase.OffenceTypeID
                             && c.ChannelID == _mdlWatertheftCase.ChannelID
                             && c.OutletID == _mdlWatertheftCase.OutletID
                             && c.TheftSiteConditionID == _mdlWatertheftCase.TheftSiteConditionID);
            return qIsCaseExists;
        }
        /// Coded by : Hira Iqbal : End
        #endregion

        #region
        public object GetInformationbyCaseNo(string _CaseNo)
        {
            ContextDB dbADO = new ContextDB();
            var WaterThefCaseData = (from wtc in context.WT_WaterTheftCase
                                     //    join users in context.UA_Users on new { A = cs.AssignedToUserID, B = cs.AssignedByUserID } equals new { A = users.ID, B = users.ID }
                                     where wtc.CaseNo == _CaseNo
                                     select new
                                     {
                                         wtc.TheftSiteRD,
                                         wtc.ChannelID
                                     }).FirstOrDefault();

            var value = (dbADO.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Where(i => i.ChannelID == WaterThefCaseData.ChannelID && i.SectionRD <= WaterThefCaseData.TheftSiteRD).Max(i => (int?)i.SectionRD));


            var record = (from cib in context.CO_ChannelIrrigationBoundaries
                          join channel in context.CO_Channel on cib.ChannelID equals channel.ID
                          join sec in context.CO_Section on cib.SectionID equals sec.ID
                          join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                          join div in context.CO_Division on subdiv.DivisionID equals div.ID
                          join cir in context.CO_Circle on div.CircleID equals cir.ID
                          join zone in context.CO_Zone on cir.ZoneID equals zone.ID
                          join wtcase in context.WT_WaterTheftCase on cib.ChannelID equals wtcase.ChannelID
                          join cs in context.WT_WaterTheftStatus on wtcase.ID equals cs.WaterTheftID
                          join users in context.UA_Users on cs.AssignedToUserID equals users.ID
                          join user in context.UA_Users on cs.AssignedByUserID equals user.ID
                          where cib.SectionRD == value && wtcase.CaseNo == _CaseNo
                          select new
                          {
                              ChannelName = channel.NAME,
                              SectionName = sec.Name,
                              SubDivisionName = subdiv.Name,
                              DivisionName = div.Name,
                              CircleName = cir.Name,
                              ZoneName = zone.Name,
                              AssignedToName = users.LoginName,
                              AssignedByName = user.LoginName,
                              StatusID = cs.ID,

                          }).OrderByDescending(c => c.StatusID).FirstOrDefault();




            return record;
        }
        #endregion

        #region roleRights

        public UA_RoleRights GetRoleRights(long _UserID, long _PageID)
        {
            UA_RoleRights objRR = new UA_RoleRights();
            try
            {
                objRR = (from usr in context.UA_Users
                         join rr in context.UA_RoleRights on usr.RoleID equals rr.RoleID
                         where usr.ID == _UserID && rr.PageID == _PageID
                         select rr).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objRR;
        }


        #endregion
    }
}
