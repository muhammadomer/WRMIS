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
    
    public partial class CW_WorkProgress
    {
        public CW_WorkProgress()
        {
            this.CW_WorkProgressAttachment = new HashSet<CW_WorkProgressAttachment>();
        }
    
        public long ID { get; set; }
        public long ClosureWorkID { get; set; }
        public System.DateTime InspectionDate { get; set; }
        public double ProgressPercentage { get; set; }
        public int WorkStatusID { get; set; }
        public Nullable<int> SiltRemovedQty { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public long CreatedByDesigID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<double> ChannelDesiltedLen { get; set; }
    
        public virtual ICollection<CW_WorkProgressAttachment> CW_WorkProgressAttachment { get; set; }
        public virtual CW_WorkStatus CW_WorkStatus { get; set; }
        public virtual CW_ClosureWork CW_ClosureWork { get; set; }
    }
}
