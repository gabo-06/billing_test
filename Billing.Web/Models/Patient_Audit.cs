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
    
    public partial class Patient_Audit
    {
        public Nullable<int> Pat_code { get; set; }
        public string Pat_code_old { get; set; }
        public string Pat_firstName { get; set; }
        public string Pat_lastName { get; set; }
        public string Pat_address { get; set; }
        public string Pat_phone { get; set; }
        public string Pat_fax { get; set; }
        public string Pat_zipCode { get; set; }
        public string Pat_city { get; set; }
        public string Pat_state { get; set; }
        public string Pat_zipCodeExt { get; set; }
        public Nullable<bool> Pat_status { get; set; }
        public byte[] Pat_socialSecurityNumber { get; set; }
        public Nullable<System.DateTime> Pat_birthday { get; set; }
        public string Pat_socialSecurityNumberD { get; set; }
        public string Pat_sex { get; set; }
        public int Pat_operatorUser { get; set; }
        public string AuditDataState { get; set; }
        public string AuditDMLAction { get; set; }
        public string AuditUser { get; set; }
        public Nullable<System.DateTime> AuditDateTime { get; set; }
        public string UpdateColumns { get; set; }
    }
}