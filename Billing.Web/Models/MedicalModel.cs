using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class MedicalModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<PMedical> ListarDoctores()
        {
            List<PMedical> Lista = new List<PMedical>();
            var Doctores = db.SP_LIST_MEDICAL();
            string CodigoEspecialidad;

            foreach (var LectorDoctor in Doctores)
            {
                PMedical ObjDoctor = new PMedical();
                Specialty ObjEspecialidad = new Specialty();

                ObjDoctor.Med_code = LectorDoctor.Med_code;
                ObjDoctor.Med_firstName = (LectorDoctor.Med_firstName == null) ? "" : LectorDoctor.Med_firstName;
                ObjDoctor.Med_lastName = (LectorDoctor.Med_lastName  == null) ? "" : LectorDoctor.Med_lastName;
                ObjDoctor.Med_address = (LectorDoctor.Med_address == null) ? "" :  LectorDoctor.Med_address; 
                ObjDoctor.Med_city = (LectorDoctor.Med_city == null) ? "" : LectorDoctor.Med_city ;
                ObjDoctor.Med_state = (LectorDoctor.Med_state == null) ? "" : LectorDoctor.Med_state;
                ObjDoctor.Med_zipCode = (LectorDoctor.Med_zipCode == null) ? "" : LectorDoctor.Med_zipCode;
                ObjDoctor.Med_zipCodeExt = (LectorDoctor.Med_zipCodeExt == null) ? "" : LectorDoctor.Med_zipCodeExt;
                ObjDoctor.Med_phone = (LectorDoctor.Med_phone == null) ? "" : LectorDoctor.Med_phone;
                ObjDoctor.Med_phoneExt = (LectorDoctor.Med_phoneExt == null) ? "" : LectorDoctor.Med_phoneExt;
                ObjDoctor.Med_alternatePhone = (LectorDoctor.Med_alternatePhone == null) ? "" : LectorDoctor.Med_alternatePhone ;
                ObjDoctor.Med_fax = (LectorDoctor.Med_fax == null) ? "" : LectorDoctor.Med_fax;

                CodigoEspecialidad = LectorDoctor.Spe_code == null ? "" : LectorDoctor.Spe_code.ToString(); // Primero se convierte el código de especialidad (entero) a cadena
                ObjEspecialidad.Spe_code = ((CodigoEspecialidad == "") ? 0 : int.Parse(CodigoEspecialidad)); // y luego se convierte de cadena a entero.
                ObjEspecialidad.Spe_name = (LectorDoctor.Spe_name == null) ? "" : LectorDoctor.Spe_name;               
                ObjDoctor.Specialty = ObjEspecialidad;

                ObjDoctor.Med_office = (LectorDoctor.Med_office == null) ? "" : LectorDoctor.Med_office;

                Lista.Add(ObjDoctor);

            }

            return Lista;
        }

        public List<PMedical> ListarDoctoresParaTrazabilidad()
        {
            List<PMedical> Lista = new List<PMedical>();
            string CodigoEspecialidad;

            try
            {
                var Doctores = db.SP_LIST_MEDICAL_FOR_TRACEABILITY();

                foreach (var LectorDoctor in Doctores)
                {
                    PMedical ObjDoctor = new PMedical();
                    Specialty ObjEspecialidad = new Specialty();

                    ObjDoctor.Med_code = LectorDoctor.Med_code;
                    ObjDoctor.Med_firstName = (LectorDoctor.Med_firstName == null) ? "" : LectorDoctor.Med_firstName;
                    ObjDoctor.Med_lastName = (LectorDoctor.Med_lastName == null) ? "" : LectorDoctor.Med_lastName;
                    ObjDoctor.Med_address = (LectorDoctor.Med_address == null) ? "" : LectorDoctor.Med_address;
                    ObjDoctor.Med_city = (LectorDoctor.Med_city == null) ? "" : LectorDoctor.Med_city;
                    ObjDoctor.Med_state = (LectorDoctor.Med_state == null) ? "" : LectorDoctor.Med_state;
                    ObjDoctor.Med_zipCode = (LectorDoctor.Med_zipCode == null) ? "" : LectorDoctor.Med_zipCode;
                    ObjDoctor.Med_zipCodeExt = (LectorDoctor.Med_zipCodeExt == null) ? "" : LectorDoctor.Med_zipCodeExt;
                    ObjDoctor.Med_phone = (LectorDoctor.Med_phone == null) ? "" : LectorDoctor.Med_phone;
                    ObjDoctor.Med_phoneExt = (LectorDoctor.Med_phoneExt == null) ? "" : LectorDoctor.Med_phoneExt;
                    ObjDoctor.Med_alternatePhone = (LectorDoctor.Med_alternatePhone == null) ? "" : LectorDoctor.Med_alternatePhone;
                    ObjDoctor.Med_fax = (LectorDoctor.Med_alternatePhone == null) ? "" : LectorDoctor.Med_alternatePhone;

                    CodigoEspecialidad = LectorDoctor.Spe_code == null ? "" : LectorDoctor.Spe_code.ToString(); // Primero se convierte el código de especialidad (entero) a cadena
                    ObjEspecialidad.Spe_code = ((CodigoEspecialidad == "") ? 0 : int.Parse(CodigoEspecialidad)); // y luego se convierte de cadena a entero.
                    ObjEspecialidad.Spe_name = (LectorDoctor.Spe_name == null) ? "" : LectorDoctor.Spe_name;
                    ObjDoctor.Specialty = ObjEspecialidad;

                    ObjDoctor.Med_office = (LectorDoctor.Med_office == null) ? "" : LectorDoctor.Med_office;

                    Lista.Add(ObjDoctor);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_LIST_MEDICAL_FOR_FIND_MATCHES_Result> ListarParaAutocompletar()
        {
            List<SP_LIST_MEDICAL_FOR_FIND_MATCHES_Result> Resultado = new List<SP_LIST_MEDICAL_FOR_FIND_MATCHES_Result>();
            ObjectResult<SP_LIST_MEDICAL_FOR_FIND_MATCHES_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_MEDICAL_FOR_FIND_MATCHES();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_MEDICAL_FOR_FIND_MATCHES_Result NodoResultado = new SP_LIST_MEDICAL_FOR_FIND_MATCHES_Result();

                    NodoResultado.Codigo = RPAD.Codigo;
                    NodoResultado.NombreCompleto = RPAD.NombreCompleto;
                    NodoResultado.Direccion = RPAD.Direccion;
                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public List<SP_SAVE_MEDICAL_Result> InsertarDoctor(Medical Doctor)
        {
            List<SP_SAVE_MEDICAL_Result> Resultado = new List<SP_SAVE_MEDICAL_Result>();
            ObjectResult<SP_SAVE_MEDICAL_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_MEDICAL(Doctor.Med_firstName
                                                                          , Doctor.Med_lastName
                                                                          , Doctor.Med_address
                                                                          , Doctor.Med_city
                                                                          , Doctor.Med_state
                                                                          , Doctor.Med_zipCode
                                                                          , Doctor.Med_zipCodeExt
                                                                          , Doctor.Med_phone
                                                                          , Doctor.Med_phoneExt
                                                                          , Doctor.Med_alternatePhone
                                                                          , Doctor.Med_fax
                                                                          , Doctor.Spe_code
                                                                          , Doctor.Med_office
                                                                          , Doctor.Med_operatorUser);
                                                                                
                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_MEDICAL_Result NodoResultado = new SP_SAVE_MEDICAL_Result();

                    NodoResultado.MedicalErrorCode = RPAD.MedicalErrorCode;
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

        public PMedical BuscarDoctor(int CodigoDoctor)
        {
            PMedical ObjDoctor = new PMedical();
            Specialty ObjEspecialidad = new Specialty();
            ObjectResult<SP_FIND_MEDICAL_Result> Resultado;
            string CodigoEspecialidad = (String)null;

            try
            {
                Resultado = db.SP_FIND_MEDICAL(CodigoDoctor);

                foreach (var NodoResultado in Resultado)
                {
                    ObjDoctor.Med_code = NodoResultado.Med_code;
                    ObjDoctor.Med_firstName = NodoResultado.Med_firstName;
                    ObjDoctor.Med_lastName = NodoResultado.Med_lastName;
                    ObjDoctor.Med_address = NodoResultado.Med_address;
                    ObjDoctor.Med_city = NodoResultado.Med_city;
                    ObjDoctor.Med_state = NodoResultado.Med_state;
                    ObjDoctor.Sta_abbreviation = NodoResultado.Sta_abbreviation;
                    ObjDoctor.Med_zipCode = NodoResultado.Med_zipCode;
                    ObjDoctor.Med_zipCodeExt = NodoResultado.Med_zipCodeExt;
                    ObjDoctor.Med_phone = NodoResultado.Med_phone;
                    ObjDoctor.Med_phoneExt = NodoResultado.Med_phoneExt;
                    ObjDoctor.Med_alternatePhone = NodoResultado.Med_alternatePhone;
                    ObjDoctor.Med_fax = NodoResultado.Med_fax;

                    CodigoEspecialidad = NodoResultado.Spe_code == null ? "" : NodoResultado.Spe_code.ToString(); // Primero se convierte el código de especialidad (entero) a cadena
                    ObjEspecialidad.Spe_code = ((CodigoEspecialidad == "") ? 0 : int.Parse(CodigoEspecialidad)); // y luego se convierte de cadena a entero.
                    ObjEspecialidad.Spe_name = NodoResultado.Spe_name;

                    ObjDoctor.Specialty = ObjEspecialidad;

                    ObjDoctor.Med_office = NodoResultado.Med_office;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjDoctor;
        }

        public List<SP_UPDATE_MEDICAL_Result> ActualizarDoctor(Medical Doctor)
        {
            List<SP_UPDATE_MEDICAL_Result> Resultado = new List<SP_UPDATE_MEDICAL_Result>();
            ObjectResult<SP_UPDATE_MEDICAL_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_MEDICAL(Doctor.Med_code
                                                                            , Doctor.Med_firstName
                                                                            , Doctor.Med_lastName
                                                                            , Doctor.Med_address
                                                                            , Doctor.Med_city
                                                                            , Doctor.Med_state
                                                                            , Doctor.Med_zipCode
                                                                            , Doctor.Med_zipCodeExt
                                                                            , Doctor.Med_phone
                                                                            , Doctor.Med_phoneExt
                                                                            , Doctor.Med_alternatePhone
                                                                            , Doctor.Med_fax
                                                                            , Doctor.Spe_code
                                                                           , Doctor.Med_office
                                                                           , Doctor.Med_operatorUser);
                    
                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_MEDICAL_Result NodoResultado = new SP_UPDATE_MEDICAL_Result();

                    NodoResultado.MedicalErrorCode = RPAD.MedicalErrorCode;
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

        public List<SP_DELETE_MEDICAL_Result> EliminarDoctor(int CodigoDoctor = 0)
        {
            List<SP_DELETE_MEDICAL_Result> Resultado = new List<SP_DELETE_MEDICAL_Result>();
            ObjectResult<SP_DELETE_MEDICAL_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_DELETE_MEDICAL(CodigoDoctor);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_DELETE_MEDICAL_Result NodoResultado = new SP_DELETE_MEDICAL_Result();

                    NodoResultado.MedicalErrorCode = RPAD.MedicalErrorCode;
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

        public List<SP_FIND_HOMONYM_MEDICALS_Result> BuscarHomonimos(Medical Doctor)
        {
            List<SP_FIND_HOMONYM_MEDICALS_Result> Resultado = new List<SP_FIND_HOMONYM_MEDICALS_Result>();
            ObjectResult<SP_FIND_HOMONYM_MEDICALS_Result> ResultadoProcedimientoAlmacenadoDuro;
            ObjectParameter ParametroSalida = new ObjectParameter("ParametroSalida", typeof(bool)); // Variable que se crea para recibir el valor del parámetro de salida del procedimiento almacenado. Si encontró coincidencias las devuelve

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_FIND_HOMONYM_MEDICALS(ParametroSalida
                                                                                   , Doctor.Med_code
                                                                                   , Doctor.Med_firstName
                                                                                   , Doctor.Med_lastName
                                                                                   , Doctor.Med_zipCode
                                                                                   , Doctor.Med_city
                                                                                   , Doctor.Med_state);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_FIND_HOMONYM_MEDICALS_Result NodoResultado = new SP_FIND_HOMONYM_MEDICALS_Result();

                    NodoResultado.Med_firstName =   RPAD.Med_firstName;
                    NodoResultado.Med_lastName =    RPAD.Med_lastName;
                    NodoResultado.Med_zipCode =     RPAD.Med_zipCode;
                    NodoResultado.Med_address =     RPAD.Med_address;
                    NodoResultado.Med_city =        RPAD.Med_city;
                    NodoResultado.Med_state =       RPAD.Med_state;
                    NodoResultado.Med_status = RPAD.Med_status;

                    Resultado.Add(RPAD);
                }

                if ((bool)ParametroSalida.Value)
                    return Resultado;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


}