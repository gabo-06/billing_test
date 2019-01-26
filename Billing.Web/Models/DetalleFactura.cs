using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class DetalleFactura
    {
        public string Bib_servDate { get; set; }
        public int Act_code { get; set; }
        public string Act_description { get; set; }
        public decimal Bib_hourMile { get; set; }
        public decimal Bib_priceAct { get; set; }
        public decimal Bib_amoReim { get; set; }
        public int Bib_code { get; set; }
        public decimal BalanceDue { get; set; }
    } 
}