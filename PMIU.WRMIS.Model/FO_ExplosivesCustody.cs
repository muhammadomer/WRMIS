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
    
    public partial class FO_ExplosivesCustody
    {
        public FO_ExplosivesCustody()
        {
            this.FO_BreachingSectionExplosives = new HashSet<FO_BreachingSectionExplosives>();
        }
    
        public short ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<FO_BreachingSectionExplosives> FO_BreachingSectionExplosives { get; set; }
    }
}
