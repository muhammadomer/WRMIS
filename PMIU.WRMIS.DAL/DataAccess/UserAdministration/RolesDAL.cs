using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{
    public class RolesDAL
    {
        public RolesDAL()
        {


        }

        ContextDB db = new ContextDB();

        public List<UA_Roles> GetAllRoles()
        {
            List<UA_Roles> lstRoles = db.Repository<UA_Roles>().GetAll().OrderBy(x=>x.Name).ToList();
            return lstRoles;
        }

        public bool UpdateRole(UA_Roles _RoleObj)
        {

            UA_Roles Role = db.Repository<UA_Roles>().FindById(_RoleObj.ID);
            Role.Name = _RoleObj.Name;
            Role.Description = _RoleObj.Description;
            db.Repository<UA_Roles>().Update(Role);
            db.Save();
            return true;
        }

        public bool AddRole(UA_Roles _RolesObj)
        {

            long ID = db.Repository<UA_Roles>().GetAll().Max(u => (long)u.ID) + 1;
            _RolesObj.ID = ID;
            db.Repository<UA_Roles>().Insert(_RolesObj);
            db.Save();
            return true;
        }

        public bool IsRoleExist(UA_Roles _RoleObj)
        {
            bool result = db.Repository<UA_Roles>().GetAll().Any(q => q.Name == _RoleObj.Name);
            return result;
        }

        public bool IsRoleExistEdit(UA_Roles _RoleObj)
        {
            bool exist = false;
            UA_Roles lstRoles = db.Repository<UA_Roles>().GetAll().Where(q => q.ID != _RoleObj.ID && q.Name == _RoleObj.Name).FirstOrDefault();
            if (lstRoles != null)
                exist = true;
            return exist;
        }

        public bool DeleteRole(int _RoleId)
        {
            bool result = false;
            try
            {
                if (_RoleId > 0)
                    db.Repository<UA_Roles>().Delete(_RoleId);
                db.Save();
                result = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return result;
        }

        public string GetUserRoleByID(long _RoleID)
        {
            UA_Roles Role = db.Repository<UA_Roles>().FindById(_RoleID);
            return Role.Name.ToString();
        }
    }
}
