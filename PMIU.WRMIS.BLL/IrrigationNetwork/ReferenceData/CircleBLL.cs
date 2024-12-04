using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData
{
    public class CircleBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all circles with the given Zone ID.
        /// Created On 22-10-2015.
        /// </summary>
        /// <param name="_ZoneId"></param>
        /// <returns>List<CO_Circle>()</returns>
        public List<CO_Circle> GetCirclesByZoneID(long _ZoneID, bool? _IsActive = null)
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.GetCirclesByZoneID(_ZoneID, _IsActive);
        }

        public long GetCircleIDByDivisionID(long _DivisionID)
        {
            CircleDAL dalCircle = new CircleDAL();
            return dalCircle.GetCircleIDByDivisionID(_DivisionID);
        }

        /// <summary>
        /// This function adds new circle.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_Circle"></param>
        /// <returns>bool</returns>
        public bool AddCircle(CO_Circle _Circle)
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.AddCircle(_Circle);
        }

        /// <summary>
        /// This function updates a circle.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_Circle"></param>
        /// <returns>bool</returns>
        public bool UpdateCircle(CO_Circle _Circle)
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.UpdateCircle(_Circle);
        }

        /// <summary>
        /// This function deletes a circle with the provided ID.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <returns>bool</returns>
        public bool DeleteCircle(long _CircleID)
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.DeleteCircle(_CircleID);
        }

        /// <summary>
        /// This function checks in Division table for given Circle ID.
        /// Created On 22-10-2015.
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <returns>bool</returns>
        public bool IsCircleIDExists(long _CircleID)
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.IsCircleIDExists(_CircleID);
        }

        /// <summary>
        /// This function gets circle by circle name and Zone ID.
        /// Created On 22-10-2015.
        /// </summary>
        /// <param name="_CircleName"></param>
        /// <param name="_ZoneID"></param>
        /// <returns>CO_Circle</returns>
        public CO_Circle GetCircleByName(string _CircleName, long _ZoneID)
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.GetCircleByName(_CircleName, _ZoneID);
        }

        /// <summary>
        /// This function return circle based on the circle id.
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>CO_Circle</returns>
        public CO_Circle GetByID(long _CircleID, bool? _IsActive = null)
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.GetByID(_CircleID, _IsActive);
        }

        /// <summary>
        /// This function returns data of only those circles whose ID is in the filtered list.
        /// Created On 26-01-2016
        /// </summary>
        /// <param name="_ZoneID"></param>
        /// <param name="_FilteredCircleIDs"></param>
        /// <returns>List<CO_Circle></returns>
        public List<CO_Circle> GetFilteredCircles(long _ZoneID, List<long> _FilteredCircleIDs)
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.GetFilteredCircles(_ZoneID, _FilteredCircleIDs);
        }

        public List<CO_Circle> GetAllCircles()
        {
            CircleDAL dalCircle = new CircleDAL();

            return dalCircle.GetAllCircles();
        }
    }
}
