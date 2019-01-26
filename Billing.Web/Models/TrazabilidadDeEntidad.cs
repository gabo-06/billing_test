namespace Billing.Web.Models
{
    public class TrazabilidadDeEntidad
    {
        public int codigoEntidad { get; set; }
        public string campo { get; set; }
        public string antiguoValor { get; set; }
        public string nuevoValor { get; set; }
        public string tipoOperacion { get; set; }
        public string fechaAuditoria { get; set; }
        public string usuarioOperador { get; set; }
    }
}