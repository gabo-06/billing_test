//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Billing.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EventoBloqueo
    {
        public int Evb_codigo { get; set; }
        public int Eve_codigo { get; set; }
        public System.DateTime Evb_fechaInicio { get; set; }
        public Nullable<System.DateTime> Evb_fechaFin { get; set; }
        public string Use_code_old { get; set; }
        public int Use_code { get; set; }
        public bool Evb_vigencia { get; set; }
    
        public virtual Evento Evento { get; set; }
        public virtual User User { get; set; }
    }
}
