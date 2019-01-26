namespace Billing.Web.Models
{
    using System;
    using System.Collections.Generic;

    public partial class PDataEntry_Lista
    {
        public int Dae_code { get; set; }                                   
        public string Dae_code_old { get; set; }
        // public Nullable<System.DateTime> Dae_date { get; set; }
        public string Dae_date { get; set; }
        public int Act_code { get; set; }
        public string Act_code_old { get; set; }
        public Nullable<decimal> Dae_hourAct { get; set; }
        public Nullable<decimal> Dae_milesAct { get; set; }
        public string Dae_comment { get; set; }
        public decimal Acumulado { get; set; }
        public int Row { get; set; }
        public Nullable<int> Dae_facNum { get; set; }
        public string InicialesUsuario { get; set; }
        public int Eve_codigo { get; set; }


        /////////////////////////////////////////brenher
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        
    }
}