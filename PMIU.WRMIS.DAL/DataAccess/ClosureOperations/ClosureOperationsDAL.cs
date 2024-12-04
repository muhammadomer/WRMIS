using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.ClosureOperations;
using PMIU.WRMIS.DAL.Repositories.WaterLosses;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.ClosureOperations
{
    public class ClosureOperationsDAL
    {
        ContextDB db = new ContextDB();

        #region Notifications and Alerts
        public CW_GetCWPPublishNotifyData_Result GetClosureWorkPlanPuslishNotifyData(long _CWPID)
        {
            return db.ExtRepositoryFor<ClosureOperationsRepository>().GetClosureWorkPlanPuslishNotifyData(_CWPID); 
        }
        public CW_GetCWProgressNotifyData_Result GetCW_Progress_NotifyData(long _ProgressID)
        {
            return db.ExtRepositoryFor<ClosureOperationsRepository>().GetCW_Progress_NotifyData(_ProgressID); 
        }
        #endregion

        #region Refrence Data
        public List<CW_TechnicalSanctionUnit> GetAllUnit()
        {
            return db.Repository<CW_TechnicalSanctionUnit>().Query().Get().ToList();
        }
        public List<CW_WorkType> GetAllClosureWorkType()
        {
            return db.Repository<CW_WorkType>().Query().Get().ToList();
        }
        public bool AddTchnclSnctnUnit(CW_TechnicalSanctionUnit _UnitMdl)
        {
            _UnitMdl.CreatedDate = DateTime.Now;
            db.Repository<CW_TechnicalSanctionUnit>().Insert(_UnitMdl);
            db.Save();

            return true;
        }
        public bool AddClsurWrkType(CW_WorkType _Mdl)
        {
            int maxID = db.Repository<CW_WorkType>().Query().Get().ToList().Max(x => x.ID);
            _Mdl.ID = (maxID + 1);
            _Mdl.CreatedDate = DateTime.Now;
            _Mdl.IsOther = true;
            db.Repository<CW_WorkType>().Insert(_Mdl);
            db.Save();

            return true;
        }
        public bool UpdateTchnclSnctnUnit(CW_TechnicalSanctionUnit _Unit)
        {
            CW_TechnicalSanctionUnit mdlUnit = db.Repository<CW_TechnicalSanctionUnit>().FindById(_Unit.ID);

            mdlUnit.Name = _Unit.Name;
            mdlUnit.Description = _Unit.Description;
            mdlUnit.ModifiedDate = DateTime.Now;
            mdlUnit.ModifiedBy = _Unit.ModifiedBy;
            mdlUnit.IsActive = _Unit.IsActive;
            db.Repository<CW_TechnicalSanctionUnit>().Update(mdlUnit);
            db.Save();

            return true;
        }
        public bool UpdateClsurWrkType(CW_WorkType _Work)
        {
            CW_WorkType mdl = db.Repository<CW_WorkType>().FindById(_Work.ID);
            mdl.Name = _Work.Name;
            mdl.Description = _Work.Description;
            mdl.ModifiedBy = _Work.ModifiedBy;
            mdl.ModifiedDate = DateTime.Now;
            mdl.IsActive = _Work.IsActive;
            db.Repository<CW_WorkType>().Update(mdl);
            db.Save();

            return true;
        }
        public bool DeleteTchnclSnctnUnit(long _ID)
        {
            db.Repository<CW_TechnicalSanctionUnit>().Delete(_ID);
            db.Save();

            return true;
        }
        public bool DeleteClsurWrkType(long _ID)
        {
            db.Repository<CW_WorkType>().Delete(_ID);
            db.Save();

            return true;
        }
        public bool UnitAssociationExists(long _UnitID)
        {
            List<CW_WorkItem> lstWorkItem = db.Repository<CW_WorkItem>().Query().Get().Where(x => x.TSUnitID == _UnitID).ToList();

            if (lstWorkItem != null && lstWorkItem.Count() > 0)
                return true;

            return false;
        }
        public bool ClosureWorkTypeAssiciatExists(long _ID)
        {
            List<CW_ClosureWork> lstWork = db.Repository<CW_ClosureWork>().Query().Get().Where(x => x.WorkTypeID == _ID).ToList();

            if (lstWork != null && lstWork.Count() > 0)
                return true;

            return false;
        }
        public CW_TechnicalSanctionUnit GetTchnclSnctnUnit_ByName(string _Name)
        {
            return db.Repository<CW_TechnicalSanctionUnit>().GetAll().Where(z => z.IsActive == true
                && z.Name.Trim().ToLower() == _Name.Trim().ToLower()).FirstOrDefault();
        }
        public CW_WorkType GetClsurWrkType_ByName(string _Name)
        {
            return db.Repository<CW_WorkType>().GetAll()
                .Where(z => z.IsActive == true && z.Name.Trim().ToLower() == _Name.Trim().ToLower())
                .FirstOrDefault();
        }

        public List<object> GetAllClosureWorkTypes()
        {
            return db.Repository<CW_WorkType>().GetAll().Where(s => s.IsActive == true).Select(c => new { c.ID, c.Name, c.Description, c.IsActive, Type = "C" }).ToList<object>();

        }

        public List<object> GetAllClosureWorkStatus()
        {
            return db.Repository<CW_WorkStatus>().GetAll().Where(s => s.IsActive == true).Select(c => new { c.ID, c.Name, c.Description, c.IsActive,Type = "C" }).ToList<object>();

        }

        #endregion

        #region Annual Canal CLosure Program
        public List<object> GetACCP(string _Year)
        {
            return db.Repository<CW_AnnualClosureProgram>().Query().Get().Where(x => x.Year.Equals(_Year)).ToList().OrderBy(x => x.ID)
                .Select(x => new { ID = x.ID, Title = x.Title, Year = x.Year, Attachment = x.Attachment })
                .ToList<object>();
        }
        public List<object> GetAllACCP()
        {
            return (from acwp in db.Repository<CW_AnnualClosureProgram>().GetAll() select new { ID = acwp.ID, Title = acwp.Title, Year = acwp.Year, acwp.CreatedDate, CreatedBy = acwp.CreatedBy, Attachment = acwp.Attachment, Description = acwp.Description, ModifiedDate=acwp.ModifiedDate, ModifiedBy = acwp.ModifiedBy }).ToList<object>();
        }
        public List<object> GetACCPYears()
        {
            List<object> lstObj = new List<object>();
            lstObj = db.Repository<CW_AnnualClosureProgram>().Query().Get().ToList().OrderByDescending(x => x.ID).Select(x => new { ID = x.Year, Name = x.Year ,Type = "C"}).ToList<object>();
            return lstObj.OrderByDescending(x => Convert.ToString(x.GetType().GetProperty("Name").GetValue(x))).ToList();
        }
        public CW_AnnualClosureProgram GetACCP_ByYear(string _Year)
        {
            return db.Repository<CW_AnnualClosureProgram>().GetAll()
                .Where(z => z.Year.Equals(_Year))
                .FirstOrDefault();
        }
        public CW_AnnualClosureProgram GetACCP_ByID(long _ID)
        {
            return db.Repository<CW_AnnualClosureProgram>().GetAll()
                .Where(z => z.ID == _ID)
                .FirstOrDefault();
        }
        public bool AddACCP(CW_AnnualClosureProgram _ACCPMdl, out int _acpID)
        {
            db.Repository<CW_AnnualClosureProgram>().Insert(_ACCPMdl);
            db.Save();
            _acpID = (int)_ACCPMdl.ID;
            return true;
        }
        public bool UpdateACCP(CW_AnnualClosureProgram _ACCP)
        {
            CW_AnnualClosureProgram mdlAccp = db.Repository<CW_AnnualClosureProgram>().FindById(_ACCP.ID);

            mdlAccp.Title = _ACCP.Title;
            mdlAccp.Year = _ACCP.Year;
            mdlAccp.Attachment = _ACCP.Attachment==string.Empty ? mdlAccp.Attachment : _ACCP.Attachment;
            mdlAccp.ModifiedDate = DateTime.Now;
            mdlAccp.ModifiedBy = _ACCP.ModifiedBy;
            db.Repository<CW_AnnualClosureProgram>().Update(mdlAccp);
            db.Save();

            return true;
        }
        #endregion

        #region Annual Canal  Closure Programme Detail
        public List<object> GetTCandMCByACCPID(long _ACCPID)
        {
            List<object> lstObj = new List<object>();
            lstObj = (from cwpd in db.Repository<CW_AnnualClosureProgramDetail>().GetAll()
                      join chnl in db.Repository<CO_Channel>().GetAll() on cwpd.ChannelID equals chnl.ID
                      where cwpd.ACPID == _ACCPID
                      select new
                      {
                          ID = cwpd.ID,
                          ChannelID = cwpd.ChannelID,
                          CreatedBy = cwpd.CreatedBy,
                          ChannelCmdType = cwpd.CommandType,
                          MainCanalName = chnl.NAME,
                          MainCanalID = chnl.ID,
                          CreatedDate = cwpd.CreatedDate,
                          ACCPID = cwpd.ACPID,
                          FromDate = cwpd.FromDate,
                          ToDate = cwpd.ToDate,
                          ModifiedBy = cwpd.ModifiedBy,
                          ModifiedDate = cwpd.ModifiedDate,

                      }
              ).ToList<object>();
            return lstObj;
        }
        public object GetACCPDetail_ByID(long _ID)
        {
            object Obj = new object();
            Obj = (from obj in db.Repository<CW_AnnualClosureProgramDetail>().GetAll()
                   where obj.ID == _ID
                   select new
                   {
                       ID = obj.ID,
                       ChannelID = obj.ChannelID,
                       ACPID = obj.ACPID,
                       FromDate = obj.FromDate,
                       ToDate = obj.ToDate,
                       CreatedDate = obj.CreatedDate,
                       CreatedBy = obj.CreatedBy,
                       ModifiedDate = obj.ModifiedDate,
                       ModifiedBy = obj.ModifiedBy,
                       ACCPID = obj.ACPID,
                   }
                ).FirstOrDefault<object>();
            return Obj;

        }
        public bool AddACCPDetail(CW_AnnualClosureProgramDetail _ACCPOrderMdl)
        {
            db.Repository<CW_AnnualClosureProgramDetail>().Insert(_ACCPOrderMdl);
            db.Save();
            return true;
        }
        public bool UpdateACCPDetail(CW_AnnualClosureProgramDetail _ACCPDetial)
        {
            CW_AnnualClosureProgramDetail mdlAccpDetial = db.Repository<CW_AnnualClosureProgramDetail>().FindById(_ACCPDetial.ID);
            mdlAccpDetial.ChannelID = _ACCPDetial.ChannelID;
            mdlAccpDetial.ACPID = _ACCPDetial.ACPID;
            mdlAccpDetial.CommandType = _ACCPDetial.CommandType;
            mdlAccpDetial.FromDate = _ACCPDetial.FromDate;
            mdlAccpDetial.ToDate = _ACCPDetial.ToDate;
            mdlAccpDetial.ModifiedBy = _ACCPDetial.ModifiedBy;
            mdlAccpDetial.ModifiedDate = _ACCPDetial.ModifiedDate;
            db.Repository<CW_AnnualClosureProgramDetail>().Update(mdlAccpDetial);
            db.Save();
            return true;
        }
        public bool DeleteACCPDetail(CW_AnnualClosureProgramDetail _ACCPOrder)
        {
            CW_AnnualClosureProgramDetail mdlAccpDetial = db.Repository<CW_AnnualClosureProgramDetail>().FindById(_ACCPOrder.ID);
            db.Repository<CW_AnnualClosureProgramDetail>().Delete(mdlAccpDetial);
            db.Save();
            return true;
        }


        public List<object> GetChannelsByChannelTypeandChannelCommandType(int _ChannelType, int _ChannelCommandType)
        {
            List<object> lstObj = new List<object>();
            lstObj = (from obj in db.Repository<CO_Channel>().GetAll()
                      where obj.ChannelTypeID == _ChannelType && obj.ComndTypeID == _ChannelCommandType
                      select new
                      {
                          ChannelID = obj.ID,
                          ChannelName = obj.NAME
                      }
                ).ToList<object>();
            return lstObj;
        }
        #endregion

        #region Annaual Canal Closure Programme Orders/Letters

        public List<object> GetACCPOrdersByACCPID(long _ACCPID)
        {
            List<object> lstObj = new List<object>();
            lstObj = (from obj in db.Repository<CW_AnnualClosureProgramOrder>().GetAll()
                      where obj.ACPID == _ACCPID
                      select new
                      {
                          ID = obj.ID,
                          FileName = obj.Attachment,
                          LatterNo = obj.LetterNo,
                          LatterDate = obj.LetterDate,
                          ACCPOrderID = obj.ID,
                          CreatedDate = obj.CreatedDate,
                          CreatedBy = obj.CreatedBy,
                          ModifiedDate = obj.ModifiedDate,
                          ModifiedBy = obj.ModifiedBy,
                          ACCPID = obj.ACPID,
                      }
                ).ToList<object>();
            return lstObj;

        }
        public object GetACCPOrder_ByID(long _ID)
        {
            object Obj = new object();
            Obj = (from obj in db.Repository<CW_AnnualClosureProgramOrder>().GetAll()
                   where obj.ID == _ID
                   select new
                   {
                       ID = obj.ID,
                       FileName = obj.Attachment,
                       LatterNo = obj.LetterNo,
                       LatterDate =   obj.LetterDate ,
                       ACCPOrderID = obj.ID,
                       CreatedDate = obj.CreatedDate,
                       CreatedBy = obj.CreatedBy,
                       ModifiedDate = obj.ModifiedDate,
                       ModifiedBy = obj.ModifiedBy,
                       ACCPID = obj.ACPID,
                   }
                ).FirstOrDefault<object>();
            return Obj;

        }


        public bool AddACCPOrders(CW_AnnualClosureProgramOrder _ACCPOrderMdl)
        {
            db.Repository<CW_AnnualClosureProgramOrder>().Insert(_ACCPOrderMdl);
            db.Save();
            return true;
        }
        public bool UpdateACCPOrder(CW_AnnualClosureProgramOrder _ACCPOrder)
        {
            CW_AnnualClosureProgramOrder mdlAccpOrder = db.Repository<CW_AnnualClosureProgramOrder>().FindById(_ACCPOrder.ID);
            mdlAccpOrder.LetterNo = _ACCPOrder.LetterNo;
            mdlAccpOrder.LetterDate = _ACCPOrder.LetterDate;
            if (!string.IsNullOrEmpty(_ACCPOrder.Attachment))
            {
                string filepath = Utility.GetImageURL(Configuration.ClosureOperations, mdlAccpOrder.Attachment);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            if (!string.IsNullOrEmpty(_ACCPOrder.Attachment))
                mdlAccpOrder.Attachment = _ACCPOrder.Attachment;
            mdlAccpOrder.ModifiedDate = _ACCPOrder.ModifiedDate;
            mdlAccpOrder.ModifiedBy = _ACCPOrder.ModifiedBy;
            db.Repository<CW_AnnualClosureProgramOrder>().Update(mdlAccpOrder);
            db.Save();
            return true;
        }
        public bool DeleteACCPOrders(CW_AnnualClosureProgramOrder _ACCPOrder)
        {
            CW_AnnualClosureProgramOrder mdlAccpOrder = db.Repository<CW_AnnualClosureProgramOrder>().FindById(_ACCPOrder.ID);
            if (!string.IsNullOrEmpty(mdlAccpOrder.Attachment))
            {
                string filepath = Utility.GetImageURL(Configuration.ClosureOperations, mdlAccpOrder.Attachment);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            db.Repository<CW_AnnualClosureProgramOrder>().Delete(mdlAccpOrder);
            db.Save();
            return true;
        }
        #endregion

        #region Exclude Channels From Annual Canal Closure Programme
        public List<object> GetChannelsforExclude(long _MainChannelID)
        {
            #region commented Code

            List<CO_ChannelParentFeeder> lstCP = db.Repository<CO_ChannelParentFeeder>().GetAll().ToList();
            List<CO_Channel> lstChnl = db.Repository<CO_Channel>().GetAll().ToList();
            List<CO_Channel> FirstFilterdlst = (from ChannelP in lstCP 
                                            join channel in lstChnl on ChannelP.ChannelID equals channel.ID
                                            where ChannelP.RelationType == "P" && channel.ChannelTypeID == 3 && ChannelP.ParrentFeederID == _MainChannelID
                                            select channel ).ToList();
            List<CO_Channel> SndFilterdlst = (from chnaPF in lstCP
                                              join chnl in FirstFilterdlst on chnaPF.ChannelID equals chnl.ID
                                              select chnl
                                                ).ToList();
            List<object> FinalFilterList = new List<object>();
            foreach (CO_Channel item in SndFilterdlst)
	        {
                int p=SndFilterdlst.Count(x=>x.ID==item.ID);
                if (p>1 && !FinalFilterList.Contains(new { ID=item.ID,ChannelName=item.NAME}))
	                {
                        FinalFilterList.Add(new { ID=item.ID,ChannelName=item.NAME});
	                }
	        }
            return FinalFilterList;
            #endregion
        }
        public List<object> GetExcludedChannels(long _DetialID)
        {
            List<object> lstObj = new List<object>();
            lstObj = (from _Xchannel in db.Repository<CW_ACPExcludedChannel>().GetAll()
                      join chnl in db.Repository<CO_Channel>().GetAll() on _Xchannel.ChannelID equals chnl.ID
                      where _Xchannel.ACPDetailID == _DetialID
                      select new
                      {
                          ID = _Xchannel.ChannelID,
                          XChannelName = chnl.NAME
                      }
              ).ToList<object>();
            return lstObj;
        }

        public bool AddACCPExcludeChannels(CW_ACPExcludedChannel _AccpXChannel)
        {
            CW_ACPExcludedChannel mdlAccpOrder = db.Repository<CW_ACPExcludedChannel>().GetAll().FirstOrDefault(x => x.ACPDetailID == _AccpXChannel.ACPDetailID && x.ChannelID == _AccpXChannel.ChannelID);
            if (mdlAccpOrder == null)
            {
                db.Repository<CW_ACPExcludedChannel>().Insert(_AccpXChannel);
                db.Save();
            }
            return true;
        }

        public bool DeleteAccpXchannels(CW_ACPExcludedChannel _ACCPExclude)
        {
            CW_ACPExcludedChannel mdlAccpOrder = db.Repository<CW_ACPExcludedChannel>().GetAll().FirstOrDefault(x => x.ACPDetailID == _ACCPExclude.ACPDetailID && x.ChannelID == _ACCPExclude.ChannelID);
            db.Repository<CW_ACPExcludedChannel>().Delete(mdlAccpOrder);
            db.Save();
            return true;
        }
        #endregion

        #region Closure Work Plan
        public string GetFinancialYearbyCWP(long _CWPID)
        {
            return db.Repository<CW_ClosureWorkPlan>().Query().Get()
                .Where(x => x.ID == _CWPID).SingleOrDefault().Year;
        }
        public bool IsCWPInTender(long _CWPID)
        {
            List<long> lstWIDs = db.Repository<CW_ClosureWork>().Query().Get().Where(x => x.CWPID == _CWPID).Select(x => x.ID).ToList<long>();

            List<TM_TenderWorks> lst = db.Repository<TM_TenderWorks>().Query().Get().Where(x => x.WorkSource.Equals("CLOSURE") && lstWIDs.Contains(x.WorkSourceID)).ToList();

            if (lst == null || lst.Count() == 0)
                return false;
            else
                return true;
            
        }

        public bool IsClosureWorkCostEstimationCorrect(long _WorkID)
        {
            bool result = false;
            long? workCost = db.Repository<CW_ClosureWork>().Query().Get().Where(x => x.ID == _WorkID).FirstOrDefault().EstimatedCost;
            double workItemsCostSum= 0;
            if (workCost != null)
            {
                List<CW_WorkItem> lst = db.Repository<CW_WorkItem>().Query().Get()
                    .Where(x=> x.WorkType.Equals("CLOSURE") && x.WorkID == _WorkID).ToList();
                foreach (var item in lst)
                {
                    double amount = item.SanctionedQty * item.TSRate;
                    workItemsCostSum = workItemsCostSum + amount; 
                }
                if (workCost == workItemsCostSum)
                    result = true; 
            }
            
            return result;
        }
        public object GetWorkProgress(long _ID)
        {
            CW_WorkProgress mdl =  db.Repository<CW_WorkProgress>().Query().Get().Where(x => x.ID == _ID).FirstOrDefault();
            if (mdl != null)
            {
                List<CW_WorkProgressAttachment> lstPrgrsAtchmnt = db.Repository<CW_WorkProgressAttachment>().Query().Get()
                    .Where(x=> x.WorkProgressID == mdl.ID).ToList(); 
                int count = 0; 
                string attchemnt = "" ;
                if(lstPrgrsAtchmnt != null && lstPrgrsAtchmnt.Count > 0)
                {
                    count = lstPrgrsAtchmnt.Count(); 
                    foreach (var i in lstPrgrsAtchmnt)
                        attchemnt = attchemnt + i.Attachment + ";" ;
                }
                object obj = new
                {
                    InspectionDate = Utility.GetFormattedDate( mdl.InspectionDate ),
                    ProgressPercentage = mdl.ProgressPercentage,
                    WorkStatus = mdl.CW_WorkStatus.Name,
                    SiltRemovedQty =  mdl.SiltRemovedQty,
                    Remarks = mdl.Remarks,
                    ChannelDesiltedLen = mdl.ChannelDesiltedLen, 
                    AttachmentCount = count,
                    Attchment = attchemnt
                };
                return obj;
            }
            return null;
        }
        public bool IsCWPComplete(long _CWPID)
        {
            List<long> lstCWrk = db.Repository<CW_ClosureWork>().Query().Get()
                .Where(x => x.CWPID == _CWPID).Select(x => x.ID).ToList();
            if (lstCWrk == null || lstCWrk.Count() <= 0)
                return false;

            foreach (var id in lstCWrk)
            {
                List<CW_WorkItem> lstWItem = db.Repository<CW_WorkItem>().Query().Get()
                    .Where(x => x.WorkType.Equals("CLOSURE") && x.WorkID == id).ToList();
                if (lstWItem == null || lstWItem.Count() <= 0) 
                    return false; 
            }
            return true;
        }
        public bool ClosureWorkAssociationExists(long _CLosureWorkID)
        {
            List<CW_WorkItem> lstData = db.Repository<CW_WorkItem>().Query().Get()
                .Where(x => x.WorkType.Equals("CLOSURE") && x.WorkID == _CLosureWorkID).ToList();
            if (lstData != null && lstData.Count() > 0)
                return true;

            List<CW_WorkProgress> lstWPrgrs = db.Repository<CW_WorkProgress>().Query().Get()
                .Where(x => x.ClosureWorkID == _CLosureWorkID).ToList();

            if (lstWPrgrs != null && lstWPrgrs.Count() > 0)
                return true;

            return false;
        }
        public List<object> GetWorkProgressHistory(int _WorkStatusID, List<long> _InspectedBy, long _WorkID, DateTime? _FromDate, DateTime? _ToDate)
        { 
            List<CW_WorkProgress> lstWorksProgress = db.Repository<CW_WorkProgress>().Query().Get().
                Where(x => x.ClosureWorkID == _WorkID
                   && ((_FromDate == null || DbFunctions.TruncateTime(x.InspectionDate) >= DbFunctions.TruncateTime(_FromDate)))
                    && (_ToDate == null || (DbFunctions.TruncateTime(x.InspectionDate) <= DbFunctions.TruncateTime(_ToDate)))
                    ).OrderByDescending(x => x.ID).ToList();

           if (lstWorksProgress != null && lstWorksProgress.Count() > 0)
               lstWorksProgress = lstWorksProgress.Where(x => _InspectedBy.Contains(x.CreatedByDesigID)).OrderByDescending(x => x.ID).ToList();
 
            if (lstWorksProgress != null && lstWorksProgress.Count() > 0 && _WorkStatusID != 0)
                lstWorksProgress = lstWorksProgress.Where(x => x.WorkStatusID == _WorkStatusID && _InspectedBy.Contains(x.CreatedByDesigID)).OrderByDescending(x => x.ID).ToList();
           
            List<object> lstData = new List<object>();

            if (lstWorksProgress != null && lstWorksProgress.Count() > 0)
            {
                lstData = lstWorksProgress.Select(x => new
                 {
                     ID = x.ID,
                     Date = Utility.GetFormattedDate(x.InspectionDate),
                     InspectedBy = GetUserNameAndDesignation(x.CreatedBy, x.CreatedByDesigID),
                     WorkStatus = x.CW_WorkStatus.Name,
                     PrgPrcntg = x.ProgressPercentage

                 }).ToList<object>();
            }
            return lstData;
        }
        public string GetUserNameAndDesignation(long _UserID, long _DesignationID)
        {
            string name = "";
            UA_Users mdl = db.Repository<UA_Users>().Query().Get().Where(u => u.ID == _UserID).FirstOrDefault();
            if (mdl != null)
            {
                name = mdl.FirstName + " " + mdl.LastName;
                name = name + " (" + db.Repository<UA_Designations>().Query().Get().Where(u => u.ID == _DesignationID).FirstOrDefault().Name + ")";
            }
            return name;
        }
        public void SaveWorkProgress(CW_WorkProgress _Mdl, List<Tuple<string, string, string>> _Attachment)
        {
            db.Repository<CW_WorkProgress>().Insert(_Mdl);

            foreach (var atchmnt in _Attachment)
            {
                CW_WorkProgressAttachment mdl = new CW_WorkProgressAttachment();
                mdl.WorkProgressID = _Mdl.ID;
                mdl.Attachment = atchmnt.Item3;
                mdl.CreatedDate = DateTime.Now;
                mdl.CreatedBy = _Mdl.CreatedBy;

                db.Repository<CW_WorkProgressAttachment>().Insert(mdl);
            }

            db.Save();
        }
        public void UpdateScheduleDetailWorks(long _RefMonitoringID, long _ScheduleDetailID)
        {
            SI_ScheduleDetailWorks mdlWorks = db.Repository<SI_ScheduleDetailWorks>().GetAll().Where(x => x.ID == _ScheduleDetailID).FirstOrDefault();
            mdlWorks.RefMonitoringID = _RefMonitoringID;
            db.Repository<SI_ScheduleDetailWorks>().Update(mdlWorks);
            db.Save();
        }
        public object GetWorkProgressByUser(long _CWID, long _UserID)
        {
            List<CW_WorkProgress> lst = db.Repository<CW_WorkProgress>().Query().Get()
                .Where(x => x.ClosureWorkID == _CWID && x.CreatedBy == _UserID).OrderByDescending(x => x.ID).ToList();
            if (lst != null && lst.Count() > 0)
            {
                CW_WorkProgress mdl = lst.ElementAt(0);
                object obj = new
                {
                    Progress = mdl.ProgressPercentage,
                    Date = Common.Utility.GetFormattedDate(mdl.InspectionDate)
                };

                return obj;
            }
            return null;
        }
        //public object GetWorkProgressByUserScheduled(long _CWID, long _UserID, long _refMonitoringID)
        //{
        //    List<CW_WorkProgress> lst = db.Repository<CW_WorkProgress>().Query().Get()
        //        .Where(x => x.ClosureWorkID == _CWID && x.CreatedBy == _UserID).OrderByDescending(x => x.ID < _refMonitoringID).ToList();
        //    if (lst != null && lst.Count() > 0)
        //    {
        //        CW_WorkProgress mdl = lst.ElementAt(0);
        //        object obj = new
        //        {
        //            Progress = mdl.ProgressPercentage,
        //            Date = Common.Utility.GetFormattedDate(mdl.InspectionDate)
        //        };

        //        return obj;
        //    }
        //    return null;
        //}
        public bool IsWorkOfDesiltingType(long _CWID)
        {
            int type = db.Repository<CW_ClosureWork>().Query().Get().Where(x => x.ID == _CWID).FirstOrDefault().WorkTypeID;
            if (type == 1)
                return true;

            return false;
        }
        public List<object> GetWorkStatusList()
        {
            return db.Repository<CW_WorkStatus>().GetAll().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
        }
        public double? ContractorAmountPerWorkItem(long _ID)
        {
            double? rate = null;
            CW_WorkItem cwItem = db.Repository<CW_WorkItem>().Query().Get()
                .Where(x => x.ID == _ID && x.WorkType.Equals("CLOSURE"))
                .FirstOrDefault();

            TM_TenderWorks mdlTndr = db.Repository<TM_TenderWorks>().Query().Get()
                .Where(x => x.WorkSource.Equals("CLOSURE") && x.WorkSourceID == cwItem.WorkID && x.WorkStatusID == 5)
                .FirstOrDefault();

            if (mdlTndr != null)
            {
                long tndrID = mdlTndr.ID;

                TM_TenderWorksContractors mdlCntrctr = db.Repository<TM_TenderWorksContractors>().Query().Get()
                    .Where(x => x.TenderWorksID == tndrID && x.Awarded == true)
                    .FirstOrDefault();
                if (mdlCntrctr != null)
                {
                    double? cRate  =  db.Repository<TM_TenderPrice>().Query().Get()
                        .Where(x => x.TWContractorID == mdlCntrctr.ID && x.WorkItemID == _ID)
                        .FirstOrDefault().ContractorRate;

                    if (cRate != null)
                    {
                        rate = cwItem.SanctionedQty * cRate;
                    }
                }

            }
            return rate;
        }
        public void DeleteWorkItem(long _ID)
        {
            db.Repository<CW_WorkItem>().Delete(_ID);
            db.Save();
        }
        public void UpdateWorkItem(CW_WorkItem _Obj)
        {
            CW_WorkItem obj = db.Repository<CW_WorkItem>().FindById(_Obj.ID);
            obj.ItemDescription = _Obj.ItemDescription;
            obj.SanctionedQty = _Obj.SanctionedQty;
            obj.TSRate = _Obj.TSRate;
            obj.TSUnitID = _Obj.TSUnitID;
            obj.ModifiedBy = _Obj.ModifiedBy;
            obj.ModifiedDate = _Obj.ModifiedDate;

            db.Repository<CW_WorkItem>().Update(obj);
            db.Save();
        }
        public void SaveWorkItem(CW_WorkItem _Obj)
        {
            db.Repository<CW_WorkItem>().Insert(_Obj);
            db.Save();
        }
        public CW_WorkItem GetClosureWorkItem(string _Name, long _ID, long _WorkID, string _SearchBy, string _AssetWork = "")
        {
            CW_WorkItem item = null;
            if (_SearchBy.Equals("ByID"))
            {
                item = db.Repository<CW_WorkItem>().Query().Get()
                    .Where(x => x.ID == _ID && x.WorkType.Equals(_AssetWork == "" ? "CLOSURE" : _AssetWork) && x.WorkID == _WorkID)
                    .FirstOrDefault();
            }
            else if (_SearchBy.Equals("ByName"))
            {
                item = db.Repository<CW_WorkItem>().Query().Get()
                    .Where(x => x.ItemDescription.ToUpper().Equals(_Name.ToUpper()) && x.WorkType.Equals(_AssetWork == "" ? "CLOSURE" : _AssetWork) && x.WorkID == _WorkID)
                    .FirstOrDefault();
            }
            return item;
        }
        public List<object> GetClosureWorkItems_ByCWID(long _CWID, string _SourceType = "")
        {
            List<CW_WorkItem> lstData = db.Repository<CW_WorkItem>().Query().Get()
                .Where(x => x.WorkType.ToUpper().Equals(_SourceType == "" ? "CLOSURE" : _SourceType) && x.WorkID == _CWID)
                .ToList();

            return lstData.Select(x => new
            {
                ID = x.ID,
                ItemDescription = x.ItemDescription,
                SanctionedQty = x.SanctionedQty,
                UnitID = x.TSUnitID,
                Unit = GetUnitNameByID(x.TSUnitID),
                TSRate = x.TSRate,
                TSAmount = Utility.GetRoundOffValue((x.TSRate * x.SanctionedQty))
            }
                )
                .ToList<object>();
        }
        private string GetUnitNameByID(long _ID)
        {
            return db.Repository<CW_TechnicalSanctionUnit>().Query().Get().Where(x => x.ID == _ID).FirstOrDefault().Name;
        }
        public object GetCWDetailByID(long _CWID)
        {
            CW_ClosureWork mdlCW = db.Repository<CW_ClosureWork>().FindById(_CWID);

            object obj = new
            {
                CWPTitle = mdlCW.CW_ClosureWorkPlan.Title,
                CWPYear = mdlCW.CW_ClosureWorkPlan.Year,
                CWPDivision = GetDivisionNameByID(Convert.ToInt64(mdlCW.CW_ClosureWorkPlan.DivisionID)),
                CWTitle = mdlCW.WorkName,
                CWWorkType = mdlCW.CW_WorkType.Name,
                CWCost = Utility.GetRoundOffValue (mdlCW.EstimatedCost )
            };

            return obj;
        }


        public bool DeleteClosureWork(long _CWID)
        {
            CW_ClosureWork mdl = db.Repository<CW_ClosureWork>().FindById(_CWID);
            if (mdl != null)
            {//TODO: might add worksource column for deletion
                List<long> lstID = db.Repository<CW_WorkItem>().Query().Get().Where(x => x.WorkID == mdl.ID).Select(x => x.ID).ToList();
                if (lstID != null && lstID.Count() > 0)
                {
                    foreach (var ID in lstID)
                        db.Repository<CW_WorkItem>().Delete(ID);
                }
            }
            else
                return false;

            mdl = null;
            db.Repository<CW_ClosureWork>().Delete(_CWID);
            db.Save();

            return true;
        }

        public double? ContractorAmount(long _CWID)
        {
            double? rate = null;
            TM_TenderWorks mdlTndr = db.Repository<TM_TenderWorks>().Query().Get()
                .Where(x => x.WorkSource.Equals("CLOSURE") && x.WorkSourceID == _CWID && x.WorkStatusID == 5).FirstOrDefault();
            if (mdlTndr != null)
            {
                long tndrID = mdlTndr.ID;

                TM_TenderWorksContractors mdlCntrctr = db.Repository<TM_TenderWorksContractors>().Query().Get()
                    .Where(x => x.TenderWorksID == tndrID && x.Awarded == true).FirstOrDefault();
                if (mdlCntrctr != null)
                {
                    rate = mdlCntrctr.TenderWorkAmount;//db.Repository<TM_TenderPrice>().GetAll().Where(x => x.TWContractorID == mdlCntrctr.ID).ToList().Sum(x => x.ContractorRate);
                }

            }
            return rate;
        }
        public bool IsTenderAwarded(long _CWID)
        {
            bool rate = false;
            TM_TenderWorks mdlTndr = db.Repository<TM_TenderWorks>().Query().Get()
                .Where(x => x.WorkSource.Equals("CLOSURE") && x.WorkSourceID == _CWID && x.WorkStatusID == 5).FirstOrDefault();
            if (mdlTndr != null)
            {
                long tndrID = mdlTndr.ID;

                TM_TenderWorksContractors mdlCntrctr = db.Repository<TM_TenderWorksContractors>().Query().Get()
                    .Where(x => x.TenderWorksID == tndrID && x.Awarded == true).FirstOrDefault();
                if (mdlCntrctr != null)
                    rate = mdlCntrctr.Awarded == null ? false : Convert.ToBoolean(mdlCntrctr.Awarded);
            }
            return rate;
        }
        public bool PublishCWP(long _CWPID, long _UserID)
        {
            CW_ClosureWorkPlan mdl = db.Repository<CW_ClosureWorkPlan>().Query().Get().Where(x => x.ID == _CWPID).FirstOrDefault();
            if (mdl != null)
            {
                mdl.Status = "Publish";
                mdl.ModifiedBy = Convert.ToInt32(_UserID);
                mdl.ModifiedDate = DateTime.Now;

                db.Repository<CW_ClosureWorkPlan>().Update(mdl);
                db.Save();
            }
            return true;
        }
        public List<object> GetClosureWorksByCWPID(long _CWPID)
        {
            return  db.Repository<CW_ClosureWork>().Query().Get().Where(x => x.CWPID == _CWPID).ToList()
                .Select(x => new { 
                    x.ID, 
                    x.WorkName,
                    EstimatedCost =  Utility.GetRoundOffValue(x.EstimatedCost), 
                    WorkType = x.CW_WorkType.Name ,
                    SnctAmnt = GetSanctionedAmountByWork(x.ID)
                })
                .ToList<object>();

        }
        public string GetSanctionedAmountByWork(long _WorkID)
        {
            string value = string.Empty;
            List<object> temp = GetClosureWorkItems_ByCWID(_WorkID);
            if (temp != null && temp.Count > 0)
                value = Utility.GetRoundOffValue(
                    temp
                    .AsEnumerable<object>()
                    .Sum(x =>
                            Convert.ToDecimal(
                            x.GetType().GetProperty("TSAmount").GetValue(x, null))
                            ).ToString());
            else
                value = Utility.GetRoundOffValue("");

            return value;
        }
        public bool isCWP_Published(long _CWPID)
        {
            CW_ClosureWorkPlan mdl = db.Repository<CW_ClosureWorkPlan>().Query().Get().Where(x => x.ID == _CWPID).FirstOrDefault();
            if (mdl != null)
            {
                if (mdl.Status.ToLower().Equals("publish"))
                    return true;
            }
            return false;
        }
        public object GetCWPDetailByID(long _CWPID)
        {
            return db.Repository<CW_ClosureWorkPlan>().Query().Get().Where(x => x.ID == _CWPID)
                .Select(x => new { x.Title, x.Year, x.DivisionID })
                .ToList()
                .Select(x =>
                    new
                    {
                        Title = x.Title,
                        Year = x.Year,
                        Division = GetDivisionNameByID(Convert.ToInt64(x.DivisionID)) 
                    }
                ).ToList().ElementAt(0);
        }
        public string GetDivisionNameByID(long _DivisionID)
        {
            return db.Repository<CO_Division>().FindById(_DivisionID).Name;
        }
        public CW_ClosureWork GetCW_ByID(long _ClosueWorkID)
        {
            return db.Repository<CW_ClosureWork>().GetAll().Where(z => z.ID == _ClosueWorkID).FirstOrDefault();
        }
        public CW_ClosureWork GetCW_ByName_CWPID_WorkType(long? _ClosueWorkPlanID, string _WorkName, int _WorkTypeID)
        {
            return db.Repository<CW_ClosureWork>().GetAll().Where(z =>
                z.WorkName.Trim().ToLower() == _WorkName.Trim().ToLower() &&
                z.WorkTypeID == _WorkTypeID &&
                z.CWPID == _ClosueWorkPlanID).FirstOrDefault();
        }
        public bool UpdateClosureWork(CW_ClosureWork _Mdl)
        {
            CW_ClosureWork mdl = db.Repository<CW_ClosureWork>().FindById(_Mdl.ID);
            mdl.ID = _Mdl.ID;
            mdl.CWPID = _Mdl.CWPID;
            mdl.WorkName = _Mdl.WorkName;
            mdl.FundingSourceID = _Mdl.FundingSourceID;
            mdl.WorkTypeID = _Mdl.WorkTypeID;

            mdl.DS_ChannelID = _Mdl.DS_ChannelID;
            mdl.DS_SiltRemoved = _Mdl.DS_SiltRemoved;
            mdl.DS_FromRD = _Mdl.DS_FromRD;
            mdl.DS_ToRD = _Mdl.DS_ToRD;
            mdl.DS_DistrictID = _Mdl.DS_DistrictID;
            mdl.DS_TehsilID = _Mdl.DS_TehsilID;

            mdl.EM_HBChannel = _Mdl.EM_HBChannel;
            mdl.EM_ChannelID = _Mdl.EM_ChannelID;
            mdl.EM_HBID = _Mdl.EM_HBID;

            mdl.BW_GRHut = _Mdl.BW_GRHut;
            mdl.BW_Office = _Mdl.BW_Office;
            mdl.BW_Others = _Mdl.BW_Others;
            mdl.BW_Residence = _Mdl.BW_Residence;
            mdl.BW_RestHouse = _Mdl.BW_RestHouse;

            mdl.OP_GaugeFixing = _Mdl.OP_GaugeFixing;
            mdl.OP_GaugePainting = _Mdl.OP_GaugePainting;
            mdl.OP_OilGreasePaint = _Mdl.OP_OilGreasePaint;
            mdl.OP_Others = _Mdl.OP_Others;
            mdl.OP_SubDivID = _Mdl.OP_SubDivID;
            mdl.OP_SectionID = _Mdl.OP_SectionID;

            mdl.OR_SubDivID = _Mdl.OR_SubDivID;
            mdl.OR_SectionID = _Mdl.OR_SectionID;
            mdl.OR_ChannelID = _Mdl.OR_ChannelID;

            mdl.CS_ChannelID = _Mdl.CS_ChannelID;

            mdl.OW_SubDivID = _Mdl.OW_SubDivID;
            mdl.OW_SectionID = _Mdl.OW_SectionID;

            mdl.EstimatedCost = _Mdl.EstimatedCost;
            mdl.CompletionPeriodFlag = _Mdl.CompletionPeriodFlag;
            mdl.CompletionPeriod = _Mdl.CompletionPeriod;
            mdl.CompletionPeriodUnit = _Mdl.CompletionPeriodUnit;
            mdl.StartDate = _Mdl.StartDate;
            mdl.EndDate = _Mdl.EndDate;

            mdl.SanctionNo = _Mdl.SanctionNo;
            mdl.SanctionDate = _Mdl.SanctionDate;
            mdl.EarnestMoney = _Mdl.EarnestMoney;
            mdl.EarnestMoneyType = _Mdl.EarnestMoneyType;
            mdl.TenderFees = _Mdl.TenderFees;

            mdl.Description = _Mdl.Description;

            mdl.ModifiedBy = _Mdl.ModifiedBy;
            mdl.ModifiedDate = _Mdl.ModifiedDate;

            db.Repository<CW_ClosureWork>().Update(mdl);
            db.Save();

            return true;
        }
        public bool AddClosureWork(CW_ClosureWork _Mdl)
        {
            db.Repository<CW_ClosureWork>().Insert(_Mdl);
            db.Save();

            return true;
        }
        public bool AddCWP(CW_ClosureWorkPlan _CWPMdl)
        {
            bool isSaved = false; 
            db.Repository<CW_ClosureWorkPlan>().Insert(_CWPMdl);
            db.Save();
            isSaved = true;
            return isSaved;
        }
        public long? GetCirleIdByDivisionID(int _DivID)
        {
            CO_Division mdCo = db.Repository<CO_Division>().FindById(_DivID);
            if (mdCo != null && mdCo.ID > 0)
                return mdCo.CircleID;
            else
                return 0;

        }
        public bool UpdateCWP(CW_ClosureWorkPlan _CWPMdl)
        {
            bool isSaved = false;
            CW_ClosureWorkPlan _Cwp = db.Repository<CW_ClosureWorkPlan>().FindById(_CWPMdl.ID);
            _Cwp.Title = _CWPMdl.Title;
            _Cwp.Comments = _CWPMdl.Comments;
            _Cwp.Year = _CWPMdl.Year;
            _Cwp.ACPID = _CWPMdl.ACPID;
            _Cwp.DivisionID = _CWPMdl.DivisionID;
            _Cwp.ModifiedDate = _CWPMdl.ModifiedDate;
            _Cwp.ModifiedBy = _CWPMdl.ModifiedBy;
            db.Repository<CW_ClosureWorkPlan>().Update(_Cwp);
            db.Save();
            isSaved = true;
            return isSaved;
        }
        public bool DeleteCWP(CW_ClosureWorkPlan _CWPMdl)
        {
            bool isSaved = false;
            db.Repository<CW_ClosureWorkPlan>().Delete(_CWPMdl);
            db.Save();
            isSaved = true;
            return isSaved;
        }

        //TODO: Removed unused code

        public bool IsClosureWorkPlanAlreadyExist(CW_ClosureWorkPlan _CWPMdl)
        {
            bool IsCWPExist = db.Repository<CW_ClosureWorkPlan>().GetAll().Any(i => i.Year == _CWPMdl.Year
                             && i.DivisionID == _CWPMdl.DivisionID && (i.ID != _CWPMdl.ID || _CWPMdl.ID == 0));
            return IsCWPExist; 
        }
        public List<object> GetUniqueYearFromCWP()
        {
            List<CW_AnnualClosureProgram> lstData = db.Repository<CW_AnnualClosureProgram>().Query().Get().ToList();
            
            if (lstData == null || lstData.Count <= 0)
                return null; 

            return lstData.OrderByDescending(x => x.Year).Select(x => new { ID = x.Year, Name = x.Year }).ToList<object>();
        }
        public List<object> GetOldestClosureWorkPlan()
        {
            return db.Repository<CW_AnnualClosureProgram>().Query().Get()
                .OrderByDescending(x => x.Year).Select(x => new { ID = x.ID, Name = x.Year }).ToList<object>(); 
        }
        public CW_ClosureWorkPlan GetCWPBy_Year_Name(string _Year, long _DivisionID)
        {
            return
                db.Repository<CW_ClosureWorkPlan>().GetAll()
                .Where(z => z.Year == _Year && z.DivisionID == _DivisionID)
                .FirstOrDefault();


        }
        public CW_ClosureWorkPlan GetClosureWorkPlanByID(long _CWPID)
        {
            CW_ClosureWorkPlan closureWorkPlan = (from cwp in db.Repository<CW_ClosureWorkPlan>().GetAll() where cwp.ID == _CWPID select cwp).FirstOrDefault();
            return closureWorkPlan;
        }
        public bool ClosureWorkPlandDependency(long _CWPID)
        {
            bool isDependent = false;
            CW_ClosureWork _cw = (from cw in db.Repository<CW_ClosureWork>().GetAll() where cw.CWPID == _CWPID select cw).FirstOrDefault();
            if (_cw != null && _cw.ID > 0)
            {
                isDependent = true;
            }
            return isDependent;

        }
        public bool DeleteClosureWorkPlan(long _CWPID)
        {
            bool isDeleted = false;
            db.Repository<CW_ClosureWorkPlan>().Delete(_CWPID);
            db.Save();
            isDeleted = true;
            return isDeleted;

        }
        public List<object> GetClosureClosureWorkPlans()
        {
            return db.ExtRepositoryFor<ClosureOperationsRepository>().GetClosureWorkPlans();
        }

        public List<object> GetCWP_By_DivID_Year(List<long> _DivisionID, string _Year , string _Status)
        { 
            List<CW_ClosureWorkPlan> lstCWP =  db.Repository<CW_ClosureWorkPlan>().Query().Get()
            .ToList()
            .OrderBy(x => x.CreatedDate)
            .ToList();

            if(lstCWP == null || lstCWP.Count() <= 0)
                return new List<object>().Select(x => new
                {
                    DivisionName = "",
                    ID = "",
                    Title = "",
                    Year = "",
                    Status = "",
                    DivisionID = ""
                })
            .ToList<object>();

            if(_DivisionID != null && _DivisionID.Count() > 0)
                lstCWP = lstCWP.Where(x => _DivisionID.Contains(x.DivisionID)).ToList();

            if (!_Year.ToUpper().Equals("ALL")) 
                lstCWP = lstCWP.Where(x => x.Year.Equals(_Year)).ToList();

            if (lstCWP == null || lstCWP.Count() <= 0)
                return new List<object>().Select(x => new
                {
                    DivisionName = "",
                    ID = "",
                    Title = "",
                    Year = "",
                    Status = "",
                    DivisionID = ""
                })
            .ToList<object>();

            if (!string.IsNullOrEmpty(_Status)) 
                lstCWP = lstCWP.Where(x => x.Status.ToUpper().Equals(_Status)).ToList();

            if (lstCWP == null || lstCWP.Count() <= 0)
                return new List<object>().Select(x => new
                {
                    DivisionName ="", 
                    ID = "",
                    Title ="" ,
                    Year = "",
                    Status = "",
                    DivisionID = ""
                })
            .ToList<object>();

            lstCWP.OrderByDescending(x => x.Year);
            return lstCWP.Select(x => new
            {
                DivisionName = GetDivisionNameByID(x.DivisionID),
                ID = x.ID,
                Title = x.Title,
                Year = x.Year,
                Status = (x.Status.Equals("Publish") ? "Published" : x.Status ),
                DivisionID = x.DivisionID
            })
            .ToList<object>();
                
        }

        #endregion 

        #region Helper Mothods
        public string GetDesignationByID(long _DesiganationID)
        {
            return db.Repository<UA_Designations>().Query().Get()
                .Where(x => x.ID == _DesiganationID)
                .FirstOrDefault().Name;
        }

        public int? GetStartRDofChannelForDivision(long _XENUserID, long _ChannelID)
        {
            long? divID = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _XENUserID).FirstOrDefault().IrrigationBoundryID;
            if (divID != null)
            {
                ClosureOperationsRepository repCO = db.ExtRepositoryFor<ClosureOperationsRepository>();
                return repCO.GetStartRDRangeByDivision(Convert.ToInt64(divID), _ChannelID);
            }
            return null;
        }
        public int? GetEndRDofChannelForDivision(long _XENUserID, long _ChannelID)
        {
            long? divID = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _XENUserID).FirstOrDefault().IrrigationBoundryID;
            if (divID != null)
            {
                ClosureOperationsRepository repCO = db.ExtRepositoryFor<ClosureOperationsRepository>();
                return repCO.GetEndRDRangeByDivision(Convert.ToInt64(divID), _ChannelID);
            }
            return null;
        }
        public List<object> GetBarragesByXEN(long _XENUserID)
        {
            List<object> lstData = new List<object>();
            long? divID = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _XENUserID).FirstOrDefault().IrrigationBoundryID;
            if (divID != null)
            {
                lstData = db.Repository<FO_StructureIrrigationBoundaries>().Query().Get()
                     .Where(x => x.DivisionID == divID && ( x.StructureTypeID == 1 || x.StructureTypeID == 2 ))
                     .Select(x => new { ID = x.StructureID })
                     .ToList()
                     .Select(x => new { ID = x.ID, Name = db.Repository<CO_Station>().FindById(x.ID).Name })
                     .OrderBy(x => x.Name)
                     .ToList<object>();
            }
            return lstData;
        }
        public List<object> GetSubdivisionByXEN(long _XENUserID)
        {
            List<object> lstData = new List<object>();
            long? divID = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _XENUserID).FirstOrDefault().IrrigationBoundryID;
            if (divID != null)
                lstData = db.Repository<CO_SubDivision>().Query().Get().Where(x => x.DivisionID == divID).Distinct().OrderBy(x => x.Name).ToList().Select(x => new { ID = x.ID, Name = x.Name }).ToList<object>();
            return lstData;
        }
        public List<object> GetSectionBySubDivID(long _SubDivID)
        {
            return db.Repository<CO_Section>().Query().Get()
                .Where(x => x.SubDivID == _SubDivID).Distinct().ToList()
                .Select(x => new { ID = x.ID, Name = x.Name }).OrderBy(x => x.Name).ToList<object>();

        }
        public List<object> GetChannelByXEN(long _XENUserID)
        {
            List<object> lstData = new List<object>();
            long? divID = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _XENUserID).FirstOrDefault().IrrigationBoundryID;
            if (divID != null)
            {
                WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();
                lstData = repWaterLosses.GetChannelsByDivision(Convert.ToInt64(divID));
            }
            return lstData;
        }
        public List<object> GetChannelBySubDivID(long _SubDivID)
        {
            List<object> lstData = new List<object>();
            WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();
            lstData = repWaterLosses.GetChannelsBySubDivision(Convert.ToInt64(_SubDivID));
            return lstData;
        }
        public List<object> GetChannelBySectionID(long _SecID)
        {
            List<object> lstData = new List<object>();
            WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();
            lstData = repWaterLosses.GetChannelBySectionID(_SecID);
            return lstData;
        }
        public List<object> GetStructureByXEN(long _XENUserID)
        {
            List<object> lstData = new List<object>();
            long? divID = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _XENUserID).FirstOrDefault().IrrigationBoundryID;
            if (divID != null)
            {
            }
            return lstData;
        }
        public List<object> GetDistrictByXEN(long _XENUserID)
        {
            List<object> lstData = new List<object>();
            WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();
            long? divID = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _XENUserID).FirstOrDefault().IrrigationBoundryID;
            if (divID != null)
            {
                lstData = repWaterLosses.GetDistrictByDivision(Convert.ToInt64(divID));
            }
            return lstData;
        }
        public List<object> GetTehsilByDistrict(long _DistrictID)
        {
            return db.Repository<CO_Tehsil>().Query().Get()
                 .Where(x => x.DistrictID == _DistrictID && x.IsActive == true)
                 .OrderBy(x => x.Name)
                 .Select(x => new { ID = x.ID, Name = x.Name })
                 .ToList<object>();
        }
        #endregion

        #region Closure Works
        /// <summary>
        /// This method return List of Closure Works by DivisionID and WorkTypeID
        /// Created On 20-12-2016.
        /// </summary>
        /// <returns> List<object></returns>
        public List<object> GetClosureWorksByDivisionID(long? _DivisionID, int? _WorkTypeID,string _Year, int _UserID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[CW_GetClosureWorksData]", _DivisionID, _WorkTypeID,_Year, _UserID);
            List<object> lstClosureWorkDetails = (from DataRow dr in dt.Rows
                                                  select new
                                                  {
                                                      WorkID = Convert.ToInt32(dr["WorkID"]),
                                                      WorkName = dr["WorkName"].ToString(),
                                                      WorkPlanID = Convert.ToInt32(dr["WorkPlanID"]),
                                                      WorkPlanTitle = dr["WorkPlanTitle"].ToString(),
                                                      DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                      YEAR = dr["YEAR"].ToString(),
                                                      Status = dr["Status"].ToString(),
                                                      WorkTypeID = Convert.ToInt32(dr["WorkTypeID"]),
                                                      WorkTypeName = dr["WorkTypeName"].ToString(),
                                                      EstimatedCost = @String.Format(new CultureInfo("ur-PK"), "{0:c}", Convert.ToInt64(dr["EstimatedCost"])),
                                                      WorkProgressID = (dr["WorkProgressID"] == DBNull.Value) ? dr["WorkProgressID"] : Convert.ToInt32(dr["WorkProgressID"]),
                                                      WorkStatusID = (dr["WorkStatusID"] == DBNull.Value) ? dr["WorkStatusID"] : Convert.ToInt32(dr["WorkStatusID"]),
                                                      ProgressPercentage = (dr["ProgressPercentage"] == DBNull.Value) ? dr["ProgressPercentage"] : Convert.ToDouble(dr["ProgressPercentage"]),
                                                      ModifiedDate = (dr["ModifiedDate"] == DBNull.Value) ? dr["ModifiedDate"] : dr["ModifiedDate"].ToString()
                                                      
                                                  }).ToList<object>();
            return lstClosureWorkDetails;
        }

        public long AddClosureWorkProgress(int _WorkProgressID, long _ClosureWorkID, double _ProgressPercentage, int _WorkStatusID, int? _SiltRemovedQty, double? _ChannelDesiltedLen, string _Remarks, List<Tuple<string, string, string>> lstNameofFiles, int _UserID, long _ScheduleDetailID)
        {
            CW_WorkProgress mdlCWProgress = new CW_WorkProgress();
               
                mdlCWProgress.ModifiedBy = Convert.ToInt32(_UserID);
                mdlCWProgress.CreatedBy = Convert.ToInt32(_UserID);
                mdlCWProgress.ModifiedDate = DateTime.Now;
                mdlCWProgress.CreatedDate =  DateTime.Now;
                mdlCWProgress.CreatedByDesigID = Convert.ToInt32(new LoginDAL().GetAndroidUserDesignationID(_UserID));

                mdlCWProgress.ClosureWorkID = _ClosureWorkID;
                mdlCWProgress.ProgressPercentage = _ProgressPercentage;
                mdlCWProgress.WorkStatusID = _WorkStatusID;
                mdlCWProgress.ChannelDesiltedLen = _ChannelDesiltedLen;
                mdlCWProgress.SiltRemovedQty = _SiltRemovedQty;
                mdlCWProgress.InspectionDate = DateTime.Now;
                mdlCWProgress.Remarks = _Remarks;
                SaveWorkProgress(mdlCWProgress, lstNameofFiles);
                if (_ScheduleDetailID != -1)
                {
                    UpdateScheduleDetailWorks(mdlCWProgress.ID, _ScheduleDetailID);
                }
            return mdlCWProgress.ID;
        }
        public void AddWorkProgressAttachment(long _WorkProgressID, int _UserID, string _AttachmentPath)
        {
            CW_WorkProgressAttachment mdlWPAtchmnt = new CW_WorkProgressAttachment();
            mdlWPAtchmnt.WorkProgressID = _WorkProgressID;
            mdlWPAtchmnt.Attachment = _AttachmentPath;
            mdlWPAtchmnt.CreatedBy = _UserID;
            mdlWPAtchmnt.CreatedDate = DateTime.Now;

            db.Repository<CW_WorkProgressAttachment>().Insert(mdlWPAtchmnt);
            db.Save();
        }
        public object GetClosureWorkProgressByID(long _WorkPorgressID)
        {

            return db.ExtRepositoryFor<ClosureOperationsRepository>().GetClosureWorkProgressByID(_WorkPorgressID);

        }
        public bool isWorkProgressAlreadyAdded(long _WorkPorgressID, int _UserID)
        {

            bool isWorkProgressAlreadyAdded = db.Repository<CW_WorkProgress>().GetAll().Any(i => i.ID == _WorkPorgressID && (i.CreatedBy == _UserID || i.ModifiedBy == _UserID));
            return isWorkProgressAlreadyAdded;
        }
        #endregion


        #region Copy Last year ACCP , its Detail and Excluded Channels if there
        public bool CopyLastYearACCP_Detial_ExcludedChannels(string _ACCPTitle, string _Year, string _Attachment, long _CreatedBy)
        {
            int AffactedRows=db.ExecuteNonQuery("CO_ACCPCopyLastYearDetailWithExcluded", _ACCPTitle, _Year, _Attachment, _CreatedBy);
            if (AffactedRows>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<object> GetWorkProgressAttachment(long? _WorkProgressID)
        {
            return db.ExtRepositoryFor<ClosureOperationsRepository>().GetWorkProgressAttachment(_WorkProgressID);
            
        }
        public object GetClosureWorkByID(long _WorkID, long? WorkProgressID, int _UserID)
        {
            DataTable dt = db.ExecuteStoredProcedureDataTable("[CW_GetClosureWorkByID]", _WorkID, WorkProgressID, _UserID);
            object ObjClosureWorkDetails = (from DataRow dr in dt.Rows
                                                  select new
                                                  {
                                                      WorkID = Convert.ToInt32(dr["WorkID"]),
                                                      WorkName = dr["WorkName"].ToString(),
                                                      WorkPlanID = Convert.ToInt32(dr["WorkPlanID"]),
                                                      WorkPlanTitle = dr["WorkPlanTitle"].ToString(),
                                                      DivisionID = Convert.ToInt32(dr["DivisionID"]),
                                                      YEAR = dr["YEAR"].ToString(),
                                                      Status = dr["Status"].ToString(),
                                                      WorkTypeID = Convert.ToInt32(dr["WorkTypeID"]),
                                                      WorkTypeName = dr["WorkTypeName"].ToString(),
                                                      EstimatedCost = @String.Format(new CultureInfo("ur-PK"), "{0:c}", Convert.ToInt64(dr["EstimatedCost"])),
                                                      WorkProgressID = (dr["WorkProgressID"] == DBNull.Value) ? dr["WorkProgressID"] : Convert.ToInt32(dr["WorkProgressID"]),
                                                      WorkStatusID = (dr["WorkStatusID"] == DBNull.Value) ? dr["WorkStatusID"] : Convert.ToInt32(dr["WorkStatusID"]),
                                                      ProgressPercentage = (dr["ProgressPercentage"] == DBNull.Value) ? dr["ProgressPercentage"] : Convert.ToDouble(dr["ProgressPercentage"]),
                                                      ModifiedDate = (dr["ModifiedDate"] == DBNull.Value) ? dr["ModifiedDate"] : dr["ModifiedDate"].ToString(),
                                                      SiltRemovedQty = (dr["SiltRemovedQty"] == DBNull.Value) ? -1 : Convert.ToInt32(dr["SiltRemovedQty"]),
                                                      ChannelDesiltedLen = (dr["ChannelDesiltedLen"] == DBNull.Value) ? -1 : Convert.ToDouble(dr["ChannelDesiltedLen"]),
                                                      Remarks = dr["Remarks"].ToString(),
                                                      IsInspectionAdded = Convert.ToBoolean(dr["IsInspectionAdded"].ToString())
                                                  }).AsEnumerable().Select(q => new
                                                  {
                                                      WorkID = q.WorkID,
                                                      WorkName = q.WorkName,
                                                      WorkPlanID = q.WorkPlanID,
                                                      WorkPlanTitle = q.WorkPlanTitle,
                                                      DivisionID = q.DivisionID,
                                                      YEAR = q.YEAR,
                                                      Status = q.Status,
                                                      WorkTypeID = q.WorkTypeID,
                                                      WorkTypeName = q.WorkTypeName,
                                                      EstimatedCost = q.EstimatedCost,
                                                      WorkProgressID = q.WorkProgressID,
                                                      WorkStatusID = q.WorkStatusID,
                                                      ProgressPercentage = q.ProgressPercentage,
                                                      ModifiedDate = q.ModifiedDate,
                                                      SiltRemovedQty = q.SiltRemovedQty,
                                                      ChannelDesiltedLen = q.ChannelDesiltedLen,
                                                      IsInspectionAdded = q.IsInspectionAdded,
                                                      AttachmentList = GetWorkProgressAttachment(Convert.ToInt64((q.WorkProgressID == DBNull.Value) ? -1 : Convert.ToInt32(q.WorkProgressID))),
                                                      Remarks= q.Remarks
                                                      

                                                  }).FirstOrDefault();
            return ObjClosureWorkDetails;
        }
        #endregion

       
    }
}
