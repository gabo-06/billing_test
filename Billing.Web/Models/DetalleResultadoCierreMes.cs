using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class DetalleResultadoCierreMes
    {
        public string Correlativo { get; set; }
        public string CodigoCaso { get; set; }
        public string PrecioCaso { get; set; }
        public string Paciente { get; set; }
        public string FacNum { get; set; }
        public string CantidadTotalEntradasCaso { get; set; }
        public string CantidadEntradasCasoCerrar { get; set; }
        public string CantidadEntradasRestantes { get; set; }
        public string Comentario { get; set; }
        public string CodigoFactura { get; set; }
    }
}