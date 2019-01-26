using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using Billing.Web.Controllers;
using Billing.Web.Models;
using System.Diagnostics; 
using System.Data.Objects;
 
namespace Billing.Web.Controllers
{
    public class OptionController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public OptionModel Optionmodel = new OptionModel();



        public PartialViewResult Index()
        {

            return PartialView();
        }

        public JsonResult BuscarMenuPorUsuario(Int32 CodigoUsuario=0)
        {
            List<string> Resultado;

            try
            {
                PUser UsuarioLogueado = new PUser();
                UsuarioLogueado = (PUser)Session["UsuarioLogueado"];                
                
                Resultado = Optionmodel.BuscarMenuPorUsuario(CodigoUsuario);

                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public JsonResult ObtenerOpciones()
        {
            List<SP_LIST_OPTION_Result> Opciones;

            try
            {
                Opciones = Optionmodel.ObtenerOpciones();
                return Json(Opciones, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
