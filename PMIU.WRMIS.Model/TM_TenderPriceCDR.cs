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
    
    public partial class TM_TenderPriceCDR
    {
        public long ID { get; set; }
        public Nullable<long> TWContractorID { get; set; }
        public string CDRNo { get; set; }
        public string BankDetail { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Attachment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual TM_TenderWorksContractors TM_TenderWorksContractors { get; set; }
    }
}
