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
    
    public partial class AM_Attachment
    {
        public long ID { get; set; }
        public Nullable<long> AssetInspectionIndID { get; set; }
        public Nullable<long> AssetInspectLotID { get; set; }
        public string Attachment { get; set; }
        public string Source { get; set; }
        public Nullable<double> GIS_X { get; set; }
        public Nullable<double> GIS_Y { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        public virtual AM_AssetInspectionInd AM_AssetInspectionInd { get; set; }
        public virtual AM_AssetInspectionLot AM_AssetInspectionLot { get; set; }
    }
}
