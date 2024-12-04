using PMIU.WRMIS.DAL.DataAccess;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.DataAccess.UserAdministration;

namespace PMIU.WRMIS.BLL.UserAdministration
{
    public class RolesBLL : BaseBLL
    {
        public List<UA_Roles> GetAllRoles()
        {
            RolesDAL dalRoles = new RolesDAL();
            return dalRoles.GetAllRoles();
        }

        public bool UpdateRole(UA_Roles _RoleObj)
        {
            RolesDAL dalRoles = new RolesDAL();
            return dalRoles.UpdateRole(_RoleObj);
        }

        public bool AddRole(UA_Roles _RolesObj)
        {
            RolesDAL dalRoles = new RolesDAL();
            return dalRoles.AddRole(_RolesObj);
        }

        public bool IsRoleExist(UA_Roles _roleObj)
        {
            RolesDAL dalRoles = new RolesDAL();
            return dalRoles.IsRoleExist(_roleObj);
        }

        public bool IsRoleExistEdit(UA_Roles _RoleObj)
        {
            RolesDAL dalRoles = new RolesDAL();
            return dalRoles.IsRoleExistEdit(_RoleObj);
        }

        public bool DeleteRole(int _RoleId)
        {
            RolesDAL dalRoles = new RolesDAL();
            return dalRoles.DeleteRole(_RoleId);
        }
        public string GetUserRoleByID(long _RoleID)
        {
            RolesDAL dalRoles = new RolesDAL();
            return dalRoles.GetUserRoleByID(_RoleID);
        }


    }
}
