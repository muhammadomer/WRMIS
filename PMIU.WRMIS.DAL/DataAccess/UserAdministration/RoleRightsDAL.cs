using PMIU.WRMIS.Model;
using PMIU.WRMIS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.DAL.DataAccess.UserAdministration;
using PMIU.WRMIS.Common;

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{
    public class RoleRightsDAL
    {
        public RoleRightsDAL()
        {

        }

        ContextDB db = new ContextDB();
        public List<UA_Roles> GetAllRoles(bool? _IsActive = null)
        {
            return db.Repository<UA_Roles>().GetAll().Where(r => /* r.ID != Constants.AdministratorRoleID &&*/ (r.IsActive == _IsActive || _IsActive == null)).OrderBy(r => r.Name).ToList();
        }

        public List<UA_Modules> getAllModules(bool forSeletedRole=false)
        {
            List <UA_Modules> lstModules = db.Repository<UA_Modules>().GetAll().Where(q => q.IsActive == true).OrderBy(e => e.Name).ToList();
            if (forSeletedRole==true)
	        {
                UA_Modules Moduel = (from item in db.Repository<UA_Modules>().GetAll() where item.Name == "User Administration" select item).FirstOrDefault();
                  //  lstModules.Remove(Moduel);
	        }
            
            return lstModules;
        }

        public List<object> GetModulePages(int roleID, int moduleID)
        {
            List<object> lstRoleRigts = db.ExtRepositoryFor<RoleRightsRepository>().GetRoleModules(roleID, moduleID);
            return lstRoleRigts;
        }

        public UA_RoleRights checkUserEditRights(long UserID, long PageID)
        {
            return db.ExtRepositoryFor<RoleRightsRepository>().GetUserEditRights(UserID, PageID);
        }

        public bool updateRoleRight(int RoleRightID, UA_RoleRights _ObjRR)
        {
            UA_RoleRights result = db.Repository<UA_RoleRights>().FindById(RoleRightID);
            if (result != null)
            {
                result.BAdd = _ObjRR.BAdd;
                result.BEdit = _ObjRR.BEdit;
                result.BDelete = _ObjRR.BDelete;
                result.BView = _ObjRR.BView;
                db.Repository<UA_RoleRights>().Update(result);
            }
            else
            {
                //  db.ExtRepositoryFor<RoleRightsRepository>().InsertParentEntry(_ObjRR);

                result = new UA_RoleRights();
                result.PageID = _ObjRR.PageID;
                result.RoleID = _ObjRR.RoleID;
                result.BAdd = _ObjRR.BAdd;
                result.BEdit = _ObjRR.BEdit;
                result.BDelete = _ObjRR.BDelete;
                result.BView = _ObjRR.BView;
                result.AddVisible = true;
                result.EditVisible = true;
                result.DeleteVisible = true;
                result.ViewVisible = true;
                db.Repository<UA_RoleRights>().Insert(result);
            }
            db.Save();

            return true;
        }

        public bool InsertParentEntry(UA_RoleRights obj)
        {
            UA_RoleRights result = new UA_RoleRights();
            result.PageID = obj.PageID;
            result.RoleID = obj.RoleID;
            result.BAdd = obj.BAdd;
            result.BEdit = obj.BEdit;
            result.BDelete = obj.BDelete;
            result.BView = obj.BView;
            result.AddVisible = true;
            result.EditVisible = true;
            result.DeleteVisible = true;
            result.ViewVisible = true;
            db.Repository<UA_RoleRights>().Insert(result);
            db.Save();
            return true;
        }
    }
}
