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
    
    public partial class AM_AssetWorkProgressAttachment
    {
        public long ID { get; set; }
        public long AssetWorkProgressID { get; set; }
        public string Attachment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        public virtual AM_AssetWorkProgress AM_AssetWorkProgress { get; set; }
    }
}
