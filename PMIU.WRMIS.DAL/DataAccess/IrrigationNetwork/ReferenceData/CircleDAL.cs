using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.ReferenceData
{
    public class CircleDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns list of all circles with the given Zone ID.
        /// Created On 22-10-2015.
        /// </summary>
        /// <param name="_ZoneId"></param>
        /// <returns>List<CO_Circle>()</returns>
        public List<CO_Circle> GetCirclesByZoneID(long _ZoneID, bool? _IsActive = null)
        {
            List<CO_Circle> lstCircle = db.Repository<CO_Circle>().GetAll().Where(c => c.ZoneID == _ZoneID && (c.IsActive == _IsActive || _IsActive == null)).OrderBy(c => c.Name).ToList<CO_Circle>();

            return lstCircle;
        }

        public long GetCircleIDByDivisionID(long _DivisionID)
        {
            long CircleID = db.Repository<CO_Division>().FindById(_DivisionID).CircleID.Value;
            return CircleID;
        }

        /// <summary>
        /// This function adds new circle.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_Circle"></param>
        /// <returns>bool</returns>
        public bool AddCircle(CO_Circle _Circle)
        {
            db.Repository<CO_Circle>().Insert(_Circle);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a circle.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_Circle"></param>
        /// <returns>bool</returns>
        public bool UpdateCircle(CO_Circle _Circle)
        {
            CO_Circle mdlCircle = db.Repository<CO_Circle>().FindById(_Circle.ID);

            mdlCircle.Name = _Circle.Name;
            mdlCircle.Description = _Circle.Description;

            db.Repository<CO_Circle>().Update(mdlCircle);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes a circle with the provided ID.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <returns>bool</returns>
        public bool DeleteCircle(long _CircleID)
        {
            db.Repository<CO_Circle>().Delete(_CircleID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function checks in Division table for given Circle ID.
        /// Created On 22-10-2015.
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <returns>bool</returns>
        public bool IsCircleIDExists(long _CircleID)
        {
            bool qDivision = db.Repository<CO_Division>().GetAll().Any(d => d.CircleID == _CircleID);

            return qDivision;
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
            CO_Circle mdlCircle = db.Repository<CO_Circle>().GetAll().Where(c => c.Name.Replace(" ", "").ToUpper() == _CircleName.Replace(" ", "").ToUpper() &&
                c.ZoneID == _ZoneID).FirstOrDefault();

            return mdlCircle;
        }

        /// <summary>
        /// This function return circle based on the circle id.
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>CO_Circle</returns>
        public CO_Circle GetByID(long _CircleID, bool? _IsActive = null)
        {
            CO_Circle mdlCircle = db.Repository<CO_Circle>().GetAll().Where(c => c.ID == _CircleID && (c.IsActive == _IsActive || _IsActive == null)).FirstOrDefault();

            return mdlCircle;
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
            List<CO_Circle> lstCircle = db.Repository<CO_Circle>().GetAll().Where(c => c.ZoneID == _ZoneID && _FilteredCircleIDs.Contains(c.ID)).ToList<CO_Circle>();

            return lstCircle;
        }

        public List<CO_Circle> GetAllCircles()
        {
            List<CO_Circle> lstCircle = db.Repository<CO_Circle>().GetAll().OrderBy(c => c.Name).ToList<CO_Circle>();

            return lstCircle;
        }
    }
}
