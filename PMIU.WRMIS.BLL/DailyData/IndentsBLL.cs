using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.DailyData;
using PMIU.WRMIS.DAL.DataAccess.DailyData;
using System.Data;


namespace PMIU.WRMIS.BLL.DailyData
{
    public class IndentsBLL : BaseBLL
    {
        /// <summary>
        /// this function return list of all channels
        /// Created On: 1-12-2015
        /// </summary>
        /// <returns>List<CO_Channel></returns>
        public List<dynamic> GetAllChannels(long _SubDivID, long _CommandTypeID, long _ChannelTypeID, long _FlowTypeID, long _ChannelNameID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetAllChannels(_SubDivID, _CommandTypeID, _ChannelTypeID, _FlowTypeID, _ChannelNameID);
        }



        /// <summary>
        /// this function adds Indents in Channel Indent Table
        /// Created On: 9/12/2015
        /// </summary>
        /// <param name="_ChannelIndent"></param>
        /// <returns>bool</returns>
        public bool AddIndnet(CO_ChannelIndent _ChannelIndent)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.AddIndnet(_ChannelIndent);
        }

        /// <summary>
        /// this function update indent in channel indent table
        /// Created On: 9/12/2015
        /// </summary>
        /// <param name="_ChannelIndent"></param>
        /// <returns>bool</returns>
        public bool UpdateIndent(CO_ChannelIndent _ChannelIndent)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.UpdateIndent(_ChannelIndent);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetMaxDateByChannelID(_ChannelID, _SubDivID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetSectionBySubDivIDAndChannelID(_SubDivID, _ChannelID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetDesignDischargeBySectionIDAndChannelID(_SectionID, _ChannelID);
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

            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetIndentHistoryByChannelIDAndSubDivID(_ChannelID, _SubDivID, _FromDate, _ToDate);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetLowerSubDivision(_ChannelID, _SubDivID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetIndentBySubDivIDandChannelID(_ChannelID, _SubDivID);
        }

        /// <summary>
        /// this function return Indent Value on the basis of ChannelID and SubDivID and date
        /// Created on: 23/04/2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_SubDivID"></param>
        /// <param name="_Date"></param>
        /// <returns>CO_ChannelIndent</returns>
        public CO_ChannelIndent GetIndentBySubDivIDandChannelIDandDate(long _ChannelID, long _SubDivID, DateTime _Date)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetIndentBySubDivIDandChannelIDandDate(_ChannelID, _SubDivID, _Date);
        }



        /// <summary>
        /// this function return list of all Sub Division on the Basis of Channel ID
        /// Created On: 12-02-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<long></returns>
        public List<long> GetListOfSubDivisions(long _ChannelID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetListOfSubDivisions(_ChannelID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetIndentWhereNotAutoGenerated(_ChannelID, _SubDivID, _Date);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChannelsByUserID(_UserID, _IrrigationLevelID, _SubDivID);
            //List<dynamic> lstChannelIDs = dalIndents.GetChannelsByUserID(_UserID, _IrrigationLevelID, _SubDivID).Select(dataRow => new
            //{
            //    ID = dataRow.Field<long>("ChannelID"),
            //    NAME = dataRow.Field<string>("ChannelName"),
            //    SortOrder = dataRow.Field<string>("SortOrder"),
            //    ParentChild = dataRow.Field<string>("ParentChild"),
            //}).ToList<dynamic>();
            //return lstChannelIDs;//dalIndents.GetChannelsByUserID(_UserID, _IrrigationLevelID);
        }

        /// <summary>
        /// this function get data from Gauge Lag Table by ChannelID
        /// Created On: 01-08-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_GaugeLag></returns>
        public List<CO_GaugeLag> GetGaugeLagByChannelID(long _ChannelID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetGaugeLagByChannelID(_ChannelID);
        }

        /// <summary>
        /// This function updates Gauge Lag into the Database
        /// Created On: 01-08-2016
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>bool</returns>
        public bool UpdateGaugeLag(CO_GaugeLag _GaugeLag)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.UpdateGaugeLag(_GaugeLag);
        }

        /// <summary>
        /// this function get SubDivisions on the basis of User ID
        /// Created On:02-08-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>CO_SubDivision</returns>
        public List<CO_SubDivision> GetSubDivisionOFSDOByUserID(long _UserID, long? _IrrigationLevelID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetSubDivisionOFSDOByUserID(_UserID, _IrrigationLevelID);
        }

        /// <summary>
        /// this function return all Channel by name on the basis of sub division id or division id
        /// Created On: 7/12/2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetChannelsNameBySubDivisionIDOrDivisionID(long? _SubDivisionID, long? _DivisionID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChannelsNameBySubDivisionIDOrDivisionID(_SubDivisionID, _DivisionID);
        }

