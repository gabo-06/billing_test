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
    
    public partial class PayHead
    {
        public PayHead()
        {
            this.PayBodies = new HashSet<PayBody>();
        }
    
        public int Pah_code { get; set; }
        public string Pah_code_old { get; set; }
        public System.DateTime Pah_payDate { get; set; }
        public Nullable<decimal> Pah_amount { get; set; }
        public int Pty_code { get; set; }
        public string Pty_code_old { get; set; }
        public string Pah_number { get; set; }
        public int Use_code { get; set; }
        public string Use_code_old { get; set; }
    
        public virtual ICollection<PayBody> PayBodies { get; set; }
        public virtual PayType PayType { get; set; }
        public virtual User User { get; set; }
    }
}
