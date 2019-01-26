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
using System.IO;
using System.Web.UI;
 


using System.Text;
using System.Text.RegularExpressions;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;

using System.Diagnostics;
using System.Xml.Serialization;

namespace Billing.Web.Controllers
{
    public class BillingController : Controller
    {
        public BillingModel model = new BillingModel();
        
        public readonly HtmlViewRenderer _HtmlViewRenderer; 
        private readonly StandardPdfRenderer StandardPdfRenderer;


        private  List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result> CasosGlobalEstatica = new List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result>();
        private List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result> CasosTemporal = new List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result>();
        //
        // GET: /Billing/

        public BillingController()
        {             
            this._HtmlViewRenderer = new HtmlViewRenderer();
            this.StandardPdfRenderer = new StandardPdfRenderer();
        }
        
        public PartialViewResult Index()
        {
            return PartialView();
        }





       
 ///////////////////////////////////////////////////PDF SI//////////////////////////////////////////////////////////////////////    

        public void DownloadPDF(String invoice = "", String patient = "", String codigoCaso = "", String codigoPaciente = "", String codigoFactura = "", String cantidadFilas = "", String closing = "", String montoTotal = "")//agrega las hojas restantes y genera el pdf
        {
            String serverPath = System.Web.HttpContext.Current.Server.MapPath("~");            
            String newFile = "", nuevaruta = "", ruta_temporal = "";

            ///ruta base
            String ruta = "";
            ruta = "\\Archivos\\Formato\\";
            String archivoBase = "";
            archivoBase = "PDFBilling.pdf";
            String pdfTemplate_base = serverPath + ruta + archivoBase;///template base a copiar 


            ///ruta temporal
            //String ruta_temporal = "";
            ruta_temporal = "\\Archivos\\TemplateTemporal\\";
            String archivoBase_temp = "";
            archivoBase_temp = "TemplateTemporal_billing.pdf";
            String pdfTemplate_temporal = serverPath + ruta_temporal + archivoBase_temp;


            ///ruta template d
            String archivoBase_duplicado = "TemplateTemporal_billing_duplicado.pdf";
            String pdfTemplate_base_duplicada = serverPath + ruta_temporal + archivoBase_duplicado;


            ///ruta resultado FInal
            String ruta_resultado = "";
            ruta_resultado = "\\Archivos\\Resultado\\";

            String archivoBase_resul_parcial = "PDFBilling_Resultado_parcial.pdf";
            String pdfTemplate_resul_parcial = serverPath + ruta_resultado + archivoBase_resul_parcial;

            String archivoBase_resul = "PDFBilling_Resultado.pdf";
            String pdfTemplate_resul = serverPath + ruta_resultado + archivoBase_resul;
            newFile = pdfTemplate_resul;

            /// filas dataEntry
            //var filas_dataEntry = "";//falo lista de dataEntries del proc
           // var cantidadFilas = "";//falo lista de dataEntries del proc


            Int32 cantidadTotalFac = Convert.ToInt32(cantidadFilas);
            Int32 residuo = cantidadTotalFac % 17;
            Int32 totalPaginas = cantidadTotalFac / 17;
            if (residuo > 0)
                totalPaginas = totalPaginas + 1;


            //////////////////////////////////LLena Formulario/////////////////////////////////////////
            llenaFormularioPdf(pdfTemplate_base, pdfTemplate_temporal, Convert.ToInt32(codigoCaso), Convert.ToInt32(codigoPaciente), Convert.ToInt32(codigoFactura), invoice,closing);//pdfTemplate_temporal sera el arcihvo temporal con el formulario lleno, este se utilizara para copiar
            //////////////////////////////////duplica pdf/////////////////////////////////////////
            duplicaPDf_page(pdfTemplate_temporal, pdfTemplate_base_duplicada, totalPaginas);
            //////////////////////////////////LLena Tabla/////////////////////////////////////////
            llenaTabla(pdfTemplate_base_duplicada, invoice, pdfTemplate_resul_parcial);
            /////////////////////////////////////////////////////////////////////////////////////
            posicionaTotal(pdfTemplate_resul_parcial, pdfTemplate_resul,montoTotal);
            ////////////////////////////////////////////////////////////////////////////////////
            Response.Clear();
            Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=" + "Billing_" + invoice + ".pdf");
            //Response.AddHeader("content-disposition", "inline;filename=" + "Billing_" + invoice + ".pdf");

            Response.AppendHeader("Content-Disposition", "attachment; filename="+ "Billing_" + invoice + ".pdf");


            Response.Cache.SetCacheability(HttpCacheability.NoCache);            
            //Response.WriteFile(GeneraPdf(newFile, codigoCase));
            Response.WriteFile(newFile);
            Response.Flush();

         
        }
    


////////////////////////////Llena formulario del pdf///////////////////////////////

