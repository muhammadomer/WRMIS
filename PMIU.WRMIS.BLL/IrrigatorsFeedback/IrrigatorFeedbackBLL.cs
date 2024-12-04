using PMIU.WRMIS.DAL.DataAccess.IrrigatorsFeedback;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigatorsFeedback
{
    public class IrrigatorFeedbackBLL : BaseBLL
    {
        /// <summary>
        /// this function add irrigator in database
        /// created on: 6/5/16
        /// </summary>
        /// <param name="_Irrigator"></param>
        /// <returns>bool</returns>
        public bool AddIrrigator(IF_Irrigator _Irrigator)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.AddIrrigator(_Irrigator);
        }

        /// <summary>
        /// this function return all channel of specific Division
        /// Created On: 9/5/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>List<dynamic></returns>
        public List<GetTailofChannelinDivision_Result> GetChannelsByDivisionID(long _DivisionID)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetChannelsByDivisionID(_DivisionID);
        }

        /// <summary>
        /// this function return all Irrigators on the basis of given criteria
        /// Created On: 11/05/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_MobileNo"></param>
        /// <param name="_Status"></param>
        /// <returns>List<IF_Irrigator></returns>
        public List<IF_Irrigator> GetIrrigatorByDivisonIDAndChannelID(long _ZoneID, long _CircleID, long _DivisionID, long _ChannelID, string _MobileNo, string _Status)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetIrrigatorByDivisonIDAndChannelID(_ZoneID, _CircleID, _DivisionID, _ChannelID, _MobileNo, _Status);
        }

        /// <summary>
        /// this function return irrigator by ID
        /// Created On:12/05/2016
        /// </summary>
        /// <param name="_IrrigatorID"></param>
        /// <returns>IF_Irrigator</returns>
        public IF_Irrigator GetIrrigatorByID(long _IrrigatorID)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetIrrigatorByID(_IrrigatorID);
        }

        /// <summary>
        /// this function updates data in database.
        /// Created On: 12/05.2016
        /// </summary>
        /// <param name="_Irrigator"></param>
        /// <returns>bool</returns>
        public bool UpdateIrrigator(IF_Irrigator _Irrigator)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.UpdateIrrigator(_Irrigator);
        }

        /// <summary>
        /// this function return Irrigator By Mobile No
        /// Created On: 13/05/2016
        /// </summary>
        /// <param name="_MobileNo1"></param>
        /// <param name="_MobileNo2"></param>
        /// <returns>IF_Irrigator</returns>
        public IF_Irrigator IsMobileUnique(string _MobileNo)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.IsMobileUnique(_MobileNo);
        }

        /// <summary>
        /// this function return irrigator feedback on the basis of given parameters.
        /// Created On: 18/05/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_TailStatus"></param>
        /// <param name="_FrontTS"></param>
        /// <param name="_LeftTS"></param>
        /// <param name="_RightTS"></param>
        /// <param name="_Status"></param>
        /// <param name="_MobileNo"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns></returns>
        public DataTable GetIrrigatorFeedback(long _ZoneID, long _CircleID, long _DivisionID, long _ChannelID, bool _FrontTS, bool _LeftTS, bool _RightTS, string _Status, string _MobileNo, int _TailStatus)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetIrrigatorFeedback(_ZoneID, _CircleID, _DivisionID, _ChannelID, _FrontTS, _LeftTS, _RightTS, _Status, _MobileNo, _TailStatus);
        }

        /// <summary>
        /// this function return irrigator history by ID
        /// Created On: 2/6/16
        /// </summary>
        /// <param name="_IrrigatorID"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns>List<IF_IrrigatorFeedback></returns>
        public List<IF_IrrigatorFeedback> GetFeedbackHistoryByIrrigatorID(long _IrrigatorID, DateTime? _FromDate, DateTime? _ToDate)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetFeedbackHistoryByIrrigatorID(_IrrigatorID, _FromDate, _ToDate);
        }

        /// <summary>
        /// this function return villages by Division ID
        /// Created On:3/6/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>List<CO_Village></returns>
        public List<CO_Village> GetVillagesByDivisionID(long _DivisionID)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetVillagesByDivisionID(_DivisionID);
        }

        /// <summary>
        /// this function add irrigator feedback in database
        /// created on: 3/6/16
        /// </summary>
        /// <param name="_Irrigator"></param>
        /// <returns>bool</returns>
        public long AddIrrigatorFeedback(IF_IrrigatorFeedback _IrrigatorFeedback)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.AddIrrigatorFeedback(_IrrigatorFeedback);
        }

        /// <summary>
        /// this function return Irrigator Information
        /// Created On:6/6/16
        /// </summary>
        /// <param name="_IrrigatorID"></param>
        /// <returns></returns>
        public DataTable GetIrrigatorInformation(long _IrrigatorID)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetIrrigatorInformation(_IrrigatorID);
        }

        /// <summary>
        /// this function return Irrigator Feedback by ID
        /// Created On: 7/6/16
        /// </summary>
        /// <param name="_IrrigatorFeedbackID"></param>
        /// <returns>IF_IrrigatorFeedback</returns>
        public IF_IrrigatorFeedback GetIrrigatorFeedbackByID(long _IrrigatorFeedbackID)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetIrrigatorFeedbackByID(_IrrigatorFeedbackID);
        }

        /// <summary>
        /// this function return Approved Rotational Program ID of Current Year according to Season 
        /// Created Date 20/7/2017
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_SeasonID"></param>
        /// <param name="_RPYear"></param>
        /// <returns>long</returns>
        public long GetRotationalProgramID(long _DivisionID, long _ChannelID, long _SeasonID, string _RPYear)
        {
            IrrigatorFeedbackDAL dalIrrigatorFeedback = new IrrigatorFeedbackDAL();
            return dalIrrigatorFeedback.GetRotationalProgramID(_DivisionID, _ChannelID, _SeasonID, _RPYear);
        }
    }
}
