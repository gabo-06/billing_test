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
    
    public partial class PayBody
    {
        public int Pab_code { get; set; }
        public string Pah_code_old { get; set; }
        public Nullable<int> Pah_code { get; set; }
        public string Bih_code_old { get; set; }
        public int Bih_code { get; set; }
        public Nullable<decimal> Pab_amount { get; set; }
    
        public virtual BillingHead BillingHead { get; set; }
        public virtual PayHead PayHead { get; set; }
    }
}