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
    
    public partial class WT_Breach
    {
        public WT_Breach()
        {
            this.WT_BreachAttachments = new HashSet<WT_BreachAttachments>();
        }
    
        public long ID { get; set; }
        public Nullable<long> ChannelID { get; set; }
        public Nullable<long> DivisionID { get; set; }
        public Nullable<int> BreachSiteRD { get; set; }
        public string BreachSide { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string SitePhoto { get; set; }
        public string Source { get; set; }
        public Nullable<double> GIS_X { get; set; }
        public Nullable<double> GIS_Y { get; set; }
        public string LogBy { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<System.DateTime> LogDateTime { get; set; }
        public Nullable<bool> SelectedEntry { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> FixDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<double> BreachLength { get; set; }
        public Nullable<double> HeadDischarge { get; set; }
        public string CaseNo { get; set; }
        public Nullable<bool> FieldStaff { get; set; }
    
        public virtual CO_Division CO_Division { get; set; }
        public virtual ICollection<WT_BreachAttachments> WT_BreachAttachments { get; set; }
        public virtual CO_Channel CO_Channel { get; set; }
    }
}
