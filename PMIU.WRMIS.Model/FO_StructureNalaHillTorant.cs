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
    
    public partial class FO_StructureNalaHillTorant
    {
        public long ID { get; set; }
        public string StructureType { get; set; }
        public string StructureName { get; set; }
        public long DivisionID { get; set; }
        public Nullable<long> DistrictID { get; set; }
        public Nullable<long> TehsilID { get; set; }
        public Nullable<long> VillageID { get; set; }
        public Nullable<double> DesignedDischarge { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual CO_District CO_District { get; set; }
        public virtual CO_Division CO_Division { get; set; }
        public virtual CO_Tehsil CO_Tehsil { get; set; }
        public virtual CO_Village CO_Village { get; set; }
    }
}
