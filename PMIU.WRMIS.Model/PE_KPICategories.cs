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
    
    public partial class PE_KPICategories
    {
        public PE_KPICategories()
        {
            this.PE_KPISubCategories = new HashSet<PE_KPISubCategories>();
            this.PE_CategoryWeightage = new HashSet<PE_CategoryWeightage>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> PEInclude { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual ICollection<PE_KPISubCategories> PE_KPISubCategories { get; set; }
        public virtual ICollection<PE_CategoryWeightage> PE_CategoryWeightage { get; set; }
    }
}
