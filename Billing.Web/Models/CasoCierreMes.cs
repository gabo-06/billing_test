using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class CasoCierreMes
    {
        public int Correlativo { get; set; }
        public int Cis_code { get; set; } 
        public string Cis_code_old { get; set; }
        public string Cis_caseCode { get; set; }
        public string Patient { get; set; }
        public string Pat_socialSecurityNumberD { get; set; }
        public string Ins_name { get; set; }
        public string Cis_accidentDate { get; set; }
        public string Hours { get; set; }
        public string Date { get; set; }
        public string Dae_facNum { get; set; }
        public string Cis_price { get; set; } 
    }
} 