using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;

namespace Billing.Web.Controllers
{
    public class UserController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities(); 
        public UserModel model = new UserModel();

        /*
        public JsonResult ObtenUsuario()
        {
            string Usuario;

            try
            {
                Usuario = model.ObtenerUsuario();
                return Json(Usuario, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        public JsonResult ListarSupervisoresParaAutocompletar()
        {
            List<SP_LIST_SUPERVISOR_FOR_FIND_MATCHES_Result> Supervisores;

            try
            {
                Supervisores = model.ListarSupervisoresParaAutocompletar();
                return Json(Supervisores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

           ////////////////////////////////////////

        public JsonResult ListarSupervisoresParaReporte()
        {
            List<SP_LIST_SUPERVISOR_FOR_FIND_MATCHES_Result> Supervisores;

            try
            {
                Supervisores = model.ListarSupervisoresParaAutocompletar();
                return Json(Supervisores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

    }
}
