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
    
    public partial class AC_AuctionCommiteeMembers
    {
        public long ID { get; set; }
        public Nullable<long> AuctionNoticeID { get; set; }
        public long AuctionOpeningID { get; set; }
        public string MemberName { get; set; }
        public string Designation { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual AC_AuctionNotice AC_AuctionNotice { get; set; }
        public virtual AC_AuctionOpening AC_AuctionOpening { get; set; }
    }
}
