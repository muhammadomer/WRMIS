using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.DAL.DataAccess;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.DAL.DataAccess.UserAdministration;
using System.Data;

namespace PMIU.WRMIS.BLL.UserAdministration
{
    public class UserBLL : BaseBLL
    {
        UserDAL dalUser = new UserDAL();

        #region User

        /// <summary>
        /// This function returns list of all organization.
        /// Created On 23-12-2015.
        /// </summary>
        /// <returns>List<UA_Organization></returns>
        public List<UA_Organization> GetAllOrganizations(bool? _IsActive = null)
        {
            return dalUser.GetAllOrganizations(_IsActive);
        }

        /// <summary>
        /// This function returns list of all designations.
        /// Created On 23-12-2015.
        /// </summary>
        /// <param name="_OrganizationID"></param>
        /// <returns>List<UA_Designations></returns>
        public List<UA_Designations> GetAllDesignations(long _OrganizationID = -1, bool? _IsActive = null)
        {
            return dalUser.GetAllDesignations(_OrganizationID, _IsActive);
        }

        /// <summary>
        /// This function return true if UserName already exists in database.
        /// Created On 23-12-2015.
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_UserName"></param>
        /// <returns>bool</returns>
        public bool IsUserNameExists(long _UserID, string _UserName)
        {
            return dalUser.IsUserNameExists(_UserID, _UserName);
        }
        public UA_Users GetUserbyID(long _ID)
        {
            return dalUser.GetUserbyID(_ID);
        }
        public long UserPasswordUpdation(long _UserID, string _Password)
        {
            return dalUser.UserPasswordUpdation(_UserID, _Password);
        }
        /// <summary>
        /// This function return true if UserEmail already exists in database.
        /// Created On 14-04-2016.
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_UserEmail"></param>
        /// <returns>bool</returns>
        public bool IsUserEmailExists(long _UserID, string _UserEmail)
        {
            return dalUser.IsUserEmailExists(_UserID, _UserEmail);
        }

        /// <summary>
        /// This function return true if UserMobile already exists in database.
        /// Created On 14-04-2016.
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_UserMobile"></param>
        /// <returns>bool</returns>
        public bool IsUserMobileExists(long _UserID, string _UserMobile)
        {
            return dalUser.IsUserMobileExists(_UserID, _UserMobile);
        }

        /// <summary>
        /// This function adds new user to the database
        /// Created On 23-12-2015
        /// </summary>
        /// <param name="_User"></param>
        /// <returns>bool</returns>
        public bool AddUser(UA_Users _User)
        {
            return dalUser.AddUser(_User);
        }

        /// <summary>
        /// This function updates the user.
        /// Created On 23-12-2015
        /// </summary>
        /// <param name="_User"></param>
        /// <returns>bool</returns>
        public bool UpdateUser(UA_Users _User, long? _Designation)
        {
            return dalUser.UpdateUser(_User, _Designation);
        }

        /// <summary>
        /// This function finds user by ID
        /// Created On 23-12-2015
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>UA_Users</returns>
        public UA_Users GetUserByID(long _UserID)
        {
            return dalUser.GetUserByID(_UserID);
        }

        /// <summary>
        /// This function finds users by DesignationID
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetUsersByDesignationID(long? _DesignationID, long _UserID)
        {
            return dalUser.GetUsersByDesignationID(_DesignationID, _UserID);
        }

        /// <summary>
        /// This function finds user by UserName
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_UserName"></param>
        /// <returns>UA_Users</returns>
        public UA_Users GetUserByUserName(string _UserName)
        {
            return dalUser.GetUserByUserName(_UserName);
        }

        /// <summary>
        /// This function adds new user manager to the database
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_UserManager"></param>
        /// <returns>bool</returns>
        public bool AddUserManager(UA_UserManager _UserManager)
        {
            return dalUser.AddUserManager(_UserManager);
        }

        /// <summary>
        /// This function updates the user manager.
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_UserManager"></param>
        /// <returns>bool</returns>
        public bool UpdateUserManager(UA_UserManager _UserManager)
        {
            return dalUser.UpdateUserManager(_UserManager);
        }

