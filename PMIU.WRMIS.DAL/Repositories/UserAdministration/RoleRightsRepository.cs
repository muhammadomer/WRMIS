using PMIU.WRMIS.DAL.DataAccess.UserAdministration;
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
    public class RoleRightsRepository : Repository<UA_RoleRights>
    {
        WRMIS_Entities _context;
        public RoleRightsRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<UA_RoleRights>();
            _context = context;
        }

        public long RoleRightsID(int _RoleID, long _PageID)
        {
            long ID = (from rr in context.UA_RoleRights
                       where rr.RoleID == _RoleID && rr.PageID == _PageID
                       select rr.ID).FirstOrDefault();

            return ID;
        }

        public bool GetBAdd(int _RoleID, long _PageID)
        {
            bool? qbAdd = (from rr in context.UA_RoleRights
                           where rr.RoleID == _RoleID && rr.PageID == _PageID
                           select rr.BAdd).FirstOrDefault();

            return (qbAdd.HasValue) ? qbAdd.Value : false;
        }

        public bool? GetBEdit(int _RoleID, long _PageID)
        {
            bool? qbEdit = (from rr in context.UA_RoleRights
                            where rr.RoleID == _RoleID && rr.PageID == _PageID
                            select rr.BEdit).FirstOrDefault();

            return (qbEdit.HasValue) ? qbEdit.Value : false;
        }

        public bool? GetBDelete(int _RoleID, long _PageID)
        {
            bool? qbDelete = (from rr in context.UA_RoleRights
                              where rr.RoleID == _RoleID && rr.PageID == _PageID
                              select rr.BDelete).FirstOrDefault();

            return (qbDelete.HasValue) ? qbDelete.Value : false;
        }

        public bool? GetBView(int _RoleID, long _PageID)
        {
            bool? qbView = (from rr in context.UA_RoleRights
                            where rr.RoleID == _RoleID && rr.PageID == _PageID
                            select rr.BView).FirstOrDefault();

            return (qbView.HasValue) ? qbView.Value : false;
        }

        public bool? GetAddVisible(int _RoleID, long _PageID)
        {
            bool? qbView = (from rr in context.UA_RoleRights
                            where rr.RoleID == _RoleID && rr.PageID == _PageID
                            select rr.AddVisible).FirstOrDefault();

            return (qbView.HasValue) ? qbView.Value : true;
        }

        public bool? GetEditVisible(int _RoleID, long _PageID)
        {
            bool? qbView = (from rr in context.UA_RoleRights
                            where rr.RoleID == _RoleID && rr.PageID == _PageID
                            select rr.EditVisible).FirstOrDefault();

            return (qbView.HasValue) ? qbView.Value : true;
        }

        public bool? GetDeleteVisible(int _RoleID, long _PageID)
        {
            bool? qbView = (from rr in context.UA_RoleRights
                            where rr.RoleID == _RoleID && rr.PageID == _PageID
                            select rr.DeleteVisible).FirstOrDefault();

            return (qbView.HasValue) ? qbView.Value : true;
        }

        public bool? GetViewVisible(int _RoleID, long _PageID)
        {
            bool? qbView = (from rr in context.UA_RoleRights
                            where rr.RoleID == _RoleID && rr.PageID == _PageID
                            select rr.ViewVisible).FirstOrDefault();

            return (qbView.HasValue) ? qbView.Value : true;
        }

        public List<dynamic> GetRoleModules(int _RoleID, int _ModuleID)
        {
            //List<dynamic> lstPages = new List<dynamic>();
            try
            {
                List<dynamic> lstResult = (from p in context.UA_Pages
                                           where p.ModuleID == _ModuleID && p.ShowInRoleRights == true
                                           select
                                           new
                                           {
                                               p.ID,
                                               p.Name,
                                               p.BAddVisible,
                                               p.BEditVisible,
                                               p.BDeleteVisible,
                                               p.BViewVisible,
                                               p.BPrintVisible,
                                               p.BExportVisible,
                                               p.ParentID,
                                               p.SortOrder
                                           }).ToList()
                           .Select(q =>
                           new
                           {
                               PageID = q.ID,
                               Name = q.Name,
                               ID = RoleRightsID(_RoleID, q.ID),
                               BAdd = GetBAdd(_RoleID, q.ID),
                               BEdit = GetBEdit(_RoleID, q.ID),
                               BDelete = GetBDelete(_RoleID, q.ID),
                               BView = GetBView(_RoleID, q.ID),
                               ParentID = q.ParentID,
                               SortOrder = q.SortOrder,
                               //AddVisible = GetAddVisible(_RoleID, q.ID),
                               //EditVisible = GetEditVisible(_RoleID, q.ID),
                               //DeleteVisible = GetDeleteVisible(_RoleID, q.ID),
                               //ViewVisible = GetViewVisible(_RoleID, q.ID)

                               AddVisible = q.BAddVisible,
                               EditVisible = q.BEditVisible,
                               DeleteVisible = q.BDeleteVisible,
                               ViewVisible = q.BViewVisible
                           })
                           .OrderBy(a => a.SortOrder)
                           .ToList<dynamic>();



                //foreach (var v in lstResult)
                //    lstPages.AddRange(v);

                //var lstRoleRights = (from modules in context.UA_Modules
                //                     join pages in context.UA_Pages on modules.ID equals pages.ModuleID
                //                     join rolerights in context.UA_RoleRights on pages.ID equals rolerights.PageID into pr
                //                     from p in pr.DefaultIfEmpty()
                //                     where modules.ID == _ModuleID && p.RoleID == _RoleID && pages.ShowInRoleRights == true
                //                     orderby pages.Name
                //                     select
                //                         new
                //                         {
                //                             p.ID,
                //                             p.RoleID,
                //                             p.PageID,
                //                             p.BAdd,
                //                             p.BEdit,
                //                             p.BDelete,
                //                             p.BPrint,
                //                             p.BView,
                //                             p.BExport,
                //                             p.AddVisible,
                //                             p.EditVisible,
                //                             p.DeleteVisible,
                //                             p.PrintVisible,
                //                             p.ViewVisible,
                //                             p.ExportVisible,
                //                             pages.Name,
                //                             pages.Description,
                //                             pages.ParentID
                //                             //moduleID= modules.ID,
                //                             //moduleName = modules.Name
                //                         }

                //               ).ToList<object>();

                return lstResult;

            }
            catch (WRException exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.DataAccess);
                return new List<object>();
            }
        }

        public UA_RoleRights GetUserEditRights(long UserID, long PageID)
        {
            UA_RoleRights objRR = new UA_RoleRights();
            try
            {
                objRR = (from usr in context.UA_Users
                         join rr in context.UA_RoleRights on usr.RoleID equals rr.RoleID
                         where usr.ID == UserID && rr.PageID == PageID
                         select rr).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objRR;
        }


        public List<object> GetAndroidUserRoleRights(long _UserID)
        {
            long? roleID = (from user in context.UA_Users where user.ID == _UserID select user).SingleOrDefault().RoleID;
            if (roleID == null)
                return null;

            var lstRights = (from uas in context.UA_AndroidScreens
                             join uarr in context.UA_AndroidRoleRights on uas.ID equals uarr.ScreenID
                             join ur in context.UA_Roles on uarr.RoleID equals ur.ID

                             where uarr.BView == true && uarr.RoleID == roleID

                             select new { uas.Name, uas.ParentID, uarr.BView, uas.ShowInMenu }
                             )
                             .ToList()
                             .Select
                             (
                                x => new { Name = x.Name, ParentName = GetScreenNameByID(x.ParentID), HasAccess = x.BView, ShowInMenu = x.ShowInMenu }
                             )
                             .ToList<object>();

            return lstRights;
        }

        public string GetScreenNameByID(long? _ScreenID)
        {
            if (_ScreenID != null && _ScreenID > 0)
                return (from uas in context.UA_AndroidScreens where uas.ID == _ScreenID select uas).SingleOrDefault().Name;
            else
                return "";
        }

        public void InsertParentEntry(UA_RoleRights _ObjRR)
        {
            UA_RoleRights Parent = new UA_RoleRights();
            try
            {
                UA_Pages objResult = (from p in context.UA_Pages
                                      where p.ID == _ObjRR.PageID
                                      select p).FirstOrDefault();

                if (objResult != null)
                {
                    long? ParentID = objResult.ParentID;

                    UA_RoleRights ObjParentExist = (from rr in context.UA_RoleRights
                                                    where rr.PageID == _ObjRR.PageID && rr.RoleID == _ObjRR.RoleID
                                                    select rr).FirstOrDefault();
                    if (ObjParentExist == null)
                    {
                        Parent = _ObjRR;
                        Parent.PageID = ParentID;
                        new RoleRightsDAL().InsertParentEntry(Parent);
                    }

                    if (Parent != null)
                        InsertParentEntry(Parent);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }
    }
}
