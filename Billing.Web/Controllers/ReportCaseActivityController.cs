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
    public class ReportCaseActivityController : Controller
    {
        public ReportModel modelReport = new ReportModel();
        //public ReportsModel modelDataEntry = new ReportsModel();


        public ReportModel model = new ReportModel();


        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;

        public ReportCaseActivityController()
        { 
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }

        public PartialViewResult listaCaseActivity(Int32 dias = 0)
        {
            List<SP_CASE_BY_INSURER_Result> Listado;
            Listado = model.ReporteCaseByInsurer(dias);
            return PartialView("lista_reporte",Listado);
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
        ///////////////////////////////////PDF///////////////////////////////////////////////////
        //public ActionResult ExportDataEntry(Int32 dias = 0)
        //{
        //    ExportToPdfCaseActivity(dias);
        //    return null;
        //}

        public ActionResult ExportToPdfCaseActivity(Int32 txtDays_pdf = 0)
        {

            string strBody = string.Empty;
            strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;font-size:16;font-family: sans-serif;'>CASES WITH NO ACTIVITY</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:300;'>Select days : </td><td style='width:250;text-align:left;'>" + txtDays_pdf + "</td><td></td><td></td></tr></table>";

            List<ReporteCasosSinActividad> Listado;
            Listado = model.reporteDeCasosSinActividad();

            Listado = Listado.Where(p => p.diasPasadosDesdeUltimaEntrada >= txtDays_pdf).ToList();
            //Listado = Listado.OrderBy(p => p.diasPasadosDesdeUltimaEntrada).ToList();
            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Case_with_no_activity_report-" + fechaActual;


            var htmlString = this._HtmlViewRenderer.RenderViewToString(this, "lista_reporte_pdf", Listado);
            htmlString = strBody + htmlString;
            byte[] buffer = StandardPdfRenderer.Render(htmlString, "");

            return File(buffer, "application/pdf", nombre_archivo + ".pdf");

            }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////


       
    }
}
