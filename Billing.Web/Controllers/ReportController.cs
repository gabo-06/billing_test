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

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;

using System.IO;
using System.Web.UI;

using Rotativa.Options;
using Rotativa;


namespace Billing.Web.Controllers
{
    public class ReportController : Controller
    {

        
        public ReportModel ReportModel = new ReportModel();
        public CaseModel CaseModel = new CaseModel();
        //public ReportsModel modelDataEntry = new ReportsModel();
        public BillingModel BillingModel = new BillingModel();

        
        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;        
        

        public ReportController()
        { 
            /*
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
            */
        }


        public PartialViewResult Index()
        {
            return PartialView();
        }
        public PartialViewResult DataEntry()
        {
            return PartialView("../ReportDataEntry/index");
        }
        public PartialViewResult Adjuster()
        {
            return PartialView("../ReportAdjuster/index");
        }

        public PartialViewResult ReferralDate()
        {
            return PartialView("../ReportReferralDate/index");
        }

        public PartialViewResult Physical()
        {
            return PartialView("../ReportPhysicalTherapy/index");
        }
        public PartialViewResult Translation()
        {
            return PartialView("../ReportTranslation/index");
        }
        public PartialViewResult Transportation()
        {
            return PartialView("../ReportTransportation/index");
        }
        public PartialViewResult DataEntryPerUser()
        {
            return PartialView("../ReportDataEntryPerUser/index");
        }
        public PartialViewResult Closing()
        {
            return PartialView("../ReportClosing/index");
        }
        public PartialViewResult OpenCasesBySupervisor()
        {
            return PartialView("../ReportOpenCaseBySupervisor/index");
        }
        public PartialViewResult PresumptionCases()
        {
            return PartialView("../ReportPresumptionCases/index");
        }

        public PartialViewResult OpenCasesByInsurer()
        {
            return PartialView("../ReportOpenCaseByInsurer/index");
        }

        public PartialViewResult CasesByInsurer()
        {
            return PartialView("../ReportCaseByInsurer/index");
        }

