using PMIU.WRMIS.Common;
using PMIU.WRMIS.DAL.Repositories.WaterLosses;
using PMIU.WRMIS.Model;
using PMIU.WRMIS.AppBlocks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIU.WRMIS.DAL.Repositories.EntitlementDelivery;
using System.Data;

namespace PMIU.WRMIS.DAL.DataAccess.WaterLosses
{
    public class WaterLossesDAL
    {
        ContextDB db = new ContextDB();
        public List<object> GetCommandList()
        {
            var lst = db.Repository<CO_ChannelComndType>().Query().Get().ToList()
                 .Select(x => new
                 {
                     ID = x.ID,
                     Name = x.Name
                 })
                 .ToList<object>();

            return lst;
        }

        public List<object> GetReachLagTime()
        {
            var lst = db.Repository<WL_ReachLag>().Query().Get().Where(x => x.IsActive == true).ToList().OrderBy(x => x.ChannelComndTypeID).ToList()
                 .Select(x => new
                 {
                     ID = x.ID,
                     Command = x.CO_ChannelComndType.Name,
                     Name = x.Name,
                     NOV_APRIL = x.Nov_April,
                     MAY_JUNE = x.May_June,
                     JUL_AUG = x.Jul_Aug,
                     SEP_OCT = x.Sep_Oct
                 })
                 .ToList<object>();

            return lst;
        }

        public List<object> GetReachListByCommand(long _CommandID)
        {
            var lst = db.Repository<WL_ReachLag>().Query().Get()
                .Where(x => x.ChannelComndTypeID == _CommandID && (x.ID != 4 && x.ID != 5 && x.ID != 10 && x.ID != 13)).ToList().OrderBy(x => x.SortOrder).ToList()
                 .Select(x => new
                 {
                     ID = x.ID,
                     Name = x.Name
                 })
                 .ToList<object>();

            return lst;
        }

        public List<object> GetIndusCommandWLYearListByReach()
        {
            var lst = db.Repository<WL_RiverIndus>().Query().Get().Where(x => x.CsbDate != null).ToList()
                 .Select(x => new
                 {
                     ID = x.CsbDate.Value.Year,
                     Name = x.CsbDate.Value.Year
                 })
                 .Distinct()
                 .ToList<object>();

            return lst;
        }

        public List<object> GetJCCommandWLYearListByReach()
        {
            var lst = db.Repository<WL_RiverJhelumChenab>().Query().Get().Where(x => x.TrmDate != null).ToList()
                 .Select(x => new
                 {
                     ID = x.TrmDate.Value.Year,
                     Name = x.TrmDate.Value.Year
                 })
                 .Distinct()
                 .ToList<object>();

            return lst;
        }

        #region Reach Lag Time
        public WL_ReachLag GetReachLagByID(long _LagTimeID)
        {
            return db.Repository<WL_ReachLag>().Query().Get().Where(x => x.ID == _LagTimeID).SingleOrDefault<WL_ReachLag>();
        }

        public bool UpdateLagTime(long _UserID, long _ID, int _NovApr, int _MayJun, int _JulAug, int _SepOct)
        {
            WL_ReachLag mdlLagTime = GetReachLagByID(_ID);

            if (mdlLagTime != null)
            {
                SaveReachLagHistory(mdlLagTime);
                mdlLagTime.Nov_April = _NovApr;
                mdlLagTime.May_June = _MayJun;
                mdlLagTime.Jul_Aug = _JulAug;
                mdlLagTime.Sep_Oct = _SepOct;
                mdlLagTime.ModifiedBy = (int)_UserID;
                mdlLagTime.ModifiedDate = DateTime.Now;

                db.Repository<WL_ReachLag>().Update(mdlLagTime);
                db.Save();
            }
            return true;
        }

        public void SaveReachLagHistory(WL_ReachLag _ReachLag)
        {
            WL_ReachLagHistory mdlLagHstry = new WL_ReachLagHistory();
            mdlLagHstry.ReachLagID = _ReachLag.ID;
            mdlLagHstry.ChannelComndTypeID = _ReachLag.ChannelComndTypeID;
            mdlLagHstry.FromStructureTypeID = _ReachLag.FromStructureTypeID;
            mdlLagHstry.FromStationID = _ReachLag.FromStationID;
            mdlLagHstry.ToStructureTypeID = _ReachLag.ToStructureTypeID;
            mdlLagHstry.ToStationID = _ReachLag.ToStationID;
            mdlLagHstry.Name = _ReachLag.Name;
            mdlLagHstry.Nov_April = _ReachLag.Nov_April;
            mdlLagHstry.May_June = _ReachLag.May_June;
            mdlLagHstry.Jul_Aug = _ReachLag.Jul_Aug;
            mdlLagHstry.Sep_Oct = _ReachLag.Sep_Oct;
            mdlLagHstry.CreatedBy = _ReachLag.CreatedBy;
            mdlLagHstry.CreatedDate = _ReachLag.CreatedDate;
            mdlLagHstry.ModifiedBy = _ReachLag.ModifiedBy;
            mdlLagHstry.ModifiedDate = _ReachLag.ModifiedDate;

            db.Repository<WL_ReachLagHistory>().Insert(mdlLagHstry);
            db.Save();
        }

        public List<object> GetReachLagTimeUpdateHistoy(long _LagTimeID)
        {
            int index = 1;
            var lst = db.Repository<WL_ReachLagHistory>().Query().Get().Where(x => x.ReachLagID == _LagTimeID).OrderByDescending(x => x.ModifiedDate).ToList()
                 .Select(x => new
                 {
                     SrNo = index++,
                     ModifiedBy = GetUserNameByID(x.ModifiedBy),
                     ModifiedDate = Utility.GetFormattedDateTime(x.ModifiedDate.Value).ToString(),
                     NOV_APRIL = x.Nov_April,
                     MAY_JUNE = x.May_June,
                     JUL_AUG = x.Jul_Aug,
                     SEP_OCT = x.Sep_Oct
                 })
                 .ToList<object>();

            return lst;
        }

        #endregion

