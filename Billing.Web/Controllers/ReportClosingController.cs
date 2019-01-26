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
    public class ReportClosingController : Controller
    {
        public CaseModel modelCase = new CaseModel();
        //public ReportsModel modelDataEntry = new ReportsModel();


        public ReportModel model = new ReportModel();


        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;

        public ReportClosingController()
        { 
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }

        public PartialViewResult listaClosing(String fechaInicial, String fechaFin, Int32 tipo = 0)
        {
            
            if (tipo == 1)
            {
                List<SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result> Listado;
                Listado = model.ReporteClosing1(fechaInicial);
                ViewBag.Tipo = "1";
                return PartialView("lista_reporte", Listado);
            }
            if (tipo == 2)
            {
                List<SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result> Listado;
                Listado = model.ReporteClosing2(fechaInicial, fechaFin);
                ViewBag.Tipo = 2;
                return PartialView("lista_reporte2", Listado);
            }
            return null;
        }


        [HttpPost]
        public ActionResult ExportToWordClosing(String fechaInicial, String fechaFin,Int32 tipo =0, String supervisor="")
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

            supervisor =  "test";

            strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 14px;font-family: sans-serif;text-decoration: underline;'>CLOSING REPORT</td></tr>";
            strBody = strBody + "<tr><td colspan=2 style='text-align:right;font-weight:bold;width:100px;font-size: 12px;font-family: sans-serif;'>Closing Date :</td><td colspan=2 style='text-align:left; width:100px;font-size: 12px;font-family: sans-serif;'>" + fechaInicial + "</td></tr>";
            if(tipo==2)
                strBody = strBody + "<tr><td colspan=2 style='text-align:right;font-weight:bold;width:100px;font-size: 12px;font-family: sans-serif;margin-top:2px;'>Closing Date 2:</td><td colspan=2 style='text-align:left; width:100px;font-size: 12px;font-family: sans-serif;'>" + fechaFin + "</td></tr>";
            strBody = strBody + "</table>";


            string htmlString = "";

            if (tipo == 1)
            {
                List<SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result> Listado;
                Listado = model.ReporteClosing1(fechaInicial);
                htmlString = this.RenderRazorViewToString(@"lista_reporte", Listado);
            }
            if (tipo == 2)
            {
                List<SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result> Listado;
                Listado = model.ReporteClosing2(fechaInicial, fechaFin);
                htmlString = this.RenderRazorViewToString(@"lista_reporte2", Listado);
            }

            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Report_closing-" + fechaActual; 
            
            
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
