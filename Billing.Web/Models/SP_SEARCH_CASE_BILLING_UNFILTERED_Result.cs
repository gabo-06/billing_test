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
    
    public partial class SP_SEARCH_CASE_BILLING_UNFILTERED_Result
    {
        public int Cis_code { get; set; }
        public string Cis_caseCode { get; set; }
        public int Pat_code { get; set; }
        public string Patient { get; set; }
        public string Pat_firstName { get; set; }
        public string Pat_lastName { get; set; }
        public string Insurer { get; set; }
        public Nullable<int> Bih_code { get; set; }
        public string Bih_code_old { get; set; }
        public System.DateTime Bih_billDate { get; set; }
        public string Bih_closingDate { get; set; }
        public string AccidentDate { get; set; }
        public bool Bih_payStatus { get; set; }
    }
}