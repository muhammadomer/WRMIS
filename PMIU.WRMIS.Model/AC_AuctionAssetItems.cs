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
    
    public partial class AC_AuctionAssetItems
    {
        public long ID { get; set; }
        public long AuctionAssetID { get; set; }
        public long AssetItemID { get; set; }
        public Nullable<long> AssetAttributeID { get; set; }
        public Nullable<int> AssetQuantity { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual AM_AssetItems AM_AssetItems { get; set; }
        public virtual AM_AssetAttributes AM_AssetAttributes { get; set; }
        public virtual AC_AuctionAssets AC_AuctionAssets { get; set; }
    }
}
