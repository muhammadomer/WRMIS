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
    
    public partial class CM_ComplaintComments
    {
        public long ID { get; set; }
        public Nullable<long> ComplaintID { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> CommentsDateTime { get; set; }
        public string Attachment { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<long> UserDesigID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    
        public virtual CM_Complaint CM_Complaint { get; set; }
        public virtual UA_Designations UA_Designations { get; set; }
        public virtual UA_Users UA_Users { get; set; }
    }
}
