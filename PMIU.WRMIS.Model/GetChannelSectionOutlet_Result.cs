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
    
    public partial class GetChannelSectionOutlet_Result
    {
        public long OutletID { get; set; }
        public Nullable<long> ChannelID { get; set; }
        public long SectionID { get; set; }
        public Nullable<int> OutletRD { get; set; }
        public string ChannelSide { get; set; }
        public Nullable<long> OutletTypeID { get; set; }
        public string OutletType { get; set; }
        public double DesignDischarge { get; set; }
        public string OutletStatus { get; set; }
        public int SectionRD { get; set; }
        public Nullable<int> SectionToRD { get; set; }
    }
}
