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
    
    public partial class WL_GetDivisionalLGData_Result
    {
        public long SubDivID { get; set; }
        public string SubDiv { get; set; }
        public long ChannelID { get; set; }
        public string ChannelName { get; set; }
        public long SubDivGaugeID { get; set; }
        public Nullable<double> SubDivGaugeDD { get; set; }
        public Nullable<int> SubDivGaugeRD { get; set; }
        public Nullable<int> IsDivisonal { get; set; }
        public string Criteria { get; set; }
        public Nullable<double> SubDivDschrg { get; set; }
        public Nullable<double> Offtakes { get; set; }
        public Nullable<double> DirectOutlet { get; set; }
        public Nullable<double> OfftakeDDSum { get; set; }
        public Nullable<double> OutletDDSum { get; set; }
    }
}
