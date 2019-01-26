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
    public class ReportBillingInsurerController : Controller
    {
        public CaseModel modelCase = new CaseModel();
        //public ReportsModel modelDataEntry = new ReportsModel();


        public ReportModel model = new ReportModel();


        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;

        public ReportBillingInsurerController()
        { 
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }

        public ActionResult listaResultado(Int32 codigoInsurer,String aseguradora, String fechaInicial, String fechaFin)
        {
            //List<DATA_ENTRY_PER_USER_Result> Listado;
            //Listado = model.ReporteDataEntryPerUser(fechaInicial, fechaFin);
            //return PartialView("lista_reporte", Listado);

            //codigoInsurer =42;
            //fechaInicial = "02/10/2016";
            //fechaFin = "11/11/2016";


            String strBody = "";
            strBody = creaContenido(codigoInsurer,aseguradora, fechaInicial, fechaFin);
            return Content(strBody);
        }


         



        public ActionResult ExportToExcel(String codigoAseguradora, String Aseguradora, String fechaInicio, String fechaFin)
        {
            //codigoInsurer = "42";
            //fechaInicial = "02/10/2016";
            //fechaFin = "11/11/2016";

            String strBody = "";
            strBody = creaContenido(Convert.ToInt32(codigoAseguradora), Aseguradora, fechaInicio, fechaFin);
            var fechaActual = DateTime.Now.ToShortDateString();
            string nombre_archivo = "Billing_Insurer_report-" + fechaActual; 


            //htmlString = strBody + htmlString;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + nombre_archivo + ".xls");
            //Response.ContentType = "application/"+nombre_archivo+".xls";
            Response.ContentType = "application/vnd.ms-excel";
            



            Response.Cache.SetCacheability(HttpCacheability.NoCache); // not necessarily required
            Response.Charset = "";
            Response.Output.Write(strBody);
            Response.End();
            return null;
        }

        //[HttpPost]
        //public ActionResult ExportToWordDataEntryPerUser(DateTime fechaInicial, DateTime fechaFin)
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

        //    String supervisor =  "test";

        //    strBody = strBody + "<table style='width:100%;margin-bottom:20px;'>";
        //    strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 30px;font-family: sans-serif;'>OPEN CASES BY SUPERVISOR</td></tr>";
        //    strBody = strBody + "<tr><td colspan=4 style='text-align:center; font-weight:bold;width:100px;font-size: 30px;font-family: sans-serif;'>" + supervisor + "</td></tr></table>";

        //    ////////Original
        //    // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.
        //    //CargaDetalleFactura

        //    List<DATA_ENTRY_PER_USER_Result> Listado;
        //    Listado = model.ReporteDataEntryPerUser(fechaInicial, fechaFin);
            
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
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }



        public String creaContenido(Int32 codigoInsurer,String aseguradora, String fechaInicial, String fechaFin)
        {


            List<PA_MONTHLY_BILLING_BY_INSURER_Result> Listado;
            List<PA_MONTHLY_BILLING_BY_INSURER_Result> Listado2;
            List<String> ListadoFecha = new List<String>();


            var fila = new List<String>(ListadoFecha.Count + 3);
            var filaDesc = new List<String>(ListadoFecha.Count + 3);
            var filaSubTotal = new List<Decimal>(ListadoFecha.Count + 3);
            //String columnas ="";
            var filaFinal = new List<Decimal>(ListadoFecha.Count + 3);

            Decimal filaMontoSubTotal = 0;
            Decimal TotalfilaMontoSubTotal = 0;
            var ColumnaMontoSubTotal = new List<String>(ListadoFecha.Count + 3);



            Listado = model.ReporteMonthlyBillingByInsurer(codigoInsurer, fechaInicial, fechaFin);
            Listado2 = Listado;

            Listado2 = Listado2.OrderBy(x => x.cierre).ToList();
//            Listado2 = Listado2.OrderByDescending(x => x.cierre).ToList();


            for (int x = 0; x < Listado2.Count; x++)
            {
                if (Listado2[x].cierre != ".")
                {
                 if (x == 0 )
                 {
                    ListadoFecha.Add(Listado2[x].cierre);
                 }
                 else
                 {
                        if (Listado2[x].cierre != Listado2[x - 1].cierre)
                            ListadoFecha.Add(Listado2[x].cierre);
                 }
                }
            }

            for (int w = 0; w < ListadoFecha.Count; w++)// 
            {
                filaSubTotal.Add(0);
            }

            

            ListadoFecha = ListadoFecha.OrderBy(q => DateTime.Parse(q)).ToList();//ordena fecha para que envista salga por ejemplo del 2016 al 2017 en orden que manda desde base

            
            String strBody = "<html><body><div style='width325px;overflow:auto;'><table style='text-align: center;width: 100%;overflow-y:scroll' >";
            strBody = strBody + "<tr><td style='text-align: right;width: 350px;'><b>Insurer :</b>&nbsp;&nbsp;&nbsp;</td><td style='text-align: left;'>" + aseguradora + "</td></tr>";
            strBody = strBody + "<tr><td style='text-align: right; width: 350px;'><b>Start Date :</b>&nbsp;&nbsp;&nbsp;</td><td style='text-align: left;'>" + fechaInicial + "</td></tr>";
            strBody = strBody + "<tr><td style='text-align: right; width: 350px;'><b>End Date :</b>&nbsp;&nbsp;&nbsp;</td><td style='text-align: left;'>" + fechaFin + "</td></tr></table>";

            strBody = strBody + "<table class='table table-striped table-bordered table-hover dataTables-example' id='tblListaReporte'>";
            strBody = strBody + "<thead class='th_tabla' style='background-color:#b42734;color:white;'>"; 
            strBody = strBody + "<tr style='background-color:#b42734;color:white;'><th>Patient Name</th><th>Claim Number</th>";
            for (int f = 1; f <= ListadoFecha.Count; f++)
            {

                  strBody = strBody + "<th>Month/Amount" + f + "</th>";
            }

            strBody = strBody + "<th>Total</th></tr></thead><tbody>";

            Int32 nC = 0;
            Int32 nFF = 0; // nro fila FInal
            Int32 test = 0;

            

            for (int j = 0; j < Listado.Count; j++)
            {

                nC = 0;
                nFF = 0;
                test = filaFinal.Count();
                if (filaFinal.Count() == 0)
                    filaFinal.Add(0);

                if (j == (Listado.Count - 1)) 
                {


                    List<String> filaTemp = new List<String>();
                    Int32 sw = 0;
                    strBody = strBody + "<tr>";


                    filaTemp.Add(filaDesc[0]);
                    filaTemp.Add(filaDesc[1]);




                    //strBody = strBody + "<td>" + fila[x] + "</td>";
                    for (int y = 0; y < ListadoFecha.Count; y++)// 
                    {
                        sw = 0;
                        for (int x = 0; x < fila.Count; x++)
                        {
                            //var dato = null;
                            String dato = "";
                            if (filaDesc[x] != null && filaDesc[x].Contains('$'))
                                dato = filaDesc[x].Split('$')[1];


                            Decimal dato_ant = 0;
                            if (ListadoFecha[y] == fila[x])
                            {


                                if (sw == 0)
                                {
                                    filaTemp.Add(filaDesc[x]);
                                    dato = filaDesc[x].Split('$')[1];
                                }
                                else// para las fechas repetidas,tomare la ultima
                                {
                                    filaTemp[filaTemp.Count - 1] = filaDesc[x];
                                    dato = filaDesc[x].Split('$')[1];
                                    dato_ant = Convert.ToDecimal(filaDesc[x - 1].Split('$')[1]);
                                }

                                sw = 1;
                                filaSubTotal[y] = filaSubTotal[y] + Convert.ToDecimal(dato);
                                if (dato_ant > 0)
                                    filaSubTotal[y] = filaSubTotal[y] - Convert.ToDecimal(dato_ant);
                            }

                        }
                        if (sw == 0)
                        {
                            filaTemp.Add("-");
                        }
                    }



                    for (int z = 0; z < filaTemp.Count; z++)//recorre filasTemp
                    {
                        strBody = strBody + "<td>" + filaTemp[z] + "</td>";
                        //filaMontoSubTotal
                    }
                    strBody = strBody + "<td>" + filaMontoSubTotal + "</td>";
                    TotalfilaMontoSubTotal = TotalfilaMontoSubTotal + filaMontoSubTotal;
                    filaMontoSubTotal = 0;




                    fila = new List<String>(ListadoFecha.Count + 3);
                    filaDesc = new List<String>(ListadoFecha.Count + 3);
                    strBody = strBody + "</tr>";

                }

                //filaMontoSubTotal = 0;
                for (int i = 0; i < ListadoFecha.Count; i++)
                {
                  if(Listado[j].Paciente !=".")
                   {
                    if (i == 0 && fila.Count == 0 )// && j < (Listado.Count - 1))
                    {

                            fila.Add(Listado[j].Paciente);
                            fila.Add(Listado[j].Cis_caseCode);

                            filaDesc.Add(Listado[j].Paciente);
                            filaDesc.Add(Listado[j].Cis_caseCode);

                            //////if (filaSubTotal.Count < nC + 1)
                            //////{
                            //////    filaSubTotal.Add(0);
                            //////    filaSubTotal.Add(0);
                            //////}

                            filaMontoSubTotal = filaMontoSubTotal + Convert.ToDecimal(Listado[j].Bih_billTotal);
                            nC = 2;

                            filaFinal[nFF] = filaFinal[nFF] + filaMontoSubTotal;
                                                
                    }
                    if (Listado[j].Paciente == fila[0])
                    {
                        if (Listado[j].cierre == ListadoFecha[i])
                        {
                            fila.Add(ListadoFecha[i]);
                            filaDesc.Add(Listado[j].Description);


                            if (i == 25)
                            {
                                var a = 0;
                            }


                            if (nC <= 1)
                            {
                                filaMontoSubTotal = filaMontoSubTotal + Convert.ToDecimal(Listado[j].Bih_billTotal);
                                ////filaSubTotal[nC] = filaSubTotal[nC] + Convert.ToDecimal(Listado[j].Bih_billTotal);
                            }
                            else
                            {
                                if (fila[nC] != fila[nC - 1] && j > 0 && nC > 2)
                                {
                                    filaMontoSubTotal = filaMontoSubTotal + Convert.ToDecimal(Listado[j].Bih_billTotal);
                                    ////filaSubTotal[nC] = filaSubTotal[nC] + Convert.ToDecimal(Listado[j].Bih_billTotal);
                                }
                                else
                                {
                                    filaMontoSubTotal = filaMontoSubTotal;
                                }
                            }

                            nC = nC + 1;
                        }
                    }
                    if ((Listado[j].Paciente != fila[0])) //|| j == (Listado.Count - 1) )//else
                    {
                        List<String> filaTemp = new List<String>();
                        Int32 sw = 0;
                        strBody = strBody + "<tr>";


                        filaTemp.Add(filaDesc[0]);
                        filaTemp.Add(filaDesc[1]);



                        ///VALIDA SUBTOTAL POR FECHAS-->filaDesc
                        //strBody = strBody + "<td>" + fila[x] + "</td>";
                        for (int y = 0; y < ListadoFecha.Count; y++)// 
                        {
                            sw = 0;
                            for (int x = 0; x < fila.Count; x++)
                            {
                                //var dato = null;
                                String dato="";
                                if (filaDesc[x] != null && filaDesc[x].Contains('$') )                                
                                        dato = filaDesc[x].Split('$')[1];                                 


                                Decimal dato_ant = 0;
                                if (ListadoFecha[y] == fila[x])  ///AKI SUMA FECHAS PARA EL SUBTOTAL 
                                {
                                    

                                    if (sw == 0)
                                    {
                                        filaTemp.Add(filaDesc[x]);
                                        dato = filaDesc[x].Split('$')[1];
                                    }
                                    else// para las fechas repetidas,tomare la ultima
                                    {
                                        String datoNuevo = filaDesc[x];
                                        String datocortado1 = filaDesc[x].Split('$')[0];
                                        String datocortado2 = filaDesc[x].Split('$')[1];//valor a sumar Actual
                                        String datocortado3 = filaDesc[x - 1].Split('$')[1];//valor a sumar Anterior

                                        decimal suma = decimal.Parse(datocortado2) + decimal.Parse(datocortado3);

                                        // + Convert.ToDecimal(datocortado3);
                                        datoNuevo = filaDesc[x].Split('$')[0] + "$" + suma; 

                                        //filaTemp[filaTemp.Count - 1] = filaDesc[x];
                                        filaTemp[filaTemp.Count - 1] = datoNuevo;
                                        dato = filaDesc[x].Split('$')[1];
                                        dato_ant = Convert.ToDecimal(filaDesc[x-1].Split('$')[1]);
                                    }

                                    sw = 1;
                                    filaSubTotal[y] = filaSubTotal[y] + Convert.ToDecimal(dato);
                                    //if(dato_ant >0)
                                    //    filaSubTotal[y] = filaSubTotal[y] - Convert.ToDecimal(dato_ant);
                                }
                                
                            }
                            if (sw == 0)
                            {
                                filaTemp.Add("-");
                            }
                        }



                        for (int z = 0; z < filaTemp.Count; z++)//recorre filasTemp
                        {
                            strBody = strBody + "<td>" + filaTemp[z] + "</td>";
                            //filaMontoSubTotal
                        }
                        strBody = strBody + "<td>" + filaMontoSubTotal + "</td>";                        
                        TotalfilaMontoSubTotal = TotalfilaMontoSubTotal + filaMontoSubTotal;
                        filaMontoSubTotal = 0;


                        fila = new List<String>(ListadoFecha.Count + 3);
                        filaDesc = new List<String>(ListadoFecha.Count + 3);
                        //if (j < (Listado.Count - 1))
                         i = -1;

                        strBody = strBody + "</tr>";
                    }


                }

                }

                // if ( j == Listado.Count-1)
                //{
                //    strBody = strBody + "<td>" + filaTemp[z] + "</td>";
                //}

                

            }

            String filaTotales = "";
            for (int d = 0; d < filaSubTotal.Count; d++)//recorre filasTemp
            {
                filaTotales = filaTotales + "<td>" + filaSubTotal[d] + "</td>";
                //filaMontoSubTotal
            }

            if(filaTotales=="")
                strBody = strBody + "<tr style='background-color: #e2e2e2;'><td colspan = 3 style='text-align:center;'>No data available in table</td></tr>";

            strBody = strBody + "<tr><td></td><td><b>Total</b></td>" + filaTotales + "<td>"+ TotalfilaMontoSubTotal +"</td></tr>";
            strBody = strBody + "</tbody></table></div></body></html>";
            return strBody;
        }

       
    }
}
