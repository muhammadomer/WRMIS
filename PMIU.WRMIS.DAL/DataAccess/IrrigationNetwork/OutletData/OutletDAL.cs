using PMIU.WRMIS.DAL;
using PMIU.WRMIS.DAL.Repositories;
using PMIU.WRMIS.DAL.Repositories.OutletRepository;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.OutletData
{
    public class OutletDAL
    {
        ContextDB db = new ContextDB();
        public List<object> GetOutletHistory(long _ChannelID, int _PageIndex, int _PageSize)
        {
            OutletRepository repOutlet = this.db.ExtRepositoryFor<OutletRepository>();
            return repOutlet.GetOutletHistory(_ChannelID, _PageIndex, _PageSize);
        }
        public bool DeleteOutletByID(long _OutletID)
        {

            int countOutletsRecords = db.Repository<CO_OutletAlterationHistroy>().Query().Get().Where(x => x.OutletID == _OutletID).Count();

            if (countOutletsRecords > 0)
            {
                long id = db.Repository<CO_OutletAlterationHistroy>().Query().Get().Where(x => x.OutletID == _OutletID).FirstOrDefault().ID;
                db.Repository<CO_OutletAlterationHistroy>().Delete(id);
                db.Save();
            }

            if (countOutletsRecords == 1)
            {

                // first it will delete all the related records from outlet location table
                List<CO_ChannelOutletsLocation> lstLocations = db.Repository<CO_ChannelOutletsLocation>().Query().Get().Where(x => x.OutletID == _OutletID).ToList<CO_ChannelOutletsLocation>();
                if (lstLocations != null)
                {
                    foreach (CO_ChannelOutletsLocation loc in lstLocations)
                    {
                        db.Repository<CO_ChannelOutletsLocation>().Delete(loc.ID);
                        db.Save();
                    }
                }

                // then it will delete its parent entry from channel outlet table
                db.Repository<CO_ChannelOutlets>().Delete(_OutletID);
                db.Save();
            }

            return true;
        }
        public string GetFlowType(long _ID)
        {
            CO_ChannelFlowType flowType = db.Repository<CO_ChannelFlowType>().Query().Get().Where(x => x.ID == _ID && x.IsActive.Value == true).FirstOrDefault();
            return flowType.Name;
        }
        public string GetCommandNameByID(long _ID)
        {

            CO_ChannelComndType CommandType = db.Repository<CO_ChannelComndType>().Query().Get().Where(x => x.ID == _ID && x.IsActive.Value == true).FirstOrDefault();
            return CommandType.Name;
        }
        public string GetChannelTypeByID(long _ID)
        {

            CO_ChannelType ChannelType = db.Repository<CO_ChannelType>().Query().Get().Where(x => x.ID == _ID && x.IsActive.Value == true).FirstOrDefault();
            return ChannelType.Name;
        }

        #region Add Outlet Reference Data
        public List<CO_PoliceStation> GetPoliceStations()
        {
            List<CO_PoliceStation> lstPoliceStation = db.Repository<CO_PoliceStation>().Query().Get().ToList<CO_PoliceStation>();
            return lstPoliceStation;
        }
        public List<CO_Village> GetVillagesByID(long _ID)
        {
            List<CO_Village> lstVillages = db.Repository<CO_Village>().Query().Get().Where(x => x.PoliceStationID == _ID).ToList<CO_Village>();
            return lstVillages;
        }
        public List<CO_OutletType> GetOutletTypes()
        {
            List<CO_OutletType> lstOutletTypes = db.Repository<CO_OutletType>().Query().Get().ToList<CO_OutletType>();
            return lstOutletTypes;
        }

        #endregion

        #region Add Outlet Data

        public long AddOutlet(CO_ChannelOutlets _Outlet)
        {
            try
            {
                db.Repository<CO_ChannelOutlets>().Insert(_Outlet);
                db.Save();
                return _Outlet.ID;
            }

            catch (Exception ex)
            {
                return 0;
            }
        }
        public bool SaveLocation(CO_ChannelOutletsLocation _Location)
        {
            try
            {
                db.Repository<CO_ChannelOutletsLocation>().Insert(_Location);
                db.Save();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }

        }
        public bool SaveOutletHistory(CO_OutletAlterationHistroy _OutletHistory)
        {
            try
            {

                db.Repository<CO_OutletAlterationHistroy>().Insert(_OutletHistory);
                db.Save();
                return true;


            }

            catch (Exception ex)
            {
                return false;
            }




        }
        public bool DeleteOutlet(long _OutletID)
        {
            db.Repository<CO_ChannelOutlets>().Delete(_OutletID);
            db.Save();
            return true;
        }
        public long GetOutletTypeID(string _OutletAbbrv)
        {
            try
            {
                long outletID = db.Repository<CO_OutletType>().Query().Get().Where(x => x.Description == _OutletAbbrv).FirstOrDefault().ID;
                return outletID;
            }

            catch (Exception ex)
            {
                return 0;
            }
        }
        public CO_OutletAlterationHistroy GetOutletRefData(long _ID)
        {
            try
            {
                CO_OutletAlterationHistroy outletRef = db.Repository<CO_OutletAlterationHistroy>().Query().Get().Where(x => x.ID == _ID).FirstOrDefault<CO_OutletAlterationHistroy>();
                return outletRef;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        public CO_ChannelAdminBoundries GetBoundary(long _ChannelID, double _OutletRD)
        {
            OutletRepository repOutlet = this.db.ExtRepositoryFor<OutletRepository>();
            return repOutlet.GetBoundary(_ChannelID, _OutletRD);
        }
        public long UpdateOutlet(CO_ChannelOutlets _Outlet)
        {
            CO_ChannelOutlets objOutlet = db.Repository<CO_ChannelOutlets>().FindById(_Outlet.ID);
            objOutlet.ID = _Outlet.ID;
            objOutlet.ChannelID = _Outlet.ChannelID;
            objOutlet.Name = _Outlet.Name;
            objOutlet.OutletRD = _Outlet.OutletRD;
            objOutlet.ChannelSide = _Outlet.ChannelSide;
            objOutlet.AdditionalInformation = _Outlet.AdditionalInformation;

            db.Repository<CO_ChannelOutlets>().Update(objOutlet);
            db.Save();
            return _Outlet.ID;
        }

        #endregion
        public CO_ChannelOutletsLocation GetVLGIDbyOutletID(long _ID)
        {
            CO_ChannelOutletsLocation mdlLoc = db.Repository<CO_ChannelOutletsLocation>().Query().Get().Where(x => x.OutletID.Value == _ID && x.LocatedIn == true).FirstOrDefault();
            return mdlLoc;
        }
        public CO_ChannelOutlets GetOutletByOutletID(long? _ID)
        {
            CO_ChannelOutlets mdlOutlet = db.Repository<CO_ChannelOutlets>().Query().Get().Where(x => x.ID == _ID && x.IsActive.Value == true).FirstOrDefault();
            return mdlOutlet;
        }
        public CO_Village GetVillageNameByID(long? _ID)
        {
            CO_Village VillageName = db.Repository<CO_Village>().Query().Get().Where(x => x.ID == _ID && x.IsActive.Value == true).FirstOrDefault();
            return VillageName;
        }
        /// <summary>
        /// This function check channel outlet exists.
        /// Created on: 08-01-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_OutletRD"></param>
        /// <param name="_ChannelSide"></param>
        /// <returns>bool</returns>
        public bool IsChannelOutletExists(CO_ChannelOutlets _ChannelOutlet, long _OutletID)
        {
            bool qIsChannelOutletExists = db.Repository<CO_ChannelOutlets>().GetAll().Any(c => c.ChannelID == _ChannelOutlet.ChannelID
                && c.ChannelSide == _ChannelOutlet.ChannelSide
                && c.OutletRD == _ChannelOutlet.OutletRD
                && (c.ID != _OutletID || _OutletID == 0));

            return qIsChannelOutletExists;
        }
        public string GetPStationNameByID(long? _ID)
        {

            string PoliceStationName = db.Repository<CO_PoliceStation>().Query().Get().Where(x => x.ID == _ID).FirstOrDefault().Name;
            return PoliceStationName;
        }
        public CO_Tehsil GetTehsilNameByID(long? _ID)
        {

            CO_Tehsil TehsilName = db.Repository<CO_Tehsil>().Query().Get().Where(x => x.ID == _ID && x.IsActive.Value == true).FirstOrDefault();
            return TehsilName;
        }
        public CO_District GetDistrictNameByID(long? _ID)
        {

            CO_District DistrictName = db.Repository<CO_District>().Query().Get().Where(x => x.ID == _ID && x.IsActive.Value == true).FirstOrDefault();
            return DistrictName;
        }
        public string GetOLTypeAbbrvByID(long? _ID)
        {

            string OLTypeName = db.Repository<CO_OutletType>().Query().Get().Where(x => x.ID == _ID).FirstOrDefault().Description;
            return OLTypeName;
        }
        /// <summary>
        /// This function return latest Alteration Date
        /// Created on: 13-01-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_OutletID"></param>
        /// <param name="_AlterationID"></param>
        /// <returns>DateTime</returns>
        public DateTime GetLatestAlterationDate(long _ChannelID, long _OutletID, long _AlterationID)
        {
            var qAlterationDate = db.Repository<CO_OutletAlterationHistroy>().GetAll()
                .Where(o => o.OutletID == _OutletID && o.ID == _AlterationID)
                .Select(o => new
                {
                    LatestAlterationDate = o.AlterationDate
                }).FirstOrDefault();
            return Convert.ToDateTime(qAlterationDate.LatestAlterationDate);
        }

        #region "Outlet Alteration History"
        public List<object> GetOutletAlterationHistory(long _OutletID, string _FromDate, string _ToDate)
        {
            List<object> lstOutletAlterationHistory = db.ExtRepositoryFor<OutletRepository>().GetOutletAlterationHistory(_OutletID, _FromDate, _ToDate);
            return lstOutletAlterationHistory;
        }
        #endregion

        #region "Outlet Village"
        /// <summary>
        /// This function return Villages exists in Tehsils
        /// Created on: 19-01-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_Village></returns>
        public List<CO_Village> GetVillagesByChannelID(long _ChannelID)
        {
            OutletRepository repChannelRepository = this.db.ExtRepositoryFor<OutletRepository>();
            return repChannelRepository.GetVillagesByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function return Channel Outlet Locations
        /// Created on: 19-01-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletVillagesByOutletID(long _OutletID)
        {
            OutletRepository repChannelRepository = this.db.ExtRepositoryFor<OutletRepository>();
            return repChannelRepository.GetOutletVillagesByOutletID(_OutletID);
        }
        /// <summary>
        ///  In case the value of  ‘Outlet Installed at Village’ is ‘Yes’,
        ///  system will check if the village name is available against Outlet 
        ///  R.D in Administrative Boundaries
        ///  Created on 19-01-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <param name="_VillageID"></param>
        /// <returns>bool</returns>
        public bool IsVillageNameExistsInAdminBoundaries(long _ChannelID, long _VillageID)
        {
            bool qIsVillageExists = db.Repository<CO_ChannelAdminBoundries>().GetAll().Any(i => i.ChannelID == _ChannelID
                && i.VillageID == _VillageID);
            return qIsVillageExists;
        }
        /// <summary>
        /// This function check Village name already exists in Channel Outlet Locations
        /// Created on 19-01-2015
        /// </summary>
        /// <param name="_OutletLocation"></param>
        /// <returns>bool</returns>
        public bool IsOutletVillageExists(CO_ChannelOutletsLocation _OutletLocation)
        {
            bool qIsOutletInstalled = db.Repository<CO_ChannelOutletsLocation>().GetAll().Any(i => i.VillageID == _OutletLocation.VillageID
             && i.OutletID == _OutletLocation.OutletID
             && (i.ID != _OutletLocation.ID || _OutletLocation.ID == 0));
            return qIsOutletInstalled;
        }
        /// <summary>
        /// This function check Outlet is installed or not
        /// Created on 19-01-2015
        /// </summary>
        /// <param name="_OutletLocation"></param>
        /// <returns>bool</returns>
        public bool IsOutletInstalled(CO_ChannelOutletsLocation _OutletLocation)
        {
            bool qIsOutletInstalled = db.Repository<CO_ChannelOutletsLocation>().GetAll().Any(i => i.OutletID == _OutletLocation.OutletID
             && i.LocatedIn == _OutletLocation.LocatedIn
             && (i.ID != _OutletLocation.ID || _OutletLocation.ID == 0));
            return qIsOutletInstalled;
        }
        /// <summary>
        /// This function save Channel Outlet Location
        /// Created on 19-01-2016
        /// </summary>
        /// <param name="_OutletLocation"></param>
        /// <returns>bool</returns>
        public bool SaveChannelOutletsLocation(CO_ChannelOutletsLocation _OutletLocation)
        {
            bool isSaved = false;

            if (_OutletLocation.ID == 0)
            {
                db.Repository<CO_ChannelOutletsLocation>().Insert(_OutletLocation);
                db.Save();
                isSaved = true;
            }
            else
            {
                CO_ChannelOutletsLocation objOutletLocation = db.Repository<CO_ChannelOutletsLocation>().FindById(_OutletLocation.ID);
                objOutletLocation.ID = _OutletLocation.ID;
                objOutletLocation.OutletID = _OutletLocation.OutletID;
                objOutletLocation.VillageID = _OutletLocation.VillageID;
                objOutletLocation.LocatedIn = _OutletLocation.LocatedIn;
                db.Repository<CO_ChannelOutletsLocation>().Update(objOutletLocation);
                db.Save();
                isSaved = true;
            }

            return isSaved;
        }
        /// <summary>
        /// This function delete Channel Outlet Location
        /// Created on 19-01-2016
        /// </summary>
        /// <param name="_OutletLocationID"></param>
        /// <returns>bool</returns>
        public bool DeleteChannelOutletsLocation(long _OutletLocationID)
        {
            db.Repository<CO_ChannelOutletsLocation>().Delete(_OutletLocationID);
            db.Save();
            return true;
        }
        #endregion

    }
}