        private void llenaFormularioPdf(string template, string newFile, Int32 codigoCase = 0, Int32 codigoPaciente = 0, Int32 codigoFactura = 0, String invoice = "", String closing = "")
        {

      

            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;

            pdfReader = new PdfReader(template);
            pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;


            PdfContentByte pdfContentByte = pdfStamper.GetOverContent(1);
            //  llama metodo 

            ///////////////////////////////////LLAMO A METODO MODELO BILLING GABo////////////////////////////////////////////////


            BillingController BillingPdf = new BillingController();
            List<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result> datoPdf = BillingPdf.Obtener_Info_Pdf_Billing(codigoCase, codigoFactura);



            PatientController Paciente = new PatientController();
            var dataPaciente = Paciente.Buscar(codigoPaciente);
            ////////var Objeto = (Pcase)data.Data; // parseo al tipo de bjeto que deseo, en este caso el tipo que tengo en el modelo
            var ObjetoPaciente = (PPatient)dataPaciente.Data; // parseo al tipo de bjeto que deseo, en este caso el tipo que tengo en el modelo


            DateTime dbo = Convert.ToDateTime(ObjetoPaciente.Pat_birthday);
            String cadenaDbo = dbo.Month + "/" + dbo.Day + "/" + dbo.Year;

            DateTime doi = Convert.ToDateTime(datoPdf[0].CaseAccidentDate );
            String cadenaDoi = doi.Month + "/" + doi.Day + "/" + doi.Year;

            DateTime CaseReferralDate = Convert.ToDateTime(datoPdf[0].CaseReferralDate);
            String cadenaCaseReferralDate = CaseReferralDate.Month + "/" + CaseReferralDate.Day + "/" + CaseReferralDate.Year;


            //DateTime dt  = DateTime.ParseExact(ObjetoPaciente.Pat_birthday,"mm/dd/yyyy",cultureInfo.)


            // Asigna los campos
            //----------------------------------------
            pdfFormFields.SetField("num_invoice", "Inv #: " + invoice);
            pdfFormFields.SetField("billing_date", closing);
            //----------------------------------------(1)
            pdfFormFields.SetField("Name", ObjetoPaciente.Pat_firstName + " " + ObjetoPaciente.Pat_lastName);
            //----------------------------------------(2)
            pdfFormFields.SetField("ssnumber", ObjetoPaciente.Pat_socialSecurityNumberD);
            //----------------------------------------(3)
            pdfFormFields.SetField("date_accidental", cadenaDoi);
            //----------------------------------------(4)
            pdfFormFields.SetField("date_referral", cadenaCaseReferralDate);
            //----------------------------------------(5)
            pdfFormFields.SetField("phone_pat", ObjetoPaciente.Pat_phone);

            //pdfFormFields.SetField("claim", objetoCaso.CaseCaseCode);

            pdfFormFields.SetField("claim", datoPdf[0].CaseCaseCode );

            //----------------------------------------(6)
            pdfFormFields.SetField("name_ins", datoPdf[0].Insurer );
            pdfFormFields.SetField("name_Adj", datoPdf[0].Adj_fullName);
            pdfFormFields.SetField("street_ins", datoPdf[0].InsurerAddress);
            pdfFormFields.SetField("city_st_ins", datoPdf[0].InsurerCity);
            pdfFormFields.SetField("zipcode_ins", datoPdf[0].InsurerZipCode);
            //////////pdfFormFields.SetField("sc_tpa_code", datoPdf.Data );
            pdfFormFields.SetField("fein_sc", datoPdf[0].Ins_feinSc);
            pdfFormFields.SetField("carrier", datoPdf[0].Ins_carrierCode);
            pdfFormFields.SetField("fein_cc", datoPdf[0].Ins_feinCc);
            //pdfFormFields.SetField("contact_name", datoPdf[0].Par_name  );
            pdfFormFields.SetField("contact_name", "");
            pdfFormFields.SetField("t_number", "");
            //pdfFormFields.SetField("t_number", datoPdf[0].Ins_phone );
            pdfFormFields.SetField("contact_email", "");

            //----------------------------------------(7)
            pdfFormFields.SetField("remito", datoPdf[0].Par_name);
            pdfFormFields.SetField("parm_address",  datoPdf[0].Par_address );
            pdfFormFields.SetField("parm_city", datoPdf[0].Par_city);
            pdfFormFields.SetField("parm_state", datoPdf[0].Par_state);
            pdfFormFields.SetField("parm_zipcode", datoPdf[0].Par_zipCode );
            //pdfFormFields.SetField("pro_fein", datoPdf[0].Pro_fein );
            pdfFormFields.SetField("pro_fein", "20-4761586");
            pdfFormFields.SetField("parm_phone", datoPdf[0].Par_phone );

            //----------------------------------------(8)
            pdfFormFields.SetField("citypat", ObjetoPaciente.Pat_city);

            //----------------------------------------(9)
            //pdfFormFields.SetField("CasillaVerificación1", Objeto.CaseCompanyStatus.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("CasillaVerificación1", "");
            pdfFormFields.SetField("CasillaVerificación2", "1");

            //----------------------------------------(10a)
            pdfFormFields.SetField("name_pro", datoPdf[0].Pro_fullName );//nombre proveedor lo lee de xtrae_datos_caseingormation --colum12

            //----------------------------------------(10b)
            pdfFormFields.SetField("number_pro", datoPdf[0].Pro_number );

            //----------------------------------------(11a)
            pdfFormFields.SetField("company", datoPdf[0].Par_name);

            //----------------------------------------(11b)
            pdfFormFields.SetField("cod_parm", datoPdf[0].Par_number);

            //----------------------------------------(12)
            pdfFormFields.SetField("date_return", "");

            //----------------------------------------(13)
            pdfFormFields.SetField("date_weekly", "");



            pdfStamper.FormFlattening = true;

            if (pdfStamper != null) pdfStamper.Close();
            if (pdfReader != null) pdfReader.Close();
        }
        
////////////////////////////Duplica las paginas pdf///////////////////////////////
       
