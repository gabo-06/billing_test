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
    public class CaseMedModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        // ********************************************************************************

        public List<SP_LIST_CASE_MEDICAL_Result> ObtenerDoctoresCaso(int CodigoCaso = 0)
        {
            List<SP_LIST_CASE_MEDICAL_Result> Resultado = new List<SP_LIST_CASE_MEDICAL_Result>();
            ObjectResult<SP_LIST_CASE_MEDICAL_Result> ResultadoProcedimientoAlmacenadoDuro;
            
            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_CASE_MEDICAL(CodigoCaso);
                var NombresConcatenados = "";
                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                { 
                    SP_LIST_CASE_MEDICAL_Result NodoResultado = new SP_LIST_CASE_MEDICAL_Result();

                    NodoResultado.Codigo = RPAD.Codigo;
                    NodoResultado.NombreCompleto = RPAD.NombreCompleto;
                    NodoResultado.Direccion = RPAD.Direccion; 
                    NodoResultado.Ciudad = RPAD.Ciudad;
                    NodoResultado.Estado = RPAD.Estado;
                    NodoResultado.CodigoPostal = RPAD.CodigoPostal;
                    NodoResultado.ExtensionCodigoPostal = RPAD.ExtensionCodigoPostal;
                    NodoResultado.Telefono = RPAD.Telefono;
                    NodoResultado.ExtensionTelefono = RPAD.ExtensionTelefono;
                    NodoResultado.TelefonoAlternativo = RPAD.TelefonoAlternativo;
                    NodoResultado.Fax = RPAD.Fax;
                    NodoResultado.Especialidad = RPAD.Especialidad;
                    NodoResultado.Oficina = RPAD.Oficina;

                    NombresConcatenados = NombresConcatenados + NodoResultado.NombreCompleto + "\n";
                                        
                    Debug.WriteLine(NombresConcatenados );

                    Resultado.Add(NodoResultado);
                }


                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SP_ASSIGN_MEDICAL_TO_CASE_Result AsignarDoctorACaso(int CodigoCaso = 0, int CodigoDoctor = 0, string ProcedenciaDoctor = "", int CodigoUsuario = 0)
        {
            SP_ASSIGN_MEDICAL_TO_CASE_Result Resultado = new SP_ASSIGN_MEDICAL_TO_CASE_Result();
            ObjectResult<SP_ASSIGN_MEDICAL_TO_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_ASSIGN_MEDICAL_TO_CASE(CodigoCaso
                                                                                    , CodigoDoctor
                                                                                    , ProcedenciaDoctor
                                                                                    , CodigoUsuario);

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

        public SP_DELETE_MEDICAL_FROM_CASE_Result EliminarDoctorDeCaso(int CodigoCaso = 0, int CodigoDoctor = 0)
        {
            SP_DELETE_MEDICAL_FROM_CASE_Result Resultado = new SP_DELETE_MEDICAL_FROM_CASE_Result();
            ObjectResult<SP_DELETE_MEDICAL_FROM_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_DELETE_MEDICAL_FROM_CASE(CodigoCaso
                                                                                    , CodigoDoctor);

                Debug.WriteLine(ResultadoProcedimientoAlmacenadoDuro);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    // Cuando el indicador es 0, el doctor se ha asigando correctamente y 
                    // cuando es 1 es que no se puede eliminar el doctor por que está relacionado a un evento.
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