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
    
    public partial class TipoArchivo
    {
        public TipoArchivo()
        {
            this.EventoFiles = new HashSet<EventoFile>();
            this.EventoTipoArchivoes = new HashSet<EventoTipoArchivo>();
            this.TipoEventoes = new HashSet<TipoEvento>();
        }
    
        public int Tia_codigo { get; set; }
        public string Tia_nombre { get; set; }
        public string Tia_descripcion { get; set; }
        public Nullable<bool> Tia_vigencia { get; set; }
        public Nullable<byte> Tia_numArchivos { get; set; }
        public Nullable<System.DateTime> Tia_fecha { get; set; }
        public string Use_code_old { get; set; }
        public Nullable<int> Use_code { get; set; }
    
        public virtual ICollection<EventoFile> EventoFiles { get; set; }
        public virtual ICollection<EventoTipoArchivo> EventoTipoArchivoes { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<TipoEvento> TipoEventoes { get; set; }
    }
}
