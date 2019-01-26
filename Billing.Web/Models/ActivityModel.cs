using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models 
{ 
    public class ActivityModel 
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<PActivity> Listar()
        {
            List<PActivity> Lista = new List<PActivity>();

            var Actividades = db.SP_LIST_ACTIVITY();

            foreach (var LectorActividad in Actividades)
            {
                PActivity ObjActividad = new PActivity();
                ActivityType ObjTipoActividad = new ActivityType();

                ObjActividad.Act_code = int.Parse(LectorActividad.Act_code.ToString());
                ObjActividad.Act_description = LectorActividad.Act_description;
                ObjTipoActividad.Aty_description = LectorActividad.Aty_description;
                ObjActividad.ActivityType = ObjTipoActividad;

                Lista.Add(ObjActividad);
            }
            return Lista;
        }

        public List<PActivity> ListarParaTrazabilidad()
        {
            List<PActivity> Lista = new List<PActivity>();

            var Actividades = db.SP_LIST_ACTIVITY();

            foreach (var LectorActividad in Actividades)
            {
                PActivity ObjActividad = new PActivity();
                ActivityType ObjTipoActividad = new ActivityType();

                ObjActividad.Act_code = int.Parse(LectorActividad.Act_code.ToString());
                ObjActividad.Act_description = LectorActividad.Act_description;
                ObjTipoActividad.Aty_description = LectorActividad.Aty_description;
                ObjActividad.ActivityType = ObjTipoActividad;

                Lista.Add(ObjActividad);
            }
            return Lista;
        }

        public List<SP_SAVE_ACTIVITY_Result> Insertar(Activity Actividad)
        {
            List<SP_SAVE_ACTIVITY_Result> Resultado = new List<SP_SAVE_ACTIVITY_Result>();
            ObjectResult<SP_SAVE_ACTIVITY_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_ACTIVITY(   Actividad.Act_description
                                                                            , Actividad.Aty_code
                                                                            , Actividad.Act_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_ACTIVITY_Result NodoResultado = new SP_SAVE_ACTIVITY_Result();

                    NodoResultado.ActivityErrorCode = RPAD.ActivityErrorCode;
                    NodoResultado.ErrorMessage = RPAD.ErrorMessage.ToString();

                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public Activity Buscar(int Codigo)
        {
            Activity ObjActividad = new Activity();

            try
            {
                var Resultado = db.SP_FIND_ACTIVITY(Codigo);

                foreach (var NodoResultado in Resultado)
                {
                    ObjActividad.Act_code = NodoResultado.Act_code;
                    ObjActividad.Act_description = NodoResultado.Act_description;
                    ObjActividad.Aty_code = NodoResultado.Aty_code;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjActividad;
        }

        public List<SP_UPDATE_ACTIVITY_Result> Actualizar(Activity Actividad)
        {
            List<SP_UPDATE_ACTIVITY_Result> Resultado = new List<SP_UPDATE_ACTIVITY_Result>();
            ObjectResult<SP_UPDATE_ACTIVITY_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_ACTIVITY( Actividad.Act_code
                                                                            , Actividad.Act_description
                                                                            , Actividad.Aty_code
                                                                            , Actividad.Act_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_ACTIVITY_Result NodoResultado = new SP_UPDATE_ACTIVITY_Result();

                    NodoResultado.ActivityErrorCode = RPAD.ActivityErrorCode;
                    NodoResultado.ErrorMessage = RPAD.ErrorMessage.ToString();

                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public decimal ObtenerPrecioSegunActividad(int CodigoCaso = 0, int CodigoActividad = 0)
        {
            ObjectResult ResultadoProcedimientoAlmacenadoDuro;
            decimal Precio = 0;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_EXTRA_PRICE_CIS_BY_ACTIVITY(CodigoCaso, CodigoActividad);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    Precio = decimal.Parse(RPAD.ToString());
                }

                return Precio;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}