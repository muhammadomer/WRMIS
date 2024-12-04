using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.ComplaintsManagement;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.ComplaintsManagement
{
    public class ComplaintsManagementDAL
    {
        ContextDB db = new ContextDB();

        /// <summary>
        /// This function returns list of all Complaints Type.
        /// Created On 06-09-2016.
        /// </summary> 
        /// <returns>List<CM_ComplaintType></returns>
        public List<CM_ComplaintType> GetAllComplaintsType()
        {
            List<CM_ComplaintType> lstComplaintsType = db.Repository<CM_ComplaintType>().GetAll().Where(a => a.IsActive == true).OrderBy(n => n.Name).ToList<CM_ComplaintType>();
            return lstComplaintsType;
        }

        /// <summary>
        /// This Function Adds Complaint Type into the Database.
        /// Created on 06-09-2016
        /// </summary>
        /// <param name="_ComplaintType"></param>
        /// <returns>bool</returns>
        public bool AddComplaintType(CM_ComplaintType _ComplaintType)
        {
            db.Repository<CM_ComplaintType>().Insert(_ComplaintType);
            db.Save();
            return true;
        }

        /// <summary>
        /// This Function Updates Complaint Type into the Database.
        /// Created on 06-09-2016
        /// </summary>
        /// <param name="_ComplaintType"></param>
        /// <returns>bool</returns>
        public bool UpdateComplaintType(CM_ComplaintType _ComplaintType)
        {
            CM_ComplaintType mdlComplaintType = db.Repository<CM_ComplaintType>().FindById(_ComplaintType.ID);
            mdlComplaintType.Name = _ComplaintType.Name;
            mdlComplaintType.ResponseTime = _ComplaintType.ResponseTime;
            mdlComplaintType.Description = _ComplaintType.Description;
            mdlComplaintType.ModifiedBy = _ComplaintType.ModifiedBy;
            mdlComplaintType.ModifiedDate = _ComplaintType.ModifiedDate;

            db.Repository<CM_ComplaintType>().Update(mdlComplaintType);
            db.Save();
            return true;
        }

        /// <summary>
        /// This Function Deletes Complaint Type from Database.
        /// Created on 06-09-2016
        /// </summary>
        /// <param name="_ComplaintTypeID"></param>
        /// <returns>bool</returns>
        public bool DeleteComplaintType(long _ComplaintTypeID)
        {
            db.Repository<CM_ComplaintType>().Delete(_ComplaintTypeID);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function gets Complaint Type by Complaint Type Name
        /// Created On:06-09-2016
        /// </summary>
        /// <param name="_ComplaintTypeName"></param>
        /// <returns>CM_ComplaintType</returns>
        public CM_ComplaintType GetComplaintTypeByName(string _ComplaintTypeName)
        {
            CM_ComplaintType mdlComplaintType = db.Repository<CM_ComplaintType>().GetAll().Where(z => z.Name.Trim().ToUpper() == _ComplaintTypeName.Trim().ToUpper()).FirstOrDefault();
            return mdlComplaintType;
        }

        /// <summary>
        /// this functions checks in Complaint table for given Complaint Type ID
        /// Created On:06-09-2016
        /// </summary>
        /// <param name="_ComplaintTypeID"></param>
        /// <returns>bool</returns>
        public bool IsComplaintTypeIDExists(long _ComplaintTypeID)
        {
            bool qIsExists = db.Repository<CM_Complaint>().GetAll().Any(c => c.ComplaintTypeID == _ComplaintTypeID);
            return qIsExists;
        }

        #region Additional Accessibility

        public List<UA_Organization> GetAllOrganization()
        {
            List<UA_Organization> lstOrganization = db.Repository<UA_Organization>().GetAll().ToList();
            return lstOrganization;
        }

        public List<object> GetUnAssignedDesignations(long _OrganizationID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetUnAssignedDesignations(_OrganizationID);
        }
        public List<object> GetAssignedDesignations(long _OrganizationID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetAssignedDesignations(_OrganizationID);
        }

        public bool SetAdditionalAccessibility(long _OrganizationID, List<long> _lstDesignations, int _UserID)
        {

            DeleteRowByOrganizationID(_OrganizationID);

            foreach (long item in _lstDesignations)
            {
                CM_NotificationList nl = new CM_NotificationList();

                nl.OrgID = _OrganizationID;
                nl.DesigID = item;
                nl.IsActive = true;
                nl.CreatedDate = DateTime.Now;
                nl.CreatedBy = _UserID;
                db.Repository<CM_NotificationList>().Insert(nl);
            }
            db.Save();
            return true;

        }

        public bool DeleteRowByOrganizationID(long _OrganizationID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().DeleteRowByOrganizationID(_OrganizationID);
        }
        #endregion

        #region
        public List<CO_Domain> GetDomainsByUserID(long _UserID, long _IrrigationLevelID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetDomainsByUserID(_UserID, _IrrigationLevelID);
        }
        public List<CO_Division> GetDivisionsByDomainID(long _DomainID, bool? _IsActive = null)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetDivisionsByDomainID(_DomainID, _IsActive);
        }

        public List<CM_ComplaintSource> GetComplaintSource()
        {
            List<CM_ComplaintSource> lstComplaintSource = db.Repository<CM_ComplaintSource>().GetAll().OrderBy(n => n.Name).ToList<CM_ComplaintSource>();
            return lstComplaintSource;
        }

        public List<CM_ComplaintStatus> GetComplaintStatus()
        {
            List<CM_ComplaintStatus> lstComplaintStatus = db.Repository<CM_ComplaintStatus>().GetAll().OrderBy(n => n.ID).ToList<CM_ComplaintStatus>();
            return lstComplaintStatus;
        }

        public List<CM_ComplaintType> GetComplaintType()
        {
            List<CM_ComplaintType> lstComplaintType = db.Repository<CM_ComplaintType>().GetAll().OrderBy(n => n.ID).ToList<CM_ComplaintType>();
            return lstComplaintType;
        }

        public IEnumerable<DataRow> GetComplaintCases(long _UserID, string _ComplaintNumber, string _ComplainantName, string _ComplainantCell, long _DomainID,
             long _DivisionID, long _ComplaintSourceID, long _StatusID, long _ActionID, long _ComplaintTypeID, DateTime? _FromDate, DateTime? _ToDate,
            long? _IrrigationLevelID, bool _OtherThenPMIUStaff, long _RoleID)
        {
            return db.ExecuteDataSet("CM_SearchComplaint", _UserID, _ComplaintNumber, _ComplainantName, _ComplainantCell, _DomainID, _DivisionID,
               _ComplaintSourceID, _StatusID, _ActionID, _ComplaintTypeID, _FromDate, _ToDate, _IrrigationLevelID, _OtherThenPMIUStaff, _RoleID);
        }


        public bool AddComplaintAsFavorite(long _RowID, long _UserID)
        {
            bool qIsExists = db.Repository<CM_ComplaintMarked>().GetAll().Any(c => c.ComplaintID == _RowID && c.Starred == true && c.UserID == _UserID);
            bool qIsExist = db.Repository<CM_ComplaintMarked>().GetAll().Any(c => c.ComplaintID == _RowID && c.UserID == _UserID);
            // CM_ComplaintMarked commark = db.Repository<CM_ComplaintMarked>().FindById(_RowID);

            if (qIsExist)
            {
                if (qIsExists)
                {
                    //commark.MarkRead = false;
                    db.Repository<CM_ComplaintMarked>().GetAll().Where(x => x.ComplaintID == _RowID && x.UserID == _UserID).ToList().ForEach(c => c.Starred = false);

                }
                //else if (qIsExist)
                //{

                    //     CM_ComplaintMarked commarks = db.Repository<CM_ComplaintMarked>().FindById(_CompMark.ComplaintID);
                //     commarks.Starred = true;
                //     db.Save();
                //     return true;

                    //}
                else
                {
                    // CM_ComplaintMarked commark = db.Repository<CM_ComplaintMarked>().FindById(_CompMark.ComplaintID);
                    //commark.MarkRead = false;
                    db.Repository<CM_ComplaintMarked>().GetAll().Where(x => x.ComplaintID == _RowID && x.UserID == _UserID).ToList().ForEach(c => c.Starred = true);
                    //db.Repository<CM_ComplaintMarked>().Insert(_CompMark);
                    //db.Save();
                    //return true;

                }
                db.Save();
                return true;
            }
            else
            {
                return false;
            }


        }

        public List<CM_Complaint> GetComplaintInfoByID(List<long> _lstofID)
        {
            // var numbers = _lstofID.Split(',').Select(Int64.Parse).ToList();
            //  return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetComplaintInfoByID(_lstofID);
            List<CM_Complaint> lstComplaints = db.Repository<CM_Complaint>().GetAll().Where(x => _lstofID.Contains(x.ID)).ToList();
            return lstComplaints;
        }

        public List<CM_Complaint> GetComplaintInfoByID(List<string> _lstofID)
        {
            // var numbers = _lstofID.Split(',').Select(Int64.Parse).ToList();
            //  return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetComplaintInfoByID(_lstofID);
            List<CM_Complaint> lstComplaints = db.Repository<CM_Complaint>().GetAll().Where(x => _lstofID.Contains(x.ComplaintNumber)).ToList();
            return lstComplaints;
        }

        public bool AddBulkData(DataTable _WTAttachments)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().AddBulkData(_WTAttachments);
        }


        public bool MarkAsCompalintsResolved(List<CM_Complaint> _lstOfComplaints)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().MarkAsCompalintsResolved(_lstOfComplaints);
        }

        public bool AddQuickComplaintData(CM_ComplaintComments _CompComments)
        {
            db.Repository<CM_ComplaintComments>().Insert(_CompComments);
            db.Save();
            return true;
        }
        public bool CompalintResolvedByID(long _ComplaintID)
        {
            CM_Complaint mdlComplaint = db.Repository<CM_Complaint>().FindById(_ComplaintID);
            mdlComplaint.ComplaintStatusID = 3;
            db.Repository<CM_Complaint>().Update(mdlComplaint);
            db.Save();
            return true;
        }

        #endregion

        /// <summary>
        /// This function return specific domains for Add complaint and criteria for domain is specified in where clause
        /// Created On:15-09-2016
        /// </summary>
        /// <returns></returns>
        public List<CO_Domain> GetDomainsForAddComplaint()
        {
            List<CO_Domain> lstDomain = db.Repository<CO_Domain>().GetAll().Where(x => x.Name.Trim().ToUpper() == "D & F" || x.Name.Trim().ToUpper() == "DEVELOPMENT" || x.Name.Trim().ToUpper() == "IRRIGATION").OrderBy(x => x.Name).ToList<CO_Domain>();
            return lstDomain;
        }

        /// <summary>
        /// this function return structureTypes on the basis of Domain ID
        /// Created On:15-09-2016
        /// </summary>
        /// <param name="_DomainID"></param>
        /// <returns>List<CO_StructureType></returns>
        public List<CO_StructureType> GetStructureTypeByDomainID(long _DomainID)
        {
            List<CO_StructureType> lstStructure = db.Repository<CO_StructureType>().GetAll().Where(x => x.DomainID == _DomainID).OrderBy(x => x.Name).ToList<CO_StructureType>();
            return lstStructure;
        }

        /// <summary>
        /// this function return Domain on the basis of Domain ID
        /// Created On:15-09-2016
        /// </summary>
        /// <param name="_DomainID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionByDomainID(long _DomainID)
        {
            List<CO_Division> lstDivision = db.Repository<CO_Division>().GetAll().Where(x => x.DomainID == _DomainID).OrderBy(x => x.Name).ToList<CO_Division>();
            return lstDivision;
        }

        /// <summary>
        /// this function return all channels on the babsis of SubDivID
        /// Created On: 21-09-2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetChannelsBySubDivID(long _SubDivID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetChannelsBySubDivID(_SubDivID);
        }

        /// <summary>
        /// this function return outlets on the basis of subdivid and channelid
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletsByChannelIDAndSubDivID(long _SubDivID, long _ChannelID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetOutletsByChannelIDAndSubDivID(_SubDivID, _ChannelID);
        }

        /// <summary>
        /// this function get all designations in notification list
        /// Created on: 22/09/2016
        /// </summary>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetAllNotifications()
        {
            List<dynamic> lstNotifications = db.Repository<CM_NotificationList>().GetAll().Where(x => x.DesigID != (long)Constants.Designation.DeputyDirectorHelpline).Select(x => new { ID = x.UA_Designations.ID, Name = "&nbsp&nbsp&nbsp" + x.UA_Designations.Name + ", " + x.UA_Organization.Name }).ToList<dynamic>();
            return lstNotifications;
        }

        /// <summary>
        /// this function add complaint in Database
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_Complaint"></param>
        /// <returns>bool</returns>
        public long AddComplaint(CM_Complaint _Complaint)
        {
            db.Repository<CM_Complaint>().Insert(_Complaint);
            db.Save();
            long ComplaintID = _Complaint.ID;
            return ComplaintID;
        }

        /// <summary>
        /// this function add complaint assignment history in database
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_ComplaintAssignmentHistory"></param>
        /// <returns>bool</returns>
        public bool AddComplaintAssignmentHistory(CM_ComplaintAssignmentHistory _ComplaintAssignmentHistory)
        {
            db.Repository<CM_ComplaintAssignmentHistory>().Insert(_ComplaintAssignmentHistory);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function add complaint Notification in database
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_ComplaintNotification"></param>
        /// <returns>bool</returns>
        public bool AddComplaintNotification(CM_ComplaintNotification _ComplaintNotification)
        {
            db.Repository<CM_ComplaintNotification>().Insert(_ComplaintNotification);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function add complaint Marked in database
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_ComplaintMarked"></param>
        /// <returns>bool</returns>
        public bool AddComplaintMarked(CM_ComplaintMarked _ComplaintMarked)
        {
            db.Repository<CM_ComplaintMarked>().Insert(_ComplaintMarked);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return latest Complaint number + 1
        /// Created On: 23/09/2016
        /// </summary>
        /// <returns>string</returns>
        public string GetNewComplaintID()
        {
            CM_Complaint mdlComplaint = db.Repository<CM_Complaint>().GetAll().OrderByDescending(x => x.ID).FirstOrDefault();
            string ComplaintNumber;
            if (mdlComplaint == null)
            {
                ComplaintNumber = "000001";
            }
            else
            {
                ComplaintNumber = mdlComplaint.ComplaintNumber;
            }
            string ModifiedComplaintNumber = string.Empty;
            long IncrementedComplaintNumber = 0;

            for (int i = 0; i < ComplaintNumber.Length; i++)
            {
                if (Char.IsDigit(ComplaintNumber[i]))
                    ModifiedComplaintNumber += ComplaintNumber[i];
            }

            if (ModifiedComplaintNumber.Length > 0)
                IncrementedComplaintNumber = int.Parse(ModifiedComplaintNumber);
            if (mdlComplaint != null)
                IncrementedComplaintNumber = IncrementedComplaintNumber + 1;

            string FinalIncrementedValue = IncrementedComplaintNumber.ToString("000000");
            return FinalIncrementedValue;
        }

        /// <summary>
        /// this function return Complaint source on the basis of complaint ID
        /// Created On: 23/09/2016
        /// </summary>
        /// <param name="_ComplaintSourceID"></param>
        /// <returns>CM_ComplaintSource</returns>
        public CM_ComplaintSource GetComplaintSourceForComplaintID(long _ComplaintSourceID)
        {
            CM_ComplaintSource mdlComplaintSource = db.Repository<CM_ComplaintSource>().GetAll().Where(x => x.ID == _ComplaintSourceID).FirstOrDefault();
            return mdlComplaintSource;
            //select * from UA_AssociatedLocation al JOIN UA_Users u on al.UserID=u.ID  where IrrigationLevelID=3
        }

        /// <summary>
        /// this function return user by divisionID
        /// Created Date: 26/09/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>UA_AssociatedLocation</returns>
        public UA_AssociatedLocation GetUserByDivisionID(long _DivisionID)
        {
            //UA_AssociatedLocation mdlAssociatedLocation = db.Repository<UA_AssociatedLocation>().GetAll().Where(x => x.IrrigationLevelID == (long)Constants.IrrigationLevelID.Division && x.IrrigationBoundryID == _DivisionID && x.DesignationID == (long)Constants.Designation.XEN).FirstOrDefault();
            //return mdlAssociatedLocation;
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetUserByDivisionID(_DivisionID);
        }


        /// <summary>
        /// this function return organization by designation ID
        /// Created On: 27/09/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>CM_NotificationList</returns>
        public CM_NotificationList GetOrganizationByDesignationID(long _DesignationID)
        {
            CM_NotificationList mdlNotificationList = db.Repository<CM_NotificationList>().GetAll().Where(x => x.DesigID == _DesignationID).FirstOrDefault();
            return mdlNotificationList;
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
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetProtectionInfrastructuresByDivisionID(_DivisionID, _DomainID, _Source);
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
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetDrainsBySubDivisionID(_SubDivID, _DomainID, _Source);
        }

        /// <summary>
        /// this function return all user on the basis of Designation ID and Irrigation Level ID and Irrigation Boundary ID and Organization ID
        /// Created On:29/09/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        ///<param name="_IrrigationLevelID"></param>
        ///<param name="_IrrigationBoundryID"></param>
        /// <param name="_OrganizationID"></param>
        /// <returns>List<long></returns>
        public List<long> GetUsersForPmiuAndIrrigation(long _DesignationID, long _IrrigationLevelID, long _IrrigationBoundryID, long _OrganizationID)
        {
            List<long> lstUsers = db.Repository<UA_AssociatedLocation>().GetAll().Where(x => x.IrrigationBoundryID == _IrrigationBoundryID
                && x.IrrigationLevelID == _IrrigationLevelID
                && x.UA_Users.DesignationID == _DesignationID
                && x.UA_Users.UA_Designations.UA_Organization.ID == _OrganizationID).Select(x => x.UserID).ToList<long>();
            return lstUsers;
        }

        /// <summary>
        /// This function return  users for Pida and Secretariat based on Designation ID and organization ID
        /// Created On: 3/10/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <param name="_OrganizationID"></param>
        /// <returns>List<long></returns>
        public List<long> GetUsersForPidaAndSecretariat(long _DesignationID, long _OrganizationID)
        {
            List<long> lstUsers = db.Repository<UA_Users>().GetAll().Where(x => x.DesignationID == _DesignationID && x.UA_Designations.UA_Organization.ID == _OrganizationID).Select(x => x.ID).ToList<long>();
            return lstUsers;
        }

        /// <summary>
        /// this function return irrigation boundaries by Division ID
        /// Created On: 03/10/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>dynamic</returns>
        public dynamic GetIrrigtionBoundaryByDivisionID(long _DivisionID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetIrrigtionBoundaryByDivisionID(_DivisionID);
        }

        /// <summary>
        /// this function return all villages based on TehsilID
        /// Created On: 04/10/2016
        /// </summary>
        /// <param name="_TehsilID"></param>
        /// <returns>List<CO_Village></returns>
        public List<CO_Village> GetVillagesByTehsilID(long _TehsilID)
        {
            List<CO_Village> lstVillage = db.Repository<CO_Village>().GetAll().Where(x => x.TehsilID == _TehsilID).OrderBy(x => x.Name).ToList<CO_Village>();
            return lstVillage;
        }

        /// <summary>
        /// this function return Divisions By Village ID
        /// Created On: 04/10/2016
        /// </summary>
        /// <param name="_VillageID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionsByVillageID(long _VillageID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetDivisionsByVillageID(_VillageID);
        }

        /// <summary>
        /// this function return channels by Village ID
        /// Created On: 04/10/2016
        /// </summary>
        /// <param name="_VillageID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetChannelsByVillageID(long _VillageID)
        {
            List<dynamic> lstChannelAdminBoundaries = db.Repository<CO_ChannelAdminBoundries>().GetAll().Where(x => x.VillageID == _VillageID).Select(x => new { x.CO_Channel.ID, x.CO_Channel.NAME }).OrderBy(x => x.NAME).ToList<dynamic>();
            return lstChannelAdminBoundaries;
        }

        /// <summary>
        /// this function return Outlets By Village ID
        /// Created On: 05/10/2016
        /// </summary>
        /// <param name="_VillageID"></param>
        /// <returns>List<CO_ChannelOutlets></returns>
        public List<CO_ChannelOutlets> GetOutletsByVillageIDAndChannelID(long _VillageID, long _ChannelID)
        {
            CO_ChannelAdminBoundries mdlChannelAdminBoundries = db.Repository<CO_ChannelAdminBoundries>().GetAll().Where(x => x.VillageID == _VillageID && x.ChannelID == _ChannelID).FirstOrDefault();
            List<CO_ChannelOutlets> lstChannelOutlets = db.Repository<CO_ChannelOutlets>().GetAll().Where(x => x.ChannelID == mdlChannelAdminBoundries.ChannelID && x.OutletRD >= mdlChannelAdminBoundries.FromRD && x.OutletRD <= mdlChannelAdminBoundries.ToRD).OrderBy(x => x.Name).ToList<CO_ChannelOutlets>();
            return lstChannelOutlets;
        }

        /// <summary>
        /// this function return Division By Outlet ID
        /// Created On: 05/10/2016
        /// </summary>
        /// <param name="_OutletID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionByOutletID(long _OutletID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetDivisionByOutletID(_OutletID);
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
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetProtectionStructureByVillageID(_VillageID, _DomainID);
        }


        #region
        public List<object> GetStatusCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long? _DesignationID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetStatusCounts(_UserID, _FromDate, _ToDate, _DesignationID);

        }

        public List<object> GetStatusCount(long _UserID, DateTime? _FromDate, DateTime? _ToDate)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetStatusCount(_UserID, _FromDate, _ToDate);

        }

        public List<object> GetComplaintBySourceCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long? _DesignationID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetComplaintBySourceCounts(_UserID, _FromDate, _ToDate, _DesignationID);

        }

        public List<object> GetAllComplaintBySourceCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetAllComplaintBySourceCounts(_UserID, _FromDate, _ToDate);

        }


        public List<object> GetComplaintTypeCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long _ComplaintTypeID, long? _DesignationID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetComplaintTypeCounts(_UserID, _FromDate, _ToDate, _ComplaintTypeID, _DesignationID);

        }

        public List<object> GetAllComplaintTypeCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long _ComplaintTypeID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetAllComplaintTypeCounts(_UserID, _FromDate, _ToDate, _ComplaintTypeID);

        }
        #endregion

        public dynamic GetSubDivisionCircleZoneAndUserIDByDivisionID(long _DivisionID, long _IrrgLevelID, long _DesignationID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetSubDivisionCircleZoneAndUserIDByDivisionID(_DivisionID, _IrrgLevelID, _DesignationID);
        }

        public long AddAutoGeneratedComplaintData(CM_Complaint _Complaint)
        {
            db.Repository<CM_Complaint>().Insert(_Complaint);
            db.Save();
            long ComplaintID = _Complaint.ID;
            return ComplaintID;
        }

        public bool AddAutoGeneratedAssignmentHistoryData(CM_ComplaintAssignmentHistory _ComplaintAssignmentHistory)
        {
            db.Repository<CM_ComplaintAssignmentHistory>().Insert(_ComplaintAssignmentHistory);
            db.Save();
            return true;
        }

        public bool AddAutoGeneratedComplaintMarked(CM_ComplaintMarked _CompMarked)
        {
            db.Repository<CM_ComplaintMarked>().Insert(_CompMarked);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return complaint by ID
        /// Created On: 14/10/2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>CM_Complaint</returns>
        public CM_Complaint GetComplaintByID(long _ComplaintID)
        {
            CM_Complaint mdlComplaint = db.Repository<CM_Complaint>().GetAll().Where(x => x.ID == _ComplaintID).FirstOrDefault();
            return mdlComplaint;
        }

        /// <summary>
        /// this function updates complaint
        /// Created On: 19/10/2016
        /// </summary>
        /// <param name="_Complaint"></param>
        /// <returns>bool</returns>
        public bool UpdateComplaint(CM_Complaint _Complaint)
        {
            CM_Complaint mdlComplaint = db.Repository<CM_Complaint>().FindById(_Complaint.ID);

            mdlComplaint.ComplaintSourceID = _Complaint.ComplaintSourceID;
            mdlComplaint.ComplaintStatusID = _Complaint.ComplaintStatusID;
            mdlComplaint.ComplaintTypeID = _Complaint.ComplaintTypeID;
            mdlComplaint.ResponseDuration = _Complaint.ResponseDuration;
            mdlComplaint.ComplaintDate = _Complaint.ComplaintDate;
            mdlComplaint.ComplaintNumber = _Complaint.ComplaintNumber;
            mdlComplaint.ComplainantName = _Complaint.ComplainantName;
            mdlComplaint.Address = _Complaint.Address;
            mdlComplaint.Phone = _Complaint.Phone;
            mdlComplaint.MobilePhone = _Complaint.MobilePhone;
            mdlComplaint.ComplaintDetails = _Complaint.ComplaintDetails;
            mdlComplaint.PMIUFileNo = _Complaint.PMIUFileNo;

            if (_Complaint.Attachment != null)
            {
                mdlComplaint.Attachment = _Complaint.Attachment;
            }

            mdlComplaint.AssignedToUser = _Complaint.AssignedToUser;
            mdlComplaint.AssignedToDesig = _Complaint.AssignedToDesig;
            mdlComplaint.AssignedDate = _Complaint.AssignedDate;
            mdlComplaint.DomainID = _Complaint.DomainID;
            mdlComplaint.StructureTypeID = _Complaint.StructureTypeID;
            mdlComplaint.ZoneID = _Complaint.ZoneID;
            mdlComplaint.CircleID = _Complaint.CircleID;
            mdlComplaint.DivisionID = _Complaint.DivisionID;
            mdlComplaint.SubDivID = _Complaint.SubDivID;
            mdlComplaint.DistrictID = _Complaint.DistrictID;
            mdlComplaint.TehsilID = _Complaint.TehsilID;
            mdlComplaint.VillageID = _Complaint.VillageID;
            mdlComplaint.ChannelID = _Complaint.ChannelID;
            mdlComplaint.StructureID = _Complaint.StructureID;
            mdlComplaint.RD = _Complaint.RD;
            mdlComplaint.UserID = _Complaint.UserID;
            mdlComplaint.UserDesigID = _Complaint.UserDesigID;
            mdlComplaint.ModifiedBy = _Complaint.ModifiedBy;
            mdlComplaint.ModifiedDate = _Complaint.ModifiedDate;

            db.Repository<CM_Complaint>().Update(mdlComplaint);
            db.Save();
            return true;
        }

        /// <summary>
        /// This Function Deletes Complaint from Complaint Assignment History.
        /// Created on: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>bool</returns>
        public bool DeleteComplaintAssignmentHistory(long _ComplaintAssignmentHistoryID)
        {
            db.Repository<CM_ComplaintAssignmentHistory>().Delete(_ComplaintAssignmentHistoryID);
            db.Save();
            return true;
        }

        /// <summary>
        /// This Function Deletes Complaint from Complaint Marked.
        /// Created on: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>bool</returns>
        public bool DeleteComplaintMarked(long _ComplaintMarkedID)
        {
            db.Repository<CM_ComplaintMarked>().Delete(_ComplaintMarkedID);
            db.Save();
            return true;
        }

        /// <summary>
        /// This Function Deletes Complaint from Complaint Notification.
        /// Created on: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>bool</returns>
        public bool DeleteComplaintNotification(long _ComplaintNotificationID)
        {
            db.Repository<CM_ComplaintNotification>().Delete(_ComplaintNotificationID);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return list of Complaint Assignment History by Complaint ID
        /// Created On: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<CM_ComplaintAssignmentHistory></returns>
        public List<CM_ComplaintAssignmentHistory> GetComplaintAssignmentHistoryByComplaintID(long _ComplaintID)
        {
            List<CM_ComplaintAssignmentHistory> lstComplaintAssignmentHistory = db.Repository<CM_ComplaintAssignmentHistory>().GetAll().Where(x => x.ComplaintID == _ComplaintID).ToList<CM_ComplaintAssignmentHistory>();
            return lstComplaintAssignmentHistory;
        }

        /// <summary>
        /// this function return list of Complaint marked by complaint ID
        /// Created On: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<CM_ComplaintMarked></returns>
        public List<CM_ComplaintMarked> GetComplaintMarkedByComplaintID(long _ComplaintID)
        {
            List<CM_ComplaintMarked> lstComplaintMarked = db.Repository<CM_ComplaintMarked>().GetAll().Where(x => x.ComplaintID == _ComplaintID).ToList<CM_ComplaintMarked>();
            return lstComplaintMarked;
        }

        /// <summary>
        /// this function return list of Complaint Notification by complaint ID
        /// Created On: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<CM_ComplaintMarked></returns>
        public List<CM_ComplaintNotification> GetComplaintNotificationByComplaintID(long _ComplaintID)
        {
            List<CM_ComplaintNotification> lstComplaintNotification = db.Repository<CM_ComplaintNotification>().GetAll().Where(x => x.ComplaintID == _ComplaintID).ToList<CM_ComplaintNotification>();
            return lstComplaintNotification;
        }
        public dynamic GetComplaintInformationByComplaintID(long _ComplaintID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetComplaintInformationByComplaintID(_ComplaintID);
        }

        public List<dynamic> GetAllCommentsByComplaintID(long _ComplaintID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().GetAllCommentsByComplaintID(_ComplaintID);
        }

        /// <summary>
        /// this function return all designations whom the specific complaint has been assigned
        /// Created On: 21/10/2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<long?></returns>
        public List<long?> GetDesignationsForListByComplaintID(long _ComplaintID)
        {
            List<long?> lstDesignationIDs = db.Repository<CM_ComplaintNotification>().GetAll().Where(x => x.ComplaintID == _ComplaintID).Distinct().Select(x => x.UserDesigID).ToList<long?>();
            return lstDesignationIDs;
        }

        public List<dynamic> DDLGetNotifySDO(long _DivisionID)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().DDLGetNotifySDO(_DivisionID);
        }

        public List<CM_ComplaintComments> GetAllComplaintCommentsByID(long _CommentsID)
        {
            List<CM_ComplaintComments> lstComplaintsComments = db.Repository<CM_ComplaintComments>().GetAll().Where(n => n.ID == _CommentsID).ToList<CM_ComplaintComments>();
            return lstComplaintsComments;
        }
        /// <summary>
        /// this function return list of complaints marked on the basis of complaint ID
        /// Created On: 26/10/2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<CM_ComplaintMarked></returns>
        public bool UpdateComplaintMarkedAndComplaint(long _ComplaintID, long _UserID, long _DesignationID)
        {
            db.Repository<CM_ComplaintMarked>().GetAll().Where(x => x.ComplaintID == _ComplaintID && x.UserID == _UserID).ToList().ForEach(c => c.MarkRead = true);
            if (Convert.ToInt64(Constants.Designation.XEN) == _DesignationID)
            {
                db.Repository<CM_Complaint>().GetAll().Where(x => x.ID == _ComplaintID && x.ComplaintStatusID != 3).ToList().ForEach(c => c.ComplaintStatusID = 2);
            }
            db.Save();
            return true;
        }

        /// <summary>
        /// this function updates complaint Marked
        /// Created On: 26/10/2016
        /// </summary>
        /// <param name="_Complaint"></param>
        /// <returns>bool</returns>
        public bool UpdateComplaintMarked(CM_ComplaintMarked _ComplaintMarked)
        {
            CM_ComplaintMarked mdlComplaintMarked = db.Repository<CM_ComplaintMarked>().FindById(_ComplaintMarked.ID);

            mdlComplaintMarked.ComplaintID = _ComplaintMarked.ComplaintID;
            mdlComplaintMarked.UserID = _ComplaintMarked.UserID;
            mdlComplaintMarked.UserDesigID = _ComplaintMarked.UserDesigID;
            mdlComplaintMarked.Starred = _ComplaintMarked.Starred;
            mdlComplaintMarked.MarkRead = _ComplaintMarked.MarkRead;
            mdlComplaintMarked.ModifiedBy = _ComplaintMarked.ModifiedBy;
            mdlComplaintMarked.ModifiedDate = _ComplaintMarked.ModifiedDate;

            db.Repository<CM_ComplaintMarked>().Update(mdlComplaintMarked);
            db.Save();
            return true;
        }

        /// <summary>
        /// this function return Complaint Type By ID
        /// Created On: 27/10/2016
        /// </summary>
        /// <param name="_ComplaintTypeID"></param>
        /// <returns></returns>
        public CM_ComplaintType GetComplaintTypeByID(long _ComplaintTypeID)
        {
            CM_ComplaintType mdlComplaintType = db.Repository<CM_ComplaintType>().GetAll().Where(x => x.ID == _ComplaintTypeID).FirstOrDefault();
            return mdlComplaintType;
        }

        public bool GetComplaintMarkedByUserID(long _UserID, long _ComplaintID)
        {
            bool qIsCompExists = false;
            qIsCompExists = db.Repository<CM_ComplaintMarked>().GetAll().Any(c => c.ComplaintID == _ComplaintID && c.UserID == _UserID);
            return qIsCompExists;

        }

        /// <summary>
        /// This function return Complaint Sources except auto generated
        /// Created On: 02/11/2016
        /// </summary>
        /// <returns></returns>
        public List<CM_ComplaintSource> GetComplaintSourceForAddComplaint()
        {
            List<CM_ComplaintSource> lstComplaintSource = db.Repository<CM_ComplaintSource>().GetAll().Where(x => x.ShortCode != "AG").OrderBy(n => n.Name).ToList<CM_ComplaintSource>();
            return lstComplaintSource;
        }

        /// <summary>
        /// This function return Complaint Detail used for Public Website
        /// Created On: 15/11/2016
        /// </summary>
        /// <returns>IEnumerable<DataRow></returns>
        public DataSet GetComplaintInformation_SearchForPublicWebSite(string _ComplaintNumber, string _MobilePhone)
        {
            return db.ExecuteStoredProcedureDataSet("CM_SearchForPublicWebSite", _ComplaintNumber==string.Empty ? null : _ComplaintNumber, _MobilePhone==string.Empty ? null : _MobilePhone);
        }

        /// <summary>
        /// This function Get Users By Designation ID
        /// Created On: 18/11/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>List<long></returns>
        public List<long> GetUsersByDesignationID(long _DesignationID)
        {
            List<long> lstUsers = db.Repository<UA_Users>().GetAll().Where(x => x.DesignationID == _DesignationID).Select(x => x.ID).ToList<long>();
            return lstUsers;
        }

        public bool UpdateComplaintStatusByComplaintID(long _ComplaintID)
        {
            db.Repository<CM_Complaint>().GetAll().Where(x => x.ID == _ComplaintID).ToList().ForEach(c => c.ComplaintStatusID = 2);
            db.Save();
            return true;
        }
        public bool InsertComplanintEntryInSMSTable(UA_SMSNotification _mdlSMS)
        {
            db.Repository<UA_SMSNotification>().Insert(_mdlSMS);
            db.Save();
            return true;
        }

        public long GetMaxIDofCompalintType()
        {
            long MaxID = db.Repository<CM_ComplaintType>().GetAll().Max(x => x.ID);
            return MaxID;
        }
        public bool IsComplaintResolved(long _ComplaintID)
        {
            bool qIsExists = db.Repository<CM_Complaint>().GetAll().Any(c => c.ID == _ComplaintID && c.ComplaintStatusID == 3);
            return qIsExists;
        }
        public DataSet GetTailStatusLastTenDays(long _ChannelID)
        {
            return db.ExecuteStoredProcedureDataSet("Proc_IF_GetTailStatusLast10Days", _ChannelID);
        }

        public bool ComplaintNumberExist(string _Number)
        {
            return db.ExtRepositoryFor<ComplaintsManagementRepository>().ComplaintNumberExist(_Number);
        }
    }
}
