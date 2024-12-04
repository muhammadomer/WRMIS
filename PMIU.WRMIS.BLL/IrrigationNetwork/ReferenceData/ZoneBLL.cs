using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class ZoneBLL : BaseBLL
    {
        ZoneDAL dalZone = new ZoneDAL();

        /// <summary>
        /// This function returns list of all zones.
        /// Created On 15-10-2015.
        /// </summary> 
        /// <returns>List<CO_Zone>()</returns>
        public List<CO_Zone> GetAllZones(bool? _IsActive = null)
        {
            return dalZone.GetAllZones(_IsActive);
        }

        /// <summary>
        /// This function adds new zone.
        /// Created On 15-10-2015.
        /// </summary>
        /// <param name="_Zone"></param>
        /// <returns>bool</returns>
        public bool AddZone(CO_Zone _Zone)
        {
            return dalZone.AddZone(_Zone);
        }

        /// <summary>
        /// This function updates a zone.
        /// Created On 15-10-2015.
        /// </summary>
        /// <param name="_Zone"></param>
        /// <returns>bool</returns>
        public bool UpdateZone(CO_Zone _Zone)
        {
            return dalZone.UpdateZone(_Zone);
        }

        /// <summary>
        /// This function deletes a zone with the provided ID.
        /// Created On 15-10-2015.
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>bool</returns>
        public bool DeleteZone(long _ZoneID)
        {
            return dalZone.DeleteZone(_ZoneID);
        }

        /// <summary>
        /// This function gets zone by zone name.
        /// Created On 20-10-2015.
        /// </summary>
        /// <param name="_ZoneName"></param>
        /// <returns>CO_Zone</returns>
        public CO_Zone GetZoneByName(string _ZoneName)
        {
            return dalZone.GetZoneByName(_ZoneName);
        }

        /// <summary>
        /// This function checks in Circle table for given Zone ID.
        /// Created On 22-10-2015.
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>bool</returns>
        public bool IsZoneIDExists(long _ZoneID)
        {
            return dalZone.IsZoneIDExists(_ZoneID);
        }

        /// <summary>
        /// This function returns data of only those zones whose ID is in the filtered list.
        /// Created On 26-01-2016
        /// </summary>
        /// <param name="_FilteredZoneIDs"></param>
        /// <returns>List<CO_Zone></returns>
        public List<CO_Zone> GetFilteredZones(List<long> _FilteredZoneIDs)
        {
            return dalZone.GetFilteredZones(_FilteredZoneIDs);
        }

        /// <summary>
        /// This function returns zone based on ID
        /// Created On 03-03-2017
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>CO_Zone</returns>
        public CO_Zone GetZoneByID(long _ZoneID)
        {
            return dalZone.GetZoneByID(_ZoneID);
        }
    }
}
