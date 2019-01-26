using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class CasoBusquedaAvanzada
    { 
        public int CaseCode { get; set; }
        public string ClaimNumber { get; set; }
        public int PatientCode { get; set; }
        public string Patient { get; set; }
        public string Patient2 { get; set; }//apellido
        public string Patient3 { get; set; }//nombre
        public string Insurer { get; set; }
        public string AccidentDate { get; set; }
        public string Status { get; set; }
    }
} 