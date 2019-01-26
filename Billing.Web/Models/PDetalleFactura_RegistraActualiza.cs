namespace Billing.Web.Models
{
    using System;
    using System.Collections.Generic;

    public class PDetalleFactura_RegistraActualiza
    {
        public int Act_code { get; set; }
        public Nullable<int> Bib_code { get; set; } 
        public int Bih_code { get; set; }        
        public Nullable<decimal> Bib_hourMile { get; set; }
        public Nullable<decimal> Bib_priceAct { get; set; }
        public Nullable<decimal> Bib_amoReim { get; set; }
        public string Bib_servDate { get; set; }
        public int NumeroCorrelativoDetalle { get; set; }
        public int Bib_createdUser { get; set; }
    }
}