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
    public class DistrictDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// This function returns list of all Districts.
        /// Created On 15-10-2015.
        /// </summary> 
        /// <returns>List<CO_District>()</returns>
        public List<CO_District> GetAllDistricts(bool? _IsActive = null)
        {
            List<CO_District> lstDistrict = db.Repository<CO_District>().GetAll().Where(c => (c.IsActive == _IsActive || _IsActive == null)).OrderBy(n => n.Name).ToList<CO_District>();
            return lstDistrict;
        }

        /// <summary>
        /// This Function Adds District into the Database.
        /// Created on 15-10-2015
        /// </summary>
        /// <param name="_District"></param>
        /// <returns>bool</returns>
        public bool AddDistrict(CO_District _District)
        {
            db.Repository<CO_District>().Insert(_District);
            db.Save();
            return true;
        }

        /// <summary>
        /// This Function Updates District into the Database.
        /// Created on 15-10-2015
        /// </summary>
        /// <param name="_District"></param>
        /// <returns>bool</returns>
        public bool UpdateDistrict(CO_District _District)
        {
            CO_District mdlDistrict = db.Repository<CO_District>().FindById(_District.ID);
            mdlDistrict.Name = _District.Name;
            mdlDistrict.Description = _District.Description;

            db.Repository<CO_District>().Update(mdlDistrict);
            db.Save();
            return true;
        }

        /// <summary>
        /// This Function Deletes District from Database.
        /// Created on 15-10-2015
        /// </summary>
        /// <param name="_District"></param>
        /// <returns>bool</returns>
        public bool DeleteDistrict(long _DistrictID)
        {
            db.Repository<CO_District>().Delete(_DistrictID);
            db.Save();
            return true;
        }

        /// <summary>
        /// this functions checks in thesil table for given District ID
        /// Created On:28-10-2015
        /// </summary>
        /// <param name="_DistrictId"></param>
        /// <returns>bool</returns>
        public bool IsDistrictIDExists(long _DistrictID)
        {
            bool qIsExists = db.Repository<CO_Tehsil>().GetAll().Any(c => c.DistrictID == _DistrictID);
            if (!qIsExists)
            {
                qIsExists = db.Repository<CO_DistrictDivision>().GetAll().Any(c => c.DistrictID == _DistrictID);
            }
            return qIsExists;
        }

        /// <summary>
        /// this function gets District by District Name
        /// Created On:28-10-2015
        /// </summary>
        /// <param name="_DistrictName"></param>
        /// <returns>CO_District</returns>
        public CO_District GetDistrictByName(string _DistrictName)
        {
            CO_District mdlDistrict = db.Repository<CO_District>().GetAll().Where(z => z.Name.Trim().ToLower() == _DistrictName.Trim().ToLower()).FirstOrDefault();
            return mdlDistrict;
        }
    }
}
