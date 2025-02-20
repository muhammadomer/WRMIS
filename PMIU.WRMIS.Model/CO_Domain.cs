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
    
    public partial class CO_Domain
    {
        public CO_Domain()
        {
            this.CM_Complaint = new HashSet<CM_Complaint>();
            this.CO_Division = new HashSet<CO_Division>();
            this.CO_StructureType = new HashSet<CO_StructureType>();
            this.TM_TenderNotice = new HashSet<TM_TenderNotice>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<long> OldID { get; set; }
    
        public virtual ICollection<CM_Complaint> CM_Complaint { get; set; }
        public virtual ICollection<CO_Division> CO_Division { get; set; }
        public virtual ICollection<CO_StructureType> CO_StructureType { get; set; }
        public virtual ICollection<TM_TenderNotice> TM_TenderNotice { get; set; }
    }
}
