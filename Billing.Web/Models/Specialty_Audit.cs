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
    
    public partial class Specialty_Audit
    {
        public Nullable<int> Spe_code { get; set; }
        public string Spe_code_old { get; set; }
        public string Spe_name { get; set; }
        public string Spe_type { get; set; }
        public string Spe_description { get; set; }
        public int Spe_operatorUser { get; set; }
        public string AuditDataState { get; set; }
        public string AuditDMLAction { get; set; }
        public string AuditUser { get; set; }
        public Nullable<System.DateTime> AuditDateTime { get; set; }
        public string UpdateColumns { get; set; }
    }
}
