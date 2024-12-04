using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Exceptions;
using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.Channel;
using PMIU.WRMIS.AppBlocks;
using System.Data.Entity;


namespace PMIU.WRMIS.DAL.Repositories.OutletRepository
{
    public class OutletRepository : Repository<CO_Channel>
    {
        WRMIS_Entities _context;
        public OutletRepository(WRMIS_Entities context)
            : base(context)
        {
            dbSet = context.Set<CO_Channel>();
            _context = context;

        }

        #region Outlet Alteration History Data
        public List<object> GetOutletHistory(long _ChannelID, int _PageIndex, int _PageSize)
        {
            List<object> lstOutletHistory = context.usp_GetOutletAlterationHistory(_ChannelID, _PageIndex, _PageSize)
                                            .ToList().OrderByDescending(s => s.AlterationDate).OrderBy(x=>x.OutletRD).Select(h => new
                                                 {
                                                     RowNumber = h.RowNumber.HasValue ? h.RowNumber.Value : 0,
                                                     OutletID = h.OutletID,
                                                     AlterationDate = h.AlterationDate.HasValue ? Utility.GetFormattedDate(h.AlterationDate.Value) : string.Empty,
                                                     AlterationID = h.AlterationID,

                                                     RDandSide = h.OutletRD.HasValue ? Calculations.GetRDText(h.OutletRD.Value) + "/" + h.ChannelSide : "",
                                                     Village = GetOutletInstalledatVillage(h.OutletID),
                                                     OutletType = GetOutletType(h.OutletTypeID).Abbrivation,

                                                     DesignDischarge = h.DesignDischarge,
                                                     Width = h.OutletWidth.HasValue ? h.OutletWidth.Value : 0,
                                                     Height = h.OutletHeight.HasValue ? h.OutletHeight : 0,

                                                     Crest = h.OutletCrest.HasValue ? h.OutletCrest : 0,
                                                     Submergence = h.OutletSubmergence.HasValue ? h.OutletSubmergence : 0,
                                                     CrestRLL = h.OutletCrestRL.HasValue ? h.OutletCrestRL : 0,
                                                     MinModHead = h.OutletMMH.HasValue ? h.OutletMMH : 0,
                                                     OutletWorkingHead = h.OutletWorkingHead.HasValue ? h.OutletWorkingHead : 0,
                                                     TotalRecords = h.TotalRecords.HasValue ? h.TotalRecords.Value : 0
                                                 }).ToList<object>();
            return lstOutletHistory;
        }
        public CO_OutletType GetOutletType(long? _ID)
        {
            CO_OutletType OutletType = (from x in context.CO_OutletType
                                        where x.ID == _ID
                                        select x).FirstOrDefault<CO_OutletType>();

            return OutletType;
        }
        public string GetOutletInstalledatVillage(long _OutletID)
        {
            string villageName = string.Empty;
            CO_ChannelOutletsLocation qOutletLocation = GetOutletInstalationLocation(_OutletID);
            if (qOutletLocation != null)
            {
                villageName = GetVillage(qOutletLocation.VillageID.Value).Name;
            }
            return villageName;
        }
        public CO_ChannelOutletsLocation GetOutletInstalationLocation(long _OutletID = 0)
        {
            CO_ChannelOutletsLocation qOutletLocation = (from l in context.CO_ChannelOutletsLocation
                                                         where l.OutletID.Value == _OutletID && l.LocatedIn == true
                                                         select l).FirstOrDefault();
            return qOutletLocation;
        }
        public CO_Village GetVillage(long _VillageID = 0)
        {
            CO_Village qVillage = (from v in context.CO_Village
                                   where v.ID == _VillageID
                                   select v).FirstOrDefault();
            return qVillage;
        }

        #endregion

        #region Outlet Boundaries

        public CO_ChannelAdminBoundries GetBoundary(long _ChannelID, double _OutletRD)
        {
            CO_ChannelAdminBoundries q_entityBoundary = new CO_ChannelAdminBoundries();

            try
            {
                q_entityBoundary = (from x in context.CO_ChannelAdminBoundries
                                    where x.ChannelID == _ChannelID && (x.FromRD >= _OutletRD && x.ToRD <= _OutletRD)
                                    select x).FirstOrDefault();


            }
            catch (Exception exp)
            {
                //new WRException(Constants.UserId, exp).LogException(Constants.MessageCategory.DataAccess);
            }

            return q_entityBoundary;

        }


