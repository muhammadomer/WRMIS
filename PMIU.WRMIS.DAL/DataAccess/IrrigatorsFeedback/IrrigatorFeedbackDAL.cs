using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.IrrigatorsFeedback;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigatorsFeedback
{
    public class IrrigatorFeedbackDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// this function add irrigator in database
        /// created on: 6/5/16
        /// </summary>
        /// <param name="_Irrigator"></param>
        /// <returns>bool</returns>
        public bool AddIrrigator(IF_Irrigator _Irrigator)
        {
            db.Repository<IF_Irrigator>().Insert(_Irrigator);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return all channel of specific Division
        /// Created On: 9/5/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>List<dynamic></returns>
        public List<GetTailofChannelinDivision_Result> GetChannelsByDivisionID(long _DivisionID)
        {
            return db.ExtRepositoryFor<IrrigatorsFeedbackRepository>().GetChannelByDivisionID(_DivisionID);
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
            List<IF_Irrigator> lstIrrigator = db.Repository<IF_Irrigator>().GetAll().Where(d => (d.CO_Division.CO_Circle.CO_Zone.ID == _ZoneID || _ZoneID == -1) && (d.CO_Division.CO_Circle.ID == _CircleID || _CircleID == -1)
                && (d.ChannelID == _ChannelID || _ChannelID == -1) && (d.DivisionID == _DivisionID || _DivisionID == -1) &&
                (d.MobileNo1 == _MobileNo || d.MobileNo2 == _MobileNo || _MobileNo == "") && (d.Status == _Status || _Status == "")).OrderBy(a => a.Name).ToList<IF_Irrigator>();
            return lstIrrigator;
        }

        /// <summary>
        /// this function return irrigator by ID
        /// Created On:12/05/2016
        /// </summary>
        /// <param name="_IrrigatorID"></param>
        /// <returns>IF_Irrigator</returns>
        public IF_Irrigator GetIrrigatorByID(long _IrrigatorID)
        {
            IF_Irrigator mdlIrrigator = db.Repository<IF_Irrigator>().GetAll().Where(x => x.ID == _IrrigatorID).FirstOrDefault();
            return mdlIrrigator;
        }

        /// <summary>
        /// this function updates data in database.
        /// Created On: 12/05.2016
        /// </summary>
        /// <param name="_Irrigator"></param>
        /// <returns>bool</returns>
        public bool UpdateIrrigator(IF_Irrigator _Irrigator)
        {
            IF_Irrigator mdlIrrigator = db.Repository<IF_Irrigator>().FindById(_Irrigator.ID);
            mdlIrrigator.Name = _Irrigator.Name;
            mdlIrrigator.MobileNo1 = _Irrigator.MobileNo1;
            mdlIrrigator.MobileNo2 = _Irrigator.MobileNo2;
            mdlIrrigator.Remarks = _Irrigator.Remarks;
            mdlIrrigator.Status = _Irrigator.Status;
            mdlIrrigator.TailFront = _Irrigator.TailFront;
            mdlIrrigator.TailLeft = _Irrigator.TailLeft;
            mdlIrrigator.TailRight = _Irrigator.TailRight;
            mdlIrrigator.DivisionID = _Irrigator.DivisionID;
            mdlIrrigator.ChannelID = _Irrigator.ChannelID;

            db.Repository<IF_Irrigator>().Update(mdlIrrigator);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return Irrigator By Mobile No
        /// Created On: 13/05/2016
        /// </summary>
        /// <param name="_MobileNo"></param>
        /// <param name="_MobileNo2"></param>
        /// <returns>IF_Irrigator</returns>
        public IF_Irrigator IsMobileUnique(string _MobileNo)
        {
            IF_Irrigator mdlIrrigator = db.Repository<IF_Irrigator>().GetAll().Where(z => z.MobileNo1 == _MobileNo || z.MobileNo2 == _MobileNo).FirstOrDefault();
            return mdlIrrigator;
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
            //List<IF_Irrigator> lstIrrigatorFeedback = db.Repository<IF_Irrigator>().GetAll().Where(x => (x.DivisionID == _DivisionID || _DivisionID == -1) &&
            //                                                                                                            (x.ChannelID == _ChannelID || _ChannelID == -1) &&
            //                                                                                                            (x.TailFront == _FrontTS || _FrontTS == false) &&
            //                                                                                                            (x.TailLeft == _LeftTS || _LeftTS == false) &&
            //                                                                                                            (x.TailRight == _RightTS || _RightTS == false) &&
            //                                                                                                            (x.Status == _Status || _Status == "") &&
            //                                                                                                            (x.MobileNo1 == _MobileNo || _MobileNo == "")
            //                                                                                                            ).ToList<IF_Irrigator>();
            //return lstIrrigatorFeedback;
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteStoredProcedureDataTable("SearchIrrigator", _ZoneID, _CircleID, _DivisionID, _ChannelID, _FrontTS, _LeftTS, _RightTS, _Status, _MobileNo, _TailStatus);
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
            List<IF_IrrigatorFeedback> lstIrrigatorFeedback = db.Repository<IF_IrrigatorFeedback>().GetAll().Where(x => x.IrrigatorID == _IrrigatorID).ToList();
            lstIrrigatorFeedback = lstIrrigatorFeedback.Where(x => (_FromDate == null || x.FeedbackDate.Value.Date >= _FromDate.Value.Date) && (_ToDate == null || x.FeedbackDate.Value.Date <= _ToDate.Value.Date)).ToList();
            return lstIrrigatorFeedback;
        }

        /// <summary>
        /// this function return villages by Division ID
        /// Created On:3/6/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>List<CO_Village></returns>
        public List<CO_Village> GetVillagesByDivisionID(long _DivisionID)
        {
            List<long> lstDistrictDivision = db.Repository<CO_DistrictDivision>().GetAll().Where(x => x.DivisionID == _DivisionID && x.DistrictID != null).Select(s => s.DistrictID.Value).ToList<long>();
            List<CO_Village> lstVillages = db.Repository<CO_Village>().GetAll().Where(x => lstDistrictDivision.Contains(x.CO_Tehsil.DistrictID.Value)).OrderBy(x => x.Name).ToList();
            return lstVillages;
        }

        /// <summary>
        /// this function add irrigator feedback in database
        /// created on: 3/6/16
        /// </summary>
        /// <param name="_Irrigator"></param>
        /// <returns>bool</returns>
        public long AddIrrigatorFeedback(IF_IrrigatorFeedback _IrrigatorFeedback)
        {
            db.Repository<IF_IrrigatorFeedback>().Insert(_IrrigatorFeedback);
            db.Save();
            long ID = _IrrigatorFeedback.ID;
            return ID;
        }

        /// <summary>
        /// this function return Irrigator Information
        /// Created On:6/6/16
        /// </summary>
        /// <param name="_IrrigatorID"></param>
        /// <returns></returns>
        public DataTable GetIrrigatorInformation(long _IrrigatorID)
        {
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteStoredProcedureDataTable("IrrigatorFeedbackInfortmation", _IrrigatorID);
        }


        /// <summary>
        /// this function return Irrigator Feedback by ID
        /// Created On: 7/6/16
        /// </summary>
        /// <param name="_IrrigatorFeedbackID"></param>
        /// <returns>IF_IrrigatorFeedback</returns>
        public IF_IrrigatorFeedback GetIrrigatorFeedbackByID(long _IrrigatorFeedbackID)
        {
            IF_IrrigatorFeedback mdlIrrigatorFeedback = db.Repository<IF_IrrigatorFeedback>().GetAll().Where(x => x.ID == _IrrigatorFeedbackID).FirstOrDefault();
            return mdlIrrigatorFeedback;
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
            List<long> lstRotationalProgramTemp = new List<long>();
            List<long> lstRotationalProgram = new List<long>();
            long SubDivision = 0;
            long RotationalProgramID = 0;

            List<long> lstSubDivisions = db.Repository<CO_SubDivision>().GetAll().Where(x => x.DivisionID == _DivisionID).Select(x => x.ID).ToList();
            for (int i = 0; i < lstSubDivisions.Count(); i++)
            {
                SubDivision = lstSubDivisions[i];
                lstRotationalProgramTemp = db.Repository<RP_RotationalProgram>().GetAll().Where(x => x.IrrigationLevelID == 4 && x.IrrigationBoundaryID == SubDivision && x.IsApproved == true && x.SeasonID == _SeasonID && x.RPYear == _RPYear).Select(x => x.ID).ToList();
                for (int j = 0; j < lstRotationalProgramTemp.Count(); j++)
                {
                    lstRotationalProgram.Add(lstRotationalProgramTemp[j]);
                }
            }

            lstRotationalProgramTemp = new List<long>();
            lstRotationalProgramTemp = db.Repository<RP_RotationalProgram>().GetAll().Where(x => x.IrrigationLevelID == 3 && x.IrrigationBoundaryID == _DivisionID && x.IsApproved == true && x.SeasonID == _SeasonID && x.RPYear == _RPYear).Select(x => x.ID).ToList();
            for (int i = 0; i < lstRotationalProgramTemp.Count(); i++)
            {
                lstRotationalProgram.Add(lstRotationalProgramTemp[i]);
            }

            RP_Channel lstChannelID = null;
            for (int i = 0; i < lstRotationalProgram.Count(); i++)
            {
                RotationalProgramID = lstRotationalProgram[i];
                lstChannelID = db.Repository<RP_Channel>().GetAll().Where(x => x.ChannelID == _ChannelID && x.RPID == RotationalProgramID).FirstOrDefault();
                if (lstChannelID != null)
                {
                    break;
                }
            }
            if (lstChannelID == null)
                return 0;
            else
                return lstChannelID.RPID;
        }
    }
}
