using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.DAL.DataAccess.UserAdministration;

namespace PMIU.WRMIS.BLL.UserAdministration
{
    public class DesignationBLL : BaseBLL
    {
        DesignationDAL dalDesignation = new DesignationDAL();

        /// <summary>
        /// this function returns list of all Designations by organization ID
        /// Created On: 16-11-2015
        /// </summary>
        /// <returns>List<UA_Designations></returns>
        public List<dynamic> GetAllDesignationsByOrganizationID(long _OrganizationID)
        {
            return dalDesignation.GetAllDesignationsByOrganizationID(_OrganizationID);
        }


        /// <summary>
        /// this function adds designation in db
        /// </summary>
        /// <param name="_Designation"></param>
        /// <returns>bool</returns>
        public bool AddDesignation(UA_Designations _Designation)
        {
            return dalDesignation.AddDesignation(_Designation);
        }

        /// <summary>
        /// this function updates designation in database
        /// Created On:16-11-2015
        /// </summary>
        /// <param name="_Designation"></param>
        /// <returns>bool</returns>
        public bool UpdateDesignation(UA_Designations _Designation)
        {
            return dalDesignation.UpdateDesignation(_Designation);
        }

        /// <summary>
        /// this function deletes designation from database
        /// Created On: 16-11-2015
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>bool</returns>
        public bool DeleteDesignation(long _DesignationID)
        {
            return dalDesignation.DeleteDesignation(_DesignationID);
        }

        /// <summary>
        /// This function gets the Irrigation Levels by default only for Irrigation;
        /// Created On 24-08-2017
        /// </summary>
        /// <param name="_IsIrrigation"></param>
        /// <returns>List<UA_IrrigationLevel></returns>   
        public List<UA_IrrigationLevel> GetAllIrrigationLevel(bool _IsIrrigation = true)
        {
            return dalDesignation.GetAllIrrigationLevel(_IsIrrigation);
        }

        /// <summary>
        /// this function checks the combination of Designation Name and Type Id, either it is unique or not.
        /// Created On: 17-11-2015
        /// </summary>
        /// <param name="_DesignationName"></param>
        /// <returns>UA_Designations</returns>
        public UA_Designations GetOtherDesignationByName(string _DesignationName, long _OrganizationID)
        {
            return dalDesignation.GetOtherDesignationByName(_DesignationName, _OrganizationID);
        }

        /// <summary>
        /// this function checks that Designation Id exists in other table or not?
        /// Created On: 17-11-2015
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>bool</returns>
        public bool IsDesignationIDExists(long _DesignationID)
        {
            return dalDesignation.IsDesignationIDExists(_DesignationID);
        }

        /// <summary>
        /// This function return designation based on the designation id.
        /// Created On 27-01-2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>UA_Designations</returns>
        public UA_Designations GetByID(long _DesignationID)
        {
            return dalDesignation.GetByID(_DesignationID);
        }
    }
}
