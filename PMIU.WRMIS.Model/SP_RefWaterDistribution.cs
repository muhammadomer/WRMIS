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
    
    public partial class SP_RefWaterDistribution
    {
        public long ID { get; set; }
        public Nullable<short> TDailyID { get; set; }
        public Nullable<double> Percentile0 { get; set; }
        public Nullable<double> Percentile5 { get; set; }
        public Nullable<double> Percentile10 { get; set; }
        public Nullable<double> Percentile15 { get; set; }
        public Nullable<double> Percentile20 { get; set; }
        public Nullable<double> Percentile25 { get; set; }
        public Nullable<double> Percentile30 { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
