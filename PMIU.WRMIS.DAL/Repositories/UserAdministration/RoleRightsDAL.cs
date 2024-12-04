using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PMIU.WRMIS.DAL.Repositories.UserAdministration
{
    public class RoleRightsDAL : Repository<UA_RoleRights>
    {
        WRMIS_Entities _context;
        public RoleRightsDAL(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<UA_RoleRights>();
            _context = context;
        }


        public List<object> GetRoleModules(int roleID, int moduleID)
        {
            try
            {
                var lstRoleRights = (from modules in context.UA_Modules
                                     join pages in context.UA_Pages on modules.ID equals pages.ModuleID
                                     join rolerights in context.UA_RoleRights on pages.ID equals rolerights.PageID
                                     where modules.ID == moduleID && rolerights.RoleID == roleID
                                     select
                                         new
                                         {
                                             rolerights.ID,
                                             rolerights.RoleID,
                                             rolerights.PageID,
                                             rolerights.BAdd,
                                             rolerights.BEdit,
                                             rolerights.BDelete,
                                             rolerights.BPrint,
                                             rolerights.BView,
                                             rolerights.BExport,
                                             rolerights.AddVisible,
                                             rolerights.EditVisible,
                                             rolerights.DeleteVisible,
                                             rolerights.PrintVisible,
                                             rolerights.ViewVisible,
                                             rolerights.ExportVisible,
                                             pages.Name,
                                             pages.Description,
                                             pages.ParentID
                                             //moduleID= modules.ID,
                                             //moduleName = modules.Name
                                         }

                               ).ToList<object>();               

                return lstRoleRights;

            }
            catch (WRException exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.ExceptionCategory.DataAccess);
                return new List<object>();
            }
        }

    }
}
