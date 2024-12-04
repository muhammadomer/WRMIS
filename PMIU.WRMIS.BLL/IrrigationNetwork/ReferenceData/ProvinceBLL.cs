using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class ProvinceBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all provinces.
        /// Created On 04-11-2015.
        /// </summary> 
        /// <returns>List<CO_Province></returns>
        public List<CO_Province> GetAllProvinces(bool? _IsActive = null)
        {
            ProvinceDAL dalProvince = new ProvinceDAL();

            return dalProvince.GetAllProvinces(_IsActive);
        }

        /// <summary>
        /// This function adds new province.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_Province"></param>
        /// <returns>bool</returns>
        public bool AddProvince(CO_Province _Province)
        {
            ProvinceDAL dalProvince = new ProvinceDAL();

            return dalProvince.AddProvince(_Province);
        }

        /// <summary>
        /// This function updates a province.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_Province"></param>
        /// <returns>bool</returns>
        public bool UpdateProvince(CO_Province _Province)
        {
            ProvinceDAL dalProvince = new ProvinceDAL();

            return dalProvince.UpdateProvince(_Province);
        }

        /// <summary>
        /// This function deletes a province with the provided ID.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_ProvinceID"></param>
        /// <returns>bool</returns>
        public bool DeleteProvince(long _ProvinceID)
        {
            ProvinceDAL dalProvince = new ProvinceDAL();

            return dalProvince.DeleteProvince(_ProvinceID);
        }

        /// <summary>
        /// This function gets province by province name.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_ProvinceName"></param>
        /// <returns>CO_Province</returns>
        public CO_Province GetProvinceByName(string _ProvinceName)
        {
            ProvinceDAL dalProvince = new ProvinceDAL();

            return dalProvince.GetProvinceByName(_ProvinceName);
        }

        /// <summary>
        /// This function checks in District table for given Province ID.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_ProvinceID"></param>
        /// <returns>bool</returns>
        public bool IsProvinceIDExists(long _ProvinceID)
        {
            ProvinceDAL dalProvince = new ProvinceDAL();

            return dalProvince.IsProvinceIDExists(_ProvinceID);
        }

        /// <summary>
        /// This function searches for a province by part of name
        /// Created On 18-03-2016
        /// </summary>
        /// <param name="_ProvinceName"></param>
        /// <returns>CO_Province</returns>
        public CO_Province GetProvinceByPartOfName(string _ProvinceName)
        {
            ProvinceDAL dalProvince = new ProvinceDAL();

            return dalProvince.GetProvinceByPartOfName(_ProvinceName);
        }
    }
}
