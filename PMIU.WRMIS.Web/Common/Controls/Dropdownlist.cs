using PMIU.WRMIS.BLL.ComplaintsManagement;
using PMIU.WRMIS.BLL.DailyData;
using PMIU.WRMIS.BLL.EntitlementDelivery;
using PMIU.WRMIS.BLL.FloodOperations;
using PMIU.WRMIS.BLL.FloodOperations.ReferenceData;
using PMIU.WRMIS.BLL.IrrigationNetwork.Channel;
using PMIU.WRMIS.BLL.IrrigationNetwork.Reach;
using PMIU.WRMIS.BLL.IrrigationNetwork.ReferenceData;
using PMIU.WRMIS.BLL.IrrigatorsFeedback;
using PMIU.WRMIS.BLL.ScheduleInspection;
using PMIU.WRMIS.BLL.Tenders;
using PMIU.WRMIS.BLL.UserAdministration;
using PMIU.WRMIS.BLL.WaterTheft;
using PMIU.WRMIS.BLL.AssetsAndWorks;
using PMIU.WRMIS.BLL.SmallDams;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.EntitlementDelivery;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using PMIU.WRMIS.BLL.Accounts;

namespace PMIU.WRMIS.Web.Common.Controls
{
    public class Dropdownlist
    {
        #region "Start Private Methods"
        //private static void DDLDesignationForOrganization(DropDownList _ddlDesignation,long _OrganizationID, string _DataTextField = "Name", string _DataValueField = "ID")
        //{
        //    BindDropdownlist<List<UA_Designations>>(_ddlDesignation, new SearchUserBLL().GetDesignationAgainstOrganization(_OrganizationID), _DataTextField, _DataValueField);
        #endregion "End Private Methods"

        #region "Start Public Methods"

        public static void BindDropdownlist<TSource>(DropDownList _ddl, TSource _DataSource, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            _ddl.Items.Clear();

            _ddl.DataValueField = _DataValueField;
            _ddl.DataTextField = _DataTextField;
            _ddl.DataSource = _DataSource;
            _ddl.DataBind();

            if (_FirstOption == (int)Constants.DropDownFirstOption.Select)
            {
                _ddl.Items.Insert(0, new ListItem("Select", ""));
            }
            else if (_FirstOption == (int)Constants.DropDownFirstOption.All)
            {
                _ddl.Items.Insert(0, new ListItem("All", ""));
            }

        }

