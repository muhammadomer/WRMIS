using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class StructureBLL : BaseBLL
    {
        #region "Structure"

        /// <summary>
        /// This function returns list of all rivers.
        /// Created On 04-11-2015.
        /// </summary>
        /// <returns>List<CO_River></returns>
        public List<CO_River> GetAllRivers(bool? _IsActive = null)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetAllRivers(_IsActive);
        }

        /// <summary>
        /// This function returns list of all structure types.
        /// Created On 06-11-2015.
        /// </summary>
        /// <returns>List<CO_StructureType></returns>
        public List<CO_StructureType> GetAllStructureTypes()
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetAllStructureTypes();
        }

        /// <summary>
        /// This function returns list of all stations with given Province ID and River ID.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_ProvinceId"></param>
        /// <param name="_RiverId"></param>
        /// <returns>List<CO_Station></returns>
        public List<CO_Station> GetStations(long _ProvinceID = -1, long _RiverID = -1, bool? _IsActive = null)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetStations(_ProvinceID, _RiverID, _IsActive);
        }

        /// <summary>
        /// This function gets station by station name.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_StationName"></param>
        /// <returns>CO_Station</returns>
        public CO_Station GetStationByName(string _StationName)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetStationByName(_StationName);
        }

        /// <summary>
        /// This function deletes a station with the station ID.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_StationID"></param>
        /// <returns>bool</returns>
        public bool DeleteStation(long _StationID)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.DeleteStation(_StationID);
        }

        /// <summary>
        /// This function adds new station.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_Station"></param>
        /// <returns>bool</returns>
        public bool AddStation(CO_Station _Station)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.AddStation(_Station);
        }

        /// <summary>
        /// This function updates a station.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_Station"></param>
        /// <returns>bool</returns>
        public bool UpdateStation(CO_Station _Station)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.UpdateStation(_Station);
        }

        /// <summary>
        /// This function returns station based on the station ID.
        /// Created On 01-03-2016
        /// </summary>
        /// <param name="_StationID"></param>
        /// <returns>CO_Station</returns>
        public CO_Station GetStationByID(long _StationID)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetStationByID(_StationID);
        }

        /// <summary>
        /// This function searches for a station by part of name.
        /// Created On 18-03-2016
        /// </summary>
        /// <param name="_StationName"></param>
        /// <returns>CO_Station</returns>
        public CO_Station GetStationByPartOfName(string _StationName)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetStationByPartOfName(_StationName);
        }

        #endregion

        #region "Structure Data"

        /// <summary>
        /// This function returns list of all gauge slip sites with the given Gauge Slip Site ID.
        /// Created On 25-11-2015.
        /// </summary>
        /// <param name="_StationId"></param>
        /// <returns>List<CO_GaugeSlipSite></returns>
        public List<CO_GaugeSlipSite> GetGaugeSlipSiteByStationID(long _StationID)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetGaugeSlipSiteByStationID(_StationID);
        }

        /// <summary>
        /// This function gets channel that are associated with a station based on Station ID.
        /// Created On 25-11-2015
        /// </summary>
        /// <param name="_StationID"></param>
        /// <returns></returns>
        public List<CO_Channel> GetChannelsByStationID(long _StationID, bool? _IsActive = null)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetChannelsByStationID(_StationID, _IsActive);
        }

        /// <summary>
        /// This function checks in related tables for given Gauge Slip Site ID.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_GaugeSlipSiteID"></param>
        /// <returns>bool</returns>
        public bool IsGaugeSlipSiteIDExists(long _GaugeSlipSiteID)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.IsGaugeSlipSiteIDExists(_GaugeSlipSiteID);
        }

        /// <summary>
        /// This function deletes a gauge slip site with the given Gauge Slip Site ID.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_GaugeSlipSiteID"></param>
        /// <returns>bool</returns>
        public bool DeleteGaugeSlipSite(long _GaugeSlipSiteID)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.DeleteGaugeSlipSite(_GaugeSlipSiteID);
        }

        /// <summary>
        /// This function gets gauge slip site by site name and Channel ID.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_SiteName"></param>
        /// <param name="_ChannelID"></param>
        /// /// <param name="_StationID"></param>
        /// <returns>CO_GaugeSlipSite</returns>
        public CO_GaugeSlipSite GetGaugeSlipSiteBySiteNameAndChannelID(string _SiteName, long _ChannelID, long _StationID)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetGaugeSlipSiteBySiteNameAndChannelID(_SiteName, _ChannelID, _StationID);
        }

        /// <summary>
        /// This function adds new gauge slip site.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_GaugeSlipSite"></param>
        /// <returns>bool</returns>
        public bool AddGaugeSlipSite(CO_GaugeSlipSite _GaugeSlipSite)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.AddGaugeSlipSite(_GaugeSlipSite);
        }

        /// <summary>
        /// This function updates a gauge slip site.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_GaugeSlipSite"></param>
        /// <returns>bool</returns>
        public bool UpdateGaugeSlipSite(CO_GaugeSlipSite _GaugeSlipSite)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.UpdateGaugeSlipSite(_GaugeSlipSite);
        }

        /// <summary>
        /// This function gets all the gauges with the given Channel Id.
        /// Created On 27-11-2015
        /// </summary>
        /// <param name="_ChannelId"></param>
        /// <returns>List<CO_ChannelGauge></returns>
        public List<CO_ChannelGauge> GetGaugesByChannelID(long _ChannelID, bool? _IsActive = null)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetGaugesByChannelID(_ChannelID, _IsActive);
        }

        /// <summary>
        /// This function searches for a gauge slip site by part of name.
        /// Created On 18-03-2016
        /// </summary>
        /// <param name="_SiteName"></param>
        /// <returns>CO_GaugeSlipSite</returns>
        public CO_GaugeSlipSite GetGaugeSlipSiteByPartOfName(string _SiteName)
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetGaugeSlipSiteByPartOfName(_SiteName);
        }

        #endregion
        /// <summary>
        /// This function returns list of all Parent types.
        /// Created On 06-11-2015.
        /// </summary>
        /// <returns>List<CO_StructureType></returns>
        public List<CO_StructureType> GetParentTypes()
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetParentTypes();
        }

        /// <summary>
        /// This function returns Max ID of table
        /// Created On: 14-03-2017
        /// </summary>       
        /// <returns>long</returns>
        public long GetMaxID()
        {
            StructureDAL dalStructure = new StructureDAL();

            return dalStructure.GetMaxID();
        }
    }
}
