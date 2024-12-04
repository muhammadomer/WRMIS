using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class PoliceStationDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// This function returns list of all Police Stations
        /// Created On 16-10-2015
        /// </summary>
        /// <returns>List<CO_PoliceStation>()</returns>
        public List<CO_PoliceStation> GetPoliceStationsByTehsilID(long _TehsilID, bool? _IsActive = null)
        {
            List<CO_PoliceStation> lstPoliceStation = db.Repository<CO_PoliceStation>().GetAll().OrderBy(n => n.Name).Where(d => d.TehsilID == _TehsilID && (d.IsActive == _IsActive || _IsActive == null)).ToList<CO_PoliceStation>();
            return lstPoliceStation;
        }

        /// <summary>
        /// This function adds Police Station into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_PoliceStation"></param>
        /// <returns> bool</returns>
        public bool AddPoliceStation(CO_PoliceStation _PoliceStation)
        {
            db.Repository<CO_PoliceStation>().Insert(_PoliceStation);
            db.Save();
            return true;
        }

        /// <summary>
        /// This functon updates Police Station into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_PoliceStation"></param>
        /// <returns>bool</returns>
        public bool UpdatePoliceStation(CO_PoliceStation _PoliceStation)
        {
            CO_PoliceStation mdlPoliceStation = db.Repository<CO_PoliceStation>().FindById(_PoliceStation.ID);

            mdlPoliceStation.Name = _PoliceStation.Name;
            mdlPoliceStation.Description = _PoliceStation.Description;

            db.Repository<CO_PoliceStation>().Update(mdlPoliceStation);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function deletes Police Station from Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_PoliceStation"></param>
        /// <returns> bool</returns>
        public bool DeletePoliceStation(long _PoliceStationID)
        {
            db.Repository<CO_PoliceStation>().Delete(_PoliceStationID);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function checks in all related tables for given police station id.
        /// Created On:30-10-2015
        /// </summary>
        /// <param name="_PoliceStationId"></param>
        /// <returns>bool</returns>
        public bool IsPoliceStationIDExists(long _PoliceStationID)
        {

            bool qIsExists = db.Repository<CO_ChannelAdminBoundries>().GetAll().Any(s => s.PoliceStationID == _PoliceStationID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<CO_Village>().GetAll().Any(s => s.PoliceStationID == _PoliceStationID);
            }

            return qIsExists;
        }

        /// <summary>
        /// this function gets Police Station by Police Station Name
        /// Created On: 30-10-2015
        /// </summary>
        /// <param name="_PoliceStationName"></param>
        /// <param name="_TehsilId"></param>
        /// <returns>CO_PoliceStation</returns>
        public CO_PoliceStation GetPoliceStationByName(string _PoliceStationName, long _TehsilID)
        {
            CO_PoliceStation mdlPoliceStation = db.Repository<CO_PoliceStation>().GetAll().Where(d => d.Name.Trim().ToLower() == _PoliceStationName.Trim().ToLower() && d.TehsilID == _TehsilID).FirstOrDefault();
            return mdlPoliceStation;
        }
    }
}
