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
    
    public partial class SI_GeneralInspectionsAttachment
    {
        public long ID { get; set; }
        public long GeneralInspectionsID { get; set; }
        public string Attachment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
    
        public virtual SI_GeneralInspections SI_GeneralInspections { get; set; }
    }
}
