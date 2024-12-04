using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class OutletTypeDAL 
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// this function returns list of all Outlet Types
        /// Created On: 19-10-2105
        /// </summary>
        /// <returns>List<CO_OutletType></returns>
        public List<CO_OutletType> GetAllOutletTypes(bool? _IsActive = null)
        {
            List<CO_OutletType> lstOutletType = db.Repository<CO_OutletType>().GetAll().Where(c => (c.IsActive == _IsActive || _IsActive == null)).OrderBy(n => n.Name).ToList<CO_OutletType>();
            return lstOutletType;
        }

        /// <summary>
        /// this function adds Outlet Type into the Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_OutletType"></param>
        /// <returns>bool</returns>
        public bool AddOutletType(CO_OutletType _OutletType)
        {
            db.Repository<CO_OutletType>().Insert(_OutletType);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function updates Outlet Type
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_OutletType"></param>
        /// <returns>bool</returns>
        public bool UpdateOutletType(CO_OutletType _OutletType)
        {
            CO_OutletType mdlOutletType = db.Repository<CO_OutletType>().FindById(_OutletType.ID);
            mdlOutletType.Name = _OutletType.Name;
            mdlOutletType.Description = _OutletType.Description;

            db.Repository<CO_OutletType>().Update(mdlOutletType);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function deletes Outlet Type From Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_OutletType"></param>
        /// <returns>bool</returns>
        public bool DeleteOutletType(long _OutletType)
        {
            db.Repository<CO_OutletType>().Delete(_OutletType);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function checks that outlet type id exists in outlet alteration history or not.
        /// Created On:27-10-2015
        /// </summary>
        /// <param name="_OutletTypeId"></param>
        /// <returns>bool</returns>
        public bool IsOutletTypeIDExists(long _OutletTypeID)
        {
            bool qOutlet = db.Repository<CO_OutletAlterationHistroy>().GetAll().Any(c => c.OutletTypeID == _OutletTypeID);
            return qOutlet;
        }

        /// <summary>
        /// this function return outlet type by name
        /// Created On:27-10-2015
        /// </summary>
        /// <param name="_OutletTypeName"></param>
        /// <returns>CO_OutletType</returns>
        public CO_OutletType GetOutletTypeByName(string _OutletTypeName)
        {
            CO_OutletType mdlOutletType = db.Repository<CO_OutletType>().GetAll().Where(z => z.Name.Trim().ToLower() == _OutletTypeName.Trim().ToLower()).FirstOrDefault();
            return mdlOutletType;
        }

        /// <summary>
        /// this function return outlet type by description
        /// Created On:27-10-2015
        /// </summary>
        /// <param name="_OutletTypeName"></param>
        /// <param name="_OutletTypeDescription"></param>
        /// <returns>CO_OutletType</returns>
        public CO_OutletType GetOutletTypeByDescription(string _OutletTypeDescription)
        {
            CO_OutletType mdlOutletType = db.Repository<CO_OutletType>().GetAll().Where(z => z.Description.Trim().ToLower() == _OutletTypeDescription.Trim().ToLower()).FirstOrDefault();
            return mdlOutletType;
        }
    }
}
