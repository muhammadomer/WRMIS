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
    
    public partial class CO_BarrageDailyDataDetail
    {
        public long ID { get; set; }
        public long BarrageDailyDataID { get; set; }
        public long GaugeReaderID { get; set; }
        public long AttributeID { get; set; }
        public double AttributeValue { get; set; }
        public Nullable<double> AttributeDischarge { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<long> DesignationID { get; set; }
        public string ReasonforChange { get; set; }
    
        public virtual CO_Attribute CO_Attribute { get; set; }
        public virtual CO_BarrageDailyData CO_BarrageDailyData { get; set; }
        public virtual UA_Users UA_Users { get; set; }
    }
}
