using PMIU.WRMIS.BLL.Notifications;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.ComplaintsManagement;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PMIU.WRMIS.BLL.ComplaintsManagement
{
    public class ComplaintsManagementBLL : BaseBLL
    {
        /// <summary>
        /// This function returns list of all Complaints Type.
        /// Created On 06-09-2016.
        /// </summary> 
        /// <returns>List<CM_ComplaintType></returns>
        public List<CM_ComplaintType> GetAllComplaintsType()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetAllComplaintsType();
        }

        /// <summary>
        /// This Function Adds Complaint Type into the Database.
        /// Created on 06-09-2016
        /// </summary>
        /// <param name="_ComplaintType"></param>
        /// <returns>bool</returns>
        public bool AddComplaintType(CM_ComplaintType _ComplaintType)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddComplaintType(_ComplaintType);
        }

        /// <summary>
        /// This Function Updates Complaint Type into the Database.
        /// Created on 06-09-2016
        /// </summary>
        /// <param name="_ComplaintType"></param>
        /// <returns>bool</returns>
        public bool UpdateComplaintType(CM_ComplaintType _ComplaintType)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.UpdateComplaintType(_ComplaintType);
        }

        /// <summary>
        /// This Function Deletes Complaint Type from Database.
        /// Created on 06-09-2016
        /// </summary>
        /// <param name="_ComplaintTypeID"></param>
        /// <returns>bool</returns>
        public bool DeleteComplaintType(long _ComplaintTypeID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.DeleteComplaintType(_ComplaintTypeID);
        }

        /// <summary>
        /// this function gets Complaint Type by Complaint Type Name
        /// Created On:06-09-2016
        /// </summary>
        /// <param name="_ComplaintTypeName"></param>
        /// <returns>CM_ComplaintType</returns>
        public CM_ComplaintType GetComplaintTypeByName(string _ComplaintTypeName)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintTypeByName(_ComplaintTypeName);
        }

        /// <summary>
        /// this functions checks in Complaint table for given Complaint Type ID
        /// Created On:06-09-2016
        /// </summary>
        /// <param name="_ComplaintTypeID"></param>
        /// <returns>bool</returns>
        public bool IsComplaintTypeIDExists(long _ComplaintTypeID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.IsComplaintTypeIDExists(_ComplaintTypeID);
        }

        #region Additional Accessibility

        public List<UA_Organization> GetAllOrganization()
        {
            ComplaintsManagementDAL dalOrganization = new ComplaintsManagementDAL();
            return dalOrganization.GetAllOrganization();
        }

        public List<object> GetUnAssignedDesignations(long _OrganizationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetUnAssignedDesignations(_OrganizationID);
        }

        public List<object> GetAssignedDesignations(long _OrganizationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetAssignedDesignations(_OrganizationID);
        }

        public bool SetAdditionalAccessibility(long _OrganizationID, List<long> _lstDesignations, int _UserID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.SetAdditionalAccessibility(_OrganizationID, _lstDesignations, _UserID);

        }
        public bool DeleteRowByOrganizationID(long _OrganizationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.DeleteRowByOrganizationID(_OrganizationID);
        }
        #endregion

        #region Search Complain

        public List<CO_Domain> GetDomainsByUserID(long _UserID, long _IrrigationLevelID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetDomainsByUserID(_UserID, _IrrigationLevelID);
        }

        public List<CO_Division> GetDivisionsByDomainID(long _DomainID, bool? _IsActive = null)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetDivisionsByDomainID(_DomainID, _IsActive);
        }

        public List<CM_ComplaintSource> GetComplaintSource()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintSource();
        }

        public List<CM_ComplaintStatus> GetComplaintStatus()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintStatus();
        }

        public List<CM_ComplaintType> GetComplaintType()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintType();
        }

        public IEnumerable<DataRow> GetComplaintCases(long _UserID, string _ComplaintNumber, string _ComplainantName, string _ComplainantCell, long _DomainID,
              long _DivisionID, long _ComplaintSourceID, long _StatusID, long _ActionID, long _ComplaintTypeID, DateTime? _FromDate, DateTime? _ToDate,
             long? _IrrigationLevelID, bool _OtherThenPMIUStaff, long _RoleID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintCases(_UserID, _ComplaintNumber, _ComplainantName, _ComplainantCell, _DomainID, _DivisionID,
                _ComplaintSourceID, _StatusID, _ActionID, _ComplaintTypeID, _FromDate, _ToDate, _IrrigationLevelID, _OtherThenPMIUStaff, _RoleID);
        }

        public bool AddComplaintAsFavorite(long _RowID, long _UserID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddComplaintAsFavorite(_RowID, _UserID);
        }


        public List<CM_Complaint> GetComplaintInfoByID(List<string> _lstComplaintNumber)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintInfoByID(_lstComplaintNumber);
        }

        public List<CM_Complaint> GetComplaintInfoByID(List<long> _lstofID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintInfoByID(_lstofID);
        }

        public bool AddBulkData(DataTable _BulkData)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddBulkData(_BulkData);
        }


        public bool MarkAsCompalintsResolved(List<CM_Complaint> _lstOfComplaints)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.MarkAsCompalintsResolved(_lstOfComplaints);
        }

        public bool AddQuickComplaintData(CM_ComplaintComments _CompComments)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddQuickComplaintData(_CompComments);
        }

        public bool CompalintResolvedByID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.CompalintResolvedByID(_ComplaintID);
        }

        #endregion

        /// <summary>
        /// This function return specific domains for Add complaint
        /// Created On:15-09-2016
        /// </summary>
        /// <returns></returns>
        public List<CO_Domain> GetDomainsForAddComplaint()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetDomainsForAddComplaint();
        }

        /// <summary>
        /// this function return structureTypes on the basis of Domain ID
        /// Created On:15-09-2016
        /// </summary>
        /// <param name="_DomainID"></param>
        /// <returns>List<CO_StructureType></returns>
        public List<CO_StructureType> GetStructureTypeByDomainID(long _DomainID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetStructureTypeByDomainID(_DomainID);
        }

        /// <summary>
        /// this function return Domain on the basis of Domain ID
        /// Created On:15-09-2016
        /// </summary>
        /// <param name="_DomainID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionByDomainID(long _DomainID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetDivisionByDomainID(_DomainID);
        }

        /// <summary>
        /// this function return all channels on the babsis of SubDivID
        /// Created On: 21-09-2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <returns>List<CO_Channel></returns>
        public List<CO_Channel> GetChannelsBySubDivID(long _SubDivID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetChannelsBySubDivID(_SubDivID);
        }

        #region Dashboards

        public List<object> GetStatusCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long? _DesignationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetStatusCounts(_UserID, _FromDate, _ToDate, _DesignationID);
        }

        public List<object> GetStatusCount(long _UserID, DateTime? _FromDate, DateTime? _ToDate)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetStatusCount(_UserID, _FromDate, _ToDate);
        }

        public List<object> GetComplaintBySourceCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long? _DesignationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintBySourceCounts(_UserID, _FromDate, _ToDate, _DesignationID);
        }

        public List<object> GetAllComplaintBySourceCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetAllComplaintBySourceCounts(_UserID, _FromDate, _ToDate);
        }


        public List<object> GetComplaintTypeCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long _ComplaintTypeID, long? _DesignationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintTypeCounts(_UserID, _FromDate, _ToDate, _ComplaintTypeID, _DesignationID);
        }

        public List<object> GetAllComplaintTypeCounts(long _UserID, DateTime? _FromDate, DateTime? _ToDate, long _ComplaintTypeID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetAllComplaintTypeCounts(_UserID, _FromDate, _ToDate, _ComplaintTypeID);
        }


        #endregion

        /// <summary>
        /// this function return outlets on the basis of subdivid and channelid
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_SubDivID"></param>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletsByChannelIDAndSubDivID(long _SubDivID, long _ChannelID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetOutletsByChannelIDAndSubDivID(_SubDivID, _ChannelID);
        }

        /// <summary>
        /// this function get all designations in notification list
        /// Created on: 22/09/2016
        /// </summary>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetAllNotifications()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetAllNotifications();
        }

        /// <summary>
        /// this function add complaint in Database
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_Complaint"></param>
        /// <returns>bool</returns>
        public long AddComplaint(CM_Complaint _Complaint)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddComplaint(_Complaint);
        }

        /// <summary>
        /// this function add complaint assignment history in database
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_ComplaintAssignmentHistory"></param>
        /// <returns>bool</returns>
        public bool AddComplaintAssignmentHistory(CM_ComplaintAssignmentHistory _ComplaintAssignmentHistory)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddComplaintAssignmentHistory(_ComplaintAssignmentHistory);
        }

        /// <summary>
        /// this function add complaint Notification in database
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_ComplaintNotification"></param>
        /// <returns>bool</returns>
        public bool AddComplaintNotification(CM_ComplaintNotification _ComplaintNotification)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddComplaintNotification(_ComplaintNotification);
        }

        /// <summary>
        /// this function add complaint Marked in database
        /// Created On: 22/09/2016
        /// </summary>
        /// <param name="_ComplaintMarked"></param>
        /// <returns>bool</returns>
        public bool AddComplaintMarked(CM_ComplaintMarked _ComplaintMarked)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddComplaintMarked(_ComplaintMarked);
        }

        /// <summary>
        /// this function return latest Complaint number + 1
        /// Created On: 23/09/2016
        /// </summary>
        /// <returns>string</returns>
        public string GetNewComplaintID()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetNewComplaintID();
        }

        /// <summary>
        /// this function return Complaint source on the basis of complaint ID
        /// Created On: 23/09/2016
        /// </summary>
        /// <param name="_ComplaintSourceID"></param>
        /// <returns>CM_ComplaintSource</returns>
        public CM_ComplaintSource GetComplaintSourceForComplaintID(long _ComplaintSourceID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintSourceForComplaintID(_ComplaintSourceID);
        }

        /// <summary>
        /// this function return user by divisionID
        /// Created Date: 26/09/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>UA_AssociatedLocation</returns>
        public UA_AssociatedLocation GetUserByDivisionID(long _DivisionID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetUserByDivisionID(_DivisionID);
        }

        /// <summary>
        /// this function return organization by designation ID
        /// Created On: 27/09/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>CM_NotificationList</returns>
        public CM_NotificationList GetOrganizationByDesignationID(long _DesignationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetOrganizationByDesignationID(_DesignationID);
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
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetProtectionInfrastructuresByDivisionID(_DivisionID, _DomainID, _Source);
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
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetDrainsBySubDivisionID(_SubDivID, _DomainID, _Source);
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
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetUsersForPmiuAndIrrigation(_DesignationID, _IrrigationLevelID, _IrrigationBoundryID, _OrganizationID);
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
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetUsersForPidaAndSecretariat(_DesignationID, _OrganizationID);
        }

        /// <summary>
        /// this function return irrigation boundaries by Division ID
        /// Created On: 03/10/2016
        /// </summary>
        /// <param name="_DivisionID"></param>
        /// <returns>dynamic</returns>
        public dynamic GetIrrigtionBoundaryByDivisionID(long _DivisionID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetIrrigtionBoundaryByDivisionID(_DivisionID);
        }

        /// <summary>
        /// this function return all villages based on TehsilID
        /// Created On: 04/10/2016
        /// </summary>
        /// <param name="_TehsilID"></param>
        /// <returns>List<CO_Village></returns>
        public List<CO_Village> GetVillagesByTehsilID(long _TehsilID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetVillagesByTehsilID(_TehsilID);
        }

        /// <summary>
        /// this function return Divisions By Village ID
        /// Created On: 04/10/2016
        /// </summary>
        /// <param name="_VillageID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetDivisionsByVillageID(long _VillageID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetDivisionsByVillageID(_VillageID);
        }

        /// <summary>
        /// this function return channels by Village ID
        /// Created On: 04/10/2016
        /// </summary>
        /// <param name="_VillageID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetChannelsByVillageID(long _VillageID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetChannelsByVillageID(_VillageID);
        }

        /// <summary>
        /// this function return Outlets By Village ID
        /// Created On: 05/10/2016
        /// </summary>
        /// <param name="_VillageID"></param>
        /// <returns>List<CO_ChannelOutlets></returns>
        public List<CO_ChannelOutlets> GetOutletsByVillageIDAndChannelID(long _VillageID, long _ChannelID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetOutletsByVillageIDAndChannelID(_VillageID, _ChannelID);
        }

        /// <summary>
        /// this function return Division By Outlet ID
        /// Created On: 05/10/2016
        /// </summary>
        /// <param name="_OutletID"></param>
        /// <returns>dynamic</returns>
        public List<dynamic> GetDivisionByOutletID(long _OutletID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetDivisionByOutletID(_OutletID);
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
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetProtectionStructureByVillageID(_VillageID, _DomainID);
        }

        public dynamic GetSubDivisionCircleZoneAndUserIDByDivisionID(long _DivisionID, long _IrrgLevelID, long _DesignationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetSubDivisionCircleZoneAndUserIDByDivisionID(_DivisionID, _IrrgLevelID, _DesignationID);
        }

        public long AddAutoGeneratedComplaintData(CM_Complaint _Complaint)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddAutoGeneratedComplaintData(_Complaint);
        }
        public bool AddAutoGeneratedAssignmentHistoryData(CM_ComplaintAssignmentHistory _CompAssignmentHistory)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddAutoGeneratedAssignmentHistoryData(_CompAssignmentHistory);
        }

        public bool AddAutoGeneratedComplaintMarked(CM_ComplaintMarked _CompMarked)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.AddAutoGeneratedComplaintMarked(_CompMarked);
        }

        /// <summary>
        /// this function return complaint by ID
        /// Created On: 14/10/2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>CM_Complaint</returns>
        public CM_Complaint GetComplaintByID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintByID(_ComplaintID);
        }

        /// <summary>
        /// this function updates complaint
        /// Created On: 19/10/2016
        /// </summary>
        /// <param name="_Complaint"></param>
        /// <returns>bool</returns>
        public bool UpdateComplaint(CM_Complaint _Complaint)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.UpdateComplaint(_Complaint);
        }

        /// <summary>
        /// This Function Deletes Complaint from Complaint Assignment History.
        /// Created on: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>bool</returns>
        public bool DeleteComplaintAssignmentHistory(long _ComplaintAssignmentHistoryID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.DeleteComplaintAssignmentHistory(_ComplaintAssignmentHistoryID);
        }

        /// <summary>
        /// This Function Deletes Complaint from Complaint Marked.
        /// Created on: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>bool</returns>
        public bool DeleteComplaintMarked(long _ComplaintMarkedID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.DeleteComplaintMarked(_ComplaintMarkedID);
        }

        /// <summary>
        /// This Function Deletes Complaint from Complaint Notification.
        /// Created on: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>bool</returns>
        public bool DeleteComplaintNotification(long _ComplaintNotificationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.DeleteComplaintNotification(_ComplaintNotificationID);
        }

        /// <summary>
        /// this function return list of Complaint Assignment History by Complaint ID
        /// Created On: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<CM_ComplaintAssignmentHistory></returns>
        public List<CM_ComplaintAssignmentHistory> GetComplaintAssignmentHistoryByComplaintID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintAssignmentHistoryByComplaintID(_ComplaintID);
        }

        /// <summary>
        /// this function return list of Complaint marked by complaint ID
        /// Created On: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<CM_ComplaintMarked></returns>
        public List<CM_ComplaintMarked> GetComplaintMarkedByComplaintID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintMarkedByComplaintID(_ComplaintID);
        }

        /// <summary>
        /// this function return list of Complaint Notification by complaint ID
        /// Created On: 20-10-2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<CM_ComplaintMarked></returns>
        public List<CM_ComplaintNotification> GetComplaintNotificationByComplaintID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintNotificationByComplaintID(_ComplaintID);
        }

        public dynamic GetComplaintInformationByComplaintID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintInformationByComplaintID(_ComplaintID);
        }

        public List<dynamic> GetAllCommentsByComplaintID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetAllCommentsByComplaintID(_ComplaintID);
        }

        /// <summary>
        /// this function return all designations whom the specific complaint has been assigned
        /// Created On: 21/10/2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<long?></returns>
        public List<long?> GetDesignationsForListByComplaintID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetDesignationsForListByComplaintID(_ComplaintID);
        }

        public List<dynamic> DDLGetNotifySDO(long _DivisionID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.DDLGetNotifySDO(_DivisionID);
        }



        public string GetComplaintID(long ComplaintSourceID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            CM_ComplaintSource mdlComplaintSource = dalComplaintsManagement.GetComplaintSourceForComplaintID(ComplaintSourceID);
            string CID = string.Empty;

            if (ComplaintSourceID == 1)
            {
                CID = mdlComplaintSource.ShortCode + "" + dalComplaintsManagement.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 2)
            {
                CID = mdlComplaintSource.ShortCode + "" + dalComplaintsManagement.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 3)
            {
                CID = mdlComplaintSource.ShortCode + "" + dalComplaintsManagement.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 4)
            {
                CID = mdlComplaintSource.ShortCode + "" + dalComplaintsManagement.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 5)
            {
                CID = mdlComplaintSource.ShortCode + "" + dalComplaintsManagement.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 6)
            {
                CID = mdlComplaintSource.ShortCode + "" + dalComplaintsManagement.GetNewComplaintID();
            }
            else if (ComplaintSourceID == 7)
            {
                CID = mdlComplaintSource.ShortCode + "" + dalComplaintsManagement.GetNewComplaintID();
            }
            return CID;
        }


        public List<CM_ComplaintComments> GetAllComplaintCommentsByID(long _CommentsID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetAllComplaintCommentsByID(_CommentsID);
        }

        /// <summary>
        /// this function return list of complaints marked on the basis of complaint ID
        /// Created On: 26/10/2016
        /// </summary>
        /// <param name="_ComplaintID"></param>
        /// <returns>List<CM_ComplaintMarked></returns>
        public bool UpdateComplaintMarkedAndComplaint(long _ComplaintID, long _UserID, long _DesignationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.UpdateComplaintMarkedAndComplaint(_ComplaintID, _UserID, _DesignationID);
        }

        /// <summary>
        /// this function updates complaint Marked
        /// Created On: 26/10/2016
        /// </summary>
        /// <param name="_Complaint"></param>
        /// <returns>bool</returns>
        public bool UpdateComplaintMarked(CM_ComplaintMarked _ComplaintMarked)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.UpdateComplaintMarked(_ComplaintMarked);
        }

        /// <summary>
        /// this function return Complaint Type By ID
        /// Created On: 27/10/2016
        /// </summary>
        /// <param name="_ComplaintTypeID"></param>
        /// <returns></returns>
        public CM_ComplaintType GetComplaintTypeByID(long _ComplaintTypeID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintTypeByID(_ComplaintTypeID);
        }

        public bool GetComplaintMarkedByUserID(long _UserID, long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintMarkedByUserID(_UserID, _ComplaintID);
        }
        /// <summary>
        /// This function return Complaint Sources except auto generated
        /// Created On: 02/11/2016
        /// </summary>
        /// <returns></returns>
        public List<CM_ComplaintSource> GetComplaintSourceForAddComplaint()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetComplaintSourceForAddComplaint();
        }

        public string AddAutoGeneratedComplaint(long _UserID, string RefCode, long DivisionID = -1, long RefCodeID = -1, long _ChannelID = -1, string _Details = "")
        {
            CM_Complaint complaint = new CM_Complaint();
            UA_Users mdlUser = db.Repository<UA_Users>().GetAll().FirstOrDefault(e => e.ID == _UserID);
            ComplaintsManagementBLL bllComplaintsManagementBLL = new ComplaintsManagementBLL();
            CM_ComplaintAssignmentHistory mdlComplaintAssignmentHistory = new CM_ComplaintAssignmentHistory();
            CM_ComplaintMarked mdlCompMarked = new CM_ComplaintMarked();
            //  List<CM_ComplaintMarked> lstComplaintMarked = new List<CM_ComplaintMarked>();
            dynamic lst = null;
            long ComplaintType = 0;
            complaint.ComplaintSourceID = (long)Constants.ComplaintSource.Automatic;
            if (RefCode.Trim().ToUpper() == "IF_WT" || RefCode.Trim().ToUpper() == "IF_ST" || RefCode.Trim().ToUpper() == "IF_DT" || RefCode.Trim().ToUpper() == "IF_RV")
            {
                complaint.ComplainantName = "N/A";  //mdlUser.FirstName + " " + mdlUser.LastName;
                complaint.MobilePhone = "N/A"; // mdlUser.MobilePhone;
                complaint.Phone = "N/A";
            }
            else
            {
                complaint.ComplainantName = mdlUser.FirstName + " " + mdlUser.LastName;
                complaint.MobilePhone = mdlUser.MobilePhone;
                complaint.Phone = mdlUser.LandLineNo;
            }
            if (RefCode.Trim().ToUpper() == "WT_C" || RefCode.Trim().ToUpper() == "WT_O")
            {
                complaint.Attachment = "";
            }
            complaint.ComplaintDate = DateTime.Now; //Convert.ToDateTime(DateTime.Now);
            complaint.RefCode = RefCode;
            complaint.RefCodeID = RefCodeID;
            complaint.ChannelID = _ChannelID;
            complaint.ComplaintDetails = _Details;
            if (DivisionID != -1)
            {
                lst = bllComplaintsManagementBLL.GetSubDivisionCircleZoneAndUserIDByDivisionID(DivisionID, (long)Constants.IrrigationLevelID.Division, (long)Constants.Designation.XEN);

            }
            if (lst != null)
            {


                if (RefCode.Trim().ToUpper() == "WT_C" || RefCode.Trim().ToUpper() == "WT_O" || RefCode.Trim().ToUpper() == "IF_WT")
                {
                    ComplaintType = 1;
                }
                else if (RefCode.Trim().ToUpper() == "IF_ST" || RefCode.Trim().ToUpper() == "SI_ST")
                {
                    ComplaintType = 3;
                }
                else if (RefCode.Trim().ToUpper() == "IF_DT" || RefCode.Trim().ToUpper() == "SI_DT")
                {
                    ComplaintType = 4;
                }
                else if (RefCode.Trim().ToUpper() == "IF_RV")
                {
                    ComplaintType = 2;
                }
                else if (RefCode.Trim().ToUpper() == "SI_HGNF")
                {
                    ComplaintType = 5;
                }
                else if (RefCode.Trim().ToUpper() == "SI_HGNP")
                {
                    ComplaintType = 6;
                }
                else if (RefCode.Trim().ToUpper() == "SI_TGNF")
                {
                    ComplaintType = 7;
                }
                else if (RefCode.Trim().ToUpper() == "SI_TGNP")
                {
                    ComplaintType = 8;
                }
                complaint.ComplaintTypeID = ComplaintType;
                CM_ComplaintType mdlComplaintType = bllComplaintsManagementBLL.GetComplaintTypeByID(ComplaintType);
                complaint.ResponseDuration = mdlComplaintType.ResponseTime;
                complaint.DomainID = lst.GetType().GetProperty("DomainID").GetValue(lst, null);
                complaint.ZoneID = lst.GetType().GetProperty("ZoneID").GetValue(lst, null);
                complaint.CircleID = lst.GetType().GetProperty("CircleID").GetValue(lst, null);
                complaint.DivisionID = lst.GetType().GetProperty("DivisionID").GetValue(lst, null);
                complaint.SubDivID = lst.GetType().GetProperty("SubDivisionID").GetValue(lst, null);
                //   complaint.ResponseDuration = 7;
                complaint.CreatedDate = DateTime.Now;
                complaint.ComplaintNumber = GetComplaintID(6);
                complaint.UserID = mdlUser.ID;
                complaint.UserDesigID = mdlUser.DesignationID;
                complaint.ComplaintStatusID = 1;
                complaint.AssignedToUser = lst.GetType().GetProperty("UserID").GetValue(lst, null);
                complaint.AssignedToDesig = lst.GetType().GetProperty("DesignationID").GetValue(lst, null);
                // Complaint Assignment History
                mdlComplaintAssignmentHistory.ComplaintStatusID = 1;
                mdlComplaintAssignmentHistory.AssignedByUser = mdlUser.ID;
                mdlComplaintAssignmentHistory.AssignedByDesig = mdlUser.DesignationID;
                mdlComplaintAssignmentHistory.AssignedToUser = lst.GetType().GetProperty("UserID").GetValue(lst, null);
                mdlComplaintAssignmentHistory.AssignedToDesig = lst.GetType().GetProperty("DesignationID").GetValue(lst, null);
                mdlComplaintAssignmentHistory.AssignedDate = DateTime.Now;
                mdlComplaintAssignmentHistory.Remarks = "Autogentrated Complaint";
                mdlComplaintAssignmentHistory.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlComplaintAssignmentHistory.CreatedDate = DateTime.Now;
                // Complaint Marked
                mdlCompMarked.UserID = lst.GetType().GetProperty("UserID").GetValue(lst, null);
                mdlCompMarked.UserDesigID = lst.GetType().GetProperty("DesignationID").GetValue(lst, null);
                mdlCompMarked.Starred = false;
                mdlCompMarked.MarkRead = false;
                mdlCompMarked.CreatedBy = Convert.ToInt32(mdlUser.ID);
                mdlCompMarked.CreatedDate = DateTime.Now;
                long ComplaintID;
                using (TransactionScope transaction = new TransactionScope())
                {
                    ComplaintID = bllComplaintsManagementBLL.AddAutoGeneratedComplaintData(complaint);
                    mdlComplaintAssignmentHistory.ComplaintID = ComplaintID;
                    mdlCompMarked.ComplaintID = ComplaintID;
                    bool IsSaved = bllComplaintsManagementBLL.AddAutoGeneratedAssignmentHistoryData(mdlComplaintAssignmentHistory);
                    bool IsSave = bllComplaintsManagementBLL.AddAutoGeneratedComplaintMarked(mdlCompMarked);
                    transaction.Complete();
                }
                //DDH Entry
                CM_ComplaintMarked mdlCompMarkedForDDH = new CM_ComplaintMarked();
                List<long> DDH = bllComplaintsManagementBLL.GetUsersForPidaAndSecretariat((long)Constants.Designation.DeputyDirectorHelpline, (long)Constants.Organization.PMIU);
                for (int i = 0; i < DDH.Count(); i++)
                {
                    mdlCompMarkedForDDH.UserID = DDH.ElementAt(i);
                    mdlCompMarkedForDDH.UserDesigID = (long)Constants.Designation.DeputyDirectorHelpline;
                    mdlCompMarkedForDDH.Starred = false;
                    mdlCompMarkedForDDH.MarkRead = false;
                    mdlCompMarkedForDDH.CreatedBy = Convert.ToInt32(mdlUser.ID);
                    mdlCompMarkedForDDH.CreatedDate = DateTime.Now;
                    mdlCompMarkedForDDH.ComplaintID = ComplaintID;
                    bllComplaintsManagementBLL.AddAutoGeneratedComplaintMarked(mdlCompMarkedForDDH);
                }

                NotifyEvent _event = new NotifyEvent();
                _event.Parameters.Add("ComplaintID", ComplaintID);
                _event.AddNotifyEvent((long)NotificationEventConstants.Complaints.Complaintisloggedintothesystem, Convert.ToInt32(mdlUser.ID));

            }
            return complaint.ComplaintNumber;
        }

        public DataSet GetComplaintInformation_SearchForPublicWebSite(string _ComplaintNumber, string _MobilePhone)
        {
            return new ComplaintsManagementDAL().GetComplaintInformation_SearchForPublicWebSite(_ComplaintNumber, _MobilePhone);
        }

        /// <summary>
        /// This function Get Users By Designation ID
        /// Created On: 18/11/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>List<long></returns>
        public List<long> GetUsersByDesignationID(long _DesignationID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetUsersByDesignationID(_DesignationID);
        }

        /// <summary>
        /// This function Update Complaint with complaint ID
        /// Created On: 19/11/2016
        /// </summary>
        /// <param name="_DesignationID"></param>
        /// <returns>List<long></returns>
        public bool UpdateComplaintStatusByComplaintID(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.UpdateComplaintStatusByComplaintID(_ComplaintID);
        }

        public bool InsertComplanintEntryInSMSTable(UA_SMSNotification _mdlSMS)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.InsertComplanintEntryInSMSTable(_mdlSMS);
        }

        public long GetMaxIDofCompalintType()
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.GetMaxIDofCompalintType();
        }

        public bool IsComplaintResolved(long _ComplaintID)
        {
            ComplaintsManagementDAL dalComplaintsManagement = new ComplaintsManagementDAL();
            return dalComplaintsManagement.IsComplaintResolved(_ComplaintID);

        }
        public DataSet GetTailStatusLastTenDays(long _ChannelID)
        {
            return new ComplaintsManagementDAL().GetTailStatusLastTenDays(_ChannelID);
        }

        public bool ComplaintNumberExist(string _Number)
        {
            return new ComplaintsManagementDAL().ComplaintNumberExist(_Number);
        }
    }
}
