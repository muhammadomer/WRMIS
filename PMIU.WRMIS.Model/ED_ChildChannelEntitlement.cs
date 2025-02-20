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
    
    public partial class ED_ChildChannelEntitlement
    {
        public long ID { get; set; }
        public Nullable<long> ChannelEntitlementID { get; set; }
        public Nullable<long> ParentChannelID { get; set; }
        public long ChannelID { get; set; }
        public short Year { get; set; }
        public short SeasonID { get; set; }
        public long TDailyCalendarID { get; set; }
        public double MAFEntitlement { get; set; }
        public double CsEntitlement { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual SP_RefTDailyCalendar SP_RefTDailyCalendar { get; set; }
        public virtual CO_Channel CO_Channel { get; set; }
        public virtual ED_ChannelEntitlement ED_ChannelEntitlement { get; set; }
    }
}
