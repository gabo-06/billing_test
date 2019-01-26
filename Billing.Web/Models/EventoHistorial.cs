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
    
    public partial class EventoHistorial
    {
        public int Evh_codigo { get; set; }
        public Nullable<int> Eve_codigo { get; set; }
        public Nullable<System.DateTime> Eve_inicio { get; set; }
        public Nullable<System.DateTime> Eve_fin { get; set; }
        public Nullable<System.DateTime> Eve_fecha { get; set; }
        public string Eve_descripcion { get; set; }
        public string Pro_code_old { get; set; }
        public string Med_code_old { get; set; }
        public string Cis_code_old { get; set; }
        public Nullable<int> Tie_codigo { get; set; }
        public Nullable<byte> Ese_codigo { get; set; }
        public Nullable<int> Eve_codigoNext { get; set; }
        public Nullable<System.DateTime> Evh_fecha { get; set; }
        public string Use_code_old { get; set; }
        public string Eve_codExchange { get; set; }
        public bool Eve_descartar { get; set; }
        public Nullable<int> Con_codigo { get; set; }
        public Nullable<bool> Evh_usado { get; set; }
        public Nullable<bool> Evh_override { get; set; }
        public Nullable<int> Mot_codigo { get; set; }
        public Nullable<bool> Eve_done { get; set; }
        public Nullable<bool> Eve_override { get; set; }
        public Nullable<bool> Eve_allDay { get; set; }
        public Nullable<int> Cat_codigo { get; set; }
        public string Evh_undoChangeUser_old { get; set; }
        public Nullable<int> Eve_codigoNov { get; set; }
        public Nullable<bool> Eve_bloqueo { get; set; }
        public Nullable<bool> Eve_nov { get; set; }
        public Nullable<bool> Eve_vigencia { get; set; }
        public string Eve_codExchangeSec { get; set; }
        public Nullable<int> Eve_statusUpdateExc { get; set; }
        public Nullable<int> Pro_code { get; set; }
        public Nullable<int> Med_code { get; set; }
        public Nullable<int> Cis_code { get; set; }
        public Nullable<int> Use_code { get; set; }
        public Nullable<int> Evh_undoChangeUser { get; set; }
    
        public virtual CaseInformationSheetHead CaseInformationSheetHead { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual Confirmacion Confirmacion { get; set; }
        public virtual EstadoEvento EstadoEvento { get; set; }
        public virtual Evento Evento { get; set; }
        public virtual Evento Evento1 { get; set; }
        public virtual Medical Medical { get; set; }
        public virtual MotivoCambioEstado MotivoCambioEstado { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual TipoEvento TipoEvento { get; set; }
        public virtual User User { get; set; }
    }
}
