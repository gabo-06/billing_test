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
    
    public partial class Adjuster
    {
        public Adjuster()
        {
            this.CaseInformationSheetHeads = new HashSet<CaseInformationSheetHead>();
        }
    
        public int Adj_code { get; set; }
        public string Adj_code_old { get; set; }
        public string Adj_firstName { get; set; }
        public string Adj_lastName { get; set; }
        public string Adj_address { get; set; }
        public string Adj_phone { get; set; }
        public string Adj_fax { get; set; }
        public bool Adj_status { get; set; }
        public string Adj_city { get; set; }
        public string Adj_state { get; set; }
        public string Adj_zipCode { get; set; }
        public string Adj_phoneExt { get; set; }
        public Nullable<int> Adj_operatorUser { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<CaseInformationSheetHead> CaseInformationSheetHeads { get; set; }
    }
}
