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
    
    public partial class AC_AuctionOpening
    {
        public AC_AuctionOpening()
        {
            this.AC_AuctionCommiteeMembers = new HashSet<AC_AuctionCommiteeMembers>();
        }
    
        public long ID { get; set; }
        public long AuctionNoticeID { get; set; }
        public string MoniteredBy { get; set; }
        public string MoniteringPerson { get; set; }
        public string OpenedBy { get; set; }
        public string Designation { get; set; }
        public string ACMAttendanceFile { get; set; }
        public string BidderAttendanceFile { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual ICollection<AC_AuctionCommiteeMembers> AC_AuctionCommiteeMembers { get; set; }
        public virtual AC_AuctionNotice AC_AuctionNotice { get; set; }
    }
}
