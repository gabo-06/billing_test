using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class PInsurer
    {
        public PInsurer()
        {
            this.CaseInformationSheetHeads = new HashSet<CaseInformationSheetHead>();
        }
    
        public int Ins_code { get; set; }
        public string Ins_code_old { get; set; }
        public string Ins_name { get; set; }
        public string Ins_address { get; set; }
        public string Ins_phone { get; set; }
        public string Ins_fax { get; set; }
        public string Ins_zipCode { get; set; }
        public string Ins_scTpaCode { get; set; }
        public string Ins_feinSc { get; set; }
        public string Ins_carrierCode { get; set; }
        public string Ins_feinCc { get; set; }
        public bool Ins_status { get; set; }
        public string Ins_city { get; set; }
        public string Ins_state { get; set; }
        public string Sta_abbreviation { get; set; }
        public string Ins_zipExt { get; set; }
        public string Ins_phoneExt { get; set; }
    
        public virtual ICollection<CaseInformationSheetHead> CaseInformationSheetHeads { get; set; }
    }
}