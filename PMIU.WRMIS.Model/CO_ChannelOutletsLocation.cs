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
    
    public partial class CO_ChannelOutletsLocation
    {
        public long ID { get; set; }
        public Nullable<long> OutletID { get; set; }
        public Nullable<long> VillageID { get; set; }
        public Nullable<bool> LocatedIn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual CO_Village CO_Village { get; set; }
        public virtual CO_ChannelOutlets CO_ChannelOutlets { get; set; }
    }
}
