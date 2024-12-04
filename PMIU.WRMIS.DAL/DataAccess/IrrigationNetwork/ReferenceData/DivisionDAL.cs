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
    public class DivisionDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns list of all divisions with the given Circle ID.
        /// Created On 28-10-2015.
        /// </summary>
        /// <param name="_CircleId"></param>
        /// <returns>List<CO_Division>()</returns>
        public List<CO_Division> GetDivisionsByCircleIDAndDomainID(long _CircleID = -1, long _DomainID = -1, bool? _IsActive = null)
        {
            List<CO_Division> lstDivision = db.Repository<CO_Division>().GetAll().Where(d => (d.CircleID == _CircleID || _CircleID == -1) &&
                (d.DomainID == _DomainID || _DomainID == -1) && (d.IsActive == _IsActive || _IsActive == null)).OrderBy(d => d.Name).ToList<CO_Division>();

            return lstDivision;
        }
        public long GetDivisionIDBySubDivisionID(long _SubDivisionID)
        {
            long mdlDivisionID = db.Repository<CO_SubDivision>().FindById(_SubDivisionID).DivisionID.Value;
            return mdlDivisionID;
        }

        /// <summary>
        /// This function adds new division.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_Division"></param>
        /// <returns>bool</returns>
        public bool AddDivision(CO_Division _Division)
        {
            db.Repository<CO_Division>().Insert(_Division);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a division.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_Division"></param>
        /// <returns>bool</returns>
        public bool UpdateDivision(CO_Division _Division)
        {
            CO_Division mdlDivision = db.Repository<CO_Division>().FindById(_Division.ID);

            mdlDivision.Name = _Division.Name;
            mdlDivision.Description = _Division.Description;
            mdlDivision.DomainID = _Division.DomainID;

            db.Repository<CO_Division>().Update(mdlDivision);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes a division with the provided ID.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>bool</returns>
        public bool DeleteDivision(long _DivisionID)
        {
            db.Repository<CO_Division>().Delete(_DivisionID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function checks in all related tables for given Division ID.
        /// Created On 28-10-2015.
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>bool</returns>
        public bool IsDivisionIDExists(long _DivisionID)
        {
            bool qIsExists = db.Repository<CO_SubDivision>().GetAll().Any(s => s.DivisionID == _DivisionID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<CO_DistrictDivision>().GetAll().Any(d => d.DivisionID == _DivisionID);

                if (!qIsExists)
                {
                    qIsExists = db.Repository<SI_ScheduleDetailChannel>().GetAll().Any(s => s.DivisionID == _DivisionID);

                    if (!qIsExists)
                    {
                        qIsExists = db.Repository<SI_ScheduleDetailTender>().GetAll().Any(s => s.DivisionID == _DivisionID);

                        if (!qIsExists)
                        {
                            qIsExists = db.Repository<SI_ScheduleDetailWorks>().GetAll().Any(s => s.DivisionID == _DivisionID);
                        }
                    }
                }
            }

            return qIsExists;
        }

        /// <summary>
        /// This function gets division by division name and Circle ID.
        /// Created On 28-10-2015.
        /// </summary>
        /// <param name="_DivisionName"></param>
        /// <param name="_CircleID"></param>
        /// <returns>CO_Division</returns>
        public CO_Division GetDivisionByName(string _DivisionName, long _CircleID)
        {
            CO_Division mdlDivision = db.Repository<CO_Division>().GetAll().Where(d => d.Name.Replace(" ", "").ToUpper() == _DivisionName.Replace(" ", "").ToUpper() &&
                d.CircleID == _CircleID).FirstOrDefault();

            return mdlDivision;
        }

        /// <summary>
        /// This function get all domains
        /// Created On 01-12-2015
        /// </summary>
        /// <returns>List<CO_Domain></returns>
        public List<CO_Domain> GetDomains()
        {
            List<CO_Domain> lstDomain = db.Repository<CO_Domain>().GetAll().ToList<CO_Domain>();

            return lstDomain;
        }

        public List<CO_Division> GetAllDivisionNames()
        {
            return db.Repository<CO_Division>().GetAll().ToList();
        }

        /// <summary>
        /// This function return division based on the division id.
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>CO_Division</returns>
        public CO_Division GetByID(long _DivisionID)
        {
            CO_Division mdlDivision = db.Repository<CO_Division>().GetAll().Where(d => d.ID == _DivisionID).FirstOrDefault();

            return mdlDivision;
        }

        /// <summary>
        /// This function returns data of only those divisions whose ID is in the filtered list.
        /// Created On 26-01-2016
        /// </summary>
        /// <param name="_CircleID"></param>
        /// <param name="_FilteredDivisionIDs"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetFilteredDivisions(long _CircleID, List<long> _FilteredDivisionIDs)
        {
            List<CO_Division> lstDivision = db.Repository<CO_Division>().GetAll().Where(d => d.CircleID == _CircleID && _FilteredDivisionIDs.Contains(d.ID)).ToList<CO_Division>();

            return lstDivision;
        }
    }
}
