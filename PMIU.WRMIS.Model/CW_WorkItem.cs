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
    
    public partial class CW_WorkItem
    {
        public CW_WorkItem()
        {
            this.TM_TenderPrice = new HashSet<TM_TenderPrice>();
        }
    
        public long ID { get; set; }
        public string WorkType { get; set; }
        public long WorkID { get; set; }
        public string ItemDescription { get; set; }
        public long SanctionedQty { get; set; }
        public int TSUnitID { get; set; }
        public double TSRate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual CW_TechnicalSanctionUnit CW_TechnicalSanctionUnit { get; set; }
        public virtual ICollection<TM_TenderPrice> TM_TenderPrice { get; set; }
    }
}
