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
    
    public partial class AM_AssetCondition
    {
        public AM_AssetCondition()
        {
            this.AM_AssetInspectLotCondition = new HashSet<AM_AssetInspectLotCondition>();
            this.AM_AssetInspectionInd = new HashSet<AM_AssetInspectionInd>();
        }
    
        public long ID { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        public virtual ICollection<AM_AssetInspectLotCondition> AM_AssetInspectLotCondition { get; set; }
        public virtual ICollection<AM_AssetInspectionInd> AM_AssetInspectionInd { get; set; }
    }
}
