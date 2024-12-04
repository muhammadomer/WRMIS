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
    
    public partial class TM_TenderWorks
    {
        public TM_TenderWorks()
        {
            this.TM_TenderCommitteeMembers = new HashSet<TM_TenderCommitteeMembers>();
            this.TM_TenderWorksContractors = new HashSet<TM_TenderWorksContractors>();
            this.TM_TenderWorkAttachment = new HashSet<TM_TenderWorkAttachment>();
            this.TM_ADMAttachment = new HashSet<TM_ADMAttachment>();
            this.SI_ScheduleDetailTender = new HashSet<SI_ScheduleDetailTender>();
        }
    
        public long ID { get; set; }
        public long TenderNoticeID { get; set; }
        public string WorkSource { get; set; }
        public long WorkSourceID { get; set; }
        public string OpeningOffice { get; set; }
        public Nullable<long> OpeningOfficeID { get; set; }
        public int WorkStatusID { get; set; }
        public string StatusReason { get; set; }
        public string StatusAttachment { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }
        public Nullable<int> StatusBy { get; set; }
        public string ECA_MonitoredBy { get; set; }
        public string ECA_MonitoredName { get; set; }
        public string ECA_OpenedBy { get; set; }
        public string ECA_Attachment { get; set; }
        public string CA_MonitoredBy { get; set; }
        public string CA_MonitoredName { get; set; }
        public string CA_Attachment { get; set; }
        public Nullable<System.DateTime> ADM_ActualSubmissionDate { get; set; }
        public string ADM_ActualSubmissionReason { get; set; }
        public Nullable<System.DateTime> ADM_ActualOpeningDate { get; set; }
        public string ADM_ActualOpeningReason { get; set; }
        public string ADM_Observation { get; set; }
        public Nullable<bool> ADM_TenderBoxEmpty { get; set; }
        public Nullable<bool> ADM_RateNoted { get; set; }
        public Nullable<bool> ADM_MebersPresent { get; set; }
        public Nullable<bool> ADM_Photography { get; set; }
        public Nullable<bool> ADM_XENRequested { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual ICollection<TM_TenderCommitteeMembers> TM_TenderCommitteeMembers { get; set; }
        public virtual TM_TenderNotice TM_TenderNotice { get; set; }
        public virtual TM_WorkStatus TM_WorkStatus { get; set; }
        public virtual ICollection<TM_TenderWorksContractors> TM_TenderWorksContractors { get; set; }
        public virtual ICollection<TM_TenderWorkAttachment> TM_TenderWorkAttachment { get; set; }
        public virtual ICollection<TM_ADMAttachment> TM_ADMAttachment { get; set; }
        public virtual ICollection<SI_ScheduleDetailTender> SI_ScheduleDetailTender { get; set; }
    }
}
