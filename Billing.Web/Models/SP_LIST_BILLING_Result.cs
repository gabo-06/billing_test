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
    
    public partial class SP_LIST_BILLING_Result
    {
        public Nullable<System.DateTime> Bib_servDate { get; set; }
        public Nullable<int> Act_code { get; set; }
        public string Act_description { get; set; }
        public Nullable<decimal> Bib_hourMile { get; set; }
        public Nullable<decimal> Bib_priceAct { get; set; }
        public Nullable<decimal> Bib_amoReim { get; set; }
        public int Bib_code { get; set; }
        public Nullable<decimal> BalanceDue { get; set; }
    }
}