using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class PAttorney
    {
        public PAttorney()
        {
            this.CaseInformationSheetBodyAtts = new HashSet<CaseInformationSheetBodyAtt>();
        }
    
        public int Att_code { get; set; }
        public string Att_code_old { get; set; }
        public string Att_firstName { get; set; }
        public string Att_lastName { get; set; }
        public string Att_address { get; set; }
        public string Att_phone { get; set; }
        public string Att_fax { get; set; }
        public string Att_assistant { get; set; }
        public int Spe_code { get; set; }
        public bool Att_status { get; set; }
        public string Att_city { get; set; }
        public string Att_state { get; set; }
        public string Sta_abbreviation { get; set; }
        public string Att_zipCode { get; set; }
        public string Att_zipCodeExt { get; set; }
    
        public virtual Specialty Specialty { get; set; }
        public virtual ICollection<CaseInformationSheetBodyAtt> CaseInformationSheetBodyAtts { get; set; }
    }
}