using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.UserAdministration
{
    public class UserAdministrationRepository : Repository<UA_Users>
    {
        public UserAdministrationRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<UA_Users>();
        }
        private string GetUserStatus(bool? _Status)
        {
            return Convert.ToString(_Status == true ? Constants.UserStatus.Active : Constants.UserStatus.Inactive);
        }
        /// <summary>
        /// This function return list of those users does not have any role
        /// Created on 22-12-2015
        /// </summary>        
        /// <returns>List<object></returns>
        public List<object> GetUnAssignedRoleUsers()
        {
            List<object> lstUsers = (from u in context.UA_Users
                                     where (u.RoleID == null)
                                     select new { u.ID, u.FirstName, u.LastName, u.LoginName, u.IsActive, u.RoleID }).ToList()
                .Select(u => new
                {
                    ID = u.ID,
                    Name = u.FirstName + " " + u.LastName + " (" + u.LoginName + " ) - " + GetUserStatus(u.IsActive)
                }).ToList<object>();

            return lstUsers;
        }
        /// <summary>
        /// This function return users having role
        /// Created on 22-12-2015
        /// </summary>
        /// <param name="_RoleID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAssignedRoleUsers(long _RoleID = 0)
        {
            List<object> lstUsers = (from u in context.UA_Users
                                     where (u.RoleID == _RoleID || _RoleID == 0)
                                     select new { u.ID, u.FirstName, u.LastName, u.LoginName, u.IsActive, u.RoleID }).ToList()
                .Select(u => new
                {
                    ID = u.ID,
                    Name = u.FirstName + " " + u.LastName + " (" + u.LoginName + " ) - " + GetUserStatus(u.IsActive)
                }).ToList<object>();

            return lstUsers;
        }

        #region Associate Location To User


        //function returns irrigation level for any user
        public UA_IrrigationLevel GetUserLevel(long _UserID)
        {
            //UA_IrrigationLevel UserLevel = (from usr in context.UA_Users
            //                                join des in context.UA_Designations on usr.DesignationID equals des.ID
            //                                join irrLvl in context.UA_IrrigationLevel on des.IrrigationLevelID equals irrLvl.ID
            //                                where usr.ID == _UserID
            //                                select irrLvl).FirstOrDefault();

            UA_IrrigationLevel UserLevel = (from usr in context.UA_Users
                                            where usr.ID == _UserID
                                            select usr.UA_Designations.UA_IrrigationLevel).FirstOrDefault();
            return UserLevel;
        }

        public string GetLevelName(string Location, long? LocationID)
        {
            string Name = "";

            if (Location == "Zone")
            {
                Name = (from zon in context.CO_Zone
                        where zon.ID == LocationID
                        select zon.Name).FirstOrDefault();
            }
            else if (Location == "Circle")
            {
                Name = (from cir in context.CO_Circle
                        where cir.ID == LocationID
                        select cir.Name).FirstOrDefault();
            }
            else if (Location == "Division")
            {
                Name = (from div in context.CO_Division
                        where div.ID == LocationID
                        select div.Name).FirstOrDefault();
            }
            else if (Location == "Sub Division")
            {
                Name = (from sub in context.CO_SubDivision
                        where sub.ID == LocationID
                        select sub.Name).FirstOrDefault();
            }
            else if (Location == "Section")
            {
                Name = (from sec in context.CO_Section
                        where sec.ID == LocationID
                        select sec.Name).FirstOrDefault();
            }
            return Name;
        }

        public List<object> GetAssignedLevelNames(long UserID, long LevelID, string LevelName)
        {
            List<object> lstLevels = (from usr in context.UA_Users
                                      join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                                      where usr.ID == UserID && assLoc.IrrigationLevelID == LevelID
                                      select new
                                      {
                                          IrrBoundryID = assLoc.IrrigationBoundryID
                                      }
                                       ).ToList().
                                       Select(k => new { ID = k.IrrBoundryID, Name = GetLevelName(LevelName, k.IrrBoundryID) })
                                       .ToList<object>();
            return lstLevels;
        }

        public List<object> GetAssignedLevels(long _UserID, long _LevelID, string _LevelName)
        {
            List<object> lstAssLevels = new List<object>();
            object result = new object();
            List<object> lstRecordDetail = new List<object>();

            try
            {
                List<UA_AssociatedLocation> lstLevels = (from usr in context.UA_Users
                                                         join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                                                         where usr.ID == _UserID && assLoc.IrrigationLevelID == _LevelID
                                                         select assLoc).ToList();
                if (_LevelName == "Section")
                {
                    foreach (var lvl in lstLevels)
                    {

                        result = (from sec in context.CO_Section
                                  join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                  join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                  join cir in context.CO_Circle on div.CircleID equals cir.ID
                                  join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                  where sec.ID == lvl.IrrigationBoundryID
                                  select new
                                  {
                                      ID = lvl.ID,
                                      zoneName = zon.Name,
                                      zoneID = zon.ID,
                                      circleName = cir.Name,
                                      circleID = cir.ID,
                                      divisionName = div.Name,
                                      divisionID = div.ID,
                                      subDivisionName = subdiv.Name,
                                      subDivisionID = subdiv.ID,
                                      sectionName = sec.Name,
                                      sectioID = sec.ID
                                  }).FirstOrDefault();

                        lstRecordDetail.Add(result);
                    }
                }
                else if (_LevelName == "Sub Division")
                {
                    foreach (var lvl in lstLevels)
                    {
                        result = (from subdiv in context.CO_SubDivision
                                  join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                  join cir in context.CO_Circle on div.CircleID equals cir.ID
                                  join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                  where subdiv.ID == lvl.IrrigationBoundryID
                                  select new
                                  {
                                      ID = lvl.ID,
                                      zoneName = zon.Name,
                                      zoneID = zon.ID,
                                      circleName = cir.Name,
                                      circleID = cir.ID,
                                      divisionName = div.Name,
                                      divisionID = div.ID,
                                      subDivisionName = subdiv.Name,
                                      subDivisionID = subdiv.ID,
                                      sectionName = "-",
                                      sectioID = -1
                                  }).FirstOrDefault();
                        lstRecordDetail.Add(result);
                    }
                }
                else if (_LevelName == "Division")
                {
                    foreach (var lvl in lstLevels)
                    {
                        result = (from div in context.CO_Division
                                  join cir in context.CO_Circle on div.CircleID equals cir.ID
                                  join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                  where div.ID == lvl.IrrigationBoundryID
                                  select new
                                  {
                                      ID = lvl.ID,
                                      zoneName = zon.Name,
                                      zoneID = zon.ID,
                                      circleName = cir.Name,
                                      circleID = cir.ID,
                                      divisionName = div.Name,
                                      divisionID = div.ID,
                                      subDivisionName = "-",
                                      subDivisionID = -1,
                                      sectionName = "-",
                                      sectioID = -1
                                  }).FirstOrDefault();
                        lstRecordDetail.Add(result);
                    }
                }
                else if (_LevelName == "Circle")
                {
                    foreach (var lvl in lstLevels)
                    {

                        result = (from cir in context.CO_Circle
                                  join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                  where cir.ID == lvl.IrrigationBoundryID
                                  select new
                                  {
                                      ID = lvl.ID,
                                      zoneName = zon.Name,
                                      zoneID = zon.ID,
                                      circleName = cir.Name,
                                      circleID = cir.ID,
                                      divisionName = "-",
                                      divisionID = -1,
                                      subDivisionName = "-",
                                      subDivisionID = -1,
                                      sectionName = "-",
                                      sectioID = -1

                                  }).FirstOrDefault();
                        lstRecordDetail.Add(result);
                    }
                }
                else if (_LevelName == "Zone")
                {
                    foreach (var lvl in lstLevels)
                    {
                        result = (from zon in context.CO_Zone
                                  where zon.ID == lvl.IrrigationBoundryID
                                  select new
                                  {
                                      ID = lvl.ID,
                                      zoneName = zon.Name,
                                      zoneID = zon.ID,
                                      circleName = "-",
                                      circleID = -1,
                                      divisionName = "-",
                                      divisionID = -1,
                                      subDivisionName = "-",
                                      subDivisionID = -1,
                                      sectionName = "-",
                                      sectioID = -1
                                  }).FirstOrDefault();
                        lstRecordDetail.Add(result);
                    }
                }

                return lstRecordDetail;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return lstRecordDetail;
            }
        }

        public List<CO_Zone> GetUserZone(long _UserID, long _LevelID)
        {
            List<CO_Zone> lstZones = new List<CO_Zone>();
            List<UA_AssociatedLocation> lstZoneIDs = (from assLoc in context.UA_AssociatedLocation
                                                      where assLoc.UserID == _UserID && assLoc.IrrigationLevelID == _LevelID
                                                      select assLoc).ToList();
            CO_Zone zon;

            foreach (var irrBndryID in lstZoneIDs)
            {
                zon = new CO_Zone();

                zon = (from zo in context.CO_Zone
                       where zo.ID == irrBndryID.IrrigationBoundryID
                       select zo).FirstOrDefault();

                lstZones.Add(zon);
            }
            return lstZones;
        }

        public List<CO_Circle> GetUserCircles(long UserID, long LevelID)
        {
            List<CO_Circle> lstZones = new List<CO_Circle>();
            List<UA_AssociatedLocation> lstCircleIDs = (from assLoc in context.UA_AssociatedLocation
                                                        where assLoc.UserID == UserID && assLoc.IrrigationLevelID == LevelID
                                                        select assLoc).ToList();
            CO_Circle circle;

            foreach (var irrBndryID in lstCircleIDs)
            {
                circle = new CO_Circle();

                circle = (from cir in context.CO_Circle
                          where cir.ID == irrBndryID.IrrigationBoundryID
                          select cir).FirstOrDefault();

                lstZones.Add(circle);
            }
            return lstZones;
        }

        public List<CO_Zone> GetZonesRelatedToCircles(List<CO_Circle> LstCircle)
        {
            List<CO_Zone> lstZones = new List<CO_Zone>();
            CO_Zone zone;

            foreach (var cir in LstCircle.Select(c => c.ZoneID).Distinct().ToList())
            {
                zone = new CO_Zone();

                zone = (from zon in context.CO_Zone
                        where zon.ID == cir
                        select zon).FirstOrDefault();

                lstZones.Add(zone);
            }

            return lstZones;
        }


        public List<CO_Division> GetUserDivisions(long UserID, long LevelID)
        {
            List<CO_Division> lstZones = new List<CO_Division>();
            List<UA_AssociatedLocation> lstDivisionIDs = (from assLoc in context.UA_AssociatedLocation
                                                          where assLoc.UserID == UserID && assLoc.IrrigationLevelID == LevelID
                                                          select assLoc).ToList();
            CO_Division division;

            foreach (var irrBndryID in lstDivisionIDs)
            {
                division = new CO_Division();

                division = (from div in context.CO_Division
                            where div.ID == irrBndryID.IrrigationBoundryID
                            select div).FirstOrDefault();

                lstZones.Add(division);
            }
            return lstZones;
        }


        public List<CO_Circle> GetCirclesRelatedToDivisions(List<CO_Division> LstDivison)
        {
            List<CO_Circle> lstCircle = new List<CO_Circle>();
            CO_Circle circle;

            foreach (var div in LstDivison.Select(d => d.CircleID).Distinct().ToList())
            {
                circle = new CO_Circle();

                circle = (from cir in context.CO_Circle
                          where cir.ID == div.Value
                          select cir).FirstOrDefault();

                lstCircle.Add(circle);
            }
            return lstCircle;
        }


        public List<CO_SubDivision> GetUserSubDivisions(long UserID, long LevelID)
        {
            List<CO_SubDivision> lstSubDiv = new List<CO_SubDivision>();
            List<UA_AssociatedLocation> lstSubDivisionIDs = (from assLoc in context.UA_AssociatedLocation
                                                             where assLoc.UserID == UserID && assLoc.IrrigationLevelID == LevelID
                                                             select assLoc).ToList();
            CO_SubDivision subDivision;

            foreach (var irrBndryID in lstSubDivisionIDs)
            {
                subDivision = new CO_SubDivision();

                subDivision = (from div in context.CO_SubDivision
                               where div.ID == irrBndryID.IrrigationBoundryID
                               select div).FirstOrDefault();

                lstSubDiv.Add(subDivision);
            }
            return lstSubDiv;
        }


        public List<CO_Division> GetDivisionsRelatedToSubDivisions(List<CO_SubDivision> LstSubDivison)
        {
            List<CO_Division> lstDivision = new List<CO_Division>();
            CO_Division division;

            foreach (var sdiv in LstSubDivison.Select(s => s.DivisionID).Distinct().ToList())
            {
                division = new CO_Division();

                division = (from div in context.CO_Division
                            where div.ID == sdiv.Value
                            select div).FirstOrDefault();

                lstDivision.Add(division);
            }
            return lstDivision;
        }


        public List<CO_Section> GetUserSections(long UserID, long LevelID)
        {
            List<CO_Section> lstSection = new List<CO_Section>();
            List<UA_AssociatedLocation> lstSectionIDs = (from assLoc in context.UA_AssociatedLocation
                                                         where assLoc.UserID == UserID && assLoc.IrrigationLevelID == LevelID
                                                         select assLoc).ToList();
            CO_Section section;

            foreach (var irrBndryID in lstSectionIDs)
            {
                section = new CO_Section();

                section = (from sec in context.CO_Section
                           where sec.ID == irrBndryID.IrrigationBoundryID
                           select sec).FirstOrDefault();

                lstSection.Add(section);
            }
            return lstSection;
        }


        public List<CO_SubDivision> GetSubDivisionsRelatedToSection(List<CO_Section> LstSection)
        {
            List<CO_SubDivision> lstSubDivision = new List<CO_SubDivision>();
            CO_SubDivision subDivision;

            foreach (var sec in LstSection)
            {
                subDivision = new CO_SubDivision();

                subDivision = (from div in context.CO_SubDivision
                               where div.ID == sec.SubDivID
                               select div).FirstOrDefault();

                lstSubDivision.Add(subDivision);
            }
            return lstSubDivision;
        }


        public List<UA_AssociatedLocation> ExistingUserLocations(long UserID, long LevelID)
        {
            List<UA_AssociatedLocation> lstUserAssociations = (from assLoc in context.UA_AssociatedLocation
                                                               where assLoc.UserID == UserID && assLoc.IrrigationLevelID == LevelID
                                                               select assLoc).ToList();

            return lstUserAssociations;
        }


        public UA_AssociatedLocation ExistingUserLocationsAssociations(long UserID, long LevelID, long BoundryID)
        {
            UA_AssociatedLocation userAssociations = (from assLoc in context.UA_AssociatedLocation
                                                      where assLoc.UserID == UserID && assLoc.IrrigationLevelID == LevelID
                                                      && assLoc.IrrigationBoundryID == BoundryID
                                                      select assLoc).FirstOrDefault();

            return userAssociations;
        }

        public bool LocationAlreadyAssigned(long _LevelID, long _BoundryID, long _UserID)
        {
            bool locationAssigned = false;

            UA_AssociatedLocation userAssociations = (from assLoc in context.UA_AssociatedLocation
                                                      where assLoc.IrrigationLevelID == _LevelID
                                                      && assLoc.IrrigationBoundryID == _BoundryID
                                                      && assLoc.UserID == _UserID
                                                      select assLoc).FirstOrDefault();

            if (userAssociations != null)
            {
                locationAssigned = true;
            }

            return locationAssigned;
        }

        public bool LocationAlreadyAssignedUpdate(long _LevelID, long _BoundryID, long _UserID, long _RecordID)
        {
            bool locationAssigned = false;

            UA_AssociatedLocation userAssociations = (from assLoc in context.UA_AssociatedLocation
                                                      where assLoc.IrrigationLevelID == _LevelID
                                                      && assLoc.IrrigationBoundryID == _BoundryID
                                                      && assLoc.UserID == _UserID
                                                      && assLoc.ID != _RecordID
                                                      select assLoc).FirstOrDefault();

            if (userAssociations != null)
            {
                locationAssigned = true;
            }

            return locationAssigned;
        }
        public bool CheckMultipleLocationAlreadyAssigned(long _LevelID, List<long> lstBoundryID, long _UserID, long _RecordID)
        {
            bool locationAssigned = false;
            foreach (long BoudryID in lstBoundryID)
            {
                UA_AssociatedLocation userAssociations = (from assLoc in context.UA_AssociatedLocation
                    where assLoc.IrrigationLevelID == _LevelID
                          && assLoc.IrrigationBoundryID==BoudryID
                          && assLoc.UserID == _UserID
                          && assLoc.ID != _RecordID
                    select assLoc).FirstOrDefault();
                if (userAssociations != null)
                {
                    locationAssigned = true;
                    break;
                }
                else
                {
                    locationAssigned = false;
                    
                }
                
            }
            return locationAssigned;
        }

        public object GetUserDetail(long _ID, string _LevelName)
        {
            object result = new object();

            try
            {
                UA_AssociatedLocation objAssociation = (from assLoc in context.UA_AssociatedLocation
                                                        where assLoc.ID == _ID
                                                        select assLoc).FirstOrDefault();

                if (_LevelName == "Section")
                {
                    result = (from sec in context.CO_Section
                              join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                              join div in context.CO_Division on subdiv.DivisionID equals div.ID
                              join cir in context.CO_Circle on div.CircleID equals cir.ID
                              join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                              where sec.ID == objAssociation.IrrigationBoundryID
                              select new
                              {
                                  zoneName = zon.Name,
                                  zoneID = zon.ID,
                                  circleName = cir.Name,
                                  circleID = cir.ID,
                                  divisionName = div.Name,
                                  divisionID = div.ID,
                                  subDivisionName = subdiv.Name,
                                  subDivisionID = subdiv.ID,
                                  sectionName = sec.Name,
                                  sectioID = sec.ID
                              }).FirstOrDefault();
                }
                else if (_LevelName == "Sub Division")
                {
                    result = (from subdiv in context.CO_SubDivision
                              join div in context.CO_Division on subdiv.DivisionID equals div.ID
                              join cir in context.CO_Circle on div.CircleID equals cir.ID
                              join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                              where subdiv.ID == objAssociation.IrrigationBoundryID
                              select new
                              {
                                  zoneName = zon.Name,
                                  zoneID = zon.ID,
                                  circleName = cir.Name,
                                  circleID = cir.ID,
                                  divisionName = div.Name,
                                  divisionID = div.ID,
                                  subDivisionName = subdiv.Name,
                                  subDivisionID = subdiv.ID,
                              }).FirstOrDefault();
                }
                else if (_LevelName == "Division")
                {
                    result = (from div in context.CO_Division
                              join cir in context.CO_Circle on div.CircleID equals cir.ID
                              join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                              where div.ID == objAssociation.IrrigationBoundryID
                              select new
                              {
                                  zoneName = zon.Name,
                                  zoneID = zon.ID,
                                  circleName = cir.Name,
                                  circleID = cir.ID,
                                  divisionName = div.Name,
                                  divisionID = div.ID,
                              }).FirstOrDefault();
                }
                else if (_LevelName == "Circle")
                {
                    result = (from cir in context.CO_Circle
                              join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                              where cir.ID == objAssociation.IrrigationBoundryID
                              select new
                              {
                                  zoneName = zon.Name,
                                  zoneID = zon.ID,
                                  circleName = cir.Name,
                                  circleID = cir.ID,
                              }).FirstOrDefault();
                }
                else if (_LevelName == "Zone")
                {
                    result = (from zon in context.CO_Zone
                              where zon.ID == objAssociation.IrrigationBoundryID
                              select new
                              {
                                  zoneName = zon.Name,
                                  zoneID = zon.ID,
                              }).FirstOrDefault();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return result;
        }

        public object GetUserDetail(long _UserID)
        {
            object UserDetal = (from usr in context.UA_Users
                                join des in context.UA_Designations on usr.DesignationID equals des.ID
                                join rol in context.UA_Roles on usr.RoleID equals rol.ID
                                where usr.ID == _UserID
                                select
                                new
                                {
                                    FullName = usr.FirstName + " " + usr.LastName,
                                    UserName = usr.LoginName,
                                    Mobile = usr.MobilePhone,
                                    Designation = des.Name,
                                    AuthorityRights = des.AuthorityRights,
                                    RoleName = rol.Name,
                                    DesignationID = usr.DesignationID
                                }).FirstOrDefault();
            return UserDetal;
        }

        public bool UpdateLocation(long _RecordID, long _LevelID, long _UserID)
        {
            try
            {
                UA_AssociatedLocation objLocation = (from assLoc in context.UA_AssociatedLocation
                                                     where assLoc.ID == _RecordID
                                                     select assLoc).FirstOrDefault();
                if (objLocation != null)
                {
                    // History Work
                    UA_AssociatedLocationHistory AssLocHist = new UA_AssociatedLocationHistory();
                    AssLocHist.UserID = objLocation.UserID;
                    AssLocHist.IrrigationLevelID = objLocation.IrrigationLevelID;
                    AssLocHist.IrrigationBoundryID = objLocation.IrrigationBoundryID;
                    AssLocHist.IsActive = objLocation.IsActive;
                    AssLocHist.DesignationID = objLocation.DesignationID;
                    AssLocHist.AssociatedLocationID = objLocation.ID;
                    AssLocHist.CreatedDate = DateTime.Now;
                    AssLocHist.CreatedBy = _UserID;
                    AssLocHist.ModifiedDate = DateTime.Now;
                    AssLocHist.ModifiedBy = _UserID;
                    //
                    objLocation.IrrigationBoundryID = _LevelID;
                    context.UA_AssociatedLocationHistory.Add(AssLocHist);
                    context.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }
        public bool SaveLocation(List<UA_AssociatedLocation> _lstAssociatedLocation)
        {
            try
            {
                context.UA_AssociatedLocation.AddRange(_lstAssociatedLocation);
                context.SaveChanges();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return false;
            }
            return true;
        }
        public string GetRoleName(long _UserID)
        {
            string RoleName = "";
            try
            {
                RoleName = (from usr in context.UA_Users
                            join rol in context.UA_Roles on usr.RoleID equals rol.ID
                            where usr.ID == _UserID
                            select rol.Name).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return RoleName;
        }

        public bool? HasRights(long _UserID)
        {
            bool? Rights = false;
            try
            {
                Rights = (from usr in context.UA_Users
                          join des in context.UA_Designations on usr.DesignationID equals des.ID
                          where usr.ID == _UserID
                          select des.AuthorityRights).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Rights;
        }

        public bool AssociationExistAgainstLocation(long _UserID)
        {
            bool AssociationExist = false;
            try
            {
                List<UA_AssociatedStations> assStatn = (from statn in context.UA_AssociatedStations
                                                        where statn.UserID == _UserID
                                                        select statn).ToList();
                if (assStatn != null && assStatn.Count() > 0)
                    AssociationExist = true;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return AssociationExist;
        }

        #endregion


        #region Associate Barrages Channels and Outlets

        public object UserBasicInformation(long _UserID)
        {
            object info = new object();
            try
            {
                info = (from usr in context.UA_Users
                        join des in context.UA_Designations on usr.DesignationID equals des.ID
                        where usr.ID == _UserID
                        select new
                        {
                            FullName = usr.FirstName + " " + usr.LastName,
                            UserName = usr.LoginName,
                            Designation = des.Name
                        }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return info;
        }

        public List<object> UserInfo(long _UserID)
        {
            List<object> UserInfo = new List<object>();
            try
            {
                UserInfo = (from usr in context.UA_Users
                            join des in context.UA_Designations on usr.DesignationID equals des.ID
                            join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                            join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                            where usr.ID == _UserID
                            select new
                            {
                                FullName = usr.FirstName + " " + usr.LastName,
                                UserName = usr.LoginName,
                                DesigName = des.Name,
                                IrrLvlID = assLoc.IrrigationLevelID,
                                IrrLvlName = irrLvl.Name,
                                IrrBndryID = assLoc.IrrigationBoundryID,
                                Rights = des.AuthorityRights
                            }
                            ).ToList<object>();

                return UserInfo;
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
                return UserInfo;
            }
        }

        //this function return the section , div, sub Div, cir zone related to user in comma separated manner
        // function is called when user has level name Sub Division
        public List<string> GetLevelsFromSubDivisionToZone(List<object> _LstInfo)
        {
            List<string> lstLevels = new List<string>();

            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");

            try
            {
                foreach (var level in _LstInfo)
                {
                    long BoundryID = Convert.ToInt64(level.GetType().GetProperty("IrrBndryID").GetValue(level));

                    var Result = (from sDiv in context.CO_SubDivision
                                  join div in context.CO_Division on sDiv.DivisionID equals div.ID
                                  join cir in context.CO_Circle on div.CircleID equals cir.ID
                                  join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                  where sDiv.ID == BoundryID
                                  select new
                                  {
                                      SubDiv = sDiv.Name,
                                      Division = div.Name,
                                      Circle = cir.Name,
                                      Zone = zon.Name
                                  }
                                   ).FirstOrDefault();

                    if (Result != null)
                    {
                        if (lstLevels[0] == "")
                        {
                            lstLevels[0] = Result.Zone;
                        }
                        else
                        {
                            bool res = lstLevels[0].Contains(Result.Zone);
                            if (!res)
                                lstLevels[0] = lstLevels[0] + ", " + Result.Zone;
                        }

                        if (lstLevels[1] == "")
                        {
                            lstLevels[1] = Result.Circle;
                        }
                        else
                        {
                            bool res = lstLevels[1].Contains(Result.Circle);
                            if (!res)
                                lstLevels[1] = lstLevels[1] + ", " + Result.Circle;
                        }

                        if (lstLevels[2] == "")
                        {
                            lstLevels[2] = Result.Division;
                        }
                        else
                        {
                            bool res = lstLevels[2].Contains(Result.Division);
                            if (!res)
                                lstLevels[2] = lstLevels[2] + ", " + Result.Division;
                        }

                        if (lstLevels[3] == "")
                        {
                            lstLevels[3] = Result.SubDiv;
                        }
                        else
                        {
                            bool res = lstLevels[3].Contains(Result.SubDiv);
                            if (!res)
                                lstLevels[3] = lstLevels[3] + ", " + Result.SubDiv;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstLevels;
        }


        //this function return the section , div, sub Div, cir zone related to user in comma separated manner
        // function is called when user has level name Section
        public List<string> GetLevelsFromSectionToZone(List<object> _lstInfo)
        {
            List<string> lstLevels = new List<string>();

            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");

            try
            {
                foreach (var level in _lstInfo)
                {
                    long BoundryID = Convert.ToInt64(level.GetType().GetProperty("IrrBndryID").GetValue(level));

                    var Result = (from sec in context.CO_Section
                                  join sDiv in context.CO_SubDivision on sec.SubDivID equals sDiv.ID
                                  join div in context.CO_Division on sDiv.DivisionID equals div.ID
                                  join cir in context.CO_Circle on div.CircleID equals cir.ID
                                  join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                  where sec.ID == BoundryID
                                  select new
                                  {
                                      Section = sec.Name,
                                      SubDivision = sDiv.Name,
                                      Division = div.Name,
                                      Circle = cir.Name,
                                      Zone = zon.Name
                                  }
                                   ).FirstOrDefault();

                    if (Result != null)
                    {
                        if (lstLevels[0] == "")
                        {
                            lstLevels[0] = Result.Zone;
                        }
                        else
                        {
                            bool res = lstLevels[0].Contains(Result.Zone);
                            if (!res)
                                lstLevels[0] = lstLevels[0] + ", " + Result.Zone;
                        }

                        if (lstLevels[1] == "")
                        {
                            lstLevels[1] = Result.Circle;
                        }
                        else
                        {
                            bool res = lstLevels[1].Contains(Result.Circle);
                            if (!res)
                                lstLevels[1] = lstLevels[1] + ", " + Result.Circle;
                        }

                        if (lstLevels[2] == "")
                        {
                            lstLevels[2] = Result.Division;
                        }
                        else
                        {
                            bool res = lstLevels[2].Contains(Result.Division);
                            if (!res)
                                lstLevels[2] = lstLevels[2] + ", " + Result.Division;
                        }

                        if (lstLevels[3] == "")
                        {
                            lstLevels[3] = Result.SubDivision;
                        }
                        else
                        {
                            bool res = lstLevels[3].Contains(Result.SubDivision);
                            if (!res)
                                lstLevels[3] = lstLevels[3] + ", " + Result.SubDivision;
                        }

                        if (lstLevels[4] == "")
                        {
                            lstLevels[4] = Result.Section;
                        }
                        else
                        {
                            bool res = lstLevels[4].Contains(Result.Section);
                            if (!res)
                                lstLevels[4] = lstLevels[4] + ", " + Result.Section;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstLevels;
        }


        //this function return the section , div, sub Div, cir zone related to user in comma separated manner
        // function is called when user has level name Division
        public List<string> GetLevelsFromDivisionToZone(List<object> _LstInfo)
        {
            List<string> lstLevels = new List<string>();

            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");

            try
            {
                foreach (var level in _LstInfo)
                {
                    long BoundryID = Convert.ToInt64(level.GetType().GetProperty("IrrBndryID").GetValue(level));

                    var Result = (from div in context.CO_Division
                                  join cir in context.CO_Circle on div.CircleID equals cir.ID
                                  join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                  where div.ID == BoundryID
                                  select new
                                  {
                                      Division = div.Name,
                                      Circle = cir.Name,
                                      Zone = zon.Name
                                  }
                                   ).FirstOrDefault();

                    if (Result != null)
                    {
                        if (lstLevels[0] == "")
                        {
                            lstLevels[0] = Result.Zone;
                        }
                        else
                        {
                            bool res = lstLevels[0].Contains(Result.Zone);
                            if (!res)
                                lstLevels[0] = lstLevels[0] + ", " + Result.Zone;
                        }

                        if (lstLevels[1] == "")
                        {
                            lstLevels[1] = Result.Circle;
                        }
                        else
                        {
                            bool res = lstLevels[1].Contains(Result.Circle);
                            if (!res)
                                lstLevels[1] = lstLevels[1] + ", " + Result.Circle;
                        }

                        if (lstLevels[2] == "")
                        {
                            lstLevels[2] = Result.Division;
                        }
                        else
                        {
                            bool res = lstLevels[2].Contains(Result.Division);
                            if (!res)
                                lstLevels[2] = lstLevels[2] + ", " + Result.Division;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstLevels;
        }


        //this function return the section , div, sub Div, cir zone related to user in comma separated manner
        // function is called when user has level name Circle
        public List<string> GetLevelsFromCircleToZone(List<object> _LstInfo)
        {
            List<string> lstLevels = new List<string>();

            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");

            try
            {
                foreach (var level in _LstInfo)
                {
                    long BoundryID = Convert.ToInt64(level.GetType().GetProperty("IrrBndryID").GetValue(level));

                    var Result = (from cir in context.CO_Circle
                                  join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                  where cir.ID == BoundryID
                                  select new
                                  {
                                      Circle = cir.Name,
                                      Zone = zon.Name
                                  }
                                   ).FirstOrDefault();

                    if (Result != null)
                    {
                        if (lstLevels[0] == "")
                        {
                            lstLevels[0] = Result.Zone;
                        }
                        else
                        {
                            bool res = lstLevels[0].Contains(Result.Zone);
                            if (!res)
                                lstLevels[0] = lstLevels[0] + ", " + Result.Zone;
                        }

                        if (lstLevels[1] == "")
                        {
                            lstLevels[1] = Result.Circle;
                        }
                        else
                        {
                            bool res = lstLevels[1].Contains(Result.Circle);
                            if (!res)
                                lstLevels[1] = lstLevels[1] + ", " + Result.Circle;
                        }

                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstLevels;
        }

        //this function return the section , div, sub Div, cir zone related to user in comma separated manner
        // function is called when user has level name Circle
        public List<string> GetLevelsFromZoneToZone(List<object> _LstInfo)
        {
            List<string> lstLevels = new List<string>();

            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");
            lstLevels.Add("");

            try
            {
                foreach (var level in _LstInfo)
                {
                    long BoundryID = Convert.ToInt64(level.GetType().GetProperty("IrrBndryID").GetValue(level));

                    var Result = (from zon in context.CO_Zone
                                  where zon.ID == BoundryID
                                  select new
                                  {
                                      Zone = zon.Name
                                  }
                                   ).FirstOrDefault();

                    if (Result != null)
                    {
                        if (lstLevels[0] == "")
                        {
                            lstLevels[0] = Result.Zone;
                        }
                        else
                        {
                            bool res = lstLevels[0].Contains(Result.Zone);
                            if (!res)
                                lstLevels[0] = lstLevels[0] + ", " + Result.Zone;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstLevels;
        }

        // function return all the barrages 
        public List<CO_Station> GetBarrages()
        {
            List<CO_Station> lstBarrages = new List<CO_Station>();
            try
            {
                lstBarrages = (from statn in context.CO_Station
                               join strTyp in context.CO_StructureType on statn.StructureTypeID equals strTyp.ID
                               where strTyp.Name == "Barrage"
                               orderby statn.Name
                               select statn).ToList();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstBarrages;
        }

        public string SiteName(string Name)
        {
            string fName = Constants.DownStream;

            if (Name == "U")
                fName = Constants.UpStream;

            return fName;
        }

        //function return existing associations of barrage with user 
        public List<object> GetExistingBarrageAssociations(long _UserID)
        {
            List<object> lstAssBarrages = new List<object>();
            try
            {
                lstAssBarrages = (from assStatn in context.UA_AssociatedStations
                                  join statn in context.CO_Station on assStatn.StationID equals statn.ID
                                  where assStatn.UserID == _UserID && assStatn.StractureTypeID == 1  // 1 is the id of barrage
                                  select new
                                  {
                                      ID = assStatn.ID,
                                      Name = statn.Name,
                                      StrTypeID = assStatn.StractureTypeID,
                                      StationSite = assStatn.StationSite
                                  }
                                  ).ToList()
                                  .Select(s => new { s.ID, s.Name, StationSite = SiteName(s.StationSite) }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstAssBarrages;
        }

        public object GetBarrageAssociationsRecord(long _RecordID)
        {
            object lstAssBarrages = new object();
            try
            {
                lstAssBarrages = (from assStatn in context.UA_AssociatedStations
                                  join statn in context.CO_Station on assStatn.StationID equals statn.ID
                                  where assStatn.ID == _RecordID
                                  select new
                                  {
                                      ID = assStatn.ID,
                                      Name = statn.Name,
                                      StrTypeID = assStatn.StractureTypeID,
                                      StationSite = assStatn.StationSite
                                  }
                                  ).ToList()
                                  .Select(s => new { s.ID, s.Name, StationSite = SiteName(s.StationSite) }).FirstOrDefault();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstAssBarrages;
        }

        public bool DataExist(UA_AssociatedStations objSave)
        {
            bool result = false;
            try
            {
                UA_AssociatedStations objResult = (from assStn in context.UA_AssociatedStations
                                                   where assStn.UserID == objSave.UserID && assStn.StractureTypeID == objSave.StractureTypeID
                                                   && assStn.StationID == objSave.StationID && assStn.StationSite == objSave.StationSite
                                                   && assStn.GaugeOutlet == objSave.GaugeOutlet
                                                   && assStn.ID != objSave.ID
                                                   select assStn).FirstOrDefault();

                if (objResult != null && objResult.ID != null)
                {
                    result = true;
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return result;
        }

        public bool SaveData(UA_AssociatedStations _ObjSave)
        {
            bool result = true;
            try
            {
                UA_AssociatedStations objResult = (from assStn in context.UA_AssociatedStations
                                                   where assStn.UserID == _ObjSave.UserID && assStn.StractureTypeID == _ObjSave.StractureTypeID
                                                   && assStn.StationID == _ObjSave.StationID && assStn.StationSite == _ObjSave.StationSite
                                                   && assStn.GaugeOutlet == _ObjSave.GaugeOutlet
                                                   select assStn).FirstOrDefault();

                if (objResult != null && objResult.ID != null)
                {
                    result = false;
                }
                else
                {
                    context.UA_AssociatedStations.Add(_ObjSave);
                    context.SaveChanges();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return result;
        }

        public string GetRDs(long? ID)
        {
            string GuageRD = "";
            try
            {
                CO_ChannelGauge objGuage = (from chnl in context.CO_ChannelGauge
                                            where chnl.ID == ID
                                            select chnl).FirstOrDefault();

                if (objGuage != null && objGuage.ID != null)
                {
                    int remainder = Convert.ToInt32(objGuage.GaugeAtRD % 1000);
                    int quotient = Convert.ToInt32((objGuage.GaugeAtRD - remainder) / 1000);
                    //GuageRD = "RD " + quotient.ToString() + "+" + Calculations.GetRDText(objGuage.GaugeAtRD);
                    GuageRD = "RD " + Calculations.GetRDText(objGuage.GaugeAtRD);
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return GuageRD;
        }

        public List<object> GetExisitngChannelAssociations(long _UserID)
        {
            List<object> lstAssChannels = new List<object>();
            try
            {
                lstAssChannels = (from assStatn in context.UA_AssociatedStations
                                  join statn in context.CO_Channel on assStatn.StationID equals statn.ID
                                  where assStatn.UserID == _UserID && assStatn.StractureTypeID == 6  // 6 is the id of channel
                                  && assStatn.StationSite == "G"
                                  select new
                                  {
                                      ID = assStatn.ID,
                                      Name = statn.NAME,
                                      StrTypeID = assStatn.StractureTypeID,
                                      StationSite = assStatn.StationSite,
                                      GuageID = assStatn.GaugeOutlet
                                  }
                                  ).ToList()
                                  .Select(s => new { s.ID, s.Name, GuageRD = GetRDs(s.GuageID) }).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstAssChannels;
        }

        public object GetChannelAssociationsRecord(long RecordID)
        {
            object lstAssChannels = new object();
            try
            {
                lstAssChannels = (from assStatn in context.UA_AssociatedStations
                                  join statn in context.CO_Channel on assStatn.StationID equals statn.ID
                                  where assStatn.ID == RecordID && assStatn.StationSite == "G"
                                  select new
                                  {
                                      ID = assStatn.ID,
                                      Name = statn.NAME,
                                      ChnlID = statn.ID,
                                      StrTypeID = assStatn.StractureTypeID,
                                      StationSite = assStatn.StationSite,
                                      GuageID = assStatn.GaugeOutlet
                                  }
                                  ).ToList()
                                  .Select(s => new { s.ID, s.Name, s.ChnlID, GuageRD = GetRDs(s.GuageID) }).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstAssChannels;
        }

        public List<object> GetUserCahnnels(long _UserID)
        {
            List<object> lstAssChannels = new List<object>();
            List<long> lstSectionIDs = new List<long>();
            try
            {
                //List<UA_AssociatedLocation> lstResult = (from usr in context.UA_Users
                //                                         join des in context.UA_Designations on usr.DesignationID equals des.ID
                //                                         join irrlvl in context.UA_IrrigationLevel on des.IrrigationLevelID equals irrlvl.ID
                //                                         join assloc in context.UA_AssociatedLocation on usr.ID equals assloc.UserID
                //                                         where usr.ID == _UserID && assloc.IrrigationLevelID == irrlvl.ID
                //                                         select assloc).ToList();

                List<UA_AssociatedLocation> lstResult = (from assloc in context.UA_AssociatedLocation
                                                         where assloc.UserID == _UserID && assloc.IrrigationLevelID == assloc.IrrigationLevelID
                                                         select assloc).ToList();

                foreach (var res in lstResult)
                {
                    if (res.IrrigationLevelID == 4)
                    {
                        var result = (from sDiv in context.CO_SubDivision
                                      join sec in context.CO_Section on sDiv.ID equals sec.SubDivID
                                      where sDiv.ID == res.IrrigationBoundryID
                                      select sec.ID).ToList();

                        foreach (var secID in result)
                        {
                            lstSectionIDs.Add(secID);
                        }
                    }
                    else if (res.IrrigationLevelID == 3)
                    {
                        var result = (from div in context.CO_Division
                                      join sDiv in context.CO_SubDivision on div.ID equals sDiv.DivisionID
                                      join sec in context.CO_Section on sDiv.ID equals sec.SubDivID
                                      where div.ID == res.IrrigationBoundryID
                                      select sec.ID).ToList();

                        foreach (var secID in result)
                        {
                            lstSectionIDs.Add(secID);
                        }
                    }
                    else if (res.IrrigationLevelID == 2)
                    {
                        var result = (from cir in context.CO_Circle
                                      join div in context.CO_Division on cir.ID equals div.CircleID
                                      join sDiv in context.CO_SubDivision on div.ID equals sDiv.DivisionID
                                      join sec in context.CO_Section on sDiv.ID equals sec.SubDivID
                                      where cir.ID == res.IrrigationBoundryID
                                      select sec.ID).ToList();

                        foreach (var secID in result)
                        {
                            lstSectionIDs.Add(secID);
                        }
                    }
                    else if (res.IrrigationLevelID == 1)
                    {
                        var result = (from zon in context.CO_Zone
                                      join cir in context.CO_Circle on zon.ID equals cir.ZoneID
                                      join div in context.CO_Division on cir.ID equals div.CircleID
                                      join sDiv in context.CO_SubDivision on div.ID equals sDiv.DivisionID
                                      join sec in context.CO_Section on sDiv.ID equals sec.SubDivID
                                      where zon.ID == res.IrrigationBoundryID
                                      select sec.ID).ToList();

                        foreach (var secID in result)
                        {
                            lstSectionIDs.Add(secID);
                        }
                    }
                    else
                    {
                        lstSectionIDs.Add(Convert.ToInt64(res.IrrigationBoundryID));
                    }
                }

                lstAssChannels.Add(new
                {
                    ID = "",
                    Name = "Select"
                });

                foreach (var secID in lstSectionIDs.Distinct())
                {
                    List<object> lstIrrBndry = (from chnlIrr in context.CO_ChannelIrrigationBoundaries
                                                join chnl in context.CO_Channel on chnlIrr.ChannelID equals chnl.ID
                                                where chnlIrr.SectionID == secID
                                                select new
                                                {
                                                    ID = chnl.ID,
                                                    Name = chnl.NAME
                                                }
                                                ).Distinct().ToList<object>();

                    foreach (var v in lstIrrBndry)
                    {
                        lstAssChannels.Add(v);
                    }

                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }

            return lstAssChannels;
        }
        public List<object> GetChannelRDsWhichAlreadyNotSaved(long _ChannelID,long _UserID)
        {
            List<object> lstRDs = new List<object>();
            try
            {
                var lstGuageRds = (from chnlGage in context.CO_ChannelGauge
                                   where chnlGage.ChannelID == _ChannelID && !(from assLocation in context.UA_AssociatedStations where assLocation.UserID == _UserID && assLocation.StationSite == "G" select assLocation.GaugeOutlet).Contains(chnlGage.ID)
                                   select chnlGage).OrderBy(x=>x.GaugeAtRD).ToList();
                foreach (var rd in lstGuageRds)
                {
                    long remainder = Convert.ToInt64(rd.GaugeAtRD % 1000);
                    long quotient = Convert.ToInt64((rd.GaugeAtRD - remainder) / 1000);
                    string GuageRD = "RD " + Calculations.GetRDText(rd.GaugeAtRD);
                    object objRD = new
                    {
                        ID = rd.ID,
                        Name = GuageRD
                    };
                    lstRDs.Add(objRD);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstRDs;
        }
        public List<object> GetChannelRDs(long _ChannelID)
        {
            List<object> lstRDs = new List<object>();

            try
            {
                var lstGuageRds = (from chnlGage in context.CO_ChannelGauge
                                   where chnlGage.ChannelID == _ChannelID
                                   select chnlGage).OrderBy(x=>x.GaugeAtRD).ToList();
               

                foreach (var rd in lstGuageRds)
                {
                    long remainder = Convert.ToInt64(rd.GaugeAtRD % 1000);
                    long quotient = Convert.ToInt64((rd.GaugeAtRD - remainder) / 1000);

                    //string GuageRD = "RD " + quotient.ToString() + "+" + remainder.ToString();
                    string GuageRD = "RD " + Calculations.GetRDText(rd.GaugeAtRD);

                    object objRD = new
                    {
                        ID = rd.ID,
                        Name = GuageRD
                    };

                    lstRDs.Add(objRD);
                }
            }
                
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstRDs;
        }

        public string GetOutletName(long? ID)
        {
            string Name = "";
            try
            {
                CO_ChannelOutlets objChnl = (from chnlOlt in context.CO_ChannelOutlets
                                             where chnlOlt.ID == ID
                                             select chnlOlt).FirstOrDefault();

                if (objChnl != null && objChnl.ID != null)
                {
                    Name = Calculations.GetRDText(objChnl.OutletRD) + " " + objChnl.ChannelSide.ToString();
                }

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return Name;
        }

        public List<object> GetExistingOutlets(long _UserID)
        {
            List<object> lstAssOutlets = new List<object>();
            try
            {
                lstAssOutlets = (from assStatn in context.UA_AssociatedStations
                                 join statn in context.CO_Channel on assStatn.StationID equals statn.ID
                                 where assStatn.UserID == _UserID && assStatn.StractureTypeID == 6  // 6 is the id of channel
                                 && assStatn.StationSite == "O"
                                 select new
                                 {
                                     ID = assStatn.ID,
                                     Name = statn.NAME,
                                     StrTypeID = assStatn.StractureTypeID,
                                     StationSite = assStatn.StationSite,
                                     GuageID = assStatn.GaugeOutlet
                                 }
                                  ).ToList()
                                  .Select(s => new { s.ID, s.Name, Outlet = GetOutletName(s.GuageID) }).ToList<object>();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstAssOutlets;
        }

        public object GetSelectedOutlet(long _ID)
        {
            object objAssOutlets = new object();
            try
            {
                objAssOutlets = (from assStatn in context.UA_AssociatedStations
                                 join statn in context.CO_Channel on assStatn.StationID equals statn.ID
                                 where assStatn.ID == _ID
                                 select new
                                 {
                                     ID = assStatn.ID,
                                     Name = statn.NAME,
                                     ChnlID = statn.ID,
                                     StrTypeID = assStatn.StractureTypeID,
                                     StationSite = assStatn.StationSite,
                                     GuageID = assStatn.GaugeOutlet
                                 }
                                  ).ToList()
                                  .Select(s => new { s.ID, s.Name, s.ChnlID, Outlet = GetOutletName(s.GuageID) }).FirstOrDefault();

            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return objAssOutlets;
        }



        //db.Repository<UA_AssociatedStations>().FindById(_ObjUpdate.ID);
        public List<object> GetListChannelOutletsWhichAlreadyNotSaved(long _ChannelID, long _UserID)
        {
            List<CO_ChannelOutlets> lstOutlets = new List<CO_ChannelOutlets>();
            List<object> lstOutletNames = new List<object>();

            try
            {
                lstOutlets = (from chnlOutlet in context.CO_ChannelOutlets
                              where chnlOutlet.ChannelID == _ChannelID && !(from assLocation in context.UA_AssociatedStations where assLocation.UserID == _UserID && assLocation.StationSite == "O" select assLocation.GaugeOutlet).Contains(chnlOutlet.ID)
                              select chnlOutlet).ToList();

                foreach (var outlet in lstOutlets)
                {

                    int remainder = Convert.ToInt32(outlet.OutletRD % 1000);
                    int quotient = Convert.ToInt32((outlet.OutletRD - remainder) / 1000);
                    object obj = new
                    {
                        ID = outlet.ID,
                        Name = quotient.ToString() + "+" + remainder.ToString("000") + " " + outlet.ChannelSide.ToString()
                    };

                    lstOutletNames.Add(obj);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstOutletNames;
        }










        public List<object> GetChannelOutlets(long _ChannelID)
        {
            List<CO_ChannelOutlets> lstOutlets = new List<CO_ChannelOutlets>();
            List<object> lstOutletNames = new List<object>();

            try
            {
                lstOutlets = (from chnlOutlet in context.CO_ChannelOutlets
                              where chnlOutlet.ChannelID == _ChannelID
                              select chnlOutlet).ToList();

                foreach (var outlet in lstOutlets)
                {
                    int remainder = Convert.ToInt32(outlet.OutletRD % 1000);
                    int quotient = Convert.ToInt32((outlet.OutletRD - remainder) / 1000);
                    object obj = new
                         {
                             ID = outlet.ID,
                             Name = quotient.ToString() + "+" + remainder.ToString("000") + " " + outlet.ChannelSide.ToString()
                         };

                    lstOutletNames.Add(obj);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstOutletNames;
        }

        public void GetAllSections(long _IrrigationLevelID, long _IrrigationBoundryID)
        {
            try
            {
                if (_IrrigationLevelID == 1)  // Zone
                {
                    List<long> lstcircleIDs = (from cir in context.CO_Circle
                                               where cir.ZoneID == _IrrigationBoundryID
                                               select cir.ID).ToList();

                    List<long> lstDivIDs = new List<long>();

                    foreach (var cirID in lstcircleIDs)
                    {
                        List<long> lstDiv = (from div in context.CO_Division
                                             where div.CircleID == cirID
                                             select div.ID).ToList();

                        lstDivIDs.AddRange(lstDiv);  // to check how it works
                    }

                    List<long> lstSubDivIDs = new List<long>();

                    foreach (var divID in lstDivIDs)
                    {
                        List<long> lstSubDiv = (from subDiv in context.CO_SubDivision
                                                where subDiv.DivisionID == divID
                                                select subDiv.ID).ToList();

                        lstSubDivIDs.AddRange(lstSubDiv);
                    }

                    List<long> lstSectionIDs = new List<long>();

                    foreach (var subDivID in lstSubDivIDs)
                    {
                        List<long> lstSections = (from sec in context.CO_Section
                                                  where sec.SubDivID == subDivID
                                                  select sec.ID).ToList();

                        lstSectionIDs.AddRange(lstSections);
                    }
                }  // zone end
                else if (_IrrigationLevelID == 2)  // circle 
                {
                    List<long> lstDivIDs = (from div in context.CO_Division
                                            where div.CircleID == _IrrigationBoundryID
                                            select div.ID).ToList();


                    List<long> lstSubDivIDs = new List<long>();

                    foreach (var divID in lstDivIDs)
                    {
                        List<long> lstSubDiv = (from subDiv in context.CO_SubDivision
                                                where subDiv.DivisionID == divID
                                                select subDiv.ID).ToList();

                        lstSubDivIDs.AddRange(lstSubDiv);
                    }

                    List<long> lstSectionIDs = new List<long>();

                    foreach (var subDivID in lstSubDivIDs)
                    {
                        List<long> lstSections = (from sec in context.CO_Section
                                                  where sec.SubDivID == subDivID
                                                  select sec.ID).ToList();

                        lstSectionIDs.AddRange(lstSections);
                    }
                }  // cir end
                else if (_IrrigationLevelID == 3)  // Division
                {
                    List<long> lstSubDivIDs = (from subDiv in context.CO_SubDivision
                                               where subDiv.DivisionID == _IrrigationBoundryID
                                               select subDiv.ID).ToList();

                    List<long> lstSectionIDs = new List<long>();

                    foreach (var subDivID in lstSubDivIDs)
                    {
                        List<long> lstSections = (from sec in context.CO_Section
                                                  where sec.SubDivID == subDivID
                                                  select sec.ID).ToList();

                        lstSectionIDs.AddRange(lstSections);
                    }
                } // Division end
                else if (_IrrigationLevelID == 4)  // sub div
                {

                    List<long> lstSectionsIDs = (from sec in context.CO_Section
                                                 where sec.SubDivID == _IrrigationBoundryID
                                                 select sec.ID).ToList();
                }
                else if (_IrrigationLevelID == 5)
                {
                    List<long> lstSectionsIDs = new List<long>();
                    lstSectionsIDs.Add(_IrrigationBoundryID);
                }
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion


        /// <summary>
        /// This function gets the user information based on the inputted string
        /// Created On: 04-01-2016
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetUserInfo(string _Name, long _ManagerID)
        {
            if (_Name != String.Empty)
            {
                List<UA_Users> lstUser = null;

                UA_Users mdlUsers = (from u in context.UA_Users
                                     where u.ID == _ManagerID
                                     select u).FirstOrDefault();

                if (mdlUsers.RoleID != Constants.AdministratorRoleID)
                {
                    List<long> lstReportingUsers = GetReportingUsers(_ManagerID);

                    lstUser = (from usr in context.UA_Users
                               where lstReportingUsers.Contains(usr.ID)
                               select usr).ToList<UA_Users>();
                }
                else
                {
                    lstUser = (from usr in context.UA_Users
                               where usr.ID != _ManagerID && usr.DesignationID != null && usr.RoleID != null
                               select usr).ToList<UA_Users>();
                }

                lstUser = lstUser.Where(usr => (String.Format("{0} {1}", usr.FirstName, usr.LastName).ToUpper().Contains(_Name.ToUpper())) || _Name == "*").ToList();

                List<dynamic> lstUserInfo = new List<dynamic>();

                foreach (UA_Users mdlUser in lstUser)
                {
                    List<UA_AssociatedLocation> lstAssociatedLocation = (from asl in context.UA_AssociatedLocation
                                                                         where asl.UserID == mdlUser.ID
                                                                         select asl).ToList();

                    StringBuilder Location = new StringBuilder();

                    foreach (UA_AssociatedLocation mdlAssociatedLocation in lstAssociatedLocation)
                    {
                        Location.AppendLine(String.Format("{0}: {1},", mdlAssociatedLocation.UA_IrrigationLevel.Name, GetLevelName(mdlAssociatedLocation.UA_IrrigationLevel.Name, mdlAssociatedLocation.IrrigationBoundryID)));
                    }

                    if (lstAssociatedLocation.Count() > 0)
                    {
                        Location = Location.Remove(Location.ToString().LastIndexOf(','), 1);
                    }

                    lstUserInfo.Add(new
                    {
                        ID = mdlUser.ID,
                        UserName = mdlUser.LoginName,
                        FullName = String.Format("{0} {1}", mdlUser.FirstName, mdlUser.LastName),
                        Designation = mdlUser.UA_Designations.Name,
                        DesignationID = mdlUser.DesignationID,
                        Location = Location.ToString()
                    });
                }

                return lstUserInfo;
            }
            else
            {
                return new List<dynamic>();
            }
        }

        /// <summary>
        /// This function return list of Assigned Temporary Roles.
        /// Created On 07-01-2016
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_AssignedUserID"></param>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetActingRoles(long _UserID, long _AssignedUserID, DateTime? _FromDate, DateTime? _ToDate, UA_Users _LoginUser)
        {
            List<UA_ActingRoles> lstActingRole = null;

            if (_LoginUser.RoleID != Constants.AdministratorRoleID)
            {
                List<long> lstReportingUsers = GetReportingUsers(_LoginUser.ID);

                lstActingRole = (from acr in context.UA_ActingRoles
                                 where lstReportingUsers.Contains(acr.UserID) && lstReportingUsers.Contains((long)acr.AssignedUserID)
                                 select acr).ToList<UA_ActingRoles>();
            }
            else
            {
                lstActingRole = (from acr in context.UA_ActingRoles
                                 select acr).ToList<UA_ActingRoles>();
            }

            lstActingRole = lstActingRole.Where(acr => (acr.UserID == _UserID || _UserID == -1) && (acr.AssignedUserID == _AssignedUserID || _AssignedUserID == -1) &&
                (acr.FromDate <= _ToDate || _ToDate == null) && (_FromDate <= acr.ToDate || _FromDate == null)).OrderByDescending(acr => acr.ID).ToList();

            List<dynamic> lstRoleData = new List<dynamic>();

            foreach (UA_ActingRoles mdlActingRole in lstActingRole)
            {
                List<UA_AssociatedLocation> lstFromLocation = (from asl in context.UA_AssociatedLocation
                                                               where asl.UserID == mdlActingRole.UserID
                                                               select asl).ToList();

                StringBuilder FromLocation = new StringBuilder();

                foreach (UA_AssociatedLocation mdlAssociatedLocation in lstFromLocation)
                {
                    FromLocation.AppendLine(String.Format("{0}: {1},", mdlAssociatedLocation.UA_IrrigationLevel.Name, GetLevelName(mdlAssociatedLocation.UA_IrrigationLevel.Name, mdlAssociatedLocation.IrrigationBoundryID)));
                }

                if (lstFromLocation.Count() > 0)
                {
                    FromLocation = FromLocation.Remove(FromLocation.ToString().LastIndexOf(','), 1);
                }

                List<UA_AssociatedLocation> lstToLocation = (from asl in context.UA_AssociatedLocation
                                                             where asl.UserID == mdlActingRole.AssignedUserID
                                                             select asl).ToList();

                StringBuilder ToLocation = new StringBuilder();

                foreach (UA_AssociatedLocation mdlAssociatedLocation in lstToLocation)
                {
                    ToLocation.AppendLine(String.Format("{0}: {1},", mdlAssociatedLocation.UA_IrrigationLevel.Name, GetLevelName(mdlAssociatedLocation.UA_IrrigationLevel.Name, mdlAssociatedLocation.IrrigationBoundryID)));
                }

                if (lstToLocation.Count() > 0)
                {
                    ToLocation = ToLocation.Remove(ToLocation.ToString().LastIndexOf(','), 1);
                }

                UA_Users mdlUser = (from usr in context.UA_Users
                                    where usr.ID == mdlActingRole.AssignedUserID
                                    select usr).FirstOrDefault();

                lstRoleData.Add(new
                {
                    ActingRole = mdlActingRole,
                    FromLocation = FromLocation.ToString(),
                    ToUser = mdlUser,
                    ToLocation = ToLocation.ToString()
                });
            }

            return lstRoleData;
        }

        /// <summary>
        /// This is a recrusive function which return the reporting users to Manager and their subordinates.
        /// Created On 18-01-2016
        /// </summary>
        /// <param name="_ManagerID"></param>
        /// <returns>List<long></returns>
        private List<long> GetReportingUsers(long _ManagerID)
        {
            List<long> lstReportingUsers = (from usrman in context.UA_UserManager
                                            where usrman.ManagerID == _ManagerID
                                            select usrman.UserID).ToList<long>();

            if (lstReportingUsers.Count() > 0)
            {
                foreach (int ManagerID in lstReportingUsers)
                {
                    lstReportingUsers = lstReportingUsers.Concat(GetReportingUsers(ManagerID)).ToList<long>();
                }
            }

            return lstReportingUsers;
        }

        /// <summary>
        /// This is a recrusive function which return the reporting designation to Manager and their subordinates.
        /// Created On 27-01-2016
        /// </summary>
        /// <param name="_ManagerID"></param>
        /// <returns>List<long></returns>
        public List<long> GetReportingDesignations(long _ManagerID)
        {
            List<long> lstReportingDesignations = (from desg in context.UA_Designations
                                                   where desg.ReportingToDesignationID == _ManagerID
                                                   select desg.ID).ToList<long>();

            lstReportingDesignations.Remove(_ManagerID);

            if (lstReportingDesignations.Count() > 0)
            {
                foreach (int ManagerID in lstReportingDesignations)
                {
                    lstReportingDesignations = lstReportingDesignations.Concat(GetReportingDesignations(ManagerID)).ToList<long>();
                }
            }

            return lstReportingDesignations;
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
        public List<UA_Users> GetUsers(string _UserName, string _FullName, long _OrganizationID, long _DesignationID, long _RoleID, bool? _Status, long _ZoneID,
            long _CircleID, long _DivisionID, long _SubDivisionID, long _SectionID, long _UserID, long? _UserLevelID, long _UserRoleID)
        {
            List<long> lstReportingUsers = null;
            List<UA_Users> lstUser = null;

            if (_UserRoleID != Constants.AdministratorRoleID)
            {
                lstReportingUsers = GetReportingUsers(_UserID);
            }
            else
            {
                lstUser = (from usr in context.UA_Users
                           where usr.ID != _UserID && usr.RoleID != null && usr.DesignationID != null
                           select usr).ToList<UA_Users>();

                lstReportingUsers = lstUser.Select(usr => usr.ID).ToList<long>();
            }

            #region Location Criteria

            if (_UserLevelID != null)
            {
                #region User Level Exists

                List<long> lstCircleID = null;
                List<long> lstDivisionID = null;
                List<long> lstSubDivisionID = null;
                List<long> lstSectionID = null;
                List<long> lstZoneUserID = null;
                List<long> lstCircleUserID = null;
                List<long> lstDivisionUserID = null;
                List<long> lstSubDivisionUserID = null;
                List<long> lstSectionUserID = null;

                if (_UserLevelID == (long)Constants.IrrigationLevelID.Zone)
                {
                    #region For User Zone Level

                    if (_ZoneID != -1)
                    {
                        lstZoneUserID = (from asl in context.UA_AssociatedLocation
                                         where asl.IrrigationLevelID == 1 && lstReportingUsers.Contains(asl.UserID) && asl.IrrigationBoundryID == _ZoneID
                                         select asl.UserID).ToList();

                        if (_CircleID != -1)
                        {
                            lstCircleID = new List<long>() { _CircleID };
                        }
                        else
                        {
                            lstCircleID = (from cir in context.CO_Circle
                                           where cir.ZoneID == _ZoneID
                                           select cir.ID).ToList();
                        }

                        lstCircleUserID = (from asl in context.UA_AssociatedLocation
                                           where asl.IrrigationLevelID == 2 && lstReportingUsers.Contains(asl.UserID) && lstCircleID.Contains((long)asl.IrrigationBoundryID)
                                           select asl.UserID).ToList();

                        if (_DivisionID != -1)
                        {
                            lstDivisionID = new List<long>() { _DivisionID };
                        }
                        else
                        {
                            lstDivisionID = (from div in context.CO_Division
                                             where lstCircleID.Contains((long)div.CircleID)
                                             select div.ID).ToList();
                        }

                        lstDivisionUserID = (from asl in context.UA_AssociatedLocation
                                             where asl.IrrigationLevelID == 3 && lstReportingUsers.Contains(asl.UserID) && lstDivisionID.Contains((long)asl.IrrigationBoundryID)
                                             select asl.UserID).ToList();

                        if (_SubDivisionID != -1)
                        {
                            lstSubDivisionID = new List<long>() { _SubDivisionID };
                        }
                        else
                        {
                            lstSubDivisionID = (from subdiv in context.CO_SubDivision
                                                where lstDivisionID.Contains((long)subdiv.DivisionID)
                                                select subdiv.ID).ToList();
                        }

                        lstSubDivisionUserID = (from asl in context.UA_AssociatedLocation
                                                where asl.IrrigationLevelID == 4 && lstReportingUsers.Contains(asl.UserID) && lstSubDivisionID.Contains((long)asl.IrrigationBoundryID)
                                                select asl.UserID).ToList();

                        if (_SectionID != -1)
                        {
                            lstSectionID = new List<long>() { _SectionID };
                        }
                        else
                        {
                            lstSectionID = (from sec in context.CO_Section
                                            where lstSubDivisionID.Contains((long)sec.SubDivID)
                                            select sec.ID).ToList();
                        }

                        lstSectionUserID = (from asl in context.UA_AssociatedLocation
                                            where asl.IrrigationLevelID == 5 && lstReportingUsers.Contains(asl.UserID) && lstSectionID.Contains((long)asl.IrrigationBoundryID)
                                            select asl.UserID).ToList();

                        lstZoneUserID = lstZoneUserID.Concat(lstCircleUserID).ToList();
                        lstZoneUserID = lstZoneUserID.Concat(lstDivisionUserID).ToList();
                        lstZoneUserID = lstZoneUserID.Concat(lstSubDivisionUserID).ToList();
                        lstZoneUserID = lstZoneUserID.Concat(lstSectionUserID).ToList();
                        lstReportingUsers = lstZoneUserID;
                    }

                    #endregion
                }
                else if (_UserLevelID == (long)Constants.IrrigationLevelID.Circle)
                {
                    #region For User Circle Level

                    if (_CircleID != -1)
                    {
                        lstCircleUserID = (from asl in context.UA_AssociatedLocation
                                           where asl.IrrigationLevelID == 2 && lstReportingUsers.Contains(asl.UserID) && asl.IrrigationBoundryID == _CircleID
                                           select asl.UserID).ToList();

                        if (_DivisionID != -1)
                        {
                            lstDivisionID = new List<long>() { _DivisionID };
                        }
                        else
                        {
                            lstDivisionID = (from div in context.CO_Division
                                             where div.CircleID == _CircleID
                                             select div.ID).ToList();
                        }

                        lstDivisionUserID = (from asl in context.UA_AssociatedLocation
                                             where asl.IrrigationLevelID == 3 && lstReportingUsers.Contains(asl.UserID) && lstDivisionID.Contains((long)asl.IrrigationBoundryID)
                                             select asl.UserID).ToList();

                        if (_SubDivisionID != -1)
                        {
                            lstSubDivisionID = new List<long>() { _SubDivisionID };
                        }
                        else
                        {
                            lstSubDivisionID = (from subdiv in context.CO_SubDivision
                                                where lstDivisionID.Contains((long)subdiv.DivisionID)
                                                select subdiv.ID).ToList();
                        }

                        lstSubDivisionUserID = (from asl in context.UA_AssociatedLocation
                                                where asl.IrrigationLevelID == 4 && lstReportingUsers.Contains(asl.UserID) && lstSubDivisionID.Contains((long)asl.IrrigationBoundryID)
                                                select asl.UserID).ToList();

                        if (_SectionID != -1)
                        {
                            lstSectionID = new List<long>() { _SectionID };
                        }
                        else
                        {
                            lstSectionID = (from sec in context.CO_Section
                                            where lstSubDivisionID.Contains((long)sec.SubDivID)
                                            select sec.ID).ToList();
                        }

                        lstSectionUserID = (from asl in context.UA_AssociatedLocation
                                            where asl.IrrigationLevelID == 5 && lstReportingUsers.Contains(asl.UserID) && lstSectionID.Contains((long)asl.IrrigationBoundryID)
                                            select asl.UserID).ToList();

                        lstCircleUserID = lstCircleUserID.Concat(lstDivisionUserID).ToList();
                        lstCircleUserID = lstCircleUserID.Concat(lstSubDivisionUserID).ToList();
                        lstCircleUserID = lstCircleUserID.Concat(lstSectionUserID).ToList();
                        lstReportingUsers = lstCircleUserID;
                    }

                    #endregion
                }
                else if (_UserLevelID == (long)Constants.IrrigationLevelID.Division)
                {
                    #region For User Division Level

                    if (_DivisionID != -1)
                    {
                        lstDivisionUserID = (from asl in context.UA_AssociatedLocation
                                             where asl.IrrigationLevelID == 3 && lstReportingUsers.Contains(asl.UserID) && asl.IrrigationBoundryID == _DivisionID
                                             select asl.UserID).ToList();

                        if (_SubDivisionID != -1)
                        {
                            lstSubDivisionID = new List<long>() { _SubDivisionID };
                        }
                        else
                        {
                            lstSubDivisionID = (from subdiv in context.CO_SubDivision
                                                where subdiv.DivisionID == _DivisionID
                                                select subdiv.ID).ToList();
                        }

                        lstSubDivisionUserID = (from asl in context.UA_AssociatedLocation
                                                where asl.IrrigationLevelID == 4 && lstReportingUsers.Contains(asl.UserID) && lstSubDivisionID.Contains((long)asl.IrrigationBoundryID)
                                                select asl.UserID).ToList();

                        if (_SectionID != -1)
                        {
                            lstSectionID = new List<long>() { _SectionID };
                        }
                        else
                        {
                            lstSectionID = (from sec in context.CO_Section
                                            where lstSubDivisionID.Contains((long)sec.SubDivID)
                                            select sec.ID).ToList();
                        }

                        lstSectionUserID = (from asl in context.UA_AssociatedLocation
                                            where asl.IrrigationLevelID == 5 && lstReportingUsers.Contains(asl.UserID) && lstSectionID.Contains((long)asl.IrrigationBoundryID)
                                            select asl.UserID).ToList();

                        lstDivisionUserID = lstDivisionUserID.Concat(lstSubDivisionUserID).ToList();
                        lstDivisionUserID = lstDivisionUserID.Concat(lstSectionUserID).ToList();
                        lstReportingUsers = lstDivisionUserID;
                    }

                    #endregion
                }
                else if (_UserLevelID == (long)Constants.IrrigationLevelID.SubDivision)
                {
                    #region For User Sub Division Level

                    if (_SubDivisionID != -1)
                    {
                        lstSubDivisionUserID = (from asl in context.UA_AssociatedLocation
                                                where asl.IrrigationLevelID == 4 && lstReportingUsers.Contains(asl.UserID) && asl.IrrigationBoundryID == _SubDivisionID
                                                select asl.UserID).ToList();

                        if (_SectionID != -1)
                        {
                            lstSectionID = new List<long>() { _SectionID };
                        }
                        else
                        {
                            lstSectionID = (from sec in context.CO_Section
                                            where sec.SubDivID == _SubDivisionID
                                            select sec.ID).ToList();
                        }

                        lstSectionUserID = (from asl in context.UA_AssociatedLocation
                                            where asl.IrrigationLevelID == 5 && lstReportingUsers.Contains(asl.UserID) && lstSectionID.Contains((long)asl.IrrigationBoundryID)
                                            select asl.UserID).ToList();

                        lstSubDivisionUserID = lstSubDivisionUserID.Concat(lstSectionUserID).ToList();
                        lstReportingUsers = lstSubDivisionUserID;
                    }

                    #endregion
                }
                else if (_UserLevelID == (long)Constants.IrrigationLevelID.Section)
                {
                    #region For User Section Level

                    if (_SectionID != -1)
                    {
                        lstReportingUsers = (from asl in context.UA_AssociatedLocation
                                             where asl.IrrigationLevelID == 5 && lstReportingUsers.Contains(asl.UserID) && asl.IrrigationBoundryID == _SectionID
                                             select asl.UserID).ToList();
                    }

                    #endregion
                }

                #endregion
            }
            else
            {
                #region User Level does not exist

                List<long> lstCircleID = null;
                List<long> lstDivisionID = null;
                List<long> lstSubDivisionID = null;
                List<long> lstSectionID = null;
                List<long> lstZoneUserID = null;
                List<long> lstCircleUserID = null;
                List<long> lstDivisionUserID = null;
                List<long> lstSubDivisionUserID = null;
                List<long> lstSectionUserID = null;

                if (_ZoneID != -1)
                {
                    lstZoneUserID = (from asl in context.UA_AssociatedLocation
                                     where asl.IrrigationLevelID == 1 && lstReportingUsers.Contains(asl.UserID) && asl.IrrigationBoundryID == _ZoneID
                                     select asl.UserID).ToList();

                    if (_CircleID != -1)
                    {
                        lstCircleID = new List<long>() { _CircleID };
                    }
                    else
                    {
                        lstCircleID = (from cir in context.CO_Circle
                                       where cir.ZoneID == _ZoneID
                                       select cir.ID).ToList();
                    }

                    lstCircleUserID = (from asl in context.UA_AssociatedLocation
                                       where asl.IrrigationLevelID == 2 && lstReportingUsers.Contains(asl.UserID) && lstCircleID.Contains((long)asl.IrrigationBoundryID)
                                       select asl.UserID).ToList();

                    if (_DivisionID != -1)
                    {
                        lstDivisionID = new List<long>() { _DivisionID };
                    }
                    else
                    {
                        lstDivisionID = (from div in context.CO_Division
                                         where lstCircleID.Contains((long)div.CircleID)
                                         select div.ID).ToList();
                    }

                    lstDivisionUserID = (from asl in context.UA_AssociatedLocation
                                         where asl.IrrigationLevelID == 3 && lstReportingUsers.Contains(asl.UserID) && lstDivisionID.Contains((long)asl.IrrigationBoundryID)
                                         select asl.UserID).ToList();

                    if (_SubDivisionID != -1)
                    {
                        lstSubDivisionID = new List<long>() { _SubDivisionID };
                    }
                    else
                    {
                        lstSubDivisionID = (from subdiv in context.CO_SubDivision
                                            where lstDivisionID.Contains((long)subdiv.DivisionID)
                                            select subdiv.ID).ToList();
                    }

                    lstSubDivisionUserID = (from asl in context.UA_AssociatedLocation
                                            where asl.IrrigationLevelID == 4 && lstReportingUsers.Contains(asl.UserID) && lstSubDivisionID.Contains((long)asl.IrrigationBoundryID)
                                            select asl.UserID).ToList();

                    if (_SectionID != -1)
                    {
                        lstSectionID = new List<long>() { _SectionID };
                    }
                    else
                    {
                        lstSectionID = (from sec in context.CO_Section
                                        where lstSubDivisionID.Contains((long)sec.SubDivID)
                                        select sec.ID).ToList();
                    }

                    lstSectionUserID = (from asl in context.UA_AssociatedLocation
                                        where asl.IrrigationLevelID == 5 && lstReportingUsers.Contains(asl.UserID) && lstSectionID.Contains((long)asl.IrrigationBoundryID)
                                        select asl.UserID).ToList();

                    lstZoneUserID = lstZoneUserID.Concat(lstCircleUserID).ToList();
                    lstZoneUserID = lstZoneUserID.Concat(lstDivisionUserID).ToList();
                    lstZoneUserID = lstZoneUserID.Concat(lstSubDivisionUserID).ToList();
                    lstZoneUserID = lstZoneUserID.Concat(lstSectionUserID).ToList();
                    lstReportingUsers = lstZoneUserID;
                }

                #endregion
            }

            #endregion

            if (_UserRoleID != Constants.AdministratorRoleID)
            {
                lstUser = (from usr in context.UA_Users
                           where lstReportingUsers.Contains(usr.ID) && usr.RoleID != Constants.AdministratorRoleID && usr.ID != _UserID
                           select usr).ToList<UA_Users>();

                lstUser = lstUser.Where(usr => ((String.Format("{0} {1}", usr.FirstName, usr.LastName).ToUpper().Contains(_FullName.ToUpper())) || _FullName == "") &&
                (usr.LoginName.ToUpper().Contains(_UserName.ToUpper()) || _UserName == "") && (usr.UA_Designations.OrganizationID == _OrganizationID || _OrganizationID == -1) &&
                (usr.DesignationID == _DesignationID || _DesignationID == -1) && (usr.RoleID == _RoleID || _RoleID == -1) && (usr.IsActive == _Status || _Status == null)).OrderBy(usr => usr.LoginName).ToList<UA_Users>();
            }
            else
            {
                lstUser = lstUser.Where(usr => lstReportingUsers.Contains(usr.ID) && ((String.Format("{0} {1}", usr.FirstName, usr.LastName).ToUpper().Contains(_FullName.ToUpper())) || _FullName == "") &&
                    (usr.LoginName.ToUpper().Contains(_UserName.ToUpper()) || _UserName == "") && (usr.UA_Designations.OrganizationID == _OrganizationID || _OrganizationID == -1) &&
                    (usr.DesignationID == _DesignationID || _DesignationID == -1) && (usr.RoleID == _RoleID || _RoleID == -1) && (usr.IsActive == _Status || _Status == null)).OrderBy(usr => usr.LoginName).ToList<UA_Users>();
            }

            return lstUser;
        }

        #region Menu

        public UA_RoleRights GetRoleId(string roleName)
        {
            UA_RoleRights v = (from u in context.UA_RoleRights
                               where u.UA_Roles.Name.ToUpper() == roleName.ToUpper()
                               select u).FirstOrDefault<UA_RoleRights>();

            return v;
        }

        public UA_RoleRights GetRoleRights(long RoleID, long PageID)
        {
            UA_RoleRights v = (from u in context.UA_RoleRights
                               where u.RoleID == RoleID && u.PageID == PageID
                               select u).FirstOrDefault<UA_RoleRights>();

            return v;
        }

        public bool IsVisible(int roleId, string pageName)
        {
            bool? v = (from u in context.UA_RoleRights
                       where u.RoleID == roleId && u.UA_Pages.Name == pageName
                       select u).FirstOrDefault<UA_RoleRights>().BView;

            return (v.HasValue) ? v.Value : false;
        }

        public List<UA_Pages> GenerateMenu(long userRoleId)
        {
            List<UA_Pages> lstPages = new List<UA_Pages>();

            lstPages = (from u in context.UA_Pages
                        join r in context.UA_RoleRights on u.ID equals r.PageID
                        where r.RoleID == userRoleId && r.UA_Pages.ShowInMenu == true && u.ParentID == 0 && r.BView == true
                        select u).OrderBy(e => e.SortOrder).ToList<UA_Pages>();

            return lstPages;
        }

        public List<object> GenerateMenu_Dashboard(long userRoleId)
        {
            //List<object> lstPages = new List<UA_Pages>();

            var lstDashboard = (from u in context.UA_Pages
                                join r in context.UA_RoleRights on u.ID equals r.PageID
                                where r.RoleID == userRoleId && r.UA_Pages.ShowInMenu == true && u.ParentID == 0 && r.BView == true
                                select u)
                        .ToList()
                        .Select(q => new { q.Name, q.UA_Modules.Icon, q.SortOrder })
                        .OrderBy(e => e.SortOrder);

            return lstDashboard.ToList<object>();
        }

        public List<UA_Pages> GetChildMenuRows(long PageID, long _RoleID)
        {
            List<UA_Pages> lstChildRows = new List<UA_Pages>();

            lstChildRows = (from u in context.UA_Pages
                            join r in context.UA_RoleRights on u.ID equals r.PageID
                            where r.RoleID == _RoleID && u.ParentID == PageID && r.UA_Pages.ShowInMenu == true
                            && r.BView == true
                            select u).OrderBy(e => e.SortOrder).ToList<UA_Pages>();

            return lstChildRows;
        }

        /// <summary>
        /// This function return page ID along with its all parent IDs.
        /// </summary>
        /// <param name="_PageID"></param>
        /// <returns>List<long</returns>
        public List<long> GetParentPageIDs(long _PageID)
        {
            List<long> lstPage = new List<long>();

            UA_Pages mdlPage = (from p in context.UA_Pages where p.ID == _PageID select p).FirstOrDefault();

            lstPage.Add(mdlPage.ID);

            if (mdlPage.ParentID != null && mdlPage.ParentID != 0)
            {
                lstPage = lstPage.Concat(GetParentPageIDs((long)mdlPage.ParentID)).ToList<long>();
            }

            return lstPage;
        }

        #endregion

        #region Notification


        public long GetAlertCount(long _UserID)
        {
            int count = (from row in context.UA_AlertNotification
                         where row.Status == (int)Constants.AlertStatus.Inbox && row.UserID == _UserID
                         select row).Count();
            return count;
        }

        public List<dynamic> GetNotificationAlertList(long _UserID)
        {

            List<dynamic> lstAlertNotification = (from row in context.UA_AlertNotification
                                                  where (row.Status == (int)Constants.AlertStatus.Inbox || row.Status == (int)Constants.AlertStatus.Unread)
                                                  && row.UserID == _UserID
                                                  orderby new { row.ID } descending
                                                  select row).ToList()
                                                               .Select(n => new
                                                               {
                                                                   n.AlertText,
                                                                   n.AlertURL,
                                                                   n.ID
                                                               }).ToList<dynamic>();

            return lstAlertNotification;
        }

        public DataTable GetAllNotificationBySearchCriteria(long _StausID, DateTime? _FromDate, DateTime? _ToDate, long _UserID)
        {
            ContextDB dbADO = new ContextDB();
            return dbADO.ExecuteStoredProcedureDataTable("UA_SearchAlertNotification", _StausID, _FromDate, _ToDate, _UserID);
        }

        public long UpdateAlertCount(long _UserID)
        {
            (from p in context.UA_AlertNotification
             where p.UserID == _UserID &&
                   p.Status == 0
             select p).ToList()
                                        .ForEach(x => x.Status = 1);
            context.SaveChanges();
            return 1;
        }

        public bool UnreadAllAlertNotification(List<dynamic> _lstofID, Int16 _StatusID)
        {
            ContextDB dbADO = new ContextDB();
            foreach (var item in _lstofID)
            {

                UA_AlertNotification mdlAlert = dbADO.Repository<UA_AlertNotification>().FindById(item);

                mdlAlert.Status = _StatusID;

                dbADO.Repository<UA_AlertNotification>().Update(mdlAlert);
                dbADO.Save();
            }
            return true;
        }

        public bool ConvertToAsRead(long _RowID)
        {
            ContextDB dbADO = new ContextDB();
            UA_AlertNotification mdlAlert = dbADO.Repository<UA_AlertNotification>().FindById(_RowID);
            mdlAlert.Status = 2;
            dbADO.Repository<UA_AlertNotification>().Update(mdlAlert);
            dbADO.Save();

            return true;
        }
        #endregion

        public List<long?> GetUserIrrigationBoundryIDs(long _UserID)
        {
            List<long?> lstIrrigationBoundryID = (from al in context.UA_AssociatedLocation
                                                  where al.UserID == _UserID
                                                  select al.IrrigationBoundryID).ToList<long?>();
            return lstIrrigationBoundryID;
        }
        public List<CO_SubDivision> GetSubDivisionsByUserAssociatedLocation(long _UserID)
        {
            List<long?> lstIrrigationBoundaryID = GetUserIrrigationBoundryIDs(_UserID);
            List<CO_SubDivision> lstSubDivision = (from sd in context.CO_SubDivision
                                                   where lstIrrigationBoundaryID.Contains(sd.ID)
                                                   select sd).ToList<CO_SubDivision>();

            //List<object> lstSubDivision = null;
            //switch (_BoundryID)
            //{
            //    case (int)Constants.IrrigationLevelID.SubDivision:
            //        lstSubDivision = (from sd in context.CO_SubDivision
            //                          where lstIrrigationBoundaryID.Contains(sd.ID)
            //                          select new
            //                          {
            //                              sd.ID,
            //                              sd.Name
            //                          }).ToList<object>();
            //        break;
            //    case (int)Constants.IrrigationLevelID.Division:
            //        lstSubDivision = (from sd in context.CO_SubDivision
            //                          join d in context.CO_Division on sd.DivisionID equals d.ID
            //                          where lstIrrigationBoundaryID.Contains(d.ID)
            //                          select new
            //                          {
            //                              sd.ID,
            //                              sd.Name
            //                          }).ToList<object>();
            //        break;
            //    case (int)Constants.IrrigationLevelID.Circle:
            //        lstSubDivision = (from sd in context.CO_SubDivision
            //                          join d in context.CO_Division on sd.DivisionID equals d.ID
            //                          where lstIrrigationBoundaryID.Contains(d.ID)
            //                          select new
            //                          {
            //                              sd.ID,
            //                              sd.Name
            //                          }).ToList<object>();
            //        break;
            //    default:
            //        break;
            //}

            return lstSubDivision;
        }
        public List<CO_Division> GetDivisionsByUserAssociatedLocation(long _UserID)
        {
            List<long?> lstIrrigationBoundaryID = GetUserIrrigationBoundryIDs(_UserID);
            List<CO_Division> lstDivisions = (from d in context.CO_Division
                                              where lstIrrigationBoundaryID.Contains(d.ID)
                                              select d).ToList<CO_Division>();
            return lstDivisions;
        }
        public List<CO_Circle> GetCirclesByUserAssociatedLocation(long _UserID)
        {
            List<long?> lstIrrigationBoundaryID = GetUserIrrigationBoundryIDs(_UserID);
            List<CO_Circle> lstCircles = (from c in context.CO_Circle
                                          where lstIrrigationBoundaryID.Contains(c.ID)
                                          select c).ToList<CO_Circle>();
            return lstCircles;
        }
        public List<CO_Zone> GetZonesByUserAssociatedLocation(long _UserID)
        {
            List<long?> lstIrrigationBoundaryID = GetUserIrrigationBoundryIDs(_UserID);
            List<CO_Zone> lstZones = (from z in context.CO_Zone
                                      where lstIrrigationBoundaryID.Contains(z.ID)
                                      select z).ToList<CO_Zone>();
            return lstZones;
        }
        public List<object> GetRegionsListByUser(long _UserID, int _IrrigationBoundaryID)
        {
            List<object> lstRegion = new List<object>();
            List<CO_SubDivision> lstSubDivision = null;
            List<CO_Division> lstDivision = null;
            List<CO_Circle> lstCircle = null;
            List<CO_Zone> lstZone = null;
            List<long?> lstIDs = new List<long?>();
            List<long> lstNIDs = new List<long>();

            switch (_IrrigationBoundaryID)
            {
                case (int)Constants.IrrigationLevelID.SubDivision:
                    lstSubDivision = GetSubDivisionsByUserAssociatedLocation(_UserID);
                    lstIDs = lstSubDivision.Distinct().Select(sd => sd.DivisionID).ToList<long?>();
                    lstDivision = (from d in context.CO_Division
                                   where lstIDs.Contains(d.ID)
                                   select d).ToList<CO_Division>();
                    lstIDs.Clear();

                    lstIDs = lstDivision.Distinct().Select(d => d.CircleID).ToList<long?>();

                    lstCircle = (from c in context.CO_Circle
                                 where lstIDs.Contains(c.ID)
                                 select c).ToList<CO_Circle>();
                    lstIDs.Clear();
                    lstNIDs = lstCircle.Distinct().Select(c => c.ZoneID).ToList<long>();
                    lstZone = (from z in context.CO_Zone
                               where lstNIDs.Contains(z.ID)
                               select z).ToList<CO_Zone>();
                    break;
                case (int)Constants.IrrigationLevelID.Division:
                    lstDivision = GetDivisionsByUserAssociatedLocation(_UserID);
                    lstNIDs = lstDivision.Distinct().Select(d => d.ID).ToList<long>();
                    lstSubDivision = (from sd in context.CO_SubDivision
                                      where lstNIDs.Contains(sd.DivisionID.Value)
                                      select sd).ToList<CO_SubDivision>();

                    lstIDs = lstDivision.Distinct().Select(d => d.CircleID).ToList<long?>();
                    lstCircle = (from c in context.CO_Circle
                                 where lstIDs.Contains(c.ID)
                                 select c).ToList<CO_Circle>();
                    lstNIDs.Clear();
                    lstNIDs = lstCircle.Distinct().Select(c => c.ZoneID).ToList<long>();
                    lstZone = (from z in context.CO_Zone
                               where lstNIDs.Contains(z.ID)
                               select z).ToList<CO_Zone>();
                    break;
                case (int)Constants.IrrigationLevelID.Circle:
                    lstCircle = GetCirclesByUserAssociatedLocation(_UserID);

                    lstNIDs = lstCircle.Distinct().Select(c => c.ID).ToList<long>();

                    lstDivision = (from d in context.CO_Division
                                   where lstNIDs.Contains(d.CircleID.Value)
                                   select d).ToList<CO_Division>();
                    lstNIDs.Clear();
                    lstNIDs = lstDivision.Distinct().Select(d => d.ID).ToList<long>();
                    lstSubDivision = (from sd in context.CO_SubDivision
                                      where lstNIDs.Contains(sd.DivisionID.Value)
                                      select sd).ToList<CO_SubDivision>();
                    lstNIDs.Clear();
                    lstNIDs = lstCircle.Distinct().Select(c => c.ZoneID).ToList<long>();
                    lstZone = (from z in context.CO_Zone
                               where lstNIDs.Contains(z.ID)
                               select z).ToList<CO_Zone>();
                    break;
                case (int)Constants.IrrigationLevelID.Zone:
                    lstZone = GetZonesByUserAssociatedLocation(_UserID);
                    lstNIDs = lstZone.Distinct().Select(d => d.ID).ToList<long>();
                    lstCircle = (from c in context.CO_Circle
                                 where lstNIDs.Contains(c.ZoneID)
                                 select c).ToList<CO_Circle>();

                    lstNIDs.Clear();
                    lstNIDs = lstCircle.Distinct().Select(c => c.ID).ToList<long>();
                    lstDivision = (from d in context.CO_Division
                                   where lstNIDs.Contains(d.CircleID.Value)
                                   select d).ToList<CO_Division>();
                    lstNIDs.Clear();
                    lstNIDs = lstDivision.Distinct().Select(d => d.ID).ToList<long>();
                    lstSubDivision = (from sd in context.CO_SubDivision
                                      where lstNIDs.Contains(sd.DivisionID.Value)
                                      select sd).ToList<CO_SubDivision>();

                    break;

                default:
                    break;
            }


            lstRegion.Add(lstSubDivision);//SubDivision 

            lstRegion.Add(lstDivision);// Division 

            lstRegion.Add(lstCircle);// Circle 

            lstRegion.Add(lstZone);// Zone 

            return lstRegion;
        }

        public List<object> GetSwitchUsers(long _UserID, DateTime _Now)
        {
            var qSwitchUser = (from ac in context.UA_ActingRoles
                               where ac.UserID == _UserID
                               //&&
                               //ac.FromDate <= _Now && _Now <= ac.ToDate
                               select
                                 new
                                 {
                                     AssignedUserID = ac.AssignedUserID,
                                     FullName = ac.UA_Users1.FirstName + " " + ac.UA_Users1.LastName
                                 }
                              )
                              .ToList()
                              .Select(k => new { k.AssignedUserID, k.FullName })
                              .ToList<object>();
            return qSwitchUser;
        }

        public long GetUserManagerByLocationID(long _UserID)
        {
            long ManagerID = (from d in context.UA_GetUserManagerID(_UserID)
                              select d).FirstOrDefault().Value;

            return ManagerID;
        }

        #region Add User

        public List<long> CheckManager(long? _ReportingToDesignationID, long? _ID)
        {
            List<long> lstUserID = new List<long>();
            try
            {
                List<UA_AssociatedLocation> AssocLoc = (from UM in context.UA_AssociatedLocation
                                                        where UM.DesignationID == _ReportingToDesignationID
                                                        select UM).ToList();

                if (AssocLoc != null && AssocLoc.Count() > 0)
                    lstUserID = AssocLoc.Where(q => q.IrrigationBoundryID == _ID).Select(w => w.UserID).ToList();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstUserID;
        }

        public List<object> GetManagers(long _DesID, long _UserID)
        {
            List<long> lstManagerID = new List<long>();
            List<object> lstManager = new List<object>();

            try
            {
                UA_Designations Designation = (from Des in context.UA_Designations
                                               where Des.ID == _DesID
                                               select Des).FirstOrDefault();

                if (Designation != null && Designation.IrrigationLevelID != null && Designation.ReportingToDesignationID != null) // Users with Irrigation Level ID's
                {

                    UA_Designations ReportingDes = (from Des in context.UA_Designations
                                                    where Des.ID == Designation.ReportingToDesignationID
                                                    select Des).FirstOrDefault();
                    if (ReportingDes != null)
                    {
                        if (ReportingDes.IrrigationLevelID != null && ReportingDes.ReportingToDesignationID != null)
                        {
                            UA_AssociatedLocation UserLoc = (from AL in context.UA_AssociatedLocation
                                                             where AL.UserID == _UserID
                                                             select AL).FirstOrDefault();
                            if (UserLoc != null)
                            {
                                if (UserLoc.IrrigationLevelID == (int)Constants.IrrigationLevelID.Section)
                                {
                                    CO_Section Section = (from Sec in context.CO_Section
                                                          where Sec.ID == UserLoc.IrrigationBoundryID
                                                          select Sec).FirstOrDefault();

                                    lstManagerID = CheckManager(Designation.ReportingToDesignationID, Section.SubDivID);
                                }
                                else if (UserLoc.IrrigationLevelID == (int)Constants.IrrigationLevelID.SubDivision)
                                {
                                    CO_SubDivision Section = (from Sec in context.CO_SubDivision
                                                              where Sec.ID == UserLoc.IrrigationBoundryID
                                                              select Sec).FirstOrDefault();

                                    lstManagerID = CheckManager(Designation.ReportingToDesignationID, Section.DivisionID);
                                }
                                else if (UserLoc.IrrigationLevelID == (int)Constants.IrrigationLevelID.Division)
                                {
                                    CO_Division Section = (from Sec in context.CO_Division
                                                           where Sec.ID == UserLoc.IrrigationBoundryID
                                                           select Sec).FirstOrDefault();

                                    lstManagerID = CheckManager(Designation.ReportingToDesignationID, Section.CircleID);
                                }
                                else if (UserLoc.IrrigationLevelID == (int)Constants.IrrigationLevelID.Circle)
                                {
                                    CO_Circle Section = (from Sec in context.CO_Circle
                                                         where Sec.ID == UserLoc.IrrigationBoundryID
                                                         select Sec).FirstOrDefault();

                                    lstManagerID = CheckManager(Designation.ReportingToDesignationID, Section.ZoneID);
                                }
                                //else if (UserLoc.IrrigationLevelID == (int)Constants.IrrigationLevelID.Zone)
                                //{
                                //    lstManagerID = (from User in context.UA_Users
                                //                    where User.DesignationID == (int)Constants.Designation.Secretary
                                //                    select User.ID).ToList();
                                //}
                            }
                        }
                        else
                        {
                            lstManagerID = (from User in context.UA_Users
                                            where User.DesignationID == Designation.ReportingToDesignationID
                                            select User.ID).ToList();
                        }
                    }
                }
                else if (Designation.IrrigationLevelID == null)
                {
                    lstManagerID = (from User in context.UA_Users
                                    where User.DesignationID == Designation.ReportingToDesignationID
                                    select User.ID).ToList();
                }

                lstManager = (from User in context.UA_Users
                              where lstManagerID.Contains(User.ID) && User.IsActive == true
                              select new
                              {
                                  ID = User.ID,
                                  Name = User.FirstName + " " + User.LastName + " (" + User.UA_Designations.Name + ")",
                                  DesignationID = User.DesignationID
                              }).ToList<object>();
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
            return lstManager;
        }

        public void RemoveLocationsForUser(long _UserID)
        {
            try
            {
                context.UA_AssociatedLocation.RemoveRange(context.UA_AssociatedLocation.Where(q => q.UserID == _UserID));
            }
            catch (Exception exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.WebApp);
            }
        }

        #endregion


    }
}
