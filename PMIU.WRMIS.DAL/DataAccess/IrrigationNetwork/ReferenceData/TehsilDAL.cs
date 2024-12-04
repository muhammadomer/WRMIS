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
    public class TehsilDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// This function returns list of all Tehsils.
        /// Created On 16-10-2015
        /// </summary>
        /// <returns>List<CO_Tehsil>()</returns>
        public List<CO_Tehsil> GetTehsilsByDistrictID(long _DistrictID, bool? _IsActive = null)
        {
            List<CO_Tehsil> lstTehsil = db.Repository<CO_Tehsil>().GetAll().OrderBy(n => n.Name).Where(c => c.DistrictID == _DistrictID && (c.IsActive == _IsActive || _IsActive == null)).ToList<CO_Tehsil>();
            return lstTehsil;
        }

        /// <summary>
        /// This Function Adds Tehsil into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>bool</returns>
        public bool AddTehsil(CO_Tehsil _Tehsil)
        {
            db.Repository<CO_Tehsil>().Insert(_Tehsil);
            db.Save();
            return true;
        }

        /// <summary>
        /// This function updates Tehsil into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>bool</returns>
        public bool UpdateTehsil(CO_Tehsil _Tehsil)
        {
            CO_Tehsil mdlTehsil = db.Repository<CO_Tehsil>().FindById(_Tehsil.ID);
            mdlTehsil.Name = _Tehsil.Name;
            mdlTehsil.Description = _Tehsil.Description;

            db.Repository<CO_Tehsil>().Update(mdlTehsil);
            db.Save();
            return true;
        }

        /// <summary>
        /// This function Deletes Tehsil into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>bool</returns>
        public bool DeleteTehsil(long _TehsilID)
        {
            db.Repository<CO_Tehsil>().Delete(_TehsilID);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function checks that Thsil ID Exist in Police Station Table or Not.
        /// Created On:29-10-2015
        /// </summary>
        /// <param name="_TehsilId"></param>
        /// <returns>bool</returns>
        public bool IsTehsilIDExists(long _TehsilID)
        {
            bool qIsExists = db.Repository<CO_PoliceStation>().GetAll().Any(d => d.TehsilID == _TehsilID);
            if (!qIsExists)
            {
                qIsExists = db.Repository<CO_Village>().GetAll().Any(d => d.TehsilID == _TehsilID);
            }
            return qIsExists;
        }

        /// <summary>
        /// this function get Tehsil by Name.
        /// Created On:29-10-2015
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>CO_Tehsil</returns>
        public CO_Tehsil GetTehsilByName(string _Tehsil, long _DistrictID)
        {
            CO_Tehsil mdlTehsil = db.Repository<CO_Tehsil>().GetAll().Where(c => c.Name.Trim().ToLower() == _Tehsil.Trim().ToLower() && c.DistrictID == _DistrictID).FirstOrDefault();
            return mdlTehsil;
        }
    }
}
