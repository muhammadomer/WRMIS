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
    
    public partial class WT_GetSBESDOWorkingByWaterTheftID_Result
    {
        public long CanalWireID { get; set; }
        public Nullable<long> WaterTheftID { get; set; }
        public Nullable<System.DateTime> ClosingRepairDate { get; set; }
        public string CanalWireNo { get; set; }
        public Nullable<System.DateTime> CanalWireDate { get; set; }
        public long AssignedToDesignationID { get; set; }
        public long AssignedByDesignationID { get; set; }
    }
}
