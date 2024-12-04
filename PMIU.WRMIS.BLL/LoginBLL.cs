using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.DataAccess;

namespace PMIU.WRMIS.BLL
{
    public class LoginBLL : BaseBLL
    {
        /// <summary>
        /// this function validate username and password
        /// Created On: 19-11-2015
        /// </summary>
        /// <param name="_LoginName"></param>
        /// <param name="_Password"></param>
        /// <returns>UA_Users</returns>
        public UA_Users ValidateUser(string _LoginName, string _Password)
        {
            LoginDAL dalLogin = new LoginDAL();

            return dalLogin.ValidateUser(_LoginName, _Password);
        }

        public void UserLog(UA_LoginHistory _UserLog) 
        {
            LoginDAL dalLogin = new LoginDAL();
            dalLogin.UserLog(_UserLog);
        }
        public List<object> GetAndroidUserRoleRights(long _UserID)
        {
            LoginDAL dalLogin = new LoginDAL();
            return dalLogin.GetAndroidUserRoleRights(_UserID);
        }

        public long? GetAndroidUserDesignationID(long _UserID) 
        {
            LoginDAL dalLogin = new LoginDAL();
            return dalLogin.GetAndroidUserDesignationID(_UserID);
        }

        public long? GetIrrigationLevelID(long _DesignationID) 
        {
            LoginDAL dalLogin = new LoginDAL();
            return dalLogin.GetIrrigationLevelID(_DesignationID);
        }

        public bool IsLocationAssigned(long _UserID, long _BoundryID, long _IrrigationLevelID)
        {
            return new LoginDAL().IsLocationAssigned(_UserID,_BoundryID, _IrrigationLevelID);
        }



    }
}