        #endregion

        #region "Outlet Alteration History"
        public List<object> GetOutletAlterationHistory(long _OutletID, string _FromDate, string _ToDate)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (!string.IsNullOrEmpty(_FromDate))
                fromDate = Utility.GetParsedDate(_FromDate);
            if (!string.IsNullOrEmpty(_ToDate))
                toDate = Utility.GetParsedDate(_ToDate);

            List<object> lstOutletAlterationHistory = (from h in context.CO_OutletAlterationHistroy
                                                       join t in context.CO_OutletType on h.OutletTypeID equals t.ID
                                                       where ((h.OutletID == _OutletID)
                                                                && (DbFunctions.TruncateTime(h.AlterationDate) >= (string.IsNullOrEmpty(_FromDate) ? DbFunctions.TruncateTime(h.AlterationDate) : fromDate))
                                                                && (DbFunctions.TruncateTime(h.AlterationDate) <= (string.IsNullOrEmpty(_ToDate) ? DbFunctions.TruncateTime(h.AlterationDate) : toDate)))
                                                       // && (h.ActionType.Trim() == ActionType.Trim()))
                                                       orderby h.ID descending
                                                       select new
                                                       {
                                                           ID = h.ID,
                                                           AlterationDate = h.AlterationDate.Value,
                                                           OutletID = h.OutletID,
                                                           DesignDischarge = h.DesignDischarge,
                                                           OutletType = t.Abbrivation,
                                                           OutletGCA = h.OutletGCA,
                                                           OutletCCA = h.OutletCCA,
                                                           OutletStatus = h.OutletStatus,
                                                           OutletHeight = h.OutletHeight,
                                                           OutletCrest = h.OutletCrest,
                                                           OutletWidth = h.OutletWidth,
                                                           OutletSubmergence = h.OutletSubmergence,
                                                           OutletCrestRL = h.OutletCrestRL,
                                                           OutletMMH = h.OutletMMH,
                                                           OutletWorkingHead = h.OutletWorkingHead,
                                                           FileName = h.OutletAttachment,
                                                           OutletRD = h.OutletRD,
                                                           ChannelSide = h.ChannelSide,
                                                           ActionType = h.ActionType
                                                       }).ToList()
                                                       .Select(h => new
                                                       {
                                                           ID = h.ID,
                                                           AlterationDate = Utility.GetFormattedDate(h.AlterationDate),
                                                           OutletID = h.OutletID,
                                                           DesignDischarge = h.DesignDischarge,
                                                           OutletType = h.OutletType,
                                                           OutletGCA = h.OutletGCA,
                                                           OutletCCA = h.OutletCCA,
                                                           OutletStatus = GetStatus(h.OutletStatus),
                                                           OutletHeight = h.OutletHeight,
                                                           OutletCrest = h.OutletCrest,
                                                           OutletWidth = h.OutletWidth,
                                                           OutletSubmergence = h.OutletSubmergence,
                                                           OutletCrestRL = h.OutletCrestRL,
                                                           OutletMMH = h.OutletMMH,
                                                           OutletWorkingHead = h.OutletWorkingHead,
                                                           FileName = h.FileName,
                                                           RDandSide = h.OutletRD.HasValue ? Calculations.GetRDText(h.OutletRD.Value) + "/" + h.ChannelSide : "",
                                                           ChannelSide = h.ChannelSide,
                                                           ActionType = h.ActionType

                                                       }).ToList<object>();
            return lstOutletAlterationHistory;
        }
        private string GetStatus(string _OutletStatusID)
        {
            string Name = string.Empty;
            if (!string.IsNullOrEmpty(_OutletStatusID))
            {
                dynamic qStatus = CommonLists.GetStatuses(_OutletStatusID)[0];
                Name = Convert.ToString(qStatus.GetType().GetProperty("Name").GetValue(qStatus, null));
            }
            return Name;
        }
        #endregion

