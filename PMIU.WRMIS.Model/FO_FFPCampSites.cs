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
    
    public partial class FO_FFPCampSites
    {
        public FO_FFPCampSites()
        {
            this.FO_OMCampSites = new HashSet<FO_OMCampSites>();
            this.FO_OverallDivItems = new HashSet<FO_OverallDivItems>();
            this.FO_DivisionStore = new HashSet<FO_DivisionStore>();
        }
    
        public long ID { get; set; }
        public long FFPID { get; set; }
        public long StructureTypeID { get; set; }
        public long StructureID { get; set; }
        public int RD { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual CO_StructureType CO_StructureType { get; set; }
        public virtual ICollection<FO_OMCampSites> FO_OMCampSites { get; set; }
        public virtual ICollection<FO_OverallDivItems> FO_OverallDivItems { get; set; }
        public virtual FO_FloodFightingPlan FO_FloodFightingPlan { get; set; }
        public virtual ICollection<FO_DivisionStore> FO_DivisionStore { get; set; }
    }
}
