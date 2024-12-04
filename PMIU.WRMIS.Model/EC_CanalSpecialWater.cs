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
    
    public partial class EC_CanalSpecialWater
    {
        public EC_CanalSpecialWater()
        {
            this.EC_SanctionedDischargeSupply = new HashSet<EC_SanctionedDischargeSupply>();
        }
    
        public long ID { get; set; }
        public Nullable<long> IndustryID { get; set; }
        public long DivisionID { get; set; }
        public Nullable<long> ChannelID { get; set; }
        public string SupplyFrom { get; set; }
        public Nullable<long> ChannelOutletID { get; set; }
        public Nullable<int> RD { get; set; }
        public string Side { get; set; }
        public Nullable<long> SupplySourceID { get; set; }
        public Nullable<System.DateTime> InstallationDate { get; set; }
        public Nullable<int> InstallationCost { get; set; }
        public Nullable<System.DateTime> AgreementSignedOn { get; set; }
        public Nullable<System.DateTime> AgreementEndDate { get; set; }
        public string AgreementParties { get; set; }
        public Nullable<bool> InActive { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual EC_DischargeSupplySource EC_DischargeSupplySource { get; set; }
        public virtual EC_Industry EC_Industry { get; set; }
        public virtual ICollection<EC_SanctionedDischargeSupply> EC_SanctionedDischargeSupply { get; set; }
        public virtual CO_Channel CO_Channel { get; set; }
        public virtual CO_ChannelOutlets CO_ChannelOutlets { get; set; }
    }
}
