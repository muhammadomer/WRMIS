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
    public class SubDivisionDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns list of all sub divisions with the given Division ID.
        /// Created On 28-10-2015.
        /// </summary>
        /// <param name="_DivisionId"></param>
        /// <returns>List<CO_SubDivision>()</returns>
        public List<CO_SubDivision> GetSubDivisionsByDivisionID(long _DivisionID, bool? _IsActive = null)
        {
            List<CO_SubDivision> lstSubDivision = db.Repository<CO_SubDivision>().GetAll().Where(s => s.DivisionID == _DivisionID && (s.IsActive == _IsActive || _IsActive == null)).OrderBy(s => s.Name).ToList<CO_SubDivision>();
            return lstSubDivision;
        }

        /// <summary>
        /// This function adds new sub division.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_SubDivision"></param>
        /// <returns>bool</returns>
        public bool AddSubDivision(CO_SubDivision _SubDivision)
        {
            db.Repository<CO_SubDivision>().Insert(_SubDivision);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a sub division.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_SubDivision"></param>
        /// <returns>bool</returns>
        public bool UpdateSubDivision(CO_SubDivision _SubDivision)
        {
            CO_SubDivision mdlSubDivision = db.Repository<CO_SubDivision>().FindById(_SubDivision.ID);

            mdlSubDivision.Name = _SubDivision.Name;
            mdlSubDivision.Description = _SubDivision.Description;

            db.Repository<CO_SubDivision>().Update(mdlSubDivision);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes a sub division with the provided ID.
        /// Created On 16-10-2015.
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>bool</returns>
        public bool DeleteSubDivision(long _SubDivisionID)
        {
            db.Repository<CO_SubDivision>().Delete(_SubDivisionID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function checks in all related tables for given Sub Division ID.
        /// Created On 30-10-2015.
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>bool</returns>
        public bool IsSubDivisionIDExists(long _SubDivisionID)
        {
            bool qIsExists = db.Repository<CO_Section>().GetAll().Any(s => s.SubDivID == _SubDivisionID);

            if (!qIsExists)
            {
                qIsExists = db.Repository<CO_ChannelIndent>().GetAll().Any(c => c.SubDivID == _SubDivisionID);
            }

            return qIsExists;
        }

        /// <summary>
        /// This function gets sub division by sub division name and Division ID..
        /// Created On 30-10-2015.
        /// </summary>
        /// <param name="_SubDivisionName"></param>
        /// <param name="_DivisionID"></param>
        /// <returns>CO_SubDivision</returns>
        public CO_SubDivision GetSubDivisionByName(string _SubDivisionName, long _DivisionID)
        {
            CO_SubDivision mdlSubDivision = db.Repository<CO_SubDivision>().GetAll().Where(s => s.Name.Replace(" ", "").ToUpper() == _SubDivisionName.Replace(" ", "").ToUpper() &&
                s.DivisionID == _DivisionID).FirstOrDefault();

            return mdlSubDivision;
        }

        /// <summary>
        /// this function returns sub division by sub divivison id
        /// Created On: 12/8/2015
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns></returns>
        public CO_SubDivision GetSubDivisionsBySubDivisionID(long _SubDivisionID)
        {
            CO_SubDivision qSubDivision = db.Repository<CO_SubDivision>().GetAll().Where(s => s.ID == _SubDivisionID).FirstOrDefault();

            return qSubDivision;
        }

        /// <summary>
        /// This function return sub division based on the sub division id.
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_SubDivisionID"></param>
        /// <returns>CO_SubDivision</returns>
        public CO_SubDivision GetByID(long _SubDivisionID)
        {
            CO_SubDivision mdlSubDivision = db.Repository<CO_SubDivision>().GetAll().Where(s => s.ID == _SubDivisionID).FirstOrDefault();

            return mdlSubDivision;
        }

        /// <summary>
        /// This function returns data of only those sub divisions whose ID is in the filtered list.
        /// Created On 26-01-2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_FilteredSubDivisionIDs"></param>
        /// <returns>List<CO_SubDivision></returns>
        public List<CO_SubDivision> GetFilteredSubDivisions(long _DivisionID, List<long> _FilteredSubDivisionIDs)
        {
            List<CO_SubDivision> lstSubDivision = db.Repository<CO_SubDivision>().GetAll().Where(sd => sd.DivisionID == _DivisionID && _FilteredSubDivisionIDs.Contains(sd.ID)).ToList<CO_SubDivision>();

            return lstSubDivision;
        }
    }
}
