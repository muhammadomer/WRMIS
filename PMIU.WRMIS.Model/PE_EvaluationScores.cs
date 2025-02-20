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
    
    public partial class PE_EvaluationScores
    {
        public PE_EvaluationScores()
        {
            this.PE_CategoryWeightage = new HashSet<PE_CategoryWeightage>();
            this.PE_SubCategoryWeightage = new HashSet<PE_SubCategoryWeightage>();
            this.PE_EvaluationReports = new HashSet<PE_EvaluationReports>();
            this.PE_EvaluationScoresDetail = new HashSet<PE_EvaluationScoresDetail>();
        }
    
        public long ID { get; set; }
        public Nullable<long> IrrigationLevelID { get; set; }
        public string Session { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string ReportName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual ICollection<PE_CategoryWeightage> PE_CategoryWeightage { get; set; }
        public virtual ICollection<PE_SubCategoryWeightage> PE_SubCategoryWeightage { get; set; }
        public virtual UA_IrrigationLevel UA_IrrigationLevel { get; set; }
        public virtual ICollection<PE_EvaluationReports> PE_EvaluationReports { get; set; }
        public virtual ICollection<PE_EvaluationScoresDetail> PE_EvaluationScoresDetail { get; set; }
    }
}
