using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;
using System.Diagnostics;
using System.Data.Objects;
using System.Xml.Serialization;

namespace Billing.Web.Controllers
{
    public class DataEntryController : Controller
    {
        public DataEntryModel modelDataEntry = new DataEntryModel();

        //private OmnimedBDEntities db = new OmnimedBDEntities();
        //public CaseModel model = new CaseModel();
        //public StateModel model2 = new StateModel();
        //public SpecialtyModel model3 = new SpecialtyModel();

        private  List<CasoBusquedaAvanzada> CasosGlobalEstatica = new List<CasoBusquedaAvanzada>();
        private  List<CasoBusquedaAvanzada> CasosTemporal = new List<CasoBusquedaAvanzada>();

        // Método que lista la vista parcial de "DataEntry".
        public PartialViewResult Index()
        {
            return PartialView();
        } 
        
        // Método que lista las vista parcial de los casos.
        public PartialViewResult ListaCasosParaBusquedaAvanzada()
        {
            List<CasoBusquedaAvanzada> Casos;

            try
            {
                Casos = modelDataEntry.ListaCasosParaBusquedaAvanzada();
                return PartialView("CasosParaBusquedaAvanzada", Casos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /*
        // Buscar las entradas  correspondientes a un caso.
        [HttpPost]
        public JsonResult ListaEntradasDeCaso(int CodigoCaso = 0)
        {
            List<PDataEntry_Lista> Entradas = new List<PDataEntry_Lista>();
         
            try
            {
                //Entradas = modelDataEntry.ListaEntradasDeCaso(CodigoCaso);
                Entradas = modelDataEntry.ListaBloqueEntradasDeCaso(CodigoCaso);
        
        
        
                return Json(Entradas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        */

        [HttpPost]
        public PartialViewResult EntradasDeCaso(int codigoCaso = 0, int totalDeEntradas = 0, int paginaDestino = 1) // Modificacion nueva
        {
            List<PDataEntry_Lista> Entradas;

            try
            {
                Entradas = modelDataEntry.ListaBloqueEntradasDeCaso(codigoCaso, paginaDestino);
                /////////////////brenher
                int PageSize = 10;
                //int Total = Entradas.Count();
                int Total = totalDeEntradas;
                //Entradas = Entradas.OrderBy(p => p.Dae_date).Take(PageSize).ToList();
                Entradas = Entradas.Take(PageSize).ToList();
                //Entradas = Entradas.OrderBy(p => p.Dae_date).ToList();

                Int64 NroPages = Convert.ToInt32(Math.Ceiling((double)Total / PageSize));
                ViewBag.NumberOfPages = NroPages + 1;
                Double residuo = Math.Ceiling((double)Total % PageSize);

                if(residuo == 0  )                 
                   ViewBag.NumberOfPages = NroPages + 2;

                ViewBag.CurrentPage = paginaDestino;
                /////////////////////////////////
                return PartialView("EntradasDeCaso_Nuevo", Entradas);
            }
            catch(Exception ex)
            {
                throw ex;
            }             
            // return PartialView();
        }

        /*
        [HttpPost]
        public PartialViewResult EntradasDeCaso(int codigoCaso = 0, int numeroPagina = 0)
        {
            List<PDataEntry_Lista> Entradas;

            try
            {
                Entradas = modelDataEntry.ListaBloqueEntradasDeCaso(codigoCaso, numeroPagina);
                return PartialView("EntradasDeCaso", Entradas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
            // return PartialView();
        }
        */

        // Actualiza las entradas de un caso seleccionado.
        [HttpPost]
        public JsonResult RegistraActualizaEntradas(List<PDataEntry_RegistraActualiza> Entradas)
        {
            int[] Resultado;
            string EntradasXML;
            string NuevasEntradasXML;

            EntradasXML = "";
            NuevasEntradasXML = "";

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PDataEntry_RegistraActualiza>));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, Entradas);
                    EntradasXML = Convert.ToString(textWriter);
                    NuevasEntradasXML = EntradasXML.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "")
                                                   .Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                                                   .Replace(@"xsi:nil=""true""", "")
                                                   .ToString().Trim();
                    

                    Debug.WriteLine(WebUtility.HtmlDecode(NuevasEntradasXML));
                }

                Resultado = modelDataEntry.RegistraActualizaEntradas(NuevasEntradasXML);

                return Json(new { response = new { FilaActualizada = Resultado[0], FilaRegistrada = Resultado[1] } });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            /*
            string EntradasXML;
            int[] Arreglo;

            try
            {
                EntradasXML = "";

                // Armado del XML.
                // EntradasXML += "<?xml version='1.0' encoding='iso-9959-1'?>";
                EntradasXML += "<Entries>";
                foreach (var LectorEntrada in Entradas)
                {
                    Debug.WriteLine(LectorEntrada.NumeroCorrelativoEntrada);

                    EntradasXML += "<Entry>";
                    EntradasXML += "<NumeroCorrelativoEntrada>" + LectorEntrada.NumeroCorrelativoEntrada + "</NumeroCorrelativoEntrada>";
                    EntradasXML += "<Cis_code>" + LectorEntrada.Cis_code + "</Cis_code>";
                    EntradasXML += "<Dae_code>" + LectorEntrada.Dae_code + "</Dae_code>";
                    EntradasXML += "<Dae_code_old>" + LectorEntrada.Dae_code_old + "</Dae_code_old>";
                    string AñoFechaEntradaSTR = Convert.ToDateTime(LectorEntrada.Dae_date).Year.ToString();
                    string MesFechaEntradaSTR = Convert.ToDateTime(LectorEntrada.Dae_date).Month.ToString();
                    string DiaFechaEntradaSTR = Convert.ToDateTime(LectorEntrada.Dae_date).Day.ToString();
                    string FechaEntradaSTR = AñoFechaEntradaSTR + "-" + MesFechaEntradaSTR + "-" + DiaFechaEntradaSTR;
                    
                    // EntradasXML += "<Dae_date>" + LectorEntrada.Dae_date + "</Dae_date>";
                    EntradasXML += "<Dae_date>" + FechaEntradaSTR + "</Dae_date>";
                    // EntradasXML += "<Dae_date>" + Convert.ToDateTime(LectorEntrada.Dae_date).ToShortDateString() + "</Dae_date>";
                    
                    // Debug.WriteLine(LectorEntrada.Dae_date);

                    EntradasXML += "<Act_code>" + LectorEntrada.Act_code + "</Act_code>";
                    EntradasXML += "<Dae_hourAct>" + LectorEntrada.Dae_hourAct + "</Dae_hourAct>";
                    EntradasXML += "<Dae_milesAct>" + LectorEntrada.Dae_milesAct + "</Dae_milesAct>";
                    EntradasXML += "<Dae_comment>" + LectorEntrada.Dae_comment + "</Dae_comment>";
                    EntradasXML += "<Use_code>" + LectorEntrada.Use_code + "</Use_code>";
                    EntradasXML += "</Entry>";

                    //EntradasXML += "\n\t<Entry>";
                    //EntradasXML += "\n\t\t<Cis_code>" + LectorEntrada.Cis_code + "</Cis_code>";
                    //EntradasXML += "\n\t\t<Dae_code>" + LectorEntrada.Dae_code + "</Dae_code>";
                    //EntradasXML += "\n\t\t<Dae_code_old>" + LectorEntrada.Dae_code_old + "</Dae_code_old>";
                    //EntradasXML += "\n\t\t<Dae_date>" + LectorEntrada.Dae_date + "</Dae_date>";
                    //EntradasXML += "\n\t\t<Act_code>" + LectorEntrada.Act_code + "</Act_code>";
                    //EntradasXML += "\n\t\t<Dae_hourAct>" + LectorEntrada.Dae_hourAct + "</Dae_hourAct>";
                    //EntradasXML += "\n\t\t<Dae_milesAct>" + LectorEntrada.Dae_milesAct + "</Dae_milesAct>";
                    //EntradasXML += "\n\t\t<Dae_comment>" + LectorEntrada.Dae_comment + "</Dae_comment>";
                    //EntradasXML += "\n\t\t<Use_code>" + LectorEntrada.Use_code + "</Use_code>";
                    //EntradasXML += "\n\t</Entry>";
                } 
                EntradasXML += "</Entries>"; 
                //EntradasXML += "\n</Entries>";

                // Indicador = modelDataEntry.RegistraActualizaEntradas(WebUtility.HtmlEncode(EntradasXML).Replace("\n", "<br />"));
                // Indicador = modelDataEntry.RegistraActualizaEntradas(WebUtility.HtmlEncode(EntradasXML));
                Arreglo = modelDataEntry.RegistraActualizaEntradas(EntradasXML);

                // return Json(Indicador, JsonRequestBehavior.AllowGet);
                return Json(new { response = new { FilaActualizada = Arreglo[0], FilaRegistrada = Arreglo[1] } });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            */
        } 
         
        [HttpPost]
        public JsonResult EliminaEntrada(int CodigoEntrada, int CodigoUsuario)
        {
            int Resultado;

            Resultado = 0;

            try
            {
                Resultado = modelDataEntry.EliminaEntrada(CodigoEntrada, CodigoUsuario);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ListaEntradasParaConvertirEnFactura(int CodigoCaso = 0
                                                            , string FechaDeCierre = ""
                                                            , string FacNum = "-")
        {
            List<EntradaConvertidaBilling> Resultado; 

            try
            {
                Resultado = modelDataEntry.ListaEntradasParaConvertirEnFactura(CodigoCaso, FechaDeCierre, FacNum);
                return Json(Resultado, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult VerificaEntradasEnCaso(int CodigoCaso = 0)
        {
            List<EntradaVerificadaEnCaso> Resultado;

            try
            {
                Resultado = modelDataEntry.VerificaEntradasEnCaso(CodigoCaso);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                    CasosTemporal = CasosGlobalEstatica.Where(p => p.Patient.ToString().ToLower().Contains(search.ToLower()) ||
                                                                   (p.Patient3.ToString() + p.Patient2.ToString()).ToLower().Contains(search.Replace(" ","").ToLower()) ||
                                                                   (p.Patient2.ToString() + p.Patient3.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower())
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


        private List<CasoBusquedaAvanzada> SortByColumnWithOrder(string order, string orderDir, List<CasoBusquedaAvanzada> data)
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
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClaimNumber).ToList()
                                                                                                 : data.OrderBy(p => p.ClaimNumber).ToList();
                        break;
                    case "3":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Patient).ToList()
                                                                                                 : data.OrderBy(p => p.Patient).ToList();
                        break;
                    case "5":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Insurer).ToList()
                                                                                                 : data.OrderBy(p => p.Insurer).ToList();
                        break;
                    case "6":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AccidentDate).ToList()
                                                                                                 : data.OrderBy(p => p.AccidentDate).ToList();
                        break;
                    case "7":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList()
                                                                                                 : data.OrderBy(p => p.Status).ToList();
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

        public void ListaCasosParaBusquedaAvanzada_tabla()
        {
            try
            {
                CasosGlobalEstatica = modelDataEntry.ListaCasosParaBusquedaAvanzada();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void limpiaVariableGlobal()
        {
            CasosGlobalEstatica = null;
        }

        public JsonResult leeEntrada(int codigoDeEntrada = 0)
        {
            PDataEntry_Lista entrada;

            try
            {
                entrada = modelDataEntry.leeEntrada(codigoDeEntrada);
                return Json(entrada, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}