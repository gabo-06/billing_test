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
    public class ReportReferralSupervisorController : Controller
    {
        public CaseModel modelCase = new CaseModel();
        //public ReportsModel modelDataEntry = new ReportsModel();


        public ReportModel model = new ReportModel();


        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;

        public ReportReferralSupervisorController()
        { 
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }

        public PartialViewResult listaReferralBySupervisor(Int32 codigoSupervisor = 0, Int32 dias = 0)
        {
            List<PA_NEW_REFERRAL_BYSUPERVISOR_Result> Listado;
            Listado = model.ReporteReferralBySupervisor(codigoSupervisor,dias);

            return PartialView("lista_reporte",Listado);
        }


        [HttpPost]
        public ActionResult ExportToWordReferralbySupervisor(Int32 codigoSupervisor = 0, String supervisor = "", Int32 diasReporte = 0)
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
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 22px;font-family: sans-serif;'>NEW REFERRAL BY SUPERVISOR</td></tr>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 18px;font-family: sans-serif;'>" + supervisor + "</td></tr>";
            strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 16px;font-family: sans-serif;'>The last " + diasReporte + " days </td></tr></table>";

            ////////Original
            // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.
            //CargaDetalleFactura

            List<PA_NEW_REFERRAL_BYSUPERVISOR_Result> Listado;
            Listado = model.ReporteReferralBySupervisor(codigoSupervisor, diasReporte);
            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Referral_Supervisor_report-" + fechaActual; 


            string htmlString = this.RenderRazorViewToString(@"lista_reporte", Listado);
            htmlString = strBody + htmlString;
            Response.Clear();
            Response.Charset = "";
            //Response.ContentType = "application/msword";
            Response.ContentType = "application/vnd.ms-word";
            Response.AddHeader("Content-Disposition", "inline;filename=" + nombre_archivo + ".doc");
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
