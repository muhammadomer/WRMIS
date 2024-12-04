using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Common;

namespace PMIU.WRMIS.DAL.Repositories.UserAdministration
{
    public class SearchUserRepository : Repository<UA_Users>
    {

        WRMIS_Entities _context;
        public SearchUserRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<UA_Users>();
            _context = context;
        }

        public string CheckRoleForDesignation(int designationID)
        {
            string role = "";
            try
            {
                //var result = (from des in context.UA_Designations
                //              join desType in context.UA_DesignationType on des.DesignationTypeID equals desType.ID
                //              where des.ID == designationID
                //              select new { desType.Name });

                //if (result != null && result.FirstOrDefault().Name != null)
                //{
                //    role = result.FirstOrDefault().Name;
                //}
            }
            catch (WRException exp)
            {
                new WRException(Constants.UserID, exp).LogException(Constants.MessageCategory.DataAccess);
            }
            return role;
        }


        public string CheckLevelForUserName(string userName)
        {
            string role = "";
            var result = (from usr in context.UA_Users
                          join asso in context.UA_AssociatedLocation on usr.ID equals asso.UserID
                          join lvl in context.UA_IrrigationLevel on asso.IrrigationLevelID equals lvl.ID
                          where usr.LoginName == userName
                          select lvl.Name).ToList();

            if (result != null && result.Count() > 0)
            {
                role = result.FirstOrDefault();
            }

            return role;
        }


        public string CheckLevelForFullName(string fullName)
        {
            string role = "";



            var result = (from usr in context.UA_Users
                          join asso in context.UA_AssociatedLocation on usr.ID equals asso.UserID
                          join lvl in context.UA_IrrigationLevel on asso.IrrigationLevelID equals lvl.ID
                          where usr.FirstName + " " + usr.LastName == fullName || usr.FirstName + usr.LastName == fullName
                          select new { lvl.Name });

            if (result != null && result.FirstOrDefault().Name != "")
            {
                role = result.FirstOrDefault().Name;
            }

            return role;
        }

        public string GetLocationName(string Location, long? LocationID)
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


        public string UpdateStatus(bool? Status)
        {
            if ((bool)Status)
                return "Active";
            else
                return "InActive";
        }


