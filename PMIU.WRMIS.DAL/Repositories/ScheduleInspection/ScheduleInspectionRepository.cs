using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.ScheduleInspection
{
    public class ScheduleInspectionRepository : Repository<SI_Schedule>
    {
        public ScheduleInspectionRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<SI_Schedule>();

        }

        #region "View Schedule"
        /// <summary>
        /// This function return Schedule by ID
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>SI_Schedule</returns>
        public SI_Schedule GetSchedule(long _ID)
        {
            SI_Schedule qSchedule = new SI_Schedule();

            //qSchedule = (from s in context.SI_Schedule
            //             where s.ID == _ID
            //             select s).FirstOrDefault();
            return qSchedule;
        }
        /// <summary>
        /// This function return Schedule Status by Schedule ID
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public SI_ScheduleStatus GetScheduleStatusByScheduleID(long _ID)
        {
            SI_ScheduleStatus qScheduleStatus = new SI_ScheduleStatus();

            //SI_ScheduleStatus qScheduleStatus = (from ss in context.SI_ScheduleStatus
            //                                     join ssd in context.SI_ScheduleStatusDetail on ss.ID equals ssd.ScheduleStatusID
            //                                     where ssd.ScheduleID == _ID
            //                                     select ss
            //                                     ).FirstOrDefault();
            return qScheduleStatus;
        }
        private dynamic GetMinInspectionRD(List<dynamic> lstInspection)
        {
            dynamic minRD = lstInspection.Min(i => i.InspectionArea);
            return minRD;
        }
        private dynamic GetMaxInspectionRD(List<dynamic> lstInspection)
        {
            dynamic maxRD = lstInspection.Max(i => i.InspectionArea);
            return maxRD;
        }
        private List<dynamic> GetInspectionArea(List<dynamic> lstInspecton)
        {
            // Get Min Inspection RD and replace with Head
            List<dynamic> lstInspectionArea = lstInspecton.Select(a => new
            {
                ScheduleID = a.ScheduleID,
                Division = a.Division,
                ChannelHeadWorkName = a.ChannelHeadWorkName,
                InspectionArea = Regex.Replace(Convert.ToString(a.InspectionArea), Convert.ToString(GetMinInspectionRD(lstInspecton)), "Head"),
                DateOfVisit = a.DateOfVisit,
                Remarks = a.Remarks
            }).ToList<dynamic>();

            // Get Max Inspection Rd and replace with Tail
            lstInspectionArea = lstInspectionArea.Select(a => new
            {
                ScheduleID = a.ScheduleID,
                Division = a.Division,
                ChannelHeadWorkName = a.ChannelHeadWorkName,
                InspectionArea = Regex.Replace(Convert.ToString(a.InspectionArea), Convert.ToString(GetMaxInspectionRD(lstInspecton)), "Tail"),
                DateOfVisit = a.DateOfVisit,
                Remarks = a.Remarks
            }).ToList<dynamic>();

            return lstInspectionArea;
        }
        private List<object> GetHeadWorkInspectionByScheduleID(long _ID, long _InspectionType)
        {
            long headWork = Convert.ToInt64(Constants.StructureType.Headwork);

            List<object> lstHeadWorkInspection = new List<object>();

            //List<object> lstHeadWorkInspection = (from GI in context.SI_ScheduleDetailChannel
            //                                      join division in context.CO_Division on GI.DivisionID equals division.ID
            //                                      join HW in context.CO_Station on new { stationID = (long)GI.StationID, stationType = (long)GI.StructureTypeID }
            //                                      equals new { stationID = HW.ID, stationType = headWork }
            //                                      where GI.ScheduleID == _ID && GI.InspectionTypeID == _InspectionType
            //                                      select new
            //                                      {
            //                                          ScheduleID = GI.ScheduleID,
            //                                          Division = division.Name,
            //                                          ChannelHeadWorkName = HW.Name,
            //                                          InspectionArea = GI.InspectionAreaRDs,
            //                                          DateOfVisit = GI.ScheduleDate,
            //                                          Remarks = GI.Remarks
            //                                      }).ToList()
            //                                      .Select(h => new
            //                                      {
            //                                          ScheduleID = h.ScheduleID,
            //                                          Division = h.Division,
            //                                          ChannelHeadWorkName = h.ChannelHeadWorkName,
            //                                          InspectionArea = h.InspectionArea,
            //                                          DateOfVisit = Utility.GetFormattedDate(h.DateOfVisit),
            //                                          Remarks = h.Remarks
            //                                      }).ToList<object>();
            return lstHeadWorkInspection;
        }
        private List<object> GetChannelInspectionByScheduleID(long _ID, long _InspectionType)
        {
            long channel = Convert.ToInt64(Constants.StructureType.Channel);

            List<object> lstInspection = new List<object>();

            //List<object> lstInspection = (from GI in context.SI_ScheduleDetailChannel
            //                              join division in context.CO_Division on GI.DivisionID equals division.ID
            //                              join c in context.CO_Channel on new { stationID = (long)GI.StationID, stationType = (long)GI.StructureTypeID }
            //                              equals new { stationID = c.ID, stationType = channel }
            //                              where GI.ScheduleID == _ID && GI.InspectionTypeID == _InspectionType
            //                              select new
            //                              {
            //                                  ScheduleID = GI.ScheduleID,
            //                                  Division = division.Name,
            //                                  ChannelHeadWorkName = c.NAME,
            //                                  InspectionArea = GI.InspectionAreaRDs,
            //                                  DateOfVisit = GI.ScheduleDate,
            //                                  Remarks = GI.Remarks
            //                              }).ToList()
            //                                      .Select(h => new
            //                                      {
            //                                          ScheduleID = h.ScheduleID,
            //                                          Division = h.Division,
            //                                          ChannelHeadWorkName = h.ChannelHeadWorkName,
            //                                          InspectionArea = h.InspectionArea,
            //                                          DateOfVisit = Utility.GetFormattedDate(Convert.ToDateTime(h.DateOfVisit)),
            //                                          Remarks = h.Remarks
            //                                      }).ToList<object>();
            return lstInspection;
        }

        /// <summary>
        /// This function return Gauge Inspection 
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetGaugeInspectionsByScheduleID(long _ID)
        {
            List<dynamic> lstChannelInspection = GetChannelInspectionByScheduleID(_ID, 1);
            List<dynamic> lstHeadWorkInspection = GetHeadWorkInspectionByScheduleID(_ID, 1);
            List<dynamic> lstGaugeInspection = null;

            if ((lstChannelInspection != null && lstChannelInspection.Count > 0) || (lstHeadWorkInspection != null && lstHeadWorkInspection.Count > 0))
            {
                List<dynamic> lstInspection = lstChannelInspection.Union(lstHeadWorkInspection).ToList<dynamic>();
                lstGaugeInspection = GetInspectionArea(lstInspection);
            }
            return lstGaugeInspection;
        }
        /// <summary>
        /// This function return Outlet Alteration
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletAlterationByScheduleID(long _ID)
        {
            List<object> lstInspection = GetChannelInspectionByScheduleID(_ID, 4);
            List<object> lstOutletAlteration = GetInspectionArea(lstInspection);
            return lstOutletAlteration;
        }
        /// <summary>
        /// This function return schedule works
        /// Created on: 02-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetWorksByScheduleID(long _ID)
        {
            List<object> lstWork = new List<object>();

            //List<object> lstWork = (from work in context.SI_ScheduleDetailWorks
            //                        join division in context.CO_Division on work.DivisionID equals division.ID
            //                        join workType in context.SI_WorkType on work.WorkSubTypeID equals workType.ID
            //                        join workCategory in context.SI_SubWorkType on workType.ID equals workCategory.WorkTypeID
            //                        select new
            //                        {
            //                            Division = division.Name,
            //                            TypeOfWork = workType.Name,
            //                            WorkSubCategory = workCategory.Name,
            //                            DateOfVisit = work.ScheduleDate,
            //                            Remarks = work.Remarks
            //                        }).ToList<object>();
            return lstWork;
        }
        /// <summary>
        /// This function return Outlet Performance Register
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletPerformanceRegisterByScheduleID(long _ID)
        {
            List<object> lstInspection = GetChannelInspectionByScheduleID(_ID, 3);
            List<object> OutletPerformanceRegister = GetInspectionArea(lstInspection);
            return OutletPerformanceRegister;
        }
        public List<object> GetTenderByScheduleID(long _ID)
        {
            List<object> lstTender = new List<object>();
            return lstTender;
        }
        /// <summary>
        /// This function return Discharge Measurement
        /// Created on: 03-12-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDischargeMeasurementByScheduleID(long _ID)
        {
            List<object> lstChannelInspection = GetChannelInspectionByScheduleID(_ID, 2);
            List<object> lstHeadWorkInspection = GetHeadWorkInspectionByScheduleID(_ID, 2);
            List<object> lstDischargeMeasurement = null;

            if (lstChannelInspection != null && lstChannelInspection.Count > 0 && lstHeadWorkInspection != null && lstHeadWorkInspection.Count > 0)
            {
                List<object> lstInspection = lstChannelInspection.Union(lstHeadWorkInspection).ToList<object>();
                lstDischargeMeasurement = GetInspectionArea(lstInspection);
            }
            return lstDischargeMeasurement;
        }

        /// <summary>
        /// This function return Schedule by UserID
        /// Created on: 08-09-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>List<SI_GetUserSchedule_Result></returns>

        public List<SI_GetUserSchedule_Result> GetUserSchedule(long _UserID)
        {

            var lstSchedule = (from g in context.SI_GetUserSchedule(_UserID)
                               select g).ToList();

            return lstSchedule.ToList<SI_GetUserSchedule_Result>();
        }
        //// <summary>
        /// This function return Schedule Inspection Types by ScheduleID
        /// Created on: 09-09-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <returns>List<SI_ScheduleTypes_Result></returns>

        public List<SI_ScheduleTypes_Result> GetScheduleInspectionTypes(long _ScheduleID)
        {
            var lstSchedule = (from g in context.SI_ScheduleTypes(_ScheduleID)
                               select g).ToList();

            return lstSchedule.ToList<SI_ScheduleTypes_Result>();
        }
        //// <summary>
        /// This function  return Gauges Inspection Areas List by Schedule ID
        /// Created on: 09-09-2016
        /// <param name="_ScheduleID"></param>
        /// <param name="_InspectionTypeID"></param>
        /// <returns>List<SI_GetGaugeInspectionAreas_Result></returns>

        public List<SI_GetGaugeInspectionAreas_Result> GetGaugesInspectionAreasRecords(long _ScheduleID, long _InspectionTypeID)
        {

            var lstSchedule = (from g in context.SI_GetGaugeInspectionAreas(_ScheduleID, _InspectionTypeID)
                               select g).ToList();

            return lstSchedule.ToList<SI_GetGaugeInspectionAreas_Result>();
        }

        //// <summary>
        /// This function return Outlet Inspection Areas List by Schedule ID
        /// Created on: 09-09-2016
        /// <param name="_ScheduleID"></param>
        /// <param name="_InspectionTypeID"></param>
        /// <returns>List<SI_GetOutletInspectionAreas_Result></returns>
        public List<SI_GetOutletInspectionAreas_Result> GetOutletsInspectionAreasRecord(long _ScheduleID, long _InspectionTypeID)
        {

            var lstSchedule = (from g in context.SI_GetOutletInspectionAreas(_ScheduleID, _InspectionTypeID)
                               select g).ToList();

            return lstSchedule.ToList<SI_GetOutletInspectionAreas_Result>();
        }
        #endregion

        #region AddInspection

        public List<object> getChnlAndHeadWrkForDvns(int dvsnID)   // headWork still to be added wait from DB team (moneeb bhai)
        {
            try
            {
                List<object> lstChannels = new List<object>();

                //List<object> lstChannels = (from div in context.CO_Division
                //                            join subDiv in context.CO_SubDivision on div.ID equals subDiv.DivisionID
                //                            join sec in context.CO_Section on subDiv.ID equals sec.SubDivID
                //                            join irrBound in context.CO_ChannelIrrigationBoundaries on sec.ID equals irrBound.SectionID
                //                            join chnl in context.CO_Channel on irrBound.ID equals chnl.ChannelTypeID
                //                            where div.ID == dvsnID
                //                            select new { chnl.ID, chnl.NAME }
                //                 ).ToList<object>();

                return lstChannels;

            }
            catch (WRException exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.DataAccess);
                return new List<object>();
            }
        }


        public List<object> getChannelsForDivisions(int dvsnIDID)
        {
            List<object> lstchannels = new List<object>();

            try
            {
                //lstchannels = (from div in context.CO_Division
                //               join subDiv in context.CO_SubDivision on div.ID equals subDiv.DivisionID
                //               join sec in context.CO_Section on subDiv.ID equals sec.SubDivID
                //               join irrBound in context.CO_ChannelIrrigationBoundaries on sec.ID equals irrBound.SectionID
                //               join chnl in context.CO_Channel on irrBound.ID equals chnl.ChannelTypeID
                //               where div.ID == dvsnIDID
                //               select new { chnl.ID, chnl.NAME }
                //                 ).ToList<object>();

                return lstchannels;
            }
            catch (WRException exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.DataAccess);
                return new List<object>();
            }
        }


        public List<CO_ChannelGauge> getInspectionAreaForChannel(int chnlID)
        {
            List<CO_ChannelGauge> listInsp = new List<CO_ChannelGauge>();

            try
            {
                //listInsp = (from chnlGage in context.CO_ChannelGauge
                //            where chnlGage.ChannelID == chnlID
                //            select chnlGage
                //                 ).ToList();      // we have to show data in increasing order
                return listInsp;

            }
            catch (WRException exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.DataAccess);
                return listInsp;
            }
        }


        public List<CO_ChannelOutlets> getInspectionAreaForHeadWorks(int headWrkID)  // for headwork AND channel to outletRD  
        {
            List<CO_ChannelOutlets> listInsp = new List<CO_ChannelOutlets>();

            try
            {
                //listInsp = (from chnlGage in context.CO_ChannelOutlets
                //            where chnlGage.ChannelID == headWrkID
                //            select chnlGage
                //                 ).ToList();      // we have to show data in increasing order
                return listInsp;

            }
            catch (WRException exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.DataAccess);
                return listInsp;
            }
        }






        #endregion

        #region EditInspection

        public List<object> GetGuageAndDischargeRecords(int scheduleID, int Guage_Discharge)
        {
            //List<SI_ScheduleDetailChannel> resChannel = (from sce in context.SI_ScheduleDetailChannel
            //                                             where sce.ScheduleID == scheduleID && sce.InspectionTypeID == Guage_Discharge  // 16 for guage and 19 for discharge
            //                                             select sce
            //                                             ).ToList();

            List<object> lstChannel = new List<object>();
            object ChannelObj;

            //foreach (var guage in resChannel)
            //{
            //    ChannelObj = new object();

            //    string divName = (from div in context.CO_Division
            //                      where div.ID == guage.DivisionID
            //                      select div.Name).FirstOrDefault();

            //    if (guage.StructureTypeID == 6)  // inspection is for channel
            //    {
            //        string ChnlName = (from chnl in context.CO_Channel
            //                           where chnl.ID == guage.StationID
            //                           select chnl.NAME).FirstOrDefault();

            //        string ResInspection = (from gage in context.CO_ChannelGauge
            //                                where gage.ID == guage.InspectionAreaRDs
            //                                select gage.GaugeAtRD).FirstOrDefault().ToString();
            //        ChannelObj = new
            //        {
            //            DivisionName = divName,
            //            ChannelName = ChnlName,
            //            Inspection = ResInspection,
            //            Date = guage.ScheduleDate,
            //            Remarks = guage.Remarks
            //        };
            //    }
            //    else // inspection is for headwork
            //    {
            //        string ResHeadWork = (from stat in context.CO_Station
            //                              where stat.ID == guage.StationID
            //                              select stat.Name).FirstOrDefault();

            //        string ResInspection = "";

            //        if (guage.InspectionAreaRDs == -1)
            //        {
            //            ResInspection = "UpStream";
            //        }
            //        else
            //        {
            //            ResInspection = "DownStream";
            //        }

            //        ChannelObj = new
            //        {
            //            DivisionName = divName,
            //            ChannelName = ResHeadWork,
            //            Inspection = ResInspection,
            //            Date = guage.ScheduleDate,
            //            Remarks = guage.Remarks

            //        };
            //    }

            //    lstChannel.Add(ChannelObj);
            //}

            return lstChannel;
        }

        public List<object> GetOutletAndPerformanceRecords(int scheduleID, int Altration_Performance)
        {
            List<object> ResOutletAltration = new List<object>();

            //List<object> ResOutletAltration = (from sce in context.SI_ScheduleDetailChannel
            //                                   join div in context.CO_Division on sce.DivisionID equals div.ID
            //                                   join chnl in context.CO_Channel on sce.StationID equals chnl.ID
            //                                   join chnlgage in context.CO_ChannelGauge on sce.InspectionAreaRDs equals chnlgage.ID
            //                                   where sce.ScheduleID == scheduleID && sce.InspectionTypeID == Altration_Performance  // 17 for outlet altration and 18 for outlet performance
            //                                   select new
            //                                   {
            //                                       DivisionName = div.Name,
            //                                       ChannelName = chnl.NAME,
            //                                       Inspection = chnlgage.GaugeAtRD,
            //                                       Date = sce.ScheduleDate,
            //                                       Remarks = sce.Remarks
            //                                   }
            //                           ).ToList<object>();

            return ResOutletAltration;
        }

        public List<object> GetWorksRecords(long _ID)
        {
            List<object> lstWork = new List<object>();

            //List<object> lstWork = (from work in context.SI_ScheduleDetailWorks
            //                        join division in context.CO_Division on work.DivisionID equals division.ID
            //                        join workType in context.SI_WorkType on work.WorkSubTypeID equals workType.ID
            //                        join workCategory in context.SI_SubWorkType on workType.ID equals workCategory.WorkTypeID
            //                        select new
            //                        {
            //                            Division = division.Name,
            //                            TypeOfWork = workType.Name,
            //                            WorkSubCategory = workCategory.Name,
            //                            DateOfVisit = work.ScheduleDate,
            //                            Remarks = work.Remarks
            //                        }).ToList<object>();
            return lstWork;
        }

        public SI_Schedule GetScheduleDetail(int ScheduleID)
        {
            SI_Schedule SceDetail = new SI_Schedule();

            //SI_Schedule SceDetail = (from sce in context.SI_Schedule
            //                         where sce.ID == ScheduleID
            //                         select sce).FirstOrDefault();
            return SceDetail;
        }

        public bool EditAllowedToUser(int ScheduleID, int UserID)
        {
            bool AllowEdit = false;

            //List<SI_ScheduleStatusDetail> lstDetail = (from sceStat in context.SI_ScheduleStatusDetail
            //                                           where sceStat.ScheduleID == ScheduleID && sceStat.UserID == UserID
            //                                           select sceStat).ToList();
            //if (lstDetail.Last().ScheduleStatusID == 1)//here ID =1 says that he can edit record because schedule is in process 
            //{
            //    AllowEdit = true;
            //}
            return AllowEdit;
        }

        public List<object> GetComments(int _ScheduleID)
        {
            List<object> lstComments = new List<object>();

            //List<object> lstComments = (from sceDet in context.SI_ScheduleStatusDetail
            //                            join scestat in context.SI_ScheduleStatus on sceDet.ScheduleStatusID equals scestat.ID
            //                            join usr in context.UA_Users on sceDet.UserID equals usr.ID
            //                            where sceDet.ScheduleID == _ScheduleID
            //                            select new
            //                            {
            //                                Heading = scestat.Description,
            //                                User = usr.FirstName + " " + usr.LastName + ",",
            //                                Date = sceDet.CommentsDate,
            //                                comment = sceDet.Comments
            //                            }
            //                       ).ToList<object>();
            return lstComments;
        }



        #endregion

        #region Delete Inspection

        public bool DeleteGuageOutletPerformanceDischarge(int ScheduleID)
        {
            bool DelResult = false;

            //SI_ScheduleDetailChannel OutletObj = (from sceDet in context.SI_ScheduleDetailChannel
            //                                      where sceDet.ID == ScheduleID
            //                                      select sceDet).FirstOrDefault();
            //if (OutletObj != null)
            //{
            //    context.SI_ScheduleDetailChannel.Remove(OutletObj);
            //    context.SaveChanges();
            //    DelResult = true;
            //}

            return DelResult;
        }

        public bool DeleteWork(int WorkID)
        {
            bool DelResult = false;
            //SI_ScheduleDetailWorks WorkObj = (from work in context.SI_ScheduleDetailWorks
            //                                  where work.ID == WorkID
            //                                  select work).FirstOrDefault();

            //if (WorkObj != null && WorkObj.ID > 0)
            //{
            //    context.SI_ScheduleDetailWorks.Remove(WorkObj);
            //    context.SaveChanges();
            //    DelResult = true;
            //}

            return DelResult;
        }

        public bool DeleteTender(int TenderID)
        {
            bool DelResult = false;
            //SI_ScheduleDetailTender TendObj = (from tend in context.SI_ScheduleDetailTender
            //                                   where tend.ID == TenderID
            //                                   select tend).FirstOrDefault();

            //if (TendObj != null && TendObj.ID > 0)
            //{
            //    context.SI_ScheduleDetailTender.Remove(TendObj);
            //    context.SaveChanges();
            //    DelResult = true;
            //}

            return DelResult;
        }

        #endregion

        #region Add Schedule


        public bool CheckInspectionDatesCheck(long _ScheduleID, DateTime _InspectionDate)
        {
            bool Result = false;
            try
            {
                SI_Schedule ScheduleObj = (from Sch in context.SI_Schedule
                                           where Sch.ID == _ScheduleID
                                           select Sch).FirstOrDefault();


                if (ScheduleObj.FromDate > _InspectionDate || ScheduleObj.ToDate < _InspectionDate)
                {
                    Result = true;
                }


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);

            }
            return Result;
        }
        public int SaveSchedule(SI_Schedule _ObjSave, int UserID, int UserDesID)
        {
            var Value = -1;
            try
            {
                ContextDB dbADO = new ContextDB();
                Value = dbADO.ExecuteScalar("SI_InsertBasicScheduleInformation", _ObjSave.Name, _ObjSave.FromDate, _ObjSave.ToDate, _ObjSave.Description, UserID, UserDesID, _ObjSave.CreatedDate);

                //int? Results = context.SI_InsertBasicScheduleInformation(_ObjSave.Name, _ObjSave.FromDate, _ObjSave.ToDate, _ObjSave.Description, UserID, UserDesID, _ObjSave.CreatedDate).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Value;
        }

        public SI_Schedule GetScheduleBasicInformation(long _ScheduleID)
        {
            SI_Schedule objSchedule = new SI_Schedule();
            try
            {
                objSchedule = (from sce in context.SI_Schedule
                               where sce.ID == _ScheduleID
                               select sce).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objSchedule;
        }

        public bool CheckScheduleDetailDates(long _ScheduleID, DateTime _FromDate, DateTime _ToDate)
        {
            bool Result = false;
            try
            {
                List<SI_ScheduleDetailChannel> lstSceDetChnl = (from sceDetChnl in context.SI_ScheduleDetailChannel
                                                                where sceDetChnl.ScheduleID == _ScheduleID
                                                                select sceDetChnl).ToList();

                foreach (var v in lstSceDetChnl)
                {
                    if (v.ScheduleDate < _FromDate || v.ScheduleDate > _ToDate)
                        Result = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }


        #endregion

        #region Add Schedule Detail

        public string GetInspectionName(long _GaugeID)
        {
            string InspectionName = "";
            try
            {
                CO_ChannelGauge objGauge = (from gage in context.CO_ChannelGauge
                                            where gage.ID == _GaugeID
                                            select gage).FirstOrDefault();
                if (objGauge != null)
                {
                    if (objGauge.GaugeCategoryID != null)
                    {
                        if (objGauge.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge)
                            InspectionName = "Head";
                        else if (objGauge.GaugeCategoryID == (long)Constants.GaugeCategory.TailGauge)
                            InspectionName = "Tail";
                        else
                            InspectionName = PMIU.WRMIS.AppBlocks.Calculations.GetRDText(objGauge.GaugeAtRD);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return InspectionName;
        }


        public string GetUserNameAndDesignationByUserID(long _UserID)
        {
            string UserNameAndDesigantion = "";
            try
            {
                dynamic objGauge = (from User in context.UA_Users
                                    join desg in context.UA_Designations on User.DesignationID equals desg.ID
                                    where User.ID == _UserID
                                    select new { InspectedBy = User.FirstName + " " + User.LastName + " " + desg.Name }).FirstOrDefault();
                UserNameAndDesigantion = Utility.GetDynamicPropertyValue(objGauge, "InspectedBy");
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return UserNameAndDesigantion;
        }
        public List<object> GetScheduleDetailByScheduleIDInspectionTypeID(long _ScheduleID, long _InspectionTypeID)
        {
            List<object> lstGauge = new List<object>();
            try
            {
                lstGauge = (from sceDetail in context.SI_ScheduleDetailChannel
                            join Div in context.CO_Division on sceDetail.DivisionID equals Div.ID
                            join subDiv in context.CO_SubDivision on sceDetail.SubDivID equals subDiv.ID into A
                            from a in A.DefaultIfEmpty()
                            join chnl in context.CO_Channel on sceDetail.ChannelID equals chnl.ID
                            join gauge in context.CO_ChannelGauge on sceDetail.GaugeID equals gauge.ID into channelGauge
                            from cGauge in channelGauge.DefaultIfEmpty()
                            join GLevel in context.CO_GaugeLevel on cGauge.GaugeLevelID equals GLevel.ID into level
                            from gaugeLevel in level.DefaultIfEmpty()
                            join cOutlet in context.CO_ChannelOutlets on sceDetail.OutletID equals cOutlet.ID into outlet
                            from channelOutlet in outlet.DefaultIfEmpty()
                            join oc in context.SI_OutletChecking.Where(x => x.ScheduleDetailChannelID != null) on sceDetail.ID equals oc.ScheduleDetailChannelID into outletChecking
                            from channelOutletCheckin in outletChecking.DefaultIfEmpty()
                            where sceDetail.ScheduleID == _ScheduleID && sceDetail.InspectionTypeID == _InspectionTypeID
                            orderby sceDetail.ScheduleDate descending
                            select new
                            {
                                ScheduleDetailID = sceDetail.ID,
                                OutletCheckingID = (channelOutletCheckin == null ? 0 : channelOutletCheckin.ID),
                                DivisionName = Div.Name,
                                SubDivisionName = a.Name,
                                ChannelName = chnl.NAME,
                                GaugeID = sceDetail.GaugeID,
                                GaugeLevelID = gaugeLevel.ID == null ? long.MinValue : gaugeLevel.ID,
                                DateOfVisit = sceDetail.ScheduleDate,
                                InspectionTime = sceDetail.CreatedDate,
                                Remarks = sceDetail.Remarks,
                                DivisionID = sceDetail.DivisionID,
                                SubDivisionID = sceDetail.SubDivID,
                                ChannelID = sceDetail.ChannelID,
                                OutletID = sceDetail.OutletID,
                                CreatedBy = sceDetail.CreatedBy,
                                CreatedDate = sceDetail.CreatedDate
                            })
                                     .ToList()
                                     .Select(u => new
                                     {
                                         ScheduleDetailID = u.ScheduleDetailID,
                                         OutletCheckingID = u.OutletCheckingID,
                                         DivisionName = u.DivisionName,
                                         SubDivisionName = u.SubDivisionName,
                                         u.ChannelName,
                                         GaugeID = u.GaugeID.HasValue ? u.GaugeID.Value : -1,
                                         GaugeLevelID = u.GaugeLevelID,
                                         InspectionRD = u.GaugeID.HasValue ? GetInspectionName(u.GaugeID.Value) : string.Empty,
                                         SortingDate = u.DateOfVisit,
                                         DateOfVisit = Utility.GetFormattedDate(u.DateOfVisit),
                                         InspectionTime = Utility.GetFormattedTime(u.InspectionTime),
                                         u.Remarks,
                                         DivisionID = u.DivisionID,
                                         SubDivisionID = u.SubDivisionID,
                                         ChannelID = u.ChannelID,
                                         OutletID = u.OutletID.HasValue ? u.OutletID.Value : -1,
                                         OutletName = u.OutletID.HasValue ? GetOtletFormattedName(u.OutletID.Value) : string.Empty,
                                         CreatedBy = u.CreatedBy,
                                         CreatedDate = u.CreatedDate
                                     }).OrderBy(x => x.SortingDate).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstGauge;
        }


        public string GetOtletFormattedName(long _ID)
        {
            string OutletName = "";
            try
            {
                CO_ChannelOutlets objResult = (from outlet in context.CO_ChannelOutlets
                                               where outlet.ID == _ID
                                               select outlet).FirstOrDefault();

                if (objResult != null)
                    OutletName = Calculations.GetRDText(objResult.OutletRD) + "/" + objResult.ChannelSide;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return OutletName;
        }

        public List<object> GetOutletExistingRecords(long _ScheduleID, long _InspectionType)
        {
            List<object> lstGauge = new List<object>();
            try
            {

                lstGauge = (from sceDetail in context.SI_ScheduleDetailChannel
                            join Div in context.CO_Division on sceDetail.DivisionID equals Div.ID
                            join subDiv in context.CO_SubDivision on sceDetail.SubDivID equals subDiv.ID into A
                            from a in A.DefaultIfEmpty()
                            join chnl in context.CO_Channel on sceDetail.ChannelID equals chnl.ID
                            join outlet in context.CO_ChannelOutlets on sceDetail.OutletID equals outlet.ID
                            join outletAlt in context.SI_OutletAlterationHistroy on sceDetail.OutletID equals outletAlt.OutletID into B
                            from b in B.DefaultIfEmpty()
                            join outletAlt in context.CO_ChannelOutletsPerformance on sceDetail.OutletID equals outletAlt.OutletID into C
                            from c in C.DefaultIfEmpty()
                            where sceDetail.ScheduleID == _ScheduleID && sceDetail.InspectionTypeID == _InspectionType
                            select new
                            {
                                ID = sceDetail.ID,
                                ScheduleID = sceDetail.ScheduleID,
                                DivName = Div.Name,
                                SubDivName = a.Name,
                                ChannelName = chnl.NAME,
                                OutletID = outlet.ID,
                                OutletAlterationRecordAlreadyExists = b.OutletID == null ? false : true,
                                OutletPerformanceRecordAlreadyExists = c.OutletID == null ? false : true,
                                SceDate = sceDetail.ScheduleDate,
                                Remarks = sceDetail.Remarks
                            })
                                     .ToList()
                                     .Select(u => new
                                     {
                                         ScheduleDetailID = u.ID,
                                         ScheduleID = u.ScheduleID,
                                         DivisionName = u.DivName,
                                         SubDivisionName = u.SubDivName,
                                         ChannelName = u.ChannelName,
                                         OutletID = u.OutletID,
                                         OutletAlterationRecordAlreadyExists = u.OutletAlterationRecordAlreadyExists,
                                         OutletPerformanceRecordAlreadyExists = u.OutletPerformanceRecordAlreadyExists,
                                         OutletName = GetOtletFormattedName(u.OutletID),
                                         DateOfVisit = Utility.GetFormattedDate(u.SceDate),
                                         Remarks = u.Remarks
                                     }).Distinct().ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstGauge;
        }

        public SI_ScheduleDetailChannel GetSelectedRecordDetail(long _ID)
        {
            SI_ScheduleDetailChannel objGauge = new SI_ScheduleDetailChannel();
            try
            {
                objGauge = (from sceDetail in context.SI_ScheduleDetailChannel
                            where sceDetail.ID == _ID
                            select sceDetail).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objGauge;
        }
        public string GetInspectioAreaName(long _GaugeCatID, double _GaugeAtRD)
        {
            string InspectionName = "";
            try
            {
                if (_GaugeCatID == (long)Constants.GaugeCategory.HeadGauge)
                {
                    InspectionName = "Head";
                }
                else if (_GaugeCatID == (long)Constants.GaugeCategory.TailGauge)
                {
                    InspectionName = "Tail";
                }
                else
                {
                    InspectionName = PMIU.WRMIS.AppBlocks.Calculations.GetRDText(_GaugeAtRD);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return InspectionName;
        }

        public List<object> GetGaugeInspectionArea(long _ChannelID)
        {
            List<object> lstInspectionArea = new List<object>();

            try
            {
                lstInspectionArea = (from chnlGage in context.CO_ChannelGauge
                                     where chnlGage.ChannelID == _ChannelID
                                     select new
                                     {
                                         ID = chnlGage.ID,
                                         GaugeCategory = chnlGage.GaugeCategoryID,
                                         GaugeAtRD = chnlGage.GaugeAtRD
                                     }).OrderBy(x => x.GaugeAtRD)
                                     .ToList()
                                     .Select
                                     (u => new
                                     {
                                         u.ID,
                                         Name = GetInspectioAreaName(u.GaugeCategory, u.GaugeAtRD)
                                     }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstInspectionArea;
        }

        public string GetOutletName(int? _RD, string _Side)
        {
            string Name = "";
            try
            {
                if (_RD != null && _Side != "")
                    Name = _RD.ToString() + "/" + _Side;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Name;
        }

        public List<object> GetOutletsAgainstChannel(long _ChannelID)
        {
            List<object> lstOutlets = new List<object>();
            try
            {

                lstOutlets = (from outlet in context.CO_ChannelOutlets
                              where outlet.ChannelID == _ChannelID
                              select new
                              {
                                  ID = outlet.ID,
                                  OutletRD = outlet.OutletRD,
                                  ChannelSide = outlet.ChannelSide
                              }).OrderBy(x => x.OutletRD).ToList()
                                .Select(u => new
                                {
                                    u.ID,
                                    Name = u.OutletRD.HasValue ? Calculations.GetRDText(u.OutletRD.Value) + "/" + u.ChannelSide : ""
                                }).ToList<object>();
                return lstOutlets;

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstOutlets;
        }

        public bool IsGaugeInspectionNotesExist(long _ScheduleID)
        {
            return this.context.SI_ChannelGaugeReading.Any(g => g.ScheduleDetailChannelID == _ScheduleID);
        }

        public bool IsOutletAltrationInspNotesExist(long _ScheduleID)
        {
            bool InspNoteExist = false;
            try
            {
                SI_OutletAlterationHistroy objResult = (from outletAlt in context.SI_OutletAlterationHistroy
                                                        where outletAlt.ScheduleDetailChannelID == _ScheduleID
                                                        select outletAlt).FirstOrDefault();

                if (objResult != null)
                    InspNoteExist = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                InspNoteExist = false;
            }
            return InspNoteExist;
        }

        public bool IsOutletPerformanceInspNotesExist(long _ScheduleID)
        {
            bool InspNoteExist = false;
            try
            {
                CO_ChannelOutletsPerformance objResult = (from outletPer in context.CO_ChannelOutletsPerformance
                                                          where outletPer.ScheduleDetailChannelID == _ScheduleID
                                                          select outletPer).FirstOrDefault();
                if (objResult != null)
                    InspNoteExist = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                InspNoteExist = false;
            }
            return InspNoteExist;
        }

        public bool IsDTParameterInspectionNotesExist(long _ScheduleID)
        {
            bool InspNoteExist = false;
            try
            {
                long? gaugeID = (from sceDet in context.SI_ScheduleDetailChannel
                                 where sceDet.ID == _ScheduleID
                                 select sceDet.GaugeID).FirstOrDefault();

                if (gaugeID != null)
                {
                    long GaugeLevel = (from gageLvl in context.CO_ChannelGauge
                                       where gageLvl.ID == gaugeID
                                       select gageLvl.GaugeLevelID).FirstOrDefault();

                    if (GaugeLevel == 1)  // Bed level
                    {
                        CO_ChannelGaugeDTPGatedStructure objResult = (from struc in context.CO_ChannelGaugeDTPGatedStructure
                                                                      where struc.ScheduleDetailChannelID == _ScheduleID
                                                                      select struc).FirstOrDefault();
                        if (objResult != null)
                            InspNoteExist = true;
                    }
                    else  // Crest level 
                    {
                        CO_ChannelGaugeDTPFall objResult = (from fall in context.CO_ChannelGaugeDTPFall
                                                            where fall.ScheduleDetailChannelID == _ScheduleID
                                                            select fall).FirstOrDefault();
                        if (objResult != null)
                            InspNoteExist = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                InspNoteExist = false;
            }
            return InspNoteExist;
        }

        #endregion

        #region Action On Schedule

        /// <summary>
        /// This function fetches the Schedule data.
        /// Created By 15-07-2016
        /// </summary>
        /// <param name="_ScheduleID"></param>
        /// <returns>dynamic</returns>
        public dynamic GetScheduleData(long _ScheduleID)
        {
            dynamic scheduleData = (from sch in context.SI_Schedule
                                    join usr in context.UA_Users on sch.CreatedBy equals usr.ID
                                    join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID into B
                                    from b in B.DefaultIfEmpty()
                                    //join ssd in context.SI_ScheduleStatusDetail on sch.ID equals ssd.ScheduleID

                                    join ssd in
                                        (from ssd3 in context.SI_ScheduleStatusDetail
                                         where
                           (from ssd2 in context.SI_ScheduleStatusDetail
                            group ssd2 by ssd2.ScheduleID into cd
                            select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
                                         select ssd3) on sch.ID equals ssd.ScheduleID


                                    join ss in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals ss.ID
                                    orderby ssd.CreatedDate descending
                                    where sch.ID == _ScheduleID
                                    select new
                                    {
                                        ID = sch.ID,
                                        Name = sch.Name,
                                        FromDate = sch.FromDate,
                                        ToDate = sch.ToDate,
                                        Description = sch.Description,
                                        PreparedBy = usr.FirstName + " " + usr.LastName + ", " + b.Name,
                                        PreparedByID = sch.CreatedBy,
                                        CreatedDate = sch.CreatedDate,
                                        PreparedByDesignationID = b.ID,
                                        Status = ss.Name,
                                        StatusID = ssd.ScheduleStatusID,
                                        IrrigationLevelId = b.IrrigationLevelID
                                    }).FirstOrDefault();

            return scheduleData;
        }

        public dynamic GetScheduleDataForGeneralInspection(long _ScheduleDetailID)
        {
            dynamic scheduleData = (from sch in context.SI_Schedule
                                    join DT in context.SI_ScheduleDetailGeneral on sch.ID equals DT.ScheduleID
                                    join GI in context.SI_GeneralInspections on DT.ID equals GI.ScheduleDetailGeneralID into GI1
                                    from GI2 in GI1.DefaultIfEmpty()
                                    join usr in context.UA_Users on sch.CreatedBy equals usr.ID
                                    join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID into B
                                    from b in B.DefaultIfEmpty()
                                    //join ssd in context.SI_ScheduleStatusDetail on sch.ID equals ssd.ScheduleID

                                    join ssd in
                                        (from ssd3 in context.SI_ScheduleStatusDetail
                                         where
                           (from ssd2 in context.SI_ScheduleStatusDetail
                            group ssd2 by ssd2.ScheduleID into cd
                            select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
                                         select ssd3) on sch.ID equals ssd.ScheduleID


                                    join ss in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals ss.ID
                                    orderby ssd.CreatedDate descending
                                    where DT.ID == _ScheduleDetailID
                                    select new
                                    {
                                        ID = sch.ID,
                                        Name = sch.Name,
                                        FromDate = sch.FromDate,
                                        ToDate = sch.ToDate,
                                        Description = sch.Description,
                                        PreparedBy = usr.FirstName + " " + usr.LastName + ", " + b.Name,
                                        PreparedByID = sch.CreatedBy,
                                        CreatedDate = sch.CreatedDate,
                                        PreparedByDesignationID = b.ID,
                                        Status = ss.Name,
                                        StatusID = ssd.ScheduleStatusID,
                                        IrrigationLevelId = b.IrrigationLevelID,
                                        InspectionTypeID = DT.GeneralInspectionTypeID,
                                        InspectionDate = DT.ScheduleDate,
                                        Location = DT.Location,
                                        GeneralInspectionID = GI2 == null ? 0 : GI2.ID,
                                        GinspectionType = GI2 == null ? 0 : GI2.GeneralInspectionTypeID,
                                        GInspectionDate = GI2 == null ? DateTime.Now : GI2.InspectionDate,
                                        GLocation = GI2 == null ? "" : GI2.InspectionLocation,
                                        GDetails = GI2 == null ? "" : GI2.InspectionDetails,
                                        GRemarks = GI2 == null ? "" : GI2.Remarks
                                    }).FirstOrDefault();

            return scheduleData;
        }

        #endregion

        #region Outlet Inspection
        public double GetOutletDischarge(long _OutletID)
        {
            double designDischarge = (from d in context.CO_OutletAlterationHistroy
                                      where d.OutletID == _OutletID
                                      orderby d.ID descending
                                      select d.DesignDischarge).FirstOrDefault();
            return designDischarge;
        }
        public dynamic GetOutletInspection(long _ScheduleDetailChannelID, long? _InspectionTypeID)
        {
            dynamic qOutletInspection = context.SI_GetOutletInspection(_ScheduleDetailChannelID, _InspectionTypeID).Select(o => new
            {
                o.ScheduleTitle,
                o.ScheduleStatus,
                o.PreparedBy,
                FromDate = Utility.GetFormattedDate(o.FromDate),
                ToDate = Utility.GetFormattedDate(o.ToDate),
                o.ChannelName,
                o.OutletName,
                o.OutletTypeName,
                o.OutletID,
                o.OutletTypeID,
                o.ChannelID,
                o.InspectionTypeID,
                o.ScheduleDetailChannelID,
                o.ScheduleID,
                OutletRDChannelSide = Calculations.GetRDText(o.OutletRD) + "/" + o.ChannelSide
            }).FirstOrDefault();

            return qOutletInspection;
        }

        public SI_OutletAlterationHistroy GetOutletAlterationInspection(long _ScheduleDetailChannleID)
        {
            SI_OutletAlterationHistroy qOutletAlterationInspection = (from oai in context.SI_OutletAlterationHistroy
                                                                      join sdc in context.SI_ScheduleDetailChannel on oai.ScheduleDetailChannelID equals sdc.ID
                                                                      where sdc.ID == _ScheduleDetailChannleID
                                                                      select oai).FirstOrDefault();
            return qOutletAlterationInspection;
        }
        public CO_ChannelOutletsPerformance GetOutletsPerformanceInspection(long _ScheduleDetailChannleID)
        {
            CO_ChannelOutletsPerformance qOutletsPerformanceInspection = (from cop in context.CO_ChannelOutletsPerformance
                                                                          join sdc in context.SI_ScheduleDetailChannel on cop.ScheduleDetailChannelID equals sdc.ID
                                                                          where sdc.ID == _ScheduleDetailChannleID
                                                                          select cop).FirstOrDefault();
            return qOutletsPerformanceInspection;
        }

        public CO_ChannelOutletsPerformance GetOutletPerformanceByID(long _OutletPerformanceID)
        {
            CO_ChannelOutletsPerformance qOutletsPerformanceInspection = (from OutletInspection in context.CO_ChannelOutletsPerformance
                                                                          where OutletInspection.ID == _OutletPerformanceID
                                                                          select OutletInspection).FirstOrDefault();
            return qOutletsPerformanceInspection;
        }

        public SI_OutletAlterationHistroy GetOutletAlterationInspoctionData(long _OutletAlterationInspectionID)
        {
            SI_OutletAlterationHistroy qOutletsAlterationInspection = (from OutletInspection in context.SI_OutletAlterationHistroy
                                                                       where OutletInspection.ID == _OutletAlterationInspectionID
                                                                       select OutletInspection).FirstOrDefault();
            return qOutletsAlterationInspection;
        }
        #endregion

        #region Search Schedule


        public List<dynamic> GetSchedulesBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _StatusID, bool _ApprovedCheck, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _Subordinates, List<long> _lstDivs, List<long> _lstSubDivs)
        {
            _Subordinates.Add(_UserID);
            List<dynamic> scheduleData = new List<dynamic>();

            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (!string.IsNullOrEmpty(_FromDate))
                fromDate = Utility.GetParsedDate(_FromDate);
            if (!string.IsNullOrEmpty(_ToDate))
                toDate = Utility.GetParsedDate(_ToDate);

            //scheduleData = (from sch in context.SI_Schedule
            //                join usr in context.UA_Users on sch.CreatedBy equals usr.ID
            //                join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
            //                join ssd in
            //                    (from ssd3 in context.SI_ScheduleStatusDetail
            //                     where
            //       (from ssd2 in context.SI_ScheduleStatusDetail
            //        group ssd2 by ssd2.ScheduleID into cd
            //        select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
            //                     select ssd3) on sch.ID equals ssd.ScheduleID

            //                join ss in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals ss.ID
            //                join sdc in context.SI_ScheduleDetailChannel on sch.ID equals sdc.ScheduleID into sdct
            //                from sdc1 in sdct.DefaultIfEmpty()
            //                join sdcT in context.SI_ScheduleDetailTender on sch.ID equals sdcT.ScheduleID into sdctT
            //                from sdc1T in sdctT.DefaultIfEmpty()
            //                join sdcW in context.SI_ScheduleDetailWorks on sch.ID equals sdcW.ScheduleID into sdctW
            //                from sdc1W in sdctW.DefaultIfEmpty()
            //                orderby sch.CreatedDate descending

            //                where (_DivisionID == -1 || (sdc1.DivisionID == _DivisionID || sdc1T.DivisionID == _DivisionID || sdc1W.DivisionID == _DivisionID))
            //                 && (_SubDivisionID == -1 || sdc1.SubDivID == _SubDivisionID)
            //                 && (_StatusID == -1 || ssd.ScheduleStatusID == _StatusID)
            //                 && (((_ToDate != "" && _FromDate != "")
            //                       && (sch.FromDate <= toDate && fromDate <= sch.ToDate))
            //                    || ((_ToDate == "")
            //                     && (sch.FromDate >= (string.IsNullOrEmpty(_FromDate) ? sch.FromDate : fromDate)))
            //                    || ((_FromDate == "")
            //                     && (sch.ToDate <= (string.IsNullOrEmpty(_ToDate) ? sch.ToDate : toDate))))
            //                 && (//If user is ADM or XEN and Assigned to me for approval is not checked then all records of this user plus its subordinates are fetched else only those records which belongs to user are fetched
            //                      ((_DesignationID == (long)Constants.Designation.ADM || _DesignationID == (long)Constants.Designation.XEN)
            //                         && (_ApprovedCheck == false ? (_Subordinates.Contains(sch.CreatedBy.Value) && ssd.ScheduleStatusID != (long)Constants.SIScheduleStatus.Prepared) : ssd.SendToUserID == _UserID))
            //                 ||
            //                    //If user is SE or DD and Assigned to me for approval is not checked then all records of this user plus its subordinates are fetched else only those records which belongs to user are fetched
            //                     ((_DesignationID == (long)Constants.Designation.SE || _DesignationID == (long)Constants.Designation.DeputyDirector)
            //                     && (_ApprovedCheck == false ? (_Subordinates.Contains(sch.CreatedBy.Value) && ssd.ScheduleStatusID != (long)Constants.SIScheduleStatus.Prepared) : ssd.SendToUserID == _UserID))
            //                 ||
            //                    ////If user is SDO or MA then all Schedules entered by user are fetched through logged in userID
            //                 (sch.CreatedBy == _UserID))
            //                select new
            //                          {
            //                              ScheduleID = sch.ID,
            //                              Name = sch.Name,
            //                              FromDate = sch.FromDate,
            //                              ToDate = sch.ToDate,
            //                              StatusValue = ss.Name,
            //                              StatusID = ssd.ScheduleStatusID,
            //                              PrepairedBy = sch.CreatedBy,
            //                              PrepairedByDesignationID = usr.DesignationID,
            //                              CreatedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
            //                              CreatedDate = sch.CreatedDate,

            //                          }).Distinct().OrderByDescending(x => x.FromDate).ToList()
            //                           .Select(u => new
            //                           {
            //                               u.ScheduleID,
            //                               ScheduleStatus = GetScheduleStatus(u.ToDate, u.ScheduleID),
            //                               u.Name,
            //                               u.FromDate,
            //                               u.ToDate,
            //                               u.StatusValue,
            //                               u.StatusID,
            //                               u.PrepairedBy,
            //                               u.PrepairedByDesignationID,
            //                               u.CreatedBy,
            //                               u.CreatedDate,

            //                           }).Distinct().OrderByDescending(x => x.FromDate).ToList<dynamic>();

            scheduleData = (from sch in context.SI_Schedule
                            join usr in context.UA_Users on sch.CreatedBy equals usr.ID
                            join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                            join ssd in
                                (from ssd3 in context.SI_ScheduleStatusDetail
                                 where
                               (from ssd2 in context.SI_ScheduleStatusDetail
                                group ssd2 by ssd2.ScheduleID into cd
                                select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
                                 select ssd3)
                            on sch.ID equals ssd.ScheduleID

                            join ss in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals ss.ID
                            join sdc in context.SI_ScheduleDetailChannel on sch.ID equals sdc.ScheduleID into sdct
                            from sdc1 in sdct.DefaultIfEmpty()
                            join sdcT in context.SI_ScheduleDetailTender on sch.ID equals sdcT.ScheduleID into sdctT
                            from sdc1T in sdctT.DefaultIfEmpty()
                            join sdcW in context.SI_ScheduleDetailWorks on sch.ID equals sdcW.ScheduleID into sdctW
                            from sdc1W in sdctW.DefaultIfEmpty()
                            orderby sch.CreatedDate descending

                            where
                                //(_DivisionID == -1 || (sdc1.DivisionID == _DivisionID || sdc1T.DivisionID == _DivisionID || sdc1W.DivisionID == _DivisionID))
                                // && (_SubDivisionID == -1 || sdc1.SubDivID == _SubDivisionID)
                                (_lstDivs.FirstOrDefault() == -1 || (_lstDivs.Contains(sdc1.DivisionID) || _lstDivs.Contains(sdc1T.DivisionID) || _lstDivs.Contains(sdc1W.DivisionID)))
                                && (_lstSubDivs.FirstOrDefault() == -1 || _lstSubDivs.Contains((long)sdc1.SubDivID))
                             && (_StatusID == -1 || ssd.ScheduleStatusID == _StatusID)
                             && (((_ToDate != "" && _FromDate != "")
                                   && (sch.FromDate <= toDate && fromDate <= sch.ToDate))
                                || ((_ToDate == "")
                                 && (sch.FromDate >= (string.IsNullOrEmpty(_FromDate) ? sch.FromDate : fromDate)))
                                || ((_FromDate == "")
                                 && (sch.ToDate <= (string.IsNullOrEmpty(_ToDate) ? sch.ToDate : toDate))))
                            && (//If user is ADM or XEN and Assigned to me for approval is not checked then all records of this user plus its subordinates are fetched else only those records which belongs to user are fetched
                                 ((_DesignationID == (long)Constants.Designation.ADM || _DesignationID == (long)Constants.Designation.XEN)
                                    && (_ApprovedCheck == false ? (_Subordinates.Contains(sch.CreatedBy.Value) && ssd.ScheduleStatusID != (long)Constants.SIScheduleStatus.Prepared) : ssd.SendToUserID == _UserID))
                            ||
                                //If user is SE or DD and Assigned to me for approval is not checked then all records of this user plus its subordinates are fetched else only those records which belongs to user are fetched
                                ((_DesignationID == (long)Constants.Designation.SE || _DesignationID == (long)Constants.Designation.DeputyDirector)
                                && (_ApprovedCheck == false ? (_Subordinates.Contains(sch.CreatedBy.Value) && ssd.ScheduleStatusID != (long)Constants.SIScheduleStatus.Prepared) : ssd.SendToUserID == _UserID))
                            ||
                                //If user is  DG and Assigned to me for approval is not checked then all records of this user plus its subordinates are fetched else only those records which belongs to user are fetched
                                 ((_DesignationID == (long)Constants.Designation.DirectorGauges)
                                  && (_ApprovedCheck == false ? (_Subordinates.Contains(ssd.SendToUserID.Value) && ssd.ScheduleStatusID != (long)Constants.SIScheduleStatus.Prepared) : ssd.SendToUserID == _UserID))
                            ||
                                ////If user is SDO or MA then all Schedules entered by user are fetched through logged in userID
                            (sch.CreatedBy == _UserID))
                            select new
                            {
                                ScheduleID = sch.ID,
                                Name = sch.Name,
                                FromDate = sch.FromDate,
                                ToDate = sch.ToDate,
                                StatusValue = ss.Name,
                                StatusID = ssd.ScheduleStatusID,
                                PrepairedBy = sch.CreatedBy,
                                PrepairedByDesignationID = usr.DesignationID,
                                CreatedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                                CreatedDate = sch.CreatedDate,

                            }).Distinct().OrderByDescending(x => x.FromDate).ToList()
                                       .Select(u => new
                                       {
                                           u.ScheduleID,
                                           ScheduleStatus = GetScheduleStatus(u.ToDate, u.ScheduleID),
                                           u.Name,
                                           u.FromDate,
                                           u.ToDate,
                                           u.StatusValue,
                                           u.StatusID,
                                           u.PrepairedBy,
                                           u.PrepairedByDesignationID,
                                           u.CreatedBy,
                                           u.CreatedDate,
                                           HasDependants = IsScheduleDependencyExists(u.ScheduleID)
                                       }).Distinct().OrderByDescending(x => x.FromDate).ToList<dynamic>();

            if (_ApprovedCheck)
            {
                scheduleData = scheduleData.Where(x => x.StatusID == 2 && x.PrepairedBy != _UserID).ToList();
            }
            var CurrentDate = scheduleData.FindAll(x => x.FromDate >= DateTime.Now.Date).OrderBy(x => x.FromDate).ToList();
            var PrevoiusDates = scheduleData.FindAll(x => x.FromDate < DateTime.Now.Date).ToList();
            CurrentDate.AddRange(PrevoiusDates);


            ///////////////
            List<long?> lstsdc = (from sdc in context.SI_ScheduleDetailChannel select sdc.ScheduleID).ToList<long?>();
            List<long> lstsdg = (from sdc in context.SI_ScheduleDetailGeneral select sdc.ScheduleID).ToList<long>();
            List<long> lstsdt = (from sdc in context.SI_ScheduleDetailTender select sdc.ScheduleID).ToList<long>();
            List<long> lstsdw = (from sdc in context.SI_ScheduleDetailWorks select sdc.ScheduleID).ToList<long>();

            List<dynamic> swd = (from s in context.SI_Schedule
                                 //join sd in context.SI_ScheduleStatusDetail on s.ID equals sd.ScheduleID


                                 join ssd in
                                     (from ssd3 in context.SI_ScheduleStatusDetail
                                      where
                        (from ssd2 in context.SI_ScheduleStatusDetail
                         group ssd2 by ssd2.ScheduleID into cd
                         select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
                                      select ssd3) on s.ID equals ssd.ScheduleID




                                 join u in context.UA_Users on s.CreatedBy equals u.ID
                                 where !lstsdc.Contains(s.ID) &&
                                       !lstsdg.Contains(s.ID) &&
                                       !lstsdt.Contains(s.ID) &&
                                       !lstsdw.Contains(s.ID) &&
                                       s.CreatedBy == _UserID
                                 select new
                                 {
                                     ScheduleID = s.ID,
                                     //                                     ScheduleStatus = GetScheduleStatus(s.FromDate),
                                     s.Name,
                                     s.FromDate,
                                     s.ToDate,
                                     StatusValue = ssd.SI_ScheduleStatus.Name,
                                     StatusID = ssd.SI_ScheduleStatus.ID,
                                     PrepairedBy = s.CreatedBy,
                                     PrepairedByDesignationID = u.DesignationID,
                                     CreatedBy = u.FirstName + " " + u.LastName + ", " + u.UA_Designations.Name,
                                     u.CreatedDate
                                 })
                                 .ToList()
                                 .Select(k => new
                                 {
                                     k.ScheduleID,
                                     ScheduleStatus = GetScheduleStatus(k.ToDate, k.ScheduleID),
                                     k.Name,
                                     k.FromDate,
                                     k.ToDate,
                                     k.StatusValue,
                                     k.StatusID,
                                     k.PrepairedBy,
                                     k.PrepairedByDesignationID,
                                     k.CreatedBy,
                                     k.CreatedDate,
                                     HasDependants = IsScheduleDependencyExists(k.ScheduleID)
                                 })
                                 .ToList<dynamic>();



            CurrentDate = CurrentDate.Union(swd).ToList<dynamic>();


            ///////////////////////




            return CurrentDate;

        }

        public bool IsScheduleDependencyExists(long _ScheduleID)
        {
            bool isDependanceExists = context.SI_ScheduleDetailChannel.Any(i => i.ScheduleID == _ScheduleID);

            if (!isDependanceExists)
            {
                isDependanceExists = context.SI_ScheduleDetailTender.Any(i => i.ScheduleID == _ScheduleID);

                if (!isDependanceExists)
                {
                    isDependanceExists = context.SI_ScheduleDetailWorks.Any(i => i.ScheduleID == _ScheduleID);

                    if (!isDependanceExists)
                    {
                        isDependanceExists = context.SI_ScheduleDetailGeneral.Any(i => i.ScheduleID == _ScheduleID);
                    }
                }
            }

            return isDependanceExists;
        }

        public bool DeleteSchedule(long _ScheduleID)
        {
            List<SI_ScheduleDetailChannel> lstScheduleDetailChannel = context.SI_ScheduleDetailChannel.Where(sdc => sdc.ScheduleID == _ScheduleID).ToList();
            context.SI_ScheduleDetailChannel.RemoveRange(lstScheduleDetailChannel);

            List<SI_ScheduleDetailTender> lstScheduleDetailTender = context.SI_ScheduleDetailTender.Where(sdt => sdt.ScheduleID == _ScheduleID).ToList();
            context.SI_ScheduleDetailTender.RemoveRange(lstScheduleDetailTender);

            List<SI_ScheduleDetailWorks> lstScheduleDetailWorks = context.SI_ScheduleDetailWorks.Where(sdw => sdw.ScheduleID == _ScheduleID).ToList();
            context.SI_ScheduleDetailWorks.RemoveRange(lstScheduleDetailWorks);

            List<SI_ScheduleDetailGeneral> lstScheduleDetailGeneral = context.SI_ScheduleDetailGeneral.Where(sdg => sdg.ScheduleID == _ScheduleID).ToList();
            context.SI_ScheduleDetailGeneral.RemoveRange(lstScheduleDetailGeneral);

            List<SI_ScheduleStatusDetail> lstScheduleStatusDetail = context.SI_ScheduleStatusDetail.Where(ssd => ssd.ScheduleID == _ScheduleID).ToList();
            context.SI_ScheduleStatusDetail.RemoveRange(lstScheduleStatusDetail);

            SI_Schedule mdlSchedule = context.SI_Schedule.Where(s => s.ID == _ScheduleID).FirstOrDefault();
            context.SI_Schedule.Remove(mdlSchedule);

            context.SaveChanges();
            return true;
        }

        //public List<dynamic> GetScheduleDataByID(long ScheduleID)
        //{

        //    List<dynamic> scheduleData = (from sch in context.SI_Schedule
        //                    join usr in context.UA_Users on sch.CreatedBy equals usr.ID
        //                    join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
        //                    join ssd in context.SI_ScheduleStatusDetail on sch.ID equals ssd.ScheduleID into ssdt
        //                    from ssd1 in ssdt.DefaultIfEmpty()
        //                    join ss in context.SI_ScheduleStatus on ssd1.ScheduleStatusID equals ss.ID into sst
        //                    from ss1 in sst.DefaultIfEmpty()
        //                    join sdc in context.SI_ScheduleDetailChannel on sch.ID equals sdc.ScheduleID into sdct
        //                    from sdc1 in sdct.DefaultIfEmpty()
        //                    orderby sch.CreatedDate descending

        //                    where 
        //                    (sch.ID == ScheduleID)

        //                    select new
        //                    {
        //                        ScheduleID = sch.ID,
        //                        Name = sch.Name,
        //                        FromDate = sch.FromDate,
        //                        ToDate = sch.ToDate,
        //                        StatusValue = ss1.Name,
        //                        StatusID = ssd1.ScheduleStatusID,
        //                        PrepairedBy = sch.CreatedBy,
        //                        CreatedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
        //                        CreatedDate = sch.CreatedDate,

        //                    }).Distinct().ToList<dynamic>();

        //    return scheduleData;
        //}

        #endregion

        #region Inspection Notes

        public dynamic GetScheduleDetailData(long _ScheduleDetailID)
        {
            dynamic scheduleDetailData = (from schDetCh in context.SI_ScheduleDetailChannel
                                          join GaugeReading in context.SI_ChannelGaugeReading on schDetCh.ID equals GaugeReading.ScheduleDetailChannelID into Gaugereading1
                                          from GaugeReading2 in Gaugereading1.DefaultIfEmpty()
                                          join usr in context.UA_Users on schDetCh.CreatedBy equals usr.ID
                                          join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                          join sch in context.SI_Schedule on schDetCh.ScheduleID equals sch.ID
                                          join ssd in context.SI_ScheduleStatusDetail on sch.ID equals ssd.ScheduleID
                                          join SchChnl in context.CO_Channel on schDetCh.ChannelID equals SchChnl.ID
                                          join ss in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals ss.ID
                                          orderby ssd.CreatedDate descending
                                          where schDetCh.ID == _ScheduleDetailID
                                          select new
                                          {
                                              ID = schDetCh.ID,
                                              GaugeReadingID = GaugeReading2.ID == null ? 0 : GaugeReading2.ID,
                                              DivisionID = schDetCh.DivisionID,
                                              ScheduleID = schDetCh.ScheduleID,
                                              Name = sch.Name,
                                              FromDate = sch.FromDate,
                                              ToDate = sch.ToDate,
                                              Description = sch.Description,
                                              PreparedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                                              PreparedByID = sch.CreatedBy,
                                              PreparedByDesignationID = dsg.ID,
                                              Status = ss.Name,
                                              StatusID = ssd.ID,
                                              ChannelName = SchChnl.NAME,
                                              GaugeID = schDetCh.GaugeID,
                                              InspectedBy = GaugeReading2.GaugeReaderID == null ? 0 : GaugeReading2.GaugeReaderID,
                                              ChannelID = SchChnl.ID

                                          }).ToList()
                                       .Select(u => new
                                       {
                                           u.ID,
                                           u.GaugeReadingID,
                                           u.DivisionID,
                                           u.ScheduleID,
                                           u.Name,
                                           u.FromDate,
                                           u.ToDate,
                                           u.Description,
                                           PreparedBy = GetUserNameAndDesignationByUserID(u.PreparedByID.Value),
                                           u.PreparedByID,
                                           u.PreparedByDesignationID,
                                           u.Status,
                                           u.StatusID,
                                           u.ChannelName,
                                           u.GaugeID,
                                           InspectionArea = GetInspectionName(u.GaugeID.Value),
                                           InspectedBy = u.InspectedBy == 0 ? "" : GetUserNameAndDesignationByUserID(u.InspectedBy),
                                           u.ChannelID

                                       }).ToList<object>();

            return scheduleDetailData;
        }

        public dynamic GetScheduleDetailDataAlterationBL(long _ScheduleDetailID)
        {
            dynamic scheduleDetailDataBL = (from schDetCh in context.SI_ScheduleDetailChannel
                                            join GaugeReading in context.CO_ChannelGaugeDTPGatedStructure on schDetCh.ID equals GaugeReading.ScheduleDetailChannelID into Gaugereading1
                                            from GaugeReading2 in Gaugereading1.DefaultIfEmpty()
                                            join usr in context.UA_Users on schDetCh.CreatedBy equals usr.ID
                                            join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                            join sch in context.SI_Schedule on schDetCh.ScheduleID equals sch.ID
                                            join ssd in context.SI_ScheduleStatusDetail on sch.ID equals ssd.ScheduleID
                                            join SchChnl in context.CO_Channel on schDetCh.ChannelID equals SchChnl.ID
                                            join ss in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals ss.ID
                                            orderby ssd.CreatedDate descending
                                            where schDetCh.ID == _ScheduleDetailID
                                            select new
                                            {
                                                ID = schDetCh.ID,
                                                GaugeReadingID = GaugeReading2.ID == null ? 0 : GaugeReading2.ID,
                                                DivisionID = schDetCh.DivisionID,
                                                ScheduleID = schDetCh.ScheduleID,
                                                Name = sch.Name,
                                                FromDate = sch.FromDate,
                                                ToDate = sch.ToDate,
                                                Description = sch.Description,
                                                PreparedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                                                PreparedByID = sch.CreatedBy,
                                                PreparedByDesignationID = dsg.ID,
                                                Status = ss.Name,
                                                StatusID = ssd.ID,
                                                ChannelName = SchChnl.NAME,
                                                GaugeID = schDetCh.GaugeID,
                                                InspectedBy = GaugeReading2.CreatedBy == null ? 0 : GaugeReading2.CreatedBy,
                                                InspectedByID = GaugeReading2.CreatedBy == null ? 0 : GaugeReading2.CreatedBy,
                                                CreatedDate = GaugeReading2.CreatedDate,
                                                Source = GaugeReading2.Source



                                            }).ToList()
                                       .Select(u => new
                                       {
                                           u.ID,
                                           u.GaugeReadingID,
                                           u.DivisionID,
                                           u.ScheduleID,
                                           u.Name,
                                           u.FromDate,
                                           u.ToDate,
                                           u.Description,
                                           PreparedBy = GetUserNameAndDesignationByUserID(u.PreparedByID.Value),
                                           u.PreparedByID,
                                           u.PreparedByDesignationID,
                                           u.Status,
                                           u.StatusID,
                                           u.ChannelName,
                                           u.GaugeID,
                                           InspectionArea = GetInspectionName(u.GaugeID.Value),
                                           InspectedBy = u.InspectedBy == 0 ? "" : GetUserNameAndDesignationByUserID(u.InspectedBy.Value),
                                           u.InspectedByID,
                                           u.CreatedDate,
                                           u.Source


                                       }).ToList<object>();

            return scheduleDetailDataBL;
        }

        public dynamic GetScheduleDetailDataAlterationCL(long _ScheduleDetailID)
        {
            dynamic scheduleDetailDataCL = (from schDetCh in context.SI_ScheduleDetailChannel
                                            join GaugeReading in context.CO_ChannelGaugeDTPFall on schDetCh.ID equals GaugeReading.ScheduleDetailChannelID into Gaugereading1
                                            from GaugeReading2 in Gaugereading1.DefaultIfEmpty()
                                            join usr in context.UA_Users on schDetCh.CreatedBy equals usr.ID
                                            join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                            join sch in context.SI_Schedule on schDetCh.ScheduleID equals sch.ID
                                            join ssd in context.SI_ScheduleStatusDetail on sch.ID equals ssd.ScheduleID
                                            join SchChnl in context.CO_Channel on schDetCh.ChannelID equals SchChnl.ID
                                            join ss in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals ss.ID
                                            orderby ssd.CreatedDate descending
                                            where schDetCh.ID == _ScheduleDetailID
                                            select new
                                            {
                                                ID = schDetCh.ID,
                                                GaugeReadingID = GaugeReading2.ID == null ? 0 : GaugeReading2.ID,
                                                DivisionID = schDetCh.DivisionID,
                                                ScheduleID = schDetCh.ScheduleID,
                                                Name = sch.Name,
                                                FromDate = sch.FromDate,
                                                ToDate = sch.ToDate,
                                                Description = sch.Description,
                                                PreparedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                                                PreparedByID = sch.CreatedBy,
                                                PreparedByDesignationID = dsg.ID,
                                                Status = ss.Name,
                                                StatusID = ssd.ID,
                                                ChannelName = SchChnl.NAME,
                                                GaugeID = schDetCh.GaugeID,
                                                InspectedBy = GaugeReading2.CreatedBy == null ? 0 : GaugeReading2.CreatedBy,
                                                InspectedByID = GaugeReading2.CreatedBy == null ? 0 : GaugeReading2.CreatedBy,
                                                CreatedDate = GaugeReading2.CreatedDate,
                                                Source = GaugeReading2.Source

                                            }).ToList()
                                       .Select(u => new
                                       {
                                           u.ID,
                                           u.GaugeReadingID,
                                           u.DivisionID,
                                           u.ScheduleID,
                                           u.Name,
                                           u.FromDate,
                                           u.ToDate,
                                           u.Description,
                                           PreparedBy = GetUserNameAndDesignationByUserID(u.PreparedByID.Value),
                                           u.PreparedByID,
                                           u.PreparedByDesignationID,
                                           u.Status,
                                           u.StatusID,
                                           u.ChannelName,
                                           u.GaugeID,
                                           InspectionArea = GetInspectionName(u.GaugeID.Value),
                                           InspectedBy = u.InspectedBy == 0 ? "" : GetUserNameAndDesignationByUserID(u.InspectedBy.Value),
                                           u.InspectedByID,
                                           u.CreatedDate,
                                           u.Source

                                       }).ToList<object>();

            return scheduleDetailDataCL;
        }

        public dynamic GetGaugeLocation(long _GaugeID)
        {
            dynamic GaugeLocation = (from Gauge in context.CO_ChannelGauge
                                     join GaugeCategory in context.CO_GaugeCategory on Gauge.GaugeCategoryID equals GaugeCategory.ID
                                     where Gauge.ID == _GaugeID && (GaugeCategory.ID == 1 || GaugeCategory.ID == 2)
                                     select new
                                     {
                                         GaugeLocationID = GaugeCategory.ID

                                     }).FirstOrDefault();

            return GaugeLocation;
        }

        public List<dynamic> GetComplaintsForGaugeInspection(long _ModuleID, long _RefCodeID)
        {
            List<dynamic> Complaints = (from Comp in context.CM_Complaint
                                        join CompType in context.CM_ComplaintType on Comp.ComplaintTypeID equals CompType.ID
                                        join comRef in context.CM_ModuleRef on Comp.RefCode.ToLower() equals comRef.RefCode.ToLower()
                                        where comRef.ModuleID == _ModuleID && Comp.RefCodeID == _RefCodeID
                                        select new
                                        {
                                            ComplaintID = Comp.ID,
                                            ComplaintNumber = Comp.ComplaintNumber,
                                            ComplaintType = CompType.Name

                                        }).ToList<dynamic>();

            return Complaints;
        }
        public dynamic GetTailStatus(long _GaugeReadingID)
        {
            dynamic TailStatusData = (from ChGaugeReading in context.SI_ChannelGaugeReading
                                      join Gauge in context.CO_ChannelGauge on ChGaugeReading.GaugeID equals Gauge.ID
                                      join channel in context.CO_Channel on Gauge.ChannelID equals channel.ID
                                      where ChGaugeReading.ID == _GaugeReadingID
                                      select new
                                      {
                                          GaugeValue = ChGaugeReading.GaugeValue,
                                          AuthorizedGaugeValue = channel.AuthorizedTailGauge,


                                      }).FirstOrDefault();

            return TailStatusData;
        }

        #endregion


        #region Inspection Search

        public dynamic GetUncsheduledChannelAreaByGaugereadingID(long _GaugeReadingID)
        {
            try
            {
                dynamic GaugeData = (from Guagereading in context.SI_ChannelGaugeReading
                                     join Gauge in context.CO_ChannelGauge on Guagereading.GaugeID equals Gauge.ID
                                     join usr in context.UA_Users on Guagereading.GaugeReaderID equals usr.ID     //join usr in context.UA_Users on Guagereading.CreatedBy equals usr.ID
                                     join desg in context.UA_Designations on Guagereading.DesignationID equals desg.ID   //join desg in context.UA_Designations on usr.DesignationID equals desg.ID
                                     join channel in context.CO_Channel on Gauge.ChannelID equals channel.ID
                                     where Guagereading.ID == _GaugeReadingID
                                     select new
                                     {
                                         CreatedBy = usr.FirstName + " " + usr.LastName + ", " + desg.Name,
                                         ChannelName = channel.NAME,
                                         GaugeID = Gauge.ID

                                     }).ToList()
                                     .Select(u => new
                                     {
                                         CreatedBy = u.CreatedBy,
                                         ChannelName = u.ChannelName,
                                         InspectionRD = GetInspectionName(u.GaugeID)

                                     }).FirstOrDefault();

                return GaugeData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetUncsheduledChannelAreaByDischargeTableBLID(long _DischargeTableID)
        {
            try
            {
                dynamic DischargeBedLvlData = (from Discharge in context.CO_ChannelGaugeDTPGatedStructure
                                               join usr in context.UA_Users on Discharge.CreatedBy equals usr.ID
                                               join desg in context.UA_Designations on usr.DesignationID equals desg.ID
                                               join Gauge in context.CO_ChannelGauge on Discharge.GaugeID equals Gauge.ID
                                               join channel in context.CO_Channel on Gauge.ChannelID equals channel.ID
                                               where Discharge.ID == _DischargeTableID
                                               select new
                                               {
                                                   CreatedBy = usr.FirstName + " " + usr.LastName + ", " + desg.Name,
                                                   ChannelName = channel.NAME,
                                                   GaugeID = Gauge.ID,
                                                   Discharge.CreatedDate

                                               }).ToList()
                                     .Select(u => new
                                     {
                                         CreatedBy = u.CreatedBy,
                                         ChannelName = u.ChannelName,
                                         InspectionRD = GetInspectionName(u.GaugeID),
                                         CreatedDate = u.CreatedDate

                                     }).FirstOrDefault();

                return DischargeBedLvlData;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public dynamic GetUncsheduledChannelAreaByOutletPerformanceID(long _OutletPerformanceID)
        {
            try
            {
                dynamic PerformanceData = (from OutletP in context.CO_ChannelOutletsPerformance
                                           join usr in context.UA_Users on OutletP.CreatedBy equals usr.ID
                                           join desg in context.UA_Designations on usr.DesignationID equals desg.ID
                                           join Outlet in context.CO_ChannelOutlets on OutletP.OutletID equals Outlet.ID
                                           join channel in context.CO_Channel on Outlet.ChannelID equals channel.ID
                                           join History in context.CO_OutletAlterationHistroy on Outlet.ID equals History.OutletID into A
                                           from a in A.DefaultIfEmpty()
                                           join OutletType in context.CO_OutletType on a.OutletTypeID equals OutletType.ID into B
                                           from b in B.DefaultIfEmpty()
                                           where OutletP.ID == _OutletPerformanceID
                                           select new
                                           {
                                               CreatedBy = usr.FirstName + " " + usr.LastName + ", " + desg.Name,
                                               ChannelName = channel.NAME,
                                               OutletID = Outlet.ID,
                                               OutletRD = Outlet.OutletRD,
                                               OutletSide = Outlet.ChannelSide,
                                               OutletType = b.Name,
                                               Outletname = Outlet.Name

                                           }).ToList()
                                     .Select(u => new
                                     {
                                         CreatedBy = u.CreatedBy,
                                         ChannelName = u.ChannelName,
                                         //OutletName = GetOutletName(u.OutletRD,u.OutletSide),
                                         OutletName = u.Outletname,
                                         OutletType = u.OutletType


                                     }).FirstOrDefault();

                return PerformanceData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public dynamic GetUncsheduledChannelAreaByOutletAlterationID(long _OutletAlterationID)
        {
            try
            {
                dynamic AlterationData = (from OutletA in context.SI_OutletAlterationHistroy
                                          join usr in context.UA_Users on OutletA.CreatedBy equals usr.ID
                                          join desg in context.UA_Designations on usr.DesignationID equals desg.ID
                                          join Outlet in context.CO_ChannelOutlets on OutletA.OutletID equals Outlet.ID
                                          join channel in context.CO_Channel on Outlet.ChannelID equals channel.ID
                                          join History in context.CO_OutletAlterationHistroy on Outlet.ID equals History.OutletID into A
                                          from a in A.DefaultIfEmpty()
                                          join OutletType in context.CO_OutletType on a.OutletTypeID equals OutletType.ID into B
                                          from b in B.DefaultIfEmpty()
                                          where OutletA.ID == _OutletAlterationID
                                          select new
                                          {
                                              CreatedBy = usr.FirstName + " " + usr.LastName + ", " + desg.Name,
                                              ChannelName = channel.NAME,
                                              OutletID = Outlet.ID,
                                              OutletRD = Outlet.OutletRD,
                                              OutletSide = Outlet.ChannelSide,
                                              OutletType = b.Name,
                                              OutletName = Outlet.Name

                                          }).ToList()
                                     .Select(u => new
                                     {
                                         CreatedBy = u.CreatedBy,
                                         ChannelName = u.ChannelName,
                                         //OutletName = GetOutletName(u.OutletRD, u.OutletSide),
                                         OutletName = u.OutletName,
                                         OutletType = u.OutletType


                                     }).FirstOrDefault();

                return AlterationData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public dynamic GetUncsheduledChannelAreaByDischargeTableCLID(long _DischargeTableID)
        {
            try
            {
                dynamic DischargeTableCLData = (from Discharge in context.CO_ChannelGaugeDTPFall
                                                join usr in context.UA_Users on Discharge.CreatedBy equals usr.ID
                                                join desg in context.UA_Designations on usr.DesignationID equals desg.ID
                                                join Gauge in context.CO_ChannelGauge on Discharge.GaugeID equals Gauge.ID
                                                join channel in context.CO_Channel on Gauge.ChannelID equals channel.ID
                                                where Discharge.ID == _DischargeTableID
                                                select new
                                                {
                                                    CreatedBy = usr.FirstName + " " + usr.LastName + ", " + desg.Name,
                                                    ChannelName = channel.NAME,
                                                    GaugeID = Gauge.ID,
                                                    CreatedDate = Discharge.CreatedDate

                                                }).ToList()
                                     .Select(u => new
                                     {
                                         CreatedBy = u.CreatedBy,
                                         ChannelName = u.ChannelName,
                                         InspectionRD = GetInspectionName(u.GaugeID),
                                         CreatedDate = u.CreatedDate

                                     }).FirstOrDefault();

                return DischargeTableCLData;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<dynamic> GetGaugeInspectionBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                List<dynamic> GaugeInspections = new List<dynamic>();

                // new work                 
                List<long> UserSubDiv = new List<long>();
                List<long> UserDiv = new List<long>();
                List<long> UserCircles = new List<long>();
                List<long> UserZones = new List<long>();

                if (_lstUserZone != null)
                    UserZones.AddRange(_lstUserZone);

                if (_lstUserCircle != null)
                    UserCircles.AddRange(_lstUserCircle);

                if (_lstUserDiv != null)
                    UserDiv.AddRange(_lstUserDiv);

                if (_lstUserSubDiv != null)
                    UserSubDiv.AddRange(_lstUserSubDiv);

                if (_ZoneID != -1)
                {
                    UserZones.Clear();
                    UserZones.Add(_ZoneID);
                }
                else
                {
                    if (_lstUserZone == null || _lstUserZone.Count() == 0)
                    {
                        UserZones.Clear();
                        UserZones.Add(-1);
                    }
                }

                if (_CircleID != -1)
                {
                    UserCircles.Clear();
                    UserCircles.Add(_CircleID);
                }
                else
                {
                    if (_lstUserCircle == null || _lstUserCircle.Count() == 0)
                    {
                        UserCircles.Clear();
                        UserCircles.Add(-1);
                    }
                }

                if (_DivisionID != -1)
                {
                    UserDiv.Clear();
                    UserDiv.Add(_DivisionID); // get selected division
                }
                else
                {
                    if (_lstUserDiv == null || _lstUserDiv.Count() == 0)
                    {
                        UserDiv.Clear();
                        UserDiv.Add(-1); // get all divisions
                    }
                    //else  retain user divisions                        
                }

                if (_SubDivisionID != -1)
                {
                    UserSubDiv.Clear();
                    UserSubDiv.Add(_DivisionID);
                }
                else
                {
                    if (_lstUserSubDiv == null || _lstUserSubDiv.Count() == 0)
                    {
                        UserSubDiv.Clear();
                        UserSubDiv.Add(-1);
                    }
                }
                //

                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);

                //GaugeInspections = (from channelGaugeReading in context.SI_ChannelGaugeReading
                //                    join usr in context.UA_Users on channelGaugeReading.GaugeReaderID equals usr.ID
                //                    join Complaints in context.CM_Complaint on channelGaugeReading.ID equals Complaints.RefCodeID into sdct
                //                    from Complaints1 in sdct.DefaultIfEmpty()
                //                    join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                //                    join Gauge in context.CO_ChannelGauge on channelGaugeReading.GaugeID equals Gauge.ID
                //                    join Channel in context.CO_Channel on Gauge.ChannelID equals Channel.ID
                //                    join Section in context.CO_Section on Gauge.SectionID equals Section.ID
                //                    join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                //                    join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                //                    join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                //                    join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                //                    orderby channelGaugeReading.ReadingDateTime descending

                //                    where (_ZoneID == -1 || Zone.ID == _ZoneID)
                //                          && (_DivisionID == -1 || Division.ID == _DivisionID)
                //                          && (_CircleID == -1 || Circle.ID == _CircleID)
                //                          && (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                //                          && (DbFunctions.TruncateTime(channelGaugeReading.ReadingDateTime) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(channelGaugeReading.ReadingDateTime) : DbFunctions.TruncateTime(fromDate)))
                //                          && (DbFunctions.TruncateTime(channelGaugeReading.ReadingDateTime) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(channelGaugeReading.ReadingDateTime) : DbFunctions.TruncateTime(toDate)))
                //                          && ((_DesignationID == (long)Constants.Designation.XEN ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.ADM ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SDO ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SE ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                //                          && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? channelGaugeReading.ScheduleDetailChannelID != null : channelGaugeReading.ScheduleDetailChannelID == null))

                //                    select new
                //                    {
                //                        SID = channelGaugeReading.ScheduleDetailChannelID == null ? channelGaugeReading.ID : channelGaugeReading.ScheduleDetailChannelID,
                //                        GaugeID = Gauge.ID,
                //                        InspectionDatetime = channelGaugeReading.ReadingDateTime,
                //                        DivisionName = Division.Name,
                //                        SortOrder = channelGaugeReading.ID,
                //                        ChannelName = Channel.NAME,
                //                        GaugeValue = channelGaugeReading.GaugeValue,
                //                        Discharge = channelGaugeReading.DailyDischarge,
                //                        InspectionType = channelGaugeReading.ScheduleDetailChannelID == null ? "No" : "Yes",
                //                        IsScheduled = channelGaugeReading.ScheduleDetailChannelID == null ? false : true,
                //                        InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                //                        ComplaintGenerated = Complaints1.ID == null ? "No" : "Yes"

                //                    }).ToList()
                //                         .Select(u => new
                //                         {
                //                             SID = u.SID,
                //                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime),
                //                             DivisionName = u.DivisionName,
                //                             ChannelName = u.ChannelName,
                //                             SortOrder = u.SortOrder,
                //                             GaugeValue = u.GaugeValue,
                //                             Discharge = u.Discharge,
                //                             InspectionType = u.InspectionType,
                //                             IsScheduled = u.IsScheduled,
                //                             InspectedBy = u.InspectedBy,
                //                             ComplaintGenerated = u.ComplaintGenerated,
                //                             InspectionArea = GetInspectionName(u.GaugeID)
                //                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                GaugeInspections = (from channelGaugeReading in context.SI_ChannelGaugeReading
                                    join usr in context.UA_Users on channelGaugeReading.GaugeReaderID equals usr.ID
                                    join Complaints in context.CM_Complaint on channelGaugeReading.ID equals Complaints.RefCodeID into sdct
                                    from Complaints1 in sdct.DefaultIfEmpty()
                                    join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                    join Gauge in context.CO_ChannelGauge on channelGaugeReading.GaugeID equals Gauge.ID
                                    join Channel in context.CO_Channel on Gauge.ChannelID equals Channel.ID
                                    join Section in context.CO_Section on Gauge.SectionID equals Section.ID
                                    join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                                    join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                                    join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                    join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                                    orderby channelGaugeReading.ReadingDateTime descending

                                    where
                                        //   (_ZoneID == -1 || Zone.ID == _ZoneID)
                                        //&& (_DivisionID == -1 || Division.ID == _DivisionID)
                                        //&& (_CircleID == -1 || Circle.ID == _CircleID)
                                        //&& (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                                        //(_ZoneID == -1 || Zone.ID == _ZoneID)
                                          (UserZones.FirstOrDefault() == -1 || UserZones.Contains(Zone.ID))// == _ZoneID)
                                          && (UserCircles.FirstOrDefault() == -1 || UserCircles.Contains(Circle.ID)) //== _CircleID)
                                          && (UserDiv.FirstOrDefault() == -1 || UserDiv.Contains(Division.ID))// == _DivisionID)
                                          && (UserSubDiv.FirstOrDefault() == -1 || UserSubDiv.Contains(SubDivision.ID)) //  == _SubDivisionID)
                                          && (DbFunctions.TruncateTime(channelGaugeReading.ReadingDateTime) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(channelGaugeReading.ReadingDateTime) : DbFunctions.TruncateTime(fromDate)))
                                          && (DbFunctions.TruncateTime(channelGaugeReading.ReadingDateTime) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(channelGaugeReading.ReadingDateTime) : DbFunctions.TruncateTime(toDate)))
                                          && ((_DesignationID == (long)Constants.Designation.XEN ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.ADM ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SDO ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SE ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                          && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? channelGaugeReading.ScheduleDetailChannelID != null : channelGaugeReading.ScheduleDetailChannelID == null))

                                    select new
                                    {
                                        SID = channelGaugeReading.ScheduleDetailChannelID == null ? channelGaugeReading.ID : channelGaugeReading.ScheduleDetailChannelID,
                                        GaugeID = Gauge.ID,
                                        InspectionDatetime = channelGaugeReading.ReadingDateTime,
                                        DivisionName = Division.Name,
                                        SortOrder = channelGaugeReading.ID,
                                        ChannelName = Channel.NAME,
                                        GaugeValue = channelGaugeReading.GaugeValue,
                                        Discharge = channelGaugeReading.DailyDischarge,
                                        InspectionType = channelGaugeReading.ScheduleDetailChannelID == null ? "No" : "Yes",
                                        IsScheduled = channelGaugeReading.ScheduleDetailChannelID == null ? false : true,
                                        InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                                        ComplaintGenerated = Complaints1.ID == null ? "No" : "Yes"

                                    }).ToList()
                         .Select(u => new
                         {
                             SID = u.SID,
                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime),
                             DivisionName = u.DivisionName,
                             ChannelName = u.ChannelName,
                             SortOrder = u.SortOrder,
                             GaugeValue = u.GaugeValue,
                             Discharge = u.Discharge,
                             InspectionType = u.InspectionType,
                             IsScheduled = u.IsScheduled,
                             InspectedBy = u.InspectedBy,
                             ComplaintGenerated = u.ComplaintGenerated,
                             InspectionArea = GetInspectionName(u.GaugeID)
                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                return GaugeInspections;
            }
            catch (Exception)
            {

                throw;
            }



        }

        public List<dynamic> GetDischargeTableBedLevelBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                List<dynamic> DischargeDataBedtLvl = new List<dynamic>();

                // new work 
                List<long> UserSubDiv = new List<long>();
                List<long> UserDiv = new List<long>();
                List<long> UserCircles = new List<long>();
                List<long> UserZone = new List<long>();

                if (_lstUserZone != null)
                    UserZone.AddRange(_lstUserZone);

                if (_lstUserCircle != null)
                    UserCircles.AddRange(_lstUserCircle);

                if (_lstUserDiv != null)
                    UserDiv.AddRange(_lstUserDiv);

                if (_lstUserSubDiv != null)
                    UserSubDiv.AddRange(_lstUserSubDiv);


                if (_ZoneID != -1)
                {
                    UserZone.Clear();
                    UserZone.Add(_ZoneID);
                }
                else
                {
                    if (_lstUserZone == null || _lstUserZone.Count() == 0)
                    {
                        UserZone.Clear();
                        UserZone.Add(-1);
                    }
                }


                if (_CircleID != -1)
                {
                    UserCircles.Clear();
                    UserCircles.Add(_CircleID);
                }
                else
                {
                    if (_lstUserCircle == null || _lstUserCircle.Count() == 0)
                    {
                        UserCircles.Clear();
                        UserCircles.Add(-1);
                    }
                }

                if (_DivisionID != -1)
                {
                    UserDiv.Clear();
                    UserDiv.Add(_DivisionID); // get selected division
                }
                else
                {
                    if (_lstUserDiv == null || _lstUserDiv.Count() == 0)
                    {
                        UserDiv.Clear();
                        UserDiv.Add(-1); // get all divisions
                    }
                    //else  retain user divisions                        
                }

                if (_SubDivisionID != -1)
                {
                    UserSubDiv.Clear();
                    UserSubDiv.Add(_DivisionID);
                }
                else
                {
                    if (_lstUserSubDiv == null || _lstUserSubDiv.Count() == 0)
                    {
                        UserSubDiv.Clear();
                        UserSubDiv.Add(-1);
                    }
                }
                //

                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);

                //DischargeDataBedtLvl = (from DischargeBedLevel in context.CO_ChannelGaugeDTPGatedStructure
                //                        join usr in context.UA_Users on DischargeBedLevel.CreatedBy equals usr.ID
                //                        join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                //                        join Gauge in context.CO_ChannelGauge on DischargeBedLevel.GaugeID equals Gauge.ID
                //                        join Channel in context.CO_Channel on Gauge.ChannelID equals Channel.ID
                //                        join Section in context.CO_Section on Gauge.SectionID equals Section.ID
                //                        join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                //                        join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                //                        join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                //                        join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                //                        orderby DischargeBedLevel.ReadingDate descending

                //                        where (_ZoneID == -1 || Zone.ID == _ZoneID)
                //                              && (_DivisionID == -1 || Division.ID == _DivisionID)
                //                              && (_CircleID == -1 || Circle.ID == _CircleID)
                //                              && (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                //                              && (DbFunctions.TruncateTime(DischargeBedLevel.ReadingDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(DischargeBedLevel.ReadingDate) : DbFunctions.TruncateTime(fromDate)))
                //                              && (DbFunctions.TruncateTime(DischargeBedLevel.ReadingDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(DischargeBedLevel.ReadingDate) : DbFunctions.TruncateTime(toDate)))
                //                               && ((_DesignationID == (long)Constants.Designation.XEN ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.ADM ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SDO ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SE ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                //                              && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? DischargeBedLevel.ScheduleDetailChannelID != null : DischargeBedLevel.ScheduleDetailChannelID == null))

                //                        select new
                //                        {
                //                            ID = DischargeBedLevel.ScheduleDetailChannelID == null ? DischargeBedLevel.ID : DischargeBedLevel.ScheduleDetailChannelID,
                //                            GaugeID = Gauge.ID,
                //                            InspectionDatetime = DischargeBedLevel.ReadingDate,
                //                            DivisionName = Division.Name,
                //                            SortOrder = DischargeBedLevel.ID,
                //                            ChannelName = Channel.NAME,
                //                            ExponentValue = DischargeBedLevel.ExponentValue,
                //                            MeanDepth = DischargeBedLevel.MeanDepth,
                //                            ObservedDischarge = DischargeBedLevel.DischargeObserved,
                //                            GaugeCorrectionType = DischargeBedLevel.GaugeCorrectionType == true ? "Bed Sourced" : "Bed Silted",
                //                            GaugeCorrectionValue = DischargeBedLevel.GaugeValueCorrection,
                //                            CoefficientDischarge = DischargeBedLevel.DischargeCoefficient,
                //                            InspectionType = DischargeBedLevel.ScheduleDetailChannelID == null ? "No" : "Yes",
                //                            IsScheduled = DischargeBedLevel.ScheduleDetailChannelID == null ? false : true,
                //                            InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,


                //                        }).ToList()
                //                         .Select(u => new
                //                         {
                //                             ID = u.ID,
                //                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime.Value),
                //                             DivisionName = u.DivisionName,
                //                             ChannelName = u.ChannelName,
                //                             SortOrder = u.SortOrder,
                //                             ExponentValue = u.ExponentValue,
                //                             MeanDepth = u.MeanDepth,
                //                             InspectionType = u.InspectionType,
                //                             IsScheduled = u.IsScheduled,
                //                             InspectedBy = u.InspectedBy,
                //                             ObservedDischarge = u.ObservedDischarge,
                //                             GaugeCorrectionType = u.GaugeCorrectionType,
                //                             GaugeCorrectionValue = u.GaugeCorrectionValue,
                //                             CoefficientDischarge = u.CoefficientDischarge,
                //                             InspectionArea = GetInspectionName(u.GaugeID)
                //                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();


                DischargeDataBedtLvl = (from DischargeBedLevel in context.CO_ChannelGaugeDTPGatedStructure
                                        join usr in context.UA_Users on DischargeBedLevel.CreatedBy equals usr.ID
                                        join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                        join Gauge in context.CO_ChannelGauge on DischargeBedLevel.GaugeID equals Gauge.ID
                                        join Channel in context.CO_Channel on Gauge.ChannelID equals Channel.ID
                                        join Section in context.CO_Section on Gauge.SectionID equals Section.ID
                                        join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                                        join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                                        join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                        join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                                        orderby DischargeBedLevel.ReadingDate descending

                                        where
                                            //(_ZoneID == -1 || Zone.ID == _ZoneID)
                                            //&& (_DivisionID == -1 || Division.ID == _DivisionID)
                                            //&& (_CircleID == -1 || Circle.ID == _CircleID)
                                            //&& (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                                            (UserZone.FirstOrDefault() == -1 || UserZone.Contains(Zone.ID))// == _ZoneID)
                                              && (UserCircles.FirstOrDefault() == -1 || UserCircles.Contains(Circle.ID))// == _CircleID)
                                              && (UserDiv.FirstOrDefault() == -1 || UserDiv.Contains(Division.ID)) // == _DivisionID)                                              
                                              && (UserSubDiv.FirstOrDefault() == -1 || UserSubDiv.Contains(SubDivision.ID))// == _SubDivisionID)
                                            //&& (DbFunctions.TruncateTime(DischargeBedLevel.ReadingDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(DischargeBedLevel.ReadingDate) : DbFunctions.TruncateTime(fromDate)))
                                            //&& (DbFunctions.TruncateTime(DischargeBedLevel.ReadingDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(DischargeBedLevel.ReadingDate) : DbFunctions.TruncateTime(toDate)))
                                              && (DbFunctions.TruncateTime(DischargeBedLevel.ObservationDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(DischargeBedLevel.ObservationDate) : DbFunctions.TruncateTime(fromDate)))
                                              && (DbFunctions.TruncateTime(DischargeBedLevel.ObservationDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(DischargeBedLevel.ObservationDate) : DbFunctions.TruncateTime(toDate)))
                                               && ((_DesignationID == (long)Constants.Designation.XEN ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.ADM ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SDO ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SE ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                              && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? DischargeBedLevel.ScheduleDetailChannelID != null : DischargeBedLevel.ScheduleDetailChannelID == null))

                                        select new
                                        {
                                            ID = DischargeBedLevel.ScheduleDetailChannelID == null ? DischargeBedLevel.ID : DischargeBedLevel.ScheduleDetailChannelID,
                                            GaugeID = Gauge.ID,
                                            InspectionDatetime = DischargeBedLevel.ObservationDate, //DischargeBedLevel.ReadingDate,
                                            DivisionName = Division.Name,
                                            SortOrder = DischargeBedLevel.ID,
                                            ChannelName = Channel.NAME,
                                            ExponentValue = DischargeBedLevel.ExponentValue,
                                            MeanDepth = DischargeBedLevel.MeanDepth,
                                            ObservedDischarge = DischargeBedLevel.DischargeObserved,
                                            GaugeCorrectionType = DischargeBedLevel.GaugeCorrectionType == true ? "Bed Sourced" : "Bed Silted",
                                            GaugeCorrectionValue = DischargeBedLevel.GaugeValueCorrection,
                                            CoefficientDischarge = DischargeBedLevel.DischargeCoefficient,
                                            InspectionType = DischargeBedLevel.ScheduleDetailChannelID == null ? "No" : "Yes",
                                            IsScheduled = DischargeBedLevel.ScheduleDetailChannelID == null ? false : true,
                                            InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,


                                        }).ToList()
                                         .Select(u => new
                                         {
                                             ID = u.ID,
                                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime.Value),
                                             DivisionName = u.DivisionName,
                                             ChannelName = u.ChannelName,
                                             SortOrder = u.SortOrder,
                                             ExponentValue = u.ExponentValue,
                                             MeanDepth = u.MeanDepth,
                                             InspectionType = u.InspectionType,
                                             IsScheduled = u.IsScheduled,
                                             InspectedBy = u.InspectedBy,
                                             ObservedDischarge = u.ObservedDischarge,
                                             GaugeCorrectionType = u.GaugeCorrectionType,
                                             GaugeCorrectionValue = u.GaugeCorrectionValue,
                                             CoefficientDischarge = u.CoefficientDischarge,
                                             InspectionArea = GetInspectionName(u.GaugeID)
                                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                return DischargeDataBedtLvl;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dynamic> GetDischargeTableCrestLevelBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZones, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                List<dynamic> DischargeDataCrestLvl = new List<dynamic>();

                // new work 
                List<long> UserSubDiv = new List<long>();
                List<long> UserDiv = new List<long>();
                List<long> UserCircles = new List<long>();
                List<long> UserZones = new List<long>();

                if (_lstUserZones != null)
                    UserZones.AddRange(_lstUserZones);

                if (_lstUserCircle != null)
                    UserCircles.AddRange(_lstUserCircle);

                if (_lstUserDiv != null)
                    UserDiv.AddRange(_lstUserDiv);

                if (_lstUserSubDiv != null)
                    UserSubDiv.AddRange(_lstUserSubDiv);

                if (_ZoneID != -1)
                {
                    UserZones.Clear();
                    UserZones.Add(_ZoneID);
                }
                else
                {
                    if (_lstUserZones == null || _lstUserZones.Count() == 0)
                    {
                        UserZones.Clear();
                        UserZones.Add(-1);
                    }
                }

                if (_CircleID != -1)
                {
                    UserCircles.Clear();
                    UserCircles.Add(_CircleID);
                }
                else
                {
                    if (_lstUserCircle == null || _lstUserCircle.Count() == 0)
                    {
                        UserCircles.Clear();
                        UserCircles.Add(-1);
                    }
                }

                if (_DivisionID != -1)
                {
                    UserDiv.Clear();
                    UserDiv.Add(_DivisionID); // get selected division
                }
                else
                {
                    if (_lstUserDiv == null || _lstUserDiv.Count() == 0)
                    {
                        UserDiv.Clear();
                        UserDiv.Add(-1); // get all divisions
                    }
                    //else  retain user divisions                        
                }

                if (_SubDivisionID != -1)
                {
                    UserSubDiv.Clear();
                    UserSubDiv.Add(_DivisionID);
                }
                else
                {
                    if (_lstUserSubDiv == null || _lstUserSubDiv.Count() == 0)
                    {
                        UserSubDiv.Clear();
                        UserSubDiv.Add(-1);
                    }
                }
                //

                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);

                //DischargeDataCrestLvl = (from DischargeCrestLevel in context.CO_ChannelGaugeDTPFall
                //                         join usr in context.UA_Users on DischargeCrestLevel.CreatedBy equals usr.ID
                //                         join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                //                         join Gauge in context.CO_ChannelGauge on DischargeCrestLevel.GaugeID equals Gauge.ID
                //                         join Channel in context.CO_Channel on Gauge.ChannelID equals Channel.ID
                //                         join Section in context.CO_Section on Gauge.SectionID equals Section.ID
                //                         join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                //                         join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                //                         join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                //                         join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                //                         orderby DischargeCrestLevel.ReadingDate descending

                //                         where (_ZoneID == -1 || Zone.ID == _ZoneID)
                //                               && (_DivisionID == -1 || Division.ID == _DivisionID)
                //                               && (_CircleID == -1 || Circle.ID == _CircleID)
                //                               && (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                //                               && (DbFunctions.TruncateTime(DischargeCrestLevel.ReadingDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(DischargeCrestLevel.ReadingDate) : DbFunctions.TruncateTime(fromDate)))
                //                               && (DbFunctions.TruncateTime(DischargeCrestLevel.ReadingDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(DischargeCrestLevel.ReadingDate) : DbFunctions.TruncateTime(toDate)))
                //                                && ((_DesignationID == (long)Constants.Designation.XEN ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.ADM ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SDO ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SE ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                //                               && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? DischargeCrestLevel.ScheduleDetailChannelID != null : DischargeCrestLevel.ScheduleDetailChannelID == null))

                //                         select new
                //                         {
                //                             ID = DischargeCrestLevel.ScheduleDetailChannelID == null ? DischargeCrestLevel.ID : DischargeCrestLevel.ScheduleDetailChannelID,
                //                             GaugeID = Gauge.ID,
                //                             InspectionDatetime = DischargeCrestLevel.ReadingDate,
                //                             SortOrder = DischargeCrestLevel.ID,
                //                             DivisionName = Division.Name,
                //                             ChannelName = Channel.NAME,
                //                             BreadthFall = DischargeCrestLevel.BreadthFall,
                //                             HeadAboveCrest = DischargeCrestLevel.HeadAboveCrest,
                //                             ObservedDischarge = DischargeCrestLevel.DischargeObserved,
                //                             CoefficientDischarge = DischargeCrestLevel.DischargeCoefficient,
                //                             InspectionType = DischargeCrestLevel.ScheduleDetailChannelID == null ? "No" : "Yes",
                //                             IsScheduled = DischargeCrestLevel.ScheduleDetailChannelID == null ? false : true,
                //                             InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,


                //                         }).ToList()
                //                         .Select(u => new
                //                         {
                //                             ID = u.ID,
                //                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime.Value),
                //                             DivisionName = u.DivisionName,
                //                             ChannelName = u.ChannelName,
                //                             SortOrder = u.SortOrder,
                //                             BreadthFall = u.BreadthFall,
                //                             HeadAboveCrest = u.HeadAboveCrest,
                //                             InspectionType = u.InspectionType,
                //                             IsScheduled = u.IsScheduled,
                //                             InspectedBy = u.InspectedBy,
                //                             ObservedDischarge = u.ObservedDischarge,
                //                             CoefficientDischarge = u.CoefficientDischarge,
                //                             InspectionArea = GetInspectionName(u.GaugeID)
                //                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                DischargeDataCrestLvl = (from DischargeCrestLevel in context.CO_ChannelGaugeDTPFall
                                         join usr in context.UA_Users on DischargeCrestLevel.CreatedBy equals usr.ID
                                         join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                         join Gauge in context.CO_ChannelGauge on DischargeCrestLevel.GaugeID equals Gauge.ID
                                         join Channel in context.CO_Channel on Gauge.ChannelID equals Channel.ID
                                         join Section in context.CO_Section on Gauge.SectionID equals Section.ID
                                         join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                                         join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                                         join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                         join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                                         orderby DischargeCrestLevel.ReadingDate descending

                                         where //(_ZoneID == -1 || Zone.ID == _ZoneID)
                                             //&& (_DivisionID == -1 || Division.ID == _DivisionID)
                                             //&& (_CircleID == -1 || Circle.ID == _CircleID)
                                             //&& (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                                              (UserZones.FirstOrDefault() == -1 || UserZones.Contains(Zone.ID))// == _ZoneID)
                                               && (UserCircles.FirstOrDefault() == -1 || UserCircles.Contains(Circle.ID))// == _CircleID)
                                               && (UserDiv.FirstOrDefault() == -1 || UserDiv.Contains(Division.ID))// == _DivisionID)                                               
                                               && (UserSubDiv.FirstOrDefault() == -1 || UserSubDiv.Contains(SubDivision.ID)) //== _SubDivisionID)
                                             //&& (DbFunctions.TruncateTime(DischargeCrestLevel.ReadingDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(DischargeCrestLevel.ReadingDate) : DbFunctions.TruncateTime(fromDate)))
                                             //&& (DbFunctions.TruncateTime(DischargeCrestLevel.ReadingDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(DischargeCrestLevel.ReadingDate) : DbFunctions.TruncateTime(toDate)))
                                               && (DbFunctions.TruncateTime(DischargeCrestLevel.ObservationDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(DischargeCrestLevel.ObservationDate) : DbFunctions.TruncateTime(fromDate)))
                                               && (DbFunctions.TruncateTime(DischargeCrestLevel.ObservationDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(DischargeCrestLevel.ObservationDate) : DbFunctions.TruncateTime(toDate)))
                                                && ((_DesignationID == (long)Constants.Designation.XEN ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.ADM ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SDO ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SE ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                               && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? DischargeCrestLevel.ScheduleDetailChannelID != null : DischargeCrestLevel.ScheduleDetailChannelID == null))

                                         select new
                                         {
                                             ID = DischargeCrestLevel.ScheduleDetailChannelID == null ? DischargeCrestLevel.ID : DischargeCrestLevel.ScheduleDetailChannelID,
                                             GaugeID = Gauge.ID,
                                             InspectionDatetime = DischargeCrestLevel.ObservationDate, //DischargeCrestLevel.ReadingDate,
                                             SortOrder = DischargeCrestLevel.ID,
                                             DivisionName = Division.Name,
                                             ChannelName = Channel.NAME,
                                             BreadthFall = DischargeCrestLevel.BreadthFall,
                                             HeadAboveCrest = DischargeCrestLevel.HeadAboveCrest,
                                             ObservedDischarge = DischargeCrestLevel.DischargeObserved,
                                             CoefficientDischarge = DischargeCrestLevel.DischargeCoefficient,
                                             InspectionType = DischargeCrestLevel.ScheduleDetailChannelID == null ? "No" : "Yes",
                                             IsScheduled = DischargeCrestLevel.ScheduleDetailChannelID == null ? false : true,
                                             InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,


                                         }).ToList()
                                         .Select(u => new
                                         {
                                             ID = u.ID,
                                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime.Value),
                                             DivisionName = u.DivisionName,
                                             ChannelName = u.ChannelName,
                                             SortOrder = u.SortOrder,
                                             BreadthFall = u.BreadthFall,
                                             HeadAboveCrest = u.HeadAboveCrest,
                                             InspectionType = u.InspectionType,
                                             IsScheduled = u.IsScheduled,
                                             InspectedBy = u.InspectedBy,
                                             ObservedDischarge = u.ObservedDischarge,
                                             CoefficientDischarge = u.CoefficientDischarge,
                                             InspectionArea = GetInspectionName(u.GaugeID)
                                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();


                return DischargeDataCrestLvl;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public List<dynamic> GetOutletPerformanceBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {

                List<dynamic> OutletPerformance = new List<dynamic>();
                // new work 
                List<long> UserSubDiv = new List<long>();
                List<long> UserDiv = new List<long>();
                List<long> UserCircles = new List<long>();
                List<long> UserZones = new List<long>();

                if (_lstUserZone != null)
                    UserZones.AddRange(_lstUserZone);

                if (_lstUserCircle != null)
                    UserCircles.AddRange(_lstUserCircle);

                if (_lstUserDiv != null)
                    UserDiv.AddRange(_lstUserDiv);

                if (_lstUserSubDiv != null)
                    UserSubDiv.AddRange(_lstUserSubDiv);


                if (_ZoneID != -1)
                {
                    UserZones.Clear();
                    UserZones.Add(_ZoneID);
                }
                else
                {
                    if (_lstUserZone == null || _lstUserZone.Count() == 0)
                    {
                        UserZones.Clear();
                        UserZones.Add(-1);
                    }
                }

                if (_CircleID != -1)
                {
                    UserCircles.Clear();
                    UserCircles.Add(_CircleID);
                }
                else
                {
                    if (_lstUserCircle == null || _lstUserCircle.Count() == 0)
                    {
                        UserCircles.Clear();
                        UserCircles.Add(-1);
                    }
                }

                if (_DivisionID != -1)
                {
                    UserDiv.Clear();
                    UserDiv.Add(_DivisionID); // get selected division
                }
                else
                {
                    if (_lstUserDiv == null || _lstUserDiv.Count() == 0)
                    {
                        UserDiv.Clear();
                        UserDiv.Add(-1); // get all divisions
                    }
                    //else  retain user divisions                        
                }

                if (_SubDivisionID != -1)
                {
                    UserSubDiv.Clear();
                    UserSubDiv.Add(_DivisionID);
                }
                else
                {
                    if (_lstUserSubDiv == null || _lstUserSubDiv.Count() == 0)
                    {
                        UserSubDiv.Clear();
                        UserSubDiv.Add(-1);
                    }
                }
                //



                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);

                //OutletPerformance = (from OutletPerformanceData in context.CO_ChannelOutletsPerformance
                //                     join usr in context.UA_Users on OutletPerformanceData.CreatedBy equals usr.ID
                //                     join ScheduleDetail in context.SI_ScheduleDetailChannel on OutletPerformanceData.ScheduleDetailChannelID equals ScheduleDetail.ID into sdt
                //                     from ScheduleDetail1 in sdt.DefaultIfEmpty()

                //                     join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                //                     join Outlet in context.CO_ChannelOutlets on OutletPerformanceData.OutletID equals Outlet.ID

                //                     join Channel in context.CO_Channel on Outlet.ChannelID equals Channel.ID
                //                     join IrrigationBoundries in context.CO_ChannelIrrigationBoundaries on Channel.ID equals IrrigationBoundries.ChannelID
                //                     join Section in context.CO_Section on IrrigationBoundries.SectionID equals Section.ID
                //                     join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                //                     join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                //                     join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                //                     join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID

                //                     orderby OutletPerformanceData.ObservationDate descending

                //                     where //(IrrigationBoundries.SectionRD < Outlet.OutletRD && IrrigationBoundries.SectionToRD > Outlet.OutletRD)
                //                         //&& 
                //                         //(context.WT_GetChannelIDAndRDsBySectionIDs(IrrigationBoundries.SectionID,IrrigationBoundries.ChannelID).)
                //                           (_ZoneID == -1 || Zone.ID == _ZoneID)
                //                           && (_DivisionID == -1 || Division.ID == _DivisionID)
                //                           && (_CircleID == -1 || Circle.ID == _CircleID)
                //                           && (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                //                           && (DbFunctions.TruncateTime(OutletPerformanceData.ObservationDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(OutletPerformanceData.ObservationDate) : DbFunctions.TruncateTime(fromDate)))
                //                           && (DbFunctions.TruncateTime(OutletPerformanceData.ObservationDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(OutletPerformanceData.ObservationDate) : DbFunctions.TruncateTime(toDate)))
                //                            && ((_DesignationID == (long)Constants.Designation.XEN ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.ADM ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SDO ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SE ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                //                           && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletPerformanceData.ScheduleDetailChannelID != null : OutletPerformanceData.ScheduleDetailChannelID == null))

                //                     select new
                //                     {
                //                         OutletPerformanceID = OutletPerformanceData.ScheduleDetailChannelID == null ? OutletPerformanceData.ID : OutletPerformanceData.ScheduleDetailChannelID,
                //                         IsScheduled = OutletPerformanceData.ScheduleDetailChannelID == null ? false : true,
                //                         ScheduleID = ScheduleDetail1.ScheduleID == null ? 0 : ScheduleDetail1.ScheduleID,
                //                         OutletID = Outlet.ID,
                //                         SortOrder = OutletPerformanceData.ID,
                //                         InspectionDatetime = OutletPerformanceData.ObservationDate,
                //                         DivisionName = Division.Name,
                //                         ChannelName = Channel.NAME,
                //                         HeadAboveCrest = OutletPerformanceData.HeadAboveCrest,
                //                         WorkingHead = OutletPerformanceData.WorkingHead,
                //                         InspectionType = OutletPerformanceData.ScheduleDetailChannelID == null ? "No" : "Yes",
                //                         InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,


                //                     }).ToList()
                //                         .Select(u => new
                //                         {
                //                             OutletPerformanceID = u.OutletPerformanceID,
                //                             IsScheduled = u.IsScheduled,
                //                             ScheduleID = u.ScheduleID,
                //                             SortOrder = u.SortOrder,
                //                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime.Value),
                //                             DivisionName = u.DivisionName,
                //                             ChannelName = u.ChannelName,
                //                             HeadAboveCrest = u.HeadAboveCrest,
                //                             InspectionType = u.InspectionType,
                //                             InspectedBy = u.InspectedBy,
                //                             WorkingHead = u.WorkingHead,
                //                             Outlet = GetOtletFormattedName(u.OutletID)
                //                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                OutletPerformance = (from OutletPerformanceData in context.CO_ChannelOutletsPerformance
                                     join usr in context.UA_Users on OutletPerformanceData.CreatedBy equals usr.ID
                                     join ScheduleDetail in context.SI_ScheduleDetailChannel on OutletPerformanceData.ScheduleDetailChannelID equals ScheduleDetail.ID into sdt
                                     from ScheduleDetail1 in sdt.DefaultIfEmpty()

                                     join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                     join Outlet in context.CO_ChannelOutlets on OutletPerformanceData.OutletID equals Outlet.ID

                                     join Channel in context.CO_Channel on Outlet.ChannelID equals Channel.ID
                                     join IrrigationBoundries in context.CO_ChannelIrrigationBoundaries on Channel.ID equals IrrigationBoundries.ChannelID
                                     join Section in context.CO_Section on IrrigationBoundries.SectionID equals Section.ID
                                     join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                                     join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                                     join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                     join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID

                                     orderby OutletPerformanceData.ObservationDate descending

                                     where //(IrrigationBoundries.SectionRD < Outlet.OutletRD && IrrigationBoundries.SectionToRD > Outlet.OutletRD)
                                         //&& 
                                         //(context.WT_GetChannelIDAndRDsBySectionIDs(IrrigationBoundries.SectionID,IrrigationBoundries.ChannelID).)
                                         //(_ZoneID == -1 || Zone.ID == _ZoneID)
                                         //&& (_DivisionID == -1 || Division.ID == _DivisionID)
                                         //&& (_CircleID == -1 || Circle.ID == _CircleID)
                                         //&& (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                                              (UserZones.FirstOrDefault() == -1 || UserZones.Contains(Zone.ID))// == _ZoneID)
                                           && (UserCircles.FirstOrDefault() == -1 || UserCircles.Contains(Circle.ID)) // == _CircleID) 
                                           && (UserDiv.FirstOrDefault() == -1 || UserDiv.Contains(Division.ID))// == _DivisionID)                                           
                                           && (UserSubDiv.FirstOrDefault() == -1 || UserSubDiv.Contains(SubDivision.ID))// == _SubDivisionID)
                                           && (DbFunctions.TruncateTime(OutletPerformanceData.ObservationDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(OutletPerformanceData.ObservationDate) : DbFunctions.TruncateTime(fromDate)))
                                           && (DbFunctions.TruncateTime(OutletPerformanceData.ObservationDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(OutletPerformanceData.ObservationDate) : DbFunctions.TruncateTime(toDate)))
                                            && ((_DesignationID == (long)Constants.Designation.XEN ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.ADM ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SDO ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SE ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                           && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletPerformanceData.ScheduleDetailChannelID != null : OutletPerformanceData.ScheduleDetailChannelID == null))

                                     select new
                                     {
                                         OutletPerformanceID = OutletPerformanceData.ScheduleDetailChannelID == null ? OutletPerformanceData.ID : OutletPerformanceData.ScheduleDetailChannelID,
                                         IsScheduled = OutletPerformanceData.ScheduleDetailChannelID == null ? false : true,
                                         ScheduleID = ScheduleDetail1.ScheduleID == null ? 0 : ScheduleDetail1.ScheduleID,
                                         OutletID = Outlet.ID,
                                         SortOrder = OutletPerformanceData.ID,
                                         InspectionDatetime = OutletPerformanceData.ObservationDate,
                                         DivisionName = Division.Name,
                                         ChannelName = Channel.NAME,
                                         HeadAboveCrest = OutletPerformanceData.HeadAboveCrest,
                                         WorkingHead = OutletPerformanceData.WorkingHead,
                                         InspectionType = OutletPerformanceData.ScheduleDetailChannelID == null ? "No" : "Yes",
                                         InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,


                                     }).ToList()
                                         .Select(u => new
                                         {
                                             OutletPerformanceID = u.OutletPerformanceID,
                                             IsScheduled = u.IsScheduled,
                                             ScheduleID = u.ScheduleID,
                                             SortOrder = u.SortOrder,
                                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime.Value),
                                             DivisionName = u.DivisionName,
                                             ChannelName = u.ChannelName,
                                             HeadAboveCrest = u.HeadAboveCrest,
                                             InspectionType = u.InspectionType,
                                             InspectedBy = u.InspectedBy,
                                             WorkingHead = u.WorkingHead,
                                             Outlet = GetOtletFormattedName(u.OutletID)
                                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                return OutletPerformance;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public List<dynamic> GetOutletCheckingBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {

                List<dynamic> OutletPerformance = new List<dynamic>();
                // new work 
                List<long> UserSubDiv = new List<long>();
                List<long> UserDiv = new List<long>();
                List<long> UserCircles = new List<long>();
                List<long> UserZones = new List<long>();

                if (_lstUserZone != null)
                    UserZones.AddRange(_lstUserZone);

                if (_lstUserCircle != null)
                    UserCircles.AddRange(_lstUserCircle);

                if (_lstUserDiv != null)
                    UserDiv.AddRange(_lstUserDiv);

                if (_lstUserSubDiv != null)
                    UserSubDiv.AddRange(_lstUserSubDiv);


                if (_ZoneID != -1)
                {
                    UserZones.Clear();
                    UserZones.Add(_ZoneID);
                }
                else
                {
                    if (_lstUserZone == null || _lstUserZone.Count() == 0)
                    {
                        UserZones.Clear();
                        UserZones.Add(-1);
                    }
                }

                if (_CircleID != -1)
                {
                    UserCircles.Clear();
                    UserCircles.Add(_CircleID);
                }
                else
                {
                    if (_lstUserCircle == null || _lstUserCircle.Count() == 0)
                    {
                        UserCircles.Clear();
                        UserCircles.Add(-1);
                    }
                }

                if (_DivisionID != -1)
                {
                    UserDiv.Clear();
                    UserDiv.Add(_DivisionID); // get selected division
                }
                else
                {
                    if (_lstUserDiv == null || _lstUserDiv.Count() == 0)
                    {
                        UserDiv.Clear();
                        UserDiv.Add(-1); // get all divisions
                    }
                    //else  retain user divisions                        
                }

                if (_SubDivisionID != -1)
                {
                    UserSubDiv.Clear();
                    UserSubDiv.Add(_DivisionID);
                }
                else
                {
                    if (_lstUserSubDiv == null || _lstUserSubDiv.Count() == 0)
                    {
                        UserSubDiv.Clear();
                        UserSubDiv.Add(-1);
                    }
                }
                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);
                OutletPerformance = (from OutletChecking in context.SI_OutletChecking
                                     join usr in context.UA_Users on OutletChecking.CreatedBy equals usr.ID
                                     join ScheduleDetail in context.SI_ScheduleDetailChannel on OutletChecking.ScheduleDetailChannelID equals ScheduleDetail.ID into sdt
                                     from ScheduleDetail1 in sdt.DefaultIfEmpty()

                                     join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                     join Outlet in context.CO_ChannelOutlets on OutletChecking.OutletID equals Outlet.ID

                                     join Channel in context.CO_Channel on Outlet.ChannelID equals Channel.ID
                                     join IrrigationBoundries in context.CO_ChannelIrrigationBoundaries on Channel.ID equals IrrigationBoundries.ChannelID
                                     join Section in context.CO_Section on IrrigationBoundries.SectionID equals Section.ID
                                     join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                                     join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                                     join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                     join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                                     where
                                     (UserZones.FirstOrDefault() == -1 || UserZones.Contains(Zone.ID))// == _ZoneID)
                                           && (UserCircles.FirstOrDefault() == -1 || UserCircles.Contains(Circle.ID)) // == _CircleID) 
                                           && (UserDiv.FirstOrDefault() == -1 || UserDiv.Contains(Division.ID))// == _DivisionID)                                           
                                           && (UserSubDiv.FirstOrDefault() == -1 || UserSubDiv.Contains(SubDivision.ID))// == _SubDivisionID)
                                           && ((_DesignationID == (long)Constants.Designation.XEN ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.ADM ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SDO ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SE ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                           && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletChecking.ScheduleDetailChannelID != null : OutletChecking.ScheduleDetailChannelID == null))

                                     select new
                                     {
                                         OutletCheckinID = OutletChecking.ID,
                                         IsScheduled = OutletChecking.ScheduleDetailChannelID == null ? false : true,
                                         ScheduleID = ScheduleDetail1.ScheduleID == null ? 0 : ScheduleDetail1.ScheduleID,
                                         OutletID = Outlet.ID,
                                         SortOrder = OutletChecking.ID,
                                         InspectionDatetime = OutletChecking.ReadingMobileDate,
                                         DivisionName = Division.Name,
                                         ChannelName = Channel.NAME,
                                         HeadAboveCrest = OutletChecking.HValue,
                                         InspectionType = OutletChecking.ScheduleDetailChannelID == null ? "No" : "Yes",
                                         InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                                     }).ToList()
                                         .Select(u => new
                                         {
                                             OutletCheckinID = u.OutletCheckinID,
                                             IsScheduled = u.IsScheduled,
                                             ScheduleID = u.ScheduleID,
                                             SortOrder = u.SortOrder,
                                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime),
                                             DivisionName = u.DivisionName,
                                             ChannelName = u.ChannelName,
                                             HeadAboveCrest = u.HeadAboveCrest,
                                             InspectionType = u.InspectionType,
                                             InspectedBy = u.InspectedBy,
                                             Outlet = GetOtletFormattedName(u.OutletID),
                                             OutletID = u.OutletID
                                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                return OutletPerformance;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<dynamic> GetOutletAlterationHistoryBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                List<dynamic> OutletAlterationHistory = new List<dynamic>();

                // new work 
                List<long> UserSubDiv = new List<long>();
                List<long> UserDiv = new List<long>();
                List<long> UserCircles = new List<long>();
                List<long> UserZone = new List<long>();

                if (_lstUserZone != null)
                    UserZone.AddRange(_lstUserZone);

                if (_lstUserCircle != null)
                    UserCircles.AddRange(_lstUserCircle);

                if (_lstUserDiv != null)
                    UserDiv.AddRange(_lstUserDiv);

                if (_lstUserSubDiv != null)
                    UserSubDiv.AddRange(_lstUserSubDiv);

                if (_ZoneID != -1)
                {
                    UserZone.Clear();
                    UserZone.Add(_ZoneID);
                }
                else
                {
                    if (_lstUserZone == null || _lstUserZone.Count() == 0)
                    {
                        UserZone.Clear();
                        UserZone.Add(-1);
                    }
                }

                if (_CircleID != -1)
                {
                    UserCircles.Clear();
                    UserCircles.Add(_CircleID);
                }
                else
                {
                    if (_lstUserCircle == null || _lstUserCircle.Count() == 0)
                    {
                        UserCircles.Clear();
                        UserCircles.Add(-1);
                    }
                }

                if (_DivisionID != -1)
                {
                    UserDiv.Clear();
                    UserDiv.Add(_DivisionID); // get selected division
                }
                else
                {
                    if (_lstUserDiv == null || _lstUserDiv.Count() == 0)
                    {
                        UserDiv.Clear();
                        UserDiv.Add(-1); // get all divisions
                    }
                    //else  retain user divisions                        
                }

                if (_SubDivisionID != -1)
                {
                    UserSubDiv.Clear();
                    UserSubDiv.Add(_DivisionID);
                }
                else
                {
                    if (_lstUserSubDiv == null || _lstUserSubDiv.Count() == 0)
                    {
                        UserSubDiv.Clear();
                        UserSubDiv.Add(-1);
                    }
                }
                //

                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);

                //OutletAlterationHistory = (from OutletHistoryData in context.SI_OutletAlterationHistroy
                //                           join usr in context.UA_Users on OutletHistoryData.CreatedBy equals usr.ID
                //                           join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                //                           join Outlet in context.CO_ChannelOutlets on OutletHistoryData.OutletID equals Outlet.ID
                //                           join ScheduleDetail in context.SI_ScheduleDetailChannel on OutletHistoryData.ScheduleDetailChannelID equals ScheduleDetail.ID into sdt
                //                           from ScheduleDetail1 in sdt.DefaultIfEmpty()
                //                           join Channel in context.CO_Channel on Outlet.ChannelID equals Channel.ID
                //                           join IrrigationBoundries in context.CO_ChannelIrrigationBoundaries on Channel.ID equals IrrigationBoundries.ChannelID
                //                           join Section in context.CO_Section on IrrigationBoundries.SectionID equals Section.ID
                //                           join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                //                           join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                //                           join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                //                           join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                //                           orderby OutletHistoryData.AlterationDate descending

                //                           where (IrrigationBoundries.SectionRD < Outlet.OutletRD && IrrigationBoundries.SectionToRD > Outlet.OutletRD)
                //                                 && (_ZoneID == -1 || Zone.ID == _ZoneID)
                //                                 && (_DivisionID == -1 || Division.ID == _DivisionID)
                //                                 && (_CircleID == -1 || Circle.ID == _CircleID)
                //                                 && (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                //                                 && (DbFunctions.TruncateTime(OutletHistoryData.AlterationDate) >= ((string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(OutletHistoryData.AlterationDate) : DbFunctions.TruncateTime(fromDate))))
                //                                 && (DbFunctions.TruncateTime(OutletHistoryData.AlterationDate) <= ((string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(OutletHistoryData.AlterationDate) : DbFunctions.TruncateTime(toDate))))
                //                                 && ((_DesignationID == (long)Constants.Designation.XEN ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.ADM ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SDO ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                //                              (_DesignationID == (long)Constants.Designation.SE ?
                //                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                //                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                //                                 && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletHistoryData.ScheduleDetailChannelID != null : OutletHistoryData.ScheduleDetailChannelID == null))

                //                           select new
                //                           {
                //                               OutletAlterationHistoryID = OutletHistoryData.ScheduleDetailChannelID == null ? OutletHistoryData.ID : OutletHistoryData.ScheduleDetailChannelID,
                //                               IsScheduled = OutletHistoryData.ScheduleDetailChannelID == null ? false : true,
                //                               ScheduleID = ScheduleDetail1.ScheduleID == null ? 0 : ScheduleDetail1.ScheduleID,
                //                               SortOrder = OutletHistoryData.ID,
                //                               OutletID = Outlet.ID,
                //                               InspectionDatetime = OutletHistoryData.AlterationDate,
                //                               DivisionName = Division.Name,
                //                               ChannelName = Channel.NAME,
                //                               HeightOrifice = OutletHistoryData.OutletHeight,
                //                               WorkingHead = OutletHistoryData.OutletWorkingHead,
                //                               InspectionType = OutletHistoryData.ScheduleDetailChannelID == null ? "No" : "Yes",
                //                               InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name


                //                           }).ToList()
                //                         .Select(u => new
                //                         {
                //                             OutletAlterationHistoryID = u.OutletAlterationHistoryID,
                //                             IsScheduled = u.IsScheduled,
                //                             ScheduleID = u.ScheduleID,
                //                             SortOrder = u.SortOrder,
                //                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime),
                //                             DivisionName = u.DivisionName,
                //                             ChannelName = u.ChannelName,
                //                             HeightOrifice = u.HeightOrifice,
                //                             InspectionType = u.InspectionType,
                //                             InspectedBy = u.InspectedBy,
                //                             WorkingHead = u.WorkingHead,
                //                             Outlet = GetOtletFormattedName(u.OutletID)
                //                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                OutletAlterationHistory = (from OutletHistoryData in context.SI_OutletAlterationHistroy
                                           join usr in context.UA_Users on OutletHistoryData.CreatedBy equals usr.ID
                                           join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                           join Outlet in context.CO_ChannelOutlets on OutletHistoryData.OutletID equals Outlet.ID
                                           join ScheduleDetail in context.SI_ScheduleDetailChannel on OutletHistoryData.ScheduleDetailChannelID equals ScheduleDetail.ID into sdt
                                           from ScheduleDetail1 in sdt.DefaultIfEmpty()
                                           join Channel in context.CO_Channel on Outlet.ChannelID equals Channel.ID
                                           join IrrigationBoundries in context.CO_ChannelIrrigationBoundaries on Channel.ID equals IrrigationBoundries.ChannelID
                                           join Section in context.CO_Section on IrrigationBoundries.SectionID equals Section.ID
                                           join SubDivision in context.CO_SubDivision on Section.SubDivID equals SubDivision.ID
                                           join Division in context.CO_Division on SubDivision.DivisionID equals Division.ID
                                           join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                           join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                                           orderby OutletHistoryData.AlterationDate descending

                                           where (IrrigationBoundries.SectionRD < Outlet.OutletRD && IrrigationBoundries.SectionToRD > Outlet.OutletRD)
                                               //&& (_ZoneID == -1 || Zone.ID == _ZoneID)
                                               //&& (_DivisionID == -1 || Division.ID == _DivisionID)
                                               //&& (_CircleID == -1 || Circle.ID == _CircleID)
                                               //&& (_SubDivisionID == -1 || SubDivision.ID == _SubDivisionID)
                                                 && (UserZone.FirstOrDefault() == -1 || UserZone.Contains(Zone.ID))// == _ZoneID)
                                                 && (UserCircles.FirstOrDefault() == -1 || UserCircles.Contains(Circle.ID))// == _CircleID)
                                                 && (UserDiv.FirstOrDefault() == -1 || UserDiv.Contains(Division.ID))// == _DivisionID)                                                 
                                                 && (UserSubDiv.FirstOrDefault() == -1 || UserSubDiv.Contains(SubDivision.ID))// == _SubDivisionID)
                                                 && (DbFunctions.TruncateTime(OutletHistoryData.AlterationDate) >= ((string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(OutletHistoryData.AlterationDate) : DbFunctions.TruncateTime(fromDate))))
                                                 && (DbFunctions.TruncateTime(OutletHistoryData.AlterationDate) <= ((string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(OutletHistoryData.AlterationDate) : DbFunctions.TruncateTime(toDate))))
                                                 && ((_DesignationID == (long)Constants.Designation.XEN ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.ADM ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SDO ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SE ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                                 && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletHistoryData.ScheduleDetailChannelID != null : OutletHistoryData.ScheduleDetailChannelID == null))

                                           select new
                                           {
                                               OutletAlterationHistoryID = OutletHistoryData.ScheduleDetailChannelID == null ? OutletHistoryData.ID : OutletHistoryData.ScheduleDetailChannelID,
                                               IsScheduled = OutletHistoryData.ScheduleDetailChannelID == null ? false : true,
                                               ScheduleID = ScheduleDetail1.ScheduleID == null ? 0 : ScheduleDetail1.ScheduleID,
                                               SortOrder = OutletHistoryData.ID,
                                               OutletID = Outlet.ID,
                                               InspectionDatetime = OutletHistoryData.AlterationDate,
                                               DivisionName = Division.Name,
                                               ChannelName = Channel.NAME,
                                               HeightOrifice = OutletHistoryData.OutletHeight,
                                               WorkingHead = OutletHistoryData.OutletWorkingHead,
                                               InspectionType = OutletHistoryData.ScheduleDetailChannelID == null ? "No" : "Yes",
                                               InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name


                                           }).ToList()
                                         .Select(u => new
                                         {
                                             OutletAlterationHistoryID = u.OutletAlterationHistoryID,
                                             IsScheduled = u.IsScheduled,
                                             ScheduleID = u.ScheduleID,
                                             SortOrder = u.SortOrder,
                                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime),
                                             DivisionName = u.DivisionName,
                                             ChannelName = u.ChannelName,
                                             HeightOrifice = u.HeightOrifice,
                                             InspectionType = u.InspectionType,
                                             InspectedBy = u.InspectedBy,
                                             WorkingHead = u.WorkingHead,
                                             Outlet = GetOtletFormattedName(u.OutletID)
                                         }).Distinct().OrderByDescending(x => x.SortOrder).ToList<dynamic>();

                return OutletAlterationHistory;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<dynamic> GetTenderMonitoringBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                List<dynamic> TenderMonitoring = new List<dynamic>();
                // new work 
                List<long> UserSubDiv = new List<long>();
                List<long> UserDiv = new List<long>();
                List<long> UserCircles = new List<long>();
                List<long> UserZone = new List<long>();

                if (_lstUserZone != null)
                    UserZone.AddRange(_lstUserZone);

                if (_lstUserCircle != null)
                    UserCircles.AddRange(_lstUserCircle);

                if (_lstUserDiv != null)
                    UserDiv.AddRange(_lstUserDiv);

                if (_lstUserSubDiv != null)
                    UserSubDiv.AddRange(_lstUserSubDiv);


                if (_ZoneID != -1)
                {
                    UserZone.Clear();
                    UserZone.Add(_ZoneID);
                }
                else
                {
                    if (_lstUserZone == null || _lstUserZone.Count() == 0)
                    {
                        UserZone.Clear();
                        UserZone.Add(-1);
                    }
                }

                if (_CircleID != -1)
                {
                    UserCircles.Clear();
                    UserCircles.Add(_CircleID);
                }
                else
                {
                    if (_lstUserCircle == null || _lstUserCircle.Count() == 0)
                    {
                        UserCircles.Clear();
                        UserCircles.Add(-1);
                    }
                }

                if (_DivisionID != -1)
                {
                    UserDiv.Clear();
                    UserDiv.Add(_DivisionID); // get selected division
                }
                else
                {
                    if (_lstUserDiv == null || _lstUserDiv.Count() == 0)
                    {
                        UserDiv.Clear();
                        UserDiv.Add(-1); // get all divisions
                    }
                    //else  retain user divisions                        
                }

                if (_SubDivisionID != -1)
                {
                    UserSubDiv.Clear();
                    UserSubDiv.Add(_DivisionID);
                }
                else
                {
                    if (_lstUserSubDiv == null || _lstUserSubDiv.Count() == 0)
                    {
                        UserSubDiv.Clear();
                        UserSubDiv.Add(-1);
                    }
                }
                //

                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);

                //TenderMonitoring = (from Tenders in context.SI_ScheduleDetailTender
                //                    join TN in context.TM_TenderNotice on Tenders.TenderNoticeID equals TN.ID
                //                    join TW in context.TM_TenderWorks on Tenders.TenderWorksID equals TW.ID
                //                    join Complaints in context.CM_Complaint on TW.ID equals Complaints.RefCodeID into sdct
                //                    from Complaints1 in sdct.DefaultIfEmpty()
                //                    join Work in context.CW_ClosureWork on TW.WorkSourceID equals Work.ID
                //                    join usr in context.UA_Users on Tenders.CreatedBy equals usr.ID
                //                    join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                //                    join Division in context.CO_Division on Tenders.DivisionID equals Division.ID
                //                    join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                //                    join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                //                    orderby Tenders.ID descending

                //                    where (_ZoneID == -1 || Zone.ID == _ZoneID)
                //                          && (_DivisionID == -1 || Division.ID == _DivisionID)
                //                          && (_CircleID == -1 || Circle.ID == _CircleID)
                //                         && (DbFunctions.TruncateTime(Tenders.OpeningDate) >= ((string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(Tenders.OpeningDate) : DbFunctions.TruncateTime(fromDate))))
                //                          && (DbFunctions.TruncateTime(Tenders.OpeningDate) <= ((string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(Tenders.OpeningDate) : DbFunctions.TruncateTime(toDate))))
                //                          && ((_DesignationID == (long)Constants.Designation.XEN ?
                //                       (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                       (_DesignationID == (long)Constants.Designation.ADM ?
                //                       (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                       (_DesignationID == (long)Constants.Designation.SDO ?
                //                       (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                       (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                //                       (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                //                       (_DesignationID == (long)Constants.Designation.SE ?
                //                       (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                //                       (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                //                    //&& (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletHistoryData.ScheduleDetailChannelID != null : OutletHistoryData.ScheduleDetailChannelID == null))

                //                    select new
                //                    {
                //                        ScheduleDetailID = Tenders.ID,
                //                        WorkSourceID = TW.WorkSourceID,
                //                        DivisionID = Division.ID,
                //                        Division = Division.Name,
                //                        TenderNoticeID = Tenders.TenderNoticeID,
                //                        TenderNotice = TN.Name,
                //                        TenderWorkID = TW.ID,
                //                        Work = Work.WorkName,
                //                        OpeningDate = TN.BidOpeningDate,
                //                        Remarks = Tenders.Remarks,
                //                        InspectionType = "ALL",
                //                        InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                //                        ComplaintGenerated = Complaints1.ID == null ? "No" : "Yes",
                //                        InspectionDatetime = Tenders.OpeningDate


                //                    }).ToList()
                //                         .Select(u => new
                //                         {
                //                             ScheduleDetailID = u.ScheduleDetailID,
                //                             WorkSourceID = u.WorkSourceID,
                //                             DivisionID = u.DivisionID,
                //                             Division = u.Division,
                //                             TenderNoticeID = u.TenderNoticeID,
                //                             TenderNotice = u.TenderNotice,
                //                             TenderWorkID = u.TenderWorkID,
                //                             Work = u.Work,
                //                             OpeningDate = Utility.GetFormattedDate(u.OpeningDate),
                //                             OpeningDateTime = u.OpeningDate,
                //                             Remarks = u.Remarks,
                //                             InspectionType = u.InspectionType,
                //                             InspectedBy = u.InspectedBy,
                //                             ComplaintGenerated = u.ComplaintGenerated,
                //                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime)
                //                         }).Distinct().OrderByDescending(x => x.ScheduleDetailID).ToList<dynamic>();


                TenderMonitoring = (from Tenders in context.SI_ScheduleDetailTender
                                    join TN in context.TM_TenderNotice on Tenders.TenderNoticeID equals TN.ID
                                    join TW in context.TM_TenderWorks on Tenders.TenderWorksID equals TW.ID
                                    join Complaints in context.CM_Complaint on TW.ID equals Complaints.RefCodeID into sdct
                                    from Complaints1 in sdct.DefaultIfEmpty()
                                    join Work in context.CW_ClosureWork on TW.WorkSourceID equals Work.ID
                                    join usr in context.UA_Users on Tenders.CreatedBy equals usr.ID
                                    join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                    join Division in context.CO_Division on Tenders.DivisionID equals Division.ID
                                    join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                    join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                                    orderby Tenders.ID descending

                                    where //(_ZoneID == -1 || Zone.ID == _ZoneID)
                                        //&& (_DivisionID == -1 || Division.ID == _DivisionID)
                                        //&& (_CircleID == -1 || Circle.ID == _CircleID)
                                          (UserZone.FirstOrDefault() == -1 || UserZone.Contains(Zone.ID))// == _ZoneID)
                                          && (UserDiv.FirstOrDefault() == -1 || UserDiv.Contains(Division.ID))// == _DivisionID)
                                          && (UserCircles.FirstOrDefault() == -1 || UserCircles.Contains(Circle.ID))// == _CircleID)
                                         && (DbFunctions.TruncateTime(Tenders.OpeningDate) >= ((string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(Tenders.OpeningDate) : DbFunctions.TruncateTime(fromDate))))
                                          && (DbFunctions.TruncateTime(Tenders.OpeningDate) <= ((string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(Tenders.OpeningDate) : DbFunctions.TruncateTime(toDate))))
                                          && ((_DesignationID == (long)Constants.Designation.XEN ?
                                       (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                       (_DesignationID == (long)Constants.Designation.ADM ?
                                       (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                       (_DesignationID == (long)Constants.Designation.SDO ?
                                       (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                       (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                       (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                       (_DesignationID == (long)Constants.Designation.SE ?
                                       (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                       (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                    //&& (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletHistoryData.ScheduleDetailChannelID != null : OutletHistoryData.ScheduleDetailChannelID == null))

                                    select new
                                    {
                                        ScheduleDetailID = Tenders.ID,
                                        WorkSourceID = TW.WorkSourceID,
                                        DivisionID = Division.ID,
                                        Division = Division.Name,
                                        TenderNoticeID = Tenders.TenderNoticeID,
                                        TenderNotice = TN.Name,
                                        TenderWorkID = TW.ID,
                                        Work = Work.WorkName,
                                        OpeningDate = TN.BidOpeningDate,
                                        Remarks = Tenders.Remarks,
                                        InspectionType = "ALL",
                                        InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                                        ComplaintGenerated = Complaints1.ID == null ? "No" : "Yes",
                                        InspectionDatetime = Tenders.OpeningDate


                                    }).ToList()
                                         .Select(u => new
                                         {
                                             ScheduleDetailID = u.ScheduleDetailID,
                                             WorkSourceID = u.WorkSourceID,
                                             DivisionID = u.DivisionID,
                                             Division = u.Division,
                                             TenderNoticeID = u.TenderNoticeID,
                                             TenderNotice = u.TenderNotice,
                                             TenderWorkID = u.TenderWorkID,
                                             Work = u.Work,
                                             OpeningDate = Utility.GetFormattedDate(u.OpeningDate),
                                             OpeningDateTime = u.OpeningDate,
                                             Remarks = u.Remarks,
                                             InspectionType = u.InspectionType,
                                             InspectedBy = u.InspectedBy,
                                             ComplaintGenerated = u.ComplaintGenerated,
                                             InspectionDatetime = Utility.GetFormattedDate(u.InspectionDatetime)
                                         }).Distinct().OrderByDescending(x => x.ScheduleDetailID).ToList<dynamic>();

                return TenderMonitoring;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<dynamic> GetClosureOperationsBySearchCriteria(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID, List<long> _lstUserZone, List<long> _lstUserCircle, List<long> _lstUserDiv, List<long> _lstUserSubDiv)
        {
            try
            {
                List<dynamic> ClosureOperations = new List<dynamic>();
                // new work 
                List<long> UserSubDiv = new List<long>();
                List<long> UserDiv = new List<long>();
                List<long> UserCircles = new List<long>();
                List<long> UserZone = new List<long>();

                if (_lstUserZone != null)
                    UserZone.AddRange(_lstUserZone);

                if (_lstUserCircle != null)
                    UserCircles.AddRange(_lstUserCircle);

                if (_lstUserDiv != null)
                    UserDiv.AddRange(_lstUserDiv);

                if (_lstUserSubDiv != null)
                    UserSubDiv.AddRange(_lstUserSubDiv);

                if (_ZoneID != -1)
                {
                    UserZone.Clear();
                    UserZone.Add(_ZoneID);
                }
                else
                {
                    if (_lstUserZone == null || _lstUserZone.Count() == 0)
                    {
                        UserZone.Clear();
                        UserZone.Add(-1);
                    }
                }

                if (_CircleID != -1)
                {
                    UserCircles.Clear();
                    UserCircles.Add(_CircleID);
                }
                else
                {
                    if (_lstUserCircle == null || _lstUserCircle.Count() == 0)
                    {
                        UserCircles.Clear();
                        UserCircles.Add(-1);
                    }
                }

                if (_DivisionID != -1)
                {
                    UserDiv.Clear();
                    UserDiv.Add(_DivisionID); // get selected division
                }
                else
                {
                    if (_lstUserDiv == null || _lstUserDiv.Count() == 0)
                    {
                        UserDiv.Clear();
                        UserDiv.Add(-1); // get all divisions
                    }
                    //else  retain user divisions                        
                }

                if (_SubDivisionID != -1)
                {
                    UserSubDiv.Clear();
                    UserSubDiv.Add(_DivisionID);
                }
                else
                {
                    if (_lstUserSubDiv == null || _lstUserSubDiv.Count() == 0)
                    {
                        UserSubDiv.Clear();
                        UserSubDiv.Add(-1);
                    }
                }
                //

                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);

                //ClosureOperations = (from Closures in context.SI_ScheduleDetailWorks
                //                     join Work in context.CW_ClosureWork on Closures.WorkSourceID equals Work.ID
                //                     join usr in context.UA_Users on Closures.CreatedBy equals usr.ID
                //                     join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                //                     join Division in context.CO_Division on Closures.DivisionID equals Division.ID
                //                     join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                //                     join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                //                     orderby Closures.ID descending

                //                     where (_ZoneID == -1 || Zone.ID == _ZoneID)
                //                           && (_DivisionID == -1 || Division.ID == _DivisionID)
                //                           && (_CircleID == -1 || Circle.ID == _CircleID)
                //                          && (DbFunctions.TruncateTime(Closures.MonitoringDate) >= ((string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(Closures.MonitoringDate) : DbFunctions.TruncateTime(fromDate))))
                //                           && (DbFunctions.TruncateTime(Closures.MonitoringDate) <= ((string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(Closures.MonitoringDate) : DbFunctions.TruncateTime(toDate))))
                //                           && ((_DesignationID == (long)Constants.Designation.XEN ?
                //                        (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                        (_DesignationID == (long)Constants.Designation.ADM ?
                //                        (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                        (_DesignationID == (long)Constants.Designation.SDO ?
                //                        (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                //                        (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                //                        (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                //                        (_DesignationID == (long)Constants.Designation.SE ?
                //                        (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                //                        (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                //                     //&& (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletHistoryData.ScheduleDetailChannelID != null : OutletHistoryData.ScheduleDetailChannelID == null))

                //                     select new
                //                     {
                //                         ScheduleDetailID = Closures.ID,
                //                         WorkSourceID = Closures.WorkSourceID,
                //                         WorkSource = Closures.WorkSource,
                //                         DivisionID = Division.ID,
                //                         Division = Division.Name,
                //                         WorkName = Work.WorkName,
                //                         MonitoringDate = Closures.MonitoringDate,
                //                         Remarks = Closures.Remarks,
                //                         RefMonitoringID = Closures.RefMonitoringID,
                //                         CWPID = Work.CWPID,
                //                         //InspectionType = "ALL",
                //                         InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                //                         //ComplaintGenerated = Complaints1.ID == null ? "No" : "Yes",
                //                         InspectionDatetime = Closures.MonitoringDate


                //                     }).ToList()
                //                         .Select(u => new
                //                         {
                //                             ScheduleDetailID = u.ScheduleDetailID,
                //                             WorkSourceID = u.WorkSourceID,
                //                             WorkType = u.WorkSource,
                //                             DivisionID = u.DivisionID,
                //                             Division = u.Division,
                //                             WorkName = u.WorkName,
                //                             MonitoringDate = Utility.GetFormattedDate(u.MonitoringDate),
                //                             MonitoringDateTime = u.MonitoringDate,
                //                             Remarks = u.Remarks,
                //                             CWPID = u.CWPID,
                //                             RefMonitoringID = u.RefMonitoringID,
                //                             InspectedBy = u.InspectedBy,
                //                             InspectionDatetime = Utility.GetFormattedDateTime(u.InspectionDatetime)
                //                         }).Distinct().OrderByDescending(x => x.ScheduleDetailID).ToList<dynamic>();

                ClosureOperations = (from Closures in context.SI_ScheduleDetailWorks
                                     join Work in context.CW_ClosureWork on Closures.WorkSourceID equals Work.ID
                                     join usr in context.UA_Users on Closures.CreatedBy equals usr.ID
                                     join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                     join Division in context.CO_Division on Closures.DivisionID equals Division.ID
                                     join Circle in context.CO_Circle on Division.CircleID equals Circle.ID
                                     join Zone in context.CO_Zone on Circle.ZoneID equals Zone.ID
                                     orderby Closures.ID descending

                                     where //(_ZoneID == -1 || Zone.ID == _ZoneID)
                                         //&& (_DivisionID == -1 || Division.ID == _DivisionID)
                                         //&& (_CircleID == -1 || Circle.ID == _CircleID)
                                            (UserZone.FirstOrDefault() == -1 || UserZone.Contains(Zone.ID))// == _ZoneID)
                                           && (UserDiv.FirstOrDefault() == -1 || UserDiv.Contains(Division.ID))// == _DivisionID)
                                           && (UserCircles.FirstOrDefault() == -1 || UserCircles.Contains(Circle.ID))// == _CircleID)
                                          && (DbFunctions.TruncateTime(Closures.MonitoringDate) >= ((string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(Closures.MonitoringDate) : DbFunctions.TruncateTime(fromDate))))
                                           && (DbFunctions.TruncateTime(Closures.MonitoringDate) <= ((string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(Closures.MonitoringDate) : DbFunctions.TruncateTime(toDate))))
                                           && ((_DesignationID == (long)Constants.Designation.XEN ?
                                        (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                        (_DesignationID == (long)Constants.Designation.ADM ?
                                        (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                        (_DesignationID == (long)Constants.Designation.SDO ?
                                        (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                        (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                        (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                        (_DesignationID == (long)Constants.Designation.SE ?
                                        (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                        (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                     //&& (_InspectionCategory == -1 || (_InspectionCategory == 1 ? OutletHistoryData.ScheduleDetailChannelID != null : OutletHistoryData.ScheduleDetailChannelID == null))

                                     select new
                                     {
                                         ScheduleDetailID = Closures.ID,
                                         WorkSourceID = Closures.WorkSourceID,
                                         WorkSource = Closures.WorkSource,
                                         DivisionID = Division.ID,
                                         Division = Division.Name,
                                         WorkName = Work.WorkName,
                                         MonitoringDate = Closures.MonitoringDate,
                                         Remarks = Closures.Remarks,
                                         RefMonitoringID = Closures.RefMonitoringID,
                                         CWPID = Work.CWPID,
                                         //InspectionType = "ALL",
                                         InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,
                                         //ComplaintGenerated = Complaints1.ID == null ? "No" : "Yes",
                                         InspectionDatetime = Closures.MonitoringDate


                                     }).ToList()
                                        .Select(u => new
                                        {
                                            ScheduleDetailID = u.ScheduleDetailID,
                                            WorkSourceID = u.WorkSourceID,
                                            WorkType = u.WorkSource,
                                            DivisionID = u.DivisionID,
                                            Division = u.Division,
                                            WorkName = u.WorkName,
                                            MonitoringDate = Utility.GetFormattedDate(u.MonitoringDate),
                                            MonitoringDateTime = u.MonitoringDate,
                                            Remarks = u.Remarks,
                                            CWPID = u.CWPID,
                                            RefMonitoringID = u.RefMonitoringID,
                                            InspectedBy = u.InspectedBy,
                                            InspectionDatetime = Utility.GetFormattedDateTime(u.InspectionDatetime)
                                        }).Distinct().OrderByDescending(x => x.ScheduleDetailID).ToList<dynamic>();

                return ClosureOperations;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<dynamic> GetGeneralInspectionsBySearchCriteria(long _InspectionCategory, string _FromDate, string _ToDate, long _UserID, long? _DesignationID)
        {
            try
            {
                List<dynamic> GeneralInspections = new List<dynamic>();
                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(_FromDate))
                    fromDate = Utility.GetParsedDate(_FromDate);
                if (!string.IsNullOrEmpty(_ToDate))
                    toDate = Utility.GetParsedDate(_ToDate);

                GeneralInspections = (from GI in context.SI_GeneralInspections
                                      join usr in context.UA_Users on GI.CreatedBy equals usr.ID
                                      join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                      join ins in context.SI_GeneralInspectionType on GI.GeneralInspectionTypeID equals ins.ID
                                      orderby GI.InspectionDate descending

                                      where (DbFunctions.TruncateTime(GI.InspectionDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(GI.InspectionDate) : DbFunctions.TruncateTime(fromDate)))
                                              && (DbFunctions.TruncateTime(GI.InspectionDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(GI.InspectionDate) : DbFunctions.TruncateTime(toDate)))
                                               && ((_DesignationID == (long)Constants.Designation.XEN ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.ADM ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SDO ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.DeputyDirector ?
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.SE) :
                                              (_DesignationID == (long)Constants.Designation.SE ?
                                              (usr.DesignationID != (long)Constants.Designation.MA && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector) :
                                              (usr.DesignationID != (long)Constants.Designation.SDO && usr.DesignationID != (long)Constants.Designation.XEN && usr.DesignationID != (long)Constants.Designation.ADM && usr.DesignationID != (long)Constants.Designation.DeputyDirector && usr.DesignationID != (long)Constants.Designation.SE)))))))
                                              && (_InspectionCategory == -1 || (_InspectionCategory == 1 ? GI.ScheduleDetailGeneralID != null : GI.ScheduleDetailGeneralID == null))

                                      select new
                                      {
                                          ID = GI.ID,
                                          ScheduleDetailID = GI.ScheduleDetailGeneralID,
                                          InspectionDatetime = GI.InspectionDate,
                                          InspectionTypeID = GI.GeneralInspectionTypeID,
                                          InspectionDetails = GI.InspectionDetails,
                                          InspectionType = ins.Name,
                                          Location = GI.InspectionLocation,
                                          Remarks = GI.Remarks,
                                          InspectionTypeCat = GI.ScheduleDetailGeneralID == null ? "UnScheduled" : "Scheduled",
                                          InspectedBy = usr.FirstName + " " + usr.LastName + ", " + dsg.Name,


                                      }).ToList()
                                         .Select(u => new
                                         {
                                             ID = u.ID,
                                             InspectionDate = Utility.GetFormattedDate(u.InspectionDatetime),
                                             ScheduleDetailID = u.ScheduleDetailID,
                                             InspectionTypeID = u.InspectionTypeID,
                                             InspectionDetails = u.InspectionDetails,
                                             InspectionType = u.InspectionType,
                                             Location = u.Location,
                                             Remarks = u.Remarks,
                                             InspectionTypeCat = u.InspectionTypeCat,
                                             InspectedBy = u.InspectedBy
                                         }).Distinct().OrderByDescending(x => x.ID).ToList<dynamic>();

                return GeneralInspections;

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        public bool IsScheduleInspectionsExists(long _ScheduleID)
        {
            return context.SI_IsSchedeleInspectionsExists(_ScheduleID).Select(x => x.Value).FirstOrDefault();
        }

        public List<SI_GetOutletDetail_Result> GetOutletByOutletID(long _OutletID)
        {
            var q = from g in context.SI_GetOutletDetail(_OutletID) select g;
            return q.ToList<SI_GetOutletDetail_Result>();
        }

        public object GetOutletAlterationHistoryDetail(long _ScheduleChnlDetailID, long _OutletID)
        {
            object qOutletAlterationInspection = (from oai in context.SI_OutletAlterationHistroy
                                                  where oai.ScheduleDetailChannelID == _ScheduleChnlDetailID &&
                                                  oai.OutletID == _OutletID
                                                  select new
                                                  {

                                                      workingHead = oai.OutletWorkingHead,
                                                      outletHeight = oai.OutletHeight,
                                                      outletWidth = oai.OutletWidth,
                                                      outletCrest = oai.OutletCrest,
                                                      outletRemarks = oai.Remarks

                                                  }).FirstOrDefault();
            return qOutletAlterationInspection;
        }

        public object GetChannelNameAndRD(long _GaugeID)
        {
            object ChannelGaugeData = (from ChannelGauge in context.CO_ChannelGauge
                                       join Channel in context.CO_Channel on ChannelGauge.ChannelID equals Channel.ID
                                       where ChannelGauge.ID == _GaugeID
                                       select new
                                       {
                                           ChannelID = ChannelGauge.ChannelID,
                                           ChannelName = Channel.NAME,
                                           GaugeRD = ChannelGauge.GaugeAtRD,

                                       }).ToList()
                                                  .Select(h => new
                                                {
                                                    ChannelID = h.ChannelID,
                                                    ChannelName = h.ChannelName,
                                                    GaugeRD = PMIU.WRMIS.AppBlocks.Calculations.GetRDText(h.GaugeRD)
                                                }).FirstOrDefault();
            return ChannelGaugeData;
        }
        public object GetOutletAlterationPerformanceDetail(long _ScheduleChnlDetailID, long _OutletID)
        {
            object qOutletAlterationInspection = (from cop in context.CO_ChannelOutletsPerformance
                                                  where cop.ScheduleDetailChannelID == _ScheduleChnlDetailID &&
                                                  cop.OutletID == _OutletID
                                                  select new
                                                  {

                                                      outletWorkingHead = cop.WorkingHead,
                                                      outletHeadAboveCrest = cop.HeadAboveCrest,
                                                      outletDischarge = cop.Discharge,
                                                      outletHeight = cop.ObservedHeightY,
                                                      outletWidth = cop.ObservedWidthB,
                                                      outletRemarks = cop.Remarks

                                                  }).FirstOrDefault();
            return qOutletAlterationInspection;
        }



        public bool SavelstInspectionScheduleDetail(List<SI_ScheduleDetailChannel> _lstScheduleDetailChannel)
        {
            try
            {
                if (_lstScheduleDetailChannel.Count > 0)
                {
                    context.SI_ScheduleDetailChannel.AddRange(_lstScheduleDetailChannel);
                    context.SaveChanges();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        #region Schedule Calendar
        public List<object> GetUserInspectionDates(long _UserID, long _Month, long _Year)
        {
            List<object> lstInspectionDates = (from ssdc in context.SI_ScheduleDetailChannel
                                               where ssdc.CreatedBy == _UserID
                                               && ssdc.ScheduleDate.Month == _Month
                                               && ssdc.ScheduleDate.Year == _Year
                                               select new
                                               {
                                                   date = ssdc.ScheduleDate,
                                               }).Distinct().ToList().
                                              Select(d => new
                                              {
                                                  date = d.date,
                                                  badge = false,
                                                  title = d.date
                                              }).ToList<object>();
            List<object> lstTenderInspectionDates = (from ssdc in context.SI_ScheduleDetailTender
                                                     where ssdc.CreatedBy == _UserID
                                                     && ssdc.OpeningDate.Month == _Month
                                                     && ssdc.OpeningDate.Year == _Year
                                                     select new
                                                     {
                                                         date = ssdc.OpeningDate,
                                                     }).Distinct().ToList().
                                           Select(d => new
                                           {
                                               date = d.date,
                                               badge = false,
                                               title = d.date
                                           }).ToList<object>();
            List<object> lstClosureInspectionDates = (from ssdc in context.SI_ScheduleDetailWorks
                                                      where ssdc.CreatedBy == _UserID
                                                      && ssdc.MonitoringDate.Month == _Month
                                                      && ssdc.MonitoringDate.Year == _Year
                                                      select new
                                                      {
                                                          date = ssdc.MonitoringDate,
                                                      }).Distinct().ToList().
                                           Select(d => new
                                           {
                                               date = d.date,
                                               badge = false,
                                               title = d.date
                                           }).ToList<object>();
            List<object> lstGeneralInspectionDates = (from ssdc in context.SI_ScheduleDetailGeneral
                                                      where ssdc.CreatedBy == _UserID
                                                      && ssdc.ScheduleDate.Month == _Month
                                                      && ssdc.ScheduleDate.Year == _Year
                                                      select new
                                                      {
                                                          date = ssdc.ScheduleDate,
                                                      }).Distinct().ToList().
                                         Select(d => new
                                         {
                                             date = d.date,
                                             badge = false,
                                             title = d.date
                                         }).ToList<object>();
            lstInspectionDates.AddRange(lstTenderInspectionDates);
            lstInspectionDates.AddRange(lstClosureInspectionDates);
            lstInspectionDates.AddRange(lstGeneralInspectionDates);
            return lstInspectionDates;
        }
        public List<dynamic> GetUserInspectionsByDate(long _UserID, DateTime _Date)
        {
            List<dynamic> lstUserInspections = context.SI_GetUserInspectionsByDate(_UserID, _Date).ToList()
                                                .Select(i => new
                                                {
                                                    i.ScheduleID,
                                                    i.ScheduleDetailID,
                                                    i.InspectionTypeID,
                                                    i.InspectionType,
                                                    i.ScheduleStatusID,
                                                    i.ChannelName,
                                                    i.ChannelID,
                                                    i.GaugeLevelID,
                                                    i.IsGaugeReadingInspectionExists,
                                                    i.IsOutletAlterationInspectionExists,
                                                    i.IsOutletPerformanceInspectionExists,
                                                    i.IsBedLevelGaugeInspectionExists,
                                                    i.IsCrestLevelGaugeInspectionExists,
                                                    i.IsOutletCheckingInspectionExists,
                                                    i.InspectionAtGauge,
                                                    i.InspectionAtOutlet,
                                                    i.OutletCheckingID
                                                }).ToList<dynamic>();
            return lstUserInspections;
        }

        public List<dynamic> GetTenderInspectionsByDate(long _UserID, DateTime _Date)
        {
            List<dynamic> lstTenderInspections = (from SCT in context.SI_ScheduleDetailTender
                                                  join ssd in
                                                      (from ssd3 in context.SI_ScheduleStatusDetail
                                                       where
                                         (from ssd2 in context.SI_ScheduleStatusDetail
                                          group ssd2 by ssd2.ScheduleID into cd
                                          select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
                                                       select ssd3) on SCT.ScheduleID equals ssd.ScheduleID
                                                  join SStatus in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals SStatus.ID
                                                  //join Schedule in context.SI_Schedule on SCT.ScheduleID equals Schedule.ID
                                                  join usr in context.UA_Users on SCT.CreatedBy equals usr.ID
                                                  join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                                  join TN in context.TM_TenderNotice on SCT.TenderNoticeID equals TN.ID
                                                  join TW in context.TM_TenderWorks on SCT.TenderWorksID equals TW.ID
                                                  join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                                                  join CWT in context.CW_WorkType on CW.WorkTypeID equals CWT.ID
                                                  join TWC in context.TM_TenderWorksContractors on SCT.TenderWorksID equals TWC.TenderWorksID into TWC1
                                                  from TWC2 in TWC1.DefaultIfEmpty()

                                                  orderby SCT.ID descending

                                                  where SCT.CreatedBy == _UserID && DbFunctions.TruncateTime(SCT.OpeningDate) == DbFunctions.TruncateTime(_Date)


                                                  select new
                                                  {
                                                      ScheduleDetailID = SCT.ID,
                                                      ScheduleID = SCT.ScheduleID,
                                                      TenderWorkID = SCT.TenderWorksID,
                                                      WorkSourceID = TW.WorkSourceID,
                                                      TenderWorkName = CW.WorkName,
                                                      TenderNoticeID = SCT.TenderNoticeID,
                                                      TenderNoticeName = TN.Name,
                                                      ScheduleStatusID = ssd.ScheduleStatusID,
                                                      ScheduleStatus = SStatus.Name,
                                                      WorkTypeName = CWT.Name,
                                                      IsTenderSold = TWC2.TenderWorksID == null ? false : true,

                                                  }).Distinct().ToList<dynamic>();



            return lstTenderInspections;
        }
        public List<dynamic> GetClosureInspectionsByDate(long _UserID, DateTime _Date)
        {
            List<dynamic> lstClosureInspections = (from SDC in context.SI_ScheduleDetailWorks
                                                   join ssd in
                                                       (from ssd3 in context.SI_ScheduleStatusDetail
                                                        where
                                          (from ssd2 in context.SI_ScheduleStatusDetail
                                           group ssd2 by ssd2.ScheduleID into cd
                                           select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
                                                        select ssd3) on SDC.ScheduleID equals ssd.ScheduleID
                                                   join SStatus in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals SStatus.ID
                                                   join usr in context.UA_Users on SDC.CreatedBy equals usr.ID
                                                   join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID
                                                   join CW in context.CW_ClosureWork on SDC.WorkSourceID equals CW.ID
                                                   join CWP in context.CW_ClosureWorkPlan on CW.CWPID equals CWP.ID

                                                   orderby SDC.ID descending

                                                   where SDC.CreatedBy == _UserID && DbFunctions.TruncateTime(SDC.MonitoringDate) == DbFunctions.TruncateTime(_Date)


                                                   select new
                                                   {
                                                       ScheduleDetailID = SDC.ID,
                                                       ScheduleID = SDC.ScheduleID,
                                                       ClosureWorkID = SDC.WorkSourceID,
                                                       WorkType = SDC.WorkSource,
                                                       CLosureWorkName = CW.WorkName,
                                                       CWPID = CWP.ID,
                                                       RefMonitoringID = SDC.RefMonitoringID,
                                                       ScheduleStatusID = ssd.ScheduleStatusID

                                                   }).Distinct().ToList<dynamic>();



            return lstClosureInspections;
        }
        public List<dynamic> GetGeneralInspectionsByDate(long _UserID, DateTime _Date)
        {
            List<dynamic> lstGeneralInspections = (from SDC in context.SI_ScheduleDetailGeneral
                                                   join SCG in context.SI_GeneralInspections on SDC.ID equals SCG.ScheduleDetailGeneralID into SCG1
                                                   from SCG2 in SCG1.DefaultIfEmpty()
                                                   join IType in context.SI_GeneralInspectionType on SDC.GeneralInspectionTypeID equals IType.ID
                                                   join ssd in
                                                       (from ssd3 in context.SI_ScheduleStatusDetail
                                                        where
                                          (from ssd2 in context.SI_ScheduleStatusDetail
                                           group ssd2 by ssd2.ScheduleID into cd
                                           select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
                                                        select ssd3) on SDC.ScheduleID equals ssd.ScheduleID
                                                   join SStatus in context.SI_ScheduleStatus on ssd.ScheduleStatusID equals SStatus.ID
                                                   join usr in context.UA_Users on SDC.CreatedBy equals usr.ID
                                                   join dsg in context.UA_Designations on usr.DesignationID equals dsg.ID


                                                   orderby SDC.ID descending

                                                   where SDC.CreatedBy == _UserID && DbFunctions.TruncateTime(SDC.ScheduleDate) == DbFunctions.TruncateTime(_Date)


                                                   select new
                                                   {
                                                       ScheduleDetailID = SDC.ID,
                                                       ScheduleID = SDC.ScheduleID,
                                                       ScheduleStatusID = ssd.ScheduleStatusID,
                                                       GIViewMode = SCG2 == null ? false : true,
                                                       Location = SDC.Location,
                                                       InspectionType = IType.Name

                                                   }).Distinct().ToList<dynamic>();



            return lstGeneralInspections;
        }
        private Int32 GetScheduleStatus(long _ScheduleID)
        {

            long scheduleStatusID = (from SSD in context.SI_ScheduleStatusDetail
                                     where SSD.ScheduleID == _ScheduleID
                                     orderby SSD.ID descending
                                     select SSD.ScheduleStatusID).FirstOrDefault();
            return 1;
        }
        #endregion

        public long GetDivisionIDByGaugeID(long _GaugeID)
        {
            long DivisionID = (from chnlGauge in context.CO_ChannelGauge
                               where chnlGauge.ID == _GaugeID
                               join section in context.CO_Section on chnlGauge.SectionID equals section.ID
                               join subDiv in context.CO_SubDivision on section.SubDivID equals subDiv.ID
                               join div in context.CO_Division on subDiv.DivisionID equals div.ID

                               select div.ID).FirstOrDefault();
            return DivisionID;
        }

        public SI_GetScheduleInspectionNotifyData_Result GetScheduleInspectionNotifyData(long _ScheduleID)
        {
            SI_GetScheduleInspectionNotifyData_Result lstScheduleInspectionNotifyData = context.SI_GetScheduleInspectionNotifyData(_ScheduleID).FirstOrDefault<SI_GetScheduleInspectionNotifyData_Result>();

            return lstScheduleInspectionNotifyData;
        }
        public SI_GetDischargeTableCalcBLNotifyData_Result GetDischargeTableCalcBLNotifyData(long _ScheduleDetailsID)
        {
            SI_GetDischargeTableCalcBLNotifyData_Result lstDischargeTableCalcBLNotifyData = context.SI_GetDischargeTableCalcBLNotifyData(_ScheduleDetailsID).FirstOrDefault<SI_GetDischargeTableCalcBLNotifyData_Result>();

            return lstDischargeTableCalcBLNotifyData;
        }
        public bool DeleteSIGaugeReading(long _GaugeReadingID)
        {
            bool DelResult = false;

            SI_ChannelGaugeReading objGaugeReading = new SI_ChannelGaugeReading();
            try
            {
                objGaugeReading = (from sce in context.SI_ChannelGaugeReading
                                   where sce.ID == _GaugeReadingID
                                   select sce).FirstOrDefault();
                if (objGaugeReading != null)
                {
                    context.SI_ChannelGaugeReading.Remove(objGaugeReading);
                    context.SaveChanges();
                    DelResult = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }


            return DelResult;
        }

        public object GetGaugeInsepctionDetail(int _GaugeID, long _ScheduleDetailID, string serverUploadFolder)
        {
            object gaugeInspection = (from gaugeInspect in context.SI_ChannelGaugeReading
                                      where gaugeInspect.GaugeID == _GaugeID && gaugeInspect.ScheduleDetailChannelID == _ScheduleDetailID
                                      select new
                                      {
                                          IsGaugeFixed = gaugeInspect.IsGaugeFixed,
                                          IsGaugePainted = gaugeInspect.IsGaugePainted,
                                          GaugeID = gaugeInspect.GaugeID,
                                          GaugeValue = gaugeInspect.GaugeValue,
                                          DailyDischarge = gaugeInspect.DailyDischarge,
                                          Remarks = gaugeInspect.Remarks,
                                          GaugePhoto = gaugeInspect.GaugePhoto,
                                      }).AsEnumerable().Select(u => new
                                                  {
                                                      IsGaugeFixed = u.IsGaugeFixed,
                                                      IsGaugePainted = u.IsGaugePainted,
                                                      GaugeID = u.GaugeID,
                                                      GaugeValue = Convert.ToString(u.GaugeValue),
                                                      DailyDischarge = Convert.ToString(u.DailyDischarge),
                                                      Remarks = u.Remarks,
                                                      image_url = Utility.GetImageURL(Configuration.ScheduleInspection, u.GaugePhoto),
                                                  }).FirstOrDefault();
            return gaugeInspection;
        }

        public object GetDTPFallInsepctionDetail(int _GaugeID, long _ScheduleDetailID)
        {
            object gaugeInspection = (from gaugeInspect in context.CO_ChannelGaugeDTPFall
                                      where gaugeInspect.GaugeID == _GaugeID && gaugeInspect.ScheduleDetailChannelID == _ScheduleDetailID
                                      select new
                                      {
                                          gaugeInspect.HeadAboveCrest,
                                          gaugeInspect.DischargeCoefficient,
                                          gaugeInspect.GaugeID,
                                          gaugeInspect.DischargeObserved,
                                          gaugeInspect.BreadthFall,
                                          gaugeInspect.Remarks

                                      }).AsEnumerable().Select(u => new
                                          {
                                              HeadAboveCrest = Convert.ToString(u.HeadAboveCrest),
                                              BreadthFall = Convert.ToString(u.BreadthFall),
                                              DischargeCoefficient = Convert.ToString(u.DischargeCoefficient),
                                              Remarks = u.Remarks,
                                              DischargeObserved = Convert.ToString(u.DischargeObserved),
                                              GaugeID = u.GaugeID
                                          }).FirstOrDefault();
            return gaugeInspection;
        }

        public string GetScheduleStatus(DateTime ToDate, long _ScheduleID)
        {
            bool qIsExists = false;
            string Status = "";
            if (ToDate >= DateTime.Now.Date)
            {
                Status = "Active/Planned";
            }
            else
            {
                List<long?> lstSDCIDs = (from SDC in context.SI_ScheduleDetailChannel
                                         where SDC.ScheduleID == _ScheduleID
                                         select (long?)SDC.ID).ToList();
                if (lstSDCIDs.Count > 0)
                {
                    qIsExists = context.SI_ChannelGaugeReading.Any(s => lstSDCIDs.Contains(s.ScheduleDetailChannelID));
                    if (!qIsExists)
                    {
                        qIsExists = context.CO_ChannelGaugeDTPGatedStructure.Any(s => lstSDCIDs.Contains(s.ScheduleDetailChannelID));
                    }
                    if (!qIsExists)
                    {
                        qIsExists = context.CO_ChannelGaugeDTPFall.Any(s => lstSDCIDs.Contains(s.ScheduleDetailChannelID));
                    }
                    if (!qIsExists)
                    {
                        qIsExists = context.SI_OutletAlterationHistroy.Any(s => lstSDCIDs.Contains(s.ScheduleDetailChannelID));
                    }
                    if (!qIsExists)
                    {
                        qIsExists = context.CO_ChannelOutletsPerformance.Any(s => lstSDCIDs.Contains(s.ScheduleDetailChannelID));
                    }

                }

                List<long?> lstSDGIDs = (from SDG in context.SI_ScheduleDetailGeneral
                                         where SDG.ScheduleID == _ScheduleID
                                         select (long?)SDG.ID).ToList();
                if (lstSDGIDs.Count > 0)
                {
                    if (!qIsExists)
                    {
                        qIsExists = context.SI_GeneralInspections.Any(s => lstSDGIDs.Contains(s.ScheduleDetailGeneralID));
                    }
                }
                //if (!qIsExists)
                //{
                //    qIsExists = context.SI_ScheduleDetailTender.Any(s => s.ScheduleID == _ScheduleID);
                //}
                if (!qIsExists)
                {
                    qIsExists = context.SI_ScheduleDetailWorks.Any(s => s.ScheduleID == _ScheduleID && s.RefMonitoringID != null);
                }

                if (qIsExists == false)
                    Status = "Expired";
                else
                    Status = "Executed";
            }
            return Status;
        }
        public object GetDTPGatedStructureInsepctionDetail(int _GaugeID, long _ScheduleDetailID)
        {

            object gaugeInspection = (from gaugeInspect in context.CO_ChannelGaugeDTPGatedStructure
                                      where gaugeInspect.GaugeID == _GaugeID && gaugeInspect.ScheduleDetailChannelID == _ScheduleDetailID
                                      select new
                                      {
                                          gaugeInspect.ExponentValue,
                                          gaugeInspect.DischargeCoefficient,
                                          gaugeInspect.GaugeID,
                                          gaugeInspect.DischargeObserved,
                                          gaugeInspect.GaugeCorrectionType,
                                          gaugeInspect.GaugeValueCorrection,
                                          gaugeInspect.MeanDepth,
                                          gaugeInspect.Remarks

                                      }).AsEnumerable().Select(u => new
                             {
                                 ExponentValue = Convert.ToString(u.ExponentValue),
                                 MeanDepth = Convert.ToString(u.MeanDepth),
                                 DischargeCoefficient = Convert.ToString(u.DischargeCoefficient),
                                 Remarks = u.Remarks,
                                 DischargeObserved = Convert.ToString(u.DischargeObserved),
                                 GaugeCorrectionType = u.GaugeCorrectionType == true ? 2 : u.GaugeCorrectionType == false ? 1 : -1,
                                 GaugeValueCorrection = Convert.ToString(u.GaugeValueCorrection),
                                 GaugeID = u.GaugeID
                             }).FirstOrDefault();
            return gaugeInspection;
        }

        public SI_Schedule GetScheduleDetailByScheduleDetailID(long _ScheduleDetailChannelID)
        {
            SI_Schedule scheduleObj = (from ss in context.SI_Schedule
                                       join ssd in context.SI_ScheduleDetailChannel on ss.ID equals ssd.ScheduleID
                                       where ssd.ID == _ScheduleDetailChannelID
                                       select ss
                                                 ).FirstOrDefault();

            return scheduleObj;
        }

        #region Tender Monitoring
        public List<object> GetTenderWorksByTenderNoticeID(long _TenderNoticeID)
        {
            List<object> lstTenderWorks = new List<object>();

            string ClosureWork = Constants.WorkType.CLOSURE.ToString();

            try
            {
                lstTenderWorks = (from TenderWorks in context.TM_TenderWorks
                                  join TN in context.TM_TenderNotice on TenderWorks.TenderNoticeID equals TN.ID
                                  //join CW in context.CW_ClosureWork on TenderWorks.WorkSourceID equals CW.ID For displaying assets.
                                  join TWC in context.TM_TenderWorksContractors on TenderWorks.ID equals TWC.TenderWorksID
                                  where TN.ID == _TenderNoticeID && TenderWorks.WorkStatusID == (long)Constants.WorkStatus.Sold
                                  select new
                                  {
                                      ID = TenderWorks.ID,
                                      Name = (TenderWorks.WorkSource == ClosureWork ?
                                                                    (from cw in context.CW_ClosureWork where cw.ID == TenderWorks.WorkSourceID select cw.WorkName).FirstOrDefault() :
                                                                    (from aw in context.AM_AssetWork where aw.ID == TenderWorks.WorkSourceID select aw.WorkName).FirstOrDefault())
                                      //Name = CW.WorkName
                                  }).Distinct().ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTenderWorks;
        }

        public List<object> GetClosureWorksByWorkSourceAndDivisionID(string _WorkSource, long _DivisionID)
        {
            List<object> lstClosureWorks = new List<object>();

            try
            {
                if (_WorkSource.ToUpper() == "CLOSURE")
                {
                    lstClosureWorks = (from ClosureWorks in context.CW_ClosureWork
                                       join CWP in context.CW_ClosureWorkPlan on ClosureWorks.CWPID equals CWP.ID
                                       join TW in context.TM_TenderWorks on ClosureWorks.ID equals TW.WorkSourceID
                                       where CWP.DivisionID == _DivisionID && CWP.Status.ToUpper() == "PUBLISH" && TW.WorkStatusID == (long)Constants.WorkStatus.Awarded
                                       select new
                                       {
                                           ID = ClosureWorks.ID,
                                           Name = ClosureWorks.WorkName,
                                           CWPID = CWP.ID,
                                           TID = TW.ID
                                       }).Distinct().ToList<object>();
                }
                else
                {

                    lstClosureWorks = (from AWorks in context.AM_AssetWork
                                       join CType in context.AM_AssetWorkType on AWorks.AssetWorkTypeID equals CType.ID
                                       //join TenderWorks in context.TM_TenderWorks on AWorks.ID equals TenderWorks.WorkSourceID into tW1
                                       //from tw2 in tW1.DefaultIfEmpty()
                                       join TW in context.TM_TenderWorks on AWorks.ID equals TW.WorkSourceID
                                       where AWorks.DivisionID == _DivisionID && AWorks.WorkStatus == "Contract Awarded" && TW.WorkStatusID == (long)Constants.WorkStatus.Awarded
                                       select new
                                       {
                                           ID = AWorks.ID,
                                           Name = AWorks.WorkName,
                                           CWPID = 0,
                                           TID = TW.ID
                                       }).Distinct().ToList<dynamic>();
                }
                //var query = from ClosureWorks in context.CW_ClosureWork
                //            join CWP in context.CW_ClosureWorkPlan on ClosureWorks.CWPID equals CWP.ID
                //            join TW in context.TM_TenderWorks on ClosureWorks.ID equals TW.WorkSourceID
                //            where CWP.DivisionID == _DivisionID && CWP.Status.ToUpper() == "PUBLISH" && TW.WorkStatusID == (long)Constants.WorkStatus.Awarded
                //            select new
                //            {
                //                ID = ClosureWorks.ID,
                //                Name = ClosureWorks.WorkName
                //            };


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstClosureWorks;
        }
        public List<object> GetScheduleDetailForTenderMonitoring(long _ScheduleID)
        {
            List<object> lstTenders = new List<object>();
            try
            {
                string ClosureWork = Constants.WorkType.CLOSURE.ToString();

                lstTenders = (from STM in context.SI_ScheduleDetailTender
                              join Division in context.CO_Division on STM.DivisionID equals Division.ID
                              join TN in context.TM_TenderNotice on STM.TenderNoticeID equals TN.ID
                              join TW in context.TM_TenderWorks on STM.TenderWorksID equals TW.ID
                              //join CW in context.CW_ClosureWork on TW.WorkSourceID equals CW.ID
                              where STM.ScheduleID == _ScheduleID
                              orderby STM.ID descending
                              select new
                              {
                                  ScheduleDetailID = STM.ID,
                                  DivisionName = Division.Name,
                                  DivisionID = Division.ID,
                                  TenderNoticeID = TN.ID,
                                  TenderWorksID = TW.ID,
                                  TenderNoticeName = TN.Name,
                                  //WorkName = CW.WorkName,
                                  WorkName = (TW.WorkSource == ClosureWork ?
                                                                    (from cw in context.CW_ClosureWork where cw.ID == TW.WorkSourceID select cw.WorkName).FirstOrDefault() :
                                                                    (from aw in context.AM_AssetWork where aw.ID == TW.WorkSourceID select aw.WorkName).FirstOrDefault()),
                                  TenderOpeningDate = TN.BidOpeningDate,
                                  WorkSourceID = TW.WorkSourceID,
                                  Remarks = STM.Remarks

                              }).ToList()
                              .Select(u => new
                              {
                                  ScheduleDetailID = u.ScheduleDetailID,
                                  DivisionName = u.DivisionName,
                                  DivisionID = u.DivisionID,
                                  TenderNoticeID = u.TenderNoticeID,
                                  TenderWorksID = u.TenderWorksID,
                                  TenderNoticeName = u.TenderNoticeName,
                                  WorkName = u.WorkName,
                                  TenderOpeningDate = Utility.GetFormattedDate(u.TenderOpeningDate),
                                  Remarks = u.Remarks,
                                  TenderOpeningDateTime = u.TenderOpeningDate,
                                  WorkSourceID = u.WorkSourceID
                              }).Distinct().ToList<object>();


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstTenders;
        }

        public List<object> GetScheduleDetailForClosureOperations(long _ScheduleID)
        {
            List<object> lstClosures = new List<object>();
            try
            {
                lstClosures = (from STW in context.SI_ScheduleDetailWorks
                               join Division in context.CO_Division on STW.DivisionID equals Division.ID
                               join CW in context.CW_ClosureWork on STW.WorkSourceID equals CW.ID
                               where STW.ScheduleID == _ScheduleID
                               orderby STW.ID descending
                               select new
                               {
                                   ScheduleDetailID = STW.ID,
                                   DivisionName = Division.Name,
                                   DivisionID = Division.ID,
                                   WorkSource = STW.WorkSource,
                                   WorkName = CW.WorkName,
                                   MonitoringDate = STW.MonitoringDate,
                                   WorkSourceID = STW.WorkSourceID,
                                   Remarks = STW.Remarks,
                                   CWPID = CW.CWPID,
                                   RefMonitoringID = STW.RefMonitoringID

                               }).ToList()
                              .Select(u => new
                              {
                                  ScheduleDetailID = u.ScheduleDetailID,
                                  DivisionName = u.DivisionName,
                                  DivisionID = u.DivisionID,
                                  WorkSource = u.WorkSource,
                                  WorkName = u.WorkName,
                                  MonitoringDate = Utility.GetFormattedDate(u.MonitoringDate),
                                  WorkSourceID = u.WorkSourceID,
                                  Remarks = u.Remarks,
                                  CWPID = u.CWPID,
                                  RefMonitoringID = u.RefMonitoringID
                              }).Distinct().ToList<object>();



                List<object> lstWorks = (from STW in context.SI_ScheduleDetailWorks
                                         join Division in context.CO_Division on STW.DivisionID equals Division.ID
                                         join CW in context.AM_AssetWork on STW.WorkSourceID equals CW.ID
                                         where STW.ScheduleID == _ScheduleID
                                         orderby STW.ID descending
                                         select new
                                         {
                                             ScheduleDetailID = STW.ID,
                                             DivisionName = Division.Name,
                                             DivisionID = Division.ID,
                                             WorkSource = STW.WorkSource,
                                             WorkName = CW.WorkName,
                                             MonitoringDate = STW.MonitoringDate,
                                             WorkSourceID = STW.WorkSourceID,
                                             Remarks = STW.Remarks,
                                             CWPID = 0,
                                             RefMonitoringID = STW.RefMonitoringID

                                         }).ToList()
                              .Select(u => new
                              {
                                  ScheduleDetailID = u.ScheduleDetailID,
                                  DivisionName = u.DivisionName,
                                  DivisionID = u.DivisionID,
                                  WorkSource = u.WorkSource,
                                  WorkName = u.WorkName,
                                  MonitoringDate = Utility.GetFormattedDate(u.MonitoringDate),
                                  WorkSourceID = u.WorkSourceID,
                                  Remarks = u.Remarks,
                                  CWPID = u.CWPID,
                                  RefMonitoringID = u.RefMonitoringID
                              }).Distinct().ToList<object>();

                lstClosures.AddRange(lstWorks);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstClosures;
        }

        public List<object> GetScheduleDetailForGeneralInspections(long _ScheduleID)
        {
            List<object> lstGeneralInspections = new List<object>();
            try
            {
                lstGeneralInspections = (from STW in context.SI_ScheduleDetailGeneral
                                         join IT in context.SI_GeneralInspectionType on STW.GeneralInspectionTypeID equals IT.ID
                                         join SD in context.SI_GeneralInspections on STW.ID equals SD.ScheduleDetailGeneralID into SD1
                                         from SD2 in SD1.DefaultIfEmpty()
                                         where STW.ScheduleID == _ScheduleID
                                         orderby STW.ID descending
                                         select new
                                         {
                                             ScheduleDetailID = STW.ID,
                                             ScheduleID = STW.ScheduleID,
                                             GeneralInspectionTypeID = STW.GeneralInspectionTypeID,
                                             Location = STW.Location,
                                             ScheduleDate = STW.ScheduleDate,
                                             Remarks = STW.Remarks,
                                             GeneralInspectionType = IT.Name,
                                             IsInspected = SD2 == null ? false : true

                                         }).ToList()
                              .Select(u => new
                              {
                                  ScheduleDetailID = u.ScheduleDetailID,
                                  ScheduleID = u.ScheduleID,
                                  GeneralInspectionTypeID = u.GeneralInspectionTypeID,
                                  Location = u.Location,
                                  GeneralInspectionType = u.GeneralInspectionType,
                                  ScheduleDate = Utility.GetFormattedDate(u.ScheduleDate),
                                  Remarks = u.Remarks,
                                  IsInspected = u.IsInspected

                              }).Distinct().ToList<object>();


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstGeneralInspections;
        }
        #endregion


        public List<object> GetGeneralInspectionAttachment(long _InspectionID)
        {
            List<object> lstAttachment = (from TA in context.SI_GeneralInspectionsAttachment

                                          where TA.GeneralInspectionsID == _InspectionID
                                          select new
                                          {
                                              ID = TA.ID,
                                              GeneralInspectionsID = TA.GeneralInspectionsID,
                                              FileName = TA.Attachment

                                          })
                                                  .AsEnumerable().Select
                                                    (q => new
                                                    {
                                                        ID = q.ID,
                                                        GeneralInspectionsID = q.GeneralInspectionsID,
                                                        FileName = Utility.GetImageURL(Configuration.ScheduleInspection, q.FileName)
                                                    }).ToList<object>();
            return lstAttachment;

        }
        public SI_GetDischargeTableCalcBLNotifyDataAndroid_Result GetDischargeTableCalcBLNotifyDataAndroid(long? _GaugeID, string _CalcType)
        {
            SI_GetDischargeTableCalcBLNotifyDataAndroid_Result lstDischargeTableCalcBLNotifyData = context.SI_GetDischargeTableCalcBLNotifyDataAndroid(_GaugeID, _CalcType).FirstOrDefault<SI_GetDischargeTableCalcBLNotifyDataAndroid_Result>();

            return lstDischargeTableCalcBLNotifyData;
        }

        public List<CO_Zone> GetUserZones(List<long> _lstZones)
        {
            List<CO_Zone> lstZone = (from zone in context.CO_Zone
                                     where _lstZones.Contains(zone.ID)
                                     select zone).ToList();
            return lstZone;
        }

        public List<CO_Circle> GetUserCircles(List<long> _lstCircles)
        {
            List<CO_Circle> lstZone = (from zone in context.CO_Circle
                                       where _lstCircles.Contains(zone.ID)
                                       select zone).ToList();
            return lstZone;
        }

        public List<CO_Division> GetUserDivisions(List<long> _lstDivisions)
        {
            List<CO_Division> lstZone = (from zone in context.CO_Division
                                         where _lstDivisions.Contains(zone.ID)
                                         select zone).ToList();
            return lstZone;
        }

        public List<CO_SubDivision> GetUserSubDivisions(List<long> _lstSubDivisions)
        {
            List<CO_SubDivision> lstZone = (from zone in context.CO_SubDivision
                                            where _lstSubDivisions.Contains(zone.ID)
                                            select zone).ToList();
            return lstZone;
        }


        public List<object> GetOutletCheckingAttachment(long _OCID)
        {
            List<object> lstAttachment = (from TA in context.WT_WaterTheftAttachments

                                          where TA.WaterTheftID == _OCID
                                          select new
                                          {
                                              ID = TA.ID,
                                              TenderWorksID = TA.WaterTheftID,
                                              FileName = TA.FileName,
                                              FileType = TA.FileType

                                          })
                                                  .AsEnumerable().Select
                                                    (q => new
                                                    {
                                                        ID = q.ID,
                                                        TenderWorksID = q.TenderWorksID,
                                                        FileName = Utility.GetImageURL(Configuration.WaterTheft, q.FileName),
                                                        FileType = q.FileType
                                                    }).ToList<object>();
            return lstAttachment;

        }
        public object GeOutletCheckingByScheduleID(long? _ScheduleChnlDetailID, long _OutletID)
        {
            object ParentInfo = new object();
            try
            {
                ParentInfo = (from soc in context.SI_OutletChecking
                              join outlet in context.CO_ChannelOutlets on soc.OutletID equals outlet.ID
                              join channel in context.CO_Channel on outlet.ChannelID equals channel.ID
                              where soc.OutletID == _OutletID && soc.ScheduleDetailChannelID == _ScheduleChnlDetailID
                              select new
                              {
                                  ChannelID = channel.ID,
                                  ChannelRD = outlet.OutletRD,
                                  ChannelSide = outlet.ChannelSide,
                                  OutletID = outlet.ID,
                                  RD = outlet.OutletRD,
                                  Side = outlet.ChannelSide,
                                  ConditionOfOutlet = soc.OutletCheckCondition,
                                  H = soc.HValue,
                                  Remarks = soc.Remarks,
                                  FileName = soc.Attachment
                              })
                              .ToList()
                              .Select(u => new
                              {
                                  ChannelName = GetChannelName(u.ChannelID),
                                  Outlet = GetOutletside(u.ChannelRD, u.ChannelSide),
                                  OutletType = GetOtletType(u.OutletID),
                                  RD = Calculations.GetRDText(u.RD),
                                  Side = GetChannelSide(u.Side),
                                  u.ConditionOfOutlet,
                                  u.H,
                                  u.Remarks,
                                  FileName = Utility.GetImageURL(Configuration.WaterTheft, u.FileName),

                              })
                            .FirstOrDefault();
            }
            catch (Exception e)
            {
                new WRException(Constants.UserID, e).LogException(Constants.MessageCategory.WebApp);
            }
            return ParentInfo;

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

        internal List<object> GetOutletsByUserID(long _UserID)
        {
            ContextDB dbADO = new ContextDB();
            DataTable dt = dbADO.ExecuteStoredProcedureDataTable("GetChannelSectionOutlet", _UserID);
            List<object> lstObject = new List<object>();
            foreach (DataRow DR in dt.Rows)
            {
                lstObject.Add(
                    new
                    {
                        ID = Convert.ToInt64(DR["OutletID"]),
                        Name = Convert.ToString(DR["OutletRD"] == DBNull.Value ? "" : DR["OutletRD"]).Length > 4 ?
                              Convert.ToString(DR["OutletRD"] == DBNull.Value ? "" : DR["OutletRD"]).Substring(0, 2) + "+" + Convert.ToString(DR["OutletRD"] == DBNull.Value ? "" : DR["OutletRD"]).Substring(2) + "/" + Convert.ToString(DR["ChannelSide"] == DBNull.Value ? "" : DR["ChannelSide"]) :
                              Convert.ToString(DR["OutletRD"] == DBNull.Value ? "" : DR["OutletRD"]).Substring(0, 1) + "+" + Convert.ToString(DR["OutletRD"] == DBNull.Value ? "" : DR["OutletRD"]).Substring(1) + "/" + Convert.ToString(DR["ChannelSide"] == DBNull.Value ? "" : DR["ChannelSide"])
                    }
                    );
            }
            return lstObject;
        }
    }
}
