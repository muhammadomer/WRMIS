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
    
    public partial class GetBarrageDailyDischargeData_Result
    {
        public Nullable<System.TimeSpan> Times { get; set; }
        public string DsGauges { get; set; }
        public string TotalDischargeDS { get; set; }
        public string UccHeadGauge { get; set; }
        public string UCC { get; set; }
        public string MRLinkHead { get; set; }
        public string MRLink { get; set; }
        public string TotalDischargeUS { get; set; }
        public string ReasonForChange { get; set; }
        public Nullable<long> ID { get; set; }
    }
}