        public List<dynamic> SearchUser(string userName, string fullName, string status, long OrganizationID, string OrganizationName, long designationID, string designationName, long LastSelectionID, long SelectionID, long? UserDesignation)//long zoneID, string zoneName, long circleID, string circleName, long divisionID, string divisionName, long subDivisionID, string subDivName, long sectionID, string sectionName)
        {
            List<dynamic> lstResult = new List<dynamic>();


            if (LastSelectionID == 1) // means query has to be sent on zone 
            {

                lstResult = (from usr in context.UA_Users
                             join des in context.UA_Designations on usr.DesignationID equals des.ID
                             join org in context.UA_Organization on des.OrganizationID equals org.ID
                             join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                             join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                             join zon in context.CO_Zone on assLoc.IrrigationLevelID equals zon.ID
                             where
                             (usr.LoginName == userName || userName == "") &&
                             (usr.FirstName + usr.LastName == fullName || usr.FirstName == fullName || usr.LastName == fullName || fullName == "") &&
                             (usr.DesignationID == designationID || designationID == -1) &&
                             (des.OrganizationID == OrganizationID || OrganizationID == -1) &&
                                 // (des.ReportingToDesignationID >= UserDesignation) &&
                             (assLoc.IrrigationLevelID == LastSelectionID) &&
                             (assLoc.IrrigationBoundryID == SelectionID) &&
                             (zon.ID == SelectionID)
                             select new
                             {
                                 ID = usr.ID,
                                 UserName = usr.LoginName,
                                 FullName = usr.FirstName + " " + usr.LastName,
                                 Organization = org.Name,
                                 Designation = des.Name,
                                 Location = irrLvl.Name + " : " + zon.Name,
                                 Status = usr.IsActive,
                             }
                            ).ToList()
                            .Select(k => new { k.ID, k.UserName, k.FullName, k.Organization, k.Designation, k.Location, Status = UpdateStatus(k.Status) })
                            .ToList<dynamic>();
            }
            else if (LastSelectionID == 2)  // means query has to be from circle table 
            {
                lstResult = (from usr in context.UA_Users
                             join des in context.UA_Designations on usr.DesignationID equals des.ID
                             join org in context.UA_Organization on des.OrganizationID equals org.ID
                             join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                             join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                             join cir in context.CO_Circle on assLoc.IrrigationLevelID equals cir.ID
                             where
                             (usr.LoginName == userName || userName == "") &&
                             (usr.FirstName + usr.LastName == fullName || usr.FirstName == fullName || usr.LastName == fullName || fullName == "") &&
                             (usr.DesignationID == designationID || designationID == -1) &&
                             (des.OrganizationID == OrganizationID || OrganizationID == -1) &&
                                 // (des.ReportingToDesignationID >= UserDesignation) &&
                             (assLoc.IrrigationLevelID == LastSelectionID) &&
                             (assLoc.IrrigationBoundryID == SelectionID)
                             select new
                             {
                                 ID = usr.ID,
                                 UserName = usr.LoginName,
                                 FullName = usr.FirstName + " " + usr.LastName,
                                 Organization = org.Name,
                                 Designation = des.Name,
                                 Location = irrLvl.Name + " : " + cir.Name,
                                 Status = usr.IsActive,
                             }
                            ).ToList()
                            .Select(k => new { k.ID, k.UserName, k.FullName, k.Organization, k.Designation, k.Location, Status = UpdateStatus(k.Status) })
                            .ToList<dynamic>();
            }
            else if (LastSelectionID == 3) //  means query has to be from division table
            {
                lstResult = (from usr in context.UA_Users
                             join des in context.UA_Designations on usr.DesignationID equals des.ID
                             join org in context.UA_Organization on des.OrganizationID equals org.ID
                             join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                             join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                             join div in context.CO_Division on assLoc.IrrigationLevelID equals div.ID
                             where
                             (usr.LoginName == userName || userName == "") &&
                             (usr.FirstName + usr.LastName == fullName || usr.FirstName == fullName || usr.LastName == fullName || fullName == "") &&
                             (usr.DesignationID == designationID || designationID == -1) &&
                             (des.OrganizationID == OrganizationID || OrganizationID == -1) &&
                                 // (des.ReportingToDesignationID >= UserDesignation) &&
                             (assLoc.IrrigationLevelID == LastSelectionID) &&
                             (assLoc.IrrigationBoundryID == SelectionID)
                             select new
                             {
                                 ID = usr.ID,
                                 UserName = usr.LoginName,
                                 FullName = usr.FirstName + " " + usr.LastName,
                                 Organization = org.Name,
                                 Designation = des.Name,
                                 Location = irrLvl.Name + " : " + div.Name,
                                 Status = usr.IsActive,
                             }
                            ).ToList()
                            .Select(k => new { k.ID, k.UserName, k.FullName, k.Organization, k.Designation, k.Location, Status = UpdateStatus(k.Status) })
                            .ToList<dynamic>();
            }
            else if (LastSelectionID == 4) //  means query has to be from division table
            {

                lstResult = (from usr in context.UA_Users
                             join des in context.UA_Designations on usr.DesignationID equals des.ID
                             join org in context.UA_Organization on des.OrganizationID equals org.ID
                             join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                             join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                             join sDiv in context.CO_SubDivision on assLoc.IrrigationLevelID equals sDiv.ID
                             where
                             (usr.LoginName == userName || userName == "") &&
                             (usr.FirstName + usr.LastName == fullName || usr.FirstName == fullName || usr.LastName == fullName || fullName == "") &&
                             (usr.DesignationID == designationID || designationID == -1) &&
                             (des.OrganizationID == OrganizationID || OrganizationID == -1) &&
                                 //  (des.ReportingToDesignationID >= UserDesignation) &&
                             (assLoc.IrrigationLevelID == LastSelectionID) &&
                             (assLoc.IrrigationBoundryID == SelectionID)
                             select new
                             {
                                 ID = usr.ID,
                                 UserName = usr.LoginName,
                                 FullName = usr.FirstName + " " + usr.LastName,
                                 Organization = org.Name,
                                 Designation = des.Name,
                                 Location = irrLvl.Name + " : " + sDiv.Name,
                                 Status = usr.IsActive,
                             }
                            ).ToList()
                            .Select(k => new { k.ID, k.UserName, k.FullName, k.Organization, k.Designation, k.Location, Status = UpdateStatus(k.Status) })
                            .ToList<dynamic>();
            }
            else if (LastSelectionID == 5)
            {
                lstResult = (from usr in context.UA_Users
                             join des in context.UA_Designations on usr.DesignationID equals des.ID
                             join org in context.UA_Organization on des.OrganizationID equals org.ID
                             join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                             join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                             join sec in context.CO_Section on assLoc.IrrigationLevelID equals sec.ID
                             where
                             (usr.LoginName == userName || userName == "") &&
                             (usr.FirstName + usr.LastName == fullName || usr.FirstName == fullName || usr.LastName == fullName || fullName == "") &&
                             (usr.DesignationID == designationID || designationID == -1) &&
                             (des.OrganizationID == OrganizationID || OrganizationID == -1) &&
                                 //  (des.ReportingToDesignationID >= UserDesignation) &&
                             (assLoc.IrrigationLevelID == LastSelectionID) &&
                             (assLoc.IrrigationBoundryID == SelectionID)
                             select new
                             {
                                 ID = usr.ID,
                                 UserName = usr.LoginName,
                                 FullName = usr.FirstName + " " + usr.LastName,
                                 Organization = org.Name,
                                 Designation = des.Name,
                                 Location = irrLvl.Name + " : " + sec.Name,
                                 Status = usr.IsActive,
                             }
                            ).ToList()
                            .Select(k => new { k.ID, k.UserName, k.FullName, k.Organization, k.Designation, k.Location, Status = UpdateStatus(k.Status) })
                            .ToList<dynamic>();
            }
            else if (userName != "" || fullName != "" || status != "" || OrganizationName != "")  // case when no level is selected and UserName,FullName, Status , organization is selected 
            {
                bool stats = true;
                if (status == "0")
                {
                    stats = false;
                }

                lstResult = (from usr in context.UA_Users
                             join des in context.UA_Designations on usr.DesignationID equals des.ID
                             join org in context.UA_Organization on des.OrganizationID equals org.ID
                             join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                             join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                             where
                             (usr.LoginName == userName || userName == "") &&
                             (usr.FirstName + " " + usr.LastName == fullName || usr.FirstName == fullName || usr.LastName == fullName || fullName == "") &&
                             (usr.DesignationID == designationID || designationID == -1) &&
                             (usr.IsActive == stats || stats == true) &&
                             (des.OrganizationID == OrganizationID || OrganizationID == -1)
                             select new
                             {
                                 ID = usr.ID,
                                 UserName = usr.LoginName,
                                 FullName = usr.FirstName + " " + usr.LastName,
                                 Organization = org.Name,
                                 Designation = des.Name,
                                 Location = irrLvl.Name,
                                 Status = usr.IsActive,
                                 IrrBoundryID = assLoc.IrrigationBoundryID
                             }
                            )
                            .ToList()
                            .Select(k => new { k.ID, k.UserName, k.FullName, k.Organization, k.Designation, Location = GetLocationName(k.Location.ToString(), k.IrrBoundryID), Status = UpdateStatus(k.Status) })
                            .ToList<dynamic>();
            }
            else  // no criteria is selected get all records 
            {
                lstResult = (from usr in context.UA_Users
                             join des in context.UA_Designations on usr.DesignationID equals des.ID
                             join org in context.UA_Organization on des.OrganizationID equals org.ID
                             join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                             join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                             //where
                             //des.ReportingToDesignationID >= UserDesignation
                             select new
                             {
                                 ID = usr.ID,
                                 UserName = usr.LoginName,
                                 FullName = usr.FirstName + " " + usr.LastName,
                                 Organization = org.Name,
                                 Designation = des.Name,
                                 Location = irrLvl.Name,
                                 Status = usr.IsActive,
                                 IrrBoundryID = assLoc.IrrigationBoundryID
                             }
                            )
                            .ToList()
                            .Select(k => new { k.ID, k.UserName, k.FullName, k.Organization, k.Designation, Location = GetLocationName(k.Location.ToString(), k.IrrBoundryID), Status = UpdateStatus(k.Status) })
                            .ToList<dynamic>();
            }

            return lstResult;

            //if(section != -1 && subDivision != -1 && division != -1 && circle != -1 && zone != -1 && designation != -1 && fullName != "" && userName != "")
            //{

            //    var allSelRes = (from usr in context.UA_Users
            //                     join rol in context.UA_DesignationsRoles on usr.DesignationsRolesID equals rol.ID
            //                     join des in context.UA_Designations on rol.DesignationID equals des.ID
            //                     join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
            //                     join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
            //                     join zon in context.CO_Zone on assLoc.IrrigationLevelID equals zon.ID
            //                     join cir in context.CO_Circle on assLoc.IrrigationLevelID equals cir.ID
            //                     join div in context.CO_Division on assLoc.IrrigationLevelID equals div.ID
            //                     join sDiv in context.CO_SubDivision on assLoc.IrrigationLevelID equals sDiv.ID
            //                     join sec in context.CO_Section on assLoc.IrrigationLevelID equals sec.ID
            //                     where usr.LoginName== userName && usr.FirstName + usr.LastName == fullName && des.Name == designationName && zon.Name == zoneName && cir.Name == circleName && div.Name == divisionName &&
            //                     sDiv.Name == divisionName && sec.Name == sectionName
            //                     select new
            //                     {
            //                         usrName = usr.LoginName,
            //                         fullName = usr.FirstName + " " + usr.LastName,
            //                         designation = des.Name,
            //                         location = "Section :" + sectionName,
            //                         status = usr.Status
            //                     }
            //        ).ToList();
            //}
            //else if (subDivision != -1 && division != -1 && circle != -1 && zone != -1 && designation != -1 && fullName != "" && userName != "")
            //{

            //    var allSelRes = (from usr in context.UA_Users
            //                     join rol in context.UA_DesignationsRoles on usr.DesignationsRolesID equals rol.ID
            //                     join des in context.UA_Designations on rol.DesignationID equals des.ID
            //                     join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
            //                     join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
            //                     join zon in context.CO_Zone on assLoc.IrrigationLevelID equals zon.ID
            //                     join cir in context.CO_Circle on assLoc.IrrigationLevelID equals cir.ID
            //                     join div in context.CO_Division on assLoc.IrrigationLevelID equals div.ID
            //                     join sDiv in context.CO_SubDivision on assLoc.IrrigationLevelID equals sDiv.ID
            //                     join sec in context.CO_Section on assLoc.IrrigationLevelID equals sec.ID
            //                     where usr.LoginName == userName && usr.FirstName + usr.LastName == fullName && des.Name == designationName && zon.Name == zoneName && cir.Name == circleName && div.Name == divisionName &&
            //                     sDiv.Name == divisionName
            //                     select new
            //                     {
            //                         usrName = usr.LoginName,
            //                         fullName = usr.FirstName + " " + usr.LastName,
            //                         designation = des.Name,
            //                         location = "SubDivision :" + subDivName,
            //                         status = usr.Status
            //                     }
            //        ).ToList();
            //}
            //else if (division != -1 && circle != -1 && zone != -1 && designation != -1 && fullName != "" && userName != "")
            //{

            //    var allSelRes = (from usr in context.UA_Users
            //                     join rol in context.UA_DesignationsRoles on usr.DesignationsRolesID equals rol.ID
            //                     join des in context.UA_Designations on rol.DesignationID equals des.ID
            //                     join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
            //                     join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
            //                     join zon in context.CO_Zone on assLoc.IrrigationLevelID equals zon.ID
            //                     join cir in context.CO_Circle on assLoc.IrrigationLevelID equals cir.ID
            //                     join div in context.CO_Division on assLoc.IrrigationLevelID equals div.ID
            //                     join sDiv in context.CO_SubDivision on assLoc.IrrigationLevelID equals sDiv.ID
            //                     join sec in context.CO_Section on assLoc.IrrigationLevelID equals sec.ID
            //                     where usr.LoginName == userName && usr.FirstName + usr.LastName == fullName && des.Name == designationName &&
            //                     zon.Name == zoneName && cir.Name == circleName && div.Name == divisionName                                 
            //                     select new
            //                     {
            //                         usrName = usr.LoginName,
            //                         fullName = usr.FirstName + " " + usr.LastName,
            //                         designation = des.Name,
            //                         location = "Division :" + divisionName,
            //                         status = usr.Status
            //                     }
            //        ).ToList();
            //}
            //else if (circle != -1 && zone != -1 && designation != -1 && fullName != "" && userName != "")
            //{

            //    var allSelRes = (from usr in context.UA_Users
            //                     join rol in context.UA_DesignationsRoles on usr.DesignationsRolesID equals rol.ID
            //                     join des in context.UA_Designations on rol.DesignationID equals des.ID
            //                     join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
            //                     join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
            //                     join zon in context.CO_Zone on assLoc.IrrigationLevelID equals zon.ID
            //                     join cir in context.CO_Circle on assLoc.IrrigationLevelID equals cir.ID
            //                     join div in context.CO_Division on assLoc.IrrigationLevelID equals div.ID
            //                     join sDiv in context.CO_SubDivision on assLoc.IrrigationLevelID equals sDiv.ID
            //                     join sec in context.CO_Section on assLoc.IrrigationLevelID equals sec.ID
            //                     where usr.LoginName == userName && usr.FirstName + usr.LastName == fullName && des.Name == designationName &&
            //                     zon.Name == zoneName && cir.Name == circleName
            //                     select new
            //                     {
            //                         usrName = usr.LoginName,
            //                         fullName = usr.FirstName + " " + usr.LastName,
            //                         designation = des.Name,
            //                         location = "Circle :" + circleName,
            //                         status = usr.Status
            //                     }
            //        ).ToList();
            //}
            //else if (zone != -1 && designation != -1 && fullName != "" && userName != "")
            //{

            //    var allSelRes = (from usr in context.UA_Users
            //                     join rol in context.UA_DesignationsRoles on usr.DesignationsRolesID equals rol.ID
            //                     join des in context.UA_Designations on rol.DesignationID equals des.ID
            //                     join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
            //                     join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
            //                     join zon in context.CO_Zone on assLoc.IrrigationLevelID equals zon.ID
            //                     join cir in context.CO_Circle on assLoc.IrrigationLevelID equals cir.ID
            //                     join div in context.CO_Division on assLoc.IrrigationLevelID equals div.ID
            //                     join sDiv in context.CO_SubDivision on assLoc.IrrigationLevelID equals sDiv.ID
            //                     join sec in context.CO_Section on assLoc.IrrigationLevelID equals sec.ID
            //                     where usr.LoginName == userName && usr.FirstName + usr.LastName == fullName && des.Name == designationName &&
            //                     zon.Name == zoneName 
            //                     select new
            //                     {
            //                         usrName = usr.LoginName,
            //                         fullName = usr.FirstName + " " + usr.LastName,
            //                         designation = des.Name,
            //                         location = "Zone :" + zoneName,
            //                         status = usr.Status
            //                     }
            //        ).ToList();
            //}
            //else if (designation != -1 && fullName != "" && userName != "")
            //{

            //    var allSelRes = (from usr in context.UA_Users
            //                     join rol in context.UA_DesignationsRoles on usr.DesignationsRolesID equals rol.ID
            //                     join des in context.UA_Designations on rol.DesignationID equals des.ID
            //                     join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
            //                     join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
            //                     join zon in context.CO_Zone on assLoc.IrrigationLevelID equals zon.ID
            //                     join cir in context.CO_Circle on assLoc.IrrigationLevelID equals cir.ID
            //                     join div in context.CO_Division on assLoc.IrrigationLevelID equals div.ID
            //                     join sDiv in context.CO_SubDivision on assLoc.IrrigationLevelID equals sDiv.ID
            //                     join sec in context.CO_Section on assLoc.IrrigationLevelID equals sec.ID
            //                     where usr.LoginName == userName && usr.FirstName + usr.LastName == fullName && des.Name == designationName

            //                     select new
            //                     {
            //                         usrName = usr.LoginName,
            //                         fullName = usr.FirstName + " " + usr.LastName,
            //                         designation = des.Name,
            //                         location = "Zone :" + zoneName,
            //                         status = usr.Status
            //                     }
            //        ).ToList();
            //}



            ///////// before 

            //List<dynamic> result = (from usr in context.UA_Users
            //                        join rol in context.UA_DesignationsRoles on usr.DesignationsRolesID equals rol.ID
            //                        join des in context.UA_Designations on rol.DesignationID equals des.ID
            //                        join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID into al
            //                        from a in al.DefaultIfEmpty()
            //                        join irrLvl in context.UA_IrrigationLevel on a.IrrigationLevelID equals irrLvl.ID
            //                        where usr.LoginName == userName && des.Name == designationName 
            //                        select new
            //                        {
            //                            usr.LoginName,
            //                            fullName = usr.FirstName + " " + usr.LastName,
            //                            usr.Status,
            //                            desName = des.Name,
            //                            irrLvlID = a.IrrigationLevelID,
            //                            lvlID = a.LevelID.Value,
            //                            irrLvlName = irrLvl.Name,
            //                            locationName = irrLvl.Name
            //                        }).ToList<dynamic>();

            //List<dynamic> lstZone = (from z in context.CO_Zone select new { z.ID, z.Name, Level = "Zone"})
            //    .Union(from c in context.CO_Circle select new { c.ID, c.Name, Level = "Circle"})
            //    .Union(from d in context.CO_Division select new { d.ID, d.Name, Level = "Division"})
            //    .Union(from sd in context.CO_SubDivision select new { sd.ID, sd.Name, Level = "Sub Division"})
            //    .Union(from s in context.CO_Section select new { s.ID, s.Name, Level = "Section"})
            //    .ToList<dynamic>();

            //List<object> lst = (from r in result
            //                    join l in lstZone on new { LevelID = r.irrLvlID, LevelName = r.irrLvlName } 
            //                    equals new {LevelID = l.ID, LevelName = l.Level }
            //                    select new
            //                    {
            //                        r.LoginName,
            //                        r.fullName,
            //                        r.Status,
            //                        r.desName,
            //                        r.irrLvlID,
            //                        r.lvlID,
            //                        LevelName = r.irrLvlName + ": " + l.Name,
            //                        r.locationName

            //                    }).ToList<object>();

            //List<object> p = lst;  


            //if (userName != "" && fullName != "" && status != "" && OrganizationID != -1 && designationID != -1 )//&& zoneID != -1 && circleID != -1 && divisionID != -1 && subDivisionID != -1 && sectionID != -1)
            //{
            //    lstResult = (from usr in context.UA_Users
            //                 join des in context.UA_Designations on usr.DesignationID equals des.ID
            //                 join org in context.UA_Organization on des.OrganizationID equals org.ID
            //                 join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
            //                 join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
            //                 where usr.LoginName == userName && usr.FirstName + usr.LastName == fullName && usr.DesignationID == designationID
            //                 && des.OrganizationID == OrganizationID
            //                 select new
            //                 {
            //                     UserName = usr.LoginName,
            //                     FullName = usr.FirstName + " " + usr.LastName,
            //                     Organization = org.Name,
            //                     Designation = des.Name,
            //                     Location = irrLvl.Name,
            //                     Status = usr.Status,
            //                     IrrBoundryID = assLoc.IrrigationBoundryID
            //                 }
            //                )
            //                .ToList()
            //                .Select(k => new { k.UserName, k.FullName, k.Organization, k.Designation, Location = GetLocationName(k.Location.ToString(), k.IrrBoundryID), k.Status })
            //                .ToList<dynamic>();
            //}

        }

