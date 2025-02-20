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
    
    public partial class CO_ChannelReachLSP
    {
        public long ID { get; set; }
        public Nullable<long> ReachID { get; set; }
        public Nullable<System.DateTime> ParameterDate { get; set; }
        public double NaturalSurfaceLevel { get; set; }
        public double AFS { get; set; }
        public double BedLevel { get; set; }
        public double FullSupplyLevel { get; set; }
        public double BedWidth { get; set; }
        public double FullSupplyDepth { get; set; }
        public double SideSlop { get; set; }
        public double SlopeIn { get; set; }
        public double CriticalVelocityRatio { get; set; }
        public double FreeBoard { get; set; }
        public double LeftBankWidth { get; set; }
        public double RightBankWidth { get; set; }
        public Nullable<bool> LinedUnlined { get; set; }
        public Nullable<long> LiningTypeID { get; set; }
        public string LSectionPhoto { get; set; }
        public string RDLocation { get; set; }
        public Nullable<double> MRCoefficient { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual CO_ChannelReach CO_ChannelReach { get; set; }
        public virtual CO_LiningType CO_LiningType { get; set; }
    }
}
