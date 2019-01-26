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
    public class AttorneyController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public AttorneyModel model = new AttorneyModel();

        static List<PAttorney> AbogadoGlobalEstatica = new List<PAttorney>();
        static List<PAttorney> AbogadoGlobalEstaticaTrazabilidad = new List<PAttorney>();

        static List<PAttorney> AbogadoTemporal = new List<PAttorney>();
        static List<PAttorney> AbogadoTemporalTrazabilidad = new List<PAttorney>();
        
        public ActionResult Index()
        {
            var Abogados = model.Listar();
            return PartialView(Abogados);            

        }

        public JsonResult ListarParaAutocompletar()
        {
            List<SP_LIST_ATTORNEY_FOR_FIND_MATCHES_Result> Abogados;

            try
            {
                Abogados = model.ListarParaAutocompletar();
                return Json(Abogados, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Create(Attorney Abogado)
        {
            List<SP_SAVE_ATTORNEY_Result> Resultado;

            try
            {
                Resultado = model.Insertar(Abogado);
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
            PAttorney ObjAbogado = null;

            try
            {
                ObjAbogado = model.Buscar(Codigo);
                return Json(ObjAbogado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("ActualizarDatos")]
        public JsonResult Edit(Attorney Abogado)
        {
            List<SP_UPDATE_ATTORNEY_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Abogado);
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
            List<SP_DELETE_ATTORNEY_Result> Resultado;

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
        public JsonResult BuscarHomonimos(Attorney Abogado)
        {
            List<SP_FIND_HOMONYM_ATTORNEYS_Result> Datos;

            try
            {
                Datos = model.BuscarHomonimos(Abogado);

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


                if (AbogadoGlobalEstatica.Count == 0)
                {
                    this.ListarAbogadoTabla();
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" ||AbogadoTemporal.Count == 0)
                {
                    AbogadoTemporal = AbogadoGlobalEstatica;
                }


                int totalRecords = AbogadoGlobalEstatica.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Buscando
                    AbogadoTemporal = AbogadoGlobalEstatica.Where(p => ( // p.Att_firstName.Replace(" ", "").ToString().Trim() + p.Att_lastName.Replace(" ", "").ToString().Trim()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                         // p.Att_firstName.Replace(" ", "").ToString().ToLower().Trim().Contains(search.Replace(" ", "").ToLower()) ||
                                                                         p.Att_lastName.Replace(" ", "").ToString()).ToLower().Trim().Contains(search.Replace(" ", "").ToLower())).ToList();
                                                                         // p.Att_address.Replace(" ", "").ToString().ToLower().Trim().Contains(search.ToLower()) ||
                                                                         // p.Att_city.Replace(" ", "").ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                         // p.Att_state.Replace(" ", "").ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                         // p.Att_zipCode.ToString().ToLower().Contains(search.Replace(" ", "").ToLower())||
                                                                         // p.Att_zipCodeExt.ToString().ToLower().Contains(search.Replace(" ", "").ToLower())||
                                                                         // p.Att_fax.ToString().ToLower().Contains(search.Replace(" ", "").ToLower())||
                                                                         // p.Att_assistant.ToString().ToLower().Contains(search.Replace(" ", "").ToLower())||
                                                                         // p.Specialty.Spe_name.Replace(" ", "").ToString().ToLower().Contains(search.Replace(" ", "").ToLower())                                                                           
                }

                //Orden
                AbogadoTemporal = this.SortByColumnWithOrder(order, orderDir, AbogadoTemporal);

                // Cantidad.
                int recFilter = AbogadoTemporal.Count;

                // Paginado
                AbogadoTemporal = AbogadoTemporal.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = AbogadoTemporal }, JsonRequestBehavior.AllowGet);
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


                if (AbogadoGlobalEstaticaTrazabilidad.Count == 0)
                {
                    // Loading.
                    // data = this.LoadData();

                    // Carga la variable global estática con los pacientes de la base.
                    this.ListarParaTrazabilidad();

                    // Lista de pacientes temporal para manipular la búsqueda.
                }

                // Clona la data en la variable temporal que se va a utilizar para manipular las búsquedas.
                if (search.Trim() == "" || AbogadoTemporalTrazabilidad.Count == 0)
                {
                    AbogadoTemporalTrazabilidad = AbogadoGlobalEstaticaTrazabilidad;
                }


                // Total record count.
                // int totalRecords = lst.Count;
                int totalRecords = AbogadoGlobalEstaticaTrazabilidad.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search)
                 && !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    AbogadoTemporalTrazabilidad = AbogadoGlobalEstaticaTrazabilidad.Where(p => // p.Pat_firstName.ToString().ToLower().Contains(search.ToLower()) ||       
                                                                                         (p.Att_firstName.ToString().Trim() + p.Att_lastName.ToString().Trim()).ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Att_firstName.ToString().ToLower().Trim().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Att_lastName.ToString().ToLower().Trim().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Att_address.ToString().ToLower().Trim().Contains(search.ToLower()) ||
                                                                                          p.Att_city.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Att_state.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Att_zipCode.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Att_zipCodeExt.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Att_fax.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Att_assistant.ToString().ToLower().Contains(search.Replace(" ", "").ToLower()) ||
                                                                                          p.Specialty.Spe_name.ToString().ToLower().Contains(search.Replace(" ", "").ToLower())
                                                                                          ).ToList();
                }

                // Sorting.
                AbogadoTemporalTrazabilidad = this.SortByColumnWithOrder(order, orderDir, AbogadoTemporalTrazabilidad);

                // Filter record count.
                int recFilter = AbogadoTemporalTrazabilidad.Count;

                // Apply pagination.
                AbogadoTemporalTrazabilidad = AbogadoTemporalTrazabilidad.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = AbogadoTemporalTrazabilidad }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<PAttorney> SortByColumnWithOrder(string order, string orderDir, List<PAttorney> data)
        {
            // Initialization.
            // List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                switch (order)
                {
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_firstName).ToList()
                                                                                                 : data.OrderBy(p => p.Att_firstName).ToList();
                        break;

                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_lastName).ToList()
                                                                                                 : data.OrderBy(p => p.Att_lastName).ToList();
                        break;
                    case "3":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_address).ToList()
                                                                                                 : data.OrderBy(p => p.Att_address).ToList();
                        break;
                    case "4":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_city).ToList()
                                                                                                 : data.OrderBy(p => p.Att_city).ToList();
                        break;
                    case "5":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_state).ToList()
                                                                                                 : data.OrderBy(p => p.Att_state).ToList();
                        break;
                    case "6":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_zipCode).ToList()
                                                                                                 : data.OrderBy(p => p.Att_zipCode).ToList();
                        break;
                    case "7":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_zipCodeExt).ToList()
                                                                                                 : data.OrderBy(p => p.Att_zipCodeExt).ToList();
                        break;
                    case "8":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_phone).ToList()
                                                                                                 : data.OrderBy(p => p.Att_phone).ToList();
                        break;
                    case "9":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_fax).ToList()
                                                                                                 : data.OrderBy(p => p.Att_fax).ToList();
                        break;
                    case "10":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Att_assistant).ToList()
                                                                                                 : data.OrderBy(p => p.Att_assistant).ToList();
                        break;
                    case "11":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Specialty.Spe_name).ToList()
                                                                                                 : data.OrderBy(p => p.Specialty.Spe_name).ToList();
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
                AbogadoGlobalEstaticaTrazabilidad = model.ListarParaTrazabilidad();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void ListarAbogadoTabla()
        {
            try
            {
                AbogadoGlobalEstatica = model.Listar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
