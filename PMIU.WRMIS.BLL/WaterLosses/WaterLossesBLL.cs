using PMIU.WRMIS.Model;
using PMIU.WRMIS.DAL;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using PMIU.WRMIS.AppBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.DataAccess.WaterLosses;
using System.Data;

namespace PMIU.WRMIS.BLL.WaterLosses
{
    public class WaterLossesBLL : BaseBLL
    {
        #region Lag Timee
        public List<object> GetReachLagTime()
        {
            return new WaterLossesDAL().GetReachLagTime();
        }

        public List<object> GetReachLagTimeUpdateHistoy(long _LagTimeID)
        {
            return new WaterLossesDAL().GetReachLagTimeUpdateHistoy(_LagTimeID);
        }

        public bool UpdateLagTime(long _UserID, long _ID, int _NovApr, int _MayJun, int _JulAug, int _SepOct)
        {
            WaterLossesDAL wlDAL = new WaterLossesDAL();
            bool result = wlDAL.UpdateLagTime(_UserID, _ID, _NovApr, _MayJun, _JulAug, _SepOct);
            return result;
        }
        #endregion

        public List<object> GetCommandList()
        {
            return new WaterLossesDAL().GetCommandList();
        }

        public List<object> GetReachListByCommand(long _CommandID)
        {
            return new WaterLossesDAL().GetReachListByCommand(_CommandID);
        }

        public List<object> GetIndusCommandWLYearListByReach()
        {
            return new WaterLossesDAL().GetIndusCommandWLYearListByReach();
        }

        public List<object> GetJCCommandWLYearListByReach()
        {
            return new WaterLossesDAL().GetJCCommandWLYearListByReach();
        }


        public List<object> GetWaterLosses_Indus(string _ReachName, long _ReachID, int _Year, int _Month)
        {
            if (_ReachName.Contains("Kalabagh-Chashma"))
            {
                return new WaterLossesDAL().GetWaterLosses_Indus_KalabaghChashma(_Year, _Month);
            }
            else if (_ReachName.Contains("Chashma-Taunsa"))
            {
                return new WaterLossesDAL().GetWaterLosses_Indus_ChashmaTaunsa(_Year, _Month);
            }
            else if (_ReachName.Contains("Taunsa-Guddu"))
            {
                return new WaterLossesDAL().GetWaterLosses_Indus_TaunsaGuddu(_Year, _Month);
            }
            else if (_ReachName.Contains("Guddu-Sukkur"))
            {
                return new WaterLossesDAL().GetWaterLosses_Indus_GudduSukkur(_Year, _Month);
            }
            else if (_ReachName.Contains("Sukkur-Kotri"))
            {
                return new WaterLossesDAL().GetWaterLosses_Indus_SukkurKotri(_Year, _Month);
            }
            else if (_ReachName.Contains("Tarbela-Kalabagh"))
            {
                return new WaterLossesDAL().GetWaterLosses_Indus_TarbelaKalabagh(_Year, _Month);
            }
            else if (_ReachName.Contains("Tarbela-Chashma"))
            {
                return new WaterLossesDAL().GetWaterLosses_Indus_TarbelaChashma(_Year, _Month);
            }
            else if (_ReachName.Contains("Chashma-Kotri"))
            {
                return new WaterLossesDAL().GetWaterLosses_Indus_ChashmaKotri(_Year, _Month);
            }
            return new List<object>();

        }

        public List<object> GetWaterLosses_JC(string _ReachName, long _ReachID, int _Year, int _Month)
        {
            if (_ReachName.Contains("Mangla-Marala-Rasul"))
            {
                return new WaterLossesDAL().GetWaterLosses_JC_ManglaMaralaRasul(_Year, _Month);
            }
            else if (_ReachName.Contains("Rasul-Trimmu"))
            {
                return new WaterLossesDAL().GetWaterLosses_JC_RasulTrimmu(_Year, _Month);
            }
            else if (_ReachName.Contains("Trimmu-Panjnad"))
            {
                return new WaterLossesDAL().GetWaterLosses_JC_TrimmuPanjnad(_Year, _Month);
            }
            else if (_ReachName.Contains("Mangla-Panjnad"))
            {
                return new WaterLossesDAL().GetWaterLosses_JC_ManglaPanjnad(_Year, _Month);
            }
            return new List<object>();

        }

