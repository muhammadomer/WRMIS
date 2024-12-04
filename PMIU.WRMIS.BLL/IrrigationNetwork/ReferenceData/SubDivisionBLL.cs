using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class SubDivisionBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all sub divisions with the given Division ID.
        /// Created On 28-10-2015.
        /// </summary>
        /// <param name="_DivisionId"></param>
        /// <returns>List<CO_SubDivision>()</returns>
        public List<CO_SubDivision> GetSubDivisionsByDivisionID(long _DivisionID, bool? _IsActive = null)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.GetSubDivisionsByDivisionID(_DivisionID, _IsActive);
        }

        /// <summary>
        /// This function adds new sub division.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_SubDivision"></param>
        /// <returns>bool</returns>
        public bool AddSubDivision(CO_SubDivision _SubDivision)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.AddSubDivision(_SubDivision);
        }

        /// <summary>
        /// This function updates a sub division.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_SubDivision"></param>
        /// <returns>bool</returns>
        public bool UpdateSubDivision(CO_SubDivision _SubDivision)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.UpdateSubDivision(_SubDivision);
        }

        /// <summary>
        /// This function deletes a sub division with the provided ID.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>bool</returns>
        public bool DeleteSubDivision(long _SubDivisionID)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.DeleteSubDivision(_SubDivisionID);
        }

        /// <summary>
        /// This function checks in all related tables for given Sub Division ID.
        /// Created On 30-10-2015.
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>bool</returns>
        public bool IsSubDivisionIDExists(long _SubDivisionID)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.IsSubDivisionIDExists(_SubDivisionID);
        }

        /// <summary>
        /// This function gets sub division by sub division name and Division ID..
        /// Created On 30-10-2015.
        /// </summary>
        /// <param name="_SubDivisionName"></param>
        /// <param name="_DivisionID"></param>
        /// <returns>CO_SubDivision</returns>
        public CO_SubDivision GetSubDivisionByName(string _SubDivisionName, long _DivisionID)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.GetSubDivisionByName(_SubDivisionName, _DivisionID);
        }

        /// <summary>
        /// this function returns sub division by sub divivison id
        /// Created On: 12/8/2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns></returns>
        public CO_SubDivision GetSubDivisionsBySubDivisionID(long _SubDivisionID)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.GetSubDivisionsBySubDivisionID(_SubDivisionID);
        }

        /// <summary>
        /// This function return sub division based on the sub division id.
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>CO_SubDivision</returns>
        public CO_SubDivision GetByID(long _SubDivisionID)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.GetByID(_SubDivisionID);
        }

        /// <summary>
        /// This function returns data of only those sub divisions whose ID is in the filtered list.
        /// Created On 26-01-2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_FilteredSubDivisionIDs"></param>
        /// <returns>List<CO_SubDivision></returns>
        public List<CO_SubDivision> GetFilteredSubDivisions(long _DivisionID, List<long> _FilteredSubDivisionIDs)
        {
            SubDivisionDAL dalSubDivision = new SubDivisionDAL();

            return dalSubDivision.GetFilteredSubDivisions(_DivisionID, _FilteredSubDivisionIDs);
        }
    }
}