        #region "Outlet Villages"
        /// <summary>
        /// This function return Channel Divisions
        /// Created on: 13-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_Division></returns>
        public List<CO_Division> GetDivisionsByChannelID(long _ChannelID)
        {
            List<CO_Division> lstDivisions = (from i in context.CO_ChannelIrrigationBoundaries
                                              join section in context.CO_Section on i.SectionID equals section.ID
                                              join subDivision in context.CO_SubDivision on section.SubDivID equals subDivision.ID
                                              join division in context.CO_Division on subDivision.DivisionID equals division.ID
                                              where i.ChannelID == _ChannelID
                                              select division).Distinct().ToList();
            return lstDivisions;

        }
        /// <summary>
        /// This function return Channel Districts
        /// Created on: 11-11-2015
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<CO_District> GetDistrictsByChannelID(long _ChannelID)
        {
            List<CO_District> lstDistricts = null;
            List<long> lstDivisionsIDs = GetDivisionsByChannelID(_ChannelID).Select(d => d.ID).ToList<long>();

            if (lstDivisionsIDs != null && lstDivisionsIDs.Count > 0)
            {
                lstDistricts = (from dd in context.CO_DistrictDivision
                                join d in context.CO_District on dd.DistrictID equals d.ID
                                where lstDivisionsIDs.Contains(dd.DivisionID.Value)
                                select d).Distinct().ToList();
            }
            return lstDistricts;

        }
        /// <summary>
        /// This function return Tehsils exists in Districts
        /// Created on: 19-01-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_Tehsil></returns>
        public List<CO_Tehsil> GetTehsilByChannelDistricts(long _ChannelID)
        {
            List<CO_Tehsil> lstTehsils = null;
            List<long> lstDistrictIDs = GetDistrictsByChannelID(_ChannelID).Select(d => d.ID).ToList<long>();

            if (lstDistrictIDs != null && lstDistrictIDs.Count > 0)
            {
                lstTehsils = (from tehsil in context.CO_Tehsil
                              where lstDistrictIDs.Contains(tehsil.DistrictID.Value)
                              select tehsil).ToList();
            }
            return lstTehsils;
        }
        /// <summary>
        /// This function return Villages exists in Tehsils
        /// Created on: 19-01-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<CO_Village></returns>
        public List<CO_Village> GetVillagesByChannelID(long _ChannelID)
        {
            List<CO_Village> lstVillages = null;
            List<long> lstTehsilsIDs = GetTehsilByChannelDistricts(_ChannelID).Select(d => d.ID).ToList<long>();

            if (lstTehsilsIDs != null && lstTehsilsIDs.Count > 0)
            {
                lstVillages = (from village in context.CO_Village
                               where lstTehsilsIDs.Contains(village.TehsilID.Value)
                               select village).ToList();
            }
            return lstVillages;
        }
        /// <summary>
        /// This function return Channel Outlet Locations
        /// Created on: 19-01-2016
        /// </summary>
        /// <param name="_ChannelID"></param>
        /// <returns>List<object></returns>
        public List<object> GetOutletVillagesByOutletID(long _OutletID)
        {
            List<object> lstOutletVillages = (from l in context.CO_ChannelOutletsLocation
                                              join v in context.CO_Village on l.VillageID equals v.ID
                                              where l.OutletID == _OutletID
                                              select new
                                              {
                                                  ID = l.ID,
                                                  OutletID = l.OutletID,
                                                  VillageID = l.VillageID,
                                                  VillageName = v.Name,
                                                  LocatedIn = l.LocatedIn
                                              }).ToList()
                                              .Select(v => new
                                              {
                                                  ID = v.ID,
                                                  OutletID = v.OutletID,
                                                  VillageID = v.VillageID,
                                                  VillageName = v.VillageName,
                                                  LocatedIn = GetYesNoValue(v.LocatedIn)
                                              }).ToList<object>();
            return lstOutletVillages;
        }
        private string GetYesNoValue(bool? _LocatedIn)
        {
            string Name = string.Empty;
            if (_LocatedIn.HasValue)
            {
                dynamic qYesNo = CommonLists.GetYesNo(Convert.ToInt16(_LocatedIn).ToString())[0];
                Name = Convert.ToString(qYesNo.GetType().GetProperty("Name").GetValue(qYesNo, null));
            }
            return Name;
        }
        #endregion
    }
}
