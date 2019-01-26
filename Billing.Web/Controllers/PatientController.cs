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
using System.Reflection;
using Microsoft.VisualBasic; 

namespace Billing.Web.Controllers
{
    public class PatientController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public PatientModel model = new PatientModel();

        private List<PPatient> PacientesGlobalEstatica = new List<PPatient>();
        static List<PPatient> PacientesGlobalEstaticaTrazabilidad = new List<PPatient>();

        private List<PPatient> PacientesTemporal = new List<PPatient>();
        static List<PPatient> PacientesTemporalTrazabilidad = new List<PPatient>();


        [OutputCache(Duration = 0)]
        public PartialViewResult Index()
        {
            try
            {
               var Pacientes = model.Listar();            
               return PartialView(Pacientes);                                   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public PartialViewResult IndexTest()
        {
            try
            {
                var Pacientes = model.Listar();
                return PartialView(Pacientes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult ListarPacientes()
        {
            try
            {
                var Pacientes = model.Listar();
                return Json(Pacientes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListarParaAutocompletar()
        {
            List<SP_LIST_PATIENT_FOR_FIND_MATCHES_Result> Pacientes;

            try
            {
                Pacientes = model.ListarParaAutocompletar();
                return Json(Pacientes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(Patient Paciente)
        {
            List<SP_SAVE_PATIENT_Result> Resultado;

            try
            {
                Resultado = model.Insertar(Paciente);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // return Redirect("~/Patient/index");
        }
        
        [HttpPost]
        public JsonResult Buscar(int Codigo = 0)
        {
            PPatient ObjPaciente = null;

            try
            {
                ObjPaciente = model.Buscar(Codigo);
                return Json(ObjPaciente, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            // Patient patient = db.Patients.Find(id);
            // if (patient == null)
            // {
            //     return HttpNotFound();
            // }
            // return View(patient);
        }

        [HttpPost]
        public JsonResult Actualizar(Patient Paciente)
        {
            List<SP_UPDATE_PATIENT_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Paciente);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // if (ModelState.IsValid)
            // {
            //     db.Entry(patient).State = EntityState.Modified;
            //     db.SaveChanges();
            //     return RedirectToAction("Index");
            // }
            // return View(patient);
        }

        [HttpPost]
        public JsonResult Delete(int Codigo = 0)
        {
            List<SP_DELETE_PATIENT_Result> Resultado;

            try
            {
                Resultado = model.Eliminar(Codigo);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Patient patient = db.Patients.Find(id);
            // if (patient == null)
            // {
            //     return HttpNotFound();
            // }
            // return View(patient); 
        } 

        [HttpPost]
        public JsonResult BuscarHomonimos(Patient Paciente)
        {
            List<SP_FIND_HOMONYM_PATIENTS_Result> Datos;

            try
            {
                Datos = model.BuscarHomonimos(Paciente);

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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult ActualizaSexo(int Codigo, string Sexo)
        {
            SP_UPDATE_SEX_PATIENT_Result Resultado = null;

            try
            {
                Resultado = model.ActualizaSexo(Codigo, Sexo);
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


                if (PacientesGlobalEstatica.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListarPacientesTest();

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || PacientesTemporal.Count == 0)
                {
                    PacientesTemporal = PacientesGlobalEstatica;
                }
                

                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = PacientesGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) 
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    PacientesTemporal = PacientesGlobalEstatica.Where(p => (// p.Pat_firstName.Replace(" ", "").ToString() + p.Pat_lastName.Replace(" ", "").ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) || 
                        // p.Pat_firstName.Replace(" ", "").ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           p.Pat_lastName.Replace(" ", "").ToString()).ToLower().Contains(search.Replace(" ", "").ToLower())).ToList();
                                                                           // p.Pat_birthday.ToString().ToLower().Contains(search.ToLower()) ||
                                                                           // p.Pat_socialSecurityNumberD.ToString().Contains(search.ToString()) ||
                                                                           // p.Pat_address.Replace(" ", "").ToLower().Trim().Contains(search.ToLower()) ||
                                                                           // p.Pat_city.ToLower().Contains(search.ToLower()) ||
                                                                           // p.Pat_state.ToLower().Contains(search.ToLower()) ||
                                                                           // p.Pat_zipCode.ToLower().Contains(search.ToLower()) ||
                                                                           // p.Pat_zipCodeExt.ToLower().Contains(search.ToLower()) ||
                                                                           // p.Pat_phone.ToLower().Contains(search.ToLower()) ||
                                                                           // p.Pat_fax.ToString().ToLower().Contains(search.ToLower()) ||
                                                                           // p.Pat_sex.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                PacientesTemporal = this.SortByColumnWithOrder(order, orderDir, PacientesTemporal);

                // Filter record count.
                int recFilter = PacientesTemporal.Count;

                // Apply pagination.
                PacientesTemporal = PacientesTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = PacientesTemporal }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetDataTrazabilidad()
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


                if (PacientesGlobalEstaticaTrazabilidad.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListarParaTrazabilidad();

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || PacientesTemporalTrazabilidad.Count == 0)
                {
                    PacientesTemporalTrazabilidad = PacientesGlobalEstaticaTrazabilidad;
                }


                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = PacientesGlobalEstaticaTrazabilidad.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    PacientesTemporalTrazabilidad = PacientesGlobalEstaticaTrazabilidad.Where(p => (p.Pat_firstName.ToString() + p.Pat_lastName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                                    p.Pat_birthday.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                    p.Pat_socialSecurityNumberD.ToString().Contains(search.ToString()) ||
                                                                                                    p.Pat_address.ToLower().Trim().Contains(search.ToLower()) ||
                                                                                                    p.Pat_city.ToLower().Contains(search.ToLower()) ||
                                                                                                    p.Pat_state.ToLower().Contains(search.ToLower()) ||
                                                                                                    p.Pat_zipCode.ToLower().Contains(search.ToLower()) ||
                                                                                                    p.Pat_zipCodeExt.ToLower().Contains(search.ToLower()) ||
                                                                                                    p.Pat_phone.ToLower().Contains(search.ToLower()) ||
                                                                                                    p.Pat_fax.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                    p.Pat_sex.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                PacientesTemporalTrazabilidad = this.SortByColumnWithOrder(order, orderDir, PacientesTemporalTrazabilidad);

                // Filter record count.
                int recFilter = PacientesTemporalTrazabilidad.Count;

                // Apply pagination.
                PacientesTemporalTrazabilidad = PacientesTemporalTrazabilidad.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = PacientesTemporalTrazabilidad }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ListarPacientesTest()
        {
            try
            {
                PacientesGlobalEstatica = model.Listar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ListarParaTrazabilidad()
        {
            try
            {
                PacientesGlobalEstaticaTrazabilidad = model.ListarParaTrazabilidad();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private List<PPatient> SortByColumnWithOrder(string order, string orderDir, List<PPatient> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_firstName).ToList()
                                                                                                 : data.OrderBy(p => p.Pat_firstName).ToList();
                        break;

                    case "1":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_lastName).ToList()
                                                                                                 : data.OrderBy(p => p.Pat_lastName).ToList();
                        break;
                        
                         case "2":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_birthday).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_birthday).ToList();
                             break;
                         
                         case "3":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_socialSecurityNumberD).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_socialSecurityNumberD).ToList();
                             break;
                         
                         case "4":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_address).ToList()
                                                                                                        : data.OrderBy(p => p.Pat_address).ToList();
                             break;
                         
                         case "5":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_city).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_city).ToList();
                             break;                         
                         case "6":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_state).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_state).ToList();
                             break;
                        case "7":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_zipCode).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_zipCode).ToList();
                             break;
                        case "8":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_zipCodeExt).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_zipCodeExt).ToList();
                             break;
                        case "9":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_phone).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_phone).ToList();
                             break;
                        case "10":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_fax).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_fax).ToList();
                             break;
                        case "11":
                             // Setting.
                             data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pat_sex).ToList()
                                                                                                      : data.OrderBy(p => p.Pat_sex).ToList();
                             break;
                         
                        // default:
                         
                        //     // Setting.
                        //     data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Sr).ToList() 
                        //                                                                             : data.OrderBy(p => p.Sr).ToList();
                        // break;
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







    }
}