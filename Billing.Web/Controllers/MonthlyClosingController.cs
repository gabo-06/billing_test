using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Controllers
{
    public class MonthlyClosingController : Controller
    {
        public MonthlyClosingModel model = new MonthlyClosingModel();
        private decimal AcumuladoUnidad = 0;

        // ----------------------------------------------------------

        // Método que lista la vista parcial de "Monthly Closing".
        public PartialViewResult Index()
        {
            return PartialView();    
        }

        // Método que lista la vista parcial de casos, según el tipo de aseguradora ("No Carlisle" ó "Carlisle") así como el tipo de cierre que se seleccione ("Cierre por Hora" o "Cierre por fecha").
        // Puede recibir tres parámetros (1: "No Carlisle", 2: "Carlisle por hora", 3:"Carlisle por fecha").
        [HttpPost]        
        public PartialViewResult ListaCasos(int TipoCarga = 0)
        {
            List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.

            try
            {
                if (TipoCarga == 1)                             // Es Para listar los casos que NO TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora.
                    Casos = model.ListaCasosNoCarlisle();
                else if (TipoCarga == 2)                        // Es Para listar los casos que TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora (Tipo de cierre: Por Hora).
                    Casos = model.ListaCasosCarlislePorHora();
                else if (TipoCarga == 3)                        // Es Para listar los casos que TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora (Tipo de cierre: Por Fecha).
                    Casos = model.ListaCasosCarlislePorFecha();

                return PartialView("CasosCierreMes", Casos);
            }
            catch (Exception ex) 
            { 
                throw ex;
            }
        }
        
        [HttpPost]
        public JsonResult ObtenerUltimaFechaCierre(int CodigoCaso = 0)
        {
           // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.

            string Fecha;

            try
            {
                Fecha = model.ObtenerUltimaFechaCierre(CodigoCaso);
                return Json(Fecha, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult RegistraCabeceraFactura(int CodigoCaso = 0
                                                 , int CodigoParametro = 0
                                                 , int CodigoUsuario = 0
                                                 , string FechaDeCierre = ""
                                                 , string NumFac = "")
        {
            int CodigoFacturaRegistrada = 0;

            try
            {
                CodigoFacturaRegistrada = model.RegistraCabeceraFactura(CodigoCaso, CodigoParametro, CodigoUsuario, FechaDeCierre, NumFac);
                return Json(CodigoFacturaRegistrada, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        [HttpPost]
        public JsonResult RegistraDetalleCuerpoFactura(int CodigoCabeceraFactura = 0
                                        , string FechaEntrada = ""
                                        , string CodigoMMC = "" // "MMC"
                                        , int CodigoActividad = 0
                                        , decimal Unidad = 0
                                        , decimal Precio = 0
                                        , int CodigoCaso = 0
                                        , char TipoActividad = '\0'
                                        , decimal AmoReim = 0
                                        , int CodigoUsuario = 0
                                        , int CodigoEntrada = 0)
        {
            int CodigoDetalleFacturaRegistrado = 0;

            try
            {
                
                CodigoDetalleFacturaRegistrado = model.RegistraDetalleCuerpoFactura(CodigoCabeceraFactura
                                                                                , FechaEntrada
                                                                                , CodigoMMC
                                                                                , CodigoActividad
                                                                                , Unidad
                                                                                , Precio
                                                                                , CodigoCaso
                                                                                , TipoActividad
                                                                                , AmoReim
                                                                                , CodigoUsuario
                                                                                , CodigoEntrada);

                AcumuladoUnidad += Unidad;
                Debug.WriteLine(AcumuladoUnidad);

                
                return Json(CodigoDetalleFacturaRegistrado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult TotalFactura(int CodigoFacturaGenerada = 0)
        {
            float[] Arreglo; 

            try
            {
                Arreglo = model.TotalFactura(CodigoFacturaGenerada);

                if (Arreglo.Count() > 0)
                    return Json(new { response = new { Total = Arreglo[0], Amount = Arreglo[1] } });
                else
                    return Json(new { response = 0 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public void ActualizaTotalFactura(int CodigoFacturaGenerada = 0, decimal Total = 0)
        {
            try
            {
                model.ActualizaTotalFactura(CodigoFacturaGenerada, Total);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public void GeneraFactura(int CodigoCaso = 0, string FechaDeCierre = "", string NumFac = "")
        {
            try
            {
                model.GeneraFactura(CodigoCaso, FechaDeCierre, NumFac);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult ObtenerUltimasFechaCierreReversion()
        {
            List<UltimaFechaCierreReversion> Fechas;

            try
            {
                Fechas = model.ObtenerUltimasFechaCierreReversion();
                return PartialView("UltimasFechasCierreReversion", Fechas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public PartialViewResult ListarBillingPorFechaDeCierre(String FechaDeCierre = "")
        // public JsonResult ListarBillingPorFechaDeCierre(string FechaDeCierre = "")
        {
            List<FacturaParaRevertir> Facturas;

            try
            {
                Facturas = model.ListarBillingPorFechaDeCierre(FechaDeCierre);
                ViewBag.fechaBusqueda =  FechaDeCierre ; 
                // return Json(Facturas, JsonRequestBehavior.AllowGet);
                ViewBag.fechaBusqueda =Convert.ToDateTime(FechaDeCierre).Date;
                return PartialView("FacturasParaReversion", Facturas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        public PartialViewResult VistaCasosReversion()
        {
            List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.

            try
            {
                if (TipoCarga == 1)                             // Es Para listar los casos que NO TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora.
                    Casos = model.ListaCasosNoCarlisle();
                else if (TipoCarga == 2)                        // Es Para listar los casos que TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora (Tipo de cierre: Por Hora).
                    Casos = model.ListaCasosCarlislePorHora();
                else if (TipoCarga == 3)                        // Es Para listar los casos que TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora (Tipo de cierre: Por Fecha).
                    Casos = model.ListaCasosCarlislePorFecha();

                return PartialView("CasosCierreMes", Casos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        [HttpPost]
        public void RevertirFactura(int CodigoFactura = 0, int CodigoUsuario = 0)
        {
            try
            {
                model.RevertirFactura(CodigoFactura, CodigoUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        // public void PruebaCierreMes(string ListaCorrelativosSeleccionados = "")
        public JsonResult PruebaCierreMes(string FechaDeCierre = ""
                                   ,string ListaCorrelativosSeleccionados = ""
                                   ,int TipoCierre = 0
                                   ,int CodigoUsuario = 0)
        {
            List<DetalleResultadoCierreMes> Lista;

            try
            {
                Lista = model.PruebaCierreMes(FechaDeCierre
                                             ,ListaCorrelativosSeleccionados
                                             ,TipoCierre
                                             ,CodigoUsuario);

                return Json(Lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
