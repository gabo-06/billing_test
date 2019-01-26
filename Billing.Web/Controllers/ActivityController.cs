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
    public class ActivityController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public ActivityModel model = new ActivityModel();

        static List<PActivity> ActividadGlobalEstatica = new List<PActivity>();
        static List<PActivity> ActividadGlobalEstaticaTrazabilidad = new List<PActivity>();

        static List<PActivity> ActividadTemporal = new List<PActivity>();
        static List<PActivity> ActividadTemporalTrazabilidad = new List<PActivity>();

        public PartialViewResult Index()
        {
            var Actividades = model.Listar();
            return PartialView(Actividades);
        }

        public JsonResult ListaActividades()
        {
            List<PActivity> Actividades;

            try
            {
                Actividades = model.Listar();
                return Json(Actividades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Create(Activity Actividad)
        {
            List<SP_SAVE_ACTIVITY_Result> Resultado;

            try
            {
                // ---d
                Resultado = model.Insertar(Actividad);
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
            Activity ObjActividad = null;

            try
            {
                ObjActividad = model.Buscar(Codigo);
                return Json(ObjActividad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ActualizarDatos")]
        public JsonResult Edit(Activity Actividad)
        {
            List<SP_UPDATE_ACTIVITY_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Actividad);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult GetData()
        {
            JsonResult result = new JsonResult();

            try
            {   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);


                if (ActividadGlobalEstatica.Count == 0)
                {
                    this.ListarActividadTabla();
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || ActividadTemporal.Count == 0)
                {
                    ActividadTemporal = ActividadGlobalEstatica;
                }


                int totalRecords = ActividadGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Buscando
                    ActividadTemporal = ActividadGlobalEstatica.Where(p => // p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||                                                                                
                                                                            // p.Pat_lastName.ToString().ToLower().Contains(search.ToLower()) ||
                                                                           p.Act_description.ToString().ToLower().Contains(search.ToLower()) ||
                                                                           p.ActivityType.Aty_description.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                //Orden
                ActividadTemporal = this.SortByColumnWithOrder(order, orderDir, ActividadTemporal);

                // Cantidad.
                int recFilter = ActividadTemporal.Count;

                // Paginado
                ActividadTemporal = ActividadTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = ActividadTemporal }, JsonRequestBehavior.AllowGet);
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


                if (ActividadGlobalEstaticaTrazabilidad.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListarParaTrazabilidad();

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || ActividadTemporalTrazabilidad.Count == 0)
                {
                    ActividadTemporalTrazabilidad = ActividadGlobalEstaticaTrazabilidad;
                }


                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = ActividadGlobalEstaticaTrazabilidad.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    ActividadTemporalTrazabilidad = ActividadGlobalEstaticaTrazabilidad.Where(p => // p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||                                                                                
                                                                            // p.Pat_lastName.ToString().ToLower().Contains(search.ToLower()) ||
                                                                           p.Act_description.Replace(" ", "").ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           p.ActivityType.Aty_description.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                ActividadTemporalTrazabilidad = this.SortByColumnWithOrder(order, orderDir, ActividadTemporalTrazabilidad);

                // Filter record count.
                int recFilter = ActividadTemporalTrazabilidad.Count;

                // Apply pagination.
                ActividadTemporalTrazabilidad = ActividadTemporalTrazabilidad.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = ActividadTemporalTrazabilidad }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<PActivity> SortByColumnWithOrder(string order, string orderDir, List<PActivity> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                switch (order)
                {
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Act_description).ToList()
                                                                                                 : data.OrderBy(p => p.Act_description).ToList();
                        break;

                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ActivityType.Aty_description).ToList()
                                                                                                 : data.OrderBy(p => p.ActivityType.Aty_description).ToList();
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

        public void ListarParaTrazabilidad()
        {
            try
            {
                ActividadGlobalEstaticaTrazabilidad = model.ListarParaTrazabilidad();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void ListarActividadTabla()
        {
            try
            {
                ActividadGlobalEstatica = model.Listar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        public JsonResult ObtenerPrecioSegunActividad(int CodigoCaso = 0, int CodigoActividad = 0)
        {
            decimal Resultado = 0;
            
            try
            {
                Resultado = decimal.Parse(model.ObtenerPrecioSegunActividad(CodigoCaso, CodigoActividad).ToString());
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }

    }
}
