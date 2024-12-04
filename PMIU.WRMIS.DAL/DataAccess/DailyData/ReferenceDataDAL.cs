using PMIU.WRMIS.DAL;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.DailyData
{
    public class ReferenceDataDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// This function returns list of all Reason For Change.
        /// Created On 09-02-2016.
        /// </summary> 
        /// <returns>List<CO_ReasonForChange>()</returns>
        public List<CO_ReasonForChange> GetAllReasonForChange()
        {
            List<CO_ReasonForChange> lstReasonForChange = db.Repository<CO_ReasonForChange>().GetAll().OrderBy(z => z.Name).ToList<CO_ReasonForChange>();

            return lstReasonForChange;
        }

        /// <summary>
        /// This function adds new Reason For Change.
        /// Created On 09-02-2016.
        /// </summary>
        /// <param name="_ReasonForChange"></param>
        /// <returns>bool</returns>
        public bool AddReasonForChange(CO_ReasonForChange _ReasonForChange)
        {
            db.Repository<CO_ReasonForChange>().Insert(_ReasonForChange);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates a Reason For Change.
        /// Created On 09-02-2016.
        /// </summary>
        /// <param name="_ReasonForChange"></param>
        /// <returns>bool</returns>
        public bool UpdateReasonForChange(CO_ReasonForChange _ReasonForChange)
        {
            CO_ReasonForChange mdlReasonForChange = db.Repository<CO_ReasonForChange>().FindById(_ReasonForChange.ID);

            mdlReasonForChange.Name = _ReasonForChange.Name;
            mdlReasonForChange.Description = _ReasonForChange.Description;

            db.Repository<CO_ReasonForChange>().Update(mdlReasonForChange);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes a Reason For Change with the provided ID.
        /// Created On 09-02-2016.
        /// </summary>
        /// <param name="_ReasonForChangeID"></param>
        /// <returns>bool</returns>
        public bool DeleteReasonForChange(long _ReasonForChangeID)
        {
            db.Repository<CO_ReasonForChange>().Delete(_ReasonForChangeID);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function gets zone by zone name.
        /// Created On 20-10-2015.
        /// </summary>
        /// <param name="_ZoneName"></param>
        /// <returns>CO_ReasonForChange</returns>
        public CO_ReasonForChange GetReasonForChangeByName(string _ReasonForChange)
        {
            CO_ReasonForChange mdlReasonForChange = db.Repository<CO_ReasonForChange>().GetAll().Where(z => z.Name.Replace(" ", "").ToLower() == _ReasonForChange.Replace(" ", "").ToLower()).FirstOrDefault();

            return mdlReasonForChange;
        }
    }
}
