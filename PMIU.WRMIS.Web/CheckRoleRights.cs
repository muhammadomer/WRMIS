using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.Model;

namespace PMIU.WRMIS.Web
{
    public class RoleRightBits
    {
        private bool? add, edit, delete, view, print, export;

        public bool? AddBit
        {
            set { add = value; }
            get { return add; }
        }
        public bool? EditBit
        {
            set { edit = value; }
            get { return edit; }
        }
        public bool? DeleteBit
        {
            set { delete = value; }
            get { return delete; }
        }
        public bool? ViewBit
        {
            set { view = value; }
            get { return view; }
        }
        public bool? PrintBit
        {
            set { print = value; }
            get { return print; }
        }
        public bool? ExportBit
        {
            set { export = value; }
            get { return export; }
        }
    }
    public class CheckRoleRights
    {
        private long userId;
        UserBLL m_controller;
        List<Object> ds;

        public CheckRoleRights(long uid)
        {
            userId = uid;
        }
        public string GetUserRole()
        {
            m_controller = new UserBLL();
            UA_Users user = m_controller.GetUserByID(userId);
            string userRole = "";
            if (user != null)
                userRole = user.UA_Roles.Name;
            return userRole;
        }
        public long GetRoleId(string roleName)
        {
            m_controller = new UserBLL();
            RoleRightBits roleRightBits = new RoleRightBits();
            UA_RoleRights ur = m_controller.GetRoleId(roleName);

            ////ds = new DataSet();
            //m_controller = new UsersBLL();

            //ds = m_controller.getRoleId();

            //foreach(DataRow row in ds.Tables[0].Rows)
            //{
            //   // string temp = row["RoleName"].ToString();
            //    if(String.Compare(row["RoleName"].ToString().Trim(),roleName)==0)
            //    {
            //        return Convert.ToInt32(row["RoleId"]);
            //    }
            //}
            if (ur == null)
                return 0;
            else
                return (long)ur.RoleID; //Check it.
        }

        public long GetRoleID()
        {
            try
            {
                UA_Users user = new UserBLL().GetUserByID(userId);
                if (user != null)
                    return (long)user.RoleID;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public RoleRightBits GetRoleRightBits(long _RoleID, long _PageID)
        {
            m_controller = new UserBLL();
            RoleRightBits roleRightBits = new RoleRightBits();
            UA_RoleRights ur = m_controller.GetRoleRights(_RoleID, _PageID);

            if (ur != null)
            {
                if (ur.BAdd != null)
                {
                    roleRightBits.AddBit = ur.BAdd;
                }
                if (ur.BEdit != null)
                {
                    roleRightBits.EditBit = ur.BEdit;
                }
                if (ur.BDelete != null)
                {
                    roleRightBits.DeleteBit = ur.BDelete;
                }
                if (ur.BPrint != null)
                {
                    roleRightBits.PrintBit = ur.BPrint;
                }
                if (ur.BView != null)
                {
                    roleRightBits.ViewBit = ur.BView;
                }
                if (ur.BExport != null)
                {
                    roleRightBits.ExportBit = ur.BExport;
                }
            }
            return roleRightBits;
        }

        public RoleRightBits GetRoleRightBits(int _RoleID, string _PageName, int _ParentPageID)
        {
            m_controller = new UserBLL();
            RoleRightBits roleRightBits = new RoleRightBits();
            UA_RoleRights ur = m_controller.GetRights(_RoleID, _PageName, _ParentPageID);
            if (ur != null)
            {
                if (ur.BAdd != null)
                {
                    roleRightBits.AddBit = ur.BAdd;
                }
                if (ur.BEdit != null)
                {
                    roleRightBits.EditBit = ur.BEdit;
                }
                if (ur.BDelete != null)
                {
                    roleRightBits.DeleteBit = ur.BDelete;
                }
                if (ur.BPrint != null)
                {
                    roleRightBits.PrintBit = ur.BPrint;
                }
                if (ur.BView != null)
                {
                    roleRightBits.ViewBit = ur.BView;
                }
                if (ur.BExport != null)
                {
                    roleRightBits.ExportBit = ur.BExport;
                }
            }
            return roleRightBits;
        }

        public RoleRightBits GetRoleRightBits_1(long _RoleID, string _PageName)
        {
            m_controller = new UserBLL();
            RoleRightBits roleRightBits = new RoleRightBits();
            UA_RoleRights ur = m_controller.GetRights_1(_RoleID, _PageName);
            if (ur != null)
            {
                if (ur.BAdd != null)
                {
                    roleRightBits.AddBit = ur.BAdd;
                }
                if (ur.BEdit != null)
                {
                    roleRightBits.EditBit = ur.BEdit;
                }
                if (ur.BDelete != null)
                {
                    roleRightBits.DeleteBit = ur.BDelete;
                }
                if (ur.BPrint != null)
                {
                    roleRightBits.PrintBit = ur.BPrint;
                }
                if (ur.BView != null)
                {
                    roleRightBits.ViewBit = ur.BView;
                }
                if (ur.BExport != null)
                {
                    roleRightBits.ExportBit = ur.BExport;
                }
            }
            return roleRightBits;
        }

        public bool HasRoleRights(int _RoleID, long _PageID)
        {
            m_controller = new UserBLL();
            RoleRightBits roleRightBits = new RoleRightBits();
            UA_RoleRights ur = m_controller.GetRoleRights(_RoleID, _PageID);
            if (ur == null)
                return false;
            else
                return true;
        }

        public bool HasRoleRights(int _RoleID, string _PageName, int _ParentPageID)
        {
            m_controller = new UserBLL();
            RoleRightBits roleRightBits = new RoleRightBits();
            UA_RoleRights ur = m_controller.GetRights(_RoleID, _PageName, _ParentPageID);
            if (ur == null)
                return false;
            else
                return true;
        }

        public bool HasRoleRights_1(long _RoleID, string _PageName)
        {
            m_controller = new UserBLL();
            RoleRightBits roleRightBits = new RoleRightBits();
            UA_RoleRights ur = m_controller.GetRights_1(_RoleID, _PageName);
            if (ur == null)
                return false;
            else
                return true;
        }

        public bool IsVisible(int roleId, string pageName)
        {
            m_controller = new UserBLL();


            RoleRightBits roleRightBits = new RoleRightBits();

            return m_controller.IsVisible(roleId, pageName);
            //ds = new DataSet();
            //m_controller = new SecurityManager();
            //ds = m_controller.getRoleRights(roleId);
            //RoleRightBits roleRightBits = new RoleRightBits();

            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    //string temp = row["PageDescription"].ToString();
            //    if (Convert.ToInt32(row["RoleID"]) == roleId)
            //    {
            //        if (String.Compare(row["PageName"].ToString(), pageName) == 0)
            //        {
            //            if (Convert.ToInt32(row["bView"]) == 1) // Check it
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //}
            //return false;
        }
    }
}