using PMIU.WRMIS.Model;
using PMIU.WRMIS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.UserAdministration;

namespace PMIU.WRMIS.BLL.UserAdministration
{
    public class RoleRightsBLL : BaseBLL
    {
        public List<UA_Roles> GetAllRoles(bool? _IsActive = null)
        {
            RoleRightsDAL dalRR = new RoleRightsDAL();
            return dalRR.GetAllRoles(_IsActive);
        }

        public List<UA_Modules> getAllModules(bool forSeletedRole = false)
        {
            RoleRightsDAL dalRR = new RoleRightsDAL();
            return dalRR.getAllModules(forSeletedRole);
        }

        public List<object> GetModulePages(int roleID, int moduleID)
        {
            RoleRightsDAL dalRR = new RoleRightsDAL();
            return dalRR.GetModulePages(roleID, moduleID);
        }

        public UA_RoleRights CheckUserEditrights(long UserID, long PageID)
        {
            RoleRightsDAL dalRR = new RoleRightsDAL();
            return dalRR.checkUserEditRights(UserID, PageID);
        }

        public bool updateRoleRight(int RoleRightID, UA_RoleRights obj)
        {
            RoleRightsDAL dalRR = new RoleRightsDAL();
            return dalRR.updateRoleRight(RoleRightID, obj);
        }
    }
}
