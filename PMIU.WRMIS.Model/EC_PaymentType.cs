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
    
    public partial class EC_PaymentType
    {
        public EC_PaymentType()
        {
            this.EC_ApplicableTaxes = new HashSet<EC_ApplicableTaxes>();
            this.EC_SurchargeAmount = new HashSet<EC_SurchargeAmount>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<EC_ApplicableTaxes> EC_ApplicableTaxes { get; set; }
        public virtual ICollection<EC_SurchargeAmount> EC_SurchargeAmount { get; set; }
    }
}
