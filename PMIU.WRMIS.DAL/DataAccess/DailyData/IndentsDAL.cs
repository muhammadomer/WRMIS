using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.DailyData;
using System.Data;

namespace PMIU.WRMIS.DAL.DataAccess.DailyData
{
    public class IndentsDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// this function return list of all channels
        /// Created On: 1-12-2015
        /// </summary>
        /// <returns>List<CO_Channel></returns>
        public List<dynamic> GetAllChannels(long _SubDivID, long _CommandTypeID, long _ChannelTypeID, long _FlowTypeID, long _ChannelNameID)
        {
            List<dynamic> lstChannel = db.ExtRepositoryFor<IndentRepository>().GetChannelData(_SubDivID, _CommandTypeID, _ChannelTypeID, _FlowTypeID, _ChannelNameID);

            return lstChannel;
        }



        /// <summary>
        /// this function adds Indents in Channel Indent Table
        /// Created On: 9/12/2015
        /// </summary>
        /// <param name="_ChannelIndent"></param>
        /// <returns>bool</returns>
        public bool AddIndnet(CO_ChannelIndent _ChannelIndent)
        {
            db.Repository<CO_ChannelIndent>().Insert(_ChannelIndent);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function updates Indents in Channel Indent Table
        /// Created On: 20/04/2016
        /// </summary>
        /// <param name="_ChannelIndent"></param>
        /// <returns>bool</returns>
        public bool UpdateIndent(CO_ChannelIndent _ChannelIndent)
        {
            CO_ChannelIndent mdlChannelIndent = db.Repository<CO_ChannelIndent>().FindById(_ChannelIndent.ID);
            mdlChannelIndent.SubDivID = _ChannelIndent.SubDivID;
            mdlChannelIndent.ParentChannelID = _ChannelIndent.ParentChannelID;
            mdlChannelIndent.SubDivGaugeID = _ChannelIndent.SubDivGaugeID;
            mdlChannelIndent.IndentDate = _ChannelIndent.IndentDate;
            mdlChannelIndent.EntryDate = _ChannelIndent.EntryDate;
            mdlChannelIndent.OutletIndent = _ChannelIndent.OutletIndent;
            mdlChannelIndent.TotalOfftakeIndent = _ChannelIndent.TotalOfftakeIndent;
            mdlChannelIndent.PercentageIncrement = _ChannelIndent.PercentageIncrement;
            mdlChannelIndent.PercentageIncrementValue = _ChannelIndent.PercentageIncrementValue;
            mdlChannelIndent.ModifiedBy = _ChannelIndent.ModifiedBy;
            mdlChannelIndent.ModifiedDate = _ChannelIndent.ModifiedDate;
            db.Repository<CO_ChannelIndent>().Update(mdlChannelIndent);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return max date on the basis of channel id
        /// Created On: 10/12/2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_Date"></param>
        /// <returns>DateTime</returns>
        public DateTime? GetMaxDateByChannelID(long _ChannelID, long _SubDivID)
        {
            //DateTime? dtMax = db.Repository<CO_ChannelIndent>().GetAll().Where(i => i.ChannelID == _ChannelID && i.SubDivID == _SubDivID).Max(i => (DateTime?)i.FromDate);
            DateTime? dtMax = DateTime.Now;
            return dtMax;
        }

        /// <summary>
        /// this function gets section by sub division id and channel id
        /// Created On: 11/12/2015
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>CO_ChannelIrrigationBoundaries</returns>
        public CO_ChannelIrrigationBoundaries GetSectionBySubDivIDAndChannelID(long _SubDivID, long _ChannelID)
        {
            CO_ChannelIrrigationBoundaries mdlChannel = db.ExtRepositoryFor<IndentRepository>().GetSectionBySubDivID(_SubDivID, _ChannelID);
            return mdlChannel;
        }

        /// <summary>
        /// this function gets Channel Gauge By Section ID and Channel ID
        /// Created On: 11/12/2015
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>CO_ChannelGauge</returns>
        public CO_ChannelGauge GetDesignDischargeBySectionIDAndChannelID(long _SectionID, long _ChannelID)
        {
            CO_ChannelGauge mdlChannelGauge = db.Repository<CO_ChannelGauge>().GetAll().Where(d => d.SectionID == _SectionID && d.ChannelID == _ChannelID).FirstOrDefault();
            return mdlChannelGauge;
        }

        /// <summary>
        /// this function return indent history by channel id and subdiv id and provided dates
        /// Created On: 15/12/2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_SubDivID"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetIndentHistoryByChannelIDAndSubDivID(long _ChannelID, long _SubDivID, DateTime? _FromDate, DateTime? _ToDate)
        {

            //List<CO_ChannelIndent> lstChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(d => d.ChannelID == _ChannelID && d.SubDivID == _SubDivID &&
            //    (d.FromDate >= _FromDate || _FromDate == null) && (d.FromDate <= _ToDate || _ToDate == null)).OrderByDescending(a => a.FromDate).ToList<CO_ChannelIndent>();

            List<dynamic> lstIndents = new List<dynamic>();
            //DateTime TempDate = DateTime.Now;

            //for (int i = 0; i < lstChannelIndent.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        TempDate = lstChannelIndent.ElementAt(i).FromDate.Value;

            //        lstIndents.Add(new
            //        {
            //            ChannelIndent = lstChannelIndent.ElementAt(i),
            //            ToDate = ""
            //        });
            //    }
            //    else
            //    {
            //        lstIndents.Add(new
            //        {
            //            ChannelIndent = lstChannelIndent.ElementAt(i),
            //            ToDate = TempDate.AddDays(-1)
            //        });

            //        TempDate = lstChannelIndent.ElementAt(i).FromDate.Value;
            //    }
            //}
            return lstIndents;
        }

        /// <summary>
        /// this function return one lower sub division of current sub division
        /// Created On: 10-02-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_SubDivID"></param>
        /// <returns>long</returns>
        public long GetLowerSubDivision(long _ChannelID, long _SubDivID)
        {
            List<long> lstSubDivision = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Where(x => x.ChannelID == _ChannelID).GroupBy(x => x.CO_Section.SubDivID).OrderBy(x => x.FirstOrDefault().SectionRD).Select(x => (long)x.FirstOrDefault().CO_Section.SubDivID).ToList();
            int ListIndex = lstSubDivision.FindIndex(a => a == _SubDivID);
            long SubDivision = -1;
            if (ListIndex != -1 && ListIndex + 1 < lstSubDivision.Count())
            {
                SubDivision = lstSubDivision.ElementAt(ListIndex + 1);
            }
            return SubDivision;
        }

        /// <summary>
        /// this function return Indent Value on the basis of ChannelID and SubDivID
        /// Cretaed On: 11-02-2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>CO_ChannelIndent</returns>
        public CO_ChannelIndent GetIndentBySubDivIDandChannelID(long _ChannelID, long _SubDivID)
        {
            DateTime? dtMax = db.Repository<CO_ChannelIndent>().GetAll().Where(i => i.ParentChannelID == _ChannelID && i.SubDivID == _SubDivID).Max(i => (DateTime?)i.IndentDate);
            CO_ChannelIndent mdlChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.SubDivID == _SubDivID && x.ParentChannelID == _ChannelID && x.IndentDate == dtMax).OrderByDescending(z => z.ID).FirstOrDefault();
            return mdlChannelIndent;
        }

        /// <summary>
        /// this function return Indent Value on the basis of ChannelID and SubDivID and date
        /// Created On:23/04/2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_SubDivID"></param>
        /// <param name="_Date"></param>
        /// <returns>CO_ChannelIndent</returns>
        public CO_ChannelIndent GetIndentBySubDivIDandChannelIDandDate(long _ChannelID, long _SubDivID, DateTime _Date)
        {
            //DateTime? dtMax = db.Repository<CO_ChannelIndent>().GetAll().Where(i => i.ChannelID == _ChannelID && i.SubDivID == _SubDivID).Max(i => (DateTime?)i.FromDate);
            //CO_ChannelIndent mdlChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.SubDivID == _SubDivID && x.ChannelID == _ChannelID && x.FromDate == _Date).OrderByDescending(z => z.ID).FirstOrDefault();
            CO_ChannelIndent mdlChannelIndent = new CO_ChannelIndent();
            return mdlChannelIndent;
        }



        /// <summary>
        /// this function return list of all Sub Division on the Basis of Channel ID
        /// Created On: 12-02-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<long></returns>
        public List<long> GetListOfSubDivisions(long _ChannelID)
        {
            List<long> lstSubDivision = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Where(x => x.ChannelID == _ChannelID).GroupBy(x => x.CO_Section.SubDivID).OrderBy(x => x.FirstOrDefault().SectionRD).Select(x => (long)x.FirstOrDefault().CO_Section.SubDivID).ToList();
            return lstSubDivision;
        }

        /// <summary>
        /// this function return record where indent is not auto generated on the basis of channl id, subdiv id and date
        /// Created On: 07/04/2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_SubDivID"></param>
        /// <param name="_Date"></param>
        /// <returns>bool</returns>
        public bool GetIndentWhereNotAutoGenerated(long _ChannelID, long _SubDivID, DateTime _Date)
        {
            //CO_ChannelIndent mdlChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.ChannelID == _ChannelID && x.SubDivID == _SubDivID && x.FromDate == _Date && x.IsAutoGenerated == false).FirstOrDefault();
            //if (mdlChannelIndent != null)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }


        /***********************************Indents New Working**********************************************/

        /// <summary>
        /// this function get subdivisional Head Gauges and offtake By User ID from procedure
        /// Created On: 01-08-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>IEnumerable<DataRow></returns>
        public IEnumerable<DataRow> GetChannelsByUserID(long _UserID, long? _IrrigationLevelID, long _SubDivID)
        {
            return db.ExecuteDataSet("DD_GetChannelsByUserIDForIndents", _UserID, _IrrigationLevelID, _SubDivID);
        }

        /// <summary>
        /// this function get data from Gauge Lag Table by ChannelID
        /// Created On: 01-08-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_GaugeLag></returns>
        public List<CO_GaugeLag> GetGaugeLagByChannelID(long _ChannelID)
        {
            List<CO_GaugeLag> lstGaugeLag = db.Repository<CO_GaugeLag>().GetAll().Where(x => x.ChannelID == _ChannelID).ToList<CO_GaugeLag>();
            return lstGaugeLag;
        }

        /// <summary>
        /// This function updates Gauge Lag into the Database
        /// Created On: 01-08-2016
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>bool</returns>
        public bool UpdateGaugeLag(CO_GaugeLag _GaugeLag)
        {
            CO_GaugeLag mdlGaugeLag = db.Repository<CO_GaugeLag>().FindById(_GaugeLag.ID);
            mdlGaugeLag.Velocity = _GaugeLag.Velocity;
            mdlGaugeLag.LagTime = _GaugeLag.LagTime;

            db.Repository<CO_GaugeLag>().Update(mdlGaugeLag);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return sub division on the basis of SDO User ID
        /// Created On: 02-08-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_SubDivision></returns>
        public List<CO_SubDivision> GetSubDivisionOFSDOByUserID(long _UserID, long? _IrrigationLevelID)
        {
            List<CO_SubDivision> lstSubDivisions = db.ExtRepositoryFor<IndentRepository>().GetSubDivisionOFSDOByUserID(_UserID, _IrrigationLevelID);
            return lstSubDivisions;
        }

        /// <summary>
        /// this function get SubDivision on the basis of User ID
        /// Created On: 02-08-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        public UA_AssociatedLocation GetSubDivisionByUserID(long _UserID, long? _IrrigationLevelID)
        {
            UA_AssociatedLocation mdlSubDivision = db.Repository<UA_AssociatedLocation>().GetAll().Where(x => x.UserID == _UserID && x.IrrigationLevelID == _IrrigationLevelID).FirstOrDefault();
            return mdlSubDivision;
        }

        /// <summary>
        /// this function return all Channel by name on the basis of sub division id
        /// Created On: 7/12/2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetChannelsNameBySubDivisionIDOrDivisionID(long? _SubDivisionID, long? _DivisionID)
        {
            List<CO_Channel> lstChannelSearch = db.ExtRepositoryFor<IndentRepository>().GetChannelsNameBySubDivisionIDOrDivisionID(_SubDivisionID, _DivisionID);
            return lstChannelSearch;
        }

        /// <summary>
        /// this function return list of sub divisions on the basis of XEN User ID
        /// Created On: 02-08-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns>List<CO_SubDivision></returns>
        public List<CO_SubDivision> GetSubDivisionsOFXENByUserID(long _UserID, long? _IrrigationLevelID)
        {
            List<CO_SubDivision> lstSubDivisions = db.ExtRepositoryFor<IndentRepository>().GetSubDivisionsOFXENByUserID(_UserID, _IrrigationLevelID);
            return lstSubDivisions;
        }

        /// <summary>
        /// this function will return Indents By Sub Division ID Or Channel ID
        /// Created On: 03-08-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_IndentPlacementDate"></param>
        /// <returns>List<CO_ChannelIndent></returns>
        public List<DD_GetDailyIndentsData_Result> GetIndentsBySubDivisionIDOrChannelID(long _SubDivisionID, long _ChannelID, DateTime _IndentPlacementDate)
        {
            //List<CO_ChannelIndent> lstChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x =>
            //    (x.SubDivID == _SubDivisionID || _SubDivisionID == -1)
            //    && (x.ParentChannelID == _ChannelID || _ChannelID == -1)).ToList<CO_ChannelIndent>();
            //lstChannelIndent = lstChannelIndent.Where(x => x.IndentDate.Value.Date <= _IndentPlacementDate.Date).OrderByDescending(x => x.IndentDate).ToList();
            //return lstChannelIndent;
            List<DD_GetDailyIndentsData_Result> lstDailyIndentData = db.ExtRepositoryFor<IndentRepository>().GetIndentsBySubDivisionIDOrChannelID(_SubDivisionID, _ChannelID, _IndentPlacementDate);
            return lstDailyIndentData;
        }

        /// <summary>
        /// this function will return all offtakes on the basis of indent ID
        /// Created On: 05-08-2016
        /// </summary>
        /// <param name="_IndentID"></param>
        /// <returns>List<CO_ChannelIndentOfftakes></returns>
        public List<CO_ChannelIndentOfftakes> GetChannelIndentOfftakes(long _IndentID, DateTime _Date)
        {
            List<CO_ChannelIndentOfftakes> lstChannelIndentOfftakes = db.Repository<CO_ChannelIndentOfftakes>().GetAll().Where(x => x.IndentID == _IndentID).ToList<CO_ChannelIndentOfftakes>();
            lstChannelIndentOfftakes = lstChannelIndentOfftakes.Where(x => x.OfftakeIndentDate.Value.Date == Convert.ToDateTime(_Date.Date.ToString("MM/dd/yyyy"))).ToList();
            return lstChannelIndentOfftakes;
        }

        /// <summary>
        /// this function will reutrn all dates between rabi and kharif season
        /// Kharif: April to September
        /// Rabi: October to March
        /// </summary>
        /// <param name="_Date"></param>
        /// <returns>List<CO_ChannelIndentOfftakes></returns>
        public List<string> GetAllDates(DateTime _Date)
        {
            List<DateTime> lstChannelIndentOfftakes;
            List<string> lstDateChannelIndentOfftakes = new List<string>();
            if (_Date.Month >= 4 && _Date.Month <= 9)
            {
                lstChannelIndentOfftakes = db.Repository<CO_ChannelIndentOfftakes>().GetAll().Where(x => x.OfftakeIndentDate.Value.Month >= 4 && x.OfftakeIndentDate.Value.Month <= 9).Select(x => x.OfftakeIndentDate.Value).Distinct().ToList<DateTime>();
            }
            else
            {
                lstChannelIndentOfftakes = db.Repository<CO_ChannelIndentOfftakes>().GetAll().Where(x => x.OfftakeIndentDate.Value.Month >= 10 || x.OfftakeIndentDate.Value.Month <= 3).Select(x => x.OfftakeIndentDate.Value).Distinct().ToList<DateTime>();
            }
            for (int i = 0; i <= lstChannelIndentOfftakes.Count - 1; i++)
            {
                lstDateChannelIndentOfftakes.Add(Utility.GetFormattedDate(lstChannelIndentOfftakes.ElementAt(i)));
            }
            return lstDateChannelIndentOfftakes;
        }

        /// <summary>
        /// this function return list of Offtakes by OfftakeID
        /// Created On: 8/8/2016
        /// </summary>
        /// <param name="_OfftakeID"></param>
        /// <param name="_Date"></param>
        /// <returns>List<CO_ChannelIndentOfftakes></returns>
        public List<CO_ChannelIndentOfftakes> GetOfftakesByOfftakeID(long _OfftakeID, DateTime _Date)
        {
            List<CO_ChannelIndentOfftakes> lstChannelIndentOfftakes;

            if (_Date.Month >= 4 && _Date.Month <= 9)
            {
                lstChannelIndentOfftakes = db.Repository<CO_ChannelIndentOfftakes>().GetAll().Where(x => x.OfftakeIndentDate.Value.Month >= 4 && x.OfftakeIndentDate.Value.Month <= 9 && x.ChannelID == _OfftakeID).OrderByDescending(a => a.OfftakeIndentDate).ToList<CO_ChannelIndentOfftakes>();
            }
            else
            {
                lstChannelIndentOfftakes = db.Repository<CO_ChannelIndentOfftakes>().GetAll().Where(x => (x.OfftakeIndentDate.Value.Month >= 10 || x.OfftakeIndentDate.Value.Month <= 3) && x.ChannelID == _OfftakeID).OrderByDescending(a => a.OfftakeIndentDate).ToList<CO_ChannelIndentOfftakes>();
            }
            return lstChannelIndentOfftakes;
        }

        /// <summary>
        /// this function return all parent and channels against specific Subdivision
        /// Created On: 9/8/2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_SubDivID"></param>
        /// <param name="_ParentChild"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetChannelsByUserIDAndSubDivID(long _UserID, long _IrrigationLevelID, long _SubDivID, string _ParentChild)
        {
            List<dynamic> lstSubDivisions = db.ExtRepositoryFor<IndentRepository>().GetChannelsByUserIDAndSubDivID(_UserID, _IrrigationLevelID, _SubDivID, _ParentChild);
            return lstSubDivisions;
        }

        /// <summary>
        /// this function will return Indent ID on the basis of SubDivID, ChannelID and Date
        /// Created On: 9/8/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        public long? GetIndentIDBySubDivIDAndParentChannelIDAndDate(long _SubDivID, long _ChannelID, DateTime _Date)
        {
            List<CO_ChannelIndent> lstChannelIndentOfftakes = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.SubDivID == _SubDivID && x.ParentChannelID == _ChannelID).OrderByDescending(x => x.IndentDate).ToList<CO_ChannelIndent>();
            CO_ChannelIndent mdlIndent = lstChannelIndentOfftakes.Where(x => x.IndentDate.Value.Date <= _Date.Date).FirstOrDefault();

            long? IndentID = null;
            //CO_ChannelIndent mdlIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.SubDivID == _SubDivID && x.ParentChannelID == _ChannelID && x.IndentDate.Value.Date <= _Date.Date).OrderByDescending(a => a.IndentDate).FirstOrDefault();
            if (mdlIndent == null)
            {
                return IndentID;
            }
            else
            {
                return IndentID = mdlIndent.ID;
            }
        }

        /// <summary>
        /// this function will return all offtakes on the basis of indent ID
        /// Created On: 05-08-2016
        /// </summary>
        /// <param name="_IndentID"></param>
        /// <returns>List<CO_ChannelIndentOfftakes></returns>
        public List<CO_ChannelIndentOfftakes> GetChannelIndentOfftakesForAddIndent(long? _IndentID, DateTime _Date)
        {
            List<CO_ChannelIndentOfftakes> lstChannelIndentOfftakes = db.Repository<CO_ChannelIndentOfftakes>().GetAll().Where(x => x.IndentID == _IndentID).ToList<CO_ChannelIndentOfftakes>();
            lstChannelIndentOfftakes = lstChannelIndentOfftakes.Where(x => x.OfftakeIndentDate.Value.Date <= _Date.Date).ToList();
            return lstChannelIndentOfftakes;
        }

        public CO_Channel GetOfftakesByParrentIDFirstTime(long _ChannelID)
        {
            CO_Channel mdlChannel = db.Repository<CO_Channel>().GetAll().Where(x => x.ID == _ChannelID).FirstOrDefault();
            return mdlChannel;
        }

        public List<DD_GetChannelsByUserIDForIndents_Result> GetChannelsByUserIDAndSubDivIDTest(long _UserID, long _IrrigationLevelID, long _SubDivID, string _ParentChild)
        {
            List<DD_GetChannelsByUserIDForIndents_Result> lstChannels = db.ExtRepositoryFor<IndentRepository>().GetChannelsByUserIDAndSubDivIDTest(_UserID, _IrrigationLevelID, _SubDivID, _ParentChild);
            return lstChannels;
        }


        public List<dynamic> GetChannelIndentsOffTakesFromOffTakesTable(long _IndentID, DateTime _Date)
        {
            List<dynamic> lstChannels = db.ExtRepositoryFor<IndentRepository>().GetChannelIndentsOffTakesFromOffTakesTable(_IndentID, _Date);
            return lstChannels;
        }


        public dynamic GetChannelIndentsOffTakesFromChannelTable(long _ChannelID, DateTime _Date)
        {
            dynamic mdlChannels = db.ExtRepositoryFor<IndentRepository>().GetChannelIndentsOffTakesFromChannelTable(_ChannelID, _Date);
            return mdlChannels;
        }

        /// <summary>
        /// this function adds Indents in Channel Indent Table
        /// Created On: 11/08/2016
        /// </summary>
        /// <param name="_ChannelIndentOfftake"></param>
        /// <returns>bool</returns>
        public bool AddChannelIndnetOfftakes(CO_ChannelIndentOfftakes _ChannelIndentOfftake)
        {
            db.Repository<CO_ChannelIndentOfftakes>().Insert(_ChannelIndentOfftake);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function updates Indents in Channel Indent Offtake Table
        /// Created On: 11/08/2016
        /// </summary>
        /// <param name="_ChannelIndentOfftake"></param>
        /// <returns>bool</returns>
        public bool UpdateChannelIndnetOfftakes(CO_ChannelIndentOfftakes _ChannelIndentOfftake)
        {
            CO_ChannelIndentOfftakes mdlChannelIndentOfftake = db.Repository<CO_ChannelIndentOfftakes>().FindById(_ChannelIndentOfftake.ID);
            mdlChannelIndentOfftake.IndentID = _ChannelIndentOfftake.IndentID;
            mdlChannelIndentOfftake.ChannelID = _ChannelIndentOfftake.ChannelID;
            mdlChannelIndentOfftake.ChannelRD = _ChannelIndentOfftake.ChannelRD;
            mdlChannelIndentOfftake.ChannelIndent = _ChannelIndentOfftake.ChannelIndent;
            mdlChannelIndentOfftake.OfftakeIndentDate = _ChannelIndentOfftake.OfftakeIndentDate;
            mdlChannelIndentOfftake.Remarks = _ChannelIndentOfftake.Remarks;
            mdlChannelIndentOfftake.ModifiedBy = _ChannelIndentOfftake.ModifiedBy;
            mdlChannelIndentOfftake.ModifiedDate = _ChannelIndentOfftake.ModifiedDate;
            db.Repository<CO_ChannelIndentOfftakes>().Update(mdlChannelIndentOfftake);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return Sub Divisional Gauge ID on the basis of SubDivID and ChannelID and GaugeCategoryID
        /// Created On: 12/08/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_GaugeCategoryID"></param>
        /// <returns>CO_ChannelGauge</returns>
        public CO_ChannelGauge GetSubDivGaugeID(long _SubDivID, long _ChannelID, long _GaugeCategoryID)
        {
            CO_ChannelGauge mdlGaugeCategory = db.Repository<CO_ChannelGauge>().GetAll().Where(x => x.ChannelID == _ChannelID && (x.GaugeSubDivID == _SubDivID || (x.GaugeCategoryID == (long)Constants.GaugeCategory.HeadGauge && x.CO_Section.SubDivID == _SubDivID))).FirstOrDefault();
            return mdlGaugeCategory;
        }

        /// <summary>
        /// this function return outlet indent on the basis of SubDivID and ChannelID
        /// Created On: 12/08/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>double?</returns>
        public double? GetOutletIndent(long _SubDivID, long _ChannelID)
        {
            double? fltGetOutletIndent = db.ExtRepositoryFor<IndentRepository>().GetOutletIndent(_SubDivID, _ChannelID);
            return fltGetOutletIndent;
        }

        /// <summary>
        /// this function will return Indents By Sub Division ID and Channel ID and date
        /// Created On: 03-08-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_IndentPlacementDate"></param>
        /// <returns>List<CO_ChannelIndent></returns>
        public List<CO_ChannelIndent> GetIndentBySubDivisionIDAndChannelIDAndDate(long _SubDivisionID, long _ChannelID, DateTime _IndentPlacementDate)
        {
            List<CO_ChannelIndent> lstChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.SubDivID == _SubDivisionID && x.ParentChannelID == _ChannelID).ToList<CO_ChannelIndent>();

            lstChannelIndent = lstChannelIndent.Where(x => x.IndentDate.Value.Date == _IndentPlacementDate.Date).ToList();
            return lstChannelIndent;
        }

        /// <summary>
        /// this function will return Offtakes By Sub Division ID and Channel ID and date
        /// Created On: 03-08-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_IndentPlacementDate"></param>
        /// <returns>List<CO_ChannelIndent></returns>
        public bool GetOfftakeIndentBySubDivisionIDAndChannelIDAndDate(long _IndentID, long _ChannelID, DateTime _Date)
        {
            CO_ChannelIndentOfftakes mdlChannelIndent = new CO_ChannelIndentOfftakes();
            List<CO_ChannelIndentOfftakes> lstChannelIndent = db.Repository<CO_ChannelIndentOfftakes>().GetAll().Where(x => x.IndentID == _IndentID && x.ChannelID == _ChannelID).ToList<CO_ChannelIndentOfftakes>();
            mdlChannelIndent = lstChannelIndent.Where(x => x.OfftakeIndentDate.Value.Date == _Date.Date).FirstOrDefault();
            if (mdlChannelIndent == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// this function return SubDivision and Channel by Indent ID
        /// Created On: 16-/8-2016
        /// </summary>
        /// <param name="_IndentID"></param>
        /// <returns>CO_ChannelIndent</returns>
        public CO_ChannelIndent GetSubDivisionAndChannelByIndentID(long _IndentID)
        {
            CO_ChannelIndent mdlChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.ID == _IndentID).FirstOrDefault();
            return mdlChannelIndent;
        }

        /// <summary>
        /// this function will reutrn all dates between rabi and kharif season
        /// Kharif: April to September
        /// Rabi: October to March
        /// </summary>
        /// <param name="_Date"></param>
        /// <returns>List<CO_ChannelIndentOfftakes></returns>
        public List<dynamic> GetAllDatesAndIndentID(long _SubDivID, long _ChannelID, DateTime _Date)
        {
            /****************************************************************/
            List<CO_ChannelIndent> lstChannelIndentOfftakes;
            //List<CO_ChannelIndent> lstDateChannelIndentOfftakes = new List<CO_ChannelIndent>();

            lstChannelIndentOfftakes = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.SubDivID == _SubDivID && x.ParentChannelID == _ChannelID).ToList<CO_ChannelIndent>();

            if (_Date.Month >= 4 && _Date.Month <= 9)
            {
                lstChannelIndentOfftakes = lstChannelIndentOfftakes.Where(x => x.IndentDate.Value.Month >= 4 && x.IndentDate.Value.Month <= 9).OrderByDescending(a => a.IndentDate).ToList<CO_ChannelIndent>();
            }
            else
            {
                lstChannelIndentOfftakes = lstChannelIndentOfftakes.Where(x => x.IndentDate.Value.Month >= 10 || x.IndentDate.Value.Month <= 3).OrderByDescending(a => a.IndentDate).ToList<CO_ChannelIndent>();
            }
            //for (int i = 0; i <= lstChannelIndentOfftakes.Count - 1; i++)
            //{
            //    lstDateChannelIndentOfftakes.Add(Utility.GetFormattedDate(lstChannelIndentOfftakes.ElementAt(i)));
            //}
            List<dynamic> lst = lstChannelIndentOfftakes.ToList().Select(x => new { IndentDate = Utility.GetFormattedDate(x.IndentDate), ID = x.ID }).ToList<dynamic>();
            return lst;
            /****************************************************************/
            //List<dynamic> lstChannelIndentOfftakes;
            ////List<CO_ChannelIndent> lstDateChannelIndentOfftakes = new List<CO_ChannelIndent>();

            //lstChannelIndentOfftakes = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.SubDivID == _SubDivID && x.ParentChannelID == _ChannelID).ToList().Select(x => new { IndentDate = Utility.GetFormattedDate(x.IndentDate), ID = x.ID }).ToList<dynamic>();

            //if (_Date.Month >= 4 && _Date.Month <= 9)
            //{
            //    lstChannelIndentOfftakes = lstChannelIndentOfftakes.Where(x => x.IndentDate.Value.Month >= 4 && x.IndentDate.Value.Month <= 9).OrderByDescending(a => a.IndentDate).ToList<dynamic>();
            //}
            //else
            //{
            //    lstChannelIndentOfftakes = lstChannelIndentOfftakes.Where(x => x.IndentDate.Value.Month >= 10 && x.IndentDate.Value.Month <= 3).OrderByDescending(a => a.IndentDate).ToList<dynamic>();
            //}
            ////for (int i = 0; i <= lstChannelIndentOfftakes.Count - 1; i++)
            ////{
            ////    lstDateChannelIndentOfftakes.Add(Utility.GetFormattedDate(lstChannelIndentOfftakes.ElementAt(i)));
            ////}
            //return lstChannelIndentOfftakes;
        }

        /// <summary>
        /// this function return Current Indent Value on the basis of ChannelID and SubDivID
        /// Cretaed On: 18-08-2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>CO_ChannelIndent</returns>
        public CO_ChannelIndent GetCurrentIndentBySubDivIDAndChannelID(long _SubDivisionID, long _ChannelID, DateTime _IndentPlacementDate)
        {
            List<CO_ChannelIndent> lstChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.SubDivID == _SubDivisionID && x.ParentChannelID == _ChannelID).ToList<CO_ChannelIndent>();

            CO_ChannelIndent mdlChannelIndent = lstChannelIndent.Where(x => x.IndentDate.Value.Date <= _IndentPlacementDate.Date).OrderByDescending(x => x.IndentDate).FirstOrDefault();
            return mdlChannelIndent;
        }

        /// <summary>
        /// this query return  Gauge ID by SubbDivID and ChannelID
        /// Created ON: 19/08/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>long</returns>
        public long GetGaugeIDByChannelIDAndSubDivID(long _SubDivID, long _ChannelID)
        {
            long GaugeID = db.ExtRepositoryFor<IndentRepository>().GetGaugeIDByChannelIDAndSubDivID(_SubDivID, _ChannelID);
            return GaugeID;
        }

        /// <summary>
        /// this function return indent date
        /// Created On: 19/8/2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_GaugeID"></param>
        /// <param name="_Date"></param>
        /// <returns>DateTime?</returns>
        public DateTime? CalculateIndentDate(int _ChannelID, int _GaugeID, DateTime _Date)
        {
            DateTime? IndentDate = db.ExtRepositoryFor<IndentRepository>().CalculateIndentDate(_ChannelID, _GaugeID, _Date);
            return IndentDate;
        }

        //select * from CO_ChannelIndent where ParentChannelID=2602 and SubDivID = 155 order by IndentDate

        //select * from CO_ChannelIndentOfftakes where ChannelID=2603 order by OfftakeIndentDate

        public CO_ChannelIndent GetParentIndentsForNotificationByChannelIDAndSubDivID(long _ChannelID, long _SubdivID)
        {
            CO_ChannelIndent mdlChannelIndent = db.Repository<CO_ChannelIndent>().GetAll().Where(x => x.ParentChannelID == _ChannelID && x.SubDivID == _SubdivID).OrderBy(x => x.IndentDate).FirstOrDefault();
            return mdlChannelIndent;
        }

        public CO_ChannelIndentOfftakes GetChildIndentsForNotificationByChannelID(long _ChannelID)
        {
            CO_ChannelIndentOfftakes mdlChannelIndent = db.Repository<CO_ChannelIndentOfftakes>().GetAll().Where(x => x.ChannelID == _ChannelID).OrderBy(x => x.OfftakeIndentDate).FirstOrDefault();
            return mdlChannelIndent;
        }
        /****************************************************************************************************/
    }
}
