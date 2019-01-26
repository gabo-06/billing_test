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
    public class ReportDataEntryController : Controller
    {
        private List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> CasosGlobalEstatica = new List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>();
        private List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> CasosTemporal = new List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>();
    


        public ReportModel model = new ReportModel();


        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;

        public ReportDataEntryController()
        { 
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }

        public PartialViewResult listaDataEntry(Int32 CodigoCasoSeleccionado = 0, Int32 Bih_code = 0)
        {
            List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> Listado;
            Listado = model.ReporteDataEntry(CodigoCasoSeleccionado, Bih_code);

            return PartialView("lista_reporte", Listado);
        }




        public ActionResult ExportToWordDataEntry(Int32 CodigoCasoSeleccionado = 0, Int32 Bih_code = 0, String paciente = "", String doi = "", String ssnumber = "", String closingDate = "")
        {

            string strBody = string.Empty;
            strBody = @"<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
            "xmlns:w='urn:schemas-microsoft-com:office:word'" + 
            "xmlns='http://www.w3.org/TR/REC-html40'>";

            strBody = strBody + "<!--[if gte mso 9]>" +
            "<xml>" +
            "<w:WordDocument>" +
            "<w:View>Web</w:View>" +
            "<w:Zoom>100</w:Zoom>" +
            "</w:WordDocument>" +
            "</xml>" +
            "<![endif]-->";


            strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;font-size:18;font-family: sans-serif;'>DATA ENTRY REPORT</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:60;'>Patient</td><td style='width:250;text-align:center;'>" + paciente + "</td><td style='font-weight:bold;width:60;'>DOI</td><td>" + doi + "</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:60;'>SSNumber</td><td style='width:250;text-align:center;'>" + ssnumber + "</td><td style='font-weight:bold;width:60;'>Closing Date</td><td>" + closingDate + "</td></tr></table>";
                
            ////////Original
            // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.
            //CargaDetalleFactura

            List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> Listado;
            Listado = model.ReporteDataEntry(CodigoCasoSeleccionado, Bih_code);
            //var fechaActual = DateTime.Today;
            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Data_entry_reports-" + fechaActual; 

            //string htmlString = this.RenderRazorViewToString(@"TestPartialViewWithModel", DetalleDeFactura);
            string htmlString = this.RenderRazorViewToString(@"lista_reporte_word", Listado);
            htmlString = strBody + htmlString;
            Response.Clear();
            Response.Charset = "";
            //Response.ContentType = "application/msword";
            Response.ContentType = "application/vnd.ms-word";
            Response.AddHeader("Content-Disposition", "inline;filename="+ nombre_archivo +".doc");
            Response.Write(htmlString);
            return null;
            ////////FIN Original

        }
        ///////////////////////////////////PRUEBA PDF///////////////////////////////////////////////////
        public ActionResult ExportDataEntry(Int32 CodigoCasoSeleccionado = 0, Int32 Bih_code = 0, String tipo="")
        {
            if (tipo == "P")
                ExportToPdfDataEntry(CodigoCasoSeleccionado);
            else
                ExportToWordDataEntry(CodigoCasoSeleccionado);


                return null;
        }

        public ActionResult ExportToPdfDataEntry(Int32 CodigoCasoSeleccionado = 0,Int32 Bih_code =0, String paciente = "", String doi = "", String ssnumber = "",String closingDate = "")
        {           
            
            string strBody = string.Empty;
            strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;font-size:15;font-family: sans-serif;'>REPORT DATA ENTRY PATIENT´S</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:60;font-size:10;'>Patient</td><td style='width:250;text-align:left;font-size:10;'>" + paciente + "</td><td style='font-weight:bold;width:60;font-size:10;'>DOI</td><td style='font-size:10;'>" + doi + "</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:60;font-size:10;'>SSNumber</td><td style='width:250;text-align:left;font-size:10;'>" + ssnumber + "</td><td style='font-weight:bold;width:60;'font-size:10;>Closing Date</td><td style='font-size:10;'>" + closingDate + "</td></tr></table>";
                
            List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> Listado;
            Listado = model.ReporteDataEntry(CodigoCasoSeleccionado,Bih_code);
            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Data_entry_reports-" + fechaActual; 


               var htmlString = this._HtmlViewRenderer.RenderViewToString(this, "lista_reporte_pdf", Listado);
               htmlString = strBody + htmlString;
               byte[] buffer = StandardPdfRenderer.Render(htmlString, "");

               return File(buffer, "application/pdf", nombre_archivo+".pdf");



            ////////////string htmlString = this.RenderRazorViewToString(@"TestPartialViewWithModel", DetalleDeFactura);
            //////////string htmlString = this.RenderRazorViewToString(@"lista_reporte", Listado);
            //////////htmlString = strBody + htmlString;
            //////////Response.Clear();
            //////////Response.Charset = "";
            ////////////Response.ContentType = "application/msword";
            //////////Response.ContentType = "application/pdf";
            //////////Response.AddHeader("Content-Disposition", "attachment;filename=EFFReport.pdf");
            //////////Response.Write(htmlString);
            //////////return null;
            //////////////////FIN Original
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







     ///////////////////////////////////////////////////////////////////////////////////

        public ActionResult GetData()
        {
            // Initialization. 
            JsonResult result = new JsonResult();
            // List<PPatient> PacientesTemporal = (List<PPatient>)null;

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // List<SalesOrderDetail> data;

                if (CasosGlobalEstatica.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.

                    this.ListaCasosParaBusquedaAvanzada_tabla();

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || CasosTemporal.Count == 0)
                {
                    CasosTemporal = CasosGlobalEstatica;
                }

                
                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = CasosGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    CasosTemporal = CasosGlobalEstatica.Where(p => p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||
                                                                   (p.Pat_firstName.ToString() + p.Pat_lastName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                   (p.Pat_lastName.ToString() + p.Pat_firstName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower())
                                                              ).ToList();
                    //p.Patient2.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                    //p.Patient3.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                    //(p.Patient2.ToString() + p.Patient3.ToString()).Contains(search.Replace(" ", "").ToLower())||
                    //(p.Patient3.ToString() + p.Patient2.ToString()).Contains(search.Replace(" ", "").ToLower())
                    //).ToList();

                    //if (CasosTemporal.Count == 0)
                    //{ 

                    //    (p.Patient3.ToString() + p.Patient2.ToString()).ToLower().Contains(search.Replace(" ","").ToLower()
                    //    ).ToList();
                    //}


                }

                // Sorting.
                CasosTemporal = this.SortByColumnWithOrder(order, orderDir, CasosTemporal);

                // Filter record count.
                int recFilter = CasosTemporal.Count;

                // Apply pagination.
                CasosTemporal = CasosTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = CasosTemporal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.

            // PacientesTemporal = (List<PPatient>)null;
            // PacientesTemporal = new List<PPatient>();
            return result;
        }


        private List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> SortByColumnWithOrder(string order, string orderDir, List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                // Sorting
                ////switch (order)
                ////{
                ////    case "1":
                ////        // Setting.
                ////        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClaimNumber).ToList()
                ////                                                                                 : data.OrderBy(p => p.ClaimNumber).ToList();
                ////        break;
                ////    case "3":
                ////        // Setting.
                ////        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Patient).ToList()
                ////                                                                                 : data.OrderBy(p => p.Patient).ToList();
                ////        break;
                ////    case "5":
                ////        // Setting.
                ////        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Insurer).ToList()
                ////                                                                                 : data.OrderBy(p => p.Insurer).ToList();
                ////        break;
                ////    case "6":
                ////        // Setting.
                ////        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AccidentDate).ToList()
                ////                                                                                 : data.OrderBy(p => p.AccidentDate).ToList();
                ////        break;
                ////    case "7":
                ////        // Setting.
                ////        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList()
                ////                                                                                 : data.OrderBy(p => p.Status).ToList();
                ////        break;


                ////}
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return data;
        }

        public void ListaCasosParaBusquedaAvanzada_tabla(String paciente="")
        {
            try
            {
                CasosGlobalEstatica = model.Reporte_Data_Entry_Search(paciente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
