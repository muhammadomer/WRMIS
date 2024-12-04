using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.Common
{
    public class CommonLists
    {
        /// <summary>
        /// This function return Active, Inactive Status
        /// Created on: 12-01-2016
        /// </summary>
        /// <param name="_SideID"></param>
        /// <returns>List<Dynamic></returns>
        public static List<dynamic> GetStatuses(string _StatusID = null)
        {
            List<dynamic> lstStatuses = new List<dynamic>() 
            {
                new { ID="1", Name="Active"},
                new { ID="2", Name="Inactive"}
            };

            List<dynamic> lstStatus = lstStatuses.Where(l => l.ID == _StatusID || _StatusID == null).ToList<dynamic>();
            return lstStatus;
        }
        /// <summary>
        /// This function return Channel Sides
        /// Created on: 10-11-2015
        /// </summary>
        /// <param name="_SideID"></param>
        /// <param name="_RemoveBoth">Remove 'Left Right' item</param>
        /// <returns>List<Dynamic></returns>
        public static List<dynamic> GetChannelSides(string _SideID = null, bool _RemoveBoth = false)
        {
            List<dynamic> lstChannelSides = new List<dynamic>()
            {
                new {ID="L", Name="Left"},
                new {ID="R", Name="Right"},
                new {ID="B", Name="Left Right"}


            };

            if (_RemoveBoth)
                lstChannelSides.RemoveAt(2);


            List<dynamic> lstSides = lstChannelSides.Where(s => s.ID == _SideID || _SideID == null).ToList<dynamic>();

            return lstSides;
        }
        //public static List<dynamic> GetOutletSides(string _SideID = null)
        //{
        //    List<dynamic> lstOutletSides = new List<dynamic>()
        //    {
        //        new {ID="1", Name="Left"},
        //        new {ID="2", Name="Right"},
        //        new {ID="3", Name="Tail Left"},
        //        new {ID="4", Name="Tail Right"},
        //        new {ID="5", Name="Tail Front"}
        //    };

        //    List<dynamic> lstSides = lstOutletSides.Where(s => s.ID == _SideID || _SideID == null).ToList<dynamic>();

        //    return lstSides;
        //}
        public static List<dynamic> GetOutletSides(string _SideID = null)
        {
            List<dynamic> lstOutletSides = new List<dynamic>()
            {
                new {ID="L", Name="Left"},
                new {ID="R", Name="Right"},
                new {ID="TL", Name="Tail Left"},
                new {ID="TR", Name="Tail Right"},
                new {ID="TF", Name="Tail Front"}
            };

            List<dynamic> lstSides = lstOutletSides.Where(s => s.ID == _SideID || _SideID == null).ToList<dynamic>();

            return lstSides;
        }
        /// <summary>
        /// This function returns Channel relationship types
        /// Created on: 10-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_RemoveFeeder">Remove Feeder because Barrage/Headworks can not be feeder of a channel</param>
        /// <returns>List<dynamic></returns>
        public static List<dynamic> GetChannelRelationshipTypes(string _RelationshipID = null, bool _RemoveFeeder = false)
        {
            List<dynamic> lstRelationshipTypes = new List<dynamic>()
            {
                new {ID="P", Name = "Parent"},
                new {ID="F", Name = "Feeder"}
            };
            if (_RemoveFeeder)
                lstRelationshipTypes.RemoveAt(1);

            List<dynamic> lstRelationships = lstRelationshipTypes.Where(t => t.ID == _RelationshipID || _RelationshipID == null).ToList<dynamic>();

            return lstRelationships;
        }

        public static List<dynamic> GetYesNo(string _ID = null)
        {
            List<dynamic> lstYesNo = new List<dynamic>()
            {
                new {ID="1", Name = "Yes"},
                new {ID="0", Name = "No"}
            };

            List<dynamic> lstValues = lstYesNo.Where(t => t.ID == _ID || _ID == null).ToList<dynamic>();

            return lstValues;
        }
        public static List<dynamic> GetUstreamDstream(string _ID = null)
        {
            List<dynamic> lstYesNo = new List<dynamic>()
            {
                new {ID="1", Name = "Upstream"},
                new {ID="0", Name = "Downstream"}
            };

            List<dynamic> lstValues = lstYesNo.Where(t => t.ID == _ID || _ID == null).ToList<dynamic>();

            return lstValues;
        }
        public static List<dynamic> GetAvailableNotAvailable(string _ID = null)
        {
            List<dynamic> lstYesNo = new List<dynamic>()
            {
                new {ID="1", Name = "Available"},
                new {ID="0", Name = "Not Available"}
            };

            List<dynamic> lstValues = lstYesNo.Where(t => t.ID == _ID || _ID == null).ToList<dynamic>();

            return lstValues;
        }
        /// <summary>
        /// This function return Active, Inactive Status
        /// Created on: 12-01-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<Dynamic></returns>
        public static List<dynamic> GetLinedOrUnlined(string _ID = null)
        {
            List<dynamic> lstLinedOrUnlined = new List<dynamic>() 
            {
                new { ID="1", Name="Unlined"},
                new { ID="0", Name="Lined"}
            };

            List<dynamic> lstLinedUnlined = lstLinedOrUnlined.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstLinedUnlined;
        }
        /// <summary>
        /// This function return Morning, Evening session
        /// Created on: 12-02-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<Dynamic></returns>
        public static List<dynamic> GetSession(string _ID = null)
        {
            List<dynamic> lstSessions = new List<dynamic>() 
            {
                new { ID="1", Name="Morning"},
                new { ID="2", Name="Evening"}
            };

            List<dynamic> lstSessionValue = lstSessions.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstSessionValue;
        }
        /// <summary>
        /// This function return Gauge Categories
        /// Created on: 02-05-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<Dynamic></returns>
        public static List<dynamic> GetGaugeCategories(string _ID = null)
        {
            List<dynamic> lstGaugeCategory = new List<dynamic>()
            {
                //new {Name="Sectional Gauge",ID="5"},
                //new {Name="Head Gauge",ID="1"},
                //new {Name="Tail Gauge",ID="2"},
                new {Name="Critical Gauge", ID="6"}
            };

            List<dynamic> lstGaugeCategoryValue = lstGaugeCategory.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstGaugeCategoryValue;
        }
        /// <summary>
        /// This function return Gauge at Bed
        /// Created on: 02-05-2016
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>List<Dynamic></returns>
        public static List<dynamic> GetGaugeLevels(string _ID = null)
        {
            List<dynamic> lstGaugeLevel = new List<dynamic>()
            {
                new {Name="Bed Level",ID="1"},
                new {Name="Crest Level", ID="2"}
            };

            List<dynamic> lstGaugeLevelValue = lstGaugeLevel.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstGaugeLevelValue;
        }

        public static List<dynamic> GetOffenceSite(string _ID = null)
        {
            List<dynamic> lstOffenceSite = new List<dynamic>()
            {
                new {Name="Channel",ID="C"},
                new {Name="Outlet", ID="O"}
            };

            List<dynamic> lstOffenceSiteValue = lstOffenceSite.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstOffenceSiteValue;
        }

        public static List<dynamic> GetWTChannelSides(string _ID = null)
        {
            List<dynamic> lstChannelSide = new List<dynamic>()
            {
                new {Name="Left",ID="L"},
                new {Name="Right", ID="R"},
                new {Name="Tail Left",ID="TL"},
                new {Name="Tail Right", ID="TR"},
                new {Name="Tail Front", ID="TF"}
            };

            List<dynamic> lstChannelSideValue = lstChannelSide.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstChannelSideValue;
        }

        public static List<dynamic> GetTailStatuses(string _StatusID = null)
        {
            List<dynamic> lstTailStatus = new List<dynamic>() 
            {
                new { ID="1", Name="Dry"},
                new { ID="2", Name="Short Tail"},
                new { ID="3", Name="Authorized Tail"},
                new { ID="4", Name="Excessive Tail"}
                //new { ID="5", Name="Chronic Dry"}
            };

            List<dynamic> lstStatus = lstTailStatus.Where(l => l.ID == _StatusID || _StatusID == null).ToList<dynamic>();
            return lstStatus;
        }

        public static List<dynamic> GetBreachSides(string _ID = null)
        {
            List<dynamic> lstChannelSide = new List<dynamic>()
            {
                new {Name="Left",ID="L"},
                new {Name="Right", ID="R"}
            };

            List<dynamic> lstChannelSideValue = lstChannelSide.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstChannelSideValue;
        }

        public static List<dynamic> GetAlertStatus(string _StatusID = null)
        {
            List<dynamic> lstAlertStatus = new List<dynamic>() 
            {
                new { ID="0", Name="All"},
                new { ID="1", Name="Unread"},
                new { ID="2", Name="Read"},
            };

            List<dynamic> lstStatus = lstAlertStatus.Where(l => l.ID == _StatusID || _StatusID == null).ToList<dynamic>();
            return lstStatus;
        }

        public static List<dynamic> GetSeasonDropDown()
        {
            List<dynamic> lstSeason = new List<dynamic>() 
            {                
                new { ID="1", Name="Rabi"},
                new { ID="2", Name="Kharif"},
            };

            return lstSeason;
        }


        public static List<dynamic> GetTDailyDropDown()
        {
            List<dynamic> lstSeason = new List<dynamic>() 
            {                
                new { ID="1", Name="1st Ten Daily"},
                new { ID="2", Name="2nd Ten Daily"},
                new { ID="3", Name="3rd Ten Daily"},
            };

            return lstSeason;
        }
        public static List<dynamic> GetRimStations()
        {
            List<dynamic> lstRimStation = new List<dynamic>() 
            {
                new { ID="", Name="Select"},
                new { ID="20", Name="Tarbela"},
                new { ID="18", Name="Mangla"}
            };

            return lstRimStation;
        }

        public static List<dynamic> GetAllRimStations()
        {
            List<dynamic> lstRimStation = new List<dynamic>() 
            {
                new { ID="", Name="Select"},
                new { ID="20", Name="Indus at Tarbela"},
                new { ID="18", Name="Jhelum at Mangla"},
                new { ID="5", Name="Chenab at Marala"},
                new { ID="24", Name="Kabul at Nowshera"}
            };

            return lstRimStation;
        }

        /// <summary>
        /// This function return Parent Type
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <returns>List</returns>
        /// <created>9/15/2016</created>
        /// <changed>9/15/2016</changed>
        public static List<dynamic> GetParentType()
        {
            List<dynamic> lstParentType = new List<dynamic>() 
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Protection Infrastructure"},
                new { ID="2", Name="Barrage/Headwork"},
                new { ID="3", Name="River"},
                new { ID="4", Name="Channels"}
            };

            return lstParentType;
        }

        public static List<dynamic> StructuresByDomainIrrigation()
        {
            List<dynamic> lstStructuresByDomain = new List<dynamic>() 
            {
                new { ID="6", Name="Channel"},
                new { ID="7", Name="Outlet"},
                new { ID="8", Name="Barrage/Headwork"},
                new { ID="9", Name="Protection Infrastructure"}
                
                
            };

            return lstStructuresByDomain;
        }

        public static List<dynamic> StructuresByDomainDandF()
        {
            List<dynamic> lstStructuresByDomain = new List<dynamic>() 
            {
                new { ID="1", Name="Protection Infrastructure"},
                new { ID="2", Name="Drain"}
            };

            return lstStructuresByDomain;
        }


        public static List<dynamic> MajorMinor()
        {
            List<dynamic> lstMajorMinor = new List<dynamic>() 
            {
                new { ID="Major", Name="Major"},
                new { ID="Minor", Name="Minor"}
            };

            return lstMajorMinor;
        }

        /// <summary>
        /// This function return Outfall Type
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <returns>List</returns>
        /// <created>9/28/2016</created>
        /// <changed>9/28/2016</changed>
        public static List<dynamic> GetOutfallType()
        {
            List<dynamic> lstOutfallType = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="River"},
                new { ID="2", Name="Drain"},

            };

            return lstOutfallType;
        }

        /// <summary>
        /// This function return Inspection Categories
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <returns>List</returns>
        /// <created>10/05/2016</created>
        /// <changed>10/05/2016</changed>
        public static List<dynamic> GetInspectionCategories()
        {
            List<dynamic> lstInspectionCategories = new List<dynamic>()
            {
                new { ID="1", Name="Scheduled"},
                new { ID="2", Name="Unscheduled"}
            };

            return lstInspectionCategories;
        }

        /// <summary>
        /// This function return Inspection Categories
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <returns>List</returns>
        /// <created>10/05/2016</created>
        /// <changed>10/05/2016</changed>
        public static List<dynamic> GetInspectionTypes()
        {
            List<dynamic> lstInspectionType = new List<dynamic>()
            {
                new { ID="1", Name="Gauge Insepction"},
                new { ID="2", Name="Discharge Table Calculation - Bed Level"},
                new { ID="3", Name="Discharge Table Calculation - Crest Level"},
                new { ID="4", Name="Outlet Performance"},
                new { ID="9", Name="Outlet Checking"},
                new { ID="5", Name="Outlet Alteration"},
                new { ID="6", Name="Tender Monitoring"},
                new { ID="7", Name="Works Inpections"},
                new { ID="8", Name="General Inspections"}
            };

            return lstInspectionType;
        }

        public static List<dynamic> GetAuctionTypes()
        {
            List<dynamic> lstAuctionTypes = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Open Bidding"},
                new { ID="2", Name="Sealed Bidding"}
           
            };

            return lstAuctionTypes;
        }

        public static List<dynamic> GetStatusForXEN()
        {
            List<dynamic> lstStatuses = new List<dynamic>()
            {
                new { ID="1", Name="Pending"},
                new { ID="2", Name="Deliver"}
           
            };

            return lstStatuses;
        }
        public static List<dynamic> GetStatusForSE()
        {
            List<dynamic> lstStatuses = new List<dynamic>()
            {
                new { ID="1", Name="Approve"},
                new { ID="2", Name="Cancel"}
           
            };

            return lstStatuses;
        }
        public static List<dynamic> GetAuctionCategories()
        {
            List<dynamic> lstAuctionCategories = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Temporary/Lease"},
                new { ID="2", Name="Permanent"}
           
            };

            return lstAuctionCategories;
        }

        public static List<dynamic> GetAuctionLevels()
        {
            List<dynamic> lstAuctionLevels = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Zone"},
                new { ID="2", Name="Circle"},
                new { ID="3", Name="Division"}
           
            };

            return lstAuctionLevels;
        }

        public static List<dynamic> GetEarnestTokenMoney()
        {
            List<dynamic> lstEarnestOrTokenMoney = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Lumpsum"},
                new { ID="2", Name="%"}
                
            };

            return lstEarnestOrTokenMoney;
        }
        public static List<dynamic> GetAuctionIndividualGrouped()
        {
            List<dynamic> lstIndividualGrouped = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Individual"},
                new { ID="2", Name="Group"}
               };

            return lstIndividualGrouped;
        }
        public static List<dynamic> GetPaymentTypes()
        {
            List<dynamic> lstPaymentTypes = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Token Money"},
                new { ID="2", Name="Remaining Amount"}
               };

            return lstPaymentTypes;
        }

        public static List<dynamic> GetWorkSource()
        {
            List<dynamic> lstWorkSource = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="CLOSURE"},
                new { ID="2", Name="WORKS"}

            };

            return lstWorkSource;
        }
        /// <summary>
        /// This function return Outfall Side
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <returns>List</returns>
        /// <created>9/28/2016</created>
        /// <changed>9/28/2016</changed>
        public static List<dynamic> GetOutfallSide()
        {
            List<dynamic> lstOutfallSide = new List<dynamic>()
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Right"},
                new { ID="2", Name="Left"},

            };

            return lstOutfallSide;
        }

        public static List<dynamic> GetComplaintTypeAndSource(string _ID = null)
        {
            List<dynamic> lstComplaints = new List<dynamic>()
            {
                new {Name="Complaint Type",ID="1"},
                new {Name="Complaint Source", ID="2"}
            };

            List<dynamic> lstComplaintsValue = lstComplaints.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstComplaintsValue;
        }

        public static List<dynamic> GetRimStationsForElevationCapacity()
        {
            List<dynamic> lstRimStation = new List<dynamic>() 
            {
                new { ID="", Name="Select"},
                new { ID="20", Name="Indus at Tarbela"},
                new { ID="18", Name="Jhelum at Mangla"},                
                new { ID="2", Name="Chashma"}
            };

            return lstRimStation;
        }
        public static List<dynamic> GateTypes(string _ID = null)
        {
            List<dynamic> lstGateTypes = new List<dynamic>()
            {
                new {Name="Radial",ID="R"},
                new {Name="Vertical Lift", ID="VL"}
            };

            List<dynamic> lstGateTypesValue = lstGateTypes.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstGateTypesValue;
        }
        public static List<dynamic> InfrastructureTypes(string _ID = null)
        {
            List<dynamic> lstInfrastructureTypes = new List<dynamic>()
            {
                 
                new { ID="1", Name="Protection Infrastructure"},
                new { ID="2", Name="Barrage/Headwork"},
                new { ID="3", Name="Drain"},
            };

            List<dynamic> lstInfrastructureTypesValue = lstInfrastructureTypes.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstInfrastructureTypesValue;
        }

        public static List<dynamic> GetGaugeCorrectionType(string _GaugeCorrectionTypeID = null)
        {
            List<dynamic> lstGaugeCorrectionTypes = new List<dynamic>() 
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Bed Silted"},
                new { ID="2", Name="Bed Scoured"}
            };

            List<dynamic> lstStatus = lstGaugeCorrectionTypes.Where(l => l.ID == _GaugeCorrectionTypeID || _GaugeCorrectionTypeID == null).ToList<dynamic>();
            return lstStatus;
        }

        public static List<dynamic> GetCommands()
        {
            List<dynamic> lstCommands = new List<dynamic>() 
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="Indus"},
                new { ID="2", Name="Jhelum Chenab"}
            };

            return lstCommands;
        }

        public static List<dynamic> GetTenderOffices(string _ID = null)
        {
            List<dynamic> lstTenderOffices = new List<dynamic>()
            {
                new {Name="Zone",ID="Z"},
                new {Name="Circle", ID="C"},
                 new {Name="Division", ID="D"},
                  new {Name="Other Offices", ID="OO"}
            };

            List<dynamic> lstOffenceSiteValue = lstTenderOffices.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstOffenceSiteValue;
        }

        public static List<dynamic> GetTSType(string _ID = null)
        {
            List<dynamic> lstOffenceSite = new List<dynamic>()
            {
                new {Name="As Per T.S",ID="1"},
                new {Name="Above T.S", ID="2"},
                new {Name="Below T.S", ID="3"},
                new {Name="Item Rate", ID="4"}
            };

            List<dynamic> lstOffenceSiteValue = lstOffenceSite.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstOffenceSiteValue;
        }
        #region "Reports"
        public static List<dynamic> GetHeadDischarge(string _HeadDischargeID = null)
        {
            List<dynamic> lstHeadDischarges = new List<dynamic>()  
            {
                new { ID="1", Name="Authorized"},
                new { ID="2", Name="Excessive"},
                new { ID="3", Name="Short"},
                new { ID="4", Name="Greater than or Equal to Design Discharge"}

            };

            List<dynamic> lstHeadDischarge = lstHeadDischarges.Where(l => l.ID == _HeadDischargeID || _HeadDischargeID == null).ToList<dynamic>();
            return lstHeadDischarge;
        }
        public static List<dynamic> GetFieldStaffTailStatus(string _TailStatusID = null)
        {
            List<dynamic> lstTailStatuses = new List<dynamic>()  
            {
                new { ID="1", Name="Dry"},
                new { ID="2", Name="Short"},
                new { ID="3", Name="Authorized"},
                new { ID="4", Name="Excessive"}
            };

            List<dynamic> lstTailStatus = lstTailStatuses.Where(l => l.ID == _TailStatusID || _TailStatusID == null).ToList<dynamic>();
            return lstTailStatus;
        }
        public static List<dynamic> GetGaugeComparison(string _GaugeComparisonID = null)
        {
            List<dynamic> lstGaugeComparisones = new List<dynamic>()  
            {
                new { ID="1", Name="Difference is Less than 0.2"},
                new { ID="2", Name="Difference is More than +0.2"},
                new { ID="3", Name="Difference is More than -0.2"},
                new { ID="4", Name="Difference is More than +-0.2"},
                new { ID="5", Name="Same values"}
            };

            List<dynamic> lstGaugeComparison = lstGaugeComparisones.Where(l => l.ID == _GaugeComparisonID || _GaugeComparisonID == null).ToList<dynamic>();
            return lstGaugeComparison;
        }
        public static List<dynamic> GetGaugeStatus(string _GaugeStatusID = null)
        {
            List<dynamic> lstGaugeStatuses = new List<dynamic>()  
            {
                new { ID="1", Name="Fixed"},
                new { ID="2", Name="Painted"},
                new { ID="3", Name="Fixed and Painted"},
                new { ID="4", Name="Not Fixed"},
                new { ID="5", Name="Not Painted"},
                new { ID="6", Name="Not Fixed & Not Painted"},
                new { ID="7", Name="Fixed but Not Painted"},
                new { ID="8", Name="Not Fixed but Painted"}
            };

            List<dynamic> lstGaugeStatus = lstGaugeStatuses.Where(l => l.ID == _GaugeStatusID || _GaugeStatusID == null).ToList<dynamic>();
            return lstGaugeStatus;
        }
        public static List<dynamic> GetGaugeStatusComparison(string _GaugeStatusComparisonID = null)
        {
            List<dynamic> lstGaugeStatusComparisones = new List<dynamic>()  
            {
                new { ID="1", Name="No"},
                new { ID="2", Name="Yes"}
            };

            List<dynamic> lstGaugeStatusComparison = lstGaugeStatusComparisones.Where(l => l.ID == _GaugeStatusComparisonID || _GaugeStatusComparisonID == null).ToList<dynamic>();
            return lstGaugeStatusComparison;
        }
        public static List<dynamic> GetIndentPlacements(string _IndentPlacementID = null)
        {
            List<dynamic> lstIndentPlacements = new List<dynamic>()  
            {
                new { ID="1", Name="Nil Indent"},
                new { ID="2", Name="Less than Design Discharge"},
                new { ID="3", Name="Greater than Design Discharge"},
                new { ID="4", Name="Equals to 10% of Design Discharge"},
                new { ID="5", Name="Greater than 10% of the Design Discharge"},
                new { ID="6", Name="Less than 10% of the Design Discharge"},
                new { ID="7", Name="Top 10 Highest Indents"},
                new { ID="8", Name="Top 10 Lowest Indents"}
            };

            List<dynamic> lstIndentPlacement = lstIndentPlacements.Where(l => l.ID == _IndentPlacementID || _IndentPlacementID == null).ToList<dynamic>();
            return lstIndentPlacement;
        }
        public static List<dynamic> GetGaugeReadingsChangedBy(string _ReadingsChangedByID = null)
        {
            List<dynamic> lstGaugeReadingsChangedBy = new List<dynamic>()   
            {
                new { ID="5", Name="SDO"},
                new { ID="4", Name="XEN"},
                new { ID="14", Name="PMIU Data Analyst"},
                new { ID="8", Name="Gauge Reader"}
            };

            List<dynamic> lstGaugeReadingChangedBy = lstGaugeReadingsChangedBy.Where(l => l.ID == _ReadingsChangedByID || _ReadingsChangedByID == null).ToList<dynamic>();
            return lstGaugeReadingChangedBy;
        }
        public static List<dynamic> GetComparsionYears(string _ComparsionID = null)
        {
            List<dynamic> lstComparsionYears = new List<dynamic>()   
            {
                new { ID="2010", Name="2010-11"},
                new { ID="2011", Name="2011-12"},
                new { ID="2012", Name="2012-13"},
                new { ID="2013", Name="2013-14"},
                new { ID="2014", Name="2014-15"},
                new { ID="2015", Name="2015-16"},
                new { ID="2016", Name="2016-17"}
            };

            List<dynamic> lstComparsionYear = lstComparsionYears.Where(l => l.ID == _ComparsionID || _ComparsionID == null).ToList<dynamic>();
            return lstComparsionYear;
        }
        public static List<dynamic> GetScheduleInspectionInspectedBy(string _InspectedByID = null)
        {
            List<dynamic> lstScheduleInspectionInspectedBy = new List<dynamic>()   
            {
                new { ID="13", Name="MA"},
                new { ID="12", Name="ADM"},
                new { ID="8", Name="Gauge Reader"},
                new { ID="6", Name="SBE"},
                new { ID="5", Name="SDO"},
                new { ID="4", Name="XEN"},
                new { ID="3", Name="SE"},
                new { ID="2", Name="CE"}
            };

            List<dynamic> lstInspectedBy = lstScheduleInspectionInspectedBy.Where(l => l.ID == _InspectedByID || _InspectedByID == null).ToList<dynamic>();
            return lstInspectedBy;
        }

        public static List<dynamic> GetEfficiency(string _EfficiencyID = null)
        {
            List<dynamic> lstEfficiencies = new List<dynamic>()   
            {
                new { ID="1", Name="100%"},
                new { ID="2", Name="100%-110%"},
                new { ID="3", Name="More than 110%"},
                new { ID="4", Name="90%-99%"},
                new { ID="5", Name="80%-89%"},
                new { ID="6", Name="70%-79%"},
                new { ID="7", Name="Less than 70%"}
            };

            List<dynamic> lstEfficiency = lstEfficiencies.Where(l => l.ID == _EfficiencyID || _EfficiencyID == null).ToList<dynamic>();
            return lstEfficiency;
        }
        public static List<dynamic> GetAlteredChangedValueOf(string _AlteredChangedValueID = null)
        {
            List<dynamic> lstAlteredChangedValueOf = new List<dynamic>()   
            {
                new { ID="1", Name="Head above Crest of Outlet (H in ft)"},
                new { ID="2", Name="Height of Outlet/Orifice (Y in ft)"},
                new { ID="3", Name="Breadh/Width/Dia (B in ft)"},
                new { ID="4", Name="Working Head (wh in ft)"},
                new { ID="5", Name="Discharge (cusec)"}
            };

            List<dynamic> lstAlteredChangedValue = lstAlteredChangedValueOf.Where(l => l.ID == _AlteredChangedValueID || _AlteredChangedValueID == null).ToList<dynamic>();
            return lstAlteredChangedValue;
        }
        public static List<dynamic> GetBreachLength(string _BreachLengthID = null)
        {
            List<dynamic> lstBreachLength = new List<dynamic>()   
            {
                new { ID="1", Name="<=50"},
                new { ID="2", Name="51-100"},
                new { ID="3", Name="100-200"},
                new { ID="4", Name=">=201"}
            };

            List<dynamic> lstBreachLengthValue = lstBreachLength.Where(l => l.ID == _BreachLengthID || _BreachLengthID == null).ToList<dynamic>();
            return lstBreachLengthValue;
        }
        public static List<dynamic> GetWaterTheftReportedBy(string _ReportedBy = null)
        {
            List<dynamic> lstWaterTheftReportedBy = new List<dynamic>()   
            {
                new { ID="1", Name="PMIU"},
                new { ID="3", Name="Field Staff"}
            };

            List<dynamic> lstReportedByValue = lstWaterTheftReportedBy.Where(l => l.ID == _ReportedBy || _ReportedBy == null).ToList<dynamic>();
            return lstReportedByValue;
        }
        public static List<dynamic> GetWaterTheftAssignedTo(string _AssignedTo = null)
        {
            List<dynamic> lstWaterTheftAssignedTo = new List<dynamic>()   
            {
                new { ID="2", Name="Chief Irrigation"},
                new { ID="3", Name="SE"},
                new { ID="4", Name="XEN"},
                new { ID="5", Name="SDO"},
                new { ID="6", Name="SBE"}
            };
            List<dynamic> lstAssignedToValue = lstWaterTheftAssignedTo.Where(l => l.ID == _AssignedTo || _AssignedTo == null).ToList<dynamic>();
            return lstAssignedToValue;
        }
        public static List<dynamic> GetComplaintAssignedTo(string _AssignedTo = null)
        {
            List<dynamic> lstComplaintAssignedTo = new List<dynamic>()   
            {
                new { ID="1", Name="ADM"},
                new { ID="3", Name="XEN"},
            };

            List<dynamic> lstAssignedToValue = lstComplaintAssignedTo.Where(l => l.ID == _AssignedTo || _AssignedTo == null).ToList<dynamic>();
            return lstAssignedToValue;
        }
        public static List<dynamic> GetComplaintSource(string _ComplaintSourceID = null)
        {
            List<dynamic> lstComplaintSource = new List<dynamic>()   
            {
                new { ID="1", Name="Auto Generator"},
                new { ID="2", Name="Default Helpline"},
                new { ID="3", Name="Chief Minister"},
                new { ID="4", Name="Minister Irrigation"},
                new { ID="5", Name="Chief Sectory"},
                new { ID="6", Name="Sectory Irrigation"}
            };

            List<dynamic> lstComplaintSourceValue = lstComplaintSource.Where(l => l.ID == _ComplaintSourceID || _ComplaintSourceID == null).ToList<dynamic>();
            return lstComplaintSourceValue;
        }




        public static List<dynamic> GetDirectives(string _AssignedTo = null)
        {
            List<dynamic> lstComplaintAssignedTo = new List<dynamic>()   
            {
                new { ID="1", Name="Chief Minister"},
                new { ID="2", Name="Minister Irrigation"},
                new { ID="3", Name="Chief Secretary"},
                new { ID="4", Name="Secretary Irrigation"},
            };

            List<dynamic> lstAssignedToValue = lstComplaintAssignedTo.Where(l => l.ID == _AssignedTo || _AssignedTo == null).ToList<dynamic>();
            return lstAssignedToValue;
        }
        public static List<dynamic> GetRabiSession(string _AssignedTo = null)
        {
            List<dynamic> lstRabi = new List<dynamic>()   
            {
                new { ID="1", Name="Anticipated Water Availability Basin"},//-------SP_AnticipatedWaterAvailabilityIndusRabi.rdl
                new { ID="2", Name="Anticipated Water Availability Indus River"},
                new { ID="16", Name="Anticipated Water Availability Jhelum Chenab"},
                new { ID="3", Name="Distribution Report -Max"}, //SP_DistributionRABI .rdl
                new { ID="4", Name="Distribution Report -Min"},  //SP_DistributionRABI .rdl
                new { ID="5", Name="Inflow Forecast"}, //SP_InflowForecastRabi.rdl
                new { ID="6", Name="Mangla Reservoir Operations"}, //SP_ReservoirOperations.rdl
                new { ID="7", Name="Mangla Dam Rule Curve"}, //SP_DamRuleCurve.rdl
                new { ID="8", Name="Provincial Shares -Max"},//SP_ProvincialSharesRABI .rdl
                new { ID="9", Name="Provincial Shares -Min"},//SP_ProvincialSharesRABI .rdl
                new { ID="10", Name="Provincial Shares Most Likely"},//SP_ProvincialSharesRABI .rdl
                
                new { ID="11", Name="System Operation of JC -Max"},//SP_SystemOperationOfJC.rdl
                new { ID="12", Name="System Operation of JC -Min"},//SP_SystemOperationOfJC.rdl
                new { ID="13", Name="System Operation of JC Most Likely"},//SP_SystemOperationOfJC.rdl
                
                new { ID="14", Name="Tarbela Reservoir Operations"}, //SP_ReservoirOperations.rdl  
                new { ID="15", Name="Tarbela Dam Rule Curve"},//SP_DamRuleCurve.rdl

                new { ID="17", Name="System Operation of Indus -Max"},//SP_SystemOperationOfJC.rdl
                new { ID="18", Name="System Operation of Indus -Min"},//SP_SystemOperationOfJC.rdl
                new { ID="19", Name="System Operation of Indus Most Likely"},//SP_SystemOperationOfJC.rdl
                
                new { ID="20", Name="Probabilistic Flows of Indus at Tarbela"},//SP_ProbabilityFlows.rdl
                new { ID="21", Name="Probabilistic Flows of Indus at Chenab"},//SP_ProbabilityFlows.rdl
                new { ID="22", Name="Probabilistic Flows of Indus at Mangla"},//SP_ProbabilityFlows.rdl
                new { ID="23", Name="Probabilistic Flows of Indus at Kabul"},//SP_ProbabilityFlows.rdl
            };
            return lstRabi;
        }
        public static List<dynamic> GetKharifSession(string _AssignedTo = null)
        {
            List<dynamic> lstKharif = new List<dynamic>()   
            {   
                new { ID="1", Name="Anticipated Water Availability Basin"},
                new { ID="2", Name="Anticipated Water Availability Indus River"},
                new { ID="16", Name="Water Availability Jhelum Chenab"},
                new { ID="3", Name="Distribution Report -Max"}, //SP_DistributionKharif.rdl
                new { ID="4", Name="Distribution Report -Min"}, //SP_DistributionKharif.rdl
                new { ID="5", Name="Inflow Forecast"}, //SP_InflowForecastKharif.rdl
                new { ID="6", Name="Mangla Reservoir Operations"},//SP_ReservoirOperations.rdl
                new { ID="7", Name="Mangla Dam Rule Curve"},//SP_DamRuleCurve.rdl
                new { ID="8", Name="Provincial Shares -Min"}, //SP_ProvincialSharesKHARIF .rdl
                new { ID="9", Name="Provincial Shares -Max"}, //SP_ProvincialSharesKHARIF .rdl
                new { ID="10", Name="Provincial Shares -Most Likely"}, //SP_ProvincialSharesKHARIF .rdl

                new { ID="11", Name="System Operation of JC -Max"},//SP_SystemOperationOfJC.rdl
                new { ID="12", Name="System Operation of JC -Min"},//SP_SystemOperationOfJC.rdl
                new { ID="13", Name="System Operation of JC Most Likely"},//SP_SystemOperationOfJC.rdl
                
                
                new { ID="14", Name="Tarbela Reservoir Operations"}, //SP_ReservoirOperations.rdl
                new { ID="15", Name="Tarbela Dam Rule Curve"},//SP_DamRuleCurve.rdl               

                new { ID="17", Name="System Operation of Indus -Max"},//SP_SystemOperationOfJC.rdl
                new { ID="18", Name="System Operation of Indus -Min"},//SP_SystemOperationOfJC.rdl
                new { ID="19", Name="System Operation of Indus Most Likely"},//SP_SystemOperationOfJC.rdl

                new { ID="20", Name="Probabilistic Flows of Indus at Tarbela"},//SP_ProbabilityFlows.rdl
                new { ID="21", Name="Probabilistic Flows of Indus at Chenab"},//SP_ProbabilityFlows.rdl
                new { ID="22", Name="Probabilistic Flows of Indus at Mangla"},//SP_ProbabilityFlows.rdl
                new { ID="23", Name="Probabilistic Flows of Indus at Kabul"},//SP_ProbabilityFlows.rdl
            };
            return lstKharif;
        }

        public static List<dynamic> GetSessionYear(string _AssignedTo = null)
        {
            List<dynamic> lstYear = new List<dynamic>()   
            {
                new { ID="2011", Name="2011"},
                new { ID="2012", Name="2012"},
                new { ID="2013", Name="2013"},
                new { ID="2014", Name="2014"},
                new { ID="2015", Name="2015"},
            };
            return lstYear;
        }
        public static List<dynamic> GetClosurePeriod(string _AssignedTo = null)
        {
            List<dynamic> lstYear = new List<dynamic>()   
            {
                new { ID="1", Name="2016-2017"},
                new { ID="2", Name="2015-2016"},
                new { ID="3", Name="2014-2015"},
                new { ID="4", Name="2013-2014"},
            };
            return lstYear;
        }
        public static List<dynamic> GetClosureWorkType(string _AssignedTo = null)
        {
            List<dynamic> lstWorkType = new List<dynamic>()   
            {
                new { ID="1", Name="Desilting"},
                new { ID="2", Name="Electrical/Mechanical"},
                new { ID="3", Name="Building Work"},
                new { ID="4", Name="Oiling/Greasing/Repairing"},
                new { ID="5", Name="Outlet Repairing"},
                new { ID="6", Name="Channel Structure Work"},
                new { ID="7", Name="Others"},
            };
            return lstWorkType;
        }
        public static List<dynamic> GetClosureWorkStatus(string _AssignedTo = null)
        {
            List<dynamic> lstWorkType = new List<dynamic>()   
            {
                new { ID="1", Name="Started"},
                new { ID="4", Name="Not Started"},                
                new { ID="2", Name="In progress"},
                new { ID="3", Name="Completed"},
            };
            return lstWorkType;
        }


        #endregion

        public static List<dynamic> GetNoOfGroupsDropDown()
        {
            List<dynamic> lstGroups = new List<dynamic>() 
            {
                new { ID="", Name="Select"},
                new { ID="1", Name="1"},
                new { ID="2", Name="2"},
                new { ID="3", Name="3"},
                new { ID="4", Name="4"},
                new { ID="5", Name="5"}                
            };

            return lstGroups;
        }

        public static List<dynamic> GetNoOfSubGroupsDropDown()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="", Name="Select"},                
                new { ID="2", Name="2"},
                new { ID="3", Name="3"}                
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetTailStatus()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Authorized"},                
                new { ID="2", Name="Dry"},
                new { ID="3", Name="Short"},
                new { ID="4", Name="Excessive"},                
            };

            return lstSubGroups;
        }

        public static List<dynamic> GetStructs()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Channels"},
                new { ID="2", Name="Outlet"},
                new { ID="3", Name="Barrage/Headwork"},
                new { ID="4", Name="Protection Infrastructure"},
                new { ID="5", Name="Drain"},
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetStructsChannels()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Bokhara Disty"},                
                new { ID="2", Name="1-L Lift Disty"},
                new { ID="3", Name="Ahmadpur Disty"},
                new { ID="4", Name="Kabirwala Disty"},
                new { ID="5", Name="Tibbi Disty"},
                new { ID="6", Name="Wan Disty"},
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetStructsOutLets()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="3+145/R"},                
                new { ID="2", Name="33+400/R"},
                new { ID="3", Name="33+860/R"},
                new { ID="4", Name="41+890/R"},
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetStructsHeadWorks()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Baloki Berrage"},                
                new { ID="2", Name="Marala Berrage"},
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetStructsProtectionInfrastructure()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Talala Bund"},                
                new { ID="2", Name="Ahmadpur Bund"},
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetStructsDrain()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Model Town Drain"},                
            };

            return lstSubGroups;
        }

        public static List<dynamic> GetTenderMonitoringDomain()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Irrigation"},
                new { ID="14", Name="D&F"},
                new { ID="5", Name="Small Dams "},
                new { ID="2", Name="Development"},
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetTenderMonitoringDivision()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="DG Khan"},
                new { ID="2", Name="CRBC"},
                new { ID="3", Name="Rajanpur"},
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetTenderMonitoringStatus()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Active"},
                new { ID="2", Name="Expired"},
            };

            return lstSubGroups;
        }
        public static List<dynamic> GetTenderMonitoringNotice()
        {
            List<dynamic> lstSubGroups = new List<dynamic>() 
            {
                new { ID="1", Name="Tender Notice for Bahawalpur Division"},
                new { ID="2", Name="Tender Notice for Rahimyar Khan"},
            };

            return lstSubGroups;
        }

        public static List<dynamic> GetGroups(long _NoOfGroups)
        {
            List<dynamic> lstGroups = new List<dynamic>();
            if (_NoOfGroups == 5)
            {
                lstGroups = new List<dynamic>() 
                {
                    new { ID="1", Name="A"},
                    new { ID="2", Name="B"},
                    new { ID="3", Name="C"},
                    new { ID="4", Name="D"},
                    new { ID="5", Name="E"}
                };
            }
            else if (_NoOfGroups == 4)
            {
                lstGroups = new List<dynamic>() 
                {
                    new { ID="1", Name="A"},
                    new { ID="2", Name="B"},
                    new { ID="3", Name="C"},
                    new { ID="4", Name="D"}                    
                };
            }
            else if (_NoOfGroups == 3)
            {
                lstGroups = new List<dynamic>() 
                {
                    new { ID="1", Name="A"},
                    new { ID="2", Name="B"},
                    new { ID="3", Name="C"}                    
                };
            }
            else if (_NoOfGroups == 2)
            {
                lstGroups = new List<dynamic>() 
                {
                    new { ID="1", Name="A"},
                    new { ID="2", Name="B"}                    
                };
            }
            else if (_NoOfGroups == 1)
            {
                lstGroups = new List<dynamic>() 
                {
                    new { ID="1", Name="A"}                    
                };
            }

            return lstGroups;
        }
        public static List<dynamic> GetPerformanceEvaluationSession(string _ID = null)
        {
            List<dynamic> lstSessions = new List<dynamic>() 
            {
                new { ID="M", Name="Morning"},
                new { ID="E", Name="Evening"},
                new { ID="A", Name="Average"}
            };

            List<dynamic> lstSessionValue = lstSessions.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstSessionValue;
        }
        public static List<dynamic> AttributeType(string _ID = null)
        {
            List<dynamic> lstAttributeType = new List<dynamic>()
            {
                 
                new { ID="1", Name="Text"},
                new { ID="2", Name="Numeric"},
                new { ID="3", Name="Date"},
            };

            List<dynamic> lstlstAttributeTypeValue = lstAttributeType.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstlstAttributeTypeValue;
        }
        public static List<dynamic> GetAssetStatus()
        {
            List<dynamic> lststatus = new List<dynamic>() 
            {
               // new { ID="", Name="Select"},
                new { ID="1", Name="Active"},
                new { ID="2", Name="Inactive"},
                new { ID="3", Name="Auctioned"}
            };

            return lststatus;
        }
        public static List<dynamic> GetInspectionStatus()
        {
            List<dynamic> lststatus = new List<dynamic>() 
            {
               // new { ID="", Name="Select"},
                new { ID="1", Name="Active"},
                new { ID="2", Name="Inactive"}
            };

            return lststatus;
        }
        public static List<dynamic> GetInspectionAllStatus()
        {
            List<dynamic> lststatus = new List<dynamic>() 
            {
               // new { ID="", Name="Select"},
                new { ID="1", Name="Active"},
                new { ID="2", Name="Inactive"},
                new { ID="3", Name="Auctioned"}
            };

            return lststatus;
        }
        public static List<dynamic> GetWorkStatus()
        {
            List<dynamic> lststatus = new List<dynamic>() 
            {
               // new { ID="", Name="Select"},
                new { ID="1", Name="Draft"},
                new { ID="2", Name="Published"},
                new { ID="3", Name="Contract Awarded"}
            };

            return lststatus;
        }
        public static List<dynamic> AccountAssetsType(string _ID = null)
        {
            List<dynamic> lstAssetsType = new List<dynamic>()
            {
                 
                new { ID="1", Name="4 Wheel Vehicle"},
                new { ID="2", Name="2 Wheel Vehicle"},
                new { ID="3", Name="Other"},
  
            };

            List<dynamic> lstlstlstAssetsTypeValue = lstAssetsType.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstlstlstAssetsTypeValue;
        }

        public static List<dynamic> AssetsType(string _ID = null)
        {
            List<dynamic> lstAssetsType = new List<dynamic>()
            {
                 
                new { ID="1", Name="Lot"},
                new { ID="2", Name="Individual Item"},
  
            };

            List<dynamic> lstlstlstAssetsTypeValue = lstAssetsType.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstlstlstAssetsTypeValue;
        }
        public static List<dynamic> AssetsCategory(string _ID = null)
        {
            List<dynamic> lstAssetsCategory = new List<dynamic>()
            {
                 
                new { ID="1", Name="Asset"},
                new { ID="2", Name="Flood"},
                new { ID="3", Name="Infrastructure"},
  
            };

            List<dynamic> lstCategoryValue = lstAssetsCategory.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstCategoryValue;
        }
        public static List<dynamic> AssetInfrastructureTypes(string _ID = null)
        {
            List<dynamic> lstInfrastructureTypes = new List<dynamic>()
            {
                new { ID="0", Name="Select"},
                new { ID="1", Name="Protection Infrastructure"},
                new { ID="2", Name="Barrage/Headwork"},
                new { ID="3", Name="Drain"},
            };

            List<dynamic> lstInfrastructureTypesValue = lstInfrastructureTypes.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstInfrastructureTypesValue;
        }
        public static List<dynamic> AssetWorkInfrastructureTypes(string _ID = null)
        {
            List<dynamic> lstInfrastructureTypes = new List<dynamic>()
            {
                new { ID="6", Name="Channels"},
                new { ID="7", Name="Outlets"},
                new { ID="1", Name="Protection Infrastructure"},
                new { ID="2", Name="Barrage/Headwork"},
                new { ID="3", Name="Drain"},
                new { ID="9", Name="Small Dams"},
                new { ID="10", Name="Small Dams Channels"},
            };

            List<dynamic> lstInfrastructureTypesValue = lstInfrastructureTypes.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstInfrastructureTypesValue;
        }
        public static List<dynamic> StructureTypesForFloodBundRef(string _ID = null)
        {
            List<dynamic> lstInfrastructureTypesFloodBund = new List<dynamic>()
            {
                 
                new { ID="1", Name="Nallah"},
                new { ID="2", Name="Hill Torrent"},
            };

            List<dynamic> lstInfrastructureTypesValue = lstInfrastructureTypesFloodBund.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstInfrastructureTypesValue;
        }
        public static List<dynamic> StructureTypesForFloodBund(string _ID = null)
        {
            List<dynamic> lstStructureTypesForFloodBund = new List<dynamic>()
            {
                new { ID="1", Name="Bund"}, 
                new { ID="2", Name="Nallah"},
                new { ID="3", Name="Hill Torrent"},
            };

            List<dynamic> lstStructureTypesValue = lstStructureTypesForFloodBund.Where(l => l.ID == _ID || _ID == null).ToList<dynamic>();
            return lstStructureTypesValue;
        }
        public static List<dynamic> GetEasternYears()
        {
            List<dynamic> lstYears = new List<dynamic>() 
            {               
                new { ID="5", Name="5"},
                new { ID="10", Name="10"},
                new { ID="15", Name="15"}
            };

            return lstYears;
        }

        #region Accounts
        public static List<dynamic> FundsReleaseType()
        {
            List<dynamic> list = new List<dynamic>()
            {
                new { ID="1", Name="Release"},
                new { ID="2", Name="Cut"},
            };

            return list;
        }

        /// <summary>
        /// This function returns the Vehicle Types
        /// Created On 05-04-2017
        /// </summary>
        /// <param name="_VehicleType"></param>
        /// <returns>List<dynamic></returns>
        public static List<dynamic> GetVehicleType(string _VehicleType = null)
        {
            List<dynamic> lstVehicleTypes = new List<dynamic>() 
            {
                new { ID="4 Wheel Vehicle", Name="4 Wheel Vehicle"},
                new { ID="2 Wheel Vehicle", Name="2 Wheel Vehicle"},
                new { ID="Other", Name="Other"}
            };

            lstVehicleTypes = lstVehicleTypes.Where(vt => vt.ID == _VehicleType || _VehicleType == null).ToList<dynamic>();

            return lstVehicleTypes;
        }

        public static List<dynamic> GetAssetAllocationAttributeType()
        {
            List<dynamic> lstAttributesTypes = new List<dynamic>() 
            {
                new { ID= 1, Name="Quantity"}
                
            };

            return lstAttributesTypes;
        }

        public static List<dynamic> GetMonthList()
        {
            List<dynamic> MonthList = new List<dynamic>()
            {
                new {ID="January",Name="January"},
                new {ID="February",Name="February"},
                new {ID="March",Name="March"},
                new {ID="April",Name="April"},
                new {ID="May",Name="May"},
                new {ID="June",Name="June"},
                new {ID="July",Name="July"},
                new {ID="August",Name="August"},
                new {ID="September",Name="September"},
                new {ID="October",Name="October"},
                new {ID="November",Name="November"},
                new {ID="December",Name="December"}
            };

            return MonthList;

        }
        #endregion

        #region SmallDam
        public static List<dynamic> GetOffTakingSidesSD()
        {
            List<dynamic> list = new List<dynamic>()
            {
                new { ID="1", Name="Left"},
                new { ID="2", Name="Right"},
            };

            return list;
        }
        public static List<dynamic> GetParentTypeSD()
        {
            List<dynamic> list = new List<dynamic>()
            {
                new { ID="1", Name="Dam"},
                new { ID="2", Name="Channel"},
            };
            return list;
        }
        public static List<dynamic> GetGrossStorageCapacitySD()
        {
            List<dynamic> list = new List<dynamic>()
            {
                
                new { ID="1", Name="Excellent"},
                new { ID="2", Name="Good"},
                new { ID="3", Name="Satisfactory"},
                new { ID="4", Name="Poor"},
            };
            return list;
        }
        #endregion

        public static List<dynamic> GetCommandType()
        {
            List<dynamic> list = new List<dynamic>()
            {
                new { ID="1", Name="Indus"},
                new { ID="2", Name="Jhelum Chenab"},
            };

            return list;
        }
        public static List<dynamic> GetReports()
        {
            List<dynamic> list = new List<dynamic>()
            {
                new { ID="1", Name="Planned Vs Actual Deliveries"},
                new { ID="2", Name="Tentative Distribution Program"},
                new { ID="3", Name="Tentative Entitlement Program"},
                new { ID="4", Name="Punjab Canal Withdrawals"},
                new { ID="5", Name="Tentative Punjab Canals Withdrawals"}
            };

            return list;
        }

        public static List<dynamic> GetMonthListAbr()
        {
            List<dynamic> MonthList = new List<dynamic>()
            {
                new {ID="Jan",Name="January"},
                new {ID="Feb",Name="February"},
                new {ID="Mar",Name="March"},
                new {ID="Apr",Name="April"},
                new {ID="May",Name="May"},
                new {ID="Jun",Name="June"},
                new {ID="Jul",Name="July"},
                new {ID="Aug",Name="August"},
                new {ID="Sep",Name="September"},
                new {ID="Oct",Name="October"},
                new {ID="Nov",Name="November"},
                new {ID="Dec",Name="December"}
            };

            return MonthList;

        }

        public static List<dynamic> GetTenDaily()
        {
            List<dynamic> MonthList = new List<dynamic>()
            {
                new {ID="1",Name="1st TenDaily"},
                new {ID="2",Name="2nd TenDaily"},
                new {ID="3",Name="3rd TenDaily"}
                
            };

            return MonthList;

        }
        public static List<dynamic> GetActivity()
        {
            List<dynamic> lstActivity = new List<dynamic>() 
            {
               // new { ID="1", Name="All"},
                new { ID="2", Name="Meter"},
                new { ID="3", Name="Fuel"},
                new { ID="4", Name="Channel Observation"},
                new { ID="5", Name="Outlet Checking"},
                new { ID="6", Name="Water Theft"},
                new { ID="7", Name="Cut & Breach"},
                new { ID="8", Name="Rotational Violation"},
                new { ID="9", Name="Leaves"}
            };
            return lstActivity;
        }
        public static List<dynamic> GetActivityBy()
        {
            List<dynamic> lstActivityBy = new List<dynamic>()   
            {
                new { ID="12", Name="ADM"},
                new { ID="13", Name="MA"}
            };
            return lstActivityBy;
        }
        public static List<dynamic> GetOutletConditionType()
        {
            List<dynamic> lstOutletChecking = new List<dynamic>()   
            {
                new { ID="2", Name="OK"},
                new { ID="1", Name="Water Theft"}
                
            };
            return lstOutletChecking;
        }

        public static List<dynamic> GetScenarios()
        {
            List<dynamic> lstScenarios = new List<dynamic>()   
            {
                new { ID="1", Name="Likely"},
                new { ID="2", Name="Maximum"},
                new { ID="2", Name="Minimum"}
                
            };
            return lstScenarios;
        }




    }
}