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
    
    public partial class SP_ForecastDraft
    {
        public SP_ForecastDraft()
        {
            this.SP_ForecastScenario = new HashSet<SP_ForecastScenario>();
            this.SP_ForecastScenario1 = new HashSet<SP_ForecastScenario>();
            this.SP_PlanDraft = new HashSet<SP_PlanDraft>();
        }
    
        public long ID { get; set; }
        public string Description { get; set; }
        public Nullable<short> DraftType { get; set; }
        public Nullable<short> SeasonID { get; set; }
        public Nullable<short> Year { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> IsValid { get; set; }
    
        public virtual ICollection<SP_ForecastScenario> SP_ForecastScenario { get; set; }
        public virtual ICollection<SP_ForecastScenario> SP_ForecastScenario1 { get; set; }
        public virtual ICollection<SP_PlanDraft> SP_PlanDraft { get; set; }
    }
}
