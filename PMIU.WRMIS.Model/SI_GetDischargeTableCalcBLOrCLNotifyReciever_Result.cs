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
    
    public partial class SI_GetDischargeTableCalcBLOrCLNotifyReciever_Result
    {
        public Nullable<long> UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string MobilePhone { get; set; }
        public long NotificationID { get; set; }
        public long UserConfigID { get; set; }
        public long EventID { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public Nullable<long> ModulesID { get; set; }
        public string SMSTemplate { get; set; }
        public string AlertTemplate { get; set; }
        public Nullable<long> PageID { get; set; }
        public string URLQueryString { get; set; }
        public string EmailTemplateSubject { get; set; }
        public string EmailTemplateBody { get; set; }
        public string CCSMS { get; set; }
        public string CCEmail { get; set; }
        public Nullable<bool> Alert { get; set; }
        public Nullable<bool> SMS { get; set; }
        public Nullable<bool> Email { get; set; }
        public int IsAlertByDefaultEnabled { get; set; }
        public int IsSMSByDefaultEnabled { get; set; }
        public int IsEmailByDefaultEnabled { get; set; }
    }
}
