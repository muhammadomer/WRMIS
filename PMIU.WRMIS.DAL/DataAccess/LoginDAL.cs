using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;

namespace PMIU.WRMIS.DAL.DataAccess
{
    public class LoginDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// this function validate username and password
        /// Created On: 19-11-2015
        /// </summary>
        /// <param name="_LoginName"></param>
        /// <param name="_Password"></param>
        /// <returns>UA_Users</returns>
        public UA_Users ValidateUser(string _LoginName, string _Password)
        {
            UA_Users mdlUser = db.Repository<UA_Users>().Query().Get().Where(u => u.LoginName == _LoginName && u.Password == _Password && u.IsActive == true).FirstOrDefault();
            return mdlUser;
        }
        public void UserLog(UA_LoginHistory _UserLog)
        {
            db.Repository<UA_LoginHistory>().Insert(_UserLog);
            db.Save();
        }

        public List<object> GetAndroidUserRoleRights(long _UserID) 
        {

            RoleRightsRepository repRoleRights = db.ExtRepositoryFor<RoleRightsRepository>();
            List<object> lstRoleRigths = new List<object>();
            lstRoleRigths = repRoleRights.GetAndroidUserRoleRights(_UserID);
            return lstRoleRigths;
        }

        /// <summary>
        /// Coded by : Hira Iqbal
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns></returns>

        public long? GetAndroidUserDesignationID(long _UserID)
        { 
            return  db.Repository<UA_Users>().Query().Get().Where(u => u.ID == _UserID).FirstOrDefault().DesignationID ; 
         }

        /// <summary>
        /// Coded By : Hira Iqbal
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns></returns>
        public long? GetIrrigationLevelID(long _DesignationID)
        {
            return db.Repository<UA_Designations>().Query().Get().Where(u => u.ID == _DesignationID).FirstOrDefault().IrrigationLevelID;
        }

        /// <summary>
        /// Return true/false about the user and irrigation boundry relation
        /// Coded by: Hira Iqbal
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_BoundryID"></param>
        /// <param name="_IrrigationLevelID"></param>
        /// <returns></returns>
        public bool IsLocationAssigned(long _UserID, long _BoundryID, long _IrrigationLevelID)
        {
            long value = _BoundryID;

            if (_IrrigationLevelID == 5) // section
            {
                value = _BoundryID;
            }
            else if (_IrrigationLevelID == 4) // Sub division
            {
                value =Convert.ToInt64 (db.Repository<CO_Section>().Query().Get().Where(x => x.ID == _BoundryID).SingleOrDefault().SubDivID);

            }
            else if (_IrrigationLevelID == 3) //division
            {
                long suDivID = Convert.ToInt64(db.Repository<CO_Section>().Query().Get().Where(x => x.ID == _BoundryID).SingleOrDefault().SubDivID);
                value = Convert.ToInt64(db.Repository<CO_SubDivision>().Query().Get().Where(x => x.ID == suDivID).SingleOrDefault().DivisionID);

            }
            var mdlLoc = db.Repository<UA_AssociatedLocation>().Query().Get().Where(u => u.UserID == _UserID && u.IrrigationBoundryID == value && u.IrrigationLevelID == _IrrigationLevelID).FirstOrDefault();
            if (mdlLoc == null)
                return false;

            return true;
        }
    }
}
