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
    
    public partial class EventoFile
    {
        public int Evf_codigo { get; set; }
        public string Evf_descripcion { get; set; }
        public bool Evf_vigencia { get; set; }
        public string Evf_nombre { get; set; }
        public int Eve_codigo { get; set; }
        public int Tia_codigo { get; set; }
        public System.DateTime Evf_fecha { get; set; }
        public Nullable<System.DateTime> Evf_fecha_delete { get; set; }
        public string Evf_user_old { get; set; }
        public Nullable<int> Evf_user { get; set; }
        public string Evf_deleteUser_old { get; set; }
        public Nullable<int> Evf_deleteUser { get; set; }
    
        public virtual Evento Evento { get; set; }
        public virtual TipoArchivo TipoArchivo { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
