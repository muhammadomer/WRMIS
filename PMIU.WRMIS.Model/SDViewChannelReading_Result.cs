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
    
    public partial class SDViewChannelReading_Result
    {
        public long SmallChannelID { get; set; }
        public long ID { get; set; }
        public System.DateTime ReadingDate { get; set; }
        public Nullable<double> Capacity { get; set; }
        public Nullable<double> Gauge { get; set; }
        public Nullable<double> Discharge { get; set; }
        public Nullable<System.DateTime> FromTime { get; set; }
        public Nullable<System.DateTime> ToTime { get; set; }
        public string ReaderName { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
