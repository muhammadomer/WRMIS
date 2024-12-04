using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Exceptions;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class DivisionDistrictRelationDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// this function returns districts by Division id
        /// Created On:04-11-2015
        /// </summary>
        /// <param name="_DivisionId"></param>
        /// <returns>List<CO_DistrictDivision></returns>
        public List<CO_DistrictDivision> GetAllDistrictsByDivisionID(long _DivisionID)
        {
            List<CO_DistrictDivision> lstDistrictDivision = db.Repository<CO_DistrictDivision>().GetAll().Where(s => s.DivisionID == _DivisionID).ToList<CO_DistrictDivision>();
            return lstDistrictDivision;
        }

        /// <summary>
        /// this function adds district division in database
        /// Created On: 05-11-2015
        /// </summary>
        /// <param name="_DistrictDivision"></param>
        /// <returns>bool</returns>
        public bool AddDistrictDivision(CO_DistrictDivision _DistrictDivision)
        {
            db.Repository<CO_DistrictDivision>().Insert(_DistrictDivision);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function deletes District from District Division Table in database
        /// Created On:05-10-2015
        /// </summary>
        /// <param name="_DistrictId"></param>
        /// <returns>bool</returns>
        public bool DeleteDistrictDivision(long _DistrictID, long _DivisionID)
        {
            CO_DistrictDivision mdlDistrictDivision = db.Repository<CO_DistrictDivision>().GetAll().Where(s => s.DistrictID == _DistrictID && s.DivisionID == _DivisionID).FirstOrDefault();
            db.Repository<CO_DistrictDivision>().Delete(mdlDistrictDivision);
            db.Save();
            return true;
        }
    }
}
