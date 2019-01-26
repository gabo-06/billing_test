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

//using iTextSharp.text;
//using iTextSharp.text.html;
//using iTextSharp.text.pdf;
//using iTextSharp.text.xml.simpleparser;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.tool.xml; 
using System.IO;

using System.Text;

//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.xml;



namespace Billing.Web.Controllers
{
    public class CaseController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public CaseModel model = new CaseModel();          
        // ******************************************************************************************

        public PartialViewResult Index()
        {
            return PartialView();    
        }

        // Método que lista los casos para una búsqueda avanzada.
        public PartialViewResult ListaCasosParaBusquedaAvanzada()
        {
            List<CasoBusquedaAvanzada> Casos;
            
            try
            {
                Casos = model.ListaCasosParaBusquedaAvanzada(); 
                return PartialView("CasosParaBusquedaAvanzada", Casos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* (1) */
        // Este procedimiento verifica la disponibilidad de un caso para poder ser leído, 
        // es decir verifica si el caso está siendo leído por otro usuario.
        // En caso de estar ocupado, devuelve 1 y el nombre del usuario loeguado que tiene cargado el caso y 
        // en caso de estar desocupado devuelve 0 y una cadena vacía.
        /*
        public JsonResult VerificarDisponibilidadCaso(int CodigoCasoSeleccionado = 0, int CodigoUsuarioQueLee = 0)
        {
            SP_CHECK_AVAILABILITY_CASE_Result Resultado;

            try
            {
                Resultado = model.VerificarDisponibilidadCaso(CodigoCasoSeleccionado
                                                                , CodigoUsuarioQueLee);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        /* (2) */
        // Este procedimiento obtiene los datos del caso, en caso esté desocupado
        //public JsonResult ObtenerDatosCaso(int CodigoCasoALeer = 0)
        //{ 
        //    SP_GET_CASE_DATA_Result Resultado;

        //    try 
        //    {
        //        Resultado = model.ObtenerDatosCaso(CodigoCasoALeer);

        //        // ViewBag.AseguradoraDeCaso = Resultado.Insurer;
        //        Session["Aseguradora"] = "piramide";

        //        return Json(Resultado, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /* (3) */
        // Este procedimiento ocupa el caso leído y libera el caso cargado anteriormente.
        public void OcupaLiberaCasos(int CodigoCasoLeido = 0, int CodigoUsuarioQueLee = 0, int CodigoCasoALiberar = 0)
        {
            try
            {
                model.OcupaLiberaCasos(CodigoCasoLeido
                                      ,CodigoUsuarioQueLee
                                      ,CodigoCasoALiberar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Este procedimiento se usa cuando hay un caso cargado y se carga un mantenedor u otro proceso.
        public void LiberaCasoDeUsuarioActual(int CodigoUsuarioActual = 0)
        {
            try
            {
                model.LiberaCasoDeUsuarioActual(CodigoUsuarioActual);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult BuscarHomonimos(CaseInformationSheetHead Caso)
        {
            List<SP_FIND_HOMONYM_CASES_Result> Datos;

            try
            {
                Datos = model.BuscarHomonimos(Caso);

                if (Datos == null)
                    return Json(new { Resultado = false }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Resultado = Datos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Insertar(CaseInformationSheetHead Caso)
        {
            List<SP_SAVE_CASE_Result> Resultado;

            try
            {
                Resultado = model.Insertar(Caso);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ///////////////////////////////PDF/////////////////////////////////////////////
        /*
        public void DownloadPDF(Int32 codigoCase)
        {

            String newFile = "", nuevaruta = "";
            String serverPath = System.Web.HttpContext.Current.Server.MapPath("~");
            String ruta = "";

            ruta = "\\Archivos\\Formato\\";
            nuevaruta = "TemplateTemporal.pdf";


            String archivoBase = "";
            archivoBase = "PDFCase.pdf";
            String pdfTemplate = "";


            String rutaV = serverPath + "\\Archivos\\TemplateTemporal\\";
            // String pdfTemplate = "";
            pdfTemplate = serverPath + ruta + archivoBase;
            newFile = rutaV + nuevaruta;

            leeRutaTemporal(pdfTemplate, newFile, codigoCase);//Aqui se llena la plantilla con una hoja, se guarda en esta direccion y en base a esta direccion en el sgt metoto(GeneraPdf)agrego las demas hojas

            Response.Clear();
            Response.ContentType = "application/pdf";

            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.WriteFile(newFile);
            //Response.WriteFile(GeneraPdf(pdfTemplate));
            Response.WriteFile(GeneraPdf(newFile, codigoCase));
            Response.End();

        }
        */
       /*
        
        private string GeneraPdf(String archivo_plantilla, Int32 codigoCase)
        {
            string outputFileName = Path.GetTempFileName() + ".pdf";
            //string outputFileName = newFile;
            //Step 1: Create a Docuement-Object
            Document document = new Document();
            FileStream Temporal = new FileStream(outputFileName, FileMode.Create);
            try
            {
                //////////////////////////////////////////////////


                //Step 2: we create a writer that listens to the document
                //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputFileName, FileMode.Create));
                PdfWriter writer = PdfWriter.GetInstance(document, Temporal);

                //Step 3: Open the document
                document.Open();

                PdfContentByte cb = writer.DirectContent;

                //The current file path
                string filename = archivo_plantilla;

                // we create a reader for the document
                PdfReader reader = new PdfReader(System.IO.File.ReadAllBytes(filename));

                for (int pageNumber = 1; pageNumber < reader.NumberOfPages + 1; pageNumber++)
                {
                    document.SetPageSize(reader.GetPageSizeWithRotation(1));
                    document.NewPage();

                    //Insert to Destination on the first page
                    if (pageNumber == 1)
                    {
                        Chunk fileRef = new Chunk(" ");
                        fileRef.SetLocalDestination(filename);
                        document.Add(fileRef);
                    }

                    PdfImportedPage page = writer.GetImportedPage(reader, pageNumber);
                    int rotation = reader.GetPageRotation(pageNumber);
                    if (rotation == 90 || rotation == 270)
                    {
                        cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(pageNumber).Height);
                    }
                    else
                    {
                        cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                    }
                }


                ///////////////////////////////////////////////////////LLENA TABLA MEDICOS///////////////////////////////////////
                // Add a new page to the pdf file
                document.NewPage();//NUEVO PDF 
                PdfPTable table = new PdfPTable(12);

                //////////////////////////////////////////////////////// CABECERA //////////////////////////////////////////////
                var FontColour = new BaseColor(255, 255, 255);
                var FontColour_H = new BaseColor(125, 0, 11);//color del texto del head
                var FontColour_F = new BaseColor(0, 0, 0);//color del texto de filas

                var colorHead = new BaseColor(125, 0, 11);

                //var colorTexto = FontFactory.GetFont("Arial", 10, FontColour);
                Font tipoTexto = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, FontColour);
                Font tipoTexto_H = new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD, FontColour_H); //tipo de texto del head
                Font tipoTexto_F = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, FontColour_F); //tipo de texto de filas



                PdfPCell cell = new PdfPCell(new Phrase("DOCTOR AND FACILITIES", tipoTexto_H));
                cell.Colspan = 12;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.WHITE;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("NAME", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("STREET", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("CITY", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ST", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ZIP", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("EXT", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("PHONE", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("EXT", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ALT PH", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("FAX", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("OFFICE", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("SPECIALITY", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                //////////////////////////////////////////////////////// FIN CABECERA //////////////////////////////////////////////



                //////////////////////////////////////////////////////// CONTENIDO TABLA MEDICOS//////////////////////////////////////////////


                //// llama metodo 
                CaseMedController caseMed = new CaseMedController();
                var data = caseMed.ObtenerDoctoresCaso(codigoCase);


                List<SP_LIST_CASE_MEDICAL_Result> listaMed = (List<SP_LIST_CASE_MEDICAL_Result>)data.Data;


                foreach (SP_LIST_CASE_MEDICAL_Result d in listaMed)
                {

                    ////NOMBRE
                    cell = new PdfPCell(new Phrase(d.NombreCompleto, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);



                    /////DIRECCION
                    cell = new PdfPCell(new Phrase(d.Direccion, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ////CIUDAD
                    cell = new PdfPCell(new Phrase(d.Ciudad, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///Estado                                        
                    cell = new PdfPCell(new Phrase(d.Estado, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///ExtensionCodigoPostal                    
                    cell = new PdfPCell(new Phrase(d.ExtensionCodigoPostal, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///                    
                    cell = new PdfPCell(new Phrase(d.ExtensionCodigoPostal, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);


                    //TELEFONO                    
                    cell = new PdfPCell(new Phrase(d.Telefono, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    //ExtensionTelefono
                    cell = new PdfPCell(new Phrase(d.ExtensionTelefono, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);


                    //TelefonoAlternativo
                    cell = new PdfPCell(new Phrase(d.TelefonoAlternativo, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ////FAX
                    cell = new PdfPCell(new Phrase(d.Fax, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///OFICINA
                    cell = new PdfPCell(new Phrase(d.Oficina, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///ESPECIALIDAD
                    cell = new PdfPCell(new Phrase(d.Especialidad, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);
                }


                //////////////////////////////////////////////////////// FIN TABLA MEDICOS//////////////////////////////////////////////
                float[] widths = new float[] { 30f, 30f, 15f, 5f, 8f, 8f, 20f, 10f, 12f, 17f, 32f, 32f };
                table.SetWidths(widths);
                table.WidthPercentage = 100;

                document.Add(table);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                ///////////////////////////////////////////////////////LLENA TABLA ATTORNEYS///////////////////////////////////////
                // Add a new page to the pdf file
                document.NewPage();//NUEVO PDF 
                table = new PdfPTable(10);

                //////////////////////////////////////////////////////// CABECERA ATTORNEYS //////////////////////////////////////////////


                cell = new PdfPCell(new Phrase("ATTORNEYS", tipoTexto_H));
                cell.Colspan = 12;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.WHITE;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("NAME", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("STREET", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("CITY", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ST", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ZIP", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("EXT", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("PHONE", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);



                cell = new PdfPCell(new Phrase("FAX", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ASSISTANT", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("SPECIALITY", tipoTexto));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = colorHead;
                cell.BorderWidth = 0f;
                table.AddCell(cell);

                //////////////////////////////////////////////////////// FIN CABECERA ATTORNEYS//////////////////////////////////////////////
                //////////////////////////////////////////////////////// CUERPO ATTORNEYS//////////////////////////////////////////////

                //// llama metodo 
                CaseAttController caseAtt = new CaseAttController();
                data = caseAtt.ObtenerAbogadosCaso(codigoCase);


                List<SP_LIST_CASE_ATTORNEY_Result> listaAtt = (List<SP_LIST_CASE_ATTORNEY_Result>)data.Data;


                foreach (SP_LIST_CASE_ATTORNEY_Result d in listaAtt)
                {

                    ////NOMBRE
                    cell = new PdfPCell(new Phrase(d.NombreCompleto, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);


                    /////DIRECCION
                    cell = new PdfPCell(new Phrase(d.Direccion, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ////CIUDAD
                    cell = new PdfPCell(new Phrase(d.Ciudad, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///Estado                                        
                    cell = new PdfPCell(new Phrase(d.Estado, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///Zip              
                    cell = new PdfPCell(new Phrase(d.ExtensionCodigoPostal, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///Ext              
                    cell = new PdfPCell(new Phrase(d.ExtensionCodigoPostal, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);


                    //Phone                    
                    cell = new PdfPCell(new Phrase(d.Telefono, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);


                    ////FAX
                    cell = new PdfPCell(new Phrase(d.Fax, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///ASISTENTE
                    cell = new PdfPCell(new Phrase(d.Asistente, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///ESPECIALIDAD
                    cell = new PdfPCell(new Phrase(d.Especialidad, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);
                }

                //////////////////////////////////////////////////////// FIN CUERPO ATTORNEYS//////////////////////////////////////////////

                widths = new float[] { 32f, 32f, 14f, 5f, 8f, 8f, 10f, 10f, 30f, 40f };
                table.SetWidths(widths);
                table.WidthPercentage = 100;
                document.Add(table);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                document.Close();


            }
            return outputFileName;
        }

        */

        /*
        private void leeRutaTemporal(string template, string newFile, Int32 codigoCase)
        {

            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;

            pdfReader = new PdfReader(template);
            pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;


            PdfContentByte pdfContentByte = pdfStamper.GetOverContent(1);
            //    //// llama metodo 
            var data = ObtenerDatosCaso(codigoCase);
            var Objeto = (SP_GET_CASE_DATA_Result)data.Data; // parseo al tipo de bjeto que deseo, en este caso el tipo que tengo en el modelo


            // Asigna los campos
            pdfFormFields.SetField("patient", Objeto.Patient);
            pdfFormFields.SetField("ssnumber", "xx");
            pdfFormFields.SetField("dob", "xx");
            pdfFormFields.SetField("streetpat", Objeto.InsurerAddress);
            pdfFormFields.SetField("citypat", Objeto.InsurerCity);
            pdfFormFields.SetField("statepat", Objeto.InsurerState);
            pdfFormFields.SetField("zippat", Objeto.InsurerZipCode);
            pdfFormFields.SetField("zipextpat", Objeto.InsurerZipCodeExt);
            pdfFormFields.SetField("phonepat", Objeto.InsurerPhone);
            pdfFormFields.SetField("alternativephonepat", Objeto.InsurerPhoneExt);
            pdfFormFields.SetField("doi", "xx");
            pdfFormFields.SetField("injury", Objeto.Insurer);
            pdfFormFields.SetField("price", Convert.ToString(Objeto.CasePrice));
            pdfFormFields.SetField("referal", Convert.ToString(Objeto.CaseReferralDate));
            pdfFormFields.SetField("provider", Objeto.Provider);

            pdfFormFields.SetField("longyes", Objeto.CaseLongshore.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("statecompyes", Objeto.CaseCompanyStatus.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("otheryes", Objeto.CaseOther.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("other_text", Objeto.CaseOtherText);

            pdfFormFields.SetField("claim", "xx");
            pdfFormFields.SetField("NameAdj", Objeto.Adjuster);
            pdfFormFields.SetField("phoneadj", Objeto.AdjusterPhone);
            pdfFormFields.SetField("phoneextadj", Objeto.AdjusterPhoneExt);
            pdfFormFields.SetField("insurer", Objeto.Insurer);
            pdfFormFields.SetField("streetins", Objeto.InsurerAddress);
            pdfFormFields.SetField("cityins", Objeto.InsurerCity);
            pdfFormFields.SetField("stateins", Objeto.InsurerState);
            pdfFormFields.SetField("zipins", Objeto.InsurerZipCode);
            pdfFormFields.SetField("zipextins", Objeto.InsurerZipCodeExt);
            pdfFormFields.SetField("phoneins", Objeto.InsurerPhone);
            pdfFormFields.SetField("phoneextins", Objeto.InsurerPhoneExt);
            pdfFormFields.SetField("faxins", Objeto.InsurerFax);
            pdfFormFields.SetField("cc1", Objeto.CaseContact1);
            pdfFormFields.SetField("cc2", Objeto.CaseContact2);

            pdfFormFields.SetField("trasyes", Objeto.CaseTranslation.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("trasno", Objeto.CaseTranslation.Equals(false) ? "0" : "1");
            pdfFormFields.SetField("traslation_comp", Objeto.CaseTranslationCompany);
            pdfFormFields.SetField("tranyes", Objeto.CaseTransportation.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("tranno", Objeto.CaseTransportation.Equals(false) ? "0" : "1");
            pdfFormFields.SetField("transportation_comp", Objeto.CaseTransportationCompany);
            pdfFormFields.SetField("phyyes", Objeto.CasePhysicalTherapy.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("phyno", Objeto.CasePhysicalTherapy.Equals(false) ? "0" : "1");
            pdfFormFields.SetField("physical_comp", Objeto.CasePhysicalTherapyCompany);
            pdfFormFields.SetField("permyes", Objeto.CasePermisionContact.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("permno", Objeto.CasePermisionContact.Equals(false) ? "0" : "1");
            pdfFormFields.SetField("perm_comp", Objeto.CasePermisionContactCompany);
            pdfFormFields.SetField("notes", Objeto.CaseComment);



            pdfStamper.FormFlattening = true;

            if (pdfStamper != null) pdfStamper.Close();
            if (pdfReader != null) pdfReader.Close();
        }
        */

        [HttpPost]
        public JsonResult Actualizar(CaseInformationSheetHead Caso)
        {
            List<SP_UPDATE_CASE_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Caso);
                return Json(Resultado, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
