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
    
    public partial class SP_LIST_BILLING_BY_CLOSING_DATE_Result
    {
        public int Bih_code { get; set; }
        public int Cis_code { get; set; }
        public string Cis_caseCode { get; set; }
        public string Patient { get; set; }
        public string Pat_socialSecurityNumberD { get; set; }
        public string Insurer { get; set; }
        public Nullable<System.DateTime> AccidentalDate { get; set; }
        public Nullable<decimal> Bih_billTotal { get; set; }
        public Nullable<decimal> Bih_billPay { get; set; }
    }
}
