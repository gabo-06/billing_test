using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class AttorneyModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<PAttorney> Listar()
        {
            List<PAttorney> Lista = new List<PAttorney>();
            var Abogados = db.SP_LIST_ATTORNEY();            
            string CodigoEspecialidad;

            foreach (var LectorAbogado in Abogados) 
            {
                PAttorney ObjAbogado = new PAttorney();
                Specialty ObjEspecialidad = new Specialty(); 

                ObjAbogado.Att_code = LectorAbogado.Att_code;
                ObjAbogado.Att_firstName = (LectorAbogado.Att_firstName == null) ? "" : LectorAbogado.Att_firstName;
                ObjAbogado.Att_lastName = (LectorAbogado.Att_lastName == null) ? "" : LectorAbogado.Att_lastName;
                ObjAbogado.Att_address = (LectorAbogado.Att_address  == null) ? "" : LectorAbogado.Att_address;
                ObjAbogado.Att_phone = (LectorAbogado.Att_phone == null) ? "" : LectorAbogado.Att_phone;
                ObjAbogado.Att_fax = (LectorAbogado.Att_fax == null) ? "" : LectorAbogado.Att_fax;
                ObjAbogado.Att_assistant = (LectorAbogado.Att_assistant == null) ? "" : LectorAbogado.Att_assistant;
                ObjAbogado.Att_city = (LectorAbogado.Att_city == null) ? "" : LectorAbogado.Att_city;
                ObjAbogado.Att_state = (LectorAbogado.Att_state == null) ? "" : LectorAbogado.Att_state;
                ObjAbogado.Att_zipCode = (LectorAbogado.Att_zipCode== null) ? "" : LectorAbogado.Att_zipCode ;
                ObjAbogado.Att_zipCodeExt = (LectorAbogado.Att_zipCodeExt == null) ? "" : LectorAbogado.Att_zipCodeExt;

                CodigoEspecialidad = LectorAbogado.Spe_code == null ? "" : LectorAbogado.Spe_code.ToString(); // Primero se convierte el código de especialidad (entero) a cadena
                ObjEspecialidad.Spe_code = ((CodigoEspecialidad == "") ? 0 : int.Parse(CodigoEspecialidad)); // y luego se convierte de cadena a entero.
                ObjEspecialidad.Spe_name = (LectorAbogado.Spe_name == null) ? "" : LectorAbogado.Spe_name;                
                ObjAbogado.Specialty = ObjEspecialidad;

                Lista.Add(ObjAbogado);
            }

            return Lista;
        }

        public List<PAttorney> ListarParaTrazabilidad()
        {
            List<PAttorney> Lista = new List<PAttorney>();
            var Abogados = db.SP_LIST_ATTORNEY_FOR_TRACEABILITY();
            string CodigoEspecialidad;

            foreach (var LectorAbogado in Abogados)
            {
                PAttorney ObjAbogado = new PAttorney();
                Specialty ObjEspecialidad = new Specialty();

                ObjAbogado.Att_code = LectorAbogado.Att_code;
                ObjAbogado.Att_firstName = (LectorAbogado.Att_firstName == null) ? "" : LectorAbogado.Att_firstName;
                ObjAbogado.Att_lastName = (LectorAbogado.Att_lastName == null) ? "" : LectorAbogado.Att_lastName;
                ObjAbogado.Att_address = (LectorAbogado.Att_address == null) ? "" : LectorAbogado.Att_address;
                ObjAbogado.Att_phone = (LectorAbogado.Att_phone == null) ? "" : LectorAbogado.Att_phone;
                ObjAbogado.Att_fax = (LectorAbogado.Att_fax == null) ? "" : LectorAbogado.Att_fax;
                ObjAbogado.Att_assistant = (LectorAbogado.Att_assistant == null) ? "" : LectorAbogado.Att_assistant;
                ObjAbogado.Att_city = (LectorAbogado.Att_city == null) ? "" : LectorAbogado.Att_city;
                ObjAbogado.Att_state = (LectorAbogado.Att_state == null) ? "" : LectorAbogado.Att_state;
                ObjAbogado.Att_zipCode = (LectorAbogado.Att_zipCode == null) ? "" : LectorAbogado.Att_zipCode;
                ObjAbogado.Att_zipCodeExt = (LectorAbogado.Att_zipCodeExt == null) ? "" : LectorAbogado.Att_zipCodeExt;

                CodigoEspecialidad = LectorAbogado.Spe_code == null ? "" : LectorAbogado.Spe_code.ToString(); // Primero se convierte el código de especialidad (entero) a cadena
                ObjEspecialidad.Spe_code = ((CodigoEspecialidad == "") ? 0 : int.Parse(CodigoEspecialidad)); // y luego se convierte de cadena a entero.
                ObjEspecialidad.Spe_name = (LectorAbogado.Spe_name == null) ? "" : LectorAbogado.Spe_name;
                ObjAbogado.Specialty = ObjEspecialidad;

                Lista.Add(ObjAbogado);
            }

            return Lista;
        }

        public List<SP_LIST_ATTORNEY_FOR_FIND_MATCHES_Result> ListarParaAutocompletar()
        {
            List<SP_LIST_ATTORNEY_FOR_FIND_MATCHES_Result> Resultado = new List<SP_LIST_ATTORNEY_FOR_FIND_MATCHES_Result>();
            ObjectResult<SP_LIST_ATTORNEY_FOR_FIND_MATCHES_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_ATTORNEY_FOR_FIND_MATCHES();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_ATTORNEY_FOR_FIND_MATCHES_Result NodoResultado = new SP_LIST_ATTORNEY_FOR_FIND_MATCHES_Result();

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

        public List<SP_SAVE_ATTORNEY_Result> Insertar(Attorney Abogado)
        {
            List<SP_SAVE_ATTORNEY_Result> Resultado = new List<SP_SAVE_ATTORNEY_Result>();
            ObjectResult<SP_SAVE_ATTORNEY_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_ATTORNEY(Abogado.Att_firstName
                                                                           , Abogado.Att_lastName
                                                                           , Abogado.Att_address
                                                                           , Abogado.Att_phone
                                                                           , Abogado.Att_fax
                                                                           , Abogado.Att_assistant
                                                                           , Abogado.Spe_code
                                                                           , Abogado.Att_city
                                                                           , Abogado.Att_state
                                                                           , Abogado.Att_zipCode
                                                                           , Abogado.Att_zipCodeExt
                                                                           ,Abogado.Att_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_ATTORNEY_Result NodoResultado = new SP_SAVE_ATTORNEY_Result();

                    NodoResultado.AttorneyErrorCode = RPAD.AttorneyErrorCode;
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

        public PAttorney Buscar(int Codigo = 0)
        {
            PAttorney ObjAbogado = new PAttorney();
            Specialty ObjEspecialidad = new Specialty();
            ObjectResult<SP_FIND_ATTORNEY_Result> Resultado;
            string CodigoEspecialidad = (String)null;

            try
            {
                Resultado = db.SP_FIND_ATTORNEY(Codigo);

                foreach (var NodoResultado in Resultado)
                {
                    ObjAbogado.Att_code = NodoResultado.Att_code;
                    ObjAbogado.Att_firstName = NodoResultado.Att_firstName;
                    ObjAbogado.Att_lastName = NodoResultado.Att_lastName;
                    ObjAbogado.Att_address = NodoResultado.Att_address;
                    ObjAbogado.Att_phone = NodoResultado.Att_phone;
                    ObjAbogado.Att_fax = NodoResultado.Att_fax;
                    ObjAbogado.Att_assistant = NodoResultado.Att_assistant;
                    ObjAbogado.Att_city = NodoResultado.Att_city;
                    ObjAbogado.Att_state = NodoResultado.Att_state;
                    ObjAbogado.Att_zipCode = NodoResultado.Att_zipCode;
                    ObjAbogado.Att_zipCodeExt = NodoResultado.Att_zipCodeExt;

                    CodigoEspecialidad = NodoResultado.Spe_code == null ? "" : NodoResultado.Spe_code.ToString(); // Primero se convierte el código de especialidad (entero) a cadena
                    ObjEspecialidad.Spe_code = ((CodigoEspecialidad == "") ? 0 : int.Parse(CodigoEspecialidad)); // y luego se convierte de cadena a entero.
                    ObjEspecialidad.Spe_name = NodoResultado.Spe_name;

                    ObjAbogado.Sta_abbreviation = NodoResultado.Sta_abbreviation;

                    ObjAbogado.Specialty = ObjEspecialidad;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjAbogado;
        }

        public List<SP_UPDATE_ATTORNEY_Result> Actualizar(Attorney Abogado)
        {
            List<SP_UPDATE_ATTORNEY_Result> Resultado = new List<SP_UPDATE_ATTORNEY_Result>();
            ObjectResult<SP_UPDATE_ATTORNEY_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_ATTORNEY(Abogado.Att_code
                                                                             , Abogado.Att_firstName
                                                                             , Abogado.Att_lastName
                                                                             , Abogado.Att_address
                                                                             , Abogado.Att_phone
                                                                             , Abogado.Att_fax
                                                                             , Abogado.Att_assistant
                                                                             , Abogado.Spe_code
                                                                             , Abogado.Att_city
                                                                             , Abogado.Att_state
                                                                             , Abogado.Att_zipCode
                                                                             , Abogado.Att_zipCodeExt
                                                                             , Abogado.Att_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_ATTORNEY_Result NodoResultado = new SP_UPDATE_ATTORNEY_Result();

                    NodoResultado.AttorneyErrorCode = RPAD.AttorneyErrorCode;
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

        public List<SP_DELETE_ATTORNEY_Result> Eliminar(int Codigo = 0)
        {
            List<SP_DELETE_ATTORNEY_Result> Resultado = new List<SP_DELETE_ATTORNEY_Result>();
            ObjectResult<SP_DELETE_ATTORNEY_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_DELETE_ATTORNEY(Codigo);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_DELETE_ATTORNEY_Result NodoResultado = new SP_DELETE_ATTORNEY_Result();

                    NodoResultado.AttorneyErrorCode = RPAD.AttorneyErrorCode;
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

        public List<SP_FIND_HOMONYM_ATTORNEYS_Result> BuscarHomonimos(Attorney Abogado)
        {
            List<SP_FIND_HOMONYM_ATTORNEYS_Result> Resultado = new List<SP_FIND_HOMONYM_ATTORNEYS_Result>();
            ObjectResult<SP_FIND_HOMONYM_ATTORNEYS_Result> ResultadoProcedimientoAlmacenadoDuro;
            ObjectParameter ParametroSalida = new ObjectParameter("ParametroSalida", typeof(bool)); // Variable que se crea para recibir el valor del parámetro de salida del procedimiento almacenado. Si encontró coincidencias las devuelve

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_FIND_HOMONYM_ATTORNEYS(ParametroSalida
                                                                                   , Abogado.Att_code
                                                                                   , Abogado.Att_firstName
                                                                                   , Abogado.Att_lastName
                                                                                   , Abogado.Att_zipCode
                                                                                   , Abogado.Att_city
                                                                                   , Abogado.Att_state);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_FIND_HOMONYM_ATTORNEYS_Result NodoResultado = new SP_FIND_HOMONYM_ATTORNEYS_Result();

                    NodoResultado.Att_firstName = RPAD.Att_firstName;
                    NodoResultado.Att_lastName  = RPAD.Att_lastName;
                    NodoResultado.Att_zipCode   = RPAD.Att_zipCode;
                    NodoResultado.Att_address   = RPAD.Att_address;
                    NodoResultado.Att_city      = RPAD.Att_city;
                    NodoResultado.Att_state     = RPAD.Att_state;
                    NodoResultado.Att_status = RPAD.Att_status;

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