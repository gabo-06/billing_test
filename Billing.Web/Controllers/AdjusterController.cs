using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;

namespace Billing.Web.Controllers
{
    public class AdjusterController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        private AdjusterModel model = new AdjusterModel();

        static List<Adjuster> AjustadorGlobalEstatica = new List<Adjuster>();
        static List<Adjuster> AjustadorGlobalEstaticaTrazabilidad = new List<Adjuster>();

        static List<Adjuster> AjustadorTemporal = new List<Adjuster>();
        static List<Adjuster> AjustadorTemporalTrazabilidad = new List<Adjuster>();

        public PartialViewResult Index()
        {
            var Ajustadores = model.Listar();
            return PartialView(Ajustadores);
        }

        public JsonResult ListarParaAutocompletar()
        {
            List<SP_LIST_ADJUSTER_FOR_FIND_MATCHES_Result> Ajustadores;

            try
            {
                Ajustadores = model.ListarParaAutocompletar();
                return Json(Ajustadores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult Create(Adjuster Ajustador)
        {
            List<SP_SAVE_ADJUSTER_Result> Resultado;

            try
            {
                Resultado = model.Insertar(Ajustador);
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
            Adjuster ObjAjustador = null;

            try
            {
                ObjAjustador = model.Buscar(Codigo);
                return Json(ObjAjustador, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ActualizarDatos")]
        public JsonResult Edit(Adjuster Ajustador)
        {
            List<SP_UPDATE_ADJUSTER_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Ajustador);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Delete(int Codigo = 0)
        {
            List<SP_DELETE_ADJUSTER_Result> Resultado;

            try
            {
                Resultado = model.Eliminar(Codigo);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult BuscarHomonimos(Adjuster Ajustador)
        {
            List<SP_FIND_HOMONYM_ADJUSTERS_Result> Datos;

            try
            {
                Datos = model.BuscarHomonimos(Ajustador);

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


                if (AjustadorGlobalEstatica.Count == 0)
                {
                    this.ListarAjustadorTabla();
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || AjustadorTemporal.Count == 0)
                {
                    AjustadorTemporal = AjustadorGlobalEstatica;
                }


                int totalRecords = AjustadorGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Buscando
                    AjustadorTemporal = AjustadorGlobalEstatica.Where(p =>(// p.Adj_firstName.Replace(" ", "").ToString().Trim() + p.Adj_lastName.Replace(" ", "").ToString().Trim()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           // p.Adj_firstName.Replace(" ", "").ToString().ToLower().Trim().Contains(search.ToLower()) ||
                                                                           p.Adj_lastName.Replace(" ", "").ToString()).ToLower().Trim().Contains(search.Replace(" ", "").ToLower())).ToList();
                                                                           // p.Adj_phone.ToString().ToLower().Trim().Contains(search.ToLower()) ||
                                                                           // p.Adj_phoneExt.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) 
                                                                           
                }

                //Orden
                AjustadorTemporal = this.SortByColumnWithOrder(order, orderDir, AjustadorTemporal);

                // Cantidad.
                int recFilter = AjustadorTemporal.Count;

                // Paginado
                AjustadorTemporal = AjustadorTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = AjustadorTemporal }, JsonRequestBehavior.AllowGet);
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


                if (AjustadorGlobalEstaticaTrazabilidad.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListarParaTrazabilidad();

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || AjustadorTemporalTrazabilidad.Count == 0)
                {
                    AjustadorTemporalTrazabilidad = AjustadorGlobalEstaticaTrazabilidad;
                }

                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = AjustadorGlobalEstaticaTrazabilidad.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    AjustadorTemporalTrazabilidad = AjustadorGlobalEstaticaTrazabilidad.Where(p => // p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||   
                                                                                             (p.Adj_firstName.Replace(" ", "").ToString().Trim() + p.Adj_lastName.Replace(" ", "").ToString().Trim()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                             p.Adj_firstName.Replace(" ", "").ToString().ToLower().Trim().Contains(search.ToLower()) ||
                                                                                             p.Adj_lastName.Replace(" ", "").ToString().ToLower().Trim().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                             p.Adj_phone.ToString().ToLower().Trim().Contains(search.ToLower()) ||
                                                                                             p.Adj_phoneExt.ToString().ToLower().Contains(search.Replace(" ", "").ToLower())).ToList();
                }

                // Sorting.
                AjustadorTemporalTrazabilidad = this.SortByColumnWithOrder(order, orderDir, AjustadorTemporalTrazabilidad);

                // Filter record count.
                int recFilter = AjustadorTemporalTrazabilidad.Count;

                // Apply pagination.
                AjustadorTemporalTrazabilidad = AjustadorTemporalTrazabilidad.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = AjustadorTemporalTrazabilidad }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Adjuster> SortByColumnWithOrder(string order, string orderDir, List<Adjuster> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                switch (order)
                {
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adj_firstName).ToList()
                                                                                                 : data.OrderBy(p => p.Adj_firstName).ToList();
                        break;

                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adj_lastName).ToList()
                                                                                                 : data.OrderBy(p => p.Adj_lastName).ToList();
                        break;
                    case "3":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adj_phone).ToList()
                                                                                                 : data.OrderBy(p => p.Adj_phone).ToList();
                        break;
                    case "4":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Adj_phoneExt).ToList()
                                                                                                 : data.OrderBy(p => p.Adj_phoneExt).ToList();
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
                AjustadorGlobalEstaticaTrazabilidad = model.ListarParaTrazabilidad();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ListarAjustadorTabla()
        {
            try
            {
                AjustadorGlobalEstatica = model.Listar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
