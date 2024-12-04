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
    public class VillageDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// this function returns list of all Villages
        /// Created On:19-10-2015
        /// </summary>
        /// <returns>List<CO_Village></returns>
        public List<CO_Village> GetVillagesByPoliceStationID(long _PoliceStationID, bool? _IsActive = null)
        {
            List<CO_Village> lstVillage = db.Repository<CO_Village>().GetAll().OrderBy(n => n.Name).Where(s => s.PoliceStationID == _PoliceStationID && (s.IsActive == _IsActive || _IsActive == null)).ToList<CO_Village>();
            return lstVillage;
        }
        public List<dynamic> GetVillagesByPoliceStationIDForinfrastructure(long _PoliceStationID)
        {
            List<dynamic> lstVillage = db.Repository<CO_Village>().GetAll().OrderBy(n => n.Name).Where(s => s.PoliceStationID.Value == _PoliceStationID).OrderBy(n => n.Name).Select(x => new { x.Name, x.TehsilID, x.PoliceStationID }).Distinct().ToList<dynamic>();
            return lstVillage;
        }

        /// <summary>
        /// this function adds Villages into the Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_Village"></param>
        /// <returns>bool</returns>
        public bool AddVillage(CO_Village _Village)
        {
            db.Repository<CO_Village>().Insert(_Village);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function updates Village into the Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_Village"></param>
        /// <returns>bool</returns>
        public bool UpdateVillage(CO_Village _Village)
        {
            CO_Village mdlVillage = db.Repository<CO_Village>().FindById(_Village.ID);
            mdlVillage.Name = _Village.Name;
            mdlVillage.Description = _Village.Description;
            db.Repository<CO_Village>().Update(mdlVillage);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function deletes Village from Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_Village"></param>
        /// <returns>bool</returns>
        public bool DeleteVillage(long _VillageID)
        {
            db.Repository<CO_Village>().Delete(_VillageID);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function checks in all related tables for given Village Id.
        /// Created On:2-11-2015
        /// </summary>
        /// <param name="_VillageId"></param>
        /// <returns>bool</returns>
        public bool IsVillageIDExists(long _VillageID)
        {
            bool qIsExists = db.Repository<CO_ChannelAdminBoundries>().GetAll().Any(s => s.VillageID == _VillageID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<CO_ChannelOutletsLocation>().GetAll().Any(s => s.VillageID == _VillageID);
            }

            return qIsExists;
        }

        /// <summary>
        /// this function gets Village by Village Name.
        /// Created On:2-11-2015
        /// </summary>
        /// <param name="_VillageName"></param>
        /// <param name="_PoliceStationId"></param>
        /// <returns>CO_Village</returns>
        public CO_Village GetVillageByName(string _VillageName, long _PoliceStationID)
        {
            CO_Village mdlVillage = db.Repository<CO_Village>().GetAll().Where(s => s.Name.Trim().ToLower() == _VillageName.Trim().ToLower() && s.PoliceStationID == _PoliceStationID).FirstOrDefault();
            return mdlVillage;
        }
    }
}
