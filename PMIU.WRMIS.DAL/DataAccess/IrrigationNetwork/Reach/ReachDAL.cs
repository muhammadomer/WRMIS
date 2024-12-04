using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.DAL;
using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.Reach;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.Reach
{
    public class ReachDAL
    {
        ContextDB db = new ContextDB();

        #region "Define Channel Reaches"
        public List<object> GetChannelReach(long _ChannelID)
        {
            List<object> lstChannelReach = (from r in db.Repository<CO_ChannelReach>().Query().Get()
                                            where r.ChannelID == _ChannelID && r.IsActive.Value == true
                                            orderby r.ToRD ascending
                                            select r).ToList()
                                                .Select(r => new
                                                {
                                                    ID = r.ID,
                                                    StartingRDTotal = r.FromRD,
                                                    EndingRDTotal = r.ToRD,
                                                    Remarks = r.Remarks,
                                                    StartingRD = Calculations.GetRDText(r.FromRD),
                                                    EndingRD = Calculations.GetRDText(r.ToRD),
                                                    IsActive = r.IsActive.Value
                                                }).ToList<object>();
            return lstChannelReach;
        }
        public bool IsChannelReachChildExists(long _ReachID)
        {
            bool IsExists = false;
            // Check in CO_ChannelReachLSP child entry exits
            bool IsReachLSPExists = db.Repository<CO_ChannelReachLSP>().GetAll().Any(l => l.ReachID == _ReachID);
            // Check in CO_ChannelReachHistory child entry exists
            bool IsReachHistoryExists = db.Repository<CO_ChannelReachHistory>().GetAll().Any(h => h.ReachID == _ReachID);
            if (IsReachLSPExists == true || IsReachHistoryExists == true)
                IsExists = true;

            return IsExists;
        }

        /// <summary>
        /// This function retun channel addition success along with message
        /// Created on 22-10-2015
        /// </summary>
        /// <param name="_Reach"></param>
        /// <returns></returns>
        public bool SaveChannelReach(CO_ChannelReach _Reach)
        {
            bool isSaved = false;

            if (_Reach.ID == 0)
            {
                db.Repository<CO_ChannelReach>().Insert(_Reach);
                db.Save();
                isSaved = true;
            }
            else
            {
                CO_ChannelReach objReach = db.Repository<CO_ChannelReach>().FindById(_Reach.ID);
                objReach.ID = _Reach.ID;
                objReach.ChannelID = _Reach.ChannelID;
                objReach.FromRD = _Reach.FromRD;
                objReach.ToRD = _Reach.ToRD;
                objReach.Remarks = _Reach.Remarks;
                objReach.UpdateDate = _Reach.UpdateDate;
                db.Repository<CO_ChannelReach>().Update(objReach);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }
        private bool SaveChannelReachHistory(CO_ChannelReachHistory _Reach)
        {
            bool isSaved = false;

            db.Repository<CO_ChannelReachHistory>().Insert(_Reach);
            db.Save();
            isSaved = true;


            return isSaved;
        }

        /// <summary>
        /// This function soft delete Channel Reach
        /// Created on 25-01-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteChannelReach(long _ID)
        {
            CO_ChannelReach channelReach = GetChannelReachByID(_ID);
            channelReach.IsActive = false;
            bool isUpdated = SaveChannelReach(channelReach);
            return isUpdated;
        }
        public bool AdjustReach(long _ReachID, long _ChannelID)
        {
            bool IsReachAdjusted = false;
            bool IsChildExists = false;

            dynamic nextReach = null;
            dynamic currentReach = null;

            List<dynamic> lstChannelReach = GetChannelReach(_ChannelID);

            currentReach = lstChannelReach.FirstOrDefault((r => r.ID == _ReachID));
            // Returns:The zero-based index of the first occurrence of an element that matches the
            //     conditions defined by match, if found; otherwise, –1
            int currentReachIndex = lstChannelReach.FindIndex(r => r.ID == _ReachID);
            if ((currentReachIndex + 1) < lstChannelReach.Count)
            {
                // Get next Reach record
                nextReach = lstChannelReach[currentReachIndex + 1];

                // If L Section Parametes exists then push next reach to history
                IsChildExists = IsChannelReachChildExists(nextReach.ID);
                if (IsChildExists)
                {
                    PushReachIntoHistory(nextReach);
                    DeleteChannelReach(currentReach.ID);
                }
                else
                {
                    DeleteChannelReach(nextReach.ID);
                    DeleteChannelReach(currentReach.ID);
                }

                CO_ChannelReach channelReach = PrepareChannelReachEntity(nextReach, _ChannelID, currentReach.StartingRDTotal);
                // Insert next reach 
                IsReachAdjusted = SaveChannelReach(channelReach);
            }
            else
            {
                // If L Section Parametes exists then push next reach to history
                IsChildExists = IsChannelReachChildExists(currentReach.ID);
                if (IsChildExists)
                {
                    PushReachIntoHistory(currentReach);
                }
                else
                    DeleteChannelReach(currentReach.ID);
                IsReachAdjusted = true;
            }
            return IsReachAdjusted;
        }
        public bool EditChannelReach(CO_ChannelReach _Reach)
        {
            bool IsScuccess = false;

            double nextReachEndingRD = 0;
            dynamic nextReach = null;
            dynamic currentReach = null;

            List<dynamic> lstChannelReach = GetChannelReach(_Reach.ChannelID.Value);

            currentReach = lstChannelReach.FirstOrDefault((r => r.ID == _Reach.ID));

            int currentReachIndex = lstChannelReach.FindIndex(r => r.ID == _Reach.ID);

            for (int i = currentReachIndex; i < lstChannelReach.Count; i++)
            {
                if ((i + 1) < lstChannelReach.Count)
                {
                    // Add one to find next record ending RD.
                    nextReachEndingRD = lstChannelReach[i + 1].EndingRDTotal;
                    // Get next Reach record
                    nextReach = lstChannelReach[i + 1];

                    if (_Reach.ToRD < nextReachEndingRD)
                    {
                        PushReachIntoHistory(nextReach);

                        break;
                    }
                    else if (_Reach.ToRD > nextReachEndingRD)
                    {
                        PushReachIntoHistory(nextReach);
                    }
                    else if (_Reach.ToRD == nextReachEndingRD)
                    {
                        PushReachIntoHistory(nextReach);
                    }
                }
            }


            PushReachIntoHistory(currentReach);
            // Set current ReachID to 0 to insert new reach
            _Reach.ID = 0;
            // Insert current reach
            IsScuccess = SaveChannelReach(_Reach);

            // Check next reach Ending RD is bigger than current reach 
            if (nextReach != null && nextReach.EndingRDTotal > _Reach.ToRD)
            {
                CO_ChannelReach channelReach = PrepareChannelReachEntity(nextReach, _Reach.ChannelID.Value, _Reach.ToRD);
                // Insert next reach 
                IsScuccess = SaveChannelReach(channelReach);
            }

            return IsScuccess;
        }
        private bool PushReachIntoHistory(dynamic _Reach)
        {
            bool isDeleted = false;
            CO_ChannelReachHistory NextReachHistory = PrepareReachHistoryEntity(_Reach);

            bool isSaved = SaveChannelReachHistory(NextReachHistory);
            if (isSaved)
            {
                isDeleted = DeleteChannelReach(NextReachHistory.ReachID.Value);
            }
            return isDeleted;
        }
        private CO_ChannelReachHistory PrepareReachHistoryEntity(dynamic _Reach)
        {
            CO_ChannelReachHistory reachHistory = new CO_ChannelReachHistory();
            reachHistory.ReachID = _Reach.ID;
            reachHistory.FromRD = _Reach.StartingRDTotal;
            reachHistory.ToRD = _Reach.EndingRDTotal;
            reachHistory.Remarks = _Reach.Remarks;
            reachHistory.TransferDate = DateTime.Now.Date;
            return reachHistory;
        }
        private CO_ChannelReach PrepareChannelReachEntity(dynamic _Reach, long _ChannelID, int _FromRD)// CO_ChannelReach _ChannelReach)
        {
            CO_ChannelReach channelReach = new CO_ChannelReach();
            channelReach.ChannelID = _ChannelID;
            channelReach.FromRD = _FromRD;
            channelReach.ToRD = _Reach.EndingRDTotal;
            channelReach.Remarks = _Reach.Remarks;
            channelReach.UpdateDate = DateTime.Now.Date;
            channelReach.IsActive = true;
            return channelReach;
        }

        private CO_ChannelReach GetChannelReachByID(long _ID)
        {
            CO_ChannelReach qChannelReach = db.Repository<CO_ChannelReach>().GetAll().Where(r => r.ID == _ID && r.IsActive.Value == true).FirstOrDefault();
            return qChannelReach;
        }
        #endregion

        #region "L Section Parameters History"
        public List<object> GetLSectionParametersHistory(long _ReachID, string _L, string _FromDate, string _ToDate)
        {
            List<object> lstLSectionParametersHistory = db.ExtRepositoryFor<ReachRepository>().GetLSectionParametersHistory(_ReachID, _L, _FromDate, _ToDate);
            return lstLSectionParametersHistory;
        }
        #endregion

        #region L Section Parameter
        /// <summary>
        /// this function add L Section Parameters into the database
        /// Created On: 26/01/2016
        /// </summary>
        /// <param name="_ChannelReachLSP"></param>
        /// <returns>bool</returns>
        public bool AddLSectionParameter(CO_ChannelReachLSP _ChannelReachLSP)
        {
            db.Repository<CO_ChannelReachLSP>().Insert(_ChannelReachLSP);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return list of lining type
        /// Created On: 26/01/2016
        /// </summary>
        /// <returns>List<CO_LiningType></returns>
        public List<CO_LiningType> GetLiningType()
        {
            List<CO_LiningType> lstLiningType = db.Repository<CO_LiningType>().GetAll().ToList<CO_LiningType>();
            return lstLiningType;
        }

        /// <summary>
        /// this function return L Section Parameter based on ReachID and Max Date
        /// Created On: 27/01/2016
        /// </summary>
        /// <param name="_ReachID"></param>
        /// <returns>CO_ChannelReachLSP</returns>
        public CO_ChannelReachLSP GetLSectionParameterByReachID(long _ReachID, string _RDLocation)
        {
            DateTime? dtMax = db.Repository<CO_ChannelReachLSP>().GetAll().Where(i => i.ReachID == _ReachID && i.RDLocation == _RDLocation).Max(i => (DateTime?)i.ParameterDate);

            CO_ChannelReachLSP mdlLSP = db.Repository<CO_ChannelReachLSP>().GetAll().Where(z => z.ReachID == _ReachID && z.RDLocation == _RDLocation && z.ParameterDate == dtMax).FirstOrDefault();
            return mdlLSP;
        }

        /// <summary>
        /// this function return max date on the basis of Reach ID
        /// Created On: 27/01/2016
        /// </summary>
        /// <param name="_ReachID"></param>
        /// <returns>DateTime</returns>
        public DateTime? GetMaxDateByReachID(long _ReachID, string _RDLocation)
        {
            DateTime? dtMax = db.Repository<CO_ChannelReachLSP>().GetAll().Where(i => i.ReachID == _ReachID && i.RDLocation == _RDLocation).Max(i => (DateTime?)i.ParameterDate);
            return dtMax;
        }


        #endregion
    }
}
