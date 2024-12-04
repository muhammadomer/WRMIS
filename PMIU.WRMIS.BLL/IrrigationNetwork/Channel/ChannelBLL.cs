using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.Channel;
using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.Channel;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PMIU.WRMIS.BLL.IrrigationNetwork.Channel
{
    public class ChannelBLL : BaseBLL
    {
        #region "Common Functions"
        public dynamic GetChannelTotalRD(long _ChannelID)
        {
            dynamic qChannelRD = (from c in db.Repository<CO_Channel>().GetAll()
                                  where c.ID == _ChannelID && c.IsActive == true
                                  select new
                                  {
                                      ChannelTotalRDs = c.TotalRDs

                                  }).SingleOrDefault();
            return qChannelRD;
        }
        public Tuple<int, int> GetIrrigationBoundariesRDs(long _ChannelID)
        {
            var qSectionRds = (from i in db.Repository<CO_ChannelIrrigationBoundaries>().GetAll()
                               where i.ChannelID == _ChannelID && i.IsActive == true
                               group i by 0 into g
                               select new
                               {
                                   Min = g.Min(p => p.SectionRD),
                                   Max = g.Max(p => p.SectionRD)

                               }).Single();

            var qChannelTotalRD = GetChannelTotalRD(_ChannelID).ChannelTotalRDs;

            Tuple<int, int> tupleRDs = Tuple.Create(qSectionRds.Min, qChannelTotalRD);

            return tupleRDs;
        }

        /// <summary>
        /// This method return Channel details
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns>CO_Channel</returns>
        public CO_Channel GetChannelByID(long _ID)
        {
            CO_Channel qChannel = db.Repository<CO_Channel>().GetAll().Where(s => s.ID == _ID && s.IsActive.Value == true).FirstOrDefault();
            return qChannel;
        }
        public List<CO_Channel> GetChannels()
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannels();
        }
        public List<CO_Channel> GetChannelsByIrrigationBoundary(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelsByIrrigationBoundary(_ZoneID, _CircleID, _DivisionID, _SubDivisionID);
        }
        #endregion

        #region "Channel Addition"
        public bool IsChannelExists(CO_Channel _Channel)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsChannelExists(_Channel);
        }

        public bool IsChannelIMISExists(CO_Channel _Channel)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsChannelIMISExists(_Channel);
        }

        /// <summary>
        /// This function retun channel addition success along with message
        /// Created on 22-10-2015
        /// </summary>
        /// <param name="_Channel"></param>
        /// <returns></returns>
        public bool SaveChannel(CO_Channel _Channel)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.SaveChannel(_Channel);
        }

        #endregion

        #region "Channel Search"
        /// <summary>
        /// This function return Channel Command Types
        /// Created on: 16-10-2015
        /// </summary>
        /// <returns>List<CO_ChannelComndType></returns>
        public List<CO_ChannelComndType> GetChannelCommandTypes(bool? _IsActive = null)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelCommandTypes(_IsActive);
        }

        /// <summary>
        /// This function return channel flow types
        /// Created on: 16-10-2015
        /// </summary>
        /// <returns>List<CO_ChannelFlowType></returns>
        public List<CO_ChannelFlowType> GetChannelFlowTypes(bool? _IsActive = null)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelFlowTypes(_IsActive);
        }

        /// <summary>
        /// This function return Channel types
        /// Created on: 16-10-2015
        /// </summary>
        /// <returns>List<CO_ChannelType></returns>
        public List<CO_ChannelType> GetChannelTypes(bool? _IsActive = null)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelTypes(_IsActive);
        }

        /// <summary>
        /// This function return parent channels
        /// Created on: 27-10-2015
        /// </summary>
        /// <returns>List<object></returns>
        public List<object> GetParentChannels()
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetParentChannels();
        }
        public List<object> GetChannelsBySearchCriteria(long _ChannelID, long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _CommandNameID, long _ChannelTypeID, long _FlowTypeID, string _ChannelName, string _IMISCode, long _ParentChannelID, long _StructureTypeID, List<long> _lstUserZone, List<long> _lstUserCircles, List<long> _lstUserDivisions, List<long> _lstUserSubDiv)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelsBySearchCriteria(_ChannelID, _ZoneID, _CircleID, _DivisionID, _SubDivisionID, _CommandNameID, _ChannelTypeID, _FlowTypeID, _ChannelName, _IMISCode, _ParentChannelID, _StructureTypeID, _lstUserZone, _lstUserCircles, _lstUserDivisions, _lstUserSubDiv);
        }
        /// <summary>
        /// This function check Channel Irrigation Boundaries existance
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>bool</returns>
        public bool IsChannelIrrigationBoundariesExists(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsChannelIrrigationBoundariesExists(_ChannelID);
        }
        public bool IsChannelDependencyExists(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsChannelDependencyExists(_ChannelID);
        }
        /// <summary>
        /// This function delete Channel
        /// Created on 10-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public bool DeleteChannel(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.DeleteChannel(_ChannelID);
        }

        /// <summary>
        /// This function return Channel Search used for Public Website
        /// Created On: 15/11/2016
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetChannelInformation_SearchForPublicWebSite(long? _DivisionID, string _ChannelName)
        {
            return new ChannelDAL().GetChannelInformation_SearchForPublicWebSite(_DivisionID, _ChannelName);
        }

        /// <summary>
        /// This function return Division Lists for Public Website        
        /// Created On: 16/11/2016
        /// </summary>
        /// <returns>DataSet</returns>
        public IEnumerable<DataRow> GetDivisions_ListForPublicWebSite()
        {
            return new ChannelDAL().GetDivisions_ListForPublicWebSite();
        }

        /// <summary>
        /// This function return Channel Detail used for Public Website
        /// Created On: 16/11/2016
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetChannelDetailInformation_ForPublicWebSite(long _ChannelID, DateTime? _ReadingDateTime)
        {
            return new ChannelDAL().GetChannelDetailInformation_ForPublicWebSite(_ChannelID, _ReadingDateTime);
        }

        #endregion

        #region "Channel Physical Location"
        /// <summary>
        /// This function return Channel Irrigation Boundaries
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetIrrigationBoundariesByChannelID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetIrrigationBoundariesByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function check Section existance in Irrigation Boundaries
        /// Created on 09-11-2015
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public bool IsSectionExists(CO_ChannelIrrigationBoundaries _IrrigationBoundary)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsSectionExists(_IrrigationBoundary);
        }
        /// <summary>
        /// This function check SectionRD existance in Irrigation Boundaries
        /// Created on 09-10-2015
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public bool IsSectionRDsExists(CO_ChannelIrrigationBoundaries _IrrigationBoundary)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsSectionRDsExists(_IrrigationBoundary);
        }
        public bool SaveIrrigationBoundaries(CO_ChannelIrrigationBoundaries _IrrigationBoundaries)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.SaveIrrigationBoundaries(_IrrigationBoundaries);
        }
        public bool DeleteIrrigationBoundaries(long _IrrigationBoundariesID, long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.DeleteIrrigationBoundaries(_IrrigationBoundariesID, _ChannelID);
        }
        /// <summary>
        /// This function returns Channel Administrative Boundaries
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByChannelID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetAdministrativeBoundariesByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function returns Channel Divisions
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByChannelID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetDivisionsByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function returns Channel Districts
        /// Created on: 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByChannelID(long _ChannelID, bool? _IsActive = null)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetDistrictsByChannelID(_ChannelID, _IsActive);

        }
        /// <summary>
        /// This function check Village existance in Admin Boundaries
        /// Created on 10-11-2015
        /// </summary>
        /// <param name="_AdminBoundary"></param>
        /// <returns>bool</returns>
        public bool IsVillageExists(CO_ChannelAdminBoundries _AdminBoundary)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsVillageExists(_AdminBoundary);
        }
        public bool SaveAdministrativeBoundaries(CO_ChannelAdminBoundries _AdministrativeBoundaries)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.SaveAdministrativeBoundaries(_AdministrativeBoundaries);
        }
        public bool DeleteAdministrativeBoundaries(long _AdministrativeBoundaries)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.DeleteAdministrativeBoundaries(_AdministrativeBoundaries);
        }
        public CO_ChannelIrrigationBoundaries GetIrrigationBoundaryByID(long _IrrigationBoundariesID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetIrrigationBoundaryByID(_IrrigationBoundariesID);
        }
        public bool IsIrrigationBoundariesDependencyExists(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsIrrigationBoundariesDependencyExists(_ChannelID);
        }
        /// <summary>
        /// This function update the IsCalcualted bit in Channel table to calculate Gauges
        /// Created on: 03-02-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_IsCalculated"></param>
        /// <returns>bool</returns>
        public bool UpdateIsCalculated(long _ChannelID, bool _IsCalculated = false)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.UpdateIsCalculated(_ChannelID, _IsCalculated);
        }
        #endregion

        #region "Gauge Information"

        /// <summary>
        /// This function returns Gauge Categories.
        /// Created On 13-11-2015.
        /// </summary> 
        /// <returns>CO_GaugeCategory<CO_Zone>()</returns>
        public List<CO_GaugeCategory> GetGaugeCategories(bool? _IsActive = null)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetGaugeCategories(_IsActive);
        }

        /// <summary>
        /// This function returns Channel Sub Divisions in Irrigational Boundaries.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<CO_SubDivision>()</returns>
        public List<CO_SubDivision> GetSubDivisionsByChannelID(long _ChannelID, bool? _IsActive = null)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetSubDivisionsByChannelID(_ChannelID, _IsActive);
        }

        /// <summary>
        /// This function returns Gauge types.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<CO_GaugeType>()</returns>
        public List<CO_GaugeType> GetGaugeTypes()
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetGaugeTypes();
        }

        /// <summary>
        /// This function returns Sub Division Sections in Irrigational Boundaries.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<CO_Section>()</returns>
        public List<CO_Section> GetSectionsBySubDivisionChannelID(long _SubDivisionID, long _ChannelID, bool? _IsActive = null)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetSectionsBySubDivisionChannelID(_SubDivisionID, _ChannelID, _IsActive);
        }

        /// <summary>
        /// This function returns Channel Gauge Informations.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<object>()</returns>
        public List<object> GetGaugeInformationsByChannelID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetGaugeInformationsByChannelID(_ChannelID);
        }

        public object GetDivisionNameBySubDivisonID(long _SubDivisionID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetDivisionNameBySubDivisonID(_SubDivisionID);
        }

        public object GetDivisionNameByIrrigationDomain()
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetDivisionNameByIrrigationDomain();
        }
        public object GetDivisionNameByChannel_ID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetDivisionNameByChannel_ID(_ChannelID);
        }

        public List<dynamic> GetSubDivisionNameByChannelID(long DivID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetSubDivisionNameByChannelID(DivID);
        }

        public List<dynamic> GetSectionNameByChannelID(long SubDivID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetSectionNameByChannelID(SubDivID);
        }

        //public List<dynamic> GetDivisionNameByID(long _DivisionID)
        //{
        //    ChannelDAL dalChannel = new ChannelDAL();
        //    return dalChannel.GetDivisionNameByID(_DivisionID);

        //}

        //public List<dynamic> GetSubDivisionNameByID(long _SubDivisionID)
        //{
        //    ChannelDAL dalChannel = new ChannelDAL();
        //    return dalChannel.GetSubDivisionNameByID(_SubDivisionID);
        //}

        //public List<dynamic> GetSectionNameByID(long _SectionID)
        //{
        //    ChannelDAL dalChannel = new ChannelDAL();
        //    return dalChannel.GetSectionNameByID(_SectionID);
        //}


        public bool SaveGaugeInformation(CO_ChannelGauge _GaugeInformation)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.SaveGaugeInformation(_GaugeInformation);
        }
        /// <summary>
        /// This function check Gauge Information dependency by Gauge Information ID 
        /// Created on: 11-11-2015
        /// </summary>
        /// <param name="_GaugeInformationID"></param>
        /// <returns>bool</returns>
        public bool IsGaugeInformationDependencyExists(long _GaugeInformationID)
        {
            return true;
        }
        /// <summary>
        /// This function delete Gauge Information
        /// Created on: 11-11-2015
        /// </summary>
        /// <param name="_GaugeInformationID"></param>
        /// <returns>bool</returns>
        public bool DeleteGaugeInformation(long _GaugeInformationID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.DeleteGaugeInformation(_GaugeInformationID);
        }
        /// <summary>
        /// This function check Gauge RD existance
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_GaugeInoformation"></param>
        /// <returns>bool</returns>
        public bool IsGaugeRDExists(CO_ChannelGauge _GaugeInoformation)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsGaugeRDExists(_GaugeInoformation);
        }
        public bool DeleteAutoDeterminedGauges(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.DeleteAutoDeterminedGauges(_ChannelID);
        }
        public bool AutoDetermineGaguesFromIrrigationBoundaries(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.AutoDetermineGaguesFromIrrigationBoundaries(_ChannelID);
        }
        /// <summary>
        /// This function check Gauge Information exists in BedLevelDT Parameters and CrestLevelDTParameters
        /// Created on: 30-11-2015
        /// </summary>
        /// <param name="_GaugeID"></param>
        /// <returns>bool</returns>
        public bool IsGaugeDependanceExists(long _GaugeID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsGaugeDependanceExists(_GaugeID);
        }
        public bool IsGaugeInformationExists(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsGaugeInformationExists(_ChannelID);
        }
        /// <summary>
        ///  This function return Section Irrigation Boundary
        ///  Created on: 10-02-2016
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>CO_ChannelIrrigationBoundaries</returns>
        public CO_ChannelIrrigationBoundaries GetIrrigationBoundaryBySection(long _ChannelID, long _SectionID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetIrrigationBoundaryBySection(_ChannelID, _SectionID);
        }
        public bool UpdateChannelGaugeByChannelID(long _ID, long? _GaugeDivID, long? _GaugeSubDivID, long? _GaugeSectionID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.UpdateChannelGaugeByChannelID(_ID, _GaugeDivID, _GaugeSubDivID, _GaugeSectionID);
        }

        public CO_ChannelGauge GetChannelGaugeByChannelID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelGaugeByChannelID(_ChannelID);
        }

        public long? GetCircleIDByDivID(long _DivID)
        {
            ChannelDAL dalchannel = new ChannelDAL();
            return dalchannel.GetCircleIDByDivID(_DivID);
        }

        public long? GetDivisionIDBySubDivID(long _SubDivID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetDivisionIDBySubDivID(_SubDivID);
        }

        public long? GetSubDivIDBySectionID(long _SectionID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetSubDivIDBySectionID(_SectionID);
        }

        #endregion

        #region "Parent Channels and Channel Feeders"
        ///// <summary>
        ///// This function return Channel Sides
        ///// Created on: 10-11-2015
        ///// </summary>
        ///// <param name="_SideID"></param>
        ///// <returns>List<Dynamic></returns>
        //public List<dynamic> GetChannelSides(string _SideID = null)
        //{
        //    List<dynamic> lstChannelSides = new List<dynamic>()
        //    {
        //        new {ID="L", Name="Left"},
        //        new {ID="R", Name="Right"}
        //    };

        //    List<dynamic> lstSides = lstChannelSides.Where(s => s.ID == _SideID || _SideID == null).ToList<dynamic>();

        //    return lstSides;
        //}
        /// <summary>
        /// This function returns Channel relationship types
        /// Created on: 10-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<dynamic></returns>
        //public List<dynamic> GetRelationshipType(string _RelationshipID = null)
        //{
        //    List<dynamic> lstRelationshipTypes = new List<dynamic>()
        //    {
        //        new {ID="P", Name="Parent"},
        //        new {ID="F",Name = "Feeder"}
        //    };

        //    List<dynamic> lstRelationships = lstRelationshipTypes.Where(t => t.ID == _RelationshipID || _RelationshipID == null).ToList<dynamic>();

        //    return lstRelationships;
        //}
        /// <summary>
        /// This function returns Channel parent feeders
        /// Created on: 10-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<dynamic></returns>
        public List<dynamic> GetChannelParentFeedersByChannelID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelParentFeedersByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function returns Channel Structures
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public List<dynamic> GetChannelStructures(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelStructures(_ChannelID);
        }
        /// <summary>
        /// This function returns RD at Channel
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public double GetRDAtChannel(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetRDAtChannel(_ChannelID);
        }

        public List<dynamic> GetChannelParentFeedersNameByChannelID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetChannelParentFeedersNameByChannelID(_ChannelID);

        }

        public List<dynamic> BindIMISCode(long _ParentFeederID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.BindIMISCode(_ParentFeederID);
        }

        public string GetStructureTypeID(long _ParentFeederID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.GetStructureTypeID(_ParentFeederID);
        }

        public bool UpdateIMISCodeChannelID(long _ID, string _IMISCode)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.UpdateIMISCodeChannelID(_ID, _IMISCode);
        }

        public bool IsIMISCodeExists(long _ChannelID, string IMIS)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsIMISCodeExists(_ChannelID, IMIS);
        }

        public bool SaveChannelParentFeeder(CO_ChannelParentFeeder _ParentFeeder)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.SaveChannelParentFeeder(_ParentFeeder);
        }
        /// <summary>
        /// This function check Channel Parent Feeder existance
        /// Created on: 10-11-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <param name="_ChannelParentFeederID"></param>
        /// <param name="_StructureTypeID"></param>
        /// <returns>bool</returns>
        public bool IsChannelParentFeederExists(long _ID, long _ChannelParentFeederID, long _StructureTypeID, long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsChannelParentFeederExists(_ID, _ChannelParentFeederID, _StructureTypeID, _ChannelID);
        }
        /// <summary>
        /// This function delete Channel Parent Feeders
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public bool DeleteChannelParentFeeder(long _ID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.DeleteChannelParentFeeder(_ID);
        }
        /// <summary>
        /// This function check Channel side and Rd at Channel exists
        /// Created on: 08-02-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_RDAtChannel"></param>
        /// <returns>bool</returns>
        public bool IsChannelSideAndRDExists(string _ChannelSide, double _RDAtChannel)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsChannelSideAndRDExists(_ChannelSide, _RDAtChannel);
        }
        /// <summary>
        /// This function check Channel side and R.D (ft) at Parent or Feeder Channel exists
        /// Created on: 08-02-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_ParrentFeederRDS"></param>
        /// <returns>bool</returns>
        public bool IsChannelSideAndParrentFeederRDSExists(string _ChannelSide, double _ParrentFeederRDS)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsChannelSideAndParrentFeederRDSExists(_ChannelSide, _ParrentFeederRDS);
        }
        #endregion

        public bool IsChannelParentFeederExistsForIMIS(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();
            return dalChannel.IsChannelParentFeederExistsForIMIS(_ChannelID);
        }

        #region GetDataTableParameters

        /// <summary>
        /// This function gets Channel Gauge by ID.
        /// Created On 13-11-2015.
        /// </summary>
        /// <param name="_ChannelGaugeID"></param>
        /// <returns>CO_ChannelGauge</returns>
        public CO_ChannelGauge GetChannelGaugeByID(long _ChannelGaugeID)
        {
            ChannelDAL dalChannel = new ChannelDAL();

            return dalChannel.GetChannelGaugeByID(_ChannelGaugeID);
        }
        public List<object> GetChannelGaugeRDByChannelID(long _ChannelID)
        {
            ChannelDAL dalChannel = new ChannelDAL();

            return dalChannel.GetChannelGaugeRDByChannelID(_ChannelID);
        }


        /// <summary>
        /// This function adds new Bed Level Datetable Parameter.
        /// Created On 13-11-2015.
        /// </summary>
        /// <param name="_ChannelGaugeDTPGatedStructure"></param>
        /// <returns>bool</returns>
        public bool AddBedLevelDTParameters(CO_ChannelGaugeDTPGatedStructure _ChannelGaugeDTPGatedStructure)
        {
            ChannelDAL dalChannel = new ChannelDAL();

            return dalChannel.AddBedLevelDTParameters(_ChannelGaugeDTPGatedStructure);
        }

        /// <summary>
        /// This function gets the Bed Level Datetable history.
        /// Created On 16-11-2015.
        /// </summary>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <param name="_GuageId"></param>
        /// <returns>List<CO_ChannelGaugeDTPGatedStructure></returns>
        public List<CO_ChannelGaugeDTPGatedStructure> GetBedLevelParameterHistory(DateTime? _FromDate, DateTime? _ToDate, long _GuageID)
        {
            ChannelDAL dalChannel = new ChannelDAL();

            return dalChannel.GetBedLevelParameterHistory(_FromDate, _ToDate, _GuageID);
        }

        /// <summary>
        /// This function adds new Crest Level Datetable Parameter.
        /// Created On 19-11-2015.
        /// </summary>
        /// <param name="_ChannelGaugeDTPFall"></param>
        /// <returns>bool</returns>
        public bool AddCrestLevelDTParameters(CO_ChannelGaugeDTPFall _ChannelGaugeDTPFall)
        {
            ChannelDAL dalChannel = new ChannelDAL();

            return dalChannel.AddCrestLevelDTParameters(_ChannelGaugeDTPFall);
        }

        /// <summary>
        /// This function gets the Crest Level Datetable history.
        /// Created On 19-11-2015.
        /// </summary>
        /// <param name="_FromDate"></param>
        /// <param name="_ToDate"></param>
        /// <param name="_GuageId"></param>
        /// <returns>List<CO_ChannelGaugeDTPFall></returns>
        public List<CO_ChannelGaugeDTPFall> GetCrestLevelParameterHistory(DateTime? _FromDate, DateTime? _ToDate, long _GuageID)
        {
            ChannelDAL dalChannel = new ChannelDAL();

            return dalChannel.GetCrestLevelParameterHistory(_FromDate, _ToDate, _GuageID);
        }



        #endregion



        public DataSet GetChannelLineDiagram(string _Division, string _Channel, DateTime? _FromDate)
        {
            return new ChannelDAL().GetChannelLineDiagram(_Division, _Channel, _FromDate);
        }

        public List<object> GetMainBranchLinkChannels(long _DivID)
        {
            return new ChannelDAL().GetMainBranchLinkChannels(_DivID);
        }
    }
}
