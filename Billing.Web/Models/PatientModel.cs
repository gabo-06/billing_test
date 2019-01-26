using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;
 
namespace Billing.Web.Models
{  
    public class PatientModel 
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<PPatient> Listar()
        {
            List<PPatient> Lista = new List<PPatient>();
            DateTime Pat_birthday;

            var Pacientes = db.SP_LIST_PATIENT();

            foreach (var LectorPaciente in Pacientes)
            {
                PPatient ObjPaciente = new PPatient();
                 
                ObjPaciente.Pat_code = LectorPaciente.Pat_code;
                ObjPaciente.Pat_firstName = LectorPaciente.Pat_firstName;
                ObjPaciente.Pat_lastName = (LectorPaciente.Pat_lastName == null) ? "" : LectorPaciente.Pat_lastName;
                Pat_birthday =  Convert.ToDateTime(LectorPaciente.Pat_birthday);
                ObjPaciente.Pat_birthday = Pat_birthday.ToString("MM/dd/yyyy");
                ObjPaciente.Pat_socialSecurityNumberD =(LectorPaciente.Pat_socialSecurityNumberD ==null )? "": LectorPaciente.Pat_socialSecurityNumberD ;
                ObjPaciente.Pat_address = (LectorPaciente.Pat_address == null) ? "" : LectorPaciente.Pat_address;
                ObjPaciente.Pat_state = (LectorPaciente.Pat_state == null) ? "" : LectorPaciente.Pat_state;
                ObjPaciente.Pat_city = (LectorPaciente.Pat_city == null) ? "" : LectorPaciente.Pat_city;
                ObjPaciente.Pat_zipCode = (LectorPaciente.Pat_zipCode == null) ? "" : LectorPaciente.Pat_zipCode;
                ObjPaciente.Pat_zipCodeExt = (LectorPaciente.Pat_zipCodeExt == null) ? "" : LectorPaciente.Pat_zipCodeExt;
                ObjPaciente.Pat_phone = (LectorPaciente.Pat_phone ==null ) ? "": LectorPaciente.Pat_phone;
                ObjPaciente.Pat_fax = (LectorPaciente.Pat_fax == null) ? "" : LectorPaciente.Pat_fax;
                ObjPaciente.Pat_sex = (LectorPaciente.Pat_sex ==null)  ? "" : LectorPaciente.Pat_sex;
                Lista.Add(ObjPaciente);
            }

            return Lista;
        }

