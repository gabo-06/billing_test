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
    public class ActivityTypeController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public ActivityTypeModel model = new ActivityTypeModel();

        public JsonResult ListaTiposActividades()
        {
            List<ActivityType> TiposActividades;

            try
            {
                TiposActividades = model.Listar();
                return Json(TiposActividades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
