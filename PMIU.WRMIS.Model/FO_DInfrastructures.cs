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
    
    public partial class FO_DInfrastructures
    {
        public long ID { get; set; }
        public Nullable<long> FloodInspectionID { get; set; }
        public Nullable<long> StructureTypeID { get; set; }
        public Nullable<long> StructureID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual CO_StructureType CO_StructureType { get; set; }
        public virtual FO_FloodInspection FO_FloodInspection { get; set; }
    }
}
