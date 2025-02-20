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
    
    public partial class AT_BudgetUtilization
    {
        public long ID { get; set; }
        public string FinancialYear { get; set; }
        public string Month { get; set; }
        public Nullable<long> AccountHeadID { get; set; }
        public Nullable<long> ObjectClassificationID { get; set; }
        public Nullable<double> CurrentExpense { get; set; }
        public Nullable<System.DateTime> ExpenseDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string Description { get; set; }
    
        public virtual AT_AccountsHead AT_AccountsHead { get; set; }
        public virtual AT_ObjectClassification AT_ObjectClassification { get; set; }
    }
}
