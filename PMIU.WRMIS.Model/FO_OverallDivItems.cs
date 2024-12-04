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
    
    public partial class FO_OverallDivItems
    {
        public FO_OverallDivItems()
        {
            this.FO_OMCampSiteItems = new HashSet<FO_OMCampSiteItems>();
        }
    
        public long ID { get; set; }
        public long DivisionID { get; set; }
        public Nullable<short> ItemCategoryID { get; set; }
        public Nullable<long> ItemSubcategoryID { get; set; }
        public long StructureTypeID { get; set; }
        public long StructureID { get; set; }
        public Nullable<long> PreMBStatusID { get; set; }
        public Nullable<int> PostAvailableQty { get; set; }
        public Nullable<int> PostRequiredQty { get; set; }
        public Nullable<long> CS_CampSiteID { get; set; }
        public Nullable<int> CS_RequiredQty { get; set; }
        public Nullable<int> OD_AdditionalQty { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string Year { get; set; }
        public Nullable<long> FloodInspectionDetailID { get; set; }
    
        public virtual CO_Division CO_Division { get; set; }
        public virtual FO_FFPCampSites FO_FFPCampSites { get; set; }
        public virtual FO_ItemCategory FO_ItemCategory { get; set; }
        public virtual FO_PreMBStatus FO_PreMBStatus { get; set; }
        public virtual ICollection<FO_OMCampSiteItems> FO_OMCampSiteItems { get; set; }
        public virtual FO_FloodInspectionDetail FO_FloodInspectionDetail { get; set; }
    }
}
