using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class ZoneDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns list of all zones.
        /// Created On 15-10-2015.
        /// </summary> 
        /// <returns>List<CO_Zone>()</returns>
        public List<CO_Zone> GetAllZones(bool? _IsActive=null)
        {
            List<CO_Zone> lstZone = db.Repository<CO_Zone>().GetAll().Where(c => (c.IsActive  == _IsActive || _IsActive == null)).OrderBy(z => z.Name).ToList<CO_Zone>();

            return lstZone;
        }

        /// <summary>
        /// This function adds new zone.
        /// Created On 15-10-2015.
        /// </summary>
        /// <param name="_Zone"></param>
        /// <returns>bool</returns>
        public bool AddZone(CO_Zone _Zone)
        {
            db.Repository<CO_Zone>().Insert(_Zone);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a zone.
        /// Created On 15-10-2015.
        /// </summary>
        /// <param name="_Zone"></param>
        /// <returns>bool</returns>
        public bool UpdateZone(CO_Zone _Zone)
        {
            db.Repository<CO_Zone>().Update(_Zone);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes a zone with the provided ID.
        /// Created On 15-10-2015.
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>bool</returns>
        public bool DeleteZone(long _ZoneID)
        {
            db.Repository<CO_Zone>().Delete(_ZoneID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function gets zone by zone name.
        /// Created On 20-10-2015.
        /// </summary>
        /// <param name="_ZoneName"></param>
        /// <returns>CO_Zone</returns>
        public CO_Zone GetZoneByName(string _ZoneName)
        {
            CO_Zone mdlZone = db.Repository<CO_Zone>().GetAll().Where(z => z.Name.Replace(" ", "").ToUpper() == _ZoneName.Replace(" ", "").ToUpper()).FirstOrDefault();

            return mdlZone;
        }

        /// <summary>
        /// This function checks in Circle table for given Zone ID.
        /// Created On 22-10-2015.
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>bool</returns>
        public bool IsZoneIDExists(long _ZoneID)
        {
            bool qCircle = db.Repository<CO_Circle>().GetAll().Any(c => c.ZoneID == _ZoneID);

            return qCircle;
        }

        /// <summary>
        /// This function returns data of only those zones whose ID is in the filtered list.
        /// Created On 26-01-2016
        /// </summary>
        /// <param name="_FilteredZoneIDs"></param>
        /// <returns>List<CO_Zone></returns>
        public List<CO_Zone> GetFilteredZones(List<long> _FilteredZoneIDs)
        {
            List<CO_Zone> lstZone = db.Repository<CO_Zone>().GetAll().Where(z => _FilteredZoneIDs.Contains(z.ID)).ToList<CO_Zone>();

            return lstZone;
        }

        /// <summary>
        /// This function returns zone based on ID
        /// Created On 03-03-2017
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <returns>CO_Zone</returns>
        public CO_Zone GetZoneByID(long _ZoneID)
        {
            return db.Repository<CO_Zone>().FindById(_ZoneID);
        }
    }
}