        public List<UA_Designations> GetDesignationAgainstOrganization(long OrganizationID)
        {
            List<UA_Designations> lstDesignations = (from des in context.UA_Designations
                                                     where des.OrganizationID == OrganizationID
                                                     select des).ToList();

            return lstDesignations;
        }

        //Summary
        //this function return at which level this user exist i.e. Zone, Circle etc
        public object GetUserLevel(string LoginName)
        {
            object LevelName = (from usr in context.UA_Users
                                join assLoc in context.UA_AssociatedLocation on usr.ID equals assLoc.UserID
                                join irrLvl in context.UA_IrrigationLevel on assLoc.IrrigationLevelID equals irrLvl.ID
                                where usr.LoginName == LoginName
                                select
            new
            {
                IrrLvlID = irrLvl.ID,
                LevelName = irrLvl.Name,
                IrrBoundryID = assLoc.IrrigationBoundryID
            }
            ).FirstOrDefault();

            return LevelName;
        }

        public List<CO_Zone> GetZonesForSection(long boundryID)
        {
            List<CO_Zone> lstZones = (from sec in context.CO_Section
                                      join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                      join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                      join cir in context.CO_Circle on div.CircleID equals cir.ID
                                      join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                      where sec.ID == boundryID
                                      orderby zon.Name
                                      select zon).ToList();

            return lstZones;
        }

