using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class ActivityTypeModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<ActivityType> Listar()
        {
            List<ActivityType> Lista = new List<ActivityType>();

            var TiposActividades = db.SP_LIST_ACTIVITYTYPE();

            foreach (var LectorTipoActividad in TiposActividades)
            {
                ActivityType ObjTipoActividad = new ActivityType();

                ObjTipoActividad.Aty_code = LectorTipoActividad.Aty_code;
                ObjTipoActividad.Aty_description = LectorTipoActividad.Aty_description;

                Lista.Add(ObjTipoActividad);
            }

            return Lista;
        }
    }
}