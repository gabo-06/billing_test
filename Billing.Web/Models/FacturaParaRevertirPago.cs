using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class FacturaParaRevertirPago
    {
        public int CodigoFactura { get; set; }
        public decimal Monto { get; set; }
    }
}