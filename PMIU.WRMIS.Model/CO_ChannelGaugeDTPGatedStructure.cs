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
    
    public partial class CO_ChannelGaugeDTPGatedStructure
    {
        public long ID { get; set; }
        public Nullable<System.DateTime> ReadingDate { get; set; }
        public Nullable<long> GaugeID { get; set; }
        public double ExponentValue { get; set; }
        public double MeanDepth { get; set; }
        public double DischargeObserved { get; set; }
        public double DischargeCoefficient { get; set; }
        public Nullable<bool> GaugeCorrectionType { get; set; }
        public Nullable<double> GaugeValueCorrection { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<long> ScheduleDetailChannelID { get; set; }
        public string Source { get; set; }
        public Nullable<double> GIS_X { get; set; }
        public Nullable<double> GIS_Y { get; set; }
        public Nullable<System.DateTime> ObservationDate { get; set; }
    
        public virtual SI_ScheduleDetailChannel SI_ScheduleDetailChannel { get; set; }
        public virtual CO_ChannelGauge CO_ChannelGauge { get; set; }
    }
}
