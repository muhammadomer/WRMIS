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
    
    public partial class FO_InfrastructureStoneStock
    {
        public long ID { get; set; }
        public Nullable<long> ProtectionInfrastructureID { get; set; }
        public Nullable<int> FromRD { get; set; }
        public Nullable<int> ToRD { get; set; }
        public Nullable<long> SanctionedLimit { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual FO_ProtectionInfrastructure FO_ProtectionInfrastructure { get; set; }
    }
}
