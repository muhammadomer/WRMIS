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
    
    public partial class WT_WaterTheftCase
    {
        public WT_WaterTheftCase()
        {
            this.WT_ChiefAppealDetails = new HashSet<WT_ChiefAppealDetails>();
            this.WT_OutletDefectiveDetails = new HashSet<WT_OutletDefectiveDetails>();
            this.WT_WaterTheftAttachments = new HashSet<WT_WaterTheftAttachments>();
            this.WT_WaterTheftCanalWire = new HashSet<WT_WaterTheftCanalWire>();
            this.WT_WaterTheftDecisionFineDetail = new HashSet<WT_WaterTheftDecisionFineDetail>();
            this.WT_WaterTheftOffender = new HashSet<WT_WaterTheftOffender>();
            this.WT_WaterTheftPoliceCase = new HashSet<WT_WaterTheftPoliceCase>();
            this.WT_WaterTheftStatus = new HashSet<WT_WaterTheftStatus>();
        }
    
        public long ID { get; set; }
        public string OffenceSite { get; set; }
        public Nullable<long> ChannelID { get; set; }
        public int TheftSiteRD { get; set; }
        public Nullable<long> OutletID { get; set; }
        public Nullable<long> OffenceTypeID { get; set; }
        public string OffenceSide { get; set; }
        public Nullable<long> TheftSiteConditionID { get; set; }
        public System.DateTime IncidentDateTime { get; set; }
        public string SitePhoto { get; set; }
        public string Source { get; set; }
        public Nullable<double> ValueofH { get; set; }
        public long UserID { get; set; }
        public Nullable<System.DateTime> LogDateTime { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> FixDate { get; set; }
        public long CaseStatusID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public string CaseNo { get; set; }
        public Nullable<double> GIS_X { get; set; }
        public Nullable<double> GIS_Y { get; set; }
    
        public virtual CO_Channel CO_Channel { get; set; }
        public virtual ICollection<WT_ChiefAppealDetails> WT_ChiefAppealDetails { get; set; }
        public virtual WT_OffenceType WT_OffenceType { get; set; }
        public virtual ICollection<WT_OutletDefectiveDetails> WT_OutletDefectiveDetails { get; set; }
        public virtual WT_Status WT_Status { get; set; }
        public virtual WT_TheftSiteCondition WT_TheftSiteCondition { get; set; }
        public virtual ICollection<WT_WaterTheftAttachments> WT_WaterTheftAttachments { get; set; }
        public virtual ICollection<WT_WaterTheftCanalWire> WT_WaterTheftCanalWire { get; set; }
        public virtual ICollection<WT_WaterTheftDecisionFineDetail> WT_WaterTheftDecisionFineDetail { get; set; }
        public virtual ICollection<WT_WaterTheftOffender> WT_WaterTheftOffender { get; set; }
        public virtual ICollection<WT_WaterTheftPoliceCase> WT_WaterTheftPoliceCase { get; set; }
        public virtual ICollection<WT_WaterTheftStatus> WT_WaterTheftStatus { get; set; }
        public virtual CO_ChannelOutlets CO_ChannelOutlets { get; set; }
    }
}