        public List<string> GetWaterLossesGains_Indus(string _ReachName, long _ReachID, int _Year, int _Month)
        {
            if (_ReachName.Contains("Kalabagh-Chashma"))
            {
                return new WaterLossesDAL().GetWaterLossesGains_Indus_KalabaghChashma(_Year, _Month);
            }
            else if (_ReachName.Contains("Chashma-Taunsa"))
            {
                return new WaterLossesDAL().GetWaterLossesGains_Indus_ChashmaTaunsa(_Year, _Month);
            }
            else if (_ReachName.Contains("Taunsa-Guddu"))
            {
                return new WaterLossesDAL().GetWaterLossesGains_Indus_TaunsaGuddu(_Year, _Month);
            }
            else if (_ReachName.Contains("Guddu-Sukkur"))
            {
                return new WaterLossesDAL().GetWaterLossesGains_Indus_GudduSukkur(_Year, _Month);
            }
            else if (_ReachName.Contains("Sukkur-Kotri"))
            {
                return new WaterLossesDAL().GetWaterLossesGains_Indus_GudduSukkur(_Year, _Month);
            }
            else if (_ReachName.Contains("Tarbela-Kalabagh"))
            {
                return new WaterLossesDAL().GetWaterLossesGains_Indus_TarbelaKalabagh(_Year, _Month);
            }
            else if (_ReachName.Contains("Tarbela-Chashma"))
            {
                return new WaterLossesDAL().GetWaterLossesGains_Indus_TarbelaChashma(_Year, _Month);
            }
            else if (_ReachName.Contains("Chashma-Kotri"))
            {
                //return new WaterLossesDAL().GetWaterLosses_Indus_ChashmaKotri(_Year, _Month);
            }
            return new List<string>();

        }

        #region Channel Water Losses
        public List<object> GetMainCanals(long _UserID, bool _UserBaseLoading)
        {
            return new WaterLossesDAL().GetMainCanals(_UserID, _UserBaseLoading);
        }

        public List<object> GetCanalsByUserLocation(long _UserID, int _IrrigationLevel)
        {
            return new WaterLossesDAL().GetCanalsByUserLocation(_UserID, _IrrigationLevel);
        }

        public List<object> GetCanalsByParentID(long _UserID, bool _UserBaseLoading, int _IrrigationLevel, long _ParentChannelID, int _ChannelType)
        {
            return new WaterLossesDAL().GetCanalsByParentID(_UserID, _UserBaseLoading, _IrrigationLevel, _ParentChannelID, _ChannelType);
        }


        public List<object> GetCanalsByParentIDForED(long _UserID, bool _UserBaseLoading, int _IrrigationLevel, long _ParentChannelID, int _ChannelType)
        {
            return new WaterLossesDAL().GetCanalsByParentIDForED(_UserID, _UserBaseLoading, _IrrigationLevel, _ParentChannelID, _ChannelType);
        }

        public long GetCommandChannelID(long _ID)
        {
            return new WaterLossesDAL().GetCommandChannelID(_ID);
        }


        public List<object> GetChannelWaterLosses(long _ChannelID, DateTime _FromDate, DateTime _ToDate)
        {
            List<GetWaterLosses_ChannelWise_Result> lst = new WaterLossesDAL().GetChannelWaterLosses(_ChannelID, _FromDate, _ToDate);

            if (lst.Count > 0)
            {
                string channelName = db.Repository<CO_Channel>().Query().Get().Where(x => x.ID == _ChannelID).SingleOrDefault().NAME;

                List<tempClass> lstData = lst.Select(x => new tempClass
                {
                    Channel = "",
                    FromRD = Calculations.GetRDText(x.FromRD),
                    ToRD = Calculations.GetRDText(x.ToRD),
                    FromDschrg = GetRoundOffValue(x.FromDischarge),
                    ToDschrg = GetRoundOffValue(x.ToDischarge),
                    Offtakes = GetRoundOffValue(x.Offtakes),
                    Outlets = GetRoundOffValue(x.Outlets),
                    Diff = GetRoundOffValue(x.Diff),
                    los = GetRoundOffValue(x.Prcntg)
                })
                .ToList<tempClass>();

                //   double? sumToRD = lst.Select(o => o.ToDischarge).Sum();
                double? sumOfftakes = lst.Select(o => o.Offtakes).Sum();
                double? sumOutlets = lst.Select(o => o.Outlets).Sum();
                double? sumDiff = lst.Select(o => o.Diff).Sum();
                double? sumPrcntg = (lst.ElementAt(0).FromDischarge == 0 ? 0 : ((sumDiff / lst.ElementAt(0).FromDischarge) * 100));

                tempClass lastValue = new tempClass();
                lastValue.Channel = "Total";

                lastValue.FromRD = Calculations.GetRDText(lst.ElementAt(0).FromRD);
                lastValue.ToRD = Calculations.GetRDText(lst.OrderBy(x => x.ToRD).ElementAt(lst.Count - 1).ToRD);

                lastValue.FromDschrg = GetRoundOffValue(lst.ElementAt(0).FromDischarge);
                lastValue.ToDschrg = GetRoundOffValue(lst.OrderBy(x => x.ToRD).ElementAt(lst.Count - 1).ToDischarge);

                lastValue.Offtakes = GetRoundOffValue(sumOfftakes);
                lastValue.Outlets = GetRoundOffValue(sumOutlets);

                lastValue.Diff = GetRoundOffValue(sumDiff);
                lastValue.los = GetRoundOffValue(sumPrcntg);

                lstData.Add(lastValue);
                lstData.ElementAt(0).Channel = channelName;

                List<object> dataLst = lstData.Select(x => new
                {
                    Channel = x.Channel,
                    FromRD = x.FromRD,
                    ToRD = x.ToRD,
                    FromDschrg = x.FromDschrg,
                    ToDschrg = x.ToDschrg,
                    Offtakes = x.Offtakes,
                    Outlets = x.Outlets,
                    Diff = x.Diff,
                    los = x.los

                }).ToList<object>();
                return dataLst;
            }
            return null;
        }


