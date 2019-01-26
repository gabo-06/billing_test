using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;


using System.Web.UI;
using Billing.Web.Controllers;
using Billing.Web.Models;
using System.Diagnostics;
using System.Data.Objects;
using System.IO;
using System.Collections;

namespace Billing.Web.Controllers
{
    public class MasterViewerController : Controller
    {


        private OmnimedBDEntities db = new OmnimedBDEntities();
        public CaseModel model = new CaseModel();
        public StateModel model2 = new StateModel();
        public SpecialtyModel model3 = new SpecialtyModel();


        //
        // GET: /MasterViewer/
        public ActionResult Index()
        {
            MembershipUser user = Membership.GetUser(true);
            if (user == null)
            {
                return RedirectToAction("login", "Account");
            }

            AccountController Account = new AccountController();
            int data = Account.LeerPermisoUsuarioLogueado();
            if (data == 0)
                ViewBag.tipoPermiso = "digit";
            else
                ViewBag.tipoPermiso = "Administrator";



            //obtener_especialidades_estados

            Session["especialidades_estados"] = obtener_especialidades_estados();         


            return View();

        }




        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Server)]
        public JsonResult obtener_especialidades_estados()
        {

            
            List<Specialty> Lista = new List<Specialty>();
            List<SP_LIST_CITY_STATE_Result> listalugares = new List<SP_LIST_CITY_STATE_Result>();
            try
            {
                Lista = model3.ListaEspecialidades();
                List<Specialty> especialidadMedicals = new List<Specialty>();
                List<Specialty> especialidadAttorneys = new List<Specialty>();

                especialidadMedicals = Lista.Where(p => p.Spe_type.Equals("Medical")).ToList();
                especialidadAttorneys = Lista.Where(p => p.Spe_type.Equals("Attorney")).ToList();

                listalugares = model2.ListaEstadosyCiudades();

                List<SP_LIST_CITY_STATE_Result> estados = new List<SP_LIST_CITY_STATE_Result>();
                List<SP_LIST_CITY_STATE_Result> ciudades = new List<SP_LIST_CITY_STATE_Result>();

                estados = listalugares.Where(p => p.tipo.Equals("E")).ToList();
                ciudades = listalugares.Where(p => p.tipo.Equals("C")).ToList();

                var jsonResult = Json(new
                {
                    response = new
                    {
                        especialidadMedicals = especialidadMedicals,
                        especialidadAttorneys = especialidadAttorneys,
                        estados = estados,
                        ciudades = ciudades
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

    }
}
