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
    
    public partial class UA_Roles
    {
        public UA_Roles()
        {
            this.UA_AndroidRoleRights = new HashSet<UA_AndroidRoleRights>();
            this.UA_RoleRights = new HashSet<UA_RoleRights>();
            this.UA_Users = new HashSet<UA_Users>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        public virtual ICollection<UA_AndroidRoleRights> UA_AndroidRoleRights { get; set; }
        public virtual ICollection<UA_RoleRights> UA_RoleRights { get; set; }
        public virtual ICollection<UA_Users> UA_Users { get; set; }
    }
}
