using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.DAL.DataAccess.UserAdministration;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.UserAdministration
{
    public class UserAdministrationBLL : BaseBLL
    {
        /// <summary>
        /// This function return list of those users does not have any role
        /// Created on 22-12-2015
        /// </summary>        
        /// <returns>List<object></returns>
        public List<object> GetUnAssignedRoleUsers()
        {//
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUnAssignedRoleUsers();
        }
        /// <summary>
        /// This function return users having role
        /// Created on 22-12-2015
        /// </summary>
        /// <param name="_RoleID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAssignedRoleUsers(long _RoleID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetAssignedRoleUsers(_RoleID);
        }
        /// <summary>
        /// This function assign Role to Users
        /// Created on 22-12-2015
        /// </summary>
        /// <param name="_RoleID"></param>
        /// <param name="_lstUsers"></param>
        /// <returns>bool</returns>
        public bool AssignRoleToUsers(long _RoleID, List<long> _lstUsers)
        {
            // Get users have particular role
            List<UA_Users> lstUsers = (from u in db.Repository<UA_Users>().GetAll()
                                       join l in _lstUsers on u.ID equals l
                                       select u)
                                       .Union(
                                       from x in db.Repository<UA_Users>().GetAll()
                                       where x.RoleID == _RoleID
                                       select x
                                       )
                .ToList<UA_Users>();

            if (lstUsers != null && lstUsers.Count > 0)
            {
                // Remove role
                foreach (UA_Users user in lstUsers)
                {
                    user.RoleID = null;
                }

                // Assing role to user
                foreach (long item in _lstUsers)
                {
                    UA_Users user = lstUsers.Where(u => u.ID == item).SingleOrDefault<UA_Users>();
                    user.RoleID = _RoleID;
                    db.Repository<UA_Users>().Update(user);
                }
                db.Save();

                return true;
            }
            else
                return false;
        }
        public UA_Users GetUserPasswordID(string _MobileNo)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserPasswordID(_MobileNo);
        }


        #region Associate Location to User

        public bool IsRecordExist(long _IrrigationLevelID, long _IrrigationBoundryID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.IsRecordExist(_IrrigationLevelID, _IrrigationBoundryID);
        }

        public UA_IrrigationLevel GetUserLevel(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserLevel(_UserID);
        }

        public List<object> GetAssignedLevels(long UserID, long LevelID, string LevelName)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetAssignedLevels(UserID, LevelID, LevelName).ToList();
        }

        public List<CO_Zone> GetUserZones(long UserID, long LevelID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserZones(UserID, LevelID);
        }

        public List<CO_Circle> GetUserCircle(long UserID, long LevelID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserCircle(UserID, LevelID);
        }

        public List<CO_Zone> GetZonesRelatedToCircle(List<CO_Circle> Lstcircle)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetZonesRelatedToCircle(Lstcircle);
        }

        public List<CO_Division> GetUserDivisions(long UserID, long LevelID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserDivisions(UserID, LevelID);
        }


        public List<CO_Circle> GetCirclesRelatedToDivision(List<CO_Division> lstDivision)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetCirclesRelatedToDivision(lstDivision);
        }

        public List<CO_SubDivision> GetUserSubDivision(long UserID, long LevelID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserSubDivision(UserID, LevelID);
        }

        public List<CO_Division> GetDivisionsReltedToSubDivision(List<CO_SubDivision> lstSubDivision)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetDivisionsReltedToSubDivision(lstSubDivision);
        }

        public List<CO_Section> GetUserSections(long UserID, long LevelID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserSections(UserID, LevelID);
        }

        public List<CO_SubDivision> GetSubDivisionReltedToSection(List<CO_Section> lstSection)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetSubDivisionReltedToSection(lstSection);
        }

        public void AddUserLocation(long UserID, long LevelID, List<long> BoundryID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            dalUA.AddUserLocation(UserID, LevelID, BoundryID);
        }

        public object GetUserDetail(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserDetail(_UserID);
        }

        // rework starts here 

        public List<object> GetAssignedLevelsList(long _UserID, long _LevelID, string _LevelName)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetAssignedLevelsList(_UserID, _LevelID, _LevelName);
        }

        public bool LocationAlreadyAssigned(long _LevelID, long _BoundryID, long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.LocationAlreadyAssigned(_LevelID, _BoundryID, _UserID);
        }

        public bool LocationAlreadyAssignedUpdate(long _LevelID, long _BoundryID, long _UserID, long _RecordID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.LocationAlreadyAssignedUpdate(_LevelID, _BoundryID, _UserID, _RecordID);
        }

        public bool CheckMultipleLocationAlreadyAssigned(long _LevelID, List<long> lstBoundryID, long _UserID,long _RecordID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.CheckMultipleLocationAlreadyAssigned(_LevelID, lstBoundryID, _UserID, _RecordID);
        }


        public bool AssignLocation(long _UserID, long _LevelID, long _BoundryID, long? _DesignationID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.AssignLocation(_UserID, _LevelID, _BoundryID, _DesignationID);
        }

        public object GetUserDetail(long _ID, string _LevelName)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserDetail(_ID, _LevelName);
        }

        public void DeleteLocation(long _RecordID, long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            dalUA.DeleteLocation(_RecordID, _UserID);
        }

        public bool UpdateLocation(long _RecordID, long _LevelID, long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.UpdateLocation(_RecordID, _LevelID, _UserID);
        }

        public bool SaveLocation(List<UA_AssociatedLocation> _lstAssociatedLocation)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.SaveLocation(_lstAssociatedLocation);
        }

        public string GetRoleName(long _USerID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetRoleName(_USerID);
        }

        public bool? HasRights(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.HasRights(_UserID);
        }

        public bool AssociationExistAgainstLocation(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.AssociationExistAgainstLocation(_UserID);
        }

        public UA_AssociatedLocation GetUserAssociateLocation(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserAssociateLocation(_UserID);
        }

        #endregion

        #region Associate Barrage Channel and Oulets

        public object UserBasicInformation(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.UserBasicInformation(_UserID);
        }

        public List<object> UserInfo(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.UserInfo(_UserID);
        }

        public List<string> SectionUser(List<object> _lstInfo)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.SectionUser(_lstInfo);
        }

        public List<string> SubDivisionUser(List<object> _lstInfo)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.SubDivisionUser(_lstInfo);
        }

        public List<string> DivisionUser(List<object> _LstInfo)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.DivisionUser(_LstInfo);
        }

        public List<string> CircleUser(List<object> _LstInfo)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.CircleUser(_LstInfo);
        }

        public List<string> ZoneUser(List<object> _LstInfo)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.ZoneUser(_LstInfo);
        }

        public List<CO_Station> GetAllBarrages()
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetAllBarrages();
        }

        public List<object> GetExistingBarrageAssociation(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetExistingBarrageAssociation(_UserID).ToList();
        }

        public bool SaveData(UA_AssociatedStations _ObjSave)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.SaveData(_ObjSave);
        }
   

        public void DeleteData(long _RecordID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            dalUA.DeleteData(_RecordID);
        }

        public bool UpdateData(UA_AssociatedStations _ObjUpdate)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.UpdateData(_ObjUpdate);
        }

        public List<object> GetExisitngChannelAssociation(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetExisitngChannelAssociation(_UserID);
        }

        public List<object> GetUserCahnnels(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetUserCahnnels(_UserID);
        }

        public List<object> GetChannelRDs(long _ChannelID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetChannelRDs(_ChannelID);
        }

        public List<object> GetExistingOutletAssociations(long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetExistingOutletAssociations(_UserID);
        }

        public List<object> GetChannelOutlets(long _ChannelID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetChannelOutlets(_ChannelID);
        }
        public List<object> GetChannelOutletsWhichAlreadyNotSaved(long _ChannelID, long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetChannelOutletsWhichAlreadyNotSaved(_ChannelID, _UserID);
        }
        public List<object> GetChannelRDsWhichAlreadyNotSaved(long _ChannelID, long _UserID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetChannelRDsWhichAlreadyNotSaved(_ChannelID, _UserID);
        }
        

        public object GetBarrageAssociationsRecord(long _RecordID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetBarrageAssociationsRecord(_RecordID);
        }

        public object GetChannelAssociationsRecord(long RecordID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetChannelAssociationsRecord(RecordID);
        }

        public object GetOutletAssociationsRecord(long _RecordID)
        {
            UserAdministrationDAL dalUA = new UserAdministrationDAL();
            return dalUA.GetOutletAssociationsRecord(_RecordID);
        }


        #endregion

        public List<object> GetNotificationLimit()
        {
            UserAdministrationDAL dalUserAdmin = new UserAdministrationDAL();
            return dalUserAdmin.GetNotificationLimit();
        }

        #region Search User

        /// <summary>
        /// This function return all locations of the user
        /// Created On 22-01-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <returns>List<UA_AssociatedLocation></returns>
        public List<UA_AssociatedLocation> GetUserLocationsByUserID(long _UserID)
        {
            UserAdministrationDAL dalUserAdministration = new UserAdministrationDAL();

            return dalUserAdministration.GetUserLocationsByUserID(_UserID);
        }

        /// <summary>
        /// This function returns true if the user designation is reporting to the manager designation.
        /// Created On 27-01-2016
        /// </summary>
        /// <param name="_ManagerDesignationID"></param>
        /// <param name="_UserDesignationID"></param>
        /// <returns>bool</returns>
        public bool IsReportingDesignation(long _ManagerDesignationID, long _UserDesignationID)
        {
            UserAdministrationDAL dalUserAdministration = new UserAdministrationDAL();

            return dalUserAdministration.IsReportingDesignation(_ManagerDesignationID, _UserDesignationID);
        }

        /// <summary>
        /// This function returns users based on the provided criteria.
        /// Created On 27-01-2016
        /// </summary>
        /// <param name="_UserName"></param>
        /// <param name="_FullName"></param>
        /// <param name="_OrganizationID"></param>
        /// <param name="_DesignationID"></param>
        /// <param name="_RoleID"></param>
        /// <param name="_Status"></param>
        /// <param name="_ZoneID"></param>
        /// <param name="_CircleID"></param>
        /// <param name="_DivisionID"></param>
        /// <param name="_SubDivisionID"></param>
        /// <param name="_SectionID"></param>
        /// <param name="_UserID"></param>
        /// <returns>List<UA_Users></returns>
        public List<UA_Users> GetUsers(string _UserName, string _FullName, long _OrganizationID, long _DesignationID, long _RoleID, bool? _Status,
            long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _SectionID, long _UserID, long? _UserLevelID,
            long _UserRoleID)
        {
            UserAdministrationDAL dalUserAdministration = new UserAdministrationDAL();

            return dalUserAdministration.GetUsers(_UserName, _FullName, _OrganizationID, _DesignationID, _RoleID, _Status, _ZoneID,
                _CircleID, _DivisionID, _SubDivisionID, _SectionID, _UserID, _UserLevelID, _UserRoleID);
        }

        #endregion

        public List<object> GetRegionsListByUser(long _UserID, int _IrrigationBoundaryID)
        {
            UserAdministrationDAL dalUserAdministration = new UserAdministrationDAL();

            return dalUserAdministration.GetRegionsListByUser(_UserID, _IrrigationBoundaryID);
        }

        public string GetSwitchUsers(long _UserID, DateTime _Now)
        {
            UserAdministrationDAL dalUserAdministration = new UserAdministrationDAL();
            List<object> lstUsers = dalUserAdministration.GetSwitchUsers(_UserID, _Now);

            string output = "";
            int i = 1;
            foreach (object obj in lstUsers)
            {
                output += "<li><a id=\"lnkSwitchUser_" + i + "\" href=\"/SwitchUser.aspx?ID=" + obj.GetType().GetProperty("AssignedUserID").GetValue(obj) + "\">Log In as '" + obj.GetType().GetProperty("FullName").GetValue(obj) + "'</a></li>";
                ++i;
            }

            if (output.Length > 0)
                output += "<li class=\"divider\"></li>";

            return output;
        }

        public string RevertSwitchUser(long _UserID)
        {
            return "<li><a id=\"lnkRevertSwitchUser\" href=\"/SwitchUser.aspx?ID=" + _UserID + "\"><< Revert Back Account</a></li>" +
                        "<li class=\"divider\"></li>";
        }

        public List<object> GetUserManagerByLocationID(long _UserID)
        {

            UserAdministrationDAL dalUserAdministration = new UserAdministrationDAL();
            object objManager = dalUserAdministration.GetUserManagerByLocationID(_UserID);

            List<object> lstManagers = new List<object>();

            lstManagers.Add(objManager);

            return lstManagers;
        }
        public List<UA_SystemParameters> GetSystemParameterValue(string _PKey)
        {
            UserAdministrationDAL dalUserAdministration = new UserAdministrationDAL();
            return dalUserAdministration.GetSystemParameterValue(_PKey);
        }
    }
}
