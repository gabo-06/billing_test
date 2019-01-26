using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class FacturaParaRevertir
    {
        public int CodigoFactura { get; set; }
        public int CodigoCaso { get; set; }
        public string Claim { get; set; }
        public string Paciente { get; set; }
        public string NumeroSeguroSocial { get; set; }
        public string Aseguradora { get; set; }
        public string FechaAccidente { get; set; }
        public double TotalFactura { get; set; }
        public double PagoFactura { get; set; }
    }
}