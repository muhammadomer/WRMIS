using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.Reach;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.Reach
{
    public class ReachBLL : BaseBLL
    {
        #region "Define Channel Reaches"
        public List<object> GetChannelReach(long _ChannelID)
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.GetChannelReach(_ChannelID);
        }
        /// <summary>
        /// This function retun channel addition success along with message
        /// Created on 22-10-2015
        /// </summary>
        /// <param name="_Reach"></param>
        /// <returns></returns>
        public bool SaveChannelReach(CO_ChannelReach _Reach)
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.SaveChannelReach(_Reach);
        }
        /// <summary>
        /// This function soft delete Channel Reach
        /// Created on 25-01-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>bool</returns>
        public bool DeleteChannelReach(long _ID)
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.DeleteChannelReach(_ID);
        }
        public bool AdjustReach(long _ReachID, long _ChannelID)
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.AdjustReach(_ReachID, _ChannelID);
        }
        public bool EditChannelReach(CO_ChannelReach _Reach)
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.EditChannelReach(_Reach);
        }
        #endregion

        #region "L Section Parameters History"
        public List<object> GetLSectionParametersHistory(long _ReachID, string _L, string _FromDate, string _ToDate)
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.GetLSectionParametersHistory(_ReachID, _L, _FromDate, _ToDate);
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
            ReachDAL dalReach = new ReachDAL();
            return dalReach.AddLSectionParameter(_ChannelReachLSP);
        }

        /// <summary>
        /// this function return list of lining type
        /// Created On: 26/01/2016
        /// </summary>
        /// <returns>List<CO_LiningType></returns>
        public List<CO_LiningType> GetLiningType()
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.GetLiningType();
        }

        /// <summary>
        /// this function return L Section Parameter based on ReachID and Max Date
        /// Created On: 27/01/2016
        /// </summary>
        /// <param name="_ReachID"></param>
        /// <returns>CO_ChannelReachLSP</returns>
        public CO_ChannelReachLSP GetLSectionParameterByReachID(long _ReachID, string _RDLocation)
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.GetLSectionParameterByReachID(_ReachID, _RDLocation);
        }

        /// <summary>
        /// this function return max date on the basis of Reach ID
        /// Created On: 27/01/2016
        /// </summary>
        /// <param name="_ReachID"></param>
        /// <returns>DateTime</returns>
        public DateTime? GetMaxDateByReachID(long _ReachID, string _RDLocation)
        {
            ReachDAL dalReach = new ReachDAL();
            return dalReach.GetMaxDateByReachID(_ReachID, _RDLocation);
        }


        #endregion
    }
}
