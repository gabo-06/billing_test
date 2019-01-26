namespace Billing.Web.Models
{
    using System;
    using System.Collections.Generic;

    public partial class PDataEntry_RegistraActualiza
    {
        // public int NumeroCorrelativoEntrada { get; set; }
        // public int Cis_code { get; set; }
        // public int Dae_code { get; set; }                                    // importa
        // public string Dae_code_old { get; set; }                            // importa
        // public Nullable<System.DateTime> Dae_date { get; set; }             // importa        
        // // public string Dae_date { get; set; }             // importa        
        // public int Act_code { get; set; }                                   // importa
        // public Nullable<decimal> Dae_hourAct { get; set; }                  // importa
        // public Nullable<decimal> Dae_milesAct { get; set; }                 // importa
        // public string Dae_comment { get; set; }                             // importa
        // public int Use_code { get; set; }

        public int NumeroCorrelativoEntrada { get; set; }
        public int CodigoCaso { get; set; }
        public int CodigoNuevoEntrada { get; set; }                                    // importa
        public string CodigoAntiguoEntrada { get; set; }                            // importa
        public Nullable<System.DateTime> FechaEntrada { get; set; }             // importa        
        public int CodigoActividad { get; set; }                                   // importa
        public Nullable<decimal> Unit { get; set; }                  // importa
        public Nullable<decimal> PriceUnit { get; set; }                 // importa
        public string Comentario { get; set; }                             // importa
        public int CodigoUsuarioGlobal { get; set; }
    }
}
