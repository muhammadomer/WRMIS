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
    
    public partial class FO_IStonePosition
    {
        public FO_IStonePosition()
        {
            this.FO_FFPStonePosition = new HashSet<FO_FFPStonePosition>();
        }
    
        public long ID { get; set; }
        public long FloodInspectionID { get; set; }
        public int RD { get; set; }
        public int BeforeFloodQty { get; set; }
        public Nullable<int> AvailableQty { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual ICollection<FO_FFPStonePosition> FO_FFPStonePosition { get; set; }
        public virtual FO_FloodInspection FO_FloodInspection { get; set; }
    }
}