        public List<PPatient> ListarParaTrazabilidad()
        {
            List<PPatient> Lista = new List<PPatient>();
            DateTime Pat_birthday;
            
            try
            {
                var Pacientes = db.SP_LIST_PATIENT_FOR_TRACEABILITY();

                foreach (var LectorPaciente in Pacientes)
                {
                    PPatient ObjPaciente = new PPatient();

                    ObjPaciente.Pat_code = LectorPaciente.Pat_code;
                    ObjPaciente.Pat_firstName = LectorPaciente.Pat_firstName;
                    ObjPaciente.Pat_lastName = (LectorPaciente.Pat_lastName == null) ? "" : LectorPaciente.Pat_lastName;
                    Pat_birthday = Convert.ToDateTime(LectorPaciente.Pat_birthday);
                    ObjPaciente.Pat_birthday = Pat_birthday.ToString("MM/dd/yyyy");
                    ObjPaciente.Pat_socialSecurityNumberD = (LectorPaciente.Pat_socialSecurityNumberD == null) ? "" : LectorPaciente.Pat_socialSecurityNumberD;
                    ObjPaciente.Pat_address = (LectorPaciente.Pat_address == null) ? "" : LectorPaciente.Pat_address;
                    ObjPaciente.Pat_state = (LectorPaciente.Pat_state == null) ? "" : LectorPaciente.Pat_state;
                    ObjPaciente.Pat_city = (LectorPaciente.Pat_city == null) ? "" : LectorPaciente.Pat_city;
                    ObjPaciente.Pat_zipCode = (LectorPaciente.Pat_zipCode == null) ? "" : LectorPaciente.Pat_zipCode;
                    ObjPaciente.Pat_zipCodeExt = (LectorPaciente.Pat_zipCodeExt == null) ? "" : LectorPaciente.Pat_zipCodeExt;
                    ObjPaciente.Pat_phone = (LectorPaciente.Pat_phone == null) ? "" : LectorPaciente.Pat_phone;
                    ObjPaciente.Pat_fax = (LectorPaciente.Pat_fax == null) ? "" : LectorPaciente.Pat_fax;
                    ObjPaciente.Pat_sex = (LectorPaciente.Pat_sex == null) ? "" : LectorPaciente.Pat_sex;
                    Lista.Add(ObjPaciente);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_LIST_PATIENT_FOR_FIND_MATCHES_Result> ListarParaAutocompletar()
        {
            List<SP_LIST_PATIENT_FOR_FIND_MATCHES_Result> Resultado = new List<SP_LIST_PATIENT_FOR_FIND_MATCHES_Result>();
            ObjectResult<SP_LIST_PATIENT_FOR_FIND_MATCHES_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_PATIENT_FOR_FIND_MATCHES();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_PATIENT_FOR_FIND_MATCHES_Result NodoResultado = new SP_LIST_PATIENT_FOR_FIND_MATCHES_Result();

                    NodoResultado.Codigo = RPAD.Codigo;
                    NodoResultado.NombreCompleto = RPAD.NombreCompleto;
                    

                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public List<SP_SAVE_PATIENT_Result> Insertar(Patient Paciente)
        
        {
            List<SP_SAVE_PATIENT_Result> Resultado = new List<SP_SAVE_PATIENT_Result>();
            ObjectResult<SP_SAVE_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_PATIENT(Paciente.Pat_firstName       
                                                                      , Paciente.Pat_lastName       
                                                                      , Paciente.Pat_socialSecurityNumberD
                                                                      , Paciente.Pat_birthday
                                                                      , Paciente.Pat_address
                                                                      , Paciente.Pat_city
                                                                      , Paciente.Pat_state
                                                                      , Paciente.Pat_zipCode
                                                                      , Paciente.Pat_phone
                                                                      , Paciente.Pat_fax
                                                                      , Paciente.Pat_zipCodeExt
                                                                      , Paciente.Pat_sex
                                                                      , Paciente.Pat_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_PATIENT_Result NodoResultado = new SP_SAVE_PATIENT_Result();

                    NodoResultado.PatientErrorCode = RPAD.PatientErrorCode;
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

        public List<SP_UPDATE_PATIENT_Result> Actualizar(Patient Paciente)
        {
            List<SP_UPDATE_PATIENT_Result> Resultado = new List<SP_UPDATE_PATIENT_Result>();
            ObjectResult<SP_UPDATE_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_PATIENT(Paciente.Pat_code
                                                                          , Paciente.Pat_firstName
                                                                          , Paciente.Pat_lastName
                                                                          , Paciente.Pat_socialSecurityNumberD
                                                                          , Paciente.Pat_birthday
                                                                          , Paciente.Pat_address
                                                                          , Paciente.Pat_city
                                                                          , Paciente.Pat_state
                                                                          , Paciente.Pat_zipCode
                                                                          , Paciente.Pat_phone
                                                                          , Paciente.Pat_fax
                                                                          , Paciente.Pat_zipCodeExt
                                                                          , Paciente.Pat_sex
                                                                          , Paciente.Pat_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_PATIENT_Result NodoResultado = new SP_UPDATE_PATIENT_Result();

                    NodoResultado.PatientErrorCode = RPAD.PatientErrorCode;
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

        public PPatient Buscar(int Codigo)
        {
            PPatient ObjPaciente = new PPatient();

            DateTime Pat_birthday;

            try
            {
                var Resultado = db.SP_FIND_PATIENT(Codigo);

                foreach (var NodoResultado in Resultado)
                {
                    ObjPaciente.Pat_code = NodoResultado.Pat_code;
                    ObjPaciente.Pat_firstName = NodoResultado.Pat_firstName;
                    ObjPaciente.Pat_lastName = NodoResultado.Pat_lastName;
                    ObjPaciente.Pat_socialSecurityNumberD = NodoResultado.Pat_socialSecurityNumberD;
                    Pat_birthday = Convert.ToDateTime(NodoResultado.Pat_birthday);
                    ObjPaciente.Pat_birthday = Pat_birthday.ToString("MM/dd/yyyy");
                    ObjPaciente.Pat_address = NodoResultado.Pat_address;
                    ObjPaciente.Pat_city = NodoResultado.Pat_city;                    
                    ObjPaciente.Pat_state = NodoResultado.Pat_state;
                    ObjPaciente.Sta_abbreviation = NodoResultado.Sta_abbreviation;
                    ObjPaciente.Pat_zipCode = NodoResultado.Pat_zipCode;   
                    ObjPaciente.Pat_phone = NodoResultado.Pat_phone;
                    ObjPaciente.Pat_fax = NodoResultado.Pat_fax;   
                    ObjPaciente.Pat_zipCodeExt = NodoResultado.Pat_zipCodeExt;
                    ObjPaciente.Pat_sex = NodoResultado.Pat_sex;                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjPaciente;
        }


        public List<SP_DELETE_PATIENT_Result> Eliminar(int Codigo= 0)
        {
            List<SP_DELETE_PATIENT_Result> Resultado = new List<SP_DELETE_PATIENT_Result>();
            ObjectResult<SP_DELETE_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_DELETE_PATIENT(Codigo);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_DELETE_PATIENT_Result NodoResultado = new SP_DELETE_PATIENT_Result();

                    NodoResultado.PatientErrorCode = RPAD.PatientErrorCode;
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

        public List<SP_FIND_HOMONYM_PATIENTS_Result> BuscarHomonimos(Patient Paciente)
        {
            List<SP_FIND_HOMONYM_PATIENTS_Result> Resultado = new List<SP_FIND_HOMONYM_PATIENTS_Result>();
            ObjectResult<SP_FIND_HOMONYM_PATIENTS_Result> ResultadoProcedimientoAlmacenadoDuro;
            ObjectParameter ParametroSalida = new ObjectParameter("ParametroSalida", typeof(Int64)); // Variable que se crea para recibir el valor del parámetro de salida del procedimiento almacenado. Si encontró coincidencias las devuelve

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_FIND_HOMONYM_PATIENTS( ParametroSalida
                                                                                   ,Paciente.Pat_code
                                                                                   ,Paciente.Pat_firstName
                                                                                    ,Paciente.Pat_lastName 
                                                                                   ,Paciente.Pat_zipCode 
                                                                                   ,Paciente.Pat_city
                                                                                   ,Paciente.Pat_state
                                                                                   , Paciente.Pat_socialSecurityNumberD);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_FIND_HOMONYM_PATIENTS_Result NodoResultado = new SP_FIND_HOMONYM_PATIENTS_Result();

                    NodoResultado.Pat_code = RPAD.Pat_code;
                    NodoResultado.Pat_firstName = RPAD.Pat_firstName;
                    NodoResultado.Pat_lastName = RPAD.Pat_lastName;
                    NodoResultado.Pat_zipCode = RPAD.Pat_zipCode;
                    NodoResultado.Pat_address = RPAD.Pat_address;
                    NodoResultado.Pat_city = RPAD.Pat_city;
                    NodoResultado.Pat_state = RPAD.Pat_state;
                    NodoResultado.Pat_status = RPAD.Pat_status;

                    Resultado.Add(RPAD);
                }

                if (Convert.ToInt64(ParametroSalida.Value) == 0)        // Retorna 0 cuando no encuentra coincidencias.
                    return null;
                else // puede retornar 1 o 2 (1 es cuando encuentra coincidencias y 2 es cuando es error de duplicado de socialSecurityNumber)
                    return Resultado;


                // else if (Convert.ToInt64(ParametroSalida.Value) == 1)   // Retorna 1 cuando encuentra coincidencias.
                //     return Resultado;
                // else if (Convert.ToInt64(ParametroSalida.Value) == 2)   // Retorna 2 cuando es error de duplicado de socialSecurityNumber.
                //     return null;

                return null;    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SP_UPDATE_SEX_PATIENT_Result ActualizaSexo(int Codigo, string Sexo)
        {
            SP_UPDATE_SEX_PATIENT_Result Resultado = new SP_UPDATE_SEX_PATIENT_Result();
            ObjectResult<SP_UPDATE_SEX_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_SEX_PATIENT(Codigo, Sexo);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    Resultado.PatientErrorCode = RPAD.PatientErrorCode;
                    Resultado.ErrorMessage = RPAD.ErrorMessage.ToString();
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