        /// <summary>
        /// this function get SubDivision on the basis of User ID
        /// Created On:12-02-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>UA_AssociatedLocation</returns>
        public UA_AssociatedLocation GetSubDivisionByUserID(long _UserID, long? _IrrigationLevelID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetSubDivisionByUserID(_UserID, _IrrigationLevelID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetSubDivisionsOFXENByUserID(_UserID, _IrrigationLevelID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetIndentsBySubDivisionIDOrChannelID(_SubDivisionID, _ChannelID, _IndentPlacementDate);
        }

        /// <summary>
        /// this function will return all offtakes on the basis of indent ID
        /// Created On: 05-08-2016
        /// </summary>
        /// <param name="_IndentID"></param>
        /// <returns>List<CO_ChannelIndentOfftakes></returns>
        public List<CO_ChannelIndentOfftakes> GetChannelIndentOfftakes(long _IndentID, DateTime _Date)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChannelIndentOfftakes(_IndentID, _Date);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetAllDates(_Date);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetOfftakesByOfftakeID(_OfftakeID, _Date);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChannelsByUserIDAndSubDivID(_UserID, _IrrigationLevelID, _SubDivID, _ParentChild);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetIndentIDBySubDivIDAndParentChannelIDAndDate(_SubDivID, _ChannelID, _Date);
        }

        /// <summary>
        /// this function will return all offtakes on the basis of indent ID
        /// Created On: 05-08-2016
        /// </summary>
        /// <param name="_IndentID"></param>
        /// <returns>List<CO_ChannelIndentOfftakes></returns>
        public List<CO_ChannelIndentOfftakes> GetChannelIndentOfftakesForAddIndent(long? _IndentID, DateTime _Date)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChannelIndentOfftakesForAddIndent(_IndentID, _Date);
        }

        public CO_Channel GetOfftakesByParrentIDFirstTime(long _ChannelID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetOfftakesByParrentIDFirstTime(_ChannelID);
        }

        public List<DD_GetChannelsByUserIDForIndents_Result> GetChannelsByUserIDAndSubDivIDTest(long _UserID, long _IrrigationLevelID, long _SubDivID, string _ParentChild)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChannelsByUserIDAndSubDivIDTest(_UserID, _IrrigationLevelID, _SubDivID, _ParentChild);
        }

        public List<dynamic> GetChannelIndentsOffTakesFromOffTakesTable(long _IndentID, DateTime _Date)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChannelIndentsOffTakesFromOffTakesTable(_IndentID, _Date);
        }


        public dynamic GetChannelIndentsOffTakesFromChannelTable(long _ChannelID, DateTime _Date)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChannelIndentsOffTakesFromChannelTable(_ChannelID, _Date);
        }

        /// <summary>
        /// this function adds Indents in Channel Indent Table
        /// Created On: 11/08/2016
        /// </summary>
        /// <param name="_ChannelIndentOfftake"></param>
        /// <returns>bool</returns>
        public bool AddChannelIndnetOfftakes(CO_ChannelIndentOfftakes _ChannelIndentOfftake)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.AddChannelIndnetOfftakes(_ChannelIndentOfftake);
        }

        /// <summary>
        /// this function updates Indents in Channel Indent Offtake Table
        /// Created On: 11/08/2016
        /// </summary>
        /// <param name="_ChannelIndentOfftake"></param>
        /// <returns>bool</returns>
        public bool UpdateChannelIndnetOfftakes(CO_ChannelIndentOfftakes _ChannelIndentOfftake)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.UpdateChannelIndnetOfftakes(_ChannelIndentOfftake);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetSubDivGaugeID(_SubDivID, _ChannelID, _GaugeCategoryID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetOutletIndent(_SubDivID, _ChannelID);
        }

        /// <summary>
        /// this function will return Indents By Sub Division ID Or Channel ID
        /// Created On: 03-08-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_IndentPlacementDate"></param>
        /// <returns>List<CO_ChannelIndent></returns>
        public List<CO_ChannelIndent> GetIndentBySubDivisionIDAndChannelIDAndDate(long _SubDivisionID, long _ChannelID, DateTime _IndentPlacementDate)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetIndentBySubDivisionIDAndChannelIDAndDate(_SubDivisionID, _ChannelID, _IndentPlacementDate);
        }

        /// <summary>
        /// this function will return Offtakes By Sub Division ID and Channel ID and date
        /// Created On: 03-08-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_ChannelID"></param>
        /// <param name="_IndentPlacementDate"></param>
        /// <returns>List<CO_ChannelIndentOfftakes></returns>
        public bool GetOfftakeIndentBySubDivisionIDAndChannelIDAndDate(long _IndentID, long _ChannelID, DateTime _Date)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetOfftakeIndentBySubDivisionIDAndChannelIDAndDate(_IndentID, _ChannelID, _Date);
        }

        /// <summary>
        /// this function return SubDivision and Channel by Indent ID
        /// Created On: 16-/8-2016
        /// </summary>
        /// <param name="_IndentID"></param>
        /// <returns>CO_ChannelIndent</returns>
        public CO_ChannelIndent GetSubDivisionAndChannelByIndentID(long _IndentID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetSubDivisionAndChannelByIndentID(_IndentID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetAllDatesAndIndentID(_SubDivID, _ChannelID, _Date);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetCurrentIndentBySubDivIDAndChannelID(_SubDivisionID, _ChannelID, _IndentPlacementDate);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetGaugeIDByChannelIDAndSubDivID(_SubDivID, _ChannelID);
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
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.CalculateIndentDate(_ChannelID, _GaugeID, _Date);
        }

        public CO_ChannelIndent GetParentIndentsForNotificationByChannelIDAndSubDivID(long _ChannelID, long _SubdivID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetParentIndentsForNotificationByChannelIDAndSubDivID(_ChannelID, _SubdivID);
        }

        public CO_ChannelIndentOfftakes GetChildIndentsForNotificationByChannelID(long _ChannelID)
        {
            IndentsDAL dalIndents = new IndentsDAL();
            return dalIndents.GetChildIndentsForNotificationByChannelID(_ChannelID);
        }
        /****************************************************************************************************/
    }
}
