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
    
    public partial class WT_BreachAttachments
    {
        public long ID { get; set; }
        public long BreachID { get; set; }
        public string FileType { get; set; }
        public string AttachmentPath { get; set; }
        public long AttachmentByUserID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<long> AttachmentByDesignationID { get; set; }
        public string FileName { get; set; }
    
        public virtual WT_Breach WT_Breach { get; set; }
    }
}
