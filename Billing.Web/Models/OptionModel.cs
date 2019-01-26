using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class OptionModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<string> BuscarMenuPorUsuario(Int32 use_code = 0)
        {
            List<string> Resultado = new List<string>();

            ObjectResult<string> ResultadoProcedimientoAlmacenadoDuro;
            List<string> OpcionesMenu;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_MENU_OPTIONS(use_code, "B");
                OpcionesMenu = ResultadoProcedimientoAlmacenadoDuro.ToList();

                return OpcionesMenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> BuscarEntradasPorPaciente(string ApelliidoPaciente = "")
        {
            List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> Resultado = new List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>();
            ObjectResult<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_SEARCH_DATAENTRY_PATIENT(ApelliidoPaciente);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result NodoResultado = new SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result();

                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Pat_firstName = RPAD.Pat_firstName;
                    NodoResultado.Pat_lastName = RPAD.Pat_lastName;
                    NodoResultado.Pat_socialSecurityNumber = RPAD.Pat_socialSecurityNumber;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }


        // Procedimiento que trae los datos de la nueva tabla "Option"
        public List<SP_LIST_OPTION_Result> ObtenerOpciones()
        {
            List<SP_LIST_OPTION_Result> Lista = new List<SP_LIST_OPTION_Result>();
            ObjectResult<SP_LIST_OPTION_Result> Opciones;

            Opciones = db.SP_LIST_OPTION();

            foreach (var NodoResultado in Opciones)
            {
                SP_LIST_OPTION_Result Opcion = new SP_LIST_OPTION_Result();

                Opcion.Opt_code = NodoResultado.Opt_code;
                Opcion.Opt_name = NodoResultado.Opt_name;

                Lista.Add(Opcion);
            }

            return Lista;
        }
    }
}


