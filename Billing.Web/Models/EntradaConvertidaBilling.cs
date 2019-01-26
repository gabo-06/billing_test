using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class EntradaConvertidaBilling
    {
        public int Dae_code { get; set; }
        public string Dae_date { get; set; }
        public int Act_code { get; set; }
        public double Dae_hourAct { get; set; }
        public double Dae_milesAct { get; set; }
        public string Dae_comment { get; set; }
        public char Aty_price { get; set; }
    }
}