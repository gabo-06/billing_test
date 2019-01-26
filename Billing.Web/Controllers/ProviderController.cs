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
    public class ProviderController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public ProviderModel model = new ProviderModel();

        static List<Provider> ProveedorGlobalEstatica = new List<Provider>();
        static List<Provider> ProveedorGlobalEstaticaTrazabilidad = new List<Provider>();

        static List<Provider> ProveedorTemporal = new List<Provider>();
        static List<Provider> ProveedorTemporalTrazabilidad = new List<Provider>();

        public PartialViewResult Index()
        {
            var Proveedores = model.Listar();
            return PartialView(Proveedores);
        }

        public JsonResult ListarParaAutocompletar()
        {
            List<SP_LIST_PROVIDER_FOR_FIND_MATCHES_Result> Proveedores;

            try
            {
                Proveedores = model.ListarParaAutocompletar();
                return Json(Proveedores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult Create(Provider Proveedor)
        {
            List<SP_SAVE_PROVIDER_Result> Resultado;

            try
            {
                Resultado = model.Insertar(Proveedor);
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
            Provider ObjProveedor = null;

            try
            {
                ObjProveedor = model.Buscar(Codigo);
                return Json(ObjProveedor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ActualizarDatos")]
        public JsonResult Edit(Provider Proveedor)
        {
            List<SP_UPDATE_PROVIDER_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Proveedor);
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
            List<SP_DELETE_PROVIDER_Result> Resultado;

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
        public JsonResult BuscarHomonimos(Provider Proveedor)
        {
            List<SP_FIND_HOMONYM_PROVIDERS_Result> Datos;

            try
            {
                Datos = model.BuscarHomonimos(Proveedor);

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


                if (ProveedorGlobalEstatica.Count == 0)
                {
                    this.ListarProveedorTabla();
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || ProveedorTemporal.Count == 0)
                {
                    ProveedorTemporal = ProveedorGlobalEstatica;
                }


                int totalRecords = ProveedorGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Buscando
                    ProveedorTemporal = ProveedorGlobalEstatica.Where(p => ( // p.Pro_firstName.Replace(" ", "").ToString().ToLower().Trim() + p.Pro_lastName.Replace(" ", "").ToString().ToLower().Trim()).Contains(search.Replace(" ", "").ToLower()) ||
                                                                            // p.Pro_firstName.Replace(" ", "").ToString().ToLower().Trim().Contains(search.Replace(" ", "").ToLower()) ||
                                                                            p.Pro_lastName.Replace(" ", "").ToString()).ToLower().Trim().Contains(search.Replace(" ", "").ToLower())).ToList();
                                                                            // p.Pro_number.ToString().ToLower().Trim().Contains(search.ToLower())).ToList();
                }

                //Orden
                ProveedorTemporal = this.SortByColumnWithOrder(order, orderDir, ProveedorTemporal);

                // Cantidad.
                int recFilter = ProveedorTemporal.Count;

                // Paginado
                ProveedorTemporal = ProveedorTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = ProveedorTemporal }, JsonRequestBehavior.AllowGet);
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


                if (ProveedorGlobalEstaticaTrazabilidad.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListarParaTrazabilidad();

                    // Lista de proveedores temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || ProveedorTemporalTrazabilidad.Count == 0)
                {
                    ProveedorTemporalTrazabilidad = ProveedorGlobalEstaticaTrazabilidad;
                }


                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = ProveedorGlobalEstaticaTrazabilidad.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    ProveedorTemporalTrazabilidad = ProveedorGlobalEstaticaTrazabilidad.Where(p => // p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||        
                                                                         (p.Pro_firstName.ToString().ToLower().Trim() + p.Pro_lastName.ToString().ToLower().Trim()).Contains(search.Replace(" ", "").ToLower()) ||
                                                                           p.Pro_firstName.ToString().ToLower().Trim().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           p.Pro_lastName.ToString().ToLower().Trim().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           p.Pro_number.ToString().ToLower().Trim().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                ProveedorTemporalTrazabilidad = this.SortByColumnWithOrder(order, orderDir, ProveedorTemporalTrazabilidad);

                // Filter record count.
                int recFilter = ProveedorTemporalTrazabilidad.Count;

                // Apply pagination.
                ProveedorTemporalTrazabilidad = ProveedorTemporalTrazabilidad.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = ProveedorTemporalTrazabilidad }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Provider> SortByColumnWithOrder(string order, string orderDir, List<Provider> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                switch (order)
                {
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pro_firstName).ToList()
                                                                                                 : data.OrderBy(p => p.Pro_firstName).ToList();
                        break;

                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pro_lastName).ToList()
                                                                                                 : data.OrderBy(p => p.Pro_lastName).ToList();
                        break;
                    case "3":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Pro_number).ToList()
                                                                                                 : data.OrderBy(p => p.Pro_number).ToList();
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
                ProveedorGlobalEstaticaTrazabilidad = model.ListarParaTrazabilidad();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ListarProveedorTabla()
        {
            try
            {
                ProveedorGlobalEstatica = model.Listar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }








    }
}
