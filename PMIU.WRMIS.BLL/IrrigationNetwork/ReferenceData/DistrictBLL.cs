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
    public class DistrictBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all Districts.
        /// Created On 15-10-2015.
        /// </summary> 
        /// <returns>List<CO_District>()</returns>
        public List<CO_District> GetAllDistricts(bool? _IsActive = null)
        {
            DistrictDAL dalDistrict = new DistrictDAL();
            return dalDistrict.GetAllDistricts(_IsActive);
        }

        /// <summary>
        /// This Function Adds District into the Database.
        /// Created on 15-10-2015
        /// </summary>
        /// <param name="_District"></param>
        /// <returns>bool</returns>
        public bool AddDistrict(CO_District _District)
        {
            DistrictDAL dalDistrict = new DistrictDAL();
            return dalDistrict.AddDistrict(_District);
        }

        /// <summary>
        /// This Function Updates District into the Database.
        /// Created on 15-10-2015
        /// </summary>
        /// <param name="_District"></param>
        /// <returns>bool</returns>
        public bool UpdateDistrict(CO_District _District)
        {
            DistrictDAL dalDistrict = new DistrictDAL();
            return dalDistrict.UpdateDistrict(_District);
        }

        /// <summary>
        /// This Function Deletes District from Database.
        /// Created on 15-10-2015
        /// </summary>
        /// <param name="_District"></param>
        /// <returns>bool</returns>
        public bool DeleteDistrict(long _DistrictID)
        {
            DistrictDAL dalDistrict = new DistrictDAL();
            return dalDistrict.DeleteDistrict(_DistrictID);
        }

        /// <summary>
        /// this functions checks in thesil table for given District ID
        /// Created On:28-10-2015
        /// </summary>
        /// <param name="_DistrictId"></param>
        /// <returns>bool</returns>
        public bool IsDistrictIDExists(long _DistrictID)
        {
            DistrictDAL dalDistrict = new DistrictDAL();
            return dalDistrict.IsDistrictIDExists(_DistrictID);
        }

        /// <summary>
        /// this function gets District by District Name
        /// Created On:28-10-2015
        /// </summary>
        /// <param name="_DistrictName"></param>
        /// <returns>CO_District</returns>
        public CO_District GetDistrictByName(string _DistrictName)
        {
            DistrictDAL dalDistrict = new DistrictDAL();
            return dalDistrict.GetDistrictByName(_DistrictName);
        }
    }
}
