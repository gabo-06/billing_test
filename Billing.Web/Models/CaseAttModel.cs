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
    public class CaseAttModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        // ********************************************************************************

        public List<SP_LIST_CASE_ATTORNEY_Result> ObtenerAbogadosCaso(int CodigoCaso = 0)
        {
            List<SP_LIST_CASE_ATTORNEY_Result> Resultado = new List<SP_LIST_CASE_ATTORNEY_Result>();
            ObjectResult<SP_LIST_CASE_ATTORNEY_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_CASE_ATTORNEY(CodigoCaso);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_CASE_ATTORNEY_Result NodoResultado = new SP_LIST_CASE_ATTORNEY_Result();

                    NodoResultado.Codigo = RPAD.Codigo;
                    NodoResultado.NombreCompleto = RPAD.NombreCompleto;
                    NodoResultado.Direccion = RPAD.Direccion;
                    NodoResultado.Ciudad = RPAD.Ciudad;
                    NodoResultado.Estado = RPAD.Estado;
                    NodoResultado.CodigoPostal = RPAD.CodigoPostal;
                    NodoResultado.ExtensionCodigoPostal = RPAD.ExtensionCodigoPostal;
                    NodoResultado.Telefono = RPAD.Telefono;
                    NodoResultado.Fax = RPAD.Fax;
                    NodoResultado.Especialidad = RPAD.Especialidad;
                    NodoResultado.Asistente = RPAD.Asistente;

                    Resultado.Add(NodoResultado);
                }


                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SP_ASSIGN_ATTORNEY_TO_CASE_Result AsignarAbogadoACaso(int CodigoCaso = 0, int CodigoAbogado = 0)
        {
            SP_ASSIGN_ATTORNEY_TO_CASE_Result Resultado = new SP_ASSIGN_ATTORNEY_TO_CASE_Result();
            ObjectResult<SP_ASSIGN_ATTORNEY_TO_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_ASSIGN_ATTORNEY_TO_CASE(CodigoCaso
                                                                                    , CodigoAbogado);

                Debug.WriteLine(ResultadoProcedimientoAlmacenadoDuro);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    Resultado.Indicator = RPAD.Indicator;
                    Resultado.ErrorMessage = RPAD.ErrorMessage;
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SP_DELETE_ATTORNEY_FROM_CASE_Result EliminarAbogadoDeCaso(int CodigoCaso = 0, int CodigoAbogado = 0)
        {
            SP_DELETE_ATTORNEY_FROM_CASE_Result Resultado = new SP_DELETE_ATTORNEY_FROM_CASE_Result();
            ObjectResult<SP_DELETE_ATTORNEY_FROM_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_DELETE_ATTORNEY_FROM_CASE(CodigoCaso
                                                                                    , CodigoAbogado);
                Debug.WriteLine(ResultadoProcedimientoAlmacenadoDuro);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    Resultado.Indicator = RPAD.Indicator;
                    Resultado.ErrorMessage = RPAD.ErrorMessage;
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}