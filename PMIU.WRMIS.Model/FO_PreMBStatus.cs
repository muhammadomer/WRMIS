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
    
    public partial class FO_PreMBStatus
    {
        public FO_PreMBStatus()
        {
            this.FO_OverallDivItems = new HashSet<FO_OverallDivItems>();
        }
    
        public long ID { get; set; }
        public long FloodInspectionID { get; set; }
        public Nullable<long> ItemID { get; set; }
        public Nullable<int> LastYrQty { get; set; }
        public Nullable<int> DivStrIssuedQty { get; set; }
        public Nullable<int> AvailableQty { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual FO_FloodInspection FO_FloodInspection { get; set; }
        public virtual FO_Items FO_Items { get; set; }
        public virtual ICollection<FO_OverallDivItems> FO_OverallDivItems { get; set; }
    }
}
