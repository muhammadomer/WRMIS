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
    
    public partial class SI_ScheduleDetailWorks
    {
        public long ID { get; set; }
        public long ScheduleID { get; set; }
        public long DivisionID { get; set; }
        public string WorkSource { get; set; }
        public long WorkSourceID { get; set; }
        public System.DateTime MonitoringDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<long> RefMonitoringID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual CO_Division CO_Division { get; set; }
        public virtual SI_Schedule SI_Schedule { get; set; }
    }
}
