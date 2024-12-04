using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class PoliceStationBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all Police Stations
        /// Created On 16-10-2015
        /// </summary>
        /// <returns>List<CO_PoliceStation>()</returns>
        public List<CO_PoliceStation> GetPoliceStationsByTehsilID(long _TehsilID, bool? _IsActive = null)
        {
            PoliceStationDAL dalPoliceStation = new PoliceStationDAL();
            return dalPoliceStation.GetPoliceStationsByTehsilID(_TehsilID,_IsActive);
        }

        /// <summary>
        /// This function adds Police Station into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_PoliceStation"></param>
        /// <returns> bool</returns>
        public bool AddPoliceStation(CO_PoliceStation _PoliceStation)
        {
            PoliceStationDAL dalPoliceStation = new PoliceStationDAL();
            return dalPoliceStation.AddPoliceStation(_PoliceStation);
        }

        /// <summary>
        /// This functon updates Police Station into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_PoliceStation"></param>
        /// <returns>bool</returns>
        public bool UpdatePoliceStation(CO_PoliceStation _PoliceStation)
        {
            PoliceStationDAL dalPoliceStation = new PoliceStationDAL();
            return dalPoliceStation.UpdatePoliceStation(_PoliceStation);
        }

        /// <summary>
        /// this function deletes Police Station from Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_PoliceStation"></param>
        /// <returns> bool</returns>
        public bool DeletePoliceStation(long _PoliceStationID)
        {
            PoliceStationDAL dalPoliceStation = new PoliceStationDAL();
            return dalPoliceStation.DeletePoliceStation(_PoliceStationID);
        }

        /// <summary>
        /// this function checks in all related tables for given police station id.
        /// Created On:30-10-2015
        /// </summary>
        /// <param name="_PoliceStationId"></param>
        /// <returns>bool</returns>
        public bool IsPoliceStationIDExists(long _PoliceStationID)
        {
            PoliceStationDAL dalPoliceStation = new PoliceStationDAL();
            return dalPoliceStation.IsPoliceStationIDExists(_PoliceStationID);
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
            PoliceStationDAL dalPoliceStation = new PoliceStationDAL();
            return dalPoliceStation.GetPoliceStationByName(_PoliceStationName, _TehsilID);
        }
    }
}
