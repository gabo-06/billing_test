//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Billing.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DataEntry_Audit
    {
        public Nullable<int> Dae_code { get; set; }
        public string Dae_code_old { get; set; }
        public int Cis_code { get; set; }
        public Nullable<System.DateTime> Dae_date { get; set; }
        public int Act_code { get; set; }
        public Nullable<decimal> Dae_hourAct { get; set; }
        public Nullable<decimal> Dae_milesAct { get; set; }
        public string Dae_comment { get; set; }
        public int Use_code { get; set; }
        public Nullable<bool> Dae_invoiceStatus { get; set; }
        public Nullable<System.DateTime> Dae_closedDate { get; set; }
        public Nullable<System.DateTime> Dae_parameterDate { get; set; }
        public Nullable<bool> Dae_deletingStatus { get; set; }
        public Nullable<int> Dae_deletedUser { get; set; }
        public Nullable<System.DateTime> Dae_registerDate { get; set; }
        public Nullable<bool> Dae_facturable { get; set; }
        public Nullable<int> Eve_codigo { get; set; }
        public Nullable<bool> Dae_Call { get; set; }
        public Nullable<int> Dae_facNum { get; set; }
        public Nullable<int> Dae_operatorUser { get; set; }
        public string AuditDataState { get; set; }
        public string AuditDMLAction { get; set; }
        public string AuditUser { get; set; }
        public Nullable<System.DateTime> AuditDateTime { get; set; }
        public string UpdateColumns { get; set; }
    }
}
