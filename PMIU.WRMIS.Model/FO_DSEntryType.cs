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
    
    public partial class FO_DSEntryType
    {
        public FO_DSEntryType()
        {
            this.FO_DivisionStore = new HashSet<FO_DivisionStore>();
        }
    
        public short ID { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<FO_DivisionStore> FO_DivisionStore { get; set; }
    }
}
