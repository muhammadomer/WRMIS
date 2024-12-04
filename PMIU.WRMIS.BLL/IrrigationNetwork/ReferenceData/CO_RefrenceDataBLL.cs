using PMIU.WRMIS.DAL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.DAL.Repositories;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.CO_ReferenceData
{
    public class CO_RefrenceDataBLL : BaseBLL 
    {
        /// <summary>
        /// Return all the users records
        /// </summary>
        /// <returns></returns>
        public List<CO_Zone> GetAllZones()
        {
            List<CO_Zone> lstZones = db.Repository <CO_Zone>().Query().Get().ToList<CO_Zone>();

            return lstZones;
        }

        /// <summary>
        /// Returns a single user based upon passed ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CO_Zone GetUserByID(long ID)
        {
            CO_Zone u = db.ExtRepositoryFor<ZoneRepository>().GetUserByID(ID);

            return u;

        }
    }
}
