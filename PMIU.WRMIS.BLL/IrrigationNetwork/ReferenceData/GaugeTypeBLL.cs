using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class GaugeTypeBLL : BaseBLL
    {
        /// <summary>
        /// this function returns list of all GaugeTypes
        /// Created On: 19-10-2015
        /// </summary>
        /// <returns>List<CO_GaugeType></returns>
        public List<CO_GaugeType> GetAllGaugeTypes(bool? _IsActive = null)
        {
            GaugeTypeDAL dalGaugeType = new GaugeTypeDAL();
            return dalGaugeType.GetAllGaugeTypes(_IsActive);
        }

        /// <summary>
        /// this function adds GaugeType into the Database
        /// Created On: 19-10-2015
        /// </summary>
        /// <param name="_GauageType"></param>
        /// <returns>bool</returns>
        public bool AddGaugeType(CO_GaugeType _GauageType)
        {
            GaugeTypeDAL dalGaugeType = new GaugeTypeDAL();
            return dalGaugeType.AddGaugeType(_GauageType);
        }

        /// <summary>
        /// this function updates GaugeType
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_GaugeType"></param>
        /// <returns>bool</returns>
        public bool UpdateGaugeType(CO_GaugeType _GaugeType)
        {
            GaugeTypeDAL dalGaugeType = new GaugeTypeDAL();
            return dalGaugeType.UpdateGaugeType(_GaugeType);
        }

        /// <summary>
        /// this function deletes GaugeType from Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_GaugeType"></param>
        /// <returns>bool</returns>
        public bool DeleteGaugeType(long _GaugeType)
        {
            GaugeTypeDAL dalGaugeType = new GaugeTypeDAL();
            return dalGaugeType.DeleteGaugeType(_GaugeType);
        }


        /// <summary>
        /// this function returns Gauge Type by Name
        /// Created On:20-10-2015
        /// </summary>
        /// <param name="gaugeName"></param>
        /// <returns> CO_GaugeType</returns>
        public CO_GaugeType GetGaugeTypeByName(string _GaugeTypeName)
        {
            GaugeTypeDAL dalGaugeType = new GaugeTypeDAL();
            return dalGaugeType.GetGaugeTypeByName(_GaugeTypeName);
        }

        /// <summary>
        /// this function checks that gauge type id exist in channel gauge
        /// Created On:22-10-2015
        /// </summary>
        /// <param name="_GaugeTypeId"></param>
        /// <returns>bool</returns>
        public bool IsGaugeTypeIDExists(long _GaugeTypeID)
        {
            GaugeTypeDAL dalGaugeType = new GaugeTypeDAL();
            return dalGaugeType.IsGaugeTypeIDExists(_GaugeTypeID);
        }
    }
}
