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

namespace Billing.Web.Controllers
{
    public class MedicalController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public MedicalModel model = new MedicalModel();

        static List<PMedical> MedicosGlobalEstatica = new List<PMedical>();
        static List<PMedical> MedicosGlobalEstaticaTrazabilidad = new List<PMedical>();

        static List<PMedical> MedicosTemporal = new List<PMedical>();
        static List<PMedical> MedicosTemporalTrazabilidad = new List<PMedical>();

        public PartialViewResult Index()
        {
            try
            {
                var Doctores = model.ListarDoctores();
                return PartialView(Doctores);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListarParaAutocompletar()
        {
            List<SP_LIST_MEDICAL_FOR_FIND_MATCHES_Result> Doctores;

            try
            {
                Doctores = model.ListarParaAutocompletar();
                return Json(Doctores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Create(Medical Doctor)
        {
            List<SP_SAVE_MEDICAL_Result> Resultado;

            try
            {
                Resultado = model.InsertarDoctor(Doctor);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ObtenerDatosParaEditar")]
        public JsonResult Edit(int CodigoDoctor = 0)
        {
            PMedical ObjDoctor = null;

            try
            {
                ObjDoctor = model.BuscarDoctor(CodigoDoctor);
                return Json(ObjDoctor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ActualizarDatos")]
        public JsonResult Edit(Medical Doctor)
        {
            List<SP_UPDATE_MEDICAL_Result> Resultado;

            try
            {
                Resultado = model.ActualizarDoctor(Doctor);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Delete(int CodigoDoctor = 0)
        {
            List<SP_DELETE_MEDICAL_Result> Resultado;

            try
            {
                Resultado = model.EliminarDoctor(CodigoDoctor);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult BuscarHomonimos(Medical Doctor)
        {
            List<SP_FIND_HOMONYM_MEDICALS_Result> Datos;

            try
            {
                Datos = model.BuscarHomonimos(Doctor);

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

                if (MedicosGlobalEstatica.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListaMedicosTabla();//tambien se usa el mismo metodo para data entry

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || MedicosTemporal.Count == 0)
                {
                    MedicosTemporal = MedicosGlobalEstatica;
                }

                // Total record count.
                // int totalRecords = lst.Count;e3
                int totalRecords = MedicosGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    MedicosTemporal = MedicosGlobalEstatica.Where(p => ( // p.Med_firstName.Replace(" ", "").ToString() + p.Med_lastName.Replace(" ", "").ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) || 
                                                                         // p.Med_firstName.Replace(" ", "").ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                         p.Med_lastName.Replace(" ", "").ToString()).ToLower().Trim().Contains(search.Replace(" ", "").ToLower())).ToList();
                                                                         // p.Med_address.Replace(" ", "").ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                         // p.Med_city.ToString().ToLower().Contains(search.ToLower()) ||
                                                                         // p.Med_state.ToString().ToLower().Contains(search.ToLower()) ||
                                                                         // p.Med_zipCode.ToString().ToLower().Contains(search.ToLower()) ||
                                                                         // p.Med_zipCodeExt.ToString().ToLower().Contains(search.ToLower()) ||
                                                                         // p.Med_phone.ToString().ToLower().Contains(search.ToLower()) ||
                                                                         // p.Med_alternatePhone.ToString().ToLower().Contains(search.ToLower()) ||
                                                                         // p.Med_fax.ToString().ToLower().Contains(search.ToLower()) ||
                                                                         // p.Specialty.Spe_name.ToString().ToLower().Contains(search.ToLower()) ||
                                                                         // p.Med_office.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                MedicosTemporal = this.SortByColumnWithOrder(order, orderDir, MedicosTemporal);

                // Filter record count.
                int recFilter = MedicosTemporal.Count;

                // Apply pagination.
                MedicosTemporal = MedicosTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = MedicosTemporal }, JsonRequestBehavior.AllowGet);
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

        public void ListarParaTrazabilidad()
        {
            try
            {
                MedicosGlobalEstaticaTrazabilidad = model.ListarDoctoresParaTrazabilidad();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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


                if (MedicosGlobalEstaticaTrazabilidad.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListarParaTrazabilidad();

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || MedicosTemporalTrazabilidad.Count == 0)
                {
                    MedicosTemporalTrazabilidad = MedicosGlobalEstaticaTrazabilidad;
                }

                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = MedicosGlobalEstaticaTrazabilidad.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    MedicosTemporalTrazabilidad = MedicosGlobalEstaticaTrazabilidad.Where(p => (// p.Med_firstName.ToString() + p.Med_lastName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                               // (p.Med_firstName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                               p.Med_lastName.ToString()).ToLower().Contains(search.Replace(" ", "").ToLower())).ToList();
                                                                                                // p.Med_address.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Med_city.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Med_state.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Med_zipCode.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Med_zipCodeExt.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Med_phone.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Med_alternatePhone.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Med_fax.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Specialty.Spe_name.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                // p.Med_office.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                MedicosTemporalTrazabilidad = this.SortByColumnWithOrder(order, orderDir, MedicosTemporalTrazabilidad);

                // Filter record count.
                int recFilter = MedicosTemporalTrazabilidad.Count;

                // Apply pagination.
                MedicosTemporalTrazabilidad = MedicosTemporalTrazabilidad.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = MedicosTemporalTrazabilidad }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private List<PMedical> SortByColumnWithOrder(string order, string orderDir, List<PMedical> data)
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
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_firstName).ToList()
                                                                                                  : data.OrderBy(p => p.Med_firstName).ToList();
                        break;
                    case "2":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_lastName).ToList()
                                                                                                 : data.OrderBy(p => p.Med_lastName).ToList();
                        break;
                    case "3":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_address).ToList()
                                                                                                 : data.OrderBy(p => p.Med_address).ToList();
                        break;
                    case "4":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_city).ToList()
                                                                                                 : data.OrderBy(p => p.Med_city).ToList();
                        break;                        
                    case "5":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_state).ToList()
                                                                                                 : data.OrderBy(p => p.Med_state).ToList();
                        break;
                    case "6":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_zipCode).ToList()
                                                                                                 : data.OrderBy(p => p.Med_zipCode).ToList();
                        break;
                    case "7":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_zipCodeExt).ToList()
                                                                                                 : data.OrderBy(p => p.Med_zipCodeExt).ToList();
                        break;
                    case "8":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_phone).ToList()
                                                                                                 : data.OrderBy(p => p.Med_phone).ToList();
                        break;
                    case "9":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_alternatePhone).ToList()
                                                                                                 : data.OrderBy(p => p.Med_alternatePhone).ToList();
                        break;
                    case "10":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_fax).ToList()
                                                                                                 : data.OrderBy(p => p.Med_fax).ToList();
                        break;
                    case "11":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Specialty.Spe_name).ToList()
                                                                                                 : data.OrderBy(p => p.Specialty.Spe_name).ToList();
                        break;
                    case "12":
                        // Setting.
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Med_office).ToList()
                                                                                                 : data.OrderBy(p => p.Med_office).ToList();
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
        public void ListaMedicosTabla()
        {
            try
            {
                MedicosGlobalEstatica = model.ListarDoctores();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}