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


using System.Globalization;

namespace Billing.Web.Controllers
{
    public class ReportDataEntryPerUserController : Controller
    {
        public CaseModel modelCase = new CaseModel();
        //public ReportsModel modelDataEntry = new ReportsModel();


        public ReportModel model = new ReportModel();


        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;

        public ReportDataEntryPerUserController()
        { 
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }

        public PartialViewResult listaDataEntryPerUser(String fechaInicial, String fechaFin, Int32 filtro, Int32 codigoUser = 0, String permiso = "")
        {
            List<DATA_ENTRY_PER_USER_Result> Listado =new List<DATA_ENTRY_PER_USER_Result>();
            List<DATA_ENTRY_PER_USER_Result> ListadoResultado = new List<DATA_ENTRY_PER_USER_Result>();
            List<SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result> ListadoUsuarios;
    

            //DateTime fechaInicial_date = DateTime.ParseExact(fechaInicial,"MM/dd/yyyy", new CultureInfo("en-US"),DateTimeStyles.None );
            DateTime fechaInicial_date =    DateTime.ParseExact(fechaInicial, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None);
            DateTime fechaFin_date = Convert.ToDateTime(fechaFin);
            //fechaFin_date.Day.ToString("00");

            //

            if (permiso == "Administrator")
            {
                ListadoUsuarios = model.ReporteDataEntryPerUserCount(fechaInicial, fechaFin);
                for (int i = 0; i < ListadoUsuarios.Count(); i++)
                {
                    Listado = model.ReporteDataEntryPerUser(fechaInicial_date, fechaFin_date, ListadoUsuarios[i].Use_code, filtro);
                    ListadoResultado.AddRange(Listado);
                }
            }
            else
            {
                Listado = model.ReporteDataEntryPerUser(fechaInicial_date, fechaFin_date, codigoUser, filtro);
                ListadoResultado.AddRange(Listado);
            }

            return PartialView("lista_reporte", ListadoResultado);
        }


        [HttpPost]
        public ActionResult ExportToWordDataEntryPerUser(String fechaInicial = "", String fechaFin = "", String supervisor = "", Int32 codigoUser = 0, Int32 filtro = 0, String permisoLocal = "")
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

            if (fechaInicial != "" && fechaFin != "")
            {
                //String supervisor =  "test";

                strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
                strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 19px;font-family: sans-serif;'>REPORT DATA ENTRY PER USER </td></tr></table>";
                //strBody = strBody + "<tr><td colspan=2 style='text-align:center; font-weight:bold;width:100px;font-size: 15px;font-family: sans-serif;'>User: </td><td colspan=2>" + supervisor + "</td></tr>";
                //strBody = strBody + "<tr><td colspan=2 style='text-align:center; font-weight:bold;width:100px;font-size: 15px;font-family: sans-serif;'>Date Input: </td><td colspan=2>" + fechaInicial + " to " + fechaFin + "</td></tr></table>";

                ////////Original
                // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.
                //CargaDetalleFactura

                //////////List<DATA_ENTRY_PER_USER_Result> ListadoPorUsuario;
                //////////ListadoPorUsuario = model.ReporteDataEntryPerUser(fechaInicial, fechaFin);



                //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                List<DATA_ENTRY_PER_USER_Result> Listado = new List<DATA_ENTRY_PER_USER_Result>();
                List<DATA_ENTRY_PER_USER_Result> ListadoResultado = new List<DATA_ENTRY_PER_USER_Result>();
                List<SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result> ListadoUsuarios;

                /*  ListadoUsuarios = model.ReporteDataEntryPerUserCount(fechaInicial, fechaFin);

                  DateTime fechaInicial_date = DateTime.ParseExact(fechaInicial, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None);
                  DateTime fechaFin_date = DateTime.Parse(fechaFin);
                  for (int i = 0; i < ListadoUsuarios.Count(); i++)
                  {
                      Listado = model.ReporteDataEntryPerUser(fechaInicial_date, fechaFin_date, ListadoUsuarios[i].Use_code, filtro);
                      ///Listado = model.ReporteDataEntryPerUser(fechaInicial, fechaFin);

                      ListadoResultado.AddRange(Listado);
                  }*/

                DateTime fechaInicial_date = DateTime.ParseExact(fechaInicial, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None);
                DateTime fechaFin_date = DateTime.Parse(fechaFin);

                if (permisoLocal == "Administrator")
                {
                    ListadoUsuarios = model.ReporteDataEntryPerUserCount(fechaInicial, fechaFin);
                    for (int i = 0; i < ListadoUsuarios.Count(); i++)
                    {
                        Listado = model.ReporteDataEntryPerUser(fechaInicial_date, fechaFin_date, ListadoUsuarios[i].Use_code, filtro);
                        ListadoResultado.AddRange(Listado);
                    }
                }
                else
                {
                    Listado = model.ReporteDataEntryPerUser(fechaInicial_date, fechaFin_date, codigoUser, filtro);
                    ListadoResultado.AddRange(Listado);
                }



                //////////////////////////////////////////////////////////////////////////////////////////////////////////////


                ///////////////////List<DATA_ENTRY_PER_USER_Result> Listado;
                ///////////////////Listado = model.ReporteDataEntryPerUser(fechaInicial, fechaFin);



                var fechaActual = DateTime.Now.ToShortDateString();
                string nombre_archivo = "Data_Entry_Per_User_Report-" + fechaActual;


                string htmlString = this.RenderRazorViewToString(@"lista_reporte_word", ListadoResultado);
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
            else
            {
                return null;
            }
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
