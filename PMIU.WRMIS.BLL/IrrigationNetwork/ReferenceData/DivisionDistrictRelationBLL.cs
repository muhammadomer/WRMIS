using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class DivisionDistrictRelationBLL : BaseBLL
    {
        /// <summary>
        /// this function returns districts by Division id
        /// Created On:04-11-2015
        /// </summary>
        /// <param name="_DivisionId"></param>
        /// <returns>List<CO_DistrictDivision></returns>
        public List<CO_DistrictDivision> GetAllDistrictsByDivisionID(long _DivisionID)
        {
            DivisionDistrictRelationDAL dalDDR = new DivisionDistrictRelationDAL();
            return dalDDR.GetAllDistrictsByDivisionID(_DivisionID);
        }

        /// <summary>
        /// this function adds district division in database
        /// Created On: 05-11-2015
        /// </summary>
        /// <param name="_DistrictDivision"></param>
        /// <returns>bool</returns>
        public bool AddDistrictDivision(CO_DistrictDivision _DistrictDivision)
        {
            DivisionDistrictRelationDAL dalDDR = new DivisionDistrictRelationDAL();
            return dalDDR.AddDistrictDivision(_DistrictDivision);
        }

        /// <summary>
        /// this function deletes District from District Division Table in database
        /// Created On:05-10-2015
        /// </summary>
        /// <param name="_DistrictId"></param>
        /// <returns>bool</returns>
        public bool DeleteDistrictDivision(long _DistrictID, long _DivisionID)
        {
            DivisionDistrictRelationDAL dalDDR = new DivisionDistrictRelationDAL();
            return dalDDR.DeleteDistrictDivision(_DistrictID, _DivisionID);
        }
    }
}
