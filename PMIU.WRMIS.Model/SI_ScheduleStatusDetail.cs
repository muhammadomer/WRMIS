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
    
    public partial class SI_ScheduleStatusDetail
    {
        public long ID { get; set; }
        public Nullable<long> SendToUserID { get; set; }
        public Nullable<long> SendToUserDesignationID { get; set; }
        public Nullable<long> ScheduleID { get; set; }
        public long ScheduleStatusID { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> CommentsDate { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<long> DesignationID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
    
        public virtual SI_Schedule SI_Schedule { get; set; }
        public virtual SI_ScheduleStatus SI_ScheduleStatus { get; set; }
        public virtual UA_Designations UA_Designations { get; set; }
        public virtual UA_Designations UA_Designations1 { get; set; }
        public virtual UA_Users UA_Users { get; set; }
        public virtual UA_Users UA_Users1 { get; set; }
    }
}
