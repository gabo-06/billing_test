using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;

namespace Billing.Web.Controllers
{
    public class StateController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public StateModel model = new StateModel();

        public JsonResult ListaEstados()
        {
            List<State> Estados;

            try
            {
                Estados = model.ListaEstados();
                return Json(Estados, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
