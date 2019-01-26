using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class FacturaPorPagar
    {
        public int CodigoNuevoFactura { get; set; }
        public decimal Pago { get; set; }
        public string NumeroCheque { get; set; }
    }
}