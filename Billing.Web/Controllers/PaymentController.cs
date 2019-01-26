using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;
using System.Diagnostics;
using System.Data.Objects;
using System.Xml.Serialization;

namespace Billing.Web.Controllers
{
    public class PaymentController : Controller
    {
        public PaymentModel model = new PaymentModel();

        public PartialViewResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult ListarFacturasPorPagar(int CodigoAseguradora = 0)
        {
            List<SP_SEARCH_BILLING_FOR_PAYMENT_Result> Resultado;
            Resultado = (List<SP_SEARCH_BILLING_FOR_PAYMENT_Result>)null;

            try
            {
                Resultado = model.ListarFacturas(CodigoAseguradora);
                ViewBag.fechaInicio = "";
                ViewBag.fechaFin ="";
                return PartialView("FacturasPorPagar", Resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public PartialViewResult ListarFacturasParaEliminar(int CodigoPago = 0)
        {
            List<SP_LIST_DETAILT_PAYMENT_Result> Resultado;
            Resultado = (List<SP_LIST_DETAILT_PAYMENT_Result>)null;

            try
            {
                Resultado = model.ListarDetallePago(CodigoPago);
                return PartialView("FacturasParaEliminar", Resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        [HttpPost]
        public PartialViewResult ListarPagosParaEliminar(int CodigoAseguradora = 0, string NumeroCheque = "")
        {
            List<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result> Resultado;
            Resultado = (List<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result>)null;

            try
            {
                Resultado = model.ListarPagosParaEliminar(CodigoAseguradora, NumeroCheque);
                return PartialView("PagosParaEliminar", Resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListarTiposDePago()
        {
            List<SP_LIST_PAY_TYPE_Result> Resultado;

            try
            {
                Resultado = model.ListarTiposDePago();
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ObtenerDeudaDePago(int CodigoAseguradora = 0)
        {
            decimal Resultado;

            Resultado = 0;

            try
            {
                Resultado = model.ObtenerDeudaDePago(CodigoAseguradora);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost]
        public JsonResult ObtenerCantidadFacturasPorPagar(int CodigoAseguradora = 0)
        {
            int Resultado;

            Resultado = 0;

            try
            {
                Resultado = model.ObtenerCantidadFacturasPorPagar(CodigoAseguradora);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ArmaXML(char TipoPago = '\0'
                                 , decimal Monto = 0
                                 , string FechaPago = ""
                                 , int FormaPago = 0
                                 , string NumeroTransaccion = ""
                                 , int CodigoUsuario = 0
                                 , List<FacturaPorPagar> FacturasPorPagar = (List<FacturaPorPagar>)null)
        {
            
            DataTable Resultado;
            string FacturasPorPagarXML;
            string NuevoXML;

            // Resultado = (List<SP_SAVE_PAY_Result>)null;
            FacturasPorPagarXML = "";
            
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<FacturaPorPagar>));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, FacturasPorPagar);
                    FacturasPorPagarXML = Convert.ToString(textWriter);
                    NuevoXML = FacturasPorPagarXML.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "").Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "").ToString().Trim();

                    Debug.WriteLine(NuevoXML);
                }

                Resultado = model.LlevaXML_A_BD(TipoPago
                                               ,Monto
                                               ,FechaPago
                                               ,FormaPago
                                               ,NumeroTransaccion
                                               ,CodigoUsuario
                                               ,NuevoXML);

                int CodigoErrorPago = (int)Resultado.Rows[0].ItemArray[0];
                string MensajeError = (string)Resultado.Rows[0].ItemArray[1];

                // return Json(Resultado, JsonRequestBehavior.AllowGet);
                return Json(new { CodigoErrorPago = CodigoErrorPago, MensajeError = MensajeError }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public PartialViewResult BuscarPorRangoDeFecha(int CodigoAseguradora = 0,String fechaIn="",String fechaFin="")
        {
            List<SP_SEARCH_BILLING_FOR_PAYMENT_Result> Resultado;
            Resultado = (List<SP_SEARCH_BILLING_FOR_PAYMENT_Result>)null;

            try
            {
                Resultado = model.BuscarPorRangoDeFecha(CodigoAseguradora,fechaIn,fechaFin);
                ViewBag.fechaInicio = fechaIn;
                ViewBag.fechaFin = fechaFin;
                return PartialView("FacturasPorPagar", Resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Este método devuelve un pago, es decir un conjunto de facturas que conforman un pago.
        [HttpPost]
        public JsonResult ListaPago(int CodigoPago = 0)
        {
            List<REMOVE_PAYMENT_Result> Resultado;
            Resultado = (List<REMOVE_PAYMENT_Result>)null;

            try
            {
                Resultado = model.ListaPago(CodigoPago);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Este método recibe las facturas cuyos pagos se van a revertir.
        [HttpPost]
        public void RevierteMontosEnFactura(List<FacturaParaRevertirPago> FacturasParaRevertir = (List<FacturaParaRevertirPago>)null)
        {
            // DataTable Resultado;
            string FacturasParaRevertirXML;
            string NuevoXML;

            FacturasParaRevertirXML = "";

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<FacturaParaRevertirPago>));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, FacturasParaRevertir);
                    FacturasParaRevertirXML = Convert.ToString(textWriter);
                    NuevoXML = FacturasParaRevertirXML.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "").Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "").ToString().Trim();

                    Debug.WriteLine(NuevoXML);
                }

                model.RevierteMontosEnFactura(NuevoXML);

                // int CodigoErrorPago = (int)Resultado.Rows[0].ItemArray[0];
                // string MensajeError = (string)Resultado.Rows[0].ItemArray[1];

                // return Json(Resultado, JsonRequestBehavior.AllowGet);
                // return Json(new { CodigoErrorPago = CodigoErrorPago, MensajeError = MensajeError }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Elimina el pago de la tabla "PayHead" y "PayBody".
        [HttpPost]
        public void EliminaPago(int CodigoPago = 0)
        {
            try
            {
                model.EliminaPago(CodigoPago);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public String Registra_Tipo_Pago_Modal(String tipo_pago ="")
        {
            try
            {
                String dato = model.buscaCoincidenciaTipoPago(tipo_pago);

                if (dato == "" || dato == null)
                {
                     model.registraTipoPago(tipo_pago);
                    dato="save";
                }
                else
                    dato = "Existe";

                return dato;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // Elimina el pago de la tabla "PayHead" y "PayBody".
        [HttpPost]
        public void EliminarTipoDePago(int id = 0)
        {
            try
            {
                model.EliminarTipoDePago(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}