        public PartialViewResult ReferralBySupervisor()
        {
            return PartialView("../ReportReferralSupervisor/index");
        }
        public PartialViewResult MonthlyBillingInsurer()
        {
            return PartialView("../ReportBillingByInsurer/index");
        }
        public PartialViewResult CaseActivity()
        {
            List<ReporteCasosSinActividad> resultado;

            try
            {
                resultado = ReportModel.reporteDeCasosSinActividad();
                return PartialView("../ReportCaseActivity/index", resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        


        ///////////////////////////////////PRUEBA PDF///////////////////////////////////////////////////
        public ActionResult ExportToPDF()
        {
            ////////Original
            List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.


            string strBody = string.Empty;
            strBody = @"<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
            "xmlns:w='urn:schemas-microsoft-com:office:word'" +
            "xmlns='http://www.w3.org/TR/REC-html40'>";

            strBody = strBody + "<!--[if gte mso 9]>" +
            "<xml>" +
            "<w:WordDocument>" +
            "<w:View>Print</w:View>" +
            "<w:Zoom>100</w:Zoom>" +
            "</w:WordDocument>" +
            "</xml>" +
            "<![endif]-->";


            string htmlString = this.RenderRazorViewToString(@"listaAdjuster_reporte", Casos);
            htmlString = strBody + htmlString;
            Response.Clear();
            Response.Charset = "";
            //Response.ContentType = "application/msword";
            Response.ContentType = "application/vnd.ms-word";
            Response.AddHeader("Content-Disposition", "inline;filename=EFFReport.doc");
            Response.Write(htmlString);
            return null;
            ////////FIN Original
        }
        public ActionResult ExportToPDF_CORRECTO_PARA_OTROS(String invoice = "", String DOI = "", String patient = "")
        {

            string strBody = string.Empty;


            List<DetalleFactura> DetalleDeFactura = new List<DetalleFactura>();
            DetalleDeFactura = BillingModel.CargaDetalleFactura(invoice);


            strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 30px;font-family: sans-serif;'>BILLING REPORT</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:100px;'>Invoice EDHER #</td><td style='width:310px;'>" + invoice + "</td>";
            strBody = strBody + "<td style='font-weight:bold;width:100px;'>Patient</td><td style='width:325px;'>" + patient + "</td>";
            strBody = strBody + "<td style='font-weight:bold;width:100px;'>DOI</td><td >" + DOI + "</td></tr></table>";


             var htmlString = this._HtmlViewRenderer.RenderViewToString(this, "Billing_List_word", DetalleDeFactura);
            htmlString = strBody + htmlString;
             byte[] buffer = StandardPdfRenderer.Render(htmlString, "REPORTE TEST");

            return File(buffer, "application/pdf", "mypdf.pdf");
            ////////FIN Original
        }


        ///////////////////////////////////FIN PRUEBA PDF///////////////////////////////////////////////////


        /////////////////////////////////////PRUEBA DOC/////////////////////////////////////////////////////



        public ActionResult ExportToWordORIGINAL(String invoice = "", String DOI = "", String patient = "")
        {


            string strBody = string.Empty;
            strBody = @"<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
            "xmlns:w='urn:schemas-microsoft-com:office:word'" +
            "xmlns='http://www.w3.org/TR/REC-html40'>";

            strBody = strBody + "<!--[if gte mso 9]>" +
            "<xml>" +
            "<w:WordDocument>" +
            "<w:View>Print</w:View>" +
            "<w:Zoom>100</w:Zoom>" +
            "</w:WordDocument>" +
            "</xml>" +
            "<![endif]-->";



            strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 30px;font-family: sans-serif;'>BILLING REPORT</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:100px;'>Invoice #</td><td style='width:310px;'>" + invoice + "</td>";
            strBody = strBody + "<td style='font-weight:bold;width:100px;'>Patient</td><td style='width:325px;'>" + patient + "</td>";
            strBody = strBody + "<td style='font-weight:bold;width:100px;'>DOI</td><td >" + DOI + "</td></tr></table>";

            ////////Original
            // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.
            //CargaDetalleFactura

            List<DetalleFactura> DetalleDeFactura;
            DetalleDeFactura = BillingModel.CargaDetalleFactura(invoice);
            ViewBag.prueba = "test";


            string htmlString = this.RenderRazorViewToString(@"Billing_List_word", DetalleDeFactura);
            htmlString = strBody + htmlString;
            Response.Clear();
            Response.Charset = "";
            //Response.ContentType = "application/msword";
            Response.ContentType = "application/vnd.ms-word";
            Response.AddHeader("Content-Disposition", "inline;filename=EFFReport.doc");
            Response.Write(htmlString);
            return null;
            ////////FIN Original

        }


    
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        ///////////////////////////////////FIN PRUEBA DOC///////////////////////////////////////////////////

        




        // Método que lista los casos para una búsqueda avanzada.
   /*     public PartialViewResult ListaCasosParaBusquedaAvanzada()
        {
            List<CasoBusquedaAvanzada> Casos;

            try
            {
                Casos = modelCase.ListaCasosParaBusquedaAvanzada();
                return PartialView("CasosParaBusquedaAvanzada", Casos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        // Buscar las entradas  correspondientes a un caso.
    /*    [HttpPost]
        public JsonResult ListaEntradasDeCaso(int CodigoCaso = 0)
        {
            List<PDataEntry> Entradas = new List<PDataEntry>();

            try
            {
                Entradas = modelDataEntry.ListaEntradasDeCaso(CodigoCaso);
                return Json(Entradas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        // Actualiza las entradas de un caso seleccionado.
 /*       [HttpPost]
        public JsonResult RegistraActualizaEntradas(List<PDataEntry> Entradas)
        {
            string EntradasXML;
            int Indicador;

            try
            {
                EntradasXML = "";

                // Armado del XML.
                EntradasXML += "<Entries>";
                foreach (var LectorEntrada in Entradas)
                {
                    EntradasXML += "<Entry>";
                    EntradasXML += "<Cis_code>" + LectorEntrada.Cis_code + "</Cis_code>";
                    EntradasXML += "<Dae_code>" + LectorEntrada.Dae_code + "</Dae_code>";
                    EntradasXML += "<Dae_code_old>" + LectorEntrada.Dae_code_old + "</Dae_code_old>";
                    EntradasXML += "<Dae_date>" + LectorEntrada.Dae_date + "</Dae_date>";
                    EntradasXML += "<Act_code>" + LectorEntrada.Act_code + "</Act_code>";
                    EntradasXML += "<Dae_hourAct>" + LectorEntrada.Dae_hourAct + "</Dae_hourAct>";
                    EntradasXML += "<Dae_milesAct>" + LectorEntrada.Dae_milesAct + "</Dae_milesAct>";
                    EntradasXML += "<Dae_comment>" + LectorEntrada.Dae_comment + "</Dae_comment>";
                    EntradasXML += "<Use_code>" + LectorEntrada.Use_code + "</Use_code>";
                    EntradasXML += "</Entry>";

                    //EntradasXML += "\n\t<Entry>";
                    //EntradasXML += "\n\t\t<Cis_code>" + LectorEntrada.Cis_code + "</Cis_code>";
                    //EntradasXML += "\n\t\t<Dae_code>" + LectorEntrada.Dae_code + "</Dae_code>";
                    //EntradasXML += "\n\t\t<Dae_code_old>" + LectorEntrada.Dae_code_old + "</Dae_code_old>";
                    //EntradasXML += "\n\t\t<Dae_date>" + LectorEntrada.Dae_date + "</Dae_date>";
                    //EntradasXML += "\n\t\t<Act_code>" + LectorEntrada.Act_code + "</Act_code>";
                    //EntradasXML += "\n\t\t<Dae_hourAct>" + LectorEntrada.Dae_hourAct + "</Dae_hourAct>";
                    //EntradasXML += "\n\t\t<Dae_milesAct>" + LectorEntrada.Dae_milesAct + "</Dae_milesAct>";
                    //EntradasXML += "\n\t\t<Dae_comment>" + LectorEntrada.Dae_comment + "</Dae_comment>";
                    //EntradasXML += "\n\t\t<Use_code>" + LectorEntrada.Use_code + "</Use_code>";
                    //EntradasXML += "\n\t</Entry>";
                } 
                EntradasXML += "</Entries>"; 
                //EntradasXML += "\n</Entries>";

                Indicador = modelDataEntry.RegistraActualizaEntradas(EntradasXML);
                return Json(Indicador, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

/*        [HttpPost]
        public JsonResult EliminaEntrada(int CodigoEntrada, int CodigoUsuario)
        {
            int Resultado;

            Resultado = 0;

            try
            {
                Resultado = modelDataEntry.EliminaEntrada(CodigoEntrada, CodigoUsuario);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/
        
        ///////////////////////////////////////////////////// TEST ///////////////////////////////////////////////
        /*
        public ActionResult TestPartialViewWithModel(string id)
        {
            return new PartialViewAsPdf("../ReportAdjuster/listaAdjuster_reporte");
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        */


        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult BuscarEntradasPorPaciente(string ApelliidoPaciente = "")
        {
            List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> Resultado;

            Resultado = (List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>)null;

            try
            {
                Resultado = ReportModel.BuscarEntradasPorPaciente(ApelliidoPaciente);
                return View(Resultado);
                // return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult ReporteVistaEntradasPorPaciente(int CodigoCaso = 0)
        {
            List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> Resultado;

            Resultado = (List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result>)null;

            try
            {
                Resultado = ReportModel.ReporteVistaEntradasPorPaciente(CodigoCaso);
                return View(Resultado);
                // return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
