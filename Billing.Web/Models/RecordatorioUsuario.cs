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
    
    public partial class RecordatorioUsuario
    {
        public int Ret_codigo { get; set; }
        public string Use_code_old { get; set; }
        public int Use_code { get; set; }
    
        public virtual RecordatorioTrigger RecordatorioTrigger { get; set; }
        public virtual User User { get; set; }
    }
}