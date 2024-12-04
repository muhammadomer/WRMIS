using PMIU.WRMIS.DAL.DataAccess.IrrigationNetwork.OutletData;
using PMIU.WRMIS.DAL.Repositories;
using PMIU.WRMIS.DAL.Repositories.OutletRepository;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.BLL.IrrigationNetwork.OutletData
{
    public class IN_OutletBLL : BaseBLL
    {
        public List<object> GetOutletHistory(long _ChannelID, int _PageIndex, int _PageSize)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetOutletHistory(_ChannelID, _PageIndex, _PageSize);
        }
        public bool DeleteOutletByID(long _OutletID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.DeleteOutletByID(_OutletID);
        }
        public string GetFlowType(long _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetFlowType(_ID);
        }
        public string GetCommandNameByID(long _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetCommandNameByID(_ID);
        }
        public string GetChannelTypeByID(long _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetChannelTypeByID(_ID);
        }

        #region Add Outlet Reference Data
        public List<CO_PoliceStation> GetPoliceStations()
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetPoliceStations();
        }
        public List<CO_Village> GetVillagesByID(long _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetVillagesByID(_ID);
        }
        public List<CO_OutletType> GetOutletTypes()
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetOutletTypes();
        }

        #endregion

        #region Add Outlet Data

        public long AddOutlet(CO_ChannelOutlets _Outlet)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.AddOutlet(_Outlet);
        }
        public bool SaveLocation(CO_ChannelOutletsLocation _Location)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.SaveLocation(_Location);
        }
        public bool SaveOutletHistory(CO_OutletAlterationHistroy _OutletHistory)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.SaveOutletHistory(_OutletHistory);
        }
        public bool DeleteOutlet(long _OutletID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.DeleteOutlet(_OutletID);
        }
        public long GetOutletTypeID(string _OutletAbbrv)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetOutletTypeID(_OutletAbbrv);
        }
        public CO_OutletAlterationHistroy GetOutletRefData(long _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetOutletRefData(_ID);
        }
        public CO_ChannelAdminBoundries GetBoundary(long _ChannelID, double _OutletRD)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetBoundary(_ChannelID, _OutletRD);
        }
        public long UpdateOutlet(CO_ChannelOutlets _Outlet)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.UpdateOutlet(_Outlet);
        }

        #endregion
        public CO_ChannelOutletsLocation GetVLGIDbyOutletID(long _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetVLGIDbyOutletID(_ID);
        }
        public CO_ChannelOutlets GetOutletByOutletID(long? _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetOutletByOutletID(_ID);
        }
        public CO_Village GetVillageNameByID(long? _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetVillageNameByID(_ID);
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
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.IsChannelOutletExists(_ChannelOutlet, _OutletID);
        }
        public string GetPStationNameByID(long? _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetPStationNameByID(_ID);
        }
        public CO_Tehsil GetTehsilNameByID(long? _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetTehsilNameByID(_ID);
        }
        public CO_District GetDistrictNameByID(long? _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetDistrictNameByID(_ID);
        }
        public string GetOLTypeAbbrvByID(long? _ID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetOLTypeAbbrvByID(_ID);
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
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetLatestAlterationDate(_ChannelID, _OutletID, _AlterationID);
        }

        #region "Outlet Alteration History"
        public List<object> GetOutletAlterationHistory(long _OutletID, string _FromDate, string _ToDate)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetOutletAlterationHistory(_OutletID, _FromDate, _ToDate);
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
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetVillagesByChannelID(_ChannelID);
        }
        /// <summary>
        /// This function return Channel Outlet Locations
        /// Created on: 19-01-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletVillagesByOutletID(long _OutletID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.GetOutletVillagesByOutletID(_OutletID);
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
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.IsVillageNameExistsInAdminBoundaries(_ChannelID, _VillageID);
        }
        /// <summary>
        /// This function check Village name already exists in Channel Outlet Locations
        /// Created on 19-01-2015
        /// </summary>
        /// <param name="_OutletLocation"></param>
        /// <returns>bool</returns>
        public bool IsOutletVillageExists(CO_ChannelOutletsLocation _OutletLocation)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.IsOutletVillageExists(_OutletLocation);
        }
        /// <summary>
        /// This function check Outlet is installed or not
        /// Created on 19-01-2015
        /// </summary>
        /// <param name="_OutletLocation"></param>
        /// <returns>bool</returns>
        public bool IsOutletInstalled(CO_ChannelOutletsLocation _OutletLocation)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.IsOutletInstalled(_OutletLocation);
        }
        /// <summary>
        /// This function save Channel Outlet Location
        /// Created on 19-01-2016
        /// </summary>
        /// <param name="_OutletLocation"></param>
        /// <returns>bool</returns>
        public bool SaveChannelOutletsLocation(CO_ChannelOutletsLocation _OutletLocation)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.SaveChannelOutletsLocation(_OutletLocation);
        }
        /// <summary>
        /// This function delete Channel Outlet Location
        /// Created on 19-01-2016
        /// </summary>
        /// <param name="_OutletLocationID"></param>
        /// <returns>bool</returns>
        public bool DeleteChannelOutletsLocation(long _OutletLocationID)
        {
            OutletDAL dalOutlet = new OutletDAL();
            return dalOutlet.DeleteChannelOutletsLocation(_OutletLocationID);
        }
        #endregion

    }
}