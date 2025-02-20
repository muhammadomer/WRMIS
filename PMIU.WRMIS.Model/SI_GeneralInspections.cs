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
    
    public partial class SI_GeneralInspections
    {
        public SI_GeneralInspections()
        {
            this.SI_GeneralInspectionsAttachment = new HashSet<SI_GeneralInspectionsAttachment>();
        }
    
        public long ID { get; set; }
        public long GeneralInspectionTypeID { get; set; }
        public string InspectionLocation { get; set; }
        public System.DateTime InspectionDate { get; set; }
        public string InspectionDetails { get; set; }
        public string Remarks { get; set; }
        public Nullable<long> ScheduleDetailGeneralID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual ICollection<SI_GeneralInspectionsAttachment> SI_GeneralInspectionsAttachment { get; set; }
        public virtual SI_GeneralInspectionType SI_GeneralInspectionType { get; set; }
        public virtual SI_ScheduleDetailGeneral SI_ScheduleDetailGeneral { get; set; }
    }
}
