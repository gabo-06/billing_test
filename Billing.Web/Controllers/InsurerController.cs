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
    public class InsurerController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        private InsurerModel model = new InsurerModel();

        static List<Insurer> InsurerGlobalEstatica = new List<Insurer>();
        static List<Insurer> InsurerGlobalEstaticaTrazabilidad = new List<Insurer>();

        static List<Insurer> InsurerTemporal = new List<Insurer>();
        static List<Insurer> InsurerTemporalTrazabilidad = new List<Insurer>();

        public PartialViewResult Index()
        {
            var Aseguradoras = model.ListaAseguradoras();
            return PartialView(Aseguradoras);
        }

        public JsonResult ListarParaAutocompletar()
        {
            List<SP_LIST_INSURER_FOR_FIND_MATCHES_Result> Aseguradoras;

            try
            {
                Aseguradoras = model.ListarParaAutocompletar();
                return Json(Aseguradoras, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListarParaPago()
        {
            List<SP_LIST_INSURER_FOR_PAYMENT_Result> Aseguradoras;

            try
            {
                Aseguradoras = model.ListarParaPago();
                return Json(Aseguradoras, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListarParaEliminarPago()
        {
            List<SP_LIST_INSURER_FOR_REMOVE_PAYMENT_Result> Aseguradoras;

            try
            {
                Aseguradoras = model.ListarParaEliminarPago();
                return Json(Aseguradoras, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Create(Insurer Aseguradora)
        {
            List<SP_SAVE_INSURER_Result> Resultado;

            try
            {
                Resultado = model.Insertar(Aseguradora);
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
            PInsurer ObjAseguradora = null;

            try
            {
                ObjAseguradora = model.Buscar(Codigo);
                return Json(ObjAseguradora, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ActualizarDatos")]
        public JsonResult Edit(Insurer Aseguradora)
        {
            List<SP_UPDATE_INSURER_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Aseguradora);
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
            List<SP_DELETE_INSURER_Result> Resultado;

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
        public JsonResult BuscarHomonimos(Insurer Aseguradora)
        {
            List<SP_FIND_HOMONYM_INSURERS_Result> Datos;

            try
            {
                Datos = model.BuscarHomonimos(Aseguradora);

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


                if (InsurerGlobalEstatica.Count == 0)
                {
                    this.ListarInsurerTabla();
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || InsurerTemporal.Count == 0)
                {
                    InsurerTemporal = InsurerGlobalEstatica;
                }


                int totalRecords = InsurerGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    // Buscando
                    InsurerTemporal = InsurerGlobalEstatica.Where(p => // p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||                                                                                                        
                                                                           p.Ins_name.Replace(" ", "").ToString().ToLower().Contains(search.Replace(" ", "").ToLower())).ToList();
                                                                           //p.Ins_name.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           // p.Ins_address.Replace(" ", "").ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           //p.Ins_address.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           // p.Ins_city.ToString().ToLower().Contains(search.ToLower()) ||
                                                                           // p.Ins_state.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) || 
                                                                           // p.Ins_phone.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) || 
                                                                           // p.Ins_phoneExt.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) || 
                                                                           // p.Ins_fax.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) || 
                                                                           // p.Ins_zipCode.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) || 
                                                                           // p.Ins_zipExt.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) || 
                                                                           // p.Ins_scTpaCode.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           // p.Ins_feinSc.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           // p.Ins_carrierCode.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                           // p.Ins_feinCc.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) 
                                                                           
                }

                //Orden
                InsurerTemporal = this.SortByColumnWithOrder(order, orderDir, InsurerTemporal);

                // Cantidad.
                int recFilter = InsurerTemporal.Count;

                // Paginado
                InsurerTemporal = InsurerTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = InsurerTemporal }, JsonRequestBehavior.AllowGet);
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
                InsurerGlobalEstaticaTrazabilidad = model.ListarParaTrazabilidad();
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


                if (InsurerGlobalEstaticaTrazabilidad.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListarParaTrazabilidad();

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || InsurerTemporalTrazabilidad.Count == 0)
                {
                    InsurerTemporalTrazabilidad = InsurerGlobalEstaticaTrazabilidad;
                }


                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = InsurerGlobalEstaticaTrazabilidad.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    InsurerTemporalTrazabilidad = InsurerGlobalEstaticaTrazabilidad.Where(p => // p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||                                                                                                        
                                                                                          p.Ins_name.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_address.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_city.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                          p.Ins_state.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_phone.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_phoneExt.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_fax.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_zipCode.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_zipExt.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_scTpaCode.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_feinSc.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_carrierCode.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Ins_feinCc.ToString().ToLower().Contains(search.Replace(" ", "").ToLower())
                                                                                          ).ToList();
                }

                // Sorting.
                InsurerTemporalTrazabilidad = this.SortByColumnWithOrder(order, orderDir, InsurerTemporalTrazabilidad);

                // Filter record count.
                int recFilter = InsurerTemporalTrazabilidad.Count;

                // Apply pagination.
                InsurerTemporalTrazabilidad = InsurerTemporalTrazabilidad.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = InsurerTemporalTrazabilidad }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Insurer> SortByColumnWithOrder(string order, string orderDir, List<Insurer> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                switch (order)
                {
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_name).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_name).ToList();
                        break;

                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_address).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_address).ToList();
                        break;
                    case "3":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_city).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_city).ToList();
                        break;
                    case "4":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_state).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_state).ToList();
                        break;
                    case "5":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_phone).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_phone).ToList();
                        break;
                    case "6":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_phoneExt).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_phoneExt).ToList();
                        break;
                    case "7":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_fax).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_fax).ToList();
                        break;
                    case "8":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_zipCode).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_zipCode).ToList();
                        break;
                    case "9":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_zipExt).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_zipExt).ToList();
                        break;
                    case "10":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_scTpaCode).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_scTpaCode).ToList();
                        break;
                    case "11":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_feinSc).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_feinSc).ToList();
                        break;
                    case "12":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_carrierCode).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_carrierCode).ToList();
                        break;
                    case "13":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ins_feinCc).ToList()
                                                                                                 : data.OrderBy(p => p.Ins_feinCc).ToList();
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


        public void ListarInsurerTabla()
        {
            try
            {
                InsurerGlobalEstatica = model.ListaAseguradoras();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
