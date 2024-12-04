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
    public class OutletTypeBLL : BaseBLL
    {
        /// <summary>
        /// this function returns list of all Outlet Types
        /// Created On: 19-10-2105
        /// </summary>
        /// <returns>List<CO_OutletType></returns>
        public List<CO_OutletType> GetAllOutletTypes(bool? _IsActive = null)
        {
            OutletTypeDAL dalOutletType = new OutletTypeDAL();
            return dalOutletType.GetAllOutletTypes(_IsActive);
        }

        /// <summary>
        /// this function adds Outlet Type into the Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_OutletType"></param>
        /// <returns>bool</returns>
        public bool AddOutletType(CO_OutletType _OutletType)
        {
            OutletTypeDAL dalOutletType = new OutletTypeDAL();
            return dalOutletType.AddOutletType(_OutletType);
        }

        /// <summary>
        /// this function updates Outlet Type
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_OutletType"></param>
        /// <returns>bool</returns>
        public bool UpdateOutletType(CO_OutletType _OutletType)
        {
            OutletTypeDAL dalOutletType = new OutletTypeDAL();
            return dalOutletType.UpdateOutletType(_OutletType);
        }

        /// <summary>
        /// this function deletes Outlet Type From Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_OutletType"></param>
        /// <returns>bool</returns>
        public bool DeleteOutletType(long _OutletType)
        {
            OutletTypeDAL dalOutletType = new OutletTypeDAL();
            return dalOutletType.DeleteOutletType(_OutletType);
        }

        /// <summary>
        /// this function checks that outlet type id exists in outlet alteration history or not.
        /// Created On:27-10-2015
        /// </summary>
        /// <param name="_OutletTypeId"></param>
        /// <returns>bool</returns>
        public bool IsOutletTypeIDExists(long _OutletTypeID)
        {
            OutletTypeDAL dalOutletType = new OutletTypeDAL();
            return dalOutletType.IsOutletTypeIDExists(_OutletTypeID);
        }

        /// <summary>
        /// this function return outlet type by name
        /// Created On:27-10-2015
        /// </summary>
        /// <param name="_OutletTypeName"></param>
        /// <returns>CO_OutletType</returns>
        public CO_OutletType GetOutletTypeByName(string _OutletTypeName)
        {
            OutletTypeDAL dalOutletType = new OutletTypeDAL();
            return dalOutletType.GetOutletTypeByName(_OutletTypeName);
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
            OutletTypeDAL dalOutletType = new OutletTypeDAL();
            return dalOutletType.GetOutletTypeByDescription(_OutletTypeDescription);
        }
    }
}
