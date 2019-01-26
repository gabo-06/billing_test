using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class FacturaParaBilling
    {
        public int CodigoCaso { get; set; }
        public string Claim { get; set; }
        public string Paciente { get; set; }
        public string Aseguradora { get; set; }
        public string CodigoFacturaSTR { get; set; }
        public int CodigoFacturaINT { get; set; }
        public string FechaFacturacion { get; set; }
        public string FechaCierre { get; set; }
        public string FechaAccidente { get; set; }
        public Int32 Pat_code { get; set; }
        
        
    }
}