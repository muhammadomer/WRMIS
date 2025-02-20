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
    
    public partial class CO_ChannelComndType
    {
        public CO_ChannelComndType()
        {
            this.WL_ReachLag = new HashSet<WL_ReachLag>();
            this.ED_ProvincialEntitlement = new HashSet<ED_ProvincialEntitlement>();
            this.CO_Channel = new HashSet<CO_Channel>();
            this.ED_CommandChannel = new HashSet<ED_CommandChannel>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual ICollection<WL_ReachLag> WL_ReachLag { get; set; }
        public virtual ICollection<ED_ProvincialEntitlement> ED_ProvincialEntitlement { get; set; }
        public virtual ICollection<CO_Channel> CO_Channel { get; set; }
        public virtual ICollection<ED_CommandChannel> ED_CommandChannel { get; set; }
    }
}
