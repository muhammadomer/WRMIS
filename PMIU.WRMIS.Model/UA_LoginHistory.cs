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
    
    public partial class UA_LoginHistory
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public Nullable<System.DateTime> LoginDatetime { get; set; }
        public string UserAgent { get; set; }
        public string IPAddress { get; set; }
    
        public virtual UA_Users UA_Users { get; set; }
    }
}
