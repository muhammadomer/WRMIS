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
    
    public partial class PE_FieldChannelData
    {
        public long ID { get; set; }
        public Nullable<long> ChannelID { get; set; }
        public Nullable<System.DateTime> ReadingDate { get; set; }
        public Nullable<long> TailStatusM { get; set; }
        public Nullable<long> TailStatusE { get; set; }
        public Nullable<long> TailStatusA { get; set; }
        public Nullable<bool> IsGaugePaintedM { get; set; }
        public Nullable<bool> IsGaugePaintedE { get; set; }
        public Nullable<bool> IsGaugeFixedM { get; set; }
        public Nullable<bool> IsGaugeFixedE { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
    
        public virtual CO_Channel CO_Channel { get; set; }
    }
}
