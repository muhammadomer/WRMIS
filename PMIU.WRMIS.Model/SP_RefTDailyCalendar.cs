//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMIU.WRMIS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SP_RefTDailyCalendar
    {
        public SP_RefTDailyCalendar()
        {
            this.SP_ForecastData = new HashSet<SP_ForecastData>();
            this.SP_RefEastern = new HashSet<SP_RefEastern>();
            this.SP_TDailyBarrageData = new HashSet<SP_TDailyBarrageData>();
            this.SP_TDailyCanalData = new HashSet<SP_TDailyCanalData>();
            this.SP_TDailyReservoirData = new HashSet<SP_TDailyReservoirData>();
            this.SP_TDailyRiverData = new HashSet<SP_TDailyRiverData>();
            this.SP_PlanData = new HashSet<SP_PlanData>();
            this.ED_ChildChannelEntitlement = new HashSet<ED_ChildChannelEntitlement>();
            this.ED_TDailyGaugeReading = new HashSet<ED_TDailyGaugeReading>();
            this.ED_ChannelDeliveries = new HashSet<ED_ChannelDeliveries>();
            this.ED_ChannelEntitlement = new HashSet<ED_ChannelEntitlement>();
            this.ED_ChannelWeightedAvg = new HashSet<ED_ChannelWeightedAvg>();
            this.ED_ChannelWeightedAvgDeliveries = new HashSet<ED_ChannelWeightedAvgDeliveries>();
        }
    
        public long ID { get; set; }
        public string ShortName { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public Nullable<short> NoOfDays { get; set; }
        public short SeasonID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<short> TDailyID { get; set; }
        public Nullable<short> Year { get; set; }
    
        public virtual ICollection<SP_ForecastData> SP_ForecastData { get; set; }
        public virtual ICollection<SP_RefEastern> SP_RefEastern { get; set; }
        public virtual ICollection<SP_TDailyBarrageData> SP_TDailyBarrageData { get; set; }
        public virtual ICollection<SP_TDailyCanalData> SP_TDailyCanalData { get; set; }
        public virtual ICollection<SP_TDailyReservoirData> SP_TDailyReservoirData { get; set; }
        public virtual ICollection<SP_TDailyRiverData> SP_TDailyRiverData { get; set; }
        public virtual ICollection<SP_PlanData> SP_PlanData { get; set; }
        public virtual ICollection<ED_ChildChannelEntitlement> ED_ChildChannelEntitlement { get; set; }
        public virtual ICollection<ED_TDailyGaugeReading> ED_TDailyGaugeReading { get; set; }
        public virtual ICollection<ED_ChannelDeliveries> ED_ChannelDeliveries { get; set; }
        public virtual ICollection<ED_ChannelEntitlement> ED_ChannelEntitlement { get; set; }
        public virtual ICollection<ED_ChannelWeightedAvg> ED_ChannelWeightedAvg { get; set; }
        public virtual ICollection<ED_ChannelWeightedAvgDeliveries> ED_ChannelWeightedAvgDeliveries { get; set; }
    }
}
