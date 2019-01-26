using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;

namespace Billing.Web.Controllers
{
    public class CityController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public CityModel model = new CityModel();
        
        [HttpPost]
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public JsonResult ListaCiudades()
        {
            List<City> Ciudades;

            try
            {
                Ciudades = model.ListaCiudades();
                return Json(Ciudades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
