using PMIU.WRMIS.AppBlocks;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.Channel;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.Channel
{
    public class ChannelDAL
    {
        ContextDB db = new ContextDB();

        #region "Common Functions"
        public dynamic GetChannelTotalRD(long _ChannelID)
        {
            dynamic qChannelRD = (from c in db.Repository<CO_Channel>().GetAll()
                                  where c.ID == _ChannelID
                                  select new
                                  {
                                      ChannelTotalRDs = c.TotalRDs

                                  }).SingleOrDefault();
            return qChannelRD;
        }
        public Tuple<double, double> GetIrrigationBoundariesRDs(long _ChannelID)
        {
            var qSectionRds = (from i in db.Repository<CO_ChannelIrrigationBoundaries>().GetAll()
                               where i.ChannelID == _ChannelID
                               group i by 0 into g
                               select new
                               {
                                   Min = g.Min(p => p.SectionRD),
                                   Max = g.Max(p => p.SectionRD)

                               }).Single();

            var qChannelTotalRD = GetChannelTotalRD(_ChannelID).ChannelTotalRDs;

            Tuple<double, double> tupleRDs = Tuple.Create(qSectionRds.Min, qChannelTotalRD);

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

        #endregion

        #region "Channel Addition"
        public bool IsChannelExists(CO_Channel _Channel)
        {
            bool qIsChannelExists = db.ExtRepositoryFor<ChannelRepository>().IsChannelExists(_Channel);
            return qIsChannelExists;
        }

        public bool IsChannelIMISExists(CO_Channel _Channel)
        {
            bool qIsChannelExists = db.ExtRepositoryFor<ChannelRepository>().IsChannelIMISExists(_Channel);
            return qIsChannelExists;
        }

        /// <summary>
        /// This function retun channel addition success along with message
        /// Created on 22-10-2015
        /// </summary>
        /// <param name="objChannel"></param>
        /// <returns></returns>
        public bool SaveChannel(CO_Channel _Channel)
        {
            bool isSaved = false;

            if (_Channel.ID == 0)
            {
                long _ChannelID = db.Repository<CO_Channel>().GetAll().Max(u => (long)u.ID);
                _Channel.ID = _ChannelID + 1;
                db.Repository<CO_Channel>().Insert(_Channel);
                db.Save();
                isSaved = true;
            }
            else
            {
                CO_Channel objChannel = this.GetChannelByID(_Channel.ID);
                objChannel.ID = _Channel.ID;
                objChannel.NAME = _Channel.NAME;
                objChannel.ChannelTypeID = _Channel.ChannelTypeID;
                objChannel.TotalRDs = _Channel.TotalRDs;
                objChannel.FlowTypeID = _Channel.FlowTypeID;
                objChannel.ComndTypeID = _Channel.ComndTypeID;
                objChannel.AuthorizedTailGauge = _Channel.AuthorizedTailGauge;
                objChannel.GCAAcre = _Channel.GCAAcre;
                objChannel.CCAAcre = _Channel.CCAAcre;
                objChannel.ChannelAbbreviation = _Channel.ChannelAbbreviation;
                objChannel.IsCalculated = _Channel.IsCalculated;
                objChannel.Remarks = _Channel.Remarks;
                objChannel.IMISCode = _Channel.IMISCode;

                db.Repository<CO_Channel>().Update(objChannel);
                db.Save();
                isSaved = true;
            }

            return isSaved;
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
            List<CO_ChannelComndType> lstChannelCommandTypes = db.Repository<CO_ChannelComndType>().GetAll().Where(c => (c.IsActive == _IsActive || _IsActive == null)).ToList<CO_ChannelComndType>();
            return lstChannelCommandTypes;

        }
        public List<CO_Channel> GetChannels()
        {
            List<CO_Channel> lstChannels = db.Repository<CO_Channel>().GetAll().Where(c => c.IsActive.Value == true).ToList<CO_Channel>();
            return lstChannels;

        }
        public List<CO_Channel> GetChannelsByIrrigationBoundary(long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID)
        {
            ChannelRepository repChannelRepository = this.db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetChannelsByIrrigationBoundary(_ZoneID
                , _CircleID
                , _DivisionID
                , _SubDivisionID
               );
        }
        /// <summary>
        /// This function return channel flow types
        /// Created on: 16-10-2015
        /// </summary>
        /// <returns>List<CO_ChannelFlowType></returns>
        public List<CO_ChannelFlowType> GetChannelFlowTypes(bool? _IsActive = null)
        {
            List<CO_ChannelFlowType> lstChannelFlowTypes = db.Repository<CO_ChannelFlowType>().GetAll().Where(c => (c.IsActive == _IsActive || _IsActive == null)).ToList<CO_ChannelFlowType>();
            return lstChannelFlowTypes;

        }

        /// <summary>
        /// This function return Channel types
        /// Created on: 16-10-2015
        /// </summary>
        /// <returns>List<CO_ChannelType></returns>
        public List<CO_ChannelType> GetChannelTypes(bool? _IsActive = null)
        {
            List<CO_ChannelType> lstChannelTypes = db.Repository<CO_ChannelType>().GetAll().Where(c => (c.IsActive == _IsActive || _IsActive == null)).ToList<CO_ChannelType>();
            return lstChannelTypes;
        }

        /// <summary>
        /// This function return parent channels
        /// Created on: 27-10-2015
        /// </summary>
        /// <returns>List<object></returns>
        public List<object> GetParentChannels()
        {
            ChannelRepository repChannelRepository = this.db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetParentChannels();
        }
        public List<object> GetChannelsBySearchCriteria(long _ChannelID, long _ZoneID, long _CircleID, long _DivisionID, long _SubDivisionID, long _CommandNameID, long _ChannelTypeID, long _FlowTypeID, string _ChannelName, string _IMISCode, long _ParentChannelID, long _StructureTypeID, List<long> _lstUserZone, List<long> _lstUserCircles, List<long> _lstUserDivisions, List<long> _lstUserSubDiv)
        {
            ChannelRepository repChannelRepository = this.db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetChannelsBySearchCriteria(_ChannelID, _ZoneID, _CircleID, _DivisionID, _SubDivisionID, _CommandNameID, _ChannelTypeID, _FlowTypeID, _ChannelName, _IMISCode, _ParentChannelID, _StructureTypeID, _lstUserZone, _lstUserCircles, _lstUserDivisions, _lstUserSubDiv);
        }
        /// <summary>
        /// This function check Channel Irrigation Boundaries existance
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>bool</returns>
        public bool IsChannelIrrigationBoundariesExists(long _ChannelID)
        {
            bool qIrrigationBoundaries = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Any(i => i.ChannelID == _ChannelID);
            return qIrrigationBoundaries;
        }
        public bool IsChannelDependencyExists(long _ChannelID)
        {
            bool qIsChannelExists = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Any(i => i.ChannelID == _ChannelID);
            return qIsChannelExists;
        }
        /// <summary>
        /// This function delete Channel
        /// Created on 10-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public bool DeleteChannel(long _ChannelID)
        {
            db.Repository<CO_Channel>().Delete(_ChannelID);
            db.Save();
            return true;
        }

        /// <summary>
        /// This function return Channel Search used for Public Website
        /// Created On: 15/11/2016
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetChannelInformation_SearchForPublicWebSite(long? _DivisionID, string _ChannelName)
        {
            return db.ExecuteStoredProcedureDataSet("CO_Channel_SearchForPublicWebSite", _DivisionID, _ChannelName);
        }

        /// <summary>
        /// This function return Division Lists for Public Website        
        /// Created On: 16/11/2016
        /// </summary>
        /// <returns>DataSet</returns>
        public IEnumerable<DataRow> GetDivisions_ListForPublicWebSite()
        {
            return db.ExecuteDataSet("CO_Divisions_GetListForPublicWebSite");
        }

        /// <summary>
        /// This function return Channel Search used for Public Website
        /// Created On: 16/11/2016
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetChannelDetailInformation_ForPublicWebSite(long _ChannelID, DateTime? _ReadingDateTime)
        {
            return db.ExecuteStoredProcedureDataSet("CO_Channel_DetailByIDForPublicWebSite", _ChannelID, _ReadingDateTime);
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
            ChannelRepository repChannelRepository = this.db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetIrrigationBoundariesByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function check Section existance in Irrigation Boundaries
        /// Created on 09-11-2015
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public bool IsSectionExists(CO_ChannelIrrigationBoundaries _IrrigationBoundary)
        {
            bool qIsSectionExists = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Any(i => i.SectionID == _IrrigationBoundary.SectionID
                         && i.ChannelID == _IrrigationBoundary.ChannelID.Value
                         && (i.ID != _IrrigationBoundary.ID || _IrrigationBoundary.ID == 0));
            return qIsSectionExists;
        }
        /// <summary>
        /// This function check SectionRD existance in Irrigation Boundaries
        /// Created on 09-10-2015
        /// </summary>
        /// <param name="_IrrigationBoundary"></param>
        /// <returns>bool</returns>
        public bool IsSectionRDsExists(CO_ChannelIrrigationBoundaries _IrrigationBoundary)
        {
            bool qIsSectionRDsExists = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Any(i => i.SectionRD == _IrrigationBoundary.SectionRD
                        && i.ChannelID == _IrrigationBoundary.ChannelID
                        && (i.ID != _IrrigationBoundary.ID || _IrrigationBoundary.ID == 0));

            return qIsSectionRDsExists;

        }
        public bool SaveIrrigationBoundaries(CO_ChannelIrrigationBoundaries _IrrigationBoundaries)
        {
            if (_IrrigationBoundaries.ID == 0)
            {
                int _ChannelTotalRD = db.Repository<CO_Channel>().FindById(_IrrigationBoundaries.ChannelID).TotalRDs;
                _IrrigationBoundaries.SectionToRD = _ChannelTotalRD;
                UpdatePreviousIrrigationBoundary(_IrrigationBoundaries.SectionRD);
                db.Repository<CO_ChannelIrrigationBoundaries>().Insert(_IrrigationBoundaries);
            }
            else
            {
                CO_ChannelIrrigationBoundaries objIrrigation = this.db.Repository<CO_ChannelIrrigationBoundaries>().FindById(_IrrigationBoundaries.ID);
                objIrrigation.ID = _IrrigationBoundaries.ID;
                objIrrigation.ChannelID = _IrrigationBoundaries.ChannelID;
                objIrrigation.SectionID = _IrrigationBoundaries.SectionID;
                objIrrigation.SectionRD = _IrrigationBoundaries.SectionRD;
                objIrrigation.SectionToRD = _IrrigationBoundaries.SectionToRD;

                db.Repository<CO_ChannelIrrigationBoundaries>().Update(objIrrigation);
            }

            db.Save();
            return true;

        }
        public bool UpdatePreviousIrrigationBoundary(int _CurrentRD)
        {
            long _IrrigationBoundaryID = db.Repository<CO_ChannelIrrigationBoundaries>().GetAll().Max(u => (long)u.ID);
            CO_ChannelIrrigationBoundaries objIrrigation = db.Repository<CO_ChannelIrrigationBoundaries>().FindById(_IrrigationBoundaryID);
            objIrrigation.SectionToRD = _CurrentRD;
            db.Repository<CO_ChannelIrrigationBoundaries>().Update(objIrrigation);
            db.Save();
            return true;

        }
        public bool DeleteIrrigationBoundaries(long _IrrigationBoundariesID, long _ChannelID)
        {
            db.Repository<CO_ChannelIrrigationBoundaries>().Delete(_IrrigationBoundariesID);
            List<CO_ChannelGauge> lstGauges = db.Repository<CO_ChannelGauge>().GetAll().Where(q => q.ChannelID == _ChannelID).ToList();
            foreach (var cg in lstGauges)
                db.Repository<CO_ChannelGauge>().Delete(cg.ID);

            db.Save();
            return true;
        }
        /// <summary>
        /// This function returns Channel Administrative Boundaries
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetAdministrativeBoundariesByChannelID(long _ChannelID)
        {
            ChannelRepository repChannelRepository = this.db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetAdministrativeBoundariesByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function returns Channel Divisions
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByChannelID(long _ChannelID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetDivisionsByChannelID(_ChannelID);

        }
        /// <summary>
        /// This function returns Channel Districts
        /// Created on: 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetDistrictsByChannelID(long _ChannelID, bool? _IsActive = null)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetDistrictsByChannelID(_ChannelID, _IsActive);

        }
        /// <summary>
        /// This function check Village existance in Admin Boundaries
        /// Created on 10-11-2015
        /// </summary>
        /// <param name="_AdminBoundary"></param>
        /// <returns>bool</returns>
        public bool IsVillageExists(CO_ChannelAdminBoundries _AdminBoundary)
        {

            bool qIsVillageExists = db.Repository<CO_ChannelAdminBoundries>().GetAll().Any(i => i.ChannelID == _AdminBoundary.ChannelID.Value
                 && i.VillageID == _AdminBoundary.VillageID.Value
                 && i.PoliceStationID == _AdminBoundary.PoliceStationID.Value
                 && i.ChannelSide.Contains(_AdminBoundary.ChannelSide)
                 && (i.ID != _AdminBoundary.ID || _AdminBoundary.ID == 0));

            return qIsVillageExists;

        }
        public bool SaveAdministrativeBoundaries(CO_ChannelAdminBoundries _AdministrativeBoundaries)
        {
            if (_AdministrativeBoundaries.ID == 0)
                db.Repository<CO_ChannelAdminBoundries>().Insert(_AdministrativeBoundaries);
            else
            {
                CO_ChannelAdminBoundries objAdmin = this.db.Repository<CO_ChannelAdminBoundries>().FindById(_AdministrativeBoundaries.ID);
                objAdmin.ID = _AdministrativeBoundaries.ID;
                objAdmin.ChannelID = _AdministrativeBoundaries.ChannelID;
                objAdmin.VillageID = _AdministrativeBoundaries.VillageID;
                objAdmin.PoliceStationID = _AdministrativeBoundaries.PoliceStationID;
                objAdmin.FromRD = _AdministrativeBoundaries.FromRD;
                objAdmin.ToRD = _AdministrativeBoundaries.ToRD;
                objAdmin.ChannelSide = _AdministrativeBoundaries.ChannelSide;
                db.Repository<CO_ChannelAdminBoundries>().Update(objAdmin);
            }

            db.Save();
            return true;
        }
        public bool DeleteAdministrativeBoundaries(long _AdministrativeBoundaries)
        {
            db.Repository<CO_ChannelAdminBoundries>().Delete(_AdministrativeBoundaries);
            db.Save();
            return true;
        }
        public CO_ChannelIrrigationBoundaries GetIrrigationBoundaryByID(long _IrrigationBoundariesID)
        {
            CO_ChannelIrrigationBoundaries irrigationBoundary = (from i in db.Repository<CO_ChannelIrrigationBoundaries>().Query().Get()
                                                                 where i.ID == _IrrigationBoundariesID
                                                                 select i).FirstOrDefault();

            return irrigationBoundary;
        }
        public bool IsIrrigationBoundariesDependencyExists(long _ChannelID)
        {
            bool isDependanceExists = false;
            try
            {
                // Check Irrigation boundary exists in Gauge Information
                bool isReachExists = db.Repository<CO_ChannelReach>().GetAll().Any(g => g.ChannelID == _ChannelID);
                // Check Irrigation boundary exists in Channel parent feeder
                bool isParentFeederExists = db.Repository<CO_ChannelParentFeeder>().GetAll().Any(g => g.ChannelID == _ChannelID);
                // Check Irrigation boundary exists in Channel outlets
                bool isOutletExists = db.Repository<CO_ChannelOutlets>().GetAll().Any(g => g.ChannelID == _ChannelID);

                if (isReachExists == true || isParentFeederExists == true || isOutletExists == true)
                    isDependanceExists = true;

                return isDependanceExists;
            }
            catch (Exception ex)
            {
                new WRException(Constants.UserID, ex).LogException(Constants.MessageCategory.Business);
                return false;
            }
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
            bool isUpdated = false;

            CO_Channel channel = GetChannelByID(_ChannelID);
            channel.IsCalculated = _IsCalculated;
            isUpdated = SaveChannel(channel);

            return isUpdated;
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
            List<CO_GaugeCategory> lstGaugeCategories = db.Repository<CO_GaugeCategory>().GetAll().Where(c => (c.IsActive == _IsActive || _IsActive == null)).ToList<CO_GaugeCategory>();

            return lstGaugeCategories;
        }

        /// <summary>
        /// This function returns Channel Sub Divisions in Irrigational Boundaries.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<CO_SubDivision>()</returns>
        public List<CO_SubDivision> GetSubDivisionsByChannelID(long _ChannelID, bool? _IsActive = null)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetSubDivisionsByChannelID(_ChannelID, _IsActive);
        }

        /// <summary>
        /// This function returns Gauge types.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<CO_GaugeType>()</returns>
        public List<CO_GaugeType> GetGaugeTypes()
        {
            List<CO_GaugeType> lstGaugeTypes = db.Repository<CO_GaugeType>().GetAll().OrderBy(z => z.Name).ToList<CO_GaugeType>();

            return lstGaugeTypes;
        }

        /// <summary>
        /// This function returns Sub Division Sections in Irrigational Boundaries.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<CO_Section>()</returns>
        public List<CO_Section> GetSectionsBySubDivisionChannelID(long _SubDivisionID, long _ChannelID, bool? _IsActive = null)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetSectionsBySubDivisionChannelID(_SubDivisionID, _ChannelID, _IsActive);
        }

        /// <summary>
        /// This function returns Channel Gauge Informations.
        /// Created On 10-11-2015.
        /// </summary> 
        /// <returns>List<object>()</returns>
        public List<object> GetGaugeInformationsByChannelID(long _ChannelID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetGaugeInformationsByChannelID(_ChannelID);
        }

        public object GetDivisionNameBySubDivisonID(long _SubDivisionID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetDivisionNameBySubDivisonID(_SubDivisionID);
        }

        public object GetDivisionNameByIrrigationDomain()
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetDivisionNameByIrrigationDomain();
        }
        public object GetDivisionNameByChannel_ID(long _ChannelID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetDivisionNameByChannel_ID(_ChannelID);
        }

        public List<dynamic> GetSubDivisionNameByChannelID(long DivID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetSubDivisionNameByChannelID(DivID);
        }

        public List<dynamic> GetSectionNameByChannelID(long SubDivID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetSectionNameByChannelID(SubDivID);

        }

        //public List<dynamic> GetDivisionNameByID(long _DivisionID)
        //{
        //    List<dynamic> lstDivision = db.Repository<CO_Division>().GetAll().Where(x => x.ID == _DivisionID).Select(x => new { x.ID, x.Name }).OrderBy(x => x.Name).ToList<dynamic>();
        //    return lstDivision;
        //}
        //public List<dynamic> GetSubDivisionNameByID(long _SubDivisionID)
        //{
        //    List<dynamic> lstDivision = db.Repository<CO_SubDivision>().GetAll().Where(x => x.ID == _SubDivisionID).Select(x => new { x.ID, x.Name }).OrderBy(x => x.Name).ToList<dynamic>();
        //    return lstDivision;
        //}
        //public List<dynamic> GetSectionNameByID(long _SectionID)
        //{
        //    List<dynamic> lstDivision = db.Repository<CO_Section>().GetAll().Where(x => x.ID == _SectionID).Select(x => new { x.ID, x.Name }).OrderBy(x => x.Name).ToList<dynamic>();
        //    return lstDivision;
        //}
        public bool SaveGaugeInformation(CO_ChannelGauge _GaugeInformation)
        {
            bool isSaved = false;

            if (_GaugeInformation.ID == 0)
            {
                long _ChannelGaugeID = db.Repository<CO_ChannelGauge>().GetAll().Max(u => (long)u.ID);
                _GaugeInformation.ID = _ChannelGaugeID + 1;
                db.Repository<CO_ChannelGauge>().Insert(_GaugeInformation);
            }
            else
            {
                CO_ChannelGauge objChannelGauge = db.Repository<CO_ChannelGauge>().FindById(_GaugeInformation.ID);
                objChannelGauge.ID = _GaugeInformation.ID;
                objChannelGauge.ChannelID = _GaugeInformation.ChannelID;
                objChannelGauge.SectionID = _GaugeInformation.SectionID;
                objChannelGauge.GaugeTypeID = _GaugeInformation.GaugeTypeID;
                objChannelGauge.GaugeCategoryID = _GaugeInformation.GaugeCategoryID;
                objChannelGauge.GaugeAtRD = _GaugeInformation.GaugeAtRD;
                objChannelGauge.DesignDischarge = _GaugeInformation.DesignDischarge;
                objChannelGauge.GaugeLevelID = _GaugeInformation.GaugeLevelID;
                objChannelGauge.IsSubDivGauge = _GaugeInformation.IsSubDivGauge;
                objChannelGauge.IsDivGauge = _GaugeInformation.IsDivGauge;
                db.Repository<CO_ChannelGauge>().Update(objChannelGauge);
            }

            db.Save();
            isSaved = true;

            return isSaved;

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
            db.Repository<CO_ChannelGauge>().Delete(_GaugeInformationID);
            db.Save();
            return true;
        }
        /// <summary>
        /// This function check Gauge RD existance
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_GaugeInoformation"></param>
        /// <returns>bool</returns>
        public bool IsGaugeRDExists(CO_ChannelGauge _GaugeInoformation)
        {
            bool GaugeRDExists = false;
            GaugeRDExists = db.Repository<CO_ChannelGauge>().GetAll().Any(g => g.ChannelID == _GaugeInoformation.ChannelID.Value
               && g.GaugeAtRD == _GaugeInoformation.GaugeAtRD
               && (g.ID != _GaugeInoformation.ID || _GaugeInoformation.ID == 0));

            return GaugeRDExists;
        }
        public bool DeleteAutoDeterminedGauges(long _ChannelID)
        {
            if (db.Repository<CO_ChannelGauge>().GetAll().Any(c => c.ChannelID == _ChannelID))
            {
                List<CO_ChannelGauge> lstChannelGauges = db.Repository<CO_ChannelGauge>().GetAll().Where(g => g.ChannelID == _ChannelID).ToList<CO_ChannelGauge>();
                foreach (CO_ChannelGauge item in lstChannelGauges)
                {
                    DeleteBedLevelDTParametersByGaugeID(item.ID);
                    DeleteCrestLevelDTParametersByGaugeID(item.ID);

                    db.Repository<CO_ChannelGauge>().Delete(item);
                    db.Save();
                }
            }
            return true;
        }
        private bool DeleteBedLevelDTParametersByGaugeID(long _GaugeID)
        {
            if (db.Repository<CO_ChannelGaugeDTPGatedStructure>().GetAll().Any(d => d.GaugeID.Value == _GaugeID))
            {
                CO_ChannelGaugeDTPGatedStructure BedLevel = (from b in db.Repository<CO_ChannelGaugeDTPGatedStructure>().Query().Get()
                                                             where b.GaugeID == _GaugeID
                                                             select b).FirstOrDefault();

                // Delete Bed Level DT Parameters
                db.Repository<CO_ChannelGaugeDTPGatedStructure>().Delete(BedLevel);
                db.Save();
            }
            return true;
        }
        private bool DeleteCrestLevelDTParametersByGaugeID(long _GaugeID)
        {
            if (db.Repository<CO_ChannelGaugeDTPFall>().GetAll().Any(d => d.GaugeID.Value == _GaugeID))
            {
                CO_ChannelGaugeDTPFall crestLevel = (from c in db.Repository<CO_ChannelGaugeDTPFall>().Query().Get()
                                                     where c.GaugeID == _GaugeID
                                                     select c).FirstOrDefault();
                // Delete Crest Level DT Parameters
                db.Repository<CO_ChannelGaugeDTPFall>().Delete(crestLevel);
                db.Save();
            }
            return true;
        }
        public bool AutoDetermineGaguesFromIrrigationBoundaries(long _ChannelID)
        {
            // Return true in case of bit is false
            if (GetChannelByID(_ChannelID).IsCalculated == false)
                return false;

            DeleteAutoDeterminedGauges(_ChannelID);

            bool isCalculated = false;

            List<dynamic> lstIrrigationBoundaries = (from i in db.Repository<CO_ChannelIrrigationBoundaries>().GetAll()
                                                     join section in db.Repository<CO_Section>().GetAll() on i.SectionID equals section.ID
                                                     join subDivision in db.Repository<CO_SubDivision>().GetAll() on section.SubDivID equals subDivision.ID
                                                     join division in db.Repository<CO_Division>().GetAll() on subDivision.DivisionID equals division.ID
                                                     where i.ChannelID == _ChannelID
                                                     select new
                                                     {
                                                         ChannelID = i.ChannelID,
                                                         DivisionID = division.ID,
                                                         DivisionName = division.Name,
                                                         SubDivisionID = subDivision.ID,
                                                         SubDivisionName = subDivision.Name,
                                                         SectionID = section.ID,
                                                         SectionName = section.Name,
                                                         SectionRD = i.SectionRD
                                                     }).OrderBy(i => i.SectionRD)
                                     .ToList<dynamic>();

            //List<dynamic> lstIrrigationBoundaries = db.ExtRepositoryFor<ChannelRepository>().GetIrrigationBoundaries(_ChannelID);

            if (lstIrrigationBoundaries != null && lstIrrigationBoundaries.Count > 0)
            {
                List<CO_ChannelGauge> lstChannelGauge = new List<CO_ChannelGauge>();

                Tuple<List<CO_ChannelGauge>, long> tupleHeadGauge = LocateHeadGauge(lstIrrigationBoundaries);
                if (tupleHeadGauge.Item1 != null && tupleHeadGauge.Item1.Count() > 0)
                    lstChannelGauge.AddRange(tupleHeadGauge.Item1);

                List<CO_ChannelGauge> lstTail = LocateTailGauge(lstIrrigationBoundaries);
                if (lstTail != null && lstTail.Count() > 0)
                    lstChannelGauge.AddRange(lstTail);

                // Get Head Gauge Division ID
                var DivisionID = lstIrrigationBoundaries.Where(s => s.SubDivisionID == tupleHeadGauge.Item2).Select(d => d.DivisionID).FirstOrDefault();

                // Smallest SectionRD can not be Sub Divisional or Divisional Gauge
                lstIrrigationBoundaries.RemoveAll(r => r.SubDivisionID == tupleHeadGauge.Item2);

                List<dynamic> lstDivision = GetIrrigationBoundaryDivisions(lstIrrigationBoundaries, DivisionID);
                //if (lstDivision != null && lstDivision.Count > 0)
                //{
                //    List<CO_ChannelGauge> lstSubDivisionalGauge = LocateSubDivisionalGauge(lstIrrigationBoundaries, lstDivision);
                //    if (lstSubDivisionalGauge != null && lstSubDivisionalGauge.Count() > 0)
                //        lstChannelGauge.AddRange(lstSubDivisionalGauge);

                //    List<CO_ChannelGauge> lstDivisionalGauge = PrepareGaugeInformationList(lstDivision);
                //    if (lstDivisionalGauge != null && lstDivisionalGauge.Count() > 0)
                //        lstChannelGauge.AddRange(lstDivisionalGauge);
                //}

                if (lstChannelGauge != null && lstChannelGauge.Count > 0)
                {
                    foreach (CO_ChannelGauge item in lstChannelGauge)
                    {
                        isCalculated = SaveGaugeInformation(item);
                    }
                }
            }
            return isCalculated;
        }
        /// <summary>
        /// This function return the Head Gauge Details along with Sub DivisionID
        /// Created On: 12-11-2015
        /// </summary>
        /// <param name="_IrrigationBoundaries"></param>
        /// <returns>Tuple<List<CO_ChannelGauge></returns>
        private Tuple<List<CO_ChannelGauge>, long> LocateHeadGauge(List<dynamic> _IrrigationBoundaries)
        {
            // Smallest RDs in Irrigation Boundaries is the Head Gauge
            List<dynamic> lstHeadGaugeDetails = _IrrigationBoundaries.GroupBy(g => new { g.ChannelID, g.DivisionID, g.SubDivisionID, g.SectionID, g.SectionRD })
                                 .Where(h => h.Key.SectionRD == _IrrigationBoundaries.Min(m => m.SectionRD))
                                 .Select(j => new
                                 {
                                     ChannelID = j.Key.ChannelID,
                                     DivisionID = j.Key.DivisionID,
                                     SubDivisionID = j.Key.SubDivisionID,
                                     SectionID = j.Key.SectionID,
                                     SectionRD = j.Key.SectionRD,
                                     GaugeCategoryID = (long)Constants.GaugeCategory.HeadGauge
                                 })
                                .ToList<dynamic>();

            List<CO_ChannelGauge> lstChannelHeadGauge = PrepareGaugeInformationList(lstHeadGaugeDetails);

            Tuple<List<CO_ChannelGauge>, long> lstChannelGaugeDetails = Tuple.Create(lstChannelHeadGauge, lstHeadGaugeDetails[0].SubDivisionID);
            return lstChannelGaugeDetails;
        }
        /// <summary>
        /// This function return the Tail Gauge Deatils
        /// Created on: 12-11-2015
        /// </summary>
        /// <param name="_IrrigationBoundaries"></param>
        /// <returns>List<CO_ChannelGauge></returns>
        private List<CO_ChannelGauge> LocateTailGauge(List<dynamic> _IrrigationBoundaries)
        {
            // Find Tail Gauge Details
            List<dynamic> lstTailGauge = _IrrigationBoundaries.GroupBy(g => new { g.ChannelID, g.DivisionID, g.SubDivisionID, g.SectionID, g.SectionRD })
                                 .Where(g => g.Key.SectionRD == _IrrigationBoundaries.Max(m => m.SectionRD))
                                 .Select(g => new
                                 {
                                     ChannelID = g.Key.ChannelID,
                                     DivisionID = g.Key.DivisionID,
                                     SubDivisionID = g.Key.SubDivisionID,
                                     SectionID = g.Key.SectionID,
                                     // Channel Total RDs is the Tail Gauge
                                     SectionRD = GetChannelTotalRD(g.Key.ChannelID).ChannelTotalRDs,
                                     GaugeCategoryID = (long)Constants.GaugeCategory.TailGauge
                                 }).ToList<dynamic>();

            List<CO_ChannelGauge> lstChannelTailGauge = PrepareGaugeInformationList(lstTailGauge);

            return lstChannelTailGauge;
        }
        private List<CO_ChannelGauge> LocateSubDivisionalGauge(List<dynamic> _IrrigationBoundaries, List<dynamic> _Divisions)
        {
            List<CO_ChannelGauge> lstGaugeInformation = null;

            List<dynamic> lstSectionRDs = GetSubDivisionSectionRDs(_IrrigationBoundaries, _Divisions);

            // Get Sub Divisions in Division
            List<dynamic> lstSubDivisions = _Divisions.Select(s => s.SubDivisionID).ToList<dynamic>();

            if (lstSectionRDs != null && lstSubDivisions != null && lstSectionRDs.Count > 0 && lstSubDivisions.Count > 0)
            {
                List<dynamic> lstSubDivisionalGauge = (from i in _IrrigationBoundaries
                                                       where lstSectionRDs.Contains(i.SectionRD)
                                                       select new
                                                       {
                                                           ChannelID = i.ChannelID,
                                                           DivisionID = i.DivisionID,
                                                           DivisionName = i.DivisionName,
                                                           SubDivisionID = i.SubDivisionID,
                                                           SubDivisionName = i.SubDivisionName,
                                                           SectionID = i.SectionID,
                                                           SectionName = i.SectionName,
                                                           SectionRD = i.SectionRD
                                                       }).ToList<dynamic>()
                                                      .Select(sub => new
                                                      {
                                                          ChannelID = sub.ChannelID,
                                                          DivisionID = sub.DivisionID,
                                                          DivisionName = sub.DivisionName,
                                                          SubDivisionID = sub.SubDivisionID,
                                                          SubDivisionName = sub.SubDivisionName,
                                                          SectionID = sub.SectionID,
                                                          SectionName = sub.SectionName,
                                                          SectionRD = sub.SectionRD,
                                                          GaugeCategoryID = (long)Constants.GaugeCategory.SubDivisionalGauge
                                                      }).Where(s => !lstSubDivisions.Contains(s.SubDivisionID)).ToList<dynamic>();



                lstGaugeInformation = PrepareGaugeInformationList(lstSubDivisionalGauge);
            }
            return lstGaugeInformation;
        }
        private List<dynamic> GetIrrigationBoundaryDivisions(List<dynamic> _IrrigationBoundaries, long _DivisionID)
        {

            List<dynamic> lstSectionRDs = GetDivisionsSectionRDs(_IrrigationBoundaries);

            List<dynamic> lstDivisions = (from i in _IrrigationBoundaries
                                          where lstSectionRDs.Contains(i.SectionRD)
                                          select new
                                          {
                                              ChannelID = i.ChannelID,
                                              DivisionID = i.DivisionID,
                                              DivisionName = i.DivisionName,
                                              SubDivisionID = i.SubDivisionID,
                                              SubDivisionName = i.SubDivisionName,
                                              SectionID = i.SectionID,
                                              SectionName = i.SectionName,
                                              SectionRD = i.SectionRD,
                                              GaugeCategoryID = (long)Constants.GaugeCategory.DivisionalGauge
                                          }).ToList<dynamic>();

            // Divisional and Sub  Divisional Guage will not be installed on Head Gauge
            // Remove Head Gauge DivisionID from list
            lstDivisions.RemoveAll(d => d.DivisionID == _DivisionID);

            return lstDivisions;
        }

        /// <summary>
        /// This function return list of Sub Divisions with smallest Section RDs
        /// Created on: 12-11-2015
        /// </summary>
        /// <param name="_IrrigationBoundaries"></param>
        /// <returns>List<dynamic></returns>
        private List<dynamic> GetSubDivisionSectionRDs(List<dynamic> _IrrigationBoundaries, List<dynamic> _Divisions)
        {
            // Get list of Sub Divisions with smallest Section RDs
            List<dynamic> lstSectionRDs = (from g in _IrrigationBoundaries
                                           group g by new { g.ChannelID, g.SubDivisionID } into grp
                                           select new
                                           {
                                               SubDivisionID = grp.Key.SubDivisionID,
                                               SectionRD = grp.Min(m => m.SectionRD)

                                           }).Select(p => p.SectionRD).ToList<dynamic>();
            return lstSectionRDs;
        }

        /// <summary>
        /// This function return list of Divisions with smallest Section RDs
        /// Created on: 12-11-2015
        /// </summary>
        /// <param name="_IrrigationBoundaries"></param>
        /// <returns>List<dynamic></returns>
        private List<dynamic> GetDivisionsSectionRDs(List<dynamic> _IrrigationBoundaries)
        {

            List<dynamic> lstDivisionSectionRDs = (from g in _IrrigationBoundaries
                                                   group g by new { g.ChannelID, g.DivisionID } into grp
                                                   select new
                                                   {
                                                       DivisionID = grp.Key.DivisionID,
                                                       SectionRD = grp.Min(m => m.SectionRD)

                                                   }).Select(p => p.SectionRD).ToList<dynamic>();
            return lstDivisionSectionRDs;
        }
        private List<CO_ChannelGauge> PrepareGaugeInformationList(List<dynamic> _lstGaugeInformatoinData)
        {
            List<CO_ChannelGauge> lstChannelGauge = new List<CO_ChannelGauge>();

            foreach (dynamic item in _lstGaugeInformatoinData)
            {
                CO_ChannelGauge channelGauge = new CO_ChannelGauge();
                channelGauge.ChannelID = item.ChannelID;
                channelGauge.SectionID = item.SectionID;
                channelGauge.GaugeLevelID = 1;
                channelGauge.GaugeAtRD = item.SectionRD;
                channelGauge.GaugeCategoryID = item.GaugeCategoryID;
                lstChannelGauge.Add(channelGauge);
            }

            return lstChannelGauge;
        }

        /// <summary>
        /// This function check Gauge Information exists in BedLevelDT Parameters and CrestLevelDTParameters
        /// Created on: 30-11-2015
        /// </summary>
        /// <param name="_GaugeID"></param>
        /// <returns>bool</returns>
        public bool IsGaugeDependanceExists(long _GaugeID)
        {
            bool IsGaugeDependanceExists = false;

            // Check Gauge exists in Bed Level DT Parameters
            bool IsBedLevelExists = db.Repository<CO_ChannelGaugeDTPGatedStructure>().GetAll().Any(g => g.GaugeID == _GaugeID);

            // Check Gauge exists in Crest Level DT Parameters
            bool IsCrestLevelExists = db.Repository<CO_ChannelGaugeDTPFall>().GetAll().Any(c => c.GaugeID == _GaugeID);

            if (IsBedLevelExists == true || IsCrestLevelExists == true)
                IsGaugeDependanceExists = true;

            return IsGaugeDependanceExists;
        }
        public bool IsGaugeInformationExists(long _ChannelID)
        {
            return db.Repository<CO_ChannelGauge>().GetAll().Any(g => g.ChannelID == _ChannelID);
        }
        /// <summary>
        ///  This function return Section Irrigation Boundary
        ///  Created on: 10-02-2016
        /// </summary>
        /// <param name="_SectionID"></param>
        /// <returns>CO_ChannelIrrigationBoundaries</returns>
        public CO_ChannelIrrigationBoundaries GetIrrigationBoundaryBySection(long _ChannelID, long _SectionID)
        {
            var qIrrigationBoundary = (from i in db.Repository<CO_ChannelIrrigationBoundaries>().GetAll()
                                       where i.ChannelID == _ChannelID && i.SectionID == _SectionID
                                       select i).FirstOrDefault();
            return qIrrigationBoundary;
        }

        public bool UpdateChannelGaugeByChannelID(long _ID, long? _GaugeDivID, long? _GaugeSubDivID, long? _GaugeSectionID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.UpdateChannelGaugeByChannelID(_ID, _GaugeDivID, _GaugeSubDivID, _GaugeSectionID);
        }
        public CO_ChannelGauge GetChannelGaugeByChannelID(long _ChannelID)
        {
            CO_ChannelGauge qChannelGauge = db.Repository<CO_ChannelGauge>().GetAll().FirstOrDefault(s => s.ChannelID == _ChannelID);
            return qChannelGauge;
        }

        public long? GetCircleIDByDivID(long _DivID)
        {
            return db.Repository<CO_Division>().GetAll().Where(x => x.ID == _DivID).Select(x => x.CircleID).FirstOrDefault();
        }
        public long? GetDivisionIDBySubDivID(long _SubDivID)
        {
            return db.Repository<CO_SubDivision>().GetAll().Where(x => x.ID == _SubDivID).Select(x => x.DivisionID).FirstOrDefault();
        }
        public long? GetSubDivIDBySectionID(long _SectionID)
        {
            return db.Repository<CO_Section>().GetAll().Where(x => x.ID == _SectionID).Select(x => x.SubDivID).FirstOrDefault();
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
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetChannelParentFeedersByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function returns Channel Structures
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public List<dynamic> GetChannelStructures(long _ChannelID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetChannelStructures(_ChannelID);
        }
        /// <summary>
        /// This function returns RD at Channel
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns></returns>
        public double GetRDAtChannel(long _ChannelID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetRDAtChannel(_ChannelID);
        }

        public List<dynamic> GetChannelParentFeedersNameByChannelID(long _ChannelID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetChannelParentFeedersNameByChannelID(_ChannelID);
        }

        public List<dynamic> BindIMISCode(long _ParentFeederID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.BindIMISCode(_ParentFeederID);
        }

        public string GetStructureTypeID(long _ParentFeederID)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.GetStructureTypeID(_ParentFeederID);
        }

        public bool UpdateIMISCodeChannelID(long _ID, string _IMISCode)
        {
            ChannelRepository repChannelRepository = db.ExtRepositoryFor<ChannelRepository>();
            return repChannelRepository.UpdateIMISCodeChannelID(_ID, _IMISCode);
        }
        public bool IsIMISCodeExists(long _ChannelID, string IMIS)
        {
            bool isIMISCodeExists = db.Repository<CO_Channel>().GetAll().Any(i => i.IMISCode == IMIS && i.ID != _ChannelID);
            //bool isIMISCodeExists = db.Repository<CO_Channel>().GetAll().Any(i => i.IMISCode == IMIS);
            return isIMISCodeExists;
        }


        public bool SaveChannelParentFeeder(CO_ChannelParentFeeder _ParentFeeder)
        {
            bool isSaved = false;

            if (_ParentFeeder.ID == 0)
                db.Repository<CO_ChannelParentFeeder>().Insert(_ParentFeeder);
            else
            {
                CO_ChannelParentFeeder objParentFeeder = db.Repository<CO_ChannelParentFeeder>().FindById(_ParentFeeder.ID);
                objParentFeeder.ID = _ParentFeeder.ID;
                objParentFeeder.ChannelID = _ParentFeeder.ChannelID;
                objParentFeeder.ParrentFeederID = _ParentFeeder.ParrentFeederID;
                objParentFeeder.RelationType = _ParentFeeder.RelationType;
                objParentFeeder.ChannelSide = _ParentFeeder.ChannelSide;
                objParentFeeder.ChannelRDS = _ParentFeeder.ChannelRDS;
                objParentFeeder.ParrentFeederRDS = _ParentFeeder.ParrentFeederRDS;
                objParentFeeder.StructureTypeID = _ParentFeeder.StructureTypeID;
                db.Repository<CO_ChannelParentFeeder>().Update(objParentFeeder);
            }

            db.Save();
            isSaved = true;

            return isSaved;
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
            bool qIsChannelParentFeederExists = db.Repository<CO_ChannelParentFeeder>().GetAll().Any(p => p.ParrentFeederID == _ChannelParentFeederID
                           && p.StructureTypeID == _StructureTypeID
                           && p.ChannelID == _ChannelID
                           && (p.ID != _ID || _ID == 0));
            return qIsChannelParentFeederExists;
        }
        /// <summary>
        /// This function delete Channel Parent Feeders
        /// Created on 11-11-2015
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public bool DeleteChannelParentFeeder(long _ID)
        {
            db.Repository<CO_ChannelParentFeeder>().Delete(_ID);
            db.Save();

            return true;
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
            bool qIsChannelSideAndRDExists = db.Repository<CO_ChannelParentFeeder>().GetAll().Any(p => p.ChannelSide.ToLower().Contains(_ChannelSide.ToLower())
               && p.ChannelRDS == _RDAtChannel);
            return qIsChannelSideAndRDExists;
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
            bool qIsChannelSideAndRDExists = db.Repository<CO_ChannelParentFeeder>().GetAll().Any(p => p.ChannelSide.ToLower().Contains(_ChannelSide.ToLower())
               && p.ParrentFeederRDS == _ParrentFeederRDS);
            return qIsChannelSideAndRDExists;
        }
        #endregion



        public bool IsChannelParentFeederExistsForIMIS(long _ChannelID)
        {
            Int64 _StructureTypeID = (Int64)Constants.StructureType.Channel;
            bool qIsChannelParentFeederExists = db.Repository<CO_ChannelParentFeeder>().GetAll().Any(p => p.ChannelID == _ChannelID
                           && p.StructureTypeID == _StructureTypeID
                           && (p.ID != 0));
            return qIsChannelParentFeederExists;
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
            CO_ChannelGauge mdlChannelGauge = db.Repository<CO_ChannelGauge>().FindById(_ChannelGaugeID);

            return mdlChannelGauge;
        }
        public List<object> GetChannelGaugeRDByChannelID(long _ChannelID)
        {
            List<object> mdlChannelGauge = (from item in db.Repository<CO_ChannelGauge>().GetAll() where item.ChannelID == _ChannelID select new { ID = item.ID, Name = item.GaugeAtRD }).ToList().Select(g => new { ID = g.ID, Name = Calculations.GetRDText(g.Name) }).ToList<object>();
            return mdlChannelGauge;
        }

        /// <summary>
        /// This function adds new Bed Level Datetable Parameter.
        /// Created On 13-11-2015.
        /// </summary>
        /// <param name="_ChannelGaugeDTPGatedStructure"></param>
        /// <returns>bool</returns>
        public bool AddBedLevelDTParameters(CO_ChannelGaugeDTPGatedStructure _ChannelGaugeDTPGatedStructure)
        {

            bool isSaved = false;

            if (_ChannelGaugeDTPGatedStructure.ID == 0)
            {
                db.Repository<CO_ChannelGaugeDTPGatedStructure>().Insert(_ChannelGaugeDTPGatedStructure);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<CO_ChannelGaugeDTPGatedStructure>().Update(_ChannelGaugeDTPGatedStructure);
                db.Save();
                isSaved = true;
            }

            return isSaved;
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
            List<CO_ChannelGaugeDTPGatedStructure> lstParameterHistory = db.Repository<CO_ChannelGaugeDTPGatedStructure>().GetAll().Where(st => st.GaugeID == _GuageID).ToList();

            lstParameterHistory = lstParameterHistory.Where(st => (_ToDate == null || st.ReadingDate.Value.Date <= _ToDate.Value.Date) &&
                (_FromDate == null || st.ReadingDate.Value.Date >= _FromDate.Value.Date)).OrderBy(cgdt => cgdt.ReadingDate).ToList();

            //lstParameterHistory.ForEach(e => Math.Round(e.DischargeCoefficient,1));

            return lstParameterHistory;
        }

        /// <summary>
        /// This function adds new Crest Level Datetable Parameter.
        /// Created On 19-11-2015.
        /// </summary>
        /// <param name="_ChannelGaugeDTPFall"></param>
        /// <returns>bool</returns>
        //public bool AddCrestLevelDTParameters(CO_ChannelGaugeDTPFall _ChannelGaugeDTPFall)
        //{
        //    db.Repository<CO_ChannelGaugeDTPFall>().Insert(_ChannelGaugeDTPFall);
        //    db.Save();

        //    return true;
        //}
        public bool AddCrestLevelDTParameters(CO_ChannelGaugeDTPFall _ChannelGaugeDTPFall)
        {
            bool isSaved = false;

            if (_ChannelGaugeDTPFall.ID == 0)
            {
                db.Repository<CO_ChannelGaugeDTPFall>().Insert(_ChannelGaugeDTPFall);
                db.Save();
                isSaved = true;
            }
            else
            {
                db.Repository<CO_ChannelGaugeDTPFall>().Update(_ChannelGaugeDTPFall);
                db.Save();
                isSaved = true;
            }

            return isSaved;
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
            List<CO_ChannelGaugeDTPFall> lstParameterHistory = db.Repository<CO_ChannelGaugeDTPFall>().GetAll().Where(st => st.GaugeID == _GuageID).ToList();

            lstParameterHistory = lstParameterHistory.Where(st => (_ToDate == null || st.ReadingDate.Value.Date <= _ToDate.Value.Date) &&
                (_FromDate == null || st.ReadingDate.Value.Date >= _FromDate.Value.Date)).OrderBy(cgdt => cgdt.ReadingDate).ToList();

            return lstParameterHistory;
        }

        public bool AddOutletPeroformance(CO_ChannelOutletsPerformance _ChannelOutletsPerformance)
        {
            db.Repository<CO_ChannelOutletsPerformance>().Insert(_ChannelOutletsPerformance);
            db.Save();

            return true;
        }

        #endregion

        public DataSet GetChannelLineDiagram(string _Division, string _Channel, DateTime? _FromDate)
        {
            return db.ExecuteStoredProcedureDataSet("PW_LineDiagram", _Division, _Channel, _FromDate);                
                //db.ExtRepositoryFor<ChannelRepository>().GetChannelLineDiagram(_Division, _Channel, _FromDate);
        }

        public List<object> GetMainBranchLinkChannels(long _DivID)
        {
            return db.ExtRepositoryFor<ChannelRepository>().GetMainBranchLinkChannels(_DivID);
        }
    }
}