        public List<CO_Zone> GetZonesForSubDivision(long boundryID)
        {
            List<CO_Zone> lstZones = (from subdiv in context.CO_SubDivision
                                      join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                      join cir in context.CO_Circle on div.CircleID equals cir.ID
                                      join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                      where subdiv.ID == boundryID
                                      orderby zon.Name
                                      select zon).ToList();

            return lstZones;
        }

        public List<CO_Zone> GetZonesForDivision(long boundryID)
        {
            List<CO_Zone> lstZones = (from div in context.CO_Division
                                      join cir in context.CO_Circle on div.CircleID equals cir.ID
                                      join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                      where div.ID == boundryID
                                      orderby zon.Name
                                      select zon).ToList();

            return lstZones;
        }

        public List<CO_Zone> GetZonesForCircle(long boundryID)
        {
            List<CO_Zone> lstZones = (from cir in context.CO_Circle
                                      join zon in context.CO_Zone on cir.ZoneID equals zon.ID
                                      where cir.ID == boundryID
                                      orderby zon.Name
                                      select zon).ToList();

            return lstZones;
        }

        public List<CO_Zone> GetZones(long boundryID)
        {
            List<CO_Zone> lstZones = (from zon in context.CO_Zone
                                      where zon.ID == boundryID
                                      orderby zon.Name
                                      select zon).ToList();

            return lstZones;
        }

