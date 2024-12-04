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
    public class TehsilBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all Tehsils.
        /// Created On 16-10-2015
        /// </summary>
        /// <returns>List<CO_Tehsil>()</returns>
        public List<CO_Tehsil> GetTehsilsByDistrictID(long _DistrictID, bool? _IsActive = null)
        {
            TehsilDAL dalTehsil = new TehsilDAL();
            return dalTehsil.GetTehsilsByDistrictID(_DistrictID, _IsActive);
        }

        /// <summary>
        /// This Function Adds Tehsil into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>bool</returns>
        public bool AddTehsil(CO_Tehsil _Tehsil)
        {
            TehsilDAL dalTehsil = new TehsilDAL();
            return dalTehsil.AddTehsil(_Tehsil);
        }

        /// <summary>
        /// This function updates Tehsil into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>bool</returns>
        public bool UpdateTehsil(CO_Tehsil _Tehsil)
        {
            TehsilDAL dalTehsil = new TehsilDAL();
            return dalTehsil.UpdateTehsil(_Tehsil);
        }

        /// <summary>
        /// This function Deletes Tehsil into the Database
        /// Created On 16-10-2015
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>bool</returns>
        public bool DeleteTehsil(long _TehsilID)
        {
            TehsilDAL dalTehsil = new TehsilDAL();
            return dalTehsil.DeleteTehsil(_TehsilID);
        }

        /// <summary>
        /// this function checks that Thsil ID Exist in Police Station Table or Not.
        /// Created On:29-10-2015
        /// </summary>
        /// <param name="_TehsilId"></param>
        /// <returns>bool</returns>
        public bool IsTehsilIDExists(long _TehsilID)
        {
            TehsilDAL dalTehsil = new TehsilDAL();
            return dalTehsil.IsTehsilIDExists(_TehsilID);
        }

        /// <summary>
        /// this function get Tehsil by Name.
        /// Created On:29-10-2015
        /// </summary>
        /// <param name="_Tehsil"></param>
        /// <returns>CO_Tehsil</returns>
        public CO_Tehsil GetTehsilByName(string _Tehsil, long _DistrictID)
        {
            TehsilDAL dalTehsil = new TehsilDAL();
            return dalTehsil.GetTehsilByName(_Tehsil, _DistrictID);
        }
    }
}
