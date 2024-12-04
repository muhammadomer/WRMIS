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
    
    public partial class CW_ClosureWorkPlan
    {
        public CW_ClosureWorkPlan()
        {
            this.CW_ClosureWork = new HashSet<CW_ClosureWork>();
        }
    
        public long ID { get; set; }
        public Nullable<long> ACPID { get; set; }
        public long DivisionID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
        public Nullable<int> PublishedBy { get; set; }
    
        public virtual CW_AnnualClosureProgram CW_AnnualClosureProgram { get; set; }
        public virtual ICollection<CW_ClosureWork> CW_ClosureWork { get; set; }
    }
}
