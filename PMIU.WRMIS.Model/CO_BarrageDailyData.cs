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
    
    public partial class CO_BarrageDailyData
    {
        public CO_BarrageDailyData()
        {
            this.CO_BarrageDailyDataHistory = new HashSet<CO_BarrageDailyDataHistory>();
            this.CO_BarrageDailyDataDetailHistory = new HashSet<CO_BarrageDailyDataDetailHistory>();
            this.CO_BarrageDailyDataDetail = new HashSet<CO_BarrageDailyDataDetail>();
        }
    
        public long ID { get; set; }
        public System.DateTime ReadingDateTime { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.TimeSpan> TimeStamp { get; set; }
        public Nullable<long> BarrageID { get; set; }
        public string ReasonForChange { get; set; }
        public Nullable<bool> GRMissed { get; set; }
        public Nullable<System.DateTime> EntryDateTime { get; set; }
        public string Source { get; set; }
    
        public virtual ICollection<CO_BarrageDailyDataHistory> CO_BarrageDailyDataHistory { get; set; }
        public virtual CO_Station CO_Station { get; set; }
        public virtual ICollection<CO_BarrageDailyDataDetailHistory> CO_BarrageDailyDataDetailHistory { get; set; }
        public virtual ICollection<CO_BarrageDailyDataDetail> CO_BarrageDailyDataDetail { get; set; }
    }
}
