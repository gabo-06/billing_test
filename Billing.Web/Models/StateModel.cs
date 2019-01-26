using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class StateModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<State> ListaEstados()
        {
            List<State> Lista = new List<State>();

            var Estados = db.SP_LIST_STATE();

            foreach (var LectorEstado in Estados)
            {
                State ObjEstado = new State();

                ObjEstado.Sta_code = LectorEstado.Sta_code;
                ObjEstado.Sta_name = LectorEstado.Sta_name;
                ObjEstado.Sta_abbreviation = LectorEstado.Sta_abbreviation;

                Lista.Add(ObjEstado);
            }

            return Lista;
        }


        public List<SP_LIST_CITY_STATE_Result> ListaEstadosyCiudades()
        {
            List<SP_LIST_CITY_STATE_Result> Lista = new List<SP_LIST_CITY_STATE_Result>();

            ObjectResult<SP_LIST_CITY_STATE_Result> Estados = db.SP_LIST_CITY_STATE();

            try
            {
                foreach (var LectorEstado in Estados)
                {
                    SP_LIST_CITY_STATE_Result ObjEstado = new SP_LIST_CITY_STATE_Result();

                    ObjEstado.code = int.Parse(LectorEstado.code.ToString());
                    ObjEstado.nombre = LectorEstado.nombre;
                    ObjEstado.abreviatura = LectorEstado.abreviatura;
                    ObjEstado.tipo = LectorEstado.tipo;
                    Lista.Add(ObjEstado);
                }

                return Lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}