        /// <summary>
        /// This function deletes a record from the user manager table.
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_UserManagerID"></param>
        /// <returns>bool</returns>
        public bool DeleteUserManager(long _UserManagerID)
        {
            return dalUser.DeleteUserManager(_UserManagerID);
        }

        #endregion

        public List<UA_Designations> GetDesignationAgainstOrganization(long OrganizationID)
        {
            return db.ExtRepositoryFor<SearchUserRepository>().GetDesignationAgainstOrganization(OrganizationID).ToList();
        }

        public long AddUsr(UA_Users _User)
        {
            return dalUser.AddUsr(_User);
        }

        //public List<object> GetDesignations(string _Name)
        //{
        //    List<object> lstDesignations = (from d in db.Repository<UA_Designations>().GetAll()
        //                                    join t in db.Repository<UA_DesignationType>().GetAll() on d.DesignationTypeID equals t.ID
        //                                    where d.Name.Contains(_Name)
        //                                    select new
        //                                    {
        //                                        ID = d.ID,
        //                                        Designation = d.Name,
        //                                        Type = t.Name,
        //                                        TypeDetail = d.DesignationTypeDetails

        //                                    }).ToList<object>();

        //    List<object> lstDesignations = db.Repository<UA_Designations>().GetAll().Where(d => d.Name.Contains(_Name)).Select(d => new
        //                                    {
        //                                        ID = d.ID,
        //                                        Designation = d.Name,
        //                                        Type = d.UA_DesignationType.Name,
        //                                        TypeDetail = d.DesignationTypeDetails

        //                                    }).ToList<object>();
        //    return lstDesignations;
        //}

        public List<UA_Roles> GetAllRoles()
        {
            List<UA_Roles> lstRoles = db.Repository<UA_Roles>().GetAll().ToList<UA_Roles>();
            return lstRoles;
        }

        #region Acting Role

        /// <summary>
        /// This function return user information based on the inputted string
        /// Created On: 04-01-2016
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetUserInfo(string _Name, long _ManagerID)
        {
            return dalUser.GetUserInfo(_Name, _ManagerID);
        }

        /// <summary>
        /// This function return acting roles list based on the provided criteria
        /// Created On: 08-01-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_AssignedUserID"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetActingRoles(long _UserID, long _AssignedUserID, DateTime? _FromDate, DateTime? _ToDate, UA_Users _LoginUser)
        {
            return dalUser.GetActingRoles(_UserID, _AssignedUserID, _FromDate, _ToDate, _LoginUser);
        }

        /// <summary>
        /// This function checks if there are any overlapping assignments.
        /// Created On: 11-01-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns>bool</returns>
        public bool IsUserAssigned(long _UserID, DateTime? _FromDate, DateTime? _ToDate)
        {
            return dalUser.IsUserAssigned(_UserID, _FromDate, _ToDate);
        }

        /// <summary>
        /// This function adds new acting role.
        /// Created On 11-01-2016.
        /// </summary>
        /// <param name="_ActingRole"></param>
        /// <returns>bool</returns>
        public bool AddActingRole(UA_ActingRoles _ActingRole)
        {
            return dalUser.AddActingRole(_ActingRole);
        }

        /// <summary>
        /// This function finds acting role by ID
        /// Created On 11-01-2016
        /// </summary>
        /// <param name="_ActingRoleID"></param>
        /// <returns>UA_ActingRoles</returns>
        public UA_ActingRoles GetActingRoleByID(long _ActingRoleID)
        {
            return dalUser.GetActingRoleByID(_ActingRoleID);
        }

        /// <summary>
        /// This function updates the acting role.
        /// Created On 11-01-2016
        /// </summary>
        /// <param name="_ActingRole"></param>
        /// <returns>bool</returns>
        public bool UpdateActingRole(UA_ActingRoles _ActingRole)
        {
            return dalUser.UpdateActingRole(_ActingRole);
        }

        /// <summary>
        /// This function deletes an acting role with the provided ID.
        /// Created On 11-01-2016.
        /// </summary>
        /// <param name="_ActingRoleID"></param>
        /// <returns>bool</returns>
        public bool DeleteActingRole(long _ActingRoleID)
        {
            return dalUser.DeleteActingRole(_ActingRoleID);
        }

        #endregion

        #region Menu

        public UA_RoleRights GetRoleId(string roleName)
        {
            return dalUser.GetRoleId(roleName);
        }

        public UA_RoleRights GetRoleRights(long RoleID, long PageID)
        {
            return dalUser.GetRoleRights(RoleID, PageID);
        }

        public UA_RoleRights GetRights(int _RoleID, string PageName, int _ParentPageID)
        {
            return dalUser.GetRights(_RoleID, PageName, _ParentPageID);
        }

        public UA_RoleRights GetRights_1(long _RoleID, string PageName)
        {
            return dalUser.GetRights_1(_RoleID, PageName);
        }

        public bool IsVisible(int roleId, string pageName)
        {
            return dalUser.IsVisible(roleId, pageName);
        }

        public List<UA_Pages> GenerateMenu(long userRoleId)
        {
            return dalUser.GenerateMenu(userRoleId);
        }

        public List<object> GenerateMenu_Dashboard(long userRoleId)
        {
            return dalUser.GenerateMenu_Dashboard(userRoleId);
        }

        public List<UA_Pages> GetChildMenuRows(long PageID, long _RoleID)
        {
            return dalUser.GetChildMenuRows(PageID, _RoleID);
        }

        /// <summary>
        /// This function return page ID along with its all parent IDs.
        /// </summary>
        /// <param name="_PageName"></param>
        /// <returns>List<long></returns>
        public List<long> GetParentPageIDs(string _PageName)
        {
            return dalUser.GetParentPageIDs(_PageName);
        }

        public List<UA_RoleRights> GetRoleRightsByUserID(long _RoleID)
        {
            return dalUser.GetRoleRightsByUserID(_RoleID);
        }

        #endregion

        #region Tokens

        public string GenerateToken(long _UserID)
        {
            return dalUser.GenerateToken(_UserID);
        }

        public bool ValidateToken(string _AuthTokenKey)
        {
            return dalUser.ValidateToken(_AuthTokenKey);
        }

        public bool KillToken(string _AuthTokenKey)
        {
            return dalUser.KillToken(_AuthTokenKey);
        }

        public bool DeleteTokenByUserID(long _UserID)
        {
            return dalUser.DeleteTokenByUserID(_UserID);
        }

        #endregion

        #region User General

        /// <summary>
        /// This function fetches the Manager for a particular User.
        /// Created on 20-07-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>UA_UserManager</returns>
        public UA_UserManager GetUserManager(long _UserID)
        {
            return dalUser.GetUserManager(_UserID);
        }

        #endregion


        #region Notification
        public long GetAlertCount(long _UserID)
        {
            return dalUser.GetAlertCount(_UserID);
        }

        public List<dynamic> GetNotificationAlertList(long _UserID)
        {
            return dalUser.GetNotificationAlertList(_UserID);
        }

        public DataTable GetAllNotificationBySearchCriteria(long _StausID, DateTime? _FromDate, DateTime? _ToDate, long _UserID)
        {
            return dalUser.GetAllNotificationBySearchCriteria(_StausID, _FromDate, _ToDate, _UserID);
        }

        public long UpdateAlertCount(long _UserID)
        {
            return dalUser.UpdateAlertCount(_UserID);
        }

        public bool UnreadAllAlertNotification(List<dynamic> _lstofID, Int16 _StatusID)
        {
            return dalUser.UnreadAllAlertNotification(_lstofID, _StatusID);
        }

        public bool ConvertToAsRead(long _RowID)
        {
            return dalUser.ConvertToAsRead(_RowID);
        }

        public UA_SystemParameters GetSystemParameterValue(short _ID)
        {
            return dalUser.GetSystemParameterValue(_ID);
        }

        #endregion

        #region Add User

        public List<object> GetManagers(long _DesID, long _UserID)
        {
            return dalUser.GetManagers(_DesID, _UserID);
        }

        public bool AddManagerAndActivateUser(UA_UserManager UM, long _UserID)
        {
            return dalUser.AddManagerAndActivateUser(UM, _UserID);
        }

        public bool ActivateUser(long _UserID)
        {
            return dalUser.ActivateUser(_UserID);
        }

        public bool ResetPassword(long _UserID, string _Password)
        {
            return dalUser.ResetPassword(_UserID, _Password);
        }

        #endregion
    }
}