        public List<CO_Division> GetDivisions(long BoundryID)
        {
            List<CO_Division> lstDivisions = (from div in context.CO_Division
                                              where div.ID == BoundryID
                                              orderby div.Name
                                              select div).ToList();

            return lstDivisions;
        }

        public List<CO_Circle> GetCircles(long BoundryID)
        {
            List<CO_Circle> lstCir = (from cir in context.CO_Circle
                                      where cir.ID == BoundryID
                                      select cir).ToList();

            return lstCir;
        }

        public List<CO_Circle> GetCirclesForSection(long boundryID)
        {
            List<CO_Circle> lstZones = (from sec in context.CO_Section
                                        join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                        join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                        join cir in context.CO_Circle on div.CircleID equals cir.ID
                                        where sec.ID == boundryID
                                        orderby cir.Name
                                        select cir).ToList();

            return lstZones;
        }

        public List<CO_Circle> GetCirclesForSubDivision(long boundryID)
        {
            List<CO_Circle> lstZones = (from subdiv in context.CO_SubDivision
                                        join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                        join cir in context.CO_Circle on div.CircleID equals cir.ID
                                        where subdiv.ID == boundryID
                                        orderby cir.Name
                                        select cir).ToList();

            return lstZones;
        }

        public List<CO_Circle> GetCirclesForDivision(long boundryID)
        {
            List<CO_Circle> lstZones = (from div in context.CO_Division
                                        join cir in context.CO_Circle on div.CircleID equals cir.ID
                                        where div.ID == boundryID
                                        orderby cir.Name
                                        select cir).ToList();

            return lstZones;
        }

