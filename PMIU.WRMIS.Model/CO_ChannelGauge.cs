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
    
    public partial class CO_ChannelGauge
    {
        public CO_ChannelGauge()
        {
            this.CO_Attribute = new HashSet<CO_Attribute>();
            this.CO_ChannelDailyGaugeReading = new HashSet<CO_ChannelDailyGaugeReading>();
            this.CO_ChannelIndent = new HashSet<CO_ChannelIndent>();
            this.CO_GaugeLag = new HashSet<CO_GaugeLag>();
            this.SI_ChannelGaugeReading = new HashSet<SI_ChannelGaugeReading>();
            this.SI_ScheduleDetailChannel = new HashSet<SI_ScheduleDetailChannel>();
            this.WL_ChannelLG = new HashSet<WL_ChannelLG>();
            this.WL_ChannelLG1 = new HashSet<WL_ChannelLG>();
            this.WL_SubDivisionLG = new HashSet<WL_SubDivisionLG>();
            this.CO_ChannelGaugeDTPGatedStructure = new HashSet<CO_ChannelGaugeDTPGatedStructure>();
            this.CO_ChannelGaugeDTPFall = new HashSet<CO_ChannelGaugeDTPFall>();
            this.CO_GaugeSlipSite = new HashSet<CO_GaugeSlipSite>();
        }
    
        public long ID { get; set; }
        public Nullable<long> ChannelID { get; set; }
        public Nullable<long> SectionID { get; set; }
        public Nullable<long> GaugeTypeID { get; set; }
        public long GaugeCategoryID { get; set; }
        public double GaugeAtRD { get; set; }
        public Nullable<double> DesignDischarge { get; set; }
        public long GaugeLevelID { get; set; }
        public Nullable<bool> IsSubDivGauge { get; set; }
        public Nullable<bool> IsDivGauge { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<long> GaugeDivID { get; set; }
        public Nullable<long> GaugeSubDivID { get; set; }
        public Nullable<long> GaugeSecID { get; set; }
        public string Alias { get; set; }
        public Nullable<bool> IsRTFM { get; set; }
    
        public virtual ICollection<CO_Attribute> CO_Attribute { get; set; }
        public virtual CO_Channel CO_Channel { get; set; }
        public virtual ICollection<CO_ChannelDailyGaugeReading> CO_ChannelDailyGaugeReading { get; set; }
        public virtual ICollection<CO_ChannelIndent> CO_ChannelIndent { get; set; }
        public virtual ICollection<CO_GaugeLag> CO_GaugeLag { get; set; }
        public virtual ICollection<SI_ChannelGaugeReading> SI_ChannelGaugeReading { get; set; }
        public virtual ICollection<SI_ScheduleDetailChannel> SI_ScheduleDetailChannel { get; set; }
        public virtual ICollection<WL_ChannelLG> WL_ChannelLG { get; set; }
        public virtual ICollection<WL_ChannelLG> WL_ChannelLG1 { get; set; }
        public virtual ICollection<WL_SubDivisionLG> WL_SubDivisionLG { get; set; }
        public virtual CO_Division CO_Division { get; set; }
        public virtual CO_Section CO_Section { get; set; }
        public virtual CO_SubDivision CO_SubDivision { get; set; }
        public virtual ICollection<CO_ChannelGaugeDTPGatedStructure> CO_ChannelGaugeDTPGatedStructure { get; set; }
        public virtual CO_Section CO_Section1 { get; set; }
        public virtual ICollection<CO_ChannelGaugeDTPFall> CO_ChannelGaugeDTPFall { get; set; }
        public virtual CO_GaugeCategory CO_GaugeCategory { get; set; }
        public virtual CO_GaugeLevel CO_GaugeLevel { get; set; }
        public virtual ICollection<CO_GaugeSlipSite> CO_GaugeSlipSite { get; set; }
        public virtual CO_GaugeType CO_GaugeType { get; set; }
    }
}
