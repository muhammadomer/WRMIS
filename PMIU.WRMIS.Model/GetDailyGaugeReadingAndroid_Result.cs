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
    
    public partial class GetDailyGaugeReadingAndroid_Result
    {
        public Nullable<long> ReadingID { get; set; }
        public long GaugeID { get; set; }
        public double GaugeAtRD { get; set; }
        public string GaugeType { get; set; }
        public Nullable<System.DateTime> ReadingTime { get; set; }
        public Nullable<double> GaugeValue { get; set; }
        public Nullable<double> DailyDischarge { get; set; }
        public Nullable<long> ReaderID { get; set; }
        public string ReaderName { get; set; }
        public Nullable<long> DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string GaugePhoto { get; set; }
        public Nullable<bool> Close { get; set; }
        public bool IsCurrent { get; set; }
        public int IsLocked { get; set; }
    }
}
