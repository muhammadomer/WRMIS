using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.UserAdministration
{
    public class UserAdministrationDAL
    {
        public UserAdministrationDAL()
        {

        }
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function return list of those users does not have any role
        /// Created on 22-12-2015
        /// </summary>        
        /// <returns>List<object></returns>
        public List<object> GetUnAssignedRoleUsers()
        {//
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUnAssignedRoleUsers();
        }
        public UA_Users GetUserPasswordID(string _MobileNo)
        {
            UA_Users qUser = db.Repository<UA_Users>().GetAll().Where(s => s.MobilePhone == _MobileNo).FirstOrDefault();
            return qUser;
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


        #region Associate Location to User

        /// <summary>
        /// This function checks if the location exists for the parameters
        /// Created On 04-02-2016
        /// </summary>
        /// <param name="_IrrigationLevelID"></param>
        /// <param name="_IrrigationBoundryID"></param>
        /// <returns>bool</returns>
        public bool IsRecordExist(long _IrrigationLevelID, long _IrrigationBoundryID)
        {
            bool IsExist = db.Repository<UA_AssociatedLocation>().GetAll().Any(asl => asl.IrrigationLevelID == _IrrigationLevelID && asl.IrrigationBoundryID == _IrrigationBoundryID);

            return IsExist;
        }

        public UA_IrrigationLevel GetUserLevel(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserLevel(_UserID);
        }

        public List<object> GetAssignedLevels(long UserID, long LevelID, string LevelName)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetAssignedLevelNames(UserID, LevelID, LevelName).ToList();
        }

        public List<CO_Zone> GetUserZones(long UserID, long LevelID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserZone(UserID, LevelID);
        }


        public List<CO_Circle> GetUserCircle(long UserID, long LevelID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserCircles(UserID, LevelID);
        }

        public List<CO_Zone> GetZonesRelatedToCircle(List<CO_Circle> Lstcircle)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetZonesRelatedToCircles(Lstcircle);
        }

        public List<CO_Division> GetUserDivisions(long UserID, long LevelID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserDivisions(UserID, LevelID);
        }


        public List<CO_Circle> GetCirclesRelatedToDivision(List<CO_Division> lstDivision)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetCirclesRelatedToDivisions(lstDivision);
        }

        public List<CO_SubDivision> GetUserSubDivision(long UserID, long LevelID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserSubDivisions(UserID, LevelID);
        }


        public List<CO_Division> GetDivisionsReltedToSubDivision(List<CO_SubDivision> lstSubDivision)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetDivisionsRelatedToSubDivisions(lstSubDivision);
        }


        public List<CO_Section> GetUserSections(long UserID, long LevelID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserSections(UserID, LevelID);
        }


        public List<CO_SubDivision> GetSubDivisionReltedToSection(List<CO_Section> lstSection)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetSubDivisionsRelatedToSection(lstSection);
        }


        public void AddUserLocation(long UserID, long LevelID, List<long> BoundryID)
        {
            try
            {
                List<UA_AssociatedLocation> lstExistLocations = db.ExtRepositoryFor<UserAdministrationRepository>().ExistingUserLocations(UserID, LevelID).ToList();

                UA_AssociatedLocation UserAssociation;

                foreach (var BID in BoundryID)
                {
                    UserAssociation = new UA_AssociatedLocation();
                    UserAssociation.UserID = UserID;
                    UserAssociation.IrrigationLevelID = LevelID;
                    UserAssociation.IrrigationBoundryID = BID;

                    db.Repository<UA_AssociatedLocation>().Insert(UserAssociation);
                }

                foreach (var assLoc in lstExistLocations)
                {
                    db.Repository<UA_AssociatedLocation>().Delete(assLoc);
                }

                db.Save();

            }
            catch (Exception exp)
            {

            }
        }

        public object GetUserDetail(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserDetail(_UserID);
        }

        // rework starts here 

        public List<object> GetAssignedLevelsList(long _UserID, long _LevelID, string _LevelName)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetAssignedLevels(_UserID, _LevelID, _LevelName);
        }

        public bool LocationAlreadyAssigned(long _LevelID, long _BoundryID, long _UserID)
        {
            bool LocationAssigned = false;
            try
            {
                LocationAssigned = db.ExtRepositoryFor<UserAdministrationRepository>().LocationAlreadyAssigned(_LevelID, _BoundryID, _UserID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return LocationAssigned;
        }

        public bool LocationAlreadyAssignedUpdate(long _LevelID, long _BoundryID, long _UserID, long _RecordID)
        {
            bool LocationAssigned = false;
            try
            {
                LocationAssigned = db.ExtRepositoryFor<UserAdministrationRepository>().LocationAlreadyAssignedUpdate(_LevelID, _BoundryID, _UserID, _RecordID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return LocationAssigned;
        }

        public bool CheckMultipleLocationAlreadyAssigned(long _LevelID, List<long> lstBoundryID, long _UserID, long _RecordID)
        {
            bool MultipleLocationAssigned = false;
            try
            {
                MultipleLocationAssigned = db.ExtRepositoryFor<UserAdministrationRepository>().CheckMultipleLocationAlreadyAssigned(_LevelID, lstBoundryID, _UserID, _RecordID);
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return MultipleLocationAssigned;
        }

        public bool AssignLocation(long _UserID, long _LevelID, long _BoundryID, long? _DesignationID)
        {
            bool Saved = false;
            try
            {
                UA_AssociatedLocation ExistAssociation = db.ExtRepositoryFor<UserAdministrationRepository>().ExistingUserLocationsAssociations(_UserID, _LevelID, _BoundryID);

                if (ExistAssociation == null)
                {
                    UA_AssociatedLocation UserAssociation;
                    UserAssociation = new UA_AssociatedLocation();
                    UserAssociation.UserID = _UserID;
                    UserAssociation.IrrigationLevelID = _LevelID;
                    UserAssociation.IrrigationBoundryID = _BoundryID;
                    UserAssociation.DesignationID = _DesignationID;
                    db.Repository<UA_AssociatedLocation>().Insert(UserAssociation);
                    db.Save();
                    Saved = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return Saved;
        }

        public object GetUserDetail(long _ID, string _LevelName)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserDetail(_ID, _LevelName);
        }

        public bool DeleteLocation(long _RecordID, long _UserID)
        {
            try
            {
                UA_AssociatedLocation AssLoc = db.Repository<UA_AssociatedLocation>().FindById(_RecordID);
                if (AssLoc != null)
                {
                    UA_AssociatedLocationHistory AssLocHist = new UA_AssociatedLocationHistory();
                    AssLocHist.UserID = AssLoc.UserID;
                    AssLocHist.IrrigationLevelID = AssLoc.IrrigationLevelID;
                    AssLocHist.IrrigationBoundryID = AssLoc.IrrigationBoundryID;
                    AssLocHist.DesignationID = AssLoc.DesignationID;
                    AssLocHist.AssociatedLocationID = AssLoc.ID;
                    AssLocHist.CreatedDate = DateTime.Now;
                    AssLocHist.CreatedBy = _UserID;
                    AssLocHist.ModifiedDate = DateTime.Now;
                    AssLocHist.ModifiedBy = _UserID;

                    db.Repository<UA_AssociatedLocation>().Delete(AssLoc);
                    db.Repository<UA_AssociatedLocationHistory>().Insert(AssLocHist);
                    db.Save();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }

        public bool UpdateLocation(long _RecordID, long _LevelID, long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().UpdateLocation(_RecordID, _LevelID, _UserID);
        }

        public bool SaveLocation(List<UA_AssociatedLocation> _lstAssociatedLocation)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().SaveLocation(_lstAssociatedLocation);
        }

        public string GetRoleName(long _USerID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetRoleName(_USerID);
        }

        public bool? HasRights(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().HasRights(_UserID);
        }

        public bool AssociationExistAgainstLocation(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().AssociationExistAgainstLocation(_UserID);
        }

        public UA_AssociatedLocation GetUserAssociateLocation(long _UserID)
        {
            UA_AssociatedLocation qAssociatedLocation = (from asl in db.Repository<UA_AssociatedLocation>().GetAll()
                                                         where asl.UserID == _UserID
                                                         select asl).FirstOrDefault();
            return qAssociatedLocation;
        }

        #endregion

        #region Associate Barrage Channel and Oulets

        public object UserBasicInformation(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().UserBasicInformation(_UserID);
        }

        public List<object> UserInfo(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().UserInfo(_UserID);
        }

        public List<string> SectionUser(List<object> _lstInfo)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetLevelsFromSectionToZone(_lstInfo);
        }

        public List<string> SubDivisionUser(List<object> _LstInfo)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetLevelsFromSubDivisionToZone(_LstInfo);
        }

        public List<string> DivisionUser(List<object> _LstInfo)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetLevelsFromDivisionToZone(_LstInfo);
        }


        public List<string> CircleUser(List<object> _LstInfo)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetLevelsFromCircleToZone(_LstInfo);
        }

        public List<string> ZoneUser(List<object> _LstInfo)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetLevelsFromZoneToZone(_LstInfo);
        }

        public List<CO_Station> GetAllBarrages()
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetBarrages();
        }

        public List<object> GetExistingBarrageAssociation(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetExistingBarrageAssociations(_UserID).ToList();
        }

        public bool SaveData(UA_AssociatedStations _ObjSave)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().SaveData(_ObjSave);
        }

        public void DeleteData(long _RecordID)
        {
            db.Repository<UA_AssociatedStations>().Delete(_RecordID);
            db.Save();
        }

        public bool UpdateData(UA_AssociatedStations _ObjUpdate)
        {
            bool result = false;
            try
            {
                bool DataExist = db.ExtRepositoryFor<UserAdministrationRepository>().DataExist(_ObjUpdate);

                if (!DataExist)
                {
                    UA_AssociatedStations objstation = db.Repository<UA_AssociatedStations>().FindById(_ObjUpdate.ID);
                    objstation.UserID = _ObjUpdate.UserID;
                    objstation.StractureTypeID = _ObjUpdate.StractureTypeID;
                    objstation.StationID = _ObjUpdate.StationID;
                    objstation.StationSite = _ObjUpdate.StationSite;

                    if (_ObjUpdate.StationSite == "G" || _ObjUpdate.StationSite == "O")
                    {
                        objstation.GaugeOutlet = _ObjUpdate.GaugeOutlet;
                    }

                    db.Repository<UA_AssociatedStations>().Update(objstation);
                    db.Save();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return result;
        }

        public List<object> GetExisitngChannelAssociation(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetExisitngChannelAssociations(_UserID);
        }

        public List<object> GetUserCahnnels(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetUserCahnnels(_UserID);
        }

        public List<object> GetChannelRDs(long _ChannelID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetChannelRDs(_ChannelID);
        }

        public List<object> GetExistingOutletAssociations(long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetExistingOutlets(_UserID);
        }

        public List<object> GetChannelOutlets(long _ChannelID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetChannelOutlets(_ChannelID);
        }

        public object GetBarrageAssociationsRecord(long _RecordID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetBarrageAssociationsRecord(_RecordID);
        }

        public object GetChannelAssociationsRecord(long RecordID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetChannelAssociationsRecord(RecordID);
        }

        public object GetOutletAssociationsRecord(long _RecordID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetSelectedOutlet(_RecordID);
        }


        #endregion

        public List<object> GetNotificationLimit()
        {
            List<object> lstNotificationLimit = new List<object>();
            for (int i = 5; i <= 100; i = i + 5)
            {
                lstNotificationLimit.Add(new { Name = i, ID = i });
            }
            return lstNotificationLimit;
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
            List<UA_AssociatedLocation> lstAssociatedLocation = db.Repository<UA_AssociatedLocation>().GetAll().Where(al => al.UserID == _UserID).ToList();

            return lstAssociatedLocation;
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
            List<long> lstReportingDesignation = db.ExtRepositoryFor<UserAdministrationRepository>().GetReportingDesignations(_ManagerDesignationID);

            bool IsReporting = lstReportingDesignation.Any(d => d == _UserDesignationID);

            return IsReporting;
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
            List<UA_Users> lstUser = db.ExtRepositoryFor<UserAdministrationRepository>().GetUsers(_UserName, _FullName, _OrganizationID, _DesignationID,
                _RoleID, _Status, _ZoneID, _CircleID, _DivisionID, _SubDivisionID, _SectionID, _UserID, _UserLevelID, _UserRoleID);

            return lstUser;
        }

        #endregion
        public List<object> GetRegionsListByUser(long _UserID, int _IrrigationBoundaryID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetRegionsListByUser(_UserID, _IrrigationBoundaryID);
        }

        public List<object> GetSwitchUsers(long _UserID, DateTime _Now)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetSwitchUsers(_UserID, _Now);
        }


        public object GetUserManagerByLocationID(long _UserID)
        {
            long ManagerID = db.ExtRepositoryFor<UserAdministrationRepository>().GetUserManagerByLocationID(_UserID);
            UA_Users user = db.Repository<UA_Users>().FindById(ManagerID);

            return new { ID = user.ID, Name = user.FirstName + " " + user.LastName };
        }

        public List<UA_SystemParameters> GetSystemParameterValue(string _PKey)
        {
            if (_PKey != null)
            {
                return
                db.Repository<UA_SystemParameters>()
                    .GetAll()
                    .Where(a => a.ParameterKey == _PKey).ToList<UA_SystemParameters>();
            }
            else
            {

                return
                db.Repository<UA_SystemParameters>()
                    .GetAll().ToList<UA_SystemParameters>();
            }



        }



        public List<object> GetChannelRDsWhichAlreadyNotSaved(long _ChannelID, long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetChannelRDsWhichAlreadyNotSaved(_ChannelID, _UserID);
        }
        public List<object> GetChannelOutletsWhichAlreadyNotSaved(long _ChannelID, long _UserID)
        {
            return db.ExtRepositoryFor<UserAdministrationRepository>().GetListChannelOutletsWhichAlreadyNotSaved(_ChannelID, _UserID);
        }
    }
}
