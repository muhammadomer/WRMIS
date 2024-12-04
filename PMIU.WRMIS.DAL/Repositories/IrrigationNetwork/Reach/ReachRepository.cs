using PMIU.WRMIS.Common;
using PMIU.WRMIS.Database;
using PMIU.WRMIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.DAL.Repositories.IrrigationNetwork.Reach
{
    public class ReachRepository : Repository<CO_ChannelReach>
    {
        public ReachRepository(WRMIS_Entities _Context)
            : base(_Context)
        {
            dbSet = _Context.Set<CO_ChannelReach>();
        }

        #region "Define Channel Reaches"
        #endregion

        #region "L Section Parameters History"
        public List<object> GetLSectionParametersHistory(long _ReachID, string L, string _FromDate, string _ToDate)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (!string.IsNullOrEmpty(_FromDate))
                fromDate = Utility.GetParsedDate(_FromDate);
            if (!string.IsNullOrEmpty(_ToDate))
                toDate = Utility.GetParsedDate(_ToDate);

            List<object> lstLSectionParametersHistory = (from l in context.CO_ChannelReachLSP
                                                         join t in context.CO_LiningType on l.LiningTypeID equals t.ID into LT
                                                         from LType in LT.DefaultIfEmpty()
                                                         //where ((l.ReachID == _ReachID) // Changed by Rizwan alvi
                                                         where ((l.ReachID == _ReachID && l.RDLocation.ToLower().Contains(L.ToLower())) 
                                                         && (l.ParameterDate >= (string.IsNullOrEmpty(_FromDate) ? l.ParameterDate : fromDate))
                                                             && (l.ParameterDate <= (string.IsNullOrEmpty(_ToDate) ? l.ParameterDate : toDate)))
                                                         select new
                                                         {
                                                             ID = l.ID,
                                                             ReachID = l.ReachID,
                                                             ParameterDate = l.ParameterDate,
                                                             NaturalSurfaceLevel = l.NaturalSurfaceLevel,
                                                             AFS = l.AFS,
                                                             BedLevel = l.BedLevel,
                                                             FullSupplyLevel = l.FullSupplyLevel,
                                                             BedWidth = l.BedWidth,
                                                             FullSupplyDepth = l.FullSupplyDepth,
                                                             SideSlop = l.SideSlop,
                                                             SlopeIn = l.SlopeIn,
                                                             CriticalVelocityRatio = l.CriticalVelocityRatio,
                                                             MRCoefficient = l.MRCoefficient,
                                                             FreeBoard = l.FreeBoard,
                                                             LeftBankWidth = l.LeftBankWidth,
                                                             RightBankWidth = l.RightBankWidth,
                                                             LinedUnlined = l.LinedUnlined,
                                                             LiningTypeID = l.LiningTypeID,
                                                             LiningType = LType.Name,
                                                             LSectionImage = l.LSectionPhoto
                                                         }).ToList()
                                                       .Select(h => new
                                                       {
                                                           ID = h.ID,
                                                           ReachID = h.ReachID,
                                                           ParameterDate = Utility.GetFormattedDate(h.ParameterDate.Value),
                                                           NaturalSurfaceLevel = h.NaturalSurfaceLevel,
                                                           AFS = h.AFS,
                                                           BedLevel = h.BedLevel,
                                                           FSL = h.FullSupplyLevel,
                                                           BedWidth = h.BedWidth,
                                                           FSDepth = h.FullSupplyDepth,
                                                           SideSlop = h.SideSlop,
                                                           SlopeIn = h.SlopeIn,
                                                           Laceys = h.CriticalVelocityRatio,
                                                           FreeBoard = h.FreeBoard,
                                                           BankWidthLeft = h.LeftBankWidth,
                                                           BankWidthRight = h.RightBankWidth,
                                                           LinedorUnlined = h.LinedUnlined != null ? GetLinedOrUnlined(Convert.ToString(Convert.ToInt16(h.LinedUnlined))) : string.Empty,
                                                           TypeofLining = h.LiningTypeID == null ? 0 : h.LiningTypeID.Value,
                                                           LiningType = h.LiningType == null ? string.Empty : h.LiningType,
                                                           LSectionImage = Utility.IsImageExists(h.LSectionImage),
                                                           MRCoefficient = h.MRCoefficient
                                                       }).ToList<object>();
            return lstLSectionParametersHistory;
        }
        private string GetLinedOrUnlined(string _ID)
        {
            string Name = string.Empty;
            if (!string.IsNullOrEmpty(_ID))
            {
                dynamic qLinedUnlined = CommonLists.GetLinedOrUnlined(_ID)[0];
                Name = Convert.ToString(qLinedUnlined.GetType().GetProperty("Name").GetValue(qLinedUnlined, null));
            }
            return Name;
        }
        #endregion
    }
}