        #region Water Losses Tabulr Data
        public List<object> GetWaterLosses_Indus_KalabaghChashma(int _Year, int _Month)
        {

            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.KalabaghDate.Value.Year == _Year && x.KalabaghDate.Value.Month == _Month)
                .Select(x => new { x.KalabaghDate, x.KalabaghUS, x.KalabaghDS, x.Thal, x.CsbDate, x.CsbMeanInflow, x.CsbLG })
                .OrderBy(x => x.KalabaghDate)
                .ToList()
                .Select(x => new
                {
                    KalabaghDate = x.KalabaghDate.Value.ToString("dd-MMM"),
                    KalabaghUS = Utility.GetRoundOffValue(x.KalabaghUS),
                    KalabaghDS = Utility.GetRoundOffValue(x.KalabaghDS),
                    Thal = Utility.GetRoundOffValue(x.Thal),
                    chashmaDate = x.CsbDate.Value.ToString("dd-MMM"),
                    CsbMeanInflow = Utility.GetRoundOffValue(x.CsbMeanInflow),
                    CsbLG = Utility.GetRoundOffValue(x.CsbLG)
                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_Indus_ChashmaTaunsa(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.CsbDate.Value.Year == _Year && x.CsbDate.Value.Month == _Month)
                .Select(x => new
                {
                    x.CsbDate,
                    x.CsbMeanOutflow,
                    x.TsbDate,
                    x.TsbUS,
                    x.TsbDS,
                    x.TsbLG
                })
                .ToList().OrderBy(x => x.CsbDate)
                .Select(x => new
                {
                    CsbDate = x.CsbDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.CsbMeanOutflow),
                    TsbDate = x.TsbDate.Value.ToString("dd-MMM"),
                    b = Utility.GetRoundOffValue(x.TsbUS),
                    c = Utility.GetRoundOffValue(x.TsbDS),
                    d = Utility.GetRoundOffValue(x.TsbLG)
                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_Indus_TaunsaGuddu(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.TsbDate.Value.Year == _Year && x.TsbDate.Value.Month == _Month)
                 .Select(x => new
                 {
                     x.TsbDate,
                     x.TsbUS,
                     x.TsbDS,
                     x.PnjDate,
                     x.PnjDS,
                     x.PnjTsbTotal,
                     x.GudduDate,
                     x.GudduUS,
                     x.GudduDS,
                     x.GudduLG,
                     x.CsbDate
                 })
                .ToList().OrderBy(x => x.TsbDate)
                .Select(x => new
                {
                    TsbDate = x.TsbDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.TsbUS),
                    b = Utility.GetRoundOffValue(x.TsbDS),
                    PnjDate = x.PnjDate.Value.ToString("dd-MMM"),
                    c = Utility.GetRoundOffValue(x.PnjDS),
                    d = Utility.GetRoundOffValue(x.PnjTsbTotal),
                    h = x.GudduDate.Value.ToString("dd-MMM"),
                    e = Utility.GetRoundOffValue(x.GudduUS),
                    f = Utility.GetRoundOffValue(x.GudduDS),
                    g = Utility.GetRoundOffValue(x.GudduLG)
                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_Indus_GudduSukkur(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.GudduDate.Value.Year == _Year && x.GudduDate.Value.Month == _Month)
                .Select(x => new
                {
                    x.GudduDate,
                    x.GudduUS,
                    x.GudduDS,
                    x.SkbDate,
                    x.SkbUS,
                    x.SkbDS,
                    x.SkbLG,
                    x.CsbDate
                })
                .ToList().OrderBy(x => x.GudduDate)
                .Select(x => new
                {
                    GudduDate = x.GudduDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.GudduUS),
                    b = Utility.GetRoundOffValue(x.GudduDS),
                    SukkurDate = x.SkbDate.Value.ToString("dd-MMM"),
                    c = Utility.GetRoundOffValue(x.SkbUS),
                    d = Utility.GetRoundOffValue(x.SkbDS),
                    e = Utility.GetRoundOffValue(x.SkbLG)
                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_Indus_SukkurKotri(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.SkbDate.Value.Year == _Year && x.SkbDate.Value.Month == _Month)
                .Select(x => new
                {
                    x.SkbDate,
                    x.SkbUS,
                    x.SkbDS,
                    x.KtbDate,
                    x.KtbUS,
                    x.KtbLG,
                    x.CsbDate
                })
                .ToList()
                .OrderBy(x => x.SkbDate)
                .Select(x => new
                {
                    SkbDate = x.SkbDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.SkbUS),
                    b = Utility.GetRoundOffValue(x.SkbDS),
                    KtbDate = x.KtbDate.Value.ToString("dd-MMM"),
                    c = Utility.GetRoundOffValue(x.KtbUS),
                    d = Utility.GetRoundOffValue(x.KtbLG)
                })
                .ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_Indus_TarbelaKalabagh(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.TbdDate.Value.Year == _Year && x.TbdDate.Value.Month == _Month)
                .Select(x => new
                {
                    x.TbdDate,
                    x.TbdMeanInflow,
                    x.TbdMeanOutflow,
                    x.TbdKabul,
                    x.TbdTotal,
                    x.KalabaghDate,
                    x.KalabaghUS,
                    x.KalabaghDS,
                    x.KalabaghLG,
                    x.CsbDate
                })
                .ToList()
                .OrderBy(x => x.TbdDate)
                .Select(x => new
                {
                    TbdDate = x.TbdDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.TbdMeanInflow),
                    b = Utility.GetRoundOffValue(x.TbdMeanOutflow),
                    c = Utility.GetRoundOffValue(x.TbdKabul),
                    d = Utility.GetRoundOffValue(x.TbdTotal),
                    KalabaghDat = x.KalabaghDate.Value.ToString("dd-MMM"),
                    e = Utility.GetRoundOffValue(x.KalabaghUS),
                    f = Utility.GetRoundOffValue(x.KalabaghDS),
                    g = Utility.GetRoundOffValue(x.KalabaghLG)
                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_Indus_TarbelaChashma(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.TbdDate.Value.Year == _Year && x.TbdDate.Value.Month == _Month)
                .ToList()
                .OrderBy(x => x.TbdDate)
                .Select(x => new
                {
                    TbdDate = x.TbdDate.Value.ToString("dd-MMM"),
                    q = Utility.GetRoundOffValue(x.TbdResLevel),
                    w = Utility.GetRoundOffValue(x.TbdMeanInflow),
                    e = Utility.GetRoundOffValue(x.TbdMeanOutflow),
                    r = Utility.GetRoundOffValue(x.TbdKabul),
                    t = Utility.GetRoundOffValue(x.TbdTotal),
                    KalabaghDate = x.KalabaghDate.Value.ToString("dd-MMM"),
                    y = Utility.GetRoundOffValue(x.KalabaghUS),
                    u = Utility.GetRoundOffValue(x.KalabaghDS),
                    i = Utility.GetRoundOffValue(x.Thal),
                    o = Utility.GetRoundOffValue(x.KalabaghLG),
                    p = Utility.GetRoundOffValue(x.TbdBalanceCsb),
                    CsbDate = x.CsbDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.CsbLG),
                    s = Utility.GetRoundOffValue(x.CsbResLevel),
                    d = Utility.GetRoundOffValue(x.CsbBalance),
                    f = Utility.GetRoundOffValue(x.CsbMeanInflow),
                    g = Utility.GetRoundOffValue(x.CsbCRBC),
                    h = Utility.GetRoundOffValue(x.CsbCJLink),
                    j = Utility.GetRoundOffValue(x.CsbDS),
                    k = Utility.GetRoundOffValue(x.CsbTotal)
                }).ToList<object>();
            return lst;
        }

        public List<object> GetWaterLosses_Indus_ChashmaKotri(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.CsbDate.Value.Year == _Year && x.CsbDate.Value.Month == _Month)
                .ToList()
                .OrderBy(x => x.CsbDate)
                .Select(x => new
                {
                    csbDate = x.CsbDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.CsbMeanOutflow),
                    TsbDate = x.TsbDate.Value.ToString("dd-MMM"),
                    s = Utility.GetRoundOffValue(x.TsbUS),
                    d = Utility.GetRoundOffValue(x.TsbDS),
                    f = Utility.GetRoundOffValue(x.TsbLG),
                    pnjDate = x.PnjDate.Value.ToString("dd-MMM"),
                    g = Utility.GetRoundOffValue(x.PnjDS),
                    h = Utility.GetRoundOffValue(x.PnjTsbTotal),
                    GudduDate = x.GudduDate.Value.ToString("dd-MMM"),
                    j = Utility.GetRoundOffValue(x.GudduUS),
                    k = Utility.GetRoundOffValue(x.GudduDS),
                    l = Utility.GetRoundOffValue(x.GudduLG),
                    skbData = x.SkbDate.Value.ToString("dd-MMM"),
                    z = Utility.GetRoundOffValue(x.SkbUS),
                    x = Utility.GetRoundOffValue(x.SkbDS),
                    c = Utility.GetRoundOffValue(x.SkbLG),
                    KtbDate = x.KtbDate.Value.ToString("dd-MMM"),
                    v = Utility.GetRoundOffValue(x.KtbUS),
                    b = Utility.GetRoundOffValue(x.KtbLG),
                    n = Utility.GetRoundOffValue(x.TbdCsb),
                    m = Utility.GetRoundOffValue(x.CsbKtb),
                    q = Utility.GetRoundOffValue(x.TbdKtb)

                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_JC_ManglaMaralaRasul(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverJhelumChenab>().Query().Get()
                .Where(x => x.MglDate.Value.Year == _Year && x.MglDate.Value.Month == _Month)
                .Select(x => new
                {
                    x.MglDate,
                    x.MglResLevel,
                    x.MglMeanInflow,
                    x.MglMeanOutflow,
                    x.MglUJC,
                    x.RslLG,
                    x.MaralaUS,
                    x.MaralaDS,
                    x.RslDate,
                    x.RslUS,
                    x.RslDS,
                    x.QbdBelow,
                    x.TrmDate
                })
                .ToList()
                .OrderBy(x => x.MglDate)
                .Select(x => new
                {
                    MglDate = x.MglDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.MglResLevel),
                    b = Utility.GetRoundOffValue(x.MglMeanInflow),
                    c = Utility.GetRoundOffValue(x.MglMeanOutflow),
                    d = Utility.GetRoundOffValue(x.MglUJC),
                    e = Utility.GetRoundOffValue(x.RslLG),
                    f = Utility.GetRoundOffValue(x.MaralaUS),
                    g = Utility.GetRoundOffValue(x.MaralaDS),
                    RslDate = x.RslDate.Value.ToString("dd-MMM"),
                    h = Utility.GetRoundOffValue(x.RslUS),
                    i = Utility.GetRoundOffValue(x.RslDS),
                    j = Utility.GetRoundOffValue(x.QbdBelow)
                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_JC_RasulTrimmu(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverJhelumChenab>().Query().Get()
                .Where(x => x.RslDate.Value.Year == _Year && x.RslDate.Value.Month == _Month)
                .Select(x => new
                {
                    x.RslDate,
                    x.RslUS,
                    x.RslDS,
                    x.QbdBelow,
                    x.CJHeadDate,
                    x.CJHead,
                    x.CJTotal,
                    x.TrmDate,
                    x.TrmUS,
                    x.TrmDS,
                    x.TrmLG
                })
                .ToList()
                .OrderBy(x => x.RslDate)
                .Select(x => new
                {
                    RslDate = x.RslDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.RslUS),
                    b = Utility.GetRoundOffValue(x.RslDS),
                    c = Utility.GetRoundOffValue(x.QbdBelow),
                    CJHeadDate = x.CJHeadDate.Value.ToString("dd-MMM"),
                    d = Utility.GetRoundOffValue(x.CJHead),
                    f = Utility.GetRoundOffValue(x.CJTotal),
                    TrmDate = x.TrmDate.Value.ToString("dd-MMM"),
                    g = Utility.GetRoundOffValue(x.TrmUS),
                    h = Utility.GetRoundOffValue(x.TrmDS),
                    i = Utility.GetRoundOffValue(x.TrmLG)
                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_JC_TrimmuPanjnad(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverJhelumChenab>().Query().Get()
                .Where(x => x.TrmDate.Value.Year == _Year && x.TrmDate.Value.Month == _Month)
                .Select(x => new
                {
                    x.TrmDate,
                    x.TrmUS,
                    x.TrmDS,
                    x.IslamDS,
                    x.SidhnaiDS,
                    x.TPHeadDate,
                    x.TPHead,
                    x.TPHeadTotal,
                    x.PnjDate,
                    x.PnjUS,
                    x.PnjLG
                })
                .ToList()
                .OrderBy(x => x.TrmDate)
                .Select(x => new
                {
                    TrmDate = x.TrmDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.TrmUS),
                    b = Utility.GetRoundOffValue(x.TrmDS),
                    c = Utility.GetRoundOffValue(x.IslamDS),
                    d = Utility.GetRoundOffValue(x.SidhnaiDS),
                    TPHeadDate = x.TPHeadDate.Value.ToString("dd-MMM"),
                    e = Utility.GetRoundOffValue(x.TPHead),
                    f = Utility.GetRoundOffValue(x.TPHeadTotal),
                    PnjDate = x.PnjDate.Value.ToString("dd-MMM"),
                    g = Utility.GetRoundOffValue(x.PnjUS),
                    h = Utility.GetRoundOffValue(x.PnjLG)
                }).ToList<object>();

            return lst;
        }

        public List<object> GetWaterLosses_JC_ManglaPanjnad(int _Year, int _Month)
        {
            List<object> lst = new List<object>();
            lst = db.Repository<WL_RiverJhelumChenab>().Query().Get()
                .Where(x => x.MglDate.Value.Year == _Year && x.MglDate.Value.Month == _Month)
                .ToList()
                .OrderBy(x => x.MglDate)
                .Select(x => new
                {
                    MglDate = x.MglDate.Value.ToString("dd-MMM"),
                    a = Utility.GetRoundOffValue(x.MglResLevel),
                    b = Utility.GetRoundOffValue(x.MglMeanInflow),
                    c = Utility.GetRoundOffValue(x.MglMeanOutflow),
                    d = Utility.GetRoundOffValue(x.MglUJC),
                    e = Utility.GetRoundOffValue(x.RslLG),
                    f = Utility.GetRoundOffValue(x.MaralaUS),
                    g = Utility.GetRoundOffValue(x.MaralaDS),
                    RslDate = x.RslDate.Value.ToString("dd-MMM"),
                    h = Utility.GetRoundOffValue(x.RslUS),
                    i = Utility.GetRoundOffValue(x.RslDS),
                    j = Utility.GetRoundOffValue(x.QbdBelow),
                    CJHeadDate = x.CJHeadDate.Value.ToString("dd-MMM"),
                    k = Utility.GetRoundOffValue(x.CJHead),
                    l = Utility.GetRoundOffValue(x.CJTotal),
                    TrmDate = x.TrmDate.Value.ToString("dd-MMM"),
                    m = Utility.GetRoundOffValue(x.TrmUS),
                    n = Utility.GetRoundOffValue(x.TrmDS),
                    o = Utility.GetRoundOffValue(x.TrmLG),
                    p = Utility.GetRoundOffValue(x.IslamDS),
                    q = Utility.GetRoundOffValue(x.SidhnaiDS),
                    TPHeadDate = x.TPHeadDate.Value.ToString("dd-MMM"),
                    r = Utility.GetRoundOffValue(x.TPHead),
                    s = Utility.GetRoundOffValue(x.TPHeadTotal),
                    PnjDate = x.PnjDate.Value.ToString("dd-MMM"),
                    t = Utility.GetRoundOffValue(x.PnjUS),
                    u = Utility.GetRoundOffValue(x.PnjLG),
                    v = Utility.GetRoundOffValue(x.PnjTotal)
                }).ToList<object>();

            return lst;
        }

        #endregion

        #region Water Losses Graph View Data
        public List<string> GetWaterLossesGains_Indus_TarbelaKalabagh(int _Year, int _Month)
        {
            var lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.KalabaghDate.Value.Year == _Year && x.KalabaghDate.Value.Month == _Month)
                .Select(x => new { x.KalabaghDate, x.KalabaghLG }).OrderBy(x => x.KalabaghDate)
                .ToList()
                .Select(x => new { data = (string)x.KalabaghDate.Value.ToString("dd-MMM") + ":" + x.KalabaghLG })
                .ToList()
                .Select(x => x.data)
                .ToList<string>();

            return lst;
        }

        public List<string> GetWaterLossesGains_Indus_KalabaghChashma(int _Year, int _Month)
        {
            var lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.CsbDate.Value.Year == _Year && x.CsbDate.Value.Month == _Month)
                .Select(x => new { x.CsbDate, x.CsbLG }).OrderBy(x => x.CsbDate)
                .ToList()
                .Select(x => new { data = (string)x.CsbDate.Value.ToString("dd-MMM") + ":" + x.CsbLG })
                .ToList()
                .Select(x => x.data)
                .ToList<string>();

            return lst;
        }

        public List<string> GetWaterLossesGains_Indus_ChashmaTaunsa(int _Year, int _Month)
        {
            var lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.TsbDate.Value.Year == _Year && x.TsbDate.Value.Month == _Month)
                .Select(x => new { x.TsbDate, x.TsbLG }).OrderBy(x => x.TsbDate)
                .ToList()
                .Select(x => new { data = (string)x.TsbDate.Value.ToString("dd-MMM") + ":" + x.TsbLG })
                .ToList()
                .Select(x => x.data)
                .ToList<string>();

            return lst;
        }

        public List<string> GetWaterLossesGains_Indus_TaunsaGuddu(int _Year, int _Month)
        {
            var lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.GudduDate.Value.Year == _Year && x.GudduDate.Value.Month == _Month)
                .Select(x => new { x.GudduDate, x.GudduLG }).OrderBy(x => x.GudduDate)
                .ToList()
                .Select(x => new { data = (string)x.GudduDate.Value.ToString("dd-MMM") + ":" + x.GudduLG })
                .ToList()
                .Select(x => x.data)
                .ToList<string>();

            return lst;
        }

        public List<string> GetWaterLossesGains_Indus_GudduSukkur(int _Year, int _Month)
        {
            var lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.GudduDate.Value.Year == _Year && x.GudduDate.Value.Month == _Month)
                .Select(x => new { x.GudduDate, x.GudduLG }).OrderBy(x => x.GudduDate)
                .ToList()
                .Select(x => new { data = (string)x.GudduDate.Value.ToString("dd-MMM") + ":" + x.GudduLG })
                .ToList()
                .Select(x => x.data)
                .ToList<string>();

            return lst;
        }

        public List<string> GetWaterLossesGains_Indus_SukkurKotri(int _Year, int _Month)
        {
            var lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.GudduDate.Value.Year == _Year && x.GudduDate.Value.Month == _Month)
                .Select(x => new { x.KtbDate, x.KtbLG }).OrderBy(x => x.KtbDate)
                .ToList()
                .Select(x => new { data = (string)x.KtbDate.Value.ToString("dd-MMM") + ":" + x.KtbLG })
                .ToList()
                .Select(x => x.data)
                .ToList<string>();

            return lst;
        }

        public List<string> GetWaterLossesGains_Indus_TarbelaChashma(int _Year, int _Month)
        {
            var lst = db.Repository<WL_RiverIndus>().Query().Get()
                .Where(x => x.KalabaghDate.Value.Year == _Year && x.KalabaghDate.Value.Month == _Month)
                .Select(x => new { x.KalabaghDate, x.KalabaghLG }).OrderBy(x => x.KalabaghDate)
                .ToList()
                .Select(x => new { data = (string)x.KalabaghDate.Value.ToString("dd-MMM") + ":" + x.KalabaghLG })
                .ToList()
                .Select(x => x.data)
                .ToList<string>();

            return lst;
        }

        #endregion

        #region Channel Water Losses

        public List<object> GetCanalsByUserLocation(long _UserID, int _IrrigationLevel)
        {
            List<object> lstCanals = new List<object>();

            List<long?> lstSections = GetSectionListByUser(_UserID, _IrrigationLevel);

            List<long?> lstChnlID = db.Repository<CO_ChannelIrrigationBoundaries>().Query().Get()
                .Where(x => lstSections.Contains(x.SectionID)).Select(x => x.ChannelID).ToList<long?>();

            List<innerClass> lstAllCanals = db.Repository<CO_Channel>().Query().Get().Where(x => lstChnlID.Contains(x.ID))
                    .Select(x => new { x.ID, x.NAME, x.ChannelTypeID }).ToList().Select(x => new innerClass { ID = x.ID, Name = x.NAME, Type = x.ChannelTypeID }).OrderBy(x => x.Name).ToList<innerClass>();

            List<object> lstMainCanal = (from c in lstAllCanals where c.Type == 1 select new { c.ID, c.Name }).ToList<object>();
            lstCanals.Add(lstMainCanal);
            List<object> lstBranchCanal = (from c in lstAllCanals where c.Type == 3 select new { c.ID, c.Name }).ToList<object>();
            lstCanals.Add(lstBranchCanal);
            List<object> lstDstrbtry = (from c in lstAllCanals where c.Type == 4 select new { c.ID, c.Name }).ToList<object>();
            lstCanals.Add(lstDstrbtry);
            List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
            lstCanals.Add(lstMinor);
            List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();
            lstCanals.Add(lstSubMinor);

            return lstCanals;
        }

        public List<object> GetMainCanals(long _UserID, bool _UserBaseLoading)
        {
            List<object> lstMainCanals = new List<object>();

            if (_UserBaseLoading)
            {
                //load user based canals
            }
            else
            {
                lstMainCanals = db.Repository<CO_Channel>().Query().Get().Where(x => x.ChannelTypeID == 1)
                     .Select(x => new { x.ID, x.NAME }).ToList().Select(x => new { ID = x.ID, Name = x.NAME }).OrderBy(x => x.Name).ToList<object>();
            }

            return lstMainCanals;
        }

        public List<object> GetCanalsByParentID(long _UserID, bool _UserBaseLoading, int _IrrigationLevel, long _ParentChannelID, int _ChannelType)
        {
            List<object> lstCanals = new List<object>();
            List<innerClass> lstAllCanals = new List<innerClass>();
            List<long?> lstChillChnlID = new List<long?>();

            if (_UserBaseLoading)
            {
                List<long?> lstSections = GetSectionListByUser(_UserID, _IrrigationLevel);

                List<long?> lstChnlID = db.Repository<CO_ChannelIrrigationBoundaries>().Query().Get()
                    .Where(x => lstSections.Contains(x.SectionID)).Select(x => x.ChannelID).ToList<long?>();

                lstChillChnlID = db.Repository<CO_ChannelParentFeeder>().Query().Get()
                    .Where(x => x.ParrentFeederID == _ParentChannelID && lstChnlID.Contains(x.ChannelID)).Select(x => x.ChannelID).ToList<long?>();
            }
            else
            {
                lstChillChnlID = db.Repository<CO_ChannelParentFeeder>().Query().Get()
                    .Where(x => x.ParrentFeederID == _ParentChannelID && x.RelationType == "P" && x.StructureTypeID == (long)Constants.StructureType.Channel).Select(x => x.ChannelID).ToList<long?>();

            }

            lstAllCanals = db.Repository<CO_Channel>().Query().Get().Where(x => lstChillChnlID.Contains(x.ID))
                    .Select(x => new { x.ID, x.NAME, x.ChannelTypeID }).ToList().Select(x => new innerClass { ID = x.ID, Name = x.NAME, Type = x.ChannelTypeID }).OrderBy(x => x.Name).ToList<innerClass>();

            if (_ChannelType == 0)//Canal System
            {
                List<object> lstMainCanal = (from c in lstAllCanals where c.Type == 1 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstBranchCanal = (from c in lstAllCanals where c.Type == 3 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstDstrbtry = (from c in lstAllCanals where c.Type == 4 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstMainCanal);
                lstCanals.Add(lstBranchCanal);
                lstCanals.Add(lstDstrbtry);
                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else if (_ChannelType == 1)//MainCanal
            {
                List<object> lstBranchCanal = (from c in lstAllCanals where c.Type == 3 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstDstrbtry = (from c in lstAllCanals where c.Type == 4 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstBranchCanal);
                lstCanals.Add(lstDstrbtry);
                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else if (_ChannelType == 3) //BranchCanal
            {
                List<object> lstDstrbtry = (from c in lstAllCanals where c.Type == 4 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstDstrbtry);
                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else if (_ChannelType == 4) //Distributory Canal
            {
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else if (_ChannelType == 5) //Minor Canal
            {
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();
                lstCanals.Add(lstSubMinor);
            }

            return lstCanals;
        }

        public List<GetWaterLosses_ChannelWise_Result> GetChannelWaterLosses(long _ChannelID, DateTime _FromDate, DateTime _ToDate)
        {
            WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();
            List<GetWaterLosses_ChannelWise_Result> lstData = new List<GetWaterLosses_ChannelWise_Result>();
            lstData = repWaterLosses.GetChannelWaterLosses(_ChannelID, _FromDate, _ToDate);
            return lstData;
        }
        public class innerClass { public long ID; public string Name; public long Type; public innerClass() { } }

        public List<object> GetSubDivisonalWLYearList(long _SubDivisionID)
        {
            var lst = db.Repository<WL_SubDivisionLG>().Query().Get()
                .Where(x => x.LGDate != null && x.SubDivID == _SubDivisionID)
                .ToList()
                .Select(x => new
                {
                    ID = x.LGDate.Value.Year,
                    Name = x.LGDate.Value.Year
                })
                .Distinct()
                .ToList<object>();

            return lst;
        }

        public List<object> GetSubDivisonalWL_RabiYears(long _SubDivisionID)
        {
            var lst = db.Repository<WL_SubDivisionLG>().Query().Get()
                .Where(x => x.LGDate != null && x.SubDivID == _SubDivisionID)
                .ToList()
                .Select(x => new
                {
                    ID = x.LGDate.Value.Year,
                    Name = x.LGDate.Value.Year
                })
                .Distinct()
                .ToList()
                .Select(x => new
                {
                    ID = x.ID + "-" + (x.ID + 1),
                    Name = x.Name + "-" + ((x.Name + 1).ToString()).Substring(2, 2)
                })
                .ToList<object>();

            return lst;
        }

        public List<object> GetDivisonalWLYearList(long _DivisionID)
        {
            List<long> lstSubDivID = db.Repository<CO_SubDivision>().Query().Get().Where(x => x.DivisionID == _DivisionID).Select(x => x.ID).Distinct().ToList();
            List<long?> temp = new List<long?>();
            foreach (var a in lstSubDivID)
                temp.Add(a);

            var lst = db.Repository<WL_SubDivisionLG>().Query().Get()
                .Where(x => x.LGDate != null && temp.Contains(x.SubDivID))
                .ToList()
                .Select(x => new
                {
                    ID = x.LGDate.Value.Year,
                    Name = x.LGDate.Value.Year
                })
                .Distinct()
                .ToList<object>();

            return lst;
        }

        public List<object> GetDivisonalWL_RabiYears(long _DivisionID)
        {
            List<long> lstSubDivID = db.Repository<CO_SubDivision>().Query().Get().Where(x => x.DivisionID == _DivisionID).Select(x => x.ID).Distinct().ToList();
            List<long?> temp = new List<long?>();
            foreach (var a in lstSubDivID)
                temp.Add(a);

            var lst = db.Repository<WL_SubDivisionLG>().Query().Get()
                .Where(x => x.LGDate != null && temp.Contains(x.SubDivID))
                .ToList()
                .Select(x => new
                {
                    ID = x.LGDate.Value.Year,
                    Name = x.LGDate.Value.Year
                })
                .Distinct()
                .ToList()
                .Select(x => new
                {
                    ID = x.ID + "-" + (x.ID + 1),
                    Name = x.Name + "-" + ((x.Name + 1).ToString()).Substring(2, 2)
                })
                .ToList<object>();

            return lst;
        }
        public List<object> GetRegionsListByUser(long _UserID, int _BoundryID)
        {
            List<object> lstRegion = new List<object>();
            List<long?> lstIDs = new List<long?>();
            List<long?> lstIDsTemp = new List<long?>();
            if (_BoundryID == 4) // SDO
            {
                lstIDs = db.Repository<UA_AssociatedLocation>().Query().Get()
                    .Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();
                var subDiv = db.Repository<CO_SubDivision>().Query().Get()
                    .Where(x => lstIDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                lstRegion.Add(subDiv);//SubDivision 


                lstIDsTemp = db.Repository<CO_SubDivision>().Query().Get()
                    .Where(x => lstIDs.Contains(x.ID)).Select(x => x.DivisionID).ToList().Distinct().ToList<long?>();
                var div = db.Repository<CO_Division>().Query().Get()
                    .Where(x => lstIDsTemp.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();
                lstRegion.Add(div);// Division 

                lstIDs.Clear();

                lstIDs = db.Repository<CO_Division>().Query().Get()
                    .Where(x => lstIDsTemp.Contains(x.ID)).Select(x => x.CircleID).ToList().Distinct().ToList<long?>();
                var circle = db.Repository<CO_Circle>().Query().Get()
                    .Where(x => lstIDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();
                lstRegion.Add(circle);// Circle 

                lstIDsTemp.Clear();

                List<long> lst_IDs = db.Repository<CO_Circle>().Query().Get()
                    .Where(x => lstIDs.Contains(x.ID)).Select(x => x.ZoneID).ToList().Distinct().ToList<long>();
                var zone = db.Repository<CO_Zone>().Query().Get()
                    .Where(x => lst_IDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();
                lstRegion.Add(zone);// Zone 

            }
            else if (_BoundryID == 3) //XEN
            {
                lstRegion.Clear();

                var dIDs = db.Repository<UA_AssociatedLocation>().Query().Get()
                                   .Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();
                var div = db.Repository<CO_Division>().Query().Get()
                    .Where(x => dIDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();


                var subDiv = db.Repository<CO_SubDivision>().Query().Get()
                    .Where(x => dIDs.Contains(x.DivisionID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                lstRegion.Add(subDiv);
                lstRegion.Add(div);

                var circleIDs = db.Repository<CO_Division>().Query().Get()
                    .Where(x => dIDs.Contains(x.ID)).Select(x => x.CircleID).ToList<long?>();
                var circls = db.Repository<CO_Circle>().Query().Get()
                    .Where(x => circleIDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();
                lstRegion.Add(circls);

                var zoneIDs = db.Repository<CO_Circle>().Query().Get()
                    .Where(x => circleIDs.Contains(x.ID)).Select(x => x.ZoneID).ToList<long>();
                var zone = db.Repository<CO_Zone>().Query().Get()
                    .Where(x => zoneIDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();
                lstRegion.Add(zone);
            }
            else if (_BoundryID == 2) //SE
            {
                lstRegion.Clear();

                var cIDs = db.Repository<UA_AssociatedLocation>().Query().Get()
                                   .Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();
                var circls = db.Repository<CO_Circle>().Query().Get()
                    .Where(x => cIDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                var div = db.Repository<CO_Division>().Query().Get()
                    .Where(x => cIDs.Contains(x.CircleID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                var dIDs = db.Repository<CO_Division>().Query().Get()
                    .Where(x => cIDs.Contains(x.CircleID)).Select(x => x.ID).ToList<long>();

                var dID = new List<long?>();
                foreach (var id in dIDs)
                    dID.Add(id);

                var subDiv = db.Repository<CO_SubDivision>().Query().Get()
                    .Where(x => dID.Contains(x.DivisionID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                lstRegion.Add(subDiv);
                lstRegion.Add(div);
                lstRegion.Add(circls);

                var zoneIDs = db.Repository<CO_Circle>().Query().Get()
                    .Where(x => cIDs.Contains(x.ID)).Select(x => x.ZoneID).ToList<long>();

                var zone = db.Repository<CO_Zone>().Query().Get()
                    .Where(x => zoneIDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();
                lstRegion.Add(zone);
            }
            else if (_BoundryID == 1) //CE
            {
                lstRegion.Clear();

                var zIDs = db.Repository<UA_AssociatedLocation>().Query().Get()
                                   .Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();
                var zone = db.Repository<CO_Zone>().Query().Get()
                  .Where(x => zIDs.Contains(x.ID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                var circls = db.Repository<CO_Circle>().Query().Get()
                    .Where(x => zIDs.Contains(x.ZoneID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                var cID = db.Repository<CO_Circle>().Query().Get()
                    .Where(x => zIDs.Contains(x.ZoneID)).Select(x => x.ID).ToList<long>();
                var cIDs = new List<long?>();
                foreach (var c in cID)
                    cIDs.Add(c);
                var div = db.Repository<CO_Division>().Query().Get()
                    .Where(x => cIDs.Contains(x.CircleID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                var dIDs = db.Repository<CO_Division>().Query().Get()
                    .Where(x => cIDs.Contains(x.CircleID)).Select(x => x.ID).ToList<long>();
                var dID = new List<long?>();
                foreach (var id in dIDs)
                    dID.Add(id);

                var subDiv = db.Repository<CO_SubDivision>().Query().Get()
                    .Where(x => dID.Contains(x.DivisionID)).Select(x => new { x.ID, x.Name }).ToList<object>();

                lstRegion.Add(subDiv);
                lstRegion.Add(div);
                lstRegion.Add(circls);
                lstRegion.Add(zone);
            }
            return lstRegion;
        }

        public List<SubDiv_WL> GetSubDivisionalLoss_Daily(long _SubDivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            List<SubDiv_WL> lstAll = new List<SubDiv_WL>();
            List<SubDiv_WL> lstParent = new List<SubDiv_WL>();
            List<SubDiv_WL> lstChild = new List<SubDiv_WL>();
            List<Dictionary<string, double?>> lstTotal = new List<Dictionary<string, double?>>();

            List<WL_SubDivisionLG> lstParantLG = db.Repository<WL_SubDivisionLG>().Query().Get()
                .Where(x => x.SubDivID == _SubDivisionID
                && (DbFunctions.TruncateTime(x.LGDate) >= DbFunctions.TruncateTime(_FromDate)
                    && DbFunctions.TruncateTime(x.LGDate) <= DbFunctions.TruncateTime(_ToDate.Date)))
                .OrderBy(x => x.ParentChannelID).ToList().
                OrderBy(x => x.LGDate).ToList<WL_SubDivisionLG>();

            List<long?> lstParentLGID = lstParantLG.Select(x => x.ParentChannelID).Distinct().ToList<long?>();

            long? currentChannel = null;
            var mdl = new SubDiv_WL();

            Dictionary<string, double?> totalCount = new Dictionary<string, double?>();
            Dictionary<string, double?> outlets = new Dictionary<string, double?>();
            #region ParentChannel Loss Gain

            foreach (var r in lstParentLGID)
            {
                mdl = new SubDiv_WL();
                mdl.ParentID = 0;
                mdl.ID = Convert.ToInt64(r);
                mdl.Name = GetChannelNameByID(mdl.ID);
                mdl.isOutlet = false;
                mdl.LstAttributes = new Dictionary<string, double?>();
                lstParent.Add(mdl);
            }

            foreach (var p in lstParent)
            {
                List<WL_SubDivisionLG> lstTemp = (from l in lstParantLG where l.ParentChannelID == p.ID select l).ToList().OrderBy(x => x.LGDate).ToList();
                foreach (var item in lstTemp)
                {
                    p.DD = GetGaugeDesignDischargeByID(Convert.ToInt64(item.SubDivGaugeID));

                    if (p.LstAttributes.ContainsKey(item.LGDate.Value.ToString("dd-MMM")))
                        p.LstAttributes[item.LGDate.Value.ToString("dd-MMM")] = item.ParentDischarge;
                    else
                        p.LstAttributes.Add(item.LGDate.Value.ToString("dd-MMM"), item.ParentDischarge);

                    if (outlets.ContainsKey(item.LGDate.Value.ToString("dd-MMM")))
                        outlets[item.LGDate.Value.ToString("dd-MMM")] = item.OutletDischarge;
                    else
                        outlets.Add(item.LGDate.Value.ToString("dd-MMM"), item.OutletDischarge);
                }
            }

            //foreach (var item in lstParantLG)
            //{ 



            //    if (currentChannel != item.ParentChannelID)
            //    {
            //        mdl = new SubDiv_WL();
            //        mdl.ParentID = 0;
            //        mdl.ID = Convert.ToInt64(item.ParentChannelID);
            //        mdl.Name = GetChannelNameByID(mdl.ID);
            //        mdl.DD = GetGaugeDesignDischargeByID(Convert.ToInt64(item.SubDivGaugeID));
            //        mdl.isOutlet = false;
            //        if(mdl.LstAttributes == null)
            //            mdl.LstAttributes = new Dictionary<string, double?>();

            //        if (mdl.LstAttributes.ContainsKey(item.LGDate.Value.ToString("dd-MMM")))
            //            mdl.LstAttributes[item.LGDate.Value.ToString("dd-MMM")] = item.ParentDischarge; 
            //        else 
            //            mdl.LstAttributes.Add(item.LGDate.Value.ToString("dd-MMM"), item.ParentDischarge);
            //        lstParent.Add(mdl);
            //        currentChannel = item.ParentChannelID;
            //    }
            //    else
            //    {
            //        if (mdl.LstAttributes.ContainsKey(item.LGDate.Value.ToString("dd-MMM")))
            //            mdl.LstAttributes[item.LGDate.Value.ToString("dd-MMM")] = item.ParentDischarge;
            //        else
            //            mdl.LstAttributes.Add(item.LGDate.Value.ToString("dd-MMM"), item.ParentDischarge);
            //    }
            //} 
            #endregion

            currentChannel = null;
            List<long> temp = lstParantLG.Select(x => x.ID).Distinct().ToList<long>();
            List<long?> _temp = new List<long?>();
            foreach (var v in temp)
                _temp.Add(v);

            #region Direct Offtake Channels Loss Gain

            List<WL_SubDivisionOfftakeLG> lstChildLG = db.Repository<WL_SubDivisionOfftakeLG>().Query().Get()
               .Where(x => _temp.Contains(x.SubDivLGID))
               .OrderBy(x => x.ChannelID).ToList<WL_SubDivisionOfftakeLG>();

            //Get List of All Offtake channels
            foreach (var item in lstChildLG)
            {
                if (currentChannel != item.ChannelID)
                {
                    mdl = new SubDiv_WL();
                    mdl.ParentID = Convert.ToInt64(item.ParentChannelID);
                    mdl.ID = Convert.ToInt64(item.ChannelID);
                    mdl.Name = GetChannelNameByID(mdl.ID);
                    mdl.DD = GetGaugeDesignDischargeByID(Convert.ToInt64(item.GaugeID));
                    mdl.isOutlet = false;
                    lstChild.Add(mdl);
                    currentChannel = item.ChannelID;
                }
            }

            foreach (var c in lstChild)
            {
                List<WL_SubDivisionOfftakeLG> tempChildLG = (from o in lstChildLG where o.ChannelID == c.ID select o).ToList().OrderBy(x => x.DischargeDate).ToList();
                c.LstAttributes = new Dictionary<string, double?>();
                foreach (var v in tempChildLG)
                    c.LstAttributes.Add(v.DischargeDate.Value.ToString("dd-MMM"), v.GaugeDischarge);
            }

            #endregion

            #region merge all in one
            lstParent = lstParent.OrderBy(x => x.Name).ToList();
            foreach (var p in lstParent)
            {
                lstAll.Add(p);
                Dictionary<string, double?> total = new Dictionary<string, double?>();
                double? totalDischarge = p.DD == null ? 0 : 0;
                foreach (var a in p.LstAttributes)
                    total.Add(a.Key, 0.0);

                //Add child 
                List<SubDiv_WL> tempC = (from c in lstChild where c.ParentID == p.ID select c).OrderBy(c => c.Name).ToList();
                foreach (var c in tempC)
                {
                    double? childDD = c.DD == null ? 0 : c.DD;
                    totalDischarge = totalDischarge + childDD;

                    foreach (var a in c.LstAttributes)
                    {
                        double? value = total[a.Key];
                        if (value == null)
                            value = 0;
                        value = value + (c.LstAttributes[a.Key] == null ? 0 : c.LstAttributes[a.Key]);
                        total[a.Key] = value;
                    }

                    lstAll.Add(c);
                }
                //Add Direct Outlet Discharge
                WL_SubDivisionLG mdlTempChnlInfo =
                    (from i in lstParantLG where i.ParentChannelID == p.ID && i.SubDivID == _SubDivisionID select i).ToList().ElementAt(0);

                if (mdlTempChnlInfo.TotalOfftakeDischarge != null)
                {
                    SubDiv_WL mdlOutlet = new SubDiv_WL();
                    mdlOutlet.ParentID = p.ID;
                    mdlOutlet.ID = 0;
                    mdlOutlet.Name = "Direct Outlets";
                    // Getting calculated Outlet Discharge. 
                    mdlOutlet.DD = mdlTempChnlInfo.OutletDischarge; //GetOutletDesignDischarge(Convert.ToInt64(mdlTempChnlInfo.ParentChannelID), Convert.ToInt64(mdlTempChnlInfo.SubDivGaugeID));
                    Dictionary<string, double?> outletsD = new Dictionary<string, double?>();
                    foreach (var item in outlets)
                    {
                        outletsD.Add(item.Key, mdlOutlet.DD);
                    }
                    mdlOutlet.LstAttributes = outletsD;
                    //    new Dictionary<string, double?>(); 
                    //foreach(var a in p.LstAttributes)
                    //    mdlOutlet.LstAttributes.Add(a.Key, mdlOutlet.DD);

                    totalDischarge = totalDischarge + (mdlOutlet.DD == null ? 0 : mdlOutlet.DD);

                    foreach (var a in mdlOutlet.LstAttributes)
                    {
                        double? value = total[a.Key];
                        double? childValue = mdlOutlet.LstAttributes[a.Key];
                        value = value + (childValue == null ? 0 : childValue);
                        total[a.Key] = value;
                    }

                    lstAll.Add(mdlOutlet);

                    //Total 
                    SubDiv_WL totalMdl = new SubDiv_WL();
                    totalMdl.ParentID = -11;
                    totalMdl.ID = 0;
                    totalMdl.Name = "Total";
                    totalMdl.DD = totalDischarge;
                    totalMdl.isOutlet = true;
                    totalMdl.LstAttributes = total;
                    lstAll.Add(totalMdl);
                }

                long? GaugeID = (from l in lstParantLG where l.ParentChannelID == p.ID select l).Select(l => l.SubDivGaugeID).ToList().ElementAt(0).Value;
                //Water to next division
                SubDiv_WL mdlWater = WaterDeliveredToNextDivision(Convert.ToInt64(GaugeID), _FromDate, _ToDate);
                if (mdlWater != null)
                    lstAll.Add(mdlWater);
            }
            #endregion

            return lstAll;
        }

        private SubDiv_WL WaterDeliveredToNextDivision(long _CurrentGaugeID, DateTime _FromDate, DateTime _ToDate)
        {
            SubDiv_WL mdlTemp = null;

            CO_ChannelGauge mdlNextGauges = null;
            CO_ChannelGauge mdl = db.Repository<CO_ChannelGauge>().Query().Get().Where(x => x.ID == _CurrentGaugeID).SingleOrDefault();

            List<CO_ChannelGauge> lstNextGg = db.Repository<CO_ChannelGauge>().Query().Get().Where(x => x.ChannelID == mdl.ChannelID
                && x.ID != mdl.ID && x.GaugeSubDivID != null && x.GaugeAtRD > mdl.GaugeAtRD).ToList().OrderBy(x => x.GaugeAtRD).ToList<CO_ChannelGauge>();

            if (lstNextGg != null && lstNextGg.Count > 0)
                mdlNextGauges = lstNextGg.ElementAt(0);

            if (mdl != null && mdlNextGauges != null)
            {
                long? subDivID = mdlNextGauges.GaugeSubDivID;//db.Repository<CO_Section>().Query().Get().Where(x => x.ID == mdlNextGauges.SectionID).SingleOrDefault().SubDivID;
                if (subDivID != null) { }
                {
                    List<WL_SubDivisionLG> lst = db.Repository<WL_SubDivisionLG>().Query().Get().Where(x => x.SubDivID == subDivID
                        && x.SubDivGaugeID == mdlNextGauges.ID && (DbFunctions.TruncateTime(x.LGDate) >= DbFunctions.TruncateTime(_FromDate)
                        && DbFunctions.TruncateTime(x.LGDate) <= DbFunctions.TruncateTime(_ToDate.Date))).ToList();

                    mdlTemp = new SubDiv_WL();
                    mdlTemp.ParentID = -12;
                    mdlTemp.ID = -12;
                    mdlTemp.Name = "Water delivered to " + GetSubDivisionNameByID(Convert.ToInt64(subDivID));
                    mdlTemp.isOutlet = false;
                    mdlTemp.LstAttributes = new Dictionary<string, double?>();

                    List<WL_SubDivisionLG> lstTemp = lst.OrderBy(x => x.LGDate).ToList();
                    foreach (var a in lstTemp)
                        mdlTemp.LstAttributes.Add(a.LGDate.Value.ToString("dd-MMM"), (a.ParentDischarge == null ? 0 : a.ParentDischarge));
                }
            }
            return mdlTemp;
        }

        private double? GetOutletDesignDischarge(long _ChannelID, long _GaugeID)
        {
            double? value = null;
            CO_ChannelGauge gauge = db.Repository<CO_ChannelGauge>().Query().Get().Where(x => x.ID == _GaugeID).SingleOrDefault();
            if (gauge != null)
            {
                double fromRD = (gauge.GaugeAtRD), toRD = 0;

                List<CO_ChannelGauge> lstGauges = db.Repository<CO_ChannelGauge>().Query().Get()
                    .Where(x => x.ChannelID == _ChannelID && x.GaugeAtRD > fromRD &&
                        (x.GaugeCategoryID == (long)Constants.GaugeCategory.TailGauge || x.GaugeSubDivID != null)).OrderBy(x => x.GaugeAtRD).ToList();

                if (lstGauges != null && lstGauges.Count > 0)
                {
                    CO_ChannelGauge mdlNextGauge = lstGauges.ElementAt(0);

                    if (mdlNextGauge.GaugeCategoryID == (long)Constants.GaugeCategory.TailGauge)
                    {
                        toRD = mdlNextGauge.CO_Channel.TotalRDs;
                    }
                    else
                    {
                        toRD = mdlNextGauge.GaugeAtRD;
                    }
                }

                if (toRD > 0)
                {
                    WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();
                    value = repWaterLosses.GetOutletsDesignDischarge(_ChannelID, Convert.ToInt32(fromRD), Convert.ToInt32(toRD));
                }
            }
            return value;

        }

        private string GetSubDivisionNameByID(long _SubDivID)
        {
            CO_SubDivision mdl = db.Repository<CO_SubDivision>().Query().Get().Where(x => x.ID == _SubDivID).SingleOrDefault();
            if (mdl != null)
                return mdl.Name;
            else
                return "";
        }

        public List<SubDiv_WL> GetSubDivisionalLosses(long _SubDivisionID, int _Year, int _From, int _To)
        {
            WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();

            List<GetSubDivisionalWL_Result> lstSubDivLG = new List<GetSubDivisionalWL_Result>();
            lstSubDivLG = repWaterLosses.GetSubdivionalWaterLosses(_SubDivisionID, _Year, _From, _To);

            List<SubDiv_WL> lstParent = new List<SubDiv_WL>();
            List<SubDiv_WL> lstChild = new List<SubDiv_WL>();
            List<SubDiv_WL> lstAll = new List<SubDiv_WL>();

            long? currentChannel = null;
            var mdl = new SubDiv_WL();

            #region Parent Channel Loss Gain
            List<GetSubDivisionalWL_Result> lstTempParents = (from a in lstSubDivLG where a.ParentID == 0 select a).ToList().OrderBy(x => x.ID).ToList();
            List<long> lstParentID = (from a in lstTempParents select a).Select(a => a.ID).Distinct().ToList<long>();

            Dictionary<string, double?> outlet = new Dictionary<string, double?>();
            foreach (var a in lstParentID)
            {
                mdl = new SubDiv_WL();
                mdl.ParentID = 0;
                mdl.ID = a;
                mdl.isOutlet = false;
                mdl.LstAttributes = new Dictionary<string, double?>();
                lstParent.Add(mdl);
            }

            foreach (var p in lstParent)
            {
                List<GetSubDivisionalWL_Result> lstTemp = (from l in lstTempParents where l.ID == p.ID select l).ToList().ToList();
                foreach (var item in lstTemp)
                {
                    p.Name = item.Name;
                    p.DD = item.DD;
                    //p.LstAttributes.Add(item.Criteria + "", (item.Discharge == null ? 0 : item.Discharge)); // p.DD == null ? 0 : p.DD;
                    //outlet.Add(item.Criteria + "", (item.Discharge == null ? 0 : item.OutletDD));

                    if (p.LstAttributes.ContainsKey(item.Criteria))
                    {
                        double ExistingDischarge = Convert.ToDouble(p.LstAttributes[item.Criteria]);
                        double Discharge = (item.Discharge == null ? 0 : item.Discharge.Value);
                        p.LstAttributes[item.Criteria] = ExistingDischarge + Discharge;
                    }
                    else
                    {
                        p.LstAttributes.Add(item.Criteria + "", (item.Discharge == null ? 0 : item.Discharge));
                    }

                    if (outlet.ContainsKey(item.Criteria))
                    {
                        double ExistingDischarge = Convert.ToDouble(outlet[item.Criteria]);
                        double Discharge = (item.OutletDD == null ? 0 : item.OutletDD.Value);
                        outlet[item.Criteria] = ExistingDischarge + Discharge;
                    }
                    else
                    {
                        outlet.Add(item.Criteria + "", (item.OutletDD == null ? 0 : item.OutletDD));
                    }
                }
            }
            #endregion

            currentChannel = null;

            #region Direct Offtake Channels Loss Gain

            List<long> tempParentIDs = lstParent.Select(x => x.ID).Distinct().ToList<long>();
            List<GetSubDivisionalWL_Result> lstTempChild = (from b in lstSubDivLG where tempParentIDs.Contains(b.ParentID) select b).ToList().OrderBy(x => x.ID).ToList();

            //Get List of All Offtake channels
            foreach (var item in lstTempChild)
            {
                if (currentChannel != item.ID)
                {
                    mdl = new SubDiv_WL();
                    mdl.ParentID = item.ParentID;
                    mdl.ID = Convert.ToInt64(item.ID);
                    mdl.Name = item.Name;
                    mdl.DD = item.DD;
                    mdl.isOutlet = false;
                    lstChild.Add(mdl);
                    currentChannel = item.ID;
                }
            }

            foreach (var c in lstChild)
            {
                List<GetSubDivisionalWL_Result> tempChildLG = (from d in lstTempChild where d.ID == c.ID select d).ToList().OrderBy(x => x.Criteria).ToList();
                c.LstAttributes = new Dictionary<string, double?>();
                foreach (var v in tempChildLG)
                    c.LstAttributes.Add(v.Criteria + "", v.Discharge);
            }
            #endregion

            #region merge all in one
            lstParent = lstParent.OrderBy(x => x.Name).ToList();
            foreach (var p in lstParent)
            {
                lstAll.Add(p);
                Dictionary<string, double?> total = new Dictionary<string, double?>();
                double? totalDischarge = p.DD == null ? 0 : 0;
                foreach (var a in p.LstAttributes)
                    total.Add(a.Key, (a.Value == null ? 0 : 0));

                //Add child 
                List<SubDiv_WL> tempC = (from c in lstChild where c.ParentID == p.ID select c).OrderBy(c => c.Name).ToList();
                foreach (var c in tempC)
                {
                    double? childDD = c.DD == null ? 0 : c.DD;
                    totalDischarge = totalDischarge + childDD;

                    foreach (var a in c.LstAttributes)
                    {
                        double? value = total[a.Key];
                        value = value + (c.LstAttributes[a.Key] == null ? 0 : c.LstAttributes[a.Key]);
                        total[a.Key] = value;
                    }

                    lstAll.Add(c);
                }
                //Add Direct Outlet Discharge
                WL_SubDivisionLG mdlTempChnlInfo =
                    db.Repository<WL_SubDivisionLG>().Query().Get().Where(x => x.ParentChannelID == p.ID && x.SubDivID == _SubDivisionID).ToList().ElementAt(0);

                if (mdlTempChnlInfo.TotalOfftakeDischarge != null)
                {
                    SubDiv_WL mdlOutlet = new SubDiv_WL();
                    mdlOutlet.ParentID = p.ID;
                    mdlOutlet.ID = 0;
                    mdlOutlet.Name = "Direct Outlets";
                    // Getting calculated Outlet Discharge.
                    mdlOutlet.DD = mdlTempChnlInfo.OutletDischarge;//GetOutletDesignDischarge(Convert.ToInt64(mdlTempChnlInfo.ParentChannelID), Convert.ToInt64(mdlTempChnlInfo.SubDivGaugeID));
                    mdlOutlet.isOutlet = false;
                    Dictionary<string, double?> outletD = new Dictionary<string, double?>();
                    foreach (var item in outlet)
                    {
                        outletD.Add(item.Key, mdlOutlet.DD);
                    }
                    mdlOutlet.LstAttributes = outletD;
                    //foreach (var a in p.LstAttributes)
                    //    mdlOutlet.LstAttributes.Add(a.Key, mdlOutlet.DD);

                    totalDischarge = totalDischarge + (mdlOutlet.DD == null ? 0 : mdlOutlet.DD);


                    foreach (var a in mdlOutlet.LstAttributes)
                    {
                        double? value = total[a.Key];
                        double? childValue = mdlOutlet.LstAttributes[a.Key];
                        value = value + (childValue == null ? 0 : childValue);
                        total[a.Key] = value;
                    }
                    lstAll.Add(mdlOutlet);

                    //Total 
                    SubDiv_WL totalMdl = new SubDiv_WL();
                    totalMdl.ParentID = -11;
                    totalMdl.ID = 0;
                    totalMdl.Name = "Total";
                    totalMdl.DD = totalDischarge;
                    totalMdl.isOutlet = true;
                    totalMdl.LstAttributes = total;
                    lstAll.Add(totalMdl);
                }

                //Water to Next Division
                //List<long> lstSec = db.Repository<CO_Section>().Query().Get().Where(x => x.SubDivID == _SubDivisionID).Select(x => x.ID).Distinct().ToList();
                //List<long?> lstSecTemp = new List<long?>();
                //foreach (var a in lstSec)
                //    lstSecTemp.Add(a);

                //long GaugeID = db.Repository<CO_ChannelGauge>().Query().Get().Where(x => x.ChannelID == p.ID && lstSecTemp.Contains(x.SectionID) && x.GaugeCategoryID == 4).SingleOrDefault().ID;

                long GaugeID = db.Repository<CO_ChannelGauge>().GetAll().Where(cg => cg.ChannelID == p.ID && cg.GaugeSubDivID == _SubDivisionID).Select(cg => cg.ID).FirstOrDefault();

                //Water to next division
                if (GaugeID != 0)
                {
                    SubDiv_WL mdlWater = WaterDeliveredToNextDivision_WL(Convert.ToInt64(GaugeID), _Year, _From, _To);
                    if (mdlWater != null)
                        lstAll.Add(mdlWater);
                }

            }
            #endregion

            return lstAll;
        }

        private SubDiv_WL WaterDeliveredToNextDivision_WL(long _CurrentGaugeID, int _Year, int _From, int _To)
        {
            SubDiv_WL mdlTemp = null;

            CO_ChannelGauge mdlNextGauges = null;
            CO_ChannelGauge mdl = db.Repository<CO_ChannelGauge>().Query().Get().Where(x => x.ID == _CurrentGaugeID).SingleOrDefault();

            List<CO_ChannelGauge> lstNextGg = db.Repository<CO_ChannelGauge>().Query().Get().Where(x => x.ChannelID == mdl.ChannelID
                && x.ID != mdl.ID && x.GaugeSubDivID != null && x.GaugeAtRD > mdl.GaugeAtRD).ToList().OrderBy(x => x.GaugeAtRD).ToList<CO_ChannelGauge>();

            if (lstNextGg != null && lstNextGg.Count > 0)
                mdlNextGauges = lstNextGg.ElementAt(0);

            if (mdl != null && mdlNextGauges != null)
            {
                long? subDivID = mdlNextGauges.GaugeSubDivID; //db.Repository<CO_Section>().Query().Get().Where(x => x.ID == mdlNextGauges.SectionID).SingleOrDefault().SubDivID;
                if (subDivID != null)
                {
                    mdlTemp = new SubDiv_WL();
                    mdlTemp.ParentID = -12;
                    mdlTemp.ID = -12;
                    mdlTemp.Name = "Water delivered to " + GetSubDivisionNameByID(Convert.ToInt64(subDivID));
                    mdlTemp.isOutlet = false;
                    mdlTemp.LstAttributes = new Dictionary<string, double?>();

                    WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();
                    List<GetSubDivDeliveredTo_Result> lst = repWaterLosses.GetSubdivisionalWL(mdlNextGauges.ID, Convert.ToInt64(subDivID), _Year, _From, _To);

                    foreach (var a in lst)
                        mdlTemp.LstAttributes.Add(a.Criteria, (a.Discharge == null ? 0 : a.Discharge));
                }
            }
            return mdlTemp;
        }

        public List<SubDiv_WL> GetDivisionalLosses(long _DivisionID, int _SearchType, string _FromDate, string _ToDate, int _Year, int _From, int _To)
        {
            WaterLossesRepository repWaterLosses = db.ExtRepositoryFor<WaterLossesRepository>();

            List<WL_GetDivisionalLGData_Result> lstDivLG =
                repWaterLosses.GetDivisionalWL(_DivisionID, _SearchType, _FromDate, _ToDate, _Year, _From, _To);
            List<SubDiv_WL> lstAll = new List<SubDiv_WL>();
            //Dictionary<string, double?> dd_WaterToNextDiv = new Dictionary<string, double?>();
            if (lstDivLG != null)
            {
                List<long> lstSubDivID = lstDivLG.Select(x => x.SubDivID).Distinct().ToList();
                List<string> keys = lstDivLG.Select(x => x.Criteria).Distinct().ToList();

                //Update for Water to next division

                //foreach (var a in keys)
                //    dd_WaterToNextDiv.Add(a, 0.0);
                //Update for Water to next division

                foreach (var id in lstSubDivID)
                {
                    List<long> lstChnlID = lstDivLG.Where(x => x.SubDivID == id).Select(x => x.ChannelID).Distinct().ToList();

                    foreach (var chnldID in lstChnlID)
                    {
                        List<WL_GetDivisionalLGData_Result> lstSubSet = lstDivLG.Where(x => x.SubDivID == id && x.ChannelID == chnldID).ToList();
                        WL_GetDivisionalLGData_Result tempChnl = lstSubSet.ElementAt(0);
                        SubDiv_WL mdl = new SubDiv_WL();
                        mdl.ParentID = id;
                        mdl.ParentName = tempChnl.SubDiv;
                        mdl.ID = chnldID;
                        mdl.Name = tempChnl.ChannelName + " (" + Calculations.GetRDText(tempChnl.SubDivGaugeRD) + ")";
                        mdl.DD = tempChnl.SubDivGaugeDD;
                        mdl.TotalOfftakeDD = tempChnl.OfftakeDDSum;
                        mdl.TotalOutletDD = tempChnl.OutletDDSum;
                        int? isDivGauge = tempChnl.IsDivisonal;
                        if (isDivGauge != null && isDivGauge > 0)
                            mdl.isOutlet = true;
                        else
                            mdl.isOutlet = false;

                        mdl.LstAttributes = new Dictionary<string, double?>();
                        mdl.LstAttributes_offtakes = new Dictionary<string, double?>();
                        mdl.LstAttributes_Outlets = new Dictionary<string, double?>();

                        foreach (var k in keys)
                        {
                            WL_GetDivisionalLGData_Result temp = null;
                            List<WL_GetDivisionalLGData_Result> lst = lstSubSet.Where(x => x.Criteria.Equals(k)).ToList();

                            if (lst != null && lst.Count > 0)
                                temp = lst.ElementAt(0);

                            if (temp != null)
                            {
                                mdl.LstAttributes.Add(k, temp.SubDivDschrg);
                                mdl.LstAttributes_offtakes.Add(k, temp.Offtakes);
                                mdl.LstAttributes_Outlets.Add(k, temp.DirectOutlet);
                            }
                            else
                            {
                                mdl.LstAttributes.Add(k, 0);
                                mdl.LstAttributes_offtakes.Add(k, 0);
                                mdl.LstAttributes_Outlets.Add(k, 0);
                            }
                        }

                        lstAll.Add(mdl);

                        //Update for Water to next division
                        //List<WL_GetDivisionalWatertoNextDivisions_Result> lstWtr2NextDiv =
                        //    repWaterLosses.GetDivisionalWatertoNextDivisions(_DivisionID, chnldID, _SearchType, _FromDate, _ToDate, _Year, _From, _To);
                        //if (lstWtr2NextDiv != null && lstWtr2NextDiv.Count() > 0)
                        //{
                        //    foreach (var K in keys)
                        //    {
                        //        double? value = dd_WaterToNextDiv[K];
                        //        double? childValue = lstWtr2NextDiv.Where(x => x.Criteria.Equals(K)).ToList().ElementAt(0).SubDivDschrg;
                        //        value = value + (childValue == null ? 0 : childValue);
                        //        dd_WaterToNextDiv[K] = value;
                        //    }
                        //}
                        //Update for Water to next division                       
                    }
                }
            }
            //lstAll.Add(WaterToNextDivisionModel(dd_WaterToNextDiv));
            return lstAll;
        }

        private SubDiv_WL WaterToNextDivisionModel(Dictionary<string, double?> _Values)
        {
            SubDiv_WL mdl = new SubDiv_WL();
            mdl.ParentID = -111;
            mdl.ParentName = "Water to Next";
            mdl.ID = -111;
            mdl.Name = "Water to Next Divisions";
            mdl.DD = null;
            mdl.TotalOfftakeDD = null;
            mdl.TotalOutletDD = null;
            mdl.isOutlet = false;

            mdl.LstAttributes = _Values;

            return mdl;
        }
        public string GetChannelNameByID(long _ChannelID)
        {
            return db.Repository<CO_Channel>().FindById(_ChannelID).NAME;
        }
        public double? GetGaugeDesignDischargeByID(long _GaugeID)
        {
            return db.Repository<CO_ChannelGauge>().FindById(_GaugeID).DesignDischarge;
        }

        public DataTable GetCurrentAndNextDivisionalDischarge(long _DivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            return db.ExecuteStoredProcedureDataTable("WL_GetCurrentAndNextDivisionalDischarge", _DivisionID, _FromDate, _ToDate);
        }

        public DataTable GetCurrentAndNextSubDivisionalDischarge(long _SubDivisionID, DateTime _FromDate, DateTime _ToDate)
        {
            return db.ExecuteStoredProcedureDataTable("WL_GetCurrentAndNextSubDivisionalDischarge", _SubDivisionID, _FromDate, _ToDate);
        }

        public class SubDiv_WL
        {
            public long ID; public string Name; public double? DD; public long ParentID; public bool isOutlet; public string ParentName;
            public double? TotalOfftakeDD; public double? TotalOutletDD;
            public Dictionary<string, double?> LstAttributes;
            public Dictionary<string, double?> LstAttributes_offtakes;
            public Dictionary<string, double?> LstAttributes_Outlets;
        }
        #endregion

        private string GetUserNameByID(int? _UserID)
        {
            if (_UserID == null)
                return "-";

            UA_Users mdlUser = db.Repository<UA_Users>().Query().Get().Where(x => x.ID == _UserID).SingleOrDefault();
            return mdlUser.FirstName + " " + mdlUser.LastName;
        }

        public List<long?> GetSectionListByUser(long _UserID, long? _IrrigationLevel)
        {
            List<long?> lstSections = new List<long?>();
            List<long?> qLst = new List<long?>();
            List<long> _lst = new List<long>();
            switch (_IrrigationLevel)
            {
                case 5: //Section level
                    lstSections = db.Repository<UA_AssociatedLocation>().Query().Get()
                        .Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();
                    break;

                case 4: //Sub divisional level
                    qLst = db.Repository<UA_AssociatedLocation>().Query().Get()
                       .Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();

                    _lst = db.Repository<CO_Section>().Query().Get()
                       .Where(x => qLst.Contains(x.SubDivID)).Select(x => x.ID).ToList<long>();

                    foreach (var v in _lst)
                    {
                        lstSections.Add((long?)v);
                    }
                    break;

                case 3: //Divisional level
                    qLst = db.Repository<UA_AssociatedLocation>().Query().Get()
                       .Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();

                    List<long> q = db.Repository<CO_SubDivision>().Query().Get()
                        .Where(x => qLst.Contains(x.DivisionID)).Select(x => x.ID)
                        .ToList<long>();

                    qLst.Clear();
                    foreach (var v in q)
                    {
                        qLst.Add((long?)v);
                    }


                    _lst = db.Repository<CO_Section>().Query().Get()
                        .Where(x => qLst.Contains(x.SubDivID)).Select(x => x.ID).ToList<long>();

                    lstSections.Clear();
                    foreach (var v in _lst)
                    {
                        lstSections.Add((long?)v);
                    }
                    break;

                case 2://Circle Level
                    qLst = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();

                    lstSections = db.Repository<CO_Section>().GetAll().Where(x => qLst.Contains(x.CO_SubDivision.CO_Division.CircleID)).Select(x => (long?)x.ID).ToList<long?>();
                    break;

                case 1://Zone Level
                    qLst = db.Repository<UA_AssociatedLocation>().Query().Get().Where(x => x.UserID == _UserID).Select(x => x.IrrigationBoundryID).ToList<long?>();

                    lstSections = db.Repository<CO_Section>().GetAll().Where(x => qLst.Contains(x.CO_SubDivision.CO_Division.CO_Circle.ZoneID)).Select(x => (long?)x.ID).ToList<long?>();
                    break;

                default:
                    break;
            }

            return lstSections;
        }

        public List<object> GetAllZones()
        {
            var lstZone = db.Repository<CO_Zone>().GetAll().OrderBy(z => z.Name).Select(x => new { x.ID, x.Name }).ToList<object>();
            return lstZone;
        }

        public List<object> GetCirclesByZoneID(long _ZoneID)
        {
            var lst = db.Repository<CO_Circle>().Query().Get().Where(x => x.ZoneID == _ZoneID)
                .OrderBy(z => z.Name).Select(x => new { x.ID, x.Name }).ToList<object>();
            return lst;
        }

        public List<object> GetDivisionsByCircleID(long _CircleID)
        {
            var lst = db.Repository<CO_Division>().Query().Get().Where(x => x.CircleID == _CircleID)
                .OrderBy(z => z.Name).Select(x => new { x.ID, x.Name }).ToList<object>();
            return lst;
        }

        public List<object> GetSubDivByDivisionID(long _DivisionID)
        {
            var lst = db.Repository<CO_SubDivision>().Query().Get().Where(x => x.DivisionID == _DivisionID)
                .OrderBy(z => z.Name).Select(x => new { x.ID, x.Name }).ToList<object>();
            return lst;
        }


        #region Entitlement (CR)

        public List<object> GetCanalsByParentIDForED(long _UserID, bool _UserBaseLoading, int _IrrigationLevel, long _ParentChannelID, int _ChannelType)
        {
            List<object> lstCanals = new List<object>();
            List<innerClass> lstAllCanals = new List<innerClass>();
            List<long> lstChillChnlID = new List<long>();

            if (_ChannelType == 0)
            {
                if (_UserBaseLoading)
                {
                    List<long?> lstSections = GetSectionListByUser(_UserID, _IrrigationLevel);

                    List<long?> lstChnlID = db.Repository<CO_ChannelIrrigationBoundaries>().Query().Get()
                        .Where(x => lstSections.Contains(x.SectionID)).Select(x => x.ChannelID).ToList<long?>();

                    lstChillChnlID = db.Repository<ED_CommandChannelChilds>().Query().Get()
                       .Where(x => x.CommandChannelID == _ParentChannelID && x.IsExcluded == false && lstChnlID.Contains(x.ChannelID)).Select(q => q.ChannelID).ToList<long>();
                }
                else
                {

                    lstChillChnlID = db.Repository<ED_CommandChannelChilds>().Query().Get()
                       .Where(x => x.CommandChannelID == _ParentChannelID && x.IsExcluded == false).Select(q => q.ChannelID).ToList<long>();
                }
            }
            else
            {
                if (_UserBaseLoading)
                {
                    List<long?> lstSections = GetSectionListByUser(_UserID, _IrrigationLevel);

                    List<long?> lstChnlID = db.Repository<CO_ChannelIrrigationBoundaries>().Query().Get()
                        .Where(x => lstSections.Contains(x.SectionID)).Select(x => x.ChannelID).ToList<long?>();

                    lstChillChnlID = db.Repository<CO_ChannelParentFeeder>().Query().Get()
                        .Where(x => x.ParrentFeederID == _ParentChannelID && lstChnlID.Contains(x.ChannelID)).Select(x => (long)x.ChannelID).ToList<long>();
                }
                else
                {
                    lstChillChnlID = db.Repository<CO_ChannelParentFeeder>().Query().Get()
                       .Where(x => x.ParrentFeederID == _ParentChannelID && x.RelationType == "P" && x.StructureTypeID == (long)Constants.StructureType.Channel).Select(x => (long)x.ChannelID).ToList<long>();
                }
            }

            lstAllCanals = db.Repository<CO_Channel>().Query().Get().Where(x => lstChillChnlID.Contains(x.ID))
                    .Select(x => new { x.ID, x.NAME, x.ChannelTypeID }).ToList().Select(x => new innerClass { ID = x.ID, Name = x.NAME, Type = x.ChannelTypeID }).OrderBy(x => x.Name).ToList<innerClass>();

            if (_ChannelType == 0)//Canal System
            {
                List<object> lstMainCanal = (from c in lstAllCanals where c.Type == 1 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstBranchCanal = (from c in lstAllCanals where c.Type == 3 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstDstrbtry = (from c in lstAllCanals where c.Type == 4 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstMainCanal);
                lstCanals.Add(lstBranchCanal);
                lstCanals.Add(lstDstrbtry);
                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else if (_ChannelType == 1)//MainCanal
            {
                List<object> lstMainCanal = (from c in lstAllCanals where c.Type == 1 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstBranchCanal = (from c in lstAllCanals where c.Type == 3 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstDstrbtry = (from c in lstAllCanals where c.Type == 4 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstMainCanal);
                lstCanals.Add(lstBranchCanal);
                lstCanals.Add(lstDstrbtry);
                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else if (_ChannelType == 3) //BranchCanal
            {
                List<object> lstBranchCanal = (from c in lstAllCanals where c.Type == 3 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstDstrbtry = (from c in lstAllCanals where c.Type == 4 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstBranchCanal);
                lstCanals.Add(lstDstrbtry);
                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else if (_ChannelType == 4) //Distributory Canal
            {
                List<object> lstDstrbtry = (from c in lstAllCanals where c.Type == 4 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstDstrbtry);
                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else if (_ChannelType == 5) //Minor Canal
            {
                List<object> lstMinor = (from c in lstAllCanals where c.Type == 5 select new { c.ID, c.Name }).ToList<object>();
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstMinor);
                lstCanals.Add(lstSubMinor);
            }
            else
            {
                List<object> lstSubMinor = (from c in lstAllCanals where c.Type == 6 select new { c.ID, c.Name }).ToList<object>();

                lstCanals.Add(lstSubMinor);
            }

            return lstCanals;
        }


        public long GetCommandChannelID(long _ID)
        {
            long ChannelID = db.Repository<ED_CommandChannel>().GetAll().Where(q => q.ID == _ID).Select(w => w.ChannelID).FirstOrDefault();
            return ChannelID;
        }





        #endregion



    }
}