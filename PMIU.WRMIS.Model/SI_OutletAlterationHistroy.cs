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
    
    public partial class SI_OutletAlterationHistroy
    {
        public long ID { get; set; }
        public Nullable<System.DateTime> AlterationDate { get; set; }
        public Nullable<long> OutletID { get; set; }
        public Nullable<long> OutletTypeID { get; set; }
        public string OutletStatus { get; set; }
        public Nullable<double> OutletHeight { get; set; }
        public Nullable<double> OutletCrest { get; set; }
        public Nullable<double> OutletWidth { get; set; }
        public Nullable<double> OutletWorkingHead { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<long> ScheduleDetailChannelID { get; set; }
        public string Remarks { get; set; }
        public string Source { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<double> Discharge { get; set; }
    
        public virtual CO_OutletType CO_OutletType { get; set; }
        public virtual SI_ScheduleDetailChannel SI_ScheduleDetailChannel { get; set; }
        public virtual UA_Users UA_Users { get; set; }
        public virtual CO_ChannelOutlets CO_ChannelOutlets { get; set; }
    }
}