        public String GetRoundOffValue(double? value)
        {
            if (value == null)
                return value.ToString();
            else
            {
                if (value == 0)
                    return value.ToString();

                if (value.ToString().Contains("."))
                {
                    return Convert.ToDecimal(value).ToString("###,###,###.#");
                }
                else
                    return Convert.ToDecimal(value).ToString("###,###,###.0");
            }
        }



        public List<object> GetSubDivisonalWLYearList(long _SubDivisionID)
        {
            return new WaterLossesDAL().GetSubDivisonalWLYearList(_SubDivisionID);
        }
        public List<object> GetSubDivisonalWL_RabiYears(long _SubDivisionID)
        {
            return new WaterLossesDAL().GetSubDivisonalWL_RabiYears(_SubDivisionID);
        }

        public List<object> GetDivisonalWLYearList(long _DivisionID)
        {
            return new WaterLossesDAL().GetDivisonalWLYearList(_DivisionID);
        }
        public List<object> GetDivisonalWL_RabiYears(long _DivisionID)
        {
            return new WaterLossesDAL().GetDivisonalWL_RabiYears(_DivisionID);
        }
        public List<object> GetRegionsListByUser(long _UserID, int _BoundryID)
        {
            return new WaterLossesDAL().GetRegionsListByUser(_UserID, _BoundryID);
        }

        public List<WaterLossesDAL.SubDiv_WL> GetSubDivisionalLoss_Daily(long _SubDivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            return new WaterLossesDAL().GetSubDivisionalLoss_Daily(_SubDivisionID, _FromDate, _ToDate);
        }

        public List<WaterLossesDAL.SubDiv_WL> GetSubDivisionalLosses(long _SubDivisionID, int _Year, int _From, int _To)
        {
            return new WaterLossesDAL().GetSubDivisionalLosses(_SubDivisionID, _Year, _From, _To);
        }

        public List<WaterLossesDAL.SubDiv_WL> GetDivisionalLosses(long _DivisionID, int _SearchType, string _FromDate, string _ToDate, int _Year, int _From, int _To)
        {
            return new WaterLossesDAL().GetDivisionalLosses(_DivisionID, _SearchType, _FromDate, _ToDate, _Year, _From, _To);
        }

        public DataTable GetCurrentAndNextDivisionalDischarge(long _DivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            return new WaterLossesDAL().GetCurrentAndNextDivisionalDischarge(_DivisionID, _FromDate, _ToDate);
        }

        public DataTable GetCurrentAndNextSubDivisionalDischarge(long _SubDivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            return new WaterLossesDAL().GetCurrentAndNextSubDivisionalDischarge(_SubDivisionID, _FromDate, _ToDate);
        }

        public class tempClass
        {
            public string Channel; public string FromRD; public string ToRD; public string FromDschrg;
            public string ToDschrg; public string Offtakes; public string Outlets; public string Diff; public string los;
        }
        #endregion

        #region generalMethod
        public List<object> GetAllZones()
        {
            return new WaterLossesDAL().GetAllZones();
        }

        public List<object> GetCirclesByZoneID(long _ZoneID)
        {
            return new WaterLossesDAL().GetCirclesByZoneID(_ZoneID);
        }

        public List<object> GetDivisionsByCircleID(long _CircleID)
        {
            return new WaterLossesDAL().GetDivisionsByCircleID(_CircleID);
        }

        public List<object> GetSubDivByDivisionID(long _DivisionID)
        {
            return new WaterLossesDAL().GetSubDivByDivisionID(_DivisionID);
        }
        #endregion
    }
}
