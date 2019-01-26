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
    public class ReportCaseByInsurerController : Controller
    {
        public CaseModel modelCase = new CaseModel();
        //public ReportsModel modelDataEntry = new ReportsModel();


        public ReportModel model = new ReportModel();


        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;

        public ReportCaseByInsurerController()
        { 
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }

        public PartialViewResult listaCaseByInsurer(Int32 codigoInsurer = 0)
        {
            List<SP_CASE_BY_INSURER_Result> Listado;
            Listado = model.ReporteCaseByInsurer(codigoInsurer);

            return PartialView("lista_reporte",Listado);
        }


        //[HttpPost]
        //public ActionResult ExportToWordCaseByInsurer(Int32 codigoAseguradora = 0, String Aseguradora = "")
        //{


        //    string strBody = string.Empty;
        //    strBody = @"<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
        //    "xmlns:w='urn:schemas-microsoft-com:office:word'" +
        //    "xmlns='http://www.w3.org/TR/REC-html40'>";

        //    strBody = strBody + "<!--[if gte mso 9]>" +
        //    "<xml>" +
        //    "<w:WordDocument>" +
        //    "<w:View>Print</w:View>" +
        //    "<w:Zoom>100</w:Zoom>" +
        //    "</w:WordDocument>" +
        //    "</xml>" +
        //    "<![endif]-->";



        //    strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
        //    strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 30px;font-family: sans-serif;'>OPEN CASES BY ASEGURADORA</td></tr>";
        //    strBody = strBody + "<tr><td colspan=2 style='text-align:center; font-weight:bold;width:100px;font-size: 30px;font-family: sans-serif;'>Insurer</td><td colspan=2>" + Aseguradora + "</td></tr></table>";

        //    ////////Original
        //    // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.
        //    //CargaDetalleFactura

        //    List<SP_CASE_BY_INSURER_Result>Listado;
        //    Listado = model.ReporteCaseByInsurer(codigoAseguradora);
            
        //    string htmlString = this.RenderRazorViewToString(@"lista_reporte", Listado);
        //    htmlString = strBody + htmlString;
        //    Response.Clear();
        //    Response.Charset = "";
        //    //Response.ContentType = "application/msword";
        //    Response.ContentType = "application/vnd.ms-word";
        //    Response.AddHeader("Content-Disposition", "inline;filename=EFFReport.doc");
        //    Response.Write(htmlString);
        //    return null;
        //    ////////FIN Original

        //}

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

        public ActionResult ExportToPdfCaseByInsurer(Int32 codigoAseguradora = 0, String Aseguradora = "")
        {


            string strBody = string.Empty;
            strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;font-size:16;font-family: sans-serif;'>REPORT CASES BY INSURER</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:60;'>Insurer</td><td style='width:250;text-align:center;'>" + Aseguradora + "</td></tr></table>";


            List<SP_CASE_BY_INSURER_Result> Listado;
            Listado = model.ReporteCaseByInsurer(codigoAseguradora);

            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Case_by_insurer_report-" + fechaActual;


            var htmlString = this._HtmlViewRenderer.RenderViewToString(this, "lista_reporte_pdf", Listado);
            htmlString = strBody + htmlString;
            byte[] buffer = StandardPdfRenderer.Render(htmlString, "");

            return File(buffer, "application/pdf", nombre_archivo + ".pdf");

        }

        public ActionResult ExportToExcel(Int32 codigoAseguradora = 0, String Aseguradora = "")
        {

            List<SP_CASE_BY_INSURER_Result> Listado;
            Listado = model.ReporteCaseByInsurer(codigoAseguradora);
            String strBody = "<html><body><table>";
            strBody = strBody + "<tr><td colspan=9 style='font-weight:bold;text-align:center;'>REPORT CASES BY INSURER</td></tr>";            
            strBody = strBody + "<tr><td colspan=4 style='font-weight:bold;text-align:right;'>Insurer</td><td colspan=5>" + Aseguradora + "</td></tr>";
            strBody = strBody + "</table><table border = 1>";
            strBody = strBody + "<tr style='background-color:#b42734;color:white;'><td>Date OF Referral</td><td>Patient Name</td><td>Insurer</td><td>Adjuster</td><td>Date of Injury</td><td>Close Date</td><td>Supervisor</td><td>Nurse</td><td>Claim Number</td></tr>";
            for (int i = 0; i < Listado.Count(); i ++  )
            {
                strBody = strBody + "<tr><td>"+Listado[i].Cis_referralDate+"</td><td>"+Listado[i].patient+"</td><td>"+Listado[i].Ins_name+"</td><td>"+Listado[i].Adjuster+"</td><td>"+Listado[i].Cis_accidentDate+"</td><td>"+Listado[i].Cis_closedDate+"</td><td>"+Listado[i].Supervisor+"</td><td>"+Listado[i].Supervisor+"</td><td>---</td></tr>";
            }

            strBody = strBody + "</table></body></html>";


            //////List<SP_PHYSICAL_THERAPY_REPORTS_Result> Listado;
            //////Listado = model.ReportePhysicalTherapy();

            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Case_by_insurer_report-" + fechaActual; 

            //string htmlString = this.RenderRazorViewToString(@"lista_excel", Listado);
            //htmlString = strBody + htmlString;
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename="+nombre_archivo+".xls");
            Response.ContentType = "application/vnd.xls";
            Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
            Response.Charset = "";
            Response.Output.Write(strBody);
            Response.End();
            return null;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////


       
    }
}
