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
    
    public partial class AM_AssetInspectLotCondition
    {
        public long ID { get; set; }
        public long AssetInspectLotID { get; set; }
        public int Quantity { get; set; }
        public long ConditionID { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual AM_AssetCondition AM_AssetCondition { get; set; }
        public virtual AM_AssetInspectionLot AM_AssetInspectionLot { get; set; }
    }
}
