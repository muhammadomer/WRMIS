using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class ProvinceDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns list of all provinces.
        /// Created On 04-11-2015.
        /// </summary> 
        /// <returns>List<CO_Province></returns>
        public List<CO_Province> GetAllProvinces(bool? _IsActive = null)
        {
            List<CO_Province> lstProvince = db.Repository<CO_Province>().GetAll().Where(p => (p.IsActive == _IsActive || _IsActive == null)).OrderBy(p => p.Name).ToList<CO_Province>();

            return lstProvince;
        }

        /// <summary>
        /// This function adds new province.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_Province"></param>
        /// <returns>bool</returns>
        public bool AddProvince(CO_Province _Province)
        {
            db.Repository<CO_Province>().Insert(_Province);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a province.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_Province"></param>
        /// <returns>bool</returns>
        public bool UpdateProvince(CO_Province _Province)
        {
            CO_Province mdlProvince = db.Repository<CO_Province>().FindById(_Province.ID);

            mdlProvince.Name = _Province.Name;
            mdlProvince.Description = _Province.Description;

            db.Repository<CO_Province>().Update(mdlProvince);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes a province with the provided ID.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_ProvinceID"></param>
        /// <returns>bool</returns>
        public bool DeleteProvince(long _ProvinceID)
        {
            db.Repository<CO_Province>().Delete(_ProvinceID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function gets province by province name.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_ProvinceName"></param>
        /// <returns>CO_Province</returns>
        public CO_Province GetProvinceByName(string _ProvinceName)
        {
            CO_Province mdlProvince = db.Repository<CO_Province>().GetAll().Where(p => p.Name.Replace(" ", "").ToUpper() == _ProvinceName.Replace(" ", "").ToUpper()).FirstOrDefault();

            return mdlProvince;
        }

        /// <summary>
        /// This function checks in District table for given Province ID.
        /// Created On 04-11-2015.
        /// </summary>
        /// <param name="_ProvinceID"></param>
        /// <returns>bool</returns>
        public bool IsProvinceIDExists(long _ProvinceID)
        {
            bool qDistrict = db.Repository<CO_District>().GetAll().Any(d => d.ProvinceID == _ProvinceID);

            return qDistrict;
        }

        /// <summary>
        /// This function searches fro a province by part of name
        /// Created On 18-03-2016
        /// </summary>
        /// <param name="_ProvinceName"></param>
        /// <returns>CO_Province</returns>
        public CO_Province GetProvinceByPartOfName(string _ProvinceName)
        {
            CO_Province mdlProvince = db.Repository<CO_Province>().GetAll().Where(p => p.Name.Replace(" ", "").ToUpper().Contains(_ProvinceName.Replace(" ", "").ToUpper())).FirstOrDefault();

            return mdlProvince;
        }
    }
}
