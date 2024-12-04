using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PMIU.WRMIS.DAL.Repositories.RotationalProgram
{
    class RotationalProgramRepository : Repository<RP_RotationalProgram>
    {
        public RotationalProgramRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<RP_RotationalProgram>();

        }

        #region Search Plan

        public List<object> GetYears()
        {
            List<object> lstYears = new List<object>();
            try
            {
                lstYears = (from RP in context.RP_RotationalProgram
                            select new
                            {
                                Year = RP.RPYear
                            }).Distinct().ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstYears;
        }

        public string GetSeasonName(long _SeasonID)
        {
            string Season = "Rabi";
            if (_SeasonID == (int)Constants.Seasons.Kharif)
                Season = "Kharif";
            return Season;
        }

        public bool SetVisibility(long _IrrigationLevelID, long _UserIrrigationLevelID, long _RPID)
        {
            bool Result = false;

            if (_IrrigationLevelID == _UserIrrigationLevelID)
                Result = true;

            RP_Approval objApproval = ShowhideApprovalButtons(_RPID);
            if (objApproval != null)
                if (objApproval.Status == Constants.RP_Approved || objApproval.Status == Constants.RP_SendToSE)
                    Result = false;

            return Result;
        }

        public string GetStatus(long _RPID)
        {
            String Status = "";
            try
            {
                RP_Approval objApproval = ShowhideApprovalButtons(_RPID);
                if (objApproval != null)
                    Status = objApproval.Status.ToString();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Status;
        }

        public bool SetGiniVisibility(long _RPID, long _IrrLevelID)
        {
            bool Visibility = false;
            try
            {
                //if (_IrrLevelID == (int)Constants.IrrigationLevelID.Circle)
                //{
                //    List<RP_Rotation_Circle> lstResult = (from DSD in context.RP_Rotation_Circle
                //                                          where DSD.RPID == _RPID
                //                                          orderby DSD.EndDate ascending
                //                                          select DSD).ToList();
                //    if (lstResult.Count() > 0)
                //    {
                //        DateTime dt = lstResult.Last().EndDate.Value;
                //        if (dt.Date < DateTime.Now)
                //            Visibility = true;
                //    }
                //}
                //else
                if (_IrrLevelID != (int)Constants.IrrigationLevelID.Circle)
                {
                    List<RP_Rotation_DivisionSubDivision> lstResult = (from DSD in context.RP_Rotation_DivisionSubDivision
                                                                       where DSD.RPID == _RPID
                                                                       orderby DSD.EndDate ascending
                                                                       select DSD).ToList();
                    if (lstResult.Count() > 0)
                    {
                        DateTime dt = lstResult.Last().EndDate.Value;
                        if (dt.Date < DateTime.Now)
                            Visibility = true;
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Visibility;
        }

        public List<dynamic> SearchRotationalPlan(long _CircleID, long _DivisionID, long _SubDivisionID, string _Year, long _SeasonID, long _UserIrrigationLevelID)
        {
            List<dynamic> lstResult = new List<dynamic>();
            try
            {
                lstResult = (from RP in context.RP_RotationalProgram
                             join PIP in context.RP_PlanImplementationPercentage on RP.ID equals PIP.RPID into A
                             from a in A.DefaultIfEmpty()
                             where
                                ((RP.IrrigationLevelID == (int)Constants.IrrigationLevelID.Circle && (RP.IrrigationBoundaryID == _CircleID || RP.IrrigationBoundaryID == -1))
                                || (RP.IrrigationLevelID == (int)Constants.IrrigationLevelID.Division && (RP.IrrigationBoundaryID == _DivisionID || RP.IrrigationBoundaryID == -1))
                                || (RP.IrrigationLevelID == (int)Constants.IrrigationLevelID.SubDivision && (RP.IrrigationBoundaryID == _SubDivisionID || RP.IrrigationBoundaryID == -1)))
                                && (RP.SeasonID == _SeasonID || _SeasonID == -1)
                                && (RP.RPYear == _Year || _Year == "")

                             select new
                             {
                                 ID = RP.ID,
                                 Name = RP.Name,
                                 Season = RP.SeasonID,
                                 RPYear = RP.RPYear,
                                 IrrigationLevelID = RP.IrrigationLevelID,
                                 FromDate = a.FromDate,
                                 ToDate = a.ToDate,
                                 Percentage = a.PIPercentage,
                                 Gini = RP.Gini
                             }).ToList<dynamic>()
                             .Select(q =>
                             new
                             {
                                 ID = q.ID,
                                 Name = q.Name,
                                 Season = GetSeasonName(q.Season),
                                 Year = q.RPYear,
                                 Visibility = SetVisibility(q.IrrigationLevelID, _UserIrrigationLevelID, q.ID),
                                 VisibilitySE = q.IrrigationLevelID == (int)Constants.IrrigationLevelID.Circle ? false : true,
                                 FromDate = q.FromDate,
                                 ToDate = q.ToDate,
                                 Date = q.FromDate == null ? "-" : (Utility.FormattedDate(q.FromDate) + " to " + Utility.FormattedDate(q.ToDate)),
                                 Percentage = q.Percentage == null ? "-" : q.Percentage,
                                 Status = GetStatus(q.ID),
                                 Gini = q.Gini == null ? "-" : q.Gini,
                                 VisibilityGini = SetGiniVisibility(q.ID, q.IrrigationLevelID)
                             }).ToList<dynamic>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstResult;
        }

        public long DeleteProgram(long _RPID)
        {
            long Result = -1;
            try
            {
                RP_RotationalProgram objDraft = (from RP in context.RP_RotationalProgram
                                                 where RP.ID == _RPID
                                                 select RP).FirstOrDefault();

                if (objDraft.IsApproved == false)
                {
                    if (objDraft.IrrigationLevelID == (int)Constants.IrrigationLevelID.Circle)
                    {
                        context.RP_Rotation_Circle.RemoveRange(context.RP_Rotation_Circle.Where(q => q.RPID == _RPID));
                        context.RP_Division.RemoveRange(context.RP_Division.Where(q => q.RPID == _RPID));
                    }
                    else
                    {
                        context.RP_Rotation_DivisionSubDivision.RemoveRange(context.RP_Rotation_DivisionSubDivision.Where(q => q.RPID == _RPID));
                        context.RP_Channel.RemoveRange(context.RP_Channel.Where(q => q.RPID == _RPID));

                    }
                    context.RP_Attachment.RemoveRange(context.RP_Attachment.Where(q => q.RPID == _RPID));
                    context.RP_Approval.RemoveRange(context.RP_Approval.Where(q => q.RPID == _RPID));
                    context.RP_EnteringChannel.RemoveRange(context.RP_EnteringChannel.Where(q => q.RPID == _RPID));
                    context.RP_RotationalProgram.RemoveRange(context.RP_RotationalProgram.Where(q => q.ID == _RPID));
                    context.SaveChanges();
                    Result = 1;
                }
                else
                    Result = 0; // means draft is approve cannot delete it 
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

        public string GetLevelName(long _ID)
        {
            string Name = "Sub Division";

            if (_ID == (int)Constants.IrrigationLevelID.Circle)
                Name = "Circle";
            else if (_ID == (int)Constants.IrrigationLevelID.Division)
                Name = "Division";
            return Name;
        }

        public string GetBoundryName(long _LevelID, long _BoundryID)
        {
            string BoundryName = "";
            try
            {
                if (_LevelID == (int)Constants.IrrigationLevelID.Circle)
                {
                    CircleDAL dalCircle = new CircleDAL();
                    BoundryName = dalCircle.GetByID(_BoundryID).Name;
                }
                else if (_LevelID == (int)Constants.IrrigationLevelID.Division)
                {
                    DivisionDAL dalDiv = new DivisionDAL();
                    BoundryName = dalDiv.GetByID(_BoundryID).Name;
                }
                else
                {
                    SubDivisionDAL dalSubDiv = new SubDivisionDAL();
                    BoundryName = dalSubDiv.GetByID(_BoundryID).Name;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return BoundryName;
        }

        public object GetBasicDetail(long _ID)
        {
            object objInfo = new object();
            try
            {
                objInfo = (from RP in context.RP_RotationalProgram
                           where RP.ID == _ID
                           select RP).ToList().
                           Select(q =>
                               new
                               {
                                   PlanName = q.Name,
                                   LevelName = GetLevelName(q.IrrigationLevelID),
                                   BoundryID = q.IrrigationBoundaryID,
                                   BoundryName = GetBoundryName(q.IrrigationLevelID, q.IrrigationBoundaryID),
                                   Season = GetSeasonName(q.SeasonID),
                                   StartDate = Utility.GetFormattedDate(q.ProgramStartDate),
                                   Year = q.RPYear,
                                   Groups = q.NoOfGroup,
                                   SubGroups = q.NoOfSubGroup,
                                   ClosureStartDate = Utility.GetFormattedDate(q.ClosureStartDate),
                                   ClosureEndDate = Utility.GetFormattedDate(q.ClosureEndDate)
                               }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objInfo;
        }

        public List<object> GetDivisionsAgainstChannelID(long _CircleID, string _Group)
        {
            List<object> lstDivisions = new List<object>();
            try
            {
                lstDivisions = (from Div in context.CO_Division
                                where Div.CircleID == _CircleID
                                select new
                                {
                                    ID = Div.ID,
                                    Name = Div.Name
                                }).ToList().
                                Select(q =>
                                    new
                                    {
                                        ID = _Group + q.ID.ToString(),
                                        Name = q.Name
                                    }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstDivisions;
        }

        public List<long?> GetSectionListByDivisionID(long _DivisionID)
        {
            List<long?> lstSections = new List<long?>();
            List<long?> qLst = new List<long?>();
            List<long> _lst = new List<long>();

            List<long> SubDivisions = (from Subdiv in context.CO_SubDivision
                                       where Subdiv.DivisionID == _DivisionID
                                       select Subdiv.ID).ToList<long>();

            foreach (var v in SubDivisions)
            {
                qLst.Add((long?)v);
            }

            _lst = (from Sec in context.CO_Section
                    where qLst.Contains(Sec.SubDivID)
                    select Sec.ID).ToList<long>();

            foreach (var v in _lst)
            {
                lstSections.Add((long?)v);
            }
            return lstSections;
        }

        public List<long?> GetSectionListBySubDivisionID(long _SubDivisionID)
        {
            List<long?> lstSections = new List<long?>();
            try
            {
                lstSections = (from Sec in context.CO_Section
                               where Sec.SubDivID == _SubDivisionID
                               select (long?)Sec.ID).ToList<long?>();
                //foreach (var v in lst)                
                //    lstSections.Add((long?)v);                
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstSections;
        }

        public List<long> RecursiveRemoveBranchCanals(List<long?> _lstChannelID, List<long> _RetLstChannelID, List<long?> _lstSectionIDs)  //function takes Canals and return direct offtakes of canals.
        {
            double StartRD = 0;
            double EndRD = 0;
            List<long?> lstCIBIDs = new List<long?>();
            List<long?> lstChnlPrntFdrID = new List<long?>();
            List<long?> lstChnlIDs = new List<long?>();
            try
            {
                //List<CO_ChannelGauge> lstChnlGauges = (from CG in context.CO_ChannelGauge
                //                                       where _lstChannelID.Contains(CG.ChannelID)
                //                                       && _lstSectionIDs.Contains(CG.SectionID)
                //                                       orderby CG.ChannelID
                //                                       select CG).ToList();
                //foreach (var v in lstChnlGauges)
                //{
                //    //StartRD = v.GaugeAtRD;  old
                //    List<CO_ChannelIrrigationBoundaries> lst = (from CIB in context.CO_ChannelIrrigationBoundaries
                //                                                where _lstSectionIDs.Contains(CIB.SectionID) && CIB.ChannelID == v.ChannelID
                //                                                select CIB).ToList();
                //    if (lst.Count() > 0)
                //    {
                //        StartRD = Convert.ToDouble(lst.OrderBy(x => x.SectionToRD).FirstOrDefault().SectionRD); // new 
                //        EndRD = Convert.ToDouble(lst.OrderByDescending(x => x.SectionToRD).FirstOrDefault().SectionToRD);

                //        lstChnlPrntFdrID = (from CPF in context.CO_ChannelParentFeeder
                //                            where CPF.ParrentFeederID == v.ChannelID && (CPF.ParrentFeederRDS >= StartRD && CPF.ParrentFeederRDS <= EndRD)
                //                            select CPF.ChannelID).ToList<long?>();
                //        lstCIBIDs.AddRange(lstChnlPrntFdrID);
                //        //
                //        lstChnlIDs.Add(v.ChannelID);
                //        lstChnlIDs.AddRange(lstChnlPrntFdrID);
                //        //
                //    }
                //}

                List<long?> lstChnlGauges = (from CG in context.CO_ChannelGauge
                                             where _lstChannelID.Contains(CG.ChannelID)
                                             && _lstSectionIDs.Contains(CG.SectionID)
                                             //orderby CG.ChannelID
                                             select CG.ChannelID).Distinct().ToList<long?>();
                foreach (long v in lstChnlGauges)
                {
                    //StartRD = v.GaugeAtRD;  old
                    List<CO_ChannelIrrigationBoundaries> lst = (from CIB in context.CO_ChannelIrrigationBoundaries
                                                                where _lstSectionIDs.Contains(CIB.SectionID) && CIB.ChannelID == v
                                                                select CIB).ToList();
                    if (lst.Count() > 0)
                    {
                        StartRD = Convert.ToDouble(lst.OrderBy(x => x.SectionToRD).FirstOrDefault().SectionRD); // new 
                        EndRD = Convert.ToDouble(lst.OrderByDescending(x => x.SectionToRD).FirstOrDefault().SectionToRD);

                        lstChnlPrntFdrID = (from CPF in context.CO_ChannelParentFeeder
                                            where CPF.ParrentFeederID == v && (CPF.ParrentFeederRDS >= StartRD && CPF.ParrentFeederRDS <= EndRD)
                                            select CPF.ChannelID).ToList<long?>();
                        lstCIBIDs.AddRange(lstChnlPrntFdrID);
                        lstChnlIDs.Add(v);
                        lstChnlIDs.AddRange(lstChnlPrntFdrID);
                    }
                }


                lstChnlIDs = lstChnlIDs.Distinct().ToList();

                List<CO_Channel> lstTestChannels = (from Chnl in context.CO_Channel
                                                    where lstChnlIDs.Contains(Chnl.ID)
                                                    select Chnl).ToList();


                List<CO_Channel> lstChannels = (from Chnl in lstTestChannels
                                                where lstCIBIDs.Contains(Chnl.ID)
                                                select Chnl).ToList();

                List<long?> lstBranchCanalIDs = lstChannels.Where(q => q.ChannelTypeID == (int)Constants.ChannelType.BranchCanal || q.ChannelTypeID == (int)Constants.ChannelType.MainCanal).Select(w => (long?)w.ID).ToList<long?>();
                //
                _RetLstChannelID.AddRange(lstTestChannels.Select(q => q.ID));
                //

                if (lstBranchCanalIDs.Count() > 0)
                {
                    foreach (long l in lstBranchCanalIDs)
                        _RetLstChannelID.Remove(l);

                    RecursiveRemoveBranchCanals(lstBranchCanalIDs, _RetLstChannelID, _lstSectionIDs);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return _RetLstChannelID;
        }

        public List<long> RemoveBranchCanals(List<CO_ChannelGauge> _lstChannelID, List<long?> _lstSectionIDs)  //function takes Canals and return direct offtakes of canals.
        {
            double StartRD;
            double EndRD;
            List<long?> lstCIBIDs = new List<long?>();
            List<long?> lstChnlPrntFdrID = new List<long?>();

            List<long?> lstChnlIDs = new List<long?>();
            List<long> lstRetChnlIDs = new List<long>();
            try
            {
                foreach (var v in _lstChannelID)
                {
                    StartRD = v.GaugeAtRD;

                    List<CO_ChannelIrrigationBoundaries> lst = (from CIB in context.CO_ChannelIrrigationBoundaries
                                                                where _lstSectionIDs.Contains(CIB.SectionID) && CIB.ChannelID == v.ChannelID
                                                                select CIB).ToList();

                    if (lst.Count() > 0)
                    {
                        EndRD = Convert.ToDouble(lst.OrderByDescending(x => x.SectionToRD).FirstOrDefault().SectionToRD);

                        lstChnlPrntFdrID = (from CPF in context.CO_ChannelParentFeeder
                                            where CPF.ParrentFeederID == v.ChannelID && (CPF.ParrentFeederRDS >= StartRD && CPF.ParrentFeederRDS <= EndRD)
                                            select CPF.ChannelID).ToList<long?>();
                        lstCIBIDs.AddRange(lstChnlPrntFdrID);

                        //
                        lstChnlIDs.Add(v.ChannelID);
                        lstChnlIDs.AddRange(lstChnlPrntFdrID);
                        //
                    }
                }

                lstChnlIDs = lstChnlIDs.Distinct().ToList();

                List<CO_Channel> lstChannelsTest = (from Chnl in context.CO_Channel
                                                    where lstChnlIDs.Contains(Chnl.ID)
                                                    select Chnl).ToList();

                List<CO_Channel> lstChannels = (from Chnl in lstChannelsTest
                                                where lstCIBIDs.Contains(Chnl.ID)
                                                select Chnl).ToList();

                // _RetLstChannelID.AddRange(lstChannels.Where(q => q.ChannelTypeID != (int)Constants.ChannelType.BranchCanal && q.ChannelTypeID != (int)Constants.ChannelType.MainCanal).Select(w => w.ID).ToList<long>());

                List<long?> lstBranchCanalIDs = lstChannels.Where(q => q.ChannelTypeID == (int)Constants.ChannelType.BranchCanal || q.ChannelTypeID == (int)Constants.ChannelType.MainCanal).Select(w => (long?)w.ID).ToList<long?>();

                lstRetChnlIDs.AddRange(lstChannelsTest.Select(w => w.ID).ToList<long>());

                if (lstBranchCanalIDs.Count() > 0)
                {
                    foreach (long l in lstBranchCanalIDs)
                        lstRetChnlIDs.Remove(l);

                    // RecursiveRemoveBranchCanals(lstBranchCanalIDs, _RetLstChannelID, _lstSectionIDs);
                    RecursiveRemoveBranchCanals(lstBranchCanalIDs, lstRetChnlIDs, _lstSectionIDs);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            // return _RetLstChannelID;
            return lstRetChnlIDs;
        }

        public List<long> RecursiveRemoveBranchCanalsForSubDivision(List<long?> _lstChannelID, List<long?> _lstSectionID, List<long> _RetLstChannelID)  //function takes Canals and return direct offtakes of canals.
        {
            double StartRD = 0;
            double EndRD = 0;
            List<long?> lstCIBIDs = new List<long?>();
            List<long?> lstChnlPrntFdrID = new List<long?>();
            List<long?> lstChnlIDs = new List<long?>();
            try
            {
                List<CO_ChannelGauge> lstChnlGauges = (from CG in context.CO_ChannelGauge
                                                       where _lstChannelID.Contains(CG.ChannelID)
                                                       && _lstSectionID.Contains(CG.SectionID)
                                                       orderby CG.ChannelID
                                                       select CG).ToList();

                foreach (var v in lstChnlGauges)
                {
                    StartRD = v.GaugeAtRD;
                    List<CO_ChannelIrrigationBoundaries> lst = (from CIB in context.CO_ChannelIrrigationBoundaries
                                                                where _lstSectionID.Contains(CIB.SectionID) && CIB.ChannelID == v.ChannelID
                                                                select CIB).ToList();
                    if (lst.Count() > 0)
                    {
                        EndRD = Convert.ToDouble(lst.OrderByDescending(x => x.SectionToRD).FirstOrDefault().SectionToRD);

                        lstChnlPrntFdrID = (from CPF in context.CO_ChannelParentFeeder
                                            where CPF.ParrentFeederID == v.ChannelID && (CPF.ParrentFeederRDS >= StartRD && CPF.ParrentFeederRDS <= EndRD)
                                            select CPF.ChannelID).ToList<long?>();
                        lstCIBIDs.AddRange(lstChnlPrntFdrID);
                        //
                        lstChnlIDs.Add(v.ChannelID);
                        lstChnlIDs.AddRange(lstChnlPrntFdrID);
                        //
                    }
                }

                lstChnlIDs = lstChnlIDs.Distinct().ToList();

                List<CO_Channel> lstTestChannels = (from Chnl in context.CO_Channel
                                                    where lstChnlIDs.Contains(Chnl.ID)
                                                    select Chnl).ToList();


                List<CO_Channel> lstChannels = (from Chnl in lstTestChannels
                                                where lstCIBIDs.Contains(Chnl.ID)
                                                select Chnl).ToList();

                List<long?> lstBranchCanalIDs = lstChannels.Where(q => q.ChannelTypeID == (int)Constants.ChannelType.BranchCanal || q.ChannelTypeID == (int)Constants.ChannelType.MainCanal).Select(w => (long?)w.ID).ToList<long?>();
                //
                _RetLstChannelID.AddRange(lstTestChannels.Select(q => q.ID));
                //

                if (lstBranchCanalIDs.Count() > 0)
                {
                    foreach (long l in lstBranchCanalIDs)
                        _RetLstChannelID.Remove(l);

                    RecursiveRemoveBranchCanalsForSubDivision(lstBranchCanalIDs, _lstSectionID, _RetLstChannelID);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return _RetLstChannelID;
        }

        public List<long> RemoveBranchCanalsForSubDivision(List<CO_ChannelGauge> _lstChannelID, List<long?> _lstSectionID)  //function takes Canals and return direct offtakes of canals.
        {
            double StartRD;
            double EndRD;
            List<long?> lstCIBIDs = new List<long?>();
            List<long?> lstChnlPrntFdrID = new List<long?>();
            List<long?> lstChnlIDs = new List<long?>();
            List<long> lstRetChnlIDs = new List<long>();
            try
            {
                foreach (var v in _lstChannelID)
                {
                    StartRD = v.GaugeAtRD;

                    List<CO_ChannelIrrigationBoundaries> lst = (from CIB in context.CO_ChannelIrrigationBoundaries
                                                                where _lstSectionID.Contains(CIB.SectionID) && CIB.ChannelID == v.ChannelID
                                                                select CIB).ToList();

                    if (lst.Count() > 0)
                    {
                        EndRD = Convert.ToDouble(lst.OrderByDescending(x => x.SectionToRD).FirstOrDefault().SectionToRD);

                        lstChnlPrntFdrID = (from CPF in context.CO_ChannelParentFeeder
                                            where CPF.ParrentFeederID == v.ChannelID && (CPF.ParrentFeederRDS >= StartRD && CPF.ParrentFeederRDS <= EndRD)
                                            select CPF.ChannelID).ToList<long?>();
                        lstCIBIDs.AddRange(lstChnlPrntFdrID);
                        //
                        lstChnlIDs.Add(v.ChannelID);
                        lstChnlIDs.AddRange(lstChnlPrntFdrID);
                        //
                    }
                }
                lstChnlIDs = lstChnlIDs.Distinct().ToList();

                List<CO_Channel> lstChannelsTest = (from Chnl in context.CO_Channel
                                                    where lstChnlIDs.Contains(Chnl.ID)
                                                    select Chnl).ToList();

                List<CO_Channel> lstChannels = (from Chnl in lstChannelsTest
                                                where lstCIBIDs.Contains(Chnl.ID)
                                                select Chnl).ToList();

                // _RetLstChannelID.AddRange(lstChannels.Where(q => q.ChannelTypeID != (int)Constants.ChannelType.BranchCanal && q.ChannelTypeID != (int)Constants.ChannelType.MainCanal).Select(w => w.ID).ToList<long>());

                List<long?> lstBranchCanalIDs = lstChannels.Where(q => q.ChannelTypeID == (int)Constants.ChannelType.BranchCanal || q.ChannelTypeID == (int)Constants.ChannelType.MainCanal).Select(w => (long?)w.ID).ToList<long?>();

                lstRetChnlIDs.AddRange(lstChannelsTest.Select(w => w.ID).ToList<long>());

                if (lstBranchCanalIDs.Count() > 0)
                {
                    foreach (long l in lstBranchCanalIDs)
                        lstRetChnlIDs.Remove(l);

                    RecursiveRemoveBranchCanalsForSubDivision(lstBranchCanalIDs, _lstSectionID, lstRetChnlIDs);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstRetChnlIDs;
        }

        public List<object> GetEnteringChannelsAgainstDivision(long _DivisionSubDivisionID, long _Designation)
        {
            List<long?> lstChannelIDs = new List<long?>();
            List<object> lstChannels = new List<object>();
            List<long?> lstSections = new List<long?>();
            try
            {
                if (_Designation == (int)Constants.Designation.XEN)
                {
                    lstSections = GetSectionListByDivisionID(_DivisionSubDivisionID);
                    List<long?> lstChnlID = (from CIB in context.CO_ChannelIrrigationBoundaries
                                             where lstSections.Contains(CIB.SectionID)
                                             select CIB.ChannelID).Distinct().ToList<long?>();

                    lstChannelIDs = (from CG in context.CO_ChannelGauge
                                     where lstChnlID.Contains(CG.ChannelID) && CG.IsDivGauge == true
                                     select CG.ChannelID).Distinct().ToList();

                    lstChannelIDs.AddRange(GetMainAndBranchChannels(true, lstSections));
                }
                else
                {
                    lstSections = GetSectionListBySubDivisionID(_DivisionSubDivisionID);
                    lstChannelIDs = (from CG in context.CO_ChannelGauge
                                     where lstSections.Contains(CG.SectionID) && CG.GaugeCategoryID == (int)Constants.GaugeCategory.SubDivisionalGauge
                                     select CG.ChannelID).Distinct().ToList();

                    lstChannelIDs.AddRange(GetMainAndBranchChannels(false, lstSections));
                }

                lstChannels = (from Chnl in context.CO_Channel
                               join CG in context.CO_ChannelGauge on Chnl.ID equals CG.ChannelID
                               where lstChannelIDs.Contains(Chnl.ID) && CG.GaugeCategoryID == (int)Constants.GaugeCategory.HeadGauge
                               select new
                               {
                                   ID = Chnl.ID,
                                   Name = Chnl.NAME,
                                   DesignDischarge = CG.DesignDischarge
                               }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstChannels;
        }

        public List<long> GetChannelsWhoseParentNotInThisDivision(bool _FromDiv, List<long?> _lstSections, List<long?> _lstChnlID)
        {
            List<long> lstRetChnlIDs = new List<long>();
            List<long?> lstChnlIDs = new List<long?>();
            List<CO_ChannelGauge> lstGauges = new List<CO_ChannelGauge>();
            try
            {
                List<long?> _lstAllChnlID = (from CIB in context.CO_ChannelIrrigationBoundaries
                                             where _lstSections.Contains(CIB.SectionID)
                                             select CIB.ChannelID).Distinct().ToList<long?>();

                List<CO_ChannelParentFeeder> lstPF = (from CPF in context.CO_ChannelParentFeeder
                                                      where _lstAllChnlID.Contains(CPF.ChannelID)
                                                      select CPF).ToList();

                List<long?> lst12IDs = (from ls in lstPF
                                        where !_lstAllChnlID.Contains(ls.ParrentFeederID)
                                        select ls.ChannelID).ToList();
                if (_FromDiv)
                {
                    lstGauges = (from CG in context.CO_ChannelGauge
                                 where lst12IDs.Contains(CG.ChannelID)
                                 && _lstSections.Contains(CG.SectionID)
                                 select CG).ToList();

                    lstChnlIDs = (from LG in lstGauges
                                  where (LG.IsDivGauge == true)
                                  select LG.ChannelID).ToList();

                    lstChnlIDs = (from LG in lstGauges
                                  where !lstChnlIDs.Contains(LG.ChannelID) &&
                                  (LG.IsDivGauge == false || LG.IsDivGauge == null)
                                  select LG.ChannelID).Distinct().ToList();

                    List<CO_Channel> lstChannels = (from Chnl in context.CO_Channel
                                                    where lstChnlIDs.Contains(Chnl.ID)
                                                    select Chnl).ToList();


                    lstRetChnlIDs.AddRange(lstChannels.Where(q => q.ChannelTypeID != (int)Constants.ChannelType.BranchCanal && q.ChannelTypeID != (int)Constants.ChannelType.MainCanal).Select(w => w.ID).ToList<long>());

                    List<long?> lstBranchCanalIDs = lstChannels.Where(q => q.ChannelTypeID == (int)Constants.ChannelType.BranchCanal || q.ChannelTypeID == (int)Constants.ChannelType.MainCanal).Select(w => (long?)w.ID).ToList<long?>();

                    if (lstBranchCanalIDs.Count() > 0)
                    {
                        if (_lstChnlID.Count() > 0)
                            lstBranchCanalIDs = (from BCI in lstBranchCanalIDs
                                                 where _lstChnlID.Contains(BCI.Value)
                                                 select BCI).ToList();

                        RecursiveRemoveBranchCanals(lstBranchCanalIDs, lstRetChnlIDs, _lstSections);
                    }
                }
                else
                {
                    lstGauges = (from CG in context.CO_ChannelGauge
                                 where _lstSections.Contains(CG.SectionID)
                                 && lst12IDs.Contains(CG.ChannelID)
                                 select CG).ToList();

                    lstChnlIDs = (from LG in lstGauges
                                  where LG.IsSubDivGauge == true
                                  select LG.ChannelID).ToList();

                    lstChnlIDs = (from LG in lstGauges
                                  where !lstChnlIDs.Contains(LG.ChannelID) &&
                                  (LG.IsSubDivGauge == false || LG.IsSubDivGauge == null)
                                  select LG.ChannelID).Distinct().ToList();

                    List<CO_Channel> lstChannels = (from Chnl in context.CO_Channel
                                                    where lstChnlIDs.Contains(Chnl.ID)
                                                    select Chnl).ToList();

                    lstRetChnlIDs.AddRange(lstChannels.Where(q => q.ChannelTypeID != (int)Constants.ChannelType.BranchCanal && q.ChannelTypeID != (int)Constants.ChannelType.MainCanal).Select(w => w.ID).ToList<long>());

                    List<long?> lstBranchCanalIDs = lstChannels.Where(q => q.ChannelTypeID == (int)Constants.ChannelType.BranchCanal || q.ChannelTypeID == (int)Constants.ChannelType.MainCanal).Select(w => (long?)w.ID).ToList<long?>();

                    if (lstBranchCanalIDs.Count() > 0)
                    {
                        if (_lstChnlID.Count() > 0)
                            lstBranchCanalIDs = (from BCI in lstBranchCanalIDs
                                                 where _lstChnlID.Contains(BCI.Value)
                                                 select BCI).ToList();

                        RecursiveRemoveBranchCanalsForSubDivision(lstBranchCanalIDs, _lstSections, lstRetChnlIDs);
                    }
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstRetChnlIDs;
        }

        public List<object> GetChannelsAgainstDivision(long _DivisionSubDivisionID, long _Designation, List<long?> _lstChnlID, bool _PostBack)
        {
            List<long> lstChannelIDs = new List<long>();
            List<object> lstChannels = new List<object>();
            List<long?> lstSections = new List<long?>();
            List<CO_ChannelGauge> lstChnlGauges = new List<CO_ChannelGauge>();
            try
            {
                if (_Designation == (int)Constants.Designation.XEN || _Designation == (int)Constants.Designation.SE)
                {
                    lstSections = GetSectionListByDivisionID(_DivisionSubDivisionID);

                    if (!_PostBack && _lstChnlID.Count() == 0)
                        _lstChnlID = (from CIB in context.CO_ChannelIrrigationBoundaries
                                      where lstSections.Contains(CIB.SectionID)
                                      select CIB.ChannelID).Distinct().ToList<long?>();

                    lstChnlGauges = (from CG in context.CO_ChannelGauge
                                     where _lstChnlID.Contains(CG.ChannelID)
                                     && lstSections.Contains(CG.SectionID)
                                     && CG.IsDivGauge == true
                                     orderby CG.ChannelID
                                     select CG).ToList();

                    if (lstChnlGauges.Count() > 0)
                        lstChannelIDs = RemoveBranchCanals(lstChnlGauges, lstSections);

                    _lstChnlID = _lstChnlID.OrderBy(q => q.Value).ToList();

                    lstChannelIDs.AddRange(GetChannelsWhoseParentNotInThisDivision(true, lstSections, _lstChnlID)); // Chnls whose parent not in this division

                }
                else
                {
                    lstSections = GetSectionListBySubDivisionID(_DivisionSubDivisionID);

                    if (_DivisionSubDivisionID == 141) //SDO DharanWala Special Case
                    {
                        lstChannelIDs = (from CIB in context.CO_ChannelIrrigationBoundaries
                                         where lstSections.Contains(CIB.SectionID)
                                         select (long)CIB.ChannelID).ToList<long>();
                    }
                    else
                    {
                        if (!_PostBack && _lstChnlID.Count() == 0)
                        {
                            lstChnlGauges = (from CG in context.CO_ChannelGauge
                                             where lstSections.Contains(CG.SectionID) && CG.GaugeCategoryID == (int)Constants.GaugeCategory.SubDivisionalGauge
                                             select CG).ToList();

                            _lstChnlID = (from LCG in lstChnlGauges
                                          select LCG.ChannelID).ToList<long?>();
                        }
                        else
                            lstChnlGauges = (from CG in context.CO_ChannelGauge
                                             where lstSections.Contains(CG.SectionID)
                                             && _lstChnlID.Contains(CG.ChannelID)
                                             && CG.GaugeCategoryID == (int)Constants.GaugeCategory.SubDivisionalGauge
                                             select CG).ToList();

                        if (lstChnlGauges.Count() > 0)
                            lstChannelIDs = RemoveBranchCanalsForSubDivision(lstChnlGauges, lstSections);

                        _lstChnlID = _lstChnlID.OrderBy(q => q.Value).ToList();
                        lstChannelIDs.AddRange(GetChannelsWhoseParentNotInThisDivision(false, lstSections, _lstChnlID)); // Chnls whose parent not in this division                    
                    }
                }

                lstChannelIDs = (from ID in lstChannelIDs
                                 select ID).Distinct().ToList();

                lstChannels = (from Chnl in context.CO_Channel
                               join CG in context.CO_ChannelGauge on Chnl.ID equals CG.ChannelID
                               where lstChannelIDs.Contains(Chnl.ID) && CG.GaugeCategoryID == (int)Constants.GaugeCategory.HeadGauge
                               select new
                               {
                                   ID = Chnl.ID,
                                   Name = Chnl.NAME,
                                   DesignDischarge = CG.DesignDischarge,
                                   ChannelTypeID = Chnl.ChannelTypeID
                               }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstChannels;
        }

        public bool DeleteInCompleteGroups(long _RPID)
        {
            bool Result = false;
            try
            {
                // context.RP_Group.RemoveRange(context.RP_Group.Where(q => q.RPID == _RPID));
                //  context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Result = false;
            }
            return Result;
        }

        public bool DeleteDivisionData(long _RPID)
        {
            try
            {
                context.RP_Division.RemoveRange(context.RP_Division.Where(q => q.RPID == _RPID));
                //context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool DeleteChannelData(long _RPID)
        {
            try
            {
                context.RP_Channel.RemoveRange(context.RP_Channel.Where(q => q.RPID == _RPID));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public string GetGroupName(List<RP_Group> _lstGroups, short? _ID)
        {
            return _lstGroups.Where(q => q.ID == _ID).Select(w => w.Name).FirstOrDefault();
        }

        public string GetSubGroupName(List<RP_SubGroup> _lstSubGroups, short? _ID)
        {
            return _lstSubGroups.Where(q => q.ID == _ID).Select(w => w.Name).FirstOrDefault();
        }

        public List<dynamic> GetCircleGridData(long _RPID)
        {
            List<dynamic> lstSaved = new List<dynamic>();
            List<RP_Group> lstGroups = new List<RP_Group>();
            try
            {
                lstGroups = (from Grp in context.RP_Group
                             where Grp.Type == "G"
                             select Grp).ToList();

                lstSaved = (from RC in context.RP_Rotation_Circle
                            orderby RC.StartDate
                            where RC.RPID == _RPID
                            select new
                            {
                                ID = RC.ID,
                                StartDate = RC.StartDate,
                                EndDate = RC.EndDate,
                                Pref1 = RC.GroupPreference1,
                                Pref2 = RC.GroupPreference2,
                                Pref3 = RC.GroupPreference3,
                                Pref4 = RC.GroupPreference4,
                                Pref5 = RC.GroupPreference5
                            }).ToList().
                           Select(q => new
                           {
                               ID = q.ID,
                               StartDate = Utility.GetFormattedDate(q.StartDate),
                               EndDate = Utility.GetFormattedDate(q.EndDate),
                               Pref1 = GetGroupName(lstGroups, q.Pref1),
                               PrefID1 = q.Pref1,
                               Pref2 = GetGroupName(lstGroups, q.Pref2),
                               PrefID2 = q.Pref2,
                               Pref3 = GetGroupName(lstGroups, q.Pref3),
                               PrefID3 = q.Pref3,
                               Pref4 = GetGroupName(lstGroups, q.Pref4),
                               PrefID4 = q.Pref4,
                               Pref5 = GetGroupName(lstGroups, q.Pref5),
                               PrefID5 = q.Pref5
                           }).ToList<dynamic>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstSaved;
        }

        public List<RP_Division> GetProgramMappedDivisions(long _RPID)
        {
            List<RP_Division> lstDivisions = new List<RP_Division>();
            try
            {
                lstDivisions = (from Div in context.RP_Division
                                where Div.RPID == _RPID
                                select Div).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstDivisions;
        }

        public List<RP_Channel> GetProgramMappedChannels(long _RPID)
        {
            List<RP_Channel> lstDivisions = new List<RP_Channel>();
            try
            {
                lstDivisions = (from Div in context.RP_Channel
                                where Div.RPID == _RPID
                                select Div).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstDivisions;
        }

        public string GetPriorityName(short _ID)
        {
            string Name = "";

            if (_ID == 1)
                Name = "LR";
            else if (_ID == 2)
                Name = "RL";
            return Name;
        }

        public List<dynamic> GetDivSubDivPreferenceData(long _RPID)
        {
            List<dynamic> lstPreference = new List<dynamic>();
            try
            {
                List<RP_Group> lstGroups = (from G in context.RP_Group
                                            where G.Type == Constants.Group
                                            select G).ToList();

                List<RP_SubGroup> lstSubGroups = (from SG in context.RP_SubGroup
                                                  select SG).ToList();

                List<RP_Rotation_DivisionSubDivision> lstDivSubDiv = (from RDSD in context.RP_Rotation_DivisionSubDivision
                                                                      where RDSD.RPID == _RPID
                                                                      orderby RDSD.StartDate
                                                                      select RDSD).ToList();

                lstPreference = lstDivSubDiv.Select(q => new
                                        {
                                            ID = q.ID,
                                            StartDate = Utility.GetFormattedDate(q.StartDate),
                                            EndDate = Utility.GetFormattedDate(q.EndDate),
                                            Priority = GetPriorityName(q.PriorityTypeID),

                                            G1 = GetGroupName(lstGroups, q.GroupPreference1),
                                            GID1 = q.GroupPreference1,
                                            SG11 = GetSubGroupName(lstSubGroups, q.SubGroupPreference11),
                                            SGID11 = q.SubGroupPreference11,
                                            SG12 = GetSubGroupName(lstSubGroups, q.SubGroupPreference12),
                                            SGID12 = q.SubGroupPreference12,
                                            SG13 = GetSubGroupName(lstSubGroups, q.SubGroupPreference13),
                                            SGID13 = q.SubGroupPreference13,

                                            G2 = GetGroupName(lstGroups, q.GroupPreference2),
                                            GID2 = q.GroupPreference2,
                                            SG21 = GetSubGroupName(lstSubGroups, q.SubGroupPreference21),
                                            SGID21 = q.SubGroupPreference21,
                                            SG22 = GetSubGroupName(lstSubGroups, q.SubGroupPreference22),
                                            SGID22 = q.SubGroupPreference22,
                                            SG23 = GetSubGroupName(lstSubGroups, q.SubGroupPreference23),
                                            SGID23 = q.SubGroupPreference23,

                                            G3 = GetGroupName(lstGroups, q.GroupPreference3),
                                            GID3 = q.GroupPreference3,
                                            SG31 = GetSubGroupName(lstSubGroups, q.SubGroupPreference31),
                                            SGID31 = q.SubGroupPreference31,
                                            SG32 = GetSubGroupName(lstSubGroups, q.SubGroupPreference32),
                                            SGID32 = q.SubGroupPreference32,
                                            SG33 = GetSubGroupName(lstSubGroups, q.SubGroupPreference33),
                                            SGID33 = q.SubGroupPreference33,

                                            G4 = GetGroupName(lstGroups, q.GroupPreference4),
                                            GID4 = q.GroupPreference4,
                                            SG41 = GetSubGroupName(lstSubGroups, q.SubGroupPreference41),
                                            SGID41 = q.SubGroupPreference41,
                                            SG42 = GetSubGroupName(lstSubGroups, q.SubGroupPreference42),
                                            SGID42 = q.SubGroupPreference42,
                                            SG43 = GetSubGroupName(lstSubGroups, q.SubGroupPreference43),
                                            SGID43 = q.SubGroupPreference43,

                                            G5 = GetGroupName(lstGroups, q.GroupPreference5),
                                            GID5 = q.GroupPreference5,
                                            SG51 = GetSubGroupName(lstSubGroups, q.SubGroupPreference51),
                                            SGID51 = q.SubGroupPreference51,
                                            SG52 = GetSubGroupName(lstSubGroups, q.SubGroupPreference52),
                                            SGID52 = q.SubGroupPreference52,
                                            SG53 = GetSubGroupName(lstSubGroups, q.SubGroupPreference53),
                                            SGID53 = q.SubGroupPreference53
                                        }).ToList<dynamic>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstPreference;
        }

        /// <summary>
        /// This function calculate actual number of sub groups for each group 
        /// and return fixed list of of size 5  
        /// 
        /// </summary>
        /// <param name="_RPID"></param>
        /// <param name="_MaxGroups"></param>
        /// <param name="_MaxSubGroups"></param>
        public List<int> GetActualSubGroups(long _RPID, int _MaxGroups, int _MaxSubGroups)
        {
            List<int> lstSubGroupCount = new List<int>();
            List<RP_Channel> lstSG = new List<RP_Channel>();
            List<RP_SubGroup> lstSubGrps = new List<RP_SubGroup>();
            List<long> lstSubGrpIDs = new List<long>();
            int Count = 0;
            try
            {
                List<RP_Channel> lstChannels = (from Chnl in context.RP_Channel
                                                where Chnl.RPID == _RPID
                                                select Chnl).ToList();

                List<long?> lstDistinctGroups = lstChannels.Where(z => z.RPID == _RPID).GroupBy(p => p.GroupID).Select(g => g.First().GroupID).OrderBy(z => z.Value).ToList<long?>();

                lstSubGrps = (from SG in context.RP_SubGroup
                              select SG).ToList();

                for (int i = 0; i < lstDistinctGroups.Count(); i++)
                {
                    lstSubGrpIDs = lstSubGrps.Where(q => q.GroupID == lstDistinctGroups[i]).Select(w => w.ID).ToList<long>();
                    if (lstSubGrpIDs.Count() == 3)
                    {
                        Count = 0;
                        lstSG = lstChannels.Where(q => q.GroupID == lstDistinctGroups[i] && q.SubGroupID == lstSubGrpIDs[0]).ToList();
                        if (lstSG.Count() > 0)
                            Count++;

                        lstSG = lstChannels.Where(q => q.GroupID == lstDistinctGroups[i] && q.SubGroupID == lstSubGrpIDs[1]).ToList();
                        if (lstSG.Count() > 0)
                            Count++;

                        lstSG = lstChannels.Where(q => q.GroupID == lstDistinctGroups[i] && q.SubGroupID == lstSubGrpIDs[2]).ToList();
                        if (lstSG.Count() > 0)
                            Count++;
                        lstSubGroupCount.Add(Count);
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstSubGroupCount;
        }

        public long GetSubGroupsOfGroup(long _RPID, long _GroupID)
        {
            long objSubGroupCount = 0;

            try
            {
                objSubGroupCount = (from Chnl in context.RP_Channel
                                    where Chnl.RPID == _RPID && Chnl.GroupID == _GroupID && Chnl.SubGroupID != null
                                    select Chnl.SubGroupID).Distinct().Count();

                var oupCount = (from Chnl in context.RP_Channel
                                where Chnl.RPID == _RPID && Chnl.GroupID == _GroupID
                                select Chnl.SubGroupID).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objSubGroupCount;
        }

        public string GetPlanCount(long _IrrigationLevelID, long _IrrigationBoundryID, long _SeasonID, string _Year)
        {
            string CountName = "";
            try
            {
                long Count = (from RP in context.RP_RotationalProgram
                              where RP.IrrigationLevelID == _IrrigationLevelID && RP.IrrigationBoundaryID == _IrrigationBoundryID
                              && RP.SeasonID == _SeasonID && RP.RPYear == _Year
                              select RP).Count();

                if (Count == 0)
                    CountName = "Ver-01";
                else if (Count == 1)
                    CountName = "Ver-02";
                else if (Count == 2)
                    CountName = "Ver-03";
                else if (Count == 3)
                    CountName = "Ver-04";
                else if (Count == 4)
                    CountName = "Ver-05";
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return CountName;
        }

        public bool DeleteExistingAttachments(long _RPID)
        {
            try
            {
                context.RP_Attachment.RemoveRange(context.RP_Attachment.Where(q => q.RPID == _RPID));
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
            List<long?> lstChannelIDs = new List<long?>();
            try
            {
                lstChannelIDs = (from EC in context.RP_EnteringChannel
                                 where EC.RPID == _RPID
                                 select (long?)EC.ChannelID).ToList<long?>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstChannelIDs;
        }

        public DateTime ChangeYear(DateTime? _Date)
        {
            string str = _Date.ToString();
            string[] lstSplit = str.Split('/');
            lstSplit[2] = DateTime.Now.Year.ToString();
            return Convert.ToDateTime(lstSplit[0] + "/" + lstSplit[1] + "/" + lstSplit[2]);
        }

        public bool SaveCloneDraftData(long _CloneRPID, long _CurrentRPID, int _CreatedBy, long _IrrigationLevelID)
        {
            try
            {
                if (_IrrigationLevelID == (int)Constants.IrrigationLevelID.Circle)
                {
                    List<RP_Division> lstDiv = (from EC in context.RP_Division
                                                where EC.RPID == _CloneRPID
                                                select EC).ToList();
                    lstDiv.ForEach(u =>
                    {
                        u.RPID = _CurrentRPID;
                        u.CreatedDate = DateTime.Now;
                        u.CreatedBy = _CreatedBy;
                    });

                    context.RP_Division.AddRange(lstDiv);





                    List<RP_Rotation_Circle> lstCircle = (from RDSD in context.RP_Rotation_Circle
                                                          where RDSD.RPID == _CloneRPID
                                                          select RDSD).ToList();
                    lstCircle.ForEach(u =>
                    {
                        u.RPID = _CurrentRPID;
                        u.StartDate = ChangeYear(u.StartDate);
                        u.EndDate = ChangeYear(u.EndDate); ;
                        u.CreatedDate = DateTime.Now;
                        u.CreatedBy = _CreatedBy;
                        u.ModifiedDate = DateTime.Now;
                        u.ModifiedBy = _CreatedBy;
                    });

                    context.RP_Rotation_Circle.AddRange(lstCircle);
                }
                else
                {
                    List<RP_EnteringChannel> lstEC = (from EC in context.RP_EnteringChannel
                                                      where EC.RPID == _CloneRPID
                                                      select EC).ToList();
                    lstEC.ForEach(u =>
                          {
                              u.RPID = _CurrentRPID;
                              u.CreatedDate = DateTime.Now;
                              u.CreatedBy = _CreatedBy;
                          });

                    context.RP_EnteringChannel.AddRange(lstEC);

                    List<RP_Channel> lstChnl = (from Chnl in context.RP_Channel
                                                where Chnl.RPID == _CloneRPID
                                                select Chnl).ToList();
                    lstChnl.ForEach(u =>
                    {
                        u.RPID = _CurrentRPID;
                        u.CreatedDate = DateTime.Now;
                        u.CreatedBy = _CreatedBy;
                        u.ModifiedDate = DateTime.Now;
                        u.ModifiedBy = _CreatedBy;
                    });

                    context.RP_Channel.AddRange(lstChnl);

                    List<RP_Rotation_DivisionSubDivision> lstDivSubDiv = (from RDSD in context.RP_Rotation_DivisionSubDivision
                                                                          where RDSD.RPID == _CloneRPID
                                                                          select RDSD).ToList();
                    lstDivSubDiv.ForEach(u =>
                    {
                        u.RPID = _CurrentRPID;
                        u.StartDate = ChangeYear(u.StartDate);
                        u.EndDate = ChangeYear(u.EndDate); ;
                        u.CreatedDate = DateTime.Now;
                        u.CreatedBy = _CreatedBy;
                        u.ModifiedDate = DateTime.Now;
                        u.ModifiedBy = _CreatedBy;
                    });

                    context.RP_Rotation_DivisionSubDivision.AddRange(lstDivSubDiv);
                }
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool AddEnteringchannels(List<RP_EnteringChannel> _lstEC)
        {
            try
            {
                context.RP_EnteringChannel.AddRange(_lstEC);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public List<long?> GetMainAndBranchChannels(bool _FromDiv, List<long?> _lstSections) // Get Main and branch canals whose parents are not in this division 
        {
            List<long?> lstRetChnlIDs = new List<long?>();
            List<long?> lstChnlIDs = new List<long?>();
            List<CO_ChannelGauge> lstGauges = new List<CO_ChannelGauge>();
            try
            {
                List<long?> _lstChnlID = (from CIB in context.CO_ChannelIrrigationBoundaries
                                          where _lstSections.Contains(CIB.SectionID)
                                          select CIB.ChannelID).Distinct().ToList<long?>();

                List<CO_ChannelParentFeeder> lstPF = (from CPF in context.CO_ChannelParentFeeder
                                                      where _lstChnlID.Contains(CPF.ChannelID)
                                                      select CPF).ToList();

                List<long?> lst12IDs = (from ls in lstPF
                                        where !_lstChnlID.Contains(ls.ParrentFeederID)
                                        select ls.ChannelID).ToList();
                if (_FromDiv)
                {
                    lstGauges = (from CG in context.CO_ChannelGauge
                                 where lst12IDs.Contains(CG.ChannelID)
                                 && _lstSections.Contains(CG.SectionID)
                                 select CG).ToList();
                    lstChnlIDs = (from LG in lstGauges
                                  where (LG.IsDivGauge == true)
                                  select LG.ChannelID).ToList();

                    lstChnlIDs = (from LG in lstGauges
                                  where !lstChnlIDs.Contains(LG.ChannelID) &&
                                  (LG.IsDivGauge == false || LG.IsDivGauge == null)
                                  select LG.ChannelID).Distinct().ToList();

                    List<CO_Channel> lstChannels = (from Chnl in context.CO_Channel
                                                    where lstChnlIDs.Contains(Chnl.ID)
                                                    select Chnl).ToList();

                    lstRetChnlIDs = lstChannels.Where(q => q.ChannelTypeID == (int)Constants.ChannelType.BranchCanal || q.ChannelTypeID == (int)Constants.ChannelType.MainCanal).Select(w => (long?)w.ID).ToList<long?>();
                }
                else
                {
                    lstGauges = (from CG in context.CO_ChannelGauge
                                 where _lstSections.Contains(CG.SectionID)
                                 && _lstChnlID.Contains(CG.ChannelID)
                                 select CG).ToList();

                    lstChnlIDs = (from LG in lstGauges
                                  where LG.IsSubDivGauge == true
                                  select LG.ChannelID).ToList();

                    lstChnlIDs = (from LG in lstGauges
                                  where !lstChnlIDs.Contains(LG.ChannelID) &&
                                  (LG.IsSubDivGauge == false || LG.IsSubDivGauge == null)
                                  select LG.ChannelID).Distinct().ToList();

                    List<CO_Channel> lstChannels = (from Chnl in context.CO_Channel
                                                    where lstChnlIDs.Contains(Chnl.ID)
                                                    select Chnl).ToList();

                    lstRetChnlIDs = lstChannels.Where(q => q.ChannelTypeID == (int)Constants.ChannelType.BranchCanal || q.ChannelTypeID == (int)Constants.ChannelType.MainCanal).Select(w => (long?)w.ID).ToList<long?>();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstRetChnlIDs;
        }

        #endregion

        #region Export


        public dynamic GetBasicInfoForSeExport(long _ID)
        {
            dynamic objInfo = null;
            try
            {
                objInfo = (from RP in context.RP_RotationalProgram
                           where RP.ID == _ID
                           select RP).ToList().
                          Select(q =>
                              new
                              {
                                  StartDate = q.ProgramStartDate,
                                  GroupQuantity = q.NoOfGroup,
                                  DivQuantity = context.RP_Division.Where(x => x.RPID == _ID).Count(),
                                  SeasonID = q.SeasonID,
                                  ClosureStartDate = q.ClosureStartDate,
                                  ClosureEndDate = q.ClosureEndDate
                              }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objInfo;
        }
        public dynamic GetBasicInfoForXENSDOExport(long _ID)
        {
            dynamic objInfo = null;
            try
            {
                objInfo = (from RP in context.RP_RotationalProgram
                           where RP.ID == _ID
                           select RP).ToList().
                          Select(q =>
                              new
                              {
                                  StartDate = q.ProgramStartDate,
                                  GroupQuantity = q.NoOfGroup,
                                  ChannelQuantity = context.RP_Channel.Where(x => x.RPID == _ID).Count(),
                                  SeasonID = q.SeasonID,
                                  ClosureStartDate = q.ClosureStartDate,
                                  ClosureEndDate = q.ClosureEndDate
                              }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objInfo;
        }
        public List<dynamic> GetDivisionsByGroups(long _ID)
        {
            List<dynamic> GroupsDivisions = new List<dynamic>();
            try
            {
                GroupsDivisions = (from RPD in context.RP_Division
                                   join Division in context.CO_Division on RPD.DivisionID equals Division.ID
                                   join RPG in context.RP_Group on RPD.GroupID equals RPG.ID
                                   where RPD.RPID == _ID
                                   group RPD by new { RPD.GroupID, RPG.Name } into g
                                   select new { GroupName = g.Key.Name, Division = g.Select(x => x.CO_Division.Name).ToList() }).ToList<dynamic>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return GroupsDivisions;
        }


        public List<dynamic> GetWaraInfoForSEExport(long _ID, string _ClosureStartDate, string _ClosureEndDate)
        {
            DateTime? ClosureStartDate = null;
            DateTime? ClosureEndDate = null;
            if (!string.IsNullOrEmpty(_ClosureStartDate))
                ClosureStartDate = Convert.ToDateTime(_ClosureStartDate);
            if (!string.IsNullOrEmpty(_ClosureEndDate))
                ClosureEndDate = Convert.ToDateTime(_ClosureEndDate);
            List<dynamic> lstWaraInfo = new List<dynamic>();
            try
            {
                lstWaraInfo = (from RPC in context.RP_Rotation_Circle
                               where RPC.RPID == _ID
                               //&& (_ClosureStartDate == "" || ((DbFunctions.TruncateTime(RPC.StartDate) != DbFunctions.TruncateTime(ClosureStartDate)) && (DbFunctions.TruncateTime(RPC.EndDate) != DbFunctions.TruncateTime(ClosureEndDate))))
                               select new
                             {
                                 StartDate = RPC.StartDate,
                                 EndDate = RPC.EndDate,
                                 GP1 = RPC.GroupPreference1,
                                 GP2 = RPC.GroupPreference2,
                                 GP3 = RPC.GroupPreference3,
                                 GP4 = RPC.GroupPreference4,
                                 GP5 = RPC.GroupPreference5,
                                 Priority = RPC.PriorityTypeID


                             }).ToList()
                              .Select(u => new
                              {
                                  Days = GetDateDifferenceInDays(u.StartDate.Value, u.EndDate.Value),
                                  GP1 = u.GP1 == null ? "N/A" : GetGroupNameByID(u.GP1),
                                  GP2 = u.GP2 == null ? "N/A" : GetGroupNameByID(u.GP2),
                                  GP3 = u.GP3 == null ? "N/A" : GetGroupNameByID(u.GP3),
                                  GP4 = u.GP4 == null ? "N/A" : GetGroupNameByID(u.GP4),
                                  GP5 = u.GP5 == null ? "N/A" : GetGroupNameByID(u.GP5),
                                  StartDate = u.StartDate,
                                  EndDate = u.EndDate,
                                  Priority = u.Priority == 1 ? "LR" : "RL"

                              }).Distinct().ToList<object>();

                lstWaraInfo = lstWaraInfo.Where(x => x.StartDate != ClosureEndDate && x.EndDate != ClosureEndDate).ToList<dynamic>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstWaraInfo;
        }

        public double GetDateDifferenceInDays(DateTime StartDate, DateTime EndDate)
        {
            return (EndDate.Date - StartDate.Date).TotalDays;
        }

        public string GetGroupNameByID(short? _GID)
        {
            string GroupName = "";
            try
            {
                GroupName = (from RG in context.RP_Group
                             where RG.ID == _GID
                             select RG.Name).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return GroupName;
        }
        public string GetSubGroupNameByID(short? _GID)
        {
            string GroupName = "";
            try
            {
                GroupName = (from RG in context.RP_SubGroup
                             where RG.ID == _GID
                             select RG.Name).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return GroupName;
        }

        public List<dynamic> GetChannelsBySubGroups(long _ID)
        {
            List<dynamic> lstChannels = new List<dynamic>();
            List<dynamic> ChannelsForGroups = new List<dynamic>();
            List<dynamic> ChannelsForSubGroups = new List<dynamic>();
            try
            {
                ChannelsForGroups = (from RPC in context.RP_Channel
                                     join Channels in context.CO_Channel on RPC.ChannelID equals Channels.ID
                                     join RPG in context.RP_Group on RPC.GroupID equals RPG.ID
                                     where RPC.RPID == _ID && RPC.SubGroupID == null
                                     group RPC by new { RPC.GroupID, RPG.Name } into g
                                     select new { GroupName = g.Key.Name, Channels = g.Select(x => new { x.CO_Channel.NAME, x.Discharge }).ToList<dynamic>() }).ToList<dynamic>();


                ChannelsForSubGroups = (from RPC in context.RP_Channel
                                        join Channels in context.CO_Channel on RPC.ChannelID equals Channels.ID
                                        //join RPG in context.RP_Group on RPC.GroupID equals RPG.ID
                                        join RPS in context.RP_SubGroup on RPC.SubGroupID equals RPS.ID
                                        where RPC.RPID == _ID && RPC.SubGroupID != null
                                        group RPC by new { RPC.SubGroupID, RPS.Name } into g
                                        select new { GroupName = g.Key.Name, Channels = g.Select(x => new { x.CO_Channel.NAME, x.Discharge }).ToList<dynamic>() }).ToList<dynamic>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            var Groups = ChannelsForGroups;
            var SubGroups = ChannelsForSubGroups;
            lstChannels.AddRange(Groups);
            lstChannels.AddRange(SubGroups);
            lstChannels = lstChannels.OrderBy(x => x.GroupName).ToList<dynamic>();

            return lstChannels;//SubGroupsChannels;
        }

        public List<dynamic> GetChannelPreferences(List<dynamic> _WaraData)
        {
            List<dynamic> TotalValues = new List<dynamic>();
            try
            {
                List<string> values = new List<string>();

                for (int i = 0; i < _WaraData.Count; i++)
                {
                    //values.Add(Utility.GetDynamicPropertyValue(lstWaraData[i], "Days"));
                    int count = 1;
                    string PreviousVal = "";
                    string NewVal = "";
                    for (int j = 0; j < 20; j++)
                    {

                        if (Utility.GetDynamicPropertyValue(_WaraData[i], "GP" + count) == "N/A")
                        {
                            count++;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(PreviousVal))
                            {
                                values.Add(Utility.GetDynamicPropertyValue(_WaraData[i], "GP" + count));
                                PreviousVal = Utility.GetDynamicPropertyValue(_WaraData[i], "GP" + count);

                            }
                            else
                            {
                                NewVal = Utility.GetDynamicPropertyValue(_WaraData[i], "GP" + count);
                                if (NewVal.Contains(PreviousVal))
                                {
                                    values[values.Count - 1] = NewVal;
                                    PreviousVal = NewVal;

                                }
                                else
                                {
                                    values.Add(NewVal);
                                    PreviousVal = NewVal;
                                }
                            }

                            count++;
                        }
                    }
                    //values.Add(Utility.GetDynamicPropertyValue(lstWaraData[i], "Priority"));
                    TotalValues.Add(values);
                    values = new List<string>();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return TotalValues;
        }

        public List<dynamic> GetChannelsWaraData(long _ID, string _ClosureStartDate, string _ClosureEndDate)
        {
            DateTime? ClosureStartDate = null;
            DateTime? ClosureEndDate = null;
            if (!string.IsNullOrEmpty(_ClosureStartDate))
                ClosureStartDate = Convert.ToDateTime(_ClosureStartDate);
            if (!string.IsNullOrEmpty(_ClosureEndDate))
                ClosureEndDate = Convert.ToDateTime(_ClosureEndDate);
            List<dynamic> lstWaraData = new List<dynamic>();
            try
            {
                lstWaraData = (from RPC in context.RP_Rotation_DivisionSubDivision
                               where RPC.RPID == _ID
                               //&& (_ClosureStartDate == "" || (DbFunctions.TruncateTime(RPC.StartDate) != DbFunctions.TruncateTime(ClosureStartDate) && DbFunctions.TruncateTime(RPC.EndDate) != DbFunctions.TruncateTime(ClosureEndDate)))
                               select new
                               {
                                   StartDate = RPC.StartDate,
                                   EndDate = RPC.EndDate,
                                   Priority = RPC.PriorityTypeID,
                                   GP1 = RPC.GroupPreference1,
                                   GP2 = RPC.SubGroupPreference11,
                                   GP3 = RPC.SubGroupPreference12,
                                   GP4 = RPC.SubGroupPreference13,
                                   GP5 = RPC.GroupPreference2,
                                   GP6 = RPC.SubGroupPreference21,
                                   GP7 = RPC.SubGroupPreference22,
                                   GP8 = RPC.SubGroupPreference23,
                                   GP9 = RPC.GroupPreference3,
                                   GP10 = RPC.SubGroupPreference31,
                                   GP11 = RPC.SubGroupPreference32,
                                   GP12 = RPC.SubGroupPreference33,
                                   GP13 = RPC.GroupPreference4,
                                   GP14 = RPC.SubGroupPreference41,
                                   GP15 = RPC.SubGroupPreference42,
                                   GP16 = RPC.SubGroupPreference43,
                                   GP17 = RPC.GroupPreference5,
                                   GP18 = RPC.SubGroupPreference51,
                                   GP19 = RPC.SubGroupPreference52,
                                   GP20 = RPC.SubGroupPreference53
                               }).ToList()
                              .Select(u => new
                              {
                                  Days = GetDateDifferenceInDays(u.StartDate.Value, u.EndDate.Value),
                                  GP1 = u.GP1 == null ? "N/A" : GetGroupNameByID(u.GP1),
                                  GP2 = u.GP2 == null ? "N/A" : GetSubGroupNameByID(u.GP2),
                                  GP3 = u.GP3 == null ? "N/A" : GetSubGroupNameByID(u.GP3),
                                  GP4 = u.GP4 == null ? "N/A" : GetSubGroupNameByID(u.GP4),
                                  GP5 = u.GP5 == null ? "N/A" : GetGroupNameByID(u.GP5),
                                  GP6 = u.GP6 == null ? "N/A" : GetSubGroupNameByID(u.GP6),
                                  GP7 = u.GP7 == null ? "N/A" : GetSubGroupNameByID(u.GP7),
                                  GP8 = u.GP8 == null ? "N/A" : GetSubGroupNameByID(u.GP8),
                                  GP9 = u.GP9 == null ? "N/A" : GetGroupNameByID(u.GP9),
                                  GP10 = u.GP10 == null ? "N/A" : GetSubGroupNameByID(u.GP10),
                                  GP11 = u.GP11 == null ? "N/A" : GetSubGroupNameByID(u.GP11),
                                  GP12 = u.GP12 == null ? "N/A" : GetSubGroupNameByID(u.GP12),
                                  GP13 = u.GP13 == null ? "N/A" : GetGroupNameByID(u.GP13),
                                  GP14 = u.GP14 == null ? "N/A" : GetSubGroupNameByID(u.GP14),
                                  GP15 = u.GP15 == null ? "N/A" : GetSubGroupNameByID(u.GP15),
                                  GP16 = u.GP16 == null ? "N/A" : GetSubGroupNameByID(u.GP16),
                                  GP17 = u.GP17 == null ? "N/A" : GetGroupNameByID(u.GP17),
                                  GP18 = u.GP18 == null ? "N/A" : GetSubGroupNameByID(u.GP18),
                                  GP19 = u.GP19 == null ? "N/A" : GetSubGroupNameByID(u.GP19),
                                  GP20 = u.GP20 == null ? "N/A" : GetSubGroupNameByID(u.GP20),
                                  StartDate = u.StartDate,
                                  EndDate = u.EndDate,
                                  Priority = u.Priority == 1 ? "LR" : (u.Priority == 0 ? "0" : "RL")

                              }).ToList<object>();

                lstWaraData = lstWaraData.Where(x => x.StartDate != ClosureEndDate && x.EndDate != ClosureEndDate).ToList<dynamic>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstWaraData;
        }

        #endregion

        #region View and Print
        public List<dynamic> GetGroupsSubGroupsChannels(long _ID)
        {
            List<dynamic> lstChannels = new List<dynamic>();
            try
            {
                lstChannels = (from RPC in context.RP_Channel
                               join Channels in context.CO_Channel on RPC.ChannelID equals Channels.ID
                               join RPG in context.RP_Group on RPC.GroupID equals RPG.ID
                               //join RPS in context.RP_SubGroup on RPC.SubGroupID equals RPS.ID
                               where RPC.RPID == _ID
                               group RPC by new { RPC.SubGroupID, RPC.GroupID, GName = RPG.Name } into g
                               select new { GroupName = g.Key.GName, SubGroupID = g.Key.SubGroupID, TotalDischarge = g.Sum(x => x.Discharge), Channels = g.Select(x => new { x.CO_Channel.NAME, x.Discharge }).ToList<dynamic>() }).ToList<dynamic>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }


            return lstChannels;
        }

        public dynamic GetProgramNameAndDates(long _RPID, string _Value)
        {
            dynamic objInfo = null;
            try
            {
                objInfo = (from RP in context.RP_RotationalProgram
                           where RP.ID == _RPID
                           select RP).ToList().
                          Select(q =>
                              new
                              {
                                  ProgramName = q.Name,
                                  StartDate = q.ProgramStartDate,
                                  EndDate = _Value.ToUpper() == "C" ? context.RP_Rotation_Circle.Where(x => x.RPID == _RPID).OrderByDescending(x => x.ID).Select(x => x.EndDate).FirstOrDefault() : context.RP_Rotation_DivisionSubDivision.Where(x => x.RPID == _RPID).OrderByDescending(x => x.ID).Select(x => x.EndDate).FirstOrDefault(),
                                  SeasonID = q.SeasonID,
                                  ClosureStartDate = q.ClosureStartDate,
                                  ClosureEndDate = q.ClosureEndDate,
                                  GroupQty = q.NoOfGroup
                              }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objInfo;
        }

        public List<dynamic> GetPreferencesDataForView(long _ID)
        {


            List<dynamic> lstWaraData = new List<dynamic>();
            try
            {
                lstWaraData = (from RPC in context.RP_Rotation_DivisionSubDivision
                               orderby RPC.StartDate
                               where RPC.RPID == _ID

                               select new
                               {
                                   StartDate = RPC.StartDate,
                                   EndDate = RPC.EndDate,
                                   Priority = RPC.PriorityTypeID,
                                   GP1 = RPC.GroupPreference1,
                                   GP2 = RPC.GroupPreference2,
                                   GP3 = RPC.GroupPreference3,
                                   GP4 = RPC.GroupPreference4,
                                   GP5 = RPC.GroupPreference5,
                                   GP6 = RPC.SubGroupPreference11,
                                   GP7 = RPC.SubGroupPreference12,
                                   GP8 = RPC.SubGroupPreference13,
                                   GP9 = RPC.SubGroupPreference21,
                                   GP10 = RPC.SubGroupPreference22,
                                   GP11 = RPC.SubGroupPreference23,
                                   GP12 = RPC.SubGroupPreference31,
                                   GP13 = RPC.SubGroupPreference32,
                                   GP14 = RPC.SubGroupPreference33,
                                   GP15 = RPC.SubGroupPreference41,
                                   GP16 = RPC.SubGroupPreference42,
                                   GP17 = RPC.SubGroupPreference43,
                                   GP18 = RPC.SubGroupPreference51,
                                   GP19 = RPC.SubGroupPreference52,
                                   GP20 = RPC.SubGroupPreference53
                               }).ToList()
                              .Select(u => new
                              {
                                  FromDate = u.StartDate,
                                  ToDate = u.EndDate,
                                  GP1 = u.GP1 == null ? "N/A" : GetGroupNameByID(u.GP1),
                                  GP2 = u.GP2 == null ? "N/A" : GetGroupNameByID(u.GP2),
                                  GP3 = u.GP3 == null ? "N/A" : GetGroupNameByID(u.GP3),
                                  GP4 = u.GP4 == null ? "N/A" : GetGroupNameByID(u.GP4),
                                  GP5 = u.GP5 == null ? "N/A" : GetGroupNameByID(u.GP5),
                                  GP6 = u.GP6 == null ? "N/A" : GetSubGroupNameByID(u.GP6),
                                  GP7 = u.GP7 == null ? "N/A" : GetSubGroupNameByID(u.GP7),
                                  GP8 = u.GP8 == null ? "N/A" : GetSubGroupNameByID(u.GP8),
                                  GP9 = u.GP9 == null ? "N/A" : GetSubGroupNameByID(u.GP9),
                                  GP10 = u.GP10 == null ? "N/A" : GetSubGroupNameByID(u.GP10),
                                  GP11 = u.GP11 == null ? "N/A" : GetSubGroupNameByID(u.GP11),
                                  GP12 = u.GP12 == null ? "N/A" : GetSubGroupNameByID(u.GP12),
                                  GP13 = u.GP13 == null ? "N/A" : GetSubGroupNameByID(u.GP13),
                                  GP14 = u.GP14 == null ? "N/A" : GetSubGroupNameByID(u.GP14),
                                  GP15 = u.GP15 == null ? "N/A" : GetSubGroupNameByID(u.GP15),
                                  GP16 = u.GP16 == null ? "N/A" : GetSubGroupNameByID(u.GP16),
                                  GP17 = u.GP17 == null ? "N/A" : GetSubGroupNameByID(u.GP17),
                                  GP18 = u.GP18 == null ? "N/A" : GetSubGroupNameByID(u.GP18),
                                  GP19 = u.GP19 == null ? "N/A" : GetSubGroupNameByID(u.GP19),
                                  GP20 = u.GP20 == null ? "N/A" : GetSubGroupNameByID(u.GP20),
                                  Priority = u.GP1 == null ? "Closure Period" : (u.Priority == 1 ? "LR" : (u.Priority == 0 ? "0" : "RL"))

                              }).Distinct().ToList<object>();





            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstWaraData;
        }

        public List<dynamic> GetPreferencesDataDivisionForView(long _ID)
        {


            List<dynamic> lstWaraData = new List<dynamic>();
            try
            {
                lstWaraData = (from RPC in context.RP_Rotation_Circle
                               where RPC.RPID == _ID

                               select new
                               {
                                   StartDate = RPC.StartDate,
                                   EndDate = RPC.EndDate,
                                   Priority = RPC.PriorityTypeID,
                                   GP1 = RPC.GroupPreference1,
                                   GP2 = RPC.GroupPreference2,
                                   GP3 = RPC.GroupPreference3,
                                   GP4 = RPC.GroupPreference4,
                                   GP5 = RPC.GroupPreference5,
                               }).ToList()
                              .Select(u => new
                              {
                                  FromDate = u.StartDate,
                                  ToDate = u.EndDate,
                                  GP1 = u.GP1 == null ? "N/A" : GetGroupNameByID(u.GP1),
                                  GP2 = u.GP2 == null ? "N/A" : GetGroupNameByID(u.GP2),
                                  GP3 = u.GP3 == null ? "N/A" : GetGroupNameByID(u.GP3),
                                  GP4 = u.GP4 == null ? "N/A" : GetGroupNameByID(u.GP4),
                                  GP5 = u.GP5 == null ? "N/A" : GetGroupNameByID(u.GP5),
                                  Priority = u.GP1 == null ? "Closure Period" : (u.Priority == 1 ? "LR" : "RL")

                              }).Distinct().ToList<object>();


            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstWaraData;
        }
        public dynamic NonNullPropertiesCount(object entity)
        {
            return entity.GetType()
                         .GetProperties()
                         .Select(x => x.GetValue(entity, null))
                         .Where(x => x != null);

        }
        #endregion

        #region Model integration

        public List<List<dynamic>> GetWaraDates(long _RPID)
        {
            List<dynamic> lstDate = new List<dynamic>();
            List<List<dynamic>> lstReturnDates = new List<List<dynamic>>();
            try
            {
                long IrrigationLevelID = (from RP in context.RP_RotationalProgram
                                          where RP.ID == _RPID
                                          select RP.IrrigationLevelID).FirstOrDefault();


                if (IrrigationLevelID == (int)Constants.IrrigationLevelID.Circle)
                {
                    List<RP_Rotation_Circle> lstResult = (from RC in context.RP_Rotation_Circle
                                                          where RC.RPID == _RPID
                                                          select RC).ToList();

                    lstDate = lstResult.Select(q =>
                    new
                    {
                        FromDate = Utility.GetFormattedDate(q.StartDate.Value)
                    }).ToList<dynamic>();
                    lstReturnDates.Add(lstDate);

                    lstDate = lstResult.Select(q =>
                    new
                    {
                        ToDate = Utility.GetFormattedDate(q.EndDate.Value)
                    }).ToList<dynamic>();
                    lstReturnDates.Add(lstDate);
                }
                else
                {
                    List<RP_Rotation_DivisionSubDivision> lstResult = (from RD in context.RP_Rotation_DivisionSubDivision
                                                                       where RD.RPID == _RPID
                                                                       select RD).ToList();
                    lstDate = lstResult.Select(q =>
                    new
                    {
                        FromDate = Utility.GetFormattedDate(q.StartDate.Value)
                    }).ToList<dynamic>();
                    lstReturnDates.Add(lstDate);

                    lstDate = lstResult.Select(q =>
                    new
                    {
                        ToDate = Utility.GetFormattedDate(q.EndDate.Value)
                    }).ToList<dynamic>();
                    lstReturnDates.Add(lstDate);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstReturnDates;
        }

        public List<RP_Rotation_DivisionSubDivision> AdjustDateDifference(long _RPID, DateTime _DiffStartDate, int _DaysDiff)
        {
            List<RP_Rotation_DivisionSubDivision> lstRecords = new List<RP_Rotation_DivisionSubDivision>();
            try
            {
                lstRecords = (from RDSD in context.RP_Rotation_DivisionSubDivision
                              where RDSD.RPID == _RPID && RDSD.StartDate >= _DiffStartDate
                              select RDSD).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstRecords;
        }

        public bool GetPlanImplementation(long _RPID, double _MaxPrecentage, double _MinPercentage, DateTime _FromDate, DateTime _ToDate)
        {
            try
            {
                context.RP_InsertionPlanImplementation(_RPID, _MaxPrecentage, _MinPercentage);
                context.RP_InsertionPlanImplementationPercentage(_RPID, _FromDate, _ToDate, _MaxPrecentage, _MinPercentage);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public long? GetPreviousUserDesignationID(long? _RPID)
        {
            long? DesID = -1;
            try
            {
                DesID = (from Aprvl in context.RP_Approval
                         where Aprvl.RPID == _RPID
                         select Aprvl).OrderByDescending(q => q.CreatedDate).FirstOrDefault().DesignationFromID;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return DesID;
        }

        public RP_Approval ShowhideApprovalButtons(long _RPID)
        {
            RP_Approval objApproval = new RP_Approval();
            try
            {
                objApproval = (from Aprvl in context.RP_Approval
                               where Aprvl.RPID == _RPID
                               select Aprvl).OrderByDescending(q => q.CreatedDate).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objApproval;
        }

        public string CheckConflictingChannels(long _RPID, List<dynamic> _lstChannels)
        {
            string DraftName = "";
            try
            {
                RP_RotationalProgram objRP = (from RP in context.RP_RotationalProgram
                                              where RP.ID == _RPID
                                              select RP).FirstOrDefault();
                if (objRP != null)
                {
                    List<RP_RotationalProgram> lstRP = (from RP in context.RP_RotationalProgram
                                                        where RP.ID != _RPID && RP.IsApproved == true &&
                                                        RP.IrrigationLevelID == objRP.IrrigationLevelID && RP.IrrigationBoundaryID == objRP.IrrigationBoundaryID &&
                                                        RP.SeasonID == objRP.SeasonID && RP.RPYear == objRP.RPYear
                                                        select RP).ToList();

                    List<dynamic> lstChnlIDs = (from Chnl in _lstChannels
                                                select new
                                                    {
                                                        ChannelID = Convert.ToInt64(Utility.GetDynamicPropertyValue(Chnl, "ChannelID"))
                                                    }).ToList<dynamic>();

                    List<long> lstFinalIDs = lstChnlIDs.Select(q => (long)q.ChannelID).Distinct().ToList<long>();

                    foreach (var rp in lstRP)
                    {
                        List<RP_Channel> lstCommonChnlID = (from Chnl in context.RP_Channel
                                                            where Chnl.RPID == rp.ID
                                                            && lstFinalIDs.Contains(Chnl.ChannelID)
                                                            select Chnl).ToList();
                        if (lstCommonChnlID.Count() > 0)
                        {
                            DraftName = (from LRP in lstRP
                                         where LRP.ID == rp.ID
                                         select LRP.Name).FirstOrDefault();
                            break;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return DraftName;
        }

        public List<dynamic> GetCommentsHistory(long _RPID)
        {
            List<dynamic> lstComments = new List<dynamic>();
            try
            {
                lstComments = (from RP in context.RP_Approval
                               where RP.RPID == _RPID && RP.DesignationFromID == (int)Constants.Designation.SE
                               select new
                               {
                                   Designation = "SE",
                                   Date = RP.CreatedDate,
                                   Comments = RP.Comments
                               }).ToList().Select(q => new
                                   {
                                       Designation = "SE",
                                       Date = Utility.GetFormattedDate(q.Date),
                                       Comments = q.Comments
                                   }).ToList<dynamic>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstComments;
        }

        public List<RP_GetAvgDPRChannelName_Result> GetGraphData(long _RPID)
        {
            List<RP_GetAvgDPRChannelName_Result> Result = new List<RP_GetAvgDPRChannelName_Result>();
            try
            {
                Result = context.RP_GetAvgDPRChannelName(_RPID).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public List<RP_GetBandFrequency_Result> GetFrequencyTable(long _RPID)
        {
            List<RP_GetBandFrequency_Result> Result = new List<RP_GetBandFrequency_Result>();
            try
            {
                Result = context.RP_GetBandFrequency(_RPID).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Result;
        }

        public double? GetMaxMinDPR(long _RPID)
        {
            double? DPR = null;
            try
            {
                DPR = context.RP_GetAvgDPRDifference(_RPID).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return DPR;
        }

        public bool GetGini(long _RPID)
        {
            try
            {
                var v = context.RP_GetChannelAvgDPR(_RPID, null, null, null);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }


        #endregion

        #region CR

        public RP_Rotation_DivisionSubDivision CopyObject(RP_Rotation_DivisionSubDivision _obj, RP_Rotation_DivisionSubDivision _newObj, DateTime? _startDate, DateTime? _endDate)
        {
            _newObj.RPID = _obj.RPID;
            _newObj.StartDate = _startDate;
            _newObj.EndDate = _endDate;
            _newObj.GroupPreference1 = _obj.GroupPreference1;
            _newObj.SubGroupPreference11 = _obj.SubGroupPreference11;
            _newObj.SubGroupPreference12 = _obj.SubGroupPreference12;
            _newObj.SubGroupPreference13 = _obj.SubGroupPreference13;

            _newObj.GroupPreference2 = _obj.GroupPreference2;
            _newObj.SubGroupPreference21 = _obj.SubGroupPreference21;
            _newObj.SubGroupPreference22 = _obj.SubGroupPreference22;
            _newObj.SubGroupPreference23 = _obj.SubGroupPreference23;

            _newObj.GroupPreference3 = _obj.GroupPreference3;
            _newObj.SubGroupPreference31 = _obj.SubGroupPreference31;
            _newObj.SubGroupPreference32 = _obj.SubGroupPreference32;
            _newObj.SubGroupPreference33 = _obj.SubGroupPreference33;

            _newObj.GroupPreference4 = _obj.GroupPreference4;
            _newObj.SubGroupPreference41 = _obj.SubGroupPreference41;
            _newObj.SubGroupPreference42 = _obj.SubGroupPreference42;
            _newObj.SubGroupPreference43 = _obj.SubGroupPreference43;

            _newObj.GroupPreference5 = _obj.GroupPreference5;
            _newObj.SubGroupPreference51 = _obj.SubGroupPreference51;
            _newObj.SubGroupPreference52 = _obj.SubGroupPreference52;
            _newObj.SubGroupPreference53 = _obj.SubGroupPreference53;
            _newObj.PriorityTypeID = _obj.PriorityTypeID;
            _newObj.CreatedDate = DateTime.Now;
            _newObj.CreatedBy = _obj.CreatedBy;
            _newObj.ModifiedDate = DateTime.Now;
            _newObj.ModifiedBy = _obj.ModifiedBy;

            return _newObj;
        }


        public RP_Rotation_DivisionSubDivision ClosureObject(RP_Rotation_DivisionSubDivision _obj, DateTime? _startDate, DateTime? _endDate)
        {
            if (_startDate != null)
                _obj.StartDate = _startDate;
            if (_endDate != null)
                _obj.EndDate = _endDate;

            _obj.GroupPreference1 = null;
            _obj.SubGroupPreference11 = null;
            _obj.SubGroupPreference12 = null;
            _obj.SubGroupPreference13 = null;

            _obj.GroupPreference2 = null;
            _obj.SubGroupPreference21 = null;
            _obj.SubGroupPreference22 = null;
            _obj.SubGroupPreference23 = null;

            _obj.GroupPreference3 = null;
            _obj.SubGroupPreference31 = null;
            _obj.SubGroupPreference32 = null;
            _obj.SubGroupPreference33 = null;

            _obj.GroupPreference4 = null;
            _obj.SubGroupPreference41 = null;
            _obj.SubGroupPreference42 = null;
            _obj.SubGroupPreference43 = null;

            _obj.GroupPreference5 = null;
            _obj.SubGroupPreference51 = null;
            _obj.SubGroupPreference52 = null;
            _obj.SubGroupPreference53 = null;
            _obj.PriorityTypeID = 0;

            return _obj;

        }

        public bool ManageClosureDates(long _ProgramID, DateTime _StartDate, DateTime _EndDate)
        {
            List<RP_Rotation_DivisionSubDivision> lstWaras = new List<RP_Rotation_DivisionSubDivision>();
            List<RP_Rotation_DivisionSubDivision> lstWarasIterations = new List<RP_Rotation_DivisionSubDivision>();
            RP_Rotation_DivisionSubDivision obj = new RP_Rotation_DivisionSubDivision();
            RP_Rotation_DivisionSubDivision newObj1 = new RP_Rotation_DivisionSubDivision();
            RP_Rotation_DivisionSubDivision newObj2 = new RP_Rotation_DivisionSubDivision();
            RP_Rotation_DivisionSubDivision closureObj = new RP_Rotation_DivisionSubDivision();
            List<long> lstIDs = new List<long>();
            bool Result = false;
            bool Delimeter = false;
            try
            {
                lstWaras = (from DSD in context.RP_Rotation_DivisionSubDivision
                            where DSD.RPID == _ProgramID
                            orderby DSD.StartDate
                            select DSD).ToList();

                lstWarasIterations.AddRange(lstWaras); // for iteration purpose

                if (lstWarasIterations.Count() == 0)
                    Result = true;

                // case when start and end date is exactly same as one ware date 

                obj = lstWaras.Where(q => q.StartDate == _StartDate && q.EndDate == _EndDate).FirstOrDefault();
                if (obj != null)
                {
                    obj = ClosureObject(obj, null, null);
                    context.SaveChanges();
                    Result = true;
                }
                if (!Result)
                {
                    foreach (var v in lstWarasIterations)
                    {
                        Result = false;
                        if (_StartDate >= v.StartDate && _EndDate <= v.EndDate) // closure within one wara
                        {
                            if ((_StartDate.Day - v.StartDate.Value.Day) >= 1 && (v.EndDate.Value.Day - _EndDate.Day) >= 1)
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                newObj1 = CopyObject(obj, newObj1, obj.StartDate, _StartDate.AddDays(-1));  // before closure 
                                newObj2 = CopyObject(obj, newObj2, _EndDate.AddDays(1), obj.EndDate); // after closure
                                obj = ClosureObject(obj, _StartDate, _EndDate); // closure period                                
                                context.RP_Rotation_DivisionSubDivision.Add(newObj1);
                                context.RP_Rotation_DivisionSubDivision.Add(newObj2);
                                context.SaveChanges();
                                Result = true;
                            }
                            else if ((_StartDate.Day - v.StartDate.Value.Day) >= 1 && v.EndDate == _EndDate)
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                newObj1 = CopyObject(obj, newObj1, obj.StartDate, _StartDate.AddDays(-1));  // before closure 
                                obj = ClosureObject(obj, _StartDate, null); // closure period
                                context.RP_Rotation_DivisionSubDivision.Add(newObj1);
                                context.SaveChanges();
                                Result = true;
                            }
                            else if (v.StartDate == _StartDate && (v.EndDate.Value.Day - _EndDate.Day) >= 1)
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                newObj2 = CopyObject(obj, newObj2, _EndDate.AddDays(1), obj.EndDate); // after closure
                                obj = ClosureObject(obj, null, _EndDate); // closure period
                                context.RP_Rotation_DivisionSubDivision.Add(newObj2);
                                context.SaveChanges();
                                Result = true;
                            }
                            break;
                        }
                        else if (_StartDate > v.StartDate && _StartDate < v.EndDate) // closure in multiple wara
                        {
                            Delimeter = true;
                            obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                            newObj1 = CopyObject(obj, newObj1, obj.StartDate, _StartDate.AddDays(-1));  // before closure 
                            context.RP_Rotation_DivisionSubDivision.Add(newObj1);
                            lstIDs.Add(v.ID);
                        }
                        else if (_StartDate == v.StartDate) // closure in multiple wara
                        {
                            Delimeter = true;
                            obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                            lstIDs.Add(v.ID);
                        }
                        else if (Delimeter)
                        {
                            if (_EndDate < v.EndDate) //|| _EndDate == v.StartDate
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                newObj2 = CopyObject(obj, newObj2, _EndDate.AddDays(1), obj.EndDate); // after closure
                                obj = ClosureObject(obj, _StartDate, _EndDate); // closure period
                                context.RP_Rotation_DivisionSubDivision.Add(newObj2);
                                Delimeter = false;

                                foreach (var ID in lstIDs)
                                    context.RP_Rotation_DivisionSubDivision.RemoveRange(context.RP_Rotation_DivisionSubDivision.Where(q => q.ID == ID));
                                Result = true;
                                break;
                            }
                            else if (_EndDate == v.EndDate)
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                obj = ClosureObject(obj, _StartDate, _EndDate); // closure period
                                Delimeter = false;

                                foreach (var ID in lstIDs)
                                    context.RP_Rotation_DivisionSubDivision.RemoveRange(context.RP_Rotation_DivisionSubDivision.Where(q => q.ID == ID));

                                Result = true;
                                break;
                            }
                            else
                            {
                                lstIDs.Add(v.ID);
                            }
                        }
                        else
                            Result = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Result = false;
            }
            return Result;
        }

        public RP_Rotation_Circle ClosureObjectCircle(RP_Rotation_Circle _obj, DateTime? _startDate, DateTime? _endDate)
        {
            if (_startDate != null)
                _obj.StartDate = _startDate;
            if (_endDate != null)
                _obj.EndDate = _endDate;

            _obj.GroupPreference1 = null;
            _obj.GroupPreference2 = null;
            _obj.GroupPreference3 = null;
            _obj.GroupPreference4 = null;
            _obj.GroupPreference5 = null;

            _obj.PriorityTypeID = 0;

            return _obj;

        }


        public RP_Rotation_Circle CopyObjectCircle(RP_Rotation_Circle _obj, RP_Rotation_Circle _newObj, DateTime? _startDate, DateTime? _endDate)
        {
            _newObj.RPID = _obj.RPID;
            _newObj.StartDate = _startDate;
            _newObj.EndDate = _endDate;
            _newObj.GroupPreference1 = _obj.GroupPreference1;
            _newObj.GroupPreference2 = _obj.GroupPreference2;
            _newObj.GroupPreference3 = _obj.GroupPreference3;
            _newObj.GroupPreference4 = _obj.GroupPreference4;
            _newObj.GroupPreference5 = _obj.GroupPreference5;
            _newObj.PriorityTypeID = _obj.PriorityTypeID;
            _newObj.CreatedDate = DateTime.Now;
            _newObj.CreatedBy = _obj.CreatedBy;
            _newObj.ModifiedDate = DateTime.Now;
            _newObj.ModifiedBy = _obj.ModifiedBy;

            return _newObj;
        }


        public bool ManageClosureDatesCircle(long _ProgramID, DateTime _StartDate, DateTime _EndDate)
        {
            List<RP_Rotation_Circle> lstWaras = new List<RP_Rotation_Circle>();
            List<RP_Rotation_Circle> lstWarasIterations = new List<RP_Rotation_Circle>();
            RP_Rotation_Circle obj = new RP_Rotation_Circle();
            RP_Rotation_Circle newObj1 = new RP_Rotation_Circle();
            RP_Rotation_Circle newObj2 = new RP_Rotation_Circle();
            RP_Rotation_Circle closureObj = new RP_Rotation_Circle();
            List<long> lstIDs = new List<long>();
            bool Result = false;
            bool Delimeter = false;
            try
            {
                lstWaras = (from DSD in context.RP_Rotation_Circle
                            where DSD.RPID == _ProgramID
                            orderby DSD.StartDate
                            select DSD).ToList();

                lstWarasIterations.AddRange(lstWaras); // for iteration purpose

                if (lstWaras.Count() == 0)
                    Result = true;

                // case when start and end date is exactly same as one ware date 

                obj = lstWaras.Where(q => q.StartDate == _StartDate && q.EndDate == _EndDate).FirstOrDefault();
                if (obj != null)
                {
                    obj = ClosureObjectCircle(obj, null, null);
                    context.SaveChanges();
                    Result = true;
                }
                if (!Result)
                {
                    foreach (var v in lstWarasIterations)
                    {
                        Result = false;
                        if (_StartDate >= v.StartDate && _EndDate <= v.EndDate) // closure within one wara
                        {
                            if ((_StartDate.Day - v.StartDate.Value.Day) >= 1 && (v.EndDate.Value.Day - _EndDate.Day) >= 1)
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                newObj1 = CopyObjectCircle(obj, newObj1, obj.StartDate, _StartDate.AddDays(-1));  // before closure 
                                newObj2 = CopyObjectCircle(obj, newObj2, _EndDate.AddDays(1), obj.EndDate); // after closure
                                obj = ClosureObjectCircle(obj, _StartDate, _EndDate); // closure period                                
                                context.RP_Rotation_Circle.Add(newObj1);
                                context.RP_Rotation_Circle.Add(newObj2);
                                context.SaveChanges();
                                Result = true;
                            }
                            else if ((_StartDate.Day - v.StartDate.Value.Day) >= 1 && v.EndDate == _EndDate)
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                newObj1 = CopyObjectCircle(obj, newObj1, obj.StartDate, _StartDate.AddDays(-1));  // before closure 
                                obj = ClosureObjectCircle(obj, _StartDate, null); // closure period
                                context.RP_Rotation_Circle.Add(newObj1);
                                context.SaveChanges();
                                Result = true;
                            }
                            else if (v.StartDate == _StartDate && (v.EndDate.Value.Day - _EndDate.Day) >= 1)
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                newObj2 = CopyObjectCircle(obj, newObj2, _EndDate.AddDays(1), obj.EndDate); // after closure
                                obj = ClosureObjectCircle(obj, null, _EndDate); // closure period
                                context.RP_Rotation_Circle.Add(newObj2);
                                context.SaveChanges();
                                Result = true;
                            }
                            break;
                        }
                        else if (_StartDate > v.StartDate && _StartDate < v.EndDate) // closure in multiple wara
                        {
                            Delimeter = true;
                            obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                            newObj1 = CopyObjectCircle(obj, newObj1, obj.StartDate, _StartDate.AddDays(-1));  // before closure 
                            context.RP_Rotation_Circle.Add(newObj1);
                            lstIDs.Add(v.ID);
                        }
                        else if (_StartDate == v.StartDate) // closure in multiple wara
                        {
                            Delimeter = true;
                            obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                            lstIDs.Add(v.ID);
                        }
                        else if (Delimeter)
                        {
                            if (_EndDate < v.EndDate) //|| _EndDate == v.StartDate
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                newObj2 = CopyObjectCircle(obj, newObj2, _EndDate.AddDays(1), obj.EndDate); // after closure
                                obj = ClosureObjectCircle(obj, _StartDate, _EndDate); // closure period
                                context.RP_Rotation_Circle.Add(newObj2);
                                Delimeter = false;

                                foreach (var ID in lstIDs)
                                    context.RP_Rotation_Circle.RemoveRange(context.RP_Rotation_Circle.Where(q => q.ID == ID));
                                Result = true;
                                break;
                            }
                            else if (_EndDate == v.EndDate)
                            {
                                obj = lstWaras.Where(q => q.ID == v.ID).FirstOrDefault();
                                obj = ClosureObjectCircle(obj, _StartDate, _EndDate); // closure period
                                Delimeter = false;

                                foreach (var ID in lstIDs)
                                    context.RP_Rotation_Circle.RemoveRange(context.RP_Rotation_Circle.Where(q => q.ID == ID));

                                Result = true;
                                break;
                            }
                            else
                            {
                                lstIDs.Add(v.ID);
                            }
                        }
                        else
                            Result = true;
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                Result = false;
            }
            return Result;
        }

        #endregion


    }
}
