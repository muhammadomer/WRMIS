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
    
    public partial class FO_DivisionStore
    {
        public long ID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public long DivisionID { get; set; }
        public Nullable<short> ItemCategoryID { get; set; }
        public Nullable<long> ItemID { get; set; }
        public Nullable<long> ItemSubcategoryID { get; set; }
        public Nullable<short> EntryTypeID { get; set; }
        public int Quantity { get; set; }
        public Nullable<int> Balance { get; set; }
        public Nullable<long> StructureTypeID { get; set; }
        public Nullable<long> StructureID { get; set; }
        public Nullable<long> FFPCampSitesID { get; set; }
        public string Reason { get; set; }
        public Nullable<long> ReversalRefID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string Year { get; set; }
    
        public virtual AM_SubCategory AM_SubCategory { get; set; }
        public virtual CO_Division CO_Division { get; set; }
        public virtual CO_StructureType CO_StructureType { get; set; }
        public virtual FO_DSEntryType FO_DSEntryType { get; set; }
        public virtual FO_FFPCampSites FO_FFPCampSites { get; set; }
        public virtual FO_ItemCategory FO_ItemCategory { get; set; }
    }
}