        public List<CO_Division> GetDivisionForSection(long boundryID)
        {
            List<CO_Division> lstZones = (from sec in context.CO_Section
                                          join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                          join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                          where sec.ID == boundryID
                                          orderby div.Name
                                          select div).ToList();

            return lstZones;
        }

        public List<CO_Division> GetDivisionForSubDivision(long boundryID)
        {
            List<CO_Division> lstZones = (from subdiv in context.CO_SubDivision
                                          join div in context.CO_Division on subdiv.DivisionID equals div.ID
                                          where subdiv.ID == boundryID
                                          orderby div.Name
                                          select div).ToList();

            return lstZones;
        }

        public List<CO_Division> GetDivision(long boundryID)
        {
            List<CO_Division> lstZones = (from div in context.CO_Division
                                          where div.ID == boundryID
                                          orderby div.Name
                                          select div).ToList();

            return lstZones;
        }        

        public List<CO_SubDivision> GetSubDivisionForSection(long boundryID)
        {
            List<CO_SubDivision> lstZones = (from sec in context.CO_Section
                                             join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                                             where sec.ID == boundryID
                                             orderby subdiv.Name
                                             select subdiv).ToList();

            return lstZones;
        }

        public List<CO_SubDivision> GetSubDivision(long boundryID)
        {
            List<CO_SubDivision> lstZones = (from subdiv in context.CO_SubDivision
                                             where subdiv.ID == boundryID
                                             orderby subdiv.Name
                                             select subdiv).ToList();

            return lstZones;
        }

