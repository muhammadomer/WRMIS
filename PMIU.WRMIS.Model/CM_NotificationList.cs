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
    
    public partial class CM_NotificationList
    {
        public CM_NotificationList()
        {
            this.CM_ComplaintNotification = new HashSet<CM_ComplaintNotification>();
        }
    
        public long ID { get; set; }
        public Nullable<long> OrgID { get; set; }
        public Nullable<long> DesigID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual ICollection<CM_ComplaintNotification> CM_ComplaintNotification { get; set; }
        public virtual UA_Organization UA_Organization { get; set; }
        public virtual UA_Designations UA_Designations { get; set; }
    }
}
