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
    
    public partial class FO_Drain
    {
        public FO_Drain()
        {
            this.FO_DrainOutfall = new HashSet<FO_DrainOutfall>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<double> TotalLength { get; set; }
        public Nullable<double> CatchmentArea { get; set; }
        public string BuildupArea { get; set; }
        public Nullable<double> DesignedDischarge { get; set; }
        public Nullable<double> BedWidth { get; set; }
        public Nullable<double> FullSupplyDepth { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<long> DrainTypeID { get; set; }
    
        public virtual CO_StructureType CO_StructureType { get; set; }
        public virtual ICollection<FO_DrainOutfall> FO_DrainOutfall { get; set; }
    }
}
