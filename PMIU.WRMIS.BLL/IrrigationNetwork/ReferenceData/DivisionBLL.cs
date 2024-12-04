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
    public class DivisionBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all divisions with the given Circle ID.
        /// Created On 28-10-2015.
        /// </summary>
        /// <param name="_CircleId"></param>
        /// <returns>List<CO_Division>()</returns>
        public List<CO_Division> GetDivisionsByCircleIDAndDomainID(long _CircleID = -1, long _DomainID = -1, bool? _IsActive = null)
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.GetDivisionsByCircleIDAndDomainID(_CircleID, _DomainID, _IsActive);
        }

        public long GetDivisionIDBySubDivisionID(long _SubDivisionID)
        {
            DivisionDAL dalDivision = new DivisionDAL();
            return dalDivision.GetDivisionIDBySubDivisionID(_SubDivisionID);
        }

        /// <summary>
        /// This function adds new division.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_Division"></param>
        /// <returns>bool</returns>
        public bool AddDivision(CO_Division _Division)
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.AddDivision(_Division);
        }

        /// <summary>
        /// This function updates a division.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_Division"></param>
        /// <returns>bool</returns>
        public bool UpdateDivision(CO_Division _Division)
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.UpdateDivision(_Division);
        }

        /// <summary>
        /// This function deletes a division with the provided ID.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>bool</returns>
        public bool DeleteDivision(long _DivisionID)
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.DeleteDivision(_DivisionID);
        }

        /// <summary>
        /// This function checks in all related tables for given Division ID.
        /// Created On 28-10-2015.
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>bool</returns>
        public bool IsDivisionIDExists(long _DivisionID)
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.IsDivisionIDExists(_DivisionID);
        }

        /// <summary>
        /// This function gets division by division name and Circle ID.
        /// Created On 28-10-2015.
        /// </summary>
        /// <param name="_DivisionName"></param>
        /// <param name="_CircleID"></param>
        /// <returns>CO_Division</returns>
        public CO_Division GetDivisionByName(string _DivisionName, long _CircleID)
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.GetDivisionByName(_DivisionName, _CircleID);
        }

        /// <summary>
        /// This function get all domains
        /// Created On 01-12-2015
        /// </summary>
        /// <returns>List<CO_Domain></returns>
        public List<CO_Domain> GetDomains()
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.GetDomains();
        }

        public List<CO_Division> GetAllDivisionNames()
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.GetAllDivisionNames();
        }

        /// <summary>
        /// This function return division based on the division id.
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>CO_Division</returns>
        public CO_Division GetByID(long _DivisionID)
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.GetByID(_DivisionID);
        }

        /// <summary>
        /// This function returns data of only those divisions whose ID is in the filtered list.
        /// Created On 26-01-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <param name="_FilteredDivisionIDs"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetFilteredDivisions(long _CircleID, List<long> _FilteredDivisionIDs)
        {
            DivisionDAL dalDivision = new DivisionDAL();

            return dalDivision.GetFilteredDivisions(_CircleID, _FilteredDivisionIDs);
        }
    }
}
