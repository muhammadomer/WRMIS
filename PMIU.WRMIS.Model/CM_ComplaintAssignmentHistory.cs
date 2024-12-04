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
    
    public partial class CM_ComplaintAssignmentHistory
    {
        public long ID { get; set; }
        public Nullable<long> ComplaintID { get; set; }
        public Nullable<long> ComplaintStatusID { get; set; }
        public Nullable<long> AssignedByUser { get; set; }
        public Nullable<long> AssignedByDesig { get; set; }
        public Nullable<long> AssignedToUser { get; set; }
        public Nullable<long> AssignedToDesig { get; set; }
        public Nullable<System.DateTime> AssignedDate { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        public virtual CM_Complaint CM_Complaint { get; set; }
        public virtual CM_ComplaintStatus CM_ComplaintStatus { get; set; }
        public virtual UA_Designations UA_Designations { get; set; }
        public virtual UA_Designations UA_Designations1 { get; set; }
        public virtual UA_Users UA_Users { get; set; }
        public virtual UA_Users UA_Users1 { get; set; }
    }
}
