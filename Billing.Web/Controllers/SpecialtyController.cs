using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;

namespace Billing.Web.Controllers
{
    public class SpecialtyController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public SpecialtyModel model = new SpecialtyModel();


        static List<Specialty> EspecialidadGlobalEstatica = new List<Specialty>();
        static List<Specialty> EspecialidadTemporal = new List<Specialty>();

        public PartialViewResult Index()
        {
            var Especialidades = model.Listar();
            return PartialView(Especialidades);
        }

        [HttpPost]
        public JsonResult Create(Specialty Especialidad)
        {
            List<SP_SAVE_SPECIALTY_Result> Resultado;

            try
            {
                Resultado = model.Insertar(Especialidad);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ObtenerDatosParaEditar")]
        public JsonResult Edit(int Codigo = 0)
        {
            Specialty ObjEspecialidad = null;

            try
            {
                ObjEspecialidad = model.Buscar(Codigo);
                return Json(ObjEspecialidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ActualizarDatos")]
        public JsonResult Edit(Specialty Especialidad)
        {
            List<SP_UPDATE_SPECIALTY_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Especialidad);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListaEspecialidadesDoctor()
        {
            List<Specialty> Especialidades;

            try
            {
                Especialidades = model.ListaEspecialidadesDoctor();
                return Json(Especialidades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListaEspecialidadesAbogado()
        {
            List<Specialty> Especialidades;

            try
            {
                Especialidades = model.ListaEspecialidadesAbogado();
                return Json(Especialidades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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


                if (EspecialidadGlobalEstatica.Count == 0)
                {
                    this.ListarEspecialidadTabla();
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || EspecialidadTemporal.Count == 0)
                {
                    EspecialidadTemporal = EspecialidadGlobalEstatica;
                }


                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = EspecialidadGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    EspecialidadTemporal = EspecialidadGlobalEstatica.Where(p => // p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||                                                                                
                        // p.Pat_lastName.ToString().ToLower().Contains(search.ToLower()) ||
                                                                           p.Spe_name.Replace(" ", "").ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           p.Spe_type.ToString().ToLower().Contains(search.ToLower()) 
                                                                           ).ToList();
                }

                // Sorting.
                EspecialidadTemporal = this.SortByColumnWithOrder(order, orderDir, EspecialidadTemporal);

                // Filter record count.
                int recFilter = EspecialidadTemporal.Count;

                // Apply pagination.
                EspecialidadTemporal = EspecialidadTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = EspecialidadTemporal }, JsonRequestBehavior.AllowGet);
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


        private List<Specialty> SortByColumnWithOrder(string order, string orderDir, List<Specialty> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                switch (order)
                {
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Spe_name).ToList()
                                                                                                 : data.OrderBy(p => p.Spe_name).ToList();
                        break;

                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Spe_type).ToList()
                                                                                                 : data.OrderBy(p => p.Spe_type).ToList();
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


        public void ListarEspecialidadTabla()
        {
            try
            {
                EspecialidadGlobalEstatica = model.Listar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