        public void duplicaPDf_page(String pdfTemplate_copiar = "", String pdfTemplate_resul = "", Int32 cantidadPaginas = 0)
        {
            string[] lstFiles = new string[3];
            //Int32 cantidadPaginas = cantidadPaginas;

            //String serverPath = System.Web.HttpContext.Current.Server.MapPath("~");
            
            //String ruta_resultado = "";
            //ruta_resultado = "\\Archivos\\Formato\\PDFBilling_Modificado.pdf";           
            //String pdfTemplate_R = serverPath + ruta_resultado;

            //String ruta = "";
            //ruta = "\\Archivos\\Formato\\TemplateTemporal_billing.pdf";
            //String pdfTemplate_A = serverPath + ruta;

            //String ruta_salida = "";     
            //ruta_salida = "\\Archivos\\TemplateTemporal\\PDFBilling_temporal.pdf";
            //String pdfTemplate_S = serverPath + ruta_salida;
            String pdfTemplate_S = pdfTemplate_resul;

            //lstFiles[0] = pdfTemplate_A;//pagina a duplicar
            lstFiles[0] = pdfTemplate_copiar;//pagina a duplicar            
            //lstFiles[1] = pdfTemplate_R;
            //lstFiles[2] = pdfTemplate_S;


            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            //string outputPdfPath = @"C:/pdf/new.pdf";
            string outputPdfPath = pdfTemplate_S;


            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            //Open the output file
            sourceDocument.Open();

            try
            {
                ////Loop through the files list
                //for (int f = 0; f < lstFiles.Length - 1; f++)
                //{
                //    int pages = get_pageCcount(lstFiles[0]);

                //    reader = new PdfReader(lstFiles[0]);
                //    //Add pages of current file
                //    for (int i = 1; i <= pages; i++)
                //    {
                //        importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                //        pdfCopyProvider.AddPage(importedPage);
                //    }

                //    reader.Close();
                //}

                //Loop through the files list
                for (int f = 0; f <= cantidadPaginas - 1; f++)
                {
                    int pages = get_pageCcount(lstFiles[0]);

                    reader = new PdfReader(lstFiles[0]);
                    //Add pages of current file
                    for (int i = 1; i <= pages; i++)
                    {
                        importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                        pdfCopyProvider.AddPage(importedPage);
                    }

                    reader.Close();
                }

                //At the end save the output file
                sourceDocument.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        private int get_pageCcount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }

///////////////////////////LLEna tabla en pdf/////////////////////////////////////////////////////


        private void llenaTabla(String pathin, String factura, String pdfTemplate_resul, object filas_dataEntry = null)//si
         {
            String pathout = "";
            //pathin = pdfTemplate_temporal;
            pathout = pdfTemplate_resul;

            //pathin => ruta que tomara como modelo (template)
            //pathout => sera el resultado con la tabla la estampada

            PdfReader reader = new PdfReader(pathin);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(pathout, FileMode.Create));
            ////////
            //AcroFields pdfFormFields = stamper.AcroFields;
            ////////
            PdfPCell cell = null;
            PdfPTable table = null;
            DataTable dt = null;

            
            // obtiene la cantidad total de filas, las guardo en lista y luego las divido
            //totalb ==> numero 22 
            //totalm ==> numero 23 
            dt = GetDataTable(factura);//Aqui obtiene la data y llena tabla
            Int32 cantidadTotalFac = dt.Rows.Count;
            Int32 residuo  = dt.Rows.Count % 17 ;
            Int32 totalPaginas = dt.Rows.Count / 17;
            if (residuo > 0)
                totalPaginas = totalPaginas + 1;

            Int32 filaActual = 0;
            Int32 filaTemporal = 0;
            Int32 filaTemporal_final = 0;


            for (int pagina = 1; pagina <= totalPaginas; pagina++)
            {

                
                if (dt != null)
                {
                    Font font8 = FontFactory.GetFont("ARIAL", 7);
                    table = new PdfPTable(dt.Columns.Count);
                    //cell = new PdfPCell(new Phrase(new Chunk("Date of service", font8)));
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(new Chunk("Code", font8)));
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(new Chunk("Description", font8)));
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(new Chunk("Units of service", font8)));
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(new Chunk("Charge per unit", font8)));
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(new Chunk("Total billed", font8)));
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(new Chunk("Amount Reimbursed", font8)));
                    //table.AddCell(cell);

                    filaTemporal = filaActual;
                    filaTemporal_final = filaTemporal + 17;
                    for (int rows = filaTemporal; rows <= filaTemporal_final -1; rows++)
                    {
                        //testc = dt.Rows[rows][2].ToString();

                        if (filaActual < cantidadTotalFac)
                        {
                            for (int column = 0; column < dt.Columns.Count; column++)
                            {
                                cell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
                                cell.Border = 0;
                                cell.PaddingBottom = 5f;
                                cell.PaddingTop = 5f;
                                table.AddCell(cell);
                            }
                        }
                        if (filaActual < filaTemporal_final)
                           filaActual = filaActual + 1;
                    }

                }


                float[] widths = new float[] { 15f, 9.5f, 28f, 16f, 15f, 16f, 13f };
                table.SetWidths(widths);
                table.WidthPercentage = 100;
                table.DefaultCell.Border = 0;


                ColumnText ct = new ColumnText(stamper.GetOverContent(pagina));
                ct.AddElement(table);
                ct.SetSimpleColumn(36, 36, PageSize.A4.Width, PageSize.A4.Height - 500);

                if (pagina == totalPaginas)
                {

                    //ct.AddElement("sssss");
                    //ct.SetSimpleColumn(36, 36, PageSize.A4.Width, PageSize.A4.Height - 500);
                }
                ct.Go();
                
            }

            stamper.Close();
            reader.Close();
        }
 
        
        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            //cell.BorderColor = "#fff";
            //cell.VerticalAlignment = PdfCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            cell.Border = 0;
            //cell.PaddingLeft = 10f;            
            return cell;
        }


        private DataTable GetDataTable(String NumeroFactura = "")
        {

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Date of service", typeof(string)),
                        new DataColumn("Code", typeof(string)),
                        new DataColumn("Description",typeof(string)),
                        new DataColumn("Units of service",typeof(string)),
                        new DataColumn("Charge per unit",typeof(string)),
                        new DataColumn("Total billed",typeof(string)),
                        new DataColumn("Amount Reimbursed",typeof(string))
            });

            /*
            dt.Rows.Add("xxxxxxxxxx", "MCC", "APPOINTMENT LETTER", 50, "United States", "United States", "United States");
            dt.Rows.Add("09/08/2016", "MCC", "LETTER TO PATIENT", 60, "United States", "United States", "United States");
            dt.Rows.Add("09/08/2016", "MCC", "EMAIL", 35, "United States", "United States", "United States");
            dt.Rows.Add("09/08/2016", "MCC", "MONTHLY REPORT", 8, "United States", "United States", "United States");*/

            List<DetalleFactura> DetalleDeFactura;
            DetalleDeFactura = model.CargaDetalleFactura(NumeroFactura);

            foreach (var LectorDetalle in DetalleDeFactura)
            {

                //for (int i = 0; i <= 16; i++)
                //{
                dt.Rows.Add(LectorDetalle.Bib_servDate, "MMC", LectorDetalle.Act_description, LectorDetalle.Bib_hourMile, LectorDetalle.Bib_priceAct, LectorDetalle.Bib_amoReim.ToString("#0.00"));
                //}
            }
            return dt;

        }



        private void posicionaTotal(String pdfTemplate_base_duplicada = "", String pdfTemplate_resul = "",String total="")
        {


            //using (var reader = new PdfReader(@"C:\Input.pdf"))
            using (var reader = new PdfReader(pdfTemplate_base_duplicada))
            {
                //using (var fileStream = new FileStream(@"C:\Output.pdf", FileMode.Create, FileAccess.Write))
                using (var fileStream = new FileStream(pdfTemplate_resul, FileMode.Create, FileAccess.Write))
                {
                    var document = new Document(reader.GetPageSizeWithRotation(1));
                    var writer = PdfWriter.GetInstance(document, fileStream);

                    document.Open();

                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {

                            document.NewPage();

                            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            var importedPage = writer.GetImportedPage(reader, i);
                            var contentByte = writer.DirectContent;
                            contentByte.BeginText();
                            contentByte.SetFontAndSize(baseFont, 12);
                            //total = "1548";
                            if (i == reader.NumberOfPages)
                            {

                                var multiLineString = "$ " +" " + total;

                                //foreach (var line in multiLineString)
                                //{
                                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, multiLineString, 452, 25, 0);
                                //}
                                contentByte.EndText();
                                contentByte.AddTemplate(importedPage, 0, 0);
                            }
                            else
                            {
                                contentByte.EndText();
                                contentByte.AddTemplate(importedPage, 0, 0);
                            }

                        
                    }

                    document.Close();
                    writer.Close();
                }
            }
        }


///////////////////////////////////////FIN PDF///////////////////////////////////////////////////////////////////////////////////





//////////////////////////////////////DOC////////////////////////////////////////////////////////////////////////
        //////public void testWord()
        //////{
        //////    Response.ClearContent();
        //////    Response.Buffer = true;
        //////    Response.AddHeader("content-disposition", "attachment;filename=marklist.doc");
        //////    Response.ContentType = "application/vnd.ms-word";
        //////    Response.Charset = string.Empty;

        //////    StringWriter sw = new StringWriter();
        //////    HtmlTextWriter htw = new HtmlTextWriter(sw);
        //////}




        public ActionResult ExportToWord(String invoice = "", String DOI = "", String patient = "")
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
            strBody = strBody + "<tr><td colspan=6 style='text-align:center; font-weight:bold;width:100px;font-size: 30px;font-family: sans-serif;'>BILLING REPORT</td></tr>";
            strBody = strBody + "<tr><td style='font-weight:bold;width:100px;'>Invoice #</td><td style='width:310px;'>" + invoice+ "</td>";
            strBody = strBody + "<td style='font-weight:bold;width:100px;'>Patient</td><td style='width:325px;'>" + patient + "</td>";
            strBody = strBody + "<td style='font-weight:bold;width:100px;'>DOI</td><td >" + DOI + "</td></tr></table>";

            ////////Original
            // List<CasoCierreMes> Casos = (List<CasoCierreMes>)null; // Inicializa la variable casos.
            //CargaDetalleFactura

            List<DetalleFactura> DetalleDeFactura;
            DetalleDeFactura = model.CargaDetalleFactura(invoice);            


            string htmlString = this.RenderRazorViewToString(@"Billing_List_word", DetalleDeFactura);
            htmlString = strBody + htmlString; 
            Response.Clear();
            Response.Charset ="";            
            //Response.ContentType = "application/msword";
            Response.ContentType = "application/vnd.ms-word";
            Response.AddHeader("Content-Disposition", "inline;filename=Billing_Report_" + invoice + ".doc");
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

        //////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public JsonResult ListaDetalleFactura(string NumeroFactura = "")
        {
            List<DetalleFactura> DetalleDeFactura;

            try
            {
                DetalleDeFactura = model.CargaDetalleFactura(NumeroFactura);
                return Json(DetalleDeFactura, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpPost]
        //public PartialViewResult CargaDetalleFactura(string NumeroFactura = "") // VERSION ANTERIOR
        //{
        //    List<DetalleFactura> DetalleDeFactura;

        //    try
        //    {
        //        DetalleDeFactura = model.CargaDetalleFactura(NumeroFactura);
        //        return PartialView("Billing_List", DetalleDeFactura);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        ////    // return PartialView();
        //}
        [HttpPost]
        public PartialViewResult CargaDetalleFactura_Nuevo(string numeroFactura = "", int totalDeDetalles = 0, int paginaDestino = 1) // VERSION NUEVA
        {
            List<DetalleFactura> DetalleDeFactura;
            try
            {
                DetalleDeFactura = model.CargaDetalleFactura_Nuevo(numeroFactura, paginaDestino);
                //////////////////////////////////////////////////////PAGINADO
                
                int PageSize = 20;                
                int Total = totalDeDetalles;
                DetalleDeFactura = DetalleDeFactura.Take(PageSize).ToList();
                //Entradas = Entradas.OrderBy(p => p.Dae_date).ToList();

                Int64 NroPages = Convert.ToInt32(Math.Ceiling((double)Total / PageSize));
                ViewBag.NumberOfPages = NroPages + 1;
                Double residuo = Math.Ceiling((double)Total % PageSize);
                if (residuo == 0)
                    ViewBag.NumberOfPages = NroPages + 2;

                ViewBag.CurrentPage = paginaDestino;                
                /////////////////////////////////////////////////////////////
                return PartialView("Billing_List_Nuevo", DetalleDeFactura);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        //    // return PartialView();
        }
        
        /*
        [HttpPost]
        public PartialViewResult CargaDetalleFactura_2(string numeroFactura = "A0B0000485"
                                                    , int totalDeDetalles = 0
                                                    , int paginaDestino = 2)
        {
            List<DetalleFactura> detalleDeFactura;
            int tamañoDePagina;
            int total;
            int cantidadDePaginas;
            double residuo;

            try
            {
                detalleDeFactura = model.CargaDetalleFactura_2(numeroFactura
                                                            , paginaDestino);
                tamañoDePagina = 20;
                total = totalDeDetalles;
                cantidadDePaginas = Convert.ToInt32(Math.Ceiling((double)total / tamañoDePagina));
                ViewBag.cantidadDePaginas = cantidadDePaginas + 1;
                residuo = Math.Ceiling((double)total % tamañoDePagina);

                if (residuo == 0)
                    ViewBag.cantidadDePaginas = cantidadDePaginas + 1;

                ViewBag.paginaACtual = paginaDestino;

                return PartialView("", detalleDeFactura);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        */

        [HttpPost]
        public JsonResult BuscarFacturasDeCaso(string Claim = ""
                                             , string ApellidoPaciente = ""
                                             , int CodigoAseguradora = 0
                                             , string EstadoPago = "")
        {
            List<FacturaParaBilling> Facturas = (List<FacturaParaBilling>)null; // Inicializa la variable facturas.
                        
            try
            {
                Facturas = model.BuscarFacturasDeCaso(Claim, ApellidoPaciente, CodigoAseguradora, EstadoPago);
                return Json(Facturas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // [HttpPost]
        // public JsonResult DesocupaFacturacion(string NumeroFactura = "")
        // {
        //     List<FacturaParaBilling> Facturas = (List<FacturaParaBilling>)null; // Inicializa la variable facturas.
        // 
        //     try
        //     {
        //         Facturas = model.BuscarFacturasDeCaso(Claim, ApellidoPaciente, CodigoAseguradora, EstadoPago);
        //         return Json(Facturas, JsonRequestBehavior.AllowGet);
        //     }
        //     catch (Exception ex)
        //     {
        //         throw ex;
        //     }
        // }
         





/////////////////////////////////////////////////////////////////////////////////////////////////////////////

        

        /* (2) */
        // Este procedimiento obtiene los datos del caso, en caso esté desocupado
        public List<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result> Obtener_Info_Pdf_Billing(int CodigoCasoALeer = 0, Int32 CodigoFactura =0)
        {
            //SP_GET_CASE_DATA_Result Resultado;
            List<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result> Resultado;

            try
            {
                //CodigoCasoALeer = 914;
                //int CodigoFactura = 2832;
                 Resultado = model.ObtenerInformacionPdfBilling_header(CodigoCasoALeer,CodigoFactura);
                 return Resultado;
          
                // return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        [HttpPost]
        public JsonResult EliminaDetalle(int codigoDeDetalle = 0, int codigoDeUsuario = 0, string numeroDeFactura = "")
        {
            int Resultado;

            Resultado = 0;

            try
            {
                Resultado = model.EliminaDetalle(codigoDeDetalle, codigoDeUsuario, numeroDeFactura);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






        public ActionResult  GetData()
        {
            // Initialization. 
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                ////////////////////////////////////////////////////////////////////////////////////////////////




                // List<SalesOrderDetail> data;


                if (CasosGlobalEstatica.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();
                    // Carga la variable global estática con los pacientes de la base.
                    this.ListaBillingReporte_Tabla_Inicial();

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
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    //String dato = "";

                    String[] cadenas;
                    cadenas = search.Split('*');
                    //foreach (string cadena in cadenas)
                    //{
                    //    Console.WriteLine(cadenas);
                    //    dato = cadenas[0];
                    //}

                    String paid = cadenas[0];
                    //if (paid == "U") paid = "false";
                    String claim = cadenas[1];
                    String paciente = cadenas[2];
                    String insurer = cadenas[3];
                    String codigoPaciente = "vacio";
                    if (cadenas.Length > 4) // valido si es mayor que 4, por que hay otro metodo que trae menos datos
                        codigoPaciente = cadenas[4];
                    



                        CasosTemporal = CasosGlobalEstatica;  
                    // Apply search
                        CasosTemporal = CasosTemporal.Where(p => p.Bih_payStatus.ToString().ToLower().Contains(paid.ToLower())).ToList();
                        if (cadenas.Length> 1 )
                            CasosTemporal = CasosTemporal.Where(p => p.Cis_caseCode.ToString().ToLower().Contains(claim.ToLower())).ToList();
                        if (cadenas.Length > 2)
                        {
                            CasosTemporal = CasosTemporal.Where(p => p.Patient.ToString().ToLower().Contains(paciente.ToLower()) ||
                                                                   (p.Pat_firstName.ToString() + p.Pat_lastName.ToString()).ToLower().Contains(paciente.Replace(" ", "").ToLower()) ||
                                                                   (p.Pat_lastName.ToString() + p.Pat_firstName.ToString()).ToLower().Contains(paciente.Replace(" ", "").ToLower())).ToList();
                        }
                        if (cadenas.Length > 3 && insurer != "vacio")
                            CasosTemporal = CasosTemporal.Where(p => p.Insurer.ToString().ToLower().Contains(insurer.ToLower())).ToList();

                        if (cadenas.Length > 4 && codigoPaciente != "vacio")
                            CasosTemporal = CasosTemporal.Where(p => p.Cis_code.ToString().ToLower().Contains(codigoPaciente.ToLower())).ToList();


                                                                     // ||
                                                                   //(p.Adj_firstName.ToString() + p.Adj_lastName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                   //(p.Adj_lastName.ToString() + p.Adj_firstName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower())



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


        private List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result> SortByColumnWithOrder(string order, string orderDir, List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "5": // claim
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cis_caseCode).ToList()
                                                                                                 : data.OrderBy(p => p.Cis_caseCode).ToList();
                        break;
                    case "6":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Patient).ToList()
                                                                                                 : data.OrderBy(p => p.Patient).ToList();
                        break;
                    case "7"://insurer
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Insurer).ToList()          
                                                                                                     : data.OrderBy(p => p.Insurer).ToList();
                        break;
                        
                    case "8"://insurer
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Bih_code_old).ToList()
                                                                                                     : data.OrderBy(p => p.Bih_code_old).ToList();
                        break;
                    case "9":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AccidentDate).ToList()
                                                                                                     : data.OrderBy(p => p.AccidentDate).ToList();
                        break;
                        
                    case "10":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Bih_closingDate).ToList()
                                                                                                 : data.OrderBy(p => p.Bih_closingDate).ToList();
                        break;


                    //case "3":
                    //    // Setting.
                    //    data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adj_phone).ToList()
                    //                                                                             : data.OrderBy(p => p.Adj_phone).ToList();
                    //    break;
                    //case "5":
                    //    // Setting.
                    //    data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adj_phoneExt).ToList()
                    //     

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

        public void ListaBillingReporte_Tabla_Inicial()
        {
            try
            {
                CasosGlobalEstatica = model.Listar_Tabla_Billing_Formulario();
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public JsonResult obtenerCantidadDeDetallesDeFactura(string numeroDeFactura = "")
        {
            int cantidadDeDetallesDeFactura = 0;

            try
            {
                cantidadDeDetallesDeFactura = model.obtenerCantidadDeDetallesDeFactura(numeroDeFactura);
                return Json(cantidadDeDetallesDeFactura, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {                
                throw ex;
            }        
        }

        // Actualiza el detalle de una factura.
        [HttpPost]
        public JsonResult RegistraActualizaDetalleFactura(List<PDetalleFactura_RegistraActualiza> Detalles)
        {
            string DetallesXML;
            string NuevosDetallesXML;

            DataTable[] ArregloTablas;      // Arreglo donde se reciben las dos tablas (Tabla de montos y Tabla de detalles procesados).
            DataTable TMontos;              // Tabla de montos.
            DataTable TDetallesDevueltos;   // Tabla de detalles procesados devueltos.
            decimal[] Montos = new decimal[2];  // Arreglo de montos que se enviará a AJAX.
            List<PDetalleFactura_RegistraActualiza> DetallesDevueltos = new List<PDetalleFactura_RegistraActualiza>();  // Lista de detalles procesados que se enviarán a AJAX.

            DetallesXML = "";
            NuevosDetallesXML = "";

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PDetalleFactura_RegistraActualiza>));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, Detalles);
                    DetallesXML = Convert.ToString(textWriter);
                    NuevosDetallesXML = DetallesXML.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "")
                               .Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                               .Replace(@"xsi:nil=""true""", "")
                               .ToString().Trim();

                    ArregloTablas = model.RegistraActualizaDetalleFactura(NuevosDetallesXML);

                    // Obtiene las tablas por separado. 
                    TMontos = ArregloTablas[0]; //  Tabla de montos
                    TDetallesDevueltos = ArregloTablas[1]; // Tabla de detalles procesados

                    // Llena el arreglo de montos con los datos de la tabla de montos.
                    Montos[0] = (decimal)TMontos.Rows[0].ItemArray[0];
                    Montos[1] = (decimal)TMontos.Rows[0].ItemArray[1];

                    // Llena el arreglo de detalles procesados con los datos de la tabla de detalles procesados.
                    foreach (DataRow Detalle in TDetallesDevueltos.Rows)
                    {
                        PDetalleFactura_RegistraActualiza ElementoLista = new PDetalleFactura_RegistraActualiza();
                        DateTime FS_DT;         // Fecha de Servicio (DateTime).
                        string MesFS_STR = "";  // Mes de la fecha de servicio (string).
                        string DiaFS_STR = "";  // Dia de la fecha de servicio (string).
                        string AñoFS_STR = "";  // Año de la fecha de servicio (string).

                        FS_DT = Convert.ToDateTime(Detalle.ItemArray.GetValue(0).ToString());
                        MesFS_STR = FS_DT.Month.ToString().PadLeft(2, '0');
                        DiaFS_STR = FS_DT.Day.ToString().PadLeft(2, '0');
                        AñoFS_STR = FS_DT.Year.ToString();

                        ElementoLista.Bib_servDate = MesFS_STR + '/' + DiaFS_STR + '/' + AñoFS_STR;             // Bib_servDate
                        ElementoLista.Bib_code = int.Parse(Detalle.ItemArray.GetValue(1).ToString());           // Bib_code
                        ElementoLista.Act_code = int.Parse(Detalle.ItemArray.GetValue(2).ToString());           // Act_code
                        ElementoLista.Bib_hourMile = decimal.Parse(Detalle.ItemArray.GetValue(4).ToString());   // Bib_hourMile
                        ElementoLista.Bib_priceAct = decimal.Parse(Detalle.ItemArray.GetValue(5).ToString());   // Bib_priceAct
                        ElementoLista.Bib_amoReim = decimal.Parse(Detalle.ItemArray.GetValue(6).ToString());    // Bib_amoReim

                        DetallesDevueltos.Add(ElementoLista);
                    }

                    return Json(new { Montos = Montos, DetallesDevueltos = DetallesDevueltos });
                }
            }
            catch (Exception ex) 
            {

                throw ex;
            }
        }

        public JsonResult obtenerMontosDeFactura(string numeroDeFactura="")
        {
            DataTable[] arregloTablas;
            decimal montoTotal;
            decimal saldoAdeudado;

            try
            {
                arregloTablas = model.obtenerMontosDeFactura(numeroDeFactura);

                montoTotal = Decimal.Parse(arregloTablas[0].Rows[0].ItemArray.GetValue(0).ToString()); // Total Amount
                saldoAdeudado = Decimal.Parse(arregloTablas[1].Rows[0].ItemArray.GetValue(0).ToString()); // Balance Due

                return Json(new { TotalAmount = montoTotal, BalanceDue = saldoAdeudado });                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
