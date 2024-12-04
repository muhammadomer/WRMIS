using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class StructureDAL
    {
        private ContextDB db = new ContextDB();

        #region "Structure"

        /// <summary>
        /// This function returns list of all rivers.
        /// Created On 04-11-2015.
        /// </summary>
        /// <returns>List<CO_River></returns>
        public List<CO_River> GetAllRivers(bool? _IsActive = null)
        {
            List<CO_River> lstRiver = db.Repository<CO_River>().GetAll().Where(r => (r.IsActive == _IsActive || _IsActive == null)).OrderBy(r => r.Name).ToList<CO_River>();

            return lstRiver;
        }

        /// <summary>
        /// This function returns list of all structure types.
        /// Created On 06-11-2015.
        /// </summary>
        /// <returns>List<CO_StructureType></returns>
        public List<CO_StructureType> GetAllStructureTypes()
        {
            List<CO_StructureType> lstStructureType = db.Repository<CO_StructureType>().GetAll().Where(s => s.ID != (int)Constants.StructureType.Channel).ToList<CO_StructureType>();

            return lstStructureType;
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
            List<CO_Station> lstStation = db.Repository<CO_Station>().GetAll().Where(s => (s.ProvinceID == _ProvinceID || _ProvinceID == -1) &&
                    (s.RiverID == _RiverID || _RiverID == -1) && (s.IsActive == _IsActive || _IsActive == null)).OrderBy(s => s.Name).ToList<CO_Station>();

            return lstStation;
        }

        /// <summary>
        /// This function gets station by station name.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_StationName"></param>
        /// <returns>CO_Station</returns>
        public CO_Station GetStationByName(string _StationName)
        {
            CO_Station mdlStation = db.Repository<CO_Station>().GetAll().Where(s => s.Name.Replace(" ", "").ToUpper() == _StationName.Replace(" ", "").ToUpper()).FirstOrDefault();

            return mdlStation;
        }

        /// <summary>
        /// This function deletes a station with the station ID.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_StationID"></param>
        /// <returns>bool</returns>
        public bool DeleteStation(long _StationID)
        {
            db.Repository<CO_Station>().Delete(_StationID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function adds new station.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_Station"></param>
        /// <returns>bool</returns>
        public bool AddStation(CO_Station _Station)
        {
            db.Repository<CO_Station>().Insert(_Station);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a station.
        /// Created On 05-11-2015.
        /// </summary>
        /// <param name="_Station"></param>
        /// <returns>bool</returns>
        public bool UpdateStation(CO_Station _Station)
        {
            CO_Station mdlStation = db.Repository<CO_Station>().FindById(_Station.ID);

            mdlStation.Name = _Station.Name;
            mdlStation.StructureTypeID = _Station.StructureTypeID;

            db.Repository<CO_Station>().Update(mdlStation);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function returns station based on the station ID.
        /// Created On 01-03-2016
        /// </summary>
        /// <param name="_StationID"></param>
        /// <returns>CO_Station</returns>
        public CO_Station GetStationByID(long _StationID)
        {
            CO_Station mdlStation = db.Repository<CO_Station>().FindById(_StationID);

            return mdlStation;
        }

        /// <summary>
        /// This function searches for a station by part of name.
        /// Created On 18-03-2016
        /// </summary>
        /// <param name="_StationName"></param>
        /// <returns>CO_Station</returns>
        public CO_Station GetStationByPartOfName(string _StationName)
        {
            CO_Station mdlStation = db.Repository<CO_Station>().GetAll().Where(s => s.Name.Replace(" ", "").ToUpper().Contains(_StationName.Replace(" ", "").ToUpper())).FirstOrDefault();

            return mdlStation;
        }

        public List<CO_StructureType> GetParentTypes()
        {
            var idlist = new int[] { 1, 2, 6, 18 };
            List<CO_StructureType> lstParentType = db.Repository<CO_StructureType>().GetAll().Where(x => x.ID == 1 || x.ID == 2 || x.ID == 6 || x.ID == 18).ToList<CO_StructureType>();

            return lstParentType;
        }

        #endregion "Structure"

        #region "Structure Data"

        /// <summary>
        /// This function returns list of all gauge slip sites with the given Gauge Slip Site ID.
        /// Created On 25-11-2015.
        /// </summary>
        /// <param name="_StationId"></param>
        /// <returns>List<CO_GaugeSlipSite></returns>
        public List<CO_GaugeSlipSite> GetGaugeSlipSiteByStationID(long _StationID)
        {
            List<CO_GaugeSlipSite> lstGaugeSlipData = db.Repository<CO_GaugeSlipSite>().GetAll().Where(g => g.StationID == _StationID).OrderBy(g => g.Name).ToList<CO_GaugeSlipSite>();

            return lstGaugeSlipData;
        }

        /// <summary>
        /// This function gets channel that are associated with a station based on Station ID.
        /// Created On 25-11-2015
        /// </summary>
        /// <param name="_StationID"></param>
        /// <returns></returns>
        public List<CO_Channel> GetChannelsByStationID(long _StationID, bool? _IsActive = null)
        {
            string Parent = Convert.ToString(Constants.ChannelRelation.P);

            List<long> lstChannelIDs = db.Repository<CO_ChannelParentFeeder>().GetAll().Where(c => c.ParrentFeederID == _StationID
                && c.RelationType == Parent && c.StructureTypeID != (int)Constants.StructureType.Channel).Select(c => c.ChannelID.Value)
                .ToList<long>();

            lstChannelIDs = lstChannelIDs.Concat(db.Repository<CO_ChannelParentFeeder>().GetAll().Where(c => lstChannelIDs.Contains(c.ParrentFeederID.Value)
                && c.RelationType == Parent && c.StructureTypeID == (int)Constants.StructureType.Channel).Select(c => c.ChannelID.Value)
                .ToList<long>()).ToList<long>();

            List<CO_Channel> lstChannel = db.Repository<CO_Channel>().GetAll().Where(c => lstChannelIDs.Contains(c.ID) && (c.IsActive == _IsActive || _IsActive == null)).OrderBy(c => c.NAME).ToList<CO_Channel>();

            return lstChannel;
        }

        /// <summary>
        /// This function checks in related tables for given Gauge Slip Site ID.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_GaugeSlipSiteID"></param>
        /// <returns>bool</returns>
        public bool IsGaugeSlipSiteIDExists(long _GaugeSlipSiteID)
        {
            bool qGaugeSlipDailyData = db.Repository<CO_GaugeSlipDailyData>().GetAll().Any(g => g.GaugeSlipSiteID == _GaugeSlipSiteID);

            return qGaugeSlipDailyData;
        }

        /// <summary>
        /// This function deletes a gauge slip site with the given Gauge Slip Site ID.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_GaugeSlipSiteID"></param>
        /// <returns>bool</returns>
        public bool DeleteGaugeSlipSite(long _GaugeSlipSiteID)
        {
            db.Repository<CO_GaugeSlipSite>().Delete(_GaugeSlipSiteID);
            db.Save();

            return true;
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
            CO_GaugeSlipSite mdlGaugeSlipSite = db.Repository<CO_GaugeSlipSite>().GetAll().Where(g => g.Name.Replace(" ", "").ToUpper() == _SiteName.Replace(" ", "").ToUpper() &&
                    g.StationID == _StationID && (g.ChannelID == _ChannelID || _ChannelID == -1)).FirstOrDefault();

            return mdlGaugeSlipSite;
        }

        /// <summary>
        /// This function adds new gauge slip site.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_GaugeSlipSite"></param>
        /// <returns>bool</returns>
        public bool AddGaugeSlipSite(CO_GaugeSlipSite _GaugeSlipSite)
        {
            db.Repository<CO_GaugeSlipSite>().Insert(_GaugeSlipSite);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a gauge slip site.
        /// Created On 26-11-2015.
        /// </summary>
        /// <param name="_GaugeSlipSite"></param>
        /// <returns>bool</returns>
        public bool UpdateGaugeSlipSite(CO_GaugeSlipSite _GaugeSlipSite)
        {
            CO_GaugeSlipSite mdlGaugeSlipSite = db.Repository<CO_GaugeSlipSite>().FindById(_GaugeSlipSite.ID);

            mdlGaugeSlipSite.Name = _GaugeSlipSite.Name;
            mdlGaugeSlipSite.ChannelID = _GaugeSlipSite.ChannelID;
            mdlGaugeSlipSite.GaugeID = _GaugeSlipSite.GaugeID;
            mdlGaugeSlipSite.AFSQ = _GaugeSlipSite.AFSQ;
            mdlGaugeSlipSite.Description = _GaugeSlipSite.Description;

            db.Repository<CO_GaugeSlipSite>().Update(mdlGaugeSlipSite);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function gets all the gauges with the given Channel Id.
        /// Created On 27-11-2015
        /// </summary>
        /// <param name="_ChannelId"></param>
        /// <returns>List<CO_ChannelGauge></returns>
        public List<CO_ChannelGauge> GetGaugesByChannelID(long _ChannelID, bool? _IsActive = null)
        {
            List<CO_ChannelGauge> lstChannelGauge = db.Repository<CO_ChannelGauge>().GetAll().Where(c => c.ChannelID == _ChannelID && (c.IsActive == _IsActive || _IsActive == null)).ToList<CO_ChannelGauge>();

            return lstChannelGauge;
        }

        /// <summary>
        /// This function searches for a gauge slip site by part of name.
        /// Created On 18-03-2016
        /// </summary>
        /// <param name="_SiteName"></param>
        /// <returns>CO_GaugeSlipSite</returns>
        public CO_GaugeSlipSite GetGaugeSlipSiteByPartOfName(string _SiteName)
        {
            CO_GaugeSlipSite mdlGaugeSlipSite = db.Repository<CO_GaugeSlipSite>().GetAll().Where(g => g.Name.Replace(" ", "").ToUpper().Contains(_SiteName.Replace(" ", "").ToUpper())).FirstOrDefault();

            return mdlGaugeSlipSite;
        }

        /// <summary>
        /// This function returns Max ID of table
        /// Created On: 14-03-2017
        /// </summary>       
        /// <returns>long</returns>
        public long GetMaxID()
        {
            return db.Repository<CO_GaugeSlipSite>().GetAll().Max(x => x.ID);
        }

        #endregion
    }
}