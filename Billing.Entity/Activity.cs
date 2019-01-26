
using System;
using System.Collections.Generic;
using System.Text;


namespace Billing.Entity
{
    [Serializable]
    public class Activity
    {
        public Int32 Act_code { get; set; }
        public String Act_code_old { get; set; }
        public String Act_description { get; set; }
        public Int32 Aty_code { get; set; }
        public Decimal Act_price { get; set; }
        /*
        public string NombreCompleto
        {
        get{ return string.Format("{0},{1}",Propiedad1,Propiedad2);}
        }
        public override string ToString()
        {
        return string.IsNullOrEmpty(propiedad1) ? propiedad1:Propiedad2;
        }
        */
    }
}

