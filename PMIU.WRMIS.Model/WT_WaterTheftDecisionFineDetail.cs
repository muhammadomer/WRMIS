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
    
    public partial class WT_WaterTheftDecisionFineDetail
    {
        public long ID { get; set; }
        public Nullable<long> WaterTheftID { get; set; }
        public string DecisionType { get; set; }
        public Nullable<System.DateTime> DecisionDate { get; set; }
        public Nullable<double> AreaBooked { get; set; }
        public Nullable<long> AreaTypeID { get; set; }
        public Nullable<int> SpecialCharges { get; set; }
        public Nullable<double> ProposedFine { get; set; }
        public long UserID { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual WT_AreaType WT_AreaType { get; set; }
        public virtual WT_WaterTheftCase WT_WaterTheftCase { get; set; }
    }
}
