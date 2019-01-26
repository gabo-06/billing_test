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
    public class PaymentReceivedController : Controller
    {
        public PaymentModel model = new PaymentModel();

        public PartialViewResult Index()//PAYMENT REPORT
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

        // [HttpPost]
        // public PartialViewResult ListarPagosParaEliminar(int CodigoAseguradora = 0)
        // {
        //     List<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result> Resultado;
        //     Resultado = (List<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result>)null;
        // 
        //     try
        //     {
        //         Resultado = model.ListarPagosParaEliminar(CodigoAseguradora);
        //         return PartialView("PagosParaEliminar", Resultado);
        //     }
        //     catch (Exception ex)
        //     {
        //         throw ex;
        //     }
        // }

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
        public PartialViewResult CargarPagos(int CodigoAseguradora = 0
                                           , int CodigoPaciente = 0
                                           , string NumeroCheque = ""
                                           , string Claim = ""
                                           , string NumeroFactura = "")
        {
            List<SP_LIST_PAYMENTS_BY_FILTERS_Result> Resultado;

            Resultado = (List<SP_LIST_PAYMENTS_BY_FILTERS_Result>)null;

            try
            {

                Resultado = model.ListarPagos(CodigoAseguradora
                                              , CodigoPaciente
                                              , NumeroCheque
                                              , Claim
                                              , NumeroFactura);
                return PartialView("Pagos", Resultado);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public PartialViewResult CargarDetallePago(int CodigoPago = 0)
        {
            List<SP_LIST_DETAILT_PAYMENT_Result> Resultado;

            Resultado = (List<SP_LIST_DETAILT_PAYMENT_Result>)null;

            try
            {

                Resultado = model.ListarDetallePago(CodigoPago);
                return PartialView("DetallePago", Resultado);

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

   

      


    }

}