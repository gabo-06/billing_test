using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{

    public class InsurerModel 
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<Insurer> ListaAseguradoras()
        {
            List<Insurer> Lista = new List<Insurer>();

            var Aseguradoras = db.SP_LIST_INSURER();

            foreach (var LectorAseguradora in Aseguradoras)
            {
                Insurer ObjAseguradora = new Insurer();

                ObjAseguradora.Ins_code        = LectorAseguradora.Ins_code;
                ObjAseguradora.Ins_name = (LectorAseguradora.Ins_name == null) ? "" : LectorAseguradora.Ins_name;
                ObjAseguradora.Ins_address     = (LectorAseguradora.Ins_address == null) ? "" : LectorAseguradora.Ins_address;
                ObjAseguradora.Ins_city        = (LectorAseguradora.Ins_city == null ) ? "" : LectorAseguradora.Ins_city;
                ObjAseguradora.Ins_state = (LectorAseguradora.Ins_state == null) ? "" : LectorAseguradora.Ins_state;
                ObjAseguradora.Ins_phone = (LectorAseguradora.Ins_phone == null) ? "" : LectorAseguradora.Ins_phone;
                ObjAseguradora.Ins_phoneExt = (LectorAseguradora.Ins_phoneExt == null) ? "" : LectorAseguradora.Ins_phoneExt;
                ObjAseguradora.Ins_fax = (LectorAseguradora.Ins_fax == null) ? "" : LectorAseguradora.Ins_fax;
                ObjAseguradora.Ins_zipCode = (LectorAseguradora.Ins_zipCode == null) ? "" : LectorAseguradora.Ins_zipCode;
                ObjAseguradora.Ins_zipExt = (LectorAseguradora.Ins_zipExt == null) ? "" : LectorAseguradora.Ins_zipExt;
                ObjAseguradora.Ins_scTpaCode = (LectorAseguradora.Ins_scTpaCode == null) ? "" : LectorAseguradora.Ins_scTpaCode;
                ObjAseguradora.Ins_feinSc =  (LectorAseguradora.Ins_feinSc == null) ? "" : LectorAseguradora.Ins_feinSc;
                ObjAseguradora.Ins_carrierCode = (LectorAseguradora.Ins_carrierCode == null) ? "" : LectorAseguradora.Ins_carrierCode;
                ObjAseguradora.Ins_feinCc = (LectorAseguradora.Ins_feinCc == null) ? "" : LectorAseguradora.Ins_feinCc;

                Lista.Add(ObjAseguradora);
            }

            return Lista;
        }

        public List<Insurer> ListarParaTrazabilidad()
        {
            List<Insurer> Lista = new List<Insurer>();

            var Aseguradoras = db.SP_LIST_INSURER_FOR_TRACEABILITY();

            foreach (var LectorAseguradora in Aseguradoras)
            {
                Insurer ObjAseguradora = new Insurer();

                ObjAseguradora.Ins_code = LectorAseguradora.Ins_code;
                ObjAseguradora.Ins_name = (LectorAseguradora.Ins_name == null) ? "" : LectorAseguradora.Ins_name;
                ObjAseguradora.Ins_address = (LectorAseguradora.Ins_address == null) ? "" : LectorAseguradora.Ins_address;
                ObjAseguradora.Ins_city = (LectorAseguradora.Ins_city == null) ? "" : LectorAseguradora.Ins_city;
                ObjAseguradora.Ins_state = (LectorAseguradora.Ins_state == null) ? "" : LectorAseguradora.Ins_state;
                ObjAseguradora.Ins_phone = (LectorAseguradora.Ins_phone == null) ? "" : LectorAseguradora.Ins_phone;
                ObjAseguradora.Ins_phoneExt = (LectorAseguradora.Ins_phoneExt == null) ? "" : LectorAseguradora.Ins_phoneExt;
                ObjAseguradora.Ins_fax = (LectorAseguradora.Ins_fax == null) ? "" : LectorAseguradora.Ins_fax;
                ObjAseguradora.Ins_zipCode = (LectorAseguradora.Ins_zipCode == null) ? "" : LectorAseguradora.Ins_zipCode;
                ObjAseguradora.Ins_zipExt = (LectorAseguradora.Ins_zipExt == null) ? "" : LectorAseguradora.Ins_zipExt;
                ObjAseguradora.Ins_scTpaCode = (LectorAseguradora.Ins_scTpaCode == null) ? "" : LectorAseguradora.Ins_scTpaCode;
                ObjAseguradora.Ins_feinSc = (LectorAseguradora.Ins_feinSc == null) ? "" : LectorAseguradora.Ins_feinSc;
                ObjAseguradora.Ins_carrierCode = (LectorAseguradora.Ins_carrierCode == null) ? "" : LectorAseguradora.Ins_carrierCode;
                ObjAseguradora.Ins_feinCc = (LectorAseguradora.Ins_feinCc == null) ? "" : LectorAseguradora.Ins_feinCc;

                Lista.Add(ObjAseguradora);
            }

            return Lista;
        }

        public List<SP_LIST_INSURER_FOR_FIND_MATCHES_Result> ListarParaAutocompletar()
        {
            List<SP_LIST_INSURER_FOR_FIND_MATCHES_Result> Resultado = new List<SP_LIST_INSURER_FOR_FIND_MATCHES_Result>();
            ObjectResult<SP_LIST_INSURER_FOR_FIND_MATCHES_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_INSURER_FOR_FIND_MATCHES();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_INSURER_FOR_FIND_MATCHES_Result NodoResultado = new SP_LIST_INSURER_FOR_FIND_MATCHES_Result();

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

        public List<SP_LIST_INSURER_FOR_PAYMENT_Result> ListarParaPago()
        {
            List<SP_LIST_INSURER_FOR_PAYMENT_Result> Resultado = new List<SP_LIST_INSURER_FOR_PAYMENT_Result>();
            ObjectResult<SP_LIST_INSURER_FOR_PAYMENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_INSURER_FOR_PAYMENT();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_INSURER_FOR_PAYMENT_Result NodoResultado = new SP_LIST_INSURER_FOR_PAYMENT_Result();

                    NodoResultado.Ins_code = RPAD.Ins_code;
                    NodoResultado.Ins_name = RPAD.Ins_name;


                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public List<SP_LIST_INSURER_FOR_REMOVE_PAYMENT_Result> ListarParaEliminarPago()
        {
            List<SP_LIST_INSURER_FOR_REMOVE_PAYMENT_Result> Resultado = new List<SP_LIST_INSURER_FOR_REMOVE_PAYMENT_Result>();
            ObjectResult<SP_LIST_INSURER_FOR_REMOVE_PAYMENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_INSURER_FOR_REMOVE_PAYMENT();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_INSURER_FOR_REMOVE_PAYMENT_Result NodoResultado = new SP_LIST_INSURER_FOR_REMOVE_PAYMENT_Result();

                    NodoResultado.Ins_code = RPAD.Ins_code;
                    NodoResultado.Ins_name = RPAD.Ins_name;

                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public List<SP_LIST_INSURER_FOR_PAYMENT_PROCESS_Result> ListarParaProcesoPago()
        {
            List<SP_LIST_INSURER_FOR_PAYMENT_PROCESS_Result> Resultado = new List<SP_LIST_INSURER_FOR_PAYMENT_PROCESS_Result>();
            ObjectResult<SP_LIST_INSURER_FOR_PAYMENT_PROCESS_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_INSURER_FOR_PAYMENT_PROCESS();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_INSURER_FOR_PAYMENT_PROCESS_Result NodoResultado = new SP_LIST_INSURER_FOR_PAYMENT_PROCESS_Result();

                    NodoResultado.Ins_code = RPAD.Ins_code;
                    NodoResultado.Ins_name = RPAD.Ins_name.Trim();
                    NodoResultado.Ins_status = RPAD.Ins_status;

                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public List<SP_SAVE_INSURER_Result> Insertar(Insurer Aseguradora)
        {
            List<SP_SAVE_INSURER_Result> Resultado = new List<SP_SAVE_INSURER_Result>();
            ObjectResult<SP_SAVE_INSURER_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_INSURER(Aseguradora.Ins_name			
                                                                          ,Aseguradora.Ins_phone			
                                                                          ,Aseguradora.Ins_phoneExt		
                                                                          ,Aseguradora.Ins_fax			
                                                                          ,Aseguradora.Ins_address		
                                                                          ,Aseguradora.Ins_city			
                                                                          ,Aseguradora.Ins_state			
                                                                          ,Aseguradora.Ins_zipCode		
                                                                          ,Aseguradora.Ins_zipExt		
                                                                          ,Aseguradora.Ins_scTpaCode		
                                                                          ,Aseguradora.Ins_feinSc		
                                                                          ,Aseguradora.Ins_carrierCode	
                                                                          ,Aseguradora.Ins_feinCc
                                                                          ,Aseguradora.Ins_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_INSURER_Result NodoResultado = new SP_SAVE_INSURER_Result();

                    NodoResultado.InsurerErrorCode = RPAD.InsurerErrorCode;
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

        public List<SP_UPDATE_INSURER_Result> Actualizar(Insurer Aseguradora)
        {
            List<SP_UPDATE_INSURER_Result> Resultado = new List<SP_UPDATE_INSURER_Result>();
            ObjectResult<SP_UPDATE_INSURER_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_INSURER(Aseguradora.Ins_code		
                                                                            ,Aseguradora.Ins_name		
                                                                            ,Aseguradora.Ins_phone		
                                                                            ,Aseguradora.Ins_phoneExt	
                                                                            ,Aseguradora.Ins_fax			
                                                                            ,Aseguradora.Ins_address		
                                                                            ,Aseguradora.Ins_city		
                                                                            ,Aseguradora.Ins_state		
                                                                            ,Aseguradora.Ins_zipCode		
                                                                            ,Aseguradora.Ins_zipExt		
                                                                            ,Aseguradora.Ins_scTpaCode	
                                                                            ,Aseguradora.Ins_feinSc		
                                                                            ,Aseguradora.Ins_carrierCode	
                                                                            ,Aseguradora.Ins_feinCc
                                                                            ,Aseguradora.Ins_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_INSURER_Result NodoResultado = new SP_UPDATE_INSURER_Result();

                    NodoResultado.InsurerErrorCode = RPAD.InsurerErrorCode;
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

        public List<SP_DELETE_INSURER_Result> Eliminar(int Codigo = 0)
        {
            List<SP_DELETE_INSURER_Result> Resultado = new List<SP_DELETE_INSURER_Result>();
            ObjectResult<SP_DELETE_INSURER_Result> ResultadoProcedimientoAlmacenadoDuro;
            ObjectParameter Name = new ObjectParameter("Name", typeof(String)); // Variable que se crea para recibir el valor del parámetro de salida del procedimiento almacenado

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_DELETE_INSURER(Codigo);
                
                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_DELETE_INSURER_Result NodoResultado = new SP_DELETE_INSURER_Result();

                    NodoResultado.InsurerErrorCode = RPAD.InsurerErrorCode;
                    NodoResultado.ErrorMessage = RPAD.ErrorMessage.ToString();

                    Resultado.Add(NodoResultado);
                }

                // if (Name.Value.Equals("hay error"))
                // {
                //     Debug.WriteLine("si");
                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public PInsurer Buscar(int Codigo = 0)
        {
            PInsurer ObjAseguradora = new PInsurer();

            try
            {
                var Resultado = db.SP_FIND_INSURER(Codigo);

                foreach (var NodoResultado in Resultado)
                {
                    ObjAseguradora.Ins_code         = NodoResultado.Ins_code;
                    ObjAseguradora.Ins_name         = NodoResultado.Ins_name;
                    ObjAseguradora.Ins_address      = NodoResultado.Ins_address;
                    ObjAseguradora.Ins_phone        = NodoResultado.Ins_phone;
                    ObjAseguradora.Ins_fax          = NodoResultado.Ins_fax;
                    ObjAseguradora.Ins_zipCode      = NodoResultado.Ins_zipCode;
                    ObjAseguradora.Ins_scTpaCode    = NodoResultado.Ins_scTpaCode;
                    ObjAseguradora.Ins_feinSc       = NodoResultado.Ins_feinSc;
                    ObjAseguradora.Ins_carrierCode  = NodoResultado.Ins_carrierCode;
                    ObjAseguradora.Ins_feinCc       = NodoResultado.Ins_feinCc;
                    ObjAseguradora.Ins_city         = NodoResultado.Ins_city;
                    ObjAseguradora.Ins_state        = NodoResultado.Ins_state;
                    ObjAseguradora.Sta_abbreviation = NodoResultado.Sta_abbreviation;
                    ObjAseguradora.Ins_zipExt       = NodoResultado.Ins_zipExt;
                    ObjAseguradora.Ins_phoneExt     = NodoResultado.Ins_phoneExt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjAseguradora;
        }

        public List<SP_FIND_HOMONYM_INSURERS_Result> BuscarHomonimos(Insurer Aseguradora)
        {
            List<SP_FIND_HOMONYM_INSURERS_Result> Resultado = new List<SP_FIND_HOMONYM_INSURERS_Result>();
            ObjectResult<SP_FIND_HOMONYM_INSURERS_Result> ResultadoProcedimientoAlmacenadoDuro;
            ObjectParameter ParametroSalida = new ObjectParameter("ParametroSalida", typeof(bool)); // Variable que se crea para recibir el valor del parámetro de salida del procedimiento almacenado. Si encontró coincidencias las devuelve

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_FIND_HOMONYM_INSURERS(ParametroSalida
                                                                                   , Aseguradora.Ins_code
                                                                                   , Aseguradora.Ins_name
                                                                                   , Aseguradora.Ins_zipCode
                                                                                   , Aseguradora.Ins_city
                                                                                   , Aseguradora.Ins_state);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_FIND_HOMONYM_INSURERS_Result NodoResultado = new SP_FIND_HOMONYM_INSURERS_Result();

                    NodoResultado.Ins_code = RPAD.Ins_code;
                    NodoResultado.Ins_name =   RPAD.Ins_name;
                    NodoResultado.Ins_zipCode =     RPAD.Ins_zipCode;
                    NodoResultado.Ins_address =     RPAD.Ins_address;
                    NodoResultado.Ins_city =        RPAD.Ins_city;
                    NodoResultado.Ins_state =       RPAD.Ins_state;
                    NodoResultado.Ins_status = RPAD.Ins_status;
                     
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