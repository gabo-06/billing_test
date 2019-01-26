using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class ReporteCasosSinActividad
    {
        // public int codigoDeCaso { get; set; }
        public string Paciente { get; set; }
        public string NewReferral { get; set; }
        public string fechaDeAccidente { get; set; }
        public string aseguradora { get; set; }
        public string fechaReferencial { get; set; }
        // public string fechaDeUltimaEntrada { get; set; }
        public int diasPasadosDesdeUltimaEntrada { get; set; }
        public string supervisor { get; set; }
    }
}