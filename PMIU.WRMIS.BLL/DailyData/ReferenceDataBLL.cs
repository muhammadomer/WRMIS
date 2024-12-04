using PMIU.WRMIS.DAL.DataAccess.DailyData;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.DailyData
{
    public class ReferenceDataBLL : BaseBLL
    {
        #region Reason For Change
        /// <summary>
        /// This function returns list of all Reason For Change.
        /// Created On 09-02-2016.
        /// </summary> 
        /// <returns>List<CO_ReasonForChange>()</returns>
        public List<CO_ReasonForChange> GetAllReasonForChange()
        {
            ReferenceDataDAL dalReferenceData = new ReferenceDataDAL();
            return dalReferenceData.GetAllReasonForChange();
        }

        /// <summary>
        /// This function adds new Reason For Change.
        /// Created On 09-02-2016.
        /// </summary>
        /// <param name="_ReasonForChange"></param>
        /// <returns>bool</returns>
        public bool AddReasonForChange(CO_ReasonForChange _ReasonForChange)
        {
            ReferenceDataDAL dalReferenceData = new ReferenceDataDAL();
            return dalReferenceData.AddReasonForChange(_ReasonForChange);
        }

        /// <summary>
        /// This function updates a Reason For Change.
        /// Created On 09-02-2016.
        /// </summary>
        /// <param name="_ReasonForChange"></param>
        /// <returns>bool</returns>
        public bool UpdateReasonForChange(CO_ReasonForChange _ReasonForChange)
        {
            ReferenceDataDAL dalReferenceData = new ReferenceDataDAL();
            return dalReferenceData.UpdateReasonForChange(_ReasonForChange);
        }

        /// <summary>
        /// This function deletes a Reason For Change with the provided ID.
        /// Created On 09-02-2016.
        /// </summary>
        /// <param name="_ReasonForChangeID"></param>
        /// <returns>bool</returns>
        public bool DeleteReasonForChange(long _ReasonForChangeID)
        {
            ReferenceDataDAL dalReferenceData = new ReferenceDataDAL();
            return dalReferenceData.DeleteReasonForChange(_ReasonForChangeID);
        }

        /// <summary>
        /// This function gets Reason by Reason name.
        /// Created On 20-10-2015.
        /// </summary>
        /// <param name="_ReasonForChange"></param>
        /// <returns>CO_ReasonForChange</returns>
        public CO_ReasonForChange GetReasonForChangeByName(string _ReasonForChange)
        {
            ReferenceDataDAL dalReferenceData = new ReferenceDataDAL();
            return dalReferenceData.GetReasonForChangeByName(_ReasonForChange);
        }
        #endregion
    }
}
