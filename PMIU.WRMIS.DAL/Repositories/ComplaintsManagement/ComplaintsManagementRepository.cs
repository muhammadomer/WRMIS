using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.UserAdministration;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.ComplaintsManagement
{
    public class ComplaintsManagementRepository : Repository<CM_Complaint>
    {
        public ComplaintsManagementRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<CM_Complaint>();

        }

        public List<object> GetUnAssignedDesignations(long _OrganizationID)
        {
            List<object> lstUnAssignedDesg = (from s in context.UA_Designations
                                              where (!context.CM_NotificationList.Any(es => (es.DesigID == s.ID)) && s.OrganizationID == _OrganizationID)
                                              select new
                                              {
                                                  ID = s.ID,
                                                  Designation = s.Name
                                              }).Distinct().ToList<object>();

            return lstUnAssignedDesg;
        }

        public List<object> GetAssignedDesignations(long _OrganizationID)
        {
            List<object> lstUnAssignedDesg = (from desg in context.UA_Designations
                                              join nl in context.CM_NotificationList on desg.ID equals nl.DesigID
                                              where desg.OrganizationID == _OrganizationID
                                              //where !context.CM_NotificationList.Any(es => (es.DesigID == s.ID) && (s.OrganizationID == _OrganizationID))
                                              select new
                                              {
                                                  ID = desg.ID,
                                                  Designation = desg.Name
                                              }).ToList<object>();
            return lstUnAssignedDesg;
        }

        public bool DeleteRowByOrganizationID(long _OrganizationID)
        {
            try
            {
                context.CM_NotificationList.RemoveRange(context.CM_NotificationList.Where(c => c.OrgID == _OrganizationID));
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        #region
        public List<CO_Domain> GetDomainsByUserID(long _UserID, long _IrrigationLevelID)
        {
            List<CO_Domain> lstDomain = null;

            if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Section)
            {
                lstDomain = (from al in context.UA_AssociatedLocation
                             join sec in context.CO_Section on al.IrrigationBoundryID equals sec.ID
                             join subdiv in context.CO_SubDivision on sec.SubDivID equals subdiv.ID
                             join div in context.CO_Division on subdiv.DivisionID equals div.ID
                             join domain in context.CO_Domain on div.DomainID equals domain.ID
                             where al.UserID == _UserID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.Section
                             select domain).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision)
            {
                lstDomain = (from al in context.UA_AssociatedLocation
                             join subdiv in context.CO_SubDivision on al.IrrigationBoundryID equals subdiv.ID
                             join div in context.CO_Division on subdiv.DivisionID equals div.ID
                             join domain in context.CO_Domain on div.DomainID equals domain.ID
                             where al.UserID == _UserID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.SubDivision
                             select domain).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {
                lstDomain = (from al in context.UA_AssociatedLocation
                             join div in context.CO_Division on al.IrrigationBoundryID equals div.ID
                             join domain in context.CO_Domain on div.DomainID equals domain.ID
                             where al.UserID == _UserID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division
                             select domain).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle)
            {
                lstDomain = (from al in context.UA_AssociatedLocation
                             join circle in context.CO_Circle on al.IrrigationBoundryID equals circle.ID
                             join div in context.CO_Division on circle.ID equals div.CircleID
                             join domain in context.CO_Domain on div.DomainID equals domain.ID
                             where al.UserID == _UserID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle
                             select domain).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone)
            {
                lstDomain = (from al in context.UA_AssociatedLocation
                             join zone in context.CO_Zone on al.IrrigationBoundryID equals zone.ID
                             join circle in context.CO_Circle on zone.ID equals circle.ZoneID
                             join div in context.CO_Division on circle.ID equals div.CircleID
                             join domain in context.CO_Domain on div.DomainID equals domain.ID
                             where al.UserID == _UserID && al.IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone
                             select domain).Distinct().OrderBy(d => d.Name).ToList();
            }
            else if (_IrrigationLevelID == 0)
            {
                lstDomain = (from domain in context.CO_Domain
                             select domain).ToList();
            }
            return lstDomain;
        }

        public List<CO_Division> GetDivisionsByDomainID(long _DomainID, bool? _IsActive = null)
        {
            List<CO_Division> lstDivision = null;

            lstDivision = (from div in context.CO_Division
                           where div.DomainID == _DomainID && (div.IsActive == _IsActive || _IsActive == null)
                           select div).Distinct().OrderBy(d => d.Name).ToList();
            return lstDivision;

        }

        public bool AddBulkData(DataTable _WTAttachments)
        {
            WRMIS_Entities context = new WRMIS_Entities();
            var param = new SqlParameter("@tblBulkData", SqlDbType.Structured);
            param.Value = _WTAttachments;
            param.TypeName = "dbo.ComplaintComments";
            string sql = String.Format("EXEC {0} {1};", "dbo.CM_SaveBulkCommentsandAttachments", "@tblBulkData");
            context.Database.ExecuteSqlCommand(sql, param);
            return true;
        }

        public bool MarkAsCompalintsResolved(List<CM_Complaint> _lstOfComplaints)
        {
            ContextDB dbADO = new ContextDB();
            foreach (var item in _lstOfComplaints)
            {
                CM_Complaint mdlComplaint = dbADO.Repository<CM_Complaint>().FindById(item.ID);
                mdlComplaint.ComplaintStatusID = 3;
                dbADO.Repository<CM_Complaint>().Update(mdlComplaint);
                dbADO.Save();
            }
            return true;
        }

        #endregion

        /// <summary>
        /// this function return all channels on the babsis of SubDivID
        /// Created On: 21-09-2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetChannelsBySubDivID(long _SubDivID)
        {
            List<CO_Channel> lstChannels = (from cib in context.CO_ChannelIrrigationBoundaries
                                            join c in context.CO_Channel on cib.ChannelID equals c.ID
                                            join s in context.CO_Section on cib.SectionID equals s.ID
                                            where s.SubDivID == _SubDivID
                                            select c).Distinct().OrderBy(d => d.NAME).ToList<CO_Channel>();
            return lstChannels;
        }

        /// <summary>
        /// this function return outlets on the basis of subdivid and channelid
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public List<object> GetOutletsByChannelIDAndSubDivID(long _SubDivID, long _ChannelID)
        {

            var lstSectionIDsComma = string.Join(",", context.CO_Section.Where(p => p.SubDivID == _SubDivID)
                                    .Select(p => p.ID));

            ContextDB dbADO = new ContextDB();
            List<object> lstOutlet;
            var result = dbADO.ExecuteStoredProcedureDataTable("WT_GetChannelIDAndRDsBySectionIDs", lstSectionIDsComma, _ChannelID);
            int MinRD = result.AsEnumerable().Min(r => r.Field<int>("MinRD"));
            int MaxRD = result.AsEnumerable().Max(r => r.Field<int>("MaxRD"));
            lstOutlet = (from outlet in context.CO_ChannelOutlets
                         where (outlet.OutletRD >= MinRD && outlet.OutletRD <= MaxRD) && outlet.ChannelID == _ChannelID
                         select new
                         {
                             ID = outlet.ID,
                             Name = outlet.OutletRD,
                             ChannelSide = outlet.ChannelSide

                         }).ToList().Select(lstout => new
                         {
                             lstout.ID,
                             Name = lstout.Name.HasValue ? Calculations.GetRDText(lstout.Name.Value) + "/" + lstout.ChannelSide : "",
                         }).ToList<object>();
            return lstOutlet;
        }

        /// <summary>
        /// this function return Protection Infrastructures on the basis of Division ID
        /// Created On: 28/09/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_DomainID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetProtectionInfrastructuresByDivisionID(long _DivisionID, long _DomainID, string _Source)
        {
            var lstPortectionInfraStructures = (from pis in context.FO_ProtectionInfrastructure
                                                join st in context.CO_StructureType on pis.InfrastructureTypeID equals st.ID
                                                join sib in context.FO_StructureIrrigationBoundaries on pis.ID equals sib.StructureID
                                                where sib.DivisionID == _DivisionID && st.DomainID == _DomainID && st.Source == _Source
                                                select new
                                                {
                                                    ID = pis.ID,
                                                    Name = pis.InfrastructureName
                                                }).OrderBy(x => x.Name).ToList<dynamic>();
            return lstPortectionInfraStructures;
        }

        /// <summary>
        /// this function return Drains on the basis of Sub Division ID
        /// Created On: 28/09/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <param name="_DomainID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDrainsBySubDivisionID(long _SubDivID, long _DomainID, string _Source)
        {
            var lstDrains = (from d in context.FO_Drain
                             join st in context.CO_StructureType on d.DrainTypeID equals st.ID
                             join sib in context.FO_StructureIrrigationBoundaries on d.ID equals sib.StructureID
                             where st.DomainID == _DomainID && st.Source == _Source && (from s in context.CO_Section
                                                                                        where s.SubDivID == _SubDivID
                                                                                        select s.ID).ToList().Contains((long)sib.SectionID)
                             select new
                             {
                                 ID = d.ID,
                                 Name = d.Name
                             }).OrderBy(x => x.Name).ToList<dynamic>();
            return lstDrains;
        }

        /// <summary>
        /// this function return irrigation boundaries by Division ID
        /// Created On: 03/10/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>dynamic</returns>
        public dynamic GetIrrigtionBoundaryByDivisionID(long _DivisionID)
        {
            var location = (from z in context.CO_Zone
                            join c in context.CO_Circle on z.ID equals c.ZoneID
                            join d in context.CO_Division on c.ID equals d.CircleID
                            where d.ID == _DivisionID
                            select new { Zone = z.ID, Circle = c.ID, Division = d.ID }).Distinct().FirstOrDefault();
            return location;
        }


        public List<long?> GetReportingUsers(long _ManagerID)
        {
            List<long?> lstReportingUsers = (from usrman in context.UA_UserManager
                                             where usrman.ManagerID == _ManagerID
                                             select (long?)usrman.UserID).ToList<long?>();

            if (lstReportingUsers.Count() > 0)
            {
                foreach (int ManagerID in lstReportingUsers)
                {
                    lstReportingUsers = lstReportingUsers.Concat(GetReportingUsers(ManagerID)).ToList<long?>();
                }
            }

            return lstReportingUsers;
        }


        public List<object> GetStatusCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long? _DesignationID)
        {
            List<UA_AssociatedLocation> lstAssociatedLocation = (from al in context.UA_AssociatedLocation
                                                                 where al.UserID == _UserID
                                                                 select al).ToList<UA_AssociatedLocation>();

            long? IrrigationLevelID = lstAssociatedLocation.Select(lal => lal.IrrigationLevelID).FirstOrDefault();
            List<long?> lstBoundryIDs = lstAssociatedLocation.Select(lal => lal.IrrigationBoundryID).ToList<long?>();

            List<object> lstComplaintsCount = null;

            if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone || IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle || IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {

                lstComplaintsCount = (from c in context.CM_Complaint
                                      where (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                            (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null) &&
                                            (lstBoundryIDs.Contains(c.ZoneID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Zone) &&
                                            (lstBoundryIDs.Contains(c.CircleID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Circle) &&
                                            (lstBoundryIDs.Contains(c.DivisionID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Division)
                                      group c by c.ComplaintStatusID into g
                                      select new { Count = g.Count(), Value = g.Key }).ToList<object>();


            }
            else
            {

                List<long> lstComplaintID = context.CM_Complaint.Where(c => c.AssignedToUser == _UserID).Select(c => (long)c.ID).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintAssignmentHistory.Where(cah => cah.AssignedToUser == _UserID).Select(cah => cah.ComplaintID.Value).ToList())).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintMarked.Where(cm => cm.UserID == _UserID).Select(cm => cm.ComplaintID.Value).ToList())).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintNotification.Where(cn => cn.UserID == _UserID).Select(cn => cn.ComplaintID.Value).ToList())).ToList();

                List<CM_Complaint> lstComplaints = (from c in context.CM_Complaint
                                                    where (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                                          (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null)
                                                    select c).ToList<CM_Complaint>();

                lstComplaintsCount = (from c in lstComplaints
                                      where lstComplaintID.Contains(c.ID)
                                      group c by c.ComplaintStatusID into g
                                      select new { Count = g.Count(), Value = g.Key }).ToList<object>();

            }

            // Old code being commented on 29-03-2018 - Start

            //if (_DesignationID == (int)Constants.Designation.ChiefIrrigation || _DesignationID == (int)Constants.Designation.SE)  // for SE and chief all sub ordinated complaints should be visible in graph (23-01-18 (CR))
            //{
            //    List<long?> lstReportingUsers = GetReportingUsers(_UserID);

            //    lstComplaintsCount = (from c in context.CM_Complaint
            //                          join cah in context.CM_ComplaintAssignmentHistory on new
            //                          {
            //                              ID = c.ID,
            //                              CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && lstReportingUsers.Contains(CAHI.AssignedToUser)).Max(CAHI => CAHI.ID))
            //                          } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }
            //                          where lstReportingUsers.Contains(cah.AssignedToUser) && //   cah.AssignedToUser == _UserID &&
            //                                (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) &&
            //                                (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                          group c by c.ComplaintStatusID into g
            //                          select new { Count = g.Count(), Value = g.Key }).ToList<object>();
            //}
            //else
            //{
            //    lstComplaintsCount = (from c in context.CM_Complaint
            //                          join cah in context.CM_ComplaintAssignmentHistory on new
            //                          {
            //                              ID = c.ID,
            //                              CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && CAHI.AssignedToUser == _UserID)
            //                                  .Max(CAHI => CAHI.ID))
            //                          } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }
            //                          where cah.AssignedToUser == _UserID &&
            //                                                       (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) &&
            //                                                       (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                          group c by c.ComplaintStatusID into g
            //                          select new { Count = g.Count(), Value = g.Key }).ToList<object>();
            //}

            // Old code being commented on 29-03-2018 - End

            //List<object> lstComplaintsCount = (from c in context.CM_Complaint
            //                                   join cah in context.CM_ComplaintAssignmentHistory on new
            //                                   {
            //                                       ID = c.ID,
            //                                       CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && CAHI.AssignedToUser == _UserID)
            //                                           .Max(CAHI => CAHI.ID))
            //                                   } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }




            //                                   //         join ssd in
            //                                   //             (from ssd3 in context.CM_ComplaintAssignmentHistory
            //                                   //              where
            //                                   //(from ssd2 in context.CM_ComplaintAssignmentHistory
            //                                   // where ssd2.AssignedToUser == _UserID
            //                                   // group ssd2 by ssd2.ComplaintID into cd
            //                                   // select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
            //                                   //              select ssd3) on c.ID equals ssd.ComplaintID





            //                                   //join CMark in context.CM_ComplaintMarked on c.ID equals CMark.ComplaintID into lmark
            //                                   //from rmark in lmark.DefaultIfEmpty()
            //                                   where cah.AssignedToUser == _UserID && (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) && (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                                   //&& rmark.UserID == _UserID
            //                                   group c by c.ComplaintStatusID into g
            //                                   select new { Count = g.Count(), Value = g.Key }).ToList<object>();
            //select c).Distinct().OrderBy(d => d.NAME).ToList<CO_Channel>();
            return lstComplaintsCount;
        }

        public List<object> GetStatusCount(long _UserID, DateTime? _FromDate, DateTime? _ToDate)
        {
            List<object> lstComplaintsCount = (from r in context.CM_Complaint
                                               where (DbFunctions.TruncateTime(r.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                                    (DbFunctions.TruncateTime(r.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null)
                                               group r by r.ComplaintStatusID into g
                                               select new { Count = g.Count(), Value = g.Key }).ToList<object>();

            // Old code being commented on 29-03-2018 - Start

            //var lstComplaintsCount = (from r in context.CM_Complaint
            //                          join CMark in context.CM_ComplaintMarked on r.ID equals CMark.ComplaintID
            //                          where CMark.UserID == _UserID && (DbFunctions.TruncateTime(r.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) && (DbFunctions.TruncateTime(r.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                          group r by r.ComplaintStatusID into g
            //                          select new { Count = g.Count(), Value = g.Key }).ToList<object>();
            ////select c).Distinct().OrderBy(d => d.NAME).ToList<CO_Channel>();

            // Old code being commented on 29-03-2018 - End

            return lstComplaintsCount;
        }

        public List<object> GetComplaintBySourceCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long? _DesignationID)
        {
            List<UA_AssociatedLocation> lstAssociatedLocation = (from al in context.UA_AssociatedLocation
                                                                 where al.UserID == _UserID
                                                                 select al).ToList<UA_AssociatedLocation>();

            long? IrrigationLevelID = lstAssociatedLocation.Select(lal => lal.IrrigationLevelID).FirstOrDefault();
            List<long?> lstBoundryIDs = lstAssociatedLocation.Select(lal => lal.IrrigationBoundryID).ToList<long?>();

            List<object> lstComplaintsCount = null;

            if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone || IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle || IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {

                lstComplaintsCount = (from c in context.CM_Complaint
                                      where (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                            (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null) &&
                                            (lstBoundryIDs.Contains(c.ZoneID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Zone) &&
                                            (lstBoundryIDs.Contains(c.CircleID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Circle) &&
                                            (lstBoundryIDs.Contains(c.DivisionID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Division)
                                      group c by new { c.ComplaintSourceID, c.ComplaintStatusID } into g
                                      select new { g.Key.ComplaintSourceID, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            }
            else
            {

                List<long> lstComplaintID = context.CM_Complaint.Where(c => c.AssignedToUser == _UserID).Select(c => (long)c.ID).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintAssignmentHistory.Where(cah => cah.AssignedToUser == _UserID).Select(cah => cah.ComplaintID.Value).ToList())).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintMarked.Where(cm => cm.UserID == _UserID).Select(cm => cm.ComplaintID.Value).ToList())).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintNotification.Where(cn => cn.UserID == _UserID).Select(cn => cn.ComplaintID.Value).ToList())).ToList();

                List<CM_Complaint> lstComplaints = (from c in context.CM_Complaint
                                                    where (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                                          (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null)
                                                    select c).ToList<CM_Complaint>();

                lstComplaintsCount = (from c in lstComplaints
                                      where lstComplaintID.Contains(c.ID)
                                      group c by new { c.ComplaintSourceID, c.ComplaintStatusID } into g
                                      select new { g.Key.ComplaintSourceID, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            }

            // Old code being commented on 29-03-2018 - Start

            //if (_DesignationID == (int)Constants.Designation.ChiefIrrigation || _DesignationID == (int)Constants.Designation.SE)
            //{
            //    List<long?> lstReportingUsers = GetReportingUsers(_UserID);

            //    lstComplaintsCount = (from c in context.CM_Complaint
            //                          join cah in context.CM_ComplaintAssignmentHistory on new
            //                          {
            //                              ID = c.ID,
            //                              CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && lstReportingUsers.Contains(CAHI.AssignedToUser))
            //                                  .Max(CAHI => CAHI.ID))
            //                          } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }
            //                          where lstReportingUsers.Contains(cah.AssignedToUser) && //cah.AssignedToUser == _UserID &&
            //                                (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) &&
            //                                (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                          group c by new { c.ComplaintSourceID, c.ComplaintStatusID } into g
            //                          select new { g.Key.ComplaintSourceID, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();
            //}
            //else
            //{
            //    lstComplaintsCount = (from c in context.CM_Complaint
            //                          join cah in context.CM_ComplaintAssignmentHistory on new
            //                          {
            //                              ID = c.ID,
            //                              CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && CAHI.AssignedToUser == _UserID)
            //                                  .Max(CAHI => CAHI.ID))
            //                          } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }
            //                          where cah.AssignedToUser == _UserID &&
            //                                (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) &&
            //                                (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                          group c by new { c.ComplaintSourceID, c.ComplaintStatusID } into g
            //                          select new { g.Key.ComplaintSourceID, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();
            //}

            // Old code being commented on 29-03-2018 - End

            //List<object> lstComplaintsCount = (from c in context.CM_Complaint
            //                                   //   join ch in context.CM_ComplaintAssignmentHistory on r.ID equals ch.ComplaintID
            //                                   join cah in context.CM_ComplaintAssignmentHistory on new
            //                                   {
            //                                       ID = c.ID,
            //                                       CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && CAHI.AssignedToUser == _UserID)
            //                                           .Max(CAHI => CAHI.ID))
            //                                   } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }


            //                                   //         join ssd in
            //                                   //             (from ssd3 in context.CM_ComplaintAssignmentHistory
            //                                   //              where
            //                                   //(from ssd2 in context.CM_ComplaintAssignmentHistory
            //                                   // where ssd2.AssignedToUser == _UserID
            //                                   // group ssd2 by ssd2.ComplaintID into cd
            //                                   // select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
            //                                   //              select ssd3) on r.ID equals ssd.ComplaintID
            //                                   //         join CMark in context.CM_ComplaintMarked on r.ID equals CMark.ComplaintID
            //                                   where cah.AssignedToUser == _UserID && (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) && (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                                   //&& CMark.UserID == _UserID
            //                                   group c by new { c.ComplaintSourceID, c.ComplaintStatusID } into g
            //                                   select new { g.Key.ComplaintSourceID, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();
            return lstComplaintsCount;
        }

        public List<object> GetAllComplaintBySourceCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate)
        {
            List<object> lstComplaintsCount = (from r in context.CM_Complaint
                                               where (DbFunctions.TruncateTime(r.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                                    (DbFunctions.TruncateTime(r.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null)
                                               group r by new { r.ComplaintSourceID, r.ComplaintStatusID } into g
                                               select new { g.Key.ComplaintSourceID, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            // Old code being commented on 29-03-2018 - Start

            //var lstComplaintsCount = (from r in context.CM_Complaint
            //                          join CMark in context.CM_ComplaintMarked on r.ID equals CMark.ComplaintID
            //                          where CMark.UserID == _UserID && (DbFunctions.TruncateTime(r.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) && (DbFunctions.TruncateTime(r.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                          group r by new { r.ComplaintSourceID, r.ComplaintStatusID } into g
            //                          select new { g.Key.ComplaintSourceID, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            // Old code being commented on 29-03-2018 - End

            return lstComplaintsCount;
        }

        public List<object> GetComplaintTypeCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long _ComplaintTypeID, long? _DesignationID)
        {
            List<UA_AssociatedLocation> lstAssociatedLocation = (from al in context.UA_AssociatedLocation
                                                                 where al.UserID == _UserID
                                                                 select al).ToList<UA_AssociatedLocation>();

            long? IrrigationLevelID = lstAssociatedLocation.Select(lal => lal.IrrigationLevelID).FirstOrDefault();
            List<long?> lstBoundryIDs = lstAssociatedLocation.Select(lal => lal.IrrigationBoundryID).ToList<long?>();

            List<object> lstComplaintsCount = null;

            if (IrrigationLevelID == (long)Constants.IrrigationLevelID.Zone || IrrigationLevelID == (long)Constants.IrrigationLevelID.Circle || IrrigationLevelID == (long)Constants.IrrigationLevelID.Division)
            {

                lstComplaintsCount = (from c in context.CM_Complaint
                                      join t in context.CM_ComplaintType on c.ComplaintTypeID equals t.ID
                                      where c.ComplaintTypeID == _ComplaintTypeID &&
                                           (lstBoundryIDs.Contains(c.ZoneID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Zone) &&
                                           (lstBoundryIDs.Contains(c.CircleID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Circle) &&
                                           (lstBoundryIDs.Contains(c.DivisionID) || IrrigationLevelID != (long)Constants.IrrigationLevelID.Division) &&
                                           (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                           (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null)
                                      group c by new { c.ComplaintTypeID, c.ComplaintStatusID, t.Name } into g
                                      select new { g.Key.ComplaintTypeID, g.Key.Name, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            }
            else
            {

                List<long> lstComplaintID = context.CM_Complaint.Where(c => c.AssignedToUser == _UserID).Select(c => (long)c.ID).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintAssignmentHistory.Where(cah => cah.AssignedToUser == _UserID).Select(cah => cah.ComplaintID.Value).ToList())).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintMarked.Where(cm => cm.UserID == _UserID).Select(cm => cm.ComplaintID.Value).ToList())).ToList();
                lstComplaintID = (lstComplaintID.Union(context.CM_ComplaintNotification.Where(cn => cn.UserID == _UserID).Select(cn => cn.ComplaintID.Value).ToList())).ToList();

                List<CM_Complaint> lstComplaints = (from c in context.CM_Complaint
                                                    where (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                                          (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null) &&
                                                          c.ComplaintTypeID == _ComplaintTypeID
                                                    select c).ToList<CM_Complaint>();

                lstComplaintsCount = (from c in lstComplaints
                                      where lstComplaintID.Contains(c.ID)
                                      group c by new { c.ComplaintTypeID, c.ComplaintStatusID, c.CM_ComplaintType.Name } into g
                                      select new { g.Key.ComplaintTypeID, g.Key.Name, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            }

            // Old code being commented on 29-03-2018 - Start

            //if (_DesigationID == (int)Constants.Designation.ChiefIrrigation || _DesigationID == (int)Constants.Designation.SE)
            //{
            //    List<long?> lstReportingUsers = GetReportingUsers(_UserID);

            //    lstComplaintsCount = (from c in context.CM_Complaint
            //                          join t in context.CM_ComplaintType on c.ComplaintTypeID equals t.ID
            //                          join cah in context.CM_ComplaintAssignmentHistory on new
            //                          {
            //                              ID = c.ID,
            //                              CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && lstReportingUsers.Contains(CAHI.AssignedToUser))
            //                                  .Max(CAHI => CAHI.ID))
            //                          } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }
            //                          where lstReportingUsers.Contains(cah.AssignedToUser) && //cah.AssignedToUser == _UserID &&
            //                          (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) &&
            //                          (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                          && c.ComplaintTypeID == _ComplaintTypeID
            //                          group c by new { c.ComplaintTypeID, c.ComplaintStatusID, t.Name } into g
            //                          select new { g.Key.ComplaintTypeID, g.Key.Name, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();
            //}
            //else
            //{
            //    lstComplaintsCount = (from c in context.CM_Complaint
            //                          join t in context.CM_ComplaintType on c.ComplaintTypeID equals t.ID
            //                          join cah in context.CM_ComplaintAssignmentHistory on new
            //                          {
            //                              ID = c.ID,
            //                              CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && CAHI.AssignedToUser == _UserID)
            //                                  .Max(CAHI => CAHI.ID))
            //                          } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }
            //                          where cah.AssignedToUser == _UserID &&
            //                          (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) &&
            //                          (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                          && c.ComplaintTypeID == _ComplaintTypeID
            //                          group c by new { c.ComplaintTypeID, c.ComplaintStatusID, t.Name } into g
            //                          select new { g.Key.ComplaintTypeID, g.Key.Name, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();
            //}

            // Old code being commented on 29-03-2018 - End

            //List<object> lstComplaintsCount = (from c in context.CM_Complaint
            //                                   //  join ch in context.CM_ComplaintAssignmentHistory on r.ID equals ch.ComplaintID

            //                                   join t in context.CM_ComplaintType on c.ComplaintTypeID equals t.ID
            //                                   join cah in context.CM_ComplaintAssignmentHistory on new
            //                                   {
            //                                       ID = c.ID,
            //                                       CAHID = (context.CM_ComplaintAssignmentHistory.Where(CAHI => CAHI.ComplaintID == c.ID && CAHI.AssignedToUser == _UserID)
            //                                           .Max(CAHI => CAHI.ID))
            //                                   } equals new { ID = cah.ComplaintID.Value, CAHID = cah.ID }

            //                                   //         join ssd in
            //                                   //             (from ssd3 in context.CM_ComplaintAssignmentHistory
            //                                   //              where
            //                                   //(from ssd2 in context.CM_ComplaintAssignmentHistory
            //                                   // where ssd2.AssignedToUser == _UserID
            //                                   // group ssd2 by ssd2.ComplaintID into cd
            //                                   // select cd.Max(s => s.ID)).ToList().Contains(ssd3.ID)
            //                                   //              select ssd3) on c.ID equals ssd.ComplaintID
            //                                   //         join CMark in context.CM_ComplaintMarked on c.ID equals CMark.ComplaintID
            //                                   where cah.AssignedToUser == _UserID && (DbFunctions.TruncateTime(c.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) && (DbFunctions.TruncateTime(c.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null)
            //                                   && c.ComplaintTypeID == _ComplaintTypeID //&& CMark.UserID == _UserID
            //                                   group c by new { c.ComplaintTypeID, c.ComplaintStatusID, t.Name } into g
            //                                   select new { g.Key.ComplaintTypeID, g.Key.Name, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            return lstComplaintsCount;
        }

        public List<object> GetAllComplaintTypeCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long _ComplaintTypeID)
        {
            List<object> lstComplaintsCount = (from r in context.CM_Complaint
                                               join t in context.CM_ComplaintType on r.ComplaintTypeID equals t.ID
                                               where (DbFunctions.TruncateTime(r.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || _FromDate == null) &&
                                                    (DbFunctions.TruncateTime(r.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || _ToDate == null) &&
                                                    r.ComplaintTypeID == _ComplaintTypeID
                                               group r by new { r.ComplaintTypeID, r.ComplaintStatusID, t.Name } into g
                                               select new { g.Key.ComplaintTypeID, g.Key.Name, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            // Old code being commented on 29-03-2018 - Start

            //var lstComplaintsCount = (from r in context.CM_Complaint
            //                          join t in context.CM_ComplaintType on r.ComplaintTypeID equals t.ID
            //                          join CMark in context.CM_ComplaintMarked on r.ID equals CMark.ComplaintID
            //                          where CMark.UserID == _UserID && (DbFunctions.TruncateTime(r.ComplaintDate) >= DbFunctions.TruncateTime(_FromDate) || DbFunctions.TruncateTime(_FromDate) == null) && (DbFunctions.TruncateTime(r.ComplaintDate) <= DbFunctions.TruncateTime(_ToDate) || DbFunctions.TruncateTime(_ToDate) == null) && r.ComplaintTypeID == _ComplaintTypeID
            //                          group r by new { r.ComplaintTypeID, r.ComplaintStatusID, t.Name } into g
            //                          select new { g.Key.ComplaintTypeID, g.Key.Name, g.Key.ComplaintStatusID, Count = g.Count() }).ToList<object>();

            // Old code being commented on 29-03-2018 - End

            return lstComplaintsCount;
        }

        /// <summary>
        /// this function return Divisions By Village ID
        /// Created On: 04/10/2016
        /// </summary>
        /// <param name="_VillageID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionsByVillageID(long _VillageID)
        {
            long District = (from d in context.CO_District
                             join t in context.CO_Tehsil on d.ID equals t.DistrictID
                             join v in context.CO_Village on t.ID equals v.TehsilID
                             where v.ID == _VillageID
                             select d.ID).FirstOrDefault();

            List<dynamic> lstDistrictDivision = context.CO_DistrictDivision.Where(x => x.DistrictID == District).Select(x => new { x.CO_Division.ID, x.CO_Division.Name }).OrderBy(x => x.Name).ToList<dynamic>();
            return lstDistrictDivision;
        }

        /// <summary>
        /// this function return Division By Outlet ID
        /// Created On: 05/10/2016
        /// </summary>
        /// <param name="_OutletID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionByOutletID(long _OutletID)
        {
            CO_ChannelOutlets mdlChannelOutlet = context.CO_ChannelOutlets.Where(x => x.ID == _OutletID).FirstOrDefault();
            CO_ChannelIrrigationBoundaries mdlChannelIrrigationBoundaries = context.CO_ChannelIrrigationBoundaries.Where(x => x.ChannelID == mdlChannelOutlet.ChannelID && x.SectionToRD >= mdlChannelOutlet.OutletRD && x.SectionRD <= mdlChannelOutlet.OutletRD).FirstOrDefault();

            List<dynamic> Division = (from d in context.CO_Division
                                      join sd in context.CO_SubDivision on d.ID equals sd.DivisionID
                                      join s in context.CO_Section on sd.ID equals s.SubDivID
                                      where s.ID == mdlChannelIrrigationBoundaries.SectionID
                                      select new { ID = d.ID, Name = d.Name }).OrderBy(x => x.Name).ToList<dynamic>();
            return Division;
        }

        /// <summary>
        /// this function return protection structure by village ID
        /// Created On: 05/10/2016
        /// </summary>
        /// <param name="_VillageID"></param>
        /// <param name="_DomainID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetProtectionStructureByVillageID(long _VillageID, long _DomainID)
        {
            List<dynamic> ProtectionStructures = (from sab in context.FO_StructureAdminBoundaries
                                                  join pis in context.FO_ProtectionInfrastructure on sab.StructureID equals pis.ID
                                                  join st in context.CO_StructureType on pis.InfrastructureTypeID equals st.ID
                                                  where sab.VillageID == _VillageID && st.DomainID == _DomainID
                                                  select new { ID = pis.ID, Name = pis.InfrastructureName }).OrderBy(x => x.Name).ToList<dynamic>();
            return ProtectionStructures;
        }

        public dynamic GetSubDivisionCircleZoneAndUserIDByDivisionID(long _DivisionID, long _IrrgLevelID, long _DesignationID)
        {
            var query = (from div in context.CO_Division
                         join cir in context.CO_Circle on div.CircleID equals cir.ID
                         join zone in context.CO_Zone on cir.ZoneID equals zone.ID
                         join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                         join al in context.UA_AssociatedLocation on div.ID equals al.IrrigationBoundryID
                         where al.IrrigationBoundryID == _DivisionID && al.IrrigationLevelID == _IrrgLevelID && al.DesignationID == _DesignationID
                         select new
                         {
                             DivisionID = div.ID,
                             CircleID = cir.ID,
                             ZoneID = zone.ID,
                             SubDivisionID = subdiv.ID,
                             UserID = al.UserID,
                             DomainID = div.DomainID,
                             DesignationID = al.DesignationID
                         }).Distinct();
            dynamic lst = (from div in context.CO_Division
                           join cir in context.CO_Circle on div.CircleID equals cir.ID
                           join zone in context.CO_Zone on cir.ZoneID equals zone.ID
                           join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                           join al in context.UA_AssociatedLocation on div.ID equals al.IrrigationBoundryID
                           where al.IrrigationBoundryID == _DivisionID && al.IrrigationLevelID == _IrrgLevelID && al.DesignationID == _DesignationID
                           select new
                           {
                               DivisionID = div.ID,
                               CircleID = cir.ID,
                               ZoneID = zone.ID,
                               SubDivisionID = subdiv.ID,
                               UserID = al.UserID,
                               DomainID = div.DomainID,
                               DesignationID = al.DesignationID
                           }).Distinct().FirstOrDefault();
            return lst;

        }

        public dynamic GetComplaintInformationByComplaintID(long _ComplaintID)
        {
            dynamic lst = (from com in context.CM_Complaint
                           join cs in context.CM_ComplaintSource on com.ComplaintSourceID equals cs.ID
                           join dom in context.CO_Domain on com.DomainID equals dom.ID
                           join div in context.CO_Division on com.DivisionID equals div.ID
                           join ct in context.CM_ComplaintType on com.ComplaintTypeID equals ct.ID
                           where com.ID == _ComplaintID
                           select new
                           {
                               ComplaintID = com.ID,
                               ComplaintNumber = com.ComplaintNumber,
                               ComplaintSource = cs.Name,
                               Domain = dom.Name,
                               Division = div.Name,
                               ComplaintDate = com.ComplaintDate,
                               ComplainantName = com.ComplainantName,
                               ComplaintType = ct.Name,
                               ComplaiantCell = com.MobilePhone,
                               ComplainantAddress = com.Address,
                               DivisionID = div.ID,
                               ComplaintDetails = com.ComplaintDetails,
                               PMIUFileNo = com.PMIUFileNo,
                               ResponseDuration = com.ResponseDuration,
                               Attachment = com.Attachment,
                               Status = com.CM_ComplaintStatus.Name
                           }).Distinct().FirstOrDefault();
            return lst;
        }

        public List<dynamic> GetAllCommentsByComplaintID(long _ComplaintID)
        {
            List<dynamic> lstComplaintComments = (from com in context.CM_ComplaintComments
                                                  join desg in context.UA_Designations on com.UserDesigID equals desg.ID
                                                  join comp in context.CM_Complaint on com.ComplaintID equals comp.ID
                                                  where com.ComplaintID == _ComplaintID
                                                  select new
                                                  {
                                                      ID = com.ID,
                                                      ComplaintID = com.ComplaintID,
                                                      ComplaintNumber = comp.ComplaintNumber,
                                                      Comments = com.Comments,
                                                      Designation = desg.Name,
                                                      Attachment = com.Attachment,
                                                      Date = com.CommentsDateTime
                                                  }).OrderByDescending(x => x.Date).ToList<dynamic>();
            return lstComplaintComments;
        }

        public List<dynamic> DDLGetNotifySDO(long _DivisionID)
        {
            List<dynamic> SDODivisionList = (from div in context.CO_Division
                                             join subdiv in context.CO_SubDivision on div.ID equals subdiv.DivisionID
                                             join al in context.UA_AssociatedLocation on subdiv.ID equals al.IrrigationBoundryID
                                             join user in context.UA_Users on al.UserID equals user.ID
                                             where div.ID == _DivisionID && al.IrrigationLevelID == 4
                                             select new { ID = al.UserID, Name = subdiv.Name + "-" + user.FirstName + " " + user.LastName }).OrderBy(x => x.Name).ToList<dynamic>();
            return SDODivisionList;
        }

        public UA_AssociatedLocation GetUserByDivisionID(long _DivisionID)
        {
            UA_AssociatedLocation mdlObject = (from AL in context.UA_AssociatedLocation
                                               join Usr in context.UA_Users on AL.UserID equals Usr.ID
                                               where AL.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division && AL.IrrigationBoundryID == _DivisionID
                                                      && AL.DesignationID == (long)Constants.Designation.XEN && Usr.IsActive == true
                                               select AL).FirstOrDefault();

            return mdlObject;
        }


        public bool ComplaintNumberExist(string _Number)
        {
            bool Result = true;
            var res = (from com in context.CM_Complaint
                       where com.ComplaintNumber.Contains(_Number)
                       select com.ComplaintNumber).FirstOrDefault();

            if (res == null)
                Result = false;

            return Result;

        }






    }
}