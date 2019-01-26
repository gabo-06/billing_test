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
    public class ReportAdjusterController : Controller
    {
        public CaseModel modelCase = new CaseModel();
        public ReportModel modelAdjuster = new ReportModel();


        public ReportModel model = new ReportModel();

        static List<SP_FIND_ADJ_REPORTS_Result> CasosGlobalEstatica = new List<SP_FIND_ADJ_REPORTS_Result>();
        static List<SP_FIND_ADJ_REPORTS_Result> CasosTemporal = new List<SP_FIND_ADJ_REPORTS_Result>();



        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;

        public ReportAdjusterController()
        { 
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }

        public PartialViewResult listaAdjuster(Int32 codigoAdjuster = 0)//codigo adjuster=35
        {
            List<SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result> Listado;
            Listado = model.ReporteAdjuster(codigoAdjuster);
            return PartialView("listaAdjuster_reporte", Listado);
        }


     
        /////////////////////////////////////////////////////////////////////ADJUSTER////////////////////////////////////////////////////////////////
 
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

                    this.ListaAdjusterReporte_Tabla_Inicial();

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
                    CasosTemporal = CasosGlobalEstatica.Where(p => p.Adj_lastName.ToString().ToLower().Contains(search.ToLower()) ||
                                                                   (p.Adj_firstName.ToString() + p.Adj_lastName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                   (p.Adj_lastName.ToString() + p.Adj_firstName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower())
                                                              ).ToList();



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


        private List<SP_FIND_ADJ_REPORTS_Result> SortByColumnWithOrder(string order, string orderDir, List<SP_FIND_ADJ_REPORTS_Result> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "1":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adjuster).ToList()
                                                                                                 : data.OrderBy(p => p.Adjuster).ToList();
                        break;
                    case "3":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adj_phone).ToList()
                                                                                                 : data.OrderBy(p => p.Adj_phone).ToList();
                        break;
                    case "5":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adj_phoneExt).ToList()
                                                                                                 : data.OrderBy(p => p.Adj_phoneExt).ToList();
                        break;


                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return data;
        }

        public void ListaAdjusterReporte_Tabla_Inicial(String name ="")
        {
            try
            {
                CasosGlobalEstatica = modelAdjuster.ListaFormularioAdjuster(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        public ActionResult ExportToWordAdjuster(Int32 CodigoCasoSeleccionado = 0, String nombreAdjuster ="")
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
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;font-size:16;font-family: sans-serif;'>REPORT BY ADJUSTER</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:60;text-align:right;'>Compan :y</td><td style='width:250;text-align:left;'> Omnimed Inc.</td><td style='font-weight:bold;width:60;text-align:right;'>Adjuster :</td><td>" + nombreAdjuster + "</td></tr></table>";

            ////////Original
            // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.
            //CargaDetalleFactura

            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Adjuster_report-" + fechaActual; 

            List<SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result> Listado;
            Listado = model.ReporteAdjuster(CodigoCasoSeleccionado);


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




        public ActionResult ExportToPdfAdjuster(Int32 CodigoCasoSeleccionado = 0, String nombreAdjuster ="")
        {


            string strBody = string.Empty;
            strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;font-size:18;font-family: sans-serif;'>REPORT BY ADJUSTER</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:60;text-align:right;'>Company</td><td style='width:250;text-align:left;'> Omnimed Inc.</td><td style='font-weight:bold;width:60;text-align:right;'>Adjuster</td><td>" + nombreAdjuster + "</td></tr></table>";


            List<SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result> Listado;
            Listado = model.ReporteAdjuster(CodigoCasoSeleccionado);

            var htmlString = this._HtmlViewRenderer.RenderViewToString(this, "lista_reporte_pdf", Listado);
            htmlString = strBody + htmlString;
            byte[] buffer = StandardPdfRenderer.Render(htmlString, "");

            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Adjuster_report-" + fechaActual; 

            return File(buffer, "application/pdf", nombre_archivo +".pdf");



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



       
    }
}
