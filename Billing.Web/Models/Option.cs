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
    
    public partial class Option
    {
        public Option()
        {
            this.aspnet_Roles = new HashSet<aspnet_Roles>();
        }
    
        public int Opt_code { get; set; }
        public string Opt_name { get; set; }
        public string Opt_id { get; set; }
        public string Opt_url { get; set; }
        public bool Opt_status { get; set; }
        public string Opt_application { get; set; }
    
        public virtual ICollection<aspnet_Roles> aspnet_Roles { get; set; }
    }
}