        public List<CO_Section> GetSection(long boundryID)
        {
            List<CO_Section> lstZones = (from sec in context.CO_Section
                                         where sec.ID == boundryID
                                         orderby sec.Name
                                         select sec).ToList();
            return lstZones;
        }

        public List<CO_Circle> GetCircleRelatedToZone(long ZoneID)
        {
            List<CO_Circle> lstCircles = (from cir in context.CO_Circle
                                          where cir.ZoneID == ZoneID
                                          orderby cir.Name
                                          select cir).ToList();

            return lstCircles;
        }

        public List<CO_Division> GetDivisionRelatedToCircle(long CircleID)
        {
            List<CO_Division> lstDivisions = (from div in context.CO_Division
                                              where div.CircleID == CircleID
                                              orderby div.Name
                                              select div).ToList();
            return lstDivisions;
        }

        public List<CO_SubDivision> GetSubDivisionForDivision(long DivisionID)
        {
            List<CO_SubDivision> lstSubDivisions = (from sub in context.CO_SubDivision
                                                    where sub.DivisionID == DivisionID
                                                    orderby sub.Name
                                                    select sub).ToList();
            return lstSubDivisions;
        }

        public List<CO_Section> GetSectionForSubDivision(long SubDivID)
        {
            List<CO_Section> lstSections = (from sec in context.CO_Section
                                            where sec.SubDivID == SubDivID
                                            orderby sec.Name
                                            select sec).ToList();
            return lstSections;
        }

        public long? GetUserDesignation(string LoginName)
        {
            long? DesignationID = (from usr in context.UA_Users
                                   where usr.LoginName == LoginName
                                   select usr.DesignationID).FirstOrDefault();
            return DesignationID;
        }

        public UA_IrrigationLevel GetIrriLvlAgainstDesignation(long DesignationID)
        {
            UA_IrrigationLevel LevelDetail = (from des in context.UA_Designations
                                              join irrLvl in context.UA_IrrigationLevel on des.IrrigationLevelID equals irrLvl.ID
                                              where des.ID == DesignationID
                                              select irrLvl).FirstOrDefault();
            return LevelDetail;
        }


    }
}