        public static void DDLChannelTypes(DropDownList _ddlChannelType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Channel types dropdownlist
            BindDropdownlist<List<CO_ChannelType>>(_ddlChannelType, new ChannelBLL().GetChannelTypes(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLChannels(DropDownList _ddlChannel, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_Channel>>(_ddlChannel, null, _FirstOption);
            else
                // Bind Channel types dropdownlist
                BindDropdownlist<List<CO_Channel>>(_ddlChannel, new ChannelBLL().GetChannels(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLFlowTypes(DropDownList _ddlFlowType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Flow type dropdownlist
            BindDropdownlist<List<CO_ChannelFlowType>>(_ddlFlowType, new ChannelBLL().GetChannelFlowTypes(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLCommandNames(DropDownList _ddlCommandName, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Command name dropdownlist
            BindDropdownlist<List<CO_ChannelComndType>>(_ddlCommandName, new ChannelBLL().GetChannelCommandTypes(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLEntitelmentYear(DropDownList _ddlEntitlement, int _SeasonID, int _CurrentYear, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Command name dropdownlist
            BindDropdownlist<List<object>>(_ddlEntitlement, new EntitlementDeliveryBLL().GetEntitlementYear(_SeasonID, _CurrentYear), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDeliveriesYear(DropDownList _ddlEntitlement, int SeasonID, int _CurrentYear, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Command name dropdownlist
            BindDropdownlist<List<object>>(_ddlEntitlement, new EntitlementDeliveryBLL().GetDeliveryYear(SeasonID, _CurrentYear), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLParentChannels(DropDownList _ddlParentChannel, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            //Bind Parent Channel dropdownlist
            Dropdownlist.BindDropdownlist<List<object>>(_ddlParentChannel, new ChannelBLL().GetParentChannels(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLZones(DropDownList _ddlZone, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_Zone>>(_ddlZone, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_Zone>>(_ddlZone, new ZoneBLL().GetAllZones(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLCircles(DropDownList _ddlCircle, bool _IsEmpty = false, long _ZoneID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Circle dropdownlist 
                BindDropdownlist<List<CO_Circle>>(_ddlCircle, null, _FirstOption);
            else
                // Bind Circle dropdownlist 
                BindDropdownlist<List<CO_Circle>>(_ddlCircle, new CircleBLL().GetCirclesByZoneID(_ZoneID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDivisions(DropDownList _ddlDivision, bool _IsEmpty = false, long _CircleID = -1, long _DomainID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<CO_Division>>(_ddlDivision, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<CO_Division>>(_ddlDivision, new DivisionBLL().GetDivisionsByCircleIDAndDomainID(_CircleID, _DomainID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSubDivisions(DropDownList _ddlSubDivision, bool _IsEmpty = false, long _DivisionID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Sub Division dropdownlist 
                BindDropdownlist<List<CO_SubDivision>>(_ddlSubDivision, null, _FirstOption);
            else
                // Bind Sub Division dropdownlist 
                BindDropdownlist<List<CO_SubDivision>>(_ddlSubDivision, new SubDivisionBLL().GetSubDivisionsByDivisionID(_DivisionID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSections(DropDownList _ddlSection, bool _IsEmpty = false, long _SubDivisionID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Section dropdownlist 
                BindDropdownlist<List<CO_Section>>(_ddlSection, null);
            else
                // Bind Section dropdownlist 
                BindDropdownlist<List<CO_Section>>(_ddlSection, new SectionBLL().GetSectionsBySubDivisionID(_SubDivisionID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLScheduleInspectionStatus(DropDownList _ddlStatus, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<SI_ScheduleStatus>>(_ddlStatus, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<SI_ScheduleStatus>>(_ddlStatus, new ScheduleInspectionBLL().GetScehduleInspectionStatuses(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSubDivisionsByChannelID(DropDownList _ddlSubDivision, long _ChannelID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Sub Division dropdownlist 
            BindDropdownlist<List<CO_SubDivision>>(_ddlSubDivision, new ChannelBLL().GetSubDivisionsByChannelID(_ChannelID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDistricts(DropDownList _ddlDistrict, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind District dropdownlist 
                BindDropdownlist<List<CO_District>>(_ddlDistrict, null, _FirstOption);
            else
                // Bind District dropdownlist 
                BindDropdownlist<List<CO_District>>(_ddlDistrict, new DistrictBLL().GetAllDistricts(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDistrictByChannelID(DropDownList _ddlDistrict, long _ChannelID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind District dropdownlist 
            BindDropdownlist<List<object>>(_ddlDistrict, new ChannelBLL().GetDistrictsByChannelID(_ChannelID, _IsActive));
        }

        public static void DDLDistrictByStructureID(DropDownList _ddlDistrict, long _StructureID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind District dropdownlist 
            BindDropdownlist<List<object>>(_ddlDistrict, new InfrastructureBLL().GetDistrictsByStructureID(_StructureID, _IsActive));
        }

        public static void DDLTehsils(DropDownList _ddlTehsil, bool _IsEmpty = false, long _DistrictID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Tehsil dropdownlist 
                BindDropdownlist<List<CO_Tehsil>>(_ddlTehsil, null, _FirstOption);
            else
                // Bind Tehsil dropdownlist 
                BindDropdownlist<List<CO_Tehsil>>(_ddlTehsil, new TehsilBLL().GetTehsilsByDistrictID(_DistrictID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLPoliceStations(DropDownList _ddlPoliceStation, bool _IsEmpty = false, long _TehsilID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Police Station dropdownlist 
                BindDropdownlist<List<CO_PoliceStation>>(_ddlPoliceStation, null, _FirstOption);
            else
                // Bind Police Station dropdownlist 
                BindDropdownlist<List<CO_PoliceStation>>(_ddlPoliceStation, new PoliceStationBLL().GetPoliceStationsByTehsilID(_TehsilID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLVillages(DropDownList _ddlVillage, bool _IsEmpty = false, int _PoliceStationID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Village dropdownlist 
                BindDropdownlist<List<CO_Village>>(_ddlVillage, null, _FirstOption);
            else
                // Bind Village dropdownlist 
                BindDropdownlist<List<CO_Village>>(_ddlVillage, new VillageBLL().GetVillagesByPoliceStationID(_PoliceStationID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLVillagesForInfrastructure(DropDownList _ddlVillage, bool _IsEmpty = false, int _PoliceStationID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Village dropdownlist 
                BindDropdownlist<List<dynamic>>(_ddlVillage, null, _FirstOption);
            else
                // Bind Village dropdownlist 
                BindDropdownlist<List<dynamic>>(_ddlVillage, new VillageBLL().GetVillagesByPoliceStationIDForinfrastructure(_PoliceStationID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLProvince(DropDownList _ddlProvince, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Province>>(_ddlProvince, new ProvinceBLL().GetAllProvinces(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLRiver(DropDownList _ddlRiver, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_River>>(_ddlRiver, new StructureBLL().GetAllRivers(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLChannelSide(DropDownList _ddlChannelSide, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlChannelSide, CommonLists.GetChannelSides(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLGaugeCategories(DropDownList _ddlGaugeCategory, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlGaugeCategory, CommonLists.GetGaugeCategories(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLGaugeLevels(DropDownList _ddlGaugeAtBed, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlGaugeAtBed, CommonLists.GetGaugeLevels(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLGaugeRDbyChannelID(DropDownList _ddlGauge, long ChannelID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlGauge, new ChannelBLL().GetChannelGaugeRDByChannelID(ChannelID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLGaugeTypes(DropDownList _ddlGaugeType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_GaugeType>>(_ddlGaugeType, new ChannelBLL().GetGaugeTypes(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLGaugeTypesForFloodbund(DropDownList _ddlGaugeType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<FO_FloodGaugeType>>(_ddlGaugeType, new InfrastructureBLL().GetGaugeTypesFloodBund(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSectionsBySubDivisionChannelID(DropDownList _ddlSection, long _SubDivisionID = -1, long _ChannelID = 0, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Section dropdownlist 
            BindDropdownlist<List<CO_Section>>(_ddlSection, new ChannelBLL().GetSectionsBySubDivisionChannelID(_SubDivisionID, _ChannelID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLGetAllGaugeCategories(DropDownList _ddlGaugeCategory, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Section dropdownlist 
                BindDropdownlist<List<CO_GaugeCategory>>(_ddlGaugeCategory, null);
            else
                BindDropdownlist<List<CO_GaugeCategory>>(_ddlGaugeCategory, new ChannelBLL().GetGaugeCategories(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLRole(DropDownList _ddlRole, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<UA_Roles>>(_ddlRole, new RoleRightsBLL().GetAllRoles(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLGetModules(DropDownList _ddlModule, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID", bool forSeletedRole = false)
        {
            BindDropdownlist<List<UA_Modules>>(_ddlModule, new RoleRightsBLL().getAllModules(forSeletedRole), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDesignations(DropDownList _ddlDesignation, long _OrganizationID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<UA_Designations>>(_ddlDesignation, new UserBLL().GetAllDesignations(_OrganizationID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLStructures(DropDownList _ddlStructure, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Station>>(_ddlStructure, new StructureBLL().GetStations(-1, -1, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLStructureChannels(DropDownList _ddlChannel, long _StructureID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Channel dropdownlist 
                BindDropdownlist<List<CO_Village>>(_ddlChannel, null, _FirstOption);
            else
                BindDropdownlist<List<CO_Channel>>(_ddlChannel, new StructureBLL().GetChannelsByStationID(_StructureID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDivisionNames(DropDownList _ddlDivision, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind all Division names only   
            BindDropdownlist<List<CO_Division>>(_ddlDivision, new DivisionBLL().GetAllDivisionNames().OrderBy(d => d.Name).ToList(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDivisionNames_ForPublicWebSite(DropDownList _ddlDivision, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            IEnumerable<DataRow> ieDivision = new ChannelBLL().GetDivisions_ListForPublicWebSite();

            List<object> lstDivisionNames = ieDivision.Select(dataRow => new
            {
                ID = dataRow.Field<long>("ID"),
                Name = dataRow.Field<string>("Name")
            }).ToList<object>();

            // Bind all Division names only   
            BindDropdownlist<List<object>>(_ddlDivision, lstDivisionNames, _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDomains(DropDownList _ddlDomain, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Division dropdownlist 
            BindDropdownlist<List<CO_Domain>>(_ddlDomain, new DivisionBLL().GetDomains().OrderBy(d => d.Name).ToList(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLStructureTypes(DropDownList _ddlStructureType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_StructureType>>(_ddlStructureType, new StructureBLL().GetAllStructureTypes(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLOrganizations(DropDownList _ddlOrganization, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<UA_Organization>>(_ddlOrganization, new UserBLL().GetAllOrganizations(_IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLNotificationEvents(DropDownList _ddlEvent, bool _IsEmpty = false, long _ModuleID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<UA_NotificationEvents>>(_ddlEvent, null, _FirstOption);
            else
                BindDropdownlist<List<UA_NotificationEvents>>(_ddlEvent, new AlertConfigurationBLL().GetNotificationEventsByModuleID(_ModuleID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDesignationAgainstOrganization(DropDownList _ddlDesignation, long _OrganizationID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<UA_Designations>>(_ddlDesignation, new UserBLL().GetDesignationAgainstOrganization(_OrganizationID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLIrrigationLevel(DropDownList _ddlLevel, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool _IsIrrigation = true, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<UA_IrrigationLevel>>(_ddlLevel, new DesignationBLL().GetAllIrrigationLevel(_IsIrrigation), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLStatus(DropDownList _ddlStatus, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlStatus, CommonLists.GetStatuses(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLYesNo(DropDownList _ddlYesNo, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlYesNo, CommonLists.GetYesNo(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLUStreamDStream(DropDownList _ddlUD, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlUD, CommonLists.GetUstreamDstream(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAvailableNotAvailable(DropDownList _ddlAN, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlAN, CommonLists.GetAvailableNotAvailable(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLLiningType(DropDownList _ddlLiningType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_LiningType>>(_ddlLiningType, new ReachBLL().GetLiningType(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLBarrages(DropDownList _ddlBarrages, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Station>>(_ddlBarrages, new DailyDataBLL().GetAllPunjabBarrages(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLReasonsForChange(DropDownList _ddlReasonsForChange, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_ReasonForChange>>(_ddlReasonsForChange, new PMIU.WRMIS.BLL.DailyData.ReferenceDataBLL().GetAllReasonForChange(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDivisionsByUserID(DropDownList _ddlDivision, long _UserID, long _IrrigationLevelID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Division>>(_ddlDivision, new WaterTheftBLL().GetDivisionsByUserID(_UserID, _IrrigationLevelID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLGeneralInspectionType(DropDownList _ddlGeneralInspectionType, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Tender Notice dropdownlist 
                BindDropdownlist<List<SI_GeneralInspectionType>>(_ddlGeneralInspectionType, null, _FirstOption);
            else
                // Bind Tender Notice dropdownlist 
                BindDropdownlist<List<SI_GeneralInspectionType>>(_ddlGeneralInspectionType, new ScheduleInspectionBLL().GetGeneralInspectionTypes(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLDivisionsByUserIDAndCircleID(DropDownList _ddlDivision, long _UserID, long _IrrigationLevelID, long _CircleID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Division dropdownlist 
                BindDropdownlist<List<CO_Division>>(_ddlDivision, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<CO_Division>>(_ddlDivision, new FloodOperationsBLL().GetDivisionsByUserIDAndCircleID(_UserID, _IrrigationLevelID, _CircleID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLCircleByUserIDAndZoneID(DropDownList _ddlCirlce, long _UserID, long _IrrigationLevelID, long _ZoneID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Circle dropdownlist 
                BindDropdownlist<List<CO_Circle>>(_ddlCirlce, null, _FirstOption);
            else
                // Bind Circle dropdownlist 
                BindDropdownlist<List<CO_Circle>>(_ddlCirlce, new FloodOperationsBLL().GetCircleByUserIDAndZoneID(_UserID, _IrrigationLevelID, _ZoneID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLZoneByUserID(DropDownList _ddlZone, long _UserID, long _IrrigationLevelID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Zone>>(_ddlZone, new FloodOperationsBLL().GetZoneByUserID(_UserID, _IrrigationLevelID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLChannelsByUserIDAndDivisionID(DropDownList _ddlChannel, long _UserID, long? _IrrigationLevelID, long _DivisionID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Channel>>(_ddlChannel, new WaterTheftBLL().GetChannelsByUserIDAndDivisionID(_UserID, _IrrigationLevelID, _DivisionID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLWaterTheftStatus(DropDownList _ddlStatus, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<WT_Status>>(_ddlStatus, new WaterTheftBLL().GetStatus(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLComplaintType(DropDownList _ddlComplaintType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CM_ComplaintType>>(_ddlComplaintType, new ComplaintsManagementBLL().GetAllComplaintsType(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLComplaintStatus(DropDownList _ddlComplaintStatus, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CM_ComplaintStatus>>(_ddlComplaintStatus, new ComplaintsManagementBLL().GetComplaintStatus(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLDirectiveTypes(DropDownList _ddlComplaintType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            List<CM_ComplaintSource> lstDirectives = (from item in new ComplaintsManagementBLL().GetComplaintSource() where (item.ShortCode.ToLower().Trim() == "CM" || item.ShortCode.ToLower().Trim() == "CM" || item.ShortCode.ToLower().Trim() == "CM" || item.ShortCode.ToLower().Trim() == "CM") select item).ToList();
            BindDropdownlist<List<CM_ComplaintSource>>(_ddlComplaintType, lstDirectives, _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLOffenceType(DropDownList _ddlOffenceType, string _OffenceSite, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<WT_OffenceType>>(_ddlOffenceType, new WaterTheftBLL().GetOffenceType(_OffenceSite), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLTailStatus(DropDownList _ddlTailStatus, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlTailStatus, CommonLists.GetTailStatuses(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLVillagesByDivisionID(DropDownList _ddlVillages, long _DivisionID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Village>>(_ddlVillages, new IrrigatorFeedbackBLL().GetVillagesByDivisionID(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }

        //public static void DDLChannelsByUserIDForIndents(DropDownList _ddlChannel, long _UserID, long? _IrrigationLevelID, long _SubdivID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        //{
        //    //BindDropdownlist<List<dynamic>>(_ddlChannel, new IndentsBLL().GetChannelsByUserID(_UserID, _IrrigationLevelID, _SubdivID), _FirstOption, _DataTextField, _DataValueField);
        //}

        public static void DDLChannelsNameBySubDivisionIDOrDivisionID(DropDownList _ddlChannel, long? _SubDivisionID, long? _DivisionID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Channel>>(_ddlChannel, new IndentsBLL().GetChannelsNameBySubDivisionIDOrDivisionID(_SubDivisionID, _DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDomainByUserID(DropDownList _ddlDomain, long _UserID, long _IrrigationLevelID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Domain>>(_ddlDomain, new ComplaintsManagementBLL().GetDomainsByUserID(_UserID, _IrrigationLevelID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLDivisionsByDomainID(DropDownList _ddlDivision, long _DomainID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            List<CO_Division> lstDvsn = new ComplaintsManagementBLL().GetDivisionsByDomainID(_DomainID, _IsActive);
            BindDropdownlist<List<CO_Division>>(_ddlDivision, lstDvsn, _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDivisionsForDFAndIrrigation(DropDownList _ddlDivision, bool _IsEmpty = false, long _CircleID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Section dropdownlist 
                BindDropdownlist<List<CO_Division>>(_ddlDivision, null);
            else
                // Bind Section dropdownlist 
                BindDropdownlist<List<CO_Division>>(_ddlDivision, new InfrastructureBLL().DDLDivisionsForDFAndIrrigation(_CircleID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDomainsForAddComplaint(DropDownList _ddlDomain, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Channel types dropdownlist
            BindDropdownlist<List<CO_Domain>>(_ddlDomain, new ComplaintsManagementBLL().GetDomainsForAddComplaint(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLStructuresByDomainID(DropDownList _ddlStructure, long _DomainID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_StructureType>>(_ddlStructure, new ComplaintsManagementBLL().GetStructureTypeByDomainID(_DomainID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLComplaintSource(DropDownList _ddlComplaintSource, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind ComplaintSource dropdownlist
            BindDropdownlist<List<CM_ComplaintSource>>(_ddlComplaintSource, new ComplaintsManagementBLL().GetComplaintSource(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void GetComplaintStatus(DropDownList _ddlComplaintStatus, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind ComplaintStatus Dropdown
            BindDropdownlist<List<CM_ComplaintStatus>>(_ddlComplaintStatus, new ComplaintsManagementBLL().GetComplaintStatus(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void GetComplaintType(DropDownList _ddlComplaintType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind ComplaintStatus Dropdown
            BindDropdownlist<List<CM_ComplaintType>>(_ddlComplaintType, new ComplaintsManagementBLL().GetComplaintType(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLChannelsBySubDivID(DropDownList _ddlChannel, long _SubDivID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Channel>>(_ddlChannel, new ComplaintsManagementBLL().GetChannelsBySubDivID(_SubDivID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLOutletsBySubDivIDAndChannelID(DropDownList _ddlOutlet, long _SubDivID, long _ChannelID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlOutlet, new ComplaintsManagementBLL().GetOutletsByChannelIDAndSubDivID(_SubDivID, _ChannelID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLStructureByDomainIrrigation(DropDownList _ddlStructure, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlStructure, CommonLists.StructuresByDomainIrrigation(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLStructureByDomainDandF(DropDownList _ddlStructure, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlStructure, CommonLists.StructuresByDomainDandF(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLProtectionInfrastructureByDivisionID(DropDownList _ddlProtectionInfrastructure, long _DivisionID, long _DomainID, string _Source, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlProtectionInfrastructure, new ComplaintsManagementBLL().GetProtectionInfrastructuresByDivisionID(_DivisionID, _DomainID, _Source), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDrainsBySubDivisionID(DropDownList _ddlDrains, long _SubDiv, long _DomainID, string _Source, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlDrains, new ComplaintsManagementBLL().GetDrainsBySubDivisionID(_SubDiv, _DomainID, _Source), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLLoading(DropDownList _ddlCommand, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, List<object> _DataSource = null)
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<object>>(_ddlCommand, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<object>>(_ddlCommand, _DataSource, _FirstOption, "Name", "ID");
        }
        public static void DDLVillagesByTehsilID(DropDownList _ddlVillages, long _TehsilID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Village>>(_ddlVillages, new ComplaintsManagementBLL().GetVillagesByTehsilID(_TehsilID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDivisionsByVillageID(DropDownList _ddlDivision, long _VillageID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlDivision, new ComplaintsManagementBLL().GetDivisionsByVillageID(_VillageID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLChannelsByVillageID(DropDownList _ddlChannel, long _VillageID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlChannel, new ComplaintsManagementBLL().GetChannelsByVillageID(_VillageID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLYear(DropDownList _ddlYear, bool _IsEmpty = false, long _NumberOfPreviousYears = 0, long _StartYear = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select)
        {

            List<object> lstYear = new List<object>();
            long currentYears = Convert.ToInt64(DateTime.Now.Year);

            if (_StartYear != -1 && _NumberOfPreviousYears != 0)
            {

                _NumberOfPreviousYears = _StartYear - _NumberOfPreviousYears;

                for (long counter = _StartYear; counter >= _NumberOfPreviousYears; counter--)
                {
                    var strYear = new
                    {
                        Name = counter,
                        ID = counter
                    };
                    lstYear.Add(strYear);
                }
            }
            else if (_StartYear != -1)
            {

                _NumberOfPreviousYears = currentYears;

                for (long counter = _NumberOfPreviousYears; counter >= _StartYear; counter--)
                {
                    var strYear = new
                    {
                        Name = counter,
                        ID = counter
                    };
                    lstYear.Add(strYear);
                }
            }
            else
            {
                for (long counter = 0; counter <= _NumberOfPreviousYears; counter++)
                {
                    var strYear = new
                    {
                        Name = currentYears - counter,
                        ID = currentYears - counter
                    };
                    lstYear.Add(strYear);
                }
            }

            if (_IsEmpty)
                // Bind Year dropdownlist 
                BindDropdownlist<List<object>>(_ddlYear, null, _FirstOption);
            else
                // Bind Year dropdownlist 
                BindDropdownlist<List<object>>(_ddlYear, lstYear, _FirstOption, "Name", "ID");
        }


        public static void DDLOutletsByVillageIDAndChannelID(DropDownList _ddlOutlets, long _VillageID, long _ChannelID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_ChannelOutlets>>(_ddlOutlets, new ComplaintsManagementBLL().GetOutletsByVillageIDAndChannelID(_VillageID, _ChannelID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDivisionByOutletID(DropDownList _ddlDivision, long _OutletID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlDivision, new ComplaintsManagementBLL().GetDivisionByOutletID(_OutletID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLProtectionStructureByVillageIDAndDomainID(DropDownList _ddlProtectionStructure, long _VillageID, long _DomainID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlProtectionStructure, new ComplaintsManagementBLL().GetProtectionStructureByVillageID(_VillageID, _DomainID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLGetNotifySDO(DropDownList _ddlSDONotify, long _DivisionID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind ComplaintSource dropdownlist
            BindDropdownlist<List<dynamic>>(_ddlSDONotify, new ComplaintsManagementBLL().DDLGetNotifySDO(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLComplaintSourceForAddComplaint(DropDownList _ddlComplaintSource, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind ComplaintSource dropdownlist
            BindDropdownlist<List<CM_ComplaintSource>>(_ddlComplaintSource, new ComplaintsManagementBLL().GetComplaintSourceForAddComplaint(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLChannelsByIrrigationBoundary(DropDownList _ddlChannel, long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Channel dropdownlist
            BindDropdownlist<List<CO_Channel>>(_ddlChannel, new ChannelBLL().GetChannelsByIrrigationBoundary(_ZoneID, _CircleID, _DivisionID, _SubDivisionID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLArrangementTypes(DropDownList _ddlArrangementTypes, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<FO_FFPArrangementType>>(_ddlArrangementTypes, new FloodFightingPlanBLL().GetAllArrangementType(), _FirstOption, _DataTextField, _DataValueField);
        }

        #endregion "End Public Methods"

        public static void SetSelectedText(DropDownList _ddl, string _TextToSearch)
        {
            ListItem items = _ddl.Items.FindByText(_TextToSearch);
            if (items != null)
            {
                items.Selected = true;
            }
        }

        public static void SetSelectedValue(DropDownList _ddl, string _ValueToSearch)
        {
            ListItem items = _ddl.Items.FindByValue(_ValueToSearch);
            if (items != null)
            {
                items.Selected = true;
            }
        }

        public static void DDLALLCircles(DropDownList _ddlCircle, bool _IsEmpty = false, long _ZoneID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Circle dropdownlist 
                BindDropdownlist<List<CO_Circle>>(_ddlCircle, null, _FirstOption);
            else
                // Bind Circle dropdownlist 
                BindDropdownlist<List<CO_Circle>>(_ddlCircle, new CircleBLL().GetAllCircles(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLALLOtherTenderOffices(DropDownList _ddlOtherOffices, bool _IsEmpty = false, long _ZoneID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Tender Offices dropdownlist 
                BindDropdownlist<List<TM_TenderOpeningOffice>>(_ddlOtherOffices, null, _FirstOption);
            else
                // Bind Tender Offices dropdownlist 
                BindDropdownlist<List<TM_TenderOpeningOffice>>(_ddlOtherOffices, new TenderManagementBLL().GetAllOtherTenderOffices(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLGetUserByDivisionandDesignationID(DropDownList _ddlSDONotify, long _DivisionID, long _DesignationID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind ComplaintSource dropdownlist
            BindDropdownlist<List<dynamic>>(_ddlSDONotify, new TenderManagementBLL().GetUsersByDivisionID(_DivisionID, _DesignationID), _FirstOption, _DataTextField, _DataValueField);
        }

        #region Performance Evaluation

        public static void DDLKPICategories(DropDownList _ddlKPICategory, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<PE_KPICategories>>(_ddlKPICategory, new PMIU.WRMIS.BLL.PerformanceEvaluation.ReferenceDataBLL().GetKPICategories(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLComplexityFactor(DropDownList _ddlComplexityFactor, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<PE_ComplexityFactor>>(_ddlComplexityFactor, new PMIU.WRMIS.BLL.PerformanceEvaluation.ReferenceDataBLL().GetComplexityFactors(), _FirstOption, _DataTextField, _DataValueField);
        }

        #endregion

        #region Entitlement and Delivery

        public static void DDLGetCannalSystemByCommandTypeID(DropDownList _ddlCanalSystem, long _UserID, bool _UserBaseLoading, long? _BoundryID, long _CommandID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            // Bind Canal System dropdownlist
            BindDropdownlist<List<dynamic>>(_ddlCanalSystem, new EntitlementDeliveryBLL().GetCannalSystemByCommandTypeID(_UserID, _UserBaseLoading, _BoundryID, _CommandID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLYearsForEntitlement(DropDownList _ddlYear, long _SeasonID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            BindDropdownlist<List<dynamic>>(_ddlYear, new EntitlementDeliveryBLL().GetDistinctYearsBySeason(_SeasonID), _FirstOption, _DataTextField, _DataValueField);
        }

        #endregion

        #region Flood Operation
        #region Flood Operation  - Irrigation Network
        public static void DDLActiveInfrastructureType(DropDownList _ddlInfrastructureType, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_StructureType>>(_ddlInfrastructureType, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_StructureType>>(_ddlInfrastructureType, new InfrastructureBLL().GetAllActiveInfrastructureType(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLActiveProtectionInfrastructure(DropDownList _ddl, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_ProtectionInfrastructure>>(_ddl, null, _FirstOption);
            else

                BindDropdownlist<List<FO_ProtectionInfrastructure>>(_ddl, new InfrastructureBLL().GetActiveProtectionInfrastructure(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLParentTypes(DropDownList _ddlParentType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_StructureType>>(_ddlParentType, new StructureBLL().GetParentTypes(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLCustody(DropDownList _ddl, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<FO_ExplosivesCustody>>(_ddl, new InfrastructureBLL().GetInfrastructureCustody(), _FirstOption, _DataTextField, _DataValueField);
        }
        #endregion Flood Operation  - Irrigation Network

        #region Controlled Infrastructure
        public static void DDLActiveControlInfrastructureType(DropDownList _ddlType, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_StructureType>>(_ddlType, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_StructureType>>(_ddlType, new ControlledInfrastructureBLL().GetAllActiveControlInfrastructureType(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLGaugesSide(DropDownList _ddlSide, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlSide, CommonLists.GetBreachSides(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLGateType(DropDownList _ddl, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddl, CommonLists.GateTypes(), _FirstOption, _DataTextField, _DataValueField);
        }


        #endregion Controlled Infrastructure

        #region Reference Data
        public static void DDLMajorMinor(DropDownList _ddlMajorMinor, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlMajorMinor, CommonLists.MajorMinor(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLItemCategory(DropDownList _ddlItemCategory, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_ItemCategory>>(_ddlItemCategory, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_ItemCategory>>(_ddlItemCategory, new ItemsBLL().GetAllItemCategoryList(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLItemCategoryWithOutAsset(DropDownList _ddlItemCategory, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_ItemCategory>>(_ddlItemCategory, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_ItemCategory>>(_ddlItemCategory, new ItemsBLL().GetAllItemCategoryListWithOutAsset(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLItemCategoryByID(DropDownList _ddlItemCategory, long catgid, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_ItemCategory>>(_ddlItemCategory, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_ItemCategory>>(_ddlItemCategory, new ItemsBLL().GetAllItemCategoryListByID(catgid), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLItem(DropDownList _ddlItem, long catgid, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_Items>>(_ddlItem, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_Items>>(_ddlItem, new ItemsBLL().GetItemsCatg(catgid), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLItemByID(DropDownList _ddlItem, long ItemID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_Items>>(_ddlItem, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_Items>>(_ddlItem, new ItemsBLL().GetItemsCatgByID(ItemID), _FirstOption, _DataTextField, _DataValueField);
        }

        //public static void DDLDSEntryType(DropDownList _ddlStockType, string _Source, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        //{
        //    if (_IsEmpty)
        //        // Bind Zone dropdownlist 
        //        BindDropdownlist<List<FO_DSEntryType>>(_ddlStockType, null, _FirstOption);
        //    else
        //        // Bind Zone dropdownlist 
        //        BindDropdownlist<List<FO_DSEntryType>>(_ddlStockType, new DivisionStoreBLL().GetDSStockType(_Source), _FirstOption, _DataTextField, _DataValueField);
        //}

        #endregion Reference Data

        #region Flood Inspection
        public static void DDLActiveInspectionType(DropDownList _ddl, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_InspectionType>>(_ddl, null, _FirstOption);
            else

                BindDropdownlist<List<FO_InspectionType>>(_ddl, new FloodOperationsBLL().GetAllActiveInspectionType(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLPrePostInspectionType(DropDownList _ddl, Int16 _InspectionId, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_InspectionType>>(_ddl, null, _FirstOption);
            else

                BindDropdownlist<List<FO_InspectionType>>(_ddl, new FloodOperationsBLL().GetPrePostInspectionByID(_InspectionId), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLActiveInspectionStatus(DropDownList _ddl, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_InspectionStatus>>(_ddl, null, _FirstOption);
            else

                BindDropdownlist<List<FO_InspectionStatus>>(_ddl, new FloodOperationsBLL().GetAllActiveInspectionStatus(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLInfrastructureType(DropDownList _ddl, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddl, CommonLists.InfrastructureTypes(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDInfrastructureNameByType(DropDownList _ddl, long _UserID, long InfrastructureType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            //BindDropdownlist<List<object>>(_ddl, new FloodOperationsBLL().GetProtectionInfrastructureName(_UserID, InfrastructureType), _FirstOption, _DataTextField, _DataValueField);
            BindDropdownlist<List<object>>(_ddl, new FloodOperationsBLL().GetInfrastructureName(_UserID, InfrastructureType), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDInfrastructureNameByTypeForAllRoles(DropDownList _ddl, string InfrastructureType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            BindDropdownlist<List<object>>(_ddl, new FloodOperationsBLL().GetProtectionInfrastructureNameByType(InfrastructureType), _FirstOption, "InfrastructureName", _DataValueField);
        }

        public static void DDLInspectionConditionsByGroup(DropDownList _ddlInspectionConditions, string _CoditionGroup, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_InspectionConditions>>(_ddlInspectionConditions, null, _FirstOption);
            else

                BindDropdownlist<List<FO_InspectionConditions>>(_ddlInspectionConditions, new FloodInspectionsBLL().GetInspectionConditionsByGroup(_CoditionGroup), _FirstOption, _DataTextField, _DataValueField);
        }

        #region R D WiseCondition
        public static void DDLActiveStonePitchSide(DropDownList _ddl, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_StonePitchSide>>(_ddl, null, _FirstOption);
            else

                BindDropdownlist<List<FO_StonePitchSide>>(_ddl, new FloodInspectionsBLL().GetAllActiveStonePitchSide(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLActiveEncroachmentType(DropDownList _ddl, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_EncroachmentType>>(_ddl, null, _FirstOption);
            else

                BindDropdownlist<List<FO_EncroachmentType>>(_ddl, new FloodInspectionsBLL().GetAllActiveEncroachmentType(), _FirstOption, _DataTextField, _DataValueField);
        }

        #endregion R D WiseCondition




        #endregion Flood Inspection

        #region MeasureBookStatus
        public static void DDLCategory(DropDownList _ddlCategory, int _FirstOption = (int)Constants.DropDownFirstOption.Select)
        {

            BindDropdownlist<List<FO_ItemCategory>>(_ddlCategory, null, _FirstOption);
        }

        #endregion


        #region ProblemForFloodInspection

        public static void DDLProblemNature(DropDownList _ddl, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_ProblemNature>>(_ddl, null, _FirstOption);
            else

                BindDropdownlist<List<FO_ProblemNature>>(_ddl, new FloodInspectionsBLL().GetAllActiveProblemNature(), _FirstOption, _DataTextField, _DataValueField);
        }


        #endregion

        #region Flood Emergency Purchase

        public static void DDLFo_NatureWork(DropDownList _ddl, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)

                BindDropdownlist<List<FO_NatureOfWork>>(_ddl, null, _FirstOption);
            else

                BindDropdownlist<List<FO_NatureOfWork>>(_ddl, new FloodOperationsBLL().GetAllActiveFO_natureofWork(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLEmergencyPurchasesYear(DropDownList _ddl, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Year", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<FO_EmergencyPurchase>>(_ddl, null, _FirstOption);
            else
                BindDropdownlist<List<FO_EmergencyPurchase>>(_ddl, new FloodOperationsBLL().GetAllEmergencyPurchasesYear(), _FirstOption, _DataTextField, _DataValueField);
        }
        #endregion
        #region DivisionStore

        public static void DDLDSEntryType(DropDownList _ddlStockType, string _Source, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_DSEntryType>>(_ddlStockType, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<FO_DSEntryType>>(_ddlStockType, new DivisionStoreBLL().GetDSStockType(_Source), _FirstOption, _DataTextField, _DataValueField);
        }
        #endregion
        #endregion Flood Operation

        #region Tender Monitoring

        public static void DDLTenderMonitoringTenderNotice(DropDownList _ddlTenderMonitoringTenderNotice, DateTime _FromDate, DateTime _ToDate, bool _IsEmpty = false, long _DivisionID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Tender Notice dropdownlist 
                BindDropdownlist<List<TM_TenderNotice>>(_ddlTenderMonitoringTenderNotice, null, _FirstOption);
            else
                // Bind Tender Notice dropdownlist 
                BindDropdownlist<List<TM_TenderNotice>>(_ddlTenderMonitoringTenderNotice, new ScheduleInspectionBLL().GetTenderNoticesByDivisionID(_DivisionID, _FromDate, _ToDate), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLTenderMonitoringTenderNotice(DropDownList _ddlTenderMonitoringTenderNotice, long _DivisionID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Tender Notice dropdownlist 
                BindDropdownlist<List<TM_TenderNotice>>(_ddlTenderMonitoringTenderNotice, null, _FirstOption);
            else
                // Bind Tender Notice dropdownlist 
                BindDropdownlist<List<TM_TenderNotice>>(_ddlTenderMonitoringTenderNotice, new ScheduleInspectionBLL().GetTenderNoticesByDivisionID(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLTenderMonitoringTenderWorks(DropDownList _ddlTenderMonitoringTenderWorks, bool _IsEmpty = false, long _TenderNoticeID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Tender Notice dropdownlist 
                BindDropdownlist<List<TM_TenderWorks>>(_ddlTenderMonitoringTenderWorks, null, _FirstOption);
            else
                // Bind Tender Notice dropdownlist 
                BindDropdownlist<List<object>>(_ddlTenderMonitoringTenderWorks, new ScheduleInspectionBLL().GetTenderWorksByTenderNoticeID(_TenderNoticeID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLClosureOperationsWorks(DropDownList _ddlClosureOperationWorks, bool _IsEmpty = false, string _WorkSource = "", long _DivisionID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Closure Work dropdownlist 
                BindDropdownlist<List<CW_ClosureWork>>(_ddlClosureOperationWorks, null, _FirstOption);
            else
                // Bind Closure Work dropdownlist 
                BindDropdownlist<List<object>>(_ddlClosureOperationWorks, new ScheduleInspectionBLL().GetClosureWorksByWorkSourceAndDivisionID(_WorkSource, _DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }
        #endregion

        #region AssetsWorks
        public static void DDLAssetCategory(DropDownList _ddlAssetCategory, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlAssetCategory, null, _FirstOption);
            else
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlAssetCategory, new AssetsWorkBLL().GetAssetCategoryList(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetSubCategory(DropDownList _ddlAssetSubCategory, long _CategoryID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlAssetSubCategory, null, _FirstOption);
            else
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlAssetSubCategory, new AssetsWorkBLL().GetAssetSubCategoryList(_CategoryID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAttributeType(DropDownList _ddlAttributeType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlAttributeType, CommonLists.AttributeType(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetStatus(DropDownList _ddlstatus, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlstatus, CommonLists.GetAssetStatus(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetsType(DropDownList _ddlAssetsType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlAssetsType, CommonLists.AssetsType(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAccountAssetsType(DropDownList _ddlAssetsType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlAssetsType, CommonLists.AccountAssetsType(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetOffice(DropDownList _ddlAssetOffice, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "OfficeName", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlAssetOffice, null, _FirstOption);
            else
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlAssetOffice, new AssetsWorkBLL().GetAssetOfficeList(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetIrrigationLevel(DropDownList _ddlLevel, long DesignationID, bool isHeadQuarter, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<UA_IrrigationLevel>>(_ddlLevel, new AssetsWorkBLL().GetIrrigationLevelByDesignation(DesignationID, isHeadQuarter), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetViewIrrigationLevel(DropDownList _ddlLevel, long DesignationID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<UA_IrrigationLevel>>(_ddlLevel, new AssetsWorkBLL().GetViewIrrigationLevelByDesignation(DesignationID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetCondition(DropDownList _ddlAssetCondition, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Condition", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlAssetCondition, null, _FirstOption);
            else
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlAssetCondition, new AssetsWorkBLL().GetAssetConditionList(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetInspectionStatus(DropDownList _ddlstatus, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlstatus, CommonLists.GetInspectionStatus(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetInspectionAllStatus(DropDownList _ddlstatus, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlstatus, CommonLists.GetInspectionAllStatus(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAddAssetZones(DropDownList _ddlZone, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_Zone>>(_ddlZone, null, _FirstOption);
            else
                // Bind Zone dropdownlist 
                BindDropdownlist<List<CO_Zone>>(_ddlZone, new AssetsWorkBLL().GetAllZone(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAddAssetCircles(DropDownList _ddlCircle, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Circle dropdownlist 
                BindDropdownlist<List<CO_Circle>>(_ddlCircle, null, _FirstOption);
            else
                // Bind Circle dropdownlist 
                BindDropdownlist<List<CO_Circle>>(_ddlCircle, new AssetsWorkBLL().GetAllCircle(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLAddAssetDivisions(DropDownList _ddlDivision, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<CO_Division>>(_ddlDivision, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<CO_Division>>(_ddlDivision, new AssetsWorkBLL().GetAllDivision(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLCirlceByDivisionlID(DropDownList _ddlCircle, long _DivisionID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Circle>>(_ddlCircle, new AssetsWorkBLL().GetCirlceByDivisionID(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLZoneByCirclelID(DropDownList _ddlZone, long _CircleID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Zone>>(_ddlZone, new AssetsWorkBLL().GetZoneByCirlceID(_CircleID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetsCategory(DropDownList _ddlAssetscategory, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlAssetscategory, CommonLists.AssetsCategory(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetWorksInfrastructureNameByType(DropDownList _ddl, long _UserID, long InfrastructureType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddl, new AssetsWorkBLL().GetInfrastructureName(_UserID, InfrastructureType), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLWorksDivision(DropDownList _ddlDivision, long _Division, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<CO_Division>>(_ddlDivision, new AssetsWorkBLL().GetByID(_Division), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetWorkInfrastructureType(DropDownList _ddl, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddl, CommonLists.AssetWorkInfrastructureTypes(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetWorkChannel(DropDownList _ddlName, long _DivisionID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "NAME", string _DataValueField = "ChannelID")
        {
            if (_IsEmpty)
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlName, null, _FirstOption);
            else
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlName, new WaterTheftBLL().GetChannelByDivisionID(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetWorkChannelOutlet(DropDownList _ddlName, long _DivisionID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlName, null, _FirstOption);
            else
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlName, new WaterTheftBLL().GetAssetWorkOutletByChannelID(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetWorkSmallDams(DropDownList _ddlName, long _DivisionID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlName, null, _FirstOption);
            else
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlName, new AssetsWorkBLL().GetSmallDamByDivisionID(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetWorkSmallDamsChannel(DropDownList _ddlName, long _DivisionID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlName, null, _FirstOption);
            else
                // Bind dropdownlist 
                BindDropdownlist<List<object>>(_ddlName, new AssetsWorkBLL().GetSmallDamChannelByDivisionID(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }
        #endregion

        #region SmallDams
        public static void DDLDamName(DropDownList _ddlDamName, bool _IsEmpty = false, long _SubDivisionID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Sub Division dropdownlist 
                BindDropdownlist<List<SD_SmallDam>>(_ddlDamName, null, _FirstOption);
            else
                // Bind Sub Division dropdownlist 
                BindDropdownlist<List<SD_SmallDam>>(_ddlDamName, new SmallDamsBLL().GetDamNameBySubDivisionID(_SubDivisionID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDamType(DropDownList _ddlDamType, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<SD_DamType>>(_ddlDamType, new SmallDamsBLL().GetAllActiveDamType(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSDDivisionsByUserID(DropDownList _ddlDivision, long _UserID, long _IrrigationLevelID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            BindDropdownlist<List<CO_Division>>(_ddlDivision, new SmallDamsBLL().GetSDDivisionsByUserID(_UserID, _IrrigationLevelID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }


        public static void DDLSDDivisionsByUserAndDamID(DropDownList _ddlDivision, long _UserID, long _DamID, long _IrrigationLevelID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, bool? _IsActive = null, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            BindDropdownlist<List<CO_Division>>(_ddlDivision, new SmallDamsBLL().GetSDDivisionsByUserAndDamID(_UserID, _DamID, _IrrigationLevelID, _IsActive), _FirstOption, _DataTextField, _DataValueField);
        }


        public static void DDLSDDistricts(DropDownList _ddlDistrict, bool _IsEmpty = false, long _DivisionID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind District dropdownlist 
                BindDropdownlist<List<CO_District>>(_ddlDistrict, null, _FirstOption);
            else
                // Bind District dropdownlist 
                BindDropdownlist<List<CO_District>>(_ddlDistrict, new SmallDamsBLL().GetAllSDDistricts(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }


        public static void DDLSDTehsilByDistrictID(DropDownList _ddlTehsil, bool _IsEmpty = false, long _DistrictID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
            {
                BindDropdownlist<List<CO_Tehsil>>(_ddlTehsil, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<CO_Tehsil>>(_ddlTehsil, new SmallDamsBLL().GetAllSDTehsil(_DistrictID), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        public static void DDLSDVillagesByTehsilID(DropDownList _ddlVillages, bool _IsEmpty = false, long _TehsilID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
            {
                BindDropdownlist<List<CO_Village>>(_ddlVillages, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<CO_Village>>(_ddlVillages, new SmallDamsBLL().GetAllSDVillages(_TehsilID), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        public static void DDLSpillwayType(DropDownList _ddlSpillwayType, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<SD_SpillwayType>>(_ddlSpillwayType, new SmallDamsBLL().GetAllSpillwayType(), _FirstOption, _DataTextField, _DataValueField);
        }



        //public static void DDLGrossStorageCapacity(DropDownList _ddlGrossStorageCapacity, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        //{
        //    BindDropdownlist<List<dynamic>>(_ddlGrossStorageCapacity, CommonLists.GetGrossStorageCapacitySD(), _FirstOption, _DataTextField, _DataValueField);
        //}

        public static void DDLPrentTypeSD(DropDownList _ddlPrentType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlPrentType, CommonLists.GetParentTypeSD(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLOffTakingSidesSD(DropDownList _ddlOffTakingSides, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlOffTakingSides, CommonLists.GetOffTakingSidesSD(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDamNameByID(DropDownList _ddlDamName, bool _IsEmpty = false, long _DamID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Sub Division dropdownlist 
                BindDropdownlist<List<SD_SmallDam>>(_ddlDamName, null, _FirstOption);
            else
                // Bind Sub Division dropdownlist 
                BindDropdownlist<List<SD_SmallDam>>(_ddlDamName, new SmallDamsBLL().GetDamNameByDamID(_DamID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLChannelsByDamID(DropDownList _ddlChannel, bool _IsEmpty = false, long _DamID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                // Bind Sub Division dropdownlist 
                BindDropdownlist<List<SD_SmallChannel>>(_ddlChannel, null, _FirstOption);
            else
                // Bind Sub Division dropdownlist 
                BindDropdownlist<List<SD_SmallChannel>>(_ddlChannel, new SmallDamsBLL().GetChannelByDamID(_DamID), _FirstOption, _DataTextField, _DataValueField);
        }
        #endregion

        #region FloodBundData
        public static void DDLStructureTypeFloodBund(DropDownList _ddlAssetsType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlAssetsType, CommonLists.StructureTypesForFloodBundRef(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLStructureTypesForFloodBund(DropDownList _ddlAssetsType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlAssetsType, CommonLists.StructureTypesForFloodBund(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLStructureNameFloodBund(DropDownList _ddl, long _UserID, string StructureType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddl, new FloodOperationsBLL().GetStructureNameFloodBund(_UserID, StructureType), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLFloodGaugesRDs(DropDownList _ddl, long _StructureID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            BindDropdownlist<List<object>>(_ddl, new FloodOperationsBLL().GetAllFloodGaugesRDs(_StructureID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLTime(DropDownList _ddl, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            BindDropdownlist<List<string>>(_ddl, new FloodOperationsBLL().GetTimeListFloodBund(), _FirstOption, _DataTextField, _DataValueField);
        }


        #endregion

        #region Accounts

        #region ResourceAllocation
        public static void DDLADM(DropDownList _ddlADM, bool _ISEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_ISEmpty)
            {
                BindDropdownlist<List<dynamic>>(_ddlADM, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<dynamic>>(_ddlADM, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetADMUser(), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        public static void DDLOfficeADMusers(DropDownList _ddlADM, bool _ISEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID", string staff = "F")
        {

            BindDropdownlist<List<dynamic>>(_ddlADM, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetOfficeUsers(staff), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLADMDesignation(DropDownList _ddlDesignation, int StaffType, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {

            Dropdownlist.BindDropdownlist<List<object>>(_ddlDesignation, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetADMDesignationID(StaffType), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLACCNameofStaff(DropDownList _ddlNameofStaff, bool _IsEmpty = false, long _DesignationID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
            {
                Dropdownlist.BindDropdownlist<List<object>>(_ddlNameofStaff, null, _FirstOption);
            }
            else
            {
                Dropdownlist.BindDropdownlist<List<object>>(_ddlNameofStaff, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetNameofStaffByDesignationID(_DesignationID), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        #endregion

        #region AssetsAllocation
        public static void DDLAACategory(DropDownList _ddlCategory, bool _ISEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_ISEmpty)
            {
                BindDropdownlist<List<dynamic>>(_ddlCategory, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<dynamic>>(_ddlCategory, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetAACategory(), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        public static void DDLAASubCategory(DropDownList _ddlSubCategory, bool _ISEmpty = false, long _CategoryID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_ISEmpty)
            {
                BindDropdownlist<List<dynamic>>(_ddlSubCategory, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<dynamic>>(_ddlSubCategory, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetAASubCategory(_CategoryID), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        public static void DDLAAAssetType(DropDownList _ddlAssetType, bool _ISEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_ISEmpty)
            {
                BindDropdownlist<List<dynamic>>(_ddlAssetType, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<dynamic>>(_ddlAssetType, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetAAAssetType(), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        public static void DDLAAAssetName(DropDownList _ddlAssetName, bool _ISEmpty = false, long _SubCategory = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_ISEmpty)
            {
                BindDropdownlist<List<dynamic>>(_ddlAssetName, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<dynamic>>(_ddlAssetName, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetAAAssetName(_SubCategory), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        public static void DDLAAAssetAttribute(DropDownList _ddlAssetAttribute, bool _ISEmpty = false, long _AssetID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_ISEmpty)
            {
                BindDropdownlist<List<dynamic>>(_ddlAssetAttribute, null, _FirstOption);
            }
            else
            {
                if (_AssetID == -2)
                {
                    BindDropdownlist<List<dynamic>>(_ddlAssetAttribute, CommonLists.GetAssetAllocationAttributeType(), _FirstOption, _DataTextField, _DataValueField);
                }
                else
                {
                    BindDropdownlist<List<dynamic>>(_ddlAssetAttribute, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetAAAssetAttribute(_AssetID), _FirstOption, _DataTextField, _DataValueField);
                }
            }

        }


        #endregion

        public static void DDLExpenseType(DropDownList _ddlExpenseType, string _Source = null, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "ExpenseType", string _DataValueField = "ID")
        {
            BindDropdownlist<List<AT_ExpenseType>>(_ddlExpenseType, new PMIU.WRMIS.BLL.Accounts.AccountsBLL().GetExpenseTypes(_Source), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLFinancialYear(DropDownList _ddlFinancialYear, string _Source = null, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Text", string _DataValueField = "Value")
        {
            BindDropdownlist<List<ListItem>>(_ddlFinancialYear, Utility.GetFinancialYear(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLFundReleaseType(DropDownList _ddlFundReleaseType, string _Source = null, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "TypeName", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlFundReleaseType, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetFundsReleaseType(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLMonthList(DropDownList _ddlMonthList, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlMonthList, CommonLists.GetMonthList(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLAssetType(DropDownList _ddlSanctionOn, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "AssetType", string _DataValueField = "ID")
        {
            BindDropdownlist<List<AT_AssetType>>(_ddlSanctionOn, new AccountsBLL().GetAssetTypes(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSanctionStatus(DropDownList _ddlSanctionStatus, int _FirstOption = (int)Constants.DropDownFirstOption.NoOption, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<AT_SanctionStatus>>(_ddlSanctionStatus, new AccountsBLL().GetSanctionStatus(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLResourceAssets(DropDownList _ddlResourceAssets, long _ResourceAllocationID, List<long> _AssetTypeIDs = null, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "AssetName", string _DataValueField = "AssetName")
        {
            Dropdownlist.BindDropdownlist<List<dynamic>>(_ddlResourceAssets, new AccountsBLL().GetAssetsByResourceIDAndType(_ResourceAllocationID, _AssetTypeIDs), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLObjectClassification(DropDownList _ddlObjectClassification, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            Dropdownlist.BindDropdownlist<List<dynamic>>(_ddlObjectClassification, new AccountsBLL().GetAllObjectClassificationsAndCode(), _FirstOption, _DataTextField, _DataValueField);
        }

        #region BudgetUtilization
        public static void DDLAccountHeadList(DropDownList _ddlHeadList, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "HeadName", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddlHeadList, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetAccountHeadByID(), _FirstOption, _DataTextField, _DataValueField);
        }
        #endregion

        #endregion

        #region Effluent
        public static void DDLEffluentSreviceType(DropDownList _ddlSreviceType, string _Source = null, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlSreviceType, Utility.GetEffluentSreviceType(), _FirstOption, _DataTextField, _DataValueField);
        }
        #endregion

        #region AccountReports
        public static void DDLAccountTaxFor(DropDownList _ddlTaxFor, string _Source = null, int _FirstOption = (int)Constants.DropDownFirstOption.All, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlTaxFor, Utility.GetTaxforType(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAccountTaxOn(DropDownList _ddlTaxOn, string _Source = null, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlTaxOn, Utility.GetTaxOnType(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLACObjectClassification(DropDownList _ddlACObjectClassification, bool _ISEmpty = false, long _ACHeadID = -1, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_ISEmpty)
            {
                BindDropdownlist<List<dynamic>>(_ddlACObjectClassification, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<dynamic>>(_ddlACObjectClassification, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetACObjectClassification(_ACHeadID), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        public static void DDLObjectClassificationCode(DropDownList _ddlObjectClassificationCode, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlObjectClassificationCode, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetObjectClassificationCode(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSanctionON(DropDownList _ddlSanctionOn, string _Source = null, int _FirstOption = (int)Constants.DropDownFirstOption.All, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<dynamic>>(_ddlSanctionOn, Utility.GetSanctionOnType(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSanctionNO(DropDownList _ddlSanctionNO, bool _ISEmpty = false, string Year = "", string Month = "", string SanctionOn = "", int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_ISEmpty)
            {
                BindDropdownlist<List<dynamic>>(_ddlSanctionNO, null, _FirstOption);
            }
            else
            {
                BindDropdownlist<List<RPT_AT_SanctionOnDropdown_Result>>(_ddlSanctionNO, new PMIU.WRMIS.BLL.Accounts.ReferenceDataBLL().GetSanctionNO(Year, Month, SanctionOn), _FirstOption, _DataTextField, _DataValueField);
            }

        }

        #endregion

        #region Assets Reports

        public static void DDLIrrigationLevelForReports(DropDownList _ddlDivision, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            BindDropdownlist<List<UA_IrrigationLevel>>(_ddlDivision, new AssetsWorkBLL().GetIrrigationLevelForReports(), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAttributeForReports(DropDownList _ddl, long? Catid, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "AttributeName", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddl, new AssetsWorkBLL().GetAttributeList(Catid), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAttributeValueForReports(DropDownList _ddl, long? attriID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "AttributeValue", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddl, new AssetsWorkBLL().GetAttributeValueList(attriID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetNameForReports(DropDownList _ddl, long? Catid, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "AssetName", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddl, new AssetsWorkBLL().GetAssetName(Catid), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLAssetNameForReports(DropDownList _ddl, long? Catid, long? IrrigationLevelID, long? IrrigationBoundaryID, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "AssetName", string _DataValueField = "ID")
        {
            BindDropdownlist<List<object>>(_ddl, new AssetsWorkBLL().GetAssetName(Catid, IrrigationLevelID, IrrigationBoundaryID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLDivisionByChannelID(DropDownList _ddl,bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<object>(_ddl, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<object>(_ddl, new ChannelBLL().GetDivisionNameByIrrigationDomain(), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLDivisionByChannel_ID(DropDownList _ddl, long _CircleID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<object>(_ddl, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<object>(_ddl, new ChannelBLL().GetDivisionNameByChannel_ID(_CircleID), _FirstOption, _DataTextField, _DataValueField);
        }

        public static void DDLSubDivisionByChannelID(DropDownList _ddl, long _DivisionID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<dynamic>>(_ddl, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<dynamic>>(_ddl, new ChannelBLL().GetSubDivisionNameByChannelID(_DivisionID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLSectionByChannelID(DropDownList _ddl, long _SubDivID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<dynamic>>(_ddl, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<dynamic>>(_ddl, new ChannelBLL().GetSectionNameByChannelID(_SubDivID), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLGetChannelParentFeedersNameByChannelID(DropDownList _ddl, long _ChannelID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.Select, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<dynamic>>(_ddl, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<dynamic>>(_ddl, new ChannelBLL().GetChannelParentFeedersNameByChannelID(_ChannelID), _FirstOption, _DataTextField, _DataValueField);
        }

        #endregion
        public static void DDLMeterReading(DropDownList _ddl, long _ID, long _Designations, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.All, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<dynamic>>(_ddl, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<dynamic>>(_ddl, new DailyDataBLL().GetMA_ADMUser(_ID, _Designations), _FirstOption, _DataTextField, _DataValueField);
        }
        public static void DDLGetMA(DropDownList _ddl, long _UserID, bool _IsEmpty = false, int _FirstOption = (int)Constants.DropDownFirstOption.All, string _DataTextField = "Name", string _DataValueField = "ID")
        {
            if (_IsEmpty)
                BindDropdownlist<List<dynamic>>(_ddl, null, _FirstOption);
            else
                // Bind Division dropdownlist 
                BindDropdownlist<List<dynamic>>(_ddl, new DailyDataBLL().GetMAUser(_UserID), _FirstOption, _DataTextField, _DataValueField);
        }

    }
}