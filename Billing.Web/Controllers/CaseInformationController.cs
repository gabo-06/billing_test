using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using Billing.Web.Controllers;
using Billing.Web.Models;
using System.Diagnostics; 
using System.Data.Objects;


using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.xml.simpleparser;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml; 
using System.IO;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;

//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.xml;


 
namespace Billing.Web.Controllers
{
    public class CaseInformationController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public CaseModel model = new CaseModel();
        public StateModel model2 = new StateModel();
        public SpecialtyModel model3 = new SpecialtyModel();

        private List<CasoBusquedaAvanzada> CasosGlobalEstatica = new List<CasoBusquedaAvanzada>();
        private List<CasoBusquedaAvanzada> CasosTemporal = new List<CasoBusquedaAvanzada>();

        // ******************************************************************************************
        //[OutputCache(Duration = 3600, VaryByParam="none", Location=OutputCacheLocation.ServerAndClient ) ]
        public PartialViewResult Index()
        {
            //aqui funcion para retornar tipo de permiso
            //AccountController Account = new AccountController();
            //int data = Account.LeerPermisoUsuarioLogueado();
            //if (data == 0 )
            //    ViewBag.tipoPermiso = "digit";
            //else
            //    ViewBag.tipoPermiso = "Aministrator";
                

            return PartialView();
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
                    this.ListaCasosParaBusquedaAvanzada_tabla();//tambien se usa el mismo metodo para data entry

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
                    //CasosTemporal = CasosGlobalEstatica.Where(p =>p.Patient.ToString().ToLower().Contains(search.ToLower())).ToList();
                    CasosTemporal = CasosGlobalEstatica.Where(p => p.Patient.ToString().ToLower().Contains(search.ToLower()) ||
                                               (p.Patient3.ToString() + p.Patient2.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                               (p.Patient2.ToString() + p.Patient3.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower())
                                          ).ToList(); 
                }

                // Sorting.
                CasosTemporal = this.SortByColumnWithOrder(order, orderDir, CasosTemporal);

                // Filter record count.
                int recFilter = CasosTemporal.Count;

                // Apply pagination.
                CasosTemporal = CasosTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(
                    new 
                    { 
                        draw = Convert.ToInt32(draw), 
                        recordsTotal = totalRecords, 
                        recordsFiltered = recFilter, 
                        data = CasosTemporal 
                    },
                    JsonRequestBehavior.AllowGet);
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

        // Método que lista los casos para una búsqueda avanzada.
        public void ListaCasosParaBusquedaAvanzada_tabla()
        {            
            try
            {
                CasosGlobalEstatica = model.ListaCasosParaBusquedaAvanzada();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public JsonResult VerificarDisponibilidadCaso(int CodigoCasoSeleccionado = 0, int CodigoUsuarioQueLee = 0)
        {
            SP_CHECK_AVAILABILITY_CASE_Result Resultado;
            DataTable[] ArregloTablas;      // Arreglo donde se reciben las dos tablas 
            DataTable TEstadoOcupacionCaso;
            DataTable TDatosCaso;
            object[] ValoresEstadoOcupacion = new object[2]; // Arreglo de enteros que almacena los valores de estado de ocupación del caso (Busy: 0 o 1 y User: Código de usuario que ocupa el caso)
            List<Pcase> listaCasos = new List<Pcase>();  // Lista de detalles procesados que se enviarán a AJAX.
            
            // Indices de las columnas de la tabla que trae la información del caso leído.
            int iCantidadTotalEntradas;
            // .
            // .
            // .

            try
            {
                ArregloTablas = model.VerificarDisponibilidadCaso(CodigoCasoSeleccionado, CodigoUsuarioQueLee);

                // Obtiene las tablas
                TEstadoOcupacionCaso = ArregloTablas[0];
                TDatosCaso           = ArregloTablas[1];

                // Obtiene los índices de las columnas de la tabla que trae la información del caso leído.
                iCantidadTotalEntradas = TDatosCaso.Columns["CantidadTotalEntradas"].Ordinal;
                // .
                // .
                // .

                // Arma el objeto del esatdo de ocupación.
                List<Object> ocupacion = new List<Object>()
                {
                    new { busy = TEstadoOcupacionCaso.Rows[0].ItemArray[0], user = TEstadoOcupacionCaso.Rows[0].ItemArray[1] }
                };

                // Arma el objeto que contiene el caso.
                foreach (DataRow Detalle in TDatosCaso.Rows)
                {
                    /* DECLARACION DE VARIABLES */
                    Pcase Caso = new Pcase();
                    // Fecha de accidente, fecha referencial y fecha de nacimiento del paciente.
                    DateTime FA_DT, FR_DT, FN_DT;
                    // Mes, día y año de la fecha de accidente. Mes, día y año de la fecha referencial. Mes, día y año de la fecha de nacimiento del paciente.
                    string MesFA_STR, DiaFA_STR, AñoFA_STR, MesFR_STR, DiaFR_STR, AñoFR_STR, MesFN_STR, DiaFN_STR, AñoFN_STR;

                    /* INICIALIZACION DE VARIABLES */
                    FA_DT = FR_DT = FN_DT = Convert.ToDateTime(null);
                    MesFA_STR = DiaFA_STR = AñoFA_STR = MesFR_STR = DiaFR_STR = AñoFR_STR = MesFN_STR = DiaFN_STR = AñoFN_STR = "";
                    
                    /* ASIGNACION */
                    Caso.CaseCode = (Detalle.ItemArray.GetValue(0).GetType().Equals(typeof(System.DBNull))) ? int.Parse(null) : int.Parse(Detalle.ItemArray.GetValue(0).ToString());
                    Caso.CaseCaseCode = (Detalle.ItemArray.GetValue(1).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(1).ToString();
                    Caso.Patient = (Detalle.ItemArray.GetValue(2).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(2).ToString();
                    // -------------------------------------------------------------------------------
                    FA_DT = (Detalle.ItemArray.GetValue(3).GetType().Equals(typeof(System.DBNull))) ? DateTime.ParseExact("01/01/1920", "d", CultureInfo.InvariantCulture) : DateTime.Parse(Detalle.ItemArray.GetValue(3).ToString());
                    if (!FA_DT.GetType().Equals(typeof(System.DBNull))) 
                    {
                        MesFA_STR = FA_DT.Month.ToString().PadLeft(2, '0');
                        DiaFA_STR = FA_DT.Day.ToString().PadLeft(2, '0');
                        AñoFA_STR = FA_DT.Year.ToString();
                        Caso.CaseAccidentDate = MesFA_STR + '/' + DiaFA_STR + '/' + AñoFA_STR;
                    }
                    // -------------------------------------------------------------------------------
                    Caso.Insurer = (Detalle.ItemArray.GetValue(4).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(4).ToString();
                    Caso.Adjuster = (Detalle.ItemArray.GetValue(5).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(5).ToString();
                    // -------------------------------------------------------------------------------
                    FN_DT = (Detalle.ItemArray.GetValue(6).GetType().Equals(typeof(System.DBNull))) ? DateTime.ParseExact("01/01/1920", "d", CultureInfo.InvariantCulture) : DateTime.Parse(Detalle.ItemArray.GetValue(6).ToString());
                    if (!FN_DT.GetType().Equals(typeof(System.DBNull)))
                    {
                        MesFN_STR = FN_DT.Month.ToString().PadLeft(2, '0');
                        DiaFN_STR = FN_DT.Day.ToString().PadLeft(2, '0');
                        AñoFN_STR = FN_DT.Year.ToString();
                        
                        Caso.Birthday = MesFN_STR + '/' + DiaFN_STR + '/' + AñoFN_STR;
                    }
                    // -------------------------------------------------------------------------------
                    Caso.CaseContact1   = (Detalle.ItemArray.GetValue(7).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(7).ToString();
                    Caso.CaseContact2   = (Detalle.ItemArray.GetValue(8).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(8).ToString();
                    Caso.CaseSupervisor = (Detalle.ItemArray.GetValue(9).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(9).ToString();
                    Caso.cantidadTotalDeEntradas = (Detalle.ItemArray.GetValue(iCantidadTotalEntradas).GetType().Equals(typeof(System.DBNull))) ? 0 : int.Parse(Detalle.ItemArray.GetValue(iCantidadTotalEntradas).ToString());
                    
                    /*
                    Caso.CaseCode                    = (Detalle.ItemArray.GetValue(0).GetType().Equals(typeof(System.DBNull))) ? int.Parse(null)     : int.Parse(Detalle.ItemArray.GetValue(0).ToString());
                    Caso.PatientCode                 = (Detalle.ItemArray.GetValue(1).GetType().Equals(typeof(System.DBNull))) ? (int?)null          : int.Parse(Detalle.ItemArray.GetValue(1).ToString());
                    Caso.AdjusterCode                = (Detalle.ItemArray.GetValue(2).GetType().Equals(typeof(System.DBNull))) ? (int?)null          : int.Parse(Detalle.ItemArray.GetValue(2).ToString());
                    Caso.InsurerCode                 = (Detalle.ItemArray.GetValue(3).GetType().Equals(typeof(System.DBNull))) ? (int?)null          : int.Parse(Detalle.ItemArray.GetValue(3).ToString());
                    Caso.ProviderCode                = (Detalle.ItemArray.GetValue(4).GetType().Equals(typeof(System.DBNull))) ? (int?)null          : int.Parse(Detalle.ItemArray.GetValue(4).ToString());
                    Caso.CasePrice                   = (Detalle.ItemArray.GetValue(5).GetType().Equals(typeof(System.DBNull))) ? decimal.Parse(null) : decimal.Parse(Detalle.ItemArray.GetValue(5).ToString());
                    // -------------------------------------------------------------------------------
                    FA_DT = (Detalle.ItemArray.GetValue(6).GetType().Equals(typeof(System.DBNull))) ? DateTime.Parse(null) : DateTime.Parse(Detalle.ItemArray.GetValue(6).ToString());
                    if (!FA_DT.GetType().Equals(typeof(System.DBNull)))
                    {
                        MesFA_STR = FA_DT.Month.ToString().PadLeft(2, '0');
                        DiaFA_STR = FA_DT.Day.ToString().PadLeft(2, '0');
                        AñoFA_STR = FA_DT.Year.ToString();
                        Caso.CaseAccidentDate = MesFA_STR + '/' + DiaFA_STR + '/' + AñoFA_STR;
                    }
                    // -------------------------------------------------------------------------------
                    Caso.CaseCaseCode = (Detalle.ItemArray.GetValue(7).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(7).ToString();
                    // -------------------------------------------------------------------------------                    
                    FR_DT = (Detalle.ItemArray.GetValue(8).GetType().Equals(typeof(System.DBNull))) ? DateTime.Parse(null) : DateTime.Parse(Detalle.ItemArray.GetValue(8).ToString());
                    if (!FR_DT.GetType().Equals(typeof(System.DBNull)))
                    {
                        MesFR_STR = FR_DT.Month.ToString().PadLeft(2, '0');
                        DiaFR_STR = FR_DT.Day.ToString().PadLeft(2, '0');
                        AñoFR_STR = FR_DT.Year.ToString();
                        Caso.CaseReferralDate = MesFR_STR + '/' + DiaFR_STR + '/' + AñoFR_STR;
                    }
                    // -------------------------------------------------------------------------------
                    Caso.CaseTransportation = (Detalle.ItemArray.GetValue(9).GetType().Equals(typeof(System.DBNull))) ? (bool?)null : bool.Parse(Detalle.ItemArray.GetValue(9).ToString());
                    Caso.CaseComment =(Detalle.ItemArray.GetValue(10).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(10).ToString();
                    Caso.CaseInjury = (Detalle.ItemArray.GetValue(11).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(11).ToString();
                    Caso.CaseStatus = (Detalle.ItemArray.GetValue(12).GetType().Equals(typeof(System.DBNull))) ? (bool?)null : bool.Parse(Detalle.ItemArray.GetValue(12).ToString());
                    Caso.Patient = (Detalle.ItemArray.GetValue(13).GetType().Equals(typeof(System.DBNull))) ? null : Detalle.ItemArray.GetValue(13).ToString();
                    // -------------------------------------------------------------------------------
                    FN_DT = (Detalle.ItemArray.GetValue(14).GetType().Equals(typeof(System.DBNull))) ? DateTime.Parse(null) : DateTime.Parse(Detalle.ItemArray.GetValue(14).ToString());
                    if (!FN_DT.GetType().Equals(typeof(System.DBNull)))
                    { 
                        MesFN_STR = FN_DT.Month.ToString().PadLeft(2, '0');
                        DiaFN_STR = FN_DT.Day.ToString().PadLeft(2, '0');
                        AñoFN_STR = FN_DT.Year.ToString();
                        Caso.Birthday = MesFN_STR + '/' + DiaFN_STR + '/' + AñoFN_STR;
                    }
                    // -------------------------------------------------------------------------------
                    Caso.Provider                    = (Detalle.ItemArray.GetValue(15).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(15).ToString();
                    Caso.Adjuster                    = (Detalle.ItemArray.GetValue(16).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(16).ToString();
                    Caso.AdjusterPhone               = (Detalle.ItemArray.GetValue(17).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(17).ToString();
                    Caso.AdjusterPhoneExt            = (Detalle.ItemArray.GetValue(18).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(18).ToString();
                    Caso.Insurer                     = (Detalle.ItemArray.GetValue(19).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(19).ToString();
                    Caso.InsurerAddress              = (Detalle.ItemArray.GetValue(20).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(20).ToString();
                    Caso.InsurerCity                 = (Detalle.ItemArray.GetValue(21).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(21).ToString();
                    Caso.InsurerState                = (Detalle.ItemArray.GetValue(22).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(22).ToString();
                    Caso.InsurerZipCode              = (Detalle.ItemArray.GetValue(23).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(23).ToString();
                    Caso.InsurerZipCodeExt           = (Detalle.ItemArray.GetValue(24).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(24).ToString();
                    Caso.InsurerPhone                = (Detalle.ItemArray.GetValue(25).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(25).ToString();
                    Caso.InsurerPhoneExt             = (Detalle.ItemArray.GetValue(26).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(26).ToString();
                    Caso.InsurerFax                  = (Detalle.ItemArray.GetValue(27).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(27).ToString();
                    Caso.CaseTransportationCompany   = (Detalle.ItemArray.GetValue(28).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(28).ToString();
                    Caso.CaseTranslation             = (Detalle.ItemArray.GetValue(29).GetType().Equals(typeof(System.DBNull))) ? (bool?)null      : bool.Parse(Detalle.ItemArray.GetValue(29).ToString());
                    Caso.CaseTranslationCompany      = (Detalle.ItemArray.GetValue(30).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(30).ToString();
                    Caso.CasePhysicalTherapy         = (Detalle.ItemArray.GetValue(31).GetType().Equals(typeof(System.DBNull))) ? (bool?)null      : bool.Parse(Detalle.ItemArray.GetValue(31).ToString());
                    Caso.CasePhysicalTherapyCompany  = (Detalle.ItemArray.GetValue(32).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(32).ToString();
                    Caso.CasePermisionContact        = (Detalle.ItemArray.GetValue(33).GetType().Equals(typeof(System.DBNull))) ? (bool?)null      : bool.Parse(Detalle.ItemArray.GetValue(33).ToString());
                    Caso.CasePermisionContactCompany = (Detalle.ItemArray.GetValue(34).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(34).ToString();
                    Caso.CaseLongshore               = (Detalle.ItemArray.GetValue(35).GetType().Equals(typeof(System.DBNull))) ? (bool?)null      : bool.Parse(Detalle.ItemArray.GetValue(35).ToString());
                    Caso.CaseCompanyStatus           = (Detalle.ItemArray.GetValue(36).GetType().Equals(typeof(System.DBNull))) ? (bool?)null      : bool.Parse(Detalle.ItemArray.GetValue(36).ToString());
                    Caso.CaseOther                   = (Detalle.ItemArray.GetValue(37).GetType().Equals(typeof(System.DBNull))) ? (bool?)null      : bool.Parse(Detalle.ItemArray.GetValue(37).ToString());
                    Caso.CaseOtherText               = (Detalle.ItemArray.GetValue(38).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(38).ToString();
                    Caso.CaseContact1                = (Detalle.ItemArray.GetValue(39).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(39).ToString();
                    Caso.CaseContact2                = (Detalle.ItemArray.GetValue(40).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(40).ToString();
                    Caso.CaseSupervisorCode          = (Detalle.ItemArray.GetValue(41).GetType().Equals(typeof(System.DBNull))) ? (int?)null       : int.Parse(Detalle.ItemArray.GetValue(41).ToString());
                    Caso.CasePresumption             = (Detalle.ItemArray.GetValue(42).GetType().Equals(typeof(System.DBNull))) ? (bool?)null      : bool.Parse(Detalle.ItemArray.GetValue(42).ToString());
                    Caso.CaseAcuity                  = (Detalle.ItemArray.GetValue(43).GetType().Equals(typeof(System.DBNull))) ? byte.Parse(null) : byte.Parse(Detalle.ItemArray.GetValue(43).ToString());
                    Caso.CaseSupervisor              = (Detalle.ItemArray.GetValue(44).GetType().Equals(typeof(System.DBNull))) ? null             : Detalle.ItemArray.GetValue(44).ToString();
                    */

                    listaCasos.Add(Caso);
                }

                // Retorna un objeto JSON que contiene dos objetos "estadoOcupacion" y "listaCasos"
                return Json(new { ocupacion = ocupacion, listaCasos = listaCasos });
            }
            catch (Exception ex) 
            {
                throw ex;
            } 
        }

        /* (2) */
        // Este procedimiento obtiene los datos del caso, en caso esté desocupado
        public JsonResult ObtenerDatosCaso(int CodigoCasoALeer = 0)
        {
            //SP_GET_CASE_DATA_Result Resultado;
            Pcase Resultado;

            try
            {
                Resultado = model.ObtenerDatosCaso(CodigoCasoALeer);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { 
                throw ex;
            } 
        }  

        /* (3) */
        // Este procedimiento ocupa el caso leído y libera el caso cargado anteriormente.
        public void OcupaLiberaCasos(int CodigoCasoLeido = 0, int CodigoUsuarioQueLee = 0, int CodigoCasoALiberar = 0)
        {
            try
            {
                model.OcupaLiberaCasos(CodigoCasoLeido
                                      , CodigoUsuarioQueLee
                                      , CodigoCasoALiberar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Obtiene el estado de un caso según un paciente.
        public JsonResult obtenerEstadoDeCaso(int codigoDeCaso = 0)
        {
            bool estadoDeCaso;

            try
            {
                estadoDeCaso = model.obtenerEstadoDeCaso(codigoDeCaso);
                return Json(estadoDeCaso, JsonRequestBehavior.AllowGet);
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

        // Libera todos los casos
        public void LiberaTodosLosCasos()
        {
            try
            {
                model.LiberaTodosLosCasos();
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
                if (Caso.Cis_referralDate == default(DateTime))
                    Caso.Cis_referralDate = Convert.ToDateTime("1900-01-01");

                Resultado = model.Insertar(Caso);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Actualizar(CaseInformationSheetHead Caso)
        {
            List<SP_UPDATE_CASE_Result> Resultado;

            try
            {
                if (Caso.Cis_referralDate == default(DateTime))
                    Caso.Cis_referralDate = Convert.ToDateTime("1900-01-01");

                Resultado = model.Actualizar(Caso);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /* FECHA: 29/09/2016
         * RAY DIAZ
         * METODO QUE TRAE TODA LA DATA INICIAL PARA CARGAR LA VISTA CASE INFORMATION EN UNA SOLA LLAMADA
         */

        [HttpPost]
        public JsonResult obtenerDataInicial_case_info()
        {
            List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> Resultado;            
            try
            {
                Resultado = model.obtener_data_inicial_case_info();

                List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> adjusters;
                List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> attorneys;
                List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> insurers;
                List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> patients;
                List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> providers;
                List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> supervisors;
                List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> medicals;

                adjusters = Resultado.Where(p => p.tipo.Equals("ADJUSTER")).ToList();
                attorneys = Resultado.Where(p => p.tipo.Equals("ATTORNEY")).ToList();
                insurers = Resultado.Where(p => p.tipo.Equals("INSURER")).ToList();
                patients = Resultado.Where(p => p.tipo.Equals("PATIENT")).ToList();
                providers = Resultado.Where(p => p.tipo.Equals("PROVIDER")).ToList();
                supervisors = Resultado.Where(p => p.tipo.Equals("SUPERVISOR")).ToList();
                medicals = Resultado.Where(p => p.tipo.Equals("MEDICAL")).ToList();
                
                var jsonResult = Json(new
                {
                    response = new
                    {
                        adjusters = adjusters,
                        attorneys = attorneys,
                        insurers = insurers,
                        patients = patients,
                        providers = providers,
                        supervisors = supervisors,
                        medicals = medicals                        
                    }
                }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost]
        //[OutputCache(Duration = 3600, VaryByParam="none", Location=OutputCacheLocation.ServerAndClient  )]
        public JsonResult obtener_especialidades_estados()
        {            
            try
            {
                JsonResult jsonResult = (JsonResult)null; 
                var dato = Session["especialidades_estados"];


                if (dato == null)
                {
                    Session["especialidades_estados"] = obtener_especialidades_estados_informacion();
                    dato = Session["especialidades_estados"];
                }
                else
                    jsonResult = (JsonResult)dato; 
                
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult obtener_especialidades_estados_informacion()
        {
            List<Specialty> Lista = new List<Specialty>();
            List<SP_LIST_CITY_STATE_Result> listalugares = new List<SP_LIST_CITY_STATE_Result>();
            try
            {
                JsonResult jsonResult = (JsonResult)null;



                    Lista = model3.ListaEspecialidades();
                    List<Specialty> especialidadMedicals = new List<Specialty>();
                    List<Specialty> especialidadAttorneys = new List<Specialty>();

                    especialidadMedicals = Lista.Where(p => p.Spe_type.Equals("Medical")).ToList();
                    especialidadAttorneys = Lista.Where(p => p.Spe_type.Equals("Attorney")).ToList();



                    listalugares = model2.ListaEstadosyCiudades();
                    List<SP_LIST_CITY_STATE_Result> estados = new List<SP_LIST_CITY_STATE_Result>();
                    List<SP_LIST_CITY_STATE_Result> ciudades = new List<SP_LIST_CITY_STATE_Result>();

                    estados = listalugares.Where(p => p.tipo.Equals("E")).ToList();
                    ///ciudades = listalugares.Where(p => p.tipo.Equals("C")).ToList();

                    jsonResult = Json(new
                    {
                        response = new
                        {
                            especialidadMedicals = especialidadMedicals,
                            especialidadAttorneys = especialidadAttorneys,
                            estados = estados,
                           // ciudades = ciudades
                        }
                    }, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;


                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ///////////////////////////////PDF/////////////////////////////////////////////
        

        public void DownloadPDF(Int32 codigoCase, Int32 codigoPaciente, String pacienteNombre="")
        {

            String newFile = "", nuevaruta = "";
            String serverPath = System.Web.HttpContext.Current.Server.MapPath("~");
            String ruta = "";

            ruta = "\\Archivos\\Formato\\";
            nuevaruta = "TemplateTemporal.pdf";


            String archivoBase = "";
            String pdfTemplate = "";


            String rutaV = serverPath + "\\Archivos\\TemplateTemporal\\";
            archivoBase = "PDFCase.pdf";
            
            pdfTemplate = serverPath + ruta + archivoBase;
            newFile = rutaV + nuevaruta;

            leeRutaTemporal(pdfTemplate, newFile, codigoCase,codigoPaciente);//Aqui se llena la plantilla con una hoja, se guarda en esta direccion y en base a esta direccion en el sgt metoto(GeneraPdf)agrego las demas hojas

            Response.Clear();
            Response.ContentType = "application/pdf";

            Response.AddHeader("content-disposition", "attachment;filename=" + "CaseInformation_" + pacienteNombre + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.WriteFile(newFile);
            //Response.WriteFile(GeneraPdf(pdfTemplate));
            Response.WriteFile(GeneraPdf(newFile, codigoCase));
            Response.End();

        }
        
        

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

                    ///CodigoPostal                    
                    cell = new PdfPCell(new Phrase(d.CodigoPostal, tipoTexto_F));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderWidth = 0f;
                    table.AddCell(cell);

                    ///ExtensionCodigoPostal         
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
                    cell = new PdfPCell(new Phrase(d.CodigoPostal, tipoTexto_F));
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

        

        
        private void leeRutaTemporal(string template, string newFile, Int32 codigoCase, Int32 codigoPaciente)
        {

            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;

            pdfReader = new PdfReader(template);
            pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;


            PdfContentByte pdfContentByte = pdfStamper.GetOverContent(1);
            //    //// llama metodo 
            var data = ObtenerDatosCaso(codigoCase);
            PatientController Paciente = new PatientController();
            var dataPaciente = Paciente.Buscar(codigoPaciente);
            
            
            var Objeto = (Pcase)data.Data; // parseo al tipo de bjeto que deseo, en este caso el tipo que tengo en el modelo
            var ObjetoPaciente = (PPatient)dataPaciente.Data; // parseo al tipo de bjeto que deseo, en este caso el tipo que tengo en el modelo


            DateTime dbo = Convert.ToDateTime(ObjetoPaciente.Pat_birthday);
            String cadenaDbo = dbo.Month +"/"+ dbo.Day + "/" + dbo.Year;

            DateTime doi = Convert.ToDateTime(Objeto.CaseAccidentDate);
            String cadenaDoi = doi.Month + "/" + doi.Day + "/" + doi.Year;
            
            DateTime CaseReferralDate = Convert.ToDateTime(Objeto.CaseReferralDate);
            String cadenaCaseReferralDate = CaseReferralDate.Month + "/" + CaseReferralDate.Day + "/" + CaseReferralDate.Year;

           


            //DateTime dt  = DateTime.ParseExact(ObjetoPaciente.Pat_birthday,"mm/dd/yyyy",cultureInfo.)
            // Asigna los campos
            pdfFormFields.SetField("patient", Objeto.Patient);
            pdfFormFields.SetField("ssnumber", ObjetoPaciente.Pat_socialSecurityNumberD);
            pdfFormFields.SetField("dob", cadenaDbo);
            pdfFormFields.SetField("streetpat", ObjetoPaciente.Pat_address);
            pdfFormFields.SetField("citypat", ObjetoPaciente.Pat_city);
            pdfFormFields.SetField("statepat", ObjetoPaciente.Pat_state);
            pdfFormFields.SetField("zippat", ObjetoPaciente.Pat_zipCode);
            pdfFormFields.SetField("zipextpat", ObjetoPaciente.Pat_zipCodeExt);
            pdfFormFields.SetField("phonepat", ObjetoPaciente.Pat_phone);
            pdfFormFields.SetField("alternativephonepat", ObjetoPaciente.Pat_fax);
            pdfFormFields.SetField("doi", cadenaDoi);
            pdfFormFields.SetField("injury", Objeto.CaseInjury);
            pdfFormFields.SetField("price", Convert.ToString(Objeto.CasePrice));
            pdfFormFields.SetField("referal",cadenaCaseReferralDate);
            pdfFormFields.SetField("provider", Objeto.Provider);

            pdfFormFields.SetField("longyes", Objeto.CaseLongshore.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("statecompyes", Objeto.CaseCompanyStatus.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("otheryes", Objeto.CaseOther.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("other_text", Objeto.CaseOtherText);

            pdfFormFields.SetField("claim", Objeto.CaseCaseCode);
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
            pdfFormFields.SetField("trasno", Objeto.CaseTranslation.Equals(false) ? "1" : "0");
            pdfFormFields.SetField("traslation_comp", Objeto.CaseTranslationCompany);
            pdfFormFields.SetField("tranyes", Objeto.CaseTransportation.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("tranno", Objeto.CaseTransportation.Equals(false) ? "1" : "0");
            pdfFormFields.SetField("transportation_comp", Objeto.CaseTransportationCompany);
            pdfFormFields.SetField("phyyes", Objeto.CasePhysicalTherapy.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("phyno", Objeto.CasePhysicalTherapy.Equals(false) ? "1" : "0");
            pdfFormFields.SetField("physical_comp", Objeto.CasePhysicalTherapyCompany);
            pdfFormFields.SetField("permyes", Objeto.CasePermisionContact.Equals(true) ? "1" : "0");
            pdfFormFields.SetField("permno", Objeto.CasePermisionContact.Equals(false) ? "1" : "0");
            pdfFormFields.SetField("perm_comp", Objeto.CasePermisionContactCompany);
            pdfFormFields.SetField("notes", Objeto.CaseComment);



            pdfStamper.FormFlattening = true;

            if (pdfStamper != null) pdfStamper.Close();
            if (pdfReader != null) pdfReader.Close();
        }
        
        [HttpPost]
        public JsonResult VerificaPrecio(int CodigoCaso = 0)
        {
            try
            {
                var CantidadCasos = model.VerificaPrecio(CodigoCaso);
                return Json(CantidadCasos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult testListaCasosParaBusquedaAvanzada()
        {
            List<CasoBusquedaAvanzada> Casos;
            try
            {
                Casos = model.ListaCasosParaBusquedaAvanzada();
                return Json(Casos, JsonRequestBehavior.AllowGet);
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


    }
}