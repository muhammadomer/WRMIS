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

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{
    public class UserDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns list of all organization.
        /// Created On 23-12-2015.
        /// </summary>
        /// <returns>List<UA_Organization></returns>
        public List<UA_Organization> GetAllOrganizations(bool? _IsActive = null)
        {
            List<UA_Organization> lstOrganization = db.Repository<UA_Organization>().GetAll().Where(o => (o.IsActive == _IsActive || _IsActive == null)).OrderBy(o => o.Name).ToList();

            return lstOrganization;
        }

        /// <summary>
        /// This function returns list of all designations.
        /// Created On 23-12-2015.
        /// </summary>
        /// <param name="_OrganizationID"></param>
        /// <returns>List<UA_Designations></returns>
        public List<UA_Designations> GetAllDesignations(long _OrganizationID = -1, bool? _IsActive = null)
        {
            List<UA_Designations> lstDesignation = db.Repository<UA_Designations>().GetAll().Where(d => (d.OrganizationID == _OrganizationID || _OrganizationID == -1) && (d.IsActive == _IsActive || _IsActive == null)).OrderBy(d => d.Name).ToList();

            return lstDesignation;
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
            bool qExists = db.Repository<UA_Users>().GetAll().Any(u => u.LoginName.ToUpper().Trim() == _UserName.ToUpper().Trim() && u.ID != _UserID);

            return qExists;
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
            bool qExists = db.Repository<UA_Users>().GetAll().Any(u => u.Email.ToUpper().Trim() == _UserEmail.ToUpper().Trim() && u.ID != _UserID);

            return qExists;
        }
        public UA_Users GetUserbyID(long _ID)
        {
            UA_Users qUser = db.Repository<UA_Users>().GetAll().Where(s => s.ID == _ID).FirstOrDefault();
            return qUser;
        }
        public long UserPasswordUpdation(long _UserID, string _Password)
        {
            long _ODIIDOut = 0;
            ContextDB dbADO = new ContextDB();
            DataSet DS = dbADO.ExecuteStoredProcedureDataSet("Proc_UA_PasswordUpdate", _UserID, _Password);
            return _ODIIDOut;
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
            bool qExists = db.Repository<UA_Users>().GetAll().Any(u => u.MobilePhone.ToUpper().Trim() == _UserMobile.ToUpper().Trim() && u.ID != _UserID);

            return qExists;
        }

        /// <summary>
        /// This function adds new user to the database
        /// Created On 23-12-2015
        /// </summary>
        /// <param name="_User"></param>
        /// <returns>bool</returns>
        public bool AddUser(UA_Users _User)
        {
            db.Repository<UA_Users>().Insert(_User);
            db.Save();

            return true;
        }

        public long AddUsr(UA_Users _User)
        {
            db.Repository<UA_Users>().Insert(_User);
            db.Save();

            return _User.ID;
        }

        /// <summary>
        /// This function updates the user.
        /// Created On 23-12-2015
        /// </summary>
        /// <param name="_User"></param>
        /// <returns>bool</returns>
        public bool UpdateUser(UA_Users _User, long? _Designation)
        {
            try
            {
                UA_Users mdlUser = db.Repository<UA_Users>().FindById(_User.ID);

                mdlUser.FirstName = _User.FirstName;
                mdlUser.LastName = _User.LastName;
                mdlUser.Email = _User.Email;
                mdlUser.Password = _User.Password;
                mdlUser.LandLineNo = _User.LandLineNo;
                mdlUser.MobilePhone = _User.MobilePhone;
                mdlUser.DesignationID = _User.DesignationID;
                mdlUser.RoleID = _User.RoleID;
                mdlUser.IsActive = _User.IsActive;
                mdlUser.ModifiedBy = _User.ModifiedBy;
                mdlUser.ModifiedDate = _User.ModifiedDate;
                db.Repository<UA_Users>().Update(mdlUser);

                if (_Designation != mdlUser.DesignationID)
                {
                    List<UA_AssociatedLocation> lstAssLoc = db.Repository<UA_AssociatedLocation>().GetAll().Where(q => q.UserID == _User.ID).ToList();
                    if (lstAssLoc.Count() > 0)
                    {
                        foreach (UA_AssociatedLocation AssLoc in lstAssLoc)
                        {
                            UA_AssociatedLocationHistory AssLocHist = new UA_AssociatedLocationHistory();
                            AssLocHist.UserID = AssLoc.UserID;
                            AssLocHist.IrrigationLevelID = AssLoc.IrrigationLevelID;
                            AssLocHist.IrrigationBoundryID = AssLoc.IrrigationBoundryID;
                            AssLocHist.DesignationID = AssLoc.DesignationID;
                            AssLocHist.AssociatedLocationID = AssLoc.ID;
                            AssLocHist.CreatedDate = DateTime.Now;
                            AssLocHist.CreatedBy = _User.ModifiedBy;
                            AssLocHist.ModifiedDate = DateTime.Now;
                            AssLocHist.ModifiedBy = _User.ModifiedBy;

                            db.Repository<UA_AssociatedLocation>().Delete(AssLoc);
                            db.Repository<UA_AssociatedLocationHistory>().Insert(AssLocHist);
                        }
                        // db.ExtRepositoryFor<UserAdministrationRepository>().RemoveLocationsForUser(_User.ID);
                    }
                }
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        /// <summary>
        /// This function finds user by ID
        /// Created On 23-12-2015
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>UA_Users</returns>
        public UA_Users GetUserByID(long _UserID)
        {
            UA_Users mdlUser = db.Repository<UA_Users>().FindById(_UserID);

            return mdlUser;
        }

        public List<UA_Designations> GetDesignationAgainstOrganization(long OrganizationID)
        {
            return db.ExtRepositoryFor<SearchUserRepository>().GetDesignationAgainstOrganization(OrganizationID).ToList();
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

        /// <summary>
        /// This function finds users by DesignationID
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetUsersByDesignationID(long? _DesignationID, long _UserID)
        {
            return db.Repository<UA_Users>().GetAll().Where(u => u.DesignationID == _DesignationID && u.ID != _UserID && u.IsActive == true).Select(u => new { ID = u.ID, Name = u.FirstName + " " + u.LastName + " (" + u.UA_Designations.Name + ")" }).OrderBy(u => u.Name).ToList<dynamic>();
        }

        /// <summary>
        /// This function finds user by UserName
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_UserName"></param>
        /// <returns>UA_Users</returns>
        public UA_Users GetUserByUserName(string _UserName)
        {
            return db.Repository<UA_Users>().GetAll().Where(u => u.LoginName.Trim().ToUpper() == _UserName.Trim().ToUpper()).FirstOrDefault();
        }

        /// <summary>
        /// This function adds new user manager to the database
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_UserManager"></param>
        /// <returns>bool</returns>
        public bool AddUserManager(UA_UserManager _UserManager)
        {
            db.Repository<UA_UserManager>().Insert(_UserManager);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function updates the user manager.
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_UserManager"></param>
        /// <returns>bool</returns>
        public bool UpdateUserManager(UA_UserManager _UserManager)
        {
            UA_UserManager mdlUserManager = db.Repository<UA_UserManager>().FindById(_UserManager.ID);

            mdlUserManager.UserID = _UserManager.UserID;
            mdlUserManager.ManagerID = _UserManager.ManagerID;
            mdlUserManager.UserDesignationID = _UserManager.UserDesignationID;
            mdlUserManager.ManagerDesignationID = _UserManager.ManagerDesignationID;
            mdlUserManager.ModifiedBy = _UserManager.ModifiedBy;
            mdlUserManager.ModifiedDate = _UserManager.ModifiedDate;

            db.Repository<UA_UserManager>().Update(mdlUserManager);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes a record from the user manager table.
        /// Created On 23-08-2016
        /// </summary>
        /// <param name="_UserManagerID"></param>
        /// <returns>bool</returns>
        public bool DeleteUserManager(long _UserManagerID)
        {
            db.Repository<UA_UserManager>().Delete(_UserManagerID);
            db.Save();

            return true;
        }

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
            List<dynamic> lstUserInfo = db.ExtRepositoryFor<UserAdministrationRepository>().GetUserInfo(_Name, _ManagerID);

            return lstUserInfo;
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
            List<dynamic> lstActingRole = db.ExtRepositoryFor<UserAdministrationRepository>().GetActingRoles(_UserID, _AssignedUserID, _FromDate, _ToDate, _LoginUser);

            return lstActingRole;
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
            bool IsAssigned = db.Repository<UA_ActingRoles>().GetAll().Any(ar => ar.AssignedUserID == _UserID && ar.FromDate <= _ToDate && _FromDate <= ar.ToDate);

            return IsAssigned;
        }

        /// <summary>
        /// This function adds new acting role.
        /// Created On 11-01-2016.
        /// </summary>
        /// <param name="_ActingRole"></param>
        /// <returns>bool</returns>
        public bool AddActingRole(UA_ActingRoles _ActingRole)
        {
            db.Repository<UA_ActingRoles>().Insert(_ActingRole);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function finds acting role by ID
        /// Created On 11-01-2016
        /// </summary>
        /// <param name="_ActingRoleID"></param>
        /// <returns>UA_ActingRoles</returns>
        public UA_ActingRoles GetActingRoleByID(long _ActingRoleID)
        {
            UA_ActingRoles mdlActingRole = db.Repository<UA_ActingRoles>().FindById(_ActingRoleID);

            return mdlActingRole;
        }

        /// <summary>
        /// This function updates the acting role.
        /// Created On 11-01-2016
        /// </summary>
        /// <param name="_ActingRole"></param>
        /// <returns>bool</returns>
        public bool UpdateActingRole(UA_ActingRoles _ActingRole)
        {
            UA_ActingRoles mdlActingRole = db.Repository<UA_ActingRoles>().FindById(_ActingRole.ID);

            mdlActingRole.UserID = _ActingRole.UserID;
            mdlActingRole.DesignationID = _ActingRole.DesignationID;
            mdlActingRole.AssignedUserID = _ActingRole.AssignedUserID;
            mdlActingRole.AssignedDesignationID = _ActingRole.AssignedDesignationID;
            mdlActingRole.FromDate = _ActingRole.FromDate;
            mdlActingRole.ToDate = _ActingRole.ToDate;

            db.Repository<UA_ActingRoles>().Update(mdlActingRole);
            db.Save();

            return true;
        }

        /// <summary>
        /// This function deletes an acting role with the provided ID.
        /// Created On 11-01-2016.
        /// </summary>
        /// <param name="_ActingRoleID"></param>
        /// <returns>bool</returns>
        public bool DeleteActingRole(long _ActingRoleID)
        {
            db.Repository<UA_ActingRoles>().Delete(_ActingRoleID);
            db.Save();

            return true;
        }

        #endregion

        #region Menu

        public UA_RoleRights GetRoleId(string roleName)
        {
            UserAdministrationRepository ResUser = db.ExtRepositoryFor<UserAdministrationRepository>();
            return ResUser.GetRoleId(roleName);
        }

        public UA_RoleRights GetRoleRights(long RoleID, long PageID)
        {
            UserAdministrationRepository ResUser = db.ExtRepositoryFor<UserAdministrationRepository>();
            return ResUser.GetRoleRights(RoleID, PageID);

        }

        public UA_RoleRights GetRights(int _RoleID, string PageName, int _ParentPageID)
        {
            return db.Repository<UA_RoleRights>().Query().Get()
                .Where(x =>
                    x.RoleID == _RoleID
                    && x.UA_Pages.ParentID == _ParentPageID
                    && x.UA_Pages.Name.Equals(PageName))
                .FirstOrDefault<UA_RoleRights>();
        }

        public UA_RoleRights GetRights_1(long _RoleID, string PageName)
        {
            return db.Repository<UA_RoleRights>().Query().Get()
                .Where(x =>
                    x.RoleID == _RoleID
                    && x.UA_Pages.Description.ToUpper().Trim().Contains(PageName.ToUpper().Trim())

                    && x.BView == true

                    )
                .FirstOrDefault<UA_RoleRights>();
        }

        public bool IsVisible(int roleId, string pageName)
        {
            UserAdministrationRepository ResUser = db.ExtRepositoryFor<UserAdministrationRepository>();
            return ResUser.IsVisible(roleId, pageName);
        }

        public List<UA_Pages> GenerateMenu(long userRoleId)
        {
            UserAdministrationRepository ResUser = db.ExtRepositoryFor<UserAdministrationRepository>();
            List<UA_Pages> Menu = ResUser.GenerateMenu(userRoleId);
            return Menu;
        }

        public List<object> GenerateMenu_Dashboard(long userRoleId)
        {
            UserAdministrationRepository ResUser = db.ExtRepositoryFor<UserAdministrationRepository>();
            List<object> Menu = ResUser.GenerateMenu_Dashboard(userRoleId);
            return Menu;
        }

        public List<UA_Pages> GetChildMenuRows(long PageID, long _RoleID)
        {
            UserAdministrationRepository ResUser = db.ExtRepositoryFor<UserAdministrationRepository>();
            List<UA_Pages> ChildItems = ResUser.GetChildMenuRows(PageID, _RoleID);
            return ChildItems;

        }

        /// <summary>
        /// This function return page ID along with its all parent IDs.
        /// </summary>
        /// <param name="_PageName"></param>
        /// <returns>List<long></returns>
        public List<long> GetParentPageIDs(string _PageName)
        {
            UA_Pages mdlPage = db.Repository<UA_Pages>().GetAll().Where(p => p.Description.ToUpper().Trim() == _PageName.ToUpper().Trim()).FirstOrDefault();

            List<long> lstPageID = new List<long>();

            if (mdlPage != null)
            {
                lstPageID = db.ExtRepositoryFor<UserAdministrationRepository>().GetParentPageIDs(mdlPage.ID);
            }

            return lstPageID;
        }

        public List<UA_RoleRights> GetRoleRightsByUserID(long _RoleID)
        {
            //UA_Users mdlUser = db.Repository<UA_Users>().FindById(_UserID);

            return db.Repository<UA_RoleRights>().GetAll().Where(e => e.RoleID == _RoleID).ToList<UA_RoleRights>();
        }

        #endregion

        #region Tokens

        public string GenerateToken(long _UserID)
        {
            string guidToken = Guid.NewGuid().ToString();
            DateTime IssuedOn = DateTime.Now;
            DateTime ExpiresOn = DateTime.Now.AddSeconds(3600);

            var mdlToken = new UA_Tokens
            {
                UserID = _UserID,
                AuthToken = guidToken,
                IssuedOn = IssuedOn,
                ExpiresOn = ExpiresOn
            };

            db.Repository<UA_Tokens>().Insert(mdlToken);
            db.Save();

            return mdlToken.AuthToken;
        }

        public bool ValidateToken(string _AuthTokenKey)
        {
            var mdlToken = db.Repository<UA_Tokens>().Query().Get().FirstOrDefault(t => t.AuthToken == _AuthTokenKey && t.ExpiresOn > DateTime.Now);

            if (mdlToken != null && !(DateTime.Now > mdlToken.ExpiresOn))
            {
                mdlToken.ExpiresOn = mdlToken.ExpiresOn.Value.AddSeconds(3600);
                db.Repository<UA_Tokens>().Update(mdlToken);
                db.Save();

                return true;
            }
            return false;
        }

        public bool KillToken(string _AuthTokenKey)
        {
            UA_Tokens mdlToken = db.Repository<UA_Tokens>().Query().Get().FirstOrDefault(t => t.AuthToken == _AuthTokenKey);

            db.Repository<UA_Tokens>().Delete(mdlToken);
            db.Save();

            bool isNotDeleted = db.Repository<UA_Tokens>().Query().Get().Any(t => t.AuthToken == _AuthTokenKey);

            if (isNotDeleted)
                return false;

            return true;
        }

        public bool DeleteTokenByUserID(long _UserID)
        {
            UA_Tokens mdlToken = db.Repository<UA_Tokens>().Query().Get().FirstOrDefault(t => t.UserID == _UserID);

            db.Repository<UA_Tokens>().Delete(mdlToken);
            db.Save();

            return true;
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
            return db.Repository<UA_UserManager>().GetAll().Where(um => um.UserID == _UserID && um.IsActive == true).FirstOrDefault();
        }

        #endregion

        #region Notification

        public long GetAlertCount(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetAlertCount(_UserID);
        }

        public List<dynamic> GetNotificationAlertList(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetNotificationAlertList(_UserID);
        }

        public DataTable GetAllNotificationBySearchCriteria(long _StausID, DateTime? _FromDate, DateTime? _ToDate, long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetAllNotificationBySearchCriteria(_StausID, _FromDate, _ToDate, _UserID);
        }

        public long UpdateAlertCount(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().UpdateAlertCount(_UserID);
        }

        public bool UnreadAllAlertNotification(List<dynamic> _lstofID, Int16 _StatusID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().UnreadAllAlertNotification(_lstofID, _StatusID);
        }

        public bool ConvertToAsRead(long _RowID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().ConvertToAsRead(_RowID);
        }

        public UA_SystemParameters GetSystemParameterValue(short _ID)
        {
            return db.Repository<UA_SystemParameters>().GetAll().Where(sp => sp.ID == _ID).FirstOrDefault();
        }

        #endregion


        #region Add User

        public List<object> GetManagers(long _DesID, long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetManagers(_DesID, _UserID);
        }

        public bool AddManagerAndActivateUser(UA_UserManager _UserManager, long _UserID)
        {
            UA_Users User = db.Repository<UA_Users>().FindById(_UserManager.ManagerID);
            if (User != null && User.DesignationID != null)
            {
                _UserManager.ManagerDesignationID = User.DesignationID;
                db.Repository<UA_UserManager>().Insert(_UserManager);
                User = db.Repository<UA_Users>().FindById(_UserID);
                if (User != null)
                {
                    User.IsActive = true;
                    // User.Status = true;
                    db.Repository<UA_Users>().Update(User);
                }
                db.Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ActivateUser(long _UserID)
        {
            UA_Users User = db.Repository<UA_Users>().FindById(_UserID);
            User.IsActive = true;
            // User.Status = true;
            db.Repository<UA_Users>().Update(User);
            db.Save();
            return true;
        }

        public bool ResetPassword(long _UserID, string _Password)
        {
            try
            {
                UA_Users User = db.Repository<UA_Users>().FindById(_UserID);
                User.Password = _Password;
                db.Repository<UA_Users>().Update(User);
                db.Save();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        #endregion
    }
}
