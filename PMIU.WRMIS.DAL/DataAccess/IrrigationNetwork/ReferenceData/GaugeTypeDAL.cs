using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class GaugeTypeDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// this function returns list of all GaugeTypes
        /// Created On: 19-10-2015
        /// </summary>
        /// <returns>List<CO_GaugeType></returns>
        public List<CO_GaugeType> GetAllGaugeTypes(bool? _IsActive = null)
        {
            List<CO_GaugeType> lstGaugeType = db.Repository<CO_GaugeType>().GetAll().Where(c => (c.IsActive == _IsActive || _IsActive == null)).OrderBy(n => n.Name).ToList<CO_GaugeType>();
            return lstGaugeType;
        }

        /// <summary>
        /// this function adds GaugeType into the Database
        /// Created On: 19-10-2015
        /// </summary>
        /// <param name="_GauageType"></param>
        /// <returns>bool</returns>
        public bool AddGaugeType(CO_GaugeType _GauageType)
        {
            db.Repository<CO_GaugeType>().Insert(_GauageType);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function updates GaugeType
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_GaugeType"></param>
        /// <returns>bool</returns>
        public bool UpdateGaugeType(CO_GaugeType _GaugeType)
        {
            CO_GaugeType mdlGaugeType = db.Repository<CO_GaugeType>().FindById(_GaugeType.ID);
            mdlGaugeType.Name = _GaugeType.Name;
            mdlGaugeType.Description = _GaugeType.Description;

            db.Repository<CO_GaugeType>().Update(mdlGaugeType);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function deletes GaugeType from Database
        /// Created On:19-10-2015
        /// </summary>
        /// <param name="_GaugeType"></param>
        /// <returns>bool</returns>
        public bool DeleteGaugeType(long _GaugeType)
        {
            db.Repository<CO_GaugeType>().Delete(_GaugeType);
            db.Save();
            return true;
        }


        /// <summary>
        /// this function returns Gauge Type by Name
        /// Created On:20-10-2015
        /// </summary>
        /// <param name="gaugeName"></param>
        /// <returns> CO_GaugeType</returns>
        public CO_GaugeType GetGaugeTypeByName(string _GaugeTypeName)
        {
            CO_GaugeType mdlGaugeType = db.Repository<CO_GaugeType>().GetAll().Where(z => z.Name.Trim().ToLower() == _GaugeTypeName.Trim().ToLower()).FirstOrDefault();
            return mdlGaugeType;
        }

        /// <summary>
        /// this function checks that gauge type id exist in channel gauge
        /// Created On:22-10-2015
        /// </summary>
        /// <param name="_GaugeTypeId"></param>
        /// <returns>bool</returns>
        public bool IsGaugeTypeIDExists(long _GaugeTypeID)
        {
            bool qChannelGauge = db.Repository<CO_ChannelGauge>().GetAll().Any(c => c.GaugeTypeID == _GaugeTypeID);
            return qChannelGauge;
        }
    }
}
