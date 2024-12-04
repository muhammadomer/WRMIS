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
    public class VillageBLL : BaseBLL
    {
        /// <summary>
        /// this function returns list of all Villages
        /// Created On:19-10-2015
        /// </summary>
        /// <returns>List<CO_Village></returns>
        public List<CO_Village> GetVillagesByPoliceStationID(long _PoliceStationID, bool? _IsActive = null)
        {
            VillageDAL dalVillage = new VillageDAL();
            return dalVillage.GetVillagesByPoliceStationID(_PoliceStationID,_IsActive);
        }

        public List<dynamic> GetVillagesByPoliceStationIDForinfrastructure(long _PoliceStationID)
        {
            VillageDAL dalVillage = new VillageDAL();
            return dalVillage.GetVillagesByPoliceStationIDForinfrastructure(_PoliceStationID);

        }

        /// <summary>
        /// this function adds Villages into the Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_Village"></param>
        /// <returns>bool</returns>
        public bool AddVillage(CO_Village _Village)
        {
            VillageDAL dalVillage = new VillageDAL();
            return dalVillage.AddVillage(_Village);
        }

        /// <summary>
        /// this function updates Village into the Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_Village"></param>
        /// <returns>bool</returns>
        public bool UpdateVillage(CO_Village _Village)
        {
            VillageDAL dalVillage = new VillageDAL();
            return dalVillage.UpdateVillage(_Village);
        }

        /// <summary>
        /// this function deletes Village from Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_Village"></param>
        /// <returns>bool</returns>
        public bool DeleteVillage(long _VillageID)
        {
            VillageDAL dalVillage = new VillageDAL();
            return dalVillage.DeleteVillage(_VillageID);
        }

        /// <summary>
        /// this function checks in all related tables for given Village Id.
        /// Created On:2-11-2015
        /// </summary>
        /// <param name="_VillageId"></param>
        /// <returns>bool</returns>
        public bool IsVillageIDExists(long _VillageID)
        {
            VillageDAL dalVillage = new VillageDAL();
            return dalVillage.IsVillageIDExists(_VillageID);
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
            VillageDAL dalVillage = new VillageDAL();
            return dalVillage.GetVillageByName(_VillageName, _PoliceStationID);
        }
    }
}
