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
    
    public partial class SP_RefShareDistributionHistory
    {
        public long ID { get; set; }
        public Nullable<short> TDailyID { get; set; }
        public Nullable<short> SeasonID { get; set; }
        public Nullable<double> BalochistanShare { get; set; }
        public Nullable<double> KPShare { get; set; }
        public Nullable<double> PunjabHistoric { get; set; }
        public Nullable<double> SindhHistoric { get; set; }
        public Nullable<double> PunjabPara2 { get; set; }
        public Nullable<double> SindhPara2 { get; set; }
        public Nullable<double> Jc7782 { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
