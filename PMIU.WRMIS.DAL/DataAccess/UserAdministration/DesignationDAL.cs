using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.DAL;

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{

    public class DesignationDAL
    {
        ContextDB db = new ContextDB();
        /// <summary>
        /// this function returns list of all Designations by organization ID
        /// Created On: 16-11-2015
        /// </summary>
        /// <returns>List<UA_Designations></returns>
        public List<dynamic> GetAllDesignationsByOrganizationID(long _OrganizationID)
        {
            List<dynamic> lstDesignation = db.ExtRepositoryFor<DesignationRepository>().GetDesignationsByOrganizatonID(_OrganizationID);
            return lstDesignation;
        }


        /// <summary>
        /// this function adds designation in db
        /// </summary>
        /// <param name="_Designation"></param>
        /// <returns>bool</returns>
        public bool AddDesignation(UA_Designations _Designation)
        {
            long ID = db.Repository<UA_Designations>().GetAll().Max(u => (long)u.ID) + 1;
            _Designation.ID = ID;
            db.Repository<UA_Designations>().Insert(_Designation);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function updates designation in database
        /// Created On:16-11-2015
        /// </summary>
        /// <param name="_Designation"></param>
        /// <returns>bool</returns>
        public bool UpdateDesignation(UA_Designations _Designation)
        {
            UA_Designations mdlDesignation = db.Repository<UA_Designations>().FindById(_Designation.ID);
            mdlDesignation.Name = _Designation.Name;
            mdlDesignation.OrganizationID = _Designation.OrganizationID;
            mdlDesignation.ReportingToDesignationID = _Designation.ReportingToDesignationID;
            mdlDesignation.IrrigationLevelID = _Designation.IrrigationLevelID;
            mdlDesignation.AuthorityRights = _Designation.AuthorityRights;
            mdlDesignation.TempAssignment = _Designation.TempAssignment;

            db.Repository<UA_Designations>().Update(mdlDesignation);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function deletes designation from database
        /// Created On: 16-11-2015
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>bool</returns>
        public bool DeleteDesignation(long _DesignationID)
        {
            db.Repository<UA_Designations>().Delete(_DesignationID);
            db.Save();
            return true;
        }

        /// <summary>
        /// This function gets the Irrigation Levels by default only for Irrigation;
        /// Created On 24-08-2017
        /// </summary>
        /// <param name="_IsIrrigation"></param>
        /// <returns>List<UA_IrrigationLevel></returns>
        public List<UA_IrrigationLevel> GetAllIrrigationLevel(bool _IsIrrigation = true)
        {
            return db.Repository<UA_IrrigationLevel>().GetAll().Where(il => (_IsIrrigation == false || il.ID != (long)Constants.IrrigationLevelID.Office)).ToList<UA_IrrigationLevel>();
        }

        /// <summary>
        /// this function checks the combination of Designation Name and Type Id, either it is unique or not.
        /// Created On: 17-11-2015
        /// </summary>
        /// <param name="_DesignationName"></param>
        /// <returns>UA_Designations</returns>
        public UA_Designations GetOtherDesignationByName(string _DesignationName, long _OrganizationID)
        {
            UA_Designations mdlDesignation = db.Repository<UA_Designations>().GetAll().Where(z => z.Name.Trim().ToLower() == _DesignationName.Trim().ToLower() && z.UA_Organization.ID == _OrganizationID).FirstOrDefault();
            return mdlDesignation;
        }

        /// <summary>
        /// this function checks that Designation Id exists in other table or not?
        /// Created On: 17-11-2015
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>bool</returns>
        public bool IsDesignationIDExists(long _DesignationID)
        {
            bool qUsers = db.Repository<UA_Users>().GetAll().Any(c => c.DesignationID == _DesignationID);
            return qUsers;
        }

        /// <summary>
        /// This function return designation based on the designation id.
        /// Created On 27-01-2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>UA_Designations</returns>
        public UA_Designations GetByID(long _DesignationID)
        {
            UA_Designations mdlDesignation = db.Repository<UA_Designations>().GetAll().Where(d => d.ID == _DesignationID).FirstOrDefault();

            return mdlDesignation;
        }
    }
}
