using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class CityModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<City> ListaCiudades()
        {
            List<City> Lista = new List<City>();
            ObjectResult<string> Ciudades;

            Ciudades = db.SP_LIST_CITY();

            foreach (var LectorCiudad in Ciudades)
            {
                City ObjCiudad = new City();

                // ObjCiudad.Cit_code = LectorCiudad.Cit_code;
                ObjCiudad.Cit_name = LectorCiudad;

                Lista.Add(ObjCiudad);
            }

            return Lista;
        }
